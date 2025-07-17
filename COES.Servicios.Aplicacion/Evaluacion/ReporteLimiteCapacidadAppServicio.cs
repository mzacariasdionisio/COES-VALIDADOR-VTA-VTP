using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using log4net;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Evaluacion
{
    /// <summary>
    /// Clases con métodos del módulo Evaluación
    /// </summary>
    public class ReporteLimiteCapacidadAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReporteLimiteCapacidadAppServicio));


        #region GESPROTEC
        /// <summary>
        /// Permite registrar el Reporte
        /// </summary>
        /// <returns></returns>
        public string RegistrarReporteLimiteCapacidad(EprEquipoDTO equipo)
        {
            return FactorySic.GetEprEquipoRepository().RegistrarTransformador(equipo);
        }

        /// <summary>
        /// Permite obtener la lista inicial del Reporte
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaReporteLimiteCapacidad(int revision, string descripcion)
        {
            return FactorySic.GetEprEquipoRepository().ListaReporteLimiteCapacidad(revision, descripcion);
        }

        /// <summary>
        /// Permite obtener el detalle del Reporte
        /// </summary>
        /// <returns></returns>
        public EprEquipoDTO ObtenerReporteLimiteCapacidadPorId(int id)
        {
            return FactorySic.GetEprEquipoRepository().ObtenerReporteLimiteCapacidadPorId(id);
        }

        /// <summary>
        /// Permite guardar el reporte de limite de capacidad
        /// </summary>
        /// <returns></returns>
        public int GuardarReporteLimiteCapacidad(EprEquipoDTO entity)
        {
            return FactorySic.GetEprEquipoRepository().GuardarReporteLimiteCapacidad(entity);
        }

        /// <summary>
        /// Permite actualizar el reporte de limite de capacidad
        /// </summary>
        /// <returns></returns>
        public void ActualizarReporteLimiteCapacidad(EprEquipoDTO entity)
        {
            FactorySic.GetEprEquipoRepository().ActualizarReporteLimiteCapacidad(entity);
        }

        /// <summary>
        /// Permite eliminar registropara el formulario el reporte de limite de capacidad
        /// </summary>
        /// <returns></returns>
        public void EliminarReporteLimiteCapacidad(EprEquipoDTO entity)
        {
            FactorySic.GetEprEquipoRepository().EliminarReporteLimiteCapacidad(entity);
        }

        /// <summary>
        /// Permite eliminar o actualizar el archivo de reporte de limite de capacidad.
        /// </summary>
        /// <returns></returns>
        public void AgregarEliminarArchivoReporteLimiteCapacidad(EprEquipoDTO entity)
        {
            FactorySic.GetEprEquipoRepository().AgregarEliminarArchivoReporteLimiteCapacidad(entity);
        }

        /// <summary>
        /// Permite obtener la ultima fecha del registro de reporte para validacion.
        /// </summary>
        /// <returns></returns>
        public string ObtenerFechaReportePorId(int id)
        {
            return FactorySic.GetEprEquipoRepository().ObtenerFechaReportePorId(id);
        }
        #endregion
    }
}
