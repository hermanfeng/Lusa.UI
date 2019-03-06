using System;
using System.Collections.Generic;
using System.Linq;

namespace AddinEngine
{
    public interface IElementOrderProvider<T>
    {
        ICollection<T> BeforeElements(ICollection<T> allElements);
        ICollection<T> AfterElements(ICollection<T> allElements);
    }
}