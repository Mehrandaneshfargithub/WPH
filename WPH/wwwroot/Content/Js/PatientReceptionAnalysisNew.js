
var Analysis;
var AnalysisItems;
var groupAnalysis;
var decimalAmount;
var selectedAnalysis;
var Address;
let addressAuto;
let SpeciallityDropDown;
let nameAuto;
let doctorAuto;
let radiologyDoctorAuto;
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
var doctor_list = [];
var patient_list = [];
let firstTime = true;
var hospitalReception = false;

function GetPatientReceptionAnalysisByReceptionId(receptionId) {
    return $.ajax({
        type: "Post",
        data: { ReceptionId: receptionId },
        url: "/PatientReceptionAnalysis/GetPatientReceptionAnalysisByReceptionId",
    });
}

function GetReception(receptionId) {
    return $.ajax({
        type: "Post",
        data: { ReceptionId: receptionId },
        url: "/PatientReceptionAnalysis/GetReception",
    });
}

$(document).ready(function () {
    GetAllDoctorList().done(function (data) {
        doctor_list = data;
    });

    GetAllPatientList().done(function (data) {
        patient_list = data;
    });

    CreateTelerikComponents();

    let receptionId = $("#Reception-Id").text();
    let patientId = $("#PatientId-Id").text();
    let doctorId = $("#Doctor-Id").text();
    let receptionClinicSectionDescription = $("#ReceptionClinicSectionDescription-Id").text();

    if (receptionId !== "" && receptionId !== null && receptionId !== '00000000-0000-0000-0000-000000000000') {
        setTimeout(() => {
            $(".loader").removeClass("hidden");
        })

        $.when(GetReception(receptionId), GetPatientReceptionAnalysisByReceptionId(receptionId)).done(function (reception, analysis) {

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");

            SetReceptionSelectedAnalysis(analysis[0], reception[0]);

        });

        $.ajax({
            type: "Post",
            data: { ReceptionId: receptionId },
            url: "/Reception/GetReceptionDoctor",
            success: function (response) {

                if (response !== null && response !== '' && response !== 0) {

                    for (var item in response) {
                        if (response[item].DoctorRoleName == "RadiologyDoctor") {
                            var radio = response[item];

                            $("#RadiologyDoctorId").val(radio.DoctorId);
                            $("#RadiologyDoctor").val(radio.Doctor.UserName);

                            break;
                        }
                    }

                    for (var key in response) {
                        if (response[key].DoctorRoleName == "DispatcherDoctor") {
                            var doc = response[key];

                            $("#DoctorId").val(doc.DoctorId);
                            $("#Doctor").val(doc.Doctor.UserName);
                            let section = $("#Speciality").data("kendoDropDownList");
                            section.value(doc.Doctor.SpecialityId);

                            break;
                        }
                    }



                }

            }
        });


    }

    else if (patientId !== "" && patientId !== null) {

        $("#ReceptionClinicSectionDescriptionParent").removeClass('hidden');
        $("#ReceptionClinicSectionDescription").val(receptionClinicSectionDescription);
        hospitalReception = true;
        $.ajax({
            type: "Post",
            data: { PatientId: patientId },
            url: "/Patient/GetPatientById",
            success: function (response) {

                SetPatientAmounts(response);

            }
        });

        if (doctorId !== "" && doctorId !== null) {
            $.ajax({
                type: "Post",
                data: { DoctorId: doctorId },
                url: "/Doctor/GetDoctorById",
                success: function (response) {

                    SetDoctorAmounts(response);

                }
            });
        }

        $.ajax({
            type: "Post",
            //data: { ReceptionId: receptionId },
            url: "/PatientReception/GetLatestReceptionNum",
            success: function (response) {

                $("#ReceptionNum").val(response);
                $("#ReceptionNumlbl").text(response);
            }
        });

    }
    else {
        $.ajax({
            type: "Post",
            //data: { ReceptionId: receptionId },
            url: "/PatientReception/GetLatestReceptionNum",
            success: function (response) {

                $("#ReceptionNum").val(response);
                $("#ReceptionNumlbl").text(response);
            }
        });

        setTimeout(function () {
            let date = $("#DateOfBirth").data("kendoDatePicker");
            date.open();
            date.close();
            let s = $("#DateOfBirth").attr('aria-owns');

            let s3 = $("#" + s + " .k-footer a").click();
            $('#Name2').focus();
        });

    }

    $.ajax({
        type: "Post",
        data: { settingName: "UseDollar" },
        url: "/ClinicSection/GetClinicSectionSettingValueBySettingName",
        success: function (response) {

            $("#useDollar").text(response);
            if (response === "true") {
                let all = $(".dollarclass");
                all.addClass("col-md-4");
                all.removeClass("col-md-8");

                let allhidden = $(".classHidden");
                allhidden.removeClass("hidden");

                if (AllMoneyConverts[0] === null) {
                    AddMoneyConvertModal();
                    MoneyConvertAmount.value(0);
                }
                else {
                    let val = AllMoneyConverts[0].Amount * 100;
                    MoneyConvertAmount.value(val);
                }

            }

        }
    });

    $.ajax({
        type: "Post",
        data: { BaseInfoName: "DoctorSpeciality" },
        url: "/BaseInfo/GetBaseInfoTypeByName",
        success: function (response) {

            $("#Speciallity").attr('data-Value', response);


        }
    });

    getTranslatedWords();



    KeyPressAndFocusConfigs();

    createSelectedGrid();
    createGroupAnalysisNameAutoComplete();
    createGroupAnalysisCodeAutoComplete();
    createAnalysisNameAutoComplete();
    createAnalysisCodeAutoComplete();
    createAnalysisItemNameAutoComplete();
    createAnalysisItemCodeAutoComplete();
    createAllNameAutoComplete();
    createAllCodeAutoComplete();

    //createPatientReceptionReceivedDetailGrid();

});

function OnRadiologyDoctorSelect(e) {

    var DataItem = this.dataItem(e.item.index());

    var set_name;
    try {
        var set_name = DataItem.User.Name;
    } catch {
        var set_name = DataItem.UserName;
    }

    $("#RadiologyDoctor").val(set_name);
    $("#RadiologyDoctorId").val(DataItem.Guid);

    setTimeout(function () {
        $("#RadiologyDoctor").val(set_name);
    }, 20);

}


async function SetReceptionSelectedAnalysis(allAnalysis, reception) {


    await SetReceptionAmounts(reception);

    selectedAnalysis = allAnalysis;

    if (selectedAnalysis !== null) {

        for (let i = 0; i < selectedAnalysis.length; i++) {
            let id;
            let name;
            let code;

            if (selectedAnalysis[i].AnalysisItemId !== null && selectedAnalysis[i].AnalysisItemId !== undefined) {
                id = selectedAnalysis[i].AnalysisItemId;
                selectedAnalysis[i].AnalysisTypeName = "Analysis Item";
                name = selectedAnalysis[i].AnalysisItem.Name;
                if (selectedAnalysis[i].AnalysisItem.Code === null)
                    code = "";
                else
                    code = selectedAnalysis[i].AnalysisItem.Code;
            }
            else if (selectedAnalysis[i].GroupAnalysisId !== null && selectedAnalysis[i].GroupAnalysisId !== undefined) {
                id = selectedAnalysis[i].GroupAnalysisId;
                selectedAnalysis[i].AnalysisTypeName = "Group Analysis";
                name = selectedAnalysis[i].GroupAnalysis.Name;
                if (selectedAnalysis[i].GroupAnalysis.Code === null)
                    code = "";
                else
                    code = selectedAnalysis[i].GroupAnalysis.Code;
            }
            else if (selectedAnalysis[i].AnalysisId !== null && selectedAnalysis[i].AnalysisId !== undefined) {
                id = selectedAnalysis[i].AnalysisId;
                selectedAnalysis[i].AnalysisTypeName = "Analysis";
                name = selectedAnalysis[i].Analysis.Name;
                if (selectedAnalysis[i].Analysis.Code === null)
                    code = "";
                else
                    code = selectedAnalysis[i].Analysis.Code;
            }

            if (selectedAnalysis[i].Discount === null)
                selectedAnalysis[i].Discount = 0;
            let total = parseFloat(selectedAnalysis[i].Amount) - parseFloat(selectedAnalysis[i].Discount);
            selectedItems.push({ Guid: id, Type: selectedAnalysis[i].AnalysisTypeName, Name: name, Code: code, Price: selectedAnalysis[i].Amount, Discount: selectedAnalysis[i].Discount, Total: total });
        }

    }

    let SelectedGrid = $("#SelectedGrid").data("kendoGrid");
    SelectedGrid.setDataSource(selectedItems);
    changeDiscountAmount();
}

async function SetReceptionAmounts(Reception) {
    if (Reception.HospitalReception) {
        disableComponents();
        $("#ReceptionClinicSectionDescriptionParent").removeClass('hidden');
        let receptionId = $("#Reception-Id").text();
        hospitalReception = true;
        $.ajax({
            type: "Post",
            data: { DestinationReceptionId: receptionId },
            url: "/Reception/GetReceptionClinicSectionByDestinationReceptionId",
            success: function (response) {

                $("#ReceptionClinicSectionDescription").val(response.Description);

            }
        });

    }
    $("#Guid").val(Reception.Guid);
    $("#CreatedUserId").val(Reception.CreatedUserId);
    $("#PatientId").val(Reception.PatientId);
    $("#AddressId").val(Reception.AddressId);
    $("#DateOfBirthDay").val(Reception.DateOfBirthDay);
    $("#DateOfBirthMonth").val(Reception.DateOfBirthMonth);
    $("#DateOfBirthYear").val(Reception.DateOfBirthYear);
    $("#ReceptionNum").val(Reception.ReceptionNum);
    $("#ReceptionNumlbl").text(Reception.ReceptionNum);
    $("#Name2").val(Reception.Patient.User.Name);
    $("#PhoneNumber").val(Reception.Patient.User.PhoneNumber);

    let gender = $("#GenderId").data("kendoDropDownList");
    gender.value(Reception.Patient.GenderId);
    $("#DateOfBirth").val(Reception.Patient.DateOfBirth);

    setTimeout(function () {
        let datepicker = $("#DateOfBirth").data("kendoDatePicker");
        let s = $("#DateOfBirth").attr('aria-owns');

        datepicker.open();
        datepicker.close();

        let s3 = $("#" + s + " .k-state-focused a").click();
    });

    TotalReceived = Reception.TotalReceived;

    if (TotalReceived === "" || TotalReceived === null) {
        TotalReceived = 0;
    }

    if (typeof TotalReceived === "string") {
        TotalReceived = parseFloat(TotalReceived);
    }

    $("#TotalReceived").val(TotalReceived);

    let section = $("#ClinicSectionId").data("kendoDropDownList");
    section.value(Reception.ClinicSectionId);
    $("#Explanation").val(Reception.Description);

    let numerictextbox = $("#Discount").data("kendoNumericTextBox");
    numerictextbox.value(Reception.Discount);

    let numerictextboxAmount = $("#DiscountAmount").data("kendoNumericTextBox");
    numerictextboxAmount.value(Reception.Discount);

    $("#TotalReceived").val(Reception.TotalReceived);
    $("#TotalReceivedDollar").val(Reception.TotalReceived);
}

function disableComponents() {

    nameAuto.enable(false);
    $("#Year").data("kendoTextBox").enable(false);
    $("#Month").data("kendoTextBox").enable(false);
    let phon = $("#PhoneNumber").data("kendoTextBox"); 
    phon.enable(false);
    let gender = $("#GenderId").data("kendoDropDownList");
    gender.enable(false);
    SpeciallityDropDown.enable(false);
    doctorAuto.enable(false);
    radiologyDoctorAuto.enable(false);
    let datepicker = $("#DateOfBirth").data("kendoDatePicker");
    datepicker.enable(false);

}

function SetPatientAmounts(patient) {

    $("#PatientId").val(patient.Guid);
    $("#AddressId").val(patient.AddressId);
    $("#DateOfBirthDay").val(patient.DateOfBirthDay);
    $("#DateOfBirthMonth").val(patient.DateOfBirthMonth);
    $("#DateOfBirthYear").val(patient.DateOfBirthYear);
    $("#Name2").val(patient.Name);

    $("#PhoneNumber").val(patient.PhoneNumber);

    let gender = $("#GenderId").data("kendoDropDownList");
    gender.value(patient.GenderId);

    $("#DateOfBirth").val(patient.DateOfBirth);
    setTimeout(function () {
        let datepicker = $("#DateOfBirth").data("kendoDatePicker");
        let s = $("#DateOfBirth").attr('aria-owns');

        datepicker.open();

        datepicker.close();

        let s3 = $("#" + s + " .k-state-focused a").click();

    });

    disableComponents();
}

function SetDoctorAmounts(doctor) {

    $("#DoctorId").val(doctor.Guid);
    $("#Doctor").val(doctor.UserName);

    try {
        SpeciallityDropDown.value(Reception.Doctor.SpecialityId);
    }
    catch (e) {

    };

}

function getTranslatedWords() {

    TotalTranslated = $("#Translated").attr("data-TotalTranslated");
    DiscountTranslated = $("#Translated").attr("data-DiscountTranslated");
    PriceTranslated = $("#Translated").attr("data-PriceTranslated");
    CodeTranslated = $("#Translated").attr("data-CodeTranslated");
    TypeTranslated = $("#Translated").attr("data-TypeTranslated");
    NameTranslated = $("#Translated").attr("data-NameTranslated");
    AmountTranslated = $("#Translated").attr("data-AmountTranslated");
    DateTranslated = $("#Translated").attr("data-DateTranslated");

}

