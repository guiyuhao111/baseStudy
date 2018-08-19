using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.DALFactory;
using DSMTMALL.DB.IDAL;
using DSMTMALL.DB.Model;
using System;
using System.Collections.Generic;

namespace DSMTMALL.DB.BLL
{
    public partial class MY_Bll
    {
        private readonly IMY_Dal dal = null;
        public MY_Bll(DBEnum dbEnum)
        {
            dal = FactoryHelper.CreateMY_Dal(dbEnum);
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
        public T GetModel<T>(string strWhere,  object objParam, string orderBy = null, string tableID=null) where T : new()
        {
            List<T> resInfo= dal.GetModelList<T>(1, strWhere, orderBy, objParam, tableID);
            if (resInfo != null && resInfo.Count>0) { return resInfo[0]; }else { return default(T); }
        }
        /// <summary>
        /// 获取实体集合
        /// </summary>
        public List<T> GetModelList<T>( string strWhere, object objParam, string orderBy=null, int topNum = 0, string tableID=null) where T : new()
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
        public int GetModelListCount<T>(string strWhere, object objParam, string tableID=null) where T : new()
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
        public bool AddModel<T>( T objParam, string tableID = null) where T : new()
        {
            List<T> tList = new List<T>();
            tList.Add(objParam);
            return dal.AddModelList(tList, tableID);
        }

        /// <summary>
        /// 添加实体对象集合
        /// </summary>
        public bool AddModelList<T>(List<T> tList, string tableID=null) where T : new()
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
        public bool AddModelList<T>(string strSql, List<T> tList, string tableID=null) where T : new()
        {
            return AddModelList<T>(strSql, tList, tableID);
        }

        /// <summary>
        /// 修改实体对象集合
        /// </summary>
        public bool UpdateModel<T>(string strWhere, object objParam, string tableID=null) where T : new()
        {
            List<object> objList = new List<object>();
            objList.Add(objParam);
            return dal.UpdateModelList<T>(strWhere, objList, tableID);
        }

        /// <summary>
        /// 修改实体对象集合
        /// </summary>
        public bool UpdateModelList<T>(string strWhere, List<object> objList, string tableID=null) where T : new()
        {
            return dal.UpdateModelList<T>(strWhere, objList, tableID);
        }

        /// <summary>
        /// 修改更新实体数据库数据(自定义sql语句)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql执行更新语句</param>
        /// <param name="objList">批量要更新的数据集合</param>
        /// <param name="tableID">分表ID</param>
        /// <returns></returns>
        public bool UpdateModelSQL<T>(string strSql, List<object> objList, string tableID=null) where T : new()
        {
            return dal.UpdateModelSQL<T>(strSql, objList, tableID);
        }

        /// <summary>
        /// 删除实体对象集合
        /// </summary>
        public bool DeleteModel<T>(string strWhere, object objParam, string tableID=null) where T : new()
        {
            return dal.DeleteModelList<T>(strWhere, objParam, tableID);
        }

        #endregion 通用

        #region 获取数据

        /// <summary>
        /// 首页加载公司推荐产品
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetGoodsListByCpyIndexPage(object objParam)
        {
            return dal.GetGoodsListByCpyIndexPage(objParam);
        }

        /// <summary>
        /// 获取用户的购物车列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public object GetUserCartList(string url, string userID)
        {
            return dal.GetUserCartList(url, userID);
        }

        /// <summary>
        /// 获取用户地址信息列表
        /// </summary>
        /// <typeparam name="UserAddressToOrder"></typeparam>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<UserAddressToOrder> GetUserAddressInfo<UserAddressToOrder>(string userID)
        {
            return dal.GetUserAddressInfo<UserAddressToOrder>(userID, 0);
        }
        
        /// <summary>
        /// 用户提交购物车获取购物车中该商品的限购数量与购买数量
        /// </summary>
        /// <param name="cartIDList"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CartGoodsNumber> GetCartGoodsNumberInfo(string[] cartIDList, string userID)
        {
            return dal.GetCartGoodsNumberInfo(cartIDList, userID);
        }

        /// <summary>
        /// 获取用户今天已下单的所有商品购买数量
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public List<UserTodayBuyOrderGoods> GetUserTodayBuyOrderGoodsInfo(string userID, DateTime startTime)
        {
            return dal.GetUserTodayBuyOrderGoodsInfo(userID, startTime);
        }

        /// <summary>
        /// 获取当前ID地址信息-提交订单时使用
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="addressID"></param>
        /// <returns></returns>
        public UserAddressToOrder GetUserAddressInfo<UserAddressToOrder>(string userID, int addressID) {
          
            var resInfo= dal.GetUserAddressInfo<UserAddressToOrder>(userID, addressID);
            return (resInfo != null && resInfo.Count>0) ? resInfo[0] : default(UserAddressToOrder);
        }

        /// <summary>
        /// 提交购物车订单时获取用户购物车商品信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<GoodsCartToOrder> GetUserCartLsitToOrder(string url, string userID, string[] cartIDList, string provience) {
            return dal.GetUserCartLsitToOrder(url, userID, cartIDList,provience);
        }
      
        #endregion

        #region 页面载入

        /// <summary>
        /// 首页加载产品
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetGoodsListByIndexPage(object objParam)
        {
            return dal.GetGoodsListByIndexPage(objParam);
        }

        /// <summary>
        /// 首页加载产品
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetGoodsListByHomePage(object objParam)
        {
            return dal.GetGoodsListByHomePage(objParam);
        }

        /// <summary>
        /// 商品显示列表
        /// </summary>
        /// <param name="type">类目</param>
        /// <param name="sort">排序</param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetGoodsListByPage(string url, int type, string searchName, int sort, int startSize, int endSize)
        {
            return dal.GetGoodsListByPage(url,type,searchName,sort,startSize,endSize);
        }

