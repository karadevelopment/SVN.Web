using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace SVN.Web.Request
{
    public static class Ajax
    {
        public static string GetSourceCode(string uri)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static T Get<T>(string uri, Dictionary<string, string> headers)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                var json = JsonConvert.DeserializeObject<T>(jsonString);

                return json;
            }
        }

        public static T Post<T>(string uri, Dictionary<string, string> headers, object json)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            using (var stream = request.GetRequestStream())
            using (var writer = new StreamWriter(stream))
            {
                var jsonString = JsonConvert.SerializeObject(json);
                writer.Write(jsonString);
                writer.Flush();
            }

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
        }

        public static T PostOLD<T>(string uri, Dictionary<string, string> headers, Dictionary<string, string> values)
        {
            using (var client = new HttpClient())
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                using (var content = new FormUrlEncodedContent(values))
                {
                    using (var response = client.PostAsync(uri, content))
                    {
                        var task = response.Result.Content.ReadAsStringAsync();

                        var jsonString = task.Result;
                        var json = JsonConvert.DeserializeObject<T>(jsonString);

                        return json;
                    }
                }
            }
        }
    }
}