function CreateTelerikComponents() {
    //$("#ReceptionNumber").kendoTextBox({});

    $('#Name2').on('input', function () {
        var txt = $("#Name2").val();

        var filter_list = FilterPatientListsByMobileAndName(patient_list, txt);

        $("#Name2").data("kendoAutoComplete").dataSource.data(filter_list);

    });

    nameAuto = $("#Name2").kendoAutoComplete({
        dataTextField: "PhoneNumberAndName",
        filter: "contains",
        highlightFirst: true,
        clearButton: false,
        select: OnPatientSelectInReception,
        dataSource: {
            //type: "odata",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/PatientReceptionAnalysis/GetPatient"
                }
            }
        }
    }).data("kendoAutoComplete");

    $("#PhoneNumber").kendoTextBox({

    });

    $("#GenderId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
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

    $("#DateOfBirth").kendoDatePicker({
        format: "dd/MM/yyyy",
        max: new Date(),
        animation: false,
        change: changeBirthOfDatePatientDatetimePicker,
    })

    $("#Year").kendoTextBox({});

    $("#Month").kendoTextBox({});


    //radiologyDoctorAuto = $("#RadiologyDoctor").kendoAutoComplete({
    //    dataTextField: "UserName",
    //    filter: "contains",
    //    highlightFirst: true,
    //    clearButton: false,
    //    //select: OnRadiologyDoctorSelect,
    //    dataSource: {
    //        serverFiltering: false,
    //        transport: {
    //            read: {
    //                read: "/Doctor/GetAllDoctors"
    //            }
    //        }
    //    }
    //}).data("kendoAutoComplete");

    $('#RadiologyDoctor').on('input', function () {
        var txt = $("#RadiologyDoctor").val();

        var filter_list = FilterDoctorLists(doctor_list, txt);

        $("#RadiologyDoctor").data("kendoAutoComplete").dataSource.data(filter_list);

    });

    radiologyDoctorAuto = $("#RadiologyDoctor").kendoAutoComplete({
        dataTextField: "NameAndSpeciallity",
        filter: "contains",
        highlightFirst: true,
        clearButton: false,
        select: OnRadiologyDoctorSelect,
        dataSource: {
            //type: "odata",
            serverFiltering: false,
            transport: {
                read: "/PatientReceptionAnalysis/GetDoctor"
            }
        }
    }).data("kendoAutoComplete");

    $('#Doctor').on('input', function () {
        var txt = $("#Doctor").val();

        var filter_list = FilterDoctorLists(doctor_list, txt);

        $("#Doctor").data("kendoAutoComplete").dataSource.data(filter_list);
    });

    doctorAuto = $("#Doctor").kendoAutoComplete({
        dataTextField: "NameAndSpeciallity",
        filter: "contains",
        highlightFirst: true,
        clearButton: false,
        select: OnDoctorSelect,
        dataSource: {
            //type: "odata",
            serverFiltering: false,
            transport: {
                read: "/PatientReceptionAnalysis/GetDoctor"
            }
        }
    }).data("kendoAutoComplete");

    SpeciallityDropDown = $("#Speciality").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        dataSource: {
            //type: "odata",
            serverFiltering: true,
            transport: {
                read: {
                    url: "/BaseInfo/GetAllDoctorSpecialities",
                }
            }
        }
    }).data("kendoDropDownList");

    $("#ClinicSectionId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        select: selectClinicSection,
        dataBound: function (e) {

            //$("#ClinicSectionId_listbox .k-state-selected").click();
            setTimeout(() => {

                let id = $("#ClinicSectionId").val();
                selectClinicSectionFirstTime(id);

            }, 20)

        },
        dataSource: {
            //type: "odata",
            serverFiltering: true,
            transport: {
                read: {
                    url: "/ClinicSection/GetAllLabAndXRayClinicSectionsForUser",
                    //dataType: "jsonp"
                }
            }
        }
    });


    $("#MoneyConvertAmount").kendoNumericTextBox({
        format: "# IQD",

    });

    $("#Explanation").kendoTextArea({
        rows: 2,
        maxLength: 200,
        placeholder: $("#Translated").attr("data-ExplanationTranslated"),
    });

    $("#ReceptionClinicSectionDescription").kendoTextArea({
        rows: 2,
        maxLength: 200,
        enable: false
    });

    $("#buttonTabstrip").kendoTabStrip({
        animation: false
        //    open: {
        //        effects: "fadeIn"
        //    }
        //}
    });

    $("#tabstrip").kendoTabStrip({
        animation: false
        //    open: {
        //        effects: "fadeIn"
        //    }
        //}
    });

    $("#totalAmount").kendoTextBox({
        readonly: true
    });

    $("#totalAmountDollar").kendoTextBox({
        readonly: true
    });

    $("#DiscountAmount").kendoNumericTextBox({
        format: "N0",
        min: 0
    });

    $("#Discount").kendoNumericTextBox({
        format: "##.00 %",
        min: 0,
        max: 100,
    });

    $("#totalAmountWithDiscount").kendoTextBox({
        readonly: true
    });

    $("#totalAmountWithDiscountDollar").kendoTextBox({
        readonly: true
    });

    $("#RemainedAmount").kendoTextBox({
        readonly: true
    });

    $("#RemainedAmountDollar").kendoTextBox({
        readonly: true
    });

    $("#PatientReceptionReceivedAmount").kendoNumericTextBox({
        format: "N0",
        min: 0,
    });

    $("#PatientReceptionReceivedAmountDollar").kendoNumericTextBox({
        format: "C2",
        min: 0,
    });

    $("#TotalReceived").kendoTextBox({
        readonly: true
    });

    $("#TotalReceivedDollar").kendoTextBox({
        readonly: true
    });

    $("#btn-address-accept").kendoButton();
    $("#btn-address-close").kendoButton();
    $("#btn-moneyConvert-accept").kendoButton();
    $("#btn-allMoneyConvert-accept").kendoButton();
    $("#btn-allMoneyConvert-close").kendoButton();
    $("#btn-PatientReceptionReceived-accept").kendoButton();
    $("#btn-PatientReceptionReceived-close").kendoButton();
    $("#btn-PatientReceptionReceivedDetailed-close").kendoButton();
    $("#btn-ChosseAnalysis-accept").kendoButton();

    $("#tabstripChooseAnalysis").kendoTabStrip({
        animation: false
        //open: {
        //    effects: "fadeIn"
        //}
        //}
    });
}


function analysisAjax(id) {


    //$('#analysisButton').empty();
    $('#analysisButton').html('<div style="margin-right: auto; display: inline-block; position: absolute;left:50%;top:30%"><img src = "../Content/kendo/css/Default/loading_2x.gif" /></div>');
    $('#analysisList').html('<div style="margin-right: auto; display: inline-block; position: absolute;left:50%;top:30%"><img src = "../Content/kendo/css/Default/loading_2x.gif" /></div>');

    return $.ajax({
        type: "Post",
        data: { ClinicSectionId: id },
        url: "/Analysis/GetAllAnalysesByClinicSectionId",
        success: function (response) {

            Analysis = response;

            for (let j = 0; j < Analysis.length; j++) {

                Analysis[j].Type = 'Analysis';
                if (Analysis[j].Code === null)
                    Analysis[j].Code = "";
                if (Analysis[j].Amount === null)
                    Analysis[j].Amount = 0;
                if (Analysis[j].Discount === null)
                    Analysis[j].Discount = 0;
                Analysis[j].disabled = false;
                AllItems.push(Analysis[j]);
            }


            createAnalysisGrid();

            let AnalysisNameAutoComplete = $("#AnalysisNameAutoComplete").data("kendoAutoComplete");
            AnalysisNameAutoComplete.setDataSource(Analysis);

            let list = [];
            //list = list.concat(Analysis);
            //let index = list.findIndex(a => a.Code == "");
            //if (index > -1) {
            //    list.splice(index, 1);
            //}

            list = Analysis.filter(function (value, index, arr) {
                return value.Code !== '';
            });


            let AnalysisCodeAutoComplete = $("#AnalysisCodeAutoComplete").data("kendoAutoComplete");
            AnalysisCodeAutoComplete.setDataSource(list);
        }
    });

}


function analysisItemAjax(id) {

    //$('#analysisItemButton').empty();
    $('#analysisItemButton').html('<div style="margin-right: auto; display: inline-block; position: absolute;left:50%;top:30%"><img src = "../Content/kendo/css/Default/loading_2x.gif" /></div>');
    $('#analysisItemList').html('<div style="margin-right: auto; display: inline-block; position: absolute;left:50%;top:30%"><img src = "../Content/kendo/css/Default/loading_2x.gif" /></div>');


    return $.ajax({
        type: "Post",
        data: { ClinicSectionId: id },
        url: "/AnalysisItem/GetAllAnalysisItemByClinicSectionId",
        success: function (response) {
            AnalysisItems = response;

            for (let j = 0; j < AnalysisItems.length; j++) {


                AnalysisItems[j].Type = 'Analysis Item';
                if (AnalysisItems[j].Code === null)
                    AnalysisItems[j].Code = "";
                if (AnalysisItems[j].Amount === null)
                    AnalysisItems[j].Amount = 0;
                if (AnalysisItems[j].Discount === null || AnalysisItems[j].Discount === undefined)
                    AnalysisItems[j].Discount = 0;
                AnalysisItems[j].disabled = false;
                AllItems.push(AnalysisItems[j]);

            }

            createAnalysisListItemsGrid();


            let AnalysisItemNameAutoComplete = $("#AnalysisItemNameAutoComplete").data("kendoAutoComplete");
            AnalysisItemNameAutoComplete.setDataSource(AnalysisItems);

            let list = [];
            //list = list.concat(AnalysisItems);
            //let index = list.findIndex(a => a.Code == "");
            //if (index > -1) {
            //    list.splice(index, 1);
            //}

            list = AnalysisItems.filter(function (value, index, arr) {
                return value.Code !== '';
            });

            let AnalysisItemCodeAutoComplete = $("#AnalysisItemCodeAutoComplete").data("kendoAutoComplete");
            AnalysisItemCodeAutoComplete.setDataSource(list);


        }
    });

}

function groupAjax(id) {

    $('#groupAnalysisButton').html('<div style="margin-right: auto; display: inline-block; position: absolute;left:50%;top:30%"><img src = "../Content/kendo/css/Default/loading_2x.gif" /></div>');
    $('#groupAnalysisList').html('<div style="margin-right: auto; display: inline-block; position: absolute;left:50%;top:30%"><img src = "../Content/kendo/css/Default/loading_2x.gif" /></div>');


    return $.ajax({
        type: "Post",
        data: { ClinicSectionId: id },
        url: "/GroupAnalysis/GetAllGroupAnalysisByClinicSectionId",
        success: function (response) {
            groupAnalysis = response;

            for (let j = 0; j < groupAnalysis.length; j++) {

                groupAnalysis[j].Type = 'Group Analysis';
                if (groupAnalysis[j].Code === null)
                    groupAnalysis[j].Code = "";
                if (groupAnalysis[j].Amount === null)
                    groupAnalysis[j].Amount = 0;
                if (groupAnalysis[j].Discount === null)
                    groupAnalysis[j].Discount = 0;
                groupAnalysis[j].disabled = false;
                AllItems.push(groupAnalysis[j]);

            }

            //createAllNameAutoComplete();
            //createAllCodeAutoComplete();
            createGroupAnalysisGrid();

            let GroupAnalysisNameAutoComplete = $("#GroupAnalysisNameAutoComplete").data("kendoAutoComplete");
            GroupAnalysisNameAutoComplete.setDataSource(groupAnalysis);

            let list = [];
            //list = list.concat(groupAnalysis);
            //let index = list.findIndex(a => a.Code == "");
            //if (index > -1) {
            //    list.splice(index, 1);
            //}

            list = groupAnalysis.filter(function (value, index, arr) {
                return value.Code !== '';
            });

            let GroupAnalysisCodeAutoComplete = $("#GroupAnalysisCodeAutoComplete").data("kendoAutoComplete");
            GroupAnalysisCodeAutoComplete.setDataSource(list);
        }
    });

}

let first = true;

function selectClinicSection(e) {





    let id = e.dataItem.Guid;


    $("#analysisList").empty();
    $("#analysisItemList").empty();
    $("#groupAnalysisList").empty();


    AllItems = [];

    $.when(analysisAjax(id), analysisItemAjax(id), groupAjax(id)).done(function (a1, a2, a3, a4) {



        let AllNameAutoComplete = $("#AllNameAutoComplete").data("kendoAutoComplete");
        AllNameAutoComplete.setDataSource(AllItems);

        let list = [];
        //list = list.concat(AllItems);
        //let index = list.findIndex(a => a.Code == "");
        //if (index > -1) {
        //    list.splice(index, 1);
        //}

        list = AllItems.filter(function (value, index, arr) {
            return value.Code !== '';
        });

        let AllCodeAutoComplete = $("#AllCodeAutoComplete").data("kendoAutoComplete");
        AllCodeAutoComplete.setDataSource(list);


        createButtons();

    });


}

function selectClinicSectionFirstTime(id) {





    //let id = e.dataItem.Guid;


    $("#analysisList").empty();
    $("#analysisItemList").empty();
    $("#groupAnalysisList").empty();


    AllItems = [];

    $.when(analysisAjax(id), analysisItemAjax(id), groupAjax(id)).done(function (a1, a2, a3, a4) {



        let AllNameAutoComplete = $("#AllNameAutoComplete").data("kendoAutoComplete");
        AllNameAutoComplete.setDataSource(AllItems);

        let list = [];
        //list = list.concat(AllItems);
        //let index = list.findIndex(a => a.Code == "");
        //if (index > -1) {
        //    list.splice(index, 1);
        //}

        list = AllItems.filter(function (value, index, arr) {
            return value.Code !== '';
        });

        let AllCodeAutoComplete = $("#AllCodeAutoComplete").data("kendoAutoComplete");
        AllCodeAutoComplete.setDataSource(list);


        createButtons();

    });


}



function ChosseAnalysisModal() {

    $('#ChosseAnalysisModal').modal('toggle');

    createChooseAnalysisTabs();

}

