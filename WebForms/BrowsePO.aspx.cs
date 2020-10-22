using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Bismark.BOL;
using Bismark.Utilities;

namespace Bismark.WebForms
{
    public partial class BrowsePO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AuthLevel"] == null)
                Response.Redirect("Login.aspx?ref=" + "BrowsePO");
        }

        
    }
}