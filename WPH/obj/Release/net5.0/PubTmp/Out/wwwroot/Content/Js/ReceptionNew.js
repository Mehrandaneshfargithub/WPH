


var Address;
let addressAuto;
let SpeciallityDropDown;
var nameAuto = $("#Name2").data("kendoAutoComplete");;
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
var reception_formData;
var doctor_list = [];
var patient_list = [];

//////////////////////////////////////////////////////////////////////////KeyPress And Focuses

$("#Name2").on('focus', function (e) {
    $("#Name2-box").addClass('hidden');
    let value = nameAuto.value();
    nameAuto.search(value);
});

$("#PhoneNumber").on('focus', function (e) {

    //let doctorAuto = $("#PhoneNumber").data("kendoAutoComplete");
    //let value = doctorAuto.value();
    //doctorAuto.search(value);
});

$("#SurgeryOne").on('focus', function (e) {

    let doctorAuto = $("#SurgeryOne").data("kendoAutoComplete");
    let value = doctorAuto.value();
    doctorAuto.search(value);
});

$("#AddressName").on('focus', function (e) {
    let doctorAuto = $("#AddressName").data("kendoAutoComplete");
    let value = doctorAuto.value();
    doctorAuto.search(value);
});

$("#DispatcherDoctor").on('focus', function (e) {
    let doctorAuto = $("#DispatcherDoctor").data("kendoAutoComplete");
    let value = doctorAuto.value();
    doctorAuto.search(value);
});

$('#Name2').on('keypress', function (e) {
    if (e.which === 13) {
        $("#PhoneNumber").focus();
    };
});

$("#FromHospitalName").on('focus', function (e) {

    let inputAuto = $("#FromHospitalName").data("kendoAutoComplete");
    let value = inputAuto.value();
    inputAuto.search(value);
});

$("#ToHospitalName").on('focus', function (e) {

    let inputAuto = $("#ToHospitalName").data("kendoAutoComplete");
    let value = inputAuto.value();
    inputAuto.search(value);
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

    let char = Numbers.indexOf(e.key);

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
        };
    }
    else {
        e.preventDefault();
    };


});

$('#GenderId').parent().on('keypress', function (e) {

    if (e.which === 13) {
        $("#IdentityNumber").focus();
    };
});

$('#DateOfBirth').on('focus', function (e) {
    let date = $("#DateOfBirth").data("kendoDatePicker");
    date.element.select();
});

$('#DateOfBirth').on('keypress', function (e) {

    if (e.which === 13) {
        $("#Year").focus();
    };
});

$('#Year').on('keypress', function (e) {

    if (e.which === 13) {
        $("#Month").focus();
    };
});

$('#Year').on('focus', function (e) {

    $("#Year").select();
});

$('#Month').on('keypress', function (e) {

    if (e.which === 13) {
        $("#AddressName").focus();
    };
});

$('#IdentityNumber').on('keypress', function (e) {

    if (e.which === 13) {
        let date = $("#DateOfBirth").data("kendoDatePicker");
        date.element.focus();
    };
});

$('#Month').on('focus', function (e) {

    $("#Month").select();
});


$('#AddressName').on('keypress', function (e) {

    if (e.which === 13) {
        $("#MotherName").focus();
    };
});

$('#SurgeryOne').on('keypress', function (e) {

    if (e.which === 13) {
        $("#DispatcherDoctor").focus();
    };
});

$('#DispatcherDoctor').on('keypress', function (e) {

    if (e.which === 13) {
        let operation = $("#Operation").data("kendoDropDownList");
        operation.focus();
        operation.open();
    };
});

$('#Operation').parent().on('keypress', function (e) {

    if (e.which === 13) {
        let date = $("#datetimepicker").data("kendoDateTimePicker");
        date.element.focus();
    };
});

$('#datetimepicker').on('keypress', function (e) {

    if (e.which === 13) {
        $("#Description").focus();
    };
});

$('#MotherName').on('keypress', function (e) {

    if (e.which === 13) {
        $("#PatientAttendanceName").focus();
    };
});

$('#MotherName').on('focus', function (e) {
    $("#MotherName").select();
});

$('#PatientAttendanceName').on('keypress', function (e) {

    if (e.which === 13) {
        //let emergency = $("#Emergency").text();

        //if (emergency.toLowerCase() === "false") {
        let clinic = $("#ClinicSectionId").data("kendoDropDownList");
        clinic.focus();
        clinic.open();
        //}
        //else {
        //    let Purpose = $("#PurposeId").data("kendoDropDownList");
        //    Purpose.focus();
        //    Purpose.open();
        //};

    };
});

$('#PatientAttendanceName').on('focus', function (e) {
    $("#PatientAttendanceName").select();
});

$('#ClinicSectionId').parent().on('keypress', function (e) {

    if (e.which === 13) {
        let purpose = $("#RoomBedId").data("kendoDropDownList");
        purpose.focus();
        purpose.open();
    };
});


$('#RoomBedId').parent().on('keypress', function (e) {

    if (e.which === 13) {
        let purpose = $("#PurposeId").data("kendoDropDownList");
        purpose.focus();
        purpose.open();
    };
});

$('#PurposeId').parent().on('keypress', function (e) {

    if (e.which === 13) {

        let emergency = $("#PurposeId").data("kendoDropDownList").text();

        if (emergency.toLowerCase() === "surgery") {
            $('#SurgeryOne').focus();
        }
        else {
            let Service = $("#Service").data("kendoDropDownList");
            Service.focus();
            Service.open();
        };


    };
});

$('#Service').parent().on('keypress', function (e) {

    if (e.which === 13) {

        $("#ServiceNumber").data("kendoTextBox").focus();

    };
});

$('#ServiceNumber').on('keypress', function (e) {

    if (e.which === 13) {

        $("#Description").focus();

    };
});

$('#Explanation').on('keydown', function (e) {

    if (e.which === 9) {
        e.preventDefault();
        $("html, body").animate({ scrollTop: 450 });
        setTimeout(function () {
            $("#AllNameAutoComplete").focus();
        }, 500);

    };

});


$('#btnOkNew').on('keydown', function (e) {

    if (e.which === 37) {
        $("#btnOkExit").focus();
    };

});

$('#btnOkExit').on('keydown', function (e) {

    if (e.which === 39) {

        $("#btnOkNew").focus();
    }
    else if (e.which === 37) {
        $("#btnExit").focus();
    };

});

$('#btnExit').on('keydown', function (e) {

    if (e.which === 39) {

        $("#btnOkExit").focus();
    }
    else if (e.which === 37) {
        $("#btnPrint").focus();
    };

});

/////////////////////////////////////////////////////////////////////KeyPress And Focuses END


//////////////Other Functions

let enable = true;

