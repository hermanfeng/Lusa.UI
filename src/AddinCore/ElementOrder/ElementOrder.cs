using System;
using System.Collections.Generic;
using System.Linq;
using Lusa.AddinEngine.Extension;

namespace Lusa.AddinEngine.ElementOrder
{
    public class ElementReorder<T> where T : class 
    {
        public ElementReorder()
        {
            UseAttribute = true;
            UseOrderProvider = true;
        }

        public bool UseAttribute
        {
            get;
            set;
        }

        public bool UseOrderProvider
        {
            get;
            set;
        }

        public IEnumerable<T> Reorder(IEnumerable<T> elements)
        {
            if (elements.IsNull())
            {
                return new List<T>();
            }

            IEnumerable<T> orderedElements = elements;
            if (UseAttribute)
            {
                orderedElements = ReorderElementsByOrderAttribute(elements.ToList());
            }
            if (UseOrderProvider)
            {
                orderedElements = ReorderElementsByOrderProvider(orderedElements);
            }
            return orderedElements;
        }

        private IEnumerable<T>  ReorderElementsByOrderAttribute(IEnumerable<T> elements)
        {
            var orderedElements = elements.Select(element =>
            {
                var attr = element.GetType().GetAttribute<ElementOrderAttribute>();
                return new {Key = element, Value = attr.IsNotNull() ? attr.Order : 100};
            }).OrderBy(item => item.Value).Select(item => item.Key);
            return orderedElements;
        }

        private IEnumerable<T> ReorderElementsByOrderProvider(IEnumerable<T> elements)
        {
            var orderinfos = GetAllElementsOrderProvider(elements);

            var originalElements = elements.ToList();
            var elementList = elements.ToList();

            orderinfos.ForEach(orderinfo =>
            {
                if (orderinfo.Value.IsNotNull())
                {
                    var oinfo = orderinfo.Value;
                    var afterElements = oinfo.AfterElements(originalElements) ?? new List<T>();
                    var beforeElements = oinfo.BeforeElements(originalElements) ?? new List<T>();
                    var step = orderinfo.Key;
                    if (step.IsNotNull())
                    {
                        var count = 0;
                        int? maxindex = null;
                        int? minindex = null;
                        int? oldindex = null;
                        elementList.ForEach(p =>
                        {
                            if (p == step)
                            {
                                oldindex = count;
                            }
                            else if (afterElements.Contains(p))
                            {
                                minindex = count;
                                if (maxindex.HasValue && minindex > maxindex)
                                {
                                    throw new Exception("Order info is error!");
                                }
                            }
                            else if (beforeElements.Contains(p))
                            {
                                if (!maxindex.HasValue)
                                {
                                    maxindex = count;
                                }
                            }
                            count += 1;
                        });

                        int? index = null;

                        if (!maxindex.HasValue && minindex.HasValue)
                        {
                            index = minindex.Value + 1;
                        }
                        else if (maxindex.HasValue && !minindex.HasValue)
                        {
                            index = maxindex.Value;
                        }
                        else if (maxindex.HasValue && minindex.HasValue)
                        {
                            index = maxindex.Value;
                        }

                        if (oldindex.HasValue && index.HasValue)
                        {
                            if (oldindex >= index)
                            {
                                elementList.Remove(step);
                                elementList.Insert(index.Value, step);
                            }
                            else
                            {
                                elementList.Remove(step);
                                elementList.Insert(index.Value - 1, step);
                            }
                        }
                    }
                }
            });

            return elementList;
        }

        private Dictionary<T, IElementOrderProvider<T>> GetAllElementsOrderProvider(IEnumerable<T> processors)
        {
            var result = new Dictionary<T, IElementOrderProvider<T>>();
            processors.Select(step =>
                                    new { Key = step, Value = GetElementOrderProvider(step) }
                                    ).Where(keyvalue => keyvalue.Value.IsNotNull()).ForEach(item => result.Add(item.Key, item.Value));

            return result;
        }

        private IElementOrderProvider<T> GetElementOrderProvider(T processor)
        {
            return processor as IElementOrderProvider<T>;
        }
    }

}