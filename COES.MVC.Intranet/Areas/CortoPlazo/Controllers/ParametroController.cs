using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CortoPlazo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class ParametroController : BaseController
    {
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();

        /// <summary>
        /// Pantalla Inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ParametroModel model = new ParametroModel();
            return View(model);
        }

        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listar()
        {
            ParametroModel model = new ParametroModel();
            model.ListaParametro = this.servicio.ListCmParametros();

            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos de la subscripcion
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int id)
        {
            ParametroModel model = new ParametroModel();
            
            if (id == 0)
            {
                model.Entidad = new CmParametroDTO();
            }
            else
            {
                model.Entidad = this.servicio.GetByIdCmParametro(id);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la relación de equivalencia
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                this.servicio.DeleteCmParametro(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar la relación de equivalencia
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(ParametroModel model)
        {
            try
            {
                CmParametroDTO entity = new CmParametroDTO
                {
                     Cmparcodi = model.Codigo,
                     Cmparnombre = model.Descripcion,
                     Cmparinferior = model.ValorInferior,
                     Cmparsuperior = model.ValorSuperior,
                     Cmparvalor = model.Color,
                     Cmparlastuser = base.UserName,
                     Cmparlastdate = DateTime.Now
                };

                int result = this.servicio.SaveCmParametro(entity);
                               
                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

    }
}
