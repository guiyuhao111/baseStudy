<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="order_pay.aspx.cs" Inherits="DSMTMALL.web.order_pay" %>

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
        <div id="divOrderPayPage" data-role="page" style="font-family: 'Microsoft YaHei'">
            <div data-role="header" data-tap-toggle="false" data-theme="j" style="margin: 0px; padding: 0px; border: none; border-bottom-color: #e1dfdf; border-bottom-width: 0.5px; border-bottom-style: solid;">
                <table style="width: 100%; height: 44px">
                    <tr>
                        <td style="width: 10%">
                            <a href="#" data-rel="back">
                                <img src="../images/web/hpage/rightarr.png" style="height: 20px; display: block" />
                            </a></td>
                        <td style="width: 90%; text-align: center; padding-right: 20px; font-weight: 400">
                            <span style="float: left; width: 100%">51IPC收银台
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
            <div data-role="concent" id="btnContent" class="content-marginleft" style="background-color: transparent">
                <div class="alipay-amount">订单金额 <span id="orderAmount" style="color: #ff0000; float: right; padding-right: 20px">元</span></div>

                <div class="alipay-pay-type">
                    <ul data-role="listview">
                        <li data-role="list-divider">支付方式</li>
                        <li><a href="#">卡余额付款</a></li>
                        <li><a href="/web/ali_pay.aspx?orderID=">支付宝付款</a></li>
                    </ul>
                </div>


            </div>






        </div>


    </form>
</body>
</html>
