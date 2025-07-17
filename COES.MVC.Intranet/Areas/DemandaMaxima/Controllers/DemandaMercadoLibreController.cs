using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.DemandaMaxima.Models;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.RechazoCarga;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.IntercambioOsinergmin;
using COES.MVC.Intranet.Models;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Net;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.DemandaMaxima.Helper;
using log4net;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.DemandaMaxima.Controllers
{
    public class DemandaMercadoLibreController : BaseController
    {
        //
        // GET: /DemandaMaxima/DemandaMercadoLibre/
        RechazoCargaAppServicio servicioRechazoCarga = new RechazoCargaAppServicio();
        DemandaMaximaAppServicio servicioDemandaMaxima = new DemandaMaximaAppServicio();
        ImportacionAppServicio servicioImportacion = new ImportacionAppServicio();
        ReporteMedidoresAppServicio servReporte = new ReporteMedidoresAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(DemandaMercadoLibreController));

        public DemandaMercadoLibreController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("DemandaMercadoLibreController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("DemandaMercadoLibreController", ex);
                throw;
            }
        }
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            DemandaMercadoLibreModel model = new DemandaMercadoLibreModel();

            //model.ListaPeriodo = this.servicioRechazoCarga.ListaPeriodoReporte(ConfigurationManager.AppSettings["FechaInicioDemandaUsuario"]);//Fecha de inicio del despliqgue de la aplicación
            model.ListaPeriodo = this.servicioDemandaMaxima.ListaPeriodoReporte(ConfigurationManager.AppSettings["FechaInicioDemandaUsuario"]);//Fecha de inicio del despliqgue de la aplicación

            bool permisoEdicion = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);  

            var permiso = permisoEdicion ? "1" : "0";
            model.ListaPeriodoSicli = this.servicioDemandaMaxima.ListPeriodoSicli(permiso);
            
            List<RcaSuministradorDTO> suministradores = new List<RcaSuministradorDTO>();
            var listaSuministradores = servicioRechazoCarga.ListRcaSuministradores();
            model.Suministradores = listaSuministradores;

            ViewBag.PermisoEdicion = permisoEdicion ? "1" : "0";

            return View(model);
        }

        [HttpPost]
        public ActionResult ListarReporteInformacion(string periodo, string empresa, string suministrador, int nroPagina)
        {
            DemandaMercadoLibreModel model = new DemandaMercadoLibreModel();

            int regIni = 0;
            int regFin = 0;

            regIni = (nroPagina - 1) * ConstantesRechazoCarga.PageSizeDemandaUsuario + 1;
            regFin = nroPagina * ConstantesRechazoCarga.PageSizeDemandaUsuario;

            List<MeDemandaMLibreDTO> listReporteInformacion = this.servicioDemandaMaxima.ListDemandaMercadoLibreReporte(periodo, suministrador, empresa, regIni, regFin);


            model.ListaDemandaMercadoLibre = listReporteInformacion;
            model.registros = listReporteInformacion.Count().ToString();
            return View(model);
        }

        /// <summary>
        /// Permite generar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string periodo, string suministrador, string empresa)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = this.servicioDemandaMaxima.ListDemandaMercadoLibreReporteCount(periodo, suministrador, empresa);

            if (nroRegistros > ConstantesRechazoCarga.NroPageShow)
            {
                int pageSize = ConstantesRechazoCarga.PageSizeDemandaUsuario;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = ConstantesRechazoCarga.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);

        }

        public JsonResult obtenerMaximaDemanda(string periodoInicial)
        {
            int anho = Int32.Parse(periodoInicial.Substring(0, 4));
            int mes = Int32.Parse(periodoInicial.Substring(4, 2));
            //string[] formats = { Constantes.FormatoFecha };
            DateTime dti = new DateTime(anho, mes, 1);
            DateTime dtf = dti;

            DateTime fec_ini = new DateTime();
            DateTime fec_fin = new DateTime();

            fec_ini = new DateTime(dti.Year, dti.Month, 1);
            fec_fin = new DateTime(dtf.Year, dtf.Month, 1).AddMonths(1).AddDays(-1);

            DemandadiaDTO entity = new DemandadiaDTO();

            bool esPortal = false; //User.Identity.Name.Length == 0;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;
            var tipoCentral = 1;
            var tipoGeneracion = -1;
            var idEmpresa = -1;
            DateTime diaMaximaDemanda = this.servReporte.GetDiaPeriodoDemanda96XFiltro(fec_ini, fec_fin, ConstantesRepMaxDemanda.TipoMDNormativa,
                                                                        tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
            var maximaDemandaDTO = this.servReporte.GetResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion,
                                                                        idEmpresa, estadoValidacion);
            entity.FechaMD = maximaDemandaDTO.FechaOnlyDia;
            entity.HoraMD = maximaDemandaDTO.FechaOnlyHora;
            entity.ValorMD = maximaDemandaDTO.Valor;

            return Json(entity);
        }
        public JsonResult obtenerMaximaDemandaSicli(string periodoInicial)
        {
            int anho = Int32.Parse(periodoInicial.Substring(0, 4));
            int mes = Int32.Parse(periodoInicial.Substring(4, 2));
            //string[] formats = { Constantes.FormatoFecha };
            DateTime dti = new DateTime(anho, mes, 1);
            DateTime dtf = dti;

            DateTime fec_ini = new DateTime();
            DateTime fec_fin = new DateTime();

            fec_ini = new DateTime(dti.Year, dti.Month, 1);
            fec_fin = new DateTime(dtf.Year, dtf.Month, 1).AddMonths(1).AddDays(-1);

            //DemandadiaDTO entity = this.servicioDemandaMaxima.ObtenerDatosMaximaDemanda(fec_ini, fec_fin);
            DemandadiaDTO entity = new DemandadiaDTO();
            
            bool esPortal = false; //User.Identity.Name.Length == 0;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;
            var tipoCentral = 1;
            var tipoGeneracion = -1;
            var idEmpresa = -1;
            DateTime diaMaximaDemanda = this.servReporte.GetDiaPeriodoDemanda96XFiltro(fec_ini, fec_fin, ConstantesRepMaxDemanda.TipoMDNormativa, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
            var maximaDemandaDTO = this.servReporte.GetResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);
            entity.FechaMD = maximaDemandaDTO.FechaOnlyDia;
            entity.HoraMD = maximaDemandaDTO.FechaOnlyHora;
            entity.ValorMD = maximaDemandaDTO.Valor;

            var periodoSicli = servicioImportacion.PeriodoGetByCodigo(periodoInicial);

            entity.EstadoPeriodo = periodoSicli.PSicliCerrado;
            entity.EstadoPeriodoDemanda = periodoSicli.PSicliCerradoDemanda;

            return Json(entity);
        }

        public JsonResult generarRegistroDemandas(string periodo, string periodoSicli, string fechaDemandaMaxima, string fechaDemandaMaximaSicli)
        {
            base.ValidarSesionUsuario();
            //fechaDemandaMaxima = "24/03/2018";
            //fechaDemandaMaximaSicli = "24/03/2018";
            var resultado = 1;
            try
            {
                resultado = this.servicioDemandaMaxima.GenerarRegistroDemandas(periodo, periodoSicli, User.Identity.Name, fechaDemandaMaxima, fechaDemandaMaximaSicli);
            }
            catch(Exception ex)
            {
                Log.Error("DemandaMercadoLibreController", ex);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerarReporte(string periodo, string periodoSICLI, string empresa, string suministrador)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel( periodo, periodoSICLI, empresa,  suministrador);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        private string GenerarArchivoExcel(string periodo, string periodoSICLI, string empresa, string suministrador)
        {
            //var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Demanda_Mercado_Libre_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            //TransferenciaRentaCongestionModel transferenciaRentaCongestion = new TransferenciaRentaCongestionModel();
            var listDemandaMercadoLibre = this.servicioDemandaMaxima.ListDemandaMercadoLibreReporteExcel(periodo, periodoSICLI, suministrador, empresa);
            //transferenciaRentaCongestion.ListRentaCongestion = servicio.ListRentaCongestion(pericodi, recacodi);
            //transferenciaRentaCongestion.ListRentaCongestionDetalle = servicio.ListRentaCongestionDetalle(pericodi, recacodi);


            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Obtenemos los cuadros asociados al programa
                //var cuadroProgramas = servicio.GetByCriteriaRcaCuadroProgs(codigoPrograma.ToString(""), "");

                var contFila = 7;
                //var contHojas = 0;
                var nombreHojaAgentes = "Reporte_Principal";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHojaAgentes);

                contFila = 2;
                //var contItem = 3;

                ws.Cells[contFila, 1].Value = "Máxima Demanda";
                ws.Cells[contFila, 2].Value = "Fuente";
                //ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                ws.Cells[contFila, 3].Value = "Código Cliente";
                //ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                ws.Cells[contFila, 4].Value = "Suministrador";
                //ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                ws.Cells[contFila, 5].Value = "RUC";
                ws.Cells[contFila, 6].Value = "Nombre SICLI";
                ws.Cells[contFila, 7].Value = "Razón Social";
                ws.Cells[contFila, 8].Value = "Area (COES)";
                ws.Cells[contFila, 9].Value = "Tensión (kv)";
                ws.Cells[contFila, 10].Value = "Código barra de transferencia (IIO)";
                ws.Cells[contFila, 11].Value = "Subestación de referencia (IIO)";
                ws.Cells[contFila, 12].Value = "Tensión de referencia (KV) (IIO)";

                for (int i = 1; i <= 96; i++)
                {
                    ws.Cells[contFila, 12 + i].Value = TimeSpan.FromMinutes(i * 15).ToString("hh':'mm");
                }

                ExcelRange rg1 = ws.Cells[contFila, 1, contFila, 108];
                ObtenerEstiloCelda(rg1, 1);

                contFila++;
                using (listDemandaMercadoLibre)
                {
                    while (listDemandaMercadoLibre.Read())
                    {
                        //incremetar numero de fila

                        //ws.Cells[contFila, 2].Value = item;
                        ws.Cells[contFila, 1].Value = Convert.ToDateTime(listDemandaMercadoLibre["DMELIBFECMAXDEM"]).ToString("dd/MM/yyyy");

                        ws.Cells[contFila, 2].Value = listDemandaMercadoLibre["DMELIBFUENTE"] != null ?
                            listDemandaMercadoLibre["DMELIBFUENTE"].ToString().Trim() : "";

                        ws.Cells[contFila, 3].Value = listDemandaMercadoLibre["OSINERGCODI"] != null ?
                            listDemandaMercadoLibre["OSINERGCODI"].ToString().Trim() : "";

                        ws.Cells[contFila, 4].Value = listDemandaMercadoLibre["SUMINISTRADOR"] != null ?
                            listDemandaMercadoLibre["SUMINISTRADOR"].ToString().Trim() : "";

                        ws.Cells[contFila, 5].Value = listDemandaMercadoLibre["EMPRRUC"] != null ?
                            listDemandaMercadoLibre["EMPRRUC"].ToString().Trim() : "";

                        ws.Cells[contFila, 6].Value = listDemandaMercadoLibre["NOMBRE_SICLI"] != null ?
                            listDemandaMercadoLibre["NOMBRE_SICLI"].ToString().Trim() : "";

                        ws.Cells[contFila, 7].Value = listDemandaMercadoLibre["RAZONSOCIAL"] != null ?
                            listDemandaMercadoLibre["RAZONSOCIAL"].ToString().Trim() : "";

                        ws.Cells[contFila, 8].Value = listDemandaMercadoLibre["AREANOMB"] != null ?
                            listDemandaMercadoLibre["AREANOMB"].ToString().Trim() : "";

                        ws.Cells[contFila, 9].Value = listDemandaMercadoLibre["EQUITENSION"] != null
                               && !string.IsNullOrEmpty(listDemandaMercadoLibre["EQUITENSION"].ToString()) ?
                           Convert.ToDecimal(listDemandaMercadoLibre["EQUITENSION"].ToString()) : Decimal.Zero;

                        ws.Cells[contFila, 10].Value = listDemandaMercadoLibre["CODBARRAREFERENCIA"] != null ?
                            listDemandaMercadoLibre["CODBARRAREFERENCIA"].ToString().Trim() : "";

                        ws.Cells[contFila, 11].Value = listDemandaMercadoLibre["DESCBARRAREFERENCIA"] != null ?
                            listDemandaMercadoLibre["DESCBARRAREFERENCIA"].ToString().Trim() : "";

                        ws.Cells[contFila, 12].Value = listDemandaMercadoLibre["TENSIONREFERENCIA"] != null ?
                            listDemandaMercadoLibre["TENSIONREFERENCIA"].ToString().Trim() : "";

                        for (int i = 1; i <= 96; i++)
                        {
                            var nombreColumna = "DMELIBH" + i.ToString();
                            ws.Cells[contFila, 12 + i].Value = listDemandaMercadoLibre[nombreColumna] != null
                                && !string.IsNullOrEmpty(listDemandaMercadoLibre[nombreColumna].ToString()) ?
                            Convert.ToDecimal(listDemandaMercadoLibre[nombreColumna].ToString()) : Decimal.Zero;
                        }
                        
                        contFila++;
                        
                    }

                }

                ws.Column(1).Width = 20;
                ws.Column(2).Width = 20;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 50;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 50;
                ws.Column(7).Width = 50;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                ws.Column(9).Style.Numberformat.Format = "#,##0.00";
                ws.Column(10).Width = 20;
                ws.Column(11).Width = 20;
                ws.Column(12).Width = 20;

                for (int i = 1; i <= 96; i++)
                {
                    ws.Column(12 + i).Width = 10;
                    ws.Column(12 + i).Style.Numberformat.Format = "#,##0.00";
                }

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

            return rango;
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato(string file)
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, file);
        }

        public JsonResult actualizarPeriodo(string periodo, string estadoPeriodoDemanda)
        {
            base.ValidarSesionUsuario();

            var estadoPeriodo = estadoPeriodoDemanda.Equals("1") ? "0" : "1";

            this.servicioDemandaMaxima.UpdatePeriodoDemandaSicli(User.Identity.Name, periodo, estadoPeriodo);

            
            var periodoSicli = servicioImportacion.PeriodoGetByCodigo(periodo);

            //entity.EstadoPeriodo = periodoSicli.PSicliCerrado;
            //entity.EstadoPeriodoDemanda = periodoSicli.PSicliCerradoDemanda;

            return Json(periodoSicli);
        }
    }
}
