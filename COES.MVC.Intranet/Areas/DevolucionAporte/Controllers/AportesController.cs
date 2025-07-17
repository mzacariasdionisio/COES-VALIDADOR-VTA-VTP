using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.DevolucionAportes;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DevolucionAporte.Controllers
{
    [ValidarSesion]
    public class AportesController : BaseController
    {
        DevolucionAportesAppServicio _svcDevolucionAporte = new DevolucionAportesAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(AportesController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("AportesController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("AportesController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            ViewBag.ListaAnioInversion = _svcDevolucionAporte.GetByCriteriaDaiPresupuestos();
            return View();
        }

        public ActionResult ListadoAportes(int prescodi)
        {
            List<DaiAportanteDTO> listadoAportantes = new List<DaiAportanteDTO>();
            try
            {
                DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(prescodi);
                listadoAportantes = _svcDevolucionAporte.GetByCriteriaDaiAportantes(prescodi, 0);

                ViewBag.Presamortizacion = presupuesto != null ? presupuesto.Presamortizacion : 0;
            }
            catch (Exception ex)
            {
                log.Error("ListadoAportes", ex);
            }
            
            return PartialView(listadoAportantes);
        }

        public ActionResult Cronograma(int aporcodi)
        {
            DaiAportanteDTO aportante = _svcDevolucionAporte.GetByIdDaiAportante(aporcodi);
            DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(aportante.Prescodi);
            
            ViewBag.Empresa = aportante.Emprrazsocial;
            ViewBag.Anio = presupuesto.Presanio;
            ViewBag.Monto = presupuesto.Presmonto;
            ViewBag.Cuota = presupuesto.Presamortizacion;
            ViewBag.MontoParticipacion = aportante.Apormontoparticipacion;
            ViewBag.AnioSinAportar = aportante.Aporaniosinaporte;

            ViewBag.Tabcdcodiestado = aportante.Tabcdcodiestado;
            ViewBag.Aporcodi = aporcodi;

            return View();
        }

        public ActionResult ListadoCronograma(int aporcodi)
        {
            List<DaiCalendariopagoDTO> calendarioPago = new List<DaiCalendariopagoDTO>();
            try
            {
                calendarioPago = _svcDevolucionAporte.GetByCriteriaDaiCalendariopagos(aporcodi);
            }
            catch (Exception ex)
            {
                log.Error("ListadoCronograma", ex);
            }

            return PartialView(calendarioPago);
        }
        
        public JsonResult Procesar(List<DaiAportanteDTO> listadoaportante, decimal montoMin)
        {
            try
            {
                int prescodi = listadoaportante.Any() ? listadoaportante[0].Prescodi : 0;
                DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(prescodi);

                presupuesto.presprocesada = "1";
                presupuesto.Presusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                presupuesto.Presfecmodificacion = DateTime.Now;

                _svcDevolucionAporte.UpdateDaiPresupuesto(presupuesto);

                foreach (DaiAportanteDTO aportante in listadoaportante)
                {
                    if (aportante.Tabcdcodiestado == Convert.ToInt32(DaiEstadoAportante.SinProcesar))
                    {
                        DaiAportanteDTO raportante = _svcDevolucionAporte.GetByIdDaiAportante(aportante.Aporcodi);

                        //if (raportante.Tabcdcodiestado == Convert.ToInt32(DaiEstadoAportante.Procesado))
                        //{
                        //    if (aportante.Aporaniosinaporte == 0) {
                        //        continue;
                        //    }
                        //}

                        aportante.Aporusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                        aportante.Aporfecmodificacion = DateTime.Now;
                        aportante.Tabcdcodiestado = Convert.ToInt32(DaiEstadoAportante.Procesado);
                        _svcDevolucionAporte.UpdateDaiAportante(aportante);

                        DaiTablacodigoDetalleDTO tablaCodigoDetalle = _svcDevolucionAporte.GetByIdDaiTablacodigoDetalle(Convert.ToInt32(TablaCodigoDetalleDAI.Tasa_Interes));

                        decimal tasa = 0;

                        if (tablaCodigoDetalle != null)
                        {
                            tasa = Convert.ToDecimal(tablaCodigoDetalle.Tabdvalor);
                        }

                        decimal caleTotal = raportante.Apormontoparticipacion.Value;
                        decimal capitalInicial = raportante.Apormontoparticipacion.Value;
                        decimal interesInicial = CalcularMonto(raportante.Apormontoparticipacion.Value, tasa);

                        //decimal intereses = CalcularMonto(presupuesto.Presmonto.Value, raportante.Aporporcentajeparticipacion.Value);

                        if (aportante.Reprocesar == "1")
                        {
                            _svcDevolucionAporte.ReprocesarDaiCalendariopago(new DaiCalendariopagoDTO
                            {
                                Aporcodi = raportante.Aporcodi,
                                Caleusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin,
                                Calefecmodificacion = DateTime.Now
                            });
                        }

                        if (presupuesto.Presamortizacion.HasValue)
                        {
                            string[] anios = presupuesto.Presanio.Split('-');
                            int anio = Convert.ToInt32(anios[0]);

                            int amortizacion = presupuesto.Presamortizacion.Value - aportante.Aporaniosinaporte.Value;
                            double cuota = CalcularCuota(Convert.ToDouble(raportante.Apormontoparticipacion.Value), amortizacion, Convert.ToDouble(tasa));
                            decimal amortizacionInicial = Convert.ToDecimal(cuota) - Convert.ToDecimal(interesInicial);

                            //tablaCodigoDetalle = _svcDevolucionAporte.GetByIdDaiTablacodigoDetalle(Convert.ToInt32(TablaCodigoDetalleDAI.MONTOMINIMOPARTICIPACION));
                            //if (raportante.Apormontoparticipacion.Value > Convert.ToDecimal(tablaCodigoDetalle.Tabdvalor))

                            if (raportante.Apormontoparticipacion.Value > montoMin)
                            {
                                int c = 0;
                                for (int i = aportante.Aporaniosinaporte.Value; i <= presupuesto.Presamortizacion.Value; i++)
                                {
                                    //decimal capital = CalcularPorcentaje(presupuesto.Presmonto.Value, raportante.Aporporcentajeparticipacion.Value);
                                    //decimal caleamortizacion = CalcularPorcentaje(capital, raportante.Aporporcentajeparticipacion.Value);

                                    if (c > 0) {
                                        caleTotal = capitalInicial - amortizacionInicial;

                                        DaiCalendariopagoDTO calendario = new DaiCalendariopagoDTO
                                        {
                                            Aporcodi = aportante.Aporcodi,
                                            Caleanio = (anios.Length > 1 ? anio + i + Convert.ToInt32(anios[1]) : (anio + i)).ToString(),
                                            Calenroamortizacion = c,
                                            Calecapital = Math.Round(capitalInicial, 3),
                                            Caleamortizacion = Math.Round(amortizacionInicial, 3),
                                            Caleinteres = Math.Round(interesInicial, 3),
                                            Caletotal = Math.Round(caleTotal, 3),
                                            Caleactivo = Convert.ToInt32(DaiEstadoRegistro.Activo).ToString(),
                                            Tabcdcodiestado = Convert.ToInt32(DaiEstadoCronogramaCuota.Pendiente),
                                            Caleusucreacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin,
                                            Calefeccreacion = DateTime.Now
                                        };

                                        _svcDevolucionAporte.SaveDaiCalendariopago(calendario);

                                        capitalInicial = capitalInicial - amortizacionInicial;
                                        interesInicial = CalcularMonto(capitalInicial, tasa);
                                        amortizacionInicial = Convert.ToDecimal(cuota) - Convert.ToDecimal(interesInicial);
                                    }

                                    c++;
                                }
                            }
                            else {
                                anio++;
                                //Caleamortizacion = Math.Round(capitalInicial + interesInicial, 3),
                                for (int i = aportante.Aporaniosinaporte.Value; i < 1; i++) {
                                    DaiCalendariopagoDTO calendario = new DaiCalendariopagoDTO
                                    {
                                        Aporcodi = aportante.Aporcodi,
                                        Caleanio = (anios.Length > 1 ? anio + Convert.ToInt32(anios[1]) : anio).ToString(),
                                        Calenroamortizacion = 1,
                                        Calecapital = Math.Round(capitalInicial, 3),
                                        Caleamortizacion = Math.Round(capitalInicial, 3),
                                        Caleinteres = Math.Round(interesInicial, 3),
                                        Caletotal = 0,
                                        Caleactivo = Convert.ToInt32(DaiEstadoRegistro.Activo).ToString(),
                                        Tabcdcodiestado = Convert.ToInt32(DaiEstadoCronogramaCuota.Pendiente),
                                        Caleusucreacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin,
                                        Calefeccreacion = DateTime.Now
                                    };

                                    _svcDevolucionAporte.SaveDaiCalendariopago(calendario);
                                }
                            }
                        }
                    }
                }

                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Procesar", ex);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public FileContentResult ExportarCronograma(int aporcodi)
        {
            base.ValidarSesionUsuario();

            MemoryStream ms = new MemoryStream();

            DaiAportanteDTO aportante = _svcDevolucionAporte.GetByIdDaiAportante(aporcodi);
            DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(aportante.Prescodi);

            string nombre = "Cronograma";

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cronograma de Devolución");
                ws.View.ShowGridLines = true;

                ws.Cells["B2:D2"].Merge = true;
                ws.Cells["B2:D2"].Style.Font.Size = 11;
                ws.Cells["B2:D2"].Style.Font.Bold = true;
                ws.Cells["B2:D2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B2:D2"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B2:D2"].Value = "Cronograma de Devolución";
                ws.Cells["B2:D2"].Style.WrapText = true;

                ws.Cells["B4:B4"].Style.Font.Size = 11;
                ws.Cells["B4:B4"].Style.Font.Bold = true;
                ws.Cells["B4:B4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B4:B4"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B4:B4"].Value = "Empresa:";

                ws.Cells["C4:F4"].Merge = true;
                ws.Cells["C4:F4"].Style.Font.Size = 11;
                ws.Cells["C4:F4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C4:F4"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["C4:F4"].Value = aportante.Emprrazsocial;

                ws.Cells["B5:B5"].Style.Font.Size = 11;
                ws.Cells["B5:B5"].Style.Font.Bold = true;
                ws.Cells["B5:B5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B5:B5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B5:B5"].Value = "Año inversión:";

                ws.Cells["C5:C5"].Merge = true;
                ws.Cells["C5:C5"].Style.Font.Size = 11;
                ws.Cells["C5:C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C5:C5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["C5:C5"].Value = presupuesto.Presanio;

                ws.Cells["D5:D5"].Style.Font.Size = 11;
                ws.Cells["D5:D5"].Style.Font.Bold = true;
                ws.Cells["D5:D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["D5:D5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["D5:D5"].Value = "Monto:";

                ws.Cells["E5:E5"].Merge = true;
                ws.Cells["E5:E5"].Style.Font.Size = 11;
                ws.Cells["E5:E5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["E5:E5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["E5:E5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["E5:E5"].Value = string.Format("{0:n}", presupuesto.Presmonto);

                ws.Cells["F5:F5"].Style.Font.Size = 11;
                ws.Cells["F5:F5"].Style.Font.Bold = true;
                ws.Cells["F5:F5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["F5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["F5:F5"].Value = "Cuotas:";

                ws.Cells["G5:G5"].Merge = true;
                ws.Cells["G5:G5"].Style.Font.Size = 11;
                ws.Cells["G5:G5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["G5:G5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["G5:G5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["G5:G5"].Value = presupuesto.Presamortizacion;

                ws.Cells["B6:B6"].Style.Font.Size = 11;
                ws.Cells["B6:B6"].Style.Font.Bold = true;
                ws.Cells["B6:B6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B6:B6"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B6:B6"].Value = "Monto Aportado:";

                ws.Cells["C6:C6"].Merge = true;
                ws.Cells["C6:C6"].Style.Font.Size = 11;
                ws.Cells["C6:C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C6:C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["C6:C6"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["C6:C6"].Value = string.Format("{0:n}", aportante.Apormontoparticipacion);

                ws.Cells["D6:D6"].Style.Font.Size = 11;
                ws.Cells["D6:D6"].Style.Font.Bold = true;
                ws.Cells["D6:D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["D6:D6"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["D6:D6"].Value = "Años sin Aportar:";

                ws.Cells["E6:E6"].Merge = true;
                ws.Cells["E6:E6"].Style.Font.Size = 11;
                ws.Cells["E6:E6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["E6:E6"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["E6:E6"].Value = aportante.Aporaniosinaporte;

                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Cells["B8:B8"].Style.Font.Size = 11;
                ws.Cells["B8:B8"].Style.Font.Bold = true;
                ws.Cells["B8:B8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B8:B8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B8:B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B8:B8"].Value = "N°";
                ws.Cells["B8:B8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["C8:C8"].Merge = true;
                ws.Cells["C8:C8"].Style.Font.Size = 11;
                ws.Cells["C8:C8"].Style.Font.Bold = true;
                ws.Cells["C8:C8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["C8:C8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C8:C8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["C8:C8"].Value = "Año";
                ws.Cells["C8:C8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["D8:D8"].Merge = true;
                ws.Cells["D8:D8"].Style.Font.Size = 11;
                ws.Cells["D8:D8"].Style.Font.Bold = true;
                ws.Cells["D8:D8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["D8:D8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["D8:D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["D8:D8"].Value = "Capital";
                ws.Cells["D8:D8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["E8:E8"].Merge = true;
                ws.Cells["E8:E8"].Style.Font.Size = 11;
                ws.Cells["E8:E8"].Style.Font.Bold = true;
                ws.Cells["E8:E8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["E8:E8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["E8:E8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["E8:E8"].Value = "Interés";
                ws.Cells["E8:E8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["F8:F8"].Merge = true;
                ws.Cells["F8:F8"].Style.Font.Size = 11;
                ws.Cells["F8:F8"].Style.Font.Bold = true;
                ws.Cells["F8:F8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["F8:F8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["F8:F8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["F8:F8"].Value = "Amortización";
                ws.Cells["F8:F8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["G8:G8"].Merge = true;
                ws.Cells["G8:G8"].Style.Font.Size = 11;
                ws.Cells["G8:G8"].Style.Font.Bold = true;
                ws.Cells["G8:G8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["G8:G8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["G8:G8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["G8:G8"].Value = "Saldo";
                ws.Cells["G8:G8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["H8:H8"].Merge = true;
                ws.Cells["H8:H8"].Style.Font.Size = 11;
                ws.Cells["H8:H8"].Style.Font.Bold = true;
                ws.Cells["H8:H8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["H8:H8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["H8:H8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["H8:H8"].Value = "Carta pago";
                ws.Cells["H8:H8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["I8:I8"].Merge = true;
                ws.Cells["I8:I8"].Style.Font.Size = 11;
                ws.Cells["I8:I8"].Style.Font.Bold = true;
                ws.Cells["I8:I8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["I8:I8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["I8:I8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["I8:I8"].Value = "N° Cheque Amortización";
                ws.Cells["I8:I8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["J8:J8"].Merge = true;
                ws.Cells["J8:J8"].Style.Font.Size = 11;
                ws.Cells["J8:J8"].Style.Font.Bold = true;
                ws.Cells["J8:J8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["J8:J8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["J8:J8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["J8:J8"].Value = "N° Cheque de Interes";
                ws.Cells["J8:J8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["K8:K8"].Merge = true;
                ws.Cells["K8:K8"].Style.Font.Size = 11;
                ws.Cells["K8:K8"].Style.Font.Bold = true;
                ws.Cells["K8:K8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["K8:K8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["K8:K8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["K8:K8"].Value = "Estado";
                ws.Cells["K8:K8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                List<DaiCalendariopagoDTO> calendarioPago = _svcDevolucionAporte.GetByCriteriaDaiCalendariopagos(aporcodi);
                
                int i = 9;
                int contador = 1;

                foreach (DaiCalendariopagoDTO item in calendarioPago)
                {
                    ws.Cells[string.Format("B{0}:B{0}", i)].Merge = true;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Value = contador;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("C{0}:C{0}", i)].Merge = true;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Value = item.Caleanio;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("D{0}:D{0}", i)].Merge = true;
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("D{0}:D{0}", i)].Value = string.Format("{0:n}", item.Calecapital);
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("D{0}:D{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("E{0}:E{0}", i)].Merge = true;
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("E{0}:E{0}", i)].Value = string.Format("{0:n}", item.Caleinteres);
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("E{0}:E{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("F{0}:F{0}", i)].Merge = true;
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("F{0}:F{0}", i)].Value = string.Format("{0:n}", item.Caleamortizacion);
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("F{0}:F{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("G{0}:G{0}", i)].Merge = true;
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("G{0}:G{0}", i)].Value = string.Format("{0:n}", item.Caletotal);
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("G{0}:G{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("H{0}:H{0}", i)].Merge = true;
                    ws.Cells[string.Format("H{0}:H{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("H{0}:H{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("H{0}:H{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("H{0}:H{0}", i)].Value = item.Calecartapago;
                    ws.Cells[string.Format("H{0}:H{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("H{0}:H{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("I{0}:I{0}", i)].Merge = true;
                    ws.Cells[string.Format("I{0}:I{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("I{0}:I{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("I{0}:I{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("I{0}:I{0}", i)].Value = item.Calechequeamortpago;
                    ws.Cells[string.Format("I{0}:I{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("I{0}:I{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("J{0}:J{0}", i)].Merge = true;
                    ws.Cells[string.Format("J{0}:J{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("J{0}:J{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("J{0}:J{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("J{0}:J{0}", i)].Value = item.Calechequeintpago;
                    ws.Cells[string.Format("J{0}:J{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("J{0}:J{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("K{0}:K{0}", i)].Merge = true;
                    ws.Cells[string.Format("K{0}:K{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("K{0}:K{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("K{0}:K{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("K{0}:K{0}", i)].Value = item.Tabddescripcion;
                    ws.Cells[string.Format("K{0}:K{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("K{0}:K{0}", i)].Style.WrapText = true;

                    i += 1;
                    contador++;
                }

                var FileBytesArray = package.GetAsByteArray();
                return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre + ".xlsx");
            }
        }

        public FileContentResult ExportarResumen(int prescodi)
        {
            base.ValidarSesionUsuario();

            MemoryStream ms = new MemoryStream();

            DaiPresupuestoDTO daiPresupuestoDTO = _svcDevolucionAporte.GetByIdDaiPresupuesto(prescodi);
            int anio = Convert.ToInt32(daiPresupuestoDTO.Presanio.Substring(0,4));

            //DaiAportanteDTO aportante = _svcDevolucionAporte.GetByIdDaiAportante(aporcodi);
            //DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(aportante.Prescodi);

            string nombre = "Cronograma";

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("AMORTIZACIONES");
                ws.View.ShowGridLines = true;

                ws.Cells["B2:F2"].Merge = true;
                ws.Cells["B2:F2"].Style.Font.Size = 11;
                ws.Cells["B2:F2"].Style.Font.Bold = true;
                ws.Cells["B2:F2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B2:F2"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B2:F2"].Value = "AMORTIZACIONES GENERADAS POR LOS APORTES DE INVERSIONES " + anio.ToString();
                ws.Cells["B2:F2"].Style.WrapText = true;


                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Cells["B5:B5"].Style.Font.Size = 11;
                ws.Cells["B5:B5"].Style.Font.Bold = true;
                ws.Cells["B5:B5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B5:B5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B5:B5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B5:B5"].Value = "N°";
                ws.Cells["B5:B5"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells["C5:C5"].Merge = true;
                ws.Cells["C5:C5"].Style.Font.Size = 11;
                ws.Cells["C5:C5"].Style.Font.Bold = true;
                ws.Cells["C5:C5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["C5:C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C5:C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["C5:C5"].Value = "Empresas";
                ws.Cells["C5:C5"].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                List<DaiCalendariopagoDTO> calendarioPago = _svcDevolucionAporte.GetByCriteriaDaiCalendariopagos(0);
                List<DaiAportanteDTO> aportantesPorTipo = _svcDevolucionAporte.GetByCriteriaDaiAportantes(prescodi, 0);
                //List<DaiAportanteDTO> aportantesPorTipo  = _svcDevolucionAporte.GetByCriteriaDaiAportantesCronograma(anio, "");
                List<string> tiposEmpresa = aportantesPorTipo.Select(a => a.Tipoempresa).Distinct().ToList();
                List<string> celdas = new List<string> { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" };

                ws.Cells["B4:B4"].Merge = true;
                ws.Cells["B4:B4"].Style.Font.Size = 11;
                ws.Cells["B4:B4"].Style.Font.Bold = true;
                ws.Cells["B4:B4"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["B4:B4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["B4:B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:B4"].Value = "";
                ws.Cells["B4:B4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                ws.Cells["C4:C4"].Merge = true;
                ws.Cells["C4:C4"].Style.Font.Size = 11;
                ws.Cells["C4:C4"].Style.Font.Bold = true;
                ws.Cells["C4:C4"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                ws.Cells["C4:C4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C4:C4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["C4:C4"].Value = "";
                ws.Cells["C4:C4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                for (int c = 0; c < celdas.Count; c++)
                {
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Merge = true;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.Font.Size = 11;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Value = anio + c + 1;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.WrapText = true;

                    ws.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Merge = true;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.Font.Size = 11;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Value = "AÑO " + (c + 1).ToString();
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.WrapText = true;
                }

                int i = 6;
                int contador = 0;

                foreach (string tipo in tiposEmpresa)
                {
                    ws.Cells[string.Format("B{0}:B{0}", i)].Merge = true;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("B{0}:B{0}", i)].Value = contador == 0 ? "" : contador.ToString();
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("B{0}:B{0}", i)].Style.WrapText = true;

                    ws.Cells[string.Format("C{0}:C{0}", i)].Merge = true;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Size = 11;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Bold = true;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Value = tipo;
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("C{0}:C{0}", i)].Style.WrapText = true;

                    for (int c = 0; c < celdas.Count; c++)
                    {
                        ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Merge = true;
                        ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.Font.Size = 11;
                        ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Value = "";
                        ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.WrapText = true;
                    }

                    i++;
                    contador++;

                    List<DaiAportanteDTO> daiAportanteDTOs = _svcDevolucionAporte.GetByCriteriaDaiAportantes(prescodi, 0);
                    daiAportanteDTOs = daiAportanteDTOs.Where(a => a.Tipoempresa == tipo).ToList();

                    foreach (DaiAportanteDTO apor in daiAportanteDTOs)
                    {
                        List<DaiCalendariopagoDTO> cp = _svcDevolucionAporte.GetByCriteriaDaiCalendariopagos(apor.Aporcodi);

                        ws.Cells[string.Format("B{0}:B{0}", i)].Merge = true;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Value = contador;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("C{0}:C{0}", i)].Merge = true;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Value = apor.Emprrazsocial;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.WrapText = true;

                        for (int c = 0; c < celdas.Count; c++)
                        {
                            int aniox = anio + c + 1;

                            ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Merge = true;
                            ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.Font.Size = 11;
                            ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Value = cp.Count == 0 || !cp.Where(cx => Convert.ToInt32(cx.Caleanio) == aniox).Any() ? 0 : cp.Where(cx => Convert.ToInt32(cx.Caleanio) == aniox).FirstOrDefault().Caleamortizacion;
                            ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.WrapText = true;
                        }

                        contador++;
                        i++;
                    }

                    contador = 1;

                    if (daiAportanteDTOs.Count == 0)
                    {
                        contador++;
                        i++;
                    }
                }

                ExcelWorksheet wsi = package.Workbook.Worksheets.Add("INTERESES");
                wsi.View.ShowGridLines = true;

                wsi.Cells["B2:F2"].Merge = true;
                wsi.Cells["B2:F2"].Style.Font.Size = 11;
                wsi.Cells["B2:F2"].Style.Font.Bold = true;
                wsi.Cells["B2:F2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsi.Cells["B2:F2"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                wsi.Cells["B2:F2"].Value = "AMORTIZACIONES GENERADAS POR LOS APORTES DE INVERSIONES " + anio.ToString();
                wsi.Cells["B2:F2"].Style.WrapText = true;


                wsi.Column(2).Width = 15;
                wsi.Column(3).Width = 15;
                wsi.Column(4).Width = 15;
                wsi.Column(5).Width = 15;
                wsi.Column(6).Width = 15;
                wsi.Column(7).Width = 15;
                wsi.Column(8).Width = 15;
                wsi.Column(9).Width = 15;
                wsi.Column(10).Width = 15;
                wsi.Cells["B5:B5"].Style.Font.Size = 11;
                wsi.Cells["B5:B5"].Style.Font.Bold = true;
                wsi.Cells["B5:B5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                wsi.Cells["B5:B5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsi.Cells["B5:B5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsi.Cells["B5:B5"].Value = "N°";
                wsi.Cells["B5:B5"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsi.Cells["C5:C5"].Merge = true;
                wsi.Cells["C5:C5"].Style.Font.Size = 11;
                wsi.Cells["C5:C5"].Style.Font.Bold = true;
                wsi.Cells["C5:C5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                wsi.Cells["C5:C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsi.Cells["C5:C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsi.Cells["C5:C5"].Value = "Empresas";
                wsi.Cells["C5:C5"].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                wsi.Cells["B4:B4"].Merge = true;
                wsi.Cells["B4:B4"].Style.Font.Size = 11;
                wsi.Cells["B4:B4"].Style.Font.Bold = true;
                wsi.Cells["B4:B4"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                wsi.Cells["B4:B4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsi.Cells["B4:B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsi.Cells["B4:B4"].Value = "";
                wsi.Cells["B4:B4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                wsi.Cells["C4:C4"].Merge = true;
                wsi.Cells["C4:C4"].Style.Font.Size = 11;
                wsi.Cells["C4:C4"].Style.Font.Bold = true;
                wsi.Cells["C4:C4"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                wsi.Cells["C4:C4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsi.Cells["C4:C4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsi.Cells["C4:C4"].Value = "";
                wsi.Cells["C4:C4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                for (int c = 0; c < celdas.Count; c++)
                {
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Merge = true;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.Font.Size = 11;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Value = anio + c + 1;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 4, celdas[c])].Style.WrapText = true;

                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Merge = true;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.Font.Size = 11;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Value = "AÑO " + (c + 1).ToString();
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsi.Cells[string.Format("{1}{0}:{1}{0}", 5, celdas[c])].Style.WrapText = true;
                }

                i = 6;
                contador = 0;

                foreach (string tipo in tiposEmpresa)
                {
                    wsi.Cells[string.Format("B{0}:B{0}", i)].Merge = true;
                    wsi.Cells[string.Format("B{0}:B{0}", i)].Style.Font.Size = 11;
                    wsi.Cells[string.Format("B{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsi.Cells[string.Format("B{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsi.Cells[string.Format("B{0}:B{0}", i)].Value = contador == 0 ? "" : contador.ToString();
                    wsi.Cells[string.Format("B{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsi.Cells[string.Format("B{0}:B{0}", i)].Style.WrapText = true;

                    wsi.Cells[string.Format("C{0}:C{0}", i)].Merge = true;
                    wsi.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Size = 11;
                    wsi.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Bold = true;
                    wsi.Cells[string.Format("C{0}:C{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsi.Cells[string.Format("C{0}:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    wsi.Cells[string.Format("C{0}:C{0}", i)].Value = tipo;
                    wsi.Cells[string.Format("C{0}:C{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsi.Cells[string.Format("C{0}:C{0}", i)].Style.WrapText = true;

                    for (int c = 0; c < celdas.Count; c++)
                    {
                        wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Merge = true;
                        wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.Font.Size = 11;
                        wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Value = "";
                        wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.WrapText = true;
                    }

                    i++;
                    contador++;

                    List<DaiAportanteDTO> daiAportanteDTOs = _svcDevolucionAporte.GetByCriteriaDaiAportantes(prescodi, 0);
                    daiAportanteDTOs = daiAportanteDTOs.Where(a => a.Tipoempresa == tipo).ToList();

                    foreach (DaiAportanteDTO apor in daiAportanteDTOs)
                    {
                        List<DaiCalendariopagoDTO> cp = _svcDevolucionAporte.GetByCriteriaDaiCalendariopagos(apor.Aporcodi);

                        wsi.Cells[string.Format("B{0}:B{0}", i)].Merge = true;
                        wsi.Cells[string.Format("B{0}:B{0}", i)].Style.Font.Size = 11;
                        wsi.Cells[string.Format("B{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsi.Cells[string.Format("B{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsi.Cells[string.Format("B{0}:B{0}", i)].Value = contador;
                        wsi.Cells[string.Format("B{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsi.Cells[string.Format("B{0}:B{0}", i)].Style.WrapText = true;

                        wsi.Cells[string.Format("C{0}:C{0}", i)].Merge = true;
                        wsi.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Size = 11;
                        wsi.Cells[string.Format("C{0}:C{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsi.Cells[string.Format("C{0}:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        wsi.Cells[string.Format("C{0}:C{0}", i)].Value = apor.Emprrazsocial;
                        wsi.Cells[string.Format("C{0}:C{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsi.Cells[string.Format("C{0}:C{0}", i)].Style.WrapText = true;

                        for (int c = 0; c < celdas.Count; c++)
                        {
                            int aniox = anio + c + 1;

                            wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Merge = true;
                            wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.Font.Size = 11;
                            wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Value = cp.Count == 0 || !cp.Where(cx => Convert.ToInt32(cx.Caleanio) == aniox).Any() ? 0 : cp.Where(cx => Convert.ToInt32(cx.Caleanio) == aniox).FirstOrDefault().Caleinteres;
                            wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            wsi.Cells[string.Format("{1}{0}:{1}{0}", i, celdas[c])].Style.WrapText = true;
                        }

                        contador++;
                        i++;
                    }

                    contador = 1;

                    if (daiAportanteDTOs.Count == 0)
                    {
                        contador++;
                        i++;
                    }
                }


                var FileBytesArray = package.GetAsByteArray();
                return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre + ".xlsx");
            }
        }

        public FileContentResult DescargarCronogramaMasivo()
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.DevolucionAporteArchivos];
            //ruta = Server.MapPath(ruta);
            ruta += @"Cronograma\";
            string rutaArchivo = ruta + "Cronogramas.zip";
            var FileBytesArray = System.IO.File.ReadAllBytes(rutaArchivo);

            System.IO.File.Delete(rutaArchivo);

            return File(FileBytesArray, "application/octet-stream", "Cronogramas.zip");
        }

        public string ExportarCronogramaMasivo(DaiAportanteDTO aportanteDTO)
        {
            List<string> archivos = GenerarCronograma(aportanteDTO.Prescodi);

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.DevolucionAporteArchivos];
            //ruta = Server.MapPath(ruta);
            ruta += @"Cronograma\";

            if (!System.IO.Directory.Exists(ruta))
            {
                System.IO.Directory.CreateDirectory(ruta);
            }

            string rutaArchivo = ruta + "Cronogramas.zip";
            GeneracionZip.AddToArchive(rutaArchivo, archivos,
                    GeneracionZip.ArchiveAction.Replace,
                    GeneracionZip.Overwrite.IfNewer,
                    System.IO.Compression.CompressionLevel.Optimal);

            foreach (string archivo in archivos)
            {
                System.IO.File.Delete(archivo);
            }

            return "";
        }

        public List<string> GenerarCronograma(int prescodi)
        {
            List<DaiAportanteDTO> aportantes = _svcDevolucionAporte.GetByCriteriaDaiAportantes(prescodi, Convert.ToInt32(DaiEstadoAportante.Procesado));
            List<string> rutaArchivos = new List<string>();
            foreach (DaiAportanteDTO a in aportantes)
            {
                DaiAportanteDTO aportante = _svcDevolucionAporte.GetByIdDaiAportante(a.Aporcodi);
                DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(aportante.Prescodi);
                SiEmpresaDTO empresa = _svcDevolucionAporte.GetByIdEmpresa(aportante.Emprcodi);

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cronograma de Devolución");
                    ws.View.ShowGridLines = true;

                    ws.Cells["B2:D2"].Merge = true;
                    ws.Cells["B2:D2"].Style.Font.Size = 11;
                    ws.Cells["B2:D2"].Style.Font.Bold = true;
                    ws.Cells["B2:D2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B2:D2"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["B2:D2"].Value = "Cronograma de Devolución";
                    ws.Cells["B2:D2"].Style.WrapText = true;

                    ws.Cells["B4:B4"].Style.Font.Size = 11;
                    ws.Cells["B4:B4"].Style.Font.Bold = true;
                    ws.Cells["B4:B4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B4:B4"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["B4:B4"].Value = "Empresa:";

                    ws.Cells["C4:F4"].Merge = true;
                    ws.Cells["C4:F4"].Style.Font.Size = 11;
                    ws.Cells["C4:F4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["C4:F4"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["C4:F4"].Value = aportante.Emprrazsocial;

                    ws.Cells["B5:B5"].Style.Font.Size = 11;
                    ws.Cells["B5:B5"].Style.Font.Bold = true;
                    ws.Cells["B5:B5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B5:B5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["B5:B5"].Value = "Año inversión:";

                    ws.Cells["C5:C5"].Merge = true;
                    ws.Cells["C5:C5"].Style.Font.Size = 11;
                    ws.Cells["C5:C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["C5:C5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["C5:C5"].Value = presupuesto.Presanio;

                    ws.Cells["D5:D5"].Style.Font.Size = 11;
                    ws.Cells["D5:D5"].Style.Font.Bold = true;
                    ws.Cells["D5:D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["D5:D5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["D5:D5"].Value = "Monto:";

                    ws.Cells["E5:E5"].Merge = true;
                    ws.Cells["E5:E5"].Style.Font.Size = 11;
                    ws.Cells["E5:E5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["E5:E5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells["E5:E5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["E5:E5"].Value = string.Format("{0:n}", presupuesto.Presmonto);

                    ws.Cells["F5:F5"].Style.Font.Size = 11;
                    ws.Cells["F5:F5"].Style.Font.Bold = true;
                    ws.Cells["F5:F5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["F5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["F5:F5"].Value = "Cuotas:";

                    ws.Cells["G5:G5"].Merge = true;
                    ws.Cells["G5:G5"].Style.Font.Size = 11;
                    ws.Cells["G5:G5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells["G5:G5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["G5:G5"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["G5:G5"].Value = presupuesto.Presamortizacion;

                    ws.Cells["B6:B6"].Style.Font.Size = 11;
                    ws.Cells["B6:B6"].Style.Font.Bold = true;
                    ws.Cells["B6:B6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B6:B6"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["B6:B6"].Value = "Monto Aportado:";

                    ws.Cells["C6:C6"].Merge = true;
                    ws.Cells["C6:C6"].Style.Font.Size = 11;
                    ws.Cells["C6:C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["C6:C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells["C6:C6"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["C6:C6"].Value = string.Format("{0:n}", aportante.Apormontoparticipacion);

                    ws.Cells["D6:D6"].Style.Font.Size = 11;
                    ws.Cells["D6:D6"].Style.Font.Bold = true;
                    ws.Cells["D6:D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["D6:D6"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["D6:D6"].Value = "Años sin Aportar:";

                    ws.Cells["E6:E6"].Merge = true;
                    ws.Cells["E6:E6"].Style.Font.Size = 11;
                    ws.Cells["E6:E6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["E6:E6"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["E6:E6"].Value = aportante.Aporaniosinaporte;

                    ws.Column(2).Width = 15;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Cells["B8:B8"].Style.Font.Size = 11;
                    ws.Cells["B8:B8"].Style.Font.Bold = true;
                    ws.Cells["B8:B8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["B8:B8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B8:B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["B8:B8"].Value = "N°";
                    ws.Cells["B8:B8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["C8:C8"].Merge = true;
                    ws.Cells["C8:C8"].Style.Font.Size = 11;
                    ws.Cells["C8:C8"].Style.Font.Bold = true;
                    ws.Cells["C8:C8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["C8:C8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["C8:C8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["C8:C8"].Value = "Año";
                    ws.Cells["C8:C8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["D8:D8"].Merge = true;
                    ws.Cells["D8:D8"].Style.Font.Size = 11;
                    ws.Cells["D8:D8"].Style.Font.Bold = true;
                    ws.Cells["D8:D8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["D8:D8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["D8:D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["D8:D8"].Value = "Capital";
                    ws.Cells["D8:D8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["E8:E8"].Merge = true;
                    ws.Cells["E8:E8"].Style.Font.Size = 11;
                    ws.Cells["E8:E8"].Style.Font.Bold = true;
                    ws.Cells["E8:E8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["E8:E8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["E8:E8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["E8:E8"].Value = "Interés";
                    ws.Cells["E8:E8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["F8:F8"].Merge = true;
                    ws.Cells["F8:F8"].Style.Font.Size = 11;
                    ws.Cells["F8:F8"].Style.Font.Bold = true;
                    ws.Cells["F8:F8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["F8:F8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["F8:F8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["F8:F8"].Value = "Amortización";
                    ws.Cells["F8:F8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["G8:G8"].Merge = true;
                    ws.Cells["G8:G8"].Style.Font.Size = 11;
                    ws.Cells["G8:G8"].Style.Font.Bold = true;
                    ws.Cells["G8:G8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["G8:G8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["G8:G8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["G8:G8"].Value = "Saldo";
                    ws.Cells["G8:G8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["H8:H8"].Merge = true;
                    ws.Cells["H8:H8"].Style.Font.Size = 11;
                    ws.Cells["H8:H8"].Style.Font.Bold = true;
                    ws.Cells["H8:H8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["H8:H8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["H8:H8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["H8:H8"].Value = "Carta pago";
                    ws.Cells["H8:H8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["I8:I8"].Merge = true;
                    ws.Cells["I8:I8"].Style.Font.Size = 11;
                    ws.Cells["I8:I8"].Style.Font.Bold = true;
                    ws.Cells["I8:I8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["I8:I8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["I8:I8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["I8:I8"].Value = "N° Cheque Amortización";
                    ws.Cells["I8:I8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["J8:J8"].Merge = true;
                    ws.Cells["J8:J8"].Style.Font.Size = 11;
                    ws.Cells["J8:J8"].Style.Font.Bold = true;
                    ws.Cells["J8:J8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["J8:J8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["J8:J8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["J8:J8"].Value = "N° Cheque de Interes";
                    ws.Cells["J8:J8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["K8:K8"].Merge = true;
                    ws.Cells["K8:K8"].Style.Font.Size = 11;
                    ws.Cells["K8:K8"].Style.Font.Bold = true;
                    ws.Cells["K8:K8"].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    ws.Cells["K8:K8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["K8:K8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["K8:K8"].Value = "Estado";
                    ws.Cells["K8:K8"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    List<DaiCalendariopagoDTO> calendarioPago = _svcDevolucionAporte.GetByCriteriaDaiCalendariopagos(a.Aporcodi);

                    int i = 9;
                    int contador = 1;

                    foreach (DaiCalendariopagoDTO item in calendarioPago)
                    {
                        ws.Cells[string.Format("B{0}:B{0}", i)].Merge = true;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Value = contador;
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("B{0}:B{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("C{0}:C{0}", i)].Merge = true;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Value = item.Caleanio;
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("C{0}:C{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("D{0}:D{0}", i)].Merge = true;
                        ws.Cells[string.Format("D{0}:D{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("D{0}:D{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("D{0}:D{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[string.Format("D{0}:D{0}", i)].Value = string.Format("{0:n}", item.Calecapital);
                        ws.Cells[string.Format("D{0}:D{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("D{0}:D{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("E{0}:E{0}", i)].Merge = true;
                        ws.Cells[string.Format("E{0}:E{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("E{0}:E{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("E{0}:E{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[string.Format("E{0}:E{0}", i)].Value = string.Format("{0:n}", item.Caleinteres);
                        ws.Cells[string.Format("E{0}:E{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("E{0}:E{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("F{0}:F{0}", i)].Merge = true;
                        ws.Cells[string.Format("F{0}:F{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("F{0}:F{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("F{0}:F{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[string.Format("F{0}:F{0}", i)].Value = string.Format("{0:n}", item.Caleamortizacion);
                        ws.Cells[string.Format("F{0}:F{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("F{0}:F{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("G{0}:G{0}", i)].Merge = true;
                        ws.Cells[string.Format("G{0}:G{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("G{0}:G{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("G{0}:G{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[string.Format("G{0}:G{0}", i)].Value = string.Format("{0:n}", item.Caletotal);
                        ws.Cells[string.Format("G{0}:G{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("G{0}:G{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("H{0}:H{0}", i)].Merge = true;
                        ws.Cells[string.Format("H{0}:H{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("H{0}:H{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("H{0}:H{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[string.Format("H{0}:H{0}", i)].Value = item.Calecartapago;
                        ws.Cells[string.Format("H{0}:H{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("H{0}:H{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("I{0}:I{0}", i)].Merge = true;
                        ws.Cells[string.Format("I{0}:I{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("I{0}:I{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("I{0}:I{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[string.Format("I{0}:I{0}", i)].Value = item.Calechequeamortpago;
                        ws.Cells[string.Format("I{0}:I{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("I{0}:I{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("J{0}:J{0}", i)].Merge = true;
                        ws.Cells[string.Format("J{0}:J{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("J{0}:J{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("J{0}:J{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[string.Format("J{0}:J{0}", i)].Value = item.Calechequeintpago;
                        ws.Cells[string.Format("J{0}:J{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("J{0}:J{0}", i)].Style.WrapText = true;

                        ws.Cells[string.Format("K{0}:K{0}", i)].Merge = true;
                        ws.Cells[string.Format("K{0}:K{0}", i)].Style.Font.Size = 11;
                        ws.Cells[string.Format("K{0}:K{0}", i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells[string.Format("K{0}:K{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Cells[string.Format("K{0}:K{0}", i)].Value = item.Tabddescripcion;
                        ws.Cells[string.Format("K{0}:K{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("K{0}:K{0}", i)].Style.WrapText = true;

                        i += 1;
                        contador++;
                    }

                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.DevolucionAporteArchivos];
                    //ruta = Server.MapPath(ruta);
                    ruta += @"Cronograma\";
                    if (!System.IO.Directory.Exists(ruta))
                    {
                        System.IO.Directory.CreateDirectory(ruta);
                    }

                    if (!System.IO.Directory.Exists(ruta))
                    {
                        System.IO.Directory.CreateDirectory(ruta);
                    }

                    string rutaArchivo = ruta + empresa.Emprrazsocial.Replace(" ", "_") + ".xlsx";
                    System.IO.File.WriteAllBytes(rutaArchivo, package.GetAsByteArray());

                    rutaArchivos.Add(rutaArchivo);
                }
            }

            return rutaArchivos;
        }

        public decimal CalcularMonto(decimal number, decimal percent)
        {
            //return ((double) 80         *       25)/100;
            return ((decimal)number * percent) / 100;
        }

        private double CalcularCuota(double monto, int mesesPlazo, double taza)
        {      //PMT = -RATE * ( FV + PV * Math.pow(1+RATE,NPER)) / ((Math.pow(1+RATE,NPER)-1));//double Monto = 100000;      int Plazos = 60;      double taza = 0.02;
            double t = taza / 100;
            double b = Math.Pow((1 + t), mesesPlazo);
            return t * monto * b / (b - 1);
        }
    }
}
