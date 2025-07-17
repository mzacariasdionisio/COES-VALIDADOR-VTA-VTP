using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.Monitoreo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Monitoreo;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Monitoreo.Controllers
{
    [ValidarSesion]
    public class IndicadoresController : BaseController
    {
        MonitoreoAppServicio servMonitoreo = new MonitoreoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(IndicadoresController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Protected de log de errores page
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

        #region Generación de Versión
        /// <summary>
        /// Index generacion
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult Index(int? periodo)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                if (!base.IsValidSesion) return base.RedirectToLogin();

                //Lista generacion
                model.Resultado4 = this.servMonitoreo.NoExisteGeneracionEnProceso();

                int numDia;
                string strFechaIni;
                this.servMonitoreo.GetFechaMaxGeneracionPermitida(out strFechaIni, out numDia);

                model.DiaMes = numDia;
                if (periodo != null)
                {
                    string sPeriodo = (periodo.Value + string.Empty).Trim();
                    int anio = Int32.Parse(sPeriodo.Substring(0, 4));
                    int mes = Int32.Parse(sPeriodo.Substring(4, sPeriodo.Length - 4));

                    model.FechaInicio = new DateTime(anio, mes, 1).ToString(ConstantesAppServicio.FormatoMes);
                }
                else
                {
                    model.FechaInicio = strFechaIni;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Crea generacion
        /// </summary>
        /// <param name="motivo"></param>
        /// <returns></returns>
        public JsonResult SaveGenerador(string motivo, string mes)
        {
            DateTime fechaIni = new DateTime(Int32.Parse(mes.Substring(3, 4)), Int32.Parse(mes.Substring(0, 2)), 1);
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);
            //DateTime fechaIni = new DateTime(Int32.Parse(mes.Substring(3, 4)), Int32.Parse(mes.Substring(0, 2)), 8);
            //DateTime fechaFin = fechaIni;
            DateTime fechaPeriodo = new DateTime(fechaIni.Year, fechaIni.Month, 1).Date;

            IndicadorModel model = new IndicadorModel();
            try
            {
                this.ValidarSesionJsonResult();

                if (motivo == null || motivo.Trim() == string.Empty)
                {
                    throw new Exception("Debe ingresar descripción.");
                }
                //Obtiene infomracion de generacion
                motivo = string.IsNullOrEmpty(motivo) ? string.Empty : motivo.Trim();
                DateTime fechaGeneracion = DateTime.Now;
                string usuario = User.Identity.Name;

                //Si la generacion es diferente al 100% nos devolvera valor 0 y no permitira el registro
                model.Resultado4 = this.servMonitoreo.NoExisteGeneracionEnProceso();
                //  Guarda nueva generacion
                if (model.Resultado4 == 1)
                {
                    //Lista las versiones de generacion
                    List<MmmVersionDTO> listVersion = servMonitoreo.GetByCriteriaMmmVersions().Where(x => x.Vermmfechaperiodo == fechaPeriodo).ToList();
                    MmmVersionDTO reg = new MmmVersionDTO();
                    reg.Vermmfechaperiodo = fechaPeriodo;
                    reg.Vermmusucreacion = usuario;
                    reg.Vermmestado = ConstantesMonitoreo.TipoFormularioPorAprobar;
                    reg.Vermmmotivoportal = ConstantesMonitoreo.TipoFormularioNoPortalWeb;
                    reg.Vermmfechageneracion = null;
                    reg.Vermmfeccreacion = fechaGeneracion;
                    //Obtiene el nuevo id de versioon
                    reg.Vermmnumero = listVersion.Count + 1;
                    reg.Vermmmotivo = motivo;
                    //Registra la nueva version
                    int vermmcodi = this.servMonitoreo.SaveMmmVersion(reg);
                    reg.Vermmcodi = vermmcodi;

                    //Generacion de version en segundo plano
                    HostingEnvironment.QueueBackgroundWorkItem(
                    token => SaveFacTableSegundoPlano(fechaIni, fechaFin, reg, usuario, token));
                }
            }
            catch (Exception ex)
            {
                model.Resultado = ex.Message;
                model.Resultado4 = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Procesos segundo plano Generacion de version y exportancion Excel
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaIni2"></param>
        /// <param name="dato"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SaveFacTableSegundoPlano(DateTime fechaIni, DateTime fechaFin, MmmVersionDTO version, string usuario, CancellationToken cancellationToken)
        {
            try
            {
                //Segundo plano Generacion de version
                this.servMonitoreo.GenerarVersionIndicadoresMME(fechaIni, fechaFin, version, usuario);
            }
            catch (Exception ex)
            {
                string msjException = ex.Message;
                version.Vermmmsjgeneracion = "Ocurrió un error cuando se realizaba el proceso.\nLa generación de la versión empezó a las " + version.Vermmfeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull2)
                    + " y se terminó a las " + DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2) + " cuando estaba al " + version.Vermmporcentaje + "%.\nEl motivo fue el siguiente: " + msjException;
                version.Vermmporcentaje = -1;
                version.Vermmmsjgeneracion = version.Vermmmsjgeneracion.Trim();
                if (version.Vermmmsjgeneracion.Length > 500)
                    version.Vermmmsjgeneracion = version.Vermmmsjgeneracion.Substring(0, 500);
                servMonitoreo.UpdateMmmVersionPorcentaje(version);
                Log.Error(NameController, ex);
            }
        }

        /// <summary>
        /// ConsultaGeneracion
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ConsultarGenerador(string fecha)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                this.ValidarSesionJsonResult();
                //Obtiener informacion de la consulta del generador
                DateTime fechaIni = new DateTime(Int32.Parse(fecha.Substring(3, 4)), Int32.Parse(fecha.Substring(0, 2)), 1);
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);
                //Lista
                model.Resultado4 = this.servMonitoreo.NoExisteGeneracionEnProceso();
                string url = Url.Content("~/");
                //Consulta de las versiones dependiente del periodo 
                model.Resultado = this.servMonitoreo.ReporteListadoVersionHtml(fechaIni, fechaFin, url);
            }
            catch (Exception ex)
            {
                // model.Resultado = ex.Message;
                model.Resultado = "-1";
                model.Resultado2 = ex.Message;
                model.Resultado3 = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// MuestraPopup Generacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult PopupEditarGenerador(int id)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                this.ValidarSesionJsonResult();
                //Te devuelve un objeto  class de los datos de generacion
                model.Generador = this.servMonitoreo.GetByIdMmmVersion(id);
            }
            catch (Exception ex)
            {
                model.Motivo = string.Empty;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Editar Generacion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="estado"></param>
        /// <param name="portal"></param>
        /// <returns></returns>
        /// 
        public JsonResult EditGeneracion(int id, int estado, int portal, string motivo)
        {
            MmmVersionDTO model = this.servMonitoreo.GetByIdMmmVersion(id);
            try
            {
                this.ValidarSesionJsonResult();
                //Obtiene los valores  para editar generacion
                model.Vermmestado = estado;
                model.Vermmmotivo = motivo;
                model.Vermmusumodificacion = User.Identity.Name;
                model.Vermmfecmodificacion = DateTime.Now;

                if (estado == ConstantesMonitoreo.TipoFormularioPublicado)
                {
                    model.Vermmfechaaprobacion = DateTime.Now;
                }
                else
                {
                    model.Vermmfechaaprobacion = null;
                }

                model.Vermmmotivoportal = portal;

                //Edita generacion especifica por id
                this.servMonitoreo.UpdateMmmVersionPeriodo(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                //Te devuelve un mensaje de error
                model.Vermmmotivoportal = 0;
            }
            return Json(model);
        }

        #endregion

        #region Reporte de Indicadores MME

        /// <summary>
        /// Página de inicio
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Reporte(int id)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            IndicadorModel model = new IndicadorModel();
            MmmVersionDTO modelID = this.servMonitoreo.GetByIdMmmVersion(id);

            int anio = modelID.Vermmfechaperiodo.Year;
            int mes = modelID.Vermmfechaperiodo.Month;

            model.MesPeriodo = anio + string.Empty + mes;
            model.FechaInicio = modelID.Vermmfechaperiodo.ToString(ConstantesAppServicio.FormatoMes);
            model.Periodo = EPDate.f_NombreMes(mes).ToUpper() + " " + anio + " (Versión N° " + modelID.Vermmnumero + ")";

            model.ListaEmpresas = this.servMonitoreo.ListarEmpresasMonitoreo(modelID.Vermmfechaperiodo, ConstantesAppServicio.ParametroDefecto);

            model.Id = modelID.Vermmcodi;

            string nombreExcel, file;
            this.servMonitoreo.GetNombreYRutaArchivoReporteGeneracion(modelID, out nombreExcel, out file);
            model.NombreArchivo = nombreExcel;
            FileInfo newFile = new FileInfo(file);
            model.Estado = newFile.Exists;

            return PartialView(model);
        }

        /// <summary>
        /// Carga el html del Indicador
        /// </summary>
        /// <param name="tipoIndicador"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="strfecha"></param>
        /// <param name="nroPagina"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarIndicadorByTipo(int tipoIndicador, string idEmpresa, int nroPagina, int id)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                this.ValidarSesionJsonResult();

                //Obtien fecha inicio y fin
                MmmVersionDTO modelID = this.servMonitoreo.GetByIdMmmVersion(id);
                DateTime fechaIni = modelID.Vermmfechaperiodo;
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

                List<DateTime> listFechas = new List<DateTime>();
                for (var f = fechaIni.Date; f <= fechaFin; f = f.AddDays(1))
                {
                    listFechas.Add(f);
                }
                if (listFechas.Count > 0)
                {
                    fechaIni = listFechas[nroPagina - 1];
                }

                idEmpresa = string.IsNullOrEmpty(idEmpresa) ? ConstantesAppServicio.ParametroDefecto : idEmpresa;
                //Consulta indicadores correspecto al filtro seleccionado
                List<MmmJustificacionDTO> listJustif;
                model.ListaResultado = this.servMonitoreo.ReporteIndicadorByTipoHtml(tipoIndicador, idEmpresa, fechaIni, fechaIni, id, ConstantesMonitoreo.ReportesIndicadores, string.Empty, string.Empty, out listJustif);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Paginar indicador 1
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoIndicador1(string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                model.IndicadorPagina = false;

                int nroRegistros = this.NumeroPaginaIndicador(fechaInicial);

                if (nroRegistros > 0)
                {
                    int nroPaginas = nroRegistros;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Paginas indicador 2
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoIndicador2(string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                model.IndicadorPagina = false;

                int nroRegistros = this.NumeroPaginaIndicador(fechaInicial);

                if (nroRegistros > 0)
                {
                    int nroPaginas = nroRegistros;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Paginar indicador 3
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoIndicador3(string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                model.IndicadorPagina = false;

                int nroRegistros = this.NumeroPaginaIndicador(fechaInicial);

                if (nroRegistros > 0)
                {
                    int nroPaginas = nroRegistros;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Paginar indicador 4
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoIndicador4(string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                model.IndicadorPagina = false;

                int nroRegistros = this.NumeroPaginaIndicador(fechaInicial);

                if (nroRegistros > 0)
                {
                    int nroPaginas = nroRegistros;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Indicador 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoIndicador5(string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                model.IndicadorPagina = false;

                int nroRegistros = this.NumeroPaginaIndicador(fechaInicial);

                if (nroRegistros > 0)
                {
                    int nroPaginas = nroRegistros;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Indicador 6
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoIndicador6(string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                model.IndicadorPagina = false;

                int nroRegistros = this.NumeroPaginaIndicador(fechaInicial);

                if (nroRegistros > 0)
                {
                    int nroPaginas = nroRegistros;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Indicador 7
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public PartialViewResult PaginadoIndicador7(string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                model.IndicadorPagina = false;

                int nroRegistros = this.NumeroPaginaIndicador(fechaInicial);

                if (nroRegistros > 0)
                {
                    int nroPaginas = nroRegistros;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Obtener el numero de dias del mes
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        private int NumeroPaginaIndicador(string mes)
        {
            DateTime fechaIni = new DateTime(Int32.Parse(mes.Substring(3, 4)), Int32.Parse(mes.Substring(0, 2)), 1);
            DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

            List<DateTime> listFechas = new List<DateTime>();
            for (var f = fechaIni.Date; f <= fechaFin; f = f.AddDays(1))
            {
                listFechas.Add(f);
            }

            return listFechas.Count;
        }

        /// <summary>
        /// Exportar excel del Indicador
        /// </summary>
        /// <param name="tipoIndicador"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteExcelByTipo(int tipoIndicador, string idEmpresa, int id)
        {
            IndicadorModel model = new IndicadorModel();

            try
            {
                this.ValidarSesionJsonResult();
                //Obtiene id de Empresa
                idEmpresa = string.IsNullOrEmpty(idEmpresa) ? ConstantesAppServicio.ParametroDefecto : idEmpresa;
                MmmVersionDTO modelID = this.servMonitoreo.GetByIdMmmVersion(id);
                DateTime fechaIni = modelID.Vermmfechaperiodo;
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

                string nameFile;
                //Generacion de archivo general de indicador especificado por tipoIndicador
                this.servMonitoreo.GenerarArchivoExcelByTipo(tipoIndicador, idEmpresa, fechaIni, fechaFin, id, ConstantesMonitoreo.ReportesIndicadores, out nameFile);
                model.Resultado = nameFile;
                model.Total = 1;
            }
            catch (Exception ex)
            {
                model.Total = -1;
                model.Resultado2 = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        #endregion

        #region Exportación

        /// <summary>
        /// Descarga de archivos Excel
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXls(string nameFile)
        {
            //Exportacion de Excel de un Indicador en específico
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMonitoreo.Directorio;
            string fullPath = ruta + nameFile;
            return File(fullPath, ConstantesMonitoreo.AppExcel, nameFile);
        }

        /// <summary>
        /// Exportacion  de archivo de los 7 indicadores 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteIndicadores(string nombreArchivo)
        {
            //Exportacion de Excel de los 7 indicadores
            String file = ConfigurationManager.AppSettings[ConstantesMonitoreo.RutaExcelIndicadores] + ConstantesMonitoreo.RptExcelGeneralIndicadores + nombreArchivo + ConstantesMonitoreo.ExtensionExcel;
            return File(file, ConstantesMonitoreo.AppExcel, ConstantesMonitoreo.RptExcelGeneralIndicadores + nombreArchivo + ConstantesMonitoreo.ExtensionExcel);
        }
        #endregion
    }
}
