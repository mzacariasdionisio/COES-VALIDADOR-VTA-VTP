using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Yupana.Helper
{
    public class UtilYupana
    {

        /// <summary>
        /// Agregar una nueva linea al formato de salida
        /// </summary>
        /// <param name="log"></param>
        /// <param name="linea"></param>
        public static void AgregaLinea(ref string log, string linea)
        {
            log += linea + "\r\n";
        }

        /// <summary>
        /// Escribe un campo en linea de archivo Gams
        /// </summary>
        /// <param name="campo"></param>
        /// <returns></returns>
        public static string WriteArchivoGams(string campo)
        {
            int sizeColumna = campo.Length;
            //if (sizeColumna < 1)
            //    return "null";
            if (sizeColumna > ConstantesBase.ColumnaGmasSize - 3)
                campo = campo.Substring(0, ConstantesBase.ColumnaGmasSize - 3);

            return campo.PadRight(ConstantesBase.ColumnaGmasSize);
        }

        public static List<FuncionComb> ConvertirStringToFuncComb(string propiedad)
        {
            List<FuncionComb> lista = new List<FuncionComb>();
            try
            {
                FuncionComb registro;
                string[] arreglo;
                string[] par;
                decimal potencia = 0M;
                decimal consumo = 0M;
                if (!string.IsNullOrEmpty(propiedad))
                {
                    arreglo = propiedad.Split('#');
                    for (var j = 0; j < arreglo.Length; j++)
                    {
                        par = arreglo[j].Split('%');
                        if (par.Length > 1)
                        {
                            potencia = decimal.Parse(par[0]);
                            if (par[1] != null)
                                consumo = decimal.Parse(par[1]);
                            registro = new FuncionComb();
                            registro.Punto = (short)(j + 1);
                            registro.Consumo = consumo;
                            registro.Potencia = potencia;
                            lista.Add(registro);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }


            return lista;
        }
    }
}
