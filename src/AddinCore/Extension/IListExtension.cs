using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonExtension
{
    public static class IListExtension
    {
        public static IEnumerable<T> Replace<T>(this IList<T> list, T sourceItem, T targetItem)
        {
            if (list.IsNotNull() && sourceItem.IsNotNull() && targetItem.IsNotNull())
            {
                var sourceIndex  = list.IndexOf(sourceItem);
                var targetIndex = list.IndexOf(targetItem);
                if (sourceIndex > 0 && targetIndex > 0 && sourceIndex != targetIndex)
                {
                    list[targetIndex] = sourceItem;
                    list[sourceIndex] = targetItem;
                }
            }
            return list;
        }
    }
}