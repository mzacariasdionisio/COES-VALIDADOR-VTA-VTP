
namespace COES.Service.YupanaContinuo
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
            this.serviceProcessInstallerYupana = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerYupana = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerYupana
            // 
            this.serviceProcessInstallerYupana.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstallerYupana.Password = null;
            this.serviceProcessInstallerYupana.Username = null;
            // 
            // serviceInstallerYupana
            // 
            this.serviceInstallerYupana.Description = "Proceso para ejecutar Yupana";
            this.serviceInstallerYupana.DisplayName = "Ejecución automática Yupana";
            this.serviceInstallerYupana.ServiceName = "EjecucionAutomaticaYupana";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerYupana,
            this.serviceInstallerYupana});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerYupana;
        private System.ServiceProcess.ServiceInstaller serviceInstallerYupana;
    }
}