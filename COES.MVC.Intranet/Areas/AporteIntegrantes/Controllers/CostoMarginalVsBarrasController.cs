using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Controllers;
using log4net;
using System.Reflection;
using COES.Servicios.Aplicacion.Transferencias;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class CostoMarginalVsBarrasController : BaseController
    {
        public CostoMarginalVsBarrasController()
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

        CalculoPorcentajesAppServicio servicioCalculoPorcentaje = new CalculoPorcentajesAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();

        // GET: /AporteIntegrantes/CostoMarginalVsBarras/

        public ActionResult Index(){
            CostoMarginalVsBarrasModel model = new CostoMarginalVsBarrasModel();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista() {
            CostoMarginalVsBarrasModel model = new CostoMarginalVsBarrasModel();
            Log.Info("Lista Equisddpbarr - ListCaiEquisddpbarrs");
            //cree otra sentencia sql para traer tambien los nombres de los fk
            model.ListaEquisddpbarr = this.servicioCalculoPorcentaje.ListCaiEquisddpbarrs2();
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }

        public ActionResult New()
        {
            CostoMarginalVsBarrasModel model = new CostoMarginalVsBarrasModel();
            model.Entidad = new CaiEquisddpbarrDTO();
            if(model.Entidad==null)
            {
                return HttpNotFound();
            }

            model.Entidad.Casddbcodi = 0;
            model.Casddbfecvigencia = DateTime.Now.ToString("dd/MM/yyyy");
            Log.Info("Lista Barra - ListBarras");
            model.ListaBarras = this.servicioBarra.ListBarras();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CostoMarginalVsBarrasModel model) {

            if (ModelState.IsValid) 
            {
                CaiEquisddpbarrDTO dto = new CaiEquisddpbarrDTO();
                dto.Barrcodi = model.Entidad.Barrcodi;
                dto.Casddbbarra = model.Entidad.Casddbbarra;
                dto.Casddbusumodificacion = User.Identity.Name;
                dto.Casddbfecmodificacion = DateTime.Now;
                if (model.Casddbfecvigencia != "" && model.Casddbfecvigencia != null)
                    dto.Casddbfecvigencia = DateTime.ParseExact(model.Casddbfecvigencia, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                if (model.Entidad.Casddbcodi == 0) {
                    dto.Casddbusucreacion = User.Identity.Name;
                    dto.Casddbfeccreacion = DateTime.Now;
                    Log.Info("Inserta registro - SaveCaiEquisddpbarr");
                    this.servicioCalculoPorcentaje.SaveCaiEquisddpbarr(dto);
                }
                else
                {
                    dto.Casddbcodi = model.Entidad.Casddbcodi;
                    Log.Info("Actualiza registro - UpdateCaiEquisddpbarr");
                    this.servicioCalculoPorcentaje.UpdateCaiEquisddpbarr(dto);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //error
            model.sError = "Se ha producido un error al insertar la información";
            model.Casddbfecvigencia = DateTime.Now.ToString("dd/MM/yyyy");
            Log.Info("Lista Barra - ListBarras");
            model.ListaBarras = this.servicioBarra.ListBarras();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model); 
        }

        public ActionResult Edit(int id) {

            CostoMarginalVsBarrasModel model = new CostoMarginalVsBarrasModel();
            Log.Info("Entidad CaiEquisddpbarr - GetByIdCaiEquisddpbarr");
            model.Entidad = this.servicioCalculoPorcentaje.GetByIdCaiEquisddpbarr(id);

            if (model.Entidad == null) {
                return HttpNotFound();
            }
            Log.Info("Lista Barra - ListBarras");
            model.ListaBarras = this.servicioBarra.ListBarras();
            model.Casddbfecvigencia = model.Entidad.Casddbfecvigencia.Value.ToString("dd/MM/yyyy");
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0) 
        {
            string salida = "";
            try
            {
                Log.Info("Elimina Registro - DeleteCaiEquisddpbarr");
                this.servicioCalculoPorcentaje.DeleteCaiEquisddpbarr(id);
                salida = "true";
            }
            catch
            {
                salida = "false";
            }
            return salida;
        }

        public ActionResult View(int id = 0)
        {
            CostoMarginalVsBarrasModel modelo = new CostoMarginalVsBarrasModel();
            Log.Info("Entidad CaiEquisddpbarr - GetByIdCaiEquisddpbarr");
            modelo.Entidad = this.servicioCalculoPorcentaje.GetByIdCaiEquisddpbarr2(id);
            return PartialView(modelo);
        }

    }
}
