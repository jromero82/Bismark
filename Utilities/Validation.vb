Option Strict On

Imports System.Text.RegularExpressions

Namespace Bismark.Utilities
    Public Class Validation

        ''' <summary>
        ''' Validates the format of the Employee Id.
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns>True if the format is correct, otherwise False.</returns>
        ''' <remarks></remarks>
        Public Shared Function ValidateEmployeeId(ByVal id As Integer) As Boolean
            If id.ToString.Length = 8 AndAlso Not id.ToString.StartsWith("0") Then
                Return True
            End If
            Throw (New ArgumentException("The Employee Id is invalid."))
        End Function

        Public Shared Function ValidatePhoneNumber(ByVal phoneNumber As String) As Boolean
            If phoneNumber Like "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]" Then
                Return True
            End If
            Throw New ArgumentException("The phone number must consist of strictly 10 digits.")
        End Function

        Public Shared Function ValidateFieldLength(ByVal value As String, ByVal sizeChoice As SizeOperator, ByVal propertyName As String, ByVal maxLength As Integer) As Boolean
            If sizeChoice = SizeOperator.NoGreaterThan Then
                If value.Length > maxLength Then
                    Throw New ArgumentException("The " & propertyName & " cannot be greater than " & maxLength & " characters in length.")
                End If
            Else
                If value.Length <> maxLength Then
                    Throw New ArgumentException("The " & propertyName & " must be " & maxLength & " characters in length.")
                End If
            End If
            Return True
        End Function

        Public Shared Function ValidateStringValue(ByVal value As String, ByVal state As CheckStringValue, ByVal propertyName As String) As Boolean
            If state = CheckStringValue.MustNotBeNullOrEmpty Then
                If String.IsNullOrEmpty(value) Then
                    Throw New ArgumentException("The " & propertyName & " cannot be null or empty.")
                End If
            ElseIf state = CheckStringValue.MustBeNumeric Then
                If Not Integer.TryParse(value, New Integer) Then
                    Throw New ArgumentException("The " & propertyName & " must be numeric.")
                End If
            End If
            Return True
        End Function

        Public Shared Function ValidateNumericValue(ByVal value As Decimal, ByVal state As CheckNumericValue, ByVal propertyName As String, Optional ByVal min As Single = 0, Optional ByVal max As Single = 0) As Boolean
            If state = CheckNumericValue.MustNotBeNegative Then
                If value < 0 Then
                    Throw New ArgumentException("The " & propertyName & " cannot be less than zero.")
                End If
            ElseIf state = CheckNumericValue.MustNotBeZero Then
                If value = 0 Then
                    Throw New ArgumentException("The " & propertyName & " cannot be zero.")
                End If
            ElseIf state = CheckNumericValue.MustBeBetween Then
                If value < min OrElse value > max Then
                    Throw New ArgumentException("The " & propertyName & " must be between " & min & " and " & max & ".")
                End If
            End If
            Return True
        End Function

        Public Shared Function ValidatePostalCode(ByVal value As String) As Boolean
            Dim pattern As String = "\b(?([A-Z])[^DFIOQUWZ])\d(?([A-Z])[^DFIOQU])\d(?([A-Z])[^DFIOQUWZ])\d\b"
            Dim rgx As New Regex(pattern, RegexOptions.IgnoreCase)
            Dim matches As MatchCollection = rgx.Matches(value)
            If matches.Count = 0 Then
                Throw New ArgumentException("Invalid Postal Code.")
            End If
            Return True
        End Function

        Public Shared Function ValidateEmail(ByVal value As String) As Boolean
            Dim pattern As String = "^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$"
            Dim rgx As New Regex(pattern, RegexOptions.IgnoreCase)
            Dim matches As MatchCollection = rgx.Matches(value)
            If matches.Count = 0 Then
                Throw New ArgumentException("Invalid Email Address.")
            End If
            Return True
        End Function

        Public Shared Function ValidateDate(ByVal value As Date, ByVal checkType As CheckDateValue, ByVal compareTo As Date, ByVal propertyName As String) As Boolean
            If checkType = CheckDateValue.MustBeEqualTo Then
                If value <> compareTo Then
                    Throw New ArgumentException(propertyName + " must be " + compareTo.ToShortDateString())
                End If
            ElseIf checkType = CheckDateValue.MustBeGreaterThanOrEqualTo Then
                If value < compareTo Then
                    Throw New ArgumentException(propertyName + " cannot be less than " + compareTo.ToShortDateString())
                End If
            ElseIf checkType = CheckDateValue.MustBeLessThanOrEqualTo Then
                If value > compareTo Then
                    Throw New ArgumentException(propertyName + " cannot be greater then " + compareTo.ToShortDateString())
                End If
            End If
            Return True
        End Function


    End Class
End Namespace