$(document).ready(function () {

    GetAllDoctorList().done(function (data) {
        doctor_list = data;
    });

    GetAllPatientList().done(function (data) {
        patient_list = data;
    });

    let receptionId = $("#Reception-Id").text();


    if (receptionId !== "" && receptionId !== null) {

        enable = false;

        readImagesFromServer("/PatientImage/GetMainPatientImageByRecptionId?receptionId=" + receptionId, "main_attachments_img", "lbl_main_attachments_count");

        readImagesFromServer("/PatientImage/GetOtherPatientImageByRecptionId?receptionId=" + receptionId, "other_attachments_img", "lbl_other_attachments_count");

        readImagesFromServer("/PatientImage/GetPolicReportPatientImageByReceptionId?receptionId=" + receptionId, "polic_report_attachments_img", "lbl_polic_report_attachments_count");
    }
    CreateTelerikComponents();



    if (receptionId !== "" && receptionId !== null && receptionId !== undefined) {

        setTimeout(() => {
            $(".loader").removeClass("hidden");
        });


        $.ajax({
            type: "Post",
            data: { ReceptionId: receptionId },
            url: "/Reception/GetReception",
            success: function (response) {

                if (response.Purpose.Name === "Surgery") {
                    $.ajax({
                        type: "Post",
                        data: { ReceptionId: receptionId },
                        url: "/Surgery/GetReceptionSurgery",
                        success: function (response) {

                            $("#SurgeryOne").val(response.SurgeryOne.UserName);
                            $("#SurgeryOneVal").text(response.SurgeryOne.Guid);
                            $("#datetimepicker").val(response.SurgeryDate);
                            let date = $("#datetimepicker").data("kendoDateTimePicker");
                            try {
                                $("#DispatcherDoctor").val(response.DispatcherDoctor.UserName);
                            }
                            catch (s) { }
                            try {
                                $("#DispatcherDoctorVal").text(response.DispatcherDoctor.Guid);
                            }
                            catch (s) { }

                            setTimeout(() => {
                                date.element.focus();
                                setTimeout(() => {
                                    $("#btnExit").focus();
                                }, 20);
                            }, 20);
                        }
                    });
                    $.ajax({
                        type: "Post",
                        data: { ReceptionId: receptionId },
                        url: "/ReceptionService/GetReceptionOperationService",
                        success: function (response) {
                            let ope = $("#Operation").data("kendoDropDownList");

                            ope.value(response);


                        }
                    });
                }
                else if (response.Purpose.Name === "Service") {
                    $.ajax({
                        type: "Post",
                        data: { ReceptionId: receptionId },
                        url: "/ReceptionService/GetReceptionExceptOperationService",
                        success: function (response) {

                            $("#Service").val(response.ServiceId)
                            $("#ServiceNumber").val(response.Number)

                        }
                    });
                };

                SetReceptionAmounts(response);
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

            }
        });

        //$.ajax({
        //    type: "Post",
        //    data: { ReceptionId: receptionId },
        //    url: "/ReceptionRoomBed/GetReceptionRoomBedId",
        //    success: function (response) {

        //        if (response != "" && response.Name != "") {
        //            var room_bed = $("#RoomBedId").data("kendoDropDownList").dataSource;

        //            room_bed.add({
        //                value: response.Guid,
        //                text: response.RoomBedName
        //            });

        //            $("#RoomBedId").val(response.Guid);
        //        }
        //        //$("#RoomBedNameParent").removeClass('hidden');
        //        //$("#RoomBedParent").addClass('hidden');

        //        $(".loader").fadeIn("slow");
        //        $(".loader").addClass("hidden");

        //    }
        //});

        let eme = $("#Emergency").text();

        if (eme.toLowerCase() === "true") {

            $.ajax({
                type: "Post",
                data: { EmergencyId: receptionId },
                url: "/EmergencyReception/GetEmergency",
                success: function (response) {

                    $("#ArrivalId").data("kendoDropDownList").value(response.ArrivalId);
                    $("#CriticallyId").data("kendoDropDownList").value(response.CriticallyId);
                    setTimeout(function () {
                        arrival_databound();

                    });
                }
            });

            $.ajax({
                type: "Post",
                data: { ReceptionId: receptionId },
                url: "/Reception/GetReceptionAmbulance",
                success: function (response) {

                    if (response !== null && response !== "") {
                        $("#AmbulanceId").data("kendoDropDownList").value(response.AmbulanceId);
                        //$("#FromHospitalId").data("kendoDropDownList").value(response.FromHospitalId);
                        $("#FromHospitalName").data("kendoAutoComplete").value(response.FromHospitalName);
                        //$("#ToHospitalId").data("kendoDropDownList").value(response.ToHospitalId);
                        $("#ToHospitalName").data("kendoAutoComplete").value(response.ToHospitalName);
                        $("#PatientHealthId").data("kendoDropDownList").value(response.PatientHealthId);
                        $("#Cost").data("kendoNumericTextBox").value(response.Cost);
                        $("#AmbulanceExplanation").val(response.Explanation);
                    }

                }
            });

        };
    }
    else {
        $.ajax({
            type: "Post",
            //data: { ReceptionId: receptionId },
            url: "/PatientReception/GetLatestReceptionNum",
            success: function (response) {

                $("#ReceptionNum").val(response);
                $("#reception-num").text(response);
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

    };

    $.ajax({
        type: "Post",
        data: { settingName: "UseDollar" },
        url: "/ClinicSection/GetClinicSectionSettingValueBySettingName",
        success: function (response) {

            $("#useDollar").text(response);
            if (response === "true") {

                let all = $(".currencyId");
                all.removeClass("hidden");

            }

        }
    });

    //$.ajax({
    //    type: "Post",
    //    data: { ClinicSectionName: "Emergency" },
    //    url: "/ClinicSection/GetClinicSectionIdByName",
    //    success: function (response) {

    //        let sec = $("#ClinicSectionId").data("kendoDropDownList");
    //        sec.value(response);

    //    }
    //});


    TotalTranslated = $("#Translated").attr("data-TotalTranslated");
    DiscountTranslated = $("#Translated").attr("data-DiscountTranslated");
    PriceTranslated = $("#Translated").attr("data-PriceTranslated");
    CodeTranslated = $("#Translated").attr("data-CodeTranslated");
    TypeTranslated = $("#Translated").attr("data-TypeTranslated");
    NameTranslated = $("#Translated").attr("data-NameTranslated");
    AmountTranslated = $("#Translated").attr("data-AmountTranslated");
    DateTranslated = $("#Translated").attr("data-DateTranslated");

    //let arri = $("#ArrivalId").data("kendoDropDownList");

    //arri.bind("dataBound", arrival_databound);

    let purpose = $("#PurposeId").data("kendoDropDownList");

    purpose.bind("dataBound", PurposeChange);

    setTimeout(function () {
        $('#Name2').focus();

        //let sec = $("#ClinicSectionId").data("kendoDropDownList");

        //sec.select(function (dataItem) {
        //    return dataItem.name === "Emergency";
        //});

    });

    let eme = $("#Emergency").text();

    let secion = $("#ClinicSectionId").data("kendoDropDownList");
    secion.bind("dataBound", secion_dataBound);
    //if (eme.toLowerCase() === "true") {

    //    let secion = $("#ClinicSectionId").data("kendoDropDownList");
    //    secion.enable(false);

    //}
    //else {
    //    let secion = $("#ClinicSectionId").data("kendoDropDownList");
    //    secion.bind("dataBound", secion_dataBound);

    //}

});

let firstLoad = true;

function secion_dataBound() {

    let secion = $("#ClinicSectionId").data("kendoDropDownList");
    //let sec = secion.dataSource;
    var oldData = secion.dataSource.data();



    let eme = $("#Emergency").text();

    if (eme.toLowerCase() === "true") {

        let index = 0;
        for (let i = 0; i < oldData.length; i++) {
            if (oldData[i].Name === "Emergency") {
                index = i;
                break;
            }


        }
        secion.select(secion.ul.children().eq(index));
    }
    else {

        let index = 0;
        for (let i = 0; i < oldData.length; i++) {
            if (oldData[i].Name !== "Emergency") {
                index = i;
                break;
            }
        }
        secion.select(secion.ul.children().eq(index));

    }
    //if (firstLoad) {
    //    firstLoad = false;

    //    secion.dataSource.remove(oldData[0]);

    //}
    //else {
    //    secion.select(secion.ul.children().eq(0));
    //}
};

function CreateTelerikComponents() {

    $('#Name2').on('input', function () {
        var txt = $("#Name2").val();

        var filter_list = FilterPatientLists(patient_list, txt);

        $("#Name2").data("kendoAutoComplete").dataSource.data(filter_list);

    });

    nameAuto = $("#Name2").kendoAutoComplete({
        dataTextField: "PhoneNumberAndName",
        filter: "contains",
        //highlightFirst: true,
        clearButton: false,
        enable: enable,
        noDataTemplate: $("#noDataTemplate").html(),
        select: OnPatientSelectInEmergencyReception,
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
        //enable: enable,
    });

    $("#GenderId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        //enable: enable,
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
        //enable: enable,
        animation: false,
        change: changeBirthOfDatePatientDatetimePicker,
    });

    //let datetime = $("#DateOfBirth").data("kendoDatePicker");

    //datetime.enable(enable);
    $("#IdentityNumber").kendoTextBox({
        //enable: enable,
    });

    $("#Year").kendoTextBox({
        //enable: enable,
    });

    $("#Month").kendoTextBox({
        //enable: enable,
    });

    $('#DispatcherDoctor').on('input', function () {
        var txt = $("#DispatcherDoctor").val();

        var filter_list = FilterDoctorLists(doctor_list, txt);

        $("#DispatcherDoctor").data("kendoAutoComplete").dataSource.data(filter_list);

    });

    $("#DispatcherDoctor").kendoAutoComplete({
        dataTextField: "UserName",
        //enable: enable,
        clearButton: false,
        select: OnDispatcherDoctorSelect,
        filter: "contains",
        //filtering: OnDispatcherDoctorChange,
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Doctor/GetAllDoctors"
                }
            }
        }
    });

    $("#AddressName").kendoAutoComplete({
        dataTextField: "Name",
        filter: "contains",
        //highlightFirst: true,
        clearButton: false,
        //enable: enable,
        select: OnSelectAddress,
        dataSource: {
            //type: "odata",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetAllAddress"
                }
            }
        }
    }).data("kendoAutoComplete");

    $("#MotherName").kendoTextBox({
        //enable: enable,
    });

    $("#PatientAttendanceName").kendoTextBox({
        //enable: enable,
    });


    $("#Service").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        enable: enable,
        dataSource: {
            //type: "jsonp",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Service/GetAllServicesExceptOperation",
                }
            }
        }
    });

    $("#ServiceNumber").kendoTextBox({
        enable: enable,
    });

    $('#SurgeryOne').on('input', function () {
        var txt = $("#SurgeryOne").val();

        var filter_list = FilterDoctorLists(doctor_list, txt);

        $("#SurgeryOne").data("kendoAutoComplete").dataSource.data(filter_list);

    });

    $("#SurgeryOne").kendoAutoComplete({
        dataTextField: "UserName",
        filter: "contains",
        enable: enable,
        highlightFirst: true,
        clearButton: false,
        select: OnSurgeryOneSelect,
        dataSource: {
            //type: "odata",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Doctor/GetAllDoctors"
                }
            }
        }
    }).data("kendoAutoComplete");

    $("#Operation").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        enable: enable,
        filter: "contains",
        dataSource: {
            //type: "jsonp",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Service/GetAllOperationServices",
                }
            }
        }
    });

    $("#datetimepicker").kendoDateTimePicker({
        value: new Date(),
        //enable: enable,
        format: "dd/MM/yyyy HH:mm",
        dateInput: true
    });

    //let datetimepicker = $("#datetimepicker").data("kendoDateTimePicker");

    //datetimepicker.enable(enable);

    $("#ArrivalId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        enable: enable,
        select: selectArrival,
        dataSource: {
            //type: "jsonp",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetAllArrivals",
                }
            }
        }
    });

    $("#CriticallyId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        enable: enable,
        dataSource: {
            //type: "jsonp",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetAllCritically",
                }
            }
        }
    });

    $("#AmbulanceId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        enable: enable,
        dataSource: {
            //type: "jsonp",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Ambulance/GetAllAmbulances",
                }
            }
        }
    });


    $("#FromHospitalName").kendoAutoComplete({
        dataTextField: "Name",
        filter: "contains",
        enable: enable,
        //highlightFirst: true,
        clearButton: false,
        //select: OnSurgeryOneSelect,
        dataSource: {
            //type: "odata",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Hospital/GetAllHospitals",
                }
            }
        }
    }).data("kendoAutoComplete");
    //$("#FromHospitalId").kendoDropDownList({
    //    dataTextField: "Name",
    //    dataValueField: "Guid",
    //    enable: enable,
    //    dataSource: {
    //        //type: "jsonp",
    //        serverFiltering: false,
    //        transport: {
    //            read: {
    //                url: "/Hospital/GetAllHospitals",
    //            }
    //        }
    //    }
    //});




    $("#ToHospitalName").kendoAutoComplete({
        dataTextField: "Name",
        filter: "contains",
        enable: enable,
        //highlightFirst: true,
        clearButton: false,
        //select: OnSurgeryOneSelect,
        dataSource: {
            //type: "odata",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/Hospital/GetAllHospitals",
                }
            }
        }
    }).data("kendoAutoComplete");
    //$("#ToHospitalId").kendoDropDownList({
    //    dataTextField: "Name",
    //    dataValueField: "Guid",
    //    enable: enable,
    //    dataSource: {
    //        //type: "jsonp",
    //        serverFiltering: false,
    //        transport: {
    //            read: {
    //                url: "/Hospital/GetAllHospitals",
    //            }
    //        }
    //    }
    //});

    $("#PatientHealthId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Guid",
        enable: enable,
        dataSource: {
            //type: "jsonp",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetAllPatientHealth",
                }
            }
        }
    });

    $("#Cost").kendoNumericTextBox({
        format: "N0",
        enable: enable,
        min: 0,
        max: 10000000,
    });

    $("#CostCurrencyId").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        enable: enable,
        dataSource: {
            //type: "jsonp",
            serverFiltering: false,
            transport: {
                read: {
                    url: "/BaseInfo/GetAllCurrencies",
                }
            }
        }
    });

    $("#AmbulanceExplanation").kendoTextArea({
        rows: 2,
        maxLength: 200,
        enable: enable,
    });

    $("#Description").kendoTextArea({
        rows: 2,
        maxLength: 200,
        enable: enable,
    });

    $("#RoomBedName").kendoTextBox({
        enable: enable,
    });
    $(".k-nodata").addClass('hidden');
};


