using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bismark.Data;
using Bismark.BOL;
using System.Web.Services;
using Bismark.Utilities;

namespace Bismark.WebForms
{
    public partial class CreatePO : System.Web.UI.Page
    {
        private Order order;
        private Control msgBox;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState.Add("order", order);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AuthLevel"] == null)
                Response.Redirect("Login.aspx?ref=" + "CreatePO");
            if (!IsPostBack)
            {
                order = Order.Create();
                this.LoadHeader();
            }
            if (ViewState["order"] != null)
                order = (Order)ViewState["order"];
            else
            {
                order = Order.Create();
                order.EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
                order.Department = (Department)Enum.Parse(typeof(Department), Session["Department"].ToString(), true);
            }
        }

        protected void LoadHeader()
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtEmployee.Text = Session["EmployeeName"].ToString();
            txtDepartment.Text = Session["Department"].ToString();
            txtSupervisor.Text = Session["Supervisor"].ToString();
        }

        [WebMethod]
        public static string ValidateInput(string property, string value)
        {
            var item = Item.Create();
            try
            {

                switch (property)
                {
                    case "name":
                        item.Name = value;
                        break;
                    case "description":
                        item.Description = value;
                        break;
                    case "price":
                        item.Price = Convert.ToDecimal(value);
                        break;
                    case "quantity":
                        item.Quantity = Convert.ToInt32(value);
                        break;
                    case "justification":
                        item.Justification = value;
                        break;
                    case "source":
                        item.Source = value;
                        break;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                var item = Item.Create();
                item.Name = txtName.Text;
                item.Description = txtDescription.Text;
                item.Price = Convert.ToDecimal(txtPrice.Text);
                item.Quantity = Convert.ToInt32(txtQuantity.Text);
                item.Justification = txtJustification.Text;
                item.Source = txtSource.Text;   
                order.Items.Add(item);
                //btnOrder.Text = "SAVE ORDER";
                txtSubTotal.Text = order.SubTotal.ToString("c");
                txtTaxTotal.Text = order.TaxTotal.ToString("c");
                txtGrandTotal.Text = order.GrandTotal.ToString("c");
                this.ClearFields();
                this.RefreshData();                
            }
            catch (Exception ex)
            {
                messageBox.Text = ex.Message;
            }
        }

        private void RefreshData()
        {
            gdvItems.DataSource = null;
            gdvItems.DataSource = order.Items;
            gdvItems.DataBind();          
        }

        protected void ClearFields()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtJustification.Text = string.Empty;
            txtSource.Text = string.Empty;
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnOrder.Text == "NEW ORDER")
                {
                    order = Order.Create();
                }
                else
                {
                    //if (MessageBox.Show("Are you sure you wish to save this order?", "Bismark", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    //    == DialogResult.Yes)
                    // {
                    OrderCUD.Create(order);
                    //MessageBox.Show("Order created succesffully! \n" +
                    //"Please save your order number: " + order.OrderId);
                    txtOrderId.Text = order.OrderId.ToString();
                    ClearFields();
                    order = Order.Create();
                    order.EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
                    order.Department = (Department)Enum.Parse(typeof(Department), Session["Department"].ToString(), true);
                    txtSubTotal.Text = string.Empty;
                    txtTaxTotal.Text = string.Empty;
                    txtGrandTotal.Text = string.Empty;
                    txtOrderStatus.Text = "Pending";
                    btnOrder.Text = "NEW ORDER";
                    RefreshData();
                    // }
                }
            }
            catch (Exception ex)
            {
                messageBox.Text = ex.Message;
            }          
        }
    }
}