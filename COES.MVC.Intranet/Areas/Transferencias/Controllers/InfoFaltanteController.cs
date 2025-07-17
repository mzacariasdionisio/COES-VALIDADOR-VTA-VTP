using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Transferencias;
//using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Helper;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Collections;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class InfoFaltanteController : Controller
    {
        // GET: /Transferencias/InfoFaltante/
        public ActionResult Index()
        {
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = (new PeriodoAppServicio()).ListPeriodo();

            TempData["Periodo"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");
            TempData["Periodo2"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");

            return Lista(0);
        }

        /// <summary>
        /// Permite cargar versiones deacuerdo al periodo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVersion(int pericodi)
        {

            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculo = (new RecalculoAppServicio()).ListRecalculos(pericodi);

            return Json(modelRecalculo.ListaRecalculo);
        }

        // POST
        [HttpPost]
        public ActionResult Lista(Int32 PeriCodi)
        {
            InfoFaltanteModel model = new InfoFaltanteModel();
            model.ListaInfoFaltante = (new InfoFaltanteAppServicio()).BuscarInfoFaltante(PeriCodi);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcel(Int32 PeriCodi)
        {
            int indicador = 1;

            try
            {
                //Consultamos por el estado del periodo
                PeriodoDTO entidad = new PeriodoDTO();
                entidad = (new PeriodoAppServicio()).GetByIdPeriodo(PeriCodi);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                InfoFaltanteModel model = new InfoFaltanteModel();
                model.ListaInfoFaltante = (new InfoFaltanteAppServicio()).BuscarInfoFaltante(PeriCodi); // Lista todas las barras incluido el atributo Nombre area

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInfoFaltanteExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteInfoFaltanteExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "REPORTE DE INFORMACIÓN FALTANTE - " + entidad.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA";
                        ws.Cells[5, 4].Value = "CLIENTE";
                        ws.Cells[5, 5].Value = "CÓDIGO";

                        rg = ws.Cells[5, 2, 5, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaInfoFaltante)
                        {
                            ws.Cells[row, 2].Value = item.Empresa.ToString();
                            ws.Cells[row, 3].Value = item.Barra.ToString();
                            ws.Cells[row, 4].Value = item.Cliente.ToString();
                            ws.Cells[row, 5].Value = item.Codigo.ToString();

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

                        //Fijar panel
                        ws.View.FreezePanes(6, 6);
                        rg = ws.Cells[5, 2, row, 5];
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
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteInfoFaltanteExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Funcion.AppExcel, sFecha + "_" + Funcion.NombreReporteInfoFaltanteExcel);
        }




        [HttpPost]
        public JsonResult DescargarInfoEntregadaFueraDeFecha(int PeriCodi, int Version)
        {
            int indicador = 1;
            try
            {
                PeriodoModel modelPeriodo = new PeriodoModel();
                modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(PeriCodi);

                EmpresaModel modelEmpresa = new EmpresaModel();
                //modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(EmprCodi);
                //string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                Session["sPrefijoExcel"] = "Reporte";

                CodigoRetiroModel modelCodigoRetiro = new CodigoRetiroModel();
          
                //Buscamos todo los codigos de entrega y retiro  que estan validos pero fuera de fecha de su periodo y version
                InfoFaltanteModel model = new InfoFaltanteModel();
                model.ListaInfoFaltante = (new InfoFaltanteAppServicio()).BuscarInfoEntregada(PeriCodi, Version); // Lista todas las barras incluido el atributo Nombre area

              
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInfoFaltanteEntregaFueradeFechaExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteInfoFaltanteEntregaFueradeFechaExcel);
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
                        ws.Cells[2, 4].Value = "LISTA DE CÓDIGOS DE ENTREGA Y RETIRO DE LAS EMPRESAS FUERA DE FECHA" ;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO/INFOBASE";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row = 6;
                        foreach (var item in model.ListaInfoFaltante)
                        {
                           
                         
         
                            string sCodigo = item.Codigo;
                            TransferenciaEntregaModel modelTransferenciaEntrega = new TransferenciaEntregaModel();
                            modelTransferenciaEntrega.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaEntregaByCodigo(item.EmprCodi, PeriCodi, Version, sCodigo);
                            if (modelTransferenciaEntrega.Entidad != null)
                            {
                      
                                TransferenciaEntregaDetalleModel modelTransferenciaEntregaDetalle = new TransferenciaEntregaDetalleModel();
                                modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaEntregaDetalle(item.EmprCodi, PeriCodi, sCodigo, Version);
                                int fila = 3;
                                decimal sum = 0;
                                decimal sumIni = 0;
                                decimal sumFin = 0;
                                if (item.FechaInicio.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaFin.ToString("yyyyMM") != modelPeriodo.Entidad.PeriAnioMes.ToString())
                                {
                                    for (int i = 0; i < item.FechaInicio.Day - 1; i++)
                                    {
                                        sum += modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle[i].TranEntrDetaSumaDia;
                                    }
                                    if (sum != 0)
                                    {
                                        ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                        ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                        ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                        ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                        ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                        row++;
                                        //print
                                        // ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah1;
                                    }
                                }
                                else if (item.FechaFin.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaInicio.ToString("yyyyMM") != modelPeriodo.Entidad.PeriAnioMes.ToString())
                                {
                                    for (int i = item.FechaFin.Day; i < modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle.Count; i++)
                                    {
                                        sum += modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle[i].TranEntrDetaSumaDia;
                                    }
                                    if (sum != 0)
                                    {
                                        ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                        ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                        ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                        ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                        ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                        row++;
                                        //print
                                        // ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah1;
                                    }
                                }
                                else if (item.FechaInicio.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaFin.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString())
                                {
                                    for (int i = 0; i < item.FechaInicio.Day - 1; i++)
                                    {
                                        sumIni += modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle[i].TranEntrDetaSumaDia;
                                    }
                                    for (int i = item.FechaFin.Day; i < modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle.Count; i++)
                                    {
                                        sumFin += modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle[i].TranEntrDetaSumaDia;
                                    }
                                    if (sumIni != 0 || sumFin != 0)
                                    {
                                        ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                        ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                        ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                        ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                        ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                        row++;
                                        //print
                                        // ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah1;
                                    }
                                }
                            }
                            else
                            {
                                TransferenciaRetiroModel modelTransferenciaRetiro = new TransferenciaRetiroModel();
                                modelTransferenciaRetiro.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigo(item.EmprCodi, PeriCodi, Version, sCodigo);
                                if (modelTransferenciaRetiro.Entidad != null)
                                {
                                   
                                    TransferenciaRetiroDetalleModel modelTransferenciaRetiroDetalle = new TransferenciaRetiroDetalleModel();
                                    modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaRetiroDetalle(item.EmprCodi, PeriCodi, sCodigo, Version);
                                    int fila = 3;
                                    decimal sum = 0;
                                    decimal sumIni = 0;
                                    decimal sumFin = 0;
                                    if (item.FechaInicio.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaFin.ToString("yyyyMM") != modelPeriodo.Entidad.PeriAnioMes.ToString())
                                    {
                                        for (int i = 0; i < item.FechaInicio.Day - 1; i++)
                                        {
                                            sum += modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle[i].TranRetiDetaSumaDia;
                                        }
                                        if (sum != 0)
                                        {
                                            ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                            ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                            ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                            ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                            row++;
                                            //ws.Cells[row, 7].Value = (item.FechaInicio != null) ? item.FechaInicio : DateTime.MinValue;
                                            //print
                                          
                                        }
                                    }
                                    else if (item.FechaFin.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaInicio.ToString("yyyyMM") != modelPeriodo.Entidad.PeriAnioMes.ToString())
                                    {
                                        for (int i = item.FechaFin.Day; i < modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle.Count; i++)
                                        {
                                            sum += modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle[i].TranRetiDetaSumaDia;
                                        }
                                        if (sum != 0)
                                        {
                                            ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                            ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                            ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                            ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                            row++;
                                            // ws.Cells[row, 7].Value = (item.FechaFin != null) ? item.FechaFin : DateTime.MinValue;
                                            //print
                                            
                                        }
                                    }
                                    else if (item.FechaInicio.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaFin.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString())
                                    {
                                        for (int i = 0; i <= item.FechaInicio.Day - 1; i++)
                                        {
                                            sumIni += modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle[i].TranRetiDetaSumaDia;
                                        }
                                        for (int i = item.FechaFin.Day; i < modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle.Count; i++)
                                        {
                                            sumFin += modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle[i].TranRetiDetaSumaDia;
                                        }
                                        if (sumIni != 0 || sumFin != 0)
                                        {
                                            ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                            ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                            ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                            ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                            row++;
                                            // ws.Cells[row, 7].Value = (item.FechaInicio != null) ? item.FechaInicio : DateTime.MinValue;
                                            // ws.Cells[row, 8].Value = (item.FechaFin != null) ? item.FechaFin : DateTime.MinValue;
                                            //print
                                       
                                        }
                                    }
                                }
                                else
                                {
                                    TransferenciaInformacionBaseModel modelTransferenciaInfoBase = new TransferenciaInformacionBaseModel();
                                    modelTransferenciaInfoBase.Entidad = (new TransferenciaInformacionBaseAppServicio()).GetTransferenciaInfoBaseByCodigo(item.EmprCodi, PeriCodi, Version, sCodigo);
                                    if (modelTransferenciaInfoBase.Entidad != null)
                                    {
                                    
                                        TransferenciaInformacionBaseDetalleModel modelTransferenciaInfoBaseDetalle = new TransferenciaInformacionBaseDetalleModel();
                                        modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle = (new TransferenciaInformacionBaseAppServicio()).BuscarTransferenciaInformacionBaseDetalle(item.EmprCodi, PeriCodi, sCodigo, Version);
                                        int fila = 3;
                                        decimal sum = 0;
                                        decimal sumIni = 0;
                                        decimal sumFin = 0;
                                        if (item.FechaInicio.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaFin.ToString("yyyyMM") != modelPeriodo.Entidad.PeriAnioMes.ToString())
                                        {
                                            for (int i = 0; i < item.FechaInicio.Day - 1; i++)
                                            {
                                                sum += modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle[i].TinfbDeSumaDia;
                                            }
                                            if (sum != 0)
                                            {
                                                ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                                ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                                ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                                ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                                ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                                row++;
                                                //ws.Cells[row, 7].Value = (item.FechaInicio != null) ? item.FechaInicio : DateTime.MinValue;
                                                //print
                                               
                                            }
                                        }
                                        else if (item.FechaFin.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaInicio.ToString("yyyyMM") != modelPeriodo.Entidad.PeriAnioMes.ToString())
                                        {
                                            for (int i = item.FechaFin.Day; i < modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle.Count; i++)
                                            {
                                                sum += modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle[i].TinfbDeSumaDia;
                                            }
                                            if (sum != 0)
                                            {
                                                ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                                ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                                ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                                ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                                ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                                row++;
                                                //ws.Cells[row, 7].Value = (item.FechaFin != null) ? item.FechaFin : DateTime.MinValue;
                                                //print
                                               
                                            }
                                        }
                                        else if (item.FechaInicio.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString() && item.FechaFin.ToString("yyyyMM") == modelPeriodo.Entidad.PeriAnioMes.ToString())
                                        {
                                            for (int i = 0; i < item.FechaInicio.Day - 1; i++)
                                            {
                                                sumIni += modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle[i].TinfbDeSumaDia;
                                            }
                                            for (int i = item.FechaFin.Day; i < modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle.Count; i++)
                                            {
                                                sumFin += modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle[i].TinfbDeSumaDia;
                                            }
                                            if (sumIni != 0 || sumFin != 0)
                                            {
                                                ws.Cells[row, 2].Value = (item.Empresa != null) ? item.Empresa.ToString() : string.Empty;
                                                ws.Cells[row, 3].Value = (item.Barra != null) ? item.Barra : string.Empty;
                                                ws.Cells[row, 4].Value = (item.Codigo != null) ? item.Codigo : string.Empty;
                                                ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Codigo : string.Empty;
                                                ws.Cells[row, 6].Value = (item.Cliente != null) ? item.Cliente : string.Empty;
                                                row++;
                                                //ws.Cells[row, 7].Value = (item.FechaInicio != null) ? item.FechaInicio : DateTime.MinValue;
                                                //ws.Cells[row, 8].Value = (item.FechaFin != null) ? item.FechaFin : DateTime.MaxValue;
                                                //print
                                               
                                            }
                                        }

                                    }  // fin  if  nullmodelTransferenciaInfoBase.Entidad != null
                                }  // fin  if else modelTransferenciaRetiro.Entidad != null

                            } // fin  if else modelTransferenciaEntrega.Entidad != null

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
        public virtual ActionResult AbrirInfoEntregadaFueraDeFecha()
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteInfoFaltanteEntregaFueradeFechaExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcel"].ToString() + "_" + Funcion.NombreReporteInfoFaltanteEntregaFueradeFechaExcel);
        }
    }
}
