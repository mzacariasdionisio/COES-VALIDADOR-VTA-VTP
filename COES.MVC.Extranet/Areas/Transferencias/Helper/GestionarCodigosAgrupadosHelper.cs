using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.Subastas.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace COES.MVC.Extranet.Areas.Transferencias.Helper
{
    public class GestionarCodigosAgrupadosHelper
    {
        public List<SolicitudCodigoDTO> tabla { get; set; }

        public GestionarCodigosAgrupadosHelper(List<SolicitudCodigoDTO> tabla)
        {
            this.tabla = tabla;
        }

        /// <summary>
        ///  Permite agrupar los registros de la solicitud
        /// </summary>
        public List<SolicitudCodigoDTO> AgruparRegistros(string userName, int esExcel, List<TrnPotenciaContratadaDTO> entity)
        {

            List<TrnPotenciaContratadaDTO> entityAuxiliar = new List<TrnPotenciaContratadaDTO>();

            int? coresoCodigAuxiliar = null;
            int? codigoGrupoAgrupado = null;

            foreach (var item in entity)
            {
                TrnPotenciaContratadaDTO potenciaResultado = new TrnPotenciaContratadaDTO();

                if (item.TrnpcTipoCasoAgrupado == "AGRVTP")
                {
                    if (coresoCodigAuxiliar != item.CoresoCodi)
                        codigoGrupoAgrupado = null;
                    potenciaResultado = entity.Where(x => x.CoresoCodi == item.CoresoCodi && x.TrnpcNumOrd == 1).ToList().FirstOrDefault();
                }
                else
                    potenciaResultado = entity.Where(x => x.TrnpcTipoCasoAgrupado == "AGRVTA").ToList().FirstOrDefault();



                var itemFirst = potenciaResultado;
                item.CoresoCodiFirst = itemFirst.CoresoCodi;
                item.CoregeCodiFirst = itemFirst.CoregeCodi;
                item.TrnpCagrp = codigoGrupoAgrupado;

                coresoCodigAuxiliar = item.CoresoCodi;
                //   codigoGrupoAgrupado = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarPotenciasAgrupadas(item);

                //1. Valida si existe codigo agrupado
                SolicitudCodigoDTO objCodigoSolicitud = this.tabla.Where(x => x.CoresoCodiPotcn == item.CoresoCodiFirst
                 && (x.CoregeCodiPotcn == item.CoregeCodiFirst || x.CoregeCodiPotcn == null)
                 && x.TrnpcNumordm == 1
                ).FirstOrDefault();

                //          SELECT max(trnpcagrp)   INTO p_codigoAgrupado
                //FROM TRN_POTENCIA_CONTRATADA
                //WHERE CORESOCODI = p_coresocodiAuxFirst
                //AND(coregecodi = p_coregecodiAuxFirst OR p_coregecodiAuxFirst IS NULL)
                //AND TRNPCNUMORD = 1
                //AND TRNPCESTADO = 'ACT'
                //AND NVL(PERIDCCODI, PERICODI)= p_PERICODI;


                //          WHERE CORESOCODI = p_coresocodiAux
                //AND(coregecodi = p_coregecodiAux OR p_coregecodiAux IS NULL)
                //AND TRNPCESTADO = 'ACT'
                //AND NVL(PERIDCCODI, PERICODI)= p_PERICODI;
                if (objCodigoSolicitud == null)
                {
                    objCodigoSolicitud = this.tabla.Where(x => x.CoresoCodiPotcn == item.CoresoCodi && (x.CoregeCodiPotcn == item.CoregeCodi || x.CoregeCodiPotcn == null)).FirstOrDefault();
                    item.TrnPctUserNameIns = userName;
                    ResultadoDTO<int> resultCodigoAgrupado = new SolicitudCodigoAppServicio().GenerarCodigosAgrupadosReservados(item.TrnPctUserNameIns);
                    if (resultCodigoAgrupado.Data > 0)
                    {
                        codigoGrupoAgrupado = resultCodigoAgrupado.Data;
                        item.TrnpCagrp = codigoGrupoAgrupado;

                        objCodigoSolicitud.TrnpCagrpVTA = item.TrnpcTipoCasoAgrupado == "AGRVTA" ? item.TrnpCagrp : objCodigoSolicitud.TrnpCagrpVTA;
                        objCodigoSolicitud.TrnpCagrpVTP = item.TrnpcTipoCasoAgrupado == "AGRVTP" ? item.TrnpCagrp : objCodigoSolicitud.TrnpCagrpVTP;

                        objCodigoSolicitud.TrnpcNumordmVTA = item.TrnpcTipoCasoAgrupado == "AGRVTA" ? item.TrnpcNumOrd : objCodigoSolicitud.TrnpcNumordmVTA;
                        objCodigoSolicitud.TrnpcNumordmVTP = item.TrnpcTipoCasoAgrupado == "AGRVTP" ? item.TrnpcNumOrd : objCodigoSolicitud.TrnpcNumordmVTP;

                        //entity.TrnpctCodi = Convert.ToInt32(valorReturn(dr, TrnpctCodi) ?? 0);
                        objCodigoSolicitud.CoresoCodiPotcn = item.CoresoCodiFirst;
                        objCodigoSolicitud.CoregeCodiPotcn = item.CoregeCodiFirst;
                        objCodigoSolicitud.TrnpCagrp = item.TrnpCagrp;
                        objCodigoSolicitud.TrnpcNumordm = item.TrnpcNumOrd;
                        objCodigoSolicitud.TrnpCcodiCas = item.TrnpcTipoCasoAgrupado == "AGRVTA" ? 1 : (item.TrnpcTipoCasoAgrupado == "AGRVTP" ? 2 : 0);
                        objCodigoSolicitud.TipCasaAbrev = item.TrnpcTipoCasoAgrupado;

                        //objCodigoSolicitud.TrnPctTotalmwFija = item.TrnPctTotalMwFija;
                        //objCodigoSolicitud.TrnPctHpmwFija = item.TrnPctHpMwFija;
                        //objCodigoSolicitud.TrnPctHfpmwFija = item.TrnPctHfpMwFija;
                        //objCodigoSolicitud.TrnPctTotalmwVariable = item.TrnPctTotalMwVariable;
                        //objCodigoSolicitud.TrnPctHpmwFijaVariable = item.TrnPctHpMwFijaVariable;
                        //objCodigoSolicitud.TrnPctHfpmwFijaVariable = item.TrnPctHfpMwFijaVariable;
                        //objCodigoSolicitud.TrnPctComeObs = item.TrnPctComeObs;
                        //objCodigoSolicitud.TrnPctExcel = esExcel; //identifica si el archivo fue cargado por excel 

                        objCodigoSolicitud.TrnpcTipoPotencia = item.TrnpcTipoCasoAgrupado == "AGRVTA" ? 1 : (item.TrnpcTipoCasoAgrupado == "AGRVTP" ? 2 : 0);
                        //objCodigoSolicitud.TrnpcTipoCasoAgrupado = item.TrnpcTipoCasoAgrupado;
                        //objCodigoSolicitud.abrevEstadoVTA = abrevEstadoVTA;
                        //objCodigoSolicitud.abrevEstadoVTP = abrevEstadoVTP;

                    }
                    else
                        throw new Exception(resultCodigoAgrupado.Mensaje);
                }
                else
                {
                    //2. Obtiene agrupado
                    objCodigoSolicitud = this.tabla.Where(x => x.CoresoCodiPotcn == item.CoresoCodi
                   && (x.CoregeCodiPotcn == item.CoregeCodi || x.CoregeCodiPotcn == null)
                  ).FirstOrDefault();



                    item.TrnpCagrp = codigoGrupoAgrupado;

                    objCodigoSolicitud.TrnpCagrpVTA = item.TrnpcTipoCasoAgrupado == "AGRVTA" ? item.TrnpCagrp : objCodigoSolicitud.TrnpCagrpVTA;
                    objCodigoSolicitud.TrnpCagrpVTP = item.TrnpcTipoCasoAgrupado == "AGRVTP" ? item.TrnpCagrp : objCodigoSolicitud.TrnpCagrpVTP;

                    objCodigoSolicitud.TrnpcNumordmVTA = item.TrnpcTipoCasoAgrupado == "AGRVTA" ? item.TrnpcNumOrd : objCodigoSolicitud.TrnpcNumordmVTA;
                    objCodigoSolicitud.TrnpcNumordmVTP = item.TrnpcTipoCasoAgrupado == "AGRVTP" ? item.TrnpcNumOrd : objCodigoSolicitud.TrnpcNumordmVTP;


                    //entity.TrnpctCodi = Convert.ToInt32(valorReturn(dr, TrnpctCodi) ?? 0);
                    objCodigoSolicitud.CoresoCodiPotcn = item.CoresoCodi;
                    objCodigoSolicitud.CoregeCodiPotcn = item.CoregeCodi;
                    objCodigoSolicitud.TrnpCagrp = item.TrnpCagrp;
                    objCodigoSolicitud.TrnpcNumordm = item.TrnpcNumOrd;
                    objCodigoSolicitud.TrnpCcodiCas = item.TrnpcTipoCasoAgrupado == "AGRVTA" ? 1 : (item.TrnpcTipoCasoAgrupado == "AGRVTP" ? 2 : 0);
                    objCodigoSolicitud.TipCasaAbrev = item.TrnpcTipoCasoAgrupado;

                    //objCodigoSolicitud.TrnPctTotalmwFija = item.TrnPctTotalMwFija;
                    //objCodigoSolicitud.TrnPctHpmwFija = item.TrnPctHpMwFija;
                    //objCodigoSolicitud.TrnPctHfpmwFija = item.TrnPctHfpMwFija;
                    //objCodigoSolicitud.TrnPctTotalmwVariable = item.TrnPctTotalMwVariable;
                    //objCodigoSolicitud.TrnPctHpmwFijaVariable = item.TrnPctHpMwFijaVariable;
                    //objCodigoSolicitud.TrnPctHfpmwFijaVariable = item.TrnPctHfpMwFijaVariable;
                    //objCodigoSolicitud.TrnPctComeObs = item.TrnPctComeObs;
                    //objCodigoSolicitud.TrnPctExcel = esExcel; //identifica si el archivo fue cargado por excel 

                    objCodigoSolicitud.TrnpcTipoPotencia = item.TrnpcTipoCasoAgrupado == "AGRVTA" ? 1 : (item.TrnpcTipoCasoAgrupado == "AGRVTP" ? 2 : 0);
                    //objCodigoSolicitud.TrnpcTipoCasoAgrupado = item.TrnpcTipoCasoAgrupado;
                    //objCodigoSolicitud.abrevEstadoVTA = abrevEstadoVTA;
                    //objCodigoSolicitud.abrevEstadoVTP = abrevEstadoVTP;

                }
            }


            //  potcn.trnpcagrp
            //,trn_codigo_retiro_solicitud.coresocodi desc
            //, empresa.emprnomb
            //,cliente.emprnomb
            //,trn_barra.barrbarratransferencia
            //,potcngn.trnpcagrp
            //,trnpcnumordm
            //,generado.coregecodvtp

            string jsonAuxiliar = new JavaScriptSerializer().Serialize(this.tabla);
            List<SolicitudCodigoAuxiliarDTO> auxiliar = new JavaScriptSerializer().Deserialize<List<SolicitudCodigoAuxiliarDTO>>(jsonAuxiliar);
            var resultadoAuxi = (from x in auxiliar
                                 orderby x.TrnpCagrpVTA descending,
                                                 x.TrnpcNumordmVTA descending,
                                                x.SoliCodiRetiCodi descending,
                                                x.EmprNombre,
                                                x.CliNombre,
                                                x.BarrNombBarrTran,
                                                x.TrnpCagrpVTP descending,
                                                x.TrnpcNumordmVTP descending,
                                                x.SoliCodiRetiCodigoVTP

                                 select x);
            jsonAuxiliar = new JavaScriptSerializer().Serialize(resultadoAuxi);
            this.tabla = new JavaScriptSerializer().Deserialize<List<SolicitudCodigoDTO>>(jsonAuxiliar);
            //this.tabla.Sort();
            return this.tabla;
        }





    }




}