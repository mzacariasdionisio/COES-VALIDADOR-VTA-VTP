using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System.IO;
using System.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;
using System.Data;
using System.Data.Common;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class FactorPerdidaAppServicio: AppServicioBase
    {
        //Declaración de servicios
        CostoMarginalAppServicio servicioCostoMarginal = new CostoMarginalAppServicio();
        TransferenciaInformacionBaseAppServicio servicioRentaCongestion = new TransferenciaInformacionBaseAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        
        /// <summary>
        /// Permite insertar o actualizar la información FactorPerdida en base  a la entidad
        /// </summary>
        /// <param name="entity">Entidad FactorPerdidaDTO</param>
        /// <returns>FactPerdCodi</returns>
        public int SaveFactorPerdida(FactorPerdidaDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.FacPerCodi == 0)
                {
                     id = FactoryTransferencia.GetFactorPerdidaRepository().Save(entity);
                }
           
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina Todos los Factores de perdida en base al periodo y version
        /// </summary>
        /// <param name="PeriCodi"></param>
        /// <param name="FactPerdVersion"></param>
        /// <returns></returns>
        public int DeleteListaFactorPerdida(int PeriCodi, int FactPerdVersion)
        {
            try
            {
                FactoryTransferencia.GetFactorPerdidaRepository().Delete(PeriCodi, FactPerdVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return PeriCodi;
        }

        /// <summary>
        /// Permite listar todas los Factores de Perdida de un periodo y versión
        /// </summary>
        /// <returns></returns>
        public List<FactorPerdidaDTO> ListByPeriodoVersion(int iPeriCodi, int iFactPerdVersion)
        {
            return FactoryTransferencia.GetFactorPerdidaRepository().ListByPeriodoVersion(iPeriCodi, iFactPerdVersion);
        }

        /// <summary>
        /// Inserta de forma masiva una lista de FactorPerdidaDTO
        /// </summary>
        /// <param name="entitys">FactorPerdidaDTO</param>    
        public void BulkInsertValorFactorPerdida(List<TrnFactorPerdidaBullkDTO> entitys)
        {
            try
            {
                FactoryTransferencia.GetFactorPerdidaRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el siguiente Primary Key de la tabla
        /// </summary>
        public int GetCodigoGenerado()
        {
            return FactoryTransferencia.GetFactorPerdidaRepository().GetCodigoGenerado();
        }

        /// <summary>
        /// Permite obtener el siguiente en menor valor del Primary Key de la tabla
        /// </summary>
        public int GetCodigoGeneradoDec()
        {
            return FactoryTransferencia.GetFactorPerdidaRepository().GetCodigoGeneradoDec();
        }

        /// <summary>
        /// Permite copiar los Factores de Perdida y Costos Marginales de un periodo de una versión anterior a la nueva versión
        /// </summary>
        /// <param name="iPeriCodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="iVersionOld">Versión anterior del recalculo</param>
        /// <param name="iVersionNew">Versión siguiente del recalculo</param>
        /// <param name="iFacPerCodi">Identificador minimo de la Tabla Factor de Perdida</param>
        /// <param name="iCostMargCodi">Identificador minimo de la Tabla Costo Marginal</param>
        /// <returns></returns>
        public void CopiarFactorPerdidaCostoMarginal(int iPeriCodi, int iVersionOld, int iVersionNew, int iFacPerCodi, int iCostMargCodi, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactoryTransferencia.GetFactorPerdidaRepository().CopiarFactorPerdidaCostoMarginal(iPeriCodi, iVersionOld, iVersionNew, iFacPerCodi, iCostMargCodi, conn, tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite copiar los Factores de Perdida desde la tabla SI_COSTOMARGINAL
        /// </summary>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="version">Versión anterior del recalculo</param>
        /// <returns></returns>
        public string CopiarSGOCOES(int pericodi, int version, string suser, string sAnioMes)
        {
            string indicador = "1";
            IDbConnection conn = null;
            DbTransaction tran = null;
            try
            {
                conn = FactoryTransferencia.GetRecalculoRepository().BeginConnection();
                tran = FactoryTransferencia.GetRecalculoRepository().StartTransaction(conn);
                //---------------------------------------------------------------------------------------------------------
                //Duplicamos la información de la tabla TRN_FACTOR_PERDIDA y TRN_COSTO_MARGINAL a la nueva versión                   
                int iFacPerCodi = this.GetCodigoGenerado();
                int iCostMargCodi = servicioCostoMarginal.GetCodigoGenerado();
                //PLSql que se encarga de ejecutar un Insert as Select
                FactoryTransferencia.GetFactorPerdidaRepository().CopiarSGOCOES(pericodi, version, iFacPerCodi, iCostMargCodi, suser, sAnioMes, conn, tran);
                tran.Commit();
            }
            catch (Exception e)
            {
                indicador = e.Message;
            }
            return indicador;
        }

        //******************************************************************************************************************************************
        //ASSETEC 202002
        /// <summary>
        /// Elimina Todos los registros de temporal trn_costo_marginal_tmp
        /// </summary>
        /// <returns></returns>
        public void DeleteCMTMP()
        {
            try
            {
                FactoryTransferencia.GetFactorPerdidaRepository().DeleteCMTMP();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todas las barras de la tabla SI_COSTO_MARGINAL
        /// </summary>
        /// <param name="sAnioMes">YYYYMM del mes de valorización - pericodi</param>
        /// <returns></returns>
        public List<FactorPerdidaDTO> ListBarrasSiCostMarg(string sAnioMes)
        {
            return FactoryTransferencia.GetFactorPerdidaRepository().ListBarrasSiCostMarg(sAnioMes);
        }

        /// <summary>
        /// Insertamos la data base de la barra en la tabla trn_costo_marginal_tmp
        /// </summary>
        /// <param name="barrcodi">Identificador de la tabla TRN_BARRA</param>
        /// <param name="iDiasMes">Ultimo dia del mes</param>
        /// <param name="sAnioMes">YYYYMM del mes de valorización - pericodi</param>
        /// <returns></returns>
        public void SaveCostMargTmp(int barrcodi, int iDiasMes, string sAnioMes)
        {
            try
            {
                FactoryTransferencia.GetFactorPerdidaRepository().SaveCostMargTmp(barrcodi, iDiasMes, sAnioMes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Migra la información de rentas de congestion de si_costomarginal a RCG_COSTOMARGINAL y RCG_COSTOMARGINAL_DET
        /// </summary>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="recacodi">Versión anterior del recalculo</param>
        /// <param name="perianiomes">YYYYMM del mes de valorización - pericodi</param>
        /// <param name="suser">Usuario del sistema</param>
        /// <returns></returns>
        public int CopiarSGOCOESRentasCongestion(int pericodi, int recacodi, int perianiomes, string suser)
        {
            int indicador = 0;
            try
            {
                //Validamos si existe registros en la cabecera
                var rcgCostoMarginalCab = new RcgCostoMarginalCabDTO();
                var listRcgCostoMarginalCab = servicioRentaCongestion.ListRcgCostoMarginalCab(pericodi, recacodi).ToList();

                 
                if (listRcgCostoMarginalCab.Count > 0)
                {
                    //Actualizamos el nombre de usuario y fecha
                    rcgCostoMarginalCab.Rccmgccodi = listRcgCostoMarginalCab.First().Rccmgccodi;
                    rcgCostoMarginalCab.Pericodi = pericodi;
                    rcgCostoMarginalCab.Recacodi = recacodi;
                    rcgCostoMarginalCab.Rccmgcusumodificacion = suser;
                    rcgCostoMarginalCab.Rccmgcfecmodificacion = DateTime.Now;
                    //servicio.UpdateRcgCostoMarginalCab(rcgCostoMarginalCab);
                }
                else
                {
                    //Insertamos el registro en la tabla
                    rcgCostoMarginalCab.Pericodi = pericodi;
                    rcgCostoMarginalCab.Recacodi = recacodi;
                    rcgCostoMarginalCab.Rccmgcusucreacion = suser;
                    rcgCostoMarginalCab.Rccmgcfeccreacion = DateTime.Now;
                    rcgCostoMarginalCab.Rccmgccodi = servicioRentaCongestion.SaveRcgCostoMarginalCab(rcgCostoMarginalCab);
                }
                //Eliminamos los datos de detalle
                servicioRentaCongestion.DeleteDatosRcgCostoMarginalDet(rcgCostoMarginalCab.Rccmgccodi);
                var maximoCostoMarginalDetalleId = servicioRentaCongestion.GetMaximoCostoMarginalDetalleId();
                // Insercion de Datos
                indicador = servicioRentaCongestion.SaveCostoMarginalDetalleSEV(maximoCostoMarginalDetalleId, rcgCostoMarginalCab.Rccmgccodi, perianiomes);
            }
            catch (Exception ex)
            {
                indicador = -1;
            }
            return indicador;
        }

        /// <summary>
        /// Permite listar todas las barras de la tabla SI_COSTO_MARGINAL
        /// </summary>
        /// <param name="sAnioMes">YYYYMM del mes de valorización - pericodi</param>
        /// <param name="barrcodi">Idetificador de la tabla TRN_BARRA</param>
        /// <returns>Lista de Fechas en el mes (mas el numero de intervalos) faltantes</returns>
        public List<string> ListFechaXBarraSiCostMarg(string sAnioMes, int barrcodi)
        {
            return FactoryTransferencia.GetFactorPerdidaRepository().ListFechaXBarraSiCostMarg(sAnioMes, barrcodi);
        }

        #region ASSETEC - Popup de Ajuste de intervalos 
        /// <summary>
        /// Devuelve los días correspondientes por periodos
        /// </summary>
        /// <param name="idPeriodo">Identificador del periodo seleccionado</param>
        /// <returns></returns>
        public List<string> ObtenerDiasxPeriodos(int idPeriodo)
        {
            PeriodoDTO entity = FactoryTransferencia
                .GetTrnCostoMarginalAjusteRepository()
                .GetPeriodo(idPeriodo);

            int mes = entity.MesCodi;
            int anio = entity.AnioCodi;
            List<string> res = new List<string>();
            DateTime fecha = new DateTime(anio, mes, 1);
            while (mes == fecha.Month)
            {
                res.Add(fecha.ToString("dd/MM/yyyy"));
                fecha = fecha.AddDays(1);
            }

            return res;
        }

        /// <summary>
        /// Devuelve los intervalos de tiempo validos
        /// </summary>
        /// <returns></returns>
        public List<string> ObtenerIntervalos()
        {
            List<string> res = new List<string>();
            DateTime ini = new DateTime(1, 1, 1, 0, 15, 0);
            int dia = ini.Day;
            while (dia == ini.Day)
            {
                res.Add(ini.ToString("HH:mm"));
                ini = ini.AddMinutes(30);
            }

            return res;
        }

        /// <summary>
        /// Registra los ajustes realizados
        /// </summary>
        /// <param name="entity">Entidad de la tabla TRN_COSTO_MARGINAL_AJUSTE</param>        
        public int RegistrarAjusteIntervalo(TrnCostoMarginalAjusteDTO entity)
        {
            int res = 1;
            try
            {
                FactoryTransferencia.GetTrnCostoMarginalAjusteRepository()
                .Save(entity);
            }
            catch
            {
                res = -1;
            }

            return res;
        }

        /// <summary>
        /// Lista los ajustes realizados para el periodo y versión seleccionados
        /// </summary>
        /// <param name="idPeriodo">Identificador del periodo</param>
        /// <param name="idVersion">Identificador de la versión</param>
        /// <returns></returns>
        public List<TrnCostoMarginalAjusteDTO> ListarAjusteIntervalo(int idPeriodo,
            int idVersion)
        {
            return FactoryTransferencia
                .GetTrnCostoMarginalAjusteRepository()
                .ListByPeriodoVersion(idPeriodo, idVersion);
        }

        /// <summary>
        /// Elimina un ajuste por intervalo segun id
        /// </summary>
        /// <param name="idRegistro">Identificador de la tabla TRN_COSTO_MARGINAL_AJUSTE</param>
        /// <returns></returns>
        public int EliminarAjusteIntervalo(int idRegistro)
        {
            int res = 1;
            try
            {
                FactoryTransferencia
                    .GetTrnCostoMarginalAjusteRepository()
                    .Delete(idRegistro);
            }
            catch
            {
                res = -1;
            }
            return res;
        }

        /// <summary>
        /// Permite copiar los Factores de Perdida desde la tabla SI_COSTOMARGINAL
        /// </summary>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="version">Versión del recalculo</param>
        /// <param name="suser">Usuario que aplica los intervalos</param>
        /// <returns></returns>
        public string AjustarCostosMarginales(int pericodi, int version, string suser)
        {
            string indicador = "1";
            try
            {
                List<string> listaIntervalos = ObtenerIntervalos(); //00:15, 00:45, 01:15, 01:45, 02:15, 02:45...

                //Traemos la lista de ajustes del periodo
                List<TrnCostoMarginalAjusteDTO> listaAjuste = FactoryTransferencia.GetTrnCostoMarginalAjusteRepository().ListByPeriodoVersion(pericodi, version);
                //---------------------------------------------------------------------------------------------------------
                foreach (TrnCostoMarginalAjusteDTO dtoAjuste in listaAjuste)
                {
                    DateTime dTrncmafecha = dtoAjuste.Trncmafecha;
                    string sCosMarRec;
                    int iCosMarDiaRec, iPericodiRec, iCosMarVerRec;
                    string sCosMarEnt;
                    int iCosMarDiaEnt, iPericodiEnt, iCosMarVerEnt;

                    string sDia = dTrncmafecha.ToString("dd");
                    string sHoraMinuto = dTrncmafecha.ToString("HH:mm");
                    if (sDia == "01" && sHoraMinuto == "00:15")
                    {
                        //Se disminute en un dia del MES ANTERIOR
                        sCosMarRec = "COSMAR1";
                        iCosMarDiaRec = dTrncmafecha.Day; //01/08/2022 00:15 Es igual a 1
                        iPericodiRec = pericodi;
                        iCosMarVerRec = version;

                        PeriodoDTO dtoPeriodo = (this).servicioPeriodo.BuscarPeriodoAnterior(pericodi);
                        int iVersionAnterior = (new RecalculoAppServicio()).GetUltimaVersion(dtoPeriodo.PeriCodi);
                        sCosMarEnt = "COSMAR96";
                        iCosMarDiaEnt = dTrncmafecha.AddDays(-1).Day; // 31/07/2022 00:15 -> Ultimo dia del mes = 31
                        iPericodiEnt = dtoPeriodo.PeriCodi;
                        iCosMarVerEnt = iVersionAnterior;

                    }
                    else if (sHoraMinuto == "00:15")
                    {
                        //Se disminute en un dia del mismo mes
                        sCosMarRec = "COSMAR1";
                        iCosMarDiaRec = dTrncmafecha.Day; // 15/08/2022 00:15 -> dia = 15
                        iPericodiRec = pericodi;
                        iCosMarVerRec = version;

                        sCosMarEnt = "COSMAR96";
                        iCosMarDiaEnt = dTrncmafecha.AddDays(-1).Day; // 14/08/2022 00:15 -> dia = 14
                        iPericodiEnt = pericodi;
                        iCosMarVerEnt = version;
                    }
                    else
                    {
                        // Mayores a 00:45, 01:15, 01:45, 02:15, 02:45, 03:15, 03:45, 04:15, 04:45.... 
                        //Del mismo día retrocede un Intervalo
                        int iIndice = 0;
                        foreach (string sIntervalo in listaIntervalos)
                        {
                            iIndice++;
                            if (sIntervalo.Equals(sHoraMinuto))
                            {
                                break;
                            }
                        }
                        sCosMarRec = "COSMAR" + (iIndice * 2 - 1).ToString(); //Obtenemos 3/5/7/9/11/...
                        iCosMarDiaRec = dTrncmafecha.Day; // 15/08/2022 05:45 -> dia = 15
                        iPericodiRec = pericodi;
                        iCosMarVerRec = version;

                        sCosMarEnt = "COSMAR" + (iIndice * 2 - 2).ToString(); //Obtenemos 2/4/6/8/10/...
                        iCosMarDiaEnt = dTrncmafecha.Day; // 15/08/2022 05:45 -> dia = 15
                        iPericodiEnt = pericodi;
                        iCosMarVerEnt = version;
                    }
                    //Ejecutamos la actualización de los costos marginales
                    FactoryTransferencia.GetCostoMarginalRepository().AjustarCostosMarginales(sCosMarRec, iCosMarDiaRec, iPericodiRec, iCosMarVerRec, sCosMarEnt, iCosMarDiaEnt, iPericodiEnt, iCosMarVerEnt);
                }
                //Actualizar los Intervalos con el usuario y Datetime de Actualización
                FactoryTransferencia.GetTrnCostoMarginalAjusteRepository().Update(pericodi, version, suser);

            }
            catch (Exception e)
            {
                indicador = e.Message;
            }
            return indicador;
        }

        /// <summary>
        /// Permite copiar los Ajustes de los Costos Marginales de un periodo de una versión anterior a la nueva versión
        /// </summary>
        /// <param name="iPeriCodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="iVersionNew">Versión siguiente del recalculo</param>
        /// <param name="iVersionOld">Versión anterior del recalculo</param>
        /// <returns></returns>
        public void CopiarAjustesCostosMarginales(int iPeriCodi, int iVersionNew, int iVersionOld)
        {
            try
            {
                FactoryTransferencia.GetTrnCostoMarginalAjusteRepository().CopiarAjustesCostosMarginales(iPeriCodi, iVersionNew, iVersionOld);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina Todos los Ajustes de los Costos Marginales en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="iVersion">Versión del recalculo</param>
        /// <returns></returns>
        public int DeleteListaAjusteCostoMarginal(int iPeriCodi, int iVersion)
        {
            try
            {
                FactoryTransferencia.GetTrnCostoMarginalAjusteRepository().DeleteListaAjusteCostoMarginal(iPeriCodi, iVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }
        #endregion
    }
}
