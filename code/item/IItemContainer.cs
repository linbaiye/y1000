using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.item
{
    public interface IItemContainer
    {
        bool RemoveItem(IItem item);

        void PutItem(IItem item);

        bool ContainsItem(IItem item);

        bool CanPut(IItem item) { return false; }

        bool AtCursor() { return false; }
    }
}