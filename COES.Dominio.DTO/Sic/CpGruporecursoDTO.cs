using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_GRUPORECURSO
    /// </summary>
    public class CpGruporecursoDTO : EntityBase
    {
        public int? Grurecorden { get; set; }
        public decimal? Grurecval4 { get; set; }
        public decimal? Grurecval3 { get; set; }
        public decimal? Grurecval2 { get; set; }
        public decimal? Grurecval1 { get; set; }
        public int Topcodi { get; set; }
        public int Recurcodi { get; set; }
        public int Recurcodidet { get; set; }

        public int Catcodi { get; set; }

        #region YupanaContinuo
        public int Catcodimain { get; set; }
        public int Catcodisec { get; set; }
        public int Recurconsideragams { get; set; }
        public int Recurtoescenario { get; set; }
        public int Recurcodisicoes { get; set; }
        public string Recurnombre { get; set; }
        public decimal? Plantamax { get; set; }
        public decimal? Plantamin { get; set; }
        #endregion
    }
}
