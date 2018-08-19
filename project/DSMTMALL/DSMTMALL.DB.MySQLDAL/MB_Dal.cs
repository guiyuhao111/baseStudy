using Dapper;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.IDAL;
using DSMTMALL.DB.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace DSMTMALL.DB.MySQLDAL
{
    public partial class MB_Dal : IMB_Dal
    {
        private readonly string connectionString = string.Empty;
        private readonly string providerName = "MySql.Data.MySqlClient";
        private readonly int commandTimeout = MyDBHelper.COMMANDTIMEOUT;
        public MB_Dal(DBEnum dbEnum)
        {
            connectionString = MyDBHelper.GetConnectionString(dbEnum);
            providerName = MyDBHelper.GetProviderName(dbEnum);
        }
        #region 通用

        /// <summary>
        /// 获取实体集合
        /// </summary>
        public List<T> GetModelList<T>(int topNum, string strWhere, string orderBy, object objParam, string tableID) where T : new()
        {
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT * FROM `");
                strSql.Append(typeof(T).Name);
                if (!string.IsNullOrEmpty(tableID))
                {
                    strSql.Append("_");
                    strSql.Append(tableID);
                }
                strSql.Append("`");
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.Append(" WHERE ");
                    strSql.Append(strWhere);
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    strSql.Append(" ORDER BY ");
                    strSql.Append(orderBy);
                }
                if (topNum > 0)
                {
                    strSql.Append(" LIMIT " + topNum.ToString());
                }
                return connection.Query<T>(strSql.ToString(), objParam, commandTimeout: commandTimeout).AsList();
            }
        }

        /// <summary>
        /// 获取查询的结果集合
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="myQuery"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetModelList(string strSql, object objParam)
        {
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql, objParam, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// 获取实体集合数量
        /// </summary>
        public int GetModelListCount<T>(string strWhere, object objParam, string tableID) where T : new()
        {
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT COUNT(*) FROM `");
                strSql.Append(typeof(T).Name);
                if (!string.IsNullOrEmpty(tableID))
                {
                    strSql.Append("_");
                    strSql.Append(tableID);
                }
                strSql.Append("`");
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.Append(" WHERE ");
                    strSql.Append(strWhere);
                }
                return connection.Query<int>(strSql.ToString(), objParam, commandTimeout: commandTimeout).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取查询的结果集合计数
        /// </summary>
        /// <param name="myQuery"></param>
        /// <returns></returns>
        public int GetModelListCount(string strSql, object objParam)
        {
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql, objParam, commandTimeout: commandTimeout).SingleOrDefault();
            }
        }

        /// <summary>
        /// 添加实体对象集合
        /// </summary>
        public bool AddModelList<T>(List<T> tList, string tableID) where T : new()
        {
            if (tList != null && tList.Count > 0)
            {
                using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("INSERT INTO `");
                            strSql.Append(typeof(T).Name);
                            if (!string.IsNullOrEmpty(tableID))
                            {
                                strSql.Append("_");
                                strSql.Append(tableID);
                            }
                            strSql.Append("` (");
                            strSql.Append(EntityHelper<T>.EntityToAddSQLString(true));
                            strSql.Append(") VALUES (");
                            strSql.Append(EntityHelper<T>.EntityToAddSQLString(false));
                            strSql.Append(")");
                            if (connection.Execute(strSql.ToString(), tList, transaction: transaction, commandTimeout: commandTimeout) != tList.Count)
                            {
                                throw new Exception();
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 添加实体对象集合（自定义sql语句）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql插入语句</param>
        /// <param name="tList">批量插入sql数据集合</param>
        /// <param name="tableID">分表ID</param>
        /// <returns></returns>
        public bool AddModelList<T>(string strSql, List<T> tList, string tableID) where T : new()
        {
            if (tList != null && tList.Count > 0)
            {
                using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            if (connection.Execute(strSql, tList, transaction: transaction, commandTimeout: commandTimeout) != tList.Count)
                            {
                                throw new Exception();
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 修改实体对象集合
        /// </summary>
        public bool UpdateModelList<T>(string strWhere, List<object> objList, string tableID) where T : new()
        {
            if (objList != null && objList.Count > 0)
            {
                using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("UPDATE `");
                            strSql.Append(typeof(T).Name);
                            if (!string.IsNullOrEmpty(tableID))
                            {
                                strSql.Append("_");
                                strSql.Append(tableID);
                            }
                            strSql.Append("` SET ");
                            strSql.Append(EntityHelper<T>.EntityToUpdateSQLString(objList[0]));
                            if (!string.IsNullOrEmpty(strWhere))
                            {
                                strSql.Append(" WHERE ");
                                strSql.Append(strWhere);
                            }
                            if (connection.Execute(strSql.ToString(), objList, transaction: transaction, commandTimeout: commandTimeout) != objList.Count)
                            {
                                throw new Exception();
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 修改更新实体数据库数据(自定义sql语句)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">sql执行更新语句</param>
        /// <param name="objList">批量要更新的数据集合</param>
        /// <param name="tableID">分表ID</param>
        /// <returns></returns>
        public bool UpdateModelSQL(string strSql, List<object> objList, string tableID)
        {
            if (objList != null && objList.Count > 0)
            {
                using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            if (connection.Execute(strSql.ToString(), objList, transaction: transaction, commandTimeout: commandTimeout) != objList.Count)
                            {
                                throw new Exception();
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 删除实体对象集合
        /// </summary>
        public bool DeleteModelList<T>(string strWhere, object objParam, string tableID) where T : new()
        {
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("DELETE FROM `");
                        strSql.Append(typeof(T).Name);
                        if (!string.IsNullOrEmpty(tableID))
                        {
                            strSql.Append("_");
                            strSql.Append(tableID);
                        }
                        strSql.Append("`");
                        if (!string.IsNullOrEmpty(strWhere))
                        {
                            strSql.Append(" WHERE ");
                            strSql.Append(strWhere);
                        }
                        else
                        {
                            throw new Exception();
                        }
                        if (connection.Execute(strSql.ToString(), objParam, transaction: transaction, commandTimeout: commandTimeout) != 1)//只允许删除一条数据
                        {
                            throw new Exception();
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        #endregion 通用

        #region 获取数据

        /// <summary>
        /// 获取类目集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetCategoryList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT C.CateID,C.CateName AS CateName_2,C.ParentID,C1.CateName AS CateName_1 FROM `M_Category` AS C ");
            strSql.Append(" LEFT JOIN `M_Category` AS C1 ON C.ParentID = C1.CateID WHERE 1=1 ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), null, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// 获取用户的模块集合
        /// </summary>
        /// <returns></returns>
        public List<Model.M_Modal> GetModalList(string adminID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM M_Modal AS M  LEFT JOIN M_AdminAction AS A ON  M.ModalSysID= A.ModalSysID WHERE A.AdminSysID = @_AdminSysID ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<Model.M_Modal>(strSql.ToString(), new { _AdminSysID = adminID }, commandTimeout: commandTimeout).ToList();
            }
        }

        /// <summary>
        /// 获取商品信息（用户EXCEL导出）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetGoodsInfo(string type, string url)
        {
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            strSql.Append(" SELECT CP.CateName AS '一级类目',C.CateName AS '二级类目',B.BrandName AS '品牌名',S.SuppliersName AS '供应来源',G.GoodsSn AS '商品编号',G.GoodsName AS '商品名称', ");
            strSql.Append(" G.GoodsNumber AS '商品库存',G.SaleNumber AS '已售数量' ,G.MarketPrice AS '市场价格',G.ShopPrice AS '本店售价',G.GoodsBrief AS '简介',G.GoodsDesc AS '详情描述' FROM M_Goods AS G  ");
            strSql.Append(" LEFT JOIN M_Category AS C ON G.CateID = C.CateID  ");
            strSql.Append(" LEFT JOIN M_Category AS CP ON C.ParentID = CP.CateID ");
            strSql.Append(" LEFT JOIN M_Brand AS B ON G.BrandID = B.BrandID ");
            strSql.Append(" LEFT JOIN M_Suppliers AS S ON G.SuppliersID = S.SuppliersID ");
            strSql.Append(" WHERE  G.IsDelete= 0 ");
            if (type == "AllList")
            {
                ds = MyDBHelper.GetDataSetQuery(strSql.ToString(), null, providerName, connectionString, commandTimeout);
            }
            else if (type == "SelectList")
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();//申明一个字典
                if (!string.IsNullOrEmpty(url))
                {
                    string[] urlArr = url.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] urlList;
                    if (urlArr != null)//不为空
                    {
                        foreach (var item in urlArr)//循环遍历数组
                        {
                            urlList = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);//再次切割字符串
                            if (urlList != null && urlList.Length == 2)//判断切割后的不为空，并且长度为2
                            {
                                if (!dic.Keys.Contains(urlList[0]))//再次判断字典中是否存在这个值
                                {
                                    dic.Add(urlList[0], urlList[1]);//往字典中添加这个键值对
                                }
                            }
                        }
                    }
                } //准备工作已完成
                if (dic.ContainsKey("byCateType"))
                {
                    strSql.Append(" AND(G.CateID = @CateID OR C.ParentID = @CateID) ");
                }
                if (dic.ContainsKey("bySuppliers"))
                {
                    strSql.Append(" AND G.SuppliersID = @SuppliersID ");
                }
                if (dic.ContainsKey("byIsEnable"))
                {
                    strSql.Append(" AND G.IsEnable = @IsEnable ");
                }
                if (dic.ContainsKey("searchName"))
                {
                    strSql.Append(" AND ( G.GoodsSn LIKE @SearchName OR G.GoodsName LIKE @SearchName)  ");
                }
                MySqlParameter[] mySqlParameterArr =
                {
                     new MySqlParameter("@CateID",MySqlDbType.Int32) { Value= dic.ContainsKey("byCateType") ? ToolHelper.ConventToInt32(dic["byCateType"]) : 0 },
                     new MySqlParameter("@SuppliersID",MySqlDbType.Int32) {Value =dic.ContainsKey("bySuppliers") ? ToolHelper.ConventToInt32(dic["bySuppliers"]):0 },
                     new MySqlParameter("@IsEnable",MySqlDbType.Int32) {Value=dic.ContainsKey("byIsEnable") ? ToolHelper.ConventToInt32(dic["byIsEnable"]):0 },
                     new MySqlParameter("@SearchName",MySqlDbType.VarChar) {Value =dic.ContainsKey("searchName") ? "%"+dic["searchName"]+"%" : string.Empty },
                };
                ds = MyDBHelper.GetDataSetQuery(strSql.ToString(), mySqlParameterArr, providerName, connectionString, commandTimeout);
            }
            return ds;
        }

        /// <summary>
        /// 获取订单信息（用户EXCEL导出）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public DataSet GetOrdersInfo(Dictionary<string, string> dic)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT U.UserName AS '姓名',U.NickName AS '昵称',U.CpyName AS '公司', O.Consignee AS '收货人', O.Mobile AS '联系方式',");
            strSql.Append(" CONCAT( O.CountryName,O.ProvinceName,O.CityName,O.DistrictName,O.Address ) AS '收货地址' , ");
            strSql.Append(" CASE WHEN O.IsVerify =0 THEN '未校对' WHEN O.IsVerify =1 THEN '已校对' ELSE '未知' END '是否校对', ");
            strSql.Append(" CASE WHEN O.OrderStatus=0 THEN '代确认' WHEN O.OrderStatus= 1 THEN '已确认' WHEN O.OrderStatus=2 THEN '已取消' WHEN O.OrderStatus=3 THEN '无效' WHEN O.OrderStatus='4' THEN '退货' WHEN O.OrderStatus=5 THEN '已发货' WHEN O.OrderStatus=7 THEN '订单异常' WHEN O.OrderStatus=8 THEN '已完成' WHEN O.OrderStatus=9 THEN '已推送订单' ELSE '未知' END '订单状态', ");
            strSql.Append(" CASE WHEN O.ShippingStatus=0 THEN '未发货' WHEN O.ShippingStatus=1 THEN '已发货' WHEN O.ShippingStatus=2 THEN '已收货' WHEN O.ShippingStatus=3 THEN '未收到货' WHEN O.ShippingStatus=4 THEN '退货' ELSE '未知' END '物流状态',");
            strSql.Append(" CASE WHEN O.PayStatus =0 THEN '未付款' WHEN O.PayStatus=1 THEN '付款中' WHEN O.PayStatus=2 THEN '已付款' WHEN O.PayStatus=3 THEN '付款失败' WHEN O.PayStatus=4 THEN '付款超时' WHEN O.PayStatus=9 THEN '支付异常' ELSE '未知' END '支付状态', ");
            strSql.Append(" O.OrderUnifySn AS '统一订单编号' ,O.OrderSn AS '订单编号' , O.TPLOrderNo AS '第三方订单编号', O.Postscript AS '订单备注',O.ShippingName AS '配送方式',O.PayName AS '支付名称', O.GoodsAmount AS '商品总金额', O.ShippingFee AS '配送费用', O.MoneyPaid AS '已付款金额' , O.OrderAmount AS '应付款金额',O.AddTime AS '下单时间',O.ConfirmTime AS '确认时间',O.PayTime AS '付款时间',O.Logistical AS '物流公司', ");
            strSql.Append(" O.ShippingTime AS '发货时间' , O.LogisticalNumber AS '物流单号', O.SuppliersName AS '供应商',O.TradeNo AS '资金流水号',GetOrderGoodsList(O.OrderID) AS '货物清单' ");
            strSql.Append(" FROM M_OrderInfo AS O ");
            strSql.Append(" LEFT JOIN M_Users AS U ON O.UserID=U.UserID ");
            strSql.Append(" WHERE 1=1 ");
            if (dic.ContainsKey("byTimeStart"))
            {
                strSql.Append(" AND O.AddTime>=@TimeStart ");
            }
            if (dic.ContainsKey("byTimeEnd"))
            {
                strSql.Append(" AND O.AddTime<=@TimeEnd ");
            }
            if (dic.ContainsKey("suppliersID"))
            {
                strSql.Append(" AND O.SuppliersID =@SuppliersID ");
            }
            if (dic.ContainsKey("orderStatus"))
            {
                if (dic["orderStatus"] == "valid")
                {
                    strSql.Append(" AND ( O.OrderStatus =0 OR  O.OrderStatus =1 OR  O.OrderStatus =4 OR  O.OrderStatus =9 OR  O.OrderStatus =7 ) ");
                }
                else
                {
                    strSql.Append(" AND O.OrderStatus =@OrderStatus ");
                }
            }
            if (dic.ContainsKey("payStatus"))
            {
                strSql.Append(" AND O.PayStatus =@PayStatus ");
            }
            if (dic.ContainsKey("payID"))
            {
                strSql.Append(" AND O.PayID =@PayID ");
            }
            if (dic.ContainsKey("shippingStatus"))
            {
                strSql.Append(" AND O.ShippingStatus =@ShippingStatus ");
            }
            if (dic.ContainsKey("searchName"))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            string outSearchName; string outOrderBy;
            dic.TryGetValue("searchName", out outSearchName);
            dic.TryGetValue("orderBy", out outOrderBy);
            strSql.Append(outOrderBy);
            MySqlParameter[] mySqlParameterArr =
           {
                 new MySqlParameter("@TimeStart", MySqlDbType.DateTime) {Value =ToolHelper.ConventToDateTime( dic.ContainsKey("byTimeStart")? dic["byTimeStart"]:string.Empty ,DateTime.Now)},
                 new MySqlParameter("@TimeEnd", MySqlDbType.DateTime) {Value =ToolHelper.ConventToDateTime(dic.ContainsKey("byTimeEnd")? dic["byTimeEnd"]:string.Empty,DateTime.Now)},
                 new MySqlParameter("@SuppliersID", MySqlDbType.Int32) {Value =ToolHelper.ConventToInt32(dic.ContainsKey("suppliersID")? dic["suppliersID"]:string.Empty,1)},
                 new MySqlParameter("@OrderStatus", MySqlDbType.Int32) {Value =ToolHelper.ConventToInt32(dic.ContainsKey("orderStatus")?dic["orderStatus"]:string.Empty,1) },
                 new MySqlParameter("@PayStatus", MySqlDbType.Int32) {Value =ToolHelper.ConventToInt32(dic.ContainsKey("payStatus")?dic["payStatus"]:string.Empty,1)},
                 new MySqlParameter("@PayID", MySqlDbType.Int32) {Value =ToolHelper.ConventToInt32(dic.ContainsKey("payID")?dic["payID"]:string.Empty,1)},
                 new MySqlParameter("@ShippingStatus", MySqlDbType.Int32) {Value =ToolHelper.ConventToInt32(dic.ContainsKey("shippingStatus")?dic["shippingStatus"]:string.Empty,1) },
                 new MySqlParameter("@SearchName", MySqlDbType.VarChar) {Value ="%"+ outSearchName+"%"}
            };
            DataSet ds = MyDBHelper.GetDataSetQuery(strSql.ToString(), mySqlParameterArr, providerName, connectionString, commandTimeout);
            return ds;
        }

        /// <summary>
        /// 获取当前订单编号的所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetOrderGoodsList(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT G.GoodsName,G.GoodsSn,NG.BuyNumber,G.GoodsNumber,G.GoodsThumb,NG.GoodsPrice,S.SuppliersName,G.SellerNote,G.GoodsID ");
            strSql.Append(" FROM ( SELECT * FROM M_OrderGoods AS OG  WHERE OG.OrderID=@OrderSysID ) AS NG ");
            strSql.Append(" LEFT JOIN M_Goods AS G ON NG.GoodsID=G.GoodsID ");
            strSql.Append(" LEFT JOIN M_Suppliers AS S ON S.SuppliersID= G.SuppliersID ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { OrderSysID = id }, commandTimeout: commandTimeout).Select(info => new
                {
                    info.GoodsName,
                    info.GoodsSn,
                    info.BuyNumber,
                    info.GoodsNumber,
                    info.GoodsThumb,
                    info.GoodsPrice,
                    info.SuppliersName,
                    info.SellerNote,
                    info.GoodsID
                });
            }
        }

        /// <summary>
        /// 获取当前退货订单编号的所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetReBackOrderGoodsList(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT NG.GoodsName,NG.GoodsSn,NG.BuyNumber,NG.RebackNumber,G.GoodsThumb,NG.GoodsPrice,S.SuppliersName,G.SellerNote,NG.GoodsID ");
            strSql.Append(" FROM ( SELECT * FROM M_ReBackGoods AS RG WHERE RG.ReBackID =@ReBackID ) AS NG ");
            strSql.Append(" LEFT JOIN M_Goods AS G ON NG.GoodsID=G.GoodsID ");
            strSql.Append(" LEFT JOIN M_Suppliers AS S ON S.SuppliersID= G.SuppliersID ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { ReBackID = id }, commandTimeout: commandTimeout).Select(info => new
                {
                    info.GoodsName,
                    info.GoodsSn,
                    info.BuyNumber,
                    info.GoodsThumb,
                    info.GoodsPrice,
                    info.SuppliersName,
                    info.SellerNote,
                    info.GoodsID,
                    info.RebackNumber
                });
            }

        }

        /// <summary>
        /// 获取退货订单流水信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Model.ResBackTrasd> GetReBackOrderTrade(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT OT.TradeNo,OT.PayMoney,OT.PayName,OT.PayID,OT.CpySysID,OT.CpyName,OT.PayTime,OT.TradeSysID,NB.* FROM ( SELECT * FROM M_ReBackOrder AS RB  WHERE  RB.ReBackID=@_ReBackID ) AS NB  ");
            strSql.Append(" LEFT JOIN M_OrderInfo AS OI ON OI.OrderID =NB.OrderID ");
            strSql.Append(" LEFT JOIN M_OrderTrade AS OT ON OT.TradeSysID = OI.TradeSysID ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<ResBackTrasd>(strSql.ToString(), new { _ReBackID = id }, commandTimeout: commandTimeout).ToList();
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT G.*,S.SuppliersName,C.CateName,F.FareName FROM M_Goods AS G  ");
            strSql.Append(" LEFT JOIN M_Suppliers AS S ON G.SuppliersID = S.SuppliersID ");
            strSql.Append(" LEFT JOIN M_Category AS C ON G.CateID = C.CateID ");
            strSql.Append(" LEFT JOIN M_FareTemplate AS F ON G.FareSysID=F.FareSysID ");
            strSql.Append(" WHERE  G.IsDelete= 0 ");
            if (!string.IsNullOrEmpty(byCateType))
            {
                strSql.Append(" AND(G.CateID = @CateID OR C.ParentID = @CateID) ");
            }
            if (!string.IsNullOrEmpty(bySuppliers))
            {
                strSql.Append(" AND G.SuppliersID = @SuppliersID ");
            }
            if (!string.IsNullOrEmpty(byIsEnable))
            {
                strSql.Append(" AND G.IsEnable = @IsEnable ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( G.GoodsSn LIKE @SearchName OR G.GoodsName LIKE @SearchName)  ");
            }
            strSql.Append(byOrderBy);
            strSql.Append(" LIMIT @StartSize,@EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(),
                    new
                    {
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize,
                        CateID = ToolHelper.ConventToInt32(byCateType, 1),
                        SuppliersID = ToolHelper.ConventToInt32(bySuppliers, 1),
                        IsEnable = ToolHelper.ConventToInt32(byIsEnable, 1)
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.SuppliersName,
                            info.CateName,
                            info.GoodsID,
                            info.CateID,
                            info.BrandID,
                            info.SuppliersID,
                            info.GoodsSn,
                            info.GoodsName,
                            info.ClickCount,
                            info.GoodsNumber,
                            info.MarketPrice,
                            info.ShopPrice,
                            info.WarnNumber,
                            info.KeyWords,
                            info.GoodsBrief,
                            info.GoodsDesc,
                            info.GoodsThumb,
                            info.GoodsImg,
                            info.OriginalImg,
                            info.IsReal,
                            info.IsEnable,
                            info.AddTime,
                            info.OrderBy,
                            info.IsDelete,
                            info.IsBest,
                            info.IsNew,
                            info.IsHot,
                            info.IsPromote,
                            info.LastUpdate,
                            info.SellerNote,
                            info.SaleNumber,
                            info.Weight,
                            info.FareName,
                            info.ModifyPerson,
                            info.QuotaNumber
                        };
                    });
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) FROM M_Goods AS G ");
            strSql.Append(" LEFT JOIN M_Category AS C ON G.CateID = C.CateID ");
            strSql.Append(" WHERE G.IsDelete= 0 ");
            if (!string.IsNullOrEmpty(byCateType))
            {
                strSql.Append(" AND(G.CateID = @CateID OR C.ParentID = @CateID) ");
            }
            if (!string.IsNullOrEmpty(bySuppliers))
            {
                strSql.Append(" AND G.SuppliersID = @SuppliersID ");
            }
            if (!string.IsNullOrEmpty(byIsEnable))
            {
                strSql.Append(" AND G.IsEnable = @IsEnable ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( G.GoodsSn LIKE @SearchName OR G.GoodsName LIKE @SearchName)  ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(), new { CateID = ToolHelper.ConventToInt32(byCateType), SuppliersID = ToolHelper.ConventToInt32(bySuppliers), IsEnable = ToolHelper.ConventToInt32(byIsEnable), SearchName = "%" + searchName + "%" }, commandTimeout: commandTimeout).SingleOrDefault();
            }
        }

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
        public object GetByPageGoodsListSuppliers(string byCateType, string bySuppliers, string byIsEnable, string byOrderBy, string searchName, int startSize, int endSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT G.*,S.SuppliersName,C.CateName,F.FareName FROM M_Goods AS G  ");
            strSql.Append(" LEFT JOIN M_Suppliers AS S ON G.SuppliersID = S.SuppliersID ");
            strSql.Append(" LEFT JOIN M_Category AS C ON G.CateID = C.CateID ");
            strSql.Append(" LEFT JOIN M_FareTemplate AS F ON G.FareSysID=F.FareSysID ");
            strSql.Append(" WHERE  G.IsDelete= 0 AND G.SuppliersID = @SuppliersID ");
            if (!string.IsNullOrEmpty(byCateType))
            {
                strSql.Append(" AND(G.CateID = @CateID OR C.ParentID = @CateID) ");
            }
            if (!string.IsNullOrEmpty(byIsEnable))
            {
                strSql.Append(" AND G.IsEnable = @IsEnable ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( G.GoodsSn LIKE @SearchName OR G.GoodsName LIKE @SearchName)  ");
            }
            strSql.Append(byOrderBy);
            strSql.Append(" LIMIT @StartSize,@EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(),
                    new
                    {
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize,
                        CateID = ToolHelper.ConventToInt32(byCateType, 1),
                        SuppliersID = ToolHelper.ConventToInt32(bySuppliers, 1),
                        IsEnable = ToolHelper.ConventToInt32(byIsEnable, 1)
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.SuppliersName,
                            info.CateName,
                            info.GoodsID,
                            info.CateID,
                            info.BrandID,
                            info.SuppliersID,
                            info.GoodsSn,
                            info.GoodsName,
                            info.ClickCount,
                            info.GoodsNumber,
                            info.MarketPrice,
                            info.ShopPrice,
                            info.WarnNumber,
                            info.KeyWords,
                            info.GoodsBrief,
                            info.GoodsDesc,
                            info.GoodsThumb,
                            info.GoodsImg,
                            info.OriginalImg,
                            info.IsReal,
                            info.IsEnable,
                            info.AddTime,
                            info.OrderBy,
                            info.IsDelete,
                            info.IsBest,
                            info.IsNew,
                            info.IsHot,
                            info.IsPromote,
                            info.LastUpdate,
                            info.SellerNote,
                            info.SaleNumber,
                            info.Weight,
                            info.FareName,
                            info.ModifyPerson,
                            info.QuotaNumber
                        };
                    });
            }
        }

        /// <summary>
        /// 获取商品列表计数
        /// </summary>
        /// <param name="byCateType"></param>
        /// <param name="bySuppliers"></param>
        /// <param name="byIsEnable"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        public int GetByPageGoodsListSuppliersCount(string byCateType, string bySuppliers, string byIsEnable, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) FROM M_Goods AS G ");
            strSql.Append(" LEFT JOIN M_Category AS C ON G.CateID = C.CateID ");
            strSql.Append(" WHERE G.IsDelete= 0 AND G.SuppliersID = @SuppliersID ");
            if (!string.IsNullOrEmpty(byCateType))
            {
                strSql.Append(" AND(G.CateID = @CateID OR C.ParentID = @CateID) ");
            }
            if (!string.IsNullOrEmpty(byIsEnable))
            {
                strSql.Append(" AND G.IsEnable = @IsEnable ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( G.GoodsSn LIKE @SearchName OR G.GoodsName LIKE @SearchName)  ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(), new { CateID = ToolHelper.ConventToInt32(byCateType), SuppliersID = ToolHelper.ConventToInt32(bySuppliers), IsEnable = ToolHelper.ConventToInt32(byIsEnable), SearchName = "%" + searchName + "%" }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            int outSysID = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT C.*,");
            strSql.Append(" CASE WHEN C.ParentID=0 THEN '一级类目' ELSE C1.CateName END PCateName  ");
            strSql.Append(" FROM M_Category AS C ");
            strSql.Append(" LEFT JOIN M_Category AS C1 ON C.ParentID = C1.CateID ");
            strSql.Append(" WHERE 1=1 AND C.IsDelete = 0 ");
            if (!string.IsNullOrEmpty(sysID) && int.TryParse(sysID, out outSysID))
            {
                strSql.Append(" AND C.ParentID = @ParentID ");
            }
            strSql.Append(" ORDER BY C.OrderBy ASC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { ParentID = outSysID, StartSize = startSize, EndSize = endSize }, commandTimeout: commandTimeout).Select(info =>
                {
                    return new
                    {
                        info.PCateName,
                        info.CateID,
                        info.CateName,
                        info.KeyWords,
                        info.CateDesc,
                        info.ParentID,
                        info.OrderBy,
                        info.ShowInNav,
                        info.ShowImage,
                        info.IsEnable,
                        info.IsDelete
                    };
                });
            }
        }
        public int GetByPageCagegoryListCount(string sysID)
        {
            int outSysID = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*)");
            strSql.Append(" FROM M_Category AS C ");
            strSql.Append(" WHERE 1=1 AND C.IsDelete = 0 ");
            if (!string.IsNullOrEmpty(sysID) && int.TryParse(sysID, out outSysID))
            {
                strSql.Append(" AND C.ParentID = @ParentID ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(), new { ParentID = outSysID }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT B.* FROM M_Brand AS B WHERE 1=1 ");
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( B.BrandName LIKE @SearchName )  ");
            }
            strSql.Append(" ORDER BY B.OrderBy ASC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { SearchName = "%" + searchName + "%", StartSize = startSize, EndSize = endSize }, commandTimeout: commandTimeout).Select(info =>
                {
                    return new
                    {
                        info.BrandID,
                        info.BrandName,
                        info.BrandLogo,
                        info.BrandUrl,
                        info.BrandDesc,
                        info.OrderBy,
                        info.IsShow
                    };
                });
            }
        }
        public int GetByPageBrandListCount(string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) FROM M_Brand AS B WHERE 1=1 ");
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( B.BrandName LIKE @SearchName )  ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(), new { SearchName = "%" + searchName + "%" }, commandTimeout: commandTimeout).SingleOrDefault();
            }
        }

        /// <summary>
        /// 获取轮播数据列表
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetByPageRollList(int startSize, int endSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT R.* ,");
            strSql.Append(" CASE WHEN  R.RollType=10 THEN '宣传' WHEN R.RollType=20 THEN '类目' WHEN R.RollType =30 THEN '商品' ELSE '未知' END NRollType, ");
            strSql.Append(" CASE WHEN  R.RollType=10 THEN A.AdvName WHEN R.RollType=20 THEN C.CateName WHEN R.RollType=30 THEN G.GoodsName ELSE '未知' END NName,");
            strSql.Append(" CASE WHEN  R.RollType=10 THEN '' WHEN R.RollType=20 THEN C.ShowImage WHEN R.RollType=30 THEN G.GoodsThumb ELSE '未知' END NIMG ");
            strSql.Append(" FROM M_Roll AS R ");
            strSql.Append(" LEFT JOIN M_Goods AS G ON R.TargetSysID = G.GoodsID ");
            strSql.Append(" LEFT JOIN M_Category AS C ON R.TargetSysID= C.CateID ");
            strSql.Append(" LEFT JOIN M_Advertisement AS A ON R.TargetSysID =A.AdvSysID ");
            strSql.Append(" ORDER BY R.OrderBy ASC ,R.CreateTime DESC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { StartSize = startSize, EndSize = endSize }, commandTimeout: commandTimeout).Select(info =>
                {
                    return new
                    {
                        info.NName,
                        info.NIMG,
                        info.NRollType,
                        info.RollSysID,
                        info.TargetSysID,
                        info.Picture,
                        info.IsEnable,
                        info.OrderBy,
                        info.ModifyPerson,
                        info.UpdateTime,
                        info.CreateTime
                    };
                });
            }
        }
        public int GetByPageRollListCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) FROM M_Roll  WHERE 1=1 ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(), null, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT U.UserName,U.CpyName,U.NickName,O.Remark, ");
            strSql.Append(" CONCAT( O.CountryName,O.ProvinceName,O.CityName,O.DistrictName,O.Address ) AS NAddress, ");
            strSql.Append(" CASE WHEN O.IsVerify =0 THEN '未校对' WHEN O.IsVerify =1 THEN '已校对' ELSE '未知' END NIsVerify, ");
            strSql.Append(" CASE WHEN O.OrderStatus=0 THEN '代确认' WHEN O.OrderStatus= 1 THEN '已确认' WHEN O.OrderStatus=2 THEN '已取消' WHEN O.OrderStatus=3 THEN '已无效' WHEN O.OrderStatus='4' THEN '已退货' WHEN O.OrderStatus=5 THEN '已发货' WHEN O.OrderStatus=6 THEN '处理中' WHEN O.OrderStatus=7 THEN '订单异常' WHEN O.OrderStatus=8 THEN '已完成' WHEN O.OrderStatus=9 THEN '已推送订单' WHEN O.OrderStatus=10 THEN '已退款' WHEN O.OrderStatus=11 THEN '已换货' ELSE '未知' END NOrderStatus, ");
            strSql.Append(" CASE WHEN O.ShippingStatus=0 THEN '未发货' WHEN O.ShippingStatus=1 THEN '已发货' WHEN O.ShippingStatus=2 THEN '已收货' WHEN O.ShippingStatus=3 THEN '未收到货' WHEN O.ShippingStatus=4 THEN '退货中'WHEN O.ShippingStatus=5 THEN '已完成' ELSE '未知' END NShippingStatus,");
            strSql.Append(" CASE WHEN O.PayStatus =0 THEN '未付款' WHEN O.PayStatus=1 THEN '付款中' WHEN O.PayStatus=2 THEN '已付款' WHEN O.PayStatus=3 THEN '付款失败' WHEN O.PayStatus=4 THEN '付款超时' WHEN O.PayStatus=9 THEN '支付异常' ELSE '未知' END NPayStatus, ");
            strSql.Append(" U.CreditLine,O.*  ");
            strSql.Append(" FROM M_OrderInfo AS O ");
            strSql.Append(" LEFT JOIN M_Users AS U ON O.UserID=U.UserID ");
            strSql.Append(" WHERE 1=1 ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND O.AddTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND O.AddTime<=@TimeEnd ");
            }
            if (!string.IsNullOrEmpty(suppliersID))
            {
                strSql.Append(" AND O.SuppliersID =@SuppliersID ");
            }
            if (!string.IsNullOrEmpty(orderStatus))
            {
                switch (orderStatus)
                {
                    case "10":
                        strSql.Append(" AND ( O.OrderStatus =0 OR  O.OrderStatus =1 OR  O.OrderStatus =5 OR O .OrderStatus =6 OR O.OrderStatus =7 OR  O.OrderStatus =9 ) ");
                        break;
                    case "20":
                        strSql.Append(" AND  O.OrderStatus =1 AND O.ShippingStatus =0 AND PayStatus=2 ");
                        break;
                    case "30":
                        strSql.Append("AND ( O.ShippingStatus =1 OR  O.ShippingStatus=2 OR  O.ShippingStatus=3 OR  O.ShippingStatus=4 OR  O.ShippingStatus=5 ) ");
                        break;
                    case "40":
                        strSql.Append(" AND ( O.OrderStatus =4 OR  O.OrderStatus =6 OR  O.OrderStatus =10 OR O .OrderStatus =11 ) ");
                        break;
                    case "50":
                        strSql.Append(" AND ( O.OrderStatus =2 OR  O.OrderStatus =3 ) ");
                        break;
                    case "60":
                        strSql.Append(" AND O.OrderStatus =8 ");
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(payID))
            {
                strSql.Append(" AND O.PayID =@PayID ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            strSql.Append(devSort);
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, DateTime.Now),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, DateTime.Now),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 1),
                        PayID = ToolHelper.ConventToInt32(payID, 1),
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.UserName,
                            info.CpyName,
                            info.NickName,
                            info.NAddress,
                            info.NIsVerify,
                            info.NOrderStatus,
                            info.NShippingStatus,
                            info.NPayStatus,
                            info.CreditLine,
                            info.OrderID,
                            info.OrderUnifySn,
                            info.OrderSn,
                            info.TPLOrderNo,
                            info.IsVerify,
                            info.UserID,
                            info.CpySysID,
                            info.OrderStatus,
                            info.ShippingStatus,
                            info.PayStatus,
                            info.Consignee,
                            info.Mobile,
                            info.Postscript,
                            info.ShippingID,
                            info.ShippingName,
                            info.PayID,
                            info.PayName,
                            info.HowOos,
                            info.GoodsAmount,
                            info.ShippingFee,
                            info.PayFee,
                            info.MoneyPaid,
                            info.OrderAmount,
                            info.AddTime,
                            info.ConfirmTime,
                            PayTime = (info.PayTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.PayTime,
                            info.Logistical,
                            ShippingTime = (info.ShippingTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.ShippingTime,
                            info.LogisticalNumber,
                            info.ModifyPerson,
                            info.TradeSysID,
                            info.SuppliersName,
                            info.TradeNo,
                            info.Remark,
                            info.LogInfo
                        };
                    });
            }
        }
        public int GetByPageOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string payID, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) ");
            strSql.Append(" FROM M_OrderInfo AS O ");
            strSql.Append(" WHERE 1=1 ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND O.AddTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND O.AddTime<=@TimeEnd ");
            }
            if (!string.IsNullOrEmpty(suppliersID))
            {
                strSql.Append(" AND O.SuppliersID =@SuppliersID ");
            }
            if (!string.IsNullOrEmpty(orderStatus))
            {
                switch (orderStatus)
                {
                    case "10":
                        strSql.Append(" AND ( O.OrderStatus =0 OR  O.OrderStatus =1 OR  O.OrderStatus =5 OR O .OrderStatus =6 OR O.OrderStatus =7 OR  O.OrderStatus =9 ) ");
                        break;
                    case "20":
                        strSql.Append(" AND  O.OrderStatus =1 AND O.ShippingStatus =0 AND PayStatus=2 ");
                        break;
                    case "30":
                        strSql.Append("AND ( O.ShippingStatus =1 OR  O.ShippingStatus=2 OR  O.ShippingStatus=3 OR  O.ShippingStatus=4 OR  O.ShippingStatus=5 ) ");
                        break;
                    case "40":
                        strSql.Append(" AND ( O.OrderStatus =4 OR  O.OrderStatus =6 OR  O.OrderStatus =10 OR O .OrderStatus =11 ) ");
                        break;
                    case "50":
                        strSql.Append(" AND ( O.OrderStatus =2 OR  O.OrderStatus =3 ) ");
                        break;
                    case "60":
                        strSql.Append(" AND O.OrderStatus =8 ");
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(payID))
            {
                strSql.Append(" AND O.PayID =@PayID ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, DateTime.Now),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, DateTime.Now),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 1),
                        PayID = ToolHelper.ConventToInt32(payID, 1),
                        SearchName = "%" + searchName + "%"
                    }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT U.UserName,U.CpyName,U.NickName, ");
            strSql.Append(" CONCAT( O.CountryName,O.ProvinceName,O.CityName,O.DistrictName,O.Address ) AS NAddress, ");
            strSql.Append(" CASE WHEN O.IsVerify =0 THEN '未校对' WHEN O.IsVerify =1 THEN '已校对' ELSE '未知' END NIsVerify, ");
            strSql.Append(" CASE WHEN O.OrderStatus=0 THEN '代确认' WHEN O.OrderStatus= 1 THEN '已确认' WHEN O.OrderStatus=2 THEN '已取消' WHEN O.OrderStatus=3 THEN '无效' WHEN O.OrderStatus='4' THEN '退货' WHEN O.OrderStatus=5 THEN '已发货' WHEN O.OrderStatus=7 THEN '订单异常' WHEN O.OrderStatus=8 THEN '已完成' WHEN O.OrderStatus=9 THEN '已推送订单' ELSE '未知' END NOrderStatus, ");
            strSql.Append(" CASE WHEN O.ShippingStatus=0 THEN '未发货' WHEN O.ShippingStatus=1 THEN '已发货' WHEN O.ShippingStatus=2 THEN '已收货' WHEN O.ShippingStatus=3 THEN '未收到货' WHEN O.ShippingStatus=4 THEN '退货' ELSE '未知' END NShippingStatus,");
            strSql.Append(" CASE WHEN O.PayStatus =0 THEN '未付款' WHEN O.PayStatus=1 THEN '付款中' WHEN O.PayStatus=2 THEN '已付款' WHEN O.PayStatus=3 THEN '付款失败' WHEN O.PayStatus=4 THEN '付款超时' WHEN O.PayStatus=9 THEN '支付异常' ELSE '未知' END NPayStatus, ");
            strSql.Append(" U.CreditLine,O.* ");
            strSql.Append(" FROM M_OrderInfo AS O ");
            strSql.Append(" LEFT JOIN M_Users AS U ON O.UserID=U.UserID ");
            strSql.Append(" WHERE 1=1 AND ( O.OrderStatus=7 OR O.PayStatus=9 OR ( O.PayStatus=1 AND O.PayTime-NOW()<=3600 ) ) ");//订单状态异常或者支付状态异常
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            strSql.Append(" ORDER BY UpdateTime ASC ");
            strSql.Append(" LIMIT @startSize , @endSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { SearchName = "%" + searchName + "%", StartSize = startSize, EndSize = endSize }, commandTimeout: commandTimeout).Select(info =>
                {
                    return new
                    {
                        info.UserName,
                        info.CpyName,
                        info.NickName,
                        info.NAddress,
                        info.NIsVerify,
                        info.NOrderStatus,
                        info.NShippingStatus,
                        info.NPayStatus,
                        info.CreditLine,
                        info.OrderID,
                        info.OrderUnifySn,
                        info.OrderSn,
                        info.TPLOrderNo,
                        info.IsVerify,
                        info.SuppliersName,
                        info.OrderStatus,
                        info.PayStatus,
                        info.AddTime,
                        info.PayName,
                        info.TradeNo,
                        info.MoneyPaid,
                        info.PayTime,
                        info.Postscript,

                    };
                });
            }
        }
        public int GetByPageUnusualOrderListCount(string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) ");
            strSql.Append(" FROM M_OrderInfo AS O ");
            strSql.Append(" WHERE 1=1 AND ( O.OrderStatus=7 OR O.PayStatus=9  OR ( O.PayStatus=1 AND O.PayTime-NOW()<=3600 ) ) ");
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(), new { SearchName = "%" + searchName + "%" }, commandTimeout: commandTimeout).SingleOrDefault();
            }
        }

        /// <summary>
        /// 获取用户的信息列表
        /// </summary>
        /// <param name="byUserType"></param>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetByPageUserList(string byUserType, string searchName, int startSize, int endSize, out int listCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT SQL_CALC_FOUND_ROWS U.UserName, U.NickName,U.Phone,U.RegTime,U.LastLogin,U.CpyName , ");
            strSql.Append(" CASE WHEN U.Sex = 0 THEN '保密' WHEN U.Sex = 1 THEN '男' WHEN U.Sex = 2 THEN '女' END NSex  FROM M_Users AS U  WHERE 1=1 ");
            if (!string.IsNullOrEmpty(byUserType))
            {
                if (byUserType == "0") { strSql.Append(" AND U.CpyName IS NOT NULL AND U.CpyName <> '' "); } else if (byUserType == "1") { strSql.Append(" AND U.CpyName IS NULL OR U.CpyName = '' "); }
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( U.Phone LIKE @SearchName  OR U.UserName LIKE @SearchName OR U.CpyName LIKE @SearchName ) ");
            }
            strSql.Append(" ORDER BY RegTime DESC ");
            strSql.Append(" LIMIT @StartSize,@EndSize;SELECT FOUND_ROWS(); ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                var gridReader = connection.QueryMultiple(strSql.ToString(), new { SearchName = "%" + searchName + "%", StartSize = startSize, EndSize = endSize }, commandTimeout: commandTimeout);
                var recordList = gridReader.Read();
                listCount = gridReader.Read<int>().SingleOrDefault();
                return recordList;
            }
        }
        public int GetByPageUserListCount(string byUserType, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) ");
            strSql.Append(" FROM M_Users AS U WHERE 1=1 ");
            if (!string.IsNullOrEmpty(byUserType))
            {
                if (byUserType == "0") { strSql.Append(" AND U.CpyName IS NOT NULL AND U.CpyName <> '' "); } else if (byUserType == "1") { strSql.Append(" AND U.CpyName IS NULL OR U.CpyName = '' "); }
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( U.NickName LIKE @SearchName  OR U.UserName LIKE @SearchName OR U.CpyName LIKE @SearchName ) ");
            }
            strSql.Append(" ORDER BY RegTime DESC ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(), new { SearchName = "%" + searchName + "%" }, commandTimeout: commandTimeout).SingleOrDefault();
            }
        }

        /// <summary>
        /// 获取管理员用户信息
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetByPageAdminList(string searchName, int startSize, int endSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT A.*,S.SuppliersName,GetAdminModalList(A.AdminID) AS NModalList ");
            strSql.Append(" FROM M_AdminUser  AS A ");
            strSql.Append(" LEFT JOIN M_Suppliers AS S ON S.SuppliersID= A.SuppliersID WHERE 1=1");
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND A.AdminName LIKE @SearchName ");
            }
            strSql.Append(" ORDER BY A.CreateTime DESC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { SearchName = "%" + searchName + "%", StartSize = startSize, EndSize = endSize }, commandTimeout: commandTimeout).Select(info =>
                {
                    return new
                    {
                        info.AdminID,
                        info.SuppliersID,
                        info.SuppliersName,
                        info.AdminName,
                        info.Phone,
                        info.LoginTime,
                        info.IsEnable,
                        info.NoteList,
                        info.Remark,
                        info.UpdateTime,
                        info.CreateTime,
                        info.TokenCardNo,
                        info.NModalList
                    };
                });
            }
        }
        public int GetByPageAdminListCount(string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT Count(*) ");
            strSql.Append(" FROM `M_AdminUser` AS A WHERE 1=1 ");
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND A.AdminName LIKE @SearchName ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(), new { SearchName = "%" + searchName + "%" }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ");
            strSql.Append(" CASE WHEN O.OrderStatus=0 THEN '代确认' WHEN O.OrderStatus= 1 THEN '已确认' WHEN O.OrderStatus=2 THEN '已取消' WHEN O.OrderStatus=3 THEN '无效' WHEN O.OrderStatus='4' THEN '退货' WHEN O.OrderStatus=5 THEN '已发货' WHEN O.OrderStatus=7 THEN '订单异常' WHEN O.OrderStatus=8 THEN '已完成' WHEN O.OrderStatus=9 THEN '已推送订单' ELSE '未知' END NOrderStatus, ");
            strSql.Append(" CASE WHEN O.ShippingStatus=0 THEN '未发货' WHEN O.ShippingStatus=1 THEN '已发货' WHEN O.ShippingStatus=2 THEN '已收货' WHEN O.ShippingStatus=3 THEN '未收到货' WHEN O.ShippingStatus=4 THEN '退货' ELSE '未知' END NShippingStatus,");
            strSql.Append(" CASE WHEN O.PayStatus =0 THEN '未付款' WHEN O.PayStatus=1 THEN '付款中' WHEN O.PayStatus=2 THEN '已付款' WHEN O.PayStatus=3 THEN '付款失败' WHEN O.PayStatus=4 THEN '付款超时' WHEN O.PayStatus=9 THEN '支付异常' ELSE '未知' END NPayStatus, ");
            strSql.Append(" O.* ,GetOrderGoodsList(O.OrderID) AS  NOrderInfo  ");
            strSql.Append(" FROM M_OrderInfo AS O ");
            strSql.Append(" WHERE 1=1 AND O.ShippingTime>=@TimeStart AND O.ShippingTime<=@TimeEnd AND O.SuppliersID = @SuppliersID");
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (orderStatus == "1")
                {
                    strSql.Append(" AND O.OrderStatus =8 ");
                }
                else if (orderStatus == "2")
                {
                    strSql.Append(" AND O.OrderStatus = 5 ");
                }
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn LIKE @SearchName ");
            }
            strSql.Append(devSort);
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 0),
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.NOrderStatus,
                            info.NShippingStatus,
                            info.NPayStatus,
                            info.OrderUnifySn,
                            info.OrderSn,
                            info.TPLOrderNo,
                            info.OrderStatus,
                            info.ShippingStatus,
                            info.PayStatus,
                            info.Postscript,
                            info.OrderAmount,
                            info.AddTime,
                            ConfirmTime = (info.ConfirmTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.ConfirmTime,
                            PayTime = (info.PayTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.PayTime,
                            info.Logistical,
                            ShippingTime = (info.ShippingTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.ShippingTime,
                            info.LogisticalNumber,
                            info.Remark,
                            info.NOrderInfo,
                            info.SuppliersName,
                            info.UpdateTime
                        };
                    });
            }
        }
        public int GetByPageDataOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            DateTime nowTime = DateTime.Now;
            strSql.Append(" SELECT COUNT(*) ");
            strSql.Append(" FROM M_OrderInfo AS O ");
            strSql.Append(" WHERE 1=1 AND O.ShippingTime>=@TimeStart AND O.ShippingTime<=@TimeEnd AND O.SuppliersID = @SuppliersID");
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (orderStatus == "1")
                {
                    strSql.Append(" AND O.OrderStatus = 8 ");
                }
                else if (orderStatus == "2")
                {
                    strSql.Append(" AND O.OrderStatus = 5 ");
                }
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn LIKE @SearchName ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 0),
                        SearchName = "%" + searchName + "%",
                    }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT NG.GoodsSn,NG.GoodsName,SUM( NG.BuyNumber) AS SUMNumber ,NG.GoodsPrice FROM (  ");
            strSql.Append(" SELECT OG.GoodsSn,OG.GoodsName,OG.BuyNumber,OG.GoodsPrice FROM M_OrderInfo AS O  ");
            strSql.Append(" LEFT JOIN  M_OrderGoods AS OG  ON OG.OrderID = O.OrderID  ");
            strSql.Append(" WHERE 1=1 AND O.AddTime>=@TimeStart AND O.AddTime<=@TimeEnd AND O.SuppliersID = @SuppliersID ");
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (orderStatus == "")
                {
                    strSql.Append(" AND ( O.OrderStatus =1 OR  O.OrderStatus =9 OR  O.OrderStatus =5 OR  O.OrderStatus =8  ) ");
                }
                else if (orderStatus == "1")
                {
                    strSql.Append(" AND O.OrderStatus =8 ");
                }
                else if (orderStatus == "2")
                {
                    strSql.Append(" AND ( O.OrderStatus =8 OR O.OrderStatus=5 ) ");
                }
                else if (orderStatus == "3")
                {
                    strSql.Append(" AND O.OrderStatus = 5 ");
                }
                else if (orderStatus == "4")
                {
                    strSql.Append(" AND O.OrderStatus = 4 ");
                }
                else if (orderStatus == "5")
                {
                    strSql.Append(" AND O.OrderStatus = 1 ");
                }
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( OG.GoodsSn LIKE @SearchName OR OG.GoodsName LIKE @SearchName ) ");
            }
            strSql.Append(" ) AS NG GROUP BY NG.GoodsSn  ORDER BY SUMNumber DESC ");
            //strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                var infoList = connection.Query(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 0),
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.GoodsSn,
                            info.GoodsName,
                            info.SUMNumber,
                            info.GoodsPrice,
                        };
                    });
                count = (infoList != null) ? infoList.Count() : 0;
                return infoList;
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT CG.*,C.CpyName,G.* FROM M_CpyGoods AS CG  LEFT JOIN  M_Goods AS G ON  G.GoodsID= CG.GoodsID LEFT JOIN M_Cpy AS C ON C.CpySysID = CG.CpySysID ");
            strSql.Append(" WHERE 1=1  ");
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( C.CpyName LIKE @SearchName  OR G.GoodsSn LIKE @SearchName ) ");
            }
            strSql.Append(" ORDER BY CG.CpySysID DESC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(),
                    new
                    {
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.CpyGoodsID,
                            info.GoodsSn,
                            info.GoodsName,
                            info.CpyName,
                            info.CpySysID,
                            info.MarketPrice,
                            info.GoodsImg,
                            info.IsEnable,
                            info.GoodsID
                        };
                    });
            }
        }
        public int GetByPageGoodsRecomListCount(string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) FROM M_CpyGoods AS CG  LEFT JOIN  M_Goods AS G ON  G.GoodsID= CG.GoodsID LEFT JOIN M_Cpy AS C ON C.CpySysID = CG.CpySysID ");
            strSql.Append(" WHERE 1=1  ");
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( C.CpyName LIKE @SearchName  OR G.GoodsSn LIKE @SearchName ) ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(),
                   new
                   {
                       SearchName = "%" + searchName + "%"
                   }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
        public object GetByPageReBackOrderList(string byTimeStart, string byTimeEnd, string suppliersID, string payID, string byReBackType, string byAuthType, string searchName, string devSort, int startSize, int endSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT R.* ,O.SuppliersName,U.UserName,U.NickName,O.CpyName,O.CpySysID,O.Consignee,O.Mobile,O.ShippingName,O.PayName,O.HowOos,O.GoodsAmount,O.ShippingFee,");
            strSql.Append(" O.PayFee,O.MoneyPaid,O.OrderAmount,O.AddTime,O.ConfirmTime,O.PayTime,O.Logistical,O.ShippingTime,O.LogisticalNumber,O.TradeNo,O.ConsigneeCardNo,O.LogInfo,O.Logistical AS OLogistical,O.LogisticalNumber AS OLogisticalNumber, ");
            strSql.Append(" CASE WHEN ReBackType=10 THEN '用户申请退货' WHEN ReBackType=20 THEN '用户申请换货' WHEN ReBackType=30 THEN '用户申请退款' ELSE '未知' END NReBackType , ");
            strSql.Append(" CASE WHEN AuthType=10 THEN '未处理' WHEN AuthType=20 THEN '等待商品寄回' WHEN AuthType=30 THEN '商品检查无误售后通过' WHEN AuthType=40 THEN '退款中'  WHEN AuthType=50 THEN '退款完成'  WHEN AuthType=60 THEN '退款失败' WHEN AuthType=70 THEN '换货完成' WHEN AuthType=90 THEN '审核不通过' ELSE '未知' END NAuthType,");
            strSql.Append(" CONCAT( O.CountryName,O.ProvinceName,O.CityName,O.DistrictName,O.Address ) AS NAddress, ");
            strSql.Append(" CASE WHEN O.IsVerify =0 THEN '未校对' WHEN O.IsVerify =1 THEN '已校对' ELSE '未知' END NIsVerify, ");
            strSql.Append(" CASE WHEN O.OrderStatus=0 THEN '代确认' WHEN O.OrderStatus= 1 THEN '已确认' WHEN O.OrderStatus=2 THEN '已取消' WHEN O.OrderStatus=3 THEN '已无效' WHEN O.OrderStatus='4' THEN '已退货' WHEN O.OrderStatus=5 THEN '已发货' WHEN O.OrderStatus=6 THEN '处理中' WHEN O.OrderStatus=7 THEN '订单异常' WHEN O.OrderStatus=8 THEN '已完成' WHEN O.OrderStatus=9 THEN '已推送订单' WHEN O.OrderStatus=10 THEN '已退款' WHEN O.OrderStatus=11 THEN '已换货' ELSE '未知' END NOrderStatus, ");
            strSql.Append(" CASE WHEN O.ShippingStatus=0 THEN '未发货' WHEN O.ShippingStatus=1 THEN '已发货' WHEN O.ShippingStatus=2 THEN '已收货' WHEN O.ShippingStatus=3 THEN '未收到货' WHEN O.ShippingStatus=4 THEN '退货' ELSE '未知' END NShippingStatus,");
            strSql.Append(" CASE WHEN O.PayStatus =0 THEN '未付款' WHEN O.PayStatus=1 THEN '付款中' WHEN O.PayStatus=2 THEN '已付款' WHEN O.PayStatus=3 THEN '付款失败' WHEN O.PayStatus=4 THEN '付款超时' WHEN O.PayStatus=9 THEN '支付异常' ELSE '未知' END NPayStatus ");
            strSql.Append(" FROM M_ReBackOrder AS R ");
            strSql.Append(" LEFT JOIN M_OrderInfo AS O ON O.OrderID=R.OrderID ");
            strSql.Append(" LEFT JOIN M_Users AS U ON O.UserID=U.UserID ");
            strSql.Append(" WHERE 1=1 ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND O.AddTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND O.AddTime<=@TimeEnd ");
            }
            if (!string.IsNullOrEmpty(suppliersID))
            {
                strSql.Append(" AND O.SuppliersID=@SuppliersID ");
            }
            if (!string.IsNullOrEmpty(byReBackType))
            {
                strSql.Append(" AND R.ReBackType=@ReBackType ");
            }
            if (!string.IsNullOrEmpty(byAuthType))
            {
                if (byAuthType == "10")
                {
                    strSql.Append(" AND ( R.AuthType=10 OR R.AuthType=20 OR R.AuthType=30 OR R.AuthType=40 OR R.AuthType=60 ) ");
                }
                else
                {
                    strSql.Append(" AND ( R.AuthType=50 OR R.AuthType=70 OR R.AuthType=90) ");
                }
            }
            if (!string.IsNullOrEmpty(payID))
            {
                strSql.Append(" AND O.PayID=@PayID ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            strSql.Append(devSort);
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, DateTime.Now),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, DateTime.Now),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 1),
                        PayID = ToolHelper.ConventToInt32(payID, 1),
                        ReBackType = ToolHelper.ConventToInt32(byReBackType, 0),
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.UserName,
                            info.CpyName,
                            info.NickName,
                            info.NAddress,
                            info.NIsVerify,
                            info.NOrderStatus,
                            info.NShippingStatus,
                            info.NPayStatus,
                            info.OrderID,
                            info.OrderUnifySn,
                            info.OrderSn,
                            info.CpySysID,
                            info.Consignee,
                            info.Mobile,
                            info.ShippingName,
                            info.PayName,
                            info.HowOos,
                            info.GoodsAmount,
                            info.ShippingFee,
                            info.PayFee,
                            info.MoneyPaid,
                            info.OrderAmount,
                            info.CreatTime,
                            info.UpdateTime,
                            info.ConfirmTime,
                            PayTime = (info.PayTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.PayTime,
                            info.Logistical,
                            ShippingTime = (info.ShippingTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.ShippingTime,
                            info.LogisticalNumber,
                            info.ModifyPerson,
                            info.SuppliersName,
                            info.TradeNo,
                            info.ConsigneeCardNo,
                            info.Remark,
                            info.LogInfo,
                            info.NReBackType,
                            info.NAuthType,
                            info.AddTime,
                            info.ReBackDesc,
                            info.ReBackRemark,
                            info.OLogisticalNumber,
                            info.OLogistical,
                            info.ReConnection,
                            info.ReBackAddress,
                            info.ReBackID,
                            info.AuthType
                        };
                    });
            }
        }
        public int GetByPageReBackOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string payID, string byReBackType, string byAuthType, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) ");
            strSql.Append(" FROM M_ReBackOrder AS R ");
            strSql.Append(" LEFT JOIN M_OrderInfo AS O ON O.OrderID=R.OrderID ");
            strSql.Append(" LEFT JOIN M_Users AS U ON O.UserID=U.UserID ");
            strSql.Append(" WHERE 1=1 ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND O.AddTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND O.AddTime<=@TimeEnd ");
            }
            if (!string.IsNullOrEmpty(suppliersID))
            {
                strSql.Append(" AND O.SuppliersID=@SuppliersID ");
            }
            if (!string.IsNullOrEmpty(payID))
            {
                strSql.Append(" AND O.PayID=@PayID ");
            }
            if (!string.IsNullOrEmpty(byReBackType))
            {
                strSql.Append(" AND R.ReBackType=@ReBackType ");
            }
            if (!string.IsNullOrEmpty(byAuthType))
            {
                if (byAuthType == "10")
                {
                    strSql.Append(" AND ( R.AuthType=10 OR R.AuthType=20 OR R.AuthType=30 OR R.AuthType=40 OR R.AuthType=60 ) ");
                }
                else
                {
                    strSql.Append(" AND ( R.AuthType=50 OR R.AuthType=70 OR R.AuthType=90) ");
                }
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, DateTime.Now),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, DateTime.Now),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 1),
                        PayID = ToolHelper.ConventToInt32(payID, 1),
                        ReBackType = ToolHelper.ConventToInt32(byReBackType, 0),
                        SearchName = "%" + searchName + "%"
                    }, commandTimeout: commandTimeout).SingleOrDefault();
            }
        }

        /// <summary>
        /// 数据统计获取退货订单信息列表
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT R.* ,O.SuppliersName,U.UserName,U.NickName,O.CpyName,O.CpySysID,O.Consignee,O.Mobile,O.ShippingName,O.PayName,O.HowOos,O.GoodsAmount,O.ShippingFee, ");
            strSql.Append(" O.PayFee,O.MoneyPaid,O.OrderAmount,O.AddTime,O.ConfirmTime,O.PayTime,O.Logistical,O.ShippingTime,O.LogisticalNumber,O.TradeNo,O.ConsigneeCardNo,O.LogInfo,O.Logistical AS OLogistical,O.LogisticalNumber AS OLogisticalNumber, ");
            strSql.Append(" CASE WHEN ReBackType=10 THEN '退货' WHEN ReBackType=20 THEN '换货' WHEN ReBackType=30 THEN '退款' ELSE '未知' END NReBackType , ");
            strSql.Append(" CASE WHEN AuthType=10 THEN '未处理' WHEN AuthType=20 THEN '等待商品寄回' WHEN AuthType=30 THEN '申请通过' WHEN AuthType=90 THEN '审核不通过' ELSE '未知' END NAuthType,");
            strSql.Append(" CONCAT( O.CountryName,O.ProvinceName,O.CityName,O.DistrictName,O.Address ) AS NAddress, ");
            strSql.Append(" CASE WHEN O.IsVerify =0 THEN '未校对' WHEN O.IsVerify =1 THEN '已校对' ELSE '未知' END NIsVerify, ");
            strSql.Append(" CASE WHEN O.OrderStatus=0 THEN '代确认' WHEN O.OrderStatus= 1 THEN '已确认' WHEN O.OrderStatus=2 THEN '已取消' WHEN O.OrderStatus=3 THEN '已无效' WHEN O.OrderStatus='4' THEN '已退货' WHEN O.OrderStatus=5 THEN '已发货' WHEN O.OrderStatus=6 THEN '处理中' WHEN O.OrderStatus=7 THEN '订单异常' WHEN O.OrderStatus=8 THEN '已完成' WHEN O.OrderStatus=9 THEN '已推送订单' WHEN O.OrderStatus=10 THEN '已退款' WHEN O.OrderStatus=11 THEN '已换货' ELSE '未知' END NOrderStatus, ");
            strSql.Append(" CASE WHEN O.ShippingStatus=0 THEN '未发货' WHEN O.ShippingStatus=1 THEN '已发货' WHEN O.ShippingStatus=2 THEN '已收货' WHEN O.ShippingStatus=3 THEN '未收到货' WHEN O.ShippingStatus=4 THEN '退货' ELSE '未知' END NShippingStatus,");
            strSql.Append(" CASE WHEN O.PayStatus =0 THEN '未付款' WHEN O.PayStatus=1 THEN '付款中' WHEN O.PayStatus=2 THEN '已付款' WHEN O.PayStatus=3 THEN '付款失败' WHEN O.PayStatus=4 THEN '付款超时' WHEN O.PayStatus=9 THEN '支付异常' ELSE '未知' END NPayStatus ");
            strSql.Append(" FROM M_ReBackOrder AS R ");
            strSql.Append(" LEFT JOIN M_OrderInfo AS O ON O.OrderID=R.OrderID ");
            strSql.Append(" LEFT JOIN M_Users AS U ON O.UserID=U.UserID ");
            strSql.Append(" WHERE 1=1 AND O.AddTime>=@TimeStart  AND O.AddTime<=@TimeEnd  AND O.SuppliersID=@SuppliersID ");
            if (!string.IsNullOrEmpty(byOrderStatus))
            {
                if (byOrderStatus == "10")
                {
                    strSql.Append(" AND R.AuthType=30 ");
                }
                else if (byOrderStatus == "20")
                {
                    strSql.Append(" AND R.AuthType=90 ");
                }
                else if (byOrderStatus == "30")
                {
                    strSql.Append(" AND ( R.AuthType=10 OR R.AuthType=20 ) ");
                }
            }
            if (!string.IsNullOrEmpty(byReBackType))
            {
                strSql.Append(" AND R.ReBackType=@ReBackType ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            strSql.Append(devSort);
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 0),
                        ReBackType = ToolHelper.ConventToInt32(byReBackType, 10),
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.UserName,
                            info.CpyName,
                            info.NickName,
                            info.NAddress,
                            info.NIsVerify,
                            info.NOrderStatus,
                            info.NShippingStatus,
                            info.NPayStatus,
                            info.OrderID,
                            info.OrderUnifySn,
                            info.OrderSn,
                            info.CpySysID,
                            info.Consignee,
                            info.Mobile,
                            info.ShippingName,
                            info.PayName,
                            info.HowOos,
                            info.GoodsAmount,
                            info.ShippingFee,
                            info.PayFee,
                            info.MoneyPaid,
                            info.OrderAmount,
                            info.CreatTime,
                            info.UpdateTime,
                            info.ConfirmTime,
                            PayTime = (info.PayTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.PayTime,
                            info.Logistical,
                            ShippingTime = (info.ShippingTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.ShippingTime,
                            info.LogisticalNumber,
                            info.ModifyPerson,
                            info.SuppliersName,
                            info.TradeNo,
                            info.ConsigneeCardNo,
                            info.Remark,
                            info.LogInfo,
                            info.NReBackType,
                            info.NAuthType,
                            info.AddTime,
                            info.ReBackDesc,
                            info.ReBackRemark,
                            info.OLogisticalNumber,
                            info.OLogistical,
                            info.ReConnection,
                            info.ReBackAddress,
                            info.ReBackID
                        };
                    });
            }
        }
        public int GetByPageReBackThirdOrderListCount(string byTimeStart, string byTimeEnd, string suppliersID, string byOrderStatus, string byReBackType, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) ");
            strSql.Append(" FROM M_ReBackOrder AS R ");
            strSql.Append(" LEFT JOIN M_OrderInfo AS O ON O.OrderID=R.OrderID ");
            strSql.Append(" LEFT JOIN M_Users AS U ON O.UserID=U.UserID ");
            strSql.Append(" WHERE 1=1 AND O.SuppliersID=@SuppliersID ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND O.AddTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND O.AddTime<=@TimeEnd ");
            }
            if (!string.IsNullOrEmpty(byOrderStatus))
            {
                if (byOrderStatus == "10")
                {
                    strSql.Append(" AND R.AuthType=30 ");
                }
                else if (byOrderStatus == "20")
                {
                    strSql.Append(" AND R.AuthType=90 ");
                }
                else if (byOrderStatus == "30")
                {
                    strSql.Append(" AND ( R.AuthType=10 OR R.AuthType=20 ) ");
                }
            }
            if (!string.IsNullOrEmpty(byReBackType))
            {
                strSql.Append(" AND R.ReBackType=@ReBackType ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND O.OrderSn Like @SearchName ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, DateTime.Now),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, DateTime.Now),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 0),
                        ReBackType = ToolHelper.ConventToInt32(byReBackType, 10),
                        SearchName = "%" + searchName + "%"
                    }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT NG.GoodsSn,NG.GoodsName,SUM( NG.RebackNumber) AS SUMNumber ,NG.GoodsPrice FROM (  ");
            strSql.Append(" SELECT OG.GoodsSn,OG.GoodsName,OG.RebackNumber,OG.GoodsPrice FROM M_ReBackOrder AS O  ");
            strSql.Append(" LEFT JOIN M_ReBackGoods AS OG  ON OG.ReBackID = O.ReBackID ");
            strSql.Append(" LEFT JOIN M_OrderInfo AS OI ON O.OrderID =OI.OrderID ");
            strSql.Append(" WHERE 1=1 AND OI.ShippingTime>=@TimeStart AND OI.ShippingTime<=@TimeEnd AND OI.SuppliersID = @SuppliersID ");
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (orderStatus == "10")
                {
                    strSql.Append(" AND O.AuthType=30 ");
                }
                else if (orderStatus == "20")
                {
                    strSql.Append(" AND O.AuthType=90 ");
                }
                else if (orderStatus == "30")
                {
                    strSql.Append(" AND ( O.AuthType=10 OR O.AuthType=20 ) ");
                }
            }
            if (!string.IsNullOrEmpty(reBackType))
            {
                strSql.Append(" AND O.ReBackType=@ReBackType ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( OG.GoodsSn LIKE @SearchName OR OG.GoodsName LIKE @SearchName ) ");
            }
            strSql.Append(" ) AS NG GROUP BY NG.GoodsSn ORDER BY SUMNumber DESC ");
            strSql.Append(" LIMIT @StartSize,@EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 0),
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.GoodsSn,
                            info.GoodsName,
                            info.SUMNumber,
                            info.GoodsPrice,
                        };
                    });
            }
        }
        public int GetByPageReBackDataThirdGoodsListCount(string byTimeStart, string byTimeEnd, string suppliersID, string orderStatus, string reBackType, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*) FROM ( ");
            strSql.Append(" SELECT NG.GoodsSn,NG.GoodsName,SUM( NG.RebackNumber) AS SUMNumber ,NG.GoodsPrice FROM (  ");
            strSql.Append(" SELECT OG.GoodsSn,OG.GoodsName,OG.RebackNumber,OG.GoodsPrice FROM M_ReBackOrder AS O  ");
            strSql.Append(" LEFT JOIN M_ReBackGoods AS OG  ON OG.ReBackID = O.ReBackID ");
            strSql.Append(" LEFT JOIN M_OrderInfo AS OI ON O.OrderID =OI.OrderID ");
            strSql.Append(" WHERE 1=1 AND OI.ShippingTime>=@TimeStart AND OI.ShippingTime<=@TimeEnd AND OI.SuppliersID = @SuppliersID ");
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (orderStatus == "10")
                {
                    strSql.Append(" AND O.AuthType=30 ");
                }
                else if (orderStatus == "20")
                {
                    strSql.Append(" AND O.AuthType=90 ");
                }
                else if (orderStatus == "30")
                {
                    strSql.Append(" AND ( O.AuthType=10 OR O.AuthType=20 ) ");
                }
            }
            if (!string.IsNullOrEmpty(reBackType))
            {
                strSql.Append(" AND O.ReBackType=@ReBackType ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( OG.GoodsSn LIKE @SearchName OR OG.GoodsName LIKE @SearchName ) ");
            }
            strSql.Append(" ) AS NG GROUP BY NG.GoodsSn ) AS NNG ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        SuppliersID = ToolHelper.ConventToInt32(suppliersID, 0),
                        SearchName = "%" + searchName + "%"
                    }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            moneyAmount = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.*,  ");
            strSql.Append(" CASE WHEN T.ComfirmStatus=10 THEN '流水未同步' WHEN T.ComfirmStatus=20 THEN '支付已确认' WHEN T.ComfirmStatus=30 THEN '流水支付失败' WHEN T.ComfirmStatus=90 THEN '流水异常' ELSE '未知' END NComfirmStatus ");
            strSql.Append(" FROM M_OrderTrade AS T ");
            strSql.Append(" WHERE 1=1  ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND T.PayTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND T.PayTime<=@TimeEnd");
            }
            if (!string.IsNullOrEmpty(byComfirmType))
            {
                strSql.Append(" AND T.ComfirmStatus=@ComfirmStatus ");
            }
            if (!string.IsNullOrEmpty(byPayID))
            {
                strSql.Append(" AND T.PayID =@PayID ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( T.TradeNo LIKE @SearchName OR CpyName LIKE @SearchName )");
            }
            strSql.Append(" ORDER BY PayTime DESC ");
            strSql.Append(" LIMIT @StartSize,@EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                IEnumerable<dynamic> resInfo = connection.Query(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        ComfirmStatus = ToolHelper.ConventToInt32(byComfirmType, 10),
                        PayID = ToolHelper.ConventToInt32(byPayID, 0),
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.TradeSysID,
                            info.PayID,
                            info.PayName,
                            info.OrderUnifySn,
                            info.OrderPayNo,
                            info.TradeNo,
                            info.PayTime,
                            info.PayMoney,
                            info.Remark,
                            info.ModifyPerson,
                            info.NComfirmStatus,
                            ComfirmTime = (info.ComfirmTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.ComfirmTime,
                            info.CpyName,
                            info.CpySysID
                        };
                    });
                foreach (var item in resInfo)
                {
                    moneyAmount += item.PayMoney;
                }
                return resInfo;
            }
        }
        public int GetByPageUnusualTradeListCount(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*)  ");
            strSql.Append(" FROM M_OrderTrade AS T ");
            strSql.Append(" WHERE 1=1  ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND T.PayTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND T.PayTime<=@TimeEnd");
            }
            if (!string.IsNullOrEmpty(byComfirmType))
            {
                strSql.Append(" AND T.ComfirmStatus=@ComfirmStatus ");
            }
            if (!string.IsNullOrEmpty(byPayID))
            {
                strSql.Append(" AND T.PayID =@PayID ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND T.TradeNo LIKE @SearchName ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        ComfirmStatus = ToolHelper.ConventToInt32(byComfirmType, 10),
                        PayID = ToolHelper.ConventToInt32(byPayID, 0),
                        SearchName = "%" + searchName + "%"
                    }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            moneyAmount = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.*,  ");
            strSql.Append(" CASE WHEN T.ComfirmStatus=10 THEN '流水未同步' WHEN T.ComfirmStatus=20 THEN '支付已确认' WHEN T.ComfirmStatus=30 THEN '流水支付失败' WHEN T.ComfirmStatus=90 THEN '流水异常' ELSE '未知' END NComfirmStatus ");
            strSql.Append(" FROM M_ReBackTrade AS T ");
            strSql.Append(" WHERE 1=1  ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND T.ReBackTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND T.ReBackTime<=@TimeEnd");
            }
            if (!string.IsNullOrEmpty(byComfirmType))
            {
                strSql.Append(" AND T.ComfirmStatus=@ComfirmStatus ");
            }
            if (!string.IsNullOrEmpty(byPayID))
            {
                strSql.Append(" AND T.PayID =@PayID ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( T.TradeNo LIKE @SearchName  OR T.ReBackTradeNo=@SearchName OR T.CpyName LIKE  @SearchName ) ");
            }
            strSql.Append(" ORDER BY ReBackTime DESC ");
            strSql.Append(" LIMIT @StartSize,@EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                IEnumerable<dynamic> resInfo = connection.Query(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        ComfirmStatus = ToolHelper.ConventToInt32(byComfirmType, 10),
                        PayID = ToolHelper.ConventToInt32(byPayID, 0),
                        SearchName = "%" + searchName + "%",
                        StartSize = startSize,
                        EndSize = endSize
                    }, commandTimeout: commandTimeout).Select(info =>
                    {
                        return new
                        {
                            info.ReBackTradeSysID,
                            info.ReBackTradeNo,
                            info.ReBackID,
                            info.TradeSysID,
                            info.TradeNo,
                            info.PayID,
                            info.PayName,
                            info.OrderID,
                            info.ReBackTime,
                            info.ReBackMoney,
                            info.NComfirmStatus,
                            ComfirmTime = (info.ComfirmTime == Convert.ToDateTime("1970/1/1 00:00:00")) ? null : info.ComfirmTime,
                            info.CpyName,
                            info.CpySysID
                        };
                    });
                foreach (var item in resInfo)
                {
                    moneyAmount += item.ReBackMoney;
                }
                return resInfo;
            }
        }
        public int GetByPageUnusualRebackTradeListCount(string byTimeStart, string byTimeEnd, string byComfirmType, string byPayID, string searchName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*)  ");
            strSql.Append(" FROM M_ReBackTrade AS T ");
            strSql.Append(" WHERE 1=1 ");
            if (!string.IsNullOrEmpty(byTimeStart))
            {
                strSql.Append(" AND T.ReBackTime>=@TimeStart ");
            }
            if (!string.IsNullOrEmpty(byTimeEnd))
            {
                strSql.Append(" AND T.ReBackTime<=@TimeEnd");
            }
            if (!string.IsNullOrEmpty(byComfirmType))
            {
                strSql.Append(" AND T.ComfirmStatus=@ComfirmStatus ");
            }
            if (!string.IsNullOrEmpty(byPayID))
            {
                strSql.Append(" AND T.PayID =@PayID ");
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( T.TradeNo LIKE @SearchName  OR T.ReBackTradeNo=@SearchName ) ");
            }
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<int>(strSql.ToString(),
                    new
                    {
                        TimeStart = ToolHelper.ConventToDateTime(byTimeStart, ToolHelper.MonthFirstDay()),
                        TimeEnd = ToolHelper.ConventToDateTime(byTimeEnd, ToolHelper.MonthLastDay()),
                        ComfirmStatus = ToolHelper.ConventToInt32(byComfirmType, 10),
                        PayID = ToolHelper.ConventToInt32(byPayID, 0),
                        SearchName = "%" + searchName + "%"
                    }, commandTimeout: commandTimeout).SingleOrDefault();
            }
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
            DateTime nowTime = DateTime.Now;
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        //创建文件表数据_1
                        if (!string.IsNullOrEmpty(fileUrl_1))
                        {
                            strSql.Append(" INSERT INTO M_FileManage ( FileType, FileName,FilePath,FileLabel,CreatTime) ");
                            strSql.Append(" VALUES ( 0, @FileName,@FilePath,'',@CreatTime ) ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                FileName = Path.GetFileNameWithoutExtension(fileUrl_1),
                                FilePath = fileUrl_1,
                                CreatTime = nowTime
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        //创建文件表数据_2
                        if (!string.IsNullOrEmpty(fileUrl_2))
                        {
                            strSql.Clear();
                            strSql.Append(" INSERT INTO M_FileManage (FileType, FileName,FilePath,FileLabel,CreatTime) ");
                            strSql.Append(" VALUES ( 0, @FileName,@FilePath,'',@CreatTime ) ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                FileName = Path.GetFileNameWithoutExtension(fileUrl_2),
                                FilePath = fileUrl_2,
                                CreatTime = nowTime
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        //更新商品图片路径
                        if (type == "goodsMainImg")
                        {
                            strSql.Clear();
                            strSql.Append(" UPDATE M_Goods SET GoodsImg = @GoodsImg ,GoodsThumb = @GoodsThumb WHERE GoodsID = @_GoodsID ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                GoodsImg = fileUrl_1,
                                GoodsThumb = fileUrl_2,
                                _GoodsID = sysID
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                            //将主题插入商品相册表
                            strSql.Clear();
                            strSql.Append(" INSERT INTO M_GoodsGallery (GoodsID, ImgUrl,ThumbUrl,ImgOriginal,ImgDesc) ");
                            strSql.Append(" VALUES ( @GoodsID, @ImgUrl,@ThumbUrl,@ImgUrl,'' ) ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                ImgUrl = fileUrl_1,
                                ThumbUrl = fileUrl_2,
                                GoodsID = sysID
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        else if (type == "categoryImg")
                        {
                            strSql.Clear();
                            strSql.Append(" UPDATE M_Category SET ShowImage = @ShowImage  WHERE CateID = @_CateID ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                ShowImage = fileUrl_1,
                                _CateID = sysID
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        else if (type == "rollImg")
                        {
                            strSql.Clear();
                            strSql.Append(" UPDATE M_Roll SET Picture = @Picture  WHERE RollSysID = @_RollSysID ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                Picture = fileUrl_1,
                                _RollSysID = sysID
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        else if (type == "galleryImg")
                        {
                            strSql.Clear();
                            strSql.Append(" INSERT INTO M_GoodsGallery (GoodsID, ImgUrl,ThumbUrl,ImgOriginal,ImgDesc) ");
                            strSql.Append(" VALUES ( @GoodsID, @ImgUrl,@ThumbUrl,@ImgUrl,'' ) ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                ImgUrl = fileUrl_1,
                                ThumbUrl = fileUrl_2,
                                GoodsID = sysID
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
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
            DateTime nowTime = DateTime.Now;
            string sysID = Guid.NewGuid().ToString();
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" INSERT INTO M_Goods (GoodsID, CateID,BrandID,SuppliersID,GoodsSn,GoodsName,ClickCount,GoodsNumber,MarketPrice,ShopPrice,WarnNumber,KeyWords,GoodsBrief,GoodsDesc,GoodsThumb,GoodsImg,OriginalImg,IsReal,IsEnable,AddTime,OrderBy,IsDelete,IsBest,IsNew,IsHot,IsPromote,LastUpdate,SellerNote,SaleNumber,Weight,Version,FareSysID,QuotaNumber) ");
                        strSql.Append(" VALUES ( @GoodsID, @CateID,@BrandID,@SuppliersID,@GoodsSn,@GoodsName,@ClickCount,@GoodsNumber,@MarketPrice,@ShopPrice,@WarnNumber,@KeyWords,@GoodsBrief,@GoodsDesc,@GoodsThumb,@GoodsImg,@GoodsImg,@IsReal,@IsEnable,@AddTime,@OrderBy,@IsDelete,@IsBest,@IsNew,@IsHot,@IsPromote,@LastUpdate,@SellerNote,@SaleNumber,@Weight,@Version,@FareSysID,@QuotaNumber) ");
                        if (connection.Execute(strSql.ToString(), new
                        {
                            GoodsID = sysID,
                            CateID = CateID,
                            BrandID = BrandID,
                            SuppliersID = SuppliersID,
                            GoodsSn = GoodsSn,
                            GoodsName = GoodsName,
                            ClickCount = 0,
                            GoodsNumber = GoodsNumber,
                            MarketPrice = MarketPrice,
                            ShopPrice = ShopPrice,
                            WarnNumber = 10,
                            KeyWords = GoodsName,
                            GoodsBrief = GoodsBrief,
                            GoodsDesc = "",
                            GoodsThumb = GoodsThumb,
                            GoodsImg = GoodsImg,
                            OriginalImg = OriginalImg,
                            IsReal = IsReal,
                            IsEnable = 0,
                            AddTime = nowTime,
                            OrderBy = 50,
                            IsDelete = 0,
                            IsBest = 0,
                            IsNew = IsNew,
                            IsHot = IsHot,
                            IsPromote = 0,
                            LastUpdate = nowTime,
                            SellerNote = SellerNote,
                            SaleNumber = 0,
                            Weight = Weight,
                            Version = 0,
                            FareSysID = FareTemp,
                            IsUpdateBySupplier = 1,
                            ModifyPerson = adminName,
                            QuotaNumber = 0
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        if (!string.IsNullOrEmpty(GoodsImg) && !string.IsNullOrEmpty(GoodsThumb))
                        {
                            strSql.Clear();
                            strSql.Append(" INSERT INTO M_GoodsGallery (GoodsID, ImgUrl,ThumbUrl,ImgOriginal,ImgDesc) ");
                            strSql.Append(" VALUES ( @GoodsID, @ImgUrl,@ThumbUrl,@ImgUrl,'' ) ");
                            if (connection.Execute(strSql.ToString(), new { GoodsID = sysID, ImgUrl = GoodsImg, ThumbUrl = GoodsThumb }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 分配商品给指定的公司-集团推荐商品
        /// </summary>
        /// <param name="goodsID"></param>
        /// <param name="cpyInfoList"></param>
        /// <returns></returns>
        public bool AssignGoodsToCpy(string goodsID, string[] cpyInfoList)
        {
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" DELETE FROM M_CpyGoods WHERE GoodsID =@_GoodsID  ");
                        connection.Execute(strSql.ToString(), new { _GoodsID = goodsID }, transaction: transaction, commandTimeout: commandTimeout);
                        //删除之前对该商品所有的分配，执行的影响行数不确定，所以不做判断
                        foreach (var item in cpyInfoList)
                        {
                            strSql.Clear();
                            strSql.Append(" INSERT INTO M_CpyGoods (CpySysID, GoodsID ) ");
                            strSql.Append(" VALUES ( @CpySysID, @GoodsID ) ");
                            if (connection.Execute(strSql.ToString(), new { GoodsID = goodsID, CpySysID = item }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

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
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        string guid = Guid.NewGuid().ToString();
                        DateTime nowTime = DateTime.Now;
                        //创建运费模板表
                        strSql.Append(" INSERT INTO M_FareTemplate (FareSysID,FareName,FareAddress,FareTime,FareType,FareDeliery,UpdateTime,FareDelierySysID) ");
                        strSql.Append(" VALUES( @FareSysID,@FareName,@FareAddress,@FareTime,@FareType,@FareDeliery,@UpdateTime,@FareDelierySysID ) ");
                        if (connection.Execute(strSql.ToString(), new
                        {
                            FareSysID = guid,
                            FareName = fareName,
                            FareAddress = fareAddress,
                            FareTime = fareTime,
                            FareType = fareType,
                            FareDeliery = 0,
                            UpdateTime = nowTime,
                            FareDelierySysID = 0
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        //创建运费模板对应的配送信息表-默认配送信息表
                        strSql.Clear();
                        strSql.Append(" INSERT INTO M_FareCarry (FareSysID,FareType,CarryAddressList,FirstCount,FirstWeight,FirstVolume,FirstMoney,ContinueCount,ContinueWeight,ContinueVolume,ContinueMoney,CarryWay,IsDefault) ");
                        strSql.Append(" VALUES(@FareSysID,@FareType,@CarryAddressList,@FirstCount,@FirstWeight,@FirstVolume,@FirstMoney,@ContinueCount,@ContinueWeight,@ContinueVolume,@ContinueMoney,@CarryWay,@IsDefault) ");
                        if (connection.Execute(strSql.ToString(), new
                        {
                            FareSysID = guid,
                            FareType = fareType,
                            CarryAddressList = "0",
                            FirstCount = defaultFareTemp.DefaultCount,
                            FirstWeight = defaultFareTemp.DefaultCount,
                            FirstVolume = defaultFareTemp.DefaultCount,
                            FirstMoney = defaultFareTemp.DefaultMoney,
                            ContinueCount = defaultFareTemp.DefaultAddCount,
                            ContinueWeight = defaultFareTemp.DefaultAddCount,
                            ContinueVolume = defaultFareTemp.DefaultAddCount,
                            ContinueMoney = defaultFareTemp.DefaultAddFee,
                            CarryWay = "快递",
                            IsDefault = 1
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        //创建指定地区配送信息表
                        if (fareCarryTempList != null && fareCarryTempList.Length > 0)
                        {
                            foreach (var item in fareCarryTempList)
                            {
                                strSql.Clear();
                                strSql.Append(" INSERT INTO M_FareCarry (FareSysID,FareType,CarryAddressList,FirstCount,FirstWeight,FirstVolume,FirstMoney,ContinueCount,ContinueWeight,ContinueVolume,ContinueMoney,CarryWay,IsDefault) ");
                                strSql.Append(" VALUES(@FareSysID,@FareType,@CarryAddressList,@FirstCount,@FirstWeight,@FirstVolume,@FirstMoney,@ContinueCount,@ContinueWeight,@ContinueVolume,@ContinueMoney,@CarryWay,@IsDefault) ");
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    FareSysID = guid,
                                    FareType = fareType,
                                    CarryAddressList = item.Address,
                                    FirstCount = item.Count,
                                    FirstWeight = item.Count,
                                    FirstVolume = item.Count,
                                    FirstMoney = item.Money,
                                    ContinueCount = item.AddCount,
                                    ContinueWeight = item.AddCount,
                                    ContinueVolume = item.AddCount,
                                    ContinueMoney = item.AddFee,
                                    CarryWay = "快递",
                                    IsDefault = 0
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 删除运费模板--同时删除相对应得配送
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public bool DeleteFareTemplate(string sysID)
        {
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        //删除运费模板
                        strSql.Append(" DELETE FROM M_FareTemplate WHERE FareSysID =@_FareSysID  ");
                        if (connection.Execute(strSql.ToString(), new { _FareSysID = sysID }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        //删除运费配送
                        strSql.Clear();
                        strSql.Append(" DELETE FROM M_FareCarry WHERE FareSysID =@_FareSysID  ");
                        if (connection.Execute(strSql.ToString(), new { _FareSysID = sysID }, transaction: transaction, commandTimeout: commandTimeout) <= 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 更新模块分配
        /// </summary>
        /// <param name="adminID">用户ID</param>
        /// <param name="modaleList"></param>
        /// <returns></returns>
        public bool AssignModale(string adminID, string[] modaleList)
        {
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" DELETE FROM M_AdminAction WHERE AdminSysID =@_AdminSysID  ");
                        connection.Execute(strSql.ToString(), new { _AdminSysID = adminID }, transaction: transaction, commandTimeout: commandTimeout);
                        //删除之前对该商品所有的分配，执行的影响行数不确定，所以不做判断
                        if (modaleList != null && modaleList.Length > 0)
                        {
                            foreach (var item in modaleList)
                            {
                                strSql.Clear();
                                strSql.Append(" INSERT INTO M_AdminAction (ModalSysID, AdminSysID,CreateTime ) ");
                                strSql.Append(" VALUES ( @ModalSysID, @AdminSysID,@CreateTime ) ");
                                if (connection.Execute(strSql.ToString(), new { AdminSysID = adminID, CreateTime = DateTime.Now, ModalSysID = item }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
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
            outEsg = string.Empty;
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" SELECT * FROM M_OrderInfo WHERE OrderID=@_OrderID  ");
                        List<Model.M_OrderInfo> orderInfo = connection.Query<Model.M_OrderInfo>(strSql.ToString(), new { _OrderID = orderSysID }, transaction: transaction, commandTimeout: commandTimeout).ToList();
                        strSql.Clear();
                        strSql.Append(" SELECT * FROM M_OrderGoods WHERE OrderID=@_OrderID ");
                        List<Model.M_OrderGoods> orderGoodsInfo = connection.Query<Model.M_OrderGoods>(strSql.ToString(), new { _OrderID = orderSysID }, transaction: transaction, commandTimeout: commandTimeout).ToList();
                        Model.M_OrderGoods goodsInfo = new Model.M_OrderGoods();
                        if (orderInfo != null && orderInfo.Count == 1 && orderGoodsInfo != null && orderGoodsInfo.Count > 0)
                        {
                            DateTime nowTime = DateTime.Now;
                            string reBackID = Guid.NewGuid().ToString();
                            //生成退货记录表信息
                            strSql.Clear();
                            strSql.Append("INSERT INTO M_ReBackOrder ( ReBackID,OrderID,OrderSn,OrderUnifySn,ReBackType,AuthType,ReConnection,ReBackDesc,CreatTime,UpdateTime,ModifyPerson,UserID,OpenID,SuppliersID,SuppliersName)  ");
                            strSql.Append(" VALUES ( @ReBackID,@OrderID,@OrderSn,@OrderUnifySn,@ReBackType,@AuthType,@ReConnection,@ReBackDesc,@CreatTime,@UpdateTime,@ModifyPerson,@UserID,@OpenID,@SuppliersID ,@SuppliersName) ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                ReBackID = reBackID,
                                OrderID = orderInfo[0].OrderID,
                                OrderSn = orderInfo[0].OrderSn,
                                OrderUnifySn = orderInfo[0].OrderUnifySn,
                                ReBackType = type,
                                AuthType = 10,
                                ReConnection = phone,
                                ReBackDesc = remark,
                                CreatTime = nowTime,
                                UpdateTime = nowTime,
                                ModifyPerson = rebackName,
                                UserID = orderInfo[0].UserID,
                                OpenID = orderInfo[0].OpenID,
                                SuppliersID = orderInfo[0].SuppliersID,
                                SuppliersName = orderInfo[0].SuppliersName
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }

                            if (backInfoList != null && backInfoList.Count > 0)
                            {
                                foreach (var item in backInfoList)
                                {
                                    if (!string.IsNullOrEmpty(item.GoodsID) && (item.GoodsNum != 0))
                                    {
                                        goodsInfo = orderGoodsInfo.Find(info => info.GoodsID == item.GoodsID);
                                        if (goodsInfo.BuyNumber >= item.GoodsNum)
                                        {
                                            strSql.Clear();
                                            strSql.Append(" INSERT INTO M_ReBackGoods (ReBackID,OrderUnifySn,OrderSn,OrderID,GoodsID,GoodsSn,GoodsName,BuyNumber,RebackNumber,MarketPrice,GoodsPrice,AddTime ) ");
                                            strSql.Append(" VALUES (@ReBackID, @OrderUnifySn,@OrderSn,@OrderID,@GoodsID,@GoodsSn,@GoodsName,@BuyNumber,@RebackNumber,@MarketPrice,@GoodsPrice,@AddTime) ");
                                            if (connection.Execute(strSql.ToString(),
                                                new
                                                {
                                                    ReBackID = reBackID,
                                                    OrderUnifySn = orderInfo[0].OrderID,
                                                    OrderSn = orderInfo[0].OrderSn,
                                                    OrderID = orderInfo[0].OrderID,
                                                    GoodsID = item.GoodsID,
                                                    GoodsSn = goodsInfo.GoodsSn,
                                                    GoodsName = goodsInfo.GoodsName,
                                                    BuyNumber = goodsInfo.BuyNumber,
                                                    RebackNumber = item.GoodsNum,
                                                    MarketPrice = goodsInfo.MarketPrice,
                                                    GoodsPrice = goodsInfo.GoodsPrice,
                                                    AddTime = nowTime
                                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                            {
                                                transaction.Rollback();
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            outEsg = "退回的商品数量不能大于购买的商品数量";
                                            transaction.Rollback();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        outEsg = "勾选的商品必须填写需要退回的商品数量";
                                        transaction.Rollback();
                                        return false;
                                    }
                                }
                            }

                            //最后修改订单状态
                            strSql.Clear();
                            strSql.Append(" UPDATE M_OrderInfo SET OrderStatus =@OrderStatus,ShippingStatus=@ShippingStatus,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo) WHERE OrderID=@_OrderID AND Version=@_Version AND ( OrderStatus=1 OR  OrderStatus=5 OR OrderStatus=8 OR OrderStatus= 7 ) ");//版本号必须一致
                            if (connection.Execute(strSql.ToString(), new
                            {
                                _OrderID = orderInfo[0].OrderID,
                                _Version = orderInfo[0].Version,
                                ShippingStatus = 4,//物流状态改为退货中
                                OrderStatus = 6, //订单状态改为处理中
                                LogInfo = nowTime + "用户提交退换货申请"
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                outEsg = "退货申请只允许提交一次";
                                transaction.Rollback();
                                return false;
                            }
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
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
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DateTime nowTime = DateTime.Now;
                        StringBuilder strSql = new StringBuilder();
                        if (authType == 90)
                        {//如果拒绝需要把订单的状态也给改了
                            strSql.Append(" UPDATE M_OrderInfo SET OrderStatus =@OrderStatus,ShippingStatus=@ShippingStatus,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo) WHERE OrderID=@_OrderID AND OrderStatus=6 AND ShippingStatus=4 ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                _OrderID = orderID,
                                ShippingStatus = 5,//物流状态改为已完成
                                OrderStatus = 8, //订单状态改为已完成
                                LogInfo = nowTime + "驳回用户的退换货申请"
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        strSql.Clear();//下面这个是一定要改的
                        strSql.Append(" UPDATE M_ReBackOrder SET AuthType=@AuthType,ReBackRemark=@ReBackRemark,ReBackAddress=@ReBackAddress WHERE ReBackID=@_ReBackID  ");
                        if (connection.Execute(strSql.ToString(), new
                        {
                            _ReBackID = reBackID,
                            AuthType = authType,
                            ReBackRemark = remark,
                            ReBackAddress = address
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
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
            resMsg = string.Empty;
            DateTime nowTime = DateTime.Now;
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        string tradeSysID = Guid.NewGuid().ToString();
                        decimal orderAmountSum = 0;
                        strSql.Append(" SELECT OrderAmount,OrderID FROM  M_OrderInfo WHERE OrderUnifySn=@_OrderUnifySn ");
                        IEnumerable<dynamic> orderInfoList = connection.Query(strSql.ToString(), new { _OrderUnifySn = orderUnifySn });
                        strSql.Clear();
                        strSql.Append(" SELECT Count(*)  FROM M_OrderTrade WHERE TradeNo=@_TradeNo");
                        int tradeInfo = connection.Query<int>(strSql.ToString(), new { _TradeNo = tradeNo }).FirstOrDefault();
                        if (tradeInfo == 0)
                        {
                            //先判断是否存在对应的流水号，不存在，才能进行创建相应的流水号
                            if (orderInfoList != null && orderInfoList.Count() > 0)
                            {
                                foreach (var item in orderInfoList)
                                {
                                    strSql.Clear();
                                    strSql.Append(" UPDATE M_OrderInfo SET PayStatus=1 ,PayID=@PayID,PayName=@PayName,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo),");
                                    strSql.Append(" PayTime=@PayTime,TradeSysID=@TradeSysID,TradeNo=@TradeNo,CpySysID=@CpySysID,CpyName=@CpyName,MoneyPaid=@MoneyPaid WHERE OrderID =@_OrderID AND PayStatus=9 AND TradeSysID='' AND TradeNo='' ");
                                    if (connection.Execute(strSql.ToString(), new
                                    {
                                        PayID = paymentInfo.PayID,
                                        PayName = paymentInfo.PayName,
                                        PayTime = tradeTime,
                                        TradeSysID = tradeSysID,
                                        TradeNo = tradeNo,
                                        CpySysID = cpySysID,
                                        CpyName = cpyName,
                                        LogInfo = nowTime + "后台流水补单成功",
                                        MoneyPaid = item.OrderAmount,
                                        _OrderID = item.OrderID
                                    }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                    {
                                        transaction.Rollback();
                                        return false;
                                    }
                                    orderAmountSum += item.OrderAmount;
                                }
                                if (orderAmountSum != orderAmount)//资金对账不相等的情况，直接返回false
                                {
                                    resMsg = "XF系统返回的流水扣款资金与订单总金额对账不相等";
                                    transaction.Rollback();
                                    return false;
                                }
                                //创建资金流水表
                                strSql.Clear();
                                strSql.Append(" INSERT INTO M_OrderTrade(TradeSysID, PayID, PayName, OrderUnifySn, OrderPayNo, TradeNo, PayTime, PayMoney,ComfirmTime, CreateTime,CpySysID,CpyName)  ");
                                strSql.Append(" VALUES ( @TradeSysID, @PayID, @PayName, @OrderUnifySn, @OrderPayNo, @TradeNo,@PayTime, @PayMoney,@ComfirmTime, @CreateTime,@CpySysID,@CpyName ) ");
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    TradeSysID = tradeSysID,
                                    PayID = paymentInfo.PayID,
                                    PayName = paymentInfo.PayName,
                                    OrderUnifySn = orderUnifySn,
                                    OrderPayNo = string.IsNullOrEmpty(orderSn) ? orderUnifySn : orderSn,
                                    TradeNo = tradeNo,
                                    PayTime = tradeTime,
                                    PayMoney = orderAmount,
                                    ComfirmTime = nowTime,
                                    CreateTime = nowTime,
                                    CpySysID = cpySysID,
                                    CpyName = cpyName
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                transaction.Commit();
                                return true;
                            }
                            else
                            {
                                resMsg = "XF系统返回的同一订单编号找不到订单信息";
                                return false;
                            }
                        }
                        else
                        {
                            resMsg = "该订单已获取到流水信息，请检查订单状态是否仍然为支付异常";
                            return false;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
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
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DateTime nowTime = DateTime.Now;
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" UPDATE M_OrderInfo SET OrderStatus =@OrderStatus,ShippingStatus=@ShippingStatus,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo) WHERE OrderID=@_OrderID AND OrderStatus=6 AND ShippingStatus=4 ");
                        if (connection.Execute(strSql.ToString(), new
                        {
                            _OrderID = orderID,
                            ShippingStatus = 5,//物流状态改为已完成
                            OrderStatus = 8, //订单状态改为已完成
                            LogInfo = nowTime + "订单商品完成换货处理"
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        strSql.Clear();//下面这个是一定要改的
                        strSql.Append(" UPDATE M_ReBackOrder SET AuthType=@AuthType,RLogistical=@RLogistical,RLogisticalNumber=@RLogisticalNumber WHERE ReBackID=@_ReBackID AND AuthType=30 ");
                        if (connection.Execute(strSql.ToString(), new
                        {
                            _ReBackID = reBackID,
                            AuthType = 70,//换货完成
                            RLogistical = logisticalName,//再次发货物流名
                            RLogisticalNumber = logisticalNumber//再次发货物流单号
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
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
            string strSql = " UPDATE M_Category SET IsDelete = 1,CateName = CONCAT(CateName,@Vals) WHERE CateID = @_SysID OR ParentID = @_SysID ";
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (connection.Execute(strSql.ToString(), new { Vals = "DELETE" + DateTime.Now.ToShortDateString(), _SysID = sysID }, transaction: transaction, commandTimeout: commandTimeout) < 0)//至少更新一条数据
                        {
                            transaction.Rollback();
                            return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 启禁类目数据（更改启禁）
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public bool IsEnableCategory(int sysID, int status)
        {
            string strSql = " UPDATE M_Category SET IsEnable = @IsEnable WHERE CateID = @_SysID OR ParentID = @_SysID ";
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (connection.Execute(strSql.ToString(), new { IsEnable = status, _SysID = sysID }, transaction: transaction, commandTimeout: commandTimeout) < 0)//至少更新一条数据
                        {
                            transaction.Rollback();
                            return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 删除文件管理图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool DeleteFileManage(string fileName)
        {
            string strSql = " DELETE FROM `M_FileManage` WHERE FileName LIKE @FileName ";
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int resCount = connection.Execute(strSql.ToString(), new { FileName = "%" + fileName + "%" }, transaction: transaction, commandTimeout: commandTimeout);
                        if (resCount < 0 || resCount > 2)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
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
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DateTime nowTime = DateTime.Now;
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" UPDATE M_ReBackOrder SET AuthType=@AuthType,UpdateTime =@UpdateTime,ReBackTradeSysID=@ReBackTradeSysID WHERE ReBackID=@_ReBackID AND AuthType=30 ");//审核流程是30审核通过
                        if (connection.Execute(strSql.ToString(), new
                        {
                            _ReBackID = reBackOrderInfo.ReBackID,
                            AuthType = 40,
                            UpdateTime = nowTime,
                            ReBackTradeSysID = tradeSysID
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        strSql.Clear();
                        strSql.Append(" INSERT INTO M_ReBackTrade ( ReBackTradeSysID,ReBackTradeNo,ReBackID,TradeSysID,TradeNo,PayID,PayName,OrderID,ReBackTime,ReBackMoney,ComfirmStatus,CreateTime,CpyName,CpySysID ) ");
                        strSql.Append(" VALUES ( @ReBackTradeSysID,@ReBackTradeNo,@ReBackID,@TradeSysID,@TradeNo,@PayID,@PayName,@OrderID,@ReBackTime,@ReBackMoney,@ComfirmStatus,@CreateTime,@CpyName,@CpySysID ) ");
                        if (connection.Execute(strSql.ToString(), new
                        {
                            ReBackTradeSysID = tradeSysID,//生成的JUID
                            ReBackTradeNo = reBackTradeNo,//退款返回的流水编号
                            ReBackID = reBackOrderInfo.ReBackID,
                            TradeSysID = reBackOrderInfo.TradeSysID,
                            TradeNo = reBackOrderInfo.TradeNo,
                            PayID = reBackOrderInfo.PayID,
                            PayName = reBackOrderInfo.PayName,
                            OrderID = reBackOrderInfo.OrderID,
                            ReBackTime = nowTime,
                            ReBackMoney = reBackMoney,
                            ComfirmStatus = 10,//退款中
                            CreateTime = nowTime,
                            CpyName = reBackOrderInfo.CpyName,
                            CpySysID = reBackOrderInfo.CpySysID
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
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
            try//如果链接数据库就出错，直接返回false
            {
                using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("INSERT INTO M_ErrorLog  (ErrLogName,ErrLogDesc,ErrLogTime) VALUES( @ErrLogName,@ErrLogDesc,@ErrLogTime)");
                            if (connection.Execute(strSql.ToString(), new { ErrLogName = errName, ErrLogDesc = errDesc, ErrLogTime = DateTime.Now }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch { return false; }
        }

        #endregion
    }
}