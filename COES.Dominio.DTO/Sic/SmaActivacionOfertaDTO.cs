using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_ACTIVACION_OFERTA
    /// </summary>
    public partial class SmaActivacionOfertaDTO : EntityBase
    {
        public int Smapaccodi { get; set; } 
        public DateTime? Smapacfecha { get; set; } 
        public string Smapacestado { get; set; } 
        public string Smapacusucreacion { get; set; } 
        public DateTime? Smapacfeccreacion { get; set; } 
        public string Smapacusumodificacion { get; set; } 
        public DateTime? Smapacfecmodificacion { get; set; } 
    }

    public partial class SmaActivacionOfertaDTO 
    {
        public List<SmaActivacionDataDTO> ListaDatosXActivacion { get; set; }
        public List<SmaActivacionMotivoDTO> ListaMotivosXActivacion { get; set; }
        public string SmapacfechaDesc { get; set; }
        public string SmapacfeccreacionDesc { get; set; }
    }
}
