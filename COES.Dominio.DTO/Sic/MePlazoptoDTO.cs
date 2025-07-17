using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_PLAZOPTO
    /// </summary>
    [Serializable]
    public class MePlazoptoDTO : EntityBase
    {
        public int Plzptocodi { get; set; }
        public int Plzptodiafinplazo { get; set; }
        public int Plzptominfinplazo { get; set; }
        public DateTime? Plzptofechavigencia { get; set; }
        public DateTime? Plzptofecharegistro { get; set; }
        public int Plzptominfila { get; set; }
        public int Formatcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Emprcodi { get; set; }
        public string Plzptousuariomodificacion { get; set; }
        public DateTime? Plzptofechamodificacion { get; set; }
        public string Plzptousuarioregistro { get; set; }

    }
}
