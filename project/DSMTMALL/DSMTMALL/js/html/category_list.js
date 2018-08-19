$(document).off("pagebeforeshow", "#divPageCategoryList").on("pagebeforeshow", "#divPageCategoryList", function () {//页面载入前执行
    var currentPage = $(this);
    ActiveFooter(currentPage, 2);//使页脚显示被选样式
    var categoryType= $.cookie("categoryType");
    var url = "/web/ajax.aspx";
    if (!ValIsDefine(categoryType)) {
        categoryType=$.trim(currentPage.find("[name=parentCateID]").eq(0).find("a").eq(0).attr("mydata"));
    }
    args = { "jsonType": "getSubCategoryList", "type": categoryType, "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data != "UNLOGIN") {
            var jsonData = eval(data);
            if (jsonData != 0) {
                $.each(jsonData, function (key, val) {
                    currentPage.find("#subCategoryList").append(GetSubCategory(val["CateName"], val["NShowImage"], val["CateID"]));
                });
            } else {
                ShowNoRecord(currentPage, "该类目暂时为空");
            }
        }
        //currentPage.find("[name=parentCateID]").siblings("li").removeClass("active");//让当前元素的父类LI标签都失去active样式
        //currentPage.find("[name=parentCateID]").eq(0).addClass("active");//让当前选择元素的父类LI标签获取active样式
        currentPage.find("[name=parentCateID]").each(function () {
            if ($.trim($(this).find("a").eq(0).attr("mydata")) == categoryType) {
                $(this).siblings("li").removeClass("active");//让当前元素的父类LI标签都失去active样式
                $(this).addClass("active");//让当前选择元素的父类LI标签获取active样式
            }
        });
        //loaded();//注意：loaded()方法是调用滑块控件的方法，为了使页面的内容加载（更换）后还能继续使用这个滑块功能，就需要重新执行这个方法进行重新方法渲染
    });
    currentPage.find("[name=parentCateID]").each(function () {
        $(this).off("tap").on("tap", function () {
            var categoryType = $.trim($(this).find("a").eq(0).attr("mydata"));
            $.cookie("categoryType", categoryType);
            if (categoryType.length > 0) {
                var url = "/web/ajax.aspx";
                args = { "jsonType": "getSubCategoryList", "type": categoryType, "createTime": new Date() };
                $.post(url, args, function (data) {
                    currentPage.find("#subCategoryList").html("");
                    if (data != "UNLOGIN") {
                        var jsonData = eval(data);
                        if (jsonData != 0) {
                            $.each(jsonData, function (key, val) {
                                currentPage.find("#subCategoryList").append(GetSubCategory(val["CateName"], val["NShowImage"], val["CateID"]));
                            });
                        }
                    }
                });
            }
            $(this).siblings("li").removeClass("active");//让当前元素的父类LI标签都失去active样式
            $(this).addClass("active");//让当前选择元素的父类LI标签获取active样式
           
            //loaded_right();//注意：loaded()方法是调用滑块控件的方法，为了使页面的内容加载（更换）后还能继续使用这个滑块功能，就需要重新执行这个方法进行重新方法渲染
            return false;//使用return false 是为了防止html控件的事件传递，没有这句话会是这个方法被执行三次
        });
    });
    var myScroll_left = new IScroll('#left_Menu', { mouseWheel: true, click: true });
    var myScroll_right = new IScroll('#right_Menu', { mouseWheel: true, click: true });
    setTimeout(function () {
        myScroll_left.refresh();
    }, 1000);
    
   

    //搜索框功能
    Search(currentPage);
});

//function loaded() {
//    var  myScroll_left = new IScroll('#left_Menu', { mouseWheel: true, click: true });
//    var myScroll_right = new IScroll('#right_Menu', { mouseWheel: true, click: true });
//}
//function loaded_right() {
//     myScroll_right = new IScroll('#right_Menu', { mouseWheel: true, click: true });
//}

//function ChangeSubCategory(obj)
//{
//    var currentPage = $("[id=divPageCategoryList]:last");
//    var categoryType = $.trim($(obj).attr("mydata"));
//    if (categoryType.length > 0) {
//        var url = "/web/ajax.aspx";
//        args = { "jsonType": "getSubCategoryList", "type": categoryType, "createTime": new Date() };
//        $.post(url, args, function (data) {
//            currentPage.find("#subCategoryList").html("");
//            if (data != "UNLOGIN") {
//                var jsonData = eval(data);
//                if (jsonData != 0) {
//                    $.each(jsonData, function (key, val) {
//                        currentPage.find("#subCategoryList").append(GetSubCategory(val["CateName"], val["NShowImage"],val["CateID"]));
//                    });
//                }
//            }
//            currentPage.find(".ot-menu  li").click(function () {
//                tabIndex = $(this).index();
//                $(this).siblings("li").removeClass("active");
//                $(this).addClass("active");
//                currentPage.find(".ot-maininfo").children(".menutype").hide();
//                currentPage.find(".ot-maininfo").children(".menutype").eq(tabIndex).show();
//            });
//        });
//    }   
//    //loaded_right();//注意：loaded()方法是调用滑块控件的方法，为了使页面的内容加载（更换）后还能继续使用这个滑块功能，就需要重新执行这个方法进行重新方法渲染
//    return false;//使用return false 是为了防止html控件的事件传递，没有这句话会是这个方法被执行三次
//}

//离开页面的时候判断是否为商品详情页，如果不是，则删除当前页内容
$(document).off("pagehide", "#divPageCategoryList").on("pagehide", "#divPageCategoryList", function () {
    $("[id=divPageCategoryList]").remove();
   
});