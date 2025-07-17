using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_QN_CAMBIOENVIO
    /// </summary>
    public class PmoQnCambioenvioDTO : EntityBase
    {
        public int Qncmbecodi { get; set; }
        public int Qnbenvcodi { get; set; }
        public int Sddpcodi { get; set; }
        public DateTime? Qncmbefecha { get; set; }
        public string Qncmbedatos { get; set; }
        public string Qncmbecolvar { get; set; }
        public string Qncmbeusucreacion { get; set; }
        public DateTime? Qncmbefeccreacion { get; set; }
        public string Qncmbeorigen { get; set; }
    }
}
