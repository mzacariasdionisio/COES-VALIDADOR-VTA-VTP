using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_FAMILIA
    /// </summary>
    public partial class EqFamiliaDTO : EntityBase
    {
        public int Famcodi { get; set; }
        public string Famabrev { get; set; }
        public int? Tipoecodi { get; set; }
        public int? Tareacodi { get; set; }
        public string Famnomb { get; set; }
        public int? Famnumconec { get; set; }
        public string Famnombgraf { get; set; }
        public string Famestado { get; set; }

        //Datos de auditoría
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public DateTime? FechaUpdate { get; set; }
    }

    public partial class EqFamiliaDTO
    {
        //Datos de Presentación
        public string EstadoDescripcion { get; set; }

        public int TotalXFamcodi { get; set; }
        public string Tareaabrev { get; set; }

        public decimal? TotalEnergia { get; set; }
    }
}
