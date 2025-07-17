using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
   
   public  class CodigoRetiroSinContratoAppServicio : AppServicioBase
    {
       
       /// <summary>
       /// Permite grabar o actualizar un  CodigoRetiroSinContratoDTO en base a la entidad
       /// </summary>
       /// <param name="entity">Entidad de CodigoRetiroSinContratoDTO</param>
       /// <returns>Retorna el iCoReSCCodi nuevo o actualizado</returns>
       public int SaveOrUpdateCodigoRetiroSinContrato(CodigoRetiroSinContratoDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.CodRetiSinConCodi == 0)
                {
                    id = FactoryTransferencia.GetCodigoRetiroSinContratoRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetCodigoRetiroSinContratoRepository().Update(entity);
                    id = entity.CodRetiSinConCodi;
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

       /// <summary>
       /// Elimina un Codigo Retiro sin contrato en base al iCoReSCCodi
       /// </summary>
       /// <param name="iCoReSCCodi">Codigo de la tabla TRN_CODIGO_RETIRO_SINCONTRATO</param>
       /// <returns>Retorna el iCodRetCodi eliminado</returns>
       public int DeleteCodigoRetiroSinContrato(int iCoReSCCodi)
       {
            try
            {
                FactoryTransferencia.GetCodigoRetiroSinContratoRepository().Delete(iCoReSCCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iCoReSCCodi;
        }

       /// <summary>
       /// Permite obtener el Codigo Retiro Sin Contrato en base al iCoReSCCodi
       /// </summary>
       /// <param name="iCoReSCCodi">Codigo de la tabla TRN_CODIGO_RETIRO_SINCONTRATO</param>
       /// <returns>CodigoRetiroSinContratoDTO</returns>
       public CodigoRetiroSinContratoDTO GetByIdCodigoRetiroSinContrato(int iCoReSCCodi)
       {
           return FactoryTransferencia.GetCodigoRetiroSinContratoRepository().GetById(iCoReSCCodi);
       }

       /// <summary>
       /// Permite listar todas Codigo Retiro Sin Contrato
       /// </summary>
       /// <returns>Lista de CodigoRetiroSinContratoDTO</returns>
       public List<CodigoRetiroSinContratoDTO> ListCodigoRetiroSinContrato()
       {
            return FactoryTransferencia.GetCodigoRetiroSinContratoRepository().List();
       }

       /// <summary>
       /// Permite realizar búsquedas de Codigo de Retiro Sin Contrato en base a cualquiera de los parametros
       /// </summary>
       /// <param name="sCliEmprNomb">Nombre del Cliente</param> 
       /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
       /// <param name="dCoReSCFechaInicio">Fecha en que inicia el código de retiro solicitado, puede ser nulo</param>        
       /// <param name="dCoReSCFechaFin">Fecha en que concluye el código de retiro solicitado, puede ser nulo</param>        
       /// <param name="sCoReSCtEstado">Estado en que se encuentra el Código de Retiro solicitado</param>
       /// <returns>Lista de CodigoRetiroDTO</returns>
       public List<CodigoRetiroSinContratoDTO> BuscarCodigoRetiroSinContrato(string sCliEmprNomb, string sBarrBarraTransferencia, DateTime? dCoReSCFechaInicio, DateTime? dCoReSCFechaFin, string sCoReSCtEstado, string codretirosc, int NroPagina, int PageSizeCodigoRetiroSC)
       {
           return FactoryTransferencia.GetCodigoRetiroSinContratoRepository().GetByCriteria(sCliEmprNomb, sBarrBarraTransferencia, dCoReSCFechaInicio, dCoReSCFechaFin, sCoReSCtEstado, codretirosc, NroPagina, PageSizeCodigoRetiroSC);
       }

        /// <summary>
        /// Permite realizar búsquedas de Codigo de Retiro Sin Contrato en base a cualquiera de los parametros
        /// </summary>
        /// <param name="sCliEmprNomb">Nombre del Cliente</param>  
        /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
        /// <param name="dCoReSCFechaInicio">Fecha en que inicia el código de retiro solicitado, puede ser nulo</param>        
        /// <param name="dCoReSCFechaFin">Fecha en que concluye el código de retiro solicitado, puede ser nulo</param>        
        /// <param name="sCoReSCtEstado">Estado en que se encuentra el Código de Retiro solicitado</param>        
        /// <returns>Numero de filas de la consulta</returns>
       public int ObtenerNroFilasCodigoRetiroSinContrato(string sCliEmprNomb, string sBarrBarraTransferencia, DateTime? dCoReSCFechaInicio, DateTime? dCoReSCFechaFin, string sCoReSCtEstado,string codretirosc)
       {
           return FactoryTransferencia.GetCodigoRetiroSinContratoRepository().ObtenerNroRegistros(sCliEmprNomb, sBarrBarraTransferencia, dCoReSCFechaInicio, dCoReSCFechaFin, sCoReSCtEstado,codretirosc);
       }

       /// <summary>
       /// Permite obtener el Codigo Retiro Sin Contrato mediante su sCoReSCCodigo
       /// </summary>
       /// <param name="sCoReSCCodigo"></param>
       /// <returns>CodigoRetiroSinContratoDTO</returns>
       public CodigoRetiroSinContratoDTO BuscarCodigoRetiroSinContratoCodigo(string sCoReSCCodigo)
       {
           return FactoryTransferencia.GetCodigoRetiroSinContratoRepository().GetByCodigoRetiroSinContratoCodigo(sCoReSCCodigo);
       }
    }
}
