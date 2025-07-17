using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.DemandaPO.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class RelacionPuntoBarraController : Controller
    {
        DemandaPOAppServicio servicio = new DemandaPOAppServicio();
        PronosticoDemandaAppServicio servicioPronostico = new PronosticoDemandaAppServicio();

        public ActionResult Index()
        {
            List<DpoRelSplFormulaDTO> relacion = this.servicio.ListaBarrasPorVersion(0);

            RelacionPuntoBarraModel model = new RelacionPuntoBarraModel();
            model.ListaVersiones = this.servicio.ListaVersiones();
            model.ListaBarrasSPL = new List<DpoRelSplFormulaDTO>();
            model.ListaPuntosTna =  this.servicioPronostico.ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaSco);
            return View(model);
        }

        /// Lista las barras SPL que pertenecen a una version
        /// </summary>
        /// <param name="version">identificador de la version</param>
        /// <returns></returns>
        public JsonResult RecargaBarra(int version)
        {
            RelacionPuntoBarraModel model = new RelacionPuntoBarraModel();
            model.ListaBarrasSPL = this.servicio.ListaBarrasPorVersion(version);
            model.ListaBarrasGrilla = this.servicio.ListaBarraPuntoVersion(version);
            return Json(model);
        }

        /// <summary>
        /// Lista las barras SPL que pertenecen a una version
        /// </summary>
        /// <param name="idVersion">identificador de la version</param>
        /// <returns></returns>
        //public JsonResult ListaBarrasxVersion(int idVersion)
        //{
        //    RelacionPuntoBarraModel model = new RelacionPuntoBarraModel();
        //    model.ListaBarrasGrilla = this.servicio.ListaBarraPuntoVersion(idVersion);
        //    return Json(model);
        //}

        /// <summary>
        /// Actualiza un registro de la relacion, se agrego Formulas
        /// </summary>
        /// <param name="relacion">identificador de DpoRelSplFormulaDTO</param>
        /// <param name="punto">identificador de MePtoMedicion</param>
        /// <param name="codigo">identificador de DpoRelacionPtoBarra</param>
        /// <param name="version">identificador de DpoRelacionPtoBarra</param>
        /// <returns></returns> 
        public JsonResult ActualizarFila(int relacion, int punto, int codigo, int version)
        {
            RelacionPuntoBarraModel model = new RelacionPuntoBarraModel();
            try
            {
                if (punto == 0)
                {
                    model.Mensaje = "Seleccione un punto TNA...";
                    model.TipoMensaje = "warning";
                }
                else {
                    this.servicio.RegistrarRelacionPtoBarra(relacion, punto, codigo, User.Identity.Name);
                    model.ListaBarrasGrilla = this.servicio.ListaBarraPuntoVersion(version);
                    model.Mensaje = "Se registro la relacion con exito...";
                    model.TipoMensaje = "success";
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message + " - " + ex.StackTrace;
                model.TipoMensaje = "error";
                return Json(model);
            }
        }

        /// <summary>
        /// Elimina por un registro de la tabla DPO_RELACION_PTOBARRA
        /// </summary>
        /// <param name="codigo">Identificador de la tabla DPO_RELACION_PTOBARRA</param>
        /// <returns></returns>
        public JsonResult EliminarFila(int codigo, int version)
        {
            RelacionPuntoBarraModel model = new RelacionPuntoBarraModel();
            try
            {
                if (codigo == -1)
                {
                    model.Mensaje = "No se puede eliminar la relacion porque no existe...";
                    model.TipoMensaje = "warning";
                }
                else {
                    this.servicio.DeleteDpoRelacionPtoBarra(codigo);
                    model.ListaBarrasGrilla = this.servicio.ListaBarraPuntoVersion(version);
                    model.Mensaje = "Se elimino la relacion con exito...";
                    model.TipoMensaje = "success";
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message + " - " + ex.StackTrace;
                model.TipoMensaje = "error";
                return Json(model);
            }
        }
    }
}