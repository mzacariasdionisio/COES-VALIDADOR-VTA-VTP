using COES.MVC.Extranet.Areas.Account.Models;
using COES.MVC.Extranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Account.Controllers
{
    public class RegistroController : Controller
    {
        /// <summary>
        /// Referencia al servicio web de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Pagina inicial del registro
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RegistroModel model = new RegistroModel();
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();
            model.ListaModulos = this.seguridad.ListarModulos().ToList();
            return View(model);
        }

        /// <summary>
        /// Permite grabar los datos del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(RegistroModel model)
        {
            try
            {
                UserDTO usuario = new UserDTO();
                usuario.AreaCode = -1;
                usuario.EmprCodi = (short)model.EmpresaId;
                usuario.Empresas = string.Empty;
                usuario.MotivoContacto = model.MotivoContacto;
                usuario.UserEmail = model.Email;
                usuario.UsernName = model.Nombre + " " + model.Apellido;
                usuario.UserTlf = model.Telefono;
                usuario.AreaLaboral = model.AreaLaboral;
                usuario.UserCargo = model.Cargo;
                usuario.Modulos = model.Modulos;
                usuario.EmpresaNombre = model.EmpresaNombre;

                int resultado = this.seguridad.RegistrarSolicitudUsuario(usuario);
                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar los datos del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult DatosEnviados(RegistroModel model)
        {
            return PartialView(model);
        }
    

    }
}
