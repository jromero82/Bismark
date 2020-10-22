using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Bismark.Data;
using Bismark.Utilities;
using System.Data;

namespace Bismark.SQL
{
    public class EmployeeSQL
    {
        public static SqlDataReader GetAll()
        {
            return DAL.GetDataReader("GetAllEmployees");
        }

        public static SqlDataReader GetAllActive()
        {
            return DAL.GetDataReader("GetAllActiveEmployees");
        }

        public static SqlDataReader GetByDept(Department dept)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@deptId", dept, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetEmployeesByDept",parameterList);
        }

        public static SqlDataReader GetById(int employeeId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetEmployeeById", parameterList);
        }

        public static SqlDataReader GetByName(string firstName = null, string lastName = null)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@firstname", firstName, ParameterDirection.Input, SqlDbType.VarChar, 25));
            parameterList.Add(new DataParameter("@lastName", lastName, ParameterDirection.Input, SqlDbType.VarChar, 25));
            
            return DAL.GetDataReader("GetEmployeeByName", parameterList);
        }

        public static SqlDataReader GetEmployeeLookup(Department department)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@department", department, ParameterDirection.Input, SqlDbType.Int));

            return DAL.GetDataReader("GetEmployeeLookup", parameterList);
        }

        public static SqlDataReader GetSupervisorName(Department department)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@deptId", department, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetDeptSupervisor", parameterList);
        }

        public static SqlDataReader GetSickDays(int employeeId)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@empId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            return DAL.GetDataReader("GetSickDays", parameterList);
        }        

        public static bool Update(IEmployee employee)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@empId", employee.EmployeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@fname", employee.FirstName, ParameterDirection.Input, SqlDbType.VarChar, 25));
            parameterList.Add(new DataParameter("@intial", employee.MiddleInitial, ParameterDirection.Input, SqlDbType.Char, 1));
            parameterList.Add(new DataParameter("@lname", employee.LastName, ParameterDirection.Input, SqlDbType.VarChar, 25));
            parameterList.Add(new DataParameter("@address", employee.Address, ParameterDirection.Input, SqlDbType.VarChar, 50));
            parameterList.Add(new DataParameter("@city", employee.City, ParameterDirection.Input, SqlDbType.VarChar, 20));
            parameterList.Add(new DataParameter("@prov", employee.Province, ParameterDirection.Input, SqlDbType.Char, 2));
            parameterList.Add(new DataParameter("@pc", employee.PostalCode, ParameterDirection.Input, SqlDbType.Char, 6));
            parameterList.Add(new DataParameter("@phone", employee.WorkPhone, ParameterDirection.Input, SqlDbType.Char, 10));
            parameterList.Add(new DataParameter("@cell", employee.CellPhone, ParameterDirection.Input, SqlDbType.Char, 10));
            parameterList.Add(new DataParameter("@email", employee.Email, ParameterDirection.Input, SqlDbType.VarChar, 50));
            parameterList.Add(new DataParameter("@dob", employee.DateOfBirth, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@sin", employee.SIN, ParameterDirection.Input, SqlDbType.Char, 9));
            parameterList.Add(new DataParameter("@status", employee.Status, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@hiredate", employee.HireDate, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@jobstartdate", employee.JobStartDate, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@jobid", employee.JobId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@salary", employee.Salary, ParameterDirection.Input, SqlDbType.Decimal));
            parameterList.Add(new DataParameter("@salaryeffectivedate", employee.SalaryEffectiveDate, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@prevsalary", employee.PrevSalary, ParameterDirection.Input, SqlDbType.Decimal));

            return DAL.SendData("UpdateEmployee", parameterList);

        }

        public static int Insert(IEmployee employee, String password)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@empId", employee.EmployeeId, ParameterDirection.Output, SqlDbType.Int));
            parameterList.Add(new DataParameter("@fname", employee.FirstName, ParameterDirection.Input, SqlDbType.VarChar, 25));
            parameterList.Add(new DataParameter("@intial", employee.MiddleInitial, ParameterDirection.Input, SqlDbType.Char, 1));
            parameterList.Add(new DataParameter("@lname", employee.LastName, ParameterDirection.Input, SqlDbType.VarChar, 25));
            parameterList.Add(new DataParameter("@address", employee.Address, ParameterDirection.Input, SqlDbType.VarChar, 50));
            parameterList.Add(new DataParameter("@city", employee.City, ParameterDirection.Input, SqlDbType.VarChar, 20));
            parameterList.Add(new DataParameter("@prov", employee.Province, ParameterDirection.Input, SqlDbType.Char, 2));
            parameterList.Add(new DataParameter("@pc", employee.PostalCode, ParameterDirection.Input, SqlDbType.Char, 6));
            parameterList.Add(new DataParameter("@phone", employee.WorkPhone, ParameterDirection.Input, SqlDbType.Char, 10));
            parameterList.Add(new DataParameter("@cell", employee.CellPhone, ParameterDirection.Input, SqlDbType.Char, 10));
            parameterList.Add(new DataParameter("@email", employee.Email, ParameterDirection.Input, SqlDbType.VarChar, 50));
            parameterList.Add(new DataParameter("@dob", employee.DateOfBirth, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@sin", employee.SIN, ParameterDirection.Input, SqlDbType.Char, 9));                       
            parameterList.Add(new DataParameter("@jobstartdate", employee.JobStartDate, ParameterDirection.Input, SqlDbType.Date));
            parameterList.Add(new DataParameter("@jobid", employee.JobId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@salary", employee.Salary, ParameterDirection.Input, SqlDbType.Decimal));
            parameterList.Add(new DataParameter("@password", password, ParameterDirection.Input, SqlDbType.VarChar, 12));
                        
           DAL.SendData("InsertEmployee", parameterList);
           return Convert.ToInt32(parameterList[0].Value);

        }

        public static bool UpdateStatus(IEmployee employee)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@empId", employee.EmployeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@status", employee.Status, ParameterDirection.Input, SqlDbType.Int));

            return DAL.SendData("UpdateEmployeeStatus", parameterList);
        }

        public static int Login(int employeeId, string password)
        {
            var parameterList = new List<DataParameter>();
            parameterList.Add(new DataParameter("@authLevel", 0, ParameterDirection.Output, SqlDbType.Int));
            parameterList.Add(new DataParameter("@employeeId", employeeId, ParameterDirection.Input, SqlDbType.Int));
            parameterList.Add(new DataParameter("@password", password, ParameterDirection.Input, SqlDbType.VarChar, 12));

            DAL.SendData("GetLoginAuthorization", parameterList);
            return Convert.ToInt32(parameterList[0].Value);
        }
    }
}
