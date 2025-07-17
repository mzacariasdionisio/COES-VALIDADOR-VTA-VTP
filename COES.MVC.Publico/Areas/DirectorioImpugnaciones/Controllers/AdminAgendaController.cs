using COES.Framework.Base.Tools;
using COES.MVC.Publico.Areas.DirectorioImpugnaciones.Models;
using COES.MVC.Publico.Helper;
using COES.MVC.Publico.SeguridadServicio;
using COES.Servicios.Aplicacion.DirectorioImpugnaciones;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.DirectorioImpugnaciones.Controllers
{
    public class AdminAgendaController : Controller
    {
        /// <summary>
        /// Instancias de las clases
        /// </summary>
        DirectorioImpugnacionesAppServicio servicio = new DirectorioImpugnacionesAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        public string UserName
        {
            get
            {
                return (Session[DatosSesion.SesionUsuario] != null) ?
                    ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin : string.Empty;
            }
        }

        /// <summary>
        /// Permite administrar la agenda.
        /// El tipo puede ser 1:Directorio y 2:Asamblea
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public ActionResult Index(int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 1:
                        ViewBag.Title = "Administrar Agenda del Directorio";
                        break;
                    case 2:
                        ViewBag.Title = "Administrar Agenda de la Asamblea";
                        break;
                }
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    UserDTO usuario = (UserDTO)Session[DatosSesion.SesionUsuario];
                    if (Helper.Helper.ValidarAccesoAdministrador(usuario.UserCode, Constantes.RolDirectorioImpugnacion))
                    {
                        Session[DatosSesion.SesionUsuario] = usuario;
                        EventoAgendaModel model = new EventoAgendaModel();
                        model.Tipo = tipo;
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction(Constantes.ActionAutorizacion, Constantes.ControladorHome);
                    }
                }
                else
                {
                    Session[DatosSesion.SesionUsuario] = null;
                    if (Request[RequestParameter.RequestUser] != null)
                    {
                        string request = Request[RequestParameter.RequestUser].ToString();
                        string user = this.seguridad.DesencriptarUsuario(request);
                        UserDTO usuario = this.seguridad.ObtenerUsuarioPorLogin(user);
                        if (usuario != null)
                        {
                            if (Helper.Helper.ValidarAccesoAdministrador(usuario.UserCode, Constantes.RolDirectorioImpugnacion))
                            {
                                Session[DatosSesion.SesionUsuario] = usuario;
                                EventoAgendaModel model = new EventoAgendaModel();
                                model.Tipo = tipo;
                                return View(model);
                            }
                            else
                            {
                                return RedirectToAction(Constantes.ActionAutorizacion, Constantes.ControladorHome);
                            }
                        }
                        else
                        {
                            return RedirectToAction(Constantes.ActionAutorizacion, Constantes.ControladorHome);
                        }
                    }
                    else
                    {
                        return RedirectToAction(Constantes.ActionAutorizacion, Constantes.ControladorHome);
                    }
                }
            }
            catch
            {
                return RedirectToAction(Constantes.ActionAutorizacion, Constantes.ControladorHome);
            }
            
        }

        [HttpPost]
        public PartialViewResult ListaAdministrar(int tipo, string fecAnio)
        {
            EventoAgendaModel model = new EventoAgendaModel();
            model.ListaEventosAgenda = servicio.ListWbEventoagendas(tipo, fecAnio);
            return PartialView(model);
        }

        /// <summary>
        /// Funcion que permite eliminar un registro de la BD y el archivo adjunto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int  id)
        {
            try
            {
                EventoAgendaModel model = new EventoAgendaModel();
                model.EventoAgenda = servicio.GetByIdWbEventoagenda(id);
                string url = RutaDirectorio.DirectorioEventos + "EVEAG" + model.EventoAgenda.Eveagcodi + "." + model.EventoAgenda.Eveagextension;
                servicio.DeleteWbEventoagenda(id);
                FileServer.DeleteBlob(url, string.Empty);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public ActionResult Upload(int chunks, int chunk, string name)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    if (chunks > 1)
                    {
                        var file = Request.Files[0];
                        string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                        using (var fs = new FileStream(Path.Combine(path, name), chunk == 0 ? FileMode.Create : FileMode.Append))
                        {
                            var buffer = new byte[file.InputStream.Length];
                            file.InputStream.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, buffer.Length);
                        }

                        if (chunk == chunks - 1)
                        {
                            FileServer.UploadFromFileDirectory(path + name, RutaDirectorio.DirectorioEventos, name, string.Empty);
                        }
                    }
                    else
                    {
                        var file = Request.Files[0];
                        FileServer.UploadFromStream(file.InputStream, RutaDirectorio.DirectorioEventos, name, string.Empty);
                    }
                }
                return Json(new { success = true, indicador = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Funcion que graba o actualiza un evento en la base de datos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(EventoAgendaModel model)
        {
            COES.Dominio.DTO.Sic.WbEventoagendaDTO entity = new Dominio.DTO.Sic.WbEventoagendaDTO();
            try
            {
                entity.Eveagtitulo = model.Titulo;
                entity.Eveagtipo = model.Tipo;
                entity.Eveagdescripcion = model.Descripcion;
                entity.Eveagubicacion = model.Ubicacion;
                entity.Eveagfechinicio = DateTime.ParseExact(model.HoraInicio, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                entity.Eveagfechfin = DateTime.ParseExact(model.HoraFin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                entity.Eveagextension = model.Extension;
                if (model.Nuevo == "S")
                {
                    entity.Eveagfechacreacion = DateTime.Now;
                    entity.Eveagusuariocreacion = this.UserName;
                    int id = servicio.SaveWbEventoagenda(entity);
                    FileServer.RenameBlob(RutaDirectorio.DirectorioEventos, model.NombreArchivo + "." + model.Extension, "EVEAG" + id + "." + model.Extension, string.Empty);
                }
                else if (model.Nuevo == "N")
                {
                    entity.Eveagcodi = model.Codigo;
                    entity.Eveagfechaupdate = DateTime.Now;
                    entity.Eveagusuarioupdate = this.UserName;
                    servicio.UpdateWbEventoagenda(entity);
                    if (model.CambiarArchivo == "S")
                    {
                        FileServer.DeleteBlob(RutaDirectorio.DirectorioEventos + "EVEAG" + model.Codigo + "." + model.ExtensionAntiguo, string.Empty);
                        FileServer.RenameBlob(RutaDirectorio.DirectorioEventos, model.NombreArchivo + "." + model.Extension, "EVEAG" + model.Codigo + "." + model.Extension, string.Empty);
                    }
                }
                return Json(1);
            }
            catch
            {

            }
            return Json(-1);
        }

        /// <summary>
        /// Permite ver vista para agregar o editar un nuevo evento en la agenda
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Editar(int tipo, string id)
        {
            EventoAgendaModel model = new EventoAgendaModel();
            model.Tipo = tipo;
            if (string.IsNullOrEmpty(id))
            {
                model.Nuevo = "S";
                model.EventoAgenda = new Dominio.DTO.Sic.WbEventoagendaDTO();
            }
            else
            {
                model.Nuevo = "N";
                model.EventoAgenda = servicio.GetByIdWbEventoagenda(int.Parse(id));
            }
            switch (tipo)
            {
                case 1:
                    ViewBag.Title = "Editar Agenda del Directorio";
                    break;
                case 2:
                    ViewBag.Title = "Editar Agenda de la Asamblea";
                    break;
            }
            return View(model);
        }

        /// <summary>
        /// Permite descargar el archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(int id)
        {
            try
            {
                EventoAgendaModel model = new EventoAgendaModel();
                model.EventoAgenda = servicio.GetByIdWbEventoagenda(id);
                string url = RutaDirectorio.DirectorioEventos + "EVEAG" + model.EventoAgenda.Eveagcodi + "." + model.EventoAgenda.Eveagextension;

                if (FileServer.VerificarExistenciaFile(RutaDirectorio.DirectorioEventos, "EVEAG" + model.EventoAgenda.Eveagcodi + "." + model.EventoAgenda.Eveagextension, string.Empty))
                {
                    System.IO.Stream stream = FileServer.DownloadToStream(url, string.Empty);
                    return File(stream, model.EventoAgenda.Eveagextension, model.EventoAgenda.Eveagtitulo + "." + model.EventoAgenda.Eveagextension);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
