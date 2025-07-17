using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_AREACONCEPUSER
    /// </summary>
    public class PrAreaConcepUserDTO : EntityBase
    {
        public int Aconuscodi { get; set; }
        public int Usercode { get; set; }
        public int Arconcodi { get; set; }
        public int Aconusactivo { get; set; }
        public string Aconususucreacion { get; set; }
        public DateTime? Aconusfeccreacion { get; set; }
        public string Aconususumodificacion { get; set; }
        public DateTime? Aconusfecmodificacion { get; set; }

        public int Concepcodi { get; set; }
    }
}
