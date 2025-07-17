using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_GENERACION_EMS
    /// </summary>
    public class CmGeneracionEmsDTO : EntityBase
    {
        public int Genemscodi { get; set; } 
        public int Cmgncorrelativo { get; set; } 
        public int Equicodi { get; set; } 
        public decimal? Genemsgeneracion { get; set; } 
        public int Genemsoperativo { get; set; } 
        public DateTime Genemsfecha { get; set; } 
        public string Genemsusucreacion { get; set; } 
        public DateTime? Genemsfechacreacion { get; set; }
        public int? Grupocodi { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public string Emprnomb { get; set; }
        public bool IndModoOperacion { get; set; }
        public bool IndTv { get; set; }
        public int IdCicloComb { get; set; }
        public decimal Genemspotmax { get; set; }
        public decimal Genemspotmin { get; set; }

        #region Horas Operacion EMS
        public int Equipadre { get; set; }
        public int Famcodipadre { get; set; }
        public string Central { get; set; }
        public string FechaIniDesc { get; set; }
        public string FechaFinDesc { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public EveHoraoperacionDTO HoraOperacion { get; set; }
        #endregion

        #region Titularidad-Instalaciones-Empresas

        public int Emprcodi { get; set; }

        #endregion

        public string Genemstipoestimador { get; set; }
    }
}
