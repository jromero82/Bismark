using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bismark.Utilities;
using Bismark.BOL;

namespace Bismark.GUI
{
    public partial class BonusForm : Form
    {
        //Form level variables to hold lists
        private List<Employee> employeeList;
        private BindingList<SalaryAdjustment> salaryIncreasePending;
        private List<SalaryAdjustment> salaryIncHistory;
        private BindingList<Bonus> bonusPending;
        private List<Bonus> bonusHistory;
        private List<PayStub> payStubs;

        public BonusForm()
        {
            InitializeComponent();           
        }

        #region "Startup"

        private void BonusForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.panel1.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.btnClose.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                cmbSearch.SelectedIndex = 0;
                cmbBonusType.SelectedIndex = 0;
                txtFixedBonus.Hide();
                cmbEffectiveDate.Items.Add("-- SELECT DATE --");
                cmbEffectiveDate.SelectedIndex = 0;
                loadEffectiveDates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
              
        }

        private void loadEffectiveDates()
        {           
            var effectiveDate = new DateTime();
            effectiveDate = Payroll.GetCurrentPayPeriodStartDate();

            for (int i = 0; i < 12; i++) 
            {
                cmbEffectiveDate.Items.Add((String.Format("{0: MMMM dd, yyyy}", effectiveDate)).ToUpper());
                effectiveDate = effectiveDate.AddDays(14);
            }

        }
        #endregion

