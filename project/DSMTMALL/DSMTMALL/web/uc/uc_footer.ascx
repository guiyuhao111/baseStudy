<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_footer.ascx.cs" Inherits="DSMTMALL.web.uc.uc_footer" %>
<div data-role="footer" id="btnFooter" data-position="fixed" data-tap-toggle="false">
    <div data-role="navbar" class="footer_css">
        <ul>
            <li><a id="btnFooterOne" href="/web/mall_index.aspx" data-transition="slide">
                <img src="../images/web/hpage/index1.png" onerror="nofindRoll()"/><span>首页</span></a></li>
            <li><a id="btnFooterTwo" href="/web/category_list.aspx" data-transition="slide">
                <img src="../images/web/hpage/category_1.png" onerror="nofindRoll()"/><span>分类</span></a></li>
            <li><a id="btnFooterThree" href="/web/goods_cart.aspx" data-transition="slide">
                <img src="../images/web/hpage/cart_1.png" onerror="nofindRoll()"/><span>购物车</span></a></li>
            <li><a id="btnFooterFour" href="/web/user_homepage.aspx" data-transition="slide">
                <img src="../images/web/hpage/myhome1.png" onerror="nofindRoll()"/><span>我的</span></a></li>
        </ul>
    </div>
</div>
