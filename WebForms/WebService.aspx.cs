using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Web.Services;
using Bismark.BOL;
using Bismark.Utilities;

namespace Bismark.WebForms
{
    public partial class WebService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static void ValidateInput(string property, string value)
        {
            var item = Item.Create();
            switch (property)
            {
                case "Name":
                    item.Name = value;
                    break;
                case "Description":
                    item.Description = value;
                    break;
                case "Price":
                    item.Price = Convert.ToDecimal(value);
                    break;
                case "Quantity":
                    item.Quantity = Convert.ToInt32(value);
                    break;
                case "Justification":
                    item.Justification = value;
                    break;
                case "Source":
                    item.Source = value;
                    break;
            }
        }

        [WebMethod(EnableSession = true)]
        public static List<Order> GetOrderById(int orderId)
        {
            var orderList = new List<Order>();
            var employeeId = Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]);
            orderList.Add(Order.Create(employeeId, orderId));
            return orderList;
        }

        [WebMethod(EnableSession = true)]
        public static List<Order> GetOrdersByDept(Filter filter, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var employeeId = Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]);
            Department department = (Department)Enum.Parse(typeof(Department), HttpContext.Current.Session["Department"].ToString(), true);
            return OrderLists.GetOrdersByDepartment(employeeId, department, filter, dateFrom, dateTo);
        }

        [WebMethod(EnableSession = true)]
        public static List<Order> GetOrdersByEmployeeName(Filter filter, string firstName = null,
            string lastName = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var employeeId = Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]);
            Department department = (Department)Enum.Parse(typeof(Department), HttpContext.Current.Session["Department"].ToString(), true);
            return OrderLists.GetOrdersByEmployeeName(employeeId, department, filter, firstName, lastName, dateFrom, dateTo);
        }

        [WebMethod(EnableSession = true)]
        public static List<Order> GetOrdersByEmployeeId(Filter filter, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var employeeId = Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]);
            return OrderLists.GetOrdersByEmployeeId(employeeId, filter, dateFrom, dateTo);
        }

        [WebMethod(EnableSession = true)]
        public static bool ProcessItem(int itemId, decimal price, int quantity, string source, ItemStatus status, string reason)
        {
            var item = Item.Create(itemId);
            item.Price = price;
            item.Quantity = quantity;
            item.Source = source;
            item.Reason = reason;
            if (!item.Process(Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]), status))
                return false;
            return true;
        }

        [WebMethod(EnableSession = true)]
        public static bool UpdateItem(Dictionary<string, string> dto)
        {
            var item = Item.Create(Convert.ToInt32(dto["itemId"]));
            item.Name = dto["name"];
            item.Description = dto["description"];
            item.Price = Convert.ToDecimal(dto["price"]);
            item.Quantity = Convert.ToInt32(dto["quantity"]);
            item.Justification = dto["justification"];
            item.Source = dto["source"];
            return ItemCUD.Update(Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]), item);
        }

        [WebMethod(EnableSession = true)]
        public static void CloseOrder(string orderId)
        {
            var employeeId = Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]);
            var order = Order.Create(employeeId, Convert.ToInt32(orderId));
            order.Close(employeeId);

            var employee = Employee.Create(order.EmployeeId);
            var email = employee.Email;

            var client = new SmtpClient("localhost");
            var msg = new MailMessage();
            msg.From = new MailAddress(HttpContext.Current.Session["Email"].ToString());
            msg.To.Add(email);
            msg.IsBodyHtml = true;
            msg.Body = "<h1>Your Order Has Been Processed</h1>" +
                       "Order Number: " + order.OrderId + "<br />";
            msg.Subject = "Your Order Has Been Processed";
            client.Send(msg);
        }
    }
}