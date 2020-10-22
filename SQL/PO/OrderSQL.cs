using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Bismark.Utilities;
using Bismark.Data;

namespace Bismark.SQL
{
    public class OrderSQL
    {

        #region CUD

        public static bool Insert(IOrder order)
        {

            
            var itemList = order.Items;
            var parameterList = new List<DataParameter>();
            var itemTable = new DataTable();
            try
            {
                itemTable.Columns.Add("Name", Type.GetType("System.String"));
                itemTable.Columns.Add("Description", Type.GetType("System.String"));
                itemTable.Columns.Add("Price", Type.GetType("System.Decimal"));
                itemTable.Columns.Add("Quantity", Type.GetType("System.Int32"));
                itemTable.Columns.Add("Justification", Type.GetType("System.String"));
                itemTable.Columns.Add("Source", Type.GetType("System.String"));

                for (int i = 0; i <= order.Items.Count - 1; i++)
                {
                    itemTable.Rows.Add(new object[] {
                        itemList[i].Name,
                        itemList[i].Description,
                        itemList[i].Price,
                        itemList[i].Quantity,
                        itemList[i].Justification,
                        itemList[i].Source
                    });
                }

                parameterList.Add(new DataParameter("@orderId", 0, ParameterDirection.Output, SqlDbType.Int));
                parameterList.Add(new DataParameter("@employeeId", order.EmployeeId, ParameterDirection.Input, SqlDbType.Int));
                parameterList.Add(new DataParameter("@itemTable", itemTable, ParameterDirection.Input, SqlDbType.Structured));

                DAL.SendData("CreateOrder", parameterList);
                order.OrderId = Convert.ToInt32(parameterList[0].Value);
                return true;
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }
        }

        public static bool Close(int employeeId, int orderId)
        {
            var parameterList = new List<DataParameter>();

            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@orderId", orderId, ParameterDirection.Input, SqlDbType.Int));            
            return DAL.SendData("CloseOrder", parameterList);
        }

        #endregion

        #region Gets

        public static DataTable GetOrderById(int employeeId, int orderId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@orderId", orderId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataTableUsingReader("GetOrderById", parameterList);
        }

        public static DataTable GetOrdersByDepartment(int employeeId, Department department, Filter filter, 
            DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var parameterList = new List<DataParameter>();

            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@department", department, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@dateFrom", dateFrom, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@dateTo", dateTo, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@filter", filter, ParameterDirection.Input, SqlDbType.Int));

            return DAL.GetDataTableUsingReader("GetOrdersByDepartment", parameterList);   
        }

        public static DataTable GetOrdersByEmployeeId(int employeeId, Filter filter, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@dateFrom", dateFrom, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@dateTo", dateTo, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@filter", filter, ParameterDirection.Input, SqlDbType.Int));

            return DAL.GetDataTableUsingReader("GetOrdersByEmployeeId", parameterList);
        }

        public static DataTable GetOrdersByEmployeeName(int employeeId, Department department, Filter filter, string firstName = null,
            string lastName = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@department", department, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@firstName", firstName, ParameterDirection.Input, SqlDbType.VarChar, 25));
            parameterList.Add(new DataParameter("@lastName", lastName, ParameterDirection.Input, SqlDbType.VarChar, 25));
            parameterList.Add(new DataParameter("@dateFrom", dateFrom, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@dateTo", dateTo, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@filter", filter, ParameterDirection.Input, SqlDbType.Int));

            return DAL.GetDataTableUsingReader("GetOrdersByEmployeeName", parameterList);
        }

        #endregion

        #region Business Rules

        public static bool CheckIfOrderIsClosed(int orderId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@result", 0, ParameterDirection.Output, SqlDbType.Bit));
            parameterList.Add(new DataParameter("@orderId", orderId, ParameterDirection.Input, SqlDbType.Int));

            DAL.SendData("CheckIfOrderIsClosed", parameterList);
            return Convert.ToBoolean(parameterList[0].Value);
        }

        public static bool CheckIfItemsArePending(int orderId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@result", 0, ParameterDirection.Output, SqlDbType.Bit));
            parameterList.Add(new DataParameter("@orderId", orderId, ParameterDirection.Input, SqlDbType.Int));

            DAL.SendData("CheckIfItemsArePending", parameterList);
            return Convert.ToBoolean(parameterList[0].Value);
        }

        #endregion
    }
}
