//删除记录
function DeleteThis(obj,warnVal,jsonType) {
	var sysID = $(obj).parents("td").eq(0).attr("mydata");
	if (sysID.length <= 0) {
		$(obj).parent().before(ShowAlert("danger", "提示：系统出错，请重新登陆"));
	}
	else {
		$("#sgConfirm").html(warnVal);
		$("#aConfirm").val("确认");
		$("#aConfirm").off("click").on("click", function () {
			BtnAble($("#aConfirm"), false, "正在操作");
			var url = "/web/web_ajax.aspx";
			var args = { "jsonType": jsonType, "id": sysID, "createTime": new Date() }
			$.post(url, args, function (data) {
				if (data != "UNLOGIN") {
					if (data == "SUCCESS") {
					    //if ($("#tableProductList").find("tr").eq(2).html() == undefined) {
					    //    ToPage("before");
					    //} else {
					    //    ToPage("now");
					    //}
					    ShowSuccess($("#aConfirm"));
					} else {
						$("#aConfirm").parent().before(ShowAlert("danger", data));
					}
				} else {
					ToPage("login");
				}
				BtnAble($("#aConfirm"), true, "确认");
			});
		});
	}
	return false;
}

//删除记录
function DeleteThisImg(obj, warnVal, jsonType) {
    var sysID = $(obj).attr("mydata");
    if (sysID.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：系统出错，请重新登陆"));
    }
    else {
        $("#sgConfirm").html(warnVal);
        $("#aConfirm").val("确认");
        $("#aConfirm").off("click").on("click", function () {
            BtnAble($("#aConfirm"), false, "正在操作");
            var url = "/web/web_ajax.aspx";
            var args = { "jsonType": jsonType, "id": sysID, "createTime": new Date() }
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {
                        ShowSuccess($("#aConfirm"));
                    } else {
                        $("#aConfirm").parent().before(ShowAlert("danger", data));
                    }
                } else {
                    ToPage("login");
                }
                BtnAble($("#aConfirm"), true, "确认");
            });
        });
    }
    return false;
}

//消息确认
function ComfirmMsg(obj, warnVal, jsonType) {
    var sysID = $(obj).parents("td").eq(0).attr("mydata");
    if (sysID.length <= 0) {
        $(obj).parent().before(ShowAlert("danger", "提示：系统出错，请重新登陆"));
    }
    else {
        $("#sgConfirm").html(warnVal);
        $("#aConfirm").val("确认");
        $("#aConfirm").off("click").on("click", function () {
            BtnAble($("#aConfirm"), false, "正在操作");
            var url = "/web/web_ajax.aspx";
            var args = { "jsonType": jsonType, "id": sysID, "createTime": new Date() }
            $.post(url, args, function (data) {
                if (data != "UNLOGIN") {
                    if (data == "SUCCESS") {
                        ShowSuccess($("#aConfirm"));
                    } else {
                        $("#aConfirm").parent().before(ShowAlert("danger", data));
                    }
                } else {
                    ToPage("login");
                }
                BtnAble($("#aConfirm"), true, "确认");
            });
        });
    }
    return false;
}

//启用禁用(checkbox的点选事件)<td name="tIsShow" mydata="<%# Eval("IsShow") %>"> <input type="checkbox" name="isCheckEnable" onchange="ChangeCheckBox(this,'brandIsShow')"></td>
function ChangeCheckBox(obj, jsonType) {
    var sysID = $.trim($(obj).parents("tr").eq(0).find("[name =tID]").eq(0).html());
    var status = 0;
    if ($(obj).prop('checked')) {
        status = 1;
    }
    if (sysID.length <= 0) {
        alert("系统出错，请重新登陆");
    } else {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": jsonType, "id": sysID, "status": status, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data.indexOf("ERR") >= 0) {
                    alert(data);
                    window.location.reload();
                } else {
                    if (data == "1") {
                        $(obj).attr("checked", "checked");
                    } else {
                        $(obj).attr("checked");
                    }
                }
            } else {
                ToPage("login");
            }
        });
    }
    return false;
}

