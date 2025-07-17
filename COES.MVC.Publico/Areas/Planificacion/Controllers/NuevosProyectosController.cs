using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.GestionEoEpo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using OfficeOpenXml.Style;
using System.Globalization;
using COES.Framework.Base.Core;
using COES.MVC.Publico.Helper;
using COES.MVC.Publico.Controllers;
using System.Drawing;
using OfficeOpenXml.Drawing;

namespace COES.MVC.Publico.Areas.Planificacion.Controllers
{
    public class NuevosProyectosController : BaseController
    {
        GestionEoEpoAppServicio _svcGestionEoEpo = new GestionEoEpoAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(NuevosProyectosController));
        //
        // GET: /Planificacion/NuevosProyectos/


        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("NuevosProyectosController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("NuevosProyectosController", ex);
                throw;
            }
        }

        public ActionResult EstudiosPO()
        {
            return View();
        }
        public ActionResult EstudiosO()
        {
            return View();
        }

        public ActionResult ConexionInstalacion()
        {
            return View();
        }

        public ActionResult OperacionComercial()
        {
            return View();
        }

        public ActionResult ConexionInstalacionSein()
        {
            return View();
        }
        public ActionResult IntegracionInstalacion()
        {
            return View();
        }
        public ActionResult OperacionComercialUnidades()
        {
            return View();
        }
        public ActionResult ConclusionOperacion()
        {
            return View();
        }
        public ActionResult ListaInstalaciones()
        {
            return View();
        }
        public ActionResult ReunionEpoyEos()
        {
            return View();
        }

        public ActionResult ConsultaWebEo()
        {
            List<EpoEstudioEstadoDTO> listadoEstudioEstados = _svcGestionEoEpo.GetByCriteriaEpoEstudioEstados();
            ViewBag.ListadoEstudioEstados = listadoEstudioEstados.Where(x => x.Estacodi == 1 || x.Estacodi == 3 || x.Estacodi > 9).OrderBy(x => x.Estadescripcion);

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoZonaDTO> listadoZonaProyecto = _svcGestionEoEpo.ListarZona();
            ViewBag.ListadoZonaProyecto = listadoZonaProyecto;

            List<EpoPuntoConexionDTO> listadoPuntoConexion = _svcGestionEoEpo.ListarPuntoConexion();
            ViewBag.ListadoPuntoConexion = listadoPuntoConexion;

            return View();
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
        public JsonResult ExportarListadoWebEo(EpoEstudioEoDTO estudioeo)
        {
            string nombreArchivo = "WebEO_" + DateTime.Now.ToString("yyyyMMddHHmm");
            string rutaArchivo = ConfigurationManager.AppSettings["ReportesSNP"] + nombreArchivo + ".xlsx";
            string PathLogo = @"Content\Images\logoportal.png";
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
                    AddImage(ws, 0, 0, rutaLogo);

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

        public FileContentResult DescargarEO(string nombreArchivo)
        {
            string rutaArchivo = ConfigurationManager.AppSettings["ReportesSNP"] + nombreArchivo + ".xlsx";
            var FileBytesArray = System.IO.File.ReadAllBytes(rutaArchivo);

            System.IO.File.Delete(rutaArchivo);

            return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Consulta_Web_EO.xlsx");
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

            return Paginado(model);
        }



        public ActionResult ConsultaWebEpo()
        {
            List<EpoEstudioEstadoDTO> listadoEstudioEstados = _svcGestionEoEpo.GetByCriteriaEpoEstudioEstados();
            ViewBag.ListadoEstudioEstados = listadoEstudioEstados.Where(x => x.Estacodi == 1 || x.Estacodi == 3 || x.Estacodi > 9).OrderBy(x => x.Estadescripcion);

            List<SiEmpresaDTO> listadoEmpresa = _svcGestionEoEpo.ListarEmpresaTodo();
            ViewBag.ListadoEmpresa = listadoEmpresa;

            List<EpoZonaDTO> listadoZonaProyecto = _svcGestionEoEpo.ListarZona();
            ViewBag.ListadoZonaProyecto = listadoZonaProyecto;

            List<EpoPuntoConexionDTO> listadoPuntoConexion = _svcGestionEoEpo.ListarPuntoConexion();
            ViewBag.ListadoPuntoConexion = listadoPuntoConexion;

            return View();
        }

        [HttpPost]
        public ActionResult ListadoWebEPO(EpoEstudioEpoDTO estudioepo)
        {
            List<EpoEstudioEpoDTO> listadoEstudioEpo = new List<EpoEstudioEpoDTO>();

            try
            {
                listadoEstudioEpo = _svcGestionEoEpo.GetByCriteriaEpoEstudioEpos(estudioepo);
            }
            catch (Exception ex)
            {
                log.Error("ListadoWebEPO", ex);
            }

            return PartialView(listadoEstudioEpo);
        }

        [HttpPost]
        public JsonResult ExportarListadoWebEpo(EpoEstudioEpoDTO estudioepo)
        {
            string nombreArchivo = "WebEPO_" + DateTime.Now.ToString("yyyyMMddHHmm");
            string rutaArchivo = ConfigurationManager.AppSettings["ReportesSNP"] + nombreArchivo + ".xlsx";
            string PathLogo = @"Content\Images\logoportal.png";
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
                    AddImage(ws, 0, 0, rutaLogo);

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

            return base.Paginado(model);
        }

        public FileContentResult DescargarEpo(string nombreArchivo)
        {
            string rutaArchivo = ConfigurationManager.AppSettings["ReportesSNP"] + nombreArchivo + ".xlsx";
            var FileBytesArray = System.IO.File.ReadAllBytes(rutaArchivo);

            System.IO.File.Delete(rutaArchivo);

            return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Consulta_Web_EPO.xlsx");
        }

        public static void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);

            }
        }
        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }
    }
}
