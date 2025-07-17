using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Monitoreo.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Monitoreo;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.App_Start;

namespace COES.MVC.Intranet.Areas.Monitoreo.Controllers
{
    [ValidarSesion]
    public class ReporteController : BaseController
    {
        IEODAppServicio servIEOD = new IEODAppServicio();
        MonitoreoAppServicio servMonitoreo = new MonitoreoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ReporteController));
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
        /// Inicio DashBoard Cuota de mercado
        /// </summary>
        /// <returns></returns>
        public ActionResult DashBoard()
        {
            IndicadorModel model = new IndicadorModel();
            try
            {
                if (!base.IsValidSesionView()) return base.RedirectToLogin();

                int numDia;
                string strFechaIni;
                this.servMonitoreo.GetFechaMaxGeneracionPermitidaDasBoard(out strFechaIni, out numDia);
                model.DiaMes = numDia;
                DateTime fecha = new DateTime(Int32.Parse(strFechaIni.Substring(3, 4)), Int32.Parse(strFechaIni.Substring(0, 2)), 1);
                model.FechaInicio = fecha.ToString(ConstantesAppServicio.FormatoFecha);

                // Devuelve lista de empresas
                var empresas = this.servIEOD.ListarEmpresasxTipoEquipos(ConstantesHorasOperacion.CodFamilias);
                model.NroSemana = EPDate.f_numerosemana(fecha);
                model.ListaEmpresas = this.servMonitoreo.ListarEmpresasMonitoreo(DateTime.Now, ConstantesAppServicio.ParametroDefecto);
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
        /// Combo de Barras Indicador 5 y 6
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fechaDia"></param>
        /// <returns></returns>
        public PartialViewResult ComboBarra(string empresa, string fechaDia, int indicador)
        {
            ReporteModel model = new ReporteModel();
            try
            {
                var barra = this.servMonitoreo.ListarBarraPorEmpresa(empresa, fechaDia);
                model.ListaBarra = barra;
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
        /// Graficos  de Cuota de mercado
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fechaDia"></param>
        /// <param name="fechaSemana"></param>
        /// <param name="fechaMes"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public JsonResult ConstruirDashboardIndicador(string empresa, string fechaDia, string fechaSemana, string fechaMes, string periodo, int indicador, string barra)
        {
            ReporteModel model = new ReporteModel();

            try
            {
                this.ValidarSesionJsonResult();
                List<MeMedicion48DTO> data = new List<MeMedicion48DTO>();
                List<MeMedicion48DTO> data2 = new List<MeMedicion48DTO>();
                empresa = string.IsNullOrEmpty(empresa) ? ConstantesAppServicio.ParametroDefecto : empresa;
                DateTime f1;
                DateTime f2;
                DateTime f3;
                DateTime f4;

                empresa = string.IsNullOrEmpty(empresa) ? ConstantesAppServicio.ParametroDefecto : empresa;
                //Rectorna una lista con data MW  y las fecha especificaras
                DateTime fechaDiaFormat = new DateTime(Int32.Parse(fechaDia.Substring(6, 4)), Int32.Parse(fechaDia.Substring(3, 2)), Int32.Parse(fechaDia.Substring(0, 2)));

                bool estado = periodo == ConstantesMonitoreo.PeriodoSemana && fechaSemana == ConstantesAppServicio.ParametroDefecto ? false : true;

                if (estado)
                {
                    this.servMonitoreo.DataDashBoardMw(out data, indicador, fechaDiaFormat, fechaDiaFormat, fechaSemana, fechaMes, periodo, out f1, out f2, out f3, out f4);
                    //Medidor cuota de mercado

                    switch (indicador)
                    {
                        case ConstantesMonitoreo.CodigoS:

                            //Medidor Grafico utilizado por Indicador 1 y 2
                            model.Grafico = this.servMonitoreo.GraficoMedidorMonitoreoCuotaHHI(empresa, f1, f2, data, indicador);
                            //Grafico  Grupo de  Hidroelectrico  y termoelectrico
                            model.GraficoBarra = this.servMonitoreo.GraficoGrupoDespacho(empresa, f1, f2, data);
                            //Total Mw Ejecutado
                            model.Resultado = this.servMonitoreo.ResultadoHtmlIndicador(empresa, f1, f2, data, indicador);
                            //Grafico de Columnas Participacion diaria cuota de Mercado y HHI 
                            model.GraficoMedidorCurva = this.servMonitoreo.GraficoColumnaCuotaMercadoHHI(ConstantesAppServicio.ParametroDefecto, f3, f4, data, indicador);
                            //Parametro de ingreso JavaScript
                            model.Indicador = ConstantesMonitoreo.CodigoS;
                            break;

                        case ConstantesMonitoreo.CodigoHHI:

                            // Grafico de Curva   HHI total  por Empresas
                            model.GraficoMedidorCurva = this.servMonitoreo.GenerarGraficoHehi(empresa, f1, f2, data);
                            //Medidor Grafico utilizado por Indicador 1 y 2
                            model.GraficoCurva = this.servMonitoreo.GraficoMedidorMonitoreoCuotaHHI(empresa, f1, f2, data, indicador);
                            //Grafico de Columnas Participacion diaria cuota de Mercado y HHI 
                            model.GraficoMedidorCurva2 = this.servMonitoreo.GraficoColumnaCuotaMercadoHHI(ConstantesAppServicio.ParametroDefecto, f3, f4, data, indicador);
                            // Total de generadores Activos HHI
                            model.Resultado = this.servMonitoreo.ResultadoHtmlIndicador(empresa, f1, f2, data, indicador);
                            //Parametro de ingreso JavaScript
                            model.Indicador = ConstantesMonitoreo.CodigoHHI;
                            break;

                        case ConstantesMonitoreo.CodigoIOP:

                            // Grafico  Indice de oferta Residual y Pivotal
                            model.GraficoMedidorCurva = this.servMonitoreo.GenerarGraficoIndiceOfertaPivotalResidual(empresa, f1, f2, data, indicador);
                            // Resultado Total HHI
                            model.Resultado = this.servMonitoreo.ResultadoHtmlIndicador(empresa, f1, f2, data, indicador);

                            //Grafico de Columnas Participacion diaria cuota de Mercado y HHI 
                            this.servMonitoreo.DataDashBoardMw(out data2, ConstantesMonitoreo.CodigoHHI, fechaDiaFormat, fechaDiaFormat, fechaSemana, fechaMes, periodo, out f1, out f2, out f3, out f4);
                            model.GraficoMedidorCurva2 = this.servMonitoreo.GraficoColumnaCuotaMercadoHHI(ConstantesAppServicio.ParametroDefecto, f3, f4, data2, ConstantesMonitoreo.CodigoHHI);
                            //Parametro de ingreso JavaScript
                            model.Indicador = ConstantesMonitoreo.CodigoIOP;
                            break;

                        case ConstantesMonitoreo.CodigoRSD:

                            //Grafico Oferta residual
                            model.GraficoMedidorCurva = this.servMonitoreo.GenerarGraficoIndiceOfertaPivotalResidual(empresa, f1, f2, data, indicador);
                            // Grafico Promedio de la oferta Residual
                            model.GraficoMedidorCurva2 = this.servMonitoreo.GenerarPromedioGraficoResidual(ConstantesAppServicio.ParametroDefecto, f1, f2, data);
                            // Resultado  Generadores Residual
                            model.Resultado = this.servMonitoreo.ResultadoHtmlIndicador(empresa, f1, f2, data, indicador);
                            //Parametro de ingreso JavaScript
                            model.Indicador = ConstantesMonitoreo.CodigoRSD;
                            break;

                        case ConstantesMonitoreo.CodigoILE:

                            //Grafico  indicador 5 y 6
                            model.GraficoMedidorCurva = this.servMonitoreo.GenerarGraficoCurvasLernerCostoPrecio(empresa, f1, f2, data, barra, indicador);
                            //Parametro de ingreso JavaScript
                            model.Indicador = ConstantesMonitoreo.CodigoILE;
                            break;

                        case ConstantesMonitoreo.CodigoIMU:

                            //Grafico  indicador 5 y 6
                            model.GraficoMedidorCurva = this.servMonitoreo.GenerarGraficoCurvasLernerCostoPrecio(empresa, f1, f2, data, barra, indicador);
                            //Parametro de ingreso JavaScript
                            model.Indicador = ConstantesMonitoreo.CodigoIMU;
                            break;
                    }
                }
                else
                {
                    model.Indicador = -1;
                }

            }
            catch (Exception ex)
            {
                model.Resultado2 = "-1";
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }
            return Json(model);
        }

        /// <summary>
        /// Carga de  fehca de semana
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho, int indicador)
        {
            ReporteModel model = new ReporteModel();
            try
            {
                List<TipoInformacion3> entitys = new List<TipoInformacion3>();
                int dia = DateTime.Now.Day;
                //resta de mes a iniciar
                int mes;
                //Devuelve meses a restar conrespondiente al dia del mes actual
                mes = dia < 6 ? mes = 2 : mes = 1;
                DateTime fechaMes = new DateTime(DateTime.Now.Year, (DateTime.Now.Month), 1);
                //Forma de fechas semanas
                DateTime dfecha = new DateTime(fechaMes.Year, (DateTime.Now.Month), fechaMes.AddMonths(1).AddDays(-1).Day).AddMonths(-mes);
                int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

                for (int i = 1; i <= nsemanas; i++)
                {
                    TipoInformacion3 reg = new TipoInformacion3();
                    reg.IdTipoInfo = (idAnho + i);
                    reg.NombreTipoInfo = "Sem" + i + "-" + idAnho;
                    entitys.Add(reg);
                }
                model.Indicador = indicador;
                model.ListaSemanas = entitys;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                //Te devuelve un mensaje de error
                model.Resultado = ex.Message;
            }
            return PartialView(model);

        }

    }
}
