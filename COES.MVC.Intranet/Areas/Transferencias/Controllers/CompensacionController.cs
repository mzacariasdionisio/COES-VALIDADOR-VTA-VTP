using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class CompensacionController : Controller
    {
        // GET: /Transferencias/Compensacion/
        //[CustomAuthorize]
        public ActionResult Index(int id = 0)
        {
            if (id != 0)
            {
                Session["sPericodi"] = id;
                PeriodoModel modelPeriodo = new PeriodoModel();
                modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(id);
                TempData["Perinombre"] = modelPeriodo.Entidad.PeriNombre;
                Session["Perinombre"] = modelPeriodo.Entidad.PeriNombre;
            }
            CompensacionModel model = new CompensacionModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            int id = Convert.ToInt32(Session["sPericodi"].ToString());
            CompensacionModel model = new CompensacionModel();
            model.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones(id);
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult New()
        {
            CompensacionModel modelo = new CompensacionModel();
            modelo.Entidad = new CompensacionDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.CabeCompPeriCodi = Convert.ToInt32(Session["sPericodi"].ToString());
            modelo.Entidad.CabeCompCodi = 0;
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(modelo.Entidad.CabeCompPeriCodi);
            TempData["Periodonombre"] = modelPeriodo.Entidad.PeriNombre;
            return PartialView(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CompensacionModel modelo)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["sPericodi"].ToString());
                modelo.Entidad.CabeCompUserName = User.Identity.Name;
                modelo.IdCompensacion = (new CompensacionAppServicio()).SaveOrUpdateCompensacion(modelo.Entidad);
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //Error
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["Periodonombre"] = Session["Perinombre"].ToString();
            return PartialView(modelo);
        }

        public ActionResult Edit(int id)
        {
            CompensacionModel modelo = new CompensacionModel();
            modelo.Entidad = (new CompensacionAppServicio()).GetByIdCompensacion(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["Periodonombre"] = Session["Perinombre"].ToString();
            return PartialView(modelo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            CompensacionModel model = new CompensacionModel();
            model.IdCompensacion = (new CompensacionAppServicio()).DeleteCompensacion(id);
            return "true";
        }

        [HttpPost]
        public JsonResult GenerarExcel()
        {
            int indicador = 1;
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                int id = Convert.ToInt32(Session["sPericodi"].ToString());
                CompensacionModel model = new CompensacionModel();
                model.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones(id);

                FileInfo newFile = new FileInfo(path + Funcion.NombreListaCompensacionExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreListaCompensacionExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 4].Value = "LISTA DE INGRESOS POR COMPENSACIÓN: " + Session["Perinombre"].ToString();
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "ORDEN";
                        ws.Column(2).Style.Numberformat.Format = "#";
                        ws.Cells[5, 3].Value = "COMPENSACIÓN";
                        ws.Cells[5, 4].Value = "VISUALIZAR";
                        ws.Cells[5, 5].Value = "FECHA";
                        ws.Cells[5, 6].Value = "PERIODO";
                        ws.Cells[5, 7].Value = "RENTA";

                        rg = ws.Cells[5, 2, 5, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        int aux = 1;
                        foreach (var item in model.ListaCompensacion)
                        {
                            ws.Cells[row, 2].Value = aux;//item.Cabecompcodi.ToString(); 
                            ws.Cells[row, 3].Value = (item.CabeCompNombre != null) ? item.CabeCompNombre : string.Empty;
                            ws.Cells[row, 4].Value = (item.CabeCompVer != null) ? item.CabeCompVer.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = item.CabeCompFecIns.ToString();
                            ws.Cells[row, 6].Value = Session["Perinombre"].ToString(); //item.Cabecomppericodi.ToString(); 
                            ws.Cells[row, 7].Value = item.CabeCompRentConge.ToString();
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 7];
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
                            aux++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 8);
                        rg = ws.Cells[5, 2, row, 7];
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
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreListaCompensacionExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["Perinombre"].ToString() + "_" + Funcion.NombreListaCompensacionExcel);
        }


    }
}
