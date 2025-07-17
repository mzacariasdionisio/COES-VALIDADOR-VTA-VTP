using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.InformesOsinergmin
{
    /// <summary>
    /// Clases con métodos del módulo InformesOsinergmin
    /// </summary>
    public class CostoRealMarginalAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CostoRealMarginalAppServicio));

        #region Métodos Tabla PSU_DESVCMG

        /// <summary>
        /// Inserta un registro de la tabla PSU_DESVCMG
        /// </summary>
        public void SavePsuDesvcmg(PsuDesvcmgDTO entity)
        {
            try
            {
                FactorySic.GetPsuDesvcmgRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PSU_DESVCMG
        /// </summary>
        public void UpdatePsuDesvcmg(PsuDesvcmgDTO entity)
        {
            try
            {
                FactorySic.GetPsuDesvcmgRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PSU_DESVCMG
        /// </summary>
        public void DeletePsuDesvcmg(DateTime desvfecha)
        {
            try
            {
                FactorySic.GetPsuDesvcmgRepository().Delete(desvfecha);
            }
            catch (Exception ex)
            {
                Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PSU_DESVCMG
        /// </summary>
        public PsuDesvcmgDTO GetByIdPsuDesvcmg(DateTime desvfecha)
        {
            return FactorySic.GetPsuDesvcmgRepository().GetById(desvfecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PSU_DESVCMG
        /// </summary>
        public List<PsuDesvcmgDTO> ListPsuDesvcmgs()
        {
            return FactorySic.GetPsuDesvcmgRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PsuDesvcmg
        /// </summary>
        public List<PsuDesvcmgDTO> GetByCriteriaPsuDesvcmgs()
        {
            return FactorySic.GetPsuDesvcmgRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla PSU_DESVCMGSNC

        /// <summary>
        /// Inserta un registro de la tabla PSU_DESVCMGSNC
        /// </summary>
        public void SavePsuDesvcmgsnc(PsuDesvcmgsncDTO entity)
        {
            try
            {
                FactorySic.GetPsuDesvcmgsncRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla PSU_DESVCMGSNC
        /// </summary>
        public void UpdatePsuDesvcmgsnc(PsuDesvcmgsncDTO entity)
        {
            try
            {
                FactorySic.GetPsuDesvcmgsncRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PSU_DESVCMGSNC
        /// </summary>
        public void DeletePsuDesvcmgsnc(DateTime desvfecha)
        {
            try
            {
                FactorySic.GetPsuDesvcmgsncRepository().Delete(desvfecha);
            }
            catch (Exception ex)
            {
                Logger.Error(Constantes.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PSU_DESVCMGSNC
        /// </summary>
        public PsuDesvcmgsncDTO GetByIdPsuDesvcmgsnc(DateTime desvfecha)
        {
            return FactorySic.GetPsuDesvcmgsncRepository().GetById(desvfecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla PSU_DESVCMGSNC
        /// </summary>
        public List<PsuDesvcmgsncDTO> ListPsuDesvcmgsncs()
        {
            return FactorySic.GetPsuDesvcmgsncRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla PsuDesvcmgsnc
        /// </summary>
        public List<PsuDesvcmgsncDTO> GetByCriteriaPsuDesvcmgsncs()
        {
            return FactorySic.GetPsuDesvcmgsncRepository().GetByCriteria();
        }

        #endregion

    }
}
