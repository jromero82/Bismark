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
    public partial class ModifyEmployee : Form
    {
        //Form level variables to hold lists
        private List<Employee> employeeList;
        private BindingList<SalaryAdjustment> salaryIncreasePending;
        private List<SalaryAdjustment> salaryIncHistory;
        private BindingList<Bonus> bonusPending;
        private List<Bonus> bonusHistory;
        private List<PayStub> payStubs;
        private int errorCount;

        public ModifyEmployee()
        {
            InitializeComponent();
        }

        #region "Startup"

        private void ModifyEmployee_Load(object sender, EventArgs e)
        {
            try
            {
                this.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.panel1.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.btnClose.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.grpPersonal.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.grpEmployment.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.btnSavePersonal.Hide();
                cmbSearch.SelectedIndex = 0;
                btnCancelPersonal.Hide();
                cmbStatus.DataSource = Enum.GetNames(typeof(EmploymentStatus));
                cmbDept2.DataSource = Enum.GetNames(typeof(Department));
                cmbDept2.Enabled = false;
                cmbStatus.Enabled = false;
                cmbJobTitle.Enabled = false;
                dtpJobStartDate.Enabled = false;
                dtpDOB.Enabled = false;
                txtSIN.ReadOnly = true;

                btnSaveEmployInfo.Hide();
                btnCancelEmployInfo.Hide();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    employeeList = EmployeeLists.GetByName(fname, lname);
                    break;
                case 2: //Get employees by department
                    employeeList = EmployeeLists.GetByDept((Department)cmbDept.SelectedIndex + 1);
                    break;
                case 3: //Get all employees
                    employeeList = EmployeeLists.GetAll();
                    break;
            }

            dgvEmployees.DataSource = null;
            dgvEmployees.DataSource = employeeList;

            var col = dgvEmployees.Columns;


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
            col["CellPhone"].Visible = false;
            col["SIN"].Visible = false;
            col["JobId"].Visible = false;
            col["Salary"].Visible = false;
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

            //col["Chk"].Width = 50;           

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

            cmbStatus.SelectedIndex = employee.Status;

            //if (employee.Status == 0) // Terminated
            //{
            //    cmbStatus.SelectedIndex = 2;
            //    //cmbStatus.BackColor = Color.DarkRed;                
            //}
            //else if (employee.Status == 1) // Active
            //{
            //    cmbStatus.SelectedIndex = 0;
            //    //cmbStatus.BackColor = Color.ForestGreen;                
            //}
            //else if (employee.Status == 2) // Retired
            //{
            //    cmbStatus.SelectedIndex = 1;
            //    //cmbStatus.BackColor = Color.Gold;
            //}

            //cmbStatus.ForeColor = Color.White;

            cmbDept2.SelectedIndex = (int)employee.Department - 1;
            cmbJobTitle.SelectedValue = employee.JobId;
            dtpJobStartDate.Value = employee.JobStartDate;
            txtHireDate.Text = employee.HireDate.ToShortDateString();
            dtpDOB.Value = employee.DateOfBirth;
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

        private void ValidateInput(object sender, CancelEventArgs e)
        {
            Control control = null;
            if (sender is TextBox)
            {
                control = (TextBox)sender;
            }
            else if (sender is ComboBox)
            {
                control = (ComboBox)sender;
            }
            else if (sender is DateTimePicker)
            {
                control = (DateTimePicker)sender;
            }
            
            try
            {
                switch (control.Name)
                {
                    // Personal INFO
                    case "txtFirstName":
                        employeeList[dgvEmployees.SelectedRows[0].Index].FirstName = txtFirstName.Text;
                        break;
                    case "txtMiddleInitial":
                        employeeList[dgvEmployees.SelectedRows[0].Index].MiddleInitial = txtMiddleInital.Text;
                        break;
                    case "txtLastName":
                        employeeList[dgvEmployees.SelectedRows[0].Index].LastName = txtLastName.Text;
                        break;
                    case "txtAddress":
                        employeeList[dgvEmployees.SelectedRows[0].Index].Address = txtAddress.Text;
                        break;
                    case "txtCity":
                        employeeList[dgvEmployees.SelectedRows[0].Index].City = txtCity.Text;
                        break;
                    case "txtProvince":
                        employeeList[dgvEmployees.SelectedRows[0].Index].Province = txtProvince.Text;
                        break;
                    case "txtPostalCode":
                        employeeList[dgvEmployees.SelectedRows[0].Index].PostalCode = txtPostalCode.Text;
                        break;
                    case "txtWorkPhone":
                        employeeList[dgvEmployees.SelectedRows[0].Index].WorkPhone = txtWorkPhone.Text;
                        break;
                    case "txtCellPhone":
                        employeeList[dgvEmployees.SelectedRows[0].Index].CellPhone = txtCellPhone.Text;
                        break;
                    case "txtEmail":
                        employeeList[dgvEmployees.SelectedRows[0].Index].Email = txtEmail.Text;
                        break;
                    case "cmbJobTitle":
                        employeeList[dgvEmployees.SelectedRows[0].Index].JobId = (int)cmbJobTitle.SelectedValue;
                        break;
                    case "dtpJobStartDate":
                        employeeList[dgvEmployees.SelectedRows[0].Index].JobStartDate = dtpJobStartDate.Value;
                        break;
                    case "dtpDOB":
                        employeeList[dgvEmployees.SelectedRows[0].Index].DateOfBirth = dtpDOB.Value;
                        break;
                    case "txtSIN":
                        employeeList[dgvEmployees.SelectedRows[0].Index].SIN = txtSIN.Text;
                        break;
                }
                errorProvider1.SetError(control, string.Empty);
                if (errorCount > 0) { errorCount--; }
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconPadding(control, -20);
                errorProvider1.SetError(control, ex.Message);
                errorCount++;
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPersonal_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.RowCount == 0)
            {
                MessageBox.Show("No employees selected.");
                return;
            }

            dgvEmployees.Enabled = false;
            btnCancelPersonal.Show();

            txtFirstName.Focus();

            btnJobInfo.Enabled = false;
            btnEmpStatus.Enabled = false;

            txtFirstName.ReadOnly = false;
            txtMiddleInital.ReadOnly = false;
            txtLastName.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtCity.ReadOnly = false;
            txtProvince.ReadOnly = false;
            txtPostalCode.ReadOnly = false;
            txtWorkPhone.ReadOnly = false;
            txtCellPhone.ReadOnly = false;
            txtEmail.ReadOnly = false;

            btnSavePersonal.Show();
        }

        private void btnSavePersonal_Click(object sender, EventArgs e)
        {
            try
            {
                if (errorCount > 0)
                {
                    MessageBox.Show("There are errors on the form.  Please fix them.");
                }
                else
                {
                    if (Employee.Update(employeeList[dgvEmployees.SelectedRows[0].Index]))
                    {
                        MessageBox.Show("Employee Updated.");
                        resetForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void resetForm()
        {
            dgvEmployees.Enabled = true;

            btnJobInfo.Enabled = true;
            btnEmpStatus.Enabled = true;

            txtFirstName.ReadOnly = true;
            txtMiddleInital.ReadOnly = true;
            txtLastName.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtCity.ReadOnly = true;
            txtProvince.ReadOnly = true;
            txtPostalCode.ReadOnly = true;
            txtWorkPhone.ReadOnly = true;
            txtCellPhone.ReadOnly = true;
            txtEmail.ReadOnly = true;
            btnPersonal.Enabled = true;

            btnSavePersonal.Hide();
            btnCancelPersonal.Hide();

            cmbDept2.Enabled = false;
            cmbJobTitle.Enabled = false;
            dtpJobStartDate.Enabled = false;
            dtpDOB.Enabled = false;
            txtSIN.Enabled = false;

            cmbStatus.Enabled = false;

            btnSaveEmployInfo.Hide();
            btnCancelEmployInfo.Hide();

        }

        private void cmbDept2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDept2.SelectedIndex > -1)
            {
                cmbJobTitle.DataSource = null;

                var dept = (Department)Enum.Parse(typeof(Department), cmbDept2.SelectedValue.ToString(), true);
                var jobList = JobLists.getJobsByDept(dept);
                cmbJobTitle.DataSource = jobList;
                cmbJobTitle.DisplayMember = "Title";
                cmbJobTitle.ValueMember = "JobId";

                txtSupervisor.Text = Job.GetSupervisor(dept);
            }
        }

        private void btnCancelPersonal_Click(object sender, EventArgs e)
        {
            resetForm();
        }

        private void btnJobInfo_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.RowCount == 0)
            {
                MessageBox.Show("No employees selected.");
                return;
            }

            if (employeeList[dgvEmployees.SelectedRows[0].Index].EmployeeId == Bismark.employee.EmployeeId)
            {
                MessageBox.Show("You are not authorized to make changes to your employment information.");
                return;
            }

            dgvEmployees.Enabled = false;

            btnSaveEmployInfo.Show();
            btnCancelEmployInfo.Show();

            btnPersonal.Enabled = false;
            btnEmpStatus.Enabled = false;

            cmbDept2.Enabled = true;
            cmbJobTitle.Enabled = true;
            dtpJobStartDate.Enabled = true;
            dtpDOB.Enabled = true;
            txtSIN.ReadOnly = false;

        }

        private void btnCancelEmployInfo_Click(object sender, EventArgs e)
        {
            dgvEmployees.Enabled = true;

            btnSaveEmployInfo.Hide();
            btnCancelEmployInfo.Hide();

            btnPersonal.Enabled = true;
            btnEmpStatus.Enabled = true;

            cmbDept2.Enabled = false;
            cmbJobTitle.Enabled = false;
            dtpJobStartDate.Enabled = false;
            dtpDOB.Enabled = false;
            txtSIN.ReadOnly = true;
        }

        private void btnSaveEmployInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (errorCount > 0)
                {
                    MessageBox.Show("There are errors on the form.  Please fix them.");
                }
                else
                {
                    if (Employee.Update(employeeList[dgvEmployees.SelectedRows[0].Index]))
                    {
                        MessageBox.Show("Employee Updated.");
                        resetForm();
                        loadEmployees();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEmpStatus_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.RowCount == 0)
            {
                MessageBox.Show("No employees selected.");
                return;
            }

            if (employeeList[dgvEmployees.SelectedRows[0].Index].EmployeeId == Bismark.employee.EmployeeId)
            {
                MessageBox.Show("You are not authorized to make changes to your employment status.");
                return;
            }

            cmbStatus.Enabled = true;

        }

        private void cmbStatus_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmployees.RowCount != 0 && cmbStatus.Enabled == true)
                {
                    employeeList[dgvEmployees.SelectedRows[0].Index].Status = cmbStatus.SelectedIndex;
                    if (Employee.UpdateStatus(employeeList[dgvEmployees.SelectedRows[0].Index]))
                    {
                        cmbStatus.Enabled = false;
                        MessageBox.Show("Status update success");
                    }
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid status");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnSaveSickDay_Click(object sender, EventArgs e)
        {
            try
            {
                var sickday = new SickDay();
                sickday.Date = dtpSickDay.Value;
                sickday.IsFullDay = !chkHalfDay.Checked;

                if (SickDay.RecordSickDay(sickday, employeeList[dgvEmployees.SelectedRows[0].Index].EmployeeId))
                {
                    MessageBox.Show("Sickday recorded.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



    }
}
