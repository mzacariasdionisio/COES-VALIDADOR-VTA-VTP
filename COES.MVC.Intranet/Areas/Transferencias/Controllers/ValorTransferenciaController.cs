using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Titularidad;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using System.Globalization;
using System.Collections;
using COES.Dominio.DTO.Enum;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class ValorTransferenciaController : Controller
    {
        // GET: /Transferencias/valortransferencia/
        //[CustomAuthorize]
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();
        TitularidadAppServicio servicioTitularidad = new TitularidadAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();
        public ActionResult Index()
        {
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListaInterValorTrans();

            TipoEmpresaModel modelTipoEmpresa = new TipoEmpresaModel();
            modelTipoEmpresa.ListaTipoEmpresas = (new TipoEmpresaAppServicio()).ListTipoEmpresas();

            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();

            PeriodoModel modelPeriodo1 = new PeriodoModel();
            modelPeriodo1.ListaPeriodos = (new PeriodoAppServicio()).ListPeriodo();

            //ASSETEC 202108 TIEE
            modelPeriodo1.ListaTrnMigracion = this.servicioTransferencia.GetByCriteriaTrnMigracions();
            //-----------------------------------------------------------------------------

            TempData["Tipoemprcodigo"] = new SelectList(modelTipoEmpresa.ListaTipoEmpresas, "Tipoemprcodi", "Tipoemprdesc");
            TempData["PERICODI"] = new SelectList(modelPeriodo1.ListaPeriodos, "PERICODI", "PERINOMBRE");
            TempData["PERIANIOMES"] = new SelectList(modelPeriodo1.ListaPeriodos, "PERIANIOMES", "PERINOMBRE");
            TempData["EMPRCODI"] = modelEmp;
            TempData["BARRCODI"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");
            TempData["MIGRACODI"] = new SelectList(modelPeriodo1.ListaTrnMigracion, "MIGRACODI", "TRNMIGSQL");

            bool bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["INDGRABAR"] = bGrabar;

            return View();
        }

        /// <summary>
        /// Permite cargar versiones deacuerdo al periodo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculo = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            modelRecalculo.bEjecutar = true;
            //Consultamos por el estado del periodo
            PeriodoDTO entidad = new PeriodoDTO();
            entidad = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
            if (entidad.PeriEstado.Equals("Cerrado"))
            { modelRecalculo.bEjecutar = false; }
            return Json(modelRecalculo);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(int? empcodi, int? barrcodi, int? pericodi, int? tipoemprcodi, int? vers, string flagEntrReti)
        {
            if (flagEntrReti.Equals(""))
                flagEntrReti = null;
            ValorTransferenciaModel model = new ValorTransferenciaModel();
            model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).BuscarValorTransferenciaGetByCriteria1(empcodi, barrcodi, pericodi, tipoemprcodi, vers, flagEntrReti);
            TempData["tdListaValorTransferencia"] = model.ListaValorTransferencia;
            return PartialView(model);
        }

        public ActionResult View(int id = 0)
        {
            ValorTransferenciaModel model = new ValorTransferenciaModel();
            //model.Entidad = (new GeneralAppServicioValorTransferencia()).GetByIdValorTransferencia(id);
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar el proceso de calculo de la matriz de pagos
        /// </summary>
        /// <returns>True si la eliminación fue correcta</returns>
        public string EliminarProceso(int pericodi, int vers)
        {
            try
            {
                return new ValorTransferenciaAppServicio().DeleteValorizacion(pericodi, vers, User.Identity.Name);                
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public ActionResult Borrar(int pericodi, int vers)
        {
            string resultado;
            resultado = EliminarProceso(pericodi, vers);
            return Json(resultado);
        }


        [HttpPost]
        public ActionResult ProcesarValorizacion(int pericodi, int vers)
        {
            try
            {
                int version = vers;

                //Eliminando la información que previamente se haya ejecutado
                string resultado;
                resultado = EliminarProceso(pericodi, vers);
                if (!resultado.Equals("1")) return Json(resultado);
                PeriodoDTO dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo2 = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, version);  
                //---------------------------------------------------------------------------------------------
                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.ProcesarValorizacionProcesar;
                objAuditoria.Estdcodi = (int)EVtpEstados.CalcularValorizacion;
                objAuditoria.Audproproceso = "Comienza la valorización del periodo";
                objAuditoria.Audprodescripcion = "Comienza la valorización del periodo " + dtoPeriodo.PeriNombre + " / " + dtoRecalculo2.RecaNombre;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);

                #endregion
                #region ASSETEC - 20181001-- Valorización de la transferencia para las Entregas[15] y  Retiros[15]
                //Calculamos la información de la tabla trn_valor_trans = Valorización de la Transferencia de Entregas por Empresa[15]
                (new ValorTransferenciaAppServicio()).GrabarValorizacionEntrega(pericodi, version, User.Identity.Name);
                //Calculamos la información de la tabla trn_valor_trans = Valorización de la Transferencia de Retiros por Empresa[15]
                (new ValorTransferenciaAppServicio()).GrabarValorizacionRetiro(pericodi, version, User.Identity.Name);
                #endregion
                //---------------------------------------------------------------------------------------------
                //Calculamos los valores de transferencias de energia (Entrega - Retiro) de cada empresa en el perido y recalculo
                decimal ValorTransEmpresaRetiroSC = 0; //Utilizaremos el valor para la distribución de los Factores de Proporción RSC
                ValorTransferenciaModel modelValorTransferenciaModel = new ValorTransferenciaModel();
                //ValorTransferenciaEmpresa = trn_valor_trans_empresa
                ValorTransferenciaEmpresaDTO dtoValorTransferenciaEmpresa = new ValorTransferenciaEmpresaDTO();
                modelValorTransferenciaModel.ListaValorTransferencia = new ValorTransferenciaAppServicio().ListValorTransferenciaEmpresaRE(pericodi, version);
                foreach (var dtoValorTransferencia in modelValorTransferenciaModel.ListaValorTransferencia)
                {
                    dtoValorTransferenciaEmpresa.EmpCodi = (int)dtoValorTransferencia.EmpCodi;
                    //La diferencia de la Suma de la Valorización de Transferencia de todas las Entregas menos todos los Retiros (solicitados y sin contrato)
                    dtoValorTransferenciaEmpresa.ValTranEmpTotal = dtoValorTransferencia.VTEmpresaEntrega - dtoValorTransferencia.VTEmpresaRetiro;
                    dtoValorTransferenciaEmpresa.PeriCodi = (int)dtoValorTransferencia.PeriCodi;
                    dtoValorTransferenciaEmpresa.ValTranEmpVersion = (int)dtoValorTransferencia.ValoTranVersion;
                    dtoValorTransferenciaEmpresa.ValtranUserName = User.Identity.Name;
                    int graboOk = 0;
                    //trn_valor_trans_empresa
                    graboOk = new ValorTransferenciaAppServicio().SaveValorTransferenciaEmpresa(dtoValorTransferenciaEmpresa);
                    if (dtoValorTransferenciaEmpresa.EmpCodi == Funcion.CODIEMPR_SINCONTRATO)
                        ValorTransEmpresaRetiroSC = dtoValorTransferenciaEmpresa.ValTranEmpTotal;
                }

                //Calculamos los Totales en el periodo recalculo de las Entregas(E), Retiros solicitados (R) y Retiros sin contrato (X) 
                decimal SumEntrega = 0;
                decimal SumRetiroSO = 0;
                decimal SumRetiroSC = 0;

                modelValorTransferenciaModel.ListaValorTransferencia = new ValorTransferenciaAppServicio().ObtenerTotalEnergiaporEntregaoRetiro(pericodi, version);
                foreach (var dtoValorTransferencia in modelValorTransferenciaModel.ListaValorTransferencia)
                {
                    if (dtoValorTransferencia.ValoTranFlag.Equals("E"))
                    {
                        SumEntrega = dtoValorTransferencia.VTTotalDia;
                    }
                    if (dtoValorTransferencia.ValoTranFlag.Equals("R"))
                    {
                        SumRetiroSO = dtoValorTransferencia.VTTotalDia;
                    }
                    if (dtoValorTransferencia.ValoTranFlag.Equals("X"))
                    {
                        SumRetiroSC = dtoValorTransferencia.VTTotalDia;
                    }
                }

                decimal SaldoTotal = (SumRetiroSO + SumRetiroSC) - SumEntrega; //Toda la Valorizacion de los retiros menos las entregas

                //MCHAVEZ 20171017: Agregamos al Saldo el total de la compensación Rentas por Congestión
                decimal dRentasXCongestion = 0;
                //ASSETEC 20181224: Entra en vigencia el módulo de rentas de cogestión
                
                if (dtoPeriodo.PeriAnioMes >= 201812)
                {
                    //Desde Diciembre 2018, se extrae del modulo de rentas de congestión
                    List<TransferenciaRentaCongestionDTO> ListRentaCongestion = (new TransferenciaInformacionBaseAppServicio()).ListRentaCongestion(pericodi, version);
                    if (ListRentaCongestion.Count == 0)
                    {
                        //return Json("No existe Rentas de Congestión para este periodo");
                    }
                    dRentasXCongestion = (new TransferenciaInformacionBaseAppServicio()).GetTotalRentaCongestion(pericodi, version);
                }
                else
                {   //Para antes de Diciembre del 2018 se extrae el valor del modulo de comepnsaciones
                    IngresoCompensacionDTO dtoIngresoCompensacion = new IngresoCompensacionAppServicio().GetByPeriVersRentasCongestion(pericodi, version);
                    if (dtoIngresoCompensacion != null)
                    {
                        dRentasXCongestion = dtoIngresoCompensacion.IngrCompImporte;
                    }
                }
                SaldoTotal = SaldoTotal - dRentasXCongestion;

                //Calcula de los Ingresos por potencia de las empresas -> trn_saldo_empresa
                IngresoPotenciaModel modelIP = new IngresoPotenciaModel();
                modelIP.ListaIngresoPotencia = new IngresoPotenciaAppServicio().ListImportesByPeriVer(pericodi, version);
                foreach (var dtoIngresoPotencia in modelIP.ListaIngresoPotencia)
                {
                    SaldoEmpresaDTO dtoSaldoEmpresa = new SaldoEmpresaDTO();
                    dtoSaldoEmpresa.EmpCodi = dtoIngresoPotencia.EmprCodi;
                    dtoSaldoEmpresa.SalEmpVersion = dtoIngresoPotencia.IngrPoteVersion;
                    dtoSaldoEmpresa.SalEmpSaldo = (SaldoTotal * (Decimal)dtoIngresoPotencia.IngrPoteImporte) / dtoIngresoPotencia.Total;
                    dtoSaldoEmpresa.PeriCodi = dtoIngresoPotencia.PeriCodi;
                    dtoSaldoEmpresa.SalEmpUserName = User.Identity.Name;
                    int saveok = 0;
                    saveok = new ValorTransferenciaAppServicio().SaveOrUpdateSaldoTransmisionEmpresa(dtoSaldoEmpresa);
                }

                //Calcula de los Ingresos por los retiros sin contrato de las empresas -> trn_saldo_coresc
                IngresoRetiroSCModel modelIPSC = new IngresoRetiroSCModel();
                modelIPSC.ListaIngresoRetiroSC = new IngresoRetiroSCAppServicio().ListImportesByPeriVer(pericodi, version);
                foreach (var dtoIngresoRetiroSC in modelIPSC.ListaIngresoRetiroSC)
                {
                    SaldoCodigoRetiroscDTO dtoCodigoRetiroSC = new SaldoCodigoRetiroscDTO();
                    dtoCodigoRetiroSC.EmprCodi = dtoIngresoRetiroSC.EmprCodi;
                    dtoCodigoRetiroSC.SalrscVersion = dtoIngresoRetiroSC.IngrscVersion;
                    dtoCodigoRetiroSC.SalrscsSaldo = (ValorTransEmpresaRetiroSC * (Decimal)dtoIngresoRetiroSC.IngrscImporte) / dtoIngresoRetiroSC.Total;
                    dtoCodigoRetiroSC.PeriCodi = dtoIngresoRetiroSC.PeriCodi;
                    dtoCodigoRetiroSC.SalrscUserName = User.Identity.Name;
                    int saveok = 0;
                    saveok = new ValorTransferenciaAppServicio().SaveOrUpdateSaldoCodigoRetiroSC(dtoCodigoRetiroSC);
                }

                //Calcula del Valor Total de la Empresa -> trn_valor_total_empresa
                ValorTotalEmpresaDTO dtoValorTotalEmpresa = new ValorTotalEmpresaDTO();
                modelValorTransferenciaModel.ListaValorTransferencia = new ValorTransferenciaAppServicio().ObtenerTotalValorEmpresa(pericodi, version);
                foreach (var dtoValorTransferencia in modelValorTransferenciaModel.ListaValorTransferencia)
                {
                    dtoValorTotalEmpresa.EmpCodi = (Int32)dtoValorTransferencia.EmpCodi;
                    dtoValorTotalEmpresa.PeriCodi = (Int32)dtoValorTransferencia.PeriCodi;
                    dtoValorTotalEmpresa.ValTotaEmpVersion = dtoValorTransferencia.ValoTranVersion;
                    if (dtoValorTotalEmpresa.EmpCodi != Funcion.CODIEMPR_SINCONTRATO)
                    {
                        dtoValorTotalEmpresa.ValTotaEmpTotal = dtoValorTransferencia.Valorizacion; //+ Valor de Transferencia de la Empresa
                        dtoValorTotalEmpresa.ValTotaEmpTotal += dtoValorTransferencia.SalEmpSaldo; //+ Ingreso por Potencia de la Empresa
                        dtoValorTotalEmpresa.ValTotaEmpTotal += dtoValorTransferencia.SalrscSaldo; //+ Ingreso por Retiros sin contrato de la Empresa
                        dtoValorTotalEmpresa.ValTotaEmpTotal += dtoValorTransferencia.Compensacion; //+ Ingreso por Compensación de la Empresa
                                                                                                    //20190409 - ASSETEC: El saldo se debe considerar en todos los recalculos, no solo en la revisión mensual
                                                                                                    //if (vers == 1)
                                                                                                    //{   //Para la valorización mensual.
                        dtoValorTotalEmpresa.ValTotaEmpTotal += dtoValorTransferencia.Salrecalculo; //+ Ingreso por Recalculos de meses anteriores 
                        //}
                        //Falta los saldos anteriores
                        dtoValorTotalEmpresa.ValTotaEmpUserName = User.Identity.Name;
                        int saveok = 0;
                        saveok = new ValorTransferenciaAppServicio().SaveOrUpdateValorTotalEmpresa(dtoValorTotalEmpresa);
                    }
                }

                //Si la version == 1 procesamos la matriz de pagos
                if (version == 1)
                {
                    //Calculando y asignando las pagos entre empresas en la Matriz de Pagos -> trn_empresa_pago
                    //Traemos la lista de empresa con montos positivos
                    ValorTotalEmpresaModel modelValorTotalEmpresaPositivo = new ValorTotalEmpresaModel();
                    modelValorTotalEmpresaPositivo.ListaValorTotalEmpresa = (new ValorTransferenciaAppServicio()).BuscarEmpresasValorPositivo(pericodi, version);
                    //Traemos la lista de empresa con su importe negativo y el importe total negativo de todas las empresas
                    ValorTotalEmpresaModel modelValorTotalEmpresaNegativo = new ValorTotalEmpresaModel();
                    modelValorTotalEmpresaNegativo.ListaValorTotalEmpresa = (new ValorTransferenciaAppServicio()).BuscarEmpresasValorNegativo(pericodi, version);

                    if (modelValorTotalEmpresaPositivo.ListaValorTotalEmpresa.Count > 0 && modelValorTotalEmpresaNegativo.ListaValorTotalEmpresa.Count > 0)
                    {
                        //Debe existir al menos una empresa positiva y una negativa
                        EmpresaPagoDTO dtoEmpresaPago = new EmpresaPagoDTO();
                        foreach (var dtoValorTotalEmpresaPositivo in modelValorTotalEmpresaPositivo.ListaValorTotalEmpresa)
                        {
                            //Para cada empresa P con un Valor positivo
                            foreach (var dtoValorTotalEmpresaNegativo in modelValorTotalEmpresaNegativo.ListaValorTotalEmpresa)
                            {
                                //Distribuimos el Importe P entre las empresa negativas N
                                dtoEmpresaPago.ValTotaEmpCodi = dtoValorTotalEmpresaPositivo.ValTotaEmpCodi; //Codigo de la tabla trn_valor_total_empresa P
                                dtoEmpresaPago.EmpCodi = dtoValorTotalEmpresaNegativo.EmpCodi; //Codigo de la tabla empresa N
                                dtoEmpresaPago.PeriCodi = dtoValorTotalEmpresaPositivo.PeriCodi; //Mes de valorización de recalculo
                                dtoEmpresaPago.ValTotaEmpVersion = dtoValorTotalEmpresaPositivo.ValTotaEmpVersion; //Versión de recalculo
                                dtoEmpresaPago.EmpPagoCodEmpPago = dtoValorTotalEmpresaPositivo.EmpCodi; //Codigo de la tabla empresa P
                                dtoEmpresaPago.EmpPagoMonto = (dtoValorTotalEmpresaNegativo.ValTotaEmpTotal / dtoValorTotalEmpresaNegativo.Total) * dtoValorTotalEmpresaPositivo.ValTotaEmpTotal; //Importe a pagar
                                dtoEmpresaPago.EmpPagpUserName = User.Identity.Name;
                                int graboPagook = 0;
                                graboPagook = (new ValorTransferenciaAppServicio()).SaveoUpdateEmpresaPago(dtoEmpresaPago);
                            }
                        }
                    }
                }
                else
                {   //Calcular los saldos entre versiones -> TRN_SALDO_RECALCULO
                    int iVerActual = version;
                    int iVerAnterior = version - 1;
                    RecalculoDTO dtoRecalculo = new RecalculoDTO();
                    dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, iVerActual);
                    //Extraemos la información de trn_valor_total_empresa
                    ValorTotalEmpresaModel modelVTEActual = new ValorTotalEmpresaModel();
                    modelVTEActual.ListaValorTotalEmpresa = (new ValorTransferenciaAppServicio()).ListarValorTotalEmpresa(pericodi, iVerActual);
                    foreach (var dtoValorTotalEmpresaActual in modelVTEActual.ListaValorTotalEmpresa)
                    {
                        decimal dSaldo = 0;
                        if (dtoValorTotalEmpresaActual.EmpCodi != Funcion.CODIEMPR_SINCONTRATO)
                        {
                            ValorTotalEmpresaDTO dtoValorTotalEmpresaAnterior = new ValorTotalEmpresaDTO();
                            dtoValorTotalEmpresaAnterior = (new ValorTransferenciaAppServicio()).GetByCriteria(pericodi, iVerAnterior, dtoValorTotalEmpresaActual.EmpCodi);
                            if (dtoValorTotalEmpresaAnterior != null)
                                dSaldo = dtoValorTotalEmpresaActual.ValTotaEmpTotal - dtoValorTotalEmpresaAnterior.ValTotaEmpTotal;
                            SaldoRecalculoDTO dtoSR = new SaldoRecalculoDTO();
                            dtoSR.EmpCodi = dtoValorTotalEmpresaActual.EmpCodi;
                            dtoSR.PeriCodi = pericodi;
                            dtoSR.RecaCodi = iVerActual;
                            dtoSR.SalRecSaldo = dSaldo;
                            dtoSR.PeriCodiDestino = dtoRecalculo.PeriCodiDestino;
                            dtoSR.SalRecUserName = User.Identity.Name;
                            int graboSaldoRecalculo = 0;
                            graboSaldoRecalculo = (new ValorTransferenciaAppServicio()).SaveoUpdateSaldoRecalculo(dtoSR);
                        }
                    }
                }
                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoriaF = new VtpAuditoriaProcesoDTO();
                objAuditoriaF.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.ProcesarValorizacionProcesar;
                objAuditoriaF.Estdcodi = (int)EVtpEstados.CalcularValorizacion;
                objAuditoriaF.Audproproceso = "Finaliza la valorización del periodo";
                objAuditoriaF.Audprodescripcion = "Finaliza la valorización del periodo " + dtoPeriodo.PeriNombre + " / " + dtoRecalculo2.RecaNombre + " - cantidad de errores - 0";
                objAuditoriaF.Audprousucreacion = User.Identity.Name;
                objAuditoriaF.Audprofeccreacion = DateTime.Now;

                int auditoriaF = this.servicioAuditoria.save(objAuditoriaF);

                #endregion
                return Json("1");
            }
            catch (Exception e)
            {
                return Json(e.Message); //return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ExportarEntregasRetirosEnergiaValorizados(int? empcodi, int? barrcodi, int? pericodi, int? tipoemprcodi, int? vers, string flagEntrReti)
        {
            string indicador = "1";

            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                //ASSETEC 20190104 
                int iAnioMes = dtoPeriodo.PeriAnioMes;
                RecalculoDTO dtoRecalculo = new RecalculoDTO();
                dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo((int)pericodi, (int)vers);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                ValorTransferenciaModel model = new ValorTransferenciaModel();
                if (TempData["tdListaValorTransferencia"] != null)
                    model.ListaValorTransferencia = (List<ValorTransferenciaDTO>)TempData["tdListaValorTransferencia"];
                else
                {
                    if (flagEntrReti.Equals(""))
                        flagEntrReti = null;
                    model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).BuscarValorTransferenciaGetByCriteria1(empcodi, barrcodi, pericodi, tipoemprcodi, vers, flagEntrReti);
                }
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteEntregasRetirosEnergiaValorizadosExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteEntregasRetirosEnergiaValorizadosExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //Fila de Inicio de la data
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaCuadro1 + " ";
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaNroInforme;
                        ws.Cells[row++, 3].Value = "ENTREGAS Y RETIROS DE ENERGÍA VALORIZADOS";
                        ws.Cells[row++, 3].Value = dtoPeriodo.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        ws.Cells[row, 3].Value = "BARRA DE TRANSFERENCIA";
                        ws.Cells[row, 4].Value = "TIPO DE USUARIO";
                        ws.Cells[row, 5].Value = "TIPO DE CONTRATO";
                        ws.Cells[row, 6].Value = "RUC CLIENTE";
                        ws.Cells[row, 7].Value = "ENTREGA/RETIRO";
                        ws.Cells[row, 8].Value = "CLIENTE / CENTRAL GENERACIÓN"; //
                        ws.Cells[row, 9].Value = "ENERGÍA (MW.h)";
                        ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 10].Value = "VALORIZACIÓN S/.";
                        ws.Column(10).Style.Numberformat.Format = "#,##0.00";


                        ws.Cells[row, 11].Value = "RENTA DE CONGESTIÓN DE LICITACIÓN S/.";
                        ws.Column(11).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 12].Value = "RENTA DE CONGESTIÓN DE BILATERAL S/.";
                        ws.Column(12).Style.Numberformat.Format = "#,##0.00";

                        ws.Cells[row, 13].Value = "INFORMACIÓN";


                        rg = ws.Cells[row, 2, row, 13]; //Posición de la cabecera
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        row++; // int row = 7;//Posición de inicio de la tabla
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.EmprNomb.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.BarrNombBarrTran;
                                ws.Cells[row, 4].Value = (item.VtranUserName != null) ? item.VtranUserName.ToString() : string.Empty;
                                ws.Cells[row, 5].Value = (item.CentGeneNombre != null) ? item.CentGeneNombre.ToString() : string.Empty;
                                ws.Cells[row, 6].Value = (item.RucEmpresa != null) ? item.RucEmpresa.ToString() : string.Empty;
                                ws.Cells[row, 7].Value = item.ValoTranCodEntRet.ToString();
                                ws.Cells[row, 8].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString() : string.Empty;
                                ws.Cells[row, 9].Value = item.VTTotalEnergia; //ENERGÍA (MW.h)
                                ws.Cells[row, 10].Value = item.VTTotalDia; //VALORIZACIÓN S/.                               

                                ws.Cells[row, 11].Value = item.RcLicitacion;
                                ws.Cells[row, 12].Value = item.RcBilateral;

                                ws.Cells[row, 13].Value = (item.VTTipoInformacion != null) ? item.VTTipoInformacion.ToString() : string.Empty;
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 13];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                        }
                        //Unimos con el reporte de retiros sin contrato
                        bool bSalir = false;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.EmprNomb.ToString().Trim() == Funcion.NOMBEMPR_NOCUBIERTO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.BarrNombBarrTran;
                                ws.Cells[row, 4].Value = (item.VtranUserName != null) ? item.VtranUserName.ToString() : string.Empty;
                                ws.Cells[row, 5].Value = (item.CentGeneNombre != null) ? item.CentGeneNombre.ToString() : string.Empty;
                                ws.Cells[row, 7].Value = item.ValoTranCodEntRet.ToString();
                                if (iAnioMes <= 201812)
                                {
                                    ws.Cells[row, 6].Value = "";
                                    ws.Cells[row, 8].Value = Funcion.NOMBEMPR_SINCONTRATO;
                                }
                                else
                                {
                                    ws.Cells[row, 6].Value = (item.RucEmpresa != null) ? item.RucEmpresa.ToString() : string.Empty;
                                    ws.Cells[row, 8].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString() : string.Empty;
                                }
                                ws.Cells[row, 9].Value = item.VTTotalEnergia;
                                ws.Cells[row, 10].Value = item.VTTotalDia;

                                ws.Cells[row, 11].Value = item.RcLicitacion;
                                ws.Cells[row, 12].Value = item.RcBilateral;

                                ws.Cells[row, 13].Value = (item.VTTipoInformacion != null) ? item.VTTipoInformacion.ToString() : string.Empty;
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 13];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                                bSalir = true;
                            }
                            else if (item.EmprNomb.ToString().Trim() != Funcion.NOMBEMPR_NOCUBIERTO && bSalir == true)
                                break;
                        }

                        //Fijar panel
                        //ws.View.FreezePanes(6, 8);
                        rg = ws.Cells[7, 2, row, 13];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 35);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }

            return Json(indicador);
        }



        [HttpGet]
        public virtual ActionResult AbrirExcelEntregasRetirosEnergiaValorizados()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteEntregasRetirosEnergiaValorizadosExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Funcion.AppExcel, sFecha + "_" + Funcion.NombreReporteEntregasRetirosEnergiaValorizadosExcel);
        }

        [HttpPost]
        public JsonResult ExportarEntregasRetirosEnergiaValorizados15min(int pericodi, int vers, int empcodi, int barrcodi, string flagEntrReti)
        {
            string indicador = "1";

            try
            {
                PeriodoModel modelPeriodo = new PeriodoModel();
                modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);

                EmpresaModel modelEmpresa = new EmpresaModel();
                modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(empcodi);
                string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                Session["sPrefijoExcel"] = sPrefijoExcel;
                if (flagEntrReti == "T")
                    flagEntrReti = null;

                ValorTransferenciaModel model = new ValorTransferenciaModel();
                model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).ListarValorTransferencia(pericodi, vers, empcodi, barrcodi, flagEntrReti);
                TransferenciaEntregaDetalleModel teddmodel = new TransferenciaEntregaDetalleModel();
                teddmodel.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListTransEntrReti(empcodi, barrcodi, pericodi, vers, flagEntrReti);

                //Obtener cuandos dias tiene el mes del periodo
                var LastDay = DateTime.DaysInMonth(modelPeriodo.Entidad.AnioCodi, modelPeriodo.Entidad.MesCodi);
                //Obtener cantidad de columnas
                var totalcolumns = model.ListaValorTransferencia.Count() / LastDay;

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteEntregasRetirosEnergiaValorizadosExcel15min);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteEntregasRetirosEnergiaValorizadosExcel15min);
                }

                int row = 4;
                int row2 = 4;
                int column = 2;
                int column2 = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("ENERGIA");
                    if (ws != null)
                    {
                        //PRIMERA HOJA
                        ws.Cells[1, 1].Value = "MW.h";
                        ws.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        ws.Cells[3, 1].Value = "TIPO";

                        ExcelRange rg = ws.Cells[2, 3, 2, 3];

                        //GRABA DETALLE PRIMERA HOJA
                        ArrayList Listporcodigoentre = new ArrayList(LastDay);
                        for (int c = 0; c < teddmodel.ListaTransferenciaEntregaDetalle.Count(); c += LastDay)
                        {
                            var arrylistdCodigoEntrRet = new ArrayList();
                            arrylistdCodigoEntrRet.AddRange(teddmodel.ListaTransferenciaEntregaDetalle.GetRange(c, LastDay));
                            Listporcodigoentre.Add(arrylistdCodigoEntrRet);
                        }

                        for (int codi = 0; codi < Listporcodigoentre.Count; codi++)
                        {
                            ArrayList arrylist = (ArrayList)Listporcodigoentre[codi];
                            int fila = 4;


                            for (int dia = 0; dia < arrylist.Count; dia++)
                            {
                                TransferenciaEntregaDetalleDTO TransEntregaDTO = (TransferenciaEntregaDetalleDTO)arrylist[dia];
                                ws.Cells[1, column].Value = TransEntregaDTO.CodiEntrCodigo;
                                ws.Cells[2, column].Value = TransEntregaDTO.TranEntrTipoInformacion;
                                if (TransEntregaDTO.FlagTipo == "E")
                                {
                                    ws.Cells[3, column].Value = "Entrega";
                                }
                                else if (TransEntregaDTO.FlagTipo == "R")
                                {
                                    ws.Cells[3, column].Value = "Retiro";
                                }
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah1;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah2;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah3;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah4;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah5;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah6;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah7;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah8;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah9;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah10;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah11;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah12;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah13;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah14;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah15;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah16;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah17;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah18;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah19;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah20;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah21;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah22;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah23;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah24;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah25;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah26;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah27;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah28;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah29;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah30;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah31;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah32;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah33;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah34;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah35;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah36;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah37;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah38;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah39;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah40;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah41;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah42;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah43;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah44;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah45;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah46;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah47;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah48;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah49;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah50;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah51;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah52;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah53;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah54;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah55;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah56;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah57;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah58;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah59;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah60;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah61;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah62;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah63;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah64;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah65;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah66;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah67;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah68;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah69;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah70;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah71;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah72;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah73;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah74;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah75;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah76;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah77;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah78;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah79;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah80;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah81;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah82;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah83;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah84;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah85;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah86;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah87;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah88;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah89;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah90;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah91;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah92;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah93;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah94;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah95;
                                ws.Cells[fila++, column].Value = TransEntregaDTO.TranEntrDetah96;

                            }
                            column++;
                            ws.Column(column).Style.Numberformat.Format = "#,##0.000000";

                        }

                        //SEGUNDA HOJA
                        ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("ENERGIA VALORIZADA");

                        ws2.Cells[1, 1].Value = "S/.";
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        ws2.Cells[3, 1].Value = "TIPO";
                        int ColumnaCabezera = 0;

                        ////Graba detalle

                        ArrayList Listportranscodigoentret = new ArrayList(LastDay);
                        for (int c = 0; c < model.ListaValorTransferencia.Count(); c += LastDay)
                        {
                            var arrylistdCodigoEntrRet = new ArrayList();
                            arrylistdCodigoEntrRet.AddRange(model.ListaValorTransferencia.GetRange(c, LastDay));
                            Listportranscodigoentret.Add(arrylistdCodigoEntrRet);
                        }



                        for (int codi = 0; codi < Listportranscodigoentret.Count; codi++)
                        {
                            ArrayList arrylist = (ArrayList)Listportranscodigoentret[codi];
                            int fila = 4;


                            for (int dia = 0; dia < arrylist.Count; dia++)
                            {
                                ValorTransferenciaDTO ValtransDTO = (ValorTransferenciaDTO)arrylist[dia];
                                ws2.Cells[1, column2].Value = ValtransDTO.ValoTranCodEntRet;
                                ws2.Cells[2, column2].Value = ValtransDTO.VTTipoInformacion;
                                if (ValtransDTO.ValoTranFlag == "E")
                                {
                                    ws2.Cells[3, column2].Value = "Entrega";
                                }
                                else if (ValtransDTO.ValoTranFlag == "R")
                                {
                                    ws2.Cells[3, column2].Value = "Retiro";
                                }
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT1;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT2;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT3;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT4;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT5;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT6;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT7;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT8;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT9;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT10;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT11;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT12;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT13;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT14;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT15;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT16;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT17;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT18;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT19;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT20;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT21;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT22;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT23;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT24;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT25;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT26;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT27;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT28;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT29;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT30;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT31;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT32;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT33;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT34;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT35;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT36;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT37;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT38;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT39;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT40;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT41;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT42;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT43;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT44;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT45;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT46;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT47;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT48;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT49;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT50;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT51;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT52;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT53;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT54;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT55;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT56;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT57;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT58;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT59;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT60;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT61;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT62;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT63;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT64;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT65;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT66;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT67;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT68;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT69;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT70;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT71;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT72;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT73;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT74;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT75;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT76;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT77;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT78;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT79;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT80;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT81;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT82;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT83;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT84;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT85;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT86;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT87;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT88;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT89;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT90;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT91;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT92;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT93;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT94;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT95;
                                ws2.Cells[fila++, column2].Value = ValtransDTO.VT96;

                            }
                            column2++;
                            ws2.Column(column2).Style.Numberformat.Format = "#,##0.000000";

                        }

                        //foreach (var item in model.ListaEntregReti)
                        //{
                        //    ws2.Cells[1, colum].Value = (item.CodiEntreRetiCodigo != null) ? item.CodiEntreRetiCodigo.ToString() : string.Empty;
                        //    ws2.Cells[2, colum].Value = "Mejor información";
                        //    ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                        //    colum++;
                        //}

                        //Color de fondo 1ra hoja
                        //Color de fondo
                        rg = ws.Cells[1, 1, 3, column - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ////////////////////////////
                        string sMes = modelPeriodo.Entidad.MesCodi.ToString();
                        if (sMes.Length == 1) sMes = "0" + sMes;
                        var Fecha = "01/" + sMes + "/" + modelPeriodo.Entidad.AnioCodi;
                        var dates = new List<DateTime>();
                        var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                        var dateFin = dateIni.AddMonths(1);

                        dateIni = dateIni.AddMinutes(15);
                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                        {
                            dates.Add(dt);
                        }

                        foreach (var item in dates)
                        {
                            ws.Cells[row, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row++;
                        }
                        rg = ws.Cells[4, 1, row - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws.Cells[1, 1, row - 1, column - 1];
                        rg.AutoFitColumns();
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //Color de fondo 2da hoja
                        rg = ws2.Cells[1, 1, 3, column2 - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ////////////////////////////
                        string sMes2 = modelPeriodo.Entidad.MesCodi.ToString();
                        if (sMes2.Length == 1) sMes2 = "0" + sMes2;
                        var Fecha2 = "01/" + sMes + "/" + modelPeriodo.Entidad.AnioCodi;
                        var dates2 = new List<DateTime>();
                        var dateIni2 = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                        var dateFin2 = dateIni2.AddMonths(1);

                        dateIni2 = dateIni2.AddMinutes(15);
                        for (var dt = dateIni2; dt <= dateFin2; dt = dt.AddMinutes(15))
                        {
                            dates2.Add(dt);
                        }

                        foreach (var item in dates2)
                        {
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row2++;
                        }
                        rg = ws2.Cells[4, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, column2 - 1];
                        rg.AutoFitColumns();
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcelEntregasRetirosEnergiaValorizados15min()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteEntregasRetirosEnergiaValorizadosExcel15min;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Funcion.AppExcel, sFecha + "_" + Funcion.NombreReporteEntregasRetirosEnergiaValorizadosExcel15min);
        }


        [HttpPost]
        public ActionResult ListaInfo(int pericodi, int vers)
        {

            //LISTA LAS COLUMNAS DE DETERMINACIÓN
            int version = vers;
            //Extraemos la información de trn_valor_total_empresa
            ValorTransferenciaModel model = new ValorTransferenciaModel();
            model.ListaValorTransferencia = new ValorTransferenciaAppServicio().ObtenerTotalValorEmpresa1(pericodi, version);
            model.IdValorTransferencia = version;
            TempData["ListaValorTransferencia"] = model.ListaValorTransferencia;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ExportarDeterminacionSaldosTransferencias(int pericodi, int vers)
        {
            string indicador = "1";
            bool bMostrarSaldoAnterior = false;

            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo = new RecalculoDTO();
                dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, vers);
                CompensacionModel modelCompensacion = new CompensacionModel();
                //modelCompensacion.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones(pericodi);
                modelCompensacion.ListaCompensacion = (new CompensacionAppServicio()).ListCompensacionesReporte(pericodi);

                if (vers > 1) bMostrarSaldoAnterior = true;

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                ValorTransferenciaModel model = new ValorTransferenciaModel();
                if (TempData["ListaValorTransferencia"] != null)
                    model.ListaValorTransferencia = (List<ValorTransferenciaDTO>)TempData["ListaValorTransferencia"];
                else
                {
                    model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).ObtenerTotalValorEmpresa1(pericodi, vers);
                }
                FileInfo newFile = new FileInfo(path + Funcion.NombreDeterminacionSaldosTransferenciasExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreDeterminacionSaldosTransferenciasExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //Fila de Inicio de la data
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaCuadro2 + " ";
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaNroInforme + " ";
                        ws.Cells[row++, 3].Value = "DETERMINACIÓN DE SALDOS DE TRANSFERENCIAS";
                        ws.Cells[row++, 3].Value = dtoPeriodo.PeriNombre;
                        ws.Cells[row++, 3].Value = Funcion.MensajeSoles;
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        //Fila 1
                        ws.Cells[row, 2].Value = "EMPRESA";
                        ws.Cells["B" + row.ToString() + ":B" + (row + 1).ToString() + ""].Merge = true; //(7,2,8,2)
                        rg = ws.Cells[row, 2, (row + 1), 2];
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //----------------------------------------------------------
                        ws.Cells[row, 3].Value = "SALDO DE TRANSFERENCIAS";
                        ws.Cells["C" + row.ToString() + ":E" + row.ToString() + ""].Merge = true; //(7,3,7,4)
                        //----------------------------------------------------------
                        ws.Cells[row, 6].Value = "ASIGNACIÓN DE SALDO RESULTANTE";
                        ws.Cells["F" + row.ToString() + ":F" + (row + 1).ToString() + ""].Merge = true; //(7,5,8,5)
                        rg = ws.Cells[row, 6, (row + 1), 6];
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; //Alineado Vertical
                        rg.Style.WrapText = true; //Ajuste de texto
                        //----------------------------------------------------------
                        ws.Cells[row, 7].Value = "SALDOS POR COMPENSACIONES";
                        int iColumnaInicio = 7;
                        int iColumnaFinal = iColumnaInicio + modelCompensacion.ListaCompensacion.Count() - 1; //Numero de cabeceras de compensación en el periodo
                        ws.Cells[row, iColumnaInicio, row, iColumnaFinal].Merge = true;
                        //----------------------------------------------------------
                        int iColumnaSaldoRecalculo = iColumnaFinal + 1;
                        ws.Cells[row, iColumnaSaldoRecalculo].Value = "SALDO DE MESES ANTERIORES";
                        ws.Cells[row, iColumnaSaldoRecalculo, (row + 1), iColumnaSaldoRecalculo].Merge = true;
                        rg = ws.Cells[row, iColumnaSaldoRecalculo, (row + 1), iColumnaSaldoRecalculo];
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; //Alineado Vertical
                        rg.Style.WrapText = true; //Ajuste de texto
                        ws.Column(iColumnaSaldoRecalculo).Style.Numberformat.Format = "#,##0.00";
                        //----------------------------------------------------------
                        int iColumnaNeto = iColumnaFinal + 2;
                        ws.Cells[row, iColumnaNeto].Value = "SALDO ACTUAL DE TRANSFERENCIA";
                        ws.Cells[row, iColumnaNeto, (row + 1), iColumnaNeto].Merge = true;
                        rg = ws.Cells[row, iColumnaNeto, (row + 1), iColumnaNeto];
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; //Alineado Vertical
                        rg.Style.WrapText = true; //Ajuste de texto
                        ws.Column(iColumnaNeto).Style.Numberformat.Format = "#,##0.00";
                        //----------------------------------------------------------
                        int iColumnaAnterior = 0;
                        int iColumnaSaldo = 0;
                        if (bMostrarSaldoAnterior)
                        {
                            iColumnaAnterior = iColumnaFinal + 3;
                            ws.Cells[row, iColumnaAnterior].Value = "SALDO ANTERIOR";
                            ws.Cells[row, iColumnaAnterior, (row + 1), iColumnaAnterior].Merge = true;
                            rg = ws.Cells[row, iColumnaAnterior, (row + 1), iColumnaAnterior];
                            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; //Alineado Vertical
                            rg.Style.WrapText = true; //Ajuste de texto
                            ws.Column(iColumnaAnterior).Style.Numberformat.Format = "#,##0.00";
                            //----------------------------------------------------------
                            iColumnaSaldo = iColumnaFinal + 4;
                            ws.Cells[row, iColumnaSaldo].Value = "SALDO";
                            ws.Cells[row, iColumnaSaldo, (row + 1), iColumnaSaldo].Merge = true;
                            rg = ws.Cells[row, iColumnaSaldo, (row + 1), iColumnaSaldo];
                            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; //Alineado Vertical
                            rg.Style.WrapText = true; //Ajuste de texto
                            ws.Column(iColumnaSaldo).Style.Numberformat.Format = "#,##0.00";
                        }
                        int iUltimo = iColumnaNeto;
                        if (iColumnaSaldo > iColumnaNeto) iUltimo = iColumnaSaldo;
                        //----------------------------------------------------------
                        //Fila 2
                        ws.Cells[(row + 1), 3].Value = "VALORIZACIÓN DE ENTREGAS Y RETIROS";
                        ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[(row + 1), 4].Value = "RETIRO NO DECLARADO"; //ASSETEC 20200421
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[(row + 1), 5].Value = "RENTAS POR CONGESTIÓN";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                        //----------------------------------------------------------
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                        //----------------------------------------------------------
                        int iAux = iColumnaInicio;
                        decimal[] dTotalCompensacion = new decimal[50]; // Donde se almacenan los totales por compensación
                        foreach (var item in modelCompensacion.ListaCompensacion)
                        {
                            ws.Cells[(row + 1), iAux].Value = item.CabeCompNombre; //Nombre de la compensación
                            ws.Column(iAux).Style.Numberformat.Format = "#,##0.00";
                            dTotalCompensacion[iAux] = 0;
                            iAux++;
                        }
                        //----------------------------------------------------------------------------------------------------------------------
                        //PARA TODA LA ZONA DE TRABAJO
                        rg = ws.Cells[row, 2, (row + 1), iUltimo];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        //----------------------------------------------------------------------------------------------------------------------
                        //Totales
                        decimal dValorizacion = 0;
                        decimal dSaldoSinContrato = 0;
                        decimal dSaldoRentaCongestion = 0;
                        decimal dSaldoResultante = 0;
                        decimal dCompensacion = 0;
                        decimal dSalrecalculo = 0;
                        decimal dValorTotalEmp = 0;
                        decimal dSaldoAnterior = 0;
                        decimal dSaldo = 0;
                        row = row + 2;// int row = 9;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if ((int)item.EmpCodi != Funcion.CODIEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.Valorizacion;
                                ws.Cells[row, 4].Value = item.SalrscSaldo;
                                ws.Cells[row, 5].Value = item.Entregas;//Rentas por Congestión
                                ws.Cells[row, 6].Value = item.SalEmpSaldo;
                                //Lista de compensaciones
                                iAux = iColumnaInicio;
                                foreach (var mCompensacion in modelCompensacion.ListaCompensacion)
                                {
                                    IngresoCompensacionDTO dtoIngresoCompensacion = new IngresoCompensacionDTO();
                                    //importe de la compensación por empresa en el mes y la versión
                                    dtoIngresoCompensacion = (new IngresoCompensacionAppServicio()).GetByPeriVersCabCompEmpr(pericodi, mCompensacion.CabeCompCodi, vers, (int)item.EmpCodi);

                                    //- Linea agregada
                                    if (item.EmpCodi == 11153)
                                    {
                                        IngresoCompensacionDTO adicional = (new IngresoCompensacionAppServicio()).GetByPeriVersCabCompEmpr(pericodi, mCompensacion.CabeCompCodi, vers, 10582);

                                        if (adicional != null)
                                        {
                                            dtoIngresoCompensacion.IngrCompImporte += adicional.IngrCompImporte;
                                        }
                                    }
                                    // Linea agregada


                                    if (dtoIngresoCompensacion != null)
                                    {
                                        ws.Cells[row, iAux].Value = dtoIngresoCompensacion.IngrCompImporte;
                                        dTotalCompensacion[iAux] += dtoIngresoCompensacion.IngrCompImporte;
                                    }
                                    else
                                    { ws.Cells[row, iAux].Value = 0; }
                                    iAux++;
                                }
                                ws.Cells[row, iColumnaSaldoRecalculo].Value = item.Salrecalculo;
                                ws.Cells[row, iColumnaNeto].Value = item.Vtotempresa;
                                dValorizacion += item.Valorizacion;
                                dSaldoSinContrato += item.SalrscSaldo;
                                dSaldoRentaCongestion += item.Entregas;
                                dSaldoResultante += item.SalEmpSaldo;
                                dCompensacion += item.Compensacion;
                                dSalrecalculo += item.Salrecalculo;
                                dValorTotalEmp += item.Vtotempresa;
                                if (bMostrarSaldoAnterior)
                                {
                                    ws.Cells[row, iColumnaAnterior].Value = item.Vtotanterior;
                                    ws.Cells[row, iColumnaSaldo].Value = (item.Vtotempresa - item.Vtotanterior);
                                    dSaldoAnterior += item.Vtotanterior;
                                    dSaldo += (item.Vtotempresa - item.Vtotanterior);
                                }
                                //Border por celda
                                rg = ws.Cells[row, 2, row, iUltimo];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                        }
                        //----------------------------------------------------------------------------------------------------------------------
                        //Unimos al reporte la linea retiros sin contrato
                        bool bSalir = false;
                        decimal dValorizacionRSC = 0;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if ((int)item.EmpCodi == Funcion.CODIEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.Valorizacion;
                                dValorizacionRSC = item.Valorizacion;
                                //Si (item.Valorizacion == dSaldoSinContrato) la distribución de los retiros sin contrato fue correcto
                                ws.Cells[row, 4].Value = (dSaldoSinContrato * -1);
                                ws.Cells[row, 5].Value = item.Entregas;//Rentas por Congestión
                                ws.Cells[row, 6].Value = Decimal.Zero;
                                //compensaciones
                                iAux = iColumnaInicio;
                                foreach (var mCompensacion in modelCompensacion.ListaCompensacion)
                                {
                                    ws.Cells[row, iAux].Value = Decimal.Zero;
                                    iAux++;
                                }
                                ws.Cells[row, iColumnaSaldoRecalculo].Value = Decimal.Zero;
                                ws.Cells[row, iColumnaNeto].Value = Decimal.Zero;
                                dValorizacion += item.Valorizacion;
                                if (bMostrarSaldoAnterior)
                                {
                                    ws.Cells[row, iColumnaAnterior].Value = Decimal.Zero;
                                    ws.Cells[row, iColumnaSaldo].Value = Decimal.Zero;
                                }
                                dSaldoRentaCongestion += item.Entregas;
                                //Border por celda
                                rg = ws.Cells[row, 2, row, iUltimo];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                                bSalir = true;
                            }
                            else if (item.EmprNomb.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO && bSalir == true)
                                break;
                        }
                        //Unimos al reporte la linea SALDO RESULTANTE
                        //si (dValorizacion == dSaldoResultante) la distribución del saldo es correcto
                        ws.Cells[row, 2].Value = "SALDO RESULTANTE";
                        ws.Cells[row, 3].Value = (-1 * dValorizacion); //dSaldoResultante;
                        ws.Cells[row, 4].Value = Decimal.Zero;
                        ws.Cells[row, 5].Value = (-1 * dSaldoRentaCongestion); //Total Rentas por Congestión;
                        ws.Cells[row, 6].Value = (dValorizacion + dSaldoRentaCongestion); //(-1 * dSaldoResultante);
                        //compensaciones
                        iAux = iColumnaInicio;
                        foreach (var mCompensacion in modelCompensacion.ListaCompensacion)
                        {
                            ws.Cells[row, iAux].Value = Decimal.Zero;
                            iAux++;
                        }
                        ws.Cells[row, iColumnaSaldoRecalculo].Value = Decimal.Zero;
                        ws.Cells[row, iColumnaNeto].Value = Decimal.Zero;
                        if (bMostrarSaldoAnterior)
                        {
                            ws.Cells[row, iColumnaAnterior].Value = Decimal.Zero;
                            ws.Cells[row, iColumnaSaldo].Value = Decimal.Zero;
                        }
                        //Border por celda
                        rg = ws.Cells[row, 2, row, iUltimo];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        row++;
                        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //Total
                        ws.Cells[row, 2].Value = "Total";
                        ws.Cells[row, 3].Value = Decimal.Zero; //dValorizacion + dSaldoResultante;
                        ws.Cells[row, 4].Value = dValorizacionRSC - dSaldoSinContrato;
                        ws.Cells[row, 5].Value = Decimal.Zero;
                        ws.Cells[row, 6].Value = dSaldoResultante + (dValorizacion + dSaldoRentaCongestion);
                        //compensaciones
                        iAux = iColumnaInicio;
                        foreach (var mCompensacion in modelCompensacion.ListaCompensacion)
                        {
                            ws.Cells[row, iAux].Value = dTotalCompensacion[iAux];
                            iAux++;
                        }
                        ws.Cells[row, iColumnaSaldoRecalculo].Value = dSalrecalculo;
                        ws.Cells[row, iColumnaNeto].Value = dValorTotalEmp;
                        if (bMostrarSaldoAnterior)
                        {
                            ws.Cells[row, iColumnaAnterior].Value = dSaldoAnterior;
                            ws.Cells[row, iColumnaSaldo].Value = dSaldo;
                        }
                        //Border por celda
                        rg = ws.Cells[row, 2, row, iUltimo];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row++;

                        //Fijar panel
                        //ws.View.FreezePanes(6, 9);
                        rg = ws.Cells[6, 2, row, iUltimo];
                        rg.AutoFitColumns();
                        ws.Column(5).Width = 17; //ancho de columna
                        ws.Column(iColumnaSaldoRecalculo).Width = 21; //ancho de columna
                        ws.Column(iColumnaNeto).Width = 21; //ancho de columna
                        if (bMostrarSaldoAnterior)
                        {
                            ws.Column(iColumnaAnterior).Width = 21; //ancho de columna
                            ws.Column(iColumnaSaldo).Width = 21; //ancho de columna
                        }
                        //Detalle del recalculo
                        if (bMostrarSaldoAnterior)
                        {
                            ws.Cells[row + 2, 2].Value = dtoRecalculo.RecaDescripcion;
                            string sInicio = "B" + (row + 2).ToString();
                            string sFin = "L" + (row + 2).ToString();
                            ws.Cells[sInicio + ":" + sFin].Merge = true;
                            rg = ws.Cells[row + 2, 2, row + 2, 2];
                            rg.Style.Font.Size = 10;
                            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                            ws.Row(row + 2).Height = 30;
                        }
                        //MOSTRAR NOTAS
                        row++;
                        ws.Cells[row++, 2].Value = "NOTAS";
                        ws.Cells[row, 2].Value = dtoRecalculo.RecaNota2 + " ";
                        //ws.Cells["B" + row.ToString() + ":M" + row.ToString() + ""].Merge = true;
                        rg = ws.Cells[(row - 1), 2, row, 13];
                        rg.Style.Font.Size = 12;
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                        rg.Style.WrapText = true;

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }
            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirDeterminacionSaldosTransferencias()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreDeterminacionSaldosTransferenciasExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Funcion.AppExcel, sFecha + "_" + Funcion.NombreDeterminacionSaldosTransferenciasExcel);
        }

        [HttpPost]
        public JsonResult ExportarPagosTransferenciasEnergíaActiva(int iPeriCodi, int iVersion)
        {
            string indicador = "1";

            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);
                RecalculoDTO dtoRecalculo = new RecalculoDTO();
                dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(iPeriCodi, iVersion);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                EmpresaPagoModel model = new EmpresaPagoModel();
                model.ListaEmpresasPago = (new ExportarExcelGAppServicio()).BuscarEmpresasPagMatriz(iPeriCodi, iVersion);
                EmpresaPagoModel model2 = new EmpresaPagoModel();

                FileInfo newFile = new FileInfo(path + Funcion.NombrePagosTransferenciasEnergíaActivaExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombrePagosTransferenciasEnergíaActivaExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //fila donde inicia la data
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaCuadro3;
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaNroInforme;
                        ws.Cells[row++, 3].Value = "PAGOS POR TRANSFERENCIAS DE ENERGIA ACTIVA";
                        ws.Cells[row++, 3].Value = dtoPeriodo.PeriNombre;
                        ws.Cells[row++, 3].Value = Funcion.MensajeSoles;
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row, 2].Value = "EMPRESA";
                        rg = ws.Cells[row, 2, row + 1, 3];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                        int colum = 4;
                        for (int i = 0; i < model.ListaEmpresasPago.Count(); i++)
                        {
                            if (i == 0)
                            {
                                model2.ListaEmpresasPago = (new ExportarExcelGAppServicio()).BuscarPagosMatriz(
                                model.ListaEmpresasPago[0].EmpCodi, iPeriCodi, iVersion);
                                foreach (var item2 in model2.ListaEmpresasPago)
                                {
                                    if (item2.EmpPagoCodi != 10582) //- Linea modificada vtajunin
                                    {//- Linea modificada vtajunin
                                        ws.Cells[row, colum].Value = (item2.EmprNombPago != null) ? item2.EmprNombPago.ToString().Trim() : string.Empty;
                                        ws.Cells[row + 1, colum].Value = (item2.EmprRuc != null) ? item2.EmprRuc.ToString().Trim() : string.Empty;
                                        ws.Cells[row, colum, row + 1, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        ws.Cells[row, colum, row + 1, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                        ws.Cells[row, colum, row + 1, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                        ws.Cells[row, colum, row + 1, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[row, colum, row + 1, colum].Style.Font.Bold = true;
                                        ws.Cells[row, colum, row + 1, colum].Style.Font.Size = 10;

                                        ws.Cells[row, 2, row + 1, colum].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[row, 2, row + 1, colum].Style.Border.Left.Color.SetColor(Color.Black);
                                        ws.Cells[row, 2, row + 1, colum].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[row, 2, row + 1, colum].Style.Border.Right.Color.SetColor(Color.Black);
                                        ws.Cells[row, 2, row, colum].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[row, 2, row, colum].Style.Border.Top.Color.SetColor(Color.Black);
                                        ws.Cells[row, 4, row, colum].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[row, 4, row, colum].Style.Border.Bottom.Color.SetColor(Color.Black);
                                        ws.Cells[row + 1, colum, row + 1, colum].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        ws.Cells[row + 1, colum, row + 1, colum].Style.Border.Bottom.Color.SetColor(Color.Black);

                                        ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                        dTotalColum[colum] = 0; //Inicializando los valores
                                        colum++;
                                    }//- Linea modificada vtajunin
                                }
                                ws.Cells[row, colum].Value = "TOTAL";
                                ws.Cells[row, colum, row + 1, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[row, colum, row + 1, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[row, colum, row + 1, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[row, colum, row + 1, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[row, colum, row + 1, colum].Style.Font.Bold = true;
                                ws.Cells[row, colum, row + 1, colum].Style.Font.Size = 10;
                                ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                dTotalColum[colum] = 0;
                            }
                        }
                        row++;
                        row++; //int row = 8;
                        foreach (var item in model.ListaEmpresasPago)
                        {
                            ws.Cells[row, 3].Value = (item.EmprRuc != null) ? item.EmprRuc.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            model2.ListaEmpresasPago = (new ExportarExcelGAppServicio()).BuscarPagosMatriz(item.EmpCodi, iPeriCodi, iVersion);

                            colum = 4;
                            decimal dTotalRow = 0;
                            foreach (var item2 in model2.ListaEmpresasPago)
                            {
                                if (item2.EmpPagoCodi != 10582) //- Linea modificada vtajunin
                                {
                                    if (item2.EmpPagoCodi == 11153) //- Linea modificada vtajunin
                                    { //- Linea modificada vtajunin
                                        EmpresaPagoDTO anterior = model2.ListaEmpresasPago.Where(x => x.EmpPagoCodi == 10582).FirstOrDefault();//- Linea modificada vtajunin

                                        if (anterior != null)//- Linea modificada vtajunin
                                        {//- Linea modificada vtajunin
                                            item2.EmpPagoMonto = item2.EmpPagoMonto + anterior.EmpPagoMonto;//- Linea modificada vtajunin
                                        }//- Linea modificada vtajunin
                                    }//- Linea modificada vtajunin


                                    ws.Cells[row, colum].Value = item2.EmpPagoMonto;
                                    dTotalRow += Convert.ToDecimal(item2.EmpPagoMonto);
                                    dTotalColum[colum] += Convert.ToDecimal(item2.EmpPagoMonto);
                                    colum++;
                                }
                            }


                            ws.Cells[row, colum].Value = dTotalRow; //Pinta el total por fila
                            dTotalColum[colum] += dTotalRow;
                            //Border por celda en la fila
                            rg = ws.Cells[row, 2, row, colum];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        ws.Cells[row, 2].Value = "TOTAL";
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        //for (int i = 0; i <= model2.ListaEmpresasPago.Count(); i++)
                        for (int i = 0; i <= model2.ListaEmpresasPago.Where(x => x.EmpPagoCodi != 10582).Count(); i++) //- Linea modificada vtajunin
                        {
                            ws.Cells[row, colum].Value = dTotalColum[colum];
                            colum++;
                        }
                        rg = ws.Cells[row, 2, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //Fijar panel
                        ws.View.FreezePanes(10, 4);//fijo hasta la fila 7 y columna 2
                        rg = ws.Cells[7, 2, row, colum];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }
            return Json(indicador);
        }

        public JsonResult ExportarPagosTransferenciasEnergíaActivaPrint(int iPeriCodi, int iVersion)
        {
            string indicador = "1";

            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);
                RecalculoDTO dtoRecalculo = new RecalculoDTO();
                dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(iPeriCodi, iVersion);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                EmpresaPagoModel model = new EmpresaPagoModel();
                model.ListaEmpresasPago = (new ExportarExcelGAppServicio()).BuscarEmpresasPagMatriz(iPeriCodi, iVersion);
                
                EmpresaPagoModel model2 = new EmpresaPagoModel();

                FileInfo newFile = new FileInfo(path + Funcion.NombrePagosTransferenciasEnergíaActivaExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombrePagosTransferenciasEnergíaActivaExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        int row2 = 2;
                        int row = 10; //fila donde inicia la data
                        int rowIni = 10; //fila donde inicia la data
                        int rowCab = 8; //fila donde inicia la tabla
                        int totalEmpr = 0;
                        ws.Cells[row2++, 4].Value = dtoRecalculo.RecaCuadro3;
                        ws.Cells[row2++, 4].Value = dtoRecalculo.RecaNroInforme;
                        ws.Cells[row2++, 4].Value = "PAGOS POR TRANSFERENCIAS DE ENERGIA ACTIVA";
                        ws.Cells[row2++, 4].Value = dtoPeriodo.PeriNombre;
                        ws.Cells[row2++, 4].Value = Funcion.MensajeSoles;
                        ExcelRange rg = ws.Cells[2, 4, row2++, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
                        int colum = 4;
                        int vuelta = 0;

                        ws.Cells[rowCab + 1, 3].Value = "RUC";
                        ws.Cells[rowCab, 2].Value = "PARA";
                        ws.Cells[rowCab + 1, 2].Value = "DE";
                        ws.Cells[rowCab, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[rowCab + 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[rowCab, 3, rowCab + 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg = ws.Cells[rowCab, 2, rowCab + 1, 3];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.Column(2).Width = 40;
                        ws.Column(3).Width = 12;

                        foreach (var item in model.ListaEmpresasPago)
                        {
                            row = rowIni + vuelta;
                            colum = 4;
                            vuelta++;
                            ws.Cells[row, 3].Value = (item.EmprRuc != null) ? item.EmprRuc.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString().Trim() : string.Empty;
                            ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                            ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                            
                            model2.ListaEmpresasPago = (new ExportarExcelGAppServicio()).BuscarPagosMatriz(item.EmpCodi, iPeriCodi, iVersion);

                            totalEmpr = model2.ListaEmpresasPago.Count();

                            decimal dTotalRow = 0;
                            int rowx = row;
                            int columnxx = colum;

                            foreach (var item2 in model2.ListaEmpresasPago) //48
                            {
                                if (item2.EmpPagoCodi != 10582) //- Linea modificada vtajunin
                                {
                                    if (item2.EmpPagoCodi == 11153) //- Linea modificada vtajunin
                                    { //- Linea modificada vtajunin
                                        EmpresaPagoDTO anterior = model2.ListaEmpresasPago.Where(x => x.EmpPagoCodi == 10582).FirstOrDefault();//- Linea modificada vtajunin

                                        if (anterior != null)//- Linea modificada vtajunin
                                        {//- Linea modificada vtajunin
                                            item2.EmpPagoMonto = item2.EmpPagoMonto + anterior.EmpPagoMonto;//- Linea modificada vtajunin
                                        }//- Linea modificada vtajunin
                                    }//- Linea modificada vtajunin

                                    ws.Column(colum).Width = 25;

                                    if (vuelta == 1)
                                    {
                                        ws.Cells[rowCab, colum].Value = (item2.EmprNombPago != null) ? item2.EmprNombPago.ToString().Trim() : string.Empty;
                                        ws.Cells[rowCab + 1, colum].Value = (item2.EmprRuc != null) ? item2.EmprRuc.ToString().Trim() : string.Empty;
                                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Font.Bold = true;
                                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Font.Size = 10;
                                        ws.Cells[rowCab, colum].Style.WrapText = true;
                                    }

                                    ws.Cells[row, colum].Value = item2.EmpPagoMonto;
                                    ws.Cells[row, colum].Style.Numberformat.Format = "#,##0.00";
                                    dTotalRow += Convert.ToDecimal(item2.EmpPagoMonto);
                                    dTotalColum[columnxx] += Convert.ToDecimal(item2.EmpPagoMonto);

                                    columnxx++;

                                    if (colum == 21)
                                    {
                                        rg = ws.Cells[row, 2, row, colum];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;

                                        colum = 4;
                                        row = row + 5 + model.ListaEmpresasPago.Count();

                                        if (vuelta == 1)
                                        {
                                            rowCab = rowCab + 5 + model.ListaEmpresasPago.Count();

                                            ws.Cells[rowCab + 1, 3].Value = "RUC";
                                            ws.Cells[rowCab, 2].Value = "PARA";
                                            ws.Cells[rowCab + 1, 2].Value = "DE";
                                            ws.Cells[rowCab, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                            ws.Cells[rowCab + 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                            ws.Cells[rowCab, 3, rowCab + 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                            rg = ws.Cells[rowCab, 2, rowCab + 1, 3];
                                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                                            rg.Style.Font.Color.SetColor(Color.White);
                                            rg.Style.Font.Size = 10;
                                            rg.Style.Font.Bold = true;
                                        }

                                        ws.Cells[row, 3].Value = (item.EmprRuc != null) ? item.EmprRuc.ToString().Trim() : string.Empty;
                                        ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString().Trim() : string.Empty;
                                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                                    }
                                    else colum++;
                                }


                            }

                            ws.Cells[row, colum].Value = dTotalRow;
                            dTotalColum[columnxx] += dTotalRow;

                            rg = ws.Cells[row, 2, row, colum];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;

                            row++;
                        }

                        ws.Cells[rowCab, colum].Value = "TOTAL";
                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Font.Bold = true;
                        ws.Cells[rowCab, colum, rowCab + 1, colum].Style.Font.Size = 10;
                        ws.Column(colum).Style.Numberformat.Format = "#,##0.00";

                        decimal resultado3 = (decimal)totalEmpr / (decimal)18.0;
                        int circ = (int)Math.Ceiling(resultado3);
                        int rowIniTot = 0;
                        int col = 4;
                        int colTab = 4;

                        rowIniTot = rowIni + model.ListaEmpresasPago.Count();
                        ws.Cells[rowIniTot, 2].Value = "TOTAL";
                        ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Font.Bold = true;
                        ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Font.Size = 10;

                        for(int k = 1; k <= totalEmpr; k++)
                        {
                            ws.Cells[rowIniTot, col].Value = dTotalColum[colTab];
                            ws.Column(col).Style.Numberformat.Format = "#,##0.00";

                            if (col == 21)
                            {
                                rg = ws.Cells[rowIniTot, 2, rowIniTot, col];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;

                                col = 4; rowIniTot = rowIniTot + 5 + model.ListaEmpresasPago.Count();
                                ws.Cells[rowIniTot, 2].Value = "TOTAL";
                                ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Font.Bold = true;
                                ws.Cells[rowIniTot, 2, rowIniTot, 3].Style.Font.Size = 10;

                            }
                            else col++;
                            colTab++;
                        }

                        ws.Cells[rowIniTot, col].Value = dTotalColum[colTab];

                        rg = ws.Cells[rowIniTot, 2, rowIniTot, col];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }
            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirPagosTransferenciasEnergíaActiva()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombrePagosTransferenciasEnergíaActivaExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Funcion.AppExcel, sFecha + "_" + Funcion.NombrePagosTransferenciasEnergíaActivaExcel);
        }

        [HttpPost]
        public JsonResult ExportarEntregasRetirosEnergia(int iPeriCodi, int iVersion)
        {
            string indicador = "1";
            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);
                RecalculoDTO dtoRecalculo = new RecalculoDTO();
                dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(iPeriCodi, iVersion);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                ValorTransferenciaModel model = new ValorTransferenciaModel();
                model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).ListarBalanceEnergia1(iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreEntregasRetirosEnergiaExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreEntregasRetirosEnergiaExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //hoja ENERGÍA
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("ENERGÍA");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //fila donde inicia la data
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaCuadro4;
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaNroInforme;
                        ws.Cells[row++, 3].Value = "ENTREGAS Y RETIROS DE ENERGÍA VALORIZADOS";
                        ws.Cells[row++, 3].Value = dtoPeriodo.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        ws.Cells[row, 3].Value = "ENTREGAS (MW.h)";
                        ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 4].Value = "RETIROS (MW.h)";
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 5].Value = "ENTREGAS-RETIROS (MW.h)";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.00";

                        rg = ws.Cells[row, 2, row, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal dEntregas = 0;
                        decimal dRetiros = 0;
                        row++; // int row = 7;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.NombEmpresa.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString().Trim() : string.Empty;
                                ws.Cells[row, 3].Value = item.Entregas;
                                dEntregas += item.Entregas;
                                ws.Cells[row, 4].Value = item.Retiros;
                                dRetiros += item.Retiros;
                                ws.Cells[row, 5].Value = (item.Entregas - item.Retiros);
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 5];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                        }
                        //Unimos con el reporte de retiros sin contrato
                        bool bSalir = false;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.NombEmpresa.ToString().Trim() == Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString().Trim() : string.Empty;
                                ws.Cells[row, 3].Value = item.Entregas;
                                dEntregas += item.Entregas;
                                ws.Cells[row, 4].Value = item.Retiros;
                                dRetiros += item.Retiros;
                                ws.Cells[row, 5].Value = (item.Entregas - item.Retiros);
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 5];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                                bSalir = true;
                            }
                            else if (item.NombEmpresa.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO && bSalir == true)
                                break;
                        }
                        //Total
                        ws.Cells[row, 2].Value = "Total";
                        ws.Cells[row, 3].Value = dEntregas;
                        ws.Cells[row, 4].Value = dRetiros;
                        ws.Cells[row, 5].Value = (dEntregas - dRetiros);
                        //Border por celda
                        rg = ws.Cells[row, 2, row, 5];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row++;

                        //Fijar panel
                        //ws.View.FreezePanes(6, 6);
                        rg = ws.Cells[6, 2, row, 5];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    //hoja VALORIZACIÓN
                    model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).ListarBalanceValorTransferencia1(iPeriCodi, iVersion);
                    ws = xlPackage.Workbook.Worksheets.Add("VALORIZACIÓN");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //fila donde inicia la data
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaCuadro5;
                        ws.Cells[row++, 3].Value = dtoRecalculo.RecaNroInforme;
                        ws.Cells[row++, 3].Value = "ENTREGAS Y RETIROS DE ENERGÍA VALORIZADOS";
                        ws.Cells[row++, 3].Value = dtoPeriodo.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        ws.Cells[row, 3].Value = "ENTREGAS (S/.)";
                        ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 4].Value = "RETIROS (S/.)";
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 5].Value = "ENTREGAS-RETIROS (S/.)";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.00";

                        rg = ws.Cells[row, 2, row, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal dEntregas = 0;
                        decimal dRetiros = 0;
                        row++; // int row = 7;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.NombEmpresa.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString().Trim() : string.Empty;
                                ws.Cells[row, 3].Value = item.Entregas;
                                dEntregas += item.Entregas;
                                ws.Cells[row, 4].Value = item.Retiros;
                                dRetiros += item.Retiros;
                                ws.Cells[row, 5].Value = (item.Entregas - item.Retiros);
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 5];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                        }
                        //Unimos con el reporte de retiros sin contrato
                        bool bSalir = false;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.NombEmpresa.ToString().Trim() == Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString().Trim() : string.Empty;
                                ws.Cells[row, 3].Value = item.Entregas;
                                dEntregas += item.Entregas;
                                ws.Cells[row, 4].Value = item.Retiros;
                                dRetiros += item.Retiros;
                                ws.Cells[row, 5].Value = (item.Entregas - item.Retiros);
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 5];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                                bSalir = true;
                            }
                            else if (item.NombEmpresa.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO && bSalir == true)
                                break;
                        }
                        //Total
                        ws.Cells[row, 2].Value = "Total";
                        ws.Cells[row, 3].Value = dEntregas;
                        ws.Cells[row, 4].Value = dRetiros;
                        ws.Cells[row, 5].Value = (dEntregas - dRetiros);

                        //Border por celda
                        rg = ws.Cells[row, 2, row, 5];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row++;

                        //Fijar panel
                        //ws.View.FreezePanes(6, 6);
                        rg = ws.Cells[6, 2, row, 5];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirBalanceEnergia()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreEntregasRetirosEnergiaExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreEntregasRetirosEnergiaExcel);
        }

        [HttpPost]
        public JsonResult ExportarDesviacionRetiros(int iPeriCodi)
        {
            string indicador = "1";
            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);
                int iNumDias = System.DateTime.DaysInMonth(dtoPeriodo.AnioCodi, dtoPeriodo.MesCodi);
                //Obtener periodo anterior
                PeriodoDTO dtoPeriodoAnterior = (new PeriodoAppServicio()).BuscarPeriodoAnterior(iPeriCodi);
                int iNumDiasAnterior = System.DateTime.DaysInMonth(dtoPeriodoAnterior.AnioCodi, dtoPeriodoAnterior.MesCodi);
                decimal dFactorNumDias = Decimal.Divide(iNumDiasAnterior, iNumDias);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                ValorTransferenciaModel model = new ValorTransferenciaModel();
                model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).ListarDesviacionRetiros(iPeriCodi, dtoPeriodoAnterior.PeriCodi);

                FileInfo newFile = new FileInfo(path + Funcion.NombreDesviacionRetirosExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreDesviacionRetirosExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //hoja ENERGÍA
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("ENERGÍA");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //fila donde inicia la data
                        ws.Cells[row++, 3].Value = "DESVIACIONES DE RETIROS DE ENERGÍA VALORIZADOS";
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 2].Value = "BARRAS DE TRANSFERENCIA";
                        ws.Cells[row, 3].Value = dtoPeriodo.PeriNombre + " [" + iNumDias + "]";
                        ws.Cells[row + 1, 3].Value = 0; //Falta el numero de dias del mes
                        ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 4].Value = dtoPeriodoAnterior.PeriNombre + " [" + iNumDiasAnterior + "]";
                        ws.Cells[row + 1, 4].Value = 0; //Falta el numero de dias del mes
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row + 1, 5].Value = "DESVIACIONES";
                        ws.Column(5).Style.Numberformat.Format = "#0\\.0%";

                        rg = ws.Cells[row, 2, row + 1, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        decimal dPeriodo = 0;
                        decimal dPeriodoAnterior = 0;
                        row = row + 2;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            ws.Cells[row, 2].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran.ToString().Trim() : string.Empty;
                            ws.Cells[row, 3].Value = item.Entregas;
                            dPeriodo += item.Entregas;
                            ws.Cells[row, 4].Value = item.Retiros;
                            dPeriodoAnterior += item.Retiros;

                            if (item.Retiros != 0)
                            {
                                ws.Cells[row, 5].Value = 1 - dFactorNumDias * (item.Entregas / item.Retiros);
                            }
                            else
                            {
                                ws.Cells[row, 5].Value = string.Empty;
                            }


                            //Border por celda
                            rg = ws.Cells[row, 2, row, 5];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Total
                        ws.Cells[row, 2].Value = "Total";
                        ws.Cells[row, 3].Value = dPeriodo;
                        ws.Cells[row, 4].Value = dPeriodoAnterior;
                        if (dPeriodoAnterior > 0)
                            ws.Cells[row, 5].Value = 1 - dFactorNumDias * (dPeriodo / dPeriodoAnterior);
                        //Border por celda
                        rg = ws.Cells[row, 2, row, 5];
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row++;

                        ws.Column(2).Width = 50;
                        ws.Column(3).Width = 30;
                        ws.Column(4).Width = 30;
                        ws.Column(5).Width = 30;


                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirDesviacionRetiros()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreDesviacionRetirosExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreDesviacionRetirosExcel);
        }
        //ASSETEC 20190116
        [HttpPost]
        public JsonResult ExportarDesviacionEntregas(int iPeriCodi, int iVersion)
        {
            string indicador = "1";
            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FactorPerdidaMediaModel model = new FactorPerdidaMediaModel();
                model.ListaFactoresPerdidaMedia = (new TransferenciasAppServicio()).ListDesviacionesEntregas(iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreDesviacionEntregasExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreDesviacionEntregasExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //hoja ENERGÍA
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("ENERGÍA");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //fila donde inicia la data
                        ws.Cells[row++, 3].Value = "DESVIACIONES DE ENTREGAS DE ENERGÍA - GENERACION " + dtoPeriodo.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Column(2).Width = 20;
                        ws.Cells[5, 3].Value = "BARRA DE TRANSFERENCIA";
                        ws.Column(3).Width = 20;
                        ws.Cells[5, 4].Value = "CÓDIGO ENTREGA";
                        ws.Column(4).Width = 10;
                        ws.Cells[5, 5].Value = "CENTRAL DE GENERACIÓN";
                        ws.Column(5).Width = 20;
                        ws.Cells[5, 6].Value = "ENERGÍA (MW.h)";
                        ws.Column(6).Width = 15;
                        ws.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";

                        ws.Cells[5, 7].Value = "MEDIDORES";
                        ws.Column(7).Width = 15;
                        ws.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Column(7).Style.Numberformat.Format = "#,##0.00";

                        ws.Cells[5, 8].Value = "FACTOR DE PÉRDIDAS";
                        ws.Column(8).Width = 15;
                        ws.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Column(8).Style.Numberformat.Format = "#,##0.00000";

                        ws.Cells[5, 9].Value = "NETO (MW.h)";
                        ws.Column(9).Width = 15;
                        ws.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Column(9).Style.Numberformat.Format = "#,##0.00";

                        ws.Cells[5, 10].Value = "% DESVIACION";
                        ws.Column(10).Width = 15;
                        ws.Column(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Column(10).Style.Numberformat.Format = "#0\\.00%";

                        rg = ws.Cells[5, 2, 5, 10];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        row = 6;
                        foreach (var item in model.ListaFactoresPerdidaMedia)
                        {
                            ws.Cells[row, 2].Value = item.Emprnomb.ToString();
                            ws.Cells[row, 3].Value = item.Barrnombre.ToString();
                            ws.Cells[row, 4].Value = item.Codentcodigo.ToString();
                            ws.Cells[row, 5].Value = item.Equinomb.ToString();
                            decimal dEntregaMes = Convert.ToDecimal(item.EntregaMes);
                            decimal dMedidoresMes = Convert.ToDecimal(item.MedidoresMes);
                            decimal dTrnfpmvalor = Convert.ToDecimal(item.Trnfpmvalor);
                            decimal dNeto = dMedidoresMes * dTrnfpmvalor;
                            decimal dDesviacion = 0;
                            if (Math.Abs(dNeto) > 0)
                                dDesviacion = Math.Abs(1 - (dEntregaMes / dNeto));
                            ws.Cells[row, 6].Value = dEntregaMes; //ENERGÍA (MW.h)
                            ws.Cells[row, 7].Value = dMedidoresMes; //(MEDIDORES-SERVICIOS AUXILIARES (MW.h)
                            ws.Cells[row, 8].Value = dTrnfpmvalor; //FACTOR DE PERDIDAS
                            ws.Cells[row, 9].Value = dNeto; //NETO (MW.h)
                            ws.Cells[row, 10].Value = dDesviacion; //% DESVIACION
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 10];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 11);
                        rg = ws.Cells[5, 2, row, 10];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirDesviacionEntregas()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreDesviacionEntregasExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreDesviacionEntregasExcel);
        }

        [HttpPost]
        public JsonResult ExportarHistoricoEntregasRetiros(int pericodiini, int pericodifin, string tipo, string codigo)
        {
            string indicador = "1";
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreHistoricoEntregasRetirosExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreHistoricoEntregasRetirosExcel);
                }

                if (tipo.Equals("")) tipo = null;
                if (codigo.Equals("")) codigo = null;
                ValorTransferenciaModel model = new ValorTransferenciaModel();
                ValorTransferenciaModel modelAnterior = new ValorTransferenciaModel();
                //Lista de Periodos:
                PeriodoDTO dtoPeriodoInicio = (new PeriodoAppServicio()).GetByAnioMes(pericodiini);
                PeriodoDTO dtoPeriodoFinal = (new PeriodoAppServicio()).GetByAnioMes(pericodifin);
                int iRecaCodi = (new RecalculoAppServicio()).GetUltimaVersion(dtoPeriodoFinal.PeriCodi);
                model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).BuscarValorTransferenciaGetByCriteria(null, null, dtoPeriodoFinal.PeriCodi, null, iRecaCodi, tipo);

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //Fila de Inicio de la data
                        ws.Cells[row++, 3].Value = "HISTÓRICO DE ENTREGAS Y RETIROS DE ENERGÍA (MW.h)";
                        ws.Cells[row++, 3].Value = dtoPeriodoInicio.PeriNombre + " a " + dtoPeriodoFinal.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 3, row++, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        ws.Cells[row, 3].Value = "BARRA DE TRANSFERENCIA";
                        ws.Cells[row, 4].Value = "CLIENTE / CENTRAL GENERACIÓN"; //
                        ws.Cells[row, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[row, 6].Value = dtoPeriodoFinal.PeriNombre;
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";

                        rg = ws.Cells[row, 2, row, 6]; //Posición de la cabecera
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int iFila = row;
                        row++; // Posición de inicio de la tabla
                        string[] aPosicionCodigos = new string[10000];
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.EmprNomb.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.BarrNombBarrTran;
                                ws.Cells[row, 4].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString() : string.Empty;
                                ws.Cells[row, 5].Value = item.ValoTranCodEntRet.ToString();
                                ws.Cells[row, 6].Value = item.VTTotalEnergia; //ENERGÍA (MW.h)
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 6];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                aPosicionCodigos[row] = item.ValoTranCodEntRet.ToString();
                                row++;
                            }
                        }
                        //Unimos con el reporte de retiros sin contrato
                        bool bSalir = false;
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.EmprNomb.ToString().Trim() == Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.BarrNombBarrTran;
                                ws.Cells[row, 4].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString() : string.Empty;
                                ws.Cells[row, 5].Value = item.ValoTranCodEntRet.ToString();
                                ws.Cells[row, 6].Value = item.VTTotalEnergia;
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 6];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                aPosicionCodigos[row] = item.ValoTranCodEntRet.ToString();
                                row++;
                                bSalir = true;
                            }
                            else if (item.EmprNomb.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO && bSalir == true)
                                break;
                        }

                        int col = 6; //Columna donde inicia los futuros periodos
                        //Si es del mismo periodo se termina
                        while (dtoPeriodoFinal.PeriCodi > dtoPeriodoInicio.PeriCodi)
                        {
                            //Se continua con los periodos anteriores
                            PeriodoDTO dtoPeriodoAnterior = (new PeriodoAppServicio()).BuscarPeriodoAnterior(dtoPeriodoFinal.PeriCodi);
                            dtoPeriodoFinal.PeriCodi = dtoPeriodoAnterior.PeriCodi; //Actualizamos el final para salir del bucle
                            //Nueva columna
                            col++;
                            row = iFila;
                            ws.Cells[row, col].Value = dtoPeriodoAnterior.PeriNombre;
                            ws.Column(col).Style.Numberformat.Format = "#,##0.00";

                            rg = ws.Cells[row, col, row, col]; //Posición de la cabecera
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            iRecaCodi = (new RecalculoAppServicio()).GetUltimaVersion(dtoPeriodoAnterior.PeriCodi);
                            modelAnterior.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).BuscarValorTransferenciaGetByCriteria(null, null, dtoPeriodoAnterior.PeriCodi, null, iRecaCodi, tipo);
                            foreach (var item in model.ListaValorTransferencia)
                            {
                                foreach (var itemAnterior in modelAnterior.ListaValorTransferencia)
                                {
                                    if (item.ValoTranCodEntRet.ToString() == itemAnterior.ValoTranCodEntRet.ToString())
                                    {
                                        int aux = iFila;
                                        do
                                        {
                                            aux++; //Ubicando la posición del codigo
                                        }
                                        while (!aPosicionCodigos[aux].Equals(itemAnterior.ValoTranCodEntRet.ToString()));
                                        ws.Cells[aux, col].Value = itemAnterior.VTTotalEnergia; //ENERGÍA (MW.h)
                                        ws.Cells[aux, col + 5].Value = itemAnterior.ValoTranCodEntRet.ToString(); //ENERGÍA (MW.h)
                                        //Border por celda
                                        rg = ws.Cells[aux, col, aux, col];
                                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                        rg.Style.Font.Size = 10;
                                        break;
                                    }
                                }
                            }
                        }
                        rg = ws.Cells[iFila, 2, 10000, col];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirHistoricoEntregasRetiros()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreHistoricoEntregasRetirosExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreHistoricoEntregasRetirosExcel);
        }

        [HttpPost]
        public JsonResult ExportarHistorico15minCodigoEntregaRetiro(int pericodiini, int pericodifin, string tipo, string codigo)
        {
            string indicador = "1";
            try
            {
                //Lista de Periodos:
                PeriodoDTO dtoPeriodoInicio = (new PeriodoAppServicio()).GetByAnioMes(pericodiini);
                PeriodoDTO dtoPeriodoFinal = (new PeriodoAppServicio()).GetByAnioMes(pericodifin);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreHistorico15minCodigoEntregaRetiroExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreHistorico15minCodigoEntregaRetiroExcel);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        int row = 2; //Fila de Inicio de la data
                        ws.Cells[row++, 2].Value = "HISTÓRICO DE " + codigo + " DE ENERGÍA (MW.h) EN INTERVALOS DE 15 min.";
                        ws.Cells[row++, 2].Value = dtoPeriodoInicio.PeriNombre + " a " + dtoPeriodoFinal.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 2, row++, 2];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[row++, 2].Value = dtoPeriodoInicio.PeriNombre; //Periodo inicial
                        ws.Column(2).Style.Numberformat.Format = "#,##0.000000";
                        ws.Cells[row++, 1].Value = "VERSION DE DECLARACION";
                        rg = ws.Cells[row - 2, 1, row - 1, 2]; //Posición de la cabecera
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int iRecaCodi = (new RecalculoAppServicio()).GetUltimaVersion(dtoPeriodoInicio.PeriCodi);

                        int colum = 2;
                        if (tipo.Equals("E"))
                        {
                            //Tipo Entrega
                            TransferenciaEntregaModel modelTransferenciaEntrega = new TransferenciaEntregaModel();
                            modelTransferenciaEntrega.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaEntregaByCodigo(0, dtoPeriodoInicio.PeriCodi, iRecaCodi, codigo);
                            if (modelTransferenciaEntrega.Entidad != null)
                            {
                                //Colocando los intervalos de tiempo del mes
                                string sMes = dtoPeriodoInicio.MesCodi.ToString();
                                if (sMes.Length == 1) sMes = "0" + sMes;
                                var Fecha = "01/" + sMes + "/" + dtoPeriodoInicio.AnioCodi;
                                var dates = new List<DateTime>();
                                var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                                var dateFin = dateIni.AddMonths(1);

                                dateIni = dateIni.AddMinutes(15);
                                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                                {
                                    dates.Add(dt);
                                }
                                int fila = row;
                                foreach (var item in dates)
                                {
                                    ws.Cells[fila, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                                    fila++;
                                }

                                ws.Cells[row - 1, colum].Value = "ENTREGA";
                                fila = row;
                                TransferenciaEntregaDetalleModel modelTransferenciaEntregaDetalle = new TransferenciaEntregaDetalleModel();
                                modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaEntregaDetalle(0, dtoPeriodoInicio.PeriCodi, codigo, iRecaCodi);
                                foreach (var dtoTransEntDeta in modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle)
                                {
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah1;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah2;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah3;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah4;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah5;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah6;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah7;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah8;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah9;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah10;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah11;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah12;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah13;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah14;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah15;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah16;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah17;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah18;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah19;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah20;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah21;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah22;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah23;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah24;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah25;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah26;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah27;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah28;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah29;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah30;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah31;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah32;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah33;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah34;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah35;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah36;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah37;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah38;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah39;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah40;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah41;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah42;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah43;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah44;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah45;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah46;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah47;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah48;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah49;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah50;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah51;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah52;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah53;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah54;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah55;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah56;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah57;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah58;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah59;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah60;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah61;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah62;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah63;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah64;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah65;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah66;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah67;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah68;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah69;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah70;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah71;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah72;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah73;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah74;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah75;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah76;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah77;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah78;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah79;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah80;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah81;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah82;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah83;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah84;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah85;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah86;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah87;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah88;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah89;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah90;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah91;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah92;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah93;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah94;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah95;
                                    ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah96;
                                }
                                row = fila; //actualizamos fila del excel
                            }

                            if (pericodiini < pericodifin)
                            {
                                //Se continua con los periodos futuros
                                List<PeriodoDTO> ListaPeriodosFuturos = (new PeriodoAppServicio()).ListarPeriodosFuturos(dtoPeriodoInicio.PeriCodi);
                                ListaPeriodosFuturos.Reverse();
                                foreach (var dtoPeriodoFuturo in ListaPeriodosFuturos)
                                {
                                    iRecaCodi = (new RecalculoAppServicio()).GetUltimaVersion(dtoPeriodoFuturo.PeriCodi);
                                    modelTransferenciaEntrega.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaEntregaByCodigo(0, dtoPeriodoFuturo.PeriCodi, iRecaCodi, codigo);
                                    if (modelTransferenciaEntrega.Entidad != null)
                                    {
                                        //Colocando los intervalos de tiempo del mes
                                        string sMes = dtoPeriodoFuturo.MesCodi.ToString();
                                        if (sMes.Length == 1) sMes = "0" + sMes;
                                        var Fecha = "01/" + sMes + "/" + dtoPeriodoFuturo.AnioCodi;
                                        var dates = new List<DateTime>();
                                        var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                                        var dateFin = dateIni.AddMonths(1);

                                        dateIni = dateIni.AddMinutes(15);
                                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                                        {
                                            dates.Add(dt);
                                        }
                                        int fila = row;
                                        ws.Cells[fila, 3].Value = dtoPeriodoFuturo.PeriNombre;
                                        foreach (var item in dates)
                                        {
                                            ws.Cells[fila, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                                            fila++;
                                        }
                                        fila = row;
                                        TransferenciaEntregaDetalleModel modelTransferenciaEntregaDetalle = new TransferenciaEntregaDetalleModel();
                                        modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaEntregaDetalle(0, dtoPeriodoFuturo.PeriCodi, codigo, iRecaCodi);
                                        foreach (var dtoTransEntDeta in modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle)
                                        {
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah1;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah2;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah3;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah4;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah5;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah6;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah7;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah8;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah9;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah10;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah11;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah12;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah13;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah14;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah15;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah16;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah17;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah18;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah19;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah20;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah21;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah22;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah23;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah24;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah25;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah26;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah27;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah28;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah29;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah30;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah31;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah32;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah33;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah34;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah35;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah36;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah37;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah38;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah39;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah40;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah41;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah42;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah43;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah44;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah45;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah46;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah47;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah48;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah49;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah50;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah51;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah52;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah53;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah54;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah55;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah56;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah57;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah58;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah59;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah60;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah61;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah62;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah63;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah64;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah65;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah66;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah67;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah68;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah69;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah70;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah71;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah72;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah73;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah74;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah75;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah76;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah77;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah78;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah79;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah80;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah81;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah82;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah83;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah84;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah85;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah86;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah87;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah88;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah89;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah90;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah91;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah92;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah93;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah94;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah95;
                                            ws.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah96;
                                        }
                                        row = fila; //actualizamos fila del excel
                                    }
                                    if (dtoPeriodoFuturo.PeriAnioMes == pericodifin)
                                        break;
                                }
                            }
                        }
                        else
                        {
                            //Tipo Retiro
                            TransferenciaRetiroModel modelTransferenciaRetiro = new TransferenciaRetiroModel();
                            modelTransferenciaRetiro.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigo(0, dtoPeriodoInicio.PeriCodi, iRecaCodi, codigo);
                            if (modelTransferenciaRetiro.Entidad != null)
                            {
                                //Colocando los intervalos de tiempo del mes
                                string sMes = dtoPeriodoInicio.MesCodi.ToString();
                                if (sMes.Length == 1) sMes = "0" + sMes;
                                var Fecha = "01/" + sMes + "/" + dtoPeriodoInicio.AnioCodi;
                                var dates = new List<DateTime>();
                                var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                                var dateFin = dateIni.AddMonths(1);

                                dateIni = dateIni.AddMinutes(15);
                                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                                {
                                    dates.Add(dt);
                                }
                                int fila = row;
                                foreach (var item in dates)
                                {
                                    ws.Cells[fila, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                                    fila++;
                                }

                                ws.Cells[row - 1, colum].Value = "RETIRO";
                                fila = row;
                                TransferenciaRetiroDetalleModel modelTransferenciaRetiroDetalle = new TransferenciaRetiroDetalleModel();
                                modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaRetiroDetalle(0, dtoPeriodoInicio.PeriCodi, codigo, iRecaCodi);
                                foreach (var dtoTransRetDeta in modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle)
                                {
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah1;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah2;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah3;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah4;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah5;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah6;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah7;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah8;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah9;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah10;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah11;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah12;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah13;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah14;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah15;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah16;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah17;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah18;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah19;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah20;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah21;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah22;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah23;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah24;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah25;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah26;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah27;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah28;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah29;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah30;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah31;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah32;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah33;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah34;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah35;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah36;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah37;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah38;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah39;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah40;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah41;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah42;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah43;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah44;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah45;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah46;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah47;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah48;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah49;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah50;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah51;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah52;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah53;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah54;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah55;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah56;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah57;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah58;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah59;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah60;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah61;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah62;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah63;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah64;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah65;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah66;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah67;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah68;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah69;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah70;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah71;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah72;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah73;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah74;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah75;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah76;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah77;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah78;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah79;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah80;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah81;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah82;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah83;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah84;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah85;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah86;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah87;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah88;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah89;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah90;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah91;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah92;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah93;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah94;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah95;
                                    ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah96;
                                } //fin foreach (var dtoTransRetDeta in modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle)
                                row = fila; //actualizamos fila del excel
                            } // fin if (modelTransferenciaRetiro.Entidad != null)
                            if (pericodiini < pericodifin)
                            {
                                //Se continua con los periodos futuros
                                List<PeriodoDTO> ListaPeriodosFuturos = (new PeriodoAppServicio()).ListarPeriodosFuturos(dtoPeriodoInicio.PeriCodi);
                                ListaPeriodosFuturos.Reverse();
                                foreach (var dtoPeriodoFuturo in ListaPeriodosFuturos)
                                {
                                    iRecaCodi = (new RecalculoAppServicio()).GetUltimaVersion(dtoPeriodoFuturo.PeriCodi);
                                    modelTransferenciaRetiro.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigo(0, dtoPeriodoFuturo.PeriCodi, iRecaCodi, codigo);
                                    if (modelTransferenciaRetiro.Entidad != null)
                                    {
                                        //Colocando los intervalos de tiempo del mes
                                        string sMes = dtoPeriodoFuturo.MesCodi.ToString();
                                        if (sMes.Length == 1) sMes = "0" + sMes;
                                        var Fecha = "01/" + sMes + "/" + dtoPeriodoFuturo.AnioCodi;
                                        var dates = new List<DateTime>();
                                        var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                                        var dateFin = dateIni.AddMonths(1);

                                        dateIni = dateIni.AddMinutes(15);
                                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                                        {
                                            dates.Add(dt);
                                        }
                                        int fila = row;
                                        ws.Cells[fila, 3].Value = dtoPeriodoFuturo.PeriNombre;
                                        foreach (var item in dates)
                                        {
                                            ws.Cells[fila, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                                            fila++;
                                        }
                                        fila = row;
                                        TransferenciaRetiroDetalleModel modelTransferenciaRetiroDetalle = new TransferenciaRetiroDetalleModel();
                                        modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaRetiroDetalle(0, dtoPeriodoFuturo.PeriCodi, codigo, iRecaCodi);
                                        foreach (var dtoTransRetDeta in modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle)
                                        {
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah1;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah2;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah3;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah4;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah5;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah6;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah7;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah8;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah9;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah10;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah11;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah12;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah13;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah14;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah15;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah16;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah17;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah18;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah19;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah20;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah21;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah22;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah23;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah24;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah25;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah26;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah27;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah28;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah29;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah30;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah31;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah32;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah33;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah34;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah35;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah36;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah37;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah38;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah39;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah40;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah41;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah42;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah43;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah44;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah45;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah46;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah47;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah48;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah49;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah50;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah51;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah52;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah53;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah54;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah55;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah56;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah57;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah58;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah59;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah60;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah61;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah62;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah63;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah64;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah65;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah66;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah67;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah68;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah69;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah70;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah71;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah72;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah73;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah74;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah75;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah76;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah77;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah78;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah79;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah80;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah81;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah82;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah83;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah84;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah85;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah86;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah87;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah88;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah89;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah90;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah91;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah92;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah93;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah94;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah95;
                                            ws.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah96;
                                        } //fin foreach (var dtoTransRetDeta in modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle)
                                        row = fila; //actualizamos fila del excel
                                    }
                                    if (dtoPeriodoFuturo.PeriAnioMes == pericodifin)
                                        break;
                                }
                            }
                        }

                        rg = ws.Cells[5, 1, row - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws.Cells[5, 1, row - 1, 2];
                        rg.AutoFitColumns();
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 0;
                        picture.From.Row = 1;
                        picture.To.Column = 1;
                        picture.To.Row = 1;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = "1";
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirHistorico15minCodigoEntregaRetiro()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreHistorico15minCodigoEntregaRetiroExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreHistorico15minCodigoEntregaRetiroExcel);
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
                string sSql = this.servicioRecalculo.MigrarSaldosVTEA(dtoSiMigracion.Emprcodiorigen, dtoSiMigracion.Emprcodi, pericodi, recpotcodi, out sMensaje, out sDetalle);
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
                    dtoTrnMigracion.Trnmigestado = "E"; //Estado del procedimiento: E: VTEA
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
        public JsonResult MigrarVTEA(int pericodi = 0, int recpotcodi = 0, int migracodi = 0)
        {
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
                string sSql = this.servicioRecalculo.MigrarCalculoVTEA(dtoSiMigracion.Emprcodiorigen, dtoSiMigracion.Emprcodi, pericodi, recpotcodi, out sMensaje, out sDetalle);
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
