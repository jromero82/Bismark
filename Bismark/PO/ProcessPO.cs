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
using System.Net.Mail;

namespace Bismark.GUI
{
    public partial class ProcessPO : Form
    {
        private BindingList<Order> orderList;
        private BindingList<IItem> itemList;
        private BindingSource formItem;
        private Employee employee = Bismark.employee;

        public ProcessPO()
        {
            InitializeComponent();
        }

        private void ProcessPO_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#d6d6d6");
            this.panel1.BackColor = ColorTranslator.FromHtml("#d6d6d6");
            this.btnClose.BackColor = ColorTranslator.FromHtml("#d6d6d6");
            itemList = new BindingList<IItem>();
            formItem = new BindingSource();
            cboFilter.Items.Add("Pending");
            cboFilter.Items.Add("Closed");
            cboFilter.Items.Add("All");
            cboFilter.SelectedItem = "Pending";
            rdoDepartment.Checked = true;            
        }

        private void SetSearchType(object sender, EventArgs e)
        {
            ClearForm();
            var searchType = (RadioButton)sender;
            switch (searchType.Name)
            {
                case "rdoDepartment":
                    txtOrderId.Enabled = false;
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    dtpFrom.Enabled = true;
                    dtpTo.Enabled = true;
                    cboFilter.Enabled = true;
                    chkDateRange.Enabled = true;
                    break;
                case "rdoEmployee":
                    txtOrderId.Enabled = false;
                    txtFirstName.Enabled = true;
                    txtLastName.Enabled = true;
                    dtpFrom.Enabled = true;
                    dtpTo.Enabled = true;
                    cboFilter.Enabled = true;
                    chkDateRange.Enabled = true;
                    break;
                case "rdoOrderId":
                    txtOrderId.Enabled = true;
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    dtpFrom.Enabled = false;
                    dtpTo.Enabled = false;
                    cboFilter.Enabled = false;
                    chkDateRange.Enabled = false;
                    break;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            gdvOrders.DataSource = null;
            gdvItems.DataSource = null;
            txtSubTotal.Text = string.Empty;
            txtTaxTotal.Text = string.Empty;
            txtGrandTotal.Text = string.Empty;

            var filter = (Filter)cboFilter.SelectedIndex;
            try
            {
                if (rdoDepartment.Checked)
                {
                    if (!chkDateRange.Checked)
                        orderList = new BindingList<Order>(OrderLists.GetOrdersByDepartment(employee.EmployeeId, employee.JobAssignment.Department, filter));
                    else
                        orderList = new BindingList<Order>(OrderLists.GetOrdersByDepartment(employee.EmployeeId, employee.JobAssignment.Department, filter,
                            dtpFrom.Value, dtpTo.Value));
                }
                else if (rdoEmployee.Checked)
                {
                    var firstName = txtFirstName.Text;
                    var lastName = txtLastName.Text;
                    if (!chkDateRange.Checked)
                        orderList = new BindingList<Order>(OrderLists.GetOrdersByEmployeeName(employee.EmployeeId, employee.JobAssignment.Department, filter, 
                            firstName, lastName));
                    else
                        orderList = new BindingList<Order>(OrderLists.GetOrdersByEmployeeName(employee.EmployeeId, employee.JobAssignment.Department, filter, 
                            firstName, lastName, dtpFrom.Value, dtpTo.Value));
                }
                else if (rdoOrderId.Checked)
                {
                    var orderId = Convert.ToInt32(txtOrderId.Text);
                    orderList = new BindingList<Order>();
                    orderList.Add(Order.Create(employee.EmployeeId, orderId));
                }

                if (orderList.Count > 0)
                {
                    gdvOrders.DataSource = orderList;
                    gdvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    gdvOrders.Columns["GrandTotal"].DefaultCellStyle.Format = "c";
                    gdvOrders.Columns["EmployeeId"].Visible = false;
                    gdvOrders.Columns["SubTotal"].Visible = false;
                    gdvOrders.Columns["TaxTotal"].Visible = false;
                    gdvOrders.Columns["IsComplete"].Visible = false;                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            gdvOrders.DataSource = null;
            gdvItems.DataSource = null;
            if (gdvItems.Columns.Contains("Process"))
                gdvItems.Columns.Remove("Process");
            txtSubTotal.Text = string.Empty;
            txtTaxTotal.Text = string.Empty;
            txtGrandTotal.Text = string.Empty;
        }


        private void DisplayOrderItems(object sender, EventArgs e)
        {
            var orderIndex = gdvOrders.CurrentRow.Index;
            itemList = new BindingList<IItem>(orderList[orderIndex].Items);
            gdvItems.DataSource = itemList;
            
            gdvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gdvItems.Columns["Price"].DefaultCellStyle.Format = "c";
            gdvItems.Columns["ItemId"].Visible = false;
            gdvItems.Columns["OrderId"].Visible = false;
            gdvItems.Columns["IsComplete"].Visible = false;

            gdvItems.Columns["SubTotal"].DefaultCellStyle.Format = "c";
            gdvItems.Columns["TaxTotal"].DefaultCellStyle.Format = "c";
            gdvItems.Columns["GrandTotal"].DefaultCellStyle.Format = "c";

            gdvItems.Rows[0].Selected = true;
            this.DisplayOrderTotals(orderIndex);          
        }

        private void ClearFormBindings()
        {
            txtName.DataBindings.Clear();
            txtDescription.DataBindings.Clear();
            txtPrice.DataBindings.Clear();
            txtQuantity.DataBindings.Clear();
            txtJustification.DataBindings.Clear();
            txtSource.DataBindings.Clear();
            txtReason.DataBindings.Clear();
        }

        private void gdvItems_SelectionChanged(object sender, EventArgs e)
        {        
            if (gdvItems.Rows.Count == itemList.Count)
            {
                if (gdvItems.CurrentRow != null)
                {
                    ClearFormBindings();
                    formItem.DataSource = itemList[gdvItems.CurrentRow.Index];
                    txtName.DataBindings.Add("Text", formItem, "Name");
                    txtDescription.DataBindings.Add("Text", formItem, "Description");
                    txtPrice.DataBindings.Add("Text", formItem, "Price");
                    txtQuantity.DataBindings.Add("Text", formItem, "Quantity");
                    txtJustification.DataBindings.Add("Text", formItem, "Justification");
                    txtSource.DataBindings.Add("Text", formItem, "Source");
                    txtReason.DataBindings.Add("Text", formItem, "Reason");
                }
            }
        }

        private void Process(object sender, EventArgs e)
        {
            var button = (Button)sender;
            try
            {
                var orderIndex = gdvOrders.CurrentRow.Index;
                var itemIndex = gdvItems.CurrentRow.Index;
                ItemStatus status;

                if (button.Text == "APPROVE")
                    status = ItemStatus.Approved;
                else 
                    status = ItemStatus.Denied;

                if (!orderList[orderIndex].Items[itemIndex].Process(employee.EmployeeId, status))
                {
                    if (MessageBox.Show("Would you like to close this order?", "Bismark",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) ==
                        DialogResult.Yes)
                    {
                        orderList[orderIndex].Close(employee.EmployeeId);
                        this.SendMail(orderIndex);                        
                    }                    
                }
                gdvItems.Refresh();
                this.DisplayOrderTotals(orderIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayOrderTotals(int orderIndex)
        {
            txtSubTotal.Text = orderList[orderIndex].SubTotal.ToString("c");
            txtTaxTotal.Text = orderList[orderIndex].TaxTotal.ToString("c");
            txtGrandTotal.Text = orderList[orderIndex].GrandTotal.ToString("c");  
        }

        private void ValidateInput(object sender, CancelEventArgs e)
        {
            var orderIndex = gdvOrders.CurrentRow.Index;
            var itemIndex = gdvItems.CurrentRow.Index;
            var property = (TextBox)sender;

            try
            {               
                switch (property.Name)
                {
                    case "txtPrice":
                        orderList[orderIndex].Items[itemIndex].Price = Convert.ToDecimal(txtPrice.Text);
                        break;
                    case "txtQuantity":
                        orderList[orderIndex].Items[itemIndex].Quantity = Convert.ToInt32(txtQuantity.Text);
                        break;
                    case "txtSource":
                        orderList[orderIndex].Items[itemIndex].Description = txtDescription.Text;
                        break;
                    case "txtReason":
                        orderList[orderIndex].Items[itemIndex].Source = txtSource.Text;
                        break;
                }
                errorProvider1.SetError(property, string.Empty);
                property.ReadOnly = true;
                property.BackColor = SystemColors.Control;
                property.ForeColor = SystemColors.GrayText;
                
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconPadding(property, -20);
                errorProvider1.SetError(property, ex.Message);
            }
        }

        private void ModifyItemProperty(object sender, MouseEventArgs e)
        {
            var property = (TextBox)sender;
            try
            {
                if (property.Name == "txtPrice" ||
                    property.Name == "txtQuantity" ||
                    property.Name == "txtSource" ||
                    property.Name == "txtReason")
                {
                    property.ReadOnly = false;
                    property.BackColor = Color.White;
                    property.ForeColor = Color.Black;
                }
            }
            catch (Exception ex) { }
        }
        

        private void CloseOrder(object sender, EventArgs e)
        {
            try
            {
                if (orderList != null)
                {
                    var orderIndex = gdvOrders.CurrentRow.Index;
                    orderList[orderIndex].Close(employee.EmployeeId);
                    this.SendMail(orderIndex);
                }
                else
                    MessageBox.Show("Please retrieve and select an order to close.", "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendMail(int orderIndex)
        {
            var receiver = Employee.Create(orderList[orderIndex].EmployeeId);
            var receiverEmail = receiver.Email;

            var client = new SmtpClient("localhost");
            var msg = new MailMessage();
            msg.From = new MailAddress(employee.Email);
            msg.To.Add(receiverEmail);
            msg.IsBodyHtml = true;
            msg.Body = "<h1>Your Order Has Been Processed</h1>" +
                       "Order Number: " + orderList[orderIndex].OrderId + "<br />";
            msg.Subject = "Your Order Has Been Processed";
            client.Send(msg);
        }
    }
}
