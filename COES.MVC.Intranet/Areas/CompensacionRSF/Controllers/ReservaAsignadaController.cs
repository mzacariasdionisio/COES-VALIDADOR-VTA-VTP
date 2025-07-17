using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Eventos;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using log4net;
using System.Reflection;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Controllers
{
    public class ReservaAsignadaController : BaseController
    {
        // GET: /CompensacionRSF/ReservaAsignada/

        public ReservaAsignadaController()
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
        CompensacionRSFAppServicio servicioCompensacionRsf = new CompensacionRSFAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        BarraUrsAppServicio servicioPrGrupo = new BarraUrsAppServicio();
        RsfAppServicio servicioRsf = new RsfAppServicio();

        public ActionResult Index(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            ReservaAsignadaModel model = new ReservaAsignadaModel();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
                
            }
            Log.Info("Entidad Periodo - GetByIdPeriodo");
            model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            Log.Info("Lista de Versiones - ListVcrRecalculos");
            model.ListaRecalculo = this.servicioCompensacionRsf.ListVcrRecalculos(pericodi); //Ordenado en descendente
            if (model.ListaRecalculo.Count > 0 && vcrecacodi == 0)
            {
                vcrecacodi = (int)model.ListaRecalculo[0].Vcrecacodi; 
            }

            if (pericodi > 0 && vcrecacodi > 0)
            {
                Log.Info("EntidadRecalculo - GetByIdVcrRecalculoView");
                model.EntidadRecalculo = this.servicioCompensacionRsf.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
            }
            else
            {
                model.EntidadRecalculo = new VcrRecalculoDTO();
            }
            model.Pericodi = pericodi;
            model.Vcrecacodi = vcrecacodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model); 
        }

        /// <summary>
        /// Permite exportar a un archivo excel la lista de la Reserva Asignada
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarPM(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            string result = "-1";
            try
            {
                ReservaAsignadaModel model = new ReservaAsignadaModel();
                if (pericodi > 0)
                {
                    Log.Info("Entidad Periodo - GetByIdPeriodo");
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    string sMes = model.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    string pathFile = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString();
                    string file = ConstantesEventos.ExportacionRSFReporte;
                    Log.Info("Exportar información - GenerarReporteReservaAsignadaSEV");
                    //ASSETEC 20201204 -> Dos formas de copiar la Reserva asiganada, segun fecha de corte 2021.01
                    if (model.EntidadPeriodo.PeriAnioMes >= ConstantesCompensacionRSF.Periodo2021)
                    {
                        this.servicioCompensacionRsf.GenerarReporteReservaAsignadaSEV(fecInicio, fecFin, pathFile, file);
                    }
                    else
                    {
                        this.servicioCompensacionRsf.GenerarReporteReservaAsignadaSEV2020(fecInicio, fecFin, pathFile, file);
                    }
                    result = file;
                }
            }
            catch (Exception e)
            {
                string sMensaje = e.Message;
                result = "-1";
            }
            return Json(result);
        }

        /// <summary>
        /// Abrir el archivode de la Reserva Asignada
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Permite copiar la información de la base de datos de la Reserva Asignada
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarPM(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            ReservaAsignadaModel model = new ReservaAsignadaModel();
            model.sError = "";
            model.iNumReg = 0;
            try
            {
                if (pericodi > 0 && vcrecacodi > 0)
                {
                    //Eliminando Todo el proceso de calculo
                    Log.Info("Executar proceso - EliminarCalculo");
                    string sBorrar = this.servicioCompensacionRsf.EliminarCalculo(vcrecacodi);
                    if (!sBorrar.Equals("1"))
                    {
                        model.sError = "Lo sentimos, No se pudo eliminar el proceso de cálculo: " + sBorrar;
                        return Json(model);
                    }

                    //Eliminando la información del periodo
                    Log.Info("Eliminando la información - DeleteVcrReservasign");
                    this.servicioCompensacionRsf.DeleteVcrReservasign(vcrecacodi);

                    Log.Info("Entidad Periodo - GetByIdPeriodo");
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    string sMes = model.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    //ASSETEC 20201204 -> Dos formas de copiar la Reserva asiganada, segun fecha de corte 2021.01
                    if (model.EntidadPeriodo.PeriAnioMes >= ConstantesCompensacionRSF.Periodo2021)
                    {
                        //NUEVO: Obtenemos la informacion de la Reserva Asignada para periodos >= 2021.01
                        List<VcrEveRsfhoraDTO> list = servicioCompensacionRsf.ObtenerReservaAsignada(fecInicio, fecFin);
                        foreach (VcrEveRsfhoraDTO item in list)
                        {
                            VcrReservasignDTO dtoReservasign = new VcrReservasignDTO();
                            dtoReservasign.Vcrecacodi = vcrecacodi;
                            Log.Info("EntidadBarraurs - GetByNombrePrGrupo");
                            TrnBarraursDTO dtoBarraurs = this.servicioPrGrupo.GetByNombrePrGrupo(item.Ursnomb.Trim());
                            if (dtoBarraurs == null)
                            {
                                model.sError += "<br>El siguiente URS no existe: " + item.Ursnomb;
                                continue;
                                //return Json(model);
                            }
                            dtoReservasign.Grupocodi = dtoBarraurs.GrupoCodi;
                            dtoReservasign.Gruponomb = dtoBarraurs.GrupoNomb;
                            dtoReservasign.Vcrasgfecha = (DateTime)item.Rsfhorfecha;
                            dtoReservasign.Vcrasghorinicio = (DateTime)item.Rsfhorinicio;
                            //ASSETEC 20200430
                            DateTime dVcrasghorfinal = (DateTime)item.Rsfhorfin;
                            int dHoraF = dVcrasghorfinal.Hour;
                            if (dHoraF == 23)
                            {
                                int dMinuteF = dVcrasghorfinal.Minute;
                                if (dMinuteF == 59)
                                {
                                    int dSegundoF = 60 - dVcrasghorfinal.Second;
                                    dVcrasghorfinal = dVcrasghorfinal.AddSeconds(dSegundoF);
                                }
                            }
                            dtoReservasign.Vcrasghorfinal = dVcrasghorfinal;
                            dtoReservasign.Vcrasgreservasign = item.Rsfdetsub;
                            dtoReservasign.Vcrasgreservasignb = item.Rsfdetbaj;
                            dtoReservasign.Vcrasgtipo = "AUTOMATICO";
                            dtoReservasign.Vcrasgusucreacion = User.Identity.Name;
                            Log.Info("Insertar registro - SaveVcrReservasign");
                            this.servicioCompensacionRsf.SaveVcrReservasign(dtoReservasign);
                            model.iNumReg++;
                        }
                    }
                    else {
                        //ANTIGUO: Obtenemos la informacion de la Reserva Asignada para periodos < 2021.01
                        List<EveRsfhoraDTO> list = servicioCompensacionRsf.ObtenerReservaAsignada2020(fecInicio, fecFin);
                        foreach (EveRsfhoraDTO item in list)
                        {
                            VcrReservasignDTO dtoReservasign = new VcrReservasignDTO();
                            dtoReservasign.Vcrecacodi = vcrecacodi;
                            Log.Info("EntidadBarraurs - GetByNombrePrGrupo");
                            TrnBarraursDTO dtoBarraurs = this.servicioPrGrupo.GetByNombrePrGrupo(item.Ursnomb.Trim());
                            if (dtoBarraurs == null)
                            {
                                model.sError += "<br>El siguiente URS no existe: " + item.Ursnomb;
                                continue;
                                //return Json(model);
                            }
                            dtoReservasign.Grupocodi = dtoBarraurs.GrupoCodi;
                            dtoReservasign.Gruponomb = dtoBarraurs.GrupoNomb;
                            dtoReservasign.Vcrasgfecha = (DateTime)item.Rsfhorfecha;
                            dtoReservasign.Vcrasghorinicio = (DateTime)item.Rsfhorinicio;
                            //ASSETEC 20200430
                            DateTime dVcrasghorfinal = (DateTime)item.Rsfhorfin;
                            int dHoraF = dVcrasghorfinal.Hour;
                            if (dHoraF == 23)
                            {
                                int dMinuteF = dVcrasghorfinal.Minute;
                                if (dMinuteF == 59)
                                {
                                    int dSegundoF = 60 - dVcrasghorfinal.Second;
                                    dVcrasghorfinal = dVcrasghorfinal.AddSeconds(dSegundoF);
                                }
                            }
                            dtoReservasign.Vcrasghorfinal = dVcrasghorfinal;
                            dtoReservasign.Vcrasgreservasign = item.Valorautomatico;
                            dtoReservasign.Vcrasgtipo = "AUTOMATICO";
                            dtoReservasign.Vcrasgusucreacion = User.Identity.Name;
                            Log.Info("Insertar registro - SaveVcrReservasign");
                            this.servicioCompensacionRsf.SaveVcrReservasign(dtoReservasign);
                            model.iNumReg++;
                        }
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
