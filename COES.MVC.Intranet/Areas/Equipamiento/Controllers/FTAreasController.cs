using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTAreasController : BaseController
    {
        FichaTecnicaAppServicio servicioFT = new FichaTecnicaAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTAreasController));
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

        #region Listado Administración de Correos Áreas
        /// <summary>
        /// Pantalla Inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAreasModel model = new FTAreasModel();
            model.Grabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.Areas = servicioFT.ObtenerAreaAdminFT();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Lista los proyectos segun filtro ingresado
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarAreas()
        {
            FTAreasModel model = new FTAreasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ListaCorreos = servicioFT.ListarCorreos();
                model.ListadoAreas = servicioFT.ListarAreaCorreos();  
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
        /// Guarda o actualiza proyectos
        /// </summary>
        /// <param name="empresaNomb"></param>
        /// <param name="codigo"></param>
        /// <param name="proyNomb"></param>
        /// <param name="proyExtNomb"></param>
        /// <param name="esteocodi"></param>
        /// <param name="empresaId"></param>
        /// <param name="conEstudio"></param>
        /// <param name="accion"></param>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarAreaCorreos(int accion, string area, List<string> correos, int? faremcodi)
        {
            FTAreasModel model = new FTAreasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                string usuario = base.UserName;

                string strValidacion = "";
                if (accion == ConstantesFichaTecnica.AccionNuevo)
                {
                    //VALIDAR DUPLICADOS
                    strValidacion = servicioFT.ValidarAreaRepetida(area);
                    if (strValidacion != "") throw new ArgumentException(strValidacion);

                    string validaciones = servicioFT.ValidarCorreosVacios(correos);
                    if (validaciones != "") throw new ArgumentException(validaciones);

                    string validacionCorreo = servicioFT.ValidarCorreosOtrasAreas(correos, 0);
                    if (validacionCorreo != "") throw new ArgumentException(validacionCorreo);

                    servicioFT.GuardarDatosAreaCorreo(area, correos, usuario);
                }
                else
                {
                    if (accion == ConstantesFichaTecnica.AccionEditar)
                    {
                        if(faremcodi == ConstantesFichaTecnica.IdAreaAdminFT)
                        {
                            FtExtCorreoareaDTO correosAdmin = servicioFT.GetByIdFtExtCorreoarea(ConstantesFichaTecnica.IdAreaAdminFT);
                            if(correosAdmin == null)
                                throw new ArgumentException("Se necesita completar con la ejecución de todos los script para los correos de la ficha técnica");
                        }
                        string validaciones = servicioFT.ValidarCorreosVacios(correos);
                        if (validaciones != "") throw new ArgumentException(validaciones);

                        if (faremcodi != ConstantesFichaTecnica.IdAreaAdminFT)
                        {
                            string validacionCorreo = servicioFT.ValidarCorreosOtrasAreas(correos, faremcodi.Value);
                            if (validacionCorreo != "") throw new ArgumentException(validacionCorreo);
                        }

                        servicioFT.ActualizarDatosAreaCorreo(area, correos, usuario, faremcodi.Value);
                    }
                }

                if (faremcodi != null )
                {
                    if (faremcodi != ConstantesFichaTecnica.IdAreaAdminFT)
                    {
                        SeguridadServicioClient servSeguridad = new SeguridadServicioClient();
                        UserDTO[] arrayUsuario = servSeguridad.ListarUsuarios();
                        AsignarRolDefaultAUsuariosAreas(area, correos, arrayUsuario);
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

        /// <summary>
        /// Muestra roles de los correos asociados
        /// </summary>
        /// <param name="correos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerRolesUsuariosArea(List<string> correos)
        {
            FTAreasModel model = new FTAreasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<int> listaUsercodes_PT = new List<int>();
                List<int> listaUsercodes_SNC = new List<int>();

                List<FwUserrolDTO> listaUsuarioPorRolPermisoTotal = servicioFT.ObtenerUsuarioPorRol(ConstantesFichaTecnica.RolUsuarioIntranetAreas_PermisoTotal);
                List<FwUserrolDTO> listaUsuarioPorRolSoloNoConfidenciales = servicioFT.ObtenerUsuarioPorRol(ConstantesFichaTecnica.RolUsuarioIntranetAreas_SoloNoConfidenciales);
                List<int> lstUsercodesEnBD_PT = listaUsuarioPorRolPermisoTotal.Where(x => x.Usercode != null).Select(x => x.Usercode).ToList();
                List<int> lstUsercodesEnBD_SNC = listaUsuarioPorRolSoloNoConfidenciales.Where(x => x.Usercode != null).Select(x => x.Usercode).ToList();

                SeguridadServicioClient servSeguridad = new SeguridadServicioClient();
                UserDTO[] arrayUsuario = servSeguridad.ListarUsuarios();
                List<UserDTO> listaUserCOES = arrayUsuario.Where(x => x.UserEmail != null && x.UserEmail.Contains("@coes.org.pe")).ToList();

                List<UserDTO> listaUserAreasVerificar = listaUserCOES.Where(x => correos.Contains(x.UserEmail.Trim())).ToList();
                List<int> lstUsercodes = listaUserAreasVerificar.Where(x => x.UserCode != null).Select(x => (int)x.UserCode).ToList();

                //hallo los usercodes para cada rol
                listaUsercodes_PT = (List<int>)lstUsercodes.Intersect(lstUsercodesEnBD_PT).ToList();
                listaUsercodes_SNC = (List<int>)lstUsercodes.Intersect(lstUsercodesEnBD_SNC).ToList();

                List<UserDTO> listaUser_PT = listaUserAreasVerificar.Where(x => listaUsercodes_PT.Contains((int)x.UserCode)).ToList();
                List<UserDTO> listaUser_SNC = listaUserAreasVerificar.Where(x => listaUsercodes_SNC.Contains((int)x.UserCode)).ToList();

                string correosRolPermisoTotal = string.Join(", ", listaUser_PT.Select(x => x.UserEmail).ToList());
                string correosRolSoloNoConfidenciales = string.Join(", ", listaUser_SNC.Select(x => x.UserEmail).ToList());

                model.correosRolPermisoTotal = correosRolPermisoTotal != "" ? correosRolPermisoTotal : "Ninguno";
                model.correosRolSoloNoConfidenciales = correosRolSoloNoConfidenciales != "" ? correosRolSoloNoConfidenciales : "Ninguno";

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
        /// Setea un rol por defecto a los usuarios de cada area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="correos"></param>
        /// <param name="arrayUsuarioS"></param>
        public void AsignarRolDefaultAUsuariosAreas(string area, List<string> correos, UserDTO[] arrayUsuarioS)
        {
            List<int> listaUsercodesAsignarRol = new List<int>();
            List<int> listaUsercodesQuitarRol = new List<int>();

            List<UserDTO> listaUserCOES = arrayUsuarioS.Where(x=>x.UserEmail != null && x.UserEmail.Contains("@coes.org.pe")).ToList();
            string correosAdminFt = servicioFT.ObtenerCorreosAdminFichaTecnica();
            List<FwUserrolDTO> listaUsuarioPorRolPermisoTotal = servicioFT.ObtenerUsuarioPorRol(ConstantesFichaTecnica.RolUsuarioIntranetAreas_PermisoTotal);
            List<FwUserrolDTO> listaUsuarioPorRolSoloNoConfidenciales = servicioFT.ObtenerUsuarioPorRol(ConstantesFichaTecnica.RolUsuarioIntranetAreas_SoloNoConfidenciales);
            List<int> lstUsercodesEnBD_PT = listaUsuarioPorRolPermisoTotal.Where(x => x.Usercode != null).Select(x => x.Usercode).ToList();
            List<int> lstUsercodesEnBD_SNC = listaUsuarioPorRolSoloNoConfidenciales.Where(x => x.Usercode != null).Select(x => x.Usercode).ToList();

            List<int> lstUsercodesEnBD = new List<int>();
            lstUsercodesEnBD.AddRange(lstUsercodesEnBD_PT);
            lstUsercodesEnBD.AddRange(lstUsercodesEnBD_SNC);
            lstUsercodesEnBD = lstUsercodesEnBD.Distinct().ToList();

            List<UserDTO> listaUserAreasGuardar = listaUserCOES.Where(x => correos.Contains(x.UserEmail.Trim())).ToList();
            List<int> lstUsercodes = listaUserAreasGuardar.Where(x => x.UserCode != null).Select(x => (int)x.UserCode).ToList();

            //obtengo usercodes a asignarle rol
            listaUsercodesAsignarRol = (List<int>)lstUsercodes.Except(lstUsercodesEnBD).ToList();

            List<FwUserrolDTO> lstUserRol = new List<FwUserrolDTO>();
            foreach (int usercode in listaUsercodesAsignarRol)
            {
                FwUserrolDTO userRol = new FwUserrolDTO();
                userRol.Usercode = usercode;
                userRol.Rolcode = ConstantesFichaTecnica.RolUsuarioIntranetAreas_PermisoTotal;
                userRol.Userrolvalidate = 1; 
                userRol.Userrolcheck = 1;
                userRol.Lastuser = ConstantesFichaTecnica.UsuarioSistema;
                userRol.Lastdate = DateTime.Now;

                servicioFT.SaveFwUserrol(userRol);
            }

        }

        /// <summary>
        /// Genera el archivo a exportar el listado de proyectos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarArea()
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Reporte_AreasCorreos_FichaTécnica_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) + string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                servicioFT.GenerarExportacionArea(ruta, pathLogo, nameFile);
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

        /// <summary>
        /// Obtiene los detalles de cierto proyecto
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerArea(int faremcodi)
        {
            FTAreasModel model = new FTAreasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.Areas = servicioFT.GetByIdFtExtCorreoarea(faremcodi);
                FtExtCorreoareaDTO correosAdmin = servicioFT.GetByIdFtExtCorreoarea(ConstantesFichaTecnica.IdAreaAdminFT);
                model.ExisteCorreosAdminFT = correosAdmin != null ? true : false;

                if(faremcodi == ConstantesFichaTecnica.IdAreaAdminFT && model.Areas == null)//Solo para correos admin FT
                {
                    model.Areas = new FtExtCorreoareaDTO();
                    model.Areas.FechaCreacionDesc = "";
                    model.Areas.FechaModificacionDesc = "";
                    model.Areas.Faremusucreacion = "";
                    model.Areas.Faremusumodificacion = "";

                }

                List<FtExtCorreoareadetDTO> lstCorreosXarea = servicioFT.ListarCorreosPorArea(faremcodi.ToString());
                model.ListaCorreosPorArea = lstCorreosXarea.Select(x => x.Faremdemail).ToList();

                var listaOpcion = servicioFT.AgregarCorreos(model.ListaCorreosPorArea);
                model.ListaCorreos = listaOpcion;


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
        /// Elimina cierto proyecto
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarArea(int faremcodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;
                servicioFT.DarBajaAreaCorreo(faremcodi, usuario);
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
        /// Activa cierto proyecto
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivarArea(int faremcodi)
        {
            FTProyectoModel model = new FTProyectoModel();

            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;
                
                servicioFT.ActivarAreaCorreo(faremcodi, usuario);
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