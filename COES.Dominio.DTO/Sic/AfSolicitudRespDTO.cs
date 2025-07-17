using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AF_SOLICITUD_RESP
    /// </summary>
    public class AfSolicitudRespDTO : EntityBase
    {
        public int Enviocodi { get; set; }
        public string Soresparchivootros { get; set; }
        public string Soresparchivoinf { get; set; }
        public string Sorespobsarchivo { get; set; }
        public string Sorespobs { get; set; }
        public string Sorespusucreacion { get; set; }
        public DateTime? Sorespfeccreacion { get; set; }
        public string Sorespusumodificacion { get; set; }
        public DateTime? Sorespfecmodificacion { get; set; }
        public string Sorespestadosol { get; set; }
        public string Sorespdesc { get; set; }
        public DateTime? Sorespfechaevento { get; set; }
        public int Emprcodi { get; set; }
        public int Sorespcodi { get; set; }

        //Datos Adicionales
        public string Emprnomb { get; set; }
        public string Estado { get; set; }

        //Datos de Consulta
        public string Empresa { get; set; } 
        public string Di { get; set; }
        public string Df { get; set; }

        public string FechEvento { get; set; }
        public string SorespfechaeventoDesc { get; set; }

        public string SorespfeccreacionDesc { get; set; }
        public string SorespfecmodificacionDesc { get; set; }

    }
}
