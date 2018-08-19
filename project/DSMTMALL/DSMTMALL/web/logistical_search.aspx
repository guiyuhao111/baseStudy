<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logistical_search.aspx.cs" Inherits="DSMTMALL.web.logistical_search" %>
<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc_show.ascx" TagPrefix="uc1" TagName="uc_show" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物中心</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="divUserHomePage" data-role="page" >
            <script src="../js/html/login/user_home.js"></script>
            <div data-role="header" data-theme="j" class="item-header-g">
                <div class="header-back-g">
                    <a href="javascript:;" data-rel="back" data-transition="slide">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span>物流查询</span>
                </div>
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
           <div data-role="content">
               <ul class="ul-logistical" id="showLogistical" runat="server">
                   <li style="width:100%;text-align:center">暂时无法获取到该订单的物流信息</li>
               </ul>
           </div>
        </div>
    </form>
</body>
</html>
