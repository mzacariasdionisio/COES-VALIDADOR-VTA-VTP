using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla tempoal RCA_EVALUACION_RMC_TMP
    /// </summary>
    public class RcaEvaluacionTempoDTO : EntityBase
    {
        public string Rcermccodeventoctaf { get; set; }        
        public int Emprcodi { get; set; } 
        public int Equicodi { get; set; }
        public DateTime Rcermcfechorini { get; set; }
        public DateTime Rcermcfechorfin { get; set; }

        public decimal? Rcermcpotenciarechazar { get; set; } 
        public decimal? Rcermcpotencianorechazada { get; set; } 
        public decimal? Rcermcenergiadebiorechazar { get; set; } 
        public decimal? Rcermcpotenciapromprevia { get; set; } 

        public int Rcermcgrupo { get; set; }
        public int Rcejeucodi { get; set; }
        public decimal? Rcermcpotenciarechazada { get; set; }

        //public string Rcprouusucreacion { get; set; } 
        //public DateTime Rcproufeccreacion { get; set; }
        //public decimal Rcproucargadisponible { get; set; }
        //public decimal Rcproufactork { get; set; }

        //public string Empresa { get; set; }
        //public string Suministrador { get; set; }
        //public string Subestacion { get; set; }
        //public string Puntomedicion { get; set; }
        //public decimal Rcproudemandaatender { get; set; }

        //public decimal Demanda { get; set; }
        //public string Rcdeulfuente { get; set; }

        //public decimal Rcejeucargarechazadapreliminar { get; set; }
        //public DateTime Rcejeufechoriniciopreliminar { get; set; }
        //public DateTime Rcejeufechorfinpreliminar { get; set; }
        //public decimal Rcejeucargarechazada { get; set; }
        //public DateTime Rcejeufechorinicio { get; set; }
        //public DateTime Rcejeufechorfin { get; set; }
        //public string Cumplio { get; set; }

        //public int Rcprouemprcodisuministrador { get; set; }

        //public decimal Rcproucargaesencial { get; set; }

        //public string Osinergcodi { get; set; }

        //public decimal? Rcproucargarechazarcoord { get; set; }
        //public string Rccuadhorinicoord { get; set; }
        //public string Rccuadhorfincoord { get; set; }
        //public decimal? Rcproucargarechazarejec { get; set; }
        //public string Rccuadhoriniejec { get; set; }
        //public string Rccuadhorfinejec { get; set; }

        //public decimal Duracion { get; set; }

        //public string RcejeufechorinicioRep { get; set; }
        //public string RcejeufechorfinRep { get; set; }

        //public string RccuadfechorfinRep { get; set; }
    }
}
