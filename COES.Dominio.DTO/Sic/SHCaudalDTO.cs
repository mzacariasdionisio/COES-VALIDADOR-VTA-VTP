using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SH_CAUDAL
    /// </summary>
    public partial class SHCaudalDTO : EntityBase
    {
        public int IdCaudal { get; set; }
        public int EmpresaCodi { get; set; }
        public int TipoSerieCodi { get; set; }
        public int TPtoMediCodi { get; set; }
        public int PtoMediCodi { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }

    }

}
