<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="roll.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.database.roll" %>

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
    <script src="../../js/web/Roll.js"></script>
</head>

<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                 <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">轮播列表</li>
                </ol>
            </div>
            <div class="wrapper">
                <div class="form-inline pull-right">
                    <button runat="server" id="addNewModal" type="button" class="btn btn-primary btn-sm" onclick="AddRoll(this)">
                        <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>&nbsp;一键追加
                    </button>
                </div>
                <div id="divTable">
                    <h4 class="my-h4">轮播图列表</h4>
                    <table id="tableRollList" class="table table-striped table-condensed table-hover table-bordered">
                        <asp:Repeater ID="repeaterRollList" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr class="info">
                                        <th>轮播序号</th>
                                        <th>导向商品</th>
                                        <th>商品图片</th>
                                        <th>轮播类型</th>
                                        <th>轮播图片</th>
                                        <th>是否启用</th>
                                        <th>优先排序</th>
                                        <th>创建时间</th>
                                        <th>系统操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td name="tID_1" mydata="<%# Eval("RollSysID") %>"><%# Eval("RollSysID")%></td>
                                    <td name="tName" mydata="<%# Eval("TargetSysID") %>"><%# Eval("NName")%></td>
                                    <td name="tImg_1" class="tb-img" mydata="<%# Eval("RollSysID") %>">
                                        <img src="<%# Eval("NIMG") %>" onerror="nofind()" />
                                    </td>
                                    <td name="tType"><%# Eval("NRollType")%></td>
                                    <td name="tImg" class="tb-img" mydata="<%# Eval("RollSysID") %>">
                                        <a href="javascript:;" data-toggle="modal" data-target="#uploadPicture" mydata="rollPicture" onclick="ShowUploadPicture(this)">
                                            <img src="<%# Eval("Picture") %>" onerror="nofind()" />
                                        </a>
                                    </td>
                                    <td name="tIsEnable" mydata="<%# Eval("IsEnable") %>">
                                        <input type="checkbox" name="isCheckEnable" onchange="ChangeCheckBox(this,'rollIsEnable')"></td>
                                    <td name="tOrderBy" class="my-overflow-title">
                                        <span onclick="ChangeText(this,'rollOrderBy')"><%# Eval("OrderBy") %></span></td>
                                    <td><%# Eval("CreateTime")%></td>
                                    <td name="tOperation" mydata="<%# Eval("RollSysID") %>">
                                        <div class="btn-group btn-group-sm">
                                            <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#updateModel" onclick="ShowUpdateRoll(this)"><span class="fa fa-pencil-square-o"></span></button>
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要删除吗？','deleteRoll')"><span class="fa fa-trash-o"></span></button>
                                        </div>
                                    </td>
                                    <td name="tID" hidden="hidden"><%# Eval("RollSysID") %></td>
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
        <div id="divHelper">
            <uc1:uc_upload runat="server" ID="uc_upload" />
            <uc1:uc_confirm runat="server" ID="uc_confirm" />
            <div class="modal fade" id="updateModel" tabindex="-1" role="dialog" aria-labelledby="updateModel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">编辑/修改</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label for="targetType">轮播类型：</label>
                                        <select id="targetType" class="form-control" onchange="ChangeTargetType(this)">
                                            <option value="10">宣传</option>
                                            <option value="20">类目</option>
                                            <option value="30">商品</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label for="targetSysID">商品导向：</label>
                                        <input type="text" id="targetSysID" class="form-control input-sm" placeholder="GoodsName..." maxlength="50" oninput="ShowUlList(this,'searchGoods')" />
                                        <ul class="dropdown-menu width-100" role="menu"></ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="UpdateRoll(this)" value="修改" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</body>
</html>
