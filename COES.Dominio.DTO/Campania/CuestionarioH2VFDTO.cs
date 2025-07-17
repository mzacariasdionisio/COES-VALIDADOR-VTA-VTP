using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CuestionarioH2VFDTO
    {
        public int H2vFCodi { get; set; }
        public int ProyCodi { get; set; }
        public Decimal? EstudioFactibilidad { get; set; }
        public Decimal? InvestigacionesCampo { get; set; }
        public Decimal? GestionesFinancieras { get; set; }
        public Decimal? DisenosPermisos { get; set; }
        public Decimal? ObrasCiviles { get; set; }
        public Decimal? Equipamiento { get; set; }
        public Decimal? LineaTransmision { get; set; }
        public Decimal? Administracion { get; set; }
        public Decimal? Aduanas { get; set; }
        public Decimal? Supervision { get; set; }
        public Decimal? GastosGestion { get; set; }
        public Decimal? Imprevistos { get; set; }
        public Decimal? Igv { get; set; }
        public Decimal? OtrosGastos { get; set; }
        public Decimal? InversionTotalSinIgv { get; set; }
        public Decimal? InversionTotalConIgv { get; set; }
        public string FinanciamientoTipo { get; set; }
        public string FinanciamientoEstado { get; set; }
        public Decimal? PorcentajeFinanciado { get; set; }
        public string ConcesionDefinitiva { get; set; }
        public string VentaEnergia { get; set; }
        public string EjecucionObra { get; set; }
        public string ContratosFinancieros { get; set; }
        public string Observaciones { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public string Empresa { get; set; }
        public string NombreProyecto { get; set; }
        public string TipoProyecto { get; set; }
        //public string SubTipoProyecto { get; set; }
        public string DetalleProyecto { get; set; }
        public string Confidencial { get; set; }

    }
}
