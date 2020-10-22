using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.SQL;

namespace Bismark.BOL
{
    public class BusinessRules
    {
        public static bool IsOrderClosed(int orderId)
        {
            return OrderSQL.CheckIfOrderIsClosed(orderId);
        }
    }
}
