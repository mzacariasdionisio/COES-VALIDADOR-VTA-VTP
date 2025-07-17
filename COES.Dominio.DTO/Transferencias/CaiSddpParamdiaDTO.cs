using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_SDDP_PARAMDIA
    /// </summary>
    /// 
    public class CaiSddpParamdiaDTO : EntityBase
    {
        public int Sddppdcodi { get; set; }
        public int Caiajcodi { get; set; }
        public DateTime ? Sddppddia { get; set; }
        public int Sddppdlaboral { get; set; } 
        public string Sddppdusucreacion { get; set; }
        public DateTime Sddppdfeccreacion { get; set; }
       
    }
}