function SetReceptionAmounts(reception) {

    $("#PatientId").val(reception.PatientId);
    $("#EntranceDate").val(reception.EntranceDate);
    $("#ExitDate").val(reception.ExitDate);
    $("#AddressId").val(reception.Patient.AddressId);
    $("#IdentityNumber").val(reception.Patient.IdentityNumber);
    $("#ReceptionNum").val(reception.ReceptionNum);
    $("#reception-num").text(reception.ReceptionNum);
    $("#ReceptionDate").val(reception.ReceptionDate);
    $("#reception-date").text(reception.ReceptionDateString);
    $("#StatusId").val(reception.StatusId);
    $("#Guid").val(reception.Guid);
    $("#CreatedUserId").val(reception.CreatedUserId);
    $("#Name2").val(reception.Patient.User.Name);
    $("#PhoneNumber").val(reception.Patient.User.PhoneNumber);

    let gender = $("#GenderId").data("kendoDropDownList");

    gender.value(reception.Patient.User.GenderId);
    $("#DateOfBirth").val(reception.Patient.DateOfBirth);
    setTimeout(function () {
        let datepicker = $("#DateOfBirth").data("kendoDatePicker");
        let s = $("#DateOfBirth").attr('aria-owns');

        datepicker.open();

        datepicker.close();

        let s3 = $("#" + s + " .k-state-focused a").click();
    });
    $("#AddressName").val((reception.Patient.Address === null) ? "" : reception.Patient.Address.Name);
    $("#MotherName").val(reception.Patient.MotherName);
    $("#PatientAttendanceName").val(reception.PatientAttendanceName);
    let clinicSection = $("#ClinicSectionId").data("kendoDropDownList");
    clinicSection.value(reception.ClinicSectionId);

    let purpose = $("#PurposeId").data("kendoDropDownList");
    purpose.value(reception.PurposeId);
    purpose.enable(enable);
    PurposeChange();
};


