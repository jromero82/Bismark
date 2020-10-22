using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Bismark.SQL;
using Bismark.Utilities;

namespace Bismark.BOL
{
    public class SickDay : ISickDay
    {
        private int sickDayID;
        private DateTime date;
        private bool isFullDay;

        public int SickDayId
        {
            get
            {
                return sickDayID;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Sick day ID");
                sickDayID = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }

        public bool IsFullDay
        {
            get
            {
                return isFullDay;
            }
            set
            {
                isFullDay = value;
            }
        }



        public static SickDay Create()
        {
            return new SickDay();
        }

        public static List<SickDay> RePackage(SqlDataReader dr)
        {
            var sickDayList = new List<SickDay>();

            var sickDayId = dr.GetOrdinal("SickDayId");
            var sickDayDate = dr.GetOrdinal("Date");
            var sickDayIsFullDay = dr.GetOrdinal("IsFullDay");

            while (dr.Read())
            {
                var sd = new SickDay();
                sd.sickDayID = Convert.ToInt32(dr[sickDayId]);
                sd.date = Convert.ToDateTime(dr[sickDayDate]);
                sd.isFullDay = Convert.ToBoolean(dr[sickDayIsFullDay]);

                sickDayList.Add(sd);
            }

            return sickDayList;
        }

        public static List<SickDay> GetSickDays(int employeeId)
        {
            return SickDay.RePackage(EmployeeSQL.GetSickDays(employeeId));
        }


        public static bool RecordSickDay(SickDay sickday, int employeeId)
        {
           return SickDaySQL.RecordSickDay(sickday, employeeId);
        }

        
    }
}
