<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editor.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.goods.editor" ValidateRequest="false" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_upload.ascx" TagPrefix="uc1" TagName="uc_upload" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_info.ascx" TagPrefix="uc1" TagName="uc_info" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <link href="../../editor/themes/default/default.css" rel="stylesheet" />
    <link href="../../editor/plugins/code/prettify.css" rel="stylesheet" />
    <script src="../../editor/kindeditor.js"></script>
    <script src="../../editor/lang/zh_CN.js"></script>
    <script src="/editor/plugins/code/prettify.js"></script>
    <script src="../../js/web/Editor.js"></script>
    <link href="/css/web/index/main.css" rel="stylesheet" />
    <script>
        //引入的方法---新框架
        function mainContentHeightAdjust() {//设置当前页的最小高度
            var docHeight = jQuery(document).height();
            //alert(docHeight);
            if (docHeight > jQuery('.main-content').height())
                jQuery('.main-content').height(docHeight+100);
        }
        $(function () { mainContentHeightAdjust(); });
    </script>
</head>

<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <div class="row">
                    <div class="col-xs-2"></div>
                    <div class="col-xs-8">
                        <div>
                            <ol class="breadcrumb">
                                <li>商品管理</li>
                                <li class="active">商品详情</li>
                                <li ><a href="#" id="thisUrl" runat="server"><span id="thisGoodsName" runat="server"></span></a></li>
                            </ol>
                        </div>
                        <div style="background-color: #ffffff; padding: 30px 10% 5% 10%;">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="myGoodsName">商品名称</label>
                                        <input id="myGoodsName" type="text" class="form-control" runat="server" />
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="myKeyWords">配置关键词</label>
                                        <input id="myKeyWords" type="text" class="form-control" runat="server" pleaseholder="请输入该商品被检索的关键词" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label for="sltBySuppliers">供应商</label>
                                        <select runat="server" id="sltBySuppliers" class="form-control input-sm">
                                        </select>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label for="sltByCateName">类目</label>
                                        <select runat="server" id="sltByCateName" class="form-control input-sm">
                                        </select>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label for="updateBrand">品牌</label>
                                        <input type="text" id="updateBrand" runat="server" class="form-control input-sm" placeholder="brandName..." maxlength="50" mydata="" oninput="ShowUlList(this,'searchBrands')" />
                                        <ul class="dropdown-menu width-100" role="menu"></ul>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label for="updateFareTemp">运费模板</label>
                                        <select runat="server" id="updateFareTemp" class="form-control input-sm">
                                        </select>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="myGoodsBrief">商品简介（简短的描述商品的相关信息）</label>
                                        <textarea id="myGoodsBrief" class="form-control" runat="server" rows="5"></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="myRemark">商品备注（只有商家可见的备注信息）</label>
                                        <textarea id="myRemark" class="form-control" runat="server" rows="5"></textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="shopPrice">本店价格：</label>
                                        <input type="text" class="form-control" id="shopPrice" placeholder="请输入市场价格..." runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="marketPrice">市场价格：</label>
                                        <input type="text" class="form-control" id="marketPrice" placeholder="请输入本店价格..." runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <h4>商品图册</h4>
                                    </div>
                                    <div class="col-xs-12 editor-img-list" id="goodsImgList">
                                        <div>
                                            <a href="javascript:;" data-toggle="modal" data-target="#uploadPicture" mydata="galleryPicture" onclick="ShowUploadPicture_this(this)">
                                                <h3>+ 添加产品图片</h3>
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="col-xs-12">
                                <div class="row ">
                                    <div class="col-xs-12">
                                        <h4>详情描述</h4>
                                    </div>
                                    <form>
                                    <div class="col-xs-12" id="addVariants">
                                        <textarea id="myContent" cols="100" rows="8" style="width: 100%; height: 756px; visibility: hidden;" runat="server" onblur="editor.sync()"></textarea>
                                        <br />
                                    </div>
                                    </form>
                                </div>
                            </div>
                            <div class="ke-dialog-row">
                                <div class="col-xs-12 ">
                                    <input type="button" class="btn btn-default btn-sm col-xs-12" id="saveInfo" runat="server" value="保存" onclick="SaveEditorInfo(this)" style="background-color: #166ee8; height: 40px; font-size: 18px; color: #ffffff" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-2"></div>
                </div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
        <uc1:uc_upload runat="server" ID="uc_upload" />
        <uc1:uc_info runat="server" ID="uc_info" />
        <uc1:uc_confirm runat="server" ID="uc_confirm" />
    </section>
</body>

</html>
