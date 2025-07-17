using COES.MVC.Intranet.Helper;
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
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Dominio.DTO.Enum;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class FactorPerdidaController : Controller
    {
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();
        // GET: /Transferencias/factorperdida/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            int iPeriodo = 0;
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = (new PeriodoAppServicio()).ListPeriodo();
            if (modelPeriodo.ListaPeriodos.Count > 0)
                iPeriodo = modelPeriodo.ListaPeriodos[0].PeriCodi;
            BarraModel modelBarraE = new BarraModel();
            modelBarraE.ListaBarras = (new BarraAppServicio()).ListBarrasTransferenciaByReporte();

            TempData["Pericodigo"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");
            TempData["PericodigoGraf"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");
            TempData["PericodigoExcel"] = new SelectList(modelPeriodo.ListaPeriodos, "PERICODI", "PERINOMBRE");
            TempData["dValorMax"] = Funcion.dValorMax;
            TempData["BarrcodigoExcel"] = modelBarraE;
            return View();
        }

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

            bool bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            if (bGrabar == false)
            {
                modelRecalculo.bEjecutar = false;
            }

            return Json(modelRecalculo);
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
        public JsonResult ProcesarArchivo(string sNombreArchivo, string sPericodi, decimal dValorMax, int vers)
        {
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
            string sMensaje = "1";

            try
            {
                //Eliminamos el proceso de valorización vigente en caso existiese.
                sMensaje = EliminarProcesoValorizacion(Int32.Parse(sPericodi), vers);
                if (!sMensaje.Equals("1")) return Json(sMensaje);

                //Para el reporte de errores                 
                //List<ReporteLogDTO> list = new List<ReporteLogDTO>();
                FileInfo archivo = new FileInfo(path + sNombreArchivo);
                ExcelPackage xlPackage = new ExcelPackage(archivo);
                ExcelWorksheet excelWorksheet = xlPackage.Workbook.Worksheets[1];

                ExcelRange rRange = (ExcelRange)excelWorksheet.Cells["A1:BEH500"]; //BEH: es el intervalo maximo en un mes de 31 dias / 500: numero de barras a lo mas
                int rFilas = 500;

                //ExcelRow rFila = null;
                ExcelRange rCelda = null;
                string sCelda = null;

                Int32 iVersion = vers;
                string sFlagBase = "S"; //Para la primera fila que es Base
                Int32 iCodBarra;
                string sNomBarra;
                PeriodoModel modelPeri = new PeriodoModel();
                modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(Int32.Parse(sPericodi));
                int iPeriCodi = modelPeri.Entidad.PeriCodi;
                int iAnioCodi = modelPeri.Entidad.AnioCodi;
                int iMesCodi = modelPeri.Entidad.MesCodi;
                int iAnioMesDiaVigencia = modelPeri.Entidad.PeriAnioMes * 100; //Para que quede 20171000
                int iDiasMes = DateTime.DaysInMonth(iAnioCodi, iMesCodi); // Extrae el numero de dias en el mes
                decimal[] FacPer = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los Factores de Perdida
                decimal[,] FacPerBase = new decimal[31, 48]; //31 dias en un mes, 48 intervalos de 30 minutos
                decimal[] CosMar = new decimal[96]; // Donde se almacena los intervalos de 15 minuos por dia para el Costo Marginal

                //ELIMINAMOS LA INFORMACION EN CASCADA
                //Eliminamos la version anterior de la tabla CostoMarginal por periodo y version
                CostoMarginalModel modelCosMar = new CostoMarginalModel();
                CostoMarginalDTO dtoCosMar = new CostoMarginalDTO();
                dtoCosMar.PeriCodi = (new CostoMarginalAppServicio()).DeleteListaCostoMarginal(iPeriCodi, iVersion);
                dtoCosMar = null;

                //Eliminamos la version anterior de la tabla FactorPerdida por periodo y version
                FactorPerdidaModel modelFacPer = new FactorPerdidaModel();
                FactorPerdidaDTO dtoFacPer = new FactorPerdidaDTO();
                dtoFacPer.PeriCodi = (new FactorPerdidaAppServicio()).DeleteListaFactorPerdida(iPeriCodi, iVersion);
                dtoFacPer = null;
                //Bandera para salir de la lectura del archivo si se ha producido algun error de procesamiento
                bool bSalir = false;
                //Iniciamos la lectura del archivo en Excel
                for (int i = 1; i <= rFilas; i++)
                {
                    if (bSalir) break;
                    //rFila = excelWorksheet.Row(i);
                    iCodBarra = 0;
                    sNomBarra = "Null";

                    //Primer elemento de la fila es el Codigo (de base de datos) de la Barra
                    rCelda = rRange.Worksheet.Cells[i, 1];

                    if (rCelda.Value != null)
                    {   //Encontramos fila de datos
                        try
                        {
                            iCodBarra = Convert.ToInt32(rCelda.Value.ToString());
                            BarraModel modelBarra = new BarraModel();
                            modelBarra.Entidad = (new BarraAppServicio()).GetByIdBarra(iCodBarra);
                            if (modelBarra.Entidad == null)
                            {
                                sMensaje = "No existe el código de la barra " + sNomBarra;
                                bSalir = true;
                                break;
                            }
                            sNomBarra = modelBarra.Entidad.BarrNombBarrTran.ToString();
                            if (sNomBarra.Equals(""))
                            {   //Segundo elemento de la fila es el Nombre de la Barra
                                rCelda = rRange.Worksheet.Cells[i, 2];
                                if (rCelda.Value != null) sNomBarra = rCelda.Value.ToString();
                            }
                        }
                        catch
                        {
                            rCelda = rRange.Worksheet.Cells[i, 2];
                            if (rCelda.Value != null) sNomBarra = rCelda.Value.ToString();
                            sMensaje = "Se encontro un caracter no valido en el código de la barra " + sNomBarra;
                            bSalir = true;
                            break;
                        }

                        int iColumna = 3;//Se inicia desde la 3ra posicion
                        for (int iDia = 1; iDia <= iDiasMes; iDia++)
                        {
                            if (bSalir) break;
                            //lectura para cad dia del mes
                            decimal dPromedioDiaCosMar = 0; //Se inicia el valor promedio en cero

                            for (int k = 0; k < 48; k++)
                            {   //lectura para las 48 celdas de un dia
                                sCelda = "0";
                                rCelda = rRange.Worksheet.Cells[i, k + iColumna]; //k celda + Grupo iColumna

                                if (rCelda.Value != null)
                                    sCelda = rCelda.Value.ToString();
                                //Guardamos lo elementos del arreglo
                                FacPer[k] = Decimal.Parse(sCelda, System.Globalization.NumberStyles.Float);  //MCHAVEZ[20180207]: Convert.ToDecimal(sCelda);
                                if (sFlagBase.Equals("S"))
                                {   //Almacenamos El Factor Perdida Base
                                    FacPerBase[iDia - 1, k] = FacPer[k];
                                }
                                else
                                {
                                    //Preparamos el Arreglo del Costo Margina del dia en intervalos de 15 minutos 
                                    CosMar[2 * k] = CosMar[(2 * k) + 1] = (FacPer[k] * FacPerBase[iDia - 1, k]);
                                    //MCHAVEZ - 20171017: Se agrego ((iAnioMesDiaVigencia + iDia) <= 20171001) a Solicitud de STR
                                    if (((iAnioMesDiaVigencia + iDia) <= 20171001) && (CosMar[2 * k] > dValorMax))
                                    {
                                        CosMar[2 * k] = CosMar[(2 * k) + 1] = dValorMax;
                                        //ReporteLogDTO objR = new ReporteLogDTO();
                                        //objR.NombreBarra = sNomBarra;
                                        //objR.Dia = iDia;
                                        //objR.ValorCostoMarginal = CosMar[2 * k];
                                        //list.Add(objR);
                                    }
                                    //else
                                    //{
                                    //SI EXISTEN REGISTROS CON VALORES MAYORES A LOS ESPERADOS SE PARA LA CARGA HASTA DONDE A LEIDO.
                                    //sMensaje = "Lo sentimos, Se ha interrumpido la carga de información";
                                    //bSalir = true;
                                    //break;
                                    //}

                                    dPromedioDiaCosMar += (2 * CosMar[2 * k]); //vamos sumarisando y luego promediamos
                                }
                            }

                            //Insertamos la informacion en la tabla Factor Perdida
                            dtoFacPer = new FactorPerdidaDTO();
                            dtoFacPer.PeriCodi = iPeriCodi;
                            dtoFacPer.BarrCodi = iCodBarra; //Asignamos el codigo de la Barra
                            dtoFacPer.FacPerBarrNombre = sNomBarra;
                            dtoFacPer.FacPerBase = sFlagBase;
                            dtoFacPer.FacPerVersion = iVersion;
                            dtoFacPer.FacPerDia = iDia; // dia del mes

                            dtoFacPer.FacPer1 = FacPer[0];
                            dtoFacPer.FacPer2 = FacPer[1];
                            dtoFacPer.FacPer3 = FacPer[2];
                            dtoFacPer.FacPer4 = FacPer[3];
                            dtoFacPer.FacPer5 = FacPer[4];
                            dtoFacPer.FacPer6 = FacPer[5];
                            dtoFacPer.FacPer7 = FacPer[6];
                            dtoFacPer.FacPer8 = FacPer[7];
                            dtoFacPer.FacPer9 = FacPer[8];
                            dtoFacPer.FacPer10 = FacPer[9];
                            dtoFacPer.FacPer11 = FacPer[10];
                            dtoFacPer.FacPer12 = FacPer[11];
                            dtoFacPer.FacPer13 = FacPer[12];
                            dtoFacPer.FacPer14 = FacPer[13];
                            dtoFacPer.FacPer15 = FacPer[14];
                            dtoFacPer.FacPer16 = FacPer[15];
                            dtoFacPer.FacPer17 = FacPer[16];
                            dtoFacPer.FacPer18 = FacPer[17];
                            dtoFacPer.FacPer19 = FacPer[18];
                            dtoFacPer.FacPer20 = FacPer[19];
                            dtoFacPer.FacPer21 = FacPer[20];
                            dtoFacPer.FacPer22 = FacPer[21];
                            dtoFacPer.FacPer23 = FacPer[22];
                            dtoFacPer.FacPer24 = FacPer[23];
                            dtoFacPer.FacPer25 = FacPer[24];
                            dtoFacPer.FacPer26 = FacPer[25];
                            dtoFacPer.FacPer27 = FacPer[26];
                            dtoFacPer.FacPer28 = FacPer[27];
                            dtoFacPer.FacPer29 = FacPer[28];
                            dtoFacPer.FacPer30 = FacPer[29];
                            dtoFacPer.FacPer31 = FacPer[30];
                            dtoFacPer.FacPer32 = FacPer[31];
                            dtoFacPer.FacPer33 = FacPer[32];
                            dtoFacPer.FacPer34 = FacPer[33];
                            dtoFacPer.FacPer35 = FacPer[34];
                            dtoFacPer.FacPer36 = FacPer[35];
                            dtoFacPer.FacPer37 = FacPer[36];
                            dtoFacPer.FacPer38 = FacPer[37];
                            dtoFacPer.FacPer39 = FacPer[38];
                            dtoFacPer.FacPer40 = FacPer[39];
                            dtoFacPer.FacPer41 = FacPer[40];
                            dtoFacPer.FacPer42 = FacPer[41];
                            dtoFacPer.FacPer43 = FacPer[42];
                            dtoFacPer.FacPer44 = FacPer[43];
                            dtoFacPer.FacPer45 = FacPer[44];
                            dtoFacPer.FacPer46 = FacPer[45];
                            dtoFacPer.FacPer47 = FacPer[46];
                            dtoFacPer.FacPer48 = FacPer[47];
                            dtoFacPer.FacPerUserName = User.Identity.Name;
                            modelFacPer.IdFactorPerdida = (new FactorPerdidaAppServicio()).SaveFactorPerdida(dtoFacPer);
                            dtoFacPer = null;

                            if (sFlagBase.Equals("N"))
                            {   //Insertamos la informacion en la tabla CostoMarginal
                                dtoCosMar = new CostoMarginalDTO();
                                dtoCosMar.PeriCodi = iPeriCodi;
                                dtoCosMar.BarrCodi = iCodBarra; //Asignamos el codigo de la Barra

                                dtoCosMar.CosMarBarraTransferencia = sNomBarra; //Agregar el nombre de la Barra de Transferencia
                                dtoCosMar.FacPerCodi = modelFacPer.IdFactorPerdida;
                                dtoCosMar.CosMarVersion = iVersion;
                                dtoCosMar.CosMarDia = iDia; // dia del mes

                                dtoCosMar.CosMar1 = CosMar[0];
                                dtoCosMar.CosMar2 = CosMar[1];
                                dtoCosMar.CosMar3 = CosMar[2];
                                dtoCosMar.CosMar4 = CosMar[3];
                                dtoCosMar.CosMar5 = CosMar[4];
                                dtoCosMar.CosMar6 = CosMar[5];
                                dtoCosMar.CosMar7 = CosMar[6];
                                dtoCosMar.CosMar8 = CosMar[7];
                                dtoCosMar.CosMar9 = CosMar[8];
                                dtoCosMar.CosMar10 = CosMar[9];
                                dtoCosMar.CosMar11 = CosMar[10];
                                dtoCosMar.CosMar12 = CosMar[11];
                                dtoCosMar.CosMar13 = CosMar[12];
                                dtoCosMar.CosMar14 = CosMar[13];
                                dtoCosMar.CosMar15 = CosMar[14];
                                dtoCosMar.CosMar16 = CosMar[15];
                                dtoCosMar.CosMar17 = CosMar[16];
                                dtoCosMar.CosMar18 = CosMar[17];
                                dtoCosMar.CosMar19 = CosMar[18];
                                dtoCosMar.CosMar20 = CosMar[19];
                                dtoCosMar.CosMar21 = CosMar[20];
                                dtoCosMar.CosMar22 = CosMar[21];
                                dtoCosMar.CosMar23 = CosMar[22];
                                dtoCosMar.CosMar24 = CosMar[23];
                                dtoCosMar.CosMar25 = CosMar[24];
                                dtoCosMar.CosMar26 = CosMar[25];
                                dtoCosMar.CosMar27 = CosMar[26];
                                dtoCosMar.CosMar28 = CosMar[27];
                                dtoCosMar.CosMar29 = CosMar[28];
                                dtoCosMar.CosMar30 = CosMar[29];
                                dtoCosMar.CosMar31 = CosMar[30];
                                dtoCosMar.CosMar32 = CosMar[31];
                                dtoCosMar.CosMar33 = CosMar[32];
                                dtoCosMar.CosMar34 = CosMar[33];
                                dtoCosMar.CosMar35 = CosMar[34];
                                dtoCosMar.CosMar36 = CosMar[35];
                                dtoCosMar.CosMar37 = CosMar[36];
                                dtoCosMar.CosMar38 = CosMar[37];
                                dtoCosMar.CosMar39 = CosMar[38];
                                dtoCosMar.CosMar40 = CosMar[39];
                                dtoCosMar.CosMar41 = CosMar[40];
                                dtoCosMar.CosMar42 = CosMar[41];
                                dtoCosMar.CosMar43 = CosMar[42];
                                dtoCosMar.CosMar44 = CosMar[43];
                                dtoCosMar.CosMar45 = CosMar[44];
                                dtoCosMar.CosMar46 = CosMar[45];
                                dtoCosMar.CosMar47 = CosMar[46];
                                dtoCosMar.CosMar48 = CosMar[47];
                                dtoCosMar.CosMar49 = CosMar[48];
                                dtoCosMar.CosMar50 = CosMar[49];
                                dtoCosMar.CosMar51 = CosMar[50];
                                dtoCosMar.CosMar52 = CosMar[51];
                                dtoCosMar.CosMar53 = CosMar[52];
                                dtoCosMar.CosMar54 = CosMar[53];
                                dtoCosMar.CosMar55 = CosMar[54];
                                dtoCosMar.CosMar56 = CosMar[55];
                                dtoCosMar.CosMar57 = CosMar[56];
                                dtoCosMar.CosMar58 = CosMar[57];
                                dtoCosMar.CosMar59 = CosMar[58];
                                dtoCosMar.CosMar60 = CosMar[59];
                                dtoCosMar.CosMar61 = CosMar[60];
                                dtoCosMar.CosMar62 = CosMar[61];
                                dtoCosMar.CosMar63 = CosMar[62];
                                dtoCosMar.CosMar64 = CosMar[63];
                                dtoCosMar.CosMar65 = CosMar[64];
                                dtoCosMar.CosMar66 = CosMar[65];
                                dtoCosMar.CosMar67 = CosMar[66];
                                dtoCosMar.CosMar68 = CosMar[67];
                                dtoCosMar.CosMar69 = CosMar[68];
                                dtoCosMar.CosMar70 = CosMar[69];
                                dtoCosMar.CosMar71 = CosMar[70];
                                dtoCosMar.CosMar72 = CosMar[71];
                                dtoCosMar.CosMar73 = CosMar[72];
                                dtoCosMar.CosMar74 = CosMar[73];
                                dtoCosMar.CosMar75 = CosMar[74];
                                dtoCosMar.CosMar76 = CosMar[75];
                                dtoCosMar.CosMar77 = CosMar[76];
                                dtoCosMar.CosMar78 = CosMar[77];
                                dtoCosMar.CosMar79 = CosMar[78];
                                dtoCosMar.CosMar80 = CosMar[79];
                                dtoCosMar.CosMar81 = CosMar[80];
                                dtoCosMar.CosMar82 = CosMar[81];
                                dtoCosMar.CosMar83 = CosMar[82];
                                dtoCosMar.CosMar84 = CosMar[83];
                                dtoCosMar.CosMar85 = CosMar[84];
                                dtoCosMar.CosMar86 = CosMar[85];
                                dtoCosMar.CosMar87 = CosMar[86];
                                dtoCosMar.CosMar88 = CosMar[87];
                                dtoCosMar.CosMar89 = CosMar[88];
                                dtoCosMar.CosMar90 = CosMar[89];
                                dtoCosMar.CosMar91 = CosMar[90];
                                dtoCosMar.CosMar92 = CosMar[91];
                                dtoCosMar.CosMar93 = CosMar[92];
                                dtoCosMar.CosMar94 = CosMar[93];
                                dtoCosMar.CosMar95 = CosMar[94];
                                dtoCosMar.CosMar96 = CosMar[95];
                                dtoCosMar.CosMarUserName = User.Identity.Name;
                                dtoCosMar.CosMarPromedioDia = (dPromedioDiaCosMar / 96);

                                modelCosMar.IdCostoMarginal = (new CostoMarginalAppServicio()).SaveCostoMarginal(dtoCosMar);
                                dtoCosMar = null;
                            }
                            iColumna += 48;
                            if (bSalir) break;
                        }
                    }
                    else
                    {   //No existen fila de datos 
                        rCelda = rRange.Worksheet.Cells[i, 2];
                        if (rCelda.Value != null)
                        {
                            sNomBarra = rCelda.Value.ToString();
                            sMensaje = "No existe el codigo para la barra " + sNomBarra;
                            bSalir = true;
                        }
                        break;
                    }
                    sFlagBase = "N"; // Cambiamos el Flag pues las siguientes filas no son Base
                }

                xlPackage.Dispose();
                rRange = null;

                //if (list.Count !=0)
                //{   //Muestra un listado de errores en pantalla
                //    TempData["ListaLog"] = list;
                //}
                return Json(sMensaje);
            }
            catch (Exception ex)
            {
                //return Json("-1");
                return Json(ex.Message);
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

        public ActionResult getListCostoMarginal(int periodo, int vers)
        {
            CostoMarginalModel CosMararginalModel = new CostoMarginalModel();
            CosMararginalModel.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginal(periodo, vers);
            var listBarr = CosMararginalModel.ListaCostoMarginal;
            return Json(new { dataBar = listBarr }, JsonRequestBehavior.AllowGet);
        }

        //Grafica
        public ActionResult FetchGraphData(int periodo, string barcodi)
        {
            CostoMarginalModel CosMararginalModel = new CostoMarginalModel();
            CosMararginalModel.ListaCostoMarginal = (new CostoMarginalAppServicio()).BuscarCostoMarginal(periodo, barcodi);
            var dataBarProm = CosMararginalModel.ListaCostoMarginal;
            var nombreBar = dataBarProm[0].CosMarBarraTransferencia.ToString();
            return Json(new { dataProm = dataBarProm, dataNomb = nombreBar }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerarCostoMarginal(string sPericodi, int vers)
        {
            int iResultado = 1;
            Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(Int32.Parse(sPericodi));
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                CostoMarginalModel model = new CostoMarginalModel();
                model.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListNombreBarraTransferencia(iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalExcel);
                }

                //Agregamos el intervalo de fechas
                string sMes = iMesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var Fecha = "01/" + sMes + "/" + iAnioCodi;
                var dates = new List<DateTime>();
                var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                var dateFin = dateIni.AddMonths(1);

                dateIni = dateIni.AddMinutes(15);
                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                {
                    dates.Add(dt);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COSTOMARGINAL");
                    if (ws != null)
                    {
                        ws.Cells[1, 1].Value = "S/./kWh";

                        //Agregamos la primera columna
                        int row = 2;
                        foreach (var item in dates)
                        {
                            ws.Cells[row, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row++;
                        }
                        ExcelRange rg = ws.Cells[1, 1, row - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Agreamos el resto del archivo
                        int colum = 2;
                        foreach (var item in model.ListaCostoMarginal)
                        {
                            row = 1;
                            //Agregamos la cabecera de la columna
                            ws.Cells[row, colum].Value = item.CosMarBarraTransferencia;
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            CostoMarginalModel modelDetalle = new CostoMarginalModel();
                            modelDetalle.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(item.BarrCodi, iPeriCodi, iVersion);

                            foreach (var item1 in modelDetalle.ListaCostoMarginal)
                            {
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar1 != null) ? item1.CosMar1 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar2 != null) ? item1.CosMar2 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar3 != null) ? item1.CosMar3 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar4 != null) ? item1.CosMar4 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar5 != null) ? item1.CosMar5 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar6 != null) ? item1.CosMar6 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar7 != null) ? item1.CosMar7 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar8 != null) ? item1.CosMar8 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar9 != null) ? item1.CosMar9 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar10 != null) ? item1.CosMar10 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar11 != null) ? item1.CosMar11 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar12 != null) ? item1.CosMar12 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar13 != null) ? item1.CosMar13 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar14 != null) ? item1.CosMar14 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar15 != null) ? item1.CosMar15 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar16 != null) ? item1.CosMar16 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar17 != null) ? item1.CosMar17 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar18 != null) ? item1.CosMar18 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar19 != null) ? item1.CosMar19 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar20 != null) ? item1.CosMar20 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar21 != null) ? item1.CosMar21 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar22 != null) ? item1.CosMar22 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar23 != null) ? item1.CosMar23 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar24 != null) ? item1.CosMar24 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar25 != null) ? item1.CosMar25 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar26 != null) ? item1.CosMar26 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar27 != null) ? item1.CosMar27 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar28 != null) ? item1.CosMar28 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar29 != null) ? item1.CosMar29 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar30 != null) ? item1.CosMar30 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar31 != null) ? item1.CosMar31 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar32 != null) ? item1.CosMar32 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar33 != null) ? item1.CosMar33 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar34 != null) ? item1.CosMar34 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar35 != null) ? item1.CosMar35 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar36 != null) ? item1.CosMar36 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar37 != null) ? item1.CosMar37 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar38 != null) ? item1.CosMar38 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar39 != null) ? item1.CosMar39 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar40 != null) ? item1.CosMar40 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar41 != null) ? item1.CosMar41 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar42 != null) ? item1.CosMar42 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar43 != null) ? item1.CosMar43 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar44 != null) ? item1.CosMar44 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar45 != null) ? item1.CosMar45 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar46 != null) ? item1.CosMar46 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar47 != null) ? item1.CosMar47 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar48 != null) ? item1.CosMar48 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar49 != null) ? item1.CosMar49 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar50 != null) ? item1.CosMar50 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar51 != null) ? item1.CosMar51 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar52 != null) ? item1.CosMar52 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar53 != null) ? item1.CosMar53 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar54 != null) ? item1.CosMar54 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar55 != null) ? item1.CosMar55 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar56 != null) ? item1.CosMar56 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar57 != null) ? item1.CosMar57 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar58 != null) ? item1.CosMar58 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar59 != null) ? item1.CosMar59 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar60 != null) ? item1.CosMar60 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar61 != null) ? item1.CosMar61 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar62 != null) ? item1.CosMar62 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar63 != null) ? item1.CosMar63 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar64 != null) ? item1.CosMar64 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar65 != null) ? item1.CosMar65 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar66 != null) ? item1.CosMar66 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar67 != null) ? item1.CosMar67 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar68 != null) ? item1.CosMar68 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar69 != null) ? item1.CosMar69 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar70 != null) ? item1.CosMar70 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar71 != null) ? item1.CosMar71 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar72 != null) ? item1.CosMar72 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar73 != null) ? item1.CosMar73 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar74 != null) ? item1.CosMar74 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar75 != null) ? item1.CosMar75 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar76 != null) ? item1.CosMar76 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar77 != null) ? item1.CosMar77 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar78 != null) ? item1.CosMar78 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar79 != null) ? item1.CosMar79 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar80 != null) ? item1.CosMar80 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar81 != null) ? item1.CosMar81 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar82 != null) ? item1.CosMar82 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar83 != null) ? item1.CosMar83 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar84 != null) ? item1.CosMar84 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar85 != null) ? item1.CosMar85 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar86 != null) ? item1.CosMar86 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar87 != null) ? item1.CosMar87 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar88 != null) ? item1.CosMar88 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar89 != null) ? item1.CosMar89 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar90 != null) ? item1.CosMar90 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar91 != null) ? item1.CosMar91 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar92 != null) ? item1.CosMar92 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar93 != null) ? item1.CosMar93 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar94 != null) ? item1.CosMar94 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar95 != null) ? item1.CosMar95 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar96 != null) ? item1.CosMar96 : Decimal.Zero;
                            }
                            colum++;
                        }
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.AutoFitColumns();//Ajustar columnas
                        //Cabecera
                        rg = ws.Cells[1, 1, 1, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        //Border por celda
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
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
        public virtual ActionResult AbrirCostoMarginal()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCostoMarginalExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCostoMarginalExcel);
        }

        [HttpPost]
        public JsonResult GenerarCMBarra(string sPericodi, string sBarrcodi, int vers)
        {
            int iResultado = 1;
            Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(Int32.Parse(sPericodi));
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;

            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();
                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalExcel);
                }

                //Agregamos el intervalo de fechas
                string sMes = iMesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var Fecha = "01/" + sMes + "/" + iAnioCodi;
                var dates = new List<DateTime>();
                var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                var dateFin = dateIni.AddMonths(1);

                dateIni = dateIni.AddMinutes(15);
                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                {
                    dates.Add(dt);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COSTOMARGINAL");
                    if (ws != null)
                    {
                        ws.Cells[1, 1].Value = "S/./kWh";

                        //Agregamos la primera columna
                        int row = 2;
                        foreach (var item in dates)
                        {
                            ws.Cells[row, 1].Value = item.ToString("dd/MM/yyyy HH:mm:ss");
                            row++;
                        }
                        ExcelRange rg = ws.Cells[1, 1, row - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Agreamos el resto del archivo
                        int colum = 2;
                        BarraModel modelBarra = new BarraModel();
                        modelBarra.Entidad = (new BarraAppServicio()).GetByIdBarra(Int32.Parse(sBarrcodi));

                        if (modelBarra.Entidad != null)
                        {
                            row = 1;
                            //Agregamos la cabecera de la columna
                            ws.Cells[row, colum].Value = modelBarra.Entidad.BarrNombBarrTran;
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            CostoMarginalModel modelDetalle = new CostoMarginalModel();
                            modelDetalle.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(modelBarra.Entidad.BarrCodi, iPeriCodi, iVersion);

                            foreach (var item1 in modelDetalle.ListaCostoMarginal)
                            {
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar1 != null) ? item1.CosMar1 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar2 != null) ? item1.CosMar2 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar3 != null) ? item1.CosMar3 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar4 != null) ? item1.CosMar4 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar5 != null) ? item1.CosMar5 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar6 != null) ? item1.CosMar6 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar7 != null) ? item1.CosMar7 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar8 != null) ? item1.CosMar8 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar9 != null) ? item1.CosMar9 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar10 != null) ? item1.CosMar10 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar11 != null) ? item1.CosMar11 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar12 != null) ? item1.CosMar12 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar13 != null) ? item1.CosMar13 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar14 != null) ? item1.CosMar14 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar15 != null) ? item1.CosMar15 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar16 != null) ? item1.CosMar16 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar17 != null) ? item1.CosMar17 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar18 != null) ? item1.CosMar18 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar19 != null) ? item1.CosMar19 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar20 != null) ? item1.CosMar20 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar21 != null) ? item1.CosMar21 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar22 != null) ? item1.CosMar22 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar23 != null) ? item1.CosMar23 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar24 != null) ? item1.CosMar24 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar25 != null) ? item1.CosMar25 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar26 != null) ? item1.CosMar26 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar27 != null) ? item1.CosMar27 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar28 != null) ? item1.CosMar28 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar29 != null) ? item1.CosMar29 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar30 != null) ? item1.CosMar30 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar31 != null) ? item1.CosMar31 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar32 != null) ? item1.CosMar32 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar33 != null) ? item1.CosMar33 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar34 != null) ? item1.CosMar34 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar35 != null) ? item1.CosMar35 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar36 != null) ? item1.CosMar36 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar37 != null) ? item1.CosMar37 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar38 != null) ? item1.CosMar38 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar39 != null) ? item1.CosMar39 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar40 != null) ? item1.CosMar40 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar41 != null) ? item1.CosMar41 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar42 != null) ? item1.CosMar42 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar43 != null) ? item1.CosMar43 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar44 != null) ? item1.CosMar44 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar45 != null) ? item1.CosMar45 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar46 != null) ? item1.CosMar46 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar47 != null) ? item1.CosMar47 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar48 != null) ? item1.CosMar48 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar49 != null) ? item1.CosMar49 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar50 != null) ? item1.CosMar50 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar51 != null) ? item1.CosMar51 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar52 != null) ? item1.CosMar52 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar53 != null) ? item1.CosMar53 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar54 != null) ? item1.CosMar54 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar55 != null) ? item1.CosMar55 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar56 != null) ? item1.CosMar56 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar57 != null) ? item1.CosMar57 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar58 != null) ? item1.CosMar58 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar59 != null) ? item1.CosMar59 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar60 != null) ? item1.CosMar60 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar61 != null) ? item1.CosMar61 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar62 != null) ? item1.CosMar62 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar63 != null) ? item1.CosMar63 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar64 != null) ? item1.CosMar64 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar65 != null) ? item1.CosMar65 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar66 != null) ? item1.CosMar66 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar67 != null) ? item1.CosMar67 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar68 != null) ? item1.CosMar68 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar69 != null) ? item1.CosMar69 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar70 != null) ? item1.CosMar70 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar71 != null) ? item1.CosMar71 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar72 != null) ? item1.CosMar72 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar73 != null) ? item1.CosMar73 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar74 != null) ? item1.CosMar74 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar75 != null) ? item1.CosMar75 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar76 != null) ? item1.CosMar76 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar77 != null) ? item1.CosMar77 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar78 != null) ? item1.CosMar78 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar79 != null) ? item1.CosMar79 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar80 != null) ? item1.CosMar80 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar81 != null) ? item1.CosMar81 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar82 != null) ? item1.CosMar82 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar83 != null) ? item1.CosMar83 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar84 != null) ? item1.CosMar84 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar85 != null) ? item1.CosMar85 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar86 != null) ? item1.CosMar86 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar87 != null) ? item1.CosMar87 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar88 != null) ? item1.CosMar88 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar89 != null) ? item1.CosMar89 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar90 != null) ? item1.CosMar90 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar91 != null) ? item1.CosMar91 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar92 != null) ? item1.CosMar92 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar93 != null) ? item1.CosMar93 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar94 != null) ? item1.CosMar94 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar95 != null) ? item1.CosMar95 : Decimal.Zero;
                                row++;
                                ws.Cells[row, colum].Value = (item1.CosMar96 != null) ? item1.CosMar96 : Decimal.Zero;
                            }
                            colum++;
                        }
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.AutoFitColumns();//Ajustar columnas
                        //Cabecera
                        rg = ws.Cells[1, 1, 1, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        //Border por celda
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
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
        public virtual ActionResult AbrirCMBarra()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCostoMarginalExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCostoMarginalExcel);
        }

        [HttpPost]
        public JsonResult GenerarPorBarra(string sPericodi, string sBarrcodi, int vers)
        {
            int iResultado = 1;
            Int32 iVersion = vers;
            PeriodoModel modelPeri = new PeriodoModel();
            modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(Int32.Parse(sPericodi));
            int iPeriCodi = modelPeri.Entidad.PeriCodi;
            int iAnioCodi = modelPeri.Entidad.AnioCodi;
            int iMesCodi = modelPeri.Entidad.MesCodi;
            int iDiasMes = DateTime.DaysInMonth(iAnioCodi, iMesCodi); // Extrae el numero de dias en el mes

            BarraModel modelBarra = new BarraModel();
            modelBarra.Entidad = (new BarraAppServicio()).GetByIdBarra(Int32.Parse(sBarrcodi));
            int iBarrCodi = modelBarra.Entidad.BarrCodi;
            try
            {
                string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString();

                CostoMarginalModel model = new CostoMarginalModel();
                model.ListaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(iBarrCodi, iPeriCodi, iVersion);

                FileInfo newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalBarraExcel);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + Funcion.NombreReporteCostoMarginalBarraExcel);
                }

                //Agregamos el intervalo de fechas
                string sMes = iMesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var Fecha = "01/" + sMes + "/" + iAnioCodi;
                var dates = new List<DateTime>();
                var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                var dateFin = dateIni.AddDays(1);

                dateIni = dateIni.AddMinutes(15);
                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                {
                    dates.Add(dt);
                }

                int row;
                int colum = 2;
                int iDia = 1;
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COSTOMARGINAL");
                    if (ws != null)
                    {
                        ws.Cells[1, 1].Value = modelBarra.Entidad.BarrNombBarrTran; //Nombre de la barra de transferencia
                        row = 2;
                        foreach (var item in dates)
                        {
                            ws.Cells[row, 1].Value = item.ToString("HH:mm:ss");
                            row++;
                        }
                        ExcelRange rg = ws.Cells[1, 1, row - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        foreach (var item1 in model.ListaCostoMarginal)
                        {
                            row = 1;
                            ws.Cells[row, colum].Value = iDia.ToString();
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.000000";
                            ws.Cells[row, colum].Style.Numberformat.Format = "0";
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar1 != null) ? item1.CosMar1 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar2 != null) ? item1.CosMar2 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar3 != null) ? item1.CosMar3 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar4 != null) ? item1.CosMar4 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar5 != null) ? item1.CosMar5 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar6 != null) ? item1.CosMar6 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar7 != null) ? item1.CosMar7 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar8 != null) ? item1.CosMar8 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar9 != null) ? item1.CosMar9 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar10 != null) ? item1.CosMar10 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar11 != null) ? item1.CosMar11 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar12 != null) ? item1.CosMar12 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar13 != null) ? item1.CosMar13 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar14 != null) ? item1.CosMar14 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar15 != null) ? item1.CosMar15 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar16 != null) ? item1.CosMar16 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar17 != null) ? item1.CosMar17 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar18 != null) ? item1.CosMar18 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar19 != null) ? item1.CosMar19 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar20 != null) ? item1.CosMar20 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar21 != null) ? item1.CosMar21 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar22 != null) ? item1.CosMar22 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar23 != null) ? item1.CosMar23 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar24 != null) ? item1.CosMar24 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar25 != null) ? item1.CosMar25 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar26 != null) ? item1.CosMar26 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar27 != null) ? item1.CosMar27 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar28 != null) ? item1.CosMar28 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar29 != null) ? item1.CosMar29 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar30 != null) ? item1.CosMar30 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar31 != null) ? item1.CosMar31 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar32 != null) ? item1.CosMar32 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar33 != null) ? item1.CosMar33 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar34 != null) ? item1.CosMar34 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar35 != null) ? item1.CosMar35 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar36 != null) ? item1.CosMar36 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar37 != null) ? item1.CosMar37 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar38 != null) ? item1.CosMar38 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar39 != null) ? item1.CosMar39 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar40 != null) ? item1.CosMar40 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar41 != null) ? item1.CosMar41 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar42 != null) ? item1.CosMar42 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar43 != null) ? item1.CosMar43 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar44 != null) ? item1.CosMar44 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar45 != null) ? item1.CosMar45 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar46 != null) ? item1.CosMar46 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar47 != null) ? item1.CosMar47 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar48 != null) ? item1.CosMar48 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar49 != null) ? item1.CosMar49 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar50 != null) ? item1.CosMar50 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar51 != null) ? item1.CosMar51 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar52 != null) ? item1.CosMar52 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar53 != null) ? item1.CosMar53 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar54 != null) ? item1.CosMar54 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar55 != null) ? item1.CosMar55 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar56 != null) ? item1.CosMar56 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar57 != null) ? item1.CosMar57 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar58 != null) ? item1.CosMar58 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar59 != null) ? item1.CosMar59 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar60 != null) ? item1.CosMar60 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar61 != null) ? item1.CosMar61 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar62 != null) ? item1.CosMar62 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar63 != null) ? item1.CosMar63 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar64 != null) ? item1.CosMar64 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar65 != null) ? item1.CosMar65 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar66 != null) ? item1.CosMar66 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar67 != null) ? item1.CosMar67 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar68 != null) ? item1.CosMar68 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar69 != null) ? item1.CosMar69 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar70 != null) ? item1.CosMar70 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar71 != null) ? item1.CosMar71 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar72 != null) ? item1.CosMar72 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar73 != null) ? item1.CosMar73 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar74 != null) ? item1.CosMar74 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar75 != null) ? item1.CosMar75 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar76 != null) ? item1.CosMar76 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar77 != null) ? item1.CosMar77 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar78 != null) ? item1.CosMar78 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar79 != null) ? item1.CosMar79 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar80 != null) ? item1.CosMar80 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar81 != null) ? item1.CosMar81 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar82 != null) ? item1.CosMar82 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar83 != null) ? item1.CosMar83 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar84 != null) ? item1.CosMar84 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar85 != null) ? item1.CosMar85 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar86 != null) ? item1.CosMar86 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar87 != null) ? item1.CosMar87 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar88 != null) ? item1.CosMar88 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar89 != null) ? item1.CosMar89 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar90 != null) ? item1.CosMar90 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar91 != null) ? item1.CosMar91 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar92 != null) ? item1.CosMar92 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar93 != null) ? item1.CosMar93 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar94 != null) ? item1.CosMar94 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar95 != null) ? item1.CosMar95 : Decimal.Zero;
                            row++;
                            ws.Cells[row, colum].Value = (item1.CosMar96 != null) ? item1.CosMar96 : Decimal.Zero;
                            colum++;
                            iDia++;
                        }
                        //Ajustar columnas
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.AutoFitColumns();//Ajustar columnas
                        //Cabecera
                        rg = ws.Cells[1, 1, 1, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;
                        //Border por celda
                        rg = ws.Cells[1, 1, row, colum - 1];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
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
        public virtual ActionResult AbrirPorBarra()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[Funcion.ReporteDirectorio].ToString() + Funcion.NombreReporteCostoMarginalBarraExcel;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, Constantes.AppExcel, sFecha + "_" + Funcion.NombreReporteCostoMarginalBarraExcel);
        }


        //ASSETEC 20181115
        //Importa la información de si_costomarginal
        [HttpPost]
        public JsonResult Copiarsgocoes(int pericodi, int version)
        {
            string indicador = "1";
            try
            {
                //Eliminamos el proceso de valorización vigente en caso existiese.
                string sMensaje = EliminarProcesoValorizacion(pericodi, version);
                if (!sMensaje.Equals("1")) return Json(sMensaje);

                //ELIMINAMOS LA INFORMACION EN CASCADA
                //Eliminamos la version anterior de la tabla CostoMarginal por periodo y version
                CostoMarginalDTO dtoCosMar = new CostoMarginalDTO();
                //TRN_COSTO_MARGINAL
                dtoCosMar.PeriCodi = (new CostoMarginalAppServicio()).DeleteListaCostoMarginal(pericodi, version);
                //Eliminamos la version anterior de la tabla FactorPerdida por periodo y version
                FactorPerdidaDTO dtoFacPer = new FactorPerdidaDTO();
                dtoFacPer.PeriCodi = (new FactorPerdidaAppServicio()).DeleteListaFactorPerdida(pericodi, version);
                //ASSETEC 202002
                //--------------------------------------------------------------------------------------------------
                //Eliminamos la tabla temporal trn_costo_marginal_tmp
                (new FactorPerdidaAppServicio()).DeleteCMTMP();
                List<ReporteLogDTO> listLogError = new List<ReporteLogDTO>();

                //Insertamos los Factores de proporción
                string suser = User.Identity.Name;
                PeriodoModel modelPeri = new PeriodoModel();
                modelPeri.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                string sAnioMes = modelPeri.Entidad.PeriAnioMes.ToString();
                int iDiasMes = DateTime.DaysInMonth(modelPeri.Entidad.AnioCodi, modelPeri.Entidad.MesCodi); // Extrae el numero de dias en el mes
                //Poblamos la tabla temporal: trn_costo_marginal_tmp
                List<FactorPerdidaDTO> listaBarras = (new FactorPerdidaAppServicio()).ListBarrasSiCostMarg(sAnioMes);
                //
                foreach (FactorPerdidaDTO dtoBarra in listaBarras)
                {
                    //Por cada barra pobamos la tabla trn_costo_marginal_tmp
                    (new FactorPerdidaAppServicio()).SaveCostMargTmp(dtoBarra.BarrCodi, iDiasMes, sAnioMes);
                    //la barra al log de errores
                    ReporteLogDTO objR = new ReporteLogDTO();
                    objR.NombreBarra = dtoBarra.FacPerBarrNombre;
                    objR.ListaFecFaltantes = new List<String>();
                        //(new FactorPerdidaAppServicio()).ListFechaXBarraSiCostMarg(sAnioMes, dtoBarra.BarrCodi);
                    listLogError.Add(objR);
                }
                 
                sMensaje = (new FactorPerdidaAppServicio()).CopiarSGOCOES(pericodi, version, suser, sAnioMes);
                if (!sMensaje.Equals("1")) return Json(sMensaje);

                //ASSETEC 202209 - Ajustar intervalos de 15 y 45 minutos.
                sMensaje = (new FactorPerdidaAppServicio()).AjustarCostosMarginales(pericodi, version, suser);
                if (!sMensaje.Equals("1")) return Json(sMensaje);
                //FIN ASSETEC

                //COPIAMOS LAS RENTAS DE CONGESTIÓN
                int iNumRentas = (new FactorPerdidaAppServicio()).CopiarSGOCOESRentasCongestion(pericodi, version, modelPeri.Entidad.PeriAnioMes, suser);
                //--------------------------------------------------------------------------------------------------

                if (listLogError.Count != 0)
                {   //Muestra un listado de errores en pantalla
                    #region AuditoriaProceso
                    PeriodoDTO dtoPeriodo2 = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                    RecalculoDTO dtoRecalculo22 = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, version);

                    VtpAuditoriaProcesoDTO objAuditoria2 = new VtpAuditoriaProcesoDTO();
                    objAuditoria2.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.ImportarCostosMarginales;
                    objAuditoria2.Estdcodi = (int)EVtpEstados.ImportarCostoMarginal;
                    objAuditoria2.Audproproceso = "Importar costos terminales";
                    objAuditoria2.Audprodescripcion = "Se importó los costos marginales con errores " + dtoPeriodo2.PeriNombre + " / " + dtoRecalculo22.RecaNombre + " cant de error : " + listLogError.Count;
                    objAuditoria2.Audprousucreacion = User.Identity.Name;
                    objAuditoria2.Audprofeccreacion = DateTime.Now;

                    _ = this.servicioAuditoria.save(objAuditoria2);

                    #endregion
                    return Json(listLogError);
                }
                #region AuditoriaProceso
                PeriodoDTO dtoPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo2 = (new RecalculoAppServicio()).GetByIdRecalculo(pericodi, version);

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.ImportarCostosMarginales;
                objAuditoria.Estdcodi = (int)EVtpEstados.ImportarCostoMarginal;
                objAuditoria.Audproproceso = "Importar costos terminales";
                objAuditoria.Audprodescripcion = "Se importó los costos marginales para el periodo " + dtoPeriodo.PeriNombre + " / " + dtoRecalculo2.RecaNombre;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                _= this.servicioAuditoria.save(objAuditoria);

                #endregion
            } // try
            catch (Exception e)
            {
                indicador = e.Message;
            }
            return Json(indicador);
        }

        //ASSETEC 20220908
        /// <summary>
        /// Popup de ajuste de intervalos
        /// </summary>
        /// <param name="idPeriodo">Identificador del periodo seleccionado</param>
        /// <param name="idVersion">Identificador de la versión seleccionada</param>
        /// <returns></returns>
        public JsonResult AjustarIntervalos(int idPeriodo, int idVersion)
        {            
            List<string> dias = (new FactorPerdidaAppServicio())
                .ObtenerDiasxPeriodos(idPeriodo);
            List<string> intervalos = (new FactorPerdidaAppServicio())
                .ObtenerIntervalos();
            
            object res = new { dias, intervalos };
            return Json(res);
        }

        /// <summary>
        /// Registra el ajuste ingresado
        /// </summary>
        /// <param name="idPeriodo">Identificador del periodo seleccionado</param>
        /// <param name="idVersion">Identificador de la versión seleccionada</param>
        /// <param name="ajuste">Fecha y hora del ajuste</param>
        /// <returns></returns>
        public JsonResult RegistrarAjusteIntervalo(int idPeriodo, int idVersion, string ajuste)
        {            
            DateTime fecha = DateTime.ParseExact(ajuste,
                "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture);

            //Validando que el intervalo no este previamente registrado
            List<TrnCostoMarginalAjusteDTO> listaAjustes = (new FactorPerdidaAppServicio()).ListarAjusteIntervalo(idPeriodo, idVersion);
            foreach (TrnCostoMarginalAjusteDTO dtoAjuste in listaAjustes)
            {
                if(dtoAjuste.Trncmafecha == fecha)
                {
                    //Intervalo duplicado
                    return Json("El intervalo " + ajuste + " ya se encuentra registrado.");
                }
            }

            TrnCostoMarginalAjusteDTO entity = new TrnCostoMarginalAjusteDTO
            {
                Pericodi = idPeriodo,
                Recacodi = idVersion,
                Trncmafecha = fecha,
                Trncmausucreacion = User.Identity.Name,
                Trncmafeccreacion = DateTime.Now,
            };

            int res = (new FactorPerdidaAppServicio())
                .RegistrarAjusteIntervalo(entity);
            return Json(res);
        }

        /// <summary>
        /// Registra el ajuste ingresado
        /// </summary>
        /// <param name="idPeriodo">Identificador del periodo seleccionado</param>
        /// <param name="idVersion">Identificador de la versión seleccionada</param>
        /// <returns></returns>
        public JsonResult ListarAjusteIntervalo(int idPeriodo, int idVersion)
        {
            List<TrnCostoMarginalAjusteDTO> res = (new FactorPerdidaAppServicio())
                .ListarAjusteIntervalo(idPeriodo, idVersion);
            return Json(res);
        }

        /// <summary>
        /// Elimna un ajuste por intervalo segun identificador
        /// </summary>
        /// <param name="idRegistro">Identificador del registro</param>
        /// <returns></returns>
        public JsonResult EliminarAjusteIntervalo(int idRegistro)
        {
            int res = (new FactorPerdidaAppServicio())
                .EliminarAjusteIntervalo(idRegistro);
            return Json(res);
        }
    }
}
