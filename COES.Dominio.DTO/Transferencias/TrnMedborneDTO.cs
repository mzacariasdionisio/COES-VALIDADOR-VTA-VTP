using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_MEDBORNE
    /// </summary>
    public class TrnMedborneDTO : EntityBase
    {
        public int Trnmebcodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Trnmebversion { get; set; } 
        public int Emprcodi { get; set; } 
        public int Equicodi { get; set; } 
        public DateTime Trnmebfecha { get; set; } 
        public string Trnmebptomed { get; set; } 
        public decimal Trnmebenergia { get; set; } 
        public string Trnmebusucreacion { get; set; } 
        public DateTime Trnmebfeccreacion { get; set; } 
    }
}
