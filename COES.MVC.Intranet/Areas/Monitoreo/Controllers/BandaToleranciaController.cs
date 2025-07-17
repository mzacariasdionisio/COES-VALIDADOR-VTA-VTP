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
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Monitoreo.Controllers
{
    [ValidarSesion]
    public class BandaToleranciaController : BaseController
    {
        MonitoreoAppServicio servMonitoreo = new MonitoreoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(BandaToleranciaController));
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

        #region Actualización de Banda de Tolerancia

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BandaToleranciaModel model = new BandaToleranciaModel();
            return View(model);
        }

        /// <summary>
        /// Listado Historico
        /// </summary>
        [HttpPost]
        public PartialViewResult ListarBandaTolerancia()
        {
            BandaToleranciaModel model = new BandaToleranciaModel();
            model.ListaBanda = this.servMonitoreo.ListarBandaToleranciaHistorico();

            return PartialView(model);
        }

        /// <summary>
        /// Editar ParametroHHI
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarBandaTolerancia(int id)
        {
            BandaToleranciaModel model = new BandaToleranciaModel();
            model.ListaEstado = this.servMonitoreo.ListarEstadoParametro();
            model.ListaIndicador = this.servMonitoreo.ListMmmIndicadors();

            MmmBandtolDTO reg;

            if (id == 0)
            {
                reg = new MmmBandtolDTO();
                reg.Periodo = DateTime.Now.ToString(ConstantesAppServicio.FormatoMes);
                reg.Mmmtolestado = ConstantesAppServicio.Activo;
            }
            else
            {
                reg = this.servMonitoreo.GetByIdMmmBandtol(id);
                reg = this.servMonitoreo.GetBandaToleranciaFromLista(reg, model.ListaEstado);
            }
            model.Banda = reg;

            return PartialView(model);
        }

        /// <summary>
        /// Actualizar ParametroHHI
        /// </summary>
        /// <param name="idCero"></param>
        /// <param name="idUno"></param>
        /// <param name="aCero"></param>
        /// <param name="aUno"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarBandaTolerancia(int id, int immme, string periodo, decimal valorreferencia, decimal valortolerancia, string criterio, string normativa, string estado)
        {
            ParametroModel model = new ParametroModel();
            try
            {
                this.ValidarSesionJsonResult();

                MmmBandtolDTO reg;
                if (id == 0)
                {
                    DateTime fechaVigencia = new DateTime(Int32.Parse(periodo.Substring(3, 4)), Int32.Parse(periodo.Substring(0, 2)), 1);

                    reg = new MmmBandtolDTO();
                    reg.Mmmtolfechavigencia = fechaVigencia;
                    reg.Mmmtolusucreacion = User.Identity.Name;
                    reg.Mmmtolfeccreacion = DateTime.Now;
                    reg.Immecodi = immme;

                    if (this.servMonitoreo.ExisteByIndicadorYPeriodo(reg.Immecodi, reg.Mmmtolfechavigencia))
                    {
                        throw new Exception("Ya existe la configuración para el Indicador y Fecha de vigencia seleccionados");
                    }
                }
                else
                {
                    reg = this.servMonitoreo.GetByIdMmmBandtol(id);
                    reg.Mmmtolusumodificacion = User.Identity.Name;
                    reg.Mmmtolfecmodificacion = DateTime.Now;
                }

                reg.Mmmtolvalorreferencia = valorreferencia;
                reg.Mmmtolvalortolerancia = valortolerancia;
                reg.Mmmtolcriterio = criterio != null ? criterio.Trim() : string.Empty;
                reg.Mmmtolnormativa = normativa != null ? normativa.Trim() : string.Empty;
                reg.Mmmtolestado = estado;


                if (id == 0)
                {
                    //TO DO validar existencia
                    this.servMonitoreo.SaveMmmBandtol(reg);
                }
                else
                {
                    this.servMonitoreo.UpdateMmmBandtol(reg);
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return Json(model);
        }

        /// <summary>
        /// Ver ParametroHHI
        /// </summary>
        [HttpPost]
        public PartialViewResult VerBandaTolerancia(int id)
        {
            BandaToleranciaModel model = new BandaToleranciaModel();

            MmmBandtolDTO reg = this.servMonitoreo.GetByIdMmmBandtol(id);

            reg = this.servMonitoreo.GetBandaToleranciaFromLista(reg, this.servMonitoreo.ListarEstadoParametro());
            model.Banda = reg;

            return PartialView(model);
        }

        #endregion

        #region Transgresión de las Bandas de Tolerancia del Osinergmin

        /// <summary>
        /// Página de inicio
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Transgresion()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            IndicadorModel model = new IndicadorModel();

            int numDia;
            string strFechaIni;
            this.servMonitoreo.GetFechaMaxGeneracionPermitida(out strFechaIni, out numDia);
            model.DiaMes = numDia;
            model.FechaInicio = strFechaIni;

            model.ListaEmpresas = this.servMonitoreo.ListarEmpresasMonitoreo(DateTime.Now, ConstantesAppServicio.ParametroDefecto);

            model.ListaIndicador = this.servMonitoreo.ListMmmIndicadors();

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
        public JsonResult CargarIndicadorByTipo(int tipoIndicador, string idEmpresa, int nroPagina, string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                this.ValidarSesionJsonResult();

                DateTime fechaIni = new DateTime(Int32.Parse(fechaInicial.Substring(3, 4)), Int32.Parse(fechaInicial.Substring(0, 2)), 1);
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

                string url = Url.Content("~/");

                List<MmmJustificacionDTO> listJustif;
                model.ListaResultado = this.servMonitoreo.ReporteIndicadorByTipoHtml(tipoIndicador, idEmpresa, fechaIni, fechaIni, -1, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, string.Empty, url, out listJustif);
                model.ListaJustif = listJustif;
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
        public JsonResult GenerarReporteExcelByTipo(int tipoIndicador, string idEmpresa, int nroPagina, string fechaInicial)
        {
            IndicadorModel model = new IndicadorModel();

            try
            {
                this.ValidarSesionJsonResult();

                //Obtiene id de Empresa
                idEmpresa = string.IsNullOrEmpty(idEmpresa) ? ConstantesAppServicio.ParametroDefecto : idEmpresa;

                DateTime fechaIni = new DateTime(Int32.Parse(fechaInicial.Substring(3, 4)), Int32.Parse(fechaInicial.Substring(0, 2)), 1);
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

                string nameFile;
                //Generacion de archivo general de indicador especificado por tipoIndicador
                this.servMonitoreo.GenerarArchivoExcelByTipo(tipoIndicador, idEmpresa, fechaIni, fechaIni, -1, ConstantesMonitoreo.ReportePorcentajeErrorBandaTolerancia, out nameFile);
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

        /// <summary>
        /// Combo de Empresas
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fechaDia"></param>
        /// <returns></returns>
        public PartialViewResult ComboEmpresa(string fechaDia, int indicador)
        {
            ReporteModel model = new ReporteModel();
            try
            {
                DateTime fechaIni = new DateTime(Int32.Parse(fechaDia.Substring(3, 4)), Int32.Parse(fechaDia.Substring(0, 2)), 1);
                model.ListaEmpresas = this.servMonitoreo.ListarEmpresasMonitoreo(fechaIni, ConstantesAppServicio.ParametroDefecto);
                model.Indicador = indicador;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }
            return PartialView(model);
        }

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

        #endregion

        #region Registro de Justificaciones

        /// <summary>
        /// Registrar/editar justificaciones
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarJustificacionXIndicadorYDia(string dataJsonJustif, int tipoIndicador, int nroPagina, string fechaInicial)
        {
            BandaToleranciaModel model = new BandaToleranciaModel();
            try
            {
                this.ValidarSesionJsonResult();
                DateTime fechaIni = new DateTime(Int32.Parse(fechaInicial.Substring(3, 4)), Int32.Parse(fechaInicial.Substring(0, 2)), 1);
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

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                List<MmmJustificacionModel> objData = serialize.Deserialize<List<MmmJustificacionModel>>(dataJsonJustif);

                List<MmmJustificacionDTO> listaData = new List<MmmJustificacionDTO>();
                foreach (var aux in objData) 
                {
                    MmmJustificacionDTO reg = new MmmJustificacionDTO();
                    reg.Immecodi = aux.Immecodi;
                    reg.Mjustcodi = aux.Mjustcodi;
                    reg.Barrcodi = aux.Barrcodi;
                    reg.Grupocodi = aux.Grupocodi;
                    reg.Emprcodi = aux.Emprcodi;
                    reg.Mjustdescripcion = aux.Mjustdescripcion;
                    reg.MjustfechaDesc = aux.MjustfechaDesc;

                    listaData.Add(reg);
                }

                this.servMonitoreo.GuardarJustificacionByFechaIndicador(tipoIndicador, fechaIni, fechaIni, listaData, User.Identity.Name);

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

        #endregion
    }
}
