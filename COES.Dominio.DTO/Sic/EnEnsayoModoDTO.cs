using COES.Base.Core;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_ENSAYOMODO
    /// </summary>
    public class EnEnsayomodoDTO : EntityBase
    {
        public int? Ensayocodi { get; set; }
        public int? Grupocodi { get; set; }
        public int Enmodocodi { get; set; }

        public List<EnEnsayomodequiDTO> ListaUnidades { get; set; }
    }
}
