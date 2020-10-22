using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Bismark.BOL
{
    public class Pension
    {       

        public static List<Year> GetDetails(int empId)
        {
            return RePackage(PayrollSQL.GetFiveHighestPaidYears(empId));
        }

        private static List<Year> RePackage(SqlDataReader dr)
        {
            var yearList = new List<Year>();

            var ytd = dr.GetOrdinal("YTD");
            var name = dr.GetOrdinal("YEAR");
            
            while (dr.Read())
            {
                var year = new Year();
                year.Name = Convert.ToInt32(dr[name]);
                year.Amount = Convert.ToDecimal(dr[ytd]);
                yearList.Add(year);
            }
            return yearList;
        }     
        

        public class Year
        {
            private int name;
            private decimal amount;

            public int Name { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
