using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public partial class GmmIncumplimientoDTO : EntityBase
    {
        public int INCUCODI { get; set; }
        public int INCUANIO { get; set; }
        public string INCUMES { get; set; }
        public string INCUACEPTADO { get; set; }
        public string INCUSUBSANADO { get; set; }
        public int EMPGCODI { get; set; }
        public int TIPOEMPRCODI { get; set; }
        public int EMPRCODI { get; set; }
        public decimal INCUMONTO { get; set; }
        public string INCUUSUCREACION { get; set; }
        public DateTime? INCUFECCREACION { get; set; }
        public string INCUUSUMODIFICACION { get; set; }
        public DateTime? INCUFECMODIFICACION { get; set; }
    }
    public partial class GmmIncumplimientoDTO : EntityBase
    {
        // Resultados de búsqueda de imcumplimientos
        public string IncumAnioMes { get; set; }
        public string IncumEmprAfectada{ get; set; }
        public string IncumEmprDeudora { get; set; }
        public decimal IncumMonto { get; set; }
        public string IncumInforme { get; set; }
    }

    public partial class GmmIncumplimientoDTO : EntityBase
    {
        // valores obtenidos para edición de un incumplimiento GetByIdEdit
        public int IncucodiEdit { get; set; }
        public int IncuanioEdit { get; set; }
        public string IncumesEdit { get; set; }
        public string IncumplidoraEdit { get; set; }
        public string AfectadaEdit { get; set; }
        public decimal IncumMontoEdit { get; set; }
        public int EmpgcodiEdit { get; set; }
        public int EmprcodiEdit { get; set; }
        public int TipoEmpresa { get; set; }
    }
    
}
