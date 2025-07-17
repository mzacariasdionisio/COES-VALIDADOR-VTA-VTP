using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Mediciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Titularidad;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mediciones.Controllers
{
    public class GeneracionRERController : BaseController
    {
        GeneracionRERAppServicio logic = new GeneracionRERAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        SeguridadServicioClient servSeguridad = new SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
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

        /// <summary>
        /// Muestra la página de inicio del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            GeneracionRERModel model = new GeneracionRERModel();

            model.ListaFormato = this.servFormato.ListarFormatosGeneracionRER();
            model.ListaEmpresas = this.servFormato.ListarEmpresaGeneracionRER();

            //
            DateTime fechaActual = DateTime.Now.AddDays(1);
            model.Fecha = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);

            //semanas
            Tuple<int, int> tupla = EPDate.f_numerosemana_y_anho(fechaActual.AddDays(6));
            model.NroSemana = tupla.Item1;

            List<GenericoDTO> entitys = new List<GenericoDTO>();

            int nsemanas = EPDate.TotalSemanasEnAnho(tupla.Item2, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + tupla.Item2;
                reg.String2 = i == tupla.Item1 ? "selected" : "";
                entitys.Add(reg);

            }
            model.Anho = tupla.Item2.ToString();
            model.ListaSemanas2 = entitys;


            return View(model);
        }

        /// <summary>
        /// Permite mostrar los datos cargados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="horizonte"></param>
        /// <param name="fecha"></param>
        /// <param name="nroSemana"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Lista(int anio, int horizonte, string fecha, int? nroSemana, string idEmpresa, string tipoCentral, string tipoCogeneracion, string tipoReporte)
        {
            GeneracionRERModel model = new GeneracionRERModel();

            try
            {
                DateTime fechaInicial = DateTime.Now;
                DateTime fechaFinal = DateTime.Now;

                if (ConstantesFormatoMedicion.LectGeneracionRERDiario == horizonte)
                {
                    fechaInicial = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    fechaFinal = fechaInicial;
                }
                else
                {
                    fechaInicial = EPDate.f_fechainiciosemana(anio, (int)nroSemana);
                    fechaFinal = fechaInicial.AddDays(6);
                }

                string resultado = this.servFormato.ReporteGeneracionRERProgHtml(ConstantesFormatoMedicion.TipoPresentacionUnidad, horizonte, fechaInicial, fechaFinal, idEmpresa, tipoCentral, tipoCogeneracion, tipoReporte);
                model.Resultado = resultado;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite generar el reporte de cumplimiento
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(int tipoPresentacion, int anio, int horizonte, string fecha, int? nroSemana, string idEmpresa, string tipoCentral, string tipoCogeneracion, string tipoReporte)
        {
            GeneracionRERModel model = new GeneracionRERModel();

            try
            {
                DateTime fechaInicial = DateTime.Now;
                DateTime fechaFinal = DateTime.Now;

                if (ConstantesFormatoMedicion.LectGeneracionRERDiario == horizonte)
                {
                    fechaInicial = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    fechaFinal = fechaInicial;
                }
                else
                {
                    fechaInicial = EPDate.f_fechainiciosemana(anio, (int)nroSemana);
                    fechaFinal = fechaInicial.AddDays(6);
                }

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel.ToString();
                string nombre = NombreArchivo.ReporteGeneracionRER;
                this.servFormato.GenerarExcelGeneracionRERProg(ruta, nombre, tipoPresentacion, horizonte, fechaInicial, fechaFinal, idEmpresa, tipoCentral, tipoCogeneracion, tipoReporte);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteGeneracionRER;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteGeneracionRER);
        }

        /// <summary>
        /// Muestra la página de inicio del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Reporte()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            GeneracionRERModel model = new GeneracionRERModel();

            model.ListaFormato = this.servFormato.ListarFormatosGeneracionRER();

            DateTime fechaActual = DateTime.Now.AddDays(1);
            model.Fecha = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);

            //semanas
            Tuple<int, int> tupla = EPDate.f_numerosemana_y_anho(fechaActual.AddDays(6));
            model.NroSemana = tupla.Item1;

            List<GenericoDTO> entitys = new List<GenericoDTO>();

            int nsemanas = EPDate.TotalSemanasEnAnho(tupla.Item2, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + tupla.Item2;
                reg.String2 = i == tupla.Item1 ? "selected" : "";
                entitys.Add(reg);

            }
            model.Anho = tupla.Item2.ToString();
            model.ListaSemanas2 = entitys;

            //caso especial
            var objUsuario = this.servSeguridad.ObtenerUsuarioPorLogin(User.Identity.Name);
            model.TienePermisoDTI = objUsuario != null && objUsuario.AreaCode == ConstantesTitularidad.AreacodiDTI;

            return View(model);
        }

        /// <summary>
        /// Permite generar el reporte de cumplimiento
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fecha"></param>
        /// <param name="nroSemana"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Cumplimiento(int horizonte, string fecha, int? nroSemana, int anio)
        {
            GeneracionRERModel model = new GeneracionRERModel();
            DateTime fechaProceso;

            if (ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario == horizonte)
            {
                fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            else
            {
                fechaProceso = EPDate.f_fechainiciosemana(anio, nroSemana.Value);
            }

            model.ListaReporte = servFormato.GenerarReporteCumplimiento(horizonte, fechaProceso);

            return PartialView(model);
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho)
        {
            GeneracionRERModel model = new GeneracionRERModel();
            List<GenericoDTO> entitys = new List<GenericoDTO>();
            if (idAnho == "0")
            {
                idAnho = DateTime.Now.Year.ToString();
            }
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = EPDate.TotalSemanasEnAnho(Int32.Parse(idAnho), FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemanas2 = entitys;
            return PartialView(model);
        }

        /// <summary>
        /// Permite ontener las fechas por semana
        /// </summary>
        /// <param name="nroSemana"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerFechasAnio(int nroSemana, int anio)
        {
            GeneracionRERModel json = new GeneracionRERModel();

            DateTime fecha = EPDate.f_fechainiciosemana(anio, nroSemana);
            json.FechaInicio = fecha.ToString(Constantes.FormatoFecha);
            json.FechaFin = fecha.AddDays(6).ToString(Constantes.FormatoFecha);

            return Json(json);
        }

        /// <summary>
        /// Permite generar el reporte de cumplimiento
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarProcesoEnvio()
        {
            GeneracionRERModel model = new GeneracionRERModel();

            try
            {
                base.ValidarSesionJsonResult();

                servFormato.EjecutarProcesoCopiarExtLogEnvioAMeEnvio();
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

    }
}
