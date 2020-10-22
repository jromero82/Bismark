using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Bismark.Utilities;


namespace Bismark.Data
{
    public sealed class DAL : DALCnn
    {
        private DAL() {}

        public static Object GetScalar(string sproc, List<DataParameter> parameterList = null)
        {
            var cmd = CreateCommand(sproc, parameterList);
            using (cmd.Connection)
            {
                cmd.Connection.Open();
                return cmd.ExecuteScalar();
            }
        }

        public static DataTable GetDataTable(string sproc, List<DataParameter> parameterList = null)
        {
            var cmd = CreateCommand(sproc, parameterList);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.Fill(dt);
            return dt;
        }

        public static DataTable GetDataTableUsingReader(string sproc, List<DataParameter> parameterList = null)
        {
            var cmd = CreateCommand(sproc, parameterList);
            var dt = new DataTable();
            using (cmd.Connection)
            {
                cmd.Connection.Open();
                dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
            }
            return dt;
        }

        public static SqlDataReader GetDataReader(string sproc, List<DataParameter> parameterList = null)
        {
            var cmd = CreateCommand(sproc, parameterList);
            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }

        public static DataSet GetDataSet(string tableName, string sproc, List<DataParameter> parameterList = null)
        {
            var cmd = CreateCommand(sproc, parameterList);
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            using (cmd.Connection)
            {
                cmd.Connection.Open();
                da.Fill(ds, tableName);
            }
            return ds;
        }

        public static bool SendData(string sproc, List<DataParameter> parameterList = null)
        {
            var cmd = CreateCommand(sproc, parameterList);
            using (cmd.Connection)
            {
                cmd.Connection.Open();
                var recordsAffected = cmd.ExecuteNonQuery();
                for (int i = 0; i <= parameterList.Count - 1; i++)
                {
                    var parameter = parameterList[i];
                    parameter.Value = cmd.Parameters[i].Value;
                    parameterList[i] = parameter;
                }
            }
            return true;
        }

        private static SqlCommand CreateCommand(string sproc, List<DataParameter> paramterList = null)
        {
            var cmd = new SqlCommand(sproc, GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            if (paramterList != null)   
            {
                foreach (var parameter in paramterList)
                {
                    var newParameter = cmd.CreateParameter();
                    newParameter.ParameterName = parameter.Name;
                    newParameter.Value = parameter.Value;
                    newParameter.SqlDbType = parameter.DataType;
                    newParameter.Direction = parameter.Direction;
                    newParameter.Size = parameter.Size;
                    cmd.Parameters.Add(newParameter);
                }
            }
            return cmd;
        }
    }
}
