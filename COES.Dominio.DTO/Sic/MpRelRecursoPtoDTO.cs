using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_REL_RECURSO_PTO
    /// </summary>
    public partial class MpRelRecursoPtoDTO : EntityBase
    {
        public int Mtopcodi { get; set; }
        public int Mrecurcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Lectcodi { get; set; }
        public string Mrptohorizonte { get; set; }        
        public int? Tptomedicodi { get; set; }
        public decimal? Mrptofactor { get; set; }
        public int? Mrptoformato { get; set; }
        public int? Equicodi { get; set; }
        public decimal? Mrptovolumen { get; set; }
    }

    public partial class MpRelRecursoPtoDTO
    {
        public string Equinomb { get; set; }
        public string Emprnomb { get; set; }
    }
}