//更换产品图片_first
function ShowUploadPicture(obj) {
    var sysID = $.trim($(obj).parents("td").eq(0).attr("mydata"));
    var uType = $.trim($(obj).attr("mydata"));
    $("#uploadPicture").data("id", sysID);
    $("#uploadPicture").attr("mydata", $.trim($(obj).attr("mydata")));
}
//上传产品图片_second
function UploadPicture(obj) {
    var sysID = $.trim($("#uploadPicture").data("id"));
    var uType = $.trim($("#uploadPicture").attr("mydata"));
    var importFileName = $("#importFileName").val();
    var fileExitArr = importFileName.split(".")
    var fileExit = fileExitArr[fileExitArr.length - 1];
    if (fileExit == "jpg" || fileExit == "png" || fileExit == "dng" || fileExit == "gif" || fileExit == "jpeg") {
        var pathUrl = "/web/web_save.aspx?id=" + sysID +"&uType=" + uType + "";
        ajaxFileUpload(pathUrl, "importFile");
    } else {
        alert("图片格式不正确");
    }
    return false;
}

//上传EXCEL文件_first
function ShowUploadExel(obj) {
    var uType = $.trim($(obj).attr("mydata"));
    $("#uploadExcel").attr("mydata", $.trim($(obj).attr("mydata")));
}
//上传EXCEL文件_second
function UploadExcelFile(obj) {
    var sysID = "uploadExcel";
    var uType = $.trim($("#uploadExcel").attr("mydata"));
    var importFileName = $("#importFileExcel").val();
    var fileExitArr = importFileName.split(".")
    var fileExit = fileExitArr[fileExitArr.length - 1];
    if (fileExit == "xlsx" || fileExit == "xls" ) {
        var pathUrl = "/web/web_save.aspx?id=" + sysID + "&uType=" + uType + "";
        ajaxFileUpload(pathUrl, "importExcel");
    } else {
        alert("EXCEL格式不正确");
    }
    return false;
}

//文件上传jQuery插件之ajaxFileUpload 
function ajaxFileUpload(postUrl, fileID) {
    $.ajaxFileUpload
    (
        {
            url: postUrl,
            secureuri: false,
            fileElementId: fileID,
            dataType: 'json',
            success: function (data, status) {
                if (data["msg"].indexOf("ERR") >= 0) {
                    alert(data["msg"]);
                } else {
                    alert(data["msg"]);
                    ToPage("now");
                }
            },
            error: function (data, status, e) {
                alert(e);
                ToPage("now");
            },
        }
    );
}

//显示返回成功提示--显示0.5秒后自动跳转当前页
function ShowSuccess(obj) {
    $(obj).parent().before(ShowAlert("success", "SUCCESS"));
    setTimeout("ToPage('now')", 500);
}
//显示错误信息--显示2秒后自动隐藏
function ShowError(obj,errVal) {
    $(obj).parent().before(ShowAlert("danger", "提示:" + errVal));
    setTimeout("$('#divAlert').remove()",2000);
}

//更改商品库存信息<td name="tNumber" class="my-overflow-title"> <span id="tInventory" onclick="ChangeText(this,'updateInverntory')"><%# Eval("GoodsNumber")%></span></td>
function ChangeText(obj,jsonType) {
    $(obj).html("<input type='text'style='max-width:80px' id='updataNum' class='form-control input-sm' onblur=\"ChangeSpan(this,'"+jsonType+"')\" />").removeAttr("onclick");
    $("#updataNum").focus();
}
function ChangeSpan(obj,jsonType) {
    var theVal = $(obj).val();
    var trTable = $(obj).parents("tr").eq(0);
    var sysID = $.trim($(obj).parents("tr").eq(0).find("[name =tID]").eq(0).html());
    if (sysID.length <= 0) {
        alert("数据出错，请重新登陆");
    } else {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType":jsonType, "id": sysID, "number": theVal, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                $(obj).parent("span").eq(0).html(data).attr("onclick", "ChangeText(this,'" + jsonType + "')");
            } else {
                ToPage("login");
            }
        });
    }
}

