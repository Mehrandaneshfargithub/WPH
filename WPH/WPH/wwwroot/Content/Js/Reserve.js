


var e = [];
var star = "";
var end = "";

var int = "";

var firstLoad = true;

var PreviousPrice;

var visitPrice;
var ChangevisitPrice;
var first = true;

var hasQueue;



$(document).ready(function () {

    $.ajax({
        type: "Post",
        url: "/Reserves/GetReserveQueueCondition",
        success: function (response) {

            $("#hasQueue").text(response);
            hasQueue = response;
        }
    });

    function onDoctorDataBound() {
        calendar1.refetchEvents();
    }

    //$("#Doctor").kendoDropDownList({
    //    dataTextField: "UserName",
    //    dataValueField: "Guid",
    //    filter: "contains",
    //    dataBound: onDoctorDataBound,
    //    change: onDoctorDataBound,
    //    dataSource: {
    //        //type: "jsonp",
    //        serverFiltering: false,
    //        transport: {
    //            read: {
    //                url: "/Doctor/GetDoctorsBaseUserAccess",
    //            }
    //        }
    //    }
    //});

    $.ajax({
        type: "Get",
        url: "/ClinicSection/GetAllClinicSectionsChild",
        success: function (response) {

            clinic_section = $("#ClinicSectionId").data("kendoDropDownList");
            clinic_section.setDataSource(response);

            clinic_section.bind("change", function (e) {

                ChangeDoctor($("#ClinicSectionId").val());
                AddReserve($("#ClinicSectionId").val());
            });

            var item = response[0];
            if (item && item.Guid) {

                AddReserve(item.Guid);

                clinic_section.select(0);

                ChangeDoctor(item.Guid);
            }

        }
    });

    function AddReserve(sectionId) {
        $(".loader").removeClass("hidden");

        $.ajax({
            type: "Get",
            url: "/Reserves/AddReserve",
            data: { clinicSectionId: sectionId },
            success: function (res) {

                $('#startTime').attr("data-value", res.StartTimeHours);
                $('#endTime').attr("data-value", res.EndTimeHours);
                $('#visitInterval').attr("data-value", res.RoundTime);
                $('#explanition').attr("value", res.Explanation);
                $('#date').attr("value", res.Date);

                $('#startTime').kendoDropDownList();
                star = $('#startTime').data("kendoDropDownList");
                star.value(res.StartTimeHours);

                $('#endTime').kendoDropDownList();
                end = $('#endTime').data("kendoDropDownList");
                end.value(res.EndTimeHours);

                var chooseDate = $("#chooseDate").data("kendoDatePicker");
                chooseDate.value(res.Date);
                chooseDate.trigger("change");

                var explanition = $("#explanition").data("kendoTextArea");
                explanition.value(res.Explanation);
                explanition.trigger("change");

                $('#visitInterval').kendoDropDownList();
                int = $('#visitInterval').data("kendoDropDownList");
                int.value(res.RoundTime);

                calendar1.setOption('slotMinTime', res.StartTimeHours + ":00");
                calendar1.setOption('slotMaxTime', res.EndTimeHours + ":00");

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

                ChangeReserveTemplateBtn();
            }
        });

    }


    function ChangeDoctor(sectionId) {

        $.ajax({
            type: "Get",
            url: "/Doctor/GetDoctorsBaseClinicSectionAccess",
            data: { clinicSectionId: sectionId },
            success: function (res) {
                doctor = $("#Doctor").data("kendoDropDownList");
                doctor.setDataSource(res);

                var item = res[0];
                if (item && item.Guid) {
                    doctor.select(0);
                }

                if (first) {
                    first = false;

                    doctor.bind("dataBound", function (e) {
                        onDoctorDataBound();
                    });

                    doctor.bind("change", function (e) {
                        onDoctorDataBound();
                    });

                    doctor.trigger("change");
                }
            }
        });

    }


    $("#reserveBtn").kendoButton({
        click: ChangeReserveTemplateBtn
    });

    $("#btnGo").kendoButton({
        click: goToDate,
    });

    $("#btnChange").kendoButton();

    calendar();
    $('[data-toggle="tooltip"]').tooltip();

    let pageSize = window.innerWidth;
    if (pageSize < 765) {
        $("#reserveEnd").css("justify-content", "start")
        $("#reserveInterval").css("justify-content", "start")
    }
    else {
        $("#reserveEnd").css("justify-content", "center")
        $("#reserveInterval").css("justify-content", "flex-end")
    }

    visitPrice = $("#VisitPriceInReserve").data("kendoNumericTextBox");
    ChangevisitPrice = $("#ChangeVisitPriceInReserve").data("kendoNumericTextBox");

    visitPrice.readonly();

});


