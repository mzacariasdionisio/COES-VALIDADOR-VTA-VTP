using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_CARGO
    /// </summary>
    public class VtpPeajeCargoDTO : EntityBase
    {
        public int Pecarcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Pingcodi { get; set; }
        public string Pecartransmision { get; set; }
        public decimal Pecarpeajecalculado { get; set; }
        public decimal Pecarpeajedeclarado { get; set; }
        public decimal Pecarpeajerecaudado { get; set; }
        public decimal Pecarsaldoanterior { get; set; }
        public decimal Pecarajuste { get; set; }
        public decimal Pecarsaldo { get; set; }
        public int Pecarpericodidest { get; set; }
        public string Pecarusucreacion { get; set; }
        public DateTime Pecarfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
        public string Pingnombre { get; set; }

    }
}
