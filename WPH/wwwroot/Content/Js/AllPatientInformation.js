
var doctor_list = [];


$(document).ready(function () {

    GetAllDoctorList().done(function (data) {
        doctor_list = data;
    });

    $("#ChildERROR_Date").addClass("hidden");
    $("#ChildERROR_Data").addClass("hidden");
    $("#ChildERROR_SomeThingWentWrong").addClass("hidden");


    CreateTelerikComponents();

    let receptionId = $("#Reception-Id").text();

    $(".loader").removeClass("hidden");

    $.ajax({
        type: "Post",
        data: { ReceptionId: receptionId },
        url: "/Reception/GetReception",
        success: function (response) {

            SetReceptionAmounts(response);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");

        }
    });

    $.ajax({
        type: "Post",
        data: { ReceptionId: receptionId },
        url: "/ReceptionRoomBed/GetReceptionRoomBedName",
        success: function (response) {

            $("#RoomBedName").val(response);

        }
    });

    $.ajax({
        type: "Post",
        data: { ReceptionId: receptionId },
        url: "/Surgery/GetReceptionSurgery",
        success: function (response) {

            if (response !== null && response !== '') {

                SetSurgeryAmounts(response);

            }

        }
    });

    $.ajax({
        type: "Post",
        data: { ReceptionId: receptionId },
        url: "/Surgery/GetReceptionOperation",
        success: function (response) {

            if (response !== null && response !== '') {
                try {
                    $("#OperationName").val(response.Name)
                }
                catch (e) { }

            }

        }
    });

    $.ajax({
        type: "Post",
        data: { ReceptionId: receptionId },
        url: "/Patient/GetAllReceptionVariable",
        success: function (response) {

            let DrugHistory = response.find(x => x.PatientVariableVariableName == "Drug History");

            let PastMedical = response.find(x => x.PatientVariableVariableName == "Past Medical And Surgical History");
            let drug = $("#DrugHistory").data("kendoTextArea");
            try {
                drug.value(DrugHistory.Value);
            } catch { }

            //$("#DrugHistory").val(DrugHistory.Value);
            let pas = $("#PastMedical").data("kendoTextArea");
            try {
                pas.value(PastMedical.Value);
            } catch { }

            //$("#PastMedical").val(PastMedical.Value);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");

        }
    });

});

