


function UpdateMedicine() {

    let MedicineNumber = $("#NumMedicine").data("kendoNumericTextBox");


    let Id = $("#EditPreId").text();
    let medNum = MedicineNumber.value();
    let medConsume = $("#ConsumeMedicine").val();
    let medExplain = $("#ExplainMedicine").val();



    let newPresc = {
        Guid: Id,
        MedicineId: medId,
        Num: medNum,
        ConsumptionInstruction: medConsume,
        Explanation: medExplain
    }

    $.ajax({
        url: '/Visit/UpdateMedicineInPrescription',
        type: "Post",
        data: { viewModel: newPresc },
        success: function (response) {
            $("#PrescriptionDetailGrid").find(".k-pager-refresh").click();
            CancelUpdateMedicine();
        }

    });

}



function CancelUpdateMedicine() {

    let MedicineNumber = $("#NumMedicine").data("kendoNumericTextBox");
    MedicineNumber.value('');
    $('#ConsumeMedicine').val('');
    $('#ExplainMedicine').val('');
    $('#AutoMedicine').val('');
    medId = "";

    //$("#ExplainMedicine").unbind();
    //$("#ExplainMedicine").keydown(function (e) { newExplainKeypress(e) });

    $("#EditMedicineId").text('');
    let autocomplete = $("#AutoMedicine").data("kendoAutoComplete");
    autocomplete.enable(true);

    $('#AutoMedicine').focus();

    $("#updateBtn").addClass('hidden');
    $("#btnAddContent").removeClass('hidden');

}


function DeletePrescriptionDetail(element) {
    let te = $("#deleteMedicineText").text();
    bootbox.confirm(te, function (result) {

        if (!result) {

            return;

        }
        else {

            let cellList = $("#PrescriptionDetailGrid").find("td:eq(0)");
            for (let i = 0; i < cellList.length; i++) {
                if ($(cellList[i]).html() == 0) {
                    $("#PrescriptionDetailGrid").find(".k-pager-refresh").click();
                    return;
                }
            }


            $(".loader").removeClass("hidden");

            let Id = $(element).attr('data-id');

            $.ajax({
                url: '/Visit/DeleteMedicineInVisit',
                type: "Post",
                data: { Id: Id },
                success: function (response) {
                    $("#PrescriptionDetailGrid").find(".k-pager-refresh").click();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                }

            });
        }
    })

}


function EditPrescriptionDetail(element) {

    let Id = $(element).attr('data-id');
    medId = $(element).attr('data-medId');
    let medName = $(element).attr('data-name');
    let medNum = $(element).attr('data-num');
    let medConsume = $(element).attr('data-consume');
    let medExplain = $(element).attr('data-explain');

    if (medNum === null || medNum === undefined || medNum === "") {
        medNum = "";
    }

    if (medConsume === null || medConsume === undefined || medConsume === "" || medConsume === "null") {
        medConsume = "";
    }

    if (medExplain === null || medExplain === undefined || medExplain === "" || medExplain === "null") {
        medExplain = "";
    }


    $("#EditPreId").text(Id);
    $("#ExplainMedicine").val(medExplain);
    $("#ConsumeMedicine").val(medConsume);
    $("#AutoMedicine").val(medName);



    let autocomplete = $("#AutoMedicine").data("kendoAutoComplete");
    autocomplete.enable(false);

    let MedicineNumber = $("#NumMedicine").data("kendoNumericTextBox");
    MedicineNumber.value(medNum);

    //$('#ExplainMedicine').replaceWith($('#ExplainMedicine').clone());

    //$("#ExplainMedicine").unbind();
    //$("#ExplainMedicine").off();

    //document.getElementById("ExplainMedicine").removeEventListener("keydown", newExplainKeypress);
    //document.getElementById("ExplainMedicine").removeEventListener("keypress", newExplainKeypress);

    //var old_element = document.getElementById("ExplainMedicine");
    //var new_element = old_element.cloneNode(true);
    //old_element.parentNode.replaceChild(new_element, old_element);
    //$("#ExplainMedicine").keydown(function (e) { UpdateMedicineKeyPress(e) });

    $("#updateBtn").removeClass('hidden');
    $("#btnAddContent").addClass('hidden');

}

