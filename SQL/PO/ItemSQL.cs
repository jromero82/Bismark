using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Bismark.Utilities;
using System.Data;
using Bismark.Data;

namespace Bismark.SQL
{
    public class ItemSQL
    {

        public static bool Update(int employeeId, IItem item)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@itemId", item.ItemId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@orderId", item.OrderId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@name", item.Name, ParameterDirection.Input, SqlDbType.VarChar, 30));
            parameterList.Add(new DataParameter("@description", item.Description, ParameterDirection.Input, SqlDbType.VarChar, 50));
            parameterList.Add(new DataParameter("@price", item.Price, ParameterDirection.Input, SqlDbType.Decimal));
            parameterList.Add(new DataParameter("@quantity", item.Quantity, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@justification", item.Justification, ParameterDirection.Input, SqlDbType.VarChar, 100));
            parameterList.Add(new DataParameter("@source", item.Source, ParameterDirection.Input, SqlDbType.VarChar, 100));

            return DAL.SendData("UpdateItem", parameterList);
        }

        public static DataTable GetItemById(int itemId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@itemId", itemId, ParameterDirection.Input, SqlDbType.Int));

            return DAL.GetDataTableUsingReader("GetItemById", parameterList);
        }

        public static DataTable GetItemsByOrderId(int orderId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@orderId", orderId, ParameterDirection.Input, SqlDbType.Int));

            return DAL.GetDataTableUsingReader("GetItemsByOrderId", parameterList);
        }

        public static bool Process(int employeeId, IItem item)
        {
            var parameterList = new List<DataParameter>();

            parameterList.Add(new DataParameter("@result", 0, ParameterDirection.Output, SqlDbType.Bit));
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@itemId", item.ItemId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@orderId", item.OrderId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@status", item.Status, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@reason", item.Reason, ParameterDirection.Input, SqlDbType.VarChar, 100));

            DAL.SendData("ProcessItem", parameterList);
            return Convert.ToBoolean(parameterList[0].Value);
        }
    }
}