function CreateTelerikComponents() {

    $("#Name2").kendoTextBox({

        enable: false
    });

    $("#PhoneNumber").kendoTextBox({

        enable: false
    });

    $("#GenderId").kendoTextBox({

        enable: false
    });

    $("#DateOfBirth").kendoDatePicker({
        format: "dd/MM/yyyy",
        max: new Date(),
        animation: false,
        change: changeBirthOfDatePatientDatetimePicker,
    });

    $("#TimePicker").kendoTimePicker({
        dateInput: true,
        format: "HH:mm"
    });

    $("#Year").kendoTextBox({

        enable: false
    });

    $("#BodyTemperature").kendoNumericTextBox({});

    $("#PulseRate").kendoNumericTextBox({});

    $("#RespirationRate").kendoNumericTextBox({});

    $("#DIABloodPressure").kendoNumericTextBox({});

    $("#SYSBloodPressure").kendoNumericTextBox({});

    $("#Month").kendoTextBox({

        enable: false
    });

    $("#ClinicSectionId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        enable: false,
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/ClinicSection/GetAllNormalClinicSectionsForUser",
                }
            }
        }
    });

    $("#EntranceTime").kendoDatePicker({
        format: "dd/MM/yyyy",
        max: new Date(),
        animation: false,
    });

    $("#IdentityNumber").kendoTextBox({

        enable: false
    });

    $("#Address").kendoTextBox({

        enable: false
    });

    $("#RoomBedName").kendoTextBox({

        enable: false
    });

    $("#MotherName").kendoTextBox({

        enable: false
    });

    $("#PatientAttendanceName").kendoTextBox({

        enable: false
    });


    $("#ExitTime").kendoDatePicker({
        format: "dd/MM/yyyy",
        max: new Date(),
        animation: false,
    });


    $("#ClearanceTypeId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        dataSource: {

            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetAllGenders",
                }
            }
        }
    });


    $("#SectionId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/ClinicSection/GetAllParentClinicSections",
                }
            }
        }
    });

    $("#ReceptionClinicSectionDescription").kendoTextArea({
        rows: 1,
        maxLength: 200,

    });

    $("#DispatcherDoctor").kendoTextBox({
        enable: false
    });

    $("#ServiceDate").kendoDateTimePicker({
        value: new Date(),
        max: new Date(),
        format: "dd/MM/yyyy HH:mm",
        dateInput: true
    });

    $("#ServiceName").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Service/GetAllSpeceficServices?ServiceName=Other",
                }
            }
        }
    });

    $("#ServiceValue").kendoNumericTextBox({

    });

    $("#StitchDate").kendoDateTimePicker({
        value: new Date(),
        max: new Date(),
        format: "dd/MM/yyyy HH:mm",
        dateInput: true
    });

    $("#StitchName").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Service/GetAllSpeceficServices?ServiceName=Stitch",
                }
            }
        }
    });

    $("#StitchValue").kendoNumericTextBox({

    });


    $("#ChiefComplaint").kendoTextArea({
        rows: 2,
        maxLength: 200,

    });

    $("#PastMedical").kendoTextArea({
        rows: 2,
        maxLength: 200,

    });

    $("#DrugHistory").kendoTextArea({
        rows: 2,
        maxLength: 200,

    });

    $("#Examination").kendoTextArea({
        rows: 2,
        maxLength: 200,

    });

    $("#AnesthesiaType").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        enable: false,
        dataSource: {

            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetBaseInfoGeneralsByName?BaseInfoGeneralName=" + "AnesthesiologistionType",
                }
            }
        }
    });

    $("#AnesthesiaMedication").kendoTextArea({
        enable: false,
        rows: 2,
        maxLength: 200,

    });

    $("#ClearanceTypeId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        dataSource: {

            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetBaseInfoGeneralsByName?BaseInfoGeneralName=" + "ClearanceType",
                }
            }
        }
    });

    $("#Anesthetist").kendoTextBox({
        enable: false
    });

    $("#Classification").kendoDropDownList({

        dataTextField: "Name",
        dataValueField: "Id",
        enable: false,
        dataSource: {

            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetBaseInfoGeneralsByName?BaseInfoGeneralName=" + "ClassificationType",
                }
            }
        }
    });

    $("#SurgeryDetails").kendoTextArea({
        enable: false,
        rows: 2,
        maxLength: 200,

    });

    $("#PostOperativeTreatment").kendoTextArea({
        enable: false,
        rows: 2,
        maxLength: 200,

    });

    $("#SurgererName").kendoTextBox({
        enable: false
    });

    $("#OperationName").kendoTextBox({

        enable: false
    });

    $("#TransfusionDate").kendoDateTimePicker({
        value: new Date(),
        max: new Date(),
        format: "dd/MM/yyyy HH:mm",
        dateInput: true
    });


    $("#TransfusionName").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Service/GetAllSpeceficServices?ServiceName=Transfusion",
                }
            }
        }
    });

    $("#TransfusionNum").kendoNumericTextBox({

    });

    $("#PatientProgressDate").kendoDateTimePicker({
        value: new Date(),
        max: new Date(),
        format: "dd/MM/yyyy HH:mm",
        dateInput: true
    });

    $("#PatientProgress").kendoTextBox({
    });

    var AnesthesiaMedication = $("#AnesthesiaMedication").data('kendoTextArea');
    AnesthesiaMedication.enable(false);

    var SurgeryDetails = $("#SurgeryDetails").data('kendoTextArea');
    SurgeryDetails.enable(false);

    var PostOperativeTreatment = $("#PostOperativeTreatment").data('kendoTextArea');
    PostOperativeTreatment.enable(false);


    $("#BabyName").kendoDropDownList({
        filter: "contains",
        dataTextField: "Name",
        dataValueField: "Guid",
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Child/GetAllUnknownChildren",
                }
            }
        }
    });

    $('#ReceptionDoctor').on('input', function () {
        var txt = $("#ReceptionDoctor").val();

        var filter_list = FilterDoctorLists(doctor_list, txt);

        $("#ReceptionDoctor").data("kendoAutoComplete").dataSource.data(filter_list);

    });

    $("#ReceptionDoctor").kendoDropDownList({
        filter: "contains",
        dataTextField: "UserName",
        dataValueField: "Guid",
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Doctor/GetAllDoctors",
                }
            }
        }
    });


    $("#RoomId").kendoDropDownList({
        filter: "contains",
        dataTextField: "SectionRoomName",
        dataValueField: "Guid",
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Room/GetAllRooms",
                }
            }
        }
    });


    $("#BabyReceivedDate").kendoDateTimePicker({
        value: new Date(),
        max: new Date(),
        format: "dd/MM/yyyy HH:mm",
        dateInput: true
    });

};

