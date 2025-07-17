using COES.Base.Core;
using COES.Dominio.DTO;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Servicios.Aplicacion.Transferencias
{
    public class TransferenciaInformacionBaseAppServicio : AppServicioBase
    {
        //INFORMACION BASE
        /// <summary>
        /// Graba o actualiza una entidad TransferenciaInformacionBaseDTO
        /// </summary>
        /// <param name="entity">TransferenciaInformacionBaseDTO</param>
        /// <returns>Código nuevo o actualizado de la tabla TRN_TRANS_INFOBASE</returns>
        public int SaveOrUpdateTransferenciaInformacionBase(TransferenciaInformacionBaseDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TinfbCodi == 0)
                {
                    id = FactoryTransferencia.GetTransferenciaInformacionBaseRepository().Save(entity);
                }
                else
                {
                    entity.TinfbEstado = "INA";
                    if (entity.TinfbCodi == -1)
                    {
                        //coloca en inactivo la información en el periodo, recacodi y emprcodi
                        FactoryTransferencia.GetTransferenciaInformacionBaseRepository().Update(entity);
                    }
                    else if (entity.TinfbCodi == -2)
                    {
                        //coloca en inactivo la información en el periodo, recacodi, emprcodi y codigo
                        FactoryTransferencia.GetTransferenciaInformacionBaseRepository().UpdateCodigo(entity);
                    }
                    id = entity.TinfbCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un listado de registros de la tabla TRN_TRANS_INFOBASE en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>     
        /// <returns>Retorna la Versión del Mes de valorización</returns>
        public int DeleteTransferenciaInfoInformacionBase(int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaInformacionBaseRepository().Delete(iPeriCodi, iTEntVersion, sCodigo);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTEntVersion;
        }

        /// <summary>
        /// Permite listar los codigo de informacion base en relacion empresa ,periodo y version
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>     
        /// <returns>Lista de TransferenciaEntregaDTO</returns>
        public List<TransferenciaInformacionBaseDTO> ListInformacionBase(int iEmprCodi, int iPeriCodi, int iTEntVersion)
        {
            return FactoryTransferencia.GetTransferenciaInformacionBaseRepository().List(iEmprCodi, iPeriCodi, iTEntVersion);
        }

        /// <summary>
        /// Permite listar las Informacion Base
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>
        /// <returns>Lista de TransferenciaInformacionBaseDTO</returns>
        public List<TransferenciaInformacionBaseDTO> ListByPeriodoVersionIB(int iPeriCodi, int iTEntVersion)
        {
            return FactoryTransferencia.GetTransferenciaInformacionBaseRepository().ListByPeriodoVersion(iPeriCodi, iTEntVersion);
        }

        /// <summary>
        /// Elimina un listado de TransferenciaInformacionBase
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>
        /// <returns>Retorna la Versión del Mes de valorización</returns>
        public int DeleteListaTransferenciaInformacionBase(int iPeriCodi, int iTEntVersion)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaInformacionBaseRepository().DeleteListaTransferenciaInfoBase(iPeriCodi, iTEntVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTEntVersion;
        }

        /// <summary>
        /// Permite Obtner la Informacion Base a partir del código
        /// </summary>
        /// <param name="iEmprCodi">Código de la empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>
        /// <param name="sCodigo">Código reportado</param>
        /// <returns>Lista de TransferenciaInformacionBaseDTO</returns>
        public TransferenciaInformacionBaseDTO GetTransferenciaInfoBaseByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion,string sCodigo)
        {
            return FactoryTransferencia.GetTransferenciaInformacionBaseRepository().GetTransferenciaInfoBaseByCodigo(iEmprCodi, iPeriCodi, iTEntVersion, sCodigo);
        }

        /// <summary>
        /// Permite Obtner la Informacion Base a partir del código
        /// </summary>
        /// <param name="iEmprCodi">Código de la empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recálculo</param>
        /// <param name="sCodigo">Código reportado</param>
        /// <returns>Lista de TransferenciaInformacionBaseDTO</returns>
        public TransferenciaInformacionBaseDTO GetTransferenciaInfoBaseByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo)
        {
            return FactoryTransferencia.GetTransferenciaInformacionBaseRepository().GetTransferenciaInfoBaseByCodigoEnvio(iEmprCodi, iPeriCodi, iTEntVersion, trnenvcodi, sCodigo);
        }
        //TRANSFERENCIA IFORMACION BASE DETALLE-------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Graba o actualiza una entidad TransferenciaInformacionBaseDetalleDTO
        /// </summary>
        /// <param name="entity">TransferenciaEntregaDetalleDTO</param>
        /// <returns>Código nuevo o actualizado de la tabla TRN_TRANS_INFOBASE_DETALLE</returns>
        public int SaveOrUpdateTransferenciaInformacionBaseDetalle(TransferenciaInformacionBaseDetalleDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TinfbDeCodi == 0)
                {
                    id = FactoryTransferencia.GetTransferenciaInformacionBaseDetalleRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetTransferenciaInformacionBaseDetalleRepository().Update(entity);
                    id = 1; // entity.TinfbDeCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite listar Transferencia InfoBase Detalle en base a empresa, periodo
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <returns>TransferenciaEntregaDetalleDTO</returns>
        public List<TransferenciaInformacionBaseDetalleDTO> ListTransferenciaInformacionBaseDetalle(int iEmprCodi, int iPeriCodi)
        {
            return FactoryTransferencia.GetTransferenciaInformacionBaseDetalleRepository().List(iEmprCodi, iPeriCodi);
        }


        /// <summary>
        /// Permite realizar búsquedas de  Transferencia Informacion Base Detalle en base al emprcodi, pericodi, codientrcodi y version
        /// </summary>
        /// <param name="iEmprCodi">Código de Empresa</param>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="sTEntCodigo">Código de Entrega</param>
        /// <param name="iTEntVersion">Versión de Recalculo</param>
        /// <returns>Lista de TransferenciaInformacionBaseDetalleDTO</returns>
        public List<TransferenciaInformacionBaseDetalleDTO> BuscarTransferenciaInformacionBaseDetalle(int iEmprCodi, int iPeriCodi, string sTEntCodigo, int iTEntVersion)
        {
            return FactoryTransferencia.GetTransferenciaInformacionBaseDetalleRepository().GetByCriteria(iEmprCodi, iPeriCodi, sTEntCodigo, iTEntVersion);
        }

        /// <summary>
        /// Elimina un listado de registros de la tabla trn_trans_infobase_detalle
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recalculo</param>
        /// <returns>Código del recalculo</returns>
        public int DeleteTransferenciaInformacionBaseDetalle(int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaInformacionBaseDetalleRepository().Delete(iPeriCodi, iTEntVersion, sCodigo);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTEntVersion;
        }

        /// <summary>
        /// Elimina un listado de TransferenciaInformacionBaseDetalle
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recalculo</param>
        /// <returns>Código del recalculo</returns>
        public int DeleteListaTransferenciaInformacionBaseDetalle(int iPeriCodi, int iTEntVersion)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaInformacionBaseDetalleRepository().DeleteListaTransferenciaInfoBaseDetalle(iPeriCodi, iTEntVersion);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iTEntVersion;
        }

        /// <summary>
        /// Permite listar los  codigo de infobase  por periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iTEntVersion">Versión de Recalculo</param>
        /// <returns>Lista de TransferenciaInformacionBaseDetalleDTO</returns>
        public List<TransferenciaInformacionBaseDetalleDTO> ListTransferenciaInforacionBaseDetallePeriVer(int iPeriCodi, int iTEntVersion)
        {
            return FactoryTransferencia.GetTransferenciaInformacionBaseDetalleRepository().GetByPeriodoVersion(iPeriCodi, iTEntVersion);
        }

        ///// <summary>
        ///// Permite öbtener el balance de energiaActiva en base al periodo
        ///// </summary>
        ///// <param name="iPeriCodi">Código de Periodo</param>
        ///// <returns>Lista de TransferenciaEntregaDetalleDTO</returns>
        //public List<TransferenciaEntregaDetalleDTO> ListBalanceEnergiaActiva(int iPeriCodi, int version)
        //{
        //    return FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().BalanceEnergiaActiva(iPeriCodi, version);
        //}


        /// <summary>
        /// Permite listar las transferencia de info base detalle
        /// </summary>
        /// <param name="iTEntCodi">Codigo de la tabla TRN_TRANS_ENTREGA</param>
        /// <returns>Lista de TransferenciaEntregaDetalleDTO</returns>
        public List<TransferenciaInformacionBaseDetalleDTO> ListByTransferenciaInformacionBase(int iTEntCodi)
        {
            return FactoryTransferencia.GetTransferenciaInformacionBaseDetalleRepository().ListByTransferenciaInfoBase(iTEntCodi);
        }

        #region Demanda Mercado Libre

        public List<DemandaMercadoLibreDTO> ListDemandaMercadoLibre(DateTime[] periodos, DateTime periodoMes, int tipoEmpresa, string empresas, int codigoLectura, int codigoOrigenLectura)
        {
            return FactoryTransferencia.GetDemandaMercadoLibreRepository().ListDemandaMercadoLibre(periodos, periodoMes, tipoEmpresa, empresas, codigoLectura, codigoOrigenLectura);
        }
        #endregion

        #region Rentas Congestion
        public List<TransferenciaRecalculoDTO> ListPeriodosRentaCongestion(int pericodi, int recacodi, int regIni, int regFin)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListPeriodosRentaCongestion(pericodi, recacodi, regIni, regFin);
        }

        public int ListPeriodosRentaCongestionCount()
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListPeriodosRentaCongestionCount();
        }

        public int CalcularRentasCongestionPeriodo(int pericodi, int recacodi, string nombreUsuario)
        {
            var resultado = 1;
            try
            {
                var resCongestion = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosRentaCongestion(pericodi, recacodi);
                var resEntrega = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosEntrega(pericodi, recacodi);
                var resRetiro = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosRetiro(pericodi, recacodi);
                var resPeriodo = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosPeriodo(pericodi, recacodi);

                var perioaniomes = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetPeriodoMes(pericodi);
                var rcentdcodi = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoEntregaId();
                var rccretdcodi = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoRetiroId();
                var rcrpercodi = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoPeriodoId();

                var periodo = Convert.ToString(perioaniomes);
                int mesIni = Int32.Parse(periodo.Substring(4, 2));
                int anioIni = Int32.Parse(periodo.Substring(0, 4));

                var diasPeriodo = DateTime.DaysInMonth(anioIni, mesIni);

                var insertRegistrosEntrega = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveDetalleEntrega(pericodi, recacodi, rcentdcodi, perioaniomes, diasPeriodo);
                var insertRegistrosRetiro = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveDetalleRetiro(pericodi, recacodi, rccretdcodi, perioaniomes, diasPeriodo);
                
                //ASSETEC 202210 - Ajustar intervalos de 15 y 45 minutos.
                var ajustarDetalleEntregaEntrega = AjustarDetalleEntregaRetiro(pericodi, recacodi, rcentdcodi, rccretdcodi, nombreUsuario);
                //FIN ASSETEC

                var insertRegistrosPeriodo = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveRentaPeriodo(pericodi, recacodi, rcrpercodi, nombreUsuario);
                var insertRegistrosCongestion = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveRentaCongestionRetiro(pericodi, recacodi, nombreUsuario);
                
                //seccion Rentas de Reparto

                var res = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosReparto(pericodi, recacodi);
                var rcrrndcodi = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoRepartoId();
                var periodoVersionReparto = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetPeriodoVersionReparto(pericodi, recacodi);

                var totalReparto = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetTotalReparto(pericodi, recacodi);

                var porc_pericodi = 0;
                var porc_version = 0;
                if (periodoVersionReparto.Count > 1)
                {
                    porc_pericodi = periodoVersionReparto[0];
                    porc_version = periodoVersionReparto[1];
                }

                if (totalReparto > 0)
                {
                    var insertRegistrosReparto = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveDetalleReparto(rcrrndcodi, pericodi,
                        recacodi, totalReparto, nombreUsuario, porc_pericodi, porc_version);
                }

                var ingcomcodi = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoIngresoCompensacionId();
                var cabcomcodi = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoCabeceraCompensacionId(pericodi);

                if (cabcomcodi > 0)
                {
                    var resIngresoCompensacion = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosingresocompensacion(pericodi, recacodi, cabcomcodi);
                    var insertRegistrosIngresosComp = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveRentaCongestionIngresoCompensacion(pericodi, recacodi, 
                        ingcomcodi, cabcomcodi, nombreUsuario);
                }

                //FactoryTransferencia.GetTransferenciaRentaCongestionRepository().CalcularRentasCongestionPeriodo(pericodi, recacodi, nombreUsuario);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        public int DeleteDatosRentaCongestion(int pericodi, int recacodi)
        {
            var resultado = 0;
            try
            {
                resultado = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosRentaCongestion(pericodi, recacodi);                

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        public int DeleteDatosEntrega(int pericodi, int recacodi)
        {
            var resultado = 0;
            try
            {
                resultado = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosEntrega(pericodi, recacodi);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        public int DeleteDatosRetiro(int pericodi, int recacodi)
        {
            var resultado = 0;
            try
            {
                resultado = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosRetiro(pericodi, recacodi);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        public int DeleteDatosPeriodo(int pericodi, int recacodi)
        {
            var resultado = 0;
            try
            {
                resultado = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosPeriodo(pericodi, recacodi);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        public int SaveDetalleEntrega(int pericodi, int recacodi, int rcentdcodi, int perianiomes, int diasPeriodo)
        {
            var resultado = 0;
            try
            {
                resultado = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveDetalleEntrega(pericodi, recacodi, rcentdcodi, perianiomes, diasPeriodo);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        public int SaveDetalleRetiro(int pericodi, int recacodi, long rccretdcodi, int perianiomes, int diasPeriodo)
        {
            var resultado = 0;
            try
            {
                resultado = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveDetalleRetiro(pericodi, recacodi, rccretdcodi, perianiomes, diasPeriodo);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        public int SaveRentaPeriodo(int pericodi, int recacodi, int rcrpercodi, string nombreUsuario)
        {
            var resultado = 0;
            try
            {
                resultado = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveRentaPeriodo(pericodi, recacodi, rcrpercodi, nombreUsuario);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        public int SaveRentaCongestionRetiro(int pericodi, int recacodi, string nombreUsuario)
        {
            var resultado = 0;
            try
            {
                resultado = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveRentaCongestionRetiro(pericodi, recacodi, nombreUsuario);

            }
            catch (Exception ex)
            {
                resultado = -1;

                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        public int GetPeriodoMes(int pericodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetPeriodoMes(pericodi);
        }
        public int GetMaximoEntregaId()
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoEntregaId();
        }
        public long GetMaximoRetiroId()
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoRetiroId();
        }
        public int GetMaximoPeriodoId()
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoPeriodoId();
        }
        public decimal GetTotalRentaCongestion(int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetTotalRentaCongestion(pericodi, recacodi);
        }

        public decimal GetTotalRentaNoAsignada(int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetTotalRentaNoAsignada(pericodi, recacodi);
        }

        public List<TransferenciaRentaCongestionDTO> ListRentaCongestion(int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListRentaCongestion(pericodi, recacodi);
        }

        public List<TransferenciaRentaCongestionDTO> ListRentaCongestionDetalle(int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListRentaCongestionDetalle(pericodi, recacodi);
        }

        public int GetTotalPorcentajes(int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetTotalPorcentajes(pericodi, recacodi);
        }
        public List<TransferenciaRentaCongestionDTO> ListErroresBarras(int pericodi, int recacodi, int perianiomes)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListErroresBarras(pericodi, recacodi, perianiomes);
        }

        public System.Data.IDataReader ListCostosMarginales(int pericodi, int recacodi, int perianiomes)
        {
            try
            {
                return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListCostosMarginales(pericodi, recacodi, perianiomes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public System.Data.IDataReader ListEntregasRetiros(int pericodi, int recacodi, int perianiomes, int ultimoDia)
        {
            try
            {
                return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListEntregasRetiros(pericodi, recacodi, perianiomes, ultimoDia);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public System.Data.IDataReader ListCostosMarginalesPorBarra(int pericodi, int recacodi, int perianiomes)
        {
            try
            {
                return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListCostosMarginalesPorBarra(pericodi, recacodi,perianiomes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public System.Data.IDataReader ListTotalRegistrosCostosMarginales(int pericodi, int recacodi)
        {
            try
            {
                return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ListTotalRegistrosCostosMarginales(pericodi, recacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Rentas Congestion v2

        public List<RcgCostoMarginalCabDTO> ListRcgCostoMarginalCab(int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetRcgCostoMarginalCabRepository().ListCostoMarginalCab(pericodi, recacodi);
        }

        public int SaveRcgCostoMarginalCab(RcgCostoMarginalCabDTO rcgCostoMarginalCabDTO)
        {
            return FactoryTransferencia.GetRcgCostoMarginalCabRepository().Save(rcgCostoMarginalCabDTO);
        }

        public int UpdateRcgCostoMarginalCab(RcgCostoMarginalCabDTO rcgCostoMarginalCabDTO)
        {
            return FactoryTransferencia.GetRcgCostoMarginalCabRepository().Update(rcgCostoMarginalCabDTO);
        }

        public int DeleteDatosRcgCostoMarginalDet(int rccmgccodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosRcgCostoMarginalDet(rccmgccodi);
        }

        public int DeleteDatosRcgCostoMarginalCab(int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosRcgCostoMarginalCab(pericodi, recacodi);
        }

        public int GetMaximoCostoMarginalDetalleId()
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().GetMaximoCostoMarginalDetalleId();
        }

        public int SaveCostoMarginalDetalleSEV(int rccmgdcodi, int rccmgccodi, int perioaniomes)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveCostoMarginalDetalleSEV(rccmgdcodi, rccmgccodi, perioaniomes);
        }

        public int SaveCostoMarginalDetalleCalculoAnterior(int rccmgdcodi, int rccmgccodi, int pericodi, int recacodiAnterior)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().SaveCostoMarginalDetalleCalculoAnterior(rccmgdcodi, rccmgccodi, pericodi, recacodiAnterior);
        }

        public int ValidaCostoMarginal(int pericodi, int recacodi)
        {
            return FactoryTransferencia.GetTransferenciaRentaCongestionRepository().ValidaCostoMarginal(pericodi, recacodi);
        }


        #endregion

        #region ASSETEC 202210 - Ajustar intervalos de 15 y 45 minutos.

        public string AjustarDetalleEntregaRetiro(int pericodi, int recacodi, int rcentdcodi, long rccretdcodi, string suser)
        {
            string indicador = "1";
            try
            {
                //Traemos la lista de ajustes del periodo
                List<TrnCostoMarginalAjusteDTO> listaAjuste = FactoryTransferencia.GetTrnCostoMarginalAjusteRepository().ListByPeriodoVersion(pericodi, recacodi);
                //---------------------------------------------------------------------------------------------------------
                foreach (TrnCostoMarginalAjusteDTO dtoAjuste in listaAjuste)
                {
                    DateTime dTrncmafecha = dtoAjuste.Trncmafecha;
                    // dTrncmafecha -> pe. 2022-07-03 10:15
                    DateTime dRcEntRetHora = dTrncmafecha.AddMinutes(-15); //-> pe. 2022-07-03 10:00
                    string sTrncmafecha = dTrncmafecha.ToString("dd/MM/yyyy HH:mm:ss");
                    string sRcEntRetHora = dRcEntRetHora.ToString("dd/MM/yyyy HH:mm:ss");

                    string sDia = dTrncmafecha.ToString("dd");
                    string sHoraMinuto = dTrncmafecha.ToString("HH:mm");
                    if (sDia == "01" && sHoraMinuto == "00:15")
                    {
                        //Se disminute en un dia del MES ANTERIOR
                        PeriodoDTO dtoPeriodo = (new PeriodoAppServicio()).BuscarPeriodoAnterior(pericodi);
                        int iVersionAnterior = (new RecalculoAppServicio()).GetUltimaVersion(dtoPeriodo.PeriCodi);
                        //Traemos la lista de TENTCODI de rcg_trans_entrega_rendet para rcentdcodi >= :rcentdcodi
                        List<TransferenciaEntregaDTO> listaEntregas = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().listRGCEntregas(rcentdcodi);
                        foreach (TransferenciaEntregaDTO dtoEntrega in listaEntregas)
                        {
                            //Solo ajusta el intervalo dia:01 00:15 para un tentcodi
                            FactoryTransferencia.GetTransferenciaRentaCongestionRepository().AjustarRGCEntregasDiaAnterior(dtoEntrega.TranEntrCodi, dtoPeriodo.PeriCodi, iVersionAnterior, sTrncmafecha, sRcEntRetHora);
                        }

                        //Traemos la lista de TRETCODI de rcg_trans_retiro_rendet para rccretdcodi >= :rccretdcodi
                        List<TransferenciaRetiroDTO> listaRetiros = FactoryTransferencia.GetTransferenciaRentaCongestionRepository().listRGCRetiros(rccretdcodi);
                        foreach (TransferenciaRetiroDTO dtoRetiro in listaRetiros)
                        {
                            //Solo ajusta el intervalo dia:01 00:15 para un tretcodi
                            FactoryTransferencia.GetTransferenciaRentaCongestionRepository().AjustarRGCRetirosDiaAnterior(dtoRetiro.TranRetiCodi, dtoPeriodo.PeriCodi, iVersionAnterior, sTrncmafecha, sRcEntRetHora);
                        }
                    }
                    else
                    {
                        // Mayores a 00:45, 01:15, 01:45, 02:15, 02:45, 03:15, 03:45, 04:15, 04:45.... 
                        // Para los Intervalos, dia:02 00:15, toma el valor del dia:02 00:00
                        //Ejecutamos la actualización de las RCG de las Entregas y Retiros
                        FactoryTransferencia.GetTransferenciaRentaCongestionRepository().AjustarRGCEntregasRetiros(rcentdcodi, rccretdcodi, sTrncmafecha, sRcEntRetHora);
                    }
                    
                }
                //Actualizar los Intervalos con el usuario y Datetime de Actualización
                FactoryTransferencia.GetTransferenciaRentaCongestionRepository().UpdateRGCEntregasRetiros(rcentdcodi, rccretdcodi);

            }
            catch (Exception e)
            {
                indicador = e.StackTrace;
            }
            return indicador;
        }

        #endregion FIN ASSETEC
    }
}
