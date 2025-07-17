using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_NUMERAL_GENFORZADA
    /// </summary>
    public class SpoNumeralGenforzadaDTO : EntityBase
    {
        public int Genforcodi { get; set; }
        public int Verncodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Grupocodi { get; set; }
        public int Hopcausacodi { get; set; }
        public DateTime? Genforhorini { get; set; }
        public DateTime? Genforhorfin { get; set; }
        public decimal Genformw { get; set; }
        public string Genforusucreacion { get; set; }
        public DateTime Genforfeccreacion { get; set; }

        public int OrdenZona { get; set; }
        public string Ptomedidesc { get; set; }
        public string Tipo { get; set; }
        public int Grupopadre { get; set; }
        public string Gruponomb { get; set; }
        public int Equipadre { get; set; }
        public string Genforhorini2 { get; set; }
        public string Genforhorfin2 { get; set; }
        public decimal Numhoras { get; set; }
        public decimal? PotenciaPromedio { get; set; }
        public decimal? Energiaforzada { get; set; }
        public decimal? CostoForzado { get; set; }
    }
}
