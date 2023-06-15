$('.btn-select-row').on("click", function (e) {
    e.preventDefault();
    var selected = $(this).attr("id");
    var med_id = selected.split("medicine_").join("");
    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: "/ApplicationHandler/RefreshMedicineFunctionsInAjaxCall",
        data: '{ medId: ' + med_id + ' }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('.label-success').removeClass("label-success");
            $("#medicine_" + med_id).addClass("label-success");
            $('#medicinenumber').text(response["MedicineNumber"]);
            $('#latest-sales-price-dinar').text(response["LatestSalesPriceDinar"]);
            $('#latest-sales-price-dolar').text(response["LatestSalesPriceDolar"]);
            $('#consideration').text(response["Consideration"]);
            $('#discount').text(response["Discount"]);
            $('#Latest-purchase-price').text(response["LatestPurchasePriceDolar"]);
            $('#profit').text(response["Profit"]);
            $('#suppliers').text(response["Suppliers"]);
            $('#expired-date').text(response["ExpiredDate"]);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });
});
function initiateSelectMedicine() {
    $('.btn-select-row').on("click", function (e) {
        e.preventDefault();
        var selected = $(this).attr("id");
        var med_id = selected.split("medicine_").join("");
        $(".loader").removeClass("hidden");
        $.ajax({
            type: "Post",
            url: "/ApplicationHandler/RefreshMedicineFunctionsInAjaxCall",
            data: '{ medId: ' + med_id + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                $('.label-success').removeClass("label-success");
                $("#medicine_" + med_id).addClass("label-success");
                $('#medicinenumber').text(response["MedicineNumber"]);
                $('#latest-sales-price-dinar').text(response["LatestSalesPriceDinar"]);
                $('#latest-sales-price-dolar').text(response["LatestSalesPriceDolar"]);
                $('#consideration').text(response["Consideration"]);
                $('#discount').text(response["Discount"]);
                $('#Latest-purchase-price').text(response["LatestPurchasePriceDolar"]);
                $('#profit').text(response["Profit"]);
                $('#suppliers').text(response["Suppliers"]);
                $('#expired-date').text(response["ExpiredDate"]);
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }

        });
    });
}



function refreshFunctions() {
    $('#medicinenumber').text(MedicineNumber);
    $('#latest-sales-price-dinar').text(LatestSalesPriceDinar);
    $('#latest-sales-price-dolar').text(LatestSalesPriceDolar);
    $('#consideration').text(Consideration);
    $('#discount').text(Discount);
    $('#Latest-purchase-price').text(LatestPurchasePrice);
    $('#profit').text(Profit);
    $('#suppliers').text(Suppliers);
    $('#expired-date').text(ExpiredDate);
}

///////////////////////////////////
//////////////////////////////////
$('#barcode_search').keypress(function (e) {
    if (e.which == 13) {
        var barcode = $(this).val();
        $('#barcode_search').val('');
        $(".loader").removeClass("hidden");
        $.ajax({
            type: "Post",
            url: "/ApplicationHandler/medicineDBTable",
            data: { barcode: barcode },
            success: function (response) {
                $('#table-holder').remove();
                $('#table-page').remove();
                $('#table-section').html(response);
                initiateSelectMedicine();
                refreshFunctions();
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        });
    } else {
        return true;
    }
    return false;
});

///////////////////////////////////
//////////////////////////////////
$('#searchBy').change(function () {
    var value = $(this).val();
    if (value == 1 || value == 2 || value == 5) {
        $('#search_by_type_select').addClass("hidden");
        $('#search_by_type_text').removeClass("hidden");
    }
    if (value == 3 || value == 4) {
        $('#search_by_type_text').addClass("hidden");
        $('#search_by_type_select').removeClass("hidden");
        if (value == 3) {
            $.ajax({
                type: "Post",
                url: "/ApplicationHandler/getListOfProducers",

                success: function (response) {
                    var search_by_type_select = $('#search_by_type_select');
                    search_by_type_select.empty().append('<option selected="selected" value="0">-------</option>');
                    var i = 0;
                    $.each(response, function () {
                        search_by_type_select.append("<option value=" + response["" + i + ""]["producerId"] + ">" + response["" + i + ""]["producerName"] + "</option>");
                        ++i;
                    });
                }
            });
        }

        if (value == 4) {
            $.ajax({
                type: "Post",
                url: "/ApplicationHandler/getListOfForms",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var search_by_type_select = $('#search_by_type_select');
                    search_by_type_select.empty().append('<option selected="selected" value="0">-------</option>');
                    var i = 0;
                    $.each(response, function () {
                        search_by_type_select.append("<option value=" + response["" + i + ""]["formId"] + ">" + response["" + i + ""]["formName"] + "</option>");
                        ++i;
                    });
                }
            });
        }
    }
});

