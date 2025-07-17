using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_COMPENSACION
    /// </summary>
    public class StCompensacionDTO : EntityBase
    {
        public int Stcompcodi { get; set; } 
        public int Sistrncodi { get; set; } 
        public string Stcompcodelemento { get; set; } 
        public string Stcompnomelemento { get; set; } 
        public decimal Stcompimpcompensacion { get; set; } 
        public int Barrcodi1 { get; set; } 
        public int Barrcodi2 { get; set; } 
        public string Sstcmpusucreacion { get; set; } 
        public DateTime Sstcmpfeccreacion { get; set; } 
        public string Sstcmpusumodificacion { get; set; } 
        public DateTime Sstcmpfecmodificacion { get; set; }
        //ATRIBUTOS DE CONSULTA
        public string Barrnombre { get; set; }
        public string Barrnombre1 { get; set; }
        public string Barrnombre2 { get; set; }
    }
}
