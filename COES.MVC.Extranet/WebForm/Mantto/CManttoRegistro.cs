using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace WSIC2010
{
    public class CManttoRegistroX
    {
        public int ii_regcodi = -1;
        public int ii_evenclasecodi = -1;
        public string  is_evenclasedesc = "";
        public DateTime idt_FechaIni = new DateTime(2001, 1, 1, 0, 0, 0);
        public DateTime idt_FechaFin = new DateTime(2001, 1, 1, 0, 0, 0);

        public override string ToString()
        {
            string ls_rangofecha;
            if (idt_FechaIni == idt_FechaFin)
                ls_rangofecha = idt_FechaIni.ToString("dd/MM/yyyy");
            else
                ls_rangofecha = idt_FechaIni.ToString("dd/MM/yyyy") + " - " + idt_FechaFin.ToString("dd/MM/yyyy");

            return "MANT. " + is_evenclasedesc + " " + ls_rangofecha;
        }

        public int nf_Cargar(DataRow dr)
        {
            ii_regcodi = Convert.ToInt32(dr["REGCODI"]);
            idt_FechaIni = (DateTime)dr["FECHAINI"];
            idt_FechaFin = (DateTime)dr["FECHAFIN"];
            ii_evenclasecodi = Convert.ToInt32(dr["EVENCLASECODI"]);
            is_evenclasedesc = Convert.ToString(dr["EVENCLASEDESC"]);
            
            return 1;
        }
    }
}