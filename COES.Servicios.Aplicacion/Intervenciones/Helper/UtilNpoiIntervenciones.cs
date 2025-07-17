using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace COES.Servicios.Aplicacion.Intervenciones.Helper
{
    /// <summary>
    /// Clase para el package NPOI (excel en formato .xls)
    /// </summary>
    public class UtilNpoiIntervenciones
    {
        #region NPOI

        /// <summary>
        /// Generar Reporte XLS
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoProgramacion"></param>
        public static void GenerarManttoRegistro(List<InIntervencionDTO> list, string path, string fileName, DateTime fechaInicio, DateTime fechaFin, int tipoProgramacion)
        {
            try
            {
                //string file = path + fileName;

                //FileInfo fi = new FileInfo(file);
                //// Revisar si existe
                //if (!fi.Exists)
                //{
                //    throw new Exception("Archivo " + file + "No existe");
                //}

                //FileStream excelStream = new FileStream(file, FileMode.Open);
                //IWorkbook wb = new HSSFWorkbook(excelStream);
                //excelStream.Close();

                //wb.MissingCellPolicy = MissingCellPolicy.RETURN_NULL_AND_BLANK;

                //ISheet ws = wb.GetSheetAt(0);
                ////ExcelWorksheet ws = xlPackage.Workbook.Worksheets.First();

                //int row = 9;
                //int numeroItem = 1;

                //int columnNroItem = 1;
                //int columnIntercodsegempr = columnNroItem + 1;
                //int columnEmprNomb = columnIntercodsegempr + 1;
                //int columnAreaNomb = columnEmprNomb + 1;
                //int columnEquiNomb = columnAreaNomb + 1;
                //int columnEquicodi = columnEquiNomb + 1;
                //int columnInterfechaini = columnEquicodi + 1;
                //int columnInterfechafin = columnInterfechaini + 1;
                //int columnInterdescrip = columnInterfechafin + 1;
                //int columnIntermwindispo = columnInterdescrip + 1;
                //int columnInterindispo = columnIntermwindispo + 1;
                //int columnInterinterrup = columnInterindispo + 1;
                //int columnIntersistemaaislado = columnInterinterrup + 1;
                //int columnInterconexionprov = columnIntersistemaaislado + 1;
                //int columnIntNombTipoIntervencion = columnInterconexionprov + 1;
                //int columnIntNombTipoProgramacion = columnIntNombTipoIntervencion + 1;

                //ws.GetRow(2).GetCell(2).SetCellValue(fechaInicio);
                //ws.GetRow(3).GetCell(2).SetCellValue(fechaFin);
                //ws.GetRow(2).GetCell(7).SetCellValue(tipoProgramacion);
                //ws.GetRow(3).GetCell(7).SetCellValue(171);

                //ws.GetRow(4).CreateCell(1).SetCellValue(DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha));// fecha actualización equipos empresas

                //foreach (var item in list)
                //{
                //    if(ws.GetRow(row) == null)
                //        ws.CreateRow(row);

                //    ws.GetRow(row).CreateCell(columnNroItem).SetCellValue(numeroItem);
                //    var rgcolumnNroItem = ws.GetRow(row).GetCell(columnNroItem);

                //    ws.GetRow(row).CreateCell(columnIntercodsegempr).SetCellValue(item.Intercodsegempr ?? string.Empty);

                //    ws.GetRow(row).CreateCell(columnEmprNomb).SetCellValue(item.EmprNomb);
                //    ws.GetRow(row).CreateCell(columnAreaNomb).SetCellValue(item.AreaNomb);
                //    ws.GetRow(row).CreateCell(columnEquiNomb).SetCellValue(item.Equiabrev);
                //    ws.GetRow(row).CreateCell(columnEquicodi).SetCellValue(item.Equicodi);

                //    ws.GetRow(row).CreateCell(columnInterfechaini).SetCellValue(item.Interfechaini);
                //    ws.GetRow(row).CreateCell(columnInterfechafin).SetCellValue(item.Interfechafin);

                //    ws.GetRow(row).CreateCell(columnInterdescrip).SetCellValue(item.Interdescrip);

                //    ws.GetRow(row).CreateCell(columnIntermwindispo).SetCellValue((double)item.Intermwindispo);
                //    var rgcolumnIntermwindispo = ws.GetRow(row).GetCell(columnIntermwindispo);
                //    //rgcolumnIntermwindispo.Style.Font.Bold = false;

                //    ws.GetRow(row).CreateCell(columnInterindispo).SetCellValue(item.InterindispoDesc);
                //    var rgcolumnInterindispo = ws.GetRow(row).GetCell(columnInterindispo);
                //    //rgcolumnInterindispo.Style.Font.Bold = false;

                //    ws.GetRow(row).CreateCell(columnInterinterrup).SetCellValue(item.InterinterrupDesc);
                //    ws.GetRow(row).CreateCell(columnIntersistemaaislado).SetCellValue(item.IntersistemaaisladoDesc);
                //    ws.GetRow(row).CreateCell(columnInterconexionprov).SetCellValue(item.InterconexionprovDesc);

                //    ws.GetRow(row).CreateCell(columnIntNombTipoIntervencion).SetCellValue(item.TipoEvenDesc);

                //    ws.SetColumnWidth(13, 22);
                //    ws.GetRow(row).CreateCell(columnIntNombTipoProgramacion).SetCellValue(item.ClaseProgramacion ?? string.Empty);

                //    row++;
                //    numeroItem++;
                //}

                //using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Write))
                //{
                //    wb.Write(fileStream);
                //    fileStream.Close();
                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generar hoja equipos
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="listEquipos"></param>
        /// <param name="listaempresas"></param>
        public static void ActualizarFileExcelHojaEquipos(string path, string fileName, List<EquipoDTO> listEquipos, List<SiEmpresaDTO> listaempresas)
        {

            try
            {
                //string file = path + fileName;

                //FileInfo fi = new FileInfo(file);
                //// Revisar si existe
                //if (!fi.Exists)
                //{
                //    throw new Exception("Archivo " + file + "No existe");
                //}

                //FileStream excelStream = new FileStream(file, FileMode.Open);
                //IWorkbook wb = new HSSFWorkbook(excelStream);
                //excelStream.Close();
                //wb.MissingCellPolicy = MissingCellPolicy.RETURN_NULL_AND_BLANK;
                //ISheet ws = wb.GetSheetAt(1); // "EQUIPOS"

                //int row = 7;
                //int columnIniDataEq = 1;

                //foreach (var item in listEquipos)
                //{
                //    var rowfila = ws.CreateRow(row);
                //    rowfila.CreateCell(columnIniDataEq++).SetCellValue(item.EQUICODI);

                //    if(item.EMPRCODI == null)
                //        rowfila.CreateCell(columnIniDataEq++);
                //    else
                //        rowfila.CreateCell(columnIniDataEq++).SetCellValue(item.EMPRCODI.Value);

                //    if (item.AREACODI == null)
                //        rowfila.CreateCell(columnIniDataEq++);
                //    else
                //        rowfila.CreateCell(columnIniDataEq++).SetCellValue(item.AREACODI.Value);

                //    if (item.FAMCODI == null)
                //        rowfila.CreateCell(columnIniDataEq++);
                //    else
                //        rowfila.CreateCell(columnIniDataEq++).SetCellValue(item.FAMCODI.Value);

                //    rowfila.CreateCell(columnIniDataEq++); // GRUPOCODI
                //    rowfila.CreateCell(columnIniDataEq++).SetCellValue(item.EQUIABREV);
                //    rowfila.CreateCell(columnIniDataEq++).SetCellValue(item.EQUINOMB);
                //    rowfila.CreateCell(columnIniDataEq++).SetCellValue(item.AREANOMB);
                //    rowfila.CreateCell(columnIniDataEq++).SetCellValue(item.TAREAABREV);

                //    if (item.EQUITENSION == null)
                //        rowfila.CreateCell(columnIniDataEq++);
                //    else
                //        rowfila.CreateCell(columnIniDataEq++).SetCellValue((double)item.EQUITENSION);

                //    if (item.EQUIPOT == null)
                //        rowfila.CreateCell(columnIniDataEq++);
                //    else
                //        rowfila.CreateCell(columnIniDataEq++).SetCellValue((double)item.EQUIPOT);

                //    row++;
                //    columnIniDataEq = 1;
                //}

                //int numeroItem = 1;
                //row = 7;
                //int columnIniDataEmp = 13;

                ////obtener empresas
                //foreach (var item in listaempresas)
                //{
                //    var rowfila = ws.GetRow(row);

                //    rowfila.CreateCell(columnIniDataEmp++).SetCellValue(numeroItem);
                //    rowfila.CreateCell(columnIniDataEmp++).SetCellValue(item.Emprcodi);
                //    rowfila.CreateCell(columnIniDataEmp++).SetCellValue(item.Emprabrev);
                //    rowfila.CreateCell(columnIniDataEmp++).SetCellValue(item.Emprnomb);
                //    row++;
                //    columnIniDataEmp = 13;
                //    numeroItem++;
                //}

                //using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Write))
                //{
                //    wb.Write(fileStream);
                //    fileStream.Close();
                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
    }
}
