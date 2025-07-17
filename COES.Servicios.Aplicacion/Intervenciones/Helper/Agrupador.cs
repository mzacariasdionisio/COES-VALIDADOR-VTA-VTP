using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Aplicacion.Intervenciones.Helper
{
    public class Agrupador
    {
        public string EmprNomb { get; set; }
        public string AreaNomb { get; set; }
        public string AreaAbrev { get; set; }
        public string Famabrev { get; set; }
        public string Equiabrev { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public int Areacodi { get; set; }
        public DateTime Interfechaini { get; set; }
        public DateTime Interfechafin { get; set; }

        public List<InIntervencionDTO> Children { get; set; }

        public string CeldaFecha { get; set; }
        public int TotalHoras { get; set; }
        public string CeldaComentario { get; set; }
    }
}
