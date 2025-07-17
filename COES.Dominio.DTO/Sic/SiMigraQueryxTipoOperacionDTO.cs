using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAQUERYXTIPOOPERACION
    /// </summary>
    public class SiMigraqueryxtipooperacionDTO : EntityBase
    {
        public int Mqxtopcodi { get; set; }
        public int Miqubacodi { get; set; }
        public int Tmopercodi { get; set; }
        public int Mqxtoporden { get; set; }
        public int Mqxtopactivo { get; set; }
        public string Mqxtopusucreacion { get; set; }
        public DateTime Mqxtopfeccreacion { get; set; }
    }
}
