using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_EQUICANAL
    /// </summary>
    public partial class EqEquicanalDTO : EntityBase
    {
        public int Ecancodi { get; set; }
        public int Areacode { get; set; }
        public int Canalcodi { get; set; }
        public int Equicodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public decimal? Ecanfactor { get; set; }
        public string Ecanestado { get; set; }

        public string Ecanusucreacion { get; set; }
        public DateTime? Ecanfecmodificacion { get; set; }
        public string Ecanusumodificacion { get; set; }
        public DateTime? Ecanfeccreacion { get; set; }
    }

    public partial class EqEquicanalDTO
    {
        public int Equipadre { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
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

        public string EquipoDesc { get; set; }
        public string CanalScadaDesc { get; set; }
        public string EcanestadoDesc { get; set; }
        public string EcanfeccreacionDesc { get; set; }
        public string EcanfecmodificacionDesc { get; set; }
        public string EcanfactorDesc { get; set; }

        public string Areaabrev { get; set; }

        public string Areadesc { get; set; }
    }
}
