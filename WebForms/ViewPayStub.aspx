<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPayStub.aspx.cs" Inherits="Bismark.ViewPayStub" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/ViewPayStub.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="payStub">
                      
        <asp:Label ID="lblEmpName" runat="server"></asp:Label>

        <label id="empIdLabel">Employee ID:</label>
        <asp:Label ID="lblEmpId" runat="server"></asp:Label>

        <label id="deptLabel">Department:</label>
        <asp:Label ID="lblDept" runat="server"></asp:Label>

        <label id="periodEndLabel">Period End:</label>
        <asp:Label ID="lblPeriodEnd" runat="server"></asp:Label>

        <label id="jobAssignmentLabel">Job Assignment:</label>
        <asp:Label ID="lblJobAssignment" runat="server"></asp:Label>

        <label id="earningsLabel">STATEMENT OF EARNINGS</label>
        <label id="deductionsLabel">EMPLOYEE DEDUCTIONS</label>
            
        <label id="typeLabel">TYPE</label>
        <label id="hoursLabel">HOURS</label>
        <label id="rateLabel">RATE</label>
        <label id="amountLabel">AMOUNT</label>            
        <label id="ytdLabel">YTD</label>
        <label id="type2Label">TYPE</label>
        <label id="currentLabel">CURRENT</label>
        <label id="ytd2Label">YTD</label>
        <label id="type3Label">TYPE</label>
        <label id="current2Label">CURRENT</label>
        <label id="ytd3Label">YTD</label>

        <label id="salaryLabel">SALARY</label>
        <asp:Label ID="lblSalaryAmt" runat="server" Width="70px" BorderStyle="None" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblSalaryYTDAmt" runat="server" Width="70px" CssClass="textAlignRight" BorderStyle="None"></asp:Label>

        <label id="bonusLabel">BONUS</label>
        <asp:Label ID="lblBonusAmt" runat="server" Width="70px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblBonusYTDAmt" runat="server" CssClass="textAlignRight" Width="70px"></asp:Label>

        <label id="incomeTaxLabel">INCOME TAX</label>
        <asp:Label ID="lblIncomeTax" runat="server" Width="56px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblIncomeTaxYTD" runat="server" Width="56px" CssClass="textAlignRight"></asp:Label>

        <label id="eiLabel">E.I.</label>
        <asp:Label ID="lblEI" runat="server" Width="56px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblEIYTD" runat="server" CssClass="textAlignRight" Width="56px"></asp:Label>

        <label id="cppLabel">C.P.P</label>
        <asp:Label ID="lblCPP" runat="server" CssClass="textAlignRight" Width="56px"></asp:Label>
        <asp:Label ID="lblCPPYTD" runat="server" CssClass="textAlignRight" Width="56px"></asp:Label>

        <label id="pensionLabel">PENSION PLAN</label>
        <asp:Label ID="lblPension" runat="server" Width="56px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblPensionYTD" runat="server" CssClass="textAlignRight" Width="56px"></asp:Label>

        <label id="summaryLabel">SUMMARY</label>
        <label id="grossLabel">GROSS PAY</label>
        <label id="dedLabel">DEDUCTIONS</label>
        <label id="netPayLabel">NET PAY</label>
        <label id="allocationLabel">NET PAY ALLOCATION</label>

        <label id="currentLabel3">Current</label>
        <label id="ytd4Label">Year-To-Date</label>

        <asp:Label ID="lblCurrGross" runat="server" Width="90px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblCurrDed" runat="server" Width="90px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblCurrNet" runat="server" Width="90px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblYTDGross" runat="server" Width="90px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblYTDDed" runat="server" Width="90px" CssClass="textAlignRight"></asp:Label>
        <asp:Label ID="lblYTDNet" runat="server" Width="90px" CssClass="textAlignRight"></asp:Label>
        
        <asp:Label ID="lblAllocationAmt" runat="server" Width="90px" CssClass="textAlignRight"></asp:Label>
        <label id="fakeAllocationLabel">DEPOSIT 112 766865 8765389</label>
        

    </div>
    </form>
</body>
</html>
