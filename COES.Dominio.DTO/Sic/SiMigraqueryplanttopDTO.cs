using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAQUERYPLANTTOP
    /// </summary>
    public class SiMigraqueryplanttopDTO : EntityBase
    {
        public int Miptopcodi { get; set; }
        public int Miqplacodi { get; set; }
        public int Tmopercodi { get; set; }
        public int Miptopactivo { get; set; }
        public string Miptopusucreacion { get; set; }
        public DateTime Miptopfeccreacion { get; set; }
    }
}
