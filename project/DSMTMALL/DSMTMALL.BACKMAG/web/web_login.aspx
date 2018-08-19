<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="web_login.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.web_login" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_meta.ascx" TagPrefix="uc1" TagName="uc_meta" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <uc1:uc_meta runat="server" ID="uc_meta" />
    <title>后台管理系统</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script src="../js/web/WebLogin.js"></script>
</head>
<body class="login-body">
    <div class="container">
        <form class="form-signin" >
            <div class="form-signin-heading text-center">
                <h1 class="sign-title"> </h1>
                <img src="/images/web/login-logo.png" />
            </div>
            <div class="login-wrap">
                <div id="myLogin">
                    <fieldset>
                        <input type="text" class="form-control" placeholder="User ID" id="iptAdminName" required="required" />
                    </fieldset>
                    <fieldset>
                        <input type="password" class="form-control" id="iptAdminPwd" placeholder="Password" required="required" /></fieldset>
                    <button class="btn btn-lg btn-login btn-block" id="btnAdminLogin" onclick="AdminLogin(this)">
                        <i class="fa fa-street-view"></i>
                    </button>
                    <div class="registration">
                        没有账户?
                        <a href="javascript:;">注册</a>
                    </div>
                    <label class="checkbox">
                        <span class="pull-right">
                            <a data-toggle="modal" href="#myModal">忘记密码?</a>
                        </span>
                    </label>
                </div>
                <div id="showImg" style="display: none">
                    <img src="/images/web/swipeCard.gif" style="width: 95%" />
                </div>
            </div>
        </form>

    </div>
</body>
</html>
