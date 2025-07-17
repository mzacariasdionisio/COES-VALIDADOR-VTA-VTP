using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_ESTUDIO_EO
    /// </summary>
    public class EpoEstudioEoDTO : EntityBase
    {
        public DateTime? Esteofechaopecomercial { get; set; } 
        public DateTime? Esteofechaintegracion { get; set; } 
        public DateTime? Esteofechaconexion { get; set; } 
        public int Esteocodi { get; set; } 
        public int Estacodi { get; set; } 
        public string Esteocodiusu { get; set; } 
        public string Esteonomb { get; set; } 
        public int Emprcoditp { get; set; } 
        public int Emprcoditi { get; set; } 
        public string Esteopotencia { get; set; } 
        public string Esteocapacidad { get; set; } 
        public string Esteocarga { get; set; }
        public string Esteopotenciarer { get; set; }
        public string Esteopuntoconexion { get; set; } 
        public string Esteoanospuestaservicio { get; set; } 
        public string Esteootros { get; set; } 
        public string Esteoobs { get; set; } 
        public DateTime? Esteofechaini { get; set; } 
        public string Esteoresumenejecutivotit { get; set; } 
        public string Esteoresumenejecutivoenl { get; set; } 
        public DateTime? Esteofechafin { get; set; } 
        public string Esteocertconformidadtit { get; set; } 
        public string Esteocertconformidadenl { get; set; } 
        public int? Esteoplazorevcoesporv { get; set; } 
        public int? Esteoplazorevcoesvenc { get; set; } 
        public int? Esteoplazolevobsporv { get; set; } 
        public int? Esteoplazolevobsvenc { get; set; } 
        public int? Esteoplazoalcancesvenc { get; set; } 
        public int? Esteoplazoverificacionvenc { get; set; } 
        public int? Esteoplazorevterinvporv { get; set; } 
        public int? Esteoplazorevterinvvenc { get; set; } 
        public int? Esteoplazoenvestterinvporv { get; set; } 
        public int? Esteoplazoenvestterinvvenc { get; set; } 
        public DateTime? Esteoalcancefechaini { get; set; } 
        public string Esteoalcancesolesttit { get; set; } 
        public string Esteoalcancesolestenl { get; set; } 
        public string Esteoalcancesolestobs { get; set; } 
        public DateTime? Esteoalcancefechafin { get; set; } 
        public string Esteoalcanceenviotit { get; set; } 
        public string Esteoalcanceenvioenl { get; set; } 
        public string Esteoalcanceenvioobs { get; set; } 
        public DateTime? Esteoverifechaini { get; set; } 
        public string Esteoverientregaesttit { get; set; } 
        public string Esteoverientregaestenl { get; set; } 
        public string Esteoverientregaestobs { get; set; } 
        public DateTime? Esteoverifechafin { get; set; } 
        public string Esteovericartatit { get; set; } 
        public string Esteovericartaenl { get; set; } 
        public string Esteovericartaobs { get; set; } 
        public DateTime? Esteopuestaenservfecha { get; set; } 
        public string Esteopuestaenservcomentario { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
        public string Esteojustificacion { get; set; } 
        public int? Esteoacumdiascoes { get; set; } 
        public int? Usercode { get; set; }

        public string Estadescripcion { get; set; }
        public string Emprnomb { get; set; }
        public string Username { get; set; }
        public string Terceroinvolucrado { get; set; }

        public string Esteoalcanceestadocolor { get; set; }
        public string Esteoalcanceestado { get; set; }

        public string Esteoveriestadocolor { get; set; }
        public string Esteoveriestado { get; set; }

        public int TotNroDiasCoes { get; set; }
        public int DiasHabilesTotales { get; set; }
        public int TotNroDiasTercerInv { get; set; }
        public int TotNroDiasTitProyect { get; set; }
        public string PromNroDiasHabilesCoes { get; set; }
        public string Esteocodiproy { get; set; }
        public string Esteoresponsable { get; set; }

        public List<int> Esteoterinvcodi { get; set; }
        public string Esteorespes { get; set; }
        public int Esteorescodi { get; set; }

        public int tipoProyecto { get; set; }

        public DateTime? FechaIniPresentacion { get; set; }
        public DateTime? FechaFinPresentacion { get; set; }
        public String FIniPresentacion { get; set; }
        public String FFinPresentacion { get; set; }
        public DateTime? FechaIniConformidad { get; set; }
        public DateTime? FechaFinConformidad { get; set; }
        public String FIniConformidad { get; set; }
        public String FFinConformidad { get; set; }
        public int Zoncodi { get; set; }
        public int? PuntCodi { get; set; }

        public string EsteoTipoProyecto { get; set; }

        

        public int TipoConfig { get; set; }

        public string EsteoAbsTit { get; set; }
        public string EsteoAbsEnl { get; set; }
        public DateTime? EsteoAbsFFin { get; set; }
        public string EsteoAbsObs { get; set; }

        public int? Esteoplazoverificacionvencabs { get; set; }
        public string EsteoAbsestadocolor { get; set; }
        public string EsteoAbsestado { get; set; }

        #region Mejoras EO-EPO
        public string EsteoVigencia { get; set; }
        #endregion
        #region Mejoras EO-EPO-II
        public string ZonDescripcion { get; set; }
        #endregion
    }
}
