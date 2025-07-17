using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.RechazoCarga;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Helper;
using FormatoHelper = COES.MVC.Intranet.Areas.RechazoCarga.Helper.FormatoHelper;
using log4net;
using COES.Servicios.Aplicacion.DemandaMaxima;
using System.Globalization;
using COES.MVC.Intranet.Areas.IntercambioOsinergmin.Helper;


namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class ProgramaRechazoCargaController : BaseController
    {
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        
        DemandaMaximaAppServicio servicioDemandaMaxima = new DemandaMaximaAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ProgramaRechazoCargaController));
        private const string _estadoRegistroEmpresaActivo = "A";
        private const int _tipoEmpresaDistribuidor = 2;

        #region propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreArchivo
        {
            get
            {
                return (Session[DatosSesionRechazoCarga.SesionNombreArchivo] != null) ?
                    Session[DatosSesionRechazoCarga.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionRechazoCarga.SesionNombreArchivo] = value; }
        }


        /// <summary>
        /// Ruta y nombre del archivo
        /// </summary>
        public String RutaCompletaArchivo
        {
            get
            {
                return (Session[DatosSesionRechazoCarga.RutaCompletaArchivo] != null) ?
                    Session[DatosSesionRechazoCarga.RutaCompletaArchivo].ToString() : null;
            }
            set { Session[DatosSesionRechazoCarga.RutaCompletaArchivo] = value; }
        }

        #endregion

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ProgramaRechazoCargaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ProgramaRechazoCargaController", ex);
                throw;
            }
        }
        public ProgramaRechazoCargaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public ViewResult Inicio()
        {
            base.ValidarSesionUsuario();
            ProgramaRechazoCargaModel model = new ProgramaRechazoCargaModel();

            var codigoPrograma = String.IsNullOrEmpty(Request["codigoPrograma"]) ? 0 : Convert.ToInt32(Request["codigoPrograma"]);
            var codigoCuadroPrograma = String.IsNullOrEmpty(Request["codigoCuadroPrograma"]) ? 0 : Convert.ToInt32(Request["codigoCuadroPrograma"]);
            var perfil = 1; //Por defecto Permiso es SPR
            bool permisoSEV = false;
            if (codigoPrograma > 0)
            {
                model.RcaProgramaDTO = servicio.GetByIdRcaPrograma(codigoPrograma);
            }
            else
            {
                model.RcaProgramaDTO = new RcaProgramaDTO();
                model.RcaProgramaDTO.Rcprogcodi = 0;
                model.RcaProgramaDTO.Rchorpcodi = 0;
                model.RcaProgramaDTO.Rcprogabrev = String.Empty;
                model.RcaProgramaDTO.Rcprognombre = String.Empty;
            }
            if (codigoCuadroPrograma > 0)
            {
                model.RcaCuadroProgDTO = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);
                //bool AccesoAdicional = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Adicional);                
               
                //if (AccesoAdicional)//Usuario SCO
                //{
                //    model.esConsulta = model.RcaCuadroProgDTO.Rcestacodi.Equals(ConstantesRechazoCarga.EstadoCuadroReprogramado) ? false : true;
                //}
                //else
                //{
                //    model.esConsulta = model.RcaCuadroProgDTO.Rcestacodi.Equals(ConstantesRechazoCarga.EstadoCuadroProgramado) ? false : true;
                //}                
            }
            else
            {
                model.RcaCuadroProgDTO = new RcaCuadroProgDTO();
                model.RcaCuadroProgDTO.Rcconpcodi = 0;
                model.RcaCuadroProgDTO.Rccuadenergiaracionar = Decimal.Zero;
                model.RcaCuadroProgDTO.Rccuadumbral = Decimal.Zero;
                model.RcaCuadroProgDTO.Rccuadbloquehor = String.Empty;
                model.RcaCuadroProgDTO.Rccuadmotivo = String.Empty;
                model.RcaCuadroProgDTO.Rccuadflageracmf = String.Empty;
                model.RcaCuadroProgDTO.Rccuadflageracmt = String.Empty;
                model.RcaCuadroProgDTO.Rccuadflagregulado = String.Empty;
                model.RcaCuadroProgDTO.Rccuadfechorinicio = DateTime.Now;
                model.RcaCuadroProgDTO.Rccuadfechorfin = DateTime.Now;
                model.RcaCuadroProgDTO.Rccuadbloquehor = "HP";
                model.RcaCuadroProgDTO.Rcestacodi = 0;
                model.esConsulta = false;
            }

            //Revision de Permisos
            bool permisoSCO = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.PermisoSCO);
            permisoSEV = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.PermisoSEV);
            //model.bAdicional = AccesoAdicional;

            if (permisoSEV)//Usuario SEV
            {
                model.esConsulta = true;
            }
            else if (permisoSCO)
            {
                model.esConsulta = model.RcaCuadroProgDTO.Rcestacodi.Equals(ConstantesRechazoCarga.EstadoCuadroNoEjecutado)
                    || model.RcaCuadroProgDTO.Rcestacodi.Equals(ConstantesRechazoCarga.EstadoCuadroEjecutado) ? true : false;
            }
            else
            {
                if(codigoCuadroPrograma > 0)
                {
                    model.esConsulta = model.RcaCuadroProgDTO.Rcestacodi.Equals(ConstantesRechazoCarga.EstadoCuadroProgramado) ? false : true;
                }
                
            }

            if (permisoSCO)
            {
                perfil = 2;
            }
            else if (permisoSEV)
            {
                perfil = 3;
            }

            model.Perfil = perfil;
            model.Horizontes = servicio.ListHorizonteProg();
            model.Configuraciones = servicio.ListConfiguracionProg();            

            List<RcaProgramaDTO> programas = new List<RcaProgramaDTO>();
            programas.Add(new RcaProgramaDTO { Rcprogcodi = 0, Rcprognombre = "-- SELECCIONE --" });
            var listaProgramas = servicio.ListProgramasRechazoCarga(false);
            if (listaProgramas.Any())
            {
                programas.AddRange(listaProgramas);
            }

            if (programas.Where(p => p.Rcprogcodi.Equals(codigoPrograma)).Count().Equals(0))
            {
                programas.Add(model.RcaProgramaDTO);
            }
            model.Programas = programas;

            var listaZonas = servicio.ListZonas(ConstantesRechazoCarga.CodigoAreaNivel);
            var zonas = new List<AreaDTO>();
            zonas.Add(new AreaDTO { AREACODI = 0, AREANOMB = "-- TODOS --" });
            if (listaZonas.Any())
            {
                zonas.AddRange(listaZonas);
            }
            model.Zonas = zonas;

            var subEstaciones = new List<AreaDTO>();
            var listaSubEstaciones = servicio.ListSubEstacion(0);
            if (listaSubEstaciones.Any())
            {
                subEstaciones.AddRange(listaSubEstaciones);
            }
            model.SubEstaciones = subEstaciones;

            var antiguedadDatosMeses = this.servicio.ListAntiguedadDatosDemandaUsuario();

            ViewBag.hdnCodigoPrograma = codigoPrograma;
            ViewBag.hdnCodigoCuadroPrograma = codigoCuadroPrograma;
            ViewBag.hdnEsNuevoPrograma = codigoPrograma > 0 ? "0" : "1";
            ViewBag.hdnConfiguracionRadial = 1;
            ViewBag.hdnFlagConsulta = model.esConsulta ? "1" : "0";
            ViewBag.hdnPeriodosAntiguedad = antiguedadDatosMeses;
            ViewBag.hdnTieneEvento = !string.IsNullOrEmpty(model.RcaCuadroProgDTO.Rccuadcodeventoctaf) ? model.RcaCuadroProgDTO.Rccuadcodeventoctaf : "";
            ViewBag.hdnVerReportes = permisoSEV ? 1 : 0;
            ViewBag.hdnTotalSubestaciones = subEstaciones.Count;

            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            model.bEditar = AccesoEditar;
            //model.bEditar = true;

            if(model.Perfil == ConstantesRechazoCarga.PerfilSPR && !model.esConsulta)//Usuario SPR
            {
                if (codigoCuadroPrograma == 0 || model.RcaCuadroProgDTO.Rcestacodi==ConstantesRechazoCarga.EstadoCuadroProgramado)
                {
                    model.Programas = model.Programas.Where(p => p.Rchorpcodi != ConstantesRechazoCarga.HorizonteReprograma).ToList();
                }
            }

            model.editarDemanda = false;
            //Validacion Editar Demanda
            if (!model.esConsulta)
            {
                if(model.Perfil == ConstantesRechazoCarga.PerfilSPR)
                {
                    if (codigoCuadroPrograma == 0 || model.RcaCuadroProgDTO.Rcestacodi == ConstantesRechazoCarga.EstadoCuadroProgramado)
                    {
                        model.editarDemanda = true;
                    }
                }

                if(codigoCuadroPrograma == 0 || (model.Perfil == ConstantesRechazoCarga.PerfilSCO && model.RcaCuadroProgDTO.Rcestacodi == ConstantesRechazoCarga.EstadoCuadroReprogramado))
                {
                    model.editarDemanda = true;
                }

            }
            ViewBag.hdnFlagEditarDemanda = model.editarDemanda ? "1" : "0";

            return View(model);
        }

        public JsonResult ObtenerPrograma(int codigoPrograma)
        {
            var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            return Json(programa);
        }

        public JsonResult ObtenerCuadroPrograma(int codigoCuadroPrograma)
        {
            var cuadroPrograma = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);
            return Json(cuadroPrograma);
        }

        [HttpPost]
        public JsonResult ObtenerFormatoModelProgramaRechazoCarga(string datosIniciales, string fechaInicio, string bloqueHorario, string empresasSeleccionadas,
            string codigoCuadroPrograma, bool esConsulta, int periodoAntiguedad, int tipoBusqueda)
        {
            FormatoModel model = FormatearModeloDesdeParametros(datosIniciales, fechaInicio, bloqueHorario, empresasSeleccionadas, codigoCuadroPrograma, esConsulta, periodoAntiguedad, tipoBusqueda);
            return Json(model);
        }

        [HttpPost]
        public JsonResult GenerarFormato(string bloqueHorario, string datos, string codigoCuadroPrograma, bool esConsulta)
        {
            int indicador = 0;
            try
            {
                FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                GenerarArchivoExcel(model, 4);
                indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarFormato", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + NombreArchivoRechazoCarga.ProgramaRechazoCarga;
            return File(fullPath, Constantes.AppExcel, NombreArchivoRechazoCarga.ProgramaRechazoCarga);
        }

        /// <summary>
        /// Carga archivo excel con un nombre temporal en la ruta configurada en el Archivo de Configuración
        /// </summary>
        /// <returns></returns>
        public ActionResult Subir()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var archivo = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.NombreArchivo = fileRandom + "." + NombreArchivoRechazoCarga.ExtensionFileUploadRechazoCarga;
                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.NombreArchivo;
                    this.RutaCompletaArchivo = ruta;
                    archivo.SaveAs(ruta);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Subir", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LeerExcelSubido(string bloqueHorario)
        {
            try
            {
                var titulos = ObtenerTitulosColumnas(bloqueHorario);
                Respuesta matrizValida;
                var matrizDatos = FormatoHelper.LeerExcelCargadoCuadroProg(this.RutaCompletaArchivo, titulos, 2, out matrizValida);
                if (matrizValida.Exito)
                {
                    FormatoModel model = FormatearModeloDesdeMatriz(matrizDatos);
                    FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true, Datos = model });
                }
                else
                {
                    return Json(matrizValida);
                }
            }
            catch (Exception ex)
            {
                log.Error("LeerExcelSubido", ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Genera el documento excel en la ruta configurada en el Archivo de configuración 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="columnasOcultas"></param>
        private void GenerarArchivoExcel(FormatoModel model, int columnasOcultas)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();
            FileInfo newFile = new FileInfo(ruta + NombreArchivoRechazoCarga.ProgramaRechazoCarga);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivoRechazoCarga.ProgramaRechazoCarga);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                var filas = model.Handson.ListaFilaReadOnly.Count;
                var columnas = model.ColumnasCabecera + columnasOcultas;

                var sumaDemandaRacionar = decimal.Zero;                
                var racionamiento = decimal.Zero;
                var sumaPostRacionamiento = decimal.Zero;
                for (var i = 0; i < filas; i++)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        decimal valor = 0;
                        bool esDecimal = decimal.TryParse(model.Handson.ListaExcelData[i][j], out valor);
                        if (esDecimal)
                        {
                            ws.Cells[i + 1, j + 1].Value = valor;                            
                        }
                        else
                        {
                            ws.Cells[i + 1, j + 1].Value = model.Handson.ListaExcelData[i][j] ?? string.Empty;
                        }

                        ws.Cells[i + 1, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        if (i == 1 || (i == 0 && j > 6))
                        {
                            ws.Cells[i + 1, j + 1].Style.WrapText = true;
                            ws.Cells[i + 1, j + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[i + 1, j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[i + 1, j + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkSlateBlue);
                            ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[i + 1, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        if (i > 1)
                        {
                            ws.Cells[i + 1, j + 1].Style.WrapText = false;
                            ws.Cells[i + 1, j + 1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            ws.Cells[i + 1, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            if (j <= 4)
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }

                            if(j== 13 || j==14 || j==16 || j == 17)
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[i + 1, j + 1].Style.Numberformat.Format = "HH:mm";
                                continue;
                            }

                            if (j >= 5)
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[i + 1, j + 1].Style.Numberformat.Format = j == 9 ? @"0.0000" : @"0.00";

                                switch (j)
                                {
                                    case 7: sumaDemandaRacionar = sumaDemandaRacionar + valor; break;                                    
                                    case 8: racionamiento = racionamiento + valor; break;
                                    case 10: sumaPostRacionamiento = sumaPostRacionamiento + valor; break;
                                }
                            }
                        }
                    }
                }

                ws.Column(1).Width = 50;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 40;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Column(11).Width = 15;
                ws.Column(12).Width = 15;
                ws.Column(13).Width = 20;

                ws.Column(19).Hidden = true;
                ws.Column(20).Hidden = true;
                ws.Column(21).Hidden = true;
                ws.Column(22).Hidden = true;

                ws.Cells[filas + 1, 6].Value = "Resumen";
                ws.Cells[filas + 1, 6].Style.WrapText = true;
                ws.Cells[filas + 1, 6].Style.Font.Color.SetColor(System.Drawing.Color.White);
                //ws.Cells[filas + 1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells[filas + 2, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkSlateBlue);
                ws.Cells[filas + 1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[filas + 1, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;               

                ws.Cells[filas + 1, 8].Value = sumaDemandaRacionar;
                ws.Cells[filas + 1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[filas + 1, 8].Style.Numberformat.Format =  @"0.00";
                ws.Cells[filas + 1, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                //ws.Cells[filas + 1, 8].Value = factor;
                //ws.Cells[filas + 1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //ws.Cells[filas + 1, 8].Style.Numberformat.Format = @"0.0000";

                ws.Cells[filas + 1, 9].Value = racionamiento;
                ws.Cells[filas + 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[filas + 1, 9].Style.Numberformat.Format = @"0.00";
                ws.Cells[filas + 1, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[filas + 1, 11].Value = sumaPostRacionamiento;
                ws.Cells[filas + 1, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[filas + 1, 11].Style.Numberformat.Format = @"0.00";
                ws.Cells[filas + 1, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                foreach (var reg in model.Handson.ListaMerge)
                {
                    ws.Cells[reg.row + 1, reg.col + 1, reg.row + reg.rowspan, reg.col + reg.colspan].Merge = true;
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Busca los registros de programa usuario usando parametros de entrada 
        /// y devuelve el modelo para graficar los datos en Handsontable
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="esConsulta"></param>
        /// <returns></returns>
        private FormatoModel FormatearModeloDesdeParametros(string datosIniciales, string fechaInicio, string bloqueHorario, string empresasSeleccionadas,
            string codigoCuadroPrograma, bool esConsulta, int periodoAntiguedad, int tipoBusqueda)
        {
            FormatoModel model = new FormatoModel();

            var listaEmpresas = String.Empty;
            var listaEquipos = String.Empty;
            var identificadores = new List<IdentificadoresModel>();

            var clientesSelecionados = new List<RcaCuadroProgUsuarioDTO>();
            if (empresasSeleccionadas.Length > 0)
            {
                empresasSeleccionadas = empresasSeleccionadas.TrimEnd(';');

                foreach (var empresa in empresasSeleccionadas.Split(';'))
                {
                    var cliente = empresa.Split('/');

                    var clienteSeleccionado = new RcaCuadroProgUsuarioDTO();
                    clienteSeleccionado.Emprcodi = Convert.ToInt32(cliente[0]);
                    clienteSeleccionado.Equicodi = Convert.ToInt32(cliente[1]);
                    clienteSeleccionado.Empresa = Helper.FormatoHelper.DecodeCaracteresEspeciales(cliente[2]);
                    clienteSeleccionado.Suministrador = cliente[3];
                    clienteSeleccionado.Subestacion = cliente[4];
                    clienteSeleccionado.Puntomedicion = cliente[5];
                    clienteSeleccionado.Rcproudemanda = Convert.ToDecimal(cliente[6]);
                    clienteSeleccionado.Rcproufuente = cliente[7];
                    clienteSeleccionado.Rcprouemprcodisuministrador = Convert.ToInt32(cliente[8]);

                    //nuevo campo demanda
                    clienteSeleccionado.Rcproudemandareal = Convert.ToDecimal(cliente[6]);

                    clientesSelecionados.Add(clienteSeleccionado);
                }
               
            }

            var registros = new List<RcaCuadroProgUsuarioDTO>();
            if (esConsulta)
            {
                registros = servicio.ListProgramaRechazoCarga("", codigoCuadroPrograma);
            }
            else
            {
                //var ultimoPeriodo = this.servicio.ListUltimoPeriodo();
                //var anioPeriodo = ultimoPeriodo.Substring(0, 4);
                //var mesPeriodo = ultimoPeriodo.Substring(4, 2);
                //DateTime fecha = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //DateTime fechaInicioMes = new DateTime(Convert.ToInt32(anioPeriodo), Convert.ToInt32(mesPeriodo), 1);
                //DateTime fechaFinMes = fechaInicioMes.AddMonths(1).AddDays(-1);
                //DemandadiaDTO entity = this.servicioDemandaMaxima.ObtenerDatosMaximaDemanda(fechaInicioMes, fechaFinMes);
                //int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaAlphaPR16"]);

                //this.servicio.EliminarDemandaUsuarioLibre();//limpiar de tabla temporal
                //this.servicio.CargarDemandaUsuarioLibreSicli(entity.IndiceMDHP, entity.IndiceMDHFP, ultimoPeriodo, fechaInicioMes.ToString("dd/MM/yyyy"),
                //    IdLectura, ConstantesIntercambioOsinergmin.tipoInfoCodi);//SICLI carga de tabla temporal

                ////Primero buscar en PR-03 y luego en PR-16
                //if (tipoBusqueda.Equals(1))
                //{
                //    this.servicio.ActualizarDemandaUsuarioLibre(fechaInicio);// PR03 actualizacion de tabla temporal
                //}
                //if (periodoAntiguedad <= 3)
                //{
                //    this.servicio.EliminarDemandaUsuarioLibre();//limpiar de tabla temporal
                //    this.servicio.CargarDemandaUsuarioLibre();//PR 16 carga de tabla temporal
                //    this.servicio.ActualizarDemandaUsuarioLibre(fechaInicio);//PR03 actualizacion de tabla temporal
                //}
                //else
                //{
                //    var ultimoPeriodo = this.servicio.ListUltimoPeriodo();
                //    var anioPeriodo = ultimoPeriodo.Substring(0, 4);
                //    var mesPeriodo = ultimoPeriodo.Substring(4, 2);
                //    DateTime fecha = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //    DateTime fechaInicioMes = new DateTime(Convert.ToInt32(anioPeriodo), Convert.ToInt32(mesPeriodo), 1);
                //    DateTime fechaFinMes = fechaInicioMes.AddMonths(1).AddDays(-1);
                //    DemandadiaDTO entity = this.servicioDemandaMaxima.ObtenerDatosMaximaDemanda(fechaInicioMes, fechaFinMes);
                //    int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaAlphaPR16"]);

                //    this.servicio.EliminarDemandaUsuarioLibre();//limpiar de tabla temporal
                //    this.servicio.CargarDemandaUsuarioLibreSicli(entity.IndiceMDHP, entity.IndiceMDHFP, ultimoPeriodo, fechaInicioMes.ToString("dd/MM/yyyy"),
                //        IdLectura, ConstantesIntercambioOsinergmin.tipoInfoCodi);//carga de tabla temporal
                //    this.servicio.ActualizarDemandaUsuarioLibre(fechaInicio);//PR03 actualizacion de tabla temporal
                //}

                //registros = servicio.ListEmpresasProgramaRechazoCarga(bloqueHorario, "", "", 0, "", listaEmpresas.TrimEnd(','), listaEquipos.TrimEnd(','));
                var registrosIniciales = new List<RcaCuadroProgUsuarioDTO>();
                if (datosIniciales != null)
                {
                    registrosIniciales = FormatearParametrosEsquema(datosIniciales).ToList();
                }

                if(registrosIniciales.Count> 0)
                {
                    registros.AddRange(registrosIniciales);
                }

                registros.AddRange(clientesSelecionados);
            }
            //var registros = servicio.ListProgramaRechazoCarga(empresasSeleccionadas, codigoCuadroPrograma);
            //if (empresasSeleccionadas.Length > 0)
            //{
            //    registros = OrdenarRegistros(registros, identificadores);
            //}

            ConfigurarFormatoModelo(model);
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            if (esConsulta)
            {
                model.Handson.ListaExcelData = GenerarDataConsulta(registros, bloqueHorario);
            }
            else
            {
                model.Handson.ListaExcelData = GenerarData(registros, bloqueHorario);

            }

            return model;
        }

        /// <summary>
        /// Devuelve la lista de RcaCuadroProgUsuarioDTO ordenada según los identificadores Emprcodi y Equicodi
        /// para mantener el orden con el que fueron consultados
        /// </summary>
        /// <param name="registros"></param>
        /// <param name="identificadores"></param>
        /// <returns></returns>
        private List<RcaCuadroProgUsuarioDTO> OrdenarRegistros(List<RcaCuadroProgUsuarioDTO> registros, List<IdentificadoresModel> identificadores)
        {
            var registrosOrdenados = new List<RcaCuadroProgUsuarioDTO>();
            identificadores.ForEach(x =>
            {
                var registro = registros.FirstOrDefault(y => y.Emprcodi == x.Emprcodi && y.Equicodi == x.Equicodi);
                if (registro != null)
                {
                    registrosOrdenados.Add(registro);
                }
            });

            return registrosOrdenados;
        }

        /// <summary>
        /// Transforma la cadena json de datos de Cuadro Programa Usuario al modelo
        /// para graficar en Handsontable
        /// </summary>
        /// <param name="bloqueHorario"></param>
        /// <param name="datos"></param>
        /// <returns></returns>
        private FormatoModel FormatearModeloDescargaArchivo(string bloqueHorario, string datos)
        {
            FormatoModel model = new FormatoModel();

            var registros = FormatearDatosDescargaFormato(datos).ToList();
            ConfigurarFormatoModelo(model);
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));

            model.Handson.ListaExcelData = GenerarDataConsulta(registros, bloqueHorario);

            return model;
        }

        /// <summary>
        /// Transforma la cadena json de datos de Cuadro Programa Usuario
        /// en una lista de RcaCuadroProgUsuarioDTO
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private ICollection<RcaCuadroProgUsuarioDTO> FormatearDatosDescargaFormato(string datos)
        {
            var filasCabecera = 2;
            var columnas = 22;
            var celdas = datos.Split(',');
            var parametros = new List<RcaCuadroProgUsuarioDTO>();
            var inicioDatos = filasCabecera * columnas;
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                var parametro = new RcaCuadroProgUsuarioDTO();
                parametro.Empresa = celdas[i].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                parametro.Suministrador = celdas[i + 1].Replace(@"\", "").Replace("\"", "").Replace("null", "");
                parametro.Subestacion = celdas[i + 2].Replace(@"\", "").Replace("\"", "");
                parametro.Puntomedicion = celdas[i + 3].Replace(@"\", "").Replace("\"", "");
                parametro.Rcproufuente = celdas[i + 4].Replace(@"\", "").Replace("\"", "").Replace("null", "");
                parametro.Rcproucargaesencial = Convert.ToDecimal(celdas[i + 5].Replace(@"\", "").Replace("\"", ""));          
                parametro.Rcproudemanda = Convert.ToDecimal(celdas[i + 6].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproudemandareal = Convert.ToDecimal(celdas[i + 7].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproucargadisponible = Convert.ToDecimal(celdas[i + 8].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproufactork = Convert.ToDecimal(celdas[i + 9].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproudemandaracionar = Convert.ToDecimal(celdas[i + 10].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproudemandaatender = Convert.ToDecimal(celdas[i + 11].Replace(@"\", "").Replace("\"", ""));

                if (!string.IsNullOrEmpty(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcproucargarechazarcoord = Convert.ToDecimal(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("null", ""));
                }
                
                parametro.Rccuadhorinicoord = celdas[i + 13].Replace(@"\", "").Replace("\"", "");
                parametro.Rccuadhorfincoord = celdas[i + 14].Replace(@"\", "").Replace("\"", "");

                if (!string.IsNullOrEmpty(celdas[i + 15].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcproucargarechazarejec = Convert.ToDecimal(celdas[i + 15].Replace(@"\", "").Replace("\"", "").Replace("null", ""));
                }
                
                parametro.Rccuadhoriniejec = celdas[i + 16].Replace(@"\", "").Replace("\"", "");
                parametro.Rccuadhorfinejec = celdas[i + 17].Replace(@"\", "").Replace("\"", "");

                parametro.Rcproucodi = Convert.ToInt32(celdas[i + 18].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Emprcodi = Convert.ToInt32(celdas[i + 19].Replace(@"\", "").Replace("\"", ""));
                parametro.Equicodi = String.IsNullOrEmpty(celdas[i + 20].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 20].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Rcprouemprcodisuministrador = String.IsNullOrEmpty(celdas[i + 21].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 21].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                parametros.Add(parametro);
            }
            return parametros;
        }

        /// <summary>
        /// Transforma una matriz de string que tiene datos de Cuadro Programa Usuario al modelo
        /// para graficar en Handsontable
        /// </summary>
        /// <param name="bloqueHorario"></param>
        /// <param name="datos"></param>
        /// <returns></returns>
        private FormatoModel FormatearModeloDesdeMatriz(string[][] matrizDatos)
        {
            FormatoModel model = new FormatoModel();
            ConfigurarFormatoModelo(model);
            model.Handson.ListaExcelData = matrizDatos;
            return model;
        }

        /// <summary>
        /// Configura el modelo con la estructura particular del Handsontable
        /// </summary>
        /// <param name="model"></param>
        private void ConfigurarFormatoModelo(FormatoModel model)
        {
            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            model.Handson.ListaMerge = GenerarMerges();
            model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 2;
            model.Formato.Formatcols = 18;
            model.FilasCabecera = 2;
            model.ColumnasCabecera = 18;
            model.ColumnasFijas = 8;
            model.Handson.ListaColWidth = new List<int> { 200, 200, 170, 170, 70, 70, 80, 80, 80, 70, 90, 80, 70, 70, 70, 70, 70, 70 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true, true };
        }

        /// <summary>
        /// Devuelve la configuración de las areas que se van a unir en el Handsontable
        /// </summary>
        /// <returns></returns>
        private List<CeldaMerge> GenerarMerges()
        {
            var listaMerge = new List<CeldaMerge>();

            listaMerge.Add(new CeldaMerge { row = 0, col = 8, colspan = 4, rowspan = 1 });
            listaMerge.Add(new CeldaMerge { row = 0, col = 12, colspan = 3, rowspan = 1 });
            listaMerge.Add(new CeldaMerge { row = 0, col = 15, colspan = 3, rowspan = 1 });

            return listaMerge;
           // return new List<CeldaMerge>{
           //    new CeldaMerge{row=0, col=7, colspan=4, rowspan=1}             
           //};
        }


        /// <summary>
        /// Devuelve la lista de Cuadro Programa Usuario en una matriz
        /// </summary>
        /// <param name="registros"></param>
        /// <param name="bloqueHorario"></param>
        /// <returns></returns>
        private string[][] GenerarData(List<RcaCuadroProgUsuarioDTO> registros, string bloqueHorario)
        {
            var filas = registros.Count + 2;
            var cabecera = ObtenerTitulosColumnas(bloqueHorario);
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];
                if (i == 0)
                {
                    matriz[i][8] = "Programado";
                    matriz[i][12] = "Coordinado";
                    matriz[i][15] = "Ejecutado";
                }
                if (i == 1)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        matriz[i][j] = cabecera[j];                       
                    }
                }
                if (i > 1)
                {
                    matriz[i][0] = registros[i - 2].Empresa;
                    matriz[i][1] = registros[i - 2].Suministrador;
                    matriz[i][2] = registros[i - 2].Subestacion;
                    matriz[i][3] = registros[i - 2].Puntomedicion;
                    matriz[i][4] = String.IsNullOrEmpty(registros[i - 2].Rcproufuente) ? String.Empty : (registros[i - 2].Rcproufuente).ToString();
                    matriz[i][5] = registros[i - 2].Rcproucargaesencial.ToString("##0.#0");
                    matriz[i][6] = registros[i - 2].Rcproudemanda.ToString("##0.#0");
                    matriz[i][7] = registros[i - 2].Rcproudemandareal.ToString("##0.#0");
                    matriz[i][8] = registros[i - 2].Rcproucargadisponible.ToString("##0.#0");
                    matriz[i][9] = registros[i - 2].Rcproufactork.ToString("n4");
                    matriz[i][10] = registros[i - 2].Rcproudemandaracionar.ToString("##0.#0");
                    matriz[i][11] = registros[i - 2].Rcproudemandaatender.ToString("##0.#0"); 

                    matriz[i][12] = registros[i - 2].Rcproucargarechazarcoord.HasValue ? registros[i - 2].Rcproucargarechazarcoord.Value.ToString("##0.#0") : String.Empty;
                    matriz[i][13] = registros[i - 2].Rccuadhorinicoord ?? String.Empty;
                    matriz[i][14] = registros[i - 2].Rccuadhorfincoord ?? String.Empty;
                    matriz[i][15] = registros[i - 2].Rcproucargarechazarejec.HasValue ? registros[i - 2].Rcproucargarechazarejec.Value.ToString("##0.#0") : String.Empty;
                    matriz[i][16] = registros[i - 2].Rccuadhoriniejec ?? String.Empty;
                    matriz[i][17] = registros[i - 2].Rccuadhorfinejec ?? String.Empty;

                    matriz[i][18] = registros[i - 2].Rcproucodi.ToString();
                    matriz[i][19] = registros[i - 2].Emprcodi.ToString();
                    matriz[i][20] = registros[i - 2].Equicodi.ToString();
                    matriz[i][21] = registros[i - 2].Rcprouemprcodisuministrador.ToString();
                }
            }
            return matriz;
        }

        /// <summary>
        /// Devuelve la lista de Cuadro Programa Usuario en una matriz
        /// </summary>
        /// <param name="registros"></param>
        /// <param name="bloqueHorario"></param>
        /// <returns></returns>
        private string[][] GenerarDataConsulta(List<RcaCuadroProgUsuarioDTO> registros, string bloqueHorario)
        {
            var filas = registros.Count + 2;
            var cabecera = ObtenerTitulosColumnas(bloqueHorario);
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];
                if (i == 0)
                {
                    matriz[i][8] = "Programado";
                    matriz[i][12] = "Coordinado";
                    matriz[i][15] = "Ejecutado";
                }
                if (i == 1)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        matriz[i][j] = cabecera[j];
                    }
                }
                if (i > 1)
                {
                    matriz[i][0] = registros[i - 2].Empresa;
                    matriz[i][1] = registros[i - 2].Suministrador ?? String.Empty;
                    matriz[i][2] = registros[i - 2].Subestacion ?? String.Empty;
                    matriz[i][3] = registros[i - 2].Puntomedicion ?? String.Empty;                    
                    matriz[i][4] = String.IsNullOrEmpty(registros[i - 2].Rcproufuente) ? String.Empty : (registros[i - 2].Rcproufuente).ToString();
                    matriz[i][5] = registros[i - 2].Rcproucargaesencial.ToString("##0.#0");
                    matriz[i][6] = registros[i - 2].Rcproudemanda.ToString("##0.#0");
                    matriz[i][7] = registros[i - 2].Rcproudemandareal.ToString("##0.#0");
                    matriz[i][8] = registros[i - 2].Rcproucargadisponible.ToString("##0.#0");
                    matriz[i][9] = registros[i - 2].Rcproufactork.ToString("n4");
                    matriz[i][10] = registros[i - 2].Rcproudemandaracionar.ToString("##0.#0");
                    matriz[i][11] = registros[i - 2].Rcproudemandaatender.ToString("##0.#0");

                    matriz[i][12] = registros[i - 2].Rcproucargarechazarcoord.HasValue ? registros[i - 2].Rcproucargarechazarcoord.Value.ToString("##0.#0") : String.Empty;
                    matriz[i][13] = registros[i - 2].Rccuadhorinicoord ?? String.Empty;
                    matriz[i][14] = registros[i - 2].Rccuadhorfincoord ?? String.Empty;
                    matriz[i][15] = registros[i - 2].Rcproucargarechazarejec.HasValue ? registros[i - 2].Rcproucargarechazarejec.Value.ToString("##0.#0") : string.Empty;
                    matriz[i][16] = registros[i - 2].Rccuadhoriniejec ?? String.Empty;
                    matriz[i][17] = registros[i - 2].Rccuadhorfinejec ?? String.Empty;

                    matriz[i][18] = registros[i - 2].Rcproucodi.ToString();
                    matriz[i][19] = registros[i - 2].Emprcodi.ToString();
                    matriz[i][20] = registros[i - 2].Equicodi.ToString();
                    matriz[i][21] = registros[i - 2].Rcprouemprcodisuministrador.ToString();
                }
            }
            return matriz;
        }

        /// <summary>
        /// Devuelve la lista de los títulos de la cabecera
        /// </summary>
        /// <returns></returns>
        private List<string> ObtenerTitulosColumnas(string bloqueHorario)
        {
            return new List<string>() { "Razón Social", "Suministrador", "Subestación", "Nombre punto Medición", "Fuente", "Carga Esencial",
                string.Concat("Demanda en ",bloqueHorario," - Fuente (MW)"), string.Concat("Demanda en ",bloqueHorario," - para cálculo (MW)"),"Carga Disponible a Racionar", "Factor K", "Racionamiento", "Carga Luego de aplicar restricción",
                "Carga a rechazar", "Hora de Inicio","Hora Fin","Carga a rechazar", "Hora de Inicio","Hora Fin","","","","" };
        }

        [HttpPost]
        public JsonResult EliminarProgramaRechazoCarga(string datos, string codigoPrograma, int codigoCuadroPrograma)
        {
            var parametros = FormatearCuadroProgramacionUsuarioEliminar(datos);
            var codigos = parametros.Where(x => x.Rcproucodi > 0).Select(x => x.Rcproucodi).ToList();
            if (codigoCuadroPrograma > 0)
            {
                //servicio.UpdateRcaCuadroProgUsuarioEstado(codigos, "0");
                servicio.UpdateRcaCuadroProgUsuarioEstado(codigoCuadroPrograma, "0");
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult GrabarProgramaRechazoCarga(string datos, string datosDistribuidor, string codigoPrograma, string codigoCuadroPrograma, int horizonte, string programaAbrev, string nombrePrograma
            , string energiaRacionar, string demandaMinima, string bloqueHorario, string configuracion, string motivo, string ubicacion,string fechahoraInicio, 
            string fechahoraFin, int flagEracmf, int flagEracmt, int flagUsuariosRegulados, string distribEliminados, string perfilUsuario, string clientesEliminados)
        {
            var datosCuadroProgUsuario = FormatearParametrosEsquema(datos);
            
            var datosCuadroProgDistribuuidor = FormatearDatosDistribuidor(datosDistribuidor).ToList();

            if(datosCuadroProgDistribuuidor.Where(p=> p.Rcprodcodi > 0 && p.Emprcodi == 0).Any())
            {
                distribEliminados = distribEliminados + string.Join(",", datosCuadroProgDistribuuidor.Where(p => p.Rcprodcodi > 0 && p.Emprcodi == 0).Select(p => p.Rcprodcodi));

                datosCuadroProgDistribuuidor = datosCuadroProgDistribuuidor.Where(p => p.Emprcodi > 0).ToList();
            }

            var rcprogcodi = String.IsNullOrEmpty(codigoPrograma) ? 0 : Convert.ToInt32(codigoPrograma);
            var rccuadcodi = String.IsNullOrEmpty(codigoCuadroPrograma) ? 0 : Convert.ToInt32(codigoCuadroPrograma);
            flagUsuariosRegulados = datosCuadroProgDistribuuidor.Count > 0 ? 1 : 0;

            var esReprograma = false;
            if(perfilUsuario.Equals(ConstantesRechazoCarga.PerfilSCO.ToString()) && (rcprogcodi == 0 && rccuadcodi == 0))
            {
                //rcprogcodi = GrabarPrograma(horizonte, programaAbrev, nombrePrograma, rcprogcodi);

                var rcaProgramaDTO = new RcaProgramaDTO();

                //Genera el codigo de reprograma
                DateTime fecha = DateTime.ParseExact(fechahoraInicio, "dd/MM/yyyy HH:mm", null);
                var codigoReprograma = "PRC-RDO-" + fecha.ToString("yyyy-MM-dd");
                esReprograma = true;

                //Validamos si el reprograma existe, si es asi asociamos el cuadro a dicho reprograma
                //sino creamos el reprograma y asociamos dicho reprograma
                var programaExistente = servicio.GetByCriteriaRcaProgramas(codigoReprograma);
                //int nuevoCodigoPrograma = 0;
                if (programaExistente.Count == 0)
                {
                    //Obtenemos los datos del Programa Anterior
                    //var programaAnterior = servicio.GetByIdRcaPrograma(codigoPrograma);
                    //Nuevo Codigo Programa
                    rcaProgramaDTO.Rcprogcodi = 0;
                    rcaProgramaDTO.Rchorpcodi = ConstantesRechazoCarga.HorizonteReprograma;
                    rcaProgramaDTO.Rcprogabrev = codigoReprograma;
                    rcaProgramaDTO.Rcprognombre = string.Empty;
                    rcaProgramaDTO.Rcprogcodipadre = 0;
                    rcaProgramaDTO.Rcprogestregistro = "1";
                    rcaProgramaDTO.Rcprogfecinicio = fecha;
                    rcaProgramaDTO.Rcprogfecfin = fecha;
                    rcaProgramaDTO.Rcprogfeccreacion = DateTime.Now;
                    rcaProgramaDTO.Rcprogusucreacion = User.Identity.Name;

                    rcaProgramaDTO.Rcprogestado = "1";

                    rcprogcodi = servicio.SaveRcaPrograma(rcaProgramaDTO);
                    //return Json(new Respuesta { Exito = false, Mensaje = "-1" });
                }
                else
                {
                    rcprogcodi = programaExistente.First().Rcprogcodi;
                }
            }                        
            
            rccuadcodi = GrabarCuadroPrograma(energiaRacionar, demandaMinima, bloqueHorario, configuracion, motivo, ubicacion,fechahoraInicio, fechahoraFin, flagEracmf,
                flagEracmt, flagUsuariosRegulados, rcprogcodi, rccuadcodi, esReprograma);
            GrabarCuadroProgramaUsuario(codigoCuadroPrograma, datosCuadroProgUsuario, rccuadcodi, clientesEliminados.TrimEnd(','));

            if(distribEliminados!= null && distribEliminados.Trim().Length > 0)
            {
                var codigosCuadroDistrib = distribEliminados.TrimEnd(',').Split(',');

                foreach(var codigo in codigosCuadroDistrib.Distinct())
                {
                    this.servicio.DeleteRcaCuadroProgDistrib(Convert.ToInt32(codigo));
                }
            }

            GrabarCuadroProgramaDistribuidor(datosCuadroProgDistribuuidor, rccuadcodi);

            return Json(new Respuesta { Exito = true, CodigoPrograma = rcprogcodi, CodigoCuadroPrograma = rccuadcodi, IncluyeUsuariosRegulados = flagUsuariosRegulados });
        }

        private void GrabarCuadroProgramaDistribuidor(List<RcaCuadroProgDistribDTO> datosCuadroProgDistribuuidor, int rccuadcodi)
        {
            foreach(var registro in datosCuadroProgDistribuuidor)
            {
                if(registro.Rcprodcodi> 0)
                {
                    registro.Rccuadcodi = rccuadcodi;
                    registro.Rcprodfecmodificacion = DateTime.Now;
                    registro.Rcprodusumodificacion = User.Identity.Name;
                    registro.Rcprodestregistro = "1";//Temporal - revisar
                   
                    servicio.UpdateRcaCuadroProgDistrib(registro);
                }
                else
                {
                    registro.Rccuadcodi = rccuadcodi;
                    registro.Rcprodfeccreacion = DateTime.Now;
                    registro.Rcprodusucreacion = User.Identity.Name;
                    registro.Rcprodestregistro = "1";//Temporal - revisar

                    servicio.SaveRcaCuadroProgDistrib(registro);
                }
            }
        }

        /// <summary>
        /// Compara los datos activos de cuadro programa usuario con los datos a grabar para eliminar, actualizar o crear 
        /// registros en RcaCuadroProgUsuario 
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="datosCuadroProgUsuario"></param>
        /// <param name="rccuadcodi"></param>
        private void GrabarCuadroProgramaUsuario(string codigoCuadroPrograma, ICollection<RcaCuadroProgUsuarioDTO> datosCuadroProgUsuario, int rccuadcodi, string clientesEliminados)
        {
            var datosCuadroProgUsuarioActual = servicio.ListProgramaRechazoCarga("", codigoCuadroPrograma);
            //var datosEliminados = ObtenerCuadroProgUsuarioEliminados(datosCuadroProgUsuarioActual, datosCuadroProgUsuario);

            if (!String.IsNullOrEmpty(clientesEliminados.Trim()))
            {
                var codigosEliminar = Array.ConvertAll(clientesEliminados.Split(','), int.Parse);
                var datosEliminados = datosCuadroProgUsuarioActual.Where(p => codigosEliminar.Contains(p.Rcproucodi)).ToList();
                //Eliminación de RcaCuadroProgUsuario
                datosEliminados.ForEach(x =>
                {
                    //servicio.DeleteRcaCuadroProgUsuario(x.Rcproucodi);
                    x.Rccuadcodi = rccuadcodi;
                    x.Rcprouestregistro = "0";
                    servicio.UpdateRcaCuadroProgUsuario(x);
                });

                //datosCuadroProgUsuarioActual = datosCuadroProgUsuarioActual.Where(p => !codigosEliminar.Contains(p.Rcproucodi)).ToList();

            }
                      

            datosCuadroProgUsuario.ToList().ForEach(x =>
            {
                //if (datosCuadroProgUsuarioActual.Any(y => y.Emprcodi == x.Emprcodi && y.Emprcodi == x.Emprcodi))
                if(x.Rcproucodi > 0)
                {
                    //Actualización de RcaCuadroProgUsuario
                    x.Rccuadcodi = rccuadcodi;
                    x.Rcproufecmodificacion = DateTime.Now;
                    x.Rcprouusumodificacion = User.Identity.Name;
                    x.Rcprouestregistro = "1";//Temporal - revisar
                    //x.Emprcodi = x.Emprcodi == 0 ? 10614 : parametro.Emprcodi;
                    //x.Equicodi = x.Equicodi == 0 ? 17547 : parametro.Equicodi;
                    servicio.UpdateRcaCuadroProgUsuario(x);
                }
                else
                {
                    //Inserción de RcaCuadroProgUsuario
                    x.Rccuadcodi = rccuadcodi;
                    x.Rcproufeccreacion = DateTime.Now;
                    x.Rcprouusucreacion = User.Identity.Name;
                    x.Rcprouestregistro = "1";//Temporal - revisar
                    //x.Rcpareanio = Convert.ToInt32(anio);
                    servicio.SaveRcaCuadroProgUsuario(x);
                }
            });
        }

        /// <summary>
        /// Actualiza o crea el registro de CuadroPrograma y devuelve el Id
        /// </summary>
        /// <param name="energiaRacionar"></param>
        /// <param name="demandaMinima"></param>
        /// <param name="bloqueHorario"></param>
        /// <param name="configuracion"></param>
        /// <param name="motivo"></param>
        /// <param name="fechahoraInicio"></param>
        /// <param name="fechahoraFin"></param>
        /// <param name="flagEracmf"></param>
        /// <param name="flagEracmt"></param>
        /// <param name="flagUsuariosRegulados"></param>
        /// <param name="rcprogcodi"></param>
        /// <param name="rccuadcodi"></param>
        /// <returns></returns>
        private int GrabarCuadroPrograma(string energiaRacionar, string demandaMinima, string bloqueHorario, string configuracion, string motivo, string ubicacion,
            string fechahoraInicio, string fechahoraFin, int flagEracmf, int flagEracmt, int flagUsuariosRegulados, int rcprogcodi, int rccuadcodi, bool esReprograma)
        {
            if (rccuadcodi > 0)
            {
                var cuadroProgramaDTO = new RcaCuadroProgDTO();
                cuadroProgramaDTO.Rccuadcodi = rccuadcodi;
                cuadroProgramaDTO.Rcprogcodi = rcprogcodi;
                cuadroProgramaDTO.Rccuadenergiaracionar = Convert.ToDecimal(energiaRacionar);
                cuadroProgramaDTO.Rccuadumbral = Convert.ToDecimal(demandaMinima);
                cuadroProgramaDTO.Rccuadmotivo = motivo;
                cuadroProgramaDTO.Rccuadubicacion = ubicacion;
                cuadroProgramaDTO.Rccuadbloquehor = bloqueHorario.Equals("HP") ? "HP" : "FP";
                cuadroProgramaDTO.Rcconpcodi = Convert.ToInt32(configuracion);
                cuadroProgramaDTO.Rccuadflageracmf = flagEracmf > 0 ? "1" : "0";
                cuadroProgramaDTO.Rccuadflageracmt = flagEracmt > 0 ? "1" : "0";
                cuadroProgramaDTO.Rccuadflagregulado = flagUsuariosRegulados > 0 ? "1" : "0";
                cuadroProgramaDTO.Rccuadfechorinicio = DateTime.ParseExact(fechahoraInicio, "dd/MM/yyyy HH:mm", null);
                cuadroProgramaDTO.Rccuadfechorfin = DateTime.ParseExact(fechahoraFin, "dd/MM/yyyy HH:mm", null);
                cuadroProgramaDTO.Rccuadestregistro = "1";
                //cuadroProgramaDTO.Rccuadestado = "P";
                //cuadroProgramaDTO.Rcestacodi = !esReprograma ? ConstantesRechazoCarga.EstadoCuadroProgramado : ConstantesRechazoCarga.EstadoCuadroReprogramado;
                cuadroProgramaDTO.Rccuadfecmodificacion = DateTime.Now;
                cuadroProgramaDTO.Rccuadusumodificacion = User.Identity.Name;

                servicio.UpdateRcaCuadroProg(cuadroProgramaDTO);
            }
            else
            {
                var cuadroProgramaDTO = new RcaCuadroProgDTO();
                cuadroProgramaDTO.Rccuadcodi = rccuadcodi;
                cuadroProgramaDTO.Rcprogcodi = rcprogcodi;
                cuadroProgramaDTO.Rccuadenergiaracionar = Convert.ToDecimal(energiaRacionar);
                cuadroProgramaDTO.Rccuadumbral = Convert.ToDecimal(demandaMinima);
                cuadroProgramaDTO.Rccuadmotivo = motivo;
                cuadroProgramaDTO.Rccuadubicacion = ubicacion;
                cuadroProgramaDTO.Rccuadbloquehor = bloqueHorario.Equals("HP") ? "HP" : "FP";
                cuadroProgramaDTO.Rcconpcodi = Convert.ToInt32(configuracion);
                cuadroProgramaDTO.Rccuadflageracmf = flagEracmf > 0 ? "1" : "0";
                cuadroProgramaDTO.Rccuadflageracmt = flagEracmt > 0 ? "1" : "0";
                cuadroProgramaDTO.Rccuadflagregulado = flagUsuariosRegulados > 0 ? "1" : "0";
                cuadroProgramaDTO.Rccuadfechorinicio = DateTime.ParseExact(fechahoraInicio, "dd/MM/yyyy HH:mm", null);
                cuadroProgramaDTO.Rccuadfechorfin = DateTime.ParseExact(fechahoraFin, "dd/MM/yyyy HH:mm", null);
                cuadroProgramaDTO.Rccuadestregistro = "1";
                //cuadroProgramaDTO.Rccuadestado = "P";
                cuadroProgramaDTO.Rcestacodi = !esReprograma ? ConstantesRechazoCarga.EstadoCuadroProgramado : ConstantesRechazoCarga.EstadoCuadroReprogramado;
                cuadroProgramaDTO.Rccuadfeccreacion = DateTime.Now;
                cuadroProgramaDTO.Rccuadusucreacion = User.Identity.Name;

                rccuadcodi = servicio.SaveRcaCuadroProg(cuadroProgramaDTO);
            }
            return rccuadcodi;
        }

        /// <summary>
        /// Crea el registro programa en caso de no existir y devuelve el Id
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="programaAbrev"></param>
        /// <param name="nombrePrograma"></param>
        /// <param name="rcprogcodi"></param>
        /// <returns></returns>
        private int GrabarPrograma(int horizonte, string programaAbrev, string nombrePrograma, int rcprogcodi)
        {
            if (rcprogcodi == 0)
            {
                var programaDTO = new RcaProgramaDTO();
                programaDTO.Rcprogcodi = rcprogcodi;
                programaDTO.Rcprogabrev = programaAbrev;
                programaDTO.Rcprognombre = nombrePrograma;
                programaDTO.Rchorpcodi = horizonte;
                programaDTO.Rcprogestregistro = "1";
                programaDTO.Rcprogfeccreacion = DateTime.Now;
                programaDTO.Rcprogusucreacion = User.Identity.Name;

                rcprogcodi = servicio.SaveRcaPrograma(programaDTO);
            }
            return rcprogcodi;
        }

        /// <summary>
        /// Obtiene los Cuadro Programa Usuario que han sido eliminados comparando
        /// la lista activa asociada al Cuadro con la lista nueva
        /// </summary>
        /// <param name="datosCuadroProgUsuarioActual"></param>
        /// <param name="datosCuadroProgUsuario"></param>
        /// <returns></returns>
        private List<RcaCuadroProgUsuarioDTO> ObtenerCuadroProgUsuarioEliminados(List<RcaCuadroProgUsuarioDTO> datosCuadroProgUsuarioActual, ICollection<RcaCuadroProgUsuarioDTO> datosCuadroProgUsuario)
        {
            var cuadroProgUsuarioEliminados = new List<RcaCuadroProgUsuarioDTO>();
            datosCuadroProgUsuarioActual.ForEach(x =>
            {
                if (!datosCuadroProgUsuario.Any(y => y.Emprcodi == x.Emprcodi && y.Equicodi == x.Equicodi))
                {
                    cuadroProgUsuarioEliminados.Add(x);
                }
            });
            return cuadroProgUsuarioEliminados;
        }

        private ICollection<RcaCuadroProgUsuarioDTO> FormatearParametrosEsquema(string datos)
        {
            var filasCabecera = 2;
            var columnas = 22;
            var celdas = datos.Split(',');
            var parametros = new List<RcaCuadroProgUsuarioDTO>();
            var inicioDatos = filasCabecera * columnas;
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                var parametro = new RcaCuadroProgUsuarioDTO();
                parametro.Empresa = celdas[i].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                parametro.Suministrador = celdas[i + 1].Replace(@"\", "").Replace("\"", "");
                parametro.Subestacion = celdas[i + 2].Replace(@"\", "").Replace("\"", "");
                parametro.Puntomedicion = celdas[i + 3].Replace(@"\", "").Replace("\"", "");
                parametro.Rcproufuente = celdas[i + 4].Replace(@"\", "").Replace("\"", "");
                parametro.Rcproucargaesencial = Convert.ToDecimal(celdas[i + 5].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                parametro.Rcproudemanda = Convert.ToDecimal(celdas[i + 6].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                parametro.Rcproudemandareal = Convert.ToDecimal(celdas[i + 7].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));

                parametro.Rcproucargadisponible = Convert.ToDecimal(celdas[i + 8].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproufactork = Convert.ToDecimal(celdas[i + 9].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproudemandaracionar = Convert.ToDecimal(celdas[i + 10].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproudemandaatender = Convert.ToDecimal(celdas[i + 11].Replace(@"\", "").Replace("\"", ""));

                if(!string.IsNullOrEmpty(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcproucargarechazarcoord = Convert.ToDecimal(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("null", ""));
                }
                
                parametro.Rccuadhorinicoord = celdas[i + 13].Replace(@"\", "").Replace("\"", "");
                parametro.Rccuadhorfincoord = celdas[i + 14].Replace(@"\", "").Replace("\"", "");

                if (!string.IsNullOrEmpty(celdas[i + 15].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcproucargarechazarejec = Convert.ToDecimal(celdas[i + 15].Replace(@"\", "").Replace("\"", "").Replace("null", ""));
                }

                //parametro.Rcproucargarechazarejec = Convert.ToDecimal(celdas[i + 14].Replace(@"\", "").Replace("\"", "").Replace("null", ""));
                parametro.Rccuadhoriniejec = celdas[i + 16].Replace(@"\", "").Replace("\"", "");
                parametro.Rccuadhorfinejec = celdas[i + 17].Replace(@"\", "").Replace("\"", "");

                parametro.Rcproucodi = Convert.ToInt32(celdas[i + 18].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Emprcodi = Convert.ToInt32(celdas[i + 19].Replace(@"\", "").Replace("\"", ""));
                parametro.Equicodi = String.IsNullOrEmpty(celdas[i + 20].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 20].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Rcprouemprcodisuministrador = String.IsNullOrEmpty(celdas[i + 21].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 21].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                //parametro.Rcproucodi = Convert.ToInt32(celdas[i + 11].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Emprcodi = Convert.ToInt32(celdas[i + 12].Replace(@"\", "").Replace("\"", ""));
                //parametro.Equicodi = String.IsNullOrEmpty(celdas[i + 13].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 13].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Rcprouemprcodisuministrador = String.IsNullOrEmpty(celdas[i + 14].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 14].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                //parametro.Emprcodi = Convert.ToInt32(celdas[i + 8].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Equicodi = Convert.ToInt32(celdas[i + 9].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                parametros.Add(parametro);

            }
            return parametros;
        }

        private ICollection<RcaCuadroProgUsuarioDTO> FormatearParametrosEsquemaCalculo(string datos)
        {
            var filasCabecera = 2;
            var columnas = 22;
            var celdas = datos.Split(',');
            var parametros = new List<RcaCuadroProgUsuarioDTO>();
            var inicioDatos = filasCabecera * columnas;
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                var parametro = new RcaCuadroProgUsuarioDTO();
                parametro.Empresa = String.IsNullOrEmpty(celdas[i].Replace(@"\", "").Replace("\"", "").Replace("[", "")) ? "" : celdas[i].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                parametro.Suministrador = celdas[i + 1].Replace(@"\", "").Replace("\"", "").Replace("null", "");
                parametro.Subestacion = celdas[i + 2].Replace(@"\", "").Replace("\"", "").Replace("null", "");
                parametro.Puntomedicion = celdas[i + 3].Replace(@"\", "").Replace("\"", "");
                parametro.Rcproufuente = celdas[i + 4].Replace(@"\", "").Replace("\"", "");                
                parametro.Rcproudemanda = Convert.ToDecimal(celdas[i + 6].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproudemandareal = Convert.ToDecimal(celdas[i + 7].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproucargadisponible = Convert.ToDecimal(celdas[i + 8].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproufactork = Convert.ToDecimal(celdas[i + 9].Replace(@"\", "").Replace("\"", ""));
                parametro.Rcproudemandaracionar = Convert.ToDecimal(celdas[i + 10].Replace(@"\", "").Replace("\"", ""));

                if (!string.IsNullOrEmpty(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcproucargarechazarcoord = Convert.ToDecimal(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("null", ""));
                }
                
                parametro.Rccuadhorinicoord = celdas[i + 13].Replace(@"\", "").Replace("\"", "");
                parametro.Rccuadhorfincoord = celdas[i + 14].Replace(@"\", "").Replace("\"", "");


                if (!string.IsNullOrEmpty(celdas[i + 15].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcproucargarechazarejec = Convert.ToDecimal(celdas[i + 15].Replace(@"\", "").Replace("\"", "").Replace("null", ""));
                }
                
                parametro.Rccuadhoriniejec = celdas[i + 16].Replace(@"\", "").Replace("\"", "");
                parametro.Rccuadhorfinejec = celdas[i + 17].Replace(@"\", "").Replace("\"", "");

                parametro.Rcproucodi = Convert.ToInt32(celdas[i + 18].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Emprcodi = Convert.ToInt32(celdas[i + 19].Replace(@"\", "").Replace("\"", ""));
                parametro.Equicodi = String.IsNullOrEmpty(celdas[i + 20].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 20].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Rcprouemprcodisuministrador = String.IsNullOrEmpty(celdas[i + 21].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 21].Replace(@"\", "").Replace("\"", "").Replace("]", ""));


                //parametro.Rcproucodi = Convert.ToInt32(celdas[i + 11].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Emprcodi = Convert.ToInt32(celdas[i + 12].Replace(@"\", "").Replace("\"", ""));
                //parametro.Equicodi = String.IsNullOrEmpty(celdas[i + 13].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 13].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Rcprouemprcodisuministrador = String.IsNullOrEmpty(celdas[i + 14].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 14].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                //parametro.Emprcodi = Convert.ToInt32(celdas[i + 8].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Equicodi = Convert.ToInt32(celdas[i + 9].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                parametros.Add(parametro);
                /*** parametros.Add(new RcaParamEsquemaDTO
                 {
                     Emprrazsocial = celdas[i].Replace(@"\","").Replace("\"",""),
                     Areanomb = celdas[i+1],
                     Equinomb = celdas[i+2],
                     Rcparehperacmf = Convert.ToDecimal(celdas[i+3]),
                     Rcparehperacmt = Convert.ToDecimal(celdas[i + 4]),
                     Rcparehfperacmf = Convert.ToDecimal(celdas[i + 5]),
                     Rcparehfperacmt = Convert.ToDecimal(celdas[i + 6])
                 });**/
            }
            return parametros;
        }

        private ICollection<RcaCuadroProgUsuarioDTO> FormatearCuadroProgramacionUsuarioEliminar(string datos)
        {
            var filasCabecera = 2;
            var columnas = 13;
            var celdas = datos.Split(',');
            var parametros = new List<RcaCuadroProgUsuarioDTO>();
            var inicioDatos = filasCabecera * columnas;
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                var parametro = new RcaCuadroProgUsuarioDTO();

                parametro.Rcproucodi = String.IsNullOrEmpty(celdas[i + 10].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 10].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Emprcodi = String.IsNullOrEmpty(celdas[i + 11].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 11].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Equicodi = String.IsNullOrEmpty(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                //parametro.Emprcodi = Convert.ToInt32(celdas[i + 8].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Equicodi = Convert.ToInt32(celdas[i + 9].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                parametros.Add(parametro);

            }
            return parametros;
        }

        private ICollection<RcaCuadroProgDistribDTO> FormatearDatosDistribuidor(string datos)
        {
            var filasCabecera = 0;
            var columnas = 5;
            var celdas = datos.Split(',');
            var parametros = new List<RcaCuadroProgDistribDTO>();
            var inicioDatos = filasCabecera * columnas;
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                var parametro = new RcaCuadroProgDistribDTO();

                parametro.Rcprodcodi = String.IsNullOrEmpty(celdas[i].Replace(@"\", "").Replace("\"", "").Replace("]", "").Replace("[","")) ? 0 : 
                    Convert.ToInt32(celdas[i].Replace(@"\", "").Replace("\"", "").Replace("]", "").Replace("[", ""));
                parametro.Emprcodi = String.IsNullOrEmpty(celdas[i + 1].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 1].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                parametro.Rcprodsubestacion = String.IsNullOrEmpty(celdas[i + 2].Replace(@"\", "").Replace("\"", "").Replace("null", "")) ? String.Empty : celdas[i + 2].Replace(@"\", "").Replace("\"", "").Replace("null", "");
                parametro.Rcproddemanda = String.IsNullOrEmpty(celdas[i + 3].Replace(@"\", "").Replace("\"", "").Replace("null", "0")) ? Decimal.Zero :
                    Convert.ToDecimal(celdas[i + 3].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                parametro.Rcprodcargarechazar = String.IsNullOrEmpty(celdas[i + 4].Replace(@"\", "").Replace("\"", "").Replace("null", "0").Replace("]", "")) ? Decimal.Zero :
                    Convert.ToDecimal(celdas[i + 4].Replace(@"\", "").Replace("\"", "").Replace("null", "0").Replace("]", ""));

                parametros.Add(parametro);

            }
            parametros = parametros.Where(p => p.Emprcodi > 0 || (p.Rcprodcodi > 0 && p.Emprcodi == 0)).ToList();

            return parametros;
        }

        [HttpPost]
        public PartialViewResult ListarEmpresas(string fechaInicio, string bloqueHorario, string zona, string estacion, string medicion,
            string empresa, string empresaequipoExcluir, int periodoAntiguedad, int tipoBusqueda, int marcaTodasSubestaciones)
        {
            ProgramaRechazoCargaModel model = new ProgramaRechazoCargaModel();
            var medicionValor = String.IsNullOrEmpty(medicion) ? Decimal.Zero : Convert.ToDecimal(medicion);
            estacion = estacion.Equals("0") || estacion.Equals("") ? String.Empty : estacion;

            var subestacionesSeleccionadas = estacion.Split(',');

            //if(subestacionesSeleccionadas.Count() > 1000)
            //{
            //    model.ListProgramaRechazoCargaEmpresa = new List<RcaCuadroProgUsuarioDTO>();
            //    model.mensajeErrorBuscadorClientes = "La seleccion maxima de subestaciones es de 1000";
            //    return PartialView("ListaEmpresas", model);
            //}

            if (zona.Equals("0") && marcaTodasSubestaciones > 0)
            {
                estacion = string.Empty;
            }

            //var antiguedadDatosMeses = this.servicio.ListAntiguedadDatosDemandaUsuario();
            var ultimoPeriodo = this.servicio.ListUltimoPeriodo();
            var anioPeriodo = ultimoPeriodo.Substring(0, 4);
            var mesPeriodo = ultimoPeriodo.Substring(4, 2);
            DateTime fecha = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaInicioMes = new DateTime(Convert.ToInt32(anioPeriodo), Convert.ToInt32(mesPeriodo), 1);
            DateTime fechaFinMes = fechaInicioMes.AddMonths(1).AddDays(-1);
            DemandadiaDTO entity = this.servicioDemandaMaxima.ObtenerDatosMaximaDemanda(fechaInicioMes, fechaFinMes);
            int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaAlphaPR16"]);

            this.servicio.EliminarDemandaUsuarioLibre();//limpiar de tabla temporal
            this.servicio.CargarDemandaUsuarioLibre();//PR16 carga de tabla temporal
            //this.servicio.CargarDemandaUsuarioLibreSicli(entity.IndiceMDHP, entity.IndiceMDHFP, ultimoPeriodo, fechaInicioMes.ToString("dd/MM/yyyy"),
            //IdLectura, ConstantesIntercambioOsinergmin.tipoInfoCodi);//SICLI carga de tabla temporal

            //Primero buscar en PR-03 y luego en PR-16
            if (tipoBusqueda.Equals(1))
            {
                this.servicio.ActualizarDemandaUsuarioLibre(fechaInicio);// PR03 actualizacion de tabla temporal
            }

            //if (periodoAntiguedad <= 3)
            //{
            //    this.servicio.EliminarDemandaUsuarioLibre();//limpiar de tabla temporal
            //    this.servicio.CargarDemandaUsuarioLibre();//PR16 carga de tabla temporal
            //    this.servicio.ActualizarDemandaUsuarioLibre(fechaInicio);//PR03 actualizacion de tabla temporal
            //}
            //else
            //{
            //    //this.servicio.EliminarDemandaUsuarioLibre();//limpiar de tabla temporal

            //    var ultimoPeriodo = this.servicio.ListUltimoPeriodo();
            //    var anioPeriodo = ultimoPeriodo.Substring(0, 4);
            //    var mesPeriodo = ultimoPeriodo.Substring(4, 2);
            //    DateTime fecha = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    DateTime fechaInicioMes = new DateTime(Convert.ToInt32(anioPeriodo), Convert.ToInt32(mesPeriodo), 1);
            //    DateTime fechaFinMes = fechaInicioMes.AddMonths(1).AddDays(-1);
            //    DemandadiaDTO entity = this.servicioDemandaMaxima.ObtenerDatosMaximaDemanda(fechaInicioMes, fechaFinMes);
            //    int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaAlphaPR16"]);

            //    this.servicio.EliminarDemandaUsuarioLibre();//limpiar de tabla temporal
            //    this.servicio.CargarDemandaUsuarioLibreSicli(entity.IndiceMDHP, entity.IndiceMDHFP, ultimoPeriodo, fechaInicioMes.ToString("dd/MM/yyyy"), 
            //        IdLectura, ConstantesIntercambioOsinergmin.tipoInfoCodi);//SICLI carga de tabla temporal
            //    this.servicio.ActualizarDemandaUsuarioLibre(fechaInicio);// PR03 actualizacion de tabla temporal

            //}

            
            var listaEmpresas = this.servicio.ListEmpresasProgramaRechazoCarga(bloqueHorario, empresa, estacion, medicionValor, empresa, String.Empty, String.Empty);
            model.ListProgramaRechazoCargaEmpresa = EliminarValores(listaEmpresas, empresaequipoExcluir);
            return PartialView("ListaEmpresas", model);
        }

        /// <summary>
        /// Elimina los registros de la lista de RcaCuadroProgUsuarioDTO que esten contenidos en los identificadores 
        /// del parametro empresaequipoExcluir para que la busqueda de Empresas no muestre valores ya seleccionados
        /// </summary>
        /// <param name="listaEmpresas"></param>
        /// <param name="empresaequipoExcluir"></param>
        /// <returns></returns>
        private List<RcaCuadroProgUsuarioDTO> EliminarValores(List<RcaCuadroProgUsuarioDTO> listaEmpresas, string empresaequipoExcluir)
        {
            var identificadores = new List<IdentificadoresModel>();

            if (empresaequipoExcluir.Length > 0)
            {
                empresaequipoExcluir = empresaequipoExcluir.TrimEnd(',');
                var empresasEquipos = empresaequipoExcluir.Split(',');
                foreach (var empresa in empresasEquipos)
                {
                    var con = empresa.Split('/');
                    identificadores.Add(new IdentificadoresModel { Emprcodi = Convert.ToInt32(con[0]), Equicodi = Convert.ToInt32(con[1]) });
                }
            }
            if (!identificadores.Any()) return listaEmpresas;
            var listaEmpresasFiltrada = new List<RcaCuadroProgUsuarioDTO>();
            listaEmpresas.ForEach(x =>
            {
                if (!identificadores.Any(y => y.Emprcodi == x.Emprcodi && y.Equicodi == x.Equicodi))
                {
                    listaEmpresasFiltrada.Add(x);
                }
            });

            return listaEmpresasFiltrada;
        }

        public JsonResult ObtenerSubEstacion(string codigoZona)
        {
            List<AreaDTO> subEstacion = new List<AreaDTO>();
            //subEstacion.Add(new AreaDTO { AREACODI = 0, AREANOMB = "-- SELECCIONE --" });
            //if (codigoZona == "0") return Json(new SelectList(subEstacion, "AREACODI", "AREANOMB"));
            var cuadrosFiltrados = servicio.ListSubEstacion(Convert.ToInt32(codigoZona));
            if (cuadrosFiltrados.Any())
            {
                subEstacion.AddRange(cuadrosFiltrados);
            }
            return Json(new SelectList(subEstacion, "AREACODI", "AREANOMB"));
        }

        [HttpPost]
        public JsonResult ObtenerCalculoRechazoCarga(string datos, string datosDistribuidor, decimal magnitudRechazoCarga, int incluyeEracmf, int incluyeEracmt, string bloqueHorario, decimal umbral)
        {
            ProgramaRechazoCargaModel model = new ProgramaRechazoCargaModel();
            model.MensajeDistribucion = string.Empty;
            model.CargarDistribuidores = false;
            try
            {
                List<RcaCuadroProgUsuarioDTO> listaUsuariosCalculados = null;

                model.ModeloUsuariosLibres = CalcularRechazoCarga(datos, magnitudRechazoCarga, incluyeEracmf, incluyeEracmt, bloqueHorario, umbral, out listaUsuariosCalculados);

                var listaDistirbuidores = FormatearDatosDistribuidor(datosDistribuidor).ToList();
                model.DistribuidoresEliminados = "";
                if (listaDistirbuidores.Where(p => p.Rcprodcodi > 0 && p.Emprcodi == 0).Any())
                {
                    model.DistribuidoresEliminados =  string.Join(",", listaDistirbuidores.Where(p => p.Rcprodcodi > 0 && p.Emprcodi == 0).Select(p => p.Rcprodcodi));

                    listaDistirbuidores = listaDistirbuidores.Where(p => p.Emprcodi > 0).ToList();
                }

                if (listaUsuariosCalculados!= null && listaDistirbuidores.Count > 0)
                {
                    //Calculos Finales
                    var sumaDemandaRacionar = listaUsuariosCalculados.Sum(p => p.Rcproudemandaracionar);
                    sumaDemandaRacionar = Math.Round(sumaDemandaRacionar, 2);

                    var mensajeDistribucion = "";
                    if (magnitudRechazoCarga - sumaDemandaRacionar > 0.01M)
                    {
                        if (listaDistirbuidores.Count > 0)
                        {
                            var promedioRepartir = (magnitudRechazoCarga - sumaDemandaRacionar) / listaDistirbuidores.Count;

                            foreach (var item in listaDistirbuidores)
                            {
                                item.Rcprodcargarechazar = Math.Round(promedioRepartir, 2);
                            }
                            model.CargarDistribuidores = true;
                        }
                        else
                        {
                            mensajeDistribucion = string.Format(@"Luego de la distribución de carga en la lista de clientes falta distribuir ({0} MW) para completar la potencia a rechazar.", (magnitudRechazoCarga - sumaDemandaRacionar).ToString("N2"));
                        }
                    }
                    model.ListSiEmpresa = this.servicio.ListaEmpresasRechazoCarga("", _tipoEmpresaDistribuidor, _estadoRegistroEmpresaActivo).OrderBy(p => p.Emprrazsocial).ToList();
                    model.MensajeDistribucion = mensajeDistribucion;
                    
                    model.DatosMatriz = FormatearDatosDistribPantalla(listaDistirbuidores);
                }
                else
                {
                    var sumaDemandaRacionar = listaUsuariosCalculados != null ? listaUsuariosCalculados.Sum(p => p.Rcproudemandaracionar) : decimal.Zero;
                    sumaDemandaRacionar = Math.Round(sumaDemandaRacionar, 2);

                    if (magnitudRechazoCarga - sumaDemandaRacionar > 0.01M)
                    {
                        model.MensajeDistribucion = string.Format(@"Luego de la distribución de carga en la lista de clientes falta distribuir ({0} MW) para completar la potencia a rechazar.", (magnitudRechazoCarga - sumaDemandaRacionar).ToString("N2") );
                    }
                        
                }
            }
            catch(Exception ex)
            {
                log.Fatal("ProgramaRechazoCargaController", ex);
                //throw;
            }
            return Json(model);
        }

        private FormatoModel CalcularRechazoCarga(string datos, decimal magnitudRechazoCarga, int incluyeEracmf, int incluyeEracmt, 
            string bloqueHorario, decimal umbral, out List<RcaCuadroProgUsuarioDTO> lista)
        {
            FormatoModel model = new FormatoModel();
            lista = null;

            if (datos.Length > 0)
            {
                lista = FormatearParametrosEsquemaCalculo(datos).ToList();

                var listaEmpresas = string.Join(",", lista.Select(p => p.Emprcodi));
                var listaEquipos = string.Join(",", lista.Select(p => p.Equicodi));
                var datosTablaParamEsquema = servicio.ListarRcaParamEsquemaPorPuntoMedicion(listaEquipos).ToList();
                var datosCargaEsencial = servicio.ListarCargaEsencialPorPuntoMedicion(listaEquipos, string.Empty).ToList();
                var sumaDemandaIncluyeCargas = decimal.Zero;//r en macro
                var factorK = decimal.Zero;
                //var datosDemandaComplementaria = new List<RcaCuadroProgUsuarioDTO>();

                //if (incluyeEracmf > 0 || incluyeEracmt > 0)
                //{
                //    //datosDemandaComplementaria = servicio.ListEmpresasProgramaRechazoCarga((bloqueHorario.Equals("HP") ? "HFP" : "HP"), "", "", 0, "", listaEmpresas.TrimEnd(','), listaEquipos.TrimEnd(','));
                //}

                foreach (var empresa in lista)
                {
                    var cargaEsencial = datosCargaEsencial.Where(p => p.Equicodi == empresa.Equicodi).Count() > 0 ? datosCargaEsencial.Where(p => p.Equicodi == empresa.Equicodi).First() : null;

                    if (cargaEsencial!=null && cargaEsencial.Rccaretipocarga.Equals(2)) 
                    {
                        empresa.Rcproucargadisponible = 0;
                        empresa.Rcproucargaesencial = empresa.Rcproudemandareal;
                    }
                    else {
                        if (incluyeEracmf > 0 || incluyeEracmt > 0)
                        {
                            var paramEsquema = datosTablaParamEsquema.Where(p => p.Equicodi == empresa.Equicodi).Count() > 0 ? datosTablaParamEsquema.Where(p => p.Equicodi == empresa.Equicodi).First() : null;
                            //var demandaComplementaria = datosDemandaComplementaria.Where(p => p.Equicodi == empresa.Equicodi).Count() > 0 ? datosDemandaComplementaria.Where(p => p.Equicodi == empresa.Equicodi).First().Demanda : 0;
                            var resERACMF = decimal.Zero;
                            var resERACMT = decimal.Zero;

                            if (incluyeEracmf > 0)
                            {
                                if (bloqueHorario.Equals("HP"))
                                {
                                    //var valorERACMF = Decimal.Zero;
                                    if (paramEsquema != null)
                                    {
                                        if (paramEsquema.Rcparenroesquema.Equals(2))
                                        {
                                            resERACMF = paramEsquema.Rcparehperacmf2.HasValue ? paramEsquema.Rcparehperacmf2.Value : decimal.Zero;
                                        }
                                        else
                                        {
                                            resERACMF = paramEsquema.Rcparehperacmf.HasValue ? paramEsquema.Rcparehperacmf.Value : decimal.Zero;
                                        }                                        
                                    }

                                    //resERACMF = ObtenerValorCargaAdicional(empresa.Rcproudemandareal, demandaComplementaria, valorERACMF);
                                    //resERACMF = ObtenerValorCargaAdicional(empresa.Rcproudemanda, demandaComplementaria, paramEsquema != null ? (paramEsquema.Rcparehperacmf.Value) : 0);
                                }
                                else
                                {
                                    //var valorERACMF = Decimal.Zero;
                                    //if (paramEsquema != null)
                                    //{
                                    //    valorERACMF = paramEsquema.Rcparenroesquema.Equals(2) ? paramEsquema.Rcparehfperacmf2.Value : paramEsquema.Rcparehfperacmf.Value;
                                    //}
                                    if (paramEsquema != null)
                                    {
                                        if (paramEsquema.Rcparenroesquema.Equals(2))
                                        {
                                            resERACMF = paramEsquema.Rcparehfperacmf2.HasValue ? paramEsquema.Rcparehfperacmf2.Value : decimal.Zero;
                                        }
                                        else
                                        {
                                            resERACMF = paramEsquema.Rcparehfperacmf.HasValue ? paramEsquema.Rcparehfperacmf.Value : decimal.Zero;
                                        }


                                    }

                                    // = ObtenerValorCargaAdicional(empresa.Rcproudemandareal, demandaComplementaria, valorERACMF);
                                    //resERACMF = ObtenerValorCargaAdicional(empresa.Rcproudemanda, demandaComplementaria, paramEsquema != null ? (paramEsquema.Rcparehfperacmf.Value) : 0);
                                }
                            }

                            if (incluyeEracmt > 0)
                            {
                                if (bloqueHorario.Equals("HP"))
                                {

                                    if (paramEsquema != null && paramEsquema.Rcparehperacmt.HasValue)
                                    {
                                        resERACMT = paramEsquema.Rcparehperacmt.Value;
                                    }

                                    //resERACMT = ObtenerValorCargaAdicional(empresa.Rcproudemandareal, demandaComplementaria, 
                                    //    (paramEsquema != null && paramEsquema.Rcparehperacmt.HasValue) ? (paramEsquema.Rcparehperacmt.Value) : 0);
                                }
                                else
                                {

                                    if (paramEsquema != null && paramEsquema.Rcparehfperacmt.HasValue)
                                    {
                                        resERACMT = paramEsquema.Rcparehfperacmt.Value;
                                    }

                                    //resERACMT = ObtenerValorCargaAdicional(empresa.Rcproudemandareal, demandaComplementaria, 
                                    //    (paramEsquema != null && paramEsquema.Rcparehfperacmt.HasValue) ? (paramEsquema.Rcparehfperacmt.Value) : 0);
                                }
                            }
                            var demandaReferencial = decimal.Zero;
                            if (paramEsquema != null)
                            {
                                if (bloqueHorario.Equals("HP"))
                                {
                                    demandaReferencial = paramEsquema.Rcparehpdemandaref.HasValue ? paramEsquema.Rcparehpdemandaref.Value : decimal.Zero;
                                }
                                else
                                {
                                    demandaReferencial = paramEsquema.Rcparehfpdemandaref.HasValue ? paramEsquema.Rcparehfpdemandaref.Value : decimal.Zero;
                                }
                            }

                            //var demandaTotal = empresa.Rcproudemandareal + demandaReferencial;
                            var demandaTotal = empresa.Rcproudemandareal;

                            if (demandaReferencial > 0)
                            {
                                empresa.Rcproucargadisponible = ( demandaTotal * (1 - (resERACMF + resERACMT)/demandaReferencial)) - (cargaEsencial != null ? (cargaEsencial.Rccarecarga.Value) : 0);
                            }
                            else
                            {                                 
                                //empresa.Rcproucargadisponible = (demandaTotal * (1 - resERACMF - resERACMT)) - (cargaEsencial != null ? (cargaEsencial.Rccarecarga.Value) : 0);
                                empresa.Rcproucargadisponible = (demandaTotal) - (cargaEsencial != null ? (cargaEsencial.Rccarecarga.Value) : 0);
                            }
                           
                            empresa.Rcproucargaesencial = (cargaEsencial != null ? (cargaEsencial.Rccarecarga.Value) : 0);

                            if (empresa.Rcproucargadisponible < 0)
                            {
                                empresa.Rcproucargadisponible = 0;
                            }

                        }
                        else
                        {
                            empresa.Rcproucargadisponible = empresa.Rcproudemandareal - (cargaEsencial != null ? (cargaEsencial.Rccarecarga.Value) : 0);
                            empresa.Rcproucargaesencial = (cargaEsencial != null ? (cargaEsencial.Rccarecarga.Value) : 0);

                            if (empresa.Rcproucargadisponible < 0)
                            {
                                empresa.Rcproucargadisponible = 0;
                            }
                        }
                    }                    

                    sumaDemandaIncluyeCargas = sumaDemandaIncluyeCargas + empresa.Rcproucargadisponible;
                }

                if (sumaDemandaIncluyeCargas > 0)
                {
                    factorK = magnitudRechazoCarga / sumaDemandaIncluyeCargas;
                }

                if (factorK > 1)
                {
                    var sumaDemandaSinCargas = lista.Sum(p => p.Rcproudemandareal);
                    if (sumaDemandaSinCargas > 0)
                    {
                        factorK = magnitudRechazoCarga / sumaDemandaSinCargas;
                    }

                    if (factorK > 1)
                    {
                        factorK = 1;
                    }

                    foreach (var empresa in lista)
                    {
                        empresa.Rcproufactork = factorK;
                        empresa.Rcproudemandaracionar = empresa.Rcproucargadisponible * factorK;
                        empresa.Rcproudemandaatender = empresa.Rcproudemandareal - empresa.Rcproudemandaracionar;
                    }
                }
                else
                {
                    foreach (var empresa in lista)
                    {
                        empresa.Rcproufactork = factorK;
                        empresa.Rcproudemandaracionar = empresa.Rcproucargadisponible * factorK;
                        empresa.Rcproudemandaatender = empresa.Rcproudemandareal - empresa.Rcproudemandaracionar;
                    }
                }

               


                ConfigurarFormatoModelo(model);
                lista.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
                model.Handson.ListaExcelData = GenerarDataConsulta(lista, bloqueHorario);
            }

            return model;
        }

        private static decimal ObtenerValorCargaAdicional(decimal demanda, decimal demandaComplementaria, decimal sumaCarga)
        {
            var resultado = decimal.Zero;

            var mayor = demanda > demandaComplementaria ? demanda : demandaComplementaria;

            if (mayor > 0)
            {
                resultado = sumaCarga / mayor;
            }

            if (resultado > 1)
            {
                resultado = 1;
            }
            return resultado;
        }

        public PartialViewResult ObtenerSubEstaciones(string codigoZona)
        {
            ProgramaRechazoCargaModel model = new ProgramaRechazoCargaModel();
            model.SubEstaciones = new List<AreaDTO>();
           
            //if (codigoZona.Equals("0"))
            //{
            //    return PartialView("CargarSubestaciones", model);
            //}
            var cuadrosFiltrados = servicio.ListSubEstacion(Convert.ToInt32(codigoZona));
            if (cuadrosFiltrados.Any())
            {
                model.SubEstaciones.AddRange(cuadrosFiltrados);
            }
            else
            {
                model.SubEstaciones.Add(new AreaDTO { AREACODI = 0, AREANOMB = "-- TODOS --" });
            }
            return PartialView("CargarSubestaciones", model);
        }

        [HttpPost]
        public JsonResult GrabarEjecucionCuadroRechazoCarga(int codigoCuadroPrograma, string fechahoraInicio, string fechahoraFin)
        {
            var rcaCuadroProgDTO = new RcaCuadroProgDTO();

            rcaCuadroProgDTO.Rccuadcodi = codigoCuadroPrograma;
            rcaCuadroProgDTO.Rccuadfechorinicioejec = DateTime.ParseExact(fechahoraInicio, "dd/MM/yyyy HH:ss", null);
            rcaCuadroProgDTO.Rccuadfechorfinejec = DateTime.ParseExact(fechahoraFin, "dd/MM/yyyy HH:ss", null);
            //rcaCuadroProgDTO.Rccuadestado = "E";
            rcaCuadroProgDTO.Rcestacodi = ConstantesRechazoCarga.EstadoCuadroEjecutado;
            rcaCuadroProgDTO.Rccuadfecmodificacion = DateTime.Now;
            rcaCuadroProgDTO.Rccuadusumodificacion = User.Identity.Name;

            servicio.UpdateRcaCuadroProgramaEjecucion(rcaCuadroProgDTO);

            return Json(new Respuesta { Exito = true });
        }

        [HttpPost]
        public JsonResult ReprogramarCuadroRechazoCarga(int codigoCuadroPrograma, int codigoPrograma, string codigoReprograma, string fechaInicio)
        {
            var rcaCuadroProgDTO = new RcaCuadroProgDTO();
            var rcaProgramaDTO = new RcaProgramaDTO();

            //Genera el codigo de reprograma
            DateTime fecha = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null);
            codigoReprograma = "PRC-RDO-" + fecha.ToString("yyyy-MM-dd");

            //Validamos si el reprograma existe, si es asi asociamos el cuadro a dicho reprograma
            //sino creamos el reprograma y asociamos dicho reprograma
            var programaExistente = servicio.GetByCriteriaRcaProgramas(codigoReprograma);
            int nuevoCodigoPrograma = 0;
            if (programaExistente.Count == 0)
            {
                //Obtenemos los datos del Programa Anterior
                //var programaAnterior = servicio.GetByIdRcaPrograma(codigoPrograma);
                //Nuevo Codigo Programa
                rcaProgramaDTO.Rcprogcodi = 0;
                rcaProgramaDTO.Rchorpcodi = ConstantesRechazoCarga.HorizonteReprograma;
                rcaProgramaDTO.Rcprogabrev = codigoReprograma;
                rcaProgramaDTO.Rcprognombre = string.Empty;
                rcaProgramaDTO.Rcprogcodipadre = codigoPrograma;
                rcaProgramaDTO.Rcprogestregistro = "1";
                rcaProgramaDTO.Rcprogfecinicio = fecha;
                rcaProgramaDTO.Rcprogfecfin = fecha;
                rcaProgramaDTO.Rcprogfeccreacion = DateTime.Now;
                rcaProgramaDTO.Rcprogusucreacion = User.Identity.Name;

                rcaProgramaDTO.Rcprogestado = "1";

                nuevoCodigoPrograma = servicio.SaveRcaPrograma(rcaProgramaDTO);
                //return Json(new Respuesta { Exito = false, Mensaje = "-1" });
            }
            else
            {
                nuevoCodigoPrograma = programaExistente.First().Rcprogcodi;
            }

            
            //Obtenemos los datos del cuadro de Programa Anterior
            var cuadroProgramaAnterior = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);

            rcaCuadroProgDTO.Rccuadcodi = 0;
            rcaCuadroProgDTO.Rcprogcodi = nuevoCodigoPrograma;
            rcaCuadroProgDTO.Rccuadenergiaracionar = cuadroProgramaAnterior.Rccuadenergiaracionar;
            rcaCuadroProgDTO.Rccuadumbral = cuadroProgramaAnterior.Rccuadumbral;
            rcaCuadroProgDTO.Rccuadmotivo = cuadroProgramaAnterior.Rccuadmotivo;
            rcaCuadroProgDTO.Rccuadubicacion = string.IsNullOrEmpty(cuadroProgramaAnterior.Rccuadubicacion) ? "" : cuadroProgramaAnterior.Rccuadubicacion;
            rcaCuadroProgDTO.Rccuadbloquehor = cuadroProgramaAnterior.Rccuadbloquehor;
            rcaCuadroProgDTO.Rcconpcodi = cuadroProgramaAnterior.Rcconpcodi;
            rcaCuadroProgDTO.Rccuadflageracmf = cuadroProgramaAnterior.Rccuadflageracmf;
            rcaCuadroProgDTO.Rccuadflageracmt = cuadroProgramaAnterior.Rccuadflageracmt;
            rcaCuadroProgDTO.Rccuadflagregulado = cuadroProgramaAnterior.Rccuadflagregulado;
            rcaCuadroProgDTO.Rccuadfechorinicio = cuadroProgramaAnterior.Rccuadfechorinicio;
            rcaCuadroProgDTO.Rccuadfechorfin = cuadroProgramaAnterior.Rccuadfechorfin;
            rcaCuadroProgDTO.Rccuadestregistro = "1";            
            rcaCuadroProgDTO.Rcestacodi = ConstantesRechazoCarga.EstadoCuadroReprogramado;
            rcaCuadroProgDTO.Rccuadfeccreacion = DateTime.Now;
            rcaCuadroProgDTO.Rccuadusucreacion = User.Identity.Name;
            rcaCuadroProgDTO.Rccuadcodipadre = codigoCuadroPrograma;

            var nuevoCuadroPrograma = servicio.SaveRcaCuadroProg(rcaCuadroProgDTO);

            //Creamos el Cuadro de Programa Usuario con los datos del cuadro de Programa Anterior

            var listaCuadroProgUsuario = new List<RcaCuadroProgUsuarioDTO>();
            listaCuadroProgUsuario = servicio.ListProgramaRechazoCarga("", codigoCuadroPrograma.ToString());

            foreach (var registro in listaCuadroProgUsuario)
            {
                var rcaCuadroProgUsuario = new RcaCuadroProgUsuarioDTO();

                rcaCuadroProgUsuario.Rcproufuente = registro.Rcproufuente;
                rcaCuadroProgUsuario.Rcproucargaesencial = registro.Rcproucargaesencial;
                rcaCuadroProgUsuario.Rcproudemanda = registro.Rcproudemanda;
                rcaCuadroProgUsuario.Rcproudemandareal = registro.Rcproudemandareal;

                rcaCuadroProgUsuario.Rcproucargadisponible = registro.Rcproucargadisponible;
                rcaCuadroProgUsuario.Rcproufactork = registro.Rcproufactork;
                rcaCuadroProgUsuario.Rcproudemandaracionar = registro.Rcproudemandaracionar;
                rcaCuadroProgUsuario.Rcproudemandaatender = registro.Rcproudemandaatender;
                rcaCuadroProgUsuario.Rcproucodi = 0;
                rcaCuadroProgUsuario.Emprcodi = registro.Emprcodi;
                rcaCuadroProgUsuario.Equicodi = registro.Equicodi;
                rcaCuadroProgUsuario.Rcprouemprcodisuministrador = registro.Rcprouemprcodisuministrador;

                rcaCuadroProgUsuario.Rccuadcodi = nuevoCuadroPrograma;
                rcaCuadroProgUsuario.Rcproufeccreacion = DateTime.Now;
                rcaCuadroProgUsuario.Rcprouusucreacion = User.Identity.Name;
                rcaCuadroProgUsuario.Rcprouestregistro = "1";
                
                servicio.SaveRcaCuadroProgUsuario(rcaCuadroProgUsuario);
            }

            //Actualiza el estado de cuadro de programa a No Ejecutado
            var cuadroProg = new RcaCuadroProgDTO();

            cuadroProg.Rccuadcodi = codigoCuadroPrograma;
            cuadroProg.Rcestacodi = ConstantesRechazoCarga.EstadoCuadroNoEjecutado;
            cuadroProg.Rccuadfecmodificacion = DateTime.Now;
            cuadroProg.Rccuadusumodificacion = User.Identity.Name;

            this.servicio.UpdateRcaCuadroProgEstado(cuadroProg);

            

            return Json(new Respuesta { Exito = true });
        }
        
        [HttpPost]
        public JsonResult ObtenerCuadroProgDistribuidores(int codigoCuadroPrograma)
        {
            ProgramaRechazoCargaModel model = new ProgramaRechazoCargaModel();
            model.ListSiEmpresa = this.servicio.ListaEmpresasRechazoCarga("", _tipoEmpresaDistribuidor, _estadoRegistroEmpresaActivo).OrderBy(p => p.Emprrazsocial).ToList();           

            var listaCuadroProgramaDistribuidores = servicio.ListCuadroProgDistrib(codigoCuadroPrograma);

            int registrosTotal = 7 + (listaCuadroProgramaDistribuidores.Count == 0 ? 1 : listaCuadroProgramaDistribuidores.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = PintarCeldas(data, 5);
            int indice = 0;

            foreach (var item in listaCuadroProgramaDistribuidores)
            {
                data[indice][0] = item.Rcprodcodi.ToString();
                data[indice][1] = item.Emprcodi > 0 ? item.Emprcodi.ToString() : "";
                data[indice][2] = item.Rcprodsubestacion;
                data[indice][3] = item.Rcproddemanda.ToString();
                data[indice][4] = item.Rcprodcargarechazar.ToString();
                                
                indice++;
            }

            model.DatosMatriz = data;
            model.Registro = indice;

            return Json(model);

        }        

        private string[][] FormatearDatosDistribPantalla(List<RcaCuadroProgDistribDTO> listaCuadroProgramaDistribuidores)
        {
            int registrosTotal = 7 + (listaCuadroProgramaDistribuidores.Count == 0 ? 1 : listaCuadroProgramaDistribuidores.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = PintarCeldas(data, 5);
            int indice = 0;

            foreach (var item in listaCuadroProgramaDistribuidores)
            {
                data[indice][0] = item.Rcprodcodi.ToString();
                data[indice][1] = item.Emprcodi > 0 ? item.Emprcodi.ToString() : "";
                data[indice][2] = item.Rcprodsubestacion;
                data[indice][3] = item.Rcproddemanda > 0 ? item.Rcproddemanda.ToString() : "";
                data[indice][4] = item.Rcprodcargarechazar > 0 ? item.Rcprodcargarechazar.ToString() : "";

                indice++;
            }

            return data;
        }

        /// <summary>
        /// Permite pintar la data por defecto
        /// </summary>
        /// <param name="data">Matriz de datos</param>
        /// <param name="columna">Número de columnas</param>
        /// <returns></returns>
        private string[][] PintarCeldas(string[][] data, int columna)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new string[columna];

                for (int j = 0; j < columna; j++)
                {
                    data[i][j] = string.Empty;
                }
            }

            return data;
        }

        /// <summary>
        /// Permite eliminar el registro de la congestion
        /// </summary>
        /// <param name="idCongestion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDistribuidor(int rcprodcodi)
        {
            try
            {
                this.servicio.DeleteRcaCuadroProgDistrib(rcprodcodi);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        #region Reportes SEV

        [HttpGet]
        public virtual ActionResult DescargarReporte(string file)
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, file);
        }

        public JsonResult GenerarReporte(int codigoCuadroPrograma, int reporteId, string eventoCTAF)
        {
            var archivo = "-1";

            switch (reporteId)
            {
                case 1:
                    {
                        var listaCuadroRechazoCarga = servicio.ReporteTotalDatos(0, eventoCTAF);
                        archivo = Helper.ExcelHelper.GenerarReporteTotalDatos(listaCuadroRechazoCarga, eventoCTAF); break;
                    }
                case 2:
                    {
                        var listaCuadroRechazoCarga = servicio.ReporteEvaluacionCumplimiento(0, eventoCTAF);
                        archivo = Helper.ExcelHelper.GenerarReporteCumplimientoRMC(listaCuadroRechazoCarga, eventoCTAF); break;
                    }
                case 3:
                    {
                        var listaCuadroRechazoCarga = servicio.ReporteInterrrupInformeTecnico(0, eventoCTAF);
                        archivo = Helper.ExcelHelper.GenerarReporteInterrupcionesSuministroInformeTecnico(listaCuadroRechazoCarga, eventoCTAF); break;
                    }
                case 4:
                    {
                        var listaCuadroRechazoCarga = servicio.ReporteDemoraEjecucion(0, eventoCTAF);
                        archivo = Helper.ExcelHelper.GenerarReporteDemorasEjecucionRMC(listaCuadroRechazoCarga, eventoCTAF); break;
                    }
                case 5:
                    {
                        var listaCuadroRechazoCarga = servicio.ReporteDemoraReestablecimiento(0, eventoCTAF);
                        archivo = Helper.ExcelHelper.GenerarReporteDemorasReestablecimiento(listaCuadroRechazoCarga, eventoCTAF); break;
                    }
                case 6:
                    {
                        var listaCuadroRechazoCarga = servicio.ReporteInterrupcionesMenores(0, eventoCTAF);
                        archivo = Helper.ExcelHelper.GenerarReporteInterrupcionesMenores(listaCuadroRechazoCarga, eventoCTAF); break;
                    }
                case 7:
                    {
                        var listaCuadroRechazoCarga = servicio.ReporteDemorasFinalizar(0, eventoCTAF);
                        archivo = Helper.ExcelHelper.GenerarReporteInterrupcionesSuministroDecision(listaCuadroRechazoCarga, eventoCTAF); break;
                    }
                case 8:
                    {
                        var listaCuadroRechazoCarga = servicio.ReporteDemorasResarcimiento(0, eventoCTAF);
                        archivo = Helper.ExcelHelper.GenerarReporteInterrupcionesResarcimiento(listaCuadroRechazoCarga, eventoCTAF); break;
                    }
            }

            return Json(archivo);
        }

        public FileResult GenerarReporteWord(int reporteId, string evento)
        {
            byte[] bytes = new byte[0];

            var fileName = "Reporte.docx";
            try
            {
                switch (reporteId)
                {
                    case 1:
                        {
                            var listaCuadroRechazoCarga = servicio.ReporteTotalDatos(0, evento);
                            bytes = Helper.WordHelper.GenerarReporteTotalDatos(listaCuadroRechazoCarga, evento); fileName = "Total de Datos.docx"; break;
                        }
                    case 2:
                        {
                            var listaCuadroRechazoCarga = servicio.ReporteEvaluacionCumplimiento(0, evento);
                            bytes = Helper.WordHelper.GenerarReporteEvaluacionCumplimiento(listaCuadroRechazoCarga, evento); fileName = "Evaluacion Cumplimiento.docx"; break;
                        }
                    case 3:
                        {
                            var listaCuadroRechazoCarga = servicio.ReporteInterrrupInformeTecnico(0, evento);
                            bytes = Helper.WordHelper.GenerarReporteInterInformeTecnico(listaCuadroRechazoCarga, evento); fileName = "Interrupciones Informe Tecnico.docx"; break;
                        }
                    case 4:
                        {
                            var listaCuadroRechazoCarga = servicio.ReporteDemoraEjecucion(0, evento);
                            bytes = Helper.WordHelper.GenerarReporteDemorasEjecucionRMC(listaCuadroRechazoCarga, evento); fileName = "Demoras Ejecucion RMC.docx"; break;
                        }
                    case 5:
                        {
                            var listaCuadroRechazoCarga = servicio.ReporteDemoraReestablecimiento(0, evento);
                            bytes = Helper.WordHelper.GenerarReporteDemorasReestablecimiento(listaCuadroRechazoCarga, evento); fileName = "Demoras Reestablecimiento.docx"; break;
                        }
                    case 6:
                        {
                            var listaCuadroRechazoCarga = servicio.ReporteInterrupcionesMenores(0, evento);
                            bytes = Helper.WordHelper.GenerarReporteInterrupcionesMenores(listaCuadroRechazoCarga, evento); fileName = "Interrupciones Menores.docx"; break;
                        }
                    case 7:
                        {
                            var listaCuadroRechazoCarga = servicio.ReporteDemorasFinalizar(0, evento);
                            bytes = Helper.WordHelper.GenerarReporteInterrupcionesSuministroDecision(listaCuadroRechazoCarga, evento); fileName = "Interrupciones Suministro Decision.docx"; break;
                        }
                    case 8:
                        {
                            var listaCuadroRechazoCarga = servicio.ReporteDemorasResarcimiento(0, evento);
                            bytes = Helper.WordHelper.GenerarReporteInterrupcionesResarcimiento(listaCuadroRechazoCarga, evento); fileName = "Interrupciones Resarcimiento.docx"; break;
                        }
                }

            }
            catch(Exception ex)
            {
                log.Error("GenerarReporteWord", ex);

                bytes = Helper.WordHelper.GenerarReporteBlanco();
            }
            
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }

        #endregion

    }   
}
