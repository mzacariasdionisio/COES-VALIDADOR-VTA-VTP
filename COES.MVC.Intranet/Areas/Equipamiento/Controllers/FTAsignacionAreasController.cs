using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTAsignacionAreasController : BaseController
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTAsignacionAreasController));
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

        #region Asignacion Areas

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAsignacionAreaModel model = new FTAsignacionAreaModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Lista de ficha tecnica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FichaTecnicaLista()
        {
            FTAsignacionAreaModel model = new FTAsignacionAreaModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListaFichaTecnica = servFictec.ListarFichaTecnica(ConstantesAppServicio.ParametroDefecto);

                model.Resultado = "1";
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

        /// <summary>
        /// Formulario de Ficha Tecnica (Tree)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult FichaTecnicaFormulario(int id)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.FichaTecnica = new FtFictecXTipoEquipoDTO();
                model.ListaFamilia = this.servIeod.ListarFamilia();
                model.ListaCategoria = this.servFictec.ListarCategoriaGrupoXCatecodi(ConstantesMigraciones.CatecodiParametroFiltro);
                model.FichaTecnica.Origen = ConstantesFichaTecnica.OrigenTipoEquipo;
                model.ListaFichaTecnicaPadre = new List<FtFictecXTipoEquipoDTO>();
                model.NumOrigenpadre = 0;
                model.ListaEstado = Util.ListaEstado();
                model.ListaNota = new List<FtFictecNotaDTO>();

                //areas diferentes a la Admin FT
                model.ListaAreas = servFictec.ListFtExtCorreoareas().Where(x => x.Faremestado == ConstantesFichaTecnica.EstadoStrActivo && x.Faremcodi != ConstantesFichaTecnica.IdAreaAdminFT).ToList();

                if (id > 0)
                {
                    model.FichaTecnica = this.servFictec.GetFichaTecnica(id);
                    model.ListaFichaTecnicaPadre = this.servFictec.ListarFichaTecnicaPadre(model.FichaTecnica.Origen, model.FichaTecnica.Catecodi, model.FichaTecnica.Famcodi)
                                                                .Where(x => x.Fteqcodi != id).ToList();

                    model.ListaNota = this.servFictec.ListFtFictecNotaByFteqcodi(id);
                }
                List<FtFictecItemDTO> listaItems, listaAllItems;
                List<TreeItemFichaTecnica> listaItemsJson;

                FTFiltroReporteExcel objFiltro = servFictec.GetFichaYDatosXEquipoOModo(id, -2, false, ConstantesFichaTecnica.INTRANET, DateTime.Today);
                servFictec.ListarTreeItemsFichaTecnica(objFiltro, out listaAllItems, out listaItems, out listaItemsJson);
                model.ListaItems = listaItems;
                model.ListaItemsJson = listaItemsJson;
                model.ListaAllItems = listaAllItems;

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                StringBuilder jsonTree = new StringBuilder();
                serializer.Serialize(model.ListaItemsJson, jsonTree);
                model.TreeJson = jsonTree.ToString();

                StringBuilder jsonNota = new StringBuilder();
                serializer.Serialize(model.ListaNota, jsonNota);
                model.NotaJson = jsonNota.ToString();

                model.ListaConcepto = servFictec.ListarTodoPrConceptos();
                model.ListaPropiedad = servFictec.ListarTodoEqPropiedades();

                //obtener valores de areas por cada item
                servFictec.ListarRelaciones(model.ListaAllItems);

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

        [HttpPost]
        public JsonResult GrabarDatosFormato(List<FtExtRelareaitemcfgDTO> relacion, int fteqcodi)
        {
            FTAsignacionAreaModel model = new FTAsignacionAreaModel();

            try
            {
                base.ValidarSesionJsonResult();

                var lstCorreosAdminFT = this.ObtenerListaCorreosAdminFT();
                servFictec.GuardarRelacionAreaItem(relacion, fteqcodi, lstCorreosAdminFT, base.UserName);

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
        /// Obtiene la informacion de la ficha tecnica Oficial
        /// </summary>
        /// <param name="fteqcodi"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="catecodi"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDataDelVigente(int fteqcodi)
        {
            FichaTecnicaModel model = new FichaTecnicaModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListaAllItems = servFictec.ObtenerInformacionFEGuardadaDelFichaOficial(fteqcodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        #endregion

        #region Asignación Áreas Operación Comercial

        [HttpPost]
        public JsonResult ListarEventos()
        {
            FTAsignacionAreaModel model = new FTAsignacionAreaModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.ListadoFtExtEventos = servFictec.GetByCriteriaFtExtEventos();
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

        [HttpPost]
        public JsonResult DetallarEvento(int ftevcodi)
        {
            FTAsignacionAreaModel model = new FTAsignacionAreaModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.FTExtEvento = servFictec.GetByIdFtExtEvento(ftevcodi);
                model.FTExtEvento.FtevfecvigenciaextDesc = model.FTExtEvento.Ftevfecvigenciaext.ToString(Constantes.FormatoFecha);
                model.ListaDetalleFTExtEvento = servFictec.GetByCriteriaFtExtEventoReqsxIdFTExtEvento(ftevcodi);

                //Areas diferente a Admin FT
                model.ListaAreas = servFictec.ListFtExtCorreoareas().Where(x => x.Faremestado == ConstantesFichaTecnica.EstadoStrActivo && x.Faremcodi != ConstantesFichaTecnica.IdAreaAdminFT).ToList();
                //obtener valores de areas por cada item
                servFictec.ListarRelAreaReq(model.ListaDetalleFTExtEvento);
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
        public JsonResult GuardarRelaAreaReq(List<FtExtRelAreareqDTO> relacion)
        {
            FTAsignacionAreaModel model = new FTAsignacionAreaModel();

            try
            {
                base.ValidarSesionJsonResult();

                servFictec.GuardarRelacionAreaReq(relacion, base.UserName);

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

        #region Asignación Áreas Dar de baja MO
        [HttpPost]
        public JsonResult ListarAreasBajaMO()
        {
            FTAsignacionAreaModel model = new FTAsignacionAreaModel();

            try
            {
                base.ValidarSesionJsonResult();

                //areas diferentes a admin FT
                model.ListaAreas = servFictec.ListFtExtCorreoareas().Where(x => x.Faremestado == ConstantesFichaTecnica.EstadoStrActivo && x.Faremcodi != ConstantesFichaTecnica.IdAreaAdminFT).ToList();

                //obtener valores de areas por cada item
                model.RequisitoBajaMO = servFictec.ListarRelacionesBajaMO();
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
        public JsonResult GuardarRelAreaBajaMO(string areas)
        {
            FTAsignacionAreaModel model = new FTAsignacionAreaModel();

            try
            {
                base.ValidarSesionJsonResult();

                servFictec.GuardarRelAreaBajaMO(areas, base.UserName);
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

        #region Utilidad

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
    }
}
