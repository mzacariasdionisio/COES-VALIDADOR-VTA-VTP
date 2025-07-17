using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAPARAMETRO
    /// </summary>
    public partial class SiMigraParametroDTO : EntityBase
    {
        public int Migparcodi { get; set; }
        public string Migparnomb { get; set; }
        public int Migpartipo { get; set; }
        public string Migpardesc { get; set; }
        public string Migparusucreacion { get; set; }
        public DateTime Migparfeccreacion { get; set; }
    }

    public partial class SiMigraParametroDTO
    {
        public string MigpartipoDesc { get; set; }
        public string MigparfeccreacionDesc { get; set; }

        public int Miqubacodi { get; set; }
        public string ValorParametro { get; set; }
        public string Miqubanomtabla { get; set; }

        public int LogMigcodigo { get; set; }
    }
}
