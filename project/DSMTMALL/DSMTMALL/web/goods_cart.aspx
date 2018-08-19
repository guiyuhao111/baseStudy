<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="goods_cart.aspx.cs" Inherits="DSMTMALL.web.goods_cart" %>
<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc_show.ascx" TagPrefix="uc1" TagName="uc_show" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="divCartList" data-role="page">
            <script src="/js/html/goods_cart.js"></script>
            <div data-role="header" data-theme="j" data-tap-toggle="false">
                <div class="header-back-g" >
                    <a data-rel="back">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span>购物车</span>
                </div>
                <span id="userID" runat="server" hidden="hidden"></span>
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
            <%--头部结束--%>
            <div data-role="concent" id="btnContent" style="background-color: transparent">
                <ul id="ulUserCart" data-role="listview">
                </ul>
               
                <div id="divNoRecord" class="divNoRecord toHide">
                    <img alt="norecord" src="/images/web/icon_norecord.png" />
                    <p id="pNoRecord"></p>
                </div>
            </div>

            <div data-role="footer" id="btnFooter" data-position="fixed" style="overflow: hidden;" data-tap-toggle="false">
                <div data-role="navbar">
                    <div class="my-footer-submit">
                        <ul style="background-color: #fff">
                            <li style="width: 60%">
                                <div class="cart-totalprice">合计(不含运费)：<em style="font-style: normal">&yen;</em><em id="emCartTotalPrice" style="font-style: normal">0.00</em></div>
                            </li>
                            <li style="width: 40%">
                               <a id="toSubmitOrder"  style="line-height: 35px; background-color: #2AA9DC; color: #ffffff; text-shadow: none; font-weight: 100;">去结算</a>
                            </li>
                        </ul>
                    </div>
                    <div data-role="navbar" class="footer_css">
                        <ul>
                            <li><a id="btnFooterOne" href="/web/mall_index.aspx" data-transition="slide" data-direction="reverse">
                                <img src="../images/web/hpage/index1.png" /><span>首页</span></a></li>
                            <li><a id="btnFooterTwo" href="/web/category_list.aspx" data-transition="slide" data-direction="reverse">
                                <img src="../images/web/hpage/category_1.png" /><span>分类</span></a></li>
                            <li><a id="btnFooterThree" href="/web/goods_cart.aspx" data-transition="slide">
                                <img src="../images/web/hpage/cart_1.png" /><span>购物车</span></a></li>
                            <li><a id="btnFooterFour" href="/web/user_homepage.aspx" data-transition="slide">
                                <img src="../images/web/hpage/myhome1.png" /><span>我的</span></a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
