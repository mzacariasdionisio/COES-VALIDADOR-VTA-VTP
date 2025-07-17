using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MMM_INDICADOR
    /// </summary>
    public class MmmIndicadorDTO : EntityBase
    {
        public int Immecodi { get; set; }
        public string Immeabrev { get; set; }
        public string Immenombre { get; set; }
        public string Immedescripcion { get; set; }
        public string Immecodigo { get; set; }
    }
}
