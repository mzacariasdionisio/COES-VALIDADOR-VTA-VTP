using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_DESPACHO_PRODGEN
    /// </summary>
    public partial class MeDespachoProdgenDTO : EntityBase
    {
        public int Dpgencodi { get; set; }
        public DateTime Dpgenfecha { get; set; }
        public int Dpgentipo { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int Grupocodi { get; set; }
        public int Tgenercodi { get; set; }
        public int Fenergcodi { get; set; }
        public int? Ctgdetcodi { get; set; }
        public decimal? Dpgenvalor { get; set; }
        public string Dpgenintegrante { get; set; }
        public string Dpgenrer { get; set; }
    }

    public partial class MeDespachoProdgenDTO
    {
        public string FechaDesc { get; set; }
        public string Fenergnomb { get; set; }
        public string Tgenernomb { get; set; }
        public string Central { get; set; }
        public string Gruponomb { get; set; }
        public string Emprnomb { get; set; }

        public decimal EnergiaH { get; set; }
        public decimal EnergiaT { get; set; }
        public decimal EnergiaE { get; set; }
        public decimal EnergiaS { get; set; }
        public decimal PotenciaMD { get; set; }

        public int Codigo { get; set; }
        public int? Ptomedicodi { get; set; }
    }
}
