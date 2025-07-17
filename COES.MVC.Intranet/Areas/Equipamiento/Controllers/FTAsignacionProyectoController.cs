using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTAsignacionProyectoController : BaseController
    {
        FichaTecnicaAppServicio servicioFT = new FichaTecnicaAppServicio();
        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        IEODAppServicio servIeod = new IEODAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTAsignacionProyectoController));
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

        #region Asignación de proyectos Extranet
        /// <summary>
        /// Pantalla Inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAsignacionProyecto()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();
            model.ListaEmpresas = servicioFT.ListarEmpresasActivas();
            model.ListaEtapas = servicioFT.ListFtExtEtapas();
            

            model.ListaFamilia = this.servIeod.ListarFamilia();
            model.ListaCategoria = servicioFT.ListarCategoriaGrupoXCatecodi(ConstantesMigraciones.CatecodiParametroFiltro);
            model.ListaUbicacion = servicioFT.ListarUbicacionesFT();

            return View(model);
        }

        /// <summary>
        /// Lista los proyectos asignados
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarProyectosAsig(string idempresa, int idetapa)
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();               
                model.ListadoProyectosAsig = servicioFT.ListarAsignacionProyExtranet(idempresa, idetapa);
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
        /// Elimina cierto proyecto
        /// </summary>
        /// <param name="fetempcodi"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CambiarestadoProyecto(int fetempcodi, int opt)
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();
            string estadoProy = (opt == 1) ? ConstantesAppServicio.Activo : ConstantesAppServicio.Baja;

            try
            {            
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;
                string msg = servicioFT.ValidarActivacionEmpresaEtapa(fetempcodi, opt);
                if (msg != "")
                    throw new Exception(msg);
                servicioFT.DarAlta_BajaProyectoAsignado(fetempcodi, usuario, estadoProy);
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
        /// Obtiene los detalles de proyecto seleccionado
        /// </summary>
        /// <param name="fetempcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DetallarProyecto(int fetempcodi)
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();                
                
                model.Proyecto = servicioFT.ObtenerInformacionRelEmpresaEtapa(fetempcodi);

                List<FtExtEtempdeteqDTO> listaElementos = model.Proyecto.ListaElementos != null ? model.Proyecto.ListaElementos : new List<FtExtEtempdeteqDTO>();
                model.ListadoRelacionEGP = listaElementos.Any() ? servicioFT.ObtenerElementosGuardados(listaElementos, fetempcodi) : new List<FTRelacionEGP>();
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
        /// Guarda o actualiza proyectos
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="fetempcodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="idetapa"></param>
        /// <param name="lstProyectos"></param>
        /// <param name="lstCambiosCIO"></param>
        /// <param name="lstElementos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarProyecto(int accion, int fetempcodi, int emprcodi, int idetapa,  List<FtExtEtempdetpryDTO> lstProyectos, List<FTRelacionEGP> lstCambiosCIO, List<FTRelacionEGP> lstElementos)
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;

                lstProyectos = lstProyectos != null ? lstProyectos : new List<FtExtEtempdetpryDTO>();
                lstCambiosCIO = lstCambiosCIO != null ? lstCambiosCIO : new List<FTRelacionEGP>();
                lstElementos = lstElementos != null ? lstElementos : new List<FTRelacionEGP>();

                string msg = servicioFT.ValidarDuplicidadYProyectosYExistenciaCambios(lstProyectos, lstCambiosCIO, lstElementos, emprcodi, idetapa, accion);
                if ( msg != "" ) 
                    throw new Exception(msg);                             
                
                servicioFT.GuardarDatosProyectoYRelaciones(accion, fetempcodi, emprcodi, idetapa, lstProyectos, lstCambiosCIO, lstElementos, usuario);

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
        /// Devuelve todos los proyectos activos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerProyectosExistentes()
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListadoProyectos = appEquipamiento.ListarProyectosExistentes();
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
        /// Obtiene los datos del proyecto seleccionado
        /// </summary>
        /// <param name="strIdsProyectos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosProysSel(string strIdsProyectos)
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListadoProyectos = servicioFT.ObtenerDatosDeProyectos(strIdsProyectos);
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
        /// Devuelve el listado de equipos y MO asociados a una empresa y proyecto
        /// </summary>
        /// <param name="feeprycodi"></param>
        /// <param name="ftprycodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="idEtapa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerMOYEquiposAsociadosAlPy(int feeprycodi, int ftprycodi, int emprcodi, int idEtapa)
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListadoRelacionEGP = servicioFT.ObtenerMOYEquiposRelacionadosAlProyecto(feeprycodi, ftprycodi, emprcodi, idEtapa);
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
        ///  Devuelve datos de auditoria
        /// </summary>
        /// <param name="feeprycodi"></param>
        /// <param name="ftprycodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="grupocodi"></param>
        /// <param name="idEtapa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosAuditoriaCIO(int feeprycodi, int ftprycodi, int emprcodi, int? equicodi, int? grupocodi, int idEtapa)
        {            
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.DetalleCIO = servicioFT.ObtenerDatosDetalleCIO(feeprycodi, ftprycodi, emprcodi, equicodi, grupocodi, idEtapa);
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
        /// Devuelve datos de auditoria
        /// </summary>
        /// <param name="feeeqcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosAuditoriaO(int feeeqcodi)
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.DetalleO = servicioFT.ObtenerDatosDetalleO(feeeqcodi);
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
        /// Obtiene los datos del elementos seleccionados
        /// </summary>
        /// <param name="strIdsSeleccionados"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosElementosSel(string strIdsSeleccionados, int tipo, int idElemento, int emprcodi, int idEtapa)
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListadoRelacionEGP = servicioFT.ObtenerDatosDeElementosSeleccionados(strIdsSeleccionados, tipo, idElemento, emprcodi, idEtapa);
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
        /// Obtiene todos los equipos o grupos relacionados con la empresa
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="idElemento"></param>
        /// <param name="emprcodi"></param>
        /// <param name="idUbicacion"></param>
        /// <returns></returns>
        [HttpPost] 
        public JsonResult ObtenerListadoElementos(int tipo, int idElemento, int emprcodi, int idUbicacion) 
        {
            FTAsignacionProyectoModel model = new FTAsignacionProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListadoRelacionEGP = servicioFT.ObtenerListadoElementosRelacionados(tipo, idElemento, emprcodi, idUbicacion); 
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