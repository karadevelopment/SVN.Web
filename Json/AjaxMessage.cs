namespace SVN.Web.Json
{
    public class AjaxMessage
    {
        internal AjaxMessageType Type { get; private set; }
        internal string Content { get; private set; }

        private AjaxMessage()
        {
        }

        private static AjaxMessage Create(AjaxMessageType type, string content)
        {
            return new AjaxMessage
            {
                Type = type,
                Content = content
            };
        }

        public static AjaxMessage CreateSuccess(string content)
        {
            return AjaxMessage.Create(AjaxMessageType.Success, content);
        }

        public static AjaxMessage CreateDanger(string content)
        {
            return AjaxMessage.Create(AjaxMessageType.Danger, content);
        }

        public static AjaxMessage CreateWarning(string content)
        {
            return AjaxMessage.Create(AjaxMessageType.Warning, content);
        }

        public static AjaxMessage CreateInfo(string content)
        {
            return AjaxMessage.Create(AjaxMessageType.Info, content);
        }
    }
}