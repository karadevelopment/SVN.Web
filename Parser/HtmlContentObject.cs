using SVN.Core.Linq;
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

        public override string ToString()
        {
            return $"<{this.Tag}{(this.Attributes.Any() ? " " : string.Empty)}{this.Attributes.Select(x => x.ToString()).Join(" ")}>";
        }
    }
}