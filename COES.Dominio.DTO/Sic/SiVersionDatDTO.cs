using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_VERSION_DAT
    /// </summary>
    public partial class SiVersionDatDTO : EntityBase
    {
        public int Verdatcodi { get; set; }
        public int Vercnpcodi { get; set; }
        public int Versdtcodi { get; set; }
        public string Verdatvalor { get; set; }
        public string Verdatvalor2 { get; set; }
        public int Verdatid { get; set; }
    }
    public partial class SiVersionDatDTO
    {
        public List<SiVersionDatdetDTO> ListaDetalle { get; set; }

        public int CodigoEntero { get; set; }
    }
}
