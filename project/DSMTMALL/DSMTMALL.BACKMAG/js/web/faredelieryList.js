$(function () {
    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    $("#chooseAddressModel").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
        $("#chooseAddressInfo").empty();
    });
    $("[name=fareSysID]").each(function () {    
        var id = $.trim($(this).html());
        var my_tr = $(this);
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "getFareCarry","id":id, "createTime": new Date() };
        $.post(url, args, function (data) {
            var jsonArr = eval(data);
            if (ValIsDefine(jsonArr)) {
                $.each(jsonArr, function (key, val) {                                                                       //( ValIsDefine(val["CarryAddressList"].split("|")[1]) ? val["CarryAddressList"].split("|")[1] : "全国" )注意加括号把它变为一个表达式
                    my_tr.parents("table").append("<tr class=\"default font-center\" mydata=\"" + val["CarrySysID"] + "\"> <td>" + val["CarryWay"] + "</td><td>" + (ValIsDefine(val["CarryAddressList"].split("|")[1]) ? val["CarryAddressList"].split("|")[1] : "全国") + "</td><td>" + val["FirstCount"] + "</td><td><span onclick=\"ChangeMoney(this,'updateFirstMoney')\">" + val["FirstMoney"] + "</span></td><td>" + val["ContinueCount"] + "</td><td><span onclick=\"ChangeMoney(this,'updateContunueMoney')\">" + val["ContinueMoney"] + "</span></td> </tr>");
                });
        };
        });
    });
    var val = new validate({
        /*rules里面是检验规则， *key为需要检验的input的id, *value为此input对应的检验规则*/
        rules: {
            addfareTime: "number",
            addfareAddress: "notEmpty",
            addfareName: "notEmpty"
        },
        /*submitFun里面为检验成功后要执行的方法*/
        submitFun: function () {
            AddNewGoods(obj);
        }
    });
});


function AddNewFareAddress(obj) {
    var str =$("#editorFareCarry");
    str.append("<tr name=\"addFareList\"><td><span>未添加地区</span><a data-toggle=\"modal\" data-target=\"#chooseAddressModel\" id=\"btnAssign\" onclick=\"ChooseAddress(this)\">编辑</a></td><td><input class=\"form-control\" type=\"text\" name=\"count\"/></td> <td><input class=\"form-control\" name=\"money\" type=\"text\" /></td> <td> <input class=\"form-control\" name=\"addCount\" type=\"text\" /></td><td><input class=\"form-control\" name=\"addFee\" type=\"text\" /></td><td><a  onclick=\"DeleteFareAddress(this)\">删除</a></td> </tr>");
    return false;
}

function DeleteFareAddress(obj) {
    $(obj).parents("tr").eq(0).remove();
    return false;
}

function ChooseAddress(obj) {
    var url = "/web/web_ajax.aspx";
    var args = { "jsonType": "getProvience", "createTime": new Date() };
    $("#chooseAddressModel").data("id", $(obj));
    $.post(url, args, function (data) {
        var jsonArr = eval(data);
        if (ValIsDefine(jsonArr)) {
            $.each(jsonArr, function (key, val) {
                $("#chooseAddressInfo").append("<div class=\"col-xs-3\"><div class=\"checkbox\"><label><input type=\"checkbox\" name=\"chiseAddress\" mydata=" + val["RegionID"] + " value=\""+ val["RegionName"] + " \" /> " + val["RegionName"] + " </label></div> </div>");
            });
        };
    });
    return false;
}

