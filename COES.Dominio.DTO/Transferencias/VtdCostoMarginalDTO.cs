using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ADDIN
    /// </summary>
    public class VtdCostoMarginalDTO : EntityBase
    {
        public int Vmgncodi { get; set; }
        public int Emprcodi { get; set; }    
        public string Emprnomb { get; set; }    
        public decimal Vmgndemtotal { get; set; }    
        public decimal Vmgncompensacion { get; set; }    
        public decimal Vmgnclasificacion { get; set; }    
        public DateTime Vmgnfecha { get; set; }    
        public string Lastuser { get; set; }    
        public DateTime Lastdate { get; set; }  
    }
}
