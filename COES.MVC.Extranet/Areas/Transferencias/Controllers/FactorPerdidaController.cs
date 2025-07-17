using COES.MVC.Extranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.MVC.Extranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Collections;
using OfficeOpenXml;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    public class FactorPerdidaController : Controller
    {
        // GET: /transferencias/factorperdida/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            int iPeriodo = 0;
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodo = (new PeriodoAppServicio()).ListarByEstadoPublicarCerrado();
            if (modelPeriodo.ListaPeriodo.Count > 0)
                iPeriodo = modelPeriodo.ListaPeriodo[0].PeriCodi;
            BarraModel modelBarraE = new BarraModel();
            modelBarraE.ListaBarras = (new BarraAppServicio()).ListBarrasTransferenciaByReporte();

            TempData["PericodigoGraf"] = new SelectList(modelPeriodo.ListaPeriodo, "PERICODI", "PERINOMBRE");
            TempData["PericodigoExcel"] = new SelectList(modelPeriodo.ListaPeriodo, "PERICODI", "PERINOMBRE");
            TempData["BarrcodigoExcel"] = modelBarraE;
            return View();
        }

        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculos = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            return Json(modelRecalculo.ListaRecalculos);
        }

        public ActionResult getListCostoMarginal(int periodo, int vers)
        {
            CostoMarginalModel CosMararginalModel = new CostoMarginalModel();
            CosMararginalModel.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByReporte(periodo, vers);
            var listBarr = CosMararginalModel.ListaCostoMarginal;
            return Json(new { dataBar = listBarr }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FetchGraphData(int periodo, string barcodi)
        {
            CostoMarginalModel CosMararginalModel = new CostoMarginalModel();
            CosMararginalModel.ListaCostoMarginal = (new CostoMarginalAppServicio()).BuscarCostoMarginal(periodo, barcodi);
            var dataBarProm = CosMararginalModel.ListaCostoMarginal;
            var nombreBar = dataBarProm[0].CosMarBarraTransferencia.ToString();
            return Json(new { dataProm = dataBarProm, dataNomb = nombreBar }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerarCostoMarginal(string sPericodi, int vers)
        {
            int iResultado = 1;
            Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(Int32.Parse(sPericodi));
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                CostoMarginalModel model = new CostoMarginalModel();
                model.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByReporte(iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalExcel);
                }

                //Agrega    mos el intervalo de fechas
                string sMes = iMesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var Fecha = "01/" + sMes + "/" + iAnioCodi;
                var dates = new List<DateTime>();
                var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                var dateFin = dateIni.AddMonths(1);

                dateIni = dateIni.AddMinutes(15);
                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                {
                    dates.Add(dt);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COSTOMARGINAL");
                    if (ws != null)
                    {
                        ws.Cells[1, 1].Value = "S/./kWh";

                        //Agregamos la primera columna
                        int row = 2;
                        foreach (var item in dates)
                        {
                            ws.Cells[row, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row++;
                        }
                        ExcelRange rg = ws.Cells[1, 1, row - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Agreamos el resto del archivo
                        int colum = 2;
                        foreach (var item in model.ListaCostoMarginal)
                        {
                            row = 1;
                            //Agregamos la cabecera de la columna
                            ws.Cells[row, colum].Value = item.CosMarBarraTransferencia;
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            CostoMarginalModel modelDetalle = new CostoMarginalModel();
                            modelDetalle.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(item.BarrCodi, iPeriCodi, iVersion);

                            foreach (var item1 in modelDetalle.ListaCostoMarginal)
                            {
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar1 != null) ? item1.CosMar1 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar2 != null) ? item1.CosMar2 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar3 != null) ? item1.CosMar3 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar4 != null) ? item1.CosMar4 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar5 != null) ? item1.CosMar5 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar6 != null) ? item1.CosMar6 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar7 != null) ? item1.CosMar7 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar8 != null) ? item1.CosMar8 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar9 != null) ? item1.CosMar9 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar10 != null) ? item1.CosMar10 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar11 != null) ? item1.CosMar11 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar12 != null) ? item1.CosMar12 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar13 != null) ? item1.CosMar13 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar14 != null) ? item1.CosMar14 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar15 != null) ? item1.CosMar15 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar16 != null) ? item1.CosMar16 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar17 != null) ? item1.CosMar17 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar18 != null) ? item1.CosMar18 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar19 != null) ? item1.CosMar19 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar20 != null) ? item1.CosMar20 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar21 != null) ? item1.CosMar21 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar22 != null) ? item1.CosMar22 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar23 != null) ? item1.CosMar23 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar24 != null) ? item1.CosMar24 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar25 != null) ? item1.CosMar25 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar26 != null) ? item1.CosMar26 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar27 != null) ? item1.CosMar27 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar28 != null) ? item1.CosMar28 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar29 != null) ? item1.CosMar29 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar30 != null) ? item1.CosMar30 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar31 != null) ? item1.CosMar31 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar32 != null) ? item1.CosMar32 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar33 != null) ? item1.CosMar33 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar34 != null) ? item1.CosMar34 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar35 != null) ? item1.CosMar35 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar36 != null) ? item1.CosMar36 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar37 != null) ? item1.CosMar37 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar38 != null) ? item1.CosMar38 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar39 != null) ? item1.CosMar39 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar40 != null) ? item1.CosMar40 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar41 != null) ? item1.CosMar41 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar42 != null) ? item1.CosMar42 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar43 != null) ? item1.CosMar43 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar44 != null) ? item1.CosMar44 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar45 != null) ? item1.CosMar45 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar46 != null) ? item1.CosMar46 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar47 != null) ? item1.CosMar47 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar48 != null) ? item1.CosMar48 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar49 != null) ? item1.CosMar49 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar50 != null) ? item1.CosMar50 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar51 != null) ? item1.CosMar51 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar52 != null) ? item1.CosMar52 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar53 != null) ? item1.CosMar53 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar54 != null) ? item1.CosMar54 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar55 != null) ? item1.CosMar55 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar56 != null) ? item1.CosMar56 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar57 != null) ? item1.CosMar57 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar58 != null) ? item1.CosMar58 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar59 != null) ? item1.CosMar59 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar60 != null) ? item1.CosMar60 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar61 != null) ? item1.CosMar61 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar62 != null) ? item1.CosMar62 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar63 != null) ? item1.CosMar63 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar64 != null) ? item1.CosMar64 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar65 != null) ? item1.CosMar65 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar66 != null) ? item1.CosMar66 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar67 != null) ? item1.CosMar67 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar68 != null) ? item1.CosMar68 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar69 != null) ? item1.CosMar69 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar70 != null) ? item1.CosMar70 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar71 != null) ? item1.CosMar71 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar72 != null) ? item1.CosMar72 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar73 != null) ? item1.CosMar73 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar74 != null) ? item1.CosMar74 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar75 != null) ? item1.CosMar75 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar76 != null) ? item1.CosMar76 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar77 != null) ? item1.CosMar77 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar78 != null) ? item1.CosMar78 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar79 != null) ? item1.CosMar79 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar80 != null) ? item1.CosMar80 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar81 != null) ? item1.CosMar81 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar82 != null) ? item1.CosMar82 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar83 != null) ? item1.CosMar83 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar84 != null) ? item1.CosMar84 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar85 != null) ? item1.CosMar85 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar86 != null) ? item1.CosMar86 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar87 != null) ? item1.CosMar87 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar88 != null) ? item1.CosMar88 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar89 != null) ? item1.CosMar89 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar90 != null) ? item1.CosMar90 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar91 != null) ? item1.CosMar91 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar92 != null) ? item1.CosMar92 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar93 != null) ? item1.CosMar93 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar94 != null) ? item1.CosMar94 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar95 != null) ? item1.CosMar95 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar96 != null) ? item1.CosMar96 : Decimal.Zero;
                            }
                            colum++;
                        }
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.AutoFitColumns();//Ajustar columnas
                        //Cabecera
                        rg = ws.Cells[1, 1, 1, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        //Border por celda
                        rg = ws.Cells[1, 1, row, colum - 1];
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
                iResultado = 1;
            }
            catch
            {
                iResultado = -1;
            }
            return Json(iResultado);
        }

        [HttpGet]
        public virtual ActionResult AbrirCostoMarginal()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCostoMarginalExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCostoMarginalExcel);
        }

        [HttpPost]
        public JsonResult GenerarCMBarra(string sPericodi, string sBarrcodi, int vers)
        {
            int iResultado = 1;
            Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(Int32.Parse(sPericodi));
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalExcel);
                }

                //Agregamos el intervalo de fechas
                string sMes = iMesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var Fecha = "01/" + sMes + "/" + iAnioCodi;
                var dates = new List<DateTime>();
                var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                var dateFin = dateIni.AddMonths(1);

                dateIni = dateIni.AddMinutes(15);
                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                {
                    dates.Add(dt);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COSTOMARGINAL");
                    if (ws != null)
                    {
                        ws.Cells[1, 1].Value = "S/./kWh";

                        //Agregamos la primera columna
                        int row = 2;
                        foreach (var item in dates)
                        {
                            ws.Cells[row, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row++;
                        }
                        ExcelRange rg = ws.Cells[1, 1, row - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Agreamos el resto del archivo
                        int colum = 2;
                        BarraModel modelBarra = new BarraModel();
                        modelBarra.Entidad = (new BarraAppServicio()).GetByIdBarra(Int32.Parse(sBarrcodi));

                        if (modelBarra.Entidad != null)
                        {
                            row = 1;
                            //Agregamos la cabecera de la columna
                            ws.Cells[row, colum].Value = modelBarra.Entidad.BarrNombBarrTran;
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            CostoMarginalModel modelDetalle = new CostoMarginalModel();
                            modelDetalle.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(modelBarra.Entidad.BarrCodi, iPeriCodi, iVersion);

                            foreach (var item1 in modelDetalle.ListaCostoMarginal)
                            {
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar1 != null) ? item1.CosMar1 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar2 != null) ? item1.CosMar2 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar3 != null) ? item1.CosMar3 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar4 != null) ? item1.CosMar4 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar5 != null) ? item1.CosMar5 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar6 != null) ? item1.CosMar6 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar7 != null) ? item1.CosMar7 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar8 != null) ? item1.CosMar8 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar9 != null) ? item1.CosMar9 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar10 != null) ? item1.CosMar10 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar11 != null) ? item1.CosMar11 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar12 != null) ? item1.CosMar12 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar13 != null) ? item1.CosMar13 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar14 != null) ? item1.CosMar14 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar15 != null) ? item1.CosMar15 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar16 != null) ? item1.CosMar16 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar17 != null) ? item1.CosMar17 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar18 != null) ? item1.CosMar18 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar19 != null) ? item1.CosMar19 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar20 != null) ? item1.CosMar20 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar21 != null) ? item1.CosMar21 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar22 != null) ? item1.CosMar22 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar23 != null) ? item1.CosMar23 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar24 != null) ? item1.CosMar24 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar25 != null) ? item1.CosMar25 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar26 != null) ? item1.CosMar26 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar27 != null) ? item1.CosMar27 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar28 != null) ? item1.CosMar28 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar29 != null) ? item1.CosMar29 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar30 != null) ? item1.CosMar30 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar31 != null) ? item1.CosMar31 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar32 != null) ? item1.CosMar32 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar33 != null) ? item1.CosMar33 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar34 != null) ? item1.CosMar34 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar35 != null) ? item1.CosMar35 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar36 != null) ? item1.CosMar36 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar37 != null) ? item1.CosMar37 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar38 != null) ? item1.CosMar38 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar39 != null) ? item1.CosMar39 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar40 != null) ? item1.CosMar40 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar41 != null) ? item1.CosMar41 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar42 != null) ? item1.CosMar42 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar43 != null) ? item1.CosMar43 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar44 != null) ? item1.CosMar44 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar45 != null) ? item1.CosMar45 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar46 != null) ? item1.CosMar46 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar47 != null) ? item1.CosMar47 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar48 != null) ? item1.CosMar48 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar49 != null) ? item1.CosMar49 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar50 != null) ? item1.CosMar50 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar51 != null) ? item1.CosMar51 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar52 != null) ? item1.CosMar52 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar53 != null) ? item1.CosMar53 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar54 != null) ? item1.CosMar54 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar55 != null) ? item1.CosMar55 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar56 != null) ? item1.CosMar56 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar57 != null) ? item1.CosMar57 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar58 != null) ? item1.CosMar58 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar59 != null) ? item1.CosMar59 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar60 != null) ? item1.CosMar60 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar61 != null) ? item1.CosMar61 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar62 != null) ? item1.CosMar62 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar63 != null) ? item1.CosMar63 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar64 != null) ? item1.CosMar64 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar65 != null) ? item1.CosMar65 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar66 != null) ? item1.CosMar66 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar67 != null) ? item1.CosMar67 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar68 != null) ? item1.CosMar68 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar69 != null) ? item1.CosMar69 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar70 != null) ? item1.CosMar70 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar71 != null) ? item1.CosMar71 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar72 != null) ? item1.CosMar72 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar73 != null) ? item1.CosMar73 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar74 != null) ? item1.CosMar74 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar75 != null) ? item1.CosMar75 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar76 != null) ? item1.CosMar76 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar77 != null) ? item1.CosMar77 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar78 != null) ? item1.CosMar78 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar79 != null) ? item1.CosMar79 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar80 != null) ? item1.CosMar80 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar81 != null) ? item1.CosMar81 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar82 != null) ? item1.CosMar82 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar83 != null) ? item1.CosMar83 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar84 != null) ? item1.CosMar84 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar85 != null) ? item1.CosMar85 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar86 != null) ? item1.CosMar86 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar87 != null) ? item1.CosMar87 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar88 != null) ? item1.CosMar88 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar89 != null) ? item1.CosMar89 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar90 != null) ? item1.CosMar90 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar91 != null) ? item1.CosMar91 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar92 != null) ? item1.CosMar92 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar93 != null) ? item1.CosMar93 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar94 != null) ? item1.CosMar94 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar95 != null) ? item1.CosMar95 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar96 != null) ? item1.CosMar96 : Decimal.Zero;
                            }
                            colum++;
                        }
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.AutoFitColumns();//Ajustar columnas
                        //Cabecera
                        rg = ws.Cells[1, 1, 1, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        //Border por celda
                        rg = ws.Cells[1, 1, row, colum - 1];
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
                iResultado = 1;
            }
            catch
            {
                iResultado = -1;
            }
            return Json(iResultado);
        }

        [HttpGet]
        public virtual ActionResult AbrirCMBarra()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCostoMarginalExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCostoMarginalExcel);
        }

        [HttpPost]
        public JsonResult GenerarPorBarra(string sPericodi, string sBarrcodi, int vers)
        {
            int iResultado = 1;
            Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(Int32.Parse(sPericodi));
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;
            int iDiasMes = DateTime.DaysInMonth(iAnioCodi, iMesCodi); // Extrae el numero de dias en el mes

            BarraModel modelBarra = new BarraModel();
            modelBarra.Entidad = (new BarraAppServicio()).GetByIdBarra(Int32.Parse(sBarrcodi));
            int iBarrCodi = modelBarra.Entidad.BarrCodi;
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                CostoMarginalModel model = new CostoMarginalModel();
                model.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(iBarrCodi, iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalBarraExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalBarraExcel);
                }

                //Agregamos el intervalo de fechas
                string sMes = iMesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var Fecha = "01/" + sMes + "/" + iAnioCodi;
                var dates = new List<DateTime>();
                var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                var dateFin = dateIni.AddDays(1);

                dateIni = dateIni.AddMinutes(15);
                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                {
                    dates.Add(dt);
                }

                int row;
                int colum = 2;
                int iDia = 1;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COSTOMARGINAL");
                    if (ws != null)
                    {
                        ws.Cells[1, 1].Value = modelBarra.Entidad.BarrNombBarrTran; //Nombre de la barra de transferencia
                        row = 2;
                        foreach (var item in dates)
                        {
                            ws.Cells[row, 1].Value = item.ToString("HH:mm:ss");
                            row++;
                        }
                        ExcelRange rg = ws.Cells[1, 1, row - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        foreach (var item1 in model.ListaCostoMarginal)
                        {
                            row = 1;
                            ws.Cells[row, colum].Value = iDia.ToString();
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            ws.Cells[row, colum].Style.Numberformat.Format = "0";
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar1 != null) ? item1.CosMar1 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar2 != null) ? item1.CosMar2 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar3 != null) ? item1.CosMar3 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar4 != null) ? item1.CosMar4 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar5 != null) ? item1.CosMar5 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar6 != null) ? item1.CosMar6 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar7 != null) ? item1.CosMar7 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar8 != null) ? item1.CosMar8 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar9 != null) ? item1.CosMar9 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar10 != null) ? item1.CosMar10 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar11 != null) ? item1.CosMar11 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar12 != null) ? item1.CosMar12 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar13 != null) ? item1.CosMar13 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar14 != null) ? item1.CosMar14 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar15 != null) ? item1.CosMar15 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar16 != null) ? item1.CosMar16 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar17 != null) ? item1.CosMar17 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar18 != null) ? item1.CosMar18 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar19 != null) ? item1.CosMar19 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar20 != null) ? item1.CosMar20 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar21 != null) ? item1.CosMar21 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar22 != null) ? item1.CosMar22 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar23 != null) ? item1.CosMar23 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar24 != null) ? item1.CosMar24 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar25 != null) ? item1.CosMar25 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar26 != null) ? item1.CosMar26 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar27 != null) ? item1.CosMar27 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar28 != null) ? item1.CosMar28 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar29 != null) ? item1.CosMar29 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar30 != null) ? item1.CosMar30 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar31 != null) ? item1.CosMar31 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar32 != null) ? item1.CosMar32 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar33 != null) ? item1.CosMar33 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar34 != null) ? item1.CosMar34 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar35 != null) ? item1.CosMar35 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar36 != null) ? item1.CosMar36 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar37 != null) ? item1.CosMar37 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar38 != null) ? item1.CosMar38 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar39 != null) ? item1.CosMar39 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar40 != null) ? item1.CosMar40 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar41 != null) ? item1.CosMar41 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar42 != null) ? item1.CosMar42 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar43 != null) ? item1.CosMar43 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar44 != null) ? item1.CosMar44 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar45 != null) ? item1.CosMar45 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar46 != null) ? item1.CosMar46 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar47 != null) ? item1.CosMar47 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar48 != null) ? item1.CosMar48 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar49 != null) ? item1.CosMar49 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar50 != null) ? item1.CosMar50 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar51 != null) ? item1.CosMar51 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar52 != null) ? item1.CosMar52 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar53 != null) ? item1.CosMar53 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar54 != null) ? item1.CosMar54 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar55 != null) ? item1.CosMar55 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar56 != null) ? item1.CosMar56 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar57 != null) ? item1.CosMar57 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar58 != null) ? item1.CosMar58 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar59 != null) ? item1.CosMar59 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar60 != null) ? item1.CosMar60 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar61 != null) ? item1.CosMar61 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar62 != null) ? item1.CosMar62 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar63 != null) ? item1.CosMar63 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar64 != null) ? item1.CosMar64 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar65 != null) ? item1.CosMar65 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar66 != null) ? item1.CosMar66 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar67 != null) ? item1.CosMar67 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar68 != null) ? item1.CosMar68 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar69 != null) ? item1.CosMar69 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar70 != null) ? item1.CosMar70 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar71 != null) ? item1.CosMar71 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar72 != null) ? item1.CosMar72 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar73 != null) ? item1.CosMar73 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar74 != null) ? item1.CosMar74 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar75 != null) ? item1.CosMar75 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar76 != null) ? item1.CosMar76 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar77 != null) ? item1.CosMar77 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar78 != null) ? item1.CosMar78 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar79 != null) ? item1.CosMar79 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar80 != null) ? item1.CosMar80 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar81 != null) ? item1.CosMar81 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar82 != null) ? item1.CosMar82 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar83 != null) ? item1.CosMar83 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar84 != null) ? item1.CosMar84 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar85 != null) ? item1.CosMar85 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar86 != null) ? item1.CosMar86 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar87 != null) ? item1.CosMar87 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar88 != null) ? item1.CosMar88 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar89 != null) ? item1.CosMar89 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar90 != null) ? item1.CosMar90 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar91 != null) ? item1.CosMar91 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar92 != null) ? item1.CosMar92 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar93 != null) ? item1.CosMar93 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar94 != null) ? item1.CosMar94 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar95 != null) ? item1.CosMar95 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar96 != null) ? item1.CosMar96 : Decimal.Zero;
                            colum++;
                            iDia++;
                        }
                        //Ajustar columnas
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.AutoFitColumns();//Ajustar columnas
                        //Cabecera
                        rg = ws.Cells[1, 1, 1, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        //Border por celda
                        rg = ws.Cells[1, 1, row, colum - 1];
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
                iResultado = 1;
            }
            catch
            {
                iResultado = -1;
            }
            return Json(iResultado);
        }

        [HttpGet]
        public virtual ActionResult AbrirPorBarra()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCostoMarginalBarraExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCostoMarginalBarraExcel);
        }

    }
}
