using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMPO_OBRA
    /// </summary>
    public partial class PmpoConfiguracionDTO : EntityBase
    {
        public int Confpmcodi { get; set; }
        public string Confpmatributo { get; set; }
        public string Confpmparametro { get; set; }
        public string Confpmvalor { get; set; }
        public string Confpmestado { get; set; }
        public string Confpmusucreacion { get; set; }
        public DateTime? Confpmfeccreacion { get; set; }
        public string Confpmusumodificacion { get; set; }
        public DateTime? Confpmfecmodificacion { get; set; }
    }

    public partial class PmpoConfiguracionDTO
    {
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string ParametroDesc { get; set; }
        public string TipoParametroDesc { get; set; }
        public int Formatcodi { get; set; }
    }
}
