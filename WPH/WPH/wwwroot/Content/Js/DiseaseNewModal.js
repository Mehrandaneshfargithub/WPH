
var DiseaseTypeDropDown;

var listbox;
var listbox2;
var listbox3;
var listbox4;

$(document).ready(function () {

    DiseaseTypeDropDown = $("#DiseaseTypeId").data("kendoDropDownList");

    setTimeout(function () {
        $("#Name").focus();
    }, 200);

    listBoxRefresh();

});


function onDataBound(e) {
    kendo.ui.progress($("#window"), false);
    $(".listbox-kendo").removeClass("hidden");
}


function listBoxRefresh() {

    dataSource = new kendo.data.DataSource({
        serverFiltering: false,
        transport: {
            read: {
                data: { diseaseId: $('#Guid').val(), All: true },
                url: "/Disease/GetAllMedicinesForDisease",
                dataType: "json"
            }
        }

    });

    $("#search").on("input", function (e) {
        listbox.dataSource.filter({ field: "JoineryName", value: $(e.target).val(), operator: "contains" });
    })

    listbox = $("#allMeds").kendoListBox({
        dataSource: dataSource,
        connectWith: "allMedsForDisease",
        dropSources: ["allMedsForDisease"],
        toolbar: {
            position: "right",
            tools: ["transferTo", "transferFrom", "transferAllTo", "transferAllFrom"]
        },
        selectable: "multiple",
        draggable: true,
        dataTextField: "JoineryName",
        dataValueField: "Guid"
    }).data("kendoListBox");

    dataSource = new kendo.data.DataSource({
        serverFiltering: false,
        transport: {
            read: {
                data: { diseaseId: $('#Guid').val(), All: false },
                url: "/Disease/GetAllMedicinesForDisease",
                dataType: "json"
            }
        }

    });

    $("#searchallMedsForDisease").on("input", function (e) {
        listbox2.dataSource.filter({ field: "JoineryName", value: $(e.target).val(), operator: "contains" });
    })

    listbox2 = $("#allMedsForDisease").kendoListBox({
        dataSource: dataSource,
        connectWith: "allMeds",
        dropSources: ["allMeds"],
        selectable: "multiple",
        draggable: true,
        dataTextField: "JoineryName",
        dataValueField: "Guid"
    }).data("kendoListBox");


    dataSource = new kendo.data.DataSource({
        serverFiltering: false,
        transport: {
            read: {
                data: { diseaseId: $('#Guid').val(), All: true },
                url: "/Disease/GetAllSymptomsForDisease",
                dataType: "json"
            }
        }

    });


    $("#searchSymptoms").on("input", function (e) {
        listbox3.dataSource.filter({ field: "Name", value: $(e.target).val(), operator: "contains" });
    })


    listbox3 = $("#allSymptoms").kendoListBox({
        dataSource: dataSource,
        connectWith: "allSymptomsForDisease",
        dropSources: ["allSymptomsForDisease"],
        toolbar: {
            position: "right",
            tools: ["transferTo", "transferFrom", "transferAllTo", "transferAllFrom"]
        },

        selectable: "multiple",
        draggable: true,
        dataTextField: "Name",
        dataValueField: "Guid"
    }).data("kendoListBox");


    dataSource = new kendo.data.DataSource({
        serverFiltering: false,
        transport: {
            read: {
                data: { diseaseId: $('#Guid').val(), All: false },
                url: "/Disease/GetAllSymptomsForDisease",
                dataType: "json"
            }
        }

    });

    $("#searchAllSymptomsForDisease").on("input", function (e) {
        listbox4.dataSource.filter({ field: "Name", value: $(e.target).val(), operator: "contains" });
    })


    listbox4 = $("#allSymptomsForDisease").kendoListBox({
        dataSource: dataSource,
        connectWith: "allSymptoms",
        dropSources: ["allSymptoms"],
        selectable: "multiple",
        draggable: true,
        dataTextField: "Name",
        dataValueField: "Guid",
        dataBound: onDataBound
    }).data("kendoListBox");
}

$('#Name').keypress(function (e) {

    if (e.which === 13 || e.which === 9) {
        $('#Explanation').focus();
    }

})

$('#Explanation').keypress(function (e) {

    if (e.which === 13 || e.which === 9) {
        DiseaseTypeDropDown.focus();
    }

})

$('#Name').focus(function () {
    $("#Name-Exist").addClass('hidden');
    $("#Name-box").addClass('hidden');
});


function AddMedicine() {

    $("#AddMedicineModal #ERROR_MedicineWrong").addClass("hidden");
    $("#AddMedicineModal #Name-Exist").addClass("hidden");
    var link = "/Medicine/AddNewModal";

    $(".loader").removeClass("hidden");
    $('#AddMedicineModal').modal('toggle');
    $('#AddMedicineModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });

}


function addMedicineHide() {

    $('#AddMedicineModal').modal('toggle');
    $('#AddMedicineModal-body').empty();

}



function addNewMedicine() {

    $("#AddMedicineModal #ERROR_MedicineWrong").addClass("hidden");
    $("#AddMedicineModal #Name-Exist").addClass("hidden");
    $(".loader").removeClass("hidden");

    let link = "/Medicine/AddOrUpdate";
    var data = $("#addNewMedicineForm").serialize();

    if (data == "")
        return;

    $.ajax({
        type: "Post",
        url: link,
        data: data,
        success: function (response) {
            if (response !== 0) {
                if (response === "ValueIsRepeated") {

                    $('#AddMedicineModal #Name-Exist').removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                }
                else {

                    listbox.setDataSource(new kendo.data.DataSource({
                        serverFiltering: false,
                        transport: {
                            read: {
                                data: { diseaseId: $('#Guid').val(), All: true },
                                url: "/Disease/GetAllMedicinesForDisease",
                                dataType: "json"
                            }
                        }

                    }));

                    $('#AddMedicineModal').modal('hide');
                    $(".modal-backdrop:last").remove();
                    $('#AddMedicineModal-body').empty();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                }
            } else {

                $("#AddMedicineModal #ERROR_MedicineWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });


}

