using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Configuration;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTFormatoExtranetController : BaseController
    {
        readonly FichaTecnicaAppServicio servicioFT = new FichaTecnicaAppServicio();
        readonly IEODAppServicio servIeod = new IEODAppServicio();

        readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTFormatoExtranetController));
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

        #region Configurar Formato Extranet para Operación Comercial

        /// <summary>
        /// Pantalla Inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexEvento()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTFormatoExtranetModel model = new FTFormatoExtranetModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.FechaActual = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Lista los eventos registrados
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEventos()
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListadoFtExtEventos = servicioFT.GetByCriteriaFtExtEventos();
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
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
        /// Elimina cierto evento
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEvento(int ftevcodi)
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;
                servicioFT.DarBajaEvento(ftevcodi, usuario);
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
        /// Obtiene los detalles de evento seleccionado
        /// </summary>
        /// <param name="ftevcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DetallarEvento(int ftevcodi)
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.FTExtEvento = servicioFT.GetByIdFtExtEvento(ftevcodi);
                model.FTExtEvento.FtevfecvigenciaextDesc = model.FTExtEvento.Ftevfecvigenciaext.ToString(Constantes.FormatoFecha);
                model.ListaDetalleFTExtEvento = servicioFT.GetByCriteriaFtExtEventoReqsxIdFTExtEvento(ftevcodi);
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
        /// Guarda o actualiza eventos
        /// </summary>
        /// <param name="ftevnombre"></param>
        /// <param name="sfechavigencia"></param>
        /// <param name="listaRequisitos"></param>
        /// <param name="accion"></param>
        /// <param name="ftevcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarEvento(string ftevnombre, string sfechavigencia, string listaRequisitos, int accion, int? ftevcodi)
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<FtExtEventoReqDTO> listaData = serializer.Deserialize<List<FtExtEventoReqDTO>>(listaRequisitos);

                FtExtEventoDTO objEvento = new FtExtEventoDTO();
                objEvento.Ftevnombre = ftevnombre;
                objEvento.Ftevfecvigenciaext = DateTime.ParseExact(sfechavigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string usuario = base.UserName;
                servicioFT.GuardarDatosEvento(objEvento, accion, usuario, ftevcodi, listaData);

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

        #region Exportar a excel

        /// <summary>
        /// Genera el archivo a exportar el listado de eventos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult exportarEventos()
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Reporte_EventoOperacionComercial_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) + string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                servicioFT.GenerarExportacionEventos(ruta, pathLogo, nameFile);
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
        /// Exporta archivo pdf, excel, csv, ...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string strArchivoTemporal = ruta + nombreArchivo;
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        #endregion

        #region Formato Extranet Conexión, Integración y Modificación
        /// <summary>
        /// Pagina principal de Formato Extranet Conexión, Integración y Modificación
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexFormato()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTFormatoExtranetModel model = new FTFormatoExtranetModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Lista las fichas tecnicas visibles en extranet
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFTVisibleExt()
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListaFichaTecnica = servicioFT.ListarFTVisibleExtranet();
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
        /// Devuelve los datos generales de detalles para cierta ficha tecnica
        /// </summary>
        /// <param name="fteqcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerCamposGeneralesDetalles(int fteqcodi)
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FtFictecXTipoEquipoDTO ft = servicioFT.GetFichaTecnica(fteqcodi);

                model.ListaEtapas = servicioFT.ListFtExtEtapas().Where(x => x.Ftetcodi != ConstantesFichaTecnica.EtapaOperacionComercial).ToList();
                model.Fecha = ft.FechaVigenciaExt != null ? ft.FechaVigenciaExt : "";
                model.Famcodi = ft.Famcodi;
                model.Catecodi = ft.Catecodi;
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
        /// Obtiene la informacion para armar la estructura izquierda de la tabla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult ObtenerDatosFT(int id)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.FichaTecnica = new FtFictecXTipoEquipoDTO();
                model.ListaFamilia = this.servIeod.ListarFamilia();
                model.ListaCategoria = this.servicioFT.ListarCategoriaGrupoXCatecodi(ConstantesMigraciones.CatecodiParametroFiltro);
                model.FichaTecnica.Origen = ConstantesFichaTecnica.OrigenTipoEquipo;
                model.ListaFichaTecnicaPadre = new List<FtFictecXTipoEquipoDTO>();
                model.NumOrigenpadre = 0;
                model.ListaEstado = Util.ListaEstado();
                model.ListaNota = new List<FtFictecNotaDTO>();

                if (id > 0)
                {
                    model.FichaTecnica = this.servicioFT.GetFichaTecnica(id);
                    model.ListaFichaTecnicaPadre = this.servicioFT.ListarFichaTecnicaPadre(model.FichaTecnica.Origen, model.FichaTecnica.Catecodi, model.FichaTecnica.Famcodi)
                                                                .Where(x => x.Fteqcodi != id).ToList();

                    model.ListaNota = this.servicioFT.ListFtFictecNotaByFteqcodi(id);
                }

                List<FtFictecItemDTO> listaItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;

                FTFiltroReporteExcel objFiltro = servicioFT.GetFichaYDatosXEquipoOModo(id, -2, false, ConstantesFichaTecnica.INTRANET, DateTime.Today);
                this.servicioFT.ListarTreeItemsFichaTecnica(objFiltro, out listaAllItems, out listaItems, out listaItemsJson);
                model.ListaItems = listaItems;
                model.ListaItemsJson = listaItemsJson;
                model.ListaAllItems = listaAllItems;

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                StringBuilder jsonTree = new StringBuilder();
                serializer.Serialize(model.ListaItemsJson, jsonTree);
                model.TreeJson = jsonTree.ToString();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                //model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guarda los datos de la tabla configuracion formato extranet
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarDatosFormato(FTFormatoExtranet formato, int fteqcodi)
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();

                var lstCorreosAdminFT = this.ObtenerListaCorreosAdminFT();
                string msg = servicioFT.ValidarDuplicidadParametroBloqueadoEdicion(formato);
                if (msg != "")
                    throw new Exception(msg);

                int val = servicioFT.GuardarDatosFormatoExtranet(ConstantesFichaTecnica.GuardadoOficial, fteqcodi, formato, lstCorreosAdminFT, base.UserName);

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
        /// Devuelve la informacion del formato desde la BD
        /// </summary>
        /// <param name="fteqcodi"></param>
        /// <param name="ftetcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerValoresConfiguracionFormatoExtranet(int fteqcodi, int ftetcodi)
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.FormatoExtranet = servicioFT.ObtenerInformacionFEGuardada(fteqcodi, ftetcodi, out int hayGuardado);
                model.ExisteInfoGuardada = hayGuardado;
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
        /// Obtiene la informacion de la ficha tecnica vigente
        /// </summary>
        /// <param name="fteqcodi"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="catecodi"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDataDelVigente(int fteqcodi, int ftetcodi, int? famcodi, int? catecodi, FTFormatoExtranet formato)
        {
            FTFormatoExtranetModel model = new FTFormatoExtranetModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.FormatoExtranet = servicioFT.ObtenerInformacionFEGuardadaDelVigente(fteqcodi, ftetcodi, famcodi, catecodi, base.UserName, formato, out int hayGuardado);
                model.ExisteInfoGuardada = hayGuardado;
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
        /// Obtener Lista de correos con rol de administrador de ficha técnica
        /// </summary>
        /// <returns></returns>
        private List<string> ObtenerListaCorreosAdminFT()
        {
            List<string> lstCorreos = new List<string>();

            var listaUsuriosFT = seguridad.ObtenerUsuariosPorRol(ConstantesFichaTecnica.RolAdministradorFichaTecnica);

            foreach (var item in listaUsuriosFT)
            {
                lstCorreos.Add(item.UserEmail);
            }

            lstCorreos = lstCorreos.Where(x => x.Contains("@coes")).ToList();

            return lstCorreos;
        }

        #endregion

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesFichaTecnica.ModuloManualUsuario;
            string nombreArchivo = ConstantesFichaTecnica.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesFichaTecnica.FolderRaizFichaTecnicaModuloManual;
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
    }
}