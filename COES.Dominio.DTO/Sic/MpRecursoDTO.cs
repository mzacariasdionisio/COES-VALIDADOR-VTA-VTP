using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_RECURSO
    /// </summary>
    public partial class MpRecursoDTO : EntityBase
    {
        public int Mtopcodi { get; set; } 
        public int Mrecurcodi { get; set; } 
        public int Mcatcodi { get; set; } 
        public string Mrecurnomb { get; set; } 
        public string Mrecurtablasicoes { get; set; } 
        public int? Mrecurcodisicoes { get; set; } 
        public int? Mrecurlogico { get; set; } 
        public int? Mrecurestado { get; set; } 
        public int? Mrecurpadre { get; set; } 
        public int? Mrecurorigen { get; set; } 
        public int? Mrecurorigen2 { get; set; } 
        public int? Mrecurorigen3 { get; set; }
        public int Mrecurorden { get; set; }
        public string Mrecurusumodificacion { get; set; } 
        public DateTime? Mrecurfecmodificacion { get; set; } 
    }

    public partial class MpRecursoDTO
    {
        public List<MpRelacionDTO> ListaRelacionesTV { get; set; }
        public List<MpRelRecursoEqDTO> ListaCentralesHidro { get; set; }
        public List<MpRelRecursoPtoDTO> ListaEmbalses { get; set; }
        public MpRelRecursoSddpDTO RelacionEstacionHidrologica { get; set; }
        public string MrecurfecmodificacionDesc { get; set; }

        #region Modificacion Central SDDP
        public int NumeroSDDP { get; set; }
        public string NombreSDDP { get; set; }  
        public List<MpProprecursoDTO> ListaPropRecursoSddp { get; set; }
        public decimal? Potencia { get; set; }
        public decimal? CoefProduccion { get; set; }
        public decimal? CaudalMinTur { get; set; }
        public decimal? CaudalMaxTur { get; set; }
        public decimal? DefluenciaTotMin { get; set; }
        public decimal? ICP { get; set; }
        public decimal? IH { get; set; }
        public int IndicadorEA { get; set; }
        public decimal VolumenMax { get; set; }
        public string RecursonombreDesc { get; set; }
        #endregion
        public int? SDDPEstacionAsoc { get; set; }
        public int? SDDPVierte { get; set; }
        public int? SDDPTurbina { get; set; }
    }
}
