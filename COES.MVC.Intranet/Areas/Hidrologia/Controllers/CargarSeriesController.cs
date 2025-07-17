using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.CPPA.Helper;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class CargarSeriesController : BaseController
    {

        #region Propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionNombreArchivo] != null) ?
                    Session[DatosSesionDemandaCP.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionDemandaCP.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionFileName] != null) ?
                    Session[DatosSesionDemandaCP.SesionFileName].ToString() : null;
            }
            set { Session[DatosSesionDemandaCP.SesionFileName] = value; }
        }

        /// <summary>
        /// Codigo del envio
        /// </summary>
        public int IdEnvio
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionIdEnvio] != null) ?
                    (int)Session[DatosSesionDemandaCP.SesionIdEnvio] : 0;
            }
            set { Session[DatosSesionDemandaCP.SesionIdEnvio] = value; }
        }

        /// <summary>
        /// Nombre del formato
        /// </summary>
        public MeFormatoDTO Formato
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionFormato] != null) ?
                    (MeFormatoDTO)Session[DatosSesionDemandaCP.SesionFormato] : new MeFormatoDTO();
            }
            set { Session[DatosSesionDemandaCP.SesionFormato] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionMatrizExcel] != null) ?
                    (string[][])Session[DatosSesionDemandaCP.SesionMatrizExcel] : new string[1][];
            }
            set { Session[DatosSesionDemandaCP.SesionMatrizExcel] = value; }
        }

        #endregion

        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        HidrologiaAppServicio logicHidro = new HidrologiaAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        private GeneralAppServicio logicGeneral =  new GeneralAppServicio();
        public FormatoMedicionAppServicio servicio =  new FormatoMedicionAppServicio();


        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CargaDatosController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int EmpresaCodi, int TipoSerie, int TipoPuntoMedicion, int PuntoMedicion, string ModoLectura)
        {
            FormatoResultado modelResul = new FormatoResultado();
            int idEnvio = 0;
            string ruta = string.Empty;
            int IdFormato = 109;
            try
            {
                ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologiaCD.FolderUpload;
                FormatoModel model = BuildHojaExcel(idEnvio, EmpresaCodi, TipoSerie, TipoPuntoMedicion, PuntoMedicion, ModoLectura);
                this.Formato.Formatnombre = NombreArchivoHidrologiaCD.NombreArchivoSeriesHidrologicasCD;

                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(PuntoMedicion);

                logic.GenerarFileExcelSH(model, ptoMedicion, ruta);
                modelResul.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResul.Resultado = -1;
                modelResul.Mensaje = ex.Message;
                modelResul.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            return Json(modelResul);
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologiaCD.FolderUpload;
            string fullPath = ruta + this.Formato.Formatnombre + ".xlsx";

            return File(fullPath, Constantes.AppExcel, this.Formato.Formatnombre + ".xlsx");
        }

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            MeArchivoDTO archivo = new MeArchivoDTO();
            MeEnvioDTO envio = new MeEnvioDTO();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.FileName = fileRandom + "." + NombreArchivoDemandaCP.ExtensionFileUploadHidrologia;
                    string fileName = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.FileName;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lee datos desde excel
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult LeerFileUpExcel(int idEmpresa, int formatoReal)
        {
            int retorno = 1;
            if (retorno > 0)
            {
                MeFormatoDTO formato = new MeFormatoDTO();
                var cabecera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();

                int anioActual = DateTime.Now.Year;
                int anioInicio = 1965;
                int nBloques = anioActual - anioInicio;
                formato.Formatrows = 1;
                formato.Formatcols = 1;
                int nCol = formato.Formatcols;
                int horizonte = formato.Formathorizonte;
                int numColsGrilla = 13;
                
                this.MatrizExcel = logic.InicializaMatrizExcel(formato.Formatrows, nBloques, formato.Formatcols, numColsGrilla);
                Boolean isValido = logic.LeerExcelFileSH(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, numColsGrilla);
                
            }
            logic.BorrarArchivo(this.NombreFile);
            return Json(retorno);
        }

        /// <summary>
        /// Graba los datos del archivo Excel Web
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string dataExcel, string dataExcel2, int EmpresaCodi, int TipoSerie, int TipoPuntoMedicion, int PuntoMedicion)
        {
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;

            try
            {
                base.ValidarSesionUsuario();

                int exito = 1;
                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();
                string empresa = string.Empty;
                var regEmp = this.logic.GetByIdSiEmpresa(EmpresaCodi);
                //////////////////////////////////////////////////
                if (regEmp != null)
                    empresa = regEmp.Emprnomb;
                MeFormatoDTO formato = new MeFormatoDTO();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                formato.Formatcols = 1;
                formato.Formatrows = 2;
                int filaHead = formato.Formatrows;
                int colHead = formato.Formatcols;

                ///////////////Grabar Caudal//////////////////////////
                

                SHCaudalDTO caudal = new SHCaudalDTO();
                caudal.IdCaudal = 0;
                caudal.EmpresaCodi = EmpresaCodi;
                caudal.TipoSerieCodi = TipoSerie;
                caudal.TPtoMediCodi = TipoPuntoMedicion;
                caudal.PtoMediCodi = PuntoMedicion;
                caudal.UsuarioRegistro = User.Identity.Name;

                //Obtener Informacion del Caudal Anterior
                SHCaudalDTO caudalAnt = new SHCaudalDTO();
                List<SHCaudalDetalleDTO> listCaudalDetAnt = new List<SHCaudalDetalleDTO>();
                caudalAnt = logic.GetCaudalActivo(caudal);
                int idCaudalAnt = 0;
                if (caudalAnt!=null)
                {
                    idCaudalAnt = caudalAnt.IdCaudal;
                    listCaudalDetAnt = logic.ListDetalle(caudalAnt);
                }

                logic.UpdateSHCaudal(caudal);
                this.IdEnvio = logic.SaveSHCaudal(caudal);
                model.IdEnvio = this.IdEnvio;

                if (celdas.Count>0)
                {
                    int contCeldas = 1;
                    int intAnio = 0;
                    decimal? decCaudalEne = 0;
                    decimal? decCaudalFeb = 0;
                    decimal? decCaudalMar = 0;
                    decimal? decCaudalAbr = 0;
                    decimal? decCaudalMay = 0;
                    decimal? decCaudalJun = 0;
                    decimal? decCaudalJul = 0;
                    decimal? decCaudalAgo = 0;
                    decimal? decCaudalSet = 0;
                    decimal? decCaudalOct = 0;
                    decimal? decCaudalNov = 0;
                    decimal? decCaudalDic = 0;

                    foreach (var reg in celdas)
                    {
                        int modCeldas = contCeldas % 13;
                        
                        if (contCeldas>13)
                        {
                            if (modCeldas == 1)
                            {
                                intAnio = Convert.ToInt32(reg);
                            }
                            if (modCeldas == 2)
                            {
                                if (reg=="")
                                {
                                    decCaudalEne = null;
                                } else
                                {
                                    decCaudalEne = Convert.ToDecimal(reg);
                                }                               
                            }
                            if (modCeldas == 3)
                            {
                                if (reg == "")
                                {
                                    decCaudalFeb = null;
                                }
                                else
                                {
                                    decCaudalFeb = Convert.ToDecimal(reg);
                                }                             
                            }
                            if (modCeldas == 4)
                            {
                                if (reg == "")
                                {
                                    decCaudalMar = null;
                                }
                                else
                                {
                                    decCaudalMar = Convert.ToDecimal(reg);
                                }                              
                            }
                            if (modCeldas == 5)
                            {                             
                                if (reg == "")
                                {
                                    decCaudalAbr = null;
                                }
                                else
                                {
                                    decCaudalAbr = Convert.ToDecimal(reg);
                                }
                            }
                            if (modCeldas == 6)
                            {
                                if (reg == "")
                                {
                                    decCaudalMay = null;
                                }
                                else
                                {
                                    decCaudalMay = Convert.ToDecimal(reg);
                                }
                            }
                            if (modCeldas == 7)
                            {                               
                                if (reg == "")
                                {
                                    decCaudalJun = null;
                                }
                                else
                                {
                                    decCaudalJun = Convert.ToDecimal(reg);
                                }
                            }
                            if (modCeldas == 8)
                            {
                                if (reg == "")
                                {
                                    decCaudalJul = null;
                                }
                                else
                                {
                                    decCaudalJul = Convert.ToDecimal(reg);
                                }
                            }
                            if (modCeldas == 9)
                            {
                                if (reg == "")
                                {
                                    decCaudalAgo = null;
                                }
                                else
                                {
                                    decCaudalAgo = Convert.ToDecimal(reg);
                                }
                            }
                            if (modCeldas == 10)
                            {
                                if (reg == "")
                                {
                                    decCaudalSet = null;
                                }
                                else
                                {
                                    decCaudalSet = Convert.ToDecimal(reg);
                                }
                            }
                            if (modCeldas == 11)
                            {
                                if (reg == "")
                                {
                                    decCaudalOct = null;
                                }
                                else
                                {
                                    decCaudalOct = Convert.ToDecimal(reg);
                                }
                            }
                            if (modCeldas == 12)
                            {
                                if (reg == "")
                                {
                                    decCaudalNov = null;
                                }
                                else
                                {
                                    decCaudalNov = Convert.ToDecimal(reg);
                                }
                            }
                            if (modCeldas == 0)
                            {
                                if (reg == "")
                                {
                                    decCaudalDic = null;
                                }
                                else
                                {
                                    decCaudalDic = Convert.ToDecimal(reg);
                                }
                                SHCaudalDetalleDTO caudalDet = new SHCaudalDetalleDTO();
                                caudalDet.IdCaudal = this.IdEnvio;
                                caudalDet.Anio = intAnio;
                                caudalDet.M1 = decCaudalEne;
                                caudalDet.M2 = decCaudalFeb;
                                caudalDet.M3 = decCaudalMar;
                                caudalDet.M4 = decCaudalAbr;
                                caudalDet.M5 = decCaudalMay;
                                caudalDet.M6 = decCaudalJun;
                                caudalDet.M7 = decCaudalJul;
                                caudalDet.M8 = decCaudalAgo;
                                caudalDet.M9 = decCaudalSet;
                                caudalDet.M10 = decCaudalOct;
                                caudalDet.M11 = decCaudalNov;
                                caudalDet.M12 = decCaudalDic;

                                if (listCaudalDetAnt.Count>0)
                                {
                                    SHCaudalDetalleDTO infoCaudalDetAnt = new SHCaudalDetalleDTO();
                                    infoCaudalDetAnt = listCaudalDetAnt.Where(i => i.Anio == intAnio).FirstOrDefault();
                                    if (infoCaudalDetAnt != null) { 
                                        if (infoCaudalDetAnt.M1 == decCaudalEne)
                                        {
                                            caudalDet.IndM1 = string.Empty;
                                        } else
                                        {
                                            caudalDet.IndM1 = "1";
                                        }
                                        if (infoCaudalDetAnt.M2 == decCaudalFeb)
                                        {
                                            caudalDet.IndM2 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM2 = "1";
                                        }
                                        if (infoCaudalDetAnt.M3 == decCaudalMar)
                                        {
                                            caudalDet.IndM3 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM3 = "1";
                                        }
                                        if (infoCaudalDetAnt.M4 == decCaudalAbr)
                                        {
                                            caudalDet.IndM4 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM4 = "1";
                                        }
                                        if (infoCaudalDetAnt.M5 == decCaudalMay)
                                        {
                                            caudalDet.IndM5 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM5 = "1";
                                        }
                                        if (infoCaudalDetAnt.M6 == decCaudalJun)
                                        {
                                            caudalDet.IndM6 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM6 = "1";
                                        }
                                        if (infoCaudalDetAnt.M7 == decCaudalJul)
                                        {
                                            caudalDet.IndM7 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM7 = "1";
                                        }
                                        if (infoCaudalDetAnt.M8 == decCaudalAgo)
                                        {
                                            caudalDet.IndM8 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM8 = "1";
                                        }
                                        if (infoCaudalDetAnt.M9 == decCaudalSet)
                                        {
                                            caudalDet.IndM9 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM9 = "1";
                                        }
                                        if (infoCaudalDetAnt.M10 == decCaudalOct)
                                        {
                                            caudalDet.IndM10 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM10 = "1";
                                        }
                                        if (infoCaudalDetAnt.M11 == decCaudalNov)
                                        {
                                            caudalDet.IndM11 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM11 = "1";
                                        }
                                        if (infoCaudalDetAnt.M12 == decCaudalDic)
                                        {
                                            caudalDet.IndM12 = string.Empty;
                                        }
                                        else
                                        {
                                            caudalDet.IndM12 = "1";
                                        }
                                    }

                                }

                                int intResultado = 0;
                                intResultado = logic.SaveSHCaudalDetalle(caudalDet);

                            }

                        }
                        
                        contCeldas++;
                    }
                }               
                model.Resultado = exito;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Muestra El formato Excel en la Web 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="desEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarHojaExcelWeb(int idEnvio, int EmpresaCodi, int TipoSerie, int TipoPuntoMedicion, int PuntoMedicion, string ModoLectura)
        {
            FormatoModel jsModel = BuildHojaExcel(idEnvio, EmpresaCodi, TipoSerie, TipoPuntoMedicion, PuntoMedicion, ModoLectura);
            Session["DatosJSON"] = jsModel.Handson.ListaExcelData;
            jsModel.Handson.ListaExcelData = new string[0][];
            return Json(jsModel);
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DescargarDatos()
        {
            string[][] list = (string[][])Session["DatosJSON"];

            var data = list;
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(data);
            result.ContentType = "application/json";
            return result;
        }

        /// <summary>
        /// Carga principal de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            FormatoModel model = new FormatoModel();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasPtoMedicion();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion();

            model.IdEnvio = 0;
            if (model.ListaEmpresas.Count == 1)
            {
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            }
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);

            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        [HttpPost]
        public ActionResult ListarPuntoMedicionPorEmpresa(int CodEmpresa, int CodTipoSerie, int CodTipoPuntoMedicion)
        {
            List<MePtomedicionDTO> listaPtoMedicion = new List<MePtomedicionDTO>();
            listaPtoMedicion = servicio.ListarPuntoMedicionPorEmpresa(CodEmpresa, CodTipoSerie, CodTipoPuntoMedicion);
            return Json(listaPtoMedicion);
        }

        [HttpPost]
        public ActionResult ListarInformacionPuntoMedicionPorEmpresa(int CodPuntoMedicion)
        {
            MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
            ptoMedicion = servicio.GetPtoMedicionById(CodPuntoMedicion);
            return Json(ptoMedicion);
        }

        /// <summary>
        ///Devuelve el model necesario para mostrar en la web
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="EmpresaCodi"></param>
        /// <param name="TipoSerie"></param>
        /// <param name="TipoPuntoMedicion"></param>
        /// <param name="PuntoMedicion"></param>
        /// <param name="ModoLectura"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcel(int idEnvio, int EmpresaCodi, int TipoSerie, int TipoPuntoMedicion, int PuntoMedicion, string ModoLectura)
        {
            FormatoModel model = new FormatoModel();

            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            model.IdEnvio = idEnvio;
            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = new MeFormatoDTO();
            this.Formato = model.Formato;
            model.Formato.Formatcols = 1;
            model.Formato.Formatrows = 1;

            model.ColumnasCabecera = 13;
            model.FilasCabecera = 1;
            model.ListaHojaPto = new List<MeHojaptomedDTO>();
            for (var x = 0; x < model.ColumnasCabecera; x++)
            {
                MeHojaptomedDTO hojaptomed = new MeHojaptomedDTO();
                hojaptomed.Hojaptoliminf = 0;
                hojaptomed.Hojaptolimsup = Convert.ToDecimal(99999.999);
                model.ListaHojaPto.Add(hojaptomed);

            }


            string mensaje = string.Empty;
            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real

        
            model.IdEmpresa = EmpresaCodi;
            model.Anho = model.Formato.FechaInicio.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);
            model.Dia = model.Formato.FechaInicio.Day.ToString();
            model.Handson.Width = HandsonConstantes.ColWidth * 13;
            //Genera La vista html complementaria a la grilla Handson, nombre de formato, area coes, fecha de formato, etc.

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
            int nCol = 0;

            int anioActual = DateTime.Now.Year;
            int anioInicio = 1965;
            int nBloques = anioActual - anioInicio;
            model.Handson.ListaFilaReadOnly = logic.InicializaListaFilaReadOnlySH(model.Formato.Formatrows, nBloques);
            model.ListaCambios = new List<CeldaCambios>();

            model.Handson.ListaExcelData = logic.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);  

            //llenado cabecera
            for (var ind = 0; ind < model.ColumnasCabecera; ind++)
            {
                if (ind==0)
                {
                    model.Handson.ListaColWidth.Add(80);
                } else
                {
                    model.Handson.ListaColWidth.Add(78);
                }
                
            }

            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();


            logic.CargarCabeceraMatrizSH(model.Handson);
            int filaInicio = 1;

            model.Handson.ListaExcelDescripcion = new string[nBloques+1][];

            for (int anio = anioInicio; anio < anioActual; anio++)
            {
                model.Handson.ListaExcelData[filaInicio] = new string[13];
                model.Handson.ListaExcelData[filaInicio][0] = anio.ToString();
                model.Handson.ListaExcelData[filaInicio][1] = "";
                model.Handson.ListaExcelData[filaInicio][2] = "";
                model.Handson.ListaExcelData[filaInicio][3] = "";
                model.Handson.ListaExcelData[filaInicio][4] = "";
                model.Handson.ListaExcelData[filaInicio][5] = "";
                model.Handson.ListaExcelData[filaInicio][6] = "";
                model.Handson.ListaExcelData[filaInicio][7] = "";
                model.Handson.ListaExcelData[filaInicio][8] = "";
                model.Handson.ListaExcelData[filaInicio][9] = "";
                model.Handson.ListaExcelData[filaInicio][10] = "";
                model.Handson.ListaExcelData[filaInicio][11] = "";
                model.Handson.ListaExcelData[filaInicio][12] = "";



                model.Handson.ListaExcelDescripcion[filaInicio] = new string[13];
                model.Handson.ListaExcelDescripcion[filaInicio][0] = anio.ToString();
                model.Handson.ListaExcelDescripcion[filaInicio][1] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][2] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][3] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][4] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][5] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][6] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][7] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][8] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][9] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][10] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][11] = "";
                model.Handson.ListaExcelDescripcion[filaInicio][12] = "";

                filaInicio++;
            }


            model.FilasCabecera = 1;
            model.ColumnasCabecera = 1;

            SHCaudalDTO caudal = new SHCaudalDTO();
            caudal.EmpresaCodi = EmpresaCodi;
            caudal.TipoSerieCodi = TipoSerie;
            caudal.TPtoMediCodi = TipoPuntoMedicion;
            caudal.PtoMediCodi = PuntoMedicion;

            if (!string.IsNullOrEmpty(ModoLectura))
            {
                model.Handson.ReadOnly = true;
            }

            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {

                SHCaudalDTO caudalActivo = new SHCaudalDTO();
                
                caudalActivo = logic.GetCaudalActivo(caudal);
                if (caudalActivo!=null)
                {
                    if (idEnvio > 0) {
                        caudalActivo.IdCaudal = idEnvio;
                    }
                    List<SHCaudalDetalleDTO> caudalDetalle = new List<SHCaudalDetalleDTO>();
                    caudalDetalle = logic.ListDetalle(caudalActivo);

                    model.Handson.ListaExcelData[0] = new string[13];
                    model.Handson.ListaExcelData[0][0] = "AÑO";
                    model.Handson.ListaExcelData[0][1] = "ENE";
                    model.Handson.ListaExcelData[0][2] = "FEB";
                    model.Handson.ListaExcelData[0][3] = "MAR";
                    model.Handson.ListaExcelData[0][4] = "ABR";
                    model.Handson.ListaExcelData[0][5] = "MAY";
                    model.Handson.ListaExcelData[0][6] = "JUN";
                    model.Handson.ListaExcelData[0][7] = "JUL";
                    model.Handson.ListaExcelData[0][8] = "AGO";
                    model.Handson.ListaExcelData[0][9] = "SET";
                    model.Handson.ListaExcelData[0][10] = "OCT";
                    model.Handson.ListaExcelData[0][11] = "NOV";
                    model.Handson.ListaExcelData[0][12] = "DIC";


                    int numInicioMatriz = 1;
                    foreach (var detalle in caudalDetalle)
                    {
                        model.Handson.ListaExcelData[numInicioMatriz][0] = detalle.Anio.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][1] = detalle.M1.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][2] = detalle.M2.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][3] = detalle.M3.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][4] = detalle.M4.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][5] = detalle.M5.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][6] = detalle.M6.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][7] = detalle.M7.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][8] = detalle.M8.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][9] = detalle.M9.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][10] = detalle.M10.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][11] = detalle.M11.ToString();
                        model.Handson.ListaExcelData[numInicioMatriz][12] = detalle.M12.ToString();

                        model.Handson.ListaExcelDescripcion[numInicioMatriz][0] = detalle.Anio.ToString();
                        if (!string.IsNullOrEmpty(detalle.IndM1))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][1] = detalle.IndM1.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM2))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][2] = detalle.IndM2.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM3))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][3] = detalle.IndM3.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM4))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][4] = detalle.IndM4.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM5))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][5] = detalle.IndM5.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM6))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][6] = detalle.IndM6.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM7))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][7] = detalle.IndM7.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM8))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][8] = detalle.IndM8.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM9))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][9] = detalle.IndM9.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM10))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][10] = detalle.IndM10.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM11))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][11] = detalle.IndM11.ToString();
                        }
                        if (!string.IsNullOrEmpty(detalle.IndM12))
                        {
                            model.Handson.ListaExcelDescripcion[numInicioMatriz][12] = detalle.IndM12.ToString();
                        }

                        numInicioMatriz++;
                    }
                }
            }
            else
            { // los datos para visualizar en el excel web provienen de un archivo excel cargado por el usuario
                //Carga de archivo Excel
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                model.Handson.ListaExcelData[0] = new string[13];
                model.Handson.ListaExcelData[0][0] = "AÑO";
                model.Handson.ListaExcelData[0][1] = "ENE";
                model.Handson.ListaExcelData[0][2] = "FEB";
                model.Handson.ListaExcelData[0][3] = "MAR";
                model.Handson.ListaExcelData[0][4] = "ABR";
                model.Handson.ListaExcelData[0][5] = "MAY";
                model.Handson.ListaExcelData[0][6] = "JUN";
                model.Handson.ListaExcelData[0][7] = "JUL";
                model.Handson.ListaExcelData[0][8] = "AGO";
                model.Handson.ListaExcelData[0][9] = "SET";
                model.Handson.ListaExcelData[0][10] = "OCT";
                model.Handson.ListaExcelData[0][11] = "NOV";
                model.Handson.ListaExcelData[0][12] = "DIC";


                int numInicioMatriz = 1;
                for (int anio = anioInicio; anio < anioActual; anio++)
                {
                    model.Handson.ListaExcelData[numInicioMatriz][0] = anio.ToString();
                    numInicioMatriz++;
                }
            }


            List<SHCaudalDTO> listaCaudal = new List<SHCaudalDTO>();
            listaCaudal = logic.GetListaSHCaudal(caudal);
            model.ListaCaudal = listaCaudal;

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (listaCaudal.Count > 0)
            {
                idUltEnvio = listaCaudal[listaCaudal.Count - 1].IdCaudal;
                if (idEnvio > 0)
                {
                    idUltEnvio = idEnvio;
                }
                var reg = listaCaudal.Find(x => x.IdCaudal == idUltEnvio);
                if (reg != null)
                    model.FechaEnvio = (reg.FechaRegistro).ToString(Constantes.FormatoFechaHora);
            }

            return model;
        }

        /// <summary>
        /// Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesHidrologiaCD.ModuloManualUsuario;
            string nombreArchivo = ConstantesHidrologiaCD.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesHidrologiaCD.FolderRaizHidrologiaModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);
                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                {
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}
