using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_HISEMPENTIDAD
    /// </summary>
    public class SiHisempentidadDTO : EntityBase
    {
        public int Migracodi { get; set; }
        public int Hempencodi { get; set; }
        public string Hempentitulo { get; set; }
        public string Hempentablename { get; set; }
        public string Hempencampoid { get; set; }
        public string Hempencampodesc { get; set; }
        public string Hempencampodesc2 { get; set; }
        public string Hempencampoestado { get; set; }
    }
}
