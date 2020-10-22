<%@ Page Title="" Language="C#" MasterPageFile="~/Bismark.Master" AutoEventWireup="true" CodeBehind="BrowsePO.aspx.cs" Inherits="Bismark.WebForms.BrowsePO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="CSS/ProcessPO.css" />
    <script type="text/javascript" src="Scripts/poShared.js"></script>
    <script type="text/javascript">
        var orderId;
        var itemId;
        var ordersRow;
        var itemsRow;

        function getOrdersByEmployeeId(filter, dateFrom, dateTo) {
            clearPageData();
            var dto = {};
            dto.filter = filter;
            dto.dateFrom = dateFrom;
            dto.dateTo = dateTo;
            $.ajax({
                type: 'POST',
                url: 'WebService.aspx/GetOrdersByEmployeeId',
                data: JSON.stringify(dto),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    displayOrders(data.d);
                },
                error: function (xhr, status, errorThrown) {
                    var error = JSON.parse(xhr.responseText);
                    alert(error.Message);
                }
            });
        }

        function updateItem(name, description, price, quantity, justification, source) {
            var item = {};
            item.itemId = itemId;
            item.orderId = orderId;
            item.name = name;
            item.description = description;
            item.price = parseFloat(price);
            item.quantity = parseInt(quantity);
            item.justification = justification;
            item.source = source;
            var dto = {"dto": item };
            $.ajax({
                type: 'POST',
                url: 'WebService.aspx/UpdateItem',
                data: JSON.stringify(dto),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    displayOrders(data.d);
                },
                error: function (xhr, status, errorThrown) {
                    var error = JSON.parse(xhr.responseText);
                    alert(error.Message);
                }
            });
        }

        $(function () {
            getOrdersByEmployeeId(0, null, null);
            $('#btnGo').click(function () {
                if ($('#txtOrderId').val() != '') {
                    getOrderById($('#txtOrderId').val());
                }
                else {
                    if ($('#chkDateRange').is(':checked'))
                        getOrdersByEmployeeId($('#ddlFilter').val(), $('#dtpFrom').val(), $('#dtpTo').val());
                    else
                        getOrdersByEmployeeId($('#ddlFilter').val(), null, null);
                }
                return false;
            });

            $('#txtName,#txtDescription,#txtPrice,#txtQuantity,#txtJustification,#txtSource').dblclick(function () {
                var itemStatus = $('#gdvItems tbody tr:nth-child(' + (itemsRow + 1) + ')').find('td:nth-child(12)').text();
                if (itemStatus == 'Pending')
                    $(this).removeAttr('readonly');
            });

            $('#txtName,#txtDescription,#txtPrice,#txtQuantity,#txtJustification,#txtSource').blur(function () {
                ValidateInput($(this).attr('data-property'), $(this).val())
                $(this).attr('readonly', 'readonly');
            });

            $('#btnRemove').click(function () {
                if (itemId != 0) {
                    var name = $('#txtName').val();
                    var description = 'No longer needed.';
                    var price = 0;
                    var quantity = 0;
                    var justification = $('#txtJustification').val();
                    var source = $('#txtSource').val();
                    updateItem(name, description, price, quantity, justification, source)
                }
                return false;
            });

            $('#btnSave').click(function () {
                if (itemId != 0) {
                    var name = $('#txtName').val();
                    var description = $('#txtDescription').val();
                    var price = $('#txtPrice').val();
                    var quantity = $('#txtQuantity').val();
                    var justification = $('#txtJustification').val();
                    var source = $('#txtSource').val();
                    updateItem(name, description, price, quantity, justification, source)
                }
                return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="form-header">
        <h2>Browse Purchase Orders</h2>
        <div id="orderId-search">
            <div class="row">
                <label for="txtOrderId">Purchase Order Id:</label>
            </div>
            <div class="row">
                <input id="txtOrderId" type="text"  />
            </div>
        </div>
        <div id="dates">
            <div class="row">
                <label for="dtpFrom">From: </label>
                <input id="dtpFrom" type="text" />
            </div>
            <div class="row">
                <label for="dtpTo">To: </label>
                <input id="dtpTo" type="text" />    
            </div>
        </div>
        <div id="search-filter">
            <div class="row">
                <label for="ddlFilter">Filter: </label>
                <select id="ddlFilter">
                    <option value="0">Pending</option>
                    <option value="1">Closed</option>
                    <option value="2">All</option>
                </select>
            </div>
            <div class="row">
                <div style="float: right">
                    <input id="chkDateRange" type="checkbox" />
                    <label for="chkDateRange"">Within Date Range</label>
                </div>
            </div>
        </div>
        <input id="btnGo" type="button" value="GO" />
    </div>    
    <div>
        <div id="wrapper-orders">   
            <h2>PURCHASE ORDERS:</h2>  
            <div id="gdvOrders-wrapper">
                <div id="gdvOrders-scroll">   
                    <table id="gdvOrders">
                        <thead>
                            <tr>
                                <th>Order Id</th>
                                <th>Order Date</th>
                                <th>Employee Name</th>
                                <th>Sub Total</th>
                                <th>Tax Total</th>
                                <th>Grand Total</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>         
                        </tbody>
                    </table>
                </div>
            </div>
            <div style="float: right">
                <label for="txtSubTotal">Sub Total: </label>
                <input id="txtSubTotal" type="text" readonly="readonly" />
                <label for="txtTaxTotal">Tax Total: </label>
                <input id="txtTaxTotal" type="text" readonly="readonly" />
                <label for="txtGrandTotal">Grand Total: </label>
                <input id="txtGrandTotal" type="text" readonly="readonly" />
            </div>
        </div>
    </div>
    <div id="item-details">
        <div class="first-column">
            <div class="row">
                <label for="txtName">Name: </label>
                <input id="txtName" type="text" data-property="Name" readonly="readonly" />
            </div>
            <div class="row">
                <label id="lblDescription" for="txtDescription">Description: </label>
                <textarea id="txtDescription" data-property="Description" rows="5" cols="17" readonly="readonly"></textarea>
            </div>
            <div class="row">
                <label for="txtPrice">Price: </label>
                <input id="txtPrice" type="number" data-property="Price" readonly="readonly" />
                <label id="lblQuantity" for="txtQuantity">Quantity: </label>
                <input id="txtQuantity" type="number" data-property="Quantity" readonly="readonly" />
            </div>              
        </div>
        <div class="last-column">
            <div class="row">
                <label for="txtJustification">Justification: </label>
                <textarea id="txtJustification" data-property="Justification" rows="5" cols="17" readonly="readonly"></textarea>
            </div>
            <div class="row">
                <label for="txtSource">Source: </label>
                <input id="txtSource" type="text" data-property="Source" readonly="readonly" />
            </div>  
            <br />
            <div class="row"><input id="btnRemove" type="button" value="NO LONGER REQUIRED" /></div>
            <div class="row"><input id="btnSave" type="button" value="SAVE CHANGES" /></div>
        </div> 
    </div>
<div>
        <div id="wrapper-items" style="margin-bottom: 25px;">
            <h2>PURCHASE ORDER DETAILS:</h2>
            <div id="gdvItems-wrapper">
                <div id="gdvItems-scroll">
                    <table id="gdvItems">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Price</th>
                                <th>Quantity</th>
                                <th>Justification</th>
                                <th>Source</th>
                                <th>Sub Total</th>
                                <th>Tax Total</th>
                                <th>Grand Total</th>
                                <th>Status</th>
                                <th>Reason</th>
                            </tr>
                        </thead>
                        <tbody>         
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
