using COES.MVC.Extranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using COES.MVC.Extranet.SeguridadServicio;
using COES.MVC.Extranet.Helper;
using WSIC2010;
using System.Configuration;
using COES.Framework.Base.Tools;
using System.IO;

namespace COES.MVC.Extranet.Controllers
{
    public class HomeController : BaseController
    {
        SeguridadServicioClient servicio = new SeguridadServicioClient();
        List<string> ListaMapa = new List<string>();
        int IdAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);

        /// <summary>
        /// Pagina de inicio del portal
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult Default()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                //int iRolInterconexion = Convert.ToInt16(ConfigurationManager.AppSettings[DatosConfiguracion.RolInterconexion]);                            
                //var oUsuario = (UserDTO)Session[DatosSesion.SesionUsuario];
                //var lsRoles = servicio.ObtenerRolPorUsuario(oUsuario.UserCode).Where(x => x.Seleccion > 0);

                //ViewBag.AnuncioInterconexion = Constantes.NO;

                //foreach (var oRol in lsRoles)
                //{
                //    if (oRol.RolCode == iRolInterconexion)
                //    {
                //        ViewBag.AnuncioInterconexion = Constantes.SI;
                //    }                    
                //}
                return View();
            }
            else
            {
                string url = ConfigurationManager.AppSettings[RutaDirectorio.InitialUrl].ToString();
                Response.Redirect(url + Constantes.PaginaLogin);
            }

            return View();
        }


        public ActionResult Info()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {

                return View();
            }
            else
            {
                string url = ConfigurationManager.AppSettings[RutaDirectorio.InitialUrl].ToString();
                Response.Redirect(url + Constantes.PaginaLogin + "?op=login");
            }

            return View();
        }

        public ActionResult Autorizacion()
        {

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CerrarSesion()
        {
            Session[DatosSesion.SesionUsuario] = null;
            Session["in_app"] = null;
            FormsAuthentication.SignOut();
            return Json(1);
        }

        public ActionResult CerrarSesionGet()
        {
            Session[DatosSesion.SesionUsuario] = null;
            Session["in_app"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Autenticar(string usuario, string password)
        {
            int resultado = 0;
            UserDTO entidad = this.servicio.AutentificarUsuario(usuario, password, out resultado);

            if (resultado == 1)
            {
                FormsAuthentication.SetAuthCookie(usuario, false);
                Session[DatosSesion.SesionUsuario] = entidad;
            }

            return Json(resultado);
        }

        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            IList<MenuModel> list = new List<MenuModel>();
            list = this.CrearMenu();
            /*
            list.Add(new MenuModel()
                    {
                        OpcionId = (int)1000,
                        PadreId = 381,
                        Descripcion = "Gestión de código para el envío de información",
                        Nombre = "Gestión de código para el envío de información",
                        NroOrden = 4,
                        OpcionURL = null,
                        Tipo = "F",
                        DesControlador = "Transferencias/SolicitudCodigo",
                        DesAccion = "Index"
                    }
            );
            */

            ViewBag.Menu = list;
            ViewBag.UserName = 0;

            if (Session[DatosSesion.SesionUsuario] != null)
            {
                UserDTO entidad = (UserDTO)Session[DatosSesion.SesionUsuario];
                ViewBag.UserName = entidad.UserCode;
            }

            string url = HttpUtility.UrlEncode((new COES.MVC.Extranet.SeguridadServicio.SeguridadServicioClient()).EncriptarUsuario(base.UserName));
            ViewBag.UserEncrypted = url;
            ViewBag.BaseUrlOtherApps = ConfigurationManager.AppSettings[RutaDirectorio.BaseUrlOtherApps].ToString();
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult Header()
        {
            ViewBag.UserName = string.Empty;

            if (Session[DatosSesion.SesionUsuario] != null)
            {
                ViewBag.UserName = ((UserDTO)Session[DatosSesion.SesionUsuario]).UsernName;
                ViewBag.UserEmail = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;
                ViewBag.UserLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin + "@coes.org.pe.jpg";
                List<string> emailList = ConfigurationManager.AppSettings["hideInfoByEmail"].ToString().Split(';').ToList();
                ViewBag.unableInfoByEmail = emailList;
            }

            return PartialView();
        }

        [HttpPost]
        public JsonResult SetearOpcion(int idOpcion)
        {
            if (idOpcion > 0)
            {
                Session[DatosSesion.SesionIdOpcion] = idOpcion;
                string str = string.Empty;
                string user = base.UserName;
                List<OptionDTO> opciones = this.servicio.ObtenerOpcionPorUsuario(this.IdAplicacion, user).ToList();

                this.ObtenerMapaRuta(opciones, idOpcion);
                this.ListaMapa.Add("Principal");

                for (int i = this.ListaMapa.Count - 1; i >= 0; i--)
                {
                    str = str + this.ListaMapa[i] + "<span>/</span>";
                }

                return Json(str);
            }
            else
            {
                Session[DatosSesion.SesionIdOpcion] = null;
                return Json("-1");
            }
        }

        ///<summary>
        ///Arma el menu segun la aplicación y el usuario
        ///</summary>
        ///<returns></returns>
        private IList<MenuModel> CrearMenu()
        {
            IList<MenuModel> list = new List<MenuModel>();
            if (User != null)
            {
                string user = base.UserName;

                UserDTO usuario = servicio.ObtenerUsuarioPorLogin(user);

                List<OptionDTO> opciones = this.servicio.ObtenerOpcionPorUsuario(this.IdAplicacion, user).ToList();

                foreach (OptionDTO item in opciones)
                {

                    if (usuario.EmprCodi == 10494 && item.OptionCode == 282) //Se asume el código de Osignermin => 10494
                    {
                        continue;
                    }


                    list.Add(new MenuModel()
                    {
                        OpcionId = (int)item.OptionCode,
                        PadreId = item.PadreCodi,
                        Descripcion = item.OptionDesc,
                        Nombre = item.OptionName,
                        NroOrden = item.NroOrden,
                        OpcionURL = item.DesUrl,
                        Tipo = item.IndTipo,
                        DesControlador = (item.DesArea != null) ? item.DesArea + "/" + item.DesController : item.DesController,
                        DesAccion = item.DesAction
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

        /// <summary>
        /// Descargar modelo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarModelo()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarModelo()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/costosmarginales.gms";
            return File(fullPath, Constantes.AppExcel, "costosmarginales.gms");
        }

        /// <summary>
        /// Descargar modelo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarManual()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarManual()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/Manual_Ejecución_Modelo_GAMS.pdf";
            return File(fullPath, Constantes.AppExcel, "Manual_Ejecución_Modelo_GAMS.pdf");
        }

        [HttpPost]
        public JsonResult DescargarPresentacionMdp()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        [HttpPost]
        public JsonResult DescargarPresentacionMdp2()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        [HttpPost]
        public JsonResult DescargarManualMdp()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        [HttpPost]
        public JsonResult DescargarAplicativoMdp()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        [HttpPost]
        public JsonResult DescargarCasopruebaMdp()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }
        [HttpPost]
        public JsonResult descargarguiapr16()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }
        [HttpGet]
        public virtual ActionResult ExportarPresentacionMdp()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/Presentacon_aplicativo_MDP.pdf";
                return File(fullPath, Constantes.AppPdf, "Presentacon_aplicativo_MDP.pdf");
            }
            return null;
        }

        [HttpGet]
        public virtual ActionResult ExportarPresentacionMdp2()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/NuevoModeloDespachoCortoPlazoCoes.pdf";
                return File(fullPath, Constantes.AppPdf, "NuevoModeloDespachoCortoPlazoCoes.pdf");
            }
            return null;
        }

        [HttpGet]
        public virtual ActionResult ExportarManualMdp()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/Manual_Usuario_Offline_YUPANA.pdf";
                return File(fullPath, Constantes.AppPdf, "Manual_Usuario_Offline_YUPANA.pdf");
            }
            return null;
        }

        [HttpGet]
        public virtual ActionResult ExportarAplicativoMdp()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/AplicativoYupanaOffline.zip";
                return File(fullPath, Constantes.AppZip, "AplicativoYupanaOffline.zip");
            }
            return null;
        }

        [HttpGet]
        public virtual ActionResult ExportarCasopruebaMdp()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/CasoDePruebaYupana.zip";
                return File(fullPath, Constantes.AppZip, "CasoDePruebaYupana.zip");
            }
            return null;
        }
        [HttpGet]
        public virtual ActionResult ExportarGuiapr16()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/Guia_de_uso_de_servicios_PR16.pdf";
                return File(fullPath, Constantes.AppPdf, "Guia_de_uso_de_servicios_PR16.pdf");
            }
            return null;
        }

        [HttpPost]
        public JsonResult DescargarModeloPR30()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        [HttpPost]
        public JsonResult DescargarManualPR30()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        [HttpPost]
        public JsonResult DescargarCasoPruebaPR30()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        [HttpGet]
        public virtual ActionResult ExportarModeloPR30()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/modelo_OPF_PR30.zip";
                return File(fullPath, Constantes.AppZip, "modelo_OPF_PR30.zip");
            }
            return null;
        }

        [HttpGet]
        public virtual ActionResult ExportarManualPR30()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/ManualUso_OPF_PR30.pdf";
                return File(fullPath, Constantes.AppZip, "ManualUso_OPF_PR30.pdf");
            }
            return null;
        }

        [HttpGet]
        public virtual ActionResult ExportarCasoPruebaPR30()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/casos_OPF_PR30.zip";
                return File(fullPath, Constantes.AppZip, "casos_OPF_PR30.zip");
            }
            return null;
        }
        [HttpGet]
        public JsonResult ListarVideos()
        {
            string directoryPath = ConfigurationManager.AppSettings["FileSystemMediaExtranet"].ToString()+ "\\Info\\VideosInstructivos\\";
            List<FileData> videos = new List<FileData>();


            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);


                FileInfo[] videoFiles = directoryInfo.GetFiles("*.mp4");

                foreach (var file in videoFiles)
                {
                    videos.Add(new FileData { FileName = file.Name, FileUrl = file.FullName });
                }

                return Json(videos, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "El directorio no existe." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult descargarvideo()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
                return Json(1);
            else
                return Json(-1);
        }

        [HttpGet]
        public virtual ActionResult exportarvideo(string nombreArchivo)
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = ConfigurationManager.AppSettings["FileSystemMediaExtranet"].ToString()+ "\\Info\\VideosInstructivos\\" + nombreArchivo;
                return File(fullPath, "mp4", nombreArchivo);
            }
            return null;
        }

    }
}
