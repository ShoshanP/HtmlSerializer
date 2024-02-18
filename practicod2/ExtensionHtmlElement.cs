using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicod2
{
    internal static class ExtensionHtmlElement
    {
        public static IEnumerable<T> Descendants<T>(this T tree, Func<T, IEnumerable<T>> getChildren)
        {
            Queue<T> queue = new Queue<T>();
            queue.Enqueue(tree);

            while (queue.Count > 0)
            {
                T currentElement = queue.Dequeue();
                yield return currentElement;


                // Enqueue children for further exploration
                foreach (var child in getChildren(currentElement))
                {
                    queue.Enqueue(child);
                }
            }

        }
    }
}