function createChooseAnalysisTabs() {

    $('#groupAnalysisListChooseAnalysis').empty();
    $('#analysisListChooseAnalysis').empty();
    $('#analysisItemListChooseAnalysis').empty();


    let allgroupAnalysis = "";

    //groupAnalysis.sort(function (a, b) { return a.Priority - b.Priority });
    groupAnalysis.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

    for (let i = 0; i < groupAnalysis.length; i++) {
        if (groupAnalysis[i].IsButton) {
            allgroupAnalysis = allgroupAnalysis + "<div class='col-ms-12' style='padding:0.5rem;border:solid 1px rgba(0,0,0,0.8);border-radius:1rem'><span class='fa fa-angle-up k-primary' style = 'padding-top:0.3rem;padding-buttom:0.3rem' onclick = 'IncreasePriority(this)' data-id='" + groupAnalysis[i].Guid + " data-type='Group' ></span ><span class='fa fa-angle-down k-primary' style='margin:0 0.5rem;padding-top:0.3rem;padding-buttom:0.3rem' data-id='" + groupAnalysis[i].Guid + "' data-type='Group' onclick='DicreasePriority(this)'></span><input type = 'checkbox' id = 'eq" + i + "' class='k-checkbox' data-id='" + groupAnalysis[i].Guid + "' data-type='Group' checked = 'checked' onchange='checkBoxChange(this)'>\
                <label class='k-checkbox-label' for= 'eq"+ i + "' >" + groupAnalysis[i].Name + "</label></div>";
        }
        else {
            allgroupAnalysis = allgroupAnalysis + "<div class='col-ms-12' style='padding:0.5rem;border:solid 1px rgba(0,0,0,0.8);border-radius:1rem'><span class='fa fa-angle-up k-primary' style='padding-top:0.3rem;padding-buttom:0.3rem' onclick='IncreasePriority(this)' data-id='" + groupAnalysis[i].Guid + " data-type='Group'></span><span class='fa fa-angle-down k-primary' style='margin:0 0.5rem;padding-top:0.3rem;padding-buttom:0.3rem' data-id='" + groupAnalysis[i].Guid + "' data-type='Group'  onclick='DicreasePriority(this)' ></span><input type='checkbox' id='eq" + i + "' data-id='" + groupAnalysis[i].Guid + "' class='k-checkbox' data-type='Group' onchange='checkBoxChange(this)'>\
                <label class='k-checkbox-label' for= 'eq"+ i + "' >" + groupAnalysis[i].Name + "</label></div>";
        }

    }

    $('#groupAnalysisListChooseAnalysis').append("<div class='col-sm-12' style=' padding: 0' >" + allgroupAnalysis + "</div>");

    let allAnalysis = "";

    //Analysis.sort(function (a, b) { return a.Priority - b.Priority });
    Analysis.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

    for (let i = 0; i < Analysis.length; i++) {
        if (Analysis[i].IsButton) {
            allAnalysis = allAnalysis + "<div class='col-ms-12' style='padding:0.5rem;border:solid 1px rgba(0,0,0,0.8);border-radius:1rem'><span class='fa fa-angle-up k-primary' style='padding-top:0.3rem;padding-buttom:0.3rem' onclick='IncreasePriority(this)' data-id='" + Analysis[i].Guid + "' data-type='Analysis' ></span><span class='fa fa-angle-down k-primary' style='margin:0 0.5rem;padding-top:0.3rem;padding-buttom:0.3rem' data-id='" + Analysis[i].Guid + "' data-type='Analysis'  onclick='DicreasePriority(this)'></span><input type='checkbox' id='eq" + i + "' class='k-checkbox' data-id='" + Analysis[i].Guid + "' data-type='Analysis' checked='checked' onchange='checkBoxChange(this)'>\
                <label class='k-checkbox-label' for= 'eq"+ i + "' >" + Analysis[i].Name + "</label></div>";
        }
        else {
            allAnalysis = allAnalysis + "<div class='col-ms-12' style='padding:0.5rem;border:solid 1px rgba(0,0,0,0.8);border-radius:1rem'><span class='fa fa-angle-up k-primary' style='padding-top:0.3rem;padding-buttom:0.3rem' onclick='IncreasePriority(this)' data-id='" + Analysis[i].Guid + "' data-type='Analysis' ></span><span class='fa fa-angle-down k-primary' style='margin:0 0.5rem;padding-top:0.3rem;padding-buttom:0.3rem' data-id='" + Analysis[i].Guid + "'  data-type='Analysis' onclick='DicreasePriority(this)'></span><input type='checkbox' id='eq" + i + "' data-id='" + Analysis[i].Guid + "' data-type='Analysis' class='k-checkbox' onchange='checkBoxChange(this)'>\
                <label class='k-checkbox-label' for= 'eq"+ i + "' >" + Analysis[i].Name + "</label></div>";
        }

    }

    $('#analysisListChooseAnalysis').append("<div class='col-sm-12' style=' padding: 0' >" + allAnalysis + "</div>");


    let allAnalysisItem = "";

    //AnalysisItems.sort(function (a, b) { return a.Priority - b.Priority });
    AnalysisItems.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));


    for (let i = 0; i < AnalysisItems.length; i++) {
        if (AnalysisItems[i].IsButton) {
            allAnalysisItem = allAnalysisItem + "<div class='col-ms-12' style='padding:0.5rem;border:solid 1px rgba(0,0,0,0.8);border-radius:1rem'><span class='fa fa-angle-up k-primary' style='padding-top:0.3rem;padding-buttom:0.3rem' onclick='IncreasePriority(this)' data-id='" + AnalysisItems[i].Guid + "'  data-type='AnalysisItem' ></span><span class='fa fa-angle-down k-primary' style='margin:0 0.5rem;padding-top:0.3rem;padding-buttom:0.3rem' data-id='" + AnalysisItems[i].Guid + "' data-type='AnalysisItem' onclick='DicreasePriority(this)'></span><input type='checkbox' id='eq" + i + "' class='k-checkbox' data-id='" + AnalysisItems[i].Guid + "' data-type='AnalysisItem' checked='checked' onchange='checkBoxChange(this)'>\
                <label class='k-checkbox-label' for= 'eq"+ i + "' >" + AnalysisItems[i].Name + "</label></div>";
        }
        else {
            allAnalysisItem = allAnalysisItem + "<div class='col-ms-12' style='padding:0.5rem;border:solid 1px rgba(0,0,0,0.8);border-radius:1rem'><span class='fa fa-angle-up k-primary' style='padding-top:0.3rem;padding-buttom:0.3rem' onclick='IncreasePriority(this)' data-id='" + AnalysisItems[i].Guid + "' data-type='AnalysisItem'></span><span class='fa fa-angle-down k-primary' style='margin:0 0.5rem;padding-top:0.3rem;padding-buttom:0.3rem' data-id='" + AnalysisItems[i].Guid + "' data-type='AnalysisItem' onclick='DicreasePriority(this)'></span><input type='checkbox' id='eq" + i + "' data-id='" + AnalysisItems[i].Guid + "' class='k-checkbox' data-type='AnalysisItem' onchange='checkBoxChange(this)'>\
                <label class='k-checkbox-label' for= 'eq"+ i + "' >" + AnalysisItems[i].Name + "</label></div>";
        }

    }

    $('#analysisItemListChooseAnalysis').append("<div class='col-sm-12' style=' padding: 0' >" + allAnalysisItem + "</div>");

}

function checkBoxChange(element) {

    let Guid = $(element).attr('data-id');
    let type = $(element).attr('data-type');
    if (type === "Group") {
        let id = groupAnalysis.find(x => x.Guid == Guid);
        id.IsButton = !id.IsButton;
    }
    else if (type === "Analysis") {
        let id = Analysis.find(x => x.Guid == Guid);
        id.IsButton = !id.IsButton;
    }
    else {
        let id = AnalysisItems.find(x => x.Guid == Guid);
        id.IsButton = !id.IsButton;
    }



}

function IncreasePriority(element) {
    let Guid = $(element).attr('data-id');
    let type = $(element).attr('data-type');
    if (type === "Group") {
        let id = groupAnalysis.find(x => x.Guid == Guid);
        if (id.Priority === 1) {
            return;
        }
        let next = groupAnalysis.find(x => x.Priority == id.Priority - 1);
        id.Priority--;

        next.Priority++;
    }
    else if (type === "Analysis") {
        let id = Analysis.find(x => x.Guid == Guid);
        if (id.Priority === 1) {
            return;
        }
        let next = Analysis.find(x => x.Priority == id.Priority - 1);
        id.Priority--;

        next.Priority++;
    }
    else {

        let id = AnalysisItems.find(x => x.Guid == Guid);
        if (id.Priority === 1) {
            return;
        }
        let next = AnalysisItems.find(x => x.Priority == id.Priority - 1);
        id.Priority--;

        next.Priority++;

    }



    createChooseAnalysisTabs();
    createButtons();
}

function DicreasePriority(element) {

    let Guid = $(element).attr('data-id');

    let type = $(element).attr('data-type');
    if (type === "Group") {
        let id = groupAnalysis.find(x => x.Guid == Guid);
        if (id.Priority === groupAnalysis.length) {
            return;
        }
        let next = groupAnalysis.find(x => x.Priority == id.Priority + 1);
        id.Priority++;

        next.Priority--;
    }
    else if (type === "Analysis") {

        let id = Analysis.find(x => x.Guid == Guid);
        if (id.Priority === Analysis.length) {
            return;
        }
        let next = Analysis.find(x => x.Priority == id.Priority + 1);
        id.Priority++;

        next.Priority--;

    }
    else {

        let id = AnalysisItems.find(x => x.Guid == Guid);
        if (id.Priority === AnalysisItems.length) {
            return;
        }
        let next = AnalysisItems.find(x => x.Priority == id.Priority + 1);
        id.Priority++;

        next.Priority--;

    }

    createChooseAnalysisTabs();
    createButtons();
}

function ChosseAnalysisAccept() {

    var clinicSectionId = $("#ClinicSectionId").val();

    createChooseAnalysisTabs();
    createButtons();

    let allGroupAnalysisChecked = [];
    let allGroupAnalysisSelected = [];

    $('#groupAnalysisListChooseAnalysis input:checked').each(function () {
        allGroupAnalysisChecked.push($(this).attr('data-id'));
    });

    for (let i = 0; i < groupAnalysis.length; i++) {
        groupAnalysis[i].IsButton = false;
    }

    for (let i = 0; i < allGroupAnalysisChecked.length; i++) {
        let id = groupAnalysis.find(x => x.Guid == allGroupAnalysisChecked[i]);
        id.IsButton = true;
        allGroupAnalysisSelected.push(id);
    }


    let allAnalysisChecked = [];
    let allAnalysisSelected = [];

    $('#analysisListChooseAnalysis input:checked').each(function () {
        allAnalysisChecked.push($(this).attr('data-id'));
    });

    for (let i = 0; i < Analysis.length; i++) {
        Analysis[i].IsButton = false;
    }

    for (let i = 0; i < allAnalysisChecked.length; i++) {
        let id = Analysis.find(x => x.Guid == allAnalysisChecked[i]);
        id.IsButton = true;
        allAnalysisSelected.push(id);
    }


    let allAnalysisItemChecked = [];
    let allAnalysisItemSelected = [];

    $('#analysisItemListChooseAnalysis input:checked').each(function () {
        allAnalysisItemChecked.push($(this).attr('data-id'));
    });

    for (let i = 0; i < AnalysisItems.length; i++) {
        AnalysisItems[i].IsButton = false;
    }

    for (let i = 0; i < allAnalysisItemChecked.length; i++) {
        let id = AnalysisItems.find(x => x.Guid == allAnalysisItemChecked[i]);
        id.IsButton = true;
        allAnalysisItemSelected.push(id);
    }



    createButtons();
    $('#ChosseAnalysisModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#groupAnalysisListChooseAnalysis').empty();
    $('#analysisListChooseAnalysis').empty();
    $('#analysisItemListChooseAnalysis').empty();

    $.ajax({
        type: "Post",
        data: {
            clinicSectionId: clinicSectionId,
            allGroup: allGroupAnalysisSelected
            //allGroup: groupAnalysis
        },
        url: "/GroupAnalysis/UpdateGroupAnalysisButtonAndPriority",

    });

    $.ajax({
        type: "Post",
        data: {
            clinicSectionId: clinicSectionId,
            allAnalysis: allAnalysisSelected
            //allAnalysis: Analysis
        },
        url: "/Analysis/UpdateAnalysisButtonAndPriority",

    });

    $.ajax({
        type: "Post",
        data: {
            clinicSectionId: clinicSectionId,
            allAnalysisItem: allAnalysisItemSelected
        },
        url: "/AnalysisItem/UpdateAnalysisItemButtonAndPriority",

    });

}

function closeChosseAnalysisModal() {

    $('#ChosseAnalysisModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#groupAnalysisListChooseAnalysis').empty();
    $('#analysisListChooseAnalysis').empty();
    $('#analysisItemListChooseAnalysis').empty();
}

