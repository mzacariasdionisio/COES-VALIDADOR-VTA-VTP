using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAQUERYPLANTPARAM
    /// </summary>
    public class SiMigraqueryplantparamDTO : EntityBase
    {
        public int Miplprcodi { get; set; }
        public int Miqplacodi { get; set; }
        public int Migparcodi { get; set; }

        public int Miplpractivo { get; set; }
        public string Miplprusucreacion { get; set; }
        public DateTime Miplprfeccreacion { get; set; }
    }
}
