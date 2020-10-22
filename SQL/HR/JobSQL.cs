using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Bismark.Data;
using Bismark.Utilities;
using System.Data;

namespace Bismark.SQL
{
    public class JobSQL
    {
        public static SqlDataReader GetJobDetails(int jobId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@jobId", jobId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetJobDetails", parameterList);
        }

        public static SqlDataReader getJobsByDept(Department dept)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@dept", dept, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetJobsByDept", parameterList);
        }

        public static SqlDataReader getJobSupervisor(Department dept)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@deptId", dept, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetDeptSupervisor", parameterList);
        }
    }
}
