using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_EMPRESAXCENTRAL
    /// </summary>
    public class IndEmpresaxcentralDTO : EntityBase
    {
        public int Empctrcodi { get; set; }
        public string Empctrestado { get; set; }
        public int? Emprcodi { get; set; }
        public string Empctrusumodificacion { get; set; }
        public DateTime? Empctrfecmodificacion { get; set; }
        public int Gasoductocodi { get; set; }
    }
}
