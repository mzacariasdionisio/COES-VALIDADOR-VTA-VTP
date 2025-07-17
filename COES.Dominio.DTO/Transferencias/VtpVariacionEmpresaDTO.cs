using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_VARIACION_EMPRESA
    /// </summary>
    [Serializable]
    public class VtpVariacionEmpresaDTO: EntityBase
    {
        public int Varempcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal Varempprocentaje { get; set; }
        public string Varemptipocomp { get; set; }
        public DateTime Varempvigencia { get; set; }
        public string Varempestado { get; set; }
        public string Varempusucreacion { get; set; }
        public DateTime Varempfeccreacion { get; set; }
        public string Varempusumodificacion { get; set; }
        public DateTime Varempfecmodificacion { get; set; }


        /// Para listas y filtros
        
        public int Siemprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int fila { get; set; }
    }
}
