using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CommonExtension
{
    public static class ICollectionExtension
    {
        public static ICollection<T> AddExtension<T>(this ICollection<T> collection, T item,bool candupicate=false)
        {
            if (collection.IsNotNull() && item.IsNotNull())
            {
                if (candupicate || !collection.Contains(item))
                {
                    collection.Add(item);
                }
            }
            return collection;
        }

        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> items, bool candupicate = false)
        {
            if (collection.IsNotNull() && items.IsNotNull())
            {
                items.ForEach(item =>
                {
                    if (candupicate || !collection.Contains(item))
                    {
                        collection.Add(item);
                    }
                });
            }
            return collection;
        }

        public static IEnumerable<T> Replace<T>(this ICollection<T> collection, T sourceItem, T targetItem)
        {
            if (collection.IsNotNull() && sourceItem.IsNotNull() && targetItem.IsNotNull())
            {
                var list = collection.ToList();

                var sourceIndex = list.IndexOf(sourceItem);
                var targetIndex = list.IndexOf(targetItem);
                if (sourceIndex > 0 && targetIndex > 0 && sourceIndex != targetIndex)
                {
                    list[targetIndex] = sourceItem;
                    list[sourceIndex] = targetItem;
                }

                collection.Clear();
                list.ForEach(item => collection.Add(item));
            }
            return collection;
        }

        public static ICollection<T> InsertAfter<T>(this ICollection<T> collection, T sourceItem, T targetItem)
        {
            if (collection.IsNotNull() && sourceItem.IsNotNull() && targetItem.IsNotNull())
            {
                if (collection.Contains(targetItem) && collection.Contains(sourceItem))
                {
                    var list = collection.ToList();
                    list.Remove(sourceItem);
                    var index = list.IndexOf(targetItem);
                    var targetindex = index + 1;
                    if (targetindex >= list.Count)
                    {
                        list.Add(sourceItem);
                    }
                    else
                    {
                        list.Insert(targetindex, sourceItem);
                    }

                    collection.Clear();
                    list.ForEach(item => collection.Add(item));
                }
            }
            return collection;
        }

        public static ICollection<T> InsertBefore<T>(this ICollection<T> collection, T sourceItem, T targetItem)
        {
            if (collection.IsNotNull() && sourceItem.IsNotNull() && targetItem.IsNotNull())
            {
                if (collection.Contains(targetItem) && collection.Contains(sourceItem))
                {
                    var list = collection.ToList();
                    list.Remove(sourceItem);
                    var index = list.IndexOf(targetItem);
                    var targetindex = index - 1;
                    if (targetindex < 0)
                    {
                        list.Insert(0,sourceItem);
                    }
                    else
                    {
                        list.Insert(targetindex, sourceItem);
                    }

                    collection.Clear();
                    list.ForEach(item => collection.Add(item));
                }
            }
            return collection;
        }
    }
}