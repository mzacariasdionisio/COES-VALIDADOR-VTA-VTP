using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_RELACION_INDISPONIBILIDADES
    /// </summary>
    public partial class PfRelacionIndisponibilidadesDTO : EntityBase
    {
        public int Pfrindcodi { get; set; }
        public int Pfrptcodi { get; set; }
        public int Irptcodi { get; set; }
    }
    public partial class PfRelacionIndisponibilidadesDTO
    {
        public int Icuacodi { get; set; }
    }
}
