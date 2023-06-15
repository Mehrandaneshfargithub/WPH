

function ValidateEmail(email) {
    if (email == '')
        return true;

    return email.match(
        /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
};


function OnlyNumberKey(evt) {

    // Only ASCII character in that range allowed
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode == 13)
        return true;
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
    return true;
}


function GridServiceUpPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Up";
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/InternalServices/ServicePriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function GridServiceDownPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Down";
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/InternalServices/ServicePriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function GridMedicineUpPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Up";
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/Medicine/MedicinePriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function GridMedicineDownPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Down";
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/Medicine/MedicinePriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function GridAnalysisItemUpPriorityFunction(element) {


    var priority = parseInt($(element).attr('data-Priority'));
    if (priority !== 1) {
        var Id = $(element).attr('data-id');
        var Type = "Up";
        //var AnalysisId = $('#analysisid').attr('data-Value');
        $.ajax({
            type: "Post",
            data: { id: Id, type: Type },
            url: "/AnalysisItem/AnalysisItemPriorityEdit",
            success: function (response) {
                $(".k-pager-refresh")[0].click();
            }
        });
    }

}

function GridAnalysisItemDownPriorityFunction(element) {

    var priority = parseInt($(element).attr('data-Priority'));

    var Id = $(element).attr('data-id');

    var Type = "Down";
    //var AnalysisId = $('#analysisid').attr('data-Value');
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/AnalysisItem/AnalysisItemPriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}



function GridAnalysisUpPriorityFunction(element) {


    var priority = parseInt($(element).attr('data-Priority'));
    if (priority !== 1) {
        var Id = $(element).attr('data-id');
        var Type = "Up";
        //var AnalysisId = $('#analysisid').attr('data-Value');
        $.ajax({
            type: "Post",
            data: { id: Id, type: Type },
            url: "/Analysis/AnalysisPriorityEdit",
            success: function (response) {
                $(".k-pager-refresh")[0].click();
            }
        });
    }

}

function GridAnalysisDownPriorityFunction(element) {

    var priority = parseInt($(element).attr('data-Priority'));

    var Id = $(element).attr('data-id');

    var Type = "Down";
    //var AnalysisId = $('#analysisid').attr('data-Value');
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/Analysis/AnalysisPriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}



function GridGroupAnalysisItemUpPriorityFunction(element) {
    var priority = parseInt($(element).attr('data-Priority'));
    if (priority !== 1) {
        var Id = $(element).attr('data-id');
        var Type = "Up";
        //var GroupAnalysisId = $('#groupanalysisid').attr('data-Value');
        $.ajax({
            type: "Post",
            data: { id: Id, type: Type },
            url: "/GroupAnalysisItem/GroupAnalysisItemPriorityEdit",
            success: function (response) {
                $(".k-pager-refresh")[1].click();
            }
        });
    }

}

function GridGroupAnalysisItemDownPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Down";
    //var GroupAnalysisId = $('#groupanalysisid').attr('data-Value');
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/GroupAnalysisItem/GroupAnalysisItemPriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[1].click();
        }
    });
}

function GridGroupAnalysisUpPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Up";
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/GroupAnalysis/GroupAnalysisPriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function GridGroupAnalysisDownPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Down";
    $.ajax({
        type: "Post",
        data: { id: Id, type: Type },
        url: "/GroupAnalysis/GroupAnalysisPriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function GridGroupAnalysis_AnalysisUpPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Up";
    var GroupAnalysisId = $('#groupanalysisid').attr('data-Value');
    $.ajax({
        type: "Post",
        data: { id: Id, groupAnalysisId: GroupAnalysisId, type: Type },
        url: "/GroupAnalysis_Analysis/GroupAnalysis_AnalysisPriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function GridGroupAnalysis_AnalysisDownPriorityFunction(element) {
    var Id = $(element).attr('data-id');
    var Type = "Down";
    var GroupAnalysisId = $('#groupanalysisid').attr('data-Value');
    $.ajax({
        type: "Post",
        data: { id: Id, groupAnalysisId: GroupAnalysisId, type: Type },
        url: "/GroupAnalysis_Analysis/GroupAnalysis_AnalysisPriorityEdit",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function ActiveDeactiveUserFunction(element) {
    var Id = $(element).attr('data-id');
    $.ajax({
        type: "Post",
        data: { id: Id },
        url: "/UserManagment/ActiveDeActiveUser",
        success: function (response) {
            $(".k-pager-refresh")[0].click();
        }
    });
}

function GridAddFunction(element) {
    var link = $("#GridAddLink").attr("data-Value");
    $(".loader").removeClass("hidden");
    $('#my-modal-new').modal('toggle');
    $('#new-modal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        kendo.ui.progress($("#window"), true);
    });
}

function GridPatientDiseaseRecordFunction(element) {

    $(".loader").removeClass("hidden");
    $("#PatientRecordModal").modal('toggle');
    var patientid = $(element).attr('data-id');
    $.ajax({
        type: "Post",
        data: { patientid: patientid },
        url: "/Patient/PatientRecordForm",
        dataType: "html",
        success: function (data) {
            $("#PatientRecordModal").modal();
            $("#PatientRecordModal-body").html(data);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        },
    });
}


function GridPatientVariablesFunction(element) {

    $(".loader").removeClass("hidden");
    $("#my-modal-PatientVariables").modal('toggle');
    var patientid = $(element).attr('data-id');
    $("#PatientVariables-modal-header").text($(element).attr('data-name'));

    $("#btn-PatientVariables-accept").attr('data-patientid', patientid);

    var link = "/Patient/PatientVariableModal?patientId=";
    $('#PatientVariables-modal-body').load(link + patientid, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });
}


function PatientVariableNewModalClose() {

    $('#PatientVariableNewModal').modal('hide');
    $('#PatientVariableNewModal-body').empty();
    $(".modal-backdrop:last").remove();

}

function GetPatientIdForPatientVariable() {
    var Id = $("#btn-PatientVariables-accept").attr('data-patientId')

    return {
        Id: Id
    };
}


function GridDeleteFunction(element) {
    $("#ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#InvoiceInUse").addClass("hidden");
    $("#ERROR_SomeThingWentWrong").addClass("hidden");

    $(".loader").removeClass("hidden");
    $('#my-modal-delete').modal('toggle');
    var Id = $(element).attr('data-id');
    $('#btn-delete-accept').attr('data-id', Id);
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}


function GridDeleteFunctionWithPass(element) {
    $("#ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#InvoiceInUse").addClass("hidden");
    $("#ERROR_SomeThingWentWrong").addClass("hidden");
    $("#Wrong_Pass").addClass("hidden");
    $("#DeletePassword-box").addClass("hidden");

    $(".loader").removeClass("hidden");
    $('#my-modal-withPass-delete').modal('toggle');
    var Id = $(element).attr('data-id');

    $('#DeletePassword').removeAttr("disabled");
    $("#my-modal-withPass-delete #DeletePassword").val('');

    $('#btn-delete-withPass-accept').attr('data-id', Id);
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}



function GridEditFunction(element) {

    $('#my-modal-edit').modal('toggle');
    var link = $("#GridEditLink").attr("data-Value");
    var Id = $(element).attr('data-id');
    $(".loader").removeClass("hidden");
    $('#edit-modal-body').load(link + Id + '', function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        kendo.ui.progress($("#window"), true);
    });

}

function GridUserAccessFunction(element) {
    $('#my-modal-Access').modal('toggle');
    var link = $("#AccessLink").attr("data-Value");
    var Id = $(element).attr('data-id');
    var name = $(element).attr('data-name');
    let access = $('#UserAccesText').text();

    //access = access + " - " + name;
    $('#Access-modal-header').text(access + " - " + name);
    $(".loader").removeClass("hidden");
    $('#Access-modal-body').load(link + Id + '', function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });
}

function IfReserve(response, GridRefreshLink) {



    if (GridRefreshLink === "/Reserves/RefreshGrid") {


        calendar1.refetchEvents();

    }

}


function IfUserCheckPass(GridRefreshLink) {
    if (GridRefreshLink === "/UserManagment/RefreshGrid") {
        var Pass3 = $.trim($('#Pass3').val());
        var Pass4 = $.trim($('#Pass4').val());
        if (Pass3 !== Pass4) {
            $('#incorrectPass').removeClass('hidden');
            return false;
        }
    }
    return true;
}



//function IfBaseInfoAddBaseInfoType(response, GridRefreshLink) {
//    if (GridRefreshLink === "/BaseInfo/RefreshGrid") {
//        var BaseInfoId = response;
//        var BaseInfoTypeId = $("#type-id").attr("data-Value");
//        $.ajax({
//            type: "Post",
//            data: { BaseInfoId: BaseInfoId, BaseInfoTypeId: BaseInfoTypeId },
//            url: "/BaseInfo/AddBaseInfoType"
//        });
//    }
//}

function IfAnalysisItemAddAnalysis(response) {
    var AnalysisItemId = response;
    var AnalysisId = $("#analysisid").attr("data-Value");
    $.ajax({
        type: "Post",
        data: { AnalysisItemId: AnalysisItemId, AnalysisId: AnalysisId },
        url: "/AnalysisItem/AddAnalysisOfAnalysisItem"
    });
}

function IfUser(response, GridRefreshLink) {
    if (GridRefreshLink === "/UserManagment/RefreshGrid") {
        if (ClinicSectionsMulti !== undefined) {
            var ClinicSections = [];
            itemList = "";
            var UserId = response;
            var itemList = $('#ClinicSections').data('kendoMultiSelect').dataItems();
            for (var i = 0; i < itemList.length; i++) {
                ClinicSections.push(itemList[i]["Guid"]);
            }

            itemList = ClinicSections.toString();
            $.ajax({
                type: "Post",
                data: { itemList: itemList, UserId: UserId },
                url: "/UserManagment/UserClinicSections"
            });
        }
        else {

            var itemList = "";
            var UserId = response;
            $.ajax({
                type: "Post",
                data: { itemList: itemList, UserId: UserId },
                url: "/UserManagment/UserClinicSections"
            });
        }


        //var favorite = [];
        //var itemList = "";
        //var UserId = response;
        //$.each($("input[name='ClinicSection']:checked"), function () {
        //    favorite.push($(this).val());
        //});
        //itemList = favorite.toString();
        //$.ajax({
        //    type: "Post",
        //    data: { itemList: itemList, UserId: UserId },
        //    url: "/UserManagment/UserClinicSection"
        //});
    }
}


function IfDisease(response, GridRefreshLink) {

    if (GridRefreshLink === "/Disease/RefreshGrid") {
        var Medicine = [];
        var Symptoms = [];
        itemList = "";
        itemList2 = "";
        var DiseaseId = response;
        var itemList = $('#allMedsForDisease').data('kendoListBox').dataItems();
        var itemList2 = $('#allSymptomsForDisease').data('kendoListBox').dataItems();
        for (var i = 0; i < itemList.length; i++) {
            Medicine.push(itemList[i]["Guid"]);
        }
        for (i = 0; i < itemList2.length; i++) {
            Symptoms.push(itemList2[i]["Guid"]);
        }
        itemList = Medicine.toString();
        itemList2 = Symptoms.toString();
        $.ajax({
            type: "Post",
            data: { itemList: itemList, DiseaseId: DiseaseId },
            url: "/Disease/MedicineForDisease"
        });
        $.ajax({
            type: "Post",
            data: { itemList: itemList2, DiseaseId: DiseaseId },
            url: "/Disease/SymptomForDisease"
        });
    }
}

function IfDiseaseAddSymptomsForDisease(response, GridRefreshLink) {
    if (GridRefreshLink === "/Disease/RefreshGrid") {
        var Symptom = [];
        itemList = "";
        var DiseaseId = response;

        $.each($(".Symptoms option:selected"), function () {
            Symptom.push($(this).val());
        });

        //Id = $.trim($('#Guid').val());
        itemList = Symptom.toString();
        //DiseaseId = Id.toString();
        $.ajax({
            type: "Post",
            data: { itemList: itemList, DiseaseId: DiseaseId },
            url: "/Disease/SymptomForDisease"
        });
    }
}


function IfVisit() {
    let test = $("#Test").attr("data-Value");
    if (test !== undefined) {
        GetAllMedicines();
        //autocomplete = $("#MedicineJoineryName").data("kendoAutoComplete");
        autocomplete.setDataSource(medicines);
    }
}


function GetBaseInfoType() {
    var Id = $('#type-id').attr('data-Value');
    return {
        Id: Id
    };
}

function GetAnalysisId() {
    var Id = $('#btn-AddAnalysisItemToAnalysisModal-close').attr('data-analysisId');
    return {
        AnalysisId: Id
    };
}


function GetGroupAnalysisIdInGroupAnalysisItemPage() {
    var Id = $('#btn-GroupAnalysisItemModal-accept').attr('data-GroupId');
    return {
        GroupId: Id
    };
}

function GetGroupAnalysisId() {
    var Id = $('#btn-GroupAnalysisItemModal-accept').attr('data-GroupId');
    return {
        GroupAnalysisId: Id
    };
}


function SaveSettings(settingName, settingValue) {
    $.ajax({
        type: "Post",
        url: "/InterfaceSettings/SaveSetting",
        data: '{ settingName :"' + settingName + '" ,settingValue:"' + settingValue + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
        }
    });
}

function loadSettings() {
    try {
        var rtl = $("#RtlSetting").data('setting');
        var compact = $("#VerticalSideBarSetting").data('setting');
        var twosidebar = $("#TwoSideBarSetting").data('setting');
        if (rtl === "True") {
            $('#ace-settings-rtl').prop('checked', true);
        }
        if (rtl !== "True") {
            $('body').removeClass('rtl');
        }
        if (compact === "compactSideBar") {
            $('#ace-settings-compact').prop('checked', true);
        }
    } catch (error) {
        console.error(error);
    }
}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode !== 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}


