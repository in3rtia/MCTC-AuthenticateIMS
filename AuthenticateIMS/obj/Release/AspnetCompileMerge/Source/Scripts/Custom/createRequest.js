/*
-----------------------------------------------------------------------------------------------
These scripts gets a stock code and compartment for a selected stock from the drop down list
-----------------------------------------------------------------------------------------------
*/
$("#stock").change(function (event) {
    var stock_code = $(this).val();
    $.ajax({
        url: "@Url.Action("getCompartment", "RequestDetails")",
        data: { id: stock_code },
        type: "Get",
        dataType: "html",
        success: function (data) {
            //Whatever result you have got from your controller with html partial view replace with a specific html.
            $("#compartment").val(data); // HTML DOM replace
        }
    });
});

$("#stock").change(function (event) {
    var stock_code = $(this).val();
    $.ajax({
        url: "@Url.Action("getUnit", "RequestDetails")",
        data: { id: stock_code },
        type: "Get",
        dataType: "html",
        success: function (data) {
            //Whatever result you have got from your controller with html partial view replace with a specific html.
            $("#units").val(data); // HTML DOM replace
        }
    });
});

/*
-----------------------------------------------------------------------------------------------
This function shows a table when clicked
-----------------------------------------------------------------------------------------------
*/
function showTable() {
    $("#table").show();
}

/*
-----------------------------------------------------------------------------------------------
This function is responsible for the client side validation
-----------------------------------------------------------------------------------------------
*/
function validateInput() {
    var isValid = true;
    $("div.form-group > input").css({ "border-color": "#d2d6de" });
    $("div.form-group > select").css({ "border-color": "#d2d6de" });

    if ($('#stock').val() == "0" || $('#stock').val() == "") {
        $('#stock').css({ "border-color": "Red" });
        ToggleNotification("Required Field!", "You must select a Stock name");
        isValid = false;
    } else if ($('#dateOfRequest').val() == "0" || $('#dateOfRequest').val() == "") {
        $('#dateOfRequest').css({ "border-color": "Red" });
        ToggleNotification("Required Field!", "You must select a valid Date of request");
        isValid = false;
    } else if ($('#quantity').val() == "0" || $('#quantity').val() == "") {
        $('#quantity').css({ "border-color": "Red" });
        ToggleNotification("Required Field!", "You must enter a valid quantity");
        isValid = false;
    } else if ($('#dateOfReturn').val() == "mm/dd/yyyy") {
        $('#dateOfReturn').css({ "border": "2px solid Red" });
        ToggleNotification("Required Field!", "You must select a valid Expected return date");
        isValid = false;
    } else if ($('#compartment').val() == "" || $('#compartment').val() == "0") {
        $('#compartment').css({ "border-color": "Red" });
        ToggleNotification("Required Field!", "You must select a valid stock to get valid compartment");
        isValid = false;
    } else if ($('#purpose').val() == "" || $('#purpose').val() == "0") {
        $('#purpose').css({ "border-color": "Red" });
        ToggleNotification("Required Field!", "You must enter a valid Reason for request");
        isValid = false;
    }
    return isValid;
}

/*
-----------------------------------------------------------------------------------------------
This function binds contents to the table that acts as the basket for the creation of requests
-----------------------------------------------------------------------------------------------
*/
function BindTable(itemName, quantity, purpose, requestDate, requestId, compartmentId, requester, unit, approvalStatus, approver) {
    $("#tableBasket > tbody").append("<tr><td>" +
        itemName +
        "</td><td>" + quantity + "</td><td>" + purpose + "</td><td>" + requestDate +
        "</td><td><a class='btn text-danger'><i class='fa fa-trash-o'>" +
        "</i></a></td><td style = 'display: none'>" + requestId + "</td><td style = 'display: none'>" +
        compartmentId + "</td><td style = 'display: none'>" + requester + "</td><td style = 'display: none'>" +
        unit + "</td><td style = 'display: none'>" + approvalStatus + "</td><td style = 'display: none'>" +
        approver + "</td></tr>");
}

/*
-----------------------------------------------------------------------------------------------
This function scrolls the page to a specific point after adding o f a request
-----------------------------------------------------------------------------------------------
*/
$.fn.scrollView = function () {
    return this.each(function () {
        $('html, body').animate({
            scrollTop: $(this).offset().top
        }, 600);
    });
}

/*
-------------------------------------------------------------------------------------------------------------
This function creates the content for the entries of the table that acts as the basket when creating requests 
-------------------------------------------------------------------------------------------------------------
*/
$("#addButton").click(function () {
    var itemName = $("#stock").val();
    var quantity = $("#quantity").val();
    var purpose = $("#purpose").val();
    var requestDate = $("#dateOfRequest").val();
    var requestId = $("#requestId").val();
    var compartmentId = $("#compartment").val()
    var requester = $("#requester").val();
    var unit = $("#units").val();
    var approvalStatus = "0";
    var approver = "";
    //var del = "<i class='fa fa-trash-o'></i>";
    if (validateInput() == true) {
        BindTable(itemName, quantity, purpose, requestDate, requestId, compartmentId, requester, unit, approvalStatus, approver);
        $("#table").show(function () {
            $("#quantity").val("");
            $("#purpose").val("");
            $("#dateOfReturn").val("");
        });
        $("#btnSubmit").scrollView();
    }
});

/*
-----------------------------------------------------------------------------------------------
This function makes an ajax call to create a request
-----------------------------------------------------------------------------------------------
*/
function saveRequest() {
    var table = document.getElementById('tableBasket'),
        rows = table.getElementsByTagName('tr'),
        i, j, cells, itemName, quantity, purpose, requestDate, requestId, compartmentId, requester, unit, approvalStatus, approver;

    for (i = 0, j = rows.length; i < j; ++i) {
        cells = rows[i].getElementsByTagName('td');
        if (!cells.length) {
            continue;
        }
        //passValues();
        itemName = cells[0].innerHTML;
        quantity = cells[1].innerHTML;
        purpose = cells[2].innerHTML;
        requestDate = cells[3].innerHTML;
        requestId = cells[5].innerHTML;
        compartmentId = cells[6].innerHTML;
        requester = cells[7].innerHTML;
        unit = cells[8].innerHTML;
        approvalStatus = cells[9].innerHTML;
        approver = cells[10].innerHTML;

        $.ajax({
            type: "POST",
            url: "@Url.Action("createRequest", "RequestDetails")",
            data: {
                request_ID: requestId,
                stock_code: itemName,
                compartment_ID: compartmentId,
                purpose_of_item: purpose,
                mine_number: requester,
                date_of_request: requestDate,
                quant: quantity,
                unit_of_issue: unit,
                status: approvalStatus,
                approver: approver
            },
            success: function () {
                alert("Success!!!");
            },
            error: function () {
                alert("Something went Wrong.");
            }
        });

    }
}
