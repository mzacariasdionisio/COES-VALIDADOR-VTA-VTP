using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_INGRESO_POTEFR_DETALLE
    /// </summary>
    [Serializable]
    public class VtpIngresoPotefrDetalleDTO : EntityBase
    {
        public int Ipefrdcodi { get; set; } 
        public int Ipefrcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public int? Cenequicodi { get; set; }
        public int? Uniequicodi { get; set; } 
        public decimal? Ipefrdpoteefectiva { get; set; } 
        public decimal? Ipefrdpotefirme { get; set; } 
        public decimal? Ipefrdpotefirmeremunerable { get; set; } 
        public string Ipefrdusucreacion { get; set; } 
        public DateTime Ipefrdfeccreacion { get; set; } 
        public string Ipefrdusumodificacion { get; set; } 
        public DateTime Ipefrdfecmodificacion { get; set; }
        public int? Unigrupocodi { get; set; }
        public string Ipefrdunidadnomb { get; set; }
        public int? Ipefrdficticio { get; set; }

        public System.String Emprnomb { get; set; }
        public System.String Cenequinomb { get; set; }
        public System.String Uniequinomb { get; set; }
        //SIOSEIN-PRIE-2021
        public System.String Osinergcodi { get; set; }
        public int Famcodi { get; set; }
    }
}
