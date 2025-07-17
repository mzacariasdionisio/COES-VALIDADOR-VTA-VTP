using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Publico.Areas.Medidores.Models;
using COES.MVC.Publico.Controllers;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Medidores;
using COES.Storage.App.Base.Core;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Medidores.Controllers
{
    public class MaximaDemandaController : BaseController
    {
        //
        // GET: /Medidores/MaximaDemanda/
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(MaximaDemandaController));
        private static string NameController = "MaximaDemandaController";

        private readonly List<Integrante> _lsIntegrante = new List<Integrante>();
        private readonly List<BloqueHorario> _listaBloqueHorario = new List<BloqueHorario>();

        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();
        MedidoresAppServicio servMedidores = new MedidoresAppServicio();
        ReporteMedidoresAppServicio servReporte = new ReporteMedidoresAppServicio();

        public MaximaDemandaController()
        {
            _lsIntegrante = new List<Integrante>(){
                    new Integrante(){Codigo= 0, Nombre= "TODOS"},
                    new Integrante(){Codigo= 1, Nombre= "COES"},
                    new Integrante(){Codigo= 10,Nombre= "NO COES"}
            };

            _listaBloqueHorario = new List<BloqueHorario> {
                new BloqueHorario(){Descripcion="Horas Punta",Bloque = "HORAS PUNTA",Tipo = ConstantesRepMaxDemanda.TipoHoraPunta},
                 new BloqueHorario(){Descripcion="Horas Fuera de Punta",Bloque = "HORAS FUERA DE PUNTA",Tipo = ConstantesRepMaxDemanda.TipoFueraHoraPunta}
            };
        }

        #region	Reporte de Máxima Demanda
        public ActionResult Index()
        {
            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("yyyy-MM");
            model.ListaEmpresas = ListarEmpresaByTipoGeneracion(-1);
            model.ListaIntegrante = this._lsIntegrante;
            var listaTipoGeneracion = this.servMedidores.ListSiTipogeneracions();
            listaTipoGeneracion = listaTipoGeneracion.Where(x => x.Tgenercodi > 0).ToList().OrderBy(x => x.Tgenernomb).ToList();
            model.ListaTipoGeneracion = listaTipoGeneracion;
            model.IdParametro = ConstantesParametro.IdParametroHPPotenciaActiva;
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();
            model.EsPortal = true;

            return View(model);
        }

        /// Listar Empresa por TipoGeneracion
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="tipoGeneracion"></param>
        /// <returns></returns>
        private List<SiEmpresaDTO> ListarEmpresaByTipoGeneracion(int tipoGeneracion)
        {
            return this.servReporte.ObteneEmpresasPorTipogeneracion(tipoGeneracion);
        }

        private List<Normativa> ListarNormativaMaximaDemanda()
        {
            List<Normativa> lista = new List<Normativa>();

            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            var listaRangoPeriodoHP = this.servParametro.GetListaParametroRangoPeriodoHP(listaParam, null).Where(x => x.Estado == ConstantesParametro.EstadoActivo || x.Estado == ConstantesParametro.EstadoBaja).ToList();
            listaRangoPeriodoHP = listaRangoPeriodoHP.OrderBy(x => x.FechaInicio).ToList();

            foreach (var reg in listaRangoPeriodoHP)
            {
                if (reg.Normativa.Length > 0)
                {
                    int pos = reg.Normativa.IndexOf(":");

                    Normativa n = new Normativa();
                    n.DescripcionFull = reg.Normativa;
                    if (pos != -1)
                    {
                        n.Nombre = (reg.Normativa.Substring(0, pos + 1)).Trim();
                        n.Descripcion = (reg.Normativa.Substring(pos + 1, reg.Normativa.Length - pos - 1)).Trim();
                    }
                    else
                    {
                        n.Nombre = string.Empty;
                        n.Descripcion = reg.Normativa;
                    }

                    lista.Add(n);
                }
            }

            return lista;
        }

        /// <summary>
        /// Generar Model de ReporteMaximaDemanda para web y excel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        private void GenerarReporteMaximaDemandaModel(ReporteMaximaDemandaModel model, int tipoCentral, int tipoGeneracion, int idEmpresa, string mes)
        {
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, string.Empty, string.Empty, string.Empty);
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
            DateTime fechaPortal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            model.EsPortal = true;
            model.FechaConsulta = DateTime.Now.ToString(ConstantesBase.FormatoFechaExtendido);

            ///
            int estadoValidacion = model.EsPortal && fechaFin == fechaPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

            DateTime diaMaximaDemanda = this.servReporte.GetDiaPeriodoDemanda96XFiltro(fechaIni, fechaFin, ConstantesRepMaxDemanda.TipoMDNormativa, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
            model.ResumenDemanda = this.servReporte.GetResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);
            model.ListaResumenDemanda = this.servReporte.ListarResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);
            model.ListaConsolidadoDemanda = this.servReporte.GetConsolidadoMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);
            model.NombreHoja = "Máxima Demanda";
            model.Titulo = "REPORTE DE MÁXIMA DEMANDA";

            model.ListaDescripcionNormativa = this.ListarDescripcionNormativa();
            model.MensajeFechaConsulta = "Fecha de Consulta: " + DateTime.Now.ToString(ConstantesBase.FormatoFechaExtendido);
            model.MensajePorcentajeConsulta = model.EsPortal ? string.Empty : this.GetMensajeCumplimiento(ConstantesMedidores.IdFormatoCargaCentralPotActiva, fechaProceso);
        }

        /// <summary>
        /// View ReporteMaximaDemanda
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ReporteMaximaDemanda(int tipoCentral, int tipoGeneracion, int idEmpresa, string mes)
        {
            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();

            this.GenerarReporteMaximaDemandaModel(model, tipoCentral, tipoGeneracion, idEmpresa, mes);

            return PartialView(model);
        }

        /// <summary>
        /// Generar archivo excel MaximaDemanda
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarMaximaDemanda(int tipoCentral, int tipoGeneracion, int idEmpresa, string mes)
        {
            string[] datos = new string[2];
            try
            {
                ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();
                GenerarReporteMaximaDemandaModel(model, tipoCentral, tipoGeneracion, idEmpresa, mes);

                string ruta = this.servReporte.GenerarFileExcelReporteMaximaDemanda(model.NombreHoja, model.Titulo, mes, model.ListaResumenDemanda, model.ListaConsolidadoDemanda, model.ListaDescripcionNormativa);
                string nombreArchivo = string.Format("{0}_{1:_HHmmss}.xlsx", "ReporteMaximaDemanda", DateTime.Now);

                datos[0] = ruta;
                datos[1] = nombreArchivo + ".xlsx";
            }
            catch (Exception ex)
            {
                datos[0] = "-1";
                datos[1] = "";
                Log.Error(NameController, ex);
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }

        #endregion

        #region Reporte de Diagrama de Carga del Día de la Máxima Demanda
        //
        // GET: /Medidores/Reportes/DiagramaCargaMaximaDemanda
        public ActionResult DiagramaCargaMaximaDemanda()
        {
            DiagramaCargaMaximaDemandaModel model = new DiagramaCargaMaximaDemandaModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("yyyy-MM");
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();

            return View(model);
        }

        /// <summary>
        /// View ReporteDiagramaCargaMaximaDemanda
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        public PartialViewResult ReporteDiagramaCargaMaximaDemanda(string mes)
        {
            DiagramaCargaMaximaDemandaModel model = new DiagramaCargaMaximaDemandaModel();
            this.GenerarDiagramaCargaMaximaDemandaModel(model, mes);

            model.Json = JsonConvert.SerializeObject(Json(model).Data);

            return PartialView(model);
        }

        /// <summary>
        /// Generar Model de DiagramaCargaMaximaDemanda para web y excel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mes"></param>
        private void GenerarDiagramaCargaMaximaDemandaModel(DiagramaCargaMaximaDemandaModel model, string mes)
        {
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, string.Empty, string.Empty, string.Empty);
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
            DateTime fechaPortal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            ///
            bool esPortal = true;
            int estadoValidacion = esPortal && fechaFin == fechaPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;
            int tipoCentral = ConstantesMedicion.IdTipogrupoCOES; //COES 
            int tipoGeneracion = ConstantesMedicion.IdTipoGeneracionTodos; //TODOS

            model.FechaConsulta = DateTime.Now.ToString(ConstantesBase.FormatoFechaExtendido);

            DateTime diaMaximaDemanda = this.servReporte.GetDiaPeriodoDemanda96XFiltro(fechaIni, fechaFin, ConstantesRepMaxDemanda.TipoMDNormativa, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);

            //Máxima demanda
            var objResumenDemanda = this.servReporte.GetResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
            model.MaximaDemanda = objResumenDemanda;

            //Lista demanda por hora
            model.ListaDemandaCuartoHora = this.servReporte.GetResumenDetalleDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion, fechaProceso);
            model.Titulo = "Día de Máxima Demanda";
            model.Leyenda = "Demanda MW";
            model.DescripcionSerie = "Demanda";
            model.ListaDescripcionNormativa = this.ListarDescripcionNormativa();
        }

        /// <summary>
        /// Generar archivo excel DiagramaCargaMaximaDemanda
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDiagramaCargaMaximaDemanda(string mes)
        {
            string[] datos = new string[2];
            try
            {
                DiagramaCargaMaximaDemandaModel model = new DiagramaCargaMaximaDemandaModel();
                this.GenerarDiagramaCargaMaximaDemandaModel(model, mes);

                string ruta = this.servReporte.GenerarFileExcelReporteDiagramaCargaMaximaDemanda(mes, model.Titulo, model.Leyenda, model.MaximaDemanda, model.ListaDemandaCuartoHora, model.ListaDescripcionNormativa);
                string nombreArchivo = string.Format("{0}_{1:_HHmmss}.xlsx", "DiagramaCargaMaximaDemanda", DateTime.Now);

                datos[0] = ruta;
                datos[1] = nombreArchivo + ".xlsx";
            }
            catch (Exception ex)
            {
                datos[0] = "-1";
                datos[1] = "";
                Log.Error(NameController, ex);
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }

        #endregion

        #region Reporte de Recurso Energético
        //
        // GET: /Medidores/Reportes/RecursoEnergetico
        public ActionResult RecursoEnergetico()
        {
            ReporteRecursoEnergeticoModel model = new ReporteRecursoEnergeticoModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("yyyy-MM");
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();

            return View(model);
        }

        /// <summary>
        /// View ReporteRecursoEnergetico
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        public PartialViewResult ReporteRecursoEnergetico(string mes)
        {
            ReporteRecursoEnergeticoModel model = new ReporteRecursoEnergeticoModel();
            this.GenerarReporteRecursoEnergeticoModel(model, mes);

            model.JsonGraficoPipe = Newtonsoft.Json.JsonConvert.SerializeObject(Json(model).Data);

            return PartialView(model);
        }

        /// <summary>
        /// Generar Model de ReporteRecursoEnergetico para web y excel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mes"></param>
        private void GenerarReporteRecursoEnergeticoModel(ReporteRecursoEnergeticoModel model, string mes)
        {
            try
            {
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, string.Empty, string.Empty, string.Empty);
                DateTime fechaIni = fechaProceso;
                DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
                DateTime fechaPortal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                ///
                bool esPortal = true;
                int estadoValidacion = esPortal && fechaFin == fechaPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;
                int tipoCentral = ConstantesMedicion.IdTipogrupoCOES; //COES 
                int tipoGeneracion = ConstantesMedicion.IdTipoGeneracionTodos; //TODOS

                model.FechaConsulta = DateTime.Now.ToString(ConstantesBase.FormatoFechaExtendido);

                DateTime diaMaximaDemanda = this.servReporte.GetDiaPeriodoDemanda96XFiltro(fechaIni, fechaFin, ConstantesRepMaxDemanda.TipoMDNormativa, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
                               

                model.ListaResumenDemanda = this.servReporte.ListarResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
                
                model.ListaConsolidadoRecursoEnergetico = this.servReporte.GetRecursoEnergeticoMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
                
                model.DemandaHP = this.servReporte.GetResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
                

                model.Titulo = "Recurso Energético utilizado en el día de la Máxima Demanda";
                model.ListaDescripcionNormativa = this.ListarDescripcionNormativa();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }
        }

        /// <summary>
        /// Generar archivo excel RecursoEnergetico
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarRecursoEnergetico(string mes)
        {
            string[] datos = new string[2];
            try
            {
                ReporteRecursoEnergeticoModel model = new ReporteRecursoEnergeticoModel();
                this.GenerarReporteRecursoEnergeticoModel(model, mes);

                string ruta = this.servReporte.GenerarFileExcelReporteRecursoEnergetico(mes, model.Titulo, model.Leyenda, model.BloqueHorario, model.ListaResumenDemanda, model.ListaConsolidadoRecursoEnergetico, model.ListaDescripcionNormativa);
                string nombreArchivo = string.Format("{0}_{1:_HHmmss}.xlsx", "RecursoEnergetico", DateTime.Now);

                datos[0] = ruta;
                datos[1] = nombreArchivo + ".xlsx";
            }
            catch (Exception ex)
            {
                datos[0] = "-1";
                datos[1] = "";
                Log.Error(NameController, ex);
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }
        #endregion

        #region	Reporte de Demanda por Periodo
        //
        // GET: /Medidores/Reportes/DemandaPeriodo
        public ActionResult DemandaPeriodo()
        {
            ReporteDemandaPeriodoModel model = new ReporteDemandaPeriodoModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaEmpresas = ListarEmpresaByTipoGeneracion(-1);
            model.ListaIntegrante = this._lsIntegrante;
            model.ListaBloqueHorario = this._listaBloqueHorario;
            var listaTipoGeneracion = this.servMedidores.ListSiTipogeneracions();
            listaTipoGeneracion = listaTipoGeneracion.Where(x => x.Tgenercodi > 0).ToList().OrderBy(x => x.Tgenernomb).ToList();
            model.ListaTipoGeneracion = listaTipoGeneracion;
            model.JsonBloqueHorario = JsonConvert.SerializeObject(Json(model.ListaBloqueHorario).Data);
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();
            model.EsPortal = true;

            return View(model);
        }

        /// <summary>
        ///  Generar Model de ReporteDemandaPeriodo para web y excel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="bloqueHorario"></param>
        private void GenerarReporteDemandaPeriodoModel(ReporteDemandaPeriodoModel model, int tipoCentral, int tipoGeneracion, int idEmpresa, string mes, int bloqueHorario)
        {
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
            DateTime fechaPortal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            bool esPortal = true; //User.Identity.Name.Length == 0;
            int estadoValidacion = esPortal && fechaFin == fechaPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

            model.FechaConsulta = DateTime.Now.ToString(ConstantesBase.FormatoFechaExtendido);

            DateTime diaMaximaDemanda = this.servReporte.GetDiaPeriodoDemanda96XFiltro(fechaIni, fechaFin, bloqueHorario, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
            model.ListaResumenDemanda = this.servReporte.ListarResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
            model.ListaConsolidadoDemanda = this.servReporte.GetConsolidadoMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion);
            model.NombreHoja = "Demanda Periodo";
            string descripcion = this._listaBloqueHorario.Where(x => x.Tipo == bloqueHorario).Select(x => x.Descripcion).FirstOrDefault();
            model.Titulo = "Reporte del Día de la Máxima Demanda en " + descripcion;

            model.ListaDescripcionNormativa = this.ListarDescripcionNormativa();
            model.MensajeFechaConsulta = "Fecha de Consulta: " + DateTime.Now.ToString(ConstantesBase.FormatoFechaExtendido);
            model.MensajePorcentajeConsulta = this.GetMensajeCumplimiento(ConstantesMedidores.IdFormatoCargaCentralPotActiva, fechaProceso);
        }

        /// <summary>
        /// View ReporteDemandaPeriodo
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="bloqueHorario"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ReporteDemandaPeriodo(int tipoCentral, int tipoGeneracion, int idEmpresa, string mes, int bloqueHorario)
        {
            ReporteDemandaPeriodoModel model = new ReporteDemandaPeriodoModel();

            this.GenerarReporteDemandaPeriodoModel(model, tipoCentral, tipoGeneracion, idEmpresa, mes, bloqueHorario);

            return PartialView(model);
        }

        /// <summary>
        /// Generar archivo excel DemandaPeriodo
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="bloqueHorario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDemandaPeriodo(int tipoCentral, int tipoGeneracion, int idEmpresa, string mes, int bloqueHorario)
        {
            string[] datos = new string[2];
            try
            {
                ReporteDemandaPeriodoModel model = new ReporteDemandaPeriodoModel();
                GenerarReporteDemandaPeriodoModel(model, tipoCentral, tipoGeneracion, idEmpresa, mes, bloqueHorario);

                string ruta = this.servReporte.GenerarFileExcelReporteMaximaDemanda(model.NombreHoja, model.Titulo, mes, model.ListaResumenDemanda, model.ListaConsolidadoDemanda, model.ListaDescripcionNormativa);
                string nombreArchivo = string.Format("{0}_{1:_HHmmss}.xlsx", "ReporteDemandaPeriodo", DateTime.Now);

                datos[0] = ruta;
                datos[1] = nombreArchivo + ".xlsx";
            }
            catch (Exception ex)
            {
                datos[0] = "-1";
                datos[1] = "";
                Log.Error(NameController, ex);
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }
        #endregion

        #region util

        private List<string> ListarDescripcionNormativa()
        {
            List<string> listaDescripcionNormativa = new List<string>();

            var listaNormativa = this.ListarNormativaMaximaDemanda();
            foreach (var reg in listaNormativa)
            {
                listaDescripcionNormativa.Add(reg.DescripcionFull);
            }

            return listaDescripcionNormativa;
        }

        private string GetMensajeCumplimiento(int formatcodi, DateTime fechaProceso)
        {
            string mensaje = string.Empty;

            var listaEmpresa = this.servMedidores.GetListaEmpresaFormato(formatcodi);
            var listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            int totalEmp = listaEmpresa.Count;
            if (totalEmp > 0)
            {
                string strEmprcodi = string.Join(",", listaEmprcodi);
                int totalEnvioEmp = this.servFormato.GetReporteTotalEnvioCumplimiento(strEmprcodi, formatcodi, fechaProceso, fechaProceso).Count;

                if (DateTime.Now.AddMonths(-1).Month == fechaProceso.Month && DateTime.Now.AddMonths(-1).Year == fechaProceso.Year)
                {
                    if ((totalEnvioEmp * 100) / totalEmp != 100)
                    {
                        string porcentaje = ((totalEnvioEmp * 100) / totalEmp).ToString();
                        mensaje = "Reporte al " + porcentaje + "% de envío de información de empresas ";
                    }
                }
            }

            return mensaje;
        }

        /// <summary>
        /// Listar Empresa por TipoGeneracion
        /// </summary>
        /// <param name="tipoGeneracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEmpresaXTipoGeneracion(int tipoGeneracion)
        {
            return Json(ListarEmpresaByTipoGeneracion(tipoGeneracion));
        }

        /// <summary>
        /// Existe RangoAnalisis
        /// </summary>
        /// <param name="idParametro"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRangoAnalisis(int idParametro, string mes)
        {
            string retorno = "false";
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            SiParametroValorDTO paramHora = null;
            List<SiParametroValorDTO> listaRango = this.servParametro.ListSiParametroValorByIdParametro(idParametro);

            switch (idParametro)
            {
                case ConstantesParametro.IdParametroHPPotenciaActiva:
                    paramHora = this.servParametro.GetParametroHPPotenciaActiva(listaRango, fechaProceso, ParametrosFormato.ResolucionCuartoHora);
                    break;
                case ConstantesParametro.IdParametroRangoPotenciaInductiva:
                    paramHora = this.servParametro.GetParametroHPPotenciaInductiva(listaRango, fechaProceso, ParametrosFormato.ResolucionCuartoHora);
                    break;
            }

            if (paramHora != null)
            {
                retorno = "true";
            }

            return Json(retorno);
        }

        /// <summary>
        /// Descargar el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivo(string rutaArchivoTemp, string nombreArchivo)
        {
            byte[] buffer = null;

            if (System.IO.File.Exists(rutaArchivoTemp))
            {
                buffer = System.IO.File.ReadAllBytes(rutaArchivoTemp);
                System.IO.File.Delete(rutaArchivoTemp);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
        }
        #endregion
    }
}
