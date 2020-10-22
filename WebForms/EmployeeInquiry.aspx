<%@ Page Title="" Language="C#" MasterPageFile="~/Bismark.Master" AutoEventWireup="true"
    CodeBehind="EmployeeInquiry.aspx.cs" Inherits="Bismark.WebForms.EmployeeInquire" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/EmployeeInquiry.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $('#txtDateRangeA, #txtDateRangeB').datepicker();
            $('#gdvPayStubs').hide();
            $('#btnGetPayStubs').click(function () {
                getPayStubs($('#txtEmpId').val(),
                            $('#txtDateRangeA').val(),
                            $('#txtDateRangeB').val());
                return false;
            });

            $('#btnClear').click(function () {
                $('.slimScrollDiv').hide();
                $('#gdvPayStubs tbody').empty();
                $('#txtDateRangeA').val('-- Pick a date --');
                $('#txtDateRangeB').val('-- Pick a date --');
                return false;
            });            

            $('#gdvPayStubs-scroll').slimScroll({
                height: '250px'
            });

            $('.slimScrollDiv').hide();

            $('#gdvPayStubs tbody tr').live('click', function () {
                var id = $(this).find('td:nth-child(1)').text();
                window.open('/ViewPayStub.aspx?id=' + id);
            });


            //Creates the table row hover effect
            $('#gdvPayStubs tbody tr').live('mouseover mouseout', function (event) {
                if (event.type == 'mouseover') {
                    $(this).css("background-color", "#003B9B");
                    $(this).css("color", "white");
                    $(this).find("a").css("color", "white");
                    $(this).css("cursor", "pointer");
                } else {
                    $(this).css("background-color", "");
                    $(this).css("color", "black");
                    $(this).find("a").css("color", "black");
                }
            });
        });       

        function getPayStubs(empId, dateFrom, dateTo) {
            $('#gdvPayStubs tbody').empty();
            var params = {};
            params.empId = empId;
            params.dateFrom = dateFrom;
            params.dateTo = dateTo;

            $.ajax({
                type: 'POST',
                url: 'EmployeeInquiry.aspx/getPayStubs',
                data: JSON.stringify(params),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.d)
                        displayStubs(data.d);
                },
                error: function (xhr, status, errorThrown) {
                    alert(xhr.responseText);
                }
            });
        }

        function displayStubs(data) {
            for (var key in data) {
                var altRow;
                if (key % 2 == 0)
                    altRow = 'class="alt-row"';
                else
                    altRow = '';

                // Convert JSON formatted date to readable short date format
                var payPeriod = data[key].PayPeriod.substr(6);
                var currentTime = new Date(parseInt(payPeriod));
                var month = currentTime.getMonth() + 1;
                var day = currentTime.getDate();
                var year = currentTime.getFullYear();
                payPeriod = month + "/" + day + "/" + year;

                $('#gdvPayStubs tbody').append($('<tr ' + altRow + '><td>' + data[key].PayStubId + '</td>' +
                '<td>' + payPeriod + '</td>' +
                '<td>$' + data[key].GrossPay.toFixed(2) + '</td>' +
                '<td>$' + data[key].YTDGrossPay.toFixed(2) + '</td>' +
                '<td>$' + data[key].BonusPay.toFixed(2) + '</td>' +
                '<td>$' + data[key].YTDBonusPay.toFixed(2) + '</td>' +
                '<td>$' + data[key].IncomeTaxDeduction.toFixed(2) + '</td>' +
                '<td>$' + data[key].YTDIncomeTaxDeduction.toFixed(2) + '</td>' +
                '<td>$' + data[key].CPPDeduction.toFixed(2) + '</td>' +
                '<td>$' + data[key].YTDCPPDeduction.toFixed(2) + '</td>' +
                '<td>$' + data[key].EIDeduction.toFixed(2) + '</td>' +
                '<td>$' + data[key].YTDEIDeduction.toFixed(2) + '</td>' +
                '<td>$' + data[key].PensionDeduction.toFixed(2) + '</td>' +
                '<td>$' + data[key].YTDPensionDeduction.toFixed(2) + '</td>' +
                '<td>$' + data[key].NetPay.toFixed(2) + '</td>' +
                '<td>$' + data[key].YTDNetPay.toFixed(2) + '</td>' +
                '<td>' + data[key].EmployeeId + '</td></tr>'));

                $('#gdvPayStubs tbody td:nth-child(1)').hide();
                $('#gdvPayStubs tbody td:nth-child(3)').hide();
                $('#gdvPayStubs tbody td:nth-child(4)').hide();
                $('#gdvPayStubs tbody td:nth-child(5)').hide();
                $('#gdvPayStubs tbody td:nth-child(6)').hide();
                $('#gdvPayStubs tbody td:nth-child(7)').hide();
                $('#gdvPayStubs tbody td:nth-child(8)').hide();
                $('#gdvPayStubs tbody td:nth-child(9)').hide();
                $('#gdvPayStubs tbody td:nth-child(10)').hide();
                $('#gdvPayStubs tbody td:nth-child(11)').hide();
                $('#gdvPayStubs tbody td:nth-child(12)').hide();
                $('#gdvPayStubs tbody td:nth-child(13)').hide();
                $('#gdvPayStubs tbody td:nth-child(14)').hide();
                $('#gdvPayStubs tbody td:nth-child(16)').hide();
                $('#gdvPayStubs tbody td:nth-child(17)').hide();
                $('#gdvPayStubs').show();
                $('.slimScrollDiv').show();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h3>
        Employee Inquiry</h3>
    <div class="left-col">
    <!-- Personal Information -->
    <fieldset name="inq-personal" title="Personal">
        <legend>Personal Information</legend>
        <!--Employee ID -->
        <asp:Label ID="lblEmpId" runat="server" Text="Employee ID" AssociatedControlID="txtEmpId"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtEmpId" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!--First name -->
        <asp:Label ID="lblFname" runat="server" Text="First Name" AssociatedControlID="txtFname"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtFname" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!--Middle Initial -->
        <asp:Label ID="lblMInitial" runat="server" Text="Middle Initial" AssociatedControlID="txtMiddleInitial"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtMiddleInitial" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!--Last name -->
        <asp:Label ID="lblLName" runat="server" Text="Last Name" AssociatedControlID="txtLName"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtLName" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!--Address -->
        <asp:Label ID="lblAddress" runat="server" Text="Address" AssociatedControlID="txtAddress"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtAddress" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!--City -->
        <asp:Label ID="lblCity" runat="server" Text="City" AssociatedControlID="txtCity"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtCity" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!--Province -->
        <asp:Label ID="lblProvince" runat="server" Text="Province" AssociatedControlID="txtProvince"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtProvince" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!--Postal Code -->
        <asp:Label ID="lblPostalCode" runat="server" Text="Postal Code" AssociatedControlID="txtPostalCode"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtPostalCode" runat="server" ReadOnly="True"></asp:TextBox><br />
    </fieldset>
    <br />
    <!-- Employment Information -->
    <fieldset>
        <legend>Employment Information</legend>
        <!-- Status -->
        <asp:Label ID="lblStatus" runat="server" Text="Employment Status" AssociatedControlID="txtEmpStatus"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtEmpStatus" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!-- Date of Birth -->
        <asp:Label ID="lblDOB" runat="server" Text="Date of Birth" AssociatedControlID="txtDOB"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtDOB" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!-- SIN -->
        <asp:Label ID="lblSIN" runat="server" Text="SIN" AssociatedControlID="txtSIN" CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtSIN" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!-- Job Title -->
        <asp:Label ID="lblJobTitle" runat="server" Text="Job Title" AssociatedControlID="txtJobTitle"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtJobTitle" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!-- Department -->
        <asp:Label ID="lblDepartment" runat="server" Text="Department" AssociatedControlID="txtDept"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtDept" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!-- Job Start Date -->
        <asp:Label ID="lblJobStartDate" runat="server" Text="Job Start Date" AssociatedControlID="txtJobStartDate"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtJobStartDate" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!-- Hire Date -->
        <asp:Label ID="lblHireDate" runat="server" Text="Hire Date" AssociatedControlID="txtHireDate"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtHireDate" runat="server" ReadOnly="True"></asp:TextBox><br />
        <!-- Supervisor -->
        <asp:Label ID="lblSupervisor" runat="server" Text="Supervisor" AssociatedControlID="txtSupervisor"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtSupervisor" runat="server" ReadOnly="True"></asp:TextBox><br /><br />
        <asp:Label ID="lblSickDays" runat="server"></asp:Label><br /><br />
        <asp:Label ID="lblSickDayDetail" runat="server"></asp:Label>
    </fieldset>
    <br />
    </div>
    <div class="right-col">
    <!-- Payroll Information -->
    <fieldset>
        <legend>Payroll Information</legend>
        <h4>
            Year-to-Date Summary</h4>
        <!-- YTD Salary -->
        <asp:Label ID="lblYTDSalary" runat="server" Text="YTD Salary" AssociatedControlID="txtYTDSalary"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtYTDSalary" runat="server" ReadOnly="True" CssClass="textAlignRight"></asp:TextBox><br />
        <!-- YTD Bonus -->
        <asp:Label ID="lblYTDBonus" runat="server" Text="YTD Bonus" AssociatedControlID="txtYTDBonus"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtYTDBonus" runat="server" ReadOnly="True" CssClass="textAlignRight"></asp:TextBox><br />
        <!-- YTD Deductions -->
        <asp:Label ID="lblYTDDeductions" runat="server" Text="YTD Deductions" AssociatedControlID="txtYTDDeductions"
            CssClass="labels"></asp:Label>
        <asp:TextBox ID="txtYTDDeductions" runat="server" ReadOnly="True" CssClass="textAlignRight"></asp:TextBox><br />
        <br />
        <h4>
            View Paystubs</h4>
        <label>
            Select a date range of pay stubs to retrieve</label><br />
        <asp:TextBox ID="txtDateRangeA" runat="server" Text="-- Pick a date --" CssClass="textAlignCenter"></asp:TextBox>
        <label>
            -</label>
        <asp:TextBox ID="txtDateRangeB" runat="server" Text="-- Pick a date --" CssClass="textAlignCenter"></asp:TextBox>
        <asp:Button ID="btnGetPayStubs" runat="server" Text="RETRIEVE" /><br />
        <div id="gdvPaystubs-wrapper">
            <div id="gdvPayStubs-scroll">
                <table id="gdvPayStubs">
                    <caption><h4>Click a pay period to view the stub</h4></caption>
                    <thead>
                        <tr>
                            <th>
                                PERIOD END
                            </th>
                            <th>
                                NET PAY
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>                
            </div>
            <asp:Button ID="btnClear" runat="server" Text="CLEAR" />
        </div>
    </fieldset>
    <fieldset>
        <legend>Pension Calculator</legend>
        <p>Pension is calculated at 70% of the average of your top 5 paid years.  You must be at least 55 years
        of age to be eligible for retirement, however a penalty of 3% per year before age 60 will be applied.</p>
        <p>Employment of at least 5 years is required to be eligible for a pension.</p>
        <hr />
        <asp:Button ID="btnCalcPension" runat="server" Text="Calculate Pension" 
            onclick="btnCalcPension_Click" />
            <asp:Button ID="btnClear2" runat="server" Text="CLEAR" 
            onclick="btnClear2_Click" /><br /><br />
        <asp:Label ID="lblPensionDetails" runat="server" Font-Size="Medium"></asp:Label>
        <asp:Label ID="lblFullPensionRetirementDate" runat="server"></asp:Label><br /><br />
        <asp:Label ID="lblhighestYears" runat="server"></asp:Label>            
    </fieldset>
    </div>
</asp:Content>
