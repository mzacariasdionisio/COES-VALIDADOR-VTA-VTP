using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VtdMontoPoExceso
    /// 
    /// </summary>
    public class VtdMontoPorExcesoDTO 
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public decimal Valofrectotal { get; set; }
        public decimal Valootrosequipos { get; set; }
        public decimal Valocostofuerabanda { get; set; }
        public decimal Valocomptermrt { get; set; }
        public decimal Valdcargoconsumo { get; set; }
        public decimal Valdaportesadicional { get; set; }
        public DateTime Valofecha { get; set; }

    }
    
}
