using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_INGRESO_TARIFARIO_AJUSTE
    /// </summary>
    public class VtpIngresoTarifarioAjusteDTO : EntityBase
    {
        public int Ingtajcodi { get; set; }
        public int Pericodi { get; set; }
        public int Emprcodiping { get; set; }
        public int Pingcodi { get; set; }
        public int Emprcodingpot { get; set; }
        public decimal Ingtajajuste { get; set; }
        public string Ingtajusucreacion { get; set; }
        public DateTime Ingtajfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnombping { get; set; }
        public string Emprnombingpot { get; set; }
        public string Pingnombre { get; set; }
    }
}
