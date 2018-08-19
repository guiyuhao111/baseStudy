<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_order.aspx.cs" Inherits="DSMTMALL.web.user_order" %>

<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc_show.ascx" TagPrefix="uc1" TagName="uc_show" %>
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
        <div id="divPageUserOrder" data-role="page">
            <script src="/js/html/order_list.js"></script>
            <div data-role="header" data-tap-toggle="false" data-theme="j">
                <div class="header-back-g">
                    <a href="/web/user_homepage.aspx" data-transition="slide" >
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span id="orderTypeName">我的订单</span>
                </div>
                <span id="userID" runat="server" hidden="hidden"></span>

                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
            <div data-role="content" class="order-list-ui">
                <div id="ulUserOrderList" >
              
                </div>
                <div id="divNoRecord" class="divNoRecord toHide">
                    <img alt="norecord" src="/images/web/icon_norecord.png" />
                    <p id="pNoRecord"></p>
                </div>
                <div id="divShowboxBack" data-theme="j" class="showbox-back">
                    <div id="divShowboxBox" data-theme="j" class="showbox-box">
                        <table class="showbox-table">
                            <tr>
                                <td class="showbox-td-left">
                                    <img alt="imgShowbox" id="imgShowbox" src="javascript:;" /></td>
                                <td class="showbox-td-right">
                                    <p id="pShowbox"></p>
                                </td>
                            </tr>
                        </table>
                        <ul class="showbox-box-ul">
                            <li><a id="btnShowboxLeft" href="javascript:;"></a></li>
                            <li><a id="btnShowboxRight" href="javascript:;" onclick="divShowboxHide_Click()"></a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
    </form>
</body>
</html>
