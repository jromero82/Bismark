using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bismark.BOL;
using Bismark.Utilities;

namespace Bismark.GUI
{
    public partial class Bismark : Form
    {
        public static Employee employee;
        public static AuthLevel authLevel;

        public Bismark()
        {
            InitializeComponent();
        }

        private void tsBtnSalary_Click(object sender, EventArgs e)
        {
            CheckMDIChildren(new SalaryChanges());          
        }

        private void tsBtnBonus_Click(object sender, EventArgs e)
        {
            CheckMDIChildren(new BonusForm());    
        }

        private void tsBtnCreatePO_Click(object sender, EventArgs e)
        {
            CheckMDIChildren(new CreatePO());
        }

        private void tsBtnProcessPO_Click(object sender, EventArgs e)
        {
            CheckMDIChildren(new ProcessPO());
        }

        private void tsBtnModifyPO_Click(object sender, EventArgs e)
        {
            CheckMDIChildren(new ModifyPO());
        }

        private void CheckMDIChildren(Form form)
        {
            foreach (var frm in this.MdiChildren)
            {
                if (frm.GetType() == form.GetType())
                {
                    frm.BringToFront();
                    return;
                }
                   
            }
            form.TopLevel = false;
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void Bismark_Load(object sender, EventArgs e)
        {
            var splash = new Splash();
            splash.ShowDialog();
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() != DialogResult.OK)
            {
                this.Close();
            }
            if (employee.Department == Department.HumanResources)
            {
                tsBtnBonus.Enabled = true;
                tsBtnSalary.Enabled = true;
                tsBtnAddEmployee.Enabled = true;
                tsBtnModifyEmployee.Enabled = true;
            }
            if (authLevel == AuthLevel.Supervisor || authLevel == AuthLevel.CEO)
                tsBtnProcessPO.Enabled = true;
            this.toolStripStatusLabel1.Text = "Logged In As: " + employee.LastName + ", " + employee.FirstName + " " + employee.MiddleInitial;
        }

        private void tsBtnChat_Click(object sender, EventArgs e)
        {
            CheckMDIChildren(new IMClient());
        }

        private void tsBtnAddEmployee_Click(object sender, EventArgs e)
        {
            CheckMDIChildren(new AddEmployee());
        }

        private void tsBtnModifyEmployee_Click(object sender, EventArgs e)
        {
            CheckMDIChildren(new ModifyEmployee());
        }   
    }
}
