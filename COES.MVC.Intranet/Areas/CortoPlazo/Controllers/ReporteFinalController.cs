using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class ReporteFinalController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        ReporteFinalAppServicio servicio = new ReporteFinalAppServicio();

        #region Configuracion

        /// <summary>
        /// Página Principal de Configuración
        /// </summary>
        /// <returns></returns>
        public ActionResult Configuracion()
        {
            ReporteFinalModel model = new ReporteFinalModel();
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite obtener la configuración por fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerConfiguracion(string fecha)
        {
            return Json(this.servicio.ObtenerConfiguracionPorFecha(DateTime.ParseExact(fecha,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Pantalla de relaciones de barras
        /// </summary>
        /// <param name="idConfiguracion"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ConfiguracionBarra(int idConfiguracion, int tipo, string fecha)
        {
            ReporteFinalModel model = new ReporteFinalModel();

            model.ListaBarrasEMS = this.servicio.ObtenerBarrasEMS();
            model.ListaBarrasTransf = this.servicio.ObtenerBarrasTransferencia();
            model.ListaBarrasDesconocida = this.servicio.ObtenerBarrasTransferenciaDesconocida(DateTime.ParseExact(
                fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture));

            if (idConfiguracion != 0)
            {
                CmBarraRelacionDTO entity = this.servicio.GetByIdCmBarraRelacion(idConfiguracion);
                model.FechaVigencia = (entity.Cmbarevigencia != null) ? ((DateTime)entity.Cmbarevigencia).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                model.FechaExpiracion = (entity.Cmbareexpira != null) ? ((DateTime)entity.Cmbareexpira).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                model.Entidad = entity;
                model.TipoRegistro = entity.Cmbaretipreg;
            }
            else
            {
                CmBarraRelacionDTO entity = new CmBarraRelacionDTO();
                model.FechaVigencia = DateTime.Now.ToString(Constantes.FormatoFecha);
                model.FechaExpiracion = string.Empty;
                model.Entidad = entity;
                entity.Barrcodi = -1;
                entity.Barrcodi2 = -1;
                entity.Cnfbarcodi = -1;
                entity.Cnfbarcodi2 = -1;
                model.TipoRegistro = tipo.ToString();
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar la relacion de barras
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarBarra(ReporteFinalModel model)
        {
            CmBarraRelacionDTO entity = new CmBarraRelacionDTO()
            {
                Cmbarevigencia = (!string.IsNullOrEmpty(model.FechaVigencia)) ? (DateTime?)(DateTime.ParseExact(model.FechaVigencia,
                 Constantes.FormatoFecha, CultureInfo.InvariantCulture)) : null,
                Cmbareexpira = (!string.IsNullOrEmpty(model.FechaExpiracion)) ? (DateTime?)(DateTime.ParseExact(model.FechaExpiracion,
                 Constantes.FormatoFecha, CultureInfo.InvariantCulture)) : null,
                Cmbaretipreg = model.TipoRegistro,
                Cmbaretiprel = model.TipoRelacion,
                Cnfbarcodi = model.BarraEMS,
                Cnfbarcodi2 = (model.BarraEMS2 == 0)?null:(int?)model.BarraEMS2,
                Barrcodi = (model.TipoRegistro == 1.ToString() && model.TipoRelacion == 1.ToString()) ? model.BarraDesconocida :
                    model.BarraTransferencia,
                Barrcodi2 = model.BarraTransferencia2,
                Cmbareestado = Constantes.EstadoActivo,
                Cmbareusucreacion = base.UserName,
                Cmbarecodi = model.CodigoBarra,
                Cmbarereporte = model.MostrarReporte
            };

            return Json(this.servicio.GrabarConfiguracionBarra(entity));

        }

        /// <summary>
        /// Permite configurar las barras adicionales
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AddBarraEms(int idRelacion, int idBarra)
        {
            ReporteFinalModel model = new ReporteFinalModel();
            model.ListaBarrasEMS = this.servicio.ObtenerBarrasEMS();
            model.ListaBarraEMSAdicionales = this.servicio.ObtenerBarrasEMSAdicionales(idRelacion);
            model.IdBarraEMS = idBarra;
            model.CodigoBarra = idRelacion;
            return PartialView(model);
        }

        /// <summary>
        /// Histórico de configuracion de barras
        /// </summary>
        /// <param name="idBarra"></param>
        /// <param name="tipoRegistro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult HistoricoBarra(int idBarra, string tipoRegistro)
        {
            ReporteFinalModel model = new ReporteFinalModel();
            model.ListadoHistorico = this.servicio.ObtenerHistoricoBarra(idBarra, tipoRegistro);
            model.TipoRegistro = tipoRegistro;
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la configuracion de una barra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarBarra(int id)
        {
            return Json(this.servicio.DeleteCmBarraRelacion(id));
        }
                            
        /// <summary>
        /// Pantalla de relaciones de barras
        /// </summary>
        /// <param name="idConfiguracion"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ConfiguracionPeriodo(int idConfiguracion)
        {
            ReporteFinalModel model = new ReporteFinalModel();

            if (idConfiguracion != 0)
            {
                CmPeriodoDTO entity = this.servicio.GetByIdCmPeriodo(idConfiguracion);
                model.FechaVigencia = (entity.Cmpervigencia != null) ? ((DateTime)entity.Cmpervigencia).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                model.FechaExpiracion = (entity.Cmperexpira != null) ? ((DateTime)entity.Cmperexpira).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                model.EntidadPeriodo = entity;               
            }
            else
            {
                CmPeriodoDTO entity = new CmPeriodoDTO();
                model.FechaVigencia = DateTime.Now.ToString(Constantes.FormatoFecha);
                model.FechaExpiracion = string.Empty;
                model.EntidadPeriodo = entity;              
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar la relacion de barras
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarPeriodo(ReporteFinalModel model)
        {
            CmPeriodoDTO entity = new CmPeriodoDTO()
            {
                Cmpervigencia = (!string.IsNullOrEmpty(model.FechaVigencia)) ? (DateTime?)(DateTime.ParseExact(model.FechaVigencia,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)) : null,
                Cmperexpira = (!string.IsNullOrEmpty(model.FechaExpiracion)) ? (DateTime?)(DateTime.ParseExact(model.FechaExpiracion,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)) : null,
                Cmperbase = model.PeriodoBase,
                Cmpermedia = model.PeriodoMedia,
                Cmperpunta = model.PeriodoPunta,
                Cmperestado = Constantes.EstadoActivo,
                Cmperusucreacion = base.UserName,
                Cmpercodi = model.CodigoPeriodo
            };

            return Json(this.servicio.GrabarConfiguracionPeriodo(entity));

        }

        /// <summary>
        /// Histórico de configuracion de barras
        /// </summary>
        /// <param name="idBarra"></param>
        /// <param name="tipoRegistro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult HistoricoPeriodo()
        {
            ReporteFinalModel model = new ReporteFinalModel();
            model.ListaHistoricoPeriodo = this.servicio.ObtenerHistoricoPeriodo();           
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la configuracion de una barra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarPeriodo(int id)
        {
            return Json(this.servicio.DeleteCmPeriodo(id));
        }

        /// <summary>
        /// Pantalla de relaciones de barras
        /// </summary>
        /// <param name="idConfiguracion"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ConfiguracionUmbral(int idConfiguracion)
        {
            ReporteFinalModel model = new ReporteFinalModel();

            if (idConfiguracion != 0)
            {
                CmUmbralreporteDTO entity = this.servicio.GetByIdCmUmbralreporte(idConfiguracion);
                model.FechaVigencia = (entity.Cmurvigencia != null) ? ((DateTime)entity.Cmurvigencia).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                model.FechaExpiracion = (entity.Cmurexpira != null) ? ((DateTime)entity.Cmurexpira).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                model.EntidadUmbral = entity;
            }
            else
            {
                CmUmbralreporteDTO entity = new CmUmbralreporteDTO();
                model.FechaVigencia = DateTime.Now.ToString(Constantes.FormatoFecha);
                model.FechaExpiracion = string.Empty;
                model.EntidadUmbral = entity;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar la relacion de barras
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarUmbral(ReporteFinalModel model)
        {
            CmUmbralreporteDTO entity = new CmUmbralreporteDTO()
            {
                Cmurvigencia = (!string.IsNullOrEmpty(model.FechaVigencia)) ? (DateTime?)(DateTime.ParseExact(model.FechaVigencia,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)) : null,
                Cmurexpira = (!string.IsNullOrEmpty(model.FechaExpiracion)) ? (DateTime?)(DateTime.ParseExact(model.FechaExpiracion,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)) : null,
                Cmurmaxbarra = model.TotalMaximo,
                Cmurminbarra = model.TotalMinimo,
                Cmurmaxenergia = model.EnergiaMaximo,
                Cmurminenergia = model.EnergiaMinimo,
                Cmurmaxconges = model.CongestionMaximo,
                Cmurminconges = model.CongestionMinimo,
                Cmurdiferencia = model.UmbralDiferencia,
                Cmurestado = Constantes.EstadoActivo,
                Cmurusucreacion = base.UserName,
                Cmurcodi = model.CodigoUmbral
            };

            return Json(this.servicio.GrabarConfiguracionUmbral(entity));

        }

        /// <summary>
        /// Histórico de configuracion de barras
        /// </summary>
        /// <param name="idBarra"></param>
        /// <param name="tipoRegistro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult HistoricoUmbral()
        {
            ReporteFinalModel model = new ReporteFinalModel();
            model.ListaHistoricoUmbral = this.servicio.ObtenerHistoricoUmbral();
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la configuracion de una barra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarUmbral(int id)
        {
            return Json(this.servicio.DeleteCmUmbralreporte(id));
        }

        /// <summary>
        /// Pantalla de relaciones de equipos
        /// </summary>
        /// <param name="idConfiguracion"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ConfiguracionEquipo(int idConfiguracion, string fecha)
        {
            ReporteFinalModel model = new ReporteFinalModel();
            model.ListaEquiposCongestion = this.servicio.ObtenerEquiposCongestion().OrderBy(x=>x.Equinomb).ToList();
            model.ListaBarraAdicional = this.servicio.ObtenerBarrasAdicionales(DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture));

            if (idConfiguracion != 0)
            {
                CmEquipobarraDTO entity = this.servicio.GetByIdCmEquipobarra(idConfiguracion);
                model.FechaVigencia = (entity.Cmeqbavigencia != null) ? ((DateTime)entity.Cmeqbavigencia).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                model.FechaExpiracion = (entity.Cmeqbaexpira != null) ? ((DateTime)entity.Cmeqbaexpira).ToString(
                    Constantes.FormatoFecha) : string.Empty;
                model.EntidadEquipo = entity;
            }
            else
            {
                CmEquipobarraDTO entity = new CmEquipobarraDTO();
                model.FechaVigencia = DateTime.Now.ToString(Constantes.FormatoFecha);
                model.FechaExpiracion = string.Empty;
                entity.ListaDetalle = new List<CmEquipobarraDetDTO>();
                model.EntidadEquipo = entity;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar la relacion de barras
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarEquipo(ReporteFinalModel model)
        {
            CmEquipobarraDTO entity = new CmEquipobarraDTO()
            {
                Cmeqbavigencia = (!string.IsNullOrEmpty(model.FechaVigencia)) ? (DateTime?)(DateTime.ParseExact(model.FechaVigencia,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)) : null,
                Cmeqbaexpira = (!string.IsNullOrEmpty(model.FechaExpiracion)) ? (DateTime?)(DateTime.ParseExact(model.FechaExpiracion,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)) : null,
                Configcodi = model.Equipo,
                Barras = model.IdsBarras,
                Cmeqbaestado = Constantes.EstadoActivo,
                Cmeqbausucreacion = base.UserName,
                Cmeqbacodi = model.CodigoEquipo
            };

            return Json(this.servicio.GrabarConfiguracionEquipo(entity));

        }

        /// <summary>
        /// Permite grabar la relacion de barras
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarBarraEmsAdicional(ReporteFinalModel model)
        {
            CmBarraRelacionDTO entity = new CmBarraRelacionDTO()
            {
                Cmbarecodi = model.CodigoBarra,               
                Barras = model.IdsBarrasEMSAdicional,               
                Cmbareusucreacion = base.UserName
            };

            return Json(this.servicio.GraberConfiguracionBarraEMSAdicional(entity));

        }        

        /// <summary>
        /// Histórico de configuracion de barras
        /// </summary>
        /// <param name="idBarra"></param>
        /// <param name="tipoRegistro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult HistoricoEquipo(int id)
        {
            ReporteFinalModel model = new ReporteFinalModel();
            model.ListHistoricoEquipo = this.servicio.ObtenerHistoricoEquipo(id);
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la configuracion de un equipo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEquipo(int id)
        {
            return Json(this.servicio.DeleteCmEquipobarra(id));
        }

        #endregion

        #region Factores de pérdida

        public ActionResult FactorPerdida()
        {
            FactorPerdidaModel model = new FactorPerdidaModel();
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite obtener los datos de perdidas margiales
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFactoresPerdida(string fecha)
        {
            return Json(this.servicio.CargarFactoresPerdida(DateTime.ParseExact(fecha,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Permite almacenar los datos de FPM
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarFactoresPerdida(string fecha, string[][] data)
        {
            return Json(this.servicio.GrabarFactoresPerdida(DateTime.ParseExact(fecha,
                  Constantes.FormatoFecha, CultureInfo.InvariantCulture), base.UserName, data));
        }

        /// <summary>
        /// Permite descargar el formato de carga de FPM
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormatoFPM(string[][] data, string fecha)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            string file = ConstantesCortoPlazo.FormatoCargaFPM;

            return Json(this.servicio.GenerarFormatoFPM(data, path, file, DateTime.ParseExact(fecha, 
                Constantes.FormatoFecha, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormatoFPM(string fecha)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.FormatoCargaFPM;
            return File(fullPath, Constantes.AppExcel, string.Format(ConstantesCortoPlazo.FormatoCargaFPM, fecha));
        }


        /// <summary>
        /// Permite cargar el archivo de FPM
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFOM()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + ConstantesCortoPlazo.FormatoUploadFMP;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite cargar la potencia desde excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFPMFromFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.FormatoUploadFMP;
            return Json(this.servicio.CargarFactorPerdidaFormato(path));
        }

        #endregion

        #region Reporte Final

        /// <summary>
        /// Muestra la pantalla de reportes
        /// </summary>
        /// <returns></returns>
        public ActionResult Reporte() 
        {
            VersionReporteModel model = new VersionReporteModel();
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite consultar las versiones de los reportes
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ReporteList(string fechaInicio, string fechaFin)
        {
            VersionReporteModel model = new VersionReporteModel();
            model.ListaReporte = this.servicio.ObtenerBusquedaReporte(
                DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture));
            return PartialView(model);

        }

        /// <summary>
        /// Permite generar el reporte final
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarVersionReporte(string fecha) 
        {
            return Json(this.servicio.GenerarReporteFinal(DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture), base.UserName));
        }

        /// <summary>
        /// Permite generar el reporte final
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteExcel(string fecha, int idReporte, int tipo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            return Json(this.servicio.GenerarArchivoReporte(DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture), idReporte, tipo, path));
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        #endregion


        /// <summary>
        /// Muestra la pantalla de reportes
        /// </summary>
        /// <returns></returns>
        public ActionResult BarraDesenergizada()
        {
            VersionReporteModel model = new VersionReporteModel();
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);           
            return View(model);
        }

        /// <summary>
        /// Permite consultar las versiones de los reportes
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BarraDesenergizadaList(string fechaInicio, string fechaFin)
        {
            VersionReporteModel model = new VersionReporteModel();
            model.ListaReporteBarra = this.servicio.ObtenerDataReporteBarra(
                DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture));
            return PartialView(model);

        }

        /// <summary>
        /// Permite generar el reporte 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarBarraDesenergizada(string fechaInicio, string fechaFin)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
                string file = ConstantesCortoPlazo.FormatoBarraDesenergizada;

                int result = this.servicio.GenerarReporteBarraDesenergizada(
                DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture),
                DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture),path, file);


                return Json(result);
            }
            catch
            {
                return Json(-11);
            }
        }


        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarBarraDesenergizada()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.FormatoBarraDesenergizada;
            return File(fullPath, Constantes.AppExcel, ConstantesCortoPlazo.FormatoBarraDesenergizada);
        }

    }
}