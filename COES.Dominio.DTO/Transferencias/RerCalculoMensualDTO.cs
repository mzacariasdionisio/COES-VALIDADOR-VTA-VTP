using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_CALCULO_MENSUAL
    /// </summary>
    public class RerCalculoMensualDTO : EntityBase
    {
        public int Rercmcodi { get; set; }
        public int Rerpprcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public string Rercmfatipintervalo { get; set; }
        public DateTime? Rercmfafecintervalo { get; set; }
        public decimal? Rercmfavalintervalo { get; set; }
        public decimal Rercmtaradj { get; set; }
        public decimal Rercmsummulinfa { get; set; }
        public decimal Rercminggarantizado { get; set; }
        public decimal Rercminsingpotencia { get; set; }
        public decimal Rercmsumfadivn { get; set; }
        public decimal Rercmingpotencia { get; set; }
        public decimal Rercmingprimarer { get; set; }
        public decimal Rercmingenergia { get; set; }
        public decimal Rercmsaldovtea { get; set; }
        public decimal Rercmsaldovtp { get; set; }
        public decimal Rercmtipocambio { get; set; }
        public decimal Rercmimcp { get; set; }
        public decimal Rercmsalmencompensar { get; set; }
        public string Rercmusucreacion { get; set; }
        public DateTime Rercmfeccreacion { get; set; }

        //Aditional
        public int Reravcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public int Rerpprmes { get; set; }
    }
}

