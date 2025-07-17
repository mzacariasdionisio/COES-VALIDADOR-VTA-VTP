using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.TransfPotencia.Helper;
using COES.MVC.Extranet.Areas.TransfPotencia.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
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

namespace COES.MVC.Extranet.Areas.TransfPotencia.Controllers
{
    public class ValorTransfPCController : BaseController
    {
        // GET: /TransfPotencia/ValorTransferencia/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        IngresoRetiroSCAppServicio servicioFactorProporcion = new IngresoRetiroSCAppServicio();
        IngresoPotenciaAppServicio servicioIngresoPotencia = new IngresoPotenciaAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult Index(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();

            ValorTransfPCModel model = new ValorTransfPCModel();
            model.ListaRecalculoPotencia = new List<VtpRecalculoPotenciaDTO>();
            model.ListaPeriodos = new List<PeriodoDTO>();

            List<PeriodoDTO> listPeriodo = this.servicioPeriodo.ListPeriodoPotencia();

            if (listPeriodo.Count > 0 && pericodi == 0)
            {
                pericodi = listPeriodo[0].PeriCodi;                
            }

            List<VtpRecalculoPotenciaDTO> listRecalculo = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi).
                   Where(x => x.Recpotestado.Trim() != "Abierto").ToList();

            if (listRecalculo.Count() > 0 && recpotcodi == 0)
            {
                recpotcodi = listRecalculo[0].Recpotcodi;
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
                while (model.EntidadRecalculoPotencia == null && recpotcodi > 0)
                {
                    recpotcodi--;
                    model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
                }
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            }
            model.ListaPeriodos = listPeriodo;
            model.ListaRecalculoPotencia = listRecalculo;
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;


            //if (listPeriodo.Count > 0 && pericodi == 0)
            //{
            //    pericodi = listPeriodo[0].PeriCodi;                
            //}

            //if (pericodi > 0)
            //{
            //    List<VtpRecalculoPotenciaDTO> listRecalculo = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi).
            //        Where(x => x.Recpotestado.Trim() != "Abierto").ToList();

            //    if (listRecalculo.Count() == 0)
            //    {
            //        listPeriodo = listPeriodo.Where(x => x.PeriCodi != pericodi).ToList();
            //    }

            //    if (listRecalculo.Count() > 0)
            //    {
            //        recpotcodi = listRecalculo[0].Recpotcodi;
            //        model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            //        while (model.EntidadRecalculoPotencia == null && recpotcodi > 0)
            //        {
            //            recpotcodi--;
            //            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            //        }
            //    }
            //    else
            //    {
            //        model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            //    }

            //    model.ListaRecalculoPotencia = listRecalculo;
            //    model.ListaPeriodos = listPeriodo;
            //}
            //else
            //{
            //    model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            //}

            //model.Pericodi = pericodi;
            //model.Recpotcodi = recpotcodi;

            //model.ListaPeriodos = 

            //if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            //{ pericodi = model.ListaPeriodos[0].PeriCodi; }
            //if (pericodi > 0)
            //{
            //    model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
            //    if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
            //    { recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            //}

            //if (pericodi > 0 && recpotcodi > 0)
            //{
            //    model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            //    while (model.EntidadRecalculoPotencia == null && recpotcodi > 0)
            //    {
            //        recpotcodi--;
            //        model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            //    }
            //}
            //else
            //{
            //    model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            //}
            //model.Pericodi = pericodi;
            //model.Recpotcodi = recpotcodi;

            return View(model);
        }

        /// <summary>
        /// Permite exportar a un archivo excel/pdf los resultados del mes de valorización y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int recpotcodi = 0, string reporte = "", int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                ExcelWorksheet hoja = null;
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = "-1";
                if (reporte.Equals("PeajeEgreso"))
                {
                    //Información ingresada para VTP y peajes
                    file = this.servicioTransfPotencia.GenerarFormatoPeajeEgresoMinfo(pericodi, recpotcodi, 0, formato, pathFile, pathLogo, 0, 0, 0, "", "", "", out hoja);
                }
                else if (reporte.Equals("RetirosSC"))
                {
                    //CU17 Retiros de potencia sin contrato
                    file = this.servicioTransfPotencia.GenerarReporteRetirosSC(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("PeajePagarse"))
                {
                    //CU18 Compensación a transmisoras por peaje de conexión y transmisión
                    file = this.servicioTransfPotencia.GenerarReportePeajePagarse(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);

                }
                else if (reporte.Equals("IngresoTarifario"))
                {
                    //CU19 Compensación a transmisoras por ingreso tarifario
                    file = this.servicioTransfPotencia.GenerarReporteIngresoTarifario(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("PeajeRecaudado"))
                {
                    //CU20 Compensaciones incluidas en el peaje por conexión
                    file = this.servicioTransfPotencia.GenerarReportePeajeRecaudado(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("PotenciaValor"))
                {
                    //CU21 Resumen de información VTP
                    file = this.servicioTransfPotencia.GenerarReportePotenciaValor(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("Egresos"))
                {
                    //CU22 Egresos por compra de potencia
                    file = this.servicioTransfPotencia.GenerarReporteEgresos(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("IngresoPotencia"))
                {
                    //CU23 Ingresos por potencia
                    file = this.servicioTransfPotencia.GenerarReporteIngresoPotencia(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("ValorTransfPotencia"))
                {
                    //CU24 Saldos VTP
                    file = this.servicioTransfPotencia.GenerarReporteValorTransfPotencia(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
                }
                else if (reporte.Equals("Matriz"))
                {
                    //CU25 Matriz de pagos VTP - Solo se muestra en versión mensual
                    file = this.servicioTransfPotencia.GenerarReporteMatriz(pericodi, recpotcodi, formato, pathFile, pathLogo, out hoja);
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
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, app, sFecha + "_" + file);
        }
    }
}
