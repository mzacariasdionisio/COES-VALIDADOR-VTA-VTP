using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PotenciaFirme.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirme;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirme.Controllers
{
    public class InsumosController : BaseController
    {
        private readonly PotenciaFirmeAppServicio pfServicio = new PotenciaFirmeAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        public InsumosController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region GENERAL
        /// <summary>
        /// Muestra las versiones de cierto insumo
        /// </summary>
        /// <param name="recurso"></param>
        /// <param name="recacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(int recurso, int recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = this.pfServicio.GenerarHtmlListadoVersion(url, model.TienePermisoEditar, recurso, recacodi);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Obtiene los recálculos para un periodo seleccionado
        /// </summary>
        /// <param name="nomPeriodo"></param>
        /// <returns></returns>
        public JsonResult CargarRevisiones(int pfpericodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.FechaIni = string.Empty;
                model.FechaFin = string.Empty;
                model.ListaRecalculo = new List<PfRecalculoDTO>();

                if (pfpericodi > 0)
                {
                    model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(pfpericodi);
                    PfPeriodoDTO regPeriodo = pfServicio.GetByIdPfPeriodo(pfpericodi);
                    model.FechaIni = regPeriodo.FechaIni.ToString(ConstantesAppServicio.FormatoFecha);
                    model.FechaFin = regPeriodo.FechaFin.ToString(ConstantesAppServicio.FormatoFecha);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(anio);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Obtiene las centrales de la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="periodo"></param>
        /// <param name="tiporecurso"></param>
        /// <returns></returns>
        public PartialViewResult CargarCentrales(int idEmpresa, int periodo, int tiporecurso)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            PfPeriodoDTO regPeriodo = pfServicio.GetByIdPfPeriodo(periodo);
            DateTime fechaIni = regPeriodo.FechaIni;

            switch (tiporecurso)
            {
                case ConstantesPotenciaFirme.RecursoPGarantizada:
                    entitys = pfServicio.ListCentralesXEmpresa(idEmpresa, fechaIni).ToList(); // para p garantizada
                    break;
                case ConstantesPotenciaFirme.RecursoPAdicional:
                    entitys = pfServicio.ListCentralesXEmpresaNodoEnergORsrvFria(idEmpresa, fechaIni).ToList();
                    break;
                case ConstantesPotenciaFirme.RecursoFactorIndispFortuita:
                    entitys = pfServicio.ListCentralesTermoXEmpresa(idEmpresa, fechaIni).ToList();
                    break;
                case ConstantesPotenciaFirme.RecursoFactorPresencia:
                    entitys = pfServicio.ListCentralesXEmpresa(idEmpresa, fechaIni).ToList();
                    break;
                case ConstantesPotenciaFirme.RecursoContratosCV:

                    break;
            }

            model.ListaCentrales = entitys;
            return PartialView(model);
        }

        #endregion

        #region POTENCIA GARANTIZADA

        /// <summary>
        /// Inicio potencia garantizada
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexPotGarantizada(int? pericodi, int? recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaPeriodo = pfServicio.GetPeriodoActual();
                model.ListaAnio = pfServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<PfRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Pfrecacodi : recacodi.GetValueOrDefault(0);
                }

                model.ListaEmpresas = pfServicio.ListarEmpresasConCentralHidro(fechaPeriodo);
                model.ListaCentrales = pfServicio.ListCentralesXEmpresa(-1, fechaPeriodo);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Listado de las potencias garantizadas por filtros
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="revision"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLstPotenciasGarantizadas(int revision, int emprcodi, int equicodi, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                pfServicio.ListarPotenciaGarantizada(revision, verscodi, emprcodi, equicodi, out List<PfPotenciaGarantizadaDTO> listadoPG, out PfVersionDTO pfVersionRecurso);

                model.ListaPotenciaGarantizada = listadoPG;
                model.Version = pfVersionRecurso.Pfverscodi;
                model.NumVersion = pfVersionRecurso.Pfversnumero;
                model.TieneRegistroPrevio = listadoPG.Count > 0;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Guardado del listado de la potencia garantizada
        /// </summary>
        /// <param name="lstPGarantizada"></param>
        /// <param name="periodo"></param>
        /// <param name="recacodi"></param>
        /// <param name="verscodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPotenciaGarantizada(List<PfPotenciaGarantizadaDTO> lstPGarantizada, int recacodi, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (recacodi <= 0)
                    throw new ArgumentException("No existe recálculo para el periodo seleccionado.");

                pfServicio.EditarListaPotenciaGarantizada(recacodi, verscodi, lstPGarantizada, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Cambia el estado de la version a APROBADO (A) y las demas a GENERADO (G)
        /// </summary>
        /// <param name="verscodi"></param>
        /// <param name="recursocodi"></param>
        /// <param name="recacodi"></param>
        /// <returns></returns>
        public JsonResult AprobarVersionInsumo(int verscodi, int recursocodi, int recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                pfServicio.AprobarVersionElegida(verscodi, recursocodi, recacodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Descargar Formato Potencia Garantizada
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="idtipoinformacion"></param>
        /// <param name="afecodi"></param>
        /// <param name="listaInterrupcionSuministro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarFormatoPG(List<PfPotenciaGarantizadaDTO> listaPG)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                string rutaBase = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaFilePotenciaFirme;
                string nombreArchivoCompleto = pfServicio.ObtenerNombreArchivoFormatoPotenciaGarantizada(rutaBase, listaPG);
                model.Resultado = nombreArchivoCompleto;
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaFilePotenciaFirme;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        /// <summary>
        /// Upload potencia Garantizada
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadInfoPG(FormCollection formCollection)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;
                List<PfPotenciaGarantizadaDTO> lstPGarantizadaExcel = new List<PfPotenciaGarantizadaDTO>();
                if (stremExcel != null)
                    lstPGarantizadaExcel = pfServicio.ObtenerPotenciaGarantizadaDataExcel(stremExcel, ConstantesPotenciaFirme.RecursoPGarantizada);

                model.ListaPotenciaGarantizada = lstPGarantizadaExcel;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }
        #endregion

        #region Potencia de Centrales con Regímenes especiales

        /// <summary>
        /// Inicio potencia adicional
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexPotAdicional(int? pericodi, int? recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);
                model.ListaAnio = pfServicio.ListaAnio(regPeriodo.FechaIni).ToList();

                model.ListaRecalculo = new List<PfRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Pfrecacodi : recacodi.GetValueOrDefault(0);
                }

                model.ListaEmpresas = pfServicio.ListEmpresasConCentralNodoEnergORsrvFria(regPeriodo.FechaFin);
                model.ListaCentrales = pfServicio.ListCentralesXEmpresaNodoEnergORsrvFria(-1, regPeriodo.FechaFin);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Listado de las potencias adicionales por filtros
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="revision"></param>
        /// <param name="emprcodi"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLstPotenciasAdicionales(int revision, int emprcodi, int central, int accion, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                pfServicio.ListarPotenciaAdicional(revision, verscodi, -1, emprcodi, central, out List<PfPotenciaAdicionalDTO> listadoPA, out PfVersionDTO pfVersionRecurso);

                model.ListaPotenciaAdicional = listadoPA;
                model.Version = pfVersionRecurso.Pfverscodi;
                model.NumVersion = pfVersionRecurso.Pfversnumero;
                model.TieneRegistroPrevio = listadoPA.Count > 0;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Guardado del listado de la potencia adicional
        /// </summary>
        /// <param name="lstPAdicional"></param>
        /// <param name="periodo"></param>
        /// <param name="recacodi"></param>
        /// <param name="verscodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPotenciaAdicional(List<PfPotenciaAdicionalDTO> lstPAdicional, int recacodi, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (recacodi <= 0)
                    throw new ArgumentException("No existe recálculo para el periodo seleccionado.");

                pfServicio.EditarListaPotenciaAdicional(recacodi, verscodi, lstPAdicional, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Descargar Formato Potencia Adicional
        /// </summary>
        /// <param name="listaPA"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarFormatoPA(List<PfPotenciaAdicionalDTO> listaPA)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                foreach (var item in listaPA)
                {
                    if (item.Pfpadiincremental == 1)
                    {
                        item.Pfpadipf = (1 - item.Pfpadifi) * item.Pfpadipe;
                    }
                }

                string rutaBase = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaFilePotenciaFirme;
                string nombreArchivoCompleto = pfServicio.ObtenerNombreArchivoFormatoPotenciaAdicional(rutaBase, listaPA);
                model.Resultado = nombreArchivoCompleto;
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.NRegistros = -1;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Upload potencia Adicional
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadInfoPA(FormCollection formCollection)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;
                List<PfPotenciaAdicionalDTO> lstPAdicionalExcel = new List<PfPotenciaAdicionalDTO>();
                if (stremExcel != null)
                    lstPAdicionalExcel = pfServicio.ObtenerPotenciaAdicionalDataExcel(stremExcel, ConstantesPotenciaFirme.RecursoPAdicional);

                model.ListaPotenciaAdicional = lstPAdicionalExcel;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.NRegistros = -1;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        #endregion

        #region FACTOR DE INDISPONIBILIDAD FORTUÍTA

        /// <summary>
        /// Página Principal de Indisponibilidad Fortuíta
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexFIndisponibilidad(int? pericodi, int? recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);
                model.ListaAnio = pfServicio.ListaAnio(regPeriodo.FechaIni).ToList();

                model.ListaRecalculo = new List<PfRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Pfrecacodi : recacodi.GetValueOrDefault(0);
                }

                model.ListaEmpresas = pfServicio.ListarEmpresasTermo(regPeriodo.FechaFin);
                model.ListaCentrales = pfServicio.ListCentralesTermoXEmpresa(-1, regPeriodo.FechaFin);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Listado de factores de indisponibilidad por filtros
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="revision"></param>
        /// <param name="emprcodi"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLstFactorIndisponibilidad(int revision, int emprcodi, int central, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pfServicio.ListarFactores(ConstantesPotenciaFirme.FactorIndispFortuita, revision, verscodi, -1, emprcodi, central, out List<PfFactoresDTO> listadoFI, out PfVersionDTO pfVersionRecurso);

                model.ListaFactorIndisponibilidad = listadoFI;
                model.Version = pfVersionRecurso.Pfverscodi;
                model.NumVersion = pfVersionRecurso.Pfversnumero;
                model.TieneRegistroPrevio = listadoFI.Count > 0;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Guardado del listado de los factores de indisponibilidad
        /// </summary>
        /// <param name="lstPAdicional"></param>
        /// <param name="periodo"></param>
        /// <param name="recacodi"></param>
        /// <param name="verscodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarListadoFactorIndisponibilidad(List<PfFactoresDTO> lstFIndisponibilidad, int recacodi, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (recacodi <= 0)
                    throw new ArgumentException("No existe recálculo para el periodo seleccionado.");

                lstFIndisponibilidad = lstFIndisponibilidad != null ? lstFIndisponibilidad : new List<PfFactoresDTO>();
                pfServicio.EditarListaFactor(recacodi, verscodi, ConstantesPotenciaFirme.FactorIndispFortuita, lstFIndisponibilidad, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Descargar Formato de Factor de Indisponibilidad
        /// </summary>
        /// <param name="listaPA"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarFormatoFI(List<PfFactoresDTO> listaFI)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                string rutaBase = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaFilePotenciaFirme;
                string nombreArchivoCompleto = pfServicio.ObtenerNombreArchivoFormatoFactores(rutaBase, listaFI, ConstantesPotenciaFirme.FactorIndispFortuita);
                model.Resultado = nombreArchivoCompleto;
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.NRegistros = -1;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Importar información desde un archivo excel
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarInfoFI(FormCollection formCollection)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {

                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;
                List<PfFactoresDTO> lstFIExcel = new List<PfFactoresDTO>();
                if (stremExcel != null)
                    lstFIExcel = pfServicio.ObtenerFactoreIndisFortuitaDataExcel(stremExcel);

                model.ListaFactorIndisponibilidad = lstFIExcel;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }
        #endregion

        #region FACTOR PRESENCIA

        /// <summary>
        /// Página Principal de Factor Presencia
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexFPresencia(int? pericodi, int? recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaPeriodo = pfServicio.GetPeriodoActual(); // primer dia del mes anterior
                model.ListaAnio = pfServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<PfRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Pfrecacodi : recacodi.GetValueOrDefault(0);
                }

                model.ListaEmpresas = pfServicio.ListarEmpresasConCentralHidro(fechaPeriodo);
                model.ListaCentrales = pfServicio.ListCentralesXEmpresa(-1, fechaPeriodo);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Listado de factores presencia por filtros
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="revision"></param>
        /// <param name="emprcodi"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLstFactorPresencia(int revision, int emprcodi, int central, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pfServicio.ListarFactores(ConstantesPotenciaFirme.FactorPresencia, revision, verscodi, -1, emprcodi, central, out List<PfFactoresDTO> listadoFP, out PfVersionDTO pfVersionRecurso);

                model.ListaFactorPresencia = listadoFP;
                model.Version = pfVersionRecurso.Pfverscodi;
                model.NumVersion = pfVersionRecurso.Pfversnumero;
                model.TieneRegistroPrevio = listadoFP.Count > 0;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Guardado del listado de los factores de presencia
        /// </summary>
        /// <param name="lstPAdicional"></param>
        /// <param name="periodo"></param>
        /// <param name="recacodi"></param>
        /// <param name="verscodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarListadoFactorPresencia(List<PfFactoresDTO> lstFPresencia, int recacodi, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (recacodi <= 0)
                    throw new ArgumentException("No existe recálculo para el periodo seleccionado.");

                lstFPresencia = lstFPresencia != null ? lstFPresencia : new List<PfFactoresDTO>();
                pfServicio.EditarListaFactor(recacodi, verscodi, ConstantesPotenciaFirme.FactorPresencia, lstFPresencia, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Descargar Formato de Potencia Presencia
        /// </summary>
        /// <param name="listaPA"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarFormatoFP(List<PfFactoresDTO> listaFP)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                string rutaBase = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaFilePotenciaFirme;
                string nombreArchivoCompleto = pfServicio.ObtenerNombreArchivoFormatoFactores(rutaBase, listaFP, ConstantesPotenciaFirme.FactorPresencia);
                model.Resultado = nombreArchivoCompleto;
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.NRegistros = -1;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Importar información desde un archivo excel
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarInfoFP(FormCollection formCollection)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {

                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;
                List<PfFactoresDTO> lstFPExcel = new List<PfFactoresDTO>();
                if (stremExcel != null)
                    lstFPExcel = pfServicio.ObtenerFactorePresenciaDataExcel(stremExcel);

                model.ListaFactorPresencia = lstFPExcel;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }
        #endregion

        #region CONTRATOS DE COMPRA Y VENTA DE POTENCIA FIRME

        /// <summary>
        /// Página Principal de Contratos de Compra y venta
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexContratosCV(int? pericodi, int? recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaPeriodo = pfServicio.GetPeriodoActual(); // primer dia del mes anterior
                model.ListaAnio = pfServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<PfRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Pfrecacodi : recacodi.GetValueOrDefault(0);
                }

                DateTime fechaInicial = fechaPeriodo;
                DateTime fechaFinal = fechaInicial.AddMonths(1).AddMinutes(-1);
                model.FechaIni = fechaInicial.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaFinal.ToString(ConstantesAppServicio.FormatoFecha);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Devuelve la lista de empresas que participan en la compra y venta de potencia firme
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLstEmpresasCV()
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            List<EmpresaGeneral> empresasTotales = new List<EmpresaGeneral>();
            try
            {
                base.ValidarSesionJsonResult();
                if (!VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                List<SiEmpresaDTO> listadoEmp = pfServicio.ObtenerEmpresasSEIN();
                foreach (var empresa in listadoEmp)
                {
                    EmpresaGeneral objEmp = new EmpresaGeneral();
                    objEmp.Emprcodi = empresa.Emprcodi;
                    objEmp.Emprnomb = empresa.Emprnomb != null ? empresa.Emprnomb.Trim() : string.Empty;

                    empresasTotales.Add(objEmp);
                }

                model.NRegistros = listadoEmp.Count;
                model.ListaEmpresasTotales = empresasTotales;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Listado de contratos de compra y venta de potencia firme por filtros
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLstContratosCV(int revision, int verscodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pfServicio.ListarContratosCV(revision, verscodi, out List<PfContratosDTO> listadoCCV, out PfVersionDTO pfVersionRecurso);

                model.ListaContratosCV = listadoCCV;
                model.Version = pfVersionRecurso.Pfverscodi;
                model.NumVersion = pfVersionRecurso.Pfversnumero;
                model.TieneRegistroPrevio = listadoCCV.Count > 0;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Guardado del listado de los contratos de compra y venta
        /// </summary>
        /// <param name="lstContratosCV"></param>
        /// <param name="periodo"></param>
        /// <param name="recacodi"></param>
        /// <param name="verscodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarListadoContratosCV(List<PfContratosDTO> lstContratosCV, int recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (recacodi <= 0)
                    throw new ArgumentException("No existe recálculo para el periodo seleccionado.");

                lstContratosCV = lstContratosCV != null ? lstContratosCV : new List<PfContratosDTO>();

                pfServicio.GuardarContratosCV(lstContratosCV, base.UserName, recacodi);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Descargar Formato de Contratos de compra y venta
        /// </summary>
        /// <param name="listaCVV"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarFormatoCCV(List<PfContratosDTO> listaCCV)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                string rutaBase = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaFilePotenciaFirme;
                string nombreArchivoCompleto = pfServicio.ObtenerNombreArchivoFormatoContratoCV(rutaBase, listaCCV);
                model.Resultado = nombreArchivoCompleto;
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.NRegistros = -1;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        #endregion

        #region RER_NC Eólicos y Solares

        /// <summary>
        /// Página Principal de RER_NC
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexRerNC(int? pericodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = pfServicio.GetPeriodoActual();
                model.ListaAnio = pfServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);

                model.FechaIniRER = ConstantesPotenciaFirme.MesIniHistoricoRER;
                model.FechaFinRER = ConstantesPotenciaFirme.MesFinHistoricoRER;

                model.ListaRecalculo = new List<PfRecalculoDTO>();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Permite generar archivo excel de Cuadro
        /// </summary>
        /// <param name="irecacodi"></param>
        /// <param name="cuadro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoRER(int pfpericodi, int tipoReporte)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                if (pfpericodi > 0)
                {
                    pfServicio.GenerarArchivoExcelRER(ruta, tipoReporte, pfpericodi, out string nameFile);
                    model.Resultado = nameFile;
                }
                else
                {
                    model.Mensaje = "No se ha seleccionado un periodo.";
                    model.Resultado = "-1";
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Generar historico
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarHistoricoRER()
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                pfServicio.EjecutarHistoricoRER();
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite exportar archivos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarRER()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            System.IO.File.Delete(ruta + nombreArchivo);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        #endregion

    }
}