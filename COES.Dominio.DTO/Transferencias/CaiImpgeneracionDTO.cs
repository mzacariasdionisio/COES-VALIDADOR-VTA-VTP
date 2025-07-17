using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_IMPGENERACION
    /// </summary>
    public class CaiImpgeneracionDTO : EntityBase
    {
        public int Caimpgcodi { get; set; } 
        public int Caiajcodi { get; set; }
        public string Caimpgfuentedat { get; set; }
        public int Emprcodi { get; set; } 
        public int Ptomedicodi { get; set; } 
        public int Caimpgmes { get; set; } 
        public decimal Caimpgtotenergia { get; set; } 
        public decimal Caimpgimpenergia { get; set; } 
        public decimal Caimpgtotpotencia { get; set; } 
        public decimal Caimpgimppotencia { get; set; } 
        public string Caimpgusucreacion { get; set; } 
        public DateTime Caimpgfeccreacion { get; set; }
    }
}