        #region "Search"

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadEmployees();
        }         

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbSearch.SelectedIndex)
            {
                case 0:         //Employee ID
                    pnlEmpId.Show();
                    pnlName.Hide();
                    pnlDept.Hide();                   
                    break;
                case 1:         //Employee name
                    pnlName.Show();
                    pnlEmpId.Hide();
                    pnlDept.Hide();
                    break;
                case 2:         //Department
                    pnlDept.Show();
                    pnlEmpId.Hide();
                    pnlName.Hide();
                    cmbDept.DataSource = Enum.GetNames(typeof(Department));
                    break;
                case 3:         //All
                    pnlEmpId.Hide();
                    pnlName.Hide();
                    pnlDept.Hide();
                    break;
            }
        }                

        #endregion
        
        #region "Employee Related"

        private void loadEmployees()
        {
            employeeList = null;
            employeeList = new List<Employee>();
            //Determine what search criteria has been entered and 
            //fill the employeeList with the results
            switch (cmbSearch.SelectedIndex)
            {
                case 0: //Get employee by ID
                    employeeList.Add(Employee.Create(Convert.ToInt32(txtEmpIdSearch.Text)));
                    break;
                case 1: //Get employees by name
                    string fname = null, lname = null;
                    if (txtFirstNameSearch.Text != "") { fname = txtFirstNameSearch.Text; }
                    if (txtLastNameSearch.Text != "") { lname = txtLastNameSearch.Text; }
                    employeeList = EmployeeLists.GetByName(fname,lname);
                    break;
                case 2: //Get employees by department
                    employeeList = EmployeeLists.GetByDept((Department)cmbDept.SelectedIndex + 1);
                    break;
                case 3: //Get all employees
                    employeeList = EmployeeLists.GetAllActive();
                    break;
            }

            dgvEmployees.DataSource = null;           
            dgvEmployees.DataSource = employeeList;   

            var col = dgvEmployees.Columns;            
            
            if(!col.Contains("Chk"))
            {
                //Add checkbox column
                var colCheckBox = new DataGridViewCheckBoxColumn();              
                colCheckBox.Name = "Chk";
                colCheckBox.HeaderText = "Select";
                colCheckBox.DisplayIndex = 0;
                col.Add(colCheckBox);
            }

            if (!col.Contains("EmployeeName"))
            {
                //Employee name column for full name            
                var colEmpName = new DataGridViewTextBoxColumn();
                colEmpName.Name = "EmployeeName";
                colEmpName.HeaderText = "Employee";
                colEmpName.DisplayIndex = 1;            
                col.Add(colEmpName);
            }
        

            foreach (DataGridViewRow row in dgvEmployees.Rows)
            {
                row.Cells["EmployeeName"].Value = (string)row.Cells["LastName"].Value + ", "
                                                + (string)row.Cells["FirstName"].Value + " "
                                                + (string)row.Cells["MiddleInitial"].Value;
            }
            
            //Enable readonly for all columns except the checkbox column
            foreach (DataGridViewColumn cols in dgvEmployees.Columns)
            {
                if (cols.Name != "Chk")
                {
                    cols.ReadOnly = true;
                }
            }

            //Set unneeded fields to false
            col["EmployeeId"].Visible = false;
            col["Address"].Visible = false;
            col["City"].Visible = false;
            col["Province"].Visible = false;
            col["PostalCode"].Visible = false;
            col["DateOfBirth"].Visible = false;
            col["SIN"].Visible = false;
            col["JobId"].Visible = false;
            col["Salary"].Visible = false;
            col["CellPhone"].Visible = false;
            col["SalaryEffectiveDate"].Visible = false;
            col["PrevSalary"].Visible = false;
            col["JobAssignment"].Visible = false;
            col["Status"].Visible = false;
            col["HireDate"].Visible = false;
            col["MiddleInitial"].Visible = false;
            col["JobStartDate"].Visible = false;
            col["FirstName"].Visible = false;
            col["LastName"].Visible = false;
            col["Supervisor"].Visible = false;

            col["JobTitle"].HeaderText = "Title";            
            
            col["Chk"].Width = 50;

        }

        private void showEmployeeDetails(Employee employee)
        {
            //Fill personal details
            txtEmpIdDisplay.Text = employee.EmployeeId.ToString();
            txtFirstName.Text = employee.FirstName;
            txtMiddleInital.Text = employee.MiddleInitial;
            txtLastName.Text = employee.LastName;
            txtAddress.Text = employee.Address;
            txtCity.Text = employee.City;
            txtProvince.Text = employee.Province;
            txtPostalCode.Text = employee.PostalCode;
            txtEmail.Text = employee.Email;
            txtWorkPhone.Text = employee.WorkPhone;
            txtCellPhone.Text = employee.CellPhone;

            //Fill employment details
            if (employee.Status == 1)
            {
                txtStatus.Text = "Active";
                txtStatus.BackColor = Color.ForestGreen;
            }
            else
            {
                txtStatus.Text = "Inactive";
            }

            txtJobTitle.Text = employee.JobTitle;
            txtDept.Text = employee.Department.ToString();
            txtJobStartDate.Text = employee.JobStartDate.ToShortDateString();
            txtHireDate.Text = employee.HireDate.ToShortDateString();
            txtDateOfBirth.Text = employee.DateOfBirth.ToShortDateString();
            txtSIN.Text = employee.SIN.ToString();
            txtSupervisor.Text = employee.Supervisor;

            //Get sick day info
            dgvSickDays.AutoGenerateColumns = false;
            dgvSickDays.DataSource = employee.SickDays;
            tabSickDay.Text = "Sick Days " + "(" + dgvSickDays.Rows.Count + ")";

            //Payroll Info
            txtSalary.Text = string.Format("{0:c}", employee.Salary / 26);
            txtSalaryCap.Text = string.Format("{0:c}", employee.JobAssignment.MaxSalary / 26);
            txtPrevSalary.Text = string.Format("{0:c}", employee.PrevSalary);
            txtSalaryChangeDate.Text = employee.SalaryEffectiveDate.ToShortDateString();

            payStubs = PayStubLists.GetPayStubsByEmpId(employee.EmployeeId);
            dgvPaystub.AutoGenerateColumns = false;
            dgvPaystub.DataSource = payStubs;
            dgvPaystub.Columns["NetPay"].DefaultCellStyle.Format = "c";
            dgvPaystub.Columns["NetPay"].DefaultCellStyle.Alignment = 
                DataGridViewContentAlignment.MiddleRight;

            if (payStubs.Count > 0)
            {
                var p = payStubs[0];
                txtYTDGross.Text = string.Format("{0:c}", p.YTDGrossPay + p.YTDBonusPay);
                txtYTDDed.Text = string.Format("{0:c}", p.YTDIncomeTaxDeduction + p.YTDCPPDeduction
                                                        + p.YTDEIDeduction + p.YTDPensionDeduction);

                txtYTDNet.Text = string.Format("{0:c}", p.YTDNetPay);

                //YTD Tab
                txtYTDSalary.Text = string.Format("{0:c}", p.YTDGrossPay);
                txtYTDBonus.Text = string.Format("{0:c}", p.YTDBonusPay);
                txtYTDGross2.Text = txtYTDGross.Text;
                txtYTDIncomeTax.Text = string.Format("{0:c}", p.YTDIncomeTaxDeduction);
                txtYTDCpp.Text = string.Format("{0:c}", p.YTDCPPDeduction);
                txtYTDEI.Text = string.Format("{0:c}", p.YTDEIDeduction);
                txtYTDPenDed.Text = string.Format("{0:c}", p.YTDPensionDeduction);
            }
            else
            {
                txtYTDGross.Text = "N/A";
                txtYTDDed.Text = "N/A";
                txtYTDNet.Text = "N/A";

                //YTD Tab
                txtYTDSalary.Text = "N/A";
                txtYTDBonus.Text = "N/A";
                txtYTDGross2.Text = "N/A";
                txtYTDIncomeTax.Text = "N/A"; ;
                txtYTDCpp.Text = "N/A";
                txtYTDEI.Text = "N/A";
                txtYTDPenDed.Text = "N/A";
            }

            //Load salary increase details
            salaryIncreasePending = new BindingList<SalaryAdjustment>(employee.SalaryAdjustmentsPending);
            dgvSalaryIncrease.AutoGenerateColumns = false;            
            dgvSalaryIncrease.DataSource = salaryIncreasePending;

            salaryIncHistory = employee.SalaryAdjustmentsHistory;
            dgvSalaryIncHistory.AutoGenerateColumns = false;
            dgvSalaryIncHistory.DataSource = salaryIncHistory;
            
            //Load Bonus details
            bonusPending = new BindingList<Bonus>(employee.BonusPending);
            dgvBonusPending.AutoGenerateColumns = false;
            dgvBonusPending.DataSource = bonusPending;

            bonusHistory = employee.BonusHistory;
            dgvBonusHistory.AutoGenerateColumns = false;
            dgvBonusHistory.DataSource = bonusHistory;

        }

        private void dgvEmployees_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            showEmployeeDetails(employeeList[e.RowIndex]);
        }

        private void dgvEmployees_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEmployees.Columns[e.ColumnIndex].Name == "Phone")
            {
                String phone = (string)e.Value;
                e.Value = '(' + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + '-' + phone.Substring(6, 4);
                e.FormattingApplied = true;
            }         

        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool checkState = chkSelectAll.Checked;

            foreach (DataGridViewRow row in dgvEmployees.Rows)
            {
                row.Cells["Chk"].Value = checkState;
            }
        }

        #endregion

        #region "Salary Changes"

        private void dgvSalaryIncrease_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvSalaryIncrease.Columns["colRemovePending"].Index)
                {
                    DialogResult = MessageBox.Show("Are you sure you want to remove this salary increase?", "Confirm", MessageBoxButtons.OKCancel);
                    if (DialogResult == DialogResult.OK)
                    {
                        //Delete the salary increase
                        if (SalaryAdjustmentCUD.DeleteSingleEmployeeSalaryAdjustment(
                            Convert.ToInt32(txtEmpIdDisplay.Text), salaryIncreasePending[e.RowIndex].SalaryAdjustmentId))
                        {
                            salaryIncreasePending.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        #endregion

        #region "Bonus Related"

        private void btnApplyBonus_Click(object sender, EventArgs e)
        {            
            try
            {
                //Gets the employee IDs selected in the grid
                var empIdList = new List<int>();
                foreach (DataGridViewRow row in dgvEmployees.Rows)
                {
                    if (row.Cells["Chk"].Value != null && (bool)row.Cells["Chk"].Value == true)
                    {
                        empIdList.Add(employeeList[row.Index].EmployeeId);
                    }
                }

                if (empIdList.Count == 0)
                {
                    throw new DataException("No employees selected.");
                }

                //Validates the entry
                checkBonusEntry();

                //Create Bonus
                var BonusObj = Bonus.Create();

                 //Get the effective date
                var effectiveDate = Convert.ToDateTime(cmbEffectiveDate.SelectedItem);
                BonusObj.EffectiveDate = effectiveDate;

                if (cmbBonusType.SelectedIndex == 0) //Percent
                {
                    //Gets the percent increase amount
                    var increasePercent = nudPercent.Value / 100;
                    BonusObj.PercentBonus = increasePercent;
                }
                else
                {
                    //Gets the fixed amount
                    var fixedAmount = Convert.ToDecimal(txtFixedBonus.Text);
                    BonusObj.FixedBonus = fixedAmount;
                }

                //Apply increase
                if (BonusCUD.Create(empIdList, BonusObj))
                {
                    MessageBox.Show("Bonus applied successful.");
                    bonusPending.Add(BonusObj);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }        

        private void cmbBonusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBonusType.SelectedIndex == 0) //Percent
            {
                txtFixedBonus.Hide();
                nudPercent.Show();
                lblPercent.Hide();
            }
            else //Fixed
            {
                txtFixedBonus.Show();
                nudPercent.Hide();
                lblPercent.Show();
            }
        }

        private void dgvBonusPending_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Format Bonus Gridview
            foreach (DataGridViewRow row in dgvBonusPending.Rows)
            {
                if (bonusPending[row.Index].PercentBonus > 0)
                {
                    row.Cells["colBonus"].Value = bonusPending[row.Index].PercentBonus;                    
                }
                else
                {
                    row.Cells["colBonus"].Value = bonusPending[row.Index].FixedBonus;
                }

            }
        }

        private void dgvBonusPending_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (bonusPending.Count > 0)
            {
                if (dgvBonusPending.Columns[e.ColumnIndex].Name == "colBonus")
                {
                    if (bonusPending[e.RowIndex].PercentBonus > 0)
                    {
                        e.CellStyle.Format = "#0.##%";
                    }
                    else
                    {
                        e.CellStyle.Format = "c";
                    }
                }
            }
        }       

        private void dgvBonusPending_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvBonusPending.Columns["DeleteBonus"].Index)
                {
                    DialogResult = MessageBox.Show("Are you sure you want to remove this bonus?", "Confirm", MessageBoxButtons.OKCancel);
                    if (DialogResult == DialogResult.OK)
                    {
                        //Delete the bonus
                        if (BonusCUD.DeleteSingleBonusByEmpId(
                            Convert.ToInt32(txtEmpIdDisplay.Text), bonusPending[e.RowIndex].BonusId))
                        {
                            bonusPending.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvBonusHistory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Format Bonus Gridview
            foreach (DataGridViewRow row in dgvBonusHistory.Rows)
            {
                if (bonusHistory[row.Index].PercentBonus > 0)
                {
                    row.Cells["colHistoryBonus"].Value = bonusHistory[row.Index].PercentBonus;
                }
                else
                {
                    row.Cells["colHistoryBonus"].Value = bonusHistory[row.Index].FixedBonus;
                }

            }
        }

        private void dgvBonusHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (bonusHistory.Count > 0)
            {
                if (dgvBonusHistory.Columns[e.ColumnIndex].Name == "colHistoryBonus")
                {
                    if (bonusHistory[e.RowIndex].PercentBonus > 0)
                    {
                        e.CellStyle.Format = "#0.##%";
                    }
                    else
                    {
                        e.CellStyle.Format = "c";
                    }
                }
            }
            
        }
           
        #endregion

        #region "Paystub"
        private void dgvPaystub_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPaystub.Columns["View"].Index)
            {
                var viewPayStub = new frmViewPayStub();
                viewPayStub.Stub = payStubs[e.RowIndex];
                viewPayStub.ShowDialog();
            }
        }
        #endregion

        #region "Validation"

        private void checkBonusEntry()
        {
            var number = new Decimal();

            if (dgvEmployees.RowCount == 0)
            {
                throw new DataException("No employees selected. Search for an employee to select");
            }
            else if (cmbBonusType.SelectedIndex == 0 && nudPercent.Value == 0)
            {
                throw new DataException("Percentage of bonus must be greater than 0");
            }
            else if (cmbBonusType.SelectedIndex == 1 && txtFixedBonus.Text == string.Empty)
            {
                throw new DataException("Fixed bonus amount empty. Please enter a bonus amount.");
            } 
            else if(cmbBonusType.SelectedIndex == 1 && !decimal.TryParse(txtFixedBonus.Text.Trim(), out number))
            {
                throw new DataException("Invalid entry. Fixed amount must be numeric.");
            }
            else if (cmbEffectiveDate.SelectedValue == string.Empty)
            {
                throw new DataException("Please select an effective date.");
            }
            
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
   
}
