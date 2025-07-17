using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class SeccionHojasDTO
    {

        public int SeccCodi { get; set; }
        public int HojaCodi {  get; set; }
        public string SeccNombre { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string IndDel { get; set; }


    }
}
