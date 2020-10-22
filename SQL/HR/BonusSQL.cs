using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.Utilities;
using System.Data;
using Bismark.Data;
using System.Data.SqlClient;

namespace Bismark.SQL
{
    public class BonusSQL
    {
        public static bool Create(List<int> employeeIdList, IBonus bonus)
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

                parameterList.Add(new DataParameter("@payPeriod", bonus.EffectiveDate, ParameterDirection.Input, SqlDbType.Date));
                parameterList.Add(new DataParameter("@employeeIdTable", idTable, ParameterDirection.Input, SqlDbType.Structured));

                if (bonus.PercentBonus != 0)
                {
                    parameterList.Add(new DataParameter("@percent", bonus.PercentBonus, ParameterDirection.Input, SqlDbType.Decimal));
                }
                else
                {
                    parameterList.Add(new DataParameter("@fixedBonus", bonus.FixedBonus, ParameterDirection.Input, SqlDbType.Money));
                }

                DAL.SendData("CreateBonus", parameterList);
                return true;
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }

        }

        public static SqlDataReader GetBonusByEmpId(int employeeId, BonusGetType type)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@getType", type, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetBonusByEmpId", parameterList);
        }

        public static SqlDataReader GetBonusById(int bonusId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@bonusId", bonusId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetBonusById", parameterList);
        }

        public static bool DeleteById(int bonusId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@bonusId", bonusId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.SendData("DeleteBonusById", parameterList);
        }

        public static bool DeleteSingleBonusByEmpId(int employeeId, int bonusId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@bonusId", bonusId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.SendData("DeleteSingleBonusByEmpId", parameterList);
        }
    }
}
