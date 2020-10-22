<%@ Page Title="" Language="C#" MasterPageFile="~/Bismark.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Bismark.WebForms.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="CSS/Master.css" />
    <style type="text/css">
        label 
        {
            width: 70px;   
            text-align: right;
            display: inline-block;
        }        
        input[type=submit] { float: right; }
        #login-controls { width: 232px; margin: 0 auto;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="login">
        <h2>Bismark Intranet System Login</h2>
        <hr />        
        <br />
        <div id="login-controls">
            <asp:Panel ID="loginError" runat="server">
                <asp:Literal ID="txtError" runat="server"></asp:Literal>
                
            </asp:Panel>
            <div class="row"> 
                <label for="txtEmployeeId">Employee ID:</label> 
                <asp:TextBox ID="txtEmployeeId" runat="server"></asp:TextBox>
            </div>
            <div class="row">
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            <br />
            <div class="row">
                <asp:Button ID="btnLogin" runat="server" Text="LOGIN" 
                    onclick="btnLogin_Click" />
            </div>
        </div>
    </div>
</asp:Content>
