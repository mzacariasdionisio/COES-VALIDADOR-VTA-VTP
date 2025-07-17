
namespace COES.Service.HTrabajo
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
            this.serviceProcessInstallerHTrabajo = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerHTrabajo = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerHTrabajo
            // 
            this.serviceProcessInstallerHTrabajo.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstallerHTrabajo.Password = null;
            this.serviceProcessInstallerHTrabajo.Username = null;
            // 
            // serviceInstallerHTrabajo
            // 
            this.serviceInstallerHTrabajo.Description = "Proceso para ejecutar HTrabajo";
            this.serviceInstallerHTrabajo.DisplayName = "Ejecución automática HTrabajo";
            this.serviceInstallerHTrabajo.ServiceName = "EjecucionAutomaticaHTrabajo";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerHTrabajo,
            this.serviceInstallerHTrabajo});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerHTrabajo;
        private System.ServiceProcess.ServiceInstaller serviceInstallerHTrabajo;
    }
}