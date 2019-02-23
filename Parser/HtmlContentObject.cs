using SVN.Core.Linq;
using SVN.Core.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVN.Web.Parser
{
    public class HtmlContentObject : HtmlContent
    {
        internal List<HtmlContent> Childs { get; } = new List<HtmlContent>();
        internal HtmlContainer Container { get; set; }
        internal string Tag { get; set; }
        internal List<HtmlAttribute> Attributes { get; set; }
        internal bool IsStart { get; set; }
        internal bool IsEnd { get; set; }

        internal HtmlContentObject()
        {
        }

        internal IEnumerable<HtmlContent> Content
        {
            get
            {
                foreach (var child in this.Childs)
                {
                    yield return child;

                    if (child is HtmlContentObject obj)
                    {
                        foreach (var content in obj.Content.ToList())
                        {
                            yield return content;
                        }
                    }
                }
            }
        }

        internal void AddChild(HtmlContent child)
        {
            child.Parent = this;
            this.Childs.Add(child);
        }

        public bool HasAttribute(string key, string value = null)
        {
            return this.Attributes.Any(x => x.Key == key && (value is null || x.Value.Contains(value)));
        }

        public bool HasChilds()
        {
            return this.Childs.Any();
        }

        public string Format(Func<string, List<HtmlAttribute>, List<HtmlContent>, string> formatter)
        {
            return formatter(this.Tag, this.Attributes, this.Childs);
        }

        public override string ToString()
        {
            return this.Format((tag, attributes, childs) => $"<{(this.IsEnd ? "/" : "")}{tag}{attributes.WhiteSpaceIfAny()}{attributes.Select(x => x.ToString()).Join(" ")}>");
        }
    }
}