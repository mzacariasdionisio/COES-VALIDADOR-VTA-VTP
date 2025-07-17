using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ABI_POTEFEC
    /// </summary>
    public class ActualizacionCVDTO : EntityBase
    {
        public DateTime Fecha { get; set; }
        public String Tipo { get; set; }
        public String Nombre { get; set; }
        public String Detalle { get; set; }
        public Int32 Codigo { get; set; }
    }
}
