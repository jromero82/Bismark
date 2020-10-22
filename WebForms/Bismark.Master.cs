using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Bismark.Data; 
using Bismark.BOL;

namespace Bismark.WebForms
{
    public partial class Bismark : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmployeeName"] != null)
            {
                lblUsername.Text = "Logged In As: " + Session["EmployeeName"].ToString();
                lnkLogout.Text = "Logout";
            }
            else
            {
                lblUsername.Text = string.Empty;
                lnkLogout.Text = string.Empty;
            }
        }
    }
}