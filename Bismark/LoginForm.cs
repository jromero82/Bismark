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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var id = Convert.ToInt32(txtID.Text);
                var password = txtPassword.Text;

                Bismark.authLevel = (AuthLevel)Employee.Login(id, password);
                Bismark.employee = Employee.Create(id);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }       
    }
}
