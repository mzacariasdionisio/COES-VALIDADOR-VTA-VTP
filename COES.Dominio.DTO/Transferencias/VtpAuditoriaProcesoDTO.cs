using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_VARIACION_EMPRESA
    /// </summary>
    [Serializable]
    public class VtpAuditoriaProcesoDTO: EntityBase
    {
        

        public int Audprocodi { get; set; }
        public int Tipprocodi { get; set; }
        public int Estdcodi { get; set; }

        public string Tipprodescripcion { get; set; }
        public string Estddescripcion { get; set; }
        
        public string Audproproceso { get; set; }
        public string Audprodescripcion { get; set; }
        public string Audprousucreacion { get; set; }
        public DateTime Audprofeccreacion { get; set; }
        public string Audprousumodificacion { get; set; }
        public DateTime Audprofecmodificacion { get; set; }

        /// Para listas y filtros
        
        public int Siemprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int fila { get; set; }

        public int numeroRegistro { get; set; }
    }
}