function SetReceptionAmounts(reception) {

    $("#ReceptionDate").text(reception.ReceptionDateString);
    $("#ReceptionNum").text(reception.ReceptionNum);
    $("#Name2").val(reception.Patient.User.Name);
    $("#PhoneNumber").val(reception.Patient.User.PhoneNumber);
    $("#GenderId").val(reception.Patient.UserGenderName);
    $("#DateOfBirth").val(reception.Patient.DateOfBirth);
    setTimeout(function () {
        let datepicker = $("#DateOfBirth").data("kendoDatePicker");
        let s = $("#DateOfBirth").attr('aria-owns');

        datepicker.open();

        datepicker.close();

        $("#" + s + " .k-state-focused a").click();
        datepicker.enable(false);

        let EntranceTime = $("#EntranceTime").data("kendoDatePicker");
        let s1 = $("#EntranceTime").attr('aria-owns');

        EntranceTime.open();

        EntranceTime.close();

        EntranceTime.enable(false);

        $("#" + s1 + " .k-state-focused a").click();


        let ExitTime = $("#ExitTime").data("kendoDatePicker");
        let s2 = $("#ExitTime").attr('aria-owns');

        ExitTime.open();

        ExitTime.close();

        $("#" + s2 + " .k-state-focused a").click();



    });
    //$("#ClinicSectionId").val(reception.ClinicSectionId);
    let section = $("#ClinicSectionId").data("kendoDropDownList");
    section.value(reception.ClinicSectionId);

    $("#IdentityNumber").val(reception.Patient.IdentityNumber);
    $("#Address").val(reception.Patient.Address.Name);
    $("#MotherName").val(reception.Patient.MotherName);
    $("#PatientAttendanceName").val(reception.PatientAttendanceName);

    $("#EntranceTime").val(reception.EntranceDate);
    $("#ExitTime").val(reception.ExitDate);
    $("#ChiefComplaint").val(reception.ChiefComplaint);
    $("#Examination").val(reception.Examination);
    //$("#ClearanceType").val(reception.ClearanceTypeId);

    let cle = $("#ClearanceTypeId").data("kendoDropDownList");
    cle.value(reception.ClearanceTypeId);
};

function SetSurgeryAmounts(reception) {

    let AnesthesiaType = $("#AnesthesiaType").data("kendoDropDownList");
    AnesthesiaType.value(reception.AnesthesiologistionTypeId);
    $("#AnesthesiaMedication").val(reception.AnesthesiologistionMedicine);

    if (reception.Anesthesiologist !== null && reception.Anesthesiologist !== undefined) {
        $("#Anesthetist").val(reception.Anesthesiologist.UserName);
    }

    if (reception.SurgeryOne !== null && reception.SurgeryOne !== undefined) {
        $("#SurgererName").val(reception.SurgeryOne.UserName);
    }
    if (reception.DispatcherDoctor !== null && reception.DispatcherDoctor !== undefined) {
        $("#DispatcherDoctor").val(reception.DispatcherDoctor.UserName);
    }
    //$("#Classification").val(reception.ClassificationId);

    let Classification = $("#Classification").data("kendoDropDownList");
    Classification.value(reception.ClassificationId);
    $("#SurgeryDetails").val(reception.SurgeryDetail);


    $("#PostOperativeTreatment").val(reception.PostOperativeTreatment);

}

