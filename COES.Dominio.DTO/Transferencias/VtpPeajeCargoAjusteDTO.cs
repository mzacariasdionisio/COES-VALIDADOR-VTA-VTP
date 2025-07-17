using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_CARGO_AJUSTE
    /// </summary>
    public class VtpPeajeCargoAjusteDTO : EntityBase
    {
        public int Pecajcodi { get; set; }
        public int Pericodi { get; set; }
        public int Emprcodi { get; set; }
        public int Pingcodi { get; set; }
        public decimal Pecajajuste { get; set; }
        public string Pecajusucreacion { get; set; }
        public DateTime Pecajfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
        public string Pingnombre { get; set; }
    }
}
