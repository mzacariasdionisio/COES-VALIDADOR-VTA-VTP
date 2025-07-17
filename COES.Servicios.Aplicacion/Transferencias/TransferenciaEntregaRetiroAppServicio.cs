using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class TransferenciaEntregaRetiroAppServicio : AppServicioBase
    {
        /// <summary>
        /// Graba o actualiza una entidad TransferenciaEntregaDTO
        /// </summary>
        /// <param name="entity">TransferenciaEntregaDTO</param>
        /// <returns>Código nuevo o actualizado de la tabla TRN_TRANS_ENTREGA</returns>
        public int SaveOrUpdateTransferenciaEntrega(TransferenciaEntregaDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TranEntrCodi == 0)
                {
                    id = FactoryTransferencia.GetTransferenciaEntregaRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetTransferenciaEntregaRepository().Update(entity);
                    id = entity.TranEntrCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un listado de registros de la tabla TRN_TRANS_ENTREGA en base al periodo de valorización y version de recalculo
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo de Valorización</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>     
        /// <returns>Retorna la Versión del Mes de valorización</returns>
        public int DeleteTransferenciaEntrega(int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaEntregaRepository().Delete(iPeriCodi, iTEntVersion, sCodigo);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTEntVersion;
        }

        /// <summary>
        /// Permite listar los codigo de entrega en base a empresa ,periodo y version
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>     
        /// <param name="iBarraCodi">Código de la Barra de Transferencia</param>     
        /// <returns>Lista de TransferenciaEntregaDTO</returns>
        public List<TransferenciaEntregaDTO> ListTransferenciaEntrega(int iEmprCodi, int iPeriCodi, int iTEntVersion, int iBarraCodi)
        {
            return FactoryTransferencia.GetTransferenciaEntregaRepository().List(iEmprCodi, iPeriCodi, iTEntVersion, iBarraCodi);
        }

        /// <summary>
        /// Permite obtener la información de Entrega en base a empresa, periodo, version y codigo
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>     
        /// <param name="sCodigo">Codigo de entrega</param>     
        /// <returns>Entidad TransferenciaEntregaDTO</returns>
        public TransferenciaEntregaDTO GetTransferenciaEntregaByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            return FactoryTransferencia.GetTransferenciaEntregaRepository().GetTransferenciaEntregaByCodigo(iEmprCodi, iPeriCodi, iTEntVersion, sCodigo);
        }

        /// <summary>
        /// Permite obtener la información de Entrega en base a empresa, periodo, version y codigo
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>
        /// <param name="trnenvcodi">Codigo de envio</param>
        /// <param name="sCodigo">Codigo de entrega</param>     
        /// <returns>Entidad TransferenciaEntregaDTO</returns>
        public TransferenciaEntregaDTO GetTransferenciaEntregaByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo)
        {
            return FactoryTransferencia.GetTransferenciaEntregaRepository().GetTransferenciaEntregaByCodigoEnvio(iEmprCodi, iPeriCodi, iTEntVersion, trnenvcodi, sCodigo);
        }

        /// <summary>
        /// Permite listar las Transferencias de entrega
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>
        /// <returns>Lista de TransferenciaEntregaDTO</returns>
        public List<TransferenciaEntregaDTO> ListByPeriodoVersionE(int iPeriCodi, int iTEntVersion)
        {
            return FactoryTransferencia.GetTransferenciaEntregaRepository().ListByPeriodoVersion(iPeriCodi, iTEntVersion);
        }

        /// <summary>
        /// Elimina un listado de TransferenciaEntrega
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>
        /// <returns>Retorna la Versión del Mes de valorización</returns>
        public int DeleteListaTransferenciaEntrega(int iPeriCodi, int iTEntVersion)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaEntregaRepository().DeleteListaTransferenciaEntrega(iPeriCodi, iTEntVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTEntVersion;
        }

        //TRANSFERENCIA ENTREGA DETALLE------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Graba o actualiza una entidad TransferenciaEntregaDetalleDTO
        /// </summary>
        /// <param name="entity">TransferenciaEntregaDetalleDTO</param>
        /// <returns>Código nuevo o actualizado de la tabla TRN_TRANS_ENTREGA_DETALLE</returns>
        public int SaveOrUpdateTransferenciaEntregaDetalle(TransferenciaEntregaDetalleDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TranEntrDetaCodi == 0)
                {
                    id = FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().Update(entity);
                    id = 1; //entity.TranEntrDetaCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite listar Transferencia Entrega Detalle en base a empresa, periodo
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <returns>TransferenciaEntregaDetalleDTO</returns>
        public List<TransferenciaEntregaDetalleDTO> ListTransferenciaEntregaDetalle(int iEmprCodi, int iPeriCodi)
        {
            return FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().List(iEmprCodi, iPeriCodi);
        }


        /// <summary>
        /// Permite realizar búsquedas de  Transferencia Entrega Detalle en base al emprcodi, pericodi, codientrcodi y version
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="sTEntCodigo">Código de Entrega</param>
        /// <param name="iTEntVersion">Versión de Recalculo</param>
        /// <returns>Lista de TransferenciaEntregaDetalleDTO</returns>
        public List<TransferenciaEntregaDetalleDTO> BuscarTransferenciaEntregaDetalle(int iEmprCodi, int iPeriCodi, string sTEntCodigo, int iTEntVersion)
        {
            return FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().GetByCriteria(iEmprCodi, iPeriCodi, sTEntCodigo, iTEntVersion);
        }

        /// <summary>
        /// Elimina un listado de registros de la tabla trn_trans_entrega_detalle
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recalculo</param>
        /// <returns>Código del recalculo</returns>
        public int DeleteTransferenciaEntregaDetalle(int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().Delete(iPeriCodi, iTEntVersion, sCodigo);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTEntVersion;
        }

        /// <summary>
        /// Elimina un listado de TransferenciaEntrega
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recalculo</param>
        /// <returns>Código del recalculo</returns>
        public int DeleteListaTransferenciaEntregaDetalle(int iPeriCodi, int iTEntVersion)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().DeleteListaTransferenciaEntregaDetalle(iPeriCodi, iTEntVersion);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTEntVersion;
        }

        /// <summary>
        /// Permite listar los codigo de entrega por periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recalculo</param>
        /// <returns>Lista de TransferenciaEntregaDetalleDTO</returns>
        public List<TransferenciaEntregaDetalleDTO> ListTransferenciaEntregaDetallePeriVer(int iPeriCodi, int iTEntVersion)
        {
            return FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().GetByPeriodoVersion(iPeriCodi, iTEntVersion);
        }

        /// <summary>
        /// Permite öbtener el balance de energiaActiva en base al periodo
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <returns>Lista de TransferenciaEntregaDetalleDTO</returns>
        public List<TransferenciaEntregaDetalleDTO> ListBalanceEnergiaActiva(int iPeriCodi, int version, Int32? barrcodi, Int32? emprcodi)
        {
            return FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().BalanceEnergiaActiva(iPeriCodi, version, barrcodi, emprcodi);
        }


        /// <summary>
        /// Permite listar las transferencia de retiro detalle
        /// </summary>
        /// <param name="iTEntCodi">Codigo de la tabla TRN_TRANS_ENTREGA</param>
        /// <param name="iTRetVersion">Codigo de la version</param>
        /// <returns>Lista de TransferenciaEntregaDetalleDTO</returns>
        public List<TransferenciaEntregaDetalleDTO> ListByTransferenciaEntrega(int iTEntCodi, int iTRetVersion)
        {
            return FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().ListByTransferenciaEntrega(iTEntCodi, iTRetVersion);
        }

        //TRANSFERENCIA RETIRO-------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Graba o actualiza una entidad TransferenciaRetiroDTO
        /// </summary>
        /// <param name="entity">TransferenciaRetiroDTO</param>       
        /// <returns>Código nuevo o actualizado de la tabla TRN_TRANS_RETIRO</returns>
        public int SaveOrUpdateTransferenciaRetiro(TransferenciaRetiroDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TranRetiCodi == 0)
                {
                    id = FactoryTransferencia.GetTransferenciaRetiroRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetTransferenciaRetiroRepository().Update(entity);
                    id = entity.TranRetiCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un listado de registros de la tabla TRN_TRANS_RETIRO en base al periodo de valorización y version de recalculo
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo de Valorización</param>
        /// <param name="iTRetVersion">Versión de Recálculo</param>     
        /// <returns>Retorna la Versión del Mes de valorización</returns>
        public int DeleteTransferenciaRetiro(int iPeriCodi, int iTRetVersion, string sCodigo)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroRepository().Delete(iPeriCodi, iTRetVersion, sCodigo);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTRetVersion;
        }


        /// <summary>
        /// Permite listar las Transferencias de retiro por empresa
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recálculo</param>     
        /// <param name="iBarraCodi">Código de la Barra de Transferencia</param>     
        /// <returns>Lista de TransferenciaRetiroDTO</returns>
        public List<TransferenciaRetiroDTO> ListTransferenciaRetiro(int iEmprCodi, int iPeriCodi, int iTRetVersion, int iBarraCodi)
        {
            return FactoryTransferencia.GetTransferenciaRetiroRepository().List(iEmprCodi, iPeriCodi, iTRetVersion, iBarraCodi);
        }

        /// <summary>
        /// Permite obtener la información de Retiro en base a empresa, periodo, version y codigo
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>     
        /// <param name="sCodigo">Codigo de entrega</param>     
        /// <returns>Entidad TransferenciaRetiroDTO</returns>
        public TransferenciaRetiroDTO GetTransferenciaRetiroByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            return FactoryTransferencia.GetTransferenciaRetiroRepository().GetTransferenciaRetiroByCodigo(iEmprCodi, iPeriCodi, iTEntVersion, sCodigo);
        }

        /// <summary>
        /// Permite obtener la información de Retiro en base a empresa, periodo, version y codigo
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>     
        /// <param name="trnenvcodi">Codigo de envio</param>     
        /// <param name="sCodigo">Codigo de retiro</param>     
        /// <returns>Entidad TransferenciaRetiroDTO</returns>
        public TransferenciaRetiroDTO GetTransferenciaRetiroByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo)
        {
            return FactoryTransferencia.GetTransferenciaRetiroRepository().GetTransferenciaRetiroByCodigoEnvio(iEmprCodi, iPeriCodi, iTEntVersion, trnenvcodi, sCodigo);
        }

        /// <summary>
        /// Permite listar las Transferencias de retiro
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recálculo</param>     
        /// <returns>Lista de TransferenciaRetiroDTO</returns>
        public List<TransferenciaRetiroDTO> ListByPeriodoVersionR(int iPeriCodi, int iTRetVersion)
        {
            return FactoryTransferencia.GetTransferenciaRetiroRepository().ListByPeriodoVersion(iPeriCodi, iTRetVersion);
        }

        /// <summary>
        /// Permite listar las Transferencias de retiro de un dia
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recálculo</param>     
        /// <returns>Lista de TransferenciaRetiroDTO</returns>
        public List<TransferenciaRetiroDTO> ListByPeriodoVersionREmpresa(int iPeriCodi, int iTRetVersion, int iEmprCodi)
        {
            return FactoryTransferencia.GetTransferenciaRetiroRepository().ListByPeriodoVersionEmpresa(iPeriCodi, iTRetVersion, iEmprCodi);
        }

        /// <summary>
        /// Elimina un listado de TransferenciaRetiro
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recálculo</param>     
        /// <returns>Retorna la Versión del Mes de valorización</returns>
        public int DeleteListaTransferenciaRetiro(int iPeriCodi, int iTRetVersion)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroRepository().DeleteListaTransferenciaRetiro(iPeriCodi, iTRetVersion);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTRetVersion;
        }

        /// <summary>
        /// Elimina un listado de TransferenciaRetiro por empresa
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recálculo</param>     
        /// <returns>Retorna la Versión del Mes de valorización</returns>
        public int DeleteListaTransferenciaRetiroEmpresa(int iPeriCodi, int iTRetVersion, int iEmprCodi)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroRepository().DeleteListaTransferenciaRetiroEmpresa(iPeriCodi, iTRetVersion, iEmprCodi);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTRetVersion;
        }

        //TRANSFERENCIA RETIRO  DETALLE

        /// <summary>
        /// Graba o actualiza una entidad TransferenciaRetiroDetalleDTO
        /// </summary>
        /// <param name="entity">TransferenciaRetiroDetalleDTO</param>
        /// <returns>Código nuevo o actualizado de la tabla TRN_TRANS_RETIRO_DETALLE</returns>
        public int SaveOrUpdateTransferenciaRetiroDetalle(TransferenciaRetiroDetalleDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TranRetiDetaCodi == 0)
                {
                    id = FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().Update(entity);
                    id = 1; // entity.TranRetiDetaCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite listar los codigo de retiro detalle en base a la codigo de la empresa ,periodo
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <returns>TransferenciaRetiroDetalleDTO</returns>
        public List<TransferenciaRetiroDetalleDTO> ListTransferenciaRetiroDetalle(int iEmprCodi, int iPeriCodi)
        {
            return FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().List(iEmprCodi, iPeriCodi);
        }

        /// <summary>
        /// Permite realizar búsquedas de codigo retiro detalle en base al codigo de periodo , empresa,codigo de retiro y version
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="sTRetCodigo">Código de Retiro solicitado</param>
        /// <param name="iTRetVersion">Versión de Recalculo</param>
        /// <returns>Lista de TransferenciaRetiroDetalleDTO</returns>
        public List<TransferenciaRetiroDetalleDTO> BuscarTransferenciaRetiroDetalle(int iEmprCodi, int iPeriCodi, string sTRetCodigo, int iTRetVersion)
        {
            return FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().GetByCriteria(iEmprCodi, iPeriCodi, sTRetCodigo, iTRetVersion);
        }

        /// <summary>
        /// Elimina la lista de TransferenciaRetiroDetalle
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recalculo</param>
        /// <returns>Código del recalculo</returns>
        public int DeleteTransferenciaRetiroDetalle(int iPeriCodi, int iTRetVersion, string sCodigo)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().Delete(iPeriCodi, iTRetVersion, sCodigo);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTRetVersion;
        }

        /// <summary>
        /// Permite listar los codigo de retiro detalle por periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recalculo</param>
        /// <returns>Lista de TransferenciaRetiroDetalleDTO</returns>
        public List<TransferenciaRetiroDetalleDTO> ListTransferenciaRetiroDetallePeriVer(int iPeriCodi, int iTRetVersion)
        {
            return FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().GetByPeriodoVersion(iPeriCodi, iTRetVersion);
        }

        /// <summary>
        /// Permite listar las transferencia de retiro detalle en base al id
        /// </summary>
        /// <param name="iTRetCodi">Codigo de la tabla TRN_TRANS_RETIRO</param>
        /// <param name="iTRetVersion">Codigo de la version TRN_TRANS_RETIRO</param>
        /// <returns>Lista de TransferenciaRetiroDetalleDTO</returns>
        public List<TransferenciaRetiroDetalleDTO> ListByTransferenciaRetiro(int iTRetCodi, int iTRetVersion)
        {
            return FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().ListByTransferenciaRetiro(iTRetCodi, iTRetVersion);
        }

        /// <summary>
        /// Elimina la lista de TransferenciaRetiroDetalle
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recalculo</param>
        /// <returns>Código del recalculo</returns> 
        public int DeleteListaTransferenciaRetiroDetalle(int iPeriCodi, int iTRetVersion)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().DeleteListaTransferenciaRetiroDetalle(iPeriCodi, iTRetVersion);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTRetVersion;
        }

        /// <summary>
        /// Elimina la lista de TransferenciaRetiroDetalle de una empresa
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTRetVersion">Versión de Recalculo</param>
        /// <returns>Código del recalculo</returns> 
        public int DeleteListaTransferenciaRetiroDetalleEmpresa(int iPeriCodi, int iTRetVersion, int iEmprCodi)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().DeleteListaTransferenciaRetiroDetalleEmpresa(iPeriCodi, iTRetVersion, iEmprCodi);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTRetVersion;
        }

        /// <summary>
        /// Permite listar las transferencia de retiro detalle y entrega detalle cada 15 min en base a la empresa,periodo,version,flagtipo
        /// </summary>
        /// <param name="iEmprcodi">Código de la Empresa</param>
        /// <param name="iBarrcodi">Código de la barra de Transferencia</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVersion">Versión de Recalculo</param>
        /// <param name="iFlagtipo">Flag que indica si es Entrega o Retiro</param>
        /// <returns>Lista de TransferenciaRetiroDetalleDTO</returns>
        public List<TransferenciaEntregaDetalleDTO> ListTransEntrReti(int iEmprcodi, int iBarrcodi, int iPericodi, int iVersion, string iFlagtipo)
        {
            return FactoryTransferencia.GetTransferenciEntregaRetiroDetalleRepository().ListTransEntrReti(iEmprcodi, iBarrcodi, iPericodi, iVersion, iFlagtipo);
        }

        /// <summary>
        /// Permite listar los codigos de Entrega y Retiro reportados en el periodo, versión por empresa
        /// </summary>
        /// <param name="emprcodi">Código de la Empresa</param>
        /// <param name="pericodi">Código de Periodo</param>
        /// <param name="recacodi">Versión de Recalculo</param>
        /// <returns>Lista de TransferenciaRetiroDetalleDTO</returns>
        public List<ExportExcelDTO> ListarCodigoReportado(int emprcodi, int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetTransferenciEntregaRetiroDetalleRepository().ListarCodigoReportado(emprcodi, pericodi, recacodi);
        }

        /// <summary>
        /// Inserta de forma masiva una lista de TransferenciaEntregaDTO
        /// </summary>
        /// <param name="entity">TransferenciaEntregaDTO</param>    
        public void BulkInsertValorTransEntrega(List<TrnTransEntregaBullk> entitys)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaEntregaRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta de forma masiva una lista de TransferenciaEntregaDetalleDTO
        /// </summary>
        /// <param name="entity">TransferenciaEntregaDetalleDTO</param>    
        public void BulkInsertValorTransEntregaDetalles(List<TrnTransEntregaDetalleBullk> entitys)
        {
            try
            {
                FactoryTransferencia.GetTransferenciEntregaRetiroDetalleRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta de forma masiva una lista de TransferenciaRetiroDTO
        /// </summary>
        /// <param name="entity">TransferenciaRetiroDTO</param>    
        public void BulkInsertValorTransRetiro(List<TrnTransRetiroBullk> entitys)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta de forma masiva una lista de TransferenciaRetiroDetalleDTO
        /// </summary>
        /// <param name="entity">TransferenciaRetiroDetalleDTO</param>    
        public void BulkInsertValorTransRetiroDetalle(List<TrnTransRetiroDetalleBullk> entitys)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Se obtiene el siguiente Identificador de la tabla TRN_TRANS_ENTREGA
        /// </summary>
        public int GetCodigoGeneradoTransEntr()
        {
            return FactoryTransferencia.GetTransferenciaEntregaRepository().GetCodigoGenerado();
        }

        /// <summary>
        /// Se obtiene el siguiente Identificador de la tabla TRN_TRANS_ENTREGA_DETALLE
        /// </summary>
        public int GetCodigoGeneradoTransEntrDet()
        {
            return FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().GetCodigoGenerado();
        }

        /// <summary>
        /// Se obtiene el siguiente Identificador de la tabla TRN_TRANS_RETIRO
        /// </summary>
        public int GetCodigoGeneradoTransRet()
        {
            return FactoryTransferencia.GetTransferenciaRetiroRepository().GetCodigoGenerado();
        }

        /// <summary>
        /// Se obtiene el siguiente Identificador de la tabla TRN_TRANS_RETIRO_DETALLE
        /// </summary>
        public int GetCodigoGeneradoTransRetDet()
        {
            return FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().GetCodigoGenerado();
        }

        /// <summary>
        /// Permite obtener el siguiente en menor valor del Primary Key de la tabla TRN_TRANS_ENTREGA
        /// </summary>
        public int GetCodigoGeneradoTransEntrDesc()
        {
            return FactoryTransferencia.GetTransferenciaEntregaRepository().GetCodigoGeneradoDec();
        }

        /// <summary>
        /// Permite copiar las Entregas (y detalle) de un periodo de una versión anterior a la nueva versión PK: iTransEntrCodi, iTransEntrDetCodi
        /// </summary>
        /// <param name="iPeriCodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="iVersionOld">Versión anterior del recalculo</param>
        /// <param name="iVersionNew">Versión siguiente del recalculo</param>
        /// <param name="iTransEntrCodi">Identificador minimo de la Tabla TRN_TRANS_ENTREGA</param>
        /// <param name="iTransEntrDetCodi">Identificador minimo de la Tabla TRN_TRANS_ENTREGA_DETALLE</param>
        /// <returns></returns>
        public void CopiarEntregas(int iPeriCodi, int iVersionOld, int iVersionNew, int iTransEntrCodi, int iTransEntrDetCodi, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaEntregaRepository().CopiarEntregas(iPeriCodi, iVersionOld, iVersionNew, iTransEntrCodi, iTransEntrDetCodi, conn, tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el siguiente en menor valor del Primary Key de la tabla TRN_TRANS_RETIRO
        /// </summary>
        public int GetCodigoGeneradoTransRetDesc()
        {
            return FactoryTransferencia.GetTransferenciaRetiroRepository().GetCodigoGeneradoDesc();
        }

        /// <summary>
        /// Permite copiar los Retiros (y detalle) de un periodo de una versión anterior a la nueva versión PK: iTranRetiCodi, iTranRetiDetaCodi
        /// </summary>
        /// <param name="iPeriCodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="iVersionOld">Versión anterior del recalculo</param>
        /// <param name="iVersionNew">Versión siguiente del recalculo</param>
        /// <param name="iTranRetiCodi">Identificador minimo de la Tabla TRN_TRANS_RETIRO</param>
        /// <param name="iTranRetiDetaCodi">Identificador minimo de la Tabla TRN_TRANS_RETIRO_DETALLE</param>
        /// <returns></returns>
        public void CopiarRetiros(int iPeriCodi, int iVersionOld, int iVersionNew, int iTranRetiCodi, int iTranRetiDetaCodi, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRetiroRepository().CopiarRetiros(iPeriCodi, iVersionOld, iVersionNew, iTranRetiCodi, iTranRetiDetaCodi, conn, tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite poner en estado INA a la lista de códigos de retiro
        /// </summary>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="recacodi">Versión de recalculo</param>
        /// <param name="listaRetiros">Lista de ID de Codigos de Retiro</param>
        /// <param name="genemprcodi">Identificador de la empresa generadora</param>
        /// <param name="suser">User de la intranet</param>
        /// <returns></returns>
        public void UpdateTransferenciaRetiroEstadoINA(int pericodi, int recacodi, List<string> listaRetiros, int genemprcodi, string suser)
        {
            FactoryTransferencia.GetTransferenciaRetiroRepository().UpdateTransferenciaRetiroEstadoINA(pericodi, recacodi, listaRetiros, genemprcodi, suser);
        }

        /// <summary>
        /// Permite poner en estado INA a la lista de códigos de entrega
        /// </summary>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="recacodi">Versión de recalculo</param>
        /// <param name="listaEntregas">Lista de ID de Codigos de Entrega</param>
        /// <param name="emprcodi">Identificador de la empresa generadora</param>
        /// <param name="suser">User de la intranet</param>
        /// <returns></returns>
        public void UpdateTransferenciaEntregaEstadoINA(int pericodi, int recacodi, List<string> listaEntregas, int emprcodi, string suser)
        {
            FactoryTransferencia.GetTransferenciaEntregaRepository().UpdateTransferenciaEntregaEstadoINA(pericodi, recacodi, listaEntregas, emprcodi, suser);
        }
    }
}
