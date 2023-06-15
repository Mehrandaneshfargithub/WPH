$('.chooseLang').on("click", function (e) {
    var language = $(this).attr('name');

    

    $.ajax({
        type: "Post",
        url: "/UserHandler/chooseLanguage",
        data: { language: language },
        success: function (response) {
            window.location.href = "/UserHandler/Index";
        }
    });
});