function arrival_databound() {
    let arri = $("#ArrivalId").data("kendoDropDownList");
    if (arri.text() === "Ambulance") {

        $("#ambulanceSection").removeClass('hidden');
    };

};


function ReserveInsertAge(element) {


    var Year = parseInt($('#Year').val());
    var Month = parseInt($('#Month').val());
    if ($('#Month').val() === "" || Month === undefined) {
        Month = 0;
    };
    if ($('#Year').val() === "" || Year === undefined) {
        Year = 0;
    };

    if (Month > 11) {
        Month = 11;
        $('#Month').val('11');
    };
    var today = new Date();

    var birthDay = today;
    birthDay.setFullYear(birthDay.getFullYear() - Year);
    birthDay.setMonth(birthDay.getMonth() - Month);
    let birthMonth = birthDay.getMonth() + 1;
    if (birthMonth.toString() !== "10" && birthMonth.toString() !== "11" && birthMonth.toString() !== "12") {
        birthMonth = "0" + birthMonth.toString();
    };

    $("#DateOfBirthDay").val(1);
    $("#DateOfBirthMonth").val(birthMonth);
    $("#DateOfBirthYear").val(birthDay.getFullYear());

    $('#DateOfBirth').val('01/' + birthMonth + '/' + birthDay.getFullYear());

};



function bodyPadding() {

    $('body').addClass('body-padding');
}

var Reception;
var Patient;


function PrintReception() {

    let dateTimePicker = $("#datetimepicker").data("kendoDateTimePicker").value();



    let operationDate = "";
    let operation = "";

    if ($("#PurposeId").data("kendoDropDownList").text() === "Surgery") {
        operationDate = dateTimePicker.getDate() + "/" + (dateTimePicker.getMonth() + 1) + "/" + dateTimePicker.getFullYear() + "  " + dateTimePicker.getHours() + ":" + dateTimePicker.getMinutes();
        operation = $("#Operation").data("kendoDropDownList").text();
    }

    let receptionInformation = {

        PatientName: $("#Name2").val(),
        MotherName: $("#MotherName").val(),
        AttendantName: $("#PatientAttendanceName").val(),
        Gender: $("#GenderId").data("kendoDropDownList").text(),
        Age: $("#Year").val(),
        Room: $("#RoomBedId").data("kendoDropDownList").text(),
        EntranceDate: $("#reception-date").text(),
        OperationDate: operationDate,
        Operation: operation,
        Surgery: $("#SurgeryOne").val(),
        DispatcherDoctor: $("#DispatcherDoctor").val(),
        Address: $("#AddressName").val(),
        Phone: $("#PhoneNumber").val(),
        IdentityNumber: $("#IdentityNumber").val()
    }

    let confirm = AggregateData();

    if (confirm) {
        $(".loader").removeClass("hidden");

        var token = $(':input:hidden[name*="RequestVerificationToken"]');

        let link = "/Reception/AddReception";
        let newlink = "/PatientReception/Form";

        let emergency = $("#Emergency").text();

        if (emergency.toLowerCase() === "true" || emergency === true) {
            link = "/EmergencyReception/AddReception";
            newlink = "/PatientReception/Form";
        };

        reception_formData.append('__RequestVerificationToken', token.attr('value'));
        var main_images = getImagesFromDiv('main_attachments_img');

        for (let i = 0; i < main_images.length; i++) {
            reception_formData.append('MainAttachments', main_images[i]);
        };

        var other_images = getImagesFromDiv('other_attachments_img');

        for (let i = 0; i < other_images.length; i++) {
            reception_formData.append('OtherAttachments', other_images[i]);
        };

        var police_images = getImagesFromDiv('polic_report_attachments_img');

        for (let i = 0; i < police_images.length; i++) {
            reception_formData.append('PoliceReport', police_images[i]);
        };


        $.ajax({
            type: "Post",
            url: link,
            data: reception_formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response !== 0) {

                    $.ajax({
                        type: "Post",
                        url: "/Reception/PrintReceptionReport",
                        data: receptionInformation,
                        success: function (response) {
                            if (response !== 0) {
                                draw2(response);
                                $(".page-content").load(newlink, function (responce) {

                                    $(".loader").fadeIn("slow");
                                    $(".loader").addClass("hidden");
                                });

                            };
                        }
                    });

                };
            }
        });
    };

    bodyPadding();

}


function Exit() {
    $(".page-content").load("/PatientReception/Form", function (responce) {

        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    });
};


async function OkAndExit() {

    let confirm = AggregateData();

    if (confirm) {
        $(".loader").removeClass("hidden");

        var token = $(':input:hidden[name*="RequestVerificationToken"]');

        let link = "/Reception/AddReception";
        let newlink = "/PatientReception/Form";

        let emergency = $("#Emergency").text();

        if (emergency.toLowerCase() === "true" || emergency === true) {
            link = "/EmergencyReception/AddReception";
            newlink = "/PatientReception/Form";
        };

        reception_formData.append('__RequestVerificationToken', token.attr('value'));
        var main_images = await getImagesFromDiv('main_attachments_img');

        for (let i = 0; i < main_images.length; i++) {
            reception_formData.append('MainAttachments', main_images[i]);
        };

        var other_images = await getImagesFromDiv('other_attachments_img');

        for (let i = 0; i < other_images.length; i++) {
            reception_formData.append('OtherAttachments', other_images[i]);
        };

        var police_images = await getImagesFromDiv('polic_report_attachments_img');

        for (let i = 0; i < police_images.length; i++) {
            reception_formData.append('PoliceReport', police_images[i]);
        };


        $.ajax({
            type: "Post",
            url: link,
            data: reception_formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response !== 0) {


                    if (response === "InsertPriceVisit") {

                        $("#PriceVisit-box").removeClass('hidden');
                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");


                    } else {

                        $(".page-content").load(newlink, function (responce) {

                            $(".loader").fadeIn("slow");
                            $(".loader").addClass("hidden");
                        });
                    }


                } else {

                    $('#ERROR_WrongReception').removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");


                    window.scrollTo(0, document.body.scrollHeight);
                }
            }
        });
    };

    bodyPadding();

};

async function OkAndNew() {

    let confirm = AggregateData();

    if (confirm) {
        $(".loader").removeClass("hidden");

        var token = $(':input:hidden[name*="RequestVerificationToken"]');

        let link = "/Reception/AddReception";
        let newlink = "/Reception/Form";

        let emergency = $("#Emergency").text();

        if (emergency.toLowerCase() === "true" || emergency === true) {
            link = "/EmergencyReception/AddReception";
            newlink = "/EmergencyReception/Form";
        };

        reception_formData.append('__RequestVerificationToken', token.attr('value'));
        var main_images = await getImagesFromDiv('main_attachments_img');

        for (let i = 0; i < main_images.length; i++) {
            reception_formData.append('MainAttachments', main_images[i]);
        };

        var other_images = await getImagesFromDiv('other_attachments_img');

        for (let i = 0; i < other_images.length; i++) {
            reception_formData.append('OtherAttachments', other_images[i]);
        };

        var police_images = await getImagesFromDiv('polic_report_attachments_img');

        for (let i = 0; i < police_images.length; i++) {
            reception_formData.append('PoliceReport', police_images[i]);
        };

        $.ajax({
            type: "Post",
            url: link,
            data: reception_formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response !== 0) {

                    if (response === "InsertPriceVisit") {

                        $("#PriceVisit-box").removeClass('hidden');
                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");


                    } else {

                        $(".page-content").load(newlink, function (responce) {

                            $(".loader").fadeIn("slow");
                            $(".loader").addClass("hidden");
                        });
                    }


                } else {

                    $('#ERROR_WrongReception').removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                    window.scrollTo(0, document.body.scrollHeight);

                }
            }
        });
    };

    bodyPadding();

}

