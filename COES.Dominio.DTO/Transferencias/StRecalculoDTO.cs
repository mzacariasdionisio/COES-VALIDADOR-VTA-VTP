using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_RECALCULO
    /// </summary>
    public class StRecalculoDTO : EntityBase
    {
        public int Strecacodi { get; set; } 
        public int Stpercodi { get; set; } 
        public int Sstversion { get; set; } 
        public string Strecanombre { get; set; } 
        public string Strecainforme { get; set; } 
        public decimal Strecafacajuste { get; set; } 
        public string Strecacomentario { get; set; } 
        public string Strecausucreacion { get; set; } 
        public DateTime Strecafeccreacion { get; set; } 
        public string Strecausumodificacion { get; set; } 
        public DateTime Strecafecmodificacion { get; set; }

        //ATRIBUTOS PARA MOSTRAR EN CONSULTAS O VISTAS
        public string Stpernombre { get; set; }
    }
}
