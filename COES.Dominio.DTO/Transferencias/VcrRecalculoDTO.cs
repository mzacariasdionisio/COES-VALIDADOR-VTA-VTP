using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_RECALCULO
    /// </summary>
    public class VcrRecalculoDTO : EntityBase
    {
        public int Vcrecacodi { get; set; } 
        public int Pericodi { get; set; } 
        public string Vcrecanombre { get; set; } 
        public int Vcrecaversion { get; set; } 
        public decimal Vcrecakcalidad { get; set; } 
        public decimal Vcrecapaosinergmin { get; set; } 
        public int Recacodi { get; set; } 
        public int? Vcrdsrcodi { get; set; } 
        public int? Vcrinccodi { get; set; } 
        public string Vcrecacomentario { get; set; } 
        public string Vcrecaestado { get; set; } 
        public int Vcrecacodidestino { get; set; } 
        public string Vcrecausucreacion { get; set; } 
        public DateTime Vcrecafeccreacion { get; set; } 
        public string Vcrecausumodificacion { get; set; } 
        public DateTime Vcrecafecmodificacion { get; set; }
        // 202012 - Nuevos atributos
        public decimal Vcrecaresaprimsig { get; set; }
        public decimal Vcrecacostoprns { get; set; }
        public decimal Vcrecafactcumpl { get; set; }

        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Perinombre { get; set; }
        public string Recanombre { get; set; }
        public string Perinombredestino { get; set; }
        public string Vcrecanombredestino { get; set; }
        //agregados para consulta
        public string Vcrdsrnombre { get; set; }
        public string Vcrincnombre { get; set; }
    }
}
