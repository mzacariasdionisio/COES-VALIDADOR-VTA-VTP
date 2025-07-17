using COES.Service.YupanaContinuo.Helper;
using COES.Service.YupanaContinuo.ServiceReferenceYupanaContinuo;
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

namespace COES.Service.YupanaContinuo
{
    public partial class ServicioYupanaContinuo : ServiceBase
    {
        Timer tiempo;
        public ServicioYupanaContinuo()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.tiempo = new Timer(1000 * 60 * 60);
            this.tiempo.Elapsed += new ElapsedEventHandler(tiempo_Elapsed);
            this.tiempo.Enabled = true;
            this.tiempo.AutoReset = true;
            this.tiempo.Start();
        }

        /// <summary>
        /// Se ejecuta cada vez que cumple los 60 minutos (cada hora)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tiempo_Elapsed(object sender, ElapsedEventArgs e)
        {
            using (YupanaContinuoServicioClient cliente = new YupanaContinuoServicioClient())
            {
                EventLogger logger = new EventLogger();
                try
                {
                    cliente.SimularArbolYupanaContinuo();
                    logger.Info("Proceso ejecutado correctamente");
                }
                catch (Exception ex)
                {
                    logger.Error("Error", ex);
                }
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
