using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_RECURSO
    /// </summary>
    public class PfRecursoDTO : EntityBase
    {
        public int Pfrecucodi { get; set; } 
        public string Pfrecunomb { get; set; } 
        public string Pfrecudescripcion { get; set; } 
        public int? Pfrecutipo { get; set; } 
    }
}
