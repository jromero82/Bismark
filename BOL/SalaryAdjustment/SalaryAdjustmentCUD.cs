using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.SQL;
using Bismark.Utilities;

namespace Bismark.BOL
{
    public class SalaryAdjustmentCUD
    {
        public static bool Create(List<int> employeeIdList, SalaryAdjustment salaryAdj)
        {            
            return SalaryAdjustmentSQL.Create(employeeIdList, salaryAdj);
        }

        public static bool DeleteByAdjId(int salaryAdjId)
        {
            if (salaryAdjId == 0)
            {
                throw new ArgumentException("No salary adjustments provided. No adjustments deleted.");
            }
            return SalaryAdjustmentSQL.DeleteByAdjId(salaryAdjId);
        }

        public static bool DeleteSingleEmployeeSalaryAdjustment(int employeeId, int salaryAdjId)
        {
            if (employeeId == 0 || salaryAdjId == 0)
            {
                throw new ArgumentException("Employee Id or Salary Adjustment ID invalid.");
            }
            return SalaryAdjustmentSQL.DeleteSingleEmployeeSalaryAdjustment(employeeId, salaryAdjId);
        }

        
    }
}




