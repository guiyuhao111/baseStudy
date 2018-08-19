jQuery.cookie = function (name, value, options) {
    if (typeof value != 'undefined') {
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString();
        }
        var path = options.path ? '; path=' + (options.path) : '';
        var domain = options.domain ? '; domain=' + (options.domain) : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else {
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};
//文件上传
jQuery.extend({
    createUploadIframe: function (id, uri) {//id为当前系统时间字符串，uri是外部传入的json对象的一个参数
        //create frame
        var frameId = 'jUploadFrame' + id; //给iframe添加一个独一无二的id
        var iframeHtml = '<iframe id="' + frameId + '" name="' + frameId + '" style="position:absolute; top:-9999px; left:-9999px"'; //创建iframe元素
        if (window.ActiveXObject) {//判断浏览器是否支持ActiveX控件
            if (typeof uri == 'boolean') {
                iframeHtml += ' src="' + 'javascript:false' + '"';
            } else if (typeof uri == 'string') {
                iframeHtml += ' src="' + uri + '"';
            }
        }
        iframeHtml += ' />';
        jQuery(iframeHtml).appendTo(document.body); //将动态iframe追加到body中
        return jQuery('#' + frameId).get(0); //返回iframe对象
    },
    createUploadForm: function (id, fileElementId, data) {//id为当前系统时间字符串，fileElementId为页面<input type='file' />的id，data的值需要根据传入json的键来决定
        //create form    
        var formId = 'jUploadForm' + id; //给form添加一个独一无二的id
        var fileId = 'jUploadFile' + id; //给<input type='file' />添加一个独一无二的id
        var form = jQuery('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data" ></form>'); //创建form元素
        if (data) {//通常为false
            for (var i in data) {
                jQuery('<input type="hidden" name="' + i + '" value="' + data[i] + '" />').appendTo(form); //根据data的内容，创建隐藏域，这部分我还不知道是什么时候用到。估计是传入json的时候，如果默认传一些参数的话要用到。
            }
        } var oldElement = jQuery('#' + fileElementId); //得到页面中的<input type='file' />对象
        var newElement = jQuery(oldElement).clone(); //克隆页面中的<input type='file' />对象
        jQuery(oldElement).attr('id', fileId); //修改原对象的id
        jQuery(oldElement).before(newElement); //在原对象前插入克隆对象
        jQuery(oldElement).appendTo(form); //把原对象插入到动态form的结尾处
        //set attributes
        jQuery(form).css('position', 'absolute'); //给动态form添加样式，使其浮动起来，
        jQuery(form).css('top', '-1200px');
        jQuery(form).css('left', '-1200px');
        jQuery(form).appendTo('body'); //把动态form插入到body中
        return form;
    },
    ajaxFileUpload: function (s) {//这里s是个json对象，传入一些ajax的参数
        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout        
        s = jQuery.extend({}, jQuery.ajaxSettings, s); //此时的s对象是由jQuery.ajaxSettings和原s对象扩展后的对象
        var id = new Date().getTime(); //取当前系统时间，目的是得到一个独一无二的数字
        var form = jQuery.createUploadForm(id, s.fileElementId, (typeof (s.data) == 'undefined' ? false : s.data)); //创建动态form
        var io = jQuery.createUploadIframe(id, s.secureuri); //创建动态iframe
        var frameId = 'jUploadFrame' + id; //动态iframe的id
        var formId = 'jUploadForm' + id; //动态form的id
        // Watch for a new set of requests
        if (s.global && !jQuery.active++) {//当jQuery开始一个ajax请求时发生
            jQuery.event.trigger("ajaxStart"); //触发ajaxStart方法
        } var requestDone = false; //请求完成标志
        // Create the request object
        var xml = {}; if (s.global)
            jQuery.event.trigger("ajaxSend", [xml, s]); //触发ajaxSend方法
        // Wait for a response to come back
        var uploadCallback = function (isTimeout) {//回调函数
            var io = document.getElementById(frameId); //得到iframe对象
            try {
                if (io.contentWindow) {//动态iframe所在窗口对象是否存在
                    xml.responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
                    xml.responseXML = io.contentWindow.document.XMLDocument ? io.contentWindow.document.XMLDocument : io.contentWindow.document;
                } else if (io.contentDocument) {//动态iframe的文档对象是否存在
                    xml.responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
                    xml.responseXML = io.contentDocument.document.XMLDocument ? io.contentDocument.document.XMLDocument : io.contentDocument.document;
                }
            } catch (e) {
                jQuery.handleError(s, xml, null, e);
            } if (xml || isTimeout == "timeout") {//xml变量被赋值或者isTimeout == "timeout"都表示请求发出，并且有响应
                requestDone = true; //请求完成
                var status; try {
                    status = isTimeout != "timeout" ? "success" : "error"; //如果不是“超时”，表示请求成功
                    // Make sure that the request was successful or notmodified
                    if (status != "error") {                        // process the data (runs the xml through httpData regardless of callback)
                        var data = jQuery.uploadHttpData(xml, s.dataType); //根据传送的type类型，返回json对象，此时返回的data就是后台操作后的返回结果
                        // If a local callback was specified, fire it and pass it the data
                        if (s.success)
                            s.success(data, status); //执行上传成功的操作
                        // Fire the global callback
                        if (s.global)
                            jQuery.event.trigger("ajaxSuccess", [xml, s]);
                    } else
                        jQuery.handleError(s, xml, status);
                } catch (e) {
                    status = "error";
                    jQuery.handleError(s, xml, status, e);
                }                // The request was completed
                if (s.global)
                    jQuery.event.trigger("ajaxComplete", [xml, s]);                // Handle the global AJAX counter
                if (s.global && ! --jQuery.active)
                    jQuery.event.trigger("ajaxStop");                // Process result
                if (s.complete)
                    s.complete(xml, status);
                jQuery(io).unbind();//移除iframe的事件处理程序
                setTimeout(function () {//设置超时时间
                    try {
                        jQuery(io).remove();//移除动态iframe
                        jQuery(form).remove();//移除动态form
                    } catch (e) {
                        jQuery.handleError(s, xml, null, e);
                    }
                }, 100)
                xml = null
            }
        }        // Timeout checker
        if (s.timeout > 0) {//超时检测
            setTimeout(function () {                // Check to see if the request is still happening
                if (!requestDone) uploadCallback("timeout");//如果请求仍未完成，就发送超时信号
            }, s.timeout);
        } try {
            var form = jQuery('#' + formId);
            jQuery(form).attr('action', s.url);//传入的ajax页面导向url
            jQuery(form).attr('method', 'POST');//设置提交表单方式
            jQuery(form).attr('target', frameId);//返回的目标iframe，就是创建的动态iframe
            if (form.encoding) {//选择编码方式
                jQuery(form).attr('encoding', 'multipart/form-data');
            } else {
                jQuery(form).attr('enctype', 'multipart/form-data');
            }
            jQuery(form).submit();//提交form表单
        } catch (e) {
            jQuery.handleError(s, xml, null, e);
        }
        jQuery('#' + frameId).load(uploadCallback); //ajax 请求从服务器加载数据，同时传入回调函数
        return { abort: function () { } };
    },
    uploadHttpData: function (r, type) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;        // If the type is "script", eval it in global context
        if (type == "script")
            jQuery.globalEval(data);        // Get the JavaScript object, if JSON is used.
        if (type == "json")
            eval("data = " + data);        // evaluate scripts within html
        if (type == "html")
            jQuery("<div>").html(data).evalScripts(); return data;
    },

    handleError: function(s,xhr,status,e ){
        if ( s.error ) {
            s.error.call( s.context || s, xhr, status, e );
        }
        if ( s.global ) {
            (s.context ? jQuery(s.context) : jQuery.event).trigger( "ajaxError", [xhr, s, e] );
        }
    },

})

