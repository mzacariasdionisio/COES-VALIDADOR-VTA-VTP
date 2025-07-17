using COES.Dominio.DTO.Enum;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class EnvioInformacionController : Controller
    {
        readonly TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();
        // GET: /Transferencias/EnvioInformacion/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = (new PeriodoAppServicio()).ListPeriodo();

            TempData["PERIANIOMES2"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");
            TempData["PERIANIOMES1"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");
            TempData["PERIANIOMES3"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");
            TempData["PERIANIOMES4"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");


            EmpresaModel modelEmp2 = new EmpresaModel
            {
                ListaEmpresas = (new EmpresaAppServicio()).ListaEmpresasCombo()
            };
            EmpresaModel modelEmpER = new EmpresaModel
            {
                //ListaEmpresas = (new EmpresaAppServicio()).ListInterCodEntregaRetiro()
                ListaEmpresas = null
            };
            EmpresaModel modelEmpIB = new EmpresaModel
            {
                ListaEmpresas = (new EmpresaAppServicio()).ListaInterCodInfoBase()
            };

            TempData["EMPRCODI1"] = modelEmp2;
            TempData["EMPRCODI2"] = modelEmp2;
            TempData["EMPRCODI3"] = modelEmpER;
            TempData["EMPRCODI4"] = modelEmpIB;
            TempData["EMPRCODI5"] = modelEmp2;


            BarraModel modelBarr = new BarraModel
            {
                ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia()
            };
            TempData["BARRCODI"] = TempData["BARRCODI3"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");

            return View();
        }

        [HttpPost]
        public JsonResult GenerarExcel(int iPeriodo, int iEmpresa)
        {
            int indicador = 1;
            try
            {
                PeriodoModel modelPeriodo = new PeriodoModel
                {
                    Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriodo)
                };

                EmpresaModel modelEmpresa = new EmpresaModel
                {
                    Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(iEmpresa)
                };
                string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                Session["sPrefijoExcel"] = sPrefijoExcel;

                CodigoRetiroModel modelCodigoRetiro = new CodigoRetiroModel();
                EnvioInformacionModel model = new EnvioInformacionModel();
                //Buacamos todos los codigos de entrega y retiro que estan validos para el periodo seleccionado
                int iBarra = 0; //Diferente de cero permite filtrar la información por Barra de Transferencia
                model.ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiroVistaTodo(iPeriodo, iEmpresa, iBarra);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteFormatoEntregaRetiroExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteFormatoEntregaRetiroExcel);
                }

                int row;
                int row2 = 3;
                int colum = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE ENTREGA Y RETIRO DE " + sPrefijoExcel;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row = 6;
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.CodiEntreRetiCodigo != null) ? item.CodiEntreRetiCodigo : string.Empty;
                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                            ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
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
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
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

                        //SEGUNDA HOJA
                        ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws2.Cells[1, colum].Value = (item.CodiEntreRetiCodigo != null) ? item.CodiEntreRetiCodigo.ToString() : string.Empty;
                            ws2.Cells[2, colum].Value = "Mejor información";
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            colum++;
                        }
                        //Color de fondo
                        rg = ws2.Cells[1, 1, 2, colum - 1];
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
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
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
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult GenerarExcelBase(int periodo, int empr)
        {
            try
            {
                int iEmprcodi = empr;

                PeriodoModel modelPeriodo = new PeriodoModel
                {
                    Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(periodo)
                };

                EmpresaModel modelEmpresa = new EmpresaModel
                {
                    Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprcodi)
                };
                string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                Session["sPrefijoExcelBase"] = sPrefijoExcel;

                EnvioInformacionModel model = new EnvioInformacionModel();
                //Buacamos todos los codigos de infomación base que estan validos para el periodo seleccionado
                model.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoInfoBase(periodo, iEmprcodi);

                string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss"); ;
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInformacionBaseExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteInformacionBaseExcel);
                }

                int row;
                int row2 = 3;
                int colum = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE INFORMACIÓN BASE DE " + sPrefijoExcel;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row = 6;
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.CoInfbCodigo != null) ? item.CoInfbCodigo : string.Empty;
                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                            ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
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
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
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

                        //SEGUNDA HOJA
                        ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws2.Cells[1, colum].Value = (item.CoInfbCodigo != null) ? item.CoInfbCodigo.ToString() : string.Empty;
                            ws2.Cells[2, colum].Value = "Mejor información";
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            colum++;
                        }
                        //Color de fondo
                        rg = ws2.Cells[1, 1, 2, colum - 1];
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
                        var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(Fecha);
                        var dateFin = dateIni.AddMonths(1);

                        dateIni = dateIni.AddMinutes(15);
                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                        {
                            dates.Add(dt);
                        }

                        foreach (var item in dates)
                        {
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
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
                return Json("1");
            }
            catch (Exception e)
            {
                return Json(e.Message);//("-1");
            }
        }

        [HttpPost]
        public JsonResult GenerarExcelModelo(int periodo, int empr, int trnmodcodi, int recacodi, int trnenvcodi)
        {
            try
            {
                int iEmprcodi = empr;
                PeriodoModel modelPeriodo = new PeriodoModel
                {
                    Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(periodo)
                };

                RecalculoDTO dtoRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(periodo, recacodi);

                EmpresaModel modelEmpresa = new EmpresaModel
                {
                    Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprcodi)
                };

                string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString() + "_" + dtoRecalculo.RecaCodi + ".";
                Session["sPrefijoExcelModelo"] = sPrefijoExcel;

                EnvioInformacionModel model = new EnvioInformacionModel();
                //Buacamos todos los codigos de infomación base que estan validos para el Modelo seleccionado
                model.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoModeloVTA(periodo, iEmprcodi, trnmodcodi);

                string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInformacionModeloExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteInformacionModeloExcel);
                }

                int row;
                int row2 = 3;
                int colum = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE DATOS DE MODELO DE" + modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriNombre.ToString() + "/" + dtoRecalculo.RecaNombre;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row = 6;
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.CodiEntreRetiCodigo != null) ? item.CodiEntreRetiCodigo : string.Empty;
                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                            ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
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
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);

                        //SEGUNDA HOJA
                        ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        foreach (var item in model.ListaEntregReti)
                        {
                            string sCodigo = item.CodiEntreRetiCodigo;
                            ws2.Cells[1, colum].Value = sCodigo;
                            ws2.Cells[2, colum].Value = "MWh";                                                        
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            colum++;
                        }
                        //Color de fondo
                        rg = ws2.Cells[1, 1, 2, colum - 1];
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
                        var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(Fecha);
                        var dateFin = dateIni.AddMonths(1);

                        dateIni = dateIni.AddMinutes(15);
                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                        {
                            dates.Add(dt);
                        }

                        foreach (var item in dates)
                        {
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
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
                return Json("1");
            }
            catch (Exception e)
            {
                return Json(e.Message);//("-1");
            }
        }
        [HttpGet]
        public virtual ActionResult AbrirExcel()
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteFormatoEntregaRetiroExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcel"].ToString() + "_" + Funcion.NombreReporteFormatoEntregaRetiroExcel);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcelBase()
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteInformacionBaseExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcelBase"].ToString() + "_" + Funcion.NombreReporteInformacionBaseExcel);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcelModelo()
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteInformacionModeloExcel;
            return File(path, Constantes.AppExcel, Session["sPrefijoExcelModelo"].ToString() + "_" + Funcion.NombreReporteInformacionModeloExcel);
        }

        [HttpPost]
        public ActionResult Upload(string sFecha)
        {
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = sFecha + "_" + file.FileName;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }

                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UploadBase(string sFecha)
        {
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = sFecha + "_" + file.FileName;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }

                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
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


            bool bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            if (bGrabar == false)
            {
                modelRecalculo.bEjecutar = false;
            }

            return Json(modelRecalculo);
        }

        public JsonResult GetEmpresasxPeriodo(int pericodi, int version)
        {
            EmpresaModel modelEmpER = new EmpresaModel();
            modelEmpER.ListaEmpresas = (new EmpresaAppServicio()).ListInterCodEntregaRetiroxPeriodo(pericodi, version);

            return Json(modelEmpER);
        }

        /// <summary>
        /// Permite procesar el archivo cargado en un directorio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sNombreArchivo, int sPericodi, int sEmp, int sVer)
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

            try
            {
                //Eliminamos el proceso de valorización vigente en caso existiese.
                string sMensaje = EliminarProcesoValorizacion(sPericodi, sVer);
                if (!sMensaje.Equals("1")) return Json(sMensaje);


                PeriodoDTO periodo = (new PeriodoAppServicio()).GetByIdPeriodo(sPericodi);
                int nroDias = DateTime.DaysInMonth(periodo.AnioCodi, periodo.MesCodi);

                int iEmprcodi = sEmp;
                CodigoEntregaAppServicio servicioTrnCodigoEntrega = new CodigoEntregaAppServicio();
                string pathfinal = path + "/" + sNombreArchivo;
                int tramver = sVer;
                var dt = new DataTable();
                List<string> erroresValor = new List<string>();
                List<string> erroresDatos = new List<string>();
                List<string> erroresRepetidos = new List<string>();
                List<string> erroresValoresNegativos = new List<string>();

                using (var reader = new ExcelDataReader(pathfinal, 1, false))
                    dt.Load(reader);

                List<DatosTransferencia> listData = Funcion.TransformarData(dt, nroDias, iEmprcodi, out erroresValor, out erroresDatos, out erroresRepetidos, out erroresValoresNegativos);

                //listData contiene para la misma empresa.emprcodi una lista de códigos de entrega y retiro que hay que asignar el estado = ACT
                List<string> listaEntregas = new List<string>();
                List<string> listaRetiros = new List<string>();
                List<string> listaRetirosNoDeclarados = new List<string>();
                int count = 0;
                for (int i = 0; i < listData.Count; i++)
                {
                    try
                    {
                        count = i;
                        
                        string sCodigo = listData[i].Codigobarra;
                        CodigoRetiroDTO dtoCodigoRetiro = (new CodigoRetiroAppServicio()).GetCodigoRetiroByCodigo(sCodigo);
                        if (dtoCodigoRetiro != null)
                        {
                            if (dtoCodigoRetiro.EmprCodi == iEmprcodi)
                            {
                                listaRetiros.Add(dtoCodigoRetiro.SoliCodiRetiCodi.ToString()); //Guardamos el ID del código de retiro
                            }
                            else
                            {
                                return Json("El código " + sCodigo + " no le pertenece a la empresa seleccionada");
                            }
                        }
                        else
                        {
                            CodigoEntregaDTO dtoCodigoEntrega = (new CodigoEntregaAppServicio()).GetByCodigoEntregaCodigo(sCodigo);
                            if (dtoCodigoEntrega != null)
                            {
                                if (dtoCodigoEntrega.EmprCodi == iEmprcodi)
                                {
                                    listaEntregas.Add(dtoCodigoEntrega.CodiEntrCodi.ToString()); //Guardamos el ID del código de entrega
                                }
                                else
                                {
                                    return Json("El código " + sCodigo + " no le pertenece a la empresa seleccionada");
                                }
                            }
                            else
                            {
                                //ASSETEC 20200907: Validamos los codigos de Retiros No Declarados
                                CodigoRetiroSinContratoDTO dtoCodigoRetiroNoDeclarado = (new CodigoRetiroSinContratoAppServicio()).BuscarCodigoRetiroSinContratoCodigo(sCodigo);
                                if (dtoCodigoRetiroNoDeclarado != null)
                                {
                                    dtoCodigoRetiroNoDeclarado.GenEmprCodi = iEmprcodi;
                                    listaRetirosNoDeclarados.Add(dtoCodigoRetiroNoDeclarado.CodRetiSinConCodi.ToString()); //Guardamos el ID del código de retiro no declarado
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string mensaje = ex.Message;

                    }
                }
                if (listaRetiros.Count() > 0)
                {
                    listaRetiros = listaRetiros.Distinct().ToList();
                    (new TransferenciaEntregaRetiroAppServicio()).UpdateTransferenciaRetiroEstadoINA(sPericodi, sVer, listaRetiros, iEmprcodi, User.Identity.Name);
                }
                if (listaEntregas.Count() > 0)
                {
                    listaEntregas = listaEntregas.Distinct().ToList();
                    (new TransferenciaEntregaRetiroAppServicio()).UpdateTransferenciaEntregaEstadoINA(sPericodi, sVer, listaEntregas, iEmprcodi, User.Identity.Name);
                }
                //ASSETEC 20200907: Validamos los codigos de Retiros No Declarados
                if (listaRetirosNoDeclarados.Count() > 0)
                {
                    listaRetirosNoDeclarados = listaRetirosNoDeclarados.Distinct().ToList();
                    (new TransferenciaEntregaRetiroAppServicio()).UpdateTransferenciaRetiroEstadoINA(sPericodi, sVer, listaRetirosNoDeclarados, iEmprcodi, User.Identity.Name);
                }

                //Agregamos el envio
                int iTrnEnvCodi = InsertarEnvio(sPericodi, tramver, iEmprcodi, "ER", 0);
                List<DatosTransferencia> resultadoProceso = servicioTrnCodigoEntrega.GrabarEntregaRetiro(listData, sPericodi, tramver, iEmprcodi, User.Identity.Name, iTrnEnvCodi);
                List<string> codigosCorrectos = resultadoProceso.Where(x => !string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();
                List<string> codigosErroneos = resultadoProceso.Where(x => string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();

                //ELIMINAMOS EL ARCHIVO TEMPORAL DEL SERVIDOR
                System.IO.File.Delete(pathfinal);

                //- Debemos obtener los códigos duplicados

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.CargaInformacionVTEAIntranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.SubirFormato;
                objAuditoria.Audproproceso = "Importación de data en excel intranet vtea";
                objAuditoria.Audprodescripcion = "Se importa la data del periodo " + periodo.PeriNombre + " - cantidad de errores - 0" + " - usuario " + User.Identity.Name;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);


                //- Debemos obtener los códigos duplicados
                if (erroresValor.Count > 0 || erroresDatos.Count > 0 || codigosErroneos.Count > 0 || erroresRepetidos.Count > 0 || erroresValoresNegativos.Count > 0)
                {
                    return Json("Códigos Correctos [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos) + "\n" +
                                "Códigos Incorrectos [" + codigosErroneos.Count + "]: " + string.Join(", ", codigosErroneos) + "\n" +
                                "Códigos con valor superior a 350 MWh [" + erroresValor.Count + "]: " + string.Join(", ", erroresValor) + "\n" +
                                "Códigos con valores erróneos [" + erroresDatos.Count + "]: " + string.Join(", ", erroresDatos) + "\n" +
                                "Códigos con valores negativos [" + erroresValoresNegativos.Count + "]: " + string.Join(", ", erroresValoresNegativos) + "\n" +
                                "Códigos repetidos [" + erroresRepetidos.Count + "]: " + string.Join(", ", erroresRepetidos) + "\n"
                                );
                }
                else
                {
                    return Json("Código(s) correcto(s) [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos));
                }
                
            }
            catch (Exception e)
            {
                return Json("Lo sentimos se ha producido un error al momento de leer los valores de energia " + e.Message);
            }
        }

        /// <summary>
        /// Permite insertar un nuevo envio (TRN_ENVIO) desde la intranet
        /// </summary>
        /// <returns>Devuelve el trnenvcodi</returns>
        public int InsertarEnvio(int pericodi, int recacodi, int emprcodi, string TrnEnvTipInf, int trnmodcodi)
        {
            try
            {
                //Preparamos al objeto
                TrnEnvioDTO EntidadEnvio = new TrnEnvioDTO();
                EntidadEnvio.PeriCodi = pericodi;
                EntidadEnvio.RecaCodi = recacodi;
                EntidadEnvio.EmprCodi = emprcodi;
                EntidadEnvio.TrnEnvTipInf = TrnEnvTipInf; //"IB"; //ER  / DM
                EntidadEnvio.TrnModCodi = trnmodcodi;
                EntidadEnvio.TrnEnvPlazo = "C"; //Registrado desde la Intranet
                EntidadEnvio.TrnEnvLiqVt = "S";
                EntidadEnvio.TrnEnvUseIns = User.Identity.Name;
                EntidadEnvio.TrnEnvUseAct = User.Identity.Name;
                //Graba nuevo envio, vacio sin detalle y es el ultimo SI
                return (new TransferenciasAppServicio()).SaveTrnEnvio(EntidadEnvio);
            }
            catch (Exception e)
            {
                string sMensaje = e.Message.ToString();
                return 0; //ERROR
            }
        }

        /// <summary>
        /// Permite eliminar el proceso de calculo de la matriz de pagos - Valorización
        /// </summary>
        /// <returns>1 si la eliminación fue correcta</returns>
        public string EliminarProcesoValorizacion(int pericodi, int vers)
        {
            try
            {
                //Elimina información de la tabla trn_valor_trans = Valorización de la Transferencia de Entregas y Retiros por Empresa[15]
                int eliminavalor = 0;
                eliminavalor = new ValorTransferenciaAppServicio().DeleteListaValorTransferencia(pericodi, vers);

                //Elimina información de la tabla trn_valor_trans_empresa
                int deletepok = 0;
                deletepok = new ValorTransferenciaAppServicio().DeleteValorTransferenciaEmpresa(pericodi, vers);

                //Elimina información calculada de los Ingresos por potencia de las empresas -> tabla trn_saldo_empresa
                int deleteSaldo = 0;
                deleteSaldo = new ValorTransferenciaAppServicio().DeleteSaldoTransmisionEmpresa(pericodi, vers);

                //Elimina información calculada de los Ingresos por Retiros sin contrato de las empresas -> de la tabla trn_saldo_coresc
                int deleteSaldoSC = 0;
                deleteSaldoSC = new ValorTransferenciaAppServicio().DeleteSaldoCodigoRetiroSC(pericodi, vers);

                //Elimina información de la tabla trn_empresa_pago = Matriz de Pagos
                int eliminook = 0;
                eliminook = (new ValorTransferenciaAppServicio()).DeleteEmpresaPago(pericodi, vers);

                //Elimina información calculado del Valor Total de la Empresa -> trn_valor_total_empresa
                int deleteTVEmpresa = 0;
                deleteTVEmpresa = new ValorTransferenciaAppServicio().DeleteValorTotalEmpresa(pericodi, vers);

                if (vers > 1)
                {
                    //Elimina información calculado del Saldo por Recalculo de la Empresa -> trn_saldo_recalculo
                    int deleteSaldoRecalculo = 0;
                    deleteSaldoRecalculo = new ValorTransferenciaAppServicio().DeleteSaldoRecalculo(pericodi, vers);
                }

                return "1";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        ///// <summary>
        ///// Permite procesar el archivo cargado en un directorio
        ///// </summary>
        ///// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoBase(string sNombreArchivo, int sPericodi, int sEmp, int sVer)
        {
            string pathfinal = "";
            string extension = "";
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

            try
            {
                //tratamos el archivo cargado en el directorio
                pathfinal = path + "/" + sNombreArchivo;

                //obtiene extension
                extension = Path.GetExtension(pathfinal);

                DataSet ds = new DataSet();
                ds = (new Funcion()).GeneraDataset(pathfinal, 2);

                CodigoInfoBaseModel modelInfoBase = new CodigoInfoBaseModel();
                TransferenciaInformacionBaseModel modelTransInfoBase = new TransferenciaInformacionBaseModel();
                TransferenciaInformacionBaseDTO entidadTInfoBase = new TransferenciaInformacionBaseDTO();
                TransferenciaInformacionBaseDetalleDTO entidadTInfoBaseDetalle = new TransferenciaInformacionBaseDetalleDTO();
                //////VARIABLES PARA CONFIRMACION DE ELIMNAR REGISTROS////////////
                int delInfobaseok = 0;
                int delInfobasedetaok = 0;

                // empresa
                int iEmprcodi = sEmp;
                //obtener el codigo de la version
                int tramver = 0;
                tramver = sVer;

                //ARRAYLIST DETA INFOBASE
                ArrayList arrylist = new ArrayList();

                //ArrayLIST DETA RETIRO
                ArrayList arrylistR = new ArrayList();

                //int Clicodi = 0;
                int cod = 0;
                decimal promedio = 0;
                int DatosVacios = 0;
                string sAux = "";
                double dAux = 0;
                bool isDouble = false;
                decimal suma = 0;
                string sResultadoCodigoNoExiste = ""; //Resultado del procesamiento: 1:Exito, sin errores
                string sResultadoCodigoDataErrada = ""; //Resultado del procesamiento de codigos con errores en sus casillas
                string sResultadoCodigoDataExcedida = ""; //Resultado del procesamiento de codigos con errores en sus casillas
                int CodigosCorrectos = 0;
                int CodigosErrados = 0;

                ArrayList listColuTodo = new ArrayList();
                //Recorremos columna por coluna el archivo
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    bool bSalir = false;
                    foreach (DataRow dtRow in ds.Tables[0].Rows)
                    {
                        if (dc.ColumnName.ToString().Equals("Column1"))
                        {   //Primera columna que contiene los encabezados
                            bSalir = true;
                            break;
                        }
                        else
                        {   //A partir de la segunda columna que contiene los datos
                            string sCelda = dtRow[dc].ToString();
                            if (String.IsNullOrEmpty(sCelda))
                            {
                                listColuTodo.Add("0");
                                DatosVacios++;
                            }
                            else if (listColuTodo.Count == 0)
                            {   //Celda que contiene: Final, Preliminar, Mejor Información
                                sCelda = sCelda.Trim().ToUpper();
                                if (!(sCelda.Equals("FINAL") || sCelda.Equals("MEJOR INFORMACIÓN")))
                                    sCelda = "PRELIMINAR";

                                listColuTodo.Add(sCelda.Trim());
                                CodigosCorrectos++;
                            }
                            else
                            {   //Lectura de datos
                                sAux = sCelda.Trim();
                                isDouble = Double.TryParse(sAux, out dAux);
                                if (isDouble)
                                    if (dAux <= Funcion.dLimiteMaxEnergia)
                                    {   //Dato correcto, se inserta a la lista
                                        listColuTodo.Add(dAux);
                                    }
                                    else
                                    {   //Error, en el codigo uno de sus valores excede dLimiteMaxEnergia. No se graba el codigo y se continua con el siguiente
                                        if (sResultadoCodigoDataExcedida.Equals(""))
                                            sResultadoCodigoDataExcedida = " Código con información que excede los 350 MWh: [" + dc.ColumnName.ToString() + "]";
                                        else
                                        {
                                            sResultadoCodigoDataExcedida = sResultadoCodigoDataExcedida + ", [" + dc.ColumnName.ToString() + "]";
                                        }
                                        bSalir = true;
                                        CodigosErrados++;
                                        CodigosCorrectos--;
                                        break;
                                    }
                                else
                                {   //Tienes caracteres extraños null por defecto (GeneraDataset)
                                    if (sResultadoCodigoDataErrada.Equals(""))
                                        sResultadoCodigoDataErrada = " Código con información nula o errada: [" + dc.ColumnName.ToString() + "]";
                                    else
                                    {
                                        sResultadoCodigoDataErrada = sResultadoCodigoDataErrada + ", [" + dc.ColumnName.ToString() + "]";
                                    }
                                    bSalir = true;
                                    CodigosErrados++;
                                    CodigosCorrectos--;
                                    break;
                                }
                            }
                        }
                    }
                    if (!bSalir)
                    {   //Si la data es correcta
                        string sCodigo = dc.ColumnName.ToString().Trim();
                        modelInfoBase.Entidad = (new CodigoInfoBaseAppServicio()).CodigoInfoBaseVigenteByPeriodo(sPericodi, sCodigo);
                        if (sEmp == 0 && modelInfoBase.Entidad != null)
                        {   //Si escoge como empresa todos, le colocamos el codigo de la empresa que esta en la entidad 
                            iEmprcodi = modelInfoBase.Entidad.EmprCodi;
                        }
                        if (modelInfoBase.Entidad != null && modelInfoBase.Entidad.EmprCodi == iEmprcodi)
                        {
                            //ASSETEC 202002 - PARA EL CASO DE LA INTRANET, LA INFORMACIÓN INGRESA AL SISTEMA CON EL CÓDIGO TRN_ENVIO.TRNENVCODI IS NULL, QUE SERA UNICO POR MES Y RECALCULO
                            int? iTrnRnvCodi = null;

                            //Eliminar registro de codigo de información base: TRNENVCODI IS NULL
                            delInfobasedetaok = new TransferenciaInformacionBaseAppServicio().DeleteTransferenciaInformacionBaseDetalle(sPericodi, tramver, sCodigo);
                            delInfobaseok = new TransferenciaInformacionBaseAppServicio().DeleteTransferenciaInfoInformacionBase(sPericodi, tramver, sCodigo);

                            //Coloca todos los envio del CODIGO de información base en inactivo
                            TransferenciaInformacionBaseDTO dtoIB = new TransferenciaInformacionBaseDTO();
                            //dtoIB.CoInfbCodi = modelInfoBase.Entidad.CoInfBCodi; /*SOLO LOS RELACIONADOS CON EL CÓDIGO*/
                            dtoIB.EmprCodi = modelInfoBase.Entidad.EmprCodi;
                            dtoIB.PeriCodi = sPericodi;
                            dtoIB.TinfbVersion = tramver;
                            dtoIB.TinfbCodi = -1; //Para que aplique el update
                            (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBase(dtoIB);

                            //modelCodigoEntrega.Entidad = (new GeneralAppServicioCodigoEntrega()).GetByCodigoEntregaCodigo(dc.ColumnName.ToString());
                            entidadTInfoBase.CoInfbCodi = modelInfoBase.Entidad.CoInfBCodi;
                            entidadTInfoBase.EmprCodi = modelInfoBase.Entidad.EmprCodi;
                            entidadTInfoBase.BarrCodi = modelInfoBase.Entidad.BarrCodi;
                            entidadTInfoBase.TinfbCodigo = sCodigo;
                            entidadTInfoBase.EquiCodi = modelInfoBase.Entidad.CentGeneCodi;
                            entidadTInfoBase.PeriCodi = sPericodi;
                            entidadTInfoBase.TinfbVersion = tramver;
                            entidadTInfoBase.TinfbTipoInformacion = listColuTodo[0].ToString();
                            entidadTInfoBase.TinfbUserName = User.Identity.Name;
                            entidadTInfoBase.TinfbEstado = "ACT";

                            //TRNENVCODI IS NULL - informacion reportada sólo desde la intranet
                            entidadTInfoBase.TrnEnvCodi = iTrnRnvCodi;
                            cod = (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBase(entidadTInfoBase);

                            //Graba detalle
                            int cantidadve = 96;
                            ArrayList Listpordias = new ArrayList(cantidadve);
                            for (int c = 1; c < listColuTodo.Count; c += cantidadve)
                            {
                                var arrylistdDiaED = new ArrayList();
                                arrylistdDiaED.AddRange(listColuTodo.GetRange(c, cantidadve));
                                Listpordias.Add(arrylistdDiaED);
                            }

                            for (int c = 0; c < Listpordias.Count; c++)
                            {
                                arrylist = (ArrayList)Listpordias[c];

                                entidadTInfoBaseDetalle.TinfbCodi = cod;
                                entidadTInfoBaseDetalle.TinfbDeDia = (c + 1);
                                entidadTInfoBaseDetalle.TinfbDeVersion = tramver;
                                entidadTInfoBaseDetalle.TinfbDeUserName = User.Identity.Name;

                                suma = 0;
                                try
                                {
                                    suma += entidadTInfoBaseDetalle.TinfbDe1 = Decimal.Parse(arrylist[0].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe2 = Decimal.Parse(arrylist[1].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe3 = Decimal.Parse(arrylist[2].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe4 = Decimal.Parse(arrylist[3].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe5 = Decimal.Parse(arrylist[4].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe6 = Decimal.Parse(arrylist[5].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe7 = Decimal.Parse(arrylist[6].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe8 = Decimal.Parse(arrylist[7].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe9 = Decimal.Parse(arrylist[8].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe10 = Decimal.Parse(arrylist[9].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe11 = Decimal.Parse(arrylist[10].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe12 = Decimal.Parse(arrylist[11].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe13 = Decimal.Parse(arrylist[12].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe14 = Decimal.Parse(arrylist[13].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe15 = Decimal.Parse(arrylist[14].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe16 = Decimal.Parse(arrylist[15].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe17 = Decimal.Parse(arrylist[16].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe18 = Decimal.Parse(arrylist[17].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe19 = Decimal.Parse(arrylist[18].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe20 = Decimal.Parse(arrylist[19].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe21 = Decimal.Parse(arrylist[20].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe22 = Decimal.Parse(arrylist[21].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe23 = Decimal.Parse(arrylist[22].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe24 = Decimal.Parse(arrylist[23].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe25 = Decimal.Parse(arrylist[24].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe26 = Decimal.Parse(arrylist[25].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe27 = Decimal.Parse(arrylist[26].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe28 = Decimal.Parse(arrylist[27].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe29 = Decimal.Parse(arrylist[28].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe30 = Decimal.Parse(arrylist[29].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe31 = Decimal.Parse(arrylist[30].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe32 = Decimal.Parse(arrylist[31].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe33 = Decimal.Parse(arrylist[32].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe34 = Decimal.Parse(arrylist[33].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe35 = Decimal.Parse(arrylist[34].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe36 = Decimal.Parse(arrylist[35].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe37 = Decimal.Parse(arrylist[36].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe38 = Decimal.Parse(arrylist[37].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe39 = Decimal.Parse(arrylist[38].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe40 = Decimal.Parse(arrylist[39].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe41 = Decimal.Parse(arrylist[40].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe42 = Decimal.Parse(arrylist[41].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe43 = Decimal.Parse(arrylist[42].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe44 = Decimal.Parse(arrylist[43].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe45 = Decimal.Parse(arrylist[44].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe46 = Decimal.Parse(arrylist[45].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe47 = Decimal.Parse(arrylist[46].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe48 = Decimal.Parse(arrylist[47].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe49 = Decimal.Parse(arrylist[48].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe50 = Decimal.Parse(arrylist[49].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe51 = Decimal.Parse(arrylist[50].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe52 = Decimal.Parse(arrylist[51].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe53 = Decimal.Parse(arrylist[52].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe54 = Decimal.Parse(arrylist[53].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe55 = Decimal.Parse(arrylist[54].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe56 = Decimal.Parse(arrylist[55].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe57 = Decimal.Parse(arrylist[56].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe58 = Decimal.Parse(arrylist[57].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe59 = Decimal.Parse(arrylist[58].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe60 = Decimal.Parse(arrylist[59].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe61 = Decimal.Parse(arrylist[60].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe62 = Decimal.Parse(arrylist[61].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe63 = Decimal.Parse(arrylist[62].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe64 = Decimal.Parse(arrylist[63].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe65 = Decimal.Parse(arrylist[64].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe66 = Decimal.Parse(arrylist[65].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe67 = Decimal.Parse(arrylist[66].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe68 = Decimal.Parse(arrylist[67].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe69 = Decimal.Parse(arrylist[68].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe70 = Decimal.Parse(arrylist[69].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe71 = Decimal.Parse(arrylist[70].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe72 = Decimal.Parse(arrylist[71].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe73 = Decimal.Parse(arrylist[72].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe74 = Decimal.Parse(arrylist[73].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe75 = Decimal.Parse(arrylist[74].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe76 = Decimal.Parse(arrylist[75].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe77 = Decimal.Parse(arrylist[76].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe78 = Decimal.Parse(arrylist[77].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe79 = Decimal.Parse(arrylist[78].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe80 = Decimal.Parse(arrylist[79].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe81 = Decimal.Parse(arrylist[80].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe82 = Decimal.Parse(arrylist[81].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe83 = Decimal.Parse(arrylist[82].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe84 = Decimal.Parse(arrylist[83].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe85 = Decimal.Parse(arrylist[84].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe86 = Decimal.Parse(arrylist[85].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe87 = Decimal.Parse(arrylist[86].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe88 = Decimal.Parse(arrylist[87].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe89 = Decimal.Parse(arrylist[88].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe90 = Decimal.Parse(arrylist[89].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe91 = Decimal.Parse(arrylist[90].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe92 = Decimal.Parse(arrylist[91].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe93 = Decimal.Parse(arrylist[92].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe94 = Decimal.Parse(arrylist[93].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe95 = Decimal.Parse(arrylist[94].ToString(), System.Globalization.NumberStyles.Float);
                                    suma += entidadTInfoBaseDetalle.TinfbDe96 = Decimal.Parse(arrylist[95].ToString(), System.Globalization.NumberStyles.Float);
                                }
                                catch
                                {
                                    return Json("Lo sentimos, se ha producido un error en la lectura de información del código: " + entidadTInfoBase.TinfbCodigo);
                                }
                                entidadTInfoBaseDetalle.TinfbDeSumaDia = suma;
                                promedio = (suma / 96);
                                entidadTInfoBaseDetalle.TinfbDePromedioDia = promedio;

                                int SaveOk = 0;
                                SaveOk = (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBaseDetalle(entidadTInfoBaseDetalle);

                                arrylist.Clear();
                            }   //Cierra el for (int c = 0; c < Listpordias.Count; c++)
                        } //Cierra el if (modelCodigoEntrega.Entidad != null)
                        else
                        {
                            ///EN EL CASO QUE EL CODIGO CARGADO NO EXISTA
                            if (sResultadoCodigoNoExiste.Equals(""))
                                sResultadoCodigoNoExiste = " Código no encontrado o inactivo: [" + dc.ColumnName.ToString() + "]";
                            else if (!sResultadoCodigoNoExiste.Equals(""))
                            {
                                sResultadoCodigoNoExiste = sResultadoCodigoNoExiste + ", [" + dc.ColumnName.ToString() + "]";
                            }
                            CodigosErrados++;
                            CodigosCorrectos--;
                        }
                    }  //Cierra bSalir
                    listColuTodo.Clear();
                } //Cierra foreach (DataColumn dc in ds.Tables[0].Columns) Recorre columna por coluna el archivo

                //ELIMINAMOS EL ARCHIVO TEMPORAL DEL SERVIDOR
                System.IO.File.Delete(pathfinal);

                if (!sResultadoCodigoNoExiste.Equals("") || !sResultadoCodigoDataErrada.Equals("") || !sResultadoCodigoDataExcedida.Equals(""))
                    return Json("Códigos Correctos[" + CodigosCorrectos + "], Códigos Errados[" + CodigosErrados + "]: " + sResultadoCodigoNoExiste + sResultadoCodigoDataErrada + sResultadoCodigoDataExcedida);
                else
                    return Json("Se han procesado [" + CodigosCorrectos + "] código(s) correctamente"); //Exito en el procesamiento... sin errores                
            }
            catch
            {
                return Json("Lo sentimos se ha producido un error al momento de leer los valores de energia");  // (Exception ex) //return Json(ex.ToString());
            }
        }

        /// <summary>
        /// Permite procesar el archivo cargado en un directorio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoBase2(string sNombreArchivo, int sPericodi, int sEmp, int sVer)
        {            
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

            try
            {
                //Eliminamos el proceso de valorización vigente en caso existiese.
                //string sMensaje = EliminarProcesoValorizacion(sPericodi, sVer);
                //if (!sMensaje.Equals("1")) return Json(sMensaje);

                int genemprcodi = sEmp;
                var testado = string.Empty;
                int recacodi = sVer;

                PeriodoDTO periodo = (new PeriodoAppServicio()).GetByIdPeriodo(sPericodi);
                int nroDias = DateTime.DaysInMonth(periodo.AnioCodi, periodo.MesCodi);

                int iEmprcodi = sEmp;
                CodigoEntregaAppServicio servicioTrnCodigoEntrega = new CodigoEntregaAppServicio();
                string pathfinal = path + "/" + sNombreArchivo;
                int tramver = sVer;

                var dt = new DataTable();
                List<string> erroresValor = new List<string>();
                List<string> erroresDatos = new List<string>();
                List<string> erroresRepetidos = new List<string>();
                List<string> erroresValoresNegativos = new List<string>();

                using (var reader = new ExcelDataReader(pathfinal, 1, false))
                    dt.Load(reader);                

                EnvioInformacionModel model = new EnvioInformacionModel();
                if (testado.Equals(""))
                {
                    testado = "ACT";
                    model.EntidadRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(sPericodi, sVer);
                    if (model.EntidadRecalculo != null)
                    {
                        IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                        string sHora = model.EntidadRecalculo.RecaHoraLimite;
                        double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                        double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                        DateTime dDiaHoraLimite = model.EntidadRecalculo.RecaFechaLimite.AddHours(dHora);
                        dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                        if (dDiaHoraLimite < System.DateTime.Now)
                        {   //YA NO esta en plazo
                            testado = "INA";
                        }
                    }
                }
                
                model.EntidadEnvio = new TrnEnvioDTO();
                model.EntidadEnvio.PeriCodi = sPericodi;
                model.EntidadEnvio.RecaCodi = recacodi;
                model.EntidadEnvio.EmprCodi = genemprcodi;
                model.EntidadEnvio.TrnEnvTipInf = "IB";                
                model.EntidadEnvio.TrnEnvPlazo = "S";
                model.EntidadEnvio.TrnEnvLiqVt = "S";

                if (testado.Equals("INA"))
                {
                    model.EntidadEnvio.TrnEnvPlazo = "N";
                    model.EntidadEnvio.TrnEnvLiqVt = "N";                    
                }

                if (model.EntidadEnvio.TrnEnvTipInf.Equals("DM"))//En este caso el proceso es de Modelo
                {
                    model.EntidadEnvio.TrnEnvLiqVt = "N";
                    testado = "INA";                    
                }

                model.EntidadEnvio.TrnEnvUseIns = User.Identity.Name;
                model.EntidadEnvio.TrnEnvUseAct = User.Identity.Name;

                if (model.EntidadEnvio.TrnEnvLiqVt.Equals("S"))
                {
                    //Antes de grabar cabecera actualiza los estados de "SI" a "NO"
                    //Todos los envios TrnEnvTipInf:ER/DM (pericodi, recacodi, emprcodi, trnmodcodi) la dtoTrnEnvio.TrnEnvLiqVt <- N
                    this.servicioTransferencia.UpdateByCriteriaTrnEnvio(sPericodi, recacodi, genemprcodi, 0, model.EntidadEnvio.TrnEnvTipInf, User.Identity.Name);
                }

                //Grabamos Cabecera
                int iTrnEnvCodi = InsertarEnvio(sPericodi, tramver, iEmprcodi, "IB", 0);

                string sResultadoCodigoNoExiste = ""; //Resultado del procesamiento: 1:Exito, sin errores
                string sResultadoCodigoDataErrada = ""; //Resultado del procesamiento de codigos con errores en sus casillas
                string sResultadoCodigoDataExcedida = ""; //Resultado del procesamiento de codigos con errores en sus casillas
                string sCodigosCorrectos = "";
                int CodigosCorrectos = 0;
                int CodigosErrados = 0;
                
                List<string> codigosErrorValor = new List<string>();
                List<string> codigosErrorDatos = new List<string>();

                TransferenciaInformacionBaseDTO dtoIB = new TransferenciaInformacionBaseDTO
                {
                    EmprCodi = sEmp,
                    PeriCodi = sPericodi,
                    TinfbVersion = sVer,
                    TinfbCodi = -1 //Para que aplique el update
                };
                (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBase(dtoIB);

                
                //Recorrer matriz para grabar detalle
                for (int col = 1; col < dt.Columns.Count; col++)
                {   
                    //Por Fila
                    if (dt.Rows[2][col] == null)
                        break; //FIN - no existe dato en celda

                    string sCodigo = dt.Rows[0][col].ToString();
                    string sTipodato = dt.Rows[1][col].ToString();
                    CodigoInfoBaseDTO dtoCodigoInfoBase = (new CodigoInfoBaseAppServicio()).CodigoInfoBaseVigenteByPeriodo(sPericodi, sCodigo);
                    if (dtoCodigoInfoBase != null && dtoCodigoInfoBase.EmprCodi == sEmp)
                    {
                        //Eliminar registro de codigo de información base
                        TransferenciaInformacionBaseDTO dtoTransInfoBase = new TransferenciaInformacionBaseDTO();
                        dtoTransInfoBase.TrnEnvCodi = iTrnEnvCodi;
                        dtoTransInfoBase.CoInfbCodi = dtoCodigoInfoBase.CoInfBCodi;
                        dtoTransInfoBase.EmprCodi = dtoCodigoInfoBase.EmprCodi;
                        dtoTransInfoBase.BarrCodi = dtoCodigoInfoBase.BarrCodi;
                        dtoTransInfoBase.TinfbCodigo = sCodigo;
                        dtoTransInfoBase.EquiCodi = dtoCodigoInfoBase.CentGeneCodi;
                        dtoTransInfoBase.PeriCodi = sPericodi;
                        dtoTransInfoBase.TinfbVersion = recacodi;
                        dtoTransInfoBase.TinfbTipoInformacion = sTipodato;
                        dtoTransInfoBase.TinfbEstado = testado;
                        dtoTransInfoBase.TinfbUserName = User.Identity.Name;
                        int iTinfbCodi = (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBase(dtoTransInfoBase);                        
                        int iSaveOk = 0;
                        bool valorMaximo = true;
                        bool valorError = true;
                        //Recorremos la matriz que se inicia en la fila 2
                        for (int indice = 1; indice <= nroDias; indice++)
                        {
                            decimal suma = 0;
                            TransferenciaInformacionBaseDetalleDTO dtoTransInfoBaseDetalle = new TransferenciaInformacionBaseDetalleDTO();
                            dtoTransInfoBaseDetalle.TinfbCodi = iTinfbCodi;
                            dtoTransInfoBaseDetalle.TinfbDeDia = indice;
                            dtoTransInfoBaseDetalle.TinfbDeVersion = recacodi;
                            dtoTransInfoBaseDetalle.TinfbDeUserName = User.Identity.Name;
                            for (int k = 1; k <= 96; k++)
                            {
                                object valor = dt.Rows[k + (indice - 1) * 96 + 3][col]; //3 por que empieza en la fila siguiente
                                decimal dvalor = 0;
                                if (valor != null)
                                {
                                    string sValue = valor.ToString();

                                    if (decimal.TryParse(sValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dvalor))
                                    {
                                        dtoTransInfoBaseDetalle.GetType().GetProperty("TinfbDe" + k).SetValue(dtoTransInfoBaseDetalle, dvalor);
                                        suma = suma + dvalor;
                                        if (dvalor > (decimal)Funcion.dLimiteMaxEnergia)
                                        {
                                            valorMaximo = false;
                                        }
                                    }
                                    else
                                    {
                                        valorError = false;
                                    }
                                }
                            }
                            dtoTransInfoBaseDetalle.TinfbDeSumaDia = suma;
                            dtoTransInfoBaseDetalle.TinfbDePromedioDia = suma / 96;
                            iSaveOk += (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBaseDetalle(dtoTransInfoBaseDetalle);
                        }
                        if (iSaveOk == nroDias)
                        {
                            if (sCodigosCorrectos.Equals(""))
                            {
                                sCodigosCorrectos = sCodigo;
                            }
                            else
                            {
                                sCodigosCorrectos = sCodigosCorrectos + ", " + sCodigo;
                            }
                            CodigosCorrectos++;
                        }
                        if (!valorMaximo)
                        {
                            if (sResultadoCodigoDataExcedida.Equals(""))
                                sResultadoCodigoDataExcedida = " Código con información que excede los 350 MWh: [" + sCodigo + "]";
                            else
                            {
                                sResultadoCodigoDataExcedida = sResultadoCodigoDataExcedida + ", [" + sCodigo + "]";
                            }
                            CodigosErrados++;
                            break;
                        }
                        if (!valorError)
                        {
                            if (sResultadoCodigoDataErrada.Equals(""))
                                sResultadoCodigoDataErrada = " Código con información nula o errada: [" + sCodigo + "]";
                            else
                            {
                                sResultadoCodigoDataErrada = sResultadoCodigoDataErrada + ", [" + sCodigo + "]";
                            }
                            CodigosErrados++;
                            break;
                        }
                    }
                    else
                    {
                        ///EN EL CASO QUE EL CODIGO CARGADO NO EXISTA
                        if (sResultadoCodigoNoExiste.Equals(""))
                            sResultadoCodigoNoExiste = " Código no encontrado o inactivo: [" + sCodigo + "]";
                        else if (!sResultadoCodigoNoExiste.Equals(""))
                        {
                            sResultadoCodigoNoExiste = sResultadoCodigoNoExiste + ", [" + sCodigo + "]";
                        }
                        CodigosErrados++;
                    }
                }

                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.CargaInformacionVTEAIntranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.SubirFormato;
                objAuditoria.Audproproceso = "Importación de data en excel intranet vtea";
                objAuditoria.Audprodescripcion = "Se importa la data del periodo " + periodo.PeriNombre + " - cantidad de errores - 0" + " - usuario " + User.Identity.Name;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);

                #endregion AuditoriaProceso

                if (!sResultadoCodigoNoExiste.Equals("") || !sResultadoCodigoDataErrada.Equals("") || !sResultadoCodigoDataExcedida.Equals(""))
                    return Json("Códigos correctos[" + CodigosCorrectos + "]: " + sCodigosCorrectos + "\n" + "Códigos Incorrectos[" + CodigosErrados + "]: \n" + sResultadoCodigoNoExiste + "\n" + sResultadoCodigoDataErrada + "\n" + sResultadoCodigoDataExcedida);
                else
                    return Json("Código(s) correcto(s) [" + CodigosCorrectos + "]: " + sCodigosCorrectos); //Exito en el procesamiento... sin errores

            }
            catch (Exception ex)
            {

                return Json("Lo sentimos se ha producido un error al momento de leer los valores de energia" + ex.ToString());  // (Exception ex) //return Json(ex.ToString());
            }                        
        }
        /// <summary>
        /// Permite procesar el archivo cargado en un directorio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoModelo(string sNombreArchivo, int sPericodi, int sEmp, int sVer, int sTrnmodcodi,int recacodi)
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

            try
            {
                int genemprcodi = sEmp;
                var testado = string.Empty;
                //string sTipoDato = "FINAL";

                //Eliminamos el proceso de valorización vigente en caso existiese.
                string sMensaje = EliminarProcesoValorizacion(sPericodi, sVer);
                if (!sMensaje.Equals("1")) return Json(sMensaje);

                PeriodoDTO periodo = (new PeriodoAppServicio()).GetByIdPeriodo(sPericodi);
                int nroDias = DateTime.DaysInMonth(periodo.AnioCodi, periodo.MesCodi);

                int iEmprcodi = sEmp;
                CodigoEntregaAppServicio servicioTrnCodigoEntrega = new CodigoEntregaAppServicio();
                string pathfinal = path + "/" + sNombreArchivo;
                int tramver = sVer;
                var dt = new DataTable();
                List<string> erroresValor = new List<string>();
                List<string> erroresDatos = new List<string>();
                List<string> erroresRepetidos = new List<string>();
                List<string> erroresValoresNegativos = new List<string>();

                using (var reader = new ExcelDataReader(pathfinal, 1, false))
                    dt.Load(reader);

                var ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoModelo(sPericodi, iEmprcodi, sTrnmodcodi);
                List<DataRow> list2 = dt.AsEnumerable().ToList();
                var listF = list2[0].ItemArray.ToList();
                listF = listF.Where(x => !string.IsNullOrEmpty(x.ToString())).ToList();

                var test2NotInTest1 = listF.Where(t2 => !ListaEntregReti.Any(t1 => t2.ToString().Contains(t1.CodiEntreRetiCodigo))).ToList();
                if (test2NotInTest1.Count > 0 )
                {
                    return Json("Códigos no pertenecen al modelo [" + test2NotInTest1.Count + "]: " + string.Join(", ", test2NotInTest1));
                }
                List<DatosTransferencia> listData = Funcion.TransformarData(dt, nroDias, iEmprcodi, out erroresValor, out erroresDatos, out erroresRepetidos, out erroresValoresNegativos);

                EnvioInformacionModel model = new EnvioInformacionModel();
                if (testado.Equals(""))
                {
                    testado = "ACT";
                    model.EntidadRecalculo = (new RecalculoAppServicio()).GetByIdRecalculo(sPericodi, recacodi);
                    if (model.EntidadRecalculo != null)
                    {
                        IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                        string sHora = model.EntidadRecalculo.RecaHoraLimite;
                        double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                        double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                        DateTime dDiaHoraLimite = model.EntidadRecalculo.RecaFechaLimite.AddHours(dHora);
                        dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                        if (dDiaHoraLimite < System.DateTime.Now)
                        {   //YA NO esta en plazo
                            testado = "INA";
                        }
                    }
                }

                //Graba Cabecera
                model.EntidadEnvio = new TrnEnvioDTO();
                model.EntidadEnvio.PeriCodi = sPericodi;
                model.EntidadEnvio.RecaCodi = recacodi;
                model.EntidadEnvio.EmprCodi = genemprcodi;
                model.EntidadEnvio.TrnEnvTipInf = "DM";
                model.EntidadEnvio.TrnModCodi = sTrnmodcodi;
                model.EntidadEnvio.TrnEnvPlazo = "S";
                model.EntidadEnvio.TrnEnvLiqVt = "S";

                if (testado.Equals("INA"))
                {
                    model.EntidadEnvio.TrnEnvPlazo = "N";
                    model.EntidadEnvio.TrnEnvLiqVt = "N";
                    //sTipoDato = "MEJOR INFORMACIÓN";
                }

                if (model.EntidadEnvio.TrnEnvTipInf.Equals("DM"))//En este caso el proceso es de Modelo
                {
                    model.EntidadEnvio.TrnEnvLiqVt = "S";
                    testado = "ACT";
                    //sTipoDato = "MEJOR INFORMACIÓN";
                }

                model.EntidadEnvio.TrnEnvUseIns = User.Identity.Name;
                model.EntidadEnvio.TrnEnvUseAct = User.Identity.Name;

                if (model.EntidadEnvio.TrnEnvLiqVt.Equals("S"))
                {
                    //Antes de grabar cabecera actualiza los estados de "SI" a "NO"
                    //Todos los envios TrnEnvTipInf:ER/DM (pericodi, recacodi, emprcodi, trnmodcodi) la dtoTrnEnvio.TrnEnvLiqVt <- N
                    this.servicioTransferencia.UpdateByCriteriaTrnEnvio(sPericodi, recacodi, genemprcodi, sTrnmodcodi, model.EntidadEnvio.TrnEnvTipInf, User.Identity.Name);
                }

                //listData contiene para la misma empresa.emprcodi una lista de códigos de entrega y retiro que hay que asignar el estado = ACT
                List<string> listaEntregas = new List<string>();
                List<string> listaRetiros = new List<string>();
                List<string> listaRetirosNoDeclarados = new List<string>();
                int count = 0;
                List<string> listaEmpresas = new List<string>();
                for (int i = 0; i < listData.Count; i++)
                {
                    try
                    {
                        count = i;
                        string sCodigo = listData[i].Codigobarra;
                        CodigoRetiroDTO dtoCodigoRetiro = (new CodigoRetiroAppServicio()).GetCodigoRetiroByCodigo(sCodigo);
                        if (dtoCodigoRetiro != null)
                        {
                            listData[i].Emprcodi = dtoCodigoRetiro.EmprCodi; //Se le asigna el correcto Empcodi
                            listaEmpresas.Add(dtoCodigoRetiro.EmprCodi.ToString());                           
                        }                        
                    }
                    catch (Exception ex)
                    {
                        string mensaje = ex.Message;

                    }
                }
                listaEmpresas = listaEmpresas.Distinct().ToList();
                //Agregamos el envio
                int iTrnEnvCodi = InsertarEnvio(sPericodi, tramver, iEmprcodi, "DM", sTrnmodcodi);
                List<DatosTransferencia> resultadoProceso = servicioTrnCodigoEntrega.GrabarModeloEnvio(listData, sPericodi, tramver, string.Join(", ", listaEmpresas), User.Identity.Name, iTrnEnvCodi, sTrnmodcodi, testado);
                //Actualizamos los envios anteriores a INA
                servicioTrnCodigoEntrega.UpdateRetirosInactivo(iTrnEnvCodi, sPericodi, tramver);
                List<string> codigosCorrectos = resultadoProceso.Where(x => !string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();
                List<string> codigosErroneos = resultadoProceso.Where(x => string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();

                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.CargaInformacionVTEAIntranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.SubirFormato;
                objAuditoria.Audproproceso = "Importación de data en excel intranet vtea";
                objAuditoria.Audprodescripcion = "Se importa la data del periodo " + periodo.PeriNombre + " - cantidad de errores - 0" + " - usuario " + User.Identity.Name;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);

                #endregion AuditoriaProceso

                //- Debemos obtener los códigos duplicados
                if (erroresValor.Count > 0 || erroresDatos.Count > 0 || codigosErroneos.Count > 0 || erroresRepetidos.Count > 0)
                {
                    return Json("Códigos Correctos [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos) + "\n" +
                                "Códigos Incorrectos [" + codigosErroneos.Count + "]: " + string.Join(", ", codigosErroneos) + "\n" +
                                "Códigos con valor superior a 350 MWh [" + erroresValor.Count + "]: " + string.Join(", ", erroresValor) + "\n" +
                                "Códigos con valores erróneos [" + erroresDatos.Count + "]: " + string.Join(", ", erroresDatos)
                                );
                }
                else
                {
                    return Json("Código(s) correcto(s) [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos));
                }
            }
            catch (Exception e)
            {
                return Json("Lo sentimos se ha producido un error al momento de leer los valores de energia " + e.Message);
            }
        }



        public ActionResult FetchGraphData(int periodo, string codigoER, int empr)
        {
            CodigoEntregaModel modelCodigoEntrega = new CodigoEntregaModel();
            TransferenciaEntregaDetalleModel modelTransferenciaEntregaDetalle = new TransferenciaEntregaDetalleModel();
            CodigoRetiroModel modelCodigoRetiro = new CodigoRetiroModel();
            TransferenciaRetiroDetalleModel modelTransferenciaRetiroDetalle = new TransferenciaRetiroDetalleModel();

            //OBETENRE OCDIGO EMPRESA LOGIN
            //int Emprcodi = Int32.Parse(Session["EMPRCODI"].ToString()); //MCHAVEZ: CORREGIR SESSION SELECCION DE LISTA DE EMPRESAS
            int Emprcodi = empr;
            int version = 0;
            if (Session["VersionG"] != null)
            {
                version = (int)(Session["VersionG"]);
            }

            modelCodigoEntrega.Entidad = (new CodigoEntregaAppServicio()).GetByCodigoEntregaCodigo(codigoER);
            if (modelCodigoEntrega.Entidad != null)
            {
                Emprcodi = modelCodigoEntrega.Entidad.EmprCodi;
                modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaEntregaDetalle(Emprcodi, periodo, codigoER, version);
                var dataEntrega = modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle;
                return Json(new { dataER = dataEntrega, dataCodigo = codigoER, tipo = "E" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                modelCodigoRetiro.Entidad = (new CodigoRetiroAppServicio()).GetByCodigoRetiroCodigo(codigoER);
                Emprcodi = modelCodigoRetiro.Entidad.EmprCodi;
                modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaRetiroDetalle(Emprcodi, periodo, codigoER, version);
                var dataRetiro = modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle;
                return Json(new { dataER = dataRetiro, dataCodigo = codigoER, tipo = "R" }, JsonRequestBehavior.AllowGet);
            }
        }

        //Retirona lista de codigo de retiro y entrega
        public ActionResult getListRetiroEntrega(int periodo, int emprcodi, int version, int barrcodi)
        {
            TransferenciaEntregaModel modelTransferenciaEntrega = new TransferenciaEntregaModel();
            TransferenciaRetiroModel modelTransferenciaRetiro = new TransferenciaRetiroModel();

            //OBETENRE OCDIGO EMPRESA LOGIN
            //int Emprcodi = Int32.Parse(Session["EMPRCODI"].ToString()); //MCHAVEZ: CORREGIR SESSION SELECCION DE LISTA DE EMPRESAS
            Session["VersionG"] = version;

            modelTransferenciaEntrega.ListaTransferenciaEntrega = (new TransferenciaEntregaRetiroAppServicio()).ListTransferenciaEntrega(emprcodi, periodo, version, barrcodi);
            modelTransferenciaRetiro.ListaTransferenciaRetiro = (new TransferenciaEntregaRetiroAppServicio()).ListTransferenciaRetiro(emprcodi, periodo, version, barrcodi);

            var dataEntrega = modelTransferenciaEntrega.ListaTransferenciaEntrega;
            var dataRetiro = modelTransferenciaRetiro.ListaTransferenciaRetiro;

            return Json(new { entr = dataEntrega, reti = dataRetiro }, JsonRequestBehavior.AllowGet);
        }

        //Retirona lista de codigo de InfoBase
        public ActionResult getListInfoBase(int periodo, int emprcodi, int version)
        {
            TransferenciaInformacionBaseModel modelTransInfoBase = new TransferenciaInformacionBaseModel();
            TransferenciaEntregaModel modelTransferenciaEntrega = new TransferenciaEntregaModel();
            TransferenciaRetiroModel modelTransferenciaRetiro = new TransferenciaRetiroModel();

            //OBETENRE OCDIGO EMPRESA LOGIN        
            int Emprcodi = emprcodi;

            //version
            int tramver = version;
            Session["Version"] = tramver;
            modelTransInfoBase.ListaInformacionBase = (new TransferenciaInformacionBaseAppServicio()).ListInformacionBase(Emprcodi, periodo, tramver);
            var dataIB = modelTransInfoBase.ListaInformacionBase;
            return Json(new { infobase = dataIB }, JsonRequestBehavior.AllowGet);
        }

        //Graf InfoBase
        public ActionResult FetchGraphDataInfoBase(int periodo, string codigoER, int empr)
        {
            TransferenciaInformacionBaseDetalleModel modelTransferenciaInfoBase = new TransferenciaInformacionBaseDetalleModel();
            //OBTENRE CODIGO EMPRESA LOGIN
            int Emprcodi = empr;
            //obtener el ultimo codigo de la version
            int tramver = 0;
            if (Session["Version"] != null)
            {
                tramver = (int)(Session["Version"]);
            }
            modelTransferenciaInfoBase.ListaInformacionBaseDetalle = (new TransferenciaInformacionBaseAppServicio()).BuscarTransferenciaInformacionBaseDetalle(Emprcodi, periodo, codigoER, tramver);
            var dataInfB = modelTransferenciaInfoBase.ListaInformacionBaseDetalle;
            return Json(new { dataIB = dataInfB, dataCodigo = codigoER }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DescargarEntregaRetiro(int PeriCodi, int Version, int EmprCodi, int BarrCodi)
        {
            int indicador = 1;
            try
            {
                PeriodoModel modelPeriodo = new PeriodoModel();
                modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(PeriCodi);

                EmpresaModel modelEmpresa = new EmpresaModel();
                modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(EmprCodi);
                string sNombreEmpresa = modelEmpresa.Entidad.EmprNombre.ToString();
                if (sNombreEmpresa.Equals("( TODOS )"))
                    sNombreEmpresa = "TODAS.LAS.EMPRESAS";
                string sPrefijoExcel = sNombreEmpresa + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                Session["sPrefijoExcel"] = sPrefijoExcel;

                CodigoRetiroModel modelCodigoRetiro = new CodigoRetiroModel();
                EnvioInformacionModel model = new EnvioInformacionModel();
                //Buacamos todos los codigos de entrega y retiro que estan validos para el periodo seleccionado
                model.ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiroVistaTodo(PeriCodi, EmprCodi, BarrCodi);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteFormatoEntregaRetiroExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteFormatoEntregaRetiroExcel);
                }

                int row;
                int row2 = 6;
                int colum = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE ENTREGA Y RETIRO DE " + sPrefijoExcel;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row = 6;
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.CodiEntreRetiCodigo != null) ? item.CodiEntreRetiCodigo : string.Empty;
                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                            ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
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
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 0;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    //SEGUNDA HOJA
                    ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                    if (ws2 != null)
                    {
                        ws2.Cells[1, 1].Value = "EMPRESA";
                        ws2.Cells[2, 1].Value = "CLIENTE/C.GENERACIÓN";
                        ws2.Cells[3, 1].Value = "BARRA TRANSFERENCIA";
                        ws2.Cells[4, 1].Value = "CÓDIGO [E/R]";
                        ws2.Cells[5, 1].Value = "VERSION DE DECLARACION";
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            string sCodigo = item.CodiEntreRetiCodigo;
                            ws2.Cells[1, colum].Value = item.EmprNomb;
                            ws2.Cells[2, colum].Value = item.CentGeneCliNombre;
                            ws2.Cells[3, colum].Value = item.BarrNombBarrTran;
                            ws2.Cells[4, colum].Value = sCodigo;
                            TransferenciaEntregaModel modelTransferenciaEntrega = new TransferenciaEntregaModel();
                            modelTransferenciaEntrega.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaEntregaByCodigo(EmprCodi, PeriCodi, Version, sCodigo);
                            if (modelTransferenciaEntrega.Entidad != null)
                            {
                                ws2.Cells[5, colum].Value = modelTransferenciaEntrega.Entidad.TranEntrTipoInformacion;
                                TransferenciaEntregaDetalleModel modelTransferenciaEntregaDetalle = new TransferenciaEntregaDetalleModel();
                                modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaEntregaDetalle(modelTransferenciaEntrega.Entidad.EmprCodi, PeriCodi, sCodigo, Version);
                                int fila = 6;
                                foreach (var dtoTransEntDeta in modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle)
                                {
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah1;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah2;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah3;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah4;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah5;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah6;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah7;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah8;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah9;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah10;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah11;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah12;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah13;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah14;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah15;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah16;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah17;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah18;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah19;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah20;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah21;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah22;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah23;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah24;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah25;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah26;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah27;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah28;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah29;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah30;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah31;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah32;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah33;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah34;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah35;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah36;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah37;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah38;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah39;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah40;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah41;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah42;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah43;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah44;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah45;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah46;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah47;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah48;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah49;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah50;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah51;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah52;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah53;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah54;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah55;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah56;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah57;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah58;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah59;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah60;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah61;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah62;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah63;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah64;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah65;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah66;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah67;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah68;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah69;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah70;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah71;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah72;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah73;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah74;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah75;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah76;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah77;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah78;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah79;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah80;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah81;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah82;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah83;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah84;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah85;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah86;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah87;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah88;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah89;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah90;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah91;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah92;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah93;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah94;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah95;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah96;
                                }
                            }
                            else
                            {
                                TransferenciaRetiroModel modelTransferenciaRetiro = new TransferenciaRetiroModel();
                                modelTransferenciaRetiro.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigo(EmprCodi, PeriCodi, Version, sCodigo);
                                if (modelTransferenciaRetiro.Entidad != null)
                                {
                                    ws2.Cells[5, colum].Value = modelTransferenciaRetiro.Entidad.TranRetiTipoInformacion;
                                    TransferenciaRetiroDetalleModel modelTransferenciaRetiroDetalle = new TransferenciaRetiroDetalleModel();
                                    modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaRetiroDetalle(modelTransferenciaRetiro.Entidad.EmprCodi, PeriCodi, sCodigo, Version);
                                    int fila = 6;
                                    foreach (var dtoTransRetDeta in modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle)
                                    {
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah1;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah2;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah3;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah4;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah5;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah6;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah7;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah8;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah9;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah10;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah11;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah12;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah13;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah14;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah15;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah16;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah17;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah18;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah19;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah20;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah21;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah22;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah23;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah24;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah25;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah26;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah27;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah28;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah29;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah30;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah31;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah32;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah33;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah34;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah35;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah36;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah37;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah38;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah39;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah40;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah41;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah42;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah43;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah44;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah45;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah46;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah47;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah48;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah49;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah50;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah51;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah52;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah53;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah54;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah55;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah56;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah57;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah58;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah59;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah60;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah61;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah62;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah63;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah64;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah65;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah66;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah67;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah68;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah69;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah70;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah71;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah72;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah73;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah74;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah75;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah76;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah77;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah78;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah79;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah80;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah81;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah82;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah83;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah84;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah85;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah86;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah87;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah88;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah89;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah90;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah91;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah92;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah93;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah94;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah95;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah96;
                                    } //fin foreach (var dtoTransRetDeta in modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle)
                                } // fin if (modelTransferenciaRetiro.Entidad != null)
                            } //fin del else
                            colum++;
                        }//din foreach (var item in model.ListaEntregReti)
                        //Color de fondo
                        ExcelRange rg = ws2.Cells[1, 1, 5, colum - 1];
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
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row2++;
                        }
                        rg = ws2.Cells[6, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
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
                    } //fin if (ws2 != null)
                    xlPackage.Save();
                } //fin using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                indicador = 1;
            } // try
          catch (Exception e)
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirEntregaRetiro()
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteFormatoEntregaRetiroExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcel"].ToString() + "_" + Funcion.NombreReporteFormatoEntregaRetiroExcel);
        }

        [HttpPost]
        public JsonResult DescargarInfoBase(int PeriCodi, int Version, int EmprCodi)
        {
            int indicador = 1;
            try
            {
                PeriodoModel modelPeriodo = new PeriodoModel();
                modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(PeriCodi);

                EmpresaModel modelEmpresa = new EmpresaModel();
                modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(EmprCodi);
                string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                Session["sPrefijoExcel"] = sPrefijoExcel;

                EnvioInformacionModel model = new EnvioInformacionModel();
                //Buacamos todos los codigos de información base que estan validos para el periodo seleccionado
                model.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoInfoBase(PeriCodi, EmprCodi);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInformacionBaseExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteInformacionBaseExcel);
                }

                int row;
                int row2 = 3;
                int colum = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE INFORMACIÓN BASE DE " + sPrefijoExcel;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row = 6;
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.CoInfbCodigo != null) ? item.CoInfbCodigo : string.Empty;
                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                            ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
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
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
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
                    //SEGUNDA HOJA
                    ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                    if (ws2 != null)
                    {
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            string sCodigo = item.CoInfbCodigo;
                            ws2.Cells[1, colum].Value = sCodigo;
                            TransferenciaInformacionBaseModel modelTransferenciaInfoBase = new TransferenciaInformacionBaseModel();
                            modelTransferenciaInfoBase.Entidad = (new TransferenciaInformacionBaseAppServicio()).GetTransferenciaInfoBaseByCodigo(EmprCodi, PeriCodi, Version, sCodigo);
                            if (modelTransferenciaInfoBase.Entidad != null)
                            {
                                ws2.Cells[2, colum].Value = modelTransferenciaInfoBase.Entidad.TinfbTipoInformacion;
                                TransferenciaInformacionBaseDetalleModel modelTransferenciaInfoBaseDetalle = new TransferenciaInformacionBaseDetalleModel();
                                modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle = (new TransferenciaInformacionBaseAppServicio()).BuscarTransferenciaInformacionBaseDetalle(EmprCodi, PeriCodi, sCodigo, Version);
                                int fila = 3;
                                foreach (var dtoTransInfoBaseDeta in modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle)
                                {
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe1;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe2;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe3;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe4;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe5;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe6;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe7;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe8;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe9;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe10;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe11;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe12;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe13;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe14;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe15;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe16;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe17;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe18;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe19;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe20;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe21;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe22;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe23;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe24;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe25;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe26;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe27;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe28;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe29;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe30;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe31;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe32;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe33;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe34;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe35;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe36;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe37;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe38;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe39;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe40;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe41;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe42;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe43;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe44;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe45;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe46;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe47;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe48;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe49;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe50;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe51;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe52;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe53;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe54;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe55;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe56;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe57;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe58;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe59;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe60;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe61;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe62;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe63;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe64;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe65;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe66;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe67;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe68;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe69;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe70;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe71;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe72;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe73;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe74;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe75;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe76;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe77;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe78;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe79;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe80;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe81;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe82;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe83;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe84;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe85;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe86;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe87;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe88;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe89;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe90;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe91;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe92;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe93;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe94;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe95;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe96;
                                }
                            }//fin del if
                            colum++;
                        }//din foreach (var item in model.ListaEntregReti)
                        //Color de fondo
                        ExcelRange rg = ws2.Cells[1, 1, 2, colum - 1];
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
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
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
                    } //fin if (ws2 != null)
                    xlPackage.Save();
                } //fin using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                indicador = 1;
            } // try
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirInfoBase()
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteInformacionBaseExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcel"].ToString() + "_" + Funcion.NombreReporteInformacionBaseExcel);
        }

        [HttpPost]
        public JsonResult DescargarEnergiaMensual(string sPericodi, int sVersion, Int32? sBarrcodi, Int32? sEmprcodi)
        {
            int indicador = 1;
            int pericodi = Int32.Parse(sPericodi);
            //int periodo = 1;
            try
            {
                TransferenciaEntregaDetalleModel model = new TransferenciaEntregaDetalleModel();
                model.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListBalanceEnergiaActiva(pericodi, sVersion, sBarrcodi, sEmprcodi);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteResumenEnergiaMensualExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteResumenEnergiaMensualExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "Reporte de Resumen Energía Mensual";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 3].Value = "EMPRESA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "CLIENTE/UNIDAD DE GENERACIÓN";
                        ws.Cells[5, 6].Value = "ENERGIA (MW.h)";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaTransferenciaEntregaDetalle)
                        {
                            ws.Cells[row, 2].Value = (item.NombBarra != null) ? item.NombBarra.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.TranEntrTipoInformacion != null) ? item.TranEntrTipoInformacion.ToString() : string.Empty; //Se utiliza el campo Tranentrtipoinformacion para traer a la empresa
                            ws.Cells[row, 4].Value = (item.TentCodigo != null) ? item.TentCodigo.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = (item.TentdeUserName != null) ? item.TentdeUserName.ToString() : string.Empty;
                            ws.Cells[row, 6].Value = item.Energia;
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
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        rg = ws.Cells[5, 2, row + 2, 6];
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
        public virtual ActionResult AbrirEnergiaMensual()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteResumenEnergiaMensualExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteResumenEnergiaMensualExcel);
        }


        public string TipoInformacion(int tipoinfocodi)
        {
            List<TipoInformacionDTO> ListaTipoInfo = ListTipoInfo();
            return ListaTipoInfo[tipoinfocodi].TipoInfoCodigo;
        }

        public List<TipoInformacionDTO> ListTipoInfo()
        {
            List<TipoInformacionDTO> Lista = new List<TipoInformacionDTO>();
            TipoInformacionDTO dtoTipoInfo = new TipoInformacionDTO
            {
                TipoInfoCodi = 0,
                TipoInfoCodigo = "ER",
                TipoInfoNombre = "ENTREGAS Y RETIROS"
            };
            Lista.Add(dtoTipoInfo);
            dtoTipoInfo = new TipoInformacionDTO
            {
                TipoInfoCodi = 1,
                TipoInfoCodigo = "IB",
                TipoInfoNombre = "INFORMACIÓN BASE"
            };
            Lista.Add(dtoTipoInfo);
            dtoTipoInfo = new TipoInformacionDTO
            {
                TipoInfoCodi = 2,
                TipoInfoCodigo = "DM",
                TipoInfoNombre = "DATOS DE MODELOS"
            };
            Lista.Add(dtoTipoInfo);
            return Lista;
        }

        public JsonResult GetModelosGM(int emprcodi)
        {
            EnvioInformacionModel modelEI = new EnvioInformacionModel
            {
                ListaModelo = this.servicioTransferencia.ListarTrnModeloByEmpresa(emprcodi)
            };
            return Json(modelEI);
        }

    }
}