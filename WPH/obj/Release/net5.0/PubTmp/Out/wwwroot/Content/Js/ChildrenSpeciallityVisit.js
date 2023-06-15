

    var optional;
    var Symptom;
    var consumeData;
    var expData;
    var expTestData;
    var index = 1;
    var duplicateMedicine = $('#duplicateMedicine').text();
    var grid;
    var Testgrid;
    var updateMode;
    var autocomplete;
    var medicines;
    var new_disease = false;
    var canvas ;
    var photo ;

    $(document).ready(function () {

        let date = $("#DateOfBirth").data("kendoDatePicker");

    if (date !== undefined)
        getAge(kendo.toString(date.value(), 'd'));

    let date2 = $("#insertDate").data("kendoDatePicker");

    if (date2 !== undefined)
    getAgeNew(kendo.toString(date2.value(), 'd'));

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






var customVis = document.getElementById('customVisitInput');

        customVis.addEventListener("keydown", function (e) {
            if (e.keyCode == 13) {
        changeVisit($('#customVisitInput').val());
}
});



         $("#optional").kendoMultiSelect({

        dataTextField: "Name",
    dataValueField: "Guid",
    autoClose: false,
    deselect: onMultiSelectDeselect,
     select: onMultiSelectSelect,
    noDataTemplate: $("#noDataTemplate").html()
 });

optional = $("#optional").data("kendoMultiSelect");




        Symptom = $("#Symptom").kendoMultiSelect({
        autoClose: false,
    deselect: onSymptomSelectDeselect,
    select: onSymptomSelectSelect,
});

$("#EditUnder").kendoButton();



GetAllVisitImages();
GetAllPatientImages();
GetAllMedicines();

        if (grid !== undefined) {
        setTimeout(function () {
            grid_AddRow();
            setTimeout(function () {
                let numericHeight = $("#HT").data("kendoNumericTextBox");
                numericHeight.focus();
            }, 50);

        }, 100);

}

canvas = document.getElementById('reportImage');
photo = document.getElementById('photo');
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




    function showChart() {
        $(".loader").removeClass("hidden");
    $('#ChartModal').modal('toggle');
    let link = "/Patient/ChartModal?patientId=";
    var patientid = $("#data").attr("data-Guid");
    $("#ChartModal-body").attr("data-id", patientid);
        $('#ChartModal-body').load(link + patientid, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");

});
}

    function GetAllMedicines() {

        $.ajax({
            url: '/Visit/GetAllMedicine',
            type: "GET",
            dataType: "JSON",

            success: function (eventList) {

                medicines = eventList;
                if (autocomplete != undefined)
                    autocomplete.setDataSource(medicines);
                
            }

        });

}


    function GetAllVisitImages() {

        $(".loader").removeClass("hidden");
    var visitId = $("#data").attr("data-visitId");

        $.ajax({

        url: '/Visit/GetAllVisitImages',

    type: "Post",

            data: {visitId: visitId },

            success: function (eventList) {
        $("#VisitImageView").html('');
                for (let i = 0; i < eventList.length; i++) {
        $("<div class='col-xs-6 col-sm-3' style='text-align:center;margin-bottom:2rem'>" +
            "<div style='margin-bottom:1rem'> <img src=" + eventList[i].ImageAddress + " width='150' height='150' ondblclick='openPic(this)' /></div>" +
            "<h5>" + eventList[i].FileName + "</h5>" +
            "<a class='k-primary grid-btn' style='margin-top:5rem'  onclick='DeleteVisitImage(this)' data-Id =" + eventList[i].Guid + " > Delete </a>" +
            "</div> ").appendTo($("#VisitImageView"));
}

$(".loader").fadeIn("slow");
$(".loader").addClass("hidden");

}


});

}


    function GetAllPatientImages() {

        $(".loader").removeClass("hidden");
    var patientId = $("#data").attr("data-Guid");
    var visitId = $("#data").attr("data-visitId");
        $.ajax({

        url: '/Patient/GetAllPatientImages',

    type: "Post",

            data: {patientId: patientId },

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


    function closeCheckboxVariableChange(element) {

        var property = $(element).attr("data-property");
    var value;
        if ($(element).hasClass("fa-stack")) {
        value = false;
    $(element).next().removeClass("hidden");
}
        else {
        value = true;
    $(element).prev().removeClass("hidden");
}

$(element).addClass("hidden");
var visitId = $('#data').attr('data-visitId');
var patientId = $('#data').attr('data-Guid');
var PatientVariableValueGuid = $(element).attr('data-patientvariablevalueGuid');

$.ajax(
            {
        type: "Post",
    url: "/Visit/UpdateVisitVariables",
                data: {Guid: PatientVariableValueGuid, Property: property, VisitId: visitId, Value: value, PatientId: patientId },
                success: function (Guid) {
        //$(".loader").fadeIn("slow");
        //$(".loader").addClass("hidden");
        $(element).attr('data-patientvariablevalueGuid', Guid);
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

                    url: '/Patient/RemovePatientImage',

                    type: "Post",

                    data: { patientImageId: Id },

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


    function DeleteVisitImage(element) {

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

                        $("#VisitImageView").html('');
                        GetAllVisitImages();
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



    $("#imagePreview").attr("src", destenation1);
    $("#window").data("kendoWindow").center().open();
}







    function onMultiSelectSelect(e) {


        if (!new_disease) {
        let dataItem = e.dataItem;

    var visitId = $("#data").attr("data-visitId");


    $(".loader").removeClass("hidden");

    var link = "/Visit/MedicineForDiseaseModal?diseaseId=";
    $("#btn-MedicineDisease-accept").attr('data-diseaseId', dataItem.Guid)
    $('#MedicineDisease-Modal').modal('toggle');
            $('#MedicineDisease-Modal-body').load(link + dataItem.Guid + "&visitId=" + visitId, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});
}
        else {
        let dataItem = e.dataItem;

    var visitId = $("#data").attr("data-visitId");

            $.ajax({
        url: '/Visit/AddDiseaseToVisit',
    type: "Post",
                data: {diseaseId: dataItem.Guid, visitId: visitId },
})
}
new_disease = false;



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
            data: {diseaseId: itemList, visitId: visitId },
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



    function onMultiSelectDeselect(e) {
        var dataItem = e.dataItem;

    var visitId = $("#data").attr("data-visitId");
        $.ajax({
        url: '/Visit/RemoveDiseaseFromVisit',
    type: "Post",
    dataType: "JSON",
            data: {diseaseId: dataItem.Guid, visitId: visitId },
});

}



    function onSymptomSelectSelect(e) {

        var dataItem = e.dataItem;

    var visitId = $("#data").attr("data-visitId");
        $.ajax({
        url: '/Visit/AddSymptomToVisit',
    type: "Post",
    dataType: "JSON",
            data: {symptomId: dataItem.value, visitId: visitId },
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
            data: {diseaseId: itemList, visitId: visitId },
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



    function onSymptomSelectDeselect(e) {

        var dataItem = e.dataItem;

    var visitId = $("#data").attr("data-visitId");
        $.ajax({
        url: '/Visit/RemoveSymptomFromVisit',
    type: "Post",
    dataType: "JSON",
            data: {symptomId: dataItem.value, visitId: visitId },
});

}


    function GetDiseaseIdForMedicines() {
        var Id = $("#btn-MedicineDisease-accept").attr('data-diseaseId')

        return {
        Id: Id
};
}


    $('#btn-MedicineDisease-accept').on("click", function () {

        var entityGrid = $("#KendoMedicineDiseaseGrid").data("kendoGrid");
        //selectedItem = entityGrid.dataItem(entityGrid.select());
    var allmed = [];
    var selectedItem = entityGrid.select();
        for (let i = 0; i < selectedItem.length; i++) {
        let medDi = entityGrid.dataItem(selectedItem[i]);
    allmed.push(medDi.MedicineId)
}


        if (selectedItem === null || selectedItem.length === 0) {
        bootbox.alert({
            message: "Please Select One Or More Medicines!",
        });
    return
}
var visitId = $("#data").attr("data-visitId");
        $.ajax({
        url: '/Visit/AddMedicineDiseaseToVisit',
    type: "Post",
    dataType: "JSON",
            data: {medicineDisease: allmed, visitId: visitId },
            success: function (eventList) {
                if (eventList !== 0) {

        $('#MedicineDisease-Modal').modal('toggle');
    $("#PrescriptionDetailGrid").find(".k-i-refresh").click();
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

var test;

    $('#btn-test-accept').on("click", function () {
        $('.emptybox').addClass('hidden');
    var isEmmpty = true;
        $('.emptybox').each(function () {
            if ($(this).attr('data-isEssential') === 'true') {
                var empty = $(this).attr('id');
                if ($('[data-checkEmpty="' + empty + '"]').val() === "") {
        $(this).removeClass('hidden');
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
var TestId;
if(test)
    TestId = $("#Test").attr('data-Value');
else
    TestId = $("#Job").attr('data-Value');
        if (!IfUserCheckPass(GridRefreshLink)) {
            return;
}
        if (!CheckIfPriceBiggerThanDiscount(GridRefreshLink)) {
            return;
}

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


                        var BaseInfoId = response;
    //var BaseInfoTypeId = $("#type-id").attr("data-Value");
                        $.ajax({
        type: "Post",
                            data: {BaseInfoId: BaseInfoId, BaseInfoTypeId: TestId },
    url: "/BaseInfo/AddBaseInfoType"
});



$('#my-modalTest-new').modal('hide');
$(".modal-backdrop:last").remove();
$('#new-modalTest-body').empty();
$(".loader").fadeIn("slow");
$(".loader").addClass("hidden");

}
}
}
});
});


    function closeModalTestInVisit() {
        $('#my-modalTest-new').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#new-modalTest-body').empty();
}


    function closeMedicineDiseaseModalInVisit() {
        $('#MedicineDisease-Modal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#MedicineDisease-Modal-body').empty();
}


    function closeVisitListModalInVisit() {
        $('#VisitList-Modal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#VisitList-Modal-body').empty();
}



    function closeDiseaseModalInVisit() {
        $('#AddDiseaseModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#AddDiseaseModal-body').empty();
}


    function closeDiseaseModalInVisit() {
        $('#AddDiseaseModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#AddDiseaseModal-body').empty();
}


    $('.HT .k-link-increase').on("click", function (e) {

        e.preventDefault();
    let height = $(this).closest('tr').prev().find('.nearHt').text();
    let numericHeight = $("#HT").data("kendoNumericTextBox");
    let val = numericHeight.value();
    //let val = $('.HT').val();
        if (height !== "") {
            if (val === 1)
    numericHeight.value(parseInt(height) + 1);
}

    //$('#HT').val(parseInt(height) + 1);
//$(this).val(parseInt(height));

});

    $('.HT .k-link-decrease').on("click", function (e) {

        e.preventDefault();
    let height = $(this).closest('tr').prev().find('.nearHt').text();
    let numericHeight = $("#HT").data("kendoNumericTextBox");
    let val = numericHeight.value();
        if (height !== "") {
            if (val === 0)
     numericHeight.value(parseInt(height) - 1);
}

//$('#HT').val(parseInt(height)-1)
//$(this).val(parseInt(height));

});


    $('.WT .k-link-increase').on("click", function (e) {

        e.preventDefault();
    let height = $(this).closest('tr').prev().find('.nearWt').text();
    let numericWeight = $("#WT").data("kendoNumericTextBox");
    let val = numericWeight.value();
        if (height !== "") {
            if (val === 0.1)
        numericWeight.value(parseInt(height) + 0.1);
}

//$('.WT').val(parseInt(height)+1)
//$(this).val(parseInt(height));

});

    $('.WT .k-link-decrease').on("click", function (e) {

        e.preventDefault();
    let height = $(this).closest('tr').prev().find('.nearWt').text();
    let numericWeight = $("#WT").data("kendoNumericTextBox");
    let val = numericWeight.value();
        if (height !== "") {
            if (val === 0)
    numericWeight.value(parseInt(height)-0.1);
}

//$('.WT').val(parseInt(height)-1)
//$(this).val(parseInt(height));

});


    $('.HC .k-link-increase').on("click", function (e) {

        e.preventDefault();
    let height = $(this).closest('tr').prev().find('.nearHc').text();
    let numericHC = $("#HC").data("kendoNumericTextBox");
    let val = numericHC.value();
        if (height !== "") {
            if (val === 1)
    numericHC.value(parseInt(height)+1);
}

//$('.HC').val(parseInt(height)+1)
//$(this).val(parseInt(height));

});

    $('.HC .k-link-decrease').on("click", function (e) {

        e.preventDefault();
    let height = $(this).closest('tr').prev().find('.nearHc').text();
    let numericHC = $("#HC").data("kendoNumericTextBox");
    let val = numericHC.value();
        if (height !== "") {
            if (val === 0)
    numericHC.value(parseInt(height)-1);
}

//$('.HC').val(parseInt(height)-1)
//$(this).val(parseInt(height));

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

        if (value == current || value < 1) {
            return
}

var link = "/Visit/ChangeVisit";

var visitNum;
var status = 0;
        switch (value) {
            case "Next":
                if (current == all) {
        bootbox.alert("!This is a Last Patient");
    return
}

visitNum = current + 1;
break;
case "Previous":
                if (current == 1) {
        bootbox.alert("!This is a First Patient");
    return
}

visitNum = current - 1;
break;
case "Visited":
                if (current == all) {

        $.ajax({
            url: '/Visit/EventChangeStatus',
            type: "Post",
            data: { current: current }


        });
    bootbox.alert("!This is a Last Patient");
    return
}
visitNum = current + 1;
status = current;
break;
default:
visitNum = value;
}
$(".loader").removeClass("hidden");

        $("#visit-content").load(link + "?visitNumber=" + visitNum + "&current=" + status, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});

}


    function visitRefresh() {
        var link = "/Visit/Form";

    $(".loader").removeClass("hidden");

        $("#visit-content").load(link, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});
}




    function AddMedicineInVisit() {

        var link = "/Medicine/AddNewModal";
    $(".loader").removeClass("hidden");
    $('#AddMedicineInVisitModal').modal('toggle');
        $('#AddMedicineInVisitModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});

      
    }


    function addNewMedicineInVisitHide() {

        $('#AddMedicineInVisitModal').modal('toggle');
    $('#AddMedicineInVisitModal-body').empty();


}



    function addNewMedicineInVisit() {

        $('.emptybox').addClass('hidden');
    var isEmmpty = true;
    $('.emptybox').each(function () {
        if ($(this).attr('data-isEssential') === 'true') {
            var empty = $(this).attr('id');
            if ($('[data-checkEmpty="' + empty + '"]').val() === "") {
        $(this).removeClass('hidden');
    isEmmpty = false;
    return;
}
}
});


//var ff = $("#ClinicSections").val();
//if ($("#ClinicSections").val())
    if (isEmmpty === false) {
        return;
}


    var link = "/Medicine/AddOrUpdate";
    var GridRefreshLink = "/Medicine/RefreshGrid"
    var data = $("#addNewForm").serialize();


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

        IfDisease(response, GridRefreshLink);
    $('#AddMedicineModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#AddMedicineModal-body').empty();
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");

}
}
}
});
}

    function PrescriptionDetailGridSave() {

        grid.saveChanges();
    $("#PrescriptionDetailGrid").find(".k-i-refresh").click();

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




    function AddTest() {
        test = true;
    var link = "/BaseInfo/AddNewModal";
    $(".loader").removeClass("hidden");
    $('#my-modalTest-new').modal('toggle');
        $('#new-modalTest-body').load(link, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});


}



    function AddJob() {
        test = false;
    var link = "/BaseInfo/AddNewModal";
    $(".loader").removeClass("hidden");
    $('#my-modalTest-new').modal('toggle');
        $('#new-modalTest-body').load(link, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});


}



    function PrescriptionTestDetailGridSave() {

        Testgrid.saveChanges();
    $("#PrescriptionTestDetailGrid").find(".k-i-refresh").click();
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

        $("#DiseaseHistory").find(".k-i-refresh").click();
    $("#AllergicDiseaseHistory").find(".k-i-refresh").click();
    $("#SocialDiseaseHistory").find(".k-i-refresh").click();

    $('#PatientRecordModal').modal('hide');

    $('#PatientRecordModal-body').empty();

}




    function NumKeypress(e) {

        //let curCell = $("#PrescriptionDetailGrid").find(".k-edit-cell");

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



//grid.closeCell(curCell);
//grid.editCell(curCell.next());
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


    function DeletePrescriptionDetail(element) {

        bootbox.confirm('@ViewBag.DeleteBody', function (result) {

            if (!result) {

                return;

            }
            else {

                let cellList = $("#PrescriptionDetailGrid").find("td:eq(0)");
                for (let i = 0; i < cellList.length; i++) {
                    if ($(cellList[i]).html() == 0) {
                        $("#PrescriptionDetailGrid").find(".k-i-refresh").click();
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
                        $("#PrescriptionDetailGrid").find(".k-i-refresh").click();
                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");
                    }

                });
            }
        })

    }



    function DeletePrescriptionTestDetail(element) {

        bootbox.confirm('@ViewBag.DeleteBody', function (result) {

            if (!result) {

                return;

            }
            else {

                let cellList = $("#PrescriptionTestDetailGrid").find("td:eq(0)");
                for (let i = 0; i < cellList.length; i++) {
                    if ($(cellList[i]).html() == 0) {
                        $("#PrescriptionTestDetailGrid").find(".k-i-refresh").click();
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
                        $("#PrescriptionTestDetailGrid").find(".k-i-refresh").click();
                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");
                    }

                });
            }
        })

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


    function TestReport() {

        let all = $(".testCheckboxes");
    var Note = "";

    $(".loader").removeClass("hidden");
    $('#ReportsModal').modal('toggle');
    var visitId = '@ViewBag.VisitId';
        for (let i = 0; i < all.length; i++) {
            if($(all[i]).prop('checked'))
    Note = Note + $(all[i]).next().text() + ",";
}
Note = Note + $('#TestDescription').val();
Note.replace(" ", "%20");
link = "/Visit/NewVisitPrescriptionTestReport?visitId=";
        $('#ReportsModal-body').load(link + visitId, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});

}

    function MedicineReport() {


        $(".loader").removeClass("hidden");
    $('#ReportsModal').modal('toggle');
    var visitId = '@ViewBag.VisitId';
    link = "/Visit/VisitPrescriptionWithmedicinesReport?visitId=";
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
//var birthDay = today.getFullYear() - age;
var birthDay = today;
birthDay.setFullYear(birthDay.getFullYear() - Year);
birthDay.setMonth(birthDay.getMonth() - Month);
let birthMonth = birthDay.getMonth() + 1;
// $('#DateOfBirth').val('1/1/' + birthDay + '');
$('#DateOfBirth').val('1/'+birthMonth+'/'+birthDay.getFullYear());

}


    function AddDisease() {

        var link = "/Disease/AddNewModal";
    $(".loader").removeClass("hidden");
    $('#AddDiseaseModal').modal('toggle');
        $('#AddDiseaseModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});

}

    function addNewDisease() {

        $('.emptybox').addClass('hidden');
    var isEmmpty = true;
    $('.emptybox').each(function () {
        if ($(this).attr('data-isEssential') === 'true') {
            var empty = $(this).attr('id');
            if ($('[data-checkEmpty="' + empty + '"]').val() === "") {
        $(this).removeClass('hidden');
    isEmmpty = false;
    return;
}
}
});



    if (isEmmpty === false) {
        return;
}


    var link = "/Disease/AddOrUpdate";
    var GridRefreshLink = "/Disease/RefreshGrid";
    var data = $("#addNewDiseaseForm").serialize();


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

        IfDisease(response, GridRefreshLink);
    $('#AddDiseaseModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#AddDiseaseModal-body').empty();
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
                        $.ajax({
        type: "Post",
    url: "/Disease/GetAllDisease",
                            success: function (response) {
        optional.setDataSource(response);
}
})


}
}
}
});
}



    function SendToPharmacy() {
        bootbox.alert({
            message: "Prescription Sent to Pharmacy",
        });
}


    function MedicinePrint() {


        $(".loader").removeClass("hidden");
        var visitId = $("#data").attr("data-visitId");
        $.ajax({
        url: '/Visit/VisitPrescriptionWithmedicinesPrint',
        type: "Post",
            data: {visitId: visitId },
            success: function (response) {

        draw2(response);
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    },
            error: function (response) {
        
            }
        });
    }



    //function draw2(imgData) {
    //    "use strict";
   
    //var ctx = canvas.getContext("2d");

    //var uInt8Array = imgData.all;
    //let width = imgData.width;
    //let height = imgData.height;
    //var i = uInt8Array.length;
    //var binaryString = [i];
    //    while (i--) {
    //    binaryString[i] = String.fromCharCode(uInt8Array[i]);
    //     }
    //var data = binaryString.join('');

    //var base64 = window.btoa(data);
    //canvas.width = width;
    //    canvas.height = height;
    //var img = new Image();
    //img.src = "data:image/jpeg;base64," + base64;
    //        img.onload = function () {
    //    ctx.drawImage(img, 0, 0, width, height);
    //    ctx.rotate(90 * Math.PI / 180);
    //    var dataUrl = canvas.toDataURL(); //attempt to save base64 string to server using this var
    //    printJS(dataUrl, 'image');
            

    //    };
      

    //}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


