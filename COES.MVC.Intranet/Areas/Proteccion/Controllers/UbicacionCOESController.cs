using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Proteccion.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;


namespace COES.MVC.Intranet.Areas.Proteccion.Controllers
{
    public class UbicacionCOESController : Controller
    {
        /// <summary>
        /// Instancia de clase para el acceso a datos
        /// </summary>
        
        EquipamientoAppServicio servicioEquipamiento = new EquipamientoAppServicio();
        AreaAppServicio servicioArea = new AreaAppServicio();
        ProyectoActualizacionAppServicio proyectoActualzacion = new ProyectoActualizacionAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(UbicacionCOESController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public UbicacionCOESController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("UbicacionCOESController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("UbicacionCOESController", ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            UbicacionCOESModel modelo = new UbicacionCOESModel();
            modelo.ListaTipoArea = servicioEquipamiento.ListProtecciones();

            return View(modelo);
        }

        public PartialViewResult Editar(int id, int idEpr)
        {
            var model = new UbicacionCOESEditarModel();
            model.IdZona = 0;
            model.Nombre = "";
            model.Areacodi = id;
            model.Epareacodi = idEpr;
            if (idEpr > 0) {
                var area = proyectoActualzacion.EprAreaGetById(idEpr);
                if (area != null)
                {
                    model.IdZona = area.Areacodizona ?? 0;
                    model.Nombre = area.Epareanomb;
                }
            }
            
            model.ListaZona = servicioEquipamiento.ListarZonasxNivel(5);
            
            return PartialView("~/Areas/Proteccion/Views/UbicacionCOES/Editar.cshtml", model);
        }

        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost(UbicacionCOESModel datos)
        {
            return View(datos);
        }

        [HttpPost]
        public JsonResult ObtenerUbicacionesCoes(string codigoTipoArea, string nombreArea)
        {
            UbicacionCOESModel model = new UbicacionCOESModel();
            try
            {

                List<EqAreaDTO> lst = servicioArea.ListUbicacionCoes(codigoTipoArea, nombreArea, "0").ToList();

            var listaGrilla =lst.Select(ubicacion => new List<string>
                                    {
                                        ubicacion.Areacodi.ToString(),
                                        ubicacion.Zona,
                                        ubicacion.Areanombenprotec,
                                        ubicacion.Tareaabrev,
                                        ubicacion.Areaabrev,
                                        ubicacion.Areacodi.ToString(),
                                        ubicacion.Flagenprotec
                                    }).ToList();

                model.listaUbicacion = listaGrilla;
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
            }
            return Json(model);
        }

        [HttpPost]
        public PartialViewResult ListaUbicaciones(string sCodigoTipoArea, string sNombre, string sProgramaExistente)
        {

            ListadoUbicacionesModel model = new ListadoUbicacionesModel();
            model.ListaUbicaciones = servicioArea.ListUbicacionCoes(sCodigoTipoArea, sNombre, sProgramaExistente).ToList();
            return PartialView("~/Areas/Proteccion/Views/UbicacionCOES/ListaUbicacion.cshtml", model);
        }

        private ListadoUbicacionCOESGrillaModel ConvertirDatoGrilla(EqAreaDTO eqArea)
        {

            return new ListadoUbicacionCOESGrillaModel
            {
                Areacodi = eqArea.Areacodi,
                Areaabrev = eqArea.Areaabrev,
                Zona = eqArea.Zona,
                Tareaabrev = eqArea.Tareaabrev,
                Areanombenprotec = eqArea.Areanombenprotec,
                Flagenprotec = eqArea.Flagenprotec
            };

        }
        public JsonResult GuardarUbicacion(int iZona, int iCodigo, int iPrCodigo, string sFlag, string sNombre)
        {
            try
            {
                EprAreaDTO oArea = null;
                var dto = proyectoActualzacion.EprAreaGetById(iPrCodigo);
                if (dto == null)
                {
                    oArea = new EprAreaDTO();
                    oArea.Areacodi = iCodigo;
                    oArea.Areacodizona = iZona;
                    oArea.Epareanomb = sNombre;
                    oArea.Epareaestregistro = "1";
                    oArea.Epareausucreacion = User.Identity.Name;
                    proyectoActualzacion.EprAreaSave(oArea);
                }
                else
                {
                    dto.Areacodi = iCodigo;
                    dto.Areacodizona = iZona;
                    dto.Epareanomb = sNombre;
                    dto.Epareaestregistro = "1";
                    dto.Epareausumodificacion = User.Identity.Name;
                    proyectoActualzacion.EprAreaUpdate(dto);
                }
                return Json(1);
            }
            catch (Exception e)
            {
                log.Fatal(NameController, e);
                return Json(-1);
            }
        }

        public JsonResult EliminarUbicacion(int iPrCodigo)
        {
            UbicacionCOESEliminarModel model = new UbicacionCOESEliminarModel();
            try
            {
                var eliminar = proyectoActualzacion.GetCantidadUbicacionSGOCOESEliminar(iPrCodigo);
                if (eliminar.NroEquipos > 0) {
                    model.Estado = 0;
                    model.Mensaje = "No es posible eliminar el registro, dado que existe(n) (" + eliminar.NroEquipos.ToString() + ") equipo(s) SGOCOES asociados a esta ubicación";
                    return Json(model);
                }

                var dto = proyectoActualzacion.EprAreaGetById(iPrCodigo);
                if (dto != null)
                {
                    dto.Epareaestregistro = "0";
                    dto.Epareausumodificacion = User.Identity.Name;
                    proyectoActualzacion.EprAreaDelete_UpdateAuditoria(dto);
                }

                model.Estado = 1;
                return Json(model);
                
            }
            catch (Exception e)
            {
                log.Fatal(NameController, e);
                model.Estado = -1;
                return Json(model);
            }
        }
    }

    


}
