using SVN.Core.Linq;
using System.Collections.Generic;
using System.Linq;

namespace SVN.Web.Parser
{
    public class HtmlContainer
    {
        public List<IHtmlContent> Content { get; internal set; }

        public HtmlContainer()
        {
        }

        private IEnumerable<IHtmlContent> SetDepth(HtmlContentObject parent, List<IHtmlContent> content)
        {
            while (content.Any())
            {
                var html = content.First();
                content.Remove(html);

                if (html is HtmlContentObject obj && !(obj.IsStart && obj.IsEnd))
                {
                    if (parent != null && parent.IsStart && parent.Tag == obj.Tag && obj.IsEnd)
                    {
                        obj.Parent = parent.Parent;
                        yield break;
                    }
                    else
                    {
                        html.Parent = parent;
                        yield return html;

                        foreach (var child in this.SetDepth(obj, content))
                        {
                            obj.Childs.Add(child);
                        }
                    }
                }
                else
                {
                    html.Parent = parent;
                    yield return html;
                }
            }
        }

        internal void SetDepth()
        {
            this.SetDepth(null, new List<IHtmlContent>(this.Content)).ToList();
        }

        public override string ToString()
        {
            return this.Content.Select(x => x.ToString()).Join("\n");
        }
    }
}