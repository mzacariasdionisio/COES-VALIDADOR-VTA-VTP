using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgoCoesDesktopReporteHidrologia
{
    internal class ReporteData
    {
        public int PtoMedicionCodi { get; set; }
        public string PtoMedicionDesc { get; set; }
        public string TipoInfoDesc { get; set; }
        public string MediFecha { get; set; }
        public double Valor { get; internal set; }

        public string getInfo() {
            string cadena = String.Format("PtoMedicionCodi => {0}" + Environment.NewLine, PtoMedicionCodi);
            cadena += String.Format("PtoMedicionDesc => {0}" + Environment.NewLine, PtoMedicionDesc);
            cadena += String.Format("TipoInfoDesc => {0}" + Environment.NewLine, TipoInfoDesc);
            cadena += String.Format("MediFecha => {0}" + Environment.NewLine, MediFecha);
            cadena += String.Format("Valor => {0}" + Environment.NewLine, Valor);
            return cadena;
        }
    }
}