//引入的方法---新框架
function mainContentHeightAdjust() {//设置当前页的最小高度
    //var docHeight = jQuery(document).height();
    //if (docHeight > jQuery('.main-content').height())
    //    jQuery('.main-content').height(docHeight-50);
}
//引入的方法--新框架--结束

//通用
$(function () {
    $("html").niceScroll({ styler: "fb", cursorcolor: "#65cea7", cursorwidth: '6', cursorborderradius: '0px', background: '#424f63', spacebarenabled: false, cursorborder: '0', zindex: '1000' });

    $(".left-side").niceScroll({ styler: "fb", cursorcolor: "#65cea7", cursorwidth: '3', cursorborderradius: '0px', background: '#424f63', spacebarenabled: false, cursorborder: '0' });

    $(".left-side").getNiceScroll();
    if ($('body').hasClass('left-side-collapsed')) {
        $(".left-side").getNiceScroll().hide();
    }

    //载入菜单
    if ($.cookie("ulMenu") == null || $.cookie("ulMenu") == "") {
        if ($(".nav").eq(1).find("li").eq(0).attr("id") != undefined) {
            $.cookie("ulMenu", $(".nav").eq(1).find("li").eq(0).find("li").eq(0).attr("id"), { path: "/" });
        } else {
            $.cookie("ulMenu", $(".nav").eq(1).find("li").eq(1).attr("id"), { path: "/" });
        }
    }
    $(".nav li").each(function () {
        if ($(this).attr("id") == $.cookie("ulMenu")) {
            if ($(this).hasClass("menu-list")) {
                $(this).addClass("nav-active");
                $(this).eq(0).find("span").eq(0).removeClass("icon-angle-right").addClass("icon-angle-up");
            } else {
                $(this).parents(".menu-list").eq(0).addClass("nav-active");
                $(this).parents(".menu-list").eq(0).find("span").eq(0).removeClass("icon-angle-right").addClass("icon-angle-up");
                $(this).addClass("active");
            }
        }
    });
    //点击菜单
    $(".nav li a").off("click").on("click", function () {
        if ($(this).parent().attr("id") != undefined) {
            $.cookie("ulMenu", $(this).parent().attr("id"), { path: "/" });
        }
    });
    //菜单过渡动画
    jQuery('.menu-list > a').off("click").on("click", function () {
        var parent = jQuery(this).parent();
        var sub = parent.find('> ul');
        if (!jQuery('body').hasClass('left-side-collapsed')) {
            if (sub.is(':visible')) {
                sub.slideUp(200, function () {
                    parent.removeClass('nav-active');
                    parent.find("span").eq(0).removeClass("icon-angle-up").addClass("icon-angle-right");
                    jQuery('.main-content').css({ height: '' });
                    mainContentHeightAdjust();
                });
            } else {
                visibleSubMenuClose();
                parent.addClass('nav-active');
                parent.find("span").eq(0).removeClass("icon-angle-right").addClass("icon-angle-up");
                sub.slideDown(200, function () {
                    mainContentHeightAdjust();
                });
            }
        }
        return false;
    });

    function visibleSubMenuClose() {
        jQuery('.menu-list').each(function () {
            var t = jQuery(this);
            if (t.hasClass('nav-active')) {
                t.find('> ul').slideUp(200, function () {
                    t.removeClass('nav-active');
                    t.find("span").eq(0).removeClass("icon-angle-up").addClass("icon-angle-right");
                });
            }
        });
    }
    //点击按钮隐藏导航栏
    $(".toggle-btn").off("click").on("click", function () {
        $(".left-side").getNiceScroll().hide();
        if ($('body').hasClass('left-side-collapsed')) {
            $(".left-side").getNiceScroll().hide();
        }
        var body = jQuery('body');
        var bodyposition = body.css('position');
        if (bodyposition != 'relative') {
            if (!body.hasClass('left-side-collapsed')) {
                body.addClass('left-side-collapsed');
                jQuery('.custom-nav ul').attr('style', '');
                jQuery(this).addClass('menu-collapsed');
            } else {
                body.removeClass('left-side-collapsed chat-view');
                jQuery('.custom-nav li.active ul').css({ display: 'block' });
                jQuery(this).removeClass('menu-collapsed');
            }
        } else {
            if (body.hasClass('left-side-show'))
                body.removeClass('left-side-show');
            else
                body.addClass('left-side-show');
            mainContentHeightAdjust();
        }
    });

    //设置当前页面最小高度
   // mainContentHeightAdjust();
    /*页面载入事件*/
    $("[data-toggle='popover']").each(function () {
        $(this).popover();
    });
    $("#confirmModal").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        HideModal($(this));
    });
    /*页面最小高度*/
   
    $(".main-content").attr("style", "min-height:" + ($(window).height()) + "px");
    
});
//图片base64编码
function FileToImg(imgId, fileId) {
    var pic = document.getElementById(imgId);
    var file = document.getElementById(fileId);
    var fileval = $("#" + fileId).val();
    if (fileval == "") {
        return;
    }
    //gif在IE浏览器暂时无法显示
    if (!/\.(gif|jpg|jpeg|png|GIF|JPG|PNG)$/.test(file.value)) {
        alert("图片类型必须是.gif,jpeg,jpg,png中的一种,请重新上传!");
        file.outerHTML = file.outerHTML.replace(/(value=\").+\"/i, "$1\"");
        return;
    }
    var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
    var fileSize = 0;
    if (isIE && !file.files) {
        var filePath = file.value;
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        var fileImg = fileSystem.GetFile(filePath);
        fileSize = fileImg.Size;
    } else {
        fileSize = file.files[0].size;
    }
    var size = fileSize / 1024 * 1024; //单位B
    if (size > (1024 * 1024)) {
        alert("文件大小不能超过1M,请重新编辑后上传");
        file.outerHTML = file.outerHTML.replace(/(value=\").+\"/i, "$1\"");
        return;
    }
    //IE浏览器
    if (document.all) {
        file.select();
        var reallocalpath = document.selection.createRange().text;
        var ie6 = /msie 6/i.test(navigator.userAgent);
        //IE6浏览器设置img的src为本地路径可以直接显示图片
        if (ie6) {
            pic.src = reallocalpath;
        } else {
            //非IE6版本的IE由于安全问题直接设置img的src无法显示本地图片，但是可以通过滤镜来实现
            pic.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='image',src=\"" + reallocalpath + "\")";
            //设置img的src为base64编码的透明图片 取消显示浏览器默认图片
            pic.src = 'data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==';
        }
    } else {
        Html5Reader(file, pic);
    }
}
function Html5Reader(file, pic) {
    var file = file.files[0];
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function (e) {
        pic.src = this.result;
    };
}
//限制输入正整数
$.fn.numeralinput = function () {
    $(this).css("ime-mode", "disabled");
    this.bind("keypress", function (e) {
        if (e.charCode === 0) return true;  //非字符键 for firefox
        var code = (e.keyCode ? e.keyCode : e.which);  //兼容火狐 IE    
        return code >= 48 && code <= 57;
    });
    this.bind("blur", function () {
        if (!/\d+/.test(this.value)) {
            this.value = "";
        }
    });
    this.bind("paste", function () {
        if (window.clipboardData) {
            var s = clipboardData.getData('text');
            if (!/\D/.test(s)) {
                value = parseInt(s, 10);
                return true;
            }
        }
        return false;
    });
    this.bind("dragenter", function () {
        return false;
    });
    this.bind("keyup", function () {
        if (this.value !== '0' && /(^0+)/.test(this.value)) {
            this.value = parseInt(this.value, 10);
        }
    });
    this.bind("propertychange", function (e) {
        if (isNaN(this.value))
            this.value = this.value.replace(/\D/g, "");
    });
    this.bind("input", function (e) {
        if (isNaN(this.value))
            this.value = this.value.replace(/\D/g, "");
    });
};
//限制输入数字//$("#num").numeral(false);//限制只能输入整数//$("#price").numeral(true);//限制只能输入浮点数
$.fn.numeral = function (bl) {//限制金额输入、兼容浏览器、屏蔽粘贴拖拽等

    $(this).keypress(function (e) {

        var keyCode = e.keyCode ? e.keyCode : e.which;

        if (bl) {//浮点数

            if ((this.value.length == 0 || this.value.indexOf(".") != -1) && keyCode == 46 || ((this.value.indexOf(".")!= -1) && ( this.value.length-this.value.indexOf(".")>2))) return false;
            //|| ((this.value.indexOf(".")!= -1) && ( this.value.length-this.value.indexOf(".")>2))) 
            return keyCode >= 48 && keyCode <= 57 || keyCode == 46 || keyCode == 8;

        } else {//整数

            return keyCode >= 48 && keyCode <= 57 || keyCode == 8;

        }

    });

    $(this).bind("copy cut paste", function (e) { // 通过空格连续添加复制、剪切、粘贴事件 

        if (window.clipboardData)//clipboardData.setData('text', clipboardData.getData('text').replace(/\D/g, ''));

            return !clipboardData.getData('text').match(/\D/);

        else

            event.preventDefault();

    });

    $(this).bind("dragenter", function () { return false; });

    $(this).css("ime-mode", "disabled");

    $(this).bind("focus", function () {

        if (this.value.lastIndexOf(".") == (this.value.length - 1)) {

            this.value = this.value.substr(0, this.value.length - 1);

        } else if (isNaN(this.value)) {

            this.value = "";

        }

    });

}
//载入地区
function LoadProvince(obj, provinceName, cityName, areaName) {
    var optionProvince = "";
    $.getJSON("/js/area.min.js", function (jsonArea) {
        if (jsonArea.arealist != undefined) {
            $.each(jsonArea.arealist, function (key, val) {
                if (provinceName != "") {
                    if (val.p == provinceName) {
                        optionProvince += "<option value=\"" + key + "\" selected>" + val.p + "</option>";
                    } else {
                        optionProvince += "<option value=\"" + key + "\">" + val.p + "</option>";
                    }
                } else {
                    if (key == 0) {
                        optionProvince += "<option value=\"" + key + "\" selected>" + val.p + "</option>";
                    } else {
                        optionProvince += "<option value=\"" + key + "\">" + val.p + "</option>";
                    }
                }
            });
            $(obj).find("[name='selectProvince']").empty();
            $(obj).find("[name='selectProvince']").append(optionProvince);
            LoadCity(obj, $(obj).find("[name='selectProvince']").val(), cityName, areaName);
        }
    });
}
function LoadCity(obj, provinceId, cityName, areaName) {
    var optionCity = "";
    $.getJSON("/js/area.min.js", function (jsonArea) {
        if (jsonArea.arealist[provinceId].c != undefined) {
            $.each(jsonArea.arealist[provinceId].c, function (key, val) {
                if (cityName != "") {
                    if (val.n == cityName) {
                        optionCity += "<option value=\"" + key + "\" selected>" + val.n + "</option>";
                    } else {
                        optionCity += "<option value=\"" + key + "\">" + val.n + "</option>";
                    }
                } else {
                    if (key == 0) {
                        optionCity += "<option value=\"" + key + "\" selected>" + val.n + "</option>";
                    } else {
                        optionCity += "<option value=\"" + key + "\">" + val.n + "</option>";
                    }
                }
            });
        } else {
            optionCity += "<option value=\"-1\">无</option>";
        }
        $(obj).find("[name='selectCity']").empty();
        $(obj).find("[name='selectCity']").append(optionCity);
        CityChange(obj, provinceId, $(obj).find("[name='selectCity']").val(), areaName);
    });
}
function ProvinceChange(obj, provinceId, cityName) {
    var optionCity = "";
    $.getJSON("/js/area.min.js", function (jsonArea) {
        if (jsonArea.arealist[provinceId].c != undefined) {
            $.each(jsonArea.arealist[provinceId].c, function (key, val) {
                if (cityName != "") {
                    if (val.n == cityName) {
                        optionCity += "<option value=\"" + key + "\" selected>" + val.n + "</option>";
                    } else {
                        optionCity += "<option value=\"" + key + "\">" + val.n + "</option>";
                    }
                } else {
                    if (key == 0) {
                        optionCity += "<option value=\"" + key + "\" selected>" + val.n + "</option>";
                    } else {
                        optionCity += "<option value=\"" + key + "\">" + val.n + "</option>";
                    }
                }
            });
        } else {
            optionCity += "<option value=\"-1\">无</option>";
        }
        $(obj).find("[name='selectCity']").empty();
        $(obj).find("[name='selectCity']").append(optionCity);
        CityChange(obj, provinceId, 0);
    });
}
function CityChange(obj, provinceId, cityId, areaName) {
    var optionArea = "";
    $.getJSON("/js/area.min.js", function (jsonArea) {
        if (jsonArea.arealist[provinceId].c[cityId].a != undefined) {
            $.each(jsonArea.arealist[provinceId].c[cityId].a, function (key, val) {
                if (areaName != "") {
                    if (val.s == areaName) {
                        optionArea += "<option value=\"" + key + "\" selected>" + val.s + "</option>";
                    } else {
                        optionArea += "<option value=\"" + key + "\">" + val.s + "</option>";
                    }
                } else {
                    if (key == 0) {
                        optionArea += "<option value=\"" + key + "\" selected>" + val.s + "</option>";
                    } else {
                        optionArea += "<option value=\"" + key + "\">" + val.s + "</option>";
                    }
                }
            });
        } else {
            optionArea += "<option value=\"-1\">无</option>";
        }
        $(obj).find("[name='selectArea']").empty();
        $(obj).find("[name='selectArea']").append(optionArea);
    });
}
//日期格式化
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}
//获取URL参数
function GetUrlParam(myUrl, myName) {
    var reg = new RegExp("(^|&)" + myName + "=([^&]*)(&|$)");
    var r = myUrl.substr(1).match(reg);
    if (r != null) return decodeURIComponent(r[2]); return null;
}
//隐藏模态框
function HideModal(obj, isRemoveBody) {
    $(obj).data("id", "");
    if (isRemoveBody) {
        $(obj).find(".modal-body").eq(0).empty();
    } else {
        $(obj).find("input[type=text],input[type=file]").each(function () {
            $(this).val("").removeAttr("mydata").removeData();
        });
        $(obj).find("textarea").each(function () {
            $(this).text("");
        });
        $(obj).find("select").each(function () {
            $(this).find("option").eq(0).prop("selected", "selected");
        });
        $(obj).find("ul").each(function () {
            $(this).hide().empty();
        });
    }
    $(obj).find("#divAlert").remove();
}
//退出框
function ExitConfirm() {
    $("#sgConfirm").html("确定要退出吗？");
    $("#aConfirm").val("退出");
    $("#aConfirm").off("click").on("click", function () {
        var url = "/web/web_ajax.aspx";
        var args = { "jsonType": "AdminLogout", "createTime": new Date() };
        $.post(url, args, function (data) {
            $.cookie("ulMenu", null, { path: "/" });
            ToPage("login");
        });
    });
    return false;
}
//提示信息
function ShowAlert(alertType, alertContent) {
    $("#divAlert").remove();
    var myList = "<div id=\"divAlert\" class=\"alert alert-" + alertType + " alert-dismissible fade in\" role=\"alert\">";
    myList += "<button type=\"button\" class=\"close\" aria-label=\"Close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">×</span></button>";
    myList += "<strong>" + alertContent + "</strong></div>";
    return myList;
}
//搜索
function btnSearch_Click(obj) {
    var divForm = $(obj).parents(".form-inline").eq(0);
    var urlSearch = "";
    divForm.find("select").each(function () {
        if (urlSearch == "") {
            urlSearch += "?" + $.trim($(this).attr("mydata")) + "=" + encodeURIComponent($.trim($(this).find("option:selected").val()));
        } else {
            urlSearch += "&" + $.trim($(this).attr("mydata")) + "=" + encodeURIComponent($.trim($(this).find("option:selected").val()));
        }
    });
    divForm.find("input[type=text]").each(function () {
        if (urlSearch == "") {
            urlSearch += "?" + $.trim($(this).attr("mydata")) + "=" + encodeURIComponent($.trim($(this).val()));
        } else {
            urlSearch += "&" + $.trim($(this).attr("mydata")) + "=" + encodeURIComponent($.trim($(this).val()));
        }
    });
   window.location.href = window.location.pathname + urlSearch;
}
//页面跳转
function ToPage(toType) {
    var urlPathname = window.location.pathname;
    var urlSearch = window.location.search;
    if (toType == "login") {
        window.location.href = "/web/web_login.aspx";
    } else if (toType == "first") {
        window.location.href = urlPathname;
    } else if (toType == "now") {
        window.location.href = urlPathname + urlSearch;
    } else if (toType == "before") {
        var pageNow = Number($.trim(GetUrlParam(urlSearch, "pageNow"))) - 1;
        if (pageNow < 1) {
            pageNow = 1;
        }
        urlSearch = urlSearch.replace(/pageNow=\d+/, "pageNow=" + pageNow);
        window.location.href = urlPathname + urlSearch;
    } else {
        window.location.href = toType;
    }
}
//按钮状态
function BtnAble(obj, isAble, btnText) {
    if (isAble) {
        $(obj).removeAttr("disabled");
    } else {
        $(obj).attr("disabled", "disabled");
    }
    $(obj).val(btnText);
}
//输入框状态
function CheckToInput(obj) {
    var nextInput = $(obj).parents("span").eq(0).next();
    if ($(obj).is(":checked")) {
        nextInput.val("");
        nextInput.removeAttr("disabled");
    } else {
        nextInput.val("");
        nextInput.attr("disabled", "disabled");
    }
}
//获取时间差
function GetDateGap(startDate, endDate, gapType) {
    if (gapType == "day") {
        return (Date.parse(endDate) - Date.parse(startDate)) / (1000 * 60 * 60 * 24);
    } else if (gapType == "hour") {
        return (Date.parse(endDate) - Date.parse(startDate)) / (1000 * 60 * 60);
    } else if (gapType == "minute") {
        return (Date.parse(endDate) - Date.parse(startDate)) / (1000 * 60);
    } else {
        return (Date.parse(endDate) - Date.parse(startDate)) / (1000);
    }
}
//获取报表查询参数
function GetQueryReportData(queryData, myData, parValue) {
    var myDataArr = $.trim(myData).split(",");
    var parValueArr = $.trim(parValue).split(",");
    if (myDataArr[1] == "string") {
        $.each(parValueArr, function (key, val) {
            if (key == 0) {
                parValue = "'" + val + "'";
            } else {
                parValue += ",'" + val + "'";
            }
        });
    }
    return queryData += queryData == "" ? myDataArr[0] + ";" + parValue : "|" + myDataArr[0] + ";" + parValue;
}
////联想框
//function ShowUlList(obj, jsonType, funName) {
//    var inputVal = $.trim($(obj).val());
//    var excludeID = $.trim($(obj).data("exclude"));
//    $(obj).removeAttr("mydata");
//    if (inputVal.length > 0) {
//        var url = "/web/web_ajax.aspx";
//        var args = { "jsonType": jsonType, "id": excludeID, "name": inputVal, "createTime": new Date() };
//        $.post(url, args, function (data) {
//            if (data != "UNLOGIN") {
//                var jsonData = eval("(" + data + ")");
//                if (jsonData != "") {
//                    $(obj).next().empty();
//                    for (var i = 0; i < jsonData.length; i++) {
//                        $(obj).next().append(GetUlList(jsonData[i].Key, jsonData[i].Value, funName));
//                    }
//                    $(obj).next().show();
//                } else {
//                    $(obj).next().hide().empty();
//                }
//            } else {
//                ToPage("login");
//            }
//        });
//    } else {
//        $(obj).next().hide().empty();
//    }
//    return false;
//}
//function ChoiseUlList(obj) {
//    $(obj).parents("ul").eq(0).prev().val($.trim($(obj).text())).attr("mydata", $.trim($(obj).attr("mydata")));
//    $(obj).parents("ul").eq(0).hide().empty();
//    return false;
//}
//function GetUlList(id, name, funName) {
//    funName = funName == undefined ? "ChoiseUlList" : funName;
//    var myList = "<li><a href=\"javascript:;\" mydata=\"" + id + "\" onclick=\"" + funName + "(this)\">" + name + "</a></li>";
//    return myList;
//}
//新添加一个记录时判断选项框中是否已经被选择了(如果已经被选，则返回true，否则返回false)
function checkIsHas(btnID, key) {//前一个btnID是div标签组的标签ID，后一个是要进行匹配判断的参数
    var dataArr = new Array();
    $("#" + btnID + "").find("input[type='button']").each(function () {  //遍历循环每一个myDevArr的type=button的标签内容mydata的属性值
        dataArr.push($.trim($(this).attr("mydata")));
    });
    if (dataArr.length > 0) {
        for (var i = 0; i < dataArr.length; i++) {
            if (key == dataArr[i]) {//如果相同，则返回true即已经存在
                return true;
            }
        }
        return false;
    } else {
        return false;
    }
}
//选择文件
function SelectFile(obj) {
    $(obj).prev().val($(obj).val());
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
                    ToPage("login");
                } else {
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
//打印内容  前端页面定义一个div 的id 通过这个div的id去寻找到这个要打印的内容，要打印的内容就是这个div内的所有内容 onclick="startPrint(document.getElementById('content'))
function startPrint(obj) {
    var oWin = window.open("", "_blank");
    var strPrint = "<h4 style=’font-size:18px; text-align:center;’>打印预览区</h4>\n";

    strPrint = strPrint + "<script type=\"text/javascript\">\n";
    strPrint = strPrint + "function printWin()\n";
    strPrint = strPrint + "{";
    strPrint = strPrint + "var oWin=window.open(\"\",\"_blank\");\n";
    strPrint = strPrint + "oWin.document.write(document.getElementById(\"content\").innerHTML);\n";
    strPrint = strPrint + "oWin.focus();\n";
    strPrint = strPrint + "oWin.document.close();\n";
    strPrint = strPrint + "oWin.print()\n";
    strPrint = strPrint + "oWin.close()\n";
    strPrint = strPrint + "}\n";
    strPrint = strPrint + "<\/script>\n";

    strPrint = strPrint + "<hr size=’1′ />\n";
    strPrint = strPrint + "<div id=\"content\">\n";
    strPrint = strPrint + obj.innerHTML + "\n";
    strPrint = strPrint + "</div>\n";
    strPrint = strPrint + "<hr size=’1′ />\n";
    strPrint = strPrint + "<div style=’text-align:center’><button onclick='printWin()' style=’padding-left:4px;padding-right:4px;’>打  印</button><button onclick='window.opener=null;window.close();'  style='padding-left:4px;padding-right:4px;'>关  闭</button></div>\n";
    oWin.document.write(strPrint);
    oWin.focus();
    oWin.document.close();
}
//打印预览，直接调用系统打印方法进行打印    <!--startprint1-->    打印这两个标签内的内容   <!--endprint1-->
function preview(oper) {
    if (oper < 10) {
        bdhtml = window.document.body.innerHTML;//获取当前页的html代码  
        sprnstr = "<!--startprint" + oper + "-->";//设置打印开始区域  
        eprnstr = "<!--endprint" + oper + "-->";//设置打印结束区域  
        prnhtml = bdhtml.substring(bdhtml.indexOf(sprnstr) + 18); //从开始代码向后取html   
        prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));//从结束代码向前取html  
        window.document.body.innerHTML = prnhtml;
        window.print();
        window.document.body.innerHTML = bdhtml;
    } else {
        window.print();
    }
}
//验证金额（可验证 大于等于零，小于等于99999999.99 的数字）
function checkMoney(obj) {
    if (/^([1-9][\d]{0,7}|0)(\.[\d]{1,2})?$/.test(obj)) {
        return true;
    }
}
//验证电话号码（手机号码+电话号码）
function checkPhoneNum(obj) {
    if (/^((\d{3}-\d{8}|\d{4}-\d{7,8})|(1[3|5|7|8][0-9]{9}))$/.test(obj)) {
        return true;
    }
}
//验证电话号码（手机号码，传入电话号码参数）
function CheckPhoneNum(number) {
    var flag = false;
    var message = "";
    var myreg = /^((\d{3}-\d{8}|\d{4}-\d{7,8})|(1[3|5|7|8][0-9]{9}))$/;
    if (number == '') {
        message = "手机号码不能为空！";
    } else if (number.length != 11) {
        message = "请输入有效的手机号码！";
    } else if (!myreg.test(number)) {
        message = "请输入有效的手机号码！";
    } else {
        flag = true;
    }
    if (!flag) {
        alert(message);
    }
    return flag;
}
//验证座机号码
function CheckTel(number) {
    var flag = false;
    var message = "";
    var myreg = /^(?:(?:0\d{2,3})-)?(?:\d{7,8})(-(?:\d{3,}))?$/;
    if (number == '') {
        message = "手机号码不能为空！";
    }  else if (!myreg.test(number)) {
        message = "请输入有效的座机号码！";
    } else {
        flag = true;
    }
    if (!flag) {
        alert(message);
    }
    return flag;
}
//检测输入的号码是否是手机或者座机号码
function CheckPhoneNumOrTel(number) {
    var flag = false;
    var myregPhone = /^((\d{3}-\d{8}|\d{4}-\d{7,8})|(1[3|5|7|8][0-9]{9}))$/;
    var myregTel = /^(?:(?:0\d{2,3})-)?(?:\d{7,8})(-(?:\d{3,}))?$/;
    if (number != '') {
        if (myregPhone.test(number)|| myregTel.test(number)) {
            flag = true;
        } 
    }
    return flag;
}
//验证企业地址（中文、英文、数字）
function checkAddress(obj) {
    if (/^[\u4e00-\u9fa5a-zA-Z0-9]+$/.test(obj)) {
        return true;
    }
}
//验证邮箱地址
function CheckEmail(values) {
    var flag = false;
    var regex = /^([0-9A-Za-z\-_\.]+)@([0-9a-z]+\.[a-z]{2,3}(\.[a-z]{2})?)$/g;
    if (!regex.test(values)) {
        alert("请输入正确的邮箱地址");
    } else {
        return flag = true;
    }
    return flag;
}
//获取文件类型(传值版本)
function getFileType(values) {//obj直接是控件
    return values.substr(values.lastIndexOf(".")).toLowerCase();//获得文件后缀名
}