function CheckIfPriceBiggerThanDiscount() {
    if (GridRefreshLink === "/InternalServices/RefreshGrid") {
        var price = $("#Price").val();
        var discount = $("#Discount").val();
        if (Number(price) > Number(discount)) {
            $("#Price-Bigger").removeClass('hidden');
            return false;
        }
    }
    return true;
}

function PatientSelect(e) {

    var DataItem = this.dataItem(e.item.index());
    var patientId = DataItem.Guid;
    $("#patient_Guid").val(patientId);
    $("#Age").val(DataItem.Age);

    if (DataItem.GenderId === 15) {
        $("#UserGenderName").val("Man");
    }
    else {
        $("#UserGenderName").val("Woman");
    }

    $(".loader").removeClass("hidden");
    $('#RefreshPatientDiseases').load('/Patient/GetAllDiseaseRecordForPatient?PatientId=' + patientId, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });

}



function OnDoctorSelect(e) {

    var DataItem = this.dataItem(e.item.index());

    let dropdownlist = $("#Speciality").data("kendoDropDownList");

    dropdownlist.value(DataItem.SpecialityId);
    var set_name;
    try {
        var set_name = DataItem.User.Name;
    } catch {
        var set_name = DataItem.UserName;
    }

    $("#Doctor").val(set_name);
    $("#DoctorId").val(DataItem.Guid);

    setTimeout(function () {
        $("#Doctor").val(set_name);
    }, 20);

}



//function checkLastPatientVisit(Id) {

//    $.ajax({
//        type: "Post",
//        data: { patientId: Id },
//        url: "/Reserves/GetLastPatientVisit",
//        success: function (response) {

//            let last = $("#patientLastVisitDateIs").text();
//            let getMoney = $("#doYouWantGetMoney").text();
//            let Yse = $("#Yes").text();
//            let No = $("#No").text();
//            if (response.result) {
//                bootbox.confirm({
//                    message: last + ":" + " " + response.time + "  " + getMoney,
//                    className: 'bootbox-class MyFont-Sarchia-grid',
//                    //buttons: {
//                    //    confirm: {
//                    //        label: Yse,
//                    //        className: 'k-primary k-button'
//                    //    },
//                    //    cancel: {
//                    //        label: No,
//                    //        className: 'k-button'
//                    //    }
//                    //},
//                    callback: function (result) {

//                        if (!result) {

//                            $("#LastVisit").val(true);

//                        }
//                        else {
//                            $("#LastVisit").val(false);
//                        }
//                    }
//                })
//            }

//        }
//    });
//}



function OnPatientSelectInReception(e) {

    var DataItem = this.dataItem(e.item.index());
    var id = this.element.attr('id');

    if (id === "AddressName") {
        $("#AddressId").val(DataItem.Guid);
        return
    }


    $("#DateOfBirth").val(DataItem.DateOfBirth);
    $("#PhoneNumber").val(DataItem.PhoneNumber);
    $("#AddressName").val(DataItem.AddressName);
    $("#AddressId").val(DataItem.AddressId);
    $("#PatientId").val(DataItem.Guid);

    var dropdownlist = $("#GenderId").data("kendoDropDownList");


    dropdownlist.value(DataItem.GenderId);

    $("#DateOfBirth").focus();

    setTimeout(function () {
        $("#Name2").val(DataItem.Name);
        $("#Doctor").focus();
        //setTimeout(function () {
        //    let value = doctorAuto.value();
        //    doctorAuto.search(value);
        //}, 30);

    }, 20);


}






function onSelectAnalysisForGroupAnalysisPage(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Guid;
        $('#analysisid').attr('data-Value', Id);
    }
}

function onSelectAnalysis(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Guid;
        $('#analysisid').attr('data-Value', Id);
        $(".k-pager-refresh")[0].click();
    }
}

function onSelectAnalysisFromGroupAnalysisItem(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Guid;
        $('#analysisidForAnalysisItem').attr('data-Value', Id);
    }
}


function onChangeAnalysisFromGroupAnalysisItem(e) {
    _analysisidForAnalysisItem = $('#analysisidForAnalysisItem').attr('data-Value');
    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    $.ajax({
        type: "Post",
        url: '/AnalysisItem/GetAllAnalysisItemWithoutInGroupAnalysisItem',
        data: {
            __RequestVerificationToken: token.attr('value'),
            AnalysisId: _analysisidForAnalysisItem
        },
        success: function (response) {
            let listbox = $("#analysisItem").data("kendoListBox");
            listbox.setDataSource(response);
        }
    });
}

function onSelectAnalysisItemFromGroupAnalysisItem(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Guid;
        $('#analysisitemid').attr('data-Value', Id);
    }
}

function onDataBoundAnalysisItemFromGroupAnalysisItem(e) {
    $("#analysisItem").data("kendoDropDownList").select(0);
    $('#analysisitemid').attr('data-Value', $("#analysisItem").val());
}

function onSelectGroupAnalysis(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Guid;
        $('#groupanalysisid').attr('data-Value', Id);
        $(".k-pager-refresh")[0].click();
        $(".k-pager-refresh")[1].click();
    }
}

function onDataBoundGroupAnalysis(e) {
    $("#groupanalysis").data("kendoDropDownList").select(0);
    groupAnalysisId = $("#groupanalysis").val();
    if (groupAnalysisId == '') {
        PleaseInsertGroupAnalysis();
    }
}

function onSelectBaseInfoType(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Guid;
        $('#type-id').attr('data-Value', Id);
        $(".k-pager-refresh")[0].click();
    }
}


function changeBirthOfDatePatientDatetimePicker() {

    var val = this.value();
    if (val == '' || val == null)
        val = this._oldText;

    var age = getAge(kendo.toString(val, 'd'));


    // $('#Age').val(age);
}

function getAge(dateString) {

    let wrong = false;
    var today = new Date();
    var birthDate = new Date(dateString);

    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
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
    $('#Year').val(age);
    $('#Month').val(m);
    $('#DateOfBirthYear').val(birthDate.getFullYear());
    $('#DateOfBirthMonth').val(mon);
    $('#DateOfBirthDay').val(birthDate.getDate());

    //return age;
}






