using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bismark.BOL;

namespace Bismark.GUI
{
    public partial class frmViewPayStub : Form
    {

        public PayStub Stub { get; set; }

        public frmViewPayStub()
        {
            InitializeComponent();
        }        

        private void frmViewPayStub_Load(object sender, EventArgs e)
        {
            Employee emp;
            if (Stub == null)
            {
                MessageBox.Show("No paystub to display.");
                this.Close();
            }
            else
            {
                emp = getEmployee(Stub.EmployeeId);
                writePayStub(emp);
            }
        }

        private Employee getEmployee(int id)
        {
            return Employee.Create(id);
        }

        private void writePayStub(Employee emp)
        {
            lblEmployeeName.Text = emp.LastName + ", " + emp.FirstName + " " + emp.MiddleInitial;
            lblEmployeeID.Text = emp.EmployeeId.ToString();
            lblDept.Text = emp.Department.ToString();
            lblJob.Text = emp.JobTitle;
            lblPayPeriod.Text = Stub.PayPeriod.ToShortDateString();
            lblSalaryAmount.Text = Stub.GrossPay.ToString("F");
            lblSalaryAmountYTD.Text = Stub.YTDGrossPay.ToString("F");
            if (Stub.BonusPay != 0)
            {
                lblBonus.Show();
                lblBonusAmount.Text = Stub.BonusPay.ToString("F");
                lblBonusAmountYTD.Text = Stub.YTDBonusPay.ToString("F");
            }
            else
            {
                lblBonus.Hide();
                lblBonusAmount.Text = string.Empty;
                lblBonusAmountYTD.Text = string.Empty;
            }

            lblTax.Text = Stub.IncomeTaxDeduction.ToString("F");
            lblTaxYTD.Text = Stub.YTDIncomeTaxDeduction.ToString("F");
            lblEI.Text = Stub.EIDeduction.ToString("F");
            lblEIYTD.Text = Stub.YTDEIDeduction.ToString("F");
            lblCPP.Text = Stub.CPPDeduction.ToString("F");
            lblCPPYTD.Text = Stub.YTDCPPDeduction.ToString("F");
            lblPension.Text = Stub.PensionDeduction.ToString("F");
            lblPensionYTD.Text = Stub.YTDPensionDeduction.ToString("F");

            lblGrossPay.Text = (Stub.GrossPay + Stub.BonusPay).ToString("F");
            lblGrossPayYTD.Text = (Stub.YTDGrossPay + Stub.YTDBonusPay).ToString("F");
            lblDeductions.Text = (Stub.IncomeTaxDeduction + Stub.EIDeduction + Stub.CPPDeduction + Stub.PensionDeduction).ToString("F");
            lblDeductionsYTD.Text = (Stub.YTDIncomeTaxDeduction + Stub.YTDEIDeduction + Stub.YTDCPPDeduction + Stub.YTDPensionDeduction).ToString("F");

            lblNetPay.Text = Stub.NetPay.ToString("F");
            lblNetPayYTD.Text = Stub.YTDNetPay.ToString("F");

            lblDeposit.Text = Stub.NetPay.ToString("F");

        }   



    }
}
