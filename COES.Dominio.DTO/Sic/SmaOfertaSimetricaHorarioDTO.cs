using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class SmaOfertaSimetricaHorarioDTO
    {
        public int Id { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFin { get; set; }
        public bool Estado { get; set; }
    }
}