function addTemperature() {

    var TempDate = $("#TimePicker").data("kendoTimePicker");
    var Date = TempDate.value();

    let time = Date.getHours() + ":" + Date.getMinutes();

    let TempNum = $("#BodyTemperature").data("kendoNumericTextBox").value();
    let pulseRate = $("#PulseRate").data("kendoNumericTextBox").value();
    let respirationRate = $("#RespirationRate").data("kendoNumericTextBox").value();
    let dIABloodPressure = $("#DIABloodPressure").data("kendoNumericTextBox").value();
    let sYSBloodPressure = $("#SYSBloodPressure").data("kendoNumericTextBox").value();
    let receptionId = $("#Reception-Id").text();

    $.ajax({
        type: "Post",
        data:
        {
            ReceptionId: receptionId,
            Temperature: TempNum,
            PulseRate: pulseRate,
            RespirationRate: respirationRate,
            DIABloodPressure: dIABloodPressure,
            SYSBloodPressure: sYSBloodPressure,
            InsertedDate: Date,
            InsertedTime: time
            //TemperatureDateDay: CostDateDay,
            //TemperatureDateMonth: CostDateMonth,
            //TemperatureDateYear: CostDateYear
        },
        url: "/Reception/AddReceptionTemperature",
        success: function (response) {

            $("#kendoReceptionTemperatureGrid .k-i-reload").click();

        }
    });

}

function addStitch() {

    var TempDate = $("#StitchDate").data("kendoDateTimePicker");
    var Date = TempDate.value();
    let cMonth = Date.getMonth() + 1;
    let cDay = Date.getDate();

    if (cMonth < 10)
        cMonth = "0" + cMonth;

    if (cDay < 10)
        cDay = "0" + cDay;

    let CostDateDay = cDay;
    let CostDateMonth = cMonth;
    let CostDateYear = Date.getFullYear();
    let CostDateHour = Date.getHours();
    let CostDateMin = Date.getMinutes();

    let SerNum = $("#StitchValue").data("kendoNumericTextBox").value();
    if (SerNum === null || SerNum < 1) {
        SerNum = 1;
    }
    let receptionId = $("#Reception-Id").text();
    let ServiceId = $("#StitchName").val();
    let Service = $("#StitchName").data("kendoDropDownList");
    var dataItem = Service.dataItem();

    $.ajax({
        type: "Post",
        data:
        {
            ReceptionId: receptionId,
            ServiceId: ServiceId,
            Number: SerNum,
            Price: dataItem.Price,
            ServiceDateDay: CostDateDay,
            ServiceDateMonth: CostDateMonth,
            ServiceDateYear: CostDateYear,
            ServiceDateHour: CostDateHour,
            ServiceDateMin: CostDateMin
        },
        url: "/ReceptionService/AddReceptionService",
        success: function (response) {
            if (response === 0) {
                bootbox.alert({
                    message: `${$("#InsertWrongText").text()}`,
                    className: 'bootbox-class'
                });

            } else if (response === "NotEnoughProductCount") {
                bootbox.alert({
                    message: `${$("#NotEnoughProductCountText").text()}`,
                    className: 'bootbox-class'
                });

            } else {
                $("#Stitch .k-i-reload").click();

            }


        }
    });

}

