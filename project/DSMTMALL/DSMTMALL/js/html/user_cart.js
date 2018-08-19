$(document).off("pagebeforeshow", "#devUserCartPage").on("pagebeforeshow", "#devUserCartPage", function () {
    var currentPage = $(this);
    var goodsCartStr = $.cookie("goodsCartStr");
    var defaultAddress = $.cookie("defaultAddress");
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "getUserAddress","id":defaultAddress, "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data != "UNLOGION") {
            var arr = data.split("|");
            if (arr.length < 5) {
                currentPage.find("#ulUserAddress").append(GetUserCollectionData(data));
            } else {
                currentPage.find("#ulUserAddress").append(GetUserCollectionAddress(arr[1], arr[2], arr[3])).attr("mydata", arr[0]).attr("provienceID", arr[4]);
                currentPage.find("#ulMoreAddress").removeAttr("hidden");
            }
        } else {
            $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
        }

        if (goodsCartStr.length <= 0) {
            currentPage.find("#emCartFeeAmount").text("0.00");
            currentPage.find("#emCartGoodsPrice").text("0.00");
            currentPage.find("#divFeeAmount").hide();
            ShowNoRecord(currentPage, "您的购物车为空");
        } else {
            args = { "jsonType": "getUserCartListToSubmit", "sort": goodsCartStr, "id": arr[4], "createTime": new Date() };
            $.post(url, args, function (data) {
                var dataArr = data.split("&");
                var jsonData = eval(dataArr[0]);
                if (jsonData != 0) {
                    $.each(jsonData, function (key, val) {
                        currentPage.find("#ulUserCart").append(GetUserCollectionDrink(val["BuyNumber"], val["CartID"], val["GoodsName"], val["NGoodsImg"], val["ShopPrice"], val["MarketPrice"], val["SaleNumber"], val["GoodsID"]));
                    });
                    var cartTotalPrice = 0;
                    currentPage.find("[id^=bSubtotalPrice_]").each(function () {
                        cartTotalPrice += Number($.trim($(this).text()));
                    });
                    currentPage.find("#emCartGoodsPrice").text(cartTotalPrice.toFixed(2));
                    var feeAmount = ValIsDefine(dataArr[1]) ? dataArr[1] : 1000;
                    currentPage.find("#emCartFeeAmount").text(Number(feeAmount).toFixed(2));
                    cartTotalPrice += Number(feeAmount);
                    currentPage.find("#emCartTotalPrice").text(cartTotalPrice.toFixed(2));
                    currentPage.find("[id^=btnToShowPage_]").data("change", "0");
                    currentPage.find("[id^=tbNumber_]").data("change", "0");
                } else {
                    currentPage.find("#emCartFeeAmount").text("0.00");
                    currentPage.find("#emCartGoodsPrice").text("0.00");
                    currentPage.find("#divFeeAmount").hide();
                    ShowNoRecord(currentPage, "您的购物车为空");

                }
            });
        }
    });

    //获取地址
    currentPage.find("#ulMoreAddress").off("tap").on("tap", function () {
        $.mobile.changePage("/web/user_address.aspx", { transition: "slidefade" });
    });

    currentPage.find("#toSubmitOrder").off("tap").on("tap", function () {
        currentPage.focus();
        if (currentPage.find("#toSubmitOrder").attr("mydata_ok") != "1") {
            currentPage.find("#toSubmitOrder").attr("mydata_ok", "1");
            var cartlistStr = "";
            var userAddressID = $.trim(currentPage.find("#ulUserAddress").attr("mydata"));
            var userPay = $.trim(currentPage.find("#emCartTotalPrice").html());
            var userShipping = "2";
            var userPayment = "20";
            if (currentPage.find("[id^=liCollectionDrinkList_]").length <= 0 || goodsCartStr.length <= 0) {
                ShowPromptBar(currentPage, "购物车为空");
                currentPage.find("#toSubmitOrder").attr("mydata_ok", "0");
            } else if (userAddressID.length <= 0) {
                ShowPromptBar(currentPage, "请添加收货地址");
                currentPage.find("#toSubmitOrder").attr("mydata_ok", "0");
            } else {
                HidePromptBar();
                ShowLoader("订单提交中...");
                var url = "/web/ajax.aspx";
                var args = { "jsonType": "addUserOrder", "address": userAddressID, "number": userPay, "id_1": userShipping, "id_2": userPayment, "sort": goodsCartStr, "createTime": new Date() };
                $.post(url, args, function (data) {
                    var dataArr = $.trim(data).split(",");
                    if (dataArr[0] == "UNLOGIN") {
                        $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
                    } else if (dataArr[0] == "SUCCESS") {
                        $.cookie("goodsCartStr", null);
                        $.mobile.changePage("/web/payment/pay_center.aspx?OrderUnifySn=" + dataArr[1], { transition: "slidefade" });
                        currentPage.find("#ulUserCart").empty();
                        currentPage.find("#divCartUserInfo").hide();
                        ShowNoRecord(currentPage, "您的购物车为空");
                        currentPage.find("#emCartTotalPrice").text("0.00");
                        currentPage.find("#emCartFeeAmount").text("0.00");
                        currentPage.find("#emCartGoodsPrice").text("0.00");
                        currentPage.find("#divFeeAmount").hide();
                    } else {
                        ShowPromptBar(currentPage, data);
                        setTimeout("window.location.reload()", 2000);//刷新当前页面.
                    }
                    HideLoader();
                });
            }
        }
        return false;
    });

});



//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#devUserCartPage").on("pagehide", "#devUserCartPage", function () {
       $("[id=devUserCartPage]").remove();
});