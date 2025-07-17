using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_GPSAISLADO
    /// </summary>
    public class EveGpsaisladoDTO : EntityBase
    {
        public int Gpsaiscodi { get; set; }
        public int Iccodi { get; set; }
        public int Gpscodi { get; set; }
        public int Gpsaisprincipal { get; set; }
        public string Gpsaisusucreacion { get; set; }
        public DateTime? Gpsaisfeccreacion { get; set; }

        public string Gpsnombre { get; set; }
        public string Gpsosinerg { get; set; }
    }
}
