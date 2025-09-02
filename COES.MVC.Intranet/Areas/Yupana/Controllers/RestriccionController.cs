using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.Yupana.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.Yupana;
using COES.Servicios.Aplicacion.Yupana.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Yupana.Controllers
{
    public class RestriccionController : BaseController
    {
        readonly YupanaAppServicio servicio = new YupanaAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public String NombreFile
        {
            get
            {
                return (Session[ConstHidrologia.SesionNombreArchivo] != null) ?
                    Session[ConstHidrologia.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstHidrologia.SesionNombreArchivo] = value; }
        }

        #endregion

        public ActionResult Index()
        {
            RestriccionModel model = new RestriccionModel();

            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            //model.ListaSemanas = semanas;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);

            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaIniSem = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFinSem = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }
        public PartialViewResult CargarSemanas(string idAnho)
        {
            RestriccionModel model = new RestriccionModel();
            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemana = entitys;
            return PartialView(model);
        }

        public PartialViewResult CargarSemanasPopup(string idAnho)
        {
            RestriccionModel model = new RestriccionModel();

            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemana = entitys;

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ObtenerRangoFechasSemana(int numSemana, int anio)
        {
            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();
            try
            {
                List<TipoInformacion> entitys = new List<TipoInformacion>();
                if (numSemana != 0 && anio != 0)
                {
                    DateTime fechaIni = COES.Base.Tools.Util.GenerarFecha(anio, numSemana, ConstantesHidrologia.SemanalProgramado);
                    DateTime fechaFin = COES.Base.Tools.Util.GenerarFecha(anio, numSemana + 1, ConstantesHidrologia.SemanalProgramado);
                    fechaFin = fechaFin.AddDays(-1);

                    model.FechaIniSem = fechaIni.ToString(Constantes.FormatoFecha);
                    model.FechaFinSem = fechaFin.ToString(Constantes.FormatoFecha);
                }
                else
                    throw new ArgumentException("No se puede obtener el rango de fechas en el filtro.");
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
        /// Carga la información de condiciones iniciales
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarRestricciones(int formato, string fecha, string semana)
        {
            RestriccionModel model = new RestriccionModel();
            DateTime fechaProceso = new DateTime();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int formatoVentana = servicio.ObtenerFormatoSegunVentana(formato, ConstantesYupana.TipoRestriccionGen);

                if (formatoVentana != ConstantesYupana.FormatoRestGenSemanal) // diario reprograma
                    fechaProceso = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                else
                    fechaProceso = EPDate.GetFechaIniPeriodo(2, string.Empty, semana, fecha, Constantes.FormatoFecha);

                DatoRestricciones data = servicio.ObtenerDatosRestriccionesGeneracion(formatoVentana, fechaProceso);

                model.DataRestricciones = data;
                model.IdEnvio = data.IdEnvio;

                //OBTENER ENVIO
                var envio = servicio.GetByIdPrRestricEnvio(data.IdEnvio);
                model.FechaProceso = envio != null ? envio.Resenvfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";


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
        public JsonResult CopiarIndex()
        {
            RestriccionModel model = new RestriccionModel();
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            //model.ListaSemanas = semanas;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);

            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.Resultado = "1";
            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarRestricciones(int formato, string fecha, string semana, DatoRestricciones datosAGuardar)
        {
            RestriccionModel model = new RestriccionModel();
            DateTime fechaProceso = new DateTime();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int formatoVentana = servicio.ObtenerFormatoSegunVentana(formato, ConstantesYupana.TipoRestriccionGen);

                if (formatoVentana != ConstantesYupana.FormatoRestGenSemanal) // diario reprograma
                    fechaProceso = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                else
                    fechaProceso = EPDate.GetFechaIniPeriodo(2, string.Empty, semana, fecha, Constantes.FormatoFecha);

                if (datosAGuardar != null)
                {
                    var codeEnvio = servicio.EnviarDatosRestriccion(formatoVentana, fechaProceso, datosAGuardar, base.UserName);
                    model.IdEnvio = codeEnvio;
                    model.FechaProceso = fechaProceso.ToString(ConstantesAppServicio.FormatoFecha);
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        #region DESCARGA Y CARGA DE PLANTILLA

        /// <summary>
        /// ExportarRestricción
        /// </summary>
        /// <param name="famcodi"></param>
        /// <param name="fichaTecnica"></param>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarRestriccion()
        {
            RestriccionModel model = new RestriccionModel();

            try
            {
                base.ValidarSesionJsonResult();

                string fileName = ConstantesYupana.NombrePlantillaExcelRestriccion;
                string pathOrigen = ConstantesYupana.FolderRaizYupana + ConstantesYupana.Plantilla;
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesYupana.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);
                servicio.GenerarExcelRestriccion(pathDestino, fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
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
        /// AbrirArchivo
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");

            return DescargarArchivoTemporalYEliminarlo(AppDomain.CurrentDomain.BaseDirectory + ConstantesYupana.RutaReportes, file, sFecha + "_" + file);
        }

        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesYupana.RutaReportes;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + ".xlsx";

                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ImportarRestriccionesExcel(string fileName)
        {
            RestriccionModel model = new RestriccionModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesYupana.RutaReportes;

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                DatoRestricciones data = servicio.LeerExcelPerfilGeneracion(path, this.NombreFile, base.UserName);
                model.DataRestricciones = data;
                model.Resultado = "1";
                model.Mensaje = "¡La Información se grabó correctamente!";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        #endregion

        #region Configuración

        public ActionResult ConfiguracionIndex()
        {
            RestriccionModel model = new RestriccionModel();
            return View(model);
        }

        /// <summary>
        /// Obtener Lista de recursos por tipo
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEntidadXTipo()
        {
            RestriccionModel model = new RestriccionModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.Recursos = servicio.ObtenerRecursos(ConstantesYupana.TipoRestriccionGen);
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
        /// Obtener datos de un recurso
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerRecurso(int rescfgcodi)
        {
            RestriccionModel model = new RestriccionModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.Recurso = new PrRestricCfgDTO();
                model.ListaModosOpCOES = servicio.ListarModosOperacionCOES(ConstantesYupana.catecodiModo);

                if (rescfgcodi > 0)
                    model.Recurso = servicio.GetByIdPrRestricCfg(rescfgcodi);

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
        /// Guardar recurso
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <param name="restipcodi"></param>
        /// <param name="codmodo"></param>
        /// <param name="nombre"></param>
        /// <param name="rescfges"></param>
        /// <param name="rescfgfs"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRecurso(int rescfgcodi, string nombre, string ecuacion)
        {
            RestriccionModel model = new RestriccionModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<PrRestricCfgDTO> listadoRecursos = servicio.GetByCriteriaPrRestricCfgs(ConstantesYupana.TipoRestriccionGen.ToString());
                listadoRecursos = listadoRecursos.Where(x => x.Rescfgestado == ConstantesAppServicio.Activo).ToList();
                var listadoValid = listadoRecursos.Where(x => x.Restipcodi == ConstantesYupana.TipoRestriccionGen && x.Rescfgcodi != rescfgcodi).ToList();

                //Valido que no exista duplicados
                PrRestricCfgDTO duplicado = listadoValid.Find(x => x.Rescfgnombre.Trim() == nombre.Trim());
                if (duplicado != null)
                    throw new ArgumentException("Existe recurso con el mismo nombre.");

                int idRecurso = 0;
                if (rescfgcodi == 0)
                {
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    model.Recurso.Rescfgtipoecuacion = ecuacion ?? "";
                    model.Recurso.Restipcodi = ConstantesYupana.TipoRestriccionGen;
                    model.Recurso.Rescfgestado = ConstantesAppServicio.Activo;
                    model.Recurso.Rescfgusucreacion = base.UserName;
                    model.Recurso.Rescfgfeccreacion = DateTime.Now;

                    // Graba los cambio en la BD
                    idRecurso = servicio.SavePrRestricCfg(model.Recurso);
                }
                else
                {
                    model.Recurso = listadoRecursos.Find(x => x.Rescfgcodi == rescfgcodi);
                    //capturar valores
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    model.Recurso.Rescfgtipoecuacion = ecuacion ?? "";
                    model.Recurso.Rescfgusumodificacion = base.UserName;
                    model.Recurso.Rescfgfecmodificacion = DateTime.Now;

                    // Graba los cambio en la BD
                    servicio.UpdatePrRestricCfg(model.Recurso);
                    idRecurso = model.Recurso.Rescfgcodi;
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
        /// Eliminar recurso
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarRecurso(int rescfgcodi)
        {
            RestriccionModel model = new RestriccionModel();
            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;
                servicio.ActualizarEstadoBaja(rescfgcodi, usuario);

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
        public PartialViewResult CargarRelaciones(int rescfgcodi)
        {
            RestriccionModel model = new RestriccionModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Cragar y listar relaciones
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarModosRelacionados(int rescfgcodi)
        {
            RestriccionModel model = new RestriccionModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                // modos de operación
                List<PrRestricCfgDTO> listaRestricciones = servicio.GetByCriteriaPrRestricCfgs(ConstantesYupana.TiposDataMestra).Where(x => x.Rescfgestado == ConstantesAppServicio.Activo).ToList();

                List<PrRestricCfgDTO> listaModosOperacion = listaRestricciones.Where(x => x.Restipcodi == ConstantesYupana.TipoModoOp).ToList();
                List<PrRestricCfgDTO> listaUnidadesHidro = listaRestricciones.Where(x => x.Restipcodi == ConstantesYupana.TipoHidro).ToList();
                List<PrRestricCfgDTO> listaRER = listaRestricciones.Where(x => x.Restipcodi == ConstantesYupana.TipoRer).ToList();

                model.TermoGenerales = servicio.ListarModosGeneral(rescfgcodi, listaModosOperacion);
                model.HidroGenerales = servicio.ListarModosGeneral(rescfgcodi, listaUnidadesHidro);
                model.RerGenerales = servicio.ListarModosGeneral(rescfgcodi, listaRER);
                model.RecursosTermo = servicio.ObtenerRelaciones(rescfgcodi, listaModosOperacion);
                model.RecursosHidro = servicio.ObtenerRelaciones(rescfgcodi, listaUnidadesHidro);
                model.RecursosRer = servicio.ObtenerRelaciones(rescfgcodi, listaRER);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Guardar relaciones
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <param name="listaRelaciones"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRelaciones(int rescfgcodi, List<PrRestricCfgdetDTO> listaRelacionesHidro, List<PrRestricCfgdetDTO> listaRelacionesTermo, List<PrRestricCfgdetDTO> listaRelacionesRer)
        {
            RestriccionModel model = new RestriccionModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<PrRestricCfgdetDTO> listaRelaciones = new List<PrRestricCfgdetDTO>();
                if (listaRelacionesHidro != null) listaRelaciones.AddRange(listaRelacionesHidro);
                if (listaRelacionesTermo != null) listaRelaciones.AddRange(listaRelacionesTermo);
                if (listaRelacionesRer != null) listaRelaciones.AddRange(listaRelacionesRer);

                servicio.GuardarRelaciones(rescfgcodi, listaRelaciones, base.UserName);

                model.Resultado = "1";

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Guardar recurso
        /// </summary>
        /// <param name="rescfgcodi"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DuplicarRecurso(int rescfgcodi, string nombre)
        {
            RestriccionModel model = new RestriccionModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<PrRestricCfgDTO> listadoRecursos = servicio.GetByCriteriaPrRestricCfgs(ConstantesYupana.TipoRestriccionGen.ToString());
                listadoRecursos = listadoRecursos.Where(x => x.Rescfgestado == ConstantesAppServicio.Activo).ToList();
                var listadoValid = listadoRecursos;

                //Valido que no exista duplicados
                PrRestricCfgDTO duplicado = listadoValid.Find(x => x.Rescfgnombre.Trim() == nombre.Trim());
                if (duplicado != null)
                    throw new ArgumentException("Existe recurso con el mismo nombre.");

                int idRecurso = 0;
                if (rescfgcodi > 0)
                {
                    model.Recurso = listadoRecursos.Find(x => x.Rescfgcodi == rescfgcodi);
                    //capturar valores
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    model.Recurso.Rescfgestado = ConstantesAppServicio.Activo;
                    model.Recurso.Rescfgusucreacion = base.UserName;
                    model.Recurso.Rescfgfeccreacion = DateTime.Now;

                    // Graba los cambio en la BD
                    idRecurso = servicio.SavePrRestricCfg(model.Recurso);

                    //Guarda sus relaciones
                    foreach (var item in model.Recurso.ListaRelaciones)
                    {
                        item.Resdetcodi = 0;
                        item.Rescfgcodi = idRecurso; //nuevo registro
                        servicio.SavePrRestricCfgdet(item);
                    }
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

        #endregion

    }
}