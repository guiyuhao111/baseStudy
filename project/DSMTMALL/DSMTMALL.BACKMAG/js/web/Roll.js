$(function () {

    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#tableRollList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tIsEnable]").eq(0).attr("mydata")) == "1") {
            $(this).find("[name=tIsEnable]").eq(0).find("[name=isCheckEnable]").attr("checked", "checked");
        }
    });
});

function AddRoll(obj) {
    BtnAble(obj, false, "正在追加");
    var url = "/web/web_ajax.aspx";
    var args = { "jsonType": "addRoll", "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data != "UNLOGIN") {
            alert(data);
            ToPage("now");
        } else {
            ToPage("login");
        }
        BtnAble(obj, true, "一键追加");
    });
}

//更新轮播图绑定产品
function ShowUpdateRoll(obj) {
    $("#targetSysID").val($.trim($(obj).parents("tr").eq(0).find("[name=tName]").html()));
    $("#targetSysID").attr("mydata", $.trim($(obj).parents("tr").eq(0).find("[name=tName]").attr("mydata")));
    $("#updateModel").data("id", $.trim($(obj).parents("tr").eq(0).find("[name=tID]").html()))
}
function UpdateRoll(obj) {
    var sysID = $.trim($("#targetSysID").attr("mydata"));
    var roolID = $.trim($("#updateModel").data("id"));
    var type = $.trim($("#targetType").val());
    if (type != "10" && !ValIsDefine(roolID)) {
        ShowError(obj, "系统出错，请选择轮播图要绑定的目标");
    }else if (!ValIsDefine(roolID) || !ValIsDefine(type)) {
        ShowError(obj, "系统出错，请重新登陆");
    }
    else {
        BtnAble(obj, false, "正在绑定中");
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "updateRoll","id":sysID,"cid":roolID,"type":type, "createTime": new Date() };
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
            BtnAble(obj, true, "一键追加");
        });
    }
    return false;
}

//更换轮播绑定类型的时候触发事件
function ChangeTargetType(obj) {
    var targetType = $(obj).val();
    $("#targetSysID").attr("oninput", "");
    $("#targetSysID").attr("mydata", "").val("");
    if (targetType == "10") {
        $("#targetSysID").attr("oninput", "ShowUlList(this,'searchAdvs')");
    }else if (targetType == "20") {
        $("#targetSysID").attr("oninput", "ShowUlList(this,'searchCates')");
    } else if(targetType=="30"){
        $("#targetSysID").attr("oninput", "ShowUlList(this,'searchGoods')");
    }
}