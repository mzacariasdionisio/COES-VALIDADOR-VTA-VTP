using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_IEODCUADRO_DET
    /// </summary>
    public class EveIeodcuadroDetDTO : EntityBase
    {
        

        public int Iccodi { get; set; } 
        public int Equicodi { get; set; } 
        public string Icdetcheck1 { get; set; }

        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public string Famabrev { get; set; }
        public string Equiabrev { get; set; }
        

    }
}
