using COES.Service.HTrabajo.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace COES.Service.HTrabajo
{
    public partial class ServicioHTrabajo : ServiceBase
    {

        Timer tiempo;
        public ServicioHTrabajo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inicio del servicio
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            this.tiempo = new Timer(1000 * 60 * 30);
            this.tiempo.Elapsed += new ElapsedEventHandler(tiempo_Elapsed);
            this.tiempo.Enabled = true;
            this.tiempo.AutoReset = true;
            this.tiempo.Start();
        }


        /// <summary>
        /// Se ejecuta cada vez que cumple los 30 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void tiempo_Elapsed(object sender, ElapsedEventArgs e)
        {
            EventLogger logger = new EventLogger();
            try
            {
                ClienteHttp cliente = new ClienteHttp();
                await cliente.EjecutarProceso();
                logger.Info("Proceso ejecutado correctamente");

            }
            catch (Exception ex)
            {
                logger.Error("Error", ex);
            }
        }

        /// <summary>
        /// Acción a realizar al finalizar
        /// </summary>
        protected override void OnStop()
        {
        }
    }
}