function AggregateData() {
    $("#ERROR_WrongReception").addClass('hidden');
    $("#SurgeryOne-box").addClass('hidden');
    $("#PriceVisit-box").addClass('hidden');
    $("#Mobile-wrong").addClass('hidden');

    reception_formData = new FormData();
    let name = $("#Name2").val().replace(/ /g, '');

    if (name === "") {


        bootbox.dialog({
            message: "<h5 class='MyFont-Roboto-header'> Please Fill Patinet Name </h5>",
            className: 'bootbox-class MyFont-Roboto-header',
            size: 'small',

        });

        window.setTimeout(function () {
            bootbox.hideAll();
            $("#Name2").focus();
        }, 2000);


        bodyPadding();

        return false;
    };

    var mobile = $("#Mobile-wrong");
    if (mobile) {
        mobile.addClass("hidden");

        if ($('[validate-mobile="Mobile-wrong"]').val() !== undefined) {
            let text = $('[validate-mobile="Mobile-wrong"]').val();
            if (text != "" & text.trim().length < 8) {
                mobile.removeClass("hidden");
                return;
            }
        }
    }



    let date = $("#DateOfBirth").data("kendoDatePicker").value();

    //let usedollar = $("#useDollar").text();
    let emergency = $("#Emergency").text();

    //let receives = 11;

    reception_formData.append('Patient.Guid', $("#PatientId").val());
    reception_formData.append('Patient.Name', $("#Name2").val());
    reception_formData.append('Patient.GenderId', $("#GenderId").val());
    reception_formData.append('Patient.PhoneNumber', $("#PhoneNumber").val());
    reception_formData.append('Patient.AddressId', $("#AddressId").val());
    reception_formData.append('Patient.DateOfBirthDay', $("#DateOfBirthDay").val());
    reception_formData.append('Patient.DateOfBirthMonth', $("#DateOfBirthMonth").val());
    reception_formData.append('Patient.DateOfBirthYear', $("#DateOfBirthYear").val());
    reception_formData.append('Patient.MotherName', $("#MotherName").val());
    reception_formData.append('Patient.IdentityNumber', $("#IdentityNumber").val());
    reception_formData.append('Patient.AddressName', $("#AddressName").val());

    //Patient = {
    //    Guid: $("#PatientId").val(),
    //    Name: $("#Name2").val(),
    //    GenderId: $("#GenderId").val(),
    //    PhoneNumber: $("#PhoneNumber").val(),
    //    AddressId: $("#AddressId").val(),
    //    DateOfBirthDay: date.getDate(),
    //    DateOfBirthMonth: date.getMonth() + 1,
    //    DateOfBirthYear: date.getFullYear(),
    //    MotherName: $("#MotherName").val(),
    //    AddressName: $("#AddressName").val()
    //};


    let Emergency = "";
    let Ambulance = "";

    if (emergency.toLowerCase() === "true" || emergency === true) {


        reception_formData.append('Emergency.Guid', $("#EmergencyId").val());
        reception_formData.append('Emergency.ArrivalId', $("#ArrivalId").val());
        reception_formData.append('Emergency.CriticallyId', $("#CriticallyId").val());

        //Emergency = {

        //    Guid: $("#EmergencyId").val(),
        //    ArrivalId: $("#ArrivalId").val(),
        //    CriticallyId: $("#CriticallyId").val(),

        //};

        if (!$("#ambulanceSection").hasClass('hidden')) {

            reception_formData.append('ReceptionAmbulance.Guid', $("#ReceptionAmbulancesId").val());
            reception_formData.append('ReceptionAmbulance.AmbulanceId', $("#AmbulanceId").val());
            //reception_formData.append('ReceptionAmbulance.FromHospitalId', $("#FromHospitalId").val());
            reception_formData.append('ReceptionAmbulance.FromHospitalName', $("#FromHospitalName").val());
            //reception_formData.append('ReceptionAmbulance.ToHospitalId', $("#ToHospitalId").val());
            reception_formData.append('ReceptionAmbulance.ToHospitalName', $("#ToHospitalName").val());
            reception_formData.append('ReceptionAmbulance.PatientHealthId', $("#PatientHealthId").val());
            reception_formData.append('ReceptionAmbulance.Cost', $("#Cost").val());
            reception_formData.append('ReceptionAmbulance.Explanation', $("#AmbulanceExplanation").val());
            reception_formData.append('ReceptionAmbulance.CostCurrencyId', $("#CostCurrencyId").val());

            //Ambulance = {

            //    Guid: $("#ReceptionAmbulancesId").val(),
            //    AmbulanceId: $("#AmbulanceId").val(),
            //    FromHospitalId: $("#FromHospitalId").val(),
            //    ToHospitalId: $("#ToHospitalId").val(),
            //    PatientHealthId: $("#PatientHealthId").val(),
            //    Cost: $("#Cost").val(),
            //    Explanation: $("#AmbulanceExplanation").val(),
            //    CostCurrencyId: $("#CostCurrencyId").val(),

            //};

        };

    };

    let purpose = $("#PurposeId").data("kendoDropDownList").text();
    let service = $("#Service").data("kendoDropDownList");
    let operation = $("#Operation").data("kendoDropDownList");

    let serviceId = null;
    let surgeryTime = null;
    let surgeryOne = null;
    let DispatcherDoctor = null;
    let PriceVisit = null;

    if (purpose === "Service") {
        serviceId = service.value();
    };

    if (purpose === "Surgery") {
        serviceId = operation.value();
        surgeryTime = $("#datetimepicker").val();
        surgeryOne = $("#SurgeryOneVal").text();
        DispatcherDoctor = $("#DispatcherDoctorVal").text();
        if ($("#SurgeryOne").val() === "" || $("#SurgeryOne").val() === null) {
            $("#SurgeryOne-box").removeClass('hidden');
            return;
        };
    };

    if (purpose === "Observation") {
        PriceVisit = $("#PriceVisit").val();
        if (PriceVisit === "" || PriceVisit === null) {
            $("#PriceVisit-box").removeClass('hidden');
            return;
        };
    };

    let dateTimePicker = $("#datetimepicker").data("kendoDateTimePicker").value();

    reception_formData.append('Guid', $("#Guid").val());
    reception_formData.append('CreatedUserId', $("#CreatedUserId").val());
    reception_formData.append('CreatedDate', $("#CreatedDate").val());
    reception_formData.append('PatientId', $("#PatientId").val());
    //reception_formData.append('Patient', Patient);
    reception_formData.append('EntranceDate', $("#EntranceDate").val());
    reception_formData.append('ExitDate', $("#ExitDate").val());
    reception_formData.append('PatientAttendanceName', $("#PatientAttendanceName").val());
    reception_formData.append('DiscountCurrencyId', 11);
    reception_formData.append('Description', $("#Description").val());
    reception_formData.append('ReceptionNum', $("#ReceptionNum").val());
    reception_formData.append('ReceptionDate', $("#ReceptionDate").val());
    reception_formData.append('ClinicSectionId', $("#ClinicSectionId").val());
    reception_formData.append('RoomBedId', $("#RoomBedId").val());
    //reception_formData.append('PoliceReport', $("#PoliceReport").val());
    //reception_formData.append('Emergency', Emergency);
    //reception_formData.append('ReceptionAmbulance', Ambulance);
    reception_formData.append('PurposeName', purpose);
    reception_formData.append('PurposeId', $("#PurposeId").data("kendoDropDownList").value());
    reception_formData.append('SurgeryOne', surgeryOne);
    reception_formData.append('SurgeryOneName', $("#SurgeryOne").val());
    reception_formData.append('DispatcherDoctor', $("#SurgeryOne").val());
    reception_formData.append('DispatcherDoctorName', $("#DispatcherDoctor").val());
    reception_formData.append('ServiceId', serviceId);
    reception_formData.append('SurgeryTime', surgeryTime);
    reception_formData.append('SurgeryTimeYear', dateTimePicker.getFullYear());
    reception_formData.append('SurgeryTimeMonth', dateTimePicker.getMonth() + 1);
    reception_formData.append('SurgeryTimeDay', dateTimePicker.getDate());
    reception_formData.append('SurgeryTimeTime', dateTimePicker.getHours() + ":" + dateTimePicker.getMinutes());
    reception_formData.append('ServiceNumber', $("#ServiceNumber").val());
    reception_formData.append('ClinicSectionName', $("#ClinicSectionId").data("kendoDropDownList").text());
    reception_formData.append('PriceVisit', PriceVisit);

    //Reception = {
    //    Guid: $("#Guid").val(),
    //    CreatedUserId: $("#CreatedUserId").val(),
    //    CreatedDate: $("#CreatedDate").val(),
    //    PatientId: $("#PatientId").val(),
    //    Patient: Patient,
    //    EntranceDate: $("#EntranceDate").val(),
    //    ExitDate: $("#ExitDate").val(),
    //    PatientAttendanceName: $("#PatientAttendanceName").val(),
    //    DiscountCurrencyId: 11,
    //    Description: $("#Description").val(),
    //    ReceptionNum: $("#ReceptionNum").val(),
    //    ReceptionDate: $("#ReceptionDate").val(),
    //    ClinicSectionId: $("#ClinicSectionId").val(),
    //    RoomBedId: $("#RoomBedId").val(),
    //    PoliceReport: $("#PoliceReport").val(),
    //    Emergency: Emergency,
    //    ReceptionAmbulance: Ambulance,
    //    Purpose: purpose,
    //    ServiceId: serviceId,
    //    ServiceNumber: $("#ServiceNumber").val(),
    //    ClinicSectionName: $("#ClinicSectionId").data("kendoDropDownList").text()
    //};


    return true;
}

