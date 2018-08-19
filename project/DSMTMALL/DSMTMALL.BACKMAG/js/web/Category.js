$(function () {

    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#tableCGList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tIsEnable]").eq(0).attr("mydata")) == "1") {
            $(this).find("[name=tIsEnable]").eq(0).find("[name=isCheckEnable]").attr("checked", "checked");
        }
    });
});

//类目添加
function AddCategory(obj) {
    var newCategoryName = $.trim($("#newCategoryName").val());
    var parentID = $.trim($("#choiceParentsID").val());
    if (newCategoryName.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：要添加的类目名称不能为空"));
    }
    else {
        BtnAble(obj, false, "正在提交");
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "addCategory", "name": newCategoryName,"id":parentID, "createTime": new Date() };
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
            BtnAble(obj, true, "提交");
        });
    }
    return false;
}

//类目修改-显示
function ShowUpdateCategory(obj) {
    $("#updateCategoryName").val($.trim($(obj).parents("tr").eq(0).find("[name=tName]").html()));
    $("#updateChoiceParentsID").val($.trim($(obj).parents("tr").eq(0).find("[name=tID_1]").html()));
    $("#updateModel").data("id", $.trim($(obj).parents("tr").eq(0).find("[name=tID]").html()))
}
//类目修改-执行
function UpdateCategory(obj) {
    var categoryName = $.trim($("#updateCategoryName").val());
    var sysID = $.trim($("#updateModel").data("id"));
    var parentID = $.trim($("#updateChoiceParentsID").val());
    if (sysID.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：系统出错，请重新登陆"));
    }
    else if (newCategoryName.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：要添加的类目名称不能为空"));
    }
    else {
        BtnAble(obj, false, "正在提交");
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "updateCategory", "name": categoryName, "id": parentID,"cid":sysID, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") {
                    ShowSuccess(obj);
                } else {
                    ShowError(obj,data);
                }
            } else {
                ToPage("login");
            }
            BtnAble(obj, true, "提交");
        });
    }
    return false;
}

