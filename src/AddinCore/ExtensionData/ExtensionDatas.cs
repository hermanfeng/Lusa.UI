using System;
using System.Collections.Generic;
using System.Linq;
using Lusa.AddinEngine.Extension;

namespace Lusa.AddinEngine.ExtensionData
{
    public class ExtensionDatas
    {
        private readonly Dictionary<object, object> _dataDictionary =
            new Dictionary<object, object>();

        public int Count
        {
            get { return _dataDictionary.Keys.Count; }
        }

        public bool ContainsKey(object key)
        {
            return _dataDictionary.ContainsKey(key);
        }

        public T GetData<T>()
        {
            var result = default(T);

            result = GetData(typeof(T), result);

            return result;
        }

        public T GetData<T>(object key, T defaultValue=default(T))
        {
            return _dataDictionary.GetValue(key,defaultValue).As<T>();
        }

        public T GetOrAdd<T>(object key, Func<object, T> valueFactory)
        {
            if (key.IsNotNull() && valueFactory.IsNotNull() && !_dataDictionary.ContainsKey(key))
            {
                _dataDictionary.Add(key,valueFactory(key));
            }
            return _dataDictionary.GetValue(key).As<T>();
        }

        public T GetOrAdd<T>(Func<object, T> valueFactory)
        {
            object key = typeof (T);
            return GetOrAdd<T>(key, valueFactory).As<T>();
        }

        public T AddData<T>(object key, T value, bool replace=false)
        {
            return _dataDictionary.GetOrAdd(key, value, replace).As<T>();
        }

        public T AddData<T>(T value, bool replace=true)
        {
            var key = typeof (T);
            return AddData(key, value, replace);
        }

        public void Remove(object key)
        {
            if (key.IsNotNull())
            {
                _dataDictionary.Remove(key);
            }
        }

        public void Combine(ExtensionDatas propertys)
        {                
            propertys._dataDictionary.Where(pair => !ContainsKey(pair.Key)).ForEach(pair =>
                AddData(pair.Key, pair.Value));
        }
    }
}