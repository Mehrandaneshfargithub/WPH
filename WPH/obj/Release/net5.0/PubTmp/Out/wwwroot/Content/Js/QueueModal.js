function startup() {
    video = document.getElementById('video');
    canvas = document.getElementById('canvas');
    photo = document.getElementById('photo');
    startbutton = document.getElementById('startbutton');
    startButtonDiv = document.getElementById('startButtonDiv');
    output = document.getElementById('Output');
    camera = document.getElementById('Camera');
    //width = (screen.width)*0.8;
    height = (screen.height) * 0.7;

    // access video stream from webcam
    navigator.mediaDevices.getUserMedia({
        video: true,
        audio: false
    })
        // on success, stream it in video tag
        .then(function (stream) {
            video.srcObject = stream;
            video.play();
        })
        .catch(function (err) {
            console.log("An error occurred: " + err);
        });

    video.addEventListener('canplay', function (ev) {
        if (!streaming) {
            //height = video.videoHeight / (video.videoWidth / width);

            //width = video.videoWidth;
            if (isNaN(height)) {
                height = width / (4 / 3);
            }

            //video.setAttribute('width', width);


            video.setAttribute('height', height);
            width = video.clientWidth;
            canvas.setAttribute('width', width);
            canvas.setAttribute('height', height);
            streaming = true;

        }
    }, false);

    startbutton.addEventListener('click', function (ev) {
        takepicture();
        ev.preventDefault();
    }, false);

    clearphoto();
}


function clearphoto() {
    var context = canvas.getContext('2d');
    context.fillStyle = "#AAA";
    context.fillRect(0, 0, canvas.width, canvas.height);

    var data = canvas.toDataURL('image/jpg');
    photo.setAttribute('src', data);

    imagebase64data = "";
    photo.setAttribute('src', '');

    camera.style.display = "block";
    startButtonDiv.style.display = "block";
    output.style.display = "none";


}


function takepicture() {



    var context = canvas.getContext('2d');
    if (width && height) {
        canvas.width = width;
        canvas.height = height;
        context.drawImage(video, 0, 0, width, height);


        imagebase64data = canvas.toDataURL('image/jpg');
        photo.setAttribute('src', imagebase64data);

        camera.style.display = "none";
        startButtonDiv.style.display = "none";
        output.style.display = "block";
        photo.width = width;
        photo.height = height;

        //imagebase64data = data.replace('data:image/png;base64,', '');
        //let vis = $("#Guid").val();
        //var patientid = $("#PatientId").val();
        //$.ajax({
        //    type: 'POST',
        //    url: '/Reserves/Async_Save',
        //    data: {files:imagebase64data, patientId:patientid, visitId:vis},  

        //    success: function (out) {  
        //        alert('Image uploaded successfully..');  
        //    }  
        //});  
    } else {
        clearphoto();
    }
}


function acceptCameraPic() {
    $(".loader").removeClass("hidden");
    //imagebase64data = imagebase64data.replace('data:image/png;base64,', '');
    let vis;
    if (type === "visit") {
        vis = $("#Guid").val();
    }
    else {
        vis = null;
    }

    var patientid = $("#PatientId").val();
    $.ajax({
        type: 'POST',
        url: '/Patient/Async_Save',
        data: { file: imagebase64data, patientId: patientid, visitId: vis },

        success: function (out) {
            clearphoto();
            var stream = video.srcObject;
            var tracks = stream.getTracks();

            for (var i = 0; i < tracks.length; i++) {
                var track = tracks[i];
                track.stop();
            }
            video.srcObject = null;
            $("#CameraModal").modal('toggle');
            if (type === "visit") {
                GetAllVisitImages();
            }
            else {
                GetAllPatientImages();
            }
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        }
    });

}

function cancelCameraPic() {
    clearphoto();
}



function GoToCamera(e) {
    type = e;
    $("#CameraModal").modal('toggle');
    startup();
}

