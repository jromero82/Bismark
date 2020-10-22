using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Bismark.BOL
{
    public class Payroll
    {
        public static void Run()
        {
            PayrollSQL.Run();
        }

        public static DateTime GetCurrentPayPeriodStartDate()
        {
            var dr = PayrollSQL.GetCurrentPayPeriodStartDate();
            while (dr.Read())
            {
                return Convert.ToDateTime(dr[0]);
            }
            throw new DataException("Error!");
            
        }
    }
}
