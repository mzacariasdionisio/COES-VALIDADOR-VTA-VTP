using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    // ASSETEC 2019-11
    public class PotenciaContratadaController : BaseController
    {
        EmpresaAppServicio empresaAppServicio = new EmpresaAppServicio();
        PeriodoAppServicio periodoAppServicio = new PeriodoAppServicio();
        CodigoRetiroAppServicio codigoRetiroAppServicio = new CodigoRetiroAppServicio();
        TransferenciasAppServicio transferenciaAppServicio = new TransferenciasAppServicio();
        BarraAppServicio barraAppServicio = new BarraAppServicio();
        #region Manejador de Exepciones
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

        // GET: Transferencias/PotenciaContratada        
        // Consulta de Potencias Contratadas
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            PotenciaContratadaModel model = new PotenciaContratadaModel();
            model.ListaEmpresas = empresaAppServicio.ListaInterCoReSoGen();
            model.ListaClientes = empresaAppServicio.ListaInterCoReSoCli();
            model.ListaBarrasTransferencia = barraAppServicio.ListaInterCoReSo();
            model.ListaPeriodos = periodoAppServicio.BuscarPeriodo("");
            return View(model);
        }

        /// <summary>
        /// Muestra la grilla excel para las potencias contratadas
        /// </summary>
        /// <param name="idEmpresa">Código de la empresa generadora</param>
        /// <param name="idPeriodo">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelConsultar(int idEmpresa, int idPeriodo, int idCliente, int idBarra)
        {
            base.ValidarSesionJsonResult();
            GrillaExcelModel modelGrillaExcel = new GrillaExcelModel();
            PotenciaContratadaModel modelPotenciaContratada = new PotenciaContratadaModel();

            try
            {
                #region Armando de contenido
                string[] Cabeceras1 = { "Datos de los Contratos Vigentes", "", "", "", "", "", "", "Vigencia del Contrato", "", "Punto(s) de Suministro (3)", "Datos de la Potencia Contratada Fija [MW]", "", "", "Datos de la Potencia Contratada  Variable [MW]", "", "", "Comentario / Observación", "User", "FechaRegistro" };
                string[] Cabeceras2 = { "Id", "Código de Retiro", "Suministrador", "Cliente (1)", "Tipo de Contrato", "Tipo de Usuario", "Barra de Transferencia (2)", "Fecha Inicio", "Fecha Fin", "", "Total [MW]", "H.P. [MW]", "H.F.P. [MW]", "Total [MW]", "H.P. [MW]", "H.F.P. [MW]", "", "", "" };

                // Ancho de cada columna
                int[] widths = { 30, 150, 150, 150, 100, 100, 150, 100, 100, 150, 100, 100, 100, 100, 100, 100, 200, 100, 150 };

                // --------------------------------------------------------------------------------------------------------
                // Se arma la matriz de datos para el HandsonTable
                // --------------------------------------------------------------------------------------------------------
                modelGrillaExcel.NumRegistros = 0;
                modelPotenciaContratada.ListaPotenciasContratadas = transferenciaAppServicio.ListarPotenciasContratadas(idEmpresa, idPeriodo, idCliente, idBarra);

                string[][] data;
                if (modelPotenciaContratada.ListaPotenciasContratadas.Count() != 0)
                {
                    modelGrillaExcel.NumRegistros = modelPotenciaContratada.ListaPotenciasContratadas.Count();
                    data = new string[modelPotenciaContratada.ListaPotenciasContratadas.Count() + 2][];

                    // Asigna la cabeceras a la matriz
                    data[0] = Cabeceras1;
                    data[1] = Cabeceras2;

                    // Asigna la data de la lista a la matriz
                    int index = 2;
                    foreach (TrnPotenciaContratadaDTO item in modelPotenciaContratada.ListaPotenciasContratadas)
                    {
                        string[] itemDato = {
                                              item.TrnPctCodi.ToString(),
                                              item.CoresoCodigo.ToString(),
                                              item.EmprNomb.ToString(),
                                              item.CliNombre.ToString(),
                                              item.TipConNombre.ToString(),
                                              item.TipUsuNombre.ToString(),
                                              item.BarrBarraTransferencia.ToString(),
                                              item.CoresoFechaInicio.ToString("dd/MM/yyyy"),
                                              item.CoresoFechaFin.ToString("dd/MM/yyyy"),
                                              (!string.IsNullOrEmpty(item.TrnPctPtoSumins)) ? item.TrnPctPtoSumins : string.Empty,
                                              item.TrnPctTotalMwFija.ToString(),
                                              item.TrnPctHpMwFija.ToString(),
                                              item.TrnPctHfpMwFija.ToString(),
                                              item.TrnPctTotalMwVariable.ToString(),
                                              item.TrnPctHpMwFijaVariable.ToString(),
                                              item.TrnPctHfpMwFijaVariable.ToString(),
                                              (!string.IsNullOrEmpty(item.TrnPctComeObs)) ? item.TrnPctComeObs : string.Empty,
                                              item.TrnPctUserNameIns.ToString(),
                                              item.TrnPctFecIns.ToString()
                                        };

                        data[index] = itemDato;

                        index++;
                    }
                }
                else
                {
                    data = new string[3][];
                    data[0] = Cabeceras1;
                    data[1] = Cabeceras2;
                    int index = 2;
                    string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                    data[index] = itemDato;
                }
                // --------------------------------------------------------------------------------------------------------

                // Armamos las columnas del HansondTable
                object[] columnas = new object[19];
                columnas[0] = new
                {
                    // Id                
                    type = GrillaExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true
                };
                columnas[1] = new
                {
                    // Código de Retiro
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true

                };
                columnas[2] = new
                {
                    // Suministrador
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true

                };
                columnas[3] = new
                {
                    // Cliente (1)
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true

                };
                columnas[4] = new
                {
                    // Tipo de Contrato
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true

                };
                columnas[5] = new
                {
                    // Tipo de Usuario
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true

                };
                columnas[6] = new
                {
                    // Barra de Transferencia (2)
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true
                };

                columnas[7] = new
                {
                    // Fecha Inicio
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true
                };
                columnas[8] = new
                {
                    // Fecha Fin
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true
                };
                columnas[9] = new
                {
                    // Punto(s) de Suministro (3)
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true
                };
                columnas[10] = new
                {
                    // Total [MW]
                    type = GrillaExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = true
                };
                columnas[11] = new
                {
                    // H.P. [MW]
                    type = GrillaExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = true
                };
                columnas[12] = new
                {
                    // H.F.P. [MW]
                    type = GrillaExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = true
                };
                columnas[13] = new
                {
                    // Total [MW]
                    type = GrillaExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = true
                };
                columnas[14] = new
                {
                    // H.P. [MW]
                    type = GrillaExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = true
                };
                columnas[15] = new
                {
                    // H.F.P. [MW]
                    type = GrillaExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = true
                };
                columnas[16] = new
                {
                    // Comentario / Observación
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true
                };
                columnas[17] = new
                {
                    // User
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true
                };
                columnas[18] = new
                {
                    // fecha de inserción
                    type = GrillaExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = true
                };
                modelGrillaExcel.Data = data;
                modelGrillaExcel.Widths = widths;
                modelGrillaExcel.Columnas = columnas;
                modelGrillaExcel.FixedRowsTop = 2;
                modelGrillaExcel.FixedColumnsLeft = 2;
                #endregion

            }
            catch (Exception e)
            {
                modelGrillaExcel.MensajeError = e.Message;
            }
            return Json(modelGrillaExcel);
        }

        /// <summary>
        /// Descarga el archivo excel de potencias contratadas
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="emprcodi">Código de la empresa generadora</param>        
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult DescargarPotenciaContratada(int pericodi, int emprcodi)
        {
            base.ValidarSesionJsonResult();

            Log.Info("Generación del formato de Potencias Contratadas - DescargarExcelCodigosSolicitudRetiro");
            PotenciaContratadaModel model = new PotenciaContratadaModel();
            model.ListaPotenciasContratadas = transferenciaAppServicio.ListarPotenciasContratadas(emprcodi, pericodi);
            if (model.ListaPotenciasContratadas.Count == 0)
            {
                return Json("-1");
            }
            else
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                string fileName = "";
                Log.Info("Generación del formato de Potencias Contratadas - GenerarFormatoPotenciaContratada");
                fileName = this.transferenciaAppServicio.ExportarPotenciaContratada(model.ListaPotenciasContratadas, pericodi, emprcodi, 1, pathFile, pathLogo);
                return Json(fileName);
            }
        }

        /// <summary>
        /// Permite descargar el archivo al explorador
        /// </summary>
        /// <param name="file">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        public virtual ActionResult AbrirArchivo(string file)
        {
            base.ValidarSesionUsuario();

            int formato = 1;

            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + file;

            string app = (formato == 1) ? Funcion.AppExcel : (formato == 2) ? Funcion.AppPdf : Funcion.AppWord;

            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, app, sFecha + "_" + file);
        }
    }
}