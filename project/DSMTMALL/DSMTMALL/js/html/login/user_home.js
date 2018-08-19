
//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divChangePhonePage").on("pagehide", "#divChangePhonePage", function () {
    $("[id=divChangePhonePage]").remove();
});


//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divUserHomePage").on("pagehide", "#divUserHomePage", function () {
    $("[id=divUserHomePage]").remove();
});


//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divChangeNickNamePage").on("pagehide", "#divChangeNickNamePage", function () {
    $("[id=divChangeNickNamePage]").remove();
});