function QueuDiseaseRecord() {
    $("#myModalLabel").removeClass("hidden");
    $("#myModalLabel2").addClass("hidden");
    $(".loader").removeClass("hidden");
    $("#PatientQueuRecordModal").modal('toggle');
    var patientid = $("#PatientId").val();
    $.ajax({
        type: "Post",
        data: { patientid: patientid },
        url: "/Patient/PatientRecordForm",
        dataType: "html",
        success: function (data) {
            $("#PatientQueuRecordModal").modal();
            $("#PatientQueuRecordModal-body").html(data);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        },
    });

}


function QueuMedicineRecord() {

    $(".loader").removeClass("hidden");
    $("#PatientQueuRecordModal").modal('toggle');
    $("#myModalLabel").addClass("hidden");
    $("#myModalLabel2").removeClass("hidden");
    var patientid = $("#PatientId").val();
    $.ajax({
        type: "Post",
        data: { patientid: patientid },
        url: "/Patient/PatientRecordMedicineForm",
        dataType: "html",
        success: function (data) {
            $("#PatientQueuRecordModal").modal();
            $("#PatientQueuRecordModal-body").html(data);
            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");
        },
    });

}


function patientQueuHide() {


    $('#PatientQueuRecordModal').modal('hide');

    $('#PatientQueuRecordModal-body').empty();


}


function Pasand() {

    $('#btn-qeue-accept').click();

}

function DeletePatientImage(element) {

    bootbox.confirm('Do you Want Delete This Image?', function (result) {

        if (!result) {

            return;

        }
        else {
            var Id = $(element).attr("data-Id");

            $(".loader").removeClass("hidden");
            $.ajax({

                url: '/Patient/RemovePatientImage',

                type: "Post",

                data: { patientImageId: Id },

                success: function () {

                    $("#PatientImages").html('');
                    GetAllVisitImages();
                    GetAllPatientImages();
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                }


            });
        }

    })

}

function GetAllVisitImages() {

    $("#VisitImages").html('');
    let vis = $("#Guid").val();
    $.ajax({

        url: '/Visit/GetAllVisitImages',

        type: "Post",

        data: { visitId: vis },

        success: function (eventList) {

            for (let i = 0; i < eventList.length; i++) {
                $("<div class='col-xs-4 col-sm-4' style='text-align:center;margin-bottom:2rem'>" +
                    "<div style='margin-bottom:1rem'> <img src=" + eventList[i].ImageAddress + " width='150' height='150' ondblclick='openPic(this)' /></div>" +
                    "<h5>" + eventList[i].FileName + "</h5>" +
                    "<a class='k-primary grid-btn' style='margin-top:5rem'  onclick='DeletePatientImage(this)' data-Id =" + eventList[i].Guid + " > Delete </a>" +
                    "</div> ").appendTo($("#VisitImages"));
            }

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");

        }


    });

}


function GetAllPatientImages() {

    $(".loader").removeClass("hidden");
    var patientId = $("#PatientId").val();
    let vis = $("#Guid").val();
    $.ajax({

        url: '/Patient/GetAllPatientImages',

        type: "Post",

        data: { patientId: patientId },

        success: function (eventList) {

            $("#PatientImages").html('');
            for (let i = 0; i < eventList.length; i++) {
                if (eventList[i].VisitId !== vis) {
                    $("<div class='col-xs-6 col-sm-4' style='text-align:center;margin-bottom:2rem'>" +
                        "<div style='margin-bottom:1rem'> <img src=" + eventList[i].ImageAddress + " width='150' height='150' ondblclick='openPic(this)' /></div>" +
                        "<h5>" + eventList[i].FileName + "</h5>" +
                        "<a class='k-primary grid-btn' style='margin-top:5rem'  onclick='DeletePatientImage(this)' data-Id =" + eventList[i].Guid + " > Delete </a>" +
                        "</div> ").appendTo($("#PatientImages"));
                }

            }

            $(".loader").fadeIn("slow");
            $(".loader").addClass("hidden");

        }


    });

}



function openPic(e) {

    //var destenation = $(e).next().next().text();
    var destenation1 = $(e).attr('src');



    $("#imagePreview").attr("src", destenation1);
    $("#window").data("kendoWindow").center().open();
}
