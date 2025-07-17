using System;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using COES.Framework.Base.Tools;
using Ionic.Zip;

namespace COES.Servicios.Aplicacion.PrimasRER.Helper
{
    /// <summary>
    /// Clase de metodos utilitarios
    /// </summary>
    public class UtilPrimasRER
    {
        /// <summary>
        /// Almacena un archivo en excel en un data set
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public static DataSet GeneraDataset(string RutaArchivo, int Hoja)
        {
            FileInfo archivo = new FileInfo(RutaArchivo);
            ExcelPackage xlPackage = new ExcelPackage(archivo);
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Hoja];

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
        /// </summary>
        /// <param name="sValor">Cadena de texto</param>
        public static decimal ValidarNumero(string sValor)
        {
            decimal dNumero;
            if (!sValor.Equals("") && (Decimal.TryParse(sValor, System.Globalization.NumberStyles.Float, null, out dNumero)))
            {
                return dNumero;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Construye un objeto DateTime según la fecha y formato
        /// </summary>
        /// <param name="sValor">Cadena con la fecha</param>
        /// <param name="sformato">Cadena con formato de fecha</param>
        public static DateTime ConstruirDateTime(string sValor, string sformato)
        {
            // Dividir la cadena en fecha y hora
            string[] partes = sValor.Split(' ');
            string fecha = partes[0];
            string hora = partes[1];

            // Dividir la fecha en día, mes y año
            string[] partesFecha = fecha.Split('/');
            int dia = int.Parse(partesFecha[0]);
            int mes = int.Parse(partesFecha[1]);
            int año = int.Parse(partesFecha[2]);

            // Agregar cero al inicio si es necesario
            string diaFormateado = dia.ToString().PadLeft(2, '0');
            string mesFormateado = mes.ToString().PadLeft(2, '0');

            // Reconstruir la fecha con los valores formateados
            string fechaFormateada = $"{diaFormateado}/{mesFormateado}/{año} {hora}";

            return DateTime.ParseExact(fechaFormateada, sformato, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Valida si la cadena es un número
        /// </summary>
        /// <param name="input">cadena a validar</param>
        public static bool ValidarCandenaNumero(string input)
        {
            decimal numero;
            if (Decimal.TryParse(input, System.Globalization.NumberStyles.Float, null, out numero))
            {
                return numero >= 0;
            }
            return false;
        }

        /// <summary>
        /// Valida que 2 Listas de tipo int sean iguales
        /// </summary>
        /// <param name="lista1">Primera Lista</param>
        /// <param name="lista2">Segunda Lista</param>
        public static bool SonListasIguales(List<int> lista1, List<int> lista2)
        {
            // Verificar si las listas tienen la misma cantidad de elementos
            if (lista1.Count != lista2.Count)
                return false;

            // Verificar si las listas tienen los mismos elementos sin repeticiones
            HashSet<int> hashSet1 = new HashSet<int>(lista1);
            HashSet<int> hashSet2 = new HashSet<int>(lista2);

            return hashSet1.SetEquals(hashSet2);
        }


        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static void CopiarFile(string origen, string destino, string file)
        {
            string pathOrigen = origen + file;
            string pathDestino = destino + file;
           
            File.Copy(pathOrigen, pathDestino);
        }

        /// <summary>
        /// Permite eliminar un archivo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static void DeleteFile(string path, string filename)
        {
            File.Delete(filename + filename);
        }

        /// <summary>
        /// Permite eliminacion recursiva del folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void DeleteFolder(string path)
        {
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Devuelve la descripcion de tipo de resultado
        /// </summary>
        /// <param name="tipoResultado"></param>
        /// <param name="junto"></param>
        /// <returns></returns>
        public static string ObtenerNombreTipoResultado(string tipoResultado, bool junto)
        {
            string nombre = "";
            switch(tipoResultado)
            {
                case ConstantesPrimasRER.tipoResultadoInyeccionNeta:
                    nombre = junto ? "InyeccionNeta" : "Inyección Neta";
                    break;
                case ConstantesPrimasRER.tipoResultadoCostoMarginal:
                    nombre = junto ? "CostoMarginal" : "Costo Marginal";
                    break;
                case ConstantesPrimasRER.tipoResultadoIngresosPotencia:
                    nombre = junto ? "IngresosporPotencia" : "Ingresos por Potencia";
                    break;
                case ConstantesPrimasRER.tipoResultadoIngresosCargoPrimaRER:
                    nombre = junto ? "IngresosporCargoPrimaRER" : "Ingresos por Cargo Prima RER";
                    break;
                case ConstantesPrimasRER.tipoResultadoEnergiaDejadaInyectar:
                    nombre = junto ? "EnergiaDejadaDeInyectar" : "Energia Dejada de Inyectar";
                    break;
                case ConstantesPrimasRER.tipoResultadoSaldosVTEA:
                    nombre = junto ? "SaldosVTEA" : "Saldos VTEA";
                    break;
                case ConstantesPrimasRER.tipoResultadoSaldosVTP:
                    nombre = junto ? "SaldosVTP" : "Saldos VTP";
                    break;
            }
            return nombre;
        }
    }
}
