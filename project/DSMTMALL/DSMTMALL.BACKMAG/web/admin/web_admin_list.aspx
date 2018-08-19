<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="web_admin_list.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.admin.web_admin_list" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script src="../../js/web/AdminList.js"></script>
</head>

<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">账户列表</li>
                    <span class="li_a">
                        <a href="javascript:;" data-toggle="modal" data-target="#addModal">添加账户</a>
                        <a href="javascript:;" data-toggle="modal" data-target="#">添加模块</a>
                    </span>
                </ol>
                <h4 class="my-h4">账户列表</h4>
                <table id="tableAdminList" class="table table-striped table-condensed table-hover table-bordered">
                    <asp:Repeater ID="repeaterAdminList" runat="server">
                        <HeaderTemplate>
                            <thead>
                                <tr class="info">
                                    <th>账户名</th>
                                    <th>联系方式</th>
                                    <th>所属供应商</th>
                                    <th>创建时间</th>
                                    <th>最后登陆时间</th>
                                    <th>卡号</th>
                                    <th>所有模块</th>
                                    <th>启用</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="info-val">
                                <td name="tName"><span onclick="ChangeText(this,'updataAdminName')"><%# Eval("AdminName")%></span></td>
                                <td name="tPhone"><span onclick="ChangeText(this,'updataAdminPhone')"><%# Eval("Phone")%></span></td>
                                <td name="tName_1"><%# Eval("SuppliersName")%></td>
                                <td name="tAddTime"><%# Eval("CreateTime") %></td>
                                <td name="tLoginTime"><%# Eval("LoginTime") %></td>
                                <td name="tNumber"><%# Eval("TokenCardNo") %></td>
                                <td name="tList"><%# Eval("NModalList") %></td>
                                <td name="tIsEnable" mydata="<%# Eval("IsEnable") %>" class="width_3">
                                    <input type="checkbox" name="isCheckEnable" onchange="ChangeCheckBox(this,'adminIsEnable')">
                                </td>
                                <td name="tOperation" mydata="<%# Eval("AdminID") %>">
                                    <div class="btn-group btn-group-sm">
                                        <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#assignCardModal" mydata="<%# Eval("AdminID") %>" onclick="assignCard(this)"><span class="fa fa-id-card-o"></span></button>
                                        <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要删除吗？','deleteAdmin')"><span class="fa fa-trash"></span></button>
                                        <button type="button" class="btn btn-success btn-sm" data-toggle="modal" data-target="#assignModal" mydata="<%# Eval("AdminID") %>" onclick="ShowAssignModal(this)"><span class="fa fa-delicious"></span></button>
                                        <button type="button" class="btn btn-danger btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要重置密码吗？','updataAdminPwd')"><span class="fa fa-key"></span></button>
                                    </div>
                                </td>
                                <td hidden="hidden" name="tID"><%# Eval("AdminID") %></td>
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
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>

        <div>
            <uc1:uc_confirm runat="server" ID="uc_confirm" />
            <div class="modal fade" id="assignCardModal" tabindex="-1" role="dialog" aria-labelledby="assignModal" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <img id="showimage" src="/images/web/swipeCard.gif" style="width: 100%;" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="addModal" tabindex="-1" role="dialog" aria-labelledby="addModal" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">添加账户</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addAdminName">账户名称：</label>
                                        <input type="text" class="form-control" id="addAdminName" placeholder="请输入商品名称..." />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addAdminPhone">联系方式：</label>
                                        <input type="text" class="form-control" id="addAdminPhone" placeholder="请输入商品编号..." />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label for="addSuppliers">供应商</label>
                                        <select runat="server" id="addSuppliers" class="form-control input-sm">
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="addRemark">备注信息</label>
                                        <textarea id="addRemark" class="form-control" runat="server" rows="5" maxlength="255"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="AddAdminAmount(this)" value="确认" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="assignModal" tabindex="-1" role="dialog" aria-labelledby="assignModal" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">模块权限分配</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row" id="chooseModalInfo">
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="AssignModal(this)" value="确认" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</body>

</html>
