using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.SQL;
using Bismark.Utilities;

namespace Bismark.BOL
{
    public class ItemLists
    {
        public static ItemList GetItemsByOrderId(int orderId)
        {
            return Item.RePackage(ItemSQL.GetItemsByOrderId(orderId));
        }
    }
}