function ConfirmAddress(obj) {
    var objs = $("#chooseAddressModel").data("id");
    var str = "";
    $.each($("input[type='checkbox']"), function () {
        if ($(this).prop("checked") == true) {
            str += "<font mydata=\"" + $(this).attr("mydata") + "\">" + $(this).val()+ "</font>"
        }
    });
    objs.parent("td").eq(0).find("span").html(str);
    return false;
}
//保存信息
function SaveAddress(obj) {
    var fareName = $.trim($("#addfareName").val());
    var fareAddress = $.trim($("#addfareAddress").val());
    var fareTime = $.trim($("#addfareTime").val());
    var radios = $("[name=inlineRadioOptions]");
    var fareType = "";
    $.each(radios, function () {
        if ($(this).prop("checked") == true) {
            fareType = $(this).val();
        }
    });
    var fareDefaultCount = $.trim($("[name=default-count]").val());
    var fareDefaultMoney = $.trim($("[name=default-money]").val());
    var fareDefaultAddCount = $.trim($("[name=default-addCount]").val());
    var fareDefaultAddFee = $.trim($("[name=default-addFee]").val());
    var faredefaultAddress ="{"+ "defaultCount:" + fareDefaultCount + ",defaultMoney:" + fareDefaultMoney + ",defaultAddCount:" + fareDefaultAddCount + ",defaultAddFee:" + fareDefaultAddFee + "}";
    var fareChooseAddress = "";
    var provienceID = "";
    var provienceName = "";
    $.each($("[name=addFareList]"), function () {
        provienceID = "";
        provienceName = "";
        $.each($(this).find("font"), function () {
            provienceID += provienceID.length <= 0 ? "k"+ $.trim($(this).attr("mydata"))+"k" : "&k" + $.trim($(this).attr("mydata"))+"k";
            provienceName += provienceName.length <= 0 ? $.trim($(this).html()) : "&" + $.trim($(this).html());
        });
        var provience = provienceID+"|"+provienceName ;
        fareChooseAddress += fareChooseAddress.length <= 0 ? "{"+ "Address:'"+provience + "',Count:'" + $.trim($(this).find("input[name='count']").val()) + "',Money:'" + $.trim($(this).find("input[name='money']").val()) + "',AddCount:'" + $.trim($(this).find("input[name='addCount']").val()) + "',AddFee:'" + $.trim($(this).find("input[name='addFee']").val()) + "'}" : ",{"+"Address:'" + provience + "',Count:'" + $.trim($(this).find("input[name='count']").val()) + "',Money:'" + $.trim($(this).find("input[name='money']").val()) + "',AddCount:'" + $.trim($(this).find("input[name='addCount']").val()) + "',AddFee:'" + $.trim($(this).find("input[name='addFee']").val()) + "'}";
    });
    
    if (ValIsDefine(fareName) && ValIsDefine(fareAddress) && ValIsDefine(fareTime) && ValIsDefine(fareType) && ValIsDefine(faredefaultAddress) && ValIsDefine(fareChooseAddress)) {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "addNewFareTemp", "name": fareName, "url": fareAddress, "startTime": fareTime, "type": fareType, "typeB": faredefaultAddress, "typeC":"["+ fareChooseAddress+"]", "createTime": new Date() };//加中括号是为了适应JSON格式（多个值）
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") { ShowSuccess(obj); } else {
                    ShowError(obj,data);
                }
            } else {
                ToPage("login");
            }
        });
    } else {
        ShowError(obj, "提交的内容有误，请核实");
    }

    return false;
}
//删除信息
function DeleteFareTemp(obj) {
    var sysID = $.trim($(obj).parents("caption").attr("mydata"));
    if (ValIsDefine(sysID)) {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "deleteFareTemp","id":sysID, "createTime": new Date() };//加中括号是为了适应JSON格式（多个值）
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
    }
    return false;
}


//更改商品库存信息<td name="tNumber" class="my-overflow-title"> <span id="tInventory" onclick="ChangeText(this,'updateInverntory')"><%# Eval("GoodsNumber")%></span></td>
function ChangeMoney(obj, jsonType) {
    $(obj).html("<input type='text'style='max-width:80px' id='updataNum' class='form-control input-sm' onblur=\"ChangeSpan(this,'" + jsonType + "')\" />").removeAttr("onclick");
    $("#updataNum").focus();
}
function ChangeSpan(obj, jsonType) {
    var theVal = $(obj).val();
    var trTable = $(obj).parents("tr").eq(0);
    var sysID = $.trim($(obj).parents("tr").eq(0).attr("mydata"));
    if (sysID.length <= 0) {
        alert("数据出错，请重新登陆");
    } else {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": jsonType, "id": sysID, "number": theVal, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                $(obj).parent("span").eq(0).html(data).attr("onclick", "ChangeMoeny(this,'" + jsonType + "')");
            } else {
                ToPage("login");
            }
        });
    }
}
