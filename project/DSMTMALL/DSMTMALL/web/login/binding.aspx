<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="binding.aspx.cs" Inherits="DSMTMALL.web.login.binding" %>
<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body style="max-height:90%">
    <form id="form1" runat="server">
        <div id="devBindingPage" data-role="page" style="background-image: url('/css/images/account_bg.jpg'); background-size: 100% 100%;">
            <script src="../../js/html/login/binding.js"></script>
            <div data-role="header" data-theme="j" style="border:none">
                <span id="uOpenID" runat="server" hidden="hidden"></span>
            </div>

            <%--头部结束--%>
            <div data-role="concent" id="btnContent" style="background-color: transparent; padding: 15% 5% 0% 5%; height: 100%; ">
                <div class="title-logo">
                    <img src="/css/images/51ipc_buy.png" />
                </div>
                <div id="bindingDiv">
                    <div id="openIDIsRegister" class="register-telephone">
                        <table>
                            <tr>
                                <td>+86</td>
                                <td>
                                    <input id="telephoneNo" type="tel" data-role="none" runat="server" maxlength="11" style="width: 60%" /><span mydata="0" id="toSubmitCode">发送</span></td>
                            </tr>
                            <tr>
                                <td>验证码</td>
                                <td>
                                    <input id="telephoneCode" type="tel" data-role="none" maxlength="6" /></td>
                            </tr>
                        </table>
                        <div class="register-btn" data-role="none">
                            <button id="toSubmitBinding">新账户绑定<span id="telephoneUserName" runat="server" mydata=""></span></button>
                        </div>
                        <div class="telephone-code" id="showTelInfo" hidden="hidden" data-theme="j"></div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