function addService() {

    var TempDate = $("#ServiceDate").data("kendoDateTimePicker");
    var Date = TempDate.value();
    let cMonth = Date.getMonth() + 1;
    let cDay = Date.getDate();

    if (cMonth < 10)
        cMonth = "0" + cMonth;

    if (cDay < 10)
        cDay = "0" + cDay;

    let CostDateDay = cDay;
    let CostDateMonth = cMonth;
    let CostDateYear = Date.getFullYear();
    let CostDateHour = Date.getHours();
    let CostDateMin = Date.getMinutes();

    let SerNum = $("#ServiceValue").data("kendoNumericTextBox").value();
    if (SerNum === null || SerNum < 1) {
        SerNum = 1;
    }
    let receptionId = $("#Reception-Id").text();
    let ServiceId = $("#ServiceName").val();
    let Service = $("#ServiceName").data("kendoDropDownList");
    var dataItem = Service.dataItem();

    $.ajax({
        type: "Post",
        data:
        {
            ReceptionId: receptionId,
            ServiceId: ServiceId,
            Number: SerNum,
            Price: dataItem.Price,
            ServiceDateDay: CostDateDay,
            ServiceDateMonth: CostDateMonth,
            ServiceDateYear: CostDateYear,
            ServiceDateHour: CostDateHour,
            ServiceDateMin: CostDateMin
        },
        url: "/ReceptionService/AddReceptionService",
        success: function (response) {
            if (response === 0) {
                bootbox.alert({
                    message: `${$("#InsertWrongText").text()}`,
                    className: 'bootbox-class'
                });

            } else if (response === "NotEnoughProductCount") {
                bootbox.alert({
                    message: `${$("#NotEnoughProductCountText").text()}`,
                    className: 'bootbox-class'
                });

            } else {
                $("#Other .k-i-reload").click();

            }

        }
    });

}

function addPatientProgress() {

    var TempDate = $("#PatientProgressDate").data("kendoDateTimePicker");
    var Date = TempDate.value();
    let cMonth = Date.getMonth() + 1;
    let cDay = Date.getDate();

    if (cMonth < 10)
        cMonth = "0" + cMonth;

    if (cDay < 10)
        cDay = "0" + cDay;

    let CostDateDay = cDay;
    let CostDateMonth = cMonth;
    let CostDateYear = Date.getFullYear();
    let CostDateHour = Date.getHours();
    let CostDateMin = Date.getMinutes();

    let ppVal = $("#PatientProgress").val();
    let receptionId = $("#Reception-Id").text();

    let VariablesValue = [];

    VariablesValue.push({
        Value: ppVal,
        VariableInsertedDateDay: CostDateDay,
        VariableInsertedDateMonth: CostDateMonth,
        VariableInsertedDateYear: CostDateYear,
        VariableInsertedDateHour: CostDateHour,
        VariableInsertedDateMin: CostDateMin,
        PatientVariableVariableName: 'Patient Progress'
    });

    $.ajax({
        type: "Post",
        data:
        {
            ReceptionId: receptionId,
            VariablesValue: VariablesValue
        },
        url: "/Patient/AddorUpdatePatentVariablesForReception",
        success: function (response) {

            $("#PatientVariablesValueKendoGrid .k-i-reload").click();

        }
    });

}