//描述框点击修改事件—first
//<td name="tDesc"> <a style="text-decoration: none" tabindex="0" role="button" data-toggle="popover" data-trigger="focus" data-content="<%# Eval("BrandDesc")%>">
//<textarea readonly="readonly" class="form-control" onclick="ChangeDescribed(this,'')" mydata="<%# Eval("BrandID") %>"><%#  Eval("BrandDesc") %></textarea></a></td>
function ChangeDescribed(obj,jsonType) {
    $(obj).removeAttr("readonly").attr("onblur", "ChangeTextarea(this,'" + jsonType + "')");
}
//描述框点击修改事件—second
function ChangeTextarea(obj, jsonType) {
    var theVal = $.trim($(obj).val());
    var sysID = $.trim($(obj).attr("mydata"));
    if (sysID.length <= 0) {
        alert("数据出错，请重新登陆");
    } else {
        var url = "/web/web_ajax.aspx";
        var args = {
            "jsonType": jsonType, "id": sysID, "word": theVal, "createTime": new Date()
        };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data.indexOf("ERROR") >= 0) {
                    alert(data);
                } else {
                    $(obj).val(data);
                }
                $(obj).attr("readonly", "true");
            } else {
                ToPage("login");
            }
        });
    }
}

//img标签的onerror事件-加载不了图片使用默认的替换图片进行显示
function nofind() {
    var img = event.srcElement;
    img.src = "/images/web/errloader.jpg";
    img.onerror = null; //控制不要一直跳动
}
//判断变量是否为NULL,"" Undefind --部位空返回true
function ValIsDefine(val) {
    if (typeof (val) == "undefined") {
        return false;
    }
    if (val == null) {
        return false
    }
    if (val == "") {
        return false;
    }
    return true;
}

//表单验证插件JS部分----开始
(function () {
    var initializing = false, fnTest = /xyz/.test(function () { xyz; }) ? /\b_super\b/ : /.*/;
    this.Class = function () { };
    Class.extend = function (prop) {
        var _super = this.prototype;
        initializing = true;
        var prototype = new this();
        initializing = false;
        for (var name in prop) {
            prototype[name] = typeof prop[name] == "function"
					&& typeof _super[name] == "function" && fnTest.test(prop[name]) ?
				(function (name, fn) {
				    return function () {
				        var tmp = this._super;
				        this._super = _super[name];
				        var ret = fn.apply(this, arguments);
				        this._super = tmp;
				        return ret;
				    };
				})(name, prop[name]) : prop[name];
        }
        function Class() {
            if (!initializing && this.init)
                this.init.apply(this, arguments);
        }
        Class.prototype = prototype;
        Class.prototype.constructor = Class;
        Class.extend = arguments.callee;
        return Class;
    };
})();
var validate = Class
		.extend({
		    defaultCfg: {
		        rules: {},
		        submitFun: function () { },
		        errorLabel: '<label style="color:red;font-family:KaiTi;"></label>',
		        errorFun: function () { }
		    },
		    init: function (cfg) {
		        this.cfg = $.extend({}, this.defaultCfg, cfg);
		        this.flag = 0;
		        this.toAction(this);
		        if (this.flag == 0) {
		            for (var i in this.cfg.rules) {
		                $("#" + i).unbind("keyup");
		            }
		            this.cfg.submitFun();
		        }
		    },
		    toAction: function (that) {
		        for (var i in that.cfg.rules) {
		            this.toVal("#" + i, that.cfg.rules[i]);
		        }
		    },
		    toVal: function (ele, constant) {
		        validateConstant[constant].test($(ele).val()) ?
					this.toRemoveError(ele) : this.toShowError(ele, errorMsg[constant]);

		    },
		    toRemoveError: function (ele) {
		        var that = this;
		        if ($(ele).closest(".form-group").attr("not-allow")) {
		            $(ele).removeAttr("style").closest(".form-group").removeAttr("style")
							.removeAttr("not-allow");
		            $(ele).next().remove();
		            $(ele).keyup(function () {
		                ele = ele.replace("#", "");
		                that.toVal("#" + ele, that.cfg.rules[ele]);
		            });
		        }
		    },
		    toShowError: function (ele, message) {
		        var error = $(this.cfg.errorLabel).text(message);
		        if (!$(ele).closest(".form-group").attr("not-allow")) {
		            $(ele).after(error);
		            $(ele).css("border", "1px solid red").closest(".form-group")
							.css("color", "red").attr("not-allow", "true");
		            $(ele).keyup(function () {
		                ele = ele.replace("#", "");
		                that.toVal("#" + ele, that.cfg.rules[ele]);
		            });
		        }
		        this.flag++;
		        var that = this;

		    }
		})
