using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.RechazoCarga;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class ProgramaController : BaseController
    {
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        private const string estadoRegistroNoEliminado = "1";
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ProgramaController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ProgramaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ProgramaController", ex);
                throw;
            }
        }
        public ProgramaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public ActionResult Index()
        {
            ProgramaModel model = new ProgramaModel();

            List<RcaHorizonteProgDTO> listHorizonte = new List<RcaHorizonteProgDTO>();
            listHorizonte.Add(new RcaHorizonteProgDTO { Rchorpcodi = 0, Rchorpnombre = "-- TODOS --" });
            var listaProgramas = servicio.ListHorizonteProg();
            if (listaProgramas.Any())
            {
                listHorizonte.AddRange(listaProgramas);
            }
            bool AccesoAdicional = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Adicional);

            if (!AccesoAdicional)
            {
                listHorizonte = listHorizonte.Where(x => x.Rchorpcodi != ConstantesRechazoCarga.HorizonteReprograma).ToList();
            }
            model.Horizontes = listHorizonte;

            model.VerReprograma = AccesoAdicional;

            var semanaInicial = COES.Base.Tools.Util.GenerarNroSemana(DateTime.Now, FirstDayOfWeek.Saturday);

            model.SemanaActual = semanaInicial;

            return View(model);
        }

        public ActionResult EditPrograma()
        {

            base.ValidarSesionUsuario();
            ProgramaModel model = new ProgramaModel();

            List<RcaHorizonteProgDTO> listHorizonte = new List<RcaHorizonteProgDTO>();
            listHorizonte.Add(new RcaHorizonteProgDTO { Rchorpcodi = 0, Rchorpnombre = "-- SELECCIONE --" });
            var listaProgramas = servicio.ListHorizonteProg();
            if (listaProgramas.Any())
            {
                listHorizonte.AddRange(listaProgramas);
            }

            bool AccesoAdicional = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Adicional);

            if (!AccesoAdicional)
            {
                listHorizonte = listHorizonte.Where(x => x.Rchorpcodi != ConstantesRechazoCarga.HorizonteReprograma).ToList();
            }

            model.Horizontes = listHorizonte;
            
            var semanaInicial = COES.Base.Tools.Util.GenerarNroSemana(DateTime.Now, FirstDayOfWeek.Saturday);

            model.SemanaActual = semanaInicial;
            
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListarPrograma(string horizonte, string codigoPrograma, string estadoPrograma, int verReprograma)
        {
            ProgramaModel model = new ProgramaModel();
            //model.ListProgramas = new List<RcaProgramaDTO>();

            var reprograma = verReprograma > 0 ? ConstantesRechazoCarga.HorizonteReprograma : 0;
            model.ListProgramas = servicio.ListProgramaFiltro(Convert.ToInt16(horizonte), codigoPrograma, estadoPrograma, reprograma);
            //var siteRoot = Url.Content("~/");
            //model.urlDescarga = siteRoot + Constantes.RutaCarga;
            return PartialView("ListarPrograma", model);
        }

        public JsonResult ObtenerPrograma(int codigoPrograma)
        {
            RcaProgramaDTO oRcaCargaEsencialDTO = new RcaProgramaDTO();//servicio.GetDataByIdHoraOperacion(pecacodi, hopcodi);
            oRcaCargaEsencialDTO = servicio.GetByIdRcaPrograma(codigoPrograma);
            
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oRcaCargaEsencialDTO));
        }

        public ActionResult EliminarPrograma(int codigoPrograma)
        {
            var usuario = User.Identity.Name;

            this.servicio.DeleteRcaPrograma(codigoPrograma, usuario);

            return Json(new { success = true, message = "Ok" });
        }

        public ActionResult GuardarPrograma(int codPrograma, int horizonte, string programaAbrev, string fechaMensual, int semana, string fechaDiaria, 
            string codigoProgramaRef, bool esNuevo)
        {
            var codigoProgramaPadre = 0;
            if (!string.IsNullOrEmpty(codigoProgramaRef))
            {
                var programaPadre = servicio.GetByCriteriaRcaProgramas(codigoProgramaRef);
                if (programaPadre.Count > 0)
                {
                    codigoProgramaPadre = programaPadre.First().Rcprogcodi;
                }
            }
            
            //Validamos si el programa existe
            var programaExistente = servicio.GetByCriteriaRcaProgramas(programaAbrev);
            if (programaExistente.Count > 0)
            {
                return Json(new { success = false, message = "El codigo de Programa ya Existe. Revisar." });
            }

            RcaProgramaDTO rcaProgramaDTO = new RcaProgramaDTO();

            rcaProgramaDTO.Rchorpcodi = horizonte;
            rcaProgramaDTO.Rcprogabrev = programaAbrev;
            //rcaProgramaDTO.Rcprognombre = nombrePrograma;
            rcaProgramaDTO.Rcprogestado = "1";            
            rcaProgramaDTO.Rcprogestregistro = estadoRegistroNoEliminado;
            rcaProgramaDTO.Rcprogusucreacion = User.Identity.Name;
            rcaProgramaDTO.Rcprogfeccreacion = DateTime.Now;
            rcaProgramaDTO.Rcprogusumodificacion = User.Identity.Name;
            rcaProgramaDTO.Rcprogfecmodificacion = DateTime.Now;
            rcaProgramaDTO.Rcprogcodipadre = codigoProgramaPadre;

            if (horizonte.Equals(ConstantesRechazoCarga.HorizonteMensual) && !string.IsNullOrEmpty(fechaMensual))
            {
                var datos = fechaMensual.Split('-');
                var fechaInicio = new DateTime(Convert.ToInt32(datos[0]), Convert.ToInt16(datos[1]), 1);
                var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                rcaProgramaDTO.Rcprogfecinicio = fechaInicio;
                rcaProgramaDTO.Rcprogfecfin = fechaFin;
            }
            else if (horizonte.Equals(ConstantesRechazoCarga.HorizonteSemanal) && semana > 0)
            {
                var fechaSemanaInicial = COES.Base.Tools.Util.GenerarFecha(DateTime.Now.Year, semana, 0);
                var fechaSemanaFin = fechaSemanaInicial.AddDays(6);

                rcaProgramaDTO.Rcprogfecinicio = fechaSemanaInicial;
                rcaProgramaDTO.Rcprogfecfin = fechaSemanaFin;
            }
            else if ((horizonte.Equals(ConstantesRechazoCarga.HorizonteDiario) || horizonte.Equals(ConstantesRechazoCarga.HorizonteReprograma)) && !string.IsNullOrEmpty(fechaDiaria))
            {
                DateTime fecha = DateTime.ParseExact(fechaDiaria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                rcaProgramaDTO.Rcprogfecinicio = fecha;
                rcaProgramaDTO.Rcprogfecfin = fecha;
            }

            if (esNuevo)
            {
                this.servicio.SaveRcaPrograma(rcaProgramaDTO);
            }
            else
            {
                rcaProgramaDTO.Rcprogcodi = codPrograma;
                this.servicio.UpdateRcaPrograma(rcaProgramaDTO);
            }


            return Json(new { success = true, message = "Ok" });
        }

        public ActionResult GenerarCodigoPrograma(int horizonte, string fechaMensual, int semana, string fechaDiaria)
        {
            var codigoPrograma = string.Empty;

            codigoPrograma = GenerarCodPrograma(horizonte, fechaMensual, semana, fechaDiaria);

            return Json(new { success = true, message = codigoPrograma });
        }

        private string GenerarCodPrograma(int horizonte, string fechaMensual, int semana, string fechaDiaria)
        {
            var codigoPrograma = string.Empty;

            if (horizonte.Equals(ConstantesRechazoCarga.HorizonteMensual) && !string.IsNullOrEmpty(fechaMensual))
            {
                var datos = fechaMensual.Split('-');
                var fechaInicio = new DateTime(Convert.ToInt32(datos[0]), Convert.ToInt16(datos[1]), 1);

                codigoPrograma = "PRC-PMI-" + fechaInicio.ToString("yyyy-MM");
            }
            else if (horizonte.Equals(ConstantesRechazoCarga.HorizonteSemanal) && semana > 0)
            {
                codigoPrograma = "PRC-PSI-" + DateTime.Now.Year + "-" + semana.ToString("");
            }
            else if ((horizonte.Equals(ConstantesRechazoCarga.HorizonteDiario) || horizonte.Equals(ConstantesRechazoCarga.HorizonteReprograma)) && !string.IsNullOrEmpty(fechaDiaria))
            {
                DateTime fecha = DateTime.ParseExact(fechaDiaria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (horizonte.Equals(ConstantesRechazoCarga.HorizonteDiario))
                {
                    codigoPrograma = "PRC-PDI-" + fecha.ToString("yyyy-MM-dd");
                }
                else
                {
                    codigoPrograma = "PRC-RDO-" + fecha.ToString("yyyy-MM-dd");
                }
            }
            return codigoPrograma;
        }
        
        public JsonResult GenerarReporte(int codigoPrograma)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel(codigoPrograma);
                //indicador = 1;
            }
            catch(Exception ex)
            {
                log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoExcel(int codigoPrograma)
        {
            var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Anexo3_Restricciones_de_Suministros_";
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + programa.Rcprogabrev + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Obtenemos los cuadros asociados al programa
                var cuadroProgramas = servicio.GetByCriteriaRcaCuadroProgs(codigoPrograma.ToString(""), "");

                var contFila = 7;
                var contHojas = 0;
                foreach (var cuadroPrograma in cuadroProgramas)
                {
                    //Obtenemos el cuadro de Programa Usuario asociado al cuadro de programa
                    var cuadroProgramasUsuario = servicio.ListProgramaRechazoCarga("", cuadroPrograma.Rccuadcodi.ToString());
                    contHojas++;
                    var nombreHoja = !string.IsNullOrEmpty(cuadroPrograma.Rccuadubicacion) ? cuadroPrograma.Rccuadubicacion + "_" + contHojas : "REPORTE" + contHojas.ToString();
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                    contFila = 7;
                    var contItem = 1;

                    var bloqueHorario = cuadroPrograma.Rccuadbloquehor.Equals("HP") ? "HP (MW)" : "HFP (MW)";
                    ws.Cells[2, 2].Value = "MOTIVO";
                    ws.Cells[2, 3].Value = cuadroPrograma.Rccuadmotivo;
                    ws.Cells[3, 2].Value = "FECHA";
                    ws.Cells[3, 3].Value = cuadroPrograma.Rccuadfechorinicio.Value.ToString("dddd, dd MMMM yyyy",CultureInfo.CreateSpecificCulture("es-PE"));
                    ws.Cells[5, 2].Value = "PERIODO";
                    ws.Cells[5, 3].Value = cuadroPrograma.Rccuadfechorinicio.Value.ToString("HH:mm") + " a " + cuadroPrograma.Rccuadfechorfin.Value.ToString("HH:mm") + " (*)";

                    ws.Cells[contFila, 2].Value = "ITEM";
                    ws.Cells[contFila, 3].Value = "CLIENTE LIBRE";
                    ws.Cells[contFila, 4].Value = "SUMINISTRADOR";
                    ws.Cells[contFila, 5].Value = "SUBESTACION";
                    ws.Cells[contFila, 6].Value = "DEMANDA EN " + bloqueHorario;
                    ws.Cells[contFila, 7].Value = "RESTRICCIÓN EN " + bloqueHorario;
                    ws.Cells[contFila, 8].Value = "CARGA DEL USUARIO LUEGO DE APLICAR LA RESTRICCIÓN " + bloqueHorario;
                    ws.Cells[contFila, 9].Value = "CODIGO";

                    ExcelRange rg1 = ws.Cells[contFila, 2, contFila, 9];
                    ObtenerEstiloCelda(rg1, 1);                    

                    contFila++;
                    foreach (var registro in cuadroProgramasUsuario)
                    {
                        ws.Cells[contFila, 2].Value = contItem;
                        ws.Cells[contFila, 3].Value = registro.Empresa;
                        ws.Cells[contFila, 4].Value = registro.Suministrador;
                        ws.Cells[contFila, 5].Value = registro.Subestacion;
                        ws.Cells[contFila, 6].Value = registro.Rcproudemandareal;
                        ws.Cells[contFila, 7].Value = registro.Rcproudemandaracionar;
                        ws.Cells[contFila, 8].Value = registro.Rcproudemandareal - registro.Rcproudemandaracionar;
                        ws.Cells[contFila, 9].Value = string.IsNullOrEmpty(registro.Osinergcodi) ? "" : registro.Osinergcodi;

                        contFila++;
                        contItem++;
                    }

                    ws.Cells[contFila, 6].Value = cuadroProgramasUsuario.Sum(p => p.Rcproudemandareal);
                    ws.Cells[contFila, 7].Value = cuadroProgramasUsuario.Sum(p => p.Rcproudemandaracionar);
                    ws.Cells[contFila, 8].Value = cuadroProgramasUsuario.Sum(p => p.Rcproudemandareal) - cuadroProgramasUsuario.Sum(p => p.Rcproudemandaracionar);

                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 50;
                    ws.Column(4).Width = 50;
                    ws.Column(5).Width = 50;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 30;
                    ws.Column(9).Width = 20;
                    
                }

                if (xlPackage.Workbook.Worksheets.Count == 0)
                {
                    xlPackage.Workbook.Worksheets.Add("Cuadro");
                }

                xlPackage.Save();
            }

            return fileName;
        }

        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                //rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;
                string colorborder = "#245C86";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(Color.DarkSlateBlue);
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                string colorborder = "#DADAD9";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato(string file)
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, file);
        }

        public ActionResult GenerarCopiaPrograma(int codigoProgramaDuplicar, int horizonte, string fechaMensual, string fechaDiaria, int semana)
        {
            try
            {
                #region Nuevo Programa

                //1. Validacion si el programa ya existe--consultar con Miguel si es necesario la validacion
                var programaAbrev = GenerarCodPrograma(horizonte, fechaMensual, semana, fechaDiaria);

                if (!string.IsNullOrEmpty(programaAbrev))
                {
                    var programaExistente = servicio.GetByCriteriaRcaProgramas(programaAbrev);
                    if (programaExistente.Count > 0)
                    {
                        return Json(new { success = false, message = "El codigo de Programa ya Existe. Revisar." });
                    }
                }

                //2. Creacion del programa
                RcaProgramaDTO rcaProgramaDTO = new RcaProgramaDTO();

                rcaProgramaDTO.Rchorpcodi = horizonte;
                rcaProgramaDTO.Rcprogabrev = programaAbrev;
                //rcaProgramaDTO.Rcprognombre = nombrePrograma;
                rcaProgramaDTO.Rcprogestado = "1";
                rcaProgramaDTO.Rcprogestregistro = estadoRegistroNoEliminado;
                rcaProgramaDTO.Rcprogusucreacion = User.Identity.Name;
                rcaProgramaDTO.Rcprogfeccreacion = DateTime.Now;
                rcaProgramaDTO.Rcprogusumodificacion = User.Identity.Name;
                rcaProgramaDTO.Rcprogfecmodificacion = DateTime.Now;
                rcaProgramaDTO.Rcprogcodipadre = 0;

                if (horizonte.Equals(ConstantesRechazoCarga.HorizonteMensual) && !string.IsNullOrEmpty(fechaMensual))
                {
                    var datos = fechaMensual.Split('-');
                    var fechaInicio = new DateTime(Convert.ToInt32(datos[0]), Convert.ToInt16(datos[1]), 1);
                    var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                    rcaProgramaDTO.Rcprogfecinicio = fechaInicio;
                    rcaProgramaDTO.Rcprogfecfin = fechaFin;
                }
                else if (horizonte.Equals(ConstantesRechazoCarga.HorizonteSemanal) && semana > 0)
                {
                    var fechaSemanaInicial = COES.Base.Tools.Util.GenerarFecha(DateTime.Now.Year, semana, 0);
                    var fechaSemanaFin = fechaSemanaInicial.AddDays(6);

                    rcaProgramaDTO.Rcprogfecinicio = fechaSemanaInicial;
                    rcaProgramaDTO.Rcprogfecfin = fechaSemanaFin;
                }
                else if ((horizonte.Equals(ConstantesRechazoCarga.HorizonteDiario) || horizonte.Equals(ConstantesRechazoCarga.HorizonteReprograma)) && !string.IsNullOrEmpty(fechaDiaria))
                {
                    DateTime fecha = DateTime.ParseExact(fechaDiaria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    rcaProgramaDTO.Rcprogfecinicio = fecha;
                    rcaProgramaDTO.Rcprogfecfin = fecha;
                }

                var nuevoCodigoPrograma = servicio.SaveRcaPrograma(rcaProgramaDTO);


                var listaCuadroProgramas = servicio.GetByCriteriaRcaCuadroProgs(codigoProgramaDuplicar.ToString(), "");
                //var listaCuadroProgamaUsuario = servicio.ListProgramaRechazoCarga("", codigoCuadroPrograma.ToString());

                //3. Creamos los nuevos cuadros asociados al programa creado
                foreach(var cuadroPrograma in listaCuadroProgramas)
                {
                    cuadroPrograma.Rcprogcodi = nuevoCodigoPrograma;
                    //cuadroPrograma.Rccuadfechorinicio = DateTime.ParseExact(fechaHoraInicio, "dd/MM/yyyy HH:mm", null);
                    //cuadroPrograma.Rccuadfechorfin = DateTime.ParseExact(fechaHoraFin, "dd/MM/yyyy HH:mm", null);
                    cuadroPrograma.Rcestacodi = ConstantesRechazoCarga.EstadoCuadroProgramado;

                    var listaCuadroProgamaUsuario = servicio.ListProgramaRechazoCarga("", cuadroPrograma.Rccuadcodi.ToString());

                    var nuevoCodigoCuadroPrograma = servicio.SaveRcaCuadroProg(cuadroPrograma);

                    //4. Creamos los cuadros de Programa de Usuario asociado al nuevo programa y nuevo cuadro de programa

                    foreach (var registro in listaCuadroProgamaUsuario)
                    {
                        registro.Rccuadcodi = nuevoCodigoCuadroPrograma;
                        registro.Rcprouusucreacion = User.Identity.Name;
                        registro.Rcproufeccreacion = DateTime.Now;

                        //servicio.SaveRcaCuadroProgUsuario(registro);
                    }
                }

                #endregion


                return Json(new { success = true, message = "Ok" });
            }
            catch (Exception ex)
            {
                log.Error("ProgramaController", ex);

                return Json(new { success = false, message = "Ocurrio un error al duplicar el cuadro." });
            }

        }


        //Nuevos Metodos 10/02/2021
        public JsonResult GenerarReporteLista(string horizonte, string codigoPrograma, string estadoPrograma, int verReprograma)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcelLista(horizonte, codigoPrograma, estadoPrograma, verReprograma);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoExcelLista(string horizonte, string codigoPrograma, string estadoPrograma, int verReprograma)
        {

            var preNombre = "Programas_" + DateTime.Now.ToString("yyyyMMddhhmmss");

            var reprograma = verReprograma > 0 ? ConstantesRechazoCarga.HorizonteReprograma : 0;
            var listReporteInformacion = servicio.ListProgramaFiltro(Convert.ToInt16(horizonte), codigoPrograma, estadoPrograma, reprograma);
                        
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {

                var nombreHoja = "REPORTE";
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                var contFila = 3;

                ws.Cells[2, 1].Value = "Horizonte";
                ws.Cells[2, 2].Value = "Código Programa";
                ws.Cells[2, 3].Value = "Nro Cuadros";
                ws.Cells[2, 4].Value = "Estado";
                //ws.Cells[2, 5].Value = "DOCUMENTO";
                //ws.Cells[2, 6].Value = "FECHA PRESENTACION AL COES";
                //ws.Cells[2, 7].Value = "ESTADO";
                //ws.Cells[2, 8].Value = "CARGA ESENCIAL";

                ExcelRange rg1 = ws.Cells[2, 1, 2, 4];
                ObtenerEstiloCelda(rg1, 1);

                foreach (var registro in listReporteInformacion)
                {

                    ws.Cells[contFila, 1].Value = registro.Rcproghorizonte;
                    ws.Cells[contFila, 2].Value = registro.Rcprogabrev;
                    ws.Cells[contFila, 3].Value = registro.Nrocuadros;
                    ws.Cells[contFila, 4].Value = (!string.IsNullOrEmpty(registro.Rcprogestado) && registro.Rcprogestado.Equals("1")) ? "Vigente" : "No Vigente";                   

                    contFila++;
                }

                ws.Column(1).Width = 25;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                //ws.Column(5).Width = 40;
                //ws.Column(6).Width = 40;
                //ws.Column(7).Width = 20;
                //ws.Column(8).Width = 20;

                xlPackage.Save();
            }

            return fileName;
        }


        //Generar Reporte Ejecutados
        public JsonResult GenerarReporteEjecutados(int codigoPrograma)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcelEjecutados(codigoPrograma);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }

        private string GenerarArchivoExcelEjecutados(int codigoPrograma)
        {
            var programa = servicio.GetByIdRcaPrograma(codigoPrograma);
            var preNombre = "Anexo3_Restricciones_de_Suministros_Ejecutados_";
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre + programa.Rcprogabrev + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + fileName);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Obtenemos los cuadros ejecutados asociados al programa
                var cuadroProgramas = servicio.GetByCriteriaRcaCuadroProgs(codigoPrograma.ToString(""), ConstantesRechazoCarga.EstadoCuadroEjecutado.ToString());

                var contFila = 7;
                var contHojas = 0;
                foreach (var cuadroPrograma in cuadroProgramas)
                {
                    //Obtenemos el cuadro de Programa Usuario asociado al cuadro de programa
                    var cuadroProgramasUsuario = servicio.ListProgramaRechazoCarga("", cuadroPrograma.Rccuadcodi.ToString());
                    contHojas++;
                    var nombreHoja = !string.IsNullOrEmpty(cuadroPrograma.Rccuadubicacion) ? cuadroPrograma.Rccuadubicacion + "_" + contHojas : "REPORTE" + contHojas.ToString();
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                    contFila = 9;
                    //var contItem = 1;

                    var bloqueHorario = cuadroPrograma.Rccuadbloquehor.Equals("HP") ? "HP (MW)" : "HFP (MW)";
                    ws.Cells[2, 2].Value = "MOTIVO";
                    ws.Cells[2, 3].Value = cuadroPrograma.Rccuadmotivo;
                    ws.Cells[3, 2].Value = "FECHA";
                    ws.Cells[3, 3].Value = cuadroPrograma.Rccuadfechorinicio.Value.ToString("dd/MM/yyyy");
                    ws.Cells[5, 2].Value = "PERIODO";
                    ws.Cells[5, 3].Value = cuadroPrograma.Rccuadfechorinicio.Value.ToString("HH:mm") + " - " + cuadroPrograma.Rccuadfechorfin.Value.ToString("HH:mm");

                    ws.Cells[contFila, 2].Value = "Razón Social";
                    ws.Cells[contFila, 3].Value = "Suministrador";
                    ws.Cells[contFila, 4].Value = "Subestación";
                    ws.Cells[contFila, 5].Value = "Nombre punto Medición";
                    ws.Cells[contFila, 6].Value = "Demanda en " + bloqueHorario;
                    ws.Cells[contFila, 6].Style.WrapText = true;
                    ws.Cells[contFila, 7].Value = "Racionamiento";
                    ws.Cells[contFila, 8].Value = "Carga Luego de aplicar restricción";
                    ws.Cells[contFila, 8].Style.WrapText = true;
                    ws.Cells[contFila, 9].Value = "Carga Rechazada";
                    ws.Cells[contFila, 10].Value = "Hora de Inicio";
                    ws.Cells[contFila, 11].Value = "Hora Fin";
                    ws.Cells[contFila, 12].Value = "Carga Rechazada";
                    ws.Cells[contFila, 13].Value = "Hora de Inicio";
                    ws.Cells[contFila, 14].Value = "Hora Fin";

                    ws.Cells[contFila - 1, 7].Value = "Programado";
                    ws.Cells[contFila - 1, 9].Value = "Coordinado";
                    ws.Cells[contFila - 1, 12].Value = "Ejecutado";

                   

                    ExcelRange rg1 = ws.Cells[contFila, 2, contFila, 14];
                    ObtenerEstiloCelda(rg1, 2);

                    rg1 = ws.Cells[contFila - 1, 7, contFila - 1, 14];
                    ObtenerEstiloCelda(rg1, 2);

                    ws.Cells[contFila - 1, 7, contFila - 1, 8].Merge = true;
                    ws.Cells[contFila - 1, 9, contFila - 1, 11].Merge = true;
                    ws.Cells[contFila - 1, 12, contFila - 1, 14].Merge = true;

                    contFila++;
                    var filaInicio = contFila;
                    foreach (var registro in cuadroProgramasUsuario)
                    {
                        
                        ws.Cells[contFila, 2].Value = registro.Empresa;
                        ws.Cells[contFila, 3].Value = registro.Suministrador;
                        ws.Cells[contFila, 4].Value = registro.Subestacion;
                        ws.Cells[contFila, 5].Value = registro.Puntomedicion;
                        ws.Cells[contFila, 6].Value = registro.Rcproudemandareal;
                        ws.Cells[contFila, 7].Value = registro.Rcproudemandaracionar;
                        ws.Cells[contFila, 7].Style.Numberformat.Format = @"0.00";
                        ws.Cells[contFila, 8].Value = registro.Rcproudemandaatender;
                        ws.Cells[contFila, 8].Style.Numberformat.Format = @"0.00";

                        ws.Cells[contFila, 9].Value = registro.Rcproucargarechazarcoord;
                        ws.Cells[contFila, 9].Style.Numberformat.Format = @"0.00";
                        ws.Cells[contFila, 10].Value = registro.Rccuadhorinicoord;
                        ws.Cells[contFila, 10].Style.Numberformat.Format = "HH:mm";
                        ws.Cells[contFila, 11].Value = registro.Rccuadhorfincoord;
                        ws.Cells[contFila, 11].Style.Numberformat.Format = "HH:mm";
                        if(registro.Rcproucargarechazarejec > 0)
                        {
                            ws.Cells[contFila, 12].Value = registro.Rcproucargarechazarejec;
                        }
                        
                        ws.Cells[contFila, 12].Style.Numberformat.Format = @"0.00";
                        ws.Cells[contFila, 13].Value = registro.Rccuadhoriniejec;
                        ws.Cells[contFila, 13].Style.Numberformat.Format = "HH:mm";
                        ws.Cells[contFila, 14].Value = registro.Rccuadhorfinejec;
                        ws.Cells[contFila, 14].Style.Numberformat.Format = "HH:mm";

                        contFila++;
                    }

                    rg1= ws.Cells[filaInicio, 2, contFila - 1, 14];
                    ObtenerEstiloCelda(rg1, 0);

                    ws.Cells[contFila, 6].Value = cuadroProgramasUsuario.Sum(p => p.Rcproudemandareal);
                    ws.Cells[contFila, 7].Value = cuadroProgramasUsuario.Sum(p => p.Rcproudemandaracionar);
                    //ws.Cells[contFila, 8].Value = cuadroProgramasUsuario.Sum(p => p.Rcproudemanda) - cuadroProgramasUsuario.Sum(p => p.Rcproudemandaracionar);

                    ws.Cells[contFila, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[contFila, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells[contFila, 6].Style.Font.Size = 10;
                    ws.Cells[contFila, 6].Style.Font.Bold = true;
                    ws.Cells[contFila, 7].Style.Font.Size = 10;
                    ws.Cells[contFila, 7].Style.Font.Bold = true;

                    ws.Column(2).Width = 50;
                    ws.Column(3).Width = 50;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 15;
                    ws.Column(13).Width = 15;
                    ws.Column(14).Width = 15;

                }

                if (xlPackage.Workbook.Worksheets.Count == 0)
                {
                    //xlPackage.Workbook.Worksheets.Add("Cuadro");

                    var nombreHoja = "REPORTE";
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);

                    contFila = 9;
                    //var contItem = 1;

                    var bloqueHorario =  "HP (MW)";
                    ws.Cells[2, 2].Value = "MOTIVO";
                    //ws.Cells[2, 3].Value = cuadroPrograma.Rccuadmotivo;
                    ws.Cells[3, 2].Value = "FECHA";
                    //ws.Cells[3, 3].Value = cuadroPrograma.Rccuadfechorinicio.Value.ToString("dd/MM/yyyy");
                    ws.Cells[5, 2].Value = "PERIODO";
                    //ws.Cells[5, 3].Value = cuadroPrograma.Rccuadfechorinicio.Value.ToString("HH:mm") + " - " + cuadroPrograma.Rccuadfechorfin.Value.ToString("HH:mm");

                    ws.Cells[contFila, 2].Value = "Razón Social";
                    ws.Cells[contFila, 3].Value = "Suministrador";
                    ws.Cells[contFila, 4].Value = "Subestación";
                    ws.Cells[contFila, 5].Value = "Nombre punto Medición";
                    ws.Cells[contFila, 6].Value = "Demanda en " + bloqueHorario;
                    ws.Cells[contFila, 6].Style.WrapText = true;
                    ws.Cells[contFila, 7].Value = "Racionamiento";
                    ws.Cells[contFila, 8].Value = "Carga Luego de aplicar restricción";
                    ws.Cells[contFila, 8].Style.WrapText = true;
                    ws.Cells[contFila, 9].Value = "Carga Rechazada";
                    ws.Cells[contFila, 10].Value = "Hora de Inicio";
                    ws.Cells[contFila, 11].Value = "Hora Fin";
                    ws.Cells[contFila, 12].Value = "Carga Rechazada";
                    ws.Cells[contFila, 13].Value = "Hora de Inicio";
                    ws.Cells[contFila, 14].Value = "Hora Fin";

                    ws.Cells[contFila - 1, 7].Value = "Programado";
                    ws.Cells[contFila - 1, 9].Value = "Coordinado";
                    ws.Cells[contFila - 1, 12].Value = "Ejecutado";



                    ExcelRange rg1 = ws.Cells[contFila, 2, contFila, 14];
                    ObtenerEstiloCelda(rg1, 2);

                    rg1 = ws.Cells[contFila - 1, 7, contFila - 1, 14];
                    ObtenerEstiloCelda(rg1, 2);

                    ws.Cells[contFila - 1, 7, contFila - 1, 8].Merge = true;
                    ws.Cells[contFila - 1, 9, contFila - 1, 11].Merge = true;
                    ws.Cells[contFila - 1, 12, contFila - 1, 14].Merge = true;

                    ws.Column(2).Width = 50;
                    ws.Column(3).Width = 50;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 15;
                    ws.Column(13).Width = 15;
                    ws.Column(14).Width = 15;
                }

                xlPackage.Save();
            }

            return fileName;
        }
    }
}
