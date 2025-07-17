using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_CUADRO_EJEC_USU_DET
    /// </summary>
    public class RcaCuadroEjecUsuarioDetDTO : EntityBase
    {
        public int Rcejeucodi { get; set; }
        public int Rcejedcodi { get; set; }
        public int Emprcodi { get; set; }        
        public decimal Rcejedpotencia { get; set; }        
        public DateTime Rcejedfechor { get; set; } 

        public string Rcejedusucreacion { get; set; } 
        public DateTime Rcejedfeccreacion { get; set; } 
        public string Rcejedusumodificacion { get; set; } 
        public DateTime? Rcejedfecmodificacion { get; set; }

        public string Empresa { get; set; }
        //public string Suministrador { get; set; }
        //public string Subestacion { get; set; }
        //public string Puntomedicion { get; set; }
       
    }
}
