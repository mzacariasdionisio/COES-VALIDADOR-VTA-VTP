using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_AMPLIACION_PLAZO
    /// </summary>
    public partial class SmaAmpliacionPlazoDTO : EntityBase
    {
        public int Smaapcodi { get; set; }
        public DateTime Smaapaniomes { get; set; }
        public DateTime Smaapplazodefecto { get; set; }
        public DateTime Smaapnuevoplazo { get; set; }
        public string Smaapestado { get; set; }
        public string Smaapusucreacion { get; set; }
        public DateTime Smaapfeccreacion { get; set; }
        public string Smaapusumodificacion { get; set; }
        public DateTime? Smaapfecmodificacion { get; set; }
    }

    public partial class SmaAmpliacionPlazoDTO
    {
        public bool EsEditable { get; set; }
        public string SmaapaniomesTexto { get; set; }
        public string SmaapaniomesDesc { get; set; }
        public string SmaapplazodefectoDesc { get; set; }
        public string SmaapnuevoplazoDesc { get; set; }
        public string SmaapestadoDesc { get; set; }

        public string SmaapfeccreacionDesc { get; set; }
        public string SmaapfecmodificacionDesc { get; set; }
    }
}
