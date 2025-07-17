using COES.Framework.Base.Core;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Security;

namespace COES.MVC.Intranet.Controllers
{
    /// <summary>
    /// Clase controladora padre
    /// </summary>
    public class BaseController: Controller
    {
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public BaseController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Sobrecarga el método de excepcion
        /// </summary>
        /// <param name="filterContext"></param>
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
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicioClient();

        /// <summary>
        /// Permite validar la existencia de la sesion
        /// </summary>
        public void ValidarSesionUsuario()
        {
            if (Session[DatosSesion.SesionUsuario] == null)
            {
                 RedirectToAction(Constantes.LoginAction, Constantes.DefaultControler);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin, false);
            }
        }

        /// <summary>
        /// Permite validar si la sesión está activa
        /// </summary>
        public bool IsValidSesion
        {
            get
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Permite redirigir a la página de inicio
        /// </summary>
        public ActionResult RedirectToLogin()
        {
            string url = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
            return RedirectToAction(Constantes.LoginAction, Constantes.DefaultControler, new { area = string.Empty, originalUrl = url });
        }

        /// <summary>
        /// /// Permite redirigir a la pagina default
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectToHomeDefault()
        {
            return RedirectToAction(Constantes.DefaultAction, Constantes.DefaultControler, new { area = string.Empty });
        }

        /// <summary>
        /// Permite redirigir a la pagina de inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectToLoginDefault()
        {
            return RedirectToAction(Constantes.LoginAction, Constantes.DefaultControler, new { area = string.Empty });
        }

        /// <summary>
        /// Nombre del usuario logeado
        /// </summary>
        public string UserName
        {
            get
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    return ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                }
                else
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        return User.Identity.Name;
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Nombre del usuario logeado
        /// </summary>
        public string UserEmail
        {
            get
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    return ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;
                }
                else
                {
                    return string.Empty;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Id del usuario logueado
        /// </summary>
        public int UserCode
        {
            get
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    return ((UserDTO)Session[DatosSesion.SesionUsuario]).UserCode;
                }

                return 0;
            }
        }

