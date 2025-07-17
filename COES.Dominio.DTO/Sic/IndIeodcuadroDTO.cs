using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_IEODCUADRO
    /// </summary>
    public partial class IndIeodcuadroDTO : EntityBase
    {
        public int Iiccocodi { get; set; }
        public int Iccodi { get; set; }
        public string Iiccotipoindisp { get; set; }
        public decimal? Iiccopr { get; set; }
        public string Iiccocomentario { get; set; }
        public string Iiccoestado { get; set; }
        public string Iiccousucreacion { get; set; }
        public DateTime? Iiccofeccreacion { get; set; }
        public string Iiccousumodificacion { get; set; }
        public DateTime? Iiccofecmodificacion { get; set; }
    }

    public partial class IndIeodcuadroDTO
    {
        public string Iiccousarencalculo { get; set; }

        public int Evenclasecodi { get; set; }
        public DateTime Ichorini { get; set; }
        public DateTime Ichorfin { get; set; }
        public string Icdescrip1 { get; set; }
        public string Icdescrip2 { get; set; }
        public string Icdescrip3 { get; set; }
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

        public string IiccoestadoDesc { get; set; }
        public int FuenteDatos { get; set; }
        public string FuenteDatosDesc { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }
}
