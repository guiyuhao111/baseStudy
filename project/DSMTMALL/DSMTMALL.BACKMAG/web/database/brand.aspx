<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="brand.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.database.brand" %>

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
    <script src="../../js/web/Brand.js"></script>
</head>

<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">品牌列表</li>
                </ol>
            </div>
            <div class="wrapper">
                <div class="form-inline pull-right">
                    <button runat="server" id="addNewModal" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#addSecondModel">
                        <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>&nbsp;品牌添加
                    </button>
                    <div class="input-group input-group-sm">
                        <input type="text" runat="server" id="iptSearch" class="form-control input-sm" mydata="searchName" placeholder="请输入品牌名称" />
                        <span class="input-group-btn">
                            <button runat="server" id="btnSearch" type="button" class="btn btn-primary btn-sm" onclick="btnSearch_Click(this)">
                                <span class="fa fa-search" aria-hidden="true"></span>&nbsp;
                            </button>
                        </span>
                    </div>
                </div>

                <div id="divTable">

                    <h4 class="my-h4">商品列表</h4>
                    <table id="tableBrandList" class="table table-striped table-condensed table-hover table-bordered">
                        <asp:Repeater ID="repeaterBrandList" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr class="info">
                                        <th>品牌名称</th>
                                        <th>品牌链接</th>
                                        <th>品牌排序</th>
                                        <th>品牌描述</th>
                                        <th>是否显示</th>
                                        <th>系统操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td name="tName" class="my-overflow-title" title="<%# Eval("BrandName") %>"><%# Eval("BrandName") %></td>
                                    <td name="tUrl"><%# Eval("BrandUrl") %></td>
                                    <td name="tOrderBy" class="my-overflow-title">
                                        <span onclick="ChangeText(this,'brandOrderBy')"><%# Eval("OrderBy") %></span></td>
                                    <td name="tDesc">
                                        <a style="text-decoration: none" tabindex="0" role="button" data-toggle="popover" data-trigger="focus" data-content="<%# Eval("BrandDesc")%>">
                                            <textarea readonly="readonly" class="form-control" onclick="ChangeDescribed(this,'brandDescribe')" mydata="<%# Eval("BrandID") %>"><%#  Eval("BrandDesc") %></textarea>
                                        </a>
                                    </td>
                                    <td name="tIsShow" mydata="<%# Eval("IsShow") %>">
                                        <input type="checkbox" name="isCheckEnable" onchange="ChangeCheckBox(this,'brandIsShow')"></td>
                                    <td name="tO    peration" mydata="<%# Eval("BrandID") %>" style="width: 80px">
                                        <div class="btn-group btn-group-sm">
                                            <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#updateModal" onclick="ShowUpdateBrand(this)"><span class="fa fa-pencil-square-o"></span></button>
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要删除吗？','deleteBrand')"><span class="fa fa-trash-o"></span></button>
                                        </div>
                                    </td>
                                    <td name="tID" hidden="hidden"><%# Eval("BrandID") %></td>
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
            <div class="modal fade" id="addSecondModel" tabindex="-1" role="dialog" aria-labelledby="addStatusModel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">添加品牌</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="newBrandName">品牌名称：</label>
                                        <input type="text" id="newBrandName" class="form-control input-sm" placeholder="多个品牌添加 / eg: 小米/华为/魅族  " maxlength="1000" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="AddBrand(this)" value="添加" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="updateModal" tabindex="-1" role="dialog" aria-labelledby="updateModellable" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">修改品牌</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="updateBrandName">品牌名称：</label>
                                        <input type="text" id="updateBrandName" class="form-control input-sm" placeholder="请输入修改后的品牌名称 " maxlength="1000" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="UpdateBrand(this)" value="添加" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</body>
</html>
