using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CostoOportunidad.Helper;
using COES.MVC.Intranet.Areas.CostoOportunidad.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CostoOportunidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using log4net;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Aplicacion.Helper;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Controllers
{
    public class AdminController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        private readonly CostoOportunidadAppServicio servicio = new CostoOportunidadAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(AdminController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Pagina iniciai del listado de periodos
        /// </summary>
        /// <returns></returns>
        public ActionResult PeriodoIndex()
        {
            PeriodoModel model = new PeriodoModel();
            model.ListaAnios = this.servicio.ListarAnios();
            return View(model);
        }

        /// <summary>
        /// Listado de periodos
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PeriodoList(int anio)
        {
            PeriodoModel model = new PeriodoModel();
            model.Listado = this.servicio.GetByCriteriaCoPeriodos(anio);
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar la vista de edición o creación de periodos
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PeriodoEdit(int idPeriodo)
        {
            PeriodoModel model = new PeriodoModel();
            model.ListaAnios = this.servicio.ListarAnios();
            model.ListaMeses = this.servicio.ListarMeses();

            if (idPeriodo == 0)
            {
                model.Entidad = new CoPeriodoDTO();
                model.Entidad.Coperestado = Constantes.EstadoActivo;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdCoPeriodo(idPeriodo);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite almacenar los datos del periodo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoSave(PeriodoModel model)
        {
            try
            {
                int result = 1;

                CoPeriodoDTO entity = new CoPeriodoDTO();

                entity.Coperanio = model.Anio;
                entity.Copermes = model.Mes;
                entity.Copernomb = model.Descripcion;
                entity.Coperestado = model.Estado;
                entity.Copercodi = model.Codigo;

                if (model.Codigo == 0)
                {
                    entity.Coperusucreacion = base.UserName;
                    entity.Copperfeccreacion = DateTime.Now;
                    bool flagExistencia = this.servicio.ValidarExistenciaPeriodo(model.Anio, model.Mes);

                    if (flagExistencia)
                    {
                        result = 2;
                    }
                }
                entity.Copperusumodificacion = base.UserName;
                entity.Copperfecmodificacion = DateTime.Now;

                if (result != 2)
                {
                    this.servicio.SaveCoPeriodo(entity);
                }

                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar los datos del periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoDelete(int idPeriodo)
        {
            try
            {
                this.servicio.DeleteCoPeriodo(idPeriodo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar la pñagina de versiones
        /// </summary>
        /// <returns></returns>
        public ActionResult VersionIndex(int id)
        {
            VersionModel model = new VersionModel();
            model.CodigoPeriodo = id;

            CoPeriodoDTO periodo = this.servicio.GetByIdCoPeriodo(id);
            model.DesPeriodo = periodo.Copernomb;

            return View(model);
        }

        /// <summary>
        /// Permite listar las versiones de un periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult VersionList(int idPeriodo)
        {
            VersionModel model = new VersionModel();
            model.Listado = this.servicio.GetByCriteriaCoVersions(idPeriodo);
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult VersionEdit(int idPeriodo, int idVersion)
        {
            VersionModel model = new VersionModel();
            model.ListaVersionBase = this.servicio.ListCoVersionbases();
            model.ListaUrs = this.servicio.ObtenerListadoURS();
            model.ListaUrsEspecial = this.servicio.GetByCriteriaCoUrsEspecials(idVersion);

            if (idVersion == 0)
            {
                model.Entidad = new CoVersionDTO();
                model.Entidad.Coverestado = Constantes.EstadoActivo;
                model.Entidad.Copercodi = idPeriodo;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdCoVersion(idVersion);
                model.FechaInicio = (model.Entidad.Coverfecinicio != null) ? ((DateTime)model.Entidad.Coverfecinicio).ToString(Constantes.FormatoFecha) : string.Empty;
                model.FechaFin = (model.Entidad.Coverfecfin != null) ? ((DateTime)model.Entidad.Coverfecfin).ToString(Constantes.FormatoFecha) : string.Empty;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener la configuración de un periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersionBase"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosVersion(int idPeriodo, int idVersionBase)
        {
            return Json(this.servicio.ObtenerDatosVersionBase(idPeriodo, idVersionBase));
        }

        /// <summary>
        /// Permite obtener los datos de una URS
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerUrs(int idGrupo)
        {
            return Json(this.servicio.ObtenerURS(idGrupo));
        }

        /// <summary>
        /// Permite grabar los datos de la URS
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionSave(VersionModel model)
        {
            try
            {
                CoVersionDTO entity = new CoVersionDTO();
                entity.Copercodi = model.CodigoPeriodo;
                entity.Covercodi = model.Codigo;
                entity.Coverdesc = model.Descripcion;
                entity.Covebacodi = model.TipoVersion;
                entity.Coverestado = model.Estado;
                entity.Coverfecinicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Coverfecfin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.ListaURS = model.ListaGrupo;
                if (model.Codigo == 0)
                {
                    entity.Coverusucreacion = base.UserName;
                    entity.Coverfeccreacion = DateTime.Now;
                }
                entity.Coverusumodificacion = base.UserName;
                entity.Coverfecmodificacion = DateTime.Now;

                this.servicio.SaveCoVersion(entity);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar los datos de una URS
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionDelete(int idPeriodo)
        {
            try
            {
                this.servicio.DeleteCoVersion(idPeriodo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite visualizar el listado de envíos del periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EnvioLiquidacion(int idPeriodo)
        {
            PeriodoModel model = new PeriodoModel();
            model.ListadoEnvio = this.servicio.ObtenerEnviosPorPeriodo(idPeriodo);

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(int idPeriodo)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion;
                string file = ConstantesCOportunidad.ArchivoReprogramas;

                this.servicio.ObtenerReporteReprograma(idPeriodo, path, file);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion + ConstantesCOportunidad.ArchivoReprogramas;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, ConstantesCOportunidad.ArchivoReprogramas);
        }

        /// <summary>
        /// Permite obtener el reporte de bandas
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Bandas(int id)
        {
            PeriodoModel model = new PeriodoModel();
            model.ListaBandas = this.servicio.ObtenerReporteBandas(id);

            CoPeriodoDTO periodo = this.servicio.GetByIdCoPeriodo(id);
            model.DesPeriodo = periodo.Copernomb;

            return View(model);
        }

        #region Configuracion URS

        /// <summary>
        /// Muestra la pantalla de configuración de la URS
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfiguracionURS()
        {
            ConfiguracionUrsModel model = new ConfiguracionUrsModel();
            model.ListaPeriodo = this.servicio.GetByCriteriaCoPeriodos(-1);
            model.FechaActual = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            List<CoPeriodoDTO> lstPeriodos = servicio.ListCoPeriodos().OrderByDescending(x => x.Coperanio).ToList();
            model.ListaAnios = lstPeriodos.GroupBy(x => x.Coperanio.Value).Select(m => m.Key).ToList();
            model.Anio = model.ListaAnios.First();

            model.ListaPeriodos = lstPeriodos.Where(x => x.Coperanio == model.Anio).OrderBy(x=>x.Copermes).ToList();

            return View(model);
        }

        /// <summary>
        /// Permite tener las versiones de un periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerVersiones(int idPeriodo)
        {
            try
            {
                return Json(this.servicio.GetByCriteriaCoVersions(idPeriodo));
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener los datos de la versión seleccionada
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerFechasVersion(int idVersion)
        {
            try
            {
                CoVersionDTO version = this.servicio.GetByIdCoVersion(idVersion);
                version.FechaInicio = ((DateTime)version.Coverfecinicio).ToString(Constantes.FormatoFecha);
                version.FechaFin = ((DateTime)version.Coverfecfin).ToString(Constantes.FormatoFecha);
                return Json(new { Result = 1, Version = version });
            }
            catch
            {
                return Json(new { Result = -1, Version = new CoVersionDTO() });
            }
        }

        /// <summary>
        /// Permite obtener el listado de URS
        /// </summary>      
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoConfiguracionURS()
        {
            ConfiguracionUrsModel model = new ConfiguracionUrsModel();
            model.ListadoURS = this.servicio.ObtenerListaURS();
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener la configuración de la URS
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="idUrs"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerConfiguracionURS(int idPeriodo, int idVersion, int idUrs)
        {
            try
            {
                return Json(new { Result = 1, Data = this.servicio.ObtenerConfiguracionURS(idPeriodo, idVersion, idUrs) });
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Result = -1 });
            }
        }

        /// <summary>
        /// Permite obtener la configuración de señales
        /// </summary>
        /// <param name="idCondiguracionDet"></param>
        /// <param name="tipo"></param>
        /// <param name="idUrs"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerConfiguracionSenial(int? idCondiguracionDet, int tipo, int idUrs)
        {
            try
            {
                return Json(new { Result = 1, Data = this.servicio.ObtenerConfiguracionSenial(idCondiguracionDet, tipo, idUrs) });
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Result = -1 });
            }
        }

        /// <summary>
        /// Permite obtener una lista filtrada por Zona (Ubicación)
        /// </summary>
        /// <param name="zonaCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaCanalPorZona(int zonaCodi)
        {
            return Json((new ScadaSp7AppServicio()).GetByZonaTrCanalSp7(zonaCodi));
        }

        [HttpPost]
        public PartialViewResult ConfiguracionSenial()
        {
            return PartialView();
        }

        /// <summary>
        /// Permite grabar la configuración de la URS
        /// </summary>
        /// <param name="idUrs"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarConfiguracionURS(int idUrs, int idPeriodo, int idVersion, string fechaInicio, string fechaFin, 
            string[][] dataOperacion, string[][] dataReporte, string[][] dataEquipo )
        {
            try
            {
                return Json(new { Result = this.servicio.GrabarConfiguracionURS(idUrs, idPeriodo, idVersion, fechaInicio, fechaFin,
                    dataOperacion, dataReporte, dataEquipo , base.UserName) });
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Result = -1 });
            }
        }  
        
        /// <summary>
        /// Permite grabar la configuración de las seniales
        /// </summary>
        /// <param name="idConfiguracionDet"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarConfiguracionSenial(int idConfiguracionDet, int idUrs, string[][] data)
        {
            try
            {
                return Json(new
                {
                    Result = this.servicio.GrabarConfiguracionSenial(idConfiguracionDet, idUrs, data, base.UserName)
                });
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Result = -1 });
            }
        }

        /// <summary>
        /// Permite realizar el importado de la configuración de URS
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarConfiguracionURS(int idPeriodo, int idVersion)
        {
            try
            {
                return Json(new
                {
                    Result = this.servicio.ImportarConfiguracionURS(idPeriodo, idVersion, base.UserName)
                });
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Result = -1 });
            }
        }

        #endregion

        #region Periodos Programacion

        /// <summary>
        /// Devuelve el listado de periodos de programacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPeriodosProg()
        {
            ConfiguracionUrsModel model = new ConfiguracionUrsModel();
            try
            {
                base.ValidarSesionJsonResult();
                string url = Url.Content("~/");

                List<CoPeriodoProgDTO> lstPeriodos = this.servicio.ListarTodosPeriodosDeProgramacion();
                List<CoPeriodoProgDTO> lstPeriodosActivos = this.servicio.ListarTodosPeriodosDeProgramacion().Where(x => x.Perprgestado == "A").ToList();
                model.UltimaVigencia = lstPeriodosActivos.Any() ? lstPeriodosActivos.First().PerprgvigenciaDesc : "";
                model.Resultado = servicio.GenerarHtmlPeriodosProg(url, lstPeriodos);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Guarda en BD la informacion de periodos de programacion 
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="valor"></param>
        /// <param name="vigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPeriodoProg(int accion, int? idPeriodo, decimal valor, string vigencia, string fechaIni)
        {
            ConfiguracionUrsModel model = new ConfiguracionUrsModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fecVigencia = DateTime.ParseExact(vigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                
                string strValidacion = servicio.ValidarPeriodoProg(accion, valor, fecVigencia, fechaIni);
                if (strValidacion != "") throw new ArgumentException(strValidacion);
                
                servicio.RegistrarPeriodosProg(accion, idPeriodo, valor, fecVigencia, base.UserName);
                List<CoPeriodoProgDTO> listaPeriodosProg = servicio.ListarPeriodosDeProgramacionActivos();
                model.UltimaVigencia = listaPeriodosProg.Any() ? listaPeriodosProg.First().PerprgvigenciaDesc : "";
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Elimina un periodo de programacion
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarPeriodoProg(int idPeriodo)
        {
            ConfiguracionUrsModel model = new ConfiguracionUrsModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);
                
                servicio.EliminarPeriodosProg(idPeriodo, base.UserName);
                List<CoPeriodoProgDTO> listaPeriodosProg = servicio.ListarPeriodosDeProgramacionActivos();
                model.UltimaVigencia = listaPeriodosProg.Any() ? listaPeriodosProg.First().PerprgvigenciaDesc : "";
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar los periodos (mes.anio) para el combo PERIODO
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            ConfiguracionUrsModel model = new ConfiguracionUrsModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodos = servicio.ListCoPeriodos().Where(x => x.Coperanio == anio).OrderBy(x => x.Copermes).ToList();
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Verifica las diferencias entre el periodo de programacion y las variables delta para cierto periodo y version
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerificarPeriodoProg(int periodo, int version)
        {
            ConfiguracionUrsModel model = new ConfiguracionUrsModel();

            try
            {
                base.ValidarSesionJsonResult();
                
                List<DiferenciaValor> lstDiferencias = servicio.ComprobarDiferenciasPeriodosProg(periodo, version, out bool existeDiferencias);
                model.HayDiferencia = existeDiferencias;
                model.Resultado = existeDiferencias ? servicio.GenerarHtmlVerificacion(lstDiferencias) : "1";
                model.Rango = servicio.ObtenerRangoVerificacion(periodo, version);
    }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }
        #endregion

        /// <summary>
        /// Permite obtener los datos de los generadors
        /// </summary>
        /// <param name="idConfiguracionDet"></param>
        /// <param name="idUrs"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ConfiguracionGen(int idCondiguracionDet, int idUrs)
        {
            ConfiguracionUrsModel model = new ConfiguracionUrsModel();
            model.ListaGenerador = this.servicio.ObtenerConfiguracionGenerador(idCondiguracionDet, idUrs);
            model.IdConfiguracionDet = idCondiguracionDet;
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar la configuración de los generadores
        /// </summary>
        /// <param name="idConfiguracionDet"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarConfiguracionGenerador(int idConfiguracionDet, string data)
        {
            try
            {
                return Json(new
                {
                    Result = this.servicio.GrabarConfiguracionGenerador(idConfiguracionDet, data, base.UserName)
                });
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Result = -1 });
            }
        }
    }
}