using SVN.Core.Linq;
using SVN.Core.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVN.Web.Parser
{
    public class HtmlContentObject : IHtmlContent
    {
        public IHtmlContent Parent { get; set; }
        public List<IHtmlContent> Childs { get; } = new List<IHtmlContent>();
        public HtmlContainer Container { get; internal set; }
        public string Tag { get; internal set; }
        public List<HtmlAttribute> Attributes { get; internal set; }
        internal bool IsStart { get; set; }
        internal bool IsEnd { get; set; }

        internal HtmlContentObject()
        {
        }

        public IEnumerable<IHtmlContent> Content
        {
            get
            {
                foreach (var child in this.Childs)
                {
                    yield return child;

                    if (child is HtmlContentObject obj)
                    {
                        foreach (var content in obj.Content)
                        {
                            yield return content;
                        }
                    }
                }
            }
        }

        public string Format(Func<string, List<HtmlAttribute>, List<IHtmlContent>, string> formatter)
        {
            return formatter(this.Tag, this.Attributes, this.Childs);
        }

        public string FormatChilds()
        {
            return this.Format((tag, attributes, childs) => childs.Select(x => x.ToString()).Join(string.Empty));
        }

        public override string ToString()
        {
            return this.Format((tag, attributes, childs) => $"<{tag}{attributes.WhiteSpaceIfAny()}{attributes.Select(x => x.ToString()).Join(" ")}>");
        }
    }
}