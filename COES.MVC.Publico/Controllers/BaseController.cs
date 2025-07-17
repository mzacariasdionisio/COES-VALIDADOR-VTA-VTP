using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Security;
using COES.Framework.Base.Core;
using COES.MVC.Publico.Helper;
using COES.MVC.Publico.SeguridadServicio;

namespace COES.MVC.Publico.Controllers
{
    public class BaseController : Controller
    {
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
                RedirectToAction("login", "home");
            }
            else
            {
                FormsAuthentication.SetAuthCookie(((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin, false);
            }
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
        /// Permite mostrar el paginado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult Paginado(Paginacion model)
        {
            return PartialView(model);
        }
    }
}

