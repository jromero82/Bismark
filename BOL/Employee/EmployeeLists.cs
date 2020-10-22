using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.Utilities;
using Bismark.SQL;

namespace Bismark.BOL
{
    public class EmployeeLists
    {
        public static List<Employee> GetAllActive()
        {
            return Employee.RePackage(EmployeeSQL.GetAllActive());
        }

        public static List<Employee> GetByDept(Department dept)
        {
            return Employee.RePackage(EmployeeSQL.GetByDept(dept));
        }

        public static List<Employee> GetByName(string firstName = null, string lastName = null)
        {
            return Employee.RePackage(EmployeeSQL.GetByName(firstName, lastName));
        }

        public static List<Employee> GetEmployeeLookup(Department department)
        {
            return Employee.RePackage(EmployeeSQL.GetEmployeeLookup(department), EmployeeGetType.Lookup);
        }

        public static List<Employee> GetAll()
        {
            return Employee.RePackage(EmployeeSQL.GetAll());
        }
    }
}
