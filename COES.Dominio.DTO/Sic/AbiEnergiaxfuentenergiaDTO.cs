using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ABI_ENERGIAXFUENTENERGIA
    /// </summary>
    public class AbiEnergiaxfuentenergiaDTO : EntityBase
    {
        public int Mdfecodi { get; set; }
        public int Fenergcodi { get; set; }
        public DateTime Mdfefecha { get; set; }
        public decimal Mdfevalor { get; set; }
        public DateTime Mdfemes { get; set; }
        public string Mdfeusumodificacion { get; set; }
        public DateTime? Mdfefecmodificacion { get; set; }
    }
}
