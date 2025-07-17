using COES.MVC.Intranet.Areas.Mediciones.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mediciones.Controllers
{
    public class ConfiguracionController : Controller
    {
        DespachoAppServicio logic = new DespachoAppServicio();

        /// <summary>
        /// Permite cargar la pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ConfiguracionModel model = new ConfiguracionModel();
            model.ListaTipoGrupo = this.logic.GetByCriteriaPrTipogrupos();
            return View(model);
        }

        /// <summary>
        /// Realizar un listado de los grupos
        /// </summary>
        /// <param name="idTipoGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int idTipoGrupo)
        {
            ConfiguracionModel model = new ConfiguracionModel();
            model.ListaGrupo = this.logic.ObtenerMantenimientoGrupoRER(idTipoGrupo);
            model.TipoGrupoCodi = idTipoGrupo;
            return PartialView(model);
        }

        /// <summary>
        /// Permite actualizar el tipo de grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idTipoGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CambiarTipoGrupo(int idGrupo, int idTipoGrupo, int idTipoGrupo2, string tipoGenerRer)
        {
            try
            {
                if (idTipoGrupo != 2)
                {
                    idTipoGrupo2 = int.Parse(Constantes.ParametroDefecto);
                    tipoGenerRer = Constantes.NO;
                }

                this.logic.CambiarTipoGrupo(idGrupo, idTipoGrupo, idTipoGrupo2, tipoGenerRer, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar la ventana para modificación del grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="emprNomb"></param>
        /// <param name="grupoNomb"></param>
        /// <param name="grupoAbrev"></param>
        /// <param name="idTipoGrupo"></param>
        /// <param name="idTipoGrupo2"></param>
        /// <param name="tipoGenerRer"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Edicion(int idGrupo, string emprNomb, string grupoNomb,
            string grupoAbrev, int idTipoGrupo, int idTipoGrupo2, string tipoGenerRer)
        {
            ConfiguracionModel model = new ConfiguracionModel();

            model.IdGrupo = idGrupo;
            model.EmpresaNombre = emprNomb;
            model.GrupoNombre = grupoNomb;
            model.GrupoAbreviacion = grupoAbrev;
            model.TipoGrupoCodi = idTipoGrupo;
            model.TipoGrupoCodi2 = idTipoGrupo2;
            model.IndAdjudicada = tipoGenerRer;
            model.ListaTipoGrupo = this.logic.GetByCriteriaPrTipogrupos();

            return PartialView(model);
        }

    }
}
