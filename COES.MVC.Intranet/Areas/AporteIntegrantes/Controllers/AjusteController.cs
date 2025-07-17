using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.CalculoPorcentajes;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Helper;
using log4net;
using System.Reflection;
using System.Globalization;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Base.Core;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Controllers
{
    public class AjusteController : BaseController
    {
        // GET: /AporteIntegrantes/Ajuste/
        public AjusteController()
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
        CalculoPorcentajesAppServicio servicioCalculoPorcentaje = new CalculoPorcentajesAppServicio();
        ConsultaMedidoresAppServicio servicioConsultaMedidores = new ConsultaMedidoresAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        Funcion servicioFuncion = new Funcion();

        public ActionResult Index(int caiprscodi = 0)
        {
            base.ValidarSesionUsuario();
            BaseModel model = new BaseModel();
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentaje.GetByIdCaiPresupuesto(caiprscodi);
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int caiprscodi)
        {
            BaseModel model = new BaseModel();
            Log.Info("Lista Ajuste - ListByCaiPrscodi");
            model.ListaAjuste = this.servicioCalculoPorcentaje.ListByCaiPrscodi(caiprscodi);
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar,base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar,base.UserName);
            return PartialView(model);
        }

        public ActionResult New(int caiprscodi) 
        {
            BaseModel model = new BaseModel();
            model.EntidadAjuste = new CaiAjusteDTO();
            
            if (model.EntidadAjuste == null) 
            {
                return HttpNotFound();
            }
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentaje.GetByIdCaiPresupuesto(caiprscodi);
            model.EntidadAjuste.Caiajcodi = 0;
            model.EntidadAjuste.Caiprscodi = caiprscodi;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            TempData["Mescodigo"] = new SelectList(this.servicioFuncion.ObtenerMes(), "value", "text", model.EntidadAjuste.Caiajmes);
            TempData["Aniocodigo"] = new SelectList(this.servicioFuncion.ObtenerAnio(), "value", "text", model.EntidadAjuste.Caiajanio);
            return PartialView(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BaseModel model) 
        {
            CaiAjusteDTO dto = new CaiAjusteDTO();
            dto.Caiprscodi = model.EntidadAjuste.Caiprscodi;
            dto.Caiajnombre = model.EntidadAjuste.Caiajnombre;
            dto.Caiajanio = model.EntidadAjuste.Caiajanio;
            dto.Caiajmes = model.EntidadAjuste.Caiajmes;
            dto.Caiajusumodificacion = User.Identity.Name.ToString();
            dto.Caiajfecmodificacion = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (model.EntidadAjuste.Caiajcodi == 0)
                {
                    //Registra
                    dto.Caiajusucreacion = User.Identity.Name.ToString();
                    dto.Caiajfeccreacion = DateTime.Now;
                    Log.Info("Inserta registro - SaveCaiAjuste");
                    dto.Caiajcodi = this.servicioCalculoPorcentaje.SaveCaiAjuste(dto);
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //IMPORTAMOS LA INFORMACIÓN COMPLEMENTARIA DEL PERIODO
                    //Calculamos la fecha de inicio del Presupuesto
                    Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
                    model.EntidadPresupuesto = this.servicioCalculoPorcentaje.GetByIdCaiPresupuesto(dto.Caiprscodi);
                    string sMes = model.EntidadPresupuesto.Caiprsmesinicio.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    string sFechaInicio = "01/" + sMes + "/" + model.EntidadPresupuesto.Caiprsanio;
                    DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    //Calculamos la fecha final Ejecutado:
                    sMes = dto.Caiajmes.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    string sFechaInicioAjuste = "01/" + sMes + "/" + dto.Caiajanio;
                    //La fecha de ajuste, es el inicio para la data Proyectada
                    DateTime dFecInicioAjuste = DateTime.ParseExact(sFechaInicioAjuste, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    DateTime dFechaAux = dFecInicio;
                    while (dFechaAux < dFecInicioAjuste)
                    {
                        //Costo Marginal: TRN_COSTO_MARGINAL
                        //Traemos el Periodo y Ultimo Recalculo Vigente:
                        int iAnioMes = dFechaAux.Year * 100 + dFechaAux.Month;
                        PeriodoDTO dtoPeriodo = this.servicioPeriodo.GetByAnioMes(iAnioMes);
                        int iRecacodi = this.servicioRecalculo.GetUltimaVersion(dtoPeriodo.PeriCodi);
                        CaiAjustecmarginalDTO dtoAjusteCMargial = new CaiAjustecmarginalDTO();
                        dtoAjusteCMargial.Pericodi = dtoPeriodo.PeriCodi;
                        dtoAjusteCMargial.Recacodi = iRecacodi;
                        dtoAjusteCMargial.Caiajcodi = dto.Caiajcodi;
                        dtoAjusteCMargial.Caajcmmes = iAnioMes;
                        dtoAjusteCMargial.Caajcmusucreacion = User.Identity.Name.ToString();
                        Log.Info("Insertamos en CAI_AJUSTECMARGINAL - SaveCaiAjustecmarginal");
                        this.servicioCalculoPorcentaje.SaveCaiAjustecmarginal(dtoAjusteCMargial);
                        //Avanzamos un mes
                        dFechaAux = dFechaAux.AddMonths(1);
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Copiamos lainfrmación de la Maxima Demanda
                    dFechaAux = dFecInicio;
                    while (dFechaAux < dFecInicioAjuste)
                    {
                        DateTime fecFin = dFechaAux.AddMonths(1); //Aumentamos un mes, esta en el dia uno del siguiente mes
                        //Retrocemos un dia para que regrese al ultimo dia del mes
                        fecFin = fecFin.AddDays(-1);
                        //Para cada mes ejecutado:        
                        Log.Info("Copia del SGOCOES a CaiMaxDemanda - CopiarSGOCOESCaiMaximaDemanda");
                        this.servicioCalculoPorcentaje.CopiarSGOCOESCaiMaximaDemanda(dto.Caiajcodi, dFechaAux, fecFin, User.Identity.Name.ToString());
                        model.iNumReg++;
                        //Avanzamos un mes
                        dFechaAux = dFechaAux.AddMonths(1);
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    string sFechaIni = dFecInicio.ToString(ConstantesBase.FormatoFecha);
                    string sFechaFin = dFecInicioAjuste.AddDays(-1).ToString(ConstantesBase.FormatoFecha);
                    //Generadores
                    List<CaiAjusteempresaDTO> listEmprXPtoGen = this.servicioCalculoPorcentaje.ListCaiAjusteempresasXPtoGeneracion(sFechaIni, sFechaFin);
                    foreach (CaiAjusteempresaDTO dtoEmprPto in listEmprXPtoGen)
                    {
                        //Insentarmos los registros:
                        dtoEmprPto.Caiajcodi = dto.Caiajcodi;
                        dtoEmprPto.Caiajetipoinfo = "E";
                        dtoEmprPto.Caiajereteneejeini = null;
                        dtoEmprPto.Caiajereteneejefin = null;
                        dtoEmprPto.Caiajeretenepryaini = null;
                        dtoEmprPto.Caiajeretenepryafin = null;
                        dtoEmprPto.Caiajereteneprybini = null;
                        dtoEmprPto.Caiajereteneprybfin = null;
                        dtoEmprPto.Caiajeusucreacion = User.Identity.Name.ToString();
                        dtoEmprPto.Caiajefeccreacion = DateTime.Now;
                        this.servicioCalculoPorcentaje.SaveCaiAjusteempresa(dtoEmprPto);
                    }
                    //Usuarios Libres
                    List<CaiAjusteempresaDTO> listEmprXPtoUL = this.servicioCalculoPorcentaje.ListCaiAjusteempresasXPtoUL(sFechaIni, sFechaFin);
                    foreach (CaiAjusteempresaDTO dtoEmprPto in listEmprXPtoUL)
                    {
                        //Insentarmos los registros:
                        dtoEmprPto.Caiajcodi = dto.Caiajcodi;
                        dtoEmprPto.Caiajetipoinfo = "E";
                        dtoEmprPto.Caiajereteneejeini = null;
                        dtoEmprPto.Caiajereteneejefin = null;
                        dtoEmprPto.Caiajeretenepryaini = null;
                        dtoEmprPto.Caiajeretenepryafin = null;
                        dtoEmprPto.Caiajereteneprybini = null;
                        dtoEmprPto.Caiajereteneprybfin = null;
                        dtoEmprPto.Caiajeusucreacion = User.Identity.Name.ToString();
                        dtoEmprPto.Caiajefeccreacion = DateTime.Now;
                        this.servicioCalculoPorcentaje.SaveCaiAjusteempresa(dtoEmprPto);
                    }
                    //Distribuidores: formatcodi = 87, 88 : CAI-INF EJECUTADA/PROYECTADA DISTRIBUIDORES
                    List<CaiAjusteempresaDTO> listEmprXPtoDist = this.servicioCalculoPorcentaje.ListCaiAjusteempresasXPtoDist();
                    foreach (CaiAjusteempresaDTO dtoEmprPto in listEmprXPtoDist)
                    {
                        //Insentarmos los registros:
                        dtoEmprPto.Caiajcodi = dto.Caiajcodi;
                        dtoEmprPto.Caiajetipoinfo = "E";
                        dtoEmprPto.Caiajereteneejeini = null;
                        dtoEmprPto.Caiajereteneejefin = null;
                        dtoEmprPto.Caiajeretenepryaini = null;
                        dtoEmprPto.Caiajeretenepryafin = null;
                        dtoEmprPto.Caiajereteneprybini = null;
                        dtoEmprPto.Caiajereteneprybfin = null;
                        dtoEmprPto.Caiajeusucreacion = User.Identity.Name.ToString();
                        dtoEmprPto.Caiajefeccreacion = DateTime.Now;
                        this.servicioCalculoPorcentaje.SaveCaiAjusteempresa(dtoEmprPto);
                    }
                    //Transmisores: formatcodi = 90, 91 : CAI-INF. EJECUTADA/PROYECTADA TRANSMISORES
                    List<CaiAjusteempresaDTO> listEmprXPtoTrans = this.servicioCalculoPorcentaje.ListCaiAjusteempresasXPtoTrans();
                    foreach (CaiAjusteempresaDTO dtoEmprPto in listEmprXPtoTrans)
                    {
                        //Insentarmos los registros:
                        dtoEmprPto.Caiajcodi = dto.Caiajcodi;
                        dtoEmprPto.Caiajetipoinfo = "T";
                        dtoEmprPto.Caiajereteneejeini = null;
                        dtoEmprPto.Caiajereteneejefin = null;
                        dtoEmprPto.Caiajeretenepryaini = null;
                        dtoEmprPto.Caiajeretenepryafin = null;
                        dtoEmprPto.Caiajereteneprybini = null;
                        dtoEmprPto.Caiajereteneprybfin = null;
                        dtoEmprPto.Caiajeusucreacion = User.Identity.Name.ToString();
                        dtoEmprPto.Caiajefeccreacion = DateTime.Now;
                        this.servicioCalculoPorcentaje.SaveCaiAjusteempresa(dtoEmprPto);
                    }
                }
                else 
                { 
                    //Actualiza
                    dto.Caiajcodi = model.EntidadAjuste.Caiajcodi;
                    Log.Info("Actualiza registro - UpdateCaiAjuste");
                    this.servicioCalculoPorcentaje.UpdateCaiAjuste(dto);

                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index", new { caiprscodi = model.EntidadAjuste.Caiprscodi });
            }
            //error
            model.sError = "Se ha producido un error al insertar la información";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            TempData["Mescodigo"] = new SelectList(this.servicioFuncion.ObtenerMes(), "Value", "Text", model.EntidadAjuste.Caiajmes);
            TempData["Aniocodigo"] = new SelectList(this.servicioFuncion.ObtenerAnio(), "Value", "Text", model.EntidadAjuste.Caiajanio);
            return PartialView(model); 
        }

        public ActionResult Edit(int caiajcodi)
        {

            BaseModel model = new BaseModel();
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            model.EntidadAjuste = this.servicioCalculoPorcentaje.GetByIdCaiAjuste(caiajcodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            TempData["Mescodigo"] = new SelectList(this.servicioFuncion.ObtenerMes(), "Value", "Text", model.EntidadAjuste.Caiajmes);
            TempData["Aniocodigo"] = new SelectList(this.servicioFuncion.ObtenerAnio(), "Value", "Text", model.EntidadAjuste.Caiajanio);
            Log.Info("Entidad Presupuesto - GetByIdCaiPresupuesto");
            model.EntidadPresupuesto = this.servicioCalculoPorcentaje.GetByIdCaiPresupuesto(model.EntidadAjuste.Caiprscodi);
            return PartialView(model);
        }

        [HttpPost]
        public string Delete(int id)
        {
            if (id > 0) 
            {
                BaseModel model = new BaseModel();
                Log.Info("Elimina registro - DeleteCaiAjustecmarginal");
                this.servicioCalculoPorcentaje.DeleteCaiAjustecmarginal(id);





                Log.Info("Elimina registro - DeleteCaiAjuste");
                this.servicioCalculoPorcentaje.DeleteCaiAjuste(id);
                return "true";
            }
            return "false";
        }

        public ActionResult View(int id = 0)
        {
            BaseModel modelo = new BaseModel();
            Log.Info("Entidad Ajuste - GetByIdCaiAjuste");
            modelo.EntidadAjuste = this.servicioCalculoPorcentaje.GetByIdCaiAjuste(id);
            return PartialView(modelo);
        }
    }
}
