using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{    
    public class PrGrupoRelasoDTO : EntityBase
    {
        public int Grrasocodi { get; set; }
        public int? Grrdefcodi { get; set; }
        public int? Grupocodi1 { get; set; }
        public int? Grupocodi2 { get; set; }        
        public DateTime? Grrasofechadesde { get; set; }
        public DateTime? Grrasofechahasta { get; set; }
        public int? Grrasosecuencia { get; set; }
        public string Grrasotag { get; set; }
        public string Grrasousucreacion { get; set; }
        public DateTime? Grrasofeccreacion { get; set; }
        public string Grrasousumodificacion { get; set; }
        public DateTime? Grrasofecmodificacion { get; set; }

        //Campos Adicionales
        public string Tiporel { get; set; }
        public int? Codsddp { get; set; }
        public string descsddp { get; set; }
        public int? Codsic { get; set; }
        public string Descsic { get; set; } 
        

    }
}
