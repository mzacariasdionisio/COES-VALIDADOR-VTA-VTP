using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Enum;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class ValorTransferenciaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Graba una entidad ValorTransferenciaDTO
        /// </summary>
        /// <param name="entity">ValorTransferenciaDTO</param>    
        /// <returns>Código de la tabla TRN_VALOR_TRANS_EMPRESA</returns> 
        public int SaveValorTransferencia(ValorTransferenciaDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.ValoTranCodi == 0)
                {
                    id = FactoryTransferencia.GetValorTransferenciaRepository().Save(entity);
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_VALOR_TRANS_EMPRESA en base  al periodo y version costo marginal
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <returns>Código del mes de valorización</returns> 
        public int DeleteListaValorTransferencia(int iPeriCodi, int iVTranVersion)
        {
            try
            {
                FactoryTransferencia.GetValorTransferenciaRepository().Delete(iPeriCodi, iVTranVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        /// <summary>
        /// Elimina valorizacion en base  al periodo y version revision
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="vers">Versión de Recalculo</param>
        /// <returns>Código del mes de valorización</returns> 
        public string DeleteValorizacion(int iPeriCodi, int vers, string user)
        {
            try
            {
                //Elimina información de la tabla trn_valor_trans = Valorización de la Transferencia de Entregas y Retiros por Empresa[15]
                int eliminavalor = 0;
                eliminavalor = this.DeleteListaValorTransferencia(iPeriCodi, vers);

                //Elimina información de la tabla trn_valor_trans_empresa
                int deletepok = 0;
                deletepok = this.DeleteValorTransferenciaEmpresa(iPeriCodi, vers);

                //Elimina información calculada de los Ingresos por potencia de las empresas -> tabla trn_saldo_empresa
                int deleteSaldo = 0;
                deleteSaldo = this.DeleteSaldoTransmisionEmpresa(iPeriCodi, vers);

                //Elimina información calculada de los Ingresos por Retiros sin contrato de las empresas -> de la tabla trn_saldo_coresc
                int deleteSaldoSC = 0;
                deleteSaldoSC = this.DeleteSaldoCodigoRetiroSC(iPeriCodi, vers);

                //Elimina información de la tabla trn_empresa_pago = Matriz de Pagos
                int eliminook = 0;
                eliminook = this.DeleteEmpresaPago(iPeriCodi, vers);

                //Elimina información calculado del Valor Total de la Empresa -> trn_valor_total_empresa
                int deleteTVEmpresa = 0;
                deleteTVEmpresa = this.DeleteValorTotalEmpresa(iPeriCodi, vers);

                if (vers > 1)
                {
                    //Elimina información calculado del Saldo por Recalculo de la Empresa -> trn_saldo_recalculo
                    int deleteSaldoRecalculo = 0;
                    deleteSaldoRecalculo = this.DeleteSaldoRecalculo(iPeriCodi, vers);
                }
                PeriodoDTO dtoPeriodo = FactoryTransferencia.GetPeriodoRepository().GetById(iPeriCodi);
                RecalculoDTO dtoRecalculo2 = FactoryTransferencia.GetRecalculoRepository().GetById(iPeriCodi, vers);
                //---------------------------------------------------------------------------------------------
                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.ProcesarValorizacionBorrar;
                objAuditoria.Estdcodi = (int)EVtpEstados.CalcularValorizacion;
                objAuditoria.Audproproceso = "Valorización borrada";
                objAuditoria.Audprodescripcion = "Se borró la valorización desde mantenimiento de revision del periodo " + dtoPeriodo.PeriNombre + " / " + dtoRecalculo2.RecaNombre;
                objAuditoria.Audprousucreacion = user;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                _ = FactoryTransferencia.GetVtpAuditoriaProcesoRepository().Save(objAuditoria);

                #endregion
                return "1";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Permite obtener una lista de Empresas con su respectivo valor de transferencia
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListValorTransferenciaEmpresaRE(int iPeriCodi, int iVTranVersion)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().List(iPeriCodi, iVTranVersion);
        }

        /// <summary>
        /// Obtiene el Valor de la transferencia en base al empcodi,barra,periodo,tipoempresa,version,flag entrega o retiro
        /// </summary>
        /// <param name="iEmprCodi">Código de la empresa</param>
        /// <param name="iBarrCodi">Código de La Barra de transferencia</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTipoEmprCodi">Código de tipo de empresa</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <param name="sTranFlag">Flag que indica si el registro es de entrega o retiro</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> BuscarValorTransferenciaGetByCriteria(int? iEmprCodi, int? iBarrCodi, int? iPeriCodi, int? iTipoEmprCodi, int? iVTranVersion, string sTranFlag)//int pericodi, string barrcodi
        {

            return FactoryTransferencia.GetValorTransferenciaRepository().GetByCriteria(iEmprCodi, iBarrCodi, iPeriCodi, iTipoEmprCodi, iVTranVersion, sTranFlag); //
        }

        /// <summary>
        /// Obtener la lista total de ValorTransferencia
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ObtenerTotalEnergiaporEntregaoRetiro(int iPeriCodi, int iVTranVersion)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().GetTotalByTipoFlag(iPeriCodi, iVTranVersion);
        }

        /// <summary>
        /// Obtener lista de  Valor de la empresa
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ObtenerTotalValorEmpresa(int iPeriCodi, int iVTranVersion)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().GetValorEmpresa(iPeriCodi, iVTranVersion);
        }

        /// <summary>
        /// Obtener lista de Empresa por Energia total del mes en Entrega y Retiro
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListarBalanceEnergia(int iPeriCodi, int iVTranVersion)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().GetBalanceEnergia(iPeriCodi, iVTranVersion);
        }

        /// <summary>
        /// Obtener lista de Barras con su respectiva suma de energias del periodo actual y el periodo anterior
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo Actual</param>
        /// <param name="iPeriCodiAnterior">Código de Periodo Anterior</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListarDesviacionRetiros(int iPeriCodi, int iPeriCodiAnterior)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().GetDesviacionRetiros(iPeriCodi, iPeriCodiAnterior);
        }

        /// <summary>
        /// Obtener lista de Empresas del valor de transferencia total del mes en Entrega y Retiro
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListarBalanceValorTransferencia(int iPeriCodi, int iVTranVersion)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().GetBalanceValorTransferencia(iPeriCodi, iVTranVersion);
        }


        /// <summary>
        /// Obtener lista  del valor de transferencia total del mes en Entrega y Retiro cada 15 min
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <param name="iEmpcodi">Código de Empresa</param>
        /// <param name="iBarrcodi">Código de la Barra de Transferencia</param>
        /// <param name="sTranFlag">Flag de Entrega o Retiro</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListarValorTransferencia(int iPeriCodi, int iVTranVersion, int iEmpcodi, int iBarrcodi, string sTranFlag)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().GetValorTransferencia(iPeriCodi, iVTranVersion, iEmpcodi, iBarrcodi, sTranFlag);
        }

        //CU21
        /// <summary>
        /// Obtener lista de Energía de Entrega cada 15 min
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <param name="iCodEntCodi">Código de Entrega</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListarEnergiaEntregaDetalle(int iPeriCodi, int iVTranVersion, int iCodEntCodi)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().ListarEnergiaEntregaDetalle(iPeriCodi, iVTranVersion, iCodEntCodi);
        }

        /// <summary>
        /// Obtener lista de Energía de Retiro cada 15 min
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <param name="listaCodigosRetiro">Lista de Códigos de Retiro, separados por comas</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListarEnergiaRetiroDetalle(int iPeriCodi, int iVTranVersion, string listaCodigosRetiro)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().ListarEnergiaRetiroDetalle(iPeriCodi, iVTranVersion, listaCodigosRetiro);
        }

        /// <summary>
        /// Obtener lista de Energía Valorizado [ENTREGA * COSTO_MARGINAL] de Entrega cada 15 min
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <param name="iCodEntCodi">Código de Entrega</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListarValorEnergiaEntregaDetalle(int iPeriCodi, int iVTranVersion, int iCodEntCodi)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().ListarValorEnergiaEntregaDetalle(iPeriCodi, iVTranVersion, iCodEntCodi);
        }

        /// <summary>
        /// Obtener lista de Energía Valorizado [ENTREGA * COSTO_MARGINAL] de Retiro cada 15 min
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <param name="listaCodigosRetiro">Lista de Códigos de Retiro, separados por comas</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<ValorTransferenciaDTO> ListarValorEnergiaRetiroDetalle(int iPeriCodi, int iVTranVersion, string listaCodigosRetiro)
        {
            return FactoryTransferencia.GetValorTransferenciaRepository().ListarValorEnergiaRetiroDetalle(iPeriCodi, iVTranVersion, listaCodigosRetiro);
        }
        //VALOR TRANSFERENCIA EMPRESA

        /// <summary>
        /// Graba un ValorTransferencia por Empresa
        /// </summary>
        /// <param name="entity">ValorTransferenciaEmpresaDTO</param>
        /// <returns>Codigo del nuevo registro de la tabla TRN_VALOR_TRANS_EMPRESA</returns> 
        public int SaveValorTransferenciaEmpresa(ValorTransferenciaEmpresaDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.ValTranEmpCodi == 0)
                {
                    id = FactoryTransferencia.GetValorTransferenciaEmpresaRepository().Save(entity);
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_VALOR_TRANS_EMPRESA
        /// </summary>
        /// <param name="iPeriCodi">Código del periodo</param>
        /// <param name="VTranEVersion">Código del recalculo</param>
        /// <returns>Código del mes de valorizacion</returns> 
        public int DeleteValorTransferenciaEmpresa(int iPeriCodi, int VTranEVersion)
        {
            try
            {
                FactoryTransferencia.GetValorTransferenciaEmpresaRepository().Delete(iPeriCodi, VTranEVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        //VALOR TOTAL EMPRESA

        /// <summary>
        /// Graba una entidad Valor Total Empresa
        /// </summary>
        /// <param name="entity">ValorTotalEmpresaDTO</param>    
        /// <returns>Código nuevo de la tabla TRN_VALOR_TOTAL_EMPRESA</returns> 
        public int SaveOrUpdateValorTotalEmpresa(ValorTotalEmpresaDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.ValTotaEmpCodi == 0)
                {
                    id = FactoryTransferencia.GetValorTotalEmpresaRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetValorTotalEmpresaRepository().Update(entity);
                    id = entity.ValTotaEmpCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un listado de registros de la tabla TRN_VALOR_TOTAL_EMPRESA
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTotEmVersion">Versión de Recalculo</param>
        /// <returns>Código nuevo del mes de valorización</returns> 
        public int DeleteValorTotalEmpresa(int iPeriCodi, int iVTotEmVersion)
        {
            try
            {
                FactoryTransferencia.GetValorTotalEmpresaRepository().Delete(iPeriCodi, iVTotEmVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        /// <summary>
        /// Permite obtener una lista de Empresas con su respectiva valorización de determinación con saldo positivo
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTotEmVersion">Versión de Recalculo</param>
        /// <returns>Lista de ValorTotalEmpresaDTO</returns>
        public List<ValorTotalEmpresaDTO> BuscarEmpresasValorPositivo(int iPeriCodi, int iVTotEmVersion)
        {
            return FactoryTransferencia.GetValorTotalEmpresaRepository().GetEmpresaPositivaByCriteria(iPeriCodi, iVTotEmVersion);
        }

        /// <summary>
        /// Permite obtener una lista de Empresas con su respectiva valorización de determinación con saldo negativo
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTotEmVersion">Versión de Recalculo</param>
        /// <returns>Lista de ValorTotalEmpresaDTO</returns>
        public List<ValorTotalEmpresaDTO> BuscarEmpresasValorNegativo(int iPeriCodi, int iVTotEmVersion)
        {
            return FactoryTransferencia.GetValorTotalEmpresaRepository().GetEmpresaNegativaByCriteria(iPeriCodi, iVTotEmVersion);
        }

        /// <summary>
        /// Permite obtener una lista de Empresas con su respectiva valorización de determinación
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTotEmVersion">Versión de Recalculo</param>
        /// <returns>Lista de ValorTotalEmpresaDTO</returns>
        public List<ValorTotalEmpresaDTO> ListarValorTotalEmpresa(int iPeriCodi, int iVTotEmVersion)
        {
            return FactoryTransferencia.GetValorTotalEmpresaRepository().ListarValorTotalEmpresa(iPeriCodi, iVTotEmVersion);
        }

        /// <summary>
        /// Permite obtener una empresa con su respectiva valorización de determinación
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTotEmVersion">Versión de Recalculo</param>
        /// <returns>ValorTotalEmpresaDTO</returns>
        public ValorTotalEmpresaDTO GetByCriteria(int iPeriCodi, int iVTotEmVersion, int iEmprCodi)
        {
            return FactoryTransferencia.GetValorTotalEmpresaRepository().GetByCriteria(iPeriCodi, iVTotEmVersion, iEmprCodi);
        }

        //SALDO EMPRESA

        /// <summary>
        /// Graba o actualiza entidad Saldo de Transmision de la Empresa: TRN_SALDO_EMPRESA
        /// </summary>
        /// <param name="entity">SaldoEmpresaDTO</param>
        /// <returns>Código de la tabla TRN_SALDO_EMPRESA</returns> 
        public int SaveOrUpdateSaldoTransmisionEmpresa(SaldoEmpresaDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.SalempCodi == 0)
                {
                    id = FactoryTransferencia.GetSaldoEmpresaRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetSaldoEmpresaRepository().Update(entity);
                    id = entity.SalempCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_SALDO_EMPRESA en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="SalEmpVersion">Versión del mes de valorización</param>   
        /// <returns>Código del mes de valorización</returns> 
        public int DeleteSaldoTransmisionEmpresa(int iPeriCodi, int SalEmpVersion)
        {
            try
            {
                FactoryTransferencia.GetSaldoEmpresaRepository().Delete(iPeriCodi, SalEmpVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        //SALDO EMPRESA RETIRO SIN CONTRATO

        /// <summary>
        /// Graba o actualiza un Saldo de Codigo de Retiro Sin Contrato TRN_SALDO_CORESC
        /// </summary>
        /// <param name="entity">SaldoCodigoRetiroscDTO</param>
        /// <returns>Código de la tabla TRN_SALDO_CORESC</returns> 
        public int SaveOrUpdateSaldoCodigoRetiroSC(SaldoCodigoRetiroscDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.SalrscCodi == 0)
                {
                    id = FactoryTransferencia.GetSaldoCodigoRetiroSCRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetSaldoCodigoRetiroSCRepository().Update(entity);
                    id = entity.SalrscCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_SALDO_CORESC en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="SalRSCVersion">Versión del mes de valorización</param>     
        /// <returns>Código del mes de valorización</returns> 
        public int DeleteSaldoCodigoRetiroSC(int iPeriCodi, int SalRSCVersion)
        {
            try
            {
                FactoryTransferencia.GetSaldoCodigoRetiroSCRepository().Delete(iPeriCodi, SalRSCVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        //EMPRESA PAGO

        /// <summary>
        /// Permite Grabar en la tabla TRN_EMPRESA_PAGO
        /// </summary>
        /// <param name="entity">Entidad de EmpresaPagoDTO</param>
        /// <returns>Retorna el Código de la tabla TRN_EMPRESA_PAGO</returns>       
        public int SaveoUpdateEmpresaPago(EmpresaPagoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.EmpPagoCodi == 0)
                {
                    id = FactoryTransferencia.GetEmpresaPagoRepository().Save(entity);
                }
                else
                {
                    id = entity.EmpPagoCodi;
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_EMPRESA_PAGO en base  al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Código del Mes de valorización</param>
        /// <param name="iEmpPagVersion">Versión del mes de valorizacion</param>
        /// <returns>Retorna el iPeriCodi</returns>  
        public int DeleteEmpresaPago(int iPeriCodi, int iEmpPagVersion)
        {
            try
            {
                FactoryTransferencia.GetEmpresaPagoRepository().Delete(iPeriCodi, iEmpPagVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        //SALDO_RECALCULO
        public int SaveoUpdateSaldoRecalculo(SaldoRecalculoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.SalRecCodi == 0)
                {
                    id = FactoryTransferencia.GetSaldoRecalculoRepository().Save(entity);
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla TRN_SALDO_RECALCULO en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iVTranVersion">Versión de Recalculo</param>
        /// <returns>Código del mes de valorización</returns> 
        public int DeleteSaldoRecalculo(int iPeriCodi, int iRecaCodi)
        {
            try
            {
                FactoryTransferencia.GetSaldoRecalculoRepository().Delete(iPeriCodi, iRecaCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        /// <summary>
        /// Consulta si existen registros en la tabla TRN_SALDO_RECALCULO asociados al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iRecaCodi">Versión de Recalculo</param>
        /// <returns>Código del mes de valorización</returns> 
        public int GetByPericodiDestino(int iPeriCodi, int iRecaCodi)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetSaldoRecalculoRepository().GetByPericodiDestino(iPeriCodi, iRecaCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return id;
        }

        /// <summary>
        /// Actualizamos la información en la tabla TRN_SALDO_RECALCULO asociados al periodo y version con el nuevo PeriCodiDestino
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iRecaCodi">Versión de Recalculo</param>
        /// <param name="iPeriCodiDestino">Periodo de Valorización actual para los TRN_SALDO_RECALCULO</param>
        /// <returns>Código del mes de valorización</returns> 
        public int UpdatePericodiDestino(int iPeriCodi, int iRecaCodi, int iPeriCodiDestino)
        {
            try
            {
                FactoryTransferencia.GetSaldoRecalculoRepository().UpdatePericodiDestino(iPeriCodi, iRecaCodi, iPeriCodiDestino);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return iPeriCodiDestino;
        }

        /// <summary>
        /// Obtener Identificador siguiente de Base de datos
        /// </summary>
        /// <returns>Identificador TRN_VALOR_TRANS</returns> 
        public int GetCodigoGenerado()
        {
            try
            {
                int id = FactoryTransferencia.GetValorTransferenciaRepository().GetCodigoGenerado();
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta de forma masiva una lista de ValorTransferenciaDTO
        /// </summary>
        /// <param name="entity">ValorTransferenciaDTO</param>    
        public void BulkInsertValorTransferencia(List<TrnValorTransBullk> entitys)
        {
            try
            {
                FactoryTransferencia.GetValorTransferenciaRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //ASSETEC - 20181001-----------------------------------------------------------------------------------------------
        /// <summary>
        /// Obtenemos mediante un Insert into Select la Valorización de la Transferencia de Entregas por Empresa[15]
        /// </summary>
        /// <param name="pericodi">Código de Periodo</param>
        /// <param name="version">Versión de Recalculo</param>
        /// <returns>a nivel de Base de datos obtiene las entregas valorizadas</returns> 
        public void GrabarValorizacionEntrega(int pericodi, int version, string user)
        {
            try
            {
                int iVtrancodi = this.GetCodigoGenerado();
                FactoryTransferencia.GetValorTransferenciaRepository().GrabarValorizacionEntrega(pericodi, version, user, iVtrancodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtenemos mediante un Insert into Select la Valorización de la Transferencia de Retiros por Empresa[15]
        /// </summary>
        /// <param name="pericodi">Código de Periodo</param>
        /// <param name="version">Versión de Recalculo</param>
        /// <returns>a nivel de Base de datos obtiene los retiros valorizadas</returns> 
        public void GrabarValorizacionRetiro(int pericodi, int version, string user)
        {
            try
            {
                int iVtrancodi = this.GetCodigoGenerado();
                FactoryTransferencia.GetValorTransferenciaRepository().GrabarValorizacionRetiro(pericodi, version, user, iVtrancodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region Modificaciones de Reportes para egejunin

        public List<ValorTransferenciaDTO> ObtenerTotalValorEmpresa1(int iPeriCodi, int iVTranVersion)
        {
            #region Codigo Modificao egejunin

            List<ValorTransferenciaDTO> entitys = FactoryTransferencia.GetValorTransferenciaRepository().GetValorEmpresa(iPeriCodi, iVTranVersion);
            /*foreach (ValorTransferenciaDTO enti_ in entitys)
            {
                enti_.SalEmpSaldo = FactoryTransferencia.GetValorTransferenciaRepository().GetSaldoEmpresa(iPeriCodi, iVTranVersion, (int)enti_.EmpCodi);
            }*/
            List<ValorTransferenciaDTO> entitys1 = entitys.Where(x => x.EmpCodi != 10582 && x.EmpCodi != 11153).ToList();

            ValorTransferenciaDTO entity = new ValorTransferenciaDTO();
            List<ValorTransferenciaDTO> entitys2 = entitys.Where(x => x.EmpCodi == 10582 || x.EmpCodi == 11153).ToList();

            Dominio.DTO.Sic.SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(11153);
            entity.EmpCodi = empresa.Emprcodi;
            entity.EmprNomb = empresa.Emprnomb;

            foreach (ValorTransferenciaDTO item in entitys2)
            {
                entity.SalEmpSaldo += item.SalEmpSaldo;
                entity.Compensacion += item.Compensacion;
                entity.Valorizacion += item.Valorizacion;
                entity.SalrscSaldo += item.SalrscSaldo;
                entity.Salrecalculo += item.Salrecalculo;
                entity.Vtotempresa += item.Vtotempresa;
                entity.Vtotanterior += item.Vtotanterior;
                entity.Entregas += item.Entregas;
                entity.ValoTranVersion = item.ValoTranVersion;
                entity.PeriCodi = item.PeriCodi;
            }

            entitys1.Add(entity);

            return entitys1.OrderBy(x => x.EmprNomb).ToList();
            #endregion

            //return FactoryTransferencia.GetValorTransferenciaRepository().GetValorEmpresa(iPeriCodi, iVTranVersion);
        }

        public List<ValorTransferenciaDTO> ListarBalanceEnergia1(int iPeriCodi, int iVTranVersion)
        {
            List<ValorTransferenciaDTO> entitys = FactoryTransferencia.GetValorTransferenciaRepository().GetBalanceEnergia(iPeriCodi, iVTranVersion);

            List<ValorTransferenciaDTO> entitys1 = entitys.Where(x => x.EmpCodi != 10582 && x.EmpCodi != 11153).ToList();

            ValorTransferenciaDTO entity = new ValorTransferenciaDTO();
            List<ValorTransferenciaDTO> entitys2 = entitys.Where(x => x.EmpCodi == 10582 || x.EmpCodi == 11153).ToList();

            Dominio.DTO.Sic.SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(11153);
            entity.EmpCodi = empresa.Emprcodi;
            entity.NombEmpresa = empresa.Emprnomb;

            foreach (ValorTransferenciaDTO item in entitys2)
            {
                entity.Entregas += item.Entregas;
                entity.Retiros += item.Retiros;
            }

            entitys1.Add(entity);

            return entitys1.OrderBy(x => x.NombEmpresa).ToList();
        }

        public List<ValorTransferenciaDTO> ListarBalanceValorTransferencia1(int iPeriCodi, int iVTranVersion)
        {
            List<ValorTransferenciaDTO> entitys = FactoryTransferencia.GetValorTransferenciaRepository().GetBalanceValorTransferencia(iPeriCodi, iVTranVersion);

            List<ValorTransferenciaDTO> entitys1 = entitys.Where(x => x.EmpCodi != 10582 && x.EmpCodi != 11153).ToList();

            ValorTransferenciaDTO entity = new ValorTransferenciaDTO();
            List<ValorTransferenciaDTO> entitys2 = entitys.Where(x => x.EmpCodi == 10582 || x.EmpCodi == 11153).ToList();

            Dominio.DTO.Sic.SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(11153);
            entity.EmpCodi = empresa.Emprcodi;
            entity.NombEmpresa = empresa.Emprnomb;

            foreach (ValorTransferenciaDTO item in entitys2)
            {
                entity.Entregas += item.Entregas;
                entity.Retiros += item.Retiros;
            }

            entitys1.Add(entity);

            return entitys1.OrderBy(x => x.NombEmpresa).ToList();
        }

        public List<ValorTransferenciaDTO> BuscarValorTransferenciaGetByCriteria1(int? iEmprCodi, int? iBarrCodi, int? iPeriCodi, int? iTipoEmprCodi, int? iVTranVersion, string sTranFlag)//int pericodi, string barrcodi
        {
            Dominio.DTO.Sic.SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(11153);
            List<ValorTransferenciaDTO> entitys = FactoryTransferencia.GetValorTransferenciaRepository().GetByCriteria(iEmprCodi, iBarrCodi, iPeriCodi, iTipoEmprCodi, iVTranVersion, sTranFlag); //

            foreach (ValorTransferenciaDTO item in entitys)
            {
                if (item.Emprcodi == 10582)
                {
                    item.EmprNomb = empresa.Emprnomb;
                }
            }

            return entitys;
        }

        #endregion


        public List<ValorTransferenciaDTO> ListarCodigosValorizados(int pericodi, int version, int? empcodi, int? cliemprcodi, int? barrcodi, string flagEntrReti, DateTime? fechaIni, DateTime? fechaFin)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            try
            {
                entitys = FactoryTransferencia.GetValorTransferenciaRepository().ListarCodigosValorizados(pericodi, version, empcodi, cliemprcodi, barrcodi, flagEntrReti, fechaIni, fechaFin);
            }
            catch (Exception ex)
            {
                entitys = null;
            }

            return entitys;
        }

        public List<ValorTransferenciaDTO> ListarCodigosValorizadosTransferencia(int pericodi, int version, int? empcodi, int? barrcodi, string flagEntrReti, DateTime? fechaIni, DateTime? fechaFin)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            try
            {
                entitys = FactoryTransferencia.GetValorTransferenciaRepository().ListarCodigosValorizadosTransferencia(pericodi, version, empcodi, barrcodi, flagEntrReti, fechaIni, fechaFin);
            }
            catch (Exception ex)
            {
                entitys = null;
            }

            return entitys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="version"></param>
        /// <param name="empcodi"></param>
        /// <param name="codigos"></param>
        /// <returns></returns>
        public List<ComparacionEntregaRetirosGraficoDTO> ListarCodigosValorizadosGrafica(int trnenvtipinf, int pericodi, int version, int? empcodi, string codigos, DateTime FechaInicio, DateTime FechaFin)
        {
            List<ComparacionEntregaRetirosGraficoDTO> resultado = new List<ComparacionEntregaRetirosGraficoDTO>();

            try
            {
                DataTable tbl = new DataTable();
                if (trnenvtipinf == 1)
                    tbl = FactoryTransferencia.GetValorTransferenciaRepository().ListarCodigosValorizadosGrafica_New(pericodi, version, empcodi, codigos, FechaInicio, FechaFin);
                else if (trnenvtipinf == 2)
                    tbl = FactoryTransferencia.GetValorTransferenciaRepository().ListarCodigosValorizadosGraficaTransferencia_New(pericodi, version, empcodi, codigos, FechaInicio, FechaFin);

                #region Generar Header Grafica
                int rowIndex = -1;
                foreach (DataRow item in tbl.Rows)
                {
                    rowIndex++;
                    DateTime fechaEnergia = Convert.ToDateTime(item["FECHA"]);

                    ComparacionEntregaRetirosGraficoDTO entregaRetirosGraficoDTO = resultado.Where(x => x.Codigo == item["TENTCODIGO"].ToString()).FirstOrDefault();

                    if (entregaRetirosGraficoDTO == null)
                    {
                        entregaRetirosGraficoDTO = new ComparacionEntregaRetirosGraficoDTO
                        {
                            Codigo = item["TENTCODIGO"].ToString(),
                            entregaRetiros = new List<CostoMarginalGraficoValoresDTO>()
                        };

                        resultado.Add(entregaRetirosGraficoDTO);
                    }

                    #region Genera Grafica

                    int columIndex = -1;
                    int intervaloIndice = 0;
                    DateTime intervalo = fechaEnergia;
                    TimeSpan tiempo = new TimeSpan(00, 00, 0);
                    foreach (DataColumn itemCols in tbl.Columns)
                    {
                        columIndex++;
                        if (columIndex > 1)
                        {

                            intervaloIndice++;
                            if (intervaloIndice == 96)
                                intervalo = intervalo.Add(new TimeSpan(00, 14, 0));
                            else
                                intervalo = intervalo.Add(new TimeSpan(00, 15, 0));

                            CostoMarginalGraficoValoresDTO entidad = new CostoMarginalGraficoValoresDTO();
                            entidad.FechaIntervalo = intervalo;
                            entidad.CMGREnergia = Convert.ToDecimal(tbl.Rows[rowIndex][columIndex] == DBNull.Value ? "0" : tbl.Rows[rowIndex][columIndex].ToString());
                            entregaRetirosGraficoDTO.entregaRetiros.Add(entidad);
                        }
                    }
                    #endregion Genera Grafica
                }

                #endregion Generar Header Grafica



            }
            catch (Exception ex)
            {
                resultado = null;
            }

            return resultado;
        }

        public List<ComparacionEntregaRetirosGraficoDTO> ListarCodigosValorizadosGraficaTotal(int trnenvtipinf, int pericodi, int version, int? empcodi, string codigos, DateTime FechaInicio, DateTime FechaFin)
        {
            List<ComparacionEntregaRetirosGraficoDTO> resultado = new List<ComparacionEntregaRetirosGraficoDTO>();

            try
            {
                DataTable tbl = new DataTable();
                if (trnenvtipinf == 1)
                    tbl = FactoryTransferencia.GetValorTransferenciaRepository().ListarCodigosValorizadosGraficaTotal(pericodi, version, empcodi, codigos, FechaInicio, FechaFin);
                if (trnenvtipinf == 2)
                    tbl = FactoryTransferencia.GetValorTransferenciaRepository().ListarCodigosValorizadosGraficaTotalTransferencia(pericodi, version, empcodi, codigos, FechaInicio, FechaFin);

                #region Generar Header Grafica
                int rowIndex = -1;
                foreach (DataRow item in tbl.Rows)
                {
                    rowIndex++;
                    DateTime fechaEnergia = Convert.ToDateTime(item["FECHA"]);

                    ComparacionEntregaRetirosGraficoDTO entregaRetirosGraficoDTO = resultado.Where(x => x.Codigo == item["TENTCODIGO"].ToString()).FirstOrDefault();

                    if (entregaRetirosGraficoDTO == null)
                    {
                        entregaRetirosGraficoDTO = new ComparacionEntregaRetirosGraficoDTO
                        {
                            Codigo = item["TENTCODIGO"].ToString(),
                            entregaRetiros = new List<CostoMarginalGraficoValoresDTO>()
                        };

                        resultado.Add(entregaRetirosGraficoDTO);
                    }

                    #region Genera Grafica

                    DateTime intervalo = fechaEnergia;
                    TimeSpan tiempo = new TimeSpan(00, 00, 0);
                    CostoMarginalGraficoValoresDTO entidad = new CostoMarginalGraficoValoresDTO();
                    entidad.FechaIntervalo = intervalo;
                    entidad.CMGREnergia = Convert.ToDecimal(tbl.Rows[rowIndex]["totalDia"] == DBNull.Value ? "0" : tbl.Rows[rowIndex]["totalDia"].ToString());
                    entregaRetirosGraficoDTO.entregaRetiros.Add(entidad);

                    #endregion Genera Grafica
                }

                #endregion Generar Header Grafica



            }
            catch (Exception ex)
            {
                resultado = null;
            }

            return resultado;
        }

        public List<ValorTransferenciaDTO> ListarCodigos(int EmprCodi)
        {
            List<ValorTransferenciaDTO> entitys = new List<ValorTransferenciaDTO>();
            try
            {
                entitys = FactoryTransferencia.GetValorTransferenciaRepository().ListarCodigos(EmprCodi);
            }
            catch (Exception ex)
            {
                entitys = null;
            }

            return entitys;
        }
        public List<EmpresaDTO> ListarEmpresasAsociadas(ComparacionEntregaRetirosFiltroDTO param)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            try
            {
                entitys = FactoryTransferencia.GetValorTransferenciaRepository().ListarEmpresasAsociadas(param);
            }
            catch (Exception ex)
            {
                entitys = null;
            }

            return entitys;
        }


        public List<ComparativoDTO> ListarComparativo(int pericodi, int version, int pericodi2, int version2, int? empcodi, int? cliemprcodi, int? barrcodi,
            string flagEntrReti, string dias, string codigos)
        {
            List<ComparativoDTO> resultado = new List<ComparativoDTO>();
            List<ValorTransferenciaDTO> periodo1 = new List<ValorTransferenciaDTO>();
            List<ValorTransferenciaDTO> periodo2 = new List<ValorTransferenciaDTO>();
            try
            {
                periodo1 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativo(pericodi, version, empcodi, cliemprcodi, barrcodi,
                                                                                flagEntrReti, dias, codigos);

                periodo2 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativo(pericodi2, version2, empcodi, cliemprcodi, barrcodi,
                                                                                flagEntrReti, dias, codigos);

                TimeSpan fecha = new TimeSpan(00, 00, 00, 0);

                decimal contarHora = 1440;


                for (int i = 0; i <= 30; i++)
                {

                    for (int j = 1; j <= 96; j++)
                    {
                        TimeSpan minutos = new TimeSpan(00, 15, 0);
                        fecha = fecha.Add(minutos);

                        decimal contarDia = (contarHora / 1440);
                        contarHora = contarHora + 15;

                        ComparativoDTO obj = new ComparativoDTO();

                        string nomColumn = "VT" + j.ToString();

                        decimal valor1 = 0;
                        if (periodo1.Exists(x => x.ValoTranDia == (i + 1)))
                            valor1 = Convert.ToDecimal(periodo1[i].GetType().GetProperty(nomColumn).GetValue(periodo1[i], null));

                        decimal valor2 = 0;
                        if (periodo2.Exists(x => x.ValoTranDia == (i + 1)))
                            valor2 = Convert.ToDecimal(periodo2[i].GetType().GetProperty(nomColumn).GetValue(periodo2[i], null));

                        obj.valorInicial = valor1;
                        obj.valorFinal = valor2;
                        obj.fecha = fecha.ToString(@"dd\.hh\.mm");
                        obj.variacion = 0;
                        obj.tiempo = Convert.ToDecimal(contarDia.ToString("0.##"));
                        resultado.Add(obj);
                    }

                }


            }
            catch (Exception ex)
            {
                resultado = null;
            }

            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>

        public ResultadoDTO<List<ComparativoDTO>> ListarComparativoEntregaRetiroValoDesviacion(ComparacionEntregaRetirosFiltroDTO parametro)
        {
            ResultadoDTO<List<ComparativoDTO>> resultado = new ResultadoDTO<List<ComparativoDTO>>();
            resultado.Data = new List<ComparativoDTO>();
            parametro.CliCodi = parametro.TipoEntregaCodi == EnumComparativoEntregaRetiros.Entrega ? null : parametro.CliCodi;
            DataTable tblPeriodo1 = new DataTable();

            if (parametro.TipoInfoCodi == 1)
                tblPeriodo1 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativoEntregaRetiroValo(parametro);
            if (parametro.TipoInfoCodi == 2)
                tblPeriodo1 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativoEntregaRetiroValoTransferencia(parametro);

            parametro.PeriCodi1 = parametro.PeriCodi2;
            parametro.VersionCodi1 = parametro.VersionCodi2;
            DataTable tblPeriodo2 = new DataTable();

            if (parametro.TipoInfoCodi == 1)
                tblPeriodo2 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativoEntregaRetiroValo(parametro);
            if (parametro.TipoInfoCodi == 2)
                tblPeriodo2 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativoEntregaRetiroValoTransferencia(parametro);


            mpGraficaGeneradaParaComparativoEntregaDesviacion(tblPeriodo1).ForEach(item =>
            {
                resultado.Data.Add(item);
            });

            mpGraficaGeneradaParaComparativoEntregaDesviacion(tblPeriodo2, true).ForEach(item =>
             {
                 ComparativoDTO entidadPeriodo1 = resultado.Data.Find(x => x.FechaIntervalo.Day.ToString() == item.FechaIntervalo.Day.ToString() && x.Hora == item.Hora);
                 if (entidadPeriodo1 != null)
                 {
                     entidadPeriodo1.EntregaRetiro2 = item.EntregaRetiro2;
                     entidadPeriodo1.Desviacion = decimal.Parse((((entidadPeriodo1.EntregaRetiro2 - entidadPeriodo1.EntregaRetiro1) / entidadPeriodo1.EntregaRetiro1) * 100).ToString("0.00")); //Se modifica fórmula a solicitud de usuario 27/10/2021
                 }
             });



            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public ResultadoDTO<List<ComparativoPeriodosDTO>> ListarComparativoEntregaRetiroValorDET(ComparacionEntregaRetirosFiltroDTO parametro)
        {

            List<ComparativoPeriodosDTO> mesVersion = new List<ComparativoPeriodosDTO>();
            DataTable tblPeriodo1 = new DataTable();
            DataTable tbl2 = new DataTable();
            ResultadoDTO<List<ComparativoPeriodosDTO>> resultado = new ResultadoDTO<List<ComparativoPeriodosDTO>>();
            resultado.Data = new List<ComparativoPeriodosDTO>();

            parametro.CliCodi = parametro.TipoEntregaCodi == EnumComparativoEntregaRetiros.Entrega ? null : parametro.CliCodi;
            #region ObtenerDatos
            if (parametro.TipoInfoCodi == 1)
                tblPeriodo1 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativoEntregaRetiroValorDET(parametro);
            if (parametro.TipoInfoCodi == 2)
                tblPeriodo1 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativoEntregaRetiroValorDETTransferencia(parametro);

            #endregion ObtenerDatos

            parametro.PeriCodi1 = parametro.PeriCodi2;
            parametro.VersionCodi1 = parametro.VersionCodi2;
            DataTable tblPeriodo2 = new DataTable();
            if (parametro.TipoInfoCodi == 1)
                tblPeriodo2 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativoEntregaRetiroValorDET(parametro);
            if (parametro.TipoInfoCodi == 2)
                tblPeriodo2 = FactoryTransferencia.GetValorTransferenciaRepository().ListarComparativoEntregaRetiroValorDETTransferencia(parametro);

            #region Meses

            mpGraficaGeneradaParaComparativoEntregaDesviacionHeader(tblPeriodo1).ForEach((item) =>
            {
                mesVersion.Add(item);
            });
            if (tblPeriodo1.Rows.Count > 0)
            {
                mpGraficaGeneradaParaComparativoEntregaDesviacionHeader(tblPeriodo2).ForEach((item) =>
                {
                    mesVersion.Add(item);
                });

            }

            #endregion Meses


            #region Genera Grafica


            mpGraficaGeneradaParaComparativoEntregaDesviacion(tblPeriodo1, false, true).ForEach(item =>
              {
                  ComparativoPeriodosDTO entidadPeriodo1 = mesVersion.Find(x => x.PERIANIO == item.PeriAnio
                  && x.PERIMES == item.PeriMes
                  && x.VTRANVERSION == item.Version
                  && x.Dia == item.Dia
                  );
                  if (entidadPeriodo1 != null)
                  {
                      ComparativoDTO entidad = new ComparativoDTO();
                      entidad.FechaIntervalo = item.FechaIntervalo;
                      entidad.valorInicial = item.EntregaRetiro1;
                      entidad.Hora = item.Hora;
                      entidadPeriodo1.ListaComparativos.Add(entidad);

                  }

              });

            mpGraficaGeneradaParaComparativoEntregaDesviacion(tblPeriodo2, false, true).ForEach(item =>
              {
                  ComparativoPeriodosDTO entidadPeriodo1 = mesVersion.Find(x => x.PERIANIO == item.PeriAnio
                  && x.PERIMES == item.PeriMes
                  && x.VTRANVERSION == item.Version
                  && x.Dia == item.Dia
                  );
                  if (entidadPeriodo1 != null)
                  {
                      ComparativoDTO entidad = new ComparativoDTO();
                      entidad.FechaIntervalo = item.FechaIntervalo;
                      entidad.valorInicial = item.EntregaRetiro1;
                      entidad.Hora = item.Hora;
                      entidadPeriodo1.ListaComparativos.Add(entidad);
                  }
              });


            mesVersion.ForEach(item =>
            {
                resultado.Data.Add(item);
            });


            #endregion Genera Grafica

            return resultado;

        }


        #region Privates

        private List<ComparativoPeriodosDTO> mpGraficaGeneradaParaComparativoEntregaDesviacionHeader(DataTable tbl)
        {
            List<ComparativoPeriodosDTO> mesVersion = new List<ComparativoPeriodosDTO>();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DateTimeFormatInfo dateTimeInfo = new DateTimeFormatInfo();
                DataRow item = tbl.Rows[i];
                int PeriMes = Convert.ToInt32(item["PERIMES"]);
                int PeriAnio = Convert.ToInt32(item["PERIANIO"]);
                int Version = Convert.ToInt32(item["VTRANVERSION"]);
                var recanombre = Convert.ToString(item["RECANOMBRE"]);
                int Dia = Convert.ToInt32(item["VTRANDIA"]);
                //string Codigo = Convert.ToString(item["TENTCODIGO"]);
                if (mesVersion.Where(c => c.PERIANIO == PeriAnio && c.PERIMES == PeriMes && c.VTRANVERSION == Version
                  && c.Dia == Dia
                ).Count() == 0)
                {
                    string cultureEn = "es-ES";
                    string monthSP = CultureInfo.CreateSpecificCulture(cultureEn).DateTimeFormat.GetAbbreviatedMonthName(PeriMes);
                    mesVersion.Add(new ComparativoPeriodosDTO
                    {
                        PERIMES = PeriMes,
                        PERIANIO = PeriAnio,
                        VTRANVERSION = Version,
                        Dia = Dia,
                        PeriodoMesVersion = string.Format("Día:{2} - {0}.{1}-{3}", PeriAnio, monthSP, Dia, recanombre),
                        //PeriodoMesVersion = string.Format("{0}.{1}", PeriAnio, dateTimeInfo.GetAbbreviatedMonthName(PeriMes)),
                        ListaComparativos = new List<ComparativoDTO>()
                    });
                }
            }

            return mesVersion;

        }
        private List<ComparativoDTO> mpGraficaGeneradaParaComparativoEntregaDesviacion(DataTable tbl, bool esPeriodo2 = false, bool esDetalle = false)
        {
            List<ComparativoDTO> resultado = new List<ComparativoDTO>();

            int columnIndexInit = esDetalle ? 5 : 0;
            int rowIndex = -1;
            foreach (DataRow itemRow in tbl.Rows)
            {
                rowIndex++;

                DateTime fechaEnergia = DateTime.Now;
                if (esDetalle)
                {
                    int PeriMes = Convert.ToInt32(itemRow["PERIMES"]);
                    int PeriAnio = Convert.ToInt32(itemRow["PERIANIO"]);
                    int Version = Convert.ToInt32(itemRow["VTRANVERSION"]);
                    int Dia = Convert.ToInt32(itemRow["VTRANDIA"]);

                    fechaEnergia = new DateTime(PeriAnio, 1, 1);
                }
                else
                    fechaEnergia = Convert.ToDateTime(itemRow["FECHA"]);



                #region Genera Grafica
                int columIndex = -1;
                int intervaloIndice = 0;
                DateTime intervalo = fechaEnergia;
                TimeSpan tiempo = new TimeSpan(00, 00, 0);
                foreach (DataColumn item in tbl.Columns)
                {
                    columIndex++;
                    if (columIndex > columnIndexInit)
                    {
                        intervaloIndice++;
                        if (intervaloIndice == 96)
                            intervalo = intervalo.Add(new TimeSpan(00, 14, 0));
                        else
                            intervalo = intervalo.Add(new TimeSpan(00, 15, 0));

                        ComparativoDTO entidad = new ComparativoDTO();
                        entidad.Hora = intervalo.ToShortTimeString();
                        entidad.FechaIntervalo = intervalo;
                        entidad.Dia = fechaEnergia.Day;


                        if (esDetalle)
                        {
                            entidad.PeriAnio = int.Parse(tbl.Rows[rowIndex]["PERIANIO"].ToString());
                            entidad.PeriMes = int.Parse(tbl.Rows[rowIndex]["PERIMES"].ToString());
                            entidad.Version = int.Parse(tbl.Rows[rowIndex]["VTRANVERSION"].ToString());
                            entidad.Dia = int.Parse(tbl.Rows[rowIndex]["VTRANDIA"].ToString());
                        }

                        if (!esPeriodo2)
                            entidad.EntregaRetiro1 = Convert.ToDecimal(tbl.Rows[rowIndex][columIndex].ToString());
                        else if (esPeriodo2)
                            entidad.EntregaRetiro2 = Convert.ToDecimal(tbl.Rows[rowIndex][columIndex].ToString());



                        //decimal desviacion = 0;
                        //if (entidad.EntregaRetiro1 > 0 && entidad.EntregaRetiro2 > 0)
                        //    desviacion = ((entidad.EntregaRetiro2 - entidad.EntregaRetiro1) / entidad.EntregaRetiro2) * 100;
                        //entidad.Desviacion = decimal.Parse(desviacion.ToString("0.00"));
                        resultado.Add(entidad);
                    }
                }
                #endregion Genera Grafica

            }



            return resultado;
        }


        #endregion Privates

    }

}
