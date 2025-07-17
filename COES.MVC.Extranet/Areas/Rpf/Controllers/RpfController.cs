using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.ServicioRPF;
using COES.Servicios.Aplicacion.ServicioRPF.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Rpf.Controllers
{
    public class RpfController : FormatoController
    {
        RpfAppServicio servicio = new RpfAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(RpfController));
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

        public DateTime FechaCarga
        {
            get
            {
                return (Session[DatosSesion.SesionFechaProceso] != null) ?
                    (DateTime)Session[DatosSesion.SesionFechaProceso] : DateTime.Now;
            }
            set { Session[DatosSesion.SesionFechaProceso] = value; }
        }

        /// <summary>
        /// Estrcutura con los datos a grabar
        /// </summary>
        public List<MeMedicion60DTO> ListaProceso
        {
            get
            {
                return (Session[DatosSesion.SesionListaProcesar] != null) ?
                    (List<MeMedicion60DTO>)Session[DatosSesion.SesionListaProcesar] : new List<MeMedicion60DTO>();
            }
            set { Session[DatosSesion.SesionListaProcesar] = value; }
        }

        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            //if (this.IdModulo == null) return base.RedirectToHomeDefault();

            FormatoModel model = new FormatoModel();
            base.IndexFormato(model, ConstantesHard.IdFormatoRpf);

            return View(model);
        }

        public ActionResult Carga()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            //if (this.IdModulo == null) return base.RedirectToHomeDefault();

            FormatoModel model = new FormatoModel();
            base.IndexFormato(model, ConstantesHard.IdFormatoRpf);

            model.MensajeError = string.Format(ConstantesRpf.MensajeInicio, ConstantesRpf.HoraPermitida);

            return View(model);
        }

        public ActionResult Consulta()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            //if (this.IdModulo == null) return base.RedirectToHomeDefault();

            FormatoModel model = new FormatoModel();
            base.IndexFormato(model, ConstantesHard.IdFormatoRpf);

            model.ListaAreas = new List<FwAreaDTO>();
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "1", Areaname = "01:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "2", Areaname = "02:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "3", Areaname = "03:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "4", Areaname = "04:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "5", Areaname = "05:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "6", Areaname = "06:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "7", Areaname = "07:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "8", Areaname = "08:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "9", Areaname = "09:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "10", Areaname = "10:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "11", Areaname = "11:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "12", Areaname = "12:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "13", Areaname = "13:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "14", Areaname = "14:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "15", Areaname = "15:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "16", Areaname = "16:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "17", Areaname = "17:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "18", Areaname = "18:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "19", Areaname = "19:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "20", Areaname = "20:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "21", Areaname = "21:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "22", Areaname = "22:00" });
            model.ListaAreas.Add(new FwAreaDTO() { Areaabrev = "23", Areaname = "23:00" });

            return View(model);
        }

        #region generar formato

        /// <summary>
        /// Permite generar el formato en formato excel de Despacho diario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormatoRpf(int idEmpresa, string fecha)
        {
            string ruta = string.Empty;
            try
            {
                int idEnvio = -1;
                FormatoModel model = BuildHojaExcelRpf(idEmpresa, idEnvio, fecha);
                ruta = this.GenerarFileExcelFormatoCsv(model);
                FechaCarga = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ruta = "-1";
            }
            return ruta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelPrincipal"></param>
        /// <returns></returns>
        private string GenerarFileExcelFormatoCsv(FormatoModel modelPrincipal)
        {
            string fileExcel = string.Empty;
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                this.GenerarFileExcelHojaFormato(modelPrincipal, xlPackage, Constantes.HojaFormatoExcel);

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }
            return fileExcel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xlPackage"></param>
        /// <param name="nombre"></param>
        private void GenerarFileExcelHojaFormato(FormatoModel model, ExcelPackage xlPackage, string nombre)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombre);
            //ws.View.ShowGridLines = false;

            //Imprimimos Codigo Empresa y Codigo Formato (ocultos)
            ws.Column(ConstantesFormato.ColEmpresaExtranet).Hidden = true;
            ws.Column(ConstantesFormato.ColFormatoExtranet).Hidden = true;
            ws.Cells[1, ConstantesFormato.ColEmpresaExtranet].Value = model.IdEmpresa.ToString();
            ws.Cells[1, ConstantesFormato.ColFormatoExtranet].Value = model.Formato.Formatcodi.ToString();

            int row = 1;
            int column = 1;

            int rowIniFormato = row;
            int colIniFormato = column;
            //ws.Cells[rowIniFormato, colIniFormato].Value = model.Formato.Formatnombre;
            //ws.Cells[rowIniFormato, colIniFormato].Style.Font.SetFromFont(new Font("Calibri", 14));
            //ws.Cells[rowIniFormato, colIniFormato].Style.Font.Bold = true;

            row = rowIniFormato;
            int rowIniEmpresa = row;
            int colIniEmpresa = column;
            int rowIniAnio = rowIniEmpresa + 1;

            //
            ws.Column(1).Width = 3;
            ws.Column(colIniFormato).Width = 19;

            ///Imprimimos cabecera de puntos de medicion
            row = 1;
            int rowIniPto = row;
            int colIniPto = column;
            int totColumnas = model.ListaHojaPto.Count;

            for (var i = 0; i <= totColumnas; i++)
            {
                if (i > 0)
                    ws.Column(colIniFormato + i).Width = 15;

                for (var j = 0; j < model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows; j++)
                {
                    decimal valor = 0;
                    model.Handson.ListaExcelData[j][i] = model.Handson.ListaExcelData[j][i] != null ? model.Handson.ListaExcelData[j][i] : string.Empty;
                    bool canConvert = decimal.TryParse(model.Handson.ListaExcelData[j][i], out valor);
                    if (canConvert)
                        ws.Cells[row + j, i + colIniPto].Value = valor;
                    else
                        ws.Cells[row + j, i + colIniPto].Value = model.Handson.ListaExcelData[j][i] != null ? model.Handson.ListaExcelData[j][i].Trim() : string.Empty;

                    //ws.Cells[row + j, i + colIniPto].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    //if (j < model.Formato.Formatrows && i >= model.Formato.Formatcols)
                    //{
                    //    //ws.Cells[row + j, i + colIniPto].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    //    //ws.Cells[row + j, i + colIniPto].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    //    //ws.Cells[row + j, i + colIniPto].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //    ws.Cells[row + j, i + colIniPto].Style.WrapText = true;
                    //}
                    //if (i == 0 && j >= model.Formato.Formatrows)
                    //{
                    //    ws.Cells[row + j, i + colIniPto].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //    ws.Cells[row + j, i + colIniPto].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //}
                }
            }
            /////////////////Formato a Celdas Head ///////////////////

            //using (var range = ws.Cells[rowIniPto, colIniPto, rowIniPto + model.Formato.Formatrows - 1, colIniPto])
            //{
            //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#87CEEB"));
            //    range.Style.Font.Color.SetColor(Color.White);
            //    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //    range.Style.WrapText = true;
            //}


            //using (var range = ws.Cells[rowIniPto, colIniPto + 1, rowIniPto + model.Formato.Formatrows - 1, colIniPto + model.ListaHojaPto.Count])
            //{
            //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //    range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
            //    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //}
            ////////////// Formato de Celdas Valores
            using (var range = ws.Cells[rowIniPto + model.Formato.Formatrows, colIniPto + 1
                , rowIniPto + model.Formato.Formathorizonte * model.Formato.RowPorDia + model.Formato.Formatrows - 1, colIniPto + model.ListaHojaPto.Count])
            {
                range.Style.Numberformat.Format = @"0.000";
            }
        }

        /// <summary>
        /// Devuelve el model con informacion de Despacho diario
        /// </summary>sic
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcelRpf(int idEmpresa, int idEnvio, string fecha)
        {
            var model = new FormatoModel { ViewHojaPto2 = 1 };
            DateTime dfecha = fecha != null && fecha.Trim() != string.Empty ? DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            int nBloques = 24 * 3600;
            model.Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
            var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == model.Formato.Cabcodi);
            model.ListaHojaPto = servFormato.GetListaPtos(dfecha, 0, idEmpresa, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);
            this.MatrizExcel = this.InicializaMatriz(cabecera.Cabfilas + 1, nBloques, model.ListaHojaPto.Count + 1, dfecha);

            this.BuildHojaExcel(model, idEmpresa, idEnvio, fecha, ConstantesHard.IdFormatoRpf, ConstantesFormato.NoVerUltimoEnvio);
            model.Formato.RowPorDia = nBloques;

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowsHead"></param>
        /// <param name="nFil"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public string[][] InicializaMatriz(int rowsHead, int nFil, int nCol, DateTime fecha)
        {
            var rows = nFil + rowsHead - 1;
            string[][] matriz = new string[rows][];
            for (int i = 0; i < rows; i++)
            {
                matriz[i] = new string[nCol];
                for (int j = 0; j < nCol; j++)
                {
                    if (i > 5)
                    {
                        if (j > 0) { matriz[i][j] = string.Empty; }
                        else
                        {
                            matriz[i][j] = fecha.AddSeconds(i - 6).ToString(ConstantesAppServicio.FormatoFechaFull2);
                        }
                    }
                    else { matriz[i][j] = string.Empty; }
                }
            }

            return matriz;
        }

        /// <summary>
        /// Carga la informacion necesar en el model para construir el grid excel web.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="idFormato"></param>
        //public void BuildHojaExcelRpf(FormatoModel model, int idEmpresa, int idEnvio, string fecha, int idFormato)
        //{
        //    short[][] matrizTipoEstado;
        //    List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>();
        //    List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
        //    model.Handson = new HandsonModel();
        //    model.Handson.ListaMerge = new List<CeldaMerge>();

        //    DateTime dfecha = fecha != null && fecha.Trim() != string.Empty ? DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

        //    ////////// Obtiene el Formato ////////////////////////
        //    model.IdEmpresa = idEmpresa;
        //    model.IdFormato = idFormato;
        //    model.Formato = servFormato.GetByIdMeFormato(idFormato);
        //    var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
        //    /// DEFINICION DEL FORMATO //////
        //    model.Formato.Formatcols = cabecera.Cabcolumnas;
        //    model.Formato.Formatrows = cabecera.Cabfilas;
        //    model.Formato.Formatheaderrow = cabecera.Cabcampodef;
        //    model.ColumnasCabecera = model.Formato.Formatcols;
        //    model.FilasCabecera = model.Formato.Formatrows;
        //    model.ValidacionFormatoCheckblanco = model.Formato.Formatcheckblanco == 1;

        //    ///
        //    var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
        //    if (entEmpresa != null)
        //        model.Empresa = entEmpresa.Emprnomb;

        //    int idCfgFormato = 0;
        //    model.Formato.IdEnvio = idEnvio;
        //    if (idEnvio <= 0)
        //    {
        //        model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, model.Mes, model.Semana, fecha, Constantes.FormatoFecha);
        //        ToolsFormato.GetSizeFormato(model.Formato);
        //        model.EnPlazo = ValidarPlazo(model.Formato);
        //        model.Handson.ReadOnly = !ValidarFecha(model.Formato, idEmpresa);
        //    }

        //    //cambiar la fecha del form por la fecha ini
        //    fecha = ToolsFormato.getDateFormatDDMMYYYY(model.Formato.FechaInicio);

        //    model.Anho = model.Formato.FechaInicio.Year.ToString();
        //    model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);
        //    model.Dia = model.Formato.FechaInicio.Day.ToString();

        //    /// Lista Hojapto
        //    model.ListaHojaPto = servFormato.GetListaPtos(idCfgFormato, idEmpresa, idFormato, cabecera.Cabquery);
        //    ///

        //    model.Handson.ListaColWidth = new List<int>();
        //    model.Handson.ListaColWidth.Add(150);
        //    for (var i = 0; i < model.ListaHojaPto.Count; i++)
        //    {
        //        model.Handson.ListaColWidth.Add(100);
        //    }


        //    var cabecerasRow = cabecera.Cabcampodef.Split(ConstantesFormato.SeparadorFila);

        //    for (var x = 0; x < cabecerasRow.Length; x++)
        //    {
        //        var reg = new CabeceraRow();
        //        var fila = cabecerasRow[x].Split(ConstantesFormato.SeparadorCol);
        //        reg.NombreRow = fila[0];
        //        reg.TituloRow = fila[1];
        //        reg.IsMerge = int.Parse(fila[2]);
        //        listaCabeceraRow.Add(reg);
        //    }

        //    model.Formato.RowPorDia = 24 * 3600;
        //    matrizTipoEstado = ToolsFormato.IncializaMatrizEstado(model.ListaHojaPto, model.FilasCabecera, model.Formato.RowPorDia, -1, model.Formato.Formathorizonte);

        //    model.Handson.MatrizTipoEstado = matrizTipoEstado;

        //    /// Generar hojas
        //    if (model.UtilizaHoja)
        //    {
        //        //GenerarHojaFormato(model);
        //    }
        //    ///

        //    {
        //        //TODO se comento esta linea, model.Handson.MatrizEstado como es generado?
        //        //ConfiguraDatosMatrizExcel(model.Handson.MatrizEstado, model.Formato.RowPorDia + model.FilasCabecera, model.ListaHojaPto.Count + 1);                
        //        if (!model.UtilizaHoja)
        //        {
        //            if (!model.Handson.ReadOnly)
        //            {
        //                ConfiguraDatosMatrizExcel(model.Handson.MatrizTipoEstado, model.FilasCabecera, model.ColumnasCabecera);
        //            }
        //            model.Handson.ListaExcelData = this.MatrizExcel;
        //        }
        //        else
        //        {
        //            GenerarHojaFormatoFromData(model, this.ListaHoja, this.ListaMatrizExcel);
        //        }
        //    }

        //    //si el formato tiene varias hoja
        //    if (!model.UtilizaHoja)
        //    {
        //        GenerarCabecera(model, listaCabeceraRow);
        //    }
        //    else
        //    {
        //        //GenerarCabeceraHojaFormato(model, listaCabeceraRow);
        //    }
        //}

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormatoCsv()
        {

            string strArchivoTemporal = Request["archivo"];

            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);


                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("Archivo_{0:yyyyMMdd}{1}", FechaCarga, ConstantesAppServicio.ExtensionExcel);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #endregion

        #region carga

        /// <summary>
        /// Action para procesamiento del archivo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarArchivo(int idEmpresa, string fecha)
        {
            string mensaje = string.Empty;
            this.ListaProceso = null;

            try
            {
                if (Session[DatosSesion.SesionFileName] != null)
                {
                    string fileName = Session[DatosSesion.SesionFileName].ToString();

                    if (System.IO.File.Exists(fileName))
                    {
                        List<String> validaciones = new List<String>();
                        bool flag = true;

                        string validacionHora = Constantes.NO;

                        DateTime fechaValidacion = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                        int nBloques = 24 * 3600;
                        var Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
                        var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == Formato.Cabcodi);
                        var ListaHojaPto = servFormato.GetListaPtos(fechaValidacion, 0, idEmpresa, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);

                        List<MeMedicion60DTO> list = servicio.ProcesarArchivoRpf(fileName, out validaciones, out flag, fechaValidacion, validacionHora, cabecera, ListaHojaPto, nBloques);

                        if (flag)
                        {
                            this.FechaCarga = fechaValidacion;
                            mensaje = ValidacionArchivo.OK;
                            this.ListaProceso = list;
                        }
                        else { mensaje = ToolHtml.ObtieneListaValidacion(validaciones); }
                    }
                    else { mensaje = ToolHtml.ObtenerValidacion(ValidacionArchivoRpf.NoExiste); }
                }
                else { mensaje = ToolHtml.ObtenerValidacion(ValidacionArchivo.NoExiste); }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                mensaje = ValidacionArchivo.Error;
            }

            var jsonResult = Json(mensaje);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Action para grabar en db el archivo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(int idEmpresa)
        {
            ResultadoOperacion resultado = new ResultadoOperacion();

            try
            {
                if (this.ListaProceso.Count > 0)
                {
                    var ptos = (from item in this.ListaProceso select item.Ptomedicodi).Distinct().ToList();
                    var tipoInfoCodi = (from item in this.ListaProceso select item.Tipoinfocodi).Distinct().ToList();

                    if (this.FechaCarga.Day == 1)
                    {
                        DateTime f2 = this.FechaCarga.AddMonths(1).AddDays(-1);
                        servicio.EliminarCargaRpf(string.Join(",", ptos), this.FechaCarga, f2, this.FechaCarga.Month, string.Join(",", tipoInfoCodi));
                    }

                    servicio.EliminarCargaRpf(string.Join(",", ptos), this.FechaCarga, this.FechaCarga.AddDays(1), this.FechaCarga.Month, string.Join(",", tipoInfoCodi));

                    servicio.GrabarDatosRpf(this.ListaProceso, this.FechaCarga, this.FechaCarga.Month);

                    foreach (int pto in ptos)
                    {
                        LogEnvioMedicionDTO entity = new LogEnvioMedicionDTO();
                        entity.EmprCodi = idEmpresa;
                        entity.LastUser = this.UserName;
                        entity.LastDate = DateTime.Now;
                        entity.FilNomb = this.NombreFile;
                        entity.LogDesc = string.Empty;
                        entity.PtoMediCodi = pto;
                        entity.Fecha = this.FechaCarga;

                        servicio.GrabarLogEnvio(entity);
                    }

                    var Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
                    var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == Formato.Cabcodi);
                    var ListaHojaPto = servFormato.GetListaPtos(this.FechaCarga, 0, idEmpresa, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);
                    List<MePtomedicionDTO> listaPto = ListaHojaPto.Where(x =>
                        ptos.Contains(x.Ptomedicodi))
                        .GroupBy(x => new { Ptomedicodi = x.Ptomedicodi, PtoMediEleNomb = x.PtoMediEleNomb })
                        .Select(x => new MePtomedicionDTO() { Ptomedicodi = x.Key.Ptomedicodi, Ptomedielenomb = x.Key.PtoMediEleNomb }).ToList();

                    StringBuilder str = new StringBuilder();

                    str.Append(ConstantesRpf.AperturaResultado);

                    foreach (var punto in listaPto)
                    {
                        str.Append(String.Format(ConstantesRpf.ItemResultado, punto.Ptomedielenomb));
                    }

                    str.Append(ConstantesRpf.CierreResultado);

                    resultado.Mensaje = str.ToString();
                }
                else
                {
                    resultado.Resultado = 0;
                }

                resultado.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                resultado.Resultado = -1;
            }

            return Json(resultado);
        }

        #endregion

        #region consulta

        /// <summary>
        /// Cargas las centrales por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarCentrales(string fecha, int idEmpresa)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            var Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
            var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == Formato.Cabcodi);
            var ListaHojaPto = servFormato.GetListaPtos(fechaConsulta, 0, idEmpresa, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);

            List<EqEquipoDTO> entitys = ListaHojaPto.GroupBy(x => new { x.Equipadre, x.Equipopadre })
                .Select(x => new EqEquipoDTO() { Equicodi = x.Key.Equipadre, Equinomb = x.Key.Equipopadre }).OrderBy(x => x.Equinomb).ToList();

            SelectList list = new SelectList(entitys, "Equicodi", "Equinomb");
            return Json(list);
        }

        /// <summary>
        /// Carga los puntos de medicion por equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarPtosMedicion(string fecha, int idEquipo, int emprcodi)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            var Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
            var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == Formato.Cabcodi);
            var ListaHojaPto = servFormato.GetListaPtos(fechaConsulta, 0, emprcodi, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);
            ListaHojaPto = ListaHojaPto.Where(x => x.Equipadre == idEquipo).ToList();

            foreach (var d in ListaHojaPto) { d.Ptomedidesc = d.Ptomedicodi + " - " + d.Equinomb; }
            var ListaHojaPto_ = ListaHojaPto.Select(x => new { x.Ptomedicodi, x.Ptomedidesc }).OrderBy(x => x.Ptomedidesc).Distinct().ToList();

            SelectList list = new SelectList(ListaHojaPto_, "Ptomedicodi", "Ptomedidesc");
            return Json(list);
        }

        /// <summary>
        /// Permite consultar los datos cargados
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="ptoMedicion"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFin"></param>
        /// <param name="idTipoDato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grilla(string fecha, int ptoMedicion, int horaInicio, int horaFin, int idTipoDato)
        {
            FormatoModel model = new FormatoModel();
            DateTime fechaini = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechafin = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            var Lista = this.servicio.BuscarDatosRpf(fechaini.AddHours(horaInicio), fechafin.AddHours(horaFin), ptoMedicion, idTipoDato);
            model.Resultado = this.servicio.DatosRpfHtml(Lista);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region comparativo

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult Comparativo(int? idEmpresa, string fecha)
        {
            int? idCentral = null;
            return Json(this.CalcularComparativo(idEmpresa, idCentral, fecha));
        }

        /// <summary>
        /// Obtener comparativo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private List<ComparativoItemModelrpf> CalcularComparativo(int? idEmpresa, int? idCentral, string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<int> puntosRpf = new List<int>();
            List<int> puntosDespacho = new List<int>();
            (new ComparativoAppServicio()).ObtenerPuntosMedicion(idEmpresa, idCentral, out puntosRpf, out puntosDespacho, fechaConsulta);

            if (puntosDespacho.Count == 0)
            {
                return new List<ComparativoItemModelrpf>();
            }

            string rpf = string.Join<int>(Constantes.CaracterComa.ToString(), puntosRpf);
            string despacho = string.Join<int>(Constantes.CaracterComa.ToString(), puntosDespacho);

            List<MeMedicion60DTO> datosRpf = new List<MeMedicion60DTO>();

            if (Session[DatosSesion.SesionFileName] != null)
            {
                string fileName = Session[DatosSesion.SesionFileName].ToString();

                bool flag = true;
                List<String> validaciones = new List<String>();
                string validacionHora = Constantes.NO;

                int nBloques = 24 * 3600;
                var Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
                var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == Formato.Cabcodi);
                var ListaHojaPto = servFormato.GetListaPtos(fechaConsulta, 0, (int)idEmpresa, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);

                DateTime fechaValidacion = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                var listarpf = servicio.ProcesarArchivoRpf(fileName, out validaciones, out flag, fechaValidacion, validacionHora, cabecera, ListaHojaPto, nBloques);
                datosRpf = this.Lista48Rpf(listarpf);
            }
            MeMedicion48DTO datosDespacho = (new ComparativoAppServicio()).ObtenerDatosDespacho(fechaConsulta, despacho);

            List<ComparativoItemModelrpf> list = this.ObtenerComparativo(fechaConsulta, datosRpf, datosDespacho);

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datosRpf"></param>
        /// <returns></returns>
        private List<MeMedicion60DTO> Lista48Rpf(List<MeMedicion60DTO> datosRpf)
        {
            List<MeMedicion60DTO> Lista = new List<MeMedicion60DTO>();
            List<int> LMinutos = new List<int>();
            LMinutos.Add(0);
            LMinutos.Add(30);
            var lista1 = datosRpf.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW && LMinutos.Contains(x.Fechahora.Minute) && !(x.Fechahora.Hour == 0 && x.Fechahora.Minute == 0))
                .GroupBy(x => x.Fechahora).Select(x => new MeMedicion60DTO
                {
                    Fechahora = x.Key,
                    H0 = x.Sum(h => h.H0)
                }).ToList();
            var lista2 = datosRpf.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW && (x.Fechahora.Hour == 23 && x.Fechahora.Minute == 59))
                .GroupBy(x => x.Fechahora).Select(x => new MeMedicion60DTO
                {
                    Fechahora = x.Key,
                    H0 = x.Sum(h => h.H59)
                }).ToList();

            Lista.AddRange(lista1);
            Lista.AddRange(lista2);

            return Lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listRPF"></param>
        /// <param name="despacho"></param>
        /// <returns></returns>
        private List<ComparativoItemModelrpf> ObtenerComparativo(DateTime fecha, List<MeMedicion60DTO> listRPF, MeMedicion48DTO despacho)
        {
            List<ComparativoItemModelrpf> entitys = new List<ComparativoItemModelrpf>();

            if (listRPF.Count == 48 && despacho != null)
            {
                for (int i = 1; i <= 48; i++)
                {
                    ComparativoItemModelrpf entity = new ComparativoItemModelrpf();
                    entity.Hora = fecha.AddMinutes((i) * 30).ToString(ConstantesAppServicio.FormatoOnlyHora);
                    entity.ValorRPF = (decimal)listRPF[i - 1].H0;
                    entity.ValorDespacho = Convert.ToDecimal(despacho.GetType().GetProperty(Constantes.CaracterH + i).GetValue(despacho, null));
                    entity.Desviacion = (entity.ValorRPF != 0) ? (entity.ValorDespacho - entity.ValorRPF) / entity.ValorRPF : 0;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
