using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicod2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }

        public List<HtmlElement> Children { get; set; }
        public HtmlElement Parent { get; set; }
        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while (q.Any())
            {
                HtmlElement current = q.Dequeue();
                yield return current;
                foreach (HtmlElement child in current.Children)
                {
                    q.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while (current.Parent != null)
            {
                yield return current;
            }
        }
        public static List<HtmlElement> Search(HtmlElement element, Selector selector, List<HtmlElement> s)
        {
            if (selector == null)
            {
                return null;
            }
            IEnumerable<HtmlElement> descendants = element.Descendants();
            foreach (HtmlElement d in descendants)
            {
                if (selector.TagName != null && selector.TagName != "")
                {
                    if (selector.TagName != d.Name)
                        continue;

                }
                if (selector.Id != null)
                {
                    if (selector.Id != d.Id)
                        continue;

                }
                if (selector.Classes != null)
                {
                    if (d.Classes != null)
                    {
                        if (!selector.Classes.All(c => d.Classes.Contains(c)))
                            continue;
                    }
                    else
                        continue;

                }

                if (selector.Child == null)
                {
                    s.Add(d);
                }
                else
                {
                    Search(d, selector.Child, s);
                }
            }
            var re = new HashSet<HtmlElement>(s);
            return re.ToList();
        }

    }
}
    







