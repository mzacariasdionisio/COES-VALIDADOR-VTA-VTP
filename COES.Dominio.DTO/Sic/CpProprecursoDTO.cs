using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_PROPRECURSO
    /// </summary>
    public class CpProprecursoDTO : EntityBase
    {
        public int Recurcodi { get; set; }
        public int Variaccodi { get; set; }
        public int Topcodi { get; set; }
        public int Propcodi { get; set; }
        public DateTime Fechaproprecur { get; set; }
        public string Valor { get; set; }

        #region Yupana Continuo
        public short Proporden { get; set; }
        public int Catcodi { get; set; }
        public short CambiaValor { get; set; }
        #endregion
    }
}
