using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_INGRESO_TARIFARIO
    /// </summary>
    [Serializable]
    public class VtpIngresoTarifarioDTO : EntityBase
    {
        public int Ingtarcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Pingcodi { get; set; }
        public int Emprcodiping { get; set; }
        public decimal Ingtartarimensual { get; set; }
        public int Emprcodingpot { get; set; }
        public decimal Ingtarporcentaje { get; set; }
        public decimal Ingtarimporte { get; set; }
        public decimal Ingtarsaldoanterior { get; set; }
        public decimal Ingtarajuste { get; set; }
        public decimal Ingtarsaldo { get; set; }
        public int Ingtarpericodidest { get; set; }
        public string Ingtarusucreacion { get; set; }
        public DateTime Ingtarfeccreacion { get; set; }
        //ATRIBUTOS ADICIONALES EMPLEADOS EN EL RESULTADO DE LAS CONSULTAS
        public string Emprnombingpot { get; set; }
        public string Emprnombping { get; set; }
        public string Emprruc { get; set; }

        #region Siosein2
        public string Pingnombre { get; set; }
        public string Pingtipo { get; set; }

        #endregion
        #region SIOSEIN
        public decimal ImporteTotal { get; set; }
        public bool EsNuevoRegistro { get; set; }
        public string Emprcodosinergminingpot { get; set; }
        public string Emprcodosinergminping { get; set; }
        #endregion
    }
}
