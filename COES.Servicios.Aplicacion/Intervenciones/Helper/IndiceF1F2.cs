using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Intervenciones.Helper
{
    public class IndiceF1F2
    {
        public string EmprNomb { get; set; }
        public string Areaabrev { get; set; }       
        public string Equiabrev { get; set; }       
        public int APE { get; set; }
        public int APNE { get; set; }
        public int AENP { get; set; }
        public int AEmAP { get; set; }
    }

    public class FactorF1F2InputWeb
    {
        public string Emprcodi { get; set; }
        public string Areacodi { get; set; }
        public string Equicodi { get; set; }
        public string Frecuenciacodi { get; set; }
        public string Infverfechaperiodo { get; set; }
        public string Infvercodi { get; set; }
        public string HorasIndispo { get; set; }
        public string InterFechaIni { get; set; }
        public string InterFechaFin { get; set; }
        public string Disponibilidad { get; set; }
    }

    public class FactorF1F2Filtro
    {
        public string StrIdsEmpresa { get; set; }
        public List<int> ListaEmprcodi { get; set; } = new List<int>();
        public string StrIdsArea { get; set; }
        public List<int> ListaAreacodi { get; set; } = new List<int>();
        public string StrIdsEquipo { get; set; }
        public List<int> ListaEquicodi { get; set; } = new List<int>();
        public string StrIdsFrecuencia { get; set; }
        public List<string> ListaFrecuenciacodi { get; set; } = new List<string>();
        public string StrDisponibilidad { get; set; }

        public int Infvercodi { get; set; }
        public int HoraIndisp { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