function createButtons() {

    $('#groupAnalysisButton').empty();
    $('#analysisButton').empty();
    $('#analysisItemButton').empty();


    let groupButtons = "";

    for (let i = 0; i < groupAnalysis.length; i++) {
        if (groupAnalysis[i].IsButton) {
            groupButtons = groupButtons +
                "<div style='margin:1rem 0.5rem;display:inline-block'><a class='analysisButtons' style='padding:0.5rem' onclick='AddButtonToSelectedGrid(this)' data-Guid='" +
                groupAnalysis[i].Guid + "' data-Type='Group Analysis' data-Name='" +
                groupAnalysis[i].Name + "' data-Code='" +
                groupAnalysis[i].Code + "' data-Price='" +
                groupAnalysis[i].Price + "'  data-Discount='" +
                groupAnalysis[i].Discount + "'>" +
                groupAnalysis[i].Name + "</a></div>";
        }

    }

    $('#groupAnalysisButton').append("<div class='col-sm-12' style=' padding: 0;height:8rem' >" + groupButtons + "</div>");


    let analysisButtons = "";

    for (let i = 0; i < Analysis.length; i++) {
        if (Analysis[i].IsButton) {
            analysisButtons = analysisButtons +
                "<div style='margin:1rem 0.5rem;display:inline-block'><a class='analysisButtons' style='padding:0.5rem' onclick='AddButtonToSelectedGrid(this)' data-Guid='" +
                Analysis[i].Guid + "' data-Type='Analysis' data-Name='" +
                Analysis[i].Name + "' data-Code='" +
                Analysis[i].Code + "' data-Price='" +
                Analysis[i].Price + "'  data-Discount='" +
                Analysis[i].Discount + "'>" +
                Analysis[i].Name + "</a></div>";
        }

    }

    $('#analysisButton').append("<div class='col-sm-12' style=' padding: 0;height:8rem' >" + analysisButtons + "</div>");


    let analysisItemButtons = "";

    for (let i = 0; i < AnalysisItems.length; i++) {
        if (AnalysisItems[i].IsButton) {
            analysisItemButtons = analysisItemButtons +
                "<div style='margin:1rem 0.5rem;display:inline-block'><a class='analysisButtons' style='padding:0.5rem' onclick='AddButtonToSelectedGrid(this)' data-Guid='" +
                AnalysisItems[i].Guid + "' data-Type='Analysis Item' data-Name='" +
                AnalysisItems[i].Name + "' data-Code='" +
                AnalysisItems[i].Code + "' data-Price='" +
                AnalysisItems[i].Price + "'  data-Discount='" +
                AnalysisItems[i].Discount + "'>" +
                AnalysisItems[i].Name + "</a></div>";
        }

    }

    $('#analysisItemButton').append("<div class='col-sm-12' style=' padding: 0 ;height:8rem' >" + analysisItemButtons + "</div>");
}

function AddButtonToSelectedGrid(element) {
    let Item = {
        Guid: $(element).attr('data-Guid'),
        Type: $(element).attr('data-Type'),
        Name: $(element).attr('data-Name'),
        Code: $(element).attr('data-Code'),
        Price: $(element).attr('data-Price'),
        Discount: $(element).attr('data-Discount')
    };


    AddToGridFromAutoCompelets(Item);
}

function AddSpeciallity() {

    Address = false;
    var link = "/BaseInfo/AddNewModal";
    $(".loader").removeClass("hidden");
    $('#AddressModal').modal('toggle');
    $('#AddressModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");

    });


}


function addNewAddress() {
    if ($("#Name").val() === "") {
        $("#Name-box").removeClass('hidden');
        return;
    }

    var AddressId;
    if (Address)
        AddressId = $("#Address").attr('data-Value');
    else
        AddressId = $("#Speciallity").attr('data-Value');


    $("#TypeId").val(AddressId);

    var link = "/BaseInfo/AddOrUpdate";
    var GridRefreshLink = "/BaseInfo/RefreshGrid";
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


                    //var BaseInfoId = response;

                    //$.ajax({
                    //    type: "Post",
                    //    data: { BaseInfoId: BaseInfoId, BaseInfoTypeId: AddressId },
                    //    url: "/BaseInfo/AddBaseInfoType",
                    //    success: function (response) {
                    //        if (Address) {
                    //            addressAuto.dataSource.read();
                    //        }
                    //        else
                    //            SpeciallityDropDown.dataSource.read();
                    //    }
                    //});

                    SpeciallityDropDown.dataSource.read();

                    $('#AddressModal').modal('hide');
                    $(".modal-backdrop:last").remove();
                    $('#AddressModal-body').empty();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    SpeciallityDropDown.focus();
                    SpeciallityDropDown.open();
                }
            }
        }
    });
}


function CloseModal() {
    $('#AddressModal').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#AddressModal-body').empty();
}

window.onafterprint = function () {

    $("#Exit").focus();
}


function draw2(imgData) {

    "use strict";

    var dataUrl = [];
    for (let index = 0; index < imgData.allb.length; index++) {

        //var uInt8Array = imgData.allb[index];
        //var i = uInt8Array.length;
        //var binaryString = [i];
        //while (i--) {
        //    binaryString[i] = String.fromCharCode(uInt8Array[i]);
        //}

        //var data = binaryString.join('');
        //var base64 = window.btoa(data);

        var img = new Image();
        img.src = "data:image/jpeg;base64," + imgData.allb[index];
        dataUrl.push(img.src)


    }

    printJS({ printable: dataUrl, type: 'image' });



}





function KeyPressAndFocusConfigs() {

    $("#Name2").on('focus', function (e) {
        $("#Name2-box").addClass('hidden');
        let value = nameAuto.value();
        nameAuto.search(value);
    });


    $("#Doctor").on('focus', function (e) {
        $("#Doctor-box").addClass('hidden');
        let value = doctorAuto.value();
        doctorAuto.search(value);
    });


    $("#RadiologyDoctor").on('focus', function (e) {

        let value = radiologyDoctorAuto.value();
        radiologyDoctorAuto.search(value);
    });

    $("#AddressName").on('focus', function (e) {
        let AddressAuto = $("#AddressName").data("kendoAutoComplete");
        let value = AddressAuto.value();
        AddressAuto.search(value);
    });

    $("#AllNameAutoComplete").on('focus', function (e) {
        let AllNameAuto = $("#AllNameAutoComplete").data("kendoAutoComplete");
        let value = AllNameAuto.value();
        AllNameAuto.search(value);
    });

    $("#AllCodeAutoComplete").on('focus', function (e) {
        let AllCode = $("#AllCodeAutoComplete").data("kendoAutoComplete");
        let value = AllCode.value();
        AllCode.search(value);
    });

    $("#GroupAnalysisNameAutoComplete").on('focus', function (e) {
        let GroupAnalysisName = $("#GroupAnalysisNameAutoComplete").data("kendoAutoComplete");
        let value = GroupAnalysisName.value();
        GroupAnalysisName.search(value);
    });


    $("#GroupAnalysisCodeAutoComplete").on('focus', function (e) {
        let GroupAnalysisCode = $("#GroupAnalysisCodeAutoComplete").data("kendoAutoComplete");
        let value = GroupAnalysisCode.value();
        GroupAnalysisCode.search(value);
    });


    $("#AnalysisNameAutoComplete").on('focus', function (e) {
        let AnalysisName = $("#AnalysisNameAutoComplete").data("kendoAutoComplete");
        let value = AnalysisName.value();
        AnalysisName.search(value);
    });


    $("#AnalysisCodeAutoComplete").on('focus', function (e) {
        let AnalysisCode = $("#AnalysisCodeAutoComplete").data("kendoAutoComplete");
        let value = AnalysisCode.value();
        AnalysisCode.search(value);
    });


    $("#AnalysisItemNameAutoComplete").on('focus', function (e) {
        let AnalysisItemName = $("#AnalysisItemNameAutoComplete").data("kendoAutoComplete");
        let value = AnalysisItemName.value();
        AnalysisItemName.search(value);
    });

    $("#AnalysisItemCodeAutoComplete").on('focus', function (e) {
        let AnalysisItemCode = $("#AnalysisItemCodeAutoComplete").data("kendoAutoComplete");
        let value = AnalysisItemCode.value();
        AnalysisItemCode.search(value);
    });

    $("#AnalysisAutoComplete").on('focus', function (e) {
        let Analysis = $("#AnalysisAutoComplete").data("kendoAutoComplete");
        let value = Analysis.value();
        Analysis.search(value);
    });


    $('#Name2').on('keypress', function (e) {
        if (e.which === 13) {
            $("#PhoneNumber").focus();
        }
    });


    var Numbers = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "/", ".", "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩", " "];

    var EnglishKey = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "q", "Q", "w", "e", "E", "r", "R", "t", "T", "y", "Y", "u", "U", "i", "I", "o", "O", "p", "P", "a"
        , "A", "s", "S", "d", "D", "f", "F", "g", "G", "h", "H", "j", "J", "k", "K", "l", "L", "z", "Z", "x", "X", "c", "C", "v", "V", "b", "B", "n", "N", "m", "M"];

    var KurdishKey = [1632, 1633, 1634, 1635, 1636, 1637, 1638, 1639, 1640, 1641, 1602, 96, 1608, 1749, 1610, 1585, 1685, 1578, 1591, 1740, 1742, 1574, 1569
        , 1581, 1593, 1734, 1572, 1662, 1579, 1575, 1570, 1587, 1588, 1583, 1584, 1601, 1573, 1711, 1594, 1607, 8204, 1688, 1571, 1705, 1603, 1604
        , 1717, 1586, 1590, 1582, 1589, 1580, 1670, 1700, 1592, 1576, 1609, 1606, 1577, 1605, 1600
    ];


    $('#PhoneNumber').on('keypress', function (e) {

        let gender = $("#GenderId").data("kendoDropDownList");

        let char = Numbers.indexOf(e.key)

        if (char !== -1 || e.which === 13 || e.which === 9) {

            let index = KurdishKey.indexOf(e.which);
            if (e.which === 13 || e.which === 9) {
                gender.focus();
                gender.open();


            }
            else if (index !== -1) {
                e.preventDefault();
                let index1 = EnglishKey[index];
                let preVal = $(this).val();
                $(this).val(preVal + index1);
            }
        }
        else {
            e.preventDefault();
        }


    });

    $('#GenderId').parent().on('keypress', function (e) {

        if (e.which === 13) {
            let date = $("#DateOfBirth").data("kendoDatePicker");
            date.element.focus();
        }
    });

    $('#DateOfBirth').on('focus', function (e) {
        let date = $("#DateOfBirth").data("kendoDatePicker");
        date.element.select();
    });

    $('#DateOfBirth').on('keypress', function (e) {

        if (e.which === 13) {
            $("#Doctor").focus();
        }
    });

    $('#Year').on('keypress', function (e) {

        if (e.which === 13) {
            $("#Month").focus();
        }
    });

    $('#Year').on('focus', function (e) {

        $("#Year").select();
    });

    $('#Month').on('keypress', function (e) {

        if (e.which === 13) {
            $("#Doctor").focus();
        }
    });

    $('#Month').on('focus', function (e) {

        $("#Month").select();
    });



    $('#Doctor').on('keypress', function (e) {

        if (e.which === 13) {
            let speciallity = $("#Speciality").data("kendoDropDownList");
            speciallity.focus();
            speciallity.open();
        }
    });


    $('#Speciality').parent().on('keypress', function (e) {

        $('#Explanation').focus();


    });


    $('#AllNameAutoComplete').on('keypress', function (e) {

        if (e.which === 13) {
            let numerictextbox = $("#Discount").data("kendoNumericTextBox");
            numerictextbox.focus();

        }
    });

    $('#AllNameAutoComplete').on('keydown', function (e) {

        if (e.which === 9) {
            e.preventDefault();
            let numerictextbox = $("#Discount").data("kendoNumericTextBox");
            numerictextbox.focus();

        }
    });


    $('#Discount').on('keypress', function (e) {

        if (e.which === 13) {
            let numerictextbox = $("#DiscountAmount").data("kendoNumericTextBox");
            numerictextbox.focus();
            //$("#DiscountAmount").focus();
        }
    });

    $('#Discount').on('focus', function (e) {

        var input = $(this);
        setTimeout(function () {
            input.select();
        });
    });

    $('#DiscountAmount').on('keypress', function (e) {
        if (e.which === 13) {
            let numerictextbox = $("#PatientReceptionReceivedAmount").data("kendoNumericTextBox");
            numerictextbox.focus();
            //$("#PatientReceptionReceivedAmount").focus();
        }
    });

    $('#DiscountAmount').on('focus', function (e) {

        var input = $(this);
        setTimeout(function () {
            input.select();
        });
    });

    $('#PatientReceptionReceivedAmount').on('keydown', function (e) {

        if (e.which === 13) {
            let value = $('#PatientReceptionReceivedAmount').val();
            if (value === 0 || value === "" || value === null || value === undefined || value === "0") {
                $("#btnPrint").focus();
            }
            else {
                addNewPatientReceptionReceived();
                $("#btnPrint").focus();
            }

        }
        else if (e.which === 9) {

            e.preventDefault();
            $("#PatientReceptionReceivedAmountDollar").focus();
        }
    });

    $('#PatientReceptionReceivedAmountDollar').on('keydown', function (e) {

        if (e.which === 13) {

            let value = $('#PatientReceptionReceivedAmountDollar').val();
            if (value === 0 || value === "" || value === null || value === undefined || value === "0") {
                $("#btnPrint").focus();
            }
            else {
                addNewPatientReceptionReceivedDollar();
                $("#btnPrint").focus();
            }

        }
        else if (e.which === 9) {

            e.preventDefault();
            $("#btnPrint").focus();
        }
    });



    $('#PatientReceptionReceivedAmount').on('focus', function (e) {

        var input = $(this);
        setTimeout(function () {
            input.select();
        });
    });

    $('#PatientReceptionReceivedAmountDollar').on('focus', function (e) {

        var input = $(this);
        setTimeout(function () {
            input.select();
        });
    });

    $('#Explanation').on('keydown', function (e) {

        if (e.which === 9) {
            e.preventDefault();
            $("html, body").animate({ scrollTop: 450 });
            setTimeout(function () {
                $("#AllNameAutoComplete").focus();
            }, 500);


        }

    });


    //$('#ReceptionNumber').on('keydown', function (e) {

    //    if (e.which === 13) {

    //        GoToCustomReception("value");
    //    }

    //});


    $('#btnOkNew').on('keydown', function (e) {

        if (e.which === 37) {
            $("#btnOkExit").focus();
        }

    });

    $('#btnOkExit').on('keydown', function (e) {

        if (e.which === 39) {

            $("#btnOkNew").focus();
        }
        else if (e.which === 37) {
            $("#btnExit").focus();
        }

    });

    $('#btnExit').on('keydown', function (e) {

        if (e.which === 39) {

            $("#btnOkExit").focus();
        }
        else if (e.which === 37) {
            $("#btnPrint").focus();
        }

    });

    $('#btnPrint').on('keydown', function (e) {

        if (e.which === 39) {

            $("#btnExit").focus();
        }


    });


    $('#MoneyConvertAmount').on('keydown', function (e) {

        if (e.which === 13) {

            changeMoneyConvert();
        }

    });



    $('#MoneyConvertAmount').on('focus', function (e) {

        var input = $(this);
        setTimeout(function () {
            input.select();
        });
    });


}



