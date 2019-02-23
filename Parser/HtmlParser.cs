using SVN.Core.Linq;
using SVN.Core.String;
using System.Collections.Generic;
using System.Linq;

namespace SVN.Web.Parser
{
    public static class HtmlParser
    {
        private static List<string> SingleTags { get; } = new List<string>
        {
            "!",
            "?",
            "meta",
            "link",
        };

        private static IEnumerable<(string key, string value)> ParseAttributes(string line)
        {
            while (!string.IsNullOrWhiteSpace(line))
            {
                var key = string.Empty;
                var value = string.Empty;

                var index = new List<int>
                {
                    line.IndexOf(' '),
                    line.IndexOf('='),
                    line.Length,
                }.Select(x => x != -1 ? x : int.MaxValue).Min();

                key = line.Copy(0, index).Trim();
                line = line.TrimStart(index);

                while (line.HasCharFirst(' '))
                {
                    line = line.TrimStart(1);
                }
                if (line.HasCharFirst('='))
                {
                    line = line.TrimStart(1);

                    if (line.HasCharFirst('"'))
                    {
                        line = line.TrimStart(1);
                        value = line.Copy(0, '"').Trim();
                        line = line.Remove(0, '"', 1);
                    }
                    else
                    {
                        index = new List<int>
                        {
                            line.IndexOf(" "),
                            line.Length,
                        }.Select(x => x != -1 ? x : int.MaxValue).Min();

                        value = line.Substring(0, index).Trim();
                        line = line.Remove(0, index);
                    }
                }
                while (line.HasCharFirst(' '))
                {
                    line = line.TrimStart(1);
                }
                if (!string.IsNullOrWhiteSpace(key))
                {
                    yield return (key, value);
                }
            }
        }

        private static IHtmlContent ParseLine(HtmlContainer container, string line)
        {
            if (line.Contains("<", ">"))
            {
                line = line.Trim(1);

                var isStart = true;
                var isEnd = false;

                while (line.HasCharFirst('/'))
                {
                    line = line.TrimStart(1);
                    isStart = false;
                    isEnd = true;
                }
                while (line.HasCharLast('/'))
                {
                    line = line.TrimEnd(1);
                    isEnd = true;
                }

                var attributes = HtmlParser.ParseAttributes(line).ToList();
                var tag = attributes.Select(x => x.key).DefaultIfEmpty(string.Empty).First();

                if (HtmlParser.SingleTags.Contains(tag.ToLower()))
                {
                    isStart = true;
                    isEnd = true;
                }

                return new HtmlContentObject
                {
                    Container = container,
                    Tag = tag,
                    Attributes = attributes.Skip(1).Select(x => new HtmlAttribute
                    {
                        Key = x.key,
                        Value = x.value,
                    }).ToList(),
                    IsStart = isStart,
                    IsEnd = isEnd,
                };
            }
            else
            {
                return new HtmlContentString
                {
                    Content = line,
                };
            }
        }

        private static IEnumerable<IHtmlContent> ParseAll(HtmlContainer container, string code)
        {
            foreach (var line in code
                .Remove("<script", "</script>")
                .Remove("<style", "</style>")
                .Remove("\n")
                .Replace("<", "\n<")
                .Replace(">", ">\n")
                .Split("\n")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim()))
            {
                yield return HtmlParser.ParseLine(container, line);
            }
        }

        public static HtmlContainer Parse(string code)
        {
            var container = new HtmlContainer();

            container.Content = HtmlParser.ParseAll(container, code).ToList();
            container.SetDepth();

            return container;
        }
    }
}