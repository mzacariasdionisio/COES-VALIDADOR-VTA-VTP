using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_EMPRESA
    /// </summary>
    [Serializable]
    public class VtpPeajeEmpresaPagoDTO : EntityBase
    {
        public int Pempagcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Emprcodipeaje { get; set; }
        public int Pingcodi { get; set; }
        public int Emprcodicargo { get; set; }
        public string Pempagtransmision { get; set; }
        public decimal Pempagpeajepago { get; set; }
        public decimal Pempagsaldoanterior { get; set; }
        public decimal Pempagajuste { get; set; }
        public decimal Pempagsaldo { get; set; }
        public int Pempagpericodidest { get; set; }
        public string Pempagusucreacion { get; set; }
        public DateTime Pempagfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnombpeaje { get; set; }
        public string Emprnombcargo { get; set; }
        public string Pingnombre { get; set; }
        public string Pingtipo { get; set; }

        public string Emprruc { get; set; }

        #region SIOSEIN
        public decimal PeajeTotal { get; set; }
        public bool EsNuevoRegistro { get; set; }
        public string Emprcodosinergminpeaje { get; set; }
        public string Emprcodosinergmincargo { get; set; }
        #endregion

        #region siosein2
        public List<VtpPeajeEmpresaPagoDTO> ListaObj { get; set; }
        #endregion
        public string Recpotnombre { get; set; }
        public string Perinombre { get; set; }
        public int Perianio { get; set; }
        public int Perimes { get; set; }
        public int Perianiomes { get; set; }
        public string Recanombre { get; set; }
        public List<decimal> lstImportesPromd { get; set; }
        public decimal PorcentajeVariacion { get; set; }
    }
}
