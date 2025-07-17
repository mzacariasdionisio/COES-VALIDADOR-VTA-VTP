using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Interconexiones;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Controllers
{
    public abstract class FormatoController : BaseController
    {
        public FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        public CorreoAppServicio servCorreo = new CorreoAppServicio();
        public ParametroAppServicio servParametro = new ParametroAppServicio();
        protected IEODAppServicio servIEOD = new IEODAppServicio();
        public HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();
        RestriccionesOperativasAppServicio servicioRestric = new RestriccionesOperativasAppServicio();
        GeneralAppServicio servGeneral = new GeneralAppServicio();
        InterconexionesAppServicio servInterconexion = new InterconexionesAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FormatoController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public FormatoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesFormato.SesionNombreArchivo] != null) ?
                    Session[ConstantesFormato.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesFormato.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[ConstantesFormato.SesionMatrizExcel] != null) ?
                    (string[][])Session[ConstantesFormato.SesionMatrizExcel] : new string[1][];
            }
            set { Session[ConstantesFormato.SesionMatrizExcel] = value; }
        }

        /// <summary>
        /// Lista de Matriz de datos
        /// </summary>
        public List<string[][]> ListaMatrizExcel
        {
            get
            {
                return (Session[ConstantesFormato.SesionListaMatrizExcel] != null) ?
                    (List<string[][]>)Session[ConstantesFormato.SesionListaMatrizExcel] : new List<string[][]>();
            }
            set { Session[ConstantesFormato.SesionListaMatrizExcel] = value; }
        }

        /// <summary>
        /// Lista hoja
        /// </summary>
        public List<int> ListaHoja
        {
            get
            {
                return (Session[ConstantesFormato.SesionListaHoja] != null) ?
                    (List<int>)Session[ConstantesFormato.SesionListaHoja] : new List<int>();
            }
            set { Session[ConstantesFormato.SesionListaHoja] = value; }
        }

        #endregion

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + "." + ConstantesFormato.ExtensionFile;
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
        /// Action para carga del archivo CSV
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadCsv()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                string fileName = path + "archivo_carga_rpf" + "." + ConstantesFormato.ExtensionFile;

                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                Session[DatosSesion.SesionFileName] = fileName;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    this.NombreFile = file.FileName;

                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
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
        public JsonResult LeerFileUpExcel(int idEmpresa, string fecha, string semana, string mes, int idFormato, Boolean tieneHojaView = false, int? idHoja = 0)
        {
            FormatoResultado modelResultado = new FormatoResultado();
            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                int retorno = ToolsFormato.VerificarIdsFormato(this.NombreFile, idEmpresa, idFormato);

                if (retorno > 0 && idFormato > 0)
                {
                    MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);
                    DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
                    formato.FechaProceso = fechaProceso;
                    var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                    var listaPtos = servFormato.GetByCriteria2MeHojaptomeds(idEmpresa, idFormato, cabecera.Cabquery, fechaProceso, fechaProceso);
                    FormatoMedicionAppServicio.GetSizeFormato(formato);

                    formato.Formatcols = cabecera.Cabcolumnas;
                    formato.Formatrows = cabecera.Cabfilas;
                    formato.Formatheaderrow = cabecera.Cabcampodef;

                    #region Formatos Caso especial

                    //if (idFormato == ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario || idFormato == ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal)
                    //    formato.TipoAgregarTiempoAdicional = ParametrosFormato.TipoAgregarTiempoAdicionalSub1;

                    #endregion

                    fecha = formato.FechaInicio.ToString(Constantes.FormatoFecha);

                    if (!tieneHojaView)
                    {
                        this.MatrizExcel = ToolsFormato.LeerExcelFile(this.NombreFile, listaPtos, formato, null);
                    }
                    else
                    {
                        FormatoModel modelPrincipal = new FormatoModel();
                        modelPrincipal.IdFormato = idFormato;
                        modelPrincipal.Formato = formato;
                        modelPrincipal.ListaHojaPto = listaPtos;
                        modelPrincipal.OpGrabar = true;
                        modelPrincipal.UtilizaHoja = true;
                        modelPrincipal.Formato.FlagUtilizaHoja = modelPrincipal.UtilizaHoja;
                        modelPrincipal.Formato.ListaHoja = servFormato.GetByCriteriaMeHoja(idFormato);
                        modelPrincipal.IdHojaPadre = idHoja.Value;

                        GenerarHojaFormato(modelPrincipal);

                        this.ListaHoja = new List<int>();
                        this.ListaMatrizExcel = new List<string[][]>();

                        foreach (var model in modelPrincipal.ListaFormatoModel)
                        {
                            this.ListaHoja.Add(model.IdHoja);
                            var matrizExcel = ToolsFormato.LeerExcelFile(this.NombreFile, model.ListaHojaPto, model.Formato, model.Hoja.Hojanombre);
                            this.ListaMatrizExcel.Add(matrizExcel);
                        }
                    }
                }
                else
                {
                    throw new Exception("El archivo importado no coincide con el formato.");
                }

                ToolsFormato.BorrarArchivo(this.NombreFile);

                modelResultado.Resultado = retorno;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResultado.Mensaje = ex.Message;
                modelResultado.Detalle = ex.StackTrace;
            }

            return Json(modelResultado);
        }

        /// <summary>
        /// carga Model para la vista index de los formatos
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idFormato"></param>
        public void IndexFormato(FormatoModel model, int idFormato)
        {
            model.ListaEmpresas = this.ListarEmpresaByFormatoYUsuario(Acciones.AccesoEmpresa, User.Identity.Name, idFormato);
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.IdFormato = idFormato;

            this.GetFechaActualEnvio(model);

            return;
        }

        /// <summary>
        /// Listar empresas por Usuario y Formato
        /// </summary>
        /// <param name="acceso"></param>
        /// <param name="usuario"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresaByFormatoYUsuario(int acceso, string usuario, int idFormato)
        {
            List<SiEmpresaDTO> listaEmpresas = new List<SiEmpresaDTO>();
            bool accesoEmpresa = base.VerificarAccesoAccion(acceso, usuario);
            List<SiEmpresaDTO> empresas = ConstantesHard.IdFormatoEnergiaPrimaria != idFormato ? servFormato.GetListaEmpresaFormato(idFormato)
                : this.servFormato.GetListaEmpresaFormatoEnergiaPrimaria(ConstantesHard.IdFormatoEnergiaPrimaria);
            if (accesoEmpresa)
            {
                if (empresas.Count > 0)
                    listaEmpresas = empresas;
                else
                {
                    listaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }
            else
            {
                var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                    Select(x => new SiEmpresaDTO()
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB,
                        Tipoemprcodi = x.TIPOEMPRCODI
                    });
                if (emprUsuario.Count() > 0)
                {
                    listaEmpresas = emprUsuario.ToList();

                }
                else
                {
                    listaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }

            return listaEmpresas.OrderBy(x => x.Emprnomb).ToList();
        }

        /// <summary>
        /// Genera las filas de la cabecera del formato excel despacho diario
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listaCabeceraRow"></param>
        public void GenerarCabecera(FormatoModel model, List<CabeceraRow> listaCabeceraRow)
        {
            //for (var ind = 0; ind < model.ColumnasCabecera; ind++)
            //{
            //    model.Handson.ListaColWidth.Add(120);
            //}
            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();

            if (model.Handson.ListaExcelData == null)
            {
                model.Handson.ListaExcelData = new string[model.FilasCabecera][];
            }

            foreach (var reg in model.ListaHojaPto)
            {
                //                model.Handson.ListaColWidth.Add(100);
                for (var w = 0; w < model.FilasCabecera; w++)
                {
                    if (column == model.ColumnasCabecera)
                    {
                        model.Handson.ListaExcelData[w] = new string[model.ListaHojaPto.Count + model.ColumnasCabecera];
                        model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                    }
                    var etiqueta = reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null);
                    if (etiqueta != null)
                    {
                        model.Handson.ListaExcelData[w][column] = etiqueta.ToString();
                    }
                    else
                    {
                        model.Handson.ListaExcelData[w][column] = string.Empty;
                    }

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

        }

        /// <summary>
        /// Inicializar Hoja
        /// </summary>
        /// <param name="modelPrincipal"></param>
        private void GenerarHojaFormato(FormatoModel modelPrincipal)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            modelPrincipal.ListaFormatoModel = new List<FormatoModel>();
            modelPrincipal.ListaMeHoja = modelPrincipal.Formato.ListaHoja;
            if (modelPrincipal.IdHojaPadre > 0)
            {
                modelPrincipal.ListaMeHoja = modelPrincipal.ListaMeHoja.Where(x => x.Hojapadre == modelPrincipal.IdHojaPadre).ToList();
            }

            var listaHoja = modelPrincipal.ListaMeHoja.Select(x => x.Hojacodi).ToList();

            foreach (var hoja in listaHoja)
            {
                string strFormato = serializer.Serialize(modelPrincipal.Formato);
                MeFormatoDTO formato = serializer.Deserialize<MeFormatoDTO>(strFormato);

                FormatoModel model = new FormatoModel();
                //datos filtros
                model.IdEmpresa = modelPrincipal.IdEmpresa;
                model.Empresa = modelPrincipal.Empresa;
                model.Anho = modelPrincipal.Anho;
                model.Mes = modelPrincipal.Mes;
                model.Dia = modelPrincipal.Formato.FechaProceso.Day.ToString();
                model.Semana = modelPrincipal.Semana;
                //hoja
                model.IdHoja = hoja;
                model.Hoja = this.servFormato.GetByIdMeHoja(model.IdHoja);
                //formato
                model.UtilizaHoja = modelPrincipal.UtilizaHoja;
                model.Formato = formato;
                if (ConstantesIEOD.IdFormatoDemandaDiaria == model.Formato.Formatcodi)
                {
                    if (ConstantesIEOD.LectCodiDemandaDiariaProgramado == model.Hoja.Lectcodi)
                    {
                        model.Formato.Lecttipo = ParametrosFormato.Programado;
                        model.Formato.FechaProceso = modelPrincipal.Formato.FechaProceso;
                        FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                        model.Formato.FechaProceso = modelPrincipal.Formato.FechaProceso.Date.AddDays(1);
                    }
                    else
                    {
                        model.Formato.Lecttipo = ParametrosFormato.Ejecutado;
                        model.Formato.FechaProceso = modelPrincipal.Formato.FechaProceso.Date.AddDays(-1);
                        FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                    }
                }
                else
                {
                    model.Formato.FechaProceso = modelPrincipal.Formato.FechaProceso;
                    FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                }
                model.Fecha = model.Formato.FechaProceso.ToString(ConstantesAppServicio.FormatoFecha);
                model.ColumnasCabecera = modelPrincipal.ColumnasCabecera;
                model.FilasCabecera = modelPrincipal.FilasCabecera;
                model.ValidacionFormatoCheckblanco = modelPrincipal.Formato.Formatcheckblanco == 1;

                var hojaPadre = this.servFormato.GetByIdMeHoja(model.Hoja.Hojapadre);
                if (hojaPadre != null)
                {
                    model.Formato.Formatnombre = model.Formato.Formatnombre + " (" + hojaPadre.Hojanombre + ")";
                }

                //puntos
                var listaPto = modelPrincipal.ListaHojaPto.Where(x => x.Hojacodi == hoja).ToList();
                model.ListaHojaPto = listaPto;

                //handson
                model.Handson = new HandsonModel();
                model.Handson.ListaMerge = new List<CeldaMerge>();
                model.Handson.ListaColWidth = new List<int>();
                model.Handson.ListaColWidth.Add(150);

                if (!modelPrincipal.OpGrabar)
                {
                    model.Handson.ReadOnly = modelPrincipal.Handson.ReadOnly;
                    for (var i = 0; i < model.ListaHojaPto.Count; i++)
                    {
                        model.Handson.ListaColWidth.Add(100);
                    }

                    //matriz estado
                    int estado = !model.Handson.ReadOnly ? 0 : -1;
                    var matrizTipoEstado = ToolsFormato.IncializaMatrizEstado(model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia, (short)estado, model.Formato.Formathorizonte);
                    model.Handson.MatrizTipoEstado = matrizTipoEstado;
                }

                //agregar
                modelPrincipal.ListaFormatoModel.Add(model);
            }
        }

        /// <summary>
        /// Generar handson para hoja
        /// </summary>
        /// <param name="modelPrincipal"></param>
        /// <param name="lista96"></param>
        /// <param name="listaCambios"></param>
        /// <param name="fecha"></param>
        /// <param name="idEnvio"></param>
        private void GenerarHojaFormatoHandson48(FormatoModel modelPrincipal, List<MeMedicion48DTO> lista48, List<MeCambioenvioDTO> listaCambios, string fecha, int idEnvio)
        {
            foreach (var model in modelPrincipal.ListaFormatoModel)
            {
                model.Handson.ListaExcelData = ToolsFormato.ObtenerListaExcelDataM48(model, lista48, listaCambios, model.Fecha, idEnvio);
            }
        }

        /// <summary>
        /// Generar handson para hoja
        /// </summary>
        /// <param name="modelPrincipal"></param>
        /// <param name="lista96"></param>
        /// <param name="listaCambios"></param>
        /// <param name="fecha"></param>
        /// <param name="idEnvio"></param>
        private void GenerarHojaFormatoHandson96(FormatoModel modelPrincipal, List<MeMedicion96DTO> lista96, List<MeCambioenvioDTO> listaCambios, string fecha, int idEnvio)
        {
            foreach (var model in modelPrincipal.ListaFormatoModel)
            {
                model.Handson.ListaExcelData = ToolsFormato.ObtenerListaExcelDataM96(model, lista96, listaCambios, fecha, idEnvio);
            }
        }

        /// <summary>
        /// Cabecera para hoja
        /// </summary>
        /// <param name="modelPrincipal"></param>
        /// <param name="listaCabeceraRow"></param>
        private void GenerarCabeceraHojaFormato(FormatoModel modelPrincipal, List<CabeceraRow> listaCabeceraRow)
        {
            foreach (var model in modelPrincipal.ListaFormatoModel)
            {
                GenerarCabecera(model, listaCabeceraRow);
            }
        }

        /// <summary>
        /// Generar data de string
        /// </summary>
        /// <param name="modelPrincipal"></param>
        /// <param name="listaHoja"></param>
        /// <param name="listaMatrizExcel"></param>
        public void GenerarHojaFormatoFromData(FormatoModel modelPrincipal, List<int> listaHoja, List<string[][]> listaMatrizExcel)
        {
            for (var i = 0; i < listaHoja.Count; i++)
            {
                int idHoja = listaHoja[i];
                string[][] data = listaMatrizExcel[i];

                var model = modelPrincipal.ListaFormatoModel.Where(x => x.IdHoja == idHoja).First();
                if (!modelPrincipal.OpGrabar)
                {
                    if (!modelPrincipal.Handson.ReadOnly)
                    {
                        ConfiguraDatosMatrizExcel(model.Handson.MatrizTipoEstado, model.FilasCabecera, model.ColumnasCabecera);
                    }
                }
                model.Handson.ListaExcelData = data;
            }
        }

        /// <summary>
        /// Carga la informacion necesar en el model para construir el grid excel web.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="idFormato"></param>
        /// <param name="verUltimoEnvio"></param>
        public void BuildHojaExcel(FormatoModel model, int idEmpresa, int idEnvio, string fecha, int idFormato, int verUltimoEnvio)
        {
            try
            {

                base.ValidarSesionJsonResult();

                verUltimoEnvio = model.ValidaTiempoReal == true ? 0 : verUltimoEnvio;

                string mes = new DateTime( DateTime.Now.Year,DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

                short[][] matrizTipoEstado;
                List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>();
                List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
                model.Handson = new HandsonModel();
                model.Handson.ListaMerge = new List<CeldaMerge>();

                DateTime dfecha = fecha != null && fecha.Trim() != string.Empty ? DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

                ////////// Obtiene el Formato ////////////////////////
                model.IdEmpresa = idEmpresa;
                model.IdFormato = idFormato;
                model.Formato = servFormato.GetByIdMeFormato(idFormato);
                model.Formato.IdFormatoNuevo = model.IdFormatoNuevo;
                model.Formato.FlagUtilizaHoja = model.UtilizaHoja;
                model.Formato.ListaHoja = servFormato.GetByCriteriaMeHoja(idFormato);
                var cabecera = servFormato.GetListMeCabecera().Find(x => x.Cabcodi == model.Formato.Cabcodi);
                /// DEFINICION DEL FORMATO //////
                model.Formato.Formatcols = cabecera.Cabcolumnas;
                model.Formato.Formatrows = cabecera.Cabfilas;
                model.Formato.Formatheaderrow = cabecera.Cabcampodef;
                model.ColumnasCabecera = model.Formato.Formatcols;
                model.FilasCabecera = model.Formato.Formatrows;
                model.ListaFilasOcultas = !string.IsNullOrEmpty(cabecera.Cabfilasocultas) ? cabecera.Cabfilasocultas.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
                model.ValidacionFormatoCheckblanco = model.Formato.Formatcheckblanco == 1;

                /// Verificación de formato
                this.AsignarVerificacionFormato(model, idFormato);

                #region Formatos Caso especial

                //if (model.IdFormato == ConstantesFormatoMedicion.IdFormatoGeneracionRERDiario || model.IdFormato == ConstantesFormatoMedicion.IdFormatoGeneracionRERSemanal)
                //    model.Formato.TipoAgregarTiempoAdicional = ParametrosFormato.TipoAgregarTiempoAdicionalSub1;

                #endregion

                ///
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, string.Empty, fecha, Constantes.FormatoFecha);

                ///
                var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
                if (entEmpresa != null)
                {
                    model.Empresa = entEmpresa.Emprnomb;
                    model.EsEmpresaVigente = servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now);
                    model.Emprabrev = entEmpresa.Emprabrev;
                }

                //Mostrar último envio cuando se muestra la interfaz de Carga de datos de un formato
                model.ListaEnvios = this.servFormato.GetByCriteriaMeEnvios(idEmpresa, idFormato, model.Formato.FechaProceso);
                model.ListaEnvios = this.servFormato.GetByCriteriaMeEnviosFormatoEnergPrimaria(model.ListaEnvios, idEmpresa, idFormato, model.IdFormatoNuevo, model.Formato.FechaProceso);
                if (model.ListaEnvios.Count > 0)
                {
                    model.IdEnvioLast = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                    model.FechaEnvioLast = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviofecha.ToString();
                    if (ConstantesFormato.VerUltimoEnvio == verUltimoEnvio)
                    {
                        idEnvio = model.IdEnvioLast;
                    }
                }

                int idCfgFormato = 0;
                model.Formato.IdEnvio = idEnvio;
                if (idEnvio <= 0)
                {
                    model.Formato.Emprcodi = idEmpresa;
                    FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                    model.EnPlazo = servFormato.ValidarPlazo(model.Formato);
                    model.TipoPlazo = servFormato.EnvioValidarPlazo(model.Formato, idEmpresa);
                    model.Handson.ReadOnly = ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == model.TipoPlazo; //!ValidarFecha(model.Formato, idEmpresa);
                    model.MensajePlazo = servFormato.ObtenerMensajePlazo(model.Formato, idEmpresa);
                }
                else  // Fecha proceso es obtenida del registro envio
                {
                    model.Handson.ReadOnly = true;

                    var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
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
                    FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                    if (ConstantesIEOD.IdFormatoDemandaDiaria == model.Formato.Formatcodi)
                    {
                        listaCambios = servFormato.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio.AddDays(-1), model.Formato.FechaFin.AddDays(1), idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                    }
                    else
                    {
                        listaCambios = servFormato.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                        if (model.Formato.IdFormatoNuevo > 0 && idFormato != model.Formato.IdFormatoNuevo)
                        {
                            listaCambios = servFormato.GetAllCambioEnvio(model.Formato.IdFormatoNuevo, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                        }
                    }
                }
                //cambiar la fecha del form por la fecha ini
                fecha = ToolsFormato.getDateFormatDDMMYYYY(model.Formato.FechaInicio);

                model.Anho = model.Formato.FechaInicio.Year.ToString();
                model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);
                model.Dia = model.Formato.FechaInicio.Day.ToString();

                if (model.ListaEnvios.Count > 0)
                {
                    var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                    if (reg != null)
                        model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(Constantes.FormatoFechaHora);
                }

                /// Lista Hojapto
                model.ListaHojaPto = servFormato.GetListaPtos(model.Formato.FechaFin, idCfgFormato, idEmpresa, idFormato, cabecera.Cabquery);
                model.ListaHojaPto = servFormato.GetListaPtosFormatoEnergPrimaria(model.ListaHojaPto, idEnvio, idCfgFormato, idEmpresa, idFormato, model.IdFormatoNuevo);

                var listaInf = model.ListaHojaPto.GroupBy(x => new { x.Tipoinfocodi, x.Tipoinfoabrev })
                    .Select(grp => new SiTipoinformacionDTO { Tipoinfocodi = grp.Key.Tipoinfocodi, Tipoinfoabrev = grp.Key.Tipoinfoabrev }).ToList();
                model.ListaTipoInformacion = listaInf;

                var listaTpto = model.ListaHojaPto.GroupBy(x => new { x.Tptomedicodi, x.Tipoptomedinomb })
                    .Select(grp => new MeTipopuntomedicionDTO { Tipoptomedicodi = grp.Key.Tptomedicodi, Tipoptomedinomb = grp.Key.Tipoptomedinomb }).ToList();
                model.ListaTipopuntotomedicion = listaTpto;

                ///

                model.Handson.ListaColWidth = new List<int>();
                model.Handson.ListaColWidth.Add(150);
                for (var i = 0; i < model.ListaHojaPto.Count; i++)
                {
                    model.Handson.ListaColWidth.Add(100);
                }


                var cabecerasRow = cabecera.Cabcampodef.Split(ConstantesFormato.SeparadorFila);

                for (var x = 0; x < cabecerasRow.Length; x++)
                {
                    var reg = new CabeceraRow();
                    var fila = cabecerasRow[x].Split(ConstantesFormato.SeparadorCol);
                    reg.NombreRow = fila[0];
                    reg.TituloRow = fila[1];
                    reg.IsMerge = int.Parse(fila[2]);
                    listaCabeceraRow.Add(reg);
                }

                matrizTipoEstado = ToolsFormato.IncializaMatrizEstado(model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia, 0, model.Formato.Formathorizonte);

                /// ******** Para visualizar la data scada
                if (model.UtilizaScada)
                {
                    matrizTipoEstado = ToolsFormato.IncializaMatrizEstado(model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia, 1, model.Formato.Formathorizonte);
                }

                ////*******
                if (model.ValidaCentralSolar)
                {
                    List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoSolar);
                    model.ParamSolar = this.servParametro.GetParametroRangoCentralSolar(listaParam, model.Formato.FechaProceso, model.Formato.Formatresolucion ?? 0);
                    matrizTipoEstado = ToolsFormato.InicializaMatrizEstadoSolares(model, matrizTipoEstado);
                }

                /// ********obtener los registros de la tabla ev_horaoperacion de la empresa seleccionada 
                List<EveHoraoperacionDTO> listaHOP = new List<EveHoraoperacionDTO>();
                if (model.ValidaHorasOperacion)
                {
                    matrizTipoEstado = ToolsFormato.IncializaMatrizEstado(model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia, -1, model.Formato.Formathorizonte);
                    DateTime fechaIni = model.Formato.FechaInicio;//DateTime fechaIni = dfecha;
                    DateTime fechaFin = model.Formato.FechaFin.AddDays(1);// DateTime fechaFin = dfecha.AddDays(1);
                    listaHOP = this.servHO.ListarEquiposxFormatoXHorasOperacion(idFormato, idEmpresa, fechaIni, fechaFin);
                    ToolsFormato.CruceMatrizConHOP(matrizTipoEstado, model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia, listaHOP, fechaIni, fechaFin, model.Formato.Formatresolucion.Value); // inicializa matriz de celdas desde las horas de operacion
                }

                //*******obtener listado de Restricciones operativas

                List<EveIeodcuadroDTO> listaRestricciones = new List<EveIeodcuadroDTO>();
                if (model.ValidaRestricOperativa)
                {
                    listaRestricciones = servicioRestric.GetListarEveIeodCuadroxEmpresa(dfecha, dfecha, ConstantesFormato.SubcausacodiRestric, idEmpresa);
                    ToolsFormato.CruceMatrizConRestricOper48(matrizTipoEstado, listaRestricciones, model.ListaHojaPto, model.FilasCabecera, model.Formato.Formatresolucion.Value);
                }

                //******Obtener listado de eventos
                List<EveEventoDTO> listaEventos = new List<EveEventoDTO>();
                //listaEventos = servicioEvento.GetListarIeodCuadroxEmpresa(dfecha, dfecha, ConstantesDespacho.SubcausacodiRestric, idEmpresa);
                if (model.ValidaEventos)
                {
                    var listaEquipos = model.ListaHojaPto.Select(x => x.Equicodi).Distinct().ToList();
                    var resultado = servFormato.ListEventoEquipoFecha(ref listaEventos, listaEquipos, dfecha);
                    ToolsFormato.CruceMatrizConEvento(matrizTipoEstado, resultado, model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia);
                }
                //******Obtener listado de mantenimiento
                //List<EveManttoDTO> listaMantenimiento = new List<EveManttoDTO>();
                List<EveManttoDTO> listaMantenimientos = new List<EveManttoDTO>();
                List<EqEquirelDTO> listaTop = new List<EqEquirelDTO>();
                List<ManttoBloque> listaBloqueManto = new List<ManttoBloque>();
                if (model.ValidaMantenimiento)
                {
                    var listaEquipos = model.ListaHojaPto.Select(x => x.Equicodi).Distinct().ToList();

                    //var resultado = servFormato.ListManttoEquipoFecha2(ref listaMantenimientos, ref listaTop, listaBloqueManto, listaEquipos, dfecha);

                    var resultado = servFormato.ListManttoEquipoFechaDisponibilidad(ref listaMantenimientos, ref listaTop, listaBloqueManto, listaEquipos, dfecha);

                    ToolsFormato.CruceMatrizConMantenimiento(matrizTipoEstado, resultado, model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia);
                }

                //******Validación de Tiempo Real
                if (model.ValidaTiempoReal)
                {
                    int maxValorEstado = 0;
                    ToolsFormato.CruceMatrizTiempoReal(matrizTipoEstado, model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia, model.Formato.FechaProceso, ref maxValorEstado);
                    model.Handson.HMaximoDataScadaDisponible = maxValorEstado;
                }

                //******Obtener listado de central para medidores de generacion
                if (model.UtilizaFiltroCentral)
                {
                    var listaEq = this.servFormato.ListCentralByEmpresaAndFormato(model.ListaHojaPto);
                    model.ListaEquipo = listaEq;
                }

                model.ListaMantenimiento = listaMantenimientos;
                model.ListaEvento = listaEventos;
                model.ListaTopologia = listaTop;
                model.ListaBloqueMantos = listaBloqueManto;
                model.Handson.MatrizTipoEstado = matrizTipoEstado;

                /// Generar hojas
                if (model.UtilizaHoja)
                {
                    GenerarHojaFormato(model);
                }
                ///

                if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
                {
                    if (idEnvio == 0 && model.IdEnvioLast == 0 && !model.MostrarDataBDSinEnvioPrevio) // obtenemos datos de scada
                    {
                        if (model.UtilizaScada)
                        {
                            var listaScada = servFormato.GetListaDataFormatoScada(idFormato, idEmpresa, dfecha, dfecha);
                            model.Handson.ListaExcelData = ToolsFormato.GetListaFormatoScada(model, listaScada, fecha);
                        }
                        else
                        {
                            switch (model.Formato.Formatresolucion)
                            {
                                case ParametrosFormato.ResolucionCuartoHora:
                                    List<MeMedicion96DTO> lista96 = new List<MeMedicion96DTO>();

                                    switch (idFormato)
                                    {
                                        case ConstantesInterconexiones.IdFormatoInterconexion:
                                            lista96 = this.servInterconexion.ObtenerDataHistoricaInterconexion(1, model.Formato.FechaInicio, model.Formato.FechaFin);
                                            break;
                                    }

                                    //si el formato tiene varias hoja
                                    if (!model.UtilizaHoja)
                                    {
                                        model.Handson.ListaExcelData = ToolsFormato.ObtenerListaExcelDataM96(model, lista96, listaCambios, fecha, idEnvio);
                                    }
                                    else
                                    {
                                        this.GenerarHojaFormatoHandson96(model, lista96, listaCambios, fecha, idEnvio);
                                    }

                                    break;
                                case ParametrosFormato.ResolucionMediaHora:
                                    List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();

                                    //si el formato tiene varias hoja
                                    if (!model.UtilizaHoja)
                                    {
                                        model.Handson.ListaExcelData = ToolsFormato.ObtenerListaExcelDataM48(model, lista48, listaCambios, fecha, idEnvio);
                                    }
                                    else
                                    {
                                        this.GenerarHojaFormatoHandson48(model, lista48, listaCambios, fecha, idEnvio);
                                    }

                                    break;
                                case ParametrosFormato.ResolucionHora:
                                    break;
                                case ParametrosFormato.ResolucionDia:
                                    List<MeMedicion1DTO> lista1 = new List<MeMedicion1DTO>();
                                    model.Handson.ListaExcelData = ToolsFormato.ObtenerListaExcelDataM1(model, lista1, listaCambios, fecha, idEnvio);
                                    break;
                                case ParametrosFormato.ResolucionMes:
                                case ParametrosFormato.ResolucionSemana:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //ya existe data previamente cargada
                        switch (model.Formato.Formatresolucion)
                        {
                            case ParametrosFormato.ResolucionCuartoHora:
                                List<MeMedicion96DTO> lista96 = new List<MeMedicion96DTO>();
                                switch (idFormato)
                                {
                                    case ConstantesInterconexiones.IdFormatoInterconexion:
                                        lista96 = this.servInterconexion.ObtenerDataHistoricaInterconexion(1, model.Formato.FechaInicio, model.Formato.FechaFin);
                                        break;
                                }

                                lista96 = servFormato.GetDataFormato96(lista96, idEmpresa, model.Formato, idEnvio, model.IdEnvioLast);

                                //si el formato tiene varias hoja
                                if (!model.UtilizaHoja)
                                {
                                    model.Handson.ListaExcelData = ToolsFormato.ObtenerListaExcelDataM96(model, lista96, listaCambios, fecha, idEnvio);
                                }
                                else
                                {
                                    this.GenerarHojaFormatoHandson96(model, lista96, listaCambios, fecha, idEnvio);
                                }

                                break;
                            case ParametrosFormato.ResolucionMediaHora:
                                List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
                                lista48 = servFormato.GetDataFormato48(idEmpresa, model.Formato, idEnvio, model.IdEnvioLast);

                                List<MeScadaSp7DTO> listaScada = new List<MeScadaSp7DTO>();
                                //si usa scada
                                if (model.UtilizaScada)
                                {
                                    listaScada = servFormato.GetListaDataFormatoScada(idFormato, idEmpresa, dfecha, dfecha);
                                }

                                //si el formato tiene varias hoja
                                if (!model.UtilizaHoja)
                                {
                                    model.Handson.ListaExcelData = ToolsFormato.ObtenerListaExcelDataM48(model, lista48, listaCambios, fecha, idEnvio, listaScada);
                                }
                                else
                                {
                                    this.GenerarHojaFormatoHandson48(model, lista48, listaCambios, fecha, idEnvio);
                                }

                                break;
                            case ParametrosFormato.ResolucionHora:
                                break;
                            case ParametrosFormato.ResolucionDia:
                                List<MeMedicion1DTO> lista1 = new List<MeMedicion1DTO>();
                                lista1 = servFormato.GetDataFormato1(idEmpresa, model.Formato, idEnvio, model.IdEnvioLast);
                                model.Handson.ListaExcelData = ToolsFormato.ObtenerListaExcelDataM1(model, lista1, listaCambios, fecha, idEnvio);
                                break;
                            case ParametrosFormato.ResolucionMes:
                            case ParametrosFormato.ResolucionSemana:
                                break;
                        }
                    }
                }
                else // -1 si viene de archivo excel importado
                {
                    //TODO se comento esta linea, model.Handson.MatrizEstado como es generado?
                    //ConfiguraDatosMatrizExcel(model.Handson.MatrizEstado, model.Formato.RowPorDia + model.FilasCabecera, model.ListaHojaPto.Count + 1);                
                    if (!model.UtilizaHoja)
                    {
                        if (!model.Handson.ReadOnly)
                        {
                            ConfiguraDatosMatrizExcel(model.Handson.MatrizTipoEstado, model.FilasCabecera, model.ColumnasCabecera);
                        }
                        model.Handson.ListaExcelData = this.MatrizExcel;
                    }
                    else
                    {
                        GenerarHojaFormatoFromData(model, this.ListaHoja, this.ListaMatrizExcel);
                    }
                }

                //si el formato tiene varias hoja
                if (!model.UtilizaHoja)
                {
                    GenerarCabecera(model, listaCabeceraRow);
                }
                else
                {
                    GenerarCabeceraHojaFormato(model, listaCabeceraRow);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ConstantesAppServicio.LogError, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }
        }

        /// <summary>
        /// Graba los datos enviados a traves de la grilla excel para formatos configurados con informacion de cada media hora.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="idFormato"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public FormatoResultado GrabarExcelWeb(FormatoModel model)
        {
            ///filtros
            string[][] data = model.Handson.ListaExcelData;
            int idEmpresa = model.IdEmpresa;
            string fecha = model.Fecha;
            string semana = model.Semana;
            string mes = model.Mes;
            int idFormato = model.IdFormato;
            List<MeJustificacionDTO> listaJustificacion = model.ListaJustificacion;
            ///

            int exito = 0;
            FormatoResultado modelResultado = new FormatoResultado();
            modelResultado.Resultado = 0;
            try
            {
                if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);
                ///////// Definicion de Variables ////////////////    

                string empresa = string.Empty;
                var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa); ;
                //////////////////////////////////////////////////
                if (regEmp != null)
                    empresa = regEmp.Emprnomb;

                MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);
                formato.IdFormatoNuevo = model.IdFormatoNuevo;
                formato.FlagUtilizaHoja = model.UtilizaHoja;
                formato.ListaHoja = servFormato.GetByCriteriaMeHoja(idFormato);
                if (model.IdHojaPadre > 0)
                {
                    formato.ListaHoja = formato.ListaHoja.Where(x => x.Hojapadre == model.IdHojaPadre).ToList();
                }

                var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                formato.Formatcols = cabecera.Cabcolumnas;
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatheaderrow = cabecera.Cabcampodef;
                int filaHead = formato.Formatrows;
                int colHead = formato.Formatcols;

                /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
                formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
                FormatoMedicionAppServicio.GetSizeFormato(formato);

                formato.Emprcodi = idEmpresa;
                string tipoPlazo = servFormato.EnvioValidarPlazo(formato, idEmpresa);
                if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == tipoPlazo)
                    throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + formato.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));
                model.Formato = formato;
                /////////////// Grabar Config Formato Envio //////////////////
                var idCfgFormato = 0;
                var listaPto = servFormato.GetListaPtos(formato.FechaFin, idCfgFormato, idEmpresa, idFormato, cabecera.Cabquery);
                int nPtos = listaPto.Count();

                MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
                config.Formatcodi = idFormato;
                config.Emprcodi = idEmpresa;
                config.FechaInicio = formato.FechaFin;
                int idConfig = servFormato.GrabarConfigFormatEnvio(config);
                ///////////////Grabar Envio//////////////////////////
                string mensajePlazo = string.Empty;
                formato.Emprcodi = idEmpresa;
                Boolean enPlazo = servFormato.ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);
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
                envio.Fdatcodi = 0;
                envio.Cfgenvcodi = idConfig;
                int idEnvio = servFormato.SaveMeEnvio(envio);
                modelResultado.IdEnvio = idEnvio;
                ///////////////////////////////////////////////////////
                int horizonte = formato.Formathorizonte;

                //////////////////////////////////////////////////////
                //obtener el formato del periodo anterior para comparar la similitud de data                        
                MeFormatoDTO formatoAnterior = servFormato.GetByIdMeFormato(idFormato);
                formatoAnterior.Formatcols = cabecera.Cabcolumnas;
                formatoAnterior.Formatrows = cabecera.Cabfilas;
                formatoAnterior.Formatheaderrow = cabecera.Cabcampodef;
                formatoAnterior.FechaProceso = ToolsFormato.GetFechaProcesoAnterior(formatoAnterior.Formatperiodo, formato.FechaProceso);
                FormatoMedicionAppServicio.GetSizeFormato(formatoAnterior);

                ///FormatoModel
                model.Empresa = empresa;
                model.ListaHojaPto = listaPto;
                /// Generar hojas
                if (model.UtilizaHoja)
                {
                    model.Formato = formato;
                    GenerarHojaFormato(model);
                    GenerarHojaFormatoFromData(model, model.ListaHoja, model.ListaData);
                }
                ///

                #region Grabar valor en Tablas de ME_MEDICIONn
                switch (formato.Formatresolucion)
                {
                    case ParametrosFormato.ResolucionCuartoHora:
                        var lista96 = new List<MeMedicion96DTO>();
                        if (!model.UtilizaHoja)
                        {
                            lista96 = ObtenerDatos96(data, listaPto, formato.Formatcheckblanco, (int)formato.Formatrows, formato.Lectcodi);
                        }
                        else
                        {
                            lista96 = ObtenerDatosHojaFormato96(model, formato.Formatcheckblanco, (int)formato.Formatrows, formato.Lectcodi);
                        }

                        bool existePeriodoAnteriorSimilar96 = model.ValidaSimilitudDataPeriodoAnt && servFormato.ValidarDataPeriodoAnterior96(lista96, idEmpresa, formatoAnterior);
                        if (!existePeriodoAnteriorSimilar96)
                        {
                            //grabar
                            servFormato.GrabarValoresCargados96(lista96, User.Identity.Name, idEnvio, idEmpresa, formato);
                        }
                        else
                        {
                            //mandar mensaje
                            throw new Exception("Los datos del periodo anterior son similares al periodo seleccionado");
                        }

                        //Validación de grabar datos de Medidores de Generación
                        if (ConstantesMedidores.IdFormatoCargaCentralPotReactiva == idFormato)
                        {
                            if (!enPlazo)
                            {
                                //verificar si existe data de Potencia Reactiva
                                var listaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesMedidores.IdFormatoCargaCentralPotActiva, formato.FechaProceso);
                                if (listaEnvios.Count == 0)
                                {
                                    throw new Exception("No existen datos de  Potencia Activa");
                                }
                            }
                        }
                        if (ConstantesMedidores.IdFormatoCargaServAuxPotActiva == idFormato)
                        {
                            if (!enPlazo)
                            {
                                //verificar si existe data de Potencia Reactiva
                                var listaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesMedidores.IdFormatoCargaCentralPotActiva, formato.FechaProceso);
                                if (listaEnvios.Count == 0)
                                {
                                    throw new Exception("No existen datos de  Potencia Activa");
                                }
                            }
                        }


                        break;
                    case ParametrosFormato.ResolucionMediaHora:
                        var lista48 = new List<MeMedicion48DTO>();
                        if (!model.UtilizaHoja)
                        {
                            lista48 = this.ObtenerDatos48(data, listaPto, formato.Formatcheckblanco, (int)formato.Formatrows, formato.Lectcodi);
                        }
                        else
                        {
                            lista48 = this.ObtenerDatosHojaFormato48(model, formato.Formatcheckblanco, (int)formato.Formatrows);
                        }

                        bool existePeriodoAnteriorSimilar48 = false; //ASSETEC 201909 - model.ValidaSimilitudDataPeriodoAnt && servFormato.ValidarDataPeriodoAnterior48(lista48, idEmpresa, formatoAnterior);
                        if (!existePeriodoAnteriorSimilar48)
                        {
                            //grabar
                            servFormato.GrabarValoresCargados48(lista48, User.Identity.Name, idEnvio, idEmpresa, formato);
                        }
                        else
                        {
                            //mandar mensaje
                            throw new Exception("Los datos del periodo anterior son similares al periodo seleccionado");
                        }

                        break;
                    case ParametrosFormato.ResolucionHora:
                        break;
                    case ParametrosFormato.ResolucionDia:
                        var lista1 = ObtenerDatos1(data, listaPto, formato.Formatcheckblanco, (int)formato.Formatrows, formato.Lectcodi);

                        //grabar
                        servFormato.GrabarValoresCargados1(lista1, User.Identity.Name, idEnvio, idEmpresa, formato, formato.Lectcodi);
                        break;
                    case ParametrosFormato.ResolucionMes:
                    case ParametrosFormato.ResolucionSemana:
                        break;
                }
                #endregion

                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                servFormato.UpdateMeEnvio(envio);

                //////////////////////////////////////////////////////
                if (listaJustificacion != null && listaJustificacion.Count > 0)
                {
                    //guardar justificacion de datos congelados
                    servFormato.GrabarJustificacionCongelados(listaJustificacion, idEnvio, User.Identity.Name);
                }

                ////////// Grabar Validación ////////////////////////////////////////////
                MeValidacionDTO validacion = new MeValidacionDTO();
                validacion.Emprcodi = idEmpresa;
                validacion.Formatcodi = formato.Formatcodi;
                validacion.Validfechaperiodo = formato.FechaProceso;
                validacion.Validestado = ConstanteValidacion.EstadoNoValidado;
                validacion.Validusumodificacion = User.Identity.Name;
                validacion.Validfecmodificacion = DateTime.Now;
                this.servFormato.GrabarMeValidacion(validacion, formato);
                //////////////////////////////////////////////////////

                try
                {
                    EnviarCorreo(enPlazo, idEnvio, idEmpresa, formato.Formatnombre, empresa, formato.Areaname, formato.FechaProceso, (DateTime)envio.Enviofecha);
                }
                catch
                {

                }

                exito = 1;
                modelResultado.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(ConstantesAppServicio.LogError, ex);
                exito = -1;
                modelResultado.Resultado = -1;
                modelResultado.Mensaje = ex.Message;
            }

            modelResultado.Resultado = exito;
            return modelResultado;
        }

        /// <summary>
        /// Lee los datos del  formato web Despacho diario y los almacena en una lista de DTO Medicion48
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> ObtenerDatos1(string[][] datos, List<MeHojaptomedDTO> ptos, int checkBlanco, int filCabecera, int lectcodi)
        {
            int nFil = datos.Length;
            int nCol = ptos.Count + 1;
            List<MeMedicion1DTO> lista = new List<MeMedicion1DTO>();
            MeMedicion1DTO reg;
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            fecha = DateTime.ParseExact(datos[filCabecera][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
            for (var i = 1; i < nCol; i++)
            {
                for (var j = filCabecera; j < nFil; j++)
                {
                    reg = new MeMedicion1DTO();
                    int indice = (j - filCabecera) % 1 + 1;

                    stValor = datos[j][i];
                    if (COES.Base.Tools.Util.EsNumero(stValor))
                    {
                        valor = decimal.Parse(stValor);
                        reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                    }
                    else
                    {
                        if (checkBlanco == 0)
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, (decimal?)0);
                    }

                    fecha = DateTime.ParseExact(datos[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);

                    reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                    reg.Tipoinfocodi = ptos[i - 1].Tipoinfocodi;
                    reg.Tipoptomedicodi = ptos[i - 1].Tptomedicodi;
                    reg.Lectcodi = lectcodi;
                    reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                    reg.Emprcodi = ptos[i - 1].Emprcodi;

                    lista.Add(reg);
                }
            }

            return lista;
        }

        /// <summary>
        /// Generar m96 a partir de excelweb
        /// </summary>
        /// <param name="modelPrincipal"></param>
        /// <param name="checkBlanco"></param>
        /// <param name="filCabecera"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDatosHojaFormato48(FormatoModel modelPrincipal, int checkBlanco, int filCabecera)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();

            foreach (var model in modelPrincipal.ListaFormatoModel)
            {
                lista.AddRange(ObtenerDatos48(model.Handson.ListaExcelData, model.ListaHojaPto, checkBlanco, filCabecera, model.Hoja.Lectcodi.Value));
            }

            return lista;
        }

        /// <summary>
        /// Lee los datos del  formato web Despacho diario y los almacena en una lista de DTO Medicion48
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDatos48(string[][] datos, List<MeHojaptomedDTO> ptos, int checkBlanco, int filCabecera, int lectcodi)
        {
            int nFil = datos.Length;
            int nCol = ptos.Count + 1;
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            MeMedicion48DTO reg;
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            fecha = DateTime.ParseExact(datos[filCabecera][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
            for (var i = 1; i < nCol; i++)
            {
                reg = new MeMedicion48DTO();

                for (var j = filCabecera; j < nFil; j++)
                {
                    int indice = (j - filCabecera) % 48 + 1;

                    stValor = datos[j][i];
                    if (COES.Base.Tools.Util.EsNumero(stValor))
                    {
                        valor = decimal.Parse(stValor);
                        reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                    }
                    else
                    {
                        if (checkBlanco == 0)
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, (decimal?)0);
                    }

                    if (indice == 48)
                    {
                        lista.Add(reg);
                        reg = new MeMedicion48DTO();
                    }
                    if ((j - filCabecera) % 48 == 0)
                    {
                        fecha = DateTime.ParseExact(datos[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);

                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Tipoinfocodi = ptos[i - 1].Tipoinfocodi;
                        reg.Emprcodi = ptos[i - 1].Emprcodi;
                        reg.Meditotal = 0;
                        reg.Lectcodi = lectcodi;
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                        reg.Hojacodi = ptos[i - 1].Hojacodi;
                    }

                }
            }
            return lista;
        }

        /// <summary>
        /// Generar m96 a partir de excelweb
        /// </summary>
        /// <param name="modelPrincipal"></param>
        /// <param name="checkBlanco"></param>
        /// <param name="filCabecera"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerDatosHojaFormato96(FormatoModel modelPrincipal, int checkBlanco, int filCabecera, int lectcodi)
        {
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();

            foreach (var model in modelPrincipal.ListaFormatoModel)
            {
                lista.AddRange(ObtenerDatos96(model.Handson.ListaExcelData, model.ListaHojaPto, checkBlanco, filCabecera, lectcodi));
            }

            return lista;
        }

        /// <summary>
        /// Lee los datos del  formato web Despacho diario y los almacena en una lista de DTO Medicion48
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerDatos96(string[][] datos, List<MeHojaptomedDTO> ptos, int checkBlanco, int filCabecera, int lectcodi)
        {
            int nFil = datos.Length;
            int nCol = ptos.Count + 1;
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            MeMedicion96DTO reg;
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            fecha = DateTime.ParseExact(datos[filCabecera][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
            for (var i = 1; i < nCol; i++)
            {
                reg = new MeMedicion96DTO();

                for (var j = filCabecera; j < nFil; j++)
                {
                    int indice = (j - filCabecera) % 96 + 1;

                    stValor = datos[j][i];
                    if (COES.Base.Tools.Util.EsNumero(stValor))
                    {
                        valor = decimal.Parse(stValor);
                        reg.Meditotal += valor;
                        reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                    }
                    else
                    {
                        if (checkBlanco == 0)
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, (decimal?)0);
                    }

                    if (indice == 96)
                    {
                        lista.Add(reg);
                        reg = new MeMedicion96DTO();
                    }
                    if ((j - filCabecera) % 96 == 0)
                    {
                        fecha = DateTime.ParseExact(datos[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);

                        reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        reg.Tipoinfocodi = ptos[i - 1].Tipoinfocodi;
                        reg.Tipoptomedicodi = ptos[i - 1].Tptomedicodi;
                        reg.Emprcodi = ptos[i - 1].Emprcodi;
                        reg.Meditotal = 0;
                        reg.Lectcodi = lectcodi;
                        reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                    }
                }
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
            string strNombreArchivo = string.Empty;
            string strArchivoTemporal = Request["archivo"];
            int pos = strArchivoTemporal.IndexOf(',');
            if (pos > 0)
            {
                strNombreArchivo = strArchivoTemporal.Substring(pos + 1, strArchivoTemporal.Length - pos - 1) + ".xlsx";
                strArchivoTemporal = strArchivoTemporal.Substring(0, pos);
            }
            else
            {
                strNombreArchivo = string.Format("Archivo_{0:dd-mm-yyyy}.xlsx", DateTime.Now);
            }
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);


                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        /// <summary>
        /// //Mostrar del manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult MostrarManualUsuario(int? app = 0)
        {
            if (app == ConstantesFormatoMedicion.AplicativoProgRER)
            {
                string nombreArchivo = "Manual_de_Usuario_Extranet_GeneracionRER.pdf";
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/" + nombreArchivo;
                return File(fullPath, Constantes.AppPdf, nombreArchivo);
            }
            return null;
        }

        /// <summary>
        /// Envia correo segun plantilla y graba el correo en base de datos
        /// </summary>
        /// <param name="enPlazo"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formatoNombre"></param>
        /// <param name="empresaNombre"></param>
        /// <param name="areaNombre"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="fechaEnvio"></param>
        protected void EnviarCorreo(bool enPlazo, int idEnvio, int idEmpresa, string formatoNombre, string empresaNombre,
            string areaNombre, DateTime fechaProceso, DateTime fechaEnvio)
        {
            var usuario = User.Identity.Name;

            ////IdModulo 2: Demanda 
            //var plantilla = (int)base.IdModulo == 2 ? servCorreo.GetByIdSiPlantillacorreo(105) : servCorreo.GetByIdSiPlantillacorreo(1);

            //if (plantilla != null)
            //{
            //    List<string> ccEmails = seguridad.ObtenerModulo((int)base.IdModulo).ListaAdministradores.ToList().Select(x => x.UserEmail).ToList();
            //    string ccMail = string.Empty;
            //    string cumplimiento = enPlazo ? " En Plazo" : " En fuera de plazo";
            //    string asunto = string.Format(plantilla.Plantasunto, formatoNombre);
            //    List<string> toMail = new List<string>();
            //    usuario = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;

            //    toMail.Add(usuario);
            //    string contenido = string.Format(plantilla.Plantcontenido, User.Identity.Name, cumplimiento, empresaNombre, formatoNombre
            //        , fechaProceso.ToString(Constantes.FormatoFecha), ((DateTime)fechaEnvio).ToString(Constantes.FormatoFechaFull), idEnvio, areaNombre);
            //    COES.Base.Tools.Util.SendEmail(toMail, ccEmails, asunto, contenido);
            //    var correo = new SiCorreoDTO();
            //    correo.Corrasunto = asunto;
            //    correo.Corrcontenido = contenido;
            //    correo.Corrfechaenvio = fechaEnvio;
            //    correo.Corrfechaperiodo = fechaProceso;
            //    correo.Corrfrom = HelperApp.ObtenerEmailRemitente();
            //    correo.Corrto = usuario;
            //    correo.Emprcodi = idEmpresa;
            //    correo.Enviocodi = idEnvio;
            //    correo.Plantcodi = plantilla.Plantcodi;
            //    servCorreo.SaveSiCorreo(correo);
            //}
        }

        /// <summary>
        /// inicializa matriz de contenido para la grilla excel web
        /// </summary>
        /// <param name="MatrizEstado"></param>
        /// <param name="nfilas"></param>
        /// <param name="ncol"></param>
        public void ConfiguraDatosMatrizExcel(short[][] MatrizTipoEstado, int filCabecera, int ColCabecera)
        {
            int nfil = MatrizTipoEstado.Count();
            int ncol = MatrizTipoEstado[0].Count();
            for (int i = filCabecera; i < nfil; i++)
            {
                for (int j = ColCabecera; j < ncol; j++)
                {
                    if (MatrizTipoEstado[i][j] == -1) // si la celda esta de solo lectura borramos data proveniente de excel
                    {
                        this.MatrizExcel[i][j] = "";
                    }
                }
            }
        }

        /// <summary>
        /// carga los datos del model para el panel IEOD
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GetInformacionPanelIEOD(int idEmpresa, string fecha)
        {
            try
            {
                if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);
                if (this.IdModulo == null) throw new Exception(Constantes.MensajeAccesoNoPermitido);

                DateTime dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string fechaEnergSolar = System.Configuration.ConfigurationManager.AppSettings[ConstantesIEOD.KeyFechaIniProcesoFEnergSolar];
                DateTime dfechaEnergSolar = DateTime.ParseExact(fechaEnergSolar, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                FormatoModel model = new FormatoModel();
                model.ListaPanelIEOD = servIEOD.ObtenerPanelIEOD((int)this.IdModulo, idEmpresa, dfecha, dfechaEnergSolar);

                return Json(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message, Detalle = ex.StackTrace });
            }
        }

        /// <summary>
        /// Establecer los valores de los flags de validaciones
        /// </summary>
        /// <param name="model"></param>
        /// <param name="formatcodi"></param>
        private void AsignarVerificacionFormato(FormatoModel model, int formatcodi)
        {
            var listaVerificacionFormato = this.servFormato.ListMeVerificacionFormatosByFormato(formatcodi);

            if (listaVerificacionFormato.Count > 0)
            {
                listaVerificacionFormato = listaVerificacionFormato.Where(x => x.Fmtverifestado == ConstantesAppServicio.Activo).ToList();

                model.ValidaMantenimiento = false;
                model.ValidaRestricOperativa = false;
                model.ValidaEventos = false;
                model.ValidaHorasOperacion = false;
                model.ValidaMedidorGeneracion = false;
                model.ValidaCentralSolar = false;

                foreach (var vf in listaVerificacionFormato)
                {
                    switch (vf.Verifcodi)
                    {
                        case ConstantesVerificacion.IdVerifMantenimiento:
                            model.ValidaMantenimiento = true;
                            break;
                        case ConstantesVerificacion.IdVerifRestriccionOperativa:
                            model.ValidaRestricOperativa = true;
                            break;
                        case ConstantesVerificacion.IdVerifEvento:
                            model.ValidaEventos = true;
                            break;
                        case ConstantesVerificacion.IdVerifHorasOperacion:
                            model.ValidaHorasOperacion = true;
                            break;
                        case ConstantesVerificacion.IdVerifMedidorGeneracion:
                            model.ValidaMedidorGeneracion = true;
                            break;
                        case ConstantesVerificacion.IdVerifCentralSolar:
                            model.ValidaCentralSolar = true;
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// Obtener la los valores inciales de las fechas del envío
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tipo"></param>
        /// <param name="model"></param>
        public void GetFechaActualEnvio(FormatoModel model)
        {
            string fecha = string.Empty;
            int semana = 0;
            int anho = 0;
            string mes = string.Empty;

            var formato = this.servFormato.GetByIdMeFormato(model.IdFormato);
            int periodo = formato.Formatperiodo.GetValueOrDefault(0);
            int tipo = formato.Lecttipo;

            DateTime fechaActual = DateTime.Now;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoDiario:
                    if (tipo == ParametrosFormato.Ejecutado)
                    {
                        fecha = fechaActual.AddDays(-1).ToString(Constantes.FormatoFecha);
                    }
                    if (tipo == ParametrosFormato.Programado)
                    {
                        fecha = fechaActual.ToString(Constantes.FormatoFecha);
                    }
                    if (tipo == ParametrosFormato.TiempoReal)
                    {
                        fecha = fechaActual.ToString(Constantes.FormatoFecha);
                    }

                    break;
                case ParametrosFormato.PeriodoSemanal:
                    var totalSemanasAnho = EPDate.TotalSemanasEnAnho(fechaActual.Year, 6);
                    semana = EPDate.f_numerosemana(fechaActual);
                    anho = fechaActual.Year;
                    if (tipo == ParametrosFormato.Ejecutado)
                    {
                        if (semana == 1)
                        {
                            semana = totalSemanasAnho;
                            anho = anho - 1;
                        }
                        else
                            semana--;
                    }
                    else
                    {

                        if (semana == totalSemanasAnho)
                        {
                            semana = 1;
                            anho++;
                        }
                        else
                            semana++;
                    }
                    break;
                case ParametrosFormato.PeriodoMensual:
                    if (tipo == ParametrosFormato.Ejecutado)
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    else
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    if (tipo == ParametrosFormato.Ejecutado)
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    else
                    {
                        mes = EPDate.GetFechaMes(fechaActual);
                    }
                    break;
                case ParametrosFormato.PeriodoAnual:
                    break;
            }

            model.Fecha = fecha;
            model.Anho = anho + string.Empty;
            model.Semana = semana + string.Empty;
            model.Mes = mes;
            model.Periodo = periodo;
        }

    }
}
