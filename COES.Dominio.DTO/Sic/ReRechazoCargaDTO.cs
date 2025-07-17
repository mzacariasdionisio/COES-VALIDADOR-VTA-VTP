using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_RECHAZO_CARGA
    /// </summary>
    public class ReRechazoCargaDTO : EntityBase
    {
        public int Rerccodi { get; set; }
        public int? Repercodi { get; set; }
        public int? Rercpadre { get; set; }
        public string Rercfinal { get; set; }
        public int? Emprcodi { get; set; }
        public string Rercestado { get; set; }
        public string Rercmotivoanulacion { get; set; }
        public string Rercusueliminacion { get; set; }
        public DateTime? Rercfecanulacion { get; set; }
        public int? Rerccorrelativo { get; set; }
        public string Rerctipcliente { get; set; }
        public int? Rerccliente { get; set; }
        public int? Repentcodi { get; set; }
        public string Rercptoentrega { get; set; }
        public string Rercalimentadorsed { get; set; }
        public decimal? Rercenst { get; set; }
        public int? Reevecodi { get; set; }
        public string Rerccomentario { get; set; }
        public DateTime? Rerctejecinicio { get; set; }
        public DateTime? Rerctejecfin { get; set; }
        public decimal? Rercpk { get; set; }
        public string Rerccompensable { get; set; }
        public decimal? Rercens { get; set; }
        public decimal? Rercresarcimiento { get; set; }
        public string Rercusucreacion { get; set; }
        public DateTime? Rercfeccreacion { get; set; }

        public string Emprnomb { get; set; }
        public string Ptoentrega { get; set; }

        public string Suministrador { get; set; }
        public string Cliente { get; set; }
        public string Evento { get; set; }

        public decimal Horasdiferencia { get; set; }
        public decimal Enscalculada { get; set; }
        public decimal Resarcimientocalculado { get; set; }
        public string Diferenciacalculo { get; set; }
        public string Indicadorenergia { get; set; }

        public double  ValorPkModificado { get; set; }
        public int CampoOrden { get; set; }

        public string Rercdisposicion1 { get; set; }
        public string Rercdisposicion2 { get; set; }
        public string Rercdisposicion3 { get; set; }
        public string Rercdisposicion4 { get; set; }
        public string Rercdisposicion5 { get; set; }
        public decimal? Rercporcentaje1 { get; set; }
        public decimal? Rercporcentaje2 { get; set; }
        public decimal? Rercporcentaje3 { get; set; }
        public decimal? Rercporcentaje4 { get; set; }
        public decimal? Rercporcentaje5 { get; set; }
        public string Rercresponsable1 { get; set; }
        public string Rercresponsable2 { get; set; }
        public string Rercresponsable3 { get; set; }
        public string Rercresponsable4 { get; set; }
        public string Rercresponsable5 { get; set; }

        public int OrdenRegistro { get; set; }
    }
}
