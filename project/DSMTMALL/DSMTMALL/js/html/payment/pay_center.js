$(document).off("pagebeforeshow", "#devPayCenterPage").on("pagebeforeshow", "#devPayCenterPage", function () {
    var currentPage = $(this);
    var url = "/web/ajax.aspx";

    //发送短信验证码
    currentPage.find("#toSubmitCode").off("tap").on("tap", function () {
        currentPage.focus();
        var count = currentPage.find("#toSubmitCode").attr("mydata");
        if (count == "0") {
            ShowNoteLoader("提交中");
            var args = { "jsonType": "toSetOrderCode",  "createTime": new Date() };
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {//成功
                        currentPage.find("#toSubmitCode").attr("mydata", "1");
                        codeTimeCountSelf(currentPage.find("#toSubmitCode"));
                    } else {
                        ShowPromptBar(currentPage, data);
                    }
                } else {
                    $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
                }
            });
        } else {
            return;
        }
    });

    //提交卡余额支付
    currentPage.find("#toSubmitCardBanlance").off("tap").on("tap", function () {
        currentPage.focus();
        if (currentPage.find("#toSubmitCardBanlance").attr("mydata_1") != "1") {
            ShowNoteLoader("提交中");
            currentPage.find("#toSubmitCardBanlance").attr("mydata_1", "1");
            var orderID = $.trim(currentPage.find("#toSubmitCardBanlance").attr("mydata"));//订单号
            var orderMoeny = $.trim(currentPage.find("#emCartOrderAmount").html());//总金额
            var orderCode = $.trim(currentPage.find("#telCode").val());//验证码
            var cartBanlance = $.trim(currentPage.find("#emCartBanlance").html());//可用余额
            if (orderID.length <= 0 || orderMoeny.length <= 0) {
                ShowPromptBar(currentPage, "数据出错，请刷新当前页");
                currentPage.find("#toSubmitCardBanlance").attr("mydata_1", "0");
            } else if (orderCode.length <= 0) {
                ShowPromptBar(currentPage, "请输入短信验证码");
                currentPage.find("#toSubmitCardBanlance").attr("mydata_1", "0");
            } else if (Number(orderMoeny) > Number(cartBanlance)) {
                ShowPromptBar(currentPage, "卡内余额不足，无法使用卡余额消费");
                currentPage.find("#toSubmitCardBanlance").attr("mydata_1", "0");
            }
            else {
                args = { "jsonType": "toSubmitCardBanlance", "id": orderID, "number": orderMoeny, "code": orderCode, "createTime": new Date() };
                $.post(url, args, function (data) {
                    if (data != "UNLOGIN") {
                        if (data == "SUCCESS") {
                            $.mobile.changePage("/web/order/order_return.aspx?type=paySuccess", { transition: "slidefade" });
                        } else {
                            ShowPromptBar(currentPage, data);
                            currentPage.find("#toSubmitCardBanlance").attr("mydata_1", "0");
                        }
                    } else {
                        $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
                    }
                });
            }
        }
        return false;
    });


});



//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#devPayCenterPage").on("pagehide", "#devPayCenterPage", function () {
      $("[id=devUserCartPage]").remove();
});