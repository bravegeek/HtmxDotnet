using System.Collections.Concurrent;
using System.Text;
using HtmxDotnet.BuilderViews;
using Microsoft.Extensions.ObjectPool;

namespace HtmxDotnet.utils
{
    internal class HtmlStringBuilderPolicy : PooledObjectPolicy<StringBuilder>
    {
        private readonly int _initialCap;
        private readonly int _maxCap;
        public HtmlStringBuilderPolicy(int initialCap, int maxCap)
        {
            this._maxCap = maxCap;
            this._initialCap = initialCap;
        }

        public override StringBuilder Create()
        {
            return new StringBuilder(_initialCap);
        }

        public override bool Return(StringBuilder sb)
        {
            if (sb.Capacity > _maxCap)
            {
                return false;
            }

            sb.Clear();

            return true;
        }
    }

    internal class HtmlNode
    {
        public HtmlTag Tag { get; set; }
        public bool IsRoot { get; set; } = false;
        public Dictionary<string, string>? Attributes { get; set; }
        public HashSet<string>? CssClasses { get; set; }
        public StringBuilder? Text { get; set; }
        public int CurrentTextContentIndex { get; set; } = 0;
        public List<(HtmlNode Node, int Position)>? Children { get; set; }

        public HtmlNode(HtmlTag tag)
        {
            Tag = tag;
        }
    }


    public class HtmlBuilder
    {
        private readonly Stack<HtmlNode> _nodeStack = new();
        private readonly HtmlNode _rootNode;
        private static readonly DefaultObjectPool<StringBuilder> _htmlSbPool = new DefaultObjectPool<StringBuilder>(new HtmlStringBuilderPolicy(64, 1024), 100);
        private bool _isBuilt = false;

        public HtmlBuilder(HtmlTag rootTag = HtmlTag.Div)
        {
            _rootNode = new HtmlNode(rootTag)
            {
                IsRoot = true
            };
            _nodeStack.Push(_rootNode);
        }

        private void EnsureNotBuilt()
        {
            if (_isBuilt)
            {
                throw new InvalidOperationException("The builder cannot be used after Build has been called.");
            }
        }

        public HtmlBuilder Open(HtmlTag tag)
        {
            EnsureNotBuilt();
            var newNode = new HtmlNode(tag);
            var parent = _nodeStack.Peek();
            getNodeChildren(parent).Add((newNode, parent.CurrentTextContentIndex));
            _nodeStack.Push(newNode);

            return this;
        }

        // Close the current tag and pop the stack
        public HtmlBuilder Close(HtmlTag tag)
        {
            EnsureNotBuilt();
            if (_nodeStack.Count <= 1)
            {
                return this;
            }

            if (_nodeStack.Peek().Tag != tag)
            {
                throw new Exception(
                $@"ERROR: Incorrect closing tag used. 
                       Received: [{tag.ToTagName()}]
                       Wants: [{_nodeStack.Peek().Tag.ToTagName()}]

                    ");
            }

            _nodeStack.Pop();

            return this;
        }

        public HtmlBuilder Close()
        {
            EnsureNotBuilt();

            if (_nodeStack.Count <= 1)
            {
                return this;
            }

            _nodeStack.Pop();

            return this;
        }

        // Add arbitrary attributes
        public HtmlBuilder AddAttributes(params (string name, string value)[] attributes)
        {
            EnsureNotBuilt();
            var curNodeAttrs = getNodeAttributes(_nodeStack.Peek());
            foreach (var (name, value) in attributes)
            {
                if (name == "class" && !string.IsNullOrEmpty(value))
                {
                    AddCssClasses(value.Split(' ')); // Intercept CSS classes
                    continue;
                }

                curNodeAttrs[name] = value;
            }

            return this;
        }

        // Add CSS classes
        public HtmlBuilder AddCssClasses(params string[] cssClasses)
        {
            EnsureNotBuilt();

            var curNodeCssClasses = getNodeCssClasses(_nodeStack.Peek());
            foreach (var cssClass in cssClasses)
            {
                curNodeCssClasses.Add(cssClass);
            }

            return this;
        }

        // Set the ID attribute
        public HtmlBuilder WithId(string id)
        {
            EnsureNotBuilt();

            getNodeAttributes(_nodeStack.Peek()).Add("id", id);

            return this;
        }

        // Add data-* attributes
        public HtmlBuilder AddDataAttributes(params (string, string)[] dataAttributes)
        {
            EnsureNotBuilt();

            var curNodeDataAttrs = getNodeAttributes(_nodeStack.Peek());

            foreach (var (key, value) in dataAttributes)
            {
                curNodeDataAttrs[$"data-{key}"] = value;
            }

            return this;
        }

        // Add text to the current node
        public HtmlBuilder AddText(string text)
        {
            EnsureNotBuilt();
            var node = _nodeStack.Peek();
            var nodeText = getNodeText(node);
            nodeText.Append(text);
            node.CurrentTextContentIndex = nodeText.Length;
            return this;
        }

        public HtmlBuilder SanitizeAndAddText(string text)
        {
            EnsureNotBuilt();
            AddText(SanitizeText(text));
            return this;
        }

        public HtmlBuilder RenderBuilderView<VT>(IBuilderView<VT> bv, VT model)
        {
            EnsureNotBuilt();
            bv.RenderHtml(this, model);
            return this;
        }

        // Build the final HTML string
        public string Build()
        {
            EnsureNotBuilt();
            var stringBuilder = _htmlSbPool.Get();
            BuildNode(_rootNode, stringBuilder);

            var text = stringBuilder.ToString();
            release(_rootNode);
            //_htmlSbPool.Return(stringBuilder);

            return text;
        }

        private void release(HtmlNode node)
        {
            if (node.Children != null && node.Children.Count > 0)
            {
                node.Children.ForEach(c =>
                {
                    release(c.Node);
                });
            }

            if (node.Text != null)
            {
                _htmlSbPool.Return(node.Text);
            }
        }

        private StringBuilder getNodeText(HtmlNode node)
        {
            if (node.Text != null)
            {
                return node.Text;
            }

            var sb = _htmlSbPool.Get();
            node.Text = sb;

            return sb;
        }

        private Dictionary<string, string> getNodeAttributes(HtmlNode node)
        {
            if (node.Attributes != null)
            {
                return node.Attributes;
            }

            var dic = new Dictionary<string, string>();
            node.Attributes = dic;

            return dic;
        }

        private HashSet<string> getNodeCssClasses(HtmlNode node)
        {
            if (node.CssClasses != null)
            {
                return node.CssClasses;
            }

            var hs = new HashSet<string>();
            node.CssClasses = hs;

            return hs;
        }

        private List<(HtmlNode Node, int Postion)> getNodeChildren(HtmlNode node)
        {
            if (node.Children != null)
            {
                return node.Children;
            }

            var children = new List<(HtmlNode Node, int Position)>();
            node.Children = children;

            return children;
        }

        // Recursively build the node and its children
        private static void BuildNode(HtmlNode node, StringBuilder sb)
        {
            var nodeText = node.Text?.GetChunks();
            var nodeTag = node.Tag;
            var nodeTagText = nodeTag.ToTagName();
            var nodeChildren = node.Children;
            var nodeAttrs = node.Attributes;
            var nodeCssClasses = node.CssClasses;

            // Open tag
            sb.Append('<').Append(nodeTagText);

            // Add CSS classes if present
            if (nodeCssClasses != null && nodeCssClasses.Count > 0)
            {
                sb.Append(" class=\"");

                foreach (var cssClass in nodeCssClasses)
                {
                    sb.Append(cssClass).Append(' ');
                }

                sb.Append('"');
            }

            // Add attributes
            if (nodeAttrs != null)
            {
                foreach (var (key, value) in nodeAttrs)
                {
                    sb.Append(' ').Append(key).Append("=\"").Append(value).Append('"');
                }
            }

            sb.Append('>');

            // Add text content before the first child

            int lastPosition = 0;

            if (nodeChildren != null)
            {
                foreach (var (child, position) in nodeChildren)
                {
                    if (nodeText != null)
                    {
                        foreach (var chunk in nodeText)
                        {
                            var tSpan = chunk.Span;
                            sb.Append(tSpan[lastPosition..position]);
                        }
                    }

                    BuildNode(child, sb);
                    lastPosition = position;
                }
            }

            // Add remaining text after the last child
            if (nodeText != null)
            {
                foreach (var chunk in nodeText)
                {
                    var tSpan = chunk.Span;
                    sb.Append(tSpan[lastPosition..]);
                }
            }

            // Close tag
            if (!nodeTag.IsSelfClosing())
            {
                sb.Append("</").Append(nodeTagText).Append('>');
            }
        }

        public static string SanitizeText(ReadOnlySpan<char> text)
        {
            var sb = _htmlSbPool.Get();

            for (var i = 0; i < text.Length; i++)
            {
                char c = text[i];
                int ascii = c;

                if ((ascii & ~0x7F) != 0) // Check if the character is non-ASCII
                {
                    sb.Append(c);
                    continue;
                }

                switch (ascii)
                {
                    case '<': // ASCII 0x3C
                        sb.Append("&lt;");
                        break;
                    case '>': // ASCII 0x3E
                        sb.Append("&gt;");
                        break;
                    case '&': // ASCII 0x26
                        sb.Append("&amp;");
                        break;
                    case '"': // ASCII 0x22
                        sb.Append("&quot;");
                        break;
                    case '\'': // ASCII 0x27
                        sb.Append("&#39;");
                        break;
                    case '/': // ASCII 0x2F
                        sb.Append("&#47;");
                        break;
                    case '\\': // ASCII 0x5C
                        sb.Append("\\\\");
                        break;
                    case '\n': // ASCII 0x0A
                        sb.Append("\\n");
                        break;
                    case '\r': // ASCII 0x0D
                        sb.Append("\\r");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            var sanitizedText = sb.ToString();

            _htmlSbPool.Return(sb);
            return sanitizedText;
        }
    }

    public static class HtmlTagEnumExtensions
    {
        private static readonly HtmlTag[] _selfClosingTags = [
            HtmlTag.Meta,
            HtmlTag.Link,
            HtmlTag.Base,
            HtmlTag.Br,
            HtmlTag.Wbr,
            HtmlTag.Img,
            HtmlTag.Embed,
            HtmlTag.Param,
            HtmlTag.Source,
            HtmlTag.Track,
            HtmlTag.Area,
            HtmlTag.Col,
            HtmlTag.Hr
        ];
        private static ConcurrentDictionary<HtmlTag, string> _tagNameFlyWeight = []; //Cache tag strings, save some memory!

        public static string ToTagName(this HtmlTag tag)
        {
            return _tagNameFlyWeight.GetOrAdd(tag, tag => tag.ToString().ToLower());
        }

        public static bool IsSelfClosing(this HtmlTag tag)
        {
            for (var i = 0; i < _selfClosingTags.Length; i++)
            {
                if (_selfClosingTags[i] == tag)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public enum HtmlTag
    {
        // Document metadata
        Html,
        Head,
        Title,
        Meta,
        Link,
        Style,
        Script,
        Base,

        // Sectioning root
        Body,

        // Content sectioning
        Header,
        Footer,
        Section,
        Nav,
        Article,
        Aside,
        H1,
        H2,
        H3,
        H4,
        H5,
        H6,

        // Text content
        P,
        Blockquote,
        Div,
        Span,
        A,
        Em,
        Strong,
        Small,
        Cite,
        Q,
        Dfn,
        Abbr,
        Time,
        Code,
        Var,
        Samp,
        Kbd,
        Sub,
        Sup,
        I,
        B,
        U,
        Mark,
        Ruby,
        Rt,
        Rp,
        Bdi,
        Bdo,
        Br,
        Wbr,

        // Edits
        Ins,
        Del,

        // Embedded content
        Img,
        Iframe,
        Embed,
        Object,
        Param,
        Video,
        Audio,
        Source,
        Track,
        Canvas,
        Map,
        Area,
        Svg,
        Math,

        // Tabular data
        Table,
        Caption,
        Colgroup,
        Col,
        Tbody,
        Thead,
        Tfoot,
        Tr,
        Td,
        Th,

        // Forms
        Form,
        Fieldset,
        Legend,
        Label,
        Input,
        Button,
        Select,
        Datalist,
        Optgroup,
        Option,
        Textarea,
        Output,
        Progress,
        Meter,

        // Interactive elements
        Details,
        Summary,
        Dialog,

        // Lists
        Ul,
        Ol,
        Li,
        Dl,
        Dt,
        Dd,

        // Miscellaneous
        Figure,
        Figcaption,
        Hr,
        Pre,
        Main,
        Template,
        Noscript
    }
}