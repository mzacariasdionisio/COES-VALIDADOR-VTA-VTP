using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_TOPOLOGIA
    /// </summary>
    public partial class MpTopologiaDTO : EntityBase
    {
        public int Mtopcodi { get; set; } 
        public string Mtopnomb { get; set; }
        public int? Mtopversion { get; set; }
        public int? Mtopestado { get; set; }        
        public DateTime? Mtopfecha { get; set; } 
        public DateTime? Mtopfechafutura { get; set; } 
        public string Mtopresolucion { get; set; } 
        public int? Mtopoficial { get; set; } 
        public string Mtopusuregistro { get; set; } 
        public DateTime? Mtopfeccreacion { get; set; } 
        public string Mtopusumodificacion { get; set; } 
        public DateTime? Mtopfecmodificacion { get; set; }
        public int Mtopcodipadre { get; set; }
    }

    public partial class MpTopologiaDTO
    {
        public string MtopfeccreacionDesc { get; set; }
        public string MtopfecmodificacionDesc { get; set; }
        public string MtopresolucionDesc { get; set; }
        public string MtopoficialDesc { get; set; }

        #region Modificacion Central SDDP
        public int Orden { get; set; }
        public MpRecursoDTO RecursoSddp { get; set; }
        //public List<MpRecursoDTO> ListaRecursoSddp { get; set; }
        public string FechaFuturaDesc { get; set; }
        #endregion
    }
}
