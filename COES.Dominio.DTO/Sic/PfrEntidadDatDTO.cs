using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_ENTIDAD_DAT
    /// </summary>
    public partial class PfrEntidadDatDTO : EntityBase, ICloneable
    {
        public int Pfrentcodi { get; set; }
        public int Pfrcnpcodi { get; set; }
        public DateTime Prfdatfechavig { get; set; }
        public int Pfrdatdeleted { get; set; }
        public string Pfrdatvalor { get; set; }
        public DateTime? Pfrdatfeccreacion { get; set; }
        public string Pfrdatusucreacion { get; set; }
        public DateTime? Pfrdatfecmodificacion { get; set; }
        public string Pfrdatusumodificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class PfrEntidadDatDTO
    {
        public int Pfrcatcodi { get; set; }
        public string Pfrcnpnomb { get; set; }
        public string Pfrcatnomb { get; set; }
        public string Prfdatfechavigdesc { get; set; }
        public List<PfrEntidadDatDTO> ListEntidadDat { get; set; }

        public string UsuarioUltimaModif { get; set; }
        public string FechaUltimaModif { get; set; }
        public string EstadoDesc { get; set; }

        public int Pfrdatdeleted2 { get; set; }
    }
}
