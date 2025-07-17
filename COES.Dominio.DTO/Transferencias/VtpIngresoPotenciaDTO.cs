using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_INGRESO_POTENCIA
    /// </summary>
    public class VtpIngresoPotenciaDTO : EntityBase
    {
        public int Potipcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int? Emprcodi { get; set; }
        public decimal? Potipimporte { get; set; }
        public decimal? Potipporcentaje { get; set; }
        public string Potipusucreacion { get; set; }
        public DateTime Potipfeccreacion { get; set; }
        //Atributos adicionales para usarlos en consultas
        public string Emprnomb { get; set; }
        //ASSETEC 20190627: muestra si la empresa tiene un saldo asignado de otro periodo
        public decimal? Potipsaldoanterior { get; set; }
    }
}
