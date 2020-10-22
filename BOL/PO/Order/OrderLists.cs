using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.SQL;
using Bismark.Utilities;

namespace Bismark.BOL
{
    public class OrderLists
    {
        public static List<Order> GetOrdersByEmployeeId(int employeeId, Filter filter, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            return Order.RePackage(OrderSQL.GetOrdersByEmployeeId(employeeId, filter, dateFrom, dateTo));
        }

        public static List<Order> GetOrdersByDepartment(int employeeId, Department department, Filter filter, 
            DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            return Order.RePackage(OrderSQL.GetOrdersByDepartment(employeeId, department, filter, dateFrom, dateTo));
        }

        public static List<Order> GetOrdersByEmployeeName(int employeeId, Department department, Filter filter, 
            string firstName = null, string lastName = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            if (string.IsNullOrEmpty(firstName) &&
                string.IsNullOrEmpty(lastName))
                throw new ArgumentException("No employee names detected. Please provide at least one value.");
            return Order.RePackage(OrderSQL.GetOrdersByEmployeeName(employeeId, department, filter, firstName, lastName, dateFrom, dateTo));
        }
    }
}
