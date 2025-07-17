using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_AREA_SUBCAUSAEVENTO
    /// </summary>
    public class EveAreaSubcausaeventoDTO : EntityBase
    {
        public int Arscaucodi { get; set; }
        public string Arscauusucreacion { get; set; }
        public DateTime? Arscaufeccreacion { get; set; }
        public string Arscauusumodificacion { get; set; }
        public DateTime? Arscaufecmodificacion { get; set; }
        public int? Arscauactivo { get; set; }
        public int Subcausacodi { get; set; }
        public int Areacode { get; set; }
    }
}
