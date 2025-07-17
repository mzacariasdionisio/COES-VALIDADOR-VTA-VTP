using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.WebService.CostoOportunidad.Contratos;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace COES.WebService.CostoOportunidad.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class CostoOportunidadServicio : ICostoOportunidadServicio
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CostoOportunidadServicio));
        private readonly CostoOportunidadAppServicio costoOpServicio = new CostoOportunidadAppServicio();

        public void ImportarTodoSeñalesSP7(int tipoImportacion, DateTime? FechaDiario, int copercodi, int covercodi, string usuario, string tipo, out bool hayEjecucionEnCurso)
        {
            this.costoOpServicio.ImportarTodoSeñalesSP7(tipoImportacion, FechaDiario, copercodi, covercodi, usuario, tipo, out hayEjecucionEnCurso);
        }

        public int ProcesarCalculo(int idVersion, DateTime fechaInicio, DateTime fechaFin, string usuario, int option)
        {
            return this.costoOpServicio.ProcesarCalculo(idVersion, fechaInicio, fechaFin, usuario, option);
        }

        public int EjecutarReproceso(int idVersion, int indicador, DateTime fecInicio, DateTime fecFin,
            int indicadorDatos, string usuario, int option, int importarSP7)
        {
            return this.costoOpServicio.EjecutarReproceso(idVersion, indicador, fecInicio, fecFin,
             indicadorDatos, usuario, option, importarSP7);
        }

        public void ReprocesarCalculoTodos(DateTime fechaIni, DateTime fechaFin, string usuario)
        {
            this.costoOpServicio.ReprocesarCalculoTodos(fechaIni, fechaFin, usuario);
        }

        public void CalculoFactoresUtilizacion()
        {
            log.Info(ConstantesCostoOportunidad.ProcesoAutomaticoFactorUtilizacion);
            try
            {
                //CostoOportunidadServicioWeb.CostoOportunidadServicioClient client = new CostoOportunidadServicioWeb.CostoOportunidadServicioClient();

                int result = this.EjecutarProcesoDiario(DateTime.Now.AddDays(-3), ConstantesCostoOportunidad.TipoAutomatico,
                    ConstantesCostoOportunidad.UsuarioProcesoAutomatico);
            }
            catch (Exception ex)
            {
                log.Error("PrcsmetodoCalculoFactoresUtilizacion", ex);
            }
        }

        public int EjecutarProcesoDiario(DateTime fecha, string tipo, string usuario)
        {
            bool salida = this.costoOpServicio.EjecutarProcesoDiario(fecha, tipo, usuario);
            return (salida) ? 1 : 0;
        }
    }
}