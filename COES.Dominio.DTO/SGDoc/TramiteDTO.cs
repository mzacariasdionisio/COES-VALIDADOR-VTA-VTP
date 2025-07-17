using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.SGDoc
{
    public class TramiteDTO
    {
        public int NReg { get; set; }
        public string A { get; set; }
        public string As { get; set; }
        public short T { get; set; }
        public string Atencion { get; set; }
        public string Archivo { get; set; }
        public string EmpRemitente { get; set; }
        public string AreaRemitente { get; set; }
        public string Destino { get; set; }
        public string NumDoc { get; set; }
        public string Asunto { get; set; }
        public System.DateTime FRecepcion { get; set; }
        public short Estado { get; set; }
    }
}

