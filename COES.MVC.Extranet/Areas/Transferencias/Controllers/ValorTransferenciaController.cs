using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Helper;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Extranet.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using System.Collections;
using System.Globalization;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class ValorTransferenciaController : Controller
    {
        // GET: /Transferencias/valortransferencia/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            EmpresaModel modelEmp = new EmpresaModel();
            modelEmp.ListaEmpresas = (new EmpresaAppServicio()).ListaInterValorTrans();

            TipoEmpresaModel modelTipoEmpresa = new TipoEmpresaModel();
            modelTipoEmpresa.ListaTipoEmpresas = (new TipoEmpresaAppServicio()).ListTipoEmpresas();

            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();

            PeriodoModel modelPeriodo1 = new PeriodoModel();
            modelPeriodo1.ListaPeriodo = (new PeriodoAppServicio()).ListarByEstadoPublicarCerrado();

            TempData["Tipoemprcodigo"] = new SelectList(modelTipoEmpresa.ListaTipoEmpresas, "Tipoemprcodi", "Tipoemprdesc");
            TempData["PERIANIOMES1"] = new SelectList(modelPeriodo1.ListaPeriodo, "PERICODI", "PERINOMBRE");
            TempData["PERIANIOMES2"] = new SelectList(modelPeriodo1.ListaPeriodo, "PERICODI", "PERINOMBRE");
            TempData["EMPRCODI2"] = modelEmp;
            TempData["BARRCODI2"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");

            return View();
        }

        /// <summary>
        /// Permite cargar versiones deacuerdo al periodo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculos = (new RecalculoAppServicio()).ListRecalculosEstadoPublicarCerrado(pericodi);
            return Json(modelRecalculo.ListaRecalculos);
        }

        public JsonResult GetEmpresasxPeriodo(int pericodi, int version)
        {
            EmpresaModel modelEmpER = new EmpresaModel();
            modelEmpER.ListaEmpresas = (new EmpresaAppServicio()).ListInterCodEntregaRetiroxPeriodo(pericodi, version);

            return Json(modelEmpER);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(int? empcodi, int? barrcodi, int? pericodi, int? tipoemprcodi, int? vers, string flagEntrReti)
        {
            if (flagEntrReti.Equals(""))
                flagEntrReti = null;
            ValorTransferenciaModel model = new ValorTransferenciaModel();
            model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).BuscarValorTransferenciaGetByCriteria(empcodi, barrcodi, pericodi, tipoemprcodi, vers, flagEntrReti);
            TempData["tdListaValorTransferencia"] = model.ListaValorTransferencia;
            return PartialView(model);
        }

        public ActionResult View(int id = 0)
        {
            ValorTransferenciaModel model = new ValorTransferenciaModel();
            //model.Entidad = (new GeneralAppServicioValorTransferencia()).GetByIdValorTransferencia(id);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ExportarEntregasRetirosEnergiaValorizados(int? empcodi, int? barrcodi, int? pericodi, int? tipoemprcodi, int? vers, string flagEntrReti)
        {
            int indicador = 1;

            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
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
                    model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).BuscarValorTransferenciaGetByCriteria(empcodi, barrcodi, pericodi, tipoemprcodi, vers, flagEntrReti);
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
                        ws.Cells[row, 4].Value = "TIPO USUARIO";
                        ws.Cells[row, 5].Value = "TIPO DE CONTRATO";
                        ws.Cells[row, 6].Value = "RUC CLIENTE";
                        ws.Cells[row, 7].Value = "ENTREGA/RETIRO";
                        ws.Cells[row, 8].Value = "CLIENTE / CENTRAL GENERACIÓN"; //
                        ws.Cells[row, 9].Value = "ENERGÍA (MW.h)";
                        ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 10].Value = "VALORIZACIÓN S/.";
                        ws.Column(10).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[row, 11].Value = "INFORMACIÓN";
                        rg = ws.Cells[row, 2, row, 11]; //Posición de la cabecera
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
                                ws.Cells[row, 4].Value = item.VtranUserName;
                                ws.Cells[row, 5].Value = item.CentGeneNombre;
                                ws.Cells[row, 6].Value = (item.RucEmpresa != null) ? item.RucEmpresa.ToString() : string.Empty;
                                ws.Cells[row, 7].Value = item.ValoTranCodEntRet.ToString();
                                ws.Cells[row, 8].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString() : string.Empty;
                                ws.Cells[row, 9].Value = item.VTTotalEnergia; //ENERGÍA (MW.h)
                                ws.Cells[row, 10].Value = item.VTTotalDia; //VALORIZACIÓN S/.
                                ws.Cells[row, 11].Value = (item.VTTipoInformacion != null) ? item.VTTipoInformacion.ToString() : string.Empty;
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 11];
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
                            if (item.EmprNomb.ToString().Trim() == Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.BarrNombBarrTran;
                                ws.Cells[row, 4].Value = item.VtranUserName;
                                ws.Cells[row, 5].Value = item.CentGeneNombre;
                                ws.Cells[row, 6].Value = (item.RucEmpresa != null) ? item.RucEmpresa.ToString() : string.Empty;
                                ws.Cells[row, 7].Value = item.ValoTranCodEntRet.ToString();
                                ws.Cells[row, 8].Value = (item.NombEmpresa != null) ? item.NombEmpresa.ToString() : string.Empty;
                                ws.Cells[row, 9].Value = item.VTTotalEnergia;
                                ws.Cells[row, 10].Value = item.VTTotalDia;
                                ws.Cells[row, 11].Value = (item.VTTipoInformacion != null) ? item.VTTipoInformacion.ToString() : string.Empty;
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 11];
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

                        //Fijar panel
                        //ws.View.FreezePanes(6, 8);
                        rg = ws.Cells[7, 2, row, 11];
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
                indicador = 1;
            }
            catch
            {
                indicador = -1;
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
        public ActionResult ListaInfo(int pericodi, int vers)
        {
            //LISTA LAS COLUMNAS DE DETERMINACIÓN
            int version = vers;
            //Extraemos la información de trn_valor_total_empresa
            ValorTransferenciaModel model = new ValorTransferenciaModel();
            model.ListaValorTransferencia = new ValorTransferenciaAppServicio().ObtenerTotalValorEmpresa(pericodi, version);
            model.IdValorTransferencia = version;
            TempData["ListaValorTransferencia"] = model.ListaValorTransferencia;
            return PartialView(model);
        }
        //ASSETEC 20190116
        [HttpPost]
        public JsonResult ExportarDeterminacionSaldosTransferencias(int pericodi, int vers)
        {
            int indicador = 1;
            bool bMostrarSaldoAnterior = false;

            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo = new RecalculoDTO();
                dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, vers);
                CompensacionModel modelCompensacion = new CompensacionModel();
                modelCompensacion.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones(pericodi);

                if (vers > 1) bMostrarSaldoAnterior = true;

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                ValorTransferenciaModel model = new ValorTransferenciaModel();
                if (TempData["ListaValorTransferencia"] != null)
                    model.ListaValorTransferencia = (List<ValorTransferenciaDTO>)TempData["ListaValorTransferencia"];
                else
                {
                    model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).ObtenerTotalValorEmpresa(pericodi, vers);
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
                        rg = ws.Cells[row, 2, 8, 2];
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //----------------------------------------------------------
                        ws.Cells[row, 3].Value = "SALDO DE TRANSFERENCIAS";
                        ws.Cells["C" + row.ToString() + ":D" + row.ToString() + ""].Merge = true; //(7,3,7,4)
                        //----------------------------------------------------------
                        ws.Cells[row, 5].Value = "ASIGNACIÓN DE SALDO RESULTANTE";
                        ws.Cells["E" + row.ToString() + ":E" + (row + 1).ToString() + ""].Merge = true; //(7,5,8,5)
                        rg = ws.Cells[row, 5, (row + 1), 5];
                        rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center; //Alineado Vertical
                        rg.Style.WrapText = true; //Ajuste de texto
                        //----------------------------------------------------------
                        ws.Cells[row, 6].Value = "SALDOS POR COMPENSACIONES";
                        int iColumnaInicio = 6;
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
                        ws.Cells[(row + 1), 4].Value = "RETIRO NO DECLARADO";//ASSETEC 20200421
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        //----------------------------------------------------------
                        ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                        //----------------------------------------------------------
                        int iAux = iColumnaInicio;
                        foreach (var item in modelCompensacion.ListaCompensacion)
                        {
                            ws.Cells[(row + 1), iAux].Value = item.CabeCompNombre; //Nombre de la compensación
                            ws.Column(iAux).Style.Numberformat.Format = "#,##0.00";
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
                        decimal dSaldoResultante = 0;
                        decimal dCompensacion = 0;
                        decimal dSalrecalculo = 0;
                        decimal dValorTotalEmp = 0;
                        decimal dSaldoAnterior = 0;
                        decimal dSaldo = 0;
                        row = row + 2;// int row = 9;//
                        foreach (var item in model.ListaValorTransferencia)
                        {
                            if (item.EmprNomb.ToString().Trim() != Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.Valorizacion;
                                ws.Cells[row, 4].Value = item.SalrscSaldo;
                                ws.Cells[row, 5].Value = item.SalEmpSaldo;
                                //Lista de compensaciones
                                iAux = iColumnaInicio;
                                foreach (var mCompensacion in modelCompensacion.ListaCompensacion)
                                {
                                    IngresoCompensacionDTO dtoIngresoCompensacion = new IngresoCompensacionDTO();
                                    //importe de la compensación por empresa en el mes y la versión
                                    dtoIngresoCompensacion = (new IngresoCompensacionAppServicio()).GetByPeriVersCabCompEmpr(pericodi, mCompensacion.CabeCompCodi, vers, (int)item.EmpCodi);
                                    if (dtoIngresoCompensacion != null)
                                    { ws.Cells[row, iAux].Value = dtoIngresoCompensacion.IngrCompImporte; }
                                    else
                                    { ws.Cells[row, iAux].Value = 0; }
                                    iAux++;
                                }
                                ws.Cells[row, iColumnaSaldoRecalculo].Value = item.Salrecalculo;
                                ws.Cells[row, iColumnaNeto].Value = item.Vtotempresa;
                                dValorizacion += item.Valorizacion;
                                dSaldoSinContrato += item.SalrscSaldo;
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
                            if (item.EmprNomb.ToString().Trim() == Funcion.NOMBEMPR_SINCONTRATO)
                            {
                                ws.Cells[row, 2].Value = item.EmprNomb.ToString().Trim();
                                ws.Cells[row, 3].Value = item.Valorizacion;
                                dValorizacionRSC = item.Valorizacion;
                                //Si (item.Valorizacion == dSaldoSinContrato) la distribución de los retiros sin contrato fue correcto
                                ws.Cells[row, 4].Value = (dSaldoSinContrato * -1);
                                ws.Cells[row, 5].Value = Decimal.Zero;
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
                        ws.Cells[row, 3].Value = dValorizacion; //dValorizacion;
                        ws.Cells[row, 4].Value = Decimal.Zero;
                        ws.Cells[row, 5].Value = dSaldoResultante; //dSaldoResultante;
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
                        ws.Cells[row, 3].Value = dValorizacion + dSaldoResultante;
                        ws.Cells[row, 4].Value = dValorizacionRSC - dSaldoSinContrato;
                        ws.Cells[row, 5].Value = dValorizacion + dSaldoResultante;
                        //compensaciones
                        iAux = iColumnaInicio;
                        foreach (var mCompensacion in modelCompensacion.ListaCompensacion)
                        {
                            ws.Cells[row, iAux].Value = Decimal.Zero;
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
                indicador = 1;
            }
            catch (Exception e)
            {
                indicador = -1;
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
            int indicador = 1;

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
                        ws.Cells[row++, 4].Value = dtoRecalculo.RecaCuadro3;
                        ws.Cells[row++, 4].Value = dtoRecalculo.RecaNroInforme;
                        ws.Cells[row++, 4].Value = "PAGOS POR TRANSFERENCIAS DE ENERGIA ACTIVA";
                        ws.Cells[row++, 4].Value = dtoPeriodo.PeriNombre;
                        ws.Cells[row++, 4].Value = Funcion.MensajeSoles;
                        ExcelRange rg = ws.Cells[2, 4, row++, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        //CABECERA DE TABLA
                        ws.Cells[row + 1, 3].Value = "RUC";
                        ws.Cells[row, 2].Value = "EMPRESA";
                        ws.Cells[row, 4].Merge = true;
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
                                    ws.Cells[row, colum].Value = (item2.EmprNombPago != null) ? item2.EmprNombPago.ToString() : string.Empty;
                                    ws.Cells[row + 1, colum].Value = (item2.EmprRuc != null) ? item2.EmprRuc.ToString().Trim() : string.Empty;
                                    ws.Cells[row, colum, row + 1, colum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[row, colum, row + 1, colum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                                    ws.Cells[row, colum, row + 1, colum].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                                    ws.Cells[row, colum, row + 1, colum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[row, colum, row + 1, colum].Style.Font.Bold = true;
                                    ws.Cells[row, colum, row + 1, colum].Style.Font.Size = 10;
                                    ws.Cells[row, colum, row + 1, colum].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[row, colum, row + 1, colum].Style.Border.Left.Color.SetColor(Color.Black);
                                    ws.Cells[row, colum, row + 1, colum].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[row, colum, row + 1, colum].Style.Border.Right.Color.SetColor(Color.Black);
                                    ws.Cells[row, colum].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[row, colum].Style.Border.Top.Color.SetColor(Color.Black);
                                    ws.Cells[row, colum].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[row, colum].Style.Border.Bottom.Color.SetColor(Color.Black);
                                    ws.Cells[row + 1, colum].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    ws.Cells[row + 1, colum].Style.Border.Bottom.Color.SetColor(Color.Black);
                                    ws.Column(colum).Style.Numberformat.Format = "#,##0.00";
                                    dTotalColum[colum] = 0; //Inicializando los valores
                                    colum++;
                                }

                                ws.Cells[row, colum].Value = "TOTAL";
                                ws.Cells[row, colum, row + 1, colum].Merge = true;
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
                            ws.Cells[row, 3].Value = (item.EmprRuc != null) ? item.EmprRuc.ToString() : string.Empty;
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
                                ws.Cells[row, colum].Value = item2.EmpPagoMonto;
                                dTotalRow += Convert.ToDecimal(item2.EmpPagoMonto);
                                dTotalColum[colum] += Convert.ToDecimal(item2.EmpPagoMonto);
                                colum++;
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
                        ws.Cells[row, 2, row, 3].Merge = true;
                        ws.Cells[row, 2, row, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        ws.Cells[row, 2, row, 3].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        ws.Cells[row, 2, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[row, 2, row, 3].Style.Font.Bold = true;
                        ws.Cells[row, 2, row, 3].Style.Font.Size = 10;

                        colum = 4;
                        for (int i = 0; i <= model2.ListaEmpresasPago.Count(); i++)
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
                indicador = 1;
            }
            catch
            {
                indicador = -1;
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
        public JsonResult exportarEntregasRetirosEnergia(int iPeriCodi, int iVersion)
        {
            int indicador = 1;
            try
            {
                //Información de Cabecera:
                PeriodoDTO dtoPeriodo = new PeriodoDTO();
                dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);
                RecalculoDTO dtoRecalculo = new RecalculoDTO();
                dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(iPeriCodi, iVersion);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                ValorTransferenciaModel model = new ValorTransferenciaModel();
                model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).ListarBalanceEnergia(iPeriCodi, iVersion);

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
                        row++; //int row = 7;
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
                    model.ListaValorTransferencia = (new ValorTransferenciaAppServicio()).ListarBalanceValorTransferencia(iPeriCodi, iVersion);
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
                        row++;//int row = 7;
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
                indicador = 1;
            }
            catch
            {
                indicador = -1;
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



    }
}
