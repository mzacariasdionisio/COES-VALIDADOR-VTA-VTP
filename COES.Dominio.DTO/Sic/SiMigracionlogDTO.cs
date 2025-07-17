using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRACIONLOG
    /// </summary>
    public class SiMigracionlogDTO : EntityBase
    {
        public int Logmigcodi { get; set; }
        public int Migracodi { get; set; }
        public int Mqxtopcodi { get; set; }

        public string Logmigoperacion { get; set; }
        public string Logmicodigo { get; set; }
    }
}
