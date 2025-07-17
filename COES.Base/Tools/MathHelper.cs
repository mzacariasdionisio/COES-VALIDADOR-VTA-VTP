using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Tools
{
    public static class MathHelper
    {
        public static decimal Round(this decimal d, int decimals)
        {
            return decimal.Round(d, decimals, MidpointRounding.AwayFromZero);
        }

        public static decimal? Round(this decimal? d, int decimals)
        {
            return d.HasValue ? Round(d.Value, decimals) : (decimal?)null;
        }

        /// <summary>
        /// Obtener la cantidad de dígitos de la parte decimal de un número
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int GetDecimalPlaces(decimal n)
        {
            n = Math.Abs(n); //make sure it is positive.
            n -= (int)n;     //remove the integer part of the number.
            var decimalPlaces = 0;
            while (n > 0)
            {
                decimalPlaces++;
                n *= 10;
                n -= (int)n;
            }
            return decimalPlaces;
        }

        /// <summary>
        /// Truncar número decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            return tmp / step;
        }

        /// <summary>
        /// Truncar número double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static double TruncateDouble(double value, int precision)
        {
            double step = Math.Pow(10, precision);
            double tmp = Math.Truncate(step * value);
            return tmp / step;
        }

        /// <summary>
        /// Desviación estandar de una muestra - Microsoft Excel
        /// </summary>
        /// <param name="listaValor"></param>
        /// <returns></returns>
        public static double GetDesviacionEstandar(List<double> listaValor)
        {
            double standardDeviation = 0;

            if (listaValor.Any())
            {
                double avg = listaValor.Average();

                double sum = listaValor.Sum(d => Math.Pow(d - avg, 2));

                standardDeviation = Math.Sqrt((sum) / (listaValor.Count() - 1));
            }

            return standardDeviation;
        }

    }
}
