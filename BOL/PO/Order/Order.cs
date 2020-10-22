using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Bismark.SQL;
using Bismark.Utilities;
using System.Data.SqlClient;
using System.ComponentModel;

namespace Bismark.BOL
{
    [Serializable]
    public class Order : IOrder, INotifyPropertyChanged
    {

        private bool trustedSource;
        private int orderId;
        private int employeeId;
        private string employeeName;
        private DateTime orderDate;
        private ItemList items;
        private decimal subTotal;
        private decimal taxTotal;
        private decimal grandTotal;
        private Department? department;
        private OrderStatus status;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>
        /// Gets or the order id.
        /// </summary>
        public int OrderId
        {
            get
            {
                return this.orderId;
            }
            set
            {
                if (this.orderId == 0)
                    this.orderId = value;
                else
                    if (this.orderId != value)
                        throw new ArgumentException("Unable to change the order Id.");
            }
        }

        /// <summary>
        /// Gets or sets the employee id.
        /// </summary>
        public int EmployeeId
        {
            get
            {
                return this.employeeId;
            }
            set
            {
                if (trustedSource)
                    this.employeeId = value;
                else
                {
                    Validation.ValidateEmployeeId(value);
                    this.employeeId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        public DateTime OrderDate
        {
            get
            {
                return this.orderDate;
            }
        }

        // Gets the employee name.
        public string EmployeeName
        {
            get
            {
                return this.employeeName;
            }
        }

        // Gets or sets the department.
        public Department? Department
        {
            get
            {
                return this.department;
            }
            set
            {
                this.department = value;
            }
        }

        /// <summary>
        /// Gets the order sub total.
        /// </summary>
        public decimal SubTotal
        {
            get
            {
                if (this.items.Count != 0)
                {
                    decimal sTotal = 0;
                    foreach (var item in this.items)
                    {
                        if (item.Status != ItemStatus.Denied)
                            sTotal += item.SubTotal;
                    }
                    return sTotal;
                }
                else
                    return subTotal;
            }
        }

        /// <summary>
        /// Gets the order tax total.
        /// </summary>
        public decimal TaxTotal
        {
            get
            {
                if (this.items.Count != 0)
                {
                    decimal tTotal = 0;
                    foreach (var item in this.items)
                    {
                        if (item.Status != ItemStatus.Denied)
                            tTotal += item.TaxTotal;
                    }
                    return tTotal;
                }
                else
                    return taxTotal;
            }
        }

        /// <summary>
        /// Gets the order grand total.
        /// </summary>
        public decimal GrandTotal
        {
            get
            {
                if (this.items.Count != 0)
                {
                    decimal gTotal = 0;
                    foreach (var item in this.items)
                    {
                        if (item.Status != ItemStatus.Denied)
                            gTotal += item.GrandTotal;
                    }
                    return gTotal;
                }
                else
                    return grandTotal;
            }
        }        

        /// <summary>
        /// Gets the closure status of the order.
        /// </summary>
        public OrderStatus Status 
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// Gets or sets a list of items for the order.
        /// </summary>
        public virtual ItemList Items
        {
            get
            {
                if (this.orderId != 0)
                {
                    if (items.Count == 0)
                        this.items = ItemLists.GetItemsByOrderId(OrderId);
                }
                return this.items;
            }
        }

        /// <summary>
        /// Ensures that an employee id is set as well as at least one item is added to the order.
        /// </summary>
        public bool IsComplete
        {
            get
            {
                if (this.employeeId == 0 || 
                    this.items.Count == 0)
                    return false;
                return true;
            }
        }

        #endregion

        #region Factory

        private Order()
        {
            items = new ItemList();
            department = null;
        }

        /// <summary>
        /// Creates a new order object.
        /// </summary>
        /// <returns></returns>
        public static Order Create()
        {
            return new Order();
        }

        public static Order Create(int employeeId, int id)
        {
            if (id == 0)
                throw new ArgumentException("Invalid order Id.");
            return RePackage(OrderSQL.GetOrderById(employeeId, id))[0];
        }

        #endregion


        #region Methods

        /// <summary>
        /// Closes the order.
        /// </summary>
        public void Close(int supeId)
        {
            if (this.orderId == 0)
                throw new InvalidOperationException("Unable to close an order not yet created.");
            if (this.status == OrderStatus.Closed)
                throw new InvalidOperationException("This order is already closed.");

            OrderSQL.Close(supeId, this.orderId);
            this.status = OrderStatus.Closed;
            this.NotifyPropertyChanged("Status");   
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        internal static List<Order> RePackage(DataTable dt)
        {
            var orderList = new List<Order>();
   
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                var order = new Order();
                var row = dt.Rows[i];

                order.trustedSource = true;
                order.orderId = Convert.ToInt32(row["OrderId"]);
                order.employeeId = Convert.ToInt32(row["EmployeeId"]);
                order.employeeName = Convert.ToString(row["EmployeeName"]);
                order.orderDate = Convert.ToDateTime(row["OrderDate"]);
                order.department = (Department)(row["Department"]);
                order.subTotal = Convert.ToDecimal(row["SubTotal"]);
                order.taxTotal = Convert.ToDecimal(row["TaxTotal"]);
                order.grandTotal = Convert.ToDecimal(row["GrandTotal"]);
                order.status = (OrderStatus)row["Status"];
                order.trustedSource = false;
                orderList.Add(order);
            }
            return orderList;
        }
    }
}
