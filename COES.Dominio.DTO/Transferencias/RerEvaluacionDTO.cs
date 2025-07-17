using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_EVALUACION
    /// </summary>
    public class RerEvaluacionDTO : EntityBase
    {
        public int Rerevacodi { get; set; }
        public int Rerrevcodi { get; set; }
        public int Rerevanumversion { get; set; }
        public string Rerevaestado { get; set; }
        public string Rerevausucreacion { get; set; }
        public DateTime Rerevafeccreacion { get; set; }
        public string Rerevausumodificacion { get; set; }
        public DateTime Rerevafecmodificacion { get; set; }

        //Additional
        public string Iperinombre { get; set; }
        public int Iperianio { get; set; }
        public int Iperimes { get; set; }
        public string Rerrevnombre { get; set; }
        public string RerevaestadoDesc { get; set; }
        public String RerevafeccreacionDesc { get; set; }
        public String RerevafecmodificacionDesc { get; set; }

    }
}