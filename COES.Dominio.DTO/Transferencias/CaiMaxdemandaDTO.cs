using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_MAXDEMANDA
    /// </summary>
    public class CaiMaxdemandaDTO : EntityBase
    {
        public int Caimdecodi { get; set; } 
        public int Caiajcodi { get; set; } 
        public int Caimdeaniomes { get; set; } 
        public DateTime? Caimdefechor { get; set; } 
        public string Caimdetipoinfo { get; set; }
        public decimal Caimdemaxdemanda { get; set; }
        public string Caimdeusucreacion { get; set; } 
        public DateTime Caimdefeccreacion { get; set; } 
        public string Caimdeusumodificacion { get; set; } 
        public DateTime Caimdefecmodificacion { get; set; }

        //variable de consulta
        public string NombreMes { get; set; }
        public string NombreTipo { get; set; }
    }
}
