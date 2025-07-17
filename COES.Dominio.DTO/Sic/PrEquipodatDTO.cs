using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_EQUIPODAT
    /// </summary>
    public class PrEquipodatDTO : EntityBase
    {
        public int Equicodi { get; set; } 
        public int Grupocodi { get; set; } 
        public int Concepcodi { get; set; } 
        public string Formuladat { get; set; } 
        public DateTime Fechadat { get; set; } 
        public int Deleted { get; set; } 
    }
}

