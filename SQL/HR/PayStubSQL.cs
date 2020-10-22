using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Bismark.Utilities;
using Bismark.Data;
using System.Data;

namespace Bismark.SQL
{
    public class PayStubSQL
    {

        public static SqlDataReader GetPayStubById(int payStubId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@payStubId", payStubId, ParameterDirection.Input, SqlDbType.Int));

            return DAL.GetDataReader("GetPayStubById", parameterList);
        }

        public static SqlDataReader GetPayStubsByEmpId(int employeeId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));

            return DAL.GetDataReader("GetPayStubsByEmpId", parameterList);
        }

        public static SqlDataReader GetPayStubsByDate(DateTime payPeriodEndDate)
        {
            throw new NotImplementedException();
        }

        public static SqlDataReader GetPayStubsByDateRange(int employeeId, DateTime dateFrom, DateTime dateTo)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@dateFrom", dateFrom, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@dateTo", dateTo, ParameterDirection.Input, SqlDbType.Date));

            return DAL.GetDataReader("GetPayStubsByDateRange", parameterList);
        }
    }
}
