using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_SUBSCRIPCION
    /// </summary>
    public class WbSubscripcionDTO : EntityBase
    {
        public int Subscripcodi { get; set; } 
        public string Subscripnombres { get; set; } 
        public string Subscripapellidos { get; set; } 
        public string Subscripemail { get; set; } 
        public string Subscriptelefono { get; set; } 
        public string Subscripempresa { get; set; } 
        public string Subscripestado { get; set; } 
        public DateTime? Subscripfecha { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Publicname { get; set; }
        public List<WbSubscripcionitemDTO> ListaDetalle { get; set; }
        public string Detalle { get; set; }
    }
}
