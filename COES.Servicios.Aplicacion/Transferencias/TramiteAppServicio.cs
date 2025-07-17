using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;

namespace COES.Servicios.Aplicacion.Transferencias
{
   public class TramiteAppServicio: AppServicioBase
    {
        /// <summary>
        /// Inserta o actualiza un registro de tipo tramite
        /// </summary>
        /// <param name="entity">TramiteDTO</param>
        /// <returns>Código nuevo o actualizado de la tabla TRN_TRAMITE</returns>
        public int SaveOrUpdateTramite(TramiteDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TramCodi == 0)
                {
                    id = FactoryTransferencia.GetTramiteRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetTramiteRepository().Update(entity);
                    id = entity.TramCodi;
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
      
        /// <summary>
        /// Permite obtener un registro de la tabla TRN_TRAMITE en base al id
        /// </summary>
        /// <param name="iTrmCodi">Código de la tabla TRN_TRAMITE</param>
        /// <returns>TramiteDTO</returns>
        public TramiteDTO GetByIdTramite(int iTrmCodi)
        {
            return FactoryTransferencia.GetTramiteRepository().GetById(iTrmCodi);
        }

        /// <summary>
        /// Permite listar todas los registros de la tabla TRN_TRAMITE
        /// </summary>
        /// <returns>Lista de TramiteDTO</returns>
        public List<TramiteDTO> ListTramite()
        {
            return FactoryTransferencia.GetTramiteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas de tramites en base al fecha, nombre ,periodo ,version y corrigioinforme
        /// </summary>
        /// <param name="dTrmFecRespuesta">Fecha de respuesta de una consulta, dato opcional</param>
        /// <param name="sTrmCorrigeInforme">Flag que indica SI corrige o NO el informe</param>
        /// <param name="iPeriCodi">Código del Mes de Valorización</param>
        /// <param name="iTrmVersion">Versión del Mes de Valorización</param>
        /// <returns>Lista de TramiteDTO</returns>
        public List<TramiteDTO> BuscarTramite(DateTime? dTrmFecRespuesta, string sTrmCorrigeInforme, int iPeriCodi, int iTrmVersion)
        {
            return FactoryTransferencia.GetTramiteRepository().GetByCriteria(dTrmFecRespuesta, sTrmCorrigeInforme, iPeriCodi, iTrmVersion);
        }

        /// <summary>
        /// Permite eliminar un listado de registros de la tabla TRN_TRAMITE
        /// </summary>
        /// <param name="iPeriCodi">Código del Mes de Valorización</param>
        /// <param name="iTrmVersion">Versión del Mes de Valorización</param>
        /// <returns>Codigo del mes de valorización </returns>
        public int DeleteListaTramite(int iPeriCodi, int iTrmVersion)
        {
            int iResultado = iPeriCodi;
            try
            {
                FactoryTransferencia.GetTramiteRepository().DeleteListaTramite(iPeriCodi, iTrmVersion);
            }
            catch (Exception ex)
            {
                iResultado = 0;
                throw new Exception(ex.Message, ex);
            }
            return iResultado; 
        }
    }
}
