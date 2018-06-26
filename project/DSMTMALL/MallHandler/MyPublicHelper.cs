using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;

namespace MallHandler
{
    public class MyPublicHelper
    {
        /// <summary>
        /// 释放订单付款超时的库存订单
        /// </summary>
        public static void  ReleaseInventory()
        {   //只有当订单状态为未确认，并且支付状态为未付款,并且未付款时间大于等于1小时才允许取消订单
            List<DSMTMALL.DB.Model.M_OrderInfo> orderInfoList = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_OrderInfo>(" PayStatus=0 AND OrderStatus= 0 AND NOW()-AddTime>3600 ",null);
            if (orderInfoList != null && orderInfoList.Count > 0)//订单列表不为空，订单集合长度大于等于0
            {
                foreach (var item in orderInfoList)//遍历每个订单
                {
                    if (orderInfoList != null && orderInfoList.Count>0)
                    {
                        if(!new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).CancelOrderSync(item.OrderID))
                        {
                            throw new Exception(DateTime.Now + "订单编号：" + item.OrderSn + "付款超时-释放库存失败");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发货后15天后未确认收货的订单自动确认收货
        /// </summary>
        public static void ComfirmTimePassOrder()
        {   //订单状态是5已发货物流状态是1付款中
            List<DSMTMALL.DB.Model.M_OrderInfo> orderInfoList = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_OrderInfo>(" OrderStatus=5 AND ShippingStatus=1 ",null);
            DateTime nowTime = DateTime.Now;
            if (orderInfoList!=null && orderInfoList.Count > 0)
            {
                foreach (var item in orderInfoList)
                {
                    if(item.ShippingTime.AddDays(15)<= nowTime)
                    {
                        if (new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_OrderInfo>(" OrderID=@_OrderID AND OrderStatus=5 AND ShippingStatus=1   ", new { _OrderID = item.OrderID, OrderStatus = 8, ShippingStatus = 2, ConfirmTime = nowTime }))
                        {
                            WeChatAPI.SendMsg("系统自动确认收货", "您的订单" + item.OrderSn + "由系统自动确认收货成功，感谢您的购买", item.OpenID);
                        }
                    }
                }
            }
        }
        
    }
}
