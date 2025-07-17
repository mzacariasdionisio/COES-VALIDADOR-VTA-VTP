using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_DESCCUADRO
    /// </summary>
    public class IndDesccuadroDTO : EntityBase
    {
        public int Descucodi { get; set; } 
        public string Descutitulocuadro { get; set; } 
        public string Descuusumodificacion { get; set; } 
        public DateTime Descufecmodificacion { get; set; } 
    }
}
