using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_EMPRESA_PAGO
    /// </summary>
    public partial class VtpEmpresaPagoDTO : EntityBase
    {
        public int Potepcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Potsecodi { get; set; }
        public int Emprcodipago { get; set; }
        public int Emprcodicobro { get; set; }
        public decimal Potepmonto { get; set; }
        public string Potepusucreacion { get; set; }
        public DateTime Potepfeccreacion { get; set; }
    }

    /// <summary>
    /// Clase parcial que mapea atributos adicionales
    /// </summary>
    public partial class VtpEmpresaPagoDTO
    {
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnombpago { get; set; }
        public string Emprnombcobro { get; set; }

        public string Recpotnombre { get; set; }
        public string Perinombre { get; set; }
        public int Perianio { get; set; }
        public int Perimes { get; set; }
        public int Perianiomes { get; set; }
        public string Recanombre { get; set; }
        public List<decimal> lstImportesPromd { get; set; }
        public decimal PorcentajeVariacion { get; set; }

        #region SIOSEIN
        public bool EsNuevoRegistro { get; set; }
        public string Emprcodosinergmincobro { get; set; }
        public string Emprcodosinergminpago { get; set; }
        #endregion

        public string EmprRuc { get; set; }
    }
}
