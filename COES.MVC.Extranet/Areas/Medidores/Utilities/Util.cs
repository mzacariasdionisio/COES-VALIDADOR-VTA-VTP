using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Utilities
{
    public class Util
    {
        public static Boolean VerificarPlazoEnvio(DateTime fecha)
        {
            Boolean resultado= true;

            return resultado;
        }

        public static Boolean EsNumero(string numString)
        {
            Boolean isNumber = false;
            long number1 = 0;
            bool canConvert = long.TryParse(numString, out number1);
            if (canConvert == true)
                isNumber = true;
            else
            {
                byte number2 = 0;
                canConvert = byte.TryParse(numString, out number2);
                if (canConvert == true)
                    isNumber = true;
                else
                {
                    double number3 = 0;

                    canConvert = double.TryParse(numString, out number3);
                    if (canConvert == true)
                        isNumber = true;

                }
            }
            return isNumber;
        }

        public static Boolean EsFecha(string stfecha)
        {
            DateTime fechaResult;
            Boolean isFecha = false;
            bool canConvert = DateTime.TryParse(stfecha, out fechaResult);
            if (canConvert == true)
                isFecha = true;
            return isFecha;
        }

        public static string ConvertirColLetra(int iCol)
        {
            int iAlpha;
            int iRemainder;
            string convert = "";
            iAlpha = (int)(iCol / 27);
            iRemainder = iCol - (iAlpha * 26);
            if (iAlpha > 0)
            {
                convert = (char)(iAlpha + 64) + "";
            }
            if (iRemainder > 0)
            {
                convert += (char)(iRemainder + 64);
            }
            return convert;

        }

        public static DateTime FromExcelSerialDate(double SerialDate)
        {
            if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
            return new DateTime(1899, 12, 31).AddDays(SerialDate);
        }

        public static string ConvertirHoraMin(int horamin)
        {
            string hora =   (horamin / 4).ToString();
            int len = hora.Length;
            hora = ("0" + hora).Substring((len > 1) ? 1 : 0, 2);
            string minuto = (horamin % 4).ToString();
            len = minuto.Length;
            minuto = ("0" + minuto).Substring((len > 1) ? 1 : 0, 2);
            return hora + ":" + minuto;
        }
    }
}