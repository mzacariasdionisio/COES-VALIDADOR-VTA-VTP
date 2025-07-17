using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.SGDoc
{
    public class ReporteDTO
    {     

        public int Fljcodi { get; set; }
        public int Fljdetcodi { get; set; }
        public int Correlativo { get; set; }
        public string Remitente { get; set; }
        public string Destino { get; set; }
        public int Areaorig { get; set; }
        public int Areadestino { get; set; }
        public string NumDocumento { get; set; }
        public string Asunto { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string Xfileruta { get; set; }
        public string Estado { get; set; }
        public string Fljcadatencion { get; set; }
        public string ComentarioPadre { get; set; }
        public DateTime Fljfechaproce { get; set; }
        public DateTime Fmax { get; set; }
        public DateTime Fasignacion { get; set; }
        public int Fljdiasmaxaten { get; set; }
        public DateTime FechaMaxAtencion { get; set; }
        public bool TiempoMaxAtencion { get; set; }
        public string ColorLetra { get; set; }
        public string ColorFondo { get; set; }
    }
}
