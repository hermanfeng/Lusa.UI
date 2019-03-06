using System.Collections.Generic;

namespace Lusa.AddinEngine.ElementOrder
{
    public interface IElementOrderProvider<T>
    {
        ICollection<T> BeforeElements(ICollection<T> allElements);
        ICollection<T> AfterElements(ICollection<T> allElements);
    }
}