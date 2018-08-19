/* divPageGoodsList */
$(document).off("pagebeforeshow", "#divPageGoodsList").on("pagebeforeshow", "#divPageGoodsList", function () {
    var currentPage = $(this);
    ActiveFooter(currentPage, 2);//使页脚显示被选样式
    var goodsType = $.trim(GetUrlParam(window.location.search, "goodstype"));//获取url中删选类型
    var searchName = $.trim(GetUrlParam(window.location.search, "searchname"));//获取url中删选类型
    var goodsSort = 0;
    var pageNow = 1;
    var pageSize = 16;
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "getByPageGoodsList" };
    if ($.trim(currentPage.find("#ulGoodsList").data("exist")) == "") {
        currentPage.find("#ulGoodsList").empty();
        currentPage.find("#ulGoodsList").data("sort", "0");
        currentPage.find("#ulGoodsList").data("page", "1");
        currentPage.find("#ulGoodsList").data("exist", "exist");
        args = { "jsonType": "getByPageGoodsList", "type": goodsType,"name":searchName, "sort": goodsSort, "pageNow": pageNow, "pageSize": pageSize, "createTime": new Date() };
        $.post(url, args, function (data) {
            var jsonData = eval(data);
            var jsonCount = 0;
            if (ValIsDefine(jsonData)) {
                $.each(jsonData, function (key, val) {
                    if (Number(key) % 2 == 0) {
                        currentPage.find("#ulGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    } else {
                        currentPage.find("#ulGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    }
                    jsonCount += 1;
                });
                currentPage.find("#ulGoodsList").data("page", Number($.trim(currentPage.find("#ulGoodsList").data("page"))) + 1);
            } else {
                ShowNoRecord(currentPage, "非常抱歉！暂无该类别商品");
            }
            currentPage.find("#ulGoodsList").data("count", jsonCount);
        });
    } else {
        goodsSort = $.trim(currentPage.find("#ulGoodsList").data("sort"));
        pageNow = $.trim(currentPage.find("#ulGoodsList").data("page"));
    }
    //类别删选更改
    function DrinkSortChange() {
        ShowLoader("");
        currentPage.find("#btnSortSaleDown").attr("style", "font-size:16px;text-shadow:none;background:#f2f2f2");
        currentPage.find("#btnSortPriceUp").attr("style", "font-size:16px;text-shadow:none;background:#f2f2f2");
        currentPage.find("#btnSortPriceDown").attr("style", "font-size:16px;text-shadow:none;background:#f2f2f2");
        if ($.trim(currentPage.find("#ulGoodsList").data("sort")) == "0") {
            currentPage.find("#btnSortSaleDown").eq(0).attr("style", "font-size:16px;text-shadow:none;background-color:#ffffff");
        } else if ($.trim(currentPage.find("#ulGoodsList").data("sort")) == "1") {
            currentPage.find("#btnSortPriceUp").eq(0).attr("style", "font-size:16px;text-shadow:none;background-color:#ffffff");
        } else {
            currentPage.find("#btnSortPriceDown").eq(0).attr("style", "font-size:16px;text-shadow:none;background-color:#ffffff");
        }
        currentPage.find("#ulGoodsList").empty();
        HideNoRecord(currentPage);
        currentPage.find("#ulGoodsList").data("page", "1");
        goodsSort = $.trim(currentPage.find("#ulGoodsList").data("sort"));
        pageNow = $.trim(currentPage.find("#ulGoodsList").data("page"));
        args = { "jsonType": "getByPageGoodsList", "type": goodsType, "name": searchName, "sort": goodsSort, "pageNow": pageNow, "pageSize": pageSize, "createTime": new Date() };
        $.post(url, args, function (data) {
            var jsonData = eval(data);
            var jsonCount = 0;
            if (jsonData != "") {
                $.each(jsonData, function (key, val) {
                    if (Number(key) % 2 == 0) {
                        currentPage.find("#ulGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    } else {
                        currentPage.find("#ulGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    }
                    jsonCount += 1;
                });
                currentPage.find("#ulGoodsList").data("page", Number($.trim(currentPage.find("#ulGoodsList").data("page"))) + 1);
            } else {
                ShowNoRecord(currentPage, "非常抱歉！暂无该类别商品");
            }
            currentPage.find("#ulGoodsList").data("count", jsonCount);
            HideLoader();
        });
    }
    //排序方式更改
    currentPage.find("#btnSortSaleDown").off("tap").on("tap", function () {
        currentPage.find("#ulGoodsList").data("sort", "0");
        DrinkSortChange();
        return false;
    });
    //排序方式更改
    currentPage.find("#btnSortPriceUp").off("tap").on("tap", function () {
        currentPage.find("#ulGoodsList").data("sort", "1");
        DrinkSortChange();
        return false;
    });
    //排序方式更改
    currentPage.find("#btnSortPriceDown").off("tap").on("tap", function () {
        currentPage.find("#ulGoodsList").data("sort", "2");
        DrinkSortChange();
        return false;
    });

    //屏幕滚动事件
    $(document).off("scrollstop").on("scrollstop", function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height() && currentPage.find("#ulGoodsList").data("count") >= pageSize) {
            //商品数大于等于当前设置的8条数据
            ShowLoader("努力加载中...");
            goodsSort = $.trim(currentPage.find("#ulGoodsList").data("sort"));
            pageNow = $.trim(currentPage.find("#ulGoodsList").data("page"));
            args = { "jsonType": "getByPageGoodsList", "type": goodsType, "name": searchName, "sort": goodsSort, "pageNow": pageNow, "pageSize": pageSize, "createTime": new Date() };
            $.ajax({
                type: "post",
                url: url,
                data: args,
                dataType: "json",
                timeout: 3000,
                async: true,
                success: function (data) {
                    var jsonCount = currentPage.find("#ulGoodsList").data("count");
                    if (data != "") {
                        $.each(data, function (key, val) {
                            if (Number(key) % 2 == 0) {
                                currentPage.find("#ulGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"],val["GoodsNumber"]));
                            } else {
                                currentPage.find("#ulGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                            }
                            jsonCount += 1;
                        });
                        currentPage.find("#ulGoodsList").data("page", Number($.trim(currentPage.find("#ulGoodsList").data("page"))) + 1);
                    } else {
                        ShowNoRecord(currentPage, "已到底");
                    }
                    currentPage.find("#ulGoodsList").data("count", jsonCount);
                    HideLoader();
                },
                error: function () { ShowErrLoader(); }
            });

            //$.post(url, args, function (data) {
            //    var jsonData = eval(data);
            //    var jsonCount = currentPage.find("#ulGoodsList").data("count");
            //    if (jsonData != "") {
            //        $.each(jsonData, function (key, val) {
            //            if (Number(key) % 2 == 0) {
            //                currentPage.find("#ulGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"]));
            //            } else {
            //                currentPage.find("#ulGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"]));
            //            }
            //            jsonCount += 1;
            //        });
            //        currentPage.find("#ulGoodsList").data("page", Number($.trim(currentPage.find("#ulGoodsList").data("page"))) + 1);
            //    } else {
            //        ShowNoRecord(currentPage, "已到底");
            //    }
            //    currentPage.find("#ulGoodsList").data("count", jsonCount);
            //    HideLoader();
            //});
        }
    });
    //搜索框功能
    currentPage.find("#myform").bind("submit", function () {
        searchName = $(this).find("input").eq(0).val();
        currentPage.find("#ulGoodsList").empty();
        currentPage.find("#ulGoodsList").data("sort", "0");
        currentPage.find("#ulGoodsList").data("page", "1");
        currentPage.find("#ulGoodsList").data("exist", "exist");
        args = { "jsonType": "getByPageGoodsList", "type": goodsType, "name": searchName, "sort": goodsSort, "pageNow": pageNow, "pageSize": pageSize, "createTime": new Date() };
        $.post(url, args, function (data) {
            var jsonData = eval(data);
            var jsonCount = 0;
            if (ValIsDefine(jsonData)) {
                $.each(jsonData, function (key, val) {
                    if (Number(key) % 2 == 0) {
                        currentPage.find("#ulGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    } else {
                        currentPage.find("#ulGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    }
                    jsonCount += 1;
                });
                currentPage.find("#ulGoodsList").data("page", Number($.trim(currentPage.find("#ulGoodsList").data("page"))) + 1);
            } else {
                ShowNoRecord(currentPage, "非常抱歉！暂无该类别商品");
            }
            currentPage.find("#ulGoodsList").data("count", jsonCount);
        });
        return false;
    });
});

//离开页面的时候清楚当前页的屏幕滚动事件
$(document).off("pagebeforehide", "#divPageGoodsList").on("pagebeforehide", "#divPageGoodsList", function () {
    $(document).off("scrollstop");//滚动事件
});

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divPageGoodsList").on("pagehide", "#divPageGoodsList", function () {
    if ($.trim(window.location.pathname) != "/web/goods_show.aspx") {
        $("[id=divPageGoodsList]").remove();
    }
});