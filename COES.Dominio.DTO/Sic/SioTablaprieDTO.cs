using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SIO_TABLAPRIE
    /// </summary>
    public class SioTablaprieDTO : EntityBase
    {
        public int Tpriecodi { get; set; }
        public string Tpriedscripcion { get; set; }
        public DateTime? Tpriefechaplazo { get; set; }
        public int? Areacodi { get; set; }
        public string Tprieabrev { get; set; }
        public string Tprieusumodificacion { get; set; }
        public DateTime? Tpriefecmodificacion { get; set; }
        public string Tprieusucreacion { get; set; }
        public string Tpriequery { get; set; }
        public DateTime? Tprieffeccreacion { get; set; }
        public int? Tprieresolucion { get; set; }
        public DateTime? Tpriefechacierre { get; set; }

        public string Areaabrev { get; set; }
        public string Tprieusutabla { get; set; }
        //Campo para Remision
        public int CountData { get; set; }
        public int Cabpricodi { get; set; }
        public string Tpriecodtablaosig { get; set; }

        public int CantidadVersion { get; set; }

        #region SIOSEIN-PRIE-2021

        public string UsuarioUltimaVerificacionDesc { get; set; }
        public string FechaUltimaVerificacionDesc { get; set; }

        public string EstadoUltimaRemisionDesc { get; set; }
        public string UsuarioUltimaRemisionDesc { get; set; }
        public string FechaUltimaRemisionDesc { get; set; }

        public int Cabpritieneregistros { get; set; }

        public int PseinCodi { get; set; }
        public int RccaCodi { get; set; }

        public string TituloTabla { get; set; }

        #endregion
    }
}
