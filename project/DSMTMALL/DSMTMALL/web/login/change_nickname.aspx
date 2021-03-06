﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="change_nickname.aspx.cs" Inherits="DSMTMALL.web.login.change_nickname" %>


<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc_show.ascx" TagPrefix="uc1" TagName="uc_show" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="divChangeNickNamePage" data-theme="j">
            <script src="../../js/html/login/user_home.js"></script>
            <div data-role="header" data-theme="j">
                <div class="header-back-g">
                    <a href="/web/user_home.aspx" data-transition="slide">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span>更换昵称</span>
                </div>
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
            <div data-role="content">
                <div class="change-phone">
                    <div class="change-phone-input">
                        <input id="newNickName" type="text" /></div>
                    <a data-role="button" href="javascript:;" onclick="ChangeNickName()"  data-theme="z" >提交</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
