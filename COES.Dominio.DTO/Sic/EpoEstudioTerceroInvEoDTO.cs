using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_ESTUDIOTERCEROINV_EO
    /// </summary>
    public class EpoEstudioTerceroInvEoDTO : EntityBase
    {
        public int Inveocodi { get; set; }
        public int Esteocodi { get; set; }
        public int Esteoemprcodi { get; set; }
        public DateTime Lastdate { get; set; }
        public string Lastuser { get; set; }
    }
}
