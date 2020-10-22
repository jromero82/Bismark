using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.Data;
using Bismark.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace Bismark
{
    public class PayrollSQL
    {
        public static void Run()
        {
            DAL.SendData("RunPayroll");
        }

        public static SqlDataReader GetCurrentPayPeriodStartDate()
        {
            return DAL.GetDataReader("GetCurrentPayPeriodStartDate");
        }
        
        public static SqlDataReader GetFiveHighestPaidYears(int employeeId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@empId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("getHighestFiveEarningYears", parameterList);
        }
    }
}
