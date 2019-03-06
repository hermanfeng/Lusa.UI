using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lusa.AddinEngine.Extension
{
    public static class DictionaryExtension
    {
        public static void AddExtension<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value,
            bool isreplace = true)
        {
            if (key.IsNull())
            {
                return;
            }

            if (!dic.ContainsKey(key))
            {
                dic.Add(key, value);
            }
            else if (isreplace)
            {
                dic[key] = value;
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value, bool isreplace = false)
        {
            if (key.IsNull())
            {
                return default(TValue);
            }

            if (!dic.ContainsKey(key))
            {
                dic.Add(key, value);
            }
            else if (isreplace)
            {
                dic[key] = value;
            }

            return dic[key];
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TKey,TValue> valueFactory, bool isreplace = false)
        {
            if (key.IsNull() && valueFactory.IsNull())
            {
                return default(TValue);
            }

            if (!dic.ContainsKey(key))
            {
                dic.Add(key, valueFactory(key));
            }
            else if (isreplace)
            {
                dic[key] = valueFactory(key);
            }

            return dic[key];
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            if (dic.IsNull() || key.IsNull() || !dic.ContainsKey(key))
            {
                return defaultValue;
            }

            return dic[key];
        }

        public static string Serialize(this IDictionary<string, string> dic)
        {
            XElement xe = new XElement("Dictionary");
            dic.ToList().ForEach(keyvalue =>
            {
                XElement childxe = new XElement("DictionaryItem");
                childxe.SetAttributeValue("Key", keyvalue.Key);
                childxe.SetAttributeValue("Value", keyvalue.Value);
                xe.Add(childxe);
            });
            return xe.ToString();
        }

        public static void DeSerialize(this IDictionary<string, string> dic, string serializedata)
        {
            if (!string.IsNullOrEmpty(serializedata))
            {
                XElement xe = XElement.Parse(serializedata);
                var items = xe.Elements(XName.Get("DictionaryItem"));
                items.ToList().ForEach(item =>
                {
                    string key = string.Empty;
                    var attr = item.Attribute("Key");
                    if (attr != null)
                    {
                        key = attr.Value;
                    }

                    string value = string.Empty;
                    attr = item.Attribute("Value");
                    if (attr != null)
                    {
                        value = attr.Value;
                    }

                    if (!string.IsNullOrEmpty(key))
                    {
                        dic.AddExtension(key, value);
                    }
                });
            }
        }
    }
}