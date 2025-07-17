using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_AMPLAZOENVIO
    /// </summary>
    public class SiAmplazoenvioDTO : EntityBase
    {
        public int Amplzcodi { get; set; }
        public int Fdatcodi { get; set; }
        public int Emprcodi { get; set; }
        public DateTime? Amplzfecha { get; set; }
        public DateTime? Amplzfechaperiodo { get; set; }
        public string Amplzusucreacion { get; set; }
        public DateTime? Amplzfeccreacion { get; set; }
        public string Amplzusumodificacion { get; set; }
        public DateTime? Amplzfecmodificacion { get; set; }

        public string Emprnomb { get; set; }
        public string Fdatnombre { get; set; }
        public string AmplzfechaDesc { get; set; }
        public string AmplzfechaperiodoDesc { get; set; }
        public string AmplzfeccreacionDesc { get; set; }
    }
}
