$("#settings-page").on("click", function (e) {
    goToSettingPage();
});
$("#settings-page-Navigation-menu").on("click", function (e) {
    goToSettingPage();
});
$("#settings-LogOut").on("click", function (e) {
    LogOut();
});
function goToSettingPage() {
    $(".loader").removeClass("hidden");
    $("body *").unbind();
    $('.page-content').load('/InterfaceSettings/loadSettingPage', function () {

        $("button").attr("disabled", true);
        $("a").attr("disabled", true);
        $("select").attr("disabled", true);
        $("input").attr("disabled", true);
        $("label").attr("disabled", true);
        $('#js').load('/InterfaceSettings/loadJs', function () {
            $("button").attr("disabled", false);
            $("a").attr("disabled", false);
            $("select").attr("disabled", false);
            $("input").attr("disabled", false);
            $("label").attr("disabled", false);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        });

    });
}

function LogOut() {
    $(".loader").removeClass("hidden");
    $(".sideItem").removeClass("active");
    $(this).addClass("active");
    
    $(".loader").fadeIn("slow");
    $(".loader").addClass("hidden");
}

$(document).ready(function () {
    loadSettings();
});
