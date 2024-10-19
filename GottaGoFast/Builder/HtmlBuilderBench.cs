using BenchmarkDotNet.Attributes;
using HtmxDotnet.BuilderViews;
using HtmxDotnet.utils;

namespace GottaGoFast.BuilderBench
{
    [MemoryDiagnoser]
    public class HtmlBuilderBench
    {
        // private readonly MultiTabFormViewModel model = new();
        private readonly HtmlBuilder hb = new();
        // private readonly MultiTabFormBV view = new();
        [WarmupCount(1)] // Number of warmup iterations
        [MinIterationCount(3)] // Minimum number of iterations
        [MaxIterationCount(5)] // Maximum number of iterations
        [Benchmark]
        public string BuilderBench()
        {
            hb
            .Open(HtmlTag.Div).AddAttributes(("article-type", "explination")).AddCssClasses("article")
                .Open(HtmlTag.P).AddAttributes(("Auther", "Will")).AddCssClasses("paragraph", "fancy-text", "document")
                    .Open(HtmlTag.Strong)
                        .AddText("Lorem Ipsum")
                    .Close()
                    .AddText("")
                    .Open(HtmlTag.Strong).AddCssClasses("cursive", "drop-cap")
                        .AddText("Where does it come from?")
                    .Close()
                    .AddText("")
                .Close()
            .Close();

            return hb.Build();
            // var html = view.RenderHtml(hb, model).Build();
            // return html;
        }
    }
}