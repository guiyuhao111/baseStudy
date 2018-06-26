/// <reference path="C:\DSMTMALL\DSMTMALL.BACKMAG\web/goods/goods_list.aspx" />
$(function () {
    $("#iptAdminName").focus();

    $("#registerAdmin").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });

    $("#registerAdmin").off("show.bs.modal").on("show.bs.modal", function (e) {
        LoadProvince($(this), "", "", "");
    });
    var wait = 60;
});
//按ENTER键登录系统
document.onkeydown = function (event) {
    var e = event || window.event || arguments.callee.caller.arguments[0];
    if (e && e.keyCode == 13) {
        AdminLogin($("#btnAdminLogin"));
    }
};
//用户登录系统事件
function AdminLogin(obj) {
    var html = $(obj).html();
    if (CheckedButton(obj)) {
        var checkCode = "ML_" + GetGuid();//获取标识码给读卡器
        var adminName = $.trim($("#iptAdminName").val());
        var adminPwd = $.trim($("#iptAdminPwd").val());
        if (ValIsDefine(adminName) && ValIsDefine(adminPwd)&& ValIsDefine(checkCode)) {
            var url = "/web/web_ajax.aspx";
            var args = { "jsonType": "adminLogin", "name": adminName, "pwd": adminPwd,"code":checkCode, "createTime": new Date() };
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {
                        $.cookie("ulMenu", null, { path: "/" });
                        $("#showImg").show();
                        $("#myLogin").hide();
                        window.location.href = "DSMTCard://" + checkCode;
                        var timerCount = 0;
                        args = { "jsonType": "checkAdminLogin", "code": checkCode, "createTime": new Date() };
                        $.ajax({
                            type: "post",
                            url: url,
                            data: args,
                            dataType: "text",
                            timeout: 0,
                            async: true,
                            success: function (data2) {
                                myArr = $.trim(data2).split(",");
                                if (myArr[0] == "SUCCESS") {
                                    window.location.href = ValIsDefine(myArr[1]) ? myArr[1] : "/web/goods/goods_list.aspx";
                                } else {
                                    $("#showImg").hide();
                                    $("#myLogin").show();
                                    UnCheckedButton(obj, html, myArr[1]);
                                }
                            },
                            error: function () {
                                $("#showImg").hide();
                                $("#myLogin").show();
                                UnCheckedButton(obj, html, "登陆失败");
                            }
                        });
                    } else {
                        UnCheckedButton(obj, html, data);
                    }
                } else {
                    ToPage("login");
                }
                $("#iptAdminPwd").focus();
                $("#iptAdminPwd").val("");

            });
        } else {
            UnCheckedButton(obj, html, "提交的数据不正确");
        }
    }
    return false;
}
