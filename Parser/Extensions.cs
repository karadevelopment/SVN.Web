using SVN.Core.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVN.Web.Parser
{
    public static class Extensions
    {
        public static List<HtmlContentObject> FilterByObject(this HtmlContainer param, string tag, Func<IEnumerable<HtmlAttribute>, bool> attributePredicate)
        {
            return param.Content.FilterByObject(tag, attributePredicate);
        }

        public static List<HtmlContentObject> FilterByObject(this HtmlContentObject param, string tag, Func<IEnumerable<HtmlAttribute>, bool> attributePredicate)
        {
            return param.Content.FilterByObject(tag, attributePredicate);
        }

        public static List<HtmlContentObject> FilterByObject(this IEnumerable<HtmlContent> param, string tag, Func<IEnumerable<HtmlAttribute>, bool> attributePredicate)
        {
            return param
                .Where(x => x is HtmlContentObject)
                .Select(x => x as HtmlContentObject)
                .Where(x => tag is null || x.Tag == tag)
                .Where(x => attributePredicate(x.Attributes))
                .ToList();
        }

        public static string GetAttributeValue(this HtmlContentObject param, string key)
        {
            return param.Attributes.GetAttributeValue(key);
        }

        public static string GetAttributeValue(this IEnumerable<HtmlAttribute> param, string key)
        {
            return param.Where(x => x.Key == key).Select(x => x.Value).DefaultIfEmpty(string.Empty).First();
        }

        public static string GetContentValue(this HtmlContentObject param)
        {
            return param.Content.GetContentValue();
        }

        public static HtmlContentObject GetContent(this HtmlContentObject param, string tag, Func<IEnumerable<HtmlAttribute>, bool> attributePredicate)
        {
            return param.Content.GetContent(tag, attributePredicate);
        }

        public static HtmlContentObject GetContent(this IEnumerable<HtmlContent> param, string tag, Func<IEnumerable<HtmlAttribute>, bool> attributePredicate)
        {
            return param
                .Where(x => x is HtmlContentObject)
                .Select(x => x as HtmlContentObject)
                .Where(x => tag is null || x.Tag == tag)
                .Where(x => attributePredicate(x.Attributes))
                .DefaultIfEmpty(new HtmlContentObject())
                .First();
        }

        public static string GetContentValue(this IEnumerable<HtmlContent> param)
        {
            var result = string.Empty;

            foreach (var item in param.Where(x => x is HtmlContentString))
            {
                result += item.ToString();
            }

            return result;
        }

        public static string GetContentValue(this HtmlContentObject param, string tag, Func<IEnumerable<HtmlAttribute>, bool> attributePredicate)
        {
            return param.Content.GetContentValue(tag, attributePredicate);
        }

        public static string GetContentValue(this IEnumerable<HtmlContent> param, string tag, Func<IEnumerable<HtmlAttribute>, bool> attributePredicate)
        {
            return param.FilterByObject(tag, attributePredicate).Select(x => x.GetContentValue()).Join(string.Empty);
        }
    }
}