//设置日期段区间(开始标签使用方法)<input type="text" id="startDay" class="form-control pointer" onfocus="iptStartDay('endDay')" placeholder="开始日期" maxlength="10" readonly="readonly"/>
function iptStartDay(iptEndDateID) {
    var iptEndDate = $dp.$(iptEndDateID);//使用 $dp.$D 函数 可以将日期框中的值,加上定义的日期差量:
    WdatePicker({ isShowClear: false, readOnly: true, onpicked: function () { iptEndDate.focus(); }, minDate: '%y-%M-{%d}', maxDate: '#F{$dp.$D(\'' + iptEndDateID + '\')}' });//这个开始日期，不能大于结束日期'#F{$dp.$D(\'iptEndDate\')}'
    //onpicked选择日期之后相应事件 ，日期选择之后，光标自动跳转到结束日期标签，前一个开始日期最大值不能大于结束日期最大值
}
//设置日期段区间（结束标签使用方法）<input type="text" id="endDay" class="form-control pointer" onfocus="iptEndDay('startDay')" placeholder="结束日期" maxlength="10" readonly="readonly"/>
function iptEndDay(iptStartDateID) {
    WdatePicker({ isShowClear: false, readOnly: true, minDate: '#F{$dp.$D(\''+ iptStartDateID +'\')}' });
}

