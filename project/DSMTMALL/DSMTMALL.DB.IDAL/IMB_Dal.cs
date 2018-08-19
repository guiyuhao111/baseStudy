using DSMTMALL.DB.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace DSMTMALL.DB.IDAL
{
    public interface IMB_Dal
    {
        #region 通用

        /// <summary>
        /// 获取实体集合
        /// </summary>
        List<T> GetModelList<T>(int topNum, string strWhere, string orderBy, object objParam, string tableID) where T : new();

        /// <summary>
        /// 获取查询的结果集合
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="myQuery"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetModelList(string strSql, object objParam);

        /// <summary>
        /// 获取实体集合数量
        /// </summary>
        int GetModelListCount<T>(string strWhere, object objParam, string tableID) where T : new();

        /// <summary>
        /// 获取查询的结果集合计数
        /// </summary>
        /// <param name="myQuery"></param>
        /// <returns></returns>
        int GetModelListCount(string strSql, object objParam);

        /// <summary>
        /// 添加实体对象集合
        /// </summary>
        bool AddModelList<T>(List<T> tList, string tableID) where T : new();

        /// <summary>
        /// 添加实体对象集合（自定义sql语句）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql插入语句</param>
        /// <param name="tList">批量插入sql数据集合</param>
        /// <param name="tableID">分表ID</param>
        /// <returns></returns>
        bool AddModelList<T>(string strSql, List<T> tList, string tableID) where T : new();

        /// <summary>
        /// 修改实体对象集合
        /// </summary>
        bool UpdateModelList<T>(string strWhere, List<object> objList, string tableID) where T : new();

        /// <summary>
        /// 修改更新实体数据库数据(自定义sql语句)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql执行更新语句</param>
        /// <param name="objList">批量要更新的数据集合</param>
        /// <param name="tableID">分表ID</param>
        /// <returns></returns>
        bool UpdateModelSQL(string strSql, List<object> objList, string tableID);

        /// <summary>
        /// 删除实体对象集合
        /// </summary>
        bool DeleteModelList<T>(string strWhere, object objParam, string tableID) where T : new();

        #endregion 通用

        #region 数据获取

        /// <summary>
        /// 获取类目集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> GetCategoryList();

        /// <summary>
        /// 获取用户的模块集合
        /// </summary>
        /// <returns></returns>
        List<Model.M_Modal> GetModalList(string adminID);

        /// <summary>
        /// 获取商品信息（用户EXCEL导出）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataSet GetGoodsInfo(string type, string url);

        /// <summary>
        /// 获取当前订单编号的所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object GetOrderGoodsList(string id);

        /// <summary>
        /// 获取当前退货订单编号的所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object GetReBackOrderGoodsList(string id);

        /// <summary>
        /// 获取订单信息（用户EXCEL导出）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        DataSet GetOrdersInfo(Dictionary<string, string> dic);

        /// <summary>
        /// 获取退货订单流水信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<DB.Model.ResBackTrasd> GetReBackOrderTrade(string id);

        #endregion

        #region 页面载入

        /// <summary>
        /// 获取商品列表数据
        /// </summary>
        /// <param name="byCateType"></param>
        /// <param name="bySuppliers"></param>
        /// <param name="byIsEnable"></param>
        /// <param name="byOrderBy"></param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageGoodsList(string byCateType, string bySuppliers, string byIsEnable, string byOrderBy, string searchName, int startSize, int endSize);
        /// <summary>
        /// 获取商品列表计数
        /// </summary>
        /// <param name="byCateType"></param>
        /// <param name="bySuppliers"></param>
        /// <param name="byIsEnable"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        int GetByPageGoodsListCount(string byCateType, string bySuppliers, string byIsEnable, string searchName);

        /// <summary>
        /// 获取商品列表数据--供应商
        /// </summary>
        /// <param name="byCateType"></param>
        /// <param name="bySuppliers"></param>
        /// <param name="byIsEnable"></param>
        /// <param name="byOrderBy"></param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageGoodsListSuppliers(string byCateType, string bySuppliers, string byIsEnable, string byOrderBy, string searchName, int startSize, int endSize);

        /// <summary>
        /// 获取商品列表计数--供应商
        /// </summary>
        /// <param name="byCateType"></param>
        /// <param name="bySuppliers"></param>
        /// <param name="byIsEnable"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        int GetByPageGoodsListSuppliersCount(string byCateType, string bySuppliers, string byIsEnable, string searchName);

        /// <summary>
        /// 获取类目数据列表
        /// </summary>
        /// <param name="sysID">类目ID</param>
        /// <param name="startSize">开始</param>
        /// <param name="endSize">结束</param>
        /// <returns></returns>
        object GetByPageCagegoryList(string sysID, int startSize, int endSize);
        int GetByPageCagegoryListCount(string sysID);

        /// <summary>
        /// 获取品牌数据列表
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageBrandList(string searchName, int startSize, int endSize);
        int GetByPageBrandListCount(string searchName);

        /// <summary>
        /// 获取轮播数据列表
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageRollList(int startSize, int endSize);
        int GetByPageRollListCount();

        /// <summary>
        /// 获取订单信息列表
        /// </summary>
        /// <param name="suppliersID">供应商ID</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="payStatus">支付状态</param>
        /// <param name="shippingStatus">物流状态</param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string payID, string searchName, string devSort, int startSize, int endSize);
        int GetByPageOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string payID, string searchName);

        /// <summary>
        /// 获取异常订单信息
        /// </summary>
        /// <param name="searchName">检索条件</param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageUnusualOrderList(string searchName, int startSize, int endSize);
        int GetByPageUnusualOrderListCount(string searchName);


        /// <summary>
        /// 获取用户的信息列表
        /// </summary>
        /// <param name="byUserType"></param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetByPageUserList(string byUserType, string searchName, int startSize, int endSize, out int listCount);

        /// <summary>
        /// 获取管理员用户信息
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageAdminList(string searchName, int startSize, int endSize);
        int GetByPageAdminListCount(string searchName);

        /// <summary>
        /// 获取数据统计页订单数量总计
        /// </summary>
        /// <param name="byTimeStart"></param>
        /// <param name="byTimeEnd"></param>
        /// <param name="suppliersID"></param>
        /// <param name="orderStatus"></param>
        /// <param name="devSort"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageDataOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string devSort, string searchName, int startSize, int endSize);
        int GetByPageDataOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string searchName);

        /// <summary>
        /// 获取数据统计页订单数量总计
        /// </summary>
        /// <param name="byTimeStart"></param>
        /// <param name="byTimeEnd"></param>
        /// <param name="suppliersID"></param>
        /// <param name="orderStatus"></param>
        /// <param name="devSort"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageDataGoodsList(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string searchName, int startSize, int endSize, out int count);

        /// <summary>
        /// 获取推荐的商品列表信息
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        object GetByPageGoodsRecomList(string searchName, int startSize, int endSize);
        int GetByPageGoodsRecomListCount(string searchName);

        /// <summary>
        /// 获取订单信息列表
        /// </summary>
        /// <param name="suppliersID">供应商ID</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="payStatus">支付状态</param>
        /// <param name="shippingStatus">物流状态</param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageReBackOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string payID, string byReBackType, string byAuthType, string searchName, string devSort, int startSize, int endSize);
        int GetByPageReBackOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string payID, string byReBackType, string byAuthType, string searchName);

        /// <summary>
        /// 获取退货订单信息列表
        /// </summary>
        /// <param name="suppliersID">供应商ID</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="payStatus">支付状态</param>
        /// <param name="shippingStatus">物流状态</param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageReBackThirdOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string byOrderStatus, string byReBackType, string searchName, string devSort, int startSize, int endSize);
        int GetByPageReBackThirdOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string byOrderStatus, string byReBackType, string searchName);

        /// <summary>
        /// 获取数据统计页退货订单商品总计
        /// </summary>
        /// <param name="byTimeStart"></param>
        /// <param name="byTimeEnd"></param>
        /// <param name="suppliersID"></param>
        /// <param name="orderStatus"></param>
        /// <param name="devSort"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageReBackDataThirdGoodsList(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string reBackType, string searchName, int startSize, int endSize);
        int GetByPageReBackDataThirdGoodsListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string reBackType, string searchName);

        /// <summary>
        /// 获取支付流水记录表
        /// </summary>
        /// <param name="byTimeStart"></param>
        /// <param name="byTimeEnd"></param>
        /// <param name="byComfirmType"></param>
        /// <param name="byPayID"></param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageUnusualTradeList(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName, int startSize, int endSize, out decimal moneyAmount);
        int GetByPageUnusualTradeListCount(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName);

        /// <summary>
        /// 获取退款流水记录表
        /// </summary>
        /// <param name="byTimeStart"></param>
        /// <param name="byTimeEnd"></param>
        /// <param name="byComfirmType"></param>
        /// <param name="byPayID"></param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetByPageUnusualRebackTradeList(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName, int startSize, int endSize, out decimal moneyAmount);
        int GetByPageUnusualRebackTradeListCount(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName);
        #endregion

        #region 事务处理

        /// <summary>
        /// 上传图片文件数据进数据库
        /// </summary>
        /// <param name="sysID">产品ID</param>
        /// <param name="fileUrl_1">原图</param>
        /// <param name="fileUrl_2">缩略图</param>
        /// <param name="type">产品主图</param>
        /// <returns></returns>
        bool UploadImgFile(string sysID, string fileUrl_1, string fileUrl_2, string type);

        /// <summary>
        /// 添加新的商品信息入数据库
        /// </summary>
        /// <param name="CateID">类目ID</param>
        /// <param name="MarketPrice">市场价</param>
        /// <param name="ShopPrice">本店价</param>
        /// <param name="BrandID">品牌ID</param>
        /// <param name="GoodsImg">商品原图</param>
        /// <param name="GoodsThumb">商品缩略图</param>
        /// <param name="OriginalImg">商品原图</param>
        /// <param name="SuppliersID">供应商ID</param>
        /// <param name="GoodsSn">商品编码</param>
        /// <param name="GoodsName">商品名称</param>
        /// <param name="GoodsNumber">商品库存</param>
        /// <param name="GoodsBrief">简介</param>
        /// <param name="IsReal">是否实物</param>
        /// <param name="IsNew">是否新品</param>
        /// <param name="IsHot">是否热销</param>
        /// <returns></returns>
        bool AddGoodsInfo(int CateID, decimal MarketPrice, decimal ShopPrice, int BrandID, string GoodsImg, string GoodsThumb, string OriginalImg, int SuppliersID, string GoodsSn, string GoodsName, int GoodsNumber, string GoodsBrief, int IsReal, int IsNew, int IsHot, double Weight, string SellerNote, string FareTemp, string adminName);

        /// <summary>
        /// 分配商品给指定的公司-集团推荐商品
        /// </summary>
        /// <param name="goodsID"></param>
        /// <param name="cpyInfoList"></param>
        /// <returns></returns>
        bool AssignGoodsToCpy(string goodsID, string[] cpyInfoList);

        /// <summary>
        /// 创建运费模板-指定地区配送运费
        /// </summary>
        /// <param name="fareName"></param>
        /// <param name="fareAddress"></param>
        /// <param name="fareTime"></param>
        /// <param name="fareType"></param>
        /// <param name="defaultFareTemp"></param>
        /// <param name="fareCarryTempList"></param>
        /// <returns></returns>
        bool AddNewFareTemplate(string fareName, string fareAddress, int fareTime, int fareType, Model.DefaultFareTemp defaultFareTemp, DB.Model.FareCarryTemp[] fareCarryTempList);

        /// <summary>
        /// 删除运费模板--同时删除相对应得配送
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        bool DeleteFareTemplate(string sysID);

        /// <summary>
        /// 更新模块分配
        /// </summary>
        /// <param name="adminID">用户ID</param>
        /// <param name="modaleList"></param>
        /// <returns></returns>
        bool AssignModale(string adminID, string[] modaleList);

        /// <summary>
        /// 添加退货订单信息
        /// </summary>
        /// <param name="backInfoList"></param>
        /// <param name="phone"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        bool AddReBackOrder(List<Model.ResBackGoodsInfo> backInfoList, string phone, int type, string remark, string rebackName, string orderSysID, out string outEsg);

        /// <summary>
        /// 接收从XF系统返回的退款信息生成退款流水
        /// </summary>
        /// <param name="reBackOrderInfo"></param>
        /// <param name="tradeSysID"></param>
        /// <param name="reBackTradeNo"></param>
        /// <param name="reBackMoney"></param>
        /// <returns></returns>
        bool CreatReBackTradeByRetrunFromXFRefundAcc(Model.ResBackTrasd reBackOrderInfo, string tradeSysID, string reBackTradeNo, decimal reBackMoney);

        /// <summary>
        /// 审核退换货申请是否通过
        /// </summary>
        /// <param name="reBackID"></param>
        /// <param name="authType"></param>
        /// <param name="remark"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        bool ComfirmReBackOrderApply(string reBackID, int authType, string remark, string address, string orderID);

        /// <summary>
        ///  更新订单信息-从付款系统返回支付结果--管理员手动补流水号
        /// </summary>
        /// <param name="orderUnifySn"></param>
        /// <param name="orderSn"></param>
        /// <param name="orderAmount"></param>
        /// <param name="tradeNo"></param>
        /// <param name="tradeTime"></param>
        /// <param name="paymentInfo"></param>
        /// <param name="cpySysID"></param>
        /// <param name="cpyName"></param>
        /// <param name="resMsg"></param>
        /// <returns></returns>
        bool UpdateOrderInfoFromXFPayment(string orderUnifySn, string orderSn, decimal orderAmount, string tradeNo, DateTime tradeTime, M_Payment paymentInfo, string cpySysID, string cpyName, out string resMsg);

        /// <summary>
        /// 第三方管理员对需要换货的商品进行换货处理
        /// </summary>
        /// <param name="reBackId"></param>
        /// <param name="logisticalName"></param>
        /// <param name="logisticalNumber"></param>
        /// <returns></returns>
        bool ComfirmGoodsForCustomer(string reBackID, string logisticalName, string logisticalNumber, string orderID);
        #endregion

        #region 数据操作

        /// <summary>
        /// 删除类目数据（更改状态）
        /// </summary>
        /// <param name="sysID">类目ID</param>
        /// <returns></returns>
        bool DeleteCategory(int sysID);

        /// <summary>
        /// 启禁类目数据（更改启禁）
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        bool IsEnableCategory(int sysID, int status);

        /// <summary>
        /// 删除文件管理图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool DeleteFileManage(string fileName);

        #endregion

        #region 记录错误

        /// <summary>
        /// 记录错误信息进错误信息表中--全程try-catch如果连不上数据库直接返回false
        /// </summary>
        /// <param name="errName">错误描述名称</param>
        /// <param name="errDesc">详细错误描述</param>
        /// <returns></returns>
        bool RecordErrInfoNote(string errName, string errDesc);

        #endregion

    }
}