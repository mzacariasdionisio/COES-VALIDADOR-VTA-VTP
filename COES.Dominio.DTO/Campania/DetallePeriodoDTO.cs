using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class DetallePeriodoDTO
    {
        public int DetPeriCodigo { get; set; }

        public int PeriCodigo { get; set; }

        public int HojaCodigo { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string IndDel { get; set; }


    }
}
