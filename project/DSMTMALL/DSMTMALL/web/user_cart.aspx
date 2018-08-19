<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_cart.aspx.cs" Inherits="DSMTMALL.web.user_cart" %>
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
        <div id="devUserCartPage" data-role="page" >
            <script src="/js/html/user_cart.js"></script>
            <div data-role="header" data-tap-toggle="false" data-theme="j" >
               <div class="header-back-g" >
                    <a data-rel="back">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span>确认订单</span>
                </div>
                <span id="userID" runat="server" hidden="hidden"></span>
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
            <%--头部结束--%>
            <div data-role="concent" id="btnContent"  class="content-marginleft" style="background-color: transparent">
                <div class="my-chose-address">
                    <table id="show" style="width: 100%">
                        <tr style="width: 100%">
                            <td style="width: 80%">
                                <div id="ulUserAddress">
                                </div>
                            </td>
                            <td class="jump-btn jump-btn-2" style="width: 20%;">
                                <span id="ulMoreAddress">
                                    <img src="../images/web/hpage/leftarr.png" />
                                </span>
                            </td>
                        </tr>
                    </table>
                    <ul id="ulUserCart" data-role="listview" class="cartlist-marginleft">
                    </ul>
                    <div id="divNoRecord" class="divNoRecord toHide">
                        <img alt="norecord" src="/images/web/icon_norecord.png" />
                        <p id="pNoRecord"></p>
                    </div>
                </div>
                <div id="divFeeAmount" class="cartlist-feeAmount">
                    <table>
                        <tr><td><span>商品金额</span><span class="fee-span"><em>&yen;</em><em id="emCartGoodsPrice"></em></span></td></tr>
                        <tr><td><span>运费</span><span class="fee-span"><i>+</i><em>&yen;</em><em id="emCartFeeAmount" runat="server"></em></span></td></tr>
                    </table>
                </div>
            </div>


            <div data-role="footer" id="btnFooter" data-position="fixed" style="margin: 0px; padding: 0px;">
                <div data-role="navbar">
                    <div class="my-footer-submit">
                        <ul>
                            <li style="width: 60%">
                                <div class="cart-totalprice">实付款：<em style="font-style:normal">&yen;</em><em id="emCartTotalPrice" style="font-style:normal"></em></div>
                            </li>
                            <li style="width: 40%;">
                                <a id="toSubmitOrder" href="/web/user_cart.aspx" style="line-height: 35px; background-color: #2AA9DC; color: #ffffff; text-shadow: none; font-weight: 100;">提交订单</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
