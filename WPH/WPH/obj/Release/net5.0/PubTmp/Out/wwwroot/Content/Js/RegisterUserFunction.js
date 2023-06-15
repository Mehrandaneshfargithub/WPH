$('#loginUserFunc').on("click", function () {
    $(this).attr("disabled", true);

    var data = $("#loginForm").serialize();
    $(".loader").removeClass("hidden");
    $.ajax({
        type: "Post",
        url: "/UserHandler/LoginUser",
        data: data,
        success: function (response) {
            $('#loginUserFunc').removeAttr("disabled");

            if (!response) {
                $('#loginError').removeClass("hidden");
                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
            }
            else if (response) {
                window.location.href = "/ApplicationHandler/Index";
                $(".loader").fadeIn("slow");
            }
        }
    })
});