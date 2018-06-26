$(document).off("pagebeforeshow", "#divCartList").on("pagebeforeshow", "#divCartList", function () {
    var currentPage = $(this);
    var userSysID = currentPage.find("#userID").html();
    if (userSysID.length <= 0) {
        $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
    } else {
        var url = "/web/ajax.aspx";
        args = { "jsonType": "getUserCartList", "id": userSysID, "createTime": new Date() };
        $.post(url, args, function (data) {
            var jsonData = eval(data);
            if (jsonData != 0) {
                $.each(jsonData, function (key, val) {
                    currentPage.find("#ulUserCart").append(GetUserCartGoods(val["BuyNumber"], val["CartID"], val["GoodsName"], val["NGoodsImg"], val["ShopPrice"], val["MarketPrice"], val["SaleNumber"], val["GoodsID"], val["IsDelete"], val["IsEnable"], val["GoodsNumber"], val["QuotaNumber"]));
                });
                currentPage.find("[name=cart-li]").each(function () {
                    $(this).off("swipeleft").on("swipeleft", function () {
                        $(this).attr("style", "-webkit-transition:all 500ms ease-in-out;-moz-transition:all 500ms ease-in-out");
                        $(this).css('-webkit-transform', 'translate(-50px,0px)').stop(1, 1);
                    });
                    $(this).off("swiperight").on("swiperight", function () {
                        $(this).attr("style", "-webkit-transition:all 500ms ease-in-out;-moz-transition:all 500ms ease-in-out");
                        $(this).css('-webkit-transform', 'translate(0px,0px)').stop(1, 1);
                    });
                });
                var cartTotalPrice = 0;
                currentPage.find("#emCartTotalPrice").text(cartTotalPrice.toFixed(2));
                currentPage.find("[id^=btnToShowPage_]").data("change", "0");
                currentPage.find("[id^=tbNumber_]").data("change", "0");
            } else {
                ShowNoRecord(currentPage, "您的购物车为空");
                currentPage.find("#toSubmitOrder").attr("href","#");
            }
        });
    }

    ActiveFooter(currentPage, 3);//使页脚显示被选样式

    currentPage.find("#toSubmitOrder").off("tap").on("tap", function () {
        var cartlistStr = "";
        currentPage.find("[name=layout]").each(function () {
            if ($(this).eq(0).is(':checked')) {
                cartlistStr += $(this).val()+"/";
            }
        });
        if (cartlistStr.length > 0) {
            $.cookie("goodsCartStr", cartlistStr);
            $.mobile.changePage("/web/user_cart.aspx?", { transition: "slidefade" });
        }
    });

});

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divCartList").on("pagehide", "#divCartList", function () {
    $("[id=divCartList]").remove();
});