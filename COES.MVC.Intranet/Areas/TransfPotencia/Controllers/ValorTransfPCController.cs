using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Enum;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Titularidad;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class ValorTransfPCController : BaseController
    {
        // GET: /TransfPotencia/ValorTransferencia/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        IngresoRetiroSCAppServicio servicioFactorProporcion = new IngresoRetiroSCAppServicio();
        IngresoPotenciaAppServicio servicioIngresoPotencia = new IngresoPotenciaAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();
        TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();
        TitularidadAppServicio servicioTitularidad = new TitularidadAppServicio();

        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();
        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult Index(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();

            ValorTransfPCModel model = new ValorTransfPCModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = model.ListaPeriodos[0].PeriCodi; }
            if (pericodi > 0)
            {
                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                { recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
                while (model.EntidadRecalculoPotencia == null && recpotcodi > 0)
                {
                    recpotcodi--;
                    model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
                }
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            }
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;

            //ASSETEC 202108 TIEE
            model.ListaTrnMigracion = this.servicioTransferencia.GetByCriteriaTrnMigracions();
            //-----------------------------------------------------------------------------

            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);

            //if (pericodi > 0 && recpotcodi > 0)
            //{
            //    VtpRecalculoPotenciaDTO recalculo = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);

            //    if (recalculo.Recpotestado == "Cerrado")
            //    {
            //        model.bGrabar = false;
            //    }
            //}

            return View(model);
        }

        /// <summary>
        /// Permite procesar la valorización de transferencia de potencia y compensación del periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarValorizacion(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            if (pericodi == 0 || recpotcodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de valorización y una versión de recalculo";
                return Json(sResultado);
            }
            try
            {

                //Eliminamos la información procesada en el periodo / versión
                string sBorrar = this.servicioTransfPotencia.EliminarProceso(pericodi, recpotcodi);
                if (!sBorrar.Equals("1"))
                {
                    return Json(sBorrar);
                }
                PeriodoDTO dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo2 = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, recpotcodi);
               
                ValorTransfPCModel modelTransfPC = new ValorTransfPCModel();
                modelTransfPC.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.ProcesarValorizacionProcesar;
                objAuditoria.Estdcodi = (int)EVtpEstados.CalcularValorizacion;
                objAuditoria.Audproproceso = "Comienza la valorización del periodo";
                objAuditoria.Audprodescripcion = "Comienza la valorización del periodo " + dtoPeriodo.PeriNombre + " / " + modelTransfPC.EntidadRecalculoPotencia.Recpotnombre;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);

                var EmpresasTTIE = this.servicioTransferencia.ListTrnMigracionDTI().ToList();

                #endregion

                if (modelTransfPC.EntidadRecalculoPotencia.Recacodi == null)
                {
                    sResultado = "Lo sentimos, no esta relacionado a la versión de recalculo del sistema de VTEA";
                    return Json(sResultado);
                }

                //En memoria la Lista de Factores de Proporción asociado
                modelTransfPC.ListaFactorProporcion = this.servicioFactorProporcion.ListImportesByPeriVer(pericodi, Convert.ToInt32(modelTransfPC.EntidadRecalculoPotencia.Recacodi));

                //En memoria
                //Traemos la lista de empresas en el mes de valorización y versión de recalculo: EmprCodi y EmprNomb de la vista VW_VTP_PEAJE_EGRESO y TRN_ING_RETIROSC
                modelTransfPC.ListaPeajeEgresoEmpresa = this.servicioTransfPotencia.ListVtpPeajeEgresoMinfoCabecera(pericodi, recpotcodi, (int)modelTransfPC.EntidadRecalculoPotencia.Recacodi);
                if (modelTransfPC.ListaPeajeEgresoEmpresa == null)
                {
                    sResultado = "Lo sentimos, ninguna empresa ha reportado Información sobre Egresos y Peajes para este mes de valorización y versión de recalculo";
                    return Json(sResultado);
                }                
                
                foreach (var factProp in modelTransfPC.ListaFactorProporcion)
                {
                    // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                    var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == factProp.EmprCodi).ToList();
                    if (empresaTTIE.Count > 0)
                    {
                        var peajeEmpresa = modelTransfPC.ListaFactorProporcion.Where(x => x.EmprCodi == empresaTTIE.First().Emprcodidestino).ToList();
                        if (peajeEmpresa.Count() > 0) factProp.EmprCodi = peajeEmpresa.First().EmprCodi;
                        var peajeEmpresa_ = modelTransfPC.ListaFactorProporcion.Where(x => x.EmprCodi == factProp.EmprCodi).ToList();
                        if (peajeEmpresa_.Count() > 0) factProp.EmprCodi = peajeEmpresa_.First().EmprCodi;
                    }                    
                    //if (empresaTTIE.Count > 0) factProp.EmprCodi = empresaTTIE.First().Emprcodidestino;
                    // FIN
                }
                var duplicadosIng = modelTransfPC.ListaFactorProporcion.GroupBy(x => x.EmprCodi).Where(emp => emp.Count() > 1).Select(x => x.Key).ToList();
                if (duplicadosIng.Any())
                {
                    foreach (var codigoEmpresaDuplicado in duplicadosIng)
                    {
                        decimal sum = modelTransfPC.ListaFactorProporcion.Where(x => x.EmprCodi == codigoEmpresaDuplicado).Sum(x => x.IngrscImporteVtp);
                        var dtoICd = modelTransfPC.ListaFactorProporcion.Where(x => x.EmprCodi == codigoEmpresaDuplicado).FirstOrDefault();
                        dtoICd.IngrscImporteVtp = sum;
                        modelTransfPC.ListaFactorProporcion.RemoveAll(x => x.EmprCodi == codigoEmpresaDuplicado);
                        modelTransfPC.ListaFactorProporcion.Add(dtoICd);
                    }
                }
                decimal dTotalRecaudacionTransmision = 0;
                Int32[] aEmprCodi = new Int32[1000];
                decimal[] aEmprTotRecaudacion = new decimal[1000];
                decimal[] aEmprPorcRecaudacion = new decimal[1000];

                #region A4: Procedimiento para la recaudación por peajes de una Empresa Generadora

                #region PASO 1: Tenemos en memoria las listas que se necesitan para los calculos: Cargos y Retiros Sin Contrato

                //Lista de Cargos (= 4.4.6 Desarrollo de Peajes e Ingresos Tarifarios): Porcentajes y Valores de Regulado, Libre y Gran Usuario
                modelTransfPC.ListaPeajeIngresoCargo = this.servicioTransfPotencia.ListVtpPeajeIngresoCargo(pericodi, recpotcodi);
                if (modelTransfPC.ListaPeajeIngresoCargo == null)
                {
                    sResultado = "Lo sentimos, no existen cargos para este mes de valorización y versión de recalculo";
                    return Json(sResultado);
                }
                //Totalizamos por:
                decimal dTotalRegulado = 0;
                decimal dTotalLibre = 0;
                decimal dTotalGranUsuario = 0;
                foreach (VtpPeajeIngresoDTO dtoCargo in modelTransfPC.ListaPeajeIngresoCargo)
                {
                    dTotalRegulado += (decimal)dtoCargo.Pingregulado;
                    dTotalLibre += (decimal)dtoCargo.Pinglibre;
                    dTotalGranUsuario += (decimal)dtoCargo.Pinggranusuario;
                }
                //Redondeando a 3 decimales los totales:
                dTotalRegulado = Math.Round(dTotalRegulado, 3);
                dTotalLibre = Math.Round(dTotalLibre, 3);
                dTotalGranUsuario = Math.Round(dTotalGranUsuario, 3);

                modelTransfPC.ListaRetiroPotenciaSC = this.servicioTransfPotencia.ListVtpRetiroPotenciaSCView(pericodi, recpotcodi);

                #endregion

                #region PASO 2 - Para cada Empresa Generadora que reporto en la Extranet/Intranet: Egresos y Peajes
             
               /* foreach (var peajeEgresoEmp in modelTransfPC.ListaPeajeEgresoEmpresa)
                {
                    // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                    var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == peajeEgresoEmp.Genemprcodi).ToList();
                    if (empresaTTIE.Count > 0) peajeEgresoEmp.Genemprcodi = empresaTTIE.First().Emprcodidestino;
                    // FIN
                }*/
                /*var duplicadosPje = modelTransfPC.ListaPeajeEgresoEmpresa.GroupBy(x => x.Genemprcodi).Where(emp => emp.Count() > 1).Select(x => x.Key).ToList();
                if (duplicadosPje.Any())
                {
                    foreach (var codigoEmpresaDuplicadoPje in duplicadosPje)
                    {
                        var dtoICd = modelTransfPC.ListaPeajeEgresoEmpresa.Where(x => x.Genemprcodi == codigoEmpresaDuplicadoPje).FirstOrDefault();
                        modelTransfPC.ListaPeajeEgresoEmpresa.RemoveAll(x => x.Genemprcodi == codigoEmpresaDuplicadoPje);
                        modelTransfPC.ListaPeajeEgresoEmpresa.Add(dtoICd);
                    }
                }*/

                int iFila = 0;
                //LISTA DE EMPRESAS: VW_VTP_PEAJE_EGRESO y TRN_ING_RETIROSC 
                foreach (VtpPeajeEgresoMinfoDTO dtoPeajeEgresoEmpresa in modelTransfPC.ListaPeajeEgresoEmpresa)
                {
                    //Para cada Empresa Generadora: EmprCodi y EmprNomb
                    int iEmprCodi = (int)dtoPeajeEgresoEmpresa.Genemprcodi;
                    string sEmprNomb = dtoPeajeEgresoEmpresa.Genemprnombre;
                    //ASSETEC 20190422: Traemos la lista de Egresos y Peajes / Mejor información de cada Cliente de la vista VW_VTP_PEAJE_EGRESO
                    modelTransfPC.ListaPeajeEgreso = this.servicioTransfPotencia.ListVtpPeajeEgresoMinfoEmpresaRecalculo(pericodi, recpotcodi, iEmprCodi);

                    #region PASO 3 - Consultar si la empresa tiene FactorProporcion
                    decimal dFactorProporcion = 0;
                    foreach (IngresoRetiroSCDTO dtoFactoProporcion in modelTransfPC.ListaFactorProporcion)
                    {
                        if (dtoFactoProporcion.EmprCodi == iEmprCodi)
                        {
                            dFactorProporcion = dtoFactoProporcion.IngrscImporteVtp;
                            break;
                        }
                    }
                    #endregion

                    #region PASO 4 - Realizamos los cálculos para la Lista de Cargos
                    //Listamos los Cargos para obtener: dPeajeCalculado, dPeajeDeclarado, dPeajeRecaudado
                    foreach (VtpPeajeIngresoDTO dtoCargo in modelTransfPC.ListaPeajeIngresoCargo)
                    {
                        //Para cada Cargo: dtoCargo.Pingcodi
                        decimal dPeajeCalculado = 0;
                        decimal dPeajeDeclarado = 0;
                        decimal dPeajeRecaudado = 0;

                        #region PASO 5 - De la información de Egresos y Peajes asignados a la Empresa Generadora: iEmprCodi
                        ////Traemos la lista de Egresos y Peajes / Mejor información de cada Cliente de la vista VW_VTP_PEAJE_EGRESO
                        //modelTransfPC.ListaPeajeEgreso = this.servicioTransfPotencia.ListVtpPeajeEgresoMinfoEmpresa(pericodi, recpotcodi, iEmprCodi);
                        foreach (VtpPeajeEgresoMinfoDTO dtoPeajeEgreso in modelTransfPC.ListaPeajeEgreso)
                        {
                            //Para cada cliente, realizamos los calculos de:
                            //PeajeCalculado
                            decimal dPotenciaCalculada = Convert.ToDecimal(dtoPeajeEgreso.Pegrmipotecalculada);
                            decimal dPCalculado = 0;
                            decimal dPDeclarado = 0;
                            if (dtoPeajeEgreso.Pegrmitipousuario.ToUpper().Equals("REGULADO"))
                            {
                                dPCalculado = (dPotenciaCalculada * dTotalRegulado) * Convert.ToDecimal(dtoCargo.Pingporctregulado);
                            }
                            else if (dtoPeajeEgreso.Pegrmitipousuario.ToUpper().Equals("LIBRE"))
                            {
                                dPCalculado = (dPotenciaCalculada * dTotalLibre) * Convert.ToDecimal(dtoCargo.Pingporctlibre);
                            }
                            else if (dtoPeajeEgreso.Pegrmitipousuario.ToUpper().Equals("GRAN USUARIO"))
                            {
                                dPCalculado = (dPotenciaCalculada * dTotalGranUsuario) * Convert.ToDecimal(dtoCargo.Pingporctgranusuario);
                            }
                            dPeajeCalculado += dPCalculado;
                            //PeajeDeclarado
                            decimal dPotenciaDeclarada = Convert.ToDecimal(dtoPeajeEgreso.Pegrmipotedeclarada);
                            if (dtoPeajeEgreso.Pegrmitipousuario.ToUpper().Equals("REGULADO"))
                            {
                                dPDeclarado = dPotenciaDeclarada * Convert.ToDecimal(dtoCargo.Pingporctregulado) * Convert.ToDecimal(dtoPeajeEgreso.Pegrmipeajeunitario);
                            }
                            else if (dtoPeajeEgreso.Pegrmitipousuario.ToUpper().Equals("LIBRE"))
                            {
                                dPDeclarado = dPotenciaDeclarada * Convert.ToDecimal(dtoCargo.Pingporctlibre) * Convert.ToDecimal(dtoPeajeEgreso.Pegrmipeajeunitario);
                            }
                            else if (dtoPeajeEgreso.Pegrmitipousuario.ToUpper().Equals("GRAN USUARIO"))
                            {
                                dPDeclarado = dPotenciaDeclarada * Convert.ToDecimal(dtoCargo.Pingporctgranusuario) * Convert.ToDecimal(dtoPeajeEgreso.Pegrmipeajeunitario);
                            }
                            dPeajeDeclarado += dPDeclarado;
                        }
                        #endregion

                        #region PASO 6 - Factor de proporción (dFactorProporcion > 0) de la Empresa Generadora para los RSC
                        //Agregamos los valores en caso la Empresa Generadora este presente en Factor de Proporcion: dFactorProporcion > 0
                        if (dFactorProporcion > 0)
                        {
                            var ListFactorP = modelTransfPC.ListaFactorProporcion.Where(x => x.EmprCodi == iEmprCodi).ToList();
                            if (ListFactorP.Count > 0)
                            {
                                foreach (var y in ListFactorP)
                                {
                                    dFactorProporcion = y.IngrscImporteVtp;
                                    //Del PASO 2
                                    foreach (VtpRetiroPotescDTO dtoRetiroPotenciaSC in modelTransfPC.ListaRetiroPotenciaSC)
                                    {
                                        decimal dPotenciaC_D = Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpoteegreso) * dFactorProporcion;
                                        decimal dPCalculado = 0;
                                        decimal dPDeclarado = 0;
                                        //PeajeCalculado
                                        if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("REGULADO"))
                                        {
                                            dPCalculado = dPotenciaC_D * Convert.ToDecimal(dtoCargo.Pingporctregulado) * dTotalRegulado;
                                        }
                                        else if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("LIBRE"))
                                        {
                                            dPCalculado = dPotenciaC_D * Convert.ToDecimal(dtoCargo.Pingporctlibre) * dTotalLibre;
                                        }
                                        else if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("GRAN USUARIO"))
                                        {
                                            dPCalculado = dPotenciaC_D * Convert.ToDecimal(dtoCargo.Pingporctgranusuario) * dTotalGranUsuario;
                                        }
                                        dPeajeCalculado += dPCalculado;
                                        //PeajeDeclarado
                                        if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("REGULADO"))
                                        {
                                            //dPDeclarado = dPotenciaC_D * dTotalRegulado * Convert.ToDecimal(dtoCargo.Pingporctregulado); //Cambiado EjemploTotal dTotalRegulado x Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpeajeunitario)
                                            dPDeclarado = dPotenciaC_D * Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpeajeunitario) * Convert.ToDecimal(dtoCargo.Pingporctregulado);
                                        }
                                        else if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("LIBRE"))
                                        {
                                            dPDeclarado = dPotenciaC_D * dTotalLibre * Convert.ToDecimal(dtoCargo.Pingporctlibre); //Cambiado EjemploTotal dTotalLibre x Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpeajeunitario)
                                        }
                                        else if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("GRAN USUARIO"))
                                        {
                                            dPDeclarado = dPotenciaC_D * dTotalGranUsuario * Convert.ToDecimal(dtoCargo.Pingporctgranusuario); //Cambiado EjemploTotal dTotalGranUsuario x Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpeajeunitario)
                                        }
                                        dPeajeDeclarado += dPDeclarado;
                                    }
                                }
                            }
                            else
                            {
                                //Del PASO 2
                                foreach (VtpRetiroPotescDTO dtoRetiroPotenciaSC in modelTransfPC.ListaRetiroPotenciaSC)
                                {
                                    decimal dPotenciaC_D = Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpoteegreso) * dFactorProporcion;
                                    decimal dPCalculado = 0;
                                    decimal dPDeclarado = 0;
                                    //PeajeCalculado
                                    if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("REGULADO"))
                                    {
                                        dPCalculado = dPotenciaC_D * Convert.ToDecimal(dtoCargo.Pingporctregulado) * dTotalRegulado;
                                    }
                                    else if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("LIBRE"))
                                    {
                                        dPCalculado = dPotenciaC_D * Convert.ToDecimal(dtoCargo.Pingporctlibre) * dTotalLibre;
                                    }
                                    else if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("GRAN USUARIO"))
                                    {
                                        dPCalculado = dPotenciaC_D * Convert.ToDecimal(dtoCargo.Pingporctgranusuario) * dTotalGranUsuario;
                                    }
                                    dPeajeCalculado += dPCalculado;
                                    //PeajeDeclarado
                                    if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("REGULADO"))
                                    {
                                        //dPDeclarado = dPotenciaC_D * dTotalRegulado * Convert.ToDecimal(dtoCargo.Pingporctregulado); //Cambiado EjemploTotal dTotalRegulado x Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpeajeunitario)
                                        dPDeclarado = dPotenciaC_D * Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpeajeunitario) * Convert.ToDecimal(dtoCargo.Pingporctregulado);
                                    }
                                    else if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("LIBRE"))
                                    {
                                        dPDeclarado = dPotenciaC_D * dTotalLibre * Convert.ToDecimal(dtoCargo.Pingporctlibre); //Cambiado EjemploTotal dTotalLibre x Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpeajeunitario)
                                    }
                                    else if (dtoRetiroPotenciaSC.Rpsctipousuario.ToUpper().Equals("GRAN USUARIO"))
                                    {
                                        dPDeclarado = dPotenciaC_D * dTotalGranUsuario * Convert.ToDecimal(dtoCargo.Pingporctgranusuario); //Cambiado EjemploTotal dTotalGranUsuario x Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpeajeunitario)
                                    }
                                    dPeajeDeclarado += dPDeclarado;
                                }
                            }
                        }
                        #endregion

                        #region PASO 7 - Insertamos en VTP_PEAJE_CARGO los cálculos obtenidos para para la recaudación del Cargo para la Empresa Generadora
                        dPeajeRecaudado = System.Math.Max(dPeajeCalculado, dPeajeDeclarado);
                        VtpPeajeCargoDTO dtoPeajeCargo = new VtpPeajeCargoDTO();
                        dtoPeajeCargo.Pericodi = pericodi;
                        dtoPeajeCargo.Recpotcodi = recpotcodi;
                        dtoPeajeCargo.Emprcodi = iEmprCodi;
                        dtoPeajeCargo.Pingcodi = dtoCargo.Pingcodi;
                        dtoPeajeCargo.Pecartransmision = dtoCargo.Pingtransmision;
                        dtoPeajeCargo.Pecarpeajecalculado = dPeajeCalculado;
                        dtoPeajeCargo.Pecarpeajedeclarado = dPeajeDeclarado;
                        dtoPeajeCargo.Pecarusucreacion = User.Identity.Name;

                        //Hay que importar todos los saldos de cargos de periodos atras (en caso existan: Pecarpericodidest = Pericodi) para sumarle a dPeajeRecaudado
                        decimal dSaldoAnterior = 0;
                        if (recpotcodi == 1)
                        {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                            int pecarpericodidest = pericodi;
                            dSaldoAnterior = this.servicioTransfPotencia.GetPeajeCargoSaldoAnterior(pecarpericodidest, dtoPeajeCargo.Emprcodi, dtoCargo.Pingcodi);
                        }
                        //El acumulado de todos los saldos anteriores aplicados a este periodo
                        dtoPeajeCargo.Pecarsaldoanterior = dSaldoAnterior;

                        //Consultamos por el Ajuste de saldo que se aplica a esta Empresa y cargo en este Periodo
                        decimal dPecajajuste = 0;
                        if (recpotcodi == 1)
                        {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                            dPecajajuste = this.servicioTransfPotencia.GetPeajeCargoAjuste(pericodi, dtoPeajeCargo.Emprcodi, dtoCargo.Pingcodi);
                        }
                        dtoPeajeCargo.Pecarajuste = dPecajajuste;

                        //EDUARDO: 20170503
                        //dtoPeajeCargo.Pecarpeajerecaudado = dPeajeRecaudado + dSaldoAnterior + dPecajajuste; 
                        //Pecarpeajerecaudado: solo almacena el calculo de esa version, mensual o revisión
                        dtoPeajeCargo.Pecarpeajerecaudado = dPeajeRecaudado;
                        //Insertamos el registro en VTP_PEAJE_CARGO: cargos por peaje en una empresa
                        dtoPeajeCargo.Pecarcodi = this.servicioTransfPotencia.SaveVtpPeajeCargo(dtoPeajeCargo);
                        if (dtoCargo.Pingtransmision.Equals("SI"))
                        {
                            aEmprCodi[iFila] = iEmprCodi; //Para cada empresa
                            aEmprTotRecaudacion[iFila] = dPeajeRecaudado; //almacenamos su importe total por tranmisión
                            dTotalRecaudacionTransmision += dPeajeRecaudado; //Acumulamos el total General
                            iFila++;

                        }
                        if (recpotcodi > 1)
                        {
                            //Hay que encontrar el Saldo entre esta versión (>1) y la anterior y almacenarlo en VTP_PEAJE_CARGO.Pecarsaldo y Pecarpericodidest
                            int iRecpotcodiOld = recpotcodi - 1;
                            VtpPeajeCargoDTO dtoPeajeCargoAnterior = new VtpPeajeCargoDTO();
                            dtoPeajeCargoAnterior = this.servicioTransfPotencia.GetByIdVtpPeajeCargoSaldo(pericodi, iRecpotcodiOld, dtoPeajeCargo.Emprcodi, dtoCargo.Pingcodi);
                            if (dtoPeajeCargoAnterior != null)
                            {
                                dtoPeajeCargo.Pecarsaldo = dtoPeajeCargo.Pecarpeajerecaudado - dtoPeajeCargoAnterior.Pecarpeajerecaudado; //Saldo entre versiones
                                dtoPeajeCargo.Pecarpericodidest = Convert.ToInt32(modelTransfPC.EntidadRecalculoPotencia.Pericodidestino); //Periodo en que se aplica el saldo
                                //Actualizamos el registro guardando el saldo y el periodo de destino
                                this.servicioTransfPotencia.UpdateVtpPeajeCargo(dtoPeajeCargo);
                            }
                        }

                        #endregion
                    }
                    #endregion
                }
                //OK
                #region PASO 8 - Insertar por Empresa el PAGO PEAJE POR TRANSMISION (TotalRecaudacion y el Porcentaje) con respecto el total de transmision en VTP_PEAJE_EMPRESA
                for (int i = 0; i < iFila; i++)
                {
                    decimal dEmprPorctRecaudacion = 0;
                    if (dTotalRecaudacionTransmision != 0)
                    {
                        dEmprPorctRecaudacion = (aEmprTotRecaudacion[i] / dTotalRecaudacionTransmision);
                    }
                    VtpPeajeEmpresaDTO dtoPeajeEmpresa = new VtpPeajeEmpresaDTO();
                    dtoPeajeEmpresa.Pericodi = pericodi;
                    dtoPeajeEmpresa.Recpotcodi = recpotcodi;
                    dtoPeajeEmpresa.Emprcodi = aEmprCodi[i];
                    dtoPeajeEmpresa.Pemptotalrecaudacion = aEmprTotRecaudacion[i];
                    dtoPeajeEmpresa.Pempporctrecaudacion = dEmprPorctRecaudacion;
                    dtoPeajeEmpresa.Pempusucreacion = User.Identity.Name;
                    //Insertamos el registro en VTP_PEAJE_EMPRESA: lo que la empresa ha recaudado en el periodo version
                    this.servicioTransfPotencia.SaveVtpPeajeEmpresa(dtoPeajeEmpresa);
                    aEmprPorcRecaudacion[i] = dEmprPorctRecaudacion;
                }
                #endregion //Fin Paso 8

                #endregion //Fin Paso 2

                #endregion //Fin A4

                #region A1 - A3: Cálculo de Pago de Peajes
                //A.1 - En 4.4.5 “Pago = Si” a un elemento se le pague o no por peajes.
                modelTransfPC.ListaPeajeIngresoCargo = this.servicioTransfPotencia.ListVtpPeajeIngresoPagoSi(pericodi, recpotcodi);
                foreach (VtpPeajeIngresoDTO dtoPeajeIngresoSi in modelTransfPC.ListaPeajeIngresoCargo)
                {
                    int iPingcodi = dtoPeajeIngresoSi.Pingcodi;
                    decimal dPeajeMensual = Convert.ToDecimal(dtoPeajeIngresoSi.Pingpeajemensual);
                    string sTrasmision = dtoPeajeIngresoSi.Pingtransmision;
                    if (sTrasmision.Equals("SI"))
                    {   //PAGO PEAJE POR TRANSMISION
                        string sEmprnomb = dtoPeajeIngresoSi.Emprnomb; //“Red De Energía del Perú”
                        if (dtoPeajeIngresoSi.Emprcodi == null)
                        {
                            sResultado = "Lo sentimos, el cargo " + dtoPeajeIngresoSi.Pingnombre + " no tiene una empresa relacionada";
                            return Json(sResultado);
                        }
                        int iEmprCodi = Convert.ToInt32(dtoPeajeIngresoSi.Emprcodi);
                        //Traemos la lista de Empresas que Recaudaron
                        for (int i = 0; i < iFila; i++)
                        {
                            VtpPeajeEmpresaPagoDTO dtoPeajeEmpresaPago = new VtpPeajeEmpresaPagoDTO();
                            dtoPeajeEmpresaPago.Pericodi = pericodi;
                            dtoPeajeEmpresaPago.Recpotcodi = recpotcodi;
                            dtoPeajeEmpresaPago.Emprcodipeaje = aEmprCodi[i]; //Codigo de la Empresa que recaudo y va a pagar
                            dtoPeajeEmpresaPago.Pingcodi = iPingcodi;
                            dtoPeajeEmpresaPago.Emprcodicargo = iEmprCodi; //Codigo de la Empresa Titular del Cargo... 
                            dtoPeajeEmpresaPago.Pempagtransmision = sTrasmision;
                            if (dPeajeMensual != 0)
                            {   //Porcentaje (del total de todo lo recaudado por totas las empresas) / Peaje Mensual
                                dtoPeajeEmpresaPago.Pempagpeajepago = aEmprPorcRecaudacion[i] * dPeajeMensual;
                            }
                            dtoPeajeEmpresaPago.Pempagusucreacion = User.Identity.Name;

                            //Hay que importar todos los saldos de Pagos de periodos atras (en caso existan: Pempagpericodidest = Pericodi) para sumarle a Pempagpeajepago
                            decimal dSaldoAnterior = 0;
                            if (recpotcodi == 1)
                            {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                                int pempagpericodidest = pericodi;
                                dSaldoAnterior = this.servicioTransfPotencia.GetPeajeEmpresaPagoSaldoAnterior(pempagpericodidest, dtoPeajeEmpresaPago.Pingcodi,
                                    dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Emprcodicargo);
                            }
                            dtoPeajeEmpresaPago.Pempagsaldoanterior = dSaldoAnterior;
                            //dtoPeajeEmpresaPago.Pempagpeajepago += dSaldoAnterior; 20170504
                            dtoPeajeEmpresaPago.Pempagpericodidest = 0; //seteamos a CERO el destino del saldo
                            //Consultamos por el Ajuste de saldo que se aplica a esta EmpresaPeaje, EmpresaCargo y Cargo en este Periodo
                            decimal dPempajajuste = 0;
                            if (recpotcodi == 1)
                            {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                                dPempajajuste = this.servicioTransfPotencia.GetPeajeEmpresaAjuste(pericodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Pingcodi, dtoPeajeEmpresaPago.Emprcodicargo);
                            }
                            dtoPeajeEmpresaPago.Pempagajuste = dPempajajuste;
                            //dtoPeajeEmpresaPago.Pempagpeajepago += dPempajajuste; 20170504

                            //Insertamos en VTP_PEAJE_EMPRESA_PAGO por el Pago Peaje por transmision
                            dtoPeajeEmpresaPago.Pempagcodi = this.servicioTransfPotencia.SaveVtpPeajeEmpresaPago(dtoPeajeEmpresaPago);
                            if (recpotcodi > 1)
                            {
                                //Hay que encontrar el Saldo entre esta versión y la anterior y almacenarlo en VTP_PEAJE_EMPRESA_PAGO
                                int iRecpotcodiOld = recpotcodi - 1;
                                List<VtpPeajeEmpresaPagoDTO> dtoPeajeEmpresaPagoAnterior = new List<VtpPeajeEmpresaPagoDTO>();
                                dtoPeajeEmpresaPagoAnterior = this.servicioTransfPotencia.GetByIdVtpPeajeEmpresaPagoSaldo(pericodi, iRecpotcodiOld, dtoPeajeEmpresaPago.Pingcodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Emprcodicargo).ToList();
                                decimal pempagpeajepago = 0;
                                if (dtoPeajeEmpresaPagoAnterior != null)
                                {
                                    //MCHAVEZ: revisar si es correcto la diferencia entre los pagos de peajes entre versiones sin la intervención de los saldos y ajustes.
                                    foreach(VtpPeajeEmpresaPagoDTO dtoPeajeEmpresaPagoAnt in dtoPeajeEmpresaPagoAnterior)
                                    {
                                         pempagpeajepago += dtoPeajeEmpresaPagoAnt.Pempagpeajepago;
                                    }
                                    dtoPeajeEmpresaPago.Pempagsaldo = dtoPeajeEmpresaPago.Pempagpeajepago - pempagpeajepago;
                                    dtoPeajeEmpresaPago.Pempagpericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                                    //Actualizamos el registro guardando el saldo y el periodo de destino
                                    this.servicioTransfPotencia.UpdateVtpPeajeEmpresaPago(dtoPeajeEmpresaPago);
                                }
                            }
                        }
                    }
                    else
                    {   //PAGO PEAJES ADICIONALES
                        //A.2 - “Pago = Si” se deberá obtener el nombre del Titular de la empresa que le corresponde el pago (4.4.5). 
                        string sEmprnomb = dtoPeajeIngresoSi.Emprnomb; //“EDEGEL / EGENOR / GeneraciónAdiconal[EGASA/EDEGEL/EGENOR]”
                        int iEmprCodi = 0;
                        if (dtoPeajeIngresoSi.Emprcodi != null)
                        {   //Empresa Titular Existe: EDEGEL / EGENOR
                            iEmprCodi = Convert.ToInt32(dtoPeajeIngresoSi.Emprcodi);
                        }
                        if (iEmprCodi != 0)
                        {
                            //Traemos la lista de Empresas que Recaudaron y Transmisión fue NO
                            modelTransfPC.ListaPeajeCargo = this.servicioTransfPotencia.ListVtpPeajeCargoPagoAdicional(pericodi, recpotcodi, iPingcodi);
                            foreach (VtpPeajeCargoDTO dtoPeajeCargo in modelTransfPC.ListaPeajeCargo)
                            {
                                VtpPeajeEmpresaPagoDTO dtoPeajeEmpresaPago = new VtpPeajeEmpresaPagoDTO();
                                dtoPeajeEmpresaPago.Pericodi = pericodi;
                                dtoPeajeEmpresaPago.Recpotcodi = recpotcodi;
                                dtoPeajeEmpresaPago.Emprcodipeaje = dtoPeajeCargo.Emprcodi; //Codigo de la Empresa que recaudo
                                dtoPeajeEmpresaPago.Pingcodi = iPingcodi;
                                dtoPeajeEmpresaPago.Emprcodicargo = iEmprCodi; //Codigo de la Empresa Titular... si es cero hay que buscar en RepaRecaPeajeDetalle
                                dtoPeajeEmpresaPago.Pempagtransmision = sTrasmision;
                                if (dPeajeMensual != 0)
                                {   //Porcentaje (del total de todo lo recaudado por totas las empresas) / Peaje Mensual
                                    dtoPeajeEmpresaPago.Pempagpeajepago = dtoPeajeCargo.Pecarpeajerecaudado * dPeajeMensual;
                                }
                                else
                                {   //Importe que la empresa recaudo
                                    dtoPeajeEmpresaPago.Pempagpeajepago = dtoPeajeCargo.Pecarpeajerecaudado;
                                }
                                dtoPeajeEmpresaPago.Pempagusucreacion = User.Identity.Name;

                                //Hay que importar todos los saldos de Pagos de periodos atras (en caso existan: Pempagpericodidest = Pericodi) para sumarle a Pempagpeajepago
                                decimal dSaldoAnterior = 0;
                                if (recpotcodi == 1)
                                {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                                    int pempagpericodidest = pericodi;
                                    dSaldoAnterior = this.servicioTransfPotencia.GetPeajeEmpresaPagoSaldoAnterior(pempagpericodidest, dtoPeajeEmpresaPago.Pingcodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Emprcodicargo);
                                }
                                dtoPeajeEmpresaPago.Pempagsaldoanterior = dSaldoAnterior;
                                //dtoPeajeEmpresaPago.Pempagpeajepago += dSaldoAnterior; 20170505

                                //Consultamos por el Ajuste de saldo que se aplica a esta EmpresaPeaje, EmpresaCargo y Cargo en este Periodo
                                //decimal dPempajajuste = this.servicioTransfPotencia.GetPeajeEmpresaAjuste(pericodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Pingcodi, dtoPeajeEmpresaPago.Emprcodicargo);
                                decimal dPempajajuste = 0;
                                if (recpotcodi == 1)
                                {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                                    dPempajajuste = this.servicioTransfPotencia.GetPeajeCargoAjuste(pericodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Pingcodi);
                                }
                                dtoPeajeEmpresaPago.Pempagajuste = dPempajajuste;
                                //dtoPeajeEmpresaPago.Pempagpeajepago += dPempajajuste; 20170505

                                //Insertamos en VTP_PEAJE_EMPRESA_PAGO por el Pago Peaje por transmision
                                dtoPeajeEmpresaPago.Pempagcodi = this.servicioTransfPotencia.SaveVtpPeajeEmpresaPago(dtoPeajeEmpresaPago);
                                if (recpotcodi > 1)
                                {
                                    //Hay que encontrar el Saldo entre esta versión y la anterior y almacenarlo en VTP_PEAJE_EMPRESA_PAGO
                                    int iRecpotcodiOld = recpotcodi - 1;
                                    List<VtpPeajeEmpresaPagoDTO> dtoPeajeEmpresaPagoAnterior = new List<VtpPeajeEmpresaPagoDTO>();
                                    dtoPeajeEmpresaPagoAnterior = this.servicioTransfPotencia.GetByIdVtpPeajeEmpresaPagoSaldo(pericodi, iRecpotcodiOld, dtoPeajeEmpresaPago.Pingcodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Emprcodicargo).ToList();
                                    decimal pempagpeajepago = 0;
                                    if (dtoPeajeEmpresaPagoAnterior != null)
                                    {
                                        foreach (VtpPeajeEmpresaPagoDTO dtoPeajeEmpresaPagoAnt in dtoPeajeEmpresaPagoAnterior)
                                        {
                                            pempagpeajepago += dtoPeajeEmpresaPagoAnt.Pempagpeajepago;
                                        }
                                        dtoPeajeEmpresaPago.Pempagsaldo = dtoPeajeEmpresaPago.Pempagpeajepago - pempagpeajepago;
                                        dtoPeajeEmpresaPago.Pempagpericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                                        //Actualizamos el registro guardando el saldo y el periodo de destino
                                        this.servicioTransfPotencia.UpdateVtpPeajeEmpresaPago(dtoPeajeEmpresaPago);
                                    }
                                }

                            }
                        }
                        else
                        {   //A.2 - No existe la Empresa Titular -> buscar entre que empresas que estan relacionadas con el concepto “Reparto”, 
                            //Listamos las empresas involucradas en el reparto con su porcentaje: Generacion Adicional.
                            int iRrpecodi = Convert.ToInt32(dtoPeajeIngresoSi.Rrpecodi); //4.4.3 CU03 - Código del reparto de recaudación de peajes
                            modelTransfPC.ListaRepartosEmpresa = this.servicioTransfPotencia.ListVtpRepaRecaPeajeDetalles(pericodi, recpotcodi, iRrpecodi);
                            //Traemos la lista de Empresas que Recaudaron y Transmisión fue NO
                            modelTransfPC.ListaPeajeCargo = this.servicioTransfPotencia.ListVtpPeajeCargoPagoAdicional(pericodi, recpotcodi, iPingcodi);
                            //Traemos la lista de Empresas que Recaudaron
                            foreach (VtpPeajeCargoDTO dtoPeajeCargo in modelTransfPC.ListaPeajeCargo)
                            {
                                foreach (VtpRepaRecaPeajeDetalleDTO dtoRepartoEmpresa in modelTransfPC.ListaRepartosEmpresa)
                                {
                                    VtpPeajeEmpresaPagoDTO dtoPeajeEmpresaPago = new VtpPeajeEmpresaPagoDTO();
                                    dtoPeajeEmpresaPago.Pericodi = pericodi;
                                    dtoPeajeEmpresaPago.Recpotcodi = recpotcodi;
                                    dtoPeajeEmpresaPago.Emprcodipeaje = dtoPeajeCargo.Emprcodi; //Codigo de la Empresa que recaudo
                                    dtoPeajeEmpresaPago.Pingcodi = iPingcodi;
                                    dtoPeajeEmpresaPago.Emprcodicargo = dtoRepartoEmpresa.Emprcodi; //Codigo de la Empresa Titular por RepaRecaPeajeDetalle
                                    dtoPeajeEmpresaPago.Pempagtransmision = sTrasmision;
                                    if (dPeajeMensual != 0)
                                    {   //Porcentaje (del total de todo lo recaudado por todas las empresas) / Peaje Mensual
                                        dtoPeajeEmpresaPago.Pempagpeajepago = dtoPeajeCargo.Pecarpeajerecaudado * dPeajeMensual * Convert.ToDecimal(dtoRepartoEmpresa.Rrpdporcentaje) / 100;
                                    }
                                    else
                                    {   //Importe que la empresa recaudo
                                        dtoPeajeEmpresaPago.Pempagpeajepago = dtoPeajeCargo.Pecarpeajerecaudado * Convert.ToDecimal(dtoRepartoEmpresa.Rrpdporcentaje) / 100;
                                    }
                                    dtoPeajeEmpresaPago.Pempagusucreacion = User.Identity.Name;

                                    //Hay que importar todos los saldos de Pagos de periodos atras (en caso existan: Pempagpericodidest = Pericodi) para sumarle a Pempagpeajepago
                                    decimal dSaldoAnterior = 0;
                                    if (recpotcodi == 1)
                                    {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                                        int pempagpericodidest = pericodi;
                                        dSaldoAnterior = this.servicioTransfPotencia.GetPeajeEmpresaPagoSaldoAnterior(pempagpericodidest, dtoPeajeEmpresaPago.Pingcodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Emprcodicargo);
                                    }
                                    dtoPeajeEmpresaPago.Pempagsaldoanterior = dSaldoAnterior;
                                    //dtoPeajeEmpresaPago.Pempagpeajepago += dSaldoAnterior; 20170505

                                    //decimal dPempajajuste = this.servicioTransfPotencia.GetPeajeEmpresaAjuste(pericodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Pingcodi, dtoPeajeEmpresaPago.Emprcodicargo);
                                    decimal dPempajajuste = 0;
                                    if (recpotcodi == 1)
                                    {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                                        dPempajajuste = this.servicioTransfPotencia.GetPeajeCargoAjuste(pericodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Pingcodi);
                                    }
                                    dPempajajuste = dPempajajuste * Convert.ToDecimal(dtoRepartoEmpresa.Rrpdporcentaje) / 100;
                                    dtoPeajeEmpresaPago.Pempagajuste = dPempajajuste;
                                    //dtoPeajeEmpresaPago.Pempagpeajepago += dPempajajuste; 20170505

                                    //Insertamos en VTP_PEAJE_EMPRESA_PAGO por el Pago Peaje por transmision
                                    dtoPeajeEmpresaPago.Pempagcodi = this.servicioTransfPotencia.SaveVtpPeajeEmpresaPago(dtoPeajeEmpresaPago);
                                    if (recpotcodi > 1)
                                    {
                                        //Hay que encontrar el Saldo entre esta versión y la anterior y almacenarlo en VTP_PEAJE_EMPRESA_PAGO
                                        int iRecpotcodiOld = recpotcodi - 1;
                                        List<VtpPeajeEmpresaPagoDTO> dtoPeajeEmpresaPagoAnterior = new List<VtpPeajeEmpresaPagoDTO>();
                                        dtoPeajeEmpresaPagoAnterior = this.servicioTransfPotencia.GetByIdVtpPeajeEmpresaPagoSaldo(pericodi, iRecpotcodiOld, dtoPeajeEmpresaPago.Pingcodi, dtoPeajeEmpresaPago.Emprcodipeaje, dtoPeajeEmpresaPago.Emprcodicargo).ToList();
                                        decimal pempagpeajepago = 0;
                                        if (dtoPeajeEmpresaPagoAnterior != null)
                                        {
                                            foreach (VtpPeajeEmpresaPagoDTO dtoPeajeEmpresaPagoAnt in dtoPeajeEmpresaPagoAnterior)
                                            {
                                                pempagpeajepago += dtoPeajeEmpresaPagoAnt.Pempagpeajepago;
                                            }
                                            dtoPeajeEmpresaPago.Pempagsaldo = dtoPeajeEmpresaPago.Pempagpeajepago - pempagpeajepago;
                                            dtoPeajeEmpresaPago.Pempagpericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                                            //Actualizamos el registro guardando el saldo y el periodo de destino
                                            this.servicioTransfPotencia.UpdateVtpPeajeEmpresaPago(dtoPeajeEmpresaPago);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region A3: “Saldo por Peaje Transmisión” de una empresa Generadora: VTP_PEAJE_SALDO_TRANSMISION
                //Diferencia entre X:Peaje recaudado total (donde transmisión=Si)
                //menos Y:Peaje pagado total (el que se ajustó a transmisión=Si) 
                //ST=X-Y
                //Traemos la lista de Empresas que han recaudado y pagado por peaje de Transmision
                modelTransfPC.ListaPeajeSaldoTransmision = this.servicioTransfPotencia.GetByCriteriaVtpPeajeSaldoTransmision(pericodi, recpotcodi);
                foreach (VtpPeajeSaldoTransmisionDTO dtoPeajeSaldoTransmision in modelTransfPC.ListaPeajeSaldoTransmision)
                {
                    dtoPeajeSaldoTransmision.Pstrnsusucreacion = User.Identity.Name;
                    //Insertamos en VTP_PEAJE_SALDO_TRANSMISION
                    this.servicioTransfPotencia.SaveVtpPeajeSaldoTransmision(dtoPeajeSaldoTransmision);
                }

                #endregion

                #region C: Calculos de pagos de Egresos
                decimal dTotalPagoEgreso = 0;
                //lista de empresas en el mes de valorización y versión de recalculo: EmprCodi y EmprNomb de la vista VW_VTP_PEAJE_EGRESO
                foreach (VtpPeajeEgresoMinfoDTO dtoPeajeEgresoEmpresa in modelTransfPC.ListaPeajeEgresoEmpresa)
                {
                    //Para cada Empresa Generadora: EmprCodi y EmprNomb
                    int iEmprCodi = (int)dtoPeajeEgresoEmpresa.Genemprcodi;
                    string sEmprNomb = dtoPeajeEgresoEmpresa.Genemprnombre;
                    decimal dEgresoEmpresa = 0;
                    decimal dSaldoEmpresa = 0;
                    //Consultar si la empresa tiene FactorProporcion
                    decimal dFactorProporcion = 0;
                    foreach (IngresoRetiroSCDTO dtoFactoProporcion in modelTransfPC.ListaFactorProporcion)
                    {
                        if (dtoFactoProporcion.EmprCodi == iEmprCodi)
                        {
                            var ListMigra = this.servicioTransferencia.ListTrnMigracionDTI();
                            var emprOrigen = ListMigra.Where(x => x.Emprcodidestino == iEmprCodi).ToList();
                            foreach (var empr in emprOrigen)
                            {
                                var ListFactorP = modelTransfPC.ListaFactorProporcion.Where(x => x.EmprCodi == empr.Emprcodidestino).ToList();
                                foreach (var y in ListFactorP)
                                {
                                    dFactorProporcion = y.IngrscImporteVtp;
                                    foreach (VtpRetiroPotescDTO dtoRetiroPotenciaSC in modelTransfPC.ListaRetiroPotenciaSC)
                                    {
                                        dEgresoEmpresa += dFactorProporcion * Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpoteegreso) * Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpreciopote); //(Ojo: Rpscprecioppb)
                                    }
                                }
                                break;
                            }
                            if (dFactorProporcion != 0)
                            {
                                break;
                            }
                            dFactorProporcion = dtoFactoProporcion.IngrscImporteVtp;
                            foreach (VtpRetiroPotescDTO dtoRetiroPotenciaSC in modelTransfPC.ListaRetiroPotenciaSC)
                            {
                                dEgresoEmpresa += dFactorProporcion * Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpoteegreso) * Convert.ToDecimal(dtoRetiroPotenciaSC.Rpscpreciopote); //(Ojo: Rpscprecioppb)
                            }
                            break;
                        }
                    }
                    //Traemos la lista de Egresos y Peajes / Mejor información de cada Cliente de la vista VW_VTP_PEAJE_EGRESO
                    modelTransfPC.ListaPeajeEgreso = this.servicioTransfPotencia.ListVtpPeajeEgresoMinfoEmpresaRecalculo(pericodi, recpotcodi, iEmprCodi);
                    foreach (VtpPeajeEgresoMinfoDTO dtoPeajeEgreso in modelTransfPC.ListaPeajeEgreso)
                    {
                        //Para cada cliente, realizamos los calculos de Pago Egreso:
                        dEgresoEmpresa += Convert.ToDecimal(dtoPeajeEgreso.Pegrmipoteegreso) * Convert.ToDecimal(dtoPeajeEgreso.Pegrmipreciopote);
                    }
                    //Traemos el SaldoTransmision de la empresa en el periodo version
                    modelTransfPC.EntidadPeajeSaldoTransmision = this.servicioTransfPotencia.GetByIdVtpPeajeSaldoTransmisionEmpresa(pericodi, recpotcodi, iEmprCodi);
                    if (modelTransfPC.EntidadPeajeSaldoTransmision != null)
                    {
                        dSaldoEmpresa = modelTransfPC.EntidadPeajeSaldoTransmision.Pstrnssaldotransmision;
                    }
                    // D. CALCULO DE EGRESO TOTAL
                    dTotalPagoEgreso += dEgresoEmpresa + dSaldoEmpresa;
                    VtpPagoEgresoDTO dtoPagoEgreso = new VtpPagoEgresoDTO();
                    dtoPagoEgreso.Pericodi = pericodi;
                    dtoPagoEgreso.Recpotcodi = recpotcodi;
                    dtoPagoEgreso.Emprcodi = iEmprCodi;
                    dtoPagoEgreso.Pagegregreso = dEgresoEmpresa;
                    dtoPagoEgreso.Pagegrsaldo = dSaldoEmpresa;
                    dtoPagoEgreso.Pagegrpagoegreso = dEgresoEmpresa + dSaldoEmpresa;
                    dtoPagoEgreso.Pagegrusucreacion = User.Identity.Name;
                    this.servicioTransfPotencia.SaveVtpPagoEgreso(dtoPagoEgreso);
                }
                #endregion

                if (dTotalPagoEgreso == 0)
                {
                    sResultado = "Lo sentimos, el Ingreso Garantizado por potencia Firme es igual a cero.";
                    return Json(sResultado);
                }

                #region E. INGRESO POR POTENCIA = EGRESO TOTAL
                decimal dIngresoPorPotenciaTotal = dTotalPagoEgreso;

                //var flag = false; 
                if (modelTransfPC.EntidadRecalculoPotencia.Recpotcargapfr != 1)
                {
                    //Lista de Intervalos de Potencia Efectiva, Firme y Firme Remunerable
                    //Para obtener el CUADRO 13: INGRESO GARANTIZADO POR POTENCIA FIRME
                    modelTransfPC.ListaIngresoPotEFR = this.servicioTransfPotencia.GetByCriteriaVtpIngresoPotefrs(pericodi, recpotcodi);
                    foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in modelTransfPC.ListaIngresoPotEFR)
                    {
                        //Para cada Intervalo: Determinar el IngresoPotenciaUnidad
                        modelTransfPC.ListaIngresoPotEFRDetalle = this.servicioTransfPotencia.GetByCriteriaVtpIngresoPotefrDetallesSumCentral(dtoIngresoPotEFR.Ipefrcodi, pericodi, recpotcodi);
                        //Primero totalizamos en este intervalo PotFirmeRemurable del Periodo
                        decimal dPotFirmeRemurable = 0;
                        foreach (VtpIngresoPotefrDetalleDTO dtoIngresoPotEFRDetalle in modelTransfPC.ListaIngresoPotEFRDetalle)
                        {
                            decimal dIngresoPreliminar = Convert.ToDecimal(dtoIngresoPotEFRDetalle.Ipefrdpotefirmeremunerable) * Convert.ToDecimal(modelTransfPC.EntidadRecalculoPotencia.Recpotpreciopoteppm);
                            dPotFirmeRemurable += dIngresoPreliminar;
                        }
                        if (dPotFirmeRemurable == 0)
                        {
                            sResultado = "Lo sentimos, los datos de Potencia Efectiva, Firme y Firme Remunerable son nulas";
                            return Json(sResultado);
                        }
                        decimal dFactorAjuste = dIngresoPorPotenciaTotal / dPotFirmeRemurable; //Factor de Ajuste del Ingreso Garantizado - OK
                                                                                               //Aplicamos la formula: IPUnidad = dIngresoPorPotenciaTotal * (PotEFR_Unidad/dPotFirmeRemurable)
                        foreach (VtpIngresoPotefrDetalleDTO dtoIngresoPotEFRDetalle in modelTransfPC.ListaIngresoPotEFRDetalle)
                        {
                            decimal dPotIPUnidad = dFactorAjuste * Convert.ToDecimal(dtoIngresoPotEFRDetalle.Ipefrdpotefirmeremunerable) * Convert.ToDecimal(modelTransfPC.EntidadRecalculoPotencia.Recpotpreciopoteppm);
                            //Insertamos La potencia Efectiva, Firme y Firme Remunerable de la Unidad en el intervalo
                            VtpIngresoPotUnidIntervlDTO dtoIngresoPotUnidIntervl = new VtpIngresoPotUnidIntervlDTO();
                            dtoIngresoPotUnidIntervl.Pericodi = pericodi;
                            dtoIngresoPotUnidIntervl.Recpotcodi = recpotcodi;
                            dtoIngresoPotUnidIntervl.Emprcodi = Convert.ToInt32(dtoIngresoPotEFRDetalle.Emprcodi);
                            dtoIngresoPotUnidIntervl.Equicodi = Convert.ToInt32(dtoIngresoPotEFRDetalle.Cenequicodi);
                            dtoIngresoPotUnidIntervl.Ipefrcodi = dtoIngresoPotEFR.Ipefrcodi; //Codigo del Intervalo
                            dtoIngresoPotUnidIntervl.Inpuinintervalo = dtoIngresoPotEFR.Ipefrintervalo; //Intervalo
                            dtoIngresoPotUnidIntervl.Inpuindia = Convert.ToInt32(dtoIngresoPotEFR.Ipefrdia); //Nro. dias en el intervalo
                            dtoIngresoPotUnidIntervl.Inpuinimporte = dPotIPUnidad;
                            dtoIngresoPotUnidIntervl.Inpuinusucreacion = User.Identity.Name;
                            //Grabamos
                            this.servicioTransfPotencia.SaveVtpIngresoPotUnidIntervl(dtoIngresoPotUnidIntervl);
                        }
                    } //Fin de Intervalos
                      //La Potencia Firme Remunerable de cada Unidad (Central) en el mes de valorización: VTP_INGRESO_POTUNID_PROMD
                      //Ingreso Garantizado por Potencia Firme por empresas
                    modelTransfPC.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    int iNumDiasMes = System.DateTime.DaysInMonth(modelTransfPC.EntidadPeriodo.AnioCodi, modelTransfPC.EntidadPeriodo.MesCodi);
                    modelTransfPC.ListaIngresoPotenciaUnidad = this.servicioTransfPotencia.ListVtpIngresoPotUnidIntervlSumIntervl(pericodi, recpotcodi);
                    foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoPotenciaUnidad in modelTransfPC.ListaIngresoPotenciaUnidad)
                    {
                        //IPUnidadPromedio = sum(dPotIPUnidad * dtoIngresoPotEFR.Ipefrdia)/NumeroDiasMes 
                        decimal dImportePromedio = dtoIngresoPotenciaUnidad.Inpuinimporte / iNumDiasMes;
                        VtpIngresoPotUnidPromdDTO dtoIngresoPotUnidPromd = new VtpIngresoPotUnidPromdDTO();
                        dtoIngresoPotUnidPromd.Pericodi = pericodi;
                        dtoIngresoPotUnidPromd.Recpotcodi = recpotcodi;
                        dtoIngresoPotUnidPromd.Emprcodi = dtoIngresoPotenciaUnidad.Emprcodi;
                        dtoIngresoPotUnidPromd.Equicodi = dtoIngresoPotenciaUnidad.Equicodi;
                        dtoIngresoPotUnidPromd.Inpuprimportepromd = dImportePromedio;
                        dtoIngresoPotUnidPromd.Inpuprusucreacion = User.Identity.Name;
                        //Grabamos
                        this.servicioTransfPotencia.SaveVtpIngresoPotUnidPromd(dtoIngresoPotUnidPromd);
                    }
                }
                else
                {
                    sResultado = servicioTransfPotencia.ProcesarValorizacionDetallePfr(pericodi, recpotcodi, modelTransfPC.EntidadRecalculoPotencia, dIngresoPorPotenciaTotal, User.Identity.Name);
                }
                if (sResultado != "1")
                    return Json(sResultado);

                #endregion

                #region E. CALCULO DE SALDOS
                //IngresoPotenciaEmpresa = Suma(IngresosPotenciaUnidades)
                modelTransfPC.ListaIngresoPotenciaEmpresa = this.servicioTransfPotencia.ListVtpIngresoPotUnidPromdEmpresa(pericodi, recpotcodi);
                foreach (VtpIngresoPotUnidPromdDTO dtoIngresoPotenciaEmpresa in modelTransfPC.ListaIngresoPotenciaEmpresa)
                {
                    VtpIngresoPotenciaDTO dtoIngresoPotencia = new VtpIngresoPotenciaDTO();
                    dtoIngresoPotencia.Pericodi = pericodi;
                    dtoIngresoPotencia.Recpotcodi = recpotcodi;
                    dtoIngresoPotencia.Emprcodi = dtoIngresoPotenciaEmpresa.Emprcodi;
                    dtoIngresoPotencia.Potipimporte = dtoIngresoPotenciaEmpresa.Inpuprimportepromd;
                    dtoIngresoPotencia.Potipporcentaje = (dtoIngresoPotenciaEmpresa.Inpuprimportepromd / dIngresoPorPotenciaTotal);
                    dtoIngresoPotencia.Potipusucreacion = User.Identity.Name;
                    //Grabar
                    this.servicioTransfPotencia.SaveVtpIngresoPotencia(dtoIngresoPotencia);
                }
                //Saldo de cada Empresa de Generación = IngresoPotenciaEmpresa - Egreso
                modelTransfPC.ListaSaldoEmpresa = this.servicioTransfPotencia.ListVtpSaldoEmpresasCalculaSaldo(pericodi, recpotcodi);
                /*foreach (var saldoEmp in modelTransfPC.ListaSaldoEmpresa)
                {
                    // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                    var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == saldoEmp.Emprcodi).ToList();
                    if (empresaTTIE.Count > 0) saldoEmp.Emprcodi = empresaTTIE.First().Emprcodidestino;
                    // FIN
                }
                var duplicadosSaldo = modelTransfPC.ListaSaldoEmpresa.GroupBy(x => x.Emprcodi).Where(emp => emp.Count() > 1).Select(x => x.Key).ToList();
                if (duplicadosSaldo.Any())
                {
                    foreach (var codigoSaldoDuplicado in duplicadosSaldo)
                    {
                        decimal sumIng = modelTransfPC.ListaSaldoEmpresa.Where(x => x.Emprcodi == codigoSaldoDuplicado).Sum(x => x.Potseingreso);
                        decimal sumEgr = modelTransfPC.ListaSaldoEmpresa.Where(x => x.Emprcodi == codigoSaldoDuplicado).Sum(x => x.Potseegreso);
                        decimal sumSal = modelTransfPC.ListaSaldoEmpresa.Where(x => x.Emprcodi == codigoSaldoDuplicado).Sum(x => x.Potsesaldo);
                        var dtoICd = modelTransfPC.ListaSaldoEmpresa.Where(x => x.Emprcodi == codigoSaldoDuplicado).FirstOrDefault();
                        dtoICd.Potseingreso = sumIng;
                        dtoICd.Potseegreso = sumEgr;
                        dtoICd.Potsesaldo = sumSal;
                        modelTransfPC.ListaSaldoEmpresa.RemoveAll(x => x.Emprcodi == codigoSaldoDuplicado);
                        modelTransfPC.ListaSaldoEmpresa.Add(dtoICd);
                    }
                }*/
                foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresa in modelTransfPC.ListaSaldoEmpresa)
                {
                    dtoSaldoEmpresa.Pericodi = pericodi;
                    dtoSaldoEmpresa.Recpotcodi = recpotcodi;
                    dtoSaldoEmpresa.Potseusucreacion = User.Identity.Name;

                    decimal dSaldoAnterior = 0;
                    //Hay que importar todos los saldos de cargos de periodos atras (en caso existan: Pecarpericodidest = Pericodi) para sumarle a dPeajeRecaudado
                    if (recpotcodi == 1)
                    {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                        int potsepericodidest = pericodi;
                        dSaldoAnterior = this.servicioTransfPotencia.GetSaldoEmpresaSaldoAnterior(potsepericodidest, dtoSaldoEmpresa.Emprcodi);
                    }
                    dtoSaldoEmpresa.Potsesaldoanterior = dSaldoAnterior;
                    //dtoSaldoEmpresa.Potsesaldo += dSaldoAnterior;

                    //Consultamos por el Ajuste de saldo que se aplica a esta Empresa en este Periodo
                    decimal dPotseaajajuste = 0;
                    if (recpotcodi == 1)
                    {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                        dPotseaajajuste = this.servicioTransfPotencia.GetSaldoEmpresaAjuste(pericodi, dtoSaldoEmpresa.Emprcodi);
                    }
                    dtoSaldoEmpresa.Potseajuste = dPotseaajajuste;
                    //dtoSaldoEmpresa.Potsesaldo += dPotseaajajuste;

                    //Grabar
                    dtoSaldoEmpresa.Potsecodi = this.servicioTransfPotencia.SaveVtpSaldoEmpresa(dtoSaldoEmpresa);
                    //En caso corresponsa a un recalculo...
                    if (recpotcodi > 1)
                    {
                        //Hay que encontrar el Saldo entre esta versión y la anterior
                        int iRecpotcodiOld = recpotcodi - 1;
                        //VtpSaldoEmpresaDTO dtoSaldoEmpresaAnterior = new VtpSaldoEmpresaDTO();
                        //dtoSaldoEmpresaAnterior = this.servicioTransfPotencia.GetByIdVtpSaldoEmpresaSaldo(pericodi, iRecpotcodiOld, dtoSaldoEmpresa.Emprcodi);


                        //if (dtoSaldoEmpresaAnterior != null)
                        //{
                        //    dtoSaldoEmpresa.Potsesaldoreca = dtoSaldoEmpresa.Potsesaldo - dtoSaldoEmpresaAnterior.Potsesaldo;
                        //    dtoSaldoEmpresa.Potsepericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                        //    //Actualizamos el registro guardando el saldo y el periodo de destino
                        //    this.servicioTransfPotencia.UpdateVtpSaldoEmpresa(dtoSaldoEmpresa);
                        //}

                        //VtpSaldoEmpresaDTO dtoSaldoEmpresaAnterior = new VtpSaldoEmpresaDTO();
                        //dtoSaldoEmpresaAnterior = this.servicioTransfPotencia.GetByIdVtpSaldoEmpresaSaldo(pericodi, iRecpotcodiOld, dtoSaldoEmpresa.Emprcodi);

                        List<VtpSaldoEmpresaDTO> listSaldoEmpresa = this.servicioTransfPotencia.GetByIdVtpSaldoEmpresaSaldoGeneral(pericodi, iRecpotcodiOld, dtoSaldoEmpresa.Emprcodi);

                        decimal saldoEmpresaAnterior = 0;
                        foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresaAnterior in listSaldoEmpresa)
                        {
                            saldoEmpresaAnterior = saldoEmpresaAnterior + dtoSaldoEmpresaAnterior.Potsesaldo;
                        }

                        //if (saldoEmpresaAnterior != 0)
                        //{
                        dtoSaldoEmpresa.Potsesaldoreca = dtoSaldoEmpresa.Potsesaldo - saldoEmpresaAnterior;
                        dtoSaldoEmpresa.Potsepericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                        //Actualizamos el registro guardando el saldo y el periodo de destino
                        this.servicioTransfPotencia.UpdateVtpSaldoEmpresa(dtoSaldoEmpresa);
                        //}

                    }
                }

                #endregion

                #region F CALCULOS DE LA MATRIZ DE PAGOS
                //Si la version == 1 procesamos la matriz de pagos
                if (recpotcodi == 1)
                {
                    //Calculando y asignando las pagos entre empresas en la Matriz de Pagos -> VTP_EMPRESA_PAGO
                    //Traemos la lista de empresa con montos positivos
                    modelTransfPC.ListaSaldoEmpresaPositiva = this.servicioTransfPotencia.ListVtpSaldoEmpresasPositiva(pericodi, recpotcodi);
                    //Traemos la lista de empresa con su importe negativo y el importe total negativo de todas las empresas
                    modelTransfPC.ListaSaldoEmpresaNegativa = this.servicioTransfPotencia.ListVtpSaldoEmpresasNegativa(pericodi, recpotcodi);

                    if (modelTransfPC.ListaSaldoEmpresaPositiva.Count > 0 && modelTransfPC.ListaSaldoEmpresaNegativa.Count > 0)
                    {
                        //Debe existir al menos una empresa positiva y una negativa
                        foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresaNegativa in modelTransfPC.ListaSaldoEmpresaNegativa)
                        {
                            //Para cada empresa P con un Valor positivo
                            foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresaPostivo in modelTransfPC.ListaSaldoEmpresaPositiva)
                            {
                                VtpEmpresaPagoDTO dtoEmpresaPago = new VtpEmpresaPagoDTO();
                                //Distribuimos el Importe P entre las empresa negativas N
                                dtoEmpresaPago.Pericodi = pericodi; //Mes de valorización de recalculo
                                dtoEmpresaPago.Recpotcodi = recpotcodi; //Versión de recalculo
                                dtoEmpresaPago.Potsecodi = dtoSaldoEmpresaNegativa.Potsecodi; //Codigo de la tabla VTP_SALDO_EMPRESA P
                                dtoEmpresaPago.Emprcodipago = dtoSaldoEmpresaNegativa.Emprcodi; //Codigo de la empresa P que pago
                                dtoEmpresaPago.Emprcodicobro = dtoSaldoEmpresaPostivo.Emprcodi; //Codigo de la tabla empresa  N que cobro 
                                //Importe a pagar
                                decimal dPositivo = dtoSaldoEmpresaPostivo.Potsesaldo + dtoSaldoEmpresaPostivo.Potsesaldoanterior + dtoSaldoEmpresaPostivo.Potseajuste;
                                decimal dNegativo = dtoSaldoEmpresaNegativa.Potsesaldo + dtoSaldoEmpresaNegativa.Potsesaldoanterior + dtoSaldoEmpresaNegativa.Potseajuste;
                                dtoEmpresaPago.Potepmonto = (dPositivo / dtoSaldoEmpresaPostivo.Potsetotalsaldopositivo) * dNegativo;
                                dtoEmpresaPago.Potepusucreacion = User.Identity.Name;
                                //Grabar
                                this.servicioTransfPotencia.SaveVtpEmpresaPago(dtoEmpresaPago);
                            }
                        }
                    }
                }
                #endregion

                #region B: Calculo de pagos Ingresos Tarifarios
                //Lista de cargos Pago=si / transmision = si, para trabajar con en campo IngresoTarifario
                modelTransfPC.ListaPeajeIngresoCargo = this.servicioTransfPotencia.ListVtpPeajeIngresoTarifarioMensual(pericodi, recpotcodi);
                //Lista de Empresa con su porcentaje de Ingreso por potencia
                modelTransfPC.ListaVtpIngresoPotencia = this.servicioTransfPotencia.ListVtpIngresoPotenciaEmpresa(pericodi, recpotcodi);
                foreach (VtpIngresoPotenciaDTO dtoIngresoPotencia in modelTransfPC.ListaVtpIngresoPotencia)
                {   //ASSETEC 20190627: Potipsaldoanterior si la empresa tiene un saldo asignado de otro periodo
                    if (dtoIngresoPotencia.Potipporcentaje <= 0 && dtoIngresoPotencia.Potipsaldoanterior == 0)
                    {
                        continue;
                    }
                    foreach (VtpPeajeIngresoDTO dtoPeajeIngreso in modelTransfPC.ListaPeajeIngresoCargo)
                    {
                        VtpIngresoTarifarioDTO dtoIngresotarifario = new VtpIngresoTarifarioDTO();
                        dtoIngresotarifario.Pericodi = pericodi;
                        dtoIngresotarifario.Recpotcodi = recpotcodi;
                        dtoIngresotarifario.Pingcodi = dtoPeajeIngreso.Pingcodi;
                        dtoIngresotarifario.Ingtartarimensual = Convert.ToDecimal(dtoPeajeIngreso.Pingtarimensual);
                        dtoIngresotarifario.Emprcodingpot = Convert.ToInt32(dtoIngresoPotencia.Emprcodi);
                        dtoIngresotarifario.Ingtarporcentaje = Convert.ToDecimal(dtoIngresoPotencia.Potipporcentaje);
                        if ((dtoPeajeIngreso.Emprcodi != null) && (dtoPeajeIngreso.Emprcodi > 0))
                        {
                            dtoIngresotarifario.Emprcodiping = Convert.ToInt32(dtoPeajeIngreso.Emprcodi);
                            dtoIngresotarifario.Ingtarimporte = dtoIngresotarifario.Ingtartarimensual * dtoIngresotarifario.Ingtarporcentaje;
                        }
                        else
                        {   //Listar la lista de empresas y porcentajes por reparto
                            int iRrpecodi = Convert.ToInt32(dtoPeajeIngreso.Rrpecodi); //4.4.3 CU03 - Código del reparto de recaudación de peajes
                            modelTransfPC.ListaRepartosEmpresa = this.servicioTransfPotencia.ListVtpRepaRecaPeajeDetalles(pericodi, recpotcodi, iRrpecodi);
                            foreach (VtpRepaRecaPeajeDetalleDTO dtoRepartoEmpresa in modelTransfPC.ListaRepartosEmpresa)
                            {
                                dtoIngresotarifario.Emprcodiping = Convert.ToInt32(dtoRepartoEmpresa.Emprcodi);
                                dtoIngresotarifario.Ingtarimporte = dtoIngresotarifario.Ingtartarimensual * dtoIngresotarifario.Ingtarporcentaje * Convert.ToDecimal(dtoRepartoEmpresa.Rrpdporcentaje);
                            }
                        }
                        dtoIngresotarifario.Ingtarusucreacion = User.Identity.Name;

                        //Hay que importar todos los saldos de cargos de periodos atras (en caso existan: Pecarpericodidest = Pericodi) para sumarle a dPeajeRecaudado
                        decimal dSaldoAnterior = 0;
                        if (recpotcodi == 1)
                        {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                            int ingtarpericodidest = pericodi;
                            dSaldoAnterior = this.servicioTransfPotencia.GetIngresoTarifarioSaldoAnterior(ingtarpericodidest, dtoIngresotarifario.Pingcodi, dtoIngresotarifario.Emprcodiping, dtoIngresotarifario.Emprcodingpot);
                        }
                        dtoIngresotarifario.Ingtarsaldoanterior = dSaldoAnterior;
                        //dtoIngresotarifario.Ingtarimporte += dSaldoAnterior; 20170505

                        //Consultamos por el Ajuste de saldo que se aplica a esta EmpresaCargo, Cargo, EmpresaIngresoPotencia en este Periodo
                        decimal dIngtajajuste = 0;
                        if (recpotcodi == 1)
                        {   //EDUARDO: 20170530: Las revisiones no deben calcular saldo anterior
                            dIngtajajuste = this.servicioTransfPotencia.GetIngresoTarifarioAjuste(pericodi, dtoIngresotarifario.Emprcodingpot, dtoIngresotarifario.Pingcodi, dtoIngresotarifario.Emprcodiping);
                        }
                        dtoIngresotarifario.Ingtarajuste += dIngtajajuste;
                        //dtoIngresotarifario.Ingtarimporte += dIngtajajuste; 20170505

                        dtoIngresotarifario.Ingtarcodi = this.servicioTransfPotencia.SaveVtpIngresoTarifario(dtoIngresotarifario);
                        //En caso corresponsa a un recalculo...                       
                        if (recpotcodi > 1)
                        {
                            ////Hay que encontrar el Saldo entre esta versión y la anterior y almacenarlo en VTP_INGRESO_TARIFARIO
                            //int iRecpotcodiOld = recpotcodi - 1;
                            //VtpIngresoTarifarioDTO dtoIngresotarifarioAnterior = new VtpIngresoTarifarioDTO();
                            //dtoIngresotarifarioAnterior = this.servicioTransfPotencia.GetByIdVtpIngresoTarifarioSaldo(pericodi, iRecpotcodiOld, dtoIngresotarifario.Pingcodi, dtoIngresotarifario.Emprcodiping, dtoIngresotarifario.Emprcodingpot);


                            //if (dtoIngresotarifarioAnterior != null)
                            //{
                            //    dtoIngresotarifario.Ingtarsaldo = dtoIngresotarifario.Ingtarimporte - dtoIngresotarifarioAnterior.Ingtarimporte;
                            //    dtoIngresotarifario.Ingtarpericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                            //    //Actualizamos el registro guardando el saldo y el periodo de destino
                            //    this.servicioTransfPotencia.UpdateVtpIngresoTarifario(dtoIngresotarifario);
                            //}
                            //else
                            //{
                            //    dtoIngresotarifario.Ingtarsaldo = dtoIngresotarifario.Ingtarimporte - 0;
                            //    dtoIngresotarifario.Ingtarpericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                            //    //Actualizamos el registro guardando el saldo y el periodo de destino
                            //    this.servicioTransfPotencia.UpdateVtpIngresoTarifario(dtoIngresotarifario);

                            //}

                            //Hay que encontrar el Saldo entre esta versión y la anterior y almacenarlo en VTP_INGRESO_TARIFARIO
                            int iRecpotcodiOld = recpotcodi - 1;

                            List<VtpIngresoTarifarioDTO> listaIngresotarifarioAnterior = this.servicioTransfPotencia.GetByCriteriaIngresoTarifarioSaldo(pericodi, iRecpotcodiOld, dtoIngresotarifario.Pingcodi,
                                dtoIngresotarifario.Emprcodiping, dtoIngresotarifario.Emprcodingpot);

                            if (listaIngresotarifarioAnterior.Count > 0)
                            {
                                decimal saldoAnterior = 0;
                                foreach (VtpIngresoTarifarioDTO itemSaldoAnterior in listaIngresotarifarioAnterior)
                                {
                                    saldoAnterior = saldoAnterior + itemSaldoAnterior.Ingtarimporte;
                                }

                                dtoIngresotarifario.Ingtarsaldo = dtoIngresotarifario.Ingtarimporte - saldoAnterior;
                                dtoIngresotarifario.Ingtarpericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                                //Actualizamos el registro guardando el saldo y el periodo de destino
                                this.servicioTransfPotencia.UpdateVtpIngresoTarifario(dtoIngresotarifario);
                            }
                            else
                            {
                                dtoIngresotarifario.Ingtarsaldo = dtoIngresotarifario.Ingtarimporte - 0;
                                dtoIngresotarifario.Ingtarpericodidest = (int)modelTransfPC.EntidadRecalculoPotencia.Pericodidestino;
                                //Actualizamos el registro guardando el saldo y el periodo de destino
                                this.servicioTransfPotencia.UpdateVtpIngresoTarifario(dtoIngresotarifario);
                            }
                        }
                    }
                }

                #endregion
                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoriaF = new VtpAuditoriaProcesoDTO();
                objAuditoriaF.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.ProcesarValorizacionProcesar;
                objAuditoriaF.Estdcodi = (int)EVtpEstados.CalcularValorizacion;
                objAuditoriaF.Audproproceso = "Finaliza la valorización del periodo";
                objAuditoriaF.Audprodescripcion = "Finaliza la valorización del periodo " + dtoPeriodo.PeriNombre + " / " + modelTransfPC.EntidadRecalculoPotencia.Recpotnombre + " - cantidad de errores - 0";
                objAuditoriaF.Audprousucreacion = User.Identity.Name;
                objAuditoriaF.Audprofeccreacion = DateTime.Now;

                int auditoriaF = this.servicioAuditoria.save(objAuditoriaF);

                #endregion
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        /// <summary>
        /// Permite borrar la valorización de transferencia de potencia y compensación del periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BorrarValorizacion(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();

            string sResultado = "1";
            if (pericodi == 0 || recpotcodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de valorización y una versión de recalculo";
                return Json(sResultado);
            }
            try
            {
                //Eliminamos la información procesada en el periodo / versión
                string sBorrar = this.servicioTransfPotencia.EliminarProceso(pericodi, recpotcodi);
                if (!sBorrar.Equals("1"))
                {
                    return Json(sBorrar);
                }
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }
            #region AuditoriaProceso
            PeriodoDTO dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
            RecalculoDTO dtoRecalculo2 = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, recpotcodi);
            VtpAuditoriaProcesoDTO objAuditoriaF = new VtpAuditoriaProcesoDTO();
            objAuditoriaF.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.ProcesarValorizacionBorrar;
            objAuditoriaF.Estdcodi = (int)EVtpEstados.CalcularValorizacion;
            objAuditoriaF.Audproproceso = "Se borró valorización";
            objAuditoriaF.Audprodescripcion = "Se eliminó la valorización del periodo " + dtoPeriodo.PeriNombre + " / " + dtoRecalculo2.RecaNombre + " - cantidad de errores - 0";
            objAuditoriaF.Audprousucreacion = User.Identity.Name;
            objAuditoriaF.Audprofeccreacion = DateTime.Now;

            _ = this.servicioAuditoria.save(objAuditoriaF);

            #endregion
            return Json(sResultado);
        }

        /// <summary>
        /// Permite exportar a un archivo excel/pdf los resultados del mes de valorización y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int recpotcodi = 0, string reporte = "", int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                ExcelWorksheet hoja = null;
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = "-1";
                if (reporte.Equals("PeajeEgreso"))
                {
                    //Los Peaje Egreso de todos los generadores
                    file = this.servicioTransfPotencia.GenerarFormatoPeajeEgresoMinfo(pericodi, recpotcodi, 0, formato, pathFile, pathLogo, 0, 0, 0, "", "", "", out hoja);
                }
                else if (reporte.Equals("RetirosSC"))
                {
                    //CU17 Retiros sin contrato
                    file = this.servicioTransfPotencia.GenerarReporteRetirosSC(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("PeajePagarse"))
                {
                    //CU18 Peajes a pagarse
                    file = this.servicioTransfPotencia.GenerarReportePeajePagarse(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);

                }
                else if (reporte.Equals("IngresoTarifario"))
                {
                    //CU19 Ingresos tarifarios
                    file = this.servicioTransfPotencia.GenerarReporteIngresoTarifario(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("PeajeRecaudado"))
                {
                    //CU20 Peajes recaudados
                    file = this.servicioTransfPotencia.GenerarReportePeajeRecaudado(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("PotenciaValor"))
                {
                    //CU21 Potencia y Valorización
                    file = this.servicioTransfPotencia.GenerarReportePotenciaValor(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("Egresos"))
                {
                    //CU22 Egresos
                    file = this.servicioTransfPotencia.GenerarReporteEgresos(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("IngresoPotencia"))
                {
                    //CU23 Ingresos por potencia
                    file = this.servicioTransfPotencia.GenerarReporteIngresoPotencia(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("ValorTransfPotencia"))
                {
                    //CU24 Valorización de transferencias de potencia
                    file = this.servicioTransfPotencia.GenerarReporteValorTransfPotencia(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("Matriz"))
                {
                    //CU25 Matriz de pagos - Solo se muestra en versión mensual
                    file = this.servicioTransfPotencia.GenerarReporteMatriz(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("General"))
                {
                    List<string> sources = new List<string>();

                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarFormatoPeajeEgresoMinfo(pericodi, recpotcodi, 0, formato, pathFile, pathLogo, 0, 0, 0, "", "", "", out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReporteRetirosSC(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReportePeajePagarse(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReporteIngresoTarifario(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReportePeajeRecaudado(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReportePotenciaValor(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReporteEgresos(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReporteIngresoPotencia(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReporteValorTransfPotencia(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));
                    sources.Add(pathFile + this.servicioTransfPotencia.GenerarReporteMatriz(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja));

                    file = "ReporteVTP.zip";
                    GeneracionZip.AddToArchive(pathFile + file,
                    sources,
                    GeneracionZip.ArchiveAction.Replace,
                    GeneracionZip.Overwrite.IfNewer,
                    System.IO.Compression.CompressionLevel.Optimal);
                }
                else if (reporte.Equals("Unificado"))
                {
                    file = this.servicioTransfPotencia.GenerarReporteUnificado(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);

                    //file = "ReporteConsolidadoVTP.xlsx";
                    //FileInfo newFile = new FileInfo(pathFile + file);

                    //if (newFile.Exists)
                    //{
                    //    newFile.Delete();
                    //    newFile = new FileInfo(pathFile + file);
                    //}

                    //using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    //{
                    //    //this.servicioTransfPotencia.GenerarFormatoPeajeEgresoMinfo(pericodi, recpotcodi, 0, formato, pathFile, pathLogo, 0, 0, 0, "", "", "", out hoja);
                    //    //xlPackage.Workbook.Worksheets.Add("INFORMACIÓN INGRESADA PARA VTP Y PEAJES", hoja);

                    //    this.servicioTransfPotencia.GenerarReporteRetirosSC(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add(hoja.Name, hoja);

                    //    this.servicioTransfPotencia.GenerarReportePeajePagarse(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add("PEAJE DE CONEXIÓN Y TRANSMISIÓN", hoja);

                    //    this.servicioTransfPotencia.GenerarReporteIngresoTarifario(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add("INGRESO TARIFARIO", hoja);

                    //    this.servicioTransfPotencia.GenerarReportePeajeRecaudado(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add("COMPENSACIONES INCLUIDAS EN EL PEAJE POR CONEXIÓN", hoja);

                    //    this.servicioTransfPotencia.GenerarReportePotenciaValor(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add("RESUMEN DE INFORMACIÓN VTP", hoja);

                    //    this.servicioTransfPotencia.GenerarReporteEgresos(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add("EGRESO POR COMPRA DE POTENCIA", hoja);

                    //    this.servicioTransfPotencia.GenerarReporteIngresoPotencia(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add("INGRESOS POR POTENCIA POR GENERADOR", hoja);

                    //    this.servicioTransfPotencia.GenerarReporteValorTransfPotencia(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add("SALDOS VTP", hoja);

                    //    this.servicioTransfPotencia.GenerarReporteMatriz(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                    //    xlPackage.Workbook.Worksheets.Add("VALORIZACIÓN DE LAS TRANSFERENCIA DE POTENCIA", hoja);

                    //    xlPackage.Save();
                    //}
                }


                return Json(file);
            }
            catch (Exception e)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Abrir el archivo
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : (formato == 3) ? Constantes.AppZip : Constantes.AppWord;

            return File(path, app, sFecha + "_" + file);
        }

        [HttpPost]
        public JsonResult ExportarVerificacion(int pericodi = 0, int recpotcodi = 0, string reporte = "", int formato = 1)
        {
            base.ValidarSesionUsuario();
            ExcelWorksheet hoja = null;
            string PathLogo = @"Content\Images\logocoes.png";
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
            string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
            string file = "-1";
            try
            {
                //verificacion
                file = this.servicioTransfPotencia.VerificarValorizacion(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);

                return Json(file);
            }
            catch (Exception ex)
            {

                return Json(-1);
            }
        }


        //ASSETEC 202108 TIEE

        /// <summary>
        /// Permite migrar los saldos de la empresa origen => destino de un periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <param name="migracodi">Código de la Migración de TIEE</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MigrarSaldo(int pericodi = 0, int recpotcodi = 0, int migracodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            if (pericodi == 0 || recpotcodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de valorización y una versión de recalculo";
                return Json(sResultado);
            }
            try
            {
                //Información de la migración
                SiMigracionDTO dtoSiMigracion = this.servicioTitularidad.GetByIdSiMigracion(migracodi);
                //Procedemos a migrar los saldos
                TrnMigracionDTO dtoTrnMigracion = new TrnMigracionDTO();
                dtoTrnMigracion.Migracodi = migracodi;
                dtoTrnMigracion.Trnmigdescripcion = dtoSiMigracion.Migradescripcion;
                dtoTrnMigracion.Emprcodiorigen = dtoSiMigracion.Emprcodiorigen;
                dtoTrnMigracion.Emprcodidestino = dtoSiMigracion.Emprcodi;
                dtoTrnMigracion.Trnmigusucreacion = User.Identity.Name;
                string sMensaje = "";
                string sDetalle = "";
                string sSql = this.servicioTransfPotencia.MigrarSaldosVTP(dtoSiMigracion.Emprcodiorigen, dtoSiMigracion.Emprcodi, pericodi, recpotcodi, out sMensaje, out sDetalle);
                if (!sMensaje.Equals(""))
                {
                    dtoTrnMigracion.Trnmigsql = sSql + " " + sMensaje + " " + sDetalle;
                    dtoTrnMigracion.Trnmigestado = "X"; //X:Error
                    this.servicioTransferencia.SaveTrnMigracion(dtoTrnMigracion);
                    sResultado = sMensaje;
                }
                else
                {
                    //Sin errores
                    dtoTrnMigracion.Trnmigsql = sSql;
                    dtoTrnMigracion.Trnmigestado = "P"; //Estado del procedimiento: P: VTP
                    this.servicioTransferencia.SaveTrnMigracion(dtoTrnMigracion);
                }
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }
            return Json(sResultado);
        }

        /// <summary>
        /// Permite migrar la información de VTP de la empresa origen => destino de un periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <param name="migracodi">Código de la Migración de TIEE</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MigrarVTP(int pericodi = 0, int recpotcodi = 0, int migracodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            if (pericodi == 0 || recpotcodi == 0 || migracodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de valorización, una versión de recalculo y un proceso de migracion";
                return Json(sResultado);
            }
            try
            {
                //Información de la migración
                SiMigracionDTO dtoSiMigracion = this.servicioTitularidad.GetByIdSiMigracion(migracodi);
                //Procedemos a migrar los saldos
                TrnMigracionDTO dtoTrnMigracion = new TrnMigracionDTO();
                dtoTrnMigracion.Migracodi = migracodi;
                dtoTrnMigracion.Trnmigdescripcion = dtoSiMigracion.Migradescripcion;
                dtoTrnMigracion.Emprcodiorigen = dtoSiMigracion.Emprcodiorigen;
                dtoTrnMigracion.Emprcodidestino = dtoSiMigracion.Emprcodi;
                dtoTrnMigracion.Trnmigusucreacion = User.Identity.Name;
                string sMensaje = "";
                string sDetalle = "";
                string sSql = this.servicioTransfPotencia.MigrarCalculoVTP(dtoSiMigracion.Emprcodiorigen, dtoSiMigracion.Emprcodi, pericodi, recpotcodi, out sMensaje, out sDetalle);
                if (!sMensaje.Equals(""))
                {
                    dtoTrnMigracion.Trnmigsql = sSql + " " + sMensaje + " " + sDetalle;
                    dtoTrnMigracion.Trnmigestado = "X"; //X:Error
                    this.servicioTransferencia.SaveTrnMigracion(dtoTrnMigracion);
                    sResultado = sMensaje;
                }
                else
                {
                    //Sin errores
                    dtoTrnMigracion.Trnmigsql = sSql;
                    dtoTrnMigracion.Trnmigestado = "P"; //Estado del procedimiento: P: VTP
                    this.servicioTransferencia.SaveTrnMigracion(dtoTrnMigracion);
                }
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }
            return Json(sResultado);
        }
    }
}
