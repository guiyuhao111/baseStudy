$(function () {

    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#assignModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#addModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#tableGoodsList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tNew]").eq(0).attr("mydata")) == "1") {
            $(this).find("[name=tNew]").eq(0).find("[name=isCheckNew]").attr("checked", "checked");
        }
    });
    $("#tableGoodsList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tIsEnable]").eq(0).attr("mydata")) == "1") {
            $(this).find("[name=tIsEnable]").eq(0).find("[name=isCheckEnable]").attr("checked", "checked");
        }
    });
    $("#tableGoodsList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
        if ($.trim($(this).find("[name=tIsPromote]").eq(0).attr("mydata")) == "1") {
            $(this).find("[name=tIsPromote]").eq(0).find("[name=isCheckPromote]").attr("checked", "checked");
        }
    });
});
//导出商品信息EXCEL
function ExportExcel(obj) {
    var type = $.trim($(obj).attr("mydata"));
    var byCateType = $.trim($("#sltByCateType").val());
    var bySuppliers =$.trim($("#sltBySuppliers").val());
    var byIsEnable =$.trim($("#sltByIsEnable").val());
    var bySearch = $.trim($("#btnSearch").val());
    var urlStr = "byCateType=" + byCateType + "&bySuppliers=" + bySuppliers + "&byIsEnable=" + byIsEnable + "&searchName=" + bySearch;
    if (type.length <= 0) {
        alert("err:系统出错，请重新登陆");
    } else {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "exportGoodsInfo", "type": type,"url":urlStr, "creatTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                window.location.href = data;
;            } else {
                ToPage("login");
            }
            BtnAble(obj, true, "提交");
        });
    }
    return false;
}
//显示可分配的公司信息
function AssignCpyGoods(obj) {
    var sysID= $(obj).attr("mydata");
    $("#assignModel").data("id",sysID);
    var url = "/web/web_ajax.aspx";
    var args = { "jsonType": "chooseCpyInfo", "creatTime": new Date() };
    $.post(url, args, function (data) {
        if (data != "UNLOGIN") {
            var jsonData = eval(data);
            var inner = "";
            if (jsonData != "") {
                $.each(jsonData, function (key, val) {
                    inner+="<div class=\"col-xs-4\"><div class=\"form-group\"><input name=\"sltCpyInfo\" type=\"checkbox\" value=\""+val["CpySysID"]+"\" /><span>"+val["CpyName"]+"</span></div></div>";
                });
                $("#chooseCpyInfo").html(inner);
            }
        } else {
            ToPage("login");
        }
        BtnAble(obj, true, "提交");
    });
    return false;
}

function AssignGoodsToCpy(obj) {
    var sysID =$.trim($("#assignModel").data("id"));
    var checkboxArr = $("[name=sltCpyInfo]");
    var charStr = "";
    for (var i = 0; i < checkboxArr.length; i++) {
        if (checkboxArr[i].checked == true) {
            charStr += $.trim(checkboxArr[i].value) + ",";
        }
    }
    if (sysID.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：系统出错请重新登陆"));
    } else if (charStr.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：请至少选择一家公司"));
    } else {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "assignCpyToGoods","id":sysID,"word":charStr, "creatTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") {
                    ShowSuccess($(obj).parent().before());
                } else {
                    $(obj).parent().before(ShowAlert("danger", data));
                }
            } else {
                ToPage("login");
            }
            BtnAble(obj, true, "分配");
        });
    }
}
//url跳转到详情编辑页
function EditorGoodsDetial(obj) {
    var sysID = $.trim($(obj).parents("tr").find("[name=tOperation]").eq(0).attr("mydata"));
    var url = "/web/goods/editor.aspx?byGoodsSysID=" + sysID;
    var thisUrl = window.location.href;
    $.cookie("editorUrl", thisUrl, { path: "/" });
    location.href = url;
}
//显示更改售价
function ShowChangePrice(obj) {
    $("#shopPrice").val($.trim($(obj).find("em").eq(0).html()));
    $("#marketPrice").val($.trim($(obj).find("em").eq(1).html()));
    $("#changePrice").data("id", $.trim($(obj).attr("mydata")))
}
function ChangePrice(obj) {
    var id = $("#changePrice").data("id");
    var shopPrice = $("#shopPrice").val();
    var marketPrice = $("#marketPrice").val();
    if (id.length <= 0 || shopPrice.length <= 0 || marketPrice.length <= 0) {
        ShowError(obj, "提交的数据不能正确");
    } else {
        BtnAble(obj, false, "正在绑定中");
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "changePrice", "id": id, "price_s": shopPrice, "price_m": marketPrice, "createTime": new Date() };
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
            BtnAble(obj, true, "确认");
        });
    }
    return false;
}
//添加新的商品-验证
function AddGoodsToValidate(obj) {
    var val = new validate({
        /*rules里面是检验规则， *key为需要检验的input的id, *value为此input对应的检验规则*/
        rules: {
            addGoodsName: "notEmpty",
            addGoodsNo: "notEmpty",
            addShopPrice: "float",
            addMarketPrice: "float",
            addWeight: "float",
            addInventory: "number"
        },
        /*submitFun里面为检验成功后要执行的方法*/
        submitFun: function () {
            AddNewGoods(obj);
        }
    });
    return false;
}
//添加新的商品-提交
function AddNewGoods(obj) {
    var goodsName = $.trim($("#addGoodsName").val());
    var goodsNo = $.trim($("#addGoodsNo").val());
    var shopPrice = $.trim($("#addShopPrice").val());
    var weight = $.trim($("#addWeight").val());
    var marketPrice = $.trim($("#addMarketPrice").val());
    var inventory = $.trim($("#addInventory").val());
    var supplierID = $.trim($("#addSuppliers").val());
    var fareTemp = $.trim($("#addFareTemp").val());
    var cateID = $.trim($("#addCateName").attr("mydata"));
    var brief = $.trim($("#addGoodsBrief").val());
    var remark = $.trim($("#addRemark").val());
    var brandID = $.trim($("#addBrand").attr("mydata"));
    if (ValIsDefine(goodsName) && ValIsDefine(goodsNo) && ValIsDefine(shopPrice) && ValIsDefine(marketPrice) && ValIsDefine(inventory) && ValIsDefine(supplierID) && ValIsDefine(fareTemp) && ValIsDefine(cateID)) {
        BtnAble(obj, false, "正在提交中");
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "addGoodsInfo", "name": goodsName,"cid":goodsNo, "price_s": shopPrice,"price_m":marketPrice, "number": weight,"count": inventory,"type":fareTemp,"typeB":brandID,"typeC":cateID,"typeT":supplierID,"word":brief,"remark":remark, "createTime": new Date() };
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
            BtnAble(obj, true, "确认");
        });
    } else {
        ShowError(obj, "提交的数据不正确");
    }
    return false;
}

function ShowAddNewGoods(obj) {
    var suppliers = $.trim($("#hiddenSupplier").html());
    if (ValIsDefine(suppliers)) {
        $("#addSuppliers").val(suppliers);
    }
}
