using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class SubestFicha1DTO
    {
        public int SubestFicha1Codi { get; set; }
        public int ProyCodi { get; set; }
        public string NombreSubestacion { get; set; }
        public string TipoProyecto { get; set; }
        public string FechaPuestaServicio { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string SistemaBarras { get; set; }
        public string OtroSistemaBarras { get; set; }
        public int NumTrafo { get; set; }
        public int NumEquipos { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
        public List<SubestFicha1Det1DTO> Lista1DTOs { get; set; }
        public List<SubestFicha1Det2DTO> Lista2DTOs { get; set; }
        public List<SubestFicha1Det3DTO> Lista3DTOs { get; set; }

    }
}
