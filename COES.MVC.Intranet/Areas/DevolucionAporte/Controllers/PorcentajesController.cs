using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.DevolucionAportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using OfficeOpenXml.Style;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
using System.Configuration;
using log4net;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.App_Start;

namespace COES.MVC.Intranet.Areas.DevolucionAporte.Controllers
{
    [ValidarSesion]
    public class PorcentajesController : BaseController
    {
        DevolucionAportesAppServicio _svcDevolucionAporte = new DevolucionAportesAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PorcentajesController));
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("PorcentajesController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("PorcentajesController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            ViewBag.ListaAnioInversion = _svcDevolucionAporte.GetByCriteriaDaiPresupuestos();
            return View();
        }

        public ActionResult ListadoPorcentajes(int prescodi)
        {
            List<DaiAportanteDTO> listadoAportantes = new List<DaiAportanteDTO>();

            try
            {
                listadoAportantes = _svcDevolucionAporte.GetByCriteriaDaiAportantes(prescodi, 0);
            }
            catch (Exception ex)
            {
                log.Error("ListadoPorcentajes", ex);
            }
            
            return PartialView(listadoAportantes);
        }

        public FileContentResult DescargarEmpresas(int prescodi)
        {
            base.ValidarSesionUsuario();

            List<DaiAportanteDTO> aportantes = _svcDevolucionAporte.GetByCriteriaDaiAportantes(prescodi, 0);

            string nombre = "Empresas";

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Empresas");
                ws.View.ShowGridLines = true;

                ws.Cells[1, 1].Value = "CÓDIGO";
                ws.Cells[1, 2].Value = "NOMBRE";
                ws.Cells[1, 3].Value = "TIPO EMPRESA";
                ws.Cells[1, 4].Value = "RUC";
                ws.Cells[1, 5].Value = "RAZÓN SOCIAL";
                ws.Cells[1, 6].Value = "PORCENTAJE(%)";

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#BFBFBF");

                ws.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(colFromHex);

                List<Dominio.DTO.Sic.SiEmpresaDTO> listadoEmpresa = _svcDevolucionAporte.ListarEmpresaDevolucion();
                listadoEmpresa = listadoEmpresa.Where(e => e.Emprcoes == "S" && e.Emprestado == "A").ToList();

                if (aportantes.Any())
                {
                    listadoEmpresa = listadoEmpresa.Join(aportantes,
                                                   e => e.Emprcodi,
                                                   a => a.Emprcodi,
                                                   (empresa, aportante) => new { empresa, aportante }).Select(e => new Dominio.DTO.Sic.SiEmpresaDTO
                                                   {
                                                       Emprcodi = e.empresa.Emprcodi,
                                                       Emprnomb = e.empresa.Emprnomb,
                                                       Emprruc = e.empresa.Emprruc,
                                                       Emprrazsocial = e.empresa.Emprrazsocial,
                                                       Tipoemprdesc = e.empresa.Tipoemprdesc,
                                                       Porcentaje = e.aportante.Aporporcentajeparticipacion.HasValue ? e.aportante.Aporporcentajeparticipacion.Value : 0
                                                   }).ToList();
                }

                int fila = 2;
                foreach (Dominio.DTO.Sic.SiEmpresaDTO item in listadoEmpresa)
                {
                    ws.Cells[fila, 1].Value = item.Emprcodi;
                    ws.Cells[fila, 2].Value = item.Emprnomb;
                    ws.Cells[fila, 3].Value = item.Tipoemprdesc;
                    ws.Cells[fila, 4].Value = item.Emprruc;
                    ws.Cells[fila, 5].Value = item.Emprrazsocial;
                    ws.Cells[fila, 6].Value = item.Porcentaje == 0 ? "" : item.Porcentaje.ToString();

                    fila++;
                }

                var memorystream = new MemoryStream();
                //package.SaveAs(memorystream);

                var FileBytesArray = package.GetAsByteArray();
                return File(FileBytesArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre + ".xlsx");
            }
        }

        int CountDecimalDigits(decimal n)
        {
            return n.ToString(System.Globalization.CultureInfo.InvariantCulture)
                    //.TrimEnd('0') uncomment if you don't want to count trailing zeroes
                    .SkipWhile(c => c != '.')
                    .Skip(1)
                    .Count();
        }

        public JsonResult ImportarAportantes()
        {
            try
            {
                List<SiEmpresaDTO> listadoImportarEmpresa = new List<SiEmpresaDTO>();

                string prescodi = Request["prescodi"];

                int id = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    int lastIndex = file.FileName.LastIndexOf(Constantes.CaracterPunto);
                    string descripcion = file.FileName;
                    string extension = file.FileName.Substring(lastIndex + 1, file.FileName.Length - lastIndex - 1);

                    DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(Convert.ToInt32(prescodi));

                    //string ruta = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[RutaDirectorio.DevolucionAporteArchivos]);
                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.DevolucionAporteArchivos];

                    if (!System.IO.Directory.Exists(ruta))
                    {
                        System.IO.Directory.CreateDirectory(ruta);
                    }

                    ruta += @"\aportantes\";

                    if (!System.IO.Directory.Exists(ruta))
                    {
                        System.IO.Directory.CreateDirectory(ruta);
                    }

                    string rutaArchivo = ruta + file.FileName;
                    file.SaveAs(rutaArchivo);

                    List<Dominio.DTO.Sic.SiEmpresaDTO> listadoEmpresa = _svcDevolucionAporte.ListarEmpresaDevolucion();
                    listadoEmpresa = listadoEmpresa.Where(e => e.Emprcoes == "S" && e.Emprestado == "A").ToList();

                    using (ExcelPackage package = new ExcelPackage(new FileInfo(rutaArchivo)))
                    {
                        ExcelWorksheet ws = package.Workbook.Worksheets[1];
                        ws.View.ShowGridLines = true;

                        for (int f = ws.Dimension.Start.Row + 1; f <= ws.Dimension.End.Row; f++)
                        {
                            string codigoEmpresa = ws.Cells[f, 1].Text;
                            string nombreEmpresa = ws.Cells[f, 2].Text;
                            string tipoEmpresa = ws.Cells[f, 3].Text;
                            string rucEmpresa = ws.Cells[f, 4].Text;
                            string rzEmpresa = ws.Cells[f, 5].Text;
                            string porcentaje = ws.Cells[f, 6].Text;

                            if (string.IsNullOrEmpty(porcentaje))
                            {
                                continue;
                            }

                            int estado = Convert.ToInt32(DaiEstadoRegistroAportanteImportado.Correcto);

                            if (string.IsNullOrEmpty(codigoEmpresa))
                            {
                                estado = Convert.ToInt32(DaiEstadoRegistroAportanteImportado.Error);
                            }
                            else
                            {
                                int nCodigoEmpresa;
                                decimal nPorcentaje;
                                bool isNumCodigoEmpresa = int.TryParse(codigoEmpresa, out nCodigoEmpresa);
                                bool isNumPorcentaje = decimal.TryParse(porcentaje, out nPorcentaje);

                                if (!isNumCodigoEmpresa || !isNumPorcentaje)
                                {
                                    estado = Convert.ToInt32(DaiEstadoRegistroAportanteImportado.Error);
                                }
                            }

                            int empresaCodi = 0;
                            decimal dporcentaje = Convert.ToDecimal(porcentaje);

                            int digitos = CountDecimalDigits(dporcentaje);
                            decimal calculoParticipacion = 0;

                            //if (digitos > 2)
                            //{
                            //    calculoParticipacion = Convert.ToDecimal(presupuesto.Presmonto * dporcentaje);
                            //}
                            //else {
                            //    calculoParticipacion = Convert.ToDecimal(presupuesto.Presmonto * (dporcentaje / 100));
                            //}

                            calculoParticipacion = Convert.ToDecimal(presupuesto.Presmonto * (dporcentaje / 100));
                            //decimal calculoParticipacion = Convert.ToDecimal(presupuesto.Presmonto * Convert.ToDecimal(porcentaje));

                            if (listadoEmpresa.Any())
                            {
                                SiEmpresaDTO empresa = listadoEmpresa.FirstOrDefault(e => e.Emprcodi == Convert.ToInt32(codigoEmpresa));
                                if (empresa != null)
                                {
                                    empresaCodi = empresa.Emprcodi;
                                }
                                else
                                {
                                    estado = Convert.ToInt32(DaiEstadoRegistroAportanteImportado.Error);
                                }
                            }
                            else
                            {
                                estado = Convert.ToInt32(DaiEstadoRegistroAportanteImportado.Error);
                            }

                            listadoImportarEmpresa.Add(new SiEmpresaDTO
                            {
                                Emprcodi = empresaCodi,
                                Emprnomb = nombreEmpresa,
                                Tipoemprdesc = tipoEmpresa,
                                Emprruc = rucEmpresa,
                                Emprrazsocial = rzEmpresa,
                                Porcentaje = Convert.ToDecimal(porcentaje),
                                Apormontoparticipacion = Convert.ToDecimal(calculoParticipacion).ToString("#,##0.00000000"),
                                Estado = estado
                            });
                        }
                    }

                    System.IO.File.Delete(rutaArchivo);
                }

                return Json(listadoImportarEmpresa.OrderBy(e => e.Emprnomb).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("ImportarAportantes", ex);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult SubirAportantes(int emprcodi, string porcentaje, int prescodi)
        //{
        //    try
        //    {
        //        DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(Convert.ToInt32(prescodi));

        //        DaiAportanteDTO aportante = new DaiAportanteDTO
        //        {
        //            Emprcodi = emprcodi,
        //            Prescodi = prescodi,
        //            Apormontoparticipacion = presupuesto.Presmonto * (Convert.ToDecimal(porcentaje) / 100),
        //            Tabcdcodiestado = Convert.ToInt32(DaiEstadoAportante.SinProcesar),
        //            Aporactivo = Convert.ToInt32(DaiEstadoRegistro.Activo).ToString(),
        //            Aporporcentajeparticipacion = Convert.ToDecimal(porcentaje),
        //            Aporusucreacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin,
        //            Aprofeccreacion = DateTime.Now
        //        };

        //        _svcDevolucionAporte.SaveDaiAportante(aportante);

        //        return Json(new { resultado = 1, mensaje = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(Servicios.Aplicacion.Helper.ConstantesAppServicio.LogError, ex);
        //        return Json(new { resultado = 0, mensaje = "Error" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult SubirAportantes(List<DaiAportanteDTO> aportantes)
        {
            try
            {
                DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(aportantes[0].Prescodi);

                if (aportantes.Any()) {
                    _svcDevolucionAporte.DeleteByPresupuesto(new DaiAportanteDTO
                    {
                        Prescodi = presupuesto.Prescodi,
                        Aporfecmodificacion = DateTime.Now,
                        Aporusumodificacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin
                    });
                }

                foreach (var item in aportantes)
                {
                    decimal porcentaje = Convert.ToDecimal(item.Porcentaje);

                    DaiAportanteDTO daportante = new DaiAportanteDTO
                    {
                        Emprcodi = item.Emprcodi,
                        Prescodi = item.Prescodi,
                        Apormontoparticipacion = Convert.ToDecimal(presupuesto.Presmonto * (porcentaje / 100)),
                        Tabcdcodiestado = Convert.ToInt32(DaiEstadoAportante.SinProcesar),
                        Aporactivo = Convert.ToInt32(DaiEstadoRegistro.Activo).ToString(),
                        Aporporcentajeparticipacion = Convert.ToDecimal(item.Porcentaje),
                        Aporusucreacion = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin,
                        Aprofeccreacion = DateTime.Now
                    };

                    _svcDevolucionAporte.SaveDaiAportante(daportante);
                }
                
                return Json(new { resultado = 1, mensaje = "Error" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("SubirAportantes", ex);
                return Json(new { resultado = 0, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerPresupuesto(int prescodi)
        {
            try
            {
                DaiPresupuestoDTO presupuesto = _svcDevolucionAporte.GetByIdDaiPresupuesto(prescodi);

                return Json(presupuesto, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("ObtenerPresupuesto", ex);
                return Json(new { resultado = 0, mensaje = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class Aportantes
    {
        public string prescodi { get; set; }
        public string emprcodi { get; set; }
        public string porcentaje { get; set; }
    }
}