var validateConstant = {
    "notEmpty": /^.+$/,// 合法字符
    "password": /^[0-9A-Za-z]{1,18}$/,// 密码
    "rightfulString": /^[A-Za-z0-9_-]+$/,// 合法字符
    "number": /^\d+$/,// 数字
    "endlish": /^[A-Za-z]+$/,// 纯英文
    "numberEnglish": /^[A-Za-z0-9]+$/,// 英文和数字
    "float": /^[+]?\d+(\.\d+)?$/,// 浮点型
    "money": /(^[1-9]\d{0,9}(\.\d{1,2})?$)/,
    "chinese": "/^[\u4e00-\u9fa5]+$/",// 纯中文
    "mobile": /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1})|(17[0-9]{1})|(14[0-9]{1}))+\d{8})$/,// 手机号
    "tel": /^(\d{3,4}-?)?\d{7,9}$/g,// 电话
    "qq": /^[1-9]\d{4,12}$/,// qq
    "zipCode": /^[0-9]{6}$/,// 邮政编码
    "email": /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/,// 邮箱
    "positive": /^[1-9][0-9]+$/,//大于0的数字
    "checkIdCard": function (idcard) {// 校验身份证
        var area = {
            11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西",
            37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃",
            63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外"
        }
        var idcard, Y, JYM;
        var S, M;
        var idcard_array = new Array();
        idcard_array = idcard.split("");

        //地区检验 
        if (area[parseInt(idcard.substr(0, 2))] == null) {
            return false;
        }
        //身份号码位数及格式检验 
        switch (idcard.length) {
            case 15:
                if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0)) {
                    ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/;
                    //测试出生日期的合法性 
                } else {
                    ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/;
                    //测试出生日期的合法性 
                }

                if (ereg.test(idcard)) {
                    return true;
                } else {
                    return false;
                }
                break;
            case 18:
                //18位身份号码检测 
                //出生日期的合法性检查 
                //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9])) 
                //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8])) 
                if (parseInt(idcard.substr(6, 4)) % 4 == 0 || (parseInt(idcard.substr(6, 4)) % 100 == 0 && parseInt(idcard.substr(6, 4)) % 4 == 0)) {
                    ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/;
                    //闰年出生日期的合法性正则表达式 
                } else {
                    ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/;
                    //平年出生日期的合法性正则表达式 
                }
                if (ereg.test(idcard)) {//测试出生日期的合法性 
                    //计算校验位 
                    S = (parseInt(idcard_array[0]) + parseInt(idcard_array[10])) * 7 + (parseInt(idcard_array[1]) + parseInt(idcard_array[11])) * 9
					+ (parseInt(idcard_array[2]) + parseInt(idcard_array[12])) * 10 + (parseInt(idcard_array[3]) + parseInt(idcard_array[13])) * 5
					+ (parseInt(idcard_array[4]) + parseInt(idcard_array[14])) * 8 + (parseInt(idcard_array[5]) + parseInt(idcard_array[15])) * 4
					+ (parseInt(idcard_array[6]) + parseInt(idcard_array[16])) * 2 + parseInt(idcard_array[7]) * 1 + parseInt(idcard_array[8]) * 6
					+ parseInt(idcard_array[9]) * 3;
                    Y = S % 11;
                    M = "F";
                    JYM = "10X98765432";
                    M = JYM.substr(Y, 1);//判断校验位 
                    if (M == idcard_array[17].toUpperCase()) {
                        return true; //检测ID的校验位 
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
                break;
            default:
                return false;
                break;
        }
    }
}
var errorMsg = {
    "notEmpty": "不能为空",
    "password": "请输入正确的密码",
    "rightfulString": "请输入合法字符",// 合法字符
    "number": "只能输入数字",// 数字
    "endlish": "只能输入英文",// 纯英文
    "numberEnglish": "只能输入英文或数字",// 英文和数字
    "float": "只能输入小数",// 浮点型
    "money": "请输入合法价格",
    "chinese": "只能输入汉字",// 纯中文
    "mobile": "请输入正确的手机号",// 手机号
    "tel": "请输入正确的电话号码",// 电话
    "qq": "请输入正确的QQ号",// qq
    "zipCode": "请输入正确的QQ号",
    "email": "请输入正确的邮箱",// 邮箱
    "positive": "请输入大于0的数字",//大于0的数字
    "checkIdCard": "请输入正确的身份证号"// 校验身份证
}
//--结束：使用示例
//html代码部分：
//<div class="form-group">
//    	  <label class="col-sm-2 control-label form-label">邮    箱:</label>
//    	  <div class="col-sm-8">
//    	    <input type="text" class="form-control" id="card_name2">
//    	  </div>
// </div>
//<div class="row">
//    	<div class="form-group btn-group col-md-offset-4">
//    		<button class="btn btn-success" onclick="toValidate()">提    交</button>
//    	</div>
//</div>
//js代码调用部分：
//function toValidate() {
//    var val = new validate({
//        /*rules里面是检验规则， *key为需要检验的input的id,*value为此input对应的检验规则*/
//        rules: {
//            card_name: "notEmpty",
//            card_name1: "mobile",
//            card_name2: "email",
//            card_name3: "notEmpty"
//        },
//        /*submitFun里面为检验成功后要执行的方法*/
//        submitFun: function () {
//            toSubmit();
//        }
//    })
//}
//function toSubmit() {
//    alert("验证通过，提交啦 ！！！")
//}
//表单验证插件JS部分----结束


