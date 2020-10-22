using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Bismark.Utilities;
using Bismark.Data;
using System.Data.SqlClient;

namespace Bismark.SQL
{
    public class SalaryAdjustmentSQL
    {
        /// <summary>
        /// Creates a new salary adjustment record for each employee in the employeeIdList.
        /// </summary>
        /// <param name="employeeIdList">List of employee Ids</param>
        /// <param name="salaryAdj">Salary Object</param>
        /// <returns>Boolean</returns>
        public static bool Create(List<int> employeeIdList, ISalaryAdjustment salaryAdj)
        {
            var parameterList = new List<DataParameter>();
            var idTable = new DataTable("EmployeeIdTable");

            try
            {
                var colName = idTable.Columns.Add("Id", Type.GetType("System.Int32"));

                for (int i = 0; i <= employeeIdList.Count - 1; i++)
                {
                    idTable.Rows.Add(new object[]{
                        employeeIdList[i]
                    });
                }

                parameterList.Add(new DataParameter("@percentIncrease", salaryAdj.PercentIncrease, ParameterDirection.Input, SqlDbType.Money));
                parameterList.Add(new DataParameter("@effectiveDate", salaryAdj.EffectiveDate, ParameterDirection.Input, SqlDbType.Date));                
                parameterList.Add(new DataParameter("@employeeIdTable", idTable, ParameterDirection.Input, SqlDbType.Structured));
                
                DAL.SendData("CreateSalaryAdjustment", parameterList);
                return true;
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }

        }        

        /// <summary>
        /// Deletes salary adjustment records
        /// </summary>
        /// <param name="salaryAdjList">List of Salary Adjustments to be deleted</param>
        /// <returns>Boolean</returns>
        public static bool DeleteByAdjId(int salaryAdjId)
        {
            var parameterList = new List<DataParameter>();          

            parameterList.Add(new DataParameter("@salaryAdjId", salaryAdjId, ParameterDirection.Input, SqlDbType.Int));
            DAL.SendData("DeleteSalaryAdjustmentById", parameterList);
            return true;            
        }

        public static bool DeleteSingleEmployeeSalaryAdjustment(int employeeId, int salaryAdjId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@salaryAdjId", salaryAdjId, ParameterDirection.Input, SqlDbType.Int));
            DAL.SendData("DeleteSingleEmployeeSalaryAdjustment", parameterList);
            return true;
        }

        public static SqlDataReader GetEmployeeIds(int salaryAdjId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@salaryAdjId", salaryAdjId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetSalaryAdjustmentEmployeeIds", parameterList);
        }

        public static SqlDataReader GetSalaryAdjustment(int salaryAdjId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@salaryAdjId", salaryAdjId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetSalayAdjustmentById", parameterList);
        }

        public static SqlDataReader GetSalaryAdjustmentByEmpId(int employeeId, SalaryAdjustmentGetType type)
        {          
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@getType", type, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetSalaryAdjustmentByEmpId", parameterList);          
            
        }
    }
}
