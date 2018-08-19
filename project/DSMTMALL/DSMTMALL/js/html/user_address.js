$(document).off("pagebeforeshow", "#divAddressPage").on("pagebeforeshow", "#divAddressPage", function () {
    var currentPage = $(this);
    var url = "/web/ajax.aspx";
    var userSysID = $.trim(currentPage.find("#userID").html());
    if (userSysID.length <= 0) {
        $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
    } else {
        var args = { "jsonType": "getUserAddressList", "createTime": new Date() };
        $.post(url, args, function (data) {
            var jsonData = eval(data);
            var jsonCount = 0;
            if (jsonData != "") {
                $.each(jsonData, function (key, val) {
                    currentPage.find("#ulUserAddressList").append(GetUserAddressList(val["Consignee"], val["Mobile"], val["CountryName"], val["ProvinceName"], val["CityName"], val["DistrictName"], val["Address"], val["AddressID"]));
                });
            } else {
                ShowNoRecord(currentPage, "非常抱歉！暂无可用地址");
            }
        });
    };
});


//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divAddressPage").on("pagehide", "#divAddressPage", function () {
   $("[id=divAddressPage]").remove();
});