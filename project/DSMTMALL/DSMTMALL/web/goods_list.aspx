<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="goods_list.aspx.cs" Inherits="DSMTMALL.web.goods_list" %>

<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_header_part.ascx" TagPrefix="uc1" TagName="uc_header_part" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />

</head>
<body>
        <div id="divPageGoodsList" data-role="page" data-dom-cache="true" data-theme="a">
            <script src="/js/html/goods_list.js"></script>
            <div data-role="header" data-tap-toggle="false" data-position="fixed" data-theme="j">
                <uc1:uc_header_part runat="server" id="uc_header_part" />
                <div data-role="navbar" data-iconpos="right" style="padding: 0px; margin: 0px;">
                    <ul class="gl_font">
                        <li><a id="btnSortSaleDown" href="#" class="item-btn-active" data-icon="carat-d" style="font-size: 16px; text-shadow: none;">热荐</a></li>
                        <li><a id="btnSortPriceUp" href="#" data-icon="carat-d" style="font-size: 16px; text-shadow: none; font-weight: 500">销量</a></li>
                        <li><a id="btnSortPriceDown" href="#" data-icon="carat-d" style="font-size: 16px; text-shadow: none">价格</a></li>
                    </ul>
                </div>
                <span hidden="hidden" id="searchName" runat="server"></span>
            </div>


            <div data-role="content" data-theme="j" style="background-color: transparent">
                <div id="ulGoodsList" class="ui-grid-a"></div>
                <div id="divNoRecord" class="divNoRecord toHide">
                    <img alt="norecord" src="/images/web/icon_norecord.png" />
                    <p id="pNoRecord"></p>
                </div>
            </div>
            <%--Content-End--%>
            <div data-role="footer" id="btnFooter" data-position="fixed" data-theme="j" data-tap-toggle="false">
                <div data-role="navbar" class="footer_css">
                    <ul>
                        <li><a id="btnFooterOne" href="/web/mall_index.aspx" data-transition="slide">
                            <img src="../images/web/hpage/index1.png" /><span>首页</span></a></li>
                        <li><a id="btnFooterTwo" href="/web/category_list.aspx" data-transition="slide">
                            <img src="../images/web/hpage/category_1.png" /><span>分类</span></a></li>
                        <li><a id="btnFooterThree" href="/web/goods_cart.aspx" data-transition="slide">
                            <img src="../images/web/hpage/cart_1.png" /><span>购物车</span></a></li>
                        <li><a id="btnFooterFour" href="/web/user_homepage.aspx" data-transition="slide">
                            <img src="../images/web/hpage/myhome1.png" /><span>我的</span></a></li>
                    </ul>
                </div>
            </div>
        </div>
</body>
</html>
