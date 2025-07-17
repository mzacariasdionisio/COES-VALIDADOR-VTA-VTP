using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using COES.WebService.YupanaContinuo.Contratos;
using log4net;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace COES.WebService.YupanaContinuo.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class YupanaContinuoServicio : IYupanaContinuoServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(YupanaContinuoServicio));

        public YupanaContinuoServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Funcion que hace llamado a la simulación automática cada hora
        /// </summary>
        public void EjecutarYupanaContinuoAutomatico(string fecha, int hora)
        {
            try
            {
                COES.Servicios.Aplicacion.YupanaContinuo.YupanaContinuoAppServicio yupanaServicio = new COES.Servicios.Aplicacion.YupanaContinuo.YupanaContinuoAppServicio();

                yupanaServicio.EjecutarSimulacionAutomaticaXFechaYHora(fecha, hora);
            }
            catch (Exception ex)
            {
                log.Error("EjecutarYupanaContinuoAutomatico", ex);
            }
        }

        /// <summary>
        /// Funcion que hace llamado a la simulación manual
        /// </summary>
        public void EjecutarYupanaContinuoManual(string fecha, int hora, string usuario)
        {
            try
            {
                COES.Servicios.Aplicacion.YupanaContinuo.YupanaContinuoAppServicio yupanaServicio = new COES.Servicios.Aplicacion.YupanaContinuo.YupanaContinuoAppServicio();

                yupanaServicio.EjecutarSimulacionManualXFechaYHora(fecha, hora, usuario);
            }
            catch (Exception ex)
            {
                log.Error("EjecutarYupanaContinuoManual", ex);
            }
        }

        /// <summary>
        /// Verificar estado del último arbol en ejecucion
        /// </summary>
        public void VerificarEstadoYupanaContinuo()
        {
            try
            {
                COES.Servicios.Aplicacion.YupanaContinuo.YupanaContinuoAppServicio yupanaServicio = new COES.Servicios.Aplicacion.YupanaContinuo.YupanaContinuoAppServicio();

                yupanaServicio.VerificarPlazosEjecucion();
            }
            catch (Exception ex)
            {
                log.Error("VerificarEstadoYupanaContinuo", ex);
            }
        }

        /// <summary>
        /// Terminar ejecución de los procesos gams ejecutando en el servidor
        /// </summary>
        public void TerminarEjecucionGams()
        {
            try
            {
                COES.Servicios.Aplicacion.YupanaContinuo.YupanaContinuoAppServicio yupanaServicio = new COES.Servicios.Aplicacion.YupanaContinuo.YupanaContinuoAppServicio();

                yupanaServicio.FinalizarEjecucionArbol();
            }
            catch (Exception ex)
            {
                log.Error("TerminarEjecucionGams", ex);
            }
        }

        public void SimularArbolYupanaContinuo()
        {
            log.Info(ConstantesYupanaContinuo.PrcsmetodoSimularArbolYupanaContinuo);
            try
            {
                Task.Factory.StartNew(() => CallWebServiceYupanaContinuo());
            }
            catch (Exception ex)
            {
                log.Error("PrcsmetodoSimularArbolYupanaContinuo", ex);
            }
        }

        /// <summary>
        /// Ejecución del proceso yupana continuo
        /// </summary>
        private void CallWebServiceYupanaContinuo()
        {
            try
            {
                string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                int hora = DateTime.Now.Hour;

                //ProcesoYupanaContinuo.YupanaContinuoServicioClient cliente = new ProcesoYupanaContinuo.YupanaContinuoServicioClient();
                this.EjecutarYupanaContinuoAutomatico(fecha, hora);
            }
            catch (Exception ex)
            {
                log.Error("CallWebServiceYupanaContinuo", ex);
            }
        }
    }
}
