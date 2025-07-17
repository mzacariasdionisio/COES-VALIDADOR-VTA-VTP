using COES.MVC.Intranet.Helper;
//using COES.MVC.Intranet.Models;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Collections;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Globalization;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class FactorPerdidaMediaController : Controller
    {
        // GET: /Transferencias/IngresoRetiroSC/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        CentralGeneracionAppServicio servicioCentral = new CentralGeneracionAppServicio();
        CodigoEntregaAppServicio servicioEntrega = new CodigoEntregaAppServicio();
        TransferenciasAppServicio servicioTransferencias = new TransferenciasAppServicio();
        ConsultaMedidoresAppServicio servicioMedidores = new ConsultaMedidoresAppServicio();
        Funcion listaFunciones = new Funcion();

        public ActionResult Index()
        {
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = servicioPeriodo.ListPeriodo();
            TempData["Pericodigo"] = new SelectList(modelPeriodo.ListaPeriodos, "Pericodi", "Perinombre");
            TempData["Pericodigo1"] = new SelectList(modelPeriodo.ListaPeriodos, "Pericodi", "Perinombre");
            return View();
        }

        /// <summary>
        /// Permite cargar versiones deacuerdo al periodo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVersion(int pericodi)
        {
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.ListaRecalculo = servicioRecalculo.ListRecalculos(pericodi);
            modelRecalculo.bEjecutar = true;
            //Consultamos por el estado del periodo
            PeriodoDTO entidad = new PeriodoDTO();
            entidad = servicioPeriodo.GetByIdPeriodo(pericodi);
            if (entidad.PeriEstado.Equals("Cerrado"))
            { modelRecalculo.bEjecutar = false; }
            return Json(modelRecalculo);
        }

        [HttpPost]
        public ActionResult Lista(int pericodi,int version)
        {
            FactorPerdidaMediaModel model = new FactorPerdidaMediaModel();
            model.ListaFactoresPerdidaMedia = servicioTransferencias.ListTrnFactorPerdidaMedias(pericodi, version);
            TempData["tdListaFactoresPerdidaMedia"] = model.ListaFactoresPerdidaMedia;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Upload(string sFecha)
        {
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = sFecha + "_" + file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ProcesarArchivo(string sNombreArchivo, int Pericodi, int Version)
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
            try
            {
                //Eliminamos el proceso de valorización vigente en caso existiese.
                string sMensaje = EliminarProcesoValorizacion(Pericodi, Version);
                if (!sMensaje.Equals("1")) return Json(sMensaje);

                //Eliminamos la version anterior de la tabla FactorPerdidaMedia por periodo y version
                servicioTransferencias.DeleteTrnFactorPerdidaMedia(Pericodi, Version);

                //Leemo el contenido del archivo Excel
                DataSet ds = new DataSet();
                ds = listaFunciones.GeneraDataset(path + sNombreArchivo, 1);
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 2)
                    {
                        continue;
                    }
                    //Codigo entrega
                    if (dtRow[1].ToString() == "null" || dtRow[1].ToString() == "")
                    {
                        break; //termino
                    }
                    string sCodigoEntrega = dtRow[0].ToString();
                    CodigoEntregaDTO dtoEntrega = servicioEntrega.GetByCodigoEntregaCodigo(sCodigoEntrega);
                    if (dtoEntrega == null)
                    {
                        continue;
                        //return Json("El código de entrega: " + sCodigoEntrega + " no se encuentra registrado");
                    }
                    decimal dTrnfpmvalor = Convert.ToDecimal(dtRow[4].ToString());
                    string sTrnfpmobserv = dtRow[5].ToString();
                    if (sTrnfpmobserv.Equals("null")) sTrnfpmobserv = "";
                    //Insertamos el registro
                    TrnFactorPerdidaMediaDTO dtoFPM = new TrnFactorPerdidaMediaDTO();
                    dtoFPM.Pericodi = Pericodi;
                    dtoFPM.Barrcodi = dtoEntrega.BarrCodi;
                    dtoFPM.Codentcodi = dtoEntrega.CodiEntrCodi;
                    dtoFPM.Emprcodi = dtoEntrega.EmprCodi;
                    dtoFPM.Equicodi = dtoEntrega.CentGeneCodi;
                    dtoFPM.Trnfpmversion = Version;
                    dtoFPM.Trnfpmvalor = dTrnfpmvalor;
                    dtoFPM.Trnfpmobserv = sTrnfpmobserv;
                    dtoFPM.Trnfpmusername = User.Identity.Name;
                    servicioTransferencias.SaveTrnFactorPerdidaMedia(dtoFPM);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());  //Json(-1);
            }
            return Json("1");
        }

        /// <summary>
        /// Permite eliminar el proceso de calculo de la matriz de pagos - Valorización
        /// </summary>
        /// <returns>1 si la eliminación fue correcta</returns>
        public string EliminarProcesoValorizacion(int pericodi, int vers)
        {
            try
            {
                //Elimina información de la tabla trn_valor_trans = Valorización de la Transferencia de Entregas y Retiros por Empresa[15]
                int eliminavalor = 0;
                eliminavalor = new ValorTransferenciaAppServicio().DeleteListaValorTransferencia(pericodi, vers);

                //Elimina información de la tabla trn_valor_trans_empresa
                int deletepok = 0;
                deletepok = new ValorTransferenciaAppServicio().DeleteValorTransferenciaEmpresa(pericodi, vers);

                //Elimina información calculada de los Ingresos por potencia de las empresas -> tabla trn_saldo_empresa
                int deleteSaldo = 0;
                deleteSaldo = new ValorTransferenciaAppServicio().DeleteSaldoTransmisionEmpresa(pericodi, vers);

                //Elimina información calculada de los Ingresos por Retiros sin contrato de las empresas -> de la tabla trn_saldo_coresc
                int deleteSaldoSC = 0;
                deleteSaldoSC = new ValorTransferenciaAppServicio().DeleteSaldoCodigoRetiroSC(pericodi, vers);

                //Elimina información de la tabla trn_empresa_pago = Matriz de Pagos
                int eliminook = 0;
                eliminook = (new ValorTransferenciaAppServicio()).DeleteEmpresaPago(pericodi, vers);

                //Elimina información calculado del Valor Total de la Empresa -> trn_valor_total_empresa
                int deleteTVEmpresa = 0;
                deleteTVEmpresa = new ValorTransferenciaAppServicio().DeleteValorTotalEmpresa(pericodi, vers);

                if (vers > 1)
                {
                    //Elimina información calculado del Saldo por Recalculo de la Empresa -> trn_saldo_recalculo
                    int deleteSaldoRecalculo = 0;
                    deleteSaldoRecalculo = new ValorTransferenciaAppServicio().DeleteSaldoRecalculo(pericodi, vers);
                }

                return "1";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpPost]
        public JsonResult GenerarExcel(int iPericodi, int iVersion)
        {
            int iResultado = 1;
            //Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = servicioPeriodo.GetByIdPeriodo(iPericodi);
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;
            RecalculoModel modelRecalculo = new RecalculoModel();
            modelRecalculo.Entidad = servicioRecalculo.GetByIdRecalculo(iPericodi, iVersion);

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FactorPerdidaMediaModel model = new FactorPerdidaMediaModel();
                model.ListaFactoresPerdidaMedia = servicioTransferencias.ListTrnFactorPerdidaMedias(iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteFactorPerdidaMediaExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteFactorPerdidaMediaExcel);
                }
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 4].Value = "LISTA DE FACTORES DE PÉRDIDA MEDIAS: " + modelPeri.Entidad.PeriNombre + " / " + modelRecalculo.Entidad.RecaNombre;
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "CÓDIGO ENTREGA";
                        ws.Column(2).Width = 10;
                        ws.Cells[5, 3].Value = "BARRA DE TRANSFERENCIA";
                        ws.Column(3).Width = 20;
                        ws.Cells[5, 4].Value = "EMPRESA";
                        ws.Column(4).Width = 20;
                        ws.Cells[5, 5].Value = "CENTRAL / GRUPO DE GENERACIÓN";
                        ws.Column(5).Width = 20;
                        ws.Cells[5, 6].Value = "FACTOR DE PÉRDIDAS MEDIAS";
                        ws.Column(6).Width = 15;
                        ws.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ws.Column(6).Style.Numberformat.Format = "#,##0.0000000000";
                        ws.Cells[5, 7].Value = "OBSERVACIONES / COMENTARIOS";
                        ws.Column(7).Width = 30;

                        rg = ws.Cells[5, 2, 5, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaFactoresPerdidaMedia)
                        {
                            ws.Cells[row, 2].Value = item.Codentcodigo.ToString();
                            ws.Cells[row, 3].Value = item.Barrnombre.ToString();
                            ws.Cells[row, 4].Value = item.Emprnomb.ToString();
                            ws.Cells[row, 5].Value = item.Equinomb.ToString();
                            ws.Cells[row, 6].Value = Convert.ToDecimal(item.Trnfpmvalor);
                            ws.Cells[row, 7].Value = item.Trnfpmobserv;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 7];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 8);
                        rg = ws.Cells[5, 2, row, 7];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                iResultado = 1;
            }
            catch
            {
                iResultado = -1;
            }
            return Json(iResultado);
        }

        [HttpGet]
        public virtual ActionResult AbrirExcel()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteIngresoRetiroSCExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteIngresoRetiroSCExcel);
        }

        //ASSETEC 20190104
        /// <summary>
        /// Permite exportar a un archivo excel la lista de Medidores de Bornes de Generación
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarMB(int pericodi = 0)
        {
            string result = "-1";
            try
            {
                FactorPerdidaMediaModel model = new FactorPerdidaMediaModel();
                if (pericodi > 0)
                {
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    string sMes = model.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                    string empresas = Constantes.ParametroDefecto;
                    string tiposGeneracion = Constantes.ParametroDefecto;
                    string tiposEmpresa = "1,2,3,4,5";
                    int central = 1;
                    string parametros = "1,3"; //1: Potencia Activa / 3: Servicios Auxiliares
                    int tipo = 1;

                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    string file = NombreArchivo.ReporteMedidoresHorizontal;
                    bool flag = (User.Identity.Name == Constantes.UsuarioAnonimo) ? false : true;
                    this.servicioMedidores.GenerarArchivoExportacion(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, central, parametros, path, file, tipo, flag);
                    result = file;
                }
            }
            catch (Exception e)
            {
                string sMensaje = e.Message;
                result = "-1";
            }
            return Json(result);
        }

        /// <summary>
        /// Abrir el archivode de Medidores de Bornes de Generación
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Permite copiar la información de la base de datos de los Medidores de Bornes de Generación
        /// </summary>
        /// <param name="pericodi">Código del Mes de cálculo</param>
        /// <param name="version">Versión de cálculo de VEA</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopiarMB(int pericodi = 0, int version = 0)
        {
            FactorPerdidaMediaModel model = new FactorPerdidaMediaModel();
            model.sError = "";
            model.iNumReg = 0;
            try
            {
                if (pericodi > 0 && version > 0)
                {
                    //Eliminamos el proceso de valorización vigente en caso existiese.
                    string sMensaje = EliminarProcesoValorizacion(pericodi, version);
                    if (!sMensaje.Equals("1")) return Json(sMensaje);

                    //Eliminamos la version anterior de la tabla FactorPerdidaMedia por periodo y version
                    servicioTransferencias.DeleteTrnMedborne(pericodi, version);

                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                    string sMes = model.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;

                    var sFechaInicio = "01/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    var sFechaFin = System.DateTime.DaysInMonth(model.EntidadPeriodo.AnioCodi, model.EntidadPeriodo.MesCodi) + "/" + sMes + "/" + model.EntidadPeriodo.AnioCodi;
                    DateTime fecInicio = DateTime.ParseExact(sFechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fecFin = DateTime.ParseExact(sFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    //Obtenemos la informacion de los Medidores de Bornes de Generación
                    string empresas = Constantes.ParametroDefecto;
                    string tiposGeneracion = Constantes.ParametroDefecto;
                    int central = 1;
                    model.iNumReg = this.servicioTransferencias.GrabarMedidorBorne(pericodi, version, User.Identity.Name, 1, empresas, central, tiposGeneracion, fecInicio, fecFin);
                }
                else
                    model.sError = "Debe seleccionar un periodo y versión correcto";
            }
            catch (Exception e)
            {
                model.sError = e.Message;
            }
            if (model.iNumReg == -1)
                model.sError = "Lo sentimos, se ha producido un error";
            return Json(model);
        }
    }
}
