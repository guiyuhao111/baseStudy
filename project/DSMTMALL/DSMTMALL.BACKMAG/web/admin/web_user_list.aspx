<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="web_user_list.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.admin.web_user_list" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>
<%@ Register Src="~/web/uc/uc_upload.ascx" TagPrefix="uc1" TagName="uc_upload" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">用户列表</li>
                </ol>
                <div class="wrapper">
                    <div class="form-inline pull-right">
                        <button runat="server" id="uploadExcels" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#uploadExcelModal" mydata="uploadKouKuanExcel" onclick="ShowUploadExel(this)">
                            <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>&nbsp;导入
                        </button>
                        <div class="input-group input-group-sm">
                            <span class="input-group-addon">账户类型</span>
                            <select id="sltByUserType" runat="server" class="form-control input-sm" mydata="byUserType">
                                <option value="">全部</option>
                                <option value="0">企业账户</option>
                                <option value="1">个人账户</option>
                            </select>
                        </div>
                        <div class="input-group input-group-sm">
                            <input type="text" runat="server" id="iptSearch" class="form-control input-sm" mydata="searchName" placeholder="请输入员工姓名/手机号/公司名" />
                            <span class="input-group-btn">
                                <button runat="server" id="btnSearch" type="button" class="btn btn-primary btn-sm" onclick="btnSearch_Click(this)">
                                    <span class="fa fa-search" aria-hidden="true"></span>&nbsp;搜索</button>
                            </span>
                        </div>
                    </div>
                    <h4 class="my-h4"><span>用户列表/今日注册人数:<strong id="register_count" runat="server"></strong></span></h4>
                    <table id="tableUserList" class="table table-striped table-condensed table-hover table-bordered">
                        <asp:Repeater ID="repeaterUserList" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr class="info">
                                        <th>序号</th>
                                        <th>用户昵称</th>
                                        <th>联系方式</th>
                                        <th>性别</th>
                                        <th>注册时间</th>
                                        <th>最后登陆时间</th>
                                        <th>所属公司</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("RowNo") %></td>
                                    <td name="tName"><%# Eval("UserName")%>-<%# Eval("NickName")%></td>
                                    <td name="tPhoen"><%# Eval("Phone")%></td>
                                    <td name="tSex"><%# Eval("NSex")%></td>
                                    <td name="tAddTime"><%# Eval("RegTime") %></td>
                                    <td name="tLoginTime"><%# Eval("LastLogin") %></td>
                                    <td name="tCpyName"><%# Eval("CpyName") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                    <div runat="server" id="pageNav">
                    </div>
                </div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
        <div>
            <uc1:uc_confirm runat="server" ID="uc_confirm" />
            <uc1:uc_upload runat="server" ID="uc_upload" />
        </div>
    </section>
</body>


</html>
