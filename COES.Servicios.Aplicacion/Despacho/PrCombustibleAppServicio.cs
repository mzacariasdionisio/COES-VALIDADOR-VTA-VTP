using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.Equipamiento
{
    /// <summary>
    /// Clases con métodos del módulo Despacho
    /// </summary>
    public class PrCombustibleAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PrCombustibleAppServicio));

        #region Métodos Tabla PR_COMBUSTIBLE

        /// <summary>
        /// Inserta un registro de la tabla PR_COMBUSTIBLE
        /// </summary>
        public void SavePrCombustible(CombustibleDTO entity)
        {
            try
            {
                FactorySic.GetPrCombustibleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_COMBUSTIBLE
        /// </summary>
        public void UpdatePrCombustible(CombustibleDTO entity)
        {
            try
            {
                FactorySic.GetPrCombustibleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PR_COMBUSTIBLE
        /// </summary>
        public void DeletePrCombustible(int combcodi)
        {
            try
            {
                FactorySic.GetPrCombustibleRepository().Delete(combcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_COMBUSTIBLE
        /// </summary>
        public CombustibleDTO GetByIdPrCombustible(int combcodi)
        {
            return FactorySic.GetPrCombustibleRepository().GetById(combcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PR_COMBUSTIBLE
        /// </summary>
        public List<CombustibleDTO> ListPrCombustibles()
        {
            return FactorySic.GetPrCombustibleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PrCombustible
        /// </summary>
        public List<CombustibleDTO> GetByCriteriaPrCombustibles()
        {
            return FactorySic.GetPrCombustibleRepository().GetByCriteria();
        }

        #endregion

    }
}
