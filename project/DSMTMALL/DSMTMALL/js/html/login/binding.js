$(document).off("pagebeforeshow", "#devBindingPage").on("pagebeforeshow", "#devBindingPage", function () {
    var currentPage = $(this);
    var url = "/web/ajax.aspx";
    currentPage.find("[name=new]").attr("style", "background-color:#ffffff;");
    currentPage.find("#toSubmitCode").off("tap").on("tap", function () {
        currentPage.focus();
        var count = currentPage.find("#toSubmitCode").attr("mydata");
        if (count == "0") {
            ShowNoteLoader("提交中");
            var telephoneNo = currentPage.find("#telephoneNo").attr("mydata");
            var openID = currentPage.find("#uOpenID").html();
            var telephoneNo_1 = currentPage.find("#telephoneNo").val();
            if (openID.length <= 0) {
                currentPage.find("#showTelInfo").html("系统忙，请稍后再试").removeAttr("hidden");//失败
                setTimeout(function () { currentPage.find("#showTelInfo").html(""); }, 2000);
            } else {
                var args = { "jsonType": "toSubmitCode", "phone": telephoneNo, "number": telephoneNo_1, "id": openID, "createTime": new Date() };
                $.ajax({
                    type: "post",
                    url: url,
                    data: args,
                    dataType: "text",
                    timeout: 3000,
                    async: true,
                    success: function (data) {
                        if (data != "UNLOGIN") {
                            if (data == "SUCCESS") {//成功
                                codeTimeCountSelf(currentPage.find("#toSubmitCode"));
                                currentPage.find("#toSubmitCode").attr("mydata", "1");
                            } else {
                                ShowNoteLoader("提交失败");
                                currentPage.find("#showTelInfo").html(data).removeAttr("hidden");//失败
                                setTimeout(function () { currentPage.find("#showTelInfo").html(""); }, 2000);
                            }
                        } else {
                            $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
                        }
                    },
                    error: function () {  ShowNoteLoader("发送失败"); }
                });
            }
        } else {
            return false;
        }
    });
    currentPage.find("#toSubmitBinding").off("tap").on("tap", function () {
        currentPage.focus();
        var telephoneNo = currentPage.find("#telephoneNo").attr("mydata");//判断如果没有的话
        var openID = currentPage.find("#uOpenID").html();//获取用户的OPenID
        var telephoneNo_1 = currentPage.find("#telephoneNo").val();
        var code = currentPage.find("#telephoneCode").val();
        var jsonInfo = currentPage.find("#telephoneUserName").attr("mydata");
        if (code.length <= 0 || telephoneNo_1.length <= 0 || openID.length<=0) {
            currentPage.find("#showTelInfo").html("手机号或验证码为空").removeAttr("hidden");//失败
        }  else {
            var args = { "jsonType": "toSubmitBindingInfo", "phone": telephoneNo, "number": telephoneNo_1, "id": openID, "code": code, "remark": jsonInfo, "createTime": new Date() };
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {//成功
                        $.mobile.changePage("/web/mall_Index.aspx", { transition: "slidefade" });
                    } else {
                        currentPage.find("#showTelInfo").html(data).removeAttr("hidden");//失败
                    }
                } else {
                    $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
                }
            });
        }
        return false;
    });

});

//离开页面的时候删除当前页内容
$(document).off("pagehide", "#devBindingPage").on("pagehide", "#devBindingPage", function () {
    $("[id=devBindingPage]").remove();
});

