using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_PORCTAPORTE
    /// </summary>
    public class CaiPorctaporteDTO : EntityBase
    {
        public int Caipacodi { get; set; } 
        public int Caiajcodi { get; set; } 
        public int Emprcodi { get; set; } 
        public decimal Caipaimpaporte { get; set; } 
        public decimal Caipapctaporte { get; set; } 
        public string Caipausucreacion { get; set; } 
        public DateTime Caipafeccreacion { get; set; }

        //Atributos empleados en consulta
        public string Emprnomb { get; set; }
    }
}