//////////////////Create Grid And AutoCompletes



function createAnalysisGrid() {

    $('#analysisList').html('');

    if (RTL === "True") {
        $("#analysisList").kendoGrid({
            dataSource: {
                data: Analysis,
                schema: {
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Code: { type: "string" },

                        }
                    }
                },
                pageSize: 10
            },
            height: gridHieght,
            //filterable: {
            //    mode: "row"
            //},
            scrollable: true,
            sortable: true,

            pageable: {
                input: true,
                numeric: false
            },
            columns: [
                {
                    template: "<i class='fa fa-arrow-left bigger-120 blue'  > </i>",
                    width: "50px",
                    attributes: {
                        "onclick": "addToSelectedGrid(this)",
                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                        "data-Discount": "#: Discount #",
                        "data-Disabled": "#: disabled #",
                        style: "cursor:pointer"
                    }

                },
                {
                    field: "Code",
                    title: CodeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong >  #: Code #  </strong>",
                    width: "60px",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {

                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                    }

                },
                {
                    field: "Name",
                    title: NameTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong   >  #: Name #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {
                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                    }

                },
                {
                    template: "<strong>  #: Index #  </strong>",
                    width: "30px"
                }

            ],
            selectable: "row"
        });
    }
    else {
        $("#analysisList").kendoGrid({
            dataSource: {
                data: Analysis,
                schema: {
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Code: { type: "string" },

                        }
                    }
                },
                pageSize: 10
            },
            height: gridHieght,
            //filterable: {
            //    mode: "row"
            //},
            scrollable: true,
            sortable: true,

            pageable: {
                input: true,
                numeric: false
            },
            columns: [

                {
                    template: "<strong>  #: Index #  </strong>",
                    width: "30px"
                },
                {
                    field: "Name",
                    title: NameTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong   >  #: Name #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {
                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                    }

                },
                {
                    field: "Code",
                    title: CodeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong >  #: Code #  </strong>",
                    width: "60px",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {

                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                    }

                },
                {
                    template: "<i class='fa fa-arrow-right bigger-120 blue'  > </i>",
                    width: "50px",
                    attributes: {
                        "onclick": "addToSelectedGrid(this)",
                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                        "data-Discount": "#: Discount #",
                        "data-Disabled": "#: disabled #",
                        style: "cursor:pointer"
                    }

                }

            ],
            selectable: "row"
        });
    }




}

function createAnalysisListItemsGrid(element) {

    $('#analysisItemList').html('');

    if (RTL === "True") {

        $("#analysisItemList").kendoGrid({
            dataSource: {

                data: AnalysisItems,
                schema: {
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Code: { type: "string" },

                        }
                    }
                },
                pageSize: 10
            },
            height: gridHieght,
            //filterable: {
            //    mode: "row"
            //},
            scrollable: true,
            sortable: true,

            pageable: {
                input: true,
                numeric: false
            },
            columns: [
                {
                    template: "<i class='fa fa-arrow-left bigger-120 blue'  > </i>",
                    width: "50px",
                    attributes: {
                        "onclick": "addToSelectedGrid(this)",
                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis Item",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                        "data-Disabled": "#: disabled #",
                        "data-Discount": 0,
                        style: "cursor:pointer"
                    }
                },
                {
                    field: "Code",
                    title: CodeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong  data-Guid = '#: Guid #'  data-Type='Analysis'  >  #: Code #  </strong>",
                    width: "60px",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {

                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",

                    }

                },
                {
                    field: "Name",
                    title: NameTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong  data-Guid = '#: Guid #'  data-Type='Analysis'   >  #: Name #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {

                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",

                    }

                },
                {
                    template: "<strong>  #: Index #  </strong>",
                    width: "30px"
                }

            ],
        });

    }
    else {
        $("#analysisItemList").kendoGrid({
            dataSource: {

                data: AnalysisItems,
                schema: {
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Code: { type: "string" },

                        }
                    }
                },
                pageSize: 10
            },
            height: gridHieght,
            //filterable: {
            //    mode: "row"
            //},
            scrollable: true,
            sortable: true,

            pageable: {
                input: true,
                numeric: false
            },
            columns: [


                {
                    template: "<strong>  #: Index #  </strong>",
                    width: "30px"
                },
                {
                    field: "Name",
                    title: NameTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong  data-Guid = '#: Guid #'  data-Type='Analysis'   >  #: Name #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {

                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",

                    }

                },
                {
                    field: "Code",
                    title: CodeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong  data-Guid = '#: Guid #'  data-Type='Analysis'  >  #: Code #  </strong>",
                    width: "60px",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {

                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",

                    }

                },
                {
                    template: "<i class='fa fa-arrow-right bigger-120 blue'  > </i>",
                    width: "50px",
                    attributes: {
                        "onclick": "addToSelectedGrid(this)",
                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis Item",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                        "data-Disabled": "#: disabled #",
                        "data-Discount": 0,
                        style: "cursor:pointer"
                    }
                }

            ],
        });

    }





}

function createGroupAnalysisGrid() {

    $('#groupAnalysisList').html('');


    if (RTL === "True") {

        $("#groupAnalysisList").kendoGrid({
            dataSource: {
                data: groupAnalysis,
                schema: {
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Code: { type: "string" },

                        }
                    }
                },
                pageSize: 10
            },
            height: gridHieght,
            //filterable: {
            //    mode: "row"
            //},
            scrollable: true,
            sortable: true,

            pageable: {
                input: true,
                numeric: false
            },
            columns: [
                {
                    template: "<i class='fa fa-arrow-left bigger-120 blue'  > </i>",
                    width: "50px",
                    attributes: {
                        "onclick": "addToSelectedGrid(this)",
                        "data-Guid": "#: Guid #",
                        "data-Type": "Group Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                        "data-Discount": "#: Discount #",
                        style: "cursor:pointer"
                    }
                },
                {
                    field: "Code",
                    title: CodeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong  data-Guid = '#: Guid #'  data-Type='Analysis'  >  #: Code #  </strong>",
                    width: "60px",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {


                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",

                    }

                },
                {
                    field: "Name",
                    title: NameTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong  data-Guid = '#: Guid #'  data-Type='Analysis'   >  #: Name #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {

                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",

                    }

                },
                {
                    template: "<strong>  #: Index #  </strong>",
                    width: "30px"
                }

            ],
        });


    }
    else {

        $("#groupAnalysisList").kendoGrid({
            dataSource: {
                data: groupAnalysis,
                schema: {
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Code: { type: "string" },

                        }
                    }
                },
                pageSize: 10
            },
            height: gridHieght,
            //filterable: {
            //    mode: "row"
            //},
            scrollable: true,
            sortable: true,

            pageable: {
                input: true,
                numeric: false
            },
            columns: [



                {
                    template: "<strong>  #: Index #  </strong>",
                    width: "30px"
                },
                {
                    field: "Name",
                    title: NameTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong  data-Guid = '#: Guid #'  data-Type='Analysis'   >  #: Name #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {

                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",

                    }

                },
                {
                    field: "Code",
                    title: CodeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong  data-Guid = '#: Guid #'  data-Type='Analysis'  >  #: Code #  </strong>",
                    width: "60px",
                    headerAttributes: {
                        style: "text-align: center"
                    },
                    attributes: {


                        "data-Guid": "#: Guid #",
                        "data-Type": "Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",

                    }

                },
                {
                    template: "<i class='fa fa-arrow-right bigger-120 blue'  > </i>",
                    width: "50px",
                    attributes: {
                        "onclick": "addToSelectedGrid(this)",
                        "data-Guid": "#: Guid #",
                        "data-Type": "Group Analysis",
                        "data-Name": "#: Name #",
                        "data-Code": "#: Code #",
                        "data-Price": "#: Price #",
                        "data-Discount": "#: Discount #",
                        style: "cursor:pointer"
                    }
                },
            ],
        });


    }





}

function createSelectedGrid() {

    //let baseCurrency = $("#BaseCurrencyId").data("kendoDropDownList");
    let format = "{0:n" + dinarDecimal + "}";


    //if (baseCurrency.text() === "IQD")
    //        format = "{0:n" + dinarDecimal + "}";
    //else if (baseCurrency.text() === "$")
    //            format = "{0:n" + dollarDecimal + "}";
    // else if (baseCurrency.text() === "€")
    //             format = "{0:n" + euroDecimal + "}";
    // else if (baseCurrency.text() === "£")
    //             format = "{0:n" + pondDecimal + "}";


    //if (baseCurrencyId === 11)
    //    format = "{0:n" + dinarDecimal + "}";
    //else if (baseCurrencyId === 12)
    //    format = "{0:n" + dollarDecimal + "}";




    if (RTL === "True") {


        $("#SelectedGrid").kendoGrid({
            dataSource: {
                data: selectedItems,

                schema: {
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Type: { type: "string" },
                            Code: { type: "string" },
                            Price: { type: "number" },
                            Discount: { type: "number" },
                            Total: { type: "number" },

                        }
                    }
                },
                sort: { field: "Type", dir: "asc" },
                pageSize: 10
            },

            //filterable: {
            //    mode: "row"
            //},
            scrollable: true,
            sortable: true,

            pageable: {
                input: true,
                numeric: false
            },
            columns: [
                {

                    template: "<i class='fa fa-trash-can bigger-120 red' style: 'cursor:pointer'> </i>",
                    width: "50px",
                    attributes: {
                        "onclick": "removeFromSelectedGrid(this)",
                        "data-Guid": "#: Guid #",
                        "data-Type": "#: Type #",
                        style: "cursor:pointer"
                    }
                },
                {
                    field: "Total",
                    title: TotalTranslated,

                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    format: format,
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Discount",
                    title: DiscountTranslated,
                    format: format,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    width: "80px",
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Price",
                    title: PriceTranslated,
                    format: format,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Code",
                    title: CodeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong >  #: Code #  </strong>",
                    width: "60px",
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Type",
                    title: TypeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong >  #: Type #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Name",
                    title: NameTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong >  #: Name #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },



            ]

        });

    }
    else {


        $("#SelectedGrid").kendoGrid({
            dataSource: {
                data: selectedItems,

                schema: {
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Type: { type: "string" },
                            Code: { type: "string" },
                            Price: { type: "number" },
                            Discount: { type: "number" },
                            Total: { type: "number" },

                        }
                    }
                },
                sort: { field: "Type", dir: "asc" },
                pageSize: 10
            },

            //filterable: {
            //    mode: "row"
            //},
            scrollable: true,
            sortable: true,

            pageable: {
                input: true,
                numeric: false
            },
            columns: [




                {
                    field: "Name",
                    title: NameTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong >  #: Name #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Type",
                    title: TypeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong >  #: Type #  </strong>",
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Code",
                    title: CodeTranslated,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    template: "<strong >  #: Code #  </strong>",
                    width: "60px",
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Price",
                    title: PriceTranslated,
                    format: format,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Discount",
                    title: DiscountTranslated,
                    format: format,
                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    width: "80px",
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {
                    field: "Total",
                    title: TotalTranslated,

                    filterable: {
                        cell: {
                            operator: "contains",
                            suggestionOperator: "contains"
                        }
                    },
                    format: format,
                    headerAttributes: {
                        style: "text-align: center"
                    }
                },
                {

                    template: "<i class='fa fa-trash-can bigger-120 red' style: 'cursor:pointer'> </i>",
                    width: "50px",
                    attributes: {
                        "onclick": "removeFromSelectedGrid(this)",
                        "data-Guid": "#: Guid #",
                        "data-Type": "#: Type #",
                        style: "cursor:pointer"
                    }
                }


            ]

        });

    }

    if (AllMoneyConverts[0] !== null) {
        changeDiscountAmount();
    }



}

function createGroupAnalysisNameAutoComplete() {
    $("#GroupAnalysisNameAutoComplete").kendoAutoComplete({
        dataSource: groupAnalysis,
        dataTextField: "Name",
        filter: "contains",
        placeholder: "Select Name...",
        clearButton: false,
        highlightFirst: true,
        select: selectGroupAnalysisNameAutoComplete

    });
}

function selectGroupAnalysisNameAutoComplete(e) {

    let groupAuto = $("#GroupAnalysisNameAutoComplete").data("kendoAutoComplete");
    let Item = groupAuto.dataItem(e.item.index())
    AddToGridFromAutoCompelets(Item);
    setTimeout(function () {
        groupAuto.value("");
    }, 20);

}

function createGroupAnalysisCodeAutoComplete() {

    //let list = [];
    //list = list.concat(groupAnalysis);
    //let index = list.findIndex(a => a.Code == "");
    //if (index > -1) {
    //    list.splice(index, 1);
    //}
    $("#GroupAnalysisCodeAutoComplete").kendoAutoComplete({
        //dataSource: list,
        dataTextField: "Code",
        filter: "contains",
        placeholder: "Select Code...",
        clearButton: false,
        highlightFirst: true,
        select: selectGroupAnalysisCodeAutoComplete

    });
}

function selectGroupAnalysisCodeAutoComplete(e) {

    let groupAuto = $("#GroupAnalysisCodeAutoComplete").data("kendoAutoComplete");
    let Item = groupAuto.dataItem(e.item.index())

    AddToGridFromAutoCompelets(Item);
    setTimeout(function () {
        groupAuto.value("");
    }, 20);
}

