using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_PTOXESTACION
    /// </summary>
    public partial class PmoPtoxestacionDTO : EntityBase
    {
        public int Pmehcodi { get; set; }
        public int Pmpxehcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public string Pmpxehestado { get; set; }
        public decimal Pmpxehfactor { get; set; }

        public string Pmpxehusucreacion { get; set; }
        public DateTime? Pmpxehfeccreacion { get; set; }
        public string Pmpxehusumodificacion { get; set; }
        public DateTime? Pmpxehfecmodificacion { get; set; }
    }
    public partial class PmoPtoxestacionDTO
    {
        public string Ptomedielenomb { get; set; }
        public string Ptomedidesc { get; set; }
        public string PmpxehfeccreacionDesc { get; set; }
        public string PmpxehfecmodificacionDesc { get; set; }
        public string PmpxehestadoDesc { get; set; }
    }
}
