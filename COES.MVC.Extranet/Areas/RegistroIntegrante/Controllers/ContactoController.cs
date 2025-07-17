using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Areas.RegistroIntegrante.Models;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using System.Configuration;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Controllers
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
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Permite pintar la lista de contactos
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Listado()
        {
            int idEmpresa = base.EmpresaId;

            //L: Representante Legal, R:Responsable del tramite, C:Persona de Contacto
            var DatosContacto = this.appContacto.GetByEmpresaSiRepresentante(idEmpresa).
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
            try
            {
                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

                SiRepresentanteDTO contacto = new SiRepresentanteDTO();
                contacto.Emprcodi = userLogin.EmprCodi;
                contacto.Rptetipo = ConstantesRegistroIntegrantes.RepresentanteTipoContacto;
                contacto.Rptebaja = ConstantesRegistroIntegrantes.RpteBajaNo;

                contacto.Rptenombres = model.Nombres;
                contacto.Rpteapellidos = model.Apellidos;
                contacto.Rptecargoempresa = model.CargoEmpresa;
                contacto.Rptetelefono = model.Telefono;
                contacto.Rptetelfmovil = model.TelefonoMovil;
                contacto.Rptecorreoelectronico = model.CorreoElectronico;

                contacto.Rpteusucreacion = userLogin.UserCode.ToString();
                contacto.Rptefeccreacion = DateTime.Now;

                contacto.Rpteinicial = ConstantesRegistroIntegrantes.RepresentanteInicialNo;
                appContacto.InsertSiRepresentante(contacto);



                ///Envio de Correo
                string toEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];

                string msg = string.Empty;
                string contactocorreo = string.Empty;
                string empresa = string.Empty;

                var appEmpresa = new EmpresaAppServicio();
                var modelEmpresa = appEmpresa.GetByIdSiEmpresa((int)userLogin.EmprCodi);

                //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                empresa = (modelEmpresa.Emprnombrecomercial.Length > 3) ? modelEmpresa.Emprnombrecomercial : modelEmpresa.Emprrazsocial;
                //fin Garantia 9Jul18 

                //correo al coes
                try
                {
                    contactocorreo = model.Nombres + " " + model.Apellidos;
                    msg = RegistroIntegrantesHelper.Contacto_BodyMailAlta(contactocorreo, empresa);
                    log.Info("Contacto - Notificar - Alta de Contacto");
                    COES.Base.Tools.Util.SendEmail(toEmail, null, "Notificacion - Alta de Contacto", msg);

                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                }

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
                //model.Documento = DatosRepresentante.Rptedocidentidad;
                //model.DocumentoAdjunto = DatosRepresentante.Rptedocidentidadadj;
                model.Nombres = DatosRepresentante.Rptenombres;
                model.Apellidos = DatosRepresentante.Rpteapellidos;
                //model.VigenciaPoderAdjunto = DatosRepresentante.Rptevigenciapoder;
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

                contacto.Rpteusumodificacion = userLogin.UserCode.ToString();
                contacto.Rptefecmodificacion = DateTime.Now;

                this.appContacto.UpdateSiRepresentante(contacto);



                ///Envio de Correo
                string toEmail = ConfigurationManager.AppSettings["admRegistroIntegrantes"];

                string msg = string.Empty;
                string contactocorreo = string.Empty;
                string empresa = string.Empty;

                var appEmpresa = new EmpresaAppServicio();
                var modelEmpresa = appEmpresa.GetByIdSiEmpresa((int)userLogin.EmprCodi);

                //Garantia 9Jul18 - nro 2 si no existe nombre debe mostrarse razon social
                empresa = (modelEmpresa.Emprnombrecomercial.Length > 3) ? modelEmpresa.Emprnombrecomercial : modelEmpresa.Emprrazsocial;
                //fin Garantia 9Jul18 

                //correo al coes
                try
                {
                    contactocorreo = model.Nombres + " " + model.Apellidos;
                    msg = RegistroIntegrantesHelper.Contacto_BodyMailEdicion(contactocorreo, empresa);
                    log.Info("Contacto - Notificar - Edición de Contacto");
                    COES.Base.Tools.Util.SendEmail(toEmail, "", "Notificacion - Edición de Contacto", msg);

                }
                catch (Exception ex)
                {
                    log.Error("Error", ex);
                }

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