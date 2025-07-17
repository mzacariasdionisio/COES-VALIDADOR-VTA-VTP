using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.MVC.Extranet.Areas.Medidores.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using System.Configuration;

namespace COES.MVC.Extranet.Areas.Medidores.Controllers
{
    public class ReporteController : BaseController
    {
        private SeguridadServicioClient servSeguridad = new SeguridadServicioClient();
        private FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string nameController = "ReporteController";

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(nameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(nameController, ex);
                throw;
            }
        }



        /// <summary>
        /// Listar empresas por Usuario y Formato
        /// </summary>
        /// <param name="acceso"></param>
        /// <param name="usuario"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresaByFormatoYUsuario(int acceso, string usuario, int idFormato)
        {
            List<SiEmpresaDTO> listaEmpresas = new List<SiEmpresaDTO>();
            bool accesoEmpresa = base.VerificarAccesoAccion(acceso, usuario);
            //accesoEmpresa = true;
            List<SiEmpresaDTO> empresas = ConstantesHard.IdFormatoEnergiaPrimaria != idFormato ? servFormato.GetListaEmpresaFormato(idFormato)
                : this.servFormato.GetListaEmpresaFormatoEnergiaPrimaria(ConstantesHard.IdFormatoEnergiaPrimaria);
            if (accesoEmpresa)
            {
                if (empresas.Count > 0)
                    listaEmpresas = empresas;
                else
                {
                    listaEmpresas = new List<SiEmpresaDTO>(){
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
                        Emprnomb = x.EMPRNOMB,
                        Tipoemprcodi = x.TIPOEMPRCODI
                    });
                if (emprUsuario.Count() > 0)
                {
                    listaEmpresas = emprUsuario.ToList();

                }
                else
                {
                    listaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }

            return listaEmpresas.OrderBy(x => x.Emprnomb).ToList();
        }

        #region Consolidado de Envio
        //
        // GET: /Medidores/Reporte/ConsolidadoEnvio
        public ActionResult ConsolidadoEnvio()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            var model = new ReporteConsolidadoEnvioModel();
            model.IdFormato = ConstantesMedidores.IdFormatoCargaCentralPotActiva;
            model.ListaEmpresas = ListarEmpresaByFormatoYUsuario(Acciones.AccesoEmpresa, User.Identity.Name, model.IdFormato);
            model.Fecha = DateTime.Now.AddDays(-1).ToString(ConstantesBase.FormatoFecha);
            model.MesIni = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.MesFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            return View(model);
        }

