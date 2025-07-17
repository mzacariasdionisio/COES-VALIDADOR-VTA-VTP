using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_PARAMETRO_REVISION
    /// </summary>
    public class RerParametroRevisionDTO : EntityBase
    {
        public int Rerprecodi { get; set; }
        public int Rerpprcodi { get; set; }
        public string Perinombre { get; set; }
        public string Recanombre { get; set; }
        public string Rerpretipo { get; set; }
        public string Rerpreusucreacion { get; set; }
        public DateTime Rerprefeccreacion { get; set; }
        public int Pericodi { get; set; }
        public int Recacodi { get; set; }
    }
}

