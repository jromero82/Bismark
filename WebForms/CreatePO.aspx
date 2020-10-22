<%@ Page Title="" Language="C#" MasterPageFile="~/Bismark.Master" AutoEventWireup="true" CodeBehind="CreatePO.aspx.cs" Inherits="Bismark.WebForms.CreatePO" ClientIDMOde="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="CSS/CreatePO.css" />
    <script type="text/javascript">
//        function ValidateInput(property, value) {
//            $.ajax({
//                type: 'POST',
//                url: 'CreatePO.aspx/ValidateInput',
//                data: '{"property": "' + property + '", "value": "' + value + '"}',
//                contentType: 'application/json; charset=utf-8',
//                dataType: 'json',
//                success: function (msg) {
//                    var textbox = $('[data-property=' + property + ']');
//                    if (msg.d) {
//                        $(textbox).addClass('input-error');
//                        $(textbox).qtip({
//                            style: {
//                                classes: 'ui-tooltip-red ui-tooltip-shadow ui-tooltip-rounded'
//                            },
//                            content: msg.d,
//                            position: {
//                                my: 'bottom left',
//                                at: 'top right'
//                            }
//                        });
//                    }
//                    else {
//                        $(textbox).removeClass('input-error');
//                        $(textbox).qtip('destroy');
//                    }
//                },
//                error: function (xhr, status, errorThrown) {
//                    $('#messageBox').html(errorThrown);
//                }
//            });
        //        }

        // Validation of item properties.
        function ValidateInput(property, value) {
            $.ajax({
                type: 'POST',
                url: 'WebService.aspx/ValidateInput',
                data: '{"property": "' + property + '", "value": "' + value + '"}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    var textbox = $('[data-property=' + property + ']');
                    $(textbox).removeClass('input-error');
                    $(textbox).qtip('destroy');
                },
                error: function (xhr, status, errorThrown) {
                    var error = JSON.parse(xhr.responseText);
                    var textbox = $('[data-property=' + property + ']');
                    $(textbox).addClass('input-error');
                    $(textbox).qtip({
                        style: {
                            classes: 'ui-tooltip-red ui-tooltip-shadow ui-tooltip-rounded'
                        },
                        content: error.Message,
                        position: {
                            my: 'bottom left',
                            at: 'top right'
                        }
                    });
                }
            });
        }
        $(function () {
            $('#item-details input, #item-details textarea').each(function () {
                $(this).blur(function () {
                    ValidateInput($(this).attr('data-property'), $(this).val())
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="form-header">
        <h2>Purchase Order Request</h2>
        <fieldset>
            <legend>Employee Details</legend>
            Date: <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" 
                Width="100"></asp:TextBox>
            Employee: 
            <asp:TextBox ID="txtEmployee" runat="server" ReadOnly="True" 
                Width="108px"></asp:TextBox>
            Department: 
            <asp:TextBox ID="txtDepartment" runat="server" ReadOnly="True" 
                Width="123px"></asp:TextBox>
            Supervisor: 
            <asp:TextBox ID="txtSupervisor" runat="server" ReadOnly="True" 
                Width="108px"></asp:TextBox>
        </fieldset>
    </div>
    <div id="content">
        <fieldset id="item-details">
            <legend>Item Details</legend>
                <div class="first-column">
                    <div class="row">
                        <label for="txtName">Name: </label>
                        <asp:TextBox ID="txtName" runat="server" data-property="Name"></asp:TextBox>
                    </div>
                    <div class="row">
                        <label id="lblDescription" for="txtDescription">Description: </label>
                        <asp:TextBox ID="txtDescription" runat="server" Rows="5" Columns="20" 
                            TextMode="MultiLine" data-property="Description"></asp:TextBox>
                    </div>
                    <div class="row">
                        <label for="txtPrice">Price: </label>
                        <asp:TextBox ID="txtPrice" runat="server" Width="62px" TextMode="Number" data-property="Price"></asp:TextBox>
                        <label id="lblQuantity" for="txtQuantity">Quantity: </label>
                        <asp:TextBox ID="txtQuantity" runat="server" Width="62px" TextMode="Number" data-property="Quantity"></asp:TextBox>
                    </div>                
                </div>
                <div class="last-column">
                    <div class="row">
                        <label for="txtJustification">Justification: </label>
                        <asp:TextBox ID="txtJustification" runat="server" Rows="5" Columns="20" 
                            TextMode="MultiLine" data-property="Justification"></asp:TextBox>
                    </div>
                    <div class="row">
                        <label for="txtSource">Source: </label>
                        <asp:TextBox ID="txtSource" runat="server" data-property="Source"></asp:TextBox>
                    </div>
                    <div class="row">
                        <asp:Button ID="btnAddItem" runat="server" Text="ADD ITEM"
                            onclick="btnAddItem_Click" CssClass="cmd-button" />
                        <asp:Literal ID="messageBox" runat="server"></asp:Literal>
                    </div>
                </div> 
        </fieldset>
        <fieldset id="totals">
            <legend>Totals</legend>
            <div class="row">
                <label for="txtSubTotal">Sub Total: </label>
                <asp:TextBox ID="txtSubTotal" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
            </div>
            <div class="row">
                <label for="txtTaxTotal">Tax Total: </label>
                <asp:TextBox ID="txtTaxTotal" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
            </div>
            <div class="row">
                <label for="txtGrandTotal">Grand Total: </label>
                <asp:TextBox ID="txtGrandTotal" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
            </div>
        </fieldset>
        <div id="last-order">
            <div class="row">
                <label for="txtOrderNumber">Order Number: </label>
                <asp:TextBox ID="txtOrderId" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
            </div>
            <div class="row">
                <label for="txtOrderStatus">Order Status: </label>
                <asp:TextBox ID="txtOrderStatus" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Button ID="btnOrder" runat="server" Text="SAVE ORDER" 
                    class="cmd-button" onclick="btnOrder_Click" />
            </div>
        </div>
        <asp:GridView ID="gdvItems" runat="server" AutoGenerateColumns="False" 
        GridLines="Horizontal" Width="957px">
        <AlternatingRowStyle BackColor="WhiteSmoke" />
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Price" DataFormatString="{0:c}" 
                HeaderText="Price" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            <asp:BoundField DataField="Justification" HeaderText="Justification" />
            <asp:BoundField DataField="Source" HeaderText="Source" />
            <asp:BoundField DataField="SubTotal" DataFormatString="{0:c}" 
                HeaderText="Sub Total" />
            <asp:BoundField DataField="TaxTotal" DataFormatString="{0:c}" 
                HeaderText="Tax Total" />
            <asp:BoundField DataField="GrandTotal" DataFormatString="{0:c}" 
                HeaderText="Grand Total" />
        </Columns>
        </asp:GridView>
    </div> 
</asp:Content>
    
    
    
