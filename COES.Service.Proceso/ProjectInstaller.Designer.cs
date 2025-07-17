
namespace COES.Service.Proceso
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
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
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstallerProceso = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerProceso = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerProceso
            // 
            this.serviceProcessInstallerProceso.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstallerProceso.Password = null;
            this.serviceProcessInstallerProceso.Username = null;
            // 
            // serviceInstallerProceso
            // 
            this.serviceInstallerProceso.Description = "Proceso para ejecutar tareas automaticas";
            this.serviceInstallerProceso.DisplayName = "Ejecución de procesos automaticos - COES";
            this.serviceInstallerProceso.ServiceName = "ProcesosAutomaticosCOES";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerProceso,
            this.serviceInstallerProceso});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerProceso;
        private System.ServiceProcess.ServiceInstaller serviceInstallerProceso;
    }
}