using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_ESTUDIOTERCEROINV_EPO
    /// </summary>
    public class EpoEstudioTerceroInvEpoDTO : EntityBase
    {
        public int Invepocodi { get; set; }
        public int Estepocodi { get; set; }
        public int Estepoemprcodi { get; set; }
        public DateTime Lastdate { get; set; }
        public string Lastuser { get; set; }
    }
}
