using COES.Base.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace COES.Framework.Base.Tools
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// Convierte un numero en una cadena de formato numérico y decimal "-d ddd ddd,ddd ...", donde "-" indica negativo
        /// </summary>
        /// <param name="value">Numero a convertir</param>
        /// <param name="decimalDigit">Dígitos decimales</param>
        /// <param name="textoReemplazo">Texto sustitución si es nulo</param>
        /// <returns></returns>
        public static string FormatoDecimal(this decimal? value, int decimalDigit, string textoReemplazo = "")
        {
            return value.HasValue ? value.Value.FormatoDecimal(decimalDigit) : textoReemplazo;
        }

        /// <summary>
        /// Convierte un numero en una cadena de formato numérico y decimal "-d ddd ddd,ddd ...", donde "-" indica negativo
        /// </summary>
        /// <param name="value">Numero a convertir</param>
        /// <param name="decimalDigit">Dígitos decimales</param>
        /// <returns></returns>
        public static string FormatoDecimal(this double value, int decimalDigit)
        {
            return ((decimal)value).FormatoDecimal(decimalDigit);
        }

        private static readonly NumberFormatInfo numberFormat = new CultureInfo("en-US", false).NumberFormat;
        /// <summary>
        /// Convierte un numero en una cadena de formato numérico y decimal "-d ddd ddd,ddd ...", donde "-" indica negativo
        /// </summary>
        /// <param name="value">Numero a convertir</param>
        /// <param name="decimalDigit">Dígitos decimales</param>
        /// <returns></returns>
        public static string FormatoDecimal(this decimal value, int decimalDigit)
        {
            NumberFormatInfo nfi = numberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = decimalDigit;
            nfi.NumberDecimalSeparator = ",";
            return value.ToString("N", nfi);
        }

        /// <summary>
        /// Retorna el nombre del mes y el año de una fecha
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NombreMesAnho(this DateTime value)
        {
            var mes = value.NombreMes();
            return string.Format("{0} {1}", mes, value.Year);
        }

        /// <summary>
        /// Retorna el nombre del mes y el año de una fecha
        /// </summary> 
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NombreMesAbrevAnho(this DateTime value)
        {
            var mes = value.NombreMesAbrev();
            return string.Format("{0} {1}", mes, value.Year);
        }

        /// <summary>
        /// Obtiene el nombre de mes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NombreMes(this DateTime value)
        {
            var mes = Util.ObtenerNombreMes(value.Month);
            return mes;
        }

        /// <summary>
        /// Obtiene el nombre de mes abrev
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NombreMesAbrev(this DateTime value)
        {
            var mes = Util.ObtenerNombreMesAbrev(value.Month);
            return mes;
        }

        public static List<int> FindAllIndex<T>(this IEnumerable<T> container, T val)
        {
            return container.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToList();
        }

        public static DateTime GetLastDateOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static DateTime ToDatetime(this string value, string format)
        {
            return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Verifica si objeto es de tipo numerico
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNumericType(this object o)
        {
            if (o == null)
                return false;

            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
