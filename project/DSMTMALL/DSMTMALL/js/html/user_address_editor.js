/* divPageUserAddressAdd */
$(document).off("pagebeforeshow", "#divAddressEditorPage").on("pagebeforeshow", "#divAddressEditorPage", function () {
    var currentPage = $(this);
    var addressID = $.trim(GetUrlParam(window.location.search, "id"));
    if (addressID.length > 0) {
        var url = "/web/ajax.aspx";
        var args = { "jsonType": "getThisAddress", "id": addressID, "createTime": new Date() };
        $.post(url, args, function (data) {
            var jsonData = eval('['+data+']');
            var jsonCount = 0;
            if (jsonData != "") {
                LoadProvince($(this).find("#addressManage"), jsonData[0]["Province"], jsonData[0]["City"], jsonData[0]["District"]);
                currentPage.find("#tbUserAddress").val(jsonData[0]["Address"])
                currentPage.find("#tbUserNickname").val(jsonData[0]["Consignee"]);
                currentPage.find("#tbUserPhone").val(jsonData[0]["Mobile"]);
                currentPage.find("#tbUserCardNo").val(jsonData[0]["ConsigneeCardNo"]);
            }
            HideLoader();
        });
    } else {
        LoadProvince($(this).find("#addressManage"), "", "","");
    }
    currentPage.find("#btnAddAddress").off("tap").on("tap", function () {
        currentPage.focus();
        if (currentPage.find("#btnAddAddress").attr("mydata_ok") != "1") {
            ShowNoteLoader("提交中");
            currentPage.find("#btnAddAddress").attr("mydata_ok", "1");
            var ddlUserProvince = $.trim(currentPage.find("#ddlUserProvince").val());
            var ddlUserCity = $.trim(currentPage.find("#ddlUserCity").val());
            var ddlUserDistrict = $.trim(currentPage.find("#ddlUserDistrict").val());
            var userAddress = $.trim(currentPage.find("#tbUserAddress").val());
            var userNickname = $.trim(currentPage.find("#tbUserNickname").val());
            var userPhone = $.trim(currentPage.find("#tbUserPhone").val());
            var cardNo = $.trim(currentPage.find("#tbUserCardNo").val());
            if (ddlUserProvince.length <= 0 || ddlUserCity.length <= 0 || ddlUserDistrict.length <= 0) {
                ShowPromptBar(currentPage, "请选择收货地区");
                currentPage.find("#btnAddAddress").attr("mydata_ok", "0");
            } else if (userAddress.length <= 0) {
                ShowPromptBar(currentPage, "详细地址不能为空");
                currentPage.find("#btnAddAddress").attr("mydata_ok", "0");
            } else if (userNickname.length <= 0) {
                ShowPromptBar(currentPage, "收货人姓名不能为空");
                currentPage.find("#btnAddAddress").attr("mydata_ok", "0");
            } else if (userPhone.length <= 0) {
                ShowPromptBar(currentPage, "联系方式不能为空");
                currentPage.find("#btnAddAddress").attr("mydata_ok", "0");
            } else if (!userPhone.match(/^(((1[0-9]{2}))+\d{8})$/)) {
                ShowPromptBar(currentPage, "手机号格式不合法");
                currentPage.find("#btnAddAddress").attr("mydata_ok", "0");
            } else if (cardNo.length <= 0 || !isCardNo(cardNo)) {
                ShowPromptBar(currentPage, "请确认输入的身份证号码与收货人姓名一致有效");
                currentPage.find("#btnAddAddress").attr("mydata_ok", "0");
            }
            else {
                HidePromptBar();
                ShowLoader("正在保存");
                var url = "/web/ajax.aspx";
                if (addressID.length <= 0) {
                    var args = { "jsonType": "addUserAddress", "id_1": ddlUserProvince, "id_2": ddlUserCity, "id_3": ddlUserDistrict, "address": userAddress, "name": userNickname, "number": userPhone, "cardNo": cardNo, "createTime": new Date() };
                } else {
                    var args = { "jsonType": "updateUserAddress", "id": addressID, "id_1": ddlUserProvince, "id_2": ddlUserCity, "id_3": ddlUserDistrict, "address": userAddress, "name": userNickname, "number": userPhone, "cardNo": cardNo, "createTime": new Date() };
                }
                $.post(url, args, function (data) {
                    if (data != "UNLOGIN") {
                        if (data == "SUCCESS") {
                            ShowPromptBar(currentPage, "操作成功");
                            history.go(-1);
                        } else {
                            ShowPromptBar(currentPage, data);
                            currentPage.find("#btnAddAddress").attr("mydata_ok", "0");
                        }
                    } else {
                        $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
                    }
                    HideLoader();
                });
            }
        }
        return false;
    });

    currentPage.find("#btnDeleteAddress").off("tap").on("tap", function () {
    currentPage.find("#btnShowboxLeft").attr("onclick", "btnDeleteAddressOk_Click('" + addressID + "')");
    ShowShowbox(currentPage, "failure", "确定要删除吗？", "#6699cc", "是", "否", "#", "slidefade");

    });

});

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divAddressEditorPage").on("pagehide", "#divAddressEditorPage", function () {
     $("[id=divAddressEditorPage]").remove();
});
