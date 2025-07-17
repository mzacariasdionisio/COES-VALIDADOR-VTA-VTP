using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRALOGDBA
    /// </summary>
    public class SiMigralogdbaDTO : EntityBase
    {
        public int Migracodi { get; set; } 
        public int Migdbacodi { get; set; } 
        public string Migdbaquery { get; set; }

        public string Migdbalogquery { get; set; }

        public string Migdbausucreacion { get; set; }
        public DateTime? Migdbafeccreacion { get; set; }
        public int? Mqxtopcodi { get; set; }

        public int? NroRegistros { get; set; }
        public string Miqubanomtabla { get; set; }
        public string FechaDesc { get; set; }
        public string HoraDesc { get; set; }
    }
}
