using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_PROCESO
    /// </summary>
    public class SiProcesoDTO : EntityBase
    {
        public int Prcscodi { get; set; }
        public string Prcsnomb { get; set; }
        public string Prcsestado { get; set; }
        public string Prcsmetodo { get; set; }
        public string Prscfrecuencia { get; set; }
        public int? Prschorainicio { get; set; }
        public int? Prscminutoinicio { get; set; }
        public int? Modcodi { get; set; }
        public int? Prscbloque { get; set; }

        public string PathFile { get; set; }
        public int NumDia { get; set; }
    }
}
