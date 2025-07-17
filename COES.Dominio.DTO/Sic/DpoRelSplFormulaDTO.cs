using System;
using System.Collections.Generic;
using COES.Base.Core;
namespace COES.Dominio.DTO.Sic
{
    public class DpoRelSplFormulaDTO
    {
        public int Splfrmcodi { get; set; }
        public int Dposplcodi { get; set; }
        public int Barsplcodi { get; set; }
        public int? Ptomedicodifveg { get; set; }
        public int? Ptomedicodiful { get; set; }
        public int? Splfrmarea { get; set; }

        //Adicionales
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public string Dposplnombre { get; set; }
        public string Nombvegetativa { get; set; }
        public string Nombindustrial { get; set; }
        public List<int?> VegNoDisponible { get; set; }
        public List<int?> UliNoDisponible { get; set; }
        public string Splareanombre { get; set; }
    }
}
