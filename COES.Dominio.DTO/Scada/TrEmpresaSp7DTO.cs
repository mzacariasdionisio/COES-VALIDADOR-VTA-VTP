using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_EMPRESA_SP7
    /// </summary>
    public class TrEmpresaSp7DTO : EntityBase
    {
        public int Emprcodi { get; set; }
        public string Emprenomb { get; set; }
        public string Emprabrev { get; set; }
        public int? Emprsiid { get; set; }
        public string Emprusucreacion { get; set; }
        public string Empriccppri { get; set; }
        public string Empriccpsec { get; set; }
        public string Empriccpconect { get; set; }
        public DateTime? Empriccplastdate { get; set; }
        public string Emprinvertrealq { get; set; }
        public string Emprinvertstateq { get; set; }
        public string Emprconec { get; set; }
        public int? Linkcodi { get; set; }
        public string Emprstateqgmt { get; set; }
        public string Emprrealqgmt { get; set; }
        public string Emprreenviar { get; set; }
        public int? Emprlatencia { get; set; }
        public DateTime? Emprfeccreacion { get; set; }
        public string Emprusumodificacion { get; set; }
        public DateTime? Emprfecmodificacion { get; set; }
    }
}
