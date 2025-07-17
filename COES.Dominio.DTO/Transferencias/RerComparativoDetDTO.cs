using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_COMPARATIVO_DET
    /// </summary>
    public class RerComparativoDetDTO : EntityBase
    {
        public int Rercdtcodi { get; set; }
        public int Rerccbcodi { get; set; }
        public int Rerevacodi { get; set; }
        public int Reresecodi { get; set; }
        public int Rereeucodi { get; set; }
        public DateTime Rercdtfecha { get; set; }
        public string Rercdthora { get; set; }
        public decimal? Rercdtmedfpm { get; set; }
        public decimal? Rercdtenesolicitada { get; set; }
        public decimal? Rercdteneestimada { get; set; }
        public decimal? Rercdtpordesviacion { get; set; }
        public string Rercdtflag { get; set; }
        public string Rercdtusucreacion { get; set; }
        public DateTime Rercdtfeccreacion { get; set; }
        public string Rercdtusumodificacion { get; set; }
        public DateTime Rercdtfecmodificacion { get; set; }

        //Atributos de consulta
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
    }
}