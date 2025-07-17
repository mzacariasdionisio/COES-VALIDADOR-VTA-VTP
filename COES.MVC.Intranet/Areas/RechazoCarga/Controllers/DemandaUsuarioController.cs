using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.Servicios.Aplicacion.RechazoCarga;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.MVC.Intranet.Models;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Net;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.DemandaMaxima.Helper;
using log4net;
using System.Reflection;
using FormatoHelper = COES.MVC.Intranet.Areas.RechazoCarga.Helper.FormatoHelper;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class DemandaUsuarioController : BaseController
    {
        //
        // GET: /RechazoCarga/DemandaUsuario/
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(DemandaUsuarioController));
        #region propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreArchivo
        {
            get
            {
                return (Session[DatosSesionRechazoCarga.SesionNombreArchivo] != null) ?
                    Session[DatosSesionRechazoCarga.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionRechazoCarga.SesionNombreArchivo] = value; }
        }


        /// <summary>
        /// Ruta y nombre del archivo
        /// </summary>
        public String RutaCompletaArchivo
        {
            get
            {
                return (Session[DatosSesionRechazoCarga.RutaCompletaArchivo] != null) ?
                    Session[DatosSesionRechazoCarga.RutaCompletaArchivo].ToString() : null;
            }
            set { Session[DatosSesionRechazoCarga.RutaCompletaArchivo] = value; }
        }

        #endregion
        public DemandaUsuarioController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("DemandaUsuarioController", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("DemandaUsuarioController", ex);
                throw;
            }
        }
        /// <summary>
        /// Carga principal de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DemandaUsuarioModel model = new DemandaUsuarioModel();
            
            model.ListaPeriodo = this.servicio.ListaPeriodoReporte(ConfigurationManager.AppSettings["FechaInicioDemandaUsuario"]);//Fecha de inicio del despliqgue de la aplicación

            List<RcaSuministradorDTO> suministradores = new List<RcaSuministradorDTO>();            
            var listaSuministradores = servicio.ListRcaSuministradores();
            model.Suministradores = listaSuministradores;

            var listaZonas = servicio.ListZonas(ConstantesRechazoCarga.CodigoAreaNivel);
            var zonas = new List<AreaDTO>();
            zonas.Add(new AreaDTO { AREACODI = 0, AREANOMB = "-- TODOS --" });
            if (listaZonas.Any())
            {
                zonas.AddRange(listaZonas);
            }
            model.Zonas = zonas;

            var subEstaciones = new List<AreaDTO>();
            var listaSubEstaciones = servicio.ListSubEstacion(0);
            if (listaSubEstaciones.Any())
            {
                subEstaciones.AddRange(listaSubEstaciones);
            }
            model.Subestaciones = subEstaciones;

            var nroRegistros = 0;
            if (model.ListaPeriodo.Count > 0)
            {
                var periodo = model.ListaPeriodo.First().Periodo;
                nroRegistros = this.servicio.ListDemandaUsuarioReporteCount(periodo, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            ViewBag.hfCantidadRegistros = nroRegistros;

            return View(model);
            
        }

        /// <summary>
        /// Metodo para obtener la lista de información
        /// </summary>
        /// <param name="nroPagina"></param>
        /// <param name="empresas"></param>
        /// <param name="tipos"></param>
        /// <param name="ini"></param>
        /// <param name="fin"></param>
        /// <param name="cumpli"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarReporteInformacion(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador, int nroPagina, int tipoBusqueda, int nroRegistros)
        {
            DemandaUsuarioModel model = new DemandaUsuarioModel();

            int regIni = 0;
            int regFin = 0;

            //regIni = (nroPagina - 1) * ConstantesRechazoCarga.PageSizeDemandaUsuario + 1;
            //regFin = nroPagina * ConstantesRechazoCarga.PageSizeDemandaUsuario;

            regIni = (nroPagina - 1) * nroRegistros + 1;
            regFin = nroPagina * nroRegistros;

            List<RcaDemandaUsuarioDTO> listReporteInformacion = null;

            if (tipoBusqueda > 0)
            {
                listReporteInformacion = this.servicio.ListDemandaUsuarioErroresPag(periodo, regIni, regFin);
            }
            else
            {
                if (codigoZona.Equals("0"))
                {
                    codigoPuntoMedicion = "";
                }
                listReporteInformacion = this.servicio.ListDemandaUsuarioReporte(periodo, codigoZona, codigoPuntoMedicion, empresa, suministrador, regIni, regFin);
            }

            


            model.ListaReporteInformacion15min = listReporteInformacion;
            model.registros = listReporteInformacion.Count().ToString();
            return View(model);
        }

        /// <summary>
        /// Permite generar la vista del paginado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador, int tipoBusqueda, int nroRegistrosPag)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = 0;
            
            //validacion registros
            if(tipoBusqueda > 0)
            {
                nroRegistros = this.servicio.ListDemandaUsuarioErroresExcel(periodo).Count;
            }
            else
            {
                if (codigoZona.Equals("0"))
                {
                    codigoPuntoMedicion = "";
                }

                nroRegistros = this.servicio.ListDemandaUsuarioReporteCount(periodo, codigoZona, codigoPuntoMedicion, empresa, suministrador);
            }
            

            if (nroRegistros > ConstantesRechazoCarga.NroPageShow)
            {
                //int pageSize = ConstantesRechazoCarga.PageSizeDemandaUsuario;
                int pageSize = nroRegistrosPag;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = ConstantesRechazoCarga.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);

        }

        [HttpPost]
        public JsonResult LeerExcelSubido(string periodo)
        {
            try
            {
                //var titulos = ObtenerTitulosColumnas(bloqueHorario);
                Respuesta matrizValida;
                var listaDemandaUsuario = FormatoHelper.LeerExcelCargadoDemandaUsuario(this.RutaCompletaArchivo, out matrizValida);
                if (matrizValida.Exito)
                {
                    //Se elimina los registros del periodo a cargar
                    this.servicio.DeleteRcaDemandaUsuario(periodo);
                    
                    //Se obtiene los datos de equicodi y empcodi
                    var listaEquipos = this.servicio.ObtenerEquipos().ToList();
                    
                    //Se obtiene el MaxId de la tabla RCA_DEMANDA_USUARIO
                    var idDemandaUsuario = this.servicio.ObtenerDemandaUsuarioMaximoId();
                    
                    //Se procede a hacer la actualizacion de cada registro en la tabla RCA_DEMANDA_USUARIO
                    var usuario = User.Identity.Name;
                    
                    foreach (var demanda in listaDemandaUsuario)
                    {
                        var equipo = listaEquipos.Where(p => p.Osinergcodi == demanda.Osinergcodi).ToList();
                        demanda.Rcdeulcodi = idDemandaUsuario;
                        demanda.Rcdeulperiodo = periodo;
                        demanda.Rcdeulfuente = "PR16";
                        demanda.Equicodi = equipo.Count > 0 ? equipo.First().Equicodi : 0;
                        demanda.Emprcodi = equipo.Count > 0 ? equipo.First().Emprcodi.Value : 0;
                        demanda.Rcdeulusucreacion = usuario;
                        demanda.Rcdeulfeccreacion = DateTime.Now;

                        this.servicio.SaveRcaDemandaUsuario(demanda);
                        idDemandaUsuario++;

                    }
                    
                    FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true });
                }
                else
                {
                    return Json(matrizValida);
                }
            }
            catch (Exception ex)
            {
                Log.Error("LeerExcelSubido", ex);
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Carga archivo excel con un nombre temporal en la ruta configurada en el Archivo de Configuración
        /// </summary>
        /// <returns></returns>
        public ActionResult Subir()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var archivo = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.NombreArchivo = fileRandom + "." + NombreArchivoRechazoCarga.ExtensionFileUploadRechazoCarga;
                    string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.NombreArchivo;
                    this.RutaCompletaArchivo = ruta;
                    archivo.SaveAs(ruta);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        //public PartialViewResult ListarSubestaciones(string codigoZona)
        //{
        //    DemandaUsuarioModel model = new DemandaUsuarioModel();
        //    model.Subestaciones = new List<AreaDTO>();
        //    //model.Subestaciones.Add(new AreaDTO { AREACODI = 0, AREANOMB = "" });
        //    //if (codigoZona.Equals("0"))
        //    //{
        //    //    return PartialView("ListarSubestaciones", model);
        //    //}
        //    var cuadrosFiltrados = servicio.ListSubEstacion(Convert.ToInt32(codigoZona));
        //    if (cuadrosFiltrados.Any())
        //    {
        //        model.Subestaciones.AddRange(cuadrosFiltrados);
        //    }
        //    return PartialView("ListarSubestaciones", model);
        //}

        public JsonResult ListarSubestaciones(string codigoZona)
        {
            DemandaUsuarioModel model = new DemandaUsuarioModel();
            model.Subestaciones = new List<AreaDTO>();
            //model.Subestaciones.Add(new AreaDTO { AREACODI = 0, AREANOMB = "" });
            //if (codigoZona.Equals("0"))
            //{
            //    return PartialView("ListarSubestaciones", model);
            //}
            var cuadrosFiltrados = servicio.ListSubEstacion(Convert.ToInt32(codigoZona));
            if (cuadrosFiltrados.Any())
            {
                model.Subestaciones.AddRange(cuadrosFiltrados);
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult ObtenerRegistrosPeriodo(string periodo)
        {
            int nroRegistros = 0;
            try
            {
                nroRegistros = this.servicio.ListDemandaUsuarioReporteCount(periodo, string.Empty, string.Empty, string.Empty, string.Empty);

                return Json(nroRegistros.ToString());
            }
            catch(Exception ex)
            {
                Log.Error("ObtenerRegistrosPeriodo", ex);
                return Json("-1");
            }
            
        }


        //Nuevos Metodos 09/02/2021
        public JsonResult GenerarReporte(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador, int tipoBusqueda)
        {
            string fileName = "";
            try
            {
                //FormatoModel model = FormatearModeloDescargaArchivo(bloqueHorario, datos);
                fileName = GenerarArchivoExcel(periodo, codigoZona, codigoPuntoMedicion, empresa, suministrador, tipoBusqueda);
                //indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error("GenerarReporte", ex);
                fileName = "-1";
            }

            return Json(fileName);
        }
        private string GenerarArchivoExcel(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador, int tipoBusqueda)
        {
            List<RcaDemandaUsuarioDTO> listReporteInformacion;
            var preNombre = "Reporte_Demanda_Usuario";

            if (tipoBusqueda > 0)
            {
                listReporteInformacion = this.servicio.ListDemandaUsuarioErroresExcel(periodo);
                preNombre = "Reporte_Demanda_Usuario_Errores" ;
            }
            else
            {
                if (codigoZona.Equals("0"))
                {
                    codigoPuntoMedicion = "";
                }

                listReporteInformacion = this.servicio.ListDemandaUsuarioReporteExcel(periodo, codigoZona, codigoPuntoMedicion, empresa, suministrador);
            }

            
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();

            var fileName = preNombre  + ".xlsx";

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
                
                ws.Cells[2, 1].Value = "PERIODO";
                ws.Cells[2, 2].Value = "CODIGO CLIENTE";
                ws.Cells[2, 3].Value = "SUMINISTRADOR";
                ws.Cells[2, 4].Value = "RUC";
                ws.Cells[2, 5].Value = "RAZON SOCIAL";
                ws.Cells[2, 6].Value = "SUB ESTACION";

                ws.Cells[2, 7].Value = "DEMANDA HP";
                ws.Cells[2, 8].Value = "DEMANDA HFP";
                for(int i = 1; i <= 96; i++)
                {
                    ws.Cells[2, 8 + i].Value = TimeSpan.FromMinutes(i * 15).ToString("hh':'mm");
                }

                ExcelRange rg1 = ws.Cells[2, 1, 2, 104];
                ObtenerEstiloCelda(rg1, 1);

                foreach (var registro in listReporteInformacion)
                {

                    ws.Cells[contFila, 1].Value = registro.Rcdeulperiodo;
                    ws.Cells[contFila, 2].Value = registro.Osinergcodi;
                    ws.Cells[contFila, 3].Value = registro.Suministrador;
                    ws.Cells[contFila, 4].Value = registro.Ruc;
                    ws.Cells[contFila, 5].Value = registro.Emprrazsocial;
                    ws.Cells[contFila, 6].Value = registro.Subestacion;
                    ws.Cells[contFila, 7].Value = registro.Rcdeuldemandahp;
                    ws.Cells[contFila, 8].Value = registro.Rcdeuldemandahfp;


                    for (int i = 1; i <= 96; i++)
                    {
                        ws.Cells[contFila, 8 + i].Value = registro.GetType().GetProperty("RCDEULH" + i).GetValue(registro, null);
                    }

                    contFila++;
                }

                ws.Column(1).Width = 20;
                ws.Column(2).Width = 20;
                ws.Column(3).Width = 50;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 50;
                ws.Column(6).Width = 50;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                for (int i = 1; i <= 96; i++)
                {
                    ws.Column(8 + i).Width = 10;
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

            return rango;
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato(string file, int tipoBusqueda)
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + file;

            string nombreArchivo = tipoBusqueda > 0 ? NombreArchivoRechazoCarga.DemandaUsuarioErrores : NombreArchivoRechazoCarga.DemandaUsuario;


            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
    }
}
