using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Areas.RDO.Helper;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.RDO;

using COES.Servicios.Aplicacion.RDO.Helper;

namespace COES.MVC.Extranet.Areas.RDO.Controllers
{
    public class CaudalVolumenController : BaseController
    {
        #region Declaración de variables

        RDOAppServicio logicRdo = new RDOAppServicio();

        GeneralAppServicio logicGeneral = new GeneralAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CaudalVolumenController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;          
        int IdAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);
        

        #endregion

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
        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[DatosSesion.SesionFileName] != null) ?
                    Session[DatosSesion.SesionFileName].ToString() : null;
            }
            set { Session[DatosSesion.SesionFileName] = value; }
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
        public MeFormatoDTO Formato
        {
            get
            {
                return (Session[DatosSesionCaudalVolumen.SesionFormato] != null) ?
                    (MeFormatoDTO)Session[DatosSesionCaudalVolumen.SesionFormato] : new MeFormatoDTO();
            }
            set { Session[DatosSesionCaudalVolumen.SesionFormato] = value; }
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
        // GET: RDO/CaudalVolumen
        /// <summary>
        /// Index para el envio de archivo
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexExcelWeb()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            //if (this.IdModulo == null) return base.RedirectToHomeDefault();

            FormatoModel model = new FormatoModel();
            string codigo = string.Empty;

            model.IdArea = 0;
            CargarFiltrosFormato(model);
            DateTime fechaActual = DateTime.Now;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Editable = 1;
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.IdLectura = ConstantesCaudalVolumen.IdLectura;
            return View(model);
        }
        /// <summary>
        /// Actualiza la informacion necesaria para cargar los filtros para la eleccion de los formatos
        /// </summary>
        /// <param name="model"></param>
        private void CargarFiltrosFormato(FormatoModel model)
        {

            int idOrigen = ConstantesCaudalVolumen.IdOrigen;
            int idOpcion = (Session[DatosSesion.SesionIdOpcion] == null) ? 0 : (int)Session[DatosSesion.SesionIdOpcion];
            //Cargar Permisos
            this.ListaPermisos = new bool[14];

            this.ListaPermisos[Acciones.Grabar] = true;
            this.ListaPermisos[Acciones.AccesoEmpresa] = true;// seguridad.ValidarPermisoOpcion(this.IdAplicacion, idOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
            this.ListaPermisos[Acciones.Editar] = true;// seguridad.ValidarPermisoOpcion(this.IdAplicacion, idOpcion, Acciones.Editar, User.Identity.Name);

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
            model.ListaAreas = logicRdo.ListAreaXFormato(idOrigen);
            if ((model.IdArea == 0) && (model.ListaAreas.Count > 0))
                model.IdArea = model.ListaAreas[0].Areacode;
            model.ListaFormato = logicRdo.ListMeFormatos().Where(x => x.Modcodi == ConstantesCaudalVolumen.IdModulo).ToList(); //lista de todos los formatos para hidrologia
                                                                                                                            //if (model.ListaFormato.Count > 0)
                                                                                                                            //    model.IdFormato = model.ListaFormato[0].Formatcodi;
                                                                                                                            //model.ListaLectura = logic.ListMeLecturas().Where(x => x.Origlectcodi == idOrigen && x.Areacode == model.IdArea).ToList();
                                                                                                                            //if (model.ListaLectura.Count > 0 && (model.IdLectura == 0))

            model.IdLectura = ConstantesCaudalVolumen.IdLectura;
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
            model.ListaFormato = logicRdo.GetByModuloLecturaMeFormatos(ConstantesCaudalVolumen.IdModulo, model.IdLectura, model.IdEmpresa);
        }
        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult GenerarFormato(int idEmpresa, string desEmpresa, int idFormato, string fecha, string semana, string mes, string horario)
        {
            int indicador = 0;
            int idEnvio = 0;

            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCaudalVolumen.FolderReporte;
            try
            {
                FormatoModel model = BuildHojaExcel(idEmpresa, idFormato, idEnvio, fecha, semana, mes, ConstantesRDO.NoVerUltimoEnvio, horario);
                CaudalVolumenHelper.GenerarFileExcel(model, ruta, horario);
                indicador = 1;

            }
            catch (Exception ex)
            {
                Log.Error(ConstantesRDO.LogError, ex);
                indicador = -1;
            }
            return Json(indicador);

        }
        /// <summary>
        ///Devuelve los modelos de caudales y volúmenes con todos sus campos llenos para mostrar el excelweb en la interfaz web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="verUltimoEnvio"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcel(int idEmpresa, int idFormato, int idEnvio, string fecha, string semana, string mes, int verUltimoEnvio, string horario)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();

            ////////// Obtiene el Fotmato ////////////
            model.Formato = logicRdo.GetByIdMeFormato(idFormato);
            this.Formato = model.Formato;
            var cabercera = logicRdo.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();

            /// Definición del formato //////
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;

            var entEmpresa = logicRdo.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
            {
                model.Empresa = entEmpresa.Emprnomb;
                model.EsEmpresaVigente = logicRdo.EsEmpresaVigente(idEmpresa, DateTime.Now);
            }

            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, semana, fecha, ConstantesRDO.FormatoFecha);

            //Mostrar último envio cuando se muestra la interfaz de Carga de datos de un formato
            int idUltEnvio = 0;
            model.ListaEnvios = logicRdo.GetByCriteriaMeEnvios(idEmpresa, idFormato, model.Formato.FechaProceso);
            if (model.ListaEnvios.Count > 0)
            {
                model.IdEnvioLast = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                idUltEnvio = model.IdEnvioLast;
                if (ConstantesRDO.VerUltimoEnvio == verUltimoEnvio)
                {
                    idEnvio = model.IdEnvioLast;
                }
            }

            int idCfgFormato = 0;
            model.Formato.IdEnvio = idEnvio;
            /// Verifica si Formato esta en Plaz0
            string mensaje = string.Empty;
            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real
            if (idEnvio <= 0)
            {
                model.Formato.Emprcodi = idEmpresa;
                RDOAppServicio.GetSizeFormato(model.Formato);
                model.EnPlazo = logicRdo.ValidarPlazo(model.Formato);
                model.TipoPlazo = logicRdo.EnvioValidarPlazo(model.Formato, idEmpresa);
                model.Handson.ReadOnly = ConstantesEnvioRdo.ENVIO_PLAZO_DESHABILITADO == model.TipoPlazo;

                //ObtenerH24IniFinTR(model.Formato, !model.Handson.ReadOnly, out horaini, out horafin);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = true;

                var envioAnt = logicRdo.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaEnvio = envioAnt.Enviofecha;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                    model.EnPlazo = envioAnt.Envioplazo == "P";
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                RDOAppServicio.GetSizeFormato(model.Formato);
            }

            //
            model.ListaHojaPto = logicRdo.GetListaPtos(model.Formato.FechaFin, idCfgFormato, idEmpresa, idFormato, cabercera.Cabquery);
            logicRdo.ListarConfigPlazoXFormatoYFechaPeriodo(idFormato, model.ListaHojaPto, model.Formato.FechaProceso);

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
            model.ViewHtml = CaudalVolumenHelper.GenerarFormatoHtml(model, idEnvio, model.EnPlazo);

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
            int nCol = model.ListaHojaPto.Count;
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;
            model.Handson.ListaFilaReadOnly = CaudalVolumenHelper.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques);
            if (model.Formato.Formatresolucion == ParametrosFormato.ResolucionHora && model.Formato.Formatperiodo == ParametrosFormato.PeriodoDiario && model.Formato.Formatdiaplazo == 0)
                model.Handson.ListaFilaReadOnly = CargarListaFilaReadOnly(model.Formato.FechaProceso, idEmpresa, idFormato, model.Formato.Formatrows, nBloques, model.EnPlazo);


            //model.Handson.ListaFilaReadOnly = CargarListaFilaReadOnly(model.Formato.FechaProceso, idEmpresa, idFormato, model.Formato.Formatrows, nBloques, model.EnPlazo);

            model.ListaCambios = new List<CeldaCambios>();
            model.Handson.ListaExcelData = CaudalVolumenHelper.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);
            if (idEnvio == 0)
            {
                lista = logicRdo.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio, horario);
                CaudalVolumenHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }
            if (idEnvio > 0)
            {
                lista = logicRdo.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio, horario);
                listaCambios = logicRdo.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                CaudalVolumenHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }
            if (idEnvio < 0)
            {
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                CaudalVolumenHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
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
        /// Verifica si el formato puede ser editado.
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        protected void ObtenerH24IniFinTR(MeFormatoDTO formato, bool enPlazo, out int horaini, out int horafin)
        {
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;

            if ((formato.Formatdiaplazo == 0) && (enPlazo)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;

                if (hora < 2)
                {
                    horaini = 0;
                }
                else
                {
                    horaini = hora - 2;
                }

                horafin = 24;
            }
        }

        /// <summary>
        /// Carga lista de Filas que indican si esta bloqueadas o no, valido para formatos en tiempo real.
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formatcodi"></param>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <param name="plazo"></param>
        /// <returns></returns>
        protected List<bool> CargarListaFilaReadOnly(DateTime fechaProceso, int idEmpresa, int formatcodi, int filHead, int filData, bool plazo)
        {
            List<bool> lista = new List<bool>();
            int horaini = 0;
            int horafin = -1;

            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fechaActual2 = DateTime.Now;
            int dif = (fechaActual - fechaProceso).Days;
            int hora = fechaActual2.Hour;
            if (dif == 0)
            {
                //int periodo = hora / ConstantesCaudalVolumen.BandaTR;
                //horaini = periodo * ConstantesCaudalVolumen.BandaTR;
                //horafin = horaini + ConstantesCaudalVolumen.BandaTR - 1;
                horaini = 0;
                horafin = 24;
            }
            if (dif == 1)
            {
                if (hora < ConstantesCaudalVolumen.BandaTR - 1)
                {
                    horafin = ConstantesCaudalVolumen.BandaTR - 1;
                }
            }

            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }


            for (int i = 0; i < filData; i++)
            {
                if (i >= horaini && i <= horafin)
                    lista.Add(false);
                else
                    lista.Add(true);

            }
            return lista;
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCaudalVolumen.FolderReporte;
            string fullPath = ruta + this.Formato.Formatnombre + ".xlsx";
            return File(fullPath, Constantes.AppExcel, this.Formato.Formatnombre + ".xlsx");
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
                    this.FileName = fileRandom + "." + NombreArchivoCaudalVolumen.ExtensionFileUpload;
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCaudalVolumen.FolderUpload;
                    string fileName = ruta + this.FileName;
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
        public JsonResult LeerFileUpExcel(int idEmpresa, string fecha, string semana, string mes, int idFormato, string horario)
        {
            //Leer Lista de Puntos
            // Leer  Formato
            //Definir Matriz de Puntos 
            //Recorrer Excel y llenar Matriz de Puntos
            //Verificar Formato
            Boolean isValido = true;
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();

            int retorno = CaudalVolumenHelper.VerificarIdsFormato(this.NombreFile, idEmpresa, idFormato);

            if (retorno > 0)
            {
                MeFormatoDTO formato = logicRdo.GetByIdMeFormato(idFormato);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                var cabecera = logicRdo.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                var cabecerasRow = cabecera.Cabcampodef.Split(QueryParametros.SeparadorFila);
                formato.Formatcols = cabecera.Cabcolumnas;
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatheaderrow = cabecera.Cabcampodef;
                RDOAppServicio.GetSizeFormato(formato);
                var listaPtos = logicRdo.GetByCriteria2MeHojaptomeds(idEmpresa, idFormato, cabecera.Cabquery, formato.FechaInicio, formato.FechaFin);
                listaPtos = listaPtos.Where(x => x.Hojaptoactivo == 1).ToList();
                int nCol = listaPtos.Count;
                int horizonte = formato.Formathorizonte;
                int nBloques = formato.RowPorDia * formato.Formathorizonte;


                if (formato.Formatresolucion == ParametrosFormato.ResolucionHora && formato.Formatperiodo == ParametrosFormato.PeriodoDiario && formato.Formatdiaplazo == 0)
                {
                    lista24 = logicRdo.GetDataAnt(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);


                    //   lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                    List<bool> listaRead = CargarListaFilaReadOnly(formato.FechaProceso, idEmpresa, idFormato, formato.Formatrows, nBloques, true);
                    this.MatrizExcel = CaudalVolumenHelper.InicializaMatrizExcel(formato.Formatrows, nBloques, formato.Formatcols, nCol);
                    isValido = CaudalVolumenHelper.LeerExcelFile2(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, nCol, listaRead, lista24, listaPtos);
                }
                else
                {
                    this.MatrizExcel = CaudalVolumenHelper.InicializaMatrizExcel(formato.Formatrows, nBloques, formato.Formatcols, nCol);
                    isValido = CaudalVolumenHelper.LeerExcelFile(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, nCol, horario);
                }
            }
            //Borrar Archivo
            CaudalVolumenHelper.BorrarArchivo(this.NombreFile);
            if (!isValido)
                retorno = 0;

            return Json(retorno);
        }

        /// <summary>
        /// Lista los formatos de acuerdo a la lectura seleccionada
        /// </summary>
        /// <param name="idLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarFormatosXLectura(int idLectura, int idEmpresa, int idModulo)
        {
            CaudalVolumenModel model = new CaudalVolumenModel();
            //model.ListaTipoInformacion = this.logic.ListMeFormatos().Where(x => x.Modcodi == Constantes.IdModulo ).ToList();
            model.ListaFormato = logicRdo.GetByModuloLecturaMeFormatos(idModulo, idLectura, idEmpresa);
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
            List<MeFormatoDTO> entitys = new List<MeFormatoDTO>();
            entitys = logicRdo.GetByModuloLecturaMeFormatos(ConstantesCaudalVolumen.IdModulo, idLectura, idEmpresa);

            SelectList list = new SelectList(entitys, EntidadPropiedadCaudalVolumen.Formatcodi, EntidadPropiedadCaudalVolumen.Formatnombre);

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
            List<MeLecturaDTO> entitys = new List<MeLecturaDTO>();
            entitys = logicRdo.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesCaudalVolumen.IdOrigen && x.Areacode == idArea).ToList();
            SelectList list = new SelectList(entitys, EntidadPropiedadCaudalVolumen.Lectcodi, EntidadPropiedadCaudalVolumen.Lectnomb);
            return Json(list);
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
        public JsonResult GrabarExcelWeb(string dataExcel, int idFormato, int idEmpresa, string fecha, string semana, string mes, int horario)
        {
            base.ValidarSesionUsuario();
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            int exito = 0;
            List<string> celdas = new List<string>();
            celdas = dataExcel.Split(',').ToList();
            string empresa = string.Empty;
            var regEmp = logicRdo.GetByIdSiEmpresa(idEmpresa); ;
            //////////////////////////////////////////////////
            if (regEmp != null)
            {
                empresa = regEmp.Emprnomb;
                if (!this.logicRdo.EsEmpresaVigente(idEmpresa, DateTime.Now))
                {
                    return Json(model);
                }
            }

            MeFormatoDTO formato = logicRdo.GetByIdMeFormato(idFormato);
            var cabercera = logicRdo.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            formato.Formatcols = cabercera.Cabcolumnas;
            formato.Formatrows = cabercera.Cabfilas;
            formato.Formatheaderrow = cabercera.Cabcampodef;
            int filaHead = formato.Formatrows;
            int colHead = formato.Formatcols;

            /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
            RDOAppServicio.GetSizeFormato(formato);

            int idCfgFormato = 0;
            var listaPto = logicRdo.GetListaPtos(formato.FechaFin, idCfgFormato, idEmpresa, idFormato, cabercera.Cabquery);
            int nPtos = listaPto.Count();

            logicRdo.ListarConfigPlazoXFormatoYFechaPeriodo(idFormato, listaPto, formato.FechaProceso);

            formato.Emprcodi = idEmpresa;
            string tipoPlazo = logicRdo.EnvioValidarPlazo(formato, idEmpresa);
            if (ConstantesEnvioRdo.ENVIO_PLAZO_DESHABILITADO == tipoPlazo)
                throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + formato.FechaPlazoIni.ToString(ConstantesRDO.FormatoFechaFull) + " y " + formato.FechaPlazoFuera.ToString(ConstantesRDO.FormatoFechaFull));

            /////////////// Grabar Config Formato Envio //////////////////
            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = idFormato;
            config.Emprcodi = idEmpresa;
            config.FechaInicio = formato.FechaInicio;
            config.FechaFin = formato.FechaFin;
            int idConfig = logicRdo.GrabarConfigFormatEnvio(config);
            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            Boolean enPlazo = true;
            int _horEnPlazo = horario + 1;
            int _horActual = DateTime.Now.Hour;
            if (_horActual > _horEnPlazo)
                enPlazo = false;
            else if (_horActual == _horEnPlazo && DateTime.Now.Minute > 0)
                enPlazo = false;
            else
                enPlazo = true;
            //Boolean enPlazo = logicRdo.ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

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
            this.IdEnvio = logicRdo.SaveMeEnvio(envio);
            model.IdEnvio = this.IdEnvio;
            ///////////////////////////////////////////////////////
            int horizonte = formato.Formathorizonte;
            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    int total = (nPtos + formato.Formatcols) * (filaHead + 96 * formato.Formathorizonte);
                    int totalRecibido = celdas.Count;

                    var lista96 = CaudalVolumenHelper.LeerExcelWeb96(celdas, listaPto, formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 4 * formato.Formathorizonte, formato.Formatcheckblanco);
                    if (lista96.Count > 0)
                    {
                        try
                        {
                            logicRdo.GrabarValoresCargados96(lista96, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            logicRdo.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Mensaje = MensajesCaudalVolumen.MensajeEnvioExito;
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
                        var lista48 = CaudalVolumenHelper.LeerExcelWeb48(celdas, listaPto, formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 2 * formato.Formathorizonte, formato.Formatcheckblanco);
                        logicRdo.GrabarValoresCargados48(lista48, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = this.IdEnvio;
                        logicRdo.UpdateMeEnvio(envio);
                        exito = 1;
                        model.Resultado = 1;
                        model.Mensaje = MensajesCaudalVolumen.MensajeEnvioExito;
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
                        var lista24 = CaudalVolumenHelper.LeerExcelWeb24(celdas, listaPto, formato.Lectcodi, colHead, nPtos, filaHead, 24 * formato.Formathorizonte);
                        var listaejecutados24 = CaudalVolumenHelper.LeerExcelWeb24Ejecutados(celdas, listaPto, formato.Lectcodi, colHead, nPtos, filaHead, 24 * formato.Formathorizonte);
                        logicRdo.GrabarValoresCargados24(lista24, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                        logicRdo.GrabarValoresEjecutados24(listaejecutados24, User.Identity.Name, this.IdEnvio, idEmpresa, formato);

                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = this.IdEnvio;
                        logicRdo.UpdateMeEnvio(envio);
                        exito = 1;
                        model.Resultado = 1;
                        model.Mensaje = MensajesCaudalVolumen.MensajeEnvioExito;
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
                        var lista1 = CaudalVolumenHelper.LeerExcelWeb1(celdas, listaPto, formato.Lectcodi, (int)formato.Formatperiodo, colHead, nPtos, filaHead, formato.Formathorizonte);
                        logicRdo.GrabarValoresCargados1(lista1, User.Identity.Name, this.IdEnvio, idEmpresa, formato, formato.Lectcodi);
                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = this.IdEnvio;
                        logicRdo.UpdateMeEnvio(envio);
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

            logicRdo.SaveHorarioMeEnvio(IdEnvio, horario);

            return Json(model);
        }

        /// <summary>
        /// Obtiene Matriz de datos del formato web
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
                    entity.Tipoinfocodi = ConstantesCaudalVolumen.Caudal;
                    entity.Lectcodi = ConstantesCaudalVolumen.EjectutadoTR;
                    entity.Medinth1 = decimal.Parse(datos[i][4]);
                    entity.Emprcodi = Int32.Parse(datos[i][5]);
                    entity.Medintdescrip = datos[i][6];
                    lista.Add(entity);
                }
            }

            return lista;
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
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, int idFormato, int idEnvio, string fecha, string semana, string mes, int verUltimoEnvio, string horario)
        {
            base.ValidarSesionUsuario();
            FormatoModel jsModel = new FormatoModel();
            jsModel = BuildHojaExcel(idEmpresa, idFormato, idEnvio, fecha, semana, mes, verUltimoEnvio, horario);
            return Json(jsModel);
        }

        /// <summary>
        /// Indica la fecha por defecto de cada formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult SetearFechasEnvio(int idFormato)
        {
            var formato = logicRdo.GetByIdMeFormato(idFormato);
            string mes = string.Empty;
            string fecha = string.Empty;
            int semana = 0;
            int anho = 0;
            int tipo = formato.Lecttipo;
            if (formato.Formatdiaplazo == 0)
                tipo = 0;
            if (formato != null)
                CaudalVolumenHelper.GetFechaActualEnvio((int)formato.Formatperiodo, tipo, ref mes, ref fecha, ref semana, ref anho);
            var jason = new
            {
                mes = mes,
                fecha = fecha,
                semana = semana,
                anho = anho
            };
            return Json(jason);
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
    }
}