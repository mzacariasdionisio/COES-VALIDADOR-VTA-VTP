using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Helper
{
    public class InformeHelper
    {
        /// <summary>
        /// Lee los artículos desde el formato excel cargado
        /// </summary>
        /// <param name="codCliente"></param>
        /// <returns></returns>
        public static List<EveInformeItemDTO> ImporatInterrupcion(string file, int idInforme, out List<string> validaciones)
        {
            try
            {
                List<string> errors = new List<string>();
                List<EveInformeItemDTO> entitys = new List<EveInformeItemDTO>();
                FileInfo fileInfo = new FileInfo(file);

                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                    int indice = 6;

                    while (true)
                    {
                        object suministro = ws.Cells[indice, 2].Value;
                        object potencia = ws.Cells[indice, 3].Value;
                        object inicio = ws.Cells[indice, 4].Value;
                        object final = ws.Cells[indice, 5].Value;
                        object proteccion = ws.Cells[indice, 6].Value;

                        if (suministro != null && potencia != null && inicio != null && final != null && proteccion != null)
                        {
                            EveInformeItemDTO entity = new EveInformeItemDTO();

                            entity.Eveninfcodi = idInforme;
                            entity.Itemnumber = 10;
                            entity.Sumininistro = Convert.ToString(suministro);

                            bool flag = true;

                            decimal potenciamw = 0;
                            if (decimal.TryParse(potencia.ToString(), out potenciamw))
                            {
                                entity.Potenciamw = potenciamw;
                            }
                            else
                            {
                                errors.Add(string.Format("Fila {0} columna 3 debe tener formato número", indice));
                                flag = false;
                            }

                            DateTime fechaInicio = DateTime.Now;
                            if (DateTime.TryParseExact(inicio.ToString(), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture, DateTimeStyles.None,
                                out fechaInicio))
                            {
                                entity.Intinicio = fechaInicio;
                            }
                            else
                            {
                                errors.Add(string.Format("Fila {0} columna 5 debe tener formato fecha", indice));
                                flag = false;
                            }

                            DateTime fechaFin = DateTime.Now;
                            if (DateTime.TryParseExact(final.ToString(), Constantes.FormatoFechaFull, CultureInfo.InvariantCulture, DateTimeStyles.None,
                                out fechaFin))
                            {
                                entity.Intfin = fechaFin;
                            }
                            else
                            {
                                errors.Add(string.Format("Fila {0} columna 4 debe tener formato fecha", indice));
                                flag = false;
                            }

                            entity.Proteccion = Convert.ToString(proteccion);

                            if (flag)
                            {
                                entitys.Add(entity);
                            }

                        }
                        else
                        {
                            break;
                        }
                        indice++;
                    }
                }

                validaciones = errors;
                return entitys;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}