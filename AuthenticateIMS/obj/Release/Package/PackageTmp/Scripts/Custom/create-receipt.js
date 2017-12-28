  
/*
    -------------------------------------------------------------------------------------------------------------------------------
    Function that performs client-side validation of the selected inputs
    -------------------------------------------------------------------------------------------------------------------------------
*/
    function validateReceipt() {
        var isValid = true;
        $("div.form-group > input").css({ "border-color": "#d2d6de" });
        $("div.form-group > select").css({ "border-color": "#d2d6de" });

        if ($('#stock_name').val() == "0" || $('#stock_name').val() == "") {
            $('#stock_name').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must select a Stock name");
            isValid = false;
        } else if ($('#date_of_receipt').val() == "0" || $('#date_of_receipt').val() == "") {
            $('#date_of_receipt').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must select a valid Date of withdraw");
            isValid = false;
        } else if ($('#stock_quantity').val() == "0" || $('#stock_quantity').val() == "") {
            $('#stock_quantity').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must enter a valid quantity");
            isValid = false;
        } else if ($('#category_Id').val() == "" || $('#category_Id').val() == "0" ) {
            $('#category_Id').css({ "border": "2px solid Red" });
            ToggleNotification("Required Field!", "You must enter a valid valid category name");
            isValid = false;
        } else if ($('#type').val() == "" || $('#type').val() == "0") {
            $('#type').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must select a valid stock to get valid stock type");
            isValid = false;
        } else if ($('#stock_units').val() == "" || $('#stock_units').val() == "0") {
            $('#stock_units').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must enter valid units");
            isValid = false;
        }
        return isValid;
    }
/*
  ---------------------------------------------------------------------------------------------------------------------------------------------------
  ---------------------------------------------------------------------------------------------------------------------------------------------------
*/

/*
    -------------------------------------------------------------------------------------------------------------------------------
    Function that binds contents to a dynamic table that acts as a basket
    -------------------------------------------------------------------------------------------------------------------------------
*/
    function BindTableBuffer(receipt_ID, stock_name, stock_quantity, units, acceptor, compartment, date_of_receipt, type, date_of_expiry, category, receipt_comment) {
        $("#tableBuffer > tbody").append("<tr><td style = 'display: none'>" + receipt_ID + "</td><td>" +
            stock_name +
            "</td><td>" + stock_quantity + "</td><td>" + units + "</td><td style = 'display: none'>" + acceptor + "</td><td>" + compartment +
            "</td><td><a class='btn text-danger'><i class='fa fa-trash-o'>" +
            "</i></a></td><td style = 'display: none'>" + date_of_receipt + "</td><td style = 'display: none'>" +
            type + "</td><td style = 'display: none'>" + date_of_expiry + "</td><td style = 'display: none'>" +
            category + "</td><td style = 'display: none'>" + receipt_comment + "</td></tr>");
    }
/*
  ---------------------------------------------------------------------------------------------------------------------------------------------------
  ---------------------------------------------------------------------------------------------------------------------------------------------------
*/

/*
    -------------------------------------------------------------------------------------------------------------------------------
    Function that causes the page to scroll down after a successful addition of a receipt to the basket
    -------------------------------------------------------------------------------------------------------------------------------
*/
    $.fn.scrollView = function () {
        return this.each(function () {
            $('html, body').animate({
                scrollTop: $(this).offset().top
            }, 600);
        });
    }
/*
  ---------------------------------------------------------------------------------------------------------------------------------------------------
  ---------------------------------------------------------------------------------------------------------------------------------------------------
*/

/*
    -------------------------------------------------------------------------------------------------------------------------------
    Function that does actual data binding to the basket upon clicking the add button
    -------------------------------------------------------------------------------------------------------------------------------
*/
    $("#addReceipt").click(function () {
        var receipt_ID = $("#receiptId").val();
        var stock_name = $("#stock_name").val();
        var stock_quantity = $("#stock_quantity").val();
        var units = $("#stock_units").val();
        var acceptor = $("#acceptor").val();
        var compartment = $("#compartment").val();
        var date_of_receipt = $("#date_of_receipt").val()
        var type = $("#type").val();
        var date_of_expiry = $("#date_of_expiry").val();
        var category = $("#category").val();
        var receipt_comment = $("#receipt_comment").val();
        //var del = "<i class='fa fa-trash-o'></i>";
        if (validateReceipt() == true) {
            BindTableBuffer(receipt_ID, stock_name, stock_quantity, units, acceptor, compartment, date_of_receipt, type, date_of_expiry, category, receipt_comment);
            $("#tableShow").show(function () {
                $("#stock_quantity").val("");
                $("#receipt_comment").val("");
                $("#date_of_expiry").val("");
            });
            $("#submitReceipt").scrollView();
        }
    });
/*
  ---------------------------------------------------------------------------------------------------------------------------------------------------
  ---------------------------------------------------------------------------------------------------------------------------------------------------
*/
