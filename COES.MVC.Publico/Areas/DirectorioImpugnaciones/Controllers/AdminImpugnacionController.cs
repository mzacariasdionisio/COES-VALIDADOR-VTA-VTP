using COES.MVC.Publico.Areas.DirectorioImpugnaciones.Models;
using COES.Servicios.Aplicacion.DirectorioImpugnaciones;
using COES.MVC.Publico.Helper;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Framework.Base.Tools;
using System.IO;
using COES.MVC.Publico.SeguridadServicio;
using System.Configuration;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Publico.Areas.DirectorioImpugnaciones.Controllers
{
    public class AdminImpugnacionController : Controller
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
        /// Permite administratrar los documentos de impugnacion
        /// El parámetro puede ser: 1:Dirección Ejecutiva, 2:Directorio y 3:Recursos de Apelación
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public ActionResult Index(int tipo)
        {
            try
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    UserDTO usuario = (UserDTO)Session[DatosSesion.SesionUsuario];
                    if (Helper.Helper.ValidarAccesoAdministrador(usuario.UserCode, Constantes.RolDirectorioImpugnacion))
                    {
                        Session[DatosSesion.SesionUsuario] = usuario;
                        ImpugnacionesModel model = new ImpugnacionesModel();
                        model.TipoImpugnacion = servicio.GetByIdWbTipoimpugnacion(tipo);
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
                                ImpugnacionesModel model = new ImpugnacionesModel();
                                model.TipoImpugnacion = servicio.GetByIdWbTipoimpugnacion(tipo);
                                ViewBag.tipoImpugnacion = tipo;
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

        /// <summary>
        /// Permite Listar las impugnaciones de un mes con las opciones de etidar y eliminar
        /// </summary>
        /// <param name="tipoImpugnacion"></param>
        /// <param name="fecAnio"></param>
        /// <param name="fecMes"></param>
        /// <returns></returns>
        public PartialViewResult ListaAdministrar(int tipoImpugnacion, string fecAnio, string fecMes)
        {
            ImpugnacionesModel model = new ImpugnacionesModel();
            DateTime fecha = DateTime.ParseExact(fecMes + ' ' + fecAnio, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
            model.ListaImpugnaciones = servicio.GetByCriteriaWbImpugnacions(tipoImpugnacion, fecha);
            model.TipoImpugnacion = servicio.GetByIdWbTipoimpugnacion(tipoImpugnacion);

            return PartialView(model);
        }

        /// <summary>
        /// Permite subir al servidor el nuevo archivo
        /// </summary>
        /// <param name="chunks"></param>
        /// <param name="chunk"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
                            FileServer.UploadFromFileDirectory(path + name, RutaDirectorio.DirectorioImpugnaciones, name, string.Empty);
                        }
                    }
                    else
                    {
                        var file = Request.Files[0];
                        FileServer.UploadFromStream(file.InputStream, RutaDirectorio.DirectorioImpugnaciones, name, string.Empty);
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
        /// Permite insertar o actualizar un registro en la base de datos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(ImpugnacionesModel model)
        {
            try
            {
                COES.Dominio.DTO.Sic.WbImpugnacionDTO entity = new WbImpugnacionDTO();
                entity.Timpgcodi = model.TipoImpugn;
                entity.Impgtitulo = model.Titulo;
                entity.Impgnumeromes = model.NumeroMes;
                entity.Impgregsgdoc = model.RegistroSGDOC;
                entity.Impginpugnante = model.Impugnante;
                entity.Impgdescinpugnad = model.DecisionImpugnada;
                entity.Impgpetitorio = model.Petitorio;
                if (model.FecRecepcion != null)
                    entity.Impgfechrecep = DateTime.ParseExact(model.FecRecepcion, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.FecPublicacion != null)
                    entity.Impgfechpubli = DateTime.ParseExact(model.FecPublicacion, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.PlazoIncorporacion != null)
                    entity.Impgplazincorp = DateTime.ParseExact(model.PlazoIncorporacion, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Impgincorpresent = model.IncorporacionesPresentadas;
                entity.Impgdescdirecc = model.Decision;
                if (model.FecDesicion != null)
                    entity.Impgfechdesc = DateTime.ParseExact(model.FecDesicion, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Impgdiastotaten = (model.DiasTotalesAtencion != null) ? (int)model.DiasTotalesAtencion : 0;
                entity.Impgmesanio = DateTime.ParseExact("01/" + model.FecMes + "/" + model.FecAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Impgextension = model.Extension;
                if (model.Nuevo == "S")
                {
                    entity.Impgfechacreacion = DateTime.Now;
                    entity.Impgusuariocreacion = UserName;
                    int id = servicio.SaveWbImpugnacion(entity);
                    FileServer.RenameBlob(RutaDirectorio.DirectorioImpugnaciones, model.NombreArchivo + "." + model.Extension, "IMPG" + id + "." + model.Extension, string.Empty);
                }
                else if (model.Nuevo == "N")
                {
                    entity.Impgcodi = model.Codigo;
                    entity.Impgfechaupdate = DateTime.Now;
                    entity.Impgusuarioupdate = UserName;
                    servicio.UpdateWbImpugnacion(entity);
                    if (model.CambiarArchivo == "S")
                    {
                        FileServer.DeleteBlob(RutaDirectorio.DirectorioImpugnaciones + "IMPG" + model.Codigo + "." + model.ExtensionAntiguo, string.Empty);
                        FileServer.RenameBlob(RutaDirectorio.DirectorioImpugnaciones, model.NombreArchivo + "." + model.Extension, "IMPG" + model.Codigo + "." + model.Extension, string.Empty);
                    }
                }
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite visualizar la vista de edicion de una impugnacion
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="id"></param>
        /// <param name="fecAnio"></param>
        /// <param name="fecMes"></param>
        /// <returns></returns>
        public ActionResult Editar(int tipo, string id, string fecAnio, string fecMes)
        {
            ImpugnacionesModel model = new ImpugnacionesModel();
            if (string.IsNullOrEmpty(id))
            {
                model.Nuevo = "S";
                model.Impugnacion = new Dominio.DTO.Sic.WbImpugnacionDTO();
                model.Impugnacion.Impgmesanio = DateTime.ParseExact("01/" + fecMes + "/" + fecAnio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            else
            {
                model.Nuevo = "N";
                model.Impugnacion = servicio.GetByIdWbImpugnacion(int.Parse(id));
            }
            model.TipoImpugnacion = servicio.GetByIdWbTipoimpugnacion(tipo);
            return View(model);
        }

        /// <summary>
        /// Permite eliminar un documento de impugnacion a travez del Id
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public JsonResult Eliminar(int codigo)
        {
            if (codigo != null)
            {
                try
                {
                    ImpugnacionesModel model = new ImpugnacionesModel();
                    model.Impugnacion = servicio.GetByIdWbImpugnacion(codigo);
                    string url = RutaDirectorio.DirectorioImpugnaciones + "IMPG" + model.Impugnacion.Impgcodi + "." + model.Impugnacion.Impgextension;
                    servicio.DeleteWbImpugnacion(model.Impugnacion.Impgcodi);
                    FileServer.DeleteBlob(url, string.Empty);
                    return Json(1);
                }
                catch
                {
                    return Json(-1);
                }

            }
            return Json(-1);
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
                ImpugnacionesModel model = new ImpugnacionesModel();
                model.Impugnacion = servicio.GetByIdWbImpugnacion(id);
                string url = "IMPG" + model.Impugnacion.Impgcodi + "." + model.Impugnacion.Impgextension;

                if (FileServer.VerificarExistenciaFile(RutaDirectorio.DirectorioImpugnaciones, url, string.Empty))
                {
                    System.IO.Stream stream = FileServer.DownloadToStream(RutaDirectorio.DirectorioImpugnaciones + url, string.Empty);
                    return File(stream, model.Impugnacion.Impgextension, model.Impugnacion.Impgtitulo + "." + model.Impugnacion.Impgextension);
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
