using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_SISTEMATRANS
    /// </summary>
    public class StSistematransDTO : EntityBase
    {
        public int Sistrncodi { get; set; }
        public int Strecacodi { get; set; }
        public int Emprcodi { get; set; }
        public string Sistrnnombre { get; set; }
        public string Sistrnsucreacion { get; set; }
        public DateTime Sistrnfeccreacion { get; set; }
        public string Sistrnsumodificacion { get; set; }
        public DateTime Sistrnfecmodificacion { get; set; }
        // variables para consultas
        public string Emprnomb { get; set; }
        public string Stcompcodelemento { get; set; }
        public string Stcompnomelemento { get; set; }
        public decimal Stcompimpcompensacion { get; set; }
        public string Barrnombre1 { get; set; }
        public string Barrnombre2 { get; set; }
    }
}
