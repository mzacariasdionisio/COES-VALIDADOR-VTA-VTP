using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_RELACION_OD_MO
    /// </summary>
    public class SmaRelacionOdMoDTO : EntityBase
    {
        public string Odmousucreacion { get; set; }
        public DateTime? Odmofeccreacion { get; set; }
        public string Odmousumodificacion { get; set; }
        public DateTime? Odmofecmodificacion { get; set; }
        public int? Ofdecodi { get; set; }
        public int Odmocodi { get; set; }
        public int? Grupocodi { get; set; }
        public decimal Odmopotmaxofer { get; set; }
        public decimal Odmobndcalificada { get; set; }
        public decimal Odmobnddisponible { get; set; }
        public string Odmoestado{ get; set; }
    }
}
