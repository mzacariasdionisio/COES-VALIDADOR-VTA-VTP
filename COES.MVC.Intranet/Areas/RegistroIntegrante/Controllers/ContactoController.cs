using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.RegistroIntegrante.Models;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Controllers
{
    public class ContactoController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ContactoController));

        /// <summary>
        /// Instancia de la clase RepresentanteAppServicio
        /// </summary>
        private RepresentanteAppServicio appContacto = new RepresentanteAppServicio();

        public ContactoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ContactoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ContactoController", ex);
                throw;
            }
        }

        /// <summary>
        /// Permite retornar la vista de personas de contactos
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public ActionResult Index(int emprcodi)
        {
            ViewBag.emprcodi = emprcodi;
            return View();
        }

        /// <summary>
        /// Permite pintar la lista de contactos
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public PartialViewResult Listado(int emprcodi)
        {                      
            var DatosContacto = this.appContacto.GetByEmpresaSiRepresentante(emprcodi).
                Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoContacto).ToList();

            ContactoModel modelContacto = new ContactoModel();
            modelContacto.ListaContacto = DatosContacto;

            return PartialView(modelContacto);
        }

        /// <summary>
        /// Permite grabar un nuevo contacto
        /// </summary>
        /// <param name="model">model ContactoModel</param>
        /// <returns></returns>
        public JsonResult GrabarNuevo(ContactoModel model)
        {
            try {

                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

                int idEmpresa = model.EmpresaId;
                //int idEmpresa = 1;// temporal

                SiRepresentanteDTO contacto = new SiRepresentanteDTO();
                contacto.Emprcodi = idEmpresa;
                contacto.Rptetipo = ConstantesRegistroIntegrantes.RepresentanteTipoContacto;
                contacto.Rptebaja = ConstantesRegistroIntegrantes.RpteBajaNo;

                contacto.Rptenombres = model.Nombres;
                contacto.Rpteapellidos = model.Apellidos;
                contacto.Rptecargoempresa = model.CargoEmpresa;
                contacto.Rptetelefono = model.Telefono;
                contacto.Rptetelfmovil= model.TelefonoMovil;
                contacto.Rptecorreoelectronico = model.CorreoElectronico;

                contacto.Rpteusucreacion = base.UserName;                
                contacto.Rptefeccreacion= DateTime.Now;

                contacto.Rpteinicial = ConstantesRegistroIntegrantes.RepresentanteInicialNo;
                appContacto.InsertSiRepresentante(contacto);

                return Json(1);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite cargar los datos del contacto seleccionado
        /// </summary>
        /// <param name="idContacto">Codigo de Contacto</param>
        [HttpPost]
        public PartialViewResult Edicion(int idContacto)
        {
            ContactoModel model = new ContactoModel();
            var DatosRepresentante = this.appContacto.GetByIdSiRepresentante(idContacto);
            if (DatosRepresentante.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoContacto)
            {
                model.RpteCodi = DatosRepresentante.Rptecodi;
                model.Nombres = DatosRepresentante.Rptenombres;
                model.Apellidos = DatosRepresentante.Rpteapellidos;
                model.CargoEmpresa = DatosRepresentante.Rptecargoempresa;
                model.Telefono = DatosRepresentante.Rptetelefono;
                model.TelefonoMovil = DatosRepresentante.Rptetelfmovil;
                model.CorreoElectronico = DatosRepresentante.Rptecorreoelectronico;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar el contacto seleccionado
        /// </summary>
        /// <param name="idContacto">Codigo de contacto</param>
        [HttpPost]
        public JsonResult Eliminar(int idContacto)
        {
            try
            {
                ContactoModel model = new ContactoModel();
                this.appContacto.DeleteSiRepresentante(idContacto);
               
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite actualizar los datos del contacto
        /// </summary>
        /// <param name="model">model ContactoModel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEdicion(ContactoModel model)
        {
            try
            {
                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);                
                SiRepresentanteDTO contacto = this.appContacto.GetByIdSiRepresentante(model.RpteCodi);

                contacto.Rptenombres = model.Nombres;
                contacto.Rpteapellidos = model.Apellidos;

                contacto.Rptecargoempresa = model.CargoEmpresa;
                contacto.Rptetelefono = model.Telefono;
                contacto.Rptetelfmovil = model.TelefonoMovil;
                contacto.Rptecorreoelectronico = model.CorreoElectronico;

                contacto.Rpteusumodificacion  = userLogin.UserCode.ToString();         
                //contacto.Rpteusumodificacion = "fit"; //test
                contacto.Rptefecmodificacion = DateTime.Now;
                
                this.appContacto.UpdateSiRepresentante(contacto);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

    }
}