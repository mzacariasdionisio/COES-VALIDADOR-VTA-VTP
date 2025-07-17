using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_ENSAYOMODEQUI
    /// </summary>
    public class EnEnsayomodequiDTO : EntityBase
    {
        public int? Enmodocodi { get; set; }
        public int? Equicodi { get; set; }
        public int Enmoeqcodi { get; set; }
    }
}
