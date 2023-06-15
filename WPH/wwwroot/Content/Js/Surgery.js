

var Analysis;
var AnalysisItems;
var groupAnalysis;
var decimalAmount;
var selectedAnalysis;
var Address;
let addressAuto;
let SpeciallityDropDown;
let nameAuto = $("#Name2").data("kendoAutoComplete");;
let doctorAuto = $("#Doctor").data("kendoAutoComplete");;
var selectedItems = [];
//var AllAnalysisItems = [];
var AllItems = [];
var previousCurrencyId;
var TotalReceived;
var ReceivedAmount = [];
var dinarDecimal;
var dollarDecimal;
var euroDecimal;
var pondDecimal;
var AllMoneyConverts = [];
var TotalTranslated;
var DiscountTranslated;
var PriceTranslated;
var CodeTranslated;
var TypeTranslated;
var NameTranslated;
var AmountTranslated;
var DateTranslated;
var baseCurrencyId;
var gridHieght = 180;
var printed = false;


/////////////////KeyPress And Focuses

$('#MotherName').on('keypress', function (e) {

    if (e.which === 13) {
        $("#PatientAttendanceName").focus();
    }
});

$('#MotherName').on('focus', function (e) {
    $("#MotherName").select();
});

$('#PatientAttendanceName').on('keypress', function (e) {

    if (e.which === 13) {
        let emergency = $("#Emergency").text();

        if (emergency.toLowerCase() === "false") {
            let clinic = $("#ClinicSectionId").data("kendoDropDownList");
            clinic.focus();
            clinic.open();
        }
        else {
            let Purpose = $("#Purpose").data("kendoDropDownList");
            Purpose.focus();
            Purpose.open();
        }

    }
});

$('#PatientAttendanceName').on('focus', function (e) {
    $("#PatientAttendanceName").select();
});


$('#ClassificationId').parent().on('keypress', function (e) {

    if (e.which === 13) {
        $('#AnesthesiologistName').data("kendoAutoComplete").focus();
    }
});

$("#AnesthesiologistName").on('focus', function (e) {

    let inputAuto = $("#AnesthesiologistName").data("kendoAutoComplete");
    let value = inputAuto.value();
    inputAuto.search(value);
});

$("#SurgeryOneName").on('focus', function (e) {

    let inputAuto = $("#SurgeryOneName").data("kendoAutoComplete");
    let value = inputAuto.value();
    inputAuto.search(value);
});

$("#SurgeryTwoName").on('focus', function (e) {

    let inputAuto = $("#SurgeryTwoName").data("kendoAutoComplete");
    let value = inputAuto.value();
    inputAuto.search(value);
});


$("#PediatricianName").on('focus', function (e) {

    let inputAuto = $("#PediatricianName").data("kendoAutoComplete");
    let value = inputAuto.value();
    inputAuto.search(value);
});

$("#PediatricianName").on('focus', function (e) {

    let inputAuto = $("#PediatricianName").data("kendoAutoComplete");
    let value = inputAuto.value();
    inputAuto.search(value);
});

$('#AnesthesiologistName').on('keypress', function (e) {

    if (e.which === 13) {
        let anesthesiologistionType = $("#AnesthesiologistionTypeId").data("kendoDropDownList");
        anesthesiologistionType.focus();
        anesthesiologistionType.open();
    }
});

$('#AnesthesiologistionTypeId').parent().on('keypress', function (e) {

    if (e.which === 13) {
        $("#SurgeryDetail").focus();
    }
});


$('#SurgeryDetail').on('keypress', function (e) {

    if (e.which === 9) {
        $("#PostOperativeTreatment").focus();
    }
});

$('#PostOperativeTreatment').on('keypress', function (e) {

    if (e.which === 9) {
        $("#SideEffects").focus();
    }
});


$('#SideEffects').on('keypress', function (e) {

    if (e.which === 9) {
        $("#AnesthesiologistionMedicine").focus();
    }
});


$('#AnesthesiologistionMedicine').on('keypress', function (e) {

    if (e.which === 9) {
        $("#Description").focus();
    }
});


$('#Description').on('keypress', function (e) {

    if (e.which === 9) {
        $("#btnOkExit").focus();
    }
});



//////////////Other Functions


function ReserveInsertAge(element) {


    var Year = parseInt($('#Year').val());
    var Month = parseInt($('#Month').val());
    if ($('#Month').val() === "" || Month === undefined) {
        Month = 0;
    }
    if ($('#Year').val() === "" || Year === undefined) {
        Year = 0;
    }

    if (Month > 11) {
        Month = 11;
        $('#Month').val('11')
    }
    var today = new Date();

    var birthDay = today;
    birthDay.setFullYear(birthDay.getFullYear() - Year);
    birthDay.setMonth(birthDay.getMonth() - Month);
    let birthMonth = birthDay.getMonth() + 1;
    if (birthMonth.toString() !== "10" && birthMonth.toString() !== "11" && birthMonth.toString() !== "12") {
        birthMonth = "0" + birthMonth.toString();
    }

    $('#DateOfBirth').val('01/' + birthMonth + '/' + birthDay.getFullYear());

}



function bodyPadding() {

    $('body').addClass('body-padding');
}

