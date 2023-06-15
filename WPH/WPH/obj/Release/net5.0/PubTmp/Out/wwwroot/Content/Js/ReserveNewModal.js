


var allPatient = [];
var Patients = [];
var formNumber;
var Job;
let addressAuto;
let fatherJobAuto;
let motherJobAuto;
let nameAuto;
let fileNumAuto;
let formNumAuto;
var patient_list = [];


var KurdishKey = [1632, 1633, 1634, 1635, 1636, 1637, 1638, 1639, 1640, 1641, 1602, 96, 1608, 1749, 1610, 1585, 1685, 1578, 1591, 1740, 1742, 1574, 1569
    , 1581, 1593, 1734, 1572, 1662, 1579, 1575, 1570, 1587, 1588, 1583, 1584, 1601, 1573, 1711, 1594, 1607, 8204, 1688, 1571, 1705, 1603, 1604
    , 1717, 1586, 1590, 1582, 1589, 1580, 1670, 1700, 1592, 1576, 1609, 1606, 1577, 1605, 1600
];

var EnglishKey = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "q", "Q", "w", "e", "E", "r", "R", "t", "T", "y", "Y", "u", "U", "i", "I", "o", "O", "p", "P", "a"
    , "A", "s", "S", "d", "D", "f", "F", "g", "G", "h", "H", "j", "J", "k", "K", "l", "L", "z", "Z", "x", "X", "c", "C", "v", "V", "b", "B", "n", "N", "m", "M"];

var Numbers = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "/", ".", "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩", " "];

$(document).ready(function () {



    GetAllPatientList().done(function (data) {
        patient_list = data;
    });

    $("#DoctorId").val($("#Doctor").val());

    let date = $("#DateOfBirth").val();
    if (date !== "") {

        var send_date = date.split('/');
        getAge(`${send_date[1]}/${send_date[0]}/${send_date[2]}`);
    }

    if ($('#Guid').val() === "00000000-0000-0000-0000-000000000000") {
        setTimeout(function () {
            //let date = $("#DateOfBirth").data("kendoDatePicker");
            //date.open();
            //date.close();
            //let s = $("#DateOfBirth").attr('aria-owns');

            //let s3 = $("#" + s + " .k-footer a").click();
            $("#Name2").focus();
        }, 200);
    }

    formNumber = '@Model.Patient.FileNumChoose.Value';
    if (formNumber === "False" || formNumber === false)
        formNumber = false;
    else
        formNumber = true;

    addressAuto = $("#AddressName").data("kendoAutoComplete");
    fatherJobAuto = $("#FatherJobName").data("kendoAutoComplete");
    motherJobAuto = $("#MotherJobName").data("kendoAutoComplete");
    nameAuto = $("#Name2").data("kendoAutoComplete");
    fileNumAuto = $("#FileNum").data("kendoAutoComplete");
    formNumAuto = $("#FormNumber").data("kendoAutoComplete");

    let old = $("#OldVisit").val();
    if (old === "False" || old === false || old === "") {
        $("#OldVisitHelper").prop("checked", false);
    }
    else
        $("#OldVisitHelper").prop("checked", true);

});

$('#FormNumber').on('input', function () {
    var txt = $("#FormNumber").val();

    var filter_list = FilterPatientListsByFormNumber(patient_list, txt);

    $("#FormNumber").data("kendoAutoComplete").dataSource.data(filter_list);

});

$('#Name2').on('input', function () {
    var txt = $("#Name2").val();

    var filter_list = FilterPatientLists(patient_list, txt);

    $("#Name2").data("kendoAutoComplete").dataSource.data(filter_list);

});

$('#PhoneNumber').on('input', function () {
    var txt = $("#PhoneNumber").val();

    var filter_list = FilterPatientListsByMobileAndName(patient_list, txt);

    $("#PhoneNumber").data("kendoAutoComplete").dataSource.data(filter_list);

});


function oldVisitChange() {
    var oldVisit = $("#OldVisitHelper").prop("checked");
    $("#OldVisit").val(oldVisit);
}


$("#AddressName").on('focus', function (e) {

    let value = addressAuto.value();
    addressAuto.search(value);
});


$("#FatherJobName").on('focus', function (e) {

    let value = fatherJobAuto.value();
    fatherJobAuto.search(value);
});


$("#MotherJobName").on('focus', function (e) {

    let value = motherJobAuto.value();
    motherJobAuto.search(value);
});

$("#FileNum").on('focus', function (e) {
    if (fileNumAuto !== undefined) {
        let value = fileNumAuto.value();
        fileNumAuto.search(value);
    }

});

$("#FormNumber").on('focus', function (e) {
    if (formNumAuto !== undefined) {
        let value = formNumAuto.value();
        formNumAuto.search(value);
    }

});

$('#Name2').keypress(function (e) {

    if (e.which === 13 || e.which === 9) {

        let date = $("#DateOfBirth").data("kendoDatePicker");
        date.element.focus();
        //if (formNumber)
        //    $('#FormNumber').focus();
        //else
        //    $('#FileNum').focus();
    }

});

$('#FormNumber').keypress(function (e) {
    let index = KurdishKey.indexOf(e.which);
    if (e.which === 13 || e.which === 9) {
        let date = $("#DateOfBirth").data("kendoDatePicker");
        date.element.focus();
    }
    else if (index !== -1) {
        e.preventDefault();
        let index1 = EnglishKey[index];
        let preVal = $(this).val();
        $(this).val(preVal + index1);
    }

});

