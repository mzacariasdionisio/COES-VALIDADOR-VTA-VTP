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
    public class TipoUsuarioController : Controller
    {
        // GET: /Transferencias/TipoUsuario/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            TipoUsuarioModel model = new TipoUsuarioModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(string nombre)
        {
            TipoUsuarioModel model = new TipoUsuarioModel();
            model.ListaTipoUsuario = (new TipoUsuarioAppServicio()).BuscarTipoUsuario(nombre);
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        //GET
        public ActionResult View(int id = 0)
        {
            TipoUsuarioModel model = new TipoUsuarioModel();
            model.Entidad = (new TipoUsuarioAppServicio()).GetByTipoUsuario(id);

            return PartialView(model);
        }

        //GET
        public ActionResult New()
        {
            TipoUsuarioDTO dto = new TipoUsuarioDTO();
            dto.TipoUsuaCodi = 0;

            if (dto == null)
            {
                return HttpNotFound();
            }
            dto.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return View(dto);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "Tipousuacodi,Tipousuanombre, Tipousuaestado")] TipoUsuarioDTO tipousu)
        {
            if (ModelState.IsValid)
            {
                tipousu.TipoUsuaUserName = User.Identity.Name;
                TipoUsuarioModel model = new TipoUsuarioModel();
                model.idTipoUsuario = (new TipoUsuarioAppServicio()).SaveOrUpdateTipoUsuario(tipousu);
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            return PartialView(tipousu);
        }
       
        //GET
        public ActionResult Edit(int id = 0)
        {
            TipoUsuarioDTO dto = new TipoUsuarioDTO();
            dto = (new TipoUsuarioAppServicio()).GetByTipoUsuario(id);

            if (dto == null)
            {
                return HttpNotFound();
            }
            dto.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            TipoUsuarioModel model = new TipoUsuarioModel();
            model.idTipoUsuario = (new TipoUsuarioAppServicio()).DeleteTipoUsuario(id);
            return "true";
        }

        [HttpPost]
        public JsonResult GenerarExcel()
        {
            int indicador = 1;
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                TipoUsuarioModel model = new TipoUsuarioModel();
                model.ListaTipoUsuario = (new TipoUsuarioAppServicio()).BuscarTipoUsuario("");

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteTipoUsuarioExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteTipoUsuarioExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 4].Value = "TIPOS DE USUARIO";
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "CÓDIGO";
                        ws.Column(2).Style.Numberformat.Format = "#";
                        ws.Cells[5, 3].Value = "NOMBRE";
                        ws.Cells[5, 4].Value = "FECHA";

                        rg = ws.Cells[5, 2, 5, 4];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaTipoUsuario)
                        {
                            ws.Cells[row, 2].Value = item.TipoUsuaCodi;
                            ws.Cells[row, 3].Value = (item.TipoUsuaNombre != null) ? item.TipoUsuaNombre : string.Empty;
                            ws.Cells[row, 4].Value = item.TipoUsuaFecIns.ToString();
                            rg = ws.Cells[row, 2, row, 4];
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
                        rg = ws.Cells[5, 2, row, 4];
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
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteTipoUsuarioExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteTipoUsuarioExcel);
        }        
    }
}
