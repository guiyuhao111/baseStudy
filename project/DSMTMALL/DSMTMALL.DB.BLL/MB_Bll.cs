using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.DALFactory;
using DSMTMALL.DB.IDAL;
using DSMTMALL.DB.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace DSMTMALL.DB.BLL
{
    public partial class MB_Bll
    {
        private readonly IMB_Dal dal = FactoryHelper.CreateMB_Dal(DBEnum.Slave);//默认选择从数据库
        public MB_Bll(DBEnum dbEnum)//构造函数传参
        {
            dal = FactoryHelper.CreateMB_Dal(dbEnum);
        }

        #region 通用

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strWhere">查询where语句</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="objParam">匿名类</param>
        /// <param name="tableID"></param>
        /// <returns></returns>
        public T GetModel<T>(string strWhere, object objParam, string orderBy = null, string tableID = null) where T : new()
        {
            List<T> resInfo = dal.GetModelList<T>(1, strWhere, orderBy, objParam, tableID);
            if (resInfo != null && resInfo.Count > 0) { return resInfo[0]; } else { return default(T); }
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        public List<T> GetModelList<T>(string strWhere, object objParam, string orderBy = null, int topNum = 0, string tableID = null) where T : new()
        {
            return dal.GetModelList<T>(topNum, strWhere, orderBy, objParam, tableID);
        }

        /// <summary>
        /// 获取查询的结果集合
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="myQuery"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetModelList(string strSql, object objParam)
        {
            return dal.GetModelList(strSql, objParam);
        }

        /// <summary>
        /// 获取实体集合数量
        /// </summary>
        public int GetModelListCount<T>(string strWhere, object objParam, string tableID = null) where T : new()
        {
            return dal.GetModelListCount<T>(strWhere, objParam, tableID);
        }

        /// <summary>
        /// 获取查询的结果集合计数
        /// </summary>
        /// <param name="myQuery"></param>
        /// <returns></returns>
        public int GetModelListCount(string strSql, object objParam)
        {
            return dal.GetModelListCount(strSql, objParam);
        }

        /// <summary>
        /// 添加实体对象集合
        /// </summary>
        public bool AddModel<T>(T objParam, string tableID = null) where T : new()
        {
            List<T> tList = new List<T>();
            tList.Add(objParam);
            return dal.AddModelList(tList, tableID);
        }

        /// <summary>
        /// 添加实体对象集合
        /// </summary>
        public bool AddModelList<T>(List<T> tList, string tableID = null) where T : new()
        {
            return dal.AddModelList<T>(tList, tableID);
        }

        /// <summary>
        /// 添加实体对象集合（自定义sql语句）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql插入语句</param>
        /// <param name="tList">批量插入sql数据集合</param>
        /// <param name="tableID">分表ID</param>
        /// <returns></returns>
        public bool AddModelList<T>(string strSql, List<T> tList, string tableID = null) where T : new()
        {
            return AddModelList<T>(strSql, tList, tableID);
        }

        /// <summary>
        /// 修改实体对象集合
        /// </summary>
        public bool UpdateModel<T>(string strWhere, object objParam, string tableID = null) where T : new()
        {
            List<object> objList = new List<object>();
            objList.Add(objParam);
            return dal.UpdateModelList<T>(strWhere, objList, tableID);
        }

        /// <summary>
        /// 修改实体对象集合
        /// </summary>
        public bool UpdateModelList<T>(string strWhere, List<object> objList, string tableID = null) where T : new()
        {
            return dal.UpdateModelList<T>(strWhere, objList, tableID);
        }

        /// <summary>
        /// 修改更新实体数据库数据单条匿名类参数
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="objParam"></param>
        /// <param name="tableID"></param>
        /// <returns></returns>
        public bool UpdateModalSql(string strSql, object objParam, string tableID = null)
        {
            List<object> list = new List<object>();
            list.Add(objParam);
            return dal.UpdateModelSQL(strSql, list, tableID);
        }

        /// <summary>
        /// 修改更新实体数据库数据(自定义sql语句)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql执行更新语句</param>
        /// <param name="objList">批量要更新的数据集合</param>
        /// <param name="tableID">分表ID</param>
        /// <returns></returns>
        ///
        public bool UpdateModelSQL(string strSql, List<object> objList, string tableID = null)
        {
            return dal.UpdateModelSQL(strSql, objList, tableID);
        }

        /// <summary>
        /// 删除实体对象集合
        /// </summary>
        public bool DeleteModel<T>(string strWhere, object objParam, string tableID = null) where T : new()
        {
            return dal.DeleteModelList<T>(strWhere, objParam, tableID);
        }

        #endregion 通用

        #region 数据获取

        /// <summary>
        /// 获取类目集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetCategoryList()
        {
            return dal.GetCategoryList();
        }
        /// <summary>
        /// 获取用户的模块集合
        /// </summary>
        /// <returns></returns>
        public List<Model.M_Modal> GetModalList(string adminID)
        {
            return dal.GetModalList(adminID);
        }

        /// <summary>
        /// 获取商品信息（用户EXCEL导出）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetGoodsInfo(string type, string url)
        {
            return dal.GetGoodsInfo(type, url);
        }

        /// <summary>
        /// 获取当前订单编号的所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetOrderGoodsList(string id)
        {
            return dal.GetOrderGoodsList(id);
        }

        /// <summary>
        /// 获取订单信息（用户EXCEL导出）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public DataSet GetOrdersInfo(Dictionary<string, string> dic)
        {
            return dal.GetOrdersInfo(dic);
        }

        /// <summary>
        /// 获取当前退货订单编号的所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetReBackOrderGoodsList(string id)
        {
            return dal.GetReBackOrderGoodsList(id);
        }

        /// <summary>
        /// 获取退货订单流水信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.ResBackTrasd GetReBackOrderTrade(string id)
        {
            List<DB.Model.ResBackTrasd> arrList = dal.GetReBackOrderTrade(id);
            if (arrList != null && arrList.Count > 0)
            {
                return arrList[0];
            }
            else
            {
                return null;
            }
        }

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
        public object GetByPageGoodsList(string byCateType, string bySuppliers, string byIsEnable, string byOrderBy, string searchName, int startSize, int endSize)
        {
            return dal.GetByPageGoodsList(byCateType, bySuppliers, byIsEnable, byOrderBy, searchName, startSize, endSize);
        }
        /// <summary>
        /// 获取商品列表计数
        /// </summary>
        /// <param name="byCateType"></param>
        /// <param name="bySuppliers"></param>
        /// <param name="byIsEnable"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        public int GetByPageGoodsListCount(string byCateType, string bySuppliers, string byIsEnable, string searchName)
        {
            return dal.GetByPageGoodsListCount(byCateType, bySuppliers, byIsEnable, searchName);
        }

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
        public object GetByPageGoodsListSuppliers(string byCateType, string bySuppliers, string byIsEnable, string byOrderBy, string searchName, int startSize, int endSize)
        {
            return dal.GetByPageGoodsListSuppliers(byCateType, bySuppliers, byIsEnable, byOrderBy, searchName, startSize, endSize);
        }

        /// <summary>
        /// 获取商品列表计数--供应商
        /// </summary>
        /// <param name="byCateType"></param>
        /// <param name="bySuppliers"></param>
        /// <param name="byIsEnable"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        public int GetByPageGoodsListSuppliersCount(string byCateType, string bySuppliers, string byIsEnable, string searchName)
        {
            return dal.GetByPageGoodsListSuppliersCount(byCateType, bySuppliers, byIsEnable, searchName);
        }

        /// <summary>
        /// 获取类目数据列表
        /// </summary>
        /// <param name="sysID">类目ID</param>
        /// <param name="startSize">开始</param>
        /// <param name="endSize">结束</param>
        /// <returns></returns>
        public object GetByPageCagegoryList(string sysID, int startSize, int endSize)
        {
            return dal.GetByPageCagegoryList(sysID, startSize, endSize);
        }
        public int GetByPageCagegoryListCount(string sysID)
        {
            return dal.GetByPageCagegoryListCount(sysID);
        }

        /// <summary>
        /// 获取品牌数据列表
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetByPageBrandList(string searchName, int startSize, int endSize)
        {
            return dal.GetByPageBrandList(searchName, startSize, endSize);
        }
        public int GetByPageBrandListCount(string searchName)
        {
            return dal.GetByPageBrandListCount(searchName);
        }

        /// <summary>
        /// 获取轮播数据列表
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetByPageRollList(int startSize, int endSize)
        {
            return dal.GetByPageRollList(startSize, endSize);
        }
        public int GetByPageRollListCount()
        {
            return dal.GetByPageRollListCount();
        }

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
        public object GetByPageOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string payID, string searchName, string devSort, int startSize, int endSize)
        {
            return dal.GetByPageOrderList(byTimeStart, byTimeEnd, suppliersID, orderStatus, payID, searchName, devSort, startSize, endSize);
        }
        public int GetByPageOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string payID, string searchName)
        {
            return dal.GetByPageOrderListCount(byTimeStart, byTimeEnd, suppliersID, orderStatus, payID, searchName);
        }

        /// <summary>
        /// 获取异常订单信息
        /// </summary>
        /// <param name="searchName">检索条件</param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetByPageUnusualOrderList(string searchName, int startSize, int endSize)
        {
            return dal.GetByPageUnusualOrderList(searchName, startSize, endSize);
        }
        public int GetByPageUnusualOrderListCount(string searchName)
        {
            return dal.GetByPageUnusualOrderListCount(searchName);
        }

        /// <summary>
        /// 获取用户的信息列表
        /// </summary>
        /// <param name="byUserType"></param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetByPageUserList(string byUserType, string searchName, int startSize, int endSize, out int listCount) {
            return dal.GetByPageUserList(byUserType, searchName, startSize, endSize,out listCount);
        }

        /// <summary>
        /// 获取管理员用户信息
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetByPageAdminList(int startSize, int endSize, string searchName = null)
        {
            return dal.GetByPageAdminList(searchName, startSize, endSize);
        }
        public int GetByPageAdminListCount(string searchName)
        {
            return dal.GetByPageAdminListCount(searchName);
        }

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
        public object GetByPageDataOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string devSort, string searchName, int startSize, int endSize)
        {
            return dal.GetByPageDataOrderList(byTimeStart, byTimeEnd, suppliersID, orderStatus, devSort, searchName, startSize, endSize);
        }
        public int GetByPageDataOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string searchName)
        {
            return dal.GetByPageDataOrderListCount(byTimeStart, byTimeEnd, suppliersID, orderStatus, searchName);
        }

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
        public object GetByPageDataGoodsList(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string searchName, int startSize, int endSize, out int count)
        {
            return dal.GetByPageDataGoodsList(byTimeStart, byTimeEnd, suppliersID, orderStatus, searchName, startSize, endSize, out count);
        }

        /// <summary>
        /// 获取推荐的商品列表信息
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public object GetByPageGoodsRecomList(string searchName, int startSize, int endSize)
        {
            return dal.GetByPageGoodsRecomList(searchName, startSize, endSize);
        }
        public int GetByPageGoodsRecomListCount(string searchName)
        {
            return dal.GetByPageGoodsRecomListCount(searchName);
        }

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
        public object GetByPageReBackOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string payID, string byReBackType, string byAuthType, string searchName, string devSort, int startSize, int endSize)
        {
            return dal.GetByPageReBackOrderList(byTimeStart, byTimeEnd, suppliersID, payID,  byReBackType,  byAuthType, searchName, devSort, startSize, endSize);
        }
        public int GetByPageReBackOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string payID, string byReBackType, string byAuthType, string searchName)
        {
            return dal.GetByPageReBackOrderListCount(byTimeStart, byTimeEnd, suppliersID, payID,byReBackType,byAuthType, searchName);
        }

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
        public object GetByPageReBackThirdOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string byOrderStatus, string byReBackType, string searchName, string devSort, int startSize, int endSize)
        {
            return dal.GetByPageReBackThirdOrderList(byTimeStart, byTimeEnd, suppliersID, byOrderStatus, byReBackType, searchName, devSort, startSize, endSize);
        }
        public int GetByPageReBackThirdOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string byOrderStatus, string byReBackType, string searchName)
        {
            return dal.GetByPageReBackThirdOrderListCount(byTimeStart, byTimeEnd, suppliersID, byOrderStatus, byReBackType, searchName);
        }

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
        public object GetByPageReBackDataThirdGoodsList(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string reBackType, string searchName, int startSize, int endSize)
        {
            return dal.GetByPageReBackDataThirdGoodsList(byTimeStart, byTimeEnd, suppliersID, orderStatus, reBackType, searchName, startSize, endSize);
        }
        public int GetByPageReBackDataThirdGoodsListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string reBackType, string searchName)
        {
            return dal.GetByPageReBackDataThirdGoodsListCount(byTimeStart, byTimeEnd, suppliersID, orderStatus, reBackType, searchName);
        }

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
        public object GetByPageUnusualTradeList(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName, int startSize, int endSize, out decimal moneyAmount)
        {
            return dal.GetByPageUnusualTradeList(byTimeStart, byTimeEnd, byComfirmType, byPayID, searchName, startSize, endSize, out moneyAmount);
        }
        public int GetByPageUnusualTradeListCount(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName)
        {
            return dal.GetByPageUnusualTradeListCount(byTimeStart, byTimeEnd, byComfirmType, byPayID, searchName);
        }

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
        public object GetByPageUnusualRebackTradeList(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName, int startSize, int endSize, out decimal moneyAmount)
        {
            return dal.GetByPageUnusualRebackTradeList(byTimeStart, byTimeEnd, byComfirmType, byPayID, searchName, startSize, endSize, out moneyAmount);
        }
        public int GetByPageUnusualRebackTradeListCount(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName)
        {
            return dal.GetByPageUnusualRebackTradeListCount(byTimeStart, byTimeEnd, byComfirmType, byPayID, searchName);
        }
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
        public bool UploadImgFile(string sysID, string fileUrl_1, string fileUrl_2, string type)
        {
            return dal.UploadImgFile(sysID, fileUrl_1, fileUrl_2, type);
        }

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
        public bool AddGoodsInfo(int CateID, decimal MarketPrice, decimal ShopPrice, int BrandID, string GoodsImg, string GoodsThumb, string OriginalImg, int SuppliersID, string GoodsSn, string GoodsName, int GoodsNumber, string GoodsBrief, int IsReal, int IsNew, int IsHot, double Weight, string SellerNote, string FareTemp, string adminName)
        {
            return dal.AddGoodsInfo(CateID, MarketPrice, ShopPrice, BrandID, GoodsImg, GoodsThumb, OriginalImg, SuppliersID, GoodsSn, GoodsName, GoodsNumber, GoodsBrief, IsReal, IsNew, IsHot, Weight, SellerNote, FareTemp, adminName);
        }

        /// <summary>
        /// 分配商品给指定的公司-集团推荐商品
        /// </summary>
        /// <param name="goodsID"></param>
        /// <param name="cpyInfoList"></param>
        /// <returns></returns>
        public bool AssignGoodsToCpy(string goodsID, string[] cpyInfoList) { return dal.AssignGoodsToCpy(goodsID, cpyInfoList); }

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
        public bool AddNewFareTemplate(string fareName, string fareAddress, int fareTime, int fareType, Model.DefaultFareTemp defaultFareTemp, DB.Model.FareCarryTemp[] fareCarryTempList)
        {
            return dal.AddNewFareTemplate(fareName, fareAddress, fareTime, fareType, defaultFareTemp, fareCarryTempList);
        }

        /// <summary>
        /// 删除运费模板--同时删除相对应得配送
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public bool DeleteFareTemplate(string sysID)
        {
            return dal.DeleteFareTemplate(sysID);
        }

        /// <summary>
        /// 更新模块分配
        /// </summary>
        /// <param name="adminID">用户ID</param>
        /// <param name="modaleList"></param>
        /// <returns></returns>
        public bool AssignModale(string adminID, string[] modaleList)
        {
            return dal.AssignModale(adminID, modaleList);
        }

        /// <summary>
        /// 添加退货订单信息
        /// </summary>
        /// <param name="backInfoList"></param>
        /// <param name="phone"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddReBackOrder(List<Model.ResBackGoodsInfo> backInfoList, string phone, int type, string remark, string rebackName, string orderSysID, out string outEsg)
        {
            return dal.AddReBackOrder(backInfoList, phone, type, remark, rebackName, orderSysID, out outEsg);
        }

        /// <summary>
        /// 接收从XF系统返回的退款信息生成退款流水
        /// </summary>
        /// <param name="reBackOrderInfo"></param>
        /// <param name="tradeSysID"></param>
        /// <param name="reBackTradeNo"></param>
        /// <param name="reBackMoney"></param>
        /// <returns></returns>
        public bool CreatReBackTradeByRetrunFromXFRefundAcc(Model.ResBackTrasd reBackOrderInfo, string tradeSysID, string reBackTradeNo, decimal reBackMoney)
        {
            return dal.CreatReBackTradeByRetrunFromXFRefundAcc(reBackOrderInfo, tradeSysID, reBackTradeNo, reBackMoney);
        }

        /// <summary>
        /// 审核退换货申请是否通过
        /// </summary>
        /// <param name="reBackID"></param>
        /// <param name="authType"></param>
        /// <param name="remark"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool ComfirmReBackOrderApply(string reBackID, int authType, string remark, string address, string orderID)
        {
            return dal.ComfirmReBackOrderApply(reBackID, authType, remark, address, orderID);
        }

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
        public bool UpdateOrderInfoFromXFPayment(string orderUnifySn, string orderSn, decimal orderAmount, string tradeNo, DateTime tradeTime, M_Payment paymentInfo, string cpySysID, string cpyName, out string resMsg)
        {
            return dal.UpdateOrderInfoFromXFPayment(orderUnifySn, orderSn, orderAmount, tradeNo, tradeTime, paymentInfo, cpySysID, cpyName, out resMsg);
        }

        /// <summary>
        /// 第三方管理员对需要换货的商品进行换货处理
        /// </summary>
        /// <param name="reBackId"></param>
        /// <param name="logisticalName"></param>
        /// <param name="logisticalNumber"></param>
        /// <returns></returns>
        public bool ComfirmGoodsForCustomer(string reBackID, string logisticalName, string logisticalNumber, string orderID)
        {
            return dal.ComfirmGoodsForCustomer(reBackID, logisticalName, logisticalNumber, orderID);
        }
        #endregion

        #region 数据操作

        /// <summary>
        /// 删除类目数据（更改状态）
        /// </summary>
        /// <param name="sysID">类目ID</param>
        /// <returns></returns>
        public bool DeleteCategory(int sysID)
        {
            return dal.DeleteCategory(sysID);
        }

        /// <summary>
        /// 启禁类目数据（更改启禁）
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public bool IsEnableCategory(int sysID, int status)
        {
            return dal.IsEnableCategory(sysID, status);
        }

        /// <summary>
        /// 删除文件管理图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool DeleteFileManage(string fileName)
        {
            return dal.DeleteFileManage(fileName);
        }
        #endregion

        #region 记录错误

        /// <summary>
        /// 记录错误信息进错误信息表中--全程try-catch如果连不上数据库直接返回false
        /// </summary>
        /// <param name="errName">错误描述名称</param>
        /// <param name="errDesc">详细错误描述</param>
        /// <returns></returns>
        public bool RecordErrInfoNote(string errName, string errDesc)
        {
            return dal.RecordErrInfoNote(errName, errDesc);
        }

        #endregion
    }
}