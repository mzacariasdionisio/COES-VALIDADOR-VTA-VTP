using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RCG_COSTOMARGINAL_CAB
    /// </summary>
    public class RcgCostoMarginalCabDTO : EntityBase
    {
        public int Rccmgccodi { get; set; }

        public int Pericodi { get; set; }
        public int Recacodi { get; set; }
        public string Rccmgcusucreacion { get; set; }
        public DateTime Rccmgcfeccreacion { get; set; }
        public string Rccmgcusumodificacion { get; set; }
        public DateTime? Rccmgcfecmodificacion { get; set; }
       

    }
}
