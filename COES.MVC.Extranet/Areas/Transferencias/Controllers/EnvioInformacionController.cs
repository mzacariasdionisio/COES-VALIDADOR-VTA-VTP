using COES.MVC.Extranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Helper;
using COES.MVC.Extranet.Areas.Transferencias.Models;
//using Excel;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Fondo = iTextSharp.text.pdf.fonts;
using System.Diagnostics;
using COES.Framework.Base.Tools;
using COES.Dominio.DTO.Enum;
using log4net;
using System.Configuration;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class EnvioInformacionController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EnvioInformacionController));

        #region  VARIABLES DE SESIÓN PARA EL HANSOMETABLE
        public List<string[]>[] ListaDatos
        {
            get
            {
                if (Session["ListaDatos"] != null)
                {
                    return (List<string[]>[])Session["ListaDatos"];
                }
                else
                {
                    return new List<string[]>[Funcion.iNroGrupos];
                }
            }
            set
            {
                Session["ListaDatos"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();
        private static string NombreControlador = "EnvioInformacionController";

        // GET: /Transferencias/EnvioInformacion/
        //[CustomAuthorize]
        public ActionResult Index(int pericodi = 0, int recacodi = 0, int tipoinfocodi = 0, int trnenvcodi = 0, int trnmodcodi = 0)
        {
            int iEmprCodi = 0;
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            EnvioInformacionModel modelEI = new EnvioInformacionModel();
            modelEI.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (modelEI.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = modelEI.ListaPeriodos[0].PeriCodi; }
            if (pericodi > 0)
            {
                modelEI.ListaRecalculo = this.servicioRecalculo.ListRecalculos(pericodi); //Ordenado en descendente
                if (modelEI.ListaRecalculo.Count > 0 && recacodi == 0)
                { recacodi = (int)modelEI.ListaRecalculo[0].RecaCodi; }
            }
            modelEI.ListaTipoInfo = ListTipoInfo();


            List<SeguridadServicio.EmpresaDTO> listTotal = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name);
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();

            //- aca debemos hacer jugada para escoger la empresa
            List<TrnInfoadicionalDTO> listaInfoAdicional = (new TransferenciasAppServicio()).ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();

            foreach (var item in listTotal)
            {
                list.Add(item);
                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        SeguridadServicio.EmpresaDTO dtoEmpresaConcepto = new SeguridadServicio.EmpresaDTO();
                        dtoEmpresaConcepto.EMPRCODI = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EMPRNOMB = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TIPOEMPRCODI = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        list.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }


            TempData["EMPRNRO"] = list.Count();
            if (list.Count() == 1)
            {
                TempData["EMPRNOMB"] = list[0].EMPRNOMB;
                Session["EmprNomb"] = list[0].EMPRNOMB;
                Session["EmprCodi"] = list[0].EMPRCODI;
                iEmprCodi = list[0].EMPRCODI;
            }
            else if (Session["EmprCodi"] != null)
            {
                iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                TempData["EMPRNOMB"] = dtoEmpresa.EmprNombre;
                Session["EmprNomb"] = dtoEmpresa.EmprNombre;
            }
            else if (list.Count() > 1)
            {
                TempData["EMPRNOMB"] = "";
                return View();

            }
            else
            {
                //No hay empresa asociada a la cuenta
                TempData["EMPRNOMB"] = "";
                TempData["EMPRNRO"] = -1;
                return View();
            }
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodo = (new PeriodoAppServicio()).ListPeriodo();

            TempData["PERIANIOMES1"] = new SelectList(modelPeriodo.ListaPeriodo, "PERICODI", "PERINOMBRE");
            TempData["PERIANIOMES2"] = new SelectList(modelPeriodo.ListaPeriodo, "PERICODI", "PERINOMBRE");
            TempData["PERIANIOMES3"] = new SelectList(modelPeriodo.ListaPeriodo, "PERICODI", "PERINOMBRE");
            TempData["PERIANIOMES4"] = new SelectList(modelPeriodo.ListaPeriodo, "PERICODI", "PERINOMBRE");

            EmpresaModel modelEmpER = new EmpresaModel();
            modelEmpER.ListaEmpresas = (new EmpresaAppServicio()).ListInterCodEntregaRetiro();
            EmpresaModel modelEmpIB = new EmpresaModel();
            modelEmpIB.ListaEmpresas = (new EmpresaAppServicio()).ListaInterCodInfoBase();
            TempData["EMPRCODI3"] = modelEmpER;
            TempData["EMPRCODI4"] = modelEmpIB;

            BarraModel modelBarr = new BarraModel();
            modelBarr.ListaBarras = (new BarraAppServicio()).ListaBarraTransferencia();
            TempData["BARRCODI3"] = new SelectList(modelBarr.ListaBarras, "BARRCODI", "BARRNOMBBARRTRAN");

            string sTipoInformacion = TipoInformacion(tipoinfocodi);
            if (sTipoInformacion.Equals("DM"))
            {
                modelEI.ListaModelo = this.servicioTransferencia.ListarTrnModeloByEmpresa(iEmprCodi);

            }
            if (pericodi > 0 && recacodi > 0)
            {
                modelEI.EntidadRecalculo = this.servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
                if (trnenvcodi == 0)
                {
                    modelEI.EntidadEnvio = this.servicioTransferencia.GetByTrnEnvioIdPeriodoEmpresa(pericodi, recacodi, iEmprCodi, sTipoInformacion, trnmodcodi);
                    if (modelEI.EntidadEnvio != null)
                    {
                        trnenvcodi = modelEI.EntidadEnvio.TrnEnvCodi;
                        trnmodcodi = modelEI.EntidadEnvio.TrnModCodi;
                    }
                    else
                        modelEI.EntidadEnvio = new TrnEnvioDTO();
                }
                else
                {
                    modelEI.EntidadEnvio = this.servicioTransferencia.GetByIdTrnEnvio(trnenvcodi);
                    if (modelEI.EntidadEnvio != null && trnmodcodi == 0)
                    {
                        trnmodcodi = modelEI.EntidadEnvio.TrnModCodi;
                    }
                }
            }
            else
            {
                modelEI.EntidadRecalculo = new RecalculoDTO();
                modelEI.EntidadEnvio = new TrnEnvioDTO();
            }
            modelEI.iNroGrupos = Funcion.iNroGrupos;
            modelEI.Trnenvcodi = trnenvcodi;
            modelEI.Trnmodcodi = trnmodcodi;
            modelEI.Pericodi = pericodi;
            modelEI.Recacodi = recacodi;
            modelEI.Emprcodi = iEmprCodi;
            modelEI.Tipoinfocodi = tipoinfocodi;
            //modelEI.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return View(modelEI);
        }

        [HttpPost]
        public ActionResult Lista(int pericodi)
        {
            int emprcodi = Convert.ToInt32(Session["EmprCodi"].ToString());
            EnvioInformacionModel modelo = new EnvioInformacionModel();
            modelo.ListaRecalculo = (new RecalculoAppServicio()).ListRecalculosTrnCodigoEnviado(pericodi, emprcodi);
            //Lista de codigos que debe reportar:
            modelo.ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiro(pericodi, emprcodi);
            string sCodigosEntregar = "0";
            foreach (ExportExcelDTO dtoEntregar in modelo.ListaEntregReti)
            {
                sCodigosEntregar += "," + dtoEntregar.CodiEntreRetiCodigo + "";
            }
            int i = 0;
            foreach (RecalculoDTO dto in modelo.ListaRecalculo)
            {
                modelo.ListaRecalculo[i].RecaNota2 = "";
                //Por versión listamos los codigos reportados
                modelo.ListaEntregReti = (new TransferenciaEntregaRetiroAppServicio()).ListarCodigoReportado(emprcodi, pericodi, dto.RecaCodi);
                foreach (ExportExcelDTO dtoReportado in modelo.ListaEntregReti)
                {
                    if (modelo.ListaRecalculo[i].RecaNota2.Equals(""))
                    {
                        modelo.ListaRecalculo[i].RecaNota2 = "" + dtoReportado.CodiEntreRetiCodigo + "";
                    }
                    else
                    {
                        modelo.ListaRecalculo[i].RecaNota2 += ", " + dtoReportado.CodiEntreRetiCodigo + " ";
                    }
                    //Por versión, reportamos los codigos faltantes
                    int iPosInicial = sCodigosEntregar.IndexOf(dtoReportado.CodiEntreRetiCodigo);
                    if (iPosInicial >= 0)
                    {
                        sCodigosEntregar = sCodigosEntregar.Substring(0, iPosInicial - 1) + sCodigosEntregar.Substring(iPosInicial + 10);
                    }
                }
                if (i == 0)
                {
                    sCodigosEntregar = sCodigosEntregar.Substring(1);
                    if (sCodigosEntregar.Equals(""))
                        modelo.ListaRecalculo[i].RecaNroInforme = "No hay pendientes";
                    else
                        modelo.ListaRecalculo[i].RecaNroInforme = sCodigosEntregar;
                }
                else
                    modelo.ListaRecalculo[i].RecaNroInforme = "Cerrado";
                i++;
            }
            return PartialView(modelo);
        }

        /// <summary>
        /// Permite consultar si el periodo y la versión del recalculo esta disponible para subir información
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPermiso(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.bEjecutar = true;
            //Consultamos por el estado del periodo
            PeriodoDTO entidad = new PeriodoDTO();
            entidad = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
            if (entidad.PeriEstado.Equals("Cerrado"))
            { modelRecalculo.bEjecutar = false; }
            else
            {   //Consultamos por la fecha limite para el envio de información
                try
                {   //Si todo el proceso sale bien 
                    IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                    string sHora = entidad.PeriHoraLimite;
                    double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                    double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                    DateTime dDiaHoraLimite = entidad.PeriFechaLimite.AddHours(dHora);
                    dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                    if (dDiaHoraLimite < System.DateTime.Now)
                    { modelRecalculo.bEjecutar = false; }
                }
                catch (Exception e)
                {   // Error en la conversión del tipo hora a fecha.
                    string sMensaje = e.ToString();

                }
            }
            return Json(modelRecalculo);
        }

        #region Antigua forma de carga de VTEA
        [HttpPost]
        public JsonResult GenerarExcel(int periodo)
        {
            try
            {
                if (Session["EmprCodi"] != null)
                {
                    int iEmprcodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                    PeriodoModel modelPeriodo = new PeriodoModel();
                    modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(periodo);

                    EmpresaModel modelEmpresa = new EmpresaModel();
                    modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprcodi);
                    string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                    Session["sPrefijoExcel"] = sPrefijoExcel;

                    CodigoRetiroModel modelCodigoRetiro = new CodigoRetiroModel();
                    EnvioInformacionModel model = new EnvioInformacionModel();
                    //Buacamos todos los codigos de entrega y retiro que estan validos para el periodo seleccionado
                    model.ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiro(periodo, iEmprcodi);

                    //FileInfo template = new FileInfo(path + Funcion.NombrePlantillaFormatoEntregaRetiroExcel);
                    string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;
                    FileInfo newFile = new FileInfo(path + Funcion.NombreReporteFormatoEntregaRetiroExcel);
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(path + Funcion.NombreReporteFormatoEntregaRetiroExcel);
                    }

                    int row = 4;
                    int row2 = 3;
                    int colum = 2;
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                        if (ws != null)
                        {

                            //TITULO
                            ws.Cells[2, 3].Value = "Fomarto de Entregas y Retiros: " + sPrefijoExcel;
                            ExcelRange rg = ws.Cells[2, 3, 2, 3];
                            rg.Style.Font.Size = 16;
                            rg.Style.Font.Bold = true;
                            //CABECERA DE TABLA
                            ws.Cells[5, 2].Value = "EMPRESA";
                            ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                            ws.Cells[5, 4].Value = "ENTREGA/RETIRO";
                            ws.Cells[5, 5].Value = "TIPO";
                            ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                            rg = ws.Cells[5, 2, 5, 6];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;
                            row = 6;
                            foreach (var item in model.ListaEntregReti)
                            {
                                ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                                ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                                ws.Cells[row, 4].Value = (item.CodiEntreRetiCodigo != null) ? item.CodiEntreRetiCodigo : string.Empty;
                                ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                                ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;

                                //Border por celda
                                rg = ws.Cells[row, 2, row, 6];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                            //Fijar panel
                            ws.View.FreezePanes(6, 7);
                            //Ajustar columnas
                            rg = ws.Cells[5, 2, row, 6];
                            rg.AutoFitColumns();

                            //Insertar el logo
                            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                            ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                            picture.From.Column = 1;
                            picture.From.Row = 1;
                            picture.To.Column = 2;
                            picture.To.Row = 2;
                            picture.SetSize(120, 60);
                            //SEGUNDA HOJA

                            ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                            ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                            foreach (var item in model.ListaEntregReti)
                            {
                                ws2.Cells[1, colum].Value = (item.CodiEntreRetiCodigo != null) ? item.CodiEntreRetiCodigo.ToString() : string.Empty;
                                ws2.Cells[2, colum].Value = "FINAL";
                                ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                                colum++;
                            }
                            //Color de fondo
                            rg = ws2.Cells[1, 1, 2, colum - 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            ////////////////////////////

                            string sMes = modelPeriodo.Entidad.MesCodi.ToString();
                            if (sMes.Length == 1) sMes = "0" + sMes;
                            var Fecha = "01/" + sMes + "/" + modelPeriodo.Entidad.AnioCodi;
                            var dates = new List<DateTime>();
                            var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(Fecha);
                            var dateFin = dateIni.AddMonths(1);

                            dateIni = dateIni.AddMinutes(15);
                            for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                            {
                                dates.Add(dt);
                            }

                            foreach (var item in dates)
                            {
                                ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                                row2++;
                            }
                            rg = ws2.Cells[3, 1, row2 - 1, 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            //Border por celda
                            rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
                            rg.AutoFitColumns();
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                        }
                        xlPackage.Save();
                    }
                    return Json("1");
                }
                else
                    return Json("Lo sentimos, no se ha procesado la información, favor de seleccionar una empresa primero");
            }
            catch (Exception e)
            {
                return Json(e.Message);//("-1");
            }
        }

        [HttpPost]
        public JsonResult GenerarExcelBase(int periodo)
        {
            try
            {
                if (Session["EmprCodi"] != null)
                {
                    int iEmprcodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                    PeriodoModel modelPeriodo = new PeriodoModel();
                    modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(periodo);

                    EmpresaModel modelEmpresa = new EmpresaModel();
                    modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprcodi);
                    string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                    Session["sPrefijoExcelBase"] = sPrefijoExcel;

                    //CodigoRetiroModel modelCodigoRetiro = new CodigoRetiroModel();
                    EnvioInformacionModel model = new EnvioInformacionModel();
                    //Buacamos todos los codigos de entrega y retiro que estan validos para el periodo seleccionado
                    model.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoInfoBase(periodo, iEmprcodi);

                    //FileInfo template = new FileInfo(path + Funcion.NombrePlantillaFormatoInformacionBaseExcel);
                    string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;
                    FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInformacionBaseExcel);
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(path + Funcion.NombreReporteInformacionBaseExcel);
                    }

                    int row2 = 3;
                    int colum = 2;
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                        if (ws != null)
                        {
                            //TITULO
                            ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE INFORMACIÓN BASE DE " + sPrefijoExcel;
                            ExcelRange rg = ws.Cells[2, 3, 2, 3];
                            rg.Style.Font.Size = 16;
                            rg.Style.Font.Bold = true;
                            //CABECERA DE TABLA
                            ws.Cells[5, 2].Value = "EMPRESA";
                            ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                            ws.Cells[5, 4].Value = "CODIGO";
                            ws.Cells[5, 5].Value = "TIPO";
                            ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                            rg = ws.Cells[5, 2, 5, 6];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            int row = 6;
                            foreach (var item in model.ListaEntregReti)
                            {
                                ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                                ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                                ws.Cells[row, 4].Value = (item.CoInfbCodigo != null) ? item.CoInfbCodigo : string.Empty;
                                ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                                ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
                                //Border por celda
                                rg = ws.Cells[row, 2, row, 6];
                                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Left.Color.SetColor(Color.Black);
                                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Right.Color.SetColor(Color.Black);
                                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Top.Color.SetColor(Color.Black);
                                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                                rg.Style.Font.Size = 10;
                                row++;
                            }
                            //Fijar panel
                            ws.View.FreezePanes(6, 7);
                            //Ajustar columnas
                            rg = ws.Cells[5, 2, row, 6];
                            rg.AutoFitColumns();

                            //Insertar el logo
                            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                            ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                            picture.From.Column = 1;
                            picture.From.Row = 1;
                            picture.To.Column = 2;
                            picture.To.Row = 2;
                            picture.SetSize(120, 60);


                            ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                            ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                            foreach (var item in model.ListaEntregReti)
                            {
                                ws2.Cells[1, colum].Value = (item.CoInfbCodigo != null) ? item.CoInfbCodigo.ToString() : string.Empty;
                                ws2.Cells[2, colum].Value = "FINAL";
                                ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                                colum++;
                            }
                            //Color de fondo
                            rg = ws2.Cells[1, 1, 2, colum - 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                            rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            ////////////////////////////

                            string sMes = modelPeriodo.Entidad.MesCodi.ToString();
                            if (sMes.Length == 1) sMes = "0" + sMes;
                            var Fecha = "01/" + sMes + "/" + modelPeriodo.Entidad.AnioCodi;
                            var dates = new List<DateTime>();
                            var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture); //Convert.ToDateTime(Fecha);
                            var dateFin = dateIni.AddMonths(1);

                            dateIni = dateIni.AddMinutes(15);
                            for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                            {
                                dates.Add(dt);
                            }

                            foreach (var item in dates)
                            {
                                ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                                row2++;
                            }
                            rg = ws2.Cells[3, 1, row2 - 1, 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Bold = true;

                            //Border por celda
                            rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
                            rg.AutoFitColumns();
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                        }
                        xlPackage.Save();
                    }
                    return Json("1");
                }
                else
                    return Json("Lo sentimos, no se ha procesado la información, favor de seleccionar una empresa primero");
            }
            catch (Exception e)
            {
                return Json(e.Message);//("-1");
            }
        }

        [HttpGet]
        public virtual ActionResult AbrirExcel()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + Funcion.NombreReporteFormatoEntregaRetiroExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcel"].ToString() + "_" + Funcion.NombreReporteFormatoEntregaRetiroExcel);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcelBase()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + Funcion.NombreReporteInformacionBaseExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcelBase"].ToString() + "_" + Funcion.NombreReporteInformacionBaseExcel);
        }

        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculos = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            return Json(modelRecalculo.ListaRecalculos);
        }

        public JsonResult GetEmpresasxPeriodo(int pericodi, int version)
        {
            EmpresaModel modelEmpER = new EmpresaModel();
            modelEmpER.ListaEmpresas = (new EmpresaAppServicio()).ListInterCodEntregaRetiroxPeriodo(pericodi, version);

            return Json(modelEmpER);
        }

        [HttpPost]
        public ActionResult Upload(string sFecha)
        {
            string sNombreArchivo = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = sFecha + "_" + file.FileName;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }

                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UploadBase(string sFecha)
        {
            string sNombreArchivo = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = sFecha + "_" + file.FileName;

                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }

                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite procesar el archivo cargado en un directorio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sNombreArchivo, int iPericodi)
        {
            try
            {
                //session de la empresa
                if (Session["EmprCodi"] != null)
                {
                    PeriodoDTO periodo = (new PeriodoAppServicio()).GetByIdPeriodo(iPericodi);
                    int nroDias = DateTime.DaysInMonth(periodo.AnioCodi, periodo.MesCodi);

                    int iEmprcodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                    CodigoEntregaAppServicio servicioTrnCodigoEntrega = new CodigoEntregaAppServicio();
                    string pathfinal = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + "/" + sNombreArchivo;
                    int tramver = (new RecalculoAppServicio()).GetUltimaVersion(iPericodi);
                    var dt = new DataTable();
                    List<string> erroresValor = new List<string>();
                    List<string> erroresDatos = new List<string>();

                    using (var reader = new ExcelDataReader(pathfinal, 1, false))
                        dt.Load(reader);

                    List<DatosTransferencia> listData = Funcion.TransformarData(dt, nroDias, iEmprcodi, out erroresValor, out erroresDatos);
                    List<DatosTransferencia> resultadoProceso = servicioTrnCodigoEntrega.GrabarEntregaRetiro(listData,
                        iPericodi, tramver, iEmprcodi, User.Identity.Name, 0);
                    List<string> codigosCorrectos = resultadoProceso.Where(x => !string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();
                    List<string> codigosErroneos = resultadoProceso.Where(x => string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();


                    #region AuditoriaProceso

                    VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                    objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.CargaInformacionVTPExtranet;
                    objAuditoria.Estdcodi = (int)EVtpEstados.SubirFormato;
                    objAuditoria.Audproproceso = "Importación de data en excel extranet vtea";
                    objAuditoria.Audprodescripcion = "Se importa la data del periodo " + periodo.PeriNombre + " - cantidad de errores - 0"  + " - usuario " + User.Identity.Name;
                    objAuditoria.Audprousucreacion = User.Identity.Name;
                    objAuditoria.Audprofeccreacion = DateTime.Now;

                    int auditoria = this.servicioAuditoria.save(objAuditoria);
                    if (auditoria == 0)
                    {
                        Logger.Error(NombreControlador + " - Error Save Auditoria - Importar Data Extranet VTEA");
                    }

                    #endregion

                    //ELIMINAMOS EL ARCHIVO TEMPORAL DEL SERVIDOR
                    System.IO.File.Delete(pathfinal);

                    if (erroresValor.Count > 0 || erroresDatos.Count > 0 || codigosErroneos.Count > 0)
                    {
                        return Json("Códigos Correctos [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos) + "\n" +
                                    "Códigos Incorrectos [" + codigosErroneos.Count + "]: " + string.Join(", ", codigosErroneos) + "\n" +
                                    "Códigos con valor superior a 350 MWh [" + erroresValor.Count + "]: " + string.Join(", ", erroresValor) + "\n" +
                                    "Códigos con valores erróneos [" + erroresDatos.Count + "]: " + string.Join(", ", erroresDatos) + "\n"
                                    );
                    }
                    else
                    {
                        return Json("Código(s) correcto(s) [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos));
                    }
                }
                else
                {
                    return Json("Lo sentimos, no se ha procesado la información, favor de seleccionar una empresa primero");
                }
            }
            catch (Exception ex)
            {
                return Json("Lo sentimos se ha producido un error al momento de leer los valores de energia " + ex.Message);
            }
        }

        /// <summary>
        /// Permite procesar el archivo cargado en un directorio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoBase(string sNombreArchivo, int iPericodi)
        {
            string pathfinal = "";
            string extension = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;

            try
            {
                //session de la empresa
                if (Session["EmprCodi"] != null)
                {
                    int iEmprcodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                    //tratamos el archivo cargado en el directorio
                    pathfinal = path + "/" + sNombreArchivo;
                    //obtiene extension
                    extension = Path.GetExtension(pathfinal);

                    DataSet ds = new DataSet();
                    ds = (new Funcion()).GeneraDataset(pathfinal, 2);

                    CodigoInfoBaseModel modelInfoBase = new CodigoInfoBaseModel();
                    TransferenciaInformacionBaseModel modelTransInfoBase = new TransferenciaInformacionBaseModel();
                    TransferenciaInformacionBaseDTO entidadTInfoBase = new TransferenciaInformacionBaseDTO();
                    TransferenciaInformacionBaseDetalleDTO entidadTInfoBaseDetalle = new TransferenciaInformacionBaseDetalleDTO();
                    //////VARIABLES PARA CONFIRMACION DE ELIMNAR REGISTROS////////////
                    int delInfobaseok = 0;
                    int delInfobasedetaok = 0;

                    //obtener el codigo de la version
                    int tramver = 0;
                    tramver = new RecalculoAppServicio().GetUltimaVersion(iPericodi);

                    //ARRAYLIST DETA INFOBASE
                    ArrayList arrylist = new ArrayList();

                    //ArrayLIST DETA RETIRO
                    ArrayList arrylistR = new ArrayList();

                    //int Clicodi = 0;
                    int cod = 0;
                    decimal promedio = 0;
                    int DatosVacios = 0;
                    string sAux = "";
                    double dAux = 0;
                    bool isDouble = false;
                    decimal suma = 0;
                    string sResultadoCodigoNoExiste = ""; //Resultado del procesamiento: 1:Exito, sin errores
                    string sResultadoCodigoDataErrada = ""; //Resultado del procesamiento de codigos con errores en sus casillas
                    string sResultadoCodigoDataExcedida = ""; //Resultado del procesamiento de codigos con errores en sus casillas
                    string sCodigosCorrectos = "";
                    int CodigosCorrectos = 0;
                    int CodigosErrados = 0;
                    int iSaveOk = 0;

                    ArrayList listColuTodo = new ArrayList();
                    //Recorremos columna por coluna el archivo
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        bool bSalir = false;
                        foreach (DataRow dtRow in ds.Tables[0].Rows)
                        {
                            if (dc.ColumnName.ToString().Equals("Column1"))
                            {   //Primera columna que contiene los encabezados
                                bSalir = true;
                                break;
                            }
                            else
                            {   //A partir de la segunda columna que contiene los datos
                                string sCelda = dtRow[dc].ToString();
                                if (String.IsNullOrEmpty(sCelda))
                                {
                                    listColuTodo.Add("0");
                                    DatosVacios++;
                                }
                                else if (listColuTodo.Count == 0)
                                {   //Celda que contiene: Final, Preliminar, Mejor Información
                                    sCelda = sCelda.Trim().ToUpper();
                                    if (!(sCelda.Equals("FINAL") || sCelda.Equals("MEJOR INFORMACIÓN")))
                                        sCelda = "PRELIMINAR";
                                    listColuTodo.Add(sCelda.Trim());
                                    CodigosCorrectos++;
                                }
                                else
                                {   //Lectura de datos
                                    sAux = sCelda.Trim();
                                    isDouble = Double.TryParse(sAux, out dAux);
                                    if (isDouble)
                                        if (dAux <= Funcion.dLimiteMaxEnergia)
                                        {   //Dato correcto, se inserta a la lista
                                            listColuTodo.Add(dAux);
                                        }
                                        else
                                        {   //Error, en el codigo uno de sus valores excede dLimiteMaxEnergia. No se graba el codigo y se continua con el siguiente
                                            if (sResultadoCodigoDataExcedida.Equals(""))
                                                sResultadoCodigoDataExcedida = " Código con información que excede los 350 MWh: [" + dc.ColumnName.ToString() + "]";
                                            else
                                            {
                                                sResultadoCodigoDataExcedida = sResultadoCodigoDataExcedida + ", [" + dc.ColumnName.ToString() + "]";
                                            }
                                            bSalir = true;
                                            CodigosErrados++;
                                            CodigosCorrectos--;
                                            break;
                                        }
                                    else
                                    {   //Tienes caracteres extraños null por defecto (GeneraDataset)
                                        if (sResultadoCodigoDataErrada.Equals(""))
                                            sResultadoCodigoDataErrada = " Código con información nula o errada: [" + dc.ColumnName.ToString() + "]";
                                        else
                                        {
                                            sResultadoCodigoDataErrada = sResultadoCodigoDataErrada + ", [" + dc.ColumnName.ToString() + "]";
                                        }
                                        bSalir = true;
                                        CodigosErrados++;
                                        CodigosCorrectos--;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!bSalir)
                        {   //Si la data es correcta
                            string sCodigo = dc.ColumnName.ToString().Trim();
                            modelInfoBase.Entidad = (new CodigoInfoBaseAppServicio()).CodigoInfoBaseVigenteByPeriodo(iPericodi, sCodigo);
                            if (modelInfoBase.Entidad != null && modelInfoBase.Entidad.EmprCodi == iEmprcodi)
                            {
                                //Eliminar registro de codigo de información base
                                delInfobasedetaok = new TransferenciaInformacionBaseAppServicio().DeleteTransferenciaInformacionBaseDetalle(iPericodi, tramver, sCodigo);
                                delInfobaseok = new TransferenciaInformacionBaseAppServicio().DeleteTransferenciaInfoInformacionBase(iPericodi, tramver, sCodigo);

                                //modelCodigoEntrega.Entidad = (new GeneralAppServicioCodigoEntrega()).GetByCodigoEntregaCodigo(dc.ColumnName.ToString());
                                entidadTInfoBase.CoInfbCodi = modelInfoBase.Entidad.CoInfBCodi;
                                entidadTInfoBase.EmprCodi = modelInfoBase.Entidad.EmprCodi;
                                entidadTInfoBase.BarrCodi = modelInfoBase.Entidad.BarrCodi;
                                entidadTInfoBase.TinfbCodigo = sCodigo;
                                entidadTInfoBase.EquiCodi = modelInfoBase.Entidad.CentGeneCodi;
                                entidadTInfoBase.PeriCodi = iPericodi;
                                entidadTInfoBase.TinfbVersion = tramver;
                                entidadTInfoBase.TinfbTipoInformacion = listColuTodo[0].ToString();
                                entidadTInfoBase.TinfbUserName = User.Identity.Name;
                                cod = (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBase(entidadTInfoBase);

                                //Graba detalle
                                int cantidadve = 96;
                                ArrayList Listpordias = new ArrayList(cantidadve);
                                for (int c = 1; c < listColuTodo.Count; c += cantidadve)
                                {
                                    var arrylistdDiaED = new ArrayList();
                                    arrylistdDiaED.AddRange(listColuTodo.GetRange(c, cantidadve));
                                    Listpordias.Add(arrylistdDiaED);
                                }

                                for (int c = 0; c < Listpordias.Count; c++)
                                {
                                    arrylist = (ArrayList)Listpordias[c];

                                    entidadTInfoBaseDetalle.TinfbCodi = cod;
                                    entidadTInfoBaseDetalle.TinfbDeDia = (c + 1);
                                    entidadTInfoBaseDetalle.TinfbDeVersion = tramver;
                                    entidadTInfoBaseDetalle.TinfbDeUserName = User.Identity.Name;

                                    suma = 0;
                                    try
                                    {
                                        suma += entidadTInfoBaseDetalle.TinfbDe1 = Decimal.Parse(arrylist[0].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe2 = Decimal.Parse(arrylist[1].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe3 = Decimal.Parse(arrylist[2].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe4 = Decimal.Parse(arrylist[3].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe5 = Decimal.Parse(arrylist[4].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe6 = Decimal.Parse(arrylist[5].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe7 = Decimal.Parse(arrylist[6].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe8 = Decimal.Parse(arrylist[7].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe9 = Decimal.Parse(arrylist[8].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe10 = Decimal.Parse(arrylist[9].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe11 = Decimal.Parse(arrylist[10].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe12 = Decimal.Parse(arrylist[11].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe13 = Decimal.Parse(arrylist[12].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe14 = Decimal.Parse(arrylist[13].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe15 = Decimal.Parse(arrylist[14].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe16 = Decimal.Parse(arrylist[15].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe17 = Decimal.Parse(arrylist[16].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe18 = Decimal.Parse(arrylist[17].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe19 = Decimal.Parse(arrylist[18].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe20 = Decimal.Parse(arrylist[19].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe21 = Decimal.Parse(arrylist[20].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe22 = Decimal.Parse(arrylist[21].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe23 = Decimal.Parse(arrylist[22].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe24 = Decimal.Parse(arrylist[23].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe25 = Decimal.Parse(arrylist[24].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe26 = Decimal.Parse(arrylist[25].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe27 = Decimal.Parse(arrylist[26].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe28 = Decimal.Parse(arrylist[27].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe29 = Decimal.Parse(arrylist[28].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe30 = Decimal.Parse(arrylist[29].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe31 = Decimal.Parse(arrylist[30].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe32 = Decimal.Parse(arrylist[31].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe33 = Decimal.Parse(arrylist[32].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe34 = Decimal.Parse(arrylist[33].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe35 = Decimal.Parse(arrylist[34].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe36 = Decimal.Parse(arrylist[35].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe37 = Decimal.Parse(arrylist[36].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe38 = Decimal.Parse(arrylist[37].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe39 = Decimal.Parse(arrylist[38].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe40 = Decimal.Parse(arrylist[39].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe41 = Decimal.Parse(arrylist[40].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe42 = Decimal.Parse(arrylist[41].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe43 = Decimal.Parse(arrylist[42].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe44 = Decimal.Parse(arrylist[43].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe45 = Decimal.Parse(arrylist[44].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe46 = Decimal.Parse(arrylist[45].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe47 = Decimal.Parse(arrylist[46].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe48 = Decimal.Parse(arrylist[47].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe49 = Decimal.Parse(arrylist[48].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe50 = Decimal.Parse(arrylist[49].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe51 = Decimal.Parse(arrylist[50].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe52 = Decimal.Parse(arrylist[51].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe53 = Decimal.Parse(arrylist[52].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe54 = Decimal.Parse(arrylist[53].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe55 = Decimal.Parse(arrylist[54].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe56 = Decimal.Parse(arrylist[55].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe57 = Decimal.Parse(arrylist[56].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe58 = Decimal.Parse(arrylist[57].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe59 = Decimal.Parse(arrylist[58].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe60 = Decimal.Parse(arrylist[59].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe61 = Decimal.Parse(arrylist[60].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe62 = Decimal.Parse(arrylist[61].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe63 = Decimal.Parse(arrylist[62].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe64 = Decimal.Parse(arrylist[63].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe65 = Decimal.Parse(arrylist[64].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe66 = Decimal.Parse(arrylist[65].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe67 = Decimal.Parse(arrylist[66].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe68 = Decimal.Parse(arrylist[67].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe69 = Decimal.Parse(arrylist[68].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe70 = Decimal.Parse(arrylist[69].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe71 = Decimal.Parse(arrylist[70].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe72 = Decimal.Parse(arrylist[71].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe73 = Decimal.Parse(arrylist[72].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe74 = Decimal.Parse(arrylist[73].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe75 = Decimal.Parse(arrylist[74].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe76 = Decimal.Parse(arrylist[75].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe77 = Decimal.Parse(arrylist[76].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe78 = Decimal.Parse(arrylist[77].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe79 = Decimal.Parse(arrylist[78].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe80 = Decimal.Parse(arrylist[79].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe81 = Decimal.Parse(arrylist[80].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe82 = Decimal.Parse(arrylist[81].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe83 = Decimal.Parse(arrylist[82].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe84 = Decimal.Parse(arrylist[83].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe85 = Decimal.Parse(arrylist[84].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe86 = Decimal.Parse(arrylist[85].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe87 = Decimal.Parse(arrylist[86].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe88 = Decimal.Parse(arrylist[87].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe89 = Decimal.Parse(arrylist[88].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe90 = Decimal.Parse(arrylist[89].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe91 = Decimal.Parse(arrylist[90].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe92 = Decimal.Parse(arrylist[91].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe93 = Decimal.Parse(arrylist[92].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe94 = Decimal.Parse(arrylist[93].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe95 = Decimal.Parse(arrylist[94].ToString(), System.Globalization.NumberStyles.Float);
                                        suma += entidadTInfoBaseDetalle.TinfbDe96 = Decimal.Parse(arrylist[95].ToString(), System.Globalization.NumberStyles.Float);
                                    }
                                    catch
                                    {
                                        return Json("Lo sentimos, se ha producido un error en la lectura de información del código: " + entidadTInfoBase.TinfbCodigo);
                                    }
                                    entidadTInfoBaseDetalle.TinfbDeSumaDia = suma;
                                    promedio = Math.Round((suma / 96), 6);
                                    entidadTInfoBaseDetalle.TinfbDePromedioDia = promedio;

                                    iSaveOk = 0;
                                    iSaveOk = (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBaseDetalle(entidadTInfoBaseDetalle);

                                    arrylist.Clear();
                                }  //Cierra el for (int c = 0; c < Listpordias.Count; c++)
                                if (iSaveOk > 0)
                                {
                                    if (sCodigosCorrectos.Equals(""))
                                    {
                                        sCodigosCorrectos = sCodigo;
                                    }
                                    else
                                    {
                                        sCodigosCorrectos = sCodigosCorrectos + ", " + sCodigo;
                                    }
                                }
                            } //Cierra el if (modelCodigoEntrega.Entidad != null)
                            else
                            {
                                ///EN EL CASO QUE EL CODIGO CARGADO NO EXISTA
                                if (sResultadoCodigoNoExiste.Equals(""))
                                    sResultadoCodigoNoExiste = " Código no encontrado o inactivo: [" + dc.ColumnName.ToString() + "]";
                                else if (!sResultadoCodigoNoExiste.Equals(""))
                                {
                                    sResultadoCodigoNoExiste = sResultadoCodigoNoExiste + ", [" + dc.ColumnName.ToString() + "]";
                                }
                                CodigosErrados++;
                                CodigosCorrectos--;
                            }
                        }  //Cierra bSalir
                        listColuTodo.Clear();
                    }  //Cierra foreach (DataColumn dc in ds.Tables[0].Columns) Recorre columna por coluna el archivo
                    
                    //ELIMINAMOS EL ARCHIVO TEMPORAL DEL SERVIDOR
                    System.IO.File.Delete(pathfinal);

                    if (!sResultadoCodigoNoExiste.Equals("") || !sResultadoCodigoDataErrada.Equals("") || !sResultadoCodigoDataExcedida.Equals(""))
                        return Json("Códigos correctos[" + CodigosCorrectos + "]: " + sCodigosCorrectos + "\n" + "Códigos Incorrectos[" + CodigosErrados + "]: \n" + sResultadoCodigoNoExiste + "\n" + sResultadoCodigoDataErrada + "\n" + sResultadoCodigoDataExcedida);
                    else
                        return Json("Código(s) correcto(s) [" + CodigosCorrectos + "]: " + sCodigosCorrectos); //Exito en el procesamiento... sin errores
                }
                else
                    return Json("Lo sentimos, no se ha procesado la información, favor de seleccionar una empresa primero");
            }
            catch
            {
                return Json("Lo sentimos se ha producido un error al momento de leer los valores de energia");  // (Exception ex) //return Json(ex.ToString());
            }
        }

        #endregion

        #region Otros tabs
        public ActionResult FetchGraphData(int periodo, string codigoER, int empr)
        {
            CodigoEntregaModel modelCodigoEntrega = new CodigoEntregaModel();
            TransferenciaEntregaDetalleModel modelTransferenciaEntregaDetalle = new TransferenciaEntregaDetalleModel();
            CodigoRetiroModel modelCodigoRetiro = new CodigoRetiroModel();
            TransferenciaRetiroDetalleModel modelTransferenciaRetiroDetalle = new TransferenciaRetiroDetalleModel();

            int Emprcodi = empr;
            //obtener el ultimo codigo de la version
            int tramver = 0;
            if (Session["Version"] != null)
            {
                tramver = (int)(Session["Version"]);
            }

            modelCodigoEntrega.Entidad = (new CodigoEntregaAppServicio()).GetByCodigoEntregaCodigo(codigoER);
            if (modelCodigoEntrega.Entidad != null)
            {
                Emprcodi = modelCodigoEntrega.Entidad.EmprCodi;
                modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaEntregaDetalle(Emprcodi, periodo, codigoER, tramver);
                var dataEntrega = modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle;
                return Json(new { dataER = dataEntrega, dataCodigo = codigoER, tipo = "E" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                modelCodigoRetiro.Entidad = (new CodigoRetiroAppServicio()).GetByCodigoRetiroCodigo(codigoER);
                Emprcodi = modelCodigoRetiro.Entidad.EmprCodi;
                modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaRetiroDetalle(Emprcodi, periodo, codigoER, tramver);
                var dataRetiro = modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle;
                return Json(new { dataER = dataRetiro, dataCodigo = codigoER, tipo = "R" }, JsonRequestBehavior.AllowGet);
            }
        }

        //Retirona lista de codigo de retiro y entrega
        public ActionResult getListRetiroEntrega(int periodo, int emprcodi, int version, int barrcodi)
        {
            TransferenciaEntregaModel modelTransferenciaEntrega = new TransferenciaEntregaModel();
            TransferenciaRetiroModel modelTransferenciaRetiro = new TransferenciaRetiroModel();
            //version
            int tramver = version;
            Session["Version"] = tramver;
            modelTransferenciaEntrega.ListaTransferenciaEntrega = (new TransferenciaEntregaRetiroAppServicio()).ListTransferenciaEntrega(emprcodi, periodo, tramver, barrcodi);
            modelTransferenciaRetiro.ListaTransferenciaRetiro = (new TransferenciaEntregaRetiroAppServicio()).ListTransferenciaRetiro(emprcodi, periodo, tramver, barrcodi);

            var dataEntrega = modelTransferenciaEntrega.ListaTransferenciaEntrega;
            var dataRetiro = modelTransferenciaRetiro.ListaTransferenciaRetiro;
            return Json(new { entr = dataEntrega, reti = dataRetiro }, JsonRequestBehavior.AllowGet);
        }

        //Retirona lista de codigo de InfoBase
        public ActionResult getListInfoBase(int periodo, int emprcodi, int version)
        {
            TransferenciaInformacionBaseModel modelTransInfoBase = new TransferenciaInformacionBaseModel();
            TransferenciaEntregaModel modelTransferenciaEntrega = new TransferenciaEntregaModel();
            TransferenciaRetiroModel modelTransferenciaRetiro = new TransferenciaRetiroModel();

            int Emprcodi = emprcodi;
            //version
            int tramver = version;
            Session["Version"] = tramver;
            modelTransInfoBase.ListaInformacionBase = (new TransferenciaInformacionBaseAppServicio()).ListInformacionBase(Emprcodi, periodo, tramver);
            var dataIB = modelTransInfoBase.ListaInformacionBase;
            return Json(new { infobase = dataIB }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FetchGraphDataInfoBase(int periodo, string codigoER, int empr)
        {
            TransferenciaInformacionBaseDetalleModel modelTransferenciaInfoBase = new TransferenciaInformacionBaseDetalleModel();
            int Emprcodi = empr;
            //obtener el ultimo codigo de la version
            int tramver = 0;
            if (Session["Version"] != null)
            {
                tramver = (int)(Session["Version"]);
            }
            modelTransferenciaInfoBase.ListaInformacionBaseDetalle = (new TransferenciaInformacionBaseAppServicio()).BuscarTransferenciaInformacionBaseDetalle(Emprcodi, periodo, codigoER, tramver);
            var dataInfB = modelTransferenciaInfoBase.ListaInformacionBaseDetalle;
            return Json(new { dataIB = dataInfB, dataCodigo = codigoER }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DescargarEntregaRetiro(int PeriCodi, int Version, int EmprCodi, int BarrCodi)
        {
            int indicador = 1;
            try
            {
                PeriodoModel modelPeriodo = new PeriodoModel();
                modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(PeriCodi);

                EmpresaModel modelEmpresa = new EmpresaModel();
                modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(EmprCodi);
                string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                Session["sPrefijoExcel"] = sPrefijoExcel;

                CodigoRetiroModel modelCodigoRetiro = new CodigoRetiroModel();
                EnvioInformacionModel model = new EnvioInformacionModel();
                //Buacamos todos los codigos de entrega y retiro que estan validos para el periodo seleccionado
                model.ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiroVistaTodo(PeriCodi, EmprCodi, BarrCodi);

                string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteFormatoEntregaRetiroExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteFormatoEntregaRetiroExcel);
                }

                int row;
                int row2 = 3;
                int colum = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE ENTREGA Y RETIRO DE " + sPrefijoExcel;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row = 6;
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.CodiEntreRetiCodigo != null) ? item.CodiEntreRetiCodigo : string.Empty;
                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                            ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 6];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    //SEGUNDA HOJA
                    ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                    if (ws2 != null)
                    {
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            string sCodigo = item.CodiEntreRetiCodigo;
                            ws2.Cells[1, colum].Value = sCodigo;
                            TransferenciaEntregaModel modelTransferenciaEntrega = new TransferenciaEntregaModel();
                            modelTransferenciaEntrega.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaEntregaByCodigo(EmprCodi, PeriCodi, Version, sCodigo);
                            if (modelTransferenciaEntrega.Entidad != null)
                            {
                                ws2.Cells[2, colum].Value = modelTransferenciaEntrega.Entidad.TranEntrTipoInformacion;
                                TransferenciaEntregaDetalleModel modelTransferenciaEntregaDetalle = new TransferenciaEntregaDetalleModel();
                                modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaEntregaDetalle(EmprCodi, PeriCodi, sCodigo, Version);
                                int fila = 3;
                                foreach (var dtoTransEntDeta in modelTransferenciaEntregaDetalle.ListaTransferenciaEntregaDetalle)
                                {
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah1;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah2;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah3;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah4;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah5;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah6;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah7;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah8;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah9;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah10;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah11;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah12;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah13;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah14;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah15;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah16;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah17;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah18;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah19;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah20;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah21;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah22;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah23;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah24;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah25;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah26;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah27;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah28;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah29;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah30;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah31;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah32;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah33;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah34;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah35;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah36;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah37;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah38;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah39;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah40;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah41;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah42;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah43;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah44;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah45;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah46;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah47;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah48;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah49;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah50;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah51;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah52;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah53;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah54;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah55;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah56;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah57;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah58;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah59;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah60;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah61;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah62;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah63;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah64;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah65;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah66;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah67;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah68;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah69;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah70;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah71;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah72;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah73;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah74;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah75;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah76;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah77;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah78;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah79;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah80;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah81;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah82;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah83;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah84;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah85;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah86;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah87;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah88;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah89;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah90;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah91;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah92;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah93;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah94;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah95;
                                    ws2.Cells[fila++, colum].Value = dtoTransEntDeta.TranEntrDetah96;
                                }
                            }
                            else
                            {
                                TransferenciaRetiroModel modelTransferenciaRetiro = new TransferenciaRetiroModel();
                                modelTransferenciaRetiro.Entidad = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigo(EmprCodi, PeriCodi, Version, sCodigo);
                                if (modelTransferenciaRetiro.Entidad != null)
                                {
                                    ws2.Cells[2, colum].Value = modelTransferenciaRetiro.Entidad.TranRetiTipoInformacion;
                                    TransferenciaRetiroDetalleModel modelTransferenciaRetiroDetalle = new TransferenciaRetiroDetalleModel();
                                    modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).BuscarTransferenciaRetiroDetalle(EmprCodi, PeriCodi, sCodigo, Version);
                                    int fila = 3;
                                    foreach (var dtoTransRetDeta in modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle)
                                    {
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah1;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah2;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah3;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah4;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah5;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah6;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah7;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah8;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah9;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah10;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah11;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah12;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah13;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah14;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah15;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah16;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah17;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah18;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah19;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah20;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah21;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah22;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah23;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah24;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah25;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah26;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah27;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah28;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah29;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah30;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah31;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah32;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah33;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah34;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah35;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah36;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah37;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah38;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah39;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah40;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah41;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah42;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah43;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah44;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah45;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah46;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah47;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah48;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah49;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah50;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah51;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah52;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah53;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah54;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah55;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah56;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah57;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah58;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah59;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah60;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah61;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah62;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah63;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah64;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah65;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah66;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah67;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah68;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah69;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah70;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah71;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah72;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah73;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah74;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah75;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah76;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah77;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah78;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah79;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah80;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah81;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah82;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah83;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah84;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah85;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah86;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah87;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah88;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah89;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah90;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah91;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah92;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah93;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah94;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah95;
                                        ws2.Cells[fila++, colum].Value = dtoTransRetDeta.TranRetiDetah96;
                                    } //fin foreach (var dtoTransRetDeta in modelTransferenciaRetiroDetalle.ListaTransferenciaRetiroDetalle)
                                } // fin if (modelTransferenciaRetiro.Entidad != null)
                            } //fin del else
                            colum++;
                        }//din foreach (var item in model.ListaEntregReti)
                        //Color de fondo
                        ExcelRange rg = ws2.Cells[1, 1, 2, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ////////////////////////////
                        string sMes = modelPeriodo.Entidad.MesCodi.ToString();
                        if (sMes.Length == 1) sMes = "0" + sMes;
                        var Fecha = "01/" + sMes + "/" + modelPeriodo.Entidad.AnioCodi;
                        var dates = new List<DateTime>();
                        var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                        var dateFin = dateIni.AddMonths(1);

                        dateIni = dateIni.AddMinutes(15);
                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                        {
                            dates.Add(dt);
                        }
                        foreach (var item in dates)
                        {
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
                        rg.AutoFitColumns();
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                    } //fin if (ws2 != null)
                    xlPackage.Save();
                } //fin using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                indicador = 1;
            } // try
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirEntregaRetiro()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + Funcion.NombreReporteFormatoEntregaRetiroExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcel"].ToString() + "_" + Funcion.NombreReporteFormatoEntregaRetiroExcel);
        }

        [HttpPost]
        public JsonResult DescargarInfoBase(int PeriCodi, int Version, int EmprCodi)
        {
            int indicador = 1;
            try
            {
                PeriodoModel modelPeriodo = new PeriodoModel();
                modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(PeriCodi);

                EmpresaModel modelEmpresa = new EmpresaModel();
                modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByIdEmpresa(EmprCodi);
                string sPrefijoExcel = modelEmpresa.Entidad.EmprNombre.ToString() + "_" + modelPeriodo.Entidad.PeriAnioMes.ToString();
                Session["sPrefijoExcel"] = sPrefijoExcel;

                EnvioInformacionModel model = new EnvioInformacionModel();
                //Buacamos todos los codigos de información base que estan validos para el periodo seleccionado
                model.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoInfoBase(PeriCodi, EmprCodi);

                string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInformacionBaseExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteInformacionBaseExcel);
                }

                int row;
                int row2 = 3;
                int colum = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÓDIGOS DE INFORMACIÓN BASE DE " + sPrefijoExcel;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        row = 6;
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.CoInfbCodigo != null) ? item.CoInfbCodigo : string.Empty;
                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                            ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 6];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    //SEGUNDA HOJA
                    ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                    if (ws2 != null)
                    {
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        foreach (var item in model.ListaEntregReti)
                        {
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            string sCodigo = item.CoInfbCodigo;
                            ws2.Cells[1, colum].Value = sCodigo;
                            TransferenciaInformacionBaseModel modelTransferenciaInfoBase = new TransferenciaInformacionBaseModel();
                            modelTransferenciaInfoBase.Entidad = (new TransferenciaInformacionBaseAppServicio()).GetTransferenciaInfoBaseByCodigo(EmprCodi, PeriCodi, Version, sCodigo);
                            if (modelTransferenciaInfoBase.Entidad != null)
                            {
                                ws2.Cells[2, colum].Value = modelTransferenciaInfoBase.Entidad.TinfbTipoInformacion;
                                TransferenciaInformacionBaseDetalleModel modelTransferenciaInfoBaseDetalle = new TransferenciaInformacionBaseDetalleModel();
                                modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle = (new TransferenciaInformacionBaseAppServicio()).BuscarTransferenciaInformacionBaseDetalle(EmprCodi, PeriCodi, sCodigo, Version);
                                int fila = 3;
                                foreach (var dtoTransInfoBaseDeta in modelTransferenciaInfoBaseDetalle.ListaInformacionBaseDetalle)
                                {
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe1;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe2;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe3;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe4;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe5;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe6;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe7;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe8;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe9;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe10;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe11;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe12;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe13;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe14;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe15;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe16;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe17;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe18;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe19;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe20;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe21;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe22;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe23;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe24;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe25;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe26;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe27;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe28;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe29;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe30;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe31;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe32;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe33;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe34;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe35;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe36;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe37;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe38;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe39;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe40;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe41;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe42;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe43;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe44;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe45;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe46;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe47;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe48;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe49;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe50;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe51;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe52;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe53;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe54;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe55;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe56;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe57;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe58;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe59;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe60;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe61;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe62;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe63;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe64;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe65;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe66;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe67;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe68;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe69;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe70;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe71;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe72;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe73;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe74;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe75;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe76;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe77;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe78;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe79;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe80;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe81;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe82;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe83;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe84;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe85;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe86;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe87;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe88;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe89;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe90;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe91;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe92;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe93;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe94;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe95;
                                    ws2.Cells[fila++, colum].Value = dtoTransInfoBaseDeta.TinfbDe96;
                                }
                            }//fin del if
                            colum++;
                        }//din foreach (var item in model.ListaEntregReti)
                        //Color de fondo
                        ExcelRange rg = ws2.Cells[1, 1, 2, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ////////////////////////////
                        string sMes = modelPeriodo.Entidad.MesCodi.ToString();
                        if (sMes.Length == 1) sMes = "0" + sMes;
                        var Fecha = "01/" + sMes + "/" + modelPeriodo.Entidad.AnioCodi;
                        var dates = new List<DateTime>();
                        var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                        var dateFin = dateIni.AddMonths(1);

                        dateIni = dateIni.AddMinutes(15);
                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                        {
                            dates.Add(dt);
                        }
                        foreach (var item in dates)
                        {
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
                        rg.AutoFitColumns();
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                    } //fin if (ws2 != null)
                    xlPackage.Save();
                } //fin using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                indicador = 1;
            } // try
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirInfoBase()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + Funcion.NombreReporteInformacionBaseExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, Session["sPrefijoExcel"].ToString() + "_" + Funcion.NombreReporteInformacionBaseExcel);
        }

        [HttpPost]
        public JsonResult DescargarEnergiaMensual(string sPericodi, int version, Int32? barrcodi, Int32? emprcodi)
        {
            int indicador = 1;
            int pericodi = Int32.Parse(sPericodi);
            //int periodo = 1;
            try
            {
                TransferenciaEntregaDetalleModel model = new TransferenciaEntregaDetalleModel();
                model.ListaTransferenciaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListBalanceEnergiaActiva(pericodi, version, barrcodi, emprcodi);

                string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteResumenEnergiaMensualExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteResumenEnergiaMensualExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "Reporte de Resumen Energía Mensual";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 3].Value = "EMPRESA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "CLIENTE/UNIDAD DE GENERACIÓN";
                        ws.Cells[5, 6].Value = "ENERGIA (MW.h)";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaTransferenciaEntregaDetalle)
                        {
                            ws.Cells[row, 2].Value = (item.NombBarra != null) ? item.NombBarra.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.TranEntrTipoInformacion != null) ? item.TranEntrTipoInformacion.ToString() : string.Empty; //Se utiliza el campo Tranentrtipoinformacion para traer a la empresa
                            ws.Cells[row, 4].Value = (item.TentCodigo != null) ? item.TentCodigo.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = (item.TentdeUserName != null) ? item.TentdeUserName.ToString() : string.Empty;
                            ws.Cells[row, 6].Value = item.Energia;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 6];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        rg = ws.Cells[5, 2, row + 2, 6];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult AbrirEnergiaMensual()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + Funcion.NombreReporteResumenEnergiaMensualExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteResumenEnergiaMensualExcel);
        }

        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<TrnInfoadicionalDTO> listaInfoAdicional = (new TransferenciasAppServicio()).ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();

            List<SeguridadServicio.EmpresaDTO> list = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name, idsEmpresas);

            EmpresaModel model = new EmpresaModel();
            List<EmpresaDTO> lista = new List<EmpresaDTO>();
            foreach (var item in list)
            {
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(item.EMPRCODI);
                lista.Add(dtoEmpresa);

                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        EmpresaDTO dtoEmpresaConcepto = new EmpresaDTO();
                        dtoEmpresaConcepto.EmprCodi = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EmprNombre = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TipoEmprCodi = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        lista.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }

            //model.ListaEmpresas = lista.Where(x => !idsEmpresas.Any(y => x.EmprCodi == y)).OrderBy(x=>x.EmprNombre).ToList();
            model.ListaEmpresas = lista.OrderBy(x => x.EmprNombre).ToList();
            return PartialView(model);
        }


        /// Permite mostrar el resultado de archivo / archivo base procesado
        [HttpPost]
        public ActionResult ResultadoArchivo(string sResultado)
        {
            TempData["Resultado"] = sResultado;
            Session["resultado"] = sResultado;
            return PartialView();
        }

        public JsonResult createPdf()
        {
            int indicador = 1;

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;
                Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 80f, 50f);
                string rutapdf = path + Funcion.NombreConstanciaEnvioInformacionPDF;

                FileInfo newFile = new FileInfo(rutapdf);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(rutapdf);
                }

                FileStream file = new FileStream(rutapdf, FileMode.Create);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, file);

                // step 4
                pdfDoc.Open();

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(response.GetResponseStream());
                gif.Alignment = Element.ALIGN_RIGHT;
                gif.Alignment = iTextSharp.text.Image.PARAGRAPH;
                pdfDoc.Add(gif);

                Paragraph pfecha = new Paragraph(new Chunk(DateTime.Now.ToString("dd/M/yyyy hh:mm:ss tt"), FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                pfecha.Alignment = Element.ALIGN_RIGHT;
                pdfDoc.Add(pfecha);
                pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
                pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

                Paragraph pHeading = new Paragraph(new Chunk("CONSTANCIA DE ENVIO", FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                pHeading.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(pHeading);

                // agregar lineas en blanco
                pdfDoc.Add(Chunk.NEWLINE);
                Chunk sLinea1 = new Chunk("Se ha cargado la siguiente información:", FontFactory.GetFont(FontFactory.HELVETICA, 10));
                Phrase pLinea1 = new Phrase(sLinea1);
                pdfDoc.Add(pLinea1);
                pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco

                string resultado = Session["resultado"].ToString();
                List<string> Lista = resultado.Split('\n').ToList();
                Chunk c = new Chunk(Lista[0], FontFactory.GetFont(FontFactory.HELVETICA, 10));
                pdfDoc.Add(c); //Phrase p1 = new Phrase(c);
                if (Lista.Count > 1)
                {
                    pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
                    Chunk c1 = new Chunk(Lista[1], FontFactory.GetFont(FontFactory.HELVETICA, 10));
                    pdfDoc.Add(c1);
                    pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
                    Chunk c2 = new Chunk("     " + Lista[2], FontFactory.GetFont(FontFactory.HELVETICA, 10));
                    pdfDoc.Add(c2);
                    pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
                    Chunk c3 = new Chunk("     " + Lista[3], FontFactory.GetFont(FontFactory.HELVETICA, 10));
                    pdfDoc.Add(c3);
                    pdfDoc.Add(Chunk.NEWLINE); // agregar lineas en blanco
                    Chunk c4 = new Chunk("     " + Lista[4], FontFactory.GetFont(FontFactory.HELVETICA, 10));
                    pdfDoc.Add(c4);
                }


                // step 5
                pdfDoc.Close();
                //Open the PDF file

                //Process.Start(rutapdf);
                indicador = 1;
            } // try
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }


        [HttpGet]
        public virtual ActionResult abrirpdf(string filename)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + Funcion.NombreConstanciaEnvioInformacionPDF;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppPdf, Funcion.NombreConstanciaEnvioInformacionPDF);
        }

        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            //ASSETEC 20190111
            if (EmprCodi != 0)
            {
                Session["EmprCodi"] = EmprCodi;
            }
            return RedirectToAction("Index");
        }

        /*CODIGO AGREGADO*/
        public ActionResult Demo()
        {
            return View();
        }
        #endregion

        #region Grilla Excel
        /// Permite listar los tipos de información
        public List<TipoInformacionDTO> ListTipoInfo()
        {
            List<TipoInformacionDTO> Lista = new List<TipoInformacionDTO>();
            TipoInformacionDTO dtoTipoInfo = new TipoInformacionDTO();
            dtoTipoInfo.TipoInfoCodi = 0;
            dtoTipoInfo.TipoInfoCodigo = "ER";
            dtoTipoInfo.TipoInfoNombre = "ENTREGAS Y RETIROS";
            Lista.Add(dtoTipoInfo);
            dtoTipoInfo = new TipoInformacionDTO();
            dtoTipoInfo.TipoInfoCodi = 1;
            dtoTipoInfo.TipoInfoCodigo = "IB";
            dtoTipoInfo.TipoInfoNombre = "INFORMACIÓN BASE";
            Lista.Add(dtoTipoInfo);
            dtoTipoInfo = new TipoInformacionDTO();
            dtoTipoInfo.TipoInfoCodi = 2;
            dtoTipoInfo.TipoInfoCodigo = "DM";
            dtoTipoInfo.TipoInfoNombre = "DATOS DE MODELOS";
            Lista.Add(dtoTipoInfo);
            return Lista;
        }

        /// Muestra el codigo del Tipo de Información
        public string TipoInformacion(int tipoinfocodi)
        {
            List<TipoInformacionDTO> ListaTipoInfo = ListTipoInfo();
            return ListaTipoInfo[tipoinfocodi].TipoInfoCodigo;
        }

        /// <summary>
        /// Muestra la grilla excel con los datos enviados trnenvcodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recacodi">Código de la Versión de Recálculo de Energia</param>
        /// <param name="emprcodi">Código de la Empresa</param>
        /// <param name="trnenvcodi">Numero de envio</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int pericodi, int recacodi, int emprcodi, int tipoinfocodi, int trnenvcodi = 0, int trnmodcodi = 0)
        {
            GridExcelModel model = new GridExcelModel();
            try
            {
                EnvioInformacionModel modelEI = new EnvioInformacionModel();
                modelEI.EntidadPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                modelEI.EntidadRecalculo = servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
                model.bGrabar = true;
                if (modelEI.EntidadRecalculo.RecaEstado.Equals("Cerrado"))
                { model.bGrabar = false; }
                else
                {   //Consultamos por la fecha limite para el envio de información
                    //Si todo el proceso sale bien 
                    IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                    string sHora = modelEI.EntidadRecalculo.RecaHoraLimite;
                    double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                    double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                    DateTime dDiaHoraLimite = modelEI.EntidadRecalculo.RecaFechaLimite.AddHours(dHora);
                    dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                    model.sEstado = "ACT"; //Versión para la Liquidación
                    if (dDiaHoraLimite < System.DateTime.Now)
                    {   //YA NO esta en plazo
                        model.sEstado = "INA";
                    }
                }

                modelEI.EntidadEmpresa = servicioEmpresa.GetByIdEmpresa(emprcodi);
                //Buacamos todos los codigos de información base que estan validos para el periodo seleccionado
                string sTipoInformacion = TipoInformacion(tipoinfocodi);
                if (sTipoInformacion == "ER")
                    modelEI.ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiro(pericodi, emprcodi);
                else if (sTipoInformacion == "IB")
                    modelEI.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoInfoBase(pericodi, emprcodi);
                else if (sTipoInformacion == "DM")
                    modelEI.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoModelo(pericodi, emprcodi, trnmodcodi);
                else
                {
                    model.MensajeError = "No se ha relacionado correctamente con el tipo de información";
                    return Json(model);
                }
                if (trnenvcodi > 0)
                {
                    modelEI.EntidadEnvio = servicioTransferencia.GetByIdTrnEnvio(trnenvcodi);
                    if (modelEI.EntidadEnvio != null)
                    {
                        trnmodcodi = modelEI.EntidadEnvio.TrnModCodi;
                        model.TrnEnvFecIns = modelEI.EntidadEnvio.TrnEnvFecIns.ToString("dd/MM/yyyy HH:mm:ss").ToString();
                    }
                }
                else
                {
                    modelEI.EntidadEnvio = this.servicioTransferencia.GetByTrnEnvioIdPeriodoEmpresa(pericodi, recacodi, emprcodi, sTipoInformacion, trnmodcodi);
                    if (modelEI.EntidadEnvio != null)
                    {
                        trnenvcodi = modelEI.EntidadEnvio.TrnEnvCodi;
                        trnmodcodi = modelEI.EntidadEnvio.TrnModCodi;
                        model.TrnEnvFecIns = modelEI.EntidadEnvio.TrnEnvFecIns.ToString("dd/MM/yyyy HH:mm:ss").ToString();
                    }
                }
                int iNroColumnas = modelEI.ListaEntregReti.Count;

                #region Armando la grilla Excel
                //Se arma la matriz de datos
                string[][] data;
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "EMPRESA" }; //Titulos de cada columna
                string[] Cabecera2 = { "BARRA" };
                string[] Cabecera3 = { "CÓDIGO" };
                string[] Cabecera4 = { "FECHA / UNIDAD" };
                int[] widths = { 120 }; //Ancho de cada columna
                string[] itemDato = { "" };
                int iFila = 4;
                //La lista de barras es dinamica
                if (iNroColumnas > 0)
                {
                    Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                    Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                    Array.Resize(ref Cabecera3, Cabecera3.Length + iNroColumnas);
                    Array.Resize(ref Cabecera4, Cabecera4.Length + iNroColumnas);
                    Array.Resize(ref widths, widths.Length + iNroColumnas);
                    Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
                    //Formato de columnas
                    object[] columnas = new object[iNroColumnas + 1];
                    int iColumna = 0;
                    //Formateamos la primera columna
                    columnas[iColumna++] = new
                    {   //INTERVALO DE 15 MINUTOS
                        type = GridExcelModel.TipoTexto,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        readOnly = true,
                    };
                    //Columna de fechas
                    string sMes = modelEI.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var Fecha = "01/" + sMes + "/" + modelEI.EntidadPeriodo.AnioCodi;
                    var dates = new List<DateTime>();
                    var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                    var dateFin = dateIni.AddMonths(1);
                    dateIni = dateIni.AddMinutes(15);
                    for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                    {
                        dates.Add(dt);
                    }
                    data = new string[dates.Count + iFila][]; //Numero de filas de la hoja de datos modelEI.ListaEntregReti.Count()
                    if (modelEI.ListaEntregReti.Count() > 0)
                    {
                        foreach (var day in dates)
                        {
                            //Para cada intervalo de 15 minutos en todos los dias del mes
                            string[] itemFila = { day.ToString("dd/MM/yyyy HH:mm") };
                            Array.Resize(ref itemFila, itemFila.Length + iNroColumnas);
                            for (int i = 1; i < iNroColumnas; i++)
                            {
                                itemFila[i] = "";
                            }
                            data[iFila] = itemFila;
                            iFila++;
                        }
                    }

                    //Registramos las cabeceras de las siguientes columnas
                    foreach (ExportExcelDTO dto in modelEI.ListaEntregReti)
                    {
                        Cabecera1[iColumna] = dto.EmprNomb;
                        Cabecera2[iColumna] = dto.BarrNombBarrTran;
                        if (sTipoInformacion == "ER" || sTipoInformacion == "DM")
                        {
                            Cabecera3[iColumna] = dto.CodiEntreRetiCodigo;
                        }
                        else if (sTipoInformacion == "IB")
                        {
                            Cabecera3[iColumna] = dto.CoInfbCodigo;
                        }
                        Cabecera4[iColumna] = "MWh";
                        widths[iColumna] = 100;
                        itemDato[iColumna] = "";
                        columnas[iColumna] = new
                        {   //R(pu)
                            type = GridExcelModel.TipoNumerico,
                            source = (new List<String>()).ToArray(),
                            strict = false,
                            correctFormat = true,
                            className = "htRight",
                            format = "0,0.000000000000",
                            readOnly = false,
                        };
                        //Traemos la data de los codigos, en caso existiese para almacenarlo en una matriz de codigos
                        if (sTipoInformacion == "ER" || sTipoInformacion == "DM")
                        {
                            if (dto.Tipo.Equals("Entrega"))
                            {
                                modelEI.EntidadEntrega = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaEntregaByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                                if (modelEI.EntidadEntrega != null)
                                {
                                    int iFilCodigo = 4;
                                    int iColCodigo = iColumna;
                                    //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                    modelEI.ListaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaEntrega(modelEI.EntidadEntrega.TranEntrCodi, modelEI.EntidadEntrega.TranEntrVersion);
                                    foreach (TransferenciaEntregaDetalleDTO dtoDetalle in modelEI.ListaEntregaDetalle)
                                    {
                                        for (int k = 1; k <= 96; k++)
                                        {
                                            data[iFilCodigo++][iColCodigo] = dtoDetalle.GetType().GetProperty("TranEntrDetah" + k).GetValue(dtoDetalle, null).ToString();
                                        }
                                    }
                                }
                            }
                            else if (dto.Tipo.Equals("Retiro"))
                            {
                                modelEI.EntidadRetiro = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                                if (modelEI.EntidadRetiro != null) //&& iColumna < 55
                                {
                                    int iFilCodigo = 4;
                                    int iColCodigo = iColumna;
                                    //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                    modelEI.ListaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaRetiro(modelEI.EntidadRetiro.TranRetiCodi, modelEI.EntidadRetiro.TranRetiVersion);
                                    foreach (TransferenciaRetiroDetalleDTO dtoDetalle in modelEI.ListaRetiroDetalle)
                                    {
                                        for (int k = 1; k <= 96; k++)
                                        {
                                            data[iFilCodigo++][iColCodigo] = dtoDetalle.GetType().GetProperty("TranRetiDetah" + k).GetValue(dtoDetalle, null).ToString();
                                        }
                                    }
                                }
                            }
                            else if (dto.Tipo.Equals("Modelo"))
                            {
                                //El generador, a quien pertenece el código
                                int genemprcodi = dto.EmprCodi;
                                modelEI.EntidadRetiro = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigoEnvio(genemprcodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                                if (modelEI.EntidadRetiro != null) //&& iColumna < 55
                                {
                                    int iFilCodigo = 4;
                                    int iColCodigo = iColumna;
                                    //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                    modelEI.ListaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaRetiro(modelEI.EntidadRetiro.TranRetiCodi, modelEI.EntidadRetiro.TranRetiVersion);
                                    foreach (TransferenciaRetiroDetalleDTO dtoDetalle in modelEI.ListaRetiroDetalle)
                                    {
                                        for (int k = 1; k <= 96; k++)
                                        {
                                            data[iFilCodigo++][iColCodigo] = dtoDetalle.GetType().GetProperty("TranRetiDetah" + k).GetValue(dtoDetalle, null).ToString();
                                        }
                                    }
                                }
                            }

                        }
                        else if (sTipoInformacion == "IB")
                        {
                            modelEI.EntidadInformacionBase = (new TransferenciaInformacionBaseAppServicio()).GetTransferenciaInfoBaseByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, dto.CoInfbCodigo);
                            if (modelEI.EntidadInformacionBase != null)
                            {
                                int iFilCodigo = 4;
                                int iColCodigo = iColumna;
                                //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                modelEI.ListaInformacionBaseDetalle = (new TransferenciaInformacionBaseAppServicio()).ListByTransferenciaInformacionBase(modelEI.EntidadInformacionBase.TinfbCodi);
                                foreach (TransferenciaInformacionBaseDetalleDTO dtoDetalle in modelEI.ListaInformacionBaseDetalle)
                                {
                                    for (int k = 1; k <= 96; k++)
                                    {
                                        data[iFilCodigo++][iColCodigo] = dtoDetalle.GetType().GetProperty("TinfbDe" + k).GetValue(dtoDetalle, null).ToString();
                                    }
                                }
                            }
                        }
                        iColumna++;
                    }
                    if (modelEI.ListaEntregReti.Count() > 0)
                    {
                        data[0] = Cabecera1;
                        data[1] = Cabecera2;
                        data[2] = Cabecera3;
                        data[3] = Cabecera4;
                    }
                    else
                    {
                        data = new string[5][];
                        data[0] = Cabecera1;
                        data[1] = Cabecera2;
                        data[2] = Cabecera3;
                        data[3] = Cabecera4;
                        data[iFila] = itemDato;
                    }
                    model.Data = data;
                    model.Widths = widths;
                    model.Columnas = columnas;
                    model.FixedRowsTop = 4;
                    model.FixedColumnsLeft = 1;
                    model.NroColumnas = iNroColumnas;
                    model.Trnenvcodi = trnenvcodi;
                    model.Trnmodcodi = trnmodcodi;
                    model.LimiteMaxEnergia = (decimal)Funcion.dLimiteMaxEnergia;
                }
                #endregion
                //return Json(JsonResult);
                var JsonResult = Json(model);
                JsonResult.MaxJsonLength = Int32.MaxValue;
                return JsonResult;
            }
            catch (Exception e)
            {
                model.MensajeError = e.Message + "<br><br>" + e.StackTrace;
                return Json(model);
            }
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcel(int pericodi, int recacodi, int emprcodi, int tipoinfocodi, int trnmodcodi, string testado)
        {
            int genemprcodi = emprcodi;
            string sTipoDato = "FINAL";
            int trnenvcodi = 0;

            try
            {
                //ARMAMOS LA LISTA DE DATOS DE LA HOJA DE CALCULO
                List<string[]> datos = new List<string[]>();
                for (int i = 0; i < Funcion.iNroGrupos; i++)
                {
                    if (this.ListaDatos[i] != null)
                    {
                        //En orden
                        datos.AddRange(this.ListaDatos[i]);
                    }
                }

                EnvioInformacionModel model = new EnvioInformacionModel();
                if (testado.Equals(""))
                {
                    testado = "ACT";
                    model.EntidadRecalculo = servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
                    if (model.EntidadRecalculo != null)
                    {
                        IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                        string sHora = model.EntidadRecalculo.RecaHoraLimite;
                        double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                        double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                        DateTime dDiaHoraLimite = model.EntidadRecalculo.RecaFechaLimite.AddHours(dHora);
                        dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                        if (dDiaHoraLimite < System.DateTime.Now)
                        {   //YA NO esta en plazo
                            testado = "INA";
                        }
                    }
                }

                string sTipoInformacion = TipoInformacion(tipoinfocodi);
                //Graba Cabecera
                model.EntidadEnvio = new TrnEnvioDTO();
                model.EntidadEnvio.PeriCodi = pericodi;
                model.EntidadEnvio.RecaCodi = recacodi;
                model.EntidadEnvio.EmprCodi = genemprcodi;
                model.EntidadEnvio.TrnEnvTipInf = sTipoInformacion; //"IB"; //ER  / DM
                model.EntidadEnvio.TrnModCodi = trnmodcodi;
                model.EntidadEnvio.TrnEnvPlazo = "S";
                model.EntidadEnvio.TrnEnvLiqVt = "S";
                if (testado.Equals("INA"))
                {
                    model.EntidadEnvio.TrnEnvPlazo = "N";
                    model.EntidadEnvio.TrnEnvLiqVt = "N";
                    sTipoDato = "MEJOR INFORMACIÓN";
                }
                if (sTipoInformacion.Equals("DM"))
                {
                    model.EntidadEnvio.TrnEnvLiqVt = "N";
                    testado = "INA";
                    sTipoDato = "MEJOR INFORMACIÓN";
                }
                model.EntidadEnvio.TrnEnvUseIns = User.Identity.Name;
                model.EntidadEnvio.TrnEnvUseAct = User.Identity.Name;

                if (model.EntidadEnvio.TrnEnvLiqVt.Equals("S"))
                {
                    //Antes de grabar cabecera actualiza los estados de "SI" a "NO"
                    //Todos los envios TrnEnvTipInf:ER/DM (pericodi, recacodi, emprcodi, trnmodcodi) la dtoTrnEnvio.TrnEnvLiqVt <- N
                    this.servicioTransferencia.UpdateByCriteriaTrnEnvio(pericodi, recacodi, genemprcodi, trnmodcodi, model.EntidadEnvio.TrnEnvTipInf, User.Identity.Name);
                }

                //Graba nuevo envio, vacio sin detalle y es el ultimo SI
                trnenvcodi = this.servicioTransferencia.SaveTrnEnvio(model.EntidadEnvio);

                //Preparamos la data
                PeriodoDTO periodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                int nroDias = DateTime.DaysInMonth(periodo.AnioCodi, periodo.MesCodi);
                if (sTipoInformacion == "ER")
                {
                    model = ProcesaEntregaRetiro(model, pericodi, recacodi, emprcodi, trnenvcodi, nroDias, testado, sTipoDato, datos);
                }
                else if (sTipoInformacion == "IB")
                {
                    model = ProcesaInformacionBase(model, pericodi, recacodi, emprcodi, trnenvcodi, nroDias, testado, sTipoDato, datos);
                }
                else if (sTipoInformacion == "DM")
                {
                    model = ProcesaModelo(model, pericodi, recacodi, emprcodi, trnenvcodi, trnmodcodi, nroDias, testado, sTipoDato, datos);
                }
                model.Trnenvcodi = trnenvcodi;
                model.sFecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").ToString();
                model.sPlazo = model.EntidadEnvio.TrnEnvPlazo;

                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.CargaInformacionVTEAExtranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.EnviarDatos;
                objAuditoria.Audproproceso = "Se realizó el envío de información desde extranet - VTEA";
                objAuditoria.Audprodescripcion = "Se realizó el envío con el código " + periodo.PeriNombre + " - cantidad de errores - 0 - " + " - usuario " + User.Identity.Name;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save Auditoria - Grabar Data Extranet VTEA");
                }

                #endregion

                return Json(model);
            }
            catch (Exception e)
            {
                EnvioInformacionModel model = new EnvioInformacionModel();
                model.Trnenvcodi = trnenvcodi;
                model.sError = e.Message; //"-1"
                return Json(model);
            }
            finally
            {
                for (int i = 0; i < Funcion.iNroGrupos; i++)
                {
                    this.ListaDatos[i] = null;
                }
            }
        }

        [HttpPost]
        public JsonResult CargarPrevio(List<string[]> datos, int nroelementos, int cont, int indiceInicio, int indiceFinal)
        {
            try
            {
                if (cont <= Funcion.iNroGrupos && datos != null)
                {
                    List<string[]>[] lista = this.ListaDatos;
                    lista[cont] = datos;
                    this.ListaDatos = lista;
                    int iNroFilas = 0;
                    for (int i = 0; i < Funcion.iNroGrupos; i++)
                    {
                        if (lista[i] != null)
                            iNroFilas += lista[i].Count();

                    }
                    if (iNroFilas == nroelementos)
                    {
                        //Todos los elementos distintos de nulo, salimos para ejecutar GrabarGrillaExcel
                        return Json(1);
                    }
                }
                //Regresar al cliente para incorporar mas registros a los grupos
                return Json(0);
            }
            catch (Exception ex)
            {
                for (int i = 0; i < Funcion.iNroGrupos; i++)
                {
                    this.ListaDatos[i] = null;
                }
                return Json(ex.Message);
            }
        }

        public EnvioInformacionModel ProcesaEntregaRetiro(EnvioInformacionModel model, int pericodi, int recacodi, int emprcodi, int trnenvcodi, int nroDias, string testado, string sTipoDato, List<string[]> datos)
        {
            try
            {
                model.sError = "";
                CodigoEntregaAppServicio servicioTrnCodigoEntrega = new CodigoEntregaAppServicio();
                List<string> erroresValor = new List<string>();
                List<string> erroresDatos = new List<string>();
                List<DatosTransferencia> listData = Funcion.TransformarDataHanson(datos, nroDias, emprcodi, out erroresValor, out erroresDatos, sTipoDato);
                List<DatosTransferencia> resultadoProceso = servicioTrnCodigoEntrega.GrabarEntregaRetiroEnvio(listData, pericodi, recacodi, emprcodi, User.Identity.Name, trnenvcodi, testado);
                List<string> codigosCorrectos = resultadoProceso.Where(x => !string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();
                List<string> codigosErroneos = resultadoProceso.Where(x => string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();

                if (erroresValor.Count > 0 || erroresDatos.Count > 0 || codigosErroneos.Count > 0)
                {
                    model.sError = "Códigos Correctos [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos) + "\n" +
                                "Códigos Incorrectos [" + codigosErroneos.Count + "]: " + string.Join(", ", codigosErroneos) + "\n" +
                                "Códigos con valor superior a 350 MWh [" + erroresValor.Count + "]: " + string.Join(", ", erroresValor) + "\n" +
                                "Códigos con valores erróneos [" + erroresDatos.Count + "]: " + string.Join(", ", erroresDatos) + "\n";
                }
                else
                {
                    model.sMensaje = "Código(s) correcto(s) [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos);
                }
            }
            catch (Exception e)
            {
                model.sError = "Lo sentimos se ha producido un error al momento de leer los valores de energia:" + e.Message;
            }
            return model;
        }

        public EnvioInformacionModel ProcesaInformacionBase(EnvioInformacionModel model, int pericodi, int recacodi, int emprcodi, int trnenvcodi, int nroDias, string testado, string sTipoDato, List<string[]> datos)
        {
            try
            {
                model.sError = "";
                string sResultadoCodigoNoExiste = ""; //Resultado del procesamiento: 1:Exito, sin errores
                string sResultadoCodigoDataErrada = ""; //Resultado del procesamiento de codigos con errores en sus casillas
                string sResultadoCodigoDataExcedida = ""; //Resultado del procesamiento de codigos con errores en sus casillas
                string sCodigosCorrectos = "";
                int CodigosCorrectos = 0;
                int CodigosErrados = 0;

                List<string> erroresValor = new List<string>();
                List<string> erroresDatos = new List<string>();
                List<string> codigosErrorValor = new List<string>();
                List<string> codigosErrorDatos = new List<string>();
                //string[] tipos = { "FINAL", "MEJOR INFORMACIÓN" };

                //Coloca todos los envio de información base en inactivo
                TransferenciaInformacionBaseDTO dtoIB = new TransferenciaInformacionBaseDTO();
                dtoIB.EmprCodi = emprcodi;
                dtoIB.PeriCodi = pericodi;
                dtoIB.TinfbVersion = recacodi;
                dtoIB.TinfbCodi = -1; //Para que aplique el update
                (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBase(dtoIB);

                //Recorrer matriz para grabar detalle
                for (int col = 1; col < datos[0].Count(); col++)
                {   //Por Fila
                    if (datos[2][col] == null)
                        break; //FIN - no existe dato en celda

                    string sCodigo = datos[2][col].ToString();
                    string sTipodato = sTipoDato; // (tipos.Contains(datos[3][col].ToString())) ? datos[3][col].ToString() : "PRELIMINAR";
                    CodigoInfoBaseDTO dtoCodigoInfoBase = (new CodigoInfoBaseAppServicio()).CodigoInfoBaseVigenteByPeriodo(pericodi, sCodigo);
                    if (dtoCodigoInfoBase != null && dtoCodigoInfoBase.EmprCodi == emprcodi)
                    {
                        //Eliminar registro de codigo de información base
                        TransferenciaInformacionBaseDTO dtoTransInfoBase = new TransferenciaInformacionBaseDTO();
                        dtoTransInfoBase.TrnEnvCodi = trnenvcodi;
                        dtoTransInfoBase.CoInfbCodi = dtoCodigoInfoBase.CoInfBCodi;
                        dtoTransInfoBase.EmprCodi = dtoCodigoInfoBase.EmprCodi;
                        dtoTransInfoBase.BarrCodi = dtoCodigoInfoBase.BarrCodi;
                        dtoTransInfoBase.TinfbCodigo = sCodigo;
                        dtoTransInfoBase.EquiCodi = dtoCodigoInfoBase.CentGeneCodi;
                        dtoTransInfoBase.PeriCodi = pericodi;
                        dtoTransInfoBase.TinfbVersion = recacodi;
                        dtoTransInfoBase.TinfbTipoInformacion = sTipodato;
                        dtoTransInfoBase.TinfbEstado = testado;
                        dtoTransInfoBase.TinfbUserName = User.Identity.Name;
                        int iTinfbCodi = (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBase(dtoTransInfoBase);
                        int iSaveOk = 0;
                        bool valorMaximo = true;
                        bool valorError = true;
                        //Recorremos la matriz que se inicia en la fila 4
                        for (int indice = 1; indice <= nroDias; indice++)
                        {
                            decimal suma = 0;
                            TransferenciaInformacionBaseDetalleDTO dtoTransInfoBaseDetalle = new TransferenciaInformacionBaseDetalleDTO();
                            dtoTransInfoBaseDetalle.TinfbCodi = iTinfbCodi;
                            dtoTransInfoBaseDetalle.TinfbDeDia = indice;
                            dtoTransInfoBaseDetalle.TinfbDeVersion = recacodi;
                            dtoTransInfoBaseDetalle.TinfbDeUserName = User.Identity.Name;
                            for (int k = 1; k <= 96; k++)
                            {
                                object valor = datos[k + (indice - 1) * 96 + 3][col]; //3 por que empieza en la fila siguiente
                                decimal dvalor = 0;
                                if (valor != null)
                                {
                                    string sValue = valor.ToString();

                                    if (decimal.TryParse(sValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dvalor))
                                    {
                                        dtoTransInfoBaseDetalle.GetType().GetProperty("TinfbDe" + k).SetValue(dtoTransInfoBaseDetalle, dvalor);
                                        suma = suma + dvalor;
                                        if (dvalor > (decimal)Funcion.dLimiteMaxEnergia)
                                        {
                                            valorMaximo = false;
                                        }
                                    }
                                    else
                                    {
                                        valorError = false;
                                    }
                                }
                            }
                            dtoTransInfoBaseDetalle.TinfbDeSumaDia = suma;
                            dtoTransInfoBaseDetalle.TinfbDePromedioDia = suma / 96;
                            iSaveOk += (new TransferenciaInformacionBaseAppServicio()).SaveOrUpdateTransferenciaInformacionBaseDetalle(dtoTransInfoBaseDetalle);
                        }
                        if (iSaveOk == nroDias)
                        {
                            if (sCodigosCorrectos.Equals(""))
                            {
                                sCodigosCorrectos = sCodigo;
                            }
                            else
                            {
                                sCodigosCorrectos = sCodigosCorrectos + ", " + sCodigo;
                            }
                            CodigosCorrectos++;
                        }
                        if (!valorMaximo)
                        {
                            if (sResultadoCodigoDataExcedida.Equals(""))
                                sResultadoCodigoDataExcedida = " Código con información que excede los 350 MWh: [" + sCodigo + "]";
                            else
                            {
                                sResultadoCodigoDataExcedida = sResultadoCodigoDataExcedida + ", [" + sCodigo + "]";
                            }
                            CodigosErrados++;
                            break;
                        }
                        if (!valorError)
                        {
                            if (sResultadoCodigoDataErrada.Equals(""))
                                sResultadoCodigoDataErrada = " Código con información nula o errada: [" + sCodigo + "]";
                            else
                            {
                                sResultadoCodigoDataErrada = sResultadoCodigoDataErrada + ", [" + sCodigo + "]";
                            }
                            CodigosErrados++;
                            break;
                        }
                    }
                    else
                    {
                        ///EN EL CASO QUE EL CODIGO CARGADO NO EXISTA
                        if (sResultadoCodigoNoExiste.Equals(""))
                            sResultadoCodigoNoExiste = " Código no encontrado o inactivo: [" + sCodigo + "]";
                        else if (!sResultadoCodigoNoExiste.Equals(""))
                        {
                            sResultadoCodigoNoExiste = sResultadoCodigoNoExiste + ", [" + sCodigo + "]";
                        }
                        CodigosErrados++;
                    }
                }
                if (!sResultadoCodigoNoExiste.Equals("") || !sResultadoCodigoDataErrada.Equals("") || !sResultadoCodigoDataExcedida.Equals(""))
                    model.sMensaje = "Códigos correctos[" + CodigosCorrectos + "]: " + sCodigosCorrectos + "\n" + "Códigos Incorrectos[" + CodigosErrados + "]: \n" + sResultadoCodigoNoExiste + "\n" + sResultadoCodigoDataErrada + "\n" + sResultadoCodigoDataExcedida;
                else
                    model.sMensaje = "Código(s) correcto(s) [" + CodigosCorrectos + "]: " + sCodigosCorrectos; //Exito en el procesamiento... sin errores
            }
            catch (Exception e)
            {
                model.sError = "Lo sentimos se ha producido un error al momento de leer los valores de energia:" + e.Message;  // (Exception ex) //return Json(ex.ToString());
            }
            return model;
        }

        public EnvioInformacionModel ProcesaModelo(EnvioInformacionModel model, int pericodi, int recacodi, int emprcodi, int trnenvcodi, int trnmodcodi, int nroDias, string testado, string sTipoDato, List<string[]> datos)
        {
            try
            {
                model.sError = "";
                CodigoEntregaAppServicio servicioTrnCodigoEntrega = new CodigoEntregaAppServicio();
                List<string> erroresValor = new List<string>();
                List<string> erroresDatos = new List<string>();
                List<DatosTransferencia> listData = Funcion.TransformarDataHanson(datos, nroDias, emprcodi, out erroresValor, out erroresDatos, sTipoDato);
                //listData contiene el emprcodi del que envia la data.
                List<string> listaEmpresas = new List<string>();
                for (int i = 0; i < listData.Count; i++)
                {
                    string sCodigo = listData[i].Codigobarra;
                    CodigoRetiroDTO dtoCodigoRetiro = (new CodigoRetiroAppServicio()).GetCodigoRetiroByCodigo(sCodigo);
                    if (dtoCodigoRetiro != null)
                    {
                        listData[i].Emprcodi = dtoCodigoRetiro.EmprCodi; //Se le asigna el correcto Empcodi
                        listaEmpresas.Add(dtoCodigoRetiro.EmprCodi.ToString());
                    }
                }
                listaEmpresas = listaEmpresas.Distinct().ToList();
                //Lista de empresas
                List<DatosTransferencia> resultadoProceso = servicioTrnCodigoEntrega.GrabarModeloEnvio(listData, pericodi, recacodi, string.Join(", ", listaEmpresas), User.Identity.Name, trnenvcodi, trnmodcodi, testado);
                List<string> codigosCorrectos = resultadoProceso.Where(x => !string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();
                List<string> codigosErroneos = resultadoProceso.Where(x => string.IsNullOrEmpty(x.Indbarra)).Select(x => x.Codigobarra).Distinct().ToList();

                if (erroresValor.Count > 0 || erroresDatos.Count > 0 || codigosErroneos.Count > 0)
                {
                    model.sError = "Códigos Correctos [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos) + "\n" +
                                "Códigos Incorrectos [" + codigosErroneos.Count + "]: " + string.Join(", ", codigosErroneos) + "\n" +
                                "Códigos con valor superior a 350 MWh [" + erroresValor.Count + "]: " + string.Join(", ", erroresValor) + "\n" +
                                "Códigos con valores erróneos [" + erroresDatos.Count + "]: " + string.Join(", ", erroresDatos) + "\n";
                }
                else
                {
                    model.sMensaje = "Código(s) correcto(s) [" + codigosCorrectos.Count + "]: " + string.Join(", ", codigosCorrectos);
                }
            }
            catch (Exception e)
            {
                model.sError = "Lo sentimos se ha producido un error al momento de leer los valores de energia:" + e.Message;
            }
            return model;
        }

        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi, int recacodi, int emprcodi, int trnenvcodi, int trnmodcodi, int tipoinfocodi, int formato = 1)
        {
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;
                string file = "";
                string sTipoInformacion = TipoInformacion(tipoinfocodi);
                if (sTipoInformacion == "ER")
                {
                    file = this.servicioTransferencia.GenerarFormatoEntregaRetiro(pericodi, recacodi, emprcodi, trnenvcodi, formato, pathFile, pathLogo);
                }
                else if (sTipoInformacion == "IB")
                {
                    file = this.servicioTransferencia.GenerarFormatoInfoBase(pericodi, recacodi, emprcodi, trnenvcodi, formato, pathFile, pathLogo);
                }
                else if (sTipoInformacion == "DM")
                {
                    file = this.servicioTransferencia.GenerarFormatoModelo(pericodi, recacodi, emprcodi, trnenvcodi, trnmodcodi, formato, pathFile, pathLogo);
                }

                PeriodoDTO periodoDTO = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo = servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);


                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.CargaInformacionVTEAExtranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.BajarFormato;
                objAuditoria.Audproproceso = "Exportación de data en excel extranet vtea";
                objAuditoria.Audprodescripcion = "Se exporta data en excel del periodo " + periodoDTO.PeriNombre + " y revisión " + dtoRecalculo.RecaNombre + " - usuario " + User.Identity.Name;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save Auditoria - Exportar Data Extranet VTEA");
                }

                #endregion

                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Descarga el archivo
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            var bytes = System.IO.File.ReadAllBytes(path); 
            System.IO.File.Delete(path);

            return File(bytes, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            string sNombreArchivo = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Muestra el excel importado en excelweb
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recacodi">Código de la Versión de Recálculo de Energia</param>
        /// <param name="emprcodi">Código de la Empresa</param>
        /// <param name="trnenvcodi">Numero de envio</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarExcelWeb(string sarchivo, int pericodi, int recacodi, int emprcodi, int tipoinfocodi, int trnmodcodi, int trnenvcodi = 0)
        {
            GridExcelModel model = new GridExcelModel();
            try
            {
                string sTipoInformacion = TipoInformacion(tipoinfocodi);
                model.MensajeError = "";
                EnvioInformacionModel modelEI = new EnvioInformacionModel();
                modelEI.EntidadPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                int nroDias = DateTime.DaysInMonth(modelEI.EntidadPeriodo.AnioCodi, modelEI.EntidadPeriodo.MesCodi);
                int nroFilas = nroDias * 96; //96 intervalos
                modelEI.EntidadRecalculo = servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
                model.bGrabar = true;
                if (modelEI.EntidadRecalculo.RecaEstado.Equals("Cerrado"))
                {
                    model.bGrabar = false; //Ya no permite mostrar los botones de importación ni Envio
                }
                else
                {   //Consultamos por la fecha limite para el envio de información
                    //Si todo el proceso sale bien 
                    IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                    string sHora = modelEI.EntidadRecalculo.RecaHoraLimite;
                    double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                    double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                    DateTime dDiaHoraLimite = modelEI.EntidadRecalculo.RecaFechaLimite.AddHours(dHora);
                    dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                    model.sEstado = "ACT"; //Versión para la Liquidación
                    if (dDiaHoraLimite < System.DateTime.Now)
                    {   //YA NO esta en plazo
                        model.sEstado = "INA";
                    }
                }

                modelEI.EntidadEmpresa = servicioEmpresa.GetByIdEmpresa(emprcodi);
                //Buacamos todos los codigos de información base que estan validos para el periodo seleccionado
                if (sTipoInformacion == "ER")
                {
                    modelEI.ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiro(pericodi, emprcodi);
                }
                else if (sTipoInformacion == "IB")
                {
                    modelEI.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoInfoBase(pericodi, emprcodi);
                }
                else if (sTipoInformacion == "DM")
                {
                    modelEI.ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoModelo(pericodi, emprcodi, trnmodcodi);
                }
                if (trnenvcodi > 0)
                {
                    modelEI.EntidadEnvio = servicioTransferencia.GetByIdTrnEnvio(trnenvcodi);
                    model.TrnEnvFecIns = modelEI.EntidadEnvio.TrnEnvFecIns.ToString("dd/MM/yyyy HH:mm:ss").ToString();
                }
                int iNroColumnas = modelEI.ListaEntregReti.Count;

                //Leemos el archivo
                string[][] excel = new string[iNroColumnas][];
                DataSet ds = new DataSet();
                ds = (new Funcion()).GeneraDataset(AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + "/" + sarchivo, 2);
                int iAux = 0;
                model.sCodigoErroneo = "";
                model.sCodigoFaltante = "";
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    if (dc.ColumnName.ToString().Equals("Column1") && iAux == 0)
                    {   //Primera columna
                        continue;
                    }
                    if (dc.ColumnName.ToString().Contains("Column"))
                    {   //Ultima columna que no contiene encabezado
                        break;
                    }
                    //ASSETEC 20200626 - 1.- EVALUAMOS SI LOS CODIGOS EXISTE Y SI ESTAN VIGENTES
                    string sCodigo = dc.ColumnName.ToString();
                    if (sTipoInformacion == "ER" || sTipoInformacion == "DM")
                    {
                        var codigoInCorrecto = modelEI.ListaEntregReti.Where(x => x.CodiEntreRetiCodigo == sCodigo).Select(x => x.CodiEntreRetiCodigo).ToList();
                        if (codigoInCorrecto.Count() == 0)
                        {
                            //Codigo no existe o esta fuera de fecha
                            model.sCodigoErroneo += " [" + sCodigo + "]";
                            continue;
                        }
                    }
                    else if (sTipoInformacion == "IB")
                    {
                        var codigoInCorrecto = modelEI.ListaEntregReti.Where(x => x.CoInfbCodigo == sCodigo).Select(x => x.CoInfbCodigo).ToList();
                        if (codigoInCorrecto.Count() == 0)
                        {
                            //Codigo no existe o esta fuera de fecha
                            model.sCodigoErroneo += " [" + sCodigo + "]";
                            continue;
                        }
                    }
                    /* - - - -  - - - -  - - - -  - - - -  - - - -  - - - -  - - - -  - - - -  - - - -  - - - -  - - - -  - - - -  */
                    string[] sfila = { sCodigo }; //"CÓDIGO" 
                    bool bTipoInfo = false;
                    int iAuxFilas = 0;
                    foreach (DataRow dtRow in ds.Tables[0].Rows)
                    {
                        //Control para que solo lea el numero de filas del mes
                        if (nroFilas < iAuxFilas)
                        {
                            break; // Salir
                        }
                        //A partir de la segunda fila que contiene los datos
                        string sCelda = dtRow[dc].ToString();
                        if (String.IsNullOrEmpty(sCelda) || sCelda.Equals("null"))
                        {
                            break;
                        }
                        else if (!bTipoInfo)
                        {   //Celda que contiene: Final, Preliminar, Mejor Información
                            sCelda = sCelda.Trim().ToUpper();
                            //if (!(sCelda.Equals("FINAL") || sCelda.Equals("MEJOR INFORMACIÓN")))
                            //    sCelda = "PRELIMINAR";
                            //sCelda = "MWh";
                            Array.Resize(ref sfila, sfila.Length + 1);
                            sfila[sfila.Length - 1] = sCelda.Trim();
                            bTipoInfo = true;
                        }
                        else
                        {
                            Array.Resize(ref sfila, sfila.Length + 1);
                            sfila[sfila.Length - 1] = sCelda;
                        }
                        iAuxFilas++;
                    }
                    excel[iAux++] = sfila;
                }
                #region Armando la grilla Excel
                //Se arma la matriz de datos
                string[][] data;
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "EMPRESA" }; //Titulos de cada columna
                string[] Cabecera2 = { "BARRA" };
                string[] Cabecera3 = { "CÓDIGO" };
                string[] Cabecera4 = { "FECHA / UNIDAD" };
                int[] widths = { 120 }; //Ancho de cada columna
                string[] itemDato = { "" };
                int iFila = 4;
                //La lista de barras es dinamica
                if (iNroColumnas > 0)
                {
                    Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                    Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                    Array.Resize(ref Cabecera3, Cabecera3.Length + iNroColumnas);
                    Array.Resize(ref Cabecera4, Cabecera4.Length + iNroColumnas);
                    Array.Resize(ref widths, widths.Length + iNroColumnas);
                    Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
                    //Formato de columnas
                    object[] columnas = new object[iNroColumnas + 1];
                    int iColumna = 0;
                    //Formateamos la primera columna para las fechas 
                    columnas[iColumna++] = new
                    {   //INTERVALO DE 15 MINUTOS
                        type = GridExcelModel.TipoTexto,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        readOnly = true,
                    };
                    //Columna de fechas
                    string sMes = modelEI.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var Fecha = "01/" + sMes + "/" + modelEI.EntidadPeriodo.AnioCodi;
                    var dates = new List<DateTime>();
                    var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                    var dateFin = dateIni.AddMonths(1);
                    dateIni = dateIni.AddMinutes(15);
                    for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                    {
                        dates.Add(dt);
                    }
                    data = new string[dates.Count + iFila][]; //Numero de filas de la hoja de datos modelEI.ListaEntregReti.Count()
                    if (modelEI.ListaEntregReti.Count() > 0)
                    {
                        foreach (var day in dates)
                        {
                            //Para cada intervalo de 15 minutos en todos los dias del mes
                            string[] itemFila = { day.ToString("dd/MM/yyyy HH:mm") };
                            Array.Resize(ref itemFila, itemFila.Length + iNroColumnas);
                            for (int i = 1; i <= iNroColumnas; i++)
                            {
                                itemFila[i] = "";
                            }
                            data[iFila] = itemFila;
                            iFila++;
                        }
                    }
                    //iColumna = 1
                    //Registramos las cabeceras de las siguientes columnas
                    foreach (ExportExcelDTO dto in modelEI.ListaEntregReti)
                    {
                        Cabecera1[iColumna] = dto.EmprNomb;
                        Cabecera2[iColumna] = dto.BarrNombBarrTran;
                        if (sTipoInformacion == "ER")
                            Cabecera3[iColumna] = dto.CodiEntreRetiCodigo;
                        else if (sTipoInformacion == "IB")
                            Cabecera3[iColumna] = dto.CoInfbCodigo;
                        else if (sTipoInformacion == "DM")
                            Cabecera3[iColumna] = dto.CodiEntreRetiCodigo;
                        //ASSETEC 20200626 - 2.- Buscamos el codigo de la lista en el Excel, sino existe, entonces existen códigos faltantes
                        int iColumExcel = -1;
                        for (int iCol = 0; iCol < excel.Count(); iCol++)
                        {
                            if (excel[iCol] != null)
                            {
                                if (Cabecera3[iColumna].ToString() == excel[iCol][0].ToString())
                                {
                                    //Código encontrado - 3.- De esta forma se puede subir la información sin importar el orden
                                    iColumExcel = iCol;
                                    break; //salimos
                                }
                            }
                        }
                        /***********************************************************************************/
                        Cabecera4[iColumna] = "MWh";
                        widths[iColumna] = 100;
                        itemDato[iColumna] = "";
                        columnas[iColumna] = new
                        {   //R(pu)
                            type = GridExcelModel.TipoNumerico,
                            source = (new List<String>()).ToArray(),
                            strict = false,
                            correctFormat = true,
                            className = "htRight",
                            format = "0,0.000000000000",
                            readOnly = false,
                        };
                        if (iColumExcel == -1)
                        {
                            //2.- No existe información de este código en el excel reportado
                            model.sCodigoFaltante += " [" + Cabecera3[iColumna].ToString() + "]";
                            iColumna++;
                            continue;
                        }
                        int iFilCodigo = 4;
                        //Traemos la data de los codigos, en caso existiese para almacenarlo en una matriz de codigos
                        for (int iFil = 2; iFil < excel[iColumExcel].Count(); iFil++)
                        {
                            data[iFilCodigo++][iColumna] = excel[iColumExcel][iFil];
                        }
                        iColumna++;
                    }
                    if (modelEI.ListaEntregReti.Count() > 0)
                    {
                        data[0] = Cabecera1;
                        data[1] = Cabecera2;
                        data[2] = Cabecera3;
                        data[3] = Cabecera4;
                    }
                    else
                    {
                        data = new string[5][];
                        data[0] = Cabecera1;
                        data[1] = Cabecera2;
                        data[2] = Cabecera3;
                        data[3] = Cabecera4;
                        data[iFila] = itemDato;
                    }
                    model.Data = data;
                    model.Widths = widths;
                    model.Columnas = columnas;
                    model.FixedRowsTop = 4;
                    model.FixedColumnsLeft = 1;
                    model.NroColumnas = iNroColumnas;
                    model.Trnenvcodi = trnenvcodi;
                    model.LimiteMaxEnergia = (decimal)Funcion.dLimiteMaxEnergia;
                }
                #endregion
                //return Json(JsonResult);
                var JsonResult = Json(model);
                JsonResult.MaxJsonLength = Int32.MaxValue;

                System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + "/" + sarchivo);

                return JsonResult;
            }
            catch (Exception e)
            {
                model.MensajeError = e.Message + "<br><br>" + e.StackTrace;
                return Json(model);
            }
        }

        /// Permite listar los envios que realizo en la empresa
        [HttpPost]
        public ActionResult ListaEnvios(int pericodi, int recacodi, int emprcodi, int tipoinfocodi, int trnmodcodi)
        {
            EnvioInformacionModel model = new EnvioInformacionModel();
            string sTipoInformacion = TipoInformacion(tipoinfocodi);
            model.EntidadRecalculo = this.servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
            model.ListaEnvios = this.servicioTransferencia.ListarTrnEnvio(pericodi, recacodi, emprcodi, sTipoInformacion, trnmodcodi);
            return PartialView(model);
        }
        #endregion
    }
}