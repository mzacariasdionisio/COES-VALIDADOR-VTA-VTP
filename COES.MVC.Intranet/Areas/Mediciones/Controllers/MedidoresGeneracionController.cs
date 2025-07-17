using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Mediciones.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mediciones.Controllers
{
    public class MedidoresGeneracionController : Controller
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(MedidoresGeneracionController));
        private static string NameController = "MedidoresGeneracionController";

        /// <summary>
        /// Clase para acceso a los datos y bl
        /// </summary>
        ConsultaMedidoresAppServicio servicio = new ConsultaMedidoresAppServicio();

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

        /// <summary>
        /// Carga inicial de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();

            //model.ListaEmpresas = this.servicio.ListaEmpresa();           
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        public ActionResult Reporte()
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();

            //model.ListaEmpresas = this.servicio.ListaEmpresa();           
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite cargar las empresas por los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();
            List<SiEmpresaDTO> entitys = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);
            model.ListaEmpresas = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Permite listar los registros de medidores de generación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central, int parametro, int nroPagina)
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            List<MeMedicion96DTO> sumatoria = new List<MeMedicion96DTO>();
            model.ListaDatos = this.servicio.ObtenerConsultaMedidores(fecInicio, fecFin, tiposEmpresa, empresas,
                tiposGeneracion, central, parametro,
                nroPagina, Constantes.PageSizeMedidores, out sumatoria);

            model.EntidadTotal = sumatoria;

            string header = string.Empty;
            if (parametro == 1) header = "Total Energía Activa  (MWh)";
            if (parametro == 5) header = "Total Energía Reactiva (MVarh)";
            if (parametro == 2) header = "Total Energía Reactiva Capacitiva (MVarh)";
            if (parametro == 4) header = "Total Energía Reactiva Inductiva (MVarh)";
            if (parametro == 3) header = "Total Servicios Auxiliares (MWh)";

            model.TextoCabecera = header;
            model.IndicadorPublico = (User.Identity.Name == Constantes.UsuarioAnonimo) ? Constantes.SI : Constantes.NO;

            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListaGeneracionNoCoes(string fechaInicial, string fechaFinal, int nroPagina)
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            List<MeMedicion48DTO> sumatoria = new List<MeMedicion48DTO>();
            model.ListaDatosGeneracionNoCoes = this.servicio.ObtenerConsultaMedidoresGeneracionNoCoes(fecInicio, fecFin, nroPagina, Constantes.PageSizeMedidores);

            model.EntidadTotalGeneracionNoCoes = sumatoria;

            string header = string.Empty;

            model.TextoCabecera = header;
            model.IndicadorPublico = (User.Identity.Name == Constantes.UsuarioAnonimo) ? Constantes.SI : Constantes.NO;

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central, int parametro)
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            int nroRegistros = !string.IsNullOrEmpty(empresas) ?
                this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin, tiposEmpresa,
                empresas, tiposGeneracion, central, parametro) : 1;

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeMedidores;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult PaginadoGeneracionNoCoes(string fechaInicial, string fechaFinal)
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }



            int nroRegistros = this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeMedidores;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central, string parametros, int tipo)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string file = string.Empty;

                if (tipo == 1) file = NombreArchivo.ReporteMedidoresHorizontal;
                if (tipo == 2) file = NombreArchivo.ReporteMedidoresVertical;
                if (tipo == 3) file = NombreArchivo.ReporteMedidoresCSV;

                bool flag = (User.Identity.Name == Constantes.UsuarioAnonimo) ? false : true;

                this.servicio.GenerarArchivoExportacion(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, central,
                    parametros, path, file, tipo, flag);


                return Json("1");
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult ExportarGeneracionNoCoes(string fechaInicial, string fechaFinal)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string file = string.Empty;

                file = "GeneraciónNoCoes.xlsx";               

                this.servicio.GenerarArchivoExportacionGeneracionNoCoes(fecInicio, fecFin, path, file);


                return Json("1");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int tipo)
        {
            string file = string.Empty;
            string app = Constantes.AppExcel;

            if (tipo == 1) file = NombreArchivo.ReporteMedidoresHorizontal;
            if (tipo == 2) file = NombreArchivo.ReporteMedidoresVertical;
            if (tipo == 3)
            {
                file = NombreArchivo.ReporteMedidoresCSV;
                app = Constantes.AppCSV;
            }

            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(fullPath, app, file);
        }

        public virtual ActionResult DescargarGeneracionNoCoes()
        {
            string file = string.Empty;
            string app = Constantes.AppExcel;

            file = "GeneraciónNoCoes.xlsx";



            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(fullPath, app, file);
        }

        /// <summary>
        /// Valida la selección de datos de exportación
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarExportacion(int formato, string fechaInicial, string fechaFinal, string parametros)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                TimeSpan ts = fecFin.Subtract(fecInicio);

                if (ts.TotalDays > 92)
                {
                    return Json(2);
                }

                if (formato == 3)
                {
                    if (!string.IsNullOrWhiteSpace(parametros))
                    {
                        string[] ids = parametros.Split(Constantes.CaracterComa);

                        if (ids.Count() > 1)
                        {
                            return Json(3);
                        }
                    }
                    else
                    {
                        return Json(4);
                    }
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ValidarExportacionGeneracionNoCoes(string fechaInicial, string fechaFinal)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                TimeSpan ts = fecFin.Subtract(fecInicio);

                if (ts.TotalDays > 92)
                {
                    return Json(2);
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Listar parámetros disponibles
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarParametro(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            try
            {
                List<EstadoModel> listaOpcion = new List<EstadoModel>();

                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }


                listaOpcion.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "Potencia Activa (MW)" });

                int nroRegistrosReact = 0, nroRegistrosReactCap = 0, nroRegistrosReactInd = 0;

                if (!string.IsNullOrEmpty(empresas))
                {
                    nroRegistrosReact = this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin, tiposEmpresa,
                        empresas, tiposGeneracion, central, 5);
                    nroRegistrosReactCap = this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin, tiposEmpresa,
                        empresas, tiposGeneracion, central, 2);
                    nroRegistrosReactInd = this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin, tiposEmpresa,
                        empresas, tiposGeneracion, central, 4);
                }

                if (nroRegistrosReact > 0 || (nroRegistrosReactCap == 0 || nroRegistrosReactInd == 0))
                {
                    listaOpcion.Add(new EstadoModel() { EstadoCodigo = "5", EstadoDescripcion = "Potencia Reactiva (MVAR)" });
                }

                if (nroRegistrosReactCap > 0 && nroRegistrosReactInd > 0)
                {
                    listaOpcion.Add(new EstadoModel() { EstadoCodigo = "2", EstadoDescripcion = "Potencia Reactiva Capacitiva (MVAR)" });
                    listaOpcion.Add(new EstadoModel() { EstadoCodigo = "4", EstadoDescripcion = "Potencia Reactiva Inductiva (MVAR)" });
                }

                listaOpcion.Add(new EstadoModel() { EstadoCodigo = "3", EstadoDescripcion = "Servicios Auxiliares" });

                return Json(listaOpcion);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(-1);
            }
        }
    }
}
