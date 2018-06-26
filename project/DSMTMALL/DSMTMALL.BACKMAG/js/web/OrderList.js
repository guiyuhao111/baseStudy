$(function () {

    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#payInfoModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#reBackModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#showInfo").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
        $("#orderProductList").empty();
    });
    $("#deliveryModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });

    $("#tableOrderList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        var orderStatus = $.trim($(this).find("[name=tStatus_1]").eq(0).attr("mydata"));
        var payStatus = $.trim($(this).find("[name=tStatus_2]").eq(0).attr("mydata"));
        var shippingStatus = $.trim($(this).find("[name=tStatus_3]").eq(0).html());
        if (shippingStatus == "已发货" || shippingStatus=="已收货") {
            $(this).addClass("success");
        } else if (orderStatus == "1" && payStatus == "2" && shippingStatus=="未发货") {
            $(this).addClass("warning");
        } else if (orderStatus == "6") {
            $(this).addClass("danger");
        } else if (orderStatus == "8" && shippingStatus == "已完成") {
            $(this).addClass("info");
        }
    });
});

function ShowPayInfo(obj) {
    var trTable = $(obj).parents("tr").eq(0).find("[name=tPayInfo]").eq(0);
    $("[name=pPayName]").html($.trim(trTable.find("span").eq(0).html()));
    $("[name=pTradeNo]").html($.trim(trTable.find("span").eq(1).html()));
    $("[name=pMoneyPaid]").html($.trim(trTable.find("span").eq(2).html()));
    $("[name=pCpyName]").html($.trim(trTable.find("span").eq(3).html()));
    $("[name=pPayTime]").html($.trim(trTable.find("span").eq(4).html()));
    $("[name=pStatus]").html($.trim(trTable.find("span").eq(5).html()));
}

