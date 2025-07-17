using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class ReporteCostoMarginalAppServicio
    {
        /// <summary>
        /// Permite listar las barras
        /// </summary>
        /// <returns></returns>
        public List<BarraDTO> ListarBarras()
        {
            return FactoryTransferencia.GetBarraRepository().ListBarrasTransferenciaByReporte();
        }

        /// <summary>
        /// Lista las barras del sistema anterior
        /// </summary>
        /// <returns></returns>
        public List<BarraDTO> ListarBarraDTR()
        {
            return FactoryTransferencia.GetBarraDTRRepository().ListarBarraReporteDTR();
        }

        /// <summary>
        /// Metodo que generar el archivo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="barraCodi"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public int ExportarReporteCostoMarginales(int anio, int mes, int barraCodi, string path, string file)
        {
            try
            {
                bool validacion = this.ValidarFecha(anio, mes);

                if (validacion)
                {
                    int periodo = Convert.ToInt32(anio.ToString() + "" + mes.ToString().PadLeft(2, '0'));
                    int version = 1;
                    PeriodoDTO entidadPeriodo = (new PeriodoAppServicio()).GetByAnioMes(periodo);
                    BarraDTO entidadBarra = (new BarraAppServicio()).GetByIdBarra(barraCodi);
                    List<RecalculoDTO> list = FactoryTransferencia.GetRecalculoRepository().List(entidadPeriodo.PeriCodi);
                    if (list.Count > 0) version = list[0].RecaCodi;
                    List<CostoMarginalDTO> listaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(barraCodi, 
                        entidadPeriodo.PeriCodi, version);
                    this.GenerarArchivoCostoMarginal(entidadPeriodo, entidadBarra, listaCostoMarginal, path, file);
                }
                else
                {
                    PeriodoDTO entidadPeriodo = FactoryTransferencia.GetPeriodoDTRRepository().ObtenerPeriodoDTR(anio, mes);
                    BarraDTO entidadBarra = FactoryTransferencia.GetBarraDTRRepository().ObtenerBarraDTR(barraCodi);
                    int version = FactoryTransferencia.GetRecalculoDTRRepository().ObtenerVersionDTR(entidadPeriodo.PeriCodi);
                    List<CostoMarginalDTO> listaCostoMarginal = FactoryTransferencia.GetCostoMarginalDTRRepository().ObtenerReporteCostoMarginalDTR(barraCodi,
                        entidadPeriodo.PeriCodi, version);
                    this.GenerarArchivoCostoMarginal(entidadPeriodo, entidadBarra, listaCostoMarginal, path, file);
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite validar el periodo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public bool ValidarFecha(int anio, int mes)
        {
            bool flag = true;

            if (anio == 2015 && mes <= 7) flag = false;
            if (anio < 2015) flag = false;

            return flag;
        }


        /// <summary>
        /// Permite generar el archivo de costos marginales
        /// </summary>
        /// <param name="version"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="barracodi"></param>
        /// <returns></returns>
        public void GenerarArchivoCostoMarginal(PeriodoDTO periodo, BarraDTO entidadBarra, List<CostoMarginalDTO> listaCostoMarginal, string path, string fileName)
        {           
            int iPeriCodi = periodo.PeriCodi;
            int iAnioCodi = periodo.AnioCodi;
            int iMesCodi = periodo.MesCodi;

            try
            {
                FileInfo newFile = new FileInfo(path + fileName);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + fileName);
                }            
               
                string sMes = iMesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var Fecha = "01/" + sMes + "/" + iAnioCodi;
                var dates = new List<DateTime>();
                var dateIni = DateTime.ParseExact(Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var dateFin = dateIni.AddMonths(1);

                dateIni = dateIni.AddMinutes(15);
                for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                {
                    dates.Add(dt);
                }

                #region Excel

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
                        
                        if (entidadBarra != null)
                        {
                            row = 1;
                            //Agregamos la cabecera de la columna
                            ws.Cells[row, colum].Value = entidadBarra.BarrNombBarrTran;
                            ws.Column(colum).Style.Numberformat.Format = "#,##0.000000";                                                      

                            foreach (var item1 in listaCostoMarginal)
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

                #endregion

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
