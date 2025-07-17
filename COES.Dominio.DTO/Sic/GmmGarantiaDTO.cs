using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla GMM_GARANTIA
    /// </summary>
    public partial class GmmGarantiaDTO : EntityBase
    {

        public int GARACODI { get; set; }
        public DateTime? GARAFECINICIO { get; set; }
        public DateTime? GARAFECFIN { get; set; }
        public decimal GARAMONTOGARANTIA { get; set; }
        public string GARAARCHIVO { get; set; }
        public string GARAESTADO { get; set; }
        public int EMPGCODI { get; set; }
        public int EMPRCODI { get; set; }
        public string TCERCODI { get; set; }
        public string TMODCODI { get; set; }
        public string GARAUSUCREACION { get; set; }
        public DateTime? GARAFECCREACION { get; set; }
        public string GARAUSUMODIFICACION { get; set; }
        public DateTime? GARAFECMODIFICACION { get; set; }
    }

    public partial class GmmGarantiaDTO : EntityBase
    {
        // Tabla de procesamiento
        public int Pericodi { get; set; }
        public string OrdenMensaje { get; set; }
        public string Mensaje { get; set; }
        public string Mensaje1 { get; set; }
        public string Mensaje2 { get; set; }
        public string Mensaje3 { get; set; }

        public decimal TCAMBIO { get; set; }
        public decimal SSCC { get; set; }
        public decimal MRESERVA { get; set; }
        public decimal TINFLEX { get; set; }
        public decimal TEXCESO { get; set; }

        // Previsualización de resultados
        public string EMPRESA { get; set; }
        public string RENERGIA { get; set; }
        public string RCAPACIDAD { get; set; }
        public string RPEAJE { get; set; }
        public string RSCOMPLE { get; set; }
        public string RINFLEXOP { get; set; }
        public string REREACTIVA { get; set; }
        public string TOTALGARANTIA { get; set; }

        public string DATCALESTADO { get; set; }

    }

}
