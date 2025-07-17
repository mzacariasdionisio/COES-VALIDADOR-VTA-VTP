using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class DemandaBarrasAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite obtener las empresas por tipo
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresaPorTIpo(int idTipoEmpresa)
        {
            if (idTipoEmpresa == 2)
            {
                return FactorySic.GetSiEmpresaRepository().ListadoComboEmpresasPorTipo(idTipoEmpresa);
            }
            else
            {
                SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(11772);
                List<SiEmpresaDTO> list = FactorySic.GetSiEmpresaRepository().ListadoComboEmpresasPorTipo(idTipoEmpresa).
                    Where(x => x.Inddemanda == ConstantesAppServicio.SI).ToList();
                list.Add(empresa);
                return list.OrderBy(x => x.Emprnomb).ToList();
            }
        }

        /// <summary>
        /// Permite obtener el nombre de la empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public string ObtenerNombreEmpresa(int idEmpresa)
        {
            SiEmpresaDTO entity = FactorySic.GetSiEmpresaRepository().GetById(idEmpresa);
            return entity.Emprnomb;
        }

        /// <summary>
        /// Permite obtener el reporte de demanda en barras
        /// </summary>
        /// <param name="tipo">1: Historica diaria, 2. Prevista diaria, 3: Provista semanal</param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresas"></param>
        /// <returns></returns>
        public string[][] ObtenerReporte(int lectcodi, string empresas, DateTime fechaInicio, DateTime fechaFin, out int indicador)
        {
            List<MeMedicion48DTO> entitys = FactorySic.GetMeMedicion48Repository().ObtenerConsultaDemandaBarras(
                lectcodi, empresas, fechaInicio, fechaFin);
            int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;
            var puntos = entitys.Select(x => new { x.Emprnomb, x.Ptomedicodi, x.Equitension, x.Equicodi, x.Equinomb, x.Areacodi, x.Areanomb }).
                Distinct().OrderBy(x => x.Emprnomb).ThenBy(x => x.Areanomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Equitension).ToList();
            int longitud = 5 + (nroDias + 1) * 48;

            string[][] datos = new string[longitud][];
            for (int i = 0; i < longitud; i++)
                datos[i] = new string[1 + puntos.Count];
            datos[0][0] = "PUNTO DE MEDICIÓN";
            datos[1][0] = "EMPRESA";
            datos[2][0] = "EQUIPO";
            datos[3][0] = "TENSION";
            datos[4][0] = "FECHA HORA / SUBESTACIÓN";

            int j = 1;
            foreach (var item in puntos)
            {
                datos[0][j] = item.Ptomedicodi.ToString();
                datos[1][j] = item.Emprnomb.Trim();
                datos[2][j] = (!string.IsNullOrEmpty(item.Equinomb)) ? item.Equinomb.Trim() : string.Empty;
                datos[3][j] = (item.Equitension != null) ? item.Equitension.ToString() : string.Empty;
                datos[4][j] = (!string.IsNullOrEmpty(item.Areanomb)) ? item.Areanomb.Trim() : string.Empty;
                j++;
            }

            int postDatos = 5;
            for (int i = 0; i <= nroDias; i++)
            {
                DateTime fecha = fechaInicio.AddDays(i);

                if (entitys.Where(x => x.Medifecha.Year == fecha.Year && x.Medifecha.Month == fecha.Month && x.Medifecha.Day == fecha.Day).Count() > 0)
                {
                    for (int k = 0; k < 48; k++)
                    {
                        datos[postDatos + 48 * i + k][0] = fecha.AddMinutes(30 * (k + 1)).ToString(ConstantesAppServicio.FormatoFechaHora);
                    }

                    int indexColumn = 1;
                    foreach (var item in puntos)
                    {
                        MeMedicion48DTO itemMedicion = entitys.Where(x => x.Medifecha.Year == fecha.Year && x.Medifecha.Month ==
                            fecha.Month && x.Medifecha.Day == fecha.Day && x.Ptomedicodi == item.Ptomedicodi).FirstOrDefault();

                        if (itemMedicion != null)
                        {
                            for (int k = 1; k <= 48; k++)
                            {
                                decimal valor = Convert.ToDecimal(itemMedicion.GetType().GetProperty(ConstantesAppServicio.CaracterH + k).GetValue(itemMedicion));
                                datos[postDatos + 48 * i + k - 1][indexColumn] = valor.ToString("0.000");
                            }
                        }
                        else
                        {
                            for (int k = 1; k <= 48; k++)
                                datos[postDatos + 48 * i + k - 1][indexColumn] = string.Empty;
                        }
                        indexColumn++;
                    }
                }
            }

            List<string[]> result = new List<string[]>();
            foreach (string[] item in datos)
            {
                if (!string.IsNullOrEmpty(item[0]))
                {
                    result.Add(item);
                }
            }

            indicador = (entitys.Count > 0) ? 1 : 0;

            return result.ToArray();
        }



        public List<MeMedicion48DTO> ObtenerReporte2(int lectcodi, string empresas, DateTime fechaInicio, DateTime fechaFin, int indicador)
        {
            List<MeMedicion48DTO> entitys = FactorySic.GetMeMedicion48Repository().ObtenerConsultaDemandaBarras(
                lectcodi, empresas, fechaInicio, fechaFin);

            return entitys;
        }

        /// <summary>
        /// Permite generar el archivo excel a exportar
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void ExportarConsulta(string path, string file, int lectcodi, string empresas, DateTime fechaInicio, DateTime fechaFin, out int resultado)
        {
            try
            {
                string fileName = path + file;
                FileInfo newFile = new FileInfo(fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(fileName);
                }

                int indicador = 0;
                string[][] datos = this.ObtenerReporte(lectcodi, empresas, fechaInicio, fechaFin, out indicador);
                resultado = indicador;
                if (indicador == 1)
                {

                    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DATOS");

                        if (ws != null)
                        {
                            ws.Cells[1, 1].Value = "CONSULTA DEMANDA EN BARRAS";
                            ws.Cells[2, 1].Value = "DESDE: ";
                            ws.Cells[3, 1].Value = "HASTA: ";
                            ws.Cells[2, 2].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                            ws.Cells[3, 2].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);

                            ExcelRange rg = ws.Cells[1, 1, 1, 1];
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 12;
                            rg = ws.Cells[2, 1, 3, 2];
                            rg.Style.Font.Bold = true;
                            rg.Style.Font.Size = 11;



                            int row = 5;
                            int contador = 0;
                            foreach (string[] fila in datos)
                            {
                                int indexColumn = 1;
                                foreach (string columna in fila)
                                {
                                    if (row > 9 && indexColumn > 1)
                                    {
                                        if (!string.IsNullOrEmpty(columna))
                                            ws.Cells[row, indexColumn].Value = Convert.ToDecimal(columna);
                                    }
                                    else
                                    {
                                        ws.Cells[row, indexColumn].Value = columna;
                                    }

                                    indexColumn++;
                                    contador = indexColumn;
                                }
                                row++;
                            }

                            rg = ws.Cells[5, 1, 5, contador - 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                            rg.Style.Font.Color.SetColor(Color.White);
                            rg.Style.Font.Bold = true;

                            rg = ws.Cells[6, 1, 6, contador - 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));

                            rg = ws.Cells[7, 1, 7, contador - 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));

                            rg = ws.Cells[8, 1, 8, contador - 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));

                            rg = ws.Cells[9, 1, 9, contador - 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EAF7D9"));

                            rg = ws.Cells[10, 1, row - 1, 1];
                            rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E8F6FF"));

                            rg = ws.Cells[5, 1, row - 1, contador - 1];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Gray);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Gray);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Gray);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Gray);
                            rg.Style.Font.Size = 10;

                            ws.Column(1).Width = 25;
                            rg = ws.Cells[5, 2, row - 1, contador - 1];
                            rg.AutoFitColumns();

                        }
                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<Medicion48DTO> ObtenerDemandaProgramadaDiariaAreas(DateTime dtFecha)
        {
            return FactorySic.GetMeMedicion48Repository().ObtenerDemandaProgramadaDiariaAreas(dtFecha);
        }

        public List<Medicion48DTO> ObtenerProgramadaDiariaCOES(DateTime dtFecha)
        {
            return FactorySic.GetMeMedicion48Repository().ObtenerProgramadaDiariaCOES(dtFecha);
        }

        public List<Medicion48DTO> ObtenerDemandaDiariaReal(DateTime fechaInicio)
        {
            return FactorySic.GetMeMedicion48Repository().ObtenerDemandaDiariaReal(fechaInicio);
        }

        public List<Medicion48DTO> LeerDemandaAreas(DateTime inicioFecha)
        {
            return FactorySic.GetMeMedicion48Repository().LeerDemandaAreas(inicioFecha);
        }
    }
}
