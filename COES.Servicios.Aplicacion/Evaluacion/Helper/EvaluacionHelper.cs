using System;

namespace COES.Servicios.Aplicacion.Evaluacion.Helper
{
    public class EvaluacionHelper
    {
        /// <summary>
        /// Permite redondear un valor a un número de decimales y regresa un numero en formato string
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="numeroDecimales"></param>
        /// <returns></returns>
        public static string RedondearValor(object valor, int numeroDecimales = 0)
        {
            double valorDouble = Convert.ToDouble(valor);

            string resultado = numeroDecimales > 0 ? Math.Round(valorDouble, numeroDecimales).ToString() : Math.Round(valorDouble).ToString("0");

            return resultado;
        }
    }
}
