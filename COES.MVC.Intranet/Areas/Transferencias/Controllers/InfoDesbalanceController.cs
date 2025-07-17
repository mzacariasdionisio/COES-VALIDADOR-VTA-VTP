using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Transferencias;
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

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class InfoDesbalanceController : Controller
    {
        //
        // GET: /Transferencias/InfoDesbalance/
        public ActionResult Index(int iDesbalance = 0, int iPeriCodi = 0, int iVersion = 0)
        {
            PeriodoModel modelPeriodo1 = new PeriodoModel();
            modelPeriodo1.ListaPeriodos = (new PeriodoAppServicio()).ListPeriodo();
            TempData["PERIANIOMES1"] = new SelectList(modelPeriodo1.ListaPeriodos, "PERICODI", "PERINOMBRE",iPeriCodi);
            Session["iVersion"] = 0;
            TempData["iDesbalance"] = iDesbalance;
            return View();
        }

        /// <summary>
        /// Permite cargar versiones deacuerdo al periodo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculo = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            Session["iVersion"] = modelRecalculo.ListaRecalculo[0].RecaCodi;
            return Json(modelRecalculo.ListaRecalculo);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(int periodo, int version, int desbalance)
        {
            if (version == 0 && !Session["iVersion"].Equals(null)) version = Convert.ToInt32(Session["iVersion"].ToString());
            TempData["Desbalance"] = desbalance;
            TempData["Pericodi"] = periodo;
            TempData["Version"] = version;

            InfoDesbalanceModel modelListaResultado = new InfoDesbalanceModel(); // Lista de resultados
            modelListaResultado.ListaInfodesbalance = new List<Dominio.DTO.Transferencias.InfoDesbalanceDTO>();
            //Cargamos la lista de Barras de Información Base
            InfoDesbalanceModel modelBarrasTransf = new InfoDesbalanceModel();
            modelBarrasTransf.ListaInfodesbalance = (new InfoDesbalanceAppServicio()).GetByListaBarrasTrans(periodo, version);
            foreach (var mBT in modelBarrasTransf.ListaInfodesbalance)
            {
                //Para cada Barra de Transferencia traemos la lista de energía por día: InfoBase, Entrega, Retiro
                InfoDesbalanceModel modelInfoDesbalance = new InfoDesbalanceModel();
                modelInfoDesbalance.ListaInfodesbalance = (new InfoDesbalanceAppServicio()).GetByListaInfoDesbalanceByBarra(periodo, version, mBT.BarrCodi);
                decimal dEnergiaDesbalance = 0;
                decimal dEnergiaEntrega = 0;
                decimal dEnergiaRetiro = 0;
                decimal dDesbalanceDia = 0;
                int iDia = 0;
                foreach (var mLista in modelInfoDesbalance.ListaInfodesbalance)
                {   //Calculo de del desbalance de energía
                    dEnergiaDesbalance += mLista.EnergiaDesbalance;
                    dEnergiaEntrega += mLista.EnergiaEntrega;
                    dEnergiaRetiro += mLista.EnergiaRetiro;
                    decimal dDesbalanceDiaAux = Math.Abs((mLista.EnergiaDesbalance - (mLista.EnergiaRetiro - mLista.EnergiaEntrega)) / mLista.EnergiaDesbalance) * 100;
                    if (dDesbalanceDiaAux > dDesbalanceDia)
                    {
                        dDesbalanceDia = dDesbalanceDiaAux;
                        iDia = mLista.Dia;
                    }
                }
                decimal dDesbalanceMensual = Math.Abs((dEnergiaDesbalance - (dEnergiaRetiro - dEnergiaEntrega)) / dEnergiaDesbalance) * 100;
                if(dDesbalanceMensual >= desbalance)
                {
                    InfoDesbalanceDTO EntidadAux = new InfoDesbalanceDTO();
                    EntidadAux.BarrCodi = mBT.BarrCodi;
                    EntidadAux.BarrTransferencia = mBT.BarrTransferencia;
                    EntidadAux.EnergiaDesbalance = dEnergiaDesbalance;
                    EntidadAux.DesbalanceMensual = dDesbalanceMensual;
                    EntidadAux.Dia = iDia;
                    EntidadAux.DesbalanceDia = dDesbalanceDia;
                    modelListaResultado.ListaInfodesbalance.Add(EntidadAux);
                }
            }
            TempData["tdListaInfodesbalance"] = modelListaResultado.ListaInfodesbalance;
            return PartialView(modelListaResultado);
        }

        [HttpPost]
        public JsonResult GenerarExcel(int iPericodi, int iVersion, int desbalance)
        {
            int iResultado = 1;
            //Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(iPericodi);
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                InfoDesbalanceModel model = new InfoDesbalanceModel();
                if (TempData["tdListaInfodesbalance"] != null)
                    model.ListaInfodesbalance = (List<InfoDesbalanceDTO>)TempData["tdListaInfodesbalance"];
                else
                {
                    model.ListaInfodesbalance = new List<Dominio.DTO.Transferencias.InfoDesbalanceDTO>();
                    //Cargamos la lista de Barras de Información Base
                    InfoDesbalanceModel modelBarrasTransf = new InfoDesbalanceModel();
                    modelBarrasTransf.ListaInfodesbalance = (new InfoDesbalanceAppServicio()).GetByListaBarrasTrans(iPericodi, iVersion);
                    foreach (var mBT in modelBarrasTransf.ListaInfodesbalance)
                    {
                        //Para cada Barra de Transferencia traemos la lista de energía por día: InfoBase, Entrega, Retiro
                        InfoDesbalanceModel modelInfoDesbalance = new InfoDesbalanceModel();
                        modelInfoDesbalance.ListaInfodesbalance = (new InfoDesbalanceAppServicio()).GetByListaInfoDesbalanceByBarra(iPericodi, iVersion, mBT.BarrCodi);
                        decimal dEnergiaDesbalance = 0;
                        decimal dEnergiaEntrega = 0;
                        decimal dEnergiaRetiro = 0;
                        decimal dDesbalanceDia = 0;
                        int iDia = 0;
                        foreach (var mLista in modelInfoDesbalance.ListaInfodesbalance)
                        {   //Calculo de del desbalance de energía
                            dEnergiaDesbalance += mLista.EnergiaDesbalance;
                            dEnergiaEntrega += mLista.EnergiaEntrega;
                            dEnergiaRetiro += mLista.EnergiaRetiro;
                            decimal dDesbalanceDiaAux = Math.Abs((mLista.EnergiaDesbalance - (mLista.EnergiaRetiro - mLista.EnergiaEntrega)) / mLista.EnergiaDesbalance) * 100;
                            if (dDesbalanceDiaAux > dDesbalanceDia)
                            {
                                dDesbalanceDia = dDesbalanceDiaAux;
                                iDia = mLista.Dia;
                            }
                        }
                        decimal dDesbalanceMensual = Math.Abs((dEnergiaDesbalance - (dEnergiaRetiro - dEnergiaEntrega)) / dEnergiaDesbalance) * 100;
                        if (dDesbalanceMensual >= desbalance)
                        {
                            InfoDesbalanceDTO EntidadAux = new InfoDesbalanceDTO();
                            EntidadAux.BarrCodi = mBT.BarrCodi;
                            EntidadAux.BarrTransferencia = mBT.BarrTransferencia;
                            EntidadAux.EnergiaDesbalance = dEnergiaDesbalance;
                            EntidadAux.DesbalanceMensual = dDesbalanceMensual;
                            EntidadAux.Dia = iDia;
                            EntidadAux.DesbalanceDia = dDesbalanceDia;
                            model.ListaInfodesbalance.Add(EntidadAux);
                        }
                    }
                
                }

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInfoDesbalanceExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteInfoDesbalanceExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "DESBALANCE DE ENERGÍA: ENTREGAS Y RETIROS - [" + modelPeri.Entidad.PeriNombre + " - " + modelPeri.Entidad.RecaNombre + "] - [% Desbalance: " + desbalance + "]";
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "BARRA DE TRANSFERENCIA";
                        ws.Cells[5, 3].Value = "ENERGÍA DE DESBALANCE MENSUAL";
                        ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[5, 4].Value = "DESBALANCE MENSUAL";
                        ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[5, 5].Value = "DÍA MAYOR DESBALANCE";
                        ws.Column(5).Style.Numberformat.Format = "#";
                        ws.Cells[5, 6].Value = "DESBALANCE DEL DÍA";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.00";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaInfodesbalance)
                        {
                            ws.Cells[row, 2].Value = item.BarrTransferencia.ToString();
                            ws.Cells[row, 3].Value = item.EnergiaDesbalance;
                            ws.Cells[row, 4].Value = item.DesbalanceMensual;
                            ws.Cells[row, 5].Value = item.Dia;
                            ws.Cells[row, 6].Value = item.DesbalanceDia;
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
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteInfoDesbalanceExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteInfoDesbalanceExcel);
        }
    }
}
