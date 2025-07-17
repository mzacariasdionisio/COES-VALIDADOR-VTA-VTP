using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_FORZADO_DET
    /// </summary>
    public partial class CpForzadoDetDTO : EntityBase, ICloneable
    {
        public int Cpfzdtcodi { get; set; }
        public int Cpfzcodi { get; set; }
        public int Grupocodi { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public int? Cpfzdtperiodoini { get; set; }
        public int? Cpfzdtperiodofin { get; set; }
        public string Cpfzdtflagcreacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class CpForzadoDetDTO
    {
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }
        public int Generadorcodi { get; set; }
        public string Generadornomb { get; set; }
        public bool ExisteYupana { get; set; }
        public bool EsFicticio { get; set; }
    }
}
