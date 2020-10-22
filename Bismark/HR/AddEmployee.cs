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

    public partial class AddEmployee : Form
    {
        private Employee emp;
        private int errorCount;
       
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#d6d6d6");
            emp = Bismark.employee;
            cmbDept.DataSource = Enum.GetNames(typeof(Department));
            dtpJobStartDate.MinDate = DateTime.Today;
            dtpDOB.MaxDate = DateTime.Today;
            btnNew.Hide();
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDept.SelectedIndex > -1)
            {
                cmbJobTitle.DataSource = null;

                var dept = (Department) Enum.Parse(typeof(Department), cmbDept.SelectedValue.ToString(), true);
                var jobList = JobLists.getJobsByDept(dept);
                cmbJobTitle.DataSource = jobList;
                cmbJobTitle.DisplayMember = "Title";
                cmbJobTitle.ValueMember = "JobId";

                txtSupervisor.Text = Job.GetSupervisor(dept);                
            }            

        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                emp.JobId = Convert.ToInt16(cmbJobTitle.SelectedValue);
                emp.JobStartDate = dtpJobStartDate.Value;
                emp.DateOfBirth = dtpDOB.Value;                
                var password = txtPassword.Text;

                emp.EmployeeId = Employee.Insert(emp, password);

                if (emp.EmployeeId > 0)
                {
                    MessageBox.Show("Employee added successful. \nEmployee ID: " + emp.EmployeeId);
                    btnAddEmployee.Enabled = false;
                    btnNew.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                
        }

        private void ValidateInput(object sender, CancelEventArgs e)
        {

            var control = (TextBox)sender;
            try
            {
                switch (control.Name)
                {
                    // Personal INFO
                    case "txtFirstName":
                        emp.FirstName = txtFirstName.Text;
                        break;
                    case "txtMiddleInitial":
                        emp.MiddleInitial = txtMiddleInital.Text;
                        break;
                    case "txtLastName":
                        emp.LastName = txtLastName.Text;
                        break;
                    case "txtAddress":
                        emp.Address = txtAddress.Text;
                        break;
                    case "txtCity":
                        emp.City = txtCity.Text;
                        break;
                    case "txtProvince":
                        emp.Province = txtProvince.Text;
                        break;
                    case "txtPostalCode":
                        emp.PostalCode = txtPostalCode.Text;
                        break;
                    case "txtWorkPhone":
                        emp.WorkPhone = txtWorkPhone.Text;
                        break;
                    case "txtCellPhone":
                        emp.CellPhone = txtCellPhone.Text;
                        break;
                    case "txtEmail":
                        emp.Email = txtEmail.Text;
                        break;
                    case "txtSIN":
                        emp.SIN = txtSIN.Text;
                        break;
                    case "txtSalary":
                        emp.JobId = (int)cmbJobTitle.SelectedValue;
                        emp.Salary = Convert.ToDecimal(txtSalary.Text);
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.ResetText();
                } 
                
                dtpDOB.Value = DateTime.Today;
                dtpJobStartDate.Value = DateTime.Today;
                btnNew.Hide();
                btnAddEmployee.Enabled = true;
            }             
        
           
        }
        
    }
}
