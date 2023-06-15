

var optional;
var consumeData;
var expData;
var expTestData;
var index = 1;
var duplicateMedicine = $('#duplicateMedicine').text();
var grid;
var Testgrid;
var updateMode;
var medicines;
var canvas;

function GetAllTodayVisits() {
    return $.ajax({
        type: "Get",
        url: "/Visit/GetAllTodayVisitsForVisit",
    });
}

$(document).ready(function () {

    $.when(GetAllTodayVisits(), GetTodayVisit(visitNumber)).done(function (visits, viewModel) {

        let model = viewModel[0];

        GetAllTodayVisitsForVisit(visits[0], model);

        if (model != null) {

            var service_container = $("#PayVisitAndServiceContainer");
            if (service_container) {
                service_container.empty();
                
                var link = `/Visit/PayVisitContainer?visitId=${model.Guid}`;

                service_container.load(link + '', function (res) {
                    if (res === '"0"')
                    $(service_container).addClass('hidden');
                });
            }

            $("#data").attr("data-visitId", model.Guid);
            $("#prescriptionVisit").attr("data-prescriptionVisitId", model.Guid);
            $("#data").attr("data-currentVisit", model.VisitNum);
            $("#data").attr("data-Guid", model.PatientId);
            $("#serverVisitId").attr("data-id", model.ServerVisitNum);
            $("#analysisServerVisitId").attr("data-id", model.AnalysisServerVisitNum);
            $("#EditUnder").attr("data-id", model.PatientId);

            $("#Guid").val(model.Guid);
            $("#PatientId").val(model.PatientId);

            $("#FormNum").text(model.FileNum);
            $("#PatientName").text(model.PatientName);
            $("#UniqueVisitNum").text(model.UniqueVisitNum);
            $("#VisitNum").text(model.VisitNum);
            $("#ReserveStartTime").text(GetTime(model.ReserveStartTime));
            $("#VisitTime").text(GetTimeStamp(model.VisitTime));
            $("#VisitDate").text(GetDate(model.VisitDate));
            $('#Explanation').val(model.Explanation);

            if (model.StatusName == "Visiting" || model.StatusName == "Visited") {

                GetAllVisitImages();
                GetAllPatientImages();

                $("#PatientMedicine").data("kendoGrid").dataSource.transport.options.read.url = `/Visit/GetAllMedicinesRecordForPatient?Id=${model.PatientId}`;
                $("#PatientMedicine .k-pager-refresh")[0].click();

                $("#SocialDisease").data("kendoGrid").dataSource.transport.options.read.url = `/Visit/GetAllSocialDiseaseForPatient?Id=${model.PatientId}`;
                $("#SocialDisease .k-pager-refresh")[0].click();

                $("#AllergicDisease").data("kendoGrid").dataSource.transport.options.read.url = `/Visit/GetAllAllergicDiseaseForPatient?Id=${model.PatientId}`;
                $("#AllergicDisease .k-pager-refresh")[0].click();

                $("#underlyingDisease").data("kendoGrid").dataSource.transport.options.read.url = `/Visit/GetAllNormalDiseaseForPatient?Id=${model.PatientId}`;
                $("#underlyingDisease .k-pager-refresh")[0].click();

                $("#PrescriptionDetailGrid").data("kendoGrid").dataSource.transport.options.read.url = `/Visit/GetAllVisitPrescriptionDetail?Id=${model.Guid}`;
                $("#PrescriptionDetailGrid .k-pager-refresh")[0].click();

                $("#PrescriptionTestDetailGrid").data("kendoGrid").dataSource.transport.options.read.url = `/Visit/GetAllVisitPrescriptionTest?Id=${model.Guid}`;
                $("#PrescriptionTestDetailGrid .k-pager-refresh")[0].click();

                let otherV = $("#otherAnalysisValue").text();

                if (otherV.toLowerCase() === "true" && otherV.toLowerCase() !== "") {
                    $("#PrescriptionOtherAnalysisGrid").data("kendoGrid").dataSource.transport.options.read.url = `/Visit/GetAllVisitPrescriptionOtherAnalysis?Id=${model.Guid}`;
                    $("#PrescriptionOtherAnalysisGrid .k-pager-refresh")[0].click();

                }

                
                $("#VisitHistory").data("kendoGrid").dataSource.transport.options.read.url = `/Visit/GetAllVisitsForPatient?Id=${model.PatientId}&visitDate=${model.VisitDate}`;
                $("#VisitHistory .k-pager-refresh")[0].click();

            } else {

                $("#Visiting_Visited").remove();
            }


            SelectDisease(model.Guid);
            SelectSymptom(model.Guid);


            if (model.StatusName == "InQueue") {
                $("#StatusName").css("color", "red");
                $("#StatusName").text("In Queue");
            }
            else if (model.StatusName == "Visiting") {
                $("#StatusName").css("color", "orange");
                $("#StatusName").text("Visiting");
            }
            else {
                $("#StatusName").css("color", "green");
                $("#StatusName").text("Visited");
            }

            $("#GenderId").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Id",
                filter: "contains",
                change: VisitVariableChange,
                value: model.GenderId,
                dataSource: {
                    //type: "jsonp",
                    serverFiltering: false,
                    transport: {
                        read: {
                            url: "/BaseInfo/GetAllGenders",
                        }
                    }
                }
            });

            $("#Explanation").kendoTextArea({
                rows: 3,
                placeholder: "Explanation",
                value: model.Explanation,
                change: VisitVariableChange
            });

            $("#DateOfBirth").kendoDatePicker({
                format: "dd/MM/yyyy",
                value: model.PatientDateOfBirth,
                change: changeBirthOfDatePatientDatetimePicker,
            });

            $("#Year").kendoTextBox({
                change: VisitVariableChange
            });

            $("#Month").kendoTextBox({
                change: VisitVariableChange
            });

            $("#patientFilesInVisit").kendoUpload({
                async: {
                    saveUrl: "/Patient/Async_Save",
                    removeUrl: "/Patient/Async_Remove",
                    autoUpload: true
                },
                multiple: true,
                showFileList: false,
                complete: onVisitImageComplete,
                error: onPatientImageError,
                upload: OnUploadVisitFile
            });

            let date = $("#DateOfBirth").data("kendoDatePicker");
            //getAge(kendo.toString(date.value(), 'd'));

            let today = new Date();
            let birthDate = new Date(kendo.toString(date.value(), 'd'));


            let age = today.getFullYear() - birthDate.getFullYear();
            let m = today.getMonth() - birthDate.getMonth();
            if (m < 0) {
                age--;
                m = 12 + m;
            }
            if (m === 0 && today.getDate() < birthDate.getDate()) {
                age--;
                m = 11;
            }
            //if (m === 12) {
            //    age++;
            //    m = 0;
            //}
            let mon = birthDate.getMonth() + 1;
            $('#lblYear').text(age);
            $('#lblMonth').text(m);
            setTimeout(() => {
                $("#lblGenderId").text($("#GenderId").data("kendoDropDownList").text())
            }, 1000);



            createVariables();
            //$("#lblYear").remove();
            //$("#lblMonth").remove();
            //$("#lblGenderId").remove();
        }
        else {
            $("#disease_symptom").remove();
            $("#DateOfBirth").remove();
            $("#Year").remove();
            $("#Month").remove();
            $("#GenderId").remove();
            $("#uploadContainer").remove();
            $("#gridsContainer").remove();
        }

    });

    grid = $("#PrescriptionDetailGrid").data("kendoGrid");

    Testgrid = $("#PrescriptionTestDetailGrid").data("kendoGrid");

    $('[data-toggle="tooltip"]').tooltip();

    var current = parseInt($("#data").attr("data-currentVisit"));
    var all = parseInt($("#data").attr("data-visitCounts"));

    if (current == all) {

        $("li.nextt").addClass("disabled");

    }

    if (current == 1) {
        $("li.previouss").addClass("disabled");
    }


    updateMode = false;



    optional = $("#optional").data("kendoMultiSelect");


    $("#EditUnder").kendoButton();

    GetAllMedicines();
    canvas = document.getElementById('reportImage');


});

