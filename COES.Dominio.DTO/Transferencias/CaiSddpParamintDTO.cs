using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_SDDP_PARAMINT
    /// </summary>
    /// 
    public class CaiSddpParamintDTO : EntityBase
    {
        public int Sddppicodi { get; set; }
        public int Caiajcodi { get; set; }
        public DateTime Sddppiintervalo { get; set; }
        public int Sddppilaboral { get; set; }
        public int Sddppibloque { get; set; }
        public string Sddppiusucreacion { get; set; }
        public DateTime Sddppifeccreacion { get; set; }
    
    }
}
