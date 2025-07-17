using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_RELACION_INSUMOS
    /// </summary>
    public class PfRelacionInsumosDTO : EntityBase
    {
        public int Pfrptcodi { get; set; } 
        public int Pfverscodi { get; set; } 
        public int Pfrinscodi { get; set; }

        //DATOS ADICIONALES
        public int Pfrecucodi { get; set; }
    }
}
