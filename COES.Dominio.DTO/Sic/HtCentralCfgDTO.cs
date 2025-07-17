using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla HT_CENTRAL_CFG
    /// </summary>
    public partial class HtCentralCfgDTO : EntityBase
    {
        public int Equicodi { get; set; } 
        public int Htcentcodi { get; set; } 
        public string Htcentfuente { get; set; } 
        public DateTime? Htcentfecregistro { get; set; } 
        public string Htcentusuregistro { get; set; } 
        public DateTime? Htcentfecmodificacion { get; set; } 
        public string Htcentusumodificacion { get; set; } 
        public int? Htcentactivo { get; set; } 
    }
    public partial class HtCentralCfgDTO
    {
        public int Famcodi { get; set; }
        public string Famnomb { get; set; }
        public string Equinomb { get; set; }
        public string FuenteDesc { get; set; }
        public string NombreElemento { get; set; }
        public string FactorDesc { get; set; }

        public string HtcentfecregistroDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        //public string HtcentactivoDesc{ get; set; }
    }
}
