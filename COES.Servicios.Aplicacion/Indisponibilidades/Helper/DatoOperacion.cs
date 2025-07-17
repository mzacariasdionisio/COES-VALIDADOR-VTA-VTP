using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Servicios.Aplicacion.Indisponibilidades.Helper
{
    /// <summary>
    /// Clase dato de operación para ser usado en el módulo de Indisponibildad
    /// </summary>
    public partial class DatoOperacion : EntityBase, ICloneable
    {
        public decimal Formuladat { get; set; }
        public DateTime Fechadat { get; set; }
        public List<int> ListDias { get; set; }
        public string TipComb { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