var RTL = $("#RTL").attr('data-Value');
var temp_room_bed_id = 'null';
var temp_reception_value = 'null';

function readImagesFromServer(path, div_id, lbl_id) {
    $.ajax({
        type: "Get",
        url: path,
        success: function (res) {
            if (res != 0) {
                var div = document.getElementById(div_id);
                var count = div.getElementsByTagName("img").length;
                for (let i = 0; i < res.length; i++) {
                    $("#" + div_id).append(
                        $("<span id='" + res[i].Guid + "' class='saved_attachments_image_container'>" +
                            "<span class='fa fa-close saved_attachments_image_remove' data-id='" + res[i].Guid + "' onclick='deleteSavedAttachment(this)'></span>" +
                            "<img ondblclick='openSavedPic(this)' class='saved_attachments_image_thumb' data-src='" + res[i].ImageAddress + "'  src=\"" + res[i].ThumbNailAddress + "\" />" +
                            "</span>")
                    );
                };
                count = count + res.length;
                document.getElementById(lbl_id).innerText = count + " file";
            };
        }
    });
};

function openPic(e) {

    //var destenation = $(e).next().next().text();
    var destenation1 = e.src;

    $("#imageEditor").empty();

    this._kendoImageEditor = $("#imageEditor").kendoImageEditor({
        imageUrl: destenation1,
        width: "100%",
        //height: 900,
        saveAs: {
            fileName: "image_edited.png"
        },
        toolbar: {
            items: [
                //"save",
                "crop",
                "resize",
                "undo",
                "redo",
                "zoomIn",
                "zoomOut",
                {
                    type: "button",
                    icon: "rotate",
                    text: "Rotate",
                    enable: true,
                    command: "RotateImageRightImageEditorCommand"
                },
                {
                    type: "button",
                    text: "Save",
                    icon: "save",
                    enable: true,
                    command: "SaveOnPreviousFile"
                },
                //{
                //    type: "button",
                //    text: "Save As",
                //    icon: "files",
                //    enable: true,
                //    command: "SaveAsNewFile"
                //},
            ]
        }
    }).data("kendoImageEditor");


    var imageEditorNS = kendo.ui.imageeditor;

    imageEditorNS.commands.RotateImageRightImageEditorCommand = imageEditorNS.ImageEditorCommand.extend({
        exec: function () {
            var that = this,
                options = that.options,
                imageeditor = that.imageeditor,
                canvas = imageeditor.getCanvasElement(),
                ctx = imageeditor.getCurrent2dContext(),
                image = imageeditor.getCurrentImage();

            let degrees = 90; //rotate right

            canvas.width = image.height;
            canvas.height = image.width;

            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.translate(image.height / 2, image.width / 2);

            ctx.rotate(degrees * Math.PI / 180);
            ctx.drawImage(image, -image.width / 2, -image.height / 2);

            imageeditor.drawImage(canvas.toDataURL()).done(function (image) {
                imageeditor.drawCanvas(image);
            }).fail(function (ev) {
                imageeditor.trigger("error", ev);
            });
        }
    });

    imageEditorNS.commands.SaveOnPreviousFile = imageEditorNS.ImageEditorCommand.extend({
        exec: function () {
            var that = this,
                options = that.options,
                imageeditor = that.imageeditor,
                canvas = imageeditor.getCanvasElement();
            //ctx = imageeditor.getCurrent2dContext(),
            //image = imageeditor.getCurrentImage();

            let base64 = canvas.toDataURL();
            let address = destenation1;

            e.src = base64;

            //$.ajax({
            //    type: "Post",
            //    data: { Base: base64, Address: address, New: false },
            //    url: "/ControlPanel/Document/SaveEditedImage",
            //    success: function (response) {
            //        $("#window").data("kendoWindow").close();
            //        filemanager.refresh();
            //    }
            //});

        }
    });


    //imageEditorNS.commands.SaveAsNewFile = imageEditorNS.ImageEditorCommand.extend({
    //    exec: function () {
    //        var that = this,
    //            options = that.options,
    //            imageeditor = that.imageeditor,
    //            canvas = imageeditor.getCanvasElement();
    //        //ctx = imageeditor.getCurrent2dContext(),
    //        //image = imageeditor.getCurrentImage();
    //        let base64 = canvas.toDataURL();
    //        let address = destenation1;
    //        $.ajax({
    //            type: "Post",
    //            data: { Base: base64, Address: address, New: true },
    //            url: "/ControlPanel/Document/SaveEditedImage",
    //            success: function (response) {
    //                $("#window").data("kendoWindow").close();
    //                filemanager.refresh();
    //            }
    //        });
    //    }
    //});

    //$("#imagePreview").attr("src", destenation1);
    $("#window").data("kendoWindow").center().open();
}