$("#AutoMedicine").on('focus', function (e) {

    let inputAuto = $("#AutoMedicine").data("kendoAutoComplete");
    let value = inputAuto.value();
    inputAuto.search(value);
});


$("#AutoTest").on('focus', function (e) {

    //let inputAuto = $("#AutoTest").data("kendoAutoComplete");
    //let value = inputAuto.value();
    //inputAuto.search(value);
});

function AddNewItemToDisease(widgetId, value) {
    var widget = $("#" + widgetId).getKendoMultiSelect();

    var dataSource = widget.dataSource;

    if (confirm("Are you sure?")) {

        $.ajax({
            url: '/Disease/AddDiseaseByName',
            type: "Post",
            data: { Name: value },
            success: function (Id) {
                new_disease = true;

                dataSource.add({
                    Guid: Id,
                    Name: value
                });
                dataSource.sync();
                optional.search(value);
                optional.ul.children().eq(0).click();

            }

        });

    }
}




function GetAllVisitImages() {

    $(".loader").removeClass("hidden");
    var visitId = $("#data").attr("data-visitId");

    $.ajax({
        url: '/Visit/GetAllVisitImages',
        type: "Post",
        data: { visitId: visitId },
        success: function (eventList) {
            $("#ImageView").html('');
            for (let i = 0; i < eventList.length; i++) {
                $("<div class='col-xs-6 col-sm-3' style='text-align:center;margin-bottom:2rem'>" +
                    "<div style='margin-bottom:1rem'> <img src=" + eventList[i].ImageAddress + " width='150' height='150' ondblclick='openPic(this)' /></div>" +
                    "<h5>" + eventList[i].FileName + "</h5>" +
                    "<a class='k-primary grid-btn' style='margin-top:5rem'  onclick='DeleteTestImage(this)' data-Id =" + eventList[i].Guid + " > Delete </a>" +
                    "</div> ").appendTo($("#ImageView"));
            }

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });
}


function DeleteTestImage(element) {

    bootbox.confirm('Do you Want Delete This Image?', function (result) {

        if (!result) {

            return;
        }
        else {
            var Id = $(element).attr("data-Id");

            $(".loader").removeClass("hidden");
            $.ajax({

                url: '/Patient/RemovePatientImage',

                type: "Post",

                data: { patientImageId: Id },

                success: function () {

                    $("#ImageView").html('');
                    GetAllVisitImages();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                }

            });
        }

    })

}




function GetAllPatientImages() {

    $(".loader").removeClass("hidden");
    var patientId = $("#data").attr("data-Guid");
    var visitId = $("#data").attr("data-visitId");
    $.ajax({

        url: '/Patient/GetAllPatientImages',

        type: "Post",

        data: { patientId: patientId },

        success: function (eventList) {
            $("#PatientImageView").html('');
            for (let i = 0; i < eventList.length; i++) {
                if (eventList[i].VisitId !== visitId) {
                    $("<div class='col-xs-6 col-sm-3' style='text-align:center;margin-bottom:2rem'>" +
                        "<div style='margin-bottom:1rem'> <img src=" + eventList[i].ImageAddress + " width='150' height='150' ondblclick='openPic(this)' /></div>" +
                        "<h5>" + eventList[i].FileName + "</h5>" +
                        "<a class='k-primary grid-btn' style='margin-top:5rem'  onclick='DeletePatientImage(this)' data-Id =" + eventList[i].Guid + " > Delete </a>" +
                        "</div> ").appendTo($("#PatientImageView"));
                }

            }

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");

        }


    });

}


function DeletePatientImage(element) {

    bootbox.confirm('Do you Want Delete This Image?', function (result) {

        if (!result) {

            return;
        }
        else {
            var Id = $(element).attr("data-Id");

            $(".loader").removeClass("hidden");
            $.ajax({
                url: "/PatientImage/remove/" + Id,
                type: "Post",
                success: function () {

                    $("#PatientImageView").html('');
                    GetAllPatientImages();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                }
            });
        }
    })
}

function openPic(e) {

    //var destenation = $(e).next().next().text();
    var destenation1 = $(e).attr('src');

    $("#imagePreview1").attr("src", destenation1);
    $("#window1").data("kendoWindow").center().open();
}


function onMultiSelectClose() {

    var itemList = $("#optional").val();
    var visitId = $("#data").attr("data-visitId");
    if (itemList == null) {

        return
    }
    $(".loader").removeClass("hidden");
    $.ajax({
        url: '/Visit/GetMedicineForDisease',
        type: "Post",
        dataType: "JSON",
        data: { diseaseId: itemList, visitId: visitId },
        success: function (eventList) {

            //var rowCount, medicineName;

            for (var i = 0; i < eventList.length; i++) {
                rowCount = $("#medicineTable tr").length;
                var exist = false;
                medicineName = eventList[i].JoineryName;
                for (var j = 0; j < rowCount; j++) {
                    if (medicineName == $("#medicineTable").find('tr:eq(' + j + ')').find('td:eq(1)').text()) {

                        exist = true;

                    }


                }
                if (!exist) {
                    addMed($("#medicineTable tr").length, eventList[i]);
                }

            }
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }

    });

}


function onMultiSelectClose() {

    var itemList = $("#optional").val();
    var visitId = $("#data").attr("data-visitId");
    if (itemList == null) {

        return
    }
    $(".loader").removeClass("hidden");
    $.ajax({
        url: '/Visit/GetMedicineForDisease',
        type: "Post",
        dataType: "JSON",
        data: { diseaseId: itemList, visitId: visitId },
        success: function (eventList) {

            //var rowCount, medicineName;

            for (var i = 0; i < eventList.length; i++) {
                rowCount = $("#medicineTable tr").length;
                var exist = false;
                medicineName = eventList[i].JoineryName;
                for (var j = 0; j < rowCount; j++) {
                    if (medicineName == $("#medicineTable").find('tr:eq(' + j + ')').find('td:eq(1)').text()) {
                        exist = true;
                    }
                }
                if (!exist) {
                    addMed($("#medicineTable tr").length, eventList[i]);
                }

            }
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }

    });

}


function GetDiseaseIdForMedicines() {
    var Id = $("#btn-MedicineDisease-accept").attr('data-diseaseId')

    return {
        Id: Id
    };
}


$('#btn-MedicineDisease-accept').on("click", function () {
    $(this).attr("disabled", true);

    entityGrid = $("#KendoMedicineDiseaseGrid").data("kendoGrid");
    //selectedItem = entityGrid.dataItem(entityGrid.select());
    var allmed = [];
    selectedItem = entityGrid.select();
    for (let i = 0; i < selectedItem.length; i++) {
        let medDi = entityGrid.dataItem(selectedItem[i]);
        allmed.push(medDi.MedicineId)
    }

    if (selectedItem == null) {
        bootbox.alert({
            message: "Please Select One Or More Medicines!",
        });
        $('#btn-MedicineDisease-accept').removeAttr("disabled");
        return
    }

    var visitId = $("#data").attr("data-visitId");

    $.ajax({
        url: '/Visit/AddMedicineDiseaseToVisit',
        type: "Post",
        dataType: "JSON",
        data: { medicineDisease: allmed, visitId: visitId },
        success: function (eventList) {
            $('#btn-MedicineDisease-accept').removeAttr("disabled");

            if (eventList !== 0) {

                $('#MedicineDisease-Modal').modal('toggle');
                $("#PrescriptionDetailGrid").find(".k-pager-refresh").click();
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else {
                bootbox.alert({
                    message: "ERROR",
                    className: 'bootbox-class'
                });
            }

        }

    });

});



$('#btn-test-accept').on("click", function () {
    $(this).attr("disabled", true);

    $('.emptybox').addClass('hidden');
    var isEmmpty = true;
    $('.emptybox').each(function () {
        if ($(this).attr('data-isEssential') === 'true') {
            var empty = $(this).attr('id');
            if ($('[data-checkEmpty="' + empty + '"]').val() === "") {
                $(this).removeClass('hidden');
                $('#btn-test-accept').removeAttr("disabled");
                isEmmpty = false;
                return;
            }
        }
    });

    if (isEmmpty === false) {
        return;
    }

    var link = "/BaseInfo/AddOrUpdate";
    var GridRefreshLink = "/BaseInfo/RefreshGrid";
    var data = $("#addNewForm").serialize();
    var TestId = $("#Test").attr('data-Value');

    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: data,
        success: function (response) {
            $('#btn-test-accept').removeAttr("disabled");

            if (response !== 0) {

                if (response === "ValueIsRepeated") {

                    $('#Name-Exist').removeClass('hidden');
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                } else {

                    $('#my-modalTest-new').modal('hide');
                    $(".modal-backdrop:last").remove();
                    $('#new-modalTest-body').empty();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    let autocomplete = $("#AutoTest").data("kendoAutoComplete");
                    autocomplete.dataSource.read();
                }
            }
        }
    });
});


