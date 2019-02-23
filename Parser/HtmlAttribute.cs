namespace SVN.Web.Parser
{
    public class HtmlAttribute
    {
        public string Key { get; internal set; }
        public string Value { get; internal set; }

        internal HtmlAttribute()
        {
        }

        public override string ToString()
        {
            return $"{this.Key}{(!string.IsNullOrWhiteSpace(this.Value) ? "=\"" + this.Value + "\"" : string.Empty)}";
        }
    }
}