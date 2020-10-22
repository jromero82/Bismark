using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Bismark.Utilities;
using Bismark.SQL;
using System.Data.SqlClient;

namespace Bismark.BOL
{
    public class SalaryAdjustment : ISalaryAdjustment
    {
        //FIELDS
        private int salaryAdjustmentId;        
        private decimal percentIncrease;
        private DateTime effectiveDate;

        //PROPERTIES
        public int SalaryAdjustmentId
        {
            get
            {
                return salaryAdjustmentId;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Salary Adjustment ID");
                salaryAdjustmentId = value;
            }
        }        
        public decimal PercentIncrease
        {
            get
            {
                return percentIncrease;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Salary increase percentage");
                Validation.ValidateNumericValue(value, CheckNumericValue.MustBeBetween, "Salary increase percentage", 0.01f, 1.0f);
                percentIncrease = value;
            }
        }
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDate;
            }
            set
            {               
                effectiveDate = value;
            }
        }

        public List<int> EmployeeIds
        {
            get
            {
                return SalaryAdjustmentLists.GetEmployeeIds(salaryAdjustmentId);
            }
        }        

        //METHODS
        public static SalaryAdjustment Create()
        {
            return new SalaryAdjustment();
        }

        public static SalaryAdjustment Create(int salaryAdjustmentId)
        {
            return RePackage(SalaryAdjustmentSQL.GetSalaryAdjustment(salaryAdjustmentId))[0];
        }

        public static List<SalaryAdjustment> RePackage(SqlDataReader dr)
        {   
            var salaryAdjustmentList = new List<SalaryAdjustment>();
            if (dr.HasRows)
            {
                var salaryAdjustmentSalaryAdjustmentId = dr.GetOrdinal("SalaryAdjustmentId");
                var salaryAdjustmentPercentIncrease = dr.GetOrdinal("PercentIncrease");
                var salaryAdjustmentEffectiveDate = dr.GetOrdinal("EffectiveDate");

                while (dr.Read())
                {
                    var salaryAdjustment = new SalaryAdjustment();

                    salaryAdjustment.SalaryAdjustmentId = Convert.ToInt32(dr[salaryAdjustmentSalaryAdjustmentId]);
                    salaryAdjustment.PercentIncrease = Convert.ToDecimal(dr[salaryAdjustmentPercentIncrease]);
                    salaryAdjustment.EffectiveDate = Convert.ToDateTime(dr[salaryAdjustmentEffectiveDate]);

                    salaryAdjustmentList.Add(salaryAdjustment);
                }
            }
            return salaryAdjustmentList;
        }

        public static List<int> RePackageEmpIdList(SqlDataReader dr)
        {
            var employeeIdList = new List<int>();

            var employeeId = dr.GetOrdinal("EmployeeId");

            while (dr.Read())
            {
                var id = new int();
                id = Convert.ToInt32(dr[employeeId]);
                employeeIdList.Add(id);
            }

            return employeeIdList;
        }
    }
}
