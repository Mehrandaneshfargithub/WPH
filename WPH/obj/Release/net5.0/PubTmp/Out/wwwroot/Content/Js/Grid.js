$('#btn-new-accept').on("click", function () {
    $(this).attr("disabled", true);

    $('#my-modal-new #ERROR_SomeThingWentWrong').addClass('hidden');
    $('.emptybox').addClass('hidden');

    var mobile = $("#Mobile-wrong");
    if (mobile) {
        mobile.addClass("hidden");

        if ($('[validate-mobile="Mobile-wrong"]').val() !== undefined) {
            let text = $('[validate-mobile="Mobile-wrong"]').val();
            if (text != "" & text.trim().length < 8) {
                mobile.removeClass("hidden");
                $('#btn-new-accept').removeAttr("disabled");
                return;
            }
        }
    }

    var email = $("#Email-wrong");
    if (email) {
        email.addClass("hidden");

        if ($('[validate-email="Email-wrong"]').val() !== undefined) {
            let text = $('[validate-email="Email-wrong"]').val();
            if (!ValidateEmail(text)) {
                email.removeClass("hidden");
                $('#btn-new-accept').removeAttr("disabled");
                return;
            }
        }
    }

    var isEmmpty = true;
    $('.emptybox').each(function () {
        if ($(this).attr('data-isEssential') === 'true') {
            var empty = $(this).attr('id');

            if ($('[data-checkEmpty="' + empty + '"]').val() !== undefined) {
                let text = $('[data-checkEmpty="' + empty + '"]').val()/*.replace(/ /g, '')*/;
                if (text === "") {
                    $(this).removeClass('hidden');
                    $('#btn-new-accept').removeAttr("disabled");
                    isEmmpty = false;
                    return;
                }
            }

        }
    });


    if (isEmmpty === false) {
        return;
    }

    if (GridRefreshLink === "/BaseInfo/RefreshGrid") {
        var costDate = $("#KendoCostDate").data("kendoDatePicker");

        var Date = costDate.value();

        let cMonth = Date.getMonth() + 1;
        let cDay = Date.getDate();

        if (cMonth < 10)
            cMonth = "0" + cMonth;

        if (cDay < 10)
            cDay = "0" + cDay;

        let from = costDate.getFullYear() + "-" + cMonth + "-" + cDay;

        $("#CostDate").val(from);
    }

    var link = $("#AddNewLink").attr("data-Value");
    var GridRefreshLink = $("#GridRefreshLink").attr("data-Value");

    let patientForm = $("#PatientNamePatinetForm").val();
    var data;

    if (GridRefreshLink === "/UserManagment/RefreshGrid") {
        if (ClinicSectionsMulti !== undefined) {
            let va = ClinicSectionsMulti.value();

            if (va[0] === undefined) {
                $("#ClinicSections-box").removeClass("hidden");
                $('#btn-new-accept').removeAttr("disabled");
                return;
            }
        }

        if (ClinicSectionsMulti !== undefined) {
            var ClinicSections = [];
            itemList = "";
            //var UserId = response;
            var itemList = $('#ClinicSections').data('kendoMultiSelect').dataItems();
            for (var i = 0; i < itemList.length; i++) {
                ClinicSections.push(itemList[i]["Guid"]);
            }

            itemList = ClinicSections.toString();

            $('#MultiSelectGuids').val(itemList);
        }
    }

    if (patientForm === undefined) {
        data = $("#addNewForm").serialize();
    }
    else {
        data = $("#addNewPatientForm").serialize();
    }


    if (!IfUserCheckPass(GridRefreshLink)) {
        $('#btn-new-accept').removeAttr("disabled");
        return;
    }
    if (!CheckIfPriceBiggerThanDiscount(GridRefreshLink)) {
        $('#btn-new-accept').removeAttr("disabled");
        return;
    }

    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: data,
        success: function (response) {
            $('#btn-new-accept').removeAttr("disabled");

            if (response !== 0) {
                if (response === "ValueIsRepeated") {

                    $('#Name-Exist').removeClass('hidden');
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else if (response === "WrongEmail") {

                    $("#Email-wrong").removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else if (response === "WrongMobile") {

                    $("#Mobile-wrong").removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else if (response === "SomeThingWentWrong") {

                    $('#my-modal-new #ERROR_SomeThingWentWrong').removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else if (response === "IncorrectPass") {

                    $('#incorrectPass').removeClass('hidden');
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                } else {
                    IfReserve(response, GridRefreshLink);
                    //IfBaseInfoAddBaseInfoType(response, GridRefreshLink);
                    //IfUser(response, GridRefreshLink);
                    IfDisease(response, GridRefreshLink);
                    IfVisit();
                    $('#my-modal-new').modal('hide');
                    $(".modal-backdrop:last").remove();
                    $('#new-modal-body').empty();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    if (GridRefreshLink === "/UserManagment/RefreshGrid") {
                        $("#kendoGrid .k-pager-refresh")[0].click();
                    } else {
                        $(".k-pager-refresh")[0].click();
                    }

                }
            } else {

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

                $('#my-modal-new #ERROR_SomeThingWentWrong').removeClass('hidden');
            }
        }
    });
});

function closeMyNewModal() {
    $(".loader").removeClass("hidden");
    $('#my-modal-new').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#new-modal-body').empty();
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}


$('#btn-update-accept').on("click", function () {
    $(this).attr("disabled", true);

    $('#my-modal-edit #ERROR_SomeThingWentWrong').addClass('hidden');
    $('.emptybox').addClass('hidden');

    var mobile = $("#Mobile-wrong");
    if (mobile) {
        mobile.addClass("hidden");

        if ($('[validate-mobile="Mobile-wrong"]').val() !== undefined) {
            let text = $('[validate-mobile="Mobile-wrong"]').val();
            if (text != "" & text.trim().length < 8) {
                mobile.removeClass("hidden");
                $('#btn-update-accept').removeAttr("disabled");
                return;
            }
        }
    }

    var email = $("#Email-wrong");
    if (email) {
        email.addClass("hidden");

        if ($('[validate-email="Email-wrong"]').val() !== undefined) {
            let text = $('[validate-email="Email-wrong"]').val();
            if (!ValidateEmail(text)) {
                email.removeClass("hidden");
                $('#btn-update-accept').removeAttr("disabled");
                return;
            }
        }
    }

    var isEmmpty = true;
    $('.emptybox').each(function () {
        if ($(this).attr('data-isEssential') === 'true') {
            var empty = $(this).attr('id');
            if ($('[data-checkEmpty="' + empty + '"]').val() === "") {
                $(this).removeClass('hidden');
                $('#btn-update-accept').removeAttr("disabled");
                isEmmpty = false;
                return;
            }
        }
    });
    if (isEmmpty === false) {
        return;
    }

    var link = $("#EditLink").attr("data-Value");
    var GridRefreshLink = $("#GridRefreshLink").attr("data-Value");

    if (!IfUserCheckPass(GridRefreshLink)) {
        $('#btn-update-accept').removeAttr("disabled");
        return;
    }
    if (GridRefreshLink === "/UserManagment/RefreshGrid") {
        if (ClinicSectionsMulti !== undefined) {
            let va = ClinicSectionsMulti.value();

            if (va[0] === undefined) {
                $("#ClinicSections-box").removeClass("hidden");
                $('#btn-update-accept').removeAttr("disabled");
                return;
            }
        }


        if (ClinicSectionsMulti !== undefined) {
            var ClinicSections = [];
            itemList = "";
            //var UserId = response;
            var itemList = $('#ClinicSections').data('kendoMultiSelect').dataItems();
            for (var i = 0; i < itemList.length; i++) {
                ClinicSections.push(itemList[i]["Guid"]);
            }

            itemList = ClinicSections.toString();

            $('#MultiSelectGuids').val(itemList);
        }
    }

    var data = $("#addNewForm").serialize();


    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: data,
        success: function (response) {
            $('#btn-update-accept').removeAttr("disabled");

            if (response !== 0) {
                if (response === "ValueIsRepeated") {
                    $('#Name-Exist').removeClass('hidden');
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else if (response === "WrongEmail") {

                    $("#Email-wrong").removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else if (response === "WrongMobile") {

                    $("#Mobile-wrong").removeClass('hidden');

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                } else if (response === "IncorrectPass") {

                    $('#incorrectPass').removeClass('hidden');
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                } else {
                    IfReserve(response, GridRefreshLink);
                    //IfUser(response, GridRefreshLink);
                    IfDisease(response, GridRefreshLink);
                    $('#my-modal-edit').modal('hide');
                    $(".modal-backdrop:last").remove();
                    $('#edit-modal-body').empty();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                    if (GridRefreshLink === "/UserManagment/RefreshGrid") {
                        $("#kendoGrid .k-pager-refresh")[0].click();
                    } else {
                        $(".k-pager-refresh")[0].click();
                    }
                }
            } else {

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

                $('#my-modal-edit #ERROR_SomeThingWentWrong').removeClass('hidden');
            }
        }
    });
});


function closeMyEditModal() {
    $(".loader").removeClass("hidden");
    $('#my-modal-edit').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#edit-modal-body').empty();
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}

$('#btn-qeue-accept-close').on("click", function () {
    $(".loader").removeClass("hidden");
    $('#my-modal-qeue').modal('hide');
    $(".modal-backdrop:last").remove();
    $('#qeue-modal-body').empty();
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
});

$('#btn-delete-accept').on("click", function () {
    $(this).attr("disabled", true);

    $("#my-modal-delete #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#my-modal-delete #InvoiceInUse").addClass("hidden");
    $("#my-modal-delete #ERROR_SomeThingWentWrong").addClass("hidden");

    var Id = $(this).attr('data-id');

    var link = $("#DeleteLink").attr("data-Value");

    var GridRefreshLink = $("#GridRefreshLink").attr("data-Value");
    if (!IfUserCheckPass(GridRefreshLink)) {
        $('#btn-delete-accept').removeAttr("disabled");
        return;
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
            $('#btn-delete-accept').removeAttr("disabled");

            if (response === "SUCCESSFUL") {
                $('#my-modal-delete').modal('hide');
                $(".modal-backdrop:last").remove();
                IfReserve(response, GridRefreshLink);
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
                if (GridRefreshLink === "/UserManagment/RefreshGrid") {
                    $("#kendoGrid .k-pager-refresh")[0].click();
                } else {
                    if (GridRefreshLink !== "/Reserves/RefreshGrid")
                        $(".k-pager-refresh")[0].click();
                }
            }
            else if (response === "ERROR_ThisRecordHasDependencyOnItInAnotherEntity") {
                $("#my-modal-delete #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "InvoiceInUse") {
                $("#my-modal-delete #InvoiceInUse").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "ERROR_SomeThingWentWrong") {
                $("#my-modal-delete #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "AreYouSure") {
                AskForDelete(Id);
            }
            else if (response === "CanNotDelete") {
                CanNotDelete();
            } else {

                $("#my-modal-delete #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });
});


$('#btn-delete-withPass-accept').on("click", function () {
    $(this).attr("disabled", true);

    $("#my-modal-withPass-delete #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").addClass("hidden");
    $("#my-modal-withPass-delete #InvoiceInUse").addClass("hidden");
    $("#my-modal-withPass-delete #ERROR_SomeThingWentWrong").addClass("hidden");
    $("#my-modal-withPass-delete #Wrong_Pass").addClass("hidden");
    $("#my-modal-withPass-delete #DeletePassword-box").addClass("hidden");

    var Id = $(this).attr('data-id');
    var pass = $("#my-modal-withPass-delete #DeletePassword").val();
    if (pass == '') {
        $("#my-modal-withPass-delete #DeletePassword-box").removeClass("hidden");
        $('#btn-delete-withPass-accept').removeAttr("disabled");
        return;
    }

    var link = $("#DeleteLink").attr("data-Value");

    var GridRefreshLink = $("#GridRefreshLink").attr("data-Value");
    if (!IfUserCheckPass(GridRefreshLink)) {
        $('#btn-delete-withPass-accept').removeAttr("disabled");
        return;
    }
    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: link,
        data: {
            __RequestVerificationToken: token.attr('value'),
            Id: Id,
            pass: pass
        },
        success: function (response) {
            $('#btn-delete-withPass-accept').removeAttr("disabled");

            if (response === "SUCCESSFUL") {
                $('#my-modal-withPass-delete').modal('hide');
                $(".modal-backdrop:last").remove();
                IfReserve(response, GridRefreshLink);
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");

                $("#my-modal-withPass-delete #DeletePassword").attr("disabled", true);

                if (GridRefreshLink === "/UserManagment/RefreshGrid") {
                    $("#kendoGrid .k-pager-refresh")[0].click();
                } else {
                    if (GridRefreshLink !== "/Reserves/RefreshGrid")
                        $(".k-pager-refresh")[0].click();
                }
            }
            else if (response === "ERROR_ThisRecordHasDependencyOnItInAnotherEntity") {
                $("#my-modal-withPass-delete #ERROR_ThisRecordHasDependencyOnItInAnotherEntity").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "InvoiceInUse") {
                $("#my-modal-withPass-delete #InvoiceInUse").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "ERROR_SomeThingWentWrong") {
                $("#my-modal-withPass-delete #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "WrongPass") {
                $("#my-modal-withPass-delete #Wrong_Pass").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response === "AreYouSure") {
                AskForDelete(Id);
            }
            else if (response === "CanNotDelete") {
                CanNotDelete();
            } else {

                $("#my-modal-withPass-delete #ERROR_SomeThingWentWrong").removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
        }
    });
});
