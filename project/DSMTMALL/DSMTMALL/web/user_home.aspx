<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_home.aspx.cs" Inherits="DSMTMALL.web.user_home" %>
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
                    <a href="/web/user_homepage.aspx" data-transition="slide">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span>个人资料</span>
                </div>
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
           <div data-role="content" class="ul-user-home">
               <ul>
                    <li><span>个人头像</span><div class="li-user-img"><img id="imgUrl" runat="server" src="../images/web/hpage/user-head.png" onerror="this.src='/images/web/hpage/user-head.png'" /></div></li>
                    <li><span>账户名</span><div id="realName" runat="server"> </div></li>
                    <li><span>昵称</span><div><i id="nickName" runat="server"></i> <span><a href="/web/login/change_nickname.aspx" data-transition="slide"><img src="../images/web/hpage/leftarr.png" /></a></span></div></li>
                    <li><span>收货地址</span><div><a href="/web/user_address.aspx" data-transition="slide"><span><img src="../images/web/hpage/leftarr.png" /></span></a></div></li>
                    <li><span>可用余额</span><div id="emCartBanlance" runat="server"> </div></li>
                   <%-- <li><a data-role="button" href="javascript:void;" onclick="UnwrapPhone()" data-transition="slide">解绑手机</a></li>--%>
                    <li><a data-role="button" href="/web/login/change_phone.aspx" data-transition="slide">换绑手机</a></li>
               </ul>
           </div>
        </div>
    </form>
</body>
</html>
