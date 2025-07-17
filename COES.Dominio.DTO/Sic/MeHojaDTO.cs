using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_HOJA
    /// </summary>
    public class MeHojaDTO : EntityBase
    {
        public int Hojacodi { get; set; }
        public int Hojaorden { get; set; } 
        public int? Formatcodi { get; set; } 
        public int? Cabcodi { get; set; } 
        public int? Lectcodi { get; set; } 
        public string Hojanombre { get; set; }
        public int Hojapadre { get; set; }

        public MeCabeceraDTO Cabecera { get; set; }
        public List<MeHojaptomedDTO> ListaPtos { get; set; }
    }
}
