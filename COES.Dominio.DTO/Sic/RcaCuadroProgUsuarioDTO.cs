using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_CUADRO_PROG_USUARIO
    /// </summary>
    public class RcaCuadroProgUsuarioDTO : EntityBase
    {
        public string Rcprouusumodificacion { get; set; } 
        public DateTime? Rcproufecmodificacion { get; set; } 
        public int Rcproucodi { get; set; } 
        public int Rccuadcodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int? Equicodi { get; set; } 
        public decimal Rcproudemanda { get; set; } 
        public string Rcproufuente { get; set; } 
        public decimal Rcproudemandaracionar { get; set; } 
        public string Rcprouestregistro { get; set; } 
        public string Rcprouusucreacion { get; set; } 
        public DateTime Rcproufeccreacion { get; set; }
        public decimal Rcproucargadisponible { get; set; }
        public decimal Rcproufactork { get; set; }

        public string Empresa { get; set; }
        public string Suministrador { get; set; }
        public string Subestacion { get; set; }
        public string Puntomedicion { get; set; }
        public decimal Rcproudemandaatender { get; set; }

        public decimal Demanda { get; set; }
        public string Rcdeulfuente { get; set; }

        public decimal Rcejeucargarechazadapreliminar { get; set; }
        public DateTime Rcejeufechoriniciopreliminar { get; set; }
        public DateTime Rcejeufechorfinpreliminar { get; set; }
        public decimal Rcejeucargarechazada { get; set; }
        public DateTime Rcejeufechorinicio { get; set; }
        public DateTime Rcejeufechorfin { get; set; }
        public string Cumplio { get; set; }

        public int Rcprouemprcodisuministrador { get; set; }

        public decimal Rcproucargaesencial { get; set; }

        public string Osinergcodi { get; set; }

        public decimal? Rcproucargarechazarcoord { get; set; }
        public string Rccuadhorinicoord { get; set; }
        public string Rccuadhorfincoord { get; set; }
        public decimal? Rcproucargarechazarejec { get; set; }
        public string Rccuadhoriniejec { get; set; }
        public string Rccuadhorfinejec { get; set; }

        public decimal Duracion { get; set; }

        public string RcejeufechorinicioRep { get; set; }
        public string RcejeufechorfinRep { get; set; }

        public string RccuadfechoriniRep { get; set; }
        public string RccuadfechorfinRep { get; set; }

        public decimal RcreevpotenciaprompreviaRep { get; set; }
        public string RccuadfechoriniPrevioRep { get; set; }
        public string RccuadfechorfinPrevioRep { get; set; }
        public string Evaluacion { get; set; }

        public decimal Rcproudemandareal { get; set; }
    }
}
