using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.DemandaBarras.Helper;
using COES.MVC.Intranet.Areas.DemandaBarras.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Hidrologia;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DemandaBarras.Controllers
{
    public class EnvioController : BaseController
    {
        #region Propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionNombreArchivo] != null) ?
                    Session[DatosSesionDemanda.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionDemanda.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionFileName] != null) ?
                    Session[DatosSesionDemanda.SesionFileName].ToString() : null;
            }
            set { Session[DatosSesionDemanda.SesionFileName] = value; }
        }

        /// <summary>
        /// Codigo del envio
        /// </summary>
        public int IdEnvio
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionIdEnvio] != null) ?
                    (int)Session[DatosSesionDemanda.SesionIdEnvio] : 0;
            }
            set { Session[DatosSesionDemanda.SesionIdEnvio] = value; }
        }

        /// <summary>
        /// Nombre del formato
        /// </summary>
        public MeFormatoDTO Formato
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionFormato] != null) ?
                    (MeFormatoDTO)Session[DatosSesionDemanda.SesionFormato] : new MeFormatoDTO();
            }
            set { Session[DatosSesionDemanda.SesionFormato] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionMatrizExcel] != null) ?
                    (string[][])Session[DatosSesionDemanda.SesionMatrizExcel] : new string[1][];
            }
            set { Session[DatosSesionDemanda.SesionMatrizExcel] = value; }
        }

        /// <summary>
        /// Codigo de formato
        /// </summary>
        public int IdFormato = 4;
        public int IdLectura = 51;

        #endregion

        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();


        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int idEmpresa, string mes)
        {
            int indicador = 0;
            int idEnvio = 0;
            try
            {
                FormatoModel model = BuildHojaExcel(idEmpresa, idEnvio, mes);
                FormatoDemandaHelper.GenerarFileExcel(model);
                indicador = 1;


            }

            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras] + this.Formato.Formatnombre + ".xlsx";
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
                    this.FileName = fileRandom + "." + NombreArchivoDemanda.ExtensionFileUploadHidrologia;
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
        public JsonResult LeerFileUpExcel(int idEmpresa, string mes)
        {
            int retorno = FormatoDemandaHelper.VerificarIdsFormato(this.NombreFile, idEmpresa, this.IdFormato);

            if (retorno > 0)
            {
                MeFormatoDTO formato = logic.GetByIdMeFormato(this.IdFormato);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                var listaPtos = this.logic.GetByCriteria2MeHojaptomeds(idEmpresa, this.IdFormato, cabercera.Cabquery, formato.FechaProceso, formato.FechaProceso.AddMonths(1).AddDays(-1));
                int nCol = listaPtos.Count;
                int horizonte = formato.Formathorizonte;
                FormatoDemandaHelper.GetSizeFormato2(formato);
                int nBloques = formato.RowPorDia * formato.Formathorizonte;
                this.MatrizExcel = FormatoDemandaHelper.InicializaMatrizExcel(cabercera.Cabfilas, nBloques, cabercera.Cabcolumnas, nCol);
                Boolean isValido = FormatoDemandaHelper.LeerExcelFile(this.MatrizExcel, this.NombreFile, cabercera.Cabfilas, nBloques, cabercera.Cabcolumnas, nCol);
            }
            FormatoDemandaHelper.BorrarArchivo(this.NombreFile);
            return Json(retorno);
        }


        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuyera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        protected bool ValidarPlazo(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        protected bool ValidarFecha(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                var regfechaPlazo = this.logic.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            return true;
            //return resultado;
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
        public JsonResult GrabarExcelWeb(string dataExcel, int idEmpresa, string mes)
        {
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            int exito = 0;
            List<string> celdas = new List<string>();
            celdas = dataExcel.Split(',').ToList();
            string empresa = string.Empty;
            var regEmp = this.logic.GetByIdSiEmpresa(idEmpresa); ;
            //////////////////////////////////////////////////
            if (regEmp != null)
                empresa = regEmp.Emprnomb;

            MeFormatoDTO formato = this.logic.GetByIdMeFormato(this.IdFormato);

            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();

            int filaHead = cabercera.Cabfilas;
            int colHead = cabercera.Cabcolumnas;

            /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            FormatoDemandaHelper.GetSizeFormato2(formato);

            //formato.ListaHoja = this.logic.GetByCriteriaMeFormatohojas(this.IdFormato);
            var listaPto = this.logic.GetByCriteriaMeHojaptomeds(idEmpresa, this.IdFormato, formato.FechaInicio, formato.FechaFin);
            int nPtos = listaPto.Count();
            //////////////////////////////////////////////////////////////////////////

            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = this.IdFormato;
            config.Emprcodi = idEmpresa;
            config.FechaInicio = formato.FechaInicio;
            config.FechaFin = formato.FechaFin;
            int idConfig = logic.GrabarConfigFormatEnvio(config);


            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            Boolean enPlazo = ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

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
            envio.Formatcodi = this.IdFormato;
            this.IdEnvio = logic.SaveMeEnvio(envio);
            model.IdEnvio = this.IdEnvio;
            ///////////////////////////////////////////////////////
            int horizonte = formato.Formathorizonte;
            switch (formato.Formatresolucion)
            {
                case ConstantesDemanda.ResolucionCuartoHora:
                    int total = (nPtos + formato.Formatcols) * (filaHead + 96 * formato.Formathorizonte);
                    int totalRecibido = celdas.Count;

                    var lista96 = FormatoDemandaHelper.LeerExcelWeb96(celdas, listaPto, (int)formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 4 * formato.Formathorizonte, formato.Formatcheckblanco);
                    if (lista96.Count > 0)
                    {
                        try
                        {
                            this.logic.GrabarValoresCargados96(lista96, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            envio.Cfgenvcodi = idConfig;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Mensaje = MensajesDemanda.MensajeEnvioExito;
                        }
                        catch
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
            }

            model.Resultado = exito;
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
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, int idEnvio, string mes)
        {
            List<MeFormatoDTO> entitys = this.logic.GetByModuloLecturaMeFormatos(Modulos.AppMedidoresDistribucion, this.IdLectura, idEmpresa);

            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcel(idEmpresa, idEnvio, mes);
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
            model.ListaEmpresas = this.logic.ObtenerEmpresaPorFormato(this.IdFormato);

            if (model.ListaEmpresas.Count == 1)
            {
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            }

            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            return View(model);
        }

        /// <summary>
        /// Permite obtener el último envio
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerUltimoEnvio(int idEmpresa, string mes)
        {
            MeFormatoDTO formato = logic.GetByIdMeFormato(this.IdFormato);
            DateTime fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            List<MeEnvioDTO> list = this.logic.GetByCriteriaMeEnvios(idEmpresa, this.IdFormato, fecha);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (list.Count > 0)
            {
                idUltEnvio = list[list.Count - 1].Enviocodi;
            }
            return Json(idUltEnvio);
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
        public FormatoModel BuildHojaExcel(int idEmpresa, int idEnvio, string mes)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            model.IdEnvio = idEnvio;
            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = logic.GetByIdMeFormato(this.IdFormato);
            this.Formato = model.Formato;

            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;

            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;

            int idCfgFormato = 0;
            if (idEnvio <= 0)
            {
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                var envioAnt = logic.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
            }

            model.ListaHojaPto = this.logic.GetByCriteria2MeHojaptomeds(idEmpresa, this.IdFormato, cabercera.Cabquery, model.Formato.FechaProceso, model.Formato.FechaProceso.AddMonths(1).AddDays(-1));
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

            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            FormatoDemandaHelper.GetSizeFormato2(model.Formato);
            model.ListaEnvios = this.logic.GetByCriteriaMeEnvios(idEmpresa, this.IdFormato, model.Formato.FechaInicio);

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

            model.EnPlazo = ValidarPlazo(model.Formato);
            if ((idEnvio <= 0) || (idEnvio == idUltEnvio)) // id envio < 0 => es una carga de archivo excel, id envio = 0 => envio nuevo 
            {
                model.Handson.ReadOnly = !ValidarFecha(model.Formato, idEmpresa, out horaini, out horafin);//ValidarFecha(idEmpresa, model.Formato.FechaProceso, idFormato, out mensaje);
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

            model.Dia = model.Formato.FechaInicio.Day.ToString();
            model.Handson.Width = HandsonConstantes.ColWidth * ((model.ListaHojaPto.Count > HandsonConstantes.ColPorHoja) ? HandsonConstantes.ColPorHoja :
                (model.ListaHojaPto.Count + model.ColumnasCabecera));
            //Genera La vista html complementaria a la grilla Handson, nombre de formato, area coes, fecha de formato, etc.
            model.ViewHtml = FormatoDemandaHelper.GenerarFormatoHtml(model, idEnvio, model.EnPlazo);

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
            int nCol = model.ListaHojaPto.Count;
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;
            model.Handson.ListaFilaReadOnly = FormatoDemandaHelper.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques, !model.Handson.ReadOnly, horaini, horafin);
            model.ListaCambios = new List<CeldaCambios>();
            model.Handson.ListaExcelData = FormatoDemandaHelper.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);
            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {
                lista = this.logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                if (idEnvio > 0) //Si se esta consultando un envio anterior se obtienen los cambios de ese envio.
                    listaCambios = this.logic.GetAllCambioEnvio(this.IdFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList(); ;
                /// Cargar Datos en Arreglo para Web
                FormatoDemandaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }
            else
            { // los datos para visualizar en el excel web provienen de un archivo excel cargado por el usuario
                //Carga de archivo Excel
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                FormatoDemandaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
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
                    var valor = reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null);

                    if (valor != null)
                        model.Handson.ListaExcelData[w][column] = valor.ToString();
                    else
                        model.Handson.ListaExcelData[w][column] = string.Empty;

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


    }
}
