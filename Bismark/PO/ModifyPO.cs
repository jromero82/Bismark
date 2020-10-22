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
    public partial class ModifyPO : Form
    {
        private Employee employee;
        private BindingList<Order> orderList;
        private BindingList<IItem> itemList;
        private BindingSource formItem;

        public ModifyPO()
        {
            InitializeComponent();
        }

        private void ModifyPO_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#d6d6d6");
            this.panel1.BackColor = ColorTranslator.FromHtml("#d6d6d6");
            this.btnClose.BackColor = ColorTranslator.FromHtml("#d6d6d6");
            cboFilter.Items.Add("Pending");
            cboFilter.Items.Add("Closed");
            cboFilter.Items.Add("All");
            cboFilter.SelectedIndex = 0;
            employee = Bismark.employee;
            orderList = new BindingList<Order>(OrderLists.GetOrdersByEmployeeId(employee.EmployeeId, Filter.Pending));
            gdvOrders.DataSource = orderList;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFormBindings();
                txtName.ResetText();
                txtDescription.ResetText();
                txtPrice.ResetText();
                txtQuantity.ResetText();
                txtJustification.ResetText();
                txtSource.ResetText();

                var filter = (Filter)cboFilter.SelectedIndex;
                if (string.IsNullOrEmpty(txtOrderId.Text))
                {
                    if (chkDateRange.Checked)
                        orderList = new BindingList<Order>(OrderLists.GetOrdersByEmployeeId(employee.EmployeeId, filter, dtpFrom.Value, dtpTo.Value));
                    else
                        orderList = new BindingList<Order>(OrderLists.GetOrdersByEmployeeId(employee.EmployeeId, filter));
                }
                else
                {
                    orderList = new BindingList<Order>();
                    orderList.Add(Order.Create(employee.EmployeeId, Convert.ToInt32(txtOrderId.Text)));
                }
                if (orderList != null &&
                    orderList.Count > 0)
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

        private void DisplayOrderTotals(int orderIndex)
        {
            txtSubTotal.Text = orderList[orderIndex].SubTotal.ToString("c");
            txtTaxTotal.Text = orderList[orderIndex].TaxTotal.ToString("c");
            txtGrandTotal.Text = orderList[orderIndex].GrandTotal.ToString("c");
        }

        private void gdvItems_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (gdvItems.Rows.Count == itemList.Count)
                {
                    if (gdvItems.CurrentRow != null)
                    {
                        ClearFormBindings();
                        formItem = new BindingSource();
                        formItem.DataSource = itemList[gdvItems.CurrentRow.Index];
                        txtName.DataBindings.Add("Text", formItem, "Name");
                        txtDescription.DataBindings.Add("Text", formItem, "Description");
                        txtPrice.DataBindings.Add("Text", formItem, "Price");
                        txtQuantity.DataBindings.Add("Text", formItem, "Quantity");
                        txtJustification.DataBindings.Add("Text", formItem, "Justification");
                        txtSource.DataBindings.Add("Text", formItem, "Source");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFormBindings()
        {
            txtName.DataBindings.Clear();
            txtDescription.DataBindings.Clear();
            txtPrice.DataBindings.Clear();
            txtQuantity.DataBindings.Clear();
            txtJustification.DataBindings.Clear();
            txtSource.DataBindings.Clear();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            var command = (Button)sender;
            try
            {
                if (orderList != null)
                {
                    if (gdvItems.SelectedRows.Count > 0)
                    {
                        var orderIndex = gdvOrders.CurrentRow.Index;
                        var itemIndex = gdvItems.CurrentRow.Index;
                        if (command.Text == "SAVE CHANGES")
                        {
                            ItemCUD.Update(employee.EmployeeId, (Item)itemList[itemIndex]);
                        }
                        else if (command.Text == "NO LONGER REQUIRED")
                        {                            
                            itemList[itemIndex].Description = "No longer needed.";
                            itemList[itemIndex].Price = 0;
                            itemList[itemIndex].Quantity = 0;

                            ItemCUD.Update(employee.EmployeeId, (Item)itemList[itemIndex]);
                        }
                        gdvItems.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {

                var itemIndex = gdvItems.CurrentRow.Index;
                var items = ItemLists.GetItemsByOrderId(orderList[gdvItems.CurrentRow.Index].OrderId);
                itemList.Clear();
                foreach (var item in items)
                {
                    itemList.Add(item);
                }
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gdvItems.Refresh();
            }
        }

        private void txtName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var property = (TextBox)sender;
            try
            {
                property.ReadOnly = false;
            }
            catch (Exception ex) { }
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            var orderIndex = gdvOrders.CurrentRow.Index;
            var itemIndex = gdvItems.CurrentRow.Index;
            var property = (TextBox)sender;

            try
            {
                switch (property.Name)
                {
                    case "txtName":
                        orderList[orderIndex].Items[itemIndex].Name = txtName.Text;
                        break;
                    case "txtDescription":
                        orderList[orderIndex].Items[itemIndex].Description = txtDescription.Text;
                        break;
                    case "txtPrice":
                        orderList[orderIndex].Items[itemIndex].Price = Convert.ToDecimal(txtPrice.Text);
                        break;
                    case "txtQuantity":
                        orderList[orderIndex].Items[itemIndex].Quantity = Convert.ToInt32(txtQuantity.Text);
                        break;
                    case "txtJustification":
                        orderList[orderIndex].Items[itemIndex].Justification = txtJustification.Text;
                        break;
                    case "txtSource":
                        orderList[orderIndex].Items[itemIndex].Source = txtSource.Text;
                        break;
                }
                errorProvider1.SetError(property, string.Empty);
                property.ReadOnly = true;
                gdvItems.Refresh();
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconPadding(property, -20);
                errorProvider1.SetError(property, ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