function createAnalysisNameAutoComplete() {
    $("#AnalysisNameAutoComplete").kendoAutoComplete({
        dataSource: Analysis,
        dataTextField: "Name",
        filter: "contains",
        placeholder: "Select Name...",
        clearButton: false,
        highlightFirst: true,
        select: selectAnalysisNameAutoComplete

    });
}

function selectAnalysisNameAutoComplete(e) {

    let groupAuto = $("#AnalysisNameAutoComplete").data("kendoAutoComplete");
    let Item = groupAuto.dataItem(e.item.index())

    AddToGridFromAutoCompelets(Item);
    setTimeout(function () {
        groupAuto.value("");
    }, 20);
}

function resetAnalysisItemAndAllItem() {

    AllAnalysisItems = [];
    AllItems = [];
    for (let j = 0; j < Analysis.length; j++) {
        for (let i = 0; i < Analysis[j].AnalysisItems.length; i++) {
            AllAnalysisItems.push(Analysis[j].AnalysisItems[i]);
            AllItems.push(Analysis[j].AnalysisItems[i]);
        }
        Analysis[j].Type = 'Analysis';
        AllItems.push(Analysis[j]);
    }


    for (let j = 0; j < groupAnalysis.length; j++) {
        groupAnalysis[j].Type = 'Group Analysis'
        AllItems.push(groupAnalysis[j]);

    }
    let groupAuto1 = $("#AnalysisItemNameAutoComplete").data("kendoAutoComplete");
    groupAuto1.setDataSource(AllAnalysisItems);
    let groupAuto2 = $("#AnalysisItemCodeAutoComplete").data("kendoAutoComplete");
    groupAuto2.setDataSource(AllAnalysisItems);
    let groupAuto3 = $("#AllNameAutoComplete").data("kendoAutoComplete");
    groupAuto3.setDataSource(AllItems);
    let groupAuto4 = $("#AllCodeAutoComplete").data("kendoAutoComplete");
    groupAuto4.setDataSource(AllItems);
    let groupAuto5 = $("#AnalysisAutoComplete").data("kendoAutoComplete");
    groupAuto5.setDataSource(Analysis);
}

function createAnalysisCodeAutoComplete() {
    //let list = [];
    //list = list.concat(Analysis);
    //let index = list.findIndex(a => a.Code == "");
    //if (index > -1) {
    //    list.splice(index, 1);
    //}
    $("#AnalysisCodeAutoComplete").kendoAutoComplete({
        //dataSource: list,
        dataTextField: "Code",
        filter: "contains",
        placeholder: "Select Code...",
        clearButton: false,
        highlightFirst: true,
        select: selectAnalysisCodeAutoComplete

    });
}

function selectAnalysisCodeAutoComplete(e) {

    let groupAuto = $("#AnalysisCodeAutoComplete").data("kendoAutoComplete");
    let Item = groupAuto.dataItem(e.item.index())

    AddToGridFromAutoCompelets(Item);
    setTimeout(function () {
        groupAuto.value("");
    }, 20);
}

function createAnalysisItemNameAutoComplete() {
    $("#AnalysisItemNameAutoComplete").kendoAutoComplete({
        //dataSource: AllAnalysisItems,
        dataSource: AnalysisItems,
        dataTextField: "Name",
        filter: "contains",
        placeholder: "Select Name...",
        clearButton: false,
        highlightFirst: true,
        select: selectAnalysisItemNameAutoComplete

    });
}

function selectAnalysisItemNameAutoComplete(e) {

    let groupAuto = $("#AnalysisItemNameAutoComplete").data("kendoAutoComplete");
    let Item = groupAuto.dataItem(e.item.index())

    AddToGridFromAutoCompelets(Item);
    setTimeout(function () {
        groupAuto.value("");
    }, 20);

}

function createAnalysisItemCodeAutoComplete() {
    //let list = [];
    ////list = list.concat(AllAnalysisItems);
    //list = list.concat(AnalysisItems);
    //let index = list.findIndex(a => a.Code == "");
    //if (index > -1) {
    //    list.splice(index, 1);
    //}
    $("#AnalysisItemCodeAutoComplete").kendoAutoComplete({
        //dataSource: list,
        dataTextField: "Code",
        filter: "contains",
        placeholder: "Select Code...",
        clearButton: false,
        highlightFirst: true,
        select: selectAnalysisItemCodeAutoComplete

    });
}

function selectAnalysisItemCodeAutoComplete(e) {

    let groupAuto = $("#AnalysisItemCodeAutoComplete").data("kendoAutoComplete");
    let Item = groupAuto.dataItem(e.item.index())

    AddToGridFromAutoCompelets(Item);
    setTimeout(function () {
        groupAuto.value("");
    }, 20);
}

function addToSelectedGrid(element) {

    let Item = {
        Guid: $(element).attr('data-Guid'),
        Type: $(element).attr('data-Type'),
        Name: $(element).attr('data-Name'),
        Code: $(element).attr('data-Code'),
        Price: $(element).attr('data-Price'),
        disabled: $(element).attr('data-Disabled'),
        Discount: $(element).attr('data-Discount')
    };


    AddToGridFromAutoCompelets(Item);



}

function removeFromSelectedGrid(element) {
    let Guid = $(element).attr('data-Guid');
    let type = $(element).attr('data-Type');
    let index = selectedItems.findIndex(a => a.Guid == Guid);
    if (index > -1) {
        selectedItems.splice(index, 1);
        createSelectedGrid();
    }

}

function AddToGridFromAutoCompelets(Item) {

    let Guid = Item.Guid;
    let type = Item.Type;
    let name = Item.Name;
    let code = Item.Code;
    let price = Item.Price;
    let discount = Item.Discount;
    let disabled = Item.disabled;

    let total = parseFloat(price) - parseFloat(discount);


    let index = selectedItems.findIndex(a => a.Guid == Guid);
    if (index === -1) {
        selectedItems.push({ Guid: Guid, Type: type, Name: name, Code: code, Price: price, Discount: discount, Total: total });

        let grid = $("#SelectedGrid").data("kendoGrid");
        grid.dataSource.read();
    }


    changeDiscountAmount();
}

function createAllNameAutoComplete() {
    $("#AllNameAutoComplete").kendoAutoComplete({
        dataSource: AllItems,
        dataTextField: "Name",
        filter: "contains",
        placeholder: "Select Name...",
        clearButton: false,
        highlightFirst: true,
        select: selectAllNameAutoComplete

    });
}

function selectAllNameAutoComplete(e) {

    let groupAuto = $("#AllNameAutoComplete").data("kendoAutoComplete");
    let Item = groupAuto.dataItem(e.item.index());
    AddToGridFromAutoCompelets(Item);

    setTimeout(function () {
        groupAuto.value("");
        $("#AllNameAutoComplete").focus();
    }, 20);

}

function createAllCodeAutoComplete() {
    //let list = [];
    //list = list.concat(AllItems);
    //let index = list.findIndex(a => a.Code == "");
    //if (index > -1) {
    //    list.splice(index, 1);
    //}
    $("#AllCodeAutoComplete").kendoAutoComplete({
        //dataSource: list,
        dataTextField: "Code",
        filter: "contains",
        placeholder: "Select Code...",
        clearButton: false,
        highlightFirst: true,
        select: selectAllCodeAutoComplete

    });
}

function selectAllCodeAutoComplete(e) {

    let groupAuto = $("#AllCodeAutoComplete").data("kendoAutoComplete");
    let Item = groupAuto.dataItem(e.item.index());
    AddToGridFromAutoCompelets(Item);


    setTimeout(function () {
        groupAuto.value("");
        $("#AllCodeAutoComplete").focus();
    }, 20);
}


function DeleteAllSelectedItems() {

    bootbox.confirm("Do you Want To Delete All Analysis?", function (result) {

        if (!result) {

            return;

        }
        else {

            selectedItems = [];

            createSelectedGrid();

        }
    })




}



//////////////////////////Calculating Part




function changeDiscount() {


    let total = 0;
    let totalWithDiscount = 0;

    for (let i = 0; i < selectedItems.length; i++) {
        total = total + parseFloat(selectedItems[i].Total);
    }

    let Discount = $("#Discount").data("kendoNumericTextBox");
    let DiscountAmount = $("#DiscountAmount").data("kendoNumericTextBox");



    let discount = $("#Discount").val();

    if (discount === null || discount === "" || discount === undefined) {
        discount = 0;
        Discount.value(discount);
    }


    if (discount > 100) {
        discount = 100;
        Discount.value(100);
    }


    discount = parseFloat(discount);


    let amount = Math.round(total * (discount / 100));

    totalWithDiscount = total - amount;

    DiscountAmount.value(amount);

    $("#totalAmount").val(total.toLocaleString());
    $("#totalAmountWithDiscount").val(totalWithDiscount.toLocaleString());

    calculateTotalReceived(totalWithDiscount);
}

function changeDiscountAmount() {

    let total = 0;
    let totalWithDiscount = 0;

    for (let i = 0; i < selectedItems.length; i++) {
        total = total + parseFloat(selectedItems[i].Total);
    }


    let Discount = $("#Discount").data("kendoNumericTextBox");
    let DiscountAmount = $("#DiscountAmount").data("kendoNumericTextBox");


    let discountAmount = $("#DiscountAmount").val();

    if (discountAmount === null || discountAmount === "" || discountAmount === undefined) {
        discountAmount = 0;
        DiscountAmount.value(discountAmount);
        Discount.value(0);
    }


    if (discountAmount > total) {
        discountAmount = total;
        DiscountAmount.value(total);
    }


    discountAmount = parseFloat(discountAmount);

    totalWithDiscount = total - discountAmount;

    let amount = (discountAmount / total) * 100;

    if (isNaN(amount))
        Discount.value(0);
    else
        Discount.value(amount);


    $("#totalAmount").val(total.toLocaleString());
    $("#totalAmountWithDiscount").val(totalWithDiscount.toLocaleString());


    calculateTotalReceived(totalWithDiscount);
}

function totalReceived(totalWithDiscount) {

    TotalReceived = 0;
    let destCurrency = 11;
    let exist = true;

    for (let i = 0; i < ReceivedAmount.length; i++) {
        if (ReceivedAmount[i].AmountCurrencyId !== destCurrency) {
            let item = AllMoneyConverts.find(a => a.BaseCurrencyId == ReceivedAmount[i].AmountCurrencyId && a.DestCurrencyId == destCurrency);
            if (item === undefined)
                item = AllMoneyConverts.find(a => a.BaseCurrencyId == destCurrency && a.DestCurrencyId == ReceivedAmount[i].AmountCurrencyId);
            if (item === undefined) {
                exist = false;
                GetConvert(ReceivedAmount[i].AmountCurrencyId, destCurrency, totalWithDiscount);
            }



        }

    }

    if (exist)
        calculateTotalReceived(totalWithDiscount);


}


function calculateTotalReceived(totalWithDiscount) {

    TotalReceived = 0;

    for (let i = 0; i < ReceivedAmount.length; i++) {
        TotalReceived = TotalReceived + parseFloat(ReceivedAmount[i].Amount);
    }

    let remainedAmount = parseFloat(totalWithDiscount) - TotalReceived;

    $("#RemainedAmount").val(remainedAmount.toLocaleString());

    let Receive = $("#PatientReceptionReceivedAmount").data("kendoNumericTextBox");
    Receive.value(remainedAmount);


    $("#TotalReceived").val(TotalReceived.toLocaleString());
}


//////////////Other Functions


function changeMoneyConvert() {

    let amount = $("#MoneyConvertAmount").data("kendoNumericTextBox").value();


    if (amount === 0 || amount === "" || amount === null) {

        bootbox.alert({
            message: "please Insert Amount",
            className: 'bootbox-class'

        });
        return;

    }

    var token = $(':input:hidden[name*="RequestVerificationToken"]');


    $.ajax({
        type: "Post",
        data: { __RequestVerificationToken: token.attr('value'), BaseCurrencyId: 12, DestCurrencyId: 11, Amount: amount },
        url: "/MoneyConvert/AddOrUpdate",
        success: function (response) {
            AllMoneyConverts = [];
            AllMoneyConverts.push({ BaseCurrencyId: 12, DestCurrencyId: 11, Amount: amount / 100, Exist: false });
            changeDiscountAmount();

        }
    });

}

function AddMoneyConvertModal() {
    var link = "/MoneyConvert/AddNewModal";
    $(".loader").removeClass("hidden");
    $('#MoneyConvertModal').modal('toggle');
    $('#MoneyConvertModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        $("#Amount").data("kendoNumericTextBox").focus();
    });
}



function addNewMoneyConvert() {


    let amount = $("#Amount").data("kendoNumericTextBox").value();


    if (amount === 0 || amount === "" || amount === null) {

        bootbox.alert({
            message: "please Insert Amount",
            className: 'bootbox-class'

        });
        return;

    }

    var token = $(':input:hidden[name*="RequestVerificationToken"]');


    $.ajax({
        type: "Post",
        data: { __RequestVerificationToken: token.attr('value'), BaseCurrencyId: 12, DestCurrencyId: 11, Amount: amount },
        url: "/MoneyConvert/AddOrUpdate",
        success: function (response) {
            AllMoneyConverts = [];
            AllMoneyConverts.push({ BaseCurrencyId: 12, DestCurrencyId: 11, Amount: amount / 100, Exist: false });

            $('#MoneyConvertModal').modal('toggle');
            $('#MoneyConvertModal-body').empty();
        }
    });


}



//function GoToCustomReception(number) {

//    let currentInvoiceNum = parseInt($('#InvoiceNum').val());
//    let LastNum = parseInt($('#LastInvoiceNum').val());
//    let FirstNum = parseInt($('#FirstInvoiceNum').val());
//    let wantedInvoiceNum = 0;