function changeBirthOfDatePatientDatetimePickerNew() {

    var age = getAgeNew(kendo.toString(this.value(), 'd'));


    // $('#Age').val(age);
}

function getAgeNew(dateString) {

    let date = $("#DateOfBirth").data("kendoDatePicker");
    var birthDate = date.value();
    var today = new Date(dateString);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
        m = 12 + m;
    }

    age = "year: " + age;
    m = "month: " + m;
    let mon = birthDate.getMonth() + 1;
    $('#Year1').val(age);
    $('#Month1').val(m);
    //$('#DateOfBirthYear').val(birthDate.getFullYear());
    //$('#DateOfBirthMonth').val(mon);
    //$('#DateOfBirthDay').val(birthDate.getDate());
    //return age;
}


function changeBirthOfDatePatientDatetimePickerForPatientVariables() {

    var age = getAgeForPatientVariables(kendo.toString(this.value(), 'd'));


    // $('#Age').val(age);
}

function getAgeForPatientVariables(dateString) {

    var today = new Date();
    var birthDate = new Date(dateString);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
        m = 12 + m;
    }


    let mon = birthDate.getMonth() + 1;
    $('#Year1').val(age);
    $('#Month1').val(m);
    //$('#DateOfBirthYear').val(birthDate.getFullYear());
    //$('#DateOfBirthMonth').val(mon);
    //$('#DateOfBirthDay').val(birthDate.getDate());
    //return age;
    DateForNewPatientVariableChange();
}





function VisitVariableJobChange(e) {

    var DataItem = this.dataItem(e.item.index());

    var property = this.element.attr("data-property");
    var value = DataItem.Guid;
    var visitId = $('#data').attr('data-visitId');
    var patientId = $('#data').attr('data-Guid');
    var PatientVariableValueGuid = this.element.attr('data-patientvariablevalueGuid');

    //$(".loader").removeClass("hidden");
    $.ajax(
        {
            type: "Post",
            url: "/Visit/UpdateVisitVariables",
            data: { Guid: PatientVariableValueGuid, Property: property, VisitId: visitId, Value: value, PatientId: patientId },
            success: function () {
                //$(".loader").fadeIn("slow");
                //$(".loader").addClass("hidden");
            }
        });

}
function VisitVariableChange() {


    var property = this.element.attr("data-property");
    var value = this.value();
    var visitId = $('#data').attr('data-visitId');
    var patientId = $('#data').attr('data-Guid');
    var PatientVariableValueGuid = this.element.attr('data-patientvariablevalueGuid');
    var Reassesst = this.element.attr('data-reassesst');

    var id = this.element.attr('id');

    if (Reassesst == 'reassesstDate') {
        var dat = this.value();
        let Month = dat.getMonth() + 1;
        let Day = dat.getDate();

        if (Month < 10)
            Month = "0" + Month;

        if (Day < 10)
            Day = "0" + Day;


        value = Day + '/' + Month + '/' + dat.getFullYear();
    }

    if (value === "" && PatientVariableValueGuid === "")
        return;

    if (property === "BirthDay") {


        var Year = parseInt($('#Year').val());
        var Month = parseInt($('#Month').val());
        if ($('#Month').val() === "") {
            Month = 0;
        }
        if ($('#Year').val() === "") {
            Year = 0;
        }
        var today = new Date();
        //var birthDay = today.getFullYear() - age;
        var birthDay = today;
        birthDay.setFullYear(birthDay.getFullYear() - Year);
        birthDay.setMonth(birthDay.getMonth() - Month);
        let birthMonth = birthDay.getMonth() + 1;
        // $('#DateOfBirth').val('1/1/' + birthDay + '');
        $('#DateOfBirth').val('1/' + birthMonth + '/' + birthDay.getFullYear());


        value = birthDay.getFullYear() + '/' + birthMonth + "/1";


    }

    if (property === "BirthDayDate") {

        getAge(kendo.toString(this.value(), 'd'));

        let dat = this.value();
        let Month = dat.getMonth() + 1;
        let Day = dat.getDate();

        if (Month < 10)
            Month = "0" + Month;

        if (Day < 10)
            Day = "0" + Day;



        value = dat.getFullYear() + "/" + Month + "/" + Day;


    }



    if (property === "Gender") {

        if ($('#female').hasClass('hidden')) {

            $('#female').removeClass('hidden');
            $('#male').addClass('hidden');
        }
        else {

            $('#male').removeClass('hidden');
            $('#female').addClass('hidden');
        }
    }

    //$(".loader").removeClass("hidden");
    $.ajax(
        {
            type: "Post",
            url: "/Visit/UpdateVisitVariables",
            data: { Guid: PatientVariableValueGuid, Property: property, VisitId: visitId, Value: value, PatientId: patientId },
            success: function (Guid) {
                // $(".loader").fadeIn("slow");
                // $(".loader").addClass("hidden");
                $('#' + id).attr('data-patientvariablevalueGuid', Guid)
            }
        });
}



function VisitCheckboxVariableChange(element) {

    // $(".loader").removeClass("hidden");

    var property = $(element).attr("data-property");
    var value = $(element).prop('checked');
    //var value1 = $(element).attr('checked');
    // var value2 = $(element).prop('checked');

    var visitId = $('#data').attr('data-visitId');
    var patientId = $('#data').attr('data-Guid');
    var PatientVariableValueGuid = $(element).attr('data-patientvariablevalueGuid');

    $.ajax(
        {
            type: "Post",
            url: "/Visit/UpdateVisitVariables",
            data: { Guid: PatientVariableValueGuid, Property: property, VisitId: visitId, Value: value, PatientId: patientId },
            success: function (Guid) {
                //$(".loader").fadeIn("slow");
                //$(".loader").addClass("hidden");
                $(element).attr('data-patientvariablevalueGuid', Guid);
            }
        });
}




//function VisitConstantVariableChange() {

//    $(".loader").removeClass("hidden");
//    var property = this.element.attr("data-property");
//    var amount = this.value();
//    var visitId = $('#data').attr('data-visitId');
//    var patientId = $('#data').attr('data-Guid');

//    if (property == "BirthDay") {
//        //var age = this.value();
//        //var today = new Date();
//        //var birthDay = today.getFullYear() - age;
//        //$('#DateOfBirth').val('1/1/' + birthDay + '');
//        //id= $('#data').attr('data-Guid');
//        //amount = '1/1/' + birthDay + '';

