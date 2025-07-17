using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_EVENTOEQUIPO
    /// </summary>
    [Serializable]
    public class EveEventoEquipoDTO : EntityBase
    {
        public int Eeqcodi { get; set; }
        public int Equicodi { get; set; }
        public int Subcausacodi { get; set; }
        public DateTime Eeqfechaini { get; set; }
        public int Eeqestado { get; set; }
        public string Eeqdescripcion { get; set; }
        public DateTime Eeqfechafin { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Subcausadesc { get; set; }
        public int Famcodi { get; set; }
        public string Famnomb { get; set; }
        public int? Areacodi { get; set; }
        public string Areanomb { get; set; }

        public string EeqfechainiDesc { get; set; }
        public string Tgenernomb { get; set; }
        public int Ctgcodi { get; set; }
        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }
        public string Ctgdetnomb { get; set; }
        public decimal Equitension { get; set; }
        public string Equinombpadre { get; set; }
        public decimal Potenciaefectiva { get; set; }
    }
}