$('#btn-test-accept-close').on("click", function () {
    $(".loader").removeClass("hidden");
    $('#my-modalTest-new').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#new-modalTest-body').empty();
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});


function EditUnderlyingDisease(element) {


    $(".loader").removeClass("hidden");
    $('#PatientRecordModal').modal('toggle');
    let link = "/Patient/PatientRecordForm?patientid=";
    var patientid = $(element).attr('data-id');

    $('#PatientRecordModal-body').load(link + patientid, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");

    });
}


function customVisitGo() {
    changeVisit($('#customVisitInput').val());
}


function changeVisit(value) {

    var current = parseInt($("#data").attr("data-currentVisit"));
    var all = parseInt($("#data").attr("data-visitCounts"));
    var visitId = $("#data").attr("data-visitId");
    if (value == current || value < 1 || value > all) {
        return
    }

    var link = "/Visit/ChangeVisit";

    var visitNum;
    var status = 0;
    switch (value) {
        case "Next":
            if (current == all) {
                bootbox.alert("!This is a Last Patient");
                return;
            }
            visitNum = current + 1;
            break;

        case "Previous":
            if (current == 1) {
                bootbox.alert("!This is a First Patient");
                return;
            }
            visitNum = current - 1;
            break;
        case "Visited":
            $.ajax({
                url: '/Visit/EventChangeStatus',
                type: "Post",
                data: { visitId: visitId },
                success: function (response) {

                    if (current == all) {
                        //bootbox.alert("!This is a Last Patient");
                        visitNum = current;

                    } else {
                        visitNum = current + 1;
                        status = current;
                    }

                    $(".loader").removeClass("hidden");

                    $(".page-content").load(link + "?visitNumber=" + visitNum + "&current=" + status, function () {

                        if (current == all) {
                            bootbox.alert("!This is a Last Patient");
                        } 

                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");
                        $(".modal-backdrop:last").remove();

                        $("body").removeClass("modal-open");
                        $("body").css("padding-right", "0px");
                    });

                    return;
                }
            });

            return;

            break;

        default:
            visitNum = value;

    }

    $(".loader").removeClass("hidden");

    $(".page-content").load(link + "?visitNumber=" + visitNum + "&current=" + status, function () {

        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        $(".modal-backdrop:last").remove();

        $("body").removeClass("modal-open");
        $("body").css("padding-right", "0px");
    });

}


