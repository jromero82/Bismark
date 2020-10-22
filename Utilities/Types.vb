Option Strict On
Imports System.ComponentModel
Namespace Bismark.Utilities

    Public Enum AuthLevel
        General
        Supervisor
        CEO
    End Enum

    Public Enum EmployeeGetType
        EntireFile
        Lookup
    End Enum

    Public Enum SalaryAdjustmentGetType
        getPending
        getHistory
    End Enum

    Public Enum SalaryGetType
        getCurrent
        getHistory
    End Enum

    Public Enum BonusGetType
        getPending
        getHistory
    End Enum

    Public Enum Department
        HumanResources = 1
        CustomerService = 2
        AccountsReceivable = 3
        Sales = 4
        Executive = 5
    End Enum

    Public Enum EmploymentStatus
        Terminated = 0
        Active = 1
        Retired = 2
    End Enum

    Public Enum Filter
        Pending
        Closed
        All
    End Enum

    Public Enum OrderStatus
        Pending
        Closed
    End Enum

    Public Enum ItemStatus
        Pending
        Approved
        Denied
    End Enum

    Public Enum SizeOperator
        MustBeEqualTo
        NoGreaterThan
    End Enum

    Public Enum CheckStringValue
        MustBeNumeric
        MustNotBeNullOrEmpty
    End Enum

    Public Enum CheckNumericValue
        MustNotBeZero
        MustNotBeNegative
        MustBeBetween
    End Enum

    Public Structure DataParameter
        Public Name As String
        Public Value As Object
        Public DataType As SqlDbType
        Public Size As Integer
        Public Direction As ParameterDirection

        Public Sub New(ByVal name As String, ByVal value As Object, ByVal direction As ParameterDirection, ByVal datatype As SqlDbType, Optional ByVal size As Integer = 0)
            Me.Name = name
            Me.Value = value
            Me.DataType = datatype
            Me.Size = size
            Me.Direction = direction
        End Sub
    End Structure

#Region "Interfaces & Classes"

    Public Interface IEmployee

        Property EmployeeId As Integer
        Property FirstName As String
        Property LastName As String
        Property MiddleInitial As String
        Property Address As String
        Property City As String
        Property Province As String
        Property PostalCode As String
        Property WorkPhone As String
        Property CellPhone As String
        Property Email As String
        Property DateOfBirth As Date
        Property SIN As String
        Property Status As Integer
        Property HireDate As Date
        Property JobStartDate As Date
        Property JobId As Integer
        Property Salary As Decimal
        Property SalaryEffectiveDate As Date
        Property PrevSalary As Decimal

    End Interface

    Public Interface ISalaryAdjustment

        Property SalaryAdjustmentId As Integer
        Property PercentIncrease As Decimal
        Property EffectiveDate As Date

    End Interface

    Public Interface IBonus

        Property BonusId As Integer
        Property PercentBonus As Decimal
        Property FixedBonus As Decimal
        Property EffectiveDate As Date

    End Interface

    Public Interface ISickDay
        Property SickDayId As Integer
        Property [Date] As Date
        Property IsFullDay As Boolean
    End Interface

    Public Enum CheckDateValue
        MustBeGreaterThanOrEqualTo
        MustBeLessThanOrEqualTo
        MustBeEqualTo
    End Enum

    Public Interface IOrder
        Property OrderId As Integer
        Property EmployeeId As Integer
        ReadOnly Property EmployeeName As String
        ReadOnly Property OrderDate As DateTime
        Property Department As Department?
        ReadOnly Property SubTotal As Decimal
        ReadOnly Property TaxTotal As Decimal
        ReadOnly Property GrandTotal As Decimal
        ReadOnly Property Status As OrderStatus
        ReadOnly Property Items As ItemList
    End Interface

    Public Interface IItem
        ReadOnly Property ItemId As Integer
        ReadOnly Property OrderId As Integer
        Property Name As String
        Property Description As String
        Property Price As Decimal
        Property Quantity As Integer
        Property Justification As String
        Property Source As String
        ReadOnly Property SubTotal As Decimal
        ReadOnly Property TaxTotal As Decimal
        ReadOnly Property GrandTotal As Decimal
        ReadOnly Property Status As ItemStatus
        Property Reason As String
        ReadOnly Property IsComplete As Boolean
        Function Process(ByVal employeeId As Integer, ByVal itemStatus As ItemStatus) As Boolean
    End Interface

    <Serializable()>
    Public Class ItemList
        Inherits List(Of IItem)
        Private IsCopy As Boolean
        Public Overloads Sub Add(ByVal item As IItem)
            If Not item.IsComplete Then
                Throw New InvalidOperationException("Unable to add an incomplete item.")
            End If
            For Each objItem In Me
                If item.Name = objItem.Name AndAlso
                    item.Description = objItem.Description AndAlso
                    item.Price = objItem.Price AndAlso
                    item.Source = objItem.Source Then
                    objItem.Quantity += item.Quantity
                    IsCopy = True
                    Exit For
                End If
            Next
            If Not IsCopy Then
                MyBase.Add(item)
            End If
            IsCopy = False
        End Sub
    End Class

#End Region

End Namespace