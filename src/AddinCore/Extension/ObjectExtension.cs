using System;
using System.Reflection;

namespace Lusa.AddinEngine.Extension
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static T As<T>(this object obj, Action<T> action=null, T defaultObj=default(T))
        {
            var convertedObj = obj is T ? (T)obj : default(T);
            if (convertedObj.IsNull())
            {
                return defaultObj;
            }
            if (action.IsNotNull())
            {
                action(convertedObj);
            }
            return convertedObj;
        }

        public static object As<T>(this object obj, Func<T, object> successCallback, Func<object> failCallback = null)
        {
            var convertedObj = obj is T ? (T)obj : default(T);
            if (convertedObj.IsNotNull())
            {
                if (successCallback.IsNotNull())
                {
                    return successCallback(convertedObj);
                }
            }

            if (failCallback.IsNotNull())
            {
                return failCallback();
            }
            return null;
        }

        public static TOut As<T, TOut>(this object obj, Func<T, TOut> successCallback, Func<TOut> failCallback = null)
        {
            var convertedObj = obj is T ? (T)obj : default(T);
            if (convertedObj.IsNotNull())
            {
                if (successCallback.IsNotNull())
                {
                    return successCallback(convertedObj);
                }
            }

            if (failCallback.IsNotNull())
            {
                return failCallback();
            }

            return default(TOut);
        }


        public static void CheckNull(this object obj, string paramname, InvalidAction action = InvalidAction.Exception)
        {
            if (obj.IsNull())
            {
                DoAction(action, paramname);
            }
        }

        private static void DoAction(InvalidAction action, string paramname)
        {
            if ((action & InvalidAction.Exception) == InvalidAction.Exception)
            {
                throw new ArgumentNullException(paramname);
            }
            // to do other types
        }
    }

    public static class RefelctionExtension
    {
        public static object ReflectGetFiled(this object target, string fieldname, Type methodtype = null)
        {
            var type = methodtype ?? target.GetType();
            var minfo = type.GetField(fieldname, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod);
            if (minfo != null)
            {
                return minfo.GetValue(target);
            }
            return null;
        }

        public static void ReflectSetFiled(this object target, string fieldname, object value, Type methodtype = null)
        {
            var type = target is Type ? (Type)target : target.GetType();
            var minfo = type.GetField(fieldname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod);
            if (minfo != null)
            {
                minfo.SetValue(target, value);
            }
        }

        public static object ReflectGetProperty(this object target, string propertyname, Type methodtype = null)
        {
            var type = methodtype ?? target.GetType();
            var minfo = type.GetProperty(propertyname, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod);
            if (minfo != null)
            {
                return minfo.GetValue(target, null);
            }
            return null;
        }

        public static void ReflectSetProperty(this object target, string propertyname, object value, Type methodtype = null)
        {
            var type = methodtype ?? target.GetType();
            var minfo = type.GetProperty(propertyname, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod);
            if (minfo != null)
            {
                minfo.SetValue(target, value, null);
            }
        }

        public static object ReflectInvorkMehod(this object target, string funcname, Type methodtype = null, object[] args = null)
        {
            var type = methodtype ?? target.GetType();
            var minfo = type.GetMethod(funcname, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod);
            if (minfo != null)
            {
                return minfo.Invoke(target, args);
            }
            return null;
        }
    }

    [Flags]
    public enum InvalidAction
    {
        Exception,
        Log,
        Message
    }
}