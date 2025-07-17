using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Campanias.Models;
using COES.MVC.Extranet.Controllers;
using COES.Servicios.Aplicacion.Campanias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
using COES.MVC.Extranet.Helper;

namespace COES.MVC.Extranet.Areas.Campanias.Controllers
{
    public class RevisionEnvioController : BaseController
    {
        CampaniasAppService campaniasAppService = new CampaniasAppService();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CampaniasModel model = new CampaniasModel();
            return View(model);
        }

         [System.Web.Mvc.HttpPost]
        public ActionResult Observacion(int id)
        {
            CampaniasModel model = new CampaniasModel();
                TransmisionProyectoDTO proyecto = campaniasAppService.GetTransmisionProyectoById(id);
                List<int> hojasPeriodo = campaniasAppService.GetDetalleHojaByPericodi(proyecto.Pericodi, Constantes.IndDel);
                model.TransmisionProyecto = proyecto;
                model.ListaHojas = hojasPeriodo;

            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ListadoObservacion(int id)
        {
            List<ObservacionDTO> lista = campaniasAppService.GetObservacionByProyCodi(id);
            CampaniasModel model = new CampaniasModel();
            model.ListaObservacion = lista;
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GrabarRespuestaObservacion(ProyectoModel proyectoModel)
        {
            int result = 0;
            try {
                RespuestaObsDTO respuestaObsDTO = proyectoModel.respuestaObsDTO;
                respuestaObsDTO.IndDel = Constantes.IndDel;
                respuestaObsDTO.UsuarioCreacion = User.Identity.Name;
                if (campaniasAppService.SaveRespuestaObs(respuestaObsDTO)) result = 1;
                else result = 0;
               
               return Json(new { success = true, responseResult = result }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex)
            {
                 return Json(new { success = false, responseResult = result }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EliminarObservacion(int id, int proyCodi)
        {
            int result = 0;
            Boolean deleteF = campaniasAppService.DeleteObservacionById(id, User.Identity.Name);
            if (deleteF)
            {
                string proyestado = Constantes.EstadoObservado;
                string proyestadoini = Constantes.EstadoEnviado;
                campaniasAppService.UpdateProyEstadoByIdProy(proyCodi, proyestado, proyestadoini);

                result = 1;
                return Json(result);
            }
            else
            {
                result = 0;
                return Json(result);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EditarObservacion(ObservacionDTO observacionDTO)
        {
            int result = 0;
            try {
                observacionDTO.UsuarioModificacion = User.Identity.Name;
                campaniasAppService.UpdateObservacion(observacionDTO);
                result = 1;
                return Json(result);
            } catch (Exception ex)
            {
                return Json(result);
            }
        }

         [System.Web.Mvc.HttpPost]
        public JsonResult ConformarObservacion(int id)
        {
            int result = 0;
            try {
                ObservacionDTO observacionDTO = campaniasAppService.GetObservacionById(id);
                observacionDTO.Estado = "Cerrada";
                observacionDTO.UsuarioModificacion = User.Identity.Name;
                campaniasAppService.UpdateObservacion(observacionDTO);
                result = 1;
                return Json(result);
            } catch (Exception ex)
            {
                return Json(result);
            }
        }

        public ActionResult UploadArchivoObservacion(int? id)
        {
            try
            {
                string extension = string.Empty;
                string fileName = string.Empty;
                string fileNameNotPath = string.Empty;
                string nombreReal = string.Empty;
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string path = campaniasAppService.ObtenerPathArchivosCampianas();
                    nombreReal = file.FileName;
                    int indexOf = nombreReal.LastIndexOf('.');
                    extension = nombreReal.Substring(indexOf + 1, nombreReal.Length - indexOf - 1);
                    Guid myuuid = Guid.NewGuid();
                    string myuuidAsString = myuuid.ToString();
                    fileNameNotPath = myuuidAsString + "." + extension;
                    fileName = myuuidAsString + "." + extension;
                    fileName = path + fileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);

                }
                return Json(new { success = true, indicador = 1, extension = extension, fileNameNotPath = fileNameNotPath, nombreReal = nombreReal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ValidarArchivo(ArchivoObsDTO archivoObsDTO)
        {
            archivoObsDTO.IndDel = Constantes.IndDel;
           
            List<ArchivoObsDTO> Lista1DTO = campaniasAppService.GetArchivoObsProyCodiNom(archivoObsDTO.ObservacionId, archivoObsDTO.Tipo, archivoObsDTO.NombreArch);
            return Json(new { success = true, responseResult = Lista1DTO }, JsonRequestBehavior.AllowGet);
        }

         [System.Web.Mvc.HttpPost]
        public JsonResult GrabarRegistroArchivo(ArchivoObsDTO archivoObsDTO)
        {
            int responseResult = 0;
            archivoObsDTO.ArchFechaSubida = DateTime.Now;
            archivoObsDTO.ArchivoId = campaniasAppService.GetLastArchivoObsId();
            archivoObsDTO.IndDel = Constantes.IndDel;
            archivoObsDTO.UsuarioCreacion = User.Identity.Name;

            if (campaniasAppService.SaveArchivoObs(archivoObsDTO))
            {
                responseResult = 1;
                return Json(new { success = true, responseResult = responseResult, id = archivoObsDTO.ArchivoId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                responseResult = 0;
                return Json(new { success = false, responseResult = responseResult, id = 0 }, JsonRequestBehavior.AllowGet);
            }


        }

        [System.Web.Mvc.HttpPost]
        public JsonResult ListarArchivosObservacion(ArchivoObsDTO archivoObsDTO)
        {
            List<ArchivoObsDTO> Lista1DTO = campaniasAppService.GetArchivoObsByObsId(archivoObsDTO.ObservacionId, archivoObsDTO.Tipo);
            return Json(new { success = true, responseResult = Lista1DTO }, JsonRequestBehavior.AllowGet);

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult EliminarFileObservacion(int id)
        {
            int result = 0;
            string usuario = User.Identity.Name;
            if (campaniasAppService.DeleteArchivoObsById(id, usuario))
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return Json(result);
        }

         [System.Web.Mvc.HttpPost]
        public JsonResult EnviarRespuesta(int idProyecto)
        {
            int result = 0;
            try
            {
                TransmisionProyectoDTO tranmsProyecto = campaniasAppService.GetTransmisionProyectoById(idProyecto);
                if (campaniasAppService.UpdateObservacionByProyectoAbs(idProyecto, "Absuelta"))
                {
                    EnvioDto envioDto = new EnvioDto();
                    envioDto.TransmisionProyectoDTO = tranmsProyecto;
                    envioDto.Correos = envioDto.TransmisionProyectoDTO.CorreoUsu;
                    int plantillaCorreo = 327;
                    campaniasAppService.EnviarCorreoNotificacionObservacion(envioDto, plantillaCorreo, false);
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return Json(result);
        }

    }
}
