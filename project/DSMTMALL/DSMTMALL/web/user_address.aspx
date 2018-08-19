<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_address.aspx.cs" Inherits="DSMTMALL.web.user_address" %>

<%@ Register Src="/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="/web/uc_show.ascx" TagPrefix="uc1" TagName="uc_show" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="divAddressPage" data-role="page" data-position="fixed">
            <script src="/js/html/user_address.js"></script>
             <div data-role="header" data-tap-toggle="false" data-theme="j" class="item-header-g">
               <div class="header-back-g" >
                    <a data-rel="back">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span>收货地址</span>
                </div>
                <span id="userID" runat="server" hidden="hidden"></span>
              
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
            <div data-role="content" id="btnContent" class="content-css">

                <div class="my-address">
                    <ul id="ulUserAddressList" data-role="listview" class="cartlist-marginleft">
                    </ul>
                </div>
            </div>

            <div data-role="footer" id="btnFooter" data-position="fixed" >
                <div style="margin-right:auto;margin-left:auto"></div>
                <div style="margin-right:auto;margin-left:auto">
                <a href="/web/user_address_editor.aspx" data-role="button" style="width: 80%;margin-right:auto;margin-left:auto;display:block;background-color:#2b90f1">
                    <span style="line-height:35px; color:#ffffff;text-shadow:none; font-weight:100;">+ 新建地址</span>
                </a></div>
                <div style="margin-right:auto;margin-left:auto"></div>
            </div>
        </div>
    </form>
</body>
</html>
