using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.SQL;
using Bismark.Utilities;

namespace Bismark.BOL
{
    public class SalaryAdjustmentLists
    {
        public static List<int> GetEmployeeIds(int salaryAdjId)
        {
            return SalaryAdjustment.RePackageEmpIdList(SalaryAdjustmentSQL.GetEmployeeIds(salaryAdjId));
        }

        public static List<SalaryAdjustment> GetSalaryAdjustmentsByEmpId(int employeeId, SalaryAdjustmentGetType type)
        {           
            return SalaryAdjustment.RePackage(SalaryAdjustmentSQL.GetSalaryAdjustmentByEmpId(employeeId, type));
        }
    }
}