//        var Year = parseInt($('#Year').val());
//        var Month = parseInt($('#Month').val());
//        if ($('#Month').val() === "") {
//            Month = 0;
//        }
//        if ($('#Year').val() === "") {
//            Year = 0;
//        }
//        var today = new Date();
//        //var birthDay = today.getFullYear() - age;
//        var birthDay = today;
//        birthDay.setFullYear(birthDay.getFullYear() - Year);
//        birthDay.setMonth(birthDay.getMonth() - Month);
//        let birthMonth = birthDay.getMonth() + 1;
//        // $('#DateOfBirth').val('1/1/' + birthDay + '');
//        $('#DateOfBirth').val('1/' + birthMonth + '/' + birthDay.getFullYear());


//        amount = '1/' + birthMonth + '/' + birthDay.getFullYear();


//    }


//    $.ajax(
//        {
//            type: "Post",
//            url: "/Visit/UpdateVisitConstantVariables",
//            data: { property: property, visitId: visitId, amount: amount, patientId: patientId },
//            success: function () {
//                $(".loader").fadeIn("slow");
//                $(".loader").addClass("hidden");
//            }
//        });
//}




function CostDateFilterInCostGrid(args) {

    args.element.kendoDatePicker({
        format: "dd/MM/yyyy"
    });

}



function AcceptReport() {




    var fromDate = $("#KendoFromDate").data("kendoDatePicker");
    var toDate = $("#KendoDateTo").data("kendoDatePicker");

    var frDate = fromDate.value();
    var tDate = toDate.value();
    let frMonth = frDate.getMonth() + 1;
    let frDay = frDate.getDate();
    let toMonth = tDate.getMonth() + 1;
    let toDay = tDate.getDate();

    if (frMonth < 10)
        frMonth = "0" + frMonth;
    if (toMonth < 10)
        toMonth = "0" + toMonth;
    if (frDay < 10)
        frDay = "0" + frDay;
    if (toDay < 10)
        toDay = "0" + toDay;

    let from = frDate.getFullYear() + "-" + frMonth + "-" + frDay;
    let too = tDate.getFullYear() + "-" + toMonth + "-" + toDay;

    $("#FromDate").val(from);
    $("#ToDate").val(too);
    $(".loader").removeClass("hidden");

    setTimeout(function () {

        let from = $("#FromDate").val();
        let to = $("#ToDate").val();

        var data = "fromDate=" + from + "&toDate=" + to;
        link = "/CostReport/PrintCostReport?";

        $(".loader").removeClass("hidden");

        $.ajax({
            url: link,
            type: "Post",
            data: data,
            success: function (response) {

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

    }, 20);

}






function GetPeriodForPatentReception() {

    var periodId = $('#period-id').attr('data-Value');

    var fromDate = $("#KendoFromDate").data("kendoDatePicker");
    var toDate = $("#KendoDateTo").data("kendoDatePicker");

    var frDate = fromDate.value();
    var tDate = toDate.value();
    let frMonth = frDate.getMonth() + 1;
    let frDay = frDate.getDate();
    let toMonth = tDate.getMonth() + 1;
    let toDay = tDate.getDate();

    if (frMonth < 10)
        frMonth = "0" + frMonth;
    if (toMonth < 10)
        toMonth = "0" + toMonth;
    if (frDay < 10)
        frDay = "0" + frDay;
    if (toDay < 10)
        toDay = "0" + toDay;

    let from = frDate.getFullYear() + "-" + frMonth + "-" + frDay;
    let too = tDate.getFullYear() + "-" + toMonth + "-" + toDay;

    var period = $("#sections").data("kendoDropDownList");

    var periodValue = period.value();
    var periodText = period.text();

    let section = { Id: periodValue, Name: periodText }

    return {

        periodId: periodId,
        dateFrom: from,
        dateTo: too,
        section: section
    };
}




function GetPeriodForAnalysisResultMaster() {
    var periodId = $('#period-id').attr('data-Value');


    var fromDate = $("#KendoFromDate").data("kendoDatePicker");
    var toDate = $("#KendoDateTo").data("kendoDatePicker");

    var frDate = fromDate.value();
    var tDate = toDate.value();
    let frMonth = frDate.getMonth() + 1;
    let frDay = frDate.getDate();
    let toMonth = tDate.getMonth() + 1;
    let toDay = tDate.getDate();

    if (frMonth < 10)
        frMonth = "0" + frMonth;
    if (toMonth < 10)
        toMonth = "0" + toMonth;
    if (frDay < 10)
        frDay = "0" + frDay;
    if (toDay < 10)
        toDay = "0" + toDay;

    let from = frDate.getFullYear() + "-" + frMonth + "-" + frDay;
    let too = tDate.getFullYear() + "-" + toMonth + "-" + toDay;
    var period = $("#sections").data("kendoDropDownList");

    var periodValue = period.value();
    var periodText = period.text();

    let section = { Id: periodValue, Name: periodText }

    return {

        periodId: periodId,
        dateFrom: from,
        dateTo: too,
        section: section
    };
}





function onSelectPeriodInTotalVisit(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Id;
        $('#periodValue').val(Id);
        $(".k-pager-refresh")[0].click();
    }
}




function getUserIdForClinicSection() {
    var UserId = $("#user-id").attr("data-Value");
    return {
        UserId: UserId
    }
}


function onSelectPeriodInCost(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Id;
        $('#period-id').attr('data-Value', Id);
        $(".k-pager-refresh")[0].click();
    }
}



function onSelectCostTypeInCost(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var Id = dataItem.Guid;
        $('#cost-type-id').attr('data-Value', Id);
        $(".k-pager-refresh")[0].click();
    }
}

function GetPatientId() {

    var Id = $('#PatientIdForDisease').val();

    return {
        Id: Id
    };
}

function AddDiseaseToPatient(e) {
    $(".loader").removeClass("hidden");
    let DiseaseId = [];

    for (let i = 0; i < e.dataItems.length; i++) {
        DiseaseId.push(e.dataItems[i].Value);
    }

    var PatientId = $('#PatientIdForDisease').val();


    $.ajax(
        {
            type: "Post",
            url: "/Patient/AddDiseaseForPatient",
            data: { DiseaseId: DiseaseId, PatientId: PatientId },
            success: function (data) {

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            },
        });


}

function RemoveDiseaseFromPatient(e) {
    $(".loader").removeClass("hidden");
    let DiseaseId = [];

    for (let i = 0; i < e.dataItems.length; i++) {
        DiseaseId.push(e.dataItems[i].Value);
    }

    var PatientId = $('#PatientIdForDisease').val();


    $.ajax(
        {
            type: "Post",
            url: "/Patient/RemoveDiseaseFromPatient",
            data: { DiseaseId: DiseaseId, PatientId: PatientId },
            success: function (data) {

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            },
        });

}


