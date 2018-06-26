/* divPageDrinkList */
$(document).off("pagebeforeshow", "#divPageIndex").on("pagebeforeshow", "#divPageIndex", function () {//页面载入前执行
    var currentPage = $(this);
    ActiveFooter(currentPage, 1);//使页脚显示被选样式
   
    var pageNow = 1;
    var pageSize = 8;
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "getGoodsList" };
    args = { "jsonType": "getCpyGoodsList", "createTime": new Date() };
    $.post(url, args, function (data) {
        currentPage.find("#cpyTopGoodsList").empty();
        var dataArr = eval(data);
        if (ValIsDefine(dataArr)) {
            var arrCount = dataArr.length;
            var rem = Number(arrCount) % 2;//余数
            currentPage.find("#divCpyGoodsList").removeAttr("hidden");
            //if (arrCount > 1) { currentPage.find("#divCpyGoodsList").removeAttr("hidden"); }
            $.each(dataArr, function (key, val) {
                if (arrCount >= 1 || (arrCount <= 1 && rem == 0)) {
                    if (Number(key) % 2 == 0) {
                        currentPage.find("#cpyTopGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    } else {
                        currentPage.find("#cpyTopGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    }
                }
                arrCount--;
            });
        }
    });
    //载入默认的首页商品信息
    if ($.trim(currentPage.find("#gridTopGoodsList").data("exist")) == "") {//是否定义exist
        currentPage.find("#gridTopGoodsList").empty();
        currentPage.find("#gridTopGoodsList").data("page", "1");
        currentPage.find("#gridTopGoodsList").data("exist", "exist");
        args = { "jsonType": "getGoodsList", "pageNow": pageNow, "pageSize": pageSize, "createTime": new Date() };
        $.post(url, args, function (data) {
            var jsonData = eval(data);
            var jsonCount = 0;
            if(ValIsDefine(jsonData)){
                $.each(jsonData, function (key, val) {
                    if (Number(key) % 2 == 0) {
                        currentPage.find("#gridTopGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    } else {
                        currentPage.find("#gridTopGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                    }
                    jsonCount += 1;
                });
                currentPage.find("#gridTopGoodsList").data("page", Number($.trim(currentPage.find("#gridTopGoodsList").data("page"))) + 1);
               
            } else {
                ShowNoRecord(currentPage, "非常抱歉！暂无热销推荐");
            }
            currentPage.find("#gridTopGoodsList").data("count", jsonCount);
            HideLoader();
        });
    } else {
        pageNow = $.trim(currentPage.find("#gridTopGoodsList").data("page"));
    }
   
    //屏幕滚动事件
    $(document).off("scrollstop").on("scrollstop", function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height() && Number(pageNow) < 6 && currentPage.find("#gridTopGoodsList").data("count") >= pageSize) {
            // 页码小于6 即只需要刷新6次并且 商品数大于等于当前设置的8条数据
            ShowLoader("努力加载中...");
            pageNow = $.trim(currentPage.find("#gridTopGoodsList").data("page"));
            pageSize = 16;
            args = { "jsonType": "getGoodsList", "pageNow": pageNow, "pageSize": pageSize, "createTime": new Date() };
            $.ajax({
                type: "post",
                url: url,
                data: args,
                dataType: "json",
                timeout: 3000,
                async:true,
                success: function (data) {
                    var jsonCount = currentPage.find("#gridTopGoodsList").data("count");
                    if (data != "") {
                        $.each(data, function (key, val) {
                            if (Number(key) % 2 == 0) {
                                currentPage.find("#gridTopGoodsList").append(GetTopGoodsList("a", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                            } else {
                                currentPage.find("#gridTopGoodsList").append(GetTopGoodsList("b", val["GoodsID"], val["NGoodsImg"], val["GoodsName"], val["ShopPrice"], val["MarketPrice"], val["GoodsNumber"]));
                            }
                            jsonCount += 1;
                        });
                        currentPage.find("#gridTopGoodsList").data("page", Number($.trim(currentPage.find("#gridTopGoodsList").data("page"))) + 1);
                    } else {
                        ShowNoRecord(currentPage, "已到底");
                    }
                    currentPage.find("#gridTopGoodsList").data("count", jsonCount);
                    HideLoader();
                },
                error: function () { ShowErrLoader(); }
            });
        }
    });
    //搜索框功能
    Search(currentPage);
});


//主要是为了实现轮播图
$(document).off("pageshow", "#divPageIndex").on("pageshow", "#divPageIndex", function () {
    var currentPage = $(this);
    if (currentPage.find("#gridTopGoodsList").html() == "" && currentPage.find("#divNoRecord").is(":hidden")) {
        //如果产品显示页尾空并且存在divNoRecord的显示提示块则显示努力加载中。。。
        ShowLoader("");
    }
    currentPage.find(".flexslider").flexslider({//轮播图初始化设置
        animation: "slide", animationLoop: "true", animationSpeed: 7000, slideshowSpeed: 5000, startAt: 0,
    });
    currentPage.find(".carousel-pic").fadeIn(1000);
    currentPage.find("#goodsTopListShow").fadeIn(1000);
});

//离开页面的时候清楚当前页的屏幕滚动事件
$(document).off("pagebeforehide", "#divPageIndex").on("pagebeforehide", "#divPageIndex", function () {
    $(document).off("scrollstop");//滚动事件
});

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divPageIndex").on("pagehide", "#divPageIndex", function () {
    if ($.trim(window.location.pathname) != "/web/goods_show.aspx") {
        $("[id=divPageIndex]").remove();
    }
});