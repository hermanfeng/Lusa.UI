using System;
using System.Collections.Generic;
using System.Linq;

namespace AddinEngine
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ElementOrderAttribute :Attribute
    {
        private readonly int _order = -1;
        public ElementOrderAttribute(int order)
        {
            _order = order;
        }

        public int Order
        {
            get { return this._order; }
        }
    }

}