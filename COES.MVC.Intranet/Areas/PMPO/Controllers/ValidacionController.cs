using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    /// <summary>
    /// Clase controladora de la Validación
    /// </summary>
    public class ValidacionController : BaseController
    {
        readonly ProgramacionAppServicio pmpo = new ProgramacionAppServicio();

        /// <summary>
        /// Muestra Ventana Inicial de VALIDACIÓN COES
        /// </summary>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            RemisionModel model = new RemisionModel();

            model.Mes = pmpo.GetMesElaboracionDefecto().ToString(ConstantesAppServicio.FormatoMes);
            model.ListaFormato = pmpo.ListFormatosPmpoExtranet();
            model.ListaEmpresa = pmpo.GetListaEmpresasPMPO();
            model.ListarTipoinformacion = pmpo.ListarTipoinformacionPmpo();

            pmpo.EliminarArchivosReporteFormatos();

            return View(model);
        }
    }
}