function AddMedicineToPatient(e) {
    $(".loader").removeClass("hidden");
    let MedicineId = [];

    for (let i = 0; i < e.dataItems.length; i++) {
        MedicineId.push(e.dataItems[i].Value);
    }

    var PatientId = $('#PatientIdForMedicine').val();


    $.ajax(
        {
            type: "Post",
            url: "/Patient/AddMedicineForPatient",
            data: { MedicineId: MedicineId, PatientId: PatientId },
            success: function (data) {

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            },
        });


}

function RemoveMedicineFromPatient(e) {
    $(".loader").removeClass("hidden");
    let MedicineId = [];

    for (let i = 0; i < e.dataItems.length; i++) {
        MedicineId.push(e.dataItems[i].Value);
    }

    var PatientId = $('#PatientIdForMedicine').val();


    $.ajax(
        {
            type: "Post",
            url: "/Patient/RemoveMedicineFromPatient",
            data: { MedicineId: MedicineId, PatientId: PatientId },
            success: function (data) {

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            },
        });

}




function onPatientImageComplete() {

    $(".loader").removeClass("hidden");
    GetAllPatientImages();


}



function onVisitImageComplete() {

    $(".loader").removeClass("hidden");
    GetAllVisitImages();


}


function onPatientImageError(e, status) {


    alert(e.data);
    alert(e.error);



}



function changeClinicSection() {

    $(".loader").removeClass("hidden");
    $('#ChooseClinicSectionModal').modal('toggle');
    let link = "/ApplicationHandler/ChooseClinicSection";
    $('#ChooseClinicSectionModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");

    });



    //$.ajax({
    //    type: "Post",
    //    url: "/UserHandler/ChangeClinicSection",
    //    success: function (response) {

    //        if (response.result) {
    //            $(".loader").removeClass("hidden");
    //             if (response.clinicAdmin === "Normal" || response.clinicAdmin === "FullAccessClinicAdmin") {
    //                if (response.clinicSectionGuid.length > 1) {
    //                    var AllGuids = [];
    //                    for (let i = 0; i < response.clinicSectionGuid.length; i++) {
    //                        AllGuids.push(response.clinicSectionGuid[i]);
    //                    }
    //                    $('#ChooseClinicSectionModal').modal('toggle');
    //                    let link = "/ApplicationHandler/ChooseClinicSection?clinicSections=";
    //                    $('#ChooseClinicSectionModal-body').load(link + AllGuids, function () {
    //                        $(".loader").fadeIn("slow");
    //                        $(".loader").addClass("hidden");

    //                    });


    //                }

    //            }
    //            $(".loader").fadeIn("slow");
    //            $(".loader").addClass("hidden");


    //        }
    //    }
    //})
}



function goToClinicAdmin() {
    window.location.href = "/UserHandler/";
}

