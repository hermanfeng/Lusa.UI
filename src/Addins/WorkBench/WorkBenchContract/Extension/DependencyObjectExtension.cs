using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CommonLibrary;
using CommonExtension;

namespace CommonExtension
{
    public static class DependencyObjectExtension
    {
        public static List<T> FindVisualChild<T>(this DependencyObject obj) where T : class
        {
            List<T> TList = new List<T>();
            try
            {
                
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    object child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        TList.Add(child as T);
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild<T>(child as DependencyObject);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                
            }
            catch (Exception ee)
            {
                MessageService.Instance.SendMessage(ee);
            }
            return TList;
        }

        public static T FindFirstVisualChild<T>(this DependencyObject obj) where T:class
        {
            try
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    object child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }
                    else
                    {
                        var childResult = FindFirstVisualChild<T>(child as DependencyObject);
                        if(childResult.IsNotNull())
                        {
                            return childResult;
                        }
                    }
                }
                
            }
            catch (Exception ee)
            {
                MessageService.Instance.SendMessage(ee);
            }
            return null;
        }
    }
}
