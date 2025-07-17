using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RegistroIntegrante.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Controllers
{
    public class AdminController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio 
        /// </summary>
        HistoricoAppServicio servicio = new HistoricoAppServicio();

        /// <summary>
        /// Acción principal de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            AdminModel model = new AdminModel();
            model.Listado = this.servicio.ListAnios();
            return View(model);
        }

        /// <summary>
        /// Muestra la ventana de listado
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int anio, string tipo)
        {
            AdminModel model = new AdminModel();
            model.Listado = this.servicio.GetByCriteriaRiHistoricos(anio, tipo);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el formulario de edición
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int id)
        {
            AdminModel model = new AdminModel();

            if (id == 0)
            {
                model.Entidad = new RiHistoricoDTO();
            }
            else
            {
                model.Entidad = this.servicio.GetByIdRiHistorico(id);
                model.Fecha = (model.Entidad.Hisrifecha != null) ? ((DateTime)model.Entidad.Hisrifecha).ToString(Constantes.FormatoFecha) : string.Empty;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Acción que permite grabar los datos del formulario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(AdminModel model)
        {
            try
            {
                RiHistoricoDTO entity = new RiHistoricoDTO
                {
                    Hisrianio = model.Anio,
                    Hisridesc = model.Descripcion,
                    Hisriestado = Constantes.EstadoActivo,
                    Hisricodi = model.Codigo,
                    Hisriusucreacion = base.UserName,
                    Hisrifecha = DateTime.ParseExact(model.Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                    Hisritipo = model.TipoOperacion
                };

                this.servicio.SaveRiHistorico(entity);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar el registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                this.servicio.DeleteRiHistorico(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
