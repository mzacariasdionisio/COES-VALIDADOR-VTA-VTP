using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_INGRESO_POTEFR
    /// </summary>
    [Serializable]
    public class VtpIngresoPotefrDTO : EntityBase
    {
        public int Ipefrcodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Recpotcodi { get; set; }
        public int Ipefrintervalo { get; set; } 
        public int? Ipefrdia { get; set; } 
        public string Ipefrdescripcion { get; set; } 
        public string Ipefrusucreacion { get; set; } 
        public DateTime Ipefrfeccreacion { get; set; } 
        public string Ipefrusumodificacion { get; set; } 
        public DateTime Ipefrfecmodificacion { get; set; } 
    }
}
