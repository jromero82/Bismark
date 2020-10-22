using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.Utilities;
using Bismark.SQL;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
namespace Bismark.BOL
{
    [Serializable]
    public class Item : IItem, INotifyPropertyChanged
	{
        private bool trustedSource;
        private int itemId;
        private int orderId;
        private string name;
        private string description;
        private decimal originalPrice;
        private decimal price;        
        private bool modified;
        private int originalQuantity;
        private int quantity;
        private string justification;
        private string originalSource;
        private string source;
        private ItemStatus status;
        private string reason;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the item Id.
        /// </summary>
        public int ItemId
        {
            get
            {
                return this.itemId;
            }
        }

        /// <summary>
        /// Gets the order Id.
        /// </summary>
        public int OrderId 
        {
            get
            {
                return this.orderId;
            }
        }

        /// <summary>
        /// Gets the item name.
        /// </summary>
        public string Name 
        {
            get
            {
                return this.name;
            }
            set
            {
                if (trustedSource)
                    this.name = value;
                else
                {
                    Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "Name");
                    if (this.name != value)
                    {
                        Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "Name", 30);
                        this.name = value;
                        this.NotifyPropertyChanged("Name");
                    }
                }
            }        
        }

        /// <summary>
        /// Gets the item description.
        /// </summary>
        public string Description 
        {
            get
            {
                return this.description;
            }
            set
            {
                if (trustedSource)
                    this.description = value;
                else
                {
                    Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "Description");
                    if (this.description != value)
                    {
                        Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "Description", 50);
                        this.description = value;
                        this.NotifyPropertyChanged("Description");
                    }
                }
            }
        
        }

        /// <summary>
        /// Gets the item price.
        /// </summary>
        public decimal Price 
        {
            get
            {
                return this.price;
            }
            set
            {
                if (trustedSource)
                    this.price = value;
                else
                {
                    if (this.price != value)
                    {
                        Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Price");
                        if (this.originalPrice != value)
                            this.modified = true;
                        this.price = value;                        
                        this.NotifyPropertyChanged("Price");
                    }
                }
            }
        }
       
        /// <summary>
        /// Gets the item quantity.
        /// </summary>
        public int Quantity 
        {
            get
            {
                return this.quantity;
            }
            set 
            {
                if (this.trustedSource)
                {
                    this.quantity = value;
                }
                else
                {
                    if (this.quantity != value)
                    {
                        Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Quantity");
                        if (this.originalQuantity != value)
                            this.modified = true;
                        this.quantity = value;
                        this.NotifyPropertyChanged("Quanitity");
                    }
                }
            }
        }

        public string Justification 
        {
            get 
            {
                return this.justification;
            }
            set 
            {
                if (this.trustedSource)
                    this.justification = value;
                else
                {
                    Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "Justification");
                    if (this.justification != value)
                    {
                        Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "Justification", 100);
                        this.justification = value;
                        this.NotifyPropertyChanged("Justification");
                    }
                }
            } 
        }
        public string Source 
        
        {
            get
            {
                return source;
            }
            set
            {
                if (this.trustedSource)
                    this.source = value;
                else
                {
                    Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "Source");
                    if (this.source != value)
                    {
                        Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "Source", 100);
                        if (this.originalSource != value)
                            this.source = value;
                        this.modified = true;
                        this.NotifyPropertyChanged("Source");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the sub total.
        /// </summary>
        public decimal SubTotal
        {
            get
            {
                return this.price * this.quantity;
            }
        }

        public decimal TaxTotal
        {
            get
            {
                return this.SubTotal * .13m;
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return this.SubTotal + this.TaxTotal;
            }
        }

        /// <summary>
        /// Gets the item status. Do not attempt to set the item's status through this property.
        /// Use the Process method instead.
        /// </summary>
        public ItemStatus Status 
        {
            get
            {
                return this.status;
            }
        }

        /// <summary>
        /// Gets the reason for the item's denial.
        /// </summary>
        public string Reason 
        {
            get
            {
                return this.reason;
            }
            set
            {
                if (trustedSource)
                    this.reason = value;
                else
                {
                    if (this.reason != value)
                    {
                        Validation.ValidateFieldLength(value, SizeOperator.NoGreaterThan, "Reason", 100);
                        reason = value;
                        this.NotifyPropertyChanged("Reason");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the completion status of the object.
        /// </summary>
        public bool IsComplete
        {
            get
            {
                if (this.itemId == 0 && this.price == 0 ||
                    this.itemId == 0 && this.quantity == 0 || 
                    string.IsNullOrEmpty(this.name) ||
                    string.IsNullOrEmpty(this.description) ||
                    string.IsNullOrEmpty(this.justification) ||
                    string.IsNullOrEmpty(this.source))
                    return false;
                return true;
            }
        }

        private Item()
        {

        }

        /// <summary>
        /// Creates an empty item object.
        /// </summary>
        /// <returns></returns>
        public static Item Create()
        {
            return new Item();
        }

        public static Item Create(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentException("Invalid item Id.");
            var itemList = RePackage(ItemSQL.GetItemById(itemId));
            return (Item)itemList[0];
        }

        #region Methods


        /// <summary>
        /// This method will update the item's status as well as determine if 
        /// any item's left in the order that are pending.
        /// </summary>
        /// <param name="itemStatus"></param>
        /// <returns></returns>
        public bool Process(int employeeId, ItemStatus itemStatus)
        {
            if (itemId == 0)
                throw new InvalidOperationException("Unable to change the status an item not yet associated with an existing order.");
            if (BusinessRules.IsOrderClosed(this.orderId))
                throw new InvalidOperationException("Unable to change in item's status on a closed order status.");
            if (itemStatus == ItemStatus.Approved)
            {
                if (this.modified && string.IsNullOrEmpty(this.reason)) // Business Rule
                    throw new InvalidOperationException("Changes to the item have been detected. Please provide a reason before processing.");
            }
            else if (itemStatus == ItemStatus.Denied)
            {
                if (string.IsNullOrEmpty(this.reason)) // Business Rule
                    throw new InvalidOperationException("A reason must be provided for denial.");
            }

            this.status = itemStatus;
            this.NotifyPropertyChanged("Status");
            var areItemsPending = ItemSQL.Process(employeeId, this);           
            return areItemsPending;
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        internal static ItemList RePackage(DataTable dt)
        {
            var itemList = new ItemList();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                var item = new Item();
                var row = dt.Rows[i];

                item.trustedSource = true;
                item.itemId = Convert.ToInt32(row["ItemId"]);
                item.orderId = Convert.ToInt32(row["OrderId"]);
                item.name = Convert.ToString(row["Name"]);
                item.description = Convert.ToString(row["Description"]);
                item.price = Convert.ToDecimal(row["Price"]);
                item.quantity = Convert.ToInt32(row["Quantity"]);
                item.originalQuantity = item.Quantity;
                item.justification = Convert.ToString(row["Justification"]);
                item.source = Convert.ToString(row["Source"]);
                item.status = (ItemStatus)(row["Status"]);
                item.reason = Convert.ToString(row["Reason"]);
                item.trustedSource = false;
                itemList.Add(item);
            }
            return itemList;
        }
    }
}
