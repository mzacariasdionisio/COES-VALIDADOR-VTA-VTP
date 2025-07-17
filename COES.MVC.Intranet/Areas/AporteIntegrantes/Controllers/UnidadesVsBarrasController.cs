using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using log4net;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class UnidadesVsBarrasController : BaseController
    {
        //
        // GET: /AporteIntegrantes/UnidadesVsBarras/


        public UnidadesVsBarrasController()
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

        CalculoPorcentajesAppServicio servicioCalculoPorcentajes = new CalculoPorcentajesAppServicio();
        CentralGeneracionAppServicio servicioEquipo = new CentralGeneracionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        CentralGeneracionAppServicio servicioCentralGeneracion = new CentralGeneracionAppServicio();
        FormatoMedicionAppServicio servicioFormatoMedicion = new FormatoMedicionAppServicio();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Lista(string nombre) {

            UnidadesVsBarrasModel model = new UnidadesVsBarrasModel();
            Log.Info("Lista Equiunidbarr - GetByCriteriaCaiEquiunidbarrs");
            model.ListaEquiunidbarr = this.servicioCalculoPorcentajes.GetByCriteriaCaiEquiunidbarrs();
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }


        public ActionResult View(int id = 0)
        {
            UnidadesVsBarrasModel model = new UnidadesVsBarrasModel();
            Log.Info("Entidad Equiunidbarr - GetByIdCaiEquiunidbarr");
            model.Entidad = this.servicioCalculoPorcentajes.GetByIdCaiEquiunidbarr(id);
            return PartialView(model);
        }

        public ActionResult New()
        {
            UnidadesVsBarrasModel modelo = new UnidadesVsBarrasModel();

            modelo.Entidad = new CaiEquiunidbarrDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }

            Log.Info("Lista PtoMedicion - ListMePtomedicion");
            modelo.ListaPtoMedicion = this.servicioCalculoPorcentajes.ListMePtomedicion("", "-1").ToList();
            Log.Info("Lista Empresa - ListEmpresasSTR");
            modelo.ListaEmpresa = this.servicioEmpresa.ListEmpresasSTR();
            Log.Info("Lista Central Generación - ListCentralGeneracion");
            modelo.ListaEquiCen = this.servicioCentralGeneracion.ListCentralGeneracion();
            Log.Info("Lista Central Unidad - ListUnidad");
            modelo.ListaEquiUni = this.servicioCentralGeneracion.ListUnidad();
            Log.Info("Lista Barras - ListBarras");
            modelo.ListaBarras = this.servicioBarra.ListBarras();
            modelo.Caiunbfecvigencia = DateTime.Now.ToString("dd/MM/yyyy");
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            return PartialView(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UnidadesVsBarrasModel modelo)
        {
            if (ModelState.IsValid)
            {
                if (modelo.Caiunbfecvigencia != "" && modelo.Caiunbfecvigencia != null)
                    modelo.Entidad.Caiunbfecvigencia = DateTime.ParseExact(modelo.Caiunbfecvigencia, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                modelo.Entidad.Caiunbusumodificacion = User.Identity.Name;
                modelo.Entidad.Caiunbfecmodificacion = DateTime.Now;
                    
                if (modelo.Entidad.Caiunbcodi == 0)
                {
                    modelo.Entidad.Caiunbusucreacion = User.Identity.Name;
                    modelo.Entidad.Caiunbfeccreacion = DateTime.Now;
                    Log.Info("Insertar registro - SaveCaiEquiunidbarr");
                    this.servicioCalculoPorcentajes.SaveCaiEquiunidbarr(modelo.Entidad);
                }
                else
                {
                    Log.Info("Actualiza registro - UpdateCaiEquiunidbarr");
                    this.servicioCalculoPorcentajes.UpdateCaiEquiunidbarr(modelo.Entidad);
                }

                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }

            modelo.sError = "Se ha producido un error al insertar la información";
            Log.Info("Lista PtoMedicion - ListMePtomedicion");
            modelo.ListaPtoMedicion = this.servicioCalculoPorcentajes.ListMePtomedicion("", "-1").ToList();
            Log.Info("Lista Empresa - ListEmpresasSTR");
            modelo.ListaEmpresa = this.servicioEmpresa.ListEmpresasSTR();
            Log.Info("Lista Central Generación - ListCentralGeneracion");
            modelo.ListaEquiCen = this.servicioCentralGeneracion.ListCentralGeneracion();
            Log.Info("Lista Central Unidad - ListUnidad");
            modelo.ListaEquiUni = this.servicioCentralGeneracion.ListUnidad();
            Log.Info("Lista Barras - ListBarras");
            modelo.ListaBarras = this.servicioBarra.ListBarras();
            modelo.Caiunbfecvigencia = DateTime.Now.ToString("dd/MM/yyyy");
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(modelo);
        }

        public ActionResult Edit(int id)
        {
            UnidadesVsBarrasModel modelo = new UnidadesVsBarrasModel();
            Log.Info("Entidad Equiunidbarr - GetByIdCaiEquiunidbarr");
            modelo.Entidad = this.servicioCalculoPorcentajes.GetByIdCaiEquiunidbarr(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            Log.Info("Lista PtoMedicion - ListMePtomedicion");
            modelo.ListaPtoMedicion = this.servicioCalculoPorcentajes.ListMePtomedicion("", "-1").ToList();
            Log.Info("Lista Empresa - ListEmpresasSTR");
            modelo.ListaEmpresa = this.servicioEmpresa.ListEmpresasSTR();
            Log.Info("Lista Central Generación - ListCentralGeneracion");
            modelo.ListaEquiCen = this.servicioCentralGeneracion.ListCentralGeneracion();
            Log.Info("Lista Central Unidad - ListUnidad");
            modelo.ListaEquiUni = this.servicioCentralGeneracion.ListUnidad();
            Log.Info("Lista Barras - ListBarras");
            modelo.ListaBarras = this.servicioBarra.ListBarras();
            modelo.Caiunbfecvigencia = modelo.Entidad.Caiunbfecvigencia.ToString("dd/MM/yyyy");
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            return PartialView(modelo);
        }

        [HttpPost, ActionName("Delete")]
        public string Delete(int id)
        {
            if (id > 0)
            {
                UnidadesVsBarrasModel model = new UnidadesVsBarrasModel();
                Log.Info("Elimina registro - DeleteCaiEquiunidbarr");
                this.servicioCalculoPorcentajes.DeleteCaiEquiunidbarr(id);
                return "true";
            }
            return "False";
        }

    }
}
