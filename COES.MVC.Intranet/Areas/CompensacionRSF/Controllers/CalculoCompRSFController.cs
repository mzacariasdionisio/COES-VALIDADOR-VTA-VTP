using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Controllers
{
    public class CalculoCompRSFController : BaseController
    {
        // GET: /CompensacionRSF/CalculoCompRSF/

        public CalculoCompRSFController()
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

        public ActionResult Index(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();
            CalculoCompRSFModel model = new CalculoCompRSFModel();
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
            model.PeriAnioMes = model.EntidadPeriodo.PeriAnioMes;
            model.Vcrecacodi = vcrecacodi;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Permite procesar el Cálculo de Compensaciones por RSF del periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de calculo</param>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarCalculo(int pericodi = 0, int vcrecacodi = 0)
        { 
            base.ValidarSesionUsuario();
            string sResultado = "1";
            if (pericodi == 0 || vcrecacodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes y una versión de cálculo correcto";
                return Json(sResultado);
            }
            string suser = User.Identity.Name;
            PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            //ASSETEC 20201204 -> Dos formas de calcular el RSF, segun fecha de corte 2021.01
            if (EntidadPeriodo.PeriAnioMes >= ConstantesCompensacionRSF.PeriodoFebrero2021)
            {   //NUEVO -> 2021.01 hacia adelante
                Log.Info("Executar calculo - ProcesarCalculo");
                sResultado = this.servicioCompensacionRsf.ProcesarCalculoDesdeFebrero2021(pericodi, vcrecacodi, suser);
                Log.Info("Ejecutado ProcesarCalculo " + EntidadPeriodo.PeriAnioMes + " | Fecha : " + DateTime.Now + " | Usuario: " + suser);
            }
            else if (EntidadPeriodo.PeriAnioMes == ConstantesCompensacionRSF.Periodo2021)
            {   //NUEVO -> 2021.01 hacia adelante
                Log.Info("Executar calculo - ProcesarCalculo");
                sResultado = this.servicioCompensacionRsf.ProcesarCalculo(pericodi, vcrecacodi, suser);
                Log.Info("Ejecutado ProcesarCalculo " + EntidadPeriodo.PeriAnioMes + " | Fecha : " + DateTime.Now + " | Usuario: " + suser);
            }
            else
            {   //ANTIGUO -> 2020.12 hacia atras
                Log.Info("Executar calculo - ProcesarCalculo");
                sResultado = this.servicioCompensacionRsf.ProcesarCalculo2020(pericodi, vcrecacodi, suser);
                Log.Info("Ejecutado ProcesarCalculo " + EntidadPeriodo.PeriAnioMes + " | Fecha : " + DateTime.Now + " | Usuario: " + suser);
            }
               

            return Json(sResultado);
        }

        /// <summary>
        /// Permite borrar el >Cálculo de Compensaciones por RSF del periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de calculo</param>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BorrarCalculo(int pericodi = 0, int vcrecacodi = 0)
        {
            base.ValidarSesionUsuario();

            string sResultado = "1";
            if (pericodi == 0 || vcrecacodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes y una versión de cálculo correcto";
                return Json(sResultado);
            }
            try
            {
                //Eliminamos la información procesada en el periodo / versión
                Log.Info("Executar proceso - EliminarCalculo");
                string sBorrar = this.servicioCompensacionRsf.EliminarCalculo(vcrecacodi);
                Log.Info("Ejecutado EliminarCalculo " + pericodi + " | Fecha : " + DateTime.Now + " | Usuario: " + User.Identity.Name);
                if (!sBorrar.Equals("1"))
                {
                    sResultado = "Lo sentimos, No se pudo eliminar el proceso de cálculo: " + sBorrar;
                    return Json(sResultado);
                }
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);
        }

        /// <summary>
        /// Permite exportar a un archivo excel los resultados del mes de calculo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de calculo</param>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int vcrecacodi = 0, string reporte = "", int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString();
                string file = "-1";
                if (reporte.Equals("Superavit") && EntidadPeriodo.PeriAnioMes < ConstantesCompensacionRSF.Periodo2021)
                {
                    //Superávit – CU10.01
                    Log.Info("Exportar información - GenerarReporteSuperavit");
                    file = this.servicioCompensacionRsf.GenerarReporteSuperavit(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("ReservaNoSuministrada"))
                {
                    //Reserva No Suministrada – CU10.02
                    Log.Info("Exportar información - GenerarReporteReservaNoSuministrada");
                    file = this.servicioCompensacionRsf.GenerarReporteReservaNoSuministrada(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("ReservaAsignada"))
                {
                    //Reserva Asignada – CU10.03
                    //ASSETEC 20201204 -> Dos formas de calcular el RSF, segun fecha de corte 2021.01
                    if (EntidadPeriodo.PeriAnioMes >= ConstantesCompensacionRSF.Periodo2021)
                    {   //NUEVO -> 2021.01 hacia adelante
                        Log.Info("Exportar información - GenerarReporteReservaAsignada");
                        file = this.servicioCompensacionRsf.GenerarReporteReservaAsignada(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                    }
                    else
                    {   //ANTIGUO -> 2020.12 hacia atras
                        Log.Info("Exportar información - GenerarReporteReservaAsignada2020");
                        file = this.servicioCompensacionRsf.GenerarReporteReservaAsignada2020(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                    }

                }
                else if (reporte.Equals("CostoOportunidad"))
                {
                    //Costo de oportunidad – CU10.04
                    Log.Info("Exportar información - GenerarReporteCostoOportunidad");
                    file = this.servicioCompensacionRsf.GenerarReporteCostoOportunidad(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("Compensacion"))
                {
                    //Compensación – CU10.05
                    Log.Info("Exportar información - GenerarReporteCompensacion");
                    file = this.servicioCompensacionRsf.GenerarReporteCompensacion(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("AsignacionPagoDiario"))
                {
                    //Asignación de pago diario – CU10.06
                    Log.Info("Exportar información - GenerarReporteAsignacionPagoDiario");
                    file = this.servicioCompensacionRsf.GenerarReporteAsignacionPagoDiario(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("CostoServicioRSFDiario"))
                {
                    //Costo del servicio de RSF diario – CU10.07
                    Log.Info("Exportar información - GenerarReporteCostoServicioRSFDiario");
                    file = this.servicioCompensacionRsf.GenerarReporteCostoServicioRSFDiario(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("CuadroPR21"))
                {
                    //Cuadro PR-21 – CU10.08
                    Log.Info("Exportar información - GenerarReporteCuadroPR21");
                    file = this.servicioCompensacionRsf.GenerarReporteCuadroPR21(pericodi, vcrecacodi, formato, pathFile, pathLogo);

                }
                else if (reporte.Equals("ReporteResumen"))
                {
                    //Reporte Resumen – CU10.09
                    Log.Info("Exportar información - GenerarReporteResumen");
                    file = this.servicioCompensacionRsf.GenerarReporteResumen(pericodi, vcrecacodi, formato, pathFile, pathLogo);
                }

                return Json(file);
            }
            catch (Exception e)
            {
                string msj = e.Message;
                return Json(-1);
            }
        }


        /// <summary>
        /// Abrir el archivo
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesCompensacionRSF.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }
    }
}
