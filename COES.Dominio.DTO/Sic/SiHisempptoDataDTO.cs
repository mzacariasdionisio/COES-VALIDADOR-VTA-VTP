using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_HISEMPPTO_DATA
    /// </summary>
    public class SiHisempptoDataDTO : EntityBase
    {
        public int Emprcodi { get; set; }
        public DateTime Hptdatfecha { get; set; }
        public string Hptdatptoestado { get; set; }
        public int Ptomedicodi { get; set; }
        public int? Ptomedicodiold { get; set; }
        public int? Ptomedicodiactual { get; set; }
        public int Hptdatcodi { get; set; }
        public string Hptdatusucreacion { get; set; }
        public DateTime Hptdatfeccreacion { get; set; }

        public string Ptomedidesc { get; set; }
        public string Emprnomb { get; set; }
    }
}