//页面加载时执行的方法
$(document).off("pagebeforeshow", "#devLoginPage").on("pagebeforeshow", "#devLoginPage", function () {
    var currentPage = $(this);
    $("#loginPhone").focus();
    currentPage.find("#btnSubmitLogin").off("tap").on("tap", function () {
        currentPage.focus();
        var phone = $.trim(currentPage.find("#loginPhone").val());
        var pwd = $.trim(currentPage.find("#loginpwd").val());
        if (phone.length <= 0) {
            ShowPromptBar(currentPage, "用户名不能为空");
            $("#loginPhone").focus();
        } else if (pwd.length <= 0) {
            ShowPromptBar(currentPage, "密码不能为空");
            $("#loginpwd").focus();
        } else {
            var url = "/web/ajax.aspx";
            args = { "jsonType": "logining", "name": phone, "pwd": pwd, "createTime": new Date() };
            $.post(url, args, function (data) {
                if (data != "SUCCESS") {
                    ShowPromptBar(currentPage, data);
                    $("#loginPhone").focus();
                } else {
                    //alert(document.referrer);
                    //window.history.back();
                    $.mobile.changePage("/web/user_homepage.aspx", { transition: "slidefade" });//跳转到固定页
                }
            });
        }
    });
});

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#devLoginPage").on("pagehide", "#devLoginPage", function () {
     $("[id=devLoginPage]").remove();
});