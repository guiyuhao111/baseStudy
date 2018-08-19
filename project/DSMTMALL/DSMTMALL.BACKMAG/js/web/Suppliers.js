$(function () {

    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#addModel").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#updateModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#tableSuppliersList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tIsShow]").eq(0).attr("mydata")) == "1") {
            $(this).find("[name=tIsShow]").eq(0).find("[name=isCheckEnable]").attr("checked", "checked");
        }
    });
    $("#tableSuppliersList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tOrderBy]").eq(0).find("span").eq(0).html()).length<=0) {
            $(this).find("[name=tOrderBy]").eq(0).find("span").eq(0).html("暂无");
        }
    });
});
//品牌信息添加
function AddSuppliers(obj) {
    var newSupplierName = $.trim($("#newSuppliersName").val());
    var newSuppliersNo = $.trim($("#newSuppliersNo").val());
    if (ValIsDefine(newSupplierName) && ValIsDefine(newSuppliersNo)) {
        BtnAble(obj, false, "正在提交");
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "addSuppliers", "number": newSuppliersNo, "name": newSupplierName, "createTime": new Date() };
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
    else {
        ShowError(obj, "提交的数据不正确");
    }
    return false;
}