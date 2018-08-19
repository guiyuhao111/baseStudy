<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="order_return.aspx.cs" Inherits="DSMTMALL.web.order.order_return" %>

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
        <div id="devOrderReturnPage" data-role="page" style="font-family: 'Microsoft YaHei'">
            <script src="../../js/html/order/order_return.js"></script>
            <div data-role="header" class="item-header">
                <div class="header-back">
                    <a href="/web/user_order.aspx" data-transition="slide">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <h4>交易成功</h4>
                </div>
                <span id="userID" runat="server" hidden="hidden"></span>
                <uc1:uc_show runat="server" ID="uc_show1" />
                <div id="divPromptBar" class="divPromptBar toHide" onclick="HidePromptBar()">
                    <h2>温馨提示！</h2>
                    <p id="pPromptBar"></p>
                </div>
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
            <%--头部结束--%>
            <div data-role="concent" id="btnContent" class="content-marginleft" style="background-color: transparent">

                <div >
                    <img id="imgShow" src="../../images/web/hpage/paysuccess.png" style="width: 100%" />
                </div>
                
            </div>
        </div>
    </form>
</body>
</html>
