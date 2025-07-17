using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_CUADRO_PROG
    /// </summary>
    public class RcaCuadroProgDTO : EntityBase
    {
        public int Rccuadcodi { get; set; } 
        public int Rcprogcodi { get; set; } 
        public decimal? Rccuadenergiaracionar { get; set; } 
        public string Rccuadmotivo { get; set; } 
        public string Rccuadbloquehor { get; set; } 
        public int Rcconpcodi { get; set; } 
        public string Rccuadflageracmf { get; set; }
        public string Rccuadflageracmt { get; set; } 
        public string Rccuadflagregulado { get; set; } 
        public DateTime? Rccuadfechorinicio { get; set; } 
        public DateTime? Rccuadfechorfin { get; set; } 
        public string Rccuadestregistro { get; set; } 
        public string Rccuadusucreacion { get; set; } 
        public DateTime Rccuadfeccreacion { get; set; } 
        public string Rccuadusumodificacion { get; set; } 
        public DateTime? Rccuadfecmodificacion { get; set; }

        public int Rchorpcodi { get; set; }
        public string Rcprognombre { get; set; }       
        public string Rcconpnombre { get; set; }
        public string Rchorpnombre { get; set; }

        //public string Rccuadestado { get; set; }

        public string Rcprogabrev { get; set; }

        public decimal Rccuadumbral { get; set; }

        public DateTime? Rccuadfechorinicioejec { get; set; }
        public DateTime? Rccuadfechorfinejec { get; set; }

        public int Rcestacodi { get; set; }
        public string Rcestanombre { get; set; }
        public int Rccuadcodipadre { get; set; }

        public string Rccuadubicacion { get; set; }

        public string Rccuadcodeventoctaf { get; set; }
    }
}
