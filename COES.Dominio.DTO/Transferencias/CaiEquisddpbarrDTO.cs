using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_EQUISDDPBARR
    /// </summary>
    public class CaiEquisddpbarrDTO : EntityBase
    {
        public int Casddbcodi { get; set; } 
        public int Barrcodi { get; set; } 
        public string Casddbbarra { get; set; }
        public DateTime? Casddbfecvigencia { get; set; } 
        public string Casddbusucreacion { get; set; } 
        public DateTime Casddbfeccreacion { get; set; } 
        public string Casddbusumodificacion { get; set; } 
        public DateTime Casddbfecmodificacion { get; set; }
        public decimal Casddbfactbarrpmpo { get; set; }
        //Atributos de consulta
        public string Barrnombre { get; set; }
    }
}
