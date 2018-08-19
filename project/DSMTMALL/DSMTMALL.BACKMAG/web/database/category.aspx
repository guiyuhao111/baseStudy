<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="category.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.database.category" %>

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
    <script src="../../js/web/Category.js"></script>
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
            </div>
            <div class="wrapper">
                <div id="categoryPage">
                    <nav class="navbar navbar-default" role="navigation">
                        <div class="container-fluid">
                            <div class="navbar-header">
                                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                                    <span class="sr-only">Toggle navigation</span>
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                </button>
                                <a class="navbar-brand" href="category.aspx?bySysID=0">类目管理</a>
                            </div>
                            <div class="collapse navbar-collapse my-sidebar-navbar" id="bs-example-navbar-collapse-1">
                                <ul class="nav navbar-nav" id="sltFirstType" runat="server">
                                </ul>

                                <ul class="nav navbar-nav navbar-right">
                                    <li class="dropdown">
                                        <form class="navbar-form navbar-left" role="search">
                                            <button runat="server" id="addNewModal" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#addSecondModel">
                                                <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>&nbsp;类目添加
                                            </button>
                                        </form>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </nav>
                    <div>
                        <table id="tableCGList" class="table table-striped table-condensed table-hover table-bordered">
                            <asp:Repeater ID="repeaterCGList" runat="server">
                                <HeaderTemplate>
                                    <thead>
                                        <tr class="info">
                                            <th>类目名称</th>
                                            <th>上级类目</th>
                                            <th>类目主图</th>
                                            <th>优先等级</th>
                                            <th>启用</th>
                                            <th>操作</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td name="tName" mydata="<%# Eval("CateID") %>"><%# Eval("CateName") %></td>
                                        <td name="tName_1" mydata="<%# Eval("ParentID") %>"><%# Eval("PCateName") %></td>
                                        <td name="tImg" class="tb-img" mydata="<%# Eval("CateID") %>">
                                            <a href="javascript:;" data-toggle="modal" data-target="#uploadPicture" mydata="categoryPicture" onclick="ShowUploadPicture(this)">
                                                <img src="<%# Eval("ShowImage") %>" onerror="nofind()" />
                                            </a>
                                        </td>
                                        <td name="tOrderBy" class="my-overflow-title">
                                            <span onclick="ChangeText(this,'categoryOrderBy')"><%# Eval("OrderBy") %></span></td>
                                        <td name="tIsEnable" mydata="<%# Eval("IsEnable") %>">
                                            <input type="checkbox" name="isCheckEnable" onchange="ChangeCheckBox(this,'categoryIsEnable')">
                                        </td>
                                        <td name="tOperation" mydata="<%# Eval("CateID") %>">
                                            <div class="btn-group btn-group-sm">
                                                <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#updateModel" onclick="ShowUpdateCategory(this)"><span class="fa fa-pencil-square-o"></span></button>
                                                <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要删除吗？','deleteCategory')"><span class="fa fa-trash-o"></span></button>
                                            </div>
                                        </td>
                                        <td hidden="hidden" name="tID"><%# Eval("CateID") %></td>
                                        <td hidden="hidden" name="tID_1"><%# Eval("ParentID") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                        <div runat="server" id="pageNav"></div>
                    </div>
                </div>
            </div>
        <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
      
        <div id="divHelper">
            <div class="modal fade" id="addSecondModel" tabindex="-1" role="dialog" aria-labelledby="addStatusModel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">添加类目</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label for="newCategoryName">类目名称：</label>
                                        <input type="text" id="newCategoryName" class="form-control input-sm" placeholder="多个类目添加 / eg: 服装/配饰/帽子  " maxlength="1000" />
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label for="choiceParentsID">一级类目：</label>
                                        <select runat="server" id="choiceParentsID" class="form-control input-sm"></select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="AddCategory(this)" value="添加" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>


            <div class="modal fade" id="updateModel" tabindex="-1" role="dialog" aria-labelledby="addStatusModel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">编辑类目</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label for="updateCategoryName">类目名称：</label>
                                        <input type="text" id="updateCategoryName" class="form-control input-sm" placeholder="多个二级类目同时添加“/”  " maxlength="1000" />
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label for="updateChoiceParentsID">一级类目：</label>
                                        <select runat="server" id="updateChoiceParentsID" class="form-control input-sm"></select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="UpdateCategory(this)" value="修改" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>
            <uc1:uc_upload runat="server" ID="uc_upload" />
            <uc1:uc_confirm runat="server" ID="uc_confirm" />
        </div>

    </section>
</body>
</html>

