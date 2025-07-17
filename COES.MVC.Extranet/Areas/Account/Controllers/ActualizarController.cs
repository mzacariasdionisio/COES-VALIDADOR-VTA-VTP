using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Areas.Account.Models;
using COES.MVC.Extranet.SeguridadServicio;
using COES.MVC.Extranet.Controllers;

namespace COES.MVC.Extranet.Areas.Account.Controllers
{
    public class ActualizarController : BaseController
    {
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();

            RegistroModel model = new RegistroModel();
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.EMPRCODI > 1).OrderBy(x => x.EMPRNOMB).ToList();
            
            UserDTO usuario = (UserDTO)Session[DatosSesion.SesionUsuario];
            UserDTO user = this.seguridad.ObtenerUsuario(usuario.UserCode);

            model.Nombre = user.UsernName;
            model.EmpresaId = (int)user.EmprCodi;
            model.Email = user.UserEmail;
            model.Telefono = user.UserTlf;
            model.AreaLaboral = user.AreaLaboral;
            model.Cargo = user.UserCargo;

            return View(model);
        }

        /// <summary>
        /// Permite actualizar los datos del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Actualizar(RegistroModel model)
        {
            try
            {
                UserDTO user = (UserDTO)Session[DatosSesion.SesionUsuario];

                UserDTO usuario = new UserDTO();
                usuario.EmprCodi = (short)model.EmpresaId;
                usuario.Empresas = string.Empty;
                usuario.MotivoContacto = model.MotivoContacto;
                usuario.UserEmail = model.Email;
                usuario.UsernName = model.Nombre;
                usuario.UserTlf = model.Telefono;
                usuario.AreaLaboral = model.AreaLaboral;
                usuario.UserCargo = model.Cargo;
                usuario.EmpresaNombre = model.EmpresaNombre;
                usuario.UserCode = user.UserCode;

                int resultado = this.seguridad.ActualizarUsuarioExtranet(usuario);
                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
