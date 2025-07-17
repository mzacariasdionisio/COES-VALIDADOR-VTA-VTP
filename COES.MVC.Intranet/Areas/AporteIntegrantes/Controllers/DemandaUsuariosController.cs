using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.Servicios.Aplicacion.CalculoPorcentajes.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class DemandaUsuariosController : BaseController
    {
        // GET: /AporteIntegrantes/DemandaUsuarios/

        public DemandaUsuariosController()
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
        
        public ActionResult Index(int caiprscodi = 0, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            DemandaUsuariosModel model = new DemandaUsuariosModel();
            Log.Info("Lista Presupuesto - ListPresupuesto");
            model.ListaPresupuesto = this.servicioCalculoPorcentajes.ListCaiPresupuestos();
            if (model.ListaPresupuesto.Count > 0 && caiprscodi == 0)
            {
                caiprscodi = model.ListaPresupuesto[0].Caiprscodi;
            }
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
            //Intervalo de fechas
            string sMes = model.EntidadPresupuesto.Caiprsmesinicio.ToString();
            if (sMes.Length == 1) sMes = "0" + sMes;
            model.sPeriodoInicio = "01/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
            model.sPeriodoFinal = "01/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
            Log.Info("Lista de Versiones de ajuste - ListCaiAjustes");
            model.ListaAjuste = this.servicioCalculoPorcentajes.ListCaiAjustes(caiprscodi); //Ordenado en descendente
            if (model.ListaAjuste.Count > 0 && caiajcodi == 0)
            {
                caiajcodi = (int)model.ListaAjuste[0].Caiajcodi;
            }

            if (caiprscodi > 0 && caiajcodi > 0)
            {
                Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
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

        /// <summary>
        /// Permite copiar la energía inyectada en bornes de generación - SGOCOES
        /// </summary>
        /// <param name="caiprscodi">Código de la Versión de Presupuesto</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarDU(int caiprscodi = 0, int caiajcodi = 0)
        {
            base.ValidarSesionUsuario();
            DemandaUsuariosModel model = new DemandaUsuariosModel();
            model.sError = "";
            model.iNumReg = 0;
            Int32 iCagdcmcodi = 0;
            string sCagdcmFuenteDatos = "EU"; //Ejecutados Usuarios Libres
            string sCagdcmcalidadinfo = "C"; //migrado del sgocoes
            string sT = "E";
            try
            {
                if (caiprscodi > 0 && caiajcodi > 0)
                {
                    //Eliminando Todo el proceso de calculo
                    Log.Info("Elimina la valorización de la versión - EliminarCalculo");
                    this.servicioCalculoPorcentajes.EliminarCalculo(caiajcodi);
                    //Eliminando la información del periodo
                    Log.Info("Elimina registro - DeleteCaiGenerdeman");
                    this.servicioCalculoPorcentajes.DeleteCaiGenerdeman(caiajcodi, sCagdcmFuenteDatos);
                    //Calculamos la fecha de inicio del Presupuesto
                    Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
                    model.EntidadPresupuesto = this.servicioCalculoPorcentajes.GetByIdCaiPresupuesto(caiprscodi);
                    string sMes = model.EntidadPresupuesto.Caiprsmesinicio.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes; 
                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
                    //DateTime dFechaInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    //Calculamos la fecha final Ejecutado:
                    Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
                    model.EntidadAjuste = this.servicioCalculoPorcentajes.GetByIdCaiAjuste(caiajcodi);
                    sMes = model.EntidadAjuste.Caiajmes.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var sFechaInicioAjuste = "01/" + sMes + "/" + model.EntidadAjuste.Caiajanio;
                    //La fecha de ajuste, es el inicio para la data Proyectada
                    DateTime dFecInicioAjuste = DateTime.ParseExact(sFechaInicioAjuste, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    var sFechaFin = dFecInicioAjuste.AddDays(-1).ToString("dd/MM/yyyy"); // al quitarle un dia, le estamos colocando al final del dia del mes anterior
                    //--------------------------------------------------------------------------------------------------------------------------------
                    int iFormatcodi = Int32.Parse(ConfigurationManager.AppSettings["IdFormatoPR16"]);
                    string lectCodiPR16 = ConfigurationManager.AppSettings["IdLecturaPR16"];
                    string lectCodiAlpha = ConfigurationManager.AppSettings["IdLecturaAlphaPR16"];
                    int iTipoEmprcodi = 4;
                    Log.Info("Obtener Codigo Generado - GetCodigoGeneradoCaiGenerdeman");
                    iCagdcmcodi = this.servicioCalculoPorcentajes.GetCodigoGeneradoCaiGenerdeman();
                    //--------------------------------------------------------------------------------------------------------------------------------
                    //INSERTANDO CON INSERT AS SELECT...
                    Log.Info("Inserción masiva a través de un Insert as Select - SaveCaiGenerdemanAsSelectUsuariosLibres");
                    this.servicioCalculoPorcentajes.SaveCaiGenerdemanAsSelectUsuariosLibres(iCagdcmcodi, caiajcodi, sCagdcmFuenteDatos, sCagdcmcalidadinfo, sT, User.Identity.Name, iFormatcodi, sFechaInicio, sFechaFin, iTipoEmprcodi, lectCodiPR16, lectCodiAlpha);
                    model.sMensaje = sFechaInicio + " al " + sFechaFin;
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
