using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_QN_ENVIO
    /// </summary>
    public partial class PmoQnEnvioDTO : EntityBase
    {
        public int Qnbenvcodi { get; set; }
        public int? Qnbenvanho { get; set; }
        public string Qnbenvnomb { get; set; }
        public int? Qnbenvestado { get; set; }
        public int? Qnbenvversion { get; set; }
        public DateTime? Qnbenvfechaperiodo { get; set; }
        public string Qnbenvusucreacion { get; set; }
        public DateTime? Qnbenvfeccreacion { get; set; }
        public string Qnbenvusumodificacion { get; set; }
        public DateTime? Qnbenvfecmodificacion { get; set; }
        public int Qnlectcodi { get; set; }
        public int Qncfgecodi { get; set; }
        public int Qnbenvidentificador { get; set; }
        public int Qnbenvdeleted { get; set; }
        public int Qnbenvbase { get; set; }
    }

    public partial class PmoQnEnvioDTO
    {
        public string Qnlectnomb { get; set; }
        public string Resolucion { get; set; }
        public string FechaperiodoDesc { get; set; }
        public string FeccreacionDesc { get; set; }
        public string FecmodificacionDesc { get; set; }
        public string EstadoDesc { get; set; }
        public string IdentificadorDesc { get; set; }
    }
}