function visitRefresh() {
    var link = "/Visit/Form";

    $(".loader").removeClass("hidden");

    $(".page-content").load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });
}

function PrescriptionDetailGridSave() {

    grid.saveChanges();
    $("#PrescriptionDetailGrid").find(".k-pager-refresh").click();

}


function PrescriptionDetailGridCancelChange() {

    grid.cancelChanges();
}


function PrescriptionDetailGridAddRow() {

    let cellList = $("#PrescriptionDetailGrid").find("td:eq(0)");
    for (let i = 0; i < cellList.length; i++) {
        if ($(cellList[i]).html() == 0) {
            return;
        }
    }

    grid.bind("dataBound", grid_AddRow);

    grid.saveChanges();

}


function PrescriptionTestDetailGridSave() {

    Testgrid.saveChanges();
    $("#PrescriptionTestDetailGrid").find(".k-pager-refresh").click();
}


function PrescriptionTestDetailGridCancelChange() {

    Testgrid.cancelChanges();
}


function PrescriptionTestDetailGridAddRow() {

    let cellList = $("#PrescriptionTestDetailGrid").find("td:eq(0)");
    for (let i = 0; i < cellList.length; i++) {
        if ($(cellList[i]).html() == 0) {
            return;
        }
    }


    Testgrid.bind("dataBound", Testgrid_AddRow);
    Testgrid.saveChanges();

}


