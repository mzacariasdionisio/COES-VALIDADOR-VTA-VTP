using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ABI_FACTORPLANTA
    /// </summary>
    public class AbiFactorplantaDTO : EntityBase
    {
        public int Fpcodi { get; set; }
        public DateTime Fpfechaperiodo { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public int Tgenercodi { get; set; }
        public decimal Fpvalor { get; set; }
        public decimal Fpvalormes { get; set; }
        public decimal Fppe { get; set; }
        public decimal Fpenergia { get; set; }
        public string Fptipogenerrer { get; set; }
        public string Fpintegrante { get; set; }
        public DateTime? Fpfecmodificacion { get; set; }
        public string Fpusumodificacion { get; set; }

        public int Grupocodi { get; set; }
        public string Central { get; set; }
        public decimal FpenergiaMes { get; set; }
        public int Equicodiactual { get; set; }
    }
}
