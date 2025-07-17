using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_VERSIONDSRNS
    /// </summary>
    public class VcrVersiondsrnsDTO : EntityBase
    {
        public int Vcrdsrcodi { get; set; } 
        public int Pericodi { get; set; } 
        public string Vcrdsrnombre { get; set; } 
        public string Vcrdsrestado { get; set; } 
        public string Vcrdsrusucreacion { get; set; } 
        public DateTime Vcrdsrfeccreacion { get; set; } 
        public string Vcrdsrusumodificacion { get; set; } 
        public DateTime Vcrdsrfecmodificacion { get; set; }

        //agregados para consulta
        public string Perinombre { get; set; }
    }
}
