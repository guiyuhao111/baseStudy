<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_login.aspx.cs" Inherits="DSMTMALL.web.user_login" %>

<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body style="max-height: 80%">
    <form id="form1" runat="server">
        <div id="devLoginPage" data-role="page" class="my-default">
            <div style="width: 100%; margin: 0px; padding: 0px;">
                <div>
                    <img src="../images/web/hpage/SessionTimeout.png" style="padding-top: 40%; width: 100%" />
                </div>
            </div>
            <script src="/js/html/user_login.js"></script>


           <%-- <div data-role="header" data-tap-toggle="false" data-theme="j">
                <div class="header-back-g">
                    <a data-rel="back">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span>登录</span>
                </div>
                <span id="userID" runat="server" hidden="hidden"></span>
            </div>
            <div data-role="content" data-theme="c" class="my-font-default">
                <div data-role="fieldcontain">
                    <span>
                        <label for="loginPhone"></label>
                        <input type="text" id="loginPhone" placeholder="手机号码..." />
                    </span>
                    <span>
                        <label for="loginpwd"></label>
                        <input type="password" id="loginpwd" placeholder="账户密码..." />
                    </span>
                </div>
                <a href="#" id="btnSubmitLogin" class="ui-btn">登录</a>

                <div style="color: red; font-size: 14px;">提示：如果不知道账户名与密码</div>
                <div style="color: red; font-size: 14px;">请关闭页面重新使用微信登陆</div>
            </div>--%>



        </div>
    </form>
</body>
</html>
