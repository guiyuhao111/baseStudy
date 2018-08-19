<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="supplier_list.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.database.supplier_list" %>

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
    <title></title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script src="../../js/web/Suppliers.js"></script>
</head>

<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">供应商列表</li>
                </ol>
            </div>
            <div class="wrapper">
                <div class="form-inline pull-right">
                    <button runat="server" id="addNewModal" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#addModel">
                        <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>&nbsp;供应商添加
                    </button>
                    <div class="input-group input-group-sm" style="display:none">
                        <input type="text" runat="server" id="iptSearch" class="form-control input-sm" mydata="searchName" placeholder="请输入供应商名称" />
                        <span class="input-group-btn">
                            <button runat="server" id="btnSearch" type="button" class="btn btn-primary btn-sm" onclick="btnSearch_Click(this)">
                                <span class="fa fa-search" aria-hidden="true"></span>&nbsp;
                            </button>
                        </span>
                    </div>
                </div>

                <div id="divTable">
                    <h4 class="my-h4">供应商列表</h4>
                    <table id="tableSuppliersList" class="table table-striped table-condensed table-hover table-bordered">
                        <asp:Repeater ID="repeaterSuppliersList" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr class="info">
                                        <th>供应商编号</th>
                                        <th>供应商名称</th>
                                        <th>供应商描述</th>
                                        <th>是否启用</th>
                                        <th>系统操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td name="tUrl"><%# Eval("SuppliersID") %></td>
                                    <td name="tName" title="<%# Eval("SuppliersName") %>"><%# Eval("SuppliersName") %></td>
                                    <td name="tOrderBy" class="my-overflow-title">
                                        <span onclick="ChangeText(this,'suppliersDesc')"><%# Eval("SuppliersDesc") %></span></td>
                                    <td name="tIsShow" mydata="<%# Eval("IsCheck") %>">
                                        <input type="checkbox" name="isCheckEnable" onchange="ChangeCheckBox(this,'suppliersIsEnable')"></td>
                                    <td name="tOperation" mydata="<%# Eval("SuppliersID") %>" style="width: 80px">
                                        <div class="btn-group btn-group-sm">
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要删除吗？','deleteSuppliers')"><span class="fa fa-trash-o"></span></button>
                                        </div>
                                    </td>
                                    <td name="tID" hidden="hidden"><%# Eval("SuppliersID") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                    <div runat="server" id="pageNav"></div>
                </div>
            </div> <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
       

        <div id="divHelper">
            <uc1:uc_upload runat="server" ID="uc_upload" />
            <uc1:uc_confirm runat="server" ID="uc_confirm" />
            <div class="modal fade" id="addModel" tabindex="-1" role="dialog" aria-labelledby="addStatusModel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">添加供应商</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label for="newSuppliersNo">供应商编号：</label>
                                        <input type="text" id="newSuppliersNo" class="form-control input-sm" placeholder="eg：101  " maxlength="25" />
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label for="newSuppliersName">供应商名称：</label>
                                        <input type="text" id="newSuppliersName" class="form-control input-sm" placeholder="eg:新的供应商名称  " maxlength="60" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="AddSuppliers(this)" value="添加" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    </section>
</body>
</html>
