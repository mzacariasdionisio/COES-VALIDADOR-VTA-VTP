using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_REQUERIMIENTO_INFORM
    /// </summary>
    public class AudRequerimientoInformDTO : EntityBase
    {
        public int Reqicodi { get; set; }
        public int Progaecodi { get; set; }
        public int Tabcdcodiestado { get; set; }
        public int Percodiresponsable { get; set; }
        public int? Archcodirequerimiento { get; set; }
        public DateTime? Reqiplazo { get; set; }
        public string Reqirequerimiento { get; set; }
        public DateTime? Reqifechasolicitada { get; set; }
        public DateTime? Reqifechapresentada { get; set; }
        public string Reqiactivo { get; set; }
        public string Reqihistorico { get; set; }
        public string Reqiusuregistro { get; set; }
        public DateTime? Reqifecregistro { get; set; }
        public string Reqiusumodificacion { get; set; }
        public DateTime? Reqifecmodificacion { get; set; }

        public int Audicodi { get; set; }
        public string Elemcodigo { get; set; }
        public string Elemdescripcion { get; set; }
        public string Estadodescripcion { get; set; }
        public int Tienearchivo { get; set; }

        public int Usercode { get; set; }
    }
}
