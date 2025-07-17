using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_CANALCAMBIO_SP7
    /// </summary>
    public class TrCanalcambioSp7DTO : EntityBase
    {
        public int Canalcodi { get; set; } 
        public DateTime Canalcmfeccreacion { get; set; } 
        public int? Emprcodi { get; set; }
        public string EmpresaNombre { get; set; }
        public int? Zonacodi { get; set; }
        public string ZonaNombre { get; set; }
        public string Canalnomb { get; set; } 
        public string Canaliccp { get; set; } 
        public string Canalabrev { get; set; } 
        public string Canalunidad { get; set; } 
        public string Canalpathb { get; set; } 
        public string Canalpointtype { get; set; } 
        public string Lastuser { get; set; } 
        public int? Emprcodiant { get; set; }
        public string EmpresaNombreant { get; set; }
        public int? Zonacodiant { get; set; }
        public string ZonaNombreant { get; set; }
        public string Canalnombant { get; set; } 
        public string Canaliccpant { get; set; } 
        public string Canalabrevant { get; set; } 
        public string Canalunidadant { get; set; } 
        public string Canalpathbant { get; set; } 
        public string Canalpointtypeant { get; set; } 
        public string Lastuserant { get; set; } 
    }
}
