using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.DirectorioImpugnaciones
{
    /// <summary>
    /// Clases con métodos del módulo DirectorioImpugnaciones
    /// </summary>
    public class DirectorioImpugnacionesAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DirectorioImpugnacionesAppServicio));

        #region Métodos Tabla WB_TIPOIMPUGNACION

        /// <summary>
        /// Inserta un registro de la tabla WB_TIPOIMPUGNACION
        /// </summary>
        public void SaveWbTipoimpugnacion(WbTipoimpugnacionDTO entity)
        {
            try
            {
                FactorySic.GetWbTipoimpugnacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_TIPOIMPUGNACION
        /// </summary>
        public void UpdateWbTipoimpugnacion(WbTipoimpugnacionDTO entity)
        {
            try
            {
                FactorySic.GetWbTipoimpugnacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_TIPOIMPUGNACION
        /// </summary>
        public void DeleteWbTipoimpugnacion(int timpgcodi)
        {
            try
            {
                FactorySic.GetWbTipoimpugnacionRepository().Delete(timpgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_TIPOIMPUGNACION
        /// </summary>
        public WbTipoimpugnacionDTO GetByIdWbTipoimpugnacion(int timpgcodi)
        {
            return FactorySic.GetWbTipoimpugnacionRepository().GetById(timpgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_TIPOIMPUGNACION
        /// </summary>
        public List<WbTipoimpugnacionDTO> ListWbTipoimpugnacions()
        {
            return FactorySic.GetWbTipoimpugnacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbTipoimpugnacion
        /// </summary>
        public List<WbTipoimpugnacionDTO> GetByCriteriaWbTipoimpugnacions(string nombreTipoImpugnacion)
        {
            return FactorySic.GetWbTipoimpugnacionRepository().GetByCriteria(nombreTipoImpugnacion);
        }

        #endregion

        #region Métodos Tabla WB_EVENTOAGENDA

        /// <summary>
        /// Inserta un registro de la tabla WB_EVENTOAGENDA
        /// </summary>
        public int SaveWbEventoagenda(WbEventoagendaDTO entity)
        {
            try
            {
                return FactorySic.GetWbEventoagendaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_EVENTOAGENDA
        /// </summary>
        public void UpdateWbEventoagenda(WbEventoagendaDTO entity)
        {
            try
            {
                FactorySic.GetWbEventoagendaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_EVENTOAGENDA
        /// </summary>
        public void DeleteWbEventoagenda(int eveagcodi)
        {
            try
            {
                FactorySic.GetWbEventoagendaRepository().Delete(eveagcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_EVENTOAGENDA
        /// </summary>
        public WbEventoagendaDTO GetByIdWbEventoagenda(int eveagcodi)
        {
            return FactorySic.GetWbEventoagendaRepository().GetById(eveagcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_EVENTOAGENDA
        /// </summary>
        public List<WbEventoagendaDTO> ListWbEventoagendas(int tipoEvento, string anio)
        {
            return FactorySic.GetWbEventoagendaRepository().List(tipoEvento, anio);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbEventoagenda
        /// </summary>
        public List<WbEventoagendaDTO> GetByCriteriaWbEventoagendas(int tipoEvento, DateTime fecha)
        {
            return FactorySic.GetWbEventoagendaRepository().GetByCriteria(tipoEvento, fecha);
        }

        #endregion

        #region Métodos Tabla WB_IMPUGNACION

        /// <summary>
        /// Inserta un registro de la tabla WB_IMPUGNACION
        /// </summary>
        public int SaveWbImpugnacion(WbImpugnacionDTO entity)
        {
            try
            {
                return FactorySic.GetWbImpugnacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla WB_IMPUGNACION
        /// </summary>
        public void UpdateWbImpugnacion(WbImpugnacionDTO entity)
        {
            try
            {
                FactorySic.GetWbImpugnacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla WB_IMPUGNACION
        /// </summary>
        public void DeleteWbImpugnacion(int impgcodi)
        {
            try
            {
                FactorySic.GetWbImpugnacionRepository().Delete(impgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla WB_IMPUGNACION
        /// </summary>
        public WbImpugnacionDTO GetByIdWbImpugnacion(int impgcodi)
        {
            return FactorySic.GetWbImpugnacionRepository().GetById(impgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla WB_IMPUGNACION
        /// </summary>
        public List<WbImpugnacionDTO> ListWbImpugnacions()
        {
            return FactorySic.GetWbImpugnacionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla WbImpugnacion
        /// </summary>
        public List<WbImpugnacionDTO> GetByCriteriaWbImpugnacions(int tipoImpugnacion, DateTime fecha)
        {
            return FactorySic.GetWbImpugnacionRepository().GetByCriteria(tipoImpugnacion, fecha);
        }

        #endregion
    }
}
