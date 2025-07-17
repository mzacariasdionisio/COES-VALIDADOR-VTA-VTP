using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RegHojaEolADTO
    {
        public int CentralACodi { get; set; }
        public int ProyCodi { get; set; }
        public string CentralNombre { get; set; }
        public string Distrito { get; set; }
        public string Propietario { get; set; }
        public string OtroPropietario { get; set; }
        public string SocioOperador { get; set; }
        public string SocioInversionista { get; set; }
        public string ConcesionTemporal { get; set; }
        public DateTime? FechaConcesionTemporal { get; set; }
        public string TipoConcesionActual { get; set; }
        public DateTime? FechaConcesionActual { get; set; }
        public string NombreEstacionMet { get; set; }
        public int? NumEstacionMet { get; set; }
        public string SerieVelViento { get; set; }
        public Decimal? PeriodoDisAnio { get; set; }
        public string EstudioGeologico { get; set; }
        public Decimal? PerfoDiamantinas { get; set; }
        public int? NumCalicatas { get; set; }
        public string EstudioTopografico { get; set; }
        public Decimal? LevantamientoTopografico { get; set; }
        public Decimal? PotenciaInstalada { get; set; }
        public Decimal? VelVientoInstalada { get; set; }
        public Decimal? HorPotNominal { get; set; }
        public Decimal? VelDesconexion { get; set; }
        public Decimal? VelConexion { get; set; }
        public string TipoContrCentral { get; set; }
        public string RangoVelTurbina { get; set; }
        public string TipoTurbina { get; set; }
        public Decimal? EnergiaAnual { get; set; }
        public string TipoParqEolico { get; set; }
        public string TipoTecGenerador { get; set; }
        public Decimal? NumPalTurbina { get; set; }
        public Decimal? DiaRotor { get; set; }
        public Decimal? LongPala { get; set; }
        public Decimal? AlturaTorre { get; set; }
        public Decimal? PotNomGenerador { get; set; }
        public Decimal? NumUnidades { get; set; }
        public Decimal? NumPolos { get; set; }
        public Decimal? TensionGeneracion { get; set; }
        public string Bess { get; set; }
        public Decimal? EnergiaMaxBat { get; set; }
        public Decimal? PotenciaMaxBat { get; set; }
        public Decimal? EfiCargaBat { get; set; }
        public Decimal? EfiDescargaBat { get; set; }
        public Decimal? TiempoMaxRegulacion { get; set; }
        public Decimal? RampaCargDescarg { get; set; }
        public Decimal? TensionKv { get; set; }
        public Decimal? LongitudKm { get; set; }
        public int? NumTernas { get; set; }
        public string NombreSubestacion { get; set; }
        public string NombreSubOtro { get; set; }
        public string Perfil { get; set; }
        public string Prefactibilidad { get; set; }
        public string Factibilidad { get; set; }
        public string EstudioDefinitivo { get; set; }
        public string Eia { get; set; }
        public string FechaInicioConstruccion { get; set; }
        public int? PeriodoConstruccion { get; set; }
        public string FechaOperacionComercial { get; set; }
        public string Comentarios { get; set; }
        public List<RegHojaEolADetDTO> RegHojaEolADetDTOs { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public UbicacionDTO ubicacionDTO { get; set; }

        public string Empresa { get; set; }

        public string NombreProyecto { get; set; }

        public string TipoProyecto { get; set; }

        public string SubTipoProyecto { get; set; }

        public string DetalleProyecto { get; set; }

        public string Confidencial { get; set; }
    }

}
