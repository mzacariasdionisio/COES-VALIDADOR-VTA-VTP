using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_CONGESGDESPACHO
    /// </summary>
    public class EveCongesgdespachoDTO : EntityBase
    {
        public int? Grupocodi { get; set; }
        public int? Iccodi { get; set; }
        public int Congdecodi { get; set; }
        public DateTime? Congdefechaini { get; set; }
        public DateTime? Congdefechafin { get; set; }
        public string CongdefechainiDesc { get; set; }
        public string CongdefechafinDesc { get; set; }
        public string Congdeusucreacion { get; set; }
        public DateTime? Congdefeccreacion { get; set; }
        public string Congdeusumodificacion { get; set; }
        public DateTime? Congdefecmodificacion { get; set; }
        public int? Congdeestado { get; set; }

        public DateTime? Ichorini { get; set; }
        public DateTime? Ichorfin { get; set; }
        public int? Emprcodi { get; set; }
        public string Equiabrev { get; set; }
        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public int? Equicodi { get; set; }
        public string Icdescrip2 { get; set; }
        public string Central { get; set; }
        public int? Grupopadre { get; set; }
        public string Gruponomb { get; set; }
        public int Catecodi { get; set; }
    }
}
