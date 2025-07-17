using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace COES.Servicios.Aplicacion.Equipamiento.Helper
{
    public class ExcelDocumentEquipamiento
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ExcelDocumentEquipamiento));

        public ExcelDocumentEquipamiento()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void GenerarListadoEquipos(string ruta, string nombreReporteEquipos, string plantilla, List<EqEquipoDTO> listaEquipos)
        {

            FileInfo newFile = new FileInfo(ruta + nombreReporteEquipos);
            FileInfo template = new FileInfo(ruta + plantilla);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + nombreReporteEquipos);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                int row = 4;
                foreach (var oEquipo in listaEquipos)
                {
                    ws.Cells[row, 2].Value = oEquipo.Equicodi;
                    ws.Cells[row, 2].StyleName = ws.Cells[4, 2].StyleName;

                    ws.Cells[row, 3].Value = oEquipo.TipoEmpresa;
                    ws.Cells[row, 3].StyleName = ws.Cells[4, 2].StyleName;

                    ws.Cells[row, 4].Value = oEquipo.EMPRNOMB;
                    ws.Cells[row, 4].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 5].Value = oEquipo.Famnomb;
                    ws.Cells[row, 5].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 6].Value = oEquipo.AREANOMB;
                    ws.Cells[row, 6].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 7].Value = oEquipo.Equinomb;
                    ws.Cells[row, 7].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 8].Value = oEquipo.Equiabrev;
                    ws.Cells[row, 8].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 9].Value = oEquipo.EstadoDesc;
                    ws.Cells[row, 9].StyleName = ws.Cells[4, 2].StyleName;

                    ws.Cells[row, 10].Value = oEquipo.Lastuser;
                    ws.Cells[row, 10].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 11].Value = oEquipo.Lastdate.HasValue ? oEquipo.Lastdate.Value.ToString(ConstantesBase.FormatoFecha) : "";
                    ws.Cells[row, 11].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 12].Value = oEquipo.UsuarioUpdate;
                    ws.Cells[row, 12].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 13].Value = oEquipo.FechaUpdate.HasValue ? oEquipo.FechaUpdate.Value.ToString(ConstantesBase.FormatoFecha) : "";
                    ws.Cells[row, 13].StyleName = ws.Cells[4, 2].StyleName;
                    row++;
                }

                ws.Column(2).AutoFit();
                ws.Column(3).AutoFit();
                ws.Column(4).AutoFit();
                ws.Column(5).AutoFit();
                ws.Column(6).AutoFit();
                ws.Column(7).AutoFit();
                ws.Column(8).AutoFit();
                ws.Column(9).AutoFit();
                ws.Column(10).AutoFit();
                ws.Column(11).AutoFit();
                ws.Column(12).AutoFit();
                ws.Column(13).AutoFit();
                xlPackage.Save();
            }

        }

        public static void GenerarReporte(List<EqEquipoDTO> listaEquipos, string path, string fileName)
        {
            string file = path + fileName;

            FileInfo fi = new FileInfo(file);
            // Revisar si existe
            if (!fi.Exists)
            {
                throw new Exception("Archivo " + file + "No existe");
            }
            //string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReportePropiedades].ToString();
            //FileInfo newFile = new FileInfo(ruta + NombreArchivo.ReportePropiedad);
            //FileInfo template = new FileInfo(ruta + Constantes.PlantillaReportePropiedadesEquipos);

            //if (newFile.Exists)
            //{
            //    newFile.Delete();
            //    newFile = new FileInfo(ruta + NombreArchivo.ReportePropiedad);
            //}
            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PLANTILLA"];
                //ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                int iColumna = 13;
                int iColumSu = iColumna + 7;

                int iColumnUni = 13;
                int iColumFecVigen = iColumnUni + 1;
                int iColumValor = iColumFecVigen + 1;
                int iColumValCero = iColumValor + 1;
                int iColumComentario = iColumValCero + 1;
                int iColumSustento = iColumComentario + 1;
                int iColumUsuario = iColumSustento + 1;
                int iColumFecMod = iColumUsuario + 1;

                //Nombres de filas para propiedades
                ws.Cells[2, iColumna - 1].Value = "CÓDIGO PROPIEDAD";
                ws.Cells[3, iColumna - 1].Value = "NOMBRE PROPIEDAD";
                ws.Cells[4, iColumna - 1].Value = "NOMBRE FICHA TÉCNICA";
                ws.Cells[2, iColumna - 1, 4, iColumna - 1].Style.Font.Bold = true;

                foreach (var prop in listaEquipos[0].PropiedadesEquipo)
                {

                    ws.Cells[2, iColumna].Value = prop.Propcodi; // 3 
                    ws.Cells[2, iColumna, 2, iColumSu].Merge = true;
                    ws.Cells[2, iColumna, 2, iColumSu].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    ws.Cells[2, iColumna, 2, iColumSu].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                    //Nombre propiedad
                    ws.Cells[3, iColumna].Value = prop.Propnomb.ToUpperInvariant().Trim(); //4 
                    ws.Cells[3, iColumna, 3, iColumSu].Merge = true;

                    ws.Cells[3, iColumna].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[3, iColumna].Style.Font.Bold = true;
                    ws.Cells[3, iColumna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[3, iColumna].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    ws.Cells[3, iColumna, 4, iColumSu].Style.Border.BorderAround(ExcelBorderStyle.Thick);

                    // Nombre ficha técnica
                    ws.Cells[4, iColumna].Value = prop.Propnombficha.ToUpperInvariant().Trim();
                    ws.Cells[4, iColumna].Style.Font.Bold = true;

                    ws.Cells[5, iColumnUni].Value = "Unidad";
                    ws.Cells[5, iColumnUni].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[5, iColumnUni].Style.Font.Bold = true;
                    ws.Cells[5, iColumnUni].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[5, iColumnUni].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                    ws.Cells[5, iColumFecVigen].Value = "Fecha de Vigencia";
                    ws.Cells[5, iColumFecVigen].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[5, iColumFecVigen].Style.Font.Bold = true;
                    ws.Cells[5, iColumFecVigen].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[5, iColumFecVigen].Style.Fill.BackgroundColor.SetColor(Color.Orange);


                    ws.Cells[5, iColumValor].Value = "Valor";
                    ws.Cells[5, iColumValor].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[5, iColumValor].Style.Font.Bold = true;
                    ws.Cells[5, iColumValor].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[5, iColumValor].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                    ws.Cells[5, iColumValCero].Value = "Valor cero(0) correcto";
                    ws.Cells[5, iColumValCero].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[5, iColumValCero].Style.Font.Bold = true;
                    ws.Cells[5, iColumValCero].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[5, iColumValCero].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                    ws.Cells[5, iColumComentario].Value = "Comentario";
                    ws.Cells[5, iColumComentario].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[5, iColumComentario].Style.Font.Bold = true;
                    ws.Cells[5, iColumComentario].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[5, iColumComentario].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                    ws.Cells[5, iColumSustento].Value = "Sustento";
                    ws.Cells[5, iColumSustento].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[5, iColumSustento].Style.Font.Bold = true;
                    ws.Cells[5, iColumSustento].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[5, iColumSustento].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                    ws.Cells[5, iColumUsuario].Value = "Usuario Modificación";
                    ws.Cells[5, iColumUsuario].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[5, iColumUsuario].Style.Font.Bold = true;
                    ws.Cells[5, iColumUsuario].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[5, iColumUsuario].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                    ws.Cells[5, iColumFecMod].Value = "Fecha Modificación";
                    ws.Cells[5, iColumFecMod].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[5, iColumFecMod].Style.Font.Bold = true;
                    ws.Cells[5, iColumFecMod].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[5, iColumFecMod].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                    ws.Column(iColumFecVigen).Width = 17;
                    ws.Column(iColumValCero).Width = 20;
                    ws.Column(iColumUsuario).Width = 15;
                    ws.Column(iColumFecMod).Width = 18;

                    iColumnUni += 8;
                    iColumFecVigen += 8;
                    iColumValor += 8;
                    iColumValCero += 8;
                    iColumComentario += 8;
                    iColumSustento += 8;
                    iColumUsuario += 8;
                    iColumFecMod += 8;
                    iColumSu += 8;
                    iColumna += 8;
                }
                int row = 6;
                foreach (var oEquipo in listaEquipos)
                {

                    ws.Cells[row, 2].Value = oEquipo.Lastdate.HasValue ? oEquipo.Lastdate.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
                    ws.Cells[row, 2].StyleName = ws.Cells[5, 2].StyleName;
                    ws.Column(2).Width = 18;

                    ws.Cells[row, 3].Value = oEquipo.Lastuser;
                    ws.Cells[row, 3].StyleName = ws.Cells[5, 2].StyleName;

                    ws.Cells[row, 4].Value = oEquipo.Equicodi;
                    ws.Cells[row, 4].StyleName = ws.Cells[5, 2].StyleName;

                    ws.Cells[row, 5].Value = oEquipo.TipoEmpresa;
                    ws.Cells[row, 5].StyleName = ws.Cells[5, 2].StyleName;

                    ws.Cells[row, 6].Value = oEquipo.EMPRNOMB;
                    ws.Cells[row, 6].StyleName = ws.Cells[5, 2].StyleName;
                    ws.Cells[row, 7].Value = oEquipo.Famnomb;
                    ws.Cells[row, 7].StyleName = ws.Cells[5, 2].StyleName;
                    ws.Cells[row, 8].Value = oEquipo.AREANOMB;
                    ws.Cells[row, 8].StyleName = ws.Cells[5, 2].StyleName;
                    ws.Cells[row, 9].Value = oEquipo.Equinomb;
                    ws.Cells[row, 9].StyleName = ws.Cells[5, 2].StyleName;
                    ws.Cells[row, 10].Value = oEquipo.Equiabrev;
                    ws.Cells[row, 10].StyleName = ws.Cells[5, 2].StyleName;
                    ws.Cells[row, 11].Value = oEquipo.EstadoDesc;
                    ws.Cells[row, 11].StyleName = ws.Cells[5, 2].StyleName;

                    ws.Cells[row, 12].Value = oEquipo.Osinergcodi;
                    ws.Cells[row, 12].StyleName = ws.Cells[5, 2].StyleName;


                    iColumna = 13;
                    iColumFecVigen = iColumna + 1;
                    iColumValor = iColumFecVigen + 1;
                    iColumValCero = iColumValor + 1;
                    iColumComentario = iColumValCero + 1;
                    iColumSustento = iColumComentario + 1;
                    iColumUsuario = iColumSustento + 1;
                    iColumFecMod = iColumUsuario + 1;

                    foreach (var prop in oEquipo.PropiedadesEquipo)
                    {
                        ws.Cells[row, iColumna].Value = prop.Propunidad;
                        //ws.Cells[row, iColumFecVigen].Style.Numberformat.Format = "dd/MM/yyyy";
                        ws.Cells[row, iColumFecVigen].Value = prop.Fechapropequi != null ? prop.Fechapropequi.Value.Date.ToString(ConstantesAppServicio.FormatoFecha) : "";
                        ws.Cells[row, iColumValor].Value = prop.Valor;
                        ws.Cells[row, iColumValCero].Value = prop.Valor == "0" ? prop.Propequicheckcero == 1 ? "SI" : "NO" : null;
                        ws.Cells[row, iColumComentario].Value = prop.Propequicomentario;
                        ws.Cells[row, iColumSustento].Value = prop.Propequisustento;
                        ws.Cells[row, iColumUsuario].Value = prop.Propequiusucreacion;
                        ws.Cells[row, iColumFecMod].Value = prop.Propequifeccreacion.HasValue ? prop.Propequifeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";

                        //ws.Cells[row, iColumna].StyleName = ws.Cells[5, 13].StyleName;
                        iColumFecVigen += 8;
                        iColumValor += 8;
                        iColumValCero += 8;
                        iColumComentario += 8;
                        iColumSustento += 8;
                        iColumUsuario += 8;
                        iColumFecMod += 8;
                        iColumna += 8;
                    }
                    row++;
                }

                xlPackage.Save();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaEquipos"></param>
        public static void GenerarDatosPropiedades(string ruta, string nombreReporteEquipos, string plantilla, List<EqEquipoDTO> listaEquipos)
        {
            FileInfo newFile = new FileInfo(ruta + nombreReporteEquipos);
            FileInfo template = new FileInfo(ruta + plantilla);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + nombreReporteEquipos);
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                int iColumna = 13;
                foreach (var prop in listaEquipos[0].PropiedadesEquipo)
                {
                    ws.Cells[2, iColumna].Value = prop.Propcodi;
                    ws.Cells[3, iColumna].Value = prop.Propnomb.ToUpperInvariant().Trim();
                    ws.Cells[3, iColumna].Style.Font.Color.SetColor(Color.White);// = ws.Cells[3, 10].Style.Font.Color.SetColor(Color.White);
                    ws.Cells[3, iColumna].Style.Font.Bold = true;
                    ws.Cells[3, iColumna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[3, iColumna].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    iColumna++;
                }
                int row = 4;
                foreach (var oEquipo in listaEquipos)
                {

                    ws.Cells[row, 2].Value = oEquipo.Lastdate.HasValue ? oEquipo.Lastdate.Value.ToString(ConstantesBase.FormatoFechaPE) : "";
                    ws.Cells[row, 2].StyleName = ws.Cells[4, 2].StyleName;

                    ws.Cells[row, 3].Value = oEquipo.Lastuser;
                    ws.Cells[row, 3].StyleName = ws.Cells[4, 2].StyleName;

                    ws.Cells[row, 4].Value = oEquipo.Equicodi;
                    ws.Cells[row, 4].StyleName = ws.Cells[4, 2].StyleName;

                    ws.Cells[row, 5].Value = oEquipo.TipoEmpresa;
                    ws.Cells[row, 5].StyleName = ws.Cells[4, 2].StyleName;

                    ws.Cells[row, 6].Value = oEquipo.EMPRNOMB;
                    ws.Cells[row, 6].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 7].Value = oEquipo.Famnomb;
                    ws.Cells[row, 7].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 8].Value = oEquipo.AREANOMB;
                    ws.Cells[row, 8].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 9].Value = oEquipo.Equinomb;
                    ws.Cells[row, 9].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 10].Value = oEquipo.Equiabrev;
                    ws.Cells[row, 10].StyleName = ws.Cells[4, 2].StyleName;
                    ws.Cells[row, 11].Value = oEquipo.EstadoDesc;
                    ws.Cells[row, 11].StyleName = ws.Cells[4, 2].StyleName;

                    ws.Cells[row, 12].Value = oEquipo.Osinergcodi;
                    ws.Cells[row, 12].StyleName = ws.Cells[4, 2].StyleName;

                    iColumna = 13;

                    foreach (var prop in oEquipo.PropiedadesEquipo)
                    {
                        ws.Cells[row, iColumna].Value = prop.Valor;
                        ws.Cells[row, iColumna].StyleName = ws.Cells[4, 13].StyleName;
                        iColumna++;
                    }
                    //ws.Cells[row, 10].Value = oEquipo.Lastuser;
                    //ws.Cells[row, 10].StyleName = ws.Cells[4, 2].StyleName;
                    //ws.Cells[row, 11].Value = oEquipo.Lastdate.HasValue ? oEquipo.Lastdate.Value.ToString(ConstantesBase.FormatoFecha) : "";
                    //ws.Cells[row, 11].StyleName = ws.Cells[4, 2].StyleName;
                    //ws.Cells[row, 12].Value = oEquipo.UsuarioUpdate;
                    //ws.Cells[row, 12].StyleName = ws.Cells[4, 2].StyleName;
                    //ws.Cells[row, 13].Value = oEquipo.FechaUpdate.HasValue ? oEquipo.FechaUpdate.Value.ToString(ConstantesBase.FormatoFecha) : "";
                    //ws.Cells[row, 13].StyleName = ws.Cells[4, 2].StyleName;
                    row++;
                }

                for (int i = 2; i <= iColumna; i++)
                {
                    ws.Column(i).AutoFit();
                }
                //ws.Column(2).AutoFit();
                //ws.Column(3).AutoFit();
                //ws.Column(4).AutoFit();
                //ws.Column(5).AutoFit();
                //ws.Column(6).AutoFit();
                //ws.Column(7).AutoFit();
                //ws.Column(8).AutoFit();
                //ws.Column(9).AutoFit();
                //ws.Column(10).AutoFit();
                //ws.Column(11).AutoFit();
                //ws.Column(12).AutoFit();
                //ws.Column(13).AutoFit();
                xlPackage.Save();
            }

        }

        public static bool GuardarDatosPropiedades(string sFile, string sUsuario)
        {
            bool bResultado = false;

            try
            {
                EquipamientoAppServicio equipoServicio = new EquipamientoAppServicio();
                List<EqPropequiDTO> ListaPropiedades = new List<EqPropequiDTO>();
                List<EqEquipoDTO> listaEquipos = new List<EqEquipoDTO>();

                FileInfo fileInfo = new FileInfo(sFile);
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                    var CodigoEquipo = ws.Cells[4, 4].Value.ToString();
                    if (String.IsNullOrEmpty(CodigoEquipo))//Se verifica que existan datos en la primera fila, sino se asume vacío
                        return false;

                    var oEquipo = equipoServicio.GetByIdEqEquipo(int.Parse(CodigoEquipo));

                    /// Verificar Formato
                    /// 
                    int iFilaCodigoPropiedad = 2;
                    int iColumnaCodigoEquipo = 4;
                    int iColumnaOsinergmin = 12;
                    for (int i = 4; i < 10000; i++)
                    {

                        var osinergcodi = ws.Cells[i, iColumnaOsinergmin].Value != null ? ws.Cells[i, iColumnaOsinergmin].Value.ToString() : string.Empty;

                        if (ws.Cells[i, iColumnaCodigoEquipo].Value != null) //Se valida datos 
                        {
                            for (int j = 12; j < 10000; j++)
                            {
                                int filasPropiedad = j + 1;
                                if (ws.Cells[iFilaCodigoPropiedad, filasPropiedad].Value != null)
                                {
                                    EqPropequiDTO oPropiedad = new EqPropequiDTO();
                                    oPropiedad.osinergcodi = osinergcodi;
                                    oPropiedad.Equicodi = int.Parse(ws.Cells[i, iColumnaCodigoEquipo].Value.ToString());
                                    oPropiedad.Propcodi = int.Parse(ws.Cells[iFilaCodigoPropiedad, filasPropiedad].Value.ToString());
                                    oPropiedad.Valor = ws.Cells[i, filasPropiedad].Value != null ? ws.Cells[i, filasPropiedad].Value.ToString() : string.Empty;
                                    oPropiedad.Propequiusucreacion = sUsuario;
                                    oPropiedad.Propequifeccreacion = DateTime.Now;
                                    oPropiedad.Fechapropequi = DateTime.Today;
                                    ListaPropiedades.Add(oPropiedad);
                                }
                                else
                                {
                                    j = 100000;
                                }
                            }
                        }
                        else
                        {
                            i = 100000;
                        }

                    }
                }
                fileInfo.Delete();
                bResultado = equipoServicio.GuardarPropiedadesMasivo(ListaPropiedades);
            }
            catch (Exception ex)
            {
                log.Error("GuardarDatosPropiedades", ex);
            }

            return bResultado;
        }

        /// <summary>
        /// Generar Reporte Excel de Auditoria Cambio
        /// </summary>
        /// <param name="listaPropiedad"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public static void GenerarReporteAuditoriaCambio(EqEquipoDTO equipoSeleccionado, List<EqPropequiDTO> listaPropiedad, string path, string fileName)
        {
            string rutaFile = path + fileName;

            FileInfo newFile = new FileInfo(rutaFile);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                ws = xlPackage.Workbook.Worksheets["REPORTE"];

                #region  Datos de equipo

                int colIniTitulo = 2;
                int rowIniTitulo = 1;

                int colIniTable = colIniTitulo;
                int rowIniTabla = rowIniTitulo + 2;

                int rowEmpresa = rowIniTabla;
                int rowCodigo = rowEmpresa + 1;
                int rowEquipo = rowCodigo + 1;
                int rowUbicacion = rowEquipo + 1;
                int rowTipoEq = rowUbicacion + 1;

                ws.Cells[rowIniTitulo, colIniTitulo].Value = "Auditoría Cambios";
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, "Centro");
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo, "Calibri", 16);
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colIniTitulo);

                ws.Cells[rowEmpresa, colIniTable].Value = "Empresa:";
                ws.Cells[rowCodigo, colIniTable].Value = "Código:";
                ws.Cells[rowEquipo, colIniTable].Value = "Equipo:";
                ws.Cells[rowUbicacion, colIniTable].Value = "Ubicación:";
                ws.Cells[rowTipoEq, colIniTable].Value = "Tipo de Equipo:";
                ws.Cells[rowEmpresa, colIniTable + 1].Value = equipoSeleccionado.Emprnomb;
                ws.Cells[rowCodigo, colIniTable + 1].Value = equipoSeleccionado.Equicodi.ToString();
                ws.Cells[rowEquipo, colIniTable + 1].Value = equipoSeleccionado.Equinomb;
                ws.Cells[rowUbicacion, colIniTable + 1].Value = equipoSeleccionado.Areanomb;
                ws.Cells[rowTipoEq, colIniTable + 1].Value = equipoSeleccionado.Famnomb;

                //Estilos cabecera
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowEmpresa, colIniTable, rowTipoEq, colIniTable, "Calibri", 11);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowEmpresa, colIniTable, rowTipoEq, colIniTable, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowEmpresa, colIniTable, rowTipoEq, colIniTable, "Centro");
                UtilExcel.BorderCeldas2(ws, rowEmpresa, colIniTable, rowTipoEq, colIniTable + 1);
                UtilExcel.CeldasExcelEnNegrita(ws, rowEmpresa, colIniTable, rowTipoEq, colIniTable);

                #endregion

                #region  Cabecera
                
                rowIniTabla += 6;

                int colParametro = colIniTable;
                int colValor = colParametro + 1;
                int colUsuario = colValor + 1;
                int colFecha = colUsuario + 1;

                ws.Row(rowIniTabla).Height = 34;
                ws.Cells[rowIniTabla, colParametro].Value = "Parámetro";
                ws.Cells[rowIniTabla, colValor].Value = "Valor";
                ws.Cells[rowIniTabla, colUsuario].Value = "Usuario \n Última modificación";
                ws.Cells[rowIniTabla, colFecha].Value = "Fecha \n Última modificación";

                ws.Column(colParametro).Width = 35;
                ws.Column(colValor).Width = 50;
                ws.Column(colUsuario).Width = 20;
                ws.Column(colFecha).Width = 20;

                //Estilos cabecera
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla, colParametro, rowIniTabla, colFecha, "Calibri", 11);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla, colParametro, rowIniTabla, colFecha, "Centro");
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTabla, colParametro, rowIniTabla, colFecha, "Centro");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniTabla, colParametro, rowIniTabla, colFecha, "#2980B9");
                UtilExcel.CeldasExcelColorTexto(ws, rowIniTabla, colParametro, rowIniTabla, colFecha, "#FFFFFF");
                UtilExcel.BorderCeldas2(ws, rowIniTabla, colParametro, rowIniTabla, colFecha);
                UtilExcel.CeldasExcelEnNegrita(ws, rowIniTabla, colParametro, rowIniTabla, colFecha);
                UtilExcel.CeldasExcelWrapText(ws, rowIniTabla, colParametro, rowIniTabla, colFecha);

                #endregion

                #region Cuerpo Principal de tabla

                int rowData = rowIniTabla + 1;

                foreach (var item in listaPropiedad)
                {
                    ws.Cells[rowData, colParametro].Value = item.Propnomb;
                    ws.Cells[rowData, colValor].Value = item.Valor;
                    ws.Cells[rowData, colUsuario].Value = item.UltimaModificacionUsuarioDesc;
                    ws.Cells[rowData, colFecha].Value = item.UltimaModificacionFechaDesc;

                    rowData++;
                }

                //Estilos registros
                UtilExcel.CeldasExcelTipoYTamanioLetra(ws, rowIniTabla + 1, colParametro, rowData - 1, colFecha, "Calibri", 11);
                UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowIniTabla + 1, colParametro, rowData - 1, colFecha, "Centro");
                UtilExcel.BorderCeldas2(ws, rowIniTabla + 1, colParametro, rowData - 1, colFecha);
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colParametro, rowData - 1, colFecha, "Centro");
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowIniTabla + 1, colParametro, rowData - 1, colParametro, "Izquierda");

                #endregion

                xlPackage.Save();
            }

        }

    }
}