using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Medidores.Helpers;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Medidores;
using COES.Servicios.Aplicacion.ServicioRPF;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class ReportesController : BaseController
    {
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();
        MedidoresAppServicio servMedidores = new MedidoresAppServicio();
        ReporteMedidoresAppServicio servReporte = new ReporteMedidoresAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ReportesController));
        private static string NameController = "ReportesController";
        private readonly List<Integrante> _lsIntegrante = new List<Integrante>();
        private readonly List<BloqueHorario> _listaBloqueHorario = new List<BloqueHorario>();
        private readonly List<PeriodoDato> LsPeriodo = new List<PeriodoDato>();
        private readonly List<FuenteInformacion> LsFuente1 = new List<FuenteInformacion>();
        private readonly List<FuenteInformacion> LsFuente2 = new List<FuenteInformacion>();
        private readonly FuenteInformacion F1 = new FuenteInformacion();
        private readonly FuenteInformacion F2 = new FuenteInformacion();
        private readonly FuenteInformacion F3 = new FuenteInformacion();
        private readonly FuenteInformacion F4 = new FuenteInformacion();
        private readonly FuenteInformacion F5 = new FuenteInformacion();

        public ReportesController()
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

            LsPeriodo.Add(new PeriodoDato { Codigo = ConstantesMedidores.PeriodoTodos, Valor = ConstantesMedidores.DescPeriodoTodos });
            LsPeriodo.Add(new PeriodoDato { Codigo = ConstantesMedidores.PeriodoHp, Valor = ConstantesMedidores.DescPeriodoHp });
            LsPeriodo.Add(new PeriodoDato { Codigo = ConstantesMedidores.PeriodoHfp, Valor = ConstantesMedidores.DescPeriodoHfp });

            F1 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteMedidores,
                Valor = ConstantesMedidores.DescFuenteMedidores,
                Nombre = ConstantesMedidores.DescNombreMedidores,
                Leyenda = ConstantesMedidores.DescLeyendaMedidores,
                Titulo = ConstantesMedidores.DescTituloMedidores
            };
            F2 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteCaudalTurbinado,
                Valor = ConstantesMedidores.DescFuenteCaudalTurbinado,
                Nombre = ConstantesMedidores.DescNombreCaudalTurbinado,
                Leyenda = ConstantesMedidores.DescLeyendaCaudalTurbinado,
                Titulo = ConstantesMedidores.DescTituloCaudalTurbinado
            };
            F3 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteDespachoDiario,
                Valor = ConstantesMedidores.DescFuenteDespachoDiario,
                Nombre = ConstantesMedidores.DescNombreDespachoDiario,
                Leyenda = ConstantesMedidores.DescLeyendaDespachoDiario,
                Titulo = ConstantesMedidores.DescTituloDespachoDiario
            };
            F4 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteDatosScada,
                Valor = ConstantesMedidores.DescFuenteDatosScada,
                Nombre = ConstantesMedidores.DescNombreDatosScada,
                Leyenda = ConstantesMedidores.DescLeyendaDatosScada,
                Titulo = ConstantesMedidores.DescTituloDatosScada
            };
            F5 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteRPF,
                Valor = ConstantesMedidores.DescFuenteRPF,
                Nombre = ConstantesMedidores.DescNombreRPF,
                Leyenda = ConstantesMedidores.DescLeyendaRPF,
                Titulo = ConstantesMedidores.DescTituloRPF
            };

            //intranet
            LsFuente1.Add(F1);

            LsFuente2.Add(F2);
            LsFuente2.Add(F3);
            LsFuente2.Add(F4);
            LsFuente2.Add(F5);
        }

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

        #region Máxima Demanda Diaria

        /// <summary>
        /// <summary>
        /// Muestra la pantalla del reporte de máxima demanda
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            BusquedaMaximaDemandaModel model = new BusquedaMaximaDemandaModel();
            model.FechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaTipoGeneracion = (new ConsultaMedidoresAppServicio()).ListaTipoGeneracion();
            model.ListaTipoEmpresas = (new ConsultaMedidoresAppServicio()).ListaTipoEmpresas();
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();
            model.ParametroDefecto = 0;
            return View(model);
        }

        /// <summary>
        /// Permite cargar las empresas por los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            BusquedaMaximaDemandaModel model = new BusquedaMaximaDemandaModel();
            List<SiEmpresaDTO> entitys = (new MedidoresAppServicio()).ObteneEmpresasPorTipo(tiposEmpresa);
            model.ListaEmpresas = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Muestra el reporte de máxima demanda diaria
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MaximaDemandaDiaria(string fecha, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            string GrupoActual = "X";
            int dia = 0;
            int diaAnt = 0;
            int mes;
            int anho;
            int totdias = 0;
            List<MaximaDemandaDia> listaMaximaDemandaDia = new List<MaximaDemandaDia>();
            List<MaximaDemandaDia> listaMaximaDemandaDiaTotalResumen = new List<MaximaDemandaDia>();
            MaximaDemandaDia regMaxDia = new MaximaDemandaDia();
            MaximaDemandaDia regMaxDiaHoramin = new MaximaDemandaDia();
            MaximaDemandaDia regMaxDiaTotal = new MaximaDemandaDia();
            MaximaDemandaDia regMaxDiaImport = new MaximaDemandaDia();
            MaximaDemandaDia regMaxDiaExport = new MaximaDemandaDia();

            regMaxDiaTotal.valores = new List<decimal>();
            regMaxDiaTotal.Gruponomb = "TOTAL";

            regMaxDiaHoramin.horamin = new List<string>();
            regMaxDiaHoramin.Gruponomb = "HORA";
            regMaxDiaImport.Gruponomb = "IMPORTACIÓN";
            regMaxDiaExport.Gruponomb = "EXPORTACIÓN";
            regMaxDiaImport.valores = new List<decimal>();
            regMaxDiaExport.valores = new List<decimal>();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            if (fecha != null)
            {
                mes = Int32.Parse(fecha.Substring(0, 2));
                anho = Int32.Parse(fecha.Substring(3, 4));
                fechaInicio = new DateTime(anho, mes, 1);
                fechaFin = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).AddMonths(1).AddDays(-1);
                totdias = fechaFin.Day - fechaInicio.Day + 1;
                for (int i = 0; i < totdias; i++)
                {
                    regMaxDiaTotal.valores.Add(0M);
                    regMaxDiaHoramin.horamin.Add("--");
                }
            }

            var lista = (new MedidoresAppServicio()).ListarDemandaPorDia(fechaInicio, fechaFin, tiposEmpresa, empresas, tiposGeneracion, central);
            var listaGenPeru = lista.Where(x => x.DestinoPotencia == ConstantesMedicion.GeneracionPeru).ToList();
            foreach (var reg in listaGenPeru)
            {
                if (GrupoActual != reg.Gruponomb.Trim())
                {
                    if (GrupoActual != "X")
                    {
                        listaMaximaDemandaDia.Add(regMaxDia);
                        diaAnt = 0;
                    }
                    regMaxDia = new MaximaDemandaDia();
                    regMaxDia.valores = new List<decimal>();
                    regMaxDia.Empresanomb = reg.Empresanomb.Trim();
                    regMaxDia.Centralnomb = reg.Centralnomb.Trim();
                    regMaxDia.Gruponomb = reg.Gruponomb.Trim();
                    if (reg.Tipogeneracion != null)
                        regMaxDia.Tipogeneracion = reg.Tipogeneracion.Trim();
                    else
                        reg.Tipogeneracion = "";
                    dia = reg.Medifecha.Day;
                    for (var i = diaAnt + 1; i < dia; i++)
                    {
                        regMaxDia.valores.Add(0M);
                    }
                    regMaxDia.valores.Add((decimal)reg.Valor);
                    regMaxDiaTotal.valores[dia - 1] += (decimal)reg.Valor;
                    regMaxDiaHoramin.horamin[dia - 1] = reg.Medifecha.AddMinutes(reg.HMax * 15).ToString("HH:mm");
                    diaAnt = dia;
                }
                else
                {
                    dia = reg.Medifecha.Day;
                    for (var i = diaAnt + 1; i < dia; i++)
                        regMaxDia.valores.Add(0M);
                    regMaxDia.valores.Add((decimal)reg.Valor);
                    regMaxDiaTotal.valores[dia - 1] += (decimal)reg.Valor;
                    regMaxDiaHoramin.horamin[dia - 1] = reg.Medifecha.AddMinutes(reg.HMax * 15).ToString("HH:mm");
                    diaAnt = dia;
                }
                GrupoActual = reg.Gruponomb.Trim();
            }
            //
            for (var i = 0; i < totdias; i++)
            {
                var fechaPiv = fechaInicio.AddDays(i);
                var reg = lista.Find(x => x.DestinoPotencia == ConstantesMedicion.ImportacionEcuador && x.Medifecha == fechaPiv);
                if (reg != null)
                    regMaxDiaImport.valores.Add((decimal)reg.Valor * -4);
                else
                    regMaxDiaImport.valores.Add(0M);
                var regEx = lista.Find(x => x.DestinoPotencia == ConstantesMedicion.ExportacionEcuador && x.Medifecha == fechaPiv);
                if (regEx != null)
                    regMaxDiaExport.valores.Add((decimal)regEx.Valor * 4);
                else
                    regMaxDiaExport.valores.Add(0M);

                regMaxDiaTotal.valores[i] += regMaxDiaImport.valores[i] - regMaxDiaExport.valores[i];
            }
            listaMaximaDemandaDiaTotalResumen.Add(regMaxDiaExport);
            listaMaximaDemandaDiaTotalResumen.Add(regMaxDiaImport);
            if (listaGenPeru.Count > 0)
            {
                listaMaximaDemandaDia.Add(regMaxDia);
                listaMaximaDemandaDiaTotalResumen.Add(regMaxDiaHoramin);
                listaMaximaDemandaDiaTotalResumen.Add(regMaxDiaTotal);
            }

            var vm = new MaximaDemanda
            {
                ListaDemandaDia = listaMaximaDemandaDia,
                ListaDemandaDiaTotalResumen = listaMaximaDemandaDiaTotalResumen,
                ndiasXMes = totdias
            };

            return PartialView(vm);
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
        public JsonResult Exportar(string fecha, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            try
            {
                #region Calculo

                string GrupoActual = "X";
                int dia = 0;
                int diaAnt = 0;
                int mes;
                int anho;
                int totdias = 0;
                List<MaximaDemandaDia> listaMaximaDemandaDia = new List<MaximaDemandaDia>();
                List<MaximaDemandaDia> listaMaximaDemandaDiaTotalResumen = new List<MaximaDemandaDia>();
                MaximaDemandaDia regMaxDia = new MaximaDemandaDia();
                MaximaDemandaDia regMaxDiaTotal = new MaximaDemandaDia();
                MaximaDemandaDia regMaxDiaImport = new MaximaDemandaDia();
                MaximaDemandaDia regMaxDiaExport = new MaximaDemandaDia();
                MaximaDemandaDia regMaxDiaHoramin = new MaximaDemandaDia();
                regMaxDiaTotal.Gruponomb = "TOTAL";
                regMaxDiaHoramin.Gruponomb = "HORA";
                regMaxDiaImport.Gruponomb = "IMPORTACIÓN";
                regMaxDiaExport.Gruponomb = "EXPORTACIÓN";
                regMaxDiaImport.valores = new List<decimal>();
                regMaxDiaExport.valores = new List<decimal>();
                regMaxDiaTotal.valores = new List<decimal>();
                regMaxDiaHoramin.horamin = new List<string>();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                if (fecha != null)
                {
                    mes = Int32.Parse(fecha.Substring(0, 2));
                    anho = Int32.Parse(fecha.Substring(3, 4));
                    fechaInicio = new DateTime(anho, mes, 1);
                    fechaFin = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).AddMonths(1).AddDays(-1);
                    totdias = fechaFin.Day - fechaInicio.Day + 1;
                    for (int i = 0; i < totdias; i++)
                    {
                        regMaxDiaTotal.valores.Add(0M);
                        regMaxDiaHoramin.horamin.Add("--");
                    }
                }

                var lista = (new MedidoresAppServicio()).ListarDemandaPorDia(fechaInicio, fechaFin, tiposEmpresa, empresas, tiposGeneracion, central);
                var listaGenPeru = lista.Where(x => x.DestinoPotencia == ConstantesMedicion.GeneracionPeru).ToList();
                foreach (var reg in listaGenPeru)
                {
                    if (GrupoActual != reg.Gruponomb.Trim())
                    {
                        if (GrupoActual != "X")
                        {
                            listaMaximaDemandaDia.Add(regMaxDia);
                            diaAnt = 0;
                        }
                        regMaxDia = new MaximaDemandaDia();
                        regMaxDia.valores = new List<decimal>();
                        regMaxDia.Empresanomb = reg.Empresanomb.Trim();
                        regMaxDia.Centralnomb = reg.Centralnomb.Trim();
                        regMaxDia.Gruponomb = reg.Gruponomb.Trim();
                        if (reg.Tipogeneracion != null)
                            regMaxDia.Tipogeneracion = reg.Tipogeneracion.Trim();
                        else
                            reg.Tipogeneracion = "";
                        dia = reg.Medifecha.Day;
                        for (var i = diaAnt + 1; i < dia; i++)
                        {
                            regMaxDia.valores.Add(0M);
                        }
                        regMaxDia.valores.Add((decimal)reg.Valor);
                        regMaxDiaTotal.valores[dia - 1] += (decimal)reg.Valor;
                        regMaxDiaHoramin.horamin[dia - 1] = reg.Medifecha.AddMinutes(reg.HMax * 15).ToString("HH:mm");
                        diaAnt = dia;
                    }
                    else
                    {
                        dia = reg.Medifecha.Day;
                        for (var i = diaAnt + 1; i < dia; i++)
                            regMaxDia.valores.Add(0M);
                        regMaxDia.valores.Add((decimal)reg.Valor);
                        regMaxDiaTotal.valores[dia - 1] += (decimal)reg.Valor;
                        regMaxDiaHoramin.horamin[dia - 1] = reg.Medifecha.AddMinutes(reg.HMax * 15).ToString("HH:mm");
                        diaAnt = dia;
                    }
                    GrupoActual = reg.Gruponomb.Trim();
                }
                //
                for (var i = 0; i < totdias; i++)
                {
                    var fechaPiv = fechaInicio.AddDays(i);
                    var reg = lista.Find(x => x.DestinoPotencia == ConstantesMedicion.ImportacionEcuador && x.Medifecha == fechaPiv);
                    if (reg != null)
                        regMaxDiaImport.valores.Add((decimal)reg.Valor * -4);
                    else
                        regMaxDiaImport.valores.Add(0M);
                    var regEx = lista.Find(x => x.DestinoPotencia == ConstantesMedicion.ExportacionEcuador && x.Medifecha == fechaPiv);
                    if (regEx != null)
                        regMaxDiaExport.valores.Add((decimal)regEx.Valor * 4);
                    else
                        regMaxDiaExport.valores.Add(0M);

                    regMaxDiaTotal.valores[i] += regMaxDiaImport.valores[i] - regMaxDiaExport.valores[i];
                }
                listaMaximaDemandaDiaTotalResumen.Add(regMaxDiaExport);
                listaMaximaDemandaDiaTotalResumen.Add(regMaxDiaImport);

                if (listaGenPeru.Count > 0)
                {
                    listaMaximaDemandaDia.Add(regMaxDia);
                    listaMaximaDemandaDiaTotalResumen.Add(regMaxDiaHoramin);
                    listaMaximaDemandaDiaTotalResumen.Add(regMaxDiaTotal);
                }

                #endregion

                var vm = new MaximaDemanda
                {
                    ListaDemandaDia = listaMaximaDemandaDia,
                    ListaDemandaDiaTotalResumen = listaMaximaDemandaDiaTotalResumen,
                    ndiasXMes = totdias
                };

                MedidorHelper.GenerarReporteMaximaDemandaDiaria(vm, fechaInicio.ToString(Constantes.FormatoFecha),
                    fechaFin.ToString(Constantes.FormatoFecha));

                return Json(1.ToString());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = NombreArchivo.ReporteMaximaDemandaDiaria;
            string app = Constantes.AppExcel;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(fullPath, app, file);
        }

        #endregion

        #region Máxima Demanda Diaria HFP HP

        /// <summary>
        /// Pagina de inicio reporte de hora punta y fuera de punta
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_HFP_HP()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            BusquedaMaximaDemandaModel model = new BusquedaMaximaDemandaModel();
            model.FechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaTipoGeneracion = (new ConsultaMedidoresAppServicio()).ListaTipoGeneracion();
            model.ListaTipoEmpresas = (new ConsultaMedidoresAppServicio()).ListaTipoEmpresas();
            model.ParametroDefecto = 0;
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();
            return View(model);
        }

        /// <summary>
        /// Muestra el reporte hfp y hf
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MaximaDemandaDiaria_HFP_HP(string fecha, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            MaximaDemanda model = new MaximaDemanda();

            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, fecha, string.Empty, string.Empty, string.Empty);
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesMedicion.IdEmpresaTodos.ToString();
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = ConstantesMedicion.IdTipoGeneracionTodos.ToString();

            ///
            bool esPortal = false; //User.Identity.Name.Length == 0;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

            DemandadiaDTO resultado = new DemandadiaDTO();
            List<DemandadiaDTO> listOrdenado = new List<DemandadiaDTO>();

            List<DemandadiaDTO> lista = (new RankingConsolidadoAppServicio()).ObtenerDemandaDiariaHFPHP(fechaIni, fechaFin, tiposEmpresa, empresas,
                tiposGeneracion, central, out resultado, out listOrdenado, estadoValidacion, fechaProceso).OrderBy(x => x.Medifecha).ToList();

            model.ListaDemandaDia_HFP_HP = lista;

            List<decimal> listHFP = lista.Select(x => x.ValorHFP).ToList();
            List<decimal> listHP = lista.Select(x => x.ValorHP).ToList();

            if (listHFP.Count > 0 && listHP.Count > 0)
            {
                var indexHFP = listHFP.IndexOf(listHFP.Max());
                var indexHP = listHP.IndexOf(listHP.Max());
                model.IndexHFP = indexHFP;
                model.IndexHP = indexHP;
            }

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
        public JsonResult ExportarHP(string fecha, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            try
            {
                #region Calculo

                MaximaDemanda model = new MaximaDemanda();

                DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, fecha, string.Empty, string.Empty, string.Empty);
                DateTime fechaIni = fechaProceso;
                DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
                if (string.IsNullOrEmpty(empresas)) empresas = ConstantesMedicion.IdEmpresaTodos.ToString();
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = ConstantesMedicion.IdTipoGeneracionTodos.ToString();

                ///
                bool esPortal = false; //User.Identity.Name.Length == 0;
                int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

                DemandadiaDTO resultado = new DemandadiaDTO();
                List<DemandadiaDTO> listOrdenado = new List<DemandadiaDTO>();

                List<DemandadiaDTO> lista = (new RankingConsolidadoAppServicio()).ObtenerDemandaDiariaHFPHP(fechaIni, fechaFin, tiposEmpresa, empresas,
                    tiposGeneracion, central, out resultado, out listOrdenado, estadoValidacion, fechaProceso).OrderBy(x => x.Medifecha).ToList();

                model.ListaDemandaDia_HFP_HP = lista;
                List<decimal> listHFP = lista.Select(x => x.ValorHFP).ToList();
                List<decimal> listHP = lista.Select(x => x.ValorHP).ToList();

                if (listHFP.Count > 0 && listHP.Count > 0)
                {
                    var indexHFP = listHFP.IndexOf(listHFP.Max());
                    var indexHP = listHP.IndexOf(listHP.Max());
                    model.IndexHFP = indexHFP;
                    model.IndexHP = indexHP;
                }

                #endregion

                MedidorHelper.GenerarReporteMaximaDemandaHFPHP(model, fechaIni.ToString(Constantes.FormatoFecha),
                    fechaFin.ToString(Constantes.FormatoFecha));

                return Json(1.ToString());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarHP()
        {
            string file = NombreArchivo.ReporteMaxinaDemandaHFPHP;
            string app = Constantes.AppExcel;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(fullPath, app, file);
        }

        #endregion Reporte de Máxima Demanda

        #region	Reporte de Máxima Demanda
        //
        // GET: /Medidores/Reportes/MaximaDemanda
        public ActionResult MaximaDemanda()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaEmpresas = ListarEmpresaByTipoGeneracion(-1);
            model.ListaIntegrante = this._lsIntegrante;
            var listaTipoGeneracion = this.servMedidores.ListSiTipogeneracions();
            listaTipoGeneracion = listaTipoGeneracion.Where(x => x.Tgenercodi > 0).ToList().OrderBy(x => x.Tgenernomb).ToList();
            model.ListaTipoGeneracion = listaTipoGeneracion;
            model.IdParametro = ConstantesParametro.IdParametroHPPotenciaActiva;
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();
            model.EsPortal = !base.IsValidSesion;

            return View(model);
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

            model.EsPortal = !base.IsValidSesion;
            model.FechaConsulta = DateTime.Now.ToString(ConstantesBase.FormatoFechaExtendido);

            ///
            bool esPortal = false; //User.Identity.Name.Length == 0;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

            DateTime diaMaximaDemanda = this.servReporte.GetDiaPeriodoDemanda96XFiltro(fechaIni, fechaFin, ConstantesRepMaxDemanda.TipoMDNormativa, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);
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
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            DiagramaCargaMaximaDemandaModel model = new DiagramaCargaMaximaDemandaModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
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

            var serialize = new JavaScriptSerializer();
            model.Json = serialize.Serialize(Json(model).Data);// SerializeObject(Json(model).Data);

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
            ///
            bool esPortal = false;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;
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
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteRecursoEnergeticoModel model = new ReporteRecursoEnergeticoModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
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
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, string.Empty, string.Empty, string.Empty);
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

            ///
            bool esPortal = false;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;
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
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteDemandaPeriodoModel model = new ReporteDemandaPeriodoModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaEmpresas = ListarEmpresaByTipoGeneracion(-1);
            model.ListaIntegrante = this._lsIntegrante;
            model.ListaBloqueHorario = this._listaBloqueHorario;
            var listaTipoGeneracion = this.servMedidores.ListSiTipogeneracions();
            listaTipoGeneracion = listaTipoGeneracion.Where(x => x.Tgenercodi > 0).ToList().OrderBy(x => x.Tgenernomb).ToList();
            model.ListaTipoGeneracion = listaTipoGeneracion;
            var serialize = new JavaScriptSerializer();

            model.JsonBloqueHorario = serialize.Serialize(Json(model.ListaBloqueHorario).Data); // JsonConvert.SerializeObject(Json(model.ListaBloqueHorario).Data);
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();
            model.EsPortal = !base.IsValidSesion;

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
            bool esPortal = false; //User.Identity.Name.Length == 0;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

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

        #region Exceso de Potencia Reactiva
        //
        // GET: /Medidores/Reportes/ExcesoPotenciaReactiva
        public ActionResult ExcesoPotenciaReactiva()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteExcesoPotenciaReacModel model = new ReporteExcesoPotenciaReacModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaEmpresas = ListarEmpresaByTipoGeneracion(-1);
            model.ListaIntegrante = this._lsIntegrante;
            var listaTipoGeneracion = this.servMedidores.ListSiTipogeneracions();
            listaTipoGeneracion = listaTipoGeneracion.Where(x => x.Tgenercodi > 0).ToList().OrderBy(x => x.Tgenernomb).ToList();
            model.ListaTipoGeneracion = listaTipoGeneracion;
            model.IdParametro = ConstantesParametro.IdParametroRangoPotenciaInductiva;

            return View(model);
        }

        /// <summary>
        /// View ReporteExcesoPotenReactiva
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ReporteExcesoPotenReactiva(int tipoCentral, int tipoGeneracion, string idEmpresa, string mes)
        {
            ReporteExcesoPotenciaReacModel model = new ReporteExcesoPotenciaReacModel();

            this.GenerarExcesoPotenReactivaModel(model, tipoCentral, tipoGeneracion, idEmpresa, mes);

            return PartialView(model);
        }

        /// <summary>
        /// View ReporteExcesoPotenReactivaDet
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ReporteExcesoPotenReactivaDet(int tipoCentral, int tipoGeneracion, string idEmpresa, string mes)
        {
            ReporteExcesoPotenciaReacModel model = new ReporteExcesoPotenciaReacModel();

            this.GenerarExcesoPotenReactivaModel(model, tipoCentral, tipoGeneracion, idEmpresa, mes);


            model.ResultadoHtml = GenerarReporteDetalle(model.DetActiva, model.DetCapacitiva, model.DetInductiva, mes);

            return PartialView(model);
        }

        /// <summary>
        /// Generar Model de ExcesoPotenReactiva para web y excel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        private void GenerarExcesoPotenReactivaModel(ReporteExcesoPotenciaReacModel model, int tipoCentral, int tipoGeneracion, string idEmpresa, string mes)
        {
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, string.Empty, string.Empty, string.Empty);
            DateTime fechaInicial = fechaProceso.Date;
            DateTime fechaFinal = fechaProceso.AddMonths(1).AddDays(-1).Date;

            List<MeMedicion96DTO> listaReporte = new List<MeMedicion96DTO>();
            List<ConsolidadoEnvioDTO> listarReporte = new List<ConsolidadoEnvioDTO>();
            List<MeMedicion96DTO> listaDataERC = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaDataERI = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaDataEA = new List<MeMedicion96DTO>();

            List<MeMedicion96DTO> listaDetERI = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaDetERC = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaDetEA = new List<MeMedicion96DTO>();

            List<SiParametroValorDTO> listaRangoInductiva = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPotenciaInductiva);
            SiParametroValorDTO paramHora = this.servParametro.GetParametroHPPotenciaInductiva(listaRangoInductiva, fechaProceso, ParametrosFormato.ResolucionCuartoHora);

            int acota1 = 0; //33;//8:00am
            int acota2 = 0;//48;//12:00am
            int acota3 = 0;//73;//6:00pm
            int acota4 = 0;// 92;//11:00pm

            if (paramHora != null)
            {
                acota1 = paramHora.HRango1Ini + 1; //33;//8:00am
                acota2 = paramHora.HRango1Fin;//48;//12:00am
                acota3 = paramHora.HRango2Ini + 1;//73;//6:00pm
                acota4 = paramHora.HRango2Fin;// 92;//11:00pm
            }

            string fechaIni = (fechaInicial).ToString("dd/MM/yyyy");
            string fechaFin = (fechaFinal).ToString("dd/MM/yyyy");

            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            listaDataEA = this.servReporte.GetConsolidadoExcesoPotenReact(tipoCentral, tipoGeneracion, idEmpresa, fechaIni, fechaFin, ConstantesMedicion.IdFamiliaSSAA, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva, ConstantesMedidores.TptoMedicodiMedElectrica);
            listaDataERI = this.servReporte.GetConsolidadoExcesoPotenReact(tipoCentral, tipoGeneracion, idEmpresa, fechaIni, fechaFin, ConstantesMedicion.IdFamiliaSSAA, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva, ConstantesMedidores.TptoMedicodiInductiva);
            listaDataERC = this.servReporte.GetConsolidadoExcesoPotenReact(tipoCentral, tipoGeneracion, idEmpresa, fechaIni, fechaFin, ConstantesMedicion.IdFamiliaSSAA, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva, ConstantesMedidores.TptoMedicodiCapacitiva);

            for (DateTime f = fechaInicial.Date; f <= fechaFinal.Date; f = f.AddDays(1))
            {
                #region Energia
                List<MeMedicion96DTO> listaEA = listaDataEA.Where(x => x.Hophorini.Date == f).OrderBy(x => x.Hophorini).ToList();
                List<MeMedicion96DTO> listaERC = listaDataERC.Where(x => x.Hophorini.Date == f).ToList();
                List<MeMedicion96DTO> listaERI = listaDataERI.Where(x => x.Hophorini.Date == f).ToList();

                if (listaEA != null && listaEA.Count > 0)
                {
                    List<int> Listgrupos = listaEA.Select(x => x.Grupocodi).Distinct().ToList();

                    for (int x = 0; x < Listgrupos.Count; x++)
                    {
                        MeMedicion96DTO tmpER = new MeMedicion96DTO();
                        MeMedicion96DTO tmpERC = new MeMedicion96DTO();
                        MeMedicion96DTO tmpERI = new MeMedicion96DTO();
                        var ListptoMedT = listaEA.Where(y => y.Grupocodi == Listgrupos[x]).Select(y => new { y.Ptomedicodi, y.Hopcodi }).Distinct().ToList();
                        List<int> listaHopcodi = ListptoMedT.Select(m => m.Hopcodi).ToList();

                        for (int y = 0; y < ListptoMedT.Count; y++)
                        {
                            //energia activa
                            var regEAHop = listaEA.Where(e => !(e.Subcausacodi == 106 && e.Tgenercodi == 2)
                                && (e.Grupocodi == Listgrupos[x] && e.Ptomedicodi == ListptoMedT[y].Ptomedicodi && e.Hopcodi == ListptoMedT[y].Hopcodi)).FirstOrDefault();

                            var listaHOPTmp = listaEA.Where(e => e.Grupocodi == Listgrupos[x]).ToList();
                            var listaPos = this.ObtenerPosicionHora(listaHOPTmp, listaHopcodi, regEAHop);
                            int inicio = listaPos[0];
                            int final = listaPos[1];

                            if (regEAHop != null)
                            {
                                tmpER.Hophorini = regEAHop.Hophorini;
                                tmpER.Gruponomb = regEAHop.Gruponomb.Trim();
                                tmpER.Emprcodi = regEAHop.Emprcodi;
                                tmpER.Emprnomb = regEAHop.Emprnomb.Trim();
                                tmpER.Grupocodi = regEAHop.Grupocodi;
                                tmpER.Ptomedicodi = regEAHop.Ptomedicodi;

                                tmpERC.Hophorini = regEAHop.Hophorini;
                                tmpERC.Gruponomb = regEAHop.Gruponomb.Trim();
                                tmpERC.Emprcodi = regEAHop.Emprcodi;
                                tmpERC.Emprnomb = regEAHop.Emprnomb.Trim();
                                tmpERC.Grupocodi = regEAHop.Grupocodi;
                                tmpERC.Ptomedicodi = regEAHop.Ptomedicodi;

                                tmpERI.Hophorini = regEAHop.Hophorini;
                                tmpERI.Gruponomb = regEAHop.Gruponomb.Trim();
                                tmpERI.Emprcodi = regEAHop.Emprcodi;
                                tmpERI.Emprnomb = regEAHop.Emprnomb.Trim();
                                tmpERI.Grupocodi = regEAHop.Grupocodi;
                                tmpERI.Ptomedicodi = regEAHop.Ptomedicodi;

                                for (int i = inicio; i <= final; i++)
                                {
                                    decimal val = (((decimal?)regEAHop.GetType().GetProperty("H" + i).GetValue(regEAHop, null)).GetValueOrDefault(0));
                                    decimal sumaR = (((decimal?)tmpER.GetType().GetProperty("H" + i).GetValue(tmpER, null)).GetValueOrDefault(0)) + val;
                                    tmpER.GetType().GetProperty("H" + i).SetValue(tmpER, sumaR);
                                }
                            }

                            //energia reactiva capacitiva
                            var regERCHop = listaERC.Where(e => !(e.Subcausacodi == 106 && e.Tgenercodi == 2)
                                && (e.Grupocodi == Listgrupos[x] && e.Ptomedicodi == ListptoMedT[y].Ptomedicodi && e.Hopcodi == ListptoMedT[y].Hopcodi)).FirstOrDefault();
                            if (regEAHop != null && regERCHop != null)
                            {

                                for (int i = inicio; i <= final; i++)
                                {
                                    decimal val = (((decimal?)regERCHop.GetType().GetProperty("H" + i).GetValue(regERCHop, null)).GetValueOrDefault(0));
                                    decimal sumaR = (((decimal?)tmpERC.GetType().GetProperty("H" + i).GetValue(tmpERC, null)).GetValueOrDefault(0)) + val;
                                    tmpERC.GetType().GetProperty("H" + i).SetValue(tmpERC, sumaR);
                                }
                            }

                            //energia reactiva inductiva
                            var regERIHop = listaERI.Where(e => !(e.Subcausacodi == 106 && e.Tgenercodi == 2)
                                && (e.Grupocodi == Listgrupos[x] && e.Ptomedicodi == ListptoMedT[y].Ptomedicodi && e.Hopcodi == ListptoMedT[y].Hopcodi)).FirstOrDefault();

                            if (regEAHop != null && regERIHop != null)
                            {

                                for (int i = inicio; i <= final; i++)
                                {
                                    decimal val = (((decimal?)regERIHop.GetType().GetProperty("H" + i).GetValue(regERIHop, null)).GetValueOrDefault(0));
                                    decimal sumaR = (((decimal?)tmpERI.GetType().GetProperty("H" + i).GetValue(tmpERI, null)).GetValueOrDefault(0)) + val;
                                    tmpERI.GetType().GetProperty("H" + i).SetValue(tmpERI, sumaR);
                                }
                            }
                        }

                        listaDetEA.Add(tmpER);
                        listaDetERC.Add(tmpERC);
                        listaDetERI.Add(tmpERI);
                    }
                }
                #endregion
            }

            #region calculo
            for (int k = 0; k < listaDetEA.Count; k++)
            {
                MeMedicion96DTO ea = listaDetEA[k];

                for (int i = 1; i <= 96; i++)
                {
                    decimal val = (((decimal?)ea.GetType().GetProperty("H" + i).GetValue(ea, null)).GetValueOrDefault(0));
                    ea.GetType().GetProperty("H" + i).SetValue(ea, val / 4);
                }
            }
            for (int k = 0; k < listaDetERI.Count; k++)
            {
                MeMedicion96DTO eri = listaDetERI[k];

                for (int i = 1; i <= 96; i++)
                {
                    decimal val = 0;
                    val = (((decimal?)eri.GetType().GetProperty("H" + i).GetValue(eri, null)).GetValueOrDefault(0));
                    eri.GetType().GetProperty("H" + i).SetValue(eri, val / 4);
                }
            }
            for (int k = 0; k < listaDetERC.Count; k++)
            {
                MeMedicion96DTO erc = listaDetERC[k];

                for (int i = 1; i <= 96; i++)
                {
                    decimal val = 0;
                    val = (((decimal?)erc.GetType().GetProperty("H" + i).GetValue(erc, null)).GetValueOrDefault(0));
                    erc.GetType().GetProperty("H" + i).SetValue(erc, val / 4);
                }
            }

            //calculo de exceso
            double tangind = Math.Tan(Math.Acos(0.95));
            double tangcap = Math.Tan(Math.Acos(0.99));
            if (listaDetEA.Count > 0)
            {
                List<int> listaGrupos = listaDetEA.Select(x => x.Grupocodi).Distinct().ToList();
                foreach (var grupocodi in listaGrupos)
                {
                    MeMedicion96DTO regFirstEA = listaDetEA.Where(x => x.Grupocodi == grupocodi).First();

                    ConsolidadoEnvioDTO temporal = new ConsolidadoEnvioDTO();
                    temporal.Empresa = regFirstEA.Emprnomb;
                    temporal.Emprcodi = regFirstEA.Emprcodi;
                    temporal.GrupSSAA = regFirstEA.Gruponomb;
                    temporal.IdGrupo = regFirstEA.Grupocodi;
                    temporal.TotalInductiva = 0;
                    temporal.TotalCapacitiva = 0;

                    for (DateTime f = fechaInicial.Date; f <= fechaFinal.Date; f = f.AddDays(1))
                    {
                        MeMedicion96DTO regEA = listaDetEA.Where(y => y.Hophorini.Date == f && y.Grupocodi == grupocodi).FirstOrDefault();
                        MeMedicion96DTO regERI = listaDetERI.Where(y => y.Hophorini.Date == f && y.Grupocodi == grupocodi).FirstOrDefault();
                        MeMedicion96DTO regERC = listaDetERC.Where(z => z.Hophorini.Date == f && z.Grupocodi == grupocodi).FirstOrDefault();

                        for (int y = 1; y <= 96; y++)
                        {
                            double energia1 = 0;
                            double energia2 = 0;

                            decimal ea = regEA != null ? ((decimal?)regEA.GetType().GetProperty("H" + y).GetValue(regEA, null)).GetValueOrDefault(0) : 0;
                            decimal eri = regERI != null ? ((decimal?)regERI.GetType().GetProperty("H" + y).GetValue(regERI, null)).GetValueOrDefault(0) : 0;
                            decimal erc = regERC != null ? ((decimal?)regERC.GetType().GetProperty("H" + y).GetValue(regERC, null)).GetValueOrDefault(0) : 0;

                            //reactiva
                            if ((y >= acota1 && y <= acota2) || (y >= acota3 && y <= acota4))
                            {
                                energia1 = Convert.ToDouble(eri) - (Convert.ToDouble(ea) * tangind);
                            }

                            //capacitiva
                            if (erc > 0)
                            {
                                energia2 = Convert.ToDouble(erc) - (Convert.ToDouble(ea) * tangcap);
                            }

                            if (energia1 > 0)
                            {
                                temporal.TotalInductiva = temporal.TotalInductiva + Convert.ToDecimal(energia1);
                            }
                            if (energia2 > 0)
                            {
                                temporal.TotalCapacitiva = temporal.TotalCapacitiva + Convert.ToDecimal(energia2);
                            }
                        }
                    }

                    temporal.TotalInductiva = Math.Round(temporal.TotalInductiva, 2);
                    temporal.TotalCapacitiva = Math.Round(temporal.TotalCapacitiva, 2);

                    listarReporte.Add(temporal);
                }
            }

            listarReporte = listarReporte.OrderBy(x => x.Empresa).ThenBy(x => x.GrupSSAA).ToList();

            listaDetEA = listaDetEA.OrderBy(x => x.Emprnomb).ThenBy(x => x.Ptomedicodi).ToList();
            listaDetERC = listaDetERC.OrderBy(x => x.Emprnomb).ThenBy(x => x.Ptomedicodi).ToList();
            listaDetERI = listaDetERI.OrderBy(x => x.Emprnomb).ThenBy(x => x.Ptomedicodi).ToList();
            #endregion

            model.DetActiva = listaDetEA;
            model.DetCapacitiva = listaDetERC;
            model.DetInductiva = listaDetERI;
            model.ListaConsolidadoDemanda = listarReporte;
            model.NombreHoja = "Exceso de Potencia Reactiva";
            model.NombreHojaDet = "Potencia Reactiva";
            model.Titulo = "REPORTE DE EXCESO DE POTENCIA REACTIVA";
            model.TituloDet = "POTENCIA REACTIVA";
        }

        private int[] ObtenerPosicionHora(List<MeMedicion96DTO> listaEA, List<int> listaHopcodi, MeMedicion96DTO reg)
        {
            int[] lista = new int[2];
            if (reg == null)
            {
                lista[0] = -1;
                lista[1] = -1;
                return lista;
            }

            int posIni = this.ObtenerPosicionHoraInicial(reg)[0];
            int posFin = this.ObtenerPosicionHoraFinal(reg)[0];

            //recorrer las horas de op de ese dia
            List<MeMedicion96DTO> listaMin = new List<MeMedicion96DTO>();
            foreach (var hopcodi in listaHopcodi)
            {
                var regaux = listaEA.Find(x => x.Hopcodi == hopcodi);

                MeMedicion96DTO tmp = new MeMedicion96DTO();
                tmp.Grupocodi = regaux.Grupocodi;
                tmp.Hopcodi = hopcodi;

                //decimal difmin = (regaux.Hophorfin.Minute) - ((inicio % 4) - 1) * 15;
                int minini = this.ObtenerPosicionHoraInicial(regaux)[0];
                decimal difini = this.ObtenerPosicionHoraInicial(regaux)[1];
                tmp.GetType().GetProperty("H" + minini).SetValue(tmp, difini);

                int minfin = this.ObtenerPosicionHoraFinal(regaux)[0];
                decimal diffin = this.ObtenerPosicionHoraFinal(regaux)[1];
                tmp.GetType().GetProperty("H" + minfin).SetValue(tmp, diffin);

                listaMin.Add(tmp);
            }

            //determinar el inicio
            MeMedicion96DTO regAnt = null;
            bool fin = false;
            for (int m = 0; m < listaMin.Count && !fin; m++)
            {
                var regAct = listaMin[m];
                if (regAnt != null && regAct.Hopcodi == reg.Hopcodi)
                {
                    var difAnt = ((decimal?)regAnt.GetType().GetProperty("H" + posIni).GetValue(regAnt, null)).GetValueOrDefault(0);
                    var difAct = this.ObtenerPosicionHoraInicial(reg)[1];

                    if (difAnt > difAct)
                    {
                        posIni = posIni + 1;
                    }
                    fin = true;
                }

                regAnt = regAct;
            }

            //
            lista[0] = posIni;
            lista[1] = posFin;
            return lista;
        }

        private int[] ObtenerPosicionHoraInicial(MeMedicion96DTO reg)
        {
            int[] lista = new int[2];
            //posicion inicial
            int hh1 = reg.Hophorini.Hour;
            int mi1 = reg.Hophorini.Minute;
            int pp1 = 0, tiempo1 = 0;

            if ((0 <= mi1) && (mi1 <= 15))
            {
                pp1 = 1;
                tiempo1 = 15 - mi1;
            }
            if ((15 < mi1) && (mi1 <= 30))
            {
                pp1 = 2;
                tiempo1 = 30 - mi1;
            }
            if ((30 < mi1) && (mi1 <= 45))
            {
                pp1 = 3;
                tiempo1 = 45 - mi1;
            }
            if ((45 < mi1) && (mi1 < 60))
            {
                pp1 = 4;
                tiempo1 = 60 - mi1;
            }
            int pos1 = hh1 * 4 + pp1;

            //
            lista[0] = pos1;
            lista[1] = tiempo1;
            return lista;
        }

        private int[] ObtenerPosicionHoraFinal(MeMedicion96DTO reg)
        {
            int[] lista = new int[2];
            //posicion final
            int hh2 = reg.Hophorfin.Hour;
            int mi2 = reg.Hophorfin.Minute;
            int pp2 = 0, tiempo2 = 0;

            if ((0 <= mi2) && (mi2 <= 15))
            {
                pp2 = 1;
                tiempo2 = mi2;
            }
            if ((15 < mi2) && (mi2 <= 30))
            {
                pp2 = 2;
                tiempo2 = mi2 - 15;
            }
            if ((30 < mi2) && (mi2 <= 45))
            {
                pp2 = 3;
                tiempo2 = mi2 - 30;
            }
            if ((45 < mi2) && (mi2 < 60))
            {
                pp2 = 4;
                tiempo2 = mi2 - 45;
            }
            int pos2 = hh2 * 4 + pp2;
            if (((reg.Hophorfin.Day - reg.Hophorini.Day) > 0) || ((reg.Hophorfin.Month - reg.Hophorini.Month) > 0))
            {
                pos2 = hh2 * 4 + 96;
            }

            //
            lista[0] = pos2;
            lista[1] = tiempo2;
            return lista;
        }

        /// <summary>
        /// Generar archivo excel ExcesoPotenReactiva
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcesoPotenReactiva(int tipoCentral, int tipoGeneracion, string idEmpresa, string mes)
        {
            string[] datos = new string[2];
            try
            {
                ReporteExcesoPotenciaReacModel model = new ReporteExcesoPotenciaReacModel();
                GenerarExcesoPotenReactivaModel(model, tipoCentral, tipoGeneracion, idEmpresa, mes);

                if (model.ListaConsolidadoDemanda.Count > 0)
                {
                    string ruta = this.servReporte.GenerarFileExcelReporteExcesoPotenReact(model.NombreHoja, model.NombreHojaDet, model.Titulo, model.TituloDet, mes, model.ListaConsolidadoDemanda, model.DetActiva, model.DetCapacitiva, model.DetInductiva);
                    string nombreArchivo = string.Format("{0}_{1:HHmmss}.xlsx", "ExcesoPotenciaReactiva", DateTime.Now);


                    datos[0] = ruta;
                    datos[1] = nombreArchivo + ".xlsx";
                }
                else
                {
                    datos[0] = "2";
                    datos[1] = "";
                }

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


        public string GenerarReporteDetalle(List<MeMedicion96DTO> listaDataEA, List<MeMedicion96DTO> listaDataERC, List<MeMedicion96DTO> listaDataERI, string mes)
        {
            StringBuilder strHtml = new StringBuilder();
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            DateTime fechaInicial = fechaProceso.Date;
            DateTime fechaFinal = fechaProceso.AddMonths(1).AddDays(-1).Date;

            if (listaDataEA.Count > 0)
            {
                strHtml.Append("<table class='pretty tabla-icono'>");
                strHtml.Append("<thead>");
                #region cabecera
                //empresas
                strHtml.Append("<tr>");
                strHtml.Append("<th colspan='1' rowspan='1'>EMPRESA</th>");
                var listaEmpresa = listaDataEA.Select(y => new { y.Emprcodi, y.Emprnomb }).Distinct().ToList().OrderBy(c => c.Emprnomb).ToList();
                for (int j = 0; j < listaEmpresa.Count; j++)
                {
                    var emprcodi = listaEmpresa[j].Emprcodi;
                    var empresa = listaEmpresa[j].Emprnomb;

                    var listaModo = listaDataEA.Where(x => x.Emprcodi == emprcodi).ToList().Select(y => new { y.Grupocodi, y.Gruponomb }).Distinct().ToList().OrderBy(c => c.Gruponomb).ToList();

                    strHtml.Append(string.Format("<th colspan='{1}'>{0}</th>", empresa, listaModo.Count * 3));
                }
                strHtml.Append("</tr>");

                //modos de operacion
                strHtml.Append("<tr>");
                strHtml.Append("<th colspan='1' rowspan='1'>MODO OPERACIÓN</th>");
                for (int j = 0; j < listaEmpresa.Count; j++)
                {
                    var emprcodi = listaEmpresa[j].Emprcodi;
                    var listaModo = listaDataEA.Where(x => x.Emprcodi == emprcodi).ToList().Select(y => new { y.Grupocodi, y.Gruponomb }).Distinct().ToList().OrderBy(c => c.Gruponomb).ToList();
                    for (int g = 0; g < listaModo.Count; g++)
                    {
                        var modo = listaModo[g].Gruponomb;
                        strHtml.Append(string.Format("<th colspan='{1}'>{0}</th>", modo, 3));
                    }

                }
                strHtml.Append("</tr>");

                strHtml.Append("<tr>");
                strHtml.Append("<th colspan='1' rowspan='1'>FECHA/HORA</th>");
                for (int j = 0; j < listaEmpresa.Count; j++)
                {
                    var emprcodi = listaEmpresa[j].Emprcodi;
                    var listaModo = listaDataEA.Where(x => x.Emprcodi == emprcodi).ToList().Select(y => new { y.Grupocodi, y.Gruponomb }).Distinct().ToList().OrderBy(c => c.Gruponomb).ToList();
                    for (int g = 0; g < listaModo.Count; g++)
                    {
                        strHtml.Append(string.Format("<th>{0}</th>", "EA"));
                        strHtml.Append(string.Format("<th>{0}</th>", "ER. IND"));
                        strHtml.Append(string.Format("<th>{0}</th>", "ER. CAP"));
                    }
                }
                strHtml.Append("</tr>");
                strHtml.Append("</thead>");
                #endregion

                strHtml.Append("<tbody>");
                #region cuerpo
                //data96
                for (DateTime f = fechaInicial.Date; f <= fechaFinal.Date; f = f.AddDays(1))
                {
                    for (int h = 1; h <= 96; h++)
                    {
                        strHtml.Append("<tr>");
                        var hora = f.AddMinutes(15 * h).ToString(ConstantesBase.FormatFechaFull);
                        strHtml.Append(string.Format("<td>{0}</th>", hora));

                        for (int j = 0; j < listaEmpresa.Count; j++)
                        {
                            var emprcodi = listaEmpresa[j].Emprcodi;
                            var listaModo = listaDataEA.Where(x => x.Emprcodi == emprcodi).ToList().Select(y => new { y.Grupocodi, y.Gruponomb }).Distinct().ToList().OrderBy(c => c.Gruponomb).ToList();
                            for (int g = 0; g < listaModo.Count; g++)
                            {
                                var ea = listaDataEA.Where(y => y.Hophorini.Date == f && y.Grupocodi == listaModo[g].Grupocodi).FirstOrDefault();
                                var eri = listaDataERI.Where(y => y.Hophorini.Date == f && y.Grupocodi == listaModo[g].Grupocodi).FirstOrDefault();
                                var erc = listaDataERC.Where(z => z.Hophorini.Date == f && z.Grupocodi == listaModo[g].Grupocodi).FirstOrDefault();
                                decimal? valorea = null, valorerc = null, valoreri = null;
                                if (ea != null)
                                {
                                    valorea = (decimal?)ea.GetType().GetProperty("H" + h).GetValue(ea, null);
                                }
                                if (eri != null)
                                {
                                    valoreri = (decimal?)eri.GetType().GetProperty("H" + h).GetValue(eri, null);
                                }
                                if (erc != null)
                                {
                                    valorerc = (decimal?)erc.GetType().GetProperty("H" + h).GetValue(erc, null);
                                }

                                strHtml.Append(string.Format("<td>{0}</th>", valorea != null ? (Math.Round(valorea.Value, 5)).ToString() : string.Empty));
                                strHtml.Append(string.Format("<td>{0}</th>", valoreri != null ? (Math.Round(valoreri.Value, 5)).ToString() : string.Empty));
                                strHtml.Append(string.Format("<td>{0}</th>", valorerc != null ? (Math.Round(valorerc.Value, 5)).ToString() : string.Empty));
                            }
                        }
                        strHtml.Append("</tr>");
                    }
                }
                #endregion
                strHtml.Append("</tbody>");
                strHtml.Append("</table>");
            }

            return strHtml.ToString();
        }

        #endregion

        #region Gráfico en carga de datos
        //
        // GET: /Medidores/Reportes/GraficoCargaDatos
        public ActionResult GraficoCargaDatos()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            MedidorGeneracionModel model = new MedidorGeneracionModel();
            //base.IndexFormato(model, ConstantesMedidores.IdFormatoMedidorGeneracion);
            this.GenerarGraficoCargaDatosModel(model, ConstantesMedidores.IdFormatoCargaCentralPotActiva);
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            List<MeHojaDTO> listaHoja = this.servFormato.GetByCriteriaMeHoja(ConstantesMedidores.IdFormatoMedidorGeneracion);
            model.TituloCargaCentralPotActiva = listaHoja.Count() > 0 ? listaHoja.Where(x => x.Hojacodi == ConstantesMedidores.IdHojaCargaCentralPotActiva).FirstOrDefault().Hojanombre : "";
            model.IdFormatoCargaCentralPotActiva = ConstantesMedidores.IdFormatoCargaCentralPotActiva;

            //grafico
            model.ListaFuente1 = LsFuente1;
            model.FechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString(Constantes.FormatoFecha);
            model.FechaHasta = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).ToString(Constantes.FormatoFecha);

            var listaTipoGeneracion = this.servMedidores.ListSiTipogeneracions();
            listaTipoGeneracion = listaTipoGeneracion.Where(x => x.Tgenercodi > 0).ToList().OrderBy(x => x.Tgenernomb).ToList();
            model.ListaTipoGeneracion = listaTipoGeneracion;

            return View(model);
        }

        /// <summary>
        /// Generar Model de GraficoCargaDatos para web y excel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idFormato"></param>
        private void GenerarGraficoCargaDatosModel(MedidorGeneracionModel model, int idFormato)
        {
            //lista empresa
            bool accesoEmpresa = true;
            var empresas = servFormato.GetListaEmpresaFormato(idFormato);
            if (accesoEmpresa)
            {
                if (empresas.Count > 0)
                    model.ListaEmpresas = empresas;
                else
                {
                    model.ListaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }
            else
            {
                var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                    Select(x => new SiEmpresaDTO()
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB
                    });
                if (emprUsuario.Count() > 0)
                {
                    model.ListaEmpresas = emprUsuario.ToList();

                }
                else
                {
                    model.ListaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }
        }

        /// <summary>
        /// Listar Central por Empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCentralByEmpresa(int tipoGeneracion, int idEmpresa)
        {
            try
            {
                MedidorGeneracionModel model = new MedidorGeneracionModel();

                var formato = this.servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva);
                var cabecera = this.servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                var listaHojaPto = this.servFormato.GetListaPtos(DateTime.Now.Date, 0, idEmpresa, formato.Formatcodi, cabecera.Cabquery);

                var listaInf = listaHojaPto.GroupBy(x => new { x.Tipoinfocodi, x.Tipoinfoabrev }).Select(
                        grp => new SiTipoinformacionDTO { Tipoinfocodi = grp.Key.Tipoinfocodi, Tipoinfoabrev = grp.Key.Tipoinfoabrev }).ToList();
                model.ListaTipoInformacion = listaInf;

                var listaCentral = this.servFormato.ListCentralByEmpresaAndFormato(listaHojaPto);

                switch (tipoGeneracion)
                {
                    case ConstantesMedicion.IdTipoGeneracionTodos:
                        model.ListaEquipo = listaCentral;
                        break;
                    case ConstantesMedicion.IdTipoGeneracionHidrolectrica:
                        model.ListaEquipo = listaCentral.Where(x => x.Famcodi == ConstantesMedidores.CentralHidraulica).ToList();
                        break;
                    case ConstantesMedicion.IdTipoGeneracionTermoelectrica:
                        model.ListaEquipo = listaCentral.Where(x => x.Famcodi == ConstantesMedidores.CentralTermica).ToList();
                        break;
                    case ConstantesMedicion.IdTipoGeneracionSolar:
                        model.ListaEquipo = listaCentral.Where(x => x.Famcodi == ConstantesMedidores.CentralSolar).ToList();
                        break;
                    case ConstantesMedicion.IdTipoGeneracionEolica:
                        model.ListaEquipo = listaCentral.Where(x => x.Famcodi == ConstantesMedidores.CentralEolica).ToList();
                        break;
                    case ConstantesMedicion.IdTipoGeneracionNuclear:
                        break;
                }

                return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return Json("-1");
        }

        /// <summary>
        /// Listar Fuente por Central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFuenteByCentral(int idEmpresa, int idCentral)
        {
            try
            {
                MedidorGeneracionModel model = new MedidorGeneracionModel();
                List<FuenteInformacion> listaFuente2 = new List<FuenteInformacion>();

                var formato = this.servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva);
                var cabecera = this.servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                var listaHojaPto = this.servFormato.GetListaPtos(DateTime.Now.Date, 0, idEmpresa, formato.Formatcodi, cabecera.Cabquery);

                var listaEq = this.servFormato.ListCentralByEmpresaAndFormato(listaHojaPto);

                var central = listaEq.Where(x => x.Equicodi == idCentral).First();

                switch (central.Famcodi)
                {
                    case ConstantesMedidores.CentralHidraulica:
                        listaFuente2.Add(F2);
                        listaFuente2.Add(F3);
                        listaFuente2.Add(F5);
                        break;
                    case ConstantesMedidores.CentralTermica:
                        listaFuente2.Add(F3);
                        listaFuente2.Add(F4);
                        listaFuente2.Add(F5);
                        break;
                    case ConstantesMedidores.CentralSolar:
                    case ConstantesMedidores.CentralEolica:
                        listaFuente2.Add(F3);
                        listaFuente2.Add(F5);
                        break;
                    default:

                        break;
                }

                model.ListaFuente2 = listaFuente2;

                return Json(model);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return Json("-1");
        }

        /// <summary>
        /// Gráficos en Carga de Datos para usuario COES
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="fuente1"></param>
        /// <param name="fuente2"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarGraficoCargaDatoUsuarioCOES(int idEmpresa, string fechaIni, string fechaFin, int periodo, int tipoDato, int idCentral, int fuente1, int fuente2)
        {
            int idFormato = ConstantesMedidores.IdFormatoCargaCentralPotActiva;
            GraficoMedidorGeneracion g = GenerarGrafico(idEmpresa, idFormato, fechaIni, fechaFin, periodo, tipoDato, idCentral, fuente1, fuente2, ConstantesMedidores.UsuarioCOES);
            return Json(g);
        }

        /// <summary>
        /// Generar archivo excel GraficoCargaDatoUsuarioCOES
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="fuente1"></param>
        /// <param name="fuente2"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcelGraficoCargaDatoUsuarioCOES(int idEmpresa, string fechaIni, string fechaFin, int periodo, int tipoDato, int idCentral, int fuente1, int fuente2)
        {
            string ruta = string.Empty;
            string[] datos = new string[2];
            try
            {
                int idFormato = ConstantesMedidores.IdFormatoCargaCentralPotActiva;
                GraficoMedidorGeneracion g = GenerarGrafico(idEmpresa, idFormato, fechaIni, fechaFin, periodo, tipoDato, idCentral, fuente1, fuente2, ConstantesMedidores.UsuarioCOES);

                switch (g.TipoGrafico)
                {
                    case ConstantesMedidores.GraficoIgualMedida:
                        ruta = GenerarExcelGraficoIgualMedida(g);
                        break;
                    case ConstantesMedidores.GraficoDiferenteMedida:
                        ruta = GenerarExcelGraficoDiferenteMedida(g);
                        break;
                }
                datos[0] = ruta;
                datos[1] = g.Nombre;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                datos[0] = "-1";
                datos[1] = "";
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }

        /// <summary>
        /// Permite descargar el formato de carga para usuario COES
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelGraficoCargaDato()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #region métodos

        /// <summary>
        /// Generar Grafico
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="fuente1"></param>
        /// <param name="fuente2"></param>
        /// <param name="resolucion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private GraficoMedidorGeneracion GenerarGrafico(int idEmpresa, int idFormato, string fechaIni, string fechaFin, int periodo, int tipoDato, int idCentral, int fuente1, int fuente2, int usuario)
        {
            List<PuntoGraficoMedidorGeneracion> listaGraf = new List<PuntoGraficoMedidorGeneracion>();
            //variables
            int tipoinfocodi = 1;
            int tipoGrafico = 0;
            bool hayDataAnterior = false;
            string nombreFuente1 = string.Empty, nombreFuente2 = string.Empty;
            string valorFuente1 = string.Empty, valorFuente2 = string.Empty;
            string tituloFuente1 = string.Empty, tituloFuente2 = string.Empty;
            string leyendaFuente1 = string.Empty, leyendaFuente2 = string.Empty;
            string descCentral = this.servFormato.GetByIdEqequipo(idCentral).Equinomb;
            DateTime dfechaIni = fechaIni != null && fechaIni != "" ? DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime dfechaFin = fechaFin != null && fechaFin != "" ? DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            //resolucion
            int resolucion = 0;
            switch (tipoDato)
            {
                case 2:
                    resolucion = ParametrosFormato.ResolucionMediaHora;
                    break;
                case 3:
                    resolucion = ParametrosFormato.ResolucionHora;
                    break;
                case 1:
                default:
                    resolucion = ParametrosFormato.ResolucionCuartoHora;
                    break;
            }
            //HP, desde 7:30pm a 11:15pm 
            int hIni = 0;
            int hFin = 0;
            int horaIniHP = 0;
            int horaFinHP = 0;
            switch (resolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    hIni = 1;
                    hFin = 96;
                    horaIniHP = 78;
                    horaFinHP = 93;
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    hIni = 1;
                    hFin = 48;
                    horaIniHP = 24;
                    horaFinHP = 46;
                    break;
                case ParametrosFormato.ResolucionHora:
                    hIni = 1;
                    hFin = 24;
                    horaIniHP = 20; //7:30pm se redondea a 8pm
                    horaFinHP = 23;
                    break;
            }

            //fuente2
            List<PuntoFuenteInformacion> listaFuente2 = new List<PuntoFuenteInformacion>();
            nombreFuente2 = LsFuente2.Where(x => x.Codigo == fuente2).First().Nombre;
            valorFuente2 = LsFuente2.Where(x => x.Codigo == fuente2).First().Valor;
            tituloFuente2 = LsFuente2.Where(x => x.Codigo == fuente2).First().Titulo;
            leyendaFuente2 = LsFuente2.Where(x => x.Codigo == fuente2).First().Leyenda;
            switch (fuente2)
            {
                case ConstantesMedidores.IdFuenteCaudalTurbinado:
                    hayDataAnterior = true;
                    tipoGrafico = ConstantesMedidores.GraficoDiferenteMedida;
                    listaFuente2 = GetListaFuenteCaudalTurbinado(idEmpresa, fechaIni, fechaFin, periodo, tipoDato, idCentral, resolucion);
                    break;

                case ConstantesMedidores.IdFuenteDespachoDiario:
                    tipoGrafico = ConstantesMedidores.GraficoIgualMedida;
                    listaFuente2 = GetListaFuenteDespachoDiario(idEmpresa, fechaIni, fechaFin, periodo, tipoDato, idCentral, resolucion, tipoinfocodi);
                    break;
                case ConstantesMedidores.IdFuenteDatosScada:
                    tipoGrafico = ConstantesMedidores.GraficoIgualMedida;
                    listaFuente2 = GetListaFuenteDatosScada(idEmpresa, fechaIni, fechaFin, periodo, tipoDato, idCentral, resolucion);
                    break;
                case ConstantesMedidores.IdFuenteRPF:
                    tipoGrafico = ConstantesMedidores.GraficoIgualMedida;
                    listaFuente2 = GetListaFuenteRPF(idEmpresa, fechaIni, fechaFin, periodo, tipoDato, idCentral, resolucion);
                    break;
            }

            //Fuente1
            List<PuntoFuenteInformacion> listaFuente1 = new List<PuntoFuenteInformacion>();
            nombreFuente1 = LsFuente1.Where(x => x.Codigo == fuente1).First().Nombre;
            valorFuente1 = LsFuente1.Where(x => x.Codigo == fuente1).First().Valor;
            tituloFuente1 = LsFuente1.Where(x => x.Codigo == fuente1).First().Titulo;
            leyendaFuente1 = LsFuente1.Where(x => x.Codigo == fuente1).First().Leyenda;
            switch (fuente1)
            {
                case ConstantesMedidores.IdFuenteMedidores:
                    listaFuente1 = GetListaFuenteMedidores(idEmpresa, idFormato, fechaIni, fechaFin, periodo, tipoDato, idCentral, resolucion, ref tipoinfocodi, hayDataAnterior);
                    break;
            }

            //Generar data
            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                var listaDataFuente1Dia = listaFuente1.Where(x => x.FechaFiltro.Date == day).ToList();
                var listaDataFuente2Dia = listaFuente2.Where(x => x.FechaFiltro.Date == day).ToList();

                for (int i = hIni; i <= hFin; i++)
                {
                    //fuente1
                    PuntoFuenteInformacion f1 = listaDataFuente1Dia.Where(x => x.NumeroTiempo == i).First();
                    decimal? valor1 = f1.ValorFuente;

                    //fuente2
                    PuntoFuenteInformacion f2 = listaDataFuente2Dia.Where(x => x.NumeroTiempo == i).First();
                    decimal? valor2 = f2.ValorFuente;

                    //fecha
                    DateTime fechaPunto = day;
                    int horas = 0;
                    int minutos = 0;

                    if (!hayDataAnterior)
                    {
                        switch (resolucion)
                        {
                            case ParametrosFormato.ResolucionCuartoHora:
                                horas = (i) / 4;
                                minutos = ((i) % 4) * 15;
                                fechaPunto = day.AddHours(horas).AddMinutes(minutos);
                                break;
                            case ParametrosFormato.ResolucionMediaHora:
                                horas = (i) / 2;
                                minutos = ((i) % 2) * 30;
                                fechaPunto = day.AddHours(horas).AddMinutes(minutos);
                                break;
                            case ParametrosFormato.ResolucionHora:
                                fechaPunto = day.AddHours(i);
                                break;
                        }
                    }
                    else
                    {
                        switch (resolucion)
                        {
                            case ParametrosFormato.ResolucionCuartoHora:
                                horas = (i - 1) / 4;
                                minutos = ((i - 1) % 4) * 15;
                                fechaPunto = day.AddHours(horas).AddMinutes(minutos);
                                break;
                            case ParametrosFormato.ResolucionMediaHora:
                                horas = (i - 1) / 2;
                                minutos = ((i - 1) % 2) * 30;
                                fechaPunto = day.AddHours(horas).AddMinutes(minutos);
                                break;
                            case ParametrosFormato.ResolucionHora:
                                fechaPunto = day.AddHours(i - 1);
                                break;
                        }
                    }

                    PuntoGraficoMedidorGeneracion gr = new PuntoGraficoMedidorGeneracion();
                    gr.Fecha = fechaPunto;
                    gr.FechaString = gr.Fecha.ToString(ConstantesBase.FormatoFechaHora);
                    gr.ValorFuente1 = valor1;
                    gr.ValorFuente2 = valor2;

                    switch (periodo)
                    {
                        case ConstantesMedidores.PeriodoTodos:
                            listaGraf.Add(gr);
                            break;
                        case ConstantesMedidores.PeriodoHp:
                            if (i >= horaIniHP && i <= horaFinHP)
                            {
                                listaGraf.Add(gr);
                            }
                            break;
                        case ConstantesMedidores.PeriodoHfp:
                            if (i < horaIniHP || i > horaFinHP)
                            {
                                listaGraf.Add(gr);
                            }
                            break;
                    }
                }
            }

            //crear objeto grafico
            GraficoMedidorGeneracion g = new GraficoMedidorGeneracion();
            g.TipoUsuario = usuario;
            g.ListaPunto = listaGraf;
            g.TituloGrafico = "Gráfico comparativo " + nombreFuente1 + " vs " + nombreFuente2;
            g.TituloFuente1 = tituloFuente1;
            g.TituloFuente2 = tituloFuente2;
            g.LeyendaFuente1 = leyendaFuente1;
            g.LeyendaFuente2 = leyendaFuente2;
            g.Nombre = string.Format("{0}_{1:HHmmss}.xlsx", g.TituloGrafico, DateTime.Now);
            g.FechaInicio = dfechaIni.ToString(ConstantesBase.FormatoFechaHora);
            g.FechaFin = dfechaFin.ToString(ConstantesBase.FormatoFechaHora);
            g.ValorFuente1 = valorFuente1;
            g.ValorFuente2 = valorFuente2;
            g.DescPeriodo = LsPeriodo.Where(x => x.Codigo == periodo).First().Valor;
            g.DescCentral = descCentral;
            g.TipoGrafico = tipoGrafico;

            return g;
        }

        /// <summary>
        /// Lista FuenteMedidores
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="resolucion"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFuenteMedidores(int idEmpresa, int idFormato, string fechaIni, string fechaFin, int periodo, int tipoDato, int idCentral, int resolucion, ref int tipoinfocodi, bool traerDataAnterior)
        {
            List<PuntoFuenteInformacion> lista = new List<PuntoFuenteInformacion>();

            DateTime dfechaIni = fechaIni != null && fechaIni != "" ? DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime dfechaIniAnt = dfechaIni.AddDays(-1);
            DateTime dfechaFin = fechaFin != null && fechaFin != "" ? DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            //formato y cabecera
            var formato = this.servFormato.GetByIdMeFormato(idFormato);
            var cabecera = this.servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            formato.Formatcols = cabecera.Cabcolumnas;
            formato.Formatrows = cabecera.Cabfilas;
            formato.Formatheaderrow = cabecera.Cabcampodef;

            //lista hoja
            var listaHojaPto1 = this.servFormato.GetListaPtos(dfechaFin, 0, idEmpresa, idFormato, cabecera.Cabquery);
            var hojaPtoFuente1 = listaHojaPto1.Where(x => x.Equipadre == idCentral).ToList();
            List<MeMedicion96DTO> listaDataFuente = new List<MeMedicion96DTO>();
            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            //listaDataFuente = ObtenerDatos96(data, listaHojaPto1, formato.Formatcheckblanco, (int)formato.Formatrows, formato.Lectcodi);
            foreach (var hoja1 in hojaPtoFuente1)
            {
                if (!traerDataAnterior)
                {
                    listaDataFuente.AddRange(this.servFormato.GetByCriteriaMeMedicion96(dfechaIni, dfechaFin, lectcodi, hoja1.Tipoinfocodi, hoja1.Ptomedicodi));
                }
                else
                {
                    listaDataFuente.AddRange(this.servFormato.GetByCriteriaMeMedicion96(dfechaIniAnt, dfechaFin, lectcodi, hoja1.Tipoinfocodi, hoja1.Ptomedicodi));
                }
            }

            if (listaDataFuente.Count > 0)
            {
                tipoinfocodi = listaDataFuente[0].Tipoinfocodi;
            }

            //data temporal
            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                var listaDataFuente1Dia = listaDataFuente.Where(x => x.Medifecha == day).ToList();
                var listaDataFuente1DiaAnt = listaDataFuente.Where(x => x.Medifecha == day.AddDays(-1)).ToList();
                for (int i = 1; i <= 96; i++)
                {
                    int horas = i / 4;
                    int minutos = (i % 4) * 15;
                    decimal? valor1 = 0;

                    if (!traerDataAnterior)
                    {
                        foreach (var m96 in listaDataFuente1Dia)
                        {
                            decimal? valorOrigen = (decimal?)m96.GetType().GetProperty("H" + (i).ToString()).GetValue(m96, null);
                            valor1 += valorOrigen.GetValueOrDefault(0);
                        }
                    }
                    else
                    {
                        if (i <= 4)
                        {
                            foreach (var m96 in listaDataFuente1DiaAnt)
                            {
                                decimal? valorOrigen = (decimal?)m96.GetType().GetProperty("H" + (92 + i).ToString()).GetValue(m96, null);
                                valor1 += valorOrigen.GetValueOrDefault(0);
                            }
                        }
                        else
                        {
                            foreach (var m96 in listaDataFuente1Dia)
                            {
                                decimal? valorOrigen = (decimal?)m96.GetType().GetProperty("H" + (i - 4).ToString()).GetValue(m96, null);
                                valor1 += valorOrigen.GetValueOrDefault(0);
                            }
                        }

                    }

                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                    gr.FechaFiltro = day;
                    gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                    gr.ValorFuente = valor1;
                    gr.NumeroTiempo = i;

                    lista.Add(gr);
                }
            }

            //obtener data segun resolucion de destino
            lista = GetListaFinalPorResolucion(dfechaIni, dfechaFin, lista, ParametrosFormato.ResolucionCuartoHora, resolucion, tipoDato);

            return lista;
        }

        /// <summary>
        /// Lista FuenteCaudalTurbinado
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFuenteCaudalTurbinado(int idEmpresa, string fechaIni, string fechaFin, int periodo, int tipoDato, int idCentral, int resolucion)
        {
            List<PuntoFuenteInformacion> lista = new List<PuntoFuenteInformacion>();

            DateTime dfechaIni = fechaIni != null && fechaIni != "" ? DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime dfechaFin = fechaFin != null && fechaFin != "" ? DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            var formatoTurbinado = this.servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoTurbinadoEjecutadoDiario);
            var listaHojaPto2 = this.servFormato.GetByCriteriaMeHojaptomeds(idEmpresa, ConstantesMedidores.IdFormatoTurbinadoEjecutadoDiario, dfechaIni, dfechaFin);
            var hojaPtoFuente2 = listaHojaPto2.Where(x => x.Equicodi == idCentral).FirstOrDefault();
            List<MeMedicion24DTO> listaDataFuente2 = new List<MeMedicion24DTO>();
            if (hojaPtoFuente2 != null)
            {
                listaDataFuente2 = this.servFormato.GetByCriteriaMeMedicion24(dfechaIni, dfechaFin, formatoTurbinado.Lectcodi, hojaPtoFuente2.Tipoinfocodi, hojaPtoFuente2.Ptomedicodi);
            }

            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                var listaDataFuente2Dia = listaDataFuente2.Where(x => x.Medifecha == day).ToList();
                for (int i = 1; i <= 24; i++)
                {
                    decimal? valor2 = 0;
                    foreach (var m24 in listaDataFuente2Dia)
                    {
                        decimal? valorOrigen = (decimal?)m24.GetType().GetProperty("H" + (i).ToString()).GetValue(m24, null);
                        valor2 += valorOrigen.GetValueOrDefault(0);
                    }

                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                    gr.FechaFiltro = day;
                    gr.Fecha = day.AddHours(i);
                    gr.ValorFuente = valor2;
                    gr.NumeroTiempo = i;

                    lista.Add(gr);
                }
            }

            //obtener data segun resolucion de destino
            lista = GetListaFinalPorResolucion(dfechaIni, dfechaFin, lista, ParametrosFormato.ResolucionHora, resolucion, tipoDato);

            return lista;
        }

        /// <summary>
        /// Lista FuenteDespachoDiario
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="resolucion"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFuenteDespachoDiario(int idEmpresa, string fechaIni, string fechaFin, int periodo, int tipoDato, int idCentral, int resolucion, int tipoinfocodi)
        {
            List<PuntoFuenteInformacion> lista = new List<PuntoFuenteInformacion>();

            DateTime dfechaIni = fechaIni != null && fechaIni != "" ? DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime dfechaFin = fechaFin != null && fechaFin != "" ? DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            //formato y cabecera
            var formatoDespachoDiario = this.servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoDespachoEjecutadoDiario);
            var cabecera = this.servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formatoDespachoDiario.Cabcodi).FirstOrDefault();
            var listaHojaPto = this.servFormato.GetListaPtos(dfechaFin, 0, idEmpresa, formatoDespachoDiario.Formatcodi, cabecera.Cabquery);
            var hojaPtoFuente = listaHojaPto.Where(x => x.Equipadre == idCentral && x.Tipoinfocodi == tipoinfocodi).ToList();
            List<MeMedicion48DTO> listaDataFuente = new List<MeMedicion48DTO>();
            foreach (var hoja in hojaPtoFuente)
            {
                listaDataFuente.AddRange(this.servFormato.GetByCriteriaMeMedicion48(dfechaIni, dfechaFin, formatoDespachoDiario.Lectcodi, hoja.Tipoinfocodi, hoja.Ptomedicodi));
            }

            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                var listaDataFuente1Dia = listaDataFuente.Where(x => x.Medifecha == day).ToList();
                for (int i = 1; i <= 48; i++)
                {
                    int horas = i / 4;
                    int minutos = (i % 2) * 30;
                    decimal? valor1 = 0;
                    foreach (var m48 in listaDataFuente1Dia)
                    {
                        decimal? valorOrigen = (decimal?)m48.GetType().GetProperty("H" + (i).ToString()).GetValue(m48, null);
                        valor1 += valorOrigen.GetValueOrDefault(0);
                    }

                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                    gr.FechaFiltro = day;
                    gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                    gr.ValorFuente = valor1;
                    gr.NumeroTiempo = i;

                    lista.Add(gr);
                }
            }

            //obtener data segun resolucion de destino
            lista = GetListaFinalPorResolucion(dfechaIni, dfechaFin, lista, ParametrosFormato.ResolucionMediaHora, resolucion, tipoDato);

            return lista;
        }

        /// <summary>
        /// Lista FuenteDatosScada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFuenteDatosScada(int idEmpresa, string fechaIni, string fechaFin, int periodo, int tipoDato, int idCentral, int resolucion)
        {
            List<PuntoFuenteInformacion> listaFinal = new List<PuntoFuenteInformacion>();
            List<PuntoFuenteInformacion> lista = new List<PuntoFuenteInformacion>();

            DateTime dfechaIni = fechaIni != null && fechaIni != "" ? DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime dfechaFin = fechaFin != null && fechaFin != "" ? DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            //formato y cabecera
            var formato = this.servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva); //TODO el formato de despacho diario tambien para scada?
            var cabecera = this.servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            var listaHojaPto = this.servFormato.GetListaPtos(dfechaFin, 0, idEmpresa, formato.Formatcodi, cabecera.Cabquery);
            var hojaPtoFuente = listaHojaPto.Where(x => x.Equipadre == idCentral).ToList();
            List<MeScadaSp7DTO> listaDataFuente = new List<MeScadaSp7DTO>();
            foreach (var hoja in hojaPtoFuente)
            {
                listaDataFuente.AddRange(this.servFormato.GetByCriteriaFormatoScada(dfechaIni, dfechaFin, formato.Formatcodi, hoja.Tipoinfocodi, hoja.Ptomedicodi));
            }

            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                var listaDataFuente1Dia = listaDataFuente.Where(x => x.Medifecha == day).ToList();
                for (int i = 1; i <= 96; i++)
                {
                    int horas = i / 4;
                    int minutos = (i % 4) * 15;
                    decimal? valor1 = 0;
                    foreach (var m96 in listaDataFuente1Dia)
                    {
                        decimal? valorOrigen = (decimal?)m96.GetType().GetProperty("H" + (i).ToString()).GetValue(m96, null);
                        valor1 += valorOrigen.GetValueOrDefault(0);
                    }

                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                    gr.FechaFiltro = day;
                    gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                    gr.ValorFuente = valor1;
                    gr.NumeroTiempo = i;

                    lista.Add(gr);
                }
            }

            //obtener data segun resolucion de destino
            lista = GetListaFinalPorResolucion(dfechaIni, dfechaFin, lista, ParametrosFormato.ResolucionCuartoHora, resolucion, tipoDato);

            return lista;
        }

        /// <summary>
        /// Lista FuenteRPF
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFuenteRPF(int idEmpresa, string fechaIni, string fechaFin, int periodo, int tipoDato, int idCentral, int resolucion)
        {
            List<PuntoFuenteInformacion> lista = new List<PuntoFuenteInformacion>();

            DateTime dfechaIni = fechaIni != null && fechaIni != "" ? DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime dfechaFin = fechaFin != null && fechaFin != "" ? DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            //obtencion de datos
            List<int> puntosRpf = this.servFormato.ListPuntosRPF(idEmpresa, idCentral, dfechaIni);
            string rpf = string.Join<int>(Constantes.CaracterComa.ToString(), puntosRpf);

            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                //TODO actualizar la funcion de RpfAppServicio.ObtenerDatosComparacionRangoResolucion
                //List<MeMedicion60DTO> listRPF = new List<MeMedicion60DTO>();
                //if (rpf != string.Empty)
                //{
                //    listRPF = (new RpfAppServicio()).ObtenerDatosComparacionRangoResolucion(day, rpf, resolucion).ToList();
                //}

                List<ServicioCloud.Medicion> listRPF = new List<ServicioCloud.Medicion>();
                if (rpf != string.Empty)
                {
                    listRPF = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRangoResolucion(day, rpf, resolucion).ToList();
                }

                switch (resolucion)
                {
                    case ParametrosFormato.ResolucionCuartoHora:
                        for (int i = 1; i <= 96; i++)
                        {
                            int horas = i / 4;
                            int minutos = (i % 4) * 15;
                            decimal? valor1 = 0;

                            if (listRPF.Count == 96)
                            {
                                valor1 = listRPF[i - 1].H0;
                            }

                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                            gr.FechaFiltro = day;
                            gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                            gr.ValorFuente = valor1;
                            gr.NumeroTiempo = i;

                            lista.Add(gr);
                        }
                        break;
                    case ParametrosFormato.ResolucionMediaHora:
                        for (int i = 1; i <= 48; i++)
                        {
                            int horas = i / 2;
                            int minutos = (i % 2) * 30;
                            decimal? valor1 = 0;

                            if (listRPF.Count == 48)
                            {
                                valor1 = listRPF[i - 1].H0;
                            }

                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                            gr.FechaFiltro = day;
                            gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                            gr.ValorFuente = valor1;
                            gr.NumeroTiempo = i;

                            lista.Add(gr);
                        }

                        break;
                    case ParametrosFormato.ResolucionHora:
                        for (int i = 1; i <= 24; i++)
                        {
                            int horas = i;
                            decimal? valor1 = 0;

                            if (listRPF.Count == 24)
                            {
                                valor1 = listRPF[i - 1].H0;
                            }

                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                            gr.FechaFiltro = day;
                            gr.Fecha = day.AddHours(horas);
                            gr.ValorFuente = valor1;
                            gr.NumeroTiempo = i;

                            lista.Add(gr);
                        }
                        break;
                }
            }

            //obtener data segun resolucion de destino
            lista = GetListaFinalPorResolucion(dfechaIni, dfechaFin, lista, resolucion, resolucion, tipoDato);

            return lista;
        }

        /// <summary>
        /// Lista FinalPorResolucion
        /// </summary>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="lista"></param>
        /// <param name="resolucionOrigen"></param>
        /// <param name="resolucionDestino"></param>
        /// <param name="tipoDato"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFinalPorResolucion(DateTime dfechaIni, DateTime dfechaFin, List<PuntoFuenteInformacion> lista, int resolucionOrigen, int resolucionDestino, int tipoDato)
        {
            List<PuntoFuenteInformacion> listaFinal = new List<PuntoFuenteInformacion>();
            int equivalencia = 0;
            if (tipoDato != resolucionDestino)
            {
                equivalencia = ConstantesMedidores.DatoPromedio;
            }

            //datos para el grafico
            switch (resolucionDestino)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora: //De 96 a 96
                            listaFinal = lista;
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 96

                            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                            {
                                var listaDay = lista.Where(x => x.FechaFiltro.Date == day);

                                for (int i = 1; i <= 96; i += 2)
                                {
                                    var numeroTiempo = i / 2 + 1;
                                    var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempo).FirstOrDefault();

                                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                    gr.FechaFiltro = reg.FechaFiltro;
                                    gr.Fecha = reg.Fecha;
                                    gr.ValorFuente = reg.ValorFuente;
                                    gr.NumeroTiempo = i;
                                    listaFinal.Add(gr);

                                    PuntoFuenteInformacion gr2 = new PuntoFuenteInformacion();
                                    gr2.FechaFiltro = reg.FechaFiltro;
                                    gr2.Fecha = reg.Fecha;
                                    gr2.ValorFuente = reg.ValorFuente;
                                    gr2.NumeroTiempo = i + 1;
                                    listaFinal.Add(gr2);
                                }
                            }
                            break;
                        case ParametrosFormato.ResolucionHora:
                            //TODO
                            break;
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora: //De 96 a 48
                            switch (equivalencia)
                            {
                                case ConstantesMedidores.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 48; i++)
                                        {
                                            var numeroTiempoIni = (i - 1) * 2 + 1;
                                            var numeroTiempoFin = i * 2;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempoFin).FirstOrDefault();
                                            var listaProm = listaDay.Where(x => x.NumeroTiempo >= numeroTiempoIni && x.NumeroTiempo <= numeroTiempoFin).ToList();
                                            decimal? resultado = 0;
                                            if (listaProm.Count > 0)
                                            {
                                                foreach (var t in listaProm)
                                                {
                                                    resultado += t.ValorFuente.GetValueOrDefault(0);
                                                }
                                                resultado = resultado / listaProm.Count;
                                            }

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = resultado;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 48
                            listaFinal = lista;
                            break;
                        case ParametrosFormato.ResolucionHora:
                            //TODO
                            break;
                    }
                    break;
                case ParametrosFormato.ResolucionHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora://De 96 a 24
                            switch (equivalencia)
                            {
                                case ConstantesMedidores.DatoHorario:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 24; i++)
                                        {
                                            var numeroTiempo = i * 4;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempo).FirstOrDefault();

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = reg.ValorFuente;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }

                                    break;
                                case ConstantesMedidores.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 24; i++)
                                        {
                                            var numeroTiempoIni = (i - 1) * 4 + 1;
                                            var numeroTiempoFin = i * 4;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempoFin).FirstOrDefault();
                                            var listaProm = listaDay.Where(x => x.NumeroTiempo >= numeroTiempoIni && x.NumeroTiempo <= numeroTiempoFin).ToList();
                                            decimal? resultado = 0;
                                            if (listaProm.Count > 0)
                                            {
                                                foreach (var t in listaProm)
                                                {
                                                    resultado += t.ValorFuente.GetValueOrDefault(0);
                                                }
                                                resultado = resultado / listaProm.Count;
                                            }

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = resultado;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 24
                            switch (equivalencia)
                            {
                                case ConstantesMedidores.DatoHorario:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 24; i++)
                                        {
                                            var numeroTiempo = i * 2;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempo).FirstOrDefault();

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = reg.ValorFuente;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }

                                    break;
                                case ConstantesMedidores.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 24; i++)
                                        {
                                            var numeroTiempoIni = (i - 1) * 2 + 1;
                                            var numeroTiempoFin = i * 2;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempoFin).FirstOrDefault();
                                            var listaProm = listaDay.Where(x => x.NumeroTiempo >= numeroTiempoIni && x.NumeroTiempo <= numeroTiempoFin).ToList();
                                            decimal? resultado = 0;
                                            if (listaProm.Count > 0)
                                            {
                                                foreach (var t in listaProm)
                                                {
                                                    resultado += t.ValorFuente.GetValueOrDefault(0);
                                                }
                                                resultado = resultado / listaProm.Count;
                                            }

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = resultado;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionHora: //De 24 a 24
                            listaFinal = lista;
                            break;
                    }
                    break;
            }

            return listaFinal;
        }

        /// <summary>
        /// Generar archivo excel Grafico carga datos
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private string GenerarExcelGraficoDiferenteMedida(GraficoMedidorGeneracion g)
        {
            string rutaArchivoExcel = string.Empty;
            //generacion de excel
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Gráfico");
                ws.TabColor = Color.Orange;

                #region cabecera excel
                //Filtros
                int row = 7;
                int rowIniCentral = row;
                int rowIniFechaDesde = rowIniCentral + 1;
                int rowIniFechaHasta = rowIniFechaDesde + 1;
                int rowIniPeriodo = rowIniFechaHasta + 1;
                int colIniFechaDesde = 1;
                int colIniFechaHasta = colIniFechaDesde;
                int colIniPeriodo = colIniFechaHasta;

                ws.Cells[rowIniCentral, colIniFechaDesde].Value = "Central";
                ws.Cells[rowIniCentral, colIniFechaDesde + 1].Value = g.DescCentral;
                ws.Cells[rowIniFechaDesde, colIniFechaDesde].Value = "Fecha Desde";
                ws.Cells[rowIniFechaDesde, colIniFechaDesde + 1].Value = g.FechaInicio;
                ws.Cells[rowIniFechaHasta, colIniFechaHasta].Value = "Fecha Hasta";
                ws.Cells[rowIniFechaHasta, colIniFechaHasta + 1].Value = g.FechaFin;
                ws.Cells[rowIniPeriodo, colIniPeriodo].Value = "Período";
                ws.Cells[rowIniPeriodo, colIniPeriodo + 1].Value = g.DescPeriodo;

                using (var range = ws.Cells[row, colIniFechaDesde, rowIniPeriodo, colIniPeriodo + 1])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
                }

                //columnas
                row = rowIniPeriodo + 2;
                var rowIniCabecera = row;
                var colIniFuente1 = 2;
                var colIniFuente2 = colIniFuente1 + 1;

                ws.Cells[rowIniCabecera, colIniFuente1].Value = g.ValorFuente1;
                ws.Cells[rowIniCabecera, colIniFuente1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniCabecera, colIniFuente1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniCabecera, colIniFuente1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowIniCabecera, colIniFuente2].Value = g.ValorFuente2;
                ws.Cells[rowIniCabecera, colIniFuente2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniCabecera, colIniFuente2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniCabecera, colIniFuente2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                using (ExcelRange r = ws.Cells[rowIniCabecera, colIniFuente1, rowIniCabecera, colIniFuente2])
                {
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.WrapText = true;
                }
                #endregion

                #region cuerpo excel
                row += 1;
                var rowIniFecha = row;
                var rowFinFecha = rowIniFecha + g.ListaPunto.Count - 1;
                var colIniFecha = 1;

                foreach (var punto in g.ListaPunto)
                {
                    //Fecha
                    ws.Cells[row, colIniFecha].Value = punto.FechaString;

                    //Fuente1
                    ws.Cells[row, colIniFuente1].Value = punto.ValorFuente1;

                    //Fuente2
                    ws.Cells[row, colIniFuente2].Value = punto.ValorFuente2;

                    row++;
                }

                using (ExcelRange range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFuente2])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
                }


                //ancho de columnas
                ws.Column(colIniFecha).Width = 16;
                ws.Column(colIniFuente1).Width = 20;
                ws.Column(colIniFuente2).Width = 20;

                #endregion

                #region chart
                var chart1 = ws.Drawings.AddChart("Chart1", eChartType.LineMarkers) as ExcelLineChart;
                chart1.SetPosition(5, 0, colIniFuente2 + 3, 0);
                chart1.SetSize(1200, 600);
                chart1.Title.Text = g.TituloGrafico;
                chart1.DataLabel.ShowLeaderLines = false;
                chart1.YAxis.Title.Text = g.TituloFuente1;
                chart1.Legend.Position = eLegendPosition.Bottom;

                var ran0 = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha];
                var ran1 = ws.Cells[rowIniFecha, colIniFuente1, rowFinFecha, colIniFuente1];
                var ran2 = ws.Cells[rowIniFecha, colIniFuente2, rowFinFecha, colIniFuente2];

                //Fuente1
                var serie = (ExcelChartSerie)chart1.Series.Add(ran1, ran0);
                serie.Header = g.LeyendaFuente1;

                switch (g.TipoUsuario)
                {
                    case ConstantesMedidores.UsuarioCOES:
                        var chart2 = chart1.PlotArea.ChartTypes.Add(eChartType.Line);

                        //Fuente2
                        var serie2 = (ExcelChartSerie)chart2.Series.Add(ran2, ran0);
                        serie2.Header = g.LeyendaFuente2;
                        chart2.UseSecondaryAxis = true;
                        chart2.YAxis.Title.Text = g.TituloFuente2;

                        break;
                    case ConstantesMedidores.UsuarioAgentes:
                        var serie3 = (ExcelChartSerie)chart1.Series.Add(ran2, ran0);
                        serie3.Header = g.LeyendaFuente2;

                        break;
                }
                #endregion

                #region Logo
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
                #endregion

                rutaArchivoExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(rutaArchivoExcel));
            }
            return rutaArchivoExcel;
        }
        /// <summary>
        /// Generar Excel Grafico
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private string GenerarExcelGraficoIgualMedida(GraficoMedidorGeneracion g)
        {
            string rutaArchivoExcel = string.Empty;
            //generacion de excel
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Gráfico");
                ws.TabColor = Color.Orange;

                #region cabecera excel
                //Filtros
                int row = 7;
                int rowIniCentral = row;
                int rowIniFechaDesde = rowIniCentral + 1;
                int rowIniFechaHasta = rowIniFechaDesde + 1;
                int rowIniPeriodo = rowIniFechaHasta + 1;
                int colIniFechaDesde = 1;
                int colIniFechaHasta = colIniFechaDesde;
                int colIniPeriodo = colIniFechaHasta;

                ws.Cells[rowIniCentral, colIniFechaDesde].Value = "Central";
                ws.Cells[rowIniCentral, colIniFechaDesde + 1].Value = g.DescCentral;
                ws.Cells[rowIniFechaDesde, colIniFechaDesde].Value = "Fecha Desde";
                ws.Cells[rowIniFechaDesde, colIniFechaDesde + 1].Value = g.FechaInicio;
                ws.Cells[rowIniFechaHasta, colIniFechaHasta].Value = "Fecha Hasta";
                ws.Cells[rowIniFechaHasta, colIniFechaHasta + 1].Value = g.FechaFin;
                ws.Cells[rowIniPeriodo, colIniPeriodo].Value = "Período";
                ws.Cells[rowIniPeriodo, colIniPeriodo + 1].Value = g.DescPeriodo;

                using (var range = ws.Cells[row, colIniFechaDesde, rowIniPeriodo, colIniPeriodo + 1])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
                }

                //columnas
                row = rowIniPeriodo + 2;
                var rowIniCabecera = row;
                var colIniFuente1 = 2;
                var colIniFuente2 = colIniFuente1 + 1;

                ws.Cells[rowIniCabecera, colIniFuente1].Value = g.ValorFuente1;
                ws.Cells[rowIniCabecera, colIniFuente1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniCabecera, colIniFuente1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniCabecera, colIniFuente1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowIniCabecera, colIniFuente2].Value = g.ValorFuente2;
                ws.Cells[rowIniCabecera, colIniFuente2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniCabecera, colIniFuente2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniCabecera, colIniFuente2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                using (ExcelRange r = ws.Cells[rowIniCabecera, colIniFuente1, rowIniCabecera, colIniFuente2])
                {
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.WrapText = true;
                }
                #endregion

                #region cuerpo excel
                row += 1;
                var rowIniFecha = row;
                var rowFinFecha = rowIniFecha + g.ListaPunto.Count - 1;
                var colIniFecha = 1;

                foreach (var punto in g.ListaPunto)
                {
                    //Fecha
                    ws.Cells[row, colIniFecha].Value = punto.FechaString;

                    //Fuente1
                    ws.Cells[row, colIniFuente1].Value = punto.ValorFuente1;

                    //Fuente2
                    ws.Cells[row, colIniFuente2].Value = punto.ValorFuente2;

                    row++;
                }

                using (ExcelRange range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFuente2])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
                }


                //ancho de columnas
                ws.Column(colIniFecha).Width = 16;
                ws.Column(colIniFuente1).Width = 20;
                ws.Column(colIniFuente2).Width = 20;

                #endregion

                #region chart
                var chart1 = ws.Drawings.AddChart("Chart1", eChartType.LineMarkers) as ExcelLineChart;
                chart1.SetPosition(5, 0, colIniFuente2 + 3, 0);
                chart1.SetSize(1200, 600);
                chart1.Title.Text = g.TituloGrafico;
                chart1.DataLabel.ShowLeaderLines = false;
                chart1.YAxis.Title.Text = g.TituloFuente1;
                chart1.Legend.Position = eLegendPosition.Bottom;

                var ran0 = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha];
                var ran1 = ws.Cells[rowIniFecha, colIniFuente1, rowFinFecha, colIniFuente1];
                var ran2 = ws.Cells[rowIniFecha, colIniFuente2, rowFinFecha, colIniFuente2];

                //Fuente1
                var serie = (ExcelChartSerie)chart1.Series.Add(ran1, ran0);
                serie.Header = g.LeyendaFuente1;

                switch (g.TipoUsuario)
                {
                    case ConstantesMedidores.UsuarioCOES:
                        var chart2 = chart1.PlotArea.ChartTypes.Add(eChartType.Line);

                        //Fuente2
                        var serie2 = (ExcelChartSerie)chart2.Series.Add(ran2, ran0);
                        serie2.Header = g.LeyendaFuente2;
                        chart2.UseSecondaryAxis = true;
                        chart2.YAxis.Title.Text = g.TituloFuente2;

                        break;
                    case ConstantesMedidores.UsuarioAgentes:
                        var serie3 = (ExcelChartSerie)chart1.Series.Add(ran2, ran0);
                        serie3.Header = g.LeyendaFuente2;

                        break;
                }
                #endregion

                #region logo
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
                #endregion

                rutaArchivoExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(rutaArchivoExcel));
            }
            return rutaArchivoExcel;
        }
        #endregion
        #endregion

        #region util
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

            var listaEmpresa = this.servMedidores.GetListaEmpresaActivasFormato(formatcodi);
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
        #endregion

        #region Estructuras para almacenamiento de información para los aplicativos BI de Producción de energía y Máxima Demanda

        //
        // GET: /Medidores/Reportes/GenerarMaximaDemanda
        public ActionResult GenerarMaximaDemanda()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            return View(model);
        }

        /// <summary>
        /// Procesar maxima demanda segun fecha escogida por el usuario
        /// </summary>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        public JsonResult GuardarMaximaDemanda(string mesIni, string mesFin)
        {
            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime fechaIniProceso = EPDate.GetFechaIniPeriodo(5, mesIni, string.Empty, string.Empty, string.Empty);
                DateTime fechaFinProceso = EPDate.GetFechaIniPeriodo(5, mesFin, string.Empty, string.Empty, string.Empty);

                if (fechaIniProceso > fechaFinProceso)
                {
                    throw new Exception("La fecha de inicio no debe ser mayor a la fecha de fin");
                }

                if (fechaIniProceso.AddMonths(4) <= fechaFinProceso)
                {
                    throw new Exception("El rango a consultar no debe exceder los 4 meses");
                }

                this.servReporte.GuardarEstructurasMaximaDemanda(fechaIniProceso, fechaFinProceso, User.Identity.Name);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Procesar maxima demanda segun fecha escogida por el usuario
        /// </summary>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        public JsonResult GuardarProduccionGeneracion(string mesIni, string mesFin)
        {
            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime fechaIniProceso = EPDate.GetFechaIniPeriodo(5, mesIni, string.Empty, string.Empty, string.Empty);
                DateTime fechaFinProceso = EPDate.GetFechaIniPeriodo(5, mesFin, string.Empty, string.Empty, string.Empty);

                if (fechaIniProceso > fechaFinProceso)
                {
                    throw new Exception("La fecha de inicio no debe ser mayor a la fecha de fin");
                }

                if (fechaIniProceso.AddMonths(4) <= fechaFinProceso)
                {
                    throw new Exception("El rango a consultar no debe exceder los 4 meses");
                }

                this.servReporte.GuardarEstructurasProduccionGeneracionYResumen(fechaIniProceso, fechaFinProceso, User.Identity.Name);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            return Json(model);
        }


        /// <summary>
        /// Procesar factor planta segun fecha escogida por el usuario
        /// </summary>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        public JsonResult GuardarFactorPlanta(string mesIni, string mesFin)
        {
            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime fechaIniProceso = EPDate.GetFechaIniPeriodo(5, mesIni, string.Empty, string.Empty, string.Empty);
                DateTime fechaFinProceso = EPDate.GetFechaIniPeriodo(5, mesFin, string.Empty, string.Empty, string.Empty);

                if (fechaIniProceso.Year != fechaFinProceso.Year)
                {
                    throw new Exception("La fecha de inicio y la fecha de fin deben ser del mismo año");
                }

                if (fechaIniProceso > fechaFinProceso)
                {
                    throw new Exception("La fecha de inicio no debe ser mayor a la fecha de fin");
                }

                if (fechaIniProceso.AddMonths(4) <= fechaFinProceso)
                {
                    throw new Exception("El rango a consultar no debe exceder los 4 meses");
                }

                this.servReporte.GuardarEstructurasFactorPlanta(fechaIniProceso, fechaFinProceso, User.Identity.Name);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Procesar Potencia Efectiva e Instalada segun fecha escogida por el usuario
        /// </summary>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        public JsonResult GuardarPotPEInst(string mesIni, string mesFin)
        {
            ReporteMaximaDemandaModel model = new ReporteMaximaDemandaModel();

            try
            {
                this.ValidarSesionJsonResult();

                DateTime fechaIniProceso = EPDate.GetFechaIniPeriodo(5, mesIni, string.Empty, string.Empty, string.Empty);
                DateTime fechaFinProceso = EPDate.GetFechaIniPeriodo(5, mesFin, string.Empty, string.Empty, string.Empty);

                if (fechaIniProceso.Year != fechaFinProceso.Year)
                {
                    throw new Exception("La fecha de inicio y la fecha de fin deben ser del mismo año");
                }

                if (fechaIniProceso > fechaFinProceso)
                {
                    throw new Exception("La fecha de inicio no debe ser mayor a la fecha de fin");
                }

                if (fechaIniProceso.AddMonths(4) <= fechaFinProceso)
                {
                    throw new Exception("El rango a consultar no debe exceder los 4 meses");
                }

                this.servReporte.GuardarEstructurasPotEfectiva(fechaIniProceso, fechaFinProceso, User.Identity.Name);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            return Json(model);
        }

        #endregion
    }
}
