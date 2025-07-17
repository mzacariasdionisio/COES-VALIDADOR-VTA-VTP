using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_RSFEQUIVALENCIA
    /// </summary>
    public class EveRsfequivalenciaDTO : EntityBase
    {
        public int Rsfequcodi { get; set; } 
        public int Equicodi { get; set; } 
        public string Rsfequagccent { get; set; } 
        public string Rsfequagcuni { get; set; } 
        public string Rsfequindicador { get; set; }
        public string Rsfequlastuser { get; set; } 
        public DateTime? Rsfequlastdate { get; set; }

        #region Cambios RSF
        public decimal? Rsfequminimo { get; set; }
        public decimal? Rsfequmaximo { get; set; }
        public int Grupocodi { get; set; }
        public string Grupotipo { get; set; }
        public string Rsfequrecurcodi { get; set; }
        public int? Rsfequasignacion { get; set; }
        public string ModosOperacion { get; set; }
        public int IndCC { get; set; }
        public int? Equipadre { get; set; }
        public decimal Diferencia { get; set; }
        public decimal Acumulado { get; set; }
        public int Famcodi { get; set; }
        public List<int> CodigosGrupos { get; set; }

        #endregion
    }
}
