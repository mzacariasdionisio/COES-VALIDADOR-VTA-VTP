using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Hidrologia.Helper;
using COES.MVC.Extranet.Areas.Hidrologia.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Hidrologia.Controllers
{
    public class EnvioController : BaseController
    {
        private readonly HidrologiaAppServicio servHidro;
        private readonly FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        private readonly SeguridadServicio.SeguridadServicioClient seguridad;
        private readonly GeneralAppServicio logicGeneral;

        private int IdAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

        public bool[] ListaPermisos
        {
            get
            {
                return (Session["ListaPermisos"] != null) ?
                    (bool[])Session["ListaPermisos"] : new bool[14];
            }
            set { Session["ListaPermisos"] = value; }
        }

        public String NombreFile
        {
            get
            {
                return (Session[DatosSesion.SesionNombreArchivo] != null) ?
                    Session[DatosSesion.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesion.SesionNombreArchivo] = value; }
        }

        public int IdEnvio
        {
            get
            {
                return (Session[DatosSesion.SesionIdEnvio] != null) ?
                    (int)Session[DatosSesion.SesionIdEnvio] : 0;
            }
            set { Session[DatosSesion.SesionIdEnvio] = value; }
        }

        public int Empresa
        {
            get
            {
                return (Session[DatosSesion.SesionEmpresa] != null) ?
                    (int)Session[DatosSesion.SesionEmpresa] : 0;
            }
            set { Session[DatosSesion.SesionEmpresa] = value; }
        }

        public string[][] MatrizExcel
        {
            get
            {
                return (Session["MatrizExcel"] != null) ?
                    (string[][])Session["MatrizExcel"] : new string[1][];
            }
            set { Session["MatrizExcel"] = value; }
        }

        #endregion

        public EnvioController()
        {
            servHidro = new HidrologiaAppServicio();
            logicGeneral = new GeneralAppServicio();
            seguridad = new SeguridadServicio.SeguridadServicioClient();
        }

        /// <summary>
        /// Index para el envio de archivo
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexExcelWeb()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            ///Llamar a funcion que carga informacion para los filtros de formato    
            FormatoModel model = new FormatoModel();
            model.IdArea = 0;
            CargarFiltrosFormato(model);

            //Escoger fechas de acuerdo a vencimiento.
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Editable = 1;
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.FechaHoy = DateTime.Today.ToString(Constantes.FormatoFecha);

            return View(model);
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
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, int idFormato, int idEnvio, string fecha, string semana, string mes, int verUltimoEnvio, int? bloqueHorario)
        {
            FormatoModel jsModel = new FormatoModel();
            try
            {
                base.ValidarSesionUsuario();

                switch (idFormato)
                {
                    case 41:
                    case 42:
                        jsModel = FormatoVariado(idEmpresa, idFormato, idEnvio, fecha, semana, mes);
                        break;
                    default:
                        jsModel = BuildHojaExcel(idEmpresa, idFormato, idEnvio, fecha, semana, mes, verUltimoEnvio, bloqueHorario ?? 24);
                        break;

                }
                jsModel.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(ConstantesAppServicio.LogError, ex);
                jsModel.Resultado = "-1";
                jsModel.Mensaje = ex.ToString();
                jsModel.DetalleMensaje = ex.StackTrace;
            }

            return Json(jsModel);
        }

        /// <summary>
        ///Devuelve el model hidrologia con todos sus capos llenos para mostrar el aexceweb en la interfaz web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        private FormatoModel BuildHojaExcel(int idEmpresa, int idFormato, int idEnvio, string fecha, string semana, string mes, int verUltimoEnvio, int bloqueHorario)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();

            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = logic.GetByIdMeFormato(idFormato);
            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();

            /// DEFINICION DEL FORMATO //////
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;

            ///
            var entEmpresa = logic.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
            {
                model.Empresa = entEmpresa.Emprnomb;
                model.EsEmpresaVigente = logic.EsEmpresaVigente(idEmpresa, DateTime.Now);
            }

            ///
            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
            model.FiltroBloqueHorario = bloqueHorario;

            //Mostrar último envio cuando se muestra la interfaz de Carga de datos de un formato
            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            model.ListaEnvios = logic.GetByCriteriaMeEnvios(idEmpresa, idFormato, model.Formato.FechaProceso);
            if (model.ListaEnvios.Count > 0)
            {
                model.IdEnvioLast = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                idUltEnvio = model.IdEnvioLast;
                if (ConstantesFormato.VerUltimoEnvio == verUltimoEnvio)
                {
                    idEnvio = model.IdEnvioLast;
                }
            }

            int idCfgFormato = 0;
            model.Formato.IdEnvio = idEnvio;
            /// Verifica si Formato esta en Plaz0
            string mensaje = string.Empty;
            if (idEnvio <= 0)
            {
                model.Formato.Emprcodi = idEmpresa;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                model.EnPlazo = logic.ValidarPlazo(model.Formato);
                model.TipoPlazo = logic.EnvioValidarPlazo(model.Formato, idEmpresa);
                model.Handson.ReadOnly = ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == model.TipoPlazo;
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = true;

                var envioAnt = logic.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaEnvio = envioAnt.Enviofecha;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Enviobloquehora > 0) model.FiltroBloqueHorario = envioAnt.Enviobloquehora.Value;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                    model.EnPlazo = envioAnt.Envioplazo == "P";
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
            }

            //
            model.ListaHojaPto = logic.GetListaPtos(model.Formato.FechaFin, idCfgFormato, idEmpresa, idFormato, cabercera.Cabquery);

            #region Check para horario seleccionado

            model.ListaHojaPto = HidrologiaHelper.ListaHojaptoCheck(model.Formato.FechaProceso, model.ListaHojaPto);

            #endregion

            servHidro.ListarConfigPlazoXFormatoYFechaPeriodo(idFormato, model.ListaHojaPto, model.Formato.FechaProceso);

            if ((idFormato == 36) || (idFormato == 35) || (idFormato == 34) || (idFormato == 33))
            {
                foreach (var reg in model.ListaHojaPto)
                {
                    reg.Tipoinfoabrev = "m3/s";
                }

            }
            model.OpGrabar = this.ListaPermisos[Acciones.Grabar];
            model.OpAccesoEmpresa = this.ListaPermisos[Acciones.AccesoEmpresa];
            model.OpEditar = this.ListaPermisos[Acciones.Editar];
            var cabecerasRow = cabercera.Cabcampodef.Split(QueryParametros.SeparadorFila);
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

            model.Editable = model.Handson.ReadOnly ? 1 : 0;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Semana = semana;
            model.Dia = model.Formato.FechaProceso.Day.ToString();

            model.Handson.Width = HandsonConstantes.ColWidth * ((model.ListaHojaPto.Count > HandsonConstantes.ColPorHoja) ? HandsonConstantes.ColPorHoja :
                (model.ListaHojaPto.Count + model.ColumnasCabecera));
            //Genera La vista html complementaria a la grilla Handson, nombre de formato, area coes, fecha de formato, etc.
            model.ViewHtml = HidrologiaHelper.GenerarFormatoHtml(model, idEnvio, model.EnPlazo);

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.

            int nCol = model.ListaHojaPto.Count;
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;
            model.Handson.ListaFilaReadOnly = HidrologiaHelper.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques);

            short[][] matrizTipoEstado = HidrologiaHelper.IncializaMatrizEstado(model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia
                                            , model.Formato.Formatcols, 0, model.Formato.Formathorizonte, model.Formato.FechaProceso, model.Formato.Areacode == 3);
            model.Handson.MatrizTipoEstado = matrizTipoEstado;

            model.ListaCambios = new List<CeldaCambios>();
            model.Handson.ListaExcelData = HidrologiaHelper.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);
            if (idEnvio == 0)
            {
                lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                HidrologiaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio, model.FiltroBloqueHorario);
            }
            if (idEnvio > 0)
            {
                lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                listaCambios = logic.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                HidrologiaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio, model.FiltroBloqueHorario);
            }
            if (idEnvio < 0)
            {
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                HidrologiaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio, model.FiltroBloqueHorario);
            }

            #region Filas Cabeceras

            for (var ind = 0; ind < model.ColumnasCabecera; ind++)
            {
                model.Handson.ListaColWidth.Add(120);
            }
            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();
            foreach (var reg in model.ListaHojaPto)
            {
                int anchoXCol = 100;
                if (reg.Hptoindcheck == "S") anchoXCol = 50;
                model.Handson.ListaColWidth.Add(anchoXCol);

                for (var w = 0; w < model.FilasCabecera; w++)
                {
                    if (column == model.ColumnasCabecera)
                    {
                        model.Handson.ListaExcelData[w] = new string[model.ColumnasCabecera + nCol];
                        model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                    }
                    if (reg.Hptoindcheck == "S")
                    {
                        model.Handson.ListaExcelData[w][column] = "";
                    }
                    else
                    {
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
                }
                column++;
            }
            if ((column - 1) != model.ColumnasCabecera)
            {
                for (var i = 0; i < listaCabeceraRow.Count; i++)
                {
                    if ((listaCabeceraRow[i].TituloRowAnt == model.Handson.ListaExcelData[i][column - 1]))
                    {
                        if ((column - listaCabeceraRow[i].ColumnIni) > 1)
                        {
                            cellMerge = new CeldaMerge();
                            cellMerge.col = listaCabeceraRow[i].ColumnIni;
                            cellMerge.row = i;
                            cellMerge.colspan = (column - listaCabeceraRow[i].ColumnIni);
                            cellMerge.rowspan = 1;
                            model.Handson.ListaMerge.Add(cellMerge);
                        }
                    }
                }
            }

            #endregion

            model.IdEnvio = idEnvio;
            return model;
        }

        /// <summary>
        /// Obtiene model para  que se visualice el Formato
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        private FormatoModel FormatoVariado(int idEmpresa, int idFormato, int idEnvio, string fecha, string semana, string mes)
        {
            FormatoModel model = new FormatoModel();

            model.Formato = logic.GetByIdMeFormato(idFormato);
            model.Handson = new HandsonModel();
            //model.Handson.ListaFilaReadOnly = true;
            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            var cabecerasRow = cabercera.Cabcampodef.Split(QueryParametros.SeparadorFila);
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;
            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
            model.Fecha = model.Formato.FechaProceso.ToString(Constantes.FormatoFecha);
            model.ListaEnvios = logic.GetByCriteriaMeEnvios(idEmpresa, idFormato, model.Formato.FechaProceso);
            model.ListaHojaPto = logic.GetByCriteria2MeHojaptomeds(idEmpresa, idFormato, cabercera.Cabquery, model.Formato.FechaProceso, model.Formato.FechaProceso).Where(x => x.Hojaptoactivo == 1).ToList();
            model.ListaPtoMedicion = model.ListaHojaPto.Select(x => new ListaSelect
            {
                id = x.Ptomedicodi,
                text = x.Ptomedibarranomb
            }).ToList();

            model.EnPlazo = false;
            model.TipoPlazo = ConstantesEnvio.ENVIO_FUERA_PLAZO;

            var entEmpresa = logic.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
                model.Empresa = entEmpresa.Emprnomb;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Semana = semana;
            model.Dia = model.Formato.FechaProceso.Day.ToString();

            int nCol = cabercera.Cabcolumnas;
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ListaColWidth.Add(70);
            model.Handson.ListaColWidth.Add(260);
            model.Handson.ListaColWidth.Add(100);
            model.Handson.ListaColWidth.Add(100);
            model.Handson.ListaColWidth.Add(90);
            model.Handson.ListaColWidth.Add(0);
            model.Handson.ListaColWidth.Add(320);
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }
            var listaData = logic.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, model.Formato.FechaProceso,
               model.Formato.FechaProceso.AddDays(1).AddSeconds(-1));
            int nBloques = listaData.Count;
            model.Handson.ListaExcelData = HidrologiaHelper.InicializaMatrizExcel2(listaCabeceraRow, model.Formato.Formatrows, nBloques, 0, nCol, model.Fecha);
            HidrologiaHelper.LoadMatrizExcel2(model.Handson.ListaExcelData, listaData, nCol);

            return model;
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormato(int idEmpresa, int idFormato, string fecha, string semana, string mes, int? bloqueHorario)
        {
            FormatoResultado modelResultado = new FormatoResultado();

            try
            {
                int idEnvio = 0;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;

                FormatoModel model = BuildHojaExcel(idEmpresa, idFormato, idEnvio, fecha, semana, mes, ConstantesFormato.NoVerUltimoEnvio, bloqueHorario ?? 24);

                string nombreArchivo = string.Format("{0}_{1}.xlsx", model.Formato.Formatnombre, DateTime.Now.Millisecond);
                HidrologiaHelper.GenerarFileExcel(model, ruta, nombreArchivo);

                modelResultado.Resultado = 1;
                modelResultado.Archivo = nombreArchivo;
            }
            catch (Exception ex)
            {
                Log.Error(ConstantesAppServicio.LogError, ex);
                modelResultado.Mensaje = ex.Message;
                modelResultado.Resultado = -1;
            }

            return Json(modelResultado);
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string strArchivoTemporal = Request["archivo"];

            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
            string fullPath = ruta + strArchivoTemporal;
            return File(fullPath, Constantes.AppExcel, strArchivoTemporal);
        }

        /// <summary>
        /// Recibe formato cargado interface web
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string sfileName = fileRandom + "." + NombreArchivoHidro.ExtensionFileUploadHidrologia;
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    string fileName = ruta + sfileName;
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
        /// Lee el archivo excel el cual sera cargado en la interfaz excel web.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns> devuelve vrdadero si la carga ha sido correcta
        public JsonResult LeerFileUpExcel(int idEmpresa, string fecha, string semana, string mes, int idFormato)
        {
            //Leer Lista de Puntos
            // Leer  Formato
            //Definir Matriz de Puntos 
            //Recorrer Excel y llenar Matriz de Puntos
            //Verificar Formato

            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();

            int retorno = HidrologiaHelper.VerificarIdsFormato(this.NombreFile, idEmpresa, idFormato);

            if (retorno > 0)
            {
                MeFormatoDTO formato = logic.GetByIdMeFormato(idFormato);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                var cabecera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                var cabecerasRow = cabecera.Cabcampodef.Split(QueryParametros.SeparadorFila);
                formato.Formatcols = cabecera.Cabcolumnas;
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatheaderrow = cabecera.Cabcampodef;
                FormatoMedicionAppServicio.GetSizeFormato(formato);

                var listaPtos = logic.GetByCriteria2MeHojaptomeds(idEmpresa, idFormato, cabecera.Cabquery, formato.FechaInicio, formato.FechaFin);
                listaPtos = listaPtos.Where(x => x.Hojaptoactivo == 1).ToList();
                #region Check para horario seleccionado

                listaPtos = HidrologiaHelper.ListaHojaptoCheck(formato.FechaProceso, listaPtos);

                #endregion
                int nCol = listaPtos.Count;

                int horizonte = formato.Formathorizonte;
                int nBloques = formato.RowPorDia * formato.Formathorizonte;

                this.MatrizExcel = HidrologiaHelper.InicializaMatrizExcel(formato.Formatrows, nBloques, formato.Formatcols, nCol);
                Boolean isValido = HidrologiaHelper.LeerExcelFile(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, nCol);
                //}
            }
            //Borrar Archivo
            HidrologiaHelper.BorrarArchivo(this.NombreFile);
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
        public JsonResult GrabarExcelWeb(string dataExcel, int idFormato, int idEmpresa, string fecha, string semana, string mes, int? bloqueHorario)
        {
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            try
            {
                base.ValidarSesionUsuario();

                ///////// Definicion de Variables ////////////////
                int exito = 0;
                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();
                string empresa = string.Empty;
                var regEmp = logic.GetByIdSiEmpresa(idEmpresa); ;
                //////////////////////////////////////////////////
                if (regEmp != null)
                {
                    empresa = regEmp.Emprnomb;
                    if (!this.logic.EsEmpresaVigente(idEmpresa, DateTime.Now))
                    {
                        return Json(model);
                    }
                }

                MeFormatoDTO formato = logic.GetByIdMeFormato(idFormato);
                var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                formato.Formatcols = cabercera.Cabcolumnas;
                formato.Formatrows = cabercera.Cabfilas;
                formato.Formatheaderrow = cabercera.Cabcampodef;
                int filaHead = formato.Formatrows;
                int colHead = formato.Formatcols;

                /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
                formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
                FormatoMedicionAppServicio.GetSizeFormato(formato);

                int idCfgFormato = 0;
                var listaPto = logic.GetListaPtos(formato.FechaFin, idCfgFormato, idEmpresa, idFormato, cabercera.Cabquery);

                #region Check para horario seleccionado

                listaPto = HidrologiaHelper.ListaHojaptoCheck(formato.FechaProceso, listaPto);

                #endregion

                int nPtos = listaPto.Count();

                servHidro.ListarConfigPlazoXFormatoYFechaPeriodo(idFormato, listaPto, formato.FechaProceso);

                formato.Emprcodi = idEmpresa;
                string tipoPlazo = logic.EnvioValidarPlazo(formato, idEmpresa);
                if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == tipoPlazo)
                    throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + formato.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));

                /////////////// Grabar Config Formato Envio //////////////////
                MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
                config.Formatcodi = idFormato;
                config.Emprcodi = idEmpresa;
                config.FechaInicio = formato.FechaInicio;
                config.FechaFin = formato.FechaFin;
                int idConfig = logic.GrabarConfigFormatEnvio(config);
                ///////////////Grabar Envio//////////////////////////
                string mensajePlazo = string.Empty;
                Boolean enPlazo = logic.ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);
                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = idEmpresa;
                envio.Enviofecha = DateTime.Now;
                envio.Enviofechaperiodo = formato.FechaProceso;
                envio.Enviofechaini = formato.FechaInicio;
                envio.Enviofechafin = formato.FechaFin;
                envio.Envioplazo = (enPlazo) ? "P" : "F";
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Lastdate = DateTime.Now;
                envio.Lastuser = User.Identity.Name;
                envio.Userlogin = User.Identity.Name;
                envio.Formatcodi = idFormato;
                envio.Cfgenvcodi = idConfig;
                envio.Enviobloquehora = bloqueHorario;
                envio.Enviofechaplazoini = formato.FechaPlazoIni;
                envio.Enviofechaplazofin = formato.FechaPlazoFuera;
                this.IdEnvio = logic.SaveMeEnvio(envio);
                model.IdEnvio = this.IdEnvio;
                ///////////////////////////////////////////////////////
                int horizonte = formato.Formathorizonte;
                switch (formato.Formatresolucion)
                {
                    case ParametrosFormato.ResolucionCuartoHora:
                        int total = (nPtos + formato.Formatcols) * (filaHead + 96 * formato.Formathorizonte);
                        int totalRecibido = celdas.Count;

                        var lista96 = HidrologiaHelper.LeerExcelWeb96(celdas, listaPto, formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 4 * formato.Formathorizonte, formato.Formatcheckblanco);
                        if (lista96.Count > 0)
                        {
                            try
                            {
                                logic.GrabarValoresCargados96(lista96, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                                envio.Enviocodi = this.IdEnvio;
                                logic.UpdateMeEnvio(envio);
                                exito = 1;
                                model.Mensaje = MensajesHidro.MensajeEnvioExito;
                            }
                            catch (Exception ex)
                            {
                                Log.Error(NameController, ex);
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
                    case ParametrosFormato.ResolucionMediaHora:
                        try
                        {
                            var lista48 = HidrologiaHelper.LeerExcelWeb48(celdas, listaPto, formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 2 * formato.Formathorizonte, formato.Formatcheckblanco);
                            logic.GrabarValoresCargados48(lista48, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Resultado = 1;
                            model.Mensaje = MensajesHidro.MensajeEnvioExito;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(NameController, ex);
                            exito = -1;
                            model.Resultado = -1;
                        }
                        break;
                    case ParametrosFormato.ResolucionHora:
                        try
                        {
                            var lista24 = HidrologiaHelper.LeerExcelWeb24(celdas, listaPto, formato.Lectcodi, colHead, nPtos, filaHead, 24 * formato.Formathorizonte);
                            logic.GrabarValoresCargados24(lista24, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Resultado = 1;
                            model.Mensaje = MensajesHidro.MensajeEnvioExito;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(NameController, ex);
                            exito = -1;
                            model.Resultado = -1;
                        }
                        break;
                    case ParametrosFormato.ResolucionDia:
                    case ParametrosFormato.ResolucionMes:
                    case ParametrosFormato.ResolucionSemana:
                        try
                        {
                            var lista1 = HidrologiaHelper.LeerExcelWeb1(celdas, listaPto, formato.Lectcodi, (int)formato.Formatperiodo, colHead, nPtos, filaHead, formato.Formathorizonte);
                            logic.GrabarValoresCargados1(lista1, User.Identity.Name, this.IdEnvio, idEmpresa, formato, formato.Lectcodi);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Resultado = 1;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(NameController, ex);
                            exito = -1;
                            model.Resultado = -1;
                        }
                        break;
                }

                model.Resultado = exito;
                //Enviar Correo de exito de envio
                var lectura = logic.GetByIdMeLectura(formato.Lectcodi);
                string stLectura = string.Empty;
                if (lectura != null)
                    stLectura = lectura.Lectnomb;
                HidrologiaHelper.EnviarCorreo(stLectura, formato.Formatnombre, enPlazo, empresa, formato.FechaInicio, formato.FechaFin, formato.Areaname, User.Identity.Name, (DateTime)envio.Enviofecha, envio.Enviocodi);

                //ok
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(ConstantesAppServicio.LogError, ex);
                model.Resultado = -1;
                model.Mensaje = ex.ToString();
                model.DetalleMensaje = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Graba datos para formato tipo 2
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarTipo2(string[][] data, int idFormato, int idEmpresa, string fecha)
        {
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;

            try
            {
                base.ValidarSesionUsuario();

                int idEnvio = 0;
                List<MeMedicionxintervaloDTO> entitys = this.ObtenerDatos(data);
                MeFormatoDTO formato = logic.GetByIdMeFormato(idFormato);
                formato.FechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                FormatoMedicionAppServicio.GetSizeFormato(formato);
                ///////////////Grabar Envio//////////////////////////
                string mensajePlazo = string.Empty;
                Boolean enPlazo = logic.ValidarPlazo(formato);

                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = idEmpresa;
                envio.Enviofecha = DateTime.Now;
                envio.Enviofechaperiodo = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                envio.Enviofechaini = formato.FechaInicio;
                envio.Enviofechafin = formato.FechaFin;
                envio.Envioplazo = (enPlazo) ? "P" : "F";
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Lastdate = DateTime.Now;
                envio.Lastuser = User.Identity.Name;
                envio.Userlogin = User.Identity.Name;
                envio.Formatcodi = idFormato;
                idEnvio = logic.SaveMeEnvio(envio);

                logic.GrabarMedicionesXIntevalo(entitys, User.Identity.Name, idEnvio, idEmpresa, formato);

                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                logic.UpdateMeEnvio1(envio);

                //ok
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(ConstantesAppServicio.LogError, ex);
                model.Resultado = -1;
                model.Mensaje = ex.ToString();
                model.DetalleMensaje = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Obtiene MAtriz de datos del formato web
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private List<MeMedicionxintervaloDTO> ObtenerDatos(string[][] datos)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            MeMedicionxintervaloDTO entity;
            if (datos.Length > 1)
            {
                for (int i = 1; i < datos.Length; i++)
                {
                    entity = new MeMedicionxintervaloDTO();
                    entity.Medintfechaini = DateTime.ParseExact(datos[i][0] + " " + datos[i][2],
                        Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    entity.Medintfechafin = DateTime.ParseExact(datos[i][0] + " " + datos[i][3],
                        Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    entity.Ptomedicodi = int.Parse(datos[i][1]);
                    entity.Medintusumodificacion = User.Identity.Name;
                    entity.Medintfecmodificacion = DateTime.Now;
                    entity.Tipoinfocodi = ConstantesHidrologia.Caudal;
                    entity.Lectcodi = ConstantesHidrologia.EjectutadoTR;
                    entity.Medinth1 = decimal.Parse(datos[i][4]);
                    entity.Emprcodi = Int32.Parse(datos[i][5]);
                    entity.Medintdescrip = datos[i][6];
                    lista.Add(entity);
                }
            }

            return lista;
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
        /// Actualiza la informacion necesaria para cargar los filtros para la eleccion de los formatos
        /// </summary>
        /// <param name="model"></param>
        private void CargarFiltrosFormato(FormatoModel model)
        {

            int idOrigen = ConstantesHidrologia.IdOrigenHidro;
            int idOpcion = (Session[DatosSesion.SesionIdOpcion] == null) ? 0 : (int)Session[DatosSesion.SesionIdOpcion];
            //Cargar Permisos
            this.ListaPermisos = new bool[14];
            //this.ListaPermisos[Permisos.Grabar] = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, idOpcion, Permisos.Grabar, User.Identity.Name);
            this.ListaPermisos[Acciones.Grabar] = true;
            this.ListaPermisos[Acciones.AccesoEmpresa] = seguridad.ValidarPermisoOpcion(this.IdAplicacion, idOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
            this.ListaPermisos[Acciones.Editar] = seguridad.ValidarPermisoOpcion(this.IdAplicacion, idOpcion, Acciones.Editar, User.Identity.Name);

            model.OpGrabar = this.ListaPermisos[Acciones.Grabar];
            model.OpAccesoEmpresa = this.ListaPermisos[Acciones.AccesoEmpresa];
            model.OpEditar = this.ListaPermisos[Acciones.Editar];
            var empresas = logicGeneral.ObtenerEmpresasHidro();
            if (model.OpAccesoEmpresa)
            {
                model.ListaEmpresas = empresas; //FormatoHelper.ObtenerEmpresasModulo(model.IdModulo);
            }
            else
            {
                var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                    Select(x => new SiEmpresaDTO()
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB
                    });
                model.ListaEmpresas = emprUsuario.ToList();//logic.ObtenerEmpresasPorUsuario(User.Identity.Name);
            }
            model.ListaAreas = logic.ListAreaXFormato(idOrigen);
            if ((model.IdArea == 0) && (model.ListaAreas.Count > 0))
                model.IdArea = model.ListaAreas[0].Areacode;
            model.ListaFormato = logic.ListMeFormatos().Where(x => x.Modcodi == ConstantesHidrologia.IdModulo).ToList(); //lista de todos los formatos para hidrologia
            //if (model.ListaFormato.Count > 0)
            //    model.IdFormato = model.ListaFormato[0].Formatcodi;
            model.ListaLectura = logic.ListMeLecturas().Where(x => x.Origlectcodi == idOrigen && x.Areacode == model.IdArea).ToList();
            List<int> listaLecturaOmitir = new List<int>() { 243, 242, 239 }; // COMPROMISO HIDRÁULICO - NO, COMPROMISO HIDRÁULICO - SI, EJECUTADO HISTORICO RDO
            model.ListaLectura = model.ListaLectura.Where(x => !listaLecturaOmitir.Contains(x.Lectcodi)).ToList();

            if (model.ListaLectura.Count > 0 && (model.IdLectura == 0))
                model.IdLectura = model.ListaLectura[0].Lectcodi;
            if ((model.ListaEmpresas.Count > 0) && (model.IdEmpresa == 0))
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, Constantes.CaracterCero));
            }
            model.ListaSemanas = semanas;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            string validacion = string.Empty;
            List<int> listaFormatCodi = new List<int>();
            List<int> listaFormatPeriodo = new List<int>();
            List<string> listastrFormatDescrip = new List<string>();
            foreach (var reg in model.ListaFormato)
            {
                listaFormatCodi.Add(reg.Formatcodi);
                listaFormatPeriodo.Add((int)reg.Formatperiodo);
                listastrFormatDescrip.Add(reg.Formatdescrip);
            }
            model.StrFormatCodi = String.Join(",", listaFormatCodi);
            model.StrFormatPeriodo = String.Join(",", listaFormatPeriodo);
            model.StrFormatDescrip = String.Join(",", listastrFormatDescrip);
            model.ListaFormato = logic.GetByModuloLecturaMeFormatos(ConstantesHidrologia.IdModulo, model.IdLectura, model.IdEmpresa);
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho)
        {
            BusquedaModel model = new BusquedaModel();
            List<TipoInformacion> entitys = new List<TipoInformacion>();
            if (idAnho == "0")
            {
                idAnho = DateTime.Now.Year.ToString();
            }
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = EPDate.TotalSemanasEnAnho(Int32.Parse(idAnho), FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemanas = entitys;
            return PartialView(model);
        }

        /// <summary>
        /// Lista los formatos de acuerdo a la lectura seleccionada
        /// </summary>
        /// <param name="idLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarFormatosXLectura(int idLectura, int idEmpresa, int idModulo)
        {
            HidrologiaModel model = new HidrologiaModel();
            //model.ListaTipoInformacion = this.logic.ListMeFormatos().Where(x => x.Modcodi == Constantes.IdModulo ).ToList();
            model.ListaFormato = logic.GetByModuloLecturaMeFormatos(idModulo, idLectura, idEmpresa);
            return PartialView(model);
        }

        /// <summary>
        /// Devuelve listado de formatos dependiendo de la lectura seleccionada y es regresado por ayax al cliente
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargaFormatosXLectura(int idLectura, int idEmpresa)
        {
            List<MeFormatoDTO> entitys = logic.GetByModuloLecturaMeFormatos(ConstantesHidrologia.IdModulo, idLectura, idEmpresa)
                                            .OrderBy(x => x.Formatnombre).ToList();

            SelectList list = new SelectList(entitys, EntidadPropiedadHidro.Formatcodi, EntidadPropiedadHidro.Formatnombre);

            return Json(list);
        }

        /// <summary>
        /// Devuelve listado de lecturas dependiendo del area seleccionada y es regresado por ayax al cliente
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargaLecturaXArea(int idArea)
        {
            List<MeLecturaDTO> entitys = logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesHidrologia.IdOrigenHidro && x.Areacode == idArea)
                                            .OrderBy(x => x.Lectnomb).ToList();
            List<int> listaLecturaOmitir = new List<int>() { 243, 242, 239 }; // COMPROMISO HIDRÁULICO - NO, COMPROMISO HIDRÁULICO - SI, EJECUTADO HISTORICO RDO
            entitys = entitys.Where(x => !listaLecturaOmitir.Contains(x.Lectcodi)).ToList();

            SelectList list = new SelectList(entitys, EntidadPropiedadHidro.Lectcodi, EntidadPropiedadHidro.Lectnomb);
            return Json(list);
        }

        /// <summary>
        /// Indica la fecha por defecto de cada formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult SetearFechasEnvio(int idFormato)
        {
            var formato = logic.GetByIdMeFormato(idFormato);
            string mes = string.Empty;
            string fecha = string.Empty;
            int semana = 0;
            int anho = 0;
            int tipo = formato.Lecttipo;
            if (formato.Formatdiaplazo == 0)
                tipo = 0;
            if (formato != null)
                HidrologiaHelper.GetFechaActualEnvio((int)formato.Formatperiodo, tipo, ref mes, ref fecha, ref semana, ref anho);
            var jason = new
            {
                mes = mes,
                fecha = fecha,
                semana = semana,
                anho = anho
            };
            return Json(jason);
        }

    }
}
