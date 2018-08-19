<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fareDeliery_list.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.database.fareDeliery_list" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script src="../../js/web/faredelieryList.js"></script>
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">运费模板</li>
                    <li>
                        <a href="fare_editor.aspx" style="color: red">+新增模板</a>
                    </li>
                </ol>
            </div>
            <div class="wrapper">
                <asp:Repeater ID="repeaterFareList" runat="server">
                    <ItemTemplate>
                        <div class="fare-table-list">
                            <table class="table table-striped table-condensed table-hover table-bordered fare-table">
                                <caption class="fare-title" mydata="<%# Eval("FareSysID") %>">
                                    <%# Eval("FareName") %>
                                    <div><font>最后修改时间：<%# Eval("UpdateTime") %></font><a href="fare_editor.aspx?id=<%# Eval("FareSysID") %>">修改</a>&nbsp<a onclick="DeleteFareTemp(this)">删除</a></div>
                                </caption>
                                <tr class="info">
                                    <th>运送方式</th>
                                    <th>运送地址</th>
                                    <th>首件（个）</th>
                                    <th>运费（元）</th>
                                    <th>续件（个）</th>
                                    <th>运费（元）</th>
                                    <th hidden="hidden" runat="server" name="fareSysID"><%# Eval("FareSysID") %></th>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>

                </asp:Repeater>
                <div runat="server" id="pageNav"></div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
    </section>
</body>
</html>
