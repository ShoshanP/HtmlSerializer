using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace practicod2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance { get { return _instance; } }
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }

        private HtmlHelper()
        {
            var AllTagsStr = File.ReadAllText("AllTags.json");
            var SelfClosingTagsStr = File.ReadAllText("SelfClosingTags.json");
            AllTags = JsonSerializer.Deserialize<string[]>(AllTagsStr);
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(SelfClosingTagsStr);
        }
    }
}
