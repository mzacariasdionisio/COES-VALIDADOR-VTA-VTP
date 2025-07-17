using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_ENTIDAD
    /// </summary>
    public partial class PfrEntidadDTO : EntityBase
    {
        public int Pfrentcodi { get; set; }
        public string Pfrentnomb { get; set; }
        public string Pfrentid { get; set; }
        public int? Grupocodi { get; set; }
        public int? Pfrentcodibarragams { get; set; }
        public int? Barrcodi { get; set; }
        public int? Equipadre { get; set; }
        public int Pfrcatcodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Pfrentcodibarragams2 { get; set; }
        public int? Pfrentficticio { get; set; }
        public string Pfrentunidadnomb { get; set; }
        public int Pfrentestado { get; set; }

        public DateTime? Pfrentfeccreacion { get; set; }
        public DateTime? Pfrentfecmodificacion { get; set; }
        public string Pfrentusucreacion { get; set; }
        public string Pfrentusumodificacion { get; set; }
    }

    public partial class PfrEntidadDTO
    {
        public List<PfrEntidadDatDTO> ListEntidadDat { get; set; } = new List<PfrEntidadDatDTO>();
        public string Usuarioultimamodif { get; set; }
        public string Fechaultimamodif { get; set; }
        public string Pfrentestadodesc { get; set; }

        public DateTime? VigenciaIni { get; set; }
        public DateTime? VigenciaFin { get; set; }
        public string VigenciaIniDesc { get; set; } = "";
        public string VigenciaFinDesc { get; set; } = "";

        public decimal? Tension { get; set; }
        public decimal? Vmax { get; set; }
        public decimal? Vmin { get; set; }
        public decimal? Compreactiva { get; set; }
        public string Idbarra1 { get; set; }
        public string Idbarra2 { get; set; }   
        public string Idbarra1desc { get; set; }
        public string Idbarra2desc { get; set; }
        public decimal? Resistencia { get; set; }
        public decimal? Reactancia { get; set; }
        public decimal? Conductancia { get; set; }
        public decimal? Admitancia { get; set; }
        public decimal? Potenciamax { get; set; }
        public decimal? Potenciamin { get; set; }
        public decimal? Tap1 { get; set; }
        public decimal? Tap2 { get; set; }
        public decimal? Qmax { get; set; }
        public decimal? Qmin { get; set; }

        public string Pfrentid2 { get; set; }
        public string Barrnombre { get; set; }
        public decimal? TensionBarra1 { get; set; }
        public string Lineasdesc { get; set; }
        public decimal? Costov { get; set; }
        public int EscenariocodiParaGams { get; set; }

        public int? Ref { get; set; }

        public string Penalidad { get; set; }
        public string Descripcion { get; set; }

        public string Equinomb { get; set; }
        public int? Numunidad { get; set; }
        public decimal? Pmax { get; set; }
        public decimal? Pmin { get; set; }
    }
}
