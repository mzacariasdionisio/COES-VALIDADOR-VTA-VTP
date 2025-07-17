using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Controllers;

namespace COES.Web.MVC.Seguridad.Controllers
{
    public class HomeController : BaseController
    {
        SeguridadServicioClient servicio = new SeguridadServicioClient();
        List<string> ListaMapa = new List<string>();

        /// <summary>
        /// Permite mostrar la vista inicial de la app
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorize]
        public ActionResult Default(string originalUrl)
        {

            HttpCookie cookie = Request.Cookies[DatosSesion.InicioSesion];
            string userLogin = string.Empty;

            if (cookie != null)
            {
                if (cookie[DatosSesion.SesionUsuario] != null)
                {
                    Session[DatosSesion.SesionMapa] = Constantes.NodoPrincipal;
                    Session[DatosSesion.SesionUsuario] = this.servicio.ObtenerUsuarioPorLogin(cookie[DatosSesion.SesionUsuario].ToString());
                    FormsAuthentication.SetAuthCookie(cookie[DatosSesion.SesionUsuario].ToString(), false);
                    userLogin = cookie[DatosSesion.SesionUsuario].ToString();
                }
            }
            else
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    Session[DatosSesion.SesionMapa] = Constantes.NodoPrincipal;
                    userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                }
                else
                {
                    return RedirectToAction(Constantes.LoginAction, Constantes.DefaultControler);
                }
            }

            //verificamos si usuario existe

            UserDTO usuario = this.servicio.ObtenerUsuarioPorLogin(userLogin);

            if (usuario != null)
            {
                if (string.IsNullOrEmpty(originalUrl))
                {

                    HeaderModel model = new HeaderModel();

                    List<OptionDTO> opciones = this.servicio.ListarAccesoDirecto(Constantes.IdAplicacion, userLogin).ToList();
                    List<MenuModel> list = new List<MenuModel>();

                    foreach (OptionDTO item in opciones)
                    {
                        list.Add(new MenuModel()
                        {
                            OpcionId = (int)item.OptionCode,
                            PadreId = item.PadreCodi,
                            Descripcion = item.OptionDesc,
                            Nombre = item.OptionName,
                            NroOrden = item.NroOrden,
                            OpcionURL = item.DesUrl,
                            Tipo = item.IndTipo,
                            DesControlador = (!string.IsNullOrEmpty(item.DesArea)) ? item.DesArea + "/" + item.DesController : item.DesController,
                            DesAccion = item.DesAction,
                            DesArea = item.DesArea,
                            AccesoDirecto = item.DesShortCut
                        });
                    }

                    model.ListaFavoritos = list;

                    return View(model);
                }
                else
                {
                    return Redirect(originalUrl);
                }
            }
            else
            {
                return RedirectToAction(Constantes.LoginAction, Constantes.DefaultControler);
            }
        }

        /// <summary>
        /// Muestra vista de info
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            return View();
        }

        /// <summary>
        /// Muestra la vista de autorizacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Autorizacion()
        {
            return View();
        }

        /// <summary>
        /// Permite mostrar la pagina de login
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string originalUrl)
        {
            ViewBag.OriginalUrl = originalUrl;
            return View();
        }

        public int ValidarSession()
        {
            if (Session[DatosSesion.SesionUsuario] != null) return 1;
            else
            {
                CerrarSesion();
                return 0;
            }
        }

        /// <summary>
        /// Permite cerrar sesion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session[DatosSesion.SesionUsuario] = null;
            HttpCookie myCookie = new HttpCookie(DatosSesion.InicioSesion);
            myCookie.Expires = DateTime.Now.AddDays(-2d);
            Response.Cookies.Add(myCookie);
            return Json(1);
        }

        /// <summary>
        /// Permite realizar la validacion del usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Autenticar(string usuario, string password, int indicador)
        {
            int resultado = 0;
            UserDTO entidad = this.servicio.AutentificarUsuarioAD(usuario, password, out resultado);


            if (resultado == 1)
            {
                FormsAuthentication.SetAuthCookie(usuario, false);
                Session[DatosSesion.SesionUsuario] = entidad;
                this.CrearCokkie(usuario, indicador);
            }
            else
            {
                entidad = this.servicio.AutentificarUsuario(usuario, password, out resultado);

                if (resultado == 1)
                {
                    FormsAuthentication.SetAuthCookie(usuario, false);
                    Session[DatosSesion.SesionUsuario] = entidad;
                    this.CrearCokkie(usuario, indicador);
                }
            }

            return Json(resultado);

        }

        /// <summary>
        /// Permite una autentificacion para usuarios anónimos
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult AutenticarAnonimo()
        {
            int resultado = 0;
            string usuario = Constantes.UsuarioAnonimo;
            string clave = Constantes.ClaveAnonimo;

            UserDTO entidad = this.servicio.AutentificarUsuario(usuario, clave, out resultado);

            if (resultado == 1)
            {
                FormsAuthentication.SetAuthCookie(usuario, false);
                Session[DatosSesion.SesionUsuario] = entidad;
            }

            return Json(resultado);
        }

        /// <summary>
        /// Permite crear una cokkie con los datos del usuario
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="indicador"></param>
        protected void CrearCokkie(string userName, int indicador)
        {
            if (indicador == 1)
            {
                HttpCookie cookie = Request.Cookies[DatosSesion.InicioSesion];

                if (cookie == null)
                {
                    cookie = new HttpCookie(DatosSesion.InicioSesion);
                }

                cookie[DatosSesion.SesionUsuario] = userName;
                cookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie myCookie = new HttpCookie(DatosSesion.InicioSesion);
                myCookie.Expires = DateTime.Now.AddDays(-2d);
                Response.Cookies.Add(myCookie);
            }
        }

        /// <summary>
        /// Permite mostrar el menu de la aplicacion
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            ViewBag.Menu = this.CrearMenu();
            return PartialView();
        }

        /// <summary>
        /// Permite mostrar los datos en la cabecera
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public PartialViewResult Header()
        {
            HeaderModel model = new HeaderModel();
            ViewBag.UserName = string.Empty;
            ViewBag.UserLogin = string.Empty;
            string userLogin = string.Empty;

            if (Session[DatosSesion.SesionUsuario] != null)
            {
                ViewBag.UserName = ((UserDTO)Session[DatosSesion.SesionUsuario]).UsernName;
                ViewBag.UserLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin + Constantes.SufijoImagenUser;
                userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
            }
            //else 
            //{
            //    if (!string.IsNullOrEmpty(User.Identity.Name))
            //    {
            //        ViewBag.UserLogin = User.Identity.Name + Constantes.SufijoImagenUser;
            //        userLogin = User.Identity.Name;
            //    }
            //}

            if (!string.IsNullOrEmpty(userLogin))
            {

                List<OptionDTO> opciones = this.servicio.ListarAccesoDirecto(Constantes.IdAplicacion, userLogin).ToList();
                List<MenuModel> list = new List<MenuModel>();

                foreach (OptionDTO item in opciones)
                {
                    list.Add(new MenuModel()
                    {
                        OpcionId = (int)item.OptionCode,
                        PadreId = item.PadreCodi,
                        Descripcion = item.OptionDesc,
                        Nombre = item.OptionName,
                        NroOrden = item.NroOrden,
                        OpcionURL = item.DesUrl,
                        Tipo = item.IndTipo,
                        DesControlador = (!string.IsNullOrEmpty(item.DesArea)) ? item.DesArea + "/" + item.DesController : item.DesController,
                        DesAccion = item.DesAction,
                        DesArea = item.DesArea
                    });
                }

                model.ListaFavoritos = list;
            }
            else
            {
                model.ListaFavoritos = new List<MenuModel>();
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar la ventana de cambio de clave
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CambioClave()
        {
            return PartialView();
        }

        /// <summary>
        /// Permite realizar el cambio de clave
        /// </summary>
        /// <param name="claveActual"></param>
        /// <param name="claveNueva"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CambiarClave(string claveActual, string claveNueva)
        {
            try
            {
                string userLogin = string.Empty;

                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                    //else if (!string.IsNullOrEmpty(User.Identity.Name))
                    //        userLogin = User.Identity.Name;


                    int resultado = this.servicio.CambiarClaveUsuario(userLogin, claveActual, claveNueva);

                    return Json(resultado);
                }
                else
                {
                    return Json(-1);
                }
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar la vista de opciones del usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Favoritos()
        {
            string userLogin = string.Empty;
            ViewBag.Menu = string.Empty;
            ViewBag.Nodos = string.Empty;
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                //else if (!string.IsNullOrEmpty(User.Identity.Name))
                //        userLogin = User.Identity.Name;

                List<OptionDTO> list = this.servicio.ObtenerOpcionPorUsuarioFavorito(Constantes.IdAplicacion, userLogin).ToList();

                string menu = Tools.ObtenerTreeOpciones(list, string.Empty);
                ViewBag.Menu = menu;

                List<int> codigos = list.Where(x => x.Favorito > 0).Select(x => x.OptionCode).Distinct().ToList();
                ViewBag.Nodos = string.Join<int>(Constantes.CaracterComa.ToString(), codigos);

            }
            return PartialView();
        }

        /// <summary>
        /// Permite grabar las opciones favoritas del usuario
        /// </summary>
        /// <param name="nodos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarFavoritos(string nodos)
        {
            try
            {
                string userLogin = string.Empty;
                List<int> ids = nodos.Split(Constantes.CaracterComa).Select(int.Parse).ToList();

                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                    //else if (!string.IsNullOrEmpty(User.Identity.Name))
                    //    userLogin = User.Identity.Name;

                    int resultado = this.servicio.GrabarAccesoDirecto(ids.ToArray(), Constantes.IdAplicacion, userLogin);

                    return Json(resultado);
                }
                else
                {
                    return Json(-1);
                }
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar las acciones del header
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AccesoHeader()
        {
            HeaderModel model = new HeaderModel();

            string userLogin = string.Empty;
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                //else if (!string.IsNullOrEmpty(User.Identity.Name))
                //    userLogin = User.Identity.Name;

                List<OptionDTO> opciones = this.servicio.ListarAccesoDirecto(Constantes.IdAplicacion, userLogin).ToList();
                List<MenuModel> list = new List<MenuModel>();

                foreach (OptionDTO item in opciones)
                {
                    list.Add(new MenuModel()
                    {
                        OpcionId = (int)item.OptionCode,
                        PadreId = item.PadreCodi,
                        Descripcion = item.OptionDesc,
                        Nombre = item.OptionName,
                        NroOrden = item.NroOrden,
                        OpcionURL = item.DesUrl,
                        Tipo = item.IndTipo,
                        DesControlador = (!string.IsNullOrEmpty(item.DesArea)) ? item.DesArea + "/" + item.DesController : item.DesController,
                        DesAccion = item.DesAction,
                        DesArea = item.DesArea
                    });
                }

                model.ListaFavoritos = list;
            }
            else
            {
                model.ListaFavoritos = new List<MenuModel>();
            }

            return PartialView(model);
        }

        /// <summary>
        /// Muestra los accesos directos para la vista
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AccesoHome()
        {
            HeaderModel model = new HeaderModel();

            string userLogin = string.Empty;
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                //else if (!string.IsNullOrEmpty(User.Identity.Name))
                //    userLogin = User.Identity.Name;

                List<OptionDTO> opciones = this.servicio.ListarAccesoDirecto(Constantes.IdAplicacion, userLogin).ToList();
                List<MenuModel> list = new List<MenuModel>();

                foreach (OptionDTO item in opciones)
                {
                    list.Add(new MenuModel()
                    {
                        OpcionId = (int)item.OptionCode,
                        PadreId = item.PadreCodi,
                        Descripcion = item.OptionDesc,
                        Nombre = item.OptionName,
                        NroOrden = item.NroOrden,
                        OpcionURL = item.DesUrl,
                        Tipo = item.IndTipo,
                        DesControlador = (!string.IsNullOrEmpty(item.DesArea)) ? item.DesArea + "/" + item.DesController : item.DesController,
                        DesAccion = item.DesAction,
                        DesArea = item.DesArea,
                        AccesoDirecto = item.DesShortCut
                    });
                }

                model.ListaFavoritos = list;
            }
            else
            {
                model.ListaFavoritos = new List<MenuModel>();
            }

            return PartialView(model);
        }


        /// <summary>
        /// Permite establecer el nodo actual consultado
        /// </summary>
        /// <param name="idOpcion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetearOpcion(int idOpcion)
        {
            if (idOpcion > 0)
            {
                Session[DatosSesion.SesionIdOpcion] = idOpcion;
                string str = string.Empty;
                string user = base.UserName;
                List<OptionDTO> opciones = this.servicio.ObtenerOpcionPorUsuario(Constantes.IdAplicacion, user).ToList();
                this.servicio.GrabarEstadistica(idOpcion, user, DateTime.Now);

                this.ObtenerMapaRuta(opciones, idOpcion);

                for (int i = this.ListaMapa.Count - 1; i >= 0; i--)
                {
                    str = str + this.ListaMapa[i] + Constantes.SeparacionMapa;
                }

                Session[DatosSesion.SesionMapa] = str;
                return Json(1);
            }
            else
            {
                Session[DatosSesion.SesionIdOpcion] = null;
                return Json(-1);
            }
        }

        /// <summary>
        /// Muestra el mapa de navegacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarMapa()
        {
            if (Session[DatosSesion.SesionMapa] != null)
            {
                return Json(Session[DatosSesion.SesionMapa].ToString());
            }
            return Json(Constantes.NodoPrincipal);
        }

        ///<summary>
        ///Arma el menu segun la aplicación y el usuario
        ///</summary>
        ///<returns></returns>
        private IList<MenuModel> CrearMenu()
        {
            IList<MenuModel> list = new List<MenuModel>();

            string userLogin = string.Empty;
            if (Session[DatosSesion.SesionUsuario] != null)
                userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
            {
                //else if (!string.IsNullOrEmpty(User.Identity.Name))
                //    userLogin = User.Identity.Name;

                List<OptionDTO> opciones = this.servicio.ObtenerOpcionPorUsuario(Constantes.IdAplicacion, userLogin).ToList();

                foreach (OptionDTO item in opciones)
                {
                    list.Add(new MenuModel()
                    {
                        OpcionId = (int)item.OptionCode,
                        PadreId = item.PadreCodi,
                        Descripcion = item.OptionDesc,
                        Nombre = item.OptionName,
                        NroOrden = item.NroOrden,
                        OpcionURL = item.DesUrl,
                        Tipo = item.IndTipo,
                        DesControlador = (!string.IsNullOrEmpty(item.DesArea)) ? item.DesArea + "/" + item.DesController : item.DesController,
                        DesAccion = item.DesAction,
                        DesArea = item.DesArea
                    });
                }

            }
            return list;
        }

        /// <summary>
        /// Permite obtener la ruta actual
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="idOpcion"></param>
        /// <returns></returns>
        private void ObtenerMapaRuta(List<OptionDTO> lista, int idOpcion)
        {
            OptionDTO dto = lista.Where(x => x.OptionCode == idOpcion).FirstOrDefault();
            if (dto != null)
            {
                this.ListaMapa.Add(dto.OptionName);
                if (dto.PadreCodi != 1)
                {
                    this.ObtenerMapaRuta(lista, dto.PadreCodi);
                }
            }
        }
    }
}
