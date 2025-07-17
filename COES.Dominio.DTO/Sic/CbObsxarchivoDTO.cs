using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_OBSXARCHIVO
    /// </summary>
    public partial class CbObsxarchivoDTO : EntityBase
    {
        public int Cbobsacodi { get; set; }
        public int Cbobscodi { get; set; }
        public string Cbobsanombreenvio { get; set; }
        public string Cbobsanombrefisico { get; set; }
        public int Cbobsaorden { get; set; }
        public int Cbobsaestado { get; set; }
    }

    public partial class CbObsxarchivoDTO
    {
        public int Equicodi { get; set; }
        public int Ccombcodi { get; set; }

        public int Archienvioorden { get; set; }
        public bool EsNuevo { get; set; }
    }
}
