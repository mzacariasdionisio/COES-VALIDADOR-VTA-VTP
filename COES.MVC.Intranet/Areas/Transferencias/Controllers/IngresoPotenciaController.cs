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
using COES.Servicios.Aplicacion.TransfPotencia;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class IngresoPotenciaController : Controller
    {
        // GET: /Transferencias/IngresoPotencia/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        IngresoPotenciaAppServicio servicioIngresoPotencia = new IngresoPotenciaAppServicio();
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = servicioPeriodo.ListPeriodo();
            TempData["Pericodigo"] = new SelectList(modelPeriodo.ListaPeriodos, "Pericodi", "Perinombre");
            TempData["Pericodigo1"] = new SelectList(modelPeriodo.ListaPeriodos, "Pericodi", "Perinombre");
            return View();
        }

        /// <summary>
        /// Permite cargar versiones de acuerdo al periodo
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
        public ActionResult Lista(int? pericodi, int? version)
        {
            IngresoPotenciaModel model = new IngresoPotenciaModel();
            model.ListaIngresoPotencia = servicioIngresoPotencia.BuscarIngresoPotencia(pericodi, version);
            TempData["tdListaIngresoPotencia"] = model.ListaIngresoPotencia;
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
        public JsonResult ProcesarArchivo(string sNombreArchivo, string sPericodi, int sVersion)
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

            try
            {
                //Eliminamos el proceso de valorización vigente en caso existiese.
                string sMensaje = EliminarProcesoValorizacion(Int32.Parse(sPericodi), sVersion);
                if (!sMensaje.Equals("1")) return Json(sMensaje);

                //tratamos el archivo cargado en el directorio
                string aux = path + "/" + sNombreArchivo;
                FileInfo archivo = new FileInfo(path + sNombreArchivo);
                ExcelPackage xlPackage = new ExcelPackage(archivo);
                ExcelWorksheet excelWorksheet = xlPackage.Workbook.Worksheets[1];

                ExcelRange rRange = (ExcelRange)excelWorksheet.Cells["A1:ALL2"]; //ALL: 1000 para registrar 1000
                int rColumna = 1000;
                ExcelRange rCelda = null;

                Int32 iVersion = sVersion;
                string sNomEmpresa;
                Int32 iCodEmpresa;
                Decimal iImporte = 0;
                PeriodoModel modelPeri = new PeriodoModel();
                modelPeri.Entidad = servicioPeriodo.GetByIdPeriodo(Int32.Parse(sPericodi));
                int iPeriCodi = modelPeri.Entidad.PeriCodi;

                //Eliminamos la version anterior de la tabla FactorPerdida por periodo y version
                IngresoPotenciaModel modelIP = new IngresoPotenciaModel();
                IngresoPotenciaDTO dtoIP = new IngresoPotenciaDTO();
                dtoIP.PeriCodi = servicioIngresoPotencia.DeleteListaIngresoPotencia(iPeriCodi, iVersion);
                dtoIP = null;

                //Iniciamos la lectura del archivo en Excel por columnas
                for (int i = 2; i <= rColumna; i++)
                {
                    iCodEmpresa = 0;
                    //Primer elemento de la fila es el Nombre de la empresa
                    rCelda = rRange.Worksheet.Cells[1, i];
                    if (rCelda.Value != null)
                    {   //Encontramos fila de datos
                        sNomEmpresa = rCelda.Value.ToString().Trim();
                        EmpresaModel modelEmp = new EmpresaModel();
                        modelEmp.Entidad = (new EmpresaAppServicio()).GetByNombre(sNomEmpresa);
                        if (modelEmp.Entidad != null)
                        {
                            iCodEmpresa = modelEmp.Entidad.EmprCodi;
                            rCelda = rRange.Worksheet.Cells[2, i];
                            if (rCelda.Value != null)
                            {
                                try
                                {
                                    iImporte = Convert.ToDecimal(rCelda.Value); //.ToString()
                                }
                                catch
                                {
                                    return Json("Error al leer el valor para la empresa: " + sNomEmpresa + " dato: " + rCelda.Value.ToString());
                                }
                            }
                            //ASSETEC 20181119
                            IngresoPotenciaDTO dtoIPExiste = servicioIngresoPotencia.GetByPeriodoVersionEmpresa(iPeriCodi, iVersion, iCodEmpresa);
                            if (dtoIPExiste != null)
                            {
                                return Json("Error la empresa: " + sNomEmpresa + " ya tiene registrado el siguiente dato: " + iImporte + " en esta revisión.");
                            }
                            dtoIP = new IngresoPotenciaDTO();
                            dtoIP.PeriCodi = iPeriCodi;
                            dtoIP.EmprCodi = iCodEmpresa; //Asignamos el codigo de la empresa
                            dtoIP.IngrPoteVersion = iVersion;
                            dtoIP.IngrPoteImporte = iImporte;
                            dtoIP.IngrPoteUserName = User.Identity.Name;
                            modelIP.IdIngresoPotencia = servicioIngresoPotencia.SaveIngresoPotencia(dtoIP);
                            dtoIP = null;
                        }
                        else
                            return Json("La empresa " + sNomEmpresa + " no se encuentra registrado");
                    }
                    else
                    {
                        break;
                    }
                }

                xlPackage.Dispose();
                rRange = null;

                return Json("1");
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());  //Json(-1);
            }
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

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                IngresoPotenciaModel model = new IngresoPotenciaModel();
                if (TempData["tdListaIngresoPotencia"] != null)
                    model.ListaIngresoPotencia = (List<IngresoPotenciaDTO>)TempData["tdListaIngresoPotencia"];
                else
                    model.ListaIngresoPotencia = servicioIngresoPotencia.BuscarIngresoPotencia(iPericodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteIngresoPotenciaExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteIngresoPotenciaExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 4].Value = "LISTA DE INGRESOS POR POTENCIA: " + modelPeri.Entidad.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "PERIODO";
                        ws.Cells[5, 3].Value = "VERSIÓN";
                        ws.Cells[5, 4].Value = "EMPRESA";
                        ws.Cells[5, 5].Value = "IMPORTE";
                        ws.Cells[5, 6].Value = "FECHA";
                        ws.Cells[5, 7].Value = "USUARIO";

                        rg = ws.Cells[5, 2, 5, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in model.ListaIngresoPotencia)
                        {
                            ws.Cells[row, 2].Value = (item.PeriNombre != null) ? item.PeriNombre.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = item.IngrPoteVersion;
                            ws.Cells[row, 4].Value = (item.EmprNombre != null) ? item.EmprNombre : string.Empty;
                            ws.Cells[row, 5].Value = item.IngrPoteImporte;
                            ws.Cells[row, 6].Value = item.IngrPoteFecIns.ToString("dd/MM/yyyy");
                            ws.Cells[row, 7].Value = (item.IngrPoteUserName != null) ? item.IngrPoteUserName.ToString() : string.Empty;
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
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteIngresoPotenciaExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteIngresoPotenciaExcel);
        }

        //ASSETEC 20181224
        [HttpPost]
        public ActionResult CopiarVTP(int pericodi)
        {
            RetiroPotenciaSCModel model = new RetiroPotenciaSCModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.Pericodi = pericodi;
            model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
            model.Recpotcodi = 0;
            if (model.ListaRecalculoPotencia.Count > 0)
            { model.Recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            if (pericodi > 0 && model.Recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, model.Recpotcodi);
            }
            return PartialView(model);
        }

        //ASSETEC 20190116
        [HttpPost]
        public JsonResult ImportarVTP(int pericodi, int version, int pericodiVTP, int recpotcodi)
        {
            string indicador = "-1";
            try
            {

                //Eliminamos el proceso de valorización vigente en caso existiese.
                string sMensaje = EliminarProcesoValorizacion(pericodi, version);
                if (!sMensaje.Equals("1")) return Json(sMensaje);
                servicioIngresoPotencia.DeleteListaIngresoPotencia(pericodi, version);

                VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodiVTP, recpotcodi);
                if (EntidadRecalculoPotencia.Recpotestado.Equals("Cerrado"))
                {
                    List<VtpSaldoEmpresaDTO> ListaSaldoEmpresa = this.servicioTransfPotencia.ListVtpSaldoEmpresas(pericodiVTP, recpotcodi);
                    //Calculando el total
                    decimal dTotalIngreso = 0;
                    foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresa in ListaSaldoEmpresa)
                    {
                        dTotalIngreso += dtoSaldoEmpresa.Potseingreso;
                    }
                    if (dTotalIngreso > 0)
                    {
                        foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresa in ListaSaldoEmpresa)
                        {
                            decimal dIngreso = dtoSaldoEmpresa.Potseingreso / dTotalIngreso;
                            IngresoPotenciaDTO dtoIP = new IngresoPotenciaDTO();
                            dtoIP.PeriCodi = pericodi;
                            dtoIP.EmprCodi = dtoSaldoEmpresa.Emprcodi; //Asignamos el codigo de la empresa
                            dtoIP.IngrPoteVersion = version;
                            dtoIP.IngrPoteImporte = dIngreso;
                            dtoIP.IngrPoteUserName = User.Identity.Name;
                            servicioIngresoPotencia.SaveIngresoPotencia(dtoIP);
                        }
                        indicador = "1";
                    }
                    else
                    {
                        indicador = "No se encontraron saldos para este periodo / versión";
                    }
                }
                else
                {
                    indicador = "El periodo de VTP aún no eta cerrado";
                }
            }
            catch (Exception e)
            {
                indicador = e.Message;//"-1";
            }
            return Json(indicador);
        }
    }
}
