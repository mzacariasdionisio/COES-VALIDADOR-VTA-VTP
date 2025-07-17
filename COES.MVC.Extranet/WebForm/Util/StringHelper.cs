using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSIC2010.Util
{
    public class StringHelper
    {
        public static string nf_get_random_str(int li_tam_pwd)
        {
            char[] chArray = new char[] { 
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
        'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 
        'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
        'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
            int num = 0;
            string str = "";
            DateTime today = DateTime.Today;
            int introduced10 = DateTime.DaysInMonth(today.Year, today.Month);
            num = introduced10 + (((today.Day + today.Hour) + today.Minute) + today.Second);
            Random random = new Random(DateTime.Now.Millisecond + num);
            int length = chArray.Length;
            for (int i = 0; i < li_tam_pwd; i++)
            {
                int index = random.Next(0, length);
                str = str + chArray[index].ToString();
            }
            return str;
        }

        //public static int nf_get_random_nbr(int li_tam)

        public static int f_getpass(int al_par1, string as_par2)
        {
            int ll_sumac;
            ll_sumac = 0;
            for (int i = 0; i < as_par2.Length; i++)
                ll_sumac = (ll_sumac * (i + 1)) % 123456 + as_par2[i] * (int)Math.Pow(as_par2.Length, 2);// ^no es potenciación

            ll_sumac = ll_sumac + al_par1 * as_par2.Length;
            return ll_sumac;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

 

 

    }
}