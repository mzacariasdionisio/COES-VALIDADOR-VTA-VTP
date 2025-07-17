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

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class RelacionBarraController : Controller
    {
        DemandaPOAppServicio servicio = new DemandaPOAppServicio();

        public ActionResult Index()
        {
            RelacionBarraModel model = new RelacionBarraModel();
            model.ListaVersiones = this.servicio.ListaVersiones();
            model.ListaFormulasVegetativa = this.servicio.ListaPerfilRuleForEstimadorByPrefijo(ConstantesDpo.prefijoFormulasDPO);
            model.ListaFormulasIndustrial = this.servicio.ListaPerfilRuleForEstimadorByPrefijo(ConstantesDpo.prefijoFormulasDPO);

            return View(model);
        }

        /// <summary>
        /// Lista las barras SPL que pertenecen a una version
        /// </summary>
        /// <param name="idVersion">identificador de la version</param>
        /// <returns></returns>
        public JsonResult ListaBarrasxVersion(int idVersion)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            model.ListaBarras = this.servicio.ListaBarrasPorVersion(idVersion);
            return Json(model);
        }

        /// <summary>
        /// Genera las listas y objetos para el popup de relacion
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerarRelacion()
        {
            RelacionBarraModel model = new RelacionBarraModel();
            model.ListaBarrasSPL = this.servicio.ListaBarraSpl().Where(x => x.Barsplestado == "A").ToList();
            return Json(model);
        }

        /// <summary>
        /// Genera las listas y objetos para el popup de conversion de una barra normal a SPL
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerarBarraSPL()
        {
            RelacionBarraModel model = new RelacionBarraModel();
            model.ListaBarrasGrupo = this.servicio.ListaBarrasGrupo().Where(x => x.Catecodi == 10).ToList();
            model.ListaBarrasSPL = this.servicio.ListaBarraSpl().Where(x => x.Barsplestado == "A").ToList();
            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Registra el nuevo juego de barras SPL
        /// </summary>
        /// <returns></returns>
        public JsonResult RegistrarBarrasSPL(int[] idBarras)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            this.servicio.RegistraBarraSPL(idBarras, User.Identity.Name);
            return Json(model);
        }

        /// <summary>
        /// Registra el nuevo juego de barras SPL
        /// </summary>
        /// <param name="nombVersion">Nombre de la version</param>
        /// <param name="idBarras">Ids de las barras asignadas a una version</param>
        /// <returns></returns>
        public JsonResult RegistrarNuevaVersion(string nombVersion, int[] idBarras)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            this.servicio.RegistraNuevaVersion(nombVersion, idBarras, User.Identity.Name);
            model.ListaVersiones = this.servicio.ListaVersiones();
            return Json(model);
        }

        /// <summary>
        /// Elimina por completo un version creada(asi tenga barras y formulas relacionadas)
        /// </summary>
        /// <param name="id">Identificador de la version</param>
        /// <returns></returns>
        public JsonResult EliminarVersion(int id)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            this.servicio.EliminarVersion(id);
            model.ListaVersiones = this.servicio.ListaVersiones();
            return Json(model);
        }

        /// <summary>
        /// Actualiza la relacion de la version con las barras SPL
        /// </summary>
        /// <param name="version">Identificador de la version</param>
        /// <returns></returns>
        public JsonResult CargarRelacion(int version)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            //model.ListaBarrasGrupo = this.servicio.ListaBarrasGrupo().Where(x => x.Catecodi == 10).ToList();
            model.ListaBarrasSPL = this.servicio.ListaBarraSpl();
            model.ListaBarras = this.servicio.ListaBarrasxVersion(version);
            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Actualiza una version
        /// </summary>
        /// <param name="version">Id de la version</param>
        /// <param name="barras">Ids de las barras asignadas a una version</param>
        /// <returns></returns>
        public JsonResult ActualizarVersion(int version, int[] barras)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            this.servicio.ActualizaVersion(version, barras, User.Identity.Name);
            //model.ListaVersiones = this.servicio.ListaVersiones();
            return Json(model);
        }

        /// <summary>
        /// Actualiza un registro de la relacion, se agrego Formulas
        /// </summary>
        /// <param name="entity">Entidad DpoRelSplFormulaDTO</param>
        /// <param name="flag">Fla para limpiar o editar</param>
        /// <returns></returns>
        public JsonResult ActualizarFila(DpoRelSplFormulaDTO entity, int flag)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            this.servicio.ActualizarRegistro(entity, flag);
            //model.ListaBarras = this.servicio.ListaBarrasxVersion(entity.Dposplcodi);
            return Json(model);
        }

        /// <summary>
        /// Elimina un registro de la relacion
        /// </summary>
        /// <param name="id">Identificador de DpoRelSplFormulaDTO</param>
        /// <returns></returns>
        public JsonResult EliminarFila(int id)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            this.servicio.DeleteDpoRelSplFormula(id);
            return Json(model);
        }
    }
}