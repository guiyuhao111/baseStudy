$(document).off("pagebeforeshow", "#divHomePage").on("pagebeforeshow", "#divHomePage", function () {
    var currentPage = $(this);
    var url = "/web/ajax.aspx";
    var userSysID = $.trim(currentPage.find("#userID").html());
    if (userSysID.length <= 0) {
        $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
    }
    ActiveFooter(currentPage, 4);//使页脚显示被选样式

    args = { "jsonType": "getNewRecomGoodsList", "createTime": new Date() };
    $.post(url, args, function (data) {
        currentPage.find("#gridNewGoodsList").empty();
        var dataArr = eval(data);
        if (ValIsDefine(dataArr)) {
            $.each(dataArr, function (key, val) {
                if (Number(key) % 2 == 0) {
                    currentPage.find("#gridNewGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                } else {
                    currentPage.find("#gridNewGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                }
            });
        }
    });
});

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divHomePage").on("pagehide", "#divHomePage", function () {
    if ($.trim(window.location.pathname) != "/web/goods_show.aspx") {
        $("[id=divHomePage]").remove();
    }
});