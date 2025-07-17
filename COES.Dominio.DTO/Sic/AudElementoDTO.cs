using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_ELEMENTO
    /// </summary>
    public class AudElementoDTO : EntityBase
    {
        public int Elemcodi { get; set; }
        public int? Tabcdcoditipoelemento { get; set; }
        public int? Proccodi { get; set; }
        public string Elemcodigo { get; set; }
        public string Elemdescripcion { get; set; }
        public string Elemactivo { get; set; }
        public string Elemhistorico { get; set; }
        public string Elemusucreacion { get; set; }
        public DateTime? Elemfeccreacion { get; set; }
        public string Elemusumodificacion { get; set; }
        public DateTime? Elemfecmodificacion { get; set; }

        public string AreaNom { get; set; }
        public string TipoElemento { get; set; }
        public string Procdescripcion { get; set; }
        public string Proccodigo { get; set; }

        public int Areacodi { get; set; }
        public int Existeprogaudielemento { get; set; }
        
        public int Audppcodi { get; set; }
        public AudElementoDTO()
        {
            Areacodi = 0;
            Elemactivo = string.Empty;
            Proccodigo = string.Empty;
        }
    }
}