function AddMedicine(e) {
    e.preventDefault();
    var link = "/Medicine/AddNewModal";
    $(".loader").removeClass("hidden");
    $('#NewMedicineModal').modal('toggle');
    $('#NewMedicineModal-body').load(link, function () {
        //setTimeout(function () {
        //    $("#Code").focus();
        //}, 300);
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        //var element = document.getElementById('PrescriptionDetailGrid');
        //var element1 = $('#PrescriptionDetailGrid');
        //console.log(element);
        //console.log(element1);
        //var position = element.offsetTop;
        //window.scrollTo(0, 500);
        //var scrollPosition = window.scrollY;

        //console.log(position);
        //console.log(scrollPosition);
    });

    $('body').css('padding', 0);

    

}

function AddNewMedicine() {


    $('.emptybox').addClass('hidden');
    var isEmmpty = true;
    $('#NewMedicineModal .emptybox').each(function () {
        if ($(this).attr('data-isEssential') === 'true') {
            var empty = $(this).attr('id');

            if ($('[data-checkEmpty="' + empty + '"]').val() !== undefined) {
                let text = $('[data-checkEmpty="' + empty + '"]').val()/*.replace(/ /g, '')*/;
                if (text === "") {
                    $(this).removeClass('hidden');
                    isEmmpty = false;
                    return;
                }
            }

        }
    });


    var link = "/Medicine/AddOrUpdate";

    var data = $("#NewMedicineModal #addNewMedicineForm").serialize();

    if (data == "")
        return;

    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: data,
        success: function (response) {
            if (response !== 0) {
                if (response === "ValueIsRepeated") {

                    $('#Name-Exist').removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else {

                    $('#NewMedicineModal').modal('hide');
                    $(".modal-backdrop:last").remove();
                    $('#NewMedicineModal-body').empty();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    GetAllMedicines();

                }
            }
        }
    });


}

