using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_HISEMPPTO
    /// </summary>
    public class SiHisempptoDTO : EntityBase
    {
        public int Hempptcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public DateTime Hempptfecha { get; set; }
        public int Migracodi { get; set; }
        public int Ptomedicodiold { get; set; }
        public string Hempptestado { get; set; }
        public int Hempptdeleted { get; set; }

        public bool EstadoRecorrido { get; set; }
        public int Ptomedicodiactual { get; set; }
        public string Ptomedidesc { get; set; }
    }
}