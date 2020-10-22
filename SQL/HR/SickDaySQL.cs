using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.Utilities;
using Bismark.Data;
using System.Data;

namespace Bismark.SQL
{
    public class SickDaySQL
    {
        public static bool RecordSickDay(ISickDay sickday, int employeeId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@empId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@date", sickday.Date, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@isFullDay", sickday.IsFullDay, ParameterDirection.Input, SqlDbType.Bit));
            return DAL.SendData("RecordSickDay", parameterList);      
        }       
    }
}
