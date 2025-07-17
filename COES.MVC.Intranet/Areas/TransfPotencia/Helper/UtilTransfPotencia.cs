using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Helper
{
    public class UtilTransfPotencia
    {
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
    }
}