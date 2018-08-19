<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="category_list.aspx.cs" Inherits="DSMTMALL.web.category_list" %>
<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>
<%@ Register Src="~/web/uc/uc_header_part.ascx" TagPrefix="uc1" TagName="uc_header_part" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>品牌团分类</title>
    <meta http-equiv="Content-Type" content="text/html" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0 , maximum-scale=1.0, user-scalable=0" />
    <uc1:uc_title runat="server" ID="uc_title" />

</head>
<body>
    <div id="divPageCategoryList" data-role="page" data-position="fixed">
        <script src="/js/handler/iscroll.js"></script>
        <script src="../js/html/category_list.js"></script>
        <div data-role="header" data-tap-toggle="false" >
            <uc1:uc_header_part runat="server" ID="uc_header_part" />
        </div>
        <div data-role="content" data-theme="j">
            <div class="mg-auto overflow">
                <!--菜单-->
                <div class="Menu_box">
                    <!---------左侧菜单---------->
                    <div id="left_Menu">
                        <div class="ot-menu" id="scroller">
                            <ul id="gridTopCategoryList" runat="server">
                            </ul>
                        </div>
                    </div>

                    <!---------右侧侧菜单---------->
                    <div id="right_Menu">
                        <div class="menutype" id="scroller2">
                            <div class="category-div">
                                <ul id="subCategoryList">
                                </ul>
                            </div>

                            <%--<div class="category_goods" >
                                <h4>品类推荐</h4>
                                 <ul id="categoodsList">
                                </ul>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <uc1:uc_footer runat="server" ID="uc_footer" />

    </div>
</body>
</html>