/////////////////////////////////////////////
////////////////////////////////////////////

$('#search_by_type_text').keyup(function () {
    var search = $(this).val();
    var contextSensitive = $('#contextsensitive').is(':checked');
    var like = $('#like').is(':checked');
    var searchType = $('#searchBy').val();
    var search_by_type_select;

    if (contextSensitive) {

        if (searchType == 1 || searchType == 2 || searchType == 5) {
            $(".loader").removeClass("hidden");
            $.ajax({
                type: "Post",
                url: "/ApplicationHandler/medicineDBTable",
                data: { search: search, searchType: searchType, like: like },
                async: false,
                success: function (response) {
                    $('#table-holder').remove();
                    $('#table-page').remove();
                    $('#table-section').html(response);
                    initiateSelectMedicine();
                    refreshFunctions();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                }
            });
        }
    }
});

//////////////////////////////////////
/////////////////////////////////////

$('#search_by_type_select').change(function () {
    var searchByTypeSelect = $(this).val();
    var searchType = $('#searchBy').val();
    if (searchByTypeSelect != 0) {
        $(".loader").removeClass("hidden");
        $.ajax({
            type: "Post",
            url: "/ApplicationHandler/medicineDBTable",
            data: { searchType: searchType, searchByTypeSelect: searchByTypeSelect },
            success: function (response) {
                $('#table-holder').remove();
                $('#table-page').remove();
                $('#table-section').html(response);
                initiateSelectMedicine();
                refreshFunctions();
                refreshFunctions();
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        });
    }
});

/////////////////////////////////////////////////
////////////////////////////////////////////////

$('#search_by_button').on("click", function (e) {
    e.preventDefault();
    var search = $('#search_by_type_text').val();
    var like = $('#like').is(':checked');
    var searchType = $('#searchBy').val();
    if (searchType == 1 || searchType == 2 || searchType == 5) {
        $(".loader").removeClass("hidden");
        $.ajax({
            type: "Post",
            url: "/ApplicationHandler/medicineDBTable",
            data: { search: search, searchType: searchType, like: like },
            success: function (response) {
                $('#table-holder').remove();
                $('#table-page').remove();
                $('#table-section').html(response);
                initiateSelectMedicine();
                refreshFunctions();
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        });
    }
});

//////////////////////////////////////////////////
/////////////////////////////////////////////////

$('#search_by_clear').on("click", function (e) {
    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: "/ApplicationHandler/medicineDBTable",
        data: { searchType: 0 },
        success: function (response) {
            $('#table-holder').remove();
            $('#table-page').remove();
            $('#table-section').html(response);
            initiateSelectMedicine();
            $('#search_by_type_text').val('');
            $('#searchBy').val(1);
            $('#search_by_type_select').val(0);
            $('#search_by_type_select').addClass("hidden");
            $('#search_by_type_text').removeClass("hidden");
            refreshFunctions();
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });


});

////////////////////////////////////////////////////////
///////////////////////////////////////////////////////

$('#sort_button').on("click", function () {

    var search = $('#search_by_type_text').val();
    var searchType = $('#searchBy').val();
    var searchByTypeSelect = $('#search_by_type_select').val();
    var like = $('#like').is(':checked');
    var sortByType = $('#sort_by_type').val();
    var sortOrder = $('#sort_order').val();
    var dat = { search: search, searchType: searchType, searchByTypeSelect: searchByTypeSelect, like: like, sortByType: sortByType, sortOrder: sortOrder };
    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: "/ApplicationHandler/medicineDBTable",
        data: dat,
        success: function (response) {
            $('#table-holder').remove();
            $('#table-page').remove();
            $('#table-section').html(response);
            initiateSelectMedicine();
            refreshFunctions();
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });

});