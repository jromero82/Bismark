<%@ Page Title="" Language="C#" MasterPageFile="~/Bismark.Master" AutoEventWireup="true" CodeBehind="ProcessPO.aspx.cs" Inherits="Bismark.WebForms.ProcessPO" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="CSS/ProcessPO.css" />
    <script type="text/javascript">
        var lastOrderRowColor;
        var lastItemRowColor;
        var itemId = 0;
        var orderId = 0;

        // Clear required data.
        function clearPageData() {
            $('#item-details input[type=text],#item-details input[type=hidden],#item-details input[type=number],#item-details textarea').each(function () {
                $(this).val('');
            });
            $('#gdvOrders tbody,#gdvItems tbody').empty();
            $('#txtSubTotal,#txtTaxTotal,#txtGrandTotal').val('');
        }

        function clearItems() {
            $('#item-details input[type=text],#item-details input[type=hidden],,#item-details input[type=number],#item-details textarea').each(function () {
                $(this).val('');
            });
            $('#gdvItems tbody').empty();
        }

        // Get order by order Id.
        function getOrderById(orderId) {
            clearPageData();
            var dto = {};
            dto.orderId = orderId;
            $.ajax({
                type: 'POST',
                url: 'WebService.aspx/GetOrderById',
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

        // Get orders by the current logged in employee's department
        function getOrdersByDept(filter, dateFrom, dateTo) {
            clearPageData();
            var dto = {};
            dto.filter = parseInt(filter);
            dto.dateFrom = dateFrom;
            dto.dateTo = dateTo;
            $.ajax({
                type: 'POST',
                url: 'WebService.aspx/GetOrdersByDept',
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


        // Get orders by employee names from the current logged in employee's department 
        function getOrdersByEmployee(filter, firstName, lastName, dateFrom, dateTo) {
            clearPageData();
            var dto = {};
            dto.filter = filter;
            dto.firstName = firstName;
            dto.lastName = lastName;
            dto.dateFrom = dateFrom;
            dto.dateTo = dateTo;
            $.ajax({
                type: 'POST',
                url: 'WebService.aspx/GetOrdersByEmployeeName',
                data: JSON.stringify(dto),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d)
                        displayOrders(data.d);
                },
                error: function (xhr, status, errorThrown) {
                    var error = JSON.parse(xhr.responseText);
                    alert(error.Message);
                }
            });
        }

        // Attempt to close the order.
        function CloseOrder(orderId) {
            var dto = {};
            dto.orderId = orderId;
            $.ajax({
                type: 'POST',
                url: 'WebService.aspx/CloseOrder',
                data: JSON.stringify(dto),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.d)
                        alert(data.d);
                },
                error: function (xhr, status, errorThrown) {
                    var error = JSON.parse(xhr.responseText);
                    alert(error.Message);
                }
            });
        }

        // Fill the orders table. 
        function displayOrders(data) {
            for (var key in data) {      
                var altRow;
                if (key % 2 == 0)
                    altRow = 'class="alt-row"';
                else
                    altRow = '';
                // Convert JSON formatted date to readable short date format 
                var orderDate = data[key].OrderDate.substr(6);
                var currentTime = new Date(parseInt(orderDate));
                var month = currentTime.getMonth() + 1;
                var day = currentTime.getDate();
                var year = currentTime.getFullYear();
                orderDate = month + "/" + day + "/" + year;

                var orderStatus;
                switch (data[key].Status) {
                    case 0:
                        orderStatus = 'Pending';
                        break;
                    case 1:
                        orderStatus = 'Closed';
                        break;
                }
                $('#gdvOrders tbody').append($('<tr ' + altRow + '><td>' + data[key].OrderId + '</td>' +
                                                    '<td>' + orderDate + '</td>' +
                                                    '<td>' + data[key].EmployeeName + '</td>' +
                                                    '<td>' + JSON.stringify(data[key].Items) + '</td>' +
                                                    '<td>$' + data[key].SubTotal.toFixed(2) + '</td>' +
                                                    '<td>$' + data[key].TaxTotal.toFixed(2) + '</td>' +
                                                    '<td>$' + data[key].GrandTotal.toFixed(2) + '</td>' +
                                                    '<td>' + orderStatus + '</td></tr>'));
            }
            $('#gdvOrders tbody td:nth-child(4)').hide();
            $('#gdvOrders tbody td:nth-child(5)').hide();
            $('#gdvOrders tbody td:nth-child(6)').hide();
        }

        // Fill the items table.
        function displayItems(data) {
            clearItems();
            for (var key in data) {
                var altRow;
                if (key % 2 == 0)
                    altRow = 'class="alt-row"';
                else
                    altRow = '';

                var itemStatus;
                switch (data[key].Status) {
                    case 0:
                        itemStatus = 'Pending';
                        break;
                    case 1:
                        itemStatus = 'Approved';
                        break;
                    case 2:
                        itemStatus = 'Denied';
                        break;
                }
                $('#gdvItems tbody').append($('<tr ' + altRow + '><td>' + data[key].ItemId + '</td>' +
                                                    '<td>' + data[key].OrderId + '</td>' +
                                                    '<td>' + data[key].Name + '</td>' +
                                                    '<td>' + data[key].Description + '</td>' +
                                                    '<td>$' + data[key].Price.toFixed(2) + '</td>' +
                                                    '<td>' + data[key].Quantity + '</td>' +
                                                    '<td>' + data[key].Justification + '</td>' +
                                                    '<td>' + data[key].Source + '</td>' +
                                                    '<td>$' + data[key].SubTotal.toFixed(2) + '</td>' +
                                                    '<td>$' + data[key].TaxTotal.toFixed(2) + '</td>' +
                                                    '<td>$' + data[key].GrandTotal.toFixed(2) + '</td>' +
                                                    '<td>' + itemStatus + '</td>' +
                                                    '<td>' + data[key].Reason + '</td></tr>'));
            }
            $('#gdvItems tbody td:nth-child(1)').hide();
            $('#gdvItems tbody td:nth-child(2)').hide();
        }

        // Process the selected item with Approved or Denied status and if
        // it is the last item in the list, prompt for order closure.
        function Process(itemId, price, quantity, source, reason, status) {
            var dto = {};
            dto.itemId = parseInt(itemId);
            dto.price = parseFloat(price);
            dto.quantity = parseInt(quantity);
            dto.source = source;
            dto.status = parseInt(status);
            dto.reason = reason
            $.ajax({
                type: 'POST',
                url: 'WebService.aspx/ProcessItem',
                data: JSON.stringify(dto),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    // If it returns false, then there are no more items pending so we 
                    // prompt for closure.
                    if (data.d == false) {
                        if (confirm('Would you like to close this order?')) {
                            CloseOrder(orderId);
                        }
                    }
                    else if (data.d != true)
                        alert(data.d);
                },
                error: function (xhr, status, errorThrown) {
                    var error = JSON.parse(xhr.responseText);
                    alert(error.Message);
                }
            });
        }

        // Add methods to controls on page after the page has loaded
        $(function () {
            // Add slim scroll bar.
            $('#gdvOrders-scroll').slimScroll({
                height: '250px'
            });

            $('#gdvItems-scroll').slimScroll({
                height: '125px'
            });

            $('#gdvOrders th:nth-child(4)').hide();
            $('#gdvOrders th:nth-child(5)').hide();

            $('#dtpFrom, #dtpTo').datepicker();

            $('#txtPrice,#txtQuantity,#txtSource,#txtReason').dblclick(function () {
                $(this).removeAttr('readonly');
            });

            $('#txtPrice,#txtQuantity,#txtSource,#txtReason').blur(function () {
                $(this).attr('readonly', 'readonly');
            });

            $('#btnCloseOrder').click(function () {
                if (orderId == 0)
                    alert('Please retrieve and select an order to close.');
                else
                    CloseOrder(orderId);
                return false;
            })

            // Get orders depending on search criteria.
            $('#btnGo').click(function () {
                if ($('#rdoDepartment').is(':checked')) {
                    if ($('#chkDateRange').is(':checked'))
                        getOrdersByDept($('#ddlFilter option:selected').val(),
                                        $('#dtpFrom').val(),
                                        $('#dtpTo').val());
                    else
                        getOrdersByDept($('#ddlFilter option:selected').val(), '', '');
                }
                else if ($('#rdoEmployee').is(':checked')) {
                    if ($('#chkDateRange').is(':checked'))
                        getOrdersByEmployee($('#ddlFilter option:selected').val(),
                                            $('#txtFirstName').val(),
                                            $('#txtLastName').val(),
                                            $('#dtpFrom').val(),
                                            $('#dtpTo').val());
                    else
                        getOrdersByEmployee($('#ddlFilter option:selected').val(),
                                            $('#txtFirstName').val(),
                                            $('#txtLastName').val(), null, null);
                }
                else if ($('#rdoOrderId').is(':checked')) {
                    getOrderById($('#txtOrderId').val());
                }
                return false;
            });

            // Enable employee name textboxes.
            $('input[name=searchType]').click(function () {
                if ($(this).val() == 'employee') {
                    $('#txtFirstName,#txtLastName').removeAttr('disabled');
                }
                else
                    $('#txtFirstName,#txtLastName').attr('disabled', 'disabled').val('');
                if ($(this).val() == 'orderId')
                    $('#txtOrderId').removeAttr('disabled');
                else
                    $('#txtOrderId').attr('disabled', 'disabled').val('');
            });

            // When a row is selected in the orders table, populate the form with the values
            // and set the row's color to a highlighted color
            $('#gdvOrders tbody tr').live('click', function () {
                displayItems(JSON.parse($(this).find('td:nth-child(4)').text()));

                orderId = parseInt($(this).find('td:nth-child(1)').text());
                $('#txtSubTotal').val($(this).find('td:nth-child(5)').text());
                $('#txtTaxTotal').val($(this).find('td:nth-child(6)').text());
                $('#txtGrandTotal').val($(this).find('td:nth-child(7)').text());

                $(this).css('background-color', '#3399ff');
                $('#gdvOrders tbody tr').each(function () {
                    if ($(this).attr('data-selected') == 'true' && $(this).css('background-color') != '#3399ff') {
                        $(this).removeAttr('data-selected');
                        $(this).css('background-color', lastOrderRowColor);
                    }
                });
                $(this).attr('data-selected', 'true');
            });

            // Row hover highlight effects for Orders table 
            $('#gdvOrders tbody tr').live(
            'mouseenter',
            function () {
                if ($(this).css('background-color') != '#3399ff') {
                    lastOrderRowColor = $(this).css('background-color');
                    $(this).css('background-color', '#3399ff');
                }
            }).live(
            'mouseleave',
            function () {
                if ($(this).attr('data-selected') != 'true')
                    $(this).css('background-color', lastOrderRowColor);
            });

            // Row hover highlight effects for Items table 
            $('#gdvItems tbody tr').live(
            'mouseenter',
            function () {
                if ($(this).css('background-color') != '#3399ff') {
                    lastOrderRowColor = $(this).css('background-color');
                    $(this).css('background-color', '#3399ff');
                }
            }).live(
            'mouseleave',
            function () {
                if ($(this).attr('data-selected') != 'true')
                    $(this).css('background-color', lastOrderRowColor);
            });

            // When a row is selected in the items table, populate the form with the values 
            // and set the row's color to a highlighted color 
            $('#gdvItems tbody tr').live('click', function () {

                // Set the form values to that of the item in the selected row 
                itemId = parseInt($(this).find('td:nth-child(1)').text());
                $('#txtName').val($(this).find('td:nth-child(3)').text());
                $('#txtDescription').val($(this).find('td:nth-child(4)').text());
                var price = $(this).find('td:nth-child(5)').text();
                price = price.substr(1, price.length - 1);
                $('#txtPrice').val(price);
                $('#txtQuantity').val($(this).find('td:nth-child(6)').text());
                $('#txtJustification').val($(this).find('td:nth-child(7)').text());
                $('#txtSource').val($(this).find('td:nth-child(8)').text());
                $('#txtReason').val($(this).find('td:nth-child(13)').text());

                // Set the selected row to a highlited color. 
                $(this).css('background-color', '#3399ff');
                $('#gdvItems tbody tr').each(function () {
                    if ($(this).attr('data-selected') == 'true' && $(this).css('background-color') != '#3399ff') {
                        $(this).removeAttr('data-selected');
                        $(this).css('background-color', lastOrderRowColor);
                    }
                });
                $(this).attr('data-selected', 'true');
            });

            // Approve and Deny Button Commands 
            $('#btnApprove').click(function () {
                var price = $('#txtPrice').val();
                var quantity = $('#txtQuantity').val();
                var source = $('#txtSource').val();
                var reason = $('#txtReason').val();
                if (itemId != '')
                    Process(itemId, price, quantity, source, reason, 1);
                else
                    alert('Please select an item.');
                return false;
            });

            $('#btnDeny').click(function () {
                var price = $('#txtPrice').val();
                var quantity = $('#txtQuantity').val();
                var source = $('#txtSource').val();
                var reason = $('#txtReason').val();
                if (itemId != '')
                    Process(itemId, price, quantity, source, reason, 2);
                else
                    alert('Please select an item.');
                return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="form-header">
        <h2>Process Purchase Order</h2>
        <fieldset id="search-type">
            <legend>Search Type</legend>
            <input id="rdoDepartment" type="radio" name="searchType" value="department" checked="checked" />
            <label for="rdoDepartment">Department</label>
            <br />
            <input id="rdoEmployee" type="radio" name="searchType" value="employee" />
            <label for="rdoEmployee">Employee</label>
            <br />
            <input id="rdoOrderId" type="radio" name="searchType" value="orderId" />            
            <label for="rdoOrderId">PO Number</label>
        </fieldset>
        <div id="orderId-search">
            <div class="row">
                <label for="txtOrderId">Purchase Order Id:</label>
            </div>
            <div class="row">
                <input id="txtOrderId" type="text" disabled="disabled" />
            </div>
        </div>
        <div id="name-search">
            <div class="row">
                <label for="txtFirstName">First Name: </label>
                <input id="txtFirstName" type="text" disabled="disabled" />
            </div>
            <div class="row">
                <label for="txtLastName">Last Name: </label>
                <input id="txtLastName" type="text" disabled="disabled" />
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
                    <label for="chkDates"">Within Date Range</label>
                </div>
            </div>
        </div>
        <input id="btnGo" type="submit" value="GO" />
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
            <br /><br />
            <input id="btnCloseOrder" type="button" value="CLOSE ORDER" />
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
            <div class="row">
                <label for="txtReason">Reason: </label>
                <input id="txtReason" type="text" data-property="Reason" readonly="readonly" />
            </div>  
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
        <div id="process-buttons">
            <input id="btnApprove" type="button" value="APPROVE" />
            <br /><br />
            <input id="btnDeny" type="button" value="DENY" />
        </div>
        <asp:HiddenField ID="txtEmpDepartment" runat="server" />
    </div>
</asp:Content>