function addPatientVariables() {

    let ChiefComplaint = $("#ChiefComplaint").val();
    let Examination = $("#Examination").val();
    let DrugHistory = $("#DrugHistory").val();
    let PastMedical = $("#PastMedical").val();
    let receptionId = $("#Reception-Id").text();

    let VariablesValue = [];

    VariablesValue.push({
        Value: DrugHistory,
        PatientVariableVariableName: 'Drug History'
    });
    VariablesValue.push({
        Value: PastMedical,
        PatientVariableVariableName: 'Past Medical And Surgical History'
    });

    $(".loader").removeClass("hidden");

    $.ajax({
        type: "Post",
        data:
        {
            ReceptionId: receptionId,
            VariablesValue: VariablesValue
        },
        url: "/Patient/AddorUpdatePatentVariablesForReception",

    });

    $.ajax({
        type: "Post",
        data:
        {
            Guid: receptionId,

            ChiefComplaint: ChiefComplaint,
            Examination: Examination,
        },
        url: "/Reception/UpdateReceptionChiefComplaint",
        success: function (response) {

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });

}

function addTransfusion() {

    var TempDate = $("#TransfusionDate").data("kendoDateTimePicker");
    var Date = TempDate.value();
    let cMonth = Date.getMonth() + 1;
    let cDay = Date.getDate();

    if (cMonth < 10)
        cMonth = "0" + cMonth;

    if (cDay < 10)
        cDay = "0" + cDay;

    let CostDateDay = cDay;
    let CostDateMonth = cMonth;
    let CostDateYear = Date.getFullYear();
    let CostDateHour = Date.getHours();
    let CostDateMin = Date.getMinutes();

    let SerNum = $("#TransfusionNum").data("kendoNumericTextBox").value();
    let receptionId = $("#Reception-Id").text();
    let ServiceId = $("#TransfusionName").val();
    let Service = $("#TransfusionName").data("kendoDropDownList");
    var dataItem = Service.dataItem();

    $.ajax({
        type: "Post",
        data:
        {
            ReceptionId: receptionId,
            ServiceId: ServiceId,
            Number: SerNum,
            Price: dataItem.Price,
            ServiceDateDay: CostDateDay,
            ServiceDateMonth: CostDateMonth,
            ServiceDateYear: CostDateYear,
            ServiceDateHour: CostDateHour,
            ServiceDateMin: CostDateMin
        },
        url: "/ReceptionService/AddReceptionService",
        success: function (response) {
            if (response === 0) {
                bootbox.alert({
                    message: `${$("#InsertWrongText").text()}`,
                    className: 'bootbox-class'
                });

            } else if (response === "NotEnoughProductCount") {
                bootbox.alert({
                    message: `${$("#NotEnoughProductCountText").text()}`,
                    className: 'bootbox-class'
                });

            } else {
                $("#Transfusion .k-i-reload").click();

            }
        }
    });

}

function ClearancePatient() {

    $("#ClearancePatientModal #ERROR_SomeThingWentWrong").addClass("hidden");

    $(".loader").removeClass("hidden");
    $('#ClearancePatientModal').modal('toggle');
    $('#ConfirmClearancePatientModal').modal('hide');

    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}

$("#btn-clearancePatientAccept").on("click", function () {

    Save_Clearance_Patient('false');
});

$("#btn-confirmClearancePatientAccept").on("click", function () {

    Save_Clearance_Patient('true');
});


function addChildInPatientHospital() {

    var link = `/Child/AddAndNewModal`;
    $(".loader").removeClass("hidden");


    $('#NewChildModal').modal('toggle');
    $('#NewChildModal-body').load(link + '', function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });

}