//    if (number === "Next") {

//        if (currentInvoiceNum >= LastNum) {

//            bootbox.dialog({
//                message: "<h5 class = 'MyFont-Roboto-header'> This is a Last Reception </h5>",
//                className: 'bootbox-class MyFont-Roboto-header',
//                size: 'small',
//            });

//            window.setTimeout(function () {
//                bootbox.hideAll();
//            }, 2000);

//            return;
//        }

//        wantedInvoiceNum = currentInvoiceNum + 1;

//        getReception(wantedInvoiceNum);
//    }
//    else if (number === "Pre") {

//        if (currentInvoiceNum === "1") {

//            bootbox.dialog({
//                message: "<h5 class = 'MyFont-Roboto-header'> This is a First Reception </h5>",
//                className: 'bootbox-class MyFont-Roboto-header',
//                size: 'small',

//            });

//            window.setTimeout(function () {
//                bootbox.hideAll();
//            }, 2000);


//            return;
//        }

//        wantedInvoiceNum = currentInvoiceNum - 1;
//        getReception(wantedInvoiceNum);
//    }
//    else if (number === "Last") {

//        if (currentInvoiceNum === LastNum) {

//            bootbox.dialog({
//                message: "<h5 class = 'MyFont-Roboto-header'> This is a Last Reception </h5>",
//                className: 'bootbox-class MyFont-Roboto-header',
//                size: 'small',

//            });

//            window.setTimeout(function () {
//                bootbox.hideAll();
//            }, 2000);


//            return;

//        }

//        wantedInvoiceNum = LastNum;
//        getReception(wantedInvoiceNum);
//    }
//    else if (number === "First") {
//        if (currentInvoiceNum === FirstNum) {

//            bootbox.dialog({
//                message: "<h5 class = 'MyFont-Roboto-header'> This is Today's First Reception </h5>",
//                className: 'bootbox-class MyFont-Roboto-header',
//                size: 'small',

//            });

//            window.setTimeout(function () {
//                bootbox.hideAll();
//            }, 2000);


//            return;

//        }

//        wantedInvoiceNum = FirstNum;
//        getReception(wantedInvoiceNum);
//    }



//    let num = $('#ReceptionNumber').val();
//    if (num === null || num === undefined || num === "") {
//        return;
//    }

//    wantedInvoiceNum = parseInt($('#ReceptionNumber').val());

//    if (wantedInvoiceNum >= LastNum) {
//        bootbox.alert({
//            message: "this reception number not exist",
//            className: 'bootbox-class MyFont-Roboto-header'

//        });
//        return;
//    }

//    if (isNaN(wantedInvoiceNum)) {
//        return;
//    }

//    getReception(wantedInvoiceNum);

//}

//function getReception(InvoiceNum) {

//    bootbox.dialog({
//        message: "<h5 class = 'MyFont-Roboto-header'> You Dont Save This Reception Do You Want Save It? </h5>",
//        className: 'bootbox-class MyFont-Roboto-header',
//        size: 'medium',
//        buttons: {
//            cancel: {
//                label: "No",
//                className: 'MyFont-Roboto-grid',
//                callback: function () {
//                    $(".loader").removeClass("hidden");
//                    $(".page-content").load("/PatientReceptionAnalysis/GetPatientReception?InvoiceNum=" + InvoiceNum, function (responce) {

//                        $(".loader").fadeIn("slow");
//                        $(".loader").addClass("hidden");
//                    });
//                }
//            },

//            ok: {
//                label: "Yes",
//                className: 'MyFont-Roboto-grid',
//                callback: function () {
//                    let confirm = AggregateData();

//                    if (confirm) {
//                        $(".loader").removeClass("hidden");

//                        var token = $(':input:hidden[name*="RequestVerificationToken"]');

//                        $.ajax({
//                            type: "Post",
//                            url: "/PatientReception/AddOrUpdate",
//                            data: {
//                                __RequestVerificationToken: token.attr('value'),
//                                patientReception: patientReception
//                            },
//                            success: function (response) {
//                                if (response !== 0) {

//                                    $(".page-content").load("/PatientReceptionAnalysis/GetPatientReception?InvoiceNum=" + InvoiceNum, function (responce) {


//                                        $(".loader").fadeIn("slow");
//                                        $(".loader").addClass("hidden");
//                                    });

//                                }
//                            }
//                        });
//                    }
//                    bodyPadding();

//                }
//            }
//        }
//    });


//}

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

    let date = $("#DateOfBirth").data("kendoDatePicker");
    date.element.focus();
}

function GetConvert(baseCurrencyId, destCurrencyId, totalWithDiscount) {



    $.ajax({
        type: "Post",
        url: "/MoneyConvert/GetConvertAmount",
        data: { BaseCurrencyId: baseCurrencyId, DestCurrencyId: destCurrencyId },
        success: function (response) {

            if (response !== -1 && response !== 0) {

                AllMoneyConverts.push({ BaseCurrencyId: baseCurrencyId, DestCurrencyId: destCurrencyId, Amount: response, Exist: false });
                calculateTotalReceived(totalWithDiscount);

            }
            else {

                var link = "/MoneyConvert/AddNewModal";
                $(".loader").removeClass("hidden");
                $('#AllMoneyConvertModal').modal('toggle');
                $('#AllMoneyConvertModal-body').load(link, function () {
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    //$("#BaseCurrencyId1").data("kendoDropDownList").value(baseCurrencyId);
                    //$("#DestCurrencyId1").data("kendoDropDownList").value(destCurrencyId);
                });
            }

        }
    })


}

function addAllNewMoneyConvert() {
    //let base = $("#BaseCurrencyId1").data("kendoDropDownList").value();

    //let dest = $("#DestCurrencyId1").data("kendoDropDownList").value();

    let amount = $("#Price").data("kendoNumericTextBox").value();



    if (amount === 0 || amount === "" || amount === null) {

        bootbox.alert({
            message: "please Insert Amount",
            className: 'bootbox-class'

        });
        return;

    }


    //if (base === dest) {
    //    bootbox.alert({
    //        message: "Both Currency Cannot be Same",
    //        className: 'bootbox-class'

    //    });
    //    return;
    //}


    var token = $(':input:hidden[name*="RequestVerificationToken"]');


    $.ajax({
        type: "Post",
        data: { __RequestVerificationToken: token.attr('value'), BaseCurrencyId: 12, DestCurrencyId: 11, Amount: amount },
        url: "/MoneyConvert/AddOrUpdate",
        success: function (response) {

            AllMoneyConverts.push({ BaseCurrencyId: 12, DestCurrencyId: 11, Amount: amount, Exist: false });
            calculateTotalReceived($("#totalAmountWithDiscount").val());

            $('#AllMoneyConvertModal').modal('toggle');
            $('#AllMoneyConvertModal-body').empty();
        }
    });
}


function bodyPadding() {

    $('body').addClass('body-padding');
}

var patientReception;
var Patient;

function Exit() {
    $(".page-content").load("/PatientReception/Form", function (responce) {


        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    })
}

function PatientReceptionAnalysisReport() {



    let confirm = AggregateData();

    if (confirm) {

        //$(".loader").removeClass("hidden");

        //var token = $(':input:hidden[name*="RequestVerificationToken"]');

        //$.ajax({
        //    type: "Post",
        //    url: "/PatientReception/AddOrUpdate",
        //    data: {
        //        __RequestVerificationToken: token.attr('value'),
        //        patientReception: patientReception
        //    },
        //    success: function (response) {

        //    }
        //});
        sendReception("report");
    }

}
function OkAndExit() {


    let confirm = AggregateData();

    if (confirm) {
        sendReception("exit");
    }

    bodyPadding();




}

function sendReception(status) {

    $(".loader").removeClass("hidden");

    var token = $(':input:hidden[name*="RequestVerificationToken"]');

    $.ajax({
        type: "Post",
        url: "/PatientReception/AddOrUpdate",
        data: {
            __RequestVerificationToken: token.attr('value'),
            patientReception: patientReception
        },
        success: function (response) {
            if (response === "NoInernetConnection" || response === "No such host is known.") {
                bootbox.dialog({
                    message: "<h5 class='MyFont-Roboto-header'>No Internet Connection </h5>",
                    className: 'bootbox-class MyFont-Roboto-header',
                    size: 'small',

                });
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response.message === "ServerError") {
                bootbox.dialog({
                    message: "<h5 class='MyFont-Roboto-header'>Server Error Please Try Again</h5>",
                    className: 'bootbox-class MyFont-Roboto-header',
                    size: 'small',

                });
                $("#Guid").val(response.receptionId);
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response.message === "InternalError") {
                bootbox.dialog({
                    message: "<h5 class='MyFont-Roboto-header'>Please Try Again</h5>",
                    className: 'bootbox-class MyFont-Roboto-header',
                    size: 'small',

                });
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response !== 0) {
                if (status === "new") {
                    $(".page-content").load("/PatientReceptionAnalysis/Form", function (responce) {

                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");
                    })
                }
                else if (status === "exit") {
                    $(".page-content").load("/PatientReception/Form", function (responce) {


                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");
                    })
                }
                else {
                    $("#Guid").val(response);
                    $.ajax({
                        url: "/PatientReceptionAnalysis/PrintPatientReceptionAnalysisReport",
                        type: "Post",
                        data: { PatientReceptionId: response },
                        success: function (response) {
                            Exit();
                            draw2(response);
                            $(".loader").fadeIn("slow");
                            $(".loader").addClass("hidden");

                        },
                        error: function (response) {

                        }
                    });
                }
            }
        }
    });

}

function OkAndNew() {

    let confirm = AggregateData();

    if (confirm) {
        sendReception("new");
    }

    bodyPadding();

}

function AggregateData() {

    let fillPatientName = $("#Translated").attr('data-PleaseFillPatientName');

    let name = $("#Name2").val().replace(/ /g, '');
    if (name === "") {

        bootbox.dialog({
            message: "<h5 class='MyFont-Roboto-header'>" + fillPatientName + " </h5>",
            className: 'bootbox-class MyFont-Roboto-header',
            size: 'small',

        });

        window.setTimeout(function () {
            bootbox.hideAll();
            $("#Name2").focus();
        }, 2000);

        bodyPadding();

        return false;
    }

    let date = $("#DateOfBirth").data("kendoDatePicker").value();

    let usedollar = $("#useDollar").text();

    let useOnlineResult = $("#onlineResult").is(":checked");

    if (useOnlineResult) {
        let number = $("#PhoneNumber").val();
        if (number === "" || number === null) {
            bootbox.dialog({
                message: "<h5 class='MyFont-Roboto-header'> For Online Result PhoneNumber Is Required </h5>",
                className: 'bootbox-class MyFont-Roboto-header',
                size: 'small',

            });

            return false;
        }
    }

    let receives = [];

    if (usedollar === "false") {

        let amount = $("#totalAmountWithDiscount").val();
        let amount2 = amount.replace(',', '');

        receives.push({ Amount: amount2, AmountCurrencyId: 11 });
    }
    else {
        for (let i = 0; i < ReceivedAmount.length; i++) {
            if (ReceivedAmount[i].New)
                receives.push({ Amount: ReceivedAmount[i].Amount, AmountCurrencyId: ReceivedAmount[i].AmountCurrencyId });
        }
    }

    Patient = {
        Guid: $("#PatientId").val(),
        Name: $("#Name2").val(),
        GenderId: $("#GenderId").val(),
        PhoneNumber: $("#PhoneNumber").val(),
        AddressId: $("#AddressId").val(),
        DateOfBirthDay: date.getDate(),
        DateOfBirthMonth: date.getMonth() + 1,
        DateOfBirthYear: date.getFullYear()
    };

    let Doctor = {
        Name: $("#Doctor").val(),
        SpecialityId: $("#Speciality").val(),
        Guid: $("#DoctorId").val()
    };

    let receptionClinicSectionId = $("#ReceptionClinicSection-Id").text();
    patientReception = {
        Guid: $("#Guid").val(),
        CreatedUserId: $("#CreatedUserId").val(),
        CreatedDate: $("#CreatedDate").val(),
        PatientId: $("#PatientId").val(),
        Patient: Patient,
        RadiologyDoctorName: $("#RadiologyDoctor").val(),
        RadiologyDoctorId: $("#RadiologyDoctorId").val(),
        DoctorId: $("#DoctorId").val(),
        Doctor: Doctor,
        ReceptionDate: $("#ReceptionDate").val(),
        Discount: $("#DiscountAmount").val(),
        DiscountCurrencyId: 11,
        Description: $("#Explanation").val(),
        ReceptionNum: $("#ReceptionNum").val(),
        BaseCurrencyId: 11,
        ClinicSectionId: $("#ClinicSectionId").val(),
        Barcode: $("#Barcode").val(),
        PatientReceptionReceiveds: receives,
        HospitalReception: hospitalReception,
        ReceptionClinicSectionId: receptionClinicSectionId
    };


    var patientReceptionAnalysis = [];

    for (let i = 0; i < selectedItems.length; i++) {

        if (selectedItems[i].Type === "Analysis") {
            patientReceptionAnalysis.push({

                AnalysisId: selectedItems[i].Guid,
                Amount: parseFloat(selectedItems[i].Price),
                AmountCurrencyId: 11,
                Discount: parseFloat(selectedItems[i].Discount),

            });
        }
        else if (selectedItems[i].Type === "Group Analysis") {
            patientReceptionAnalysis.push({

                GroupAnalysisId: selectedItems[i].Guid,
                Amount: parseFloat(selectedItems[i].Price),
                AmountCurrencyId: 11,
                Discount: parseFloat(selectedItems[i].Discount),

            });
        }
        else if (selectedItems[i].Type === "Analysis Item") {
            patientReceptionAnalysis.push({

                AnalysisItemId: selectedItems[i].Guid,
                Amount: parseFloat(selectedItems[i].Price),
                AmountCurrencyId: 11,
                Discount: parseFloat(selectedItems[i].Discount),

            });
        }

    }

    patientReception.PatientReceptionAnalyses = patientReceptionAnalysis;
    return true;
}