        #region Modificacion PR15 - 24/11/2017
        /// <summary>
        /// Model para web y excel
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        private ReporteConsolidadoEnvioModel GenerarModel(int idEmpresa, string mesIni, string mesFin)
        {
            var model = new ReporteConsolidadoEnvioModel();

            var formato = this.servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva);
            var fechaProcesoIni = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mesIni, string.Empty, string.Empty, string.Empty);
            var fechaProcesoFin = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mesFin, string.Empty, string.Empty, string.Empty);

            DateTime fechaInicio = fechaProcesoIni;
            DateTime fechaFin = fechaProcesoFin.AddMonths(1).AddDays(-1);

            //envio
            var idEnvioLast = 0;
            var listaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesMedidores.IdFormatoCargaCentralPotActiva, fechaProcesoIni);

            if (listaEnvios.Count > 0)
            {
                idEnvioLast = listaEnvios[listaEnvios.Count - 1].Enviocodi;
                var envioAnt = servFormato.GetByIdMeEnvio(idEnvioLast);

                var usuarioEnvio = this.servSeguridad.ObtenerUsuarioPorLogin(envioAnt.Userlogin);
                model.UsuarioNombre = usuarioEnvio.UsernName;
                model.UsuarioCargo = usuarioEnvio.UserCargo;
                model.UsuarioCorreo = usuarioEnvio.UserEmail;
                model.UsuarioTelefono = usuarioEnvio.UserTlf;
                model.FechaEnvio = envioAnt.Enviofecha.Value.ToString(ConstantesBase.FormatoFechaHora);
            }
            model.IdEnvio = idEnvioLast;
            model.FechaConsulta = DateTime.Now.ToString(ConstantesBase.FormatoFechaHora);

            //data
            List<ConsolidadoEnvioDTO> reporteConsolidado = new List<ConsolidadoEnvioDTO>();
            reporteConsolidado.AddRange(this.servFormato.GetConsolidadoEnvioByEmpresaAndFormato96(idEmpresa, formato.Lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva, fechaInicio, fechaFin));
            //reporteConsolidado.AddRange(this.servFormato.GetConsolidadoEnvioByEmpresaAndFormato96(idEmpresa, formato.Lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva, fechaInicio, fechaFin));

            var rows = from consolidado in reporteConsolidado
                       group consolidado by consolidado.Central into g
                       select new ConsolidadoCentral
                       {
                           CentralHead = g.Key.ToString(),
                           total = g.Where(x => x.TipoGeneracion < 20).Sum(x => x.Total),
                           Ngrupo = g.Where(x => x.TipoGeneracion< 20).Count(),
                           listaGrupo = reporteConsolidado.Where(x => x.Central == g.Key.ToString()).Select(y => new GrupoSSAA() { Nombre = y.GrupSSAA, SubTotal = y.Total, tipoG = y.TipoGeneracion }).ToList()
                       };

            model.ListaConsolidado = rows.ToList();


            return model;
        }
        #endregion

        /// <summary>
        /// Consolidado Envio por Empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        public PartialViewResult ConsolidadoEnvioByEmpresa(int idEmpresa, string mesIni, string mesFin)
        {
            var model = GenerarModel(idEmpresa, mesIni, mesFin);

            return PartialView(model);
        }

        /// <summary>
        /// Generar excel
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        public string ExportaExcelConsolidadoEnvio(int idEmpresa, string mesIni, string mesFin)
        {
            string ruta = string.Empty;
            try
            {
                var model = GenerarModel(idEmpresa, mesIni, mesFin);
                ruta = GenerarFileExcelConsolidadoEnvio(model);
            }
            catch (Exception ex)
            {
                log.Error(nameController, ex);
                ruta = "-1";
            }
            return ruta;
        }

        /// <summary>
        /// Descargar excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelConsolidadoEnvio()
        {
            string strArchivoTemporal = Request["archivo"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("ConsolidadoEnvio_{0:MM_yyyy}.xlsx", DateTime.Now);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        /// <summary>
        /// Generar File Excel Consolidado Envio
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GenerarFileExcelConsolidadoEnvio(ReporteConsolidadoEnvioModel model)
        {
            //generacion de excel
            string fileExcel = string.Empty;
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Consolidado");

                int row = 6;
                #region filtros
                int rowIniResp = row;
                int rowIniCargo = rowIniResp + 1;
                int rowIniCorreo = rowIniCargo + 1;
                int rowIniPeriodo = rowIniCorreo + 1;
                int rowIniTlf = rowIniPeriodo + 1;
                int rowIniFecEnvio = rowIniTlf + 1;
                int rowIniFecConsulta = rowIniFecEnvio + 2;

                int colIniResp = 1;
                int colIniCargo = colIniResp;
                int colIniCorreo = colIniCargo;
                int colIniTlf = colIniCorreo;
                int colIniFecEnvio = colIniTlf;
                int colIniFecConsulta = colIniFecEnvio;

                ws.Cells[rowIniResp, colIniResp].Value = "Responsable de Envío";
                ws.Cells[rowIniResp, colIniResp].Style.Font.Bold = true;
                ws.Cells[rowIniResp, colIniResp + 1].Value = model.UsuarioNombre;
                ws.Cells[rowIniCargo, colIniCargo].Value = "Cargo";
                ws.Cells[rowIniCargo, colIniCargo].Style.Font.Bold = true;
                ws.Cells[rowIniCargo, colIniCargo + 1].Value = model.UsuarioCargo;
                ws.Cells[rowIniCorreo, colIniCorreo].Value = "Correo";
                ws.Cells[rowIniCorreo, colIniCorreo].Style.Font.Bold = true;
                ws.Cells[rowIniCorreo, colIniCorreo + 1].Value = model.UsuarioCorreo;
                ws.Cells[rowIniTlf, colIniTlf].Value = "Teléfono";
                ws.Cells[rowIniTlf, colIniTlf].Style.Font.Bold = true;
                ws.Cells[rowIniTlf, colIniTlf + 1].Value = model.UsuarioTelefono;
                ws.Cells[rowIniFecEnvio, colIniFecEnvio].Value = "Fecha de envío";
                ws.Cells[rowIniFecEnvio, colIniFecEnvio].Style.Font.Bold = true;
                ws.Cells[rowIniFecEnvio, colIniFecEnvio + 1].Value = model.FechaEnvio;

                ws.Cells[rowIniFecConsulta, colIniFecConsulta].Value = "Fecha de consulta";
                ws.Cells[rowIniFecConsulta, colIniFecConsulta + 1].Value = model.FechaConsulta;

                using (var range = ws.Cells[rowIniResp, colIniResp, rowIniFecEnvio, colIniFecEnvio + 1])
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
                #endregion

                #region cabecera detalle
                row = rowIniFecConsulta + 2;

                int rowIniCentral = row;
                int rowFinCentral = rowIniCentral + 1;
                int rowIniUnidad = rowIniCentral;
                int rowFinUnidad = rowIniUnidad + 1;
                int rowIniProduccion = rowIniCentral;
                int rowIniTipoGen = rowIniProduccion + 1;
                int rowIniSerAux = rowIniCentral;
                int rowFinSerAux = rowIniSerAux + 1;

                int colIniCentral = 1;
                int colIniUnidad = colIniCentral + 1;
                int colIniProduccion = colIniUnidad + 1;
                int colFinProduccion = colIniProduccion + 3;
                int colIniHidro = colIniUnidad + 1;
                int colIniTermo = colIniHidro + 1;
                int colIniSolar = colIniTermo + 1;
                int colIniEolica = colIniSolar + 1;
                int colIniSerAux = colFinProduccion + 1;

                ws.Cells[rowIniCentral, colIniCentral].Value = "Central";
                ws.Cells[rowIniCentral, colIniCentral, rowFinCentral, colIniCentral].Merge = true;
                ws.Cells[rowIniCentral, colIniCentral, rowFinCentral, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[rowIniUnidad, colIniUnidad].Value = "Unidad";
                ws.Cells[rowIniUnidad, colIniUnidad, rowFinUnidad, colIniUnidad].Merge = true;
                ws.Cells[rowIniUnidad, colIniUnidad, rowFinUnidad, colIniUnidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[rowIniProduccion, colIniProduccion].Value = "Producción de Energía (MWh)";
                ws.Cells[rowIniProduccion, colIniProduccion, rowIniProduccion, colFinProduccion].Merge = true;
                ws.Cells[rowIniProduccion, colIniProduccion, rowIniProduccion, colFinProduccion].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[rowIniTipoGen, colIniHidro].Value = "Hidroeléctrica";
                ws.Cells[rowIniTipoGen, colIniHidro].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[rowIniTipoGen, colIniTermo].Value = "Termoeléctrica";
                ws.Cells[rowIniTipoGen, colIniTermo].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[rowIniTipoGen, colIniSolar].Value = "Solar";
                ws.Cells[rowIniTipoGen, colIniSolar].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[rowIniTipoGen, colIniEolica].Value = "Eólica";
                ws.Cells[rowIniTipoGen, colIniEolica].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[rowIniSerAux, colIniSerAux].Value = "Servicios Auxiliares (MWh)";
                ws.Cells[rowIniSerAux, colIniSerAux, rowFinSerAux, colIniSerAux].Merge = true;
                ws.Cells[rowIniSerAux, colIniSerAux, rowFinSerAux, colIniSerAux].Style.WrapText = true;
                ws.Cells[rowIniSerAux, colIniSerAux, rowFinSerAux, colIniSerAux].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                using (var range = ws.Cells[rowIniCentral, colIniCentral, rowFinCentral, colIniSerAux])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                #endregion

                row = rowFinCentral + 1;
                decimal? totalSA = 0.0M;
                decimal? totalEmpresaH = 0.0M;
                decimal? totalEmpresaT = 0.0M;
                decimal? totalEmpresaE = 0.0M;
                decimal? totalEmpresaS = 0.0M;
                decimal? totalEmpresaSA = 0.0M;
                decimal? totalEmpresaSAH = 0.0M;
                decimal? totalEmpresaSAT = 0.0M;
                decimal? totalEmpresaSAE = 0.0M;
                decimal? totalEmpresaSAS = 0.0M;

                #region cuerpo detalle
                var i = 0;
                decimal? subTotalH = 0.0M;
                decimal? subTotalT = 0.0M;
                decimal? subTotalE = 0.0M;
                decimal? subTotalS = 0.0M;
                decimal? subTotalSA = 0.0M;
                decimal? totalH = 0.0M;
                decimal? totalT = 0.0M;
                decimal? totalE = 0.0M;
                decimal? totalS = 0.0M;

                int NroGrupos = 0;
                int numError = 0;
                foreach (var item in model.ListaConsolidado)
                {
                    numError++;
                    i = 0;
                    subTotalH = null;
                    subTotalT = null;
                    subTotalE = null;
                    subTotalS = null;
                    totalH = 0.0M;
                    totalT = 0.0M;
                    totalE = 0.0M;
                    totalS = 0.0M;
                    totalSA = 0.0M;
                    NroGrupos = item.Ngrupo;

                    foreach (var subitem in item.listaGrupo)
                    {
                        switch (subitem.tipoG)
                        {
                            case 1: subTotalH = subitem.SubTotal;
                                totalH = item.total;       /// Solo basta con el primer grupo para totalizar el tipo de Generacion
                                totalEmpresaH += subitem.SubTotal;
                                break;
                            case 2: subTotalT = subitem.SubTotal;
                                totalT = item.total;
                                totalEmpresaT += subitem.SubTotal;
                                break;
                            case 4: subTotalE = subitem.SubTotal;
                                totalE = item.total;
                                totalEmpresaE += subitem.SubTotal;
                                break;
                            case 3:
                                subTotalS = subitem.SubTotal;
                                totalS = item.total;
                                totalEmpresaS += subitem.SubTotal;

                                break;
                            case 20:
                                subTotalSA = subitem.SubTotal;
                                totalSA += subitem.SubTotal;
                                totalEmpresaSA += subitem.SubTotal;
                                totalEmpresaSAH += subitem.SubTotal;
                                break;
                            case 21:
                                subTotalSA = subitem.SubTotal;
                                totalSA += subitem.SubTotal;
                                totalEmpresaSA += subitem.SubTotal;
                                totalEmpresaSAT += subitem.SubTotal;
                                break;
                            case 22:
                                subTotalSA = subitem.SubTotal;
                                totalSA += subitem.SubTotal;
                                totalEmpresaSA += subitem.SubTotal;
                                totalEmpresaSAS += subitem.SubTotal;
                                break;
                            case 23:
                                subTotalSA = subitem.SubTotal;
                                totalSA += subitem.SubTotal;
                                totalEmpresaSA += subitem.SubTotal;
                                totalEmpresaSAE += subitem.SubTotal;

                                break;
                        }
                        if (i == 0)
                        {

                            /*
                            if (NroGrupos == 0)
                                NroGrupos = 1;
                            */
                            if (NroGrupos > 0)
                            {
                                ws.Cells[row, colIniCentral].Value = item.CentralHead;
                                ws.Cells[row, colIniCentral, row + NroGrupos - 1, colIniCentral].Merge = true;
                                ws.Cells[row, colIniCentral, row + NroGrupos - 1, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                ws.Cells[row, colIniCentral, row + NroGrupos - 1, colIniCentral].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                i++;
                            }
                            else
                            {
                                log.Error("La central no cuenta con grupos relacionados => " + item.CentralHead);
                            }
                        }
                        if (subitem.tipoG <= 4)
                        {
                            ws.Cells[row, colIniUnidad].Value = subitem.Nombre;
                            ws.Cells[row, colIniUnidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            ws.Cells[row, colIniHidro].Value = subTotalH;
                            ws.Cells[row, colIniHidro].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            ws.Cells[row, colIniTermo].Value = subTotalT;
                            ws.Cells[row, colIniTermo].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            ws.Cells[row, colIniSolar].Value = subTotalS;
                            ws.Cells[row, colIniSolar].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            ws.Cells[row, colIniEolica].Value = subTotalE;
                            ws.Cells[row, colIniEolica].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            //ws.Cells[row, colIniSerAux].Value = subTotalSA;
                            ws.Cells[row, colIniSerAux].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                        if (i == 0 || subitem.tipoG <= 4)
                        {
                            row++;
                        }
                    }

                    //Total por central
                    ws.Cells[row, colIniCentral].Value = "Total " + item.CentralHead;
                    ws.Cells[row, colIniCentral, row, colIniUnidad].Merge = true;
                    ws.Cells[row, colIniCentral, row, colIniUnidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniHidro].Value = totalH;
                    ws.Cells[row, colIniHidro].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniTermo].Value = totalT;
                    ws.Cells[row, colIniTermo].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniSolar].Value = totalS;
                    ws.Cells[row, colIniSolar].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniEolica].Value = totalE;
                    ws.Cells[row, colIniEolica].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[row, colIniSerAux].Value = totalSA;
                    ws.Cells[row, colIniSerAux].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    using (var range = ws.Cells[row, colIniCentral, row, colIniSerAux])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DCE6F2"));
                    }
                    row++;
                }

                //Total por empresa
                ws.Cells[row, colIniCentral].Value = "TOTAL";
                ws.Cells[row, colIniCentral, row, colIniUnidad].Merge = true;
                ws.Cells[row, colIniCentral, row, colIniUnidad].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniHidro].Value = totalEmpresaH;
                ws.Cells[row, colIniHidro].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniTermo].Value = totalEmpresaT;
                ws.Cells[row, colIniTermo].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniSolar].Value = totalEmpresaS;
                ws.Cells[row, colIniSolar].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniEolica].Value = totalEmpresaE;
                ws.Cells[row, colIniEolica].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[row, colIniSerAux].Value = totalEmpresaSA;
                ws.Cells[row, colIniSerAux].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                using (var range = ws.Cells[row, colIniCentral, row, colIniSerAux])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                #endregion

                row += 2;
                #region cabecera resumen

                int rowIniTipoGenRes = row;
                int rowIniProduccionRes = rowIniTipoGenRes;
                int rowIniSerAuxRes = rowIniTipoGenRes;
                int rowIniProduccionNetaRes = rowIniTipoGenRes;

                int colIniTipoGenRes = 1;
                int colIniProduccionRes = colIniTipoGenRes + 1;
                int colIniSerAuxRes = colIniProduccionRes + 1;
                int colIniProduccionNetaRes = colIniSerAuxRes + 1;

                ws.Cells[rowIniTipoGenRes, colIniTipoGenRes].Value = "Tipo de Generación";
                ws.Cells[rowIniTipoGenRes, colIniTipoGenRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniProduccionRes, colIniProduccionRes].Value = "Producción de Energía Eléctrica (MWh)";
                ws.Cells[rowIniProduccionRes, colIniProduccionRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniSerAuxRes, colIniSerAuxRes].Value = "Servicios Auxiliares (MWh)";
                ws.Cells[rowIniSerAuxRes, colIniSerAuxRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniProduccionNetaRes, colIniProduccionNetaRes].Value = "Producción Neta (MWh)";
                ws.Cells[rowIniProduccionNetaRes, colIniProduccionNetaRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                using (var range = ws.Cells[rowIniTipoGenRes, colIniTipoGenRes, rowIniProduccionNetaRes, colIniProduccionNetaRes])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    range.Style.WrapText = true;
                }
                #endregion
                row = rowIniProduccionNetaRes + 1;
                #region cuerpo resumen

                ws.Cells[row, colIniTipoGenRes].Value = "HIDROELÉCTRICA";
                ws.Cells[row, colIniTipoGenRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionRes].Value = totalEmpresaH;
                ws.Cells[row, colIniProduccionRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniSerAuxRes].Value = totalEmpresaSAH;
                ws.Cells[row, colIniSerAuxRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionNetaRes].Value = Math.Abs(totalEmpresaH.GetValueOrDefault(0) - totalEmpresaSAH.GetValueOrDefault(0));
                ws.Cells[row, colIniProduccionNetaRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                row++;

                ws.Cells[row, colIniTipoGenRes].Value = "TERMOELÉCTRICA";
                ws.Cells[row, colIniTipoGenRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionRes].Value = totalEmpresaT;
                ws.Cells[row, colIniProduccionRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniSerAuxRes].Value = totalEmpresaSAT;
                ws.Cells[row, colIniSerAuxRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionNetaRes].Value = Math.Abs(totalEmpresaT.GetValueOrDefault(0) - totalEmpresaSAT.GetValueOrDefault(0));
                ws.Cells[row, colIniProduccionNetaRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                row++;

                ws.Cells[row, colIniTipoGenRes].Value = "SOLAR";
                ws.Cells[row, colIniTipoGenRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionRes].Value = totalEmpresaS;
                ws.Cells[row, colIniProduccionRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniSerAuxRes].Value = totalEmpresaSAS;
                ws.Cells[row, colIniSerAuxRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionNetaRes].Value = Math.Abs(totalEmpresaS.GetValueOrDefault(0) - totalEmpresaSAS.GetValueOrDefault(0));
                ws.Cells[row, colIniProduccionNetaRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                row++;

                ws.Cells[row, colIniTipoGenRes].Value = "EÓLICA";
                ws.Cells[row, colIniTipoGenRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionRes].Value = totalEmpresaE;
                ws.Cells[row, colIniProduccionRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniSerAuxRes].Value = totalEmpresaSAE;
                ws.Cells[row, colIniSerAuxRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionNetaRes].Value = Math.Abs(totalEmpresaE.GetValueOrDefault(0) - totalEmpresaSAE.GetValueOrDefault(0));
                ws.Cells[row, colIniProduccionNetaRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                row++;

                var sumaTotalesResumenEnergia = totalEmpresaH + totalEmpresaT + totalEmpresaS + totalEmpresaE;
                var sumaTotalesResumenSSAA = totalEmpresaSAH + totalEmpresaSAT + totalEmpresaSAS + totalEmpresaSAE;

                ws.Cells[row, colIniTipoGenRes].Value = "TOTAL";
                ws.Cells[row, colIniTipoGenRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionRes].Value = sumaTotalesResumenEnergia;
                ws.Cells[row, colIniProduccionRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniSerAuxRes].Value = sumaTotalesResumenSSAA;
                ws.Cells[row, colIniSerAuxRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[row, colIniProduccionNetaRes].Value = Math.Abs(sumaTotalesResumenEnergia.GetValueOrDefault(0) - sumaTotalesResumenSSAA.GetValueOrDefault(0));
                ws.Cells[row, colIniProduccionNetaRes].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                using (var range = ws.Cells[row, colIniTipoGenRes, row, colIniProduccionNetaRes])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                #endregion


                //ancho de columnas
                ws.Column(colIniCentral).Width = 25;
                ws.Column(colIniUnidad).Width = 25;
                ws.Column(colIniHidro).Width = 18;
                ws.Column(colIniTermo).Width = 18;
                ws.Column(colIniSolar).Width = 18;
                ws.Column(colIniEolica).Width = 18;
                ws.Column(colIniSerAux).Width = 18;

                ws.Row(rowIniTipoGenRes).Height = 30;

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }
            return fileExcel;
        }

        #endregion
    }
}
