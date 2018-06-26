<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pay_center.aspx.cs" Inherits="DSMTMALL.web.payment.pay_center" %>

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
        <div id="devPayCenterPage" data-role="page" style="font-family: 'Microsoft YaHei'">
            <script src="/js/html/payment/pay_center.js"></script>
            <div data-role="header"  class="item-header">
                <div class="header-back">
                    <a href="/web/user_order.aspx" data-transition="slide">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <h4>51IPC结算中心</h4>
                </div>
                <uc1:uc_show runat="server" ID="uc_show" />
              
            </div>
            <%--头部结束--%>
            <div data-role="concent" id="btnContent" class="content-marginleft" style="background-color: transparent">

                <div id="divGoodsAmount" class="cartlist-payAmount">
                    <table>
                        <tr>
                            <td><span>订单金额</span><span class="fee-span"><em id="emCartGoodsPrice" runat="server"></em>元</span></td>
                        </tr>
                    </table>
                </div>
                <div id="divFeeAmount" class="cartlist-payAmount">
                    <table>
                        <tr>
                            <td><span>运费价格</span><span class="fee-span">+<em id="emFeeAmount" runat="server"></em>元</span></td>
                        </tr>
                    </table>
                </div>
                <div id="divOrderAmount" class="cartlist-payAmount">
                    <table>
                        <tr>
                            <td><span>实际支付</span><span class="fee-span"><em id="emCartOrderAmount" runat="server"></em>元</span></td>
                        </tr>
                    </table>
                </div>
                <div id="divPhoneCode" class="cartlist-payAmount">
                    <table>
                        <tr>
                            <td><span id="toSubmitCode" mydata="0"style="border:1px solid #ff0000;padding:5px" >点击获取验证码</span><span class="fee-span"><input id="telCode" type="tel" maxlength="6" placeholder="验证码"  style="width:100px;"/></span></td>
                        </tr>
                    </table>
                </div>
                <div id="cardBalance" class="cartlist-balance"  >
                    <table>
                        <tr >
                            <td><span>可用余额</span><span class="fee-span"><em id="emCartBanlance" runat="server"></em>元</span></td>
                        </tr>
                        <tr style="border-bottom: none;">
                            <td><span class="cardpay" id="toSubmitCardBanlance" runat="server">卡余额支付</span></td>
                        </tr>
                    </table>
                </div>

                <div id="antherPay" class="cartlist-anther" hidden="hidden">
                    <table>
                        <tr>
                            <td><span>其它支付方式</span></td>
                        </tr>
                        <tr>
                            <td><a>微信支付<span><img src="../../images/web/hpage/leftarr.png" /></span></a></td>
                        </tr>
                    </table>
                </div>

            </div>

        </div>
    </form>
</body>
</html>
