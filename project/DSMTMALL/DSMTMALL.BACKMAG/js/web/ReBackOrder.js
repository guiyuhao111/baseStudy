$(function () {
    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#showInfo").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#followUpModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#refundModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });

});


function ShowInfo(obj) {
    var trTable = $(obj).parents("tr").eq(0);
    var trTable_1 = $(obj).parents("tr").eq(0).find("[name=tPayInfo]").eq(0);
    var trTable_2 = $(obj).parents("tr").eq(0).find("[name=tShippingInfo]").eq(0);
    var trTable_3 = $(obj).parents("tr").eq(0).find("[name=tOrderInfo]").eq(0);
    $("#showInfo").data("id", $(obj).parents("td").eq(0).attr("mydata"));
    $("#showInfo").attr("mydata", $.trim($(obj).parents("tr").eq(0).find("[name=tStatus]").eq(0).attr("mydata")));
    $("[name=pPayName]").html($.trim(trTable_1.find("span").eq(0).html()));
    $("[name=pTradeNo]").html($.trim(trTable_1.find("span").eq(1).html()));
    $("[name=pMoneyPaid]").html($.trim(trTable_1.find("span").eq(2).html()));
    $("[name=pCpyName]").html($.trim(trTable_1.find("span").eq(3).html()));
    $("[name=pPayTime]").html($.trim(trTable_1.find("span").eq(4).html()));
    $("[name=pStatus]").html($.trim(trTable_1.find("span").eq(5).html()));
    //订单
    $("[name=pOrderNo]").html($.trim(trTable_3.find("span").eq(0).html()));
    $("[name=pOrderstatus]").html($.trim(trTable_3.find("span").eq(1).html()));
    $("[name=pSuppliers]").html($.trim(trTable_3.find("span").eq(2).html()));
    $("[name=pOrderAmount]").html($.trim(trTable_3.find("span").eq(3).html()));
    $("[name=pCreatTime]").html($.trim(trTable_3.find("span").eq(4).html()));
    $("[name=pReBackType]").html($.trim(trTable.find("[name=tType]").eq(0).html()));
    $("[name=pAuthType]").html($.trim(trTable.find("[name=tStatus]").eq(0).html()));
    $("[name=pConnection]").html($.trim(trTable_3.find("span").eq(5).html()));
    $("[name=pBackAddress]").html($.trim(trTable_3.find("span").eq(6).html()));
    $("[name=pBackLogistical]").html($.trim(trTable_3.find("span").eq(7).html()));
    $("[name=pBackLogisticalNumber]").html($.trim(trTable_3.find("span").eq(8).html()));
    $("[name=pRemark]").html($.trim(trTable_3.find("span").eq(9).html()));
    //物流
    $("[name=pShippingStatus]").html($.trim(trTable.find("[name=tStatus_3]").eq(0).html()));
    $("[name=pLogistical]").html($.trim(trTable_2.find("span").eq(4).html()));
    $("[name=pLogisticalNumber]").html($.trim(trTable_2.find("span").eq(6).html()));
    $("[name=pShippingTime]").html($.trim(trTable_2.find("span").eq(5).html()));
    $("[name=pConsignee]").html($.trim(trTable_2.find("span").eq(0).html()));
    $("[name=pConsigneeCardNo]").html($.trim(trTable_2.find("span").eq(3).html()));
    $("[name=pAddress]").html($.trim(trTable_2.find("span").eq(1).html()));
    $("[name=pPhone]").html($.trim(trTable_2.find("span").eq(2).html()));
    $("[name=pShippingFee]").html($.trim(trTable_2.find("span").eq(7).html()));
    $("#reBackAddress").val("浙江宁波高新区软件园区A座410室");
    $("#reBackRemark").val($.trim(trTable_3.find("span").eq(10).html()));
    var sysID = $.trim(trTable.find("[name=tOperation]").attr("mydata"));
    var htmlStr = "";
    if (sysID.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：数据出错，请重新登陆"));
    } else {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": "reBackOrderGoods", "id": sysID, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            $("#orderProductList").html("");
            if (data != "UNLOGIN") {
                var jsonData = eval(data);
                var jsonCount = 0;
                if (jsonData != "") {
                    $.each(jsonData, function (key, val) {
                        htmlStr = "";
                        htmlStr += " <div><h3>产品信息</h3> <p>商品编号：<span>" + val["GoodsSn"] + "</span></p>";
                        htmlStr += " <p>商品名称：<span>" + val["GoodsName"] + "</span></p><p>商品来源：<span>" + val["SuppliersName"] + "</span></p>";
                        htmlStr += " <p>购买数量：<span>" + val["BuyNumber"] + "</span></p><p>退货数量：<span>" + val["RebackNumber"] + "</span></p></div>";
                        htmlStr += " <p style=\"color:red\">商品价格：<span>" + val["GoodsPrice"] + "</span></p> ";
                        //htmlStr += " <p>商品备注：<span><a href=\"" + val["SellerNote"] + "\"  target=\"view_window\" >" + val["SellerNote"] + "</a></span></p>"
                        htmlStr += "<p>产品图册：<span><img style=\"width:50px;height:50px;\" src='" + val["GoodsThumb"] + "' alt='" + val["GoodsName"] + "'></span></p></div>";
                        $("#orderProductList").append(htmlStr);
                    });
                }
            } else {
                ToPage("login");
            }
        });
    }
    return false;
}


