
namespace COES.Service.Tawa
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstallerTawa = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerTawa = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerTawa
            // 
            this.serviceProcessInstallerTawa.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstallerTawa.Password = null;
            this.serviceProcessInstallerTawa.Username = null;
            // 
            // serviceInstallerTawa
            // 
            this.serviceInstallerTawa.Description = "Proceso para ejecutar Tawa";
            this.serviceInstallerTawa.DisplayName = "Ejecución automática Tawa";
            this.serviceInstallerTawa.ServiceName = "EjecucionAutomaticaTawa";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerTawa,
            this.serviceInstallerTawa});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerTawa;
        private System.ServiceProcess.ServiceInstaller serviceInstallerTawa;
    }
}