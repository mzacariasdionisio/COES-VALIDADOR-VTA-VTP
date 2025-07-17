using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.Eventos
{
    public class DetalleEventoAppServicio
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DetalleEventoAppServicio));

        #region Métodos Tabla EVE_SUBEVENTOS

        /// <summary>
        /// Inserta un registro de la tabla EVE_SUBEVENTOS
        /// </summary>
        public void SaveEveSubeventos(EveSubeventosDTO entity)
        {
            try
            {
                FactorySic.GetEveSubeventosRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_SUBEVENTOS
        /// </summary>
        public void UpdateEveSubeventos(EveSubeventosDTO entity)
        {
            try
            {
                FactorySic.GetEveSubeventosRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_SUBEVENTOS
        /// </summary>
        public void DeleteEveSubeventos(int evencodi, int equicodi, DateTime subevenini)
        {
            try
            {
                FactorySic.GetEveSubeventosRepository().Delete(evencodi, equicodi, subevenini);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_SUBEVENTOS
        /// </summary>
        public EveSubeventosDTO GetByIdEveSubeventos(int evencodi, int equicodi, DateTime subevenini)
        {
            return FactorySic.GetEveSubeventosRepository().GetById(evencodi, equicodi, subevenini);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_SUBEVENTOS
        /// </summary>
        public List<EveSubeventosDTO> ListEveSubeventoss()
        {
            return FactorySic.GetEveSubeventosRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveSubeventos
        /// </summary>
        public List<EveSubeventosDTO> GetByCriteriaEveSubeventoss(int idEvento)
        {
            return FactorySic.GetEveSubeventosRepository().GetByCriteria(idEvento);
        }

        #endregion

        #region Métodos Tabla EVE_INTERRUPCION

        /// <summary>
        /// Inserta un registro de la tabla EVE_INTERRUPCION
        /// </summary>
        public void SaveEveInterrupcion(EveInterrupcionDTO entity)
        {
            try
            {
                FactorySic.GetEveInterrupcionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_INTERRUPCION
        /// </summary>
        public void UpdateEveInterrupcion(EveInterrupcionDTO entity)
        {
            try
            {
                FactorySic.GetEveInterrupcionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_INTERRUPCION
        /// </summary>
        public void DeleteEveInterrupcion(int interrupcodi)
        {
            try
            {
                FactorySic.GetEveInterrupcionRepository().Delete(interrupcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_INTERRUPCION
        /// </summary>
        public EveInterrupcionDTO GetByIdEveInterrupcion(int interrupcodi)
        {
            return FactorySic.GetEveInterrupcionRepository().GetById(interrupcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_INTERRUPCION
        /// </summary>
        public List<EveInterrupcionDTO> ListEveInterrupcions()
        {
            return FactorySic.GetEveInterrupcionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveInterrupcion
        /// </summary>
        public List<EveInterrupcionDTO> GetByCriteriaEveInterrupcions(int idEvento)
        {
            return FactorySic.GetEveInterrupcionRepository().GetByCriteria(idEvento.ToString());
        }

        #endregion

        #region Métodos Tabla EVE_PTOINTERRUP

        /// <summary>
        /// Inserta un registro de la tabla EVE_PTOINTERRUP
        /// </summary>
        public void SaveEvePtointerrup(EvePtointerrupDTO entity)
        {
            try
            {
                FactorySic.GetEvePtointerrupRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_PTOINTERRUP
        /// </summary>
        public void UpdateEvePtointerrup(EvePtointerrupDTO entity)
        {
            try
            {
                FactorySic.GetEvePtointerrupRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_PTOINTERRUP
        /// </summary>
        public void DeleteEvePtointerrup(int ptointerrcodi)
        {
            try
            {
                FactorySic.GetEvePtointerrupRepository().Delete(ptointerrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_PTOINTERRUP
        /// </summary>
        public EvePtointerrupDTO GetByIdEvePtointerrup(int ptointerrcodi)
        {
            return FactorySic.GetEvePtointerrupRepository().GetById(ptointerrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_PTOINTERRUP
        /// </summary>
        public List<EvePtointerrupDTO> ListEvePtointerrups()
        {
            return FactorySic.GetEvePtointerrupRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EvePtointerrup
        /// </summary>
        public List<EvePtointerrupDTO> GetByCriteriaEvePtointerrups()
        {
            return FactorySic.GetEvePtointerrupRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EVE_PTOENTREGA

        /// <summary>
        /// Inserta un registro de la tabla EVE_PTOENTREGA
        /// </summary>
        public void SaveEvePtoentrega(EvePtoentregaDTO entity)
        {
            try
            {
                FactorySic.GetEvePtoentregaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_PTOENTREGA
        /// </summary>
        public void UpdateEvePtoentrega(EvePtoentregaDTO entity)
        {
            try
            {
                FactorySic.GetEvePtoentregaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_PTOENTREGA
        /// </summary>
        public void DeleteEvePtoentrega(int ptoentregacodi)
        {
            try
            {
                FactorySic.GetEvePtoentregaRepository().Delete(ptoentregacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_PTOENTREGA
        /// </summary>
        public EvePtoentregaDTO GetByIdEvePtoentrega(int ptoentregacodi)
        {
            return FactorySic.GetEvePtoentregaRepository().GetById(ptoentregacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_PTOENTREGA
        /// </summary>
        public List<EvePtoentregaDTO> ListEvePtoentregas()
        {
            return FactorySic.GetEvePtoentregaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EvePtoentrega
        /// </summary>
        public List<EvePtoentregaDTO> GetByCriteriaEvePtoentregas()
        {
            return FactorySic.GetEvePtoentregaRepository().GetByCriteria();
        }

        #endregion
    }
}
