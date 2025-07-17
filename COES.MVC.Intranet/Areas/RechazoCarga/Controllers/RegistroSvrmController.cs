using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.RechazoCarga;

using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.Framework.Base.Core;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class RegistroSvrmController : BaseController
    {
        //
        // GET: /RechazoCarga/RegistroSvrm/

        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();

        private const string estadoRegistroNoEliminado = "1";
        private const string estadoRegistroEmpresaActivo = "A";
        private const int familiaEquipo = 45;
        private const int codUsuarioLibre = 4;
        private const string nombreReporteDescarga = "ReporteRegistroSvrm.xlsx";
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(RegistroSvrmController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("RegistroSvrmController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("RegistroSvrmController", ex);
                throw;
            }
        }
        public RegistroSvrmController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult ListarRegistroSvrm(string empresa, string codigoSuministro, string fecIni, string fecFin, 
            string maxDemComprometidaIni, string maxDemComprometidaFin, int nroPagina, int nroRegistros)
        {
            RegistroSvrmModel model = new RegistroSvrmModel();

            int regIni = 0;
            int regFin = 0;

            regIni = (nroPagina - 1) * nroRegistros + 1;
            regFin = nroPagina * nroRegistros;

            model.ListRegistroSvrm = servicio.ListRcaRegistroSvrmsFiltro(empresa, codigoSuministro, fecIni, fecFin, maxDemComprometidaIni, 
                maxDemComprometidaFin, estadoRegistroNoEliminado, regIni, regFin);
            return PartialView("ListarRegistroSvrm", model);
        }

         [HttpPost]
        public PartialViewResult ListarEmpresas(string empresa)
        {
            RegistroSvrmModel model = new RegistroSvrmModel();
            model.ListSiEmpresa = this.servicio.ListaEmpresasRechazoCarga(empresa, codUsuarioLibre, estadoRegistroEmpresaActivo);
            return PartialView("ListarEmpresas", model);
        }

        public ActionResult EliminarRegistroSvrm(int rcsvrmcodi)
        {
            this.servicio.DeleteRcaRegistroSvrm(rcsvrmcodi);

            return Json(new { success = true, message = "Ok" });
        }

        public JsonResult ObtenerListaPuntoMedicion(int codigoEmpresa, int codigoEquipo)
        {
            List<EqEquipoDTO> list = this.servicio.ObtenerEquiposPorFamilia(codigoEmpresa, familiaEquipo);
            if (codigoEquipo > 0)
            {
                list = list.Where(p => p.Equicodi == codigoEquipo).ToList();
            }            

            return Json(list);
        }
        [HttpGet]
        public ActionResult EditRegistroSvrm()
        {
            EditRegistroSvrmModel model = new EditRegistroSvrmModel();

            //model.ListSiEmpresa = servicio.ListaEmpresasRechazoCarga();
            //model.ListEqEquipo=servicio.ObtenerEquiposPorFamilia()

            return View("EditRegistroSvrm", model);
        }
        public JsonResult ObtenerRegistroSvrm(int rcsvrmcodi)
        {
            RcaRegistroSvrmDTO oRcaRegistroSvrmDTO = new RcaRegistroSvrmDTO();
            oRcaRegistroSvrmDTO = servicio.ObtenerRegistroSvrmPorCodigo(rcsvrmcodi);
           
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oRcaRegistroSvrmDTO));
        }

        public ActionResult GuardarRegistroSvrm(int codigoRegistroSvrm, int empresa, int puntoMedicion, decimal eracmfHP, decimal eracmfHFP, decimal eracmtHP, decimal eracmtHFP,
            decimal maxDemCont, decimal maxDemDisp, decimal maxDemComp, string documento, string fechaRegistro, string estado, bool esNuevo)
        {
            RcaRegistroSvrmDTO rcaRegistroSvrmDTO = new RcaRegistroSvrmDTO();
                        
            rcaRegistroSvrmDTO.Equicodi = puntoMedicion;
            rcaRegistroSvrmDTO.Rcsvrmhperacmf = eracmfHP;
            rcaRegistroSvrmDTO.Rcsvrmhfperacmf = eracmfHFP;
            rcaRegistroSvrmDTO.Rcsvrmhperacmt = eracmtHP;
            rcaRegistroSvrmDTO.Rcsvrmhfperacmt = eracmtHFP;
            rcaRegistroSvrmDTO.Rcsvrmmaxdemcont = maxDemCont;
            rcaRegistroSvrmDTO.Rcsvrmmaxdemdisp = maxDemDisp;
            rcaRegistroSvrmDTO.Rcsvrmmaxdemcomp = maxDemComp;

            rcaRegistroSvrmDTO.Rcsvrmdocumento = documento;
            rcaRegistroSvrmDTO.Rcsvrmfechavigencia = DateTime.ParseExact(fechaRegistro, "dd/MM/yyyy", null);
            rcaRegistroSvrmDTO.Rcsvrmestado = estado;            
            rcaRegistroSvrmDTO.Rcsvrmestregistro = estadoRegistroNoEliminado;
            rcaRegistroSvrmDTO.Rcsvrmusucreacion = User.Identity.Name;
            rcaRegistroSvrmDTO.Rcsvrmfeccreacion = DateTime.Now;
            rcaRegistroSvrmDTO.Rcsvrmusumodificacion = User.Identity.Name;
            rcaRegistroSvrmDTO.Rcsvrmfecmodificacion = DateTime.Now;

            if (esNuevo)
            {
                this.servicio.SaveRcaRegistroSvrm(rcaRegistroSvrmDTO);
            }
            else
            {
                rcaRegistroSvrmDTO.Rcsvrmcodi = codigoRegistroSvrm;
                this.servicio.UpdateRcaRegistroSvrm(rcaRegistroSvrmDTO);
            }


            return Json(new { success = true, message = "Ok" });
        }

        //Nuevos Metodos 10/02/2021
        public JsonResult GenerarReporte(string empresa, string codigoSuministro, string fecIni, string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel(empresa, codigoSuministro, fecIni, fecFin, maxDemComprometidaIni, maxDemComprometidaFin);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoExcel(string empresa, string codigoSuministro, string fecIni, string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin)
        {

            var preNombre = "RegistroSvrm" ;

            List<RcaRegistroSvrmDTO> listReporteInformacion = servicio.ListRcaRegistroSvrmsExcel(empresa, codigoSuministro, fecIni, fecFin, maxDemComprometidaIni, maxDemComprometidaFin, estadoRegistroNoEliminado);


            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var nombreHoja = "REPORTE";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                var contFila = 3;

                ws.Cells[1, 6, 1, 7].Merge = true;
                ws.Cells[1, 8, 1, 9].Merge = true;
                ws.Cells[1, 6, 1, 7].Value = "HP";
                ws.Cells[1, 8, 1, 9].Value = "HFP";



                ws.Cells[2, 1].Value = "RAZON SOCIAL";
                ws.Cells[2, 2].Value = "SUMINISTRADOR";
                ws.Cells[2, 3].Value = "SUB ESTACION";
                ws.Cells[2, 4].Value = "NOMBRE PUNTO MEDICION";
                ws.Cells[2, 5].Value = "MAX DEM CONTRATADA";
                ws.Cells[2, 6].Value = "ERACMF";//HP
                ws.Cells[2, 7].Value = "ERACMT";//HP
                ws.Cells[2, 8].Value = "ERACMF";
                ws.Cells[2, 9].Value = "ERACMT";
                ws.Cells[2, 10].Value = "MAX DEM DISPONIBLE";
                ws.Cells[2, 11].Value = "MAX DEM COMPROMETIDA(SVRM)";
                ws.Cells[2, 12].Value = "DOCUMENTO";
                ws.Cells[2, 13].Value = "FECHA DE VIGENCIA";
                ws.Cells[2, 14].Value = "ESTADO";


                ExcelRange rg1 = ws.Cells[2, 1, 2, 14];
                ObtenerEstiloCelda(rg1, 1);

                rg1 = ws.Cells[1, 6, 1, 7];
                ObtenerEstiloCelda(rg1, 2);

                rg1 = ws.Cells[1, 8, 1, 9];
                ObtenerEstiloCelda(rg1, 3);
                               
                ws.Cells[2, 6, 2, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[2, 6, 2, 7].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                               
                ws.Cells[2, 8, 2, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[2, 8, 2, 9].Style.Fill.BackgroundColor.SetColor(Color.Green);

                foreach (var registro in listReporteInformacion)
                {

                    ws.Cells[contFila, 1].Value = registro.Emprrazsocial;
                    ws.Cells[contFila, 2].Value = registro.Emprsum;
                    ws.Cells[contFila, 3].Value = registro.Areanomb;
                    ws.Cells[contFila, 4].Value = registro.Equinomb;
                    ws.Cells[contFila, 5].Value = registro.Rcsvrmmaxdemcont;
                    ws.Cells[contFila, 6].Value = registro.Rcsvrmhperacmf;
                    ws.Cells[contFila, 7].Value = registro.Rcsvrmhperacmt;
                    ws.Cells[contFila, 8].Value = registro.Rcsvrmhfperacmf;
                    ws.Cells[contFila, 9].Value = registro.Rcsvrmhfperacmt;
                    ws.Cells[contFila, 10].Value = registro.Rcsvrmmaxdemdisp;
                    ws.Cells[contFila, 11].Value = registro.Rcsvrmmaxdemcomp;
                    ws.Cells[contFila, 12].Value = registro.Rcsvrmdocumento;
                    ws.Cells[contFila, 13].Value = ((DateTime)registro.Rcsvrmfechavigencia).ToString("dd/MM/yyyy");
                    ws.Cells[contFila, 14].Value = registro.Rcsvrmestado.Equals("1") ? "Vigente" : "No Vigente";
                    //ws.Cells[contFila, 8].Value = "";                                                          

                    contFila++;
                }

                ws.Column(1).Width = 50;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 50;
                ws.Column(4).Width = 50;
                ws.Column(5).Width = 30;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                ws.Column(10).Width = 30;
                ws.Column(11).Width = 30;
                ws.Column(12).Width = 30;
                ws.Column(13).Width = 30;
                ws.Column(14).Width = 20;


                xlPackage.Save();
            }

            return fileName;
        }

        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                //rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;
                string colorborder = "#245C86";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            if (seccion == 3)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(Color.Green);
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato(string file)
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, nombreReporteDescarga);
        }

        /// <summary>
        /// Permite generar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string empresa, string codigoSuministro, string fecIni, string fecFin,
            string maxDemComprometidaIni, string maxDemComprometidaFin, int nroRegistrosPag)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = 0;

            nroRegistros = servicio.ListRcaRegistroSvrmsCount(empresa, codigoSuministro, fecIni, fecFin, maxDemComprometidaIni,
                maxDemComprometidaFin, estadoRegistroNoEliminado);


            if (nroRegistros > ConstantesRechazoCarga.NroPageShow)
            {
                //int pageSize = ConstantesRechazoCarga.PageSizeDemandaUsuario;
                int pageSize = nroRegistrosPag;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = ConstantesRechazoCarga.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);

        }
    }
}
