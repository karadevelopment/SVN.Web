using SVN.Core.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVN.Web.Parser
{
    public static class Extensions
    {
        public static List<HtmlContentObject> FilterByObject(this IEnumerable<IHtmlContent> param, string tag, Func<List<HtmlAttribute>, bool> attributePredicate)
        {
            return param
                .Where(x => x is HtmlContentObject)
                .Select(x => x as HtmlContentObject)
                .Where(x => tag is null || x.Tag == tag)
                .Where(x => attributePredicate(x.Attributes))
                .ToList();
        }

        public static List<HtmlContentObject> FilterByObject(this HtmlContainer param, string tag, Func<List<HtmlAttribute>, bool> attributePredicate)
        {
            return param.Content.FilterByObject(tag, attributePredicate);
        }

        public static List<HtmlContentObject> FilterByObject(this HtmlContentObject param, string tag, Func<List<HtmlAttribute>, bool> attributePredicate)
        {
            return param.Content.FilterByObject(tag, attributePredicate);
        }

        public static string GetAttributeValue(this HtmlContentObject param, string key)
        {
            return param.Attributes.GetAttributeValue(key);
        }

        public static string GetAttributeValue(this List<HtmlAttribute> param, string key)
        {
            return param.Where(x => x.Key == key).Select(x => x.Value).DefaultIfEmpty(string.Empty).First();
        }

        public static string GetContentValue(this HtmlContentObject param)
        {
            return param.Childs.Select(x => x.ToString()).Join(string.Empty);
        }
    }
}