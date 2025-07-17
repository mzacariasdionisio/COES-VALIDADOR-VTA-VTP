using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using log4net;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System.Net;
using OfficeOpenXml.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Linq;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class ComparacionCoincidenteController : BaseController
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsultaDatosHistoricosController));
        private static string NombreControlador = "ComparacionCoincidenteController";

        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();

        public ActionResult Index(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            PeajeEgresoMinfoModel model = new PeajeEgresoMinfoModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            var periodoCorte = Convert.ToInt32(ConfigurationManager.AppSettings[Funcion.periodoCorte] ?? "0" );
            model.ListaPeriodos = model.ListaPeriodos.Where(x=>x.PeriCodi>= periodoCorte).ToList();
           
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
            }

            if (pericodi > 0)
            {

                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                {
                    recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi;
                }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            }
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int NroPagina, int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo, int pericodi, int recpotcodi)
        {
            ViewBag.NroPagina = NroPagina;

            CodigoRetiroEquivalenciaModel model = new CodigoRetiroEquivalenciaModel();
            model.ListaCodigoRetiroRelacion = new CodigoRetiroRelacionEquivalenciasAppServicio().ListarRelacionCodigoRetiros(NroPagina, Funcion.PageSize,
                genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum, tipConCodi, tipUsuCodi, estado, codigo, pericodi, recpotcodi, 1);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Paginado(int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo, int paginadoActual = 1)
        {
            ViewBag.paginadoActual = paginadoActual;
            CodigoRetiroModel model = new CodigoRetiroModel();
            model.IndicadorPagina = false;
            model.NroRegistros = new CodigoRetiroRelacionEquivalenciasAppServicio().TotalRecordsRelacionCodigoRetiros(genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum, tipConCodi, tipUsuCodi, estado, codigo);
            if (model.NroRegistros > Funcion.NroPageShow)
            {
                int pageSize = Funcion.PageSizeCodigoEntrega;
                int nroPaginas = (model.NroRegistros % pageSize == 0) ? model.NroRegistros / pageSize : model.NroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Funcion.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcel(int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo, int pericodi, int recpotcodi)
        {
            int indicador = -1;
            string filePath = "";
            FileInfo newFile;
            try
            {
                CodigoRetiroEquivalenciaModel model = new CodigoRetiroEquivalenciaModel();
                model.ListaCodigoRetiroRelacion = new CodigoRetiroRelacionEquivalenciasAppServicio().ListarRelacionCodigoRetiros(1, 5000,
                    genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum, tipConCodi, tipUsuCodi, estado, codigo, pericodi, recpotcodi, 1);

                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                filePath = Funcion.NombreReporteComparacionCoincidenteExcel;

                newFile = new FileInfo(path + Funcion.NombreReporteComparacionCoincidenteExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteComparacionCoincidenteExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {

                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE COMPARACIÓN COINCIDENTE DE CÓDIGOS VTP Y VTEA ";
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;

                        int row = 5;
                        ws.Cells[row, 2].Value = "CÓDIGO VTEA";
                        rg = ws.Cells[row, 2, row, 7];
                        rg.Merge = true;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF6600"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ws.Cells[row, 8].Value = "CÓDIGO VTP";
                        rg = ws.Cells[row, 8, row, 13];
                        rg.Merge = true;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#339900"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;


                        row = 6;
                        //CABECERA DE TABLA
                        ws.Cells[row, 2].Value = "EMPRESA";
                        ws.Cells[row, 3].Value = "CLIENTE";
                        ws.Cells[row, 4].Value = "CONTRATO";
                        ws.Cells[row, 5].Value = "TIPO USUARIO";
                        ws.Cells[row, 6].Value = "BARRA";
                        ws.Cells[row, 7].Value = "CÓDIGO";
                        ws.Cells[row, 8].Value = "EMPRESA";
                        ws.Cells[row, 9].Value = "CLIENTE";
                        ws.Cells[row, 10].Value = "CONTRATO";
                        ws.Cells[row, 11].Value = "TIPO USUARIO";
                        ws.Cells[row, 12].Value = "BARRA";
                        ws.Cells[row, 13].Value = "CÓDIGO";
                        ws.Cells[row, 14].Value = "% VAR MÁXIMO.";
                        ws.Cells[row, 15].Value = "% VAR CALCULADO.";
                        ws.Cells[row, 16].Value = "Potencia VTP.";
                        ws.Cells[row, 17].Value = "Energía VTEA.";
                        ws.Cells[row, 18].Value = "Diferencia VTP - VTEA.";

                        rg = ws.Cells[row, 2, row, 18];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        row = 7;
                        int rowInit = 0;
                        foreach (var item in model.ListaCodigoRetiroRelacion)
                        {
                            rowInit = row;
                            ws.Cells[row, 14].Value = (item.Retrelvari != null) ? item.Retrelvari.ToString() : "0";
                            ws.Cells[row, 15].Value = (item.PorcentajeVariacionCalculado != null) ? item.PorcentajeVariacionCalculado.ToString() : "0";
                            ws.Cells[row, 16].Value = item.PotenciaVtp.ToString();
                            ws.Cells[row, 17].Value = item.EnergiaVtea.ToString();
                            ws.Cells[row, 18].Value = item.DiferenciaVteaVtp.ToString();
                            foreach (var item2 in item.ListarRelacion)
                            {
                                ws.Cells[row, 2].Value = (item2.Genemprnombvtea != null) ? item2.Genemprnombvtea : string.Empty;
                                ws.Cells[row, 3].Value = (item2.Cliemprnombvtea != null) ? item2.Cliemprnombvtea : string.Empty;
                                ws.Cells[row, 4].Value = (item2.Tipocontratovtea != null) ? item2.Tipocontratovtea : string.Empty;
                                ws.Cells[row, 5].Value = (item2.Tipousuariovtea != null) ? item2.Tipousuariovtea : string.Empty;
                                ws.Cells[row, 6].Value = (item2.Barrnombvtea != null) ? item2.Barrnombvtea : string.Empty;
                                ws.Cells[row, 7].Value = (item2.Codigovtea != null) ? item2.Codigovtea : string.Empty;

                                ws.Cells[row, 8].Value = (item2.Genemprnombvtp != null) ? item2.Genemprnombvtp : string.Empty;
                                ws.Cells[row, 9].Value = (item2.Cliemprnombvtp != null) ? item2.Cliemprnombvtp : string.Empty;
                                ws.Cells[row, 10].Value = (item2.Tipocontratovtp != null) ? item2.Tipocontratovtp : string.Empty;
                                ws.Cells[row, 11].Value = (item2.Tipousuariovtp != null) ? item2.Tipousuariovtp : string.Empty;
                                ws.Cells[row, 12].Value = (item2.Barrnombvtp != null) ? item2.Barrnombvtp : string.Empty;
                                ws.Cells[row, 13].Value = (item2.Codigovtp != null) ? item2.Codigovtp : string.Empty;

                                //Border por celda

                                row++;
                            }

                            rg = ws.Cells[rowInit, 2, row - 1, 18];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            rg = ws.Cells[rowInit, 14, row - 1, 14];
                            rg.Merge = true;
                            rg = ws.Cells[rowInit, 15, row - 1, 15];
                            rg.Merge = true;
                            rg = ws.Cells[rowInit, 16, row - 1, 16];
                            rg.Merge = true;
                            rg = ws.Cells[rowInit, 17, row - 1, 17];
                            rg.Merge = true;
                            rg = ws.Cells[rowInit, 18, row - 1, 18];
                            rg.Merge = true;

                        }

                        rg = ws.Cells[6, 2, row, 18];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create("http://www.coes.org.pe/wcoes/images/logocoes.png");
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);

                        xlPackage.Save();
                    }
                }
                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
            }

            return Json(filePath);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcel(string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + file;
            return File(path, Constantes.AppExcel, sFecha + "_" + file);
        }
    }
}