//联想框
function ShowUlList(obj, jsonType, funName) {
    var inputVal = $.trim($(obj).val());
    var excludeID = $.trim($(obj).data("exclude"));
    $(obj).removeAttr("mydata");
    if (inputVal.length > 0) {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": jsonType, "id": excludeID, "name": inputVal, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                 var jsonData = eval( '('+data+')' );
                if (jsonData != "") {
                    $(obj).next().empty();
                    $.each(jsonData, function (key, val) {
                        console.log(val);
                        $(obj).next().append(GetUlList(key, val, funName));
                    });
                    $(obj).next().show();
                } else {
                    $(obj).next().hide().empty();
                }
            } else {
                ToPage("login");
            }
        });
    } else {
        $(obj).next().hide().empty();
    }
    return false;
}
function ChoiseUlList(obj) {
    $(obj).parents("ul").eq(0).prev().val($.trim($(obj).text())).attr("mydata", $.trim($(obj).attr("mydata")));
    $(obj).parents("ul").eq(0).hide().empty();
    return false;
}
function GetUlList(id, name, funName) {
    funName = funName == undefined ? "ChoiseUlList" : funName;
    var myList = "<li><a href=\"javascript:;\" mydata=\"" + id + "\" onclick=\"" + funName + "(this)\">" + name + "</a></li>";
    return myList;
}

//获取Guid
function GetGuid() {
    var s = [];
    var hexDigits = "0123456789abcdef";
    for (var i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
    }
    s[14] = "4";
    s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);
    s[8] = s[13] = s[18] = s[23] = "-";
    var uuid = s.join("");
    return uuid;
}


