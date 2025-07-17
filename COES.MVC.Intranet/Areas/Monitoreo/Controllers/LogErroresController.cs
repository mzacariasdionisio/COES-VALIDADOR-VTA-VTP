using COES.Dominio.DTO.Sic;
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

namespace COES.MVC.Intranet.Areas.Monitoreo.Controllers
{
    [ValidarSesion]
    public class LogErroresController : BaseController
    {
        MonitoreoAppServicio servMonitoreo = new MonitoreoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(LogErroresController));
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

        /// <summary>
        /// Inicio INDEX monitoreo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            IndicadorModel model = new IndicadorModel();
            try
            {
                int numDia;
                string strFechaIni;
                this.servMonitoreo.GetFechaMaxGeneracionPermitida(out strFechaIni, out numDia);
                model.DiaMes = numDia;
                model.FechaInicio = strFechaIni;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Combo versiones Indicadores
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        public PartialViewResult ComboVersion(string fecha, int indicador)
        {

            IndicadorModel model = new IndicadorModel();
            try
            {
                //Modificar

                DateTime fechaIni = new DateTime(Int32.Parse(fecha.Substring(3, 4)), Int32.Parse(fecha.Substring(0, 2)), 1);
                model.ListaVersion = this.servMonitoreo.ListMmmVersionComboLogErrores(fechaIni);
                model.Indicador = indicador;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                //Te devuelve un mensaje de error
                model.Resultado = ex.Message;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Reporte Log de errores Indicadores
        /// </summary>
        /// <param name="tipoIndicador"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="nroPagina"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarIndicadorByTipo(int tipoIndicador, string idEmpresa, int nroPagina, int id)
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                this.ValidarSesionJsonResult();

                if (id != 0)
                {
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
                    model.Resultado = "1";
                    int respuesta = ConstantesMonitoreo.LogErroresMonitoreo;
                    List<MmmJustificacionDTO> listJustif;
                    model.ListaResultado = this.servMonitoreo.ReporteIndicadorByTipoHtml(tipoIndicador, idEmpresa, fechaIni, fechaIni, id, respuesta, string.Empty, string.Empty, out listJustif);
                }
                else
                {
                    model.Resultado = "-1";
                }
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
        /// Combo de Empresa
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

    }
}
