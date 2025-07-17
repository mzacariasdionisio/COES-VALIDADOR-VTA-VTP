using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.Helper;
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
using FormatoHelper = COES.MVC.Intranet.Areas.RechazoCarga.Helper.FormatoHelper;
using log4net;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class EnvioArchivosMagnitudController : BaseController
    {
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        FormatoMedicionAppServicio formatoMedicion = new FormatoMedicionAppServicio();
        GeneralAppServicio servGeneral = new GeneralAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioArchivosMagnitudController));
        #region Propiedades

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

        public EnvioArchivosMagnitudController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("EnvioArchivosMagnitudController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("EnvioArchivosMagnitudController", ex);
                throw;
            }
        }
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
            model.Suministradores = listaSuministradores;//quitar despues
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
            cuadros.Add(new RcaCuadroProgDTO { Rccuadcodi = 0, Rccuadmotivo = "" });
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
            model.Handson.ListaColWidth = new List<int> { 280, 220, 220, 80, 100, 80, 80, 80, 80 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true, true };
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            model.Handson.ListaExcelData = GenerarData(registros);

            return Json(model);
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
                "Rechazo Carga Ejecutado", "Fecha Hora Inicio", "Fecha Hora Fin", "RCEJEUCODI", "RCPROUCODI","EQUICODI" };
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
                    matriz[i][3] = registros[i - 2].Rcproudemanda.ToString("######0.#0");
                    matriz[i][4] = registros[i - 2].Rcproufuente.ToString();
                    matriz[i][5] = registros[i - 2].Rcproudemandaracionar.ToString("######0.#0");
                    matriz[i][6] = registros[i - 2].Rcejeucargarechazada > 0 ? registros[i - 2].Rcejeucargarechazada.ToString() : string.Empty;
                    matriz[i][7] = registros[i - 2].Rcejeufechorinicio != DateTime.MinValue ? registros[i - 2].Rcejeufechorinicio.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                    matriz[i][8] = registros[i - 2].Rcejeufechorfin != DateTime.MinValue ? registros[i - 2].Rcejeufechorfin.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                    matriz[i][9] = registros[i - 2].Rcejeucodi.ToString();
                    matriz[i][10] = registros[i - 2].Rcproucodi.ToString();
                    matriz[i][11] = registros[i - 2].Equicodi.ToString();
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
            catch
            {
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
            model.Handson.ListaColWidth = new List<int> { 280, 220, 220, 80, 100, 80, 80, 80, 80 };
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
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;
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

        

        [HttpPost]
        public JsonResult LeerExcelSubido()
        {
            try
            {
                var titulos = ObtenerTitulosColumnas();
                Respuesta matrizValida;
                var matrizDatos = FormatoHelper.LeerExcelCargado(this.RutaCompletaArchivo, titulos, 2, out matrizValida);
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
            catch(Exception ex)
            {
                Log.Error("LeerExcelSubido", ex);
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
            var datosCuadroEjecUsuario = FormatearDatosArchivoMagnitud(datos);                     

            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            //Boolean enPlazo = true;//ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = codigoSuministrador;
            envio.Enviofecha = DateTime.Now;
            envio.Enviofechaperiodo = DateTime.Now;//formato.FechaInicio;
            envio.Envioplazo = tipo;//(enPlazo) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = DateTime.Now;
            envio.Lastuser = User.Identity.Name;
            envio.Userlogin = User.Identity.Name;
            //envio.Formatcodi = this.IdFormato;
            this.IdEnvio = logic.SaveMeEnvio(envio);
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

            
            return Json(new Respuesta { Exito = true });
        }

        private ICollection<RcaCuadroEjecUsuarioDTO> FormatearDatosArchivoMagnitud(string datos)
        {
            var filasCabecera = 2;
            var columnas = 12;
            var celdas = datos.Split(',');
            var parametros = new List<RcaCuadroEjecUsuarioDTO>();
            var inicioDatos = filasCabecera * columnas;
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                var parametro = new RcaCuadroEjecUsuarioDTO();
                parametro.Empresa = celdas[i].Replace(@"\", "").Replace("\"", "").Replace("[", "");
                parametro.Subestacion = celdas[i + 1].Replace(@"\", "").Replace("\"", "");
                parametro.Puntomedicion = celdas[i + 2].Replace(@"\", "").Replace("\"", "");
                parametro.Rcproudemanda = Convert.ToDecimal(celdas[i + 3].Replace(@"\", "").Replace("\"", "").Replace("null", "0"));
                parametro.Rcproufuente = celdas[i + 4].Replace(@"\", "").Replace("\"", "");
                parametro.Rcproudemandaracionar = Convert.ToDecimal(celdas[i + 5].Replace(@"\", "").Replace("\"", ""));
                //parametro.Rcproufactork = Convert.ToDecimal(celdas[i + 7].Replace(@"\", "").Replace("\"", ""));
                if (!string.IsNullOrEmpty(celdas[i + 6].Replace(@"\", "").Replace("\"", "")))
                {
                    parametro.Rcejeucargarechazada = Convert.ToDecimal(celdas[i + 6].Replace(@"\", "").Replace("\"", ""));
                }

                if (!string.IsNullOrEmpty(celdas[i + 7].Replace(@"\", "").Replace("\"", "")))
                {
                    parametro.Rcejeufechorinicio = DateTime.ParseExact(celdas[i + 7].Replace(@"\", "").Replace("\"", ""), "dd/MM/yyyy HH:mm", null);
                }

                if (!string.IsNullOrEmpty(celdas[i + 8].Replace(@"\", "").Replace("\"", "")))
                {
                    parametro.Rcejeufechorfin = DateTime.ParseExact(celdas[i + 8].Replace(@"\", "").Replace("\"", ""), "dd/MM/yyyy HH:mm", null);
                }
               
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
                logic.GrabarCambios(listaCambio);
                //GrabarCambios(listaOrigen);
                //si es primer reenvio grabar valores origen

            }


        }
    }
}
