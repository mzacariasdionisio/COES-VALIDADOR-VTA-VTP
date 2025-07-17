using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_SALDO_EMPRESA
    /// </summary>
    [Serializable]
    public class VtpSaldoEmpresaDTO : EntityBase
    {
        public int Potsecodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal Potseingreso { get; set; }
        public decimal Potseegreso { get; set; }
        public decimal Potsesaldoanterior { get; set; }
        public decimal Potsesaldo { get; set; }
        public decimal Potseajuste { get; set; }
        public decimal Potsesaldoreca { get; set; }
        public int Potsepericodidest { get; set; }
        public string Potseusucreacion { get; set; }
        public DateTime Potsefeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
        public string Perinombre { get; set; }
        public decimal Potsetotalsaldopositivo { get; set; }
        public decimal Potsetotalsaldonegativo { get; set; }

    }
}
