using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAQUERYPARAMETRO
    /// </summary>
    public partial class SiMigraqueryparametroDTO : EntityBase
    {
        public int Mgqparcodi { get; set; }
        public int Miqubacodi { get; set; }
        public int Migparcodi { get; set; }
        public string Mgqparvalor { get; set; }
        public int Mgqparactivo { get; set; }
        public string Mgqparusucreacion { get; set; }
        public DateTime Mgqparfeccreacion { get; set; }

    }

    public partial class SiMigraqueryparametroDTO
    {

    }
}
