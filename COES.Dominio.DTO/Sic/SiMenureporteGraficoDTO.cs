using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MENUREPORTE_GRAFICO
    /// </summary>
    public class SiMenureporteGraficoDTO : EntityBase
    {
        public int Mrepcodi { get; set; } 
        public int Mrgrcodi { get; set; } 
        public int? Mrgrestado { get; set; } 
        public int Reporcodi { get; set; } 
    }
}
