using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class BioHojaADTO
    {
        public int BiohojaaCodi { get; set; }
        public int ProyCodi { get; set; }
        public string CentralNombre { get; set; }
        public string Distrito { get; set; }
        public string Propietario { get; set; }
        public string Otro { get; set; }
        public string SocioOperador { get; set; }
        public string SocioInversionista { get; set; }
        public string ConTemporal { get; set; }
        public DateTime? FecAdjudicacionTemp { get; set; }
        public string TipoConActual { get; set; }
        public DateTime? FecAdjudicacionAct { get; set; }
        public Decimal? PotInstalada { get; set; }
        public string TipoNomComb { get; set; }
        public string OtroComb { get; set; }
        public Decimal? PotMaxima { get; set; }
        public Decimal? PoderCalorInf { get; set; }
        public string CombPoderCalorInf { get; set; }
        public Decimal? PotMinima { get; set; }
        public Decimal? PoderCalorSup { get; set; }
        public string CombPoderCalorSup { get; set; }
        public Decimal? CostCombustible { get; set; }
        public string CombCostoCombustible { get; set; }
        public Decimal? CostTratamiento { get; set; }
        public string CombCostTratamiento { get; set; }
        public Decimal? CostTransporte { get; set; }
        public string CombCostTransporte { get; set; }
        public Decimal? CostoVariableNoComb { get; set; }
        public string CombCostoVariableNoComb { get; set; }
        public Decimal? CostInversion { get; set; }
        public string CombCostoInversion { get; set; }
        public Decimal? RendPlanta { get; set; }
        public string CombRendPlanta { get; set; }
        public Decimal? ConsEspec { get; set; }
        public string CombConsEspec { get; set; }
        public string TipoMotorTer { get; set; }
        public Decimal? VelNomRotacion { get; set; }
        public Decimal? PotEjeMotorTer { get; set; }
        public Decimal? NumMotoresTer { get; set; }
        public Decimal? PotNomGenerador { get; set; }
        public Decimal? NumGeneradores { get; set; }
        public Decimal? TipoGenerador { get; set; }
        public Decimal? TenGeneracion { get; set; }
        public Decimal? Tension { get; set; }
        public Decimal? Longitud { get; set; }
        public Decimal? NumTernas { get; set; }
        public string NomSubEstacion { get; set; }
        public string OtroSubEstacion { get; set; }
        public string Perfil { get; set; }
        public string Prefactibilidad { get; set; }
        public string Factibilidad { get; set; }
        public string EstDefinitivo { get; set; }
        public string Eia { get; set; }
        public string FecInicioConst { get; set; }
        public string PeriodoConst { get; set; }
        public string FecOperacionComer { get; set; }
        public string Comentarios { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public string Empresa { get; set; }

        public string NombreProyecto { get; set; }

        public string TipoProyecto { get; set; }

        public string SubTipoProyecto { get; set; }

        public string DetalleProyecto { get; set; }
        public string Condifencial { get; set; }

        public UbicacionDTO ubicacionDTO { get; set; }

    }
}