//--针对button按钮使按钮图标变成转圈样式，同时禁用该按钮图标，同时使用mydata来防止二次提交//onsubmit="return false;"
/*下面是登陆页面的一套*/  //=====使用方法=====  function(){  var html = $(obj).html(); if (CheckedButton(obj)) {  UnCheckedButton(obj,html,"数据不正确");//或者//UnCheckedButton(obj,html,"数据不正确",12);//或者// UnCheckedButton(obj,html) }  retrun false;  };
function CheckedButton(obj) {
    if ($(obj).attr("mydata") == "1") {
        return false;
    } else {
        $(obj).attr("mydata", "1");
        $(obj).html(" <i class=\"fa fa-spinner fa-pulse fa-3x fa-fw\"></i>");//开始转圈
        $(obj).prop("disabled", "disabled");
        return true;
    }
}
function UnCheckedButton(obj, vals) {
    $(obj).html(vals);//变回来
    $(obj).removeAttr("disabled").attr("mydata", "0");
}
function UnCheckedButton(obj, vals, errVal) {
    $(obj).html("<h4 style=\"font-size:16px;color:red\">" + errVal + "</h4>");
    setTimeout(function () {
        $(obj).html(vals);
        $(obj).removeAttr("disabled").attr("mydata", "0");
    }, 2000);
}
/*下面是通用button的一套，不对button按钮进行禁用，这时候可以使用HTML自带的表单验证*/ //注意 字体大小可以忽略，默认12px
function CheckedComButton(obj, font_size) {
    if ($(obj).attr("mydata") == "1") {
        return false;
    } else {
        $(obj).attr("mydata", "1");
        font_size= ValIsDefine(font_size) ? font_size :"12";
        $(obj).html(" 提交中&nbsp<i class=\"fa fa-spinner fa-pulse fa-3x fa-fw\" style=\"font-size:" + font_size + "px;\"></i>");//开始转圈
        return true;
    }
}//是否第一次点击
function UnCheckedComBtnBack(obj, vals) {
    $(obj).html(vals);//变回来
    $(obj).removeAttr("disabled").attr("mydata", "0");
}//从禁用立马变回来
function UnCheckedComButton(obj, vals, errVal, font_size) {
    font_size = ValIsDefine(font_size) ? font_size : "12";
    $(obj).html("<span style=\"font-size:"+font_size+"px;color:red\">" + errVal + "</span>");
    setTimeout(function () {
        $(obj).html(vals);
        $(obj).removeAttr("disabled").attr("mydata", "0");
    }, 2000);
}//从禁用变回来--同时2秒显示错误内容
function BtnAbled(obj) {
    $(obj).prop("disabled", "disabled");
}//用来ajax提交后禁用按钮---当然不掉用这个方法也可以，就不变灰
/*下面是通用的BUTTON的一套，对BUTTON按钮进行禁用，这时候没办法使用HTML自带的表单验证*/ //注意 字体大小可以忽略，默认12px
function ComButton(obj, font_size) {
    if ($(obj).attr("mydata") == "1") {
        return false;
    } else {
        $(obj).attr("mydata", "1");
        font_size = ValIsDefine(font_size) ? font_size : "12";
        $(obj).html(" 提交中&nbsp<i class=\"fa fa-spinner fa-pulse fa-3x fa-fw\" style=\"font-size:" + font_size + "px;\"></i>");//开始转圈
        $(obj).attr("disabled", "disabled");
        return true;
    }
}
function UnComBtnBack(obj, vals) {
    $(obj).html(vals);//变回来
    $(obj).removeAttr("disabled").attr("mydata", "0");
}
function UnComButton(obj, vals, errVal, font_size) {
    font_size = ValIsDefine(font_size) ? font_size : "12";
    $(obj).html("<span style=\"font-size:" + font_size + "px;color:red\">" + errVal + "</span>");
    setTimeout(function () {
        $(obj).html(vals);
        $(obj).removeAttr("disabled").attr("mydata", "0");
    }, 2000);
}


//提示框
function showInfo(msg) {
    showTip(msg, 'info');
}
function showSuccess(msg) {
    showTip(msg, 'success');
}
function showFailure(msg) {
    showTip(msg, 'danger');
}
//tip是提示信息，type:'success'是成功信息，'danger'是失败信息,'info'是普通信息
function showTip(tip, type) {
    var win = parent ? parent : window;
    var $tip = $('#tip', win.document);
    $tip.attr('class', 'alert alert-' + type).text(tip).css('margin-left', -$tip.outerWidth() / 2).fadeIn(500).delay(2000).fadeOut(500);
}
