using Dapper;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.IDAL;
using DSMTMALL.DB.Model;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DSMTMALL.DB.MySQLDAL
{
    public partial class MY_Dal : IMY_Dal
    {
        private readonly string connectionString = string.Empty;
        private readonly string providerName = "MySql.Data.MySqlClient";
        private readonly int commandTimeout = MyDBHelper.COMMANDTIMEOUT;
        public MY_Dal(DBEnum dbEnum)
        {
            connectionString =MyDBHelper.GetConnectionString(dbEnum);
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
                return connection.Query<int>(strSql.ToString(), objParam, commandTimeout: commandTimeout).SingleOrDefault();
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
        public bool UpdateModelSQL<T>(string strSql, List<object> objList, string tableID) where T : new()
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
                        }else
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
        
        #region 数据获取

        /// <summary>
        /// 首页加载公司推荐产品
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetGoodsListByCpyIndexPage(object objParam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT G.GoodsID,G.GoodsName,G.GoodsImg,G.GoodsNumber, ");
            strSql.Append(" CONCAT(@PathAdd,G.GoodsImg) AS NGoodsImg,");
            strSql.Append(" G.ShopPrice,G.MarketPrice FROM ( SELECT * FROM M_CpyGoods WHERE CpySysID=@CpySysID ) AS CG ");
            strSql.Append(" LEFT JOIN M_Goods AS G ON G.GoodsID = CG.GoodsID ");
            strSql.Append(" WHERE G.IsDelete=0 AND G.IsEnable=1 ORDER BY G.OrderBy ASC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), objParam, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// 获取用户的购物车列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public object GetUserCartList(string url, string userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT C.GoodsID,G.GoodsImg,G.GoodsName,G.GoodsNumber ,C.BuyNumber,G.ShopPrice ,G.MarketPrice ,G.QuotaNumber,C.CartID ,G.SaleNumber,G.IsDelete,G.IsEnable, ");
            strSql.Append(" CONCAT(@PathAdd,G.GoodsImg) AS NGoodsImg ");
            strSql.Append(" FROM M_Cart AS C  ");
            strSql.Append(" LEFT JOIN M_Goods AS G ON C.GoodsID = G.GoodsID ");
            strSql.Append(" WHERE UserID = @UserID ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { UserID=userID, PathAdd =url}, commandTimeout: commandTimeout).Select(info=> {
                    return new { info.GoodsID, info.GoodsImg, info.GoodsName, info.GoodsNumber, info.BuyNumber, info.ShopPrice, info.MarketPrice, info.CartID, info.SaleNumber, info.IsDelete, info.IsEnable,info.NGoodsImg ,info.QuotaNumber };
                });
            }
        }

        /// <summary>
        /// 获取当前ID地址信息-提交订单时使用
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="addressID"></param>
        /// <returns></returns>
        public List<UserAddressToOrder> GetUserAddressInfo<UserAddressToOrder>(string userID, int addressID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT A.*,R.RegionName AS CountryName,R1.RegionName AS ProvinceName,R2.RegionName AS CityName ,R3.RegionName AS DistrictName ");
            if (addressID != 0)
            {
                strSql.Append(" FROM ( SELECT * FROM M_UserAddress WHERE UserID =@UserID AND AddressID = @AddressID ORDER BY AddressFirst DESC ) AS A ");
            }else
            {
                strSql.Append(" FROM ( SELECT * FROM M_UserAddress WHERE UserID =@UserID ORDER BY AddressFirst DESC ) AS A ");
            }
            strSql.Append(" LEFT JOIN M_Region AS R ON A.Country =R.RegionID  ");
            strSql.Append(" LEFT JOIN M_Region AS R1 ON A.Province =R1.RegionID  ");
            strSql.Append(" LEFT JOIN M_Region AS R2 ON A.City =R2.RegionID  ");
            strSql.Append(" LEFT JOIN M_Region AS R3 ON A.District =R3.RegionID  ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<UserAddressToOrder>(strSql.ToString(), new { UserID = userID, AddressID = addressID }, commandTimeout: commandTimeout).ToList();
            }
        }
        
        /// <summary>
        /// 用户提交购物车获取购物车中该商品的限购数量与购买数量
        /// </summary>
        /// <param name="cartIDList"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CartGoodsNumber> GetCartGoodsNumberInfo(string[] cartIDList,string userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT NC.*,G.GoodsNumber,G.QuotaNumber FROM ( SELECT * FROM  M_Cart AS C  WHERE C.UserID = @UserID AND C.CartID IN @CartIDList ) AS NC LEFT JOIN M_Goods AS G ON NC.GoodsID = G.GoodsID ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<CartGoodsNumber>(strSql.ToString(), new { UserID = userID, CartIDList = cartIDList }, commandTimeout: commandTimeout).ToList();
            }
        }
        
        /// <summary>
        /// 获取用户今天已下单的所有商品购买数量
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public List<UserTodayBuyOrderGoods> GetUserTodayBuyOrderGoodsInfo(string userID, DateTime startTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT SUM(OGS.BuyNumber) AS SUMBuyNumber ,OGS.* FROM ( ");
            strSql.Append(" SELECT OG.GoodsID, OG.BuyNumber,OG.GoodsName,OS.* FROM (  ");
            strSql.Append(" SELECT * FROM M_OrderInfo AS O WHERE O.UserID=@UserID AND O.AddTime>@AddTime AND O.OrderStatus IN (0,1,5,6,7,8,9) ) AS OS ");
            strSql.Append(" LEFT JOIN M_OrderGoods AS OG ON OS.OrderID= OG.OrderID ) AS OGS GROUP BY OGS.GoodsID ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<UserTodayBuyOrderGoods>(strSql.ToString(), new { UserID = userID, AddTime = startTime }, commandTimeout: commandTimeout).ToList();
            }
        }


        /// <summary>
        /// 提交购物车订单时获取用户购物车商品信息--合成后
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<GoodsCartToOrder> GetUserCartLsitToOrder(string url, string userID, string[] cartIDList,string provience)
        {
            provience = !string.IsNullOrEmpty(provience) ? provience : "noProvienceID";//判断身份是否为空，如果为空的情况下，将它赋值为noProvienceID字符串--保证匹配不到任何值，只能默认运费模板
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  SELECT * FROM ( SELECT C.BuyNumber,C.CartID,G.GoodsName,G.ShopPrice,G.MarketPrice,G.SaleNumber,G.GoodsNumber AS Inventory, G.GoodsSn,C.GoodsID,G.IsDelete,G.IsEnable ,CONCAT(@PathAdd,G.GoodsImg) AS NGoodsImg ,");
            strSql.Append("  S.SuppliersID,S.SuppliersName,G.FareSysID,F.FirstMoney,G.Weight,F.ContinueCount,F.ContinueMoney,F.FirstCount,F.CarryAddressList ");
            strSql.Append("  FROM M_Cart AS C    ");
            strSql.Append("  LEFT JOIN M_Goods AS G ON C.GoodsID = G.GoodsID    ");
            strSql.Append("  LEFT JOIN M_FareCarry AS F ON G.FareSysID =F.FareSysID ");
            strSql.Append("  LEFT JOIN M_Suppliers AS S ON S.SuppliersID = G.SuppliersID  ");
            strSql.Append("  WHERE C.UserID = @UserID AND C.CanHandler =1");
            strSql.Append("  AND G.IsEnable=1 AND G.IsDelete= 0 AND ( C.CartID IN @CartIDList )");
            strSql.Append("  AND ( F.CarryAddressList LIKE  @ProvienceID  OR F.CarryAddressList='0') ORDER BY F.CarryAddressList DESC ) AS NSelect GROUP BY NSelect.CartID  ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query<GoodsCartToOrder>(strSql.ToString(), new { PathAdd = url, UserID = userID, CartIDList = cartIDList, @ProvienceID= "%k"+provience+"k%" }, commandTimeout: commandTimeout).ToList();
            }
        }
        
        ///// <summary>
        ///// 提交购物车订单时获取用户购物车商品信息——原始版本
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public List<GoodsCartToOrder> GetUserCartLsitToOrder<GoodsCartToOrder>(string url, string userID, string[] cartIDList)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(" SELECT C.GoodsID , G.GoodsImg,G.GoodsName,C.BuyNumber,G.ShopPrice, G.MarketPrice,C.CartID,G.GoodsSn,G.GoodsNumber AS Inventory,G.SaleNumber,G.SuppliersID,S.SuppliersName ,");
        //    strSql.Append(" CONCAT(@PathAdd,G.GoodsImg) AS NGoodsImg ");
        //    strSql.Append(" FROM M_Cart AS C ");
        //    strSql.Append(" LEFT JOIN M_Goods AS G ON C.GoodsID = G.GoodsID ");
        //    strSql.Append(" LEFT JOIN M_Suppliers AS S ON S.SuppliersID = G.SuppliersID");
        //    strSql.Append(" WHERE UserID = @UserID AND CanHandler =1 AND G.IsEnable=1 AND G.IsDelete= 0 AND ( CartID IN @CartIDList ) ");
        //    using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
        //    {
        //        connection.ConnectionString = connectionString;
        //        return  connection.Query<GoodsCartToOrder>(strSql.ToString(), new { PathAdd = url, UserID = userID, CartIDList = cartIDList }, commandTimeout: commandTimeout).ToList();
        //    }
        //}
        ///// <summary>
        ///// 获取用户购物车的商品对应的结算运费信息——原始版本
        ///// </summary>
        ///// <param name="provience">发货的地区ID</param>
        ///// <returns></returns>
        //public IEnumerable<dynamic> GetCartGoodsFareTempInfo(string provience)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(" SELECT CartID,FirstMoney,FareSysID,FareType,FirstCount,ContinueCount,ContinueMoney,Weight,BuyNumber,SuppliersID,CarryAddressList FROM");
        //    strSql.Append(" (SELECT C.*,G.Weight,G.SuppliersID,NFareCarry.* from M_Cart AS C LEFT JOIN M_Goods AS G ON C.GoodsID = G.GoodsID ");
        //    strSql.Append(" LEFT JOIN(SELECT * FROM M_FareCarry WHERE(CarryAddressList LIKE @_ProvienceID  OR CarryAddressList = '0')) AS NFareCarry ON G.FareSysID = NFareCarry.FareSysID  ");
        //    strSql.Append(" ORDER BY CarryAddressList DESC) AS NewTab GROUP BY NewTab.CartID");
        //    using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
        //    {
        //        connection.ConnectionString = connectionString;
        //        return connection.Query(strSql.ToString(), new { _ProvienceID = "%_"+provience+"%" }, commandTimeout: commandTimeout);
        //    }
        //}
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT G.GoodsID,G.GoodsName,G.GoodsImg,G.GoodsNumber, ");
            strSql.Append(" CONCAT(@PathAdd,G.GoodsImg) AS NGoodsImg ,");
            strSql.Append(" G.ShopPrice,G.MarketPrice FROM M_Goods AS G WHERE ( G.IsHot= 1 OR G.IsNew=1 ) AND G.IsDelete=0 AND G.IsEnable=1 AND G.IsPromote=0 ORDER BY G.OrderBy ASC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), objParam, commandTimeout: commandTimeout).Select(info => { return new { info.GoodsID, info.GoodsName, info.NGoodsImg, info.MarketPrice, info.ShopPrice, info.GoodsNumber }; });
            }
        }

        /// <summary>
        /// 首页加载产品
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <returns></returns>
        public object GetGoodsListByHomePage(object objParam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT G.GoodsID,G.GoodsName,G.GoodsImg,G.GoodsNumber, ");
            strSql.Append(" CONCAT(@PathAdd,G.GoodsImg) AS NGoodsImg ,");
            strSql.Append(" G.ShopPrice,G.MarketPrice FROM M_Goods AS G WHERE  G.IsDelete=0 AND G.IsEnable=1 AND G.IsPromote=0 ORDER BY G.LastUpdate ASC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), objParam, commandTimeout: commandTimeout).Select(info => { return new { info.GoodsID, info.GoodsName, info.NGoodsImg, info.MarketPrice, info.ShopPrice, info.GoodsNumber }; });
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT G.GoodsID,G.GoodsName,G.GoodsImg,G.GoodsName,G.ShopPrice,G.MarketPrice,G.SaleNumber,G.GoodsNumber, ");
            strSql.Append(" CONCAT(@PathAdd,G.GoodsImg) AS NGoodsImg ");
            strSql.Append(" FROM M_Goods AS G  ");
            strSql.Append(" LEFT JOIN M_Category AS C ON G.CateID = C.CateID ");
            strSql.Append(" WHERE 1=1 AND G.IsDelete=0 AND G.IsEnable=1 AND G.IsPromote=0 ");
            if (!string.IsNullOrEmpty(searchName))
            {
                strSql.Append(" AND ( G.KeyWords LIKE @SearchName OR C.KeyWords LIKE @SearchName ) ");
                if (type != 0)
                {
                    strSql.Append(" AND ( G.CateID = @CateID OR C.ParentID = @CateID) ");
                }
            }
            else
            {
                strSql.Append(" AND ( G.CateID = @CateID OR C.ParentID = @CateID) ");
            }
            if (sort == 0)
            {
                strSql.Append(" ORDER BY G.IsHot DESC ,G.OrderBy ASC , G.LastUpdate DESC ");
            }
            else if (sort == 1)
            {
                strSql.Append(" ORDER BY G.SaleNumber DESC , G.OrderBy ASC , G.LastUpdate DESC ");
            }
            else if (sort == 2)
            {
                strSql.Append(" ORDER BY G.ShopPrice ASC , G.OrderBy ASC , G.LastUpdate DESC ");
            }
            strSql.Append(" LIMIT @StartSize , @EndSize ");
             using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { PathAdd = url, CateID = type, SearchName ="%"+searchName+"%", StartSize = startSize, EndSize = endSize }, commandTimeout: commandTimeout).Select(info => { return new { info.GoodsID, info.GoodsName, info.NGoodsImg, info.MarketPrice, info.ShopPrice,info.GoodsNumber }; });
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT O.OrderSn,O.Mobile,O.PayTime,CONCAT(O.CountryName,O.ProvinceName,O.CityName,O.DistrictName,O.Address) AS NOrderAddress,O.Consignee, O.PayName,O.OrderID,O.OrderStatus,O.SuppliersName,O.ShippingStatus,O.ShippingFee,");
            strSql.Append(" CASE WHEN O.OrderStatus=0 THEN '待确认' WHEN O.OrderStatus=1 THEN '订单已确认' WHEN O.OrderStatus=2 THEN '订单已取消' WHEN O.OrderStatus =3 THEN '订单已关闭' WHEN O.OrderStatus =4 THEN '退货完成' WHEN O.OrderStatus =5 THEN '订单已发货' ");
            strSql.Append(" WHEN O.OrderStatus =6 THEN '售后处理中' WHEN O.OrderStatus =7 THEN '订单异常' WHEN O.OrderStatus =8 THEN '订单已完成' WHEN O.OrderStatus =9 THEN '订单已推送' WHEN O.OrderStatus =10 THEN '退款成功' WHEN O.OrderStatus =11 THEN '已完成换货' ELSE '未知' END NOrderStatus, ");
            strSql.Append(" CASE WHEN O.PayStatus=0 THEN '未付款' WHEN O.PayStatus=1 THEN '扣款中' WHEN O.PayStatus=2 THEN '扣款已成功' WHEN O.PayStatus =3 THEN '扣款失败' WHEN O.PayStatus =4 THEN '付款超时' WHEN O.PayStatus =9 THEN '支付异常'  ELSE '未知' END NPayStatus, ");
            strSql.Append(" CASE WHEN O.ShippingStatus=0 THEN '未发货' WHEN O.ShippingStatus=1 THEN '商品已发出' WHEN O.ShippingStatus=2 THEN '已确认收货' WHEN O.ShippingStatus =3 THEN '未收到货' WHEN O.ShippingStatus=4 THEN '退货中' WHEN O.ShippingStatus=5 THEN '已完成' ELSE '未知' END NShippingStatus, ");
            strSql.Append(" O.OrderStatus, O.PayStatus,O.ShippingStatus,O.OrderAmount,O.AddTime,O.Logistical,O.LogisticalNumber FROM M_OrderInfo AS O ");
            strSql.Append(" WHERE O.UserID =@UserID ");
            if (!string.IsNullOrEmpty(statusType))
            {
                if (statusType == "obligation")//未付款
                {
                    strSql.Append(" AND PayStatus=0 ");
                }
                else if (statusType == "TRS")//代收货
                {
                    strSql.Append(" AND ( ShippingStatus = 1 OR ShippingStatus = 0) AND PayStatus =2 AND (OrderStatus = 1 OR OrderStatus=9 OR OrderStatus=5) ");
                }
                strSql.Append(" AND OrderStatus !=2 ");
            }
            strSql.Append(" ORDER BY O.AddTime DESC ");
            strSql.Append(" LIMIT @StartSize , @EndSize ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { StartSize = startSize, EndSize = endSize , UserID =sysID}, commandTimeout: commandTimeout).Select(info => {
                    return new { info.OrderSn, info.Consignee, info.PayName, info.Mobile, info.NOrderAddress ,PayTime=info.PayTime.ToString("yyyy-MM-dd HH:mm:ss") ,AddTime= info.AddTime.ToString("yyyy-MM-dd HH:mm:ss") ,info.NOrderStatus ,info.OrderStatus,
                        info.NPayStatus,info.PayStatus,info.NShippingStatus,info.OrderAmount,info.OrderID,info.SuppliersName ,info.ShippingStatus,info.ShippingFee,info.LogisticalNumber,info.Logistical
                    };
                });
            }
        }

        /// <summary>
        /// 获取订单的产品信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public object GetUserOrderGoods(string url, string orderID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT OG.*,G.GoodsImg ,");
            strSql.Append(" CONCAT(@PathAdd,G.GoodsImg) AS NGoodsImg ");
            strSql.Append(" FROM M_OrderGoods AS OG ");
            strSql.Append(" LEFT JOIN M_Goods AS G ON OG.GoodsID = G.GoodsID ");
            strSql.Append(" WHERE OG.OrderID =@OrderID ");
            using (IDbConnection connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                return connection.Query(strSql.ToString(), new { PathAdd = url, OrderID = orderID }, commandTimeout: commandTimeout).Select(info => {
                    return new{ info.OrderSn,info.GoodsID,info.GoodsSn,info.GoodsPrice,info.GoodsName,info.NGoodsImg,info.BuyNumber,info.GoodsAttr };
                });
            }
        }

        #endregion

        #region 事务操作

        /// <summary>
        /// 提交订单待付款
        /// </summary>
        /// <param name="dt">订单的商品集合</param>
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
        public bool SubmitOrderList(List<GoodsCartToOrder> goodsCarts, string orderUnifySn, List<string> orderSnList, string userID, decimal payMonery, UserAddressToOrder addressInfo, Dictionary<int, double> feeAmount, M_Payment payment, out string outErrInfo, string openID)
        {
            DateTime nowTime = DateTime.Now;
            string orderID = string.Empty;
            string orderSn = string.Empty;
            int suppliersID = 0;
            string suppliersName = string.Empty;
            outErrInfo = string.Empty;
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        decimal payMoneryCount = 0;
                        decimal orderAmount = 0;
                        decimal orderPayAmount = 0;
                        decimal subFeeAmount = 0;
                        decimal sumFeeAmount = 0;
                        //对订单进行拆分，按照供应商来源的不同，拆分成不同的子订单，由统一下单ID进行下单,统一下单ID就是传入的orderSn, 子订单ID就是在这个orderSn后面加上供应商数字ID
                        for (int i = 0; i < orderSnList.Count; i++)//循环子订单列表信息
                        {
                            orderID = Guid.NewGuid().ToString();//生成该子订单的订单GUID
                            orderAmount = 0;//遍历子订单列表初始化当前子订单总支付金额
                            orderPayAmount = 0;//初始化当前子订单商品总额
                            suppliersID = 0;//初始化供货商ID
                            subFeeAmount = 0;//初始化运费
                            suppliersName = string.Empty;//初始化供货商名称
                            //生成订单商品中间表//遍历商品信息表
                            for (int x = 0; x < goodsCarts.Count; x++)//for循环所有的商品信息
                            {
                                //多一步判断这个子订单号是否在orderSnList集合中作校验添加一个保险
                                orderSn = orderUnifySn + goodsCarts[x].SuppliersID.ToString();
                                if (orderSnList.Contains(orderSn))//根据供应商来源生成的子订单号是否在子订单集合中，
                                {
                                    if (orderSn == orderSnList[i])//并且当等于当前的这个子订单循环体的当前子订单号//进行子订单号商品的中间表生成
                                    {
                                        orderPayAmount += goodsCarts[x].BuyNumber * goodsCarts[x].ShopPrice;//此商品价格
                                        suppliersID = Convert.ToInt32(goodsCarts[x].SuppliersID);
                                        suppliersName = goodsCarts[x].SuppliersName;
                                        strSql.Clear();
                                        strSql.Append(" INSERT INTO M_OrderGoods (OrderID ,OrderSn,OrderUnifySn, GoodsID, GoodsName,GoodsSn,BuyNumber,MarketPrice,GoodsPrice,GoodsAttr,IsReal,AddTime)");
                                        strSql.Append(" VALUES ( @OrderID,@OrderSn,@OrderUnifySn,@GoodsID,@GoodsName,@GoodsSn,@BuyNumber,@MarketPrice,@GoodsPrice,@GoodsAttr,@IsReal,@AddTime ) ");
                                        if (connection.Execute(strSql.ToString(), new
                                        {
                                            OrderID = orderID,
                                            OrderSn = orderSn,
                                            OrderUnifySn = orderUnifySn,
                                            GoodsID = goodsCarts[x].GoodsID,
                                            GoodsName = goodsCarts[x].GoodsName,
                                            GoodsSn = goodsCarts[x].GoodsSn,
                                            BuyNumber = goodsCarts[x].BuyNumber,
                                            MarketPrice = goodsCarts[x].MarketPrice,
                                            GoodsPrice = goodsCarts[x].ShopPrice,
                                            GoodsAttr = string.Empty,
                                            IsReal = 1,
                                            AddTime = nowTime
                                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                        {
                                            outErrInfo = "系统访问量过载，请稍后提交";
                                            transaction.Rollback();
                                            return false;
                                        }
                                    }
                                }
                                else
                                {
                                    outErrInfo = goodsCarts[x].GoodsName + "供应商数据出错";
                                    transaction.Rollback();
                                    return false;
                                }

                            }
                            if (feeAmount.ContainsKey(suppliersID))
                            {
                                subFeeAmount =Convert.ToDecimal( feeAmount[suppliersID]);//获取当前分单的供应商运费价格
                                sumFeeAmount += subFeeAmount;
                            }
                            orderAmount = orderPayAmount +subFeeAmount+ payment.PayFee;
                            //创建订单表数据
                            strSql.Clear();
                            strSql.Append(" INSERT INTO M_OrderInfo(OrderID,OrderUnifySn,OrderSn,UserID,OpenID,Consignee,Country,CountryName,Province,ProvinceName,City,CityName,District,DistrictName,Address,Mobile,Postscript,ShippingID,ShippingName,ShippingFee,PayID,PayName,GoodsAmount,PayFee,MoneyPaid,OrderAmount,AddTime,UpdateTime,ModifyPerson,SuppliersID,SuppliersName,UserAddressID,ConsigneeCardNo,Version,LogInfo ) ");
                            strSql.Append(" VALUES (@OrderID,@OrderUnifySn,@OrderSn,@UserID,@OpenID,@Consignee,@Country,@CountryName,@Province,@ProvinceName,@City,@CityName,@District,@DistrictName,@Address,@Mobile,@Postscript,@ShippingID,@ShippingName,@ShippingFee,@PayID,PayName,@GoodsAmount,@PayFee,@MoneyPaid,@OrderAmount,@AddTime,@UpdateTime,@ModifyPerson,@SuppliersID,@SuppliersName,@UserAddressID,@ConsigneeCardNo,@Version,@LogInfo ) ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                OrderID = orderID,
                                OrderUnifySn = orderUnifySn,
                                OrderSn = orderSnList[i],
                                UserID = userID,
                                OpenID = openID,
                                Consignee = addressInfo.Consignee,
                                Country = addressInfo.Country,
                                CountryName = addressInfo.CountryName,
                                Province = addressInfo.Province,
                                ProvinceName = addressInfo.ProvinceName,
                                City = addressInfo.City,
                                CityName = addressInfo.CityName,
                                District = addressInfo.District,
                                DistrictName = addressInfo.DistrictName,
                                Address = addressInfo.Address,
                                Mobile = addressInfo.Mobile,
                                Postscript = string.Empty,
                                ShippingID = 2,
                                ShippingName = string.Empty,
                                ShippingFee = subFeeAmount,
                                PayID = payment.PayID,
                                PayName = payment.PayName,
                                GoodsAmount = orderPayAmount,
                                PayFee = payment.PayFee,//支付金额
                                MoneyPaid = 0,//已付款金额
                                OrderAmount = orderAmount,
                                AddTime = nowTime,
                                UpdateTime = nowTime,
                                ModifyPerson = "用户提交",
                                SuppliersID = suppliersID,
                                SuppliersName = suppliersName,
                                UserAddressID = addressInfo.AddressID,
                                ConsigneeCardNo = addressInfo.ConsigneeCardNo,
                                Version = 0,
                                LogInfo = nowTime + "用户提交订单"
                            }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                outErrInfo = "系统访问量过载，请稍后提交";
                                transaction.Rollback();
                                return false;
                            }
                        }
                        //库存余量更改表 与 删除该用户购物车相应的商品
                        for (int i = 0; i < goodsCarts.Count; i++)
                        {   //库存余量更改表
                            strSql.Clear();
                            payMoneryCount += goodsCarts[i].BuyNumber * goodsCarts[i].ShopPrice;
                            strSql.Append(" UPDATE M_Goods G SET G.GoodsNumber = G.GoodsNumber-@BuyNumber, G.SaleNumber = G.SaleNumber+@BuyNumber WHERE GoodsID =@GoodsID AND G.GoodsNumber>=@BuyNumber  ");
                            if (connection.Execute(strSql.ToString(), new { BuyNumber = goodsCarts[i].BuyNumber, GoodsID = goodsCarts[i].GoodsID }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                outErrInfo = goodsCarts[i].GoodsName + "库存余量不足";
                                transaction.Rollback();
                                return false;
                            }

                            //删除购物车相应的商品
                            strSql.Clear();
                            strSql.Append(" DELETE FROM M_Cart WHERE UserID = @UserID AND GoodsID =@GoodsID ");
                            if (connection.Execute(strSql.ToString(), new { UserID = userID, GoodsID = goodsCarts[i].GoodsID }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                outErrInfo = "系统访问量过载，请稍后提交";
                                transaction.Rollback();
                                return false;
                            }
                        }
                        if (payMoneryCount + sumFeeAmount != payMonery)//核对金额是否正确
                        {
                            outErrInfo = "资金结算出错";
                            transaction.Rollback();
                            return false;
                        }
                        
                        transaction.Commit();
                        outErrInfo = "SUCCESS";
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        outErrInfo = e.Message;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 去掉订单释放库存
        /// </summary>
        /// <param name="orderArr">订单商品集合</param>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        public bool CancelOrder(string orderID)
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
                        //获取订单的商品集合
                        strSql.Append(" SELECT * FROM M_OrderGoods WHERE OrderID = @OrderID ");
                        IEnumerable<dynamic> orderGoodsInfoList = connection.Query(strSql.ToString(), new { OrderID = orderID }, transaction: transaction, commandTimeout: commandTimeout);
                        foreach (var item in orderGoodsInfoList)
                        {
                            strSql.Clear();
                            strSql.Append(" UPDATE M_Goods G SET G.GoodsNumber = G.GoodsNumber+@BuyNumber, G.SaleNumber=G.SaleNumber-@BuyNumber WHERE GoodsID =@_GoodsID ");
                            if (connection.Execute(strSql.ToString(), new { _GoodsID = item.GoodsID, BuyNumber = item.BuyNumber }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        //更改订单信息
                        strSql.Clear();
                        strSql.Append(" UPDATE M_OrderInfo SET OrderStatus=2 ,PayStatus=0,Version=Version+1 WHERE OrderID = @_OrderID AND PayStatus=0 AND OrderStatus= 0 ");//付款状态为未付款，订单状态为已取消
                        if (connection.Execute(strSql.ToString(), new { _OrderID = orderID }, transaction: transaction, commandTimeout: commandTimeout) != 1)
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
        /// 同步程序去订单释库存
        /// </summary>
        /// <param name = "orderID" ></ param >
        /// < returns ></ returns >
        public bool CancelOrderSync(string orderID)
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
                        //更改订单信息
                        strSql.Clear();
                        strSql.Append(" UPDATE M_OrderInfo SET OrderStatus=2 ,PayStatus=4,Version=Version+1 WHERE OrderID = @_OrderID AND PayStatus=0 AND OrderStatus= 0");//付款状态为未付款，订单状态为已取消
                        if (connection.Execute(strSql.ToString(), new { _OrderID = orderID }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        //获取订单的商品集合
                        strSql.Clear();
                        strSql.Append(" SELECT * FROM M_OrderGoods WHERE OrderID = @_OrderID ");
                        IEnumerable<dynamic> orderGoodsInfoList = connection.Query(strSql.ToString(), new { _OrderID = orderID }, transaction: transaction, commandTimeout: commandTimeout);
                        foreach (var item in orderGoodsInfoList)
                        {
                            strSql.Clear();
                            strSql.Append(" UPDATE M_Goods G SET G.GoodsNumber = G.GoodsNumber+@BuyNumber, G.SaleNumber=G.SaleNumber-@BuyNumber WHERE GoodsID =@_GoodsID ");
                            if (connection.Execute(strSql.ToString(), new { _GoodsID = item.GoodsID, BuyNumber = item.BuyNumber }, transaction: transaction, commandTimeout: commandTimeout) != 1)
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
        public bool BingUserAccount(string phone, string openID, string pwd, BindingInfo userInfo)
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
                        strSql.Clear();
                        strSql.Append(" INSERT INTO M_Users(UserID, UserName, NickName, Password, Phone, Sex, IsEnable, CpyName, CpySysID, OpenID,RegTime,LastLogin,SimpleName)  ");
                        strSql.Append(" VALUES ( @UserID,@UserName, @NickName, @Password, @Phone, @Sex, @IsEnable, @CpyName, @CpySysID, @OpenID,@RegTime,@LastLogin,@SimpleName ) ");
                        if (connection.Execute(strSql.ToString(), new
                        {
                            UserID = Guid.NewGuid().ToString(),
                            UserName =!string.IsNullOrEmpty(userInfo.RealName) ? userInfo.RealName : phone,
                            NickName = phone,
                            Password = pwd,
                            Phone = phone,
                            Sex = ToolHelper.ConventToInt32(userInfo.UserGender, 0),
                            IsEnable = 1,
                            CpyName = userInfo.CpyName,
                            CpySysID = userInfo.CpySysID,
                            OpenID = openID,
                            RegTime = nowTime,
                            LastLogin = nowTime,
                            SimpleName=userInfo.SimpleName
                        }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        //删除手机短信
                        strSql.Clear();
                        strSql.Append(" DELETE FROM M_TelPhoneCode WHERE Telphone = @Telphone ");
                        connection.Execute(strSql.ToString(), new { Telphone = phone }, transaction: transaction, commandTimeout: commandTimeout);

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
        /// 更新订单状态信息为支付异常-OK
        /// </summary>
        /// <param name="pushType">推送类型统一支付还是分单支付</param>
        /// <param name="pushOrderSn">推送的订单Sn</param>
        /// <returns></returns>
        public bool UpdateOrderStatusByXFPaymentToUnusual(string pushType, string pushOrderSn,string pushDsc)
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
                        if (pushType == "orderSn")
                        {
                            strSql.Append(" UPDATE M_OrderInfo SET PayStatus=9, Version=Version+1,Postscript=@Postscript,LogInfo=CONCAT(LogInfo,@LogInfo) WHERE OrderSn =@_PushOrderSn AND (PayStatus=1 OR PayStatus=0)  ");//订单会发生支付异常只会发生在支付未确认,或未支付时候
                        }
                        else if (pushType == "unifySn")
                        {
                            strSql.Append(" UPDATE M_OrderInfo SET PayStatus=9,Version=Version+1,Postscript=@Postscript,LogInfo=CONCAT(LogInfo,@LogInfo) WHERE OrderUnifySn =@_PushOrderSn AND (PayStatus=1 OR PayStatus=0) ");
                        }
                        else
                        {
                            return false;
                        }
                        if (connection.Execute(strSql.ToString(), new { _PushOrderSn = pushOrderSn , Postscript =pushDsc, LogInfo =DateTime.Now+"订单发生支付异常" }, transaction: transaction, commandTimeout: commandTimeout) != 1)
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
        /// 更新订单信息-从付款系统返回支付结果 -OK
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
        public bool UpdateOrderInfoFromXFPayment(List<M_OrderInfo> orderInfoList,  decimal orderAmount, string tradeNo, DateTime tradeTime, M_Payment paymentInfo, string backUnifyOrderSn, string backOrderSn, string phone, string cpySysID, string cpyName)
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
                        string tradeSysID = Guid.NewGuid().ToString();
                        strSql.Append(" SELECT Count(*)  FROM M_OrderTrade WHERE TradeNo=@_TradeNo");
                        int tradeInfo = connection.Query<int>(strSql.ToString(), new { _TradeNo = tradeNo }).FirstOrDefault();//判断是否已经存在XF系统返回的流水号信息
                        if (tradeInfo == 0)
                        {
                            //创建资金流水表
                            strSql.Clear();
                            strSql.Append(" INSERT INTO M_OrderTrade(TradeSysID, PayID, PayName, OrderUnifySn, OrderPayNo, TradeNo, PayTime, PayMoney,ComfirmTime, CreateTime,CpySysID,CpyName)  ");
                            strSql.Append(" VALUES ( @TradeSysID, @PayID, @PayName, @OrderUnifySn, @OrderPayNo, @TradeNo,@PayTime, @PayMoney,@ComfirmTime, @CreateTime,@CpySysID,@CpyName ) ");
                            if (connection.Execute(strSql.ToString(), new
                            {
                                TradeSysID = tradeSysID,
                                PayID = paymentInfo.PayID,
                                PayName = paymentInfo.PayName,
                                OrderUnifySn = backUnifyOrderSn,
                                OrderPayNo = string.IsNullOrEmpty(backOrderSn) ? backUnifyOrderSn : backOrderSn,
                                TradeNo = tradeNo,
                                PayTime = tradeTime,//支付时间与XF系统的时间一致
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
                            //更改订单状态信息
                            for (int i = 0; i < orderInfoList.Count; i++)
                            {
                                strSql.Clear();//只有订单状态是待确认支付状态是付款中才允许更改订单状态
                                strSql.Append(" UPDATE M_OrderInfo SET PayStatus=1 ,PayID=@PayID,PayName=@PayName,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo), ");
                                strSql.Append(" PayTime=@PayTime,TradeSysID=@TradeSysID,TradeNo=@TradeNo,CpySysID=@CpySysID,CpyName=@CpyName,MoneyPaid=@MoneyPaid WHERE OrderID =@_OrderID ");
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    PayID = paymentInfo.PayID,
                                    PayName = paymentInfo.PayName,
                                    PayTime = nowTime,
                                    TradeSysID = tradeSysID,
                                    TradeNo = tradeNo,
                                    CpySysID = cpySysID,
                                    CpyName = cpyName,
                                    LogInfo = nowTime + "用户提交付款成功",
                                    MoneyPaid = orderInfoList[i].OrderAmount,
                                    _OrderID = orderInfoList[i].OrderID
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                            //删除手机短信
                            strSql.Clear();
                            strSql.Append(" DELETE FROM M_TelPhoneCode WHERE Telphone = @_Telphone ");
                            connection.Execute(strSql.ToString(), new { _Telphone = phone }, transaction: transaction, commandTimeout: commandTimeout);//删除手机短信无所谓成功与否

                            transaction.Commit();
                            return true;
                        }else
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
        /// 更新订单状态信息为支付成功 -OK
        /// </summary>
        /// <param name="orderInfoList">要更新的订单集合</param>
        /// <param name="payStatus">推送的订单返回的XF系统同步扣款状态</param>
        /// <returns></returns>
        public bool UpdateOrderStatusByXFPaymentToComfirm(decimal tradeMoney, string tradeNo, string payStatus,out string msg,out string openID)
        {
            msg = string.Empty;
            openID = string.Empty;
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" SELECT * FROM M_OrderInfo WHERE TradeNo=@_TradeNo ");
                        IEnumerable<dynamic> infos = connection.Query(strSql.ToString(), new { _TradeNo = tradeNo });
                        if (infos != null && infos.Count() > 0)
                        {
                            decimal amountMoney = 0;
                            DateTime nowTime = DateTime.Now;
                            foreach (var item in infos)
                            {
                                amountMoney += item.OrderAmount;
                                openID = item.OpenID;
                            }
                            if (amountMoney == tradeMoney)//真正执行更改操作
                            {
                                strSql.Clear();
                                strSql.Append(" UPDATE M_OrderInfo SET PayStatus=@PayStatus,OrderStatus =@OrderStatus,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo)  WHERE TradeNo=@_TradeNo AND PayStatus=1 AND OrderStatus=0 ");
                                //更改订单状态信息 --这笔订单必须有这个流水号，同时订单状态是待确认支付状态是支付中
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    _TradeNo = tradeNo,
                                    PayStatus = (payStatus == "10") ? 2 : 3,//支付状态2已付款3付款失败
                                    OrderStatus = (payStatus == "10") ? 1 : 3,//订单状态1已确认3无效
                                    LogInfo = nowTime + "获取XF系统返回的扣款状态"
                                }, transaction: transaction, commandTimeout: commandTimeout) != infos.Count())
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                strSql.Clear();
                                strSql.Append(" UPDATE M_OrderTrade SET ComfirmStatus=20 ,ComfirmTime=@_ComfirmTime  WHERE TradeNo=@_TradeNo ");
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    _TradeNo = tradeNo,
                                    _ComfirmTime = nowTime
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                transaction.Commit();
                                return true;
                            }else
                            {
                                msg = tradeNo+ "该笔流水号对应的订单金额总和有出入，请校对该笔流水信息";
                            }
                        }else
                        {
                            msg = tradeNo+ "该笔流水号无法找到对应的订单信息，用户提交支付时很可能系统出错，导致订单支付没有被记录";
                        }
                        return false;
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
        /// 更新从XF系统返回的流水号同步状态 -OK
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        public bool UpdateOrderStatusByXFPaymentToComfirm(decimal tradeMoney,string tradeSysID,string payStatus,out string openID)
        {
            openID = string.Empty;
            using (var connection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" SELECT * FROM M_OrderInfo WHERE TradeSysID=@_TradeSysID ");
                        IEnumerable<dynamic> infos = connection.Query(strSql.ToString(), new { _TradeSysID = tradeSysID });
                        if (infos != null&&infos.Count()>0)
                        {
                            decimal amountMoney = 0;
                            DateTime nowTime = DateTime.Now;
                            foreach (var item in infos)
                            {
                                openID = item.OpenID;
                                amountMoney += item.OrderAmount;
                            }
                            if (amountMoney == tradeMoney)//真正执行更改操作
                            {
                                strSql.Clear();
                                strSql.Append(" UPDATE M_OrderInfo SET PayStatus=@PayStatus,OrderStatus =@OrderStatus,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo)  WHERE TradeSysID=@_TradeSysID AND PayStatus=1 AND OrderStatus=0 ");
                                //更改订单状态信息
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    _TradeSysID = tradeSysID,
                                    PayStatus = (payStatus == "10") ? 2 : 3,//支付状态2已付款3付款失败
                                    OrderStatus = (payStatus == "10") ? 1 : 3,//订单状态1已确认3无效
                                    LogInfo = nowTime + "获取XF系统返回的扣款状态"
                                }, transaction: transaction, commandTimeout: commandTimeout) != infos.Count())
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                strSql.Clear();
                                strSql.Append(" UPDATE M_OrderTrade SET ComfirmStatus=20 ,ComfirmTime=@_ComfirmTime  WHERE TradeSysID=@_TradeSysID ");
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    _TradeSysID = tradeSysID,
                                    _ComfirmTime=nowTime
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                transaction.Commit();
                                return true;
                            }
                        }
                        return false;
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
        /// 更新从XF系统返回的退款流水号同步状态-OK
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        public bool UpdateReBackPayAccByXFPaymentToComfirm(decimal tradeMoney, string tradeSysID, string payStatus,out string openID)
        {
            openID = string.Empty;
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
                        strSql.Append(" SELECT RT.ReBackMoney,RT.OrderID,RO.ReBackType,RO.OpenID FROM M_ReBackTrade AS RT ");
                        strSql.Append(" LEFT JOIN M_ReBackOrder AS RO ON RO.ReBackID=RT.ReBackID ");
                        strSql.Append(" WHERE RT.ReBackTradeSysID=@_ReBackTradeSysID ");
                        IEnumerable<dynamic> infos = connection.Query(strSql.ToString(), new { _ReBackTradeSysID = tradeSysID }).Select(info => new { info.ReBackMoney, info.OrderID, info.ReBackType,info.OpenID });
                        decimal returnBackMoney = 0;
                        int reBackType = 0;
                        string orderID = string.Empty;
                        if (infos != null && infos.Count()==1)
                        {
                            foreach (var item in infos)
                            {
                                reBackType = item.ReBackType;
                                returnBackMoney += item.ReBackMoney;
                                orderID = item.OrderID;
                                openID = item.OpenID;
                            }
                            if (returnBackMoney == tradeMoney)
                            {
                                strSql.Clear();
                                strSql.Append(" UPDATE M_ReBackOrder SET AuthType=@AuthType ,UpdateTime=@UpdateTime WHERE ReBackTradeSysID = @_ReBackTradeSysID ");
                                //更改订单状态信息
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    _ReBackTradeSysID = tradeSysID,
                                    UpdateTime=nowTime,
                                    AuthType = (payStatus == "10") ? 50 : 60,//退款订单状态2已付款3付款失败
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                strSql.Clear();//付款流水与退款流水不同，付款流水只需要确认流水是否同步成功就行了，因为失败后整个订单就失效了，但是退款流水如果退款失败，是还可以继续发起退款的，所以只能在退款流水这里做标记，所以才有20，30
                                strSql.Append(" UPDATE M_ReBackTrade SET ComfirmStatus=@ComfirmStatus ,ComfirmTime=@ComfirmTime  WHERE ReBackTradeSysID=@_ReBackTradeSysID ");
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    _ReBackTradeSysID = tradeSysID,
                                    ComfirmTime = nowTime,
                                    ComfirmStatus = (payStatus == "10") ? 20 : 30,//20退款流水返回成功//30退款流水返回失败
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                if (payStatus == "10")
                                {
                                    strSql.Clear();
                                    strSql.Append(" UPDATE M_OrderInfo SET OrderStatus =@OrderStatus,ShippingStatus=@ShippingStatus,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo)  WHERE OrderID=@_OrderID AND OrderStatus=6  ");
                                    //更改订单状态信息
                                    if (connection.Execute(strSql.ToString(), new
                                    {
                                        _OrderID = orderID,
                                        OrderStatus = (reBackType ==10 ) ? 4 : 10,//订单状态改变，如果没有
                                        ShippingStatus=5,
                                        LogInfo = nowTime + "获取XF系统返回的退款状态"
                                    }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                    {
                                        transaction.Rollback();
                                        return false;
                                    }
                                }
                                transaction.Commit();
                                return true;
                            }
                        }
                        return false;
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
        /// 更新从XF系统返回的退款流水号同步状态 -OK
        /// </summary>
        /// <param name="tradeMoney"></param>
        /// <param name="tradeSysID"></param>
        /// <returns></returns>
        public bool UpdateReBackPayAccByXFPaymentToComfirm(decimal tradeMoney, string tradeNo, string payStatus,out string msg, out string openID)
        {
            msg = string.Empty;
            openID = string.Empty;
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
                        strSql.Append(" SELECT RT.ReBackMoney,RT.OrderID,RO.ReBackType,RO.ReBackTradeSysID,RO.OpenID FROM M_ReBackTrade AS RT ");
                        strSql.Append(" LEFT JOIN M_ReBackOrder AS RO ON RO.ReBackID=RT.ReBackID ");
                        strSql.Append(" WHERE RT.ReBackTradeNo=@_ReBackTradeNo ");
                        IEnumerable<dynamic> infos = connection.Query(strSql.ToString(), new { _ReBackTradeNo = tradeNo }).Select(info=>new {info.ReBackMoney,info.OrderID,info.ReBackType,info.ReBackTradeSysID,info.OpenID });
                        decimal returnBackMoney = 0;
                        int reBackType = 0;
                        string orderID = string.Empty;
                        string reBackTradeSysID = string.Empty;
                        if (infos != null && infos.Count() == 1)
                        {
                            foreach (var item in infos)
                            {
                                reBackType = item.ReBackType;
                                returnBackMoney += item.ReBackMoney;
                                orderID = item.OrderID;
                                reBackTradeSysID = item.ReBackTradeSysID;
                                openID = item.OpenID;
                            }
                            if (returnBackMoney == tradeMoney)
                            {
                                strSql.Clear();
                                strSql.Append(" UPDATE M_ReBackOrder SET AuthType=@AuthType ,UpdateTime=@UpdateTime WHERE ReBackTradeSysID = @_ReBackTradeSysID ");
                                //更改订单状态信息
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    _ReBackTradeSysID = reBackTradeSysID,
                                    UpdateTime = nowTime,
                                    AuthType = (payStatus == "10") ? 50 : 60,//退款订单状态2已付款3付款失败
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                strSql.Clear();//付款流水与退款流水不同，付款流水只需要确认流水是否同步成功就行了，因为失败后整个订单就失效了，但是退款流水如果退款失败，是还可以继续发起退款的，所以只能在退款流水这里做标记，所以才有20，30
                                strSql.Append(" UPDATE M_ReBackTrade SET ComfirmStatus=@ComfirmStatus ,ComfirmTime=@ComfirmTime  WHERE ReBackTradeNo=@_ReBackTradeNo ");
                                if (connection.Execute(strSql.ToString(), new
                                {
                                    _ReBackTradeNo = tradeNo,
                                    ComfirmTime = nowTime,
                                    ComfirmStatus = (payStatus == "10") ? 20 : 30,//20退款流水返回成功//30退款流水返回失败
                                }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                if (payStatus == "10")
                                {
                                    strSql.Clear();
                                    strSql.Append("  UPDATE M_OrderInfo SET OrderStatus=@OrderStatus,ShippingStatus=@ShippingStatus,Version=Version+1,LogInfo=CONCAT(LogInfo,@LogInfo)  WHERE OrderID=@_OrderID AND OrderStatus=6 ");
                                    //更改订单状态信息
                                    if (connection.Execute(strSql.ToString(), new
                                    {
                                        _OrderID = orderID,
                                        OrderStatus = (reBackType == 10) ? 4 : 10,//订单状态改变，如果没有
                                        ShippingStatus=5,
                                        LogInfo = nowTime + "获取XF系统返回的退款状态"
                                    }, transaction: transaction, commandTimeout: commandTimeout) != 1)
                                    {
                                        transaction.Rollback();
                                        return false;
                                    }
                                }
                                transaction.Commit();
                                return true;
                            }else
                            {
                                msg = tradeNo+ "退款流水的资金对账不相等";
                            }
                        }else
                        {
                            msg = tradeNo+ "XF系统返回的同步接口数据找不到对应的流水信息";
                        }
                        return false;
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        #endregion

    }
}