function MedicinePrint(e) {
    e.preventDefault();
    $(".loader").removeClass("hidden");
    var visitId = $("#prescriptionVisit").attr("data-prescriptionVisitId");// $("#data").attr("data-visitId");

    $.ajax({
        url: '/Visit/VisitPrescriptionWithmedicinesPrint',
        type: "Post",
        data: { visitId: visitId },
        success: function (response) {

            draw2(response);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        },
        error: function (response) {
            console.log(response);
            console.log(response.data);
            console.log(response.error);

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });

    $('body').css('padding', 0);
}

function MedicineList(e) {
    e.preventDefault();

    var link = "/Visit/ShowVisitFromServerModal";
    $(".loader").removeClass("hidden");
    $('#ShowMedicineListModal').modal('toggle');
    $('#ShowMedicineListModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });
    $('body').css('padding', 0);
}

function closeShowMedicineListModal() {
    $('#ShowMedicineListModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#ShowMedicineListModal-body').empty();
}

function SendMedicine(e) {
    e.preventDefault();
    $("#SendMedicineModal #ERROR_SomeThingWentWrong").addClass("hidden");
    $("#SendMedicineModal #ServerSomeThingWentWrong").addClass("hidden");
    $("#SendMedicineModal #VisitNotExists").addClass("hidden");
    $("#SendMedicineModal #EmptyMedicine").addClass("hidden");
    $("#SendMedicineModal #CantConnectToServer").addClass("hidden");
    $("#SendMedicineModal #EmptyMobileOrPatientOrDoctorName").addClass("hidden");
    $("#SendMedicineModal #InvalidMedicines").addClass("hidden");
    $("#SendMedicineModal #UpdateNotAllowed").addClass("hidden");
    $("#SendMedicineModal #NotFound").addClass("hidden");
    $("#SendMedicineModal #EmptyPatientMobile").addClass("hidden");


    var dataId = $("#serverVisitId").attr("data-id");
    var txt = "";
    if (dataId != "0" && dataId != "") {
        txt = "Do You Want To Update Medicines?";
    } else {
        txt = "Do You Want To Send Medicine To Drugstore?";
    }
    $("#SendMedicineModal-txt").text(txt);

    $(".loader").removeClass("hidden");
    $('#SendMedicineModal').modal('toggle');

    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
    $('body').css('padding', 0);
}

$('#btn-SendMedicine-accept').on("click", function () {
    $(this).attr("disabled", true);

    $("#SendMedicineModal #ERROR_SomeThingWentWrong").addClass("hidden");
    $("#SendMedicineModal #ServerSomeThingWentWrong").addClass("hidden");
    $("#SendMedicineModal #VisitNotExists").addClass("hidden");
    $("#SendMedicineModal #EmptyMedicine").addClass("hidden");
    $("#SendMedicineModal #CantConnectToServer").addClass("hidden");
    $("#SendMedicineModal #EmptyMobileOrPatientOrDoctorName").addClass("hidden");
    $("#SendMedicineModal #InvalidMedicines").addClass("hidden");
    $("#SendMedicineModal #UpdateNotAllowed").addClass("hidden");
    $("#SendMedicineModal #NotFound").addClass("hidden");
    $("#SendMedicineModal #EmptyPatientMobile").addClass("hidden");

    $(".loader").removeClass("hidden");
    var visitId = $("#prescriptionVisit").attr("data-prescriptionVisitId"); // $("#data").attr("data-visitId");

    $.ajax({
        url: '/Visit/SendVisitToServer',
        type: "Get",
        data: { visitId: visitId },
        success: function (response) {
            $('#btn-SendMedicine-accept').removeAttr("disabled");

            switch (response) {
                case "0": {
                    $("#SendMedicineModal #ERROR_SomeThingWentWrong").removeClass("hidden");
                }
                    break;
                case "SomethingWentWrong": {
                    $("#SendMedicineModal #ServerSomeThingWentWrong").removeClass("hidden");
                }
                    break;
                case "VisitNotExists": {
                    $("#SendMedicineModal #VisitNotExists").removeClass("hidden");
                }
                    break;
                case "EmptyMedicines": {
                    $("#SendMedicineModal #EmptyMedicine").removeClass("hidden");
                }
                    break;
                case "CantConnectToServer": {
                    $("#SendMedicineModal #CantConnectToServer").removeClass("hidden");
                }
                    break;
                case "EmptyMobileOrPatientOrDoctorName": {
                    $("#SendMedicineModal #EmptyMobileOrPatientOrDoctorName").removeClass("hidden");
                }
                    break;
                case "InvalidMedicines": {
                    $("#SendMedicineModal #InvalidMedicines").removeClass("hidden");
                }
                    break;
                case "UpdateNotAllowed": {
                    $("#SendMedicineModal #UpdateNotAllowed").removeClass("hidden");
                }
                    break;
                case "NotFound": {
                    $("#SendMedicineModal #NotFound").removeClass("hidden");
                }
                    break;
                case "EmptyPatientMobile": {
                    $("#SendMedicineModal #EmptyPatientMobile").removeClass("hidden");
                }
                    break;
                default: {

                    $('#SendMedicineModal').modal('hide');
                    $(".modal-backdrop:last").remove();

                    $("#serverVisitId").attr("data-id", response);

                    bootbox.alert({
                        title: "Medications Have Been Sent Successfully",
                        message: `Prescription Number :    ${response}`,
                        className: 'bootbox-class'
                    });
                }
                    break;

            }

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        },
        error: function (response) {
            $('#btn-SendMedicine-accept').removeAttr("disabled");

            console.log(response);
            console.log(response.data);
            console.log(response.error);

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });

});

function TestReport() {

    $(".loader").removeClass("hidden");
    $('#ReportsModal').modal('toggle');
    var visitId = $("#prescriptionVisit").attr("data-prescriptionVisitId");// $("#data").attr("data-visitId");
    link = "/Visit/VisitPrescriptionTestReport?visitId="
    $('#ReportsModal-body').load(link + visitId, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });

}

function TestPrint() {


    $(".loader").removeClass("hidden");
    var visitId = $("#prescriptionVisit").attr("data-prescriptionVisitId");// $("#data").attr("data-visitId");

    $.ajax({
        url: '/Visit/VisitPrescriptionTestPrint',
        type: "Post",
        data: { visitId: visitId },
        success: function (response) {

            draw2(response);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        },
        error: function (response) {
            console.log(response);
            console.log(response.data);
            console.log(response.error);

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });
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

function DeletePrescriptionTestDetail(element) {

    let te = $("#deleteMedicineText").text();
    bootbox.confirm(te, function (result) {

        if (!result) {

            return;

        }
        else {

            let cellList = $("#PrescriptionTestDetailGrid").find("td:eq(0)");
            for (let i = 0; i < cellList.length; i++) {
                if ($(cellList[i]).html() == 0) {
                    $("#PrescriptionTestDetailGrid").find(".k-pager-refresh").click();
                    return;
                }
            }


            $(".loader").removeClass("hidden");

            let Id = $(element).attr('data-id');

            $.ajax({
                url: '/Visit/DeleteTestInVisit',
                type: "Post",
                data: { Id: Id },
                success: function (response) {
                    $("#PrescriptionTestDetailGrid").find(".k-pager-refresh").click();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                }

            });
        }
    })

}

function DeletePrescriptionOtherAnalysis(element) {

    let te = $("#deleteMedicineText").text();
    bootbox.confirm(te, function (result) {

        if (!result) {

            return;

        }
        else {

            let cellList = $("#PrescriptionOtherAnalysisGrid").find("td:eq(0)");
            for (let i = 0; i < cellList.length; i++) {
                if ($(cellList[i]).html() == 0) {
                    $("#PrescriptionOtherAnalysisGrid").find(".k-pager-refresh").click();
                    return;
                }
            }


            $(".loader").removeClass("hidden");

            let Id = $(element).attr('data-id');

            $.ajax({
                url: '/Visit/DeleteOtherAnalysisInVisit',
                type: "Post",
                data: { Id: Id },
                success: function (response) {
                    $("#PrescriptionOtherAnalysisGrid").find(".k-pager-refresh").click();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                }

            });
        }
    })

}

function AddTest() {

    var link = "/BaseInfo/AddNewModal";
    var TestId = $("#Test").attr('data-Value');
    $(".loader").removeClass("hidden");
    $('#my-modalTest-new').modal('toggle');
    $('#new-modalTest-body').load(link, function () {

        $("#TypeId").val(TestId);
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });


}


function closeNewMedicineModal() {

    $('#NewMedicineModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#NewMedicineModal-body').empty();

}



function GetAllMedicines() {

    $.ajax({
        url: '/Visit/GetAllMedicine',
        type: "GET",
        dataType: "JSON",
        success: function (eventList) {
            medicines = eventList;
            let autocomplete = $("#AutoMedicine").data("kendoAutoComplete");
            autocomplete.setDataSource(medicines);
        }
    });
}



function AnalysisList() {

    var link = "/Visit/ShowAnalysisFromServerModal";
    $(".loader").removeClass("hidden");
    $('#ShowAnalysisListModal').modal('toggle');
    $('#ShowAnalysisListModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });

}

function closeShowAnalysisListModal() {
    $('#ShowAnalysisListModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#ShowAnalysisListModal-body').empty();
}


function SendAnalysis() {

    $("#SendAnalysisModal #ERROR_SomeThingWentWrong").addClass("hidden");
    $("#SendAnalysisModal #ServerSomeThingWentWrong").addClass("hidden");
    $("#SendAnalysisModal #VisitNotExists").addClass("hidden");
    $("#SendAnalysisModal #EmptyAnalysis").addClass("hidden");
    $("#SendAnalysisModal #CantConnectToServer").addClass("hidden");
    $("#SendAnalysisModal #EmptyMobileOrPatientOrDoctorName").addClass("hidden");
    $("#SendAnalysisModal #InvalidMedicines").addClass("hidden");
    $("#SendAnalysisModal #UpdateNotAllowed").addClass("hidden");
    $("#SendAnalysisModal #NotFound").addClass("hidden");
    $("#SendAnalysisModal #EmptyPatientMobile").addClass("hidden");


    var dataId = $("#analysisServerVisitId").attr("data-id");
    var txt = "";
    if (dataId) {
        txt = "Do You Want To Update Analysis?";
    } else {
        txt = "Do You Want To Send Analysis To Laboratory?";
    }
    $("#SendAnalysisModal-txt").text(txt);

    $(".loader").removeClass("hidden");
    $('#SendAnalysisModal').modal('toggle');

    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");

}


$('#btn-SendAnalysis-accept').on("click", function () {
    $(this).attr("disabled", true);

    $("#SendAnalysisModal #ERROR_SomeThingWentWrong").addClass("hidden");
    $("#SendAnalysisModal #ServerSomeThingWentWrong").addClass("hidden");
    $("#SendAnalysisModal #VisitNotExists").addClass("hidden");
    $("#SendAnalysisModal #EmptyAnalysis").addClass("hidden");
    $("#SendAnalysisModal #CantConnectToServer").addClass("hidden");
    $("#SendAnalysisModal #EmptyMobileOrPatientOrDoctorName").addClass("hidden");
    $("#SendAnalysisModal #InvalidMedicines").addClass("hidden");
    $("#SendAnalysisModal #UpdateNotAllowed").addClass("hidden");
    $("#SendAnalysisModal #NotFound").addClass("hidden");
    $("#SendAnalysisModal #EmptyPatientMobile").addClass("hidden");

    $(".loader").removeClass("hidden");
    var visitId = $("#prescriptionVisit").attr("data-prescriptionVisitId"); // $("#data").attr("data-visitId");

    $.ajax({
        url: '/Visit/SendAnalysisToServer',
        type: "Get",
        data: { visitId: visitId },
        success: function (response) {
            $('#btn-SendAnalysis-accept').removeAttr("disabled");

            switch (response) {
                case "0": {
                    $("#SendAnalysisModal #ERROR_SomeThingWentWrong").removeClass("hidden");
                }
                    break;
                case "SomethingWentWrong": {
                    $("#SendAnalysisModal #ServerSomeThingWentWrong").removeClass("hidden");
                }
                    break;
                case "VisitNotExists": {
                    $("#SendAnalysisModal #VisitNotExists").removeClass("hidden");
                }
                    break;
                case "EmptyAnalysis": {
                    $("#SendAnalysisModal #EmptyAnalysis").removeClass("hidden");
                }
                    break;
                case "CantConnectToServer": {
                    $("#SendAnalysisModal #CantConnectToServer").removeClass("hidden");
                }
                    break;
                case "EmptyMobileOrPatientOrDoctorName": {
                    $("#SendAnalysisModal #EmptyMobileOrPatientOrDoctorName").removeClass("hidden");
                }
                    break;
                case "InvalidMedicines": {
                    $("#SendAnalysisModal #InvalidMedicines").removeClass("hidden");
                }
                    break;
                case "UpdateNotAllowed": {
                    $("#SendAnalysisModal #UpdateNotAllowed").removeClass("hidden");
                }
                    break;
                case "NotFound": {
                    $("#SendAnalysisModal #NotFound").removeClass("hidden");
                }
                    break;
                case "EmptyPatientMobile": {
                    $("#SendAnalysisModal #EmptyPatientMobile").removeClass("hidden");
                }
                    break;
                default: {

                    $('#SendAnalysisModal').modal('hide');
                    $(".modal-backdrop:last").remove();

                    $("#analysisServerVisitId").attr("data-id", response);

                    bootbox.alert({
                        title: "Analysis Have Been Sent Successfully",
                        message: `Prescription Number :    ${response}`,
                        className: 'bootbox-class'
                    });
                }
                    break;

            }

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        },
        error: function (response) {
            $('#btn-SendAnalysis-accept').removeAttr("disabled");
            console.log(response);
            console.log(response.data);
            console.log(response.error);

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });

});

