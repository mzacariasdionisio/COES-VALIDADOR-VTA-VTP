using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_DSTELE_BARRA
    /// </summary>
    public class StDsteleBarraDTO : EntityBase
    {
        public int Dstelecodi { get; set; } 
        public int Barrcodi { get; set; } 
        public decimal Delbarrpu { get; set; } 
        public decimal Delbarxpu { get; set; } 
    }
}
