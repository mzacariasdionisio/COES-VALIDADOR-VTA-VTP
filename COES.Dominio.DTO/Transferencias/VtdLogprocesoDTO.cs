using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class VtdLogProcesoDTO
    {
        public decimal Logpcodi { get; set; }
        public decimal Valocodi { get; set; }
        public DateTime Logpfecha { get; set; }
        public DateTime Logphorainicio { get; set; }
        public DateTime Logphorafin { get; set; }
        public string Logplog { get; set; }
        public char Logptipo { get; set; }
        public char Logpestado { get; set; }
        public string Logpusucreacion { get; set; }
        public DateTime? Logpfeccreacion { get; set; }
        public string Logpusumodificacion { get; set; }
        public DateTime? Logpfecmodificacion { get; set; }
        public DateTime? Valofecha { get; set; }
    }
}
