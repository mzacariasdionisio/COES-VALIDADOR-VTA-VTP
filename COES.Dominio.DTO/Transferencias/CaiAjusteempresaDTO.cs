using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_AJUSTEEMPRESA
    /// </summary>
    public class CaiAjusteempresaDTO : EntityBase
    {
        public int Caiajecodi { get; set; } 
        public int Caiajcodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int Ptomedicodi { get; set; }
        public string Caiajetipoinfo { get; set; }
        public DateTime? Caiajereteneejeini { get; set; } 
        public DateTime? Caiajereteneejefin { get; set; } 
        public DateTime? Caiajeretenepryaini { get; set; } 
        public DateTime? Caiajeretenepryafin { get; set; } 
        public DateTime? Caiajereteneprybini { get; set; } 
        public DateTime? Caiajereteneprybfin { get; set; } 
        public string Caiajeusucreacion { get; set; } 
        public DateTime Caiajefeccreacion { get; set; }

        //para listar
        public DateTime FechaPeriodo { get; set; }
        public string Periodo { get; set; }
        public string Emprnomb { get; set; }
        public string Ptomedidesc { get; set; }
        public string Ptomedielenomb { get; set; }
        public string Tipoemprdesc { get; set; }
    }
}