function ChangeReserveTemplateBtn(e) {
    let startTime = "";
    let endTime = "";
    let explanition = "";
    let dur = "";
    let calendarDate = "";

    startTime = star.value();
    endTime = end.value();

    var reseveTimeError = $('#data-reserveTime').text();

    if (parseInt(endTime) < parseInt(startTime)) {
        bootbox.alert({
            message: reseveTimeError,
            className: 'bootbox-class'

        });
        return;
    }
    explanition = $('#explanition').val().toString();
    dur = int.value();
    calendarDate = $('.fc-toolbar-title').text();
    var link = "/Reserves/ReserveDay";
    $(".loader").removeClass("hidden");
    //calendar1.destroy();

    $.ajax({
        type: "Post",
        url: link,
        data: { StartTime: startTime, EndTime: endTime, Explanition: explanition, Dur: dur, CalendarDate: calendarDate, Pasand: true, Direct: 1, ClinicSectionId: $('#ClinicSectionId').val() },
        success: function (response) {
            if (response !== 0) {

                calendar1.setOption('slotMinTime', response.StartTime.Hours.toString() + ":00");
                calendar1.setOption('slotMaxTime', response.EndTime.Hours.toString() + ":00");
                let rint;

                switch (response.RoundTime.toString()) {
                    case "5":
                        rint = "00:05";
                        break;
                    case "10":
                        rint = "00:10";
                        break;
                    case "15":
                        rint = "00:15";
                        break;
                    case "20":
                        rint = "00:20";
                        break;
                    case "30":
                        rint = "00:30";
                        break;
                    case "60":
                        rint = "01:00";
                        break;
                    default:
                        rint = "02:00";
                }
                calendar1.setOption('slotDuration', rint);
                calendar1.setOption('slotLabelInterval', rint);
                star.value(response.StartTime.Hours);
                end.value(response.EndTime.Hours);
                int.value(response.RoundTime);
                $('#explanition').val(response.Explanation)
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });

}


function ChangeReserveTemplate(pasand, direct) {

    var startTime = "";
    var endTime = "";
    var explanition = "";
    var dur = "";
    var calendarDate = "";

    startTime = star.value().toString();
    endTime = end.value().toString();
    explanition = $('#explanition').val().toString();
    dur = int.value();
    calendarDate = $('.fc-toolbar-title').text();
    var link = "/Reserves/ReserveDay";


    $.ajax({
        type: "Post",
        url: link,
        data: { StartTime: startTime, EndTime: endTime, Explanition: explanition, Dur: dur, CalendarDate: calendarDate, Pasand: pasand, Direct: direct, ClinicSectionId: $('#ClinicSectionId').val() },
        success: function (response) {
            if (response !== 0) {

                calendar1.setOption('slotMinTime', response.StartTime.Hours.toString() + ":00");
                star.value(response.StartTime.Hours);

                calendar1.setOption('slotMaxTime', response.EndTime.Hours.toString() + ":00");
                end.value(response.EndTime.Hours);
                let rint;
                switch (response.RoundTime.toString()) {
                    case "5":
                        rint = "00:05";
                        break;
                    case "10":
                        rint = "00:10";
                        break;
                    case "15":
                        rint = "00:15";
                        break;
                    case "20":
                        rint = "00:20";
                        break;
                    case "30":
                        rint = "00:30";
                        break;
                    case "60":
                        rint = "01:00";
                        break;
                    default:
                        rint = "02:00";
                }

                calendar1.setOption('slotDuration', rint);

                calendar1.setOption('slotLabelInterval', rint);

                //end.value(response.EndTime.Hours);
                int.value(response.RoundTime);
                $('#explanition').val(response.Explanation);

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
                if (direct == "prev")
                    calendar1.prev();
                else
                    calendar1.next();
            }
        }
    });

}


$('#btn-qeue-accept').on("click", function () {
    $(this).attr("disabled", true);

    var link = "/Reserves/AddToQeue";

    var variables = $('[data-variable]');
    var variableValues = [];

    let clinicSectionId = $("#ClinicSectionId").val();
    let patientId = $("#PatientId").val();

    for (let i = 0; i < variables.length; i++) {
        let v;

        if ($(variables[i]).attr('data-valueGuid') === "") {
            if ($(variables[i]).hasClass('checkBoxVariable'))
                v = { PatientId: patientId, ReceptionId: $("#Guid").val(), PatientVariableId: $(variables[i]).attr('data-id'), Value: $(variables[i]).prop('checked'), Status: $(variables[i]).attr('data-status') };
            else
                v = { PatientId: patientId, ReceptionId: $("#Guid").val(), PatientVariableId: $(variables[i]).attr('data-id'), Value: $(variables[i]).val(), Status: $(variables[i]).attr('data-status') };
            variableValues.push(v);
        }
    }

    var reserveDetailId = $("#ReserveDetailId").val();

    let visit = {
        Guid: $("#Guid").val(), ClinicSectionId: $("#ClinicSectionId").val(), ReserveDetailId: reserveDetailId, StatusId: $("#StatusId").val()
        , VisitDate: $("#VisitDate").val(), VisitNum: $("#VisitNum").val(), VisitTime: $("#VisitTime").val(), UniqueVisitNum: $("#UniqueVisitNum").val(), Index: $("#Index").val()
        , PatientId: $("#PatientId").val()
    }


    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: { visit: visit, Variables: variableValues },
        success: function (response) {
            $('#btn-qeue-accept').removeAttr("disabled");

            if (response !== 0) {

                $('#qeue-modal-body').empty();
                $('#my-modal-qeue').modal('hide');
                $(".modal-backdrop:last").remove();

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

                CheckLastPatientVisit(reserveDetailId, '', true);
            }
            else {
                bootbox.alert({
                    message: reserveVisited,
                    className: 'bootbox-class'
                });
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });
});


function goToDate() {

    var date = $("#chooseDate").val();
    var startTime = $('#startTime').val().toString();
    var endTime = $('#endTime').val().toString();
    var explanition = $('#explanition').val().toString();
    var dur = $('#visitInterval').val();

    var calendarDate = date;
    var link = "/Reserves/GoToDate";
    $(".loader").removeClass("hidden");


    $.ajax({
        type: "Post",
        url: link,
        data: { StartTime: startTime, EndTime: endTime, Explanition: explanition, Dur: dur, CalendarDate: calendarDate, Pasand: false, ClinicSectionId: $('#ClinicSectionId').val() },
        success: function (response) {
            if (response !== 0) {

                calendar1.setOption('slotMinTime', response.StartTime.Hours.toString() + ":00");
                star.value(response.StartTime.Hours);

                calendar1.setOption('slotMaxTime', response.EndTime.Hours.toString() + ":00");
                end.value(response.EndTime.Hours);
                let rint;
                switch (response.RoundTime.toString()) {
                    case "5":
                        rint = "00:05";
                        break;
                    case "10":
                        rint = "00:10";
                        break;
                    case "15":
                        rint = "00:15";
                        break;
                    case "20":
                        rint = "00:20";
                        break;
                    case "30":
                        rint = "00:30";
                        break;
                    case "60":
                        rint = "01:00";
                        break;
                    default:
                        rint = "02:00";
                }

                $('#explanition').val(response.Explanation)
                calendar1.setOption('slotDuration', rint);
                int.value(response.RoundTime);
                calendar1.setOption('slotLabelInterval', rint);
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
                let d = date.split('/');
                //let d = Date.parse(response.Date);

                calendar1.gotoDate(new Date(parseInt(d[2]), parseInt(d[1]) - 1, parseInt(d[0])));

            }
        }
    });
}

var date = new Date();
var d1 = date.getDate();
var m = date.getMonth();
var calendarStartTime = "";
var calendarEndTime = "";
var calendarDur = "";
var calendarDate = "";
var calendar1;
var reserveVisited = $('#reserveVisited').text();
var reserveVisiting = $('#reserveVisiting').text();
var reserveToQueue = $('#reserveToQueue').text();
var reserveToVisit = $('#reserveToVisit').text();
var reserveToDoctor = $('#reserveToDoctor').text();
var reserveVisitingDelete = $('#PatientVisitingCannotDeleted').text();
var reserveVisitedDelete = $('#reserveVisitedCannotDelete').text();
var removeReserveFromQueue = $('#removeReserveFromQueue').text();

function calendar() {
    calendarStartTime = "07:00";
    calendarEndTime = "23:00";

    let durVal = $('#visitInterval').val();

    switch (durVal) {
        case "5":
            calendarDur = "00:05";
            break;
        case "10":
            calendarDur = "00:10";
            break;
        case "15":
            calendarDur = "00:15";
            break;
        case "20":
            calendarDur = "00:20";
            break;
        case "30":
            calendarDur = "00:30";
            break;
        case "60":
            calendarDur = "01:00";
            break;
        default:
            calendarDur = "02:00";
    }

    var calendarEl = document.getElementById('calendar');

    calendar1 = new FullCalendar.Calendar(calendarEl, {


        headerToolbar: {
            left: 'timeGridDay,timeGridWeek,dayGridMonth',
            center: 'title',
            right: 'myNext,myPrev myToday'

        },

        buttonText: {
            today: $("#date").attr("data-today"),
            month: $("#date").attr("data-month"),
            week: $("#date").attr("data-week"),
            day: $("#date").attr("data-day"),
        },

        customButtons: {

            myDay: {
                text: $("#date").attr("data-today"),
                click: function () {

                    let date = new Date();
                    $("#chooseDate").val(date.getDate().toString().padStart(2, 0) + '/' + (date.getMonth() + 1).toString().padStart(2, 0) + '/' + date.getFullYear().toString());
                    goToDate();
                }
            },

            myToday: {
                text: $("#date").attr("data-today"),
                click: function () {

                    let date = new Date();
                    $("#chooseDate").val(date.getDate().toString().padStart(2, 0) + '/' + (date.getMonth() + 1).toString().padStart(2, 0) + '/' + date.getFullYear().toString());
                    goToDate();
                }
            },
            myPrev: {
                text: '>',
                click: function () {
                    if ($(".fc-timeGridWeek-button").hasClass("fc-button-active") || $(".fc-dayGridMonth-button").hasClass("fc-button-active")) {

                        calendar1.prev();
                        return;
                    }

                    ChangeReserveTemplate(false, "prev");
                }
            },
            myNext: {
                text: '<',
                click: function () {

                    if ($(".fc-timeGridWeek-button").hasClass("fc-button-active") || $(".fc-dayGridMonth-button").hasClass("fc-button-active")) {
                        calendar1.next();
                        return;
                    }
                    ChangeReserveTemplate(false, "next");
                }
            }
        },

        initialView: 'timeGridDay',
        direction: 'rtl',
        allDaySlot: false,
        titleFormat: function (date) {

            return date.date.day + '/' + (date.date.month + 1) + '/' + date.date.year;
        },
        slotMinTime: calendarStartTime,
        slotMaxTime: calendarEndTime,
        eventContent: function (arg) {

            $(".loader").removeClass("hidden");

            var eventMounth = arg.event.start.getMonth();
            var eventDay = arg.event.start.getDate();

            let Div = document.createElement('div');
            Div.classList.add('calendar-div');
            let stat = arg.event.backgroundColor;
            //////////////////////////////////////////////////////////////////////edit
            let reserveEdit = $('#reserveEdit').text();
            let btnEdit = document.createElement('a');
            btnEdit.classList.add('calendar-button');
            btnEdit.classList.add('btn-info');
            btnEdit.setAttribute('data-toggle', 'tooltip');
            btnEdit.setAttribute('title', reserveEdit);
            btnEdit.style.border = '2px solid #2d90d2';
            btnEdit.onclick = function () {

                var clinic_id = $("#ClinicSectionId").val();
                var doctor_id = $("#Doctor").val();

                if (clinic_id == '' || doctor_id == '') {

                    bootbox.alert({
                        message: 'SelectSectionOrDoctor',
                        className: 'bootbox-class'
                    });

                    return;
                }

                $('#my-modal-reserve').modal('toggle');

                $('#reserve-modal-header').text($("#date").attr("data-editResreve"));

                $("#my-modal-reserve #ERROR_SomeThingWentWrong").addClass("hidden");
                $("#my-modal-reserve #VisitedChange").addClass("hidden");

                var link = $("#GridEditLink").attr("data-Value");
                var Id = arg.event.id;
                $(".loader").removeClass("hidden");


                $('#reserve-modal-body').load(link + `${Id}&clinicSectionId=${clinic_id}&doctorId=${doctor_id}`, function () {

                    $(".loader").fadeIn("slow");

                    $(".loader").addClass("hidden");

                });
            };

            let btnEditIcon = document.createElement('i');
            btnEditIcon.classList.add('fa');
            btnEditIcon.classList.add('fa-pencil');
            btnEdit.appendChild(btnEditIcon);

            ///////////////////////////////////////////////////////////////////////////Image


            let btnImage = document.createElement('a');
            btnImage.classList.add('calendar-button');
            btnImage.classList.add('btn-info');

            btnImage.onclick = function () {

                $(".loader").removeClass("hidden");
                var Id = arg.event.id;
                $.ajax({
                    type: "Post",
                    data: { id: Id },
                    url: "/Reserves/GetPatientByReserveId",
                    success: function (response) {

                        $("#my-modal-PatientVariables").modal('toggle');
                        var patientid = response.Guid;
                        $("#PatientVariables-modal-header").text(response.Name);

                        $("#btn-PatientVariables-accept").attr('data-patientId', patientid);

                        var link = "/Patient/PatientVariableModal?patientId=";

                        $('#PatientVariables-modal-body').load(link + patientid, function () {
                            $(".loader").fadeIn("slow");

                            $(".loader").addClass("hidden");

                        });

                    }
                });

            };

            let btnImageIcon = document.createElement('i');
            btnImageIcon.classList.add('fa');
            btnImageIcon.classList.add('fa-image');
            btnImage.appendChild(btnImageIcon);

            //////////////////////////////////////////////////////////////////////qeue


            let reserveQueue = $('#reserveQueue').text();
            let btnQeue = document.createElement('a');
            btnQeue.classList.add('calendar-button');
            btnQeue.classList.add('btn-success');
            btnQeue.setAttribute('data-toggle', 'tooltip');
            btnQeue.setAttribute('title', reserveQueue);
            btnQeue.style.border = '2px solid #64a45b';
            btnQeue.onclick = function () {

                if (stat == "#76D7C4") {
                    bootbox.alert({
                        message: reserveVisited,
                        className: 'bootbox-class'
                    });
                    return;
                }

                let e = calendar1.getEventById(arg.event.id);


                $('#my-modal-qeue').modal('toggle');

                var link = "/Reserves/QeueModal?Id=";
                var Id = arg.event.id;
                var section = $('#ClinicSectionId').val();
                $(".loader").removeClass("hidden");

                $('#qeue-modal-body').load(link + `${Id}&clinicSectionId=${section}`, function (responce) {

                    let iii = responce;
                    if (responce === "0") {
                        $('#my-modal-qeue').modal('toggle');
                        bootbox.alert({
                            message: reserveVisiting,
                            className: 'bootbox-class'
                        });
                    }


                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                });
            };

            let btnQeueIcon = document.createElement('i');
            btnQeueIcon.classList.add('fa');
            btnQeueIcon.classList.add('fa-random');
            btnQeue.appendChild(btnQeueIcon);

            ///////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////delete

            let btnDelete = document.createElement('a');
            let reserveDelete = $('#reserveDelete').text();
            btnDelete.classList.add('calendar-button');

            btnDelete.classList.add('btn-danger');
            btnDelete.setAttribute('data-toggle', 'tooltip');
            btnDelete.setAttribute('title', reserveDelete);
            btnDelete.style.border = '2px solid #8f3424';
            btnDelete.onclick = function () {

                $(".loader").removeClass("hidden");
                $('#my-modal-delete').modal('toggle');
                $('#btn-delete-accept').attr('data-id', arg.event.id);

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            };

            let btnDeleteIcon = document.createElement('i');
            btnDeleteIcon.classList.add('fa');
            btnDeleteIcon.classList.add('fa-trash-can');
            btnDelete.appendChild(btnDeleteIcon);

            ///////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////prescription

            let btnPrescription = document.createElement('a');
            let reservePrescription = $('#reservePrescription').text();
            btnPrescription.classList.add('calendar-button');

            btnPrescription.classList.add('btn-prescription');
            btnPrescription.setAttribute('data-toggle', 'tooltip');
            btnPrescription.setAttribute('title', reservePrescription);
            btnPrescription.style.border = '2px solid #8f3424';
            btnPrescription.onclick = function () {



                //$('#btn-acceptPatientQueuDisease').attr('data-id', arg.event.id);

                //let e = calendar1.getEventById(arg.event.id);


                $('#prescriptionModal').modal('toggle');

                var link = "/Visit/GetPrescription?Id=";
                var Id = arg.event.id;
                $(".loader").removeClass("hidden");

                $('#prescriptionModal-body').load(link + Id + '', function (responce) {
                    if (responce === "DoNotHaveVisit") {
                        $('#prescriptionModal').modal('hide');
                        $('#prescriptionModal-body').empty();
                        bootbox.alert({
                            message: reserveToQueue,
                            className: 'bootbox-class MyFont-Sarchia-grid'
                        });
                    }
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                });

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            };

            let btnPrescriptionIcon = document.createElement('i');
            btnPrescriptionIcon.classList.add('fa');
            btnPrescriptionIcon.classList.add('fa-clipboard-prescription');
            btnPrescription.appendChild(btnPrescriptionIcon);

            ///////////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////send

            let btnSend = document.createElement('a');
            let reserveSend = $('#reserveSend').text();

            btnSend.classList.add('calendar-button');
            btnSend.classList.add('btn-warning');
            btnSend.setAttribute('data-toggle', 'tooltip');
            btnSend.setAttribute('title', reserveSend);
            btnSend.style.border = '2px solid #e68600';
            btnSend.onclick = function () {


                if (stat == "#f5df4d") {
                    bootbox.alert({
                        message: reserveVisiting,
                        className: 'bootbox-class MyFont-Sarchia-grid'
                    });
                    return
                }

                if (hasQueue) {
                    if (stat == "#EAF2F8") {
                        bootbox.alert({
                            message: reserveToQueue,
                            className: 'bootbox-class'
                        });
                        return;
                    }
                }


                let correntSatus = "";

                if (stat == "#76D7C4") {
                    correntSatus = "Visited";
                }

                if (stat == "#EAF2F8") {
                    correntSatus = "NotVisited";
                }

                let e = calendar1.getEventById(arg.event.id);

                bootbox.confirm({
                    message: reserveToDoctor,
                    className: 'bootbox-class',
                    callback: function (result) {

                        if (!result) {

                            return;
                        }
                        else {
                            $(".loader").removeClass("hidden");
                            $.ajax({
                                url: '/Reserves/EventChangeStatus',
                                type: "Post",
                                data: {
                                    id: arg.event.id, status: "Visiting", correntReserveStatus: correntSatus, clinicSectionId: $('#ClinicSectionId').val()
                                },
                                success: function (response) {

                                    if (response !== 0) {

                                        CheckLastPatientVisit(arg.event.id, '#f5df4d', false);

                                    }
                                    else {
                                        bootbox.alert({
                                            message: reserveVisited,
                                            className: 'bootbox-class'
                                        });
                                    }

                                    $(".loader").fadeIn("slow");
                                    $(".loader").addClass("hidden");
                                }

                            });
                        }
                    },
                })

            };

            let btnSendIcon = document.createElement('i');
            btnSendIcon.classList.add('fa');
            btnSendIcon.classList.add('fa-paper-plane');
            btnSend.appendChild(btnSendIcon);

            ////////////////////////////////////////////////////////////////////


            ////////////////////////////////////////////////////////////ok

            let btnOk = document.createElement('a');
            let reserveOk = $('#reserveOk').text();
            btnOk.classList.add('calendar-button');
            btnOk.classList.add('btn-success');
            btnOk.setAttribute('data-toggle', 'tooltip');
            btnOk.setAttribute('title', reserveOk);
            btnOk.style.border = '2px solid #64a45b ';
            btnOk.onclick = function () {

                if (stat == "#76D7C4") {
                    bootbox.alert({
                        message: reserveVisited,
                        className: 'bootbox-class'
                    });
                    return;
                }
                if (stat == "#5DADE2") {
                    bootbox.alert({
                        message: reserveToVisit,
                        className: 'bootbox-class'
                    });

                    return;
                }

                if (stat == "#EAF2F8") {
                    bootbox.alert({
                        message: reserveToVisit,
                        className: 'bootbox-class'
                    });
                    return;
                }

                let e = calendar1.getEventById(arg.event.id);

                $(".loader").removeClass("hidden");

                $.ajax({
                    url: '/Reserves/EventChangeStatus',
                    type: "Post",
                    data: { id: arg.event.id, status: "Visited", correntReserveStatus: "" },
                    success: function (response) {
                        if (response !== 0) {

                            CheckLastPatientVisit(arg.event.id, '#76D7C4', false);

                            $(".loader").fadeIn("slow");
                            $(".loader").addClass("hidden");
                        }
                    }

                });
            };

            let btnOkIcon = document.createElement('i');
            btnOkIcon.classList.add('fa');
            btnOkIcon.classList.add('fa-check');
            btnOk.appendChild(btnOkIcon);

            /////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////title

            let Title = document.createElement('span');
            Title.style.marginRight = '1.2rem';
            //Title.style.overflow = 'auto';

            //Title.style.padding = '0.3rem';
            //Title.style.width = '35rem';
            //Title.style.fontSize = '1.5rem';
            //Title.style.fontWeight = 'bold';
            //Title.style.color = 'black';
            Title.classList.add('Calendar-title');
            //Title.style.backgroundColor = '#EAF2F8';
            Title.onclick = function () {

                if (stat == "#5DADE2") {
                    bootbox.confirm("Do You Want To Remove Patient From Queue?", function (result) {

                        if (!result) {

                            return;

                        }
                        else {
                            $(".loader").removeClass("hidden");
                            $.ajax({
                                url: '/Reserves/EventChangeStatus',
                                type: "Post",
                                data: { id: arg.event.id, status: "NotVisited", correntReserveStatus: "InQueue" },
                                success: function (response) {
                                    if (response !== 0) {

                                        $.ajax({
                                            url: '/Reserves/GetReceptionRemByReceptionId',
                                            type: "Get",
                                            data: { reserveDetailId: arg.event.id },
                                            success: function (ressss) {

                                                e.setProp('backgroundColor', '#EAF2F8');
                                                e.setExtendedProp('text', ressss);
                                            }
                                        });

                                        $(".loader").fadeIn("slow");
                                        $(".loader").addClass("hidden");
                                    }
                                }
                            });
                        }
                    })

                }

                if (stat == "#76D7C4") {
                    bootbox.alert({
                        message: reserveVisited,
                        className: 'bootbox-class'
                    });
                    return;
                }
                if (stat == "#f5df4d") {
                    bootbox.alert({
                        message: reserveVisiting,
                        className: 'bootbox-class'
                    });
                    return;
                }
                if (stat == "#EAF2F8") {

                    return;
                }

                let e = calendar1.getEventById(arg.event.id);

            };

            /////////////////////////////////////////////////////////////////


            Title.innerHTML = arg.event.title;


            ///////////////////////////////////////////////////////////title

            let Status = document.createElement('span');

            Status.classList.add('Calendar-status');
            //Status.style.marginRight = '1.2rem';
            ////Status.style.overflow = 'auto';

            //Status.style.padding = '0.3rem';
            //Status.style.fontSize = '1.5rem';
            //Status.style.fontWeight = 'bold';

            //if (stat == "#EAF2F8") {
            //    Status.style.color = '#6E2C00';
            //}

            switch (stat) {
                case "#f5df4d":
                    Status.innerHTML = "Visiting";
                    //Status.style.color = '#6E2C00';
                    break;
                case "#76D7C4":
                    Status.innerHTML = "Visited";
                    //Status.style.color = '#6E2C00';
                    break;
                case "#5DADE2":
                    Status.innerHTML = "In Queue";
                    break;
                default:
                    Status.innerHTML = "Not Visited";
            }

            if (arg.event.extendedProps.oldVisit)
                Status.innerHTML = Status.innerHTML + " - " + arg.event.extendedProps.testResult;
            if (arg.event.extendedProps.lastVisit)
                Status.innerHTML = Status.innerHTML + " - " + arg.event.extendedProps.lastVisitUnderSix;
            ///////////////////////////////////////////////////////////////////////Remain


            let Remain = document.createElement('span');
            Remain.classList.add('Calendar-status');
            //Remain.style.marginRight = '1.2rem';
            //Remain.style.fontSize = '1.5rem';
            //Remain.style.fontWeight = 'bold';
            //if (stat == "#EAF2F8") {
            //    Remain.style.color = '#6E2C00';
            //}
            //else if (stat == "#f5df4d") {
            //    Remain.style.color = '#6E2C00';
            //}
            //else if (stat == "#76D7C4") {
            //    Remain.style.color = '#6E2C00';
            //}

            Remain.innerHTML = arg.event.extendedProps.text;


            ///////////////////////////////////////////////////////////////

            let btnReceive = document.createElement('a');

            let reserveServices = $('#reserveServices').text();
            btnReceive.classList.add('calendar-button');
            btnReceive.classList.add('btn-warning');
            btnReceive.setAttribute('title', reserveServices);
            btnReceive.style.border = '2px solid #e68600';
            btnReceive.onclick = function () {

                if (stat == "#EAF2F8") {
                    bootbox.alert({
                        message: reserveToQueue,
                        className: 'bootbox-class'
                    });
                    return;
                }


                let e = calendar1.getEventById(arg.event.id);

                $(".loader").removeClass("hidden");

                $('#my-modal-Receive').modal('toggle');

                var link = "/Visit/PayVisitModal?reserveDetailId=";
                var Id = arg.event.id;
                $(".loader").removeClass("hidden");

                $('#Receive-modal-body').load(link + Id, function (responce) {

                    $("#my-modal-Receive #ERROR_SomeThingWentWrong").addClass("hidden");
                    $("#my-modal-Receive #EmptyDoctor").addClass("hidden");
                    $("#my-modal-Receive #ERROR_Data").addClass("hidden");
                    $("#TxtReserveDetailId").val(Id);

                    if (responce === "NoVisit") {
                        $('#my-modal-Receive').modal('toggle');
                        bootbox.alert({
                            message: reserveToQueue,
                            className: 'bootbox-class'
                        });
                    }


                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                });
            };

            let btnReceiveIcon = document.createElement('i');
            btnReceiveIcon.classList.add('fa');
            btnReceiveIcon.classList.add('fa-dollar');
            btnReceive.appendChild(btnReceiveIcon);


            /////////////////////////////////////////////////////////////////

            let previuseDay = $("#CanAddReceptionToPreviousDays").text();

            if ($(".fc-dayGridMonth-button").hasClass("fc-button-active") || $(".fc-timeGridWeek-button").hasClass("fc-button-active")) {
                Title.style.fontSize = '1rem';
                Title.style.width = '12rem';
                Status.style.fontSize = '1rem';

                Div.appendChild(Title);
                //Div.appendChild(Status);
                let arrayOfDomNodes = [Div];
                $(".loader").addClass("hidden");
                return { domNodes: arrayOfDomNodes }
            }
            else if (((eventMounth !== m) || (eventMounth == m && eventDay !== d1)) && previuseDay.toLowerCase() == "false") {
                Div.appendChild(Title);
                Div.appendChild(Status);
                Div.appendChild(btnDelete);
                let arrayOfDomNodes = [Div];
                $(".loader").addClass("hidden");
                return { domNodes: arrayOfDomNodes }
            }
            else {
                Div.appendChild(Title);
                Div.appendChild(btnEdit);
                //Div.appendChild(btnImage);

                let hasQueue = $("#hasQueue").text();

                //if (hasQueue === "true") {
                Div.appendChild(btnQeue);
                //}


                Div.appendChild(btnSend);

                let accessPrescription = $("#date").attr('data-accessPrescription');
                if (accessPrescription.toLowerCase() === "true")
                Div.appendChild(btnPrescription);
                Div.appendChild(btnOk);
                Div.appendChild(btnReceive);
                Div.appendChild(btnDelete);
                Div.appendChild(Status);
                Div.appendChild(Remain);


                let arrayOfDomNodes = [Div];
                $(".loader").addClass("hidden");
                return { domNodes: arrayOfDomNodes }
            }

        },

        events: function (info, callback) {

            let doctorId = $("#Doctor").val();
            let section_id = $("#ClinicSectionId").val();
            let calendarstat = "";

            setTimeout(() => {
                if ($(".fc-timeGridWeek-button").hasClass("fc-button-active"))
                    calendarstat = "w";
                else if ($(".fc-dayGridMonth-button").hasClass("fc-button-active"))
                    calendarstat = "m";
                else
                    calendarstat = "d";

                if (doctorId != "") {

                    let calDay = info.start.getDate();
                    let calMonth = info.start.getMonth() + 1;
                    let calYear = info.start.getFullYear();
                    let calDate = calDay + "/" + calMonth + "/" + calYear;
                    $(".loader").removeClass("hidden");
                    $.ajax({
                        url: `/Reserves/GetEvents?date=${info.startStr}&doctorId=${doctorId}&clinicSectionId=${section_id}&calendarStatus=${calendarstat}`,
                        type: "GET",
                        dataType: "JSON",
                        error: function (request, error) {
                            console.log(error);
                            alert(" please Refresh The Page ");
                        },
                        success: function (eventList) {

                            var events = [];
                            var status;
                            var statusId;
                            let borderC;
                            $.each(eventList, function (i, data) {

                                statusId = data.StatusName;

                                switch (statusId) {
                                    case "Visiting":
                                        status = "#f5df4d";
                                        borderC = "#f2d726";
                                        break;
                                    case "Visited":
                                        status = "#76D7C4";
                                        borderC = "#34b299";
                                        break;
                                    case "InQueue":
                                        status = "#5DADE2";
                                        borderC = "#2283c3";
                                        break;
                                    default:
                                        status = "#EAF2F8";
                                        borderC = "#c6dbeb";

                                }
                                let title;

                                if (data.PhoneNumber === null)
                                    data.PhoneNumber = "";

                                if (data.Explanation === null)
                                    data.Explanation = "";

                                if (data.UseFormNum) {
                                    if (data.FormNumber === null)
                                        data.FormNumber = "";
                                    title = data.Name + " - " + data.PhoneNumber + " - " + data.Explanation/*+ " - " + data.FormNumber*/;


                                }
                                else {
                                    title = data.Name + " - " + data.PhoneNumber + " - " + data.Explanation/*+ " - " + data.FileNum*/;

                                }


                                events.push(
                                    {
                                        id: data.Guid,
                                        title: title,

                                        start: data.ReserveStartTime,
                                        end: data.ReserveEndTime,

                                        className: 'eventClass',

                                        text: data.Remain,

                                        oldVisit: data.OldVisit,
                                        lastVisit: data.LastVisit,
                                        testResult: 'Test Result',
                                        lastVisitUnderSix: 'Last Visit Is Under 6 Days',
                                        backgroundColor: status,
                                        borderColor: borderC


                                    });
                            });

                            callback(events);

                            $(".loader").addClass("hidden");
                        }
                    });
                }
            })

            

        },

        slotLabelFormat: {
            hour12: false,
            hour: 'numeric',
            minute: '2-digit'
        },

        slotLabelClassNames: 'timeSlot',

        slotLaneClassNames: 'slotLane',

        slotLabelInterval: calendarDur,

        slotDuration: calendarDur,

        editable: true,

        droppable: true, // this allows things to be dropped onto the calendar !!!

        eventDrop: function (info) {
            var eventMounth = info.event.start.getMonth();
            var eventDay = info.event.start.getDate();
            let stat = info.event.backgroundColor;
            let reserveTransform = $('#reserveTransform').text();
            let todayReserve = false;
            if ((eventMounth < m) || (eventMounth == m && eventDay < d1)) {
                if (stat != "#EAF2F8") {
                    bootbox.alert({
                        message: reserveTransform,
                        className: 'bootbox-class'
                    });
                    info.revert();
                    return;
                }
            }
            if ((eventMounth == m && eventDay == d1) || (eventMounth == m && eventDay > d1) || (eventMounth > m)) {
                todayReserve = true;
            }
            var id = info.event.id;
            var start = info.event.startStr;
            var end = info.event.endStr;
            $(".loader").removeClass("hidden");
            $.ajax({
                url: '/Reserves/EventChangePosition',
                type: "Post",
                data: { id: id, start: start, end: end, today: todayReserve },
                success: function (response) {
                    if (response === 0) {
                        bootbox.alert({
                            message: reserveTransform,
                            className: 'bootbox-class'
                        });
                        info.revert();
                    }

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                }

            });
        },

        selectable: true,

        selectHelper: true,

        eventResize: function (info) {
            var id = info.event.id;
            var start = info.event.startStr;
            var end = info.event.endStr;
            $(".loader").removeClass("hidden");
            $.ajax({
                url: '/Reserves/EventResized',
                type: "Post",
                data: { id: id, start: start, end: end },
                success: function (response) {
                    if (response !== 0) {
                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");
                    }
                }

            });

        },


        select: function (info) {

            if ($(".fc-dayGridMonth-button").hasClass("fc-button-active")) {

                var d = info.startStr.split('-');

                var startTime = "";
                var endTime = "";
                var explanition = "";
                var dur = "";
                var calendarDate = "";

                startTime = $('#startTime').val().toString();
                endTime = $('#endTime').val().toString();
                explanition = $('#explanition').val().toString();
                dur = $('#visitInterval').val();

                $("#chooseDate").val(parseInt(d[2]) + '/' + parseInt(d[1]) + '/' + parseInt(d[0]));
                goToDate();
                $('.fc-timeGridDay-button').click();

            } else {

                var link = $("#GridAddLink").attr("data-Value");

                let eventMounth = info.start.getMonth();
                let eventDay = info.start.getDate();
                let previuseDay = $("#CanAddReceptionToPreviousDays").text();

                if (eventMounth > m || (eventMounth === m && eventDay >= d1) || previuseDay.toLowerCase() == "true") {
                    var clinic_id = $("#ClinicSectionId").val();
                    var doctor_id = $("#Doctor").val();

                    if (clinic_id == '' || doctor_id == '') {

                        bootbox.alert({
                            message: 'SelectSectionOrDoctor',
                            className: 'bootbox-class'
                        });

                        return;
                    }

                    $(".loader").removeClass("hidden");
                    $('#my-modal-reserve').modal('toggle');


                    $("#my-modal-reserve #ERROR_SomeThingWentWrong").addClass("hidden");
                    $("#my-modal-reserve #VisitedChange").addClass("hidden");

                    $('#reserve-modal-header').text($('#date').attr("data-newResreve"));
                    $('#reserve-modal-body').load(link + `?start=${info.startStr}&end=${info.endStr}&clinicSectionId=${clinic_id}&doctorId=${doctor_id}`, function (response) {
                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");

                    });
                }
                else {
                    let cant = $('#cannotInsertReserve').text();
                    bootbox.alert({
                        message: cant,
                        className: 'bootbox-class'
                    });
                }

            }

        },

    });


    calendar1.render();
    
}



function VisitPriceChange() {
    $('#my-modal-VisitPrice').modal('toggle');
}


function VisitPriceInReserveChange() {

    let visitInsertedPrice = ChangevisitPrice.value();


    $(".loader").removeClass("hidden");
    $.ajax({
        url: '/ClinicSection/ChangeSettingValue',
        type: "Post",
        data: { value: visitInsertedPrice, Name: "VisitPrice" },
        success: function (response) {
            if (response !== 0) {
                calendar1.refetchEvents();
                visitPrice.value(visitInsertedPrice);
            }

            $('#my-modal-VisitPrice').modal('toggle');
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }

    });
}

$('#btn-reserve-accept').on("click", function () {
    $(this).attr("disabled", true);

    $("#my-modal-reserve #ERROR_SomeThingWentWrong").addClass("hidden");
    $("#my-modal-reserve #VisitedChange").addClass("hidden");

    $('.emptybox').addClass('hidden');
    var isEmmpty = true;
    $('.emptybox').each(function () {
        if ($(this).attr('data-isEssential') === 'true') {
            var empty = $(this).attr('id');
            if ($('[data-checkEmpty="' + empty + '"]').val() === "") {
                $(this).removeClass('hidden');
                $('#btn-reserve-accept').removeAttr("disabled");
                isEmmpty = false;
                return;
            }
        }
    });

    if (isEmmpty === false) {
        return;
    }

    var link = $("#AddNewLink").attr("data-Value");
    var GridRefreshLink = $("#GridRefreshLink").attr("data-Value");
    var data = $("#ReserveForm").serialize();


    if (!IfUserCheckPass(GridRefreshLink)) {
        $('#btn-reserve-accept').removeAttr("disabled");
        return;
    }
    if (!CheckIfPriceBiggerThanDiscount(GridRefreshLink)) {
        $('#btn-reserve-accept').removeAttr("disabled");
        return;
    }

    $(".loader").removeClass("hidden");

    $.ajax({
        type: "Post",
        url: link,
        data: data,
        success: function (response) {
            $('#btn-reserve-accept').removeAttr("disabled");

            if (response !== 0) {
                if (response === "ValueIsRepeated") {

                    $('#Name-Exist').removeClass('hidden');
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else if (response === "VisitedChange") {

                    $("#my-modal-reserve #VisitedChange").removeClass("hidden");

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else {
                    IfReserve(response, GridRefreshLink);

                    $('#my-modal-reserve').modal('hide');
                    $(".modal-backdrop:last").remove();
                    $('#reserve-modal-body').empty();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                }
            } else {

                $("#my-modal-reserve #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });
});


function closeReserveModal() {

    $('#my-modal-reserve').modal('hide');
    $('#reserve-modal-body').empty();
    $(".modal-backdrop:last").remove();
}

function closeQeueModal() {

    $('#my-modal-qeue').modal('hide');
    $('#qeue-modal-body').empty();
    $(".modal-backdrop:last").remove();
}


function closeReceiveModal() {

    RefreshReservePrice($("#TxtReserveDetailId").val());

    $('#my-modal-Receive').modal('hide');
    $('#Receive-modal-body').empty();
    $(".modal-backdrop:last").remove();

}


function closeVisitPriceModal() {

    $('#my-modal-VisitPrice').modal('hide');
    // $('#VisitPrice-modal-body').empty();
    //$(".modal-backdrop:last").remove();

}


function HideCameraModal() {
    $("#CameraModal").modal('toggle');
    var stream = video.srcObject;
    var tracks = stream.getTracks();

    for (var i = 0; i < tracks.length; i++) {
        var track = tracks[i];
        track.stop();
    }
    video.srcObject = null;
}

function RefreshReservePrice(element) {

    let e = calendar1.getEventById(element);
    $.ajax({
        url: '/Reserves/GetReceptionRemByReceptionId',
        type: "Get",
        data: { reserveDetailId: element },
        success: function (ressss) {

            e.setExtendedProp('text', ressss);
        }
    });
}

function CheckLastPatientVisit(element, color, refresh) {

    let e = calendar1.getEventById(element);

    $.ajax({
        url: '/Reserves/CheckLastPatientVisit',
        type: "Get",
        data: { reserveDetailId: element },
        success: function (ressss) {

            if (ressss === "CheckPay") {

                bootbox.confirm({
                    message: $('#doYouWantGetMoney').text(),
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (result) {
                        if (result) {

                            $.ajax({
                                url: '/Reserves/GetReceptionRemByReceptionId',
                                type: "Get",
                                data: { reserveDetailId: element },
                                success: function (output) {
                                    if (color !== '')
                                        e.setProp('backgroundColor', color);

                                    if (refresh) {
                                        calendar1.refetchEvents();
                                    } else {
                                        e.setExtendedProp('text', output);
                                    }
                                }
                            });

                        }
                        else {

                            $.ajax({
                                url: '/Reserves/RemoveReserveVisitPrice',
                                type: "Get",
                                data: { reserveDetailId: element },
                                success: function (res) {
                                    if (res !== 0) {
                                        if (color !== '')
                                            e.setProp('backgroundColor', color);

                                        if (refresh) {
                                            calendar1.refetchEvents();
                                        } else {
                                            e.setExtendedProp('text', res);
                                        }
                                    }
                                }
                            });
                        }
                    }
                })

            } else {

                $.ajax({
                    url: '/Reserves/GetReceptionRemByReceptionId',
                    type: "Get",
                    data: { reserveDetailId: element },
                    success: function (output) {
                        if (color !== '')
                            e.setProp('backgroundColor', color);

                        if (refresh) {
                            calendar1.refetchEvents();
                        } else {
                            e.setExtendedProp('text', output);
                        }
                    }
                });
            }
        }
    });
}

$('#chooseDate').on('keydown', function (e) {

    if (e.which === 13) {

        goToDate();
    }
    

});