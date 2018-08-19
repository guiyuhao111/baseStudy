using DSMTMALL.Core.Common;
using System;
using System.Configuration;
using System.ServiceProcess;

namespace MallHandler
{
    public partial class MainService : ServiceBase
    {
        private System.Timers.Timer timer = new System.Timers.Timer();
        private System.Timers.Timer timerGoodsSync = new System.Timers.Timer();
        public MainService()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                //开启数据同步服务
                timer.Interval = 3000;//30秒同步一次
                timer.Elapsed += DataSync;//委托
                timer.AutoReset = true;
                timer.Enabled = true;
                //第二个定时器库存系统同步订单释放
                timerGoodsSync.Interval = 3600;//一个小时执行一次
                timerGoodsSync.Elapsed += DataGoodsSync;
                timerGoodsSync.AutoReset = true;
                timerGoodsSync.Enabled = true;
                FileHelper.logger.Info("同步程序启动");
            }
            catch {
                FileHelper.logger.Info("同步程序启动失败");
            }
        }
        protected override void OnStop()
        {
            try
            {
                //关闭数据同步服务
                timer.Elapsed -= DataSync;
                timer.Enabled = false;
                timer.Close();
                timer.Dispose();
                //关闭第二个定时器
                timerGoodsSync.Elapsed -= DataGoodsSync;
                timerGoodsSync.Enabled = false;
                timerGoodsSync.Close();
                timerGoodsSync.Dispose();
                FileHelper.logger.Info("同步程序关闭成功");
            }
            catch
            {
                FileHelper.logger.Info("同步程序关闭失败");
            }
        }

        /// <summary>
        /// 数据同步事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void DataSync(object source, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            try
            {
                WMSAPIHandler.WMSNotifyAdd();//执行订单推送
                WMSAPIHandler.WMSNotifyQuery();//执行订单校对
                //XFAPIHandler.GetOrderInfoPaymentStatusSync();//执行XF系统支付同步扣款状态查询
                //XFAPIHandler.GetReBackPayAccStatusSync();//执行XF系统支付退款状态查询
            }
            catch (Exception ex)
            {
                FileHelper.logger.Error(ex.Message);
            }
            timer.Enabled = true;
        }
        private void DataGoodsSync(object source, System.Timers.ElapsedEventArgs e)
        {
            timerGoodsSync.Enabled = false;
            try
            {
                //释放库存 
                //MyPublicHelper.ReleaseInventory();
                //自动确认收货
                //MyPublicHelper.ComfirmTimePassOrder();
                //同步库存
                WMSAPIHandler.WMSSyncStockOut();
                WMSAPIHandler.WMSNotifyQueryStockOut();//同步订单
                FileHelper.logger.Info("执行了库存同步,物流同步与释放库存任务");
            }
            catch (Exception ex)
            {
                FileHelper.logger.Error(ex.Message);
            }
            timerGoodsSync.Enabled = true;
        }
    }
}
