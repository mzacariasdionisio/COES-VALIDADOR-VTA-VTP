using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Publico.Areas.MarcoNormativo.Models;
using COES.MVC.Publico.Helper;
using COES.MVC.Publico.SeguridadServicio;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.MarcoNormativo.Controllers
{
    public class DecisionEjecutivaController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicios
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();
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
        public ActionResult Index()
        {
            try
            {
                if (Session[DatosSesion.SesionUsuario] != null)
                {
                    UserDTO usuario = (UserDTO)Session[DatosSesion.SesionUsuario];
                    if (Helper.Helper.ValidarAccesoAdministrador(usuario.UserCode, Constantes.RolDirectorioImpugnacion))
                    {
                        Session[DatosSesion.SesionUsuario] = usuario;                       
                        return View();
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
                                return View();
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
        /// Muestra el listado
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listar(string tipo)
        {
            DecisionEjecutivaModel model = new DecisionEjecutivaModel();
            model.ListaDecisiones = this.servicio.GetByCriteriaWbDecisionejecutivas(tipo);

            string path = RutaDirectorio.DirectorioNotasTecnicas;

            foreach(WbDecisionejecutivaDTO itemFile in model.ListaDecisiones)
            {
                string filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnica, itemFile.Desejecodi, itemFile.Desejeextension);

                if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                {
                    itemFile.Icono = Util.ObtenerIcono(ConstantesBase.TipoFile, Constantes.CaracterPunto + itemFile.Desejeextension);
                    itemFile.FileName = filename;
                }

                foreach (WbDecisionejecutadoDetDTO item in itemFile.ListaItems)
                {
                    filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnicaDet, itemFile.Desejecodi, item.Dejdetcodi, item.Desdetextension);

                    if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                    {
                        item.Icono = Util.ObtenerIcono(ConstantesBase.TipoFile, Constantes.CaracterPunto + item.Desdetextension);
                        item.FileName = filename;
                    }
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar un registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                //- Eliminamos los archivos asociados
                WbDecisionejecutivaDTO entity = this.servicio.GetByIdWbDecisionejecutiva(id);

                string path = RutaDirectorio.DirectorioNotasTecnicas;
                string filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnica, entity.Desejecodi, entity.Desejeextension);

                if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                {
                    FileServer.DeleteBlob(path + filename, string.Empty);
                }

                foreach (WbDecisionejecutadoDetDTO item in entity.ListaItems)
                {
                    filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnicaDet, entity.Desejecodi, item.Dejdetcodi, item.Desdetextension);

                    if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                    {
                        FileServer.DeleteBlob(path + filename, string.Empty);
                    }
                }

                //- Eliminamos el registro en base de datos
                this.servicio.DeleteWbDecisionejecutiva(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Muestra la venta de edicion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Editar(int id, string tipo, string indicador)
        {
            DecisionEjecutivaModel model = new DecisionEjecutivaModel();

            if (id == 0)
            {
                model.Entidad = new WbDecisionejecutivaDTO();
                model.Entidad.Desejecodi = 0;
                model.Entidad.Desejetipo = tipo;
                model.Entidad.Desejefechapub = DateTime.Now;
                model.Entidad.ListaCarta = new List<WbDecisionejecutadoDetDTO>();
                model.Entidad.ListaAntecedentes = new List<WbDecisionejecutadoDetDTO>();
            }
            else
            {
                model.Entidad = this.servicio.GetByIdWbDecisionejecutiva(id);

                string path = RutaDirectorio.DirectorioNotasTecnicas;
                string filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnica, model.Entidad.Desejecodi, model.Entidad.Desejeextension);

                if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                {
                    model.Entidad.Icono = Util.ObtenerIcono(ConstantesBase.TipoFile, Constantes.CaracterPunto + model.Entidad.Desejeextension);
                    model.Entidad.FileName = filename;
                }

                foreach (WbDecisionejecutadoDetDTO item in model.Entidad.ListaItems)
                {
                    filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnicaDet, model.Entidad.Desejecodi, item.Dejdetcodi, item.Desdetextension);

                    if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                    {
                        item.Icono = Util.ObtenerIcono(ConstantesBase.TipoFile, Constantes.CaracterPunto + item.Desdetextension);
                        item.FileName = filename;
                    }
                }
            }

            model.Indicador = indicador;

            return View(model);
        }

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <param name="opcion">1: Decision Ejecutiva, 2: Nota Técnica</param>
        /// <param name="codigo">ID del Registro Principal</param>
        /// <param name="indicador">1: Archivo principal, 2: Carta notificación, 3: Antecendentes</param>
        /// <returns></returns>
        public ActionResult Upload(int chunks, int chunk, string name, int codigo, int indicador)
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
                            this.GrabarDetalle(codigo, indicador, name, path + name, null, 1);
                        }
                    }
                    else
                    {
                        var file = Request.Files[0];
                        this.GrabarDetalle(codigo, indicador, name, string.Empty, file.InputStream, 0);
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
        /// Permite grabar los archivos detalle
        /// </summary>
        /// <param name="opcion">1: Decision Ejecutiva, 2: Nota Técnica</param>
        /// <param name="codigo">ID del Registro Principal</param>
        /// <param name="indicador">1: Archivo principal, 2: Carta notificación, 3: Antecendentes</param>
        /// <param name="modo">Si es de archivo o directamente la carga del archivo</param>
        private void GrabarDetalle(int codigo, int indicador, string file, string path, Stream strFile, int modo)
        {
            //- Obteniendo la extension del archivo
            string fileName = string.Empty;
            int indexOf = file.LastIndexOf('.');
            string extension = file.Substring(indexOf + 1, file.Length - indexOf - 1);

            //- Grabamos el archivo principal del registro
            if (indicador == ConstantesPortal.ArchivoPrincipal)
            {
                fileName = string.Format(ConstantesPortal.PrefijoFileNotaTecnica, codigo, extension);
                if (modo == 0)
                    FileServer.UploadFromStream(strFile, RutaDirectorio.DirectorioNotasTecnicas, fileName, string.Empty);
                else
                    FileServer.UploadFromFileDirectory(path, RutaDirectorio.DirectorioNotasTecnicas, fileName, string.Empty);

                this.servicio.ActualizarDecisionEjecutiva(codigo, extension, file);
            }

            //- Caso para las cartas y los antecedentes

            if (indicador == ConstantesPortal.ArchivoCarta || indicador == ConstantesPortal.ArchivoAntecendente)
            {
                //- Grabando el regisgtro en base de datos
                int idDetalle = 0;
                WbDecisionejecutadoDetDTO entity = new WbDecisionejecutadoDetDTO
                {
                    Dejdetcodi = idDetalle,
                    Dejdetdescripcion = file,
                    Dejdetfile = file,
                    Dejdetestado = Constantes.EstadoActivo,
                    Dejdettipo = indicador.ToString(),
                    Desejecodi = codigo,
                    Desdetextension = extension
                };

                idDetalle = this.servicio.SaveWbDecisionejecutadoDet(entity);

                //- Grabamos el archivo en el repositorio
                fileName = string.Format(ConstantesPortal.PrefijoFileNotaTecnicaDet, codigo, idDetalle, extension);
                if (modo == 0)
                    FileServer.UploadFromStream(strFile, RutaDirectorio.DirectorioNotasTecnicas, fileName, string.Empty);
                else
                    FileServer.UploadFromFileDirectory(path, RutaDirectorio.DirectorioNotasTecnicas, fileName, string.Empty);
            }
        }


        /// <summary>
        /// Permite grabar los datos de la decisión ejecutiva
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="tipoRegistro"></param>
        /// <param name="fecha"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int codigo, int tipoRegistro, string fecha, string descripcion, string[][] datos)
        {
            try
            {
                WbDecisionejecutivaDTO entity = new WbDecisionejecutivaDTO
                {
                    Desejecodi = codigo,
                    Desejedescripcion = descripcion,
                    Desejeestado = Constantes.EstadoActivo,
                    Desejefechapub = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                    Desejetipo = tipoRegistro.ToString(),
                    Lastdate = DateTime.Now,
                    Lastuser = this.UserName
                };

                int result = this.servicio.SaveWbDecisionejecutiva(entity);

                if (codigo > 0) 
                {
                    this.servicio.ActualizarDescripcionItemDecision(datos);
                }

                return Json(result);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite descargar el archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string file)
        {
            try
            {
                if (FileServer.VerificarExistenciaFile(RutaDirectorio.DirectorioNotasTecnicas, file, string.Empty))
                {
                    int indexOf = file.LastIndexOf('.');
                    string extension = file.Substring(indexOf + 1, file.Length - indexOf - 1);
                    System.IO.Stream stream = FileServer.DownloadToStream(RutaDirectorio.DirectorioNotasTecnicas + file, string.Empty);
                    return File(stream, extension, file);
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

        /// <summary>
        /// Permite eliminar un archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarItem(int id)
        {
            try
            {
                //- Eliminamos primero el archivo del fileserver
                WbDecisionejecutadoDetDTO entity = this.servicio.GetByIdWbDecisionejecutadoDet(id);
                string path = RutaDirectorio.DirectorioNotasTecnicas;
                string filename = string.Format(ConstantesPortal.PrefijoFileNotaTecnicaDet, entity.Desejecodi, entity.Dejdetcodi, entity.Desdetextension);

                if (FileServer.VerificarExistenciaFile(path, filename, string.Empty))
                {
                    FileServer.DeleteBlob(path + filename, string.Empty);
                }

                //- Eliminar el registro de base de datos
                this.servicio.DeleteWbDecisionejecutadoDet(id);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


    }
}
