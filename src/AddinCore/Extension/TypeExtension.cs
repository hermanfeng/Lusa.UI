using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CommonExtension
{
    public static class TypeExtension
    {
        public static string GetAssemblyPath(this Type type)
        {
            return type.Assembly.Location;
        }

        public static string GetAssemblyDirectory(this Type type)
        {
            return Path.GetDirectoryName(type.GetAssemblyPath());
        }

        public static T GetAttribute<T>(this Type type)  where T : Attribute
        {
            if (type.IsNull())
            {
                return default(T);
            }

            return type.GetCustomAttributes(true).FirstOrDefault(attr => attr is T) as T;
        }

        public static bool Is<T>(this Type type)
        {
            if (type.IsNull())
            {
                return false;
            }
            return typeof (T).IsAssignableFrom(type);
        }

        public static bool Is(this Type type,Type checktype)
        {
            if (type.IsNull() || checktype.IsNull())
            {
                return false;
            }
            return checktype.IsAssignableFrom(type);
        }

        public static IEnumerable<Type> GetAssemblyTypes<T>(this Type type)
        {
            return type.Assembly.GetTypes()
                .Select(stype => typeof (T).IsAssignableFrom(stype) && stype.IsPublic && !stype.IsAbstract && !stype.IsInterface ? stype : null)
                .OfType<Type>();
        }

        public static IEnumerable<T> GetAssemblyTypeInstances<T>(this Type type,Func<Type,bool> canInstance=null)
        {
            List<T> allInstances = new List<T>();
            type.GetAssemblyTypes<T>().ForEach(realtype =>
                {
                    if (canInstance == null || canInstance(realtype))
                    {
                        allInstances.AddExtension((T)Activator.CreateInstance(realtype));
                    }
                });
            return allInstances;
        }

        public static T CreateInstance<T>(this Type type)
        {
            return type.IsNotNull() ? (T)Activator.CreateInstance(type): default(T);
        }
    }
}