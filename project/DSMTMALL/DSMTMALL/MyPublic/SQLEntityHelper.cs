using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.BLL;
using DSMTMALL.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSMTMALL.MyPublic
{
    public class SQLEntityHelper
    {
        /// <summary>
        /// 获取订单的商品价格总计--总应付款项
        /// </summary>
        /// <param name="orderSn"></param>
        /// <returns></returns>
        public decimal GetOrderInfoAmountByOrderSn(string orderSn, out decimal fareTempMoney,out DateTime orderCreateTime)
        {
            M_OrderInfo orderInfo = new MY_Bll(DBEnum.Slave).GetModel<M_OrderInfo>("OrderSn=@_OrderSn AND UserID=@_UserID", new { _OrderSn = orderSn, _UserID = WebLoginHelper.GetUserID() });
            fareTempMoney = 100;
            orderCreateTime = DateTime.Now;
            if (orderInfo != null)
            {
                fareTempMoney = orderInfo.ShippingFee;
                orderCreateTime = orderInfo.AddTime;
                return  orderInfo.OrderAmount;
            }
            return 0;
        }

        /// <summary>
        /// 获取统一下单ID的商品价格总计--总应付款项
        /// </summary>
        /// <param name="orderUnifySn"></param>
        /// <returns></returns>
        public decimal GetOrderInfoAmountByOrderUnifySn(string orderUnifySn,out decimal fareTempMoney,out DateTime orderCreateTime)
        {
            List<M_OrderInfo> orderInfo = new MY_Bll(DBEnum.Slave).GetModelList<M_OrderInfo>("OrderUnifySn = @_OrderUnifySn AND UserID = @_UserID", new { _OrderUnifySn=orderUnifySn, _UserID=WebLoginHelper.GetUserID()});
            decimal outAmount = 0;
            orderCreateTime = DateTime.Now;
            fareTempMoney = 100;
            if (orderInfo != null && orderInfo.Count > 0)
            {
                fareTempMoney = 0;
                foreach (var item in orderInfo)
                {
                    fareTempMoney += item.ShippingFee;
                    outAmount += item.OrderAmount;
                    orderCreateTime = item.AddTime;
                }
            }
            return outAmount;
        }

        /// <summary>
        /// 根据子订单编号获取该订单号的统一下单编号
        /// </summary>
        /// <param name="orderSn"></param>
        /// <returns></returns>
        public string GetOrderUnifySnByOrderSn(string orderSn)
        {
            List<M_OrderInfo> orderInfo = new MY_Bll(DBEnum.Slave).GetModelList<M_OrderInfo>(" OrderSn=@_OrderSn AND UserID=@_UserID ",new { _OrderSn = orderSn, _UserID= WebLoginHelper.GetUserID() });
            if (orderInfo != null && orderInfo.Count == 1)//获取到的子订单信息不为空，并且只有一条记录的情况下
            {
                return orderInfo[0].OrderUnifySn;
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据子订单编号获取该订单号订单信息
        /// </summary>
        /// <param name="orderSn"></param>
        /// <returns></returns>
        public M_OrderInfo GetOrderInfoByOrderSn(string orderSn)
        {
            List<M_OrderInfo> orderInfo = new MY_Bll(DBEnum.Slave).GetModelList<M_OrderInfo>(" OrderSn=@_OrderSn ", new { _OrderSn=orderSn }, "");
            if (orderInfo != null && orderInfo.Count == 1)//获取到的子订单信息不为空，并且只有一条记录的情况下
            {
                return orderInfo[0];
            }
            return null;
        }

        /// <summary>
        /// 根据统一订单编号获取该订单号的所有子订单号订单信息
        /// </summary>
        /// <param name="orderUnfiySn"></param>
        /// <returns></returns>
        public List<M_OrderInfo> GetOrderInfoListByOrderUnifySn(string orderUnfiySn)
        {
            return new MY_Bll(DBEnum.Slave).GetModelList<M_OrderInfo>("OrderUnifySn = @_OrderUnifySn",new { _OrderUnifySn = orderUnfiySn });
        }

    }
}