namespace MallHandler
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.MallHandler = new System.ServiceProcess.ServiceProcessInstaller();
            this.MallTEST = new System.ServiceProcess.ServiceInstaller();
            // 
            // MallHandler
            // 
            this.MallHandler.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.MallHandler.Password = null;
            this.MallHandler.Username = null;
            // 
            // MallTEST
            // 
            this.MallTEST.Description = "MALL测试服务";
            this.MallTEST.ServiceName = "MallService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.MallHandler,
            this.MallTEST});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller MallHandler;
        private System.ServiceProcess.ServiceInstaller MallTEST;
    }
}