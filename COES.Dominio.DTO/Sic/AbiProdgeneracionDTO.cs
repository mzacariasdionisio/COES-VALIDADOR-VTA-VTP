using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ABI_PRODGENERACION
    /// </summary>
    public partial class AbiProdgeneracionDTO : EntityBase
    {
        public int Pgencodi { get; set; }
        public DateTime Pgenfecha { get; set; }
        public int Emprcodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Tgenercodi { get; set; }
        public int Equicodi { get; set; }
        public int Equipadre { get; set; }
        public int Grupocodi { get; set; }
        public decimal Pgenvalor { get; set; }
        public string Pgentipogenerrer { get; set; }
        public string Pgenintegrante { get; set; }

        public DateTime Pgenfecmodificacion { get; set; }
        public string Pgenusumodificacion { get; set; }
    }

    public partial class AbiProdgeneracionDTO 
    {
        public string FechaDesc { get; set; }
        public string Fenergnomb { get; set; }
        public string Tgenernomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Emprnomb { get; set; }

        public decimal EnergiaH { get; set; }
        public decimal EnergiaT { get; set; }
        public decimal EnergiaE { get; set; }
        public decimal EnergiaS { get; set; }
        public decimal PotenciaMD { get; set; }

        public int Codigo { get; set; }
    }
}
