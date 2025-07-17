using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea a un objeto AnioTarifario
    /// </summary>
    public class RerAnioTarifarioDTO : EntityBase
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public string NomAnio { get; set; }

    }
}