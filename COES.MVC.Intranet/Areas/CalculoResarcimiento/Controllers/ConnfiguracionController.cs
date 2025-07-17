using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CalculoResarcimiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.Helper;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Controllers
{
    public class ConfiguracionController : BaseController
    {
        /// <summary>
        /// Instancia de clase de servicios
        /// </summary>
        CalculoResarcimientoAppServicio servicio = new CalculoResarcimientoAppServicio();

        /// <summary>
        /// Muestra la pagina principal de tipos de interrupcion
        /// </summary>
        /// <returns></returns>
        public ActionResult TipoInterrupcion()
        {
            ConfiguracionModel model = new ConfiguracionModel();
            
            return View(model);
        }

        /// <summary>
        /// Muestra la ventana de causas de interrupcion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CausaInterrupcion(int id)
        {
            ConfiguracionModel model = new ConfiguracionModel();
            model.EntidadTipoInterrupcion = this.servicio.ObtenerRegistroTipoInterrupcion(id);
            model.NombreTipoInterrupcion = model.EntidadTipoInterrupcion.Retintnombre;
            model.IdTipoInterrupcion = model.EntidadTipoInterrupcion.Retintcodi;
            return View(model);
        }
                
        /// <summary>
        /// Muestra el listado de tipos de interrupcion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaTipoInterrupcion()
        {
            ConfiguracionModel model = new ConfiguracionModel();
            model.ListaTiposInterrupcion = this.servicio.ObtenerConfiguracionTiposInterrupcion();
            return PartialView(model);
        }

        /// <summary>
        /// Permite listar las causas de interrupcion
        /// </summary>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCausaInterrupcion(int idTipo)
        {
            ConfiguracionModel model = new ConfiguracionModel();
            model.ListaCausasInterrupcion = this.servicio.ObtenerConfiguracionCausasInterrupcion(idTipo);
            return PartialView(model);
        }

        /// <summary>
        /// Permite visualiza la ventana de edicion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EditarTipoInterrupcion(int id)
        {
            ConfiguracionModel model = new ConfiguracionModel();
            
            if(id == 0)
            {
                model.EntidadTipoInterrupcion = new ReTipoInterrupcionDTO();
                model.EntidadTipoInterrupcion.Retintestado = ConstantesAppServicio.Activo;
            }
            else
            {
                model.EntidadTipoInterrupcion = this.servicio.ObtenerRegistroTipoInterrupcion(id);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite visualiza la ventana de edicion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EditarCausaInterrupcion(int id, int idTipo)
        {
            ConfiguracionModel model = new ConfiguracionModel();
            model.EntidadTipoInterrupcion = this.servicio.ObtenerRegistroTipoInterrupcion(idTipo);

            if (id == 0)
            {
                model.EntidadCausaInterrupcion = new ReCausaInterrupcionDTO();
                model.EntidadCausaInterrupcion.Recintestado = ConstantesAppServicio.Activo;
            }
            else
            {
                model.EntidadCausaInterrupcion = this.servicio.ObtenerRegistroCausaInterrupcion(id);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar el tipo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarTipoInterrupcion(int id)
        {
            return Json(this.servicio.EliminarTipoInterrupcion(id));
        }

        /// <summary>
        /// Permite eliminar la causa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarCausaInterrupcion(int id)
        {
            return Json(this.servicio.EliminarCausaInterrupcion(id));
        }

        /// <summary>
        /// Permite grabar el tipo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarTipoInterrupcion(int id, string nombre, string estado)
        {
            ReTipoInterrupcionDTO entity = new ReTipoInterrupcionDTO();
            entity.Retintcodi = id;
            entity.Retintnombre = nombre;
            entity.Retintestado = estado;

            return Json(this.servicio.GrabarTipoInterrupcion(entity, base.UserName));
            
        }

        /// <summary>
        /// Permite grabar la causa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCausaInterrupcion(int id, string nombre, string estado, int idTipo)
        {
            ReCausaInterrupcionDTO entity = new ReCausaInterrupcionDTO();
            entity.Retintcodi = idTipo;
            entity.Recintcodi = id;
            entity.Recintnombre = nombre;
            entity.Recintestado = estado;

            return Json(this.servicio.GrabarCausaInterrupcion(entity, base.UserName));

        }
    }
}
