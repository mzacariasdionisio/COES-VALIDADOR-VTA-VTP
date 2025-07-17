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

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class CargaDatosController : BaseController
    {
        //
        // GET: /Hidrologia/CargaDatos/
        //
        // GET: /DemandaCP/Envio/

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
        public JsonResult GenerarFormato(int idEmpresa, int formatoReal, string fecha, string semana)
        {
            FormatoResultado modelResul = new FormatoResultado();
            int idEnvio = 0;
            string ruta = string.Empty;
            int IdFormato = logicHidro.ObtenerIdFormatoPadre(formatoReal);
            try
            {
                ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologiaCD.FolderUpload;
                FormatoModel model = BuildHojaExcel(idEmpresa, formatoReal, IdFormato, idEnvio, fecha, semana);
                logic.GenerarFileExcel(model, ruta, IdFormato);
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
            //string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras] + this.Formato.Formatnombre + ".xlsx";
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
        public JsonResult LeerFileUpExcel(int idEmpresa, int formatoReal, string fecha, string semana)
        {
            int IdFormato = logicHidro.ObtenerIdFormatoPadre(formatoReal);
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
                var listaPtos = logicHidro.GetByCriteria2MeHojaptomeds(idEmpresa, IdFormato, cabecera.Cabquery, formato.FechaInicio, formato.FechaFin).Where(x => x.Hojaptoactivo == 1).ToList();
                int nCol = listaPtos.Count;
                int horizonte = formato.Formathorizonte;
                int nBloques = formato.RowPorDia * formato.Formathorizonte;
                if (IdFormato == ConstantesHidrologiaCD.FormatoDiarioCodi)
                {
                    this.MatrizExcel = logic.InicializaMatrizExcel(formato.Formatrows, nBloques, formato.Formatcols, nCol);
                    Boolean isValido = logic.LeerExcelFile(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, nCol);
                }
                if (IdFormato == ConstantesHidrologiaCD.FormatoVolumenDIarioCodi)
                {
                    this.MatrizExcel = logic.InicializaMatrizExcelVolumenInicial(formato.Formatrows, nBloques, formato.Formatcols, nCol);
                    Boolean isValido = logic.LeerExcelFileVI(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, nCol);
                }
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
        public JsonResult GrabarExcelWeb(string dataExcel, string dataExcel2, int idEmpresa, int formatoReal, string fecha, string semana)
        {
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;

            try
            {
                base.ValidarSesionUsuario();

                int exito = 0;
                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();
                string empresa = string.Empty;
                var regEmp = this.logic.GetByIdSiEmpresa(idEmpresa);
                //////////////////////////////////////////////////
                if (regEmp != null)
                    empresa = regEmp.Emprnomb;
                int idFormato = logicHidro.ObtenerIdFormatoPadre(formatoReal);
                MeFormatoDTO formato = this.logic.GetByIdMeFormato(formatoReal);

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                formato.Formatcols = cabercera.Cabcolumnas;
                formato.Formatrows = cabercera.Cabfilas;
                formato.Formatheaderrow = cabercera.Cabcampodef;
                int filaHead = formato.Formatrows;
                int colHead = formato.Formatcols;

                /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
                formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, semana, fecha, Constantes.FormatoFecha);
                logic.GetSizeFormato2(formato);

                //formato.ListaHoja = this.logic.GetByCriteriaMeFormatohojas(IdFormato);
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
                Boolean enPlazo = logic.ValidarPlazoController(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

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
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        List<MeMedicion48DTO> listaData = serializer.Deserialize<List<MeMedicion48DTO>>(dataExcel2);
                        var fechaNew = new DateTime();
                        foreach (var reg in listaData)
                        {
                            fechaNew = DateTime.ParseExact(reg.MedifechaPto, ConstantesFormat.FormatoFechaHora, CultureInfo.InvariantCulture);
                            reg.Medifecha = new DateTime(fechaNew.Year, fechaNew.Month, fechaNew.Day);
                        }
                        //>Obetener Lectocodi
                        listaData.ForEach(x => x.Lectcodi = formato.Lectcodi);
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        //validar checklanco
                        logic.ValidarCheckBlancoExcelWeb48(listaData, formato.Formatcheckblanco);
                        //var lista48 = logic.LeerExcelWeb48(celdas, listaPto, (int)formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 2 * formato.Formathorizonte, formato.Formatcheckblanco);
                        var lista48 = listaData;
                        if (lista48.Count > 0)
                        {
                            this.logic.GrabarValoresCargados48(lista48, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Mensaje = MensajesDemandaCP.MensajeEnvioExito;
                        }
                        else
                        {
                            exito = -2;
                            model.Resultado = -2;
                        }
                        break;
                    case ParametrosFormato.ResolucionHora:
                        var lista24 = logic.LeerExcelWeb24(celdas, listaPto, formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * formato.Formathorizonte);
                        this.logic.GrabarValoresCargados24(lista24, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = this.IdEnvio;
                        logic.UpdateMeEnvio(envio);
                        exito = 1;
                        model.Resultado = 1;
                        model.Mensaje = MensajesDemandaCP.MensajeEnvioExito;
                        break;
                    case ParametrosFormato.ResolucionDia:

                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        List<MeMedicion1DTO> listaDataMed1 = serializer.Deserialize<List<MeMedicion1DTO>>(dataExcel2);
                        var fechaNewMe1 = new DateTime();

                        //calcula la fecha
                        listaDataMed1.ForEach(x => x.MedifechaPto = formato.FechaProceso.ToString(ConstantesFormat.FormatoFechaHora));
                        foreach (var reg in listaDataMed1)
                        {
                            fechaNewMe1 = DateTime.ParseExact(reg.MedifechaPto, ConstantesFormat.FormatoFechaHora, CultureInfo.InvariantCulture);
                            reg.Medifecha = new DateTime(fechaNewMe1.Year, fechaNewMe1.Month, fechaNewMe1.Day);
                        }
                        //>Obetener Lectocodi
                        listaDataMed1.ForEach(x => x.Lectcodi = formato.Lectcodi);
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        //validar checklanco
                        logic.ValidarCheckBlancoExcelWeb1(listaDataMed1, formato.Formatcheckblanco);
                        var lista1 = listaDataMed1;
                        if (lista1.Count > 0)
                        {
                            this.logic.GrabarValoresCargados1(lista1, User.Identity.Name, this.IdEnvio, idEmpresa, formato, formato.Lectcodi);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Mensaje = MensajesDemandaCP.MensajeEnvioExito;
                        }
                        else
                        {
                            exito = -2;
                            model.Resultado = -2;
                        }
                        break;
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

            //Enviar Correo de exito de envio
            //FormatoWebHelper.EnviarCorreo(formato.Formatnombre, enPlazo, empresa, formato.FechaInicio, formato.FechaFin, formato.Areaname, User.Identity.Name, (DateTime)envio.Enviofecha, envio.Enviocodi);
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
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, int formatoReal, int idEnvio, string fecha, string semana)
        {
            int IdFormato = logicHidro.ObtenerIdFormatoPadre(formatoReal);
            var totalPto = this.logic.GetByCriteriaMeHojaptomeds(idEmpresa, IdFormato, DateTime.Now.Date, DateTime.Now.Date);
            var formatoPto = IdFormato;

            if (totalPto.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcel(idEmpresa, formatoReal, formatoPto, idEnvio, fecha, semana);
                Session["DatosJSON"] = jsModel.Handson.ListaExcelData;
                jsModel.Handson.ListaExcelData = new string[0][];
                return Json(jsModel);
            }
            else
            {
                Session["DatosJSON"] = null;
                return Json(-1);
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
            var cabcab = logic.GetListMeCabecera();
            var cabecera = logicHidro.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            model.Formato.Formatcols = cabecera.Cabcolumnas;
            model.Formato.Formatrows = cabecera.Cabfilas;
            model.Formato.Formatheaderrow = cabecera.Cabcampodef;

            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;
            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, semana, fecha, Constantes.FormatoFecha);
            logic.GetSizeFormato2(model.Formato);
            model.ListaHojaPto = logicHidro.GetByCriteria2MeHojaptomeds(idEmpresa, formatoPto, cabecera.Cabquery, model.Formato.FechaInicio, model.Formato.FechaFin)
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

            model.ListaEnvios = logicHidro.GetByCriteriaMeEnvios(idEmpresa, idFormato, model.Formato.FechaInicio);

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
            if (formatoPto == ConstantesHidrologiaCD.FormatoDiarioCodi)
                model.Handson.ListaExcelData = logic.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);
            if (formatoPto == ConstantesHidrologiaCD.FormatoVolumenDIarioCodi)
                model.Handson.ListaExcelData = logic.InicializaMatrizExcelVolumenInicial(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);
            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {
                lista = this.logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                if (idEnvio > 0) //Si se esta consultando un envio anterior se obtienen los cambios de ese envio.
                    listaCambios = this.logic.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                /// Cargar Datos en Arreglo para Web
                if (formatoPto == ConstantesHidrologiaCD.FormatoDiarioCodi)
                    logic.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
                if (formatoPto == ConstantesHidrologiaCD.FormatoVolumenDIarioCodi)
                    logic.ObtenerListaExcelDataM1(model, lista, listaCambios, idEnvio);
            }
            else
            { // los datos para visualizar en el excel web provienen de un archivo excel cargado por el usuario
                //Carga de archivo Excel
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                if (formatoPto == ConstantesHidrologiaCD.FormatoDiarioCodi)
                    logic.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
                if (formatoPto == ConstantesHidrologiaCD.FormatoVolumenDIarioCodi)
                    logic.ObtenerListaExcelDataM1(model, lista, listaCambios, idEnvio);
            }

            //llenado cabecera
            for (var ind = 0; ind < model.ColumnasCabecera; ind++)
            {
                model.Handson.ListaColWidth.Add(160);
            }

            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();

            if (formatoPto == ConstantesHidrologiaCD.FormatoDiarioCodi)
            {
                #region carga cabecera hidrologia

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

                #endregion
            }
            if (formatoPto == ConstantesHidrologiaCD.FormatoVolumenDIarioCodi)
            {
                #region Filas Cabecera
                //ingresas nombres de puntos
                var m = 0;
                model.ListaHojaPto = model.ListaHojaPto.OrderBy(x => x.Hojaptoorden).ToList();
                foreach (var listAgrup in model.ListaHojaPto.GroupBy(x => x.Ptomedicodi))
                {
                    //listAgrup.OrderBy(x => x.Hojaptoorden);
                    var entidad = listAgrup.First();
                    model.Handson.ListaColWidth.Add(100);
                    model.Handson.ListaExcelData[m + model.FilasCabecera][0] = (string)entidad.GetType().GetProperty("Equinomb").GetValue(entidad, null);
                    m++;
                }

                //ingresa cabecera
                for (var w = 0; w < model.FilasCabecera; w++)
                {
                    if (column == model.ColumnasCabecera)
                    {
                        model.Handson.ListaExcelData[w] = new string[4];
                        model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;

                        model.Handson.ListaExcelData[w][1] = "VOLUMEN MÍNIMO";
                        model.Handson.ListaExcelData[w][2] = "VOLUMEN MÁXIMO";
                        model.Handson.ListaExcelData[w][3] = "VOLUMEN INICIAL";

                    }
                    else
                    {
                        model.Handson.ListaExcelData[w] = new string[4];
                        model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                        model.Handson.ListaExcelData[w][1] = "MH3";
                        model.Handson.ListaExcelData[w][2] = "MH3";
                        model.Handson.ListaExcelData[w][3] = "MH3";
                    }
                    column++;
                }
                #endregion
            }

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

        #region Volumen Inicial

        /// <summary>
        /// Pantalla principal de craga de Volumen Inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexVolumenInicial()
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
