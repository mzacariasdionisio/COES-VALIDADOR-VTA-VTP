using System;
using System.Collections.Generic;
using COES.Base.Core;
namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DPO_FERIADO
    /// </summary>
    [Serializable]
    public class DpoFeriadosDTO : EntityBase
    {
        public int Dpofercodi { get; set; }
        public int Dpoferanio { get; set; }
        public DateTime Dpoferfecha { get; set; }
        public string Dpoferdescripcion { get; set; }
        public string Dpoferspl { get; set; }
        public string Dpofersco { get; set; }
        public string Dpoferusucreacion { get; set; }
        public DateTime Dpoferfeccreacion { get; set; }

        // Adicionales
        public string Strfecha { get; set; }
    }
}
