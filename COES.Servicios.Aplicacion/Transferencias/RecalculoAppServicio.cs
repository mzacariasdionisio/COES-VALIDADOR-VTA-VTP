using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System.Data;
using System.Data.Common;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class RecalculoAppServicio: AppServicioBase
    {
        //Declaración de servicios
        FactorPerdidaAppServicio servicioFactorPerdida = new FactorPerdidaAppServicio();
        CostoMarginalAppServicio servicioCostoMarginal = new CostoMarginalAppServicio();
        CodigoEntregaAppServicio servicioTrnCodigoEntrega = new CodigoEntregaAppServicio();
        TransferenciaEntregaRetiroAppServicio servicioTransEntregaRetiro = new TransferenciaEntregaRetiroAppServicio();
        ValorTransferenciaAppServicio servicioValorTransferencia = new ValorTransferenciaAppServicio();

        /// <summary>
        /// Graba o actualiza un recalculo
        /// </summary>
        /// <param name="entity">RecalculoDTO</param>
        /// <returns>Código nuevo o actuazalizado de la tabla TRN_RECALCULO</returns> 
        public int SaveOrUpdateRecalculo(RecalculoDTO entity)        
        {
            try
            {
                int id = 0;

                if (entity.RecaCodi == 0)
                {
                    id = FactoryTransferencia.GetRecalculoRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetRecalculoRepository().Update(entity);
                    id = entity.RecaCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_RECALCULO en base a su iRecaCodi
        /// </summary>
        /// <param name="iRecaCodi">Código de la tabla TRN_RECALCULO</param>
        /// <returns>Código del registro eliminado</returns> 
        public int DeleteRecalculo(int iPeriCodi,int iRecaCodi)
        {
            try 
            {
                FactoryTransferencia.GetRecalculoRepository().Delete(iPeriCodi, iRecaCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iRecaCodi;
        }

        public int DeleteRepartoRnd(int iPeriCodi, int iRecaCodi)
        {
            try
            {
                FactoryTransferencia.GetTransferenciaRentaCongestionRepository().DeleteDatosReparto(iPeriCodi, iRecaCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iRecaCodi;
        }

        /// <summary>
        /// Permite obtener la lista de recálculos del periodo
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iRecaCodi">Versión del Recálculo</param>
        /// <returns></returns>
        public RecalculoDTO GetByIdRecalculo(int iPerCodi, int iRecaCodi)
        {
            return FactoryTransferencia.GetRecalculoRepository().GetById(iPerCodi, iRecaCodi);
        }

        /// <summary>
        /// Permite obtener recalculo en base al iPerCodi
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <returns>Lista de RecalculoDTO</returns>
        public List<RecalculoDTO> ListRecalculos(int iPerCodi)
        {
            return FactoryTransferencia.GetRecalculoRepository().List(iPerCodi);
        }

        #region PrimasRER.2023

        /// <summary>
        /// Permite obtener recalculo en base al iPerCodi
        /// </summary>
        /// <param name="anio">Año del recalculo</param>
        /// /// <param name="mes">Mes del recalculo</param>
        /// <returns>Lista de RecalculoDTO</returns>
        public List<RecalculoDTO> ListRecalculosByAnioMes(int anio, int mes)
        {
            return FactoryTransferencia.GetRecalculoRepository().ListByAnioMes(anio, mes);
        }

        /// <summary>
        /// Permite obtener recalculo en base al iPerCodi
        /// </summary>
        /// <param name="anio">Año del recalculo</param>
        /// /// <param name="mes">Mes del recalculo</param>
        /// <returns>Lista de RecalculoDTO segun VTEA</returns>
        public List<RecalculoDTO> ListVTEAByAnioMes(int anio, int mes)
        {
            return FactoryTransferencia.GetRecalculoRepository().ListVTEAByAnioMes(anio, mes);
        }

        #endregion

        /// <summary>
        /// Permite obtener recalculo ('Publicar', 'Cerrado') en base al iPerCodi
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <returns>Lista de RecalculoDTO</returns>
        public List<RecalculoDTO> ListRecalculosEstadoPublicarCerrado(int iPerCodi)
        {
            return FactoryTransferencia.GetRecalculoRepository().ListEstadoPublicarCerrado(iPerCodi);
        }

        /// <summary>
        /// Permite buscar recalculo en base a su nombre
        /// </summary>
        /// <param name="sRecaNombre">Nombre del recalculo</param>
        /// <returns>Lista de RecalculoDTO</returns>
        public List<RecalculoDTO> BuscarRecalculo(string sRecaNombre)
        {
            return FactoryTransferencia.GetRecalculoRepository().GetByCriteria(sRecaNombre);
        }

        /// <summary>
        /// Permite buscar el ultimo recalculo en base a su periodo
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <returns>Retorna el Código del último recalculo</returns>
        public int GetUltimaVersion(int iPeriCodi)
        {
            return FactoryTransferencia.GetRecalculoRepository().GetUltimaVersion(iPeriCodi);
        }

        /// <summary>
        /// Permite Listar los Recalculos de la tablas TRN_TRANS_ENTREGA/RETIRO, por periodo y empresa
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <returns>Retorna el Código del último recalculo</returns>
        public List<RecalculoDTO> ListRecalculosTrnCodigoEnviado(int pericodi, int emprcodi)
        {
            return FactoryTransferencia.GetRecalculoRepository().ListRecalculosTrnCodigoEnviado(pericodi, emprcodi);
        }

        /// <summary>
        /// Permite copiar las tablas asociados al recalculo Old a New
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        public void CopiarRecalculo(int iPeriCodi, int iVersionNew, int iVersionOld)
        {
            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactoryTransferencia.GetRecalculoRepository().BeginConnection();
                tran = FactoryTransferencia.GetRecalculoRepository().StartTransaction(conn);
                //---------------------------------------------------------------------------------------------------------
                //Duplicamos la información de la tabla TRN_FACTOR_PERDIDA y TRN_COSTO_MARGINAL a la nueva versión                   
                int iFacPerCodi = servicioFactorPerdida.GetCodigoGeneradoDec();
                int iCostMargCodi = servicioCostoMarginal.GetCodigoGeneradoDec();
                //PLSql que se encarga de ejecutar un Insert as Select
                //
                this.servicioFactorPerdida.CopiarFactorPerdidaCostoMarginal(iPeriCodi, iVersionOld, iVersionNew, iFacPerCodi, iCostMargCodi, conn, tran);
                //---------------------------------------------------------------------------------------------------------
                //Duplicamos la información de la tabla TRN_TRANS_ENTREGA (TENTCODI, PERICODI, TENTVERSION) a la nueva versión
                int iTransEntrCodi = this.servicioTransEntregaRetiro.GetCodigoGeneradoTransEntrDesc();
                int iTransEntrDetCodi = -1;
                //PLSql que se encarga de ejecutar un Insert as Select
                this.servicioTransEntregaRetiro.CopiarEntregas(iPeriCodi, iVersionOld, iVersionNew, iTransEntrCodi, iTransEntrDetCodi, conn, tran);
                //---------------------------------------------------------------------------------------------------------
                //Duplicamos la información de la tabla TRN_TRANS_RETIRO(TRETCODI, PERICODI, TRETVERSION) a la nueva versión
                int iTranRetiCodi = this.servicioTransEntregaRetiro.GetCodigoGeneradoTransRetDesc();
                int iTranRetiDetaCodi = -1;
                //PLSql que se encarga de ejecutar un Insert as Select
                this.servicioTransEntregaRetiro.CopiarRetiros(iPeriCodi, iVersionOld, iVersionNew, iTranRetiCodi, iTranRetiDetaCodi, conn, tran);

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                throw new Exception(ex.Message, ex);
            }
        }

        //ASSETEC 202108 - TIEE
        /// <summary>
        /// Permite Lista de Periodos con la ultima versión de recalculo
        /// </summary>
        /// <returns>Lista de VtpRecalculoPotenciaDTO</returns>
        public List<RecalculoDTO> ListMaxRecalculoByPeriodo()
        {
            return FactoryTransferencia.GetRecalculoRepository().ListMaxRecalculoByPeriodo();
        }

        /// <summary>
        /// Permite migrar los saldos de VTEA de la empresaorigen hacia empresadestino en un periodo y revisióm
        /// </summary>
        /// <param name="emprcodiorigen">Empresa de origen de los saldos</param>
        /// <param name="emprcodidestino">Empresa de destino de los saldos</param>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="recacodi">Versión del recalculo</param>
        /// <param name="sMensaje">Mensaje de error</param>
        /// <param name="sDetalle">Detalle del error</param>
        /// <returns></returns>
        public string MigrarSaldosVTEA(int emprcodiorigen, int emprcodidestino, int pericodi, int recacodi, out string sMensaje, out string sDetalle)
        {   string sSql = "";
            try
            {
                sMensaje = "";
                sDetalle = "";
                sSql = FactoryTransferencia.GetRecalculoRepository().MigrarSaldosVTEA(emprcodiorigen, emprcodidestino, pericodi, recacodi);
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                sDetalle = ex.StackTrace;
            }
            return sSql;
        }


        /// <summary>
        /// Permite migrar la información de VTEA de la empresa origen => destino de un periodo y versión de recalculo
        /// </summary>
        /// <param name="emprcodiorigen">Empresa de origen de los saldos</param>
        /// <param name="emprcodidestino">Empresa de destino de los saldos</param>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="recpotcodi">Versión del recalculo</param>
        /// <param name="sMensaje">Mensaje de error</param>
        /// <param name="sDetalle">Detalle de error</param>
        /// <returns></returns>
        public string MigrarCalculoVTEA(int emprcodiorigen, int emprcodidestino, int pericodi, int recpotcodi, out string sMensaje, out string sDetalle)
        {
            string sSql = "";
            try
            {
                sMensaje = "";
                sDetalle = "";
                sSql = FactoryTransferencia.GetRecalculoRepository().MigrarCalculoVTEA(emprcodiorigen, emprcodidestino, pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                sDetalle = ex.StackTrace;
            }
            return sSql;
        }
    }
}
