using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.SQL;

namespace Bismark.BOL
{
    public class ItemCUD
    {
        public static bool Update(int employeeId, Item item)
        {
            return ItemSQL.Update(employeeId, item);
        }
    }
}
