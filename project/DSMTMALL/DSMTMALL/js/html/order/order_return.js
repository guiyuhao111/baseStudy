$(document).off("pagebeforeshow", "#devOrderReturnPage").on("pagebeforeshow", "#devOrderReturnPage", function () {
    var currentPage = $(this);
    var url = "/web/ajax.aspx";
    var type = $.trim(GetUrlParam(window.location.search, "type"));//获取url中删选类型
    //alert(type);
    if (type == "dealsuccess") {
        currentPage.find(".header-back").eq(0).find("h4").html("交易完成");
        currentPage.find("#imgShow").eq(0).attr("src", "../../images/web/hpage/dealsuccess.png")
    } else if (type == "cancelOrder") {
        currentPage.find(".header-back").eq(0).find("h4").html("取消成功");
        currentPage.find("#imgShow").eq(0).attr("src", "../../images/web/hpage/cancelsuccess.png")
    } else if (type == "paySuccess") {
        currentPage.find(".header-back").eq(0).find("h4").html("付款完成");
        currentPage.find("#imgShow").eq(0).attr("src", "../../images/web/hpage/paysuccess.png")
    }
});



//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#devOrderReturnPage").on("pagehide", "#devOrderReturnPage", function () {
    $("[id=devOrderReturnPage]").remove();
});