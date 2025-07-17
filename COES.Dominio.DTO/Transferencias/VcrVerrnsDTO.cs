using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_VERRNS
    /// </summary>
    public class VcrVerrnsDTO : EntityBase
    {
        public int Vcvrnscodi { get; set; } 
        public int Vcrdsrcodi { get; set; } 
        public DateTime? Vcvrnsfecha { get; set; } 
        public DateTime? Vcvrnshorinicio { get; set; } 
        public DateTime? Vcvrnshorfinal { get; set; } 
        public int Emprcodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public decimal Vcvrnsrns { get; set; } 
        public string Vcvrnsusucreacion { get; set; } 
        public DateTime Vcvrnsfeccreacion { get; set; }
        public int Vcvrnstipocarga { get; set; }

        //atributos adicionales
        public string EmprNombre { get; set; }
    }
}
