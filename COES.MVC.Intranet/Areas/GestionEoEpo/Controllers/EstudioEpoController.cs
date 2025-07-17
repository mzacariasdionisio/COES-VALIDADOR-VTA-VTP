using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.DemandaBarras.Helper;
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
    public class EstudioEpoController : BaseController
    {
        GestionEoEpoAppServicio _svcGestionEoEpo = new GestionEoEpoAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EstudioEpoController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EstudioEpoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EstudioEpoController", ex);
                throw;
            }
        }
        #region Propiedades

        public string[][] MatrizExcel
        {
            get
            {
                return (Session["MatrizExcelEpo"] != null) ?
                    (string[][])Session["MatrizExcelEpo"] : new string[1][];
            }
            set { Session["MatrizExcelEpo"] = value; }
        }

        public String FileName
        {
            get
            {
                return (Session["FileNameEpo"] != null) ?
                    Session["FileNameEpo"].ToString() : null;
            }
            set { Session["FileNameEpo"] = value; }
        }

        public String NombreFile
        {
            get
            {
                return (Session["NombreArchivoEpo"] != null) ?
                    Session["NombreArchivoEpo"].ToString() : null;
            }
            set { Session["NombreArchivoEpo"] = value; }
        }

        /// <summary>
        /// Codigo de formato
        /// </summary>
        public int IdFormato = 4;
        public int IdLectura = 51;

        #endregion

        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

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


        public ActionResult ConsultaWeb()
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
        public ActionResult ListadoWeb(EpoEstudioEpoDTO estudioepo)
        {
            List<EpoEstudioEpoDTO> listadoEstudioEpo = new List<EpoEstudioEpoDTO>();

            try
            {
                listadoEstudioEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEpos(estudioepo);
            }
            catch (Exception ex)
            {
                log.Error("ListadoWeb", ex);
            }

            return PartialView(listadoEstudioEpo);
        }

        [HttpPost]
        public ActionResult ListadoWebEo(EpoEstudioEoDTO estudioeo)
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
        public JsonResult ExportarListadoWeb(EpoEstudioEpoDTO estudioepo)
        {
            string nombreArchivo = "WebEPO_" + DateTime.Now.ToString("yyyyMMddHHmm");
            string rutaArchivo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + nombreArchivo + ".xlsx";
            string PathLogo = @"Content\Images\logocoes.png";
            string rutaLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;

            estudioepo.nroFilas = 10000;
            List<EpoEstudioEpoDTO> listadoEstudioEpo = new List<EpoEstudioEpoDTO>();

            try
            {
                listadoEstudioEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEpos(estudioepo);

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = null;
                    ws = package.Workbook.Worksheets.Add("Consulta_Web_EPO");
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

                    System.IO.File.WriteAllBytes(rutaArchivo, package.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                log.Error("ListadoPorcentajes", ex);
            }

            return Json(nombreArchivo, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult Descargar(string nombreArchivo)
        {
            string[] archivo = nombreArchivo.Split('|');
            string nombre = string.Empty;
            if (Convert.ToInt32(archivo[1]) == 1)
                nombre = "Consulta_Web_EPO.xlsx";
            else
                nombre = "Consulta_Web_EO.xlsx";
            string rutaArchivo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + archivo[0] + ".xlsx";
            var FileBytesArray = System.IO.File.ReadAllBytes(rutaArchivo);

            System.IO.File.Delete(rutaArchivo);

            return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre);
        }

        public ActionResult VerDetalle(int id)
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

        public ActionResult VerDetalleEo(int id)
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

        [HttpPost]
        public ActionResult Listado(EpoEstudioEpoDTO estudioepo)
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

        [HttpPost]
        public ActionResult ListadoEo(EpoEstudioEoDTO estudioepo)
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
        public PartialViewResult Paginado(EpoEstudioEpoDTO estudioepo)
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

            return base.Paginado(model);
        }

        [HttpPost]
        public PartialViewResult PaginadoEo(EpoEstudioEoDTO estudioepo)
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

            return base.PaginadoEo(model);
        }

        [ValidarSesion]
        public ActionResult RegistrarEstudioEpo(int id)
        {
            EpoEstudioEpoDTO estudioEpo = _svcGestionEoEpo.GetByIdEpoEstudioEpo(id);

            if (estudioEpo == null)
            {
                estudioEpo = new EpoEstudioEpoDTO();
            }

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoEstudioEoDTO> listadoResponsable = _svcGestionEoEpo.ListFwUser();
            ViewBag.ListadoResponsable = listadoResponsable;

            List<EpoPuntoConexionDTO> listadoPuntoConexion = _svcGestionEoEpo.ListarPuntoConexion();
            ViewBag.ListadoPuntoConexion = listadoPuntoConexion;

            List<EpoZonaDTO> listadoZonaProyecto = _svcGestionEoEpo.ListarZona();
            ViewBag.ListadoZonaProyecto = listadoZonaProyecto;

            #region Mejoras EPO-EO

            List<EpoEstudioEstadoDTO> listadoEstudioEstados = _svcGestionEoEpo.GetByCriteriaEpoEstudioEstados();
            ViewBag.ListadoEstudioEstados = listadoEstudioEstados.Where(x => x.Estacodi > 9).OrderBy(x => x.Estadescripcion);

            List<EpoEstudioEstadoDTO> listadoVigencia = _svcGestionEoEpo.GetByCriteriaEpoEstudioEstados();
            ViewBag.listadoVigencia = listadoVigencia.Where(x => x.Estacodi == 8 || x.Estacodi == 9).OrderBy(x => x.Estadescripcion); 

            #endregion

            ViewBag.Zona = "";
            if (estudioEpo.PuntCodi != null)
            {
                EpoZonaDTO objZona = _svcGestionEoEpo.MostrarZonaXPunto(estudioEpo.PuntCodi.Value);

                if (objZona != null)
                {
                    ViewBag.Zona = objZona.ZonDescripcion;
                }

            }


            List<EpoEstudioTerceroInvEpoDTO> listadoTerceroInvEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEpo(id);

            if (listadoTerceroInvEpo.Count > 0)
            {
                estudioEpo.Estepoterinvcodi = listadoTerceroInvEpo.Select(t => t.Estepoemprcodi).ToList();
            }

            EpoConfiguraDTO configura;
            if (id == 0)
            {
                configura = _svcGestionEoEpo.GetByIdEpoConfigura(2);
                estudioEpo.TipoConfig = 2; 
            }
            else
            {
                configura = _svcGestionEoEpo.GetByIdEpoConfigura(estudioEpo.TipoConfig);                
            }
                


            List<EpoConfiguraDTO>  listadoConfigura= _svcGestionEoEpo.ListEpoConfiguras();
            ViewBag.ListadoConfigura = listadoConfigura;


            if (configura != null)
            {
                estudioEpo.Estepoplazorevcoesporv = configura.Confplazorevcoesporv;
                estudioEpo.Estepoplazorevcoesvenc = configura.Confplazorevcoesvenc;
                estudioEpo.Estepoplazolevobsporv = configura.Confplazolevobsporv;
                estudioEpo.Estepoplazolevobsvenc = configura.Confplazolevobsvenc;
                estudioEpo.Estepoplazoalcancesvenc = configura.Confplazoalcancesvenc;
                estudioEpo.Estepoplazoverificacionvenc = configura.Confplazoverificacionvenc;
                estudioEpo.Estepoplazorevterinvporv = configura.Confplazorevterceroinvporv;
                estudioEpo.Estepoplazorevterinvvenc = configura.Confplazorevterceroinvvenc;
                estudioEpo.Estepoplazoenvestterinvporv = configura.Confplazoenvestterceroinvporv;
                estudioEpo.Estepoplazoenvestterinvvenc = configura.Confplazoenvestterceroinvvenc;
                estudioEpo.Estepoplazoverificacionvencabs = configura.Confplazoverificacionvencabs;
            }
            

            ViewBag.FormatoFechaFull = Constantes.FormatoFechaFull;

            if (estudioEpo.Estepofechaini.HasValue)
            {
                if (estudioEpo.Estepofechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEpo.Estepofechaini = DateTime.ParseExact(estudioEpo.Estepofechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEpo.Estepofechafin.HasValue)
            {
                if (estudioEpo.Estepofechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEpo.Estepofechafin = DateTime.ParseExact(estudioEpo.Estepofechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEpo.Estepoalcancefechaini.HasValue)
            {
                if (estudioEpo.Estepoalcancefechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEpo.Estepoalcancefechaini = DateTime.ParseExact(estudioEpo.Estepoalcancefechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEpo.Estepoalcancefechafin.HasValue)
            {
                if (estudioEpo.Estepoalcancefechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEpo.Estepoalcancefechafin = DateTime.ParseExact(estudioEpo.Estepoalcancefechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEpo.Estepoverifechaini.HasValue)
            {
                if (estudioEpo.Estepoverifechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEpo.Estepoverifechaini = DateTime.ParseExact(estudioEpo.Estepoverifechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEpo.Estepoverifechafin.HasValue)
            {
                if (estudioEpo.Estepoverifechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEpo.Estepoverifechafin = DateTime.ParseExact(estudioEpo.Estepoverifechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }

            if (estudioEpo.Lastdate.HasValue)
            {
                if (estudioEpo.Lastdate.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEpo.Lastdate = DateTime.ParseExact(estudioEpo.Lastdate.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }


            if (estudioEpo.EstepoAbsFFin.HasValue)
            {
                if (estudioEpo.EstepoAbsFFin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    estudioEpo.EstepoAbsFFin = DateTime.ParseExact(estudioEpo.EstepoAbsFFin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                }
            }



            return PartialView(estudioEpo);
        }


        public JsonResult ListaConfiguraciones()
        {
            Object DataConfi = null;
            List<EpoConfiguraDTO> listadoConfigura = _svcGestionEoEpo.ListEpoConfiguras();
            DataConfi = listadoConfigura;
            return Json(new { DataConfi }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarEstudioEpo(EpoEstudioEpoDTO estudioepo)
        {
            try
            {
                estudioepo.Lastuser = (string)Session["Usuario"];
                estudioepo.Lastdate = DateTime.Now;

                #region Mejoras EO-EPO
                if (estudioepo.Estepofechaini.HasValue)
                {
                    if (!estudioepo.Estepofechafin.HasValue)
                    {
                        if(estudioepo.Estacodi != 10 && estudioepo.Estacodi != 11)
                            estudioepo.Estacodi = 1;
                        // no se pone la fecha final (EN REVISION)

                    }
                    else if (estudioepo.Estepofechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001")
                    {
                        // si se pone la fecha final (APROBADO)
                        estudioepo.Estacodi = 1;
                    }
                    else
                    {
                        // no se pone la fecha final (EN REVISION)
                        estudioepo.Estacodi = 3;
                    }
                }
                else { estudioepo.Estacodi = 1; }

                #endregion

                if (estudioepo.Estepofechaini.HasValue)
                {
                    if (estudioepo.Estepofechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        log.Info(estudioepo.Estepofechaini.Value);
                        estudioepo.Estepofechaini = DateTime.ParseExact(estudioepo.Estepofechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                        log.Info(estudioepo.Estepofechaini.Value);
                    }
                }

                log.Info(estudioepo.Estepofechafin);
                if (estudioepo.Estepofechafin.HasValue)
                {
                    if (estudioepo.Estepofechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        log.Info(estudioepo.Estepofechafin.Value);
                        estudioepo.Estepofechafin = DateTime.ParseExact(estudioepo.Estepofechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                        log.Info(estudioepo.Estepofechafin.Value);
                    }
                }

                if (estudioepo.Estepoalcancefechaini.HasValue)
                {
                    if (estudioepo.Estepoalcancefechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioepo.Estepoalcancefechaini = DateTime.ParseExact(estudioepo.Estepoalcancefechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioepo.Estepoalcancefechafin.HasValue)
                {
                    if (estudioepo.Estepoalcancefechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioepo.Estepoalcancefechafin = DateTime.ParseExact(estudioepo.Estepoalcancefechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioepo.Estepoverifechaini.HasValue)
                {
                    if (estudioepo.Estepoverifechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioepo.Estepoverifechaini = DateTime.ParseExact(estudioepo.Estepoverifechaini.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioepo.Estepoverifechafin.HasValue)
                {
                    if (estudioepo.Estepoverifechafin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioepo.Estepoverifechafin = DateTime.ParseExact(estudioepo.Estepoverifechafin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioepo.Lastdate.HasValue)
                {
                    if (estudioepo.Lastdate.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioepo.Lastdate = DateTime.ParseExact(estudioepo.Lastdate.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }

                if (estudioepo.EstepoAbsFFin.HasValue)
                {
                    if (estudioepo.EstepoAbsFFin.Value.ToString("dd/MM/yyyy") != "01/01/0001")
                    {
                        estudioepo.EstepoAbsFFin = DateTime.ParseExact(estudioepo.EstepoAbsFFin.Value.ToString(Constantes.FormatoFechaFull), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }
                }



                int estudioEpoCodi = 0;
                if (estudioepo.Estepocodi == 0)
                {
                    estudioEpoCodi = _svcGestionEoEpo.SaveEpoEstudioEpo(estudioepo);
                }
                else
                {
                    estudioEpoCodi = estudioepo.Estepocodi;
                    _svcGestionEoEpo.UpdateEpoEstudioEpo(estudioepo);
                }

                _svcGestionEoEpo.DeleteEpoEstudioTercerInvEpo(estudioEpoCodi);
                if (estudioepo.Estepoterinvcodi != null)
                {
                    if (estudioepo.Estepoterinvcodi.Count > 0)
                    {
                        foreach (int id in estudioepo.Estepoterinvcodi)
                        {
                            EpoEstudioTerceroInvEpoDTO estudioTerceroInvEpo = new EpoEstudioTerceroInvEpoDTO();
                            estudioTerceroInvEpo.Estepocodi = estudioEpoCodi;
                            estudioTerceroInvEpo.Estepoemprcodi = id;
                            estudioTerceroInvEpo.Lastdate = new DateTime();
                            estudioTerceroInvEpo.Lastuser = "";

                            _svcGestionEoEpo.SaveEpoEstudioTercerInvEpo(estudioTerceroInvEpo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("RegistrarEstudioEPO", ex);
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Anular(int Estepocodi)
        {
            string strMsg = string.Empty;
            bool bResult = true;

            try
            {
                EpoEstudioEpoDTO _estudioEpo = _svcGestionEoEpo.GetByIdEpoEstudioEpo(Estepocodi);

                _estudioEpo.Lastuser = (string)Session["Usuario"];
                _estudioEpo.Lastdate = DateTime.Now;

                _estudioEpo.Estacodi = 5; // Anulado;

                _svcGestionEoEpo.UpdateEpoEstudioEpo(_estudioEpo);
            }
            catch (Exception ex)
            {
                bResult = false;
                strMsg = ex.Message.ToString();
                log.Error("AnularEPO", ex);
            }

            var rpta = new
            {
                sMensaje = strMsg,
                bResult = bResult

            };
            return Json(rpta);
        }

        [ValidarSesion]
        public ActionResult EstablecerNoVigencia(int Estepocodi)
        {

            EpoEstudioEpoDTO epoEstudioEpoDTO = _svcGestionEoEpo.GetByIdEpoEstudioEpo(Estepocodi);

            List<EpoEstudioTerceroInvEpoDTO> listadoTerceroInvEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEpo(Estepocodi);
            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();

            if (listadoTerceroInvEpo.Count > 0)
            {
                List<int> idsTerceroInvEo = listadoTerceroInvEpo.Select(t => t.Estepoemprcodi).ToList();
                List<string> empresas = listadoEmpresa.Where(e => idsTerceroInvEo.Contains(e.Emprcodi)).Select(e => e.Emprnomb).ToList();

                ViewBag.TerceroInvolucrado = string.Join(",", empresas);
            }

            return View(epoEstudioEpoDTO);
        }

        [HttpPost]
        public JsonResult EstablecerNoVigencia(EpoEstudioEpoDTO estudio)
        {
            string strMsg = string.Empty;
            bool bResult = true;

            try
            {
                EpoEstudioEpoDTO _estudioEpo = _svcGestionEoEpo.GetByIdEpoEstudioEpo(estudio.Estepocodi);

                _estudioEpo.Lastuser = (string)Session["Usuario"];
                _estudioEpo.Lastdate = DateTime.Now;
                _estudioEpo.Estepojustificacion = estudio.Estepojustificacion;
                _estudioEpo.Estacodi = 4; // Anulado;

                _svcGestionEoEpo.UpdateEpoEstudioEpo(_estudioEpo);
            }
            catch (Exception ex)
            {
                bResult = false;
                strMsg = ex.Message.ToString();
                log.Error("EstablecerNoVigenciaEPO", ex);
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
            EpoEstudioEpoDTO estudioEpo = _svcGestionEoEpo.GetByIdEpoEstudioEpo(id);
            List<EpoRevisionEpoDTO> listadoRevisionEpo = _svcGestionEoEpo.GetByCriteriaEpoRevisionEpos(id);

            ViewBag.ListadoRevision = listadoRevisionEpo;

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoEstudioTerceroInvEpoDTO> listadoTerceroInvEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioTercerInvEpo(id);

            if (listadoTerceroInvEpo.Count > 0)
            {
                List<int> idsTerceroInvEpo = listadoTerceroInvEpo.Select(t => t.Estepoemprcodi).ToList();
                List<string> empresas = listadoEmpresa.Where(e => idsTerceroInvEpo.Contains(e.Emprcodi)).Select(e => e.Emprnomb).ToList();

                ViewBag.TerceroInvolucrado = string.Join(",", empresas);
            }

            return View(estudioEpo);
        }

        [ValidarSesion]
        public ActionResult NuevaRevision(int epocodi, int id, int TipoConfig)
        {
            ViewBag.Estepocodi = epocodi;
            ViewBag.Revepocodi = id;
            ViewBag.EstepoTipoConfig = TipoConfig;
            return View();
        }

        public JsonResult VerificarRevisionesEstudioEPO(int epocodi)
        {
            bool b = true;
            string sMensaje = string.Empty;
            string TipoPopUp = string.Empty;

            int NroRevisions = 0;

            List<EpoRevisionEpoDTO> ListadoRevision = _svcGestionEoEpo.GetByCriteriaEpoRevisionEpos(epocodi);

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
                    int _id = (from c in ListadoRevision select c).Max(x => x.Revepocodi);
                    EpoRevisionEpoDTO e = ListadoRevision.FirstOrDefault(x => x.Revepocodi == _id);

                    b = this.VerificarTermino(e, ref sMensaje);

                    TipoPopUp = "X";

                }
            }

            return Json(sMensaje);
        }

        private bool VerificarTermino(EpoRevisionEpoDTO e, ref string strMensaje)
        {

            //REVEPOCOESFECHAFIN(RevisionCOESFechaFin),
            //REVEPOENVESTTERCINVINVFECHAFIN(EnvioEstudioTerceroInvolucradoFechaFin),
            //REVEPOREVTERINVFECHAFIN(RevisionTerceroInvolucradoFechaFin),
            //REVEPOLEVOBSFECHAFIN(LevantamientoObservacionFechaFin)

            //"01/01/0001"

            if (e.Revepocoesfechafin.HasValue)
            {
                if (e.Revepocoesfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001" || e.Revepocoesfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001")
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

            if (e.Revepoenvesttercinvfechaini.HasValue && e.Revepoenvesttercinvinvfechafin.HasValue)
            {
                if ((e.Revepoenvesttercinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001" || e.Revepoenvesttercinvfechaini.Value.ToString("dd/MM/yyyy") != "1/01/0001") && (e.Revepoenvesttercinvinvfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001" || e.Revepoenvesttercinvinvfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001"))//Se agregró la validación de fecha inicio en caso no exista Tercero Involucrado
                {
                    strMensaje = "el registro último de revisión, no tiene la fecha fin de Envío de Estudio Tercero Involucrado (COES)";
                    return false;
                }
            }
            else if (e.Revepoenvesttercinvfechaini.HasValue && !e.Revepoenvesttercinvinvfechafin.HasValue)
            {
                strMensaje = "el registro último de revisión, no tiene la fecha fin de Envío de Estudio Tercero Involucrado (COES)";
                return false;
            }


            if (e.Reveporevterinvfechaini.HasValue && e.Reveporevterinvfechafin.HasValue)
            {
                if ((e.Reveporevterinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001" || e.Reveporevterinvfechaini.Value.ToString("dd/MM/yyyy") != "01/01/0001") && (e.Reveporevterinvfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001" || e.Reveporevterinvfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001")) //Se agregró la validación de fecha inicio en caso no exista Tercero Involucrado
                {
                    strMensaje = "el registro último de revisión, no tiene la fecha fin del Revisión de Estudio Tercero Involucrado";
                    return false;
                }
            }
            else if (e.Reveporevterinvfechaini.HasValue && !e.Reveporevterinvfechafin.HasValue)
            {
                strMensaje = "el registro último de revisión, no tiene la fecha fin del Revisión de Estudio Tercero Involucrado";
                return false;
            }


            if (e.Revepolevobsfechafin.HasValue)
            {
                if (e.Revepolevobsfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001" || e.Revepolevobsfechafin.Value.ToString("dd/MM/yyyy") == "01/01/0001")
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
        public JsonResult EliminarRevisionEPO(int revcodi)
        {
            try
            {
                _svcGestionEoEpo.DeleteEpoRevisionEpo(revcodi);
                return Json("1");
            }
            catch (Exception ex)
            {
                log.Error("EstablecerNoVigenciaEPO", ex);
                return Json("0");
            }
        }

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
                EpoRevisionEpoDTO revision = _svcGestionEoEpo.GetByIdEpoRevisionEpo(revcodi);
                bool EsNuevo = false;

                if (revision == null)
                {
                    revision = new EpoRevisionEpoDTO();
                    EsNuevo = true;
                }

                revision.Estepocodi = epocodi;

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[3]))
                {
                    DateTime strRevepolevobsfechaini = DateTime.ParseExact(dataExcel.Split(',')[3], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrRevepolevobsfechaini = strRevepolevobsfechaini.ToString(Constantes.FormatoFecha);
                }

                revision.Revepolevobstit = dataExcel.Split(',')[5];
                revision.Revepolevobsenl = dataExcel.Split(',')[7];
                revision.Revepolevobsobs = dataExcel.Split(',')[9];

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[11]))
                {
                    revision.Revepolevobsfinalizado = "1";
                    DateTime strRevepolevobsfechafin = DateTime.ParseExact(dataExcel.Split(',')[11], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrRevepolevobsfechafin = strRevepolevobsfechafin.ToString(Constantes.FormatoFecha);
                }
                else
                {
                    revision.Revepolevobsfinalizado = "0";
                }


                if (!string.IsNullOrEmpty(dataExcel.Split(',')[13]))
                {
                    revision.Revepopreampl = Convert.ToInt32(dataExcel.Split(',')[13]);
                }
                else
                {
                    revision.Revepopreampl = 0;
                }

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[17]))
                {
                    DateTime strRevepoenvesttercinvfechaini = DateTime.ParseExact(dataExcel.Split(',')[17], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrRevepoenvesttercinvfechaini = strRevepoenvesttercinvfechaini.ToString(Constantes.FormatoFecha);
                }

                revision.Revepoenvesttercinvtit = dataExcel.Split(',')[19];
                revision.Revepoenvesttercinvenl = dataExcel.Split(',')[21];
                revision.Revepoenvesttercinvobs = dataExcel.Split(',')[23];

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[25]))
                {
                    revision.Revepoenvesttercinvfinalizado = "1";
                    DateTime strRevepoenvesttercinvinvfechafin = DateTime.ParseExact(dataExcel.Split(',')[25], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrRevepoenvesttercinvinvfechafin = strRevepoenvesttercinvinvfechafin.ToString(Constantes.FormatoFecha);
                }
                else
                {
                    revision.Revepoenvesttercinvfinalizado = "0";
                }

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[29]))
                {
                    DateTime strReveporevterinvfechaini = DateTime.ParseExact(dataExcel.Split(',')[29], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrReveporevterinvfechaini = strReveporevterinvfechaini.ToString(Constantes.FormatoFecha);
                }

                revision.Reveporevterinvtit = dataExcel.Split(',')[31];
                revision.Reveporevterinvenl = dataExcel.Split(',')[33];
                revision.Reveporevterinvobs = dataExcel.Split(',')[35];

                if (!string.IsNullOrEmpty(dataExcel.Split(',')[37]))
                {
                    revision.Reveporevterinvfinalizado = "1";
                    DateTime strReveporevterinvfechafin = DateTime.ParseExact(dataExcel.Split(',')[37], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    revision.StrReveporevterinvfechafin = strReveporevterinvfechafin.ToString(Constantes.FormatoFecha);
                }
                else
                {
                    revision.Reveporevterinvfinalizado = "0";
                }

                if(tipoconfig == 1)
                {
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[39])) //Ampliación tercer involucrado
                    {
                        revision.Reveporevterinvampl = Convert.ToInt32(dataExcel.Split(',')[39]);
                    }
                    else
                    {
                        revision.Reveporevterinvampl = 0;
                    }
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[43]))
                    {
                        DateTime strReveporevcoesfechaini = DateTime.ParseExact(dataExcel.Split(',')[43], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        revision.StrReveporevcoesfechaini = strReveporevcoesfechaini.ToString(Constantes.FormatoFecha);
                    }
                    revision.Reveporevcoescartarevisiontit = dataExcel.Split(',')[45];
                    revision.Reveporevcoescartarevisionenl = dataExcel.Split(',')[47];
                    revision.Reveporevcoescartarevisionobs = dataExcel.Split(',')[49];
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[51]))
                    {
                        DateTime strRevepocoesfechafin = DateTime.ParseExact(dataExcel.Split(',')[51], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        revision.StrRevepocoesfechafin = strRevepocoesfechafin.ToString(Constantes.FormatoFecha);
                    }
                    else
                    {
                        revision.Reveporevcoesfinalizado = "0";
                    }
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[53]))
                    {
                        revision.Reveporevcoesampl = Convert.ToInt32(dataExcel.Split(',')[53]);
                    }
                    else
                    {
                        revision.Reveporevcoesampl = 0;
                    }
                }
                else if(tipoconfig == 2)
                {
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[41]))
                    {
                        DateTime strReveporevcoesfechaini = DateTime.ParseExact(dataExcel.Split(',')[41], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        revision.StrReveporevcoesfechaini = strReveporevcoesfechaini.ToString(Constantes.FormatoFecha);
                    }
                    revision.Reveporevcoescartarevisiontit = dataExcel.Split(',')[43];
                    revision.Reveporevcoescartarevisionenl = dataExcel.Split(',')[45];
                    revision.Reveporevcoescartarevisionobs = dataExcel.Split(',')[47];
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[49]))
                    {
                        DateTime strRevepocoesfechafin = DateTime.ParseExact(dataExcel.Split(',')[49], Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        revision.StrRevepocoesfechafin = strRevepocoesfechafin.ToString(Constantes.FormatoFecha);
                    }
                    else
                    {
                        revision.Reveporevcoesfinalizado = "0";
                    }
                    if (!string.IsNullOrEmpty(dataExcel.Split(',')[51]))
                    {
                        revision.Reveporevcoesampl = Convert.ToInt32(dataExcel.Split(',')[51]);
                    }
                    else
                    {
                        revision.Reveporevcoesampl = 0;
                    }
                }

                revision.Lastuser = (string)Session["Usuario"];

                DateTime fechaActual = DateTime.ParseExact(DateTime.Now.ToString(Constantes.FormatoFecha), Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                revision.Lastdate = fechaActual;

                if (EsNuevo)
                {
                    _svcGestionEoEpo.SaveEpoRevisionEpo(revision);
                }
                else
                {
                    _svcGestionEoEpo.UpdateEpoRevisionEpo(revision);
                }


                return Json("1");
            }
            catch (Exception ex)
            {
                log.Error("GrabarRevision", ex);
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
        /// <returns></returns>
        public FormatoModel BuildHojaExcel(int revcodi, int cargaformato, int tipoConfig)
        {
            EpoRevisionEpoDTO revision = _svcGestionEoEpo.GetByIdEpoRevisionEpo(revcodi);

            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = new MeFormatoDTO(); //logic.GetByIdMeFormato(this.IdFormato);

            //var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            model.Formato.Formatcols = 2;
            model.Formato.Formatrows = 2;

            List<string> labels = new List<string>();
            labels.Add("Presentación EPO / Absolución de Observaciones,Presentación EPO / Absolución de Observaciones,1");
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
            labels.Add("Fecha Fin,Fecha Fin,0"); //18

            if(tipoConfig == 1)
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
                                    valor = revision.Revepolevobsfechaini.HasValue ? revision.Revepolevobsfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 2:
                                    valor = revision.Revepolevobstit;
                                    break;
                                case 3:
                                    valor = revision.Revepolevobsenl;
                                    break;
                                case 4:
                                    valor = revision.Revepolevobsobs;
                                    break;
                                case 5:
                                    valor = revision.Revepolevobsfechafin.HasValue ? revision.Revepolevobsfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 6:
                                    valor = revision.Revepopreampl.ToString();
                                    break;
                                case 8:
                                    valor = revision.Revepoenvesttercinvfechaini.HasValue ? revision.Revepoenvesttercinvfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 9:
                                    valor = revision.Revepoenvesttercinvtit;
                                    break;
                                case 10:
                                    valor = revision.Revepoenvesttercinvenl;
                                    break;
                                case 11:
                                    valor = revision.Revepoenvesttercinvobs;
                                    break;
                                case 12:
                                    valor = revision.Revepoenvesttercinvinvfechafin.HasValue ? revision.Revepoenvesttercinvinvfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 14:
                                    valor = revision.Reveporevterinvfechaini.HasValue ? revision.Reveporevterinvfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 15:
                                    valor = revision.Reveporevterinvtit;
                                    break;
                                case 16:
                                    valor = revision.Reveporevterinvenl;
                                    break;
                                case 17:
                                    valor = revision.Reveporevterinvobs;
                                    break;
                                case 18:
                                    valor = revision.Reveporevterinvfechafin.HasValue ? revision.Reveporevterinvfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 19:
                                    valor = revision.Reveporevterinvampl.ToString();
                                    break;
                                case 21:
                                    valor = revision.Reveporevcoesfechaini.HasValue ? revision.Reveporevcoesfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 22:
                                    valor = revision.Reveporevcoescartarevisiontit;
                                    break;
                                case 23:
                                    valor = revision.Reveporevcoescartarevisionenl;
                                    break;
                                case 24:
                                    valor = revision.Reveporevcoescartarevisionobs;
                                    break;
                                case 25:
                                    valor = revision.Revepocoesfechafin.HasValue ? revision.Revepocoesfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 26:
                                    valor = revision.Reveporevcoesampl.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if(revision != null && tipoConfig == 2)
                        {
                            switch (w)
                            {
                                case 1:
                                    valor = revision.Revepolevobsfechaini.HasValue ? revision.Revepolevobsfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 2:
                                    valor = revision.Revepolevobstit;
                                    break;
                                case 3:
                                    valor = revision.Revepolevobsenl;
                                    break;
                                case 4:
                                    valor = revision.Revepolevobsobs;
                                    break;
                                case 5:
                                    valor = revision.Revepolevobsfechafin.HasValue ? revision.Revepolevobsfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 6:
                                    valor = revision.Revepopreampl.ToString();
                                    break;
                                case 8:
                                    valor = revision.Revepoenvesttercinvfechaini.HasValue ? revision.Revepoenvesttercinvfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 9:
                                    valor = revision.Revepoenvesttercinvtit;
                                    break;
                                case 10:
                                    valor = revision.Revepoenvesttercinvenl;
                                    break;
                                case 11:
                                    valor = revision.Revepoenvesttercinvobs;
                                    break;
                                case 12:
                                    valor = revision.Revepoenvesttercinvinvfechafin.HasValue ? revision.Revepoenvesttercinvinvfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 14:
                                    valor = revision.Reveporevterinvfechaini.HasValue ? revision.Reveporevterinvfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 15:
                                    valor = revision.Reveporevterinvtit;
                                    break;
                                case 16:
                                    valor = revision.Reveporevterinvenl;
                                    break;
                                case 17:
                                    valor = revision.Reveporevterinvobs;
                                    break;
                                case 18:
                                    valor = revision.Reveporevterinvfechafin.HasValue ? revision.Reveporevterinvfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 20:
                                    valor = revision.Reveporevcoesfechaini.HasValue ? revision.Reveporevcoesfechaini.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 21:
                                    valor = revision.Reveporevcoescartarevisiontit;
                                    break;
                                case 22:
                                    valor = revision.Reveporevcoescartarevisionenl;
                                    break;
                                case 23:
                                    valor = revision.Reveporevcoescartarevisionobs;
                                    break;
                                case 24:
                                    valor = revision.Revepocoesfechafin.HasValue ? revision.Revepocoesfechafin.Value.ToString(Constantes.FormatoFecha) : "";
                                    break;
                                case 25:
                                    valor = revision.Reveporevcoesampl.ToString();
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

        #region Mejoras EO-EPO

        public JsonResult EnviarCorreo()
        {
            string strMsg = string.Empty;
            bool bResult = true;

            try
            {
                _svcGestionEoEpo.ProcesosEoEpo();
                strMsg = "Proceso de correos EPO realizado exitosamente.";
            }
            catch (Exception ex)
            {
                bResult = false;
                strMsg = ex.Message.ToString();
                log.Error("Correo EPO", ex);
            }

            var rpta = new
            {
                sMensaje = strMsg,
                bResult = bResult

            };
            return Json(rpta);
        }

        [HttpPost]
        public JsonResult ExportarListadoWebEO(EpoEstudioEoDTO estudioeo)
        {
            string nombreArchivo = "WebEO_" + DateTime.Now.ToString("yyyyMMddHHmm");
            string rutaArchivo = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + nombreArchivo + ".xlsx";
            string PathLogo = @"Content\Images\logocoes.png";
            string rutaLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;

            estudioeo.nroFilas = 10000;
            List<EpoEstudioEoDTO> listadoEstudioEo = new List<EpoEstudioEoDTO>();

            try
            {
                #region EstudioEO

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

                    List <SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();

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

                #endregion
            }
            catch (Exception ex)
            {
                log.Error("ExportarListadoWebEo", ex);
            }

            return Json(nombreArchivo, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
