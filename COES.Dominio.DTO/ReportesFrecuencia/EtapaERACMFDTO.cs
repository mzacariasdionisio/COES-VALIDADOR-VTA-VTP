using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class EtapaERADTO
    {
        public int EtapaCodi { get; set; }
        public string NombreEtapa { get; set; }
        public decimal Umbral { get; set; }
        public string EtapaEstado { get; set; }
        public DateTime? FecRegi { get; set; }
        public string UsuarioRegi { get; set; }
        public DateTime? FecAct { get; set; }
        public string UsuarioAct { get; set; }
        public string Mensaje { get; set; }
    }
}
