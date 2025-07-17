using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_REPORTE
    /// </summary>
    public class InReporteDTO : EntityBase
    {
        public int Inrepcodi { get; set; }
        public string Inrepnombre { get; set; }
        public int Inrephorizonte { get; set; }
        public int Inreptipo { get; set; }
        public string Inrepusucreacion { get; set; }
        public DateTime Inrepfeccreacion { get; set; }
        public string Inrepusumodificacion { get; set; }
        public DateTime Inrepfecmodificacion { get; set; }
        public int? Progrcodi { get; set; }
        public List<InSeccionDTO> ListaSecciones { get; set; }
        public List<InReporteVariableDTO> ListaVariables { get; set; }
        public bool IndicadorModificado { get; set; }
    }
}
