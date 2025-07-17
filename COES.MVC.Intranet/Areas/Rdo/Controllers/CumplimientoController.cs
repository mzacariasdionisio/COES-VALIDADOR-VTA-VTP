using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Rdo.Helper;
using COES.MVC.Intranet.Areas.Rdo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.RDO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Rdo.Controllers
{
    public class CumplimientoController : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CumplimientoController));
        RDOAppServicio _svcGestionRdo = new RDOAppServicio();
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

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

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CumplimientoController));
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
        public JsonResult GenerarFormato(int idEmpresa, int formatoReal, string fecha, string semana)
        {
            int idEnvio = 0;
            string ruta = string.Empty;
            int IdFormato = logic.ObtenerIdFormatoPadre(formatoReal);
            FormatoResultado modelResul = new FormatoResultado();
            try
            {
                ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedicionesCD.FolderUpload;
                FormatoModel model = BuildHojaExcel(idEmpresa, formatoReal, IdFormato, idEnvio, fecha, semana);
                logic.GenerarFileExcelRer(model, ruta);
                modelResul.Resultado = 1;
            }

            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResul.Resultado = -1;
                modelResul.Mensaje = ex.Message;
                modelResul.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            return Json(new { modelResul }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            //string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras] + this.Formato.Formatnombre + ".xlsx";
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedicionesCD.FolderUpload;
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
                    //file.SaveAs(fileName);
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
        public JsonResult LeerFileUpExcel(int idEmpresa, int formatoReal, string fecha, string semana)
        {
            int IdFormato = logic.ObtenerIdFormatoPadre(formatoReal);
            int retorno = logic.VerificarIdsFormato(this.NombreFile, idEmpresa, formatoReal);

            if (retorno > 0)
            {

                MeFormatoDTO formato = logic.GetByIdMeFormato(formatoReal);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, semana, fecha, Constantes.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                logic.GetSizeFormato2(formato);
                var cabecera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatcols = cabecera.Cabcolumnas;
                //
                var listaPtos = logic.GetByCriteria2MeHojaptomeds(idEmpresa, IdFormato, cabecera.Cabquery, formato.FechaInicio, formato.FechaFin).Where(x => x.Hojaptoactivo == 1).ToList();
                int nCol = listaPtos.Count;
                int horizonte = formato.Formathorizonte;
                int nBloques = formato.RowPorDia * formato.Formathorizonte;
                this.MatrizExcel = logic.InicializaMatrizExcel(formato.Formatrows, nBloques, formato.Formatcols, nCol);
                Boolean isValido = logic.LeerExcelFile(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, nCol, ConstantesMedicionesCD.FormatoDiarioCodi);
            }
            logic.BorrarArchivo(this.NombreFile);
            return Json(new { retorno }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GrabarExcelWeb(string dataExcel, string dataExcel2, int idEmpresa, int formatoReal, string fecha, string semana)
        {
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            int exito = 0;
            List<string> celdas = new List<string>();
            celdas = dataExcel.Split(',').ToList();
            string empresa = string.Empty;
            var regEmp = this.logic.GetByIdSiEmpresa(idEmpresa);
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<MeMedicion48DTO> listaData = serializer.Deserialize<List<MeMedicion48DTO>>(dataExcel2);

            var fechaNew = new DateTime();
            foreach (var reg in listaData)
            {
                fechaNew = DateTime.ParseExact(reg.MedifechaPto, ConstantesFormat.FormatoFechaHora, CultureInfo.InvariantCulture);
                reg.Medifecha = new DateTime(fechaNew.Year, fechaNew.Month, fechaNew.Day);
            }

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //////////////////////////////////////////////////
            if (regEmp != null)
                empresa = regEmp.Emprnomb;
            int idFormato = logic.ObtenerIdFormatoPadre(formatoReal);
            MeFormatoDTO formato = this.logic.GetByIdMeFormato(formatoReal);

            //>Obetener Lectocodi
            listaData.ForEach(x => x.Lectcodi = formato.Lectcodi);

            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            formato.Formatcols = cabercera.Cabcolumnas;
            formato.Formatrows = cabercera.Cabfilas;
            formato.Formatheaderrow = cabercera.Cabcampodef;
            int filaHead = formato.Formatrows;
            int colHead = formato.Formatcols;

            /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, semana, fecha, Constantes.FormatoFecha);
            logic.GetSizeFormato2(formato);

            var listaPto = this.logic.GetByCriteriaMeHojaptomeds(idEmpresa, idFormato, formato.FechaInicio, formato.FechaFin);
            int nPtos = listaPto.Count();

            /////////////// Grabar Config Formato Envio //////////////////
            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = formatoReal;
            config.Emprcodi = idEmpresa;
            config.FechaInicio = formato.FechaInicio;
            config.FechaFin = formato.FechaFin;
            int idConfig = logic.GrabarConfigFormatEnvio(config);

            //////////////////////////////////////////////////////////////////////////

            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            Boolean enPlazo = logic.ValidarPlazoController(formato);

            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = idEmpresa;
            envio.Enviofecha = DateTime.Now;
            envio.Enviofechaperiodo = formato.FechaInicio;
            envio.Envioplazo = (enPlazo) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = DateTime.Now;
            envio.Lastuser = User.Identity.Name;
            envio.Userlogin = User.Identity.Name;
            envio.Formatcodi = formatoReal;
            envio.Cfgenvcodi = idConfig;
            this.IdEnvio = logic.SaveMeEnvio(envio);
            model.IdEnvio = this.IdEnvio;

            ///////////////////////////////////////////////////////
            int horizonte = formato.Formathorizonte;
            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionMediaHora:
                    int total = (nPtos + formato.Formatcols) * (filaHead + 48 * formato.Formathorizonte);
                    int totalRecibido = celdas.Count;
                    //validar checklanco
                    logic.ValidarCheckBlancoExcelWeb48(listaData, formato.Formatcheckblanco);
                    //var lista48 = logic.LeerExcelWeb48(celdas, listaPto, (int)formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 2 * formato.Formathorizonte, formato.Formatcheckblanco);
                    var lista48 = listaData;
                    if (lista48.Count > 0)
                    {
                        try
                        {
                            this.logic.GrabarValoresCargados48(lista48, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Mensaje = MensajesDemandaCP.MensajeEnvioExito;
                        }
                        catch (Exception ex)
                        {
                            exito = -1;
                            model.Resultado = -1;
                        }
                    }
                    else
                    {
                        exito = -2;
                        model.Resultado = -2;
                    }
                    break;
                case ParametrosFormato.ResolucionHora:
                    try
                    {
                        var lista24 = logic.LeerExcelWeb24(celdas, listaPto, formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * formato.Formathorizonte);
                        this.logic.GrabarValoresCargados24(lista24, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = this.IdEnvio;
                        logic.UpdateMeEnvio(envio);
                        exito = 1;
                        model.Resultado = 1;
                        model.Mensaje = MensajesDemandaCP.MensajeEnvioExito;
                    }
                    catch
                    {
                        exito = -1;
                        model.Resultado = -1;
                    }
                    break;
            }

            model.Resultado = exito;
            return Json(new { model }, JsonRequestBehavior.AllowGet);
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
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, int formatoReal, int idEnvio, string fecha, string semana)
        {
            int IdFormato = logic.ObtenerIdFormatoPadre(formatoReal);
            var totalPto = this.logic.GetByCriteriaMeHojaptomeds(idEmpresa, IdFormato, DateTime.Now.Date, DateTime.Now.Date);
            var formatoPto = IdFormato;

            FormatoModel jsModel;
            if (totalPto.Count > 0)
            {
                jsModel = BuildHojaExcel(idEmpresa, formatoReal, formatoPto, idEnvio, fecha, semana);
                Session["DatosJSON"] = jsModel.Handson.ListaExcelData;
                jsModel.Handson.ListaExcelData = new string[0][];
                return Json(new { jsModel }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Session["DatosJSON"] = null;
                return Json(new { jsModel = -1 }, JsonRequestBehavior.AllowGet);
            }
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
            model.IdModulo = Modulos.AppMedidoresDistribucion;
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.TIPOEMPRCODI == 2).Select(x =>
                new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
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
            //model.ListaSemanas = semanas;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);

            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        ///Devuelve el model necesario para mostrar en la web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcel(int idEmpresa, int idFormato, int formatoPto, int idEnvio, string fecha, string semana)
        {
            FormatoModel model = new FormatoModel();

            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            model.IdEnvio = idEnvio;
            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = logic.GetByIdMeFormato(idFormato);
            this.Formato = model.Formato;
            var cabecera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            model.Formato.Formatcols = cabecera.Cabcolumnas;
            model.Formato.Formatrows = cabecera.Cabfilas;
            model.Formato.Formatheaderrow = cabecera.Cabcampodef;

            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;
            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, semana, fecha, Constantes.FormatoFecha);
            logic.GetSizeFormato2(model.Formato);
            model.ListaHojaPto = this.logic.GetByCriteria2MeHojaptomeds(idEmpresa, formatoPto, cabecera.Cabquery, model.Formato.FechaInicio, model.Formato.FechaFin)
                .Where(x => x.Hojaptoactivo == 1).ToList();
            var cabecerasRow = model.Formato.Formatheaderrow.Split(QueryParametros.SeparadorFila);
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }

            model.ListaEnvios = this.logic.GetByCriteriaMeEnvios(idEmpresa, idFormato, model.Formato.FechaInicio);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (model.ListaEnvios.Count > 0)
            {
                idUltEnvio = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                if (reg != null)
                    model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(Constantes.FormatoFechaHora);
            }
            /// Verifica si Formato esta en Plaz0
            string mensaje = string.Empty;
            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real

            model.EnPlazo = logic.ValidarPlazoController(model.Formato);
            if ((idEnvio <= 0) || (idEnvio == idUltEnvio)) // id envio < 0 => es una carga de archivo excel, id envio = 0 => envio nuevo 
            {
                model.Handson.ReadOnly = !logic.ValidarFecha(model.Formato, idEmpresa, out horaini, out horafin);//ValidarFecha(idEmpresa, model.Formato.FechaProceso, idFormato, out mensaje);
                //model.Handson.ReadOnly = !model.EnPlazo;
            }
            else /// id envio > 0 => reenvio
            { /// Es solo para visualizar envios anteriores
                model.Handson.ReadOnly = true;
            }
            var entEmpresa = this.logic.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
                model.Empresa = entEmpresa.Emprnomb;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaInicio.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);
            model.Semana = semana;
            model.Dia = model.Formato.FechaInicio.Day.ToString();
            model.Handson.Width = HandsonConstantes.ColWidth * ((model.ListaHojaPto.Count > HandsonConstantes.ColPorHoja) ? HandsonConstantes.ColPorHoja :
                (model.ListaHojaPto.Count + model.ColumnasCabecera));
            //Genera La vista html complementaria a la grilla Handson, nombre de formato, area coes, fecha de formato, etc.
            model.ViewHtml = logic.GenerarFormatoHtml(model, idEnvio, model.EnPlazo);

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
            int nCol = model.ListaHojaPto.Count;
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;
            model.Handson.ListaFilaReadOnly = logic.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques, !model.Handson.ReadOnly, horaini, horafin);
            model.ListaCambios = new List<CeldaCambios>();
            model.Handson.ListaExcelData = logic.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);
            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {
                lista = this.logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                //if (model.ListaEnvios.Count() == 0)
                if (true)
                {
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Obtener datos de Generación RerNC
                    List<Object> listaGeneracionRer = new List<Object>();
                    List<Object> listaValidacion = new List<Object>();
                    listaGeneracionRer = this.logic.ObtenerDatosRerNC(model, idEmpresa);
                    lista = listaGeneracionRer;
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                }
                model.Handson.TieneData = lista.Count() > 0 ? 1 : 0;

                if (idEnvio > 0) //Si se esta consultando un envio anterior se obtienen los cambios de ese envio.
                    listaCambios = this.logic.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                /// Cargar Datos en Arreglo para Web
                logic.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);

            }
            else
            { // los datos para visualizar en el excel web provienen de un archivo excel cargado por el usuario
                //Carga de archivo Excel
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                logic.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);

            }

            #region Filas Cabeceras

            for (var ind = 0; ind < model.ColumnasCabecera; ind++)
            {
                model.Handson.ListaColWidth.Add(160);
            }
            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();
            foreach (var reg in model.ListaHojaPto)
            {
                model.Handson.ListaColWidth.Add(100);
                for (var w = 0; w < model.FilasCabecera; w++)
                {
                    if (column == model.ColumnasCabecera)
                    {
                        model.Handson.ListaExcelData[w] = new string[model.ListaHojaPto.Count + model.ColumnasCabecera];
                        model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                    }
                    //
                    if (reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null) is Int32)
                        model.Handson.ListaExcelData[w][column] = (string)reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null).ToString();
                    else
                        model.Handson.ListaExcelData[w][column] = (string)reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null);
                    if (listaCabeceraRow[w].IsMerge == 1)
                    {
                        if (listaCabeceraRow[w].TituloRowAnt != model.Handson.ListaExcelData[w][column])
                        {
                            if (column != model.ColumnasCabecera)
                            {
                                if ((column - listaCabeceraRow[w].ColumnIni) > 1)
                                {
                                    cellMerge = new CeldaMerge();
                                    cellMerge.col = listaCabeceraRow[w].ColumnIni;
                                    cellMerge.row = w;
                                    cellMerge.colspan = (column - listaCabeceraRow[w].ColumnIni);
                                    cellMerge.rowspan = 1;
                                    model.Handson.ListaMerge.Add(cellMerge);
                                }
                            }
                            listaCabeceraRow[w].TituloRowAnt = model.Handson.ListaExcelData[w][column];
                            listaCabeceraRow[w].ColumnIni = column;
                        }
                    }
                }
                column++;

            }
            //if ((column - 1) != model.ColumnasCabecera)
            //{
            //    for (var i = 0; i < listaCabeceraRow.Count; i++)
            //    {
            //        if ((listaCabeceraRow[i].TituloRowAnt == model.Handson.ListaExcelData[i][column - 1]))
            //        {
            //            if ((column - listaCabeceraRow[i].ColumnIni) > 1)
            //            {
            //                cellMerge = new CeldaMerge();
            //                cellMerge.col = listaCabeceraRow[i].ColumnIni;
            //                cellMerge.row = i;
            //                cellMerge.colspan = (column - listaCabeceraRow[i].ColumnIni);
            //                cellMerge.rowspan = 1;
            //                model.Handson.ListaMerge.Add(cellMerge);
            //            }
            //        }
            //    }
            //}

            #endregion

            return model;
        }

        public PartialViewResult CargarSemanas(string idAnho)
        {
            FormatoModel model = new FormatoModel();
            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemana = entitys;
            return PartialView(model);
        }

        #region CUMPLIMIENTO

        public ActionResult Cumplimiento()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CumplimientoModel model = new CumplimientoModel();
            model.Fecha = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        [HttpPost]
        public ActionResult Listado(RdoCumplimiento rdo)
        {
            if (rdo.sRdofechaini is null) rdo.sRdofechaini = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
            rdo.Rdofechaini = DateTime.ParseExact(rdo.sRdofechaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<RdoCumplimiento> listadoRdo = new List<RdoCumplimiento>();
            if (ConstantesFormatoMedicion.ListadoFormatcodiExtranetTrCumpl.Contains(rdo.codFormato))
            {
                listadoRdo = logic.ListarRptCumplimientoExtranetHidrologiaTr(rdo.Rdofechaini, rdo.codFormato);
            }
            else
            {
                listadoRdo = _svcGestionRdo.GetByCriteriaRdoCumplimiento(rdo);
            }

            listadoRdo = listadoRdo.OrderBy(x => x.NombreEmpresa).ToList();

            return PartialView(listadoRdo);
        }

        #endregion


        [HttpPost]
        public ActionResult ListadoFallas(RdoCumplimiento rdo)
        {
            List<MeEnvioDTO> listadoEnvios = new List<MeEnvioDTO>();
            RdoCumplimiento cumplimientoResponse = new RdoCumplimiento();

            MeEnvioDTO envio = new MeEnvioDTO();

            envio.Periodo = rdo.Periodo;
            envio.TipoFalla = rdo.TipoFalla;
            envio.EtapaInforme = rdo.EtapaInforme;

            try
            {
                //RdoCumplimiento objRdoCumplimiento = new RdoCumplimiento();

                listadoEnvios = _svcGestionRdo.ListaEnviosPorEvento(envio);
                cumplimientoResponse.HtmlReporte = _svcGestionRdo.GenerarListaFallasHtml(listadoEnvios);
            }
            catch (Exception ex)
            {
                log.Error("Listado", ex);
            }

            return PartialView(cumplimientoResponse);
        }


        [HttpPost]
        public JsonResult ExcelCumplimiento(RdoCumplimiento rdo)
        {
            int indicador = 0;
            try
            {
                if (rdo.sRdofechaini is null) rdo.sRdofechaini = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
                rdo.Rdofechaini = DateTime.ParseExact(rdo.sRdofechaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<RdoCumplimiento> listadoRdo = new List<RdoCumplimiento>();

                //formato 
                MeFormatoDTO objFmt = logic.GetByIdMeFormato(rdo.codFormato);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporteRDO;


                if (ConstantesFormatoMedicion.ListadoFormatcodiExtranetTrCumpl.Contains(rdo.codFormato))
                {
                    listadoRdo = logic.ListarRptCumplimientoExtranetHidrologiaTr(rdo.Rdofechaini, rdo.codFormato);
                    listadoRdo = listadoRdo.OrderBy(x => x.NombreEmpresa).ToList();

                    logic.GeneraExcelCumplimientoExtranetHidrologiaTr(listadoRdo, objFmt.Formatnombre, ruta + "Cumplimiento.xlsx");
                }
                else
                {
                    if (rdo.codFormato == 130)
                    {
                        List<MeEnvioDTO> listadoEnvios = new List<MeEnvioDTO>();

                        MeEnvioDTO envio = new MeEnvioDTO();

                        envio.Periodo = rdo.Periodo;
                        envio.TipoFalla = rdo.TipoFalla;
                        envio.EtapaInforme = rdo.EtapaInforme;
                        listadoEnvios = _svcGestionRdo.ListaEnviosPorEvento(envio);
                        logic.GeneraExcelCumplimientoExtranetFallas(listadoEnvios, objFmt.Formatnombre, ruta + "Cumplimiento.xlsx");

                    }
                    else
                    {

                        listadoRdo = _svcGestionRdo.GetByCriteriaRdoCumplimiento(rdo);
                        listadoRdo = listadoRdo.OrderBy(x => x.NombreEmpresa).ToList();

                        logic.GeneraExcelCumplimientoExtranetHidrologiaTr(listadoRdo, objFmt.Formatnombre, ruta + "Cumplimiento.xlsx");
                    }
                }

                indicador = 1;
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = "Cumplimiento.xlsx";
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporteRDO;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #region Despacho de Generación RER NO CONVENCIONAL
        public ActionResult DespachoGeneracion()
        {
            FormatoModel model = new FormatoModel();
            model.IdModulo = Modulos.AppMedidoresDistribucion;
            model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => x.TIPOEMPRCODI == 2).Select(x =>
                new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
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
            //model.ListaSemanas = semanas;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);

            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        #endregion

    }
}

