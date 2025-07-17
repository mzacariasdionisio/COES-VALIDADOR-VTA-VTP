using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Helper
{
    public class ValidarData
    {
        public static bool ValidarEnteroDbf(string data)
        {
            try
            {
                var correcto = Convert.ToInt32(data);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        // NET 20190307 - Modificado para corregir la funcionalidad "Guardar Data DBF"
        public static bool ValidarFechaDbf(string data)
        {
            try
            {
                DateTime dtn = new DateTime();
                DateTime dt;
                var correcto = DateTime.TryParseExact(data, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                //var correcto = Convert.ToDateTime(data);
                if (dt.Equals(dtn))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static bool ValidarDecimalDbf(string data)
        {
            try
            {
                var correcto = Convert.ToDecimal(data);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}