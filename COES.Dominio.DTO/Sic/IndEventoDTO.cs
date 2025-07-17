using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_EVENTO
    /// </summary>
    public partial class IndEventoDTO : EntityBase
    {
        public int Ieventcodi { get; set; }
        public int Evencodi { get; set; }
        public string Ieventtipoindisp { get; set; }
        public decimal? Ieventpr { get; set; }
        public string Ieventcomentario { get; set; }
        public string Ieventestado { get; set; }
        public string Ieventusucreacion { get; set; }
        public DateTime? Ieventfeccreacion { get; set; }
        public string Ieventusumodificacion { get; set; }
        public DateTime? Ieventfecmodificacion { get; set; }
    }

    public partial class IndEventoDTO
    {
        public string Ieventusarencalculo { get; set; }

        public int Evenclasecodi { get; set; }
        public DateTime Evenini { get; set; }
        public DateTime Evenfin { get; set; }
        public string Evenasunto { get; set; }
        public int Equicodi { get; set; }
        public int Equipadre { get; set; }
        public string Central { get; set; }
        public string Equiabrev { get; set; }
        public int Grupocodi { get; set; }
        public int Emprcodi { get; set; }
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
        public string Grupotipocogen { get; set; }

        public string IeventestadoDesc { get; set; }

        public int FuenteDatos { get; set; }
        public string FuenteDatosDesc { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }
}
