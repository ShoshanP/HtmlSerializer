using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practicod2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector ParseToSelctor(string selctor)
        {
            string[] selectors = selctor.Split(" ");
            Selector selector = new Selector();
            Selector current = selector;
            foreach (string s in selectors)
            {
                string[] props = s.Split(new char[] {'.','#' });

                List<string> cleanWordsList = new List<string>(props);
                cleanWordsList.RemoveAll(string.IsNullOrEmpty);
                props = cleanWordsList.ToArray();

                foreach (string prop in props)
                {
                    if (s.Contains($"#{prop}"))
                        current.Id = prop;
                    else if (s.Contains($".{prop}"))
                    {
                        current.Classes= new List<string>(prop.Split(' ').ToList());
                    }
                    else if (HtmlHelper.Instance.AllTags.Contains(prop))
                    {
                        current.TagName = prop;
                    }
                }
                current.Child=new Selector() { Parent=current};
                current=current.Child;
            }
            return selector;
        }
    }
}
