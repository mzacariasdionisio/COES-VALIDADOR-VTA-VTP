using COES.MVC.Extranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Extranet.Models;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.MVC.Extranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Collections;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
//using Excel;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class RatioCumplimientoController : Controller
    {
        //
        // GET: /Transferencias/RatioCumplimiento/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            TipoEmpresaModel modelTipoEmpresa = new TipoEmpresaModel();
            modelTipoEmpresa.ListaTipoEmpresas = (new TipoEmpresaAppServicio()).ListTipoEmpresas();

            TempData["Tipoemprcodigo"] = new SelectList(modelTipoEmpresa.ListaTipoEmpresas, "Tipoemprcodi", "Tipoemprdesc");

            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodo = (new PeriodoAppServicio()).ListPeriodo();

            TempData["Pericodigo"] = new SelectList(modelPeriodo.ListaPeriodo, "Pericodi", "Perinombre");

            return View();
        }

        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculos = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            return Json(modelRecalculo.ListaRecalculos);
        }

        [HttpPost]
        public ActionResult Lista(int? tipoemprcodi, int? pericodi, int vers)
        {
            RatioCumplimientoModel model = new RatioCumplimientoModel();
            model.ListaRatioCumplimiento = (new RatioCumplimientoAppServicio()).ListRatioCumplimiento(tipoemprcodi, pericodi, vers);

            TempData["RatioCumplimiento"] = model.ListaRatioCumplimiento;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcel()
        {
            int indicador = 1;

            try
            {
                string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                Session["FechaImpresion"] = sFecha;
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                RatioCumplimientoModel model = new RatioCumplimientoModel();
                model.ListaRatioCumplimiento = (List<RatioCumplimientoDTO>)TempData["RatioCumplimiento"];

                //FileInfo template = new FileInfo(path + Funcion.NombrePlantillaRatioCumplimientoExcel);
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteRatioCumplimientoExcel);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteRatioCumplimientoExcel);
                }


                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                    if (ws != null)
                    {

                        //TITULO
                        ws.Cells[2, 4].Value = "LISTA DE RATIO DE CUMPLIMIENTO ";
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "REQUERIDO";
                        ws.Cells[5, 4].Value = "INFORMADO";
                        ws.Cells[5, 5].Value = "INFORMACION FINAL";


                        rg = ws.Cells[5, 2, 5, 5];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;


                        int row = 6;
                        foreach (var item in model.ListaRatioCumplimiento)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb : string.Empty;
                            ws.Cells[row, 3].Value = item.Requerido.ToString();
                            ws.Cells[row, 4].Value = item.Informado.ToString();
                            ws.Cells[row, 5].Value = item.Infofinal.ToString();

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
                        ws.View.FreezePanes(6, 10);
                        rg = ws.Cells[5, 2, row, 9];
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
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteRatioCumplimientoExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteRatioCumplimientoExcel);
        }
    }
}
