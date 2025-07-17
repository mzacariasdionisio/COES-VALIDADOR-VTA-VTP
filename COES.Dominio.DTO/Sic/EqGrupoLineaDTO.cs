using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_GRUPO_LINEA
    /// </summary>
    public partial class EqGrupoLineaDTO : EntityBase
    {
        public int Grulincodi { get; set; } 
        public string Grulinnombre { get; set; } 
        public decimal? Grulinvallintrans { get; set; } 
        public decimal? Grulinporlimtrans { get; set; } 
        public string Grulinestado { get; set; }
        public string Nombrencp { get; set; }
        public int? Codincp { get; set; }
        #region CMgCP_PR07
        public string Grulintipo { get; set; }
        #endregion
        public int? Equicodi { get; set; }
    }

    public partial class EqGrupoLineaDTO
    {
        public string Areanomb { get; set; }
        public string Areaoperativa { get; set; }
        public string Equipo { get; set; }
    }
}
