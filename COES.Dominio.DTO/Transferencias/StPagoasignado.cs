using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_PAGOASIGNADO
    /// </summary>
    public class StPagoasignadoDTO : EntityBase
    {
        public int Pagasgcodi { get; set; }
        public int Strecacodi { get; set; }
        public int Stcntgcodi { get; set; }
        public int Stcompcodi { get; set; }
        public decimal Pagasgcmggl { get; set; }
        public decimal Pagasgcmgglp { get; set; }
        public decimal Pagasgcmgglfinal { get; set; }
        public string Pagasgusucreacion { get; set; }
        public DateTime Pagasgfeccreacion { get; set; } 
        //variables para reportes
        public string Equinomb { get; set; }
        public string Stcompcodelemento { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Emprruc { get; set; }
        public int Sistrncodi { get; set; }
        public string Sistrnnombre { get; set; }
    }
}
