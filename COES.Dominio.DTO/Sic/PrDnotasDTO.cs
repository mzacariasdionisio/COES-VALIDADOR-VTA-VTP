using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_DNOTAS
    /// </summary>
    public class PrDnotasDTO : EntityBase
    {
        public DateTime Fecha { get; set; } 
        public int Lectcodi { get; set; } 
        public int Notaitem { get; set; } 
        public string Notadesc { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
