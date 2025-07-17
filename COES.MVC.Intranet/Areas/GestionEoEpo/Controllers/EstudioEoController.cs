using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.GestionEoEpo.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.GestionEoEpo;
//using DocumentFormat.OpenXml.Spreadsheet;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.GestionEoEpo.Controllers
{
    public class EstudioEoController : BaseController
    {
        GestionEoEpoAppServicio _svcGestionEoEpo = new GestionEoEpoAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EstudioEoController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EstudioEoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EstudioEoController", ex);
                throw;
            }
        }

        #region Propiedades

        /// <summary>
        /// Codigo de formato
        /// </summary>
        public int IdFormato = 4;
        public int IdLectura = 51;

        public string[][] MatrizExcel
        {
            get
            {
                return (Session["MatrizExcel"] != null) ?
                    (string[][])Session["MatrizExcel"] : new string[1][];
            }
            set { Session["MatrizExcel"] = value; }
        }

        public String FileName
        {
            get
            {
                return (Session["FileName"] != null) ?
                    Session["FileName"].ToString() : null;
            }
            set { Session["FileName"] = value; }
        }

        public String NombreFile
        {
            get
            {
                return (Session["NombreArchivo"] != null) ?
                    Session["NombreArchivo"].ToString() : null;
            }
            set { Session["NombreArchivo"] = value; }
        }

        #endregion

        [ValidarSesion]
        public ActionResult Index()
        {
            #region Mejoras EO-EPO
            List<EpoEstudioEstadoDTO> listadoEstudioEstados = _svcGestionEoEpo.GetByCriteriaEpoEstudioEstados();
            ViewBag.ListadoEstudioEstados = listadoEstudioEstados.Where(x => x.Estacodi == 1 || x.Estacodi == 3 || x.Estacodi > 9).OrderBy(x => x.Estadescripcion);
            #endregion

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoZonaDTO> listadoZonaProyecto = _svcGestionEoEpo.ListarZona();
            ViewBag.ListadoZonaProyecto = listadoZonaProyecto;

            List<EpoPuntoConexionDTO> listadoPuntoConexion = _svcGestionEoEpo.ListarPuntoConexion();
            ViewBag.ListadoPuntoConexion = listadoPuntoConexion;


            return View();
        }

        [HttpPost]
        public ActionResult Listado(EpoEstudioEoDTO estudioepo)
        {
            List<EpoEstudioEoDTO> listadoEstudioEpo = new List<EpoEstudioEoDTO>();

            try
            {
                listadoEstudioEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEos(estudioepo);
            }
            catch (Exception ex)
            {
                log.Error("ListadoEstudioEo", ex);
            }

            return PartialView(listadoEstudioEpo);
        }

        [HttpPost]
        public ActionResult ListadoEpo(EpoEstudioEpoDTO estudioepo)
        {
            List<EpoEstudioEpoDTO> listadoEstudioEpo = new List<EpoEstudioEpoDTO>();

            try
            {
                listadoEstudioEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEpos(estudioepo);
            }
            catch (Exception ex)
            {
                log.Error("ListadoPorcentajes", ex);
            }

            return PartialView(listadoEstudioEpo);
        }

        public ActionResult ConsultaWeb()
        {
            #region Mejoras EO-EPO
            List<EpoEstudioEstadoDTO> listadoEstudioEstados = _svcGestionEoEpo.GetByCriteriaEpoEstudioEstados();
            ViewBag.ListadoEstudioEstados = listadoEstudioEstados.Where(x => x.Estacodi == 1 || x.Estacodi == 3 || x.Estacodi > 9).OrderBy(x=>x.Estadescripcion);
            #endregion

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoZonaDTO> listadoZonaProyecto = _svcGestionEoEpo.ListarZona();
            ViewBag.ListadoZonaProyecto = listadoZonaProyecto;

            List<EpoPuntoConexionDTO> listadoPuntoConexion = _svcGestionEoEpo.ListarPuntoConexion();
            ViewBag.ListadoPuntoConexion = listadoPuntoConexion;


            return View();
        }

        [HttpPost]
        public ActionResult ListadoWeb(EpoEstudioEoDTO estudioeo)
        {
            List<EpoEstudioEoDTO> listadoEstudioEo = new List<EpoEstudioEoDTO>();

            try
            {
                listadoEstudioEo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEos(estudioeo);
            }
            catch (Exception ex)
            {
                log.Error("ListaWebEo", ex);
            }

            return PartialView(listadoEstudioEo);
        }

        [HttpPost]
        public ActionResult ListadoWebEpo(EpoEstudioEpoDTO estudioepo)
        {
            List<EpoEstudioEpoDTO> listadoEstudioEpo = new List<EpoEstudioEpoDTO>();

            try
            {
                listadoEstudioEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEpos(estudioepo);
            }
            catch (Exception ex)
            {
                log.Error("ListadoWebEpo", ex);
            }

            return PartialView(listadoEstudioEpo);
        }

        [HttpPost]
        public JsonResult ExportarListadoWeb(EpoEstudioEoDTO estudioeo)
        {
            string nombreArchivo = "WebEO_" + DateTime.Now.ToString("yyyyMMddHHmm");
            string rutaArchivo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + nombreArchivo + ".xlsx";
            string PathLogo = @"Content\Images\logocoes.png";
            string rutaLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;

            estudioeo.nroFilas = 10000;
            List<EpoEstudioEoDTO> listadoEstudioEo = new List<EpoEstudioEoDTO>();

            try
            {
                listadoEstudioEo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEos(estudioeo);
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = null;
                    ws = package.Workbook.Worksheets.Add("Consulta_Web_EO");
                    ws.View.ShowGridLines = true;
                    FormatoEoEpoHelper.AddImage(ws, 0, 0, rutaLogo);

                    ws.Cells[2, 3].Value = "Reporte de Estudios de Operatividad";
                    ws.Cells[4, 1].Value = "Código de Proyecto";
                    ws.Cells[4, 2].Value = "Código de Estudio";
                    ws.Cells[4, 3].Value = "Nombre del Estudio";
                    ws.Cells[4, 4].Value = "Fecha de Presentación";
                    ws.Cells[4, 5].Value = "Fecha de Conformidad";
                    ws.Cells[4, 6].Value = "Estado";
                    ws.Cells[4, 7].Value = "Vigencia";
                    ws.Cells[4, 8].Value = "Punto de Conexión";
                    ws.Cells[4, 9].Value = "Zona de Proyecto";                  
                    ws.Cells[4, 10].Value = "Año de puesta de servicio";
                    ws.Cells[4, 11].Value = "Gestor del Proyecto";
                    ws.Cells[4, 12].Value = "Tercero Involucrado";
                    ws.Cells[4, 13].Value = "Comentarios";

                    System.Drawing.Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                    ws.Cells["C2:E2"].Style.Font.Bold = true;
                    ws.Cells["A4:M4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells["A4:M4"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    ws.Cells["A4:M4"].Style.Font.Bold = true;
                    ws.Cells["A4:M4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    ws.Cells["A4:M4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A4:M4"].Style.Fill.BackgroundColor.SetColor(colFromHex);

                    ws.Column(1).Width = 20;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 50;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 50;
                    ws.Column(9).Width = 50;
                    ws.Column(10).Width = 25;
                    ws.Column(11).Width = 50;
                    ws.Column(12).Width = 80;
                    ws.Column(13).Width = 80;

                    List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();

                    int fila = 5;
                    foreach (EpoEstudioEoDTO item in listadoEstudioEo)
                    {
                        string sTercerInvolucrado = "";

                        List<EpoEstudioTerceroInvEoDTO> listadoTerceroInvEo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEo(item.Esteocodi);

                        if (listadoTerceroInvEo.Count > 0)
                        {
                            List<int> idsTerceroInvEo = listadoTerceroInvEo.Select(t => t.Esteoemprcodi).ToList();
                            List<string> empresas = listadoEmpresa.Where(e => idsTerceroInvEo.Contains(e.Emprcodi)).Select(e => e.Emprnomb).ToList();

                            sTercerInvolucrado = string.Join(",", empresas);
                        }

                        ws.Cells[fila, 1].Value = item.Esteocodiproy;
                        ws.Cells[fila, 2].Value = item.Esteocodiusu;
                        ws.Cells[fila, 3].Value = item.Esteonomb;
                        ws.Cells[fila, 4].Value = item.Esteofechaini.HasValue ? item.Esteofechaini.Value.ToString("dd/MM/yyyy") : "";
                        ws.Cells[fila, 5].Value = item.Esteofechafin.HasValue ? item.Esteofechafin.Value.ToString("dd/MM/yyyy") : "";
                        ws.Cells[fila, 6].Value = item.Estadescripcion;
                        ws.Cells[fila, 7].Value = item.EsteoVigencia;
                        ws.Cells[fila, 8].Value = item.Esteopuntoconexion;
                        ws.Cells[fila, 9].Value = item.ZonDescripcion;                        
                        ws.Cells[fila, 10].Value = item.Esteoanospuestaservicio;
                        ws.Cells[fila, 11].Value = item.Emprnomb;
                        ws.Cells[fila, 12].Value = sTercerInvolucrado;
                        ws.Cells[fila, 13].Value = item.Esteoobs;

                        fila++;
                    }

                    System.IO.File.WriteAllBytes(rutaArchivo, package.GetAsByteArray());
                }               
            }
            catch (Exception ex)
            {
                log.Error("ExportarListadoWebEo", ex);
            }

            return Json(nombreArchivo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExportarListadoWebEpo(EpoEstudioEpoDTO estudioepo)
        {
            string nombreArchivoEpo = "WebEPO_" + DateTime.Now.ToString("yyyyMMddHHmm");
            string rutaArchivoEpo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + nombreArchivoEpo + ".xlsx";
            string PathLogo = @"Content\Images\logocoes.png";
            string rutaLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;

            estudioepo.nroFilas = 10000;
            List<EpoEstudioEpoDTO> listadoEstudioEpo = new List<EpoEstudioEpoDTO>();

            try
            {
                listadoEstudioEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEpos(estudioepo);

                using (ExcelPackage packageEpo = new ExcelPackage())
                {
                    ExcelWorksheet ws = null;
                    ws = packageEpo.Workbook.Worksheets.Add("Consulta_Web_EPO");
                    ws.View.ShowGridLines = true;
                    FormatoEoEpoHelper.AddImage(ws, 0, 0, rutaLogo);

                    ws.Cells[2, 3].Value = "Reporte de Estudios de Pre Operatividad";

                    ws.Cells[4, 1].Value = "Código de Proyecto";
                    ws.Cells[4, 2].Value = "Código de Estudio";
                    ws.Cells[4, 3].Value = "Nombre del Estudio";
                    ws.Cells[4, 4].Value = "Fecha de Presentación";
                    ws.Cells[4, 5].Value = "Fecha de Conformidad";
                    ws.Cells[4, 6].Value = "Estado";
                    ws.Cells[4, 7].Value = "Vigencia";
                    ws.Cells[4, 8].Value = "Punto de Conexión";
                    ws.Cells[4, 9].Value = "Zona de Proyecto";                   
                    ws.Cells[4, 10].Value = "Año de puesta de servicio";
                    ws.Cells[4, 11].Value = "Gestor del Proyecto";
                    ws.Cells[4, 12].Value = "Tercero Involucrado";
                    ws.Cells[4, 13].Value = "Comentarios";

                    System.Drawing.Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                    ws.Cells["C2:E2"].Style.Font.Bold = true;
                    ws.Cells["A4:M4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells["A4:M4"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    ws.Cells["A4:M4"].Style.Font.Bold = true;
                    ws.Cells["A4:M4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    ws.Cells["A4:M4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //ws.Cells["A4:L4"].Style.WrapText = true;
                    ws.Cells["A4:M4"].Style.Fill.BackgroundColor.SetColor(colFromHex);

                    ws.Column(1).Width = 20;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 50;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 50;
                    ws.Column(9).Width = 50;
                    ws.Column(10).Width = 25;
                    ws.Column(11).Width = 50;
                    ws.Column(12).Width = 80;
                    ws.Column(13).Width = 80;

                    List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();

                    int fila = 5;
                    foreach (EpoEstudioEpoDTO item in listadoEstudioEpo)
                    {
                        string sTercerInvolucrado = "";

                        List<EpoEstudioTerceroInvEpoDTO> listadoTerceroInvEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEpo(item.Estepocodi);

                        if (listadoTerceroInvEpo.Count > 0)
                        {
                            List<int> idsTerceroInvEo = listadoTerceroInvEpo.Select(t => t.Estepoemprcodi).ToList();
                            List<string> empresas = listadoEmpresa.Where(e => idsTerceroInvEo.Contains(e.Emprcodi)).Select(e => e.Emprnomb).ToList();

                            sTercerInvolucrado = string.Join(",", empresas);
                        }

                        ws.Cells[fila, 1].Value = item.Estepocodiproy;
                        ws.Cells[fila, 2].Value = item.Estepocodiusu;
                        ws.Cells[fila, 3].Value = item.Esteponomb;
                        ws.Cells[fila, 4].Value = item.Estepofechaini.HasValue ? item.Estepofechaini.Value.ToString("dd/MM/yyyy") : "";
                        ws.Cells[fila, 5].Value = item.Estepofechafin.HasValue ? item.Estepofechafin.Value.ToString("dd/MM/yyyy") : "";
                        ws.Cells[fila, 6].Value = item.Estadescripcion;
                        ws.Cells[fila, 7].Value = item.EstepoVigencia; //Vigencia
                        ws.Cells[fila, 8].Value = item.Estepopuntoconexion;
                        ws.Cells[fila, 9].Value = item.ZonDescripcion; //Zona de Proyecto                       
                        ws.Cells[fila, 10].Value = item.Estepoanospuestaservicio;
                        ws.Cells[fila, 11].Value = item.Emprnomb;
                        ws.Cells[fila, 12].Value = sTercerInvolucrado;
                        ws.Cells[fila, 13].Value = item.Estepoobs;

                        fila++;
                    }

                    System.IO.File.WriteAllBytes(rutaArchivoEpo, packageEpo.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                log.Error("ExportarListadoWebEpo", ex);
            }

            return Json(nombreArchivoEpo, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult Descargar(string nombreArchivo)
        {
            string[] archivo = nombreArchivo.Split('|');
            string nombre = string.Empty;
            if (Convert.ToInt32(archivo[1]) == 1)
                nombre = "Consulta_Web_EO.xlsx";
            else
                nombre = "Consulta_Web_EPO.xlsx";
            string rutaArchivo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + archivo[0] + ".xlsx";
            var FileBytesArray = System.IO.File.ReadAllBytes(rutaArchivo);

            System.IO.File.Delete(rutaArchivo);

            return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre);
        }

        public ActionResult VerDetalle(int id)
        {
            EpoEstudioEoDTO estudioeo = _svcGestionEoEpo.GetByIdEpoEstudioEo(id);
            ViewBag.ListadoRevision = _svcGestionEoEpo.GetByCriteriaEpoRevisionEos(id);

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoEstudioTerceroInvEoDTO> listadoTerceroInvEo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEo(id);

            if (listadoTerceroInvEo.Count > 0)
            {
                List<int> idsTerceroInvEo = listadoTerceroInvEo.Select(t => t.Esteoemprcodi).ToList();
                List<string> empresas = listadoEmpresa.Where(e => idsTerceroInvEo.Contains(e.Emprcodi)).Select(e => e.Emprnomb).ToList();

                ViewBag.TerceroInvolucrado = string.Join(",", empresas);
            }

            return View(estudioeo);
        }

        public ActionResult VerDetalleEpo(int id)
        {
            EpoEstudioEpoDTO estudioepo = _svcGestionEoEpo.GetByIdEpoEstudioEpo(id);
            ViewBag.ListadoRevision = _svcGestionEoEpo.GetByCriteriaEpoRevisionEpos(id);

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoEstudioTerceroInvEpoDTO> listadoTerceroInvEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEpo(id);

            if (listadoTerceroInvEpo.Count > 0)
            {
                List<int> idsTerceroInvEo = listadoTerceroInvEpo.Select(t => t.Estepoemprcodi).ToList();
                List<string> empresas = listadoEmpresa.Where(e => idsTerceroInvEo.Contains(e.Emprcodi)).Select(e => e.Emprnomb).ToList();

                ViewBag.TerceroInvolucrado = string.Join(",", empresas);
            }

            return View(estudioepo);
        }

        [HttpPost]
        public PartialViewResult Paginado(EpoEstudioEoDTO estudioepo)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = _svcGestionEoEpo.ObtenerNroRegistroBusquedaEpoEstudioEos(estudioepo);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = estudioepo.nroFilas == 0 ? Constantes.PageSize : estudioepo.nroFilas;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            string[] cantidadRegistros = { "20", "30", "50", "100" };

            model.CantidadRegistros = cantidadRegistros;

            return base.Paginado(model);
        }

        [HttpPost]
        public PartialViewResult PaginadoEpo(EpoEstudioEpoDTO estudioepo)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = _svcGestionEoEpo.ObtenerNroRegistroBusquedaEpoEstudioEpos(estudioepo);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = estudioepo.nroFilas == 0 ? Constantes.PageSize : estudioepo.nroFilas;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            string[] cantidadRegistros = { "20", "30", "50", "100" };

            model.CantidadRegistros = cantidadRegistros;

            return base.PaginadoEpo(model);
        }

        [ValidarSesion]
        public ActionResult RegistrarEstudioEo(int id)
        {
            EpoEstudioEoDTO estudioEo = _svcGestionEoEpo.GetByIdEpoEstudioEo(id);

            if (estudioEo == null)
            {
                estudioEo = new EpoEstudioEoDTO();
            }

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoEstudioEoDTO> listadoResponsable = _svcGestionEoEpo.ListFwUser();
            ViewBag.ListadoResponsable = listadoResponsable;

            List<EpoPuntoConexionDTO> listadoPuntoConexion = _svcGestionEoEpo.ListarPuntoConexion();
            ViewBag.ListadoPuntoConexion = listadoPuntoConexion;

            List<EpoEstudioTerceroInvEoDTO> listadoTerceroInvEo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEo(id);

            #region Mejoras EO-EPO
            List<EpoEstudioEstadoDTO> listadoEstudioEstados = _svcGestionEoEpo.GetByCriteriaEpoEstudioEstados();
            ViewBag.ListadoEstudioEstados = listadoEstudioEstados.Where(x => x.Estacodi > 9).OrderBy(x => x.Estadescripcion);
            #endregion

            ViewBag.Zona = "";
            if (estudioEo.PuntCodi != null) 
            {
                EpoZonaDTO objZona = _svcGestionEoEpo.MostrarZonaXPunto(estudioEo.PuntCodi.Value);
                ViewBag.Zona = "";
                if (objZona != null)
                {
                    ViewBag.Zona = objZona.ZonDescripcion;
                }
            }


            if (listadoTerceroInvEo.Count > 0)
            {
                estudioEo.Esteoterinvcodi = listadoTerceroInvEo.Select(t => t.Esteoemprcodi).ToList();
            }

            if (estudioEo.Esteofechaini.HasValue)
            {
                if (estudioEo.Esteofechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteofechaini = DateTime.ParseExact(estudioEo.Esteofechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Esteofechafin.HasValue)
            {
                if (estudioEo.Esteofechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteofechafin = DateTime.ParseExact(estudioEo.Esteofechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Esteoalcancefechaini.HasValue)
            {
                if (estudioEo.Esteoalcancefechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteoalcancefechaini = DateTime.ParseExact(estudioEo.Esteoalcancefechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Esteoalcancefechafin.HasValue)
            {
                if (estudioEo.Esteoalcancefechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteoalcancefechafin = DateTime.ParseExact(estudioEo.Esteoalcancefechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Esteoverifechaini.HasValue)
            {
                if (estudioEo.Esteoverifechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteoverifechaini = DateTime.ParseExact(estudioEo.Esteoverifechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Esteoverifechafin.HasValue)
            {
                if (estudioEo.Esteoverifechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteoverifechafin = DateTime.ParseExact(estudioEo.Esteoverifechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Esteofechaconexion.HasValue)
            {
                if (estudioEo.Esteofechaconexion.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteofechaconexion = DateTime.ParseExact(estudioEo.Esteofechaconexion.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Esteofechaopecomercial.HasValue)
            {
                if (estudioEo.Esteofechaopecomercial.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteofechaopecomercial = DateTime.ParseExact(estudioEo.Esteofechaopecomercial.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Esteofechaintegracion.HasValue)
            {
                if (estudioEo.Esteofechaintegracion.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Esteofechaintegracion = DateTime.ParseExact(estudioEo.Esteofechaintegracion.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.Lastdate.HasValue)
            {
                if (estudioEo.Lastdate.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.Lastdate = DateTime.ParseExact(estudioEo.Lastdate.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEo.EsteoAbsFFin.HasValue)
            {
                if (estudioEo.EsteoAbsFFin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEo.EsteoAbsFFin = DateTime.ParseExact(estudioEo.EsteoAbsFFin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }




            EpoConfiguraDTO configura;
            if (id == 0)
            {
                configura = _svcGestionEoEpo.GetByIdEpoConfigura(2);
                estudioEo.TipoConfig = 2;
            }                
            else

                configura = _svcGestionEoEpo.GetByIdEpoConfigura(estudioEo.TipoConfig);

            List<EpoConfiguraDTO> listadoConfigura = _svcGestionEoEpo.ListEpoConfiguras();
            ViewBag.ListadoConfigura = listadoConfigura;


            if (configura != null)
            {
                estudioEo.Esteoplazorevcoesporv = configura.Confplazorevcoesporv;
                estudioEo.Esteoplazorevcoesvenc = configura.Confplazorevcoesvenc;
                estudioEo.Esteoplazolevobsporv = configura.Confplazolevobsporv;
                estudioEo.Esteoplazolevobsvenc = configura.Confplazolevobsvenc;
                estudioEo.Esteoplazoalcancesvenc = configura.Confplazoalcancesvenc;
                estudioEo.Esteoplazoverificacionvenc = configura.Confplazoverificacionvenc;
                estudioEo.Esteoplazorevterinvporv = configura.Confplazorevterceroinvporv;
                estudioEo.Esteoplazorevterinvvenc = configura.Confplazorevterceroinvvenc;
                estudioEo.Esteoplazoenvestterinvporv = configura.Confplazoenvestterceroinvporv;
                estudioEo.Esteoplazoenvestterinvvenc = configura.Confplazoenvestterceroinvvenc;
                estudioEo.Esteoplazoverificacionvencabs = configura.Confplazoverificacionvencabs;

            }            

            ViewBag.FormatoFechaFull = Constantes.FormatoFechaFull;

            return PartialView(estudioEo);
        }


        public JsonResult ListaConfiguraciones()
        {
            Object DataConfi = null;
            List<EpoConfiguraDTO> listadoConfigura = _svcGestionEoEpo.ListEpoConfiguras();
            DataConfi = listadoConfigura;
            return Json(new { DataConfi }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarEstudioEo(EpoEstudioEoDTO estudioeo)
        {
            try
            {
                estudioeo.Lastuser = (string)Session["Usuario"];
                estudioeo.Lastdate = DateTime.Now;

                #region Mejoras EO-EPO
                if (estudioeo.Esteofechaini.HasValue)
                {
                    if (!estudioeo.Esteofechafin.HasValue)
                    {
                        if(estudioeo.Estacodi != 10 && estudioeo.Estacodi != 11)
                            estudioeo.Estacodi = 1;
                        // no se pone la fecha final (EN REVISION)

                    }
                    else if (estudioeo.Esteofechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001")
                    {
                        estudioeo.Estacodi = 1;
                    }
                    else
                    {
                        // si se pone la fecha final (APROBADO)
                        estudioeo.Estacodi = 3;
                    }
                }
                else { estudioeo.Estacodi = 1; }
                #endregion

                if (estudioeo.Esteofechaini.HasValue) {
                    if (estudioeo.Esteofechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001") {
                        estudioeo.Esteofechaini = DateTime.ParseExact(estudioeo.Esteofechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Esteofechafin.HasValue) {
                    if (estudioeo.Esteofechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001") {
                        estudioeo.Esteofechafin = DateTime.ParseExact(estudioeo.Esteofechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Esteoalcancefechaini.HasValue) {
                    if (estudioeo.Esteoalcancefechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.Esteoalcancefechaini = DateTime.ParseExact(estudioeo.Esteoalcancefechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Esteoalcancefechafin.HasValue) {
                    if (estudioeo.Esteoalcancefechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.Esteoalcancefechafin = DateTime.ParseExact(estudioeo.Esteoalcancefechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Esteoverifechaini.HasValue) {
                    if (estudioeo.Esteoverifechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.Esteoverifechaini = DateTime.ParseExact(estudioeo.Esteoverifechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Esteoverifechafin.HasValue)
                {
                    if (estudioeo.Esteoverifechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.Esteoverifechafin = DateTime.ParseExact(estudioeo.Esteoverifechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Esteofechaconexion.HasValue)
                {
                    if (estudioeo.Esteofechaconexion.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.Esteofechaconexion = DateTime.ParseExact(estudioeo.Esteofechaconexion.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Esteofechaopecomercial.HasValue)
                {
                    if (estudioeo.Esteofechaopecomercial.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.Esteofechaopecomercial = DateTime.ParseExact(estudioeo.Esteofechaopecomercial.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Esteofechaintegracion.HasValue)
                {
                    if (estudioeo.Esteofechaintegracion.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.Esteofechaintegracion = DateTime.ParseExact(estudioeo.Esteofechaintegracion.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.Lastdate.HasValue)
                {
                    if (estudioeo.Lastdate.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.Lastdate = DateTime.ParseExact(estudioeo.Lastdate.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioeo.EsteoAbsFFin.HasValue)
                {
                    if (estudioeo.EsteoAbsFFin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioeo.EsteoAbsFFin = DateTime.ParseExact(estudioeo.EsteoAbsFFin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }



                int estudioEoCodi = 0;
                if (estudioeo.Esteocodi == 0)
                {
                    estudioEoCodi = _svcGestionEoEpo.SaveEpoEstudioEo(estudioeo);
                }
                else
                {
                    estudioEoCodi = estudioeo.Esteocodi;
                    _svcGestionEoEpo.UpdateEpoEstudioEo(estudioeo);
                }

                

                _svcGestionEoEpo.DeleteEpoEstudioTercerInvEo(estudioEoCodi);
                if (estudioeo.Esteoterinvcodi != null)
                {
                    if (estudioeo.Esteoterinvcodi.Count > 0)
                    {
                        foreach (int id in estudioeo.Esteoterinvcodi)
                        {
                            EpoEstudioTerceroInvEoDTO estudioTerceroInvEo = new EpoEstudioTerceroInvEoDTO();
                            estudioTerceroInvEo.Esteocodi = estudioEoCodi;
                            estudioTerceroInvEo.Esteoemprcodi = id;
                            estudioTerceroInvEo.Lastdate = new DateTime();
                            estudioTerceroInvEo.Lastuser = "";

                            _svcGestionEoEpo.SaveEpoEstudioTercerInvEo(estudioTerceroInvEo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("RegistrarEstudioEO", ex);
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Anular(int Esteocodi)
        {
            string strMsg = string.Empty;
            bool bResult = true;

            try
            {
                EpoEstudioEoDTO _estudioEO = _svcGestionEoEpo.GetByIdEpoEstudioEo(Esteocodi);

                _estudioEO.Lastuser = (string)Session["Usuario"];
                _estudioEO.Lastdate = DateTime.Now;

                _estudioEO.Estacodi = 5; // Anulado;

                _svcGestionEoEpo.UpdateEpoEstudioEo(_estudioEO);
            }
            catch (Exception ex)
            {
                bResult = false;
                strMsg = ex.Message.ToString();
                log.Error("AnularEO", ex);
                //throw;
            }

            var rpta = new
            {
                sMensaje = strMsg,
                bResult = bResult

            };
            return Json(rpta);
        }

        [ValidarSesion]
        public ActionResult EstablecerNoVigencia(int Esteocodi)
        {

            EpoEstudioEoDTO epoEstudioEoDTO = _svcGestionEoEpo.GetByIdEpoEstudioEo(Esteocodi);

            List<EpoEstudioTerceroInvEoDTO> listadoTerceroInvEo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEo(Esteocodi);
            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();

            if (listadoTerceroInvEo.Count > 0)
            {
                List<int> idsTerceroInvEo = listadoTerceroInvEo.Select(t => t.Esteoemprcodi).ToList();
                List<string> empresas = listadoEmpresa.Where(e => idsTerceroInvEo.Contains(e.Emprcodi)).Select(e => e.Emprnomb).ToList();

                ViewBag.TerceroInvolucrado = string.Join(",", empresas);
            }

            return View(epoEstudioEoDTO);
        }

        [HttpPost]
        public JsonResult EstablecerNoVigencia(EpoEstudioEoDTO estudio)
        {
            string strMsg = string.Empty;
            bool bResult = true;

            try
            {
                EpoEstudioEoDTO _estudioEO = _svcGestionEoEpo.GetByIdEpoEstudioEo(estudio.Esteocodi);

                _estudioEO.Lastuser = (string)Session["Usuario"];
                _estudioEO.Lastdate = DateTime.Now;
                _estudioEO.Esteojustificacion = estudio.Esteojustificacion;
                _estudioEO.Estacodi = 4; // Anulado;

                _svcGestionEoEpo.UpdateEpoEstudioEo(_estudioEO);
            }
            catch (Exception ex)
            {
                bResult = false;
                strMsg = ex.Message.ToString();
                log.Error("EstablecerNoVigenciaEO", ex);
            }

            var rpta = new
            {
                sMensaje = strMsg,
                bResult = bResult

            };
            return Json(rpta);
        }

        [ValidarSesion]
        public ActionResult Revision(int id)
        {
            EpoEstudioEoDTO estudioEo = _svcGestionEoEpo.GetByIdEpoEstudioEo(id);
            List<EpoRevisionEoDTO> listadoRevisionEo = _svcGestionEoEpo.GetByCriteriaEpoRevisionEos(id);

            ViewBag.ListadoRevision = listadoRevisionEo;

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoEstudioTerceroInvEoDTO> listadoTerceroInvEo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEo(id);

            if (listadoTerceroInvEo.Count > 0)
            {
                List<int> idsTerceroInvEo = listadoTerceroInvEo.Select(t => t.Esteoemprcodi).ToList();
                List<string> empresas = listadoEmpresa.Where(e => idsTerceroInvEo.Contains(e.Emprcodi)).Select(e => e.Emprnomb).ToList();

                ViewBag.TerceroInvolucrado = string.Join(",", empresas);
            }

            return View(estudioEo);
        }

        public JsonResult VerificarRevisionesEstudioEO(int eocodi)
        {
            bool b = true;
            string sMensaje = string.Empty;
            string TipoPopUp = string.Empty;

            int NroRevisions = 0;

            List<EpoRevisionEoDTO> ListadoRevision = _svcGestionEoEpo.GetByCriteriaEpoRevisionEos(eocodi);

            NroRevisions = ListadoRevision.Count;

            string nroRevisiones = ConfigurationManager.AppSettings[DatosSesion.NroRevisiones];
            int iNroRevisiones = string.IsNullOrEmpty(nroRevisiones) ? 4 : Convert.ToInt32(nroRevisiones);

            if (NroRevisions == iNroRevisiones)
            {
                sMensaje = "Actualmente el estudio cuenta con " + iNroRevisiones + " revisiones. ¿Desea el estudio a estado No Concluido?";
            }
            else
            {
                if (NroRevisions > 0)
                {
                    int _id = (from c in ListadoRevision select c).Max(x => x.Reveocodi);
                    EpoRevisionEoDTO e = ListadoRevision.FirstOrDefault(x => x.Reveocodi == _id);

                    b = this.VerificarTermino(e, ref sMensaje);

                    TipoPopUp = "X";

                }
            }

            return Json(sMensaje);
        }

        private bool VerificarTermino(EpoRevisionEoDTO e, ref string strMensaje)
        {

            //REVEPOCOESFECHAFIN(RevisionCOESFechaFin),
            //REVEPOENVESTTERCINVINVFECHAFIN(EnvioEstudioTerceroInvolucradoFechaFin),
            //REVEPOREVTERINVFECHAFIN(RevisionTerceroInvolucradoFechaFin),
            //REVEPOLEVOBSFECHAFIN(LevantamientoObservacionFechaFin)

            //"01/01/0001"

            if (e.Reveocoesfechafin.HasValue)
            {
                if (e.Reveocoesfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001" || e.Reveocoesfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001")
                {
                    strMensaje = "el registro último de revisión, no tiene la fecha fin de Revisión y Conformidad (COES)";
                    return false;
                }
            }
            else
            {
                strMensaje = "el registro último de revisión, no tiene la fecha fin de Revisión y Conformidad (COES)";
                return false;
            }

            if (e.Reveoenvesttercinvfechaini.HasValue && e.Reveoenvesttercinvinvfechafin.HasValue)
            {
                if ((e.Reveoenvesttercinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001" || e.Reveoenvesttercinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001") && (e.Reveoenvesttercinvinvfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001" || e.Reveoenvesttercinvinvfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001"))//Se agregró la validación de fecha inicio en caso no exista Tercero Involucrado
                {
                    strMensaje = "el registro último de revisión, no tiene la fecha fin de Envío de Estudio Tercero Involucrado (COES)";
                    return false;
                }
            }
            else if (e.Reveoenvesttercinvfechaini.HasValue && !e.Reveoenvesttercinvinvfechafin.HasValue)
            {
                strMensaje = "el registro último de revisión, no tiene la fecha fin de Envío de Estudio Tercero Involucrado (COES)";
                return false;
            }


            if (e.Reveorevterinvfechaini.HasValue && e.Reveorevterinvfechafin.HasValue)
            {
                if ((e.Reveorevterinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001" || e.Reveorevterinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001") && (e.Reveorevterinvfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001" || e.Reveorevterinvfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001")) //Se agregró la validación de fecha inicio en caso no exista Tercero Involucrado
                {
                    strMensaje = "el registro último de revisión, no tiene la fecha fin del Revisión de Estudio Tercero Involucrado";
                    return false;
                }
            }
            else if (e.Reveorevterinvfechaini.HasValue && !e.Reveorevterinvfechafin.HasValue)
            {
                strMensaje = "el registro último de revisión, no tiene la fecha fin del Revisión de Estudio Tercero Involucrado";
                return false;
            }


            if (e.Reveolevobsfechafin.HasValue)
            {
                if (e.Reveolevobsfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001" || e.Reveolevobsfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001")
                {
                    strMensaje = "el registro último de revisión, no tiene la fecha fin del Absolución de Observaciones (Gestor del Proyecto)";
                    return false;
                }
            }
            else
            {
                strMensaje = "el registro último de revisión, no tiene la fecha fin del Absolución de Observaciones (Gestor del Proyecto)";
                return false;
            }

            return true;
        }


        [HttpPost]
        public JsonResult EliminarRevisionEO(int revcodi)
        {
            try
            {
                _svcGestionEoEpo.DeleteEpoRevisionEo(revcodi);
                return Json("1");
            }
            catch (Exception ex)
            {
                log.Error("EliminarRevisionEO", ex);
                return Json("0");
            }
        }

        [ValidarSesion]
        public ActionResult NuevaRevision(int eocodi, int id, int TipoConfig)
        {
            ViewBag.Esteocodi = eocodi;
            ViewBag.Reveocodi = id;
            ViewBag.EstepoTipoConfig = TipoConfig;
            return View();
        }

        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int revcodi)
        {
            int indicador = 0;
            int idEnvio = 0;
            try
            {
                FormatoModel model = BuildHojaExcel(revcodi, 0,0);
                FormatoEoEpoHelper.GenerarFileExcel(model);
                indicador = 1;
            }

            catch (Exception ex)
            {
                log.Error("GenerarFormatoEO", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteGestionEoEpo] + "Revision.xlsx";
            return File(fullPath, Constantes.AppExcel, "Revision.xlsx");
        }

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            MeArchivoDTO archivo = new MeArchivoDTO();
            MeEnvioDTO envio = new MeEnvioDTO();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.FileName = fileRandom + ".xlsx";
                    string fileName = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.FileName;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lee datos desde excel
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult LeerFileUpExcel(int revcodi)
        {
            //MeFormatoDTO formato = logic.GetByIdMeFormato(this.IdFormato);
            //formato.FechaProceso = fechaProceso;
            //var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            //var listaPtos = this.logic.GetByCriteria2MeHojaptomeds(idEmpresa, this.IdFormato, cabercera.Cabquery);
            //int nCol = listaPtos.Count;
            //int horizonte = formato.Formathorizonte;
            //FormatoDemandaHelper.GetSizeFormato2(formato);
            //int nBloques = formato.RowPorDia * formato.Formathorizonte;
            //this.MatrizExcel = FormatoEoEpoHelper.InicializaMatrizExcel(cabercera.Cabfilas, nBloques, cabercera.Cabcolumnas, nCol);
            Boolean isValido = FormatoEoEpoHelper.LeerExcelFile(this.MatrizExcel, this.NombreFile, 0, 0, 0, 0);

            return Json(isValido ? 1 : -1);
        }


        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuyera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        protected bool ValidarPlazo(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        protected bool ValidarFecha(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                var regfechaPlazo = this.logic.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            return true;
            //return resultado;
        }

        public static double GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays = 1 + ((endD - startD).TotalDays * 5 - (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;
            if ((int)endD.DayOfWeek == 6) calcBusinessDays--;
            if ((int)startD.DayOfWeek == 0) calcBusinessDays--;

            return calcBusinessDays;
        }


        /// <summary>
        /// Graba los datos del archivo Excel Web
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string dataExcel, int epocodi, int revcodi, int tipoconfig)
        {
            try
            {
                EpoRevisionEoDTO revision = _svcGestionEoEpo.GetByIdEpoRevisionEo(revcodi);
                bool EsNuevo = false;

                if (revision == null)
                {
                    revision = new EpoRevisionEoDTO();
                    EsNuevo = true;
                }

                revision.Esteocodi = epocodi;
              
                if (!string.IsNullOrEmpty(dataExcel.Split(',')[3]))
                {
                    DateTime strReveolevobsfechaini = DateTime.ParseExact(dataExcel.Split(',')[3], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrReveolevobsfechaini = strReveolevobsfechaini.ToString(Constantes.FormatoFecha);
                }

                revision.Reveolevobstit = dataExcel.Split(',')[5];
                revision.Reveolevobsenl = dataExcel.Split(',')[7];
                revision.Reveolevobsobs = dataExcel.Split(',')[9];

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[11]))
                {
                    revision.Reveolevobsfinalizado = "1";
                    DateTime strReveolevobsfechafin = DateTime.ParseExact(dataExcel.Split(',')[11], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrReveolevobsfechafin = strReveolevobsfechafin.ToString(Constantes.FormatoFecha);
                }
                else
                {
                    revision.Reveolevobsfinalizado = "0";
                }

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[13]))
                {
                    revision.Reveopreampl = Convert.ToInt32(dataExcel.Split(',')[13]);
                }
                else
                {
                    revision.Reveopreampl = 0;
                }

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[39]))
                {
                    revision.Reveorevcoesampl = Convert.ToInt32(dataExcel.Split(',')[39]);
                }
                else
                {
                    revision.Reveorevcoesampl = 0;
                }

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[17]))
                {
                    DateTime strReveoenvesttercinvfechaini = DateTime.ParseExact(dataExcel.Split(',')[17], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrReveoenvesttercinvfechaini = strReveoenvesttercinvfechaini.ToString(Constantes.FormatoFecha);
                }

                revision.Reveoenvesttercinvtit = dataExcel.Split(',')[19];
                revision.Reveoenvesttercinvenl = dataExcel.Split(',')[21];
                revision.Reveoenvesttercinvobs = dataExcel.Split(',')[23];

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[25]))
                {
                    revision.Reveoenvesttercinvfinalizado = "1";
                    DateTime strReveoenvesttercinvinvfechafin = DateTime.ParseExact(dataExcel.Split(',')[25], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrReveoenvesttercinvinvfechafin = strReveoenvesttercinvinvfechafin.ToString(Constantes.FormatoFecha);
                }
                else
                {
                    revision.Reveoenvesttercinvfinalizado = "0";
                }

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[29]))
                {
                    DateTime strReveorevterinvfechaini = DateTime.ParseExact(dataExcel.Split(',')[29], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrReveorevterinvfechaini = strReveorevterinvfechaini.ToString(Constantes.FormatoFecha);
                }

                revision.Reveorevterinvtit = dataExcel.Split(',')[31];
                revision.Reveorevterinvenl = dataExcel.Split(',')[33];
                revision.Reveorevterinvobs = dataExcel.Split(',')[35];

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[37]))
                {
                    revision.Reveorevterinvfinalizado = "1";
                    DateTime strReveorevterinvfechafin = DateTime.ParseExact(dataExcel.Split(',')[37], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrReveorevterinvfechafin = strReveorevterinvfechafin.ToString(Constantes.FormatoFecha);
                }
                else
                {
                    revision.Reveorevterinvfinalizado = "0";
                }

                if (tipoconfig == 1)
                {
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[39])) //Ampliación tercer involucrado
                    {
                        revision.Reveorevterinvampl = Convert.ToInt32(dataExcel.Split(',')[39]);
                    }
                    else
                    {
                        revision.Reveorevterinvampl = 0;
                    }
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[43]))
                    {
                        DateTime strReveorevcoesfechaini = DateTime.ParseExact(dataExcel.Split(',')[43], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        revision.StrReveorevcoesfechaini = strReveorevcoesfechaini.ToString(Constantes.FormatoFecha);
                    }
                    revision.Reveorevcoescartarevisiontit = dataExcel.Split(',')[45];
                    revision.Reveorevcoescartarevisionenl = dataExcel.Split(',')[47];
                    revision.Reveorevcoescartarevisionobs = dataExcel.Split(',')[49];
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[51]))
                    {
                        DateTime strReveocoesfechafin = DateTime.ParseExact(dataExcel.Split(',')[51], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        revision.StrReveocoesfechafin = strReveocoesfechafin.ToString(Constantes.FormatoFecha);
                    }
                    else
                    {
                        revision.Reveorevcoesfinalizado = "0";
                    }
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[53]))
                    {
                        revision.Reveorevcoesampl = Convert.ToInt32(dataExcel.Split(',')[53]);
                    }
                    else
                    {
                        revision.Reveorevcoesampl = 0;
                    }
                }
                else if (tipoconfig == 2)
                {
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[41]))
                    {
                        DateTime strReveorevcoesfechaini = DateTime.ParseExact(dataExcel.Split(',')[41], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        revision.StrReveorevcoesfechaini = strReveorevcoesfechaini.ToString(Constantes.FormatoFecha);
                    }
                    revision.Reveorevcoescartarevisiontit = dataExcel.Split(',')[43];
                    revision.Reveorevcoescartarevisionenl = dataExcel.Split(',')[45];
                    revision.Reveorevcoescartarevisionobs = dataExcel.Split(',')[47];
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[49]))
                    {
                        DateTime strReveocoesfechafin = DateTime.ParseExact(dataExcel.Split(',')[49], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        revision.StrReveocoesfechafin = strReveocoesfechafin.ToString(Constantes.FormatoFecha);
                    }
                    else
                    {
                        revision.Reveorevcoesfinalizado = "0";
                    }
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[51]))
                    {
                        revision.Reveorevcoesampl = Convert.ToInt32(dataExcel.Split(',')[51]);
                    }
                    else
                    {
                        revision.Reveorevcoesampl = 0;
                    }
                }
                
                revision.Lastuser = (string)Session["Usuario"];

                DateTime fechaActual = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                revision.Lastdate = fechaActual;


                if (EsNuevo)
                {
                    _svcGestionEoEpo.SaveEpoRevisionEo(revision);
                }
                else
                {
                    _svcGestionEoEpo.UpdateEpoRevisionEo(revision);
                }


                return Json("1");
            }
            catch (Exception ex)
            {
                log.Error("GrabarRevisionEO", ex);
                return Json("0");
            }
        }

        /// <summary>
        /// Muestra El formato Excel en la Web 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="desEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarHojaExcelWeb(int revcodi, int cargaformato, int tipoconfig)
        {
            //List<MeFormatoDTO> entitys = this.logic.GetByModuloLecturaMeFormatos(Modulos.AppMedidoresDistribucion, this.IdLectura, idEmpresa);

            //if (entitys.Count > 0)
            //{
            FormatoModel jsModel = BuildHojaExcel(revcodi, cargaformato, tipoconfig);
            Session["DatosJSON"] = jsModel.Handson.ListaExcelData;
            return Json(jsModel);
            //}
            //else
            //{
            //    Session["DatosJSON"] = null;
            //    return Json(-1);
            //}
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DescargarDatos()
        {
            string[][] list = (string[][])Session["DatosJSON"];

            var data = list;
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(data);
            result.ContentType = "application/json";

            return result;
        }

        /// <summary>
        /// Carga principal de la pantalla
        /// </summary>
        /// <returns></returns>
        //public ActionResult Index()
        //{
        //    FormatoModel model = new FormatoModel();
        //    model.IdModulo = Modulos.AppMedidoresDistribucion;
        //    model.ListaEmpresas = this.logic.ObtenerEmpresaPorFormato(this.IdFormato);

        //    if (model.ListaEmpresas.Count == 1)
        //    {
        //        model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
        //    }

        //    model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
        //    return View(model);
        //}


        /// <summary>
        ///Devuelve el model necesario para mostrar en la web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcel(int revcodi, int cargaformato, int tipoConfig)
        {
            EpoRevisionEoDTO revision = _svcGestionEoEpo.GetByIdEpoRevisionEo(revcodi);

            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = new MeFormatoDTO(); //logic.GetByIdMeFormato(this.IdFormato);
            //this.Formato = model.Formato;

            //var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            model.Formato.Formatcols = 2;
            model.Formato.Formatrows = 2;

            List<string> labels = new List<string>();
            labels.Add("Presentación EO / Absolución de Observaciones,Presentación EO / Absolución de Observaciones,1");
            labels.Add("Fecha Inicio,Fecha Inicio,0");
            labels.Add("Titulo Carta de Revisión,Titulo Carta de Revisión,0");
            labels.Add("Enlace,Enlace,0");
            labels.Add("Observación,Observación,0");
            labels.Add("Fecha Fin,Fecha Fin,0");
            labels.Add("Ampliación,Ampliación,0");

            labels.Add("Envío del Estudio al Tercero Involucrado,Envío del Estudio al Tercero Involucrado,1");
            labels.Add("Fecha Inicio,Fecha Inicio,0");
            labels.Add("Título,Título,0");
            labels.Add("Enlace,Enlace,0");
            labels.Add("Observación,Observación,0");
            labels.Add("Fecha Fin,Fecha Fin,0");

            labels.Add("Revisión del Tercero Involucrado,Revisión del Tercero Involucrado,1");
            labels.Add("Fecha Inicio,Fecha Inicio,0");
            labels.Add("Título,Título,0");
            labels.Add("Enlace,Enlace,0");
            labels.Add("Observación,Observación,0");
            labels.Add("Fecha Fin,Fecha Fin,0");//18
            if (tipoConfig == 1)
                labels.Add("Ampliación,Ampliación,0");

            labels.Add("Revisión COES,Revisión COES,1");
            labels.Add("Fecha Inicio,Fecha Inicio,0");
            labels.Add("Título,Título,0");
            labels.Add("Enlace,Enlace,0");
            labels.Add("Observación,Observación,0");
            labels.Add("Fecha Fin,Fecha Fin,0");
            labels.Add("Ampliación,Ampliación,0");
            model.Formato.Formatheaderrow = string.Join("#", labels);

            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = labels.Count;

            int idCfgFormato = 0;


            //model.ListaHojaPto = this.logic.GetByCriteria2MeHojaptomeds(idEmpresa, this.IdFormato, cabercera.Cabquery);
            var cabecerasRow = model.Formato.Formatheaderrow.Split(QueryParametros.SeparadorFila);
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }

            model.Formato.FechaProceso = new DateTime(); //EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            //FormatoDemandaHelper.GetSizeFormato2(model.Formato);
            model.ListaEnvios = new List<MeEnvioDTO>(); //this.logic.GetByCriteriaMeEnvios(idEmpresa, this.IdFormato, model.Formato.FechaInicio);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar

            /// Verifica si Formato esta en Plaz0
            string mensaje = string.Empty;
            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real

            model.EnPlazo = ValidarPlazo(model.Formato);
            model.Handson.ReadOnly = false;

            model.Dia = model.Formato.FechaInicio.Day.ToString();
            model.Handson.Width = HandsonConstantes.ColWidth * 20;
            //Genera La vista html complementaria a la grilla Handson, nombre de formato, area coes, fecha de formato, etc.
            //model.ViewHtml = FormatoDemandaHelper.GenerarFormatoHtml(model, idEnvio, model.EnPlazo);

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
            int nCol = 2;
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;
            model.Handson.ListaFilaReadOnly = new List<bool>();
            model.ListaCambios = new List<CeldaCambios>();

            if (cargaformato == 0)
            {
                model.Handson.ListaExcelData = FormatoEoEpoHelper.InicializaMatrizExcel(model.FilasCabecera, model.FilasCabecera, 1, 1);
            }
            else
            {
                model.Handson.ListaExcelData = this.MatrizExcel;
            }

            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();

            model.Handson.ListaColWidth.Add(200);
            model.Handson.ListaColWidth.Add(200);

            for (var w = 0; w < model.FilasCabecera; w++)
            {


                model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                if (listaCabeceraRow[w].IsMerge == 1)
                {
                    cellMerge = new CeldaMerge();
                    cellMerge.col = 0;
                    cellMerge.row = w;
                    cellMerge.colspan = 2;
                    cellMerge.rowspan = 1;
                    model.Handson.ListaMerge.Add(cellMerge);
                }
                else
                {
                    if (cargaformato == 0)
                    {
                        var valor = "";

                        if (revision != null && tipoConfig == 1)
                        {
                            switch (w)
                            {
                                case 1:
                                    valor = revision.Reveolevobsfechaini.HasValue ? revision.Reveolevobsfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 2:
                                    valor = revision.Reveolevobstit;
                                    break;
                                case 3:
                                    valor = revision.Reveolevobsenl;
                                    break;
                                case 4:
                                    valor = revision.Reveolevobsobs;
                                    break;
                                case 5:
                                    valor = revision.Reveolevobsfechafin.HasValue ? revision.Reveolevobsfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 6:
                                    valor = revision.Reveopreampl.ToString();
                                    break;

                                case 8:
                                    valor = revision.Reveoenvesttercinvfechaini.HasValue ? revision.Reveoenvesttercinvfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 9:
                                    valor = revision.Reveoenvesttercinvtit;
                                    break;
                                case 10:
                                    valor = revision.Reveoenvesttercinvenl;
                                    break;
                                case 11:
                                    valor = revision.Reveoenvesttercinvobs;
                                    break;
                                case 12:
                                    valor = revision.Reveoenvesttercinvinvfechafin.HasValue ? revision.Reveoenvesttercinvinvfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 14:
                                    valor = revision.Reveorevterinvfechaini.HasValue ? revision.Reveorevterinvfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 15:
                                    valor = revision.Reveorevterinvtit;
                                    break;
                                case 16:
                                    valor = revision.Reveorevterinvenl;
                                    break;
                                case 17:
                                    valor = revision.Reveorevterinvobs;
                                    break;
                                case 18:
                                    valor = revision.Reveorevterinvfechafin.HasValue ? revision.Reveorevterinvfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 19:
                                    valor = revision.Reveorevterinvampl.ToString(); 
                                    break;
                                case 21:
                                    valor = revision.Reveorevcoesfechaini.HasValue ? revision.Reveorevcoesfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 22:
                                    valor = revision.Reveorevcoescartarevisiontit;
                                    break;
                                case 23:
                                    valor = revision.Reveorevcoescartarevisionenl;
                                    break;
                                case 24:
                                    valor = revision.Reveorevcoescartarevisionobs;
                                    break;
                                case 25:
                                    valor = revision.Reveocoesfechafin.HasValue ? revision.Reveocoesfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 26:
                                    valor = revision.Reveorevcoesampl.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (revision != null && tipoConfig == 2)
                        {
                            switch (w)
                            {
                                case 1:
                                    valor = revision.Reveolevobsfechaini.HasValue ? revision.Reveolevobsfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 2:
                                    valor = revision.Reveolevobstit;
                                    break;
                                case 3:
                                    valor = revision.Reveolevobsenl;
                                    break;
                                case 4:
                                    valor = revision.Reveolevobsobs;
                                    break;
                                case 5:
                                    valor = revision.Reveolevobsfechafin.HasValue ? revision.Reveolevobsfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 6:
                                    valor = revision.Reveopreampl.ToString();
                                    break;
                                case 8:
                                    valor = revision.Reveoenvesttercinvfechaini.HasValue ? revision.Reveoenvesttercinvfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 9:
                                    valor = revision.Reveoenvesttercinvtit;
                                    break;
                                case 10:
                                    valor = revision.Reveoenvesttercinvenl;
                                    break;
                                case 11:
                                    valor = revision.Reveoenvesttercinvobs;
                                    break;
                                case 12:
                                    valor = revision.Reveoenvesttercinvinvfechafin.HasValue ? revision.Reveoenvesttercinvinvfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 14:
                                    valor = revision.Reveorevterinvfechaini.HasValue ? revision.Reveorevterinvfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 15:
                                    valor = revision.Reveorevterinvtit;
                                    break;
                                case 16:
                                    valor = revision.Reveorevterinvenl;
                                    break;
                                case 17:
                                    valor = revision.Reveorevterinvobs;
                                    break;
                                case 18:
                                    valor = revision.Reveorevterinvfechafin.HasValue ? revision.Reveorevterinvfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 20:
                                    valor = revision.Reveorevcoesfechaini.HasValue ? revision.Reveorevcoesfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 21:
                                    valor = revision.Reveorevcoescartarevisiontit;
                                    break;
                                case 22:
                                    valor = revision.Reveorevcoescartarevisionenl;
                                    break;
                                case 23:
                                    valor = revision.Reveorevcoescartarevisionobs;
                                    break;
                                case 24:
                                    valor = revision.Reveocoesfechafin.HasValue ? revision.Reveocoesfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 25:
                                    valor = revision.Reveorevcoesampl.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (valor != null)
                            model.Handson.ListaExcelData[w][1] = valor.ToString();
                        else
                            model.Handson.ListaExcelData[w][1] = string.Empty;
                    }

                }
            }

            this.MatrizExcel = model.Handson.ListaExcelData;

            return model;
        }

        public JsonResult MostrarZonaXPunto(int PuntCodi)
        {
            EpoZonaDTO objZona = new EpoZonaDTO();
            try
            {
                objZona = _svcGestionEoEpo.MostrarZonaXPunto(PuntCodi);
            }
            catch (Exception ex)
            {
                log.Error("MostrarZonaXPunto", ex);
            }
            return Json(objZona.ZonDescripcion);
        }

    }
}

