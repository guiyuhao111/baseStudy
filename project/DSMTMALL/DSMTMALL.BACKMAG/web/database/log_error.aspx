<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="log_error.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.database.log_error" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_upload.ascx" TagPrefix="uc1" TagName="uc_upload" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>


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
                        <li class="active">错误日志</li>
                    </ol>  
                </div>
            <div class="wrapper">
                    <div id="divTable">
                        <h4 class="my-h4">日志列表</h4>
                        <table id="tableErrLogList" class="table table-striped table-condensed table-hover table-bordered">
                            <asp:Repeater ID="repeaterErrLogList" runat="server">
                                <HeaderTemplate>
                                    <thead>
                                        <tr class="info">
                                            <th>序号</th>
                                            <th>错误名称</th>
                                            <th>错误详情</th>
                                            <th>发生时间</th>
                                            <th>系统操作</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ErrorLogID") %></td>
                                        <td name="tID_1"><%# Eval("ErrLogName")%></td>
                                        <td name="tName"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus"  data-content=" <%# Eval("ErrLogDesc")%>"> <%# Eval("ErrLogDesc")%></a></td>
                                        <td><%# Eval("ErrLogTime")%></td>
                                        <td name="tOperation" mydata="<%# Eval("ErrorLogID") %>">
                                            <div class="btn-group btn-group-sm">
                                                <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要删除吗？','deleteErrLog')"><span class="fa fa-trash-o"></span></button>
                                            </div>
                                        </td>
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
                </div>   <uc1:uc_footer runat="server" ID="uc_footer" />
            </div>
        <div id="divHelper">
            <uc1:uc_upload runat="server" ID="uc_upload" />
            <uc1:uc_confirm runat="server" ID="uc_confirm" />
        </div>
    </section>
</body>
</html>