$("#btn-NewChildModal").click(function () {


    $(".loader").removeClass("hidden");
    var Child = $("#addNewChildForm").serialize();

    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    $.ajax({
        type: "Post",
        url: "/Child/AddOrUpdate",
        data: Child,

        success: function (response) {



            if (response === "SomeThingWentWrong") {

                $('#NewChildModal #ERROR_SomeThingWentWrong').removeClass('hidden');

                $('#NewChildModal').modal('hide');

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

            } else {

                $('#NewChildModal').modal('hide');
                $('#NewChildModal-body').empty();
                $("#BabyName").data("kendoDropDownList").dataSource.read();
                let pr = $("#BabyName").data("kendoDropDownList");
                pr.bind("dataBound", function () {
                    pr.value(response);
                });
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });


})


function Save_Clearance_Patient(confirm) {
    $("#ClearancePatientModal #ERROR_SomeThingWentWrong").addClass("hidden");
    let receptionId = $("#Reception-Id").text();
    $(".loader").removeClass("hidden");

    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    $.ajax({
        type: "Post",
        url: "/Reception/ClearancePatient",
        data:
        {
            __RequestVerificationToken: token.attr('value'),
            receptionId: receptionId,
            confirm: confirm,
        },
        success: function (response) {


            if (response === "SomeThingWentWrong") {

                $('#ClearancePatientModal #ERROR_SomeThingWentWrong').removeClass('hidden');

                $('#ConfirmClearancePatientModal').modal('hide');
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

            } else if (response === "Unpaid") {

                $('#ConfirmClearancePatientModal').modal('toggle');

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

            } else {

                $('#ClearancePatientModal').modal('hide');
                $('#ConfirmClearancePatientModal').modal('hide');

                $(".page-content").load("/HospitalPatient/Form", function (responce) {


                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                })
            }
        }
    });
}

function OkAndExit() {


    $(".loader").removeClass("hidden");
    var TempDate = $("#ExitTime").data("kendoDatePicker");
    var Date = TempDate.value();
    let CostDateDay;
    let CostDateMonth;
    let CostDateYear;
    if (Date !== null && Date !== undefined) {
        let cMonth = Date.getMonth() + 1;
        let cDay = Date.getDate();

        if (cMonth < 10)
            cMonth = "0" + cMonth;

        if (cDay < 10)
            cDay = "0" + cDay;

        CostDateDay = cDay;
        CostDateMonth = cMonth;
        CostDateYear = Date.getFullYear();
    }


    let receptionId = $("#Reception-Id").text();
    let ClearanceTypeId = $("#ClearanceTypeId").val();
    //let ChiefComplaint = $("#ChiefComplaint").val();
    //let Examination = $("#Examination").val();

    $.ajax({
        type: "Post",
        data:
        {
            Guid: receptionId,
            ClearanceTypeId: ClearanceTypeId,
            ExitDateDay: CostDateDay,
            ExitDateMonth: CostDateMonth,
            ExitDateYear: CostDateYear,
            //ChiefComplaint: ChiefComplaint,
            //Examination: Examination,
        },
        url: "/Reception/UpdateReceptionCleareance",
        success: function (response) {

            $(".page-content").load("/HospitalPatient/Form", function (responce) {


                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            })
        }
    });

}


function PrintPatientInformation() {

    $(".loader").removeClass("hidden");
    var TempDate = $("#ExitTime").data("kendoDatePicker");
    var Date = TempDate.value();
    let cMonth = Date.getMonth() + 1;
    let cDay = Date.getDate();

    if (cMonth < 10)
        cMonth = "0" + cMonth;

    if (cDay < 10)
        cDay = "0" + cDay;

    let CostDateDay = cDay;
    let CostDateMonth = cMonth;
    let CostDateYear = Date.getFullYear();

    let receptionId = $("#Reception-Id").text();
    let ClearanceTypeId = $("#ClearanceTypeId").val();
    let ChiefComplaint = $("#ChiefComplaint").val();
    let Examination = $("#Examination").val();

    $.ajax({
        type: "Post",
        data:
        {
            Guid: receptionId,
            ClearanceTypeId: ClearanceTypeId,
            ExitDateDay: CostDateDay,
            ExitDateMonth: CostDateMonth,
            ExitDateYear: CostDateYear,
            ChiefComplaint: ChiefComplaint,
            Examination: Examination,
        },
        url: "/Reception/UpdateReceptionCleareance",
        success: function (response) {

            let id = receptionId;
            $.ajax({
                url: "/HospitalPatient/PrintHospitalPatientReport",
                type: "Post",
                data: { ReceptionId: id },
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
    });

}


function Exit() {


    $(".loader").removeClass("hidden");
    $(".page-content").load("/HospitalPatient/Form", function (responce) {

        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    })

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

function DeleteReceptionTemperature(element) {

    $("#DeleteModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#DeleteModal #ERROR_SomeThingWentWrong").addClass("hidden");

    $(".loader").removeClass("hidden");
    $('#DeleteModal').modal('toggle');
    var Id = $(element).attr('data-id');
    var type = $(element).attr('data-type');

    $('#btn-deleteaccept').attr('data-id', Id);
    $('#btn-deleteaccept').attr('data-type', type);
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}

$('#btn-deleteaccept').on("click", function () {
    $(this).attr("disabled", true);

    $("#DeleteModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#DeleteModal #ERROR_SomeThingWentWrong").addClass("hidden");

    var Id = $(this).attr('data-id');
    var type = $(this).attr('data-type');

    var link = "";

    if (type === "Temp") {
        link = "/Reception/RemoveReceptionTemperature";
    }
    else if (type === "Product" || type === "Stitch" || type === "Other" || type === "Transfusion") {
        link = "/ReceptionService/RemoveReceptionService";
    }
    else if (type === "PatientVariable") {
        link = "/PatientVariable/RemoveVariableValue";
    }

    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: {
            __RequestVerificationToken: token.attr('value'),
            Id: Id
        },
        success: function (response) {
            $('#btn-deleteaccept').removeAttr("disabled");

            if (response === "SUCCESSFUL") {
                $('#DeleteModal').modal('hide');
                $(".modal-backdrop:last").remove();

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

                if (type === "Temp") {
                    $("#kendoReceptionTemperatureGrid .k-pager-refresh")[0].click();
                }
                else if (type === "Product") {
                    $("#Product .k-pager-refresh")[0].click();
                }
                else if (type === "Stitch") {
                    $("#Stitch .k-pager-refresh")[0].click();
                }
                else if (type === "Other") {
                    $("#Other .k-pager-refresh")[0].click();
                }
                else if (type === "Transfusion") {
                    $("#Transfusion .k-pager-refresh")[0].click();
                }
                else if (type === "PatientVariable") {
                    $("#PatientVariablesValueKendoGrid .k-i-reload").click();
                }

            }
            else if (response === "ERROR_ThisRecordHasDependencyOnItInAnotherEntity") {
                $("#DeleteModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "ERROR_SomeThingWentWrong") {
                $("#DeleteModal #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });
});


function addReceptionClinicSection() {


    let receptionId = $("#Reception-Id").text();
    let clinicSectionId = $("#SectionId").val();
    let Desc = $("#ReceptionClinicSectionDescription").val();

    $.ajax({
        type: "Post",
        data:
        {
            ReceptionId: receptionId,
            ClinicSectionId: clinicSectionId,
            Description: Desc,
        },
        url: "/Reception/AddReceptionClinicSection",
        success: function (response) {

            $("#ReceptionClinicSectionGrid .k-i-reload").click();

        }
    });

}



function addBabyReception() {
    $("#ChildERROR_Date").addClass("hidden");
    $("#ChildERROR_Data").addClass("hidden");
    $("#ChildERROR_SomeThingWentWrong").addClass("hidden");

    var link = "/Child/AddToHospitalPatient";

    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    let receptionId = $("#Reception-Id").text();

    var Child = {
        __RequestVerificationToken: token.attr('value'),
        Guid: $("#BabyName").val(),
        ReceptionId: receptionId,
        TxtReceptionDate: $("#BabyReceivedDate").val(),
        DoctorId: $("#ReceptionDoctor").val(),
        RoomId: $("#RoomId").val(),
    };


    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: Child,
        success: function (response) {
            if (response !== 0) {
                if (response === "DataNotValid") {

                    $("#ChildERROR_Data").removeClass("hidden");
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                } else if (response === "DateNotValid") {

                    $("#ChildERROR_Date").removeClass("hidden");
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                } else {

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    $("#childrenKendoGrid .k-pager-refresh")[0].click();
                    $("#BabyName").data("kendoDropDownList").dataSource.read();
                }
            } else {

                $("#ChildERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });
}


function GridDeleteBabyReception(element) {
    $("#ChildERROR_Date").addClass("hidden");
    $("#ChildERROR_Data").addClass("hidden");
    $("#ChildERROR_SomeThingWentWrong").addClass("hidden");

    $("#DeleteChildModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#DeleteChildModal #ERROR_SomeThingWentWrong").addClass("hidden");

    $(".loader").removeClass("hidden");
    $('#DeleteChildModal').modal('toggle');
    var Id = $(element).attr('data-id');

    $('#btn-deleteChildAccept').attr('data-id', Id);
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}


$('#btn-deleteChildAccept').on("click", function () {
    $(this).attr("disabled", true);

    $("#DeleteChildModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#DeleteChildModal #ERROR_SomeThingWentWrong").addClass("hidden");

    var Id = $(this).attr('data-id');
    var link = "/Child/RemoveFromHospitalPatient";

    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: {
            __RequestVerificationToken: token.attr('value'),
            Id: Id
        },
        success: function (response) {
            $('#btn-deleteChildAccept').removeAttr("disabled");

            if (response === 0) {

                $("#DeleteChildModal #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            } else {
                $('#DeleteChildModal').modal('hide');
                $(".modal-backdrop:last").remove();

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

                $("#childrenKendoGrid .k-pager-refresh")[0].click();
                $("#BabyName").data("kendoDropDownList").dataSource.read();
            }
        }
    });
});