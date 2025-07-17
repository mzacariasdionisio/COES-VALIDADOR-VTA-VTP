using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;
using System.IO;

namespace COES.Servicios.Aplicacion.CalculoPorcentajes.Helper
{
    public class UtilCalculoPorcentajes
    {
        /// <summary>
        /// Almacena un archivo en excel en un data set
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public static DataSet GeneraDataset(string RutaArchivo, int hoja)
        {
            FileInfo archivo = new FileInfo(RutaArchivo);
            ExcelPackage xlPackage = new ExcelPackage(archivo);
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
            {
                string Columna = "";
                if (ws.Cells[1, j].Value != null) Columna = ws.Cells[1, j].Value.ToString();
                dt.Columns.Add(Columna);
            }

            for (int i = 2; i <= ws.Dimension.End.Row; i = i + 1)
            {
                DataRow row = dt.NewRow();
                for (int j = 1; j <= ws.Dimension.End.Column; j = j + 1)
                {
                    if (ws.Cells[i, j].Value == null)
                        row[j - 1] = "null";
                    else
                        row[j - 1] = ws.Cells[i, j].Value.ToString().Trim();
                }
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            xlPackage.Dispose();
            return ds;
        }

        /// <summary>
        /// Valida que la información ingresada solo contenga numeros y letras
        /// /// </summary>
        /// <param name="str">Cadena de texto</param>
        public static string CorregirCodigo(string str)
        {
            string strLimpio = "";
            str = str.ToUpper();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9') ||
                     (str[i] >= 'A' && str[i] <= 'Z')
                )
                { strLimpio = strLimpio + str[i]; }
            }
            return strLimpio;
        }

        /// <summary>
        /// Valida que la información ingresada sea un numero valido, caso contrario devuelve cero
        /// /// </summary>
        /// <param name="sValor">Cadena de texto</param>
        public static decimal ValidarNumero(string sValor)
        {
            decimal dNumero;
            if (!sValor.Equals("") && (Decimal.TryParse(sValor, out dNumero)))
            {
                return dNumero;
            }
            else
            {
                return 0;
            }
        }

        public static string ObtenerNombreMes(int index)
        {
            string resultado = string.Empty;

            switch (index)
            {
                case 1:
                    resultado = "Enero";
                    break;
                case 2:
                    resultado = "Febrero";
                    break;
                case 3:
                    resultado = "Marzo";
                    break;
                case 4:
                    resultado = "Abril";
                    break;
                case 5:
                    resultado = "Mayo";
                    break;
                case 6:
                    resultado = "Junio";
                    break;
                case 7:
                    resultado = "Julio";
                    break;
                case 8:
                    resultado = "Agosto";
                    break;
                case 9:
                    resultado = "Setiembre";
                    break;
                case 10:
                    resultado = "Octubre";
                    break;
                case 11:
                    resultado = "Noviembre";
                    break;
                case 12:
                    resultado = "Diciembre";
                    break;
                default:
                    resultado = string.Empty;
                    break;
            }

            return resultado;
        }

        public static string ObtenerDescTipo(string tipo)
        {
            string resultado = string.Empty;
            switch (tipo)
            {
                case "P":
                    resultado = "Proyectado";
                    break;
                case "E":
                    resultado = "Ejecutado";
                    break;

                default:
                    resultado = string.Empty;
                    break;
            }

            return resultado;
        }

    }
}