function openSavedPic(e) {

    //var destenation = $(e).next().next().text();
    //var destenation1 = e.src;
    var destenation1 = $(e).attr('data-src');

    $("#imageEditor").empty();

    this._kendoImageEditor = $("#imageEditor").kendoImageEditor({
        imageUrl: destenation1,
        width: "100%",
        //height: 900,
        saveAs: {
            fileName: "image_edited.png"
        },
        toolbar: {
            items: [
                //"save",
                //"crop",
                //"resize",
                //"undo",
                //"redo",
                "zoomIn",
                "zoomOut",
                //{
                //    type: "button",
                //    icon: "rotate",
                //    text: "Rotate",
                //    enable: true,
                //    command: "RotateImageRightImageEditorCommand"
                //},
                //{
                //    type: "button",
                //    text: "Save",
                //    icon: "save",
                //    enable: true,
                //    command: "SaveOnPreviousFile"
                //},
                //{
                //    type: "button",
                //    text: "Save As",
                //    icon: "files",
                //    enable: true,
                //    command: "SaveAsNewFile"
                //},
            ]
        }
    }).data("kendoImageEditor");


    var imageEditorNS = kendo.ui.imageeditor;

    imageEditorNS.commands.RotateImageRightImageEditorCommand = imageEditorNS.ImageEditorCommand.extend({
        exec: function () {
            var that = this,
                options = that.options,
                imageeditor = that.imageeditor,
                canvas = imageeditor.getCanvasElement(),
                ctx = imageeditor.getCurrent2dContext(),
                image = imageeditor.getCurrentImage();

            let degrees = 90; //rotate right

            canvas.width = image.height;
            canvas.height = image.width;

            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.translate(image.height / 2, image.width / 2);

            ctx.rotate(degrees * Math.PI / 180);
            ctx.drawImage(image, -image.width / 2, -image.height / 2);

            imageeditor.drawImage(canvas.toDataURL()).done(function (image) {
                imageeditor.drawCanvas(image);
            }).fail(function (ev) {
                imageeditor.trigger("error", ev);
            });
        }
    });

    imageEditorNS.commands.SaveOnPreviousFile = imageEditorNS.ImageEditorCommand.extend({
        exec: function () {
            var that = this,
                options = that.options,
                imageeditor = that.imageeditor,
                canvas = imageeditor.getCanvasElement();
            //ctx = imageeditor.getCurrent2dContext(),
            //image = imageeditor.getCurrentImage();

            let base64 = canvas.toDataURL();
            let address = destenation1;

            e.src = base64;

            //$.ajax({
            //    type: "Post",
            //    data: { Base: base64, Address: address, New: false },
            //    url: "/ControlPanel/Document/SaveEditedImage",
            //    success: function (response) {
            //        $("#window").data("kendoWindow").close();
            //        filemanager.refresh();
            //    }
            //});

        }
    });


    //imageEditorNS.commands.SaveAsNewFile = imageEditorNS.ImageEditorCommand.extend({
    //    exec: function () {
    //        var that = this,
    //            options = that.options,
    //            imageeditor = that.imageeditor,
    //            canvas = imageeditor.getCanvasElement();
    //        //ctx = imageeditor.getCurrent2dContext(),
    //        //image = imageeditor.getCurrentImage();
    //        let base64 = canvas.toDataURL();
    //        let address = destenation1;
    //        $.ajax({
    //            type: "Post",
    //            data: { Base: base64, Address: address, New: true },
    //            url: "/ControlPanel/Document/SaveEditedImage",
    //            success: function (response) {
    //                $("#window").data("kendoWindow").close();
    //                filemanager.refresh();
    //            }
    //        });
    //    }
    //});

    //$("#imagePreview").attr("src", destenation1);
    $("#window").data("kendoWindow").center().open();
}

function deleteSavedAttachment(element) {

    $("#DeleteSavedAttachmentModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#DeleteSavedAttachmentModal #ERROR_SomeThingWentWrong").addClass("hidden");

    $(".loader").removeClass("hidden");
    $('#DeleteSavedAttachmentModal').modal('toggle');
    var Id = $(element).attr('data-id');
    $('#btn-deleteSavedAttachment-accept').attr('data-id', Id);
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}

$('#btn-deleteSavedAttachment-accept').on("click", function () {
    $(this).attr("disabled", true);

    $("#DeleteSavedAttachmentModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#DeleteSavedAttachmentModal #ERROR_SomeThingWentWrong").addClass("hidden");

    var Id = $(this).attr('data-id');

    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Get",
        url: "/PatientImage/remove/" + Id,
        success: function (response) {
            $('#btn-deleteSavedAttachment-accept').removeAttr("disabled");

            if (response === "SUCCESSFUL") {
                $('#DeleteSavedAttachmentModal').modal('hide');
                $(".modal-backdrop:last").remove();
                $("#" + Id + "").remove();
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
                setFileCount();
            }
            else if (response === "ERROR_SomeThingWentWrong") {
                $("#DeleteSavedAttachmentModal #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "ERROR_ThisRecordHasDependencyOnItInAnotherEntity") {
                $("#DeleteSavedAttachmentModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            };
        }
    });
});

$("#lbl_other_attachments").click(function (e) {
    $('#otherAttachmentsUpload').trigger('click');
});

$('#other_attachments').on('click', function (e) {
    if (!e.target.matches("#lbl_other_attachments")) {
        $("#other_attachments span:first").toggleClass("rotate");
        $("#other_attachments_container").slideToggle(200, function () {
            // Animation complete.
        });
    };
});

$("#lbl_main_attachments").click(function () {
    $('#mainAttachmentsUpload').trigger('click');
});

$('#main_attachments').on('click', function (e) {
    if (!e.target.matches("#lbl_main_attachments")) {
        $("#main_attachments span:first").toggleClass("rotate");
        $("#main_attachments_container").slideToggle(200, function () {
            // Animation complete.
        });
    };
});


$("#lbl_polic_report_attachments").click(function () {
    $('#policReportAttachmentsUpload').trigger('click');
});

$('#polic_report_attachments').on('click', function (e) {
    if (!e.target.matches("#lbl_polic_report_attachments")) {
        $("#polic_report_attachments span:first").toggleClass("rotate");
        $("#polic_report_attachments_container").slideToggle(200, function () {
            // Animation complete.
        });
    };
});

function resizeImage(img_src, data_type) {
    return new Promise((resolve) => {
        let img = new Image();
        img.src = img_src;
        img.onload = () => {

            var MAX_WIDTH = 1000;
            var MAX_HEIGHT = 1000;

            let width = img.width;
            let height = img.height;

            if (width > height) {
                if (width > MAX_WIDTH) {
                    height = height * (MAX_WIDTH / width);
                    width = MAX_WIDTH;
                };
            } else {
                if (height > MAX_HEIGHT) {
                    width = width * (MAX_HEIGHT / height);
                    height = MAX_HEIGHT;
                };
            };

            var canvas = document.createElement("canvas");
            canvas.width = width;
            canvas.height = height;
            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0, width, height);
            resolve(canvas.toDataURL(data_type));
        };
    });
};

function dataURLtoFile(dataurl, filename) {
    var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    };
    return new File([u8arr], filename, { type: mime });
}

async function getImagesFromDiv(divId) {
    let files = [];
    var div = document.getElementById(divId);
    var images = div.getElementsByClassName("attachments_image_thumb");
    for (let i = 0; i < images.length; i++) {
        let img = images[i];
        if (img.naturalWidth > 1000 || img.naturalHeight > 1000) {
            let response = await resizeImage($(img).attr('src'), $(img).attr('data-type'));
            img.src = response;
        };
        files.push(dataURLtoFile($(img).attr('src'), $(img).attr('name')));
    };
    return files;
}

function checkName(container_id, _name) {
    return new Promise((resolve) => {
        var div = document.getElementById(container_id);
        var images = div.getElementsByClassName("attachments_image_thumb");
        if (images.length === 0) {
            resolve(false);
        } else {
            for (let i = 0; i < images.length; i++) {
                if ($(images[i]).attr('name') == _name) {
                    resolve(true);
                };
            };
            resolve(false);
        };
    });
}

function setFileCount() {
    var div_main = document.getElementById('main_attachments_img');
    //var count_main=div_main.getElementsByClassName("attachments_image_thumb").length;
    var count_main = div_main.getElementsByTagName("img").length;
    document.getElementById('lbl_main_attachments_count').innerText = count_main + " file";

    var div_other = document.getElementById('other_attachments_img');
    //var count_other=div_other.getElementsByClassName("attachments_image_thumb").length;
    var count_other = div_other.getElementsByTagName("img").length;
    document.getElementById('lbl_other_attachments_count').innerText = count_other + " file";

    var div_polic_report = document.getElementById('polic_report_attachments_img');
    //var count_polic_report=div_polic_report.getElementsByClassName("attachments_image_thumb").length;
    var count_polic_report = div_polic_report.getElementsByTagName("img").length;
    document.getElementById('lbl_polic_report_attachments_count').innerText = count_polic_report + " file";
}