        /// <summary>
        /// 获取用的订单记录表
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="statusType"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetUserOrderList(string sysID, string statusType, int startSize, int endSize)
        {
            return dal.GetUserOrderList(sysID, statusType, startSize, endSize);
        }

        /// <summary>
        /// 获取订单的产品信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public object GetUserOrderGoods(string url, string orderID)
        {
            return dal.GetUserOrderGoods(url, orderID);
        }
        #endregion

        #region 事务处理

        /// <summary>
        /// 提交订单待付款
        /// </summary>
        /// <param name="goodsCarts">订单的商品集合</param>
        /// <param name="orderUnifySn">统一下单ID</param>
        /// <param name="orderSnList">分单集合</param>
        /// <param name="userID">用户ID</param>
        /// <param name="payMonery">需支付的总价格</param>
        /// <param name="addressInfo">地址信息实体类</param>
        /// <param name="feeAmount">运费价格</param>
        /// <param name="payment">支付信息实体类</param>
        /// <param name="outErrInfo">返回的错误信息</param>
        /// <param name="feeAmountPrice">运费满总价格减免金额</param>
        /// <returns></returns>
        public bool SubmitOrderList(List<GoodsCartToOrder> goodsCarts, string orderUnifySn, List<string> orderSnList, string userID, decimal payMonery, UserAddressToOrder addressInfo, Dictionary<int, double> feeAmount, M_Payment payment, out string outErrInfo, string openID) {
            return dal.SubmitOrderList(goodsCarts, orderUnifySn, orderSnList, userID, payMonery, addressInfo, feeAmount, payment, out outErrInfo, openID);
        }

        /// <summary>
        /// 绑定用户信息（注册）
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="openID"></param>
        /// <param name="pwd"></param>
        /// <param name="userName"></param>
        /// <param name="sex"></param>
        /// <param name="cpyName"></param>
        /// <param name="cpySysID"></param>
        /// <returns></returns>
        public bool BingUserAccount(string phone, string openID, string pwd, BindingInfo userInfo) {
            return dal.BingUserAccount(phone, openID, pwd, userInfo);
        }

        /// <summary>
        /// 去掉订单释放库存
        /// </summary>
        /// <param name="orderArr">订单商品集合</param>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        public bool CancelOrder(string orderID) {
            return dal.CancelOrder(orderID);
        }

        /// <summary>
        /// 同步程序去订单释库存
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public bool CancelOrderSync(string orderID)
        {
            return dal.CancelOrderSync(orderID);
        }

        /// <summary>
        /// 更新订单状态信息为支付异常
        /// </summary>
        /// <param name="pushType">推送类型统一支付还是分单支付</param>
        /// <param name="pushOrderSn">推送的订单Sn</param>
        /// <returns></returns>
        public bool UpdateOrderStatusByXFPaymentToUnusual(string pushType, string pushOrderSn, string pushDsc) {
            return dal.UpdateOrderStatusByXFPaymentToUnusual(pushType, pushOrderSn,pushDsc);
        }

        /// <summary>
        /// 更新订单信息-从付款系统返回支付结果
        /// </summary>
        /// <param name="orderInfoList">订单信息集合</param>
        /// <param name="feeAmount">运费价格</param>
        /// <param name="orderAmount">订单支付总价格</param>
        /// <param name="tradeNo">流水号</param>
        /// <param name="tradeTime">流水号生成时间</param>
        /// <param name="paymentInfo">支付方式实体类</param>
        /// <param name="backUnifyOrderSn">统一下单ID</param>
        /// <param name="backOrderSn">子订单ID</param>
        /// <returns></returns>
        public bool UpdateOrderInfoFromXFPayment(List<M_OrderInfo> orderInfoList, decimal orderAmount, string tradeNo, DateTime tradeTime, M_Payment paymentInfo, string backUnifyOrderSn, string backOrderSn, string phone, string cpySysID, string cpyName)
        {
            return dal.UpdateOrderInfoFromXFPayment(orderInfoList, orderAmount, tradeNo, tradeTime, paymentInfo, backUnifyOrderSn, backOrderSn, phone, cpySysID, cpyName);
        }

        /// <summary>
        /// 更新从XF系统返回的流水号同步状态
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        public bool UpdateOrderStatusByXFPaymentToComfirm(decimal tradeMoney, string tradeSysID, string payStatus,out string openID)
        {
            return dal.UpdateOrderStatusByXFPaymentToComfirm(tradeMoney, tradeSysID, payStatus,out openID);
        }

        /// <summary>
        /// XF系统同步扣款成功后回调商品接口
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeNo"></param>
        /// <param name="payStatus"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateOrderStatusByXFPaymentToComfirm(decimal tradeMoney, string tradeNo, string payStatus, out string msg, out string openID)
        {
            return dal.UpdateOrderStatusByXFPaymentToComfirm(tradeMoney, tradeNo, payStatus, out msg,out openID);
        }


        /// <summary>
        /// 更新从XF系统返回的退款流水号同步状态
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        public bool UpdateReBackPayAccByXFPaymentToComfirm(decimal tradeMoney, string tradeSysID, string payStatus,out string openID)
        {
            return dal.UpdateReBackPayAccByXFPaymentToComfirm(tradeMoney, tradeSysID, payStatus,out openID);
        }

        /// <summary>
        /// 更新从XF系统返回的退款流水号同步状态
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        public bool UpdateReBackPayAccByXFPaymentToComfirm(decimal tradeMoney, string tradeNo, string payStatus, out string msg, out string openID)
        {
            return dal.UpdateReBackPayAccByXFPaymentToComfirm(tradeMoney, tradeNo, payStatus, out msg, out openID);
        }

        #endregion
    }
}
