using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_VERSION
    /// </summary>
    public class PfVersionDTO : EntityBase
    {
        public int Pfverscodi { get; set; }
        public int Pfrecacodi { get; set; }
        public int Pfrecucodi { get; set; }
        public int? Irptcodi { get; set; }
        public int Pfversnumero { get; set; }
        public string Pfversestado { get; set; }
        public string Pfversusucreacion { get; set; }
        public DateTime? Pfversfeccreacion { get; set; }

        public string Descripcion { get; set; }
    }
}