async function readURLAttachments(input, container_id) {
    var div = document.getElementById(container_id);
    //let count=div.getElementsByClassName("attachments_image_thumb").length;
    let count = div.getElementsByTagName("img").length;
    if (input.files && input.files.length > 0) {
        for (let i = 0; i < input.files.length; i++) {

            let img = input.files[i];
            let response = await checkName(container_id, img.name);

            if (!img.type.match('image') || response == true) continue;
            var reader = new FileReader();
            reader.onload = function (e) {

                $("#" + container_id).append(
                    $("<span class='attachments_image_container'>" +
                        "<span class='fa fa-close attachments_image_remove' ></span>" +
                        "<img ondblclick='openPic(this)' class='attachments_image_thumb' data-type='" + img.type + "' data-size='" + img.size + "' name='" + img.name + "' src=\"" + e.target.result + "\" />" +
                        "</span>")
                );
                $(".attachments_image_remove").click(function () {
                    $(this).parent(".attachments_image_container").remove();
                    setFileCount();
                });
            }
            count++;
            reader.readAsDataURL(img);
        };
    };
    return count;
};

$("#otherAttachmentsUpload").change(async function () {
    let count = await readURLAttachments(this, 'other_attachments_img');
    document.getElementById('lbl_other_attachments_count').innerText = count + " file";
});

$("#mainAttachmentsUpload").change(async function () {
    let count = await readURLAttachments(this, 'main_attachments_img');
    document.getElementById('lbl_main_attachments_count').innerText = count + " file";
});

$("#policReportAttachmentsUpload").change(async function () {
    let count = await readURLAttachments(this, 'polic_report_attachments_img');
    document.getElementById('lbl_polic_report_attachments_count').innerText = count + " file";
});



function ChosseAnalysisModal() {

    $('#ChosseAnalysisModal').modal('toggle');

    createChooseAnalysisTabs();

};


function addNewAddress() {
    if ($("#Name").val() === "") {
        $("#Name-box").removeClass('hidden');
        return;
    };

    var link = "/BaseInfo/AddOrUpdate";
    var GridRefreshLink = "/BaseInfo/RefreshGrid";
    var data = $("#addNewForm").serialize();
    var AddressId;
    if (Address)
        AddressId = $("#Address").attr('data-Value');
    else
        AddressId = $("#Speciallity").attr('data-Value');


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

                    $.ajax({
                        type: "Post",
                        data: { BaseInfoId: BaseInfoId, BaseInfoTypeId: AddressId },
                        url: "/BaseInfo/AddBaseInfoType",
                        success: function (response) {
                            if (Address) {
                                addressAuto.dataSource.read();
                            }
                            else
                                SpeciallityDropDown.dataSource.read();
                        }
                    });



                    $('#AddressModal').modal('hide');
                    $(".modal-backdrop:last").remove();
                    $('#AddressModal-body').empty();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    SpeciallityDropDown.focus();
                    SpeciallityDropDown.open();
                };
            };
        }
    });
}



function PurposeChange() {

    let purpose = $("#PurposeId").data("kendoDropDownList");

    if (purpose.text() === "Service") {

        $(".serviceSection").removeClass("hidden");
    }
    else {
        $(".serviceSection").addClass("hidden");
    };

    if (purpose.text() === "Surgery") {

        $(".surgerySection").removeClass("hidden");
    }
    else {
        $(".surgerySection").addClass("hidden");
    };

    if (purpose.text() === "Observation") {

        $(".observationSection").removeClass("hidden");
    }
    else {
        $(".observationSection").addClass("hidden");
    };
}


function closeConfirmSelectPatientBedModal() {

    $('#ConfirmSelectPatientBedModal').modal('hide');
    $(".modal-backdrop:last").remove();

}

function confirmSelectPatientBedModal() {

    if (!$("#SelectRoomBedModal #ERROR_SomeThingWentWrong").hasClass("hidden")) {
        $("#SelectRoomBedModal #ERROR_SomeThingWentWrong").addClass("hidden");
    };

    $.ajax({
        type: "post",
        url: "/ReceptionRoomBed/CloseOldReceptionRoomBed?receptionId=" + temp_reception_value,
        success: function (res) {
            if (res == 0) {
                var dropdownlist = $("#RoomBedId").data("kendoDropDownList");
                dropdownlist.dataSource.read();
                dropdownlist.value(temp_room_bed_id);
                hideSelectRoomBedModal();
                closeConfirmSelectPatientBedModal();
            } else {
                closeConfirmSelectPatientBedModal();
                $("#SelectRoomBedModal #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            };
        }
    });
}

function SelectRoomBed() {
    $('#Bed_Select').addClass('hidden');
    $('#SelectRoomBedModal #ERROR_SomeThingWentWrong').addClass('hidden');
    $('#SelectRoomBedModal').modal('toggle');
    var link = "/Room/SelectModal?ClinicSectionId=";
    var Id = document.getElementById("ClinicSectionId").value;
    document.getElementById('TempRoomBedId').value = "";
    $(".loader").removeClass("hidden");
    $('#SelectRoomBedModal-body').load(link + Id + '', function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");

    });
}

function selectNewRoomBed() {
    if (!$("#ConfirmSelectPatientBedModal #ChangePatientBed").hasClass("hidden")) {
        $("#ConfirmSelectPatientBedModal #ChangePatientBed").addClass("hidden");
    };

    if (!$("#ConfirmSelectPatientBedModal #ChangeBedPatient").hasClass("hidden")) {
        $("#ConfirmSelectPatientBedModal #ChangeBedPatient").addClass("hidden");
    };

    if (!$("#ConfirmSelectPatientBedModal #SwapPatientsBeds").hasClass("hidden")) {
        $("#ConfirmSelectPatientBedModal #SwapPatientsBeds").addClass("hidden");
    };

    if (temp_room_bed_id == "null") {
        $("#Bed_Select").removeClass("hidden");
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        return;
    };

    if (temp_reception_value != 'null') {
        $(".loader").removeClass("hidden");
        $("#ConfirmSelectPatientBedModal #ChangeBedPatient").removeClass("hidden");
        $('#ConfirmSelectPatientBedModal').modal('toggle');
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        return;
    };

    var dropdownlist = $("#RoomBedId").data("kendoDropDownList");
    dropdownlist.dataSource.read();
    dropdownlist.value(temp_room_bed_id);
    hideSelectRoomBedModal();


    //$.ajax({
    //    type: "Get",
    //    url: "/RoomBed/RoomName?Id="+temp_room_bed_id,
    //    success: function (res) {
    //        if(res != 0){
    //            //document.getElementById('RoomBedId').value=room_val;
    //            dropdownlist.value(temp_room_bed_id);
    //            //document.getElementById('RoomBedName').value=res;
    //            hideSelectRoomBedModal();
    //        }else{
    //            closeSelectRoomBedModal();
    //        }
    //    }
    //});
}

function closeSelectRoomBedModal() {
    var dropdownlist = $("#RoomBedId").data("kendoDropDownList");
    dropdownlist.dataSource.read();
    document.getElementById('TempRoomBedId').value = "";
    document.getElementById('RoomBedId').value = "";
    temp_room_bed_id = 'null';
    temp_reception_value = 'null';
    //document.getElementById('RoomBedName').value="";
    hideSelectRoomBedModal();

}

function hideSelectRoomBedModal() {

    $('#SelectRoomBedModal').modal('hide');
    $('#SelectRoomBedModal-body').empty();
    $(".modal-backdrop:last").remove();

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

    //printJS({ printable: imgData.allb[0], type: 'pdf', base64: true });

}