function patientHide() {


    $('#PatientRecordModal').modal('hide');

    $('#PatientRecordModal-body').empty();


}


function patientHideAndRedresh() {

    $("#DiseaseHistory").find(".k-pager-refresh").click();
    $("#AllergicDiseaseHistory").find(".k-pager-refresh").click();
    $("#SocialDiseaseHistory").find(".k-pager-refresh").click();

    $('#PatientRecordModal').modal('hide');

    $('#PatientRecordModal-body').empty();

}


function NumKeypress(e) {

    if (e.which === 13 || e.which === 9) {

        setTimeout(function () {

            var curCell = $("#PrescriptionDetailGrid").find(".k-state-selected")
            var eCell = $("#PrescriptionDetailGrid").find(".k-edit-cell")

            curCell.removeClass("k-state-selected");
            curCell.removeClass("k-state-focused");
            curCell.removeAttr("data-role");
            curCell.next().addClass("k-state-selected");
            curCell.next().addClass("k-state-focused");
            try {
                $('#PrescriptionDetailGrid').data('kendoGrid').closeCell(eCell);
            } catch (ex) {
            }
            $('#PrescriptionDetailGrid').data('kendoGrid').select();
            $('#PrescriptionDetailGrid').data('kendoGrid').editCell(eCell.next());



        }, 50);
    }

}

function explainKeypress(e) {


    if (e.which === 13) {
        setTimeout(function () {

            var curCell = $("#PrescriptionDetailGrid").find(".k-state-selected")
            var eCell = $("#PrescriptionDetailGrid").find(".k-edit-cell")

            curCell.removeClass("k-state-selected");
            curCell.removeClass("k-state-focused");
            curCell.removeAttr("data-role");
            curCell.next().addClass("k-state-selected");
            curCell.next().addClass("k-state-focused");
            try {
                $('#PrescriptionDetailGrid').data('kendoGrid').closeCell(eCell);
            } catch (ex) {
            }
            grid.bind("dataBound", grid_AddRow);

            grid.saveChanges();



        }, 50);

    }

}


function grid_AddRow() {

    setTimeout(function () {

        if (!updateMode) {

            grid.addRow();
            updateMode = false;

        }


    }, 50);


    grid.unbind("dataBound", grid_AddRow);
}

function TestExplainKeypress(e) {


    if (e.which === 13) {

        setTimeout(function () {

            var curCell = $("#PrescriptionTestDetailGrid").find(".k-state-selected")

            var eCell = $("#PrescriptionTestDetailGrid").find(".k-edit-cell")


            curCell.removeClass("k-state-selected");

            curCell.removeClass("k-state-focused");

            curCell.removeAttr("data-role");

            curCell.next().addClass("k-state-selected");

            curCell.next().addClass("k-state-focused");

            try {

                $('#PrescriptionTestDetailGrid').data('kendoGrid').closeCell(eCell);

            } catch (ex) {

            }

            Testgrid.bind("dataBound", Testgrid_AddRow);


            Testgrid.saveChanges();

        }, 50);


    }

}

function Testgrid_AddRow() {

    setTimeout(function () {

        if (!updateMode) {

            Testgrid.addRow();
            updateMode = false;

        }


    }, 50);


    Testgrid.unbind("dataBound", Testgrid_AddRow);
}

function UpdateMedicineKeyPress(e) {

    if (e.which === 13 || e.which === 9) {

        UpdateMedicine();

    }

}

function AddPatientVariable(PatientId) {

    $(".loader").removeClass("hidden");
    $("#my-modal-PatientVariables").modal('toggle');
    var patientid = PatientId;
    let pName = $("#PatientName").text();
    $("#PatientVariables-modal-header").text(pName);

    $("#btn-PatientVariables-accept").attr('data-patientId', patientid);

    var link = "/Patient/PatientVariableModal?patientId=";
    $('#PatientVariables-modal-body').load(link + patientid, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });

}


