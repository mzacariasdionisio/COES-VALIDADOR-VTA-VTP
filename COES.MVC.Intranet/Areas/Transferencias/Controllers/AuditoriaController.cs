using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.Servicios.Aplicacion.Transferencias;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using System.Globalization;
using System.Collections;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using WebGrease.Css.Ast.Selectors;
using COES.MVC.Intranet.Controllers;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class AuditoriaController : BaseController
    {
        // GET: /Transferencias/valortransferencia/
        //[CustomAuthorize]

        TipoAplicacionAppServicio servicioTipoAplicacion = new TipoAplicacionAppServicio();
        TipoProcesoAppServicio servicioTipoProceso =new TipoProcesoAppServicio();
        AuditoriaProcesoAppServicio servicioAuditoriaProceso = new AuditoriaProcesoAppServicio();
        public ActionResult Index()
        {

            AuditoriaModel model = new AuditoriaModel();
            model.ListaTipoAplicacion = this.servicioTipoAplicacion.ListTipoAplicacion();
            model.ListaTipoProceso =this.servicioTipoProceso.ListTipoProceso(1);
            model.NroPaginas = 1;
            return View(model);
        }
        [HttpPost]
        public ActionResult TraerComboTipoProceso(int TipoProceso)
        {
            AuditoriaModel model = new AuditoriaModel();
            model.ListaTipoProceso = this.servicioTipoProceso.ListTipoProceso(TipoProceso);

            return Json(model);
        }

        [HttpPost]
        public ActionResult Lista(string audprousucreacion, int? tipoproceso, string fechainicial, string fechafinal,int? NroPagina)
        {
            String audpro = audprousucreacion == null || audprousucreacion == "" ? "*" : audprousucreacion;
            String[] fecinic = fechainicial.Split('/');
            String[] fecfinc = fechafinal.Split('/');
            String fecini = fecinic[0];
            String fecfin = fecfinc[0];
            fecini = Int32.Parse(fecini) < 10 ? fecini.Contains("0") ? fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2] : "0" + fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2] : fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2];
            fecfin = Int32.Parse(fecfin) < 10 ? fecfin.Contains("0") ? fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2] : "0" + fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2] : fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2];
            AuditoriaModel model = new AuditoriaModel();
            DateTime fecIni = DateTime.ParseExact(fecini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fecfin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            VtpAuditoriaProcesoDTO ap = new VtpAuditoriaProcesoDTO();
 
   
            model.ListaAuditoriaProceso = this.servicioAuditoriaProceso.ListAuditoriaProcesoByFiltro(audpro, (Int32)tipoproceso, fecIni, fecFin, (Int32)NroPagina,Funcion.PageSize);
            return PartialView(model);

        }
 
        [HttpPost]
        public ActionResult Paginado(string audprousucreacion,int tipoproceso, string fechainicial, string fechafinal)
        {
            String audpro = audprousucreacion == null || audprousucreacion == "" ? "*" : audprousucreacion;
            String[] fecinic = fechainicial.Split('/');
            String[] fecfinc = fechafinal.Split('/');
            String fecini = fecinic[0];
            String fecfin = fecfinc[0];
            fecini = Int32.Parse(fecini) < 10 ? fecini.Contains("0") ? fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2] : "0" + fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2] : fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2];
            fecfin = Int32.Parse(fecfin) < 10 ? fecfin.Contains("0") ? fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2] : "0" + fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2] : fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2];
            DateTime fecIni = DateTime.ParseExact(fecini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fecfin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            AuditoriaModel model = new AuditoriaModel();
            model.NroRegistros=this.servicioAuditoriaProceso.NroRegistroAuditoriaProcesoByFiltro(audpro, tipoproceso, fecIni, fecFin);
            if (model.NroRegistros > Funcion.NroPageShow)
            {
                int pageSize = Funcion.PageSizeVariacionCodigo;
                int nroPaginas = (model.NroRegistros % pageSize == 0) ? model.NroRegistros / pageSize : model.NroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Funcion.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);

        }
        [HttpPost]
        public JsonResult GenerarExcel(string audprousucreacion,int tipoproceso, string fechainicial, string fechafinal, int tipoaplicacion, int NroPagina)
        {
            int indicador = 1;
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                String[] fecinic = fechainicial.Split('/');
                String[] fecfinc = fechafinal.Split('/');
                String fecini = fecinic[0];
                String fecfin = fecfinc[0];
                fecini = Int32.Parse(fecini) < 10 ? fecini.Contains("0") ? fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2] : "0" + fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2] : fecinic[0] + "/" + fecinic[1] + "/" + fecinic[2];
                fecfin = Int32.Parse(fecfin) < 10 ? fecfin.Contains("0") ? fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2] : "0" + fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2] : fecfinc[0] + "/" + fecfinc[1] + "/" + fecfinc[2];
                DateTime fecIni = DateTime.ParseExact(fecini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fecfin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                String audpro = audprousucreacion == null || audprousucreacion == "" ? "*" : audprousucreacion;

                VtpAuditoriaProcesoDTO datosAuditoria = new VtpAuditoriaProcesoDTO();
                AuditoriaModel model = new AuditoriaModel();

                model.ListaAuditoriaProceso = this.servicioAuditoriaProceso.ListAuditoriaProcesoByFiltro(audpro, (Int32)tipoproceso, fecIni, fecFin, 1, 10000);
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteAuditoria);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteAuditoria);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 4].Value = "REPORTE DE AUDITORÍA";
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "Nro Item";
                        ws.Column(2).Style.Numberformat.Format = "#";
                        ws.Cells[5, 3].Value = "LoginUsuario";
                        ws.Cells[5, 4].Value = "Proceso/Actividad";
                        ws.Cells[5, 5].Value = "Fecha y hora de Ejecución";
                        ws.Cells[5, 6].Value = "Estado";
                        ws.Cells[5, 7].Value = "Detalle";
                 

                        rg = ws.Cells[5, 2, 5, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        int i = 1;
                        foreach (var item in model.ListaAuditoriaProceso)
                        {

                            ws.Cells[row, 2].Value = i;
                            ws.Cells[row, 3].Value = (item.Audprousucreacion != null) ? item.Audprousucreacion : string.Empty;
                            ws.Cells[row, 4].Value = (item.Tipprodescripcion != null) ? item.Tipprodescripcion.ToString() : string.Empty;
                            ws.Cells[row, 5].Value = (item.Audprofeccreacion != null) ? item.Audprofeccreacion.ToString() : string.Empty;
                            ws.Cells[row, 6].Value = (item.Estddescripcion != null) ? item.Estddescripcion.ToString() : string.Empty;
                            ws.Cells[row, 7].Value = (item.Audprodescripcion != null) ? item.Audprodescripcion.ToString() : string.Empty;

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
                            i++;
                        }

                        //Fijar panel
                        ws.View.FreezePanes(6, 12);
                        rg = ws.Cells[5, 2, row, 7];
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
                    }
                    xlPackage.Save();
                }

                // MedidorHelper.GenerarReporteValidacionMedidores(list, fechaInicial, fechaFinal, datosMDDespacho, datosMDMedidor);

                return Json(1);
            }
            catch (Exception ex)
            {
                //Log.Error(NameController, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteAuditoria;
            return File(path, Funcion.AppExcel, sFecha + "_" + Funcion.NombreReporteAuditoria);
        }




    }


}
