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
    public class TransversalAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransversalAppServicio));


        #region GESPROTEC
        /// <summary>
        /// Devuelve el listado de transversal consultar equipo
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaConsultarEquipo(string codigoId)
        {
            return FactorySic.GetEprEquipoRepository().ListaTransversalConsultarEquipo(codigoId);
        }

        /// <summary>
        /// Devuelve la cabecera del transversal historial de actualizacion
        /// </summary>
        /// <returns></returns>
        public EprEquipoDTO ObtenerHistorialActualizacion(string codigoId)
        {
            return FactorySic.GetEprEquipoRepository().ObtenerTransversalHistorialActualizacion(codigoId);
        }

        /// <summary>
        /// Devuelve la lista transversal de actualizacion
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaActualizaciones(string codigoId)
        {
            return FactorySic.GetEprEquipoRepository().ListaTransversalActualizaciones(codigoId);
        }

        /// <summary>
        /// Devuelve la cabecera del transversal propiedades de actualizacion
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaPropiedadesActualizadas(string codigoId, string proyId)
        {
            return FactorySic.GetEprEquipoRepository().ListaTransversalPropiedadesActualizadas(codigoId, proyId);
        }

        /// <summary>
        /// Devuelve el detalle del equipo seleccionado
        /// </summary>
        /// <returns></returns>
        public EprEquipoDTO ObtenerEquipoPorId(string codigoId)
        {
            return FactorySic.GetEprEquipoRepository().ObtenerEquipoPorId(codigoId);
        }

        /// <summary>
        /// Devuelve la cabecera del transversal propiedades de actualizacion
        /// </summary>
        /// <returns></returns>
        public List<EprEquipoDTO> ListaInterruptorPorAreacodi(string id)
        {
            return FactorySic.GetEprEquipoRepository().ListaInterruptorPorAreacodi(id);
        }

        /// <summary>
        /// Devuelve el detalle del equipo seleccionado
        /// </summary>
        /// <returns></returns>
        public EprEquipoDTO ObtenerCabeceraEquipoPorId(int codigoId)
        {
            return FactorySic.GetEprEquipoRepository().ObtenerCabeceraEquipoPorId(codigoId);
        }

        /// <summary>
        /// Permite excluir el equipo del modulo de protecciones
        /// </summary>
        /// <returns></returns>
        public string ExcluirEquipoProtecciones(EprEquipoDTO equipo)
        {
            return FactorySic.GetEprEquipoRepository().ExcluirEquipoProtecciones(equipo);
        }

        #endregion
    }
}
