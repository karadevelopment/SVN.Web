namespace SVN.Web.Parser
{
    public class HtmlContentString : IHtmlContent
    {
        public IHtmlContent Parent { get; set; }
        public string Content { get; internal set; }

        internal HtmlContentString()
        {
        }

        public override string ToString()
        {
            return this.Content;
        }
    }
}