//设置时间区间(开始标签使用方法)<input type="text" id="startDay" class="form-control pointer" onfocus="iptStartDay('endDay')" placeholder="开始日期" maxlength="10" readonly="readonly"/>
function iptStartTime(iptEndTimeID) {
    var iptEndTime = $dp.$(iptEndTimeID);//使用 $dp.$D 函数 可以将日期框中的值,加上定义的日期差量:
    WdatePicker({ dateFmt:'H:mm:ss', isShowClear: false, readOnly: true, onpicked: function () { iptEndTime.focus(); }, maxDate: '#F{$dp.$D(\'' + iptEndTimeID + '\')}' });//这个开始日期，不能大于结束日期'#F{$dp.$D(\'iptEndDate\')}'
    //onpicked选择日期之后相应事件 ，日期选择之后，光标自动跳转到结束日期标签，前一个开始日期最大值不能大于结束日期最大值
}
//设置时间区间（结束标签使用方法）<input type="text" id="endDay" class="form-control pointer" onfocus="iptEndDay('startDay')" placeholder="结束日期" maxlength="10" readonly="readonly"/>
function iptEndTime(iptStartTimeID) {
    WdatePicker({ dateFmt: 'H:mm:ss', isShowClear: false, readOnly: true, minDate: '#F{$dp.$D(\'' + iptStartTimeID + '\')}' });
}