function onlineResultChange(element) {

    let res = $(element).is(":checked");

    $.ajax({
        type: "Post",
        url: "/ClinicSectionSetting/UpdateClinicSectionSetting",
        data: {
            SettingName: "UseOnlineResult",
            SrttingVal: res
        },
        success: function (response) {

        }
    });

}


function ConvertPrices() {

    let baseCurrency = 11;
    let currentCurrencyId = baseCurrency.value();

    $.ajax({
        type: "Post",
        url: "/MoneyConvert/GetConvertAmount",
        data: { BaseCurrencyId: previousCurrencyId, DestCurrencyId: currentCurrencyId },
        success: function (response) {

            if (response !== -1 && response !== 0) {
                previousCurrencyId = currentCurrencyId;
                for (let i = 0; i < AllItems.length; i++) {
                    if (baseCurrency.text() === "IQD") {
                        AllItems[i].Price = Math.round(AllItems[i].Price * response);
                        AllItems[i].Discount = Math.round(AllItems[i].Discount * response);
                    }
                    else {
                        AllItems[i].Price = AllItems[i].Price * response;
                        AllItems[i].Discount = AllItems[i].Discount * response;
                    }

                }
                for (let i = 0; i < selectedItems.length; i++) {

                    if (baseCurrency.text() === "IQD") {
                        selectedItems[i].Price = Math.round(selectedItems[i].Price * response);
                        selectedItems[i].Discount = Math.round(selectedItems[i].Discount * response);
                        selectedItems[i].Total = Math.round(selectedItems[i].Total * response);
                        let discountAmount = $("#DiscountAmount").data("kendoNumericTextBox");
                        discountAmount.value(Math.round(parseFloat($("#DiscountAmount").val()) * response));

                    }
                    else {
                        selectedItems[i].Price = selectedItems[i].Price * response;
                        selectedItems[i].Discount = selectedItems[i].Discount * response;
                        selectedItems[i].Total = selectedItems[i].Total * response;
                        let discountAmount = $("#DiscountAmount").data("kendoNumericTextBox");
                        discountAmount.value(parseFloat($("#DiscountAmount").val()) * response);

                    }

                }

                createSelectedGrid();
                createAnalysisGrid();
                createAnalysisListItemsGrid();
                createGroupAnalysisGrid();

            }
            else {
                var link = "/MoneyConvert/AddNewModal";
                $(".loader").removeClass("hidden");
                $('#MoneyConvertModal').modal('toggle');
                $('#MoneyConvertModal-body').load(link, function () {
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    $("#BaseCurrencyId1").data("kendoDropDownList").value(previousCurrencyId);
                    $("#DestCurrencyId1").data("kendoDropDownList").value(currentCurrencyId);
                });
            }

        }
    })

    if (baseCurrency.text() === "IQD")
        format = "n" + dinarDecimal;
    else if (baseCurrency.text() === "$")
        format = "n" + dollarDecimal;
    else if (baseCurrency.text() === "€")
        format = "n" + euroDecimal;
    else if (baseCurrency.text() === "£")
        format = "n" + pondDecimal;


    //let discount = $("#Discount").data("kendoNumericTextBox");
    //discount.setOptions({ format: format });
    let discountAmount = $("#DiscountAmount").data("kendoNumericTextBox");
    discountAmount.setOptions({ format: format });

}


function closeMoneyConvertModal() {

    let amount = $("#Amount1").data("kendoNumericTextBox").value();

    if (amount === 0 || amount === "" || amount === null) {

        let baseCurrency = 11;
        baseCurrency.value(previousCurrencyId);

    }

    $('#MoneyConvertModal').modal('toggle');
    $('#MoneyConvertModal-body').empty();
}


function closeAllMoneyConvertModal() {
    $('#AllMoneyConvertModal').modal('toggle');
    $('#AllMoneyConvertModal-body').empty();
}


function InsertTotalAmountWithDiscountToReceived() {


    let baseCurrency = 11;
    let today = new Date();

    let dd = today.getDate();
    let mm = today.getMonth() + 1;

    let yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    let todayDate = dd + '/' + mm + '/' + yyyy;

    let max;

    if (ReceivedAmount.length === 0) {
        max = 1;
    }
    else {
        max = ReceivedAmount.length + 1;
    }



    let ee = $("#RemainedAmount").val().replace(',', '');

    let remain = parseFloat(ee);

    if (remain === 0) {
        return;
    }

    ReceivedAmount.push({
        Amount: remain,
        AmountCurrencyId: 11,
        CurrencyType: baseCurrency.text(),
        New: true,
        Date: todayDate,
        Index: max,
        CanDelete: true
    });


    changeDiscountAmount();


}


function AddNewReceived() {

    var link = "/PatientReceptionReceived/AddNewModal";
    $(".loader").removeClass("hidden");
    $('#PatientReceptionReceivedModal').modal('toggle');
    $('#PatientReceptionReceivedModal-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });

}


function addNewPatientReceptionReceived() {

    //let baseCurrency = $("#PatientReceptionReceivedAmountCorrencyId").data("kendoDropDownList");
    let today = new Date();

    let dd = today.getDate();
    let mm = today.getMonth() + 1;

    let yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    let todayDate = dd + '/' + mm + '/' + yyyy;
    let max;

    if (ReceivedAmount.length === 0) {
        max = 1;
    }
    else {
        max = ReceivedAmount.length + 1;
    }

    let amount = parseFloat($("#PatientReceptionReceivedAmount").val());


    if (amount === 0 || amount === null || amount === undefined || Number.isNaN(amount)) {
        //$("#PatientReceptionReceivedAmount-box").removeClass('hidden');
        return;
    }


    ReceivedAmount.push({
        Amount: amount,
        AmountCurrencyId: 11,
        CurrencyType: 'IQD',
        New: true,
        Date: todayDate,
        Index: max,
        CanDelete: true
    });

    //$('#PatientReceptionReceivedModal').modal('toggle');
    //$('#PatientReceptionReceivedModal-body').empty();

    changeDiscountAmount();
    let am = $("#PatientReceptionReceivedAmount").data("kendoNumericTextBox");
    am.focus();
    am.value('');
    createPatientReceptionReceivedDetailGrid();
}



function addNewPatientReceptionReceivedDollar() {

    //let baseCurrency = $("#PatientReceptionReceivedAmountCorrencyId").data("kendoDropDownList");
    let today = new Date();

    let dd = today.getDate();
    let mm = today.getMonth() + 1;

    let yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    let todayDate = dd + '/' + mm + '/' + yyyy;
    let max;

    if (ReceivedAmount.length === 0) {
        max = 1;
    }
    else {
        max = ReceivedAmount.length + 1;
    }

    let amount = parseFloat($("#PatientReceptionReceivedAmountDollar").val());


    if (amount === 0 || amount === null || amount === undefined || Number.isNaN(amount)) {
        //$("#PatientReceptionReceivedAmount-box").removeClass('hidden');
        return;
    }


    ReceivedAmount.push({
        Amount: amount,
        AmountCurrencyId: 12,
        CurrencyType: '$',
        New: true,
        Date: todayDate,
        Index: max,
        CanDelete: true
    });

    //$('#PatientReceptionReceivedModal').modal('toggle');
    //$('#PatientReceptionReceivedModal-body').empty();

    changeDiscountAmount();
    let am = $("#PatientReceptionReceivedAmountDollar").data("kendoNumericTextBox");
    am.focus();
    am.value('');
    createPatientReceptionReceivedDetailGrid();
}



function ShowReceivedDetails() {


    $('#PatientReceptionReceivedDetailedModal').modal('toggle');
    //createPatientReceptionReceivedDetailGrid();

}


//function createPatientReceptionReceivedDetailGrid() {

//    if (RTL === "True") {
//        $("#PatientReceptionReceivedDetailGrid").kendoGrid({
//            dataSource: {
//                data: ReceivedAmount,
//                schema: {
//                    model: {
//                        fields: {
//                            receivedDate: { type: "string" },
//                            receivedAmount: { type: "numeric" },
//                            receivedCurrencyType: { type: "string" },

//                        }
//                    }
//                },
//                pageSize: 10
//            },

//            sortable: true,

//            pageable: {
//                input: true,
//                numeric: false
//            },
//            columns: [

//                {
//                    template: "<strong>  #: Index #  </strong>",
//                    width: "50px"
//                },
//                {
//                    field: "receivedDate",
//                    title: DateTranslated,

//                    template: "<strong   >  #: Date #  </strong>",
//                    headerAttributes: {
//                        style: "text-align: center"
//                    },


//                },
//                {
//                    field: "receivedAmount",
//                    title: AmountTranslated,

//                    template: "<strong   >  #: Amount #  </strong>",
//                    headerAttributes: {
//                        style: "text-align: center"
//                    },


//                },
//                {
//                    field: "receivedCurrencyType",
//                    title: TypeTranslated,

//                    template: "<strong >  #: CurrencyType #  </strong>",

//                    headerAttributes: {
//                        style: "text-align: center"
//                    },


//                },


//                {
//                    template: "# if (!CanDelete) { # <i> </i>#}else{#<i class='fa fa-trash bigger-120 red'   > </i># } #",
//                    width: "50px",
//                    attributes: {
//                        "onclick": "removeFromPatientReceptionReceivedDetailGrid(this)",

//                        "data-Index": "#: Index #",

//                        style: "cursor:pointer"
//                    }

//                }


//            ],
//            selectable: "row"
//        });
//    }
//    else {
//        $("#PatientReceptionReceivedDetailGrid").kendoGrid({
//            dataSource: {
//                data: ReceivedAmount,
//                schema: {
//                    model: {
//                        fields: {
//                            receivedDate: { type: "string" },
//                            receivedAmount: { type: "numeric" },
//                            receivedCurrencyType: { type: "string" },

//                        }
//                    }
//                },
//                pageSize: 10
//            },

//            sortable: true,

//            pageable: {
//                input: true,
//                numeric: false
//            },
//            columns: [

//                {
//                    template: "# if (!CanDelete) { # <i> </i>#}else{#<i class='fa fa-trash bigger-120 red'   > </i># } #",
//                    width: "50px",
//                    attributes: {
//                        "onclick": "removeFromPatientReceptionReceivedDetailGrid(this)",

//                        "data-Index": "#: Index #",

//                        style: "cursor:pointer"
//                    }

//                },
//                {
//                    field: "receivedCurrencyType",
//                    title: TypeTranslated,

//                    template: "<strong >  #: CurrencyType #  </strong>",

//                    headerAttributes: {
//                        style: "text-align: center"
//                    },


//                },
//                {
//                    field: "receivedAmount",
//                    title: AmountTranslated,

//                    template: "<strong   >  #: Amount #  </strong>",
//                    headerAttributes: {
//                        style: "text-align: center"
//                    },


//                },
//                {
//                    field: "receivedDate",
//                    title: DateTranslated,

//                    template: "<strong   >  #: Date #  </strong>",
//                    headerAttributes: {
//                        style: "text-align: center"
//                    },


//                },

//                {
//                    template: "<strong>  #: Index #  </strong>",
//                    width: "50px"
//                }

//            ],
//            selectable: "row"
//        });
//    }

//}

//function removeFromPatientReceptionReceivedDetailGrid(element) {

//    let id = $(element).attr('data-index');
//    let index = ReceivedAmount.findIndex(a => a.Index == id);
//    let Guid = ReceivedAmount[index].Guid;

//    if (!ReceivedAmount[index].CanDelete) {
//        return;
//    }

//    bootbox.confirm("Do you Want To Delete This Record?", function (result) {

//        if (!result) {

//            return;

//        }
//        else {

//            if (Guid !== null && Guid !== undefined) {
//                $.ajax({
//                    type: "Post",
//                    data: { Id: Guid },
//                    url: "/PatientReceptionReceived/Remove",
//                    success: function (response) {

//                        if (index > -1) {
//                            ReceivedAmount.splice(index, 1);
//                            createPatientReceptionReceivedDetailGrid();
//                        }
//                        changeDiscountAmount();
//                    }
//                });
//            }
//            else {

//                if (index > -1) {
//                    ReceivedAmount.splice(index, 1);
//                    createPatientReceptionReceivedDetailGrid();
//                }
//                changeDiscountAmount();
//            }


//        }
//    })


//}

function closePatientReceptionReceivedModal() {
    $('#PatientReceptionReceivedModal').modal('toggle');
    $('#PatientReceptionReceivedModal-body').empty();
}



function closePatientReceptionReceivedDetailedModal() {
    $('#PatientReceptionReceivedDetailedModal').modal('toggle');

}


function ShowDescription() {

    bootbox.confirm({
        title: "Description",
        message: " <textarea id='DescriptionTextBox' style='width:40vw' ></textarea>",
        className: 'bootbox-class MyFont-Roboto-header',
        size: 'lg',
        callback: function (result) {
            if (!result) {

                return;
            }
            else {

                $("#Explanation").val($("#DescriptionTextBox").val());

            }
        }

    });

    $("#DescriptionTextBox").kendoTextArea({
        rows: 2,
        maxLength: 2000,
        placeholder: "your Text..."
    });

    if ($("#Explanation").val() !== null || $("#Explanation").val() !== "" || $("#Explanation").val() !== undefined) {
        $("#DescriptionTextBox").val($("#Explanation").val());
    }
}

function searchPrescription() {

    $('#onlineTxtContainer .emptybox').addClass('hidden');
    var isEmmpty = true;
    $('#onlineTxtContainer .emptybox').each(function () {
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

    var service_container = $("#onlineGridContainer");
    if (service_container) {
        service_container.empty();
        var link = '/Visit/ShowAnalysisFromServerModal';

        service_container.load(link + '', function () {
        });
    }

}