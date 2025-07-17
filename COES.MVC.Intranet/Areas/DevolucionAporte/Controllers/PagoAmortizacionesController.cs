using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.DevolucionAportes;
using log4net;
using Novacode;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DevolucionAporte.Controllers
{
    [ValidarSesion]
    public class PagoAmortizacionesController : BaseController
    {
        DevolucionAportesAppServicio _svcDevolucionAporte = new DevolucionAportesAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PagoAmortizacionesController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("PagoAmortizacionesController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("PagoAmortizacionesController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            List<DaiAportanteDTO> listadoAniosCronograma = _svcDevolucionAporte.GetByCriteriaAniosCronograma(0, 0);
            ViewBag.ListaAnioInversion = listadoAniosCronograma.Select(a => new DaiPresupuestoDTO
            {
                Presanio = a.Anio.ToString()
            });
            return View();
        }

        public ActionResult Listado(int anio)
        {
            List<DaiAportanteDTO> listadoAportantes = new List<DaiAportanteDTO>();
            try
            {
                listadoAportantes = _svcDevolucionAporte.GetByCriteriaDaiAportantesCronograma(anio, "");
                listadoAportantes = listadoAportantes.GroupBy(a => new { Emprcodi = a.Emprcodi, Emprnomb = a.Emprnomb, Tipoempresa = a.Tipoempresa, Emprruc = a.Emprruc, Emprrazsocial = a.Emprrazsocial }).Select(a => new DaiAportanteDTO
                {
                    Emprcodi = a.FirstOrDefault().Emprcodi,
                    Emprnomb = a.FirstOrDefault().Emprnomb,
                    Tipoempresa = a.FirstOrDefault().Tipoempresa,
                    Emprruc = a.FirstOrDefault().Emprruc,
                    Emprrazsocial = a.FirstOrDefault().Emprrazsocial
                }).ToList();

            }
            catch (Exception ex)
            {
                log.Error("Listado", ex);
            }

            return PartialView(listadoAportantes);
        }

        public ActionResult Parametros(int anio)
        {
            ViewBag.AnioInversion = anio;

            return View();
        }

        //public JsonResult ListarAnios(string prescodi, string apors) {
        //    List<DaiAportanteDTO> listadoAniosCronograma = _svcDevolucionAporte.GetByCriteriaAniosCronograma(Convert.ToInt32(prescodi), Convert.ToInt32(DaiEstadoCronogramaCuota.Pendiente));
        //    listadoAniosCronograma = listadoAniosCronograma.Where(ac => apors.Contains(ac.Aporcodi.ToString())).ToList();

        //    return Json(listadoAniosCronograma, JsonRequestBehavior.AllowGet);
        //}
        
        public FileContentResult DescargarAportrantes(string aports)
        {
            base.ValidarSesionUsuario();

            MemoryStream ms = new MemoryStream();

            string nombre = "Aportantes";

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("CuadroDevolucaciones");
                ws.View.ShowGridLines = false;

                ws.Cells["A1:B1"].Merge = true;
                ws.Cells["A1:B1"].Style.Font.Size = 11;
                ws.Cells["A1:B1"].Style.Font.Bold = true;
                ws.Cells["A1:B1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A1:B1"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["A1:B1"].Value = "CÓDIGO";
                ws.Cells["A1:B1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["C1:D1"].Merge = true;
                ws.Cells["C1:D1"].Style.Font.Size = 11;
                ws.Cells["C1:D1"].Style.Font.Bold = true;
                ws.Cells["C1:D1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["C1:D1"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["C1:D1"].Value = "RAZON SOCIAL";
                ws.Cells["C1:D1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["E1:F1"].Merge = true;
                ws.Cells["E1:F1"].Style.Font.Size = 11;
                ws.Cells["E1:F1"].Style.Font.Bold = true;
                ws.Cells["E1:F1"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["E1:F1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["E1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["E1:F1"].Value = "N° CHEQUE AMORTIZACION";
                ws.Cells["E1:F1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["G1:H1"].Merge = true;
                ws.Cells["G1:H1"].Style.Font.Size = 11;
                ws.Cells["G1:H1"].Style.Font.Bold = true;
                ws.Cells["G1:H1"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["G1:H1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["G1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["G1:H1"].Value = "N° RECIBO EGRESOS";
                ws.Cells["G1:H1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["I1:J1"].Merge = true;
                ws.Cells["I1:J1"].Style.Font.Size = 11;
                ws.Cells["I1:J1"].Style.Font.Bold = true;
                ws.Cells["I1:J1"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["I1:J1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["I1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["I1:J1"].Value = "N° CHEQUE INTERESES";
                ws.Cells["I1:J1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                int i = 2;
                int contador = 1;

                List<SiEmpresaDTO> listadoAportante = new List<SiEmpresaDTO>();
                string[] aaports = aports.Split(',');
                for (int x = 0; x < aaports.Length; x++)
                {
                    SiEmpresaDTO aportante = _svcDevolucionAporte.GetByIdEmpresa(Convert.ToInt32(aaports[x]));
                    
                    listadoAportante.Add(aportante);
                }

                listadoAportante = listadoAportante.OrderBy(a => a.Emprnomb).ToList();
                foreach (SiEmpresaDTO aportante in listadoAportante)
                {
                    ws.Cells[string.Format("A{0}:B{0}", i)].Merge = true;
                    ws.Cells[string.Format("A{0}:B{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("A{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("A{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("A{0}:B{0}", i)].Value = aportante.Emprcodi;
                    ws.Cells[string.Format("A{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("A{0}:B{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("C{0}:D{0}", i)].Merge = true;
                    ws.Cells[string.Format("C{0}:D{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("C{0}:D{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("C{0}:D{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("C{0}:D{0}", i)].Value = aportante.Emprnomb;
                    ws.Cells[string.Format("C{0}:D{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("C{0}:D{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("E{0}:F{0}", i)].Merge = true;
                    ws.Cells[string.Format("E{0}:F{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("E{0}:F{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("E{0}:F{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("E{0}:F{0}", i)].Value = "";
                    ws.Cells[string.Format("E{0}:F{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("E{0}:F{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("G{0}:H{0}", i)].Merge = true;
                    ws.Cells[string.Format("G{0}:H{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("G{0}:H{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("G{0}:H{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("G{0}:H{0}", i)].Value = "";
                    ws.Cells[string.Format("G{0}:H{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("G{0}:H{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("I{0}:J{0}", i)].Merge = true;
                    ws.Cells[string.Format("I{0}:J{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("I{0}:J{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("I{0}:J{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("I{0}:J{0}", i)].Value = "";
                    ws.Cells[string.Format("I{0}:J{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("I{0}:J{0}", i)].Style.WrapText = true;

                    i += 1;
                    contador++;
                }

                var FileBytesArray = package.GetAsByteArray();
                return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre + ".xlsx");
            }
        }

        public JsonResult ImportarNroCheque()
        {
            try
            {
                List<DaiAportanteDTO> listadoAportante = new List<DaiAportanteDTO>();

                int id = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    int lastIndex = file.FileName.LastIndexOf(Constantes.CaracterPunto);
                    string descripcion = file.FileName;
                    string extension = file.FileName.Substring(lastIndex + 1, file.FileName.Length - lastIndex - 1);

                    //string ruta = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[RutaDirectorio.DevolucionAporteArchivos]);
                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.DevolucionAporteArchivos];

                    ruta += @"\aportantes\";

                    if (!System.IO.Directory.Exists(ruta))
                    {
                        System.IO.Directory.CreateDirectory(ruta);
                    }

                    string rutaArchivo = ruta + file.FileName;
                    file.SaveAs(rutaArchivo);

                    using (ExcelPackage package = new ExcelPackage(new FileInfo(rutaArchivo)))
                    {
                        ExcelWorksheet ws = package.Workbook.Worksheets[1];
                        ws.View.ShowGridLines = true;

                        for (int f = ws.Dimension.Start.Row + 1; f <= ws.Dimension.End.Row; f++)
                        {
                            string codigoAportante = ws.Cells[f, 1].Text;
                            string nroChequeEgreso = ws.Cells[f, 5].Text;
                            string nroReciboEgreso = ws.Cells[f, 7].Text;
                            string nroChequeInteres = ws.Cells[f, 9].Text;

                            listadoAportante.Add(new DaiAportanteDTO
                            {
                                Emprcodi = Convert.ToInt32(codigoAportante),
                                NroChequeEgreso = nroChequeEgreso,
                                NroReciboEgreso = nroReciboEgreso,
                                NroChequeInteres = nroChequeInteres
                            });
                        }
                    }

                    System.IO.File.Delete(rutaArchivo);
                }

                return Json(listadoAportante.OrderBy(e => e.Emprnomb).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("ImportarNroCheque", ex);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        private string f_get_nombre_mes(int pi_mes)
        {

            switch (pi_mes)
            {
                case 1:
                    return "enero";
                case 2:
                    return "febrero";
                case 3:
                    return "marzo";
                case 4:
                    return "abril";
                case 5:
                    return "mayo";
                case 6:
                    return "junio";
                case 7:
                    return "julio";
                case 8:
                    return "agosto";
                case 9:
                    return "setiembre";
                case 10:
                    return "octubre";
                case 11:
                    return "noviembre";
                case 12:
                    return "diciembre";
                default:
                    return "";
            }
        }

        public JsonResult GenerarCarta(List<DaiAportanteDTO> aportantes)
        {
            try
            {
                MemoryStream ms = new MemoryStream();

                string nuevaruta = string.Format(ConfigurationManager.AppSettings[RutaDirectorio.RutaDevoluciones], aportantes[0].Anio);

                if (Directory.Exists(nuevaruta))
                {
                    foreach (var item in System.IO.Directory.GetFiles(nuevaruta, "*.docx"))
                    {
                        System.IO.File.Delete(item);
                    }
                }
                else{
                    Directory.CreateDirectory(nuevaruta);
                }

                foreach (DaiAportanteDTO iaportante in aportantes)
                {
                    SiEmpresaDTO aportante = _svcDevolucionAporte.GetByIdEmpresa(iaportante.Emprcodi);


                    List<DaiCalendariopagoDTO> cuotas = _svcDevolucionAporte.GetByCriteriaAnioDaiCalendariopagos(aportante.Emprcodi);
                    

                    List<DaiCalendariopagoDTO>  calendarioPendiente = cuotas.Where(cp => Convert.ToInt32(cp.Caleanio) == iaportante.Anio && cp.Tabcdcodiestado == Convert.ToInt32(DaiEstadoCronogramaCuota.Pendiente)).ToList();
                    
                    if (iaportante.Pago == 1)
                    {
                        
                        foreach (DaiCalendariopagoDTO item in calendarioPendiente.Distinct())
                        {
                            item.Tabcdcodiestado = Convert.ToInt32(DaiEstadoCronogramaCuota.Pagado);
                            item.Caleusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                            item.Calefecmodificacion = DateTime.Now;
                            item.Calecartapago = iaportante.NroCarta;
                            item.Calechequeamortpago = iaportante.NroChequeEgreso;
                            item.Calechequeintpago = iaportante.NroChequeInteres;

                            _svcDevolucionAporte.UpdateDaiCalendariopago(item);

                            DaiAportanteDTO apor = _svcDevolucionAporte.GetByIdDaiAportante(item.Aporcodi);
                            List<DaiAportanteDTO> cuotasPendientes = _svcDevolucionAporte.GetByCriteriaDaiAportantesFinalizar(new DaiAportanteDTO { Tabcdcodiestado = Convert.ToInt32(DaiEstadoCronogramaCuota.Pendiente), Prescodi = apor.Prescodi, Emprcodi = aportante.Emprcodi });
                            if (cuotasPendientes.Count == 0)
                            {
                                apor.Tabcdcodiestado = Convert.ToInt32(DaiEstadoAportante.Finalizado);
                                apor.Aporusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                                apor.Aporfecmodificacion = DateTime.Now;

                                _svcDevolucionAporte.UpdateDaiAportante(apor);
                            }
                        }
                    }

                    List<DaiCalendariopagoDTO> calendariopago = cuotas.Where(cp => Convert.ToInt32(cp.Caleanio) == iaportante.Anio).ToList();

                    string nuevaRuta = string.Format(@"{0}\{1}.docx", nuevaruta, aportante.Emprrazsocial);

                    using (DocX document = DocX.Create(nuevaRuta))
                    {
                        document.InsertParagraph("\n");
                        Paragraph bTitulo = document.InsertParagraph("San Isidro, " + DateTime.Now.Day.ToString("00") + " de " + f_get_nombre_mes(DateTime.Now.Month) + " de " + DateTime.Now.Year.ToString() + "\r\n").FontSize(12).Font(new FontFamily("Calibri"));
                        bTitulo.Alignment = Alignment.left;

                        Paragraph Titulo = document.InsertParagraph(string.Format("COES/D-{0}-{1}", iaportante.NroCarta, iaportante.Anio)).FontSize(10).Font(new FontFamily("Arial")).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                        Titulo.Alignment = Alignment.left;

                        document.InsertParagraph("\n");

                        Paragraph Titulo2 = document.InsertParagraph("Señor Representante:").FontSize(10).Font(new FontFamily("Arial"));
                        Titulo2.Alignment = Alignment.left;

                        Paragraph Titulo3 = document.InsertParagraph(aportante.Emprrazsocial).FontSize(10).Font(new FontFamily("Arial")).Bold();
                        Titulo3.Alignment = Alignment.left;

                        Paragraph Titulo4 = document.InsertParagraph("Presente.-").FontSize(10).Font(new FontFamily("Arial")).UnderlineStyle(UnderlineStyle.singleLine);
                        Titulo4.Alignment = Alignment.left;

                        document.InsertParagraph("\n");

                        Paragraph p4 = document.InsertParagraph("ASUNTO     :     Pago de amortización e intereses correspondiente a los Aporte de Inversiones.").FontSize(10).Font(new FontFamily("Arial")).Bold();
                        p4.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        Paragraph p5 = document.InsertParagraph("De nuestra consideración:").FontSize(12).Font(new FontFamily("Calibri"));
                        p5.Alignment = Alignment.left;

                        document.InsertParagraph("\n");

                        Paragraph p6 = document.InsertParagraph(string.Format("Tenemos el agrado de dirigirnos a usted con la finalidad de alcanzarle el Cheque Nº {0} del Interbank por el importe de US$ {1} el Recibo de Egresos N° {2} para efectuar el pago de la amortización de capital, correspondiente al aporte reembolsable referido a la deuda de Inversiones.", iaportante.NroChequeEgreso, calendariopago.Sum(c => c.Caleamortizacion.Value).ToString("#,##0.00"), iaportante.NroReciboEgreso)).FontSize(10).Font(new FontFamily("Arial"));
                        p6.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        Paragraph p7 = document.InsertParagraph(string.Format("Asimismo, se adjunta el Cheque Nº {0} del Interbank por el importe de US$ {1} por los intereses generados en el {2} a favor de su representada y la constancia de Detracción si fuera el caso.", iaportante.NroChequeInteres, calendariopago.Sum(c => c.Caleinteres.Value).ToString("#,##0.00"), iaportante.Anio.ToString())).FontSize(10).Font(new FontFamily("Arial"));
                        p7.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        Paragraph p8 = document.InsertParagraph("En el cuadro adjunto se indican los aportes consolidados de las amortizaciones e intereses.").FontSize(10).Font(new FontFamily("Arial"));
                        p8.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        Paragraph p9 = document.InsertParagraph("Para cualquier consulta favor comunicarse con el Ing. Giancarlo Velarde a la dirección electrónica giancarlo.velarde@coes.org.pe o al teléfono 611-8530.").FontSize(10).Font(new FontFamily("Arial"));
                        p9.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        Paragraph p10 = document.InsertParagraph("Sin otro particular, hacemos propicia la ocasión para reiterarle nuestros cordiales saludos. ").FontSize(10).Font(new FontFamily("Arial"));
                        p10.Alignment = Alignment.left;

                        document.InsertParagraph("\n");

                        Paragraph p11 = document.InsertParagraph("Atentamente,").FontSize(10).Font(new FontFamily("Arial"));
                        p11.Alignment = Alignment.left;

                        var imagenfirma = Server.MapPath("~/Areas/DevolucionAporte/Content/Images/firmap.png");
                        Novacode.Image ImagenFirma = document.AddImage(imagenfirma);

                        Picture picture = ImagenFirma.CreatePicture();
                        picture.Width = 120;
                        picture.Height = 120;

                        Paragraph firma = document.InsertParagraph("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t").AppendPicture(picture);
                        firma.Alignment = Alignment.right;

                        document.InsertSectionPageBreak(true);

                        Paragraph p12 = document.InsertParagraph("ANEXO").FontSize(10).Font(new FontFamily("Arial")).Bold();
                        p12.Alignment = Alignment.center;

                        
                        document.InsertParagraph("\n");

                        List<string> cuotasPresanio = calendariopago.Select(c => c.Presanio).Distinct().ToList();
                        if (cuotasPresanio.Count > 0)
                        {
                            //calendariopago
                            int rows = cuotasPresanio.Count + 2;

                            Table table = document.InsertTable(rows, 4);
                            table.Design = TableDesign.TableGrid;
                            table.AutoFit = AutoFit.ColumnWidth;

                            table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;

                            table.Rows[0].Cells[0].Width = 120;
                            table.Rows[0].Cells[1].Width = 120;
                            table.Rows[0].Cells[2].Width = 120;
                            table.Rows[0].Cells[3].Width = 120;

                            table.Rows[0].Cells[0].MarginBottom = 10;
                            table.Rows[0].Cells[1].MarginBottom = 10;
                            table.Rows[0].Cells[2].MarginBottom = 10;
                            table.Rows[0].Cells[3].MarginBottom = 10;

                            table.Rows[0].Cells[0].FillColor = Color.LightGray;
                            table.Rows[0].Cells[1].FillColor = Color.LightGray;
                            table.Rows[0].Cells[2].FillColor = Color.LightGray;
                            table.Rows[0].Cells[3].FillColor = Color.LightGray;

                            table.Rows[0].Cells[0].Paragraphs[0].Append("INVERSIONES").FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.center;
                            table.Rows[0].Cells[1].Paragraphs[0].Append("AMORTIZADO").FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.center;
                            table.Rows[0].Cells[2].Paragraphs[0].Append("INTERES").FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.center;
                            table.Rows[0].Cells[3].Paragraphs[0].Append("TOTAL").FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.center;

                            decimal sumCaleamortizacion = 0;
                            decimal sumCaleinteres = 0;
                            

                            for (int c = 0; c < cuotasPresanio.Count; c++)
                            {
                                decimal amortizacion = calendariopago.Where(cp => cp.Presanio == cuotasPresanio[c]).Sum(cuo => cuo.Caleamortizacion.Value);
                                decimal interes = calendariopago.Where(cp => cp.Presanio == cuotasPresanio[c]).Sum(cuo => cuo.Caleinteres.Value);

                                table.Rows[c + 1].Cells[0].Paragraphs[0].Append(string.Format("{0}", "INVERSIONES " + cuotasPresanio[c].ToString())).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.left;
                                table.Rows[c + 1].Cells[1].Paragraphs[0].Append(string.Format("US$ {0}", amortizacion.ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;
                                table.Rows[c + 1].Cells[2].Paragraphs[0].Append(string.Format("US$ {0}", interes.ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;
                                table.Rows[c + 1].Cells[3].Paragraphs[0].Append(string.Format("US$ {0}", (amortizacion + interes).ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;


                                sumCaleamortizacion += amortizacion;
                                sumCaleinteres += interes;

                                //if (calendarioAnioAgrupado[c].Tabcdcodiestado == Convert.ToInt32(DaiEstadoCronogramaCuota.Pagado) || calendarioAnioAgrupado[c].Tabcdcodiestado == Convert.ToInt32(DaiEstadoCronogramaCuota.Liquidado))
                                //{
                                //    decimal amortizacion = cuota.Sum(cuo => cuo.Caleamortizacion.Value);
                                //    decimal interes = cuota.Sum(cuo => cuo.Caleinteres.Value);

                                //    table.Rows[c + 1].Cells[0].Paragraphs[0].Append(string.Format("{0}", "INVERSIONES " + calendarioAnioAgrupado[c].Caleanio.ToString())).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.left;
                                //    table.Rows[c + 1].Cells[1].Paragraphs[0].Append(string.Format("US$ {0}", amortizacion.ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;
                                //    table.Rows[c + 1].Cells[2].Paragraphs[0].Append(string.Format("US$ {0}", interes.ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;
                                //    table.Rows[c + 1].Cells[3].Paragraphs[0].Append(string.Format("US$ {0}", (amortizacion + interes).ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;


                                //    sumCaleamortizacion += amortizacion;
                                //    sumCaleinteres += interes;
                                //}
                                //else {
                                //    table.Rows[c + 1].Cells[0].Paragraphs[0].Append(string.Format("{0}", "INVERSIONES " + calendarioAnioAgrupado[c].Caleanio.ToString())).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.left;
                                //    table.Rows[c + 1].Cells[1].Paragraphs[0].Append(string.Format("US$ {0}", "0")).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;
                                //    table.Rows[c + 1].Cells[2].Paragraphs[0].Append(string.Format("US$ {0}", "0")).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;
                                //    table.Rows[c + 1].Cells[3].Paragraphs[0].Append(string.Format("US$ {0}", "0")).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;

                                //    sumCaleamortizacion += 0;
                                //    sumCaleinteres += 0;
                                //}
                            }

                            table.Rows[cuotasPresanio.Count + 1].Cells[0].Paragraphs[0].Append("TOTAL").FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.center;
                            table.Rows[cuotasPresanio.Count + 1].Cells[1].Paragraphs[0].Append(string.Format("US$ {0}", sumCaleamortizacion.ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.right;
                            table.Rows[cuotasPresanio.Count + 1].Cells[2].Paragraphs[0].Append(string.Format("US$ {0}", sumCaleinteres.ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.right;
                            table.Rows[cuotasPresanio.Count + 1].Cells[3].Paragraphs[0].Append(string.Format("US$ {0}", (sumCaleamortizacion + sumCaleinteres).ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.right;
                        }

                        //MemoryStream ms = new MemoryStream();
                        document.Save();
                    }
                }

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("GenerarCarta", ex);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public FileContentResult Descargarzip(int anio)
        {
            string nuevaruta = string.Format(ConfigurationManager.AppSettings[RutaDirectorio.RutaDevoluciones], anio);

            var zipFile = nuevaruta + string.Format(@"\{0}.zip", anio);

            if (System.IO.File.Exists(zipFile))
            {
                System.IO.File.Delete(zipFile);
            }

            using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                foreach (var fPath in System.IO.Directory.GetFiles(nuevaruta, "*.docx"))
                {
                    archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                }
            }

            //foreach (var fPath in System.IO.Directory.GetFiles(nuevaruta, ".docx"))
            //{
            //    System.IO.File.Delete(fPath);
            //}

            return File(System.IO.File.ReadAllBytes(zipFile), "application/octet-stream", string.Format("{0}.zip", anio));
        }
    }
}
