
/*
    -------------------------------------------------------------------------------------------------------------------------------
    Function that performs client-side validation of thr selectd inputs
    -------------------------------------------------------------------------------------------------------------------------------
*/
    function validateInput() {
        var isValid = true;
        $("div.form-group > input").css({ "border-color": "#d2d6de" });
        $("div.form-group > select").css({ "border-color": "#d2d6de" });

        if ($('#stock').val() == "0" || $('#stock').val() == "") {
            $('#stock').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must select a Stock name");
            isValid = false;
        } else if ($('#date_of_withdraw').val() == "0" || $('#date_of_withdraw').val() == "") {
            $('#date_of_withdraw').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must select a valid Date of withdraw");
            isValid = false;
        } else if ($('#quantity').val() == "0" || $('#quantity').val() == "") {
            $('#quantity').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must enter a valid quantity");
            isValid = false;
        } else if ($('#category_Id').val() == "" || $('#category_Id').val() == "0" ) {
            $('#category_Id').css({ "border": "2px solid Red" });
            ToggleNotification("Required Field!", "You must enter a valid valid category name");
            isValid = false;
        } else if ($('#stock_type').val() == "" || $('#stock_type').val() == "0") {
            $('#stock_type').css({ "border-color": "Red" });
            ToggleNotification("Required Field!", "You must select a valid stock to get valid stock type");
            isValid = false;
        } else if ($('#units').val() == "" || $('#units').val() == "0") {
            $('#units').css({ "border-color": "Red" });
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
    function BindTable(withdraw_ID, stock_code, quantity, unit_of_withdraw, withdrawer, compartment_ID, date_of_withdraw, stock_type, expiry_date, category_ID, comment) {
        $("#tableBasket > tbody").append("<tr><td style = 'display: none'>" + withdraw_ID + "</td><td>" +
            stock_code +
            "</td><td>" + quantity + "</td><td>" + unit_of_withdraw + "</td><td style = 'display: none'>" + withdrawer + "</td><td>" + compartment_ID +
            "</td><td><a class='btn text-danger'><i class='fa fa-trash-o'>" +
            "</i></a></td><td style = 'display: none'>" + date_of_withdraw + "</td><td style = 'display: none'>" +
            stock_type + "</td><td style = 'display: none'>" + expiry_date + "</td><td style = 'display: none'>" +
            category_ID + "</td><td style = 'display: none'>" + comment + "</td></tr>");
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
    $("#addButton").click(function () {
        var withdraw_ID = $("#withdrawId").val();
        var stock_code = $("#stock").val();
        var quantity = $("#quantity").val();
        var unit_of_withdraw = $("#units").val();
        var withdrawer = $("#withdrawer").val();
        var compartment_ID = $("#compartment_Id").val();
        var date_of_withdraw = $("#date_of_withdraw").val()
        var stock_type = $("#stock_type").val();
        var expiry_date = $("#expiry_date").val();
        var category_ID = $("#stock_category").val();
        var comment = $("#comment").val();
        //var del = "<i class='fa fa-trash-o'></i>";
        if (validateInput() == true) {
            BindTable(withdraw_ID, stock_code, quantity, unit_of_withdraw, withdrawer, compartment_ID, date_of_withdraw, stock_type, expiry_date, category_ID, comment);
            $("#table").show(function () {
                $("#quantity").val("");
                $("#comment").val("");
                $("#expiry_date").val("");
            });
            $("#btnSubmit").scrollView();
        }
    });
/*
  ---------------------------------------------------------------------------------------------------------------------------------------------------
  ---------------------------------------------------------------------------------------------------------------------------------------------------
*/