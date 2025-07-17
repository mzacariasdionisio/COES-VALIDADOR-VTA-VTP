using System;
using System.Collections.Generic;
using System.Text;

namespace fwapp
{
   public class EPSicoes
   {

      static public string shora(int a_ihora)
      {

         if (a_ihora % 2 == 0)
         {
            return (Convert.ToString(a_ihora / 2)).PadLeft(2, '0') + ":30";
         }
         else
         {
            return (Convert.ToString(a_ihora / 2 + 1)).PadLeft(2, '0') + ":00";
         }

      }
   }
}
