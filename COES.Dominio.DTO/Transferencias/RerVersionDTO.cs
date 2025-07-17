using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea a un objeto VERSION
    /// </summary>
    public class RerVersionDTO : EntityBase
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Nombre { get; set; }
    }
}