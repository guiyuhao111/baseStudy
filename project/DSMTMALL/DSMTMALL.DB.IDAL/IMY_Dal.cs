using DSMTMALL.DB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSMTMALL.DB.IDAL
{
    public interface IMY_Dal
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
        bool UpdateModelSQL<T>(string strSql, List<object> objList, string tableID) where T : new();

        /// <summary>
        /// 删除实体对象集合
        /// </summary>
        bool DeleteModelList<T>(string strWhere, object objParam, string tableID) where T : new();

        #endregion 通用

        #region 数据获取

        /// <summary>
        /// 首页加载公司推荐产品
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetGoodsListByCpyIndexPage(object objParam);

        /// <summary>
        /// 获取用户的购物车列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        object GetUserCartList(string url, string userID);

        /// <summary>
        /// 用户提交购物车获取购物车中该商品的限购数量与购买数量
        /// </summary>
        /// <param name="cartIDList"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<CartGoodsNumber> GetCartGoodsNumberInfo(string[] cartIDList, string userID);

        /// <summary>
        /// 获取用户今天已下单的所有商品购买数量
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        List<UserTodayBuyOrderGoods> GetUserTodayBuyOrderGoodsInfo(string userID, DateTime startTime);

        /// <summary>
        /// 获取当前ID地址信息-提交订单时使用
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="addressID"></param>
        /// <returns></returns>
        List<UserAddressToOrder> GetUserAddressInfo<UserAddressToOrder> (string userID, int addressID);

        /// <summary>
        /// 提交购物车订单时获取用户购物车商品信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<GoodsCartToOrder> GetUserCartLsitToOrder(string url, string userID, string[] cartIDList, string provience);
       

        #endregion

        #region 页面载入

        /// <summary>
        /// 首页加载产品
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetGoodsListByIndexPage(object objParam);

        /// <summary>
        /// 首页加载产品
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetGoodsListByHomePage(object objParam);

        /// <summary>
        /// 商品显示列表
        /// </summary>
        /// <param name="type">类目</param>
        /// <param name="sort">排序</param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetGoodsListByPage(string url, int type, string searchName, int sort, int startSize, int endSize);

        /// <summary>
        /// 获取用的订单记录表
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="statusType"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        object GetUserOrderList(string sysID, string statusType, int startSize, int endSize);

        /// <summary>
        /// 获取订单的产品信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        object GetUserOrderGoods(string url, string orderID);
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
        bool SubmitOrderList(List<GoodsCartToOrder> goodsCarts, string orderUnifySn, List<string> orderSnList, string userID, decimal payMonery, UserAddressToOrder addressInfo, Dictionary<int, double> feeAmount, M_Payment payment, out string outErrInfo, string openID);

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
        bool BingUserAccount(string phone, string openID, string pwd, BindingInfo userInfo);

        /// <summary>
        /// 去掉订单释放库存
        /// </summary>
        /// <param name="orderArr">订单商品集合</param>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        bool CancelOrder(string orderID);

        /// <summary>
        /// 同步程序去订单释库存
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        bool CancelOrderSync(string orderID);

        /// <summary>
        /// 更新订单状态信息为支付异常
        /// </summary>
        /// <param name="pushType">推送类型统一支付还是分单支付</param>
        /// <param name="pushOrderSn">推送的订单Sn</param>
        /// <returns></returns>
        bool UpdateOrderStatusByXFPaymentToUnusual(string pushType, string pushOrderSn, string pushDsc);

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
        bool UpdateOrderInfoFromXFPayment(List<M_OrderInfo> orderInfoList,decimal orderAmount, string tradeNo, DateTime tradeTime, M_Payment paymentInfo, string backUnifyOrderSn, string backOrderSn, string phone, string cpySysID, string cpyName);

        /// <summary>
        /// 更新从XF系统返回的流水号同步状态
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        bool UpdateOrderStatusByXFPaymentToComfirm(decimal tradeMoney, string tradeSysID, string payStatus,out string openID);
        bool UpdateOrderStatusByXFPaymentToComfirm(decimal tradeMoney, string tradeNo, string payStatus, out string msg, out string openID);

        /// <summary>
        /// 更新从XF系统返回的退款流水号同步状态
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        bool UpdateReBackPayAccByXFPaymentToComfirm(decimal tradeMoney, string tradeSysID, string payStatus,out string openID);

        /// <summary>
        /// 更新从XF系统返回的退款流水号同步状态
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        bool UpdateReBackPayAccByXFPaymentToComfirm(decimal tradeMoney, string tradeNo, string payStatus, out string msg, out string openID);
        #endregion


    }
}
