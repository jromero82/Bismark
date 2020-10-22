Imports System.Text
Imports Bismark.BOL
Imports Bismark.Utilities

Namespace Bismark.UnitTests
    <TestClass()>
    Public Class OrderTest

        ''' <summary>
        ''' This method tests the creation of an empty Order object.
        ''' </summary>
        ''' <remarks></remarks>
        <TestMethod()>
        Public Sub Create_Empty_Order_Positive()
            Dim objOrder As Order = Order.Create()
            Dim employeeId As Integer = objOrder.EmployeeId

            Assert.IsTrue(TypeOf objOrder Is Order)
            Assert.IsTrue(employeeId = 0)
        End Sub

        ''' <summary>
        ''' This method tests the inability to add an order record using an incomplete Order object.
        ''' </summary>
        ''' <remarks></remarks>
        <TestMethod(), ExpectedException(GetType(DataException), "Unable to add an incomplete Order record.")>
        Public Sub Create_Order_Record_Negative()
            Dim objOrder As Order = Order.Create()
            OrderCUD.Create(objOrder)
        End Sub

        ''' <summary>
        ''' This method tests the successful creation of an Order record  with multiple items.
        ''' </summary>
        ''' <remarks></remarks>
        <TestMethod()>
        Public Sub Create_Order_Record_Positive()
            Dim objOrder As Order = Order.Create()
            Dim objItem As Item = Item.Create()

            For i As Integer = 1 To 10
                objItem.Name = "Stack of Paper"
                objItem.Description = "Printer Paper"
                objItem.Price = 22.99
                objItem.Quantity = 50
                objItem.Justification = "One stack left in stock."
                objItem.Source = "Staples"
                objOrder.Items.Add(objItem)
            Next
            objOrder.EmployeeId = 10000001
            OrderCUD.Create(objOrder)
        End Sub

        ''' <summary>
        ''' This method tests the inability to add an incomplete item to the order.
        ''' </summary>
        ''' <remarks></remarks>
        '<TestMethod(), ExpectedException(GetType(InvalidOperationException), "Unable to add an incomplete item.")>
        'Public Sub Add_Item_Negative()
        '    Dim objOrder As Order = Order.Create()
        '    Dim objItem As Item = Item.Create()

        '    objItem.Description = "Printer Paper"
        '    objItem.Price = 22.99
        '    objItem.Quantity = 50
        '    objItem.Justification = "One stack left in stock."
        '    objItem.Source = "Staples"
        '    objOrder.Items.Add(objItem)
        'End Sub

        '<TestMethod()>
        'Public Sub GetOrdersByDepartment_Positive()
        '    Dim orderList As New List(Of Order)
        '    orderList = OrderLists.GetOrdersByDepartment(Department.HumanResources, Filter.All)
        '    Assert.IsTrue(orderList.Count = 2)
        'End Sub

        '<TestMethod()>
        'Public Sub GetOrdersByEmployee_Positive()
        '    Dim orderList As New List(Of Order)
        '    orderList = OrderLists.GetOrdersByEmployeeName(10000001, Filter.All)
        '    Assert.IsTrue(orderList(0).EmployeeId = 10000001)
        '    Assert.IsTrue(orderList.Count = 2)
        'End Sub

    End Class
End Namespace