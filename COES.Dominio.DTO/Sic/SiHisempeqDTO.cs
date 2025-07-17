using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_HISEMPEQ
    /// </summary>
    public class SiHisempeqDTO : EntityBase
    {
        public int Hempeqcodi { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public DateTime Hempeqfecha { get; set; }
        public int Migracodi { get; set; }
        public int Equicodiold { get; set; }
        public string Hempeqestado { get; set; }
        public int Hempeqdeleted { get; set; }

        public bool EstadoRecorrido { get; set; }
        public int Equicodiactual { get; set; }
        public string Equinomb { get; set; }
        public int Operadoremprcodi { get; set; }
    }
}
