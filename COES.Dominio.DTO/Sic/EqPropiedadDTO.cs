using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_PROPIEDAD
    /// </summary>
    public partial class EqPropiedadDTO : EntityBase
    {
        public string Propnombficha { get; set; }
        public int? Proptipolong1 { get; set; }
        public int? Proptipolong2 { get; set; }
        public int Propactivo { get; set; }
        public string Propusucreacion { get; set; }
        public DateTime? Propfeccreacion { get; set; }
        public string Propusumodificacion { get; set; }
        public DateTime? Propfecmodificacion { get; set; }
        public string Propfichaoficial { get; set; }
        public string Propocultocomentario { get; set; }
        public int Propcodi { get; set; }
        public int Famcodi { get; set; }
        public string Propabrev { get; set; }
        public string Propnomb { get; set; }
        public string Propunidad { get; set; }
        public int? Orden { get; set; }
        public string Proptipo { get; set; }
        public string Propdefinicion { get; set; }
        public int? Propcodipadre { get; set; }
        public decimal? Propliminf { get; set; }
        public decimal? Proplimsup { get; set; }
        public int? Propflagcolor { get; set; }
    }

    public partial class EqPropiedadDTO
    {
        public string EstiloEstado { get; set; }
        public bool ExisteCambio { get; set; }
        public string PropfichaoficialDesc { get; set; }
        public string PropfeccreacionDesc { get; set; }
        public string PropfecmodificacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string StrPropliminf { get; set; }
        public string StrProplimsup { get; set; }
        public string StrProptipolong1 { get; set; }
        public string StrProptipolong2 { get; set; }

        public int NroItem { get; set; } // Indice para la importación de propiedades
        public string Observaciones { get; set; }
        public bool ChkMensaje { get; set; }

        //CAMPOS DE OTRAS TABLAS
        public string NombreFamilia { get; set; }
        public string NombrePadre { get; set; }
        public string Valor { get; set; }

        public string Propformula {  get; set; }
    }
}