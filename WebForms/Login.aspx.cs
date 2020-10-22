using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bismark.BOL;

namespace Bismark.WebForms
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var employeeId = txtEmployeeId.Text.Trim();
                var password = txtPassword.Text.Trim();
                if (!string.IsNullOrEmpty(employeeId) &&
                    !string.IsNullOrEmpty(password))
                {
                    var authLevel = Employee.Login(Convert.ToInt32(employeeId), password);
                    Session["AuthLevel"] = authLevel;
                    var employee = Employee.Create(Convert.ToInt32(employeeId));
                    Session["EmployeeId"] = employee.EmployeeId;
                    Session["EmployeeName"] = employee.LastName + ", " + employee.FirstName + " " + employee.MiddleInitial;
                    Session["Department"] = employee.Department;
                    Session["JobTitle"] = employee.JobTitle;
                    Session["Supervisor"] = employee.Supervisor;
                    Session["Email"] = employee.Email;
                    if (Request["ref"] != null)
                        Response.Redirect(Request["ref"].ToString() + ".aspx");
                    Response.Redirect("EmployeeInquiry.aspx", true);
                }
                else
                    throw new Exception("Both Employee Id and Password are required.");
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message;
                loginError.Style["display"] = "block";
            }
        }
    }
}