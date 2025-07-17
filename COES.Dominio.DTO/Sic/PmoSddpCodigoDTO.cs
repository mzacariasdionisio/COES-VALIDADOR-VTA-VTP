using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_SDDP_CODIGO
    /// </summary>
    public partial class PmoSddpCodigoDTO : EntityBase
    {
        public int Sddpcodi { get; set; }
        public int Tsddpcodi { get; set; }
        public int Sddpnum { get; set; }
        public string Sddpnomb { get; set; }
        public string Sddpestado { get; set; }
        public string Sddpdesc { get; set; }
        public string Sddpcomentario { get; set; }
        public int? Ptomedicodi { get; set; }
        public int? Tptomedicodi { get; set; }

        public string Sddpusucreacion { get; set; }
        public DateTime? Sddpfeccreacion { get; set; }
        public string Sddpusumodificacion { get; set; }
        public DateTime? Sddpfecmodificacion { get; set; }
    }

    public partial class PmoSddpCodigoDTO
    {
        public int? Grupocodi { get; set; }
        public int? Equicodi { get; set; }
        public int Equipadre { get; set; }
        public string Central { get; set; }

        public decimal Pe { get; set; }

        public int? Emprcodi { get; set; }

        public string Ptomedidesc { get; set; }
        public string Ptomedielenomb { get; set; }
        public string Tsddpnomb { get; set; }
        public string PtoPmpo { get; set; }
        public string Planta { get; set; }

        public string CodinombSDDP { get; set; }
        public string DescripcionSDDP { get; set; }

        public bool TieneAlertaValidacion { get; set; }
        public string MensajeValidacion { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public List<int> ListaEquicodi { get; set; }
        public List<string> ListaEquiabrev { get; set; }
        public List<int> ListaGrupocodi { get; set; }
        public int Grupocodimodo { get; set; }

        public List<int> ListaEquicodiModo { get; set; } = new List<int>();
        public int EquicodiTVCicloComb { get; set; }
        public bool TieneCicloComb { get; set; }
        public bool TieneModoCicloSimple { get; set; }
        public bool TieneModoEspecial { get; set; }
        public bool TieneCentralCicloComb { get; set; }
    }
}
