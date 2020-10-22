using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bismark.BOL;

namespace Bismark
{
    public partial class ViewPayStub : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AuthLevel"] == null)
                Response.Redirect("Login.aspx?ref=ViewPayStub", true);
             //Create the paystub object
            var stubId = Convert.ToInt32(Request.QueryString["id"]);
            var paystub = PayStub.Create(stubId);

            if (Convert.ToInt32(Session["EmployeeId"]) == paystub.EmployeeId)
            {
                lblEmpName.Text = Session["EmployeeName"].ToString();
                lblEmpId.Text = paystub.EmployeeId.ToString();
                lblDept.Text = Session["Department"].ToString();
                lblPeriodEnd.Text = paystub.PayPeriod.ToShortDateString();
                lblJobAssignment.Text = Session["JobTitle"].ToString();

                lblSalaryAmt.Text = paystub.GrossPay.ToString("n");
                lblSalaryYTDAmt.Text = paystub.YTDGrossPay.ToString("n");

                lblBonusAmt.Text = paystub.BonusPay.ToString("n");
                lblBonusYTDAmt.Text = paystub.YTDBonusPay.ToString("n");

                lblIncomeTax.Text = paystub.IncomeTaxDeduction.ToString("n");
                lblIncomeTaxYTD.Text = paystub.YTDIncomeTaxDeduction.ToString("n");

                lblEI.Text = paystub.EIDeduction.ToString("n");
                lblEIYTD.Text = paystub.YTDEIDeduction.ToString("n");

                lblCPP.Text = paystub.CPPDeduction.ToString("n");
                lblCPPYTD.Text = paystub.YTDCPPDeduction.ToString("n");

                lblPension.Text = paystub.PensionDeduction.ToString("n");
                lblPensionYTD.Text = paystub.YTDPensionDeduction.ToString("n");

                lblCurrGross.Text = (paystub.GrossPay + paystub.BonusPay).ToString("n");
                lblCurrDed.Text = (paystub.IncomeTaxDeduction + paystub.EIDeduction + paystub.CPPDeduction + paystub.PensionDeduction).ToString("n");
                lblCurrNet.Text = paystub.NetPay.ToString("n");

                lblYTDGross.Text = (paystub.YTDGrossPay + paystub.YTDBonusPay).ToString("n");
                lblYTDDed.Text = (paystub.YTDIncomeTaxDeduction + paystub.YTDEIDeduction + paystub.YTDCPPDeduction + paystub.YTDPensionDeduction).ToString("n");
                lblYTDNet.Text = paystub.YTDNetPay.ToString("n");

                lblAllocationAmt.Text = paystub.NetPay.ToString("n");
            }
        }
    }
}