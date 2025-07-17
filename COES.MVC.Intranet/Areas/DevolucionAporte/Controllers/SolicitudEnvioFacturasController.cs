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
    public class SolicitudEnvioFacturasController : BaseController
    {
        DevolucionAportesAppServicio _svcDevolucionAporte = new DevolucionAportesAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(SolicitudEnvioFacturasController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("SolicitudEnvioFacturasController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("SolicitudEnvioFacturasController", ex);
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

        public JsonResult GenerarCarta(string nrocarta, int anio, string aports)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                string nuevaruta = string.Format(ConfigurationManager.AppSettings[RutaDirectorio.RutaFacturas], anio);

                if (System.IO.Directory.Exists(nuevaruta))
                {
                    foreach (var item in System.IO.Directory.GetFiles(nuevaruta, "*.docx"))
                    {
                        System.IO.File.Delete(item);
                    }
                }
                else
                {
                    Directory.CreateDirectory(nuevaruta);
                }

                string[] aaports = aports.Split(',');
                for (int i = 0; i < aaports.Length; i++)
                {
                    SiEmpresaDTO aportante = _svcDevolucionAporte.GetByIdEmpresa(Convert.ToInt32(aaports[i]));

                    List<DaiCalendariopagoDTO> calendariopago = _svcDevolucionAporte.GetByCriteriaAnioDaiCalendariopagos(aportante.Emprcodi);
                    calendariopago = calendariopago.Where(cp => Convert.ToInt32(cp.Caleanio) == anio).ToList();

                    DaiCalendariopagoDTO calendariopagoactual = calendariopago.FirstOrDefault(cp => cp.Caleanio == anio.ToString());

                    if (calendariopagoactual == null)
                    {
                        continue;
                    }

                    DaiTablacodigoDetalleDTO tablaCodigoDetalle = _svcDevolucionAporte.GetByIdDaiTablacodigoDetalle(Convert.ToInt32(TablaCodigoDetalleDAI.IGV));
                    decimal igv = Convert.ToDecimal(tablaCodigoDetalle.Tabdvalor);

                    string nuevaRuta = string.Format(@"{0}\{1}.docx", nuevaruta, aportante.Emprrazsocial);
                    //doc.SaveAs(nuevaRuta);

                    using (DocX document = DocX.Create(nuevaRuta))
                    {
                        document.InsertParagraph("\n");
                        Paragraph bTitulo = document.InsertParagraph("San Isidro, " + DateTime.Now.Day.ToString("00") + " de " + f_get_nombre_mes(DateTime.Now.Month) + " de " + DateTime.Now.Year.ToString() + "\r\n").FontSize(12).Font(new FontFamily("Calibri"));
                        bTitulo.Alignment = Alignment.left;

                        Paragraph Titulo = document.InsertParagraph(string.Format("COES/D-{0}-{1}", nrocarta, anio.ToString())).FontSize(10).Font(new FontFamily("Arial")).Bold().UnderlineStyle(UnderlineStyle.singleLine);
                        Titulo.Alignment = Alignment.left;

                        document.InsertParagraph("\n");

                        Paragraph Titulo2 = document.InsertParagraph("Señores:").FontSize(10).Font(new FontFamily("Arial"));
                        Titulo2.Alignment = Alignment.left;

                        Paragraph Titulo3 = document.InsertParagraph(aportante.Emprrazsocial).FontSize(10).Font(new FontFamily("Arial")).Bold();
                        Titulo3.Alignment = Alignment.left;

                        Paragraph Titulo4 = document.InsertParagraph("Presente.-").FontSize(10).Font(new FontFamily("Arial")).UnderlineStyle(UnderlineStyle.singleLine);
                        Titulo4.Alignment = Alignment.left;

                        document.InsertParagraph("\n");

                        Paragraph p4 = document.InsertParagraph("ASUNTO     :      Se solicita emitir una factura a cargo del COES-SINAC por los intereses").FontSize(10).Font(new FontFamily("Arial")).Bold();
                        p4.Alignment = Alignment.both;

                        Paragraph p4sub = document.InsertParagraph(string.Format("\t\t generados durante el año {0} para el Presupuesto de Inversiones.", anio.ToString())).FontSize(10).Font(new FontFamily("Arial")).Bold();
                        p4sub.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        Paragraph p5 = document.InsertParagraph("De nuestra consideración:").FontSize(10).Font(new FontFamily("Arial"));
                        p5.Alignment = Alignment.left;

                        document.InsertParagraph("\n");

                        Paragraph p6 = document.InsertParagraph("Tengo el agrado de dirigirme a ustedes, con relación a los aportes reembolsables del Presupuesto de Inversiones que el COES-SINAC mantiene con las empresas aportantes.").FontSize(10).Font(new FontFamily("Arial"));
                        p6.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        decimal intereses = calendariopago.Sum(cp => cp.Caleinteres.Value);
                        decimal interesesigv = CalcularMonto(intereses, igv);

                        Paragraph p7 = document.InsertParagraph(string.Format("Al respecto, se procederá al pago de los intereses correspondiente al año {0}. Por lo cual solicitamos la emisión de una factura a cargo del COES-SINAC (RUC. 20261159733) por el importe de US$ {1} más US$ {2} por IGV, haciendo un importe total de US$ {3}, cuyo detalle es el siguiente: ", anio.ToString(), intereses.ToString("#,##0.00"), interesesigv.ToString("#,##0.00"), (intereses + interesesigv).ToString("#,##0.00"))).FontSize(10).Font(new FontFamily("Arial"));
                        p7.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        //List<DaiCalendariopagoDTO> cuotas = _svcDevolucionAporte.GetByCriteriaAnioDaiCalendariopagos(aportante.Emprcodi);
                        //List<DaiCalendariopagoDTO> calendariopagoanterior = cuotas.Where(cp => Convert.ToInt32(cp.Caleanio) <= anio).ToList();
                        //.GroupBy(cp => cp.Caleanio).Select(cp => new DaiCalendariopagoDTO { Caleanio = cp.FirstOrDefault().Caleanio, Caleamortizacion = cp.Sum(c => c.Caleamortizacion.Value) }).ToList();

                        if (calendariopago.Count > 0)
                        {
                            int rows = calendariopago.Count + 1;

                            Table table = document.InsertTable(rows, 2);
                            table.Design = TableDesign.TableGrid;
                            table.AutoFit = AutoFit.ColumnWidth;

                            table.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;

                            table.Rows[0].Cells[0].Width = 301;
                            table.Rows[0].Cells[1].Width = 301;

                            decimal interesesDetalle = 0;
                            calendariopago = calendariopago.OrderBy(cp => cp.Presanio).ToList();
                            for (int c = 0; c < calendariopago.Count; c++)
                            {
                                decimal montoInteres = calendariopago[c].Caleinteres.Value;
                                //montoInteres = montoInteres + CalcularMonto(montoInteres, igv);

                                interesesDetalle += montoInteres;

                                table.Rows[c + 1].Cells[0].MarginTop = 3;
                                table.Rows[c + 1].Cells[1].MarginBottom = 3;
                                table.Rows[c + 1].Cells[0].Paragraphs[0].Append("INVERSIONES " + calendariopago[c].Presanio).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.left;
                                table.Rows[c + 1].Cells[1].Paragraphs[0].Append("US$ " + montoInteres.ToString("#,##0.00")).FontSize(10).Font(new FontFamily("Arial")).Alignment = Alignment.right;
                            }

                            table.Rows[0].Cells[0].MarginTop = 3;
                            table.Rows[0].Cells[1].MarginBottom = 3;
                            table.Rows[0].Cells[0].Paragraphs[0].Append("TOTAL").FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.center;
                            table.Rows[0].Cells[1].Paragraphs[0].Append("US$ " + interesesDetalle.ToString("#,##0.00")).FontSize(10).Font(new FontFamily("Arial")).Bold().Alignment = Alignment.right;
                        }

                        Paragraph p12 = document.InsertParagraph("Más IGV.").FontSize(10).Font(new FontFamily("Arial"));
                        p12.Alignment = Alignment.right;

                        document.InsertParagraph("\n");

                        Paragraph p8 = document.InsertParagraph("Una vez recibida la factura, se procederá a realizar la devolución de los intereses y amortizaciones correspondientes a los aportes antes mencionados.").FontSize(10).Font(new FontFamily("Arial"));
                        p8.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        Paragraph p9 = document.InsertParagraph("Para cualquier consulta favor comunicarse con el Ing. Giancarlo Velarde a la dirección electrónica giancarlo.velarde@coes.org.pe o al teléfono 611-8530.").FontSize(10).Font(new FontFamily("Arial"));
                        p9.Alignment = Alignment.both;

                        document.InsertParagraph("\n");

                        Paragraph p10 = document.InsertParagraph("Sin otro particular, hacemos propicia la ocasión para reiterarles nuestros cordiales saludos.").FontSize(10).Font(new FontFamily("Arial"));
                        p10.Alignment = Alignment.left;

                        document.InsertParagraph("\n");

                        Paragraph p11 = document.InsertParagraph("Atentamente,").FontSize(10).Font(new FontFamily("Arial"));
                        p11.Alignment = Alignment.left;

                        var imagenfirma = Server.MapPath("~/Areas/DevolucionAporte/Content/Images/firmaw.png");
                        Novacode.Image ImagenFirma = document.AddImage(imagenfirma);

                        Picture picture = ImagenFirma.CreatePicture();
                        picture.Width = 150;
                        picture.Height = 120;

                        Paragraph firma = document.InsertParagraph("").AppendPicture(picture);
                        firma.Alignment = Alignment.left;

                        //MemoryStream ms = new MemoryStream();
                        document.Save();
                    }
                }

                int archivos = System.IO.Directory.GetFiles(nuevaruta, "*.docx").Length;

                return Json(archivos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("GenerarCarta", ex);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public FileContentResult Descargarzip(int anio)
        {
            string nuevaruta = string.Format(ConfigurationManager.AppSettings[RutaDirectorio.RutaFacturas], anio);

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

        public decimal CalcularMonto(decimal number, decimal percent)
        {
            //return ((double) 80         *       25)/100;
            return ((decimal)number * percent) / 100;
        }
    }
}
