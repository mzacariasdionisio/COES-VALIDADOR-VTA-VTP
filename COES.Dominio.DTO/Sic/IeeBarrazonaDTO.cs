using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IEE_BARRAZONA
    /// </summary>
    public class IeeBarrazonaDTO : EntityBase
    {
        public int Barrzcodi { get; set; }
        public int Barrzarea { get; set; }
        public int? Barrcodi { get; set; }
        public int? Mrepcodi { get; set; }
        public string Barrzdesc { get; set; }

        public string Barrnombre { get; set; }
        public string Barrzusumodificacion { get; set; }
        public DateTime? Barrzfecmodificacion { get; set; }
    }
}