function ShowFollowUp(obj) {
    $("#reBackGoodsRemark").text($.trim($(obj).parents("tr").eq(0).find("[name=tOrderInfo]").eq(0).find("span").eq(10).html()));
    $("#followUpModal").data("id", $(obj).parents("td").eq(0).attr("mydata"));
    $("#followUpModal").attr("mydata", $.trim($(obj).parents("tr").eq(0).find("[name=tStatus]").eq(0).attr("mydata")));
}

function ConfirmRebackInfo(obj) {
    var sysID =$.trim( $("#showInfo").data("id"));
    var rebackAddress =$.trim($("#reBackAddress").val());
    var reBackIsPass = $.trim($("#reBackIsPass").val());
    var orderStatus =$.trim( $("#showInfo").attr("mydata"));
    var remark = $("#reBackRemark").val();
    if (orderStatus == "10") {
        if (ValIsDefine(sysID) && ValIsDefine(rebackAddress) && ValIsDefine(reBackIsPass)) {
            var url = "/web/web_ajax.aspx";
            var args = {
                "jsonType": "reBackOrderIsPass", "id": sysID, "url": rebackAddress,"type": reBackIsPass,"remark":remark, "createTime": new Date()
            };
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {
                        ShowSuccess(obj);
                    } else {
                        ShowError(obj, data);
                    }
                } else {
                    ToPage("login");
                }
            });
        } else {
            ShowError(obj, "提交的数据不正确");
        }
    } else {
        ShowError(obj, "该记录已被处理");
    }
}

function ConfirmReBackGoods(obj){
    var sysID = $.trim($("#followUpModal").data("id"));
    var remark = $("#reBackGoodsRemark").val();
    var type = $.trim($("#reBackGoodsIsOK").val());
    var orderStatus = $.trim($("#followUpModal").attr("mydata"));
    if (orderStatus == "20") {
        if (ValIsDefine(sysID) && ValIsDefine(remark)) {
            var url = "/web/web_ajax.aspx";
            var args = {
                "jsonType": "reBackOrderGoodsIsOK", "id": sysID,"remark": remark,"type":type, "createTime": new Date()
            };
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {
                        ShowSuccess(obj);
                    } else {
                        ShowError(obj, data);
                    }
                } else {
                    ToPage("login");
                }
            });
        } else {
            ShowError(obj, "提交的数据不正确");
        }
    } else {
        ShowError(obj, "该记录已被处理");
    }
}

function ShowRefund(obj) {
    $("#emGooodsAount").html($.trim($(obj).parents("tr").eq(0).find("[name=tPayInfo]").eq(0).find("span").eq(2).html()));
    $("#refundModal").data("id", $(obj).parents("td").eq(0).attr("mydata"));
    $("#refundModal").attr("mydata", $.trim($(obj).parents("tr").eq(0).find("[name=tStatus]").eq(0).attr("mydata")));
}

function ConfirmReBackMoney(obj) {
    var sysID = $.trim($("#refundModal").data("id"));
    var orderStatus = $.trim($("#refundModal").attr("mydata"));
    var money = $.trim($("#rebackMoney").val());
    if (orderStatus == "30") {
        if (ValIsDefine(sysID) && ValIsDefine(money)) {
            var url = "/web/web_ajax.aspx";
            var args = {
                "jsonType": "reBackOrderMoney", "id": sysID,"number":money,  "createTime": new Date()
            };
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {
                        ShowSuccess(obj);
                    } else {
                        ShowError(obj,data);
                    }
                } else {
                    ToPage("login");
                }
            });
        } else {
            ShowError(obj, "提交的数据不正确");
        }
    } else {
        ShowError(obj, "请确认当前退款订单可以执行退款操作");
    }
}
