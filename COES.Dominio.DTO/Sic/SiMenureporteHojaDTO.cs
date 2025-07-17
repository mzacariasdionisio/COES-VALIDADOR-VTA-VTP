using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MENUREPORTE_HOJA
    /// </summary>
    public partial class SiMenureporteHojaDTO : EntityBase
    {
        public int Mrephcodi { get; set; }
        public string Mrephnombre { get; set; }
        public int Mrephestado { get; set; }
        public int Mrephvisible { get; set; }
        public int Mrephorden { get; set; }
        public int Mrepcodi { get; set; }
    }

    public partial class SiMenureporteHojaDTO
    {
        public string Repdescripcion { get; set; }
    }
}
