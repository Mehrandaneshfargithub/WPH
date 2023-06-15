$('#loginUserFunc').unbind('click');




$('#loginUserFunc').on("click", function () {
    $(this).attr("disabled", true);

    var data = $("#loginForm").serialize();
    $(".loader").removeClass("hidden");

    $.ajax({
        type: "Post",
        url: "/UserHandler/LoginUser",
        data: data,
        success: function (response) {
            $('#loginUserFunc').removeAttr("disabled");

            if (response === 'EnterUserCode') {
                $("#UserCodeContainer").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (!response.result) {
                $('#loginError').removeClass("hidden");
                $("#UserCodeContainer").addClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
                $('#ExampleCaptcha_ReloadIcon').trigger("click");
            }
            else if (response.result) {

                if (response.clinicAdmin === "Normal") {
                    
                    window.location.href = "/ApplicationHandler/Index?clinicSectionId=" + response.clinicSectionGuid[0];
                    $(".loader").fadeIn("slow");
                    
                }
                else {
                    window.location.href = "/ApplicationHandler/ClinicAdmin";
                    $(".loader").fadeIn("slow");
                }

            }
        }
    })
});




function AjaxFunctions() {


    $.ajax({
        type: "Post",
        url: "/AnalysisResult/Form"

    });


    $.ajax({
        type: "Post",
        url: "/AnalysisResultMaster/Form"

    });



    //$.ajax({
    //    type: "Post",
    //    url: "/Disease/Form"

    //});





    //$.ajax({
    //    type: "Post",
    //    url: "/Medicine/Form"

    //});




    $.ajax({
        type: "Post",
        url: "/PatientReception/Form"

    });

    $.ajax({
        type: "Post",
        url: "/PatientReceptionAnalysis/Form"

    });





    //$.ajax({
    //    type: "Post",
    //    url: "/Reserves/Form"

    //});

    //$.ajax({
    //    type: "Post",
    //    url: "/Symptom/Form"

    //});

    //$.ajax({
    //    type: "Post",
    //    url: "/TotalReserves/Form"

    //});

    $.ajax({
        type: "Post",
        url: "/UserManagment/Form"

    });

    //$.ajax({
    //    type: "Post",
    //    url: "/Visit/Form"

    //});

    $.ajax({
        type: "Post",
        url: "/GroupAnalysis/Form"

    });

    $.ajax({
        type: "Post",
        url: "/Analysis/Form"

    });

    $.ajax({
        type: "Post",
        url: "/AnalysisItem/Form"

    });

    $.ajax({
        type: "Post",
        url: "/Doctor/Form"

    });

    $.ajax({
        type: "Post",
        url: "/MoneyConvert/Form"

    });

    $.ajax({
        type: "Post",
        url: "/Patient/Form"

    });

    $.ajax({
        type: "Post",
        url: "/BaseInfo/Form"

    });

    $.ajax({
        type: "Post",
        url: "/Cost/Form"

    });

    $.ajax({
        type: "Post",
        url: "/CostReport/Form"

    });

    $.ajax({
        type: "Post",
        url: "/PatientReceptionReceived/Form"

    });

    $.ajax({
        type: "Post",
        url: "/ClinicSectionSetting/Form"

    });
}


$('.chooseLoginSkin').on("click", function (e) {
    var theme = $(this).attr('id');
    $.ajax({
        type: "Post",
        url: "/UserHandler/chooseLoginTheme",
        data: '{ theme:"' + theme + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    });
});

$(document).ready(function () {
    $("#UserName").focus();
});

$('#Code').on('keypress', function (e) {
    if (e.which === 13) {
        $("#UserName").focus();
    }
});

$('#UserName').on('keypress', function (e) {
    if (e.which === 13) {
        $("#Pass3").focus();
    }
});

$('#Pass3').on('keypress', function (e) {
    if (e.which === 13) {
        $('#loginUserFunc').trigger("click");
    }
});

$('#CaptchaCode').on('keypress', function (e) {
    if (e.which === 13) {
        $('#loginUserFunc').trigger("click");
    }
});

