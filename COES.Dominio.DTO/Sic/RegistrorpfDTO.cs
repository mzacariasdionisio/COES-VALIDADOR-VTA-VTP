using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class RegistrorpfDTO
    {
        public int PTOMEDICODI { get; set; }
        public int TIPOINFOCODI { get; set; }
        public DateTime FECHAHORA { get; set; }
        public int SEGUNDO { get; set; }
        public decimal VALOR { get; set; }
        public decimal POTENCIA { get; set; }
        public decimal FRECUENCIA { get; set; }
        public bool CUMPLEPOTENCIA { get; set; }
        public bool CUMPLEFRECUENCIA { get; set; }
        public DateTime HORAINICIO { get; set; }
        public DateTime HORAFIN { get; set; }
        public decimal DIFERENCIA { get; set; }
        public string INDICADORPOT { get; set; }
        public decimal BALANCE { get; set; }
        public decimal NRODATOS { get; set; }
    }
}

