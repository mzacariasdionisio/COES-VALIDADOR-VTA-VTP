using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_CIRCULAR_SP7
    /// </summary>
    public class TrCircularSp7DTO : EntityBase
    {
        public int Canalcodi { get; set; } 
        public DateTime Canalfhsist { get; set; } 
        public decimal? Canalvalor { get; set; } 
        public int? Canalcalidad { get; set; }

        private string canalcalidadDescripcion;

        public string GetCanalcalidadDescripcion()
        {
            return canalcalidadDescripcion;
        }

        public void SetCanalcalidadDescripcion(string value)
        {
            if(value == null)
            {
                value = string.Empty;
            }

            canalcalidadDescripcion = value;
        }

        public DateTime Canalfechahora { get; set; }
        public string Canalnomb { get; set; }
        public string Calnomb { get; set; }
    }
}
