using COES.Service.Proceso.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace COES.Service.Proceso
{
    public partial class ServicioProceso : ServiceBase
    {
        Timer tiempo;

        public ServicioProceso()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inicio del servicio
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            this.tiempo = new Timer(1000 * 60 * 1);
            this.tiempo.Elapsed += new ElapsedEventHandler(tiempo_Elapsed);
            this.tiempo.Enabled = true;
            this.tiempo.AutoReset = true;
            this.tiempo.Start();
        }

        /// <summary>
        /// Se ejecuta cada vez que cumple los 5 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void tiempo_Elapsed(object sender, ElapsedEventArgs e)
        {
            EventLogger logger = new EventLogger();
            try
            {
                List<SiProcesoDTO> listProcesos = await (new ClienteHttp()).ObtenerProcesos();
                int nroNroHilosEnParaleo = int.Parse(ConfigurationManager.AppSettings[Constantes.NroHilosEnParalelo]);
                int nroGruposEjecucion = (listProcesos.Count % nroNroHilosEnParaleo == 0) ?
                    (int)(listProcesos.Count / nroNroHilosEnParaleo) : (int)(listProcesos.Count / nroNroHilosEnParaleo) + 1;

                for (int i = 0; i < nroGruposEjecucion; i++)
                {
                    List<SiProcesoDTO> listProcesoEjecutar = listProcesos.Skip(i * nroNroHilosEnParaleo).Take(nroNroHilosEnParaleo).ToList();

                    Task[] procesos = new Task[listProcesoEjecutar.Count];

                    for (int j = 0; j < listProcesoEjecutar.Count; j++)
                    {
                        procesos[j] = Task.Run(() => (new ClienteHttp()).EjecutarProceso(listProcesoEjecutar[j].Prcscodi, listProcesoEjecutar[j].Prcsmetodo, logger));
                    }

                    Task.WaitAll(procesos);
                }
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
