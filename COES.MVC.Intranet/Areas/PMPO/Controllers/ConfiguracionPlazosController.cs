using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using log4net;
using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class ConfiguracionPlazosController : BaseController
    {
        readonly ProgramacionAppServicio pmpo = new ProgramacionAppServicio();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Muestra Ventana de Configuración de Parametro
        /// </summary>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            RemisionModel model = new RemisionModel();

            model.Mes = pmpo.GetMesElaboracionDefecto().ToString(ConstantesAppServicio.FormatoMes);
            model.ListaFormato = pmpo.ListFormatosPmpoExtranet();
            model.ListaEmpresa = pmpo.GetListaEmpresasPMPO();
            model.ListarTipoinformacion = pmpo.ListarTipoinformacionPmpo();

            //precargar el popup de nueva ampliacion
            model.Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
            if (model.ListaEmpresa.Count > 0)
                model.ListaFormatoAmpl = pmpo.ListarTipoPMPOXEmpresa(model.ListaEmpresa[0].Emprcodi);

            return View(model);
        }

        /// <summary>
        /// Reporte de envíos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="formatcodi"></param>
        /// <param name="mesElaboracion"></param>
        /// <param name="estEnvio"></param>
        /// <param name="estDerivacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaAmpliacion(int emprcodi, int formatcodi, string mesElaboracion)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                string url = Url.Content("~/");

                model.PlazoFormato = pmpo.GetPlazoFormatoPmpoXFecha(fecha1Mes);
                model.ListaAmpliacion = pmpo.ListarAmpliacionXMes(formatcodi, emprcodi, fecha1Mes);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ListaFormatoXEmpresa(int emprcodi)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                model.ListaFormato = pmpo.ListarTipoPMPOXEmpresa(emprcodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GuardarAmpliacion(int emprcodi, int formatcodi, string mesElaboracion, string sfechaAmpl, int horaAmpl)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                DateTime amplifechaplazo = DateTime.ParseExact(sfechaAmpl, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture).AddMinutes(horaAmpl * 30);
                if (horaAmpl == 48) amplifechaplazo = amplifechaplazo.AddSeconds(-1);

                if (amplifechaplazo < DateTime.Now) throw new ArgumentException("La fecha de ampliación debe ser posterior a la fecha y hora actual del sistema.");

                MeAmpliacionfechaDTO reg = new MeAmpliacionfechaDTO();
                reg.Amplifecha = fecha1Mes;
                reg.Emprcodi = emprcodi;
                reg.Formatcodi = formatcodi;
                reg.Amplifechaplazo = amplifechaplazo;
                reg.Lastdate = DateTime.Now;
                reg.Lastuser = base.UserName;

                pmpo.GuardarAmpliacion(reg);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ObtenerAmpliacion(int emprcodi, int formatcodi, string mesElaboracion)
        {
            RemisionModel model = new RemisionModel();

            try
            {
                DateTime fecha1Mes = DateTime.ParseExact(mesElaboracion, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                model.ListaFormato = pmpo.ListarTipoPMPOXEmpresa(emprcodi);
                model.Ampliacion = pmpo.GetByIdMeAmpliacionfecha(fecha1Mes, emprcodi, formatcodi);
                model.Fecha = model.Ampliacion.Amplifechaplazo.ToString(ConstantesAppServicio.FormatoFecha);

                int mediaHora = model.Ampliacion.Amplifechaplazo.Hour * 2 + model.Ampliacion.Amplifechaplazo.Minute / 30;
                mediaHora = mediaHora + 1;
                model.Ampliacion.MediaHora = mediaHora;

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }
    }
}