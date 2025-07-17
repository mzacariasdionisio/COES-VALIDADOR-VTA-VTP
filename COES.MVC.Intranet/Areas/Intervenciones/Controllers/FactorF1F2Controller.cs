using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.Intervenciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Intervenciones.Controllers
{
    public class FactorF1F2Controller : BaseController
    {
      readonly IntervencionesAppServicio appIntervenciones = new IntervencionesAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #region Versiones

        public ActionResult Index(string fechaPeriodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            string strFecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString(ConstantesAppServicio.FormatoMes);

            if (fechaPeriodo != null)
            {

                DateTime Mes = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                strFecha = Mes.ToString(ConstantesAppServicio.FormatoMes);
            }

            var modelo = new FactorF1F2Model
            {
                Mes = strFecha
            };

            return View(modelo);
        }

        [HttpPost]
        public JsonResult ListadoVersion(string fechaPeriodo)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime Mes = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                model.ListInFactorVersion = appIntervenciones.GetByFechaInFactorVersions(Mes, ConstantesIntervencionesAppServicio.ModuloFactores);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guardar versión
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public JsonResult GuardarNuevaVersion(string fechaPeriodo, string estado)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                this.ValidarSesionJsonResult();

                // Validación
                DateTime dFechaPeriodo = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                estado = (estado ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(estado))
                {
                    throw new Exception("Debe ingresar un estado.");
                }

                //Obtener versión anterior
                List<InFactorVersionDTO> listInFactorVersion = appIntervenciones.GetByFechaInFactorVersions(dFechaPeriodo, ConstantesIntervencionesAppServicio.ModuloFactores);
                InFactorVersionDTO inFactorVersionDTO = listInFactorVersion.OrderByDescending(p => p.Infvercodi).FirstOrDefault(p => p.Infverflagfinal == "S");

                // Guardamos la nueva version
                bool esVersionCopia = estado == "S" && inFactorVersionDTO != null; //crear copia cuando ya existe una versión final
                int vercodi = 0;
                if (!esVersionCopia)
                {
                    vercodi = appIntervenciones.CrearVersionFactorF1F2(dFechaPeriodo, estado, base.UserName);
                }
                else
                {
                    vercodi = appIntervenciones.AgruparMantenimientoMayorXVersion(inFactorVersionDTO.Infvercodi, base.UserName);
                }

                appIntervenciones.CopiarArchivoVersionFinal(vercodi);

                model.Resultado = "1";
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
        /// Genera un reporte Excel de los factores
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoExcelReporte(int infvercodi)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();

                appIntervenciones.GenerarRptExcelFactorF1F2(infvercodi, out string nameFile);

                model.Resultado = nameFile;
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

        [HttpPost]
        public JsonResult AgruparMmayorXVersion(int infvercodi)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();

                var codigo = appIntervenciones.AgruparMantenimientoMayorXVersion(infvercodi, base.UserName);

                appIntervenciones.CopiarArchivoVersionFinal(codigo);

                model.Resultado = codigo.ToString();
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

        [HttpGet]
        public virtual FileResult ExportarReporte(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nameFile;
            // descargamos y borramos el archivo
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nameFile);
        }

        #endregion

        #region Consultas cruzadas

        public ActionResult ConsultasCruzadasF1F2(int infvercodi)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            FactorF1F2Model model = new FactorF1F2Model
            {
                Infvercodi = infvercodi
            };

            //buscamos la version y obtenemos su periodo y su F1 y F2
            var factorVersion = appIntervenciones.GetByIdInFactorVersion(infvercodi);

            decimal F1 = factorVersion.Infverf1 * 100;
            decimal F2 = factorVersion.Infverf2 * 100;

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ".";

            model.InfverF1 = F1.ToString("N", nfi) + "%";
            model.InfverF2 = F2.ToString("N", nfi) + "%";
            model.Version = factorVersion.Infvernro;
            model.NumMes = factorVersion.Infverfechaperiodo.Month;
            model.NumAnio = factorVersion.Infverfechaperiodo.Year;
            model.Infverfechaperiodo = factorVersion.Infverfechaperiodo.ToString(ConstantesAppServicio.FormatoFecha);

            //combo llenado de empresas
            appIntervenciones.ListarFiltroFactorF1F2(infvercodi, -1,
                                                ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                , out _
                                            , out List<SiEmpresaDTO> listaEmpresa
                                            , out List<EqAreaDTO> listaUbicacion
                                            , out List<EqEquipoDTO> listaEquipo);
            model.ListaEmpresa = listaEmpresa;
            model.ListaUbicacion = listaUbicacion;
            model.ListaEquipo = listaEquipo;

            return View(model);
        }

        /// <summary>
        /// Muestra la grilla excel para inervenciones Cruzadas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(FactorF1F2InputWeb objInput)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();
                DateTime fecha = DateTime.ParseExact(objInput.Infverfechaperiodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                objInput.HorasIndispo = "0";
                FactorF1F2Filtro objFiltro = GetFiltroConsultaWeb(objInput);

                objFiltro.FechaIni = fecha;
                objFiltro.FechaFin = fecha.AddMonths(1).AddDays(-1);

                model.GridExcel = appIntervenciones.ObtenerExcelConsultasCruzadaF1F2(objFiltro);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        private FactorF1F2Filtro GetFiltroConsultaWeb(FactorF1F2InputWeb input)
        {
            FactorF1F2Filtro obj = appIntervenciones.GetFiltroConsultaF1F2(input);

            return obj;
        }

        /// <summary>
        /// Genera un reporte Excel de las Consultas Cruzadas F1/F2
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoExcelReporteCruzado(int tipoReporte, int infvercodi, string idEmpresa, string ubicacion, string equipo)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                idEmpresa = idEmpresa ?? ConstantesAppServicio.ParametroDefecto;

                appIntervenciones.GenerarRptExcelReporteCruzadoF1F2(tipoReporte, ruta, infvercodi, idEmpresa, ubicacion, equipo, out string nameFile);

                model.Resultado = nameFile;

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

        #endregion

        #region Programado y Ejecutado - Justificación y sustento

        public ActionResult IndexProgramado(int infvercodi)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            InFactorVersionDTO inFactorVersionDTO = appIntervenciones.GetByIdInFactorVersion(infvercodi);
            appIntervenciones.ListarFiltroFactorF1F2(infvercodi, ConstantesIntervencionesAppServicio.HojaProgramadoMensual,
                                                ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                , out _
                                            , out List<SiEmpresaDTO> listaEmpresa
                                            , out List<EqAreaDTO> listaUbicacion
                                            , out List<EqEquipoDTO> listaEquipo);
            var model = new FactorF1F2Model()
            {
                Version = infvercodi,
                VersionDesc = inFactorVersionDTO.Infverfechaperiodo.ToString(ConstantesAppServicio.FormatoAnioMes),
                Mes = inFactorVersionDTO.Infverfechaperiodo.ToString(ConstantesAppServicio.FormatoFecha),
                Infverflagfinal = inFactorVersionDTO.Infverflagfinal,
                Hoja = ConstantesIntervencionesAppServicio.HojaProgramadoMensual,
                ListaEmpresa = listaEmpresa,
                ListaUbicacion = listaUbicacion,
                ListaEquipo = listaEquipo
            };

            return View(model);
        }

        public ActionResult IndexEjecutado(int infvercodi)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            InFactorVersionDTO inFactorVersionDTO = appIntervenciones.GetByIdInFactorVersion(infvercodi);
            appIntervenciones.ListarFiltroFactorF1F2(infvercodi, ConstantesIntervencionesAppServicio.HojaEjecutado,
                                                ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                , out _
                                            , out List<SiEmpresaDTO> listaEmpresa
                                            , out List<EqAreaDTO> listaUbicacion
                                            , out List<EqEquipoDTO> listaEquipo);
            var model = new FactorF1F2Model()
            {
                Version = infvercodi,
                VersionDesc = inFactorVersionDTO.Infverfechaperiodo.ToString(ConstantesAppServicio.FormatoAnioMes),
                Mes = inFactorVersionDTO.Infverfechaperiodo.ToString(ConstantesAppServicio.FormatoFecha),
                Infverflagfinal = inFactorVersionDTO.Infverflagfinal,
                Hoja = ConstantesIntervencionesAppServicio.HojaEjecutado,
                ListaEmpresa = listaEmpresa,
                ListaUbicacion = listaUbicacion,
                ListaEquipo = listaEquipo
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult ViewCargarFiltros(int infvercodi, int infmmhoja, string empresa, string ubicacion)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();

                appIntervenciones.ListarFiltroFactorF1F2(infvercodi, infmmhoja, empresa, ubicacion, ConstantesAppServicio.ParametroDefecto
                                                , out List<InFactorVersionMmayorDTO> listaDetalleXFiltro
                                                , out List<SiEmpresaDTO> listaEmpresa
                                                , out List<EqAreaDTO> listaUbicacion
                                                , out List<EqEquipoDTO> listaEquipo);

                model.ListaEmpresa = listaEmpresa;
                model.ListaUbicacion = listaUbicacion;
                model.ListaEquipo = listaEquipo;
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

        [HttpPost]
        public JsonResult ListarMmayorXFiltro(int infvercodi, int infmmhoja, string empresa, string ubicacion, string equipo, decimal similitud)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                this.ValidarSesionJsonResult();

                appIntervenciones.ListarFiltroFactorF1F2(infvercodi, infmmhoja, empresa, ubicacion, equipo
                                                , out List<InFactorVersionMmayorDTO> listaDetalleXFiltro
                                                , out List<SiEmpresaDTO> listaEmpresa
                                                , out List<EqAreaDTO> listaUbicacion
                                                , out List<EqEquipoDTO> listaEquipo);
                listaDetalleXFiltro = appIntervenciones.ListarSimilitudMMayor(listaDetalleXFiltro, similitud);

                model.ListInFactorVersionMmayor = listaDetalleXFiltro;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetMmayor(int infmmcodi)
        {
            FactorF1F2MmayorModel model = new FactorF1F2MmayorModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.Entidad = appIntervenciones.GetByIdInFactorVersionMmayor(infmmcodi);
                var listaArchivos = appIntervenciones.GetByCriteriaInArchivos(model.Entidad.Infvercodi, model.Entidad.Infmmhoja.ToString());
                model.Entidad.ListaArchivos = listaArchivos.Where(p => p.Infmmcodi == model.Entidad.Infmmcodi).ToList();

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

        [HttpPost]
        public JsonResult ActualizarVersionPreliminar(int infvercodi, List<GenericoDTO> listaMmayor)
        {
            FactorF1F2Model model = new FactorF1F2Model();
            try
            {
                this.ValidarSesionJsonResult();

                //actualizar descripciones de version preliminar
                appIntervenciones.ActualizarListadoDescripcion(infvercodi, listaMmayor, base.UserName);

                model.Resultado = "1";
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

        [HttpPost]
        public JsonResult ActualizarSustento(int infmmcodi, string sustento, List<InArchivoDTO> listaArhchivoWeb)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                this.ValidarSesionJsonResult();

                //Validación
                List<string> listaArchivoFisicoAGuardar = new List<string>();
                if (listaArhchivoWeb == null) { listaArhchivoWeb = new List<InArchivoDTO>(); }
                sustento = (sustento ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(sustento))
                {
                    throw new Exception("Debe ingresar el sustento.");
                }

                //Actualizamos el sustento
                appIntervenciones.ActualizarSustentoF1F2(infmmcodi, sustento, base.UserName, listaArhchivoWeb, ref listaArchivoFisicoAGuardar);

                model.Resultado = "1";
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

        [HttpPost]
        public JsonResult GenerarArchivoExcelReporteHojaIndividual(int infvercodi, int infmmhoja, string empresa, string ubicacion, string equipo)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();

                appIntervenciones.ListarFiltroFactorF1F2(infvercodi, infmmhoja, empresa, ubicacion, equipo
                                                , out List<InFactorVersionMmayorDTO> listaDetalleXFiltro
                                                , out List<SiEmpresaDTO> listaEmpresa
                                                , out List<EqAreaDTO> listaUbicacion
                                                , out List<EqEquipoDTO> listaEquipo);

                appIntervenciones.GenerarRptExcelFactorHojaProgEjec(infmmhoja, listaDetalleXFiltro, out string nameFile);

                model.Resultado = nameFile;
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
        /// descargar el archivo en formato zip
        /// </summary>
        /// <param name="infvercodi"></param>
        /// <returns></returns>
        public JsonResult ExportarFormatoZip(int infvercodi)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();

                int numeroAleatorio = (int)DateTime.Now.Ticks;
                appIntervenciones.ComprimirSustentoVersionF1F2(infvercodi, numeroAleatorio.ToString(), out string nameFile);
                model.Resultado = nameFile;
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

        #endregion

        #region Control Upload

        /// <summary>
        /// Sube los archivos a una carpeta aislada para luego ser movida a la carpeta del id generado
        /// </summary>
        /// <param name="sFecha">Fecha</param>
        /// <param name="sModulo">Modulo</param>
        /// <returns>Json</returns>
        [HttpPost]
        public ActionResult UploadFileMmayor(string sModulo, int anioMes, int infmmcodi)
        {
            ArchivosModel model = new ArchivosModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];

                    DateTime fechaAhora = DateTime.Now;
                    appIntervenciones.UploadArchivoEnF1F2(sModulo, anioMes, infmmcodi, file.FileName, file.InputStream, fechaAhora, out string fileNamefisico);

                    return Json(new
                    {
                        success = true,
                        nuevonombre = fileNamefisico,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(new { success = model.Resultado == "1", response = model }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult DescargarArchivoMmayor(int anioMes, int infmmcodi, string fileName, string fileNameOriginal)
        {
            byte[] buffer = appIntervenciones.GetBufferArchivoArchivoMmayor(anioMes, infmmcodi, fileName);

            if (buffer != null)
            {
                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameOriginal);
            }

            return base.DescargarArchivoNoDisponible();
        }

        #endregion

        #region Dashboard

        [HttpGet]
        public ActionResult IndexDashboard(string fechaPeriodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            string strFecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            if (fechaPeriodo != null)
            {

                DateTime Mes = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                strFecha = Mes.ToString("MM yyyy");
            }

            var modelo = new FactorF1F2Model
            {
                Mes = strFecha
            };

            return View(modelo);
        }

        [HttpPost]
        public JsonResult ConstruirDashboard(string fecha)
        {
            var model = new FactorF1F2Model();
            DateTime fechaC = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

            model.Graficos = new List<GraficoWeb>();

            var graficoF1 = appIntervenciones.GenerarGwebTacometro(fechaC, 1);
            model.Graficos.Add(graficoF1);

            var graficoF2 = appIntervenciones.GenerarGwebTacometro(fechaC, 2);
            model.Graficos.Add(graficoF2);

            var graficoMensualF1 = appIntervenciones.GenerarGwebLineaMensual(fechaC, 1);
            model.Graficos.Add(graficoMensualF1);

            var graficoMensualF2 = appIntervenciones.GenerarGwebLineaMensual(fechaC, 2);
            model.Graficos.Add(graficoMensualF2);

            return Json(model);
        }

        /// <summary>
        /// Genera un reporte Excel del Dashboard
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoExcelDashboard(DateTime fechaInicio)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();

                appIntervenciones.GenerarArchivoExcelDashboard(fechaInicio, out string nameFile);

                model.Resultado = nameFile;
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

        [HttpGet]
        public virtual FileResult ExportarDashboard(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nameFile;
            // descargamos y borramos el archivo
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nameFile);
        }

        #endregion
    }
}