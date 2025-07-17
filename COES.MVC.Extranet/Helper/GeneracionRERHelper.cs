using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace COES.MVC.Extranet.Helper
{
    public class GeneracionRERHelper
    {
        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarFormato(List<WbGeneracionrerDTO> list, string empresa, int horizonte, DateTime fecha,
            int? semana, DateTime fechaInicio, DateTime fechaFin)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteGeneracionRER].ToString();
            string fileTemplate = (horizonte == 0) ? NombreArchivo.PlantillaFormatoGeneracionRERDiario :
                NombreArchivo.PlantillaFormatoGeneracionRERSemanal;
            FileInfo template = new FileInfo(ruta + fileTemplate);

            FileInfo newFile = new FileInfo(ruta + NombreArchivo.FormatoGeneracionRER);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivo.FormatoGeneracionRER);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaFormatoExcel];

                ws.Cells[2, 2].Value = empresa;

                if (horizonte == 0)
                {
                    ws.Cells[4, 2].Value = fecha.ToString(Constantes.FormatoFecha);
                }
                if (horizonte == 1)
                {
                    ws.Cells[4, 2].Value = semana.ToString();
                    ws.Cells[5, 2].Value = fechaInicio.ToString(Constantes.FormatoFecha);
                    ws.Cells[6, 2].Value = fechaFin.ToString(Constantes.FormatoFecha);
                }

                List<WbGeneracionrerDTO> listCentrales = list.Where(x => x.IndPorCentral == Constantes.SI).OrderBy(x => x.Central).ToList();
                List<WbGeneracionrerDTO> listUnidades = list.Where(x => x.IndPorCentral == Constantes.NO).OrderBy(x => x.EquiNomb).ToList();

                int row = 9;
                int column = 2;

                foreach (WbGeneracionrerDTO item in listCentrales)
                {
                    ws.Cells[row, column].Value = item.Ptomedicodi.ToString();
                    ws.Cells[row + 1, column].Value = item.Central;
                    ws.Cells[row + 2, column].Value = Constantes.TextoCentral;
                    ws.Cells[row + 3, column].Value = Constantes.TextoMW;
                    ws.Cells[row, column].StyleID = ws.Cells[row, 2].StyleID;
                    ws.Cells[row + 1, column].StyleID = ws.Cells[row, 2].StyleID;
                    ws.Cells[row + 2, column].StyleID = ws.Cells[row, 2].StyleID;
                    ws.Cells[row + 3, column].StyleID = ws.Cells[row, 2].StyleID;
                    column++;
                }

                List<int> ids = listUnidades.Select(x => x.CentralCodi).Distinct().ToList();

                foreach (int id in ids)
                {
                    List<WbGeneracionrerDTO> listUnidad = listUnidades.Where(x => x.CentralCodi == id).ToList();

                    foreach (WbGeneracionrerDTO item in listUnidad)
                    {
                        ws.Cells[row, column].Value = item.Ptomedicodi.ToString();
                        ws.Cells[row + 1, column].Value = item.Central;
                        ws.Cells[row + 2, column].Value = item.EquiNomb;
                        ws.Cells[row + 3, column].Value = Constantes.TextoMW;
                        ws.Cells[row, column].StyleID = ws.Cells[row, 2].StyleID;
                        ws.Cells[row + 1, column].StyleID = ws.Cells[row, 2].StyleID;
                        ws.Cells[row + 2, column].StyleID = ws.Cells[row, 2].StyleID;
                        ws.Cells[row + 3, column].StyleID = ws.Cells[row, 2].StyleID;
                        column++;
                    }
                }

                row = row + 4;

                if (horizonte == 0)
                {
                    for (int i = 0; i < 48; i++)
                    {
                        ws.Cells[row, 1].Value = ((fecha.AddMinutes(i * 30))).ToString(Constantes.FormatoFechaHora);

                        for (int k = 2; k < column; k++)
                        {
                            ws.Cells[row, k].StyleID = ws.Cells[row, 2].StyleID;
                        }

                        row++;
                    }
                }
                if (horizonte == 1)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 48; j++)
                        {
                            ws.Cells[row, 1].Value = ((fechaInicio.AddDays(i).AddMinutes(j * 30))).ToString(Constantes.FormatoFechaHora);

                            for (int k = 2; k < column; k++)
                            {
                                ws.Cells[row, k].StyleID = ws.Cells[row, 2].StyleID;
                            }
                            row++;
                        }
                    }
                }

                xlPackage.Save();
            }
        }         


        /// <summary>
        /// Permite realizar las validaciones
        /// </summary>
        /// <returns></returns>
        public static List<MeMedicion48DTO> ValidarCarga(string file, List<int> idsPuntos,  int horizonte,
            DateTime fecha, int anio, int nroSemana, out List<string> listValidaciones, out List<string> listValidacionDatos,
            List<WbGeneracionrerDTO> listPuntos)
        {
            try
            {
                List<string> validaciones = new List<string>();
                List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();

                List<string> validacionesPuntos = new List<string>();
                string validacionPuntos = string.Empty;
                int countValidacionFormato = 0;
                int countValidacionNegativo = 0;
                int countValidacionExistencia = 0;
                int countValidacionMaximo = 0;
                
                FileInfo fileInfo = new FileInfo(file);
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                    #region Validacion fechas y semana

                    if (horizonte == 0)
                    {
                        string fechaFormato = (ws.Cells[4, 2].Value != null) ? ws.Cells[4, 2].Value.ToString() : string.Empty;
                        if (!string.IsNullOrEmpty(fechaFormato))
                        {
                            DateTime fechaValidacion = DateTime.Now;
                            if (DateTime.TryParseExact(fechaFormato, Constantes.FormatoFecha, CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out fechaValidacion))
                            {
                                double diferencia = (fecha.Subtract(fechaValidacion)).TotalDays;
                                if (diferencia != 0)
                                {
                                    validaciones.Add(ValidacionArchivo.FechasNoCoinciden);
                                }
                            }
                            else
                            {
                                validaciones.Add(ValidacionArchivo.FechaFormatoIncorrecto);
                            }
                        }
                        else
                        {
                            validaciones.Add(ValidacionArchivo.FechaNoExisteFormato);
                        }
                    }

                    if (horizonte == 1)
                    {
                        string semana = (ws.Cells[4, 2].Value != null) ? ws.Cells[4, 2].Value.ToString() : string.Empty;

                        if (!string.IsNullOrEmpty(semana))
                        {
                            int nSemana = 0;
                            if (int.TryParse(semana, out nSemana))
                            {
                                if (nSemana != nroSemana)
                                {
                                    validaciones.Add(ValidacionArchivo.SemanasNoCoinciden);
                                }
                            }
                            else
                            {
                                validaciones.Add(ValidacionArchivo.SemanaFormatoIncorrecto);
                            }
                        }
                        else
                        {
                            validaciones.Add(ValidacionArchivo.SemanaNoExisteFormato);
                        }
                    }

                    #endregion

                    #region Validamos puntos

                    List<int> puntos = new List<int>();

                    int row = 9;
                    int column = 2;

                    bool validacionPunto = true;

                    while (true)
                    {
                        string punto = (ws.Cells[row, column].Value != null) ? ws.Cells[row, column].Value.ToString() : string.Empty;
                        if (!string.IsNullOrEmpty(punto))
                        {
                            int ptoMedicion = 0;
                            if (int.TryParse(punto, out ptoMedicion))
                            {
                                puntos.Add(ptoMedicion);
                            }
                            else
                            {
                                validaciones.Add(String.Format(ValidacionArchivo.CodigoPuntoFormatoIncorrecto, column));
                                validacionPunto = false;
                            }
                            column++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (puntos.Count == 0)
                    {
                        validaciones.Add(ValidacionArchivo.NoExistenPuntosCargados);
                        validacionPunto = false;
                    }
                    else
                    {
                        if (idsPuntos.Count != puntos.Count)
                        {
                            validaciones.Add(ValidacionArchivo.FaltaPuntosEnFormato);
                            validacionPunto = false;
                        }

                        foreach (int id in puntos)
                        {
                            if (!idsPuntos.Contains(id))
                            {
                                validaciones.Add(String.Format(ValidacionArchivo.PuntoNoPerteneceEmpresa, id));
                                validacionPunto = false;
                            }
                        }
                    }

                    #endregion

                    #region Validacion de fechas de carga

                    bool validacionFecha = true;
                    row = 13;
                    int rowFinal = (horizonte == 0) ? 60 : 348;
                    DateTime fechaInicial = (horizonte == 0) ? fecha : EPDate.f_fechainiciosemana(anio, nroSemana);

                    for (int i = row; i <= rowFinal; i++)
                    {
                        string date = (ws.Cells[i, 1].Value != null) ? ws.Cells[i, 1].Value.ToString() : string.Empty;
                        DateTime fechaComparacion = DateTime.Now;

                        if (DateTime.TryParseExact(date, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out fechaComparacion))
                        {
                            if ((fechaComparacion.Subtract(fechaInicial.AddMinutes((i - row) * 30))).TotalMinutes != 0)
                            {
                                validaciones.Add(String.Format(ValidacionArchivo.FechaNoEstanEnOrden, i));
                                validacionFecha = false;
                                break;
                            }
                        }
                        else
                        {
                            validaciones.Add(String.Format(ValidacionArchivo.FechaCargaNoTieneFormato, i));
                            validacionFecha = false;
                            break;
                        }
                    }
                    
                    //validamos que sea el formato correcto
                    int countValidacion = 0;
                    bool validacionEstructura = true;
                    string mensajeEstructura = string.Empty;
                    while (true)
                    {
                        if (ws.Cells[row, 1].Value == null)
                        {
                            break;
                        }
                        row++;
                        countValidacion++;
                    }

                    if (countValidacion > 300 && horizonte == 0)
                    {
                        mensajeEstructura = ValidacionArchivo.FormatoCorrespondeSemanal;
                        validacionEstructura = false;
                    }

                    if (countValidacion <= 60 && horizonte == 1)
                    {
                        mensajeEstructura = ValidacionArchivo.FormatoCorrespondeDiario;
                        validacionEstructura = false;
                    }

                    row = 13;

                    #endregion

                    if (!validacionEstructura)
                    {
                        validaciones.Clear();
                        validaciones.Add(mensajeEstructura);
                    }
                    else
                    {
                        #region Validamos los datos

                        if (validacionPunto && validacionFecha)
                        {
                            column = 2;
                            foreach (int id in puntos)
                            {
                                int count = (rowFinal - row + 1) / 48;

                                for (int k = 0; k < count; k++)
                                {
                                    MeMedicion48DTO entity = new MeMedicion48DTO();
                                    decimal total = 0;
                                    entity.Ptomedicodi = id;
                                    entity.Medifecha = fechaInicial.AddDays(k);

                                    decimal max = decimal.MaxValue;                                  

                                    if (listPuntos.Where(x => x.Ptomedicodi == id).Count() > 0)
                                    {
                                        WbGeneracionrerDTO entityValidador = listPuntos.Where(x => x.Ptomedicodi == id).FirstOrDefault();

                                        if (entityValidador != null)
                                        {
                                            if (entityValidador.Genrermax != null)
                                                max = (decimal)entityValidador.Genrermax;                                           
                                        }
                                    }

                                    for (int i = 1; i <= 48; i++)
                                    {
                                        int indice = row + k * 48 + i - 1;
                                        string valor = (ws.Cells[indice, column].Value != null) ? ws.Cells[indice, column].Value.ToString() : string.Empty;
                                        decimal dato = 0;

                                        if (!string.IsNullOrEmpty(valor))
                                        {
                                            if (decimal.TryParse(valor, out dato))
                                            {
                                                if (dato >= 0)
                                                {
                                                    if (dato <= max)
                                                    {
                                                        entity.GetType().GetProperty(Constantes.CaracterH + i.ToString()).SetValue(entity, dato);
                                                        total = total + dato;
                                                    }
                                                    else
                                                    {
                                                        countValidacionMaximo++;
                                                        if (countValidacionMaximo == 1)
                                                        {
                                                            validaciones.Add(string.Format(ValidacionArchivo.SuperaMaximo, max, indice, column));
                                                        }
                                                        validacionesPuntos.Add(string.Format(ValidacionArchivo.SuperaMaximo, max, indice, column));
                                                    }

                                                }
                                                else
                                                {
                                                    countValidacionNegativo++;
                                                    if (countValidacionNegativo == 1)
                                                    {
                                                        validaciones.Add(string.Format(ValidacionArchivo.DatoNegativo, indice, column));
                                                    }
                                                    validacionesPuntos.Add(string.Format(ValidacionArchivo.DatoNegativo, indice, column));
                                                }
                                            }
                                            else
                                            {
                                                countValidacionFormato++;
                                                if (countValidacionFormato == 1)
                                                {
                                                    validaciones.Add(string.Format(ValidacionArchivo.DatoFormatoIncorrecto, indice, column));
                                                }
                                                validacionesPuntos.Add(string.Format(ValidacionArchivo.DatoFormatoIncorrecto, indice, column));
                                            }
                                        }
                                        else
                                        {
                                            countValidacionExistencia++;
                                            if (countValidacionExistencia == 1)
                                            {
                                                validaciones.Add(string.Format(ValidacionArchivo.DatoNoExiste, indice, column));
                                            }
                                            validacionesPuntos.Add(string.Format(ValidacionArchivo.DatoNoExiste, indice, column));
                                        }
                                    }

                                    entity.Meditotal = total;
                                    entitys.Add(entity);
                                }
                                column++;
                            }
                        }

                        #endregion
                    }
                }

                if (countValidacionExistencia > 0)
                {
                    validacionPuntos = validacionPuntos + String.Format("Existen {0} celdas sin dato.<br />", countValidacionExistencia);
                }
                if (countValidacionFormato > 0)
                {
                    validacionPuntos = validacionPuntos + String.Format("Existen {0} celdas que no tienen formato correcto.<br />", countValidacionFormato);
                }
                if (countValidacionNegativo > 0)
                {
                    validacionPuntos = validacionPuntos + String.Format("Existen {0} celdas que tienen datos negativos.<br />", countValidacionNegativo);
                }
                if(countValidacionMaximo > 0)
                {
                    validacionPuntos = validacionPuntos + String.Format("Existen {0} celdas que superaron el valor máximo.<br />", countValidacionMaximo);
                }
                if ((countValidacionExistencia > 0 || countValidacionFormato > 0 || countValidacionNegativo > 0 || countValidacionMaximo > 0) &&
                    (countValidacionExistencia + countValidacionFormato + countValidacionNegativo + countValidacionMaximo) > 1)
                {
                    validacionPuntos = validacionPuntos + "<a href='JavaScript:verValidaciones();'>Ver todas las validaciones de datos.</a>";
                }

                listValidacionDatos = validacionesPuntos;

                if(!string.IsNullOrEmpty(validacionPuntos))
                {
                    validaciones.Add(validacionPuntos);
                }

                listValidaciones = validaciones;

                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }            
        }
    }
}