using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bismark.BOL;
using System.Web.Services;
using System.Data;

namespace Bismark.WebForms
{
    public partial class EmployeeInquire : System.Web.UI.Page
    {
        private Employee emp;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AuthLevel"] == null)
                Response.Redirect("Login.aspx?ref=EmployeeInquiry", true);
            
            emp = loadEmployee(Convert.ToInt32(Session["EmployeeId"]));

            txtDateRangeA.Attributes.Add("readonly", "readonly");
            txtDateRangeB.Attributes.Add("readonly", "readonly");

            txtEmpId.Text = emp.EmployeeId.ToString();
            txtFname.Text = emp.FirstName;
            txtLName.Text = emp.LastName;
            txtMiddleInitial.Text = emp.MiddleInitial;
            txtAddress.Text = emp.Address;
            txtCity.Text = emp.City;
            txtProvince.Text = emp.Province;
            txtPostalCode.Text = emp.PostalCode;

            string status = string.Empty;
            switch (emp.Status)
            {
                case 1:
                    status = "Active";
                    break;
                case 2:
                    status = "Retired";
                    break;
                case 3:
                    status = "Terminated";
                    break;
            }

            txtEmpStatus.Text = status;

            txtDOB.Text = emp.DateOfBirth.ToShortDateString();
            txtSIN.Text = emp.SIN;
            txtJobTitle.Text = emp.JobTitle;
            txtDept.Text = emp.Department.ToString();
            txtJobStartDate.Text = emp.JobStartDate.ToShortDateString();
            txtHireDate.Text = emp.HireDate.ToShortDateString();
            txtSupervisor.Text = emp.Supervisor;
            lblSickDays.Text = "Sick days: " + emp.SickDays.Count;
            
            //Get paystubs
            var stubs = new List<PayStub>();
            stubs = PayStubLists.GetPayStubsByEmpId(emp.EmployeeId);

            txtYTDSalary.Text = (stubs[0].YTDGrossPay + stubs[0].YTDBonusPay).ToString("c");
            txtYTDBonus.Text = stubs[0].YTDBonusPay.ToString("c");
            txtYTDDeductions.Text = (stubs[0].YTDIncomeTaxDeduction + stubs[0].YTDEIDeduction
                                        + stubs[0].YTDPensionDeduction + stubs[0].YTDCPPDeduction).ToString("c");
            
            //Load sick day detail
            var sickdays = emp.SickDays;
            lblSickDayDetail.Text = string.Empty;
            foreach (SickDay s in sickdays)
            {
                var day = "";
                if (s.IsFullDay == true)
                {
                    day = "Full Day";
                }
                else
                {
                    day = "Half Day";
                }
                lblSickDayDetail.Text += s.Date.ToShortDateString() + " - " + day + "<br />";
            }

        }

        private Employee loadEmployee(int empId)
        {
            return Employee.Create(empId);
        }      
        

        [WebMethod]
        public static List<PayStub> getPayStubs(int empId, DateTime dateFrom, DateTime dateTo)
        {
            return PayStubLists.GetPayStubsByDateRange(empId, dateFrom, dateTo);
        }

        [WebMethod]
        public static List<Pension.Year> getFiveHighestPaidYears(int empId)
        {
            return Pension.GetDetails(empId);
        }

        protected void btnCalcPension_Click(object sender, EventArgs e)
        {            

            lblPensionDetails.Text = string.Empty;
            lblHireDate.Text = string.Empty;

            var years = Pension.GetDetails(emp.EmployeeId);
            decimal fullPension = ((years[0].Amount + years[1].Amount + years[2].Amount + years[3].Amount + years[4].Amount) / 5) * .7m;

            var retireDateFullPension = emp.DateOfBirth.AddYears(60);

            lblPensionDetails.Text = "Age: 55 - Penalty: 15% - Amount: " + (fullPension - (fullPension * .15m)).ToString("c") + "<br/>" +
                                     "Age: 56 - Penalty: 12% - Amount: " + (fullPension - (fullPension * .12m)).ToString("c") + "<br/>" +
                                     "Age: 57 - Penalty: 09% - Amount: " + (fullPension - (fullPension * .09m)).ToString("c") + "<br/>" +
                                     "Age: 58 - Penalty: 06% - Amount: " + (fullPension - (fullPension * .06m)).ToString("c") + "<br/>" +
                                     "Age: 59 - Penalty: 03% - Amount: " + (fullPension - (fullPension * .03m)).ToString("c") + "<br/>" +
                                     "Age: 60 - Penalty: 00% - Amount: " + fullPension.ToString("c") + "<br/><br/>";

            lblFullPensionRetirementDate.Text = "Retirement date for full pension: " + retireDateFullPension.ToShortDateString();

            lblhighestYears.Text = "<h4>Highest Paid Years</h4>" +
                                   years[0].Name + " - " + years[0].Amount.ToString("c") + "<br />" +
                                   years[1].Name + " - " + years[1].Amount.ToString("c") + "<br />" +
                                   years[2].Name + " - " + years[2].Amount.ToString("c") + "<br />" +
                                   years[3].Name + " - " + years[3].Amount.ToString("c") + "<br />" +
                                   years[4].Name + " - " + years[4].Amount.ToString("c") + "<br />";
        }

        protected void btnClear2_Click(object sender, EventArgs e)
        {
            lblPensionDetails.Text = string.Empty;
            lblFullPensionRetirementDate.Text = string.Empty;
            lblhighestYears.Text = string.Empty;
        }
    }
}