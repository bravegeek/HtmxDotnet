using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmxDotnet.Models
{
    public class HtmxConfig
    {
        public string HxAction { get; set; } = ""; // Determines whether it's get, post, delete, or put
        public string HxUrl { get; set; } = "";    // The URL to be used with the action
        public string HxTrigger { get; set; } = "";
        public string HxTarget { get; set; } = "";
        public string HxSwap { get; set; } = "";
        public string HxPushUrl { get; set; } = "";

        public override string ToString()
        {
            var attributes = new List<string>();

            if (!string.IsNullOrEmpty(HxAction) && !string.IsNullOrEmpty(HxUrl))
            {
                attributes.Add($"{HxAction}=\"{HxUrl}\"");
            }

            if (!string.IsNullOrEmpty(HxTrigger))
                attributes.Add($"hx-trigger=\"{HxTrigger}\"");

            if (!string.IsNullOrEmpty(HxTarget))
                attributes.Add($"hx-target=\"#{HxTarget}\"");

            if (!string.IsNullOrEmpty(HxSwap))
                attributes.Add($"hx-swap=\"{HxSwap}\"");

            if (!string.IsNullOrEmpty(HxPushUrl))
                attributes.Add($"hx-push-url=\"{HxPushUrl}\"");

            return string.Join(" ", attributes);
        }

        public (string, string)[] ToTupleArray()
        {
            var attributes = new List<(string, string)>();

            if (!string.IsNullOrEmpty(HxAction) && !string.IsNullOrEmpty(HxUrl))
                attributes.Add((HxAction, HxUrl));

            if (!string.IsNullOrEmpty(HxTrigger))
                attributes.Add(("hx-trigger", HxTrigger));

            if (!string.IsNullOrEmpty(HxTarget))
                attributes.Add(("hx-target", '#' + HxTarget));

            if (!string.IsNullOrEmpty(HxSwap))
                attributes.Add(("hx-swap", HxSwap));

            if (!string.IsNullOrEmpty(HxPushUrl))
                attributes.Add(("hx-push-url", HxPushUrl));

            return [.. attributes];
        }
    }
}