function TryDate(element) {

    let date_txt = $(element).val();

    let today = new Date();
    let Month = today.getMonth() + 1;
    let month_txt = Month;
    let Day = today.getDate();
    let day_txt = Day;
    let year_txt = today.getFullYear();

    if (Month < 10)
        month_txt = `0${Month}`;

    if (Day < 10)
        day_txt = `0${Day}`;

    if (date_txt.length === 4 && !isNaN(date_txt)) {

        year_txt = date_txt;
    }

    if (date_txt.length === 8 && !isNaN(date_txt)) {
        let dd = date_txt.substring(0, 2);
        let mm = date_txt.substring(2, 4);
        let yy = date_txt.substring(4);

        if (!isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {

            day_txt = dd;
            month_txt = mm;
            year_txt = yy;
        }
    }

    let space = date_txt.indexOf(' ');
    if ((date_txt.length === 10 || date_txt.length === 7) && space !== -1) {

        let ele = date_txt.split(" ");
        let dd;
        let mm;
        let yy;

        if (ele.length == 3) {
            if (ele[0].length == 4) {
                dd = ele[2];
                mm = ele[1];
                yy = ele[0];
            } else if (ele[2].length == 4) {
                dd = ele[0];
                mm = ele[1];
                yy = ele[2];
            }

        } else if (ele.length == 2) {
            if (ele[0].length == 4) {
                dd = day_txt;
                mm = ele[1];
                yy = ele[0];
            } else if (ele[1].length == 4) {
                dd = day_txt;
                mm = ele[0];
                yy = ele[1];
            }

        }

        if (!isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {

            day_txt = dd;
            month_txt = mm;
            year_txt = yy;
        }
    }

    let slash = date_txt.indexOf('/');
    if ((date_txt.length === 10 || date_txt.length === 7) && slash !== -1) {

        let ele = date_txt.split("/");
        let dd;
        let mm;
        let yy;

        if (ele.length == 3) {
            if (ele[0].length == 4) {
                dd = ele[2];
                mm = ele[1];
                yy = ele[0];
            } else if (ele[2].length == 4) {
                dd = ele[0];
                mm = ele[1];
                yy = ele[2];
            }

        } else if (ele.length == 2) {
            if (ele[0].length == 4) {
                dd = day_txt;
                mm = ele[1];
                yy = ele[0];
            } else if (ele[1].length == 4) {
                dd = day_txt;
                mm = ele[0];
                yy = ele[1];
            }

        }

        if (!isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {

            day_txt = dd;
            month_txt = mm;
            year_txt = yy;
        }
    }

    let res = day_txt + '/' + month_txt + '/' + year_txt;

    $(element).val(res);
}

function TryDateWithOutPast(element) {

    let date_txt = $(element).val();

    let today = new Date();
    let Month = today.getMonth() + 1;
    let month_txt = Month;
    let Day = today.getDate();
    let day_txt = Day;
    let year_txt = today.getFullYear();

    if (Month < 10)
        month_txt = `0${Month}`;

    if (Day < 10)
        day_txt = `0${Day}`;

    if (date_txt.length === 4 && !isNaN(date_txt)) {
        if (year_txt > Number(date_txt))
            year_txt = date_txt;
    }

    if (date_txt.length === 8 && !isNaN(date_txt)) {
        let dd = date_txt.substring(0, 2);
        let mm = date_txt.substring(2, 4);
        let yy = date_txt.substring(4);

        var new_date = new Date(`${yy}/${mm}/${dd}`);
        if (new_date > today && !isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {

            day_txt = dd;
            month_txt = mm;
            year_txt = yy;
        }
    }

    let space = date_txt.indexOf(' ');
    if ((date_txt.length === 10 || date_txt.length === 7) && space !== -1) {

        let ele = date_txt.split(" ");
        let dd;
        let mm;
        let yy;

        if (ele.length == 3) {
            if (ele[0].length == 4) {
                dd = ele[2];
                mm = ele[1];
                yy = ele[0];
            } else if (ele[2].length == 4) {
                dd = ele[0];
                mm = ele[1];
                yy = ele[2];
            }

        } else if (ele.length == 2) {
            if (ele[0].length == 4) {
                dd = day_txt;
                mm = ele[1];
                yy = ele[0];
            } else if (ele[1].length == 4) {
                dd = day_txt;
                mm = ele[0];
                yy = ele[1];
            }

        }

        if (!isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {
            var new_date = new Date(`${yy}/${mm}/${dd}`);

            if (new_date > today) {
                day_txt = dd;
                month_txt = mm;
                year_txt = yy;
            }
        }
    }

    let slash = date_txt.indexOf('/');
    if ((date_txt.length === 10 || date_txt.length === 7) && slash !== -1) {

        let ele = date_txt.split("/");
        let dd;
        let mm;
        let yy;

        if (ele.length == 3) {
            if (ele[0].length == 4) {
                dd = ele[2];
                mm = ele[1];
                yy = ele[0];
            } else if (ele[2].length == 4) {
                dd = ele[0];
                mm = ele[1];
                yy = ele[2];
            }

        } else if (ele.length == 2) {
            if (ele[0].length == 4) {
                dd = day_txt;
                mm = ele[1];
                yy = ele[0];
            } else if (ele[1].length == 4) {
                dd = day_txt;
                mm = ele[0];
                yy = ele[1];
            }

        }

        if (!isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {
            var new_date = new Date(`${yy}/${mm}/${dd}`);

            if (new_date > today) {
                day_txt = dd;
                month_txt = mm;
                year_txt = yy;
            }
        }
    }

    let res = day_txt + '/' + month_txt + '/' + year_txt;

    $(element).val(res);
}

function TryDateWithOutFuture(element) {

    let date_txt = $(element).val();

    let today = new Date();
    let Month = today.getMonth() + 1;
    let month_txt = Month;
    let Day = today.getDate();
    let day_txt = Day;
    let year_txt = today.getFullYear();

    if (Month < 10)
        month_txt = `0${Month}`;

    if (Day < 10)
        day_txt = `0${Day}`;

    if (date_txt.length === 4 && !isNaN(date_txt)) {
        if (year_txt < Number(date_txt))
            year_txt = date_txt;
    }

    if (date_txt.length === 8 && !isNaN(date_txt)) {
        let dd = date_txt.substring(0, 2);
        let mm = date_txt.substring(2, 4);
        let yy = date_txt.substring(4);

        var new_date = new Date(`${yy}/${mm}/${dd}`);
        if (new_date < today && !isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {

            day_txt = dd;
            month_txt = mm;
            year_txt = yy;
        }
    }

    let space = date_txt.indexOf(' ');
    if ((date_txt.length === 10 || date_txt.length === 7) && space !== -1) {

        let ele = date_txt.split(" ");
        let dd;
        let mm;
        let yy;

        if (ele.length == 3) {
            if (ele[0].length == 4) {
                dd = ele[2];
                mm = ele[1];
                yy = ele[0];
            } else if (ele[2].length == 4) {
                dd = ele[0];
                mm = ele[1];
                yy = ele[2];
            }

        } else if (ele.length == 2) {
            if (ele[0].length == 4) {
                dd = day_txt;
                mm = ele[1];
                yy = ele[0];
            } else if (ele[1].length == 4) {
                dd = day_txt;
                mm = ele[0];
                yy = ele[1];
            }

        }

        if (!isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {
            var new_date = new Date(`${yy}/${mm}/${dd}`);

            if (new_date < today) {
                day_txt = dd;
                month_txt = mm;
                year_txt = yy;
            }
        }
    }

    let slash = date_txt.indexOf('/');
    if ((date_txt.length === 10 || date_txt.length === 7) && slash !== -1) {

        let ele = date_txt.split("/");
        let dd;
        let mm;
        let yy;

        if (ele.length == 3) {
            if (ele[0].length == 4) {
                dd = ele[2];
                mm = ele[1];
                yy = ele[0];
            } else if (ele[2].length == 4) {
                dd = ele[0];
                mm = ele[1];
                yy = ele[2];
            }

        } else if (ele.length == 2) {
            if (ele[0].length == 4) {
                dd = day_txt;
                mm = ele[1];
                yy = ele[0];
            } else if (ele[1].length == 4) {
                dd = day_txt;
                mm = ele[0];
                yy = ele[1];
            }

        }

        if (!isNaN(yy) && !isNaN(mm) && !isNaN(dd)) {
            var new_date = new Date(`${yy}/${mm}/${dd}`);

            if (new_date < today) {
                day_txt = dd;
                month_txt = mm;
                year_txt = yy;
            }
        }
    }

    let res = day_txt + '/' + month_txt + '/' + year_txt;

    $(element).val(res);
}



function dateTimePickerWorkWithSpace(element) {

    let today = new Date();
    let date = $(element).val();

    if (date.length === 4) {
        let datePickerDate = 01 + '/' + 01 + '/' + date;
        $(element).val(datePickerDate);
        getAge(datePickerDate);

        return;
    }

    if (date.length < 4) {
        let datePickerDate = 01 + '/' + 01 + '/' + today.getFullYear();
        $(element).val(datePickerDate);
        getAge(datePickerDate);

        return;
    }

    if (date.length > 4) {

        let sp = date.indexOf(' ');
        if (sp !== -1) {

            let correctDate = date.split(' ');
            let year = today.getFullYear();

            if (correctDate.length > 3) {
                let datePickerDate = 01 + '/' + 01 + '/' + year;
                $(element).val(datePickerDate);
                getAge(datePickerDate);
                return;
            }

            if (correctDate.length === 3) {
                if (correctDate[2] > year) {
                    let Month = today.getMonth() + 1;
                    let Day = today.getDate();

                    if (Month < 10)
                        Month = "0" + Month;

                    if (Day < 10)
                        Day = "0" + Day;

                    let datePickerDate = Day + '/' + Month + '/' + year;
                    $(element).val(datePickerDate);
                    getAge(datePickerDate);
                    return;
                }
                if (correctDate[2] < 1900) {
                    let Month = today.getMonth() + 1;
                    let Day = today.getDate();

                    if (Month < 10)
                        Month = "0" + Month;

                    if (Day < 10)
                        Day = "0" + Day;

                    let datePickerDate = Day + '/' + Month + '/' + 1900;
                    $(element).val(datePickerDate);
                    getAge(datePickerDate);
                    return;
                }

                let datePickerDate = correctDate[0] + '/' + correctDate[1] + '/' + correctDate[2];
                $(element).val(datePickerDate);
                getAge(datePickerDate);
                return;
            }

            if (correctDate.length === 2) {

                if (correctDate[1] > year) {
                    let Month = today.getMonth() + 1;
                    let Day = today.getDate();

                    if (Month < 10)
                        Month = "0" + Month;

                    if (Day < 10)
                        Day = "0" + Day;

                    let datePickerDate = Day + '/' + Month + '/' + year;
                    $(element).val(datePickerDate);
                    getAge(datePickerDate);
                    return;
                }
                if (correctDate[1] < 1900) {
                    let Month = today.getMonth() + 1;
                    let Day = today.getDate();

                    if (Month < 10)
                        Month = "0" + Month;

                    if (Day < 10)
                        Day = "0" + Day;

                    let datePickerDate = Day + '/' + Month + '/' + 1900;
                    $(element).val(datePickerDate);
                    getAge(datePickerDate);
                    return;
                }

                let datePickerDate = 01 + '/' + correctDate[0] + '/' + correctDate[1];
                $(element).val(datePickerDate);
                getAge(datePickerDate);
                return;
            }

            if (correctDate.length === 1) {
                if (correctDate[0] > year) {
                    let Month = today.getMonth() + 1;
                    let Day = today.getDate();

                    if (Month < 10)
                        Month = "0" + Month;

                    if (Day < 10)
                        Day = "0" + Day;

                    let datePickerDate = Day + '/' + Month + '/' + year;
                    $(element).val(datePickerDate);
                    getAge(datePickerDate);
                    return;
                }
                if (correctDate[0] < 1900) {
                    let Month = today.getMonth() + 1;
                    let Day = today.getDate();

                    if (Month < 10)
                        Month = "0" + Month;

                    if (Day < 10)
                        Day = "0" + Day;

                    let datePickerDate = Day + '/' + Month + '/' + 1900;
                    $(element).val(datePickerDate);
                    getAge(datePickerDate);
                    return;
                }

                let datePickerDate = 01 + '/' + 01 + '/' + correctDate[0];
                $(element).val(datePickerDate);
                getAge(datePickerDate);
                return;
            }


        }
        let slash = date.indexOf('/');
        if (slash === -1) {

            let datePickerDate = 01 + '/' + 01 + '/' + today.getFullYear();
            $(element).val(datePickerDate);
            getAge(datePickerDate);
            return;
        }

    }

}



function GetClinicId() {

    var clinicId = $('#btn-clinic-accept').attr('data-ClinicId');

    return {

        ClinicId: clinicId

    };
}



function GetPeriodAndTimeForMoneyConvert() {

    var periodId = $('#period-id').attr('data-Value');
    var fromDate = $("#KendoFromDate").data("kendoDatePicker");
    var toDate = $("#KendoDateTo").data("kendoDatePicker");

    var frDate = fromDate.value();
    var tDate = toDate.value();
    let frMonth = frDate.getMonth() + 1;
    let frDay = frDate.getDate();
    let toMonth = tDate.getMonth() + 1;
    let toDay = tDate.getDate();

    if (frMonth < 10)
        frMonth = "0" + frMonth;
    if (toMonth < 10)
        toMonth = "0" + toMonth;
    if (frDay < 10)
        frDay = "0" + frDay;
    if (toDay < 10)
        toDay = "0" + toDay;

    let from = frDate.getFullYear() + "-" + frMonth + "-" + frDay;
    let too = tDate.getFullYear() + "-" + toMonth + "-" + toDay;


    return {
        periodId: periodId,
        dateFrom: from,
        dateTo: too
    };
}

function GetAllDoctorList() {
    return $.ajax({
        type: "Get",
        url: "/Doctor/GetAllDoctorsForFilter",
    });
}

function FilterDoctorLists(doctor_list, filter) {

    var filter_list = doctor_list.filter(function (item) {
        if (this.count < 20 && item.NameAndSpeciallity.toLowerCase().indexOf(filter) > -1) {
            this.count++;
            return true;
        }

        return false;

    }, { count: 0 });

    return filter_list;
}

function GetAllPatientList() {
    return $.ajax({
        type: "Get",
        url: "/Patient/GetAllPatientForFilter",
    });
}

function FilterPatientLists(patient_list, filter) {

    var filter_list = patient_list.filter(function (item) {
        if (this.count < 20 && item.Name.toLowerCase().indexOf(filter) > -1) {
            this.count++;
            return true;
        }

        return false;

    }, { count: 0 });

    return filter_list;
}

function FilterPatientListsByMobileAndName(patient_list, filter) {

    var filter_list = patient_list.filter(function (item) {
        if (this.count < 20 && item.PhoneNumberAndName.toLowerCase().indexOf(filter) > -1) {
            this.count++;
            return true;
        }

        return false;

    }, { count: 0 });

    return filter_list;
}

function FilterPatientListsByFormNumber(patient_list, filter) {

    var filter_list = patient_list.filter(function (item) {
        if (this.count < 20 && item.FormNumber.toLowerCase().indexOf(filter) > -1) {
            this.count++;
            return true;
        }

        return false;

    }, { count: 0 });

    return filter_list;
}

function GetAllTotalProductList() {
    return $.ajax({
        type: "Get",
        url: "/Product/GetAllTotalProductsForFilter",
    });
}

function GetAllMaterialProductList() {
    return $.ajax({
        type: "Get",
        url: "/Product/GetAllMaterialProductsForFilter",
    });
}

function GetAllProductList() {
    return $.ajax({
        type: "Get",
        url: "/Product/GetAllProductsForFilter",
    });
}

function FilterProductLists(product_list, filter) {

    var filter_list = product_list.filter(function (item) {
        if (this.count < 20 && item.FullName.toLowerCase().indexOf(filter) > -1) {
            this.count++;
            return true;
        }

        return false;

    }, { count: 0 });

    return filter_list;
}

function RemoveFromListByGuid(main_list, guid) {

    var filter_list = main_list.filter(function (item) {
        if (item.Guid.toLowerCase().indexOf(guid) > -1) {
            return false;
        }

        item.Index = this.count;
        this.count++;
        return true;

    }, { count: 1 });

    return filter_list;
}

function ResetIndexList(main_list) {

    var filter_list = main_list.filter(function (item) {

        item.Index = this.count;
        this.count++;
        return true;

    }, { count: 1 });

    return filter_list;
}

function GetFirstNumber(text) {
    text = text.replaceAll("٬", ",");
    let numbers;

    if (!text || typeof text !== 'string') {
        return text;
    }

    numbers = text.match(/(-\d+|\d+)(,\d+)*(\.\d+)*/g);
    numbers = numbers.map(n => Number(n.replace(/,/g, '')));

    return numbers[0];
}
