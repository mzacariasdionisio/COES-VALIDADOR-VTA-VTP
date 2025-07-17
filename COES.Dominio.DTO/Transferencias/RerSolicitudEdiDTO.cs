using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_SOLICITUDEDI
    /// </summary>
    public class RerSolicitudEdiDTO : EntityBase
    {
        public int Rersedcodi { get; set; }
        public int Rercencodi { get; set; }
        public int Emprcodi { get; set; }
        public int Ipericodi { get; set; }
        public int Reroricodi { get; set; }
        public DateTime Rersedfechahorainicio { get; set; }
        public DateTime Rersedfechahorafin { get; set; }
        public string Rerseddesc { get; set; }
        public decimal Rersedtotenergia { get; set; }
        public string Rersedsustento { get; set; }
        public string Rersedestadodeenvio { get; set; }
        public string Rersedeliminado { get; set; }
        public string Rersedusucreacion { get; set; }
        public DateTime? Rersedfeccreacion { get; set; }
        public string Rersedusumodificacion { get; set; }
        public DateTime? Rersedfecmodificacion { get; set; }

        //Additional
        public string Emprnomb { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string Reroridesc { get; set; }

    }

}