function ShowVisitList() {

    $(".loader").removeClass("hidden");
    $("#VisitList-Modal").modal('toggle');

    var link = "/Visit/VisitTodayList";

    $('#VisitList-Modal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });

}


function MedicineReport() {

    $(".loader").removeClass("hidden");
    $('#ReportsModal').modal('toggle');
    var visitId = $("#data").attr("data-visitId");
    link = "/Visit/VisitPrescriptionWithmedicinesReport?visitId="
    $('#ReportsModal-body').load(link + visitId, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });
}


function ReserveInsertAge(element) {

    var Year = parseInt($('#Year').val());
    var Month = parseInt($('#Month').val());
    if (Month === NaN) {
        Month = 0;
    }
    var today = new Date();
    var birthDay = today;
    birthDay.setFullYear(birthDay.getFullYear() - Year);
    birthDay.setMonth(birthDay.getMonth() - Month);
    let birthMonth = birthDay.getMonth() + 1;
    $('#DateOfBirth').val('1/' + birthMonth + '/' + birthDay.getFullYear());

}


function SelectDisease(visitId) {

    $.when(GetAllDiseases(), GetAllVisitDisease(visitId)).done(function (disease, visitDisease) {

        $("#optional").kendoMultiSelect({
            placeholder: "Select disease...",
            dataTextField: "Name",
            dataValueField: "Guid",
            autoClose: false,
            deselect: onMultiSelectDeselect,
            select: onMultiSelectSelect,
            dataSource: disease[0],
            value: visitDisease[0]
        });

    });
}

function GetAllDiseases() {
    return $.ajax({
        type: "Get",
        url: "/Disease/GetAllDiseasesJustNameAndGuid",
    });
}

function GetAllVisitDisease(visitId) {
    return $.ajax({
        type: "Get",
        url: "/Visit/GetAllVisitDisease?visitId=" + visitId,
    });
}

function SelectSymptom(visitId) {

    $.when(GetAllSymptom(), GetAllVisitSymptom(visitId)).done(function (symptom, visitSymptom) {
        $("#Symptom").kendoMultiSelect({
            placeholder: "Select symptom...",
            dataTextField: "Name",
            dataValueField: "Guid",
            autoClose: false,
            deselect: onSymptomSelectDeselect,
            select: onSymptomSelectSelect,
            dataSource: symptom[0],
            value: visitSymptom[0]
        });
    });
}

function GetAllSymptom() {
    return $.ajax({
        type: "Get",
        url: "/Symptom/GetAllSymptomJustNameAndGuid",
    });
}

function GetAllVisitSymptom(visitId) {
    return $.ajax({
        type: "Get",
        url: "/Visit/GetAllVisitSymptom?visitId=" + visitId,
    });
}

function GetTimeStamp(timeStamp) {
    let hrs = timeStamp.Hours;
    let mins = timeStamp.Minutes;
    if (hrs <= 9)
        hrs = '0' + hrs;
    if (mins < 10)
        mins = '0' + mins;
    const postTime = hrs + ':' + mins;
    return postTime;
}

function GetTime(dateTime) {
    let data = new Date(dateTime);
    let hrs = data.getHours();
    let mins = data.getMinutes();
    if (hrs <= 9)
        hrs = '0' + hrs;
    if (mins < 10)
        mins = '0' + mins;
    const postTime = hrs + ':' + mins;
    return postTime;
}

function GetDate(dateTime) {
    let data = new Date(dateTime);
    let day = data.getDate();
    let month = data.getMonth() + 1;
    let year = data.getFullYear();
    if (day < 10)
        day = '0' + day;
    if (month < 10)
        month = '0' + month;
    const postDate = day + '/' + month + '/' + year;
    return postDate;
}

function GetTodayVisit(visitNumber) {
    return $.ajax({
        type: "Get",
        url: `/Visit/GetTodayVisit?visitNumber=${visitNumber}`,
    });
}



