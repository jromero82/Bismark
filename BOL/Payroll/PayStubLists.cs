using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.SQL;

namespace Bismark.BOL
{
    public class PayStubLists
    {
        public static List<PayStub> GetPayStubsByEmpId(int employeeId)
        {
            return PayStub.RePackage(PayStubSQL.GetPayStubsByEmpId(employeeId));
        }

        public static List<PayStub> GetPayStubsByDate(DateTime payPeriodEndDate)
        {
            return PayStub.RePackage(PayStubSQL.GetPayStubsByDate(payPeriodEndDate));
        }

        public static List<PayStub> GetPayStubsByDateRange(int employeeId, DateTime dateFrom, DateTime dateTo)
        {
            return PayStub.RePackage(PayStubSQL.GetPayStubsByDateRange(employeeId, dateFrom, dateTo));
        }
    }
}
