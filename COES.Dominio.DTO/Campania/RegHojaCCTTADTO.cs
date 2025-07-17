using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RegHojaCCTTADTO
    {
        public int Centralcodi { get; set; }
        public int Proycodi { get; set; }
        public string Centralnombre { get; set; }
        public string Distrito { get; set; }
        public string Propietario { get; set; }
        public string Sociooperador { get; set; }
        public string Socioinversionista { get; set; }
        public string Tipoconcesionactual { get; set; }
        public DateTime? Fechaconcesionactual { get; set; }
        public decimal? Potenciainstalada { get; set; }
        public decimal? Potenciamaxima { get; set; }
        public decimal? Potenciaminima { get; set; }
        public string Combustibletipo { get; set; }
        public string CombustibletipoOtro { get; set; }
        public decimal? Podercalorificoinferior { get; set; }
        public string Undpci { get; set; }
        public decimal? Podercalorificosuperior { get; set; }
        public string Undpcs { get; set; }
        public decimal? Costocombustible { get; set; }
        public string Undcomb { get; set; }
        public decimal? Costotratamientocombustible { get; set; }
        public string Undtrtcomb { get; set; }
        public decimal? Costotransportecombustible { get; set; }
        public string Undtrnspcomb { get; set; }
        public decimal? Costovariablenocombustible { get; set; }
        public string Undvarncmb { get; set; }
        public decimal? Costoinversioninicial { get; set; }
        public string Undinvinic { get; set; }
        public decimal? Rendimientoplantacondicion { get; set; }
        public string Undrendcnd { get; set; }
        public decimal? Consespificacondicion { get; set; }
        public string Undconscp { get; set; }
        public string Tipomotortermico { get; set; }
        public decimal? Velnomrotacion { get; set; }
        public decimal? Potmotortermico { get; set; }
        public string Nummotorestermicos { get; set; }
        public decimal? Potgenerador { get; set; }
        public string Numgeneradores { get; set; }
        public decimal? Tensiongeneracion { get; set; }
        public decimal? Rendimientogenerador { get; set; }
        public decimal? Tensionkv { get; set; }
        public decimal? Longitudkm { get; set; }
        public string Numternas { get; set; }
        public string Nombresubestacion { get; set; }
        public string Perfil { get; set; }
        public string Prefactibilidad { get; set; }
        public string Factibilidad { get; set; }
        public string Estudiodefinitivo { get; set; }
        public string Eia { get; set; }
        public string Fechainicioconstruccion { get; set; }
        public string Periodoconstruccion { get; set; }
        public string Fechaoperacioncomercial { get; set; }
        public string Comentarios { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
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
