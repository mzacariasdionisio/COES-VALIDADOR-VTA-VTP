using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_ENVIO_CENTRAL
    /// </summary>
    public partial class CbEnvioCentralDTO : EntityBase
    {
        public int Cbcentcodi { get; set; }
        public string Cbcentestado { get; set; }
        public int Cbvercodi { get; set; }
        public int Equicodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Grupocodi { get; set; }
    }

    public partial class CbEnvioCentralDTO 
    {
        public string Equinomb { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Fenergnomb { get; set; }
        public int? OrdenLista { get; set; }
        public int UsadoEnExtranet { get; set; }

        public int TipoC3 { get; set; }
        public DateTime? Cbenvfechaperiodo { get; set; }

        public List<CbDatosDTO> ListaDato { get; set; } = new List<CbDatosDTO>();
    }
}
