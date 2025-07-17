using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DAI_APORTANTE
    /// </summary>
    public class DaiAportanteDTO : EntityBase
    {
        public int Aporcodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int Prescodi { get; set; } 
        public int Tabcdcodiestado { get; set; } 
        public decimal? Aporporcentajeparticipacion { get; set; } 
        public decimal? Apormontoparticipacion { get; set; } 
        public int? Aporaniosinaporte { get; set; } 
        public string Aporliquidado { get; set; } 
        public string Aporusuliquidacion { get; set; } 
        public DateTime? Aporfecliquidacion { get; set; } 
        public string Aporactivo { get; set; } 
        public string Aporusucreacion { get; set; } 
        public DateTime? Aprofeccreacion { get; set; } 
        public string Aporusumodificacion { get; set; } 
        public DateTime? Aporfecmodificacion { get; set; }

        public string Emprnomb { get; set; }
        public string Emprrazsocial { get; set; }
        public string Emprruc { get; set; }
        public string Tipoempresa { get; set; }
        public string Estadoaportante { get; set; }

        public string Reprocesar { get; set; }
        public string Porcentaje { get; set; }
        public int EstadoImportado { get; set; }
        public int Anio { get; set; }

        public string NroChequeEgreso { get; set; }
        public string NroReciboEgreso { get; set; }
        public string NroChequeInteres { get; set; }
        public string NroCarta { get; set; }
        public int Pago { get; set; }

        public decimal Caleinteres { get; set; }
        public decimal Caleinteresigv { get; set; }
        public decimal Totalinteresesigv { get; set; }
        public decimal Amortizacion { get; set; }
        public decimal Total { get; set; }
    }
}
