using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class ModeloEnvioAppServicio: AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ModeloEnvioAppServicio));

        #region Métodos Tabla TRN_MODELO_ENVIO

        /// <summary>
        /// Inserta un registro de la tabla TRN_MODELO_ENVIO
        /// </summary>
        public int SaveTrnModeloEnvio(TrnModeloEnvioDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetTrnModeloEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_MODELO_ENVIO
        /// </summary>
        public void UpdateTrnModeloEnvio(TrnModeloEnvioDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnModeloEnvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_MODELO_ENVIO
        /// </summary>
        public void DeleteTrnModeloEnvio(int modenvcodi)
        {
            try
            {
                FactoryTransferencia.GetTrnModeloEnvioRepository().Delete(modenvcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_MODELO_ENVIO
        /// </summary>
        public TrnModeloEnvioDTO GetByIdTrnModeloEnvio(int modenvcodi)
        {
            return FactoryTransferencia.GetTrnModeloEnvioRepository().GetById(modenvcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_MODELO_ENVIO
        /// </summary>
        public List<TrnModeloEnvioDTO> ListTrnModeloEnvios()
        {
            return FactoryTransferencia.GetTrnModeloEnvioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrnModeloEnvio
        /// </summary>
        public List<TrnModeloEnvioDTO> GetByCriteriaTrnModeloEnvios(int empresa, int periodo, int version)
        {
            return FactoryTransferencia.GetTrnModeloEnvioRepository().GetByCriteria( empresa,  periodo,  version);
        }

        #endregion
    }
}
