$(function () {
    $("#uploadPicture").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    var brandID = $.trim($("#updateBrand").attr("mydata"));
    var goodsID = $.trim($("#saveInfo").attr("mydata"));
    if (brandID.length > 0) {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": "getBrandName", "id": brandID, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            var dataArr = data.split("&");
            if (dataArr.length > 0) {
                $("#updateBrand").attr("mydata", dataArr[0]).val(dataArr[1]);
            }
        });
    }
    if (ValIsDefine(goodsID)) {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": "getGalleryImgList", "id": goodsID, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            var jsonArr = eval(data);
            if (jsonArr.length > 0) {
                var objHtml = $("#goodsImgList");
                $.each(jsonArr, function (key,val) {
                    AddImgsHtml(val["ImgUrl"], val["ImgID"], objHtml);
                });
               
            }
        });
    }
});

KindEditor.ready(function (K) {
    var editor1 = K.create('#myContent', {
        //cssPath: '/editor/plugins/code/prettify.css',
        uploadJson: '/web/upload_file.ashx',
        allowFileManager: false,
        afterCreate: function () {
            var self = this;
            K.ctrl(document, 13, function () {
                self.sync();
                K('form[name=example]')[0].submit();
            });
            K.ctrl(self.edit.doc, 13, function () {
                self.sync();
                K('form[name=example]')[0].submit();
            });
        }
    });
    
    prettyPrint();
});

//Editor编辑器保存内容
function SaveEditorInfo(obj) {
    //var editorText = $.trim($(document.getElementsByTagName("iframe")[0].contentWindow.document.body).html());
    var editorText = KindEditor.escape($.trim($(document.getElementsByTagName("iframe")[0].contentWindow.document.body).html()));
    var goodsName = $.trim($("#myGoodsName").val());
    var goodsSupplier = $.trim($("#sltBySuppliers").val());
    var goodsCateName = $.trim($("#sltByCateName").val());
    var goodsBrand = $.trim($("#updateBrand").attr("mydata"));
    var goodsBrief = $.trim($("#myGoodsBrief").val());
    var goodsRemark = $.trim($("#myRemark").val());
    var goodsShopPrice = $.trim($("#shopPrice").val());
    var goodsMarketPrice = $.trim($("#marketPrice").val());
    var goodsKeyWords = $.trim($("#myKeyWords").val());
    var goodsFareSysID = $.trim($("#updateFareTemp").val());
    var sysID = $.trim($("#saveInfo").attr("mydata"));
    if (sysID.length <= 0) {
        ShowError(obj, "系统忙，请稍后再试");
    } else if (ValIsDefine(goodsName) && ValIsDefine(goodsSupplier) && ValIsDefine(goodsCateName)  && ValIsDefine(goodsShopPrice) && ValIsDefine(goodsMarketPrice) && ValIsDefine(goodsKeyWords) && ValIsDefine(goodsFareSysID)) {
        BtnAble(obj, false, "正在保存");
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": "updataGoodsDetial", "id": sysID, "name": goodsName, "typeB": goodsSupplier, "typeC": goodsCateName,"word":editorText, "typeT": goodsBrand, "url": goodsBrief, "remark": goodsRemark, "price_s": goodsShopPrice, "price_m": goodsMarketPrice, "cid": goodsFareSysID, "isVal": goodsKeyWords, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") {
                    ShowSuccess(obj);
                } else {
                    ShowError(obj, data);
                }
            } else {
                ToPage("login");
            }
            BtnAble(obj, true, "保存");
        });
    } else {
        ShowError(obj, "提交的参数不正确");
    }
    return false;
}

function DeleteThisImg(obj) {
    var sysID = $.trim($(obj).attr("mydata"));
    if (sysID.length <= 0) {
        alert("提示：新上传的图片须先刷新当前页面后才能删除");
    } else {
        $("#confirmModal").modal("show");
        $("#sgConfirm").html("确认删除当前这张商品图片");
        $("#aConfirm").val("确认");
        $("#aConfirm").off("click").on("click", function () {
            BtnAble($("#aConfirm"), false, "正在操作");
            var url = "/web/web_ajax.aspx";
            var args = {
                "jsonType": "deleteGalleryImg", "id": sysID, "createTime": new Date()
            };
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {
                        $("#aConfirm").parent().before(ShowAlert("success", "SUCCESS"));
                        BtnAble($("#aConfirm"), true, "确认");
                        $("#confirmModal").modal("hide");
                        $(obj).parents("div").eq(0).remove();
                    } else {
                        ShowError($("#aConfirm"), data);
                        BtnAble($("#aConfirm"), true, "确认");
                    }
                } else {
                    ToPage("login");
                }
            });
        });
    }
    return false;
}

function AddImgsHtml(valUrl,valID, objHtml) {
    objHtml.prepend("<div ><img  src=\"" + valUrl + "\" onclick=\"DeleteThisImg(this)\" onerror=\"nofind()\" mydata=\"" + valID + "\"/></div>");
}

//更换产品图片_first改
function ShowUploadPicture_this(obj) {
    var sysID = $.trim($("#saveInfo").attr("mydata"));
    var uType = $.trim($(obj).attr("mydata"));
    $("#uploadPicture").data("id", sysID);
    $("#uploadPicture").attr("mydata", $.trim($(obj).attr("mydata")));
}

function ajaxFileUpload(postUrl, fileID) {
    $.ajaxFileUpload
    (
        {
            url: postUrl,
            secureuri: false,
            fileElementId: fileID,
            dataType: 'json',
            success: function (data, status) {
                if (data["msg"].indexOf("ERR") >= 0) {
                    alert(data["msg"]);
                    ToPage("now");
                } else {
                    var objHtml = $("#goodsImgList");
                    AddImgsHtml(data["msg"], "", objHtml);
                    $("#uploadPicture").modal("hide");
                }
            },
            error: function (data, status, e) {
                alert(e);
                ToPage("now");
            },
        }
    );
}