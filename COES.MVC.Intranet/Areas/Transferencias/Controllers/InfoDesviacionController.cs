using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Helper;
//using COES.MVC.Intranet.Models;
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
    public class InfoDesviacionController : Controller
    {
        // GET: /Transferencias/InfoDesviacion/
        public ActionResult Index(int iBarrCodi = 0, int iDesbalance = 0, int iPeriCodi = 0, int iVersion = 0)
        {
            TempData["iBarrCodi"] = iBarrCodi;
            BarraModel modelBarra = new BarraModel();
            modelBarra.Entidad = (new BarraAppServicio()).GetByIdBarra(iBarrCodi);
            TempData["BarrNombBarrTran"] = modelBarra.Entidad.BarrNombBarrTran;
            TempData["iDesbalance"] = iDesbalance;
            TempData["iPeriCodi"] = iPeriCodi;
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);
            TempData["PeriNombre"] = modelPeriodo.Entidad.PeriNombre;
            TempData["iVersion"] = iVersion;
            return View();
            
        }

        //POST
        [HttpPost]
        public ActionResult Lista(int iPeriCodi, int iVersion, int iDesbalance, int iBarrCodi, int iValorizacion)
        {
            InfoDesviacionModel modelListaResultado = new InfoDesviacionModel(); // Lista de resultados
            modelListaResultado.ListaInfoDesviacion = new List<Dominio.DTO.Transferencias.InfoDesviacionDTO>();
            
            //Cargamos la lista de Códigos de Entrega o Retiro con su respectiva energía mensual.
            InfoDesviacionModel modelListaCodigo = new InfoDesviacionModel();
            modelListaCodigo.ListaInfoDesviacion = (new InfoDesviacionAppServicio()).GetByListaCodigo(iPeriCodi, iVersion, iBarrCodi);
            foreach (var mLC in modelListaCodigo.ListaInfoDesviacion)
            {   //Para cada código, calcularemos la energia de los últimos iValorizacion meses
                string sCodigo = mLC.Codigo;
                decimal EnergiaAnterior = 0;
                decimal iNumDiaAnterior = 0;
                int iContador = 0;
                int iPeriCodiAnterior = iPeriCodi;
                while (iContador < iValorizacion)
                {
                    //Traemos el periodo anterior junto con su ultimo recalculo
                    PeriodoModel modelPeriodo = new PeriodoModel();
                    modelPeriodo.Entidad = (new PeriodoAppServicio()).BuscarPeriodoAnterior(iPeriCodiAnterior);
                    if (modelPeriodo.Entidad != null)
                    {
                        iPeriCodiAnterior = modelPeriodo.Entidad.PeriCodi;
                        int iVersionAnterior = (new RecalculoAppServicio()).GetUltimaVersion(iPeriCodiAnterior);
                        //Traemos Para este código el numero de dias y la energia
                        InfoDesviacionDTO entidadCodigoAnterior = new InfoDesviacionDTO();
                        entidadCodigoAnterior = (new InfoDesviacionAppServicio()).GetByEnergiaByBarraCodigo(iPeriCodiAnterior, iVersionAnterior, iBarrCodi, sCodigo);
                        if (entidadCodigoAnterior != null)
                        {   //El resultado me dara un solo código con su energia y el numero de dias 
                            EnergiaAnterior += entidadCodigoAnterior.Energia;
                            iNumDiaAnterior += entidadCodigoAnterior.NroDia;
                        }
                        iContador++;
                    }
                    else
                        break;
                }
                if (EnergiaAnterior > 0 && iNumDiaAnterior > 0)
                {   //aplicar formula y llenar el campo Desviacion 
                    decimal PromedioEnergiaAnterior = EnergiaAnterior / iNumDiaAnterior;
                    decimal dDesviacion = 100 - Math.Abs(((mLC.Energia - PromedioEnergiaAnterior) / PromedioEnergiaAnterior)*100);
                    mLC.Desviacion = dDesviacion;
                }
                else
                    mLC.Desviacion = 0;
                //Agregamos el registro a la lista de resultados
                InfoDesviacionDTO EntidadAux = new InfoDesviacionDTO();
                EntidadAux.Codigo = mLC.Codigo;
                EntidadAux.Generador = mLC.Generador;
                EntidadAux.Cliente = mLC.Cliente;
                EntidadAux.Energia = mLC.Energia;
                EntidadAux.EnergiaAnterior = EnergiaAnterior;
                EntidadAux.Desviacion = mLC.Desviacion;
                modelListaResultado.ListaInfoDesviacion.Add(EntidadAux);
            }
            TempData["tdListaInfoDesviacion"] = modelListaResultado.ListaInfoDesviacion;
            return PartialView(modelListaResultado);
        }

        [HttpPost]
        public JsonResult GenerarExcel(int iPeriCodi, int iVersion, int iDesbalance, int iBarrCodi, int iValorizacion)
        {
            int iResultado = 1;
            //Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(iPeriCodi);
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;
            BarraDTO dtoBarra = new BarraDTO();
            dtoBarra = (new BarraAppServicio()).GetByIdBarra(iBarrCodi);

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                InfoDesviacionModel model = new InfoDesviacionModel();
                if (TempData["tdListaInfodesbalance"] != null)
                    model.ListaInfoDesviacion = (List<InfoDesviacionDTO>)TempData["tdListaInfoDesviacion"];
                else
                {
                    model.ListaInfoDesviacion = new List<Dominio.DTO.Transferencias.InfoDesviacionDTO>();
                    //Cargamos la lista de Códigos de Entrega o Retiro con su respectiva energía mensual.
                    InfoDesviacionModel modelListaCodigo = new InfoDesviacionModel();
                    modelListaCodigo.ListaInfoDesviacion = (new InfoDesviacionAppServicio()).GetByListaCodigo(iPeriCodi, iVersion, iBarrCodi);
                    foreach (var mLC in modelListaCodigo.ListaInfoDesviacion)
                    {   //Para cada código, calcularemos la energia de los últimos iValorizacion meses
                        string sCodigo = mLC.Codigo;
                        decimal EnergiaAnterior = 0;
                        decimal iNumDiaAnterior = 0;
                        int iContador = 0;
                        int iPeriCodiAnterior = iPeriCodi;
                        while (iContador < iValorizacion)
                        {
                            //Traemos el periodo anterior junto con su ultimo recalculo
                            PeriodoModel modelPeriodo = new PeriodoModel();
                            modelPeriodo.Entidad = (new PeriodoAppServicio()).BuscarPeriodoAnterior(iPeriCodiAnterior);
                            if (modelPeriodo.Entidad != null)
                            {
                                iPeriCodiAnterior = modelPeriodo.Entidad.PeriCodi;
                                int iVersionAnterior = (new RecalculoAppServicio()).GetUltimaVersion(iPeriCodiAnterior);
                                //Traemos Para este código el numero de dias y la energia
                                InfoDesviacionDTO entidadCodigoAnterior = new InfoDesviacionDTO();
                                entidadCodigoAnterior = (new InfoDesviacionAppServicio()).GetByEnergiaByBarraCodigo(iPeriCodiAnterior, iVersionAnterior, iBarrCodi, sCodigo);
                                if (entidadCodigoAnterior != null)
                                {   //El resultado me dara un solo código con su energia y el numero de dias 
                                    EnergiaAnterior += entidadCodigoAnterior.Energia;
                                    iNumDiaAnterior += entidadCodigoAnterior.NroDia;
                                }
                                iContador++;
                            }
                            else
                                break;
                        }
                        if (EnergiaAnterior > 0 && iNumDiaAnterior > 0)
                        {   //aplicar formula y llenar el campo Desviacion 
                            decimal PromedioEnergiaAnterior = EnergiaAnterior / iNumDiaAnterior;
                            decimal dDesviacion = 100 - Math.Abs(((mLC.Energia - PromedioEnergiaAnterior) / PromedioEnergiaAnterior) * 100);
                            mLC.Desviacion = dDesviacion;
                        }
                        else
                            mLC.Desviacion = 0;
                        //Agregamos el registro a la lista de resultados
                        InfoDesviacionDTO EntidadAux = new InfoDesviacionDTO();
                        EntidadAux.Codigo = mLC.Codigo;
                        EntidadAux.Generador = mLC.Generador;
                        EntidadAux.Cliente = mLC.Cliente;
                        EntidadAux.Energia = mLC.Energia;
                        EntidadAux.EnergiaAnterior = EnergiaAnterior;
                        EntidadAux.Desviacion = mLC.Desviacion;
                        model.ListaInfoDesviacion.Add(EntidadAux);
                    }
                }

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteInfoDesviacionExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteInfoDesviacionExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "DESBALANCE DE ENERGÍA: ENTREGAS Y RETIROS - [" + modelPeri.Entidad.PeriNombre + " - " + modelPeri.Entidad.RecaNombre + "]";
                        ws.Cells[2, 4].Value = "% Desviación: " + iDesbalance + " / Barra de transferencia: " + dtoBarra.BarrNombBarrTran + " / Nro Valorizaciones pasadas: " + iValorizacion;
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "CÓDIGO";
                        ws.Cells[5, 3].Value = "GENERADOR";
                        ws.Cells[5, 4].Value = "CLIENTE";
                        ws.Cells[5, 5].Value = "ENERGÍA MWh";
                        ws.Column(5).Style.Numberformat.Format = "#,##0.000";
                        ws.Cells[5, 6].Value = "ENERGÍA ANTERIOR MWh";
                        ws.Column(6).Style.Numberformat.Format = "#,##0.000";
                        ws.Cells[5, 7].Value = "DESVIACIÓN (%)";
                        ws.Column(7).Style.Numberformat.Format = "#,##0.00";

                        rg = ws.Cells[5, 2, 5, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaInfoDesviacion)
                        {
                            ws.Cells[row, 2].Value = item.Codigo.ToString();
                            ws.Cells[row, 3].Value = item.Generador;
                            ws.Cells[row, 4].Value = item.Cliente;
                            ws.Cells[row, 5].Value = item.Energia;
                            ws.Cells[row, 6].Value = item.EnergiaAnterior;
                            ws.Cells[row, 7].Value = item.Desviacion;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 7];
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
                        ws.View.FreezePanes(6, 8);
                        rg = ws.Cells[5, 2, row, 7];
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
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteInfoDesviacionExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteInfoDesviacionExcel);
        }

    }
}
