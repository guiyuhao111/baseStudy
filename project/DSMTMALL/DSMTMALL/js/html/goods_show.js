$(document).off("pagebeforeshow", "#divPageGoodsShow").on("pagebeforeshow", "#divPageGoodsShow", function () {
    var currentPage = $(this);
    //currentPage.find(".flexslider").removeData("flexslider");
    //currentPage.find(".flexslider").flexslider({//轮播图初始化设置
    //    animation: "slide",
    //});
    //currentPage.find(".flexslider").show();//是轮播图显示
    //商品详情页下拉显示更多信息
    currentPage.find("#spanShow").off("tap").on("tap", function () {
        currentPage.focus();
        if (currentPage.find("[id^=tableGoodsShow_tr_]").css("display") == "none") {
            currentPage.find("#spanShow").eq(0).find("img").attr("src","../../images/web/hpage/reduce.png");
            currentPage.find("[id^=tableGoodsShow_tr_]").show();
        } else {
            currentPage.find("#spanShow").eq(0).find("img").attr("src", "../../images/web/hpage/add.png");
            currentPage.find("[id^=tableGoodsShow_tr_]").hide();
        }
    });

    //商品数量加一
    currentPage.find("#btnReduce").off("tap").on("tap", function () {
        currentPage.focus();
        var numberValue = $.trim(currentPage.find("#tbNumber").val());
        if (Number(numberValue) > 1) {
            currentPage.find("#tbNumber").val(Number(numberValue) - 1);
            currentPage.find("#btnAdd").removeAttr("disabled");
            numberValue = currentPage.find("#tbNumber").val();
            $.tipsBox({
                obj: $(this),
                str: "-1",
                callback: function () {
                }
            });
        }
        if (numberValue <= 1) {
            $(this).attr("disabled", "true");
        }
        return false;
    });
    //商品数量减一
    currentPage.find("#btnAdd").off("tap").on("tap", function () {
        currentPage.focus();
        var numberValue = $.trim(currentPage.find("#tbNumber").val());
        if (Number(numberValue) < 99) {
            currentPage.find("#tbNumber").val(Number(numberValue) + 1);
            currentPage.find("#btnReduce").removeAttr("disabled");
            numberValue = currentPage.find("#tbNumber").val();
            $.tipsBox({
                obj: $(this),
                str: "+1",
                callback: function () {
                }
            });
        }
        if (numberValue >= 99) {
            $(this).attr("disabled", "true");
        }
        return false;
    });
    //商品加入购物车
    currentPage.find("#btnAddToCart").off("tap").on("tap", function () {
        currentPage.focus();
        var goodsID = $.trim(currentPage.find("#goodsId").val());
        var tbNumber = $.trim(currentPage.find("#tbNumber").val());
        var quotaNumber = $.trim(currentPage.find("#quotaNumber").attr("mydata"));
        quotaNumber = quotaNumber.length > 0 ? Number(quotaNumber) : 0;
        if ((quotaNumber <= 0)||(quotaNumber > 0 && tbNumber <= quotaNumber)) {//如果限购次数大于0并且提交数量小于限购次数
            var url = "/web/ajax.aspx";
            args = { "jsonType": "addGoodsToCart", "id": goodsID, "number": tbNumber, "createTime": new Date() };
            $.post(url, args, function (data) {
                if (data == "UNLOGIN") {
                    $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
                } else {
                    if (data.indexOf("ERROR") >= 0) {
                        ShowPromptBar(currentPage,data);
                    } else {
                        ShowSuccessLoader("成功加入购物车");//ShowPromptBar(currentPage, "成功加入购物车");
                    }
                }
            });
        } else {
            ShowNoteLoader("该商品的限购数量为"+quotaNumber+"件");
        }
    });

});

//主要是为了实现轮播图//页脚的被选样式
$(document).off("pageshow", "#divPageGoodsShow").on("pageshow", "#divPageGoodsShow", function () {
    var currentPage = $(this);
    if (currentPage.find("#divPageGoodsShow").html() == "" && currentPage.find("#divNoRecord").is(":hidden")) {
        //如果产品显示页尾空并且存在divNoRecord的显示提示块则显示努力加载中。。。
        ShowLoader("");
    }
    currentPage.find(".flexslider").flexslider({//轮播图初始化设置
        animation: "slide",
    });
    currentPage.find("#tableGoodsShow").fadeIn(1000);
});


//离开页面的时候清楚当前页的屏幕滚动事件
$(document).off("pagebeforehide", "#divPageGoodsShow").on("pagebeforehide", "#divPageGoodsShow", function () {
    $(document).off("scrollstop");//滚动事件
    var currentPage = $(this);
    //currentPage.find(".flexslider").removeData("flexslider");

});

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divPageGoodsShow").on("pagehide", "#divPageGoodsShow", function () {
        $("[id=divPageGoodsShow]").remove();
});

