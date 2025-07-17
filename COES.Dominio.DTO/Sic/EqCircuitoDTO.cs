using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_CIRCUITO
    /// </summary>
    public class EqCircuitoDTO : EntityBase
    {
        public int Circodi { get; set; }
        public int Equicodi { get; set; }
        public string Circnomb { get; set; }
        public int Circestado { get; set; }
        public DateTime? Circfecmodificacion { get; set; }
        public string Circusumodificacion { get; set; }
        public DateTime? Circfeccreacion { get; set; }
        public string Circusucreacion { get; set; }

        public string Famabrev { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Areanomb { get; set; }

        public string CircestadoDesc { get; set; }
        public string CirfeccreacionDesc { get; set; }
        public string CirfecmodificacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }
}
