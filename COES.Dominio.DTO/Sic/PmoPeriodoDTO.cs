using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    public partial class PmoPeriodoDTO : EntityBase
    {
        public int PmPeriCodi { get; set; }
        public string PmPeriNombre { get; set; }
        public int? PmPeriAniOMes { get; set; }
        public string PmPeriTipo { get; set; }
        public string PmPeriEstado { get; set; }
        public string PmPeriUsuCreacion { get; set; }
        public DateTime? PmPeriFecCreacion { get; set; }
        public string PmPeriUsuModificacion { get; set; }
        public DateTime? PmPeriFecModificacion { get; set; }

        public DateTime PmPeriFecIniMantAnual { get; set; }
        public DateTime PmPeriFecFinMantAnual { get; set; }
        public DateTime PmPeriFecIniMantMensual { get; set; }
        public DateTime PmPeriFecFinMantMensual { get; set; }

        public DateTime PmPeriFechaPeriodo { get; set; }
        public DateTime Pmperifecini { get; set; }
        public DateTime Pmperifecinimes { get; set; }
        public int Pmperinumsem { get; set; }
    }

    public partial class PmoPeriodoDTO
    {
        public string Semanadesc { get; set; }
        public string SPmPeriFecIniMantAnual { get; set; }
        public string SPmPeriFecFinMantAnual { get; set; }
        public string SPmPeriFecIniMantMensual { get; set; }
        public string SPmPeriFecFinMantMensual { get; set; }
    }
}
