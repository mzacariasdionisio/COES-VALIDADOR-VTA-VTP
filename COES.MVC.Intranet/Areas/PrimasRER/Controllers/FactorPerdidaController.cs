using System;
using System.Linq;
using log4net;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using COES.Servicios.Aplicacion.PrimasRER;
using System.Reflection;
using COES.MVC.Intranet.Controllers;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class FactorPerdidaController : BaseController
    {
        // GET: /PrimasRER/FactorPerdida/

        public FactorPerdidaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        PrimasRERAppServicio servicioPrimasRER = new PrimasRERAppServicio();

        /// <summary>
        /// PrimasRER.2023
        /// Mostrar la vista principal de FactorPerdida
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            return View();
        }

        /// <summary>
        /// PrimasRER.2023
        /// Muestra la lista de datos de la central RER
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista()
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            model.ListFacPerMedDTO = this.servicioPrimasRER.ListRerFacPerMeds();
            return PartialView(model);
        }

        /// <summary>
        /// PrimasRER.2023
        /// Permite eliminar un registro de la db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            base.ValidarSesionUsuario();
            PrimasRERModel model = new PrimasRERModel();
            model.ListFacPerMedDetDTO = this.servicioPrimasRER.ListRerFacPerMedDets().Where(det => det.Rerfpmcodi == id).ToList(); 
            foreach (var facPerMedDet in model.ListFacPerMedDetDTO)
            {
                this.servicioPrimasRER.DeleteRerFacPerMedDet(facPerMedDet.Rerfpdcodi);
            }
            this.servicioPrimasRER.DeleteRerFacPerMed(id);
            return "true";
        }

    }
}