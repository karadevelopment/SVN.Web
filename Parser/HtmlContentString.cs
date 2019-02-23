namespace SVN.Web.Parser
{
    public class HtmlContentString : HtmlContent
    {
        internal string Content { get; set; }

        internal HtmlContentString()
        {
        }

        public override string ToString()
        {
            return this.Content;
        }
    }
}