<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="goods_list.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.goods.goods_list" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_upload.ascx" TagPrefix="uc1" TagName="uc_upload" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script src="../../js/web/GoodsList.js"></script>
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ul class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">商品列表</li>
                </ul>
            </div>
            <div class="wrapper">
                <div class="form-inline pull-right marign-5px">
                    <button runat="server" id="uploadExcels" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#uploadExcelModal" mydata="uploadGoodsExcel" onclick="ShowUploadExel(this)">
                        <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>&nbsp;导入
                    </button>
                    <div class="btn-group">
                        <button type="button" class="btn btn-info dropdown-toggle btn-sm" data-toggle="dropdown">
                            下载 <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="/excel/上传错误信息表.xls" runat="server" id="downLoad" class="btn btn-info" style="color: #ff0000">导出错误商品信息</a></li>
                            <li><a href="#" class="btn btn-success" mydata="AllList" onclick="ExportExcel(this)">导出所有商品信息</a></li>
                            <li><a href="#" class="btn btn-warning" mydata="SelectList" onclick="ExportExcel(this)">导出当前筛选信息</a></li>
                        </ul>
                    </div>
                    <button runat="server" id="addNewModal" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#addModal">
                        <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>&nbsp;添加
                    </button>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">商品类目</span>
                        <select runat="server" id="sltByCateType" class="form-control input-sm" mydata="byCateType">
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">供应商</span>
                        <select runat="server" id="sltBySuppliers" class="form-control input-sm" mydata="bySuppliers">
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">是否上架</span>
                        <select runat="server" id="sltByIsEnable" class="form-control input-sm" mydata="byIsEnable">
                            <option value="">全部</option>
                            <option value="0">下架</option>
                            <option value="1">上架</option>
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon"><i class="fa fa-sort-alpha-asc"></i></span>
                        <select runat="server" id="sltOrderBy" class="form-control input-sm" mydata="byOrderBy">
                            <option value="0">按商品库存</option>
                            <option value="1">按本店售价</option>
                            <option value="2">按已售数量</option>
                            <option value="3">按添加时间</option>
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <input type="text" runat="server" id="iptSearch" class="form-control input-sm" mydata="searchName" placeholder="请输入商品编号或名称" />
                        <span class="input-group-btn">
                            <button runat="server" id="btnSearch" type="button" class="btn btn-primary btn-sm" onclick="btnSearch_Click(this)">
                                <span class="fa fa-search" aria-hidden="true"></span>&nbsp;
                            </button>
                        </span>
                    </div>
                </div>
                <div id="divTable">
                    <table id="tableGoodsList" class="table table-striped table-condensed table-hover table-bordered">
                        <asp:Repeater ID="repeaterGoodsList" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr class="info">
                                        <th>序列</th>
                                        <th>主图</th>
                                        <th>类目</th>
                                        <th>编号</th>
                                        <th>品名</th>
                                        <th>重量</th>
                                        <th>售价</th>
                                        <th>库存</th>
                                        <th>销量</th>
                                        <th>创建时间</th>
                                        <th>限购件数</th>
                                        <th>供应商</th>
                                        <th>详情</th>
                                        <th>操作</th>
                                        <th>上新</th>
                                        <th>上架</th>
                                        <th>特供</th>
                                        <th>备注</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="info-val">
                                    <td name="tOrderBy" class="width_3">
                                        <span onclick="ChangeText(this,'goodsOrderBy')"><%# Eval("OrderBy") %></span></td>
                                    <td name="tImg" class="tb-img" mydata="<%# Eval("GoodsID") %>">
                                        <a href="javascript:;" data-toggle="modal" data-target="#uploadPicture" mydata="mainPicture" onclick="ShowUploadPicture(this)">
                                            <img src="<%# Eval("GoodsThumb") %>" onerror="nofind()" />
                                        </a>
                                    </td>
                                    <td name="tName_2" class="width_5"><%# Eval("CateName") %></td>
                                    <td name="tNo" class="width_8" title="<%# Eval("GoodsSn") %>"><%# Eval("GoodsSn") %></td>
                                    <td name="tName" class="width_8" title="<%# Eval("GoodsName") %>"><%# Eval("GoodsName") %></td>
                                    <td name="tWeight" class="width_5"><span id="tWeight" onclick="ChangeText(this,'updateWeight')"><%# Eval("Weight") %></span></td>
                                    <td name="tPrice" class="width_5"><a href="javascript:;" data-toggle="modal" data-target="#changePrice" mydata="<%# Eval("GoodsID") %>" onclick="ShowChangePrice(this)">¥<em style="font-style: normal"> <%# Eval("ShopPrice") %></em><br>
                                        ¥ <em style="color: red; text-decoration: line-through; font-style: normal"><%# Eval("MarketPrice") %></em></a>
                                    </td>
                                    <td name="tNumber" class="width_5">
                                        <span id="tInventory" onclick="ChangeText(this,'updateInverntory')"><%# Eval("GoodsNumber")%></span></td>
                                    <td name="tNumber_1" class="width_5"><%# Eval("SaleNumber") %></td>

                                    <td name="tTime" class="width_10"><a tabindex="0" role="button" data-toggle="popover" data-trigger="focus" data-title="最后修改人：<%# Eval("ModifyPerson") %>" data-content="更新时间：<%# Eval("LastUpdate")%>"><%# Eval("AddTime") %></a></td>
                                    <td name="tQuotaNumber" class="width_5"><span id="tQuotaNumber" onclick="ChangeText(this,'updateQuotaNumber')"><%# Eval("QuotaNumber") %></span></td>
                                    <td name="tName_1" class="width_8"><%# Eval("SuppliersName") %></td>
                                    <td name="tMore" class="width_5"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus" title="配送：<%# Eval("FareName")%>" data-content="简介：<%# Eval("GoodsBrief")%>">更多</a></td>
                                    <td name="tOperation" mydata="<%# Eval("GoodsID") %>" class="width_12">
                                        <div class="btn-group btn-group-sm">
                                            <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#assignModel" id="btnAssign" mydata="<%# Eval("GoodsID") %>" onclick="AssignCpyGoods(this)">分配</button>
                                            <button type="button" class="btn btn-info btn-sm" id="editorDetial" onclick="EditorGoodsDetial(this)"><span class="fa fa-pencil-square-o"></span></button>
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要删除吗？','deleteGoods')"><span class="fa fa-trash-o"></span></button>
                                        </div>
                                    </td>
                                    <td name="tNew" mydata="<%# Eval("IsNew") %>" class="width_3">
                                        <input type="checkbox" name="isCheckNew" onchange="ChangeCheckBox(this,'goodsNew')"></td>
                                    <td name="tIsEnable" mydata="<%# Eval("IsEnable") %>" class="width_3">
                                        <input type="checkbox" name="isCheckEnable" onchange="ChangeCheckBox(this,'goodsIsEnable')"></td>
                                    <td name="tIsPromote" mydata="<%# Eval("IsPromote") %>" class="width_3">
                                        <input type="checkbox" name="isCheckPromote" onchange="ChangeCheckBox(this,'goodsIsPromote')"></td>
                                    <td hidden="hidden" name="tID"><%# Eval("GoodsID") %></td>
                                    <td name="tRemark" class="width_12"><a style="text-decoration: none" tabindex="0" role="button" data-toggle="popover" data-trigger="focus" data-content="<%# Eval("SellerNote")%>">
                                        <textarea readonly="readonly" class="form-control" onclick="ChangeDescribed(this,'goodsSellerNote')" mydata="<%# Eval("GoodsID") %>"><%#  Eval("SellerNote") %></textarea></a></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                    <span style="color:red">*限购件数小于等于0时，代表不限购</span>
                    <div runat="server" id="pageNav"></div>
                </div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
        <div id="divHelper">
            <uc1:uc_upload runat="server" ID="uc_upload" />
            <uc1:uc_confirm runat="server" ID="uc_confirm" />

            <div class="modal fade" id="assignModel" tabindex="-1" role="dialog" aria-labelledby="assignModel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">分配公司</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row" id="chooseCpyInfo">
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="AssignGoodsToCpy(this)" value="分配" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="changePrice" tabindex="-1" role="dialog" aria-labelledby="changePriceModel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">更改价格</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="shopPrice">本店价格：</label>
                                        <input type="text" class="form-control" id="shopPrice" placeholder="请输入市场价格..." />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="marketPrice">市场价格：</label>
                                        <input type="text" class="form-control" id="marketPrice" placeholder="请输入本店价格..." />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="ChangePrice(this)" value="确认" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="addModal" tabindex="-1" role="dialog" aria-labelledby="addModal" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">添加商品</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addGoodsName">商品名称：</label>
                                        <input type="text" class="form-control" id="addGoodsName" placeholder="请输入商品名称..." />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addGoodsNo">商品编号：</label>
                                        <input type="text" class="form-control" id="addGoodsNo" placeholder="请输入商品编号..." />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addShopPrice">本店价格：</label>
                                        <input type="text" class="form-control" id="addShopPrice" placeholder="请输入市场价格..." />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addMarketPrice">市场价格：</label>
                                        <input type="text" class="form-control" id="addMarketPrice" placeholder="请输入市场价格..." />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addWeight">商品重量：</label>
                                        <input type="text" class="form-control" id="addWeight" placeholder="请输入商品重量..." />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addInventory">商品库存：</label>
                                        <input type="text" class="form-control" id="addInventory" placeholder="请输入商品库存..." />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addSuppliers">供应商</label>
                                        <select runat="server" id="addSuppliers" class="form-control input-sm">
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="addFareTemp">运费模板</label>
                                        <select runat="server" id="addFareTemp" class="form-control input-sm">
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label for="addCateName">类目</label>
                                        <input type="text" id="addCateName" class="form-control input-sm" placeholder="cateName..." maxlength="50" mydata="" oninput="ShowUlList(this,'searchCates')" />
                                        <ul class="dropdown-menu width-100" role="menu"></ul>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label for="addBrand">品牌</label>
                                        <input type="text" id="addBrand" class="form-control input-sm" placeholder="brandName..." maxlength="50" mydata="" oninput="ShowUlList(this,'searchBrands')" />
                                        <ul class="dropdown-menu width-100" role="menu"></ul>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="addGoodsBrief">商品简介（简短的描述商品的相关信息）</label>
                                        <textarea id="addGoodsBrief" class="form-control" runat="server" rows="5" maxlength="255"></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="addRemark">商家备注（只有商家可见的备注信息）</label>
                                        <textarea id="addRemark" class="form-control" runat="server" rows="5" maxlength="255"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="button" class="btn btn-primary btn-sm" onclick="AddGoodsToValidate(this)" value="确认" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </section>
</body>
</html>