function ShowInfo(obj) {
    var trTable = $(obj).parents("tr").eq(0);
    var trTable_1 = $(obj).parents("tr").eq(0).find("[name=tPayInfo]").eq(0);
    var trTable_2 = $(obj).parents("tr").eq(0).find("[name=tShippingInfo]").eq(0);
    $("[name=pPayName]").html($.trim(trTable_1.find("span").eq(0).html()));
    $("[name=pTradeNo]").html($.trim(trTable_1.find("span").eq(1).html()));
    $("[name=pMoneyPaid]").html($.trim(trTable_1.find("span").eq(2).html()));
    $("[name=pCpyName]").html($.trim(trTable_1.find("span").eq(3).html()));
    $("[name=pPayTime]").html($.trim(trTable_1.find("span").eq(4).html()));
    $("[name=pStatus]").html($.trim(trTable_1.find("span").eq(5).html()));
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
    var sysID = $.trim(trTable.find("[name=tOperation]").attr("mydata"));
    var htmlStr = "";
    if (sysID.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：数据出错，请重新登陆"));
    } else {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": "orderGoods", "id": sysID, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                var jsonData = eval(data);
                var jsonCount = 0;
                if (jsonData != "") {
                    $.each(jsonData, function (key, val) {
                        htmlStr = "";
                        htmlStr += " <div><h3>产品信息</h3> <p>商品编号：<span>" + val["GoodsSn"] + "</span></p>";
                        htmlStr += " <p>商品名称：<span>" + val["GoodsName"] + "</span></p><p>商品来源：<span>" + val["SuppliersName"] + "</span></p>";
                        htmlStr += " <p>库存数量：<span>" + val["GoodsNumber"] + "</span></p><p>购买数量：<span>" + val["BuyNumber"] + "</span></p></div>";
                        htmlStr += " <p style=\"color:red\">商品价格：<span>" + val["GoodsPrice"] + "</span></p> ";
                        htmlStr += " <p>商品备注：<span><a href=\"" + val["SellerNote"] + "\"  target=\"view_window\" >" + val["SellerNote"] + "</a></span></p>"
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

function ShowDelivery(obj) {
    var trTable = $(obj).parents("tr").eq(0);
    var trTable_2 = $(obj).parents("tr").eq(0).find("[name=tShippingInfo]").eq(0);
    $("[name=pShippingStatus]").html($.trim(trTable.find("[name=tStatus_3]").eq(0).html()));
    $("[name=pShippingTime]").html($.trim(trTable_2.find("span").eq(5).html()));
    $("[name=pConsignee]").html($.trim(trTable_2.find("span").eq(0).html()));
    $("[name=pConsigneeCardNo]").html($.trim(trTable_2.find("span").eq(3).html()));
    $("[name=pAddress]").html($.trim(trTable_2.find("span").eq(1).html()));
    $("[name=pPhone]").html($.trim(trTable_2.find("span").eq(2).html()));
    $("[name=pShippingFee]").html($.trim(trTable_2.find("span").eq(7).html()));
    $("#deliveryModal").data("id", $.trim($(obj).parents("tr").eq(0).find("[name=tID]").html()));
    $("#logisticalName").val($.trim(trTable_2.find("span").eq(4).html()));
    $("#logisticalNumber").val($.trim(trTable_2.find("span").eq(6).html()));
    $("[name=pLogisticalName]").html($.trim(trTable_2.find("span").eq(4).html()));
    $("[name=pLogisticalNumber]").html($.trim(trTable_2.find("span").eq(6).html()));
    $("#remark").text($.trim(trTable_2.find("span").eq(8).html()));
}

function ConfirmDelivery(obj) {
    var html = $(obj).html();
    if (CheckedComButton(obj)) {
        var logisticalName = $.trim($("#logisticalName").val());
        var logisticalNumber = $.trim($("#logisticalNumber").val());
        var logisticalRemark = $.trim($("#remark").val());
        var orderID = $.trim($("#deliveryModal").data("id"));
        if (logisticalName.length <= 0 || logisticalNumber.length <= 0 || orderID.length <= 0) {
            UnCheckedComButton(obj, html, "数据不正确", 12);//ShowError(obj, "提交的数据不正确");
        } else {
            BtnAbled(obj);
            var url = "/web/web_ajax.aspx";
            var args = { "jsonType": "confirmDelivery", "id": orderID, "name": logisticalName, "number": logisticalNumber, "remark": logisticalRemark, "createTime": new Date() };
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {
                        ShowSuccess(obj);
                        UnCheckedComBtnBack(obj, html);
                    } else {
                        ShowError(obj, data);
                        UnCheckedComBtnBack(obj, html);
                    }
                } else {
                    ToPage("login");
                }
            });
        }
    }
    return false;
}

//导出当前订单信息EXCEL
function ExportExcel(obj) {
    var byTimeStart = $.trim($("#sltByTimeStart").val());
    var byTimeEnd = $.trim($("#sltByTimeEnd").val());
    var suppliersID = $.trim($("#sltBySuppliers").val());
    var orderStatus = $.trim($("#sltByOrderStatus").val());
    var payStatus = $.trim($("#sltByPayStatus").val());
    var payID = $.trim($("#sltByPayID").val());
    var shippingStatus = $.trim($("#sltByShippingStatus").val());
    var searchName = $.trim($("#btnSearch").val());
    var sltOrderBy = $.trim($("#sltOrderBy").val());
    if (sltOrderBy == "0") {
        devSort = " ORDER BY O.PayTime DESC ";
    }
    if (sltOrderBy == "1") {
        devSort = " ORDER BY O.AddTime DESC ";
    }
    if (sltOrderBy == "2") {
        devSort = " ORDER BY O.UpdateTime DESC ";
    }
    var urlStr = "byTimeStart=" + byTimeStart + "&byTimeEnd=" + byTimeEnd + "&suppliersID=" + suppliersID + "&orderStatus=" + orderStatus + "&payStatus=" + payStatus + "&payID=" + payID + "&shippingStatus=" + shippingStatus + "&searchName=" + searchName + "&orderBy=" + devSort;
    var url = "/web/web_ajax.aspx";
    var args = { "jsonType": "exportOrderInfo", "url": urlStr, "creatTime": new Date() };
    $.post(url, args, function (data) {
        if (data != "UNLOGIN") {
            window.location.href = data;
        } else {
            ToPage("login");
        }
        BtnAble(obj, true, "提交");
    });
    return false;
}


function ThirdShowInfo(obj) {
    var trTable = $(obj).parents("tr").eq(0);
    var trTable_1 = $(obj).parents("tr").eq(0).find("[name=tPayInfo]").eq(0);
    var trTable_2 = $(obj).parents("tr").eq(0).find("[name=tShippingInfo]").eq(0);
    $("[name=pPayName]").html($.trim(trTable_1.find("span").eq(0).html()));
    $("[name=pTradeNo]").html($.trim(trTable_1.find("span").eq(1).html()));
    $("[name=pMoneyPaid]").html($.trim(trTable_1.find("span").eq(2).html()));
    $("[name=pCpyName]").html($.trim(trTable_1.find("span").eq(3).html()));
    $("[name=pPayTime]").html($.trim(trTable_1.find("span").eq(4).html()));
    $("[name=pStatus]").html($.trim(trTable_1.find("span").eq(5).html()));
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
    $("[name=Remark]").html($.trim(trTable_2.find("span").eq(8).val()));
    var sysID = $.trim(trTable.find("[name=tOperation]").attr("mydata"));
    var htmlStr = "";
    if (sysID.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：数据出错，请重新登陆"));
    } else {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": "orderGoods", "id": sysID, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                var jsonData = eval(data);
                var jsonCount = 0;
                if (jsonData != "") {
                    $.each(jsonData, function (key, val) {
                        htmlStr = "";
                        htmlStr += " <div><h3>产品信息</h3> <p>商品编号：<span>" + val["GoodsSn"] + "</span></p>";
                        htmlStr += " <p>商品名称：<span>" + val["GoodsName"] + "</span></p><p>商品来源：<span>" + val["SuppliersName"] + "</span></p>";
                        htmlStr += " <p>库存数量：<span>" + val["GoodsNumber"] + "</span></p><p>购买数量：<span>" + val["BuyNumber"] + "</span></p></div>";
                        htmlStr += " <p style=\"color:red\">商品价格：<span>" + val["GoodsPrice"] + "</span></p> ";
                        htmlStr += " <p>商品备注：<span>" + val["SellerNote"] + "</span></p>"
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

function ShowRebackInfo(obj) {
    var trTable = $(obj).parents("tr").eq(0);
    var trTable_1 = $(obj).parents("tr").eq(0).find("[name=tPayInfo]").eq(0);
    var trTable_2 = $(obj).parents("tr").eq(0).find("[name=tShippingInfo]").eq(0);
    $("[name=pPayName]").html($.trim(trTable_1.find("span").eq(0).html()));
    $("[name=pTradeNo]").html($.trim(trTable_1.find("span").eq(1).html()));
    $("[name=pMoneyPaid]").html($.trim(trTable_1.find("span").eq(2).html()));
    $("[name=pCpyName]").html($.trim(trTable_1.find("span").eq(3).html()));
    $("[name=pPayTime]").html($.trim(trTable_1.find("span").eq(4).html()));
    $("[name=pStatus]").html($.trim(trTable_1.find("span").eq(5).html()));
    $("#reBackModal").data("id", $.trim($(obj).parents("tr").eq(0).find("[name=tID]").html()));
    var sysID = $.trim(trTable.find("[name=tOperation]").attr("mydata"));
    var htmlStr = "";
    if (sysID.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：数据出错，请重新登陆"));
    } else {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": "orderGoods", "id": sysID, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                $("#orderProductList_back").html("").show();
                var jsonData = eval(data);
                var jsonCount = 0;
                if (jsonData != "") {
                    $.each(jsonData, function (key, val) {
                        $("#orderProductList_back").append(AppendReBackGoodsList(val["GoodsThumb"], val["BuyNumber"], val["GoodsSn"], val["GoodsName"], val["GoodsPrice"], val["GoodsID"]));
                    });
                }
            } else {
                ToPage("login");
            }
        });
    }
    return false;
}


//进行退货商品信息拼接
function AppendReBackGoodsList(imgUrl, count, goodsSn, goodsName, goodsPrice, goodsID) {
    var html = " <li class=\"list-group-item\"><span><input type=\"checkbox\" name=\"slt_back\"/></span> <span><img src=\""+imgUrl+"\"/></span> <div>";
    html += "<h4>" + goodsSn + "-" + goodsName + "-&yen" + goodsPrice + "</h4>";
    html+="<span>购买数量:<strong>"+count+"</strong>件</span><span><input type=\"number\" mydata=\""+goodsID+"\" class=\"form-control\" name=\"reback_goods_Num\"  placeholder=\"请输入退货数量...\"  /></span>";
    html += "</div></li>";
    return html;
}

function ConfirmReBackGoods(obj) {
    var sltGoodsInfo = "";
    var html = $(obj).html();
    var isTrue = true;
    var orderSysID= $("#reBackModal").data("id");
    if (CheckedComButton(obj)) {
        $.each($("[name=slt_back]"), function () {
            if ($(this).prop("checked") == true) {
                var id = $(this).parents("li").eq(0).find("[name=reback_goods_Num]").attr("mydata");
                var number = $(this).parents("li").eq(0).find("[name=reback_goods_Num]").val();
                if (ValIsDefine(id) && ValIsDefine(number)) {
                    sltGoodsInfo += sltGoodsInfo.length <= 0 ? "{" + "GoodsID:'" + id + "',GoodsNum:'" + number + "'}" : ",{" + "GoodsID:'" + id + "',GoodsNum:'" + number + "'}";
                    
                } else {
                    alert("凡是勾选的商品退回商品数量不能为空");
                    isTrue = false;
                }
            }
        });
        if (isTrue) {
            var phone = $("#telPhone_back").val();
            var type = $("#type_back").val();
            var remark = $("#remark_back").val();
            if (ValIsDefine(sltGoodsInfo) && ValIsDefine(phone) && ValIsDefine(type) && ValIsDefine(remark)) {
                BtnAbled(obj);
                var url = "/web/web_ajax.aspx";
                var args = {
                    "jsonType": "rebackGoods", "id": "[" + sltGoodsInfo + "]", "number": phone, "type": type, "remark": remark,"cid":orderSysID, "createTime": new Date()
                };
                $.post(url, args, function (data) {
                    if (data != "UNLOGIN") {
                        if (data == "SUCCESS") {
                            ShowSuccess(data);
                        } else {
                            ShowError(obj, data); UnCheckedComBtnBack(obj, html);
                        }
                    } else {
                        ToPage("login");
                    }
                });
            } else {
                UnCheckedComButton(obj, html, "数据不正确");
            }
        } else {
            UnCheckedComBtnBack(obj, html);
        }
    }
    return false;
}