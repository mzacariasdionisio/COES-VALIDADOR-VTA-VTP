using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.CalculoPorcentajes.Helper;
using log4net;
using System.Reflection;
using COES.MVC.Intranet.Areas.Transferencias.Helper;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class MaximaDemandaController : BaseController
    {
        // GET: /AporteIntegrantes/MaximaDemanda/
        public MaximaDemandaController()
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
        CalculoPorcentajesAppServicio servicioCalculoPorcentajes = new CalculoPorcentajesAppServicio();
        Funcion servicioFuncion = new Funcion();

        public ActionResult Index(int caiprscodi = 0, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            MaximaDemandaModel model = new MaximaDemandaModel();
            Log.Info("Lista de Presupuestos - ListPresupuesto");
            model.ListaPresupuesto = this.servicioCalculoPorcentajes.ListCaiPresupuestos();
            if (model.ListaPresupuesto.Count > 0 && caiprscodi == 0)
            {
                caiprscodi = model.ListaPresupuesto[0].Caiprscodi;

            }
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
            Log.Info("Lista de Versiones de ajuste - ListVcrRecalculos");
            model.ListaAjuste = this.servicioCalculoPorcentajes.ListCaiAjustes(caiprscodi); //Ordenado en descendente
            if (model.ListaAjuste.Count > 0 && caiajcodi == 0)
            {
                caiajcodi = (int)model.ListaAjuste[0].Caiajcodi;
            }

            if (caiprscodi > 0 && caiajcodi > 0)
            {
                Log.Info("EntidadAjuste - GetByIdVcrRecalculoView");
                model.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(caiajcodi);
            }
            else
            {
                model.EntidadAjuste = new CaiAjusteDTO();
            }
            model.Caiprscodi = caiprscodi;
            model.Caiajcodi = caiajcodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Lista(int caiajcodi)
        {
            MaximaDemandaModel model = new MaximaDemandaModel();
            Log.Info("Lista MaximaDemanda - ListCaiMaxdemandas");
            model.ListaMaximaDemanda = this.servicioCalculoPorcentajes.ListCaiMaxdemandas(caiajcodi);
            foreach (CaiMaxdemandaDTO item in model.ListaMaximaDemanda)
            {
                string sAnio = item.Caimdeaniomes.ToString().Substring(0,4);
                int anioLength = item.Caimdeaniomes.ToString().Length;
                string sMes = item.Caimdeaniomes.ToString().Substring(4, anioLength - 4);
                item.NombreMes = sAnio + "." + Tools.ObtenerNombreMes(int.Parse(sMes));
                item.NombreTipo = UtilCalculoPorcentajes.ObtenerDescTipo(item.Caimdetipoinfo);
                
            }
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }

        public ActionResult View(int id = 0)
        {
            MaximaDemandaModel model = new MaximaDemandaModel();
            Log.Info("Entidad MaximaDemanda - GetByIdCaiMaxdemanda");
            model.EntidadMaximaDemanda = this.servicioCalculoPorcentajes.GetByIdCaiMaxdemanda(id);
            string sAnio = model.EntidadMaximaDemanda.Caimdeaniomes.ToString().Substring(0, 4);
            int anioLength = model.EntidadMaximaDemanda.Caimdeaniomes.ToString().Length;
            string sMes = model.EntidadMaximaDemanda.Caimdeaniomes.ToString().Substring(4, anioLength - 4);
            model.EntidadMaximaDemanda.NombreMes = sAnio + "." + Tools.ObtenerNombreMes(int.Parse(sMes));
            model.EntidadMaximaDemanda.NombreTipo = UtilCalculoPorcentajes.ObtenerDescTipo(model.EntidadMaximaDemanda.Caimdetipoinfo);

            return PartialView(model);
        }

        public ActionResult New(int caiajcodi = 0)
        {
            MaximaDemandaModel modelo = new MaximaDemandaModel();
            modelo.EntidadMaximaDemanda = new CaiMaxdemandaDTO();
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            modelo.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(caiajcodi);
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            modelo.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(modelo.EntidadAjuste.Caiprscodi);
            modelo.EntidadMaximaDemanda.Caiajcodi = caiajcodi;
            modelo.EntidadMaximaDemanda.Caimdetipoinfo = "P";
            modelo.Codfech = DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Codhor = DateTime.Now.ToString("hh:mm");
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(MaximaDemandaModel modelo)
        {
            if (ModelState.IsValid)
            {
                //if (modelo.Codmes.Length == 1) modelo.Codmes = "0" + modelo.Codmes; 
                //string anioMes = modelo.Codanio + modelo.Codmes;
                //modelo.EntidadMaximaDemanda.Caimdeaniomes = Int32.Parse(anioMes);
                var FechHora = modelo.Codfech + " " + modelo.Codhor;
                modelo.EntidadMaximaDemanda.Caimdefechor = DateTime.ParseExact(FechHora, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                int iAnio = modelo.EntidadMaximaDemanda.Caimdefechor.Value.Year;
                int iMes = modelo.EntidadMaximaDemanda.Caimdefechor.Value.Month;
                modelo.EntidadMaximaDemanda.Caimdeaniomes = iAnio * 100 + iMes;
                modelo.EntidadMaximaDemanda.Caimdeusucreacion = User.Identity.Name;
                modelo.EntidadMaximaDemanda.Caimdeusumodificacion = User.Identity.Name;
                modelo.EntidadMaximaDemanda.Caimdefeccreacion = DateTime.Now;
                modelo.EntidadMaximaDemanda.Caimdefecmodificacion = DateTime.Now;
                modelo.EntidadMaximaDemanda.Caimdetipoinfo = "P";
                if (modelo.EntidadMaximaDemanda.Caimdecodi == 0)
                {
                    Log.Info("Insertar registro - SaveCaiMaxdemanda");
                    this.servicioCalculoPorcentajes.SaveCaiMaxdemanda(modelo.EntidadMaximaDemanda);
                }
                else
                {
                    Log.Info("Actualiza registro - UpdateCaiMaxdemanda");
                    this.servicioCalculoPorcentajes.UpdateCaiMaxdemanda(modelo.EntidadMaximaDemanda);
                }

                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }

            modelo.sError = "Se ha producido un error al insertar la información";
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            modelo.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(modelo.EntidadMaximaDemanda.Caiajcodi);
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            modelo.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(modelo.EntidadAjuste.Caiprscodi);
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return PartialView(modelo);
        }

        public ActionResult Edit(int caimdecodi)
        {
            MaximaDemandaModel model = new MaximaDemandaModel();
            Log.Info("Entidad MaximaDemanda - GetByIdCaiMaxdemanda");
            model.EntidadMaximaDemanda = this.servicioCalculoPorcentajes.GetByIdCaiMaxdemanda(caimdecodi);
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            model.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(model.EntidadMaximaDemanda.Caiajcodi);
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(model.EntidadAjuste.Caiprscodi);
            model.EntidadMaximaDemanda.Caimdetipoinfo = "P";
            
            model.Codfech = model.EntidadMaximaDemanda.Caimdefechor.Value.ToString("dd/MM/yyyy");
            int fechaLength = model.EntidadMaximaDemanda.Caimdefechor.ToString().Length;
            model.Codhor = model.EntidadMaximaDemanda.Caimdefechor.ToString().Substring(11, fechaLength - 11).Substring(0, 5);
            
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            return PartialView(model);
        }

        [HttpPost, ActionName("Delete")]
        public string Delete(int id)
        {
            if (id > 0)
            {
                Log.Info("Elimina registro - DeleteCaiMaxdemanda");
                this.servicioCalculoPorcentajes.DeleteCaiMaxdemanda(id);
                return "true";
            }
            return "False";
        }

        /// <summary>
        /// Permite copiar la Fecha y Hora de la máxima demanda ejecutada - SGOCOES
        /// </summary>
        /// <param name="caiprscodi">Código de la Versión de Presupuesto</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarMD(int caiprscodi = 0, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            MaximaDemandaModel model = new MaximaDemandaModel();
            model.sError = "";
            model.iNumReg = 0;
            //string sCagdcmFuenteDatos = "EG"; //Ejecutados Generación
            try
            {
                string sUser = User.Identity.Name;
                if (caiprscodi > 0 && caiajcodi > 0)
                {
                    //Eliminando Todo el proceso de calculo
                    Log.Info("Elimina la valorización de la versión - EliminarCalculo");
                    this.servicioCalculoPorcentajes.EliminarCalculo(caiajcodi);
                    //Elimina todos los registro Ejecutados de la tabla CAI_MAXDEMANDA pertenenecientes a una versión de ajuste
                    Log.Info("Eliminando la información - DeleteCaiGenerdemandia / DeleteCaiGenerdeman");
                    this.servicioCalculoPorcentajes.DeleteCaiMaxdemandaEjecutada(caiajcodi);
                    
                    //Intervalos de fecha del Presupuesto
                    Log.Info("Entidad Periodo - GetByIdCaiPresupuesto");
                    model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
                    string sMes = model.EntidadPresupuesto.Caiprsmesinicio.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes; 
                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    //Intervalos de fecha del Ajuste
                    Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
                    model.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(caiajcodi);
                    sMes = model.EntidadAjuste.Caiajmes.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicioAjuste = "01/" + sMes + "/" + model.EntidadAjuste.Caiajanio;
                    DateTime fecInicioAjuste = DateTime.ParseExact(sFechaInicioAjuste, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    while (fecInicio < fecInicioAjuste)
                    {
                        DateTime fecFin = fecInicio.AddMonths(1); //Aumentamos un mes, esta en el dia uno del siguiente mes
                        //Retrocemos un dia para que regrese al ultimo dia del mes
                        fecFin = fecFin.AddDays(-1);
                        //Para cada mes ejecutado:        
                        Log.Info("Copia del SGOCOES a CaiMaxDemanda - CopiarSGOCOESCaiMaximaDemanda");
                        servicioCalculoPorcentajes.CopiarSGOCOESCaiMaximaDemanda(caiajcodi, fecInicio, fecFin, sUser);
                        model.iNumReg++;
                        //Avanzamos un mes
                        fecInicio = fecInicio.AddMonths(1);
                    }
                }
                else
                    model.sError = "Debe seleccionar un periodo y versión correcto";
            }
            catch (Exception e)
            {
                model.sError = e.Message;
            }
            return Json(model);
        }
    }
}
