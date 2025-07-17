using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_ENERGIAUNIDAD_DET
    /// </summary>
    public class RerEnergiaUnidadDetDTO : EntityBase
    {
        public int Rereudcodi { get; set; }
        public int Rereucodi { get; set; }
        public string Rereudenergiaunidad { get; set; }
    }
}
