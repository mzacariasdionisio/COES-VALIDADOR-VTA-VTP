using COES.Base.Core;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_ARCHIVOENVIO
    /// </summary>
    public partial class CbArchivoenvioDTO : EntityBase
    {
        public int Cbarchcodi { get; set; }
        public int Cbvercodi { get; set; }
        public int Ccombcodi { get; set; }
        public string Cbarchnombreenvio { get; set; }
        public string Cbarchnombrefisico { get; set; }
        public int Cbarchorden { get; set; }
        public int Cbarchestado { get; set; }

        public int? Corrcodi { get; set; }
        public int? Cbarchconfidencial { get; set; }
        public string Cbarchobs { get; set; }
    }

    public partial class CbArchivoenvioDTO
    {
        public int Archienvioorden { get; set; }
        public bool EsNuevo { get; set; }
    }
}
