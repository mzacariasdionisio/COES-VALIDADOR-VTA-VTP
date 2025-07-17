using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_INGTRANSMISION
    /// </summary>
    public class CaiIngtransmisionDTO : EntityBase
    {
        public int Caitrcodi { get; set; } 
        public int Caiajcodi { get; set; } 
        public string Caitrcalidadinfo { get; set; } 
        public int Emprcodi { get; set; } 
        public int Ptomedicodi { get; set; }
        public int Caitrmes { get; set; }
        public decimal Caitringreso { get; set; }
        public string Caitrtipoinfo { get; set; }
        public string Caitrusucreacion { get; set; } 
        public DateTime Caitrfeccreacion { get; set; } 
    }
}
