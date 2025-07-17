using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_MANTTO
    /// </summary>
    public partial class IndManttoDTO : EntityBase, ICloneable
    {
        public int Indmancodi { get; set; }
        public int Emprcodi { get; set; }
        public int? Manttocodi { get; set; }
        public int Equicodi { get; set; }
        public int Evenclasecodi { get; set; }
        public DateTime Indmanfecini { get; set; }
        public DateTime Indmanfecfin { get; set; }
        public string Indmandescripcion { get; set; }

        public string Indmantipoindisp { get; set; }
        public decimal? Indmanpr { get; set; }
        public string Indmanasocproc { get; set; }
        public string Indmanusarencalculo { get; set; }
        public string Indmanestado { get; set; }
        public string Indmantipoaccion { get; set; }
        public string Indmancomentario { get; set; }

        public int? Tipoevencodi { get; set; }
        public string Indmanindispo { get; set; }
        public string Indmaninterrup { get; set; }
        
        public string Indmanusucreacion { get; set; }
        public DateTime? Indmanfeccreacion { get; set; }
        public string Indmanusumodificacion { get; set; }
        public DateTime? Indmanfecmodificacion { get; set; }

        public int? Indmancodiold { get; set; }

        public string Indmanomitir7d { get; set; }
        public string Indmanomitirexcesopr { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class IndManttoDTO : EntityBase
    {
        public int? Equipadre { get; set; }
        public string Central { get; set; }
        public string Equiabrev { get; set; }
        public int Grupocodi { get; set; }
        public string Emprnomb { get; set; }
        public string Emprabrev { get; set; }
        public string Evenclasedesc { get; set; }
        public int Areacodi { get; set; }
        public string Areanomb { get; set; }
        public string Areadesc { get; set; }
        public int Famcodi { get; set; }
        public string Famabrev { get; set; }
        public string Famnomb { get; set; }
        public string Evenclaseabrev { get; set; }
        public decimal? Equitension { get; set; }
        public string Tipoevenabrev { get; set; }
        public string Tipoevendesc { get; set; }
        public string Tipoemprdesc { get; set; }
        public int Tipoemprcodi { get; set; }
        public string Osigrupocodi { get; set; }
        public string Grupotipocogen { get; set; }

        public string Evenusucreacion { get; set; }
        public DateTime? Evefeccreacion { get; set; }

        public string IndmanestadoDesc { get; set; }
        public string TipoDelete { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }
}
