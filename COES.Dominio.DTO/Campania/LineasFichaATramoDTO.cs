using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class LineasFichaATramoDTO
    {
        public int LinFichaACodi { get; set; }
        public int ProyCodi { get; set; }
        public string NombreLinea { get; set; }
        public int LinFichaADet1Codi { get; set; }
        public int? Tramo { get; set; }
        public string Tipo { get; set; }
        public decimal? Longitud { get; set; }
        public string MatConductor { get; set; }
        public decimal? SecConductor { get; set; }
        public decimal? ConductorFase { get; set; }
        public decimal? CapacidadTot { get; set; }
        public decimal? CabGuarda { get; set; }
        public decimal? ResistCabGuarda { get; set; }
        public int LinFichaADet2Codi { get; set; }
        public decimal? R { get; set; }
        public decimal? X { get; set; }
        public decimal? B { get; set; }
        public decimal? G { get; set; }
        public decimal? R0 { get; set; }
        public decimal? X0 { get; set; }
        public decimal? B0 { get; set; }
        public decimal? G0 { get; set; }
        public string Empresa { get; set; }
        public string NombreProyecto { get; set; }
        public string TipoProyecto { get; set; }
        public string SubTipoProyecto { get; set; }
        public string DetalleProyecto { get; set; }
        public string Condifencial { get; set; }
    }
}
