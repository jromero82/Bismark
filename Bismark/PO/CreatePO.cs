using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal;
using Bismark.BOL;
using Bismark.Utilities;

namespace Bismark.GUI
{
    public partial class CreatePO : Form
    {
        private Order order;
        private Item item;
        private Employee employee;
        public CreatePO()
        {
            InitializeComponent();       
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.panel1.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                this.bnClose.BackColor = ColorTranslator.FromHtml("#d6d6d6");
                employee = Bismark.employee;
                order = Order.Create();
                order.EmployeeId = employee.EmployeeId;
                order.Department = employee.JobAssignment.Department;
                item = Item.Create();

                txtDate.Text = DateTime.Now.ToShortDateString();
                txtEmployee.Text = employee.LastName + ", " + employee.FirstName;
                txtDepartment.Text = order.Department.ToString();
                txtSupervisor.Text = employee.Supervisor;

                DisableFields(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {                   
                order.Items.Add(item);
                btnOrder.Text = "SAVE ORDER";
                txtSubTotal.Text = order.SubTotal.ToString("c");
                txtTaxTotal.Text = order.TaxTotal.ToString("c");
                txtGrandTotal.Text = order.GrandTotal.ToString("c");
                RefreshData();
                ClearFields();
                item = Item.Create();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnOrder.Text == "NEW ORDER")
                {
                    order = Order.Create();
                    order.EmployeeId = employee.EmployeeId;
                    item = Item.Create();
                    btnOrder.Text = "SAVE ORDER";
                    DisableFields(false);
                }
                else
                {
                    if (MessageBox.Show("Are you sure you wish to save this order?", "Bismark", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                    {
                        OrderCUD.Create(order);
                        MessageBox.Show("Order created succesffully! \n" +
                        "Please save your order number: " + order.OrderId);
                        txtOrderId.Text = order.OrderId.ToString();
                        ClearFields();
                        order = Order.Create();
                        order.EmployeeId = employee.EmployeeId;
                        order.Department = employee.JobAssignment.Department;
                        txtSubTotal.Text = string.Empty;
                        txtTaxTotal.Text = string.Empty;
                        txtGrandTotal.Text = string.Empty;
                        txtOrderStatus.Text = "Pending";
                        btnOrder.Text = "NEW ORDER";
                        DisableFields(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bismark", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshData()
        {
            var subTotal = 0m;
            var taxTotal = 0m;
            var grandTotal = 0m;
            dgvItems.DataSource = null;
            dgvItems.DataSource = order.Items;
            dgvItems.Columns["ItemId"].Visible = false;
            dgvItems.Columns["OrderId"].Visible = false;
            dgvItems.Columns["IsComplete"].Visible = false;
            dgvItems.Columns["Reason"].Visible = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (var item in order.Items)
            {
                subTotal += item.Price * item.Quantity;
            }
            taxTotal = subTotal * 0.13m;
            grandTotal = subTotal + taxTotal;
            txtSubTotal.Text = subTotal.ToString("c");
            txtTaxTotal.Text = taxTotal.ToString("c");
            txtGrandTotal.Text = grandTotal.ToString("c");
        }

        private void ValidateInput(object sender, CancelEventArgs e)
        {
            var control = (TextBox)sender;
            try
            {
                switch (control.Name)
                {
                    case "txtName":
                        item.Name = txtName.Text;
                        break;
                    case "txtDescription":
                        item.Description = txtDescription.Text;
                        break;
                    case "txtPrice":
                        item.Price = Convert.ToDecimal(txtPrice.Text);
                        break;
                    case "txtQuantity":
                        item.Quantity = Convert.ToInt32(txtQuantity.Text);
                        break;
                    case "txtJustification":
                        item.Justification = txtJustification.Text;
                        break;
                    case "txtSource":
                        item.Source = txtSource.Text;
                        break;
                }
                errorProvider1.SetError(control, string.Empty);
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconPadding(control, -20);
                errorProvider1.SetError(control, ex.Message);
            }
        }

        private void DisableFields(bool value)
        {
            txtName.ReadOnly = value;
            txtDescription.ReadOnly = value;
            txtPrice.ReadOnly = value;
            txtQuantity.ReadOnly = value;
            txtJustification.ReadOnly = value;
            txtSource.ReadOnly = value;
        }

        private void ClearFields()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtJustification.Text = string.Empty;
            txtSource.Text = string.Empty;
        }      

        private void bnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