async function GetAllTodayVisitsForVisit(visits, visitNum) {

    let ListCount = Object.keys(visits).length;
    $("#data").attr("data-visitCounts", `${ListCount}`);
    let elements = "";

    if (ListCount != 0) {

        let currentVisitNum = visitNum.VisitNum;
        let vis1 = await FirstOrDefault(visits, 1);
        let vis2 = await FirstOrDefault(visits, 2);
        let vis3 = await FirstOrDefault(visits, 3);
        let visCurr = await FirstOrDefault(visits, currentVisitNum);
        let visCurr1 = await FirstOrDefault(visits, currentVisitNum + 1);
        let visCurr0 = await FirstOrDefault(visits, currentVisitNum - 1);
        let visLast = await FirstOrDefault(visits, ListCount);

        if (currentVisitNum - 1 > 0) {
            elements += `<li class="pagee previouss" onclick="changeVisit('Previous')"><a class="page-link" href="#">Previous</a></li>`;

            var color1 = "";
            if (vis1.StatusName == "Visiting") { color1 = "orange"; }
            else if (vis1.StatusName == "Visited") { color1 = "green"; }
            else { color1 = "red"; }

            elements += `<li class="pagee" onclick="changeVisit(1)"><a class="page-link" href="#" style="color:${color1}" data-toggle="tooltip" data-placement="bottom">1</a></li>`;

            if (currentVisitNum >= 3) {
                if (currentVisitNum == 3) {
                    var color2 = "";
                    if (vis2.StatusName == "Visiting") { color2 = "orange"; }
                    else if (vis2.StatusName == "Visited") { color2 = "green"; }
                    else { color2 = "red"; }

                    elements += `<li class="pagee" onclick="changeVisit(2)"><a class="page-link" href="#" style="color:${color2}" data-toggle="tooltip" data-placement="bottom">2</a></li>`;

                    var color3 = "";
                    if (vis3.StatusName == "Visiting") { color3 = "orange"; }
                    else if (vis3.StatusName == "Visited") { color3 = "green"; }
                    else { color3 = "red"; }

                    elements += `<li class="pagee disabled" onclick="changeVisit(3)"><a class="page-link" href="#" style="color:${color3}" data-toggle="tooltip" data-placement="bottom">3</a></li>`;

                    if (currentVisitNum + 1 <= ListCount) {
                        var color4 = "";
                        if (visCurr1.StatusName == "Visiting") { color4 = "orange"; }
                        else if (visCurr1.StatusName == "Visited") { color4 = "green"; }
                        else { color4 = "red"; }

                        elements += `<li class="pagee " onclick="changeVisit(${currentVisitNum + 1})"><a class="page-link" href="#" style="color:${color4}" data-toggle="tooltip" data-placement="bottom">${currentVisitNum + 1}</a></li>`;
                    }
                    if (currentVisitNum + 3 <= ListCount) {
                        elements += `<li class="pagee disabled"><a class="page-link" href="#">...</a></li>`;
                    }
                    if (currentVisitNum + 2 <= ListCount) {
                        var color5 = "";
                        if (visLast.StatusName == "Visiting") { color5 = "orange"; }
                        else if (visLast.StatusName == "Visited") { color5 = "green"; }
                        else { color5 = "red"; }

                        elements += `<li class="pagee " onclick="changeVisit(${ListCount})"><a class="page-link" href="#" style="color:${color5}" data-toggle="tooltip" data-placement="bottom">${ListCount}</a></li>`;
                    }
                }
                if (currentVisitNum > 3) {
                    elements += `<li class="pagee disabled"><a class="page-link" href="#">...</a></li>`;

                    var color6 = "";
                    if (visCurr0.StatusName == "Visiting") { color6 = "orange"; }
                    else if (visCurr0.StatusName == "Visited") { color6 = "green"; }
                    else { color6 = "red"; }

                    elements += `<li class="pagee " onclick="changeVisit(${currentVisitNum - 1})"><a class="page-link" href="#" style="color:${color6}" data-toggle="tooltip" data-placement="bottom">${currentVisitNum - 1}</a></li>`;

                    var color7 = "";
                    if (visCurr.StatusName == "Visiting") { color7 = "orange"; }
                    else if (visCurr.StatusName == "Visited") { color7 = "green"; }
                    else { color7 = "red"; }

                    elements += `<li class="pagee disabled"><a class="page-link" href="#" style="color:${color7}" data-toggle="tooltip" data-placement="bottom">${currentVisitNum}</a></li>`;

                    if (currentVisitNum + 1 <= ListCount) {
                        var color8 = "";
                        if (visCurr1.StatusName == "Visiting") { color8 = "orange"; }
                        else if (visCurr1.StatusName == "Visited") { color8 = "green"; }
                        else { color8 = "red"; }

                        elements += `<li class="pagee " onclick="changeVisit(${currentVisitNum + 1})"><a class="page-link" href="#" style="color:${color8}" data-toggle="tooltip" data-placement="bottom">${currentVisitNum + 1}</a></li>`;
                    }

                    if (currentVisitNum + 3 <= ListCount) {
                        elements += `<li class="pagee disabled"><a class="page-link" href="#">...</a></li>`;
                    }

                    if (currentVisitNum + 2 <= ListCount) {
                        var color9 = "";
                        if (visLast.StatusName == "Visiting") { color9 = "orange"; }
                        else if (visLast.StatusName == "Visited") { color9 = "green"; }
                        else { color9 = "red"; }

                        elements += `<li class="pagee " onclick="changeVisit(${ListCount})"><a class="page-link" href="#" style="color:${color9}" data-toggle="tooltip" data-placement="bottom">${ListCount}</a></li>`;
                    }
                }

                elements += `<li class="pagee nextt" onclick="changeVisit('Next')"><a class="page-link" href="#">Next</a></li>`;
            }
            else {
                var color10 = "";
                if (vis2.StatusName == "Visiting") { color10 = "orange"; }
                else if (vis2.StatusName == "Visited") { color10 = "green"; }
                else { color10 = "red"; }

                elements += `<li class="pagee disabled" onclick="changeVisit(2)"><a class="page-link" href="#" style="color:${color10}" data-toggle="tooltip" data-placement="bottom">2</a></li>`;

                if (currentVisitNum + 1 <= ListCount) {
                    var color11 = "";
                    if (visCurr1.StatusName == "Visiting") { color11 = "orange"; }
                    else if (visCurr1.StatusName == "Visited") { color11 = "green"; }
                    else { color11 = "red"; }

                    elements += `<li class="pagee " onclick="changeVisit(${currentVisitNum + 1})"><a class="page-link" href="#" style="color:${color11}" data-toggle="tooltip" data-placement="bottom">${currentVisitNum + 1}</a></li>`;
                }

                if (currentVisitNum + 3 <= ListCount) {
                    elements += `<li class="pagee disabled"><a class="page-link" href="#">...</a></li>`;
                }

                if (currentVisitNum + 2 <= ListCount) {
                    var color12 = "";
                    if (visLast.StatusName == "Visiting") { color12 = "orange"; }
                    else if (visLast.StatusName == "Visited") { color12 = "green"; }
                    else { color12 = "red"; }

                    elements += `<li class="pagee " onclick="changeVisit(${ListCount})"><a class="page-link" href="#" style="color:${color12}" data-toggle="tooltip" data-placement="bottom">${ListCount}</a></li>`;
                }

                elements += `<li class="pagee nextt" onclick="changeVisit('Next')"><a class="page-link" href="#">Next</a></li>`;
            }
        }
        else {
            elements += `<li class="pagee previouss" onclick="changeVisit('Previous')"><a class="page-link" href="#">Previous</a></li>`;

            var color13 = "";
            if (vis1.StatusName == "Visiting") { color13 = "orange"; }
            else if (vis1.StatusName == "Visited") { color13 = "green"; }
            else { color13 = "red"; }

            elements += `<li class="pagee disabled"><a class="page-link" href="#" style="color:${color13}" data-toggle="tooltip" data-placement="bottom">1</a></li>`;

            if (currentVisitNum + 1 <= ListCount) {
                var color14 = "";
                if (vis2.StatusName == "Visiting") { color14 = "orange"; }
                else if (vis2.StatusName == "Visited") { color14 = "green"; }
                else { color14 = "red"; }

                elements += `<li class="pagee" onclick="changeVisit(2)"><a class="page-link" href="#" style="color:${color14}" data-toggle="tooltip" data-placement="bottom">2</a></li>`;
            }

            if (currentVisitNum + 3 <= ListCount) {
                elements += `<li class="pagee disabled"><a class="page-link" href="#">...</a></li>`;
            }

            if (currentVisitNum + 2 <= ListCount) {
                var color15 = "";
                if (visLast.StatusName == "Visiting") { color15 = "orange"; }
                else if (visLast.StatusName == "Visited") { color15 = "green"; }
                else { color15 = "red"; }

                elements += `<li class="pagee " onclick="changeVisit(${ListCount})"><a class="page-link" href="#" style="color:${color15}" data-toggle="tooltip" data-placement="bottom">${ListCount}</a></li>`;

            }

            elements += `<li class="pagee nextt" onclick="changeVisit('Next')"><a class="page-link" href="#">Next</a></li>`;
        }

    } else {
        elements +=
            `<li class="pagee disabled"><a class="page-link" href="#">Previous</a></li>
                 <li class="pagee disabled"><a class="page-link" href="#">Next</a></li>`;
    }

    elements +=
        `<li class="pagee disabled" style="padding:0;margin-left:0.5rem"><input id="customVisitInput" type="number" style="margin:0;width:6rem" /> </li>
             <li class="pagee disabled btn-success" style="margin-left:0.5rem" onclick="customVisitGo()">GO </li>
             <li class="pagee disabled btn-success" style="margin-left:0.5rem" onclick="visitRefresh()"><i class="fa fa-refresh"></i> </li>
             <li class="pagee disabled btn-success" style="margin-left:0.5rem" onclick="changeVisit('Visited')">Visited And Next </li>
             <li class="pagee disabled btn-success" style="margin-left:0.5rem" onclick="ShowVisitList()">Visit List</li>`;

    var ul = document.getElementById("visitPaging");
    ul.insertAdjacentHTML('afterbegin', elements);

}


function FirstOrDefault(visitList, number) {
    return new Promise((resolve) => {
        let vis = null;

        for (var key in visitList) {
            if (visitList[key].VisitNum == number) {
                vis = visitList[key];
                break;
            }
        }

        resolve(vis);
    });
};

