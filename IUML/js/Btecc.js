var _ErrorMessage = "<b style=\"font-size:26px;color:red;text-decoration:underline;background-color:yellow;\">PLEASE CONTACT TO ADMIN TEAM.</b><br /><br />@ERROR_MSG";
function OpenAlertPopup(_Msg, _status)//
{
    $("#btnOkAlert").removeAttr("class");
    if (_status == "danger")//
    {
        $("#btnOkAlert").addClass("btn btn-danger");
    }
    else if (_status == "warning")//
    {
        $("#btnOkAlert").addClass("btn btn-warning");
    }
    else if (_status == "success")//
    {
        $("#btnOkAlert").addClass("btn btn-success");
    }
    else if (_status == "info")//
    {
        $("#btnOkAlert").addClass("btn btn-info");
    }
    else//
    {
        $("#btnOkAlert").addClass("btn btn-primary");
    }
    $("#lblAlertMesage").html(_Msg);
    $("#alertPopup").modal({
        backdrop: 'static',
        keyboard: false
    });
}

function Upload_CV_via_Service(_imageType, files, _fileUploadControlName, _oldImagePath, _imagesParentPanelID, _userID, _UserType)//
{
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }

    $.ajax({
        url: "../WebService/FileUploadHandler.ashx?imagetype=" + _imageType + "&userid=" + _userID + "&oldimagepath=" + _oldImagePath + "&clientdatetime=" + Get_CurrentDateTime(),
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            Get_ImageList_by_ImageType(_imageType, _fileUploadControlName, _imagesParentPanelID, _UserType);
        },
        error: function (err) {
            $("#" + _fileUploadControlName).removeAttr("disabled");
            $("#" + _fileUploadControlName).html("<i class='fa fa-upload'></i>&nbsp;Upload");

            OpenAlertPopup(_ErrorMessage.replace("@ERROR_MSG", err.responseText), "danger");
            waitingDialog.hide();
        }
    });
}