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

function fileUpload_ClientSelect()//
{
    alert('File Upload Hurrrrr');
}

function Setting()//
{
    alert('Work-in-Progress');
}

function Logout()//
{
    localStorage.setItem("SHAJAN_CAM_USERID", "");
    window.location = "../index.html";
}

function imageDelete(_id)//
{
    alert('Work-in-Progress' + _id);
}

function Upload_Image_via_Service(_imageType, files, _fileUploadControlName, _oldImagePath, _imagesParentPanelID, _userID, _UserType)//
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

function Get_ImageList_by_ImageType(_imageType, _fileUploadControlName, _imagesParentPanelID, _UserType)//
{
    $("#" + _imagesParentPanelID).html('');

    var Get_ImageList_by_ImageType_Param = {
        _ImageType: _imageType,
        _UserType: _UserType
    };

    $.ajax({
        type: "POST",
        url: "../WebService/ShajanCam_Service.asmx/Get_ImageList_by_ImageType",
        data: JSON.stringify(Get_ImageList_by_ImageType_Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != null && data.d != undefined)//
            {
                var _DivContentCenterHtml = "";

                var _imgParentHtml = "";
                var _img_html = "";
                if (_UserType == "admin")//
                {
                    var _UserImgHtml = '<div class="col-sm-4 col-md-4 col-lg-3 nopad text-center"><label class="image-checkbox"><img class="img-responsive" src="../@IMAGE_URL" /><input id="cbx@IMAGE_ID" type="checkbox" name="image[]" value="@IMAGE_ID" /><i class="fa fa-check fa-lg hidden"></i></label></div>';
                    var _UserImgParentHtml = '@IMAGE_LIST';
                    _imgParentHtml = _UserImgParentHtml;
                    _img_html = _UserImgHtml;
                }
                else if (_UserType == "user")//
                {
                    var _UserImgHtml = '<a id="@IMAGE_A_ID" href="../@IMAGE_URL" data-lightbox="studio1"><img id="img@IMAGE_ID" src="../@IMAGE_URL" class="all @IMAGE_TYPE" /></a>';
                    var _UserImgParentHtml = '<div id="gallery-content"><div id="gallery-content-center">@IMAGE_LIST</div></div>';
                    _imgParentHtml = _UserImgParentHtml;
                    _img_html = _UserImgHtml;
                }
                else if (_UserType == "gallery")//
                {
                    var _AdminImgHtml = '<figure><a href="@IMAGE_URL" data-lightbox="studio2" class="photostack-img"><img src="@IMAGE_URL" alt="img05" width="280" height="320" /></a></figure>';
                    var _AdminImgParentHtml = "@IMAGE_LIST";
                    _imgParentHtml = _AdminImgParentHtml;
                    _img_html = _AdminImgHtml;
                }
                else if (_UserType == "home")//
                {
                    var _AdminImgHtml = '<div class="item@ACTIVE_STATUS"><img src="@IMAGE_URL"><div class="carousel-caption"><h2 class="sub-title-home">We Don\'t Take Photograph</h2><h1 class="title-home">We Make It</h1></div></div>';
                    var _AdminImgParentHtml = '<div class="carousel-inner col-centered" role="listbox">@IMAGE_LIST</div>';
                    _imgParentHtml = _AdminImgParentHtml;
                    _img_html = _AdminImgHtml;
                }

                var _jsonData = JSON.parse(data.d);
                for (var i = 0; i < _jsonData.length; i++)//
                {
                    var dataItem = _jsonData[i];

                    var _text = _img_html;
                    _text = _text.replace("@IMAGE_A_ID", "btn" + dataItem.ID);
                    _text = _text.replace(/@IMAGE_ID/g, dataItem.ID);
                    _text = _text.replace("@IMAGE_URL", dataItem.Image_URL).replace("@IMAGE_URL", dataItem.Image_URL);
                    _text = _text.replace("@IMAGE_TYPE", dataItem.Image_Type == "portrait" ? "studio" : "landscape");
                    if (_UserType == "home")//
                    {
                        if (i == 0)//
                        {
                            _text = _text.replace("@ACTIVE_STATUS", " active");
                        }
                        else//
                        {
                            _text = _text.replace("@ACTIVE_STATUS", "");
                        }
                    }
                    _DivContentCenterHtml += _text;
                }

                _DivContentCenterHtml = _imgParentHtml.replace("@IMAGE_LIST", _DivContentCenterHtml);
                $("#" + _imagesParentPanelID).html(_DivContentCenterHtml);

                if (_fileUploadControlName != "")//
                {
                    $("#" + _fileUploadControlName).removeAttr("disabled");
                    $("#" + _fileUploadControlName).html("<i class='fa fa-upload'></i>&nbsp;Upload");

                    OpenAlertPopup("Selected image(s) uploaded successfully.", "success");
                }
                
                setTimeout(function () {
                    if (_UserType == "gallery")//
                    {
                        gallery_Load();
                    }
                    else if (_UserType == "admin")//
                    {
                        imgCheckBox_Click();
                    }
                    else if (_UserType == "user")//
                    {
                        animated_masonry_gallery_Load();
                    }
                }, 1000);
            }

            waitingDialog.hide();
        },
        error: function (err) {
            $("#" + _fileUploadControlName).removeAttr("disabled");
            $("#" + _fileUploadControlName).html("<i class='fa fa-upload'></i>&nbsp;Upload");

            OpenAlertPopup(_ErrorMessage.replace("@ERROR_MSG", err.responseText), "danger");
            waitingDialog.hide();
        }
    });
}


function Get_CurrentDateTime() {
    var todaysDate = new Date();
    var _UserSystemDate = todaysDate.getFullYear() + "-" + (todaysDate.getMonth() + 1) + "-" + todaysDate.getDate() + " " + todaysDate.getHours() + ":" + todaysDate.getMinutes() + ":" + todaysDate.getSeconds(); //+ "." + todaysDate.getMilliseconds();
    return _UserSystemDate;
}

function Delete_Selected_Images(_imageType, _fileUploadControlName, _imagesParentPanelID, _UserType, _imgID_List, _fileDeleteButtonID)//
{
    var Delete_Image_Param = {
        _ImageType: _imageType,
        _IDs: _imgID_List
    };

    $.ajax({
        type: "POST",
        url: "../WebService/ShajanCam_Service.asmx/Delete_Image",
        data: JSON.stringify(Delete_Image_Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != null && data.d != undefined)//
            {
                if (data.d == true)//
                {
                    $("#DeleteImagePopup").modal("hide");
                    OpenAlertPopup("Selected image(s) deleted successfully.", "danger");

                    Get_ImageList_by_ImageType(_imageType, _fileUploadControlName, _imagesParentPanelID, _UserType);

                    $("#" + _fileDeleteButtonID).removeAttr("disabled");
                    $("#" + _fileDeleteButtonID).html("<i class='fa fa-trash fa-2x'></i>");

                    var arr = [];
                    $('input[type="checkbox"]:checked').each(function () {
                        arr.push($(this).val());
                    });
                    if (arr.length > 0)//
                    {
                        $("#" + _fileDeleteButtonID).css("display", "block");
                    }
                    else//
                    {
                        $("#" + _fileDeleteButtonID).css("display", "none");
                    }
                }
            }
        },
        error: function (err) {
            $("#" + _fileDeleteButtonID).removeAttr("disabled");
            $("#" + _fileDeleteButtonID).html("<i class='fa fa-trash fa-2x'></i>");

            OpenAlertPopup(_ErrorMessage.replace("@ERROR_MSG", err.responseText), "danger");
            waitingDialog.hide();
        }
    });
}

function imgCheckBox_Click()//
{
    // image gallery
    // init the state from the input
    $(".image-checkbox").each(function () {
        if ($(this).find('input[type="checkbox"]').first().attr("checked")) {
            $(this).addClass('image-checkbox-checked');
        }
        else {
            $(this).removeClass('image-checkbox-checked');
        }
    });

    // sync the state to the input
    $(".image-checkbox").on("click", function (e) {
        $(this).toggleClass('image-checkbox-checked');
        var $checkbox = $(this).find('input[type="checkbox"]');
        $checkbox.prop("checked", !$checkbox.prop("checked"));

        var arr = [];
        $('input[type="checkbox"]:checked').each(function () {
            arr.push($(this).val());
        });
        if (arr.length > 0)//
        {
            $("#btnFileDelete").css("display", "block");
        }
        else//
        {
            $("#btnFileDelete").css("display", "none");
        }

        e.preventDefault();
    });
}