using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_RELACION_RPT
    /// </summary>
    public class IndRelacionRptDTO : EntityBase
    {
        public int Irelrpcodi { get; set; }
        public int Irelrpidprinc { get; set; }
        public int Irelpridsec { get; set; }
    }
}