        /// <summary>
        /// Código de área
        /// </summary>
        public int IdArea
        {
            get
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    return (int)((UserDTO)Session[DatosSesion.SesionUsuario]).AreaCode;
                }
                return -1;
            }
        }

        /// <summary>
        /// Ruta donde se almacenarán los archivos
        /// </summary>
        public string PathFiles
        {
            get
            {
                int? idIpcion = this.IdOpcion;
                if (idIpcion != null)
                {
                    OptionDTO option = this.seguridad.ObtenerOpcion((int)idIpcion);
                    if (option != null)
                    {
                        if (option.ModCodi != null)
                        {
                            ModuloDTO modulo = this.seguridad.ObtenerModulo((int)option.ModCodi);
                            if (modulo != null)
                            {
                                return modulo.PathFile;
                            }
                        }
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Código del módulo
        /// </summary>
        public int? IdModulo
        {
            get
            {
                int? idIpcion = this.IdOpcion;
                if (idIpcion != null)
                {
                    OptionDTO option = this.seguridad.ObtenerOpcion((int)idIpcion);
                    if (option != null)
                    {
                        return option.ModCodi;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Lista de empresas del usuario logueado
        /// </summary>
        public List<EmpresaDTO> ListaEmpresas
        {
            get
            {
                string userLogin = string.Empty;
                List<EmpresaDTO> entitys = new List<EmpresaDTO>();

                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                }
                else
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        userLogin = User.Identity.Name;
                    }
                }

                if (!string.IsNullOrEmpty(userLogin))
                {
                    entitys = this.seguridad.ObtenerEmpresasPorUsuario(userLogin).ToList();
                }

                return entitys;
            }
        }

        /// <summary>
        /// Código de la opción del menú actual
        /// </summary>
        public int? IdOpcion
        {
            get
            {
                if (Session[DatosSesion.SesionIdOpcion] != null)
                {
                    return Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]);
                }

                return null;
            }
        }

        /// <summary>
        /// Valida si un usuario tiene acceso a realizar alguna acción dentro de una pantalla
        /// </summary>
        /// <param name="idPermiso"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool VerificarAccesoAccion(int idPermiso, string userLogin)
        {            
            int? idOpcion = null;
            if (Session[DatosSesion.SesionIdOpcion] != null)
            {
                idOpcion = Convert.ToInt32(Session[DatosSesion.SesionIdOpcion]);
            }

            if (idOpcion != null)
            {
                bool flag = (new SeguridadServicio.SeguridadServicioClient()).ValidarPermisoOpcion(Constantes.IdAplicacion,
                    (int)idOpcion, idPermiso, userLogin);

                return flag;
            }

            return false;
        }

        /// <summary>
        /// Valida si un usuario tiene acceso a realizar alguna acción dentro de una pantalla especifica
        /// </summary>
        /// <param name="idPermiso"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool VerificarAccesoAccionXOpcion(int idPermiso, string userLogin, int idOpcion)
        {
            bool flag = (new SeguridadServicio.SeguridadServicioClient()).ValidarPermisoOpcion(Constantes.IdAplicacion,
                (int)idOpcion, idPermiso, userLogin);

            return flag;
        }

        /// <summary>
        /// Permite grabar el log de alguna acción en el sistema
        /// </summary>
        /// <param name="tipoLog">Tipo de Log: Mensaje, Error o Alerta</param>
        /// <param name="idAccion">Posibles acciones.</param>
        /// <param name="mensaje">Mensaje que ha grabarse en la base de datoss</param>
        public void GrabarLog(string tipoLog, int idAccion, string mensaje)
        {
            try
            {
                LogAppDTO log = new LogAppDTO();
                log.LogFecha = DateTime.Now;
                log.LogIndTipo = tipoLog;  
                log.LogUser = this.UserName;
                log.ModCodi = (short?)this.IdModulo;
                log.OptionCode = (short?)this.IdOpcion;
                log.PermiCode = (short?)idAccion;
                log.LogDesc = mensaje;
                log.LogPathFile = string.Empty;
                
                this.seguridad.GrabarLog(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite mostrar el paginado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult Paginado(Paginacion model)
        {
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginadoEpo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoEpo(Paginacion model)
        {
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginadoEpo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoEo(Paginacion model)
        {
            return PartialView(model);
        }

        /// <summary>
        /// Validación de sesión en JsonResult
        /// </summary>
        public void ValidarSesionJsonResult()
        {
            if (!IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);
            string usuario = string.IsNullOrEmpty(User.Identity.Name) ? string.Empty : User.Identity.Name.Trim();
            string usuario2 = string.IsNullOrEmpty(this.UserName) ? string.Empty : this.UserName.Trim();
            if (usuario.Length < 2 || usuario2.Length < 2) throw new Exception(Constantes.MensajeSesionExpirado);
        }

        /// <summary>
        /// Validación de sesión
        /// </summary>
        public bool IsValidSesionView()
        {
            if (!IsValidSesion) return false;
            string usuario = string.IsNullOrEmpty(User.Identity.Name) ? string.Empty : User.Identity.Name.Trim();
            if (usuario.Length < 2) return false;

            return true;
        }

        /// <summary>
        /// Renderiza vista parcial a cadena
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        /// <summary>
        /// Método para determinar si el usuario logueado tiene el rol de administrador de ficha técnica
        /// </summary>
        /// <returns></returns>
        public bool TieneRolAdministradorFicha()
        {
            bool salida = false;

            UserDTO entity = (UserDTO)Session[DatosSesion.SesionUsuario];
            List<RolDTO> listaRol = seguridad.ObtenerRolPorUsuario(entity.UserCode).ToList();
            RolDTO rol = listaRol.Find(x => x.RolCode == ConstantesFichaTecnica.RolAdministradorFichaTecnica);
            if (listaRol.Find(x => x.RolCode == ConstantesFichaTecnica.RolAdministradorFichaTecnica && x.Seleccion == 1) != null)
                salida = true;

            return salida;
        }

        public bool TienePermisosConfidencialFT(int idPermiso)
        {
            bool tienePermisoConfidencial;

            //Rol Administrador FT
            bool tienePermisoConfidencial2 = VerificarAccesoAccionXOpcion(idPermiso, UserName, ConstantesFichaTecnica.IdoptionAdminFicha); //FTAdministradorController EnvioFormato

            //Rol Usuario Proceso de revisión áreas FT (Permiso Total) 
            bool tienePermisoConfidencial1 = VerificarAccesoAccionXOpcion(idPermiso, UserName, ConstantesFichaTecnica.IdOptionModuloAreas); //FTAreasRevisionController Index

            /*
            //para pantallas de visualización de fichas en intranet
            bool tienePermisoConfidencial3 = VerificarAccesoAccionXOpcion(idPermiso, UserName, ConstantesFichaTecnica.IdoptionVisualizarFTVEIntranet); //FichaTecnicaController IndexFichaExtranet
            bool tienePermisoConfidencial4 = VerificarAccesoAccionXOpcion(idPermiso, UserName, ConstantesFichaTecnica.IdoptionVisualizarFTVIntranet); //FTVigenteController Index
            bool tienePermisoConfidencial5 = VerificarAccesoAccionXOpcion(idPermiso, UserName, ConstantesFichaTecnica.IdoptionConfigurarFTVIntranet); //FTVigenteController IndexConfiguracion
            */
            tienePermisoConfidencial = tienePermisoConfidencial1 || tienePermisoConfidencial2; //|| tienePermisoConfidencial3 || tienePermisoConfidencial4 || tienePermisoConfidencial5;

            return tienePermisoConfidencial;
        }

        /// <summary>
        /// Función genérica para descargar archivo temporal y posteriormente eliminarlo
        /// </summary>
        /// <param name="directorioTemporal"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoTemporalYEliminarlo(string directorioTemporal, string nombreArchivo)
        {
            string fullPath = directorioTemporal + nombreArchivo;

            //eliminar archivo temporal
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            //descargar archivo
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
        }

        /// <summary>
        /// Si el archivo no existe en el FileServer (ambiente Test o Preproducción) entonces descargar un archivo de texto de no disponible
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoNoDisponible()
        {
            //descargar archivo que no existe en el fileapp
            string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoNoDisponible;
            return Redirect(urlArchivoSinPermiso);
        }

    }
}