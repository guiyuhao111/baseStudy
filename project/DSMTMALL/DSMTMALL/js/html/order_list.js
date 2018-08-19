$(document).off("pagebeforeshow", "#divPageUserOrder").on("pagebeforeshow", "#divPageUserOrder", function () {
    var currentPage = $(this);
    var orderState = $.trim(GetUrlParam(window.location.search, "orderType"));//获取url中删选类型
    if (orderState == "TRS") {
        currentPage.find("#orderTypeName").html("待收货订单");
    } else if (orderState == "obligation") {
        currentPage.find("#orderTypeName").html("待付款订单");
    }
    var url = "/web/ajax.aspx";
    var pageNow = 1;
    var pageSize = 8;
    if ($.trim(currentPage.find("#ulUserOrderList").data("exist")) == "") {
        currentPage.find("#ulUserOrderList").empty();
        currentPage.find("#ulUserOrderList").data("page", "1");
        currentPage.find("#ulUserOrderList").data("exist", "exist");
        args = { "jsonType": "getUserOrderList", "type": orderState, "pageNow": pageNow, "pageSize": pageSize, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                var jsonData = eval(data);
                var jsonCount= 0;
                if (jsonData != "") {
                    $.each(jsonData, function (key, val) {
                        currentPage.find("#ulUserOrderList").append(GetUserOrder(val["OrderSn"], val["Consignee"], val["PayName"], val["Mobile"], val["NOrderAddress"], val["PayTime"], val["AddTime"], val["NOrderStatus"], val["OrderStatus"], val["NPayStatus"], val["PayStatus"], val["NShippingStatus"], val["OrderAmount"].toFixed(2), val["OrderID"], val["SuppliersName"], val["ShippingStatus"], val["ShippingFee"].toFixed(2), val["Logistical"], val["LogisticalNumber"]));
                        args = { "jsonType": "getUserOrderGoods", "id": val["OrderID"], "createTime": new Date() };
                        $.post(url, args, function (data) {
                            var jsonDataList = eval(data);
                            if (data != "UNLOGIN" && jsonDataList != "") {
                                $.each(jsonDataList, function (key, val) {
                                    currentPage.find("[id^=liOrder_" + val["OrderSn"] + "_top]").after(GetUserOrderGoods(val["OrderSn"], val["GoodsID"], val["GoodsSn"], val["GoodsPrice"], val["GoodsName"], val["NGoodsImg"], val["BuyNumber"], val["GoodsAttr"]));
                                });
                            }
                        });
                        jsonCount += 1;
                    });
                    currentPage.find("#ulUserOrderList").data("page",Number($.trim(currentPage.find("#ulUserOrderList").data("page")))+1);
                }else{
                    ShowNoRecord(currentPage,"您暂无历史订单");
                }
                currentPage.find("#ulUserOrderList").data("count",jsonCount);
                HideLoader();
            } else {
                $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
            }
        });
    } else {
        pageNow = $.trim(currentPage.find("#ulUserOrderList").data("page"));
    }

  
      
    //屏幕滚动事件
    $(document).off("scrollstop").on("scrollstop", function () {
        //alert($(window).scrollTop());//$(document).scrollTop() 获取垂直滚动的距离  即当前滚动的地方的窗口顶端到整个页面顶端的距离
        //alert($(document).height());//$(document).height()  //是获取整个页面的高度
        //alert($(window).height());//$(window).height()  //是获取当前 也就是你浏览器所能看到的页面的那部分的高度 
        //alert(currentPage.find("#ulUserOrderList").data("count"));
        if ($(window).scrollTop() >= $(document).height() - $(window).height() && currentPage.find("#ulUserOrderList").data("count") >= pageSize) {
            //商品数大于等于当前设置的8条数据
            //要获取底端 只要获取scrollTop()>=$(document).height()-$(window).height()  就可以知道已经滚动到底端了
            currentPage.focus();
            ShowLoader("努力加载中...");
            pageNow = $.trim(currentPage.find("#ulUserOrderList").data("page"));
            args = { "jsonType": "getUserOrderList", "type": orderState, "pageNow": pageNow, "pageSize": pageSize, "createTime": new Date() };
            $.ajax({
                type: "post",
                url: url,
                data: args,
                dataType: "json",
                timeout: 3000,
                async: true,
                success: function (data) {
                    if (data != "UNLOGIN") {
                        var jsonCount = currentPage.find("#ulUserOrderList").data("count");
                        if (data != "") {
                            $.each(data, function (key, val) {
                                currentPage.find("#ulUserOrderList").append(GetUserOrder(val["OrderSn"], val["Consignee"], val["PayName"], val["Mobile"], val["NOrderAddress"], val["PayTime"], val["AddTime"], val["NOrderStatus"], val["OrderStatus"], val["NPayStatus"], val["PayStatus"], val["NShippingStatus"], val["OrderAmount"], val["OrderID"], val["SuppliersName"], val["ShippingStatus"], val["ShippingFee"].toFixed(2), val["Logistical"], val["LogisticalNumber"]));
                                args = { "jsonType": "getUserOrderGoods", "id": val["OrderID"], "createTime": new Date() };
                                $.post(url, args, function (data) {
                                    var jsonDataList = eval(data);
                                    if (data != "UNLOGIN" && jsonDataList != "") {
                                        $.each(jsonDataList, function (key, val) {
                                            currentPage.find("[id^=liOrder_" + val["OrderSn"] + "_top]").after(GetUserOrderGoods(val["OrderSn"], val["GoodsID"], val["GoodsSn"], val["GoodsPrice"], val["GoodsName"], val["NGoodsImg"], val["BuyNumber"], val["GoodsAttr"]));
                                        });
                                    }
                                });
                                jsonCount += 1;
                            });
                            currentPage.find("#ulUserOrderList").data("page", Number($.trim(currentPage.find("#ulUserOrderList").data("page"))) + 1);
                        } else {
                            ShowNoRecord(currentPage, "已到底");
                        }
                        currentPage.find("#ulUserOrderList").data("count", jsonCount);
                        HideLoader();
                    } else {
                        $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
                    }
                },
                error: function () { ShowErrLoader(); }
            });

        }
    });
 
    ActiveFooter(currentPage, 4);//使页脚显示被选样式
});

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divPageUserOrder").on("pagehide", "#divPageUserOrder", function () {
     $("[id=divPageUserOrder]").remove();
});