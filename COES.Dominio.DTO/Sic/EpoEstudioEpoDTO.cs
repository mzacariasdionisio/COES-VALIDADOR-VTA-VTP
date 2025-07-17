using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_ESTUDIO_EPO
    /// </summary>
    public class EpoEstudioEpoDTO : EntityBase
    {
        public int Estepocodi { get; set; } 
        public int Estacodi { get; set; } 
        public string Estepocodiusu { get; set; } 
        public string Esteponomb { get; set; } 
        public int Emprcoditp { get; set; } 
        public int Emprcoditi { get; set; } 
        public string Estepopotencia { get; set; } 
        public string Estepocapacidad { get; set; } 
        public string Estepocarga { get; set; } 
        public string Estepopuntoconexion { get; set; } 
        public string Estepoanospuestaservicio { get; set; } 
        public string Estepootros { get; set; } 
        public string Estepoobs { get; set; } 
        public DateTime? Estepofechaini { get; set; } 
        public string Esteporesumenejecutivotit { get; set; } 
        public string Esteporesumenejecutivoenl { get; set; } 
        public DateTime? Estepofechafin { get; set; } 
        public string Estepocertconformidadtit { get; set; } 
        public string Estepocertconformidadenl { get; set; } 
        public int? Estepoplazorevcoesporv { get; set; } 
        public int? Estepoplazorevcoesvenc { get; set; } 
        public int? Estepoplazolevobsporv { get; set; } 
        public int? Estepoplazolevobsvenc { get; set; } 
        public int? Estepoplazoalcancesvenc { get; set; } 
        public int? Estepoplazoverificacionvenc { get; set; } 
        public int? Estepoplazorevterinvporv { get; set; } 
        public int? Estepoplazorevterinvvenc { get; set; } 
        public int? Estepoplazoenvestterinvporv { get; set; } 
        public int? Estepoplazoenvestterinvvenc { get; set; } 
        public DateTime? Estepoalcancefechaini { get; set; } 
        public string Estepoalcancesolesttit { get; set; } 
        public string Estepoalcancesolestenl { get; set; } 
        public string Estepoalcancesolestobs { get; set; } 
        public DateTime? Estepoalcancefechafin { get; set; } 
        public string Estepoalcanceenviotit { get; set; } 
        public string Estepoalcanceenvioenl { get; set; } 
        public string Estepoalcanceenvioobs { get; set; } 
        public DateTime? Estepoverifechaini { get; set; } 
        public string Estepoverientregaesttit { get; set; } 
        public string Estepoverientregaestenl { get; set; } 
        public string Estepoverientregaestobs { get; set; } 
        public DateTime? Estepoverifechafin { get; set; } 
        public string Estepovericartatit { get; set; } 
        public string Estepovericartaenl { get; set; } 
        public string Estepovericartaobs { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
        public string Estepojustificacion { get; set; } 
        public int? Estepoacumdiascoes { get; set; } 
        public int? Usercode { get; set; }


        public string Estadescripcion { get; set; }
        public string Emprnomb { get; set; }
        public string Username { get; set; }
        public string Terceroinvolucrado { get; set; }

        public string Estepoalcanceestadocolor { get; set; }
        public string Estepoalcanceestado { get; set; }

        public string Estepoveriestadocolor { get; set; }
        public string Estepoveriestado { get; set; }

        public int TotNroDiasCoes { get; set; }
        public int DiasHabilesTotales { get; set; }
        public int TotNroDiasTercerInv { get; set; }
        public int TotNroDiasTitProyect { get; set; }
        public string PromNroDiasHabilesCoes { get; set; }
        public string Estepocodiproy { get; set; }
        public string Esteporesponsable { get; set; }
        public int TipoConfig { get; set; }

        public List<int> Estepoterinvcodi { get; set; }


        public DateTime? FechaIniPresentacion { get; set; }
        public DateTime? FechaFinPresentacion { get; set; }
        public DateTime? FechaIniConformidad { get; set; }
        public DateTime? FechaFinConformidad { get; set; }

        public String FIniPresentacion { get; set; }
        public String FFinPresentacion { get; set; }
        public String FIniConformidad { get; set; }
        public String FFinConformidad { get; set; }

        public int tipoProyecto { get; set; }
        public int Zoncodi { get; set; }
        public int? PuntCodi { get; set; }
        public string EstepoTipoProyecto { get; set; }


        public string EstepoAbsTit { get; set; }
        public string EstepoAbsEnl { get; set; }
        public DateTime? EstepoAbsFFin { get; set; }
        public string EstepoAbsObs { get; set; }

        public int? Estepoplazoverificacionvencabs { get; set; }
        public string EstepoAbsestadocolor { get; set; }
        public string EstepoAbsestado { get; set; }

        #region Mejoras EO-EPO
        public string EstepoVigencia { get; set; }
        public int? Estacodivigencia { get; set; }
        #endregion
        #region Mejoras EO-EPO-II
        public string ZonDescripcion { get; set; }
        #endregion
    }
}
