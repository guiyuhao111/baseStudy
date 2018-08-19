//更新密码事件
function UpdatePwd(obj) {
    var adminPwd = $.trim($("#adminPwd").val());
    var newPwd = $.trim($("#newPwd").val());
    var againPwd = $.trim($("#againPwd").val());
    if (adminPwd.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：旧密码不能为空"));
    } else if (newPwd.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：新密码不能为空"));
    } else if (againPwd.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：确认密码不能为空"));
    } else if (newPwd != againPwd) {
        $(obj).parent().before(ShowAlert("danger", "提示：两次输入的密码不一致"));
    } else {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "updateAdminPwd", "pwd": adminPwd, "newPwd": newPwd, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") {
                    $("#adminPwd").val("");
                    $("#newPwd").val("");
                    $("#againPwd").val("");
                    $(obj).parent().before(ShowAlert("success", "提示：密码修改成功"));
                } else {
                    $(obj).parent().before(ShowAlert("danger", data));
                }
            } else {
                ToPage("login");
            }
        });
    }
    return false;
}
//重置已经填写的密码内容
function ResetPwd() {
    $("#divAlert").remove();
}