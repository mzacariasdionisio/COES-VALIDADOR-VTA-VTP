using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Medidores;
using COES.MVC.Publico.Areas.Medidores.Models;
using COES.Dominio.DTO.Sic;
using System.Globalization;
using COES.Base.Core;
using System.Configuration;
using COES.MVC.Publico.Helper;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using COES.Servicios.Aplicacion.Mediciones;
using COES.MVC.Publico.Areas.Medidores.Helpers;

namespace COES.MVC.Publico.Areas.Medidores.Controllers
{
    public class ReportesController : Controller
    {

        /// <summary>
        /// Muestra la pantalla del reporte de máxima demanda
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BusquedaMaximaDemandaModel model = new BusquedaMaximaDemandaModel();
            model.FechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaTipoGeneracion = (new ConsultaMedidoresAppServicio()).ListaTipoGeneracion();
            model.ListaTipoEmpresas = (new ConsultaMedidoresAppServicio()).ListaTipoEmpresas();
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
            MaximaDemandaDia regMaxDiaTotal = new MaximaDemandaDia();
            regMaxDiaTotal.valores = new List<decimal>();
            regMaxDiaTotal.Gruponomb = "TOTAL";
            MaximaDemandaDia regMaxDiaHoramin = new MaximaDemandaDia();
            regMaxDiaHoramin.horamin = new List<string>();
            regMaxDiaHoramin.Gruponomb = "HORA MINUNTO";
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
            foreach (var reg in lista)
            {
                string sNombreGrupo = string.IsNullOrEmpty(reg.Gruponomb) ? string.Empty : reg.Gruponomb.Trim();
                if (GrupoActual != sNombreGrupo)
                {
                    if (GrupoActual != "X")
                    {
                        listaMaximaDemandaDia.Add(regMaxDia);
                        diaAnt = 0;
                    }
                    regMaxDia = new MaximaDemandaDia();
                    regMaxDia.valores = new List<decimal>();
                    regMaxDia.Empresanomb = string.IsNullOrEmpty(reg.Empresanomb)? string.Empty :reg.Empresanomb.Trim();
                    regMaxDia.Centralnomb = string.IsNullOrEmpty(reg.Centralnomb) ? string.Empty : reg.Centralnomb.Trim();
                    regMaxDia.Gruponomb = string.IsNullOrEmpty(reg.Gruponomb) ? string.Empty : reg.Gruponomb.Trim();
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
                GrupoActual = string.IsNullOrEmpty(reg.Gruponomb) ? string.Empty : reg.Gruponomb.Trim();
            }

            if (lista.Count > 0)
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
                regMaxDiaTotal.valores = new List<decimal>();
                regMaxDiaTotal.Gruponomb = "TOTAL";
                MaximaDemandaDia regMaxDiaHoramin = new MaximaDemandaDia();
                regMaxDiaHoramin.horamin = new List<string>();
                regMaxDiaHoramin.Gruponomb = "HORA MINUNTO";
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
                foreach (var reg in lista)
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

                if (lista.Count > 0)
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
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, app, file);
        }


        /// <summary>
        /// Pagina de inicio reporte de hora punta y fuera de punta
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_HFP_HP()
        {
            BusquedaMaximaDemandaModel model = new BusquedaMaximaDemandaModel();
            model.FechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaTipoGeneracion = (new ConsultaMedidoresAppServicio()).ListaTipoGeneracion();
            model.ListaTipoEmpresas = (new ConsultaMedidoresAppServicio()).ListaTipoEmpresas();
            model.ParametroDefecto = 0;
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
            int mes;
            int anho;

            DateTime fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            DateTime fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            if (fecha != null)
            {
                mes = Int32.Parse(fecha.Substring(0, 2));
                anho = Int32.Parse(fecha.Substring(3, 4));
                fechaInicio = new DateTime(anho, mes, 1);
                fechaFin = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).AddMonths(1).AddDays(-1);
            }

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            var lista = (new MedidoresAppServicio()).ListarDemandaDiaHFPHP(fechaInicio, fechaFin, tiposEmpresa, empresas,
                tiposGeneracion, central).OrderBy(x => x.Medifecha).ToList();

            MaximaDemanda model = new MaximaDemanda();
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

                int mes;
                int anho;

                DateTime fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                DateTime fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                if (fecha != null)
                {
                    mes = Int32.Parse(fecha.Substring(0, 2));
                    anho = Int32.Parse(fecha.Substring(3, 4));
                    fechaInicio = new DateTime(anho, mes, 1);
                    fechaFin = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).AddMonths(1).AddDays(-1);
                }

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                var lista = (new MedidoresAppServicio()).ListarDemandaDiaHFPHP(fechaInicio, fechaFin, tiposEmpresa, empresas,
                    tiposGeneracion, central).OrderBy(x => x.Medifecha).ToList();

                MaximaDemanda model = new MaximaDemanda();
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

                MedidorHelper.GenerarReporteMaximaDemandaHFPHP(model, fechaInicio.ToString(Constantes.FormatoFecha),
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
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, app, file);
        }
    }
}
