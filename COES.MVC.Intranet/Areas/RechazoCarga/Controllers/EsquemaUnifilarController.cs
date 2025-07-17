using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.RechazoCarga;

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
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.Framework.Base.Core;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class EsquemaUnifilarController : BaseController
    {
        //
        // GET: /RechazoCarga/EsquemaUnifilar/

        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();

        private const int codUsuarioLibre = 4;
        private const string estadoRegistroNoEliminado = "1";
        private const string estadoRegistroEmpresaActivo = "A";
        private const int familiaEquipo = 45;
        private const int OrigenIntranet = 1;
        private const string nombreReporteDescarga = "ReporteDiagramaUnifilar.xlsx";
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EsquemaUnifilarController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EsquemaUnifilarController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EsquemaUnifilarController", ex);
                throw;
            }
        }
        public EsquemaUnifilarController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ListarEsquemaUnifilar(string empresa, string codigoSuministro, string fecIni, string fecFin, int nroPagina, int nroRegistros)
        {
            EsquemaUnifilarModel model = new EsquemaUnifilarModel();

            int regIni = 0;
            int regFin = 0;

            regIni = (nroPagina - 1) * nroRegistros + 1;
            regFin = nroPagina * nroRegistros;

            model.ListEsquemaUnifilar = servicio.ListarEsquemaUnifilarFiltro(empresa, codigoSuministro, fecIni, fecFin, OrigenIntranet, regIni, regFin);
            return PartialView("Lista", model);
        }

        public JsonResult ObtenerListaEmpresas(string empresa)
        {
            List<SiEmpresaDTO> listaEmpresas = this.servicio.ListaEmpresasRechazoCarga(empresa, codUsuarioLibre, estadoRegistroEmpresaActivo);
            
            return Json(listaEmpresas);
        }

        [HttpPost]
        public PartialViewResult ListarEsquemaUnifilarHistorial(int emprcodi, int equicodi)
        {
            EsquemaUnifilarModel model = new EsquemaUnifilarModel();
            model.ListEsquemaUnifilar = servicio.ListarEsquemaUnifilarHistorial(emprcodi, equicodi);

            return PartialView("ListaEsquemaUnifilarHistorial", model);
        }

        public ActionResult EliminarEsquemaUnifilar(int rccarecodi)
        {
            this.servicio.DeleteRcaEsquemaUnifilar(rccarecodi);

            return Json(new { success = true, message = "Ok" });
        }
        public JsonResult ObtenerEsquemaUnifilar(int rccarecodi)
        {
            RcaEsquemaUnifilarDTO oRcaEsquemaUnifilarDTO = new RcaEsquemaUnifilarDTO();//servicio.GetDataByIdHoraOperacion(pecacodi, hopcodi);
            oRcaEsquemaUnifilarDTO = servicio.ObtenerEsquemaUnifilarlPorCodigo(rccarecodi);
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oRcaEsquemaUnifilarDTO));
        }

        public ActionResult GuardarEsquemaUnifilar(int rcesqucodi,int emprcodi, int equicodi, string documento, 
            string rcesqufecharecepcion, string estado, string archivo, bool EsNuevo, string archivoDatos)
        {
            RcaEsquemaUnifilarDTO rcaEsquemaUnifilarDTO = new RcaEsquemaUnifilarDTO();

            rcaEsquemaUnifilarDTO.Emprcodi = emprcodi;
            rcaEsquemaUnifilarDTO.Equicodi = equicodi;
            rcaEsquemaUnifilarDTO.Rcesqudocumento = documento;
            var fechaRecepcion = DateTime.ParseExact(rcesqufecharecepcion, "dd/MM/yyyy", null);
            rcaEsquemaUnifilarDTO.Rcesqufecharecepcion = new DateTime(fechaRecepcion.Year, fechaRecepcion.Month, fechaRecepcion.Day
                , DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);   

            //rcaEsquemaUnifilarDTO.Rcesqufecharecepcion = DateTime.ParseExact(rcesqufecharecepcion, "dd/MM/yyyy", null);
            rcaEsquemaUnifilarDTO.Rcesquestado = estado;
            rcaEsquemaUnifilarDTO.Rcesqunombarchivo = archivo;
            rcaEsquemaUnifilarDTO.Rcesquestregistro = estadoRegistroNoEliminado;
            rcaEsquemaUnifilarDTO.Rcesquusucreacion = this.UserName;
            rcaEsquemaUnifilarDTO.Rcesqufeccreacion = DateTime.Now;
            rcaEsquemaUnifilarDTO.Rcesquusumodificacion = this.UserName;
            rcaEsquemaUnifilarDTO.Rcesqufecmodificacion = DateTime.Now;
            rcaEsquemaUnifilarDTO.Rcesquorigen = OrigenIntranet;
            rcaEsquemaUnifilarDTO.Rcesqunombarchivodatos = archivoDatos;

            if (EsNuevo)
            {
                this.servicio.SaveRcaEsquemaUnifilar(rcaEsquemaUnifilarDTO);
            }
            else
            {
                rcaEsquemaUnifilarDTO.Rcesqucodi = rcesqucodi;
                this.servicio.UpdateRcaEsquemaUnifilar(rcaEsquemaUnifilarDTO);
            }
            

            return Json(new { success = true, message = "Ok" });
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
                log.Error("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult EditEsquemaUnifilar()
        {
            EditEsquemaUnifilarModel model = new EditEsquemaUnifilarModel();
                       

            return View("EditEsquemaUnifilar", model);
        }

        [HttpPost]
        public PartialViewResult ListarEmpresas(string empresa)
        {
            EsquemaUnifilarModel model = new EsquemaUnifilarModel();
            model.ListSiEmpresa = this.servicio.ListaEmpresasRechazoCarga(empresa, codUsuarioLibre, estadoRegistroEmpresaActivo);
            return PartialView("ListarEmpresas", model);
        }
        public JsonResult ObtenerListaPuntoMedicion(int codigoEmpresa)
        {
            List<EqEquipoDTO> listaPuntoMedicion = this.servicio.ObtenerEquiposPorFamilia(codigoEmpresa, familiaEquipo);

            return Json(listaPuntoMedicion);
        }

        //Nuevos Metodos 10/02/2021
        public JsonResult GenerarReporte(string empresa, string codigoSuministro, string fecIni, string fecFin)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel(empresa, codigoSuministro, fecIni, fecFin);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoExcel(string empresa, string codigoSuministro, string fecIni, string fecFin)
        {

            var preNombre = "Diagrama_Unifilar";

            List<RcaEsquemaUnifilarDTO> listReporteInformacion = servicio.ListarEsquemaUnifilarExcel(empresa, codigoSuministro, fecIni, fecFin, OrigenIntranet);


            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var nombreHoja = "REPORTE";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                var contFila = 3;

                ws.Cells[2, 1].Value = "RAZON SOCIAL";
                ws.Cells[2, 2].Value = "SUB ESTACION";
                ws.Cells[2, 3].Value = "NOMBRE PUNTO MEDICION";                
                ws.Cells[2, 4].Value = "DIAGRAMA UNIFILAR";
                ws.Cells[2, 5].Value = "FECHA DE PRESENTACION";
                ws.Cells[2, 6].Value = "ESTADO";                

                ExcelRange rg1 = ws.Cells[2, 1, 2, 6];
                ObtenerEstiloCelda(rg1, 1);

                foreach (var registro in listReporteInformacion)
                {

                    ws.Cells[contFila, 1].Value = registro.Emprrazsocial;
                    ws.Cells[contFila, 2].Value = registro.Areanomb;
                    ws.Cells[contFila, 3].Value = registro.Equinomb;                    
                    ws.Cells[contFila, 4].Value = registro.Rcesqudocumento;
                    ws.Cells[contFila, 5].Value = ((DateTime)registro.Rcesqufecharecepcion).ToString("dd/MM/yyyy");
                    ws.Cells[contFila, 6].Value = registro.Rcesquestado.Equals("1") ? "Vigente" : "No Vigente";
                    //ws.Cells[contFila, 8].Value = "";                                                          

                    contFila++;
                }

                ws.Column(1).Width = 50;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 50;                
                ws.Column(4).Width = 40;
                ws.Column(5).Width = 40;
                ws.Column(6).Width = 20;
                      

                xlPackage.Save();
            }

            return fileName;
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
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, nombreReporteDescarga);
        }

        /// <summary>
        /// Permite generar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string empresa, string codigoSuministro, string fecIni, string fecFin, int nroRegistrosPag)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = 0;

            nroRegistros = this.servicio.ListarEsquemaUnifilarCount(empresa, codigoSuministro, fecIni, fecFin, OrigenIntranet);


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
