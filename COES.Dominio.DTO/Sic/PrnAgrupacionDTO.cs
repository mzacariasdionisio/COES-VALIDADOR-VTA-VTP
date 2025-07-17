using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnAgrupacionDTO
    {
        public int Agrupcodi { get; set; }
        public int Ptogrpcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Agrupfactor { get; set; }
        public DateTime Agrupfechaini { get; set; }
        public DateTime Agrupfechafin { get; set; }

        public int Areacodi { get; set; }
        public string Areanomb { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Ptomedidesc { get; set; }
        public int Ptogrppronostico { get; set; }
        public int Ptogrphijocodi { get; set; }
        public string Ptogrphijodesc { get; set; }
        public decimal Meditotal { get; set; }
        public int Prnm48tipo { get; set; }
        public int Tipoemprcodi { get; set; }
        public int Prnmestado { get; set; }


        //Auxiliares
        public List<object> ListaDetalle { get; set; }
        public int Grupocodibarra { get; set; }
        public string Prnpmpvarexoproceso { get; set; }

    }
}
