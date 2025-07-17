using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Eventos.Helper;
using COES.MVC.Publico.Areas.Eventos.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Eventos.Controllers
{
    public class MantenimientoController : Controller
    {
        EventoAppServicio servicio = new EventoAppServicio();
        IntervencionesAppServicio servicioIntervenciones = new IntervencionesAppServicio();

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

        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            BusquedaMantenimientoModel model = new BusquedaMantenimientoModel();
            List<SiEmpresaDTO> entitys = (new ConsultaMedidoresAppServicio()).ObteneEmpresasPorTipoGeneralSein(tiposEmpresa).OrderBy(x => x.Emprnomb).ToList();
            model.ListaEmpresas = entitys;

            return PartialView(model);
        }

        public ActionResult Index()
        {
            BusquedaMantenimientoModel model = new BusquedaMantenimientoModel();
            model.ListaTipoMantenimiento = this.servicio.ListarClaseEventos().Where(x => x.Evenclasecodi < 5).ToList(); //no mostrar el programa anual ni los otros tipos
            model.ListaEmpresas = (new ConsultaMedidoresAppServicio()).ListaEmpresa().OrderBy(x => x.Emprnomb).ToList();
            model.ListaFamilias = this.servicio.ListarFamilias();
            model.ListaTipoEmpresas = this.servicio.ListarTipoEmpresas();
            model.ListaCausaEvento = this.servicio.ListarCausasEventos();
            model.ListaTipoEvento = this.servicio.ListarTipoEventoMantto();

            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFechaISO);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFechaISO);
            model.ParametroDefecto = 0;
            return View(model);
        }

        [HttpPost]

        public PartialViewResult Lista(string tiposMantenimiento, string fechaInicial, string fechaFinal, string indispo,
                                        string tiposEmpresa, string empresas, string tiposEquipo, string interrupcion, string tiposMantto, int nroPagina = 1)
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

            if (indispo == null) indispo = "-1";

            TimeSpan ts = fechaFin.Subtract(fechaInicio);

            if (ts.TotalDays > 92)
            {
                model.Resultado = "-1";
                model.ListaManttos = new List<EveManttoDTO>();
            }
            else
            {
                var listaCompleta = servicioIntervenciones.GenerarReportesGrafico(
                    tiposMantenimiento, fechaInicio, fechaFin, indispo,
                    tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto);

                const int pageSize = Constantes.PageSizeEvento;

                var paginados = listaCompleta
                    .Skip((nroPagina - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                model.Resultado = "1";
                model.ListaManttos = paginados;
            }

            return PartialView(model);
        }


        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string tiposMantenimiento, string fechaInicial, string fechaFinal, string indispo,
            string tiposEmpresa, string empresas, string tiposEquipo, string interrupcion, string tiposMantto, int? nroPagina = 1)
        {
            BusquedaMantenimientoModel model = new BusquedaMantenimientoModel();
            model.IndicadorPagina = false;
            model.NroPagina = nroPagina ?? 1;

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

            if (indispo == null) indispo = "-1";

            int nroRegistros = servicioIntervenciones.ObtenerNroFilasMantenimiento(tiposMantenimiento, fechaInicio, fechaFin, indispo,
                tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
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

                if (indispo == null) indispo = "-1";
                this.ListaManttos = servicioIntervenciones.GenerarReportesGrafico(tiposMantenimiento, fechaInicio, fechaFin, indispo,
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
            int indicador = 1;

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
                if (indispo == null) indispo = "-1";
                var list = servicioIntervenciones.GenerarReportesGrafico(tiposMantenimiento, fechaInicio, fechaFin, indispo,
                    tiposEmpresa, empresas, tiposEquipo, interrupcion, tiposMantto);
                MantenimientoModel model = new MantenimientoModel();
                model.FechaInicio = fechaInicial;
                model.FechaFin = fechaFinal;
                var lista = from e in list
                            orderby e.Evenclasecodi, e.Evenini
                            select e;
                model.ListaManttos = lista.ToList();
                ExcelGrafico.GernerarArchivoMantenimiento(model);
                indicador = 1;
            }
            catch(Exception ex)
            {
                indicador = -1;
            }

            return Json(indicador);
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
    }
}
