using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using COES.Servicios.Aplicacion.Helper;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class TipoContratoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TipoContratoAppServicio));
        /// Inserta o actualiza un registro de Tipo Contrato
        /// </summary>
        /// <param name="entity">TipoContratoDTO</param>
        /// <returns>Código de la tabla TRN_TIPO_CONTRATO</returns>
        public int SaveOrUpdateTipoContrato(TipoContratoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TipoContCodi == 0)
                {               
                    id = FactoryTransferencia.GetTipoContratoRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetTipoContratoRepository().Update(entity);
                    id = entity.TipoContCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_TIPO_CONTRATO
        /// </summary>
        /// <param name="iTipConCodi">Código de la tabla TRN_TIPO_CONTRATO</param>        
        /// <returns>Código del registro eliminado</returns>
        public int DeleteTipoContrato(int iTipConCodi)
        {
            try
            {
                FactoryTransferencia.GetTipoContratoRepository().Delete(iTipConCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTipConCodi;
        }

        /// <summary>
        /// Permite obtener el tipo de contrato en base al id
        /// </summary>
        /// <param name="iTipConCodi">Código de la tabla TRN_TIPO_CONTRATO</param>        
        /// <returns>TipoContratoDTO</returns>
        public TipoContratoDTO GetByIdTipoContrato(int iTipConCodi)
        {
            return FactoryTransferencia.GetTipoContratoRepository().GetById(iTipConCodi);
        }

        /// <summary>
        /// Permite listar todos los tipo de contrato
        /// </summary>
        /// <returns>Lista de TipoContratoDTO</returns>
        public List<TipoContratoDTO> ListTipoContrato()
        {
            return FactoryTransferencia.GetTipoContratoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas de tipos de contrato en base al nombre
        /// </summary>
        /// <param name="sTipConNombre">Nombre del tipo de contrato</param>
        /// <returns>Lista de TipoContratoDTO</returns>
        public List<TipoContratoDTO> BuscarTipoContrato(string sTipConNombre)
        {
            return FactoryTransferencia.GetTipoContratoRepository().GetByCriteria(sTipConNombre);
        }

        /// <summary>
        /// Permite realizar obtener el tipo de contrato por nombre
        /// </summary>
        /// <param name="sTipConNombre">Nombre del tipo de contrato</param>
        /// <returns>Lista de TipoContratoDTO</returns>
        public TipoContratoDTO GetByNombre(string sTipConNombre)
        {
            try
            {
                return FactoryTransferencia.GetTipoContratoRepository().GetByNombre(sTipConNombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            
        }
    }
}
