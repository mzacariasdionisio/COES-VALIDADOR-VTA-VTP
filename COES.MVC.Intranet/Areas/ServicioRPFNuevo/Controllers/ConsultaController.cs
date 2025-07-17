using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPFNuevo.Helper;
using COES.MVC.Intranet.Areas.ServicioRPFNuevo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.ServicioRPF;
using COES.Servicios.Aplicacion.ServicioRPF.Helper;
using log4net;
using OfficeOpenXml;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Controllers
{
    public class ConsultaController : FormatoController
    {
        RpfAppServicio logic = new RpfAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(RpfAppServicio));
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

        /// <summary>
        /// Pagina de consulta de carga de datos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ServicioModel model = new ServicioModel();
            model.FechaConsulta = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Pagina para consultar las potencias maximas
        /// </summary>
        /// <returns></returns>
        public ActionResult Potencias()
        {
            ServicioModel model = new ServicioModel();
            model.FechaConsulta = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

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
                FormatoModel model = BuildHojaExcelRpf(idEmpresa, idEnvio, fecha, ConstantesFormato.NoVerUltimoEnvio);
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
            ws.Column(ConstantesFormato.ColEmpresa).Hidden = true;
            ws.Column(ConstantesFormato.ColFormato).Hidden = true;
            ws.Cells[1, ConstantesFormato.ColEmpresa].Value = model.IdEmpresa.ToString();
            ws.Cells[1, ConstantesFormato.ColFormato].Value = model.Formato.Formatcodi.ToString();

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
                }
            }
            /////////////////Formato a Celdas Head ///////////////////

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
        public FormatoModel BuildHojaExcelRpf(int idEmpresa, int idEnvio, string fecha, int verUltimoEnvio)
        {
            var model = new FormatoModel { ViewHojaPto2 = 1 };
            DateTime dfecha = fecha != null && fecha.Trim() != string.Empty ? DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            int nBloques = 24 * 3600;
            model.Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
            var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == model.Formato.Cabcodi);
            model.ListaHojaPto = servFormato.GetListaPtos(dfecha, 0, idEmpresa, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);
            this.MatrizExcel = this.InicializaMatriz(cabecera.Cabfilas + 1, nBloques, model.ListaHojaPto.Count + 1, dfecha);

            this.BuildHojaExcel(model, idEmpresa, idEnvio, fecha, ConstantesHard.IdFormatoRpf, verUltimoEnvio);
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
        /// Pagina de carga de archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Carga()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            FormatoModel model = new FormatoModel();
            base.IndexFormato(model, ConstantesHard.IdFormatoRpf);

            model.MensajeError = string.Format(ConstantesRpf.MensajeInicio, ConstantesRpf.HoraPermitida);

            return View(model);
        }

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
                        List<MePtomedicionDTO> listItem = logic.ObtenerPuntosMedicion(idEmpresa);

                        string validacionHora = Constantes.NO;

                        List<int> codigos = (from items in listItem select items.Ptomedicodi).Distinct().ToList();

                        DateTime fechaValidacion = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                        int nBloques = 24 * 3600;
                        var Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
                        var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == Formato.Cabcodi);
                        var ListaHojaPto = servFormato.GetListaPtos(fechaValidacion, 0, idEmpresa, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);

                        List<MeMedicion60DTO> list = logic.ProcesarArchivoRpf(fileName, out validaciones, out flag, fechaValidacion, validacionHora, cabecera, ListaHojaPto, nBloques);

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
                        logic.EliminarCargaRpf(string.Join(",", ptos), this.FechaCarga, f2, this.FechaCarga.Month, string.Join(",", tipoInfoCodi));
                    }

                    logic.EliminarCargaRpf(string.Join(",", ptos), this.FechaCarga, this.FechaCarga.AddDays(1), this.FechaCarga.Month, string.Join(",", tipoInfoCodi));

                    logic.GrabarDatosRpf(this.ListaProceso, this.FechaCarga, this.FechaCarga.Month);

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

                        logic.GrabarLogEnvio(entity);
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

                DateTime fechaValidacion = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                int nBloques = 24 * 3600;
                var Formato = servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoRpf);
                var cabecera = servFormato.GetListMeCabecera().FirstOrDefault(x => x.Cabcodi == Formato.Cabcodi);
                var ListaHojaPto = servFormato.GetListaPtos(fechaValidacion, 0, (int)idEmpresa, ConstantesHard.IdFormatoRpf, cabecera.Cabquery);

                var listarpf = logic.ProcesarArchivoRpf(fileName, out validaciones, out flag, fechaValidacion, validacionHora, cabecera, ListaHojaPto, nBloques);
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

        #region consulta rpf

        public ActionResult ConsultaRpf()
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
            var Lista = logic.BuscarDatosRpf(fechaini.AddHours(horaInicio), fechafin.AddHours(horaFin), ptoMedicion, idTipoDato);
            model.Resultado = logic.DatosRpfHtml(Lista);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        /// <summary>
        /// Consulta el estado de envio
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PartialViewResult Consulta(string fecha)
        {
            ServicioModel model = new ServicioModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<ServicioRpfDTO> list = this.logic.ObtenerUnidadesCarga(fechaConsulta);
            List<int> ids = (from puntos in list select puntos.PTOMEDICODI).Distinct().ToList();

            List<ResultadoverificacionDTO> resultado = this.logic.VerificarEnvio(ids.ToArray(), fechaConsulta).ToList();

            foreach (ServicioRpfDTO item in list)
            {
                foreach (ResultadoverificacionDTO result in resultado)
                {
                    if (item.PTOMEDICODI == result.PtoMediCodi)
                    {
                        item.INDICADOR = (result.IndicadorEnvio == Constantes.SI) ? Constantes.TextoSI : Constantes.TextoNO;
                        break;
                    }
                }
            }

            model.ListaConsulta = list;

            return PartialView(model);
        }

        /// <summary>
        /// Consulta las potencias maximas
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public PartialViewResult ConsultaPotencia(string fecha)
        {
            ServicioModel model = new ServicioModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<ServicioRpfDTO> list = this.logic.ObtenerUnidadesCarga(fechaConsulta);

            List<RegistrorpfDTO> resultado = this.logic.ObtenerPotenciasMaximas(fechaConsulta).ToList();

            foreach (ServicioRpfDTO item in list)
            {
                RegistrorpfDTO rpf = resultado.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).FirstOrDefault();

                if (rpf != null)
                {
                    item.POTENCIAMAX = rpf.POTENCIA;
                }
            }

            model.ListaConsulta = list;

            return PartialView(model);
        }

        /// <summary>
        /// Genera el archivo del reporte de cumplimiento
        /// </summary>
        /// <param name="puntos"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivo(string puntos, string fecha)
        {
            int result = 1;
            try
            {
                string[] codigos = puntos.Split(Constantes.CaracterComa);
                List<int> list = new List<int>();
                foreach (string codigo in codigos)
                    if (!string.IsNullOrEmpty(codigo))
                        list.Add(int.Parse(codigo));

                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<MeMedicion60DTO> entitys = logic.DescargarEnvio(list, fechaConsulta).ToList();
                string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreCSVServicioRPF;

                this.GenerarArchivo(entitys, fullPath, fecha);

                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Exporta el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Exportar()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreCSVServicioRPF;
            return File(fullPath, Constantes.AppCSV, Constantes.NombreCSVServicioRPF);
        }

        /// <summary>
        /// Genera el archivo de reporte de potencias maximas
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoPotencia(string fecha)
        {
            int result = 1;
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<ServicioRpfDTO> list = this.logic.ObtenerUnidadesCarga(fechaConsulta);

                List<RegistrorpfDTO> resultado = this.logic.ObtenerPotenciasMaximas(fechaConsulta).ToList();

                foreach (ServicioRpfDTO item in list)
                {
                    RegistrorpfDTO rpf = resultado.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).FirstOrDefault();

                    if (rpf != null)
                    {
                        item.POTENCIAMAX = rpf.POTENCIA;
                    }
                }

                ExcelDocument.GenerarReportePotencia(list);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Exporta el reporte de potencias generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarPotencia()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreReporteRPFPotencia;
            return File(fullPath, Constantes.AppExcel, Constantes.NombreReporteRPFPotencia);
        }

        /// <summary>
        /// Permite generar el archivo de reporte de datos cargados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string fecha)
        {
            int indicador = 1;

            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                ServicioModel model = new ServicioModel();
                List<ServicioRpfDTO> list = this.logic.ObtenerUnidadesCarga(fechaConsulta);
                List<int> ids = (from puntos in list select puntos.PTOMEDICODI).Distinct().ToList();

                List<ResultadoverificacionDTO> resultado = this.logic.VerificarEnvio(ids.ToArray(), fechaConsulta).ToList();

                foreach (ServicioRpfDTO item in list)
                {
                    foreach (ResultadoverificacionDTO result in resultado)
                    {
                        if (item.PTOMEDICODI == result.PtoMediCodi)
                        {
                            item.INDICADOR = (result.IndicadorEnvio == Constantes.SI) ? Constantes.TextoSI : Constantes.TextoNO;
                            break;
                        }
                    }
                }

                ExcelDocument.GenerarReporteCarga(list);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreReporteCargaRPF;
            return File(fullPath, Constantes.AppExcel, Constantes.NombreReporteCargaRPF);
        }

        /// <summary>
        /// Generar el archivo CSV con los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="fileName"></param>
        /// <param name="fecha"></param>
        protected void GenerarArchivo(List<MeMedicion60DTO> entitys, string fileName, string fecha)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<int> puntos = (from punto in entitys select punto.Ptomedicodi).Distinct().ToList();
            List<ServicioRpfDTO> configuracion = this.logic.ObtenerUnidadesCarga(fechaConsulta);

            string[] texto = new string[86406];
            int[] tipos = { 1, 6 };

            texto[0] = " ";
            texto[1] = " ";
            texto[2] = "Empresa";
            texto[3] = "Central";
            texto[4] = "Unidad";
            texto[5] = "Fecha: ," + fecha;
            int t = 0;
            foreach (int punto in puntos)
            {
                ServicioRpfDTO entidad = configuracion.Where(x => x.PTOMEDICODI == punto).FirstOrDefault();

                for (int i = 0; i < tipos.Length; i++)
                {
                    texto[0] = texto[0] + "," + punto;
                    texto[1] = texto[1] + "," + tipos[i];
                    texto[2] = texto[2] + "," + entidad.EMPRNOMB;
                    texto[3] = texto[3] + "," + entidad.EQUINOMB;
                    texto[4] = texto[4] + "," + entidad.EQUIABREV;

                    List<MeMedicion60DTO> list = entitys.Where(x => x.Ptomedicodi == punto && x.Tipoinfocodi == tipos[i]).OrderBy(x => x.Fechahora).ToList();

                    int k = 6;
                    foreach (MeMedicion60DTO item in list)
                    {
                        for (int j = 0; j <= 59; j++)
                        {
                            if (t == 0 && i == 0)
                                texto[k] = item.Fechahora.AddSeconds(j).ToString("HH:mm:ss") + "," + item.GetType().GetProperty("H" + j.ToString()).GetValue(item, null);
                            else
                                texto[k] = texto[k] + "," + item.GetType().GetProperty("H" + j.ToString()).GetValue(item, null);
                            k++;
                        }
                    }
                }

                t++;
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                foreach (string item in texto)
                {
                    file.WriteLine(item);
                }
            }

        }

    }
}
