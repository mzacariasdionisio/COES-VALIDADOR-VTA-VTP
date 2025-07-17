using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_PTOMEDCANAL
    /// </summary>
    public partial class MePtomedcanalDTO : EntityBase
    {
        public int Pcancodi { get; set; }
        public int Canalcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public string Pcanestado { get; set; }
        public string Pcanusucreacion { get; set; }
        public DateTime? Pcanfeccreacion { get; set; }
        public string Pcanusumodificacion { get; set; }
        public DateTime? Pcanfecmodificacion { get; set; }
        public decimal? Pcanfactor { get; set; }
    }

    public partial class MePtomedcanalDTO
    {
        public int Equipadre { get; set; }
        public string Central { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Famcodi { get; set; }
        public string Famnomb { get; set; }
        public string Famabrev { get; set; }
        public string Tipoinfoabrev { get; set; }

        public string Canalnomb { get; set; }
        public string Canaliccp { get; set; }
        public string Canalunidad { get; set; }
        public string CanalPointType { get; set; }
        public string Canalabrev { get; set; }
        public int Zonacodi { get; set; }
        public string Zonanomb { get; set; }
        public string Zonaabrev { get; set; }
        public int TrEmprcodi { get; set; }
        public string TrEmprnomb { get; set; }
        public string TrEmprabrev { get; set; }

        public string PcanestadoDesc { get; set; }
        public string PcanfeccreacionDesc { get; set; }
        public string PcanfecmodificacionDesc { get; set; }
        public string PuntoPR5 { get; set; }
        public string PuntoCanalScada { get; set; }

        #region MigracionSGOCOES-GrupoB
        public string Origlectnombre { get; set; }
        #endregion

        public string Ptomedielenomb { get; set; }
        public string PcanfactorDesc { get; set; }
    }
}
