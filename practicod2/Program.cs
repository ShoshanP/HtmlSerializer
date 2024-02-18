using practicod2;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

static HtmlElement Serializer(List<string> htmlTags)
{
    HtmlElement root = new HtmlElement();//איבר השורש
    HtmlElement current = root;//האיבר הנוכחי
    foreach (string tag in htmlTags)
    {
        int index = tag.IndexOf(" ");//היכן מסתיימת המילה הראשונה
        if (index == -1)//אם יש רק מילה אחת
            index = tag.Length;
        var firstWord = tag.Substring(0, index);//המילה הראשונה...
        if (firstWord == "/html")
            break;
        if (firstWord.StartsWith("/"))//אם יש תוית נסגרת
            current = current.Parent != null ? current.Parent : root;
        else if (HtmlHelper.Instance.AllTags.Contains(firstWord))//אם זוהי פתיחת תווית
        {
            HtmlElement newElement = new HtmlElement() { Name = firstWord, Parent = current };
            var attributes = new Regex("(.*?)=\"(.*?)\"").Matches(tag.Replace(firstWord, ""));
            foreach (var attribute in attributes)
            {
                string fixAttribute = attribute.ToString().TrimStart();
                if (fixAttribute.StartsWith("id"))
                    newElement.Id = fixAttribute.Replace("id=", "").Replace("\"", "");
                else if (fixAttribute.StartsWith("class"))
                {
                    string[] classes = fixAttribute.Replace("class=", "").Replace("\"", "").Split(' ');
                    foreach (var className in classes)
                    {
                        if (className != "class" && className != "=")
                        {
                            newElement.Classes.Add(className);
                        }
                    }
                }
                else newElement.Attributes.Add(fixAttribute);
            }
            current.Children.Add(newElement);
            current = newElement;
            if (firstWord.EndsWith("/") || HtmlHelper.Instance.SelfClosingTags.Contains(firstWord))
            {
                current = newElement;
            }
        }
        else current.InnerHtml = tag;
    }
    return root;
}

static void PrintTree(HtmlElement element, int depth)
{
    Console.WriteLine(new string(' ', depth * 2) + element.Name);

    foreach (var child in element.Children)
    {
        PrintTree(child, depth + 1);
    }
}




var html = await Load("https://hebrewbooks.org/advanced");
var html1 = "<div id=\"first1\" onclick=\"aaa()\" class=\"allContent class2\">hellow</div><div><h1 id=\"first1\"><p class=\"allContent class1\">hello1</p></h1></div>";

var cleanHtml = new Regex("\\s").Replace(html1, " ");
cleanHtml = Regex.Replace(cleanHtml, @"\s+", " ");

var htmlTags = new Regex("<(.*?)>").Split(cleanHtml).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

HtmlElement tree = Serializer(htmlTags.ToList());
PrintTree(tree, 0);

var descendants = tree.Descendants();
foreach (var descendant in descendants)
{
    Console.WriteLine(descendant);
}

Selector selector1 = Selector.ParseToSelctor(".allContent");
Selector selector2 = Selector.ParseToSelctor("#first1 .allContent");
Selector selector3 = Selector.ParseToSelctor("div #first1 .allContent");
Selector selector4 = Selector.ParseToSelctor("div#first1.allContent");
Selector selector5 = Selector.ParseToSelctor("#first1");
Selector selector6 = Selector.ParseToSelctor("div");
Selector selector7 = Selector.ParseToSelctor("#Table1");
Selector selector8 = Selector.ParseToSelctor("div#Table1");


var resultSelector1 = HtmlElement.Search(tree,selector1,new List<HtmlElement>());
var resultSelector2 = HtmlElement.Search(tree,selector2,new List<HtmlElement>());
var resultSelector3 = HtmlElement.Search(tree,selector3,new List<HtmlElement>());
var resultSelector4 = HtmlElement.Search(tree,selector4,new List<HtmlElement>());
var resultSelector5 = HtmlElement.Search(tree,selector5,new List<HtmlElement>());
var resultSelector6 = HtmlElement.Search(tree,selector6,new List<HtmlElement>());
var resultSelector7 = HtmlElement.Search(tree,selector7,new List<HtmlElement>());
var resultSelector8 = HtmlElement.Search(tree,selector8,new List<HtmlElement>());




Console.ReadLine();
