using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class SolHojaADTO
    {

        public int Solhojaacodi { get; set; }
        public int Proycodi { get; set; }
        public string Centralnombre { get; set; }
        public string Distrito { get; set; }
        public string Propietario { get; set; }
        public string Otro { get; set; }
        public string Sociooperador { get; set; }
        public string Socioinversionista { get; set; }
        public string Concesiontemporal { get; set; }
        public string Tipoconcesionact { get; set; }
        public DateTime? Fechaconcesiontem { get; set; }
        public DateTime? Fechaconcesionact { get; set; }
        public string Nomestacion { get; set; }
        public string Serieradiacion { get; set; }
        public decimal? Potinstnom { get; set; }
        public decimal? Ntotalmodfv { get; set; }
        public decimal? Horutilequ { get; set; }
        public decimal? Eneestanual { get; set; }
        public decimal? Facplantaact { get; set; }
        public string Tecnologia { get; set; }
        public decimal? Potenciapico { get; set; }
        public decimal? Nivelradsol { get; set; }
        public string Seguidorsol { get; set; }
        public decimal? Volpunmax { get; set; }
        public decimal? Intpunmax { get; set; }
        public string Modelo { get; set; }
        public decimal? Entpotmax { get; set; }
        public decimal? Salpotmax { get; set; }
        public string Siscontro { get; set; }
        public string Baterias { get; set; }
        public decimal? Enemaxbat { get; set; }
        public decimal? Potmaxbat { get; set; }
        public decimal? Eficargamax { get; set; }
        public decimal? Efidesbat { get; set; }
        public decimal? Timmaxreg { get; set; }
        public decimal? Rampascardes { get; set; }
        public decimal? Tension { get; set; }
        public decimal? Longitud { get; set; }
        public decimal? Numternas { get; set; }
        public string Nombsubest { get; set; }
        public string Perfil { get; set; }
        public string Prefact { get; set; }
        public string Factibilidad { get; set; }
        public string Estdefinitivo { get; set; }
        public string Eia { get; set; }
        public string Fecinicioconst { get; set; }
        public string Perconstruccion { get; set; }
        public string Fecoperacioncom { get; set; }
        public string Comentarios { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime? FecModificacion { get; set; }
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

