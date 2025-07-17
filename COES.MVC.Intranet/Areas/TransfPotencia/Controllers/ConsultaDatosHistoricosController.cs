using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using log4net;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using Newtonsoft.Json;
using System.Linq;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class ConsultaDatosHistoricosController : BaseController
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsultaDatosHistoricosController));
        private static string NombreControlador = "ConsultaDatosHistoricosController";

        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        CentralGeneracionAppServicio servicioCentralGeneracion = new CentralGeneracionAppServicio();
        TransfPotenciaAppServicio servicioPotencia = new TransfPotenciaAppServicio();
        public ActionResult Index(int pericodi = 0, int pericodi2 = 0)
        {
            base.ValidarSesionUsuario();
            PeajeEgresoMinfoModel model = new PeajeEgresoMinfoModel();
            model.ListaEmpresas = this.servicioEmpresa.ListarEmpresasComboActivos();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.ListaCentrales = this.servicioCentralGeneracion.ListCentralGeneracion();

            if (pericodi >= 0 || pericodi2 >= 0)
            {
                if (Session["PeriCodi"] == null)
                {
                    Session["PeriCodi"] = pericodi;
                }
                if (Session["PeriCodi2"] == null)
                {
                    Session["PeriCodi2"] = pericodi2;
                }
                if (Session["PeriCodi"] != null)
                {
                    Session["PeriCodi"] = pericodi;
                }
                if (Session["PeriCodi2"] != null)
                {
                    Session["PeriCodi2"] = pericodi2;
                }
                model.ListaRecalculoPotencia = this.servicioPotencia.ListByPericodiVtpRecalculoPotencia(Convert.ToInt32(Session["PeriCodi"])); //Ordenado en descendente
                model.ListaRecalculoPotencia2 = this.servicioPotencia.ListByPericodiVtpRecalculoPotencia(Convert.ToInt32(Session["PeriCodi2"]));
                if (model.ListaRecalculoPotencia.Count > 0)
                {
                    model.Pericodi = Convert.ToInt32(Session["PeriCodi"]);
                    model.Recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi;
                }

                if (model.ListaRecalculoPotencia2.Count > 0)
                {
                    model.Pericodi2 = Convert.ToInt32(Session["PeriCodi2"]);
                    model.Recpotcodi2 = (int)model.ListaRecalculoPotencia2[0].Recpotcodi;
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ListaConsulta(int periini, int perifin, int emprcodi, int equicodi, int pegrtipinfo, int recpotcodiConsulta, string datos = "")
        {
            List<String> periodos = new List<String>();
            ConsultaDatosModel model = new ConsultaDatosModel();
            model.TipoComp = pegrtipinfo;
            if (pegrtipinfo == 0)
            {
                model.ListaConsulta = this.servicioPotencia.GetIngresoPotUnidPromdByComparative(periini, perifin, emprcodi, equicodi, ref periodos, pegrtipinfo, recpotcodiConsulta);
            }
            else if (pegrtipinfo == 1)
            {
                List<VtpEmpresaPagoDTO> lstEmpresasBack = new List<VtpEmpresaPagoDTO>();
                List<VtpEmpresaPagoDTO> lstEmpresa = this.servicioPotencia.GetEmpresaPagoByComparative(periini, perifin, emprcodi, ref periodos, recpotcodiConsulta);
                foreach (VtpEmpresaPagoDTO item2 in lstEmpresa)
                {
                    VtpEmpresaPagoDTO itemEmpresa = lstEmpresasBack.Find(x => x.Emprcodipago == item2.Emprcodipago);
                    if (itemEmpresa == null && item2.lstImportesPromd.Any())
                    {
                        lstEmpresasBack.Add(item2);
                    }
                }
                foreach (VtpEmpresaPagoDTO item3 in lstEmpresa)
                {
                    VtpEmpresaPagoDTO itemEmpresa = lstEmpresasBack.Find(x => x.Emprcodicobro == item3.Emprcodicobro && x.Emprcodicobro != 0);
                    if (itemEmpresa == null && item3.lstImportesPromd.Any())
                    {
                        lstEmpresasBack.Add(item3);
                    }
                }
                var distinctEmpresa = lstEmpresasBack
                      .GroupBy(p => p.lstImportesPromd.Sum())
                      .Select(g => g.First())
                      .OrderBy(x => x.Emprnombpago)
                      .ToList();

                List<VtpEmpresaPagoDTO> empresasFinal = new List<VtpEmpresaPagoDTO>();
                foreach (var empresa in distinctEmpresa)
                {
                    var empresafiltro = empresasFinal.FirstOrDefault(x => x.Emprnombpago == empresa.Emprnombpago);
                    if (empresafiltro != null)
                    {
                        for (int i = 0; i < empresafiltro.lstImportesPromd.Count(); ++i)
                        {
                            empresafiltro.lstImportesPromd[i] = empresafiltro.lstImportesPromd[i] + empresa.lstImportesPromd[i];
                        }
                    }
                    else
                    {
                        empresasFinal.Add(empresa);
                    }

                }

                model.ListaConsultaValorizacionPotencia = empresasFinal;
            }
            else
            {
                List<int> lstCargos = new List<int>();
                if (datos != "[]")
                {
                    string dataStr = JsonConvert.DeserializeObject<string>(datos);
                    string[] dataList = dataStr.Split(',');
                    for (int i = 0; i < dataList.Length; i++)
                    {
                        lstCargos.Add(int.Parse(dataList[i]));
                    }
                }
                model.ListaConsultaPeajeEmpresaPago = this.servicioPotencia.GetPeajeEmpresaPagoByComparative(periini, perifin, emprcodi, equicodi, ref periodos, lstCargos, recpotcodiConsulta);
            }
            model.ListaPeriodos = periodos;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ListaComparacion(int periini, int recpotini, int perifin, int recpotfin, int emprcodi, int pegrtipinfo)
        {
            List<String> periodos = new List<String>();
            ConsultaDatosModel model = new ConsultaDatosModel();
            model.TipoComp = pegrtipinfo;
            if (pegrtipinfo == 0)
            {
                model.ListaConsulta = this.servicioPotencia.GetIngresoPotUnidPromdByHistComp(periini, recpotini, perifin, recpotfin, emprcodi, ref periodos, pegrtipinfo);
            }
            else if (pegrtipinfo == 1)
            {
                List<VtpEmpresaPagoDTO> lstEmpresasBack = new List<VtpEmpresaPagoDTO>();
                List<VtpEmpresaPagoDTO> lstEmpresa = this.servicioPotencia.GetEmpresaPagoByHist(periini, recpotini, perifin, recpotfin, emprcodi, ref periodos);
                foreach (VtpEmpresaPagoDTO item2 in lstEmpresa)
                {
                    VtpEmpresaPagoDTO itemEmpresa = lstEmpresasBack.Find(x => x.Emprcodipago == item2.Emprcodipago);
                    if (itemEmpresa == null && item2.lstImportesPromd.Any())
                    {
                        lstEmpresasBack.Add(item2);
                    }
                }
                foreach (VtpEmpresaPagoDTO item3 in lstEmpresa)
                {
                    VtpEmpresaPagoDTO itemEmpresa = lstEmpresasBack.Find(x => x.Emprcodicobro == item3.Emprcodicobro && x.Emprcodicobro != 0);
                    if (itemEmpresa == null && item3.lstImportesPromd.Any())
                    {
                        lstEmpresasBack.Add(item3);
                    }
                }
                var distinctEmpresa = lstEmpresasBack
                       .GroupBy(p => p.lstImportesPromd.Sum())
                       .Select(g => g.First())
                       .OrderBy(x => x.Emprnombpago)
                       .ToList();
                List<VtpEmpresaPagoDTO> empresasFinal = new List<VtpEmpresaPagoDTO>();
                foreach (var empresa in distinctEmpresa)
                {
                    var empresafiltro = empresasFinal.FirstOrDefault(x => x.Emprnombpago == empresa.Emprnombpago);
                    if (empresafiltro != null)
                    {
                        for (int i = 0; i < empresafiltro.lstImportesPromd.Count(); ++i)
                        {
                            empresafiltro.lstImportesPromd[i] = empresafiltro.lstImportesPromd[i] + empresa.lstImportesPromd[i];
                        }
                    }
                    else
                    {
                        empresasFinal.Add(empresa);
                    }
                }

                foreach (var x in empresasFinal)
                {
                    if (x.lstImportesPromd.Count == 0 || x.lstImportesPromd.Count == 1)
                    {
                        x.PorcentajeVariacion = 0;
                    }
                    else
                    {
                        x.PorcentajeVariacion = x.lstImportesPromd[0] == 0 ? 0 : ((x.lstImportesPromd[0] - x.lstImportesPromd[1]) / x.lstImportesPromd[0]);
                        x.PorcentajeVariacion = x.PorcentajeVariacion < 0 ? (x.PorcentajeVariacion * -100) : (x.PorcentajeVariacion * 100);
                        x.PorcentajeVariacion = Math.Round(x.PorcentajeVariacion, 2);
                    }
                }
                model.ListaConsultaValorizacionPotencia = empresasFinal;
            }
            else
            {
                model.ListaConsultaPeajeEmpresaPago = this.servicioPotencia.GetPeajeEmpresaPagoByHist(periini, recpotini, perifin, recpotfin, emprcodi, ref periodos);
            }
            model.ListaPeriodos = periodos;
            return PartialView(model);
        }

        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int periini = 0, int recpotini = 0, int perifin = 0, int recpotfin = 0, int emprcodi = 0, int equicodi = 0, int formato = 1, int opt = 0, int pegrtipinfo = 0, int recpotcodiConsulta = 0, string datos = "")
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = this.servicioPotencia.GenerarReporteConsultaHistoricos(periini, recpotini, perifin, recpotfin, emprcodi, equicodi, formato, pathFile, pathLogo, opt, pegrtipinfo, datos, recpotcodiConsulta);
                return Json(file);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ExportarData", ex);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult ObtenerEmpresas()
        {
            base.ValidarSesionUsuario();
            try
            {
                List<EmpresaDTO> lstEmpresas = this.servicioEmpresa.ListarEmpresasComboActivos();
                EmpresaDTO emprCerroVerde = new EmpresaDTO
                {
                    EmprNombre = "MINERA CERRO VERDE - GU",
                    EmprCodi = -1002
                };
                lstEmpresas.Add(emprCerroVerde);
                return Json(lstEmpresas);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ExportarData", ex);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult ObtenerEmpresasGeneradoras(int periini = 0)
        {
            base.ValidarSesionUsuario();
            try
            {
                List<VtpPeajeEmpresaPagoDTO> lstEmpresasBack = new List<VtpPeajeEmpresaPagoDTO>();
                List<VtpPeajeEmpresaPagoDTO> lstPeajeEmpresaPago = this.servicioPotencia.ListVtpPeajeEmpresaPagoPeajeCobroSelect(periini, 1);

                foreach (VtpPeajeEmpresaPagoDTO item2 in lstPeajeEmpresaPago)
                {
                    VtpPeajeEmpresaPagoDTO itemEmpresa = lstEmpresasBack.Find(x => x.Emprcodicargo == item2.Emprcodicargo);
                    if (itemEmpresa == null)
                    {
                        lstEmpresasBack.Add(item2);
                    }
                }

                return Json(lstEmpresasBack);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ExportarData", ex);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult ObtenerCargos(int periini = 0, int emprcodi = 0)
        {
            base.ValidarSesionUsuario();
            try
            {
                List<VtpPeajeIngresoDTO> lstPeajeEmpresaPago = this.servicioPotencia.ListGetByEmpresaGeneradora(periini, 1, emprcodi);
                return Json(lstPeajeEmpresaPago);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ExportarData", ex);
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Descarga el archivo
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }
    }
}
