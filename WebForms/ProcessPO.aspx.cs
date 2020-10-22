using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bismark.Utilities;
using Bismark.BOL;
using System.ComponentModel;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
using System.Net.Mail;

namespace Bismark.WebForms
{
    public partial class ProcessPO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var authLevel = Convert.ToInt32(Session["AuthLevel"]);
            if (authLevel != 1 &&
                authLevel != 2)
            {
                Response.Redirect("Login.aspx?ref=" + "ProcessPO");
            }          
        }

        
    }
}