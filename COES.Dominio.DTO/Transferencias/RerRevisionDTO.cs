using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_REVISION
    /// </summary>
    public class RerRevisionDTO : EntityBase
    {
        public int Rerrevcodi { get; set; }
        public int Ipericodi { get; set; }
        public string Rerrevnombre { get; set; }
        public string Rerrevtipo { get; set; }
        //En BD es Not Null, pero se coloca que acepte nulos para la consulta de ListarPeriodosConUltima<Revision
        public DateTime? Rerrevfecha { get; set; }
        public string Rerrevestado { get; set; }
        public string Rerrevusucreacion { get; set; }
        public DateTime Rerrevfeccreacion { get; set; }
        public string Rerrevusumodificacion { get; set; }
        public DateTime Rerrevfecmodificacion { get; set; }

        //Additional
        public string Iperinombre { get; set; }
        public int Iperianio { get; set; }
        public int Iperimes { get; set; }

        public string RerrevtipoDesc { get; set; }
        public string RerrevestadoDesc { get; set; }
        public string RerrevfechaDesc { get; set; }
        public DateTime? RerrevfechaentregaEDI { get; set; }
        public string RerrevfechaentregaEDIDesc { get; set; }
        public string RerrevfeccreacionDesc { get; set; }
        public string RerrevfecmodificacionDesc { get; set; }

    }
}