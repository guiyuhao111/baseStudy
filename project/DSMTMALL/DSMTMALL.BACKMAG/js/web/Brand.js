$(function () {

    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#tableBrandList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tIsShow]").eq(0).attr("mydata")) == "1") {
            $(this).find("[name=tIsShow]").eq(0).find("[name=isCheckEnable]").attr("checked", "checked");
        }
    });
});
//品牌信息添加
function AddBrand(obj) {
    var newBrandName = $.trim($("#newBrandName").val());
    if (newBrandName.length <= 0) {
        ShowError(obj, "要添加的品牌名称不能为空");
        //$(obj).parent().before(ShowAlert("danger", "提示：要添加的品牌名称不能为空"));
    }
    else {
        BtnAble(obj, false, "正在提交");
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "addBrand", "name": newBrandName, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") {
                    ShowSuccess(obj);
                } else {
                    $(obj).parent().before(ShowAlert("danger", data));
                }
            } else {
                ToPage("login");
            }
            BtnAble(obj, true, "提交");
        });
    }
    return false;
}
//显示品牌更新
function ShowUpdateBrand(obj) {
    $("#updateBrandName").val($.trim($(obj).parents("tr").eq(0).find("[name=tName]").eq(0).html()));
    $("#updateModal").data("id", $.trim($(obj).parents("tr").eq(0).find("[name=tID]").eq(0).html()))
}
function UpdateBrand(obj) {
    var updateBrandName = $.trim($("#updateBrandName").val());
    var updateBrandID = $.trim($("#updateModal").data("id"));
    if (updateBrandName.length <= 0) {
        //$(obj).parent().before(ShowAlert("danger", "提示：要添加的类目名称不能为空"));
        ShowError(obj, "要添加的类目名称不能为空");
    }
    else {
        BtnAble(obj, false, "正在提交");
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "updateBrand", "name": updateBrandName, "id": updateBrandID, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") {
                    ShowSuccess(obj);
                } else {
                    $(obj).parent().before(ShowAlert("danger", data));
                }
            } else {
                ToPage("login");
            }
            BtnAble(obj, true, "提交");
        });
    }
    return false;
}