var surgery;
var Patient;

function Exit() {
    $(".page-content").load("/Surgery/Form", function (responce) {

        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    })
}


function OkAndExit() {

    $('.DuplicateDoctor').addClass('hidden');
    $('#ERROR_SomeThingWentWrong').addClass('hidden');

    let confirm = AggregateData();

    if (confirm) {
        $(".loader").removeClass("hidden");

        var token = $(':input:hidden[name*="RequestVerificationToken"]');

        let link = "/Surgery/UpdateSurgery";
        let newlink = "/Surgery/Form";


        $.ajax({
            type: "Post",
            url: link,
            data: {
                __RequestVerificationToken: token.attr('value'),
                Surgery: surgery
            },
            success: function (response) {
                if (response !== 0) {

                    var er = response.split("*");

                    if (er[0] === "ERROR_HumanResourceSallaryDependency") {

                        $('#SallaryDependency').removeClass('hidden');

                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");

                        window.scrollTo(0, document.body.scrollHeight);

                        $("#doctorType").text("(" + er[1] + ")");

                    } else if (er[0] === "DuplicateDoctor") {
                        $(`#DuplicateDoctor${er[1]}`).removeClass('hidden');
                        $(`#DuplicateDoctor${er[2]}`).removeClass('hidden');

                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");

                    } else {

                        $(".page-content").load(newlink, function (responce) {

                            $(".loader").fadeIn("slow");
                            $(".loader").addClass("hidden");
                        })
                    }


                } else {
                    $('#ERROR_SomeThingWentWrong').removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                    window.scrollTo(0, document.body.scrollHeight);
                }
            }
        });
    }

    bodyPadding();




}

function AggregateData() {


    surgery = {

        Guid: $("#Guid").val(),
        CreatedUserId: $("#CreatedUserId").val(),
        CreatedDate: $("#CreatedDate").val(),
        ReceptionId: $("#ReceptionId").val(),
        ClinicSectionId: $("#ClinicSectionId").val(),
        ClassificationId: $("#ClassificationId").val(),
        //AnesthesiologistId: $("#AnesthesiologistId").val(),
        AnesthesiologistName: $("#AnesthesiologistName").val(),
        AnesthesiologistionTypeId: $("#AnesthesiologistionTypeId").val(),
        SurgeryDetail: $("#SurgeryDetail").val(),
        PostOperativeTreatment: $("#PostOperativeTreatment").val(),
        SideEffects: $("#SideEffects").val(),
        AnesthesiologistionMedicine: $("#AnesthesiologistionMedicine").val(),
        Explanation: $("#Explanation").val(),
        //SurgeryOneId: $("#SurgeryOne").val(),
        SurgeryOneName: $("#SurgeryOneName").val(),
        //SurgeryTwoId: $("#SurgeryTwo").val(),
        SurgeryTwoName: $("#SurgeryTwoName").val(),
        PediatricianName: $("#PediatricianName").val(),
        OperationId: $("#Operation").val(),
    };


    return true;
}


function Exit() {


    $(".loader").removeClass("hidden");
    $(".page-content").load("/Surgery/Form", function (responce) {

        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    })

}

function PrintSurgery() {

    $('.DuplicateDoctor').addClass('hidden');
    $('#ERROR_SomeThingWentWrong').addClass('hidden');

    let confirm = AggregateData();

    if (confirm) {
        $(".loader").removeClass("hidden");

        var token = $(':input:hidden[name*="RequestVerificationToken"]');

        let link = "/Surgery/UpdateSurgery";


        $.ajax({
            type: "Post",
            url: link,
            data: {
                __RequestVerificationToken: token.attr('value'),
                Surgery: surgery
            },
            success: function (response) {
                if (response !== 0) {
                    var er = response.split("*");

                    if (er[0] === "ERROR_HumanResourceSallaryDependency") {

                        $('#SallaryDependency').removeClass('hidden');

                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");

                        $("#doctorType").text("(" + er[1] + ")");

                    } else if (er[0] === "DuplicateDoctor") {
                        $(`#DuplicateDoctor${er[1]}`).removeClass('hidden');
                        $(`#DuplicateDoctor${er[2]}`).removeClass('hidden');

                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");

                    } else {

                        let id = $("#Guid").val();
                        $.ajax({
                            url: "/Surgery/PrintSurgeryReport",
                            type: "Post",
                            data: { SurgeryId: id },
                            success: function (response) {
                                Exit();
                                draw2(response);
                                $(".loader").fadeIn("slow");
                                $(".loader").addClass("hidden");

                            },
                            error: function (response) {
                                console.log(response);
                                console.log(response.data);
                                console.log(response.error);
                            }
                        });

                    }

                } else {
                    $('#ERROR_SomeThingWentWrong').removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                    window.scrollTo(0, document.body.scrollHeight);
                }

            }
        });
    }

}


function draw2(imgData) {

    "use strict";

    var dataUrl = [];
    for (let index = 0; index < imgData.allb.length; index++) {

        var img = new Image();
        img.src = "data:image/jpeg;base64," + imgData.allb[index];
        dataUrl.push(img.src)


    }

    printJS({ printable: dataUrl, type: 'image' });

}
