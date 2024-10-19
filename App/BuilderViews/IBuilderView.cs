using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmxDotnet.utils;

namespace HtmxDotnet.BuilderViews
{
    public interface IBuilderView<T>
    {
        public HtmlBuilder RenderHtml(HtmlBuilder builder, T viewModel);
    }
}