using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Publicaciones.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Publicaciones.Controllers
{
    public class SuscripcionController : Controller
    {
        /// <summary>
        /// Instancia de la clase de servicio respectiva
        /// </summary>
        SubscripcionAppServicio servicio = new SubscripcionAppServicio();

        /// <summary>
        /// Pagina inicial de subscripciones
        /// </summary>
        /// <returns></returns>
        public ActionResult Suscripcion()
        {
            SubscripcionModel model = new SubscripcionModel();
            model.ListaPublicacion = this.servicio.ListarPublicaciones();
            return View(model);
        }

        /// <summary>
        /// Permite grabar los datos de la subscripcion
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult Grabar(SubscripcionModel model)
        {
            try
            {
                WbSubscripcionDTO entity = new WbSubscripcionDTO
                {
                    Subscripapellidos = model.Apellidos,
                    Subscripcodi = model.Codigo,
                    Subscripemail = model.Correo,
                    Subscripempresa = model.Empresa,
                    Subscripestado = Constantes.EstadoActivo,
                    Subscripfecha = DateTime.Now,
                    Subscripnombres = model.Nombres,
                    Subscriptelefono = model.Telefono
                };

                string items = string.Empty;

                if (model.Detalle.Length > 0)
                {
                    items = model.Detalle.Remove(model.Detalle.Length - 1, 1);
                }

                int result = this.servicio.GrabarSubscripcion(entity, items);
                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
