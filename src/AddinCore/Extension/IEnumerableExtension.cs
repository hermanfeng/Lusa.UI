using AddinEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonExtension
{
    public static class IEnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable.IsNotNull() && action.IsNotNull())
            {
                foreach (var element in enumerable)
                {
                    action(element);
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T,T> action)
        {
            if (enumerable.IsNotNull() && action.IsNotNull())
            {
                T lastElement = default(T);
                foreach (var element in enumerable)
                {
                    action(element,lastElement);
                    lastElement = element;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T,int> action)
        {
            if (enumerable.IsNotNull() && action.IsNotNull())
            {
                var index = 0;
                foreach (var element in enumerable)
                {
                    action(element, index);
                    index += 1;
                }
            }
        }

        public static IEnumerable<T> ReorderElements<T>(this IEnumerable<T> enumerable) where T : class 
        {
            var order = new ElementReorder<T>();
            return order.Reorder(enumerable); ;
        }

        private static void AdjustContentIndex<T>(List<T> orignallist, List<T> resultlist, int orginalindex, int targetindex)
        {
            if (orginalindex != targetindex && orginalindex > -1 && targetindex > -1 && orginalindex < orignallist.Count && targetindex < orignallist.Count)
            {
                var orignalitem = orignallist[orginalindex];
                resultlist[targetindex] = orignalitem;
            }
        }

    }
}