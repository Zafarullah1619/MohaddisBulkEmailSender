




$("#AddFeedbackForm").validate({
    //ignore: [],
    //ignore: ':hidden:not("#FeedBackForm")',
    rules: {

        FeedBack: {
            required: true,
        },
        //FeedBackForm: {
        //    required: true
        //},
        StatusId: {
            required:true
        }
    },

    messages: {

        FeedBack: {
            required: "Please Enter Feed Back"
        },
        //FeedBackForm: {
        //    required: "Please Upload Feed Back Form"
        //},
        StatusId: {
            required:"Please Select Status"
        }
    },
    submitHandler: function (form) {
        //AssignEditorValueToFields();
        form.submit();
    },
    errorPlacement: function (error, element) {
        $(element)
                .closest("form")
                .find("label[for='" + element.attr("id") + "']")
                .append(error);
    },
    errorElement: "span",
});




$("#FeedbackFile").fileupload({
    dataType: "json",
    url: $("#hdUploadFeedBackFormURL").val(),
    autoUpload: true,

    always: function (e, data) {
        if (data.result.name === "success") {
            ShowStickyPopup();

            var OldImg = $("#FeedBackForm").val();
            var src = '/Images/NoImageAvailable.png';
            $("#FeedBackForm").val(data.result.filename);
            $("#imgLogo").attr("src", "../../../" + data.result.path);
            if ($('#lnkViewFeedback').length >0) {
                $("#lnkViewFeedback").attr("href", "../../../" + data.result.path);
                $('#lnkViewFeedback').attr('target', '_blank');
                //$('#lnkViewFeedback').bind('click');
            }
            
            $(".removeForm").removeClass("hidden");
            $(".removeForm").attr("data-name", data.result.filename);
            //alert(OldImg);
            $.ajax({
                type: "POST",
                url: $("#hdRemoveFeedbackForm").val(),
                data: {
                    "FileName": OldImg
                },
            success: function (data) {
                HideStickyPopup();
            },
            error: function (e, t, s) {
                //HideLoadingPopup();
                swal({
                    title: "Error!",
                    text: "An unkonwn error has occured, please try later.",
                    type: "error"
                });
                HideStickyPopup();
            }

        });
        HideLoadingPopup();

    } else if (data.result.name === "invalidExtension") {
    alert('Only Image extensions are allowed for example GIF, PNG, JPG, JPEG. Please check your file extension.');
}
else if (data.result.name === "size_exceeded") {
    alert('Maximum file size allowed is 2MB. Please check your file size.');
}
else if (data.result.name === "invalid_format") {
    alert("System could not verify the Image file format, Only Image file extensions are allowed for example GIF, PNG, JPG, JPEG. Please check your file extension/format.");
}
else if (data.result.name === "fail") {
    alert("File could not be uploaded. Please try again.");
}
},
add: function (e, data) {

    var uploadFile = data.files[0];
    var ext = uploadFile.name.split('.').pop().toLowerCase();
    if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
        alert('Only Image extensions are allowed for example GIF, PNG, JPG, JPEG. Please check your file extension.');
    }
    else if (data.files[0].size > 2097152) {
        //524288 = 500KB
        //1048576 = 1MB
        //alert(data.files[0].size);
        alert('Maximum file size allowed is 2MB.\n Please check your file size.');
    } else {
        //data.formData = { ImageType: "IMAGE", IsAutoResize: $('#chkAutoResize').is(":checked") ? true : false };
        data.submit();
        ShowLoadingPopup();
    }

},
fail: function (e, data) {
    alert(data.errorThrown);
},
//progressall: function (e, data) {
//$("#IdProgressBar").css("display", "block");
//var progress = parseInt(data.loaded / data.total * 100, 10);
//$(".progress .progress-bar").css("width", progress + "%");
//},
done: function (e, data) {
    // alert('File uploaded successfully');
}


});

$(".removeForm").click(function(){
    //debugger;
    var OldImg = $(this).attr("data-name");
    //var src = '/Images/BannerNotAvailable.jpg';
    var src = '/Images/NoImageAvailable.png';
    swal({
        title: "Remove Form",
        text: "Are you sure to remove Form?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#D24637',
        confirmButtonText: 'Yes, I am sure!',
        cancelButtonText: "No, cancel it!",
        closeOnConfirm: true,
        closeOnCancel: true
    },
    function (isConfirm) {
        if (isConfirm) {
        ShowStickyPopup();
            $.ajax({
                type: "POST",
                url: $("#hdRemoveFeedbackForm").val(),
                data: {
                    "FileName": OldImg
                },
            success: function (data) {
                //HideLoadingPopup();
                $("#FeedBackForm").val('');
                $("#imgLogo").attr("src", src);
                $(".removeForm").addClass("hidden");
                if ($('#lnkViewFeedback').length > 0) {
                    $("#lnkViewFeedback").attr("href", src);
                    $('#lnkViewFeedback').attr('target', '_blank');
                    //$('#lnkViewFeedback').unbind("click");
                }
                HideStickyPopup();
            },
            error: function (e, t, s) {
                HideStickyPopup();
                swal({
                    title: "Error!",
                    text: "An unkonwn error has occured, please try later.",
                    type: "error"
                });

            }

        });
    }
    })
})