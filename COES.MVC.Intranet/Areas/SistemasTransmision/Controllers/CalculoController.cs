using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.SistemasTransmision.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.SistemasTransmision;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Controllers
{
    public class CalculoController : BaseController
    {
        // GET: /SistemasTransmision/ValorTransferencia/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        IngresoRetiroSCAppServicio servicioFactorProporcion = new IngresoRetiroSCAppServicio();
        IngresoPotenciaAppServicio servicioIngresoPotencia = new IngresoPotenciaAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <param name="stpercodi">Código del Mes de cálculo</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        public ActionResult Index(int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();

            CalculoModel model = new CalculoModel();
            model.ListaPeriodos = this.servicioSistemasTransmision.ListStPeriodos();
            if (model.ListaPeriodos.Count > 0 && stpercodi == 0)
            { stpercodi = model.ListaPeriodos[0].Stpercodi; }
            if (stpercodi > 0)
            {
                model.ListaRecalculo = this.servicioSistemasTransmision.ListStRecalculos(stpercodi); //Ordenado en descendente
                if (model.ListaRecalculo.Count > 0 && strecacodi == 0)
                { strecacodi = (int)model.ListaRecalculo[0].Strecacodi; }
            }

            if (stpercodi > 0 && strecacodi > 0)
            {
                model.EntidadRecalculo = this.servicioSistemasTransmision.GetByIdStRecalculo(strecacodi);
            }
            else
            {
                model.EntidadRecalculo = new StRecalculoDTO();
            }
            model.Stpercodi = stpercodi;
            model.Strecacodi = strecacodi;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Permite procesar la cálculo de Asignación de responsabilidad de pagos de los SST y SCT del periodo y versión de recalculo
        /// </summary>
        /// <param name="stpercodi">Código del Mes de cálculo</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarCalculo(int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();

            string sResultado = "1";
            if (stpercodi == 0 || strecacodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de cálculo y una versión de recalculo";
                return Json(sResultado);
            }
            string suser = User.Identity.Name;
            sResultado = this.servicioSistemasTransmision.ProcesarCalculo(stpercodi, strecacodi, suser);

            return Json(sResultado);

        }

        /// <summary>
        /// Permite borrar la cálculo de Asignación de responsabilidad de pagos de los SST y SCT del periodo y versión de recalculo
        /// </summary>
        /// <param name="stpercodi">Código del Mes de cálculo</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BorrarCalculo(int stpercodi = 0, int strecacodi = 0)
        {
            base.ValidarSesionUsuario();

            string sResultado = "1";
            if (stpercodi == 0 || strecacodi == 0)
            {
                sResultado = "Lo sentimos, debe seleccionar un mes de cálculo y una versión de recalculo";
                return Json(sResultado);
            }
            try
            {
                //Eliminamos la información procesada en el periodo / versión
                sResultado = "1";
                this.servicioSistemasTransmision.DeleteStDistelectricaGenele(strecacodi);
                this.servicioSistemasTransmision.DeleteStElementoCompensado(strecacodi);
                this.servicioSistemasTransmision.DeleteStFactorpago(strecacodi);
                this.servicioSistemasTransmision.DeleteStPagoasignado(strecacodi);
                return Json(sResultado);

            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);
        }

        /// <summary>
        /// Permite exportar a un archivo excel los resultados del mes de cálculo y versión de recalculo
        /// </summary>
        /// <param name="stpercodi">Código del Mes de cálculo</param>
        /// <param name="strecacodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int stpercodi = 0, int strecacodi = 0, string reporte = "", int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                string file = "-1";

                if (reporte.Equals("Reporte301Excel"))
                {
                    //CU06	Reportes 301 – GWh/OHMIOS Mensuales de Generadores Relevantes 
                    file = this.servicioSistemasTransmision.GenerarFormatoReporte301(strecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("Reporte302Excel"))
                {
                    //CU07	Reportes 302 – Cálculo del Factor de Participación Mensual o Anual 
                    file = this.servicioSistemasTransmision.GenerarFormatoReporte302(strecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("Reporte303Excel"))
                {
                    //CU08	Reportes 303 – Compensación Mensual 
                    file = this.servicioSistemasTransmision.GenerarFormatoReporte303(strecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("ReporteDistElecExcel"))
                {
                    //CU09 lo seReportes Distancias Eléctricas 
                    file = this.servicioSistemasTransmision.GenerarFormatoReporteDistElec(strecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("FactorParticipacion"))
                {
                    //CU10	Reportes Factor de Participación
                    file = this.servicioSistemasTransmision.GenerarFormatoFactorParticipacion(strecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("FactorParticipacionRecalculado"))
                {
                    //CU11 Reportes Factor de Participación Recalculado 
                    file = this.servicioSistemasTransmision.GenerarFormatoFactorParticipacionRecalculado(strecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("CompensacionMensual"))
                {
                    //CU12 Reportes Compensación Mensual Asignada 
                    file = this.servicioSistemasTransmision.GenerarReporteCompensacionMensual(strecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("CompensacionMensualFiltrada"))
                {
                    //CU13 Reportes Compensación Mensual Filtrada CU13
                    file = this.servicioSistemasTransmision.GenerarReporteCompensacionMensualFiltrada(strecacodi, formato, pathFile, pathLogo);
                }
                else if (reporte.Equals("ResponsabilidadPago"))
                {
                    //CU14 Reportes Asignación de Responsabilidad de Pago de Sistemas Secundarios de Transmisión y Sistemas Complementarios de Transmisión por Parte de los Generadores por el Criterio de Uso
                    file = this.servicioSistemasTransmision.GenerarReporteResponsabilidadPago(strecacodi, formato, pathFile, pathLogo);
                }
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Abrir el archivo
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }
    }
}
