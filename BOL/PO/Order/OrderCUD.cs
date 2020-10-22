using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Bismark.SQL;
using Bismark.Utilities;

namespace Bismark.BOL
{
    public class OrderCUD
    {
        /// <summary>
        /// This method creates a new Order record and adds it to the database.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>True if successful, False if not.</returns>
        public static bool Create(Order order)
        {
            if (!order.IsComplete)
            {
                throw new InvalidOperationException("The order is incomplete.");
            }
            return OrderSQL.Insert(order);
        }
    }
}