$('#FileNum').keypress(function (e) {
    let index = KurdishKey.indexOf(e.which);
    if (e.which === 13 || e.which === 9) {
        let date = $("#DateOfBirth").data("kendoDatePicker");
        date.element.focus();
    }
    else if (index !== -1) {
        e.preventDefault();
        let index1 = EnglishKey[index];
        let preVal = $(this).val();
        $(this).val(preVal + index1);
    }
});


$('#DateOfBirth').keypress(function (e) {

    let char = Numbers.indexOf(e.key)

    if (char !== -1 || e.which === 13 || e.which === 9) {
        let index = KurdishKey.indexOf(e.which);

        if (e.which === 13 || e.which === 9)

            $('#PhoneNumber').focus();

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

$('#Age').keypress(function (e) {
    let index = KurdishKey.indexOf(e.which);

    if (e.which === 13 || e.which === 9)

        $('#PhoneNumber').focus();
    else if (index !== -1) {
        e.preventDefault();
        let index1 = EnglishKey[index];
        let preVal = $(this).val();
        $(this).val(preVal + index1);
    }
});

$('#PhoneNumber').keypress(function (e) {

    let char = Numbers.indexOf(e.key)

    if (char !== -1 || e.which === 13 || e.which === 9) {
        let index = KurdishKey.indexOf(e.which);
        if (e.which === 13 || e.which === 9) {
            let gender = $("#GenderId").data("kendoDropDownList");
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

$('#GenderId').parent().keypress(function (e) {
    if (e.which === 13 || e.which === 9) {
        $('#AddressName').focus();
    }
});



$('#AddressName').keypress(function (e) {
    if (e.which === 13 || e.which === 9) {
        fatherJobAuto.focus();
    }
});

$('#FatherJobName').keypress(function (e) {
    if (e.which === 13 || e.which === 9) {
        motherJobAuto.focus();
    }
});

$('#MotherJobName').keypress(function (e) {
    if (e.which === 13 || e.which === 9) {
        $('#Explanation').focus();
    }
});

$('#Email').keypress(function (e) {
    if (e.which === 13 || e.which === 9)
        $('#Explanation').focus();
});

$('#Explanation').keypress(function (e) {
    if (e.which === 13 || e.which === 9) {
        $('#btn-reserve-accept').focus();

    }
});


function DiseaseRecord() {
    $("#PatientRecordModal").modal('toggle');
    var Patientid = $("#PatientId").val();
    $(".loader").removeClass("hidden");
    let link = "/Patient/PatientRecordForm?Patientid=";

    $('#PatientRecordModal-body').load(link + Patientid, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");

    });

}


function PatientHide() {
    $('#PatientRecordModal').modal('toggle');
    $('#PatientRecordModal-body').empty();
}


function Pasand() {
    $('#btn-reserve-accept').click();
}


function ReserveInsertAge(element) {

    var Year = parseInt($('#Year').val());
    var Month = parseInt($('#Month').val());
    if ($('#Month').val() === "") {
        Month = 0;
    }
    if ($('#Year').val() === "") {
        Year = 0;
    }
    var today = new Date();
    var birthDay = today;
    birthDay.setFullYear(birthDay.getFullYear() - Year);
    birthDay.setMonth(birthDay.getMonth() - Month);
    let birthMonth = birthDay.getMonth() + 1;

    if (parseInt(birthMonth) < 10)
        birthMonth = "0" + birthMonth;

    $('#DateOfBirth').val('01/' + birthMonth + '/' + birthDay.getFullYear());

    let date = $("#DateOfBirth").data("kendoDatePicker");
    date.element.focus();
}


function AddJob() {
    Job = true;
    var link = "/BaseInfo/AddNewModal";
    $(".loader").removeClass("hidden");
    let jobId = $("#Job").attr('data-Value');
    let header = $("#JobText").text();
    $("#new-modalTest-header").text(header);
    $('#my-modalTest-new').modal('toggle');
    $('#new-modalTest-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        $("#TypeId").val(jobId);
    });


}


function AddAddress() {

    Job = false;
    var link = "/BaseInfo/AddNewModal";
    $(".loader").removeClass("hidden");
    let header = $("#AddressText").text();
    let addressId = $("#Address").attr('data-Value');
    $("#new-modalTest-header").text(header);
    $('#my-modalTest-new').modal('toggle');
    $('#new-modalTest-body').load(link, function () {
        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
        $("#TypeId").val(addressId);

    });
}

$('#btn-test-accept').on("click", function () {
    $(this).attr("disabled", true);

    if ($("#Name").val() === "") {
        $("#Name-box").removeClass('hidden');
        return;
    }

    var link = "/BaseInfo/AddOrUpdate";
    var GridRefreshLink = "/BaseInfo/RefreshGrid";
    var data = $("#addNewForm").serialize();
    var JobId;
    if (Job)
        JobId = $("#Job").attr('data-Value');
    else
        JobId = $("#Address").attr('data-Value');


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

                    if (Job) {
                        fatherJobAuto.dataSource.read();
                        motherJobAuto.dataSource.read();
                    }
                    else
                        addressAuto.dataSource.read();

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


$('#btn-test-accept-close').on("click", function () {
    $('#my-modalTest-new').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#new-modalTest-body').empty();
});

function CloseModal() {
    $('#btn-test-accept-close').click();
}
