using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Bismark;
using Bismark.Utilities;
using Bismark.SQL;
using System.ComponentModel;

namespace Bismark.BOL
{
    public class Employee : IEmployee
    {
    
        #region 'Properties'

        //FIELDS
        private int employeeId;
        private string firstName;
        private string lastName;
        private string middleInitial;
        private string address;
        private string city;
        private string province;
        private string postalCode;
        private string workPhone;
        private string cellPhone;
        private string email;
        private DateTime dateOfBirth;
        private string sin;
        private int status;
        private DateTime hireDate;
        private DateTime jobStartDate;
        private int jobId;
        private decimal salary;
        private DateTime salaryEffectiveDate;
        private decimal prevSalary;
        private string jobTitle;
        private Department department;
        //private string supervisor;

        //PROPERTIES
        public int EmployeeId
        {
            get
            {
                return employeeId;
            }
            set
            {
                Validation.ValidateEmployeeId(value);               
                employeeId = value;                               
            }
        }
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "First Name");
                Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "First Name", 25);
                firstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "Last Name");
                Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "First Name", 25);
                lastName = value;
            }
        }
        public string MiddleInitial
        {
            get
            {
                return middleInitial;
            }
            set
            {
                Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "Middle Initial", 1);
                middleInitial = value;
            }
        }
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "Address");
                Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "Address", 50);
                address = value;
            }
        }
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "City");
                Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "City", 20);
                city = value;
            }
        }
        public string Province
        {
            get
            {
                return province;
            }
            set
            {
                Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "Province");
                Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "Province", 2);
                province = value;
            }
        }
        public string PostalCode
        {
            get
            {
                return postalCode;
            }
            set
            {
                //Validation.ValidatePostalCode(value);
                postalCode = value;
            }
        }
        public string WorkPhone
        {
            get
            {
                return workPhone;
            }
            set
            {
                Validation.ValidatePhoneNumber(value);
                workPhone = value;
            }
        }
        public string CellPhone
        {
            get
            {
                return cellPhone;
            }
            set
            {
                Validation.ValidatePhoneNumber(value);
                cellPhone = value;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                Validation.ValidateEmail(value);
                email = value;
            }
        }
        public DateTime DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }
            set
            {
                Validation.ValidateDate(value, CheckDateValue.MustBeLessThanOrEqualTo, DateTime.Today, "Date of Birth");
                dateOfBirth = value;
            }
        }
        public string SIN
        {
            get
            {
                return sin;
            }
            set
            {
                Validation.ValidateFieldLength(value, SizeOperator.MustBeEqualTo, "SIN", 9);
                Validation.ValidateStringValue(value, CheckStringValue.MustBeNumeric, "SIN");
                sin = value;
            }
        }
        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustBeBetween, "Status", 0, 2);
                status = value;
            }
        }
        public DateTime HireDate
        {
            get
            {
                return hireDate;
            }
            set
            {
                Validation.ValidateDate(value, CheckDateValue.MustBeGreaterThanOrEqualTo, DateTime.Today, "Hire Date");
                hireDate = value;
            }
        }
        public DateTime JobStartDate
        {
            get
            {
                return jobStartDate;
            }
            set
            {
                if(hireDate == new DateTime())
                {
                    throw new ArgumentException("Hire Date must be set before a Job start date can be set");
                }                
                jobStartDate = value;
            }
        }
        public int JobId
        {
            get
            {
                return jobId;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Job ID");
                jobId = value;
            }
        }
        public decimal Salary
        {
            get
            {
                return salary;
            }
            set
            {
                if (value > JobAssignment.MaxSalary)
                {
                    throw new ArgumentException("Salary cannot exceed the Max salary for the job assignment");
                }
                salary = value;
            }
        }
        public DateTime SalaryEffectiveDate
        {
            get
            {
                return salaryEffectiveDate;
            }
            set
            {
                Validation.ValidateDate(value, CheckDateValue.MustBeGreaterThanOrEqualTo, DateTime.Today, "Salary Effective Date");
                salaryEffectiveDate = value;
            }
        }
        public decimal PrevSalary
        {
            get
            {
                return prevSalary;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Previous salary");
                prevSalary = value;
            }
        }
        

        //COMPOSITION PROPERTIES
        public Job JobAssignment
        {
            get
            {
                return Job.Create(jobId);
            }
        }

        public String JobTitle
        {
            get
            {
                return jobTitle;
            }
        }

        public Department Department
        {
            get
            {
                return department;
            }
        }

        public String Supervisor
        {
            get
            {
                if (Department == Department.Executive)
                {
                    return "N/A";
                }
                else
                {
                    var superName = RePackage(EmployeeSQL.GetSupervisorName(department), EmployeeGetType.Lookup)[0];
                    return superName.lastName + ", " + superName.firstName + " " +  superName.middleInitial;                    
                }
                
            }
        }

        public List<SalaryAdjustment> SalaryAdjustmentsPending
        {
            get
            {
                return SalaryAdjustmentLists.GetSalaryAdjustmentsByEmpId(employeeId, SalaryAdjustmentGetType.getPending);
            }
        }

        public List<SalaryAdjustment> SalaryAdjustmentsHistory
        {
            get
            {
                return SalaryAdjustmentLists.GetSalaryAdjustmentsByEmpId(employeeId, SalaryAdjustmentGetType.getHistory);
            }
        }

        public List<Bonus> BonusPending
        {
            get
            {
                return BonusLists.GetBonusByEmpId(employeeId, BonusGetType.getPending);
            }
        }

        public List<Bonus> BonusHistory
        {
            get
            {
                return BonusLists.GetBonusByEmpId(employeeId, BonusGetType.getHistory);
            }
        }


        public List<SickDay> SickDays
        {
            get
            {
                return SickDay.GetSickDays(employeeId);
            }
        }

        #endregion  
      
        private Employee()
        {
            this.HireDate = DateTime.Today;
        }

        public static Employee Create()
        {
            return new Employee();
        }

        public static int Login(int employeeId, string password)
        {
            return EmployeeSQL.Login(employeeId, password);
        }

        public static Employee Create(int employeeId)
        {            
           return RePackage(EmployeeSQL.GetById(employeeId))[0];
        }

        public static int Insert(Employee employee, String password)
        {
            return EmployeeSQL.Insert(employee, password);
        }

        internal static List<Employee> RePackage(SqlDataReader dr, EmployeeGetType type = EmployeeGetType.EntireFile)
        {
            var employeeList = new List<Employee>();


            if (type == EmployeeGetType.Lookup)
            {
                var empId = dr.GetOrdinal("EmployeeId");
                var empFirstName = dr.GetOrdinal("FirstName");
                var empLastName = dr.GetOrdinal("LastName");
                var empMiddleInitial = dr.GetOrdinal("MiddleInitial");

                while (dr.Read())
                {
                    var employee = new Employee();

                    employee.employeeId = Convert.ToInt32(dr[empId]);
                    employee.firstName = Convert.ToString(dr[empFirstName]);
                    employee.lastName = Convert.ToString(dr[empLastName]);
                    employee.middleInitial = Convert.ToString(dr[empMiddleInitial]);
                    employeeList.Add(employee);
                }
            }
            else
            {                

                var employeeEmployeeId = dr.GetOrdinal("EmployeeId");
                var employeeFirstName = dr.GetOrdinal("FirstName");
                var employeeLastName = dr.GetOrdinal("LastName");
                var employeeMiddleInitial = dr.GetOrdinal("MiddleInitial");
                var employeeAddress = dr.GetOrdinal("Address");
                var employeeCity = dr.GetOrdinal("City");
                var employeeProvince = dr.GetOrdinal("Province");
                var employeePostalCode = dr.GetOrdinal("PostalCode");
                var employeePhone = dr.GetOrdinal("Phone");
                var employeeCellPhone = dr.GetOrdinal("CellPhone");
                var employeeEmail = dr.GetOrdinal("Email");
                var employeeDateOfBirth = dr.GetOrdinal("DateOfBirth");
                var employeeSIN = dr.GetOrdinal("SIN");
                var employeeStatus = dr.GetOrdinal("Status");
                var employeeHireDate = dr.GetOrdinal("HireDate");
                var employeeJobStartDate = dr.GetOrdinal("JobStartDate");
                var employeeJobId = dr.GetOrdinal("JobId");
                var employeeJobTitle = dr.GetOrdinal("Title");
                var employeeDepartment = dr.GetOrdinal("Department");
                var employeeSalary = dr.GetOrdinal("Salary");
                var employeeSalaryEffectiveDate = dr.GetOrdinal("SalaryEffectiveDate");
                var employeePrevSalary = dr.GetOrdinal("PrevSalary");

                while (dr.Read())
                {
                    var employee = new Employee();
                    employee.employeeId = Convert.ToInt32(dr[employeeEmployeeId]);
                    employee.firstName = Convert.ToString(dr[employeeFirstName]);
                    employee.lastName = Convert.ToString(dr[employeeLastName]);
                    if (dr[employeeMiddleInitial] != DBNull.Value) employee.middleInitial = Convert.ToString(dr[employeeMiddleInitial]);
                    employee.address = Convert.ToString(dr[employeeAddress]);
                    employee.city = Convert.ToString(dr[employeeCity]);
                    employee.province = Convert.ToString(dr[employeeProvince]);
                    employee.postalCode = Convert.ToString(dr[employeePostalCode]);
                    employee.workPhone = Convert.ToString(dr[employeePhone]);
                    employee.cellPhone = Convert.ToString(dr[employeeCellPhone]);
                    employee.email = Convert.ToString(dr[employeeEmail]);
                    employee.dateOfBirth = Convert.ToDateTime(dr[employeeDateOfBirth]);
                    employee.sin = Convert.ToString(dr[employeeSIN]);
                    employee.status = Convert.ToInt32(dr[employeeStatus]);
                    employee.hireDate = Convert.ToDateTime(dr[employeeHireDate]);
                    employee.jobStartDate = Convert.ToDateTime(dr[employeeJobStartDate]);
                    employee.jobId = Convert.ToInt32(dr[employeeJobId]);
                    employee.jobTitle = Convert.ToString(dr[employeeJobTitle]);
                    employee.department = (Department)(dr[employeeDepartment]);
                    employee.salary = Convert.ToDecimal(dr[employeeSalary]);
                    employee.salaryEffectiveDate = Convert.ToDateTime(dr[employeeSalaryEffectiveDate]);
                    if (dr[employeePrevSalary] != DBNull.Value) employee.prevSalary = Convert.ToDecimal(dr[employeePrevSalary]);

                    employeeList.Add(employee);
                }
            }
            return employeeList;
        }

        
        public static bool Update(Employee employee)
        {
            return EmployeeSQL.Update(employee);
        }

        public static bool UpdateStatus(Employee employee)
        {
            return EmployeeSQL.UpdateStatus(employee);
        }
    }
    
}
