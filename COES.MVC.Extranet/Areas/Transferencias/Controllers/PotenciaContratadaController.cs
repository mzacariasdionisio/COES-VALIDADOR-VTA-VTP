using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.Transferencias.Helper;
using COES.MVC.Extranet.Areas.Transferencias.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Framework.Base.Tools;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Transferencias.Controllers
{
    // ASSETEC 2019-11
    public class PotenciaContratadaController : BaseController
    {
        /// <summary>
        /// Instancia de las clases de aplicación
        /// </summary>   
        EmpresaAppServicio empresaAppServicio = new EmpresaAppServicio();
        PeriodoAppServicio periodoAppServicio = new PeriodoAppServicio();
        CodigoRetiroAppServicio codigoRetiroAppServicio = new CodigoRetiroAppServicio();
        TransferenciasAppServicio transferenciaAppServicio = new TransferenciasAppServicio();

        #region Manejador de Exepciones
        public PotenciaContratadaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        #endregion

        /// <summary>
        /// Carga el formulario de potencias contratadas
        /// </summary>         
        /// <returns></returns>
        // GET: Transferencias/PotenciaContratada        
        // Carga de Pagina de Potencias Contratadas
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            PotenciaContratadaModel model = new PotenciaContratadaModel();
            
            model.ListaPeriodos = periodoAppServicio.BuscarPeriodo("");
            //
            List<SeguridadServicio.EmpresaDTO> list = Funcion.ObtenerEmpresasPorUsuario(User.Identity.Name);
            model.ListaEmpresas = new List<EmpresaDTO>(); // empresaAppServicio.ListaInterCoReSoCli();
            foreach (var item in list)
            {
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(item.EMPRCODI);
                model.ListaEmpresas.Add(dtoEmpresa);
            }

            model.ListaEmpresas = model.ListaEmpresas.OrderBy(x => x.EmprNombre).ToList();
            return View(model);
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
            string PathLogo = @"Content\Images\logocoes.png";
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
            string pathFile = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
            string fileName = "";

            Log.Info("Generación del formato de Potencias Contratadas - DescargarExcelCodigosSolicitudRetiro");
            PotenciaContratadaModel model = new PotenciaContratadaModel();
            model.ListaPotenciasContratadas = transferenciaAppServicio.ListarPotenciasContratadas(emprcodi, pericodi);
            if (model.ListaPotenciasContratadas.Count == 0)
            {
                model.ListaCodigoRetiro = codigoRetiroAppServicio.ImportarPotenciasContratadas(pericodi, emprcodi);
                if (model.ListaCodigoRetiro.Count == 0)
                {
                    return Json("-1");
                }
                else
                {
                    Log.Info("Generación del formato de Potencias Contratadas - GenerarFormatoPotenciaContratada");
                    fileName = this.transferenciaAppServicio.GenerarFormatoPotenciaContratada(model.ListaCodigoRetiro, pericodi, emprcodi, 1, pathFile, pathLogo);
                }
            }
            else
            {
                Log.Info("Exporta las Potencias Contratadas - ExportarPotenciaContratada");
                fileName = this.transferenciaAppServicio.ExportarPotenciaContratada(model.ListaPotenciasContratadas, pericodi, emprcodi, 1, pathFile, pathLogo);
            }
            return Json(fileName);
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

        /// <summary>
        /// Sube los archivos a una carpeta
        /// </summary>
        /// <param name="sFecha">Cadena de Fecha</param>        
        /// <returns>Json</returns>
        [HttpPost]
        public ActionResult UploadArchivo(string fecha)
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = string.Empty;
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString(); 
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = fecha + "_" + file.FileName;
                    if (FileServer.VerificarExistenciaFile(null, sNombreArchivo, path))
                    {
                        FileServer.DeleteBlob(sNombreArchivo, path);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lista los archivos nuevos
        /// </summary>
        /// <param name="sFecha">Cadena de la fecha</param>
        /// <param name="sFileName">Cadena del nombre del archivo</param> 
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult MostrarArchivo(string sFecha, string sFileName)
        {
            base.ValidarSesionJsonResult();

            PotenciaContratadaModel model = new PotenciaContratadaModel(); 
            string fileName = sFecha + "_" + sFileName;
            model.ListaDocumentos = FileServer.ListarArhivos(null, ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString());
            model.ListaDocumentosFiltrado = new List<FileData>();

            foreach (var item in model.ListaDocumentos)
            {
                if (String.Equals(item.FileName, fileName))
                {
                    model.ListaDocumentosFiltrado.Add(item);
                }
            }
            return Json(model);
        }

        /// <summary>
        /// Metodo para eliminar los archivos importados
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <returns>Entero</returns>
        [HttpPost]
        public int EliminarArchivos(string nombreArchivo)
        {
            base.ValidarSesionUsuario();
            string pathBase = base.PathFiles;
            string nombreFile = string.Empty;

            PotenciaContratadaModel model = new PotenciaContratadaModel(); //ArchivosModel modelArchivos = new ArchivosModel();
            model.ListaDocumentos = FileServer.ListarArhivos(null, ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString());
            foreach (var item in model.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombreFile = item.FileName;
                    break;
                }
            }
            if (FileServer.VerificarExistenciaFile(null, nombreFile, ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString()))
            {
                FileServer.DeleteBlob(nombreFile, ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString());
            }
            return -1;
        }

        /// <summary>
        /// Muestra en Excel Web las potencias contratadas a partir del upload archivo excel 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="emprcodi">id de la empresa generadora</param>
        /// <param name="sfile">Archivo excel que se desea procesar</param>
        /// <returns></returns>       
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult MostrarExcelWeb(int pericodi, int emprcodi, string sfile)
        {
            int rspta = -1;
            try
            {
                base.ValidarSesionJsonResult();
                PotenciaContratadaModel model = new PotenciaContratadaModel();
                model.ListaPotenciasContratadas = new List<TrnPotenciaContratadaDTO>();
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                model.ListaPotenciasContratadas = transferenciaAppServicio.LeerArchivoModificadoPotenciasContratadas(emprcodi, pericodi, User.Identity.Name, path, sfile);
                if (model.ListaPotenciasContratadas.Count > 0)
                {
                    GridExcelModel modelGrillaExcel = ConfigurarExcelWeb(pericodi, emprcodi, model);
                    return Json(modelGrillaExcel);
                }
                Json(rspta);
            }
            catch (Exception e)
            {
                string smsg = e.Message;
            }
            return Json(rspta);
        }

        public GridExcelModel ConfigurarExcelWeb(int pericodi, int emprcodi, PotenciaContratadaModel model)
        {
            // Armando la grilla Excel
            GridExcelModel modelGrillaExcel = new GridExcelModel();
            modelGrillaExcel.MensajeError = "";
            modelGrillaExcel.NumRegistros = 0;
            string[] Cabeceras1 = { "Datos de los Contratos Vigentes", "", "", "", "", "", "", "Vigencia del Contrato", "", "Punto(s) de Suministro (3)", "Datos de la Potencia Contratada Fija [MW]", "", "", "Datos de la Potencia Contratada  Variable [MW]", "", "", "Comentario / Observación" };
            string[] Cabeceras2 = { "Id", "Código de Retiro", "Suministrador", "Cliente (1)", "Tipo de Contrato", "Tipo de Usuario", "Barra de Transferencia (2)", "Fecha Inicio", "Fecha Fin", "", "Total [MW]", "H.P. [MW]", "H.F.P. [MW]", "Total [MW]", "H.P. [MW]", "H.F.P. [MW]", "" };

            // Ancho de cada columna
            int[] widths = { 30, 150, 150, 150, 100, 100, 150, 100, 100, 150, 100, 100, 100, 100, 100, 100, 200 };

            modelGrillaExcel.EntidadPeriodo = periodoAppServicio.GetByIdPeriodo(pericodi);
            modelGrillaExcel.bGrabar = true;
            if (modelGrillaExcel.EntidadPeriodo.PeriEstado.Equals("Cerrado"))
            { modelGrillaExcel.bGrabar = false; }
            // --------------------------------------------------------------------------------------------------------
            // Se arma la matriz de datos para el HandsonTable con la lista de codigos original
            // --------------------------------------------------------------------------------------------------------
            model.ListaCodigoRetiro = codigoRetiroAppServicio.ImportarPotenciasContratadas(pericodi, emprcodi);

            string[][] data;
            if (model.ListaCodigoRetiro.Count() != 0)
            {
                data = new string[model.ListaCodigoRetiro.Count() + 2][];

                // Asigna la cabeceras a la matriz
                data[0] = Cabeceras1;
                data[1] = Cabeceras2;

                // Asigna la data de la lista a la matriz
                int index = 2;
                foreach (CodigoRetiroDTO item in model.ListaCodigoRetiro)
                {
                    int iSoliCodiRetiCodi = item.SoliCodiRetiCodi;
                    string sTrnPctPtoSumins = "";
                    string sTrnPctTotalMwFija = "";
                    string sTrnPctHpMwFija = "";
                    string sTrnPctHfpMwFija = "";
                    string sTrnPctTotalMwVariable = "";
                    string sTrnPctHpMwFijaVariable = "";
                    string sTrnPctHfpMwFijaVariable = "";
                    string sTrnPctComeObs = "";
                    bool sExisteCodigo = false;
                    foreach (TrnPotenciaContratadaDTO dto in model.ListaPotenciasContratadas)
                    {
                        if (dto.CoresoCodi == iSoliCodiRetiCodi)
                        {
                            sTrnPctPtoSumins = dto.TrnPctPtoSumins;
                            sTrnPctTotalMwFija = dto.TrnPctTotalMwFija.ToString();
                            sTrnPctHpMwFija = dto.TrnPctHpMwFija.ToString();
                            sTrnPctHfpMwFija = dto.TrnPctHfpMwFija.ToString();
                            sTrnPctTotalMwVariable = dto.TrnPctTotalMwVariable.ToString();
                            sTrnPctHpMwFijaVariable = dto.TrnPctHpMwFijaVariable.ToString();
                            sTrnPctHfpMwFijaVariable = dto.TrnPctHfpMwFijaVariable.ToString();
                            sTrnPctComeObs = dto.TrnPctComeObs;
                            sExisteCodigo = true;
                            break;
                        }
                    }
                    string[] itemDato = {
                                              (index-1).ToString(),
                                              item.SoliCodiRetiCodigo.ToString(),
                                              item.EmprNombre.ToString(),
                                              item.CliNombre.ToString(),
                                              item.TipoContNombre.ToString(),
                                              item.TipoUsuaNombre.ToString(),
                                              item.BarrNombBarrTran.ToString(),
                                              item.SoliCodiRetiFechaInicio.GetValueOrDefault().ToString("dd/MM/yyyy"),
                                              item.SoliCodiRetiFechaFin.GetValueOrDefault().ToString("dd/MM/yyyy"),
                                              sTrnPctPtoSumins,
                                              sTrnPctTotalMwFija,
                                              sTrnPctHpMwFija,
                                              sTrnPctHfpMwFija,
                                              sTrnPctTotalMwVariable,
                                              sTrnPctHpMwFijaVariable,
                                              sTrnPctHfpMwFijaVariable,
                                              sTrnPctComeObs
                                        };
                    data[index] = itemDato;
                    modelGrillaExcel.NumRegistros += 1;
                    if (!sExisteCodigo)
                        modelGrillaExcel.MensajeError += item.SoliCodiRetiCodigo.ToString() + ", "; 
                    index++;
                }
            }
            else
            {
                data = new string[3][];
                data[0] = Cabeceras1;
                data[1] = Cabeceras2;
                int index = 2;
                string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                data[index] = itemDato;
            }
            // --------------------------------------------------------------------------------------------------------
            #region Columnas
            // Armamos las columnas del HansondTable
            object[] columnas = new object[17];
            columnas[0] = new
            {
                // Id                
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true
            };
            columnas[1] = new
            {
                // Código de Retiro
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true

            };
            columnas[2] = new
            {
                // Suministrador
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true

            };
            columnas[3] = new
            {
                // Cliente (1)
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true

            };
            columnas[4] = new
            {
                // Tipo de Contrato
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true

            };
            columnas[5] = new
            {
                // Tipo de Usuario
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true

            };
            columnas[6] = new
            {
                // Barra de Transferencia (2)
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true
            };

            columnas[7] = new
            {
                // Fecha Inicio
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true
            };
            columnas[8] = new
            {
                // Fecha Fin
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true
            };
            columnas[9] = new
            {
                // Punto(s) de Suministro (3)
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false
            };
            columnas[10] = new
            {
                // Total [MW]
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = false
            };
            columnas[11] = new
            {
                // H.P. [MW]
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = false
            };
            columnas[12] = new
            {
                // H.F.P. [MW]
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = false
            };
            columnas[13] = new
            {
                // Total [MW]
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = false
            };
            columnas[14] = new
            {
                // H.P. [MW]
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = false
            };
            columnas[15] = new
            {
                // H.F.P. [MW]
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.0000",
                readOnly = false
            };
            columnas[16] = new
            {
                // Comentario / Observación
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false
            };
            #endregion
            modelGrillaExcel.Data = data;
            modelGrillaExcel.Widths = widths;
            modelGrillaExcel.Columnas = columnas;
            modelGrillaExcel.FixedRowsTop = 2;
            modelGrillaExcel.FixedColumnsLeft = 2;
            return modelGrillaExcel;
        }

        /// <summary>
        /// Muestra la grilla excel para las potencias contratadas
        /// </summary>
        /// <param name="idEmpresa">Código de la empresa generadora</param>
        /// <param name="idPeriodo">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelConsultar(int idEmpresa, int idPeriodo)
        {
            base.ValidarSesionJsonResult();

            PotenciaContratadaModel modelPotenciaContratada = new PotenciaContratadaModel();
            modelPotenciaContratada.ListaPotenciasContratadas = transferenciaAppServicio.ListarPotenciasContratadas(idEmpresa, idPeriodo);

            GridExcelModel modelGrillaExcel = ConfigurarExcelWeb(idPeriodo, idEmpresa, modelPotenciaContratada);
            return Json(modelGrillaExcel);
        }

        /// <summary>
        /// Procesa las potencias contratadas a partir del upload y lectura de un archivo excel 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="emprcodi">id de la empresa generadora</param>
        /// <param name="sfile">Archivo excel que se desea procesar</param>
        /// <returns></returns>       
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult ProcesarPotenciaContratada(int pericodi, int emprcodi, List<string[]> datos)
        {
            int rspta = -1;
            try {
                base.ValidarSesionJsonResult();
                PotenciaContratadaModel model = new PotenciaContratadaModel();
                model.ListaPotenciasContratadas = new List<TrnPotenciaContratadaDTO>();
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                model.ListaPotenciasContratadas = transferenciaAppServicio.LeerHojaCalculoWebPotenciasContratadas(emprcodi, pericodi, User.Identity.Name, datos);
                if (model.ListaPotenciasContratadas.Count > 0)
                {
                    //Eliminamos información previa que pueda existir
                    transferenciaAppServicio.DeleteTrnPotenciaContratada(pericodi, emprcodi);
                    rspta = transferenciaAppServicio.SavePotenciasContratadas(model.ListaPotenciasContratadas, User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                string smsg = e.Message;
                return Json(smsg);
            }
            return Json(rspta);
        }
   }
}