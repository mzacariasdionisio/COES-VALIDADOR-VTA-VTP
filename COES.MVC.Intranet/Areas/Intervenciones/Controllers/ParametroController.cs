using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Intervenciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Intervenciones.Controllers
{
    public class ParametroController : BaseController
    {
        readonly IntervencionesAppServicio intServicio = new IntervencionesAppServicio();
        readonly ParametroAppServicio parametroServicio = new ParametroAppServicio();
        readonly SeguridadServicioClient servSeguridad = new SeguridadServicioClient();

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

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            Intervencion model = new Intervencion
            {
                HoraEjecucion = this.intServicio.ObtenerHoraEjecucionIntervencionProgramadaNoEjec(),
                FormatoEjecExtranet = this.intServicio.ListarFormatosIntervenciones().Find(x => x.Formatcodi == ConstantesIntervencionesAppServicio.FormatoEjecutadoExtranet)
            };

            var objUsuario = this.servSeguridad.ObtenerUsuarioPorLogin(User.Identity.Name);
            model.TienePermisoDTI = objUsuario != null && objUsuario.AreaCode == 1;

            model.ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListaTiposProgramacion = model.ListaTiposProgramacion.Where(x => x.Evenclasecodi != 1).ToList();
            model.ParametroPlazo = new Parametro();
            //solo tipo diario y semanal
            model.ListaTipProgramFiltro = model.ListaTiposProgramacion.Where(x => x.Evenclasecodi == 2 || x.Evenclasecodi == 3).ToList();

            //Archivos
            model.PathPrincipal = this.intServicio.GetPathPrincipal();
            model.PathAplicativo = this.intServicio.GetPathPrincipal() + ConstantesIntervencionesAppServicio.FolderRaizIntervenciones;
            model.Resultado = string.Join("|", intServicio.ListarSubcarpetaFromPrincipal());

            return View(model);
        }

        #region Proceso automático

        /// <summary>
        /// Guardar hora de ejecucion
        /// </summary>
        /// <param name="horaejecucion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarHoraEjecucion(string horaejecucion)
        {
            Intervencion model = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                string[] separadas;
                separadas = horaejecucion.Split(':');
                int hora = Convert.ToInt32(separadas[0]);
                int minutos = Convert.ToInt32(separadas[1]);

                SiProcesoDTO proceso = new SiProcesoDTO
                {
                    Prschorainicio = hora,
                    Prscminutoinicio = minutos
                };

                intServicio.UpdateHoraEjecucion(proceso);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
        #endregion

        #region FileServer

        /// <summary>
        /// Permite mostrar los archivos relacionado a la subcarpeta
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="subcarpeta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerFolder(string subcarpeta)
        {
            Intervencion model = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();

                string path = this.intServicio.GetPathSubcarpeta(subcarpeta);
                model.PathSubcarpeta = path;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// cargar archivos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarArchivosAutomatico()
        {
            Intervencion model = new Intervencion();

            try
            {
                intServicio.CargarArchivosAutomatico();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        #region Parámetro de Reversión

        [HttpPost]
        public JsonResult ObtenerParametroPlazo(int tipoProgramacion)
        {
            Intervencion model = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();

                if (tipoProgramacion > 0)
                {
                    var regParam1 = intServicio.ObtenerParametro(tipoProgramacion);
                    int[] lParam1 = (regParam1.Siparvnota ?? "").Trim().Split('|').Select(x => Convert.ToInt32(x)).ToArray();

                    model.ParametroPlazo = new Parametro
                    {
                        DiaPlazo = lParam1[0],
                        MinutoPlazo = lParam1[1],

                        FechaConsulta = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaHora),

                        //datos de auditoría
                        UsuarioModificacion = regParam1.Siparvusucreacion,
                        FechaModificacion = regParam1.Siparvfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull)
                    };
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarParametroPlazoReversion(int diaPlazo, int minutoPlazo, int tipoProgramacion)
        {
            Intervencion modelResultado = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();
                SiParametroValorDTO param = new SiParametroValorDTO();

                var regParam1 = intServicio.ObtenerParametro(tipoProgramacion);
                param.Siparcodi = regParam1.Siparcodi;

                int[] lParam1 = new int[2];
                lParam1[0] = diaPlazo;
                lParam1[1] = minutoPlazo;

                param.Siparvnota = string.Join("|", lParam1);
                param.Siparveliminado = regParam1.Siparveliminado;
                param.Siparvusucreacion = base.UserName;
                param.Siparvfeccreacion = DateTime.Now;

                //Guardar en parámetro valor
                parametroServicio.SaveSiParametroValor(param);

                modelResultado.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResultado.Resultado = "-1";
                modelResultado.StrMensaje = ex.Message;
                modelResultado.Detalle = ex.StackTrace;
            }

            return Json(modelResultado);
        }

        #endregion

        #region Plantilla de correo

        public ActionResult IndexPlantillaCorreo()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            InPlantillaCorreo model = new InPlantillaCorreo
            {
                LogoEmail = ConstantesAppServicio.LogoCoesEmail
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult ListarPlantillaCorreo()
        {
            InPlantillaCorreo model = new InPlantillaCorreo();

            try
            {
                model.AccionGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoPlantillasCorreo = intServicio.ListarPlantillaCorreoIntervencion();
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
        public JsonResult ObtenerDetalleCorreo(int plantillacodi)
        {
            InPlantillaCorreo model = new InPlantillaCorreo();

            try
            {
                var objPlantilla = intServicio.GetByIdSiPlantillacorreo(plantillacodi);
                intServicio.AgregarHoraEstadoRecordatorio(ref objPlantilla);

                model.PlantillaCorreo = objPlantilla;
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
        public JsonResult GuardarPlantillaCorreo(SiPlantillacorreoDTO plantillaCorreo)
        {
            InPlantillaCorreo model = new InPlantillaCorreo();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //Actualizo la plantilla
                intServicio.ActualizarDatosPlantillaCorreo(plantillaCorreo, base.UserName);
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
        public JsonResult EjecutarRecordatorioInclusionExclusion(int progrcodi)
        {
            InPlantillaCorreo model = new InPlantillaCorreo();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                intServicio.EjecutarProcesoRecordatorioInclExcl(progrcodi);

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

        #region Aprobación automática

        [HttpPost]
        public JsonResult ObtenerAprobacionAuto(int tipoProgramacion)
        {
            Intervencion model = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();
                int codParametro = 0;

                if (tipoProgramacion == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoDiario)
                    codParametro = ConstantesIntervencionesAppServicio.IdParametroAprobacionDiario;

                if (tipoProgramacion == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoSemanal)
                    codParametro = ConstantesIntervencionesAppServicio.IdParametroAprobacionSemanal;

                var listaParametroValor = parametroServicio.ListSiParametroValorByIdParametro(codParametro).OrderByDescending(x => x.Siparvfeccreacion).ToList();

                if (listaParametroValor.Any())
                {
                    var regParam1 = listaParametroValor.First(); // toma el último elemento guardado

                    var entidad = intServicio.ObtenerProcesoAprobacionAuto(tipoProgramacion);
                    model.HoraEjecucion = entidad.Prschorainicio.ToString().PadLeft(2, '0') + ":" + entidad.Prscminutoinicio.ToString().PadLeft(2, '0');
                    model.Dia = entidad.NumDia.ToString();
                    model.ParametroPlazo = new Parametro
                    {
                        UsuarioModificacion = regParam1.Siparvusucreacion,
                        FechaModificacion = regParam1.Siparvfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull)
                    };
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarProcesoAprobacionAutomatica(int tipoProgramacion, int dia, string horaejecucion)
        {
            Intervencion modelResultado = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();

                SiParametroValorDTO param = new SiParametroValorDTO();

                if (tipoProgramacion == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoDiario)
                    param.Siparcodi = ConstantesIntervencionesAppServicio.IdParametroAprobacionDiario;

                if (tipoProgramacion == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoSemanal)
                    param.Siparcodi = ConstantesIntervencionesAppServicio.IdParametroAprobacionSemanal;


                var minutoPlazo = int.Parse(horaejecucion.Substring(0, 2)) * 60 + int.Parse(horaejecucion.Substring(3, 2));

                if (tipoProgramacion == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoDiario)
                    param.Siparvnota = minutoPlazo.ToString();

                if (tipoProgramacion == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoSemanal)
                {
                    var abrevDia = intServicio.ObtenerAbrevDiaSemana(dia);

                    string[] lParam1 = new string[2];
                    lParam1[0] = abrevDia;
                    lParam1[1] = minutoPlazo.ToString();
                    param.Siparvnota = string.Join("|", lParam1);
                }

                param.Siparveliminado = "N";
                param.Siparvusucreacion = base.UserName;
                param.Siparvfeccreacion = DateTime.Now;

                //Guardar en parámetro valor
                parametroServicio.SaveSiParametroValor(param);

                intServicio.UpdateDiaHoraEjecucion(tipoProgramacion, dia, horaejecucion);

                modelResultado.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResultado.Resultado = "-1";
                modelResultado.StrMensaje = ex.Message;
                modelResultado.Detalle = ex.StackTrace;
            }

            return Json(modelResultado);
        }

        [HttpPost]
        public JsonResult EjecutarAprobacionAutomatica(int progrcodi)
        {
            InPlantillaCorreo model = new InPlantillaCorreo();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var resultado = intServicio.EjecutarProcesoAutomaticoAprobacion(progrcodi);

                model.Resultado = resultado.ToString();
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

        #region Plantillas Informes Word

        public ActionResult ActualizaReporte()
        {
            IntervencionesReporte model = new IntervencionesReporte
            {
                ListaReportes = intServicio.ListInReporte()
            };

            return View(model);
        }

        /// <summary>
        /// Carga Los items del Reporte 
        /// </summary>
        /// <param name="id">Id del reporte "Inrepcodi"</param> 
        /// <returns>ActionResult</returns>     
        public ActionResult DetalleInformeListado(int id, int? progcodi)
        {
            IntervencionesReporte model = new IntervencionesReporte();
            InReporteDTO reporte = intServicio.ObtenerReportePorTipo(id, progcodi);
            model.ListaSecciones = reporte.ListaSecciones;
            model.Inrepcodi = id;
            model.NroTabs = model.ListaSecciones.Count;
            model.Inrepnombre = reporte.Inrepnombre;

            if (progcodi != null)
            {
                string nombrePrograma = (this.intServicio.ObtenerProgramacionesPorId((int)progcodi)).Nomprogramacion.Replace("Plan - ", " - ");
                model.NombrePrograma = nombrePrograma;
            }

            model.IndicadorPersonalizado = (progcodi != null) ? 1 : 0;
            model.Progrcodi = (progcodi != null) ? (int)progcodi : 0;
            model.ListaVariables = (progcodi != null) ? this.intServicio.ObtenerVariablesPorPrograma((int)progcodi, id) :
                new string[1][];

            return View(model);
        }

        [HttpPost]
        public JsonResult GuardarSecciones(List<InSeccionDTO> lista, int tipoReporte, int progcodi, string[][] variables)
        {
            Programacion model = new Programacion();

            try
            {
                base.ValidarSesionJsonResult();
                this.intServicio.SaveInReporte(tipoReporte, progcodi, lista, variables, base.UserName);

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

        #region Plantillas Inclusión / Exclusión

        public ActionResult IndexPlantilla()
        {
            InPlantillaSustento model = new InPlantillaSustento();

            return View(model);
        }

        [HttpPost]
        public JsonResult ListarPlantillas()
        {
            InPlantillaSustento model = new InPlantillaSustento();

            try
            {
                model.AccionGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoPlantillas = intServicio.ListarPlantillas();
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
        public JsonResult RequisitosListado(int inpstcodi)
        {
            InPlantillaSustento model = new InPlantillaSustento();

            try
            {
                base.ValidarSesionJsonResult();
                model.PlantillaSustento = new InSustentopltDTO();
                //model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.AccionGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.PlantillaSustento.Requisitos = intServicio.ListarRequisitos(inpstcodi);

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
        public JsonResult HistorialListado(int inpsttipo)
        {
            InPlantillaSustento model = new InPlantillaSustento();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoPlantillas = new List<InSustentopltDTO>();
                model.ListadoPlantillas = intServicio.ObtenerHistorial(inpsttipo);
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

        public JsonResult GuardarRequisitoPlantilla(string dataJson, int codPlantilla)
        {
            InPlantillaSustento model = new InPlantillaSustento();

            try
            {
                this.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                model.Requisito = new InSustentopltItemDTO();
                InSustentopltItemDTO miObj = serializer.Deserialize<InSustentopltItemDTO>(dataJson);
                model.Requisito = miObj;

                intServicio.GuardarRequisitoPlantilla(model.Requisito, codPlantilla, base.UserName);

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
        public JsonResult EliminarRequisito(int inpsticodi, int inpstcodi)
        {
            InPlantillaSustento model = new InPlantillaSustento();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                intServicio.EliminarRequisito(inpsticodi, inpstcodi, base.UserName);

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
        /// Actualiza el campo orden de los requisitos cuando se reordena el listado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="direction"></param>
        public void UpdateOrder(int codigo, int fromPosition, int toPosition, string direction)
        {
            intServicio.ActualizarOrdenRequisitos(codigo, fromPosition, toPosition, direction);
        }

        #endregion

        #region Porcentaje similitud

        [HttpPost]
        public JsonResult ObtenerPorcentajeSimilitud()
        {
            Intervencion model = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();

                var listaParametroValor = parametroServicio.ListSiParametroValorByIdParametro(ConstantesIntervencionesAppServicio.IdParametroPorcentajeSimilitud).OrderByDescending(x => x.Siparvfeccreacion).ToList();

                if (listaParametroValor.Any())
                {
                    var regParam1 = listaParametroValor.First(); // toma el último elemento guardado

                    model.ParametroPlazo = new Parametro
                    {
                        ValorPorcentaje = regParam1.Siparvnota,
                        UsuarioModificacion = regParam1.Siparvusucreacion,
                        FechaModificacion = regParam1.Siparvfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull)
                    };
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarPorcentajeSimilitud(string valor)
        {
            Intervencion modelResultado = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();

                SiParametroValorDTO param = new SiParametroValorDTO
                {
                    Siparcodi = ConstantesIntervencionesAppServicio.IdParametroPorcentajeSimilitud,

                    Siparvnota = valor,
                    Siparveliminado = "N",
                    Siparvusucreacion = base.UserName,
                    Siparvfeccreacion = DateTime.Now
                };

                //Guardar en parámetro valor
                parametroServicio.SaveSiParametroValor(param);

                modelResultado.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResultado.Resultado = "-1";
                modelResultado.StrMensaje = ex.Message;
                modelResultado.Detalle = ex.StackTrace;
            }

            return Json(modelResultado);
        }

        #endregion
    }
}