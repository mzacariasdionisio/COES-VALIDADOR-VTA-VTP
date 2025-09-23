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
    public class DisponibilidadCombustibleController : BaseController
    {
        readonly YupanaAppServicio servicioYup = new YupanaAppServicio();

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

        #region Metodos

        /// <summary>
        /// Evento principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //base.ValidarSesionUsuario();
            base.ValidarSesionJsonResult();

            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();

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

            model.ListaSemana = new List<TipoInformacion>();


            return View(model);
        }

        /// <summary>
        /// Devuelve combo semanas
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho)
        {
            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();

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

        /// <summary>
        /// Carga las semanas para el popup
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanasPopup(string idAnho)
        {
            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();

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

        /// <summary>
        /// Devuielve listado de restricciones para cierto filtro
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarRestricciones(int formato, string fecha, string semana)
        {
            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                DateTime fechaProceso = new DateTime();

                int formatoVentana = servicioYup.ObtenerFormatoSegunVentana(formato, ConstantesYupana.TipoRestriccionDispComb);

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                if (formatoVentana != ConstantesYupana.FormatoDispCombSemanal) // diario reprograma
                    fechaProceso = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                else
                    fechaProceso = EPDate.GetFechaIniPeriodo(2, string.Empty, semana, fecha, Constantes.FormatoFecha);

                //carga listado de restricciones para el handson
                DatoRestricciones data = servicioYup.ObtenerDatosRestriccionesDispComb(formatoVentana, fechaProceso);

                model.DataRestricciones = data;
                model.IdEnvio = data.IdEnvio;
                model.ListaEnvios = servicioYup.ObtenerListadoEnvios(formatoVentana, fechaProceso, ConstantesYupana.TipoRestriccionDispComb);


                DateTime? fechaEnvio = data.IdEnvio != null ? (data.IdEnvio > 0 ? servicioYup.GetByIdPrRestricEnvio(data.IdEnvio).Resenvfeccreacion : (DateTime?)null) : (DateTime?)null;
                model.FechaProceso = fechaEnvio != null ? fechaEnvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            //Log.Info($"Serializando: {JsonConvert.SerializeObject(model, Formatting.Indented)}");
            return Json(model);
        }

        /// <summary>
        /// Muestra datos de envio
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarEnvio(int idEnvio)
        {
            GeneracionMetaModel model = new GeneracionMetaModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //carga listado de restricciones para el handson
                DatoRestricciones data = servicioYup.ObtenerDatosRestriccionesDispCombPorEnvio(idEnvio);

                model.DataRestricciones = data;
                model.IdEnvio = idEnvio;

                DateTime? fechaEnvio = servicioYup.GetByIdPrRestricEnvio(idEnvio).Resenvfeccreacion;
                model.FechaProceso = fechaEnvio != null ? fechaEnvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";

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
        /// Obtiene el rango de fechas para cierta semana (SOLO EN SEMANAL)
        /// </summary>
        /// <param name="numSemana"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
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
                {
                    throw new ArgumentException("No se puede obtener el rango de fechas en el filtro.");
                }

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
        /// Envia datos a la BD
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="datosAGuardar"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarDatosDispCombustible(int horizonte, string fecha, string semana, DatoRestriccionesWeb datosAGuardar)
        {


            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();

            try
            {
                //base.ValidarSesionUsuario();
                base.ValidarSesionJsonResult();
                DateTime fechaProceso = new DateTime();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int formatoVentana = servicioYup.ObtenerFormatoSegunVentana(horizonte, ConstantesYupana.TipoRestriccionDispComb);

                if (formatoVentana != ConstantesYupana.FormatoDispCombSemanal) // diario reprograma
                    fechaProceso = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                else
                    fechaProceso = EPDate.GetFechaIniPeriodo(ConstantesYupana.PeriodoSemanal, string.Empty, semana, fecha, Constantes.FormatoFecha);

                if (datosAGuardar != null)
                {
                    var codeEnvio = servicioYup.GuardarDatosRest(formatoVentana, fechaProceso, datosAGuardar, ConstantesYupana.TipoRestriccionDispComb, base.UserName);
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

        /// <summary>
        /// Copia registos de otras fechas
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <returns></returns>              

        [HttpPost]
        public JsonResult CopiarDatosDC(string fechaVentana, int caso, int horizonteC, string fechaC, string anioC, string semanaC, string fechaOriIniC, string fechaOriFinC, string fechaDestIniC, string fechaDestFinC)
        {
            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();

            try
            {
                int asd = caso;
                int asd2 = horizonteC;
                string asd3 = fechaC;
                string asd4 = anioC;
                string asd5 = semanaC;
                string asd6 = fechaOriIniC;
                string asd7 = fechaOriFinC;
                string asd8 = fechaDestIniC;
                string asd9 = fechaDestFinC;

                base.ValidarSesionJsonResult();
                DateTime fechaProceso = new DateTime();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int formatoVentana = servicioYup.ObtenerFormatoSegunVentana(horizonteC, ConstantesYupana.TipoRestriccionDispComb);

                if (formatoVentana != ConstantesYupana.FormatoDispCombSemanal) // diario reprograma
                    fechaProceso = DateTime.ParseExact(fechaC, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                else
                    fechaProceso = EPDate.GetFechaIniPeriodo(ConstantesYupana.PeriodoSemanal, string.Empty, semanaC, fechaC, Constantes.FormatoFecha);

                //carga listado de restricciones para el handson

                DatoRestricciones data = servicioYup.ObtenerDatosRestriccionesCopiar(fechaVentana, formatoVentana, fechaProceso, ConstantesYupana.TipoRestriccionDispComb, fechaOriIniC, fechaOriFinC, fechaDestIniC, fechaDestFinC, caso);

                model.DataRestricciones = data;

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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarIndex()
        {
            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();
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

        #endregion

        #region CONFIGURACION DE DC

        /// <summary>
        /// Principal
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfiguracionIndex()
        {
            DisponibilidadCombustibleModel model = new DisponibilidadCombustibleModel();
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
                model.Recursos = servicioYup.ObtenerRecursos(ConstantesYupana.TipoRestriccionDispComb);
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
                model.ListaModosOpCOES = servicioYup.ListarModosOperacionCOES(ConstantesYupana.catecodiModo);

                if (rescfgcodi > 0)
                    model.Recurso = servicioYup.GetByIdPrRestricCfg(rescfgcodi);

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
        public JsonResult GuardarRecurso(int rescfgcodi, string nombre)
        //public JsonResult GuardarRecurso(int rescfgcodi, string nombre, string ecuacion)
        {
            RestriccionModel model = new RestriccionModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<PrRestricCfgDTO> listadoRecursos = servicioYup.GetByCriteriaPrRestricCfgs(ConstantesYupana.TipoRestriccionDispComb.ToString());
                listadoRecursos = listadoRecursos.Where(x => x.Rescfgestado == ConstantesAppServicio.Activo).ToList();
                var listadoValid = listadoRecursos.Where(x => x.Restipcodi == ConstantesYupana.TipoRestriccionDispComb && x.Rescfgcodi != rescfgcodi).ToList();

                //Valido que no exista duplicados
                PrRestricCfgDTO duplicado = listadoValid.Find(x => x.Rescfgnombre.Trim() == nombre.Trim());
                if (duplicado != null)
                    throw new ArgumentException("Ya existe un registro de Disponibilidad de Combustible con el mismo nombre ingresado.");

                int idRecurso = 0;
                if (rescfgcodi == 0)  //Guarda Nuevo
                {
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    //model.Recurso.Rescfgtipoecuacion = ecuacion ?? "";
                    model.Recurso.Restipcodi = ConstantesYupana.TipoRestriccionDispComb;
                    model.Recurso.Rescfgestado = ConstantesAppServicio.Activo;
                    model.Recurso.Rescfgusucreacion = base.UserName;
                    model.Recurso.Rescfgfeccreacion = DateTime.Now;

                    // Graba los cambio en la BD
                    idRecurso = servicioYup.SavePrRestricCfg(model.Recurso);
                }
                else  //Actualiza registro
                {
                    model.Recurso = listadoRecursos.Find(x => x.Rescfgcodi == rescfgcodi);
                    //capturar valores
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    //model.Recurso.Rescfgtipoecuacion = ecuacion ?? "";
                    model.Recurso.Rescfgusumodificacion = base.UserName;
                    model.Recurso.Rescfgfecmodificacion = DateTime.Now;

                    // Graba los cambio en la BD
                    servicioYup.UpdatePrRestricCfg(model.Recurso);
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
                servicioYup.ActualizarEstadoBaja(rescfgcodi, usuario);

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

                model.Recurso = new PrRestricCfgDTO();
                model.NombreR = "";
                if (rescfgcodi > 0)
                {
                    model.Recurso = servicioYup.GetByIdPrRestricCfg(rescfgcodi);
                    model.NombreR = model.Recurso != null ? model.Recurso.Rescfgnombre : "";
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
                List<PrRestricCfgDTO> listaModosOperacion = servicioYup.ObtenerRecursos(ConstantesYupana.TipoModoOp);
                //List<PrRestricCfgDTO> listaUnidadesHidro = servicio.ObtenerRecursos(ConstantesYupana.TipoHidro);
                //List<PrRestricCfgDTO> listaRER = servicio.ObtenerRecursos(ConstantesYupana.TipoRer);

                model.TermoGenerales = servicioYup.ListarModosGeneral(rescfgcodi, listaModosOperacion);
                //model.HidroGenerales = servicio.ListarModosGeneral(rescfgcodi, listaUnidadesHidro);
                //model.RerGenerales = servicio.ListarModosGeneral(rescfgcodi, listaRER);
                model.RecursosTermo = servicioYup.ObtenerRelaciones(rescfgcodi, listaModosOperacion);
                //model.RecursosHidro = servicio.ObtenerRelaciones(rescfgcodi, listaUnidadesHidro);
                //model.RecursosRer = servicio.ObtenerRelaciones(rescfgcodi, listaRER);

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
        /// Guardar data de Generación Meta
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="rescfgcodi"></param>
        /// <param name="listaRelacionesHidro"></param>
        /// <param name="listaRelacionesTermo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarDatoYRelaciones(string nombre, int rescfgcodi, List<PrRestricCfgdetDTO> listaRelacionesTermo)
        {
            RestriccionModel model = new RestriccionModel();
            model.Recurso = new PrRestricCfgDTO();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<PrRestricCfgdetDTO> listaRelaciones = new List<PrRestricCfgdetDTO>();


                if (listaRelacionesTermo != null && listaRelacionesTermo.Any())
                {
                    listaRelacionesTermo.ForEach(item => item.Resdetfuente = "G");
                    listaRelaciones.AddRange(listaRelacionesTermo);
                }

                List<PrRestricCfgDTO> listadoRecursos = servicioYup.GetByCriteriaPrRestricCfgs(ConstantesYupana.TipoRestriccionDispComb.ToString());
                listadoRecursos = listadoRecursos.Where(x => x.Rescfgestado == ConstantesAppServicio.Activo).ToList();

                string msgValidacion = servicioYup.ValidarRestriccion(nombre, rescfgcodi, listadoRecursos, new List<PrRestricCfgdetDTO>(), listaRelacionesTermo, ConstantesYupana.TipoRestriccionDispComb, listaRelaciones);
                if (msgValidacion != "")
                    throw new ArgumentException(msgValidacion);

                #region Nombre
                int idRecurso = 0;
                if (rescfgcodi == 0)  //Guarda Nuevo
                {
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    //model.Recurso.Rescfgtipoecuacion = ecuacion ?? "";
                    model.Recurso.Restipcodi = ConstantesYupana.TipoRestriccionDispComb;
                    model.Recurso.Rescfgestado = ConstantesAppServicio.Activo;
                    model.Recurso.Rescfgusucreacion = base.UserName;
                    model.Recurso.Rescfgfeccreacion = DateTime.Now;

                    // Graba los cambio en la BD
                    idRecurso = servicioYup.SavePrRestricCfg(model.Recurso);
                }
                else  //Actualiza registro
                {
                    model.Recurso = listadoRecursos.Find(x => x.Rescfgcodi == rescfgcodi);
                    //capturar valores
                    model.Recurso.Rescfgnombre = nombre ?? "";
                    //model.Recurso.Rescfgtipoecuacion = ecuacion ?? "";
                    model.Recurso.Rescfgusumodificacion = base.UserName;
                    model.Recurso.Rescfgfecmodificacion = DateTime.Now;

                    // Graba los cambio en la BD
                    servicioYup.UpdatePrRestricCfg(model.Recurso);
                    idRecurso = model.Recurso.Rescfgcodi;
                }
                #endregion

                #region Relaciones

                servicioYup.GuardarRelacionesConValidaciones(idRecurso, listaRelaciones, base.UserName);
                #endregion

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

                List<PrRestricCfgDTO> listadoRecursos = servicioYup.GetByCriteriaPrRestricCfgs(ConstantesYupana.TipoRestriccionDispComb.ToString());
                listadoRecursos = listadoRecursos.Where(x => x.Rescfgestado == ConstantesAppServicio.Activo).ToList();
                var listadoValid = listadoRecursos;

                //Valido que no exista duplicados
                PrRestricCfgDTO duplicado = listadoValid.Find(x => x.Rescfgnombre.Trim() == nombre.Trim());
                if (duplicado != null)
                    throw new ArgumentException("Ya existe un registro de Disponibilidad de Combustible con el mismo nombre ingresado.");

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
                    idRecurso = servicioYup.SavePrRestricCfg(model.Recurso);

                    //Guarda sus relaciones
                    foreach (var item in model.Recurso.ListaRelaciones)
                    {
                        item.Resdetcodi = 0;
                        item.Rescfgcodi = idRecurso; //nuevo registro
                        servicioYup.SavePrRestricCfgdet(item);
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

        #region DESCARGA Y CARGA DE PLANTILLA

        /// <summary>
        /// Exportar Restricción DC
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

                string fileName = ConstantesYupana.NombrePlantillaExcelRestriccionDC;
                string pathOrigen = ConstantesYupana.FolderRaizYupana + ConstantesYupana.Plantilla;
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesYupana.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, pathDestino, fileName, null);
                servicioYup.GenerarExcelRestriccionDC(pathDestino, fileName);

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
                DatoRestricciones data = servicioYup.LeerExcelPerfilDC(path, this.NombreFile, base.UserName);
                model.DataRestricciones = data;
                model.Resultado = "1";
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

        #endregion
    }

}