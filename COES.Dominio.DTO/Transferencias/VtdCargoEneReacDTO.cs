using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTD_CARGOSENEREAC
    /// </summary>
    ///Se cambio el nombre de VtdCargosEneReactDTO -> VtdCargoEneReacDTO
    public class VtdCargoEneReacDTO : EntityBase
    {
        public int Caercodi { get; set; }
        public int Emprcodi { get; set; }
        public DateTime Caerfecha { get; set; }
        public decimal Caermonto { get; set; }
        public int Caerdeleted { get; set; }
        public string Caersucreacion { get; set; }
        public DateTime? Caerfeccreacion { get; set; }
        public string Caerusumodificacion { get; set; }
        public DateTime? Caerfecmodificacion { get; set; }

        public string Emprnomb { get; set; } // cambio
    }
}

