using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_TIPOAREA
    /// </summary>
    public class EqTipoareaDTO : EntityBase
    {
        public int Tareacodi { get; set; } 
        public string Tareaabrev { get; set; } 
        public string Tareanomb { get; set; } 
    }

}
