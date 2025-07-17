using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.RechazoCarga.Helper;
using COES.MVC.Extranet.Areas.RechazoCarga.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Models;
using COES.MVC.Extranet.Helper;
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
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Globalization;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Controllers
{
    public class EnvioArchivosMagnitudController : BaseController
    {
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        FormatoMedicionAppServicio formatoMedicion = new FormatoMedicionAppServicio();
        GeneralAppServicio servGeneral = new GeneralAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioArchivosMagnitudController));


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

        #region Propiedades
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

        public String NombreArchivoDetalle
        {
            get
            {
                return (Session[DatosSesionRechazoCarga.SesionNombreArchivoDetalle] != null) ?
                    Session[DatosSesionRechazoCarga.SesionNombreArchivoDetalle].ToString() : null;
            }
            set { Session[DatosSesionRechazoCarga.SesionNombreArchivoDetalle] = value; }
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

        public String RutaCompletaArchivoDetalle
        {
            get
            {
                return (Session[DatosSesionRechazoCarga.RutaCompletaArchivoDetalle] != null) ?
                    Session[DatosSesionRechazoCarga.RutaCompletaArchivoDetalle].ToString() : null;
            }
            set { Session[DatosSesionRechazoCarga.RutaCompletaArchivoDetalle] = value; }
        }

        public int IdEmpresa
        {
            get
            {
                return (Session["sIdEmpresa"] != null) ?
                    (int)Session["sIdEmpresa"] : 0;
            }
            set { Session["sIdEmpresa"] = value; }
        }

        /// <summary>
        /// Codigo del envio
        /// </summary>
        public int IdEnvio
        {
            get
            {
                return (Session[DatosSesionRechazoCarga.SesionIdEnvio] != null) ?
                    (int)Session[DatosSesionRechazoCarga.SesionIdEnvio] : 0;
            }
            set { Session[DatosSesionRechazoCarga.SesionIdEnvio] = value; }
        } 

        #endregion

        public ActionResult Inicio()
        {
            EnvioArchivosMagnitudModel model = new EnvioArchivosMagnitudModel();

            List<RcaSuministradorDTO> suministradores = new List<RcaSuministradorDTO>();
            //suministradores.Add(new RcaSuministradorDTO { Emprcodi = 0, Emprrazsocial = "--SELECCIONE--" });
            var listaSuministradores = servicio.ListRcaSuministradores();
            //if (listaSuministradores.Any())
            //{
            //    suministradores.AddRange(listaSuministradores);
            //}
            #region Validacion de Suministrador
            bool permisoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);

            if (permisoEmpresas)
            {
                model.Suministradores = this.seguridad.ListarEmpresas().Where(x => listaSuministradores.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
                    OrderBy(x => x.EMPRNOMB).Select(x => new RcaSuministradorDTO
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprrazsocial = x.EMPRNOMB
                        
                    }).ToList();
            }
            else
            {
                model.Suministradores = this.seguridad.ObtenerEmpresasActivasPorUsuario(User.Identity.Name).
                    Where(x => listaSuministradores.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).OrderBy(x => x.EMPRRAZSOCIAL).Select(x => new RcaSuministradorDTO
                    {
                        Emprcodi = x.EMPRCODI,                        
                        Emprrazsocial = x.EMPRRAZSOCIAL
                    }).ToList();
            }
            //model.Suministradores = listaSuministradores;//quitar despues
            model.Suministradores.Add(new RcaSuministradorDTO { Emprcodi = 0, Emprrazsocial = "--SELECCIONE--" });
            #endregion

            List<RcaProgramaDTO> programas = new List<RcaProgramaDTO>();
            programas.Add(new RcaProgramaDTO { Rcprogcodi = 0, Rcprognombre = "--SELECCIONE--" });
            //var listaProgramas = servicio.ListProgramaEnvioArchivo();
            //if (listaProgramas.Any())
            //{
            //    programas.AddRange(listaProgramas);
            //}

            model.Programas = programas;
            //model.Suministradores = suministradores;

            return View(model);
        }

        public JsonResult ObtenerCuadrosPorPrograma(string programa)
        {
            List<RcaCuadroProgDTO> cuadros = new List<RcaCuadroProgDTO>();
            cuadros.Add(new RcaCuadroProgDTO { Rccuadcodi = 0, Rccuadmotivo = "--SELECCIONE--" });
            if (programa == "0") return Json(new SelectList(cuadros, "Rccuadcodi", "Rccuadmotivo"));
            var cuadrosFiltrados = servicio.GetByCriteriaRcaCuadroProgs(programa, ConstantesRechazoCarga.EstadoCuadroEjecutado.ToString());
            if (cuadrosFiltrados.Any())
            {
                cuadros.AddRange(cuadrosFiltrados);
            }
            return Json(new SelectList(cuadros, "Rccuadcodi", "Rccuadmotivo"));
        }

        public JsonResult ObtenerProgramas(string tipo)
        {
            List<RcaProgramaDTO> programas = new List<RcaProgramaDTO>();
            programas.Add(new RcaProgramaDTO { Rcprogcodi = 0, Rcprognombre = "--SELECCIONE--" });
            if (string.IsNullOrEmpty(tipo)) return Json(new SelectList(programas, "Rcprogcodi", "Rcprognombre"));

            var fechaReferencia = DateTime.Now;
            if (tipo.Equals("D"))
            {
                DateTime fechaActual = DateTime.Now;
                fechaReferencia = servGeneral.FechaEnDiasHabiles(fechaActual, -1 * ConstantesRechazoCarga.DiasEspera);
            }
            else
            {
                fechaReferencia = fechaReferencia.AddHours(-1 * ConstantesRechazoCarga.HorasEspera);
            }

            var listaProgramas = servicio.ListProgramaEnvioArchivo(fechaReferencia);
            if (listaProgramas.Any())
            {
                programas.AddRange(listaProgramas);
            }
            return Json(new SelectList(programas, "Rcprogcodi", "Rcprognombre"));
        }

        [HttpPost]
        public JsonResult ObtenerFormatoModelArchivosMagnitud(string programa, string cuadro, string suministrador, string tipo)
        {
            EnvioArchivosMagnitudModel modelEnvio = new EnvioArchivosMagnitudModel();
            FormatoModel model = new FormatoModel();
            suministrador = suministrador.Equals("0") ? string.Empty : suministrador;
            var registros = servicio.ListFiltro(programa, cuadro, suministrador, tipo);
            //var registros = new List<RcaCuadroEjecUsuarioDTO>();
            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            model.Handson.ListaMerge = GenerarMerges();
            model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 1;
            model.Formato.Formatcols = 9;
            model.FilasCabecera = model.Formato.Formatrows;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.Handson.ListaColWidth = new List<int> { 280, 140, 200, 80, 100, 100, 100, 120, 120 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true, true };
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            model.Handson.ListaExcelData = GenerarData(registros);

            modelEnvio.formatoClientes = model;

            var listaClientes = new List<RcaCuadroEjecUsuarioDTO>();
            listaClientes.Add(new RcaCuadroEjecUsuarioDTO { Rcproucodi = 0, Empresa = "-- SELECCIONE --" });

            if(registros.Where(p => p.Rcejeucargarechazada > 0).Count()> 0)
            {
                foreach(var item in registros.Where(p => p.Rcejeucargarechazada > 0))
                {
                    var cliente = new RcaCuadroEjecUsuarioDTO();
                    cliente.Rcproucodi = item.Rcproucodi;
                    cliente.Empresa = item.Empresa + " " + item.Puntomedicion;

                    listaClientes.Add(cliente);
                }                
            }

            modelEnvio.ListClientes = listaClientes;

            return Json(modelEnvio);
        }
        private List<CeldaMerge> GenerarMerges()
        {
            return new List<CeldaMerge>{
               new CeldaMerge{row=0, col=6, colspan=3, rowspan=1},               
           };
        }
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + NombreArchivoRechazoCarga.CuadroEjecucionUsuario;
            return File(fullPath, Constantes.AppExcel, NombreArchivoRechazoCarga.CuadroEjecucionUsuario);
        }
        private List<string> ObtenerTitulosColumnas()
        {
            return new List<string>() { "Razón Social", "Subestación", "Nombre Punto Medición", "Demanda (MW)", "Fuente", "Rechazo Carga Programado", 
                "Rechazo Carga Ejecutado", "Fecha Hora Inicio dd/mm/yyyy hh:mm", "Fecha Hora Fin dd/mm/yyyy hh:mm", "RCEJEUCODI", "RCPROUCODI","EQUICODI" };
        }
        private string[][] GenerarData(List<RcaCuadroEjecUsuarioDTO> registros)
        {
            var filas = registros.Count + 2;
            var cabecera = ObtenerTitulosColumnas();
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];
                if (i == 0)
                {
                    matriz[i][6] = "Ejecución";                    
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
                    matriz[i][1] = registros[i - 2].Subestacion;
                    matriz[i][2] = registros[i - 2].Puntomedicion;
                    matriz[i][3] = registros[i - 2].Rcproudemandareal.ToString("######0.#0");
                    matriz[i][4] = !string.IsNullOrEmpty(registros[i - 2].Rcproufuente) ? registros[i - 2].Rcproufuente.ToString() : string.Empty;
                    matriz[i][5] = registros[i - 2].Rcproudemandaracionar.ToString("######0.#0");
                    matriz[i][6] =  registros[i - 2].Rcejeucargarechazada.ToString("######0.#0");
                    matriz[i][7] = registros[i - 2].Rcejeufechorinicio != DateTime.MinValue ? registros[i - 2].Rcejeufechorinicio.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                    matriz[i][8] = registros[i - 2].Rcejeufechorfin != DateTime.MinValue ? registros[i - 2].Rcejeufechorfin.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                    matriz[i][9] = registros[i - 2].Rcejeucodi.ToString();
                    matriz[i][10] = registros[i - 2].Rcproucodi.ToString();
                    matriz[i][11] = registros[i - 2].Equicodi.ToString();
                }
            }
            return matriz;
        }

        private List<string> ObtenerTitulosColumnasDetalle()
        {
            return new List<string>() { "FECHA Y HORA dd/mm/yyyy hh:mm", "POTENCIA (MW)" };
        }
        private string[][] GenerarDataDetalle(List<RcaCuadroEjecUsuarioDetDTO> registros)
        {
            var filas = registros.Count + 1;
            var cabecera = ObtenerTitulosColumnasDetalle();
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];                
                if (i == 0)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        matriz[i][j] = cabecera[j];
                    }
                }
                if (i > 0)
                {                   
                    matriz[i][0] = registros[i - 1].Rcejedfechor != DateTime.MinValue ? registros[i - 1].Rcejedfechor.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                    matriz[i][1] = registros[i - 1].Rcejedpotencia.ToString("N3");
                    //matriz[i][2] = registros[i - 1].Rcejedcodi.ToString();
                   
                }
            }
            return matriz;
        }

        [HttpPost]
        public JsonResult GenerarFormato(string programa, string cuadro, string suministrador, string tipo ,string datos)
        {
            int indicador = 0;
            try
            {
                FormatoModel model = FormatearModeloDescargaArchivo(datos);//FormatearModeloDesdeParametros(programa, cuadro, suministrador, tipo, esConsulta);
                GenerarArchivoExcel(model, 2);
                indicador = 1;
            }
            catch(Exception ex)
            {
                Log.Error("GenerarFormato", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        private FormatoModel FormatearModeloDesdeParametros(string programa, string cuadro, string suministrador, string tipo, bool esConsulta)
        {
            FormatoModel model = new FormatoModel();


            var registros = new List<RcaCuadroEjecUsuarioDTO>();

            registros = servicio.ListFiltro(programa, cuadro, suministrador, tipo);

            ConfigurarFormatoModelo(model);
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            model.Handson.ListaExcelData = GenerarData(registros);
            return model;
        }

        private FormatoModel FormatearModeloDescargaArchivo(string datos)
        {
            FormatoModel model = new FormatoModel();


            var registros = new List<RcaCuadroEjecUsuarioDTO>();

            registros = FormatearDatosArchivoMagnitud(datos).ToList(); 

            ConfigurarFormatoModelo(model);
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            model.Handson.ListaExcelData = GenerarData(registros);
            return model;
        }

        private void ConfigurarFormatoModelo(FormatoModel model)
        {
            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            model.Handson.ListaMerge = GenerarMerges();
            model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 1;
            model.Formato.Formatcols = 10;
            model.FilasCabecera = model.Formato.Formatrows;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.Handson.ListaColWidth = new List<int> { 280, 140, 200, 80, 100, 100, 100, 120, 120 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true, true };
        }

        private void GenerarArchivoExcel(FormatoModel model, int columnasOcultas)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();
            FileInfo newFile = new FileInfo(ruta + NombreArchivoRechazoCarga.CuadroEjecucionUsuario);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivoRechazoCarga.CuadroEjecucionUsuario);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                var filas = model.Handson.ListaFilaReadOnly.Count;
                var columnas = model.ColumnasCabecera + columnasOcultas;
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

                        if (i == 1 || (i == 0 && j > 5))
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

                            if (j <= 2)
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }

                            if (j==3 || j == 5 || j==6 )
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[i + 1, j + 1].Style.Numberformat.Format = @"0.00";
                            }

                            if (j == 7 || j == 8)
                            {
                                var fecha = DateTime.MinValue;
                                if(!DateTime.TryParseExact(model.Handson.ListaExcelData[i][j], Constantes.FormatoFechaHora, null, DateTimeStyles.None,
                                out fecha))
                                {
                                    ws.Cells[i + 1, j + 1].Value = "";
                                }


                                ws.Cells[i + 1, j + 1].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                            }
                        }
                    }
                }

                ws.Column(1).Width = 50;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 40;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 18;
                ws.Column(9).Width = 18;
                //ws.Column(10).Width = 15;

                ws.Column(10).Hidden = true;
                ws.Column(11).Hidden = true;
                ws.Column(12).Hidden = true;

                foreach (var reg in model.Handson.ListaMerge)
                {
                    ws.Cells[reg.row + 1, reg.col + 1, reg.row + reg.rowspan, reg.col + reg.colspan].Merge = true;
                }

                xlPackage.Save();
            }
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
            catch(Exception ex)
            {
                Log.Error("Subir", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LeerExcelSubido()
        {
            try
            {
                var titulos = ObtenerTitulosColumnas();
                Respuesta matrizValida;
                var matrizDatos = COES.MVC.Extranet.Areas.RechazoCarga.Helper.FormatoHelper.LeerExcelCargadoCabecera(this.RutaCompletaArchivo, titulos, 2, out matrizValida);
                if (matrizValida.Exito)
                {
                    FormatoModel model = FormatearModeloDesdeMatriz(matrizDatos);
                    COES.MVC.Extranet.Areas.RechazoCarga.Helper.FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true, Datos = model });
                }
                else
                {
                    return Json(matrizValida);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Subir", ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        private FormatoModel FormatearModeloDesdeMatriz(string[][] matrizDatos)
        {
            FormatoModel model = new FormatoModel();
            ConfigurarFormatoModelo(model);
            model.Handson.ListaExcelData = matrizDatos;
            return model;
        }

        [HttpPost]
        public JsonResult GrabarDatosArchivosMagnitud(string datos, int codigoSuministrador, string tipo, int codigoPrograma, int codigoCuadroPrograma)            
        {
            EnvioArchivosMagnitudModel model = new EnvioArchivosMagnitudModel();

            try
            {
                base.ValidarSesionUsuario();

                var datosCuadroEjecUsuario = FormatearDatosArchivoMagnitud(datos);

                //Validacion de Datos antes de enviar
                //var listaErrores = ValidarDatos(datosCuadroEjecUsuario.ToList(), datos, codigoCuadroPrograma);

                //if (listaErrores.Count > 0)
                //{
                //    model.Exito = false;
                //    model.ListaErrores = listaErrores;

                //    return Json(model);
                //}

                ///////////////Grabar Envio//////////////////////////
                string mensajePlazo = string.Empty;
                //Boolean enPlazo = true;//ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = codigoSuministrador;
                envio.Enviofecha = DateTime.Now;
                envio.Enviofechaperiodo = DateTime.Now;//formato.FechaInicio;
                envio.Envioplazo = "P";//(enPlazo) ? "P" : "F";
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Lastdate = DateTime.Now;
                envio.Lastuser = User.Identity.Name;
                envio.Userlogin = User.Identity.Name;
                //envio.Formatcodi = this.IdFormato;
                //this.IdEnvio = logic.SaveMeEnvio(envio);
                //model.IdEnvio = this.IdEnvio;
                ///////////////////////////////////////////////////////
                if (datosCuadroEjecUsuario.Count > 0)
                {
                    GrabarCuadroProgramaEjecUsuario(datosCuadroEjecUsuario, codigoSuministrador, tipo, codigoPrograma,
                        codigoCuadroPrograma, this.IdEnvio, User.Identity.Name);

                    envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                    envio.Enviocodi = this.IdEnvio;
                    envio.Cfgenvcodi = 0;//idConfig;
                                         //logic.UpdateMeEnvio(envio);
                }

                model.Exito = true;
            }
            catch(Exception ex)
            {
                Log.Error("EnvioArchivosMagnitudController", ex);
                model.Exito = false;
                model.MensajeError = ex.Message;
            }
            return Json(model);

            //return Json(new Respuesta { Exito = true });
        }

        private ICollection<RcaCuadroEjecUsuarioDTO> FormatearDatosArchivoMagnitud(string datos)
        {
            var cont = 2;
            var filasCabecera = 2;
            var columnas = 12;
            var celdas = datos.Split(',');
            var parametros = new List<RcaCuadroEjecUsuarioDTO>();
            var inicioDatos = filasCabecera * columnas;
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                cont++;
                var parametro = new RcaCuadroEjecUsuarioDTO();
                parametro.Empresa = celdas[i].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                parametro.Subestacion = celdas[i + 1].Replace(@"\", "").Replace("\"", "");
                parametro.Puntomedicion = celdas[i + 2].Replace(@"\", "").Replace("\"", "");
                parametro.Rcproudemandareal = Convert.ToDecimal(celdas[i + 3].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                parametro.Rcproufuente = celdas[i + 4].Replace(@"\", "").Replace("\"", "");
                parametro.Rcproudemandaracionar = Convert.ToDecimal(celdas[i + 5].Replace(@"\", "").Replace("\"", ""));
                //parametro.Rcproufactork = Convert.ToDecimal(celdas[i + 7].Replace(@"\", "").Replace("\"", ""));
                if (!string.IsNullOrEmpty(celdas[i + 6].Replace(@"\", "").Replace("\"", "")))
                {
                    parametro.Rcejeucargarechazada = Convert.ToDecimal(celdas[i + 6].Replace(@"\", "").Replace("\"", ""));
                }

                if (!string.IsNullOrEmpty(celdas[i + 7].Replace(@"\", "").Replace("\"", "")))
                {
                    var fecha = DateTime.MinValue;
                    var fechaInicio = celdas[i + 7].Replace(@"\", "").Replace("\"", "");
                    if (DateTime.TryParseExact(fechaInicio, Constantes.FormatoFechaHora, null, DateTimeStyles.None, out fecha))
                    {
                        //parametro.Rcejeufechorinicio = DateTime.ParseExact(celdas[i + 7].Replace(@"\", "").Replace("\"", ""), "dd/MM/yyyy HH:mm", null);
                        parametro.Rcejeufechorinicio = fecha;
                    }
                    
                    parametro.CeldaFechaInicio = "H" + cont.ToString();
                }

                if (!string.IsNullOrEmpty(celdas[i + 8].Replace(@"\", "").Replace("\"", "")))
                {
                    var fecha = DateTime.MinValue;
                    var fechaFin = celdas[i + 8].Replace(@"\", "").Replace("\"", "");
                    if (DateTime.TryParseExact(fechaFin, Constantes.FormatoFechaHora, null, DateTimeStyles.None, out fecha))
                    {
                        //parametro.Rcejeufechorinicio = DateTime.ParseExact(celdas[i + 7].Replace(@"\", "").Replace("\"", ""), "dd/MM/yyyy HH:mm", null);
                        parametro.Rcejeufechorfin = fecha;
                    }
                    //parametro.Rcejeufechorfin = DateTime.ParseExact(celdas[i + 8].Replace(@"\", "").Replace("\"", ""), "dd/MM/yyyy HH:mm", null);
                    parametro.CeldaFechaFin = "I" + cont.ToString();
                }
                //parametro.Rcejeucargarechazada = Convert.ToDecimal(celdas[i + 6].Replace(@"\", "").Replace("\"", ""));
                //parametro.Rcejeufechorinicio = DateTime.ParseExact(celdas[i + 7].Replace(@"\", "").Replace("\"", ""),"dd/MM/yyyy HH:mm",null);
                //parametro.Rcejeufechorfin = DateTime.ParseExact(celdas[i + 8].Replace(@"\", "").Replace("\"", ""), "dd/MM/yyyy HH:mm", null);
                parametro.Rcejeucodi = Convert.ToInt32(celdas[i + 9].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Rcproucodi = Convert.ToInt32(celdas[i + 10].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Equicodi = String.IsNullOrEmpty(celdas[i + 11].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 
                    0 : Convert.ToInt32(celdas[i + 11].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Rcprouemprcodisuministrador = String.IsNullOrEmpty(celdas[i + 13].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ? 0 : Convert.ToInt32(celdas[i + 13].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                //parametro.Emprcodi = Convert.ToInt32(celdas[i + 8].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                //parametro.Equicodi = Convert.ToInt32(celdas[i + 9].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                parametros.Add(parametro);

            }
            return parametros;
        }

        private void GrabarCuadroProgramaEjecUsuario(ICollection<RcaCuadroEjecUsuarioDTO> datosCuadroEjecUsuario, int codigoSuministrador, 
            string tipo, int codigoPrograma, int codigoCuadroPrograma, int idEnvio, string usuario)
        {           

            var listaOrigen = servicio.ListFiltro(codigoPrograma.ToString(), codigoCuadroPrograma.ToString(), codigoSuministrador.ToString(), tipo);
            listaOrigen = listaOrigen.Where(x => x.Rcejeucodi > 0).ToList();
            //var listaCambios = datosCuadroEjecUsuario.Where(x => x.Rcejeucodi > 0).ToList();

            int count = 0;
            var listaCambio = new List<MeCambioenvioDTO>();
            foreach (var entidad in datosCuadroEjecUsuario)
            {
                #region Registrar/Actualizar registro tabla RCA_CUADRO_EJECU_SUARIO
                if (entidad.Rcejeucodi > 0)
                {
                    //Actualización de RcaCuadroProgUsuario
                    //x.Rccuadcodi = rccuadcodi;
                    entidad.Rcejeufecmodificacion = DateTime.Now;
                    entidad.Rcejeuusumodificacion = User.Identity.Name;
                    entidad.Rcejeuestregistro = "1";//Temporal - revisar
                    entidad.Rcejeutiporeporte = tipo;//Temporal
                    //x.Emprcodi = x.Emprcodi == 0 ? 10614 : parametro.Emprcodi;
                    //x.Equicodi = x.Equicodi == 0 ? 17547 : parametro.Equicodi;
                    servicio.UpdateRcaCuadroEjecUsuario(entidad);
                }
                else
                {
                    //Inserción de RcaCuadroProgUsuario
                    //x.Rccuadcodi = rccuadcodi;
                    entidad.Rcejeufeccreacion = DateTime.Now;
                    entidad.Rcejeuusucreacion = User.Identity.Name;
                    entidad.Rcejeuestregistro = "1";//Temporal - revisar
                    entidad.Rcejeutiporeporte = tipo;
                    //x.Rcpareanio = Convert.ToInt32(anio);
                    servicio.SaveRcaCuadroEjecUsuario(entidad);
                }
                #endregion
                
                var regAnt = listaOrigen.Find(x => x.Equicodi == entidad.Equicodi && x.Rcejeucodi == entidad.Rcejeucodi);

                if (regAnt != null)
                {
                    List<string> filaValores = new List<string>();
                    List<string> filaValoresOrigen = new List<string>();
                    List<string> filaCambios = new List<string>();

                    #region Campo Rechazado Carga Ejecutado
                    decimal? valorOrigen = (decimal?)regAnt.GetType().GetProperty("Rcejeucargarechazada").GetValue(regAnt, null);
                    decimal? valorModificado = (decimal?)entidad.GetType().GetProperty("Rcejeucargarechazada").GetValue(entidad, null);

                    if (valorModificado != null)
                    {
                        filaValores.Add(valorModificado.ToString());
                    }
                    else
                    {
                        filaValores.Add("");
                    }

                    if (valorOrigen != null)
                    {
                        filaValoresOrigen.Add(valorOrigen.ToString());
                    }
                    else
                    {
                        filaValoresOrigen.Add("");
                    }

                    if (valorOrigen != valorModificado)//&& valorOrigen != null && valorModificado != null)
                    {
                        count++;
                        filaCambios.Add("7");
                    }   
                    #endregion

                    #region Campo Fecha Inicio ejecucion
                    DateTime? valorOrigenFechaInicio = (DateTime?)regAnt.GetType().GetProperty("Rcejeufechorinicio").GetValue(regAnt, null);
                    DateTime? valorModificadoFechaInicio = (DateTime?)entidad.GetType().GetProperty("Rcejeufechorinicio").GetValue(entidad, null);

                    if (valorModificadoFechaInicio != null)
                    {
                        filaValores.Add(valorModificadoFechaInicio.Value.ToString("dd/MM/yyyy HH:mm"));
                    }
                    else
                    {
                        filaValores.Add("");
                    }

                    if (valorOrigenFechaInicio != null)
                    {
                        filaValoresOrigen.Add(valorOrigenFechaInicio.Value.ToString("dd/MM/yyyy HH:mm"));
                    }
                    else
                    {
                        filaValoresOrigen.Add("");
                    }

                    if (valorOrigenFechaInicio != valorModificadoFechaInicio)//&& valorOrigen != null && valorModificado != null)
                    {
                        count++;
                        filaCambios.Add("8");
                    }   

                    #endregion

                    #region Campo Fecha Fin ejecucion
                    DateTime? valorOrigenFechaFin = (DateTime?)regAnt.GetType().GetProperty("Rcejeufechorfin").GetValue(regAnt, null);
                    DateTime? valorModificadoFechaFin = (DateTime?)entidad.GetType().GetProperty("Rcejeufechorfin").GetValue(entidad, null);

                    if (valorModificadoFechaFin != null)
                    {
                        filaValores.Add(valorModificadoFechaFin.Value.ToString("dd/MM/yyyy HH:mm"));
                    }
                    else
                    {
                        filaValores.Add("");
                    }

                    if (valorOrigenFechaFin != null)
                    {
                        filaValoresOrigen.Add(valorOrigenFechaFin.Value.ToString("dd/MM/yyyy HH:mm"));
                    }
                    else
                    {
                        filaValoresOrigen.Add("");
                    }

                    if (valorOrigenFechaFin != valorModificadoFechaFin)//&& valorOrigen != null && valorModificado != null)
                    {
                        count++;
                        filaCambios.Add("9");
                    }

                    #endregion
                                        
                    if (filaCambios.Count > 0)
                    {
                        MeCambioenvioDTO cambio = new MeCambioenvioDTO();
                        cambio.Cambenvdatos = String.Join(",", filaValores);
                        cambio.Cambenvcolvar = String.Join(",", filaCambios);
                        cambio.Cambenvfecha = DateTime.Now;//(DateTime)reg.Medifecha;
                        cambio.Enviocodi = idEnvio;
                        //cambio.Formatcodi = formato.Formatcodi;
                        cambio.Ptomedicodi = entidad.Equicodi;
                        //cambio.Tipoinfocodi = reg.Tipoinfocodi;
                        cambio.Lastuser = usuario;
                        cambio.Lastdate = DateTime.Now;
                        listaCambio.Add(cambio);
                        
                    }
                }
            }

            if (listaCambio.Count > 0)
            {//Grabar Cambios
                //logic.GrabarCambios(listaCambio);
                //GrabarCambios(listaOrigen);
                //si es primer reenvio grabar valores origen

            }


        }

        private List<ErrorHelper> ValidarDatos(List<RcaCuadroEjecUsuarioDTO> listaClientes, string datos, int codigoCuadroPrograma)
        {
            var listaErrores = new List<ErrorHelper>();

            //validamos fecha inicio y fecha fin
            foreach (var cliente in listaClientes.Where(p=>p.Rcejeucargarechazada > 0))
            {
                if(cliente.Rcejeufechorinicio > cliente.Rcejeufechorfin)
                {
                    listaErrores.Add(new ErrorHelper { Error = "Fecha Fin debe ser mayor a Fecha Inicio", Celda = cliente.CeldaFechaFin });
                }
            }

            if(listaErrores.Count > 0)
            {
                return listaErrores;
            }

            ////Validamos la hora de inicio y fin con lo ingresado en el cuadro de programa

            //var listaClientesCuadro = servicio.ListProgramaRechazoCarga("", codigoCuadroPrograma.ToString());
            //var cuadroPrograma = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);

            //foreach(var cliente in listaClientes)
            //{
            //    if(listaClientesCuadro.Any(x=>x.Rcproucodi == cliente.Rcproucodi))
            //    {
            //        var clienteCuadroProg = listaClientesCuadro.Where(p => p.Rcproucodi == cliente.Rcproucodi).First();

            //        DateTime horaInicioCoordinado = string.IsNullOrEmpty(clienteCuadroProg.Rccuadhorinicoord) ?
            //            cuadroPrograma.Rccuadfechorinicio.Value : DateTime.ParseExact(clienteCuadroProg.Rccuadhorinicoord, "HH:mm", null);

            //        DateTime horaFinCoordinado = string.IsNullOrEmpty(clienteCuadroProg.Rccuadhorfincoord) ?
            //           cuadroPrograma.Rccuadfechorfin.Value : DateTime.ParseExact(clienteCuadroProg.Rccuadhorfincoord, "HH:mm", null);

            //        TimeSpan resIni = horaInicioCoordinado.TimeOfDay - cliente.Rcejeufechorinicio.TimeOfDay;
            //        if(resIni.TotalMinutes < 60)
            //        {
            //            listaErrores.Add(new ErrorHelper { Error = string.Format("Debe reportar con al menos una hora antes de {0}",horaInicioCoordinado.ToString("HH:mm")), Celda = cliente.CeldaFechaInicio });
            //        }

            //        TimeSpan resFin = cliente.Rcejeufechorfin.TimeOfDay - horaFinCoordinado.TimeOfDay;
            //        if (resFin.TotalMinutes < 60)
            //        {
            //            listaErrores.Add(new ErrorHelper { Error = string.Format("Debe reportar con al menos una hora posterior de {0}", horaFinCoordinado.ToString("HH:mm")), Celda = cliente.CeldaFechaFin });
            //        }

            //    }
            //}

            return listaErrores;
        }

        [HttpPost]
        public JsonResult ObtenerFormatoModelArchivosMagnitudDetalle(int codigoCuadroPrograma, int clienteId)
        {
            EnvioArchivosMagnitudModel modelEnvio = new EnvioArchivosMagnitudModel();
            FormatoModel model = new FormatoModel();

            var ejecucionUsuario = servicio.GetByCriteriaRcaCuadroEjecUsuarios(clienteId).FirstOrDefault();

            var registros = servicio.ListFiltro(ejecucionUsuario.Rcejeucodi);
            var cuadroPrograma = servicio.GetByIdRcaCuadroProg(codigoCuadroPrograma);

            DateTime horaInicioCoordinado;
            DateTime horaFinEjecutado;

            modelEnvio.RangoFechas = mensajeFecha(cuadroPrograma, ejecucionUsuario, out horaInicioCoordinado, out horaFinEjecutado);

            if (registros.Count == 0)
            {
                registros = FormatoFechasDetalle(horaInicioCoordinado, horaFinEjecutado);
            }

            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            //model.Handson.ListaMerge = GenerarMerges();
            //model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 1;
            model.Formato.Formatcols = 2;
            model.FilasCabecera = model.Formato.Formatrows;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.Handson.ListaColWidth = new List<int> { 150, 100 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true };
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            model.Handson.ListaExcelData = GenerarDataDetalle(registros);

            modelEnvio.formatoClientes = model;

            modelEnvio.fechaInicioDetalle = horaInicioCoordinado.ToString("dd/MM/yyyy HH:mm");
            modelEnvio.fechaFinDetalle = horaFinEjecutado.ToString("dd/MM/yyyy HH:mm");

            return Json(modelEnvio);
        }

        private string mensajeFecha(RcaCuadroProgDTO cuadroPrograma, RcaCuadroEjecUsuarioDTO ejecucionUsuario, out DateTime horaInicioCoordinado, out DateTime horaFinEjecutado)
        {
            var mensaje = "";

            var clienteCuadroProgUsuario = servicio.GetByIdRcaCuadroProgUsuario(ejecucionUsuario.Rcproucodi);

            horaInicioCoordinado = string.IsNullOrEmpty(clienteCuadroProgUsuario.Rccuadhorinicoord) ? cuadroPrograma.Rccuadfechorinicio.Value : 
                DateTime.ParseExact(cuadroPrograma.Rccuadfechorinicio.Value.ToString("dd/MM/yyyy") + " " + clienteCuadroProgUsuario.Rccuadhorinicoord, "dd/MM/yyyy HH:mm", null);

             horaFinEjecutado = (ejecucionUsuario.Rcejeufechorfin == DateTime.MinValue) ?
            cuadroPrograma.Rccuadfechorfin.Value : ejecucionUsuario.Rcejeufechorfin;

            horaInicioCoordinado = horaInicioCoordinado.AddHours(-1);
            horaFinEjecutado = horaFinEjecutado.AddHours(1);

            mensaje = "<strong>Fecha y hora Inicio:</strong> " + horaInicioCoordinado.ToString("dd/MM/yyyy HH:mm") 
                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Fecha y hora Fin:</strong> " + horaFinEjecutado.ToString("dd/MM/yyyy HH:mm");

            return mensaje;
        }

        private List<RcaCuadroEjecUsuarioDetDTO> FormatoFechasDetalle( DateTime horaInicioCoordinado, DateTime horaFinEjecutado)
        {
            var registros = new List<RcaCuadroEjecUsuarioDetDTO>();

            var minutosInicio = horaInicioCoordinado.Minute;
            var minutosFin = horaFinEjecutado.Minute;

            if(minutosInicio % 15 > 0)
            {
                minutosInicio = minutosInicio - (minutosInicio % 15);
            }

            var horaFinAgregar = 0;
            if (minutosFin > 45)
            {
                horaFinAgregar = 1;
                minutosFin = 0;
            }
            else if(minutosFin % 15 > 0)
            {
                minutosFin = minutosFin + (15 - (minutosFin % 15));
            }

            horaInicioCoordinado = new DateTime(horaInicioCoordinado.Year, horaInicioCoordinado.Month, horaInicioCoordinado.Day, horaInicioCoordinado.Hour, minutosInicio, 0);
            horaFinEjecutado = new DateTime(horaFinEjecutado.Year, horaFinEjecutado.Month, horaFinEjecutado.Day, horaFinEjecutado.Hour + horaFinAgregar, minutosFin, 0);

            for (int i=0; i < 96; i++)
            {
                var fecha = horaInicioCoordinado.AddMinutes(i * 15);
                registros.Add(new RcaCuadroEjecUsuarioDetDTO { Rcejedfechor = fecha, Rcejedpotencia = decimal.Zero });

                if(fecha >= horaFinEjecutado)
                {
                    break;
                }
            }

            return registros;
        }

        [HttpPost]
        public JsonResult GrabarDatosArchivosMagnitudDetalle(string datos, int codigoCuadroPrograma, int clienteId)
        {
            EnvioArchivosMagnitudModel model = new EnvioArchivosMagnitudModel();

            try 
            {                
                base.ValidarSesionUsuario();

                var datosCuadroEjecUsuario = FormatearDatosArchivoMagnitudDetalle(datos);

                if (datosCuadroEjecUsuario.Count > 0)
                {
                    GrabarCuadroProgramaEjecUsuarioDetalle(datosCuadroEjecUsuario.ToList(), User.Identity.Name, clienteId);
                }

                model.Exito = true;
            }
            catch (Exception ex)
            {
                Log.Error("EnvioArchivosMagnitudController", ex);
                model.Exito = false;
                model.MensajeError = ex.Message;
            }


            return Json(model);

            //return Json(new Respuesta { Exito = true });
        }

        private ICollection<RcaCuadroEjecUsuarioDetDTO> FormatearDatosArchivoMagnitudDetalle(string datos)
        {
            var cont = 1;
            var filasCabecera = 1;
            var columnas = 2;
            var celdas = datos.Split(',');
            var parametros = new List<RcaCuadroEjecUsuarioDetDTO>();
            var inicioDatos = filasCabecera * columnas;
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                cont++;
                var parametro = new RcaCuadroEjecUsuarioDetDTO();
                parametro.Rcejedfechor = DateTime.ParseExact(celdas[i].Replace(@"\", "").Replace("\"", "").Replace("[", ""), "dd/MM/yyyy HH:mm", null);
                parametro.Rcejedpotencia = String.IsNullOrEmpty(celdas[i + 1].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ?
                    decimal.Zero : decimal.Round(Convert.ToDecimal(celdas[i + 1].Replace(@"\", "").Replace("\"", "").Replace("]", "")), 3);
                //parametro.Rcejedcodi = String.IsNullOrEmpty(celdas[i + 2].Replace(@"\", "").Replace("\"", "").Replace("]", "")) ?
                //    0 : Convert.ToInt32(celdas[i + 2].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

               

                parametros.Add(parametro);

            }
            return parametros;
        }

        private void GrabarCuadroProgramaEjecUsuarioDetalle(List<RcaCuadroEjecUsuarioDetDTO> datosCuadroEjecUsuario,  string usuario, int rcproucodi)
        {
            var ejecucionUsuario = servicio.GetByCriteriaRcaCuadroEjecUsuarios(rcproucodi).FirstOrDefault();

            //Eliminamos ultimos registros ingresados

            servicio.DeletePorCliente(ejecucionUsuario.Rcejeucodi);            

            //Registramos la nueva carga de datos
            foreach (var entidad in datosCuadroEjecUsuario)
            {
                //Inserción de RcaCuadroProgUsuario
                entidad.Rcejeucodi = ejecucionUsuario.Rcejeucodi;
                entidad.Rcejedfeccreacion = DateTime.Now;
                entidad.Rcejedusucreacion = usuario;

                servicio.SaveRcaCuadroEjecDetUsuario(entidad);
            }

        }

        [HttpPost]
        public JsonResult GenerarFormatoDetalle(string datos)
        {
            int indicador = 0;
            try
            {
                FormatoModel model = FormatearModeloDescargaArchivoDetalle(datos);//FormatearModeloDesdeParametros(programa, cuadro, suministrador, tipo, esConsulta);
                GenerarArchivoExcelDetalle(model, 0);
                indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarFormatoDetalle", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        private FormatoModel FormatearModeloDescargaArchivoDetalle(string datos)
        {
            FormatoModel model = new FormatoModel();


            var registros = new List<RcaCuadroEjecUsuarioDetDTO>();

            registros = FormatearDatosArchivoMagnitudDetalle(datos).ToList();

            ConfigurarFormatoModeloDetalle(model);
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            model.Handson.ListaExcelData = GenerarDataDetalle(registros);
            return model;
        }

        private void ConfigurarFormatoModeloDetalle(FormatoModel model)
        {
            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            model.Handson.ListaMerge = GenerarMerges();
            model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 1;
            model.Formato.Formatcols = 2;
            model.FilasCabecera = model.Formato.Formatrows;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.Handson.ListaColWidth = new List<int> { 150, 100 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true };
        }

        private void GenerarArchivoExcelDetalle(FormatoModel model, int columnasOcultas)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();
            FileInfo newFile = new FileInfo(ruta + NombreArchivoRechazoCarga.CuadroEjecucionDetUsuario);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivoRechazoCarga.CuadroEjecucionDetUsuario);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                var filas = model.Handson.ListaFilaReadOnly.Count;
                var columnas = model.ColumnasCabecera + columnasOcultas;
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

                        if (i == 0)
                        {
                            ws.Cells[i + 1, j + 1].Style.WrapText = true;
                            ws.Cells[i + 1, j + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[i + 1, j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[i + 1, j + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkSlateBlue);
                            ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[i + 1, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        if (i > 0)
                        {
                            ws.Cells[i + 1, j + 1].Style.WrapText = false;
                            ws.Cells[i + 1, j + 1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            ws.Cells[i + 1, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            if (j <= 0)
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[i + 1, j + 1].Style.Numberformat.Format = @"dd/MM/yyyy HH:mm";
                            }

                            if (j == 1)
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                ws.Cells[i + 1, j + 1].Style.Numberformat.Format = @"0.000";
                            }
                        }
                    }
                }

                ws.Column(1).Width = 20;
                ws.Column(2).Width = 20;
                //ws.Column(3).Width = 10;
               
                //ws.Column(10).Width = 15;
               
                //ws.Column(3).Hidden = true;               

                xlPackage.Save();
            }
        }

        [HttpGet]
        public virtual ActionResult DescargarFormatoDetalle()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + NombreArchivoRechazoCarga.CuadroEjecucionDetUsuario;
            return File(fullPath, Constantes.AppExcel, NombreArchivoRechazoCarga.CuadroEjecucionDetUsuario);
        }

        /// <summary>
        /// Carga archivo excel con un nombre temporal en la ruta configurada en el Archivo de Configuración
        /// </summary>
        /// <returns></returns>
        public ActionResult SubirDetalle()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var archivo = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.NombreArchivoDetalle = fileRandom + "." + NombreArchivoRechazoCarga.ExtensionFileUploadRechazoCarga;
                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.NombreArchivoDetalle;
                    this.RutaCompletaArchivoDetalle = ruta;
                    archivo.SaveAs(ruta);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("SubirDetalle", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LeerExcelSubidoDetalle()
        {
            try
            {
                var titulos = ObtenerTitulosColumnasDetalle();
                Respuesta matrizValida;
                var matrizDatos = COES.MVC.Extranet.Areas.RechazoCarga.Helper.FormatoHelper.LeerExcelCargado(this.RutaCompletaArchivoDetalle, titulos, 1, out matrizValida);
                if (matrizValida.Exito)
                {
                    FormatoModel model = FormatearModeloDesdeMatrizDetalle(matrizDatos);
                    COES.MVC.Extranet.Areas.RechazoCarga.Helper.FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true, Datos = model });
                }
                else
                {
                    return Json(matrizValida);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Subir", ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        private FormatoModel FormatearModeloDesdeMatrizDetalle(string[][] matrizDatos)
        {
            FormatoModel model = new FormatoModel();
            ConfigurarFormatoModeloDetalle(model);
            model.Handson.ListaExcelData = matrizDatos;
            return model;
        }

    }
}
