using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Eventos.Helper;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class MantenimientoController : BaseController
    {
        GeneralAppServicio servGeneral = new GeneralAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(MantenimientoController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            BusquedaMantenimientoModel model = new BusquedaMantenimientoModel();
            List<SiEmpresaDTO> entitys = this.servGeneral.ListarEmpresasPorTipo(tiposEmpresa).OrderBy(x => x.Emprnomb).ToList();
            model.ListaEmpresas = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Almacena los datos del reporte por tipo de generación
        /// </summary>
        public List<ReporteManttoDTO> ListaManttoEmpresa
        {
            get
            {
                return (Session[DatosSesion.ListaManttoEmpresa] != null) ?
                    (List<ReporteManttoDTO>)Session[DatosSesion.ListaManttoEmpresa] : new List<ReporteManttoDTO>();
            }
            set { Session[DatosSesion.ListaManttoEmpresa] = value; }
        }

        public List<EveManttoDTO> ListaManttos
        {
            get
            {
                return (Session[DatosSesion.ListaManttosTotal] != null) ?
                    (List<EveManttoDTO>)Session[DatosSesion.ListaManttosTotal] : new List<EveManttoDTO>();
            }
            set { Session[DatosSesion.ListaManttosTotal] = value; }
        }
        EventoAppServicio servicio = new EventoAppServicio();

        public ActionResult Index()
        {
            BusquedaMantenimientoModel model = new BusquedaMantenimientoModel();
            model.ListaTipoMantenimiento = this.servicio.ListarClaseEventos();
            model.ListaEmpresas = this.servGeneral.ObtenerEmpresasCOES().OrderBy(x => x.Emprnomb).ToList();
            model.ListaFamilias = this.servicio.ListarFamilias();
            model.ListaTipoEmpresas = this.servicio.ListarTipoEmpresas();
            model.ListaCausaEvento = this.servicio.ListarCausasEventos();
            model.ListaTipoEvento = this.servicio.ListarTipoEventoMantto();

            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ParametroDefecto = 0;
            return View(model);
        }

        [HttpPost]
        public PartialViewResult Lista(string tiposMantenimiento, string fechaInicial, string fechaFinal, string indispo, string tiposEmpresa,
            string empresas, string tiposEquipo, string interrupcion, string tiposMantto, int nroPagina)//
        {
            MantenimientoModel model = new MantenimientoModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fechaInicial != null)
            {
                fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFinal != null)
            {
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            model.ListaManttos = servicio.BuscarMantenimientos(tiposMantenimiento, fechaInicio, fechaFin, indispo,
                tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto, nroPagina, Constantes.PageSizeEvento);
            //Where(x => x.Tipoevencodi != TipoEventos.EventoPruebas).ToList();


            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string tiposMantenimiento, string fechaInicial, string fechaFinal, string indispo,
            string tiposEmpresa, string empresas, string tiposEquipo, string interrupcion, string tiposMantto)
        {
            BusquedaMantenimientoModel model = new BusquedaMantenimientoModel();
            model.IndicadorPagina = false;

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fechaInicial != null)
            {
                fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFinal != null)
            {
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            int nroRegistros = servicio.ObtenerNroFilasMantenimiento(tiposMantenimiento, fechaInicio, fechaFin, indispo,
                tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = nroRegistros > 3000 ? false : true;
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ObtenerTipoManttos()
        {
            var listTipoManto = this.ListaManttos.Select(x => new { x.Evenclasecodi, x.Evenclasedesc }).Distinct().ToList();
            return Json(listTipoManto.OrderBy(x => x.Evenclasecodi));
        }

        private List<ReporteManttoDTO> ObtenerListaManttoEmpresa()
        {
            var listEmpresa = (from t in this.ListaManttos
                               orderby t.Evenclasecodi
                               group t by new { t.Evenclasecodi, t.Evenclasedesc, t.Emprcodi, t.Emprnomb }
                                   into destino
                               select new ReporteManttoDTO()
                               {
                                   Evenclasecodi = destino.Key.Evenclasecodi,
                                   Evenclasedesc = destino.Key.Evenclasedesc,
                                   Emprcodi = destino.Key.Emprcodi,
                                   Emprnomb = destino.Key.Emprnomb,
                                   Porcentajemantto = (decimal)destino.Count()
                               }).ToList();

            var lista = from e in listEmpresa
                        orderby e.Evenclasecodi, e.Porcentajemantto descending
                        select e;
            return lista.ToList();
        }

        [HttpPost]
        public JsonResult GraficoManntoEmpresa(int idmanto)
        {
            if (idmanto == 0 && ListaManttos.Count > 0)
                idmanto = (int)ListaManttos.Min(x => x.Evenclasecodi);
            var list = ObtenerListaManttoEmpresa();
            int totalMantos = (int)list.Sum(x => x.Porcentajemantto);
            foreach (var reg in list)
                reg.Porcentajemantto = reg.Porcentajemantto / totalMantos * 100;
            var entitys = list.Where(x => x.Evenclasecodi == idmanto).Select(x => new { x.Emprnomb, x.Porcentajemantto }).OrderBy(x => x.Emprnomb).ToList();
            return Json(entitys);
        }

        private List<ReporteManttoDTO> ObtenerListaManttoEquipo()
        {
            var listEquipos = (from t in this.ListaManttos
                               orderby t.Evenclasecodi
                               group t by new { t.Evenclasecodi, t.Evenclasedesc, t.Famcodi, t.Famnomb }
                                   into destino
                               select new ReporteManttoDTO()
                               {
                                   Evenclasecodi = destino.Key.Evenclasecodi,
                                   Evenclasedesc = destino.Key.Evenclasedesc,
                                   Famcodi = destino.Key.Famcodi,
                                   Famnomb = destino.Key.Famnomb,
                                   Porcentajemantto = (decimal)destino.Count()
                               }).ToList();
            var lista = from e in listEquipos
                        orderby e.Evenclasecodi, e.Porcentajemantto descending
                        select e;
            return lista.ToList();
        }

        [HttpPost]
        public JsonResult GraficoManntoEquipo(int idmanto)
        {
            if (idmanto == 0 && ListaManttos.Count > 0)
                idmanto = (int)ListaManttos.Min(x => x.Evenclasecodi);
            var list = ObtenerListaManttoEquipo();
            int totalMantos = (int)list.Sum(x => x.Porcentajemantto);
            var entitys = list.Where(x => x.Evenclasecodi == idmanto).Select(x => new { x.Famcodi, x.Famnomb, x.Porcentajemantto }).OrderBy(x => x.Famnomb).ToList();
            return Json(entitys);
        }

        private List<ReporteManttoDTO> ObtenerListaManttoEmpresaEquipo1()
        {
            var listEquipos = (from t in this.ListaManttos
                               orderby t.Evenclasecodi, t.Emprcodi, t.Tipoevendesc
                               group t by new { t.Evenclasecodi, t.Evenclasedesc, t.Emprcodi, t.Emprabrev, t.Tipoevendesc }
                                   into destino
                               select new ReporteManttoDTO()
                               {
                                   Evenclasecodi = destino.Key.Evenclasecodi,
                                   Evenclasedesc = destino.Key.Evenclasedesc,
                                   Emprcodi = destino.Key.Emprcodi,
                                   Emprabrev = destino.Key.Emprabrev,
                                   Tipoevendesc = destino.Key.Tipoevendesc,
                                   Subtotal = destino.Count()
                               }).ToList();

            return listEquipos;
        }

        public JsonResult GraficoManntoEmpresaEquipo1(int idmanto)
        {
            if (idmanto == 0 && ListaManttos.Count > 0)
                idmanto = (int)ListaManttos.Min(x => x.Evenclasecodi);
            var list = ObtenerListaManttoEmpresaEquipo1();
            int totalMantos = (int)list.Sum(x => x.Porcentajemantto);
            var entitys = list.Where(x => x.Evenclasecodi == idmanto).Select(x => new { x.Emprcodi, x.Emprabrev, x.Tipoevendesc, x.Subtotal }).ToList();
            return Json(entitys);
        }

        private List<ReporteManttoDTO> ObtenerListaManttoEmpresaEquipo2()
        {
            var list = (from t in this.ListaManttos
                        orderby t.Evenclasecodi, t.Emprcodi, t.Famnomb
                        group t by new { t.Evenclasecodi, t.Evenclasedesc, t.Emprcodi, t.Emprabrev, t.Famnomb, t.Famcodi }
                            into destino
                        select new ReporteManttoDTO()
                        {
                            Evenclasecodi = destino.Key.Evenclasecodi,
                            Evenclasedesc = destino.Key.Evenclasedesc,
                            Emprcodi = destino.Key.Emprcodi,
                            Emprabrev = destino.Key.Emprabrev,
                            Famnomb = destino.Key.Famnomb,
                            Famcodi = (int)destino.Key.Famcodi,
                            Subtotal = destino.Count()
                        }).ToList();
            return list;
        }

        public JsonResult GraficoManntoEmpresaEquipo2(int idmanto)
        {
            if (idmanto == 0 && ListaManttos.Count > 0)
                idmanto = (int)ListaManttos.Min(x => x.Evenclasecodi);
            var list = ObtenerListaManttoEmpresaEquipo2();
            int totalMantos = (int)list.Sum(x => x.Porcentajemantto);
            var entitys = list.Where(x => x.Evenclasecodi == idmanto).Select(x => new { x.Emprcodi, x.Emprabrev, x.Famnomb, x.Famcodi, x.Subtotal }).ToList();
            return Json(entitys);
        }

        private List<ReporteManttoDTO> ObtenerListaManttoEquipo2()
        {
            var list = (from t in this.ListaManttos
                        orderby t.Evenclasecodi, t.Tipoevendesc, t.Famcodi
                        group t by new { t.Evenclasecodi, t.Tipoevendesc, t.Evenclasedesc, t.Famnomb }
                            into destino
                        select new ReporteManttoDTO()
                        {
                            Evenclasecodi = destino.Key.Evenclasecodi,
                            Evenclasedesc = destino.Key.Evenclasedesc,
                            Famnomb = destino.Key.Famnomb,
                            Tipoevendesc = destino.Key.Tipoevendesc,
                            Subtotal = destino.Count()
                        })
                            .ToList();
            return list;
        }

        public JsonResult GraficoManntoEquipo2(int idmanto)
        {
            if (idmanto == 0 && ListaManttos.Count > 0)
                idmanto = (int)ListaManttos.Min(x => x.Evenclasecodi);
            var list = ObtenerListaManttoEquipo2();
            var entitys = list.Where(x => x.Evenclasecodi == idmanto).Select(x => new { x.Famnomb, x.Tipoevendesc, x.Subtotal }).ToList();
            return Json(entitys);
        }

        private List<ReporteManttoDTO> ObtenerListaTipoManttoTipoEmpresa()
        {
            var list = (from t in this.ListaManttos
                        orderby t.Evenclasecodi, t.Tipoevendesc, t.Tipoemprdesc
                        group t by new { t.Evenclasecodi, t.Evenclasedesc, t.Tipoevendesc, t.Tipoemprdesc }
                            into destino
                        select new ReporteManttoDTO()
                        {
                            Evenclasecodi = destino.Key.Evenclasecodi,
                            Evenclasedesc = destino.Key.Evenclasedesc,
                            Tipoevendesc = destino.Key.Tipoevendesc,
                            Tipoemprdesc = destino.Key.Tipoemprdesc,
                            Subtotal = destino.Count()
                        }).ToList();

            return list;
        }

        public JsonResult GraficoTipoMantoTipoEmpresa(int idmanto)
        {
            if (idmanto == 0 && ListaManttos.Count > 0)
                idmanto = (int)ListaManttos.Min(x => x.Evenclasecodi);
            var list = ObtenerListaTipoManttoTipoEmpresa();
            var entitys = list.Where(x => x.Evenclasecodi == idmanto).Select(x => new { x.Tipoevendesc, x.Tipoemprdesc, x.Subtotal }).ToList();
            return Json(entitys);
        }

        [HttpPost]
        public JsonResult GenerarArchivoManttoEmpresa(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            var list = ObtenerListaManttoEmpresa();

            MantenimientoModel model = new MantenimientoModel();
            model.FechaInicio = fechaInicial;
            model.FechaFin = fechaFinal;
            model.ListarReporteManttos = list;
            ExcelGrafico.GenerarArchivoMantoEmpresa(model);
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult GenerarArchivoManttoEquipo(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            var list = ObtenerListaManttoEquipo();
            MantenimientoModel model = new MantenimientoModel();
            model.FechaInicio = fechaInicial;
            model.FechaFin = fechaFinal;
            model.ListarReporteManttos = list;
            ExcelGrafico.GenerarArchivoMantoEquipo(model);
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult GenerarArchivoManttoEmpresaEquipo1(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            var list = ObtenerListaManttoEmpresaEquipo1();
            MantenimientoModel model = new MantenimientoModel();
            model.FechaInicio = fechaInicial;
            model.FechaFin = fechaFinal;
            model.ListarReporteManttos = list;
            ExcelGrafico.GenerarArchivoMantoEmpresaEquipo1(model);
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult GenerarArchivoManttoEmpresaEquipo2(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            var list = ObtenerListaManttoEmpresaEquipo2();
            MantenimientoModel model = new MantenimientoModel();
            model.FechaInicio = fechaInicial;
            model.FechaFin = fechaFinal;
            model.ListarReporteManttos = list;
            ExcelGrafico.GenerarArchivoMantoEmpresaEquipo2(model);
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult GenerarArchivoManttoEquipo2(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            var list = ObtenerListaManttoEquipo2();
            MantenimientoModel model = new MantenimientoModel();
            model.FechaInicio = fechaInicial;
            model.FechaFin = fechaFinal;
            model.ListarReporteManttos = list;
            ExcelGrafico.GenerarArchivoMantoEquipo2(model);
            return Json(indicador);
        }

        [HttpPost]
        public JsonResult GenerarArchivoTipoMantoTipoEmpresa(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            var list = ObtenerListaTipoManttoTipoEmpresa();
            MantenimientoModel model = new MantenimientoModel();
            model.FechaInicio = fechaInicial;
            model.FechaFin = fechaFinal;
            model.ListarReporteManttos = list;
            ExcelGrafico.GenerarArchivoTipoMantoTipoEmpresa(model);
            return Json(indicador);
        }

        [HttpPost]
        public PartialViewResult GraficoReporte(string tiposMantenimiento, string fechaInicial, string fechaFinal, string indispo, string tiposEmpresa,
            string empresas, string tiposEquipo, string interrupcion, string tiposMantto)
        {
            int indicador = 1;
            BusquedaMantenimientoModel model = new BusquedaMantenimientoModel();
            try
            {

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (fechaInicial != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFinal != null)
                {
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                fechaFin = fechaFin.AddDays(1);
                this.ListaManttos = servicio.GenerarReportesGrafico(tiposMantenimiento, fechaInicio, fechaFin, indispo,
                 tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto);//.Where(x => x.Tipoevencodi != TipoEventos.EventoPruebas).ToList();
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }
            return PartialView(model);
        }
        // exporta el reporte general consultado a archivo excel
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string tiposMantenimiento, string fechaInicial, string fechaFinal, string indispo, string tiposEmpresa,
             string empresas, string tiposEquipo, string interrupcion, string tiposMantto)
        {
            MantenimientoModel model = new MantenimientoModel();

            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (fechaInicial != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFinal != null)
                {
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                fechaFin = fechaFin.AddDays(1);

                int nroRegistros = servicio.ObtenerNroFilasMantenimiento(tiposMantenimiento, fechaInicio, fechaFin, indispo,
                    tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto);
                if (nroRegistros > 3000)
                {
                    throw new Exception("La información consultada sobrepasa la cantidad máxima de registros: 3000.");
                }

                var list = servicio.BuscarMantenimientos(tiposMantenimiento, fechaInicio, fechaFin, indispo,
                    tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto, 1, 100000);

                
                model.FechaInicio = fechaInicial;
                model.FechaFin = fechaFinal;

                var lista = list.OrderBy(x => x.Evenclasecodi).ThenBy(x => x.Emprnomb).ThenBy(x => x.Areanomb).ThenBy(x => x.Famnomb).ThenBy(x => x.Equiabrev).ThenBy(x => x.Evenini).ThenBy(x => x.Evenfin).ToList();
                List<EveManttoDTO> listaManttos = lista.ToList();
                if (!listaManttos.Any())
                {
                    throw new Exception("No hay registros a exportar para el filtro seleccionado.");
                }

                ExcelGrafico.GernerarArchivoMantenimiento(listaManttos, fechaInicial, fechaFinal);
                model.Resultado = "1";
            }
            catch(Exception ex)
            {

                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite exportar el reporte general y por tipo de mantenimientos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            var sTipoReporte = Request["tipo"];
            short tipoReporte = short.Parse(sTipoReporte);
            switch (tipoReporte)
            {
                case 0:
                    nombreArchivo = Constantes.NombreReporteMantenimiento;
                    break;
                case 1:
                    nombreArchivo = Constantes.NombreReporteMantenimiento01;
                    break;
                case 2:
                    nombreArchivo = Constantes.NombreReporteMantenimiento02;
                    break;
                case 3:
                    nombreArchivo = Constantes.NombreReporteMantenimiento03;
                    break;
                case 4:
                    nombreArchivo = Constantes.NombreReporteMantenimiento04;
                    break;
                case 5:
                    nombreArchivo = Constantes.NombreReporteMantenimiento05;
                    break;
                case 6:
                    nombreArchivo = Constantes.NombreReporteMantenimiento06;
                    break;


            }
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteEvento] + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #region Migraciones 2024

        /// <summary>
        /// Valida la cantidad de registros de mantenimientos consultado
        /// </summary>
        /// <param name="tiposMantenimiento"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="indispo"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposEquipo"></param>
        /// <param name="interrupcion"></param>
        /// <param name="tiposMantto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarNumRegistros(string tiposMantenimiento, string fechaInicial, string fechaFinal, string indispo, string tiposEmpresa,
            string empresas, string tiposEquipo, string interrupcion, string tiposMantto)
        {
            MantenimientoModel model = new MantenimientoModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddDays(1);

                int nroRegistros = servicio.ObtenerNroFilasMantenimiento(tiposMantenimiento, fechaInicio, fechaFin, indispo,
                    tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto);

                if (nroRegistros > 3000)
                {
                    throw new Exception("La información consultada sobrepasa la cantidad máxima de registros: 3000.");
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Genera el reporte de formato cruzado de mantenimientos
        /// </summary>
        /// <param name="tiposMantenimiento"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="Indisponibilidad"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposEquipo"></param>
        /// <param name="conInterrupcion"></param>
        /// <param name="tiposMantto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoFmtCruzado(string tiposMantenimiento, string fechaInicial, string fechaFinal, string Indisponibilidad, string tiposEmpresa,
             string empresas, string tiposEquipo, string conInterrupcion, string tiposMantto)
        {
            MantenimientoModel model = new MantenimientoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                bool fechasIguales = fechaInicial == fechaFinal;
                string nombArchivo = fechasIguales ? (fecInicial.ToString("yyyyMMdd") + "_ReporteMantenimientoCruzado.xlsx") : (fecInicial.ToString("yyyyMMdd") + "_" + fecFinal.ToString("yyyyMMdd") + "_ReporteMantenimientoCruzado.xlsx");

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                EventoAppServicio servicioEv = new EventoAppServicio();
                servicioEv.GenerarExportacionRFmtCruzado(ruta, pathLogo, nombArchivo, fecInicial, fecFinal, tiposMantenimiento, Indisponibilidad, tiposEmpresa, empresas, tiposEquipo, conInterrupcion, tiposMantto);
                model.Resultado = nombArchivo;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesEventos.ModuloManualUsuarioSGI;
            string nombreArchivo = ConstantesEventos.ArchivoManualUsuarioIntranetSGI;
            string pathDestino = modulo + ConstantesEventos.FolderRaizSGIModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        /// <summary>
        /// Exporta archivo pdf, excel, csv, ...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarArchivoReporte()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        #endregion
    }
}
