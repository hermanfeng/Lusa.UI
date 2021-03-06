﻿using System.Xml.Linq;

namespace Lusa.AddinEngine.Extension
{
    public static class XElementExtension
    {
        public static string GetAttributeValue(this XElement xe, string attrname,string defaultvalue = "")
        {
            if (xe.IsNotNull() && attrname.IsNotNullOrEmpty())
            {
                var attr = xe.Attribute(attrname);
                if (attr.IsNotNull())
                {
                    return attr.Value;
                }
            }
            return defaultvalue;
        }
    }
}