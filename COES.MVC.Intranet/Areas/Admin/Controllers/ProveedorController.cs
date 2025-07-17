using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class ProveedorController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio correspondiente
        /// </summary>
        TramiteVirtualAppServicio servicio = new TramiteVirtualAppServicio();

        /// <summary>
        /// Código de módulo asociado
        /// </summary>
        public int IdModulo = 31;

        /// <summary>
        /// Muestra la pantalla inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ProveedorModel model = new ProveedorModel();
            
            return View(model);
        }

        [HttpPost]
        public PartialViewResult Empresas(int tipoEmpresa)
        {
            ProveedorModel model = new ProveedorModel();
            model.ListadoEmpresa = this.servicio.ObtenerEmpresasProveedor(tipoEmpresa);
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener el listado de correos
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Correos(int idEmpresa)
        {
            ProveedorModel model = new ProveedorModel();
            model.ListaEmpresaCorreo = this.servicio.ObtenerCorreosPorEmpresa(idEmpresa, IdModulo);
            model.IdEmpresa = idEmpresa;
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult Listado(int idEmpresa)
        {
            ProveedorModel model = new ProveedorModel();
            model.ListaEmpresaCorreo = this.servicio.ObtenerCorreosPorEmpresa(idEmpresa, IdModulo);           
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ObtenerCorreo(int idEmpresaCorreo)
        {
            return Json(this.servicio.ObtenerCorreo(idEmpresaCorreo));
        }

        /// <summary>
        /// Permite eliminar los datos del correo
        /// </summary>
        /// <param name="idEmpresaCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarCorreo(int idEmpresaCorreo)
        {
            try
            {
                this.servicio.EliminarCuentaCorreo(idEmpresaCorreo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// permite grabar los datos de la nueva cuenta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEmpresaCorreo"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCorreo(int idEmpresa, int idEmpresaCorreo, string email)
        {
            try
            {
                this.servicio.GrabarCuenta(idEmpresa, idEmpresaCorreo, email, base.UserName, IdModulo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite crear las credenciales
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CrearCredenciales(int idEmpresa)
        {
            try
            {
                int result = this.servicio.CrearCredenciales(idEmpresa);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }
    }
}
