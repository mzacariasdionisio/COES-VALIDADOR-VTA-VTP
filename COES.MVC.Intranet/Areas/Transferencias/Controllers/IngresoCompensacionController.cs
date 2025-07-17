using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
//using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.Servicios.Aplicacion.Transferencias;
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

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class IngresoCompensacionController : Controller
    {
        // GET: /Transferencias/IngresoCompensacion/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = (new PeriodoAppServicio()).ListPeriodo();
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
            modelRecalculo.ListaRecalculo = (new RecalculoAppServicio()).ListRecalculos(pericodi);
            modelRecalculo.bEjecutar = true;
            //Consultamos por el estado del periodo
            PeriodoDTO entidad = new PeriodoDTO();
            entidad = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
            if (entidad.PeriEstado.Equals("Cerrado"))
            { modelRecalculo.bEjecutar = false; }
            return Json(modelRecalculo);
        }

        [HttpPost]
        public ActionResult Lista(int? pericodi, int? version)
        {
            IngresoCompensacionModel model = new IngresoCompensacionModel();
            model.ListaIngresoEmpresa = (new IngresoCompensacionAppServicio()).BuscarListaEmpresas((int)pericodi, (int)version);
            TempData["tdListaIngresoEmpresa"] = model.ListaIngresoEmpresa;
            model.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones((int)pericodi).Where(x=>x.CabeCompEstado == "ACT").ToList();
            TempData["tdListaCompensacion"] = model.ListaCompensacion;
            model.ListaIngresoCompensacion = (new IngresoCompensacionAppServicio()).ListByPeriodoVersion((int)pericodi, (int)version);
            TempData["tdListaIngresoCompensacion"] = model.ListaIngresoCompensacion;
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
        public JsonResult ProcesarArchivo(string sNombreArchivo, string sPericodi,int sVersion)
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

                ExcelRange rRange = (ExcelRange)excelWorksheet.Cells["A1:Y1000"]; //ALL: 1000 para registrar 1000
                int rColumna = 1000;
                int rFila = 1000;
                ExcelRange rCelda = null;

                Int32 iVersion = sVersion;
                string sNomEmpresa;
                Int32 iCodEmpresa;
                string sNomCompensacion;
                Int32 iCodCompensacion=0;
                Decimal iImporte = 0;
                PeriodoModel modelPeri = new PeriodoModel();
                modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(Int32.Parse(sPericodi));
                int iPeriCodi = modelPeri.Entidad.PeriCodi;

                //Eliminamos la version anterior de la tabla FactorPerdida por periodo y version
                IngresoCompensacionModel modelIC = new IngresoCompensacionModel();
                IngresoCompensacionDTO dtoIC = new IngresoCompensacionDTO();
                dtoIC.PeriCodi = (new IngresoCompensacionAppServicio()).DeleteListaIngresoCompensacion(iPeriCodi, iVersion);
                dtoIC = null;

                //Iniciamos la lectura del archivo en Excel
                for (int i = 2; i <= rColumna; i++)
                {
                    iCodEmpresa = 0;
                    rCelda = rRange.Worksheet.Cells[1, i]; //Nombre de la compensacion
                    if (rCelda.Value != null)
                    {   sNomCompensacion = rCelda.Value.ToString();
                        CompensacionModel modelComp = new CompensacionModel();
                        modelComp.Entidad = (new CompensacionAppServicio()).GetByCodigo(sNomCompensacion, iPeriCodi);
                        if (modelComp.Entidad != null)
                        {
                            iCodCompensacion = modelComp.Entidad.CabeCompCodi;
                            for (int j = 2; j <= rFila; j++)
                            {   //Listamos por empresa
                                rCelda = rRange.Worksheet.Cells[j, 1];
                                if (rCelda.Value != null)
                                {
                                    sNomEmpresa = rCelda.Value.ToString().Trim();
                                    EmpresaModel modelEmp = new EmpresaModel();
                                    modelEmp.Entidad = (new EmpresaAppServicio()).GetByNombre(sNomEmpresa);

                                    if (modelEmp.Entidad != null)
                                    {
                                        iCodEmpresa = modelEmp.Entidad.EmprCodi;
                                        rCelda = rRange.Worksheet.Cells[j, i];
                                        if (rCelda.Value != null)
                                            try
                                            {
                                                iImporte = Convert.ToDecimal(rCelda.Value);
                                            }
                                            catch
                                            {
                                                return Json("Error al leer el valor para la empresa: " + sNomEmpresa + ", compensación: " + sNomCompensacion + " dato: " + rCelda.Value.ToString()); 
                                            }
                                        dtoIC = new IngresoCompensacionDTO();
                                        dtoIC.PeriCodi = iPeriCodi;
                                        dtoIC.CompCodi = iCodCompensacion;
                                        dtoIC.EmprCodi = iCodEmpresa; //Asignamos el codigo de la empresa
                                        dtoIC.IngrCompVersion = iVersion;
                                        dtoIC.IngrCompImporte = iImporte;
                                        dtoIC.IngrCompUserName = User.Identity.Name;
                                        modelIC.IdIngresoCompensacion = (new IngresoCompensacionAppServicio()).SaveIngresoCompensacion(dtoIC);
                                        dtoIC = null;
                                    }
                                    else
                                        return Json("La empresa " + sNomEmpresa + " no se encuentra registrado");
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                            return Json("No existe el concepto de compensación " + sNomCompensacion); 
                        
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
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(iPericodi);
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                IngresoCompensacionModel model = new IngresoCompensacionModel();
                if (TempData["tdListaIngresoEmpresa"] != null)
                    model.ListaIngresoEmpresa = (List<IngresoCompensacionDTO>)TempData["tdListaIngresoEmpresa"];
                else
                    model.ListaIngresoEmpresa = (new IngresoCompensacionAppServicio()).BuscarListaEmpresas(iPeriCodi, iVersion);
                if (TempData["tdListaCompensacion"] != null)
                    model.ListaCompensacion = (List<CompensacionDTO>)TempData["tdListaCompensacion"];
                else
                    model.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones(iPeriCodi).Where(x => x.CabeCompEstado == "ACT").ToList(); 
                if (TempData["tdListaIngresoCompensacion"] != null)
                    model.ListaIngresoCompensacion = (List<IngresoCompensacionDTO>)TempData["tdListaIngresoCompensacion"];
                else
                    model.ListaIngresoCompensacion = (new IngresoCompensacionAppServicio()).ListByPeriodoVersion(iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteIngresoCompensacionExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteIngresoCompensacionExcel);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {   //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE INGRESOS POR COMPENSACIÓN: " + modelPeri.Entidad.PeriNombre + " - " + modelPeri.Entidad.RecaNombre;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        //----------------------------------------------------------
                        //ws.Cells[5, 3].Value = "COMPENSACIÓN";
                        int iColumnaInicio = 3;
                        int iColumnaFinal = iColumnaInicio + model.ListaCompensacion.Count()-1; //Numero de cabeceras de compensación en el periodo
                        int iAux = iColumnaInicio;
                        foreach (var item in model.ListaCompensacion)
                        {
                            ws.Cells[5, iAux].Value = item.CabeCompNombre; //Nombre de la compensación
                            ws.Column(iAux).Style.Numberformat.Format = "#,##0.00";
                            iAux++;
                        }

                        rg = ws.Cells[5, 2, 5, iColumnaFinal];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var dtoEmpresa in model.ListaIngresoEmpresa)
                        {
                            ws.Cells[row, 2].Value = (dtoEmpresa.EmprNombre != null) ? dtoEmpresa.EmprNombre : string.Empty;
                            //Lista de compensaciones
                            iAux = iColumnaInicio;
                            foreach (var dtoCompensacion in model.ListaCompensacion)
                            {
                                bool flag = false;
                                foreach (var dtoIngComp in model.ListaIngresoCompensacion)
                                {
                                    if (dtoIngComp.CompCodi == dtoCompensacion.CabeCompCodi && dtoEmpresa.EmprCodi == dtoIngComp.EmprCodi)
                                    {
                                        ws.Cells[row, iAux].Value = dtoIngComp.IngrCompImporte;
                                        iAux++;
                                        flag = true;
                                    }
                                }
                                if (!flag)
                                {
                                    iAux++;
                                }
                            }
                            //Border por celda
                            rg = ws.Cells[row, 2, row, iColumnaFinal];
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
                        ws.View.FreezePanes(6, iColumnaFinal+1);
                        rg = ws.Cells[5, 2, row, iColumnaFinal];
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
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteIngresoCompensacionExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteIngresoCompensacionExcel);
        }

    }
}
