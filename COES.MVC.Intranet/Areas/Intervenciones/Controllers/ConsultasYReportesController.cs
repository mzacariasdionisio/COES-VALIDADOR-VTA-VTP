using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Intervenciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Intervenciones.Controllers
{
    public class ConsultasYReportesController : BaseController
    {
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        IntervencionesAppServicio interServicio = new IntervencionesAppServicio();
        SeguridadServicioClient servSeguridad = new SeguridadServicioClient();
        MigracionesAppServicio servMigraciones = new MigracionesAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        #region REPORTES - ANEXOS PROGRAMA ANUAL
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptIntervencionesMayores()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();

            model.ListaCboEmpresa = interServicio.ListarComboEmpresas();
            model.ListaProgramaciones = interServicio.ListarProgramacionesAnuales();

            return View(model);
        }

        [HttpPost]
        public PartialViewResult RptIntervencionesMayoresListado(int idProgramacionAnual, string emprCodi)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = this.interServicio.ListadoIntervencionesMayores(idProgramacionAnual, auxEmprCodi);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelIntervencionesMayores(int idProgramacionAnual, string emprCodi, int anexo)
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            // Obtener los datos de la programación
            InProgramacionDTO entity = interServicio.ObtenerProgramacionesPorIdSinPlazo(idProgramacionAnual);

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            if (anexo == ConstantesIntervencionesAppServicio.iListado)
            {
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelListadoIntervencionesMayores;
                if (interServicio.ExportarToExcelListadoIntervencionesMayores(idProgramacionAnual, auxEmprCodi, path, fileName, pathLogo) == "-1")
                {
                    return Json("-1");
                }
            }
            else if (anexo == ConstantesIntervencionesAppServicio.iAnexo2)
            {
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelAnexo2;
                if (interServicio.ExportarToExcelReporteAnexo2IntervencionesMayores(idProgramacionAnual, auxEmprCodi, entity.Progrfechaini.Year, path, fileName, pathLogo) == "-1")
                {
                    return Json("-1");
                }
            }
            else if (anexo == ConstantesIntervencionesAppServicio.iAnexo3)
            {
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelAnexo3;
                if (interServicio.ExportarToExcelReporteAnexo3SistemasAislados(idProgramacionAnual, auxEmprCodi, entity.Progrfechaini.Year, path, fileName, pathLogo) == "-1")
                {
                    return Json("-1");
                }
            }
            else if (anexo == ConstantesIntervencionesAppServicio.iAnexo4)
            {
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelAnexo4;
                if (interServicio.ExportarToExcelReporteAnexo4RestriccionSuministros(idProgramacionAnual, auxEmprCodi, entity.Progrfechaini.Year, path, fileName, pathLogo) == "-1")
                {
                    return Json("-1");
                }
            }
            else if (anexo == ConstantesIntervencionesAppServicio.iAnexo5ES)
            {
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelAnexo5ES;
                if (interServicio.ExportarToExcelReporteAnexo5EnServicio(idProgramacionAnual, auxEmprCodi, entity.Progrfechaini.Year, path, fileName, pathLogo) == "-1")
                {
                    return Json("-1");
                }
            }
            else if (anexo == ConstantesIntervencionesAppServicio.iAnexo5FS)
            {
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelAnexo5FS;
                if (interServicio.ExportarToExcelReporteAnexo5FueraServicio(idProgramacionAnual, auxEmprCodi, entity.Progrfechaini.Year, path, fileName, pathLogo) == "-1")
                {
                    return Json("-1");
                }
            }
            else if (anexo == ConstantesIntervencionesAppServicio.iAnexo6Generacion)
            {
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelAnexo6Generacion;
                if (interServicio.ExportarToExcelReporteAnexo6GeneracionESFS(idProgramacionAnual, auxEmprCodi, entity.Progrfechaini.Year, path, fileName) == "-1")
                {
                    return Json("-1");
                }
            }
            else if (anexo == ConstantesIntervencionesAppServicio.iAnexo6Transmision)
            {
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelAnexo6Transmision;
                if (interServicio.ExportarToExcelReporteAnexo6TransmisionESFS(idProgramacionAnual, auxEmprCodi, entity.Progrfechaini.Year, path, fileName) == "-1")
                {
                    return Json("-1");
                }
            }

            return Json(fileName);
        }
        #endregion

        #region REPORTES - INTERVENCIONES MAYORES
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptIntervencionesMayoresPorPeriodo()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();

            model.ListaCboEmpresa = interServicio.ListarComboEmpresas();
            model.ListaTiposProgramacion = interServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult RptIntervencionesMayoresPorPeriodoListado(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin, int nroPagina)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = this.interServicio.ReporteIntervencionesMayoresPorPeriodo(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelRptIntervencionesMayoresPorPeriodo(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin)
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = new List<InIntervencionDTO>();
            model.ListaIntervenciones = interServicio.ReporteIntervencionesMayoresPorPeriodo(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelntervencionesMayores;
                interServicio.ExportarToExcelReporteIntervencionesMayoresPorPeriodo(model.ListaIntervenciones, path, pathLogo, fileName);

                return Json(fileName);
            }
        }
        #endregion

        #region REPORTES - INTERVENCIONES IMPORTANTES
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptIntervencioneImportantes()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();

            model.ListaCboEmpresa = interServicio.ListarComboEmpresas();
            model.ListaTiposProgramacion = interServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult RptIntervencioneImportantesListado(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin, int nroPagina)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = this.interServicio.ReporteIntervencionesImportantes(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelIntervencionesImportantes(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin)
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = new List<InIntervencionDTO>();
            model.ListaIntervenciones = interServicio.ReporteIntervencionesImportantes(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelIntervencionesImportantes;
                interServicio.ExportarToExcelReporteIntervencionesImportantes(model.ListaIntervenciones, path, pathLogo, fileName);

                return Json(fileName);
            }
        }
        #endregion

        #region REPORTES CONEXIONES PROVISIONALES
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptIntervencionesConexionesProvisionales()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();

            model.ListaCboEmpresa = interServicio.ListarComboEmpresas();
            model.ListaTiposProgramacion = interServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult RptIntervencionesConexionesProvisionalesListado(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin, int nroPagina)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = this.interServicio.ReporteConexionesProvisionales(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelIntervencionesConexionesProvisionales(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin)
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = new List<InIntervencionDTO>();
            model.ListaIntervenciones = interServicio.ReporteConexionesProvisionales(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelConexionesProvisionales;
                interServicio.ExportarToExcelReporteConexionesProvisionales(model.ListaIntervenciones, path, pathLogo, fileName);

                return Json(fileName);
            }
        }
        #endregion

        #region REPORTES SISTEMAS AISLADOS
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptIntervencionesSistemasAislados()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();

            model.ListaCboEmpresa = interServicio.ListarComboEmpresas();
            model.ListaTiposProgramacion = interServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult RptIntervencionesSistemasAisladosListado(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin, int nroPagina)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = this.interServicio.ReporteSistemasAislados(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelIntervencionesSistemasAislados(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin)
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = new List<InIntervencionDTO>();
            model.ListaIntervenciones = interServicio.ReporteSistemasAislados(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelSistemasAislados;
                interServicio.ExportarToExcelReporteSistemasAislados(model.ListaIntervenciones, path, pathLogo, fileName);
                return Json(fileName);
            }

        }
        #endregion

        #region REPORTES INTERRUPCION RESTRICCION SUMINISTROS
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptInterrupcionRestriccionSuministros()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();

            model.ListaCboEmpresa = interServicio.ListarComboEmpresas();
            model.ListaTiposProgramacion = interServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult RptInterrupcionRestriccionSuministrosListado(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin, int nroPagina)
        {
            base.ValidarSesionUsuario();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            Intervencion model = new Intervencion();
            model.ListaIntervenciones = this.interServicio.ReporteInterrupcionRestriccionSuministros(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelInterrupcionRestriccionSuministros(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin)
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            Intervencion model = new Intervencion();

            model.ListaIntervenciones = new List<InIntervencionDTO>();
            model.ListaIntervenciones = interServicio.ReporteInterrupcionRestriccionSuministros(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelInterrupcionRestriccionSuministros;
                interServicio.ExportarToExcelReporteRestriccionSuministros(model.ListaIntervenciones, path, pathLogo, fileName);

                return Json(fileName);
            }
        }
        #endregion

        #region REPORTES AGENTES
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptAgentes()
        {
            Intervencion model = new Intervencion();

            model.ListaCboEmpresa = interServicio.ListarComboEmpresas();
            model.ListaTiposProgramacion = interServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult RptAgentesListado(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin, int nroPagina)
        {
            base.ValidarSesionUsuario();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            Intervencion model = new Intervencion();
            model.ListaIntervenciones = this.interServicio.ReporteAgentes(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelAgentes(int tipoProCodi, string emprCodi, DateTime? fechaInicio, DateTime? fechaFin)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();
            model.ListaIntervenciones = new List<InIntervencionDTO>();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            model.ListaIntervenciones = interServicio.ReporteAgentes(0, tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoAgentesXls;

                //string pathOrigen = FileServer.GetDirectory() + base.PathFiles + ConstantesIntervencionesAppServicio.PlantillaXlsm;
                //string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;

                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;                

                //FileServer.CopiarFileAlterFinal(pathOrigen, pathDestino, fileName, AppDomain.CurrentDomain.BaseDirectory);    
                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                interServicio.ExportarToExcelReporteAgentesXlsm(model.ListaIntervenciones, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, fechaInicio, fechaFin, tipoProCodi);

                return Json(fileName);
            }
        }
        #endregion

        #region REPORTES OSINERGMIN PROC 25
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptOSINERGMIN()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();

            return View(model);
        }

        [HttpPost]
        public PartialViewResult RptOSINERGMINProc257dListado()
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();
            model.ListaIntervenciones = this.interServicio.ReporteOSINERGMINProc257dListado();
            return PartialView(model);
        }

        public JsonResult RptOSINERGMINProc257dListadoHtml()
        {
            base.ValidarSesionUsuario();

            string dRspta = string.Empty;
            string resultadoReporte = this.interServicio.ReporteOSINERGMINProc257dHtml(ref dRspta);

            string[] datos = new string[2];
            datos[0] = dRspta;
            datos[1] = resultadoReporte;

            var json = Json(datos);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        [HttpPost]
        public JsonResult RptOSINERGMINProc257dExportarExcel()
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();

            model.ListaIntervenciones = new List<InIntervencionDTO>();
            model.ListaIntervenciones = interServicio.ReporteOSINERGMINProc257dListado();

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelOSINERGMINProc257d;
                interServicio.ReporteOSINERGMINProc257dExportarExcel(model.ListaIntervenciones, path, pathLogo, fileName);

                return Json(fileName);
            }
        }
        #endregion

        #region REPORTES INTERVENCIONES FORMATO OSINERGMIN
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptIntervenciones()
        {
            Intervencion model = new Intervencion();

            model.ListaTiposProgramacion = interServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListaTiposProgramacion.Add(new EveEvenclaseDTO() { Evenclasecodi = ConstantesIntervencionesAppServicio.TipoProgramacionPM7, Evenclasedesc = "Mantenimientos Programados 7d" });

            model.ListaCboEmpresa = interServicio.ListarComboEmpresas();
            model.Entidad = new InIntervencionDTO();
            model.Entidad.Interfechaini = DateTime.Now;
            model.Entidad.Interfechafin = DateTime.Now;

            //Permisos OSINERGMIN
            var objUsuario = this.servSeguridad.ObtenerUsuarioPorLogin(User.Identity.Name);
            model.TienePermisoSPR = objUsuario != null && objUsuario.AreaCode == ConstantesIntervencionesAppServicio.AreacodiSPR;

            return View(model);
        }

        [HttpPost]
        public JsonResult RptIntervencionesListadoHtml(int tipoProCodi, string emprCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            base.ValidarSesionUsuario();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion
            string resultadoReporte;

            if (tipoProCodi != ConstantesIntervencionesAppServicio.TipoProgramacionPM7)
            {
                resultadoReporte = this.interServicio.ReporteIntervencionesHtml(tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);
            }
            else
            {
                DateTime f1 = DateTime.ParseExact(fechaInicio.ToString(ConstantesAppServicio.FormatoFecha), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                var listaIntervencion = interServicio.GetMantenimientos(f1);
                listaIntervencion = interServicio.ObtenerIntervencionesPartidasPorDias(listaIntervencion).OrderBy(x => x.EmprNomb).ThenBy(x => x.Areaabrev).ThenBy(x => x.Equiabrev).ToList();
                if (auxEmprCodi != "0")
                {
                    var empresas = auxEmprCodi.Split(',');
                    List<int> emprecodis = new List<int>();
                    foreach (var cod in empresas)
                    {
                        emprecodis.Add(Convert.ToInt32(cod));
                    }
                    listaIntervencion = listaIntervencion.Where(t => emprecodis.Contains(t.Emprcodi)).ToList();
                }
                resultadoReporte = interServicio.InformacionMantProgHtml(listaIntervencion);
            }

            var json = Json(resultadoReporte);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        [HttpPost]
        public JsonResult GenerarExcelQuebradoIntervenciones(int tipoProCodi, string emprCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            base.ValidarSesionUsuario();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();

            model.ListaIntervenciones = new List<InIntervencionDTO>();

            model.ListaIntervenciones = interServicio.ReporteIntervenciones(tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                //path = ConstantesIntervencionesAppServicio.sDirectory + ConstantesIntervencionesAppServicio.Reportes;
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = "ExportIntervencion.xlsx";

                interServicio.ExportarToExcelReporteQuebradoIntervenciones(model.ListaIntervenciones, path, pathLogo, fileName);

                return Json(fileName);
            }
        }

        [HttpPost]
        public JsonResult GenerarExcelIntervenciones(int tipoProCodi, string emprCodi, bool flgFormatoOsinergmin, DateTime fechaInicio, DateTime fechaFin)
        {
            base.ValidarSesionUsuario();

            #region MANEJO DE COMBOS MULTI-SELECCIÓN
            string auxEmprCodi = ValidarComboMultiseleccionEntero(emprCodi);
            #endregion

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();

            model.ListaIntervenciones = new List<InIntervencionDTO>();

            if (tipoProCodi != ConstantesIntervencionesAppServicio.TipoProgramacionPM7)
            {
                model.ListaIntervenciones = interServicio.ReporteIntervenciones(tipoProCodi, auxEmprCodi, fechaInicio, fechaFin);
            }
            else
            {
                DateTime f1 = DateTime.ParseExact(fechaInicio.ToString(ConstantesAppServicio.FormatoFecha), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                var listaIntervencion = interServicio.GetMantenimientos(f1);
                listaIntervencion = interServicio.ObtenerIntervencionesPartidasPorDias(listaIntervencion).OrderBy(x => x.EmprNomb).ThenBy(x => x.Areaabrev).ThenBy(x => x.Equiabrev).ToList();
                if (auxEmprCodi != "0")
                {
                    var empresas = auxEmprCodi.Split(',');
                    List<int> emprecodis = new List<int>();
                    foreach (var cod in empresas)
                    {
                        emprecodis.Add(Convert.ToInt32(cod));
                    }
                    listaIntervencion = listaIntervencion.Where(t => emprecodis.Contains(t.Emprcodi)).ToList();
                }
                model.ListaIntervenciones = listaIntervencion;
            }

            if (model.ListaIntervenciones.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                if (flgFormatoOsinergmin)
                {
                    fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelIntervencionesOSINERGMING;
                    if (tipoProCodi != ConstantesIntervencionesAppServicio.TipoProgramacionPM7)
                        interServicio.ExportarToExcelReporteIntervenciones(model.ListaIntervenciones, path, fileName, flgFormatoOsinergmin);
                    else
                    {
                        List<EveManttoDTO> listaMantto = this.interServicio.ConvertirListaIntervencionToListaMantto(model.ListaIntervenciones);
                        this.servMigraciones.GenerarArchivoExcelInfoP25(listaMantto, fechaInicio, path + fileName);
                    }
                }
                else
                {
                    fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelIntervenciones;
                    interServicio.ExportarToExcelReporteIntervencionesSinFormatoOsinergmin(model.ListaIntervenciones, path, pathLogo, fileName, flgFormatoOsinergmin);
                }
                return Json(fileName);
            }
        }

        #endregion

        #region REPORTES INDICE F1 Y F2
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptF1F2()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();
            CultureInfo ci = new CultureInfo("es-ES");

            var months = ci.DateTimeFormat
                        .MonthNames
                        .TakeWhile(monthName => monthName != String.Empty)
                        .Select((monthName, index) => new SelectListItem
                        {
                            Value = (index + 1).ToString(CultureInfo.InvariantCulture),
                            Text = string.Format("{1}", index + 1, monthName.ToString().ToUpper())
                        });
            model.ListaMes = months;

            var years = Enumerable
                        .Range(DateTime.Now.Year, 15)
                        .Select(year => new SelectListItem
                        {
                            Value = year.ToString(CultureInfo.InvariantCulture),
                            Text = year.ToString(CultureInfo.InvariantCulture)
                        });
            model.ListaAnio = years;

            return View(model);
        }

        #region PARA INTERVENCIONES PROGRAMADAS
        public PartialViewResult RptF1F2ListadoProgramados(int anio, int mes)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();
            model.ListaIntervenciones = this.interServicio.ReporteIntervencionesF1F2Programados(anio, mes);

            return PartialView(model);
        }
        #endregion

        #region PARA INTERVENCIONES EJECUTADAS
        public PartialViewResult RptF1F2ListadoEjecutados(int anio, int mes)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();
            model.ListaIntervenciones = this.interServicio.ReporteIntervencionesF1F2Ejecutados(anio, mes);

            return PartialView(model);
        }
        #endregion

        #region PARA LISTADOS F1 Y F2 MENSUAL Y EJECUTADOS
        public JsonResult GenerarExcelF1F2ProgramadosEjecutados(int anio, int mes)
        {
            base.ValidarSesionUsuario();

            string filename = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();
            model.ListaIntervencionesProgramadas = new List<InIntervencionDTO>();
            model.ListaIntervencionesProgramadas = interServicio.ReporteIntervencionesF1F2Programados(anio, mes);

            model.ListaIntervencionesEjecutadas = new List<InIntervencionDTO>();
            model.ListaIntervencionesEjecutadas = interServicio.ReporteIntervencionesF1F2Ejecutados(anio, mes);

            if (model.ListaIntervencionesProgramadas.Count == 0 || model.ListaIntervencionesEjecutadas.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                filename = ConstantesIntervencionesAppServicio.NombreReporteExcelF1F2ProgramadosEjecutados;
                interServicio.ExportarToExcelReporteF1F2ProgramadosEjecutados(model.ListaIntervencionesProgramadas, model.ListaIntervencionesEjecutadas, path, pathLogo, filename);

                return Json(filename);
            }
        }
        #endregion

        #region PARA INDICES F1 Y F2
        [HttpPost]
        public JsonResult GenerarExcelIndicesF1F2(int anio, int mes, bool flgCorrectivo)
        {
            base.ValidarSesionUsuario();

            string filename = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();
            model.ListaIntervenciones = new List<InIntervencionDTO>();
            model.ListaIndiceF1F2 = interServicio.ReporteIntervencionesIndicesF1F2(flgCorrectivo);

            if (model.ListaIndiceF1F2 == null)
            {
                return Json("-1");
            }
            else
            {
                if (model.ListaIndiceF1F2.Count == 0)
                {
                    return Json("-1");
                }
                else
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                    filename = ConstantesIntervencionesAppServicio.NombreReporteExcelF1F2Indices;
                    interServicio.ExportarToExcelReporteIntervencionesIndicesF1F2(model.ListaIndiceF1F2, path, pathLogo, filename);

                    return Json(filename);
                }
            }
        }
        #endregion
        #endregion

        #region INFORMES DE OPERACIONES COES
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult Informes()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            return View();
        }

        [HttpPost]
        public JsonResult GenerarWordInforme(int tipoInforme, DateTime fechaProceso)
        {
            base.ValidarSesionUsuario();

            string rspta = "-1";
            string file = string.Empty;
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();
            model.ListaIntervenciones = new List<InIntervencionDTO>();

            if (tipoInforme == ConstantesIntervencionesAppServicio.iInfProgramaDiarioOpe)
            {
                file = ConstantesIntervencionesAppServicio.InformeProgramaDiarioOperaciones;
                interServicio.GenerarInformeProgramaDiarioOperaciones(fechaProceso, path + file, pathLogo);
            }
            else if (tipoInforme == ConstantesIntervencionesAppServicio.iInfProgSemanalOpe)
            {
                file = ConstantesIntervencionesAppServicio.InformeProgramaSemanalOperaciones;
                interServicio.GenerarInformeProgramaSemanalOperaciones(fechaProceso, path + file, pathLogo);
            }

            rspta = file;

            return Json(rspta);
        }
        #endregion

        #region REPORTES MENSAJES
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptMensajes()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();
            model.Entidad = new InIntervencionDTO();
            model.Entidad.Interfechaini = DateTime.Now.Date.AddDays(-1);
            model.Entidad.Interfechafin = DateTime.Now.Date;

            return View(model);
        }

        /// <summary>
        /// Listado de mensajes
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public PartialViewResult RptMensajesListado(DateTime fechaIni, DateTime fechaFin, int estado)
        {
            Intervencion model = new Intervencion();

            model.TipoMensaje = estado;
            model.ListaMensajes = this.interServicio.ReporteMensajes(fechaIni, fechaFin, ConstantesIntervencionesAppServicio.TMsgcodiTodos).OrderByDescending(x => x.Msgfeccreacion).ToList();
            model.TotalMensaje = model.ListaMensajes.Where(x => x.Tmsgcodi == ConstantesIntervencionesAppServicio.TMsgcodiMensajes).Count();
            model.TotalMensajeNoEjecutado = model.ListaMensajes.Where(x => x.Tmsgcodi == ConstantesIntervencionesAppServicio.TMsgcodiNoEjecutados).Count();
            model.TotalMensajeAlertaHo = model.ListaMensajes.Where(x => x.Tmsgcodi == ConstantesIntervencionesAppServicio.TMsgcodiAlertaHo).Count();
            model.ListaMensajes = model.ListaMensajes.Where(x => estado == -1 || x.Tmsgcodi == estado).ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelMensajes(DateTime fechaIni, DateTime fechaFin)
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();
            model.ListaMensajes = new List<SiMensajeDTO>();

            model.ListaMensajes = interServicio.ReporteMensajes(fechaIni, fechaFin, ConstantesIntervencionesAppServicio.TMsgcodiTodos);

            if (model.ListaMensajes.Count == 0)
            {
                return Json("-2");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelMensajes;
                interServicio.ExportarToExcelReporteMensajes(model.ListaMensajes, path, pathLogo, fileName);

                return Json(fileName);
            }
        }


        /// <summary>
        /// Permite visualizar el contenido del mensaje enviado
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerContenidoMensaje(int idMensaje)
        {
            Intervencion model = new Intervencion();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                SiMensajeDTO entity = this.interServicio.GetMensajePorId(idMensaje);
                model.StrMensaje = entity.Msgcontenido;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        #endregion

        #region REPORTES HISTORIAL
        // GET: Intervenciones/ConsultasYReportes
        public ActionResult RptHistorial()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();

            model.ListadoActividad = new List<string>();

            model.ListadoActividad.Add(ConstantesIntervencionesAppServicio.sCreacion);
            model.ListadoActividad.Add(ConstantesIntervencionesAppServicio.sModificacion);
            model.ListadoActividad.Add(ConstantesIntervencionesAppServicio.sEliminacion);
            model.ListadoActividad.Add(ConstantesIntervencionesAppServicio.sAprobacionRechazo);
            model.ListadoActividad.Add(ConstantesIntervencionesAppServicio.sProcesamiento);
            model.ListadoActividad.Add(ConstantesIntervencionesAppServicio.sCopiar);
            model.ListadoActividad.Add(ConstantesIntervencionesAppServicio.sImportar);
            model.ListadoActividad.Add(ConstantesIntervencionesAppServicio.sMensaje);

            model.Entidad = new InIntervencionDTO();
            model.Entidad.Interfechaini = DateTime.Now.Date.AddDays(-1);
            model.Entidad.Interfechafin = DateTime.Now.Date;

            return View(model);
        }

        public PartialViewResult RptHistorialListado(DateTime? fechaInicio, DateTime? fechaFin, string actividad, int nroPagina)
        {
            base.ValidarSesionUsuario();

            Intervencion model = new Intervencion();

            model.ListaHistorial = this.interServicio.ReporteHistorial(fechaInicio, fechaFin, actividad).OrderByDescending(x => x.LogFecha).ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GenerarExcelHistorial(DateTime? fechaInicio, DateTime? fechaFin, string actividad)
        {
            base.ValidarSesionUsuario();

            string fileName = string.Empty;
            string path = string.Empty;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

            Intervencion model = new Intervencion();
            model.ListaHistorial = new List<SiLogDTO>();

            model.ListaHistorial = interServicio.ReporteHistorial(fechaInicio, fechaFin, actividad);

            if (model.ListaHistorial.Count == 0)
            {
                return Json("-2");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                fileName = ConstantesIntervencionesAppServicio.NombreReporteExcelLogActividadesSistema;
                interServicio.ExportarToExcelReporteHistorial(model.ListaHistorial, path, pathLogo, fileName);

                return Json(fileName);
            }
        }
        #endregion

        #region METODOS COMUNES
        #region METODO PARA DESCARGAR ARCHIVOS
        /// <summary>
        /// Función que se encarga de descargar el archivo generado al vuelo en la raiz de la aplicación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(string file)
        {
            int formato = 1;
            string fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes + file;
            string app = (formato == 1) ? ConstantesIntervencionesAppServicio.AppExcel : (formato == 2) ? ConstantesIntervencionesAppServicio.AppPdf : ConstantesIntervencionesAppServicio.AppWord;
            return File(path, app, fecha + "_" + file);
        }

        #endregion

        #region METODOS PARA COMBOS MULTI SELECCION
        /// <summary>
        /// Metodo para manejo de combos multiselección
        /// </summary>
        private string ValidarComboMultiseleccionEntero(string strCampo)
        {
            string auxiliar = strCampo;
            if (auxiliar == null || auxiliar.Equals("") || auxiliar.Equals("null"))
            {
                return auxiliar = "0";
            }

            List<int> lstAuxliar = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(strCampo);
            if (lstAuxliar != null && lstAuxliar.Count() > 0)
            {
                auxiliar = "";
                for (int i = 0; i < lstAuxliar.Count(); i++)
                {
                    auxiliar += lstAuxliar[i] + ",";
                }
                if (auxiliar.TrimEnd(',') == string.Empty)
                {
                    auxiliar = "0";
                }
                else
                {
                    auxiliar = auxiliar.TrimEnd(',');
                }
            }

            return auxiliar;
        }

        /// <summary>
        /// Metodo para manejo de combos multiselección
        /// </summary>
        private string ValidarComboMultiseleccionCadena(string strCampo)
        {
            string auxiliar = strCampo;
            if (auxiliar == null || auxiliar.Equals("") || auxiliar.Equals("null"))
            {
                auxiliar = "0";
            }

            List<string> lstAuxliar = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(strCampo);
            if (lstAuxliar != null && lstAuxliar.Count() > 0)
            {
                auxiliar = "";
                for (int i = 0; i < lstAuxliar.Count(); i++)
                {
                    auxiliar += lstAuxliar[i] + ",";
                }
                if (auxiliar.TrimEnd(',') == string.Empty)
                {
                    auxiliar = "'0'";
                }
                else
                {
                    string aux = string.Empty;
                    for (int i = 0; i < auxiliar.TrimEnd(',').Split(',').Count(); i++)
                    {
                        aux += "'" + auxiliar.TrimEnd(',').Split(',')[i] + "',";
                    }
                    auxiliar = aux.TrimEnd(',');
                }
            }

            return auxiliar;
        }
        #endregion
        #endregion        
    }
}