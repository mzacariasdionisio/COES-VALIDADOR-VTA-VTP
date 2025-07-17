using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_VERSION
    /// </summary>
    public partial class SiVersionDTO : EntityBase
    {
        public int Verscodi { get; set; }
        public string Versnombre { get; set; }
        public int Verscorrelativo { get; set; }
        public byte[] Versdatos { get; set; }
        public int Versnroreporte { get; set; }
        public int Mprojcodi { get; set; }
        public int Tmrepcodi { get; set; }
        public string Versmotivo { get; set; }

        public string Versusucreacion { get; set; }
        public DateTime? Versfeccreacion { get; set; }
        public DateTime Versfechaperiodo { get; set; }
        public DateTime? Versfechaversion { get; set; }
    }

    public partial class SiVersionDTO
    {
        public string VersfeccreacionDesc { get; set; }
        public string VersfechaperiodoDesc { get; set; }
        public string VerscodiDesc { get; set; }
        public string VerscorrelativoDesc { get; set; }
        public string VersionTituloWeb { get; set; }

        public bool TienePdf { get; set; }
    }

}
