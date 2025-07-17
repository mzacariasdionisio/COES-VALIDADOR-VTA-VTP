using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class ConfiguracionController : BaseController
    {
        readonly SubastasAppServicio servicio = new SubastasAppServicio();
        readonly MigracionesAppServicio servMigraciones = new MigracionesAppServicio();
        readonly DespachoAppServicio servDespacho = new DespachoAppServicio();

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

        public ConfiguracionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Parámetro Generales (Osinergmin)

        public ActionResult Default()
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Permite mostrar la lista de los parámetros de configuración
        /// </summary>
        /// <param name="fecha">fecha de consulta</param>        
        /// <returns></returns>
        public PartialViewResult ListadoParametros(string fechaConsulta)
        {
            SmaConfiguracionModel conf = new SmaConfiguracionModel();
            DateTime fecha = new DateTime();

            if (fechaConsulta != "")
            {
                fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            else
            {
                fecha = DateTime.Now;
            }

            var listaData = this.servicio.ListarParametrosConfiguracionPorFecha(fecha, ConstantesSubasta.ConcepcodiConfig);
            conf.ListaParametros = listaData;
            conf.TienePermiso = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            return PartialView(conf);
        }

        /// <summary>
        /// Listar historico de grupocodi y concepto
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHistoricoParametros(int grupocodi, int concepcodi, int opedicion)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            model.AccesoEditar = opedicion == 2 ? base.VerificarAccesoAccion(Acciones.Editar, base.UserName) : false;
            model.ListaParametros = servMigraciones.ListarGrupodatHistoricoValores(concepcodi, grupocodi);

            foreach (var reg in model.ListaParametros)
            {
                DateTime fechaDatDesc = DateTime.ParseExact(reg.FechadatDesc, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                reg.FechadatDesc = fechaDatDesc.Date.ToString(Constantes.FormatoFecha);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Registrar/editar parámetros configuración
        /// </summary>
        /// <param name="deleted"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupodatGuardar(int tipoAccion, int grupocodi, int concepcodi, string strfechaDat, string formuladat, int deleted)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //Validaciones

                if (concepcodi <= 0)
                {
                    throw new Exception("Debe seleccionar un parámetro.");
                }

                if (deleted != 0)
                {
                    throw new Exception("El registro ya ha sido eliminado, no puede modificarse.");
                }

                //Guardar
                if (tipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    PrGrupodatDTO reg = new PrGrupodatDTO();
                    reg.Grupocodi = grupocodi;
                    reg.Formuladat = formuladat;
                    reg.Concepcodi = concepcodi;
                    reg.Fechadat = fechaDat;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;
                    reg.Deleted = ConstantesSubasta.GrupodatActivo;

                    this.servDespacho.SavePrGrupodat(reg);
                }
                if (tipoAccion == ConstantesSubasta.AccionEditar)
                {
                    PrGrupodatDTO reg = this.servDespacho.GetByIdPrGrupodat(fechaDat, concepcodi, grupocodi, ConstantesSubasta.GrupodatActivo);
                    if (reg == null)
                    {
                        throw new Exception("El registro no existe, no puede modificarse.");
                    }
                    reg.Formuladat = formuladat;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;

                    this.servDespacho.UpdatePrGrupodat(reg);
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
        /// Eliminar grupodat
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupodatEliminar(int grupocodi, int concepcodi, string strfechaDat, int deleted)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                PrGrupodatDTO reg = this.servDespacho.GetByIdPrGrupodat(fechaDat, concepcodi, grupocodi, ConstantesSubasta.GrupodatActivo);

                if (reg == null)
                {
                    throw new Exception("El registro no existe o ha sido eliminada.");
                }

                reg.Lastuser = User.Identity.Name;
                reg.Fechaact = DateTime.Now;
                reg.Deleted2 = ConstantesSubasta.GrupodatInactivo;

                this.servDespacho.UpdatePrGrupodat(reg);

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

        #region Motivos

        /// <summary>
        /// listado de motivos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoMotivo()
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            try
            {
                this.ValidarSesionJsonResult();

                model.ListaMotivo = servicio.ListarMaestroMotivoOfDefecto();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guardar nuevo o edición de motivo
        /// </summary>
        /// <param name="smammcodi"></param>
        /// <param name="motivo"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public JsonResult GuardarMotivo(int smammcodi, string motivo, string estado)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            try
            {
                this.ValidarSesionJsonResult();

                SmaMaestroMotivoDTO obj = new SmaMaestroMotivoDTO()
                {
                    Smammcodi = smammcodi,
                    Smammdescripcion = motivo,
                    Smammestado = estado
                };
                servicio.GuardarMotivo(obj, base.UserName);

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

        #endregion

        #region URS Calificadas

        public ActionResult IndexURSCalificadas()
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
                model.Modulo = ConstantesSubasta.SModuloSubastas;
                model.TamanioMaxActa = ConfigurationManager.AppSettings[ConstantesSubasta.KeyTamanioArchivoActa].ToString();

                #region Filserver

                // Ruta base de Eventos          
                string pathBaseSubastas = base.PathFiles + "\\" + GetCarpetaActaXTipo(1);
                // Obtener un identificador unico
                string currentUserSession = HttpContext.Session.SessionID;

                // Crear carpeta temporal SNombreCarpetaTemporal
                string nombreCarpetaTemporal = ConstantesSubasta.SNombreCarpetaTemporal + "_" + currentUserSession;
                FileServer.CreateFolder(pathBaseSubastas, nombreCarpetaTemporal, null); // para asegurarnos de su existencia
                string pathTemporal = pathBaseSubastas + nombreCarpetaTemporal;
                FileServer fs = new FileServer();
                fs.DeleteFolder(pathTemporal);// borramos la carpeta    
                FileServer.CreateFolder(pathBaseSubastas, nombreCarpetaTemporal, null); // creamos la nueva carpeta vacía

                #endregion

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Listar las URS
        /// </summary>
        /// <param name="miDataM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoURSCalificadas(string fechaConsulta)
        {

            SmaConfiguracionModel model = new SmaConfiguracionModel();
            DateTime fecha = new DateTime();

            if (fechaConsulta != "")
            {
                fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            else
            {
                fecha = DateTime.Now;
            }

            string url = Url.Content("~/");
            string listaHtml = servicio.ReporteListadoURSCalificadasHtml(url, fecha, base.VerificarAccesoAccion(Acciones.Editar, base.UserName));

            model.Resultado = listaHtml;

            return Json(model);
        }

        /// <summary>
        /// Listar historico de grupocodi y concepto
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHistoricoURSCalificadas(int grupocodi, int opedicion)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            model.AccesoEditar = opedicion == 2 ? base.VerificarAccesoAccion(Acciones.Editar, base.UserName) : false;

            //devolver valores correctos para la urs y sus históricos
            model.ListaGrupodat = servicio.CompletarDataHistoricoURS(grupocodi);
            return PartialView(model);
        }

        /// <summary>
        /// Dibuja la sección para edición de URS
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="ursCodi"></param>
        /// <param name="opcionActual"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEdicionURS(string fechaConsulta, int ursCodi, int opcionActual)
        {
            DateTime fecha = new DateTime();

            if (fechaConsulta != "")
            {
                fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            else
            {
                fecha = DateTime.Now;
            }
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            string listaHtml = servicio.ListadoEdicionURS(fecha, ursCodi, opcionActual);

            model.Resultado = listaHtml;

            return Json(model);
        }

        /// <summary>
        /// Registrar/editar URS calificadas
        /// </summary>
        /// <param name="deleted"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarURSGrupodat(string strJsonData)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                model = serialize.Deserialize<SmaConfiguracionModel>(strJsonData);

                DateTime fechaDat = DateTime.ParseExact(model.FechaData, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //Validaciones
                if (model.Deleted != 0)
                {
                    throw new Exception("El registro ya ha sido eliminado, no puede modificarse.");
                }

                if (!model.ModosOpList.Any())
                {
                    throw new Exception("No existe modos y/o unidades.");
                }

                var rangosValidate = "";
                //validar rangos de fechas
                if (model.TipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    rangosValidate = servicio.ValidarInterseccion(model.Grupocodi, model.FechaInicio, model.FechaFin, ConstantesSubasta.AccionURSCalificada);
                    if (rangosValidate != "")
                        throw new Exception(rangosValidate);
                }

                #region Archivos

                // Obtener un identificador unico
                string currentUserSession = HttpContext.Session.SessionID;

                // Crear carpeta temporal          
                string nombreCarpetaTemporal = ConstantesSubasta.SNombreCarpetaTemporal + "_" + currentUserSession;

                // Ruta base a mover           
                string pathTemporalEventos = base.PathFiles + "\\" + GetCarpetaActaXTipo(1) + nombreCarpetaTemporal;

                // Ruta base de Eventos            
                string pathBaseSubastas = base.PathFiles + "\\" + GetCarpetaActaXTipo(1);
                string fechaId = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha) + "_" + DateTime.Today.ToString("hh:mm:ss") + DateTime.Today.ToString("tt");

                fechaId = fechaId.Replace('/', '_').Replace(".", string.Empty).Replace(":", string.Empty).ToString();
                fechaId = fechaId.Trim();

                //archivos
                //Crear path temporal
                var path = base.PathFiles + "//" + GetCarpetaActaXTipo(1) + nombreCarpetaTemporal + "\\";
                List<FileData> listaDocumentos = FileServer.ListarArhivos(path, null);
                bool existeArchivo = listaDocumentos.Any();
                if (existeArchivo)
                    model.Acta = listaDocumentos.First().FileName;

                #endregion


                var resultValidate = "";
                //Guardar
                if (model.TipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    PrGrupodatDTO reg = new PrGrupodatDTO();
                    reg.Grupocodi = model.Grupocodi;
                    reg.FechaInicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    reg.FechaFin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    reg.BandaURS = model.BandaURS;
                    reg.Acta = model.Acta;

                    reg.Fechadat = fechaDat;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;
                    reg.Deleted = ConstantesSubasta.GrupodatActivo;

                    resultValidate = this.servicio.RegistrarGrupoDat(reg, model.ModosOpList, model.TipoAccion);
                }
                if (model.TipoAccion == ConstantesSubasta.AccionEditar)
                {
                    PrGrupodatDTO reg = new PrGrupodatDTO();

                    reg.FechaInicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    reg.FechaFin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    reg.BandaURS = model.BandaURS;
                    reg.Acta = model.Acta;
                    reg.Grupocodi = model.Grupocodi;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;
                    reg.Fechadat = fechaDat;

                    resultValidate = this.servicio.RegistrarGrupoDat(reg, model.ModosOpList, model.TipoAccion);
                }

                if (resultValidate != "")
                {
                    throw new Exception(resultValidate);
                }

                if (existeArchivo)
                {
                    FileServer fs = new FileServer();
                    FileServer.CreateFolder(pathBaseSubastas, model.Grupocodi.ToString(), null);
                    fs.CortarDirectory(pathTemporalEventos, pathBaseSubastas + model.Grupocodi + "\\" + ConstantesSubasta.SNombreCarpetaActa);
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
        /// Eliminar la Urs y los modos
        /// </summary>
        /// <param name="strJsonDataDeleted"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult URSEliminar(string strJsonDataDeleted)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                model = serialize.Deserialize<SmaConfiguracionModel>(strJsonDataDeleted);

                DateTime fechaDat = DateTime.ParseExact(model.FechaData, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                PrGrupodatDTO reg = new PrGrupodatDTO();

                reg.Grupocodi = model.Grupocodi;
                reg.Lastuser = User.Identity.Name;
                reg.Fechaact = DateTime.Now;
                reg.Fechadat = fechaDat;
                reg.Deleted2 = ConstantesSubasta.GrupodatInactivo;

                var resultValidate = this.servicio.EliminarURSGrupoDat(reg, model.ModosOpList);

                if (resultValidate != "")
                {
                    throw new Exception(resultValidate);
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

        public ActionResult ExportarReporteURSCalificadas(string fechaConsulta)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            DateTime fecha = new DateTime();
            try
            {
                base.ValidarSesionJsonResult();

                fecha = fechaConsulta != "" ? DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nombreArchivo = NombreArchivo.ReporteURSCalificadas;

                this.servicio.GenerarArchivoExcelURSCalificadas(ruta, nombreArchivo, fecha);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        public virtual ActionResult DescargarReporte()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteURSCalificadas;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteURSCalificadas);
        }

        #endregion

        #region Provisión Base

        public ActionResult IndexProvisionaBase()
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);


                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
                model.Modulo = ConstantesSubasta.SModuloSubastas;
                model.TamanioMaxActa = ConfigurationManager.AppSettings[ConstantesSubasta.KeyTamanioArchivoActa].ToString();

                #region Filserver

                // Ruta base de Eventos          
                string pathBaseSubastas = base.PathFiles + "\\" + GetCarpetaActaXTipo(2);
                // Obtener un identificador unico
                string currentUserSession = HttpContext.Session.SessionID;

                // Crear carpeta temporal SNombreCarpetaTemporal
                string nombreCarpetaTemporal = ConstantesSubasta.SNombreCarpetaTemporal + "_" + currentUserSession;
                FileServer.CreateFolder(pathBaseSubastas, nombreCarpetaTemporal, null); // para asegurarnos de su existencia
                string pathTemporal = pathBaseSubastas + nombreCarpetaTemporal;
                FileServer fs = new FileServer();
                fs.DeleteFolder(pathTemporal);// borramos la carpeta    
                FileServer.CreateFolder(pathBaseSubastas, nombreCarpetaTemporal, null); // creamos la nueva carpeta vacía

                #endregion

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Listar las URS
        /// </summary>
        /// <param name="miDataM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoProvisionBase(string fechaConsulta, string estadoFiltro)
        {

            SmaConfiguracionModel model = new SmaConfiguracionModel();
            DateTime fecha = new DateTime();

            if (fechaConsulta != "")
            {
                fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            else
            {
                fecha = DateTime.Now;
            }

            string url = Url.Content("~/");
            string listaHtml = servicio.ReporteListadoProvisionBaseHtml(url, fecha, estadoFiltro, base.VerificarAccesoAccion(Acciones.Editar, base.UserName));

            model.Resultado = listaHtml;

            return Json(model);
        }

        /// <summary>
        /// Listar historico de grupocodi y concepto
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHistoricoProvisionBase(int grupocodi, int opedicion)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            model.AccesoEditar = opedicion == 2 ? base.VerificarAccesoAccion(Acciones.Editar, base.UserName) : false;

            //devolver valores correctos para la urs y sus históricos
            model.ListaGrupodat = servicio.CompletarDataHistoricoURSBase(grupocodi);
            return PartialView(model);
        }

        /// <summary>
        /// Dibuja la sección para edición de URS
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="ursCodi"></param>
        /// <param name="opcionActual"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEdicionProvisionBase(string fechaConsulta, int ursCodi, int opcionActual)
        {
            DateTime fecha = new DateTime();

            if (fechaConsulta != "")
            {
                fecha = DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            else
            {
                fecha = DateTime.Now;
            }
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            //string url = Url.Content("~/");
            string listaHtml = servicio.ListadoEdicionProvisionBase(fecha, ursCodi, opcionActual, out List<PrGrupoDTO> listaModoXUrs);

            model.Resultado = listaHtml;
            model.ListaModo = listaModoXUrs;

            return Json(model);
        }

        /// <summary>
        /// Registrar/editar URS calificadas
        /// </summary>
        /// <param name="deleted"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProvisionBaseGuardarGrupodat(string strJsonData)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                model = serialize.Deserialize<SmaConfiguracionModel>(strJsonData);

                DateTime fechaDat = DateTime.ParseExact(model.FechaData, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaConsult = DateTime.ParseExact(model.FechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //Validaciones
                if (model.Deleted != 0)
                {
                    throw new Exception("El registro ya ha sido eliminado, no puede modificarse.");
                }

                if (!model.ModosOpList.Any())
                {
                    throw new Exception("No existe modos y/o unidades.");
                }

                //validar rangos de fechas URS BASE
                var rangosValidate = "";
                if (model.TipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    rangosValidate = servicio.ValidarInterseccion(model.Grupocodi, model.FechaInicio, model.FechaFin, ConstantesSubasta.AccionURSBase);
                    if (rangosValidate != "")
                        throw new Exception(rangosValidate);
                }

                // VALIDAR RANGO POR UNIDADES / MODOS
                var rangosModosValid = "";
                rangosModosValid = servicio.ValidarInterseccionModosURSBase(model.Grupocodi, model.ModosOpList, fechaConsult);
                if (rangosModosValid != "")
                    throw new Exception(rangosModosValid);

                //if (model.TipoAccion == ConstantesSubasta.AccionEditar)
                //{
                //    rangosModosValid = servicio.ValidarInterseccionModosURSBase(model.Grupocodi, model.ModosOpList, fechaConsult);
                //    if (rangosModosValid != "")
                //        throw new Exception(rangosModosValid);
                //}

                // VALIDAR Rango de los modos dentro de la URS
                rangosValidate = servicio.ValidarModosIncluidosEnUrs(model.FechaInicio, model.FechaFin, model.ModosOpList);
                if (rangosValidate != "")
                    throw new Exception(rangosValidate);

                #region Archivos

                // Obtener un identificador unico
                string currentUserSession = HttpContext.Session.SessionID;

                // Crear carpeta temporal          
                string nombreCarpetaTemporal = ConstantesSubasta.SNombreCarpetaTemporal + "_" + currentUserSession;

                // Ruta base a mover           
                string pathTemporalEventos = base.PathFiles + "\\" + GetCarpetaActaXTipo(2) + nombreCarpetaTemporal;

                // Ruta base de Eventos            
                string pathBaseSubastas = base.PathFiles + "\\" + GetCarpetaActaXTipo(2);
                string fechaId = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha) + "_" + DateTime.Today.ToString("hh:mm:ss") + DateTime.Today.ToString("tt");

                fechaId = fechaId.Replace('/', '_').Replace(".", string.Empty).Replace(":", string.Empty).ToString();
                fechaId = fechaId.Trim();

                //archivos
                //Crear path temporal
                var path = base.PathFiles + "//" + GetCarpetaActaXTipo(2) + nombreCarpetaTemporal + "\\";
                List<FileData> listaDocumentos = FileServer.ListarArhivos(path, null);
                bool existeArchivo = listaDocumentos.Any();
                if (existeArchivo)
                    model.Acta = listaDocumentos.First().FileName;

                #endregion

                var resultValidate = "";
                //Guardar
                if (model.TipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    PrGrupodatDTO reg = new PrGrupodatDTO();
                    reg.Grupocodi = model.Grupocodi;
                    reg.FechaInicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    reg.FechaFin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    reg.Acta = model.Acta;

                    reg.Fechadat = fechaDat;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;
                    reg.Deleted = ConstantesSubasta.GrupodatActivo;

                    resultValidate = this.servicio.RegistrarURSBaseGrupoDat(reg, model.ModosOpList, model.TipoAccion);
                }
                if (model.TipoAccion == ConstantesSubasta.AccionEditar)
                {
                    PrGrupodatDTO reg = new PrGrupodatDTO();

                    reg.FechaInicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    reg.FechaFin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    reg.Acta = model.Acta;
                    reg.Grupocodi = model.Grupocodi;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;
                    reg.Fechadat = fechaDat;

                    resultValidate = this.servicio.RegistrarURSBaseGrupoDat(reg, model.ModosOpList, model.TipoAccion);
                }

                if (resultValidate != "")
                {
                    throw new Exception(resultValidate);
                }

                if (existeArchivo)
                {
                    FileServer fs = new FileServer();
                    FileServer.CreateFolder(pathBaseSubastas, model.Grupocodi.ToString(), null);
                    fs.CortarDirectory(pathTemporalEventos, pathBaseSubastas + model.Grupocodi + "\\" + ConstantesSubasta.SNombreCarpetaActa);
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
        /// Eliminar la Urs y los modos
        /// </summary>
        /// <param name="strJsonDataDeleted"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProvisionBaseEliminar(string strJsonDataDeleted)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                model = serialize.Deserialize<SmaConfiguracionModel>(strJsonDataDeleted);

                DateTime fechaDat = DateTime.ParseExact(model.FechaData, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                PrGrupodatDTO reg = new PrGrupodatDTO();

                reg.Grupocodi = model.Grupocodi;
                reg.Lastuser = User.Identity.Name;
                reg.Fechaact = DateTime.Now;
                reg.Fechadat = fechaDat;
                reg.Deleted2 = ConstantesSubasta.GrupodatInactivo;

                var resultValidate = this.servicio.EliminarURSBaseGrupoDat(reg, model.ModosOpList);

                if (resultValidate != "")
                {
                    throw new Exception(resultValidate);
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

        public ActionResult ExportarReporteURSBase(string fechaConsulta, string estadoFiltro)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();
            DateTime fecha = new DateTime();
            try
            {
                base.ValidarSesionJsonResult();

                fecha = fechaConsulta != "" ? DateTime.ParseExact(fechaConsulta, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nombreArchivo = NombreArchivo.ReporteURSBase;

                this.servicio.GenerarArchivoExcelURSBase(ruta, nombreArchivo, fecha, estadoFiltro);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        public virtual ActionResult DescargarReporteURSBase()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteURSBase;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteURSBase);
        }

        #endregion

        #region Archivos

        private string GetCarpetaActaXTipo(int tipo)
        {
            return tipo == 1 ? ConstantesSubasta.SUrsCalSubastasFile : ConstantesSubasta.SUrsProvSubastasFile;
        }

        /// <summary>
        /// metodos archivos solicitudes
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="sModulo"></param>
        /// <param name="nroOrden"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadActa(string sFecha, string sModulo, int tipo)
        {
            SmaConfiguracionModel model = new SmaConfiguracionModel();

            try
            {
                base.ValidarSesionUsuario();
                if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                string sNombreArchivo = string.Empty;
                string path = "";
                string sNombreOriginal = "";
                string rootPath = FileServer.GetDirectory();
                string currentUserSession = HttpContext.Session.SessionID;

                if (String.Equals(sModulo, ConstantesSubasta.SModuloSubastas))
                {
                    // Obtener path temporal
                    string nombreCarpetaTemporal = ConstantesSubasta.SNombreCarpetaTemporal + "_" + currentUserSession;
                    path = base.PathFiles + "//" + GetCarpetaActaXTipo(tipo) + nombreCarpetaTemporal + "\\";
                }
                else
                {
                    path = base.PathFiles + "//";
                    throw new Exception("Módulo no permitido");
                }

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreOriginal = file.FileName;
                    sNombreArchivo = sFecha + "_" + file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, path + "\\" + sNombreArchivo, null))
                    {
                        FileServer.DeleteBlob(path + "\\" + sNombreArchivo, null);
                    }

                    FileServer.UploadFromStream(file.InputStream, path, sNombreArchivo, null);
                }
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Metodo para eliminar los archivos en un registro nuevo
        /// </summary>
        /// <param name="nombreArchivo">Nombre de archivo</param>
        /// <returns>Entero</returns>
        [HttpPost]
        public int EliminarArchivosNuevo(string nombreArchivo, int tipo)
        {
            base.ValidarSesionUsuario();

            //path temporal   
            string currentUserSession = HttpContext.Session.SessionID;
            string nombreCarpetaTemporal = ConstantesSubasta.SNombreCarpetaTemporal + "_" + currentUserSession;
            string pathTemporal = base.PathFiles + "//" + GetCarpetaActaXTipo(tipo) + nombreCarpetaTemporal;

            string nombrePath = string.Empty;

            SmaConfiguracionModel model = new SmaConfiguracionModel();
            model.ListaDocumentos = FileServer.ListarArhivos(pathTemporal + "\\", null);
            foreach (var item in model.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombrePath = item.FileName;
                    break;
                }
            }

            if (FileServer.VerificarExistenciaFile(pathTemporal, nombrePath, null))
            {
                FileServer.DeleteBlob(pathTemporal + "//" + nombrePath, null);
            }

            return -1;
        }

        /// <summary>
        /// Permite mostrar los archivos del directorio Files para los nuevos registros
        /// </summary>
        /// <param name="sModulo">Modulo</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult ListaArchivosNuevo(string sModulo, int tipo)
        {
            base.ValidarSesionUsuario();

            SmaConfiguracionModel model = new SmaConfiguracionModel();

            string path = "";
            string currentUserSession = HttpContext.Session.SessionID;

            if (String.Equals(sModulo, ConstantesSubasta.SModuloSubastas))
            {
                //Crear path temporal    
                string nombreCarpetaTemporal = ConstantesSubasta.SNombreCarpetaTemporal + "_" + currentUserSession;
                path = base.PathFiles + "//" + GetCarpetaActaXTipo(tipo) + nombreCarpetaTemporal + "\\";
            }
            else
            {
                path = base.PathFiles + "//";
            }

            model.ListaDocumentos = FileServer.ListarArhivos(path, null);
            model.ListaDocumentosFiltrado = new List<FileData>();
            foreach (var item in model.ListaDocumentos)
            {
                model.ListaDocumentosFiltrado.Add(item);
            }
            return Json(model);
        }

        /// <summary>
        /// Descargar Archivos
        /// </summary>
        /// <param name="nameArchivo"></param>
        /// <param name="idSoli"></param>
        /// <param name="tipoArchivo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarActa(string nameArchivo, int grupocodi, int tipo)
        {
            try
            {
                string nombreCarpeta = ConstantesSubasta.SNombreCarpetaActa;
                string pathBaseSubastas = base.PathFiles + "\\" + GetCarpetaActaXTipo(tipo) + grupocodi + "\\" + nombreCarpeta + "\\" + nameArchivo;

                Stream stream = FileServer.DownloadToStream(pathBaseSubastas, null);
                int indexOf = nameArchivo.LastIndexOf('.');
                string extension = nameArchivo.Substring(indexOf + 1, nameArchivo.Length - indexOf - 1);

                if (stream != null)
                    return File(stream, extension, nameArchivo);
                else
                {
                    Log.Info("Download - No se encontro el archivo: " + pathBaseSubastas);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        #endregion
    }
}