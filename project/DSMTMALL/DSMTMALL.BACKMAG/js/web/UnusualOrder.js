$(function () {

    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#payInfoModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#showInfo").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
        $("#orderProductList").empty();
    });
});
function ShowPayInfo(obj) {
    var trTable = $(obj).parents("tr").eq(0).find("[name=tPayInfo]").eq(0);
    $("[name=pPayName]").html($.trim(trTable.find("span").eq(0).html()));
    $("[name=pTradeNo]").html($.trim(trTable.find("span").eq(1).html()));
    $("[name=pMoneyPaid]").html($.trim(trTable.find("span").eq(2).html()));
    $("[name=pCpyName]").html($.trim(trTable.find("span").eq(3).html()));
    $("[name=pPayTime]").html($.trim(trTable.find("span").eq(4).html()));
    $("[name=pStatus]").html($.trim(trTable.find("span").eq(5).html()));
}

function ShowInfo(obj) {
    var trTable= $(obj).parents("tr").eq(0);
    var orderStatus = $.trim(trTable.find("[name=tStatus_1]").eq(0).attr("mydata"));
    var payStatus = $.trim(trTable.find("[name=tStatus_2]").eq(0).attr("mydata"));
    if (orderStatus == "7") {
        $("[name=pUnusual]").html("订单异常");
        $("[name=pDescribe]").html("查询该订单-海淘信息异常，请检查当订单的日志文件");
    } else if (payStatus == "9") {
        $("[name=pUnusual]").html("支付异常");
        $("[name=pDescribe]").html("该订单-XF系统返回的支付结果异常，1.检查XF系统查看该订单是否已被支付过2.查看该订单的日志文件");
    } else if(orderStatus!="7" && payStatus!="9") {
        $("[name=pUnusual]").html("订单支付已超过1小时，但系统没有同步到XF系统返回的支付状态信息");
        $("[name=pDescribe]").html("该订单-XF系统返回的支付结果同步异常，1.检查与XF系统的接口是否正常2.查看该订单的日志文件");
    }
    $("[name=pPostscript]").html($.trim(trTable.find("[name=tStatus_1]").find("a").attr("data-content")))
}