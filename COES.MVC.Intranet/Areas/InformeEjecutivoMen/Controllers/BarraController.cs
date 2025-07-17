using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.InformeEjecutivoMen.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Siosein2;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.InformeEjecutivoMen.Controllers
{
    public class BarraController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NotaController));
        private static readonly string NameController = "BarraController";
        private readonly AdministracionAppServicio servicioBarras;
        private readonly BarraAppServicio servicioBarraTrandferencia;

        public BarraController()
        {
            servicioBarras = new AdministracionAppServicio();
            servicioBarraTrandferencia = new BarraAppServicio();
        }

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
       
        public ActionResult Index()
        {
            BarrasModel model = new BarrasModel();
            model.ListaBarraTransferencia = servicioBarraTrandferencia.ListaBarraTransferencia();
            return View(model);

        }

        /// <summary>
        /// Lista de relación de barras de transferencia y área operativa
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult listarBarras()
        {
            BarrasModel model = new BarrasModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.ListadoBarras = servicioBarras.ListarBarraAreas();
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
        public JsonResult DetallarBarra(int bararecodi)
        {
            BarrasModel model = new BarrasModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.Barra = servicioBarras.GetByIdTrnBarraArea(bararecodi);                
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
        public JsonResult GuardarBarra(int bararecodi, int barrcodi, string bararearea, string barareejecutiva, int accion)
        {           
            BarrasModel model = new BarrasModel();
            try
            {
                base.ValidarSesionJsonResult();
                TrnBarraAreaDTO objBarra = new TrnBarraAreaDTO();                
                objBarra.Bararecodi = bararecodi;
                objBarra.Barrcodi = barrcodi;
                objBarra.Bararearea = bararearea;
                objBarra.Barareejecutiva = barareejecutiva;                                                
                string usuario = base.UserName;
                servicioBarras.GuardarDatosBarra(objBarra, accion, usuario);
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
        public JsonResult EliminarBarra(int bararecodi)
        {
            BarrasModel model = new BarrasModel();

            try
            {                            
                base.ValidarSesionJsonResult();
                servicioBarras.DeleteTrnBarraArea(bararecodi);
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
        public PartialViewResult Agrupacion(int zona)
        {
            BarrasModel model = new BarrasModel();
            model.ListaAgrupacion = this.servicioBarras.ObtenerAgrupacionBarraPorZona(zona);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult EliminarAgrupacion(int zona, string agrupacion)
        {
            return Json(this.servicioBarras.EliminarAgrupacion(zona, agrupacion));
        }

        [HttpPost]
        public PartialViewResult DetalleAgrupacion(int zona, string agrupacion)
        {
            BarrasModel model = new BarrasModel();
            model.Zona = zona;
            model.NombreAgrupacion = agrupacion;
            model.ListaBarraTransferencia = servicioBarraTrandferencia.ListaBarraTransferencia();
            model.ListaBarra = servicioBarras.ObtenerBarrasPorAgrupacion(zona, agrupacion);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GrabarAgrupacion(int zona, string agrupacion, string nombre, string barras)
        {
            return Json(servicioBarras.GrabarAgrupacionBarra(zona, agrupacion, nombre, barras, base.UserName));
        }
    }
}
