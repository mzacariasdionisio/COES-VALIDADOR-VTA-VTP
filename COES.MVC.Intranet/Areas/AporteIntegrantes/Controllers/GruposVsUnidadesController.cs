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
    public class GruposVsUnidadesController : BaseController
    {
        public GruposVsUnidadesController()
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
        CentralGeneracionAppServicio servicioCentralGeneracion = new CentralGeneracionAppServicio();

        // GET: /AporteIntegrantes/CostoMarginalVsBarras/

        public ActionResult Index(){
            GruposVsUnidadesModel model = new GruposVsUnidadesModel();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista() {
            GruposVsUnidadesModel model = new GruposVsUnidadesModel();
            Log.Info("Lista Equisddpuni - ListCaiEquisddpuni2");
            //cree otra sentencia sql para traer tambien los nombres de los fk
            model.ListaEquisddpuni = this.servicioCalculoPorcentaje.ListCaiEquisddpuni2();
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }

        public ActionResult New()
        {
            GruposVsUnidadesModel model = new GruposVsUnidadesModel();
            model.Entidad = new CaiEquisddpuniDTO();
            if(model.Entidad==null)
            {
                return HttpNotFound();
            }

            model.Entidad.Casdducodi = 0;
            model.Casddufecvigencia = DateTime.Now.ToString("dd/MM/yyyy");
            Log.Info("Lista EquiUni - ListUnidad");
            model.ListaEquiUni = this.servicioCalculoPorcentaje.ListUnidad();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(GruposVsUnidadesModel model)
        {

            if (ModelState.IsValid)
            {
                CaiEquisddpuniDTO dto = new CaiEquisddpuniDTO();
                dto.Equicodi = model.Entidad.Equicodi;
                dto.Casdduunidad = model.Entidad.Casdduunidad;
                dto.Casdduusumodificacion = User.Identity.Name;
                dto.Casddufecmodificacion = DateTime.Now;
                if (model.Casddufecvigencia != "" && model.Casddufecvigencia != null)
                    dto.Casddufecvigencia = DateTime.ParseExact(model.Casddufecvigencia, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                if (model.Entidad.Casdducodi == 0)
                {
                    dto.Casdduusucreacion = User.Identity.Name;
                    dto.Casddufeccreacion = DateTime.Now;
                    Log.Info("Inserta registro - SaveCaiEquisddpuni");
                    this.servicioCalculoPorcentaje.SaveCaiEquisddpuni(dto);
                }
                else
                {
                    dto.Casdducodi = model.Entidad.Casdducodi;
                    Log.Info("Actualiza registro - UpdateCaiEquisddpuni");
                    this.servicioCalculoPorcentaje.UpdateCaiEquisddpuni(dto);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //error
            model.sError = "Se ha producido un error al insertar la información";
            model.Casddufecvigencia = DateTime.Now.ToString("dd/MM/yyyy");
            Log.Info("Lista EquiUni - ListUnidad");
            model.ListaEquiUni = this.servicioCentralGeneracion.ListUnidad();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        public ActionResult Edit(int casdducodi)
        {

            GruposVsUnidadesModel model = new GruposVsUnidadesModel();
            Log.Info("Entidad CaiEquisddpuni - GetByIdCaiEquisddpuni");
            model.Entidad = this.servicioCalculoPorcentaje.GetByIdCaiEquisddpuni(casdducodi);

            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            Log.Info("Lista EquiUni - ListUnidad");
            model.ListaEquiUni = this.servicioCentralGeneracion.ListUnidad();
            model.Casddufecvigencia = model.Entidad.Casddufecvigencia.Value.ToString("dd/MM/yyyy");
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Delete(int casdducodi = 0)
        {
            string salida = "";
            try
            {
                Log.Info("Elimina Registro - DeleteCaiEquisddpuni");
                this.servicioCalculoPorcentaje.DeleteCaiEquisddpuni(casdducodi);
                salida = "true";
            }
            catch
            {
                salida = "false";
            }
            return salida;
        }

        public ActionResult View(int casdducodi = 0)
        {
            GruposVsUnidadesModel modelo = new GruposVsUnidadesModel();
            Log.Info("Entidad CaiEquisddpbarr - GetByIdCaiEquisddpuni2");
            modelo.Entidad = this.servicioCalculoPorcentaje.GetByIdCaiEquisddpuni2(casdducodi);
            return PartialView(modelo);
        }

    }
}
