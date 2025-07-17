using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class IioSicliOsigFacturaDTO
    {
        public int Clofaccodi { get; set; }
        public string Clofacanhiomes { get; set; }
        public string Clofaccodempresa { get; set; }
        public string Clofacnomempresa { get; set; }
        public string Clofacruc { get; set; }
        public string Clofaccodcliente { get; set; }
        public string Clofacnomcliente { get; set; }
        public string Clofaccodbarrasumin { get; set; }
        public string Clofacnombarrasumin { get; set; }
        public decimal Clofactensionentrega { get; set; }
        public string Clofaccodbrg { get; set; }
        public string Clofacnombrg { get; set; }
        public decimal Clofactensionbrg { get; set; }
        public decimal Clofacphpbe { get; set; }
        public decimal Clofacpfpbe { get; set; }
        public decimal Clofacehpbe { get; set; }
        public decimal Clofacefpbe { get; set; }
        //public DateTime? Cuadr3fecmodificacion { get; set; }

        public string Clofacusucreacion { get; set; }
        public DateTime Clofacfeccreacion { get; set; }

        public string Clofacusumodificacion { get; set; }
        public DateTime Clofacfecmodificacion { get; set; }
    }
}
