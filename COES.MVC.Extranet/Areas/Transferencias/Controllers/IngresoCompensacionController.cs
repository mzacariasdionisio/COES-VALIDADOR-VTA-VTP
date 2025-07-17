using COES.MVC.Extranet.Helper;
using COES.Dominio.DTO.Transferencias;
//using COES.MVC.Intranet.Models;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.MVC.Extranet.Areas.Transferencias.Helper;
using COES.Servicios.Aplicacion.Transferencias;
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

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class IngresoCompensacionController : Controller
    {
        // GET: /Transferencias/IngresoCompensacion/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodo = (new PeriodoAppServicio()).ListarByEstadoPublicarCerrado();
            TempData["Pericodigo1"] = new SelectList(modelPeriodo.ListaPeriodo, "Pericodi", "Perinombre");
            return View();
        }

        /// <summary>
        /// Permite cargar versiones deacuerdo al periodo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculos = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            return Json(modelRecalculo.ListaRecalculos);
        }

        [HttpPost]
        public ActionResult Lista(int? pericodi, int? version)
        {
            IngresoCompensacionModel model = new IngresoCompensacionModel();
            model.ListaIngresoEmpresa = (new IngresoCompensacionAppServicio()).BuscarListaEmpresas((int)pericodi, (int)version);
            TempData["tdListaIngresoEmpresa"] = model.ListaIngresoEmpresa;
            model.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones((int)pericodi);
            TempData["tdListaCompensacion"] = model.ListaCompensacion;
            model.ListaIngresoCompensacion = (new IngresoCompensacionAppServicio()).ListByPeriodoVersion((int)pericodi, (int)version);
            TempData["tdListaIngresoCompensacion"] = model.ListaIngresoCompensacion;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcel(int iPericodi, int iVersion)
        {
            int iResultado = 1;
            //Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(iPericodi);
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                IngresoCompensacionModel model = new IngresoCompensacionModel();
                if (TempData["tdListaIngresoEmpresa"] != null)
                    model.ListaIngresoEmpresa = (List<IngresoCompensacionDTO>)TempData["tdListaIngresoEmpresa"];
                else
                    model.ListaIngresoEmpresa = (new IngresoCompensacionAppServicio()).BuscarListaEmpresas(iPeriCodi, iVersion);
                if (TempData["tdListaCompensacion"] != null)
                    model.ListaCompensacion = (List<CompensacionDTO>)TempData["tdListaCompensacion"];
                else
                    model.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones(iPeriCodi);
                if (TempData["tdListaIngresoCompensacion"] != null)
                    model.ListaIngresoCompensacion = (List<IngresoCompensacionDTO>)TempData["tdListaIngresoCompensacion"];
                else
                    model.ListaIngresoCompensacion = (new IngresoCompensacionAppServicio()).ListByPeriodoVersion(iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteIngresoCompensacionExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteIngresoCompensacionExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE INGRESOS POR COMPENSACIÓN: " + modelPeri.Entidad.PeriNombre + " - " + modelPeri.Entidad.RecaNombre;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        //----------------------------------------------------------
                        //ws.Cells[5, 3].Value = "COMPENSACIÓN";
                        int iColumnaInicio = 3;
                        int iColumnaFinal = iColumnaInicio + model.ListaCompensacion.Count()-1; //Numero de cabeceras de compensación en el periodo
                        int iAux = iColumnaInicio;
                        foreach (var item in model.ListaCompensacion)
                        {
                            ws.Cells[5, iAux].Value = item.CabeCompNombre; //Nombre de la compensación
                            ws.Column(iAux).Style.Numberformat.Format = "#,##0.00";
                            iAux++;
                        }

                        rg = ws.Cells[5, 2, 5, iColumnaFinal];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var dtoEmpresa in model.ListaIngresoEmpresa)
                        {
                            ws.Cells[row, 2].Value = (dtoEmpresa.EmprNombre != null) ? dtoEmpresa.EmprNombre : string.Empty;
                            //Lista de compensaciones
                            iAux = iColumnaInicio;
                            foreach (var dtoCompensacion in model.ListaCompensacion)
                            {
                                foreach (var dtoIngComp in model.ListaIngresoCompensacion)
                                {
                                    if (dtoIngComp.CompCodi == dtoCompensacion.CabeCompCodi && dtoEmpresa.EmprCodi == dtoIngComp.EmprCodi)
                                    {
                                        ws.Cells[row, iAux].Value = dtoIngComp.IngrCompImporte;
                                        iAux++; 
                                    }
                                }
                            }
                            //Border por celda
                            rg = ws.Cells[row, 2, row, iColumnaFinal];
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
                        ws.View.FreezePanes(6, iColumnaFinal+1);
                        rg = ws.Cells[5, 2, row, iColumnaFinal];
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
                iResultado = 1;
            }
            catch
            {
                iResultado = -1;
            }
            return Json(iResultado);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss"); 
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteIngresoCompensacionExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteIngresoCompensacionExcel);
        }

    }
}
