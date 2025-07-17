using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Distribuidos.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace COES.Servicios.Distribuidos.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class CostoOportunidadServicio : ICostoOportunidadServicio
    {
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

        public int EjecutarProcesoDiario(DateTime fecha, string tipo, string usuario)
        {
            bool salida = this.costoOpServicio.EjecutarProcesoDiario(fecha, tipo, usuario);
            return (salida) ? 1 : 0;
        }

    }
}