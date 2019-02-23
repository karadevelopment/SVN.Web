using SVN.Core.Linq;
using System.Collections.Generic;
using System.Linq;

namespace SVN.Web.Parser
{
    public class HtmlContainer
    {
        internal List<HtmlContent> Content { get; set; }

        internal HtmlContainer()
        {
        }

        internal void SetDepth()
        {
            var parent = default(HtmlContentObject);
            var content = this.Content.ToList();

            while (content.Any())
            {
                var item = content.First();
                content.Remove(item);

                if (item is HtmlContentObject obj && !(obj.IsStart && obj.IsEnd))
                {
                    if (parent != null && parent.IsStart && parent.Tag == obj.Tag && obj.IsEnd)
                    {
                        parent = parent.Parent as HtmlContentObject;
                        parent?.AddChild(item);
                    }
                    else
                    {
                        parent?.AddChild(item);
                        parent = obj;
                    }
                }
                else
                {
                    parent?.AddChild(item);
                }
            }
        }

        public override string ToString()
        {
            return this.Content.Select(x => x.ToString()).Join("\n");
        }
    }
}