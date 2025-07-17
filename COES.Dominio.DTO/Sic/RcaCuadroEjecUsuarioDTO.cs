using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_CUADRO_EJEC_USUARIO
    /// </summary>
    public class RcaCuadroEjecUsuarioDTO : EntityBase
    {
        public int Rcejeucodi { get; set; } 
        public int Rcproucodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int Equicodi { get; set; } 
        public decimal Rcejeucargarechazada { get; set; } 
        public string Rcejeutiporeporte { get; set; } 
        public DateTime Rcejeufechorinicio { get; set; } 
        public DateTime Rcejeufechorfin { get; set; } 
        public string Rcejeuestregistro { get; set; } 
        public string Rcejeuusucreacion { get; set; } 
        public DateTime Rcejeufeccreacion { get; set; } 
        public string Rcejeuusumodificacion { get; set; } 
        public DateTime? Rcejeufecmodificacion { get; set; }

        public string Empresa { get; set; }
        public string Suministrador { get; set; }
        public string Subestacion { get; set; }
        public string Puntomedicion { get; set; }

        public decimal Rcproudemanda { get; set; }
        public string Rcproufuente { get; set; }
        public decimal Rcproudemandaracionar { get; set; }

        public int Rcprouemprcodisuministrador { get; set; } 

        public string CeldaFechaInicio { get; set; }
        public string CeldaFechaFin { get; set; }

        public decimal Rcproudemandareal { get; set; }
    }
}
