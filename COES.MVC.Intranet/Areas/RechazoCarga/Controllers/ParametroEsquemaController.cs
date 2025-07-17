using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RechazoCarga.Helper;
using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.RechazoCarga;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using FormatoHelper = COES.MVC.Intranet.Areas.RechazoCarga.Helper.FormatoHelper;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Controllers
{
    public class ParametroEsquemaController : BaseController
    {
        RechazoCargaAppServicio servicio = new RechazoCargaAppServicio();
        FormatoMedicionAppServicio formatoMedicion = new FormatoMedicionAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ParametroEsquemaController));

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ParametroEsquemaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ParametroEsquemaController", ex);
                throw;
            }
        }
        public ParametroEsquemaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public ActionResult Consulta()
        {
            ParametroEsquemaModel model = new ParametroEsquemaModel();
            model.Anios = ObtenerListaAnios();
            model.Tipos = servicio.ListSiTipoEmpresa();
            return View(model);
        }

        /// <summary>
        /// Devuelve Lista de Años para los parámetros esquema
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> ObtenerListaAnios()
        {
            var anios = servicio.ListarAniosParametroEsquema();
            var lista = new List<SelectListItem>();
            anios.ForEach(x =>
            {
                lista.Add(new SelectListItem { Value = x.ToString(), Text = x.ToString() });
            });
            return lista;
        }

        [HttpPost]
        public JsonResult ObtenerFormatoModelParametrosEsquema(string anio, string tipoEmpresa)
        {
            ParametroEsquemaModel modeloParametroEsquema = new ParametroEsquemaModel();
            FormatoModel model = new FormatoModel();

            if(tipoEmpresa.Trim().Length > 0)
            {
                var cadenaBusqueda = new System.Text.StringBuilder();
                cadenaBusqueda.Append(tipoEmpresa);

                tipoEmpresa = FormatoHelper.EncodeNombreEmpresa(cadenaBusqueda);
            }

            var registros = servicio.ListarRcaParamEsquemaPorFiltros(anio, tipoEmpresa);
            //temporal
            //var registrosPrueba = new List<RcaParamEsquemaDTO>();
            //for(int i = 0; i < 2; i++)
            //{
            //    registrosPrueba.Add(registros[i]);
            //}
            //registros = registrosPrueba;

            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            model.Handson.ListaMerge = GenerarMerges();
            model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 2;
            model.Formato.Formatcols = 10;
            model.FilasCabecera = model.Formato.Formatrows;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.Handson.ListaColWidth = new List<int> { 250, 200, 220, 70, 70, 70, 70, 70, 70, 70, 70, 130 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true, true };
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            model.Handson.ListaExcelData = GenerarData(registros);

            modeloParametroEsquema.FormatoHandsonTable = model;
            modeloParametroEsquema.ListaTipoInstancia = GenerarTipoInstancia();

            return Json(modeloParametroEsquema);
            //return Json(model);
        }

        /// <summary>
        /// Devuelve la configuración de las areas que se van a unir en el Handsontable
        /// </summary>
        /// <returns></returns>
        private List<CeldaMerge> GenerarMerges()
        {
            return new List<CeldaMerge>{
               new CeldaMerge{row=0, col=3, colspan=4, rowspan=1},
               new CeldaMerge{row=0, col=7, colspan=2, rowspan=1},
               new CeldaMerge{row=0, col=9, colspan=2, rowspan=1},
               new CeldaMerge{row=1, col=3, colspan=2, rowspan=1},
               new CeldaMerge{row=1, col=5, colspan=2, rowspan=1},              
           };
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga] + NombreArchivoRechazoCarga.ParamEsquema;
            return File(fullPath, Constantes.AppExcel, NombreArchivoRechazoCarga.ParamEsquema);
        }

        /// <summary>
        /// Devuelve la lista de los títulos de la cabecera
        /// </summary>
        /// <returns></returns>
        private List<string> ObtenerTitulosColumnas()
        {
            return new List<string>() { "Razón Social", "Subestación", "Nombre Punto Medición", "ERACMF", "ERACMT", "ERACMF", "ERACMT", "ERACMF", "ERACMF", 
                "", "","","PARECODI", "EMPRCODI", "EQUICODI" };
        }

        /// <summary>
        /// Devuelve la lista de parametros esquema en una matriz
        /// </summary>
        /// <param name="registros"></param>
        /// <returns></returns>
        private string[][] GenerarData(List<RcaParamEsquemaDTO> registros)
        {
            var filas = registros.Count + 3;
            var cabecera = ObtenerTitulosColumnas();
            var columnas = cabecera.Count;
            string[][] matriz = new string[filas][];

            for (var i = 0; i < filas; i++)
            {
                matriz[i] = new string[columnas];
                if (i == 0)
                {
                    matriz[i][3] = "Primer Esquema";
                    matriz[i][7] = "Segundo Esquema";
                    matriz[i][9] = "Demanda de Referencia";
                }
                if (i == 1)
                {
                    matriz[i][3] = "HP";
                    matriz[i][5] = "HFP";
                    matriz[i][7] = "HP";
                    matriz[i][8] = "HFP";
                    matriz[i][9] = "HP";
                    matriz[i][10] = "HFP";
                }
                if (i == 2)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        matriz[i][j] = cabecera[j];
                    }
                }
                if (i > 2)
                {
                    matriz[i][0] = registros[i - 3].Emprrazsocial;
                    matriz[i][1] = registros[i - 3].Areanomb;
                    matriz[i][2] = registros[i - 3].Equinomb;
                    matriz[i][3] = registros[i - 3].Rcparehperacmf.ToString();
                    matriz[i][4] = registros[i - 3].Rcparehperacmt.ToString();
                    matriz[i][5] = registros[i - 3].Rcparehfperacmf.ToString();
                    matriz[i][6] = registros[i - 3].Rcparehfperacmt.ToString();
                    
                    matriz[i][7] = registros[i - 3].Rcparehperacmf2.ToString();
                    matriz[i][8] = registros[i - 3].Rcparehfperacmf2.ToString();

                    matriz[i][9] = registros[i - 3].Rcparehpdemandaref.ToString();
                    matriz[i][10] = registros[i - 3].Rcparehfpdemandaref.ToString();

                    matriz[i][11] = registros[i - 3].Rcparenroesquema > 0 ? registros[i - 3].Rcparenroesquema.ToString() : "";

                    matriz[i][12] = registros[i - 3].Rcparecodi.ToString();
                    matriz[i][13] = registros[i - 3].Emprcodi.ToString();
                    matriz[i][14] = registros[i - 3].Equicodi.ToString();
                }
            }
            return matriz;
        }

        [HttpPost]
        public JsonResult GrabarParametrosEsquema(string anio, string datos)
        {
            var parametros = FormatearParametrosEsquema(datos, anio);
            
            //Grabar o Actualizar
            foreach (var parametro in parametros)
            {
                if (parametro.Rcparecodi > 0)
                {
                    parametro.Rcparefecmodificacion = DateTime.Now;
                    parametro.Rcpareusumodificacion = User.Identity.Name;
                    parametro.Rcpareestregistro = "1";//Temporal - revisar
                    //parametro.Emprcodi = parametro.Emprcodi == 0 ? 10614 : parametro.Emprcodi;
                    //parametro.Equicodi = parametro.Equicodi == 0 ? 17547 : parametro.Equicodi;

                    servicio.UpdateRcaParamEsquema(parametro);
                }
                else
                {
                    parametro.Rcparefeccreacion = DateTime.Now;
                    parametro.Rcpareusucreacion = User.Identity.Name;
                    parametro.Rcpareestregistro = "1";//Temporal - revisar
                    parametro.Rcpareanio = Convert.ToInt32(anio);

                    servicio.SaveRcaParamEsquema(parametro);
                }
            }

            return Json(true);
        }

        /// <summary>
        /// Transforma el json de los datos en una lista de RcaParamEsquemaDTO
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        private ICollection<RcaParamEsquemaDTO> FormatearParametrosEsquema(string datos, string anio)
        {
            var filasCabecera = 3;
            var columnas = 15;
            var celdas = datos.Split(',');
            var parametros = new List<RcaParamEsquemaDTO>();
            var inicioDatos = filasCabecera * columnas;
            //var prueba = Json(datos);
            for (var i = inicioDatos; i < celdas.Count(); i += columnas)
            {
                var parametro = new RcaParamEsquemaDTO();
                parametro.Emprrazsocial = celdas[i].Replace(@"\","").Replace("\"","").Replace("[","");
                parametro.Areanomb = celdas[i + 1].Replace(@"\", "").Replace("\"", "");
                parametro.Equinomb = celdas[i + 2].Replace(@"\", "").Replace("\"", "");
                if(!string.IsNullOrEmpty(celdas[i + 3].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparehperacmf =  Convert.ToDecimal(celdas[i + 3].Replace(@"\", "").Replace("\"", ""));
                }

                if (!string.IsNullOrEmpty(celdas[i + 4].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparehperacmt = Convert.ToDecimal(celdas[i + 4].Replace(@"\", "").Replace("\"", ""));
                }
                if (!string.IsNullOrEmpty(celdas[i + 5].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparehfperacmf = Convert.ToDecimal(celdas[i + 5].Replace(@"\", "").Replace("\"", ""));
                }
                if (!string.IsNullOrEmpty(celdas[i + 6].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparehfperacmt = Convert.ToDecimal(celdas[i + 6].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                }

                if (!string.IsNullOrEmpty(celdas[i + 7].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparehperacmf2 = Convert.ToDecimal(celdas[i + 7].Replace(@"\", "").Replace("\"", ""));
                }

                if (!string.IsNullOrEmpty(celdas[i + 8].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparehfperacmf2 = Convert.ToDecimal(celdas[i + 8].Replace(@"\", "").Replace("\"", ""));
                }

                if (!string.IsNullOrEmpty(celdas[i + 9].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparehpdemandaref = Convert.ToDecimal(celdas[i + 9].Replace(@"\", "").Replace("\"", ""));
                }

                if (!string.IsNullOrEmpty(celdas[i + 10].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparehfpdemandaref = Convert.ToDecimal(celdas[i + 10].Replace(@"\", "").Replace("\"", ""));
                }

                if (!string.IsNullOrEmpty(celdas[i + 11].Replace(@"\", "").Replace("\"", "").Replace("null", "")))
                {
                    parametro.Rcparenroesquema = Convert.ToInt32(celdas[i + 11].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                }
                

                parametro.Rcparecodi = Convert.ToInt32(celdas[i + 12].Replace(@"\", "").Replace("\"", "").Replace("]", ""));               
                parametro.Emprcodi = Convert.ToInt32(celdas[i + 13].Replace(@"\", "").Replace("\"", "").Replace("]", ""));
                parametro.Equicodi = Convert.ToInt32(celdas[i + 14].Replace(@"\", "").Replace("\"", "").Replace("]", ""));

                parametros.Add(parametro);
            }
            return parametros;
        }

        [HttpPost]
        public JsonResult GenerarFormato(int anio, string tipoEmpresa, bool esConsulta)
        {
            int indicador = 0;
            try
            {
                FormatoModel model = FormatearModeloDesdeParametros(anio, tipoEmpresa, esConsulta);
                GenerarArchivoExcel(model, 3);
                indicador = 1;
            }
            catch(Exception ex)
            {
                log.Error("GenerarFormato", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Busca los registros de parámetros usando parametros de entrada 
        /// y devuelve el modelo para graficar los datos en Handsontable
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="esConsulta"></param>
        /// <returns></returns>
        private FormatoModel FormatearModeloDesdeParametros(int anio, string tipoEmpresa, bool esConsulta)
        {
            FormatoModel model = new FormatoModel();


            var registros = new List<RcaParamEsquemaDTO>();
            if (esConsulta)
            {
                registros = servicio.ListarRcaParamEsquemaPorFiltros(anio.ToString(), tipoEmpresa);
            }
            
            ConfigurarFormatoModelo(model);
            registros.ForEach(x => model.Handson.ListaFilaReadOnly.Add(false));
            model.Handson.ListaExcelData = GenerarData(registros);
            return model;
        }

        /// <summary>
        /// Configura el modelo con la estructura particular del Handsontable
        /// </summary>
        /// <param name="model"></param>
        private void ConfigurarFormatoModelo(FormatoModel model)
        {
            model.Handson = new HandsonModel();
            model.Formato = new MeFormatoDTO();
            model.Handson.ListaMerge = GenerarMerges();
            model.Handson.ListaColWidth = new List<int>();
            model.Formato.Formatrows = 2;
            model.Formato.Formatcols = 12;
            model.FilasCabecera = model.Formato.Formatrows;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.Handson.ListaColWidth = new List<int> { 250, 200, 220, 70, 70, 70, 70, 70, 70, 70, 70, 130 };
            model.Handson.ReadOnly = false;
            model.Handson.ListaFilaReadOnly = new List<bool> { true, true };
        }

        /// <summary>
        /// Genera el documento excel en la ruta configurada en el Archivo de configuración 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="columnasOcultas"></param>
        private void GenerarArchivoExcel(FormatoModel model, int columnasOcultas)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteRechazoCarga].ToString();
            FileInfo newFile = new FileInfo(ruta + NombreArchivoRechazoCarga.ParamEsquema);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivoRechazoCarga.ParamEsquema);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaFormatoExcel);
                //var filas = model.Handson.ListaFilaReadOnly.Count;
                var filas = model.Handson.ListaExcelData.Count();
                var columnas = model.ColumnasCabecera + columnasOcultas;
                for (var i = 0; i < filas; i++)
                {
                    for (var j = 0; j < columnas; j++)
                    {
                        decimal valor = 0;
                        bool esDecimal = decimal.TryParse(model.Handson.ListaExcelData[i][j], out valor);
                        if (esDecimal)
                        {
                            ws.Cells[i + 1, j + 1].Value = valor;
                        }
                        else
                        {
                            ws.Cells[i + 1, j + 1].Value = model.Handson.ListaExcelData[i][j] ?? string.Empty;
                        }

                        ws.Cells[i + 1, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        if (i == 2 || (i == 0 && j > 2) || (i == 1 && j > 2))
                        {
                            ws.Cells[i + 1, j + 1].Style.WrapText = true;
                            ws.Cells[i + 1, j + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells[i + 1, j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[i + 1, j + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkSlateBlue);
                            ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[i + 1, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        if (i > 2)
                        {
                            ws.Cells[i + 1, j + 1].Style.WrapText = false;
                            ws.Cells[i + 1, j + 1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            ws.Cells[i + 1, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            if (j <= 2 || j == 11)
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }

                            if (j >= 3 && j!= 11)
                            {
                                ws.Cells[i + 1, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[i + 1, j + 1].Style.Numberformat.Format = @"0.00";
                            }

                            if (j == 11)
                            {
                                ws.Cells[i + 1, j + 1].Value = TipoEsquema(model.Handson.ListaExcelData[i][j]);
                            }
                        }
                    }
                }

                ws.Column(1).Width = 50;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 40;
                ws.Column(4).Width = 10;
                ws.Column(5).Width = 10;
                ws.Column(6).Width = 10;
                ws.Column(7).Width = 10;
                ws.Column(8).Width = 10;
                ws.Column(9).Width = 10;

                ws.Column(10).Width = 10;
                ws.Column(11).Width = 10;

                ws.Column(12).Width = 20;

                ws.Column(13).Hidden = true;
                ws.Column(14).Hidden = true;
                ws.Column(15).Hidden = true;

                foreach (var reg in model.Handson.ListaMerge)
                {
                    ws.Cells[reg.row + 1, reg.col + 1, reg.row + reg.rowspan, reg.col + reg.colspan].Merge = true;
                }

                xlPackage.Save();
            }
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

        [HttpPost]
        public JsonResult LeerExcelSubido()
        {
            try
            {
                var titulos = ObtenerTitulosColumnas();
                Respuesta matrizValida;
                var matrizDatos = FormatoHelper.LeerExcelCargadoParametrosERAC(this.RutaCompletaArchivo, titulos, 3, out matrizValida);
                if (matrizValida.Exito)
                {
                    FormatoModel model = FormatearModeloDesdeMatriz(matrizDatos);
                    FormatoHelper.BorrarArchivo(this.RutaCompletaArchivo);
                    return Json(new Respuesta { Exito = true, Datos = model });
                }
                else
                {
                    return Json(matrizValida);
                }
            }
            catch(Exception ex)
            {
                log.Error("LeerExcelSubido",ex);
                return Json(new Respuesta { Exito = false, Datos = null });
            }
        }

        /// <summary>
        /// Genera el modelo de datos para el Handsontable en base a una matriz de datos 
        /// </summary>
        /// <param name="matrizDatos"></param>
        /// <returns></returns>
        private FormatoModel FormatearModeloDesdeMatriz(string[][] matrizDatos)
        {
            FormatoModel model = new FormatoModel();
            ConfigurarFormatoModelo(model);
            model.Handson.ListaExcelData = matrizDatos;
            return model;
        }

        private List<TipoInstancia> GenerarTipoInstancia()
        {
            return new List<TipoInstancia>{
             new TipoInstancia{IipoInstanciaId = 1, TipoInstanciaTexto = "Primer Esquema"},
             new TipoInstancia{IipoInstanciaId = 2, TipoInstanciaTexto = "Segundo Esquema"},
            };
        }

        private static string TipoEsquema(string valor)
        {
            var resp = string.Empty;

            valor = string.IsNullOrEmpty(valor) ? "" : valor;

            switch (valor)
            {
                case "1": resp = "Primer Esquema";break;
                case "2": resp = "Segundo Esquema"; break;
            }

            return resp;
        }
    }
}
