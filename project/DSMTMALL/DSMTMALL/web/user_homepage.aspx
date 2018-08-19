<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_homepage.aspx.cs" Inherits="DSMTMALL.web.user_homepage" %>

<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="divHomePage" data-role="page">
            <span id="userID" runat="server" hidden="hidden"></span>
            <script src="/js/html/homepage.js"></script>
            <div data-role="header" data-position="fixed" data-tap-toggle="false" class="my-header-color">
                <div class="homeTable fontxiti">
                    <table>
                        <tr>
                            <td>
                                <img id="imgUrl" runat="server" class="homepage-img" src="../images/web/hpage/user-head.png" onerror="this.src='/images/web/hpage/user-head.png'" style="border-radius: 50%" />
                            </td>
                            <td>
                                <table class="tab_2">
                                    <tr>
                                        <td id="tdUserName" runat="server">用户名
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a href="/web/user_address.aspx" data-transition="slide">我的收货地址&gt; </a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <span><a href="/web/user_home.aspx" data-transition="slide">账户管理<strong>></strong></a></span>
                </div>
            </div>
            <div data-role="content" class="homecontent" style="width: 100%;" data-theme="z">
                <ul data-role="listview" style="margin: 0px; padding: 0px;" class="homelistview">
                    <li>
                        <a href="/web/user_order.aspx?orderType=obligation" data-transition="slide">
                            <img src="../images/web/hpage/payment.png" />
                            <span>待付款</span>
                        </a>
                    </li>
                    <li>
                        <a href="/web/user_order.aspx?orderType=TRS" data-transition="slide">
                            <img src="../images/web/hpage/takegoods.png" />
                            <span>待收货</span>
                        </a>
                    </li>
                    <li>
                        <a href="/web/user_order.aspx" data-transition="slide">
                            <img src="../images/web/hpage/order.png" />
                            <span>我的订单</span>
                        </a>
                    </li>
                    <%--  <li>
                         <a target="_blank" href="mqqwpa://im/chat?chat_type=wpa&uin=757102006&version=1&src_type=web&web_src=oicqzone.com">
                            <img src="../images/web/hpage/aftersale.png" />
                            <span>售后服务</span>
                        </a>
                    </li>--%>
                    <li>
                        <a href="tel:0574-83888011" style="line-height: 30px">
                            <img src="../images/web/hpage/custservice.png" />
                            <span>客服联系</span>
                        </a>
                    </li>
                </ul>

                <div id="newGoodsList" style="margin-top:10px;">
                    <div class="mallIndex-nav">
                        <span style="background-image: url(../images/web/hpage/cpychoose.png); background-repeat: no-repeat; background-size: contain;">新品推荐</span>
                    </div>
                    <div id="gridNewGoodsList" class="ui-grid-a  myTopGoodsList" style="margin-left: auto; margin-right: auto"></div>
                    <div id="divNoRecord" class="divNoRecord toHide">
                        <img alt="norecord" src="/images/web/icon_norecord.png" />
                        <p id="pNoRecord"></p>
                    </div>
                </div>
            </div>
            <%--Content-End--%>
            <uc1:uc_footer runat="server" ID="uc_footer" />

        </div>


    </form>
</body>
</html>
