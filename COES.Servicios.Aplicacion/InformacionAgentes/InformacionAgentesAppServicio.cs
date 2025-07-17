using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.InformacionAgentes
{
    /// <summary>
    /// Clases con métodos del módulo InformacionAgentes
    /// </summary>
    public class InformacionAgentesAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InformacionAgentesAppServicio));

        #region Métodos Tabla INF_ARCHIVO_AGENTE

        /// <summary>
        /// Inserta un registro de la tabla INF_ARCHIVO_AGENTE
        /// </summary>
        public void SaveInfArchivoAgente(InfArchivoAgenteDTO entity)
        {
            try
            {
                FactorySic.GetInfArchivoAgenteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla INF_ARCHIVO_AGENTE
        /// </summary>
        public void UpdateInfArchivoAgente(InfArchivoAgenteDTO entity)
        {
            try
            {
                FactorySic.GetInfArchivoAgenteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla INF_ARCHIVO_AGENTE
        /// </summary>
        public void DeleteInfArchivoAgente(int archicodi)
        {
            try
            {
                FactorySic.GetInfArchivoAgenteRepository().Delete(archicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla INF_ARCHIVO_AGENTE
        /// </summary>
        public InfArchivoAgenteDTO GetByIdInfArchivoAgente(int archicodi)
        {
            return FactorySic.GetInfArchivoAgenteRepository().GetById(archicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla INF_ARCHIVO_AGENTE
        /// </summary>
        public List<InfArchivoAgenteDTO> ListInfArchivoAgentes()
        {
            return FactorySic.GetInfArchivoAgenteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla InfArchivoAgente
        /// </summary>
        public List<InfArchivoAgenteDTO> GetByCriteriaInfArchivoAgentes()
        {
            return FactorySic.GetInfArchivoAgenteRepository().GetByCriteria();
        }

        public List<InfArchivoAgenteDTO> ListarArchivosPorEmpresa(int iEmpresa, int nroPagina, int nroFilas)
        {
            iEmpresa = iEmpresa == 0 ? -2 : iEmpresa;
            return FactorySic.GetInfArchivoAgenteRepository().ListarArchivosPorEmpresa(iEmpresa, nroPagina, nroFilas);
        }

        public List<InfArchivoAgenteDTO> ListarArchivosPorFiltro(int iEmpresa, DateTime dtFechaInicio,
            DateTime dtFechaFin, int nroPagina, int nroFilas)
        {
            iEmpresa = iEmpresa == 0 ? -2 : iEmpresa;
            return FactorySic.GetInfArchivoAgenteRepository().ListarArchivosPorFiltro(iEmpresa, dtFechaInicio, dtFechaFin, nroPagina, nroFilas);
        }

        public int TotalListarArchivosPorEmpresa(int iEmpresa)
        {
            iEmpresa = iEmpresa == 0 ? -2 : iEmpresa;
            return FactorySic.GetInfArchivoAgenteRepository().TotalListarArchivosPorEmpresa(iEmpresa);
        }

        public int TotalListarArchivosPorFiltro(int iEmpresa, DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            iEmpresa = iEmpresa == 0 ? -2 : iEmpresa;
            return FactorySic.GetInfArchivoAgenteRepository().TotalListarArchivosPorFiltro(iEmpresa, dtFechaInicio, dtFechaFin);
        }

        public int CantidadArchivosPorNombre(string sNombreArchivo)
        {
            return FactorySic.GetInfArchivoAgenteRepository().CantidadArchivosPorNombre(sNombreArchivo);
        }

        #endregion

    }
}
