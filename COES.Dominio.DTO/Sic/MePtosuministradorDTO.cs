using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_PTOSUMINISTRADOR
    /// </summary>
    public class MePtosuministradorDTO : EntityBase
    {
        public int Ptosucodi { get; set; } 
        public int Ptomedicodi { get; set; } 
        public int Emprcodi { get; set; } 
        public DateTime Ptosufechainicio { get; set; } 
        public DateTime? Ptosufechafin { get; set; } 
        public string Ptosuusucreacion { get; set; } 
        public DateTime Ptosufeccreacion { get; set; } 
        public string Ptosuusumodificacion { get; set; } 
        public DateTime? Ptosufecmodificacion { get; set; }
        //- pr16.JDEL - Inicio 21/10/2016: Cambio para atender el requerimiento.
        public string Ptomedidesc { get; set; }
        public string Emprrazsocial { get; set; }
        public int VigEmprcodi { get; set; }
        public int PerPtosucodi { get; set; }
        public int PerEmprcodi { get; set; }
        public int SelEmprcodi { get; set; }
        //- JDEL Fin

    }
}
