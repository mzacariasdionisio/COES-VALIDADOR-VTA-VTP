using COES.MVC.Intranet.Areas.IndicadoresSup.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Siosein2;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IndicadoresSup.Controllers
{
    public class InformesCUCGEController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InformesCUCGEController));
        private static readonly string NameController = "InformesCUCGEController";
        private readonly Siosein2AppServicio _servicioSiosein2;

        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        public InformesCUCGEController()
        {
            _servicioSiosein2 = new Siosein2AppServicio();
        }

        public ActionResult Index()
        {
            var model = new Siosein2Model
            {
                MesAnio = DateTime.Today.ToString(ConstantesAppServicio.FormatoMes),
                Titulo = "Cargo Unitario Compensación Seguridad Suministro (CUCSS), Reserva Fría y Concepto de compensación de la confiabilidad en la cadena de Suministro Energía"
            };

            return View(model);
        }

        /// <summary>
        /// carga lista InformesCUCGE
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarInformesCUCGE(string mesanio)
        {
            DateTime fecha = DateTime.ParseExact(ConstantesAppServicio.IniDiaFecha + mesanio.Replace(" ", "/"), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            var model = new Siosein2Model();

            var resultado2 = string.Empty;
            var resultado1 = _servicioSiosein2.InformesCUCGEhtml(fecha, ref resultado2);
            model.Resultados = new List<string>() { resultado1, resultado2 };

            return Json(model);
        }
    }
}
