using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_FAC_PER_MED
    /// </summary>
    public class RerFacPerMedDTO : EntityBase
    {
        public int Rerfpmcodi { get; set; }
        public string Rerfpmnombrearchivo { get; set; }
        public DateTime Rerfpmdesde { get; set; }
        public DateTime Rerfpmhasta { get; set; }
        public string Rerfpmusucreacion { get; set; }
        public DateTime Rerfpmfeccreacion { get; set; }
    }
}