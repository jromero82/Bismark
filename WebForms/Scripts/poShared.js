// Get order by order Id.
function getOrderById(orderId) {
    clearPageData();
    var dto = {};
    dto.orderId = parseInt(orderId);
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
        $('#gdvOrders tbody').append($('<tr ' + altRow + ' data-row="' + key + '"><td>' + data[key].OrderId + '</td>' +
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
        $('#gdvItems tbody').append($('<tr ' + altRow + ' data-row="' + key + '"><td>' + data[key].ItemId + '</td>' +
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

    // When a row is selected in the orders table, populate the form with the values
    // and set the row's color to a highlighted color
    $('#gdvOrders tbody tr').live('click', function () {
        displayItems(JSON.parse($(this).find('td:nth-child(4)').text()));
        ordersRow = $(this).attr('data-row');
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

        itemsRow = $(this).attr('data-row');

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
});

