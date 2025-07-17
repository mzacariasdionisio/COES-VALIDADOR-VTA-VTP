using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class LineasFichaADet1DTO
    {
        public int LinFichaADet1Codi { get; set; }
        public int LinFichaACodi { get; set; }
        public int? Tramo { get; set; }
        public string Tipo { get; set; }
        public decimal? Longitud { get; set; }
        public string MatConductor { get; set; }
        public decimal? SecConductor { get; set; }
        public decimal? ConductorFase { get; set; }
        public decimal? CapacidadTot { get; set; }
        public decimal? CabGuarda { get; set; }
        public decimal? ResistCabGuarda { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
    }
}
