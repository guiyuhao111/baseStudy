<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="web_admin_pwd.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.admin.web_admin_pwd" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>密码修改</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script src="/js/web/WebAdminPwd.js"></script>
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>系统设置</li>
                    <li class="active">修改密码</li>
                </ol>
                </div>
            <div class="wrapper">
                <div class="admin_pwd_row">
                    <div class="row">
                        <div class="col-xs-6">
                            <div class="row">
                                <div class="col-xs-6 margin-bottom-10">
                                    <label for="adminPwd" class="control-label">旧密码：</label>
                                    <input type="password" id="adminPwd" class="form-control input-sm" placeholder="请输入旧密码" maxlength="20" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6">
                            <div class="row">
                                <div class="col-xs-6 margin-bottom-10">
                                    <label for="newPwd" class="control-label">新密码：</label>
                                    <input type="password" id="newPwd" class="form-control input-sm" placeholder="请输入新密码" maxlength="20" />
                                </div>
                                <div class="col-xs-6 margin-bottom-10">
                                    <label for="againPwd" class="control-label">确认新密码：</label>
                                    <input type="password" id="againPwd" class="form-control input-sm" placeholder="请再次输入新密码" maxlength="20" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <div class="col-xs-12">
                            <button type="button" class="btn btn-primary btn-sm" onclick="UpdatePwd(this)">修改</button>
                            <button type="reset" class="btn btn-default btn-sm" onclick="ResetPwd()">重置</button>
                        </div>
                    </div>
                </div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
        <uc1:uc_confirm runat="server" ID="uc_confirm" />
    </section>
</body>
</html>



