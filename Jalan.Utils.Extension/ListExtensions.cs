using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jalan.Utils.Extension
{
    public static class ListExtensions
    {
        /// <summary>
        /// Sort a list by a topological sorting, which consider their  dependencies
        /// </summary>
        /// <typeparam name="T">The type of the members of values.</typeparam>
        /// <param name="source">A list of objects to sort</param>
        /// <param name="getDependencies">Function to resolve the dependencies</param>
        /// <returns></returns>
        public static IList<T> SortByDependencies<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
        {
            /* See: http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp
             *      http://en.wikipedia.org/wiki/Topological_sorting
             */

            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>();

            foreach (var item in source)
            {
                SortByDependenciesVisit(item, getDependencies, sorted, visited);
            }

            return sorted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the members of values.</typeparam>
        /// <param name="item">Item to resolve</param>
        /// <param name="getDependencies">Function to resolve the dependencies</param>
        /// <param name="sorted">List with the sortet items</param>
        /// <param name="visited">Dictionary with the visited items</param>
        private static void SortByDependenciesVisit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found!");
                }
            }
            else
            {
                visited[item] = true;

                var dependencies = getDependencies(item);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        SortByDependenciesVisit(dependency, getDependencies, sorted, visited);
                    }
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }


        /// <summary>
        /// 对List<Dictionary<string,object>>指定key排序
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sort">排序列</param>
        /// <param name="dir">排序方式 asc / desc</param>
        public static void ListDictCompareTo<T>(this List<Dictionary<string, T>> data, string sort, string dir)
        {
            var isAsc = dir.ToLower() == "asc";
            data.Sort((x, y) =>
            {
                T value1 = default(T);
                T value2 = default(T);
                var xkeys = x.Keys.ToDictionary(k => k.ToLower(), v => v);
                var ykeys = y.Keys.ToDictionary(k => k.ToLower(), v => v);
                if (xkeys.ContainsKey(sort))
                    value1 = x[xkeys[sort]];
                if (ykeys.ContainsKey(sort))
                    value2 = y[ykeys[sort]];
                if (value1 == null || value1.ToString() == "")
                {
                    if (value2 == null || value2.ToString() == "")
                    {
                        return 0;
                    }
                    return isAsc ? -1 : 1;
                }
                if (value2 == null || value2.ToString() == "")
                {
                    return isAsc ? 1 : -1;
                }
                var sortIndex = value1.ToString().CompareTo(value2.ToString());
                return isAsc ? sortIndex : -sortIndex;
            });
        }
    }
}
