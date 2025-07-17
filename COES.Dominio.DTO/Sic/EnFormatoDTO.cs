using COES.Base.Core;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_FORMATO
    /// </summary>
    public class EnFormatoDTO : EntityBase
    {
        public int Formatocodi { get; set; }
        public string Formatodesc { get; set; }
        public int? Formatotipoarchivo { get; set; }
        public int? Formatopadre { get; set; }
        public string Formatoprefijo { get; set; }
        public decimal? Formatonumero { get; set; }
        public int? Formatoestado { get; set; }
        public List<EnFormatoDTO> ListaFormato { get; set; }
    }
}
