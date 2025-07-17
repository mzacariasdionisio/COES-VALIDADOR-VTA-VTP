using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Servicios.Aplicacion.Indisponibilidades.Helper
{
    /// <summary>
    /// Clase modo de operación para ser usado en el módulo de Indisponibildad
    /// </summary>
    public partial class ModoOperacion : EntityBase, ICloneable
    {
        public decimal Pe { get; set; }
        public decimal Combpe { get; set; }
        public string TipComb { get; set; }
        public List<int> ListDias { get; set; }
        public string RngDias { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
