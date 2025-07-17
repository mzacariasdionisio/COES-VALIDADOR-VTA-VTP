using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using COES.MVC.Intranet.Areas.RegistroIntegrante.Models;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Controllers
{
    public class EmpresaController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary> 
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EmpresaController));

        /// <summary>
        /// Instancia de la clase EmpresaAppServicio
        /// </summary>
        private EmpresaAppServicio appEmpresa = new EmpresaAppServicio();

        public EmpresaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EmpresaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EmpresaController", ex);
                throw;
            }
        }

        /// <summary>
        /// Permite retornar la vista empresa
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public ActionResult Index(int emprcodi)
        {
            ViewBag.emprcodi = emprcodi;

            EmpresaModel model = new EmpresaModel();
            var Empresa = appEmpresa.GetByIdSiEmpresa(emprcodi);

            model.Emprcodi            = Empresa.Emprcodi;
            model.Emprruc             = Empresa.Emprruc;
            model.Emprnombrecomercial = Empresa.Emprnombrecomercial;
            model.Emprrazsocial       = Empresa.Emprrazsocial;
            model.Emprdomiciliolegal  = Empresa.Emprdomiciliolegal;
            model.Emprsigla           = Empresa.Emprsigla;
            model.Emprnumpartidareg   = Empresa.Emprnumpartidareg;
            model.Emprtelefono        = Empresa.Emprtelefono;
            model.Emprfax             = Empresa.Emprfax;
            model.Emprpagweb          = Empresa.Emprpagweb;
            model.Emprnroregistro =    Empresa.Emprnroregistro;

            return View(model);            
        }

        /// <summary>
        /// Permite ACTUALIZAR los datos de la empresa de la gestion de modificacion
        /// </summary>
        /// <param name="domicilioLegal">domicilio legal</param>
        /// <param name="telefono">telefono</param>
        /// <param name="fax">fax</param>
        /// <param name="paginaWeb">Pagina web</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarGestionModificacion(int idEmpresa, string nombreComercial, string razonSocial,
            string domicilioLegal, string sigla, string nroPartida, string telefono, string fax, string paginaWeb, string nroRegistro)
        {
            try
            {
                appEmpresa.ActualizarEmpresaGestionModificacion(idEmpresa, nombreComercial, razonSocial, domicilioLegal, sigla, nroPartida, telefono, fax, paginaWeb, nroRegistro);                

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