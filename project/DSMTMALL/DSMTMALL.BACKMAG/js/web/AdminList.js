$(function () {
    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });

    $("#assignCardModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });

    $("#addModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });

    $("#assignModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });

    $("#tableAdminList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tIsEnable]").eq(0).attr("mydata")) == "1") {
            $(this).find("[name=tIsEnable]").eq(0).find("[name=isCheckEnable]").attr("checked", "checked");
        }
    });

    $("#tableAdminList").find("tr").each(function () {
        if (!ValIsDefine( $.trim($(this).find("[name=tName]").eq(0).find("span").html()))) {
            $(this).find("[name=tName]").eq(0).find("span").html("暂无");
        }
    });
    $("#tableAdminList").find("tr").each(function () {    
        if (!ValIsDefine($.trim($(this).find("[name=tPhone]").eq(0).find("span").html()))) {
            $(this).find("[name=tPhone]").eq(0).find("span").html("暂无");
        }
    });
});

//function assignCard(obj) {
//    var id = $.trim($(obj).attr("mydata"));
//    var url = "/web/web_ajax.aspx";
//    var args = { "jsonType": "assignCard", "createTime": new Date() };
//    $.post(url, args, function (data) {
//        var myArr = $.trim(data).split(",");
//        if (myArr[0] != "UNLOGIN") {
//            if (myArr[0] == "SUCCESS") {
//                if (myArr.length == 2) {
//                    var checkCode = GetGuid();//获取标识码给读卡器
//                    window.location.href = "DSMTCard://MALL/" + myArr[1] + "/" + checkCode;
//                    var timerCount = 0;
//                    args = { "jsonType": "checkAdminAssign", "token": myArr[1], "code": checkCode, "id": id, "createTime": new Date() };
//                    var timer = self.setInterval(function () {
//                        if (timerCount < 5) {
//                            $.post(url, args, function (data2) {
//                                myArr = $.trim(data2).split(",");
//                                if (myArr[0] != "ERROR") {
//                                    clearInterval(timer);
//                                    if (myArr[0] == "SUCCESS") {
//                                        $("#assignCardModal").modal("hide");
//                                        alert("发卡成功");
//                                        clearInterva(timer);
//                                    } else {
//                                        $("#assignCardModal").modal("hide");
//                                        alert(data2);
//                                    }
//                                }
//                            });
//                        } else {
//                            clearInterval(timer);
//                            alert("提示：连接超时，请重试");
//                            $("#assignCardModal").modal("hide");
//                        }
//                        timerCount++;
//                    }, 1000);
//                } else {
//                    $("#assignCardModal").modal("hide");
//                    alert(myArr[0]);
//                }
//            } else {
//                $("#assignCardModal").modal("hide");
//                alert(data);
//            }
//        } else {
//            ToPage("login");
//        }
//    });
//}

//进行账户发卡  ----user_list.aspx
function assignCard(obj) {
    var id = $.trim($(obj).attr("mydata"));
    var checkCode = "ML_" + GetGuid();//获取标识码给读卡器
    if (ValIsDefine(id)) {
        window.location.href = "DSMTCard://" + checkCode;
        var args = { "jsonType": "checkAdminAssign", "code": checkCode, "id": id, "createTime": new Date() };
        $.ajax({
            type: "post",
            url: "/web/web_ajax.aspx",
            data: args,
            dataType: "text",
            timeout: 0,
            async: true,
            success: function (data) {
                if (data == "SUCCESS") {
                    $("#assignCardModal").modal("hide");
                    alert("发卡成功");
                } else {
                    $("#assignCardModal").modal("hide");
                    alert(data);
                }
            },
            error: function () {
                $("#assignCardModal").modal("hide");
                alert("发卡失败");
            }
        });
    } else {
        alert("数据错误,请刷新页面");
    }
    return false;
}



function AddAdminAmount(obj) {
    var addName = $.trim($("#addAdminName").val());
    var addPhone = $.trim($("#addAdminPhone").val());
    var addSupplier = $.trim($("#addSuppliers").val());
    var addRemark = $.trim($("#addRemark").val());
    if (ValIsDefine(addName)) {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "addAdminAmount","name":addName,"number":addPhone,"cid":addSupplier,"remark":addRemark, "createTime": new Date() };
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
        });
    } else {
        ShowError(obj, "提交的数据不正确");
    }
}

function ShowAssignModal(obj) {
    $("#assignModal").data("id", $.trim($(obj).attr("mydata")))
    var url = "/web/web_ajax.aspx";
    var args = { "jsonType": "getModalList", "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data != "UNLOGIN") {
            var jsonData = eval(data);
            var inner = "";
            if (jsonData != "") {
                $.each(jsonData, function (key, val) {
                    inner += "<div class=\"col-xs-4\"><div class=\"form-group\"><input name=\"sltModalInfo\" type=\"checkbox\" value=\"" + val["ModalSysID"] + "\" />" + val["ModalName"] + "</div></div>";
                });
                $("#chooseModalInfo").html(inner);
            }
        } else {
            ToPage("login");
        }
    });
}

function AssignModal(obj) {
    var sysID = $("#assignModal").data("id");
    var charStr = "";
    $.each($("input[name=sltModalInfo]"), function () {
        if ($(this).prop("checked") == true) {
            charStr += $.trim($(this).val()) + ",";
        }
    });
    if (sysID.length <= 0) {
        ShowError(obj, "提示：系统出错请重新登陆");
    }else {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "assignAdminModal", "id": sysID, "word": charStr, "creatTime": new Date() };
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
            BtnAble(obj, true, "分配");
        });
    }
}