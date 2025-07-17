using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_INDISPONIBILIDAD_TEMP_CAB
    /// </summary>
    public partial class SmaIndisponibilidadTempCabDTO : EntityBase
    {
        public int Intcabcodi { get; set; } 
        public DateTime? Intcabfecha { get; set; } 
        public string Intcabusucreacion { get; set; } 
        public DateTime? Intcabfeccreacion { get; set; } 
        public string Intcabusumodificacion { get; set; } 
        public DateTime? Intcabfecmodificacion { get; set; } 
    }

    public partial class SmaIndisponibilidadTempCabDTO
    {
        public string IntcabfechaDesc { get; set; }
        public string IntcabfeccreacionDesc { get; set; }
        public string IntcabfecmodificacionDesc { get; set; }
        public  List<SmaIndisponibilidadTempDetDTO> ListaDetalles { get; set; }
    }
}
