<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="imges.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.database.imges" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
                    <li class="active">图片中心</li>
                </ol>
            </div>
            <div class="wrapper">
                <div id="divBrandPage">
                    <div id="divTable">
                        <div id="gallery" class="media-gal">
                            <asp:Repeater ID="repeaterImgList" runat="server">
                                <ItemTemplate>
                                    <div class="images item">
                                        <a href="javascript:;" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThisImg(this,'确认要删除吗？','deleteImgs')" mydata="<%# Eval("FileSysID") %>">
                                            <img src="<%# Eval("FilePath") %>" alt="" onerror="nofind()" />
                                        </a>
                                        <p><%# Eval("FileName") %> </p>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                        </div>
                        <div runat="server" id="pageNav"></div>
                    </div>
                </div>
            </div>  <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
          

        <div id="divHelper">
            <uc1:uc_confirm runat="server" ID="uc_confirm" />
        </div>
    </section>
</body>
</html>
