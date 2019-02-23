using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SVN.Web.Json
{
    public static class JsonResponse
    {
        private static JsonSerializerSettings Settings
        {
            get => new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            };
        }

        public static ContentResult Create()
        {
            return JsonResponse.Create(true);
        }

        public static ContentResult Create(bool success)
        {
            return JsonResponse.Create(success, null);
        }

        public static ContentResult Create(bool success, IEnumerable<AjaxMessage> messages)
        {
            return JsonResponse.Create(success, null, messages);
        }

        public static ContentResult Create(object data, IEnumerable<AjaxMessage> messages)
        {
            return JsonResponse.Create(true, data, messages);
        }

        public static ContentResult Create(object data)
        {
            return JsonResponse.Create(true, data, null);
        }

        public static ContentResult Create(bool success, object data, IEnumerable<AjaxMessage> messages)
        {
            var content = new
            {
                success,
                data,
                messages = messages?.Select(x => new
                {
                    type = x.Type.ToString().ToLower(),
                    content = x.Content,
                }),
            };
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(content),
                ContentType = "application/json",
            };
        }

        public static ContentResult Redirect(string url)
        {
            var data = new
            {
                url,
            };
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(data),
                ContentType = "application/json",
            };
        }
    }
}