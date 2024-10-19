using System.ComponentModel.DataAnnotations;
using System.Text;
using HtmxDotnet.BuilderViews;

namespace HtmxDotnet.utils
{
    public class HtmlNode
    {
        public HtmlTag Tag { get; set; }
        public Dictionary<string, string> Attributes { get; } = [];
        public HashSet<string> CssClasses { get; } = [];
        public Dictionary<string, string> DataAttributes { get; } = [];
        public StringBuilder Text { get; } = new StringBuilder();
        public List<(HtmlNode Node, int Position)> Children { get; } = [];

        public HtmlNode(HtmlTag tag)
        {
            Tag = tag;
        }
    }


    public class HtmlBuilder
    {
        private readonly Stack<HtmlNode> _nodeStack = new Stack<HtmlNode>();
        private HtmlNode _rootNode;
        // Open a new tag and add it to the stack

        public HtmlBuilder(HtmlTag rootTag = HtmlTag.Div)
        {
            _rootNode = new HtmlNode(rootTag);
            _nodeStack.Push(_rootNode);
        }
        public HtmlBuilder Open(HtmlTag tag)
        {
            var newNode = new HtmlNode(tag);
            if (_nodeStack.Count > 0)
            {
                // Record the child and the position in the parent's text
                var parent = _nodeStack.Peek();
                parent.Children.Add((newNode, parent.Text.Length));
            }
            else
            {
                _rootNode = newNode;
            }
            _nodeStack.Push(newNode);
            return this;
        }

        // Close the current tag and pop the stack
        public HtmlBuilder Close(HtmlTag? tag = null)
        {
            if (_nodeStack.Count <= 1)
            {
                return this;
            }

            if (tag != null && _nodeStack.Peek().Tag != tag)
            {
                throw new Exception(
                $@"ERROR: Incorrect closing tag used. 
                       Received: [{tag.Value.ToTagName()}]
                       Wants: [{_nodeStack.Peek().Tag.ToTagName()}]

                    ");
            }

            _nodeStack.Pop();

            return this;
        }

        // Add arbitrary attributes
        public HtmlBuilder AddAttributes(params (string, string)[] attributes)
        {
            if (_nodeStack.Count > 0)
            {
                var curNodeAttrs = _nodeStack.Peek().Attributes;
                foreach (var (key, value) in attributes)
                {
                    if (key == "class")
                    {
                        AddCssClasses(value.Split(' ')); // Intercept CSS classes
                        continue;
                    }

                    curNodeAttrs[key] = value;
                }
            }
            return this;
        }

        // Add CSS classes
        public HtmlBuilder AddCssClasses(params string[] cssClasses)
        {
            if (_nodeStack.Count > 0)
            {
                var curNodeCssClasses = _nodeStack.Peek().CssClasses;
                foreach (var cssClass in cssClasses)
                {
                    curNodeCssClasses.Add(cssClass);
                }
            }
            return this;
        }

        // Set the ID attribute
        public HtmlBuilder WithId(string id)
        {
            return AddAttributes(("id", id));
        }

        // Add data-* attributes
        public HtmlBuilder AddDataAttributes(params (string, string)[] dataAttributes)
        {
            if (_nodeStack.Count <= 0)
            {
                return this;
            }

            var curNodeDataAttrs = _nodeStack.Peek().DataAttributes;

            foreach (var (key, value) in dataAttributes)
            {
                curNodeDataAttrs[$"data-{key}"] = value;
            }

            return this;
        }

        // Add text to the current node
        public HtmlBuilder AddText(string text)
        {
            if (_nodeStack.Count > 0)
            {
                _nodeStack.Peek().Text.Append(text.AsSpan());
            }
            return this;
        }

        public HtmlBuilder RenderBuilderView<VT>(IBuilderView<VT> bv, VT model)
        {
            bv.RenderHtml(this, model);
            return this;
        }

        // Build the final HTML string
        public string Build()
        {
            if (_rootNode == null)
            {
                throw new InvalidOperationException("No root node present. You must open at least one tag.");
            }

            var stringBuilder = new StringBuilder();
            BuildNode(_rootNode, stringBuilder);
            return stringBuilder.ToString();
        }

        // Recursively build the node and its children
        private void BuildNode(HtmlNode node, StringBuilder sb)
        {
            var nodeText = node.Text.ToString().AsSpan();
            var nodeTag = node.Tag.ToTagName().AsSpan();
            var nodeChildren = node.Children;
            var nodeAttrs = node.Attributes;
            var nodeCssClasses = node.CssClasses;
            var nodeDataAttrs = node.DataAttributes;

            // Open tag
            sb.Append('<').Append(nodeTag);

            // Add attributes
            foreach (var (key, value) in nodeAttrs)
            {
                sb.Append(' ').Append(key.AsSpan()).Append("=\"").Append(value.AsSpan()).Append('"');
            }

            // Add CSS classes if present
            if (nodeCssClasses.Count > 0)
            {
                sb.Append(" class=\"").Append(string.Join(' ', nodeCssClasses)).Append('"');
            }

            // Add data-* attributes
            foreach (var (key, value) in nodeDataAttrs)
            {
                sb.Append(' ').Append(key.AsSpan()).Append("=\"").Append(value.AsSpan()).Append('"');
            }

            sb.Append('>');

            // Add text content before the first child
            int lastPosition = 0;
            foreach (var (child, position) in nodeChildren)
            {
                sb.Append(nodeText[lastPosition..position]);
                BuildNode(child, sb);
                lastPosition = position;
            }

            // Add remaining text after the last child
            sb.Append(nodeText[lastPosition..]);

            // Close tag
            sb.Append("</").Append(nodeTag).Append('>');
        }
    }

    public static class HtmlTagEnumExtensions
    {
        private static Dictionary<HtmlTag, string> _tagNameFlyWeight = []; //Cache tag strings, save some memory!
        public static string ToTagName(this HtmlTag tag)
        {
            if (_tagNameFlyWeight.ContainsKey(tag))
            {
                return _tagNameFlyWeight[tag];
            }

            var res = tag.ToString().ToLower();
            _tagNameFlyWeight[tag] = res;

            return res;
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
        S,
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
        Menu,
        Menuitem,

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