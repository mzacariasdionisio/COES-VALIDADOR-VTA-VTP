using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.RechazoCarga;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class CargaEsencialController : BaseController
    {
        //
        // GET: /RechazoCarga/CargaEsencial/
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();

        private const string _estadoRegistroNoEliminado = "1";
        private const string _estadoRegistroEmpresaActivo = "A";
        private const int _tipoEmpresaUsuarioLibre = 4;
        private const int _familiaEquipo = 45;
        private const int OrigenIntranet = 1;
        private const string nombreReporteDescarga = "ReporteCargaEsencial.xlsx";
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CargaEsencialController));

        public CargaEsencialController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("CargaEsencialController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("CargaEsencialController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {           
            return View();
        }
        
        public ActionResult CargaEsencial()
        {           
            base.ValidarSesionUsuario();
            CargaEsencialModel model = new CargaEsencialModel();
            
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListarCargaEsencial(string estado, string razonSocial, string documento, string cargaIni, 
            string cargaFin, string fecIni, string fecFin, int nroPagina, int nroRegistros)
        {
            CargaEsencialModel model = new CargaEsencialModel();

            int regIni = 0;
            int regFin = 0;           

            regIni = (nroPagina - 1) * nroRegistros + 1;
            regFin = nroPagina * nroRegistros;

            model.ListCargaEsencial = servicio.ListarCargaEsencialFiltro(estado, razonSocial, documento, cargaIni,
                cargaFin, fecIni, fecFin, _estadoRegistroNoEliminado, OrigenIntranet, regIni, regFin);
            var siteRoot = Url.Content("~/");
            model.urlDescarga = siteRoot + Constantes.RutaCarga;
            return PartialView("ListarCargaEsencial", model);
        }

        public JsonResult ObtenerCargaEsencial(int rccarecodi)
        {
            RcaCargaEsencialDTO oRcaCargaEsencialDTO = new RcaCargaEsencialDTO();
            oRcaCargaEsencialDTO = servicio.ObtenerCargaEsencialPorCodigo(rccarecodi);
            if (oRcaCargaEsencialDTO.Tipoemprcodi.Equals(_tipoEmpresaUsuarioLibre))
            {
                oRcaCargaEsencialDTO.EsUsuarioLibre = true;
            }
            else
            {
                oRcaCargaEsencialDTO.EsUsuarioLibre = false;
            }
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oRcaCargaEsencialDTO));
        }

        [HttpPost]
        public PartialViewResult ListarCargaEsencialHistorial(int emprcodi, int equicodi)
        {
            CargaEsencialModel model = new CargaEsencialModel();
            model.ListCargaEsencial = servicio.ListarCargaEsencialHistorial(emprcodi, equicodi, _estadoRegistroNoEliminado);

            return PartialView("ListarCargaEsencialHistorial", model);
        }

        /// <summary>
        /// Elimina una carga esencial
        /// </summary>
        /// <param name="rccarecodi"></param>
        /// <returns></returns>
        public ActionResult EliminarCargaEsencial(int rccarecodi)
        {            
            this.servicio.DeleteRcaCargaEsencial(rccarecodi);
            
            return Json(new { success = true, message = "Ok" });
        }

        [HttpGet]
        public ActionResult EditCargaEsencial()
        {
            EditCargaEsencialModel model = new EditCargaEsencialModel();           

            return View("EditCargaEsencial", model);
        }

        [HttpPost]
        public PartialViewResult ListarEmpresas(string empresa, int tipoEmpresa)
        {            
            CargaEsencialModel model = new CargaEsencialModel();
            model.ListSiEmpresa = this.servicio.ListaEmpresasRechazoCarga(empresa, tipoEmpresa, _estadoRegistroEmpresaActivo).OrderBy(p=>p.Emprrazsocial).ToList();
            return PartialView("ListarEmpresas", model);
        }

        /// <summary>
        /// Guarda o actualiza una Carga Esencial
        /// </summary>
        /// <param name="codigoCargaEsencial"></param>
        /// <param name="empresa"></param>
        /// <param name="puntoMedicion"></param>
        /// <param name="carga"></param>
        /// <param name="documento"></param>
        /// <param name="fechaRecepcion"></param>
        /// <param name="estado"></param>
        /// <param name="archivo"></param>
        /// <param name="esNuevo"></param>
        /// <returns></returns>
        public ActionResult GuardarCargaEsencial(int codigoCargaEsencial, int empresa, int puntoMedicion, decimal carga, 
            string documento, string fechaRecepcion, string estado, string archivo, bool esNuevo, int tipoCarga)
        {
            RcaCargaEsencialDTO oRcaCargaEsencialDTO = new RcaCargaEsencialDTO();

            oRcaCargaEsencialDTO.Emprcodi = empresa;
            oRcaCargaEsencialDTO.Equicodi = puntoMedicion;
            oRcaCargaEsencialDTO.Rccarecarga = carga;
            oRcaCargaEsencialDTO.Rccaredocumento = documento; 
            var rccarefecharecepcion = DateTime.ParseExact(fechaRecepcion, "dd/MM/yyyy", null);
            oRcaCargaEsencialDTO.Rccarefecharecepcion = new DateTime(rccarefecharecepcion.Year, rccarefecharecepcion.Month, rccarefecharecepcion.Day
                , DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);            

            oRcaCargaEsencialDTO.Rccareestado = estado;
            oRcaCargaEsencialDTO.Rccarenombarchivo = archivo;
            oRcaCargaEsencialDTO.Rccareestregistro = _estadoRegistroNoEliminado;
            oRcaCargaEsencialDTO.Rccareusucreacion = User.Identity.Name;
            oRcaCargaEsencialDTO.Rccarefeccreacion = DateTime.Now;
            oRcaCargaEsencialDTO.Rccareusumodificacion = User.Identity.Name;
            oRcaCargaEsencialDTO.Rccarefecmodificacion = DateTime.Now;
            oRcaCargaEsencialDTO.Rccareorigen = OrigenIntranet;
            oRcaCargaEsencialDTO.Rccaretipocarga = tipoCarga;

            if (esNuevo)
            {
                this.servicio.SaveRcaCargaEsencial(oRcaCargaEsencialDTO);
            }
            else
            {
                oRcaCargaEsencialDTO.Rccarecodi = codigoCargaEsencial;
                this.servicio.UpdateRcaCargaEsencial(oRcaCargaEsencialDTO);
            }           

            return Json(new { success = true, message = "Ok" });
        }

        /// <summary>
        /// Obtiene la lista de punto de medición, segun código de Empresa seleccionado
        /// </summary>
        /// <param name="codigoEmpresa"></param>
        /// <returns></returns>
        public JsonResult ObtenerListaPuntoMedicion(int codigoEmpresa)
        {
            List<EqEquipoDTO> listaPuntoMedicion = this.servicio.ObtenerEquiposPorFamilia(codigoEmpresa, _familiaEquipo);
            
            return Json(listaPuntoMedicion);
        }
        public ActionResult Upload(string fecha)
        {
            try
            {                
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                string extension = string.Empty;
                string nombreArchivo = string.Empty;
                string nombreArchivoFinal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(file.FileName);

                    extension = System.IO.Path.GetExtension(file.FileName);
                    nombreArchivoFinal = nombreArchivo + "_" + fecha + extension;
                    string fileName = path + nombreArchivoFinal;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    //archivo.Nombre = fileName;                    
                    file.SaveAs(fileName);
                }
               
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                
            }
            catch(Exception ex)
            {
                Log.Fatal("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        //Nuevos Metodos 10/02/2021
        public JsonResult GenerarReporte(string estado, string razonSocial, string documento, string cargaIni,
            string cargaFin, string fecIni, string fecFin)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel(estado, razonSocial, documento, cargaIni, cargaFin, fecIni, fecFin);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoExcel(string estado, string razonSocial, string documento, string cargaIni,
            string cargaFin, string fecIni, string fecFin)
        {
            
            //var preNombre = "Carga_Esencial_" + DateTime.Now.ToString("yyyyMMddhhmmss");

            const string nombreReporte= "ReporteCargaEsencial.xlsx";

            List<RcaCargaEsencialDTO> listReporteInformacion = servicio.ListarCargaEsencialExcel(estado, razonSocial, documento, cargaIni,
                cargaFin, fecIni, fecFin, _estadoRegistroNoEliminado, OrigenIntranet);


            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            //var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + nombreReporte);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + nombreReporte);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var nombreHoja = "REPORTE";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                var contFila = 3;

                ws.Cells[2, 1].Value = "RAZON SOCIAL";
                ws.Cells[2, 2].Value = "SUB ESTACION";
                ws.Cells[2, 3].Value = "NOMBRE PUNTO MEDICION";
                ws.Cells[2, 4].Value = "MW";
                ws.Cells[2, 5].Value = "DOCUMENTO";
                ws.Cells[2, 6].Value = "FECHA PRESENTACION AL COES";
                ws.Cells[2, 7].Value = "ESTADO";
                ws.Cells[2, 8].Value = "CARGA ESENCIAL";
                
                ExcelRange rg1 = ws.Cells[2, 1, 2, 8];
                ObtenerEstiloCelda(rg1, 1);

                foreach (var registro in listReporteInformacion)
                {

                    ws.Cells[contFila, 1].Value = registro.Emprrazsocial;
                    ws.Cells[contFila, 2].Value = registro.Areanomb;
                    ws.Cells[contFila, 3].Value = registro.Equinomb;
                    ws.Cells[contFila, 4].Value = registro.Rccarecarga;
                    ws.Cells[contFila, 5].Value = registro.Rccaredocumento;
                    ws.Cells[contFila, 6].Value = ((DateTime)registro.Rccarefecharecepcion).ToString("dd/MM/yyyy");
                    ws.Cells[contFila, 7].Value = registro.Rccareestado.Equals("1") ? "Vigente" : "No Vigente";
                    ws.Cells[contFila, 8].Value = registro.Rccaretipocarga.Equals(1) ? "Parcial" : "Total";

                    contFila++;
                }

                ws.Column(1).Width = 50;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 50;
                ws.Column(4).Width = 10;
                ws.Column(5).Width = 40;
                ws.Column(6).Width = 40;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;               

                xlPackage.Save();
            }

            return nombreReporte;
        }

        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                //rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;
                string colorborder = "#245C86";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato(string file)
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString() + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);            
            return File(bytes, Constantes.AppExcel, nombreReporteDescarga);
        }

      

        /// <summary>
        /// Permite generar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string estado, string razonSocial, string documento, string cargaIni,
            string cargaFin, string fecIni, string fecFin, int nroRegistrosPag)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = 0;

            nroRegistros = this.servicio.ListarCargaEsencialCount(estado, razonSocial, documento, cargaIni,
                cargaFin, fecIni, fecFin, _estadoRegistroNoEliminado, OrigenIntranet);


            if (nroRegistros > ConstantesRechazoCarga.NroPageShow)
            {
                //int pageSize = ConstantesRechazoCarga.PageSizeDemandaUsuario;
                int pageSize = nroRegistrosPag;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = ConstantesRechazoCarga.